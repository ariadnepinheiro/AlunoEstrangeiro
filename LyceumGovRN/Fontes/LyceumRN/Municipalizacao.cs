using System;
using System.Collections.Generic;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using System.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    public class Municipalizacao : RNBase
    {
        public DataTable ListaPor(string censo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT ID_MUNICIPALIZACAO, PROCESSO
                                        FROM TCE_MUNICIPALIZACAO
                                        WHERE   CENSO = @CENSO ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo); 

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

        public static TceMunicipalizacao Carregar(string censo)
        {
            var municipalizacao = new TceMunicipalizacao();

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT  ID_MUNICIPALIZACAO, PROCESSO, DT_PUBLICACAO_DO, PAGINA_DO,
                                            NUM_AUTORIZO_PROVISORIO, DT_AUTORIZO_PROVISORIO, DT_VALIDADE_AUTORIZO,
                                            CENSO, MATRICULA, DT_CADASTRO, DT_ALTERACAO
                                    FROM    dbo.TCE_MUNICIPALIZACAO
                                    WHERE   CENSO = @CENSO "
                };
                contextQuery.Parameters.Add("@CENSO", censo);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    if (reader.Read())
                    {
                        municipalizacao.IdMunicipalizacao = Convert.ToInt32(reader["ID_MUNICIPALIZACAO"]);
                        municipalizacao.Processo = Convert.ToString(reader["PROCESSO"]);
                        municipalizacao.PaginaDo = Convert.ToString(reader["PAGINA_DO"]);
                        municipalizacao.NumAutorizoProvisorio = Convert.ToString(reader["NUM_AUTORIZO_PROVISORIO"]);
                        municipalizacao.DtAutorizoProvisorio = Convert.ToDateTime(reader["DT_AUTORIZO_PROVISORIO"]);
                        municipalizacao.DtValidadeAutorizo = Convert.ToDateTime(reader["DT_VALIDADE_AUTORIZO"]);
                        municipalizacao.Censo = censo;
                        municipalizacao.Matricula = Convert.ToString(reader["MATRICULA"]);
                        municipalizacao.DtCadastro = Convert.ToDateTime(reader["DT_CADASTRO"]);

                        if (reader["DT_PUBLICACAO_DO"] != DBNull.Value)
                        {
                            municipalizacao.DtPublicacaoDo = Convert.ToDateTime(reader["DT_PUBLICACAO_DO"]);
                        }
                        if (reader["DT_ALTERACAO"] != DBNull.Value)
                        {
                            municipalizacao.DtAlteracao = Convert.ToDateTime(reader["DT_ALTERACAO"]);
                        }
                    }
                }
                return municipalizacao;
            }
        }

        public static ValidacaoDados Validar(TceMunicipalizacao municipalizacao)
        {
            var mensagens = new List<string>();
            RN.Municipalizacao rnMunicipalizacao = new Municipalizacao();
            DataContext contexto = null;
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (municipalizacao == null)
            {
                return validacaoDados;
            }

            //Regra retirada a pedido do chamado 23233 para permitira cadastro de historico
            //if (!UnidadeEnsinoSituacao.VerificarSituacaoMunicipalizacao(municipalizacao.Censo))
            //{
            //    mensagens.Add("Unidade de Ensino não se encontra na Situação de Municipalização.");
            //}

            if (string.IsNullOrEmpty(municipalizacao.Censo))
            {
                mensagens.Add("O campo CENSO é obrigatório!");
            }

            if (string.IsNullOrEmpty(municipalizacao.Processo))
            {
                mensagens.Add("O campo Nº DE PROCESSO DE MUNICIPALIZAÇÃO é obrigatório!");
            }
            else
            {
                if (municipalizacao.Processo.Length > 50)
                {
                    mensagens.Add("O campo PROCESSO deve conter no máximo 50 caracteres!");
                }
            }


            if (string.IsNullOrEmpty(municipalizacao.NumAutorizoProvisorio))
            {
                mensagens.Add("O campo Nº DE AUTORIZO PROVISÓRIO é obrigatório!");
            }
            else
            {
                if (municipalizacao.NumAutorizoProvisorio.Length > 50)
                {
                    mensagens.Add("O campo Nº DE AUTORIZO PROVISÓRIO deve conter no máximo 50 caracteres!");
                }
            }       

            if (municipalizacao.DtAutorizoProvisorio < Convert.ToDateTime("25/02/1988"))
            {
                mensagens.Add("O campo DATA DO AUTOZIO PROVISÓRIO é obrigatório e deve ser maior que '25/02/1988'!");
            }

            if (municipalizacao.DtValidadeAutorizo < Convert.ToDateTime("25/02/1988"))
            {
                mensagens.Add("O campo VALIDADE DO AUTORIZO é obrigatório e deve ser maior que '25/02/1988'!");
            }

            //if (municipalizacao.DtPublicacaoDo > DateTime.MinValue)
            //{
            //    if (string.IsNullOrEmpty(municipalizacao.PaginaDo))
            //    {
            //        mensagens.Add("O campo PÁGINA DO é obrigatório quando for informada a DATA DA PUBLICAÇÃO DO EXTRATO EM DO!");
            //    }
            //}

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe cens/inicio cadastrado
                    if (rnMunicipalizacao.PossuiOutroCadastradoPor(contexto, municipalizacao.Censo, municipalizacao.DtAutorizoProvisorio, municipalizacao.IdMunicipalizacao))
                    {
                        mensagens.Add("Já existe uma MUNICIPALIZAÇÃO cadastrada para esta unidade de ensino com esta DATA DO AUTOZIO PROVISÓRIO.");
                    }

                    //Verifica se a data de inicio está intercalada com outro
                    if (rnMunicipalizacao.PossuiDataEmOutroIntervaloPor(contexto, municipalizacao.Censo, municipalizacao.DtAutorizoProvisorio, municipalizacao.IdMunicipalizacao))
                    {
                        mensagens.Add("DATA DO AUTOZIO PROVISÓRIO não pode estar dentro do intervalo de outra municipalização dessa unidade de ensino.");
                    }

                    //Verifica se a data de inicio está intercalada com outro
                    if (rnMunicipalizacao.PossuiDataEmOutroIntervaloPor(contexto, municipalizacao.Censo, municipalizacao.DtValidadeAutorizo, municipalizacao.IdMunicipalizacao))
                    {
                        mensagens.Add("VALIDADE DO AUTORIZO não pode estar dentro do intervalo de outra municipalização dessa unidade de ensino.");
                    }

                    //Verifica se as datas de inicio e de fim estão intercalada com outro
                    if (rnMunicipalizacao.PossuiOutraIntercaladaPor(contexto, municipalizacao.Censo, municipalizacao.DtAutorizoProvisorio, municipalizacao.DtValidadeAutorizo, municipalizacao.IdMunicipalizacao))
                    {
                        mensagens.Add("DATA DO AUTOZIO PROVISÓRIO E VALIDADE DO AUTORIZO não podem intercalar com outra municipalização dessa unidade de ensino.");
                    }
                }
                catch (Exception ex)
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
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

        private bool PossuiDataEmOutroIntervaloPor(DataContext ctx, string censo, DateTime data, int idMunicipalizacao)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM  TCE_MUNICIPALIZACAO  (NOLOCK)
                                    WHERE CENSO = @CENSO 
                                        AND ID_MUNICIPALIZACAO <> @IDMUNICIPALIZACAO
	                                    AND @DATA BETWEEN DT_AUTORIZO_PROVISORIO AND 
			                                    CONVERT(DATE, CONVERT(DATETIME, ISNULL(DT_VALIDADE_AUTORIZO, GETDATE())) ) ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@IDMUNICIPALIZACAO", SqlDbType.Int, idMunicipalizacao);
            contextQuery.Parameters.Add("@DATA", SqlDbType.Date, data.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiOutraIntercaladaPor(DataContext ctx, string censo, DateTime dataInicio, DateTime dataFim, int idMunicipalizacao)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM   TCE_MUNICIPALIZACAO  (NOLOCK)
                                    WHERE CENSO = @CENSO 
	                                    AND ID_MUNICIPALIZACAO <> @IDMUNICIPALIZACAO
	                                    AND @DATAINICIO <= CONVERT(DATE, DT_AUTORIZO_PROVISORIO) 
	                                    AND @DATAFIM >= CONVERT(DATE, DT_VALIDADE_AUTORIZO) ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@IDMUNICIPALIZACAO", SqlDbType.Int, idMunicipalizacao);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, dataInicio.Date);
            contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, dataFim.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiOutroCadastradoPor(DataContext ctx, string censo, DateTime inicio, int idMunicipalizacao)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM TCE_MUNICIPALIZACAO (NOLOCK)
                                WHERE CENSO = @CENSO 
                                    AND DT_AUTORIZO_PROVISORIO = @DATAINICIO
	                                AND ID_MUNICIPALIZACAO <> @IDMUNICIPALIZACAO ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@IDMUNICIPALIZACAO", SqlDbType.Int, idMunicipalizacao);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, inicio);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public int Insere(TceMunicipalizacao municipalizacao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            int id = 0;
            try
            {
                contextQuery.Command = @" INSERT  INTO TCE_MUNICIPALIZACAO ( PROCESSO, DT_PUBLICACAO_DO, PAGINA_DO,
                                                       NUM_AUTORIZO_PROVISORIO,
                                                       DT_AUTORIZO_PROVISORIO,
                                                       DT_VALIDADE_AUTORIZO, CENSO, MATRICULA )
                                        VALUES  ( @PROCESSO, @DT_PUBLICACAO_DO, @PAGINA_DO, @NUM_AUTORIZO_PROVISORIO,
                                                  @DT_AUTORIZO_PROVISORIO, @DT_VALIDADE_AUTORIZO, @CENSO, @MATRICULA ) 
                                           
                                        SELECT IDENT_CURRENT('TCE_MUNICIPALIZACAO') ";

                contextQuery.Parameters.Add("@PROCESSO", municipalizacao.Processo);
                contextQuery.Parameters.Add("@DT_PUBLICACAO_DO", municipalizacao.DtPublicacaoDo);
                contextQuery.Parameters.Add("@PAGINA_DO", municipalizacao.PaginaDo);
                contextQuery.Parameters.Add("@NUM_AUTORIZO_PROVISORIO", municipalizacao.NumAutorizoProvisorio);
                contextQuery.Parameters.Add("@DT_AUTORIZO_PROVISORIO", municipalizacao.DtAutorizoProvisorio);
                contextQuery.Parameters.Add("@DT_VALIDADE_AUTORIZO", municipalizacao.DtValidadeAutorizo);
                contextQuery.Parameters.Add("@CENSO", municipalizacao.Censo);
                contextQuery.Parameters.Add("@MATRICULA", municipalizacao.Matricula);

                id = Convert.ToInt32(contexto.GetReturnValue(contextQuery));

                return id;
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

        public static void Alterar(TceMunicipalizacao municipalizacao)
        {
            var contextQuery = new ContextQuery(
            @" UPDATE  TCE_MUNICIPALIZACAO
                SET     PROCESSO = @PROCESSO, 
		                DT_PUBLICACAO_DO = @DT_PUBLICACAO_DO,
                        PAGINA_DO = @PAGINA_DO,
                        NUM_AUTORIZO_PROVISORIO = @NUM_AUTORIZO_PROVISORIO,
                        DT_AUTORIZO_PROVISORIO = @DT_AUTORIZO_PROVISORIO,
                        DT_VALIDADE_AUTORIZO = @DT_VALIDADE_AUTORIZO, 
		                CENSO = @CENSO,
                        MATRICULA = @MATRICULA, 
                        DT_ALTERACAO = GETDATE()
                WHERE   ID_MUNICIPALIZACAO = @ID_MUNICIPALIZACAO ");

            contextQuery.Parameters.Add("@ID_MUNICIPALIZACAO", municipalizacao.IdMunicipalizacao);
            contextQuery.Parameters.Add("@PROCESSO", municipalizacao.Processo);
            contextQuery.Parameters.Add("@DT_PUBLICACAO_DO", municipalizacao.DtPublicacaoDo);
            contextQuery.Parameters.Add("@PAGINA_DO", municipalizacao.PaginaDo);
            contextQuery.Parameters.Add("@NUM_AUTORIZO_PROVISORIO", municipalizacao.NumAutorizoProvisorio);
            contextQuery.Parameters.Add("@DT_AUTORIZO_PROVISORIO", municipalizacao.DtAutorizoProvisorio);
            contextQuery.Parameters.Add("@DT_VALIDADE_AUTORIZO", municipalizacao.DtValidadeAutorizo);
            contextQuery.Parameters.Add("@CENSO", municipalizacao.Censo);
            contextQuery.Parameters.Add("@MATRICULA", municipalizacao.Matricula);

            ExecutarAlteracao(contextQuery);
        }

        public static bool VerificarMunicipalizacao(string censo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT 1 
                        FROM TCE_MUNICIPALIZACAO
                        WHERE CENSO = @CENSO 
                        ");

                contextQuery.Parameters.Add("@CENSO", censo);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    return true;
                }
            }
            return false;
        }

        public TceMunicipalizacao ObtemMunicipalizacaoPor(int idMunicipalizacao)
        {
            TceMunicipalizacao municipalizacao = new TceMunicipalizacao();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();


            try
            {
                contextQuery.Command = @"SELECT  ID_MUNICIPALIZACAO, 
                                                PROCESSO, 
                                                DT_PUBLICACAO_DO, 
                                                PAGINA_DO,
                                                NUM_AUTORIZO_PROVISORIO, 
                                                DT_AUTORIZO_PROVISORIO, 
                                                DT_VALIDADE_AUTORIZO,
                                                CENSO, 
                                                MATRICULA, 
                                                DT_CADASTRO, 
                                                DT_ALTERACAO
                                    FROM    dbo.TCE_MUNICIPALIZACAO
                                    WHERE   ID_MUNICIPALIZACAO = @ID_MUNICIPALIZACAO  ";

                contextQuery.Parameters.Add("@ID_MUNICIPALIZACAO", idMunicipalizacao);
               

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    municipalizacao.IdMunicipalizacao = Convert.ToInt32(reader["ID_MUNICIPALIZACAO"]);
                    municipalizacao.Processo = Convert.ToString(reader["PROCESSO"]);
                    municipalizacao.PaginaDo = Convert.ToString(reader["PAGINA_DO"]);
                    municipalizacao.NumAutorizoProvisorio = Convert.ToString(reader["NUM_AUTORIZO_PROVISORIO"]);
                    municipalizacao.DtAutorizoProvisorio = Convert.ToDateTime(reader["DT_AUTORIZO_PROVISORIO"]);
                    municipalizacao.DtValidadeAutorizo = Convert.ToDateTime(reader["DT_VALIDADE_AUTORIZO"]);
                    municipalizacao.Censo = Convert.ToString(reader["CENSO"]); 
                    municipalizacao.Matricula = Convert.ToString(reader["MATRICULA"]);
                    municipalizacao.DtCadastro = Convert.ToDateTime(reader["DT_CADASTRO"]);

                    if (reader["DT_PUBLICACAO_DO"] != DBNull.Value)
                    {
                        municipalizacao.DtPublicacaoDo = Convert.ToDateTime(reader["DT_PUBLICACAO_DO"]);
                    }
                    if (reader["DT_ALTERACAO"] != DBNull.Value)
                    {
                        municipalizacao.DtAlteracao = Convert.ToDateTime(reader["DT_ALTERACAO"]);
                    }
                }

                return municipalizacao;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
     
        public DataTable ObtemDadosUltimaMunicipalizacaoPor(string censo)
        {             
            ContextQuery contextQuery = new ContextQuery();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            DataTable lista = null;

            try
            {
                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"SP_MUNICIPALIZACAOVIGENTE";
                contextQuery.Parameters.Add("@CENSO", censo);


                lista = contexto.GetDataTable(contextQuery);

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
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }

            return lista;
        }
    }
}
