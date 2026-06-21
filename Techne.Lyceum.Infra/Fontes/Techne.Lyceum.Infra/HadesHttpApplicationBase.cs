using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Resources;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Techne.Data;

namespace Techne
{
    public abstract class HadesHttpApplicationBase : TechneHttpApplication
    {
        private static readonly string[] httpHandlersPermitidos = new[] { ".techne.ashx", ".axd", ".png", "webservice.htc", ".asmx" };

        private static readonly object sync = new object();

        private volatile static IDictionary cache = new Hashtable();

        private static bool forceRefresh;

        private static ResourceManager pvRM;

        private static bool ForceLegacyPasswords
        {
            get
            {
                return ConfigurationSettings.AppSettings["forceLegacyPasswords"] == "True";
            }
        }

        public static void ForceRefresh()
        {
            forceRefresh = true;
        }

        public static string cs(byte[] bInputStr)
        {
            const string CryptKey = "TECHNELYCEUM";

            var enc = Encoding.GetEncoding(1252);
            var bCryptKey = enc.GetBytes(CryptKey);

            var bWSenha = new byte[bInputStr.Length];

            var j = 0;
            byte woffset = 0;
            for (var i = 0; i < bInputStr.Length; i++)
            {
                var cod = (byte)((bInputStr[i] - woffset - bCryptKey[j]) & 255);
                bWSenha[i] = cod;
                woffset = (byte)((woffset + bCryptKey[j]) & 255);
                if (++j >= CryptKey.Length)
                {
                    j = 0;
                }
            }

            return enc.GetString(bWSenha).Trim();
        }

        public static string cs(string p)
        {
            var enc = Encoding.GetEncoding(1252);
            return cs(enc.GetBytes(p));
        }

