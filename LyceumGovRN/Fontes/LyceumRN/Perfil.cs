using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    public class Perfil : RNBase
    {
        public static DataTable Listar()
        {
            using (var ctx = DataContextBuilder.FromHades.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT  *
                            FROM    dbo.TCE_PERFIL "
                };

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static ValidacaoDados Validar(TcePerfil perfil)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (perfil == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(perfil.Descricao)
                || (!string.IsNullOrEmpty(perfil.Descricao)
                    && perfil.Descricao.Length > 500))
            {
                mensagens.Add("O campo DESCRIÇÃO é obrigatório com o máximo de 500 caracteres!");
            }

            if (string.IsNullOrEmpty(perfil.Matricula)
                || (!string.IsNullOrEmpty(perfil.Matricula)
                    && perfil.Matricula.Length > 20))
            {
                mensagens.Add("O campo MATRICULA DO RESPONSÁVEL é obrigatório com o máximo de 20 caracteres!");
            }

            if (mensagens.Count == 0)
            {
                using (var ctx = DataContextBuilder.FromHades.ToFastReadingOnly())
                {
                    //Verifica se existe outro perfil com a aquela mesma descrição
                    var contextQuery = new ContextQuery
                           {
                               Command =
                                   @" SELECT  1
                                        FROM    dbo.TCE_PERFIL
                                        WHERE   DESCRICAO = @DESCRICAO "
                           };

                    if (perfil.IdPerfil > 0)
                    {
                        contextQuery.Command += " AND ID_PERFIL <> @ID_PERFIL ";
                    }

                    contextQuery.Parameters.Add("@DESCRICAO", perfil.Descricao);

                    if (perfil.IdPerfil > 0)
                    {
                        contextQuery.Parameters.Add("@ID_PERFIL", perfil.IdPerfil);
                    }

                    var obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("DESCRIÇÃO já cadastrada anteriormente.");
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public static void Inserir(TcePerfil perfil)
        {
            using (var ctx = DataContextBuilder.FromHades.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" INSERT INTO dbo.TCE_PERFIL
                                    ( DESCRICAO ,
                                      MATRICULA
                                    )
                            VALUES  (@DESCRICAO ,
                                     @MATRICULA 
                                    ) "
                    };

                    contextQuery.Parameters.Add("@DESCRICAO", perfil.Descricao);
                    contextQuery.Parameters.Add("@MATRICULA", perfil.Matricula);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static void Alterar(TcePerfil perfil)
        {
            using (var ctx = DataContextBuilder.FromHades.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" UPDATE  dbo.TCE_PERFIL
                            SET     DESCRICAO = @DESCRICAO ,
                                    MATRICULA = @MATRICULA ,
                                    DT_ALTERACAO = GETDATE()
                            WHERE   ID_PERFIL = @ID_PERFIL "
                    };

                    contextQuery.Parameters.Add("@DESCRICAO", perfil.Descricao);
                    contextQuery.Parameters.Add("@MATRICULA", perfil.Matricula);
                    contextQuery.Parameters.Add("@ID_PERFIL", perfil.IdPerfil);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static ValidacaoDados ValidarRemover(int id)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (id <= 0)
            {
                mensagens.Add("O campo ID é obrigatório!");
            }

            if (mensagens.Count == 0)
            {
                using (var ctx = DataContextBuilder.FromHades.ToFastReadingOnly())
                {
                    //Verifica se o perfil já foi usado com um padrao de acesso
                    var contextQuery = new ContextQuery(
                        @" SELECT  1
                            FROM    dbo.TCE_PADACES_PERFIL
                            WHERE  ID_PERFIL = @ID_PERFIL ");

                    contextQuery.Parameters.Add("@ID_PERFIL", id);

                    var obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("Não é possivel excluir este perfil pois ele já se encontra associado a um padrao de acesso.");
                    }

                    //Verifica se o perfil já foi usado com um componente
                    contextQuery = new ContextQuery(
                        @" SELECT  1
                            FROM    dbo.TCE_PERFIL_COMPONENTE
                            WHERE   ID_PERFIL = @ID_PERFIL ");

                    contextQuery.Parameters.Add("@ID_PERFIL", id);

                    obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("Não é possivel excluir este perfil pois já existe um componente associado.");
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public static void Remover(int id)
        {
            if (id < 1)
            {
                return;
            }

            using (var ctx = DataContextBuilder.FromHades.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" DELETE  dbo.TCE_PERFIL
                            WHERE   ID_PERFIL = @ID "
                    };

                    contextQuery.Parameters.Add("@ID", id);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static DataTable ListarPerfil(string usuario)
        {
            using (var ctx = DataContextBuilder.FromHades.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT
                                    PU.PADACES ,
                                    P.DESCRICAO AS PERFIL ,
                                    PA.NOME AS PADRAO ,
                                    P.ID_PERFIL ,
                                    U.PRIVILEGIADO
                                FROM
                                    HADES.DBO.HD_PADUSUARIO PU INNER JOIN HADES.DBO.HD_PADACES PA
                                        ON PA.PADACES = PU.PADACES INNER JOIN HADES.DBO.TCE_PADACES_PERFIL PP
                                        ON PP.PADACES = PA.PADACES INNER JOIN HADES.DBO.TCE_PERFIL P
                                        ON P.ID_PERFIL = PP.ID_PERFIL INNER JOIN HADES.DBO.HD_USUARIO U
                                        ON U.USUARIO = PU.USUARIO
                                WHERE
                                    PU.USUARIO = @USUARIO"
                };
                contextQuery.Parameters.Add("@USUARIO", usuario);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarPerfisPorUsuarioETransacao(string usuario, string transacao)
        {
            using (var ctx = DataContextBuilder.FromHades.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT
                                    PU.PADACES, P.DESCRICAO AS PERFIL, PA.NOME AS PADRAO, P.ID_PERFIL, U.PRIVILEGIADO,
                                    PT.PODECAD, PT.PODEALT, PT.PODEREM
                                FROM
	                                HADES.DBO.HD_PADUSUARIO PU 
                                    LEFT JOIN HADES.DBO.HD_PADACES PA ON 
    	                                PA.PADACES = PU.PADACES 
                                    LEFT JOIN HADES.DBO.HD_PADTRANS PT ON
    	                                PT.PADACES = PA.PADACES                                        
                                        AND PT.TRANS = @TRANS
	                                LEFT JOIN HADES.DBO.TCE_PADACES_PERFIL PP ON 
		                                PP.PADACES = PA.PADACES 
	                                LEFT JOIN HADES.DBO.TCE_PERFIL P ON 
		                                P.ID_PERFIL = PP.ID_PERFIL 
	                                LEFT JOIN HADES.DBO.HD_USUARIO U
                                        ON U.USUARIO = PU.USUARIO
                                WHERE
                                    PU.USUARIO = @USUARIO "
                };

                contextQuery.Parameters.Add("@USUARIO", usuario);
                contextQuery.Parameters.Add("@TRANS", transacao);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarComponentesDoPerfil(string usuario, string tela)
        {
            using (var ctx = DataContextBuilder.FromHades.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT DISTINCT PC.COMPONENTE, 
			                                PU.USUARIO,
                                            P.DESCRICAO AS PERFIL,
                                            P.ID_PERFIL,
                                            PC.HABILITAR
                                FROM HD_PADUSUARIO PU
                                            INNER JOIN DBO.HD_PADACES PA 
				                                ON PA.PADACES =  PU.PADACES
                                            LEFT JOIN dbo.TCE_PADACES_PERFIL PP 
				                                ON PP.PADACES=PA.PADACES
                                            LEFT JOIN TCE_PERFIL P 
				                                ON P.ID_PERFIL = PP.ID_PERFIL
                                            RIGHT JOIN TCE_PERFIL_COMPONENTE PC 
                                            on PC.ID_PERFIL = P.ID_PERFIL
                                WHERE PU.USUARIO = @USUARIO and PC.TELA =@TELA "
                };
                contextQuery.Parameters.Add("@USUARIO", usuario);
                contextQuery.Parameters.Add("@TELA", tela);


                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarPerfis(string usuario, string tela)
        {
            using (var ctx = DataContextBuilder.FromHades.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT 
                                P.DESCRICAO AS PERFIL,
                                P.ID_PERFIL
                                FROM HD_PADUSUARIO PU
                                            INNER JOIN DBO.HD_PADACES PA 
				                                ON PA.PADACES =  PU.PADACES
                                            LEFT JOIN dbo.TCE_PADACES_PERFIL PP 
				                                ON PP.PADACES=PA.PADACES
                                            LEFT JOIN TCE_PERFIL P 
				                                ON P.ID_PERFIL = PP.ID_PERFIL
                                            LEFT JOIN TCE_PERFIL_COMPONENTE PC 
                                            on PC.ID_PERFIL = P.ID_PERFIL
                                WHERE PU.USUARIO = @USUARIO 
                                    AND P.ID_PERFIL IS NOT NULL
                                group by 
                                P.ID_PERFIL,
                                P.DESCRICAO "

                };
                contextQuery.Parameters.Add("@USUARIO", usuario);


                return ctx.GetDataTable(contextQuery);
            }
        }

        public List<string> ListaDescricoesPerfisPor(string usuario)
        {
            List<string> lista = new List<string>();
            string perfil = string.Empty;
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT
	                                    DISTINCT P.DESCRICAO AS PERFIL
                                    FROM
                                        HADES.DBO.HD_PADUSUARIO PU INNER JOIN HADES.DBO.HD_PADACES PA
                                            ON PA.PADACES = PU.PADACES INNER JOIN HADES.DBO.TCE_PADACES_PERFIL PP
                                            ON PP.PADACES = PA.PADACES INNER JOIN HADES.DBO.TCE_PERFIL P
                                            ON P.ID_PERFIL = PP.ID_PERFIL INNER JOIN HADES.DBO.HD_USUARIO U
                                            ON U.USUARIO = PU.USUARIO
                                    WHERE
                                        PU.USUARIO = @USUARIO ";

                contextQuery.Parameters.Add("@USUARIO", usuario);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    perfil = Convert.ToString(reader["PERFIL"]).ToUpper();
                    lista.Add(perfil);
                }

                return lista;
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

        public static TcePerfil RetornaPerfilPor(string strUsuario)
        {
            TcePerfil objTcePerfil = null;
            SqlDataReader reader = null;
            var ctx = DataContextBuilder.FromHades.UsingNoLock();

            try
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" 
                            SELECT PF.ID_PERFIL ID_PERFIL, PF.DESCRICAO DESCRICAO
                            FROM HADES.dbo.TCE_PERFIL PF
                            INNER JOIN HADES.dbo.TCE_PADACES_PERFIL PP
                            ON (PF.ID_PERFIL = PP.ID_PERFIL)
                            INNER JOIN HADES.dbo.HD_PADUSUARIO PU
                            ON (PP.PADACES = PU.PADACES)
                            WHERE PU.USUARIO = @USUARIO "
                };

                contextQuery.Parameters.Add("@USUARIO", strUsuario);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    objTcePerfil = new TcePerfil
                    {
                        IdPerfil = !String.IsNullOrEmpty(reader["ID_PERFIL"].ToString()) ? Convert.ToInt32(reader["ID_PERFIL"]) : default(int),
                        Descricao = !String.IsNullOrEmpty(reader["DESCRICAO"].ToString()) ? Convert.ToString(reader["DESCRICAO"]) : default(string),
                    };
                }

            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
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

            return objTcePerfil;
        }

        public DataTable ObtemListaPerfilPor(string usuario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"SELECT
                                    PU.PADACES ,
                                    P.DESCRICAO AS PERFIL ,
                                    PA.NOME AS PADRAO ,
                                    P.ID_PERFIL ,
                                    U.PRIVILEGIADO
                                FROM
                                    HADES.DBO.HD_PADUSUARIO PU INNER JOIN HADES.DBO.HD_PADACES PA
                                        ON PA.PADACES = PU.PADACES INNER JOIN HADES.DBO.TCE_PADACES_PERFIL PP
                                        ON PP.PADACES = PA.PADACES INNER JOIN HADES.DBO.TCE_PERFIL P
                                        ON P.ID_PERFIL = PP.ID_PERFIL INNER JOIN HADES.DBO.HD_USUARIO U
                                        ON U.USUARIO = PU.USUARIO
                                WHERE
                                    PU.USUARIO = @USUARIO";

                contextQuery.Parameters.Add("@USUARIO", usuario);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;
        }

        public bool PossuiPerfilAlteraMaximoAlunoPor(string usuario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)                                        
                                          FROM
                                                HADES.DBO.HD_PADUSUARIO PU INNER JOIN HADES.DBO.HD_PADACES PA
                                                    ON PA.PADACES = PU.PADACES INNER JOIN HADES.DBO.TCE_PADACES_PERFIL PP
                                                    ON PP.PADACES = PA.PADACES INNER JOIN HADES.DBO.TCE_PERFIL P
                                                    ON P.ID_PERFIL = PP.ID_PERFIL INNER JOIN HADES.DBO.HD_USUARIO U
                                                    ON U.USUARIO = PU.USUARIO
                                          WHERE
                                                PU.USUARIO = @USUARIO
                                                AND PP.ID_PERFIL = 19 ";

                contextQuery.Parameters.Add("@USUARIO", usuario);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
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

        public bool PossuiPerfilSaldoInicialTotalPor(string usuario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)                                        
                                          FROM
                                                HADES.DBO.HD_PADUSUARIO PU INNER JOIN HADES.DBO.HD_PADACES PA
                                                    ON PA.PADACES = PU.PADACES INNER JOIN HADES.DBO.TCE_PADACES_PERFIL PP
                                                    ON PP.PADACES = PA.PADACES INNER JOIN HADES.DBO.TCE_PERFIL P
                                                    ON P.ID_PERFIL = PP.ID_PERFIL INNER JOIN HADES.DBO.HD_USUARIO U
                                                    ON U.USUARIO = PU.USUARIO
                                          WHERE
                                                PU.USUARIO = @USUARIO
                                                AND PP.ID_PERFIL = 46 ";

                contextQuery.Parameters.Add("@USUARIO", usuario);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
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

        public bool PossuiPerfilTransferenciaTurmaTotalPor(string usuario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            bool retorno = false;

            try
            {
                retorno = this.PossuiPerfilTransferenciaTurmaTotalPor(ctx, usuario);
                return retorno;
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

        public bool PossuiPerfilTransferenciaTurmaTotalPor(DataContext ctx, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   HADES.DBO.HD_PADUSUARIO PU 
                                               INNER JOIN HADES.DBO.HD_PADACES PA 
                                                       ON PA.PADACES = PU.PADACES 
                                               INNER JOIN HADES.DBO.TCE_PADACES_PERFIL PP 
                                                       ON PP.PADACES = PA.PADACES 
                                               INNER JOIN HADES.DBO.TCE_PERFIL P 
                                                       ON P.ID_PERFIL = PP.ID_PERFIL 
                                               INNER JOIN HADES.DBO.HD_USUARIO U 
                                                       ON U.USUARIO = PU.USUARIO 
                                        WHERE  PU.USUARIO = @USUARIO 
                                               AND PP.ID_PERFIL = 20 --TRANSFERÊNCIA TURMA TOTAL ";

            contextQuery.Parameters.Add("@USUARIO", usuario);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public bool PossuiPerfilCoordenacaoPatrimonioPor(string usuario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)                                        
                                          FROM
                                                HADES.DBO.HD_PADUSUARIO PU INNER JOIN HADES.DBO.HD_PADACES PA
                                                    ON PA.PADACES = PU.PADACES INNER JOIN HADES.DBO.TCE_PADACES_PERFIL PP
                                                    ON PP.PADACES = PA.PADACES INNER JOIN HADES.DBO.TCE_PERFIL P
                                                    ON P.ID_PERFIL = PP.ID_PERFIL INNER JOIN HADES.DBO.HD_USUARIO U
                                                    ON U.USUARIO = PU.USUARIO
                                          WHERE
                                                PU.USUARIO = @USUARIO
                                                AND PP.ID_PERFIL = 21 ";

                contextQuery.Parameters.Add("@USUARIO", usuario);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
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

        public bool PossuiPerfilMatriculaTransferenciaPeriodoBloqueioPor(string usuario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)                                        
                                          FROM
                                                HADES.DBO.HD_PADUSUARIO PU INNER JOIN HADES.DBO.HD_PADACES PA
                                                    ON PA.PADACES = PU.PADACES INNER JOIN HADES.DBO.TCE_PADACES_PERFIL PP
                                                    ON PP.PADACES = PA.PADACES INNER JOIN HADES.DBO.TCE_PERFIL P
                                                    ON P.ID_PERFIL = PP.ID_PERFIL INNER JOIN HADES.DBO.HD_USUARIO U
                                                    ON U.USUARIO = PU.USUARIO
                                          WHERE
                                                PU.USUARIO = @USUARIO
                                                AND PP.ID_PERFIL = 24 ";

                contextQuery.Parameters.Add("@USUARIO", usuario);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
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


        public bool PossuiPerfilReaberturaPeriodoBloqueioPor(string usuario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)                                        
                                          FROM
                                                HADES.DBO.HD_PADUSUARIO PU INNER JOIN HADES.DBO.HD_PADACES PA
                                                    ON PA.PADACES = PU.PADACES INNER JOIN HADES.DBO.TCE_PADACES_PERFIL PP
                                                    ON PP.PADACES = PA.PADACES INNER JOIN HADES.DBO.TCE_PERFIL P
                                                    ON P.ID_PERFIL = PP.ID_PERFIL INNER JOIN HADES.DBO.HD_USUARIO U
                                                    ON U.USUARIO = PU.USUARIO
                                          WHERE
                                                PU.USUARIO = @USUARIO
                                                AND PP.ID_PERFIL = 25 ";

                contextQuery.Parameters.Add("@USUARIO", usuario);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
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

        public bool PossuiPerfilAlteracaoFrequenciaGLPPor(string usuario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)                                        
                                          FROM
                                                HADES.DBO.HD_PADUSUARIO PU INNER JOIN HADES.DBO.HD_PADACES PA
                                                    ON PA.PADACES = PU.PADACES INNER JOIN HADES.DBO.TCE_PADACES_PERFIL PP
                                                    ON PP.PADACES = PA.PADACES INNER JOIN HADES.DBO.TCE_PERFIL P
                                                    ON P.ID_PERFIL = PP.ID_PERFIL INNER JOIN HADES.DBO.HD_USUARIO U
                                                    ON U.USUARIO = PU.USUARIO
                                          WHERE
                                                PU.USUARIO = @USUARIO
                                                AND PP.ID_PERFIL = 26 ";

                contextQuery.Parameters.Add("@USUARIO", usuario);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
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

        public bool PossuiPerfilAlteracaoDadosCadastraisAlunoForaPeriodoPor(string usuario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.PossuiPerfilAlteracaoDadosCadastraisAlunoForaPeriodoPor(ctx, usuario);
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

        public bool PossuiPerfilAlteracaoDadosCadastraisAlunoForaPeriodoPor(DataContext ctx, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*)                                        
                                          FROM
                                                HADES.DBO.HD_PADUSUARIO PU INNER JOIN HADES.DBO.HD_PADACES PA
                                                    ON PA.PADACES = PU.PADACES INNER JOIN HADES.DBO.TCE_PADACES_PERFIL PP
                                                    ON PP.PADACES = PA.PADACES INNER JOIN HADES.DBO.TCE_PERFIL P
                                                    ON P.ID_PERFIL = PP.ID_PERFIL INNER JOIN HADES.DBO.HD_USUARIO U
                                                    ON U.USUARIO = PU.USUARIO
                                          WHERE
                                                PU.USUARIO = @USUARIO
                                                AND PP.ID_PERFIL = 71 ";

            contextQuery.Parameters.Add("@USUARIO", usuario);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public bool PossuiPerfilLiberacaoRegistroConfirmacaoMatriculaPor(string usuario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)                                        
                                          FROM
                                                HADES.DBO.HD_PADUSUARIO PU INNER JOIN HADES.DBO.HD_PADACES PA
                                                    ON PA.PADACES = PU.PADACES INNER JOIN HADES.DBO.TCE_PADACES_PERFIL PP
                                                    ON PP.PADACES = PA.PADACES INNER JOIN HADES.DBO.TCE_PERFIL P
                                                    ON P.ID_PERFIL = PP.ID_PERFIL INNER JOIN HADES.DBO.HD_USUARIO U
                                                    ON U.USUARIO = PU.USUARIO
                                          WHERE
                                                PU.USUARIO = @USUARIO
                                                AND PP.ID_PERFIL = 27 ";

                contextQuery.Parameters.Add("@USUARIO", usuario);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
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

        public bool PossuiPerfilValidaCardapioPor(string usuario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.PossuiPerfilValidaCardapioPor(ctx, usuario);
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

        public bool PossuiPerfilValidaCardapioPor(DataContext ctx, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*)                                        
                                          FROM
                                                HADES.DBO.HD_PADUSUARIO PU INNER JOIN HADES.DBO.HD_PADACES PA
                                                    ON PA.PADACES = PU.PADACES INNER JOIN HADES.DBO.TCE_PADACES_PERFIL PP
                                                    ON PP.PADACES = PA.PADACES INNER JOIN HADES.DBO.TCE_PERFIL P
                                                    ON P.ID_PERFIL = PP.ID_PERFIL INNER JOIN HADES.DBO.HD_USUARIO U
                                                    ON U.USUARIO = PU.USUARIO
                                          WHERE
                                                PU.USUARIO = @USUARIO
                                                AND PP.ID_PERFIL = 29 ";

            contextQuery.Parameters.Add("@USUARIO", usuario);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public bool PossuiPerfilFinalizaCardapioPor(string usuario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.PossuiPerfilFinalizaCardapioPor(ctx, usuario);
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

        public bool PossuiPerfilFinalizaCardapioPor(DataContext ctx, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*)                                        
                                          FROM
                                                HADES.DBO.HD_PADUSUARIO PU INNER JOIN HADES.DBO.HD_PADACES PA
                                                    ON PA.PADACES = PU.PADACES INNER JOIN HADES.DBO.TCE_PADACES_PERFIL PP
                                                    ON PP.PADACES = PA.PADACES INNER JOIN HADES.DBO.TCE_PERFIL P
                                                    ON P.ID_PERFIL = PP.ID_PERFIL INNER JOIN HADES.DBO.HD_USUARIO U
                                                    ON U.USUARIO = PU.USUARIO
                                          WHERE
                                                PU.USUARIO = @USUARIO
                                                AND PP.ID_PERFIL = 30 ";

            contextQuery.Parameters.Add("@USUARIO", usuario);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public bool PossuiPerfilLiberacaoPatrimonioFinalizadoPor(string usuario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.PossuiPerfilLiberacaoPatrimonioFinalizadoPor(ctx, usuario);
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

        public bool PossuiPerfilLiberacaoPatrimonioFinalizadoPor(DataContext ctx, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*)                                        
                                          FROM
                                                HADES.DBO.HD_PADUSUARIO PU INNER JOIN HADES.DBO.HD_PADACES PA
                                                    ON PA.PADACES = PU.PADACES INNER JOIN HADES.DBO.TCE_PADACES_PERFIL PP
                                                    ON PP.PADACES = PA.PADACES INNER JOIN HADES.DBO.TCE_PERFIL P
                                                    ON P.ID_PERFIL = PP.ID_PERFIL INNER JOIN HADES.DBO.HD_USUARIO U
                                                    ON U.USUARIO = PU.USUARIO
                                          WHERE
                                                PU.USUARIO = @USUARIO
                                                AND PP.ID_PERFIL = 31 ";

            contextQuery.Parameters.Add("@USUARIO", usuario);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public bool PossuiPerfilCoordenadorProtocoloPor(string usuario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.PossuiPerfilCoordenadorProtocoloPor(ctx, usuario);
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

        public bool PossuiPerfilCoordenadorProtocoloPor(DataContext ctx, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*)                                        
                                          FROM
                                                HADES.DBO.HD_PADUSUARIO PU INNER JOIN HADES.DBO.HD_PADACES PA
                                                    ON PA.PADACES = PU.PADACES INNER JOIN HADES.DBO.TCE_PADACES_PERFIL PP
                                                    ON PP.PADACES = PA.PADACES INNER JOIN HADES.DBO.TCE_PERFIL P
                                                    ON P.ID_PERFIL = PP.ID_PERFIL INNER JOIN HADES.DBO.HD_USUARIO U
                                                    ON U.USUARIO = PU.USUARIO
                                          WHERE
                                                PU.USUARIO = @USUARIO
                                                AND PP.ID_PERFIL = 33 ";

            contextQuery.Parameters.Add("@USUARIO", usuario);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public bool PossuiPerfilAdministradorRVEPor(string usuario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.PossuiPerfilAdministradorRVEPor(ctx, usuario);
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

        public bool PossuiPerfilAdministradorRVEPor(DataContext ctx, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*)                                        
                                          FROM
                                                HADES.DBO.HD_PADUSUARIO PU INNER JOIN HADES.DBO.HD_PADACES PA
                                                    ON PA.PADACES = PU.PADACES INNER JOIN HADES.DBO.TCE_PADACES_PERFIL PP
                                                    ON PP.PADACES = PA.PADACES INNER JOIN HADES.DBO.TCE_PERFIL P
                                                    ON P.ID_PERFIL = PP.ID_PERFIL INNER JOIN HADES.DBO.HD_USUARIO U
                                                    ON U.USUARIO = PU.USUARIO
                                          WHERE
                                                PU.USUARIO = @USUARIO
                                                AND PP.ID_PERFIL = 64 ";

            contextQuery.Parameters.Add("@USUARIO", usuario);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public bool PossuiPerfilExclusaoAEDHPor(string usuario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)                                        
                                          FROM
                                                HADES.DBO.HD_PADUSUARIO PU INNER JOIN HADES.DBO.HD_PADACES PA
                                                    ON PA.PADACES = PU.PADACES INNER JOIN HADES.DBO.TCE_PADACES_PERFIL PP
                                                    ON PP.PADACES = PA.PADACES INNER JOIN HADES.DBO.TCE_PERFIL P
                                                    ON P.ID_PERFIL = PP.ID_PERFIL INNER JOIN HADES.DBO.HD_USUARIO U
                                                    ON U.USUARIO = PU.USUARIO
                                          WHERE
                                                PU.USUARIO = @USUARIO
                                                AND PP.ID_PERFIL = 70 "; //ALUNO - EXCLUIR ESCOLARIZAÇÃO EM OUTROS ESPAÇOS

                contextQuery.Parameters.Add("@USUARIO", usuario);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
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

    }
}
