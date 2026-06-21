using System;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Data;
using System.Linq;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    public class PadroesDeAcessos : RNBase
    {
        [
        MethodDescription("Valida os dados de padrões de acesso para inserção e atualização"),
        ToolTip("Valida se o padrão de acesso existe"),
        ControlText("Validação"),
        ]

        public static bool VerificarPadaces(string padaces)
        {
            if (!string.IsNullOrEmpty(padaces))
            {
                TConnection connection = Techne.HadesLyc.Config.CreateConnection();
                connection.Open();

                try
                {

                    string sql = " select 1 from HD_PADACES where PADACES = ? ";

                    DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, padaces);

                    if (!valorConsulta.IsNull)
                        return true;
                }
                finally
                {
                    connection.Close();
                }
            }

            return false;
        }

        public static QueryTable ConsultarMenu()
        {
            TConnection cn = HadesLyc.Config.CreateConnection();

            QueryTable qt = new QueryTable("select TEXTO, TRANS, MENUITEM, ORDEM from HD_MENUITEM where MENUITEMPAI is null order by ORDEM");

            qt.Query(cn);

            return qt;
        }

        public static QueryTable ConsultarMenu(string padaces)
        {
            TConnection cn = HadesLyc.Config.CreateConnection();

            QueryTable qt = new QueryTable(
                @"select m.TEXTO, m.TRANS, m.MENUITEM, m.ORDEM 
                  from HD_MENUITEM m where m.MENUITEMPAI is null 
                  and exists (select 1 from HD_PADTRANS pad where pad.PADACES = ? and pad.TRANS = m.TRANS)");

            qt.Query(cn, padaces);

            return qt;
        }

        public static QueryTable ConsultarItemMenu(string padaces)
        {
            TConnection cn = HadesLyc.Config.CreateConnection();

            QueryTable qt = new QueryTable(
                @"select TEXTO, TRANS, MENUITEM, MENUITEMPAI, ORDEM, SIS, 
                (select MENUITEMPAI from HD_MENUITEM where MENUITEM = m.MENUITEMPAI) as intermediario
                from HD_MENUITEM m where MENUITEMPAI is not null and IMAGEM is null 
                and exists(select 1 from  HD_PADTRANS pad where pad.padaces = ? and m.TRANS = PAD.trans)");
            qt.Query(cn, padaces);

            return qt;
        }

        //public static QueryTable ConsultarItemMenuCompleto()
        //{
        //    TConnection cn = HadesLyc.Config.CreateConnection();

        //    QueryTable qt = new QueryTable("select TEXTO, TRANS, MENUITEM, MENUITEMPAI, ORDEM,  " +
        //                                    "(select MENUITEMPAI from HD_MENUITEM where MENUITEM = m.MENUITEMPAI) as intermediario " +
        //                                    "from HD_MENUITEM m where MENUITEMPAI is not null and IMAGEM is null");

        //    qt.Query(cn);

        //    return qt;
        //}

        public static QueryTable ConsultarItensMenu(string superPai)
        {
            TConnection cn = HadesLyc.Config.CreateConnection();

            QueryTable qt = new QueryTable("select TEXTO, TRANS, MENUITEM, MENUITEMPAI, ORDEM, SIS,  " +
                "(select MENUITEMPAI from HD_MENUITEM where MENUITEM = m.MENUITEMPAI) as superPai " +
                "from HD_MENUITEM m where MENUITEMPAI is not null and IMAGEM is null " +
                "and (select MENUITEMPAI from HD_MENUITEM where MENUITEM = m.MENUITEMPAI) = ? ");

            qt.Query(cn, superPai);

            return qt;
        }

        public static bool VerificaExiste(string padaces, string trans, string sistema)
        {
            string sql = "select 1 from HD_PADTRANS where PADACES = ? and TRANS = ? and SIS = ?";

            int retorno = ExecutarFuncaoHades(sql, padaces, trans, sistema);

            if (retorno == 1)
                return true;//tem acesso a transação
            else
                return false;
        }

        public bool ExistePadraoAcessoPor(DataContext contexto, string padaces, string trans, string sistema)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   HD_PADTRANS (NOLOCK) 
                                        WHERE  PADACES = @PADACES 
                                               AND TRANS = @TRANS 
                                               AND SIS = @SIS  ";

            contextQuery.Parameters.Add("@PADACES", padaces);
            contextQuery.Parameters.Add("@TRANS", trans);
            contextQuery.Parameters.Add("@SIS", sistema);  

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public static bool VerificaPrivilegio(string usuario)
        {
            string sql = "select 1 from usuario where PRIVIL = 'S' and USUARIO = ?";

            int retorno = ExecutarFuncaoHades(sql, usuario);

            if (retorno == 1)
                return true;//é privilegiado
            else
                return false;
        }


        public static bool VerificaExiste(TConnectionWritable connection, string padaces, string trans, string sistema)
        {
            string sql = "select 1 from HD_PADTRANS where PADACES = ? and TRANS = ? and SIS = ? ";

            int retorno = ExecutarFuncaoHades(connection, sql, padaces, trans, sistema);

            if (retorno == 1)
                return true;
            else
                return false;
        }

        public static bool VerificaPodeAlt(string padaces, string trans, string sistema)
        {
            string sql = "select 1 from HD_PADTRANS where PADACES = ? and TRANS= ? and PODEALT = 'S' and SIS = ? ";

            int retorno = ExecutarFuncao(sql, padaces, trans, sistema);

            if (retorno == 1)
                return true;
            else
                return false;
        }

        public static bool VerificaPodeCad(string padaces, string trans, string sistema)
        {
            string sql = "select 1 from HD_PADTRANS where PADACES = ? and TRANS= ? and PODECAD = 'S' and SIS = ?";

            int retorno = ExecutarFuncao(sql, padaces, trans, sistema);

            if (retorno == 1)
                return true;
            else
                return false;
        }

        public static bool VerificaPodeRem(string padaces, string trans, string sistema)
        {
            string sql = "select 1 from HD_PADTRANS where PADACES = ? and TRANS= ? and PODEREM = 'S' and SIS = ?";

            int retorno = ExecutarFuncao(sql, padaces, trans, sistema);

            if (retorno == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// deleta uma página por vez
        /// </summary>
        public static void DeletaPagina(TConnectionWritable conn, string padaces, string trans, int menuitem, string sistema)
        {

            string sql = "delete HD_PADTRANS where PADACES = ? and TRANS = ? and SIS = ? ";

            IAE(conn, sql, padaces, trans, sistema);

            QueryTable qt = ConsultarTerceiroNivel(conn, menuitem);
            if (qt != null)
            {
                if (qt.Rows.Count > 0)
                {
                    for (int i = 0; i < qt.Rows.Count; i++)
                    {
                        trans = qt.Rows[i]["TRANS"].ToString();
                        sistema = qt.Rows[i]["SIS"].ToString();
                        sql = " delete HD_PADTRANS where PADACES = ? and TRANS = ? and SIS = ? ";
                        IAE(conn, sql, padaces, trans, sistema);
                    }
                }
            }
        }

        public static void DeletaMenu(TConnectionWritable conn, string padaces, string trans, string sistema)
        {
            string sql = "delete HD_PADTRANS where PADACES = ? and TRANS = ? and SIS = ? ";
            IAE(conn, sql, padaces, trans, sistema);
        }


        public static void InserePagina(TConnectionWritable conn, string padaces, string trans, string podecad, string podealt, string poderem, int menuitem, string sistema)
        {
            string sql;
            if (!VerificaExiste(conn, padaces, trans, sistema))
            {
                sql = "insert into HD_PADTRANS values (?, ?, ?, ? ,? ,? )";
                IAE(conn, sql, padaces, sistema, trans, podealt, podecad, poderem);
            }
            else
            {
                sql = "update HD_PADTRANS set PODEALT = ?, PODECAD = ?, PODEREM = ? where TRANS = ? and PADACES = ? and SIS = ?";
                IAE(conn, sql, podealt, podecad, poderem, trans, padaces, sistema);
            }

            //verifica se tem terceiro nivel
            QueryTable qt = ConsultarTerceiroNivel(conn, menuitem);
            if (qt != null)
            {
                if (qt.Rows.Count > 0)
                {
                    for (int i = 0; i < qt.Rows.Count; i++)
                    {
                        trans = qt.Rows[i]["TRANS"].ToString();
                        sistema = qt.Rows[i]["SIS"].ToString();

                        if (!VerificaExiste(conn, padaces, trans, sistema))
                        {
                            sql = "insert into HD_PADTRANS values (?, ?, ?, ? ,? ,? )";
                            IAE(conn, sql, padaces, sistema, trans, podealt, podecad, poderem);
                        }
                        else
                        {
                            sql = "update HD_PADTRANS set PODEALT = ?, PODECAD = ?, PODEREM = ? where TRANS = ? and PADACES = ? and SIS = ? ";
                            IAE(conn, sql, podealt, podecad, poderem, trans, padaces, sistema);
                        }
                    }
                }
            }

        }

        //nível não é listado no menu, seus valores de alteração/remoção/cadastro são copiados dos valores isneridos para os pais, invisível ao usuário
        public static QueryTable ConsultarTerceiroNivel(TConnection connection, int menuitempai)
        {
            QueryTable qt = new QueryTable(" select TRANS, MENUITEM, SIS from HD_MENUITEM m where MENUITEMPAI = ? and TIPO = 'SUBTELA'");
            qt.Query(connection, menuitempai);

            return qt;
        }

        //operações da página diferentes de alteração/remoção/cadastro
        public static QueryTable ConsultarOperacoes(int menuitempai)
        {
            TConnection cn = HadesLyc.Config.CreateConnection();
            QueryTable qt = new QueryTable(" select TRANS, TEXTO from HD_MENUITEM m where MENUITEMPAI = ? and TIPO = 'OPERACAO'");
            qt.Query(cn, menuitempai);

            return qt;
        }

        //operações da página diferentes de alteração/remoção/cadastro passando a conexão
        public static QueryTable ConsultarOperacoes(TConnection cn, int menuitempai)
        {
            QueryTable qt = new QueryTable(" select TRANS, TEXTO from HD_MENUITEM m where MENUITEMPAI = ? and TIPO = 'OPERACAO'");
            qt.Query(cn, menuitempai);
            return qt;
        }


        public static void InsereMenu(TConnectionWritable conn, string padaces, string trans, string sistema)
        {

            string sql = "insert into HD_PADTRANS values (?, ?, ?, 'N', 'N' ,'N')";

            IAE(conn, sql, padaces, sistema, trans);

        }

        public static string ConsultaTransacao(TConnection connection, string menuitem)
        {
            string sql = "select TRANS from HD_MENUITEM where MENUITEM = ?";
            DbObject valor = null;

            valor = TCommand.ExecuteScalar(connection, sql, menuitem);
            if (!valor.IsNull)
                return Convert.ToString(valor);

            return string.Empty;
        }

        public static string ConsultaTransacao(string menuitem)
        {
            TConnection connection = HadesLyc.Config.CreateConnection();
            try
            {
                connection.Open();

                string sql = "select TRANS from HD_MENUITEM where MENUITEM = ?";
                DbObject valor = null;

                valor = TCommand.ExecuteScalar(connection, sql, menuitem);
                if (!valor.IsNull)
                    return Convert.ToString(valor);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return string.Empty;
        }

        public static string ConsultaSistema(string menuitem)
        {
            TConnection connection = HadesLyc.Config.CreateConnection();
            try
            {
                connection.Open();

                string sql = "select SIS from HD_MENUITEM where MENUITEM = ?";
                DbObject valor = null;
                valor = TCommand.ExecuteScalar(connection, sql, menuitem);
                if (!valor.IsNull)
                    return Convert.ToString(valor);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return string.Empty;
        }

        public static string ConsultaSistema(TConnection connection, string menuitem)
        {
            string sql = "select SIS from HD_MENUITEM where MENUITEM = ?";
            DbObject valor = null;
            valor = TCommand.ExecuteScalar(connection, sql, menuitem);
            if (!valor.IsNull)
                return Convert.ToString(valor);

            return string.Empty;
        }

        public static string SalvaPadroes(System.Web.UI.WebControls.TreeView treePaginas, string padaces, ref bool ok)
        {
            TConnectionWritable conn = Techne.HadesLyc.Config.CreateWritableConnection();
            try
            {
                conn.Open(true);
                string transacao = string.Empty;
                string sistema = string.Empty;
                string trans_oper = string.Empty;
                //transacao associada ao valor menuitem
                transacao = RN.PadroesDeAcessos.ConsultaTransacao(conn, treePaginas.Nodes[0].Value.ToString());
                sistema = RN.PadroesDeAcessos.ConsultaSistema(conn, treePaginas.Nodes[0].Value.ToString());

                #region Insere ou Deleta MENUs
                //se estiver checado insere o menu, senão deleta ele caso já exista
                if (treePaginas.Nodes[0].Checked == true)
                {
                    if (!string.IsNullOrEmpty(transacao))
                    {
                        if (!VerificaExiste(conn, padaces, transacao, sistema))
                        {
                            RN.PadroesDeAcessos.InsereMenu(conn, padaces, transacao, sistema);
                        }
                    }
                }
                else
                {
                    //deleta Menu
                    if (VerificaExiste(conn, padaces, transacao, sistema))
                    {
                        DeletaMenu(conn, padaces, transacao, sistema);
                    }
                }
                #endregion

                #region InsereFilhos
                foreach (System.Web.UI.WebControls.TreeNode noPagina in treePaginas.Nodes[0].ChildNodes)
                {
                    //consulta transação associada ao valor menuitem do filho
                    string transFilho = RN.PadroesDeAcessos.ConsultaTransacao(conn, noPagina.Value);
                    string sisFilho = RN.PadroesDeAcessos.ConsultaSistema(conn, noPagina.Value);

                    if (noPagina.Checked == true)
                    {
                        string podecad = "N", podealt = "N", poderem = "N";
                        foreach (System.Web.UI.WebControls.TreeNode noPode in noPagina.ChildNodes)
                        {
                            if (noPode.Value == "podecad" && noPode.Checked == true)
                                podecad = "S";
                            else if (noPode.Value == "podealt" && noPode.Checked == true)
                                podealt = "S";
                            else if (noPode.Value == "poderem" && noPode.Checked == true)
                                poderem = "S";
                            else if (noPode.Value != "podecad" && noPode.Value != "podealt" && noPode.Value != "poderem" && noPode.Checked == true)
                            {
                                //existe um nó operação checado e deve ser inserida sua transação
                                trans_oper = Convert.ToString(noPode.Value);
                                InserePagina(conn, padaces, trans_oper, "N", "N", "N", -1, sisFilho);

                            }
                            else if (noPode.Value != "podecad" && noPode.Value != "podealt" && noPode.Value != "poderem" && noPode.Checked != true)
                            {
                                //não está checada a operação e será deletada caso exista
                                trans_oper = Convert.ToString(noPode.Value);
                                if (VerificaExiste(conn, padaces, trans_oper, sisFilho))
                                    DeletaPagina(conn, padaces, trans_oper, -1, sisFilho);
                            }
                        }

                        if (!string.IsNullOrEmpty(transFilho))
                        {
                            InserePagina(conn, padaces, transFilho, podecad, podealt, poderem, Convert.ToInt32(noPagina.Value), sisFilho);
                        }
                    }
                    else //página não está checada:
                    {
                        //deleta a transação toda
                        if (!string.IsNullOrEmpty(transFilho))
                            if (VerificaExiste(conn, padaces, transFilho, sisFilho))
                                DeletaPagina(conn, padaces, transFilho, Convert.ToInt32(noPagina.Value), sisFilho);

                        //consulta e deleta todas as operações dela
                        QueryTable qt_oper = RN.PadroesDeAcessos.ConsultarOperacoes(conn, Convert.ToInt32(noPagina.Value));
                        if (qt_oper != null)
                            if (qt_oper.Rows.Count > 0)
                                for (int f = 0; f < qt_oper.Rows.Count; f++)
                                    if (VerificaExiste(conn, padaces, qt_oper.Rows[f]["TRANS"].ToString(), sisFilho))
                                        DeletaPagina(conn, padaces, qt_oper.Rows[f]["TRANS"].ToString(), -1, sisFilho);
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                conn.Rollback();
                ok = false;
                return "Erro ao alterar padrões de acesso. " + e.Message;

            }
            finally
            {
                conn.Close();
            }
            ok = true;
            return "Padrões de acesso alterados com sucesso.";
        }

        public static QueryTable ConsultarPadaces()
        {
            TConnection connection = Techne.HadesLyc.Config.CreateConnection();
            QueryTable qt = new QueryTable("Select padaces, nomepadaces from padaces order by nomepadaces");
            qt.Query(connection);

            return qt;
        }

        public static String[] ConsultarPadaces(TConnection connection, String usuario)
        {
            System.Collections.Generic.List<String> padaces = new System.Collections.Generic.List<String>();
            QueryTable qt = new QueryTable("select padaces from padusuario where usuario = ?");
            qt.Query(connection, usuario);
            return qt.Rows.Cast<SimpleRow>().Select(row => Convert.ToString(row[0])).ToArray();
        }

        public static Boolean ConsultarPadacesParcial(String usuario)
        {
            TConnection connection = Techne.HadesLyc.Config.CreateConnection();
            String sql = @"select padaces from padusuario where usuario = ? and padaces = 'ALTDOC_PARCIAL'";
            QueryTable qt = new QueryTable(sql);
            qt.Query(connection, usuario);
            if (qt.Rows.Count > 0)
                return true;
            else
                return false;

        }

        public static string ConsultarPadacesLicencas(String usuario, string motivo)
        {
            TConnection connection = Config.CreateConnection();
            string sql = "select top 1 padaces from padusuario where usuario = ? and PADACES in (select padaces from LY_PADACES_LICENCA where motivo = ?)";
            DbObject valor = null;
            valor = TCommand.ExecuteScalar(connection, sql, usuario, motivo);
            if (!valor.IsNull)
                return Convert.ToString(valor);

            return string.Empty;

        }

        public static DataTable VerificaPadrao(string usuario)
        {
            if (string.IsNullOrEmpty(usuario))
            {
                return null;
            }

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT DISTINCT
                                    PRIVILEGIADO,
                                    PADACES
                             FROM   HADES.dbo.HD_USUARIO u
                                    LEFT JOIN HADES.dbo.HD_PADUSUARIO pu ON u.USUARIO = pu.USUARIO
                             WHERE  u.USUARIO = @USUARIO
                                    AND ( u.PRIVILEGIADO = 'S'
                                          OR PU.PADACES IN ( 'ISNPECAO ESC', 'DIRETOR_UE' )
                                        )"
                };
                contextQuery.Parameters.Add("@USUARIO", usuario);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public bool PossuiPadraoDiretorPor(string usuario)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.PossuiPadraoDiretorPor(contexto, usuario);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }
        }

        public bool PossuiPadraoDiretorPor(DataContext contexto, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*)
                             FROM   HADES.dbo.HD_USUARIO u
                                    LEFT JOIN HADES.dbo.HD_PADUSUARIO pu ON u.USUARIO = pu.USUARIO
                             WHERE  u.USUARIO = @USUARIO
                                    AND PU.PADACES = 'DIRETOR_UE' ";

            contextQuery.Parameters.Add("@USUARIO", usuario);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public static bool VerificaPodeCadAlt(string usuario, string trans, string sistema)
        {
            string sql = "select 1 from HD_PADTRANS where PADACES IN (select PADACES from PADUSUARIO where USUARIO = ?) and TRANS= ? and (PODEALT = 'S' or PODECAD = 'S') and SIS = ? ";

            int retorno = ExecutarFuncao(sql, usuario, trans, sistema);

            if (retorno == 1)
                return true;
            else
                return false;
        }

        public static Boolean ConsultarPadacesContratoTempoPorUsuario(string usuario)
        {
            TConnection connection = Techne.HadesLyc.Config.CreateConnection();

            String sql = @"SELECT USU.USUARIO
							FROM HADES.DBO.HD_PADACES PA WITH (NOLOCK)
							INNER JOIN HADES.DBO.HD_PADUSUARIO PU WITH (NOLOCK) ON PA.PADACES = PU.PADACES
							INNER JOIN HADES.DBO.HD_USUARIO USU WITH (NOLOCK) ON USU.USUARIO = PU.USUARIO
							WHERE PA.PADACES = 'CONTRATO TEMPO' AND USU.USUARIO = ? ";
            QueryTable qt = new QueryTable(sql);
            qt.Query(connection, usuario);
            if (qt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        public static bool VerificaPodeIncluirExcluirSituacao(string padaces, string motivo)
        {
            string sql = "select 1 from LY_PADACES_LICENCA where padaces = ? and motivo = ?";

            int retorno = ExecutarFuncao(sql, padaces, motivo);

            if (retorno == 1)
                return true;//tem acesso a transação
            else
                return false;
        }

        public bool PodeIncluirExcluirSituacaoPor(DataContext contexto, string usuario, string motivo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT  COUNT(*)
                        FROM    DBO.LY_PADACES_LICENCA PL
                        WHERE   EXISTS ( SELECT *
                                         FROM   PADUSUARIO PU
                                         WHERE  USUARIO = @USUARIO
                                                AND PL.PADACES = PU.PADACES )
                                AND MOTIVO = @MOTIVO ";

            contextQuery.Parameters.Add("@USUARIO", usuario);
            contextQuery.Parameters.Add("@MOTIVO", motivo);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public string ObtemPadraoAcessoLicencaPor(string usuario, string situacao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            string padraoAcesso = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT TOP 1 PADACES 
                                            FROM   PADUSUARIO 
                                            WHERE  USUARIO = @USUARIO 
                                                   AND PADACES IN (SELECT PADACES 
                                                                   FROM   LY_PADACES_LICENCA 
                                                                   WHERE  MOTIVO = @SITUACAO) ";

                contextQuery.Parameters.Add("@USUARIO", usuario);
                contextQuery.Parameters.Add("@SITUACAO", situacao);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    if (reader["PADACES"] != DBNull.Value)
                    {
                        padraoAcesso = Convert.ToString(reader["PADACES"]);
                    }
                }

                return padraoAcesso;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

        public bool EhPadraoAcessoBloqueadoLancamentoPericiaPor(DataContext contexto, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   PADUSUARIO (NOLOCK) 
                                        WHERE  PADACES IN ('COORDENADORIA','LINKS_CGP')
                                               AND USUARIO = @USUARIO 
                                               ";

            contextQuery.Parameters.Add("@USUARIO", usuario);
 

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiPadraoCoordQHIPor(string usuario)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.PossuiPadraoCoordQHIPor(contexto, usuario);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }
        }

        public bool PossuiPadraoCoordQHIPor(DataContext contexto, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*)
                             FROM   HADES.dbo.HD_USUARIO u
                                    LEFT JOIN HADES.dbo.HD_PADUSUARIO pu ON u.USUARIO = pu.USUARIO
                             WHERE  u.USUARIO = @USUARIO
                                    AND PU.PADACES = 'COORD_QHI' ";

            contextQuery.Parameters.Add("@USUARIO", usuario);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiPadraoCOCACPor(string usuario)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.PossuiPadraoCOCACPor(contexto, usuario);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }
        }


        public bool PossuiPadraoCOCACPor(DataContext contexto, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*)
                             FROM   HADES.dbo.HD_USUARIO u
                                    LEFT JOIN HADES.dbo.HD_PADUSUARIO pu ON u.USUARIO = pu.USUARIO
                             WHERE  u.USUARIO = @USUARIO
                                    AND PU.PADACES = 'COCAC' ";

            contextQuery.Parameters.Add("@USUARIO", usuario);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }
    }
}
