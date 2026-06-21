namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Seeduc.Infra.Data;
    using Techne.Data;
    using Techne.HadesLyc.CR;
    using Techne.Library;
    using Techne.Lyceum.CR;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Lyceum.RN.Util;
    using System.Data.SqlClient;

    public class Usuarios : RNBase
    {
        /// <summary>
        /// Exclui usuário.
        /// </summary>
        /// <param name="usuario">usuário</param>
        /// <returns>sucesso ou erro</returns>
        public static RetValue Excluir(string usuario)
        {
            TConnectionWritable connection = HadesLyc.Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                Hd_usuario dtUsuario = Hd_usuario.Query(connection, "usuario = ?", usuario);

                if (dtUsuario != null)
                {
                    if (dtUsuario.Rows != null)
                    {
                        foreach (Hd_usuario.Row linha in dtUsuario.Rows)
                        {
                            linha.Delete();
                        }

                        dtUsuario.Put(HadesLyc.Config.CreateWritableConnection());
                        retorno = VerificarErro(dtUsuario);

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }

                        retorno = new RetValue(true, "Usuário removido com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }

        //obtém o setor do usuário logado no sistema
        public static string ObterMatriculaUsuario(string usuario)
        {
            string sql = "select MATRICULA from USUARIO where USUARIO = ?";
            return ConsultarCampo(sql, usuario);
        }

        public static string ObterUsuarioPorMatricula(string matricula)
        {
            string sql = "select USUARIO from USUARIO where MATRICULA = ?";
            return ConsultarCampo(sql, matricula);
        }

        public static string ObterUsuarioPorIdVinculo(string idVinculo)
        {
            string sql = "select USUARIO from USUARIO where IDVINCULO = ?";
            return ConsultarCampo(sql, idVinculo);
        }

        /// <summary>
        /// Consultar dados do usuário.
        /// </summary>
        /// <param name="usuario">usuário</param>
        /// <returns>linha com dados do usuário</returns>
        public static Hd_usuario.Row Consultar(string usuario)
        {
            var consulta = Hd_usuario.QueryFirstRow(HadesLyc.Config.CreateWritableConnection(), "usuario = ?", usuario);

            return consulta;
        }

        public static bool UsuarioPrivilegiado(string usuario)
        {
            RN.Usuarios rnUsuarios = new Usuarios();
            return rnUsuarios.EhPrivilegiado(usuario);
        }

        /// <summary>
        /// Valida e-mail do usuário.
        /// </summary>
        /// <param name="usuario">usuário</param>
        /// <param name="email">e-mail</param>
        /// <returns>true se o e-mail estiver correto, false caso contrário</returns>
        public static bool ValidaEmail(string usuario, string email)
        {
            string consulta_pessoa = ConsultaPessoa(usuario);
            if (!string.IsNullOrEmpty(consulta_pessoa))
            {
                decimal pessoa = Convert.ToDecimal(consulta_pessoa);
                string sql = "select E_MAIL_INTERNO from ly_pessoa where pessoa = ?";
                string email_pessoa = ConsultarCampo(sql, pessoa);
                if (!string.IsNullOrEmpty(email_pessoa))
                {
                    string emailUpper = email == null ? string.Empty : email.Trim().ToUpper();
                    string emailPessoaUpper = email_pessoa.Trim().ToUpper();
                    return emailPessoaUpper == emailUpper;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Consulta pessoa do usuário.
        /// </summary>
        /// <param name="usuario">usuário</param>
        /// <returns>pessoa</returns>
        public static string ConsultaPessoa(string usuario)
        {
            string sql = "select pessoa from hd_usuario where usuario = ?";

            return ConsultarCampoHades(sql, usuario);
        }

        public string ObtemCpfPor(string usuario)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT P.CPF
                                    FROM HADES.DBO.HD_USUARIO U (NOLOCK)
	                                    INNER JOIN LY_PESSOA P (NOLOCK) ON U.PESSOA = P.PESSOA
                                    WHERE USUARIO = @USUARIO ";

                contextQuery.Parameters.Add("@USUARIO", usuario);

                resultado = contexto.GetReturnValue<string>(contextQuery);

                return resultado;
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

        public List<string> ListaUsuarioPor(decimal pessoa)
        {
            List<string> lista = new List<string>();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT U.USUARIO
                                    FROM HADES.DBO.HD_USUARIO U (NOLOCK)
                                    WHERE  U.PESSOA = @PESSOA
                                           AND HABILITADO = 'S' ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    lista.Add(Convert.ToString(reader["USUARIO"]));
                }

                return lista;
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }

        /// <summary>
        /// Valida e-mail do usuário.
        /// </summary>
        /// <param name="usuario">usuário</param>
        /// <returns>E-mail do usuário caso exista, caso não exista retorna vazio.</returns>
        public static string BuscaEmail(string usuario)
        {
            var consultaPessoa = ConsultaPessoa(usuario);
            var pessoa = !string.IsNullOrEmpty(consultaPessoa) ? Convert.ToDecimal(consultaPessoa) : 0m;

            if (pessoa == 0)
            {
                return string.Empty;
            }

            return ConsultarCampo("SELECT E_MAIL_INTERNO FROM LY_PESSOA WHERE PESSOA = ?", pessoa);
        }

        public static string BuscaNome(string usuario)
        {
            DataContext ctx = DataContextBuilder.FromHades.UsingNoLock();
            string nome = string.Empty;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT NOME
                        FROM    DBO.HD_USUARIO
                        WHERE   USUARIO = @USUARIO "
                };

                contextQuery.Parameters.Add("@USUARIO", usuario);

                nome = ctx.GetReturnValue<string>(contextQuery);

                return nome;
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        /// <summary>
        /// Consulta dados do servidor da matrícula.
        /// </summary>
        /// <param name="matricula">matrícula</param>
        /// <returns>dados do servidor</returns>
        public static QueryTable ConsultaServidorMatricula(string matricula)
        {
            QueryTable qt = null;
            string sql = @"select top 1 nome_compl, 
	                            idvinculo_matricula, 
	                            idvinculo, 
	                            matricula, 
	                            vf.fone, 
	                            celular, 
	                            e_mail_interno, 
	                            vf.setor, 
	                            vs.UA_ATUAL 
                            from vw_funcionarios vf 
	                            inner join HADES..VW_SETOR vs on vs.SETOR = vf.setor 
                            where matricula = ? ";
            qt = Consultar(sql, matricula);
            return qt;
        }

        public static QueryTable ConsultaServidorIdVinculo(string idVinculo)
        {
            QueryTable qt = null;
            string sql = @"select top 1 nome_compl, 
	                            idvinculo_matricula, 
	                            idvinculo, 
	                            matricula, 
	                            vf.fone, 
	                            celular, 
	                            e_mail_interno, 
	                            vf.setor, 
	                            vs.UA_ATUAL 
                            from vw_funcionarios vf 
	                            inner join HADES..VW_SETOR vs on vs.SETOR = vf.setor 
                            where idvinculo = ? ";
            qt = Consultar(sql, idVinculo);
            return qt;
        }

        /// <summary>
        /// Verifica se usuário está habilitado.
        /// </summary>
        /// <param name="usuario">usuário</param>
        /// <returns>true caso esteja habilitado, false caso contrário</returns>
        public static bool VerificaHabilitado(string usuario)
        {
            string sql = "select habilitado from hd_usuario where usuario = ?";
            if (ConsultarCampoHades(sql, usuario) == "S")
                return true;
            else
                return false;
        }

        /// <summary>
        /// Insere acesso a todas as unidades físicas para o usuário.
        /// </summary>
        /// <param name="usuario">usuário</param>
        /// <returns>sucesso ou erro</returns>
        public static RetValue InserirAcessoTodasUnidadesFisicas(String usuario)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                QueryTable unidades = Consultar(connection, "SELECT UNIDADE_FIS, NOME_COMP FROM Ly_Unidade_Fisica ORDER BY NOME_COMP");
                if (unidades == null || unidades.Rows.Count == 0)
                    return null;

                RetValue remocao = IAE(connection, "DELETE from Ly_usuario_unidade_fis WHERE usuario = ?", usuario);
                if (remocao != null && !remocao.Ok)
                {
                    connection.Rollback();
                    return remocao;
                }

                foreach (SimpleRow row in unidades.Rows)
                {
                    String unidade_fis = (String)row["unidade_fis"];
                    Ly_usuario_unidade_fis.Row.Insert(connection, usuario, unidade_fis);
                    RetValue ret = VerificarErro(connection.GetErrors());
                    if (ret != null && !ret.Ok)
                    {
                        connection.Rollback();
                        return ret;
                    }
                }
                return null;
            }
            catch
            {
                connection.Rollback();
                return VerificarErro(connection.GetErrors());
            }
            finally
            {
                connection.Close();
            }
        }

        public void RemoveAcessoUnidadesFisicasPor(string usuario)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE LY_USUARIO_UNIDADE_FIS
                                    WHERE  USUARIO = @USUARIO ";

                contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuario);

                contexto.ApplyModifications(contextQuery);
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

        /// <summary>
        /// Obtém matrícula do usuário.
        /// </summary>
        /// <param name="user">usuário</param>
        /// <returns>matrícula</returns>
        public static string ObterMatricula(string user)
        {
            TConnection connection = Techne.HadesLyc.Config.CreateConnection();
            var qt = new QueryTable("select matricula from hd_usuario where USUARIO = ? ");
            qt.Query(connection, user);

            if (qt.Rows.Count > 0)
            {
                return Convert.ToString(qt.Rows[0]["matricula"]);
            }

            return string.Empty;
        }

        public void AlteraSenhaUsuario(string usuario, string senha)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery(
                        @" UPDATE HADES..HD_USUARIO
                                    SET SENHA = DBO.CRYPT(@SENHA),
	                                    DATA_ALTERACAO_SENHA = GETDATE(),
	                                    ALTERAR_SENHA = 'S'
                                    WHERE USUARIO = @USUARIO ");

                    contextQuery.Parameters.Add("@USUARIO", usuario);
                    contextQuery.Parameters.Add("@SENHA", senha);

                    if (RN.AcessoUsuario.VerificarDadosUltimoReset(ctx, usuario))
                    {
                        RN.AcessoUsuario.AlterarDadosUltimoResetAcesso(ctx, usuario);
                    }
                    else
                    {
                        RN.AcessoUsuario.InserirDadosUltimoResetAcesso(ctx, usuario);
                    }

                    ExecutarAlteracao(contextQuery);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static RetValue InserirAcessoUnidadeFisica(TConnectionWritable connection, String usuario, String unidadeFis)
        {
            RetValue retorno = null;

            Ly_usuario_unidade_fis.Row.Insert(connection, usuario, unidadeFis);
            retorno = VerificarErro(connection.GetErrors());
            if (retorno != null && !retorno.Ok)
            {
                return retorno;
            }

            retorno = new RetValue(true, "Unidade Física incluída com sucesso.", null);

            return retorno;
        }

        /// <summary>
        /// Insere acesso a todas as unidades físicas de uma coordenadoria para o usuário.
        /// </summary>
        /// <param name="usuario">usuário</param>
        /// <returns>sucesso ou erro</returns>
        public static RetValue InserirAcessoTodasUnidadesFisicas(string regional, string usuario)
        {
            TConnectionWritable cn = Config.CreateWritableConnection();
            RetValue retorno = null;
            cn.Open(true);

            try
            {
                string cmd = @"insert into Ly_usuario_unidade_fis (USUARIO, UNIDADE_FIS)
                            select ?, ua.unidade_fis from LY_UNIDADES_ASSOCIADAS ua 
                            join LY_UNIDADE_ENSINO ue on ue.UNIDADE_ENS = ua.UNIDADE_ENS
                            where SIT_FUNCIONAMENTO = 'EmAtividade' AND ue.ID_REGIONAL = ?
                            and NOT exists (select * from Ly_usuario_unidade_fis uuf where uuf.UNIDADE_FIS = ua.UNIDADE_FIS and USUARIO = ?)";
                retorno = IAE(cn, cmd, usuario, regional, usuario);
                return retorno;
            }
            catch (Exception ex)
            {
                cn.Rollback();
                return new RetValue(false, "", new ErrorList(ex.Message));
            }
            finally
            {
                cn.Close();
            }
        }

        /// <summary>
        /// Remove acesso a todas as unidades físicas de uma coordenadoria para o usuário.
        /// </summary>
        /// <param name="usuario">usuário</param>
        /// <returns>sucesso ou erro</returns>
        public static RetValue RemoverAcessoUnidadesFisicas(string regional, string usuario)
        {
            TConnectionWritable cn = Config.CreateWritableConnection();
            RetValue retorno = null;
            cn.Open(true);

            try
            {
                string cmd = @"delete Ly_usuario_unidade_fis where USUARIO = ? 
                            and UNIDADE_FIS in 
                            (select ua.unidade_fis from LY_UNIDADES_ASSOCIADAS ua 
                            join LY_UNIDADE_ENSINO ue on ue.UNIDADE_ENS = ua.UNIDADE_ENS
                            where ue.ID_REGIONAL = ?)";
                retorno = IAE(cn, cmd, usuario, regional);
                return retorno;
            }
            catch (Exception ex)
            {
                cn.Rollback();
                return new RetValue(false, "", new ErrorList(ex.Message));
            }
            finally
            {
                cn.Close();
            }
        }

        /// <summary>
        /// Consulta unidade de ensino da lotação do usuário.
        /// </summary>
        /// <param name="matricula">matrícula do usuário</param>
        /// <returns>código da unidade de ensino</returns>
        public static string ConsultaUnidadeEnsino(string matricula)
        {
            string sql = @"SELECT top 1 unidade_ens from ly_lotacao WHERE matricula = ? 
            and (data_desativacao is null
            OR convert(date,data_desativacao) > convert(date,GetDate()))";
            return ConsultarCampo(sql, matricula);
        }

        /// <summary>
        /// Verifica se unidade de ensino possui alunos duplicados em turmas diferentes.
        /// </summary>
        /// <param name="unidade_ens">unidade de ensino</param>
        /// <returns>true se existe, false caso contrário</returns>
        public static bool VerificaUnidadePossuiDuplicados(string unidade_ens)
        {
            string sql = @"select top 1 1 from ly_matricula m inner join LY_ALUNO a
                on a.ALUNO = m.ALUNO INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA
                where m.SIT_MATRICULA = 'Matriculado' and a.UNIDADE_ENSINO = ?
                group by m.aluno, m.DISCIPLINA, m.ANO, m.SEMESTRE, PE.NOME_COMPL
                having COUNT(1) > 1";
            int retorno = ExecutarFuncao(sql, unidade_ens);
            if (retorno == 1)
                return true;
            else
                return false;
        }

        public static HdUsuario Carregar(string usuario)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromHades.UsingNoLock())
                {
                    var contextQuery = new ContextQuery("SELECT * FROM hd_usuario WHERE USUARIO = @USUARIO ");
                    contextQuery.Parameters.Add("@USUARIO", usuario);

                    return ctx.TryToBindEntity<HdUsuario>(contextQuery);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static ICollection<HdUsuario> Listar()
        {
            using (var ctx = DataContextBuilder.FromHades.UsingNoLock())
            {
                var contextQuery = new ContextQuery("SELECT * FROM hd_usuario WHERE USUARIO = @USUARIO ");

                return ctx.TryToBindEntities<HdUsuario>(contextQuery);
            }
        }

        public static void Inserir(HdUsuario usuario)
        {
            using (var ctx = DataContextBuilder.FromHades.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    (
                        @"INSERT INTO dbo.hd_usuario
                                               (
                                                USUARIO,                                                
                                                NOME,                                                
                                                SENHA,
                                                PRIVILEGIADO,
                                                EMAIL,
                                                MATRICULA,
                                                IDVINCULO,
                                                SETOR,
                                                HABILITADO,
                                                MOTIVO_DESABILITADO,
                                                PESSOA,
                                                PRIV_UNIDADE_ENS,
                                                GRUPOUSU
                                                )
                                        VALUES  (
                                                @USUARIO,                                                
                                                @NOME,                                                
                                                @SENHA,
                                                @PRIVILEGIADO,
                                                @EMAIL,
                                                @MATRICULA,
                                                @IDVINCULO,
                                                @SETOR,
                                                @HABILITADO,
                                                @MOTIVO_DESABILITADO,
                                                @PESSOA,
                                                @PRIV_UNIDADE_ENS,
                                                @GRUPOUSU
                                                )");

                    contextQuery.Parameters.Add("@USUARIO", usuario.Usuario);
                    contextQuery.Parameters.Add("@NOME", usuario.Nome);
                    contextQuery.Parameters.Add("@SENHA", usuario.Senha);
                    contextQuery.Parameters.Add("@PRIVILEGIADO", usuario.Privilegiado);
                    contextQuery.Parameters.Add("@EMAIL", usuario.Email);
                    contextQuery.Parameters.Add("@MATRICULA", usuario.Matricula);
                    contextQuery.Parameters.Add("@IDVINCULO", usuario.IdVinculo);
                    contextQuery.Parameters.Add("@SETOR", usuario.Setor);
                    contextQuery.Parameters.Add("@HABILITADO", usuario.Habilitado);
                    contextQuery.Parameters.Add("@MOTIVO_DESABILITADO", usuario.MotivoDesabilitado);
                    contextQuery.Parameters.Add("@PESSOA", usuario.Pessoa);
                    contextQuery.Parameters.Add("@PRIV_UNIDADE_ENS", usuario.PrivUnidadeEns);
                    contextQuery.Parameters.Add("@GRUPOUSU", usuario.Grupousu); //Alimentar apenas quando for usuario externo, com valor que exista no RN.RecursosHumanos.UsuarioExterno.TiposUsuarioExterno

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static void Alterar(HdUsuario usuario)
        {
            using (var ctx = DataContextBuilder.FromHades.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"UPDATE  dbo.hd_usuario
                                        SET
                                            PRIVILEGIADO = @PRIVILEGIADO,
                                            EMAIL = @EMAIL,                                           
                                            SETOR = @SETOR,
                                            HABILITADO = @HABILITADO,
                                            MOTIVO_DESABILITADO = @MOTIVO_DESABILITADO,
                                            PRIV_UNIDADE_ENS = @PRIV_UNIDADE_ENS
                                        WHERE USUARIO = @USUARIO"
                    };

                    contextQuery.Parameters.Add("@USUARIO", usuario.Usuario);
                    contextQuery.Parameters.Add("@PRIVILEGIADO", usuario.Privilegiado);
                    contextQuery.Parameters.Add("@EMAIL", usuario.Email);
                    contextQuery.Parameters.Add("@SETOR", usuario.Setor);
                    contextQuery.Parameters.Add("@HABILITADO", usuario.Habilitado);
                    contextQuery.Parameters.Add("@MOTIVO_DESABILITADO", usuario.MotivoDesabilitado);
                    contextQuery.Parameters.Add("@PRIV_UNIDADE_ENS", usuario.PrivUnidadeEns);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public void DesativaUsuarioExterno(DataContext ctx, decimal pessoa, int grupoUsuario)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE HADES..HD_USUARIO
		                           SET HABILITADO = 'N',
				                        MOTIVO_DESABILITADO = @MOTIVO_DESABILITADO
		                           WHERE PESSOA = @PESSOA
				                        AND GRUPOUSU = @GRUPOUSU ";

            contextQuery.Parameters.Add("@MOTIVO_DESABILITADO", "Recurso Externo Desativo");
            contextQuery.Parameters.Add("@PESSOA", pessoa);
            contextQuery.Parameters.Add("@GRUPOUSU", grupoUsuario.ToString());

            ctx.ApplyModifications(contextQuery);
        }

        public static void Remover(string usuario)
        {
            if (string.IsNullOrEmpty(usuario))
            {
                return;
            }

            using (var ctx = DataContextBuilder.FromHades.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = "DELETE FROM hd_usuario WHERE WHERE USUARIO = @USUARIO"
                    };

                    contextQuery.Parameters.Add("@USUARIO", usuario);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static DataTable ListarUsuariosPorUE(string usuario)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT e.UNIDADE_ENS,e.NOME_COMP
                                           From USUARIO U  

                                                              JOIN LY_USUARIO_UNIDADE_FIS USUUNI  
                                                                   ON U.USUARIO = USUUNI.USUARIO   
                                                              JOIN LY_UNIDADES_ASSOCIADAS UA
                                                                   ON USUUNI.UNIDADE_FIS = UA.UNIDADE_FIS
                                                              JOIN LY_UNIDADE_ENSINO E  
                                                                   ON UA.UNIDADE_ENS = E.UNIDADE_ENS  
                                           WHERE  U.USUARIO=@USUARIO  
                                           GROUP BY e.UNIDADE_ENS,e.NOME_COMP";

                contextQuery.Parameters.Add("@USUARIO", usuario);

                return ctx.GetDataTable(contextQuery);
            }
        }

        /// <summary>
        /// Verifica a permissão do usuário logado para alteração das informações de Educação Profissional Concomitante
        /// </summary>
        /// <param name="unidadeFis"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public static bool VerificaAcesso(string unidadeFis, string usuario)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                //busca permissão para o censo e usuário informados
                var contextQuery = new ContextQuery(
                    @"SELECT  COUNT(1)
                        FROM    dbo.USUARIO u
                        WHERE   u.USUARIO = @USUARIO
                                AND ( u.PRIVIL = 'S'
                                      OR u.USUARIO IN (
                                      SELECT DISTINCT
                                                USUARIO
                                      FROM      dbo.LY_USUARIO_UNIDADE_FIS uf
                                                INNER JOIN LY_UNIDADES_ASSOCIADAS a ON uf.UNIDADE_FIS = a.UNIDADE_FIS
                                      WHERE     u.USUARIO = uf.USUARIO
                                                AND a.UNIDADE_ENS = @UNIDADE_FIS )
                                    )");

                contextQuery.Parameters.Add("@UNIDADE_FIS", unidadeFis);
                contextQuery.Parameters.Add("@USUARIO", usuario);

                var retorno = ctx.GetReturnValue<int>(contextQuery);

                if (retorno > 0)
                {
                    return true;
                }

                return false;
            }
        }

        public bool PossuiUsuarioCadastrado(string usuario)
        {
            using (var ctx = DataContextBuilder.FromHades.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @"SELECT    *
                              FROM      HD_USUARIO
                              WHERE     USUARIO = @USUARIO"
                };
                contextQuery.Parameters.Add("@USUARIO", usuario);

                object obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    return true;
                }
            }
            return false;
        }

        public static int RetornarRegionalUsuario(string usuario)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(@"SELECT DISTINCT RG.POLO as ID_REGIONAL
													FROM USUARIO USU
													INNER JOIN LY_LOTACAO LT ON USU.MATRICULA = LT.MATRICULA
													INNER JOIN UNIDADEADMINISTRATIVA_REGIONAL UR ON UR.SETOR = LT.SETOR
													INNER JOIN REGIONAL RG ON RG.POLO = UR.ID_REGIONAL
													WHERE USU.USUARIO = @USUARIO AND
													(lt.DATA_DESATIVACAO IS NULL OR CONVERT(DATE, lt.DATA_DESATIVACAO) > CONVERT(DATE,GETDATE()))");

                contextQuery.Parameters.Add("@USUARIO", usuario);

                return ExecutarFuncao(contextQuery);
            }
        }

        public bool EhPrivilegiado(DataContext ctx, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool privilegiado = false;

            contextQuery.Command = @"SELECT COUNT(*) FROM HADES..HD_USUARIO WHERE USUARIO = @USUARIO AND PRIVILEGIADO = 'S'";

            contextQuery.Parameters.Add("@USUARIO", usuario);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                privilegiado = true;
            }

            return privilegiado;
        }

        public bool EhPrivilegiado(string usuario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool privilegiado = false;

            try
            {
                privilegiado = this.EhPrivilegiado(ctx, usuario);
                return privilegiado;
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
                ctx.Dispose();
            }
        }

        public bool PossuiPermissaoPaginaPor(string usuario, string pagina)
        { 
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)
                                            FROM HADES.DBO.HD_PADTRANS PT (NOLOCK)
	                                            INNER JOIN HADES.DBO.PADUSUARIO PU (NOLOCK) ON PT.PADACES = PU.PADACES
                                            WHERE TRANS = @TRANS
	                                            AND USUARIO = @USUARIO ";

                contextQuery.Parameters.Add("@TRANS", SqlDbType.VarChar, pagina);
                contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuario); 

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
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
        public bool EhUsuarioRegional(string usuario)
        {
            DataContext ctx = DataContextBuilder.FromHades.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool regional = false;

            try
            {
                contextQuery.Command = @"SELECT COUNT(*) FROM HD_PADUSUARIO WHERE USUARIO = @USUARIO AND PADACES = 'COORDENADORIA'";

                contextQuery.Parameters.Add("@USUARIO", usuario);


                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    regional = true;
                }

                return regional;
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
                ctx.Dispose();
            }
        }

        public DataTable ObtemDadosUsuario(string usuario)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" select DISTINCT	F.IDVINCULO,F.NOME_COMPL	                   
                                            FROM HADES..HD_USUARIO U
	                                            INNER JOIN LYCEUM..VW_FUNCIONARIOS F ON U.PESSOA = F.PESSOA
                                            WHERE USUARIO = @USUARIO ";

                contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuario);

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Empty;
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

            return dt;
        }
    }
}