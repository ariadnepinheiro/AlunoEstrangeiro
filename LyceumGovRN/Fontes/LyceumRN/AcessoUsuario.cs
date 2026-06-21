namespace Techne.Lyceum.RN
{
    using System;
    using System.Data;
    using System.Web.Security;
    using Seeduc.Infra.Data;
    using Techne.Data;
    using Techne.Library;

    public class AcessoUsuario : RNBase
    {
        public AcessoUsuario(string usuario, string senha)
        {
            this.Usuario = usuario;
            this.Senha = senha;
            this.Valido = false;
            this.AlterarSenha = false;
        }

        public AcessoUsuario(string usuario)
        {
            this.Usuario = usuario;
            this.Senha = string.Empty;
            this.Valido = false;
            this.AlterarSenha = false;
        }

        public AcessoUsuario(string usuario, string senha, string novaSenha)
        {
            this.Usuario = usuario;
            this.Senha = senha;
            this.NovaSenha = novaSenha;
            this.Valido = false;
            this.AlterarSenha = false;
        }

        public bool AlterarSenha { get; set; }

        public string NovaSenha { get; set; }

        public string Senha { get; set; }

        public string Usuario { get; set; }

        public bool Valido { get; set; }

        public static RetValue AtualizaUltimoAcesso(string sistema, string usuario)
        {
            if (string.IsNullOrEmpty(sistema))
            {
                return new RetValue(false, string.Empty, new ErrorList("Sistema não informado."));
            }

            if (string.IsNullOrEmpty(usuario))
            {
                return new RetValue(false, string.Empty, new ErrorList("Usuário não informado."));
            }

            try
            {
                var contextQuery = new ContextQuery(
                    @"INSERT  INTO LY_LOGIN
                            ( SISTEMA, USUARIO, DT_ACESSO )
                    VALUES  ( @SISTEMA, @USUARIO, @DT_ACESSO )");

                contextQuery.Parameters.Add("@SISTEMA", sistema);
                contextQuery.Parameters.Add("@USUARIO", usuario);
                contextQuery.Parameters.Add("@DT_ACESSO", SqlDbType.DateTime, DateTime.Now);

                ExecutarAlteracao(contextQuery);

                return new RetValue(true, "Registro inserido com sucesso.", null);
            }
            catch (Exception e)
            {
                return new RetValue(false, null, new ErrorList(e.Message));
            }
        }

        public static RetValue AtualizaUltimoAcessoAlunoOnline(string sistema, string usuario)
        {
            //Monta string de conexão para outro banco
            string stringConexao = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["StringConexao"].ConnectionString;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLockWithConnectionString(stringConexao);
            ContextQuery contextQuery = new ContextQuery();

            if (string.IsNullOrEmpty(sistema))
            {
                return new RetValue(false, string.Empty, new ErrorList("Sistema não informado."));
            }

            if (string.IsNullOrEmpty(usuario))
            {
                return new RetValue(false, string.Empty, new ErrorList("Usuário não informado."));
            }

            try
            {
                contextQuery.Command = @"INSERT  INTO LY_LOGIN
                            ( SISTEMA, USUARIO, DT_ACESSO )
                    VALUES  ( @SISTEMA, @USUARIO, GETDATE() )";

                contextQuery.Parameters.Add("@SISTEMA", sistema);
                contextQuery.Parameters.Add("@USUARIO", usuario);
                //contextQuery.Parameters.Add("@DT_ACESSO", SqlDbType.DateTime, DateTime.Now);

                ctx.ApplyModifications(contextQuery);

                return new RetValue(true, "Registro inserido com sucesso.", null);
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
                return new RetValue(false, null, new ErrorList(mensagem));
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public static DateTime? BuscaUltimoAcesso(string sistema, string usuario)
        {
            if (string.IsNullOrEmpty(sistema)
                || string.IsNullOrEmpty(usuario))
            {
                return null;
            }

            var dataAcesso = ExecutarFuncaoScalar("SELECT TOP 1 dt_acesso FROM ly_login (NOLOCK) WHERE sistema = ? AND usuario = ? ORDER BY dt_acesso DESC", sistema, usuario, DateTime.Now);

            if (!dataAcesso.IsNull)
            {
                return Convert.ToDateTime(dataAcesso);
            }

            return null;
        }

        public static void CarregaConexoes()
        {
            ConnectionList.Current = new HadesConnectionList();
            ConnectionList.Load();
        }
       

        public void AlteraSenhaDocente()
        {
            var contextQuery = new ContextQuery(
                @"UPDATE  LY_DOCENTE
                SET     SENHA_DOL = @SENHA,
                        SENHA_ALTERADA = 'S'
                WHERE   MATRICULA = @MATRICULA");

            contextQuery.Parameters.Add("@SENHA", FormsAuthentication.HashPasswordForStoringInConfigFile(this.NovaSenha, "SHA1"));
            contextQuery.Parameters.Add("@MATRICULA", this.Usuario);

            ExecutarAlteracao(contextQuery);
        }

        public void LoginDocente()
        {
            var contextQuery = new ContextQuery(
                @"SELECT TOP 1
                        SENHA_ALTERADA,
                        SENHA_DOL
                FROM    LY_DOCENTE
                WHERE   MATRICULA = @MATRICULA");

            contextQuery.Parameters.Add("@MATRICULA", this.Usuario);

            var dataTable = Consultar(contextQuery);
            var senhaCriptograda = FormsAuthentication.HashPasswordForStoringInConfigFile(this.Senha, "SHA1");

            this.Valido = false;

            if (dataTable.Rows.Count == 1)
            {


                if (dataTable.Rows[0]["senha_dol"].ToString() == senhaCriptograda || dataTable.Rows[0]["senha_dol"].ToString() == this.Senha)
                {
                    this.Valido = true;
                }

                if (dataTable.Rows[0]["senha_alterada"].ToString() == "S")
                {
                    this.AlterarSenha = true;
                }
            }
        }

        public void LoginProcessoSeletivo()
        {
            var contextQuery = new ContextQuery(
                @"SELECT  USUSENHA
                FROM    LY_CANDIDATO_DOCENTE
                WHERE   USULOGIN = @LOGIN");

            contextQuery.Parameters.Add("@LOGIN", this.Usuario);

            var dataTable = Consultar(contextQuery);
            var senhaCriptograda = HdPal(this.Senha);

            if (dataTable.Rows.Count > 0)
            {
                for (var i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (dataTable.Rows[i]["ususenha"].ToString().Length > 15)
                    {
                        if (dataTable.Rows[i]["ususenha"].ToString() == senhaCriptograda)
                        {
                            this.AlterarSenha = false;
                            this.Valido = true;

                            return;
                        }
                    }
                    else
                    {
                        if (dataTable.Rows[i]["ususenha"].ToString() == this.Senha)
                        {
                            this.AlterarSenha = false;
                            this.Valido = true;

                            return;
                        }
                    }
                }

                this.Valido = false;
            }
        }

        public string ResetarSenhaAluno()
        {
            try
            {
                var contextQuery = new ContextQuery(
                    @"UPDATE  LY_PESSOA
                    SET     SENHA_TAC = NULL,
                            SENHA_ALTERADA = 'N'
                    WHERE   PESSOA = (
                                       SELECT TOP 1
                                                PESSOA
                                       FROM     LY_ALUNO
                                       WHERE    ALUNO = @ALUNO
                                     )");

                contextQuery.Parameters.Add("@ALUNO", this.Usuario);

                ExecutarAlteracao(contextQuery);

                return "Senha resetada com sucesso.";
            }
            catch (Exception)
            {
                return "Não foi possível resetar a senha.";
            }
        }

        public static void ResetarSenhaDocente(string novasenha, string numFunc, string matricula)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery(
                        @"UPDATE  LY_DOCENTE
                            SET     SENHA_DOL = @SENHA,
                                    SENHA_ALTERADA = 'S'
                            FROM LY_DOCENTE D 
                            INNER JOIN LY_PESSOA P ON P.PESSOA = D.PESSOA
                            WHERE D.NUM_FUNC = @NUM_FUNC");

                    contextQuery.Parameters.Add("@SENHA", FormsAuthentication.HashPasswordForStoringInConfigFile(novasenha, "SHA1"));
                    contextQuery.Parameters.Add("@NUM_FUNC", numFunc);

                    ctx.ApplyModifications(contextQuery);                 

                    if (VerificarDadosUltimoReset(ctx, matricula))
                    {
                        AlterarDadosUltimoResetAcesso(ctx, matricula);
                    }
                    else
                    {
                        InserirDadosUltimoResetAcesso(ctx, matricula);
                    }                       
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static DataTable CarregarDadosUltimoResetAcesso(String matricula)
        {

            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" select max(dt_acesso) as 'ULTIMO_ACESSO', 
                        (select DT_ULTIMO_RESET from TCE_ULTIMO_RESET
                        where MATRICULA = @MATRICULA) as 'ULTIMO_RESET'
                        from LY_LOGIN
                        where USUARIO = @MATRICULA "
                    };
                    contextQuery.Parameters.Add("@MATRICULA", matricula);
                    return ctx.GetDataTable(contextQuery);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool VerificarDadosUltimoReset(DataContext ctx, String matricula)
        {
            try
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @" select 1 
                        from TCE_ULTIMO_RESET
                        where MATRICULA = @MATRICULA ";

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                object obj = ctx.GetReturnValue(contextQuery);

                if (obj == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                ctx.Abandon();
                throw e;
            }

        }

        public static void InserirDadosUltimoResetAcesso(DataContext ctx, String matricula)
        {

            try
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" Insert into dbo.TCE_ULTIMO_RESET
                            (matricula)
                            values
                            (@matricula)"
                };

                contextQuery.Parameters.Add("@matricula", matricula);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception e)
            {
                ctx.Abandon();
                throw e;
            }

        }

        public static void AlterarDadosUltimoResetAcesso(DataContext ctx, String matricula)
        {

            try
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"  UPDATE TCE_ULTIMO_RESET 
                          SET DT_ULTIMO_RESET = GetDate()
                          WHERE MATRICULA = @MATRICULA "
                };
                contextQuery.Parameters.Add("@MATRICULA", matricula);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception e)
            {
                ctx.Abandon();
                throw e;
            }
        }

    }
}