        public override bool ChangePassword(string user, string oldpassword, string newpassword, out string message)
        {
            var cn1 = ConnectionList.CreateConnection("Hades");

            var isOracle = cn1.Rdbms == Rdbms.Oracle;
            if (!isOracle && cn1.Rdbms != Rdbms.SQLServer)
            {
                throw new NotImplementedException();
            }

            var colunaSenha = isOracle || !ForceLegacyPasswords ? "senha" : "CAST(senha AS VARBINARY) senha";

            var rowUsuario = SimpleRow.QueryFirstRow(
                cn1,
                "SELECT usuario, privilegiado, " + colunaSenha + ", habilitado, alterar_senha, senhas_anteriores " +
                "FROM hd_usuario (NOLOCK) " +
                "WHERE LOWER(usuario) = ?",
                user.ToLower()
                );

            // Checa se existe usuário
            if (rowUsuario == null)
            {
                message = GetMessage(TechneAuthenticationResult.InvalidUserOrPassword);
                return false;
            }

            bool senhaValida;
            if (isOracle || !ForceLegacyPasswords)
            {
                var senha = !rowUsuario["senha"].IsNull ? (string)rowUsuario["senha"] : string.Empty;
                senhaValida = ValidaSenha(oldpassword, senha);
            }
            else
            {
                var senha = rowUsuario["senha"].IsNull ? new byte[0] : (byte[])rowUsuario["senha"];
                senhaValida = ValidaSenha(oldpassword, senha);
            }

            // Checa se a senha confere
            if (!senhaValida)
            {
                message = GetMessage(TechneAuthenticationResult.InvalidUserOrPassword);
                return false;
            }

            // Checa se o usuário está habilitado
            if (!((VarChar)rowUsuario["habilitado"]).Equals("S", true))
            {
                message = GetMessage(TechneAuthenticationResult.UserDisabled);
                return false;
            }

            const decimal memoria_maxima = 50;
            decimal memoria_senhas = 3;
            string senhas_anteriores = null;

            // Checa se a nova senha é igual a uma das anteriores
            var newpasswordc = HdPal(newpassword);
            var hadMemoriasenhas = HadesParameter.Get("Hades", "SEGURANCA", "MEMORIASENHAS");
            if (hadMemoriasenhas is decimal)
            {
                memoria_senhas = (decimal)hadMemoriasenhas;
            }

            memoria_senhas = memoria_senhas < 0 ? 0 : (memoria_senhas > memoria_maxima ? memoria_maxima : memoria_senhas);
            if (memoria_senhas > 0)
            {
                if (oldpassword == newpassword)
                {
                    message = string.Format(GetMessage("ChangePassword.MustBeDifferent"), memoria_senhas);
                    return false;
                }

                senhas_anteriores = rowUsuario["senhas_anteriores"].ToString();

                // TODO Isto dá erro de divisão por zero se a nova senha for vazia
                for (var i = 0; i < (int)Math.Ceiling((double)(senhas_anteriores.Length / newpasswordc.Length)); i++)
                {
                    if (newpasswordc == senhas_anteriores.Substring(i * newpasswordc.Length, newpasswordc.Length))
                    {
                        message = string.Format(GetMessage("ChangePassword.MustBeDifferent"), memoria_senhas);
                        return false;
                    }
                }
            }

            // Checa tamanho mínimo da senha
            decimal tamanho_minimo = 5;
            var hadTamanhominsenha = HadesParameter.Get("Hades", "SEGURANCA", "TAMANHOMINSENHA");
            if (hadTamanhominsenha is decimal)
            {
                tamanho_minimo = (decimal)hadTamanhominsenha;
            }

            if (newpassword.Length < tamanho_minimo)
            {
                message = string.Format(GetMessage("ChangePassword.MinSize"), tamanho_minimo);
                return false;
            }

            if (newpassword.Length > 30)
            {
                message = string.Format(GetMessage("ChangePassword.MaxSize"), 30);
                return false;
            }

            // Adiciona senha velha na memória de senhas
            if (memoria_senhas > 0)
            {
                senhas_anteriores = HdPal(oldpassword) + senhas_anteriores;
                if (senhas_anteriores.Length > memoria_senhas * newpasswordc.Length)
                {
                    senhas_anteriores = senhas_anteriores.Substring(0, (int)(memoria_senhas * newpasswordc.Length));
                }
            }
            else
            {
                senhas_anteriores = string.Empty;
            }

            // Altera a senha
            var cn = HadesConnectionList.CreateWritableConnectionWithoutPermission("Hades");
            cn.Open(true);
            try
            {
                TCommand.ExecuteNonQuery(cn,
                                         "UPDATE hd_usuario " +
                                         "SET senha = ?, " +
                                         "senha_temp = ?, " +
                                         "data_alteracao_senha = ?, " +
                                         "alterar_senha = ?, " +
                                         "senhas_anteriores = ? " +
                                         "WHERE LOWER(usuario) = ?",
                                         newpasswordc, DBNull.Value, DateTime.Now, "N", senhas_anteriores, user.ToLower()
                    );
            }
            catch (Exception exc)
            {
                message = string.Format(GetMessage("ChangePassword.DBError"), exc.Message);
                return false;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

            // Autenticação concluída com sucesso
            message = null;
            return true;
        }

        internal static string GetMessage(string messagecode)
        {
            if (pvRM == null)
            {
                pvRM = new ResourceManager(typeof(HadesHttpApplicationBase));
            }

            return pvRM.GetString(messagecode) + string.Empty;
        }

        internal static string GetMessage(TechneAuthenticationResult result)
        {
            if (pvRM == null)
            {
                pvRM = new ResourceManager(typeof(HadesHttpApplicationBase));
            }

            return pvRM.GetString("TechneAuthenticationResult." + result) + string.Empty;
        }

        internal static TPermission GetPermission(IPrincipal princ, string sistema, string resource, string resourcetype)
        {
            var dsCache = DataSetRecursos(sistema);

            // Concede permissão para os httpHandlers configurados
            for (var i = 0; i < httpHandlersPermitidos.Length; i++)
            {
                if (resource.ToLower().EndsWith(httpHandlersPermitidos[i].ToLower()))
                {
                    return new HadesPermission(resource, "PAGINA", true, true, true, true);
                }
            }

            // Checa se o objeto existe buscando na view de recursos
            bool permiteAcessoAnonimo;
            string recurso;
            {
                var dvRecursos = new DataView(dsCache.Tables["hd_vw_recurso"]);
                dvRecursos.RowFilter = "recurso = '" + resource + "' AND tipo_recurso = '" + resourcetype + "'";
                if (dvRecursos.Count != 1)
                {
                    dvRecursos.RowFilter = "url = '" + resource + "' AND tipo_recurso = '" + resourcetype + "'";
                }

                // se o recurso não está cadastrado no Hades, concede permissões em casos especiais
                if (dvRecursos.Count != 1)
                {
                    // Concede permissão para web services
                    if (resource.ToLower().EndsWith(".asmx") || resource.ToLower().Contains(".asmx/"))
                    {
                        return new HadesPermission(resource, "PAGINA", true, true, true, true);
                    }

                    // Concede permissão para web services
                    if (resource.ToLower().EndsWith(".ashx") || resource.ToLower().Contains(".ashx/"))
                    {
                        return new HadesPermission(resource, "PAGINA", true, true, true, true);
                    }

                    // se não for web service, sai sem conceder pemissão
                    return null;
                }

                recurso = dvRecursos[0]["recurso"] as string;
                permiteAcessoAnonimo = dvRecursos[0]["acessoanonimo"].Equals("S");
            }

            bool exec = false, cad = false, alt = false, rem = false;

            // Se não houver usuário autenticado, cria usuário anônimo com padrão de acesso ANONIMO
            if (princ == null)
            {
                princ = CreateTPrincipal("anonimo", new[] { "?" }, false);
            }

            if (princ.IsInRole("PRIVILEGIADO"))
            {
                // Dá permissão pro usuário privilegiado
                exec = true;
                cad = true;
                alt = true;
                rem = true;
            }
            else if (princ.Identity.Name == "anonimo" && permiteAcessoAnonimo)
            {
                // Dá permissão pro usuário anônimo
                exec = true;
                cad = true;
                alt = true;
                rem = true;
            }
            else
            {
                // Busca as permissões
                var dvPermissoes = new DataView(dsCache.Tables["hd_vw_permissao"]);
                dvPermissoes.RowFilter = "recurso = '" + recurso + "' AND tipo_recurso = '" + resourcetype + "'";

                if (dvPermissoes.Count != 0)
                {
                    foreach (DataRowView drv in dvPermissoes)
                    {
                        if (princ.IsInRole(drv["padaces"] as string))
                        {
                            exec = true;
                            cad = cad || (drv["podecad"] as string == "S");
                            alt = alt || (drv["podealt"] as string == "S");
                            rem = rem || (drv["poderem"] as string == "S");
                        }
                    }
                }
            }

            return new HadesPermission(recurso, resourcetype, exec, cad, alt, rem);
        }

        internal static bool IsAudit(string sistema, string resource, string resourcetype)
        {
            var dsCache = DataSetRecursos(sistema);
            var dvRecursos = new DataView(dsCache.Tables["hd_vw_recurso"]);
            dvRecursos.RowFilter = "recurso = '" + resource + "' AND tipo_recurso = '" + resourcetype + "'";
            if (dvRecursos.Count != 1)
            {
                dvRecursos.RowFilter = "url = '" + resource + "' AND tipo_recurso = '" + resourcetype + "'";
            }

            if (dvRecursos.Count == 1)
            {
                return "S".Equals(dvRecursos[0]["AUDITAR"] as string, StringComparison.InvariantCultureIgnoreCase);
            }
            else
            {
                return false;
            }
        }

        protected internal override ConnectionList GetConnectionList()
        {
            if (ConnectionList.Current == null)
            {
                ConnectionList.Current = new HadesConnectionList();
            }

            return ConnectionList.Current;
        }

        protected internal override TPermission GetPermission(string resource, string resourcetype)
        {
            return GetPermission(Thread.CurrentPrincipal, Settings.AppName, resource, resourcetype);
        }

        protected internal override TLog GetTLog()
        {
            return new TLog();
        }

        /// <summary>
        ///   Retorna dados do usuário do Hades. Retorna Null se o usuário não existir ou se estiver desabilitado
        /// </summary>
        /// <param name = "user">Código do usuário</param>
        /// <param name="roles">Perfis do usuário</param>
        protected internal override IPrincipal GetUserInfo(string user, out string[] roles)
        {
            var tabPadusuario = new QueryTable("SELECT padaces FROM hd_padusuario (NOLOCK) WHERE usuario = ?");

            SimpleRow rowUsuario;
            SimpleRow[] rowsPadusuario;

            var cn = ConnectionList.CreateConnection("Hades");

            cn.Open();

            try
            {
                rowUsuario = SimpleRow.QueryFirstRow(
                    cn,
                    @"SELECT usuario, privilegiado 
                    FROM hd_usuario (NOLOCK) 
                    WHERE UPPER(usuario) = UPPER(?) 
                    AND UPPER(habilitado) = 'S'",
                    user);

                if (rowUsuario == null)
                {
                    roles = new string[0];

                    return null;
                }

                rowsPadusuario = tabPadusuario.Query(cn, rowUsuario["usuario"].ToString().Trim());
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

            var privilegiado = ((VarChar)rowUsuario["privilegiado"]).Equals("S", true);

            // Pega padrões de acesso do usuário Hades
            var rolesList = new ArrayList();

            foreach (var row in rowsPadusuario)
            {
                rolesList.Add((string)row["padaces"]);
            }

            if (privilegiado)
            {
                rolesList.Add("PRIVILEGIADO");
            }

            // Força via código que o usuário tenha acesso ao padrão publico, representado por asterisco
            if (!rolesList.Contains("*"))
            {
                rolesList.Add("*");
            }

            roles = (string[])rolesList.ToArray(typeof(string));

            return CreateTPrincipal((string)rowUsuario["usuario"].ToString().Trim(), roles, privilegiado);
        }

        protected internal override bool IsAudit(string resource, string resourcetype)
        {
            return IsAudit(Settings.AppName, resource, resourcetype);
        }

        /// <summary>
        ///   Valida usuário e senha do Hades.
        /// </summary>
        /// <param name = "user">Usuário do Hades</param>
        /// <param name = "password">Senha do usuário no Hades</param>
        /// <returns>Retorna OK, ChangePassword, InvalidUserOrPassword ou UserDisabled, dependendo do resultado da validação</returns>
        protected internal override TechneAuthenticationResult ValidateUser(string user, string password)
        {
            var now = DateTime.Now;
            var conn = HadesConnectionList.CreateWritableConnectionWithoutPermission("Hades");
            conn.Open(true);
            try
            {
                var isOracle = conn.Rdbms == Rdbms.Oracle;
                if (!isOracle && conn.Rdbms != Rdbms.SQLServer)
                {
                    throw new NotImplementedException();
                }

                var colunaSenha = isOracle || !ForceLegacyPasswords ? "senha" : "CAST(senha AS VARBINARY) senha";

                var rowUsuario = SimpleRow.QueryFirstRow(conn,
                                                         "SELECT usuario, " + colunaSenha + ", habilitado, alterar_senha, ultimo_login, tentativas_incorretas, data_alteracao_senha " +
                                                         "FROM hd_usuario (NOLOCK) " +
                                                         "WHERE UPPER(usuario) = UPPER(?)",
                                                         user
                    );

                // Checa se existe usuário
                if (rowUsuario == null)
                {
                    return TechneAuthenticationResult.InvalidUserOrPassword;
                }

                bool senhaValida;
                bool clientServer;
                if (isOracle || !ForceLegacyPasswords)
                {
                    var senha = rowUsuario["senha"].IsNull ? string.Empty : (string)rowUsuario["senha"];
                    senhaValida = ValidaSenha(password, senha, out clientServer);
                }
                else
                {
                    var senha = rowUsuario["senha"].IsNull ? new byte[0] : (byte[])rowUsuario["senha"];
                    senhaValida = ValidaSenha(password, senha, out clientServer);
                }

                var habilitado = rowUsuario["habilitado"].IsNull ? "N" : ((string)rowUsuario["habilitado"]).ToUpper();

                // Checa se a senha confere
                if (!senhaValida)
                {
                    if (habilitado == "S")
                    {
                        // Atualiza tentativas_incorretas,habilitado e motivo_desabilitado
                        decimal tentativas_incorretas = 0, tentativas_permitidas = -1;
                        var maxTentativasLogin = HadesParameter.Get("Hades", "SEGURANCA", "MAXTENTATIVASLOGIN");
                        if (maxTentativasLogin is decimal)
                        {
                            tentativas_permitidas = (decimal)maxTentativasLogin;
                        }

                        tentativas_incorretas = rowUsuario["tentativas_incorretas"].IsNull ? 0 : (decimal)rowUsuario["tentativas_incorretas"];
                        tentativas_incorretas++;
                        if (tentativas_permitidas > 0)
                        {
                            if (tentativas_incorretas >= tentativas_permitidas)
                            {
                                var motivo = string.Format(TechneAuthentication.GetMessage("ValidateUser.DisableUserReason"), tentativas_incorretas);
                                TCommand.ExecuteNonQuery(conn,
                                                         "UPDATE hd_usuario SET tentativas_incorretas = ?, habilitado = ?, motivo_desabilitado = ? WHERE usuario = ?",
                                                         tentativas_incorretas, "N", motivo, user
                                    );
                            }
                            else
                            {
                                TCommand.ExecuteNonQuery(conn,
                                                         "UPDATE hd_usuario SET tentativas_incorretas = ? WHERE usuario = ?",
                                                         tentativas_incorretas, user
                                    );
                            }
                        }
                    }

                    return TechneAuthenticationResult.InvalidUserOrPassword;
                }

                // Checa se o usuário está habilitado
                if (habilitado != "S")
                {
                    return TechneAuthenticationResult.UserDisabled;
                }

                // //Checa período de inatividade, e desabilita usuário se for o caso

                // if(tabUsuario.Rows[0]["ultimo_login"] is DateTime) {

                // decimal inatividade=180;

                // hdpar=HadesParameter.Get("Hades","SEGURANCA","INATIVIDADEPERMITIDA");

                // if(hdpar is decimal)

                // inatividade=(decimal)hdpar;

                // ultimo_login=(DateTime)tabUsuario.Rows[0]["ultimo_login"];

                // if(ultimo_login.AddDays((double)inatividade) < now) {

                // TCommand.ExecuteNonQuery(conn, "UPDATE hd_usuario SET habilitado = ? WHERE usuario = ?", "N", user);

                // return TechneAuthenticationResult.UserDisabled;

                // }

                // }

                // Checa se ele tem que mudar a senha
                if (((VarChar)rowUsuario["alterar_senha"]).Equals("S", true))
                {
                    return TechneAuthenticationResult.ChangePassword;
                }

                // Força mudança de senha se ela expirou
                if (!rowUsuario["DATA_ALTERACAO_SENHA"].IsNull)
                {
                    decimal duracaoSenhaDef = 90;
                    var duracaoSenha = HadesParameter.Get("Hades", "SEGURANCA", "DURACAOSENHA");
                    if (duracaoSenha is decimal)
                    {
                        duracaoSenhaDef = (decimal)duracaoSenha;
                    }

                    var alteracao_senha = (DateTime)rowUsuario["DATA_ALTERACAO_SENHA"];
                    if (alteracao_senha.AddDays((double)duracaoSenhaDef) < now)
                    {
                        TCommand.ExecuteNonQuery(conn,
                                                 "UPDATE hd_usuario SET alterar_senha = 'S' WHERE usuario = ?",
                                                 user
                            );
                        return TechneAuthenticationResult.ChangePassword;
                    }
                }

                // Registra login
                if (!clientServer || ForceLegacyPasswords)
                {
                    TCommand.ExecuteNonQuery(conn,
                                             "UPDATE hd_usuario SET ultimo_login = ?, tentativas_incorretas = 0 WHERE usuario = ?",
                                             now, user
                        );
                }
                else
                {
                    // Atualiza a senha no formato client-server para o novo formato
                    TCommand.ExecuteNonQuery(conn,
                                             "UPDATE hd_usuario SET ultimo_login = ?, tentativas_incorretas = 0, senha = ? WHERE usuario = ?",
                                             now, HdPal(password), user
                        );
                }
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            // Autenticação concluída com sucesso
            return TechneAuthenticationResult.OK;
        }

        /// <summary>
        ///   Valida um usuário do windows contra o banco de dados do Hades. Se
        ///   o usuário for válido e estiver habilitado, retorna o código do usuário no Hades
        ///   Caso contrário, retorna null.
        /// </summary>
        /// <param name = "windowsuser">Usuário do Windows</param>
        /// <param name = "message">Mensagem de erro</param>
        /// <returns>código do usuário Hades correspondente</returns>
        protected internal override TechneAuthenticationResult ValidateWindowsUser(string windowsuser, out string user)
        {
            var conn = HadesConnectionList.CreateWritableConnectionWithoutPermission("Hades");

            user = null;
            var tabUsuario = new QueryTable(
                "SELECT usuario, habilitado, ultimo_login " + // privilegiado, senha,
                "FROM hd_usuario (NOLOCK) " +
                "WHERE LOWER(winusuario) = ?"
                );
            tabUsuario.Query(conn, windowsuser.Trim().ToLower());

            // Checa se existe usuário
            if (tabUsuario.Rows.Count == 0)
            {
                return TechneAuthenticationResult.InvalidUserOrPassword;
            }
            else if (tabUsuario.Rows.Count > 1)
            {
                throw new ApplicationException("Mais de um usuário cadastrado com o mesmo usuário Windows.");
            }

            var rowUsuario = tabUsuario.Rows[0];

            var habilitado = ((VarChar)rowUsuario["habilitado"]).Equals("S", true);

            // Checa se o usuário está habilitado
            if (!habilitado)
            {
                return TechneAuthenticationResult.UserDisabled;
            }

            // Pega usuário
            user = (string)rowUsuario["usuario"].ToString().Trim();

            conn.Open(true);
            try
            {
                // Checa período de inatividade, e desabilita usuário se for o caso
                if (!rowUsuario["ultimo_login"].IsNull)
                {
                    decimal inatividade = 180;
                    var hdpar = HadesParameter.Get("Hades", "SEGURANCA", "INATIVIDADEPERMITIDA");
                    if (hdpar is decimal)
                    {
                        inatividade = (decimal)hdpar;
                    }

                    var ultimo_login = (DateTime)rowUsuario["ultimo_login"];
                    if (ultimo_login.AddDays((double)inatividade) < DateTime.Now)
                    {
                        TCommand.ExecuteNonQuery(conn,
                                                 "UPDATE hd_usuario " +
                                                 "SET habilitado = ? " +
                                                 "WHERE usuario = ?",
                                                 "N", user
                            );
                        return TechneAuthenticationResult.UserDisabled;
                    }
                }

                // Registra login
                var dateFunction = conn.Rdbms == Rdbms.Oracle ? "SYSDATE" : "GETDATE()";
                TCommand.ExecuteNonQuery(conn,
                                         "UPDATE hd_usuario " +
                                         "SET ultimo_login = " + dateFunction + ", " +
                                         "tentativas_incorretas = 0 " +
                                         "WHERE usuario = ?",
                                         user
                    );
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            // Autenticação concluída com sucesso
            return TechneAuthenticationResult.OK;
        }

        private static bool ArraysEqual(byte[] a1, byte[] a2)
        {
            if (a1 == null)
            {
                return a2 == null;
            }
            else if (a2 == null || a1.Length != a2.Length)
            {
                return false;
            }
            else
            {
                for (var i = 0; i < a1.Length; i++)
                {
                    if (a1[i] != a2[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static DataSet CarregaPermissoes(string sistema)
        {
            var ds = new DataSet();
            ds.ExtendedProperties["data"] = DateTime.Now;

            // tabRecurso
            var tabRecurso = new QueryTable(
                "SELECT tipo_recurso, recurso, nome, url, trans, acessoanonimo, auditar " +
                "FROM (" +
                "SELECT sis, trans recurso, 'PAGINA' tipo_recurso, nome, paginaweb url, trans, acessoanonimo, auditar " +
                "FROM hd_transacao (NOLOCK) " +
                "UNION " +
                "SELECT sis, recurso, tipo_recurso, nome, url, trans, 'N' acessoanonimo, 'N' auditar " +
                "FROM hd_recurso (NOLOCK) " +
                "UNION " +
                "SELECT sis, relatorio recurso, 'RELATORIO' tipo_recurso, descricao nome, NULL url, NULL trans, 'N' acessoanonimo, auditar " +
                "FROM hd_relatorio (NOLOCK) " +
                "UNION " +
                "SELECT sis, processo recurso, 'PROCESSO' tipo_recurso, descricao nome, NULL url, NULL trans, 'N' acessoanonimo, auditar " +
                "FROM hd_processo (NOLOCK) " +
                ") hd_vw_recurso " +
                "WHERE sis = ?"
                );
            tabRecurso.TableName = "hd_vw_recurso";
            ds.Tables.Add(tabRecurso);

            // tabPermissao
            var tabPermissao = new QueryTable(
                "SELECT padaces, sis, tipo_recurso, recurso, podecad, podealt, poderem " +
                "FROM (" +
                "SELECT '*' padaces, t.sis, t.trans recurso, 'PAGINA' tipo_recurso, 'S' podecad, 'S' podealt, 'S' poderem " +
                "FROM hd_transacao t (NOLOCK) " +
                "WHERE publica = 'S' AND " +
                "NOT EXISTS (SELECT pt.padaces " +
                "FROM hd_padtrans pt " +
                "WHERE pt.padaces = '*' AND " +
                "pt.sis = t.sis AND " +
                "pt.trans = t.trans) " +
                "UNION " +
                "SELECT padaces, sis, recurso, tipo_recurso, podecad, podealt, poderem " +
                "FROM hd_padrec (NOLOCK) " +
                "UNION " +
                "SELECT padaces, sis, trans recurso, 'PAGINA' tipo_recurso, podecad, podealt, poderem " +
                "FROM hd_padtrans (NOLOCK) " +
                "UNION " +
                "SELECT padaces, sis, relatorio recurso, 'RELATORIO' tipo_recurso, 'N' podecad, 'N' podealt, 'N' poderem " +
                "FROM hd_padrel (NOLOCK) " +
                "UNION " +
                "SELECT padaces, sis, processo recurso, 'PROCESSO' tipo_recurso, 'S' podecad, 'S' podealt, 'S' poderem " +
                "FROM hd_padproc (NOLOCK) " +
                ") hd_vw_permissao " +
                "WHERE sis = ?"
                );
            tabPermissao.TableName = "hd_vw_permissao";
            ds.Tables.Add(tabPermissao);

            // Realiza a consulta no banco
            var cn = ConnectionList.CreateConnection("Hades");
            cn.Open();
            try
            {
                tabRecurso.Query(cn, sistema);
                tabPermissao.Query(cn, sistema);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

            return ds;
        }

        private static DataSet DataSetRecursos(string sistema)
        {
            var dsCache = (DataSet)cache[sistema];

            // Recarrega as permissões quando necessário
            if (forceRefresh || dsCache == null || (dsCache.ExtendedProperties["data"] is DateTime &&
                                                    ((DateTime)dsCache.ExtendedProperties["data"]) < DateTime.Now.AddMinutes(-5)))
            {
                lock (sync)
                    if (forceRefresh || dsCache == null || (dsCache.ExtendedProperties["data"] is DateTime &&
                                                            ((DateTime)dsCache.ExtendedProperties["data"]) < DateTime.Now.AddMinutes(-5)))
                    {
                        var ds = CarregaPermissoes(Settings.AppName);
                        if (ds != null)
                        {
                            dsCache = ds;
                            cache[sistema] = dsCache;
                            forceRefresh = false;
                        }
                    }
            }

            if (dsCache == null)
            {
                throw new ApplicationException("Falha no acesso ao Hades");
            }

            return dsCache;
        }

        private static string HdPal(string text)
        {
            var b = _HdPal(text);

            if (b.Length == 0)
            {
                return string.Empty;
            }
            else if (!ForceLegacyPasswords)
            {
                return Convert.ToBase64String(b);
            }
            else
            {
                return Encoding.GetEncoding(1252).GetString(b);
            }
        }

        private static byte[] HdPal2(string text)
        {
            if (!ForceLegacyPasswords)
            {
                throw new InvalidOperationException();
            }

            return _HdPal(text);
        }

        private static bool ValidaSenha(string digitada, byte[] cadastrada)
        {
            bool clientServer;
            return ValidaSenha(digitada, cadastrada, out clientServer);
        }

        private static bool ValidaSenha(string digitada, string cadastrada)
        {
            bool clientServer;
            return ValidaSenha(digitada, cadastrada, out clientServer);
        }

        private static bool ValidaSenha(string digitada, byte[] cadastrada, out bool clientServer)
        {
            clientServer = false;

            try
            {
                if (ArraysEqual(HdPal2(digitada), cadastrada))
                {
                    // Senha válida
                    return true;
                }
                else if (digitada == cs(cadastrada))
                {
                    // Senha antiga do client-server
                    clientServer = true;
                    return true;
                }
                else
                {
                    // Senha não reconhecida
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private static bool ValidaSenha(string digitada, string cadastrada, out bool clientServer)
        {
            clientServer = false;

            try
            {
                if (HdPal(digitada) == cadastrada)
                {
                    // Senha válida
                    return true;
                }
                else if (digitada == cs(cadastrada))
                {
                    // Senha antiga do client-server
                    clientServer = true;
                    return true;
                }
                else
                {
                    // Senha não reconhecida
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private static byte[] _HdPal(string text)
        {
            const string h = "TECHNELYCEUM";
            if (text == null || text == string.Empty)
            {
                return new byte[0];
            }

            if (text.Length > 30)
            {
                text = text.Substring(0, 30);
            }
            else
            {
                text = text.PadRight(30, ' ');
            }

            var WSenha = new byte[30];
            byte WOffset = 0;
            var j = 0;

            var enc = Encoding.GetEncoding(1252);
            for (var i = 0; i < 30; i++)
            {
                var a = enc.GetBytes(text.Substring(i, 1))[0];
                var b = enc.GetBytes(h.Substring(j, 1))[0];
                var cod = Convert.ToByte((a + WOffset + b) & 255);
                WSenha[i] = cod;
                WOffset = Convert.ToByte((WOffset + b) & 255);
                if (++j >= h.Length)
                {
                    j = 0;
                }
            }

            return WSenha;
        }
    }
}