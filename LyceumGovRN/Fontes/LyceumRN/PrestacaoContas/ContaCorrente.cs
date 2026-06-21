using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class ContaCorrente
    {
        public DTOs.DadosContaCorrente ObtemContaAtivaUnidadePor(DataContext contexto, string censo)
        {
            DTOs.DadosContaCorrente dadosContaCorrente = new DTOs.DadosContaCorrente();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"  SELECT   CC.CONTACORRENTEID,
											        CC.CENSO,
											        CC.CENSO,
											        CC.BANCO,
											        B.NOME AS BANCONOME,
											        CC.AGENCIA,
											        A.NOME AS AGENCIANOME,
											        CC.CONTA
                                            FROM	PrestacaoContas.CONTACORRENTE cc (NOLOCK)	                                
											        INNER JOIN HADES.DBO.BANCOS B (NOLOCK) 
					                                        ON CONVERT(INT, CC.BANCO) = CONVERT(INT, B.BANCO)
											        LEFT JOIN  HADES.DBO.AGENCIAS A (NOLOCK) 
					                                        ON CONVERT(VARCHAR, CC.AGENCIA) = CONVERT(VARCHAR, A.AGENCIA) 
                                                                AND CONVERT(VARCHAR, CC.BANCO) = CONVERT(VARCHAR, A.BANCO)
                                            WHERE	CENSO = @CENSO
		                                            AND DATAINICIO <= GETDATE()
		                                            AND (DATAFIM IS NULL OR DATAFIM >= GETDATE()) ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosContaCorrente.ContaCorrenteId = Convert.ToInt32(reader["CONTACORRENTEID"]);
                    dadosContaCorrente.IdRegional = Convert.ToInt32(reader["CONTACORRENTEID"]);
                    dadosContaCorrente.Censo = null;
                    dadosContaCorrente.Banco = Convert.ToString(reader["BANCO"]);
                    dadosContaCorrente.BancoNome = Convert.ToString(reader["BANCONOME"]);
                    dadosContaCorrente.Agencia = Convert.ToString(reader["AGENCIA"]);
                    dadosContaCorrente.AgenciaNome = Convert.ToString(reader["AGENCIANOME"]);
                    dadosContaCorrente.Conta = Convert.ToString(reader["CONTA"]);
                }

                return dadosContaCorrente;
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

        public DTOs.DadosContaCorrente ObtemContaAtivaRegionalPor(DataContext contexto, int idRegional)
        {
            DTOs.DadosContaCorrente dadosContaCorrente = new DTOs.DadosContaCorrente();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"  SELECT  CC.CONTACORRENTEID,
											        CC.REGIONALID,
											        CC.CENSO,
											        CC.BANCO,
											        B.NOME AS BANCONOME,
											        CC.AGENCIA,
											        CASE
														WHEN A.NOME IS NULL THEN CC.AGENCIA
														ELSE CC.AGENCIA + ' - ' + A.NOME 
											        END AGENCIANOME,
											        CC.CONTA
                                            FROM	PrestacaoContas.CONTACORRENTE cc (NOLOCK)	                                
											        INNER JOIN HADES.DBO.BANCOS B (NOLOCK) 
					                                        ON CONVERT(INT, CC.BANCO) = CONVERT(INT, B.BANCO)
											        LEFT JOIN  HADES.DBO.AGENCIAS A (NOLOCK) 
					                                        ON CONVERT(INT, CC.AGENCIA) = CONVERT(INT, A.AGENCIA) 
                                                                AND CONVERT(INT, CC.BANCO) = CONVERT(INT, A.BANCO)
                                            WHERE	REGIONALID = @REGIONALID
		                                            AND DATAINICIO <= GETDATE()
		                                            AND (DATAFIM IS NULL OR DATAFIM >= GETDATE()) ";

                contextQuery.Parameters.Add("@REGIONALID", SqlDbType.Int, idRegional);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosContaCorrente.ContaCorrenteId = Convert.ToInt32(reader["CONTACORRENTEID"]);
                    dadosContaCorrente.IdRegional = Convert.ToInt32(reader["CONTACORRENTEID"]);
                    dadosContaCorrente.Censo = null;
                    dadosContaCorrente.Banco = Convert.ToString(reader["BANCO"]);
                    dadosContaCorrente.BancoNome = Convert.ToString(reader["BANCONOME"]);
                    dadosContaCorrente.Agencia = Convert.ToString(reader["AGENCIA"]);
                    dadosContaCorrente.AgenciaNome = Convert.ToString(reader["AGENCIANOME"]);
                    dadosContaCorrente.Conta = Convert.ToString(reader["CONTA"]);
                }

                return dadosContaCorrente;
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

        public DataTable ListaPorRegional(int regionalId)
        {
            return this.ListaPor(regionalId, null);
        }

        public DataTable ListaPorUnidadeEnsino(string censo)
        {
            return this.ListaPor(null, censo);
        }

        private DataTable ListaPor(int? regionalId, string censo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder sql = new StringBuilder();
            DataTable dt = null;

            try
            {
                sql.Append(@"      SELECT CONTACORRENTEID, 
		                                REGIONALID, 
		                                CENSO, 
		                                cc.BANCO, 
		                                b.NOME as NOMEBANCO, 
		                                 cc.AGENCIA, 
		                                a.NOME as NOMEAGENCIA,
		                                 cc.CONTA, 		                                
		                                USUARIOID, 
		                                DATACADASTRO, 
		                                DATAALTERACAO,
                                        DATAINICIO,
										DATAFIM
                                FROM PrestacaoContas.CONTACORRENTE cc (NOLOCK)	                                
	                                 INNER JOIN  HADES.DBO.BANCOS B (NOLOCK) 
					                                ON CONVERT(INT, CC.BANCO) = CONVERT(INT, B.BANCO)
	                                 LEFT JOIN  HADES.DBO.AGENCIAS A (NOLOCK) 
					                                ON CONVERT(VARCHAR, CC.AGENCIA) = CONVERT(VARCHAR, A.AGENCIA) 
                                                        AND CONVERT(INT, CC.BANCO) = CONVERT(INT, A.BANCO)");

                if (censo.IsNullOrEmptyOrWhiteSpace())
                {
                    sql.Append(@"
                            WHERE REGIONALID = @REGIONALID ");
                    contextQuery.Parameters.Add("@REGIONALID", SqlDbType.Int, regionalId);
                }
                else
                {
                    sql.Append(@"
                            WHERE CENSO = @CENSO ");
                    contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                }


                contextQuery.Command = sql.ToString();

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                if (contexto != null)
                {
                    contexto.Abandon();
                }
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    if (Convert.ToString(ex.Message).Contains("ERRO:"))
                    {
                        mensagem = ex.Message.Replace("ERRO: ", string.Empty);
                    }
                    else
                    {
                        mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine,
                            Convert.ToString(ex.Message));
                    }
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

        public ICollection<Entidades.ContaCorrente> ListaPor(DataContext contexto)
        {
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@"      SELECT CONTACORRENTEID, 
		                                REGIONALID, 
		                                CENSO, 
		                                cc.BANCO, 
		                                b.NOME as NOMEBANCO, 
		                                 cc.AGENCIA, 
		                                a.NOME as NOMEAGENCIA,
		                                 cc.CONTA, 		                                
		                                USUARIOID, 
		                                DATACADASTRO, 
		                                DATAALTERACAO,
                                        DATAINICIO,
										DATAFIM
                                FROM PrestacaoContas.CONTACORRENTE cc (NOLOCK)	                                
	                                 INNER JOIN  HADES.DBO.BANCOS B (NOLOCK) 
					                                ON CONVERT(INT, CC.BANCO) = CONVERT(INT, B.BANCO)
	                                 LEFT JOIN  HADES.DBO.AGENCIAS A (NOLOCK) 
					                                ON CONVERT(VARCHAR, CC.AGENCIA) = CONVERT(VARCHAR, A.AGENCIA) 
                                                        AND CONVERT(INT, CC.BANCO) = CONVERT(INT, A.BANCO)");


                contextQuery.Command = sql.ToString();

                return contexto.TryToBindEntities<Entidades.ContaCorrente>(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    if (Convert.ToString(ex.Message).Contains("ERRO:"))
                    {
                        mensagem = ex.Message.Replace("ERRO: ", string.Empty);
                    }
                    else
                    {
                        mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine,
                            Convert.ToString(ex.Message));
                    }
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
        }

        public bool PossuiContaVigentePorRegional(int regionalId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"SELECT COUNT(*) as TOTAL FROM PRESTACAOCONTAS.CONTACORRENTE (NOLOCK) 
                                            WHERE DATAFIM is null
                                                    AND REGIONALID = @REGIONALID  ";


            contextQuery.Parameters.Add("@REGIONALID", SqlDbType.Int, regionalId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiContaVigenteRegional(int regionalId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"SELECT DATAFIM FROM PRESTACAOCONTAS.CONTACORRENTE (NOLOCK)
                                            WHERE REGIONALID = @REGIONALID  ";


            contextQuery.Parameters.Add("@REGIONALID", SqlDbType.Int, regionalId);

            //var id = contexto.GetReturnValue(contextQuery);

            //if (id != DBNull.Value)
            //{
            //    if (Convert.ToInt32(id) != 0)
            //    {
            //        existe = true;
            //    }
            //}
            //else
            //{
            //    existe = false;
            //}

            return existe;
        }

        public bool PossuiContaVigenteCenso(int censo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) as TOTAL FROM PRESTACAOCONTAS.CONTACORRENTE (NOLOCK) 
                                            WHERE DATAFIM is null AND CENSO = @CENSO ";


            contextQuery.Parameters.Add("@CENSO", SqlDbType.Int, censo);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            //var id = contexto.GetReturnValue(contextQuery);

            //if (id != DBNull.Value)
            //{
            //    if (Convert.ToInt32(id) != 0)
            //    {
            //        existe = true;
            //    }
            //}
            //else
            //{
            //    existe = false;
            //}

            return existe;
        }

        public bool PossuiContaVigentePorCenso(int censo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"SELECT COUNT(*)
                                            FROM PRESTACAOCONTAS.CONTACORRENTE (NOLOCK)
                                            WHERE DATAFIM is null
                                                    AND CENSO = @CENSO  ";


            contextQuery.Parameters.Add("@CENSO", SqlDbType.Int, censo);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutroCadastradoPor(DataContext contexto, string conta, string banco, string agencia, int contaCorrenteId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*)
                                            FROM PRESTACAOCONTAS.CONTACORRENTE (NOLOCK)
                                            WHERE CONTA = @CONTA
                                                    AND BANCO = @BANCO
                                                    AND AGENCIA = @AGENCIA
                                                    AND CONTACORRENTEID <> @CONTACORRENTEID ";

            contextQuery.Parameters.Add("@CONTA", SqlDbType.VarChar, conta);
            contextQuery.Parameters.Add("@BANCO", SqlDbType.VarChar, banco);
            contextQuery.Parameters.Add("@AGENCIA", SqlDbType.VarChar, agencia);
            contextQuery.Parameters.Add("@CONTACORRENTEID", SqlDbType.Int, contaCorrenteId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public ValidacaoDados Valida(Entidades.ContaCorrente contaCorrente, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (contaCorrente == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (contaCorrente.ContaCorrenteId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (contaCorrente.RegionalId == null && contaCorrente.RegionalId <= 0
                && contaCorrente.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE DE ENSINO ou REGIONAL é obrigatório.");
            }
            else
            {
                if (!contaCorrente.Censo.IsNullOrEmptyOrWhiteSpace())
                {
                    contaCorrente.RegionalId = null;
                }
                else
                {
                    contaCorrente.Censo = null;
                }
            }

            if (contaCorrente.Banco.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NÚMERO DO BANCO é obrigatório.");
            }
            else
            {
                int resultado;
                if (!int.TryParse(contaCorrente.Banco, out resultado))
                {
                    mensagens.Add("Campo NÚMERO DO BANCO deve ser um número.");
                }
            }

            if (contaCorrente.Agencia.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NÚMERO DA AGÊNCIA é obrigatório.");
            }
            else
            {
                int resultado;
                if (!int.TryParse(contaCorrente.Banco, out resultado))
                {
                    mensagens.Add("Campo NÚMERO DA AGÊNCIA deve ser um número.");
                }
            }

            if (contaCorrente.Conta.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NÚMERO DA CONTA é obrigatório.");
            }

            if (contaCorrente.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO é obrigatório.");
            }


            if (contaCorrente.DataInicio <= DateTime.MinValue)
            {
                mensagens.Add("Campo obrigatório DATA INICIO não foi preenchido");
            }
            else if (contaCorrente.DataInicio > DateTime.Now)
            {
                mensagens.Add("Campo DATA INICIO não pode ser maior que a data atual");
            }

            if (contaCorrente.DataFim != null && contaCorrente.DataFim != null)
            {
                if (contaCorrente.DataInicio > contaCorrente.DataFim)
                {
                    mensagens.Add("A DATA INÍCIO não pode ser superior a DATA FIM.");
                }

                if (contaCorrente.DataFim.Value.Date > DateTime.Now.Date)
                {
                    mensagens.Add("A DATA FIM da conta corrente não pode ser maior do que a data atual.");
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se tem conta vigente
                    if (PossuiContaVigentePor(contexto, contaCorrente.ContaCorrenteId, contaCorrente.Censo, contaCorrente.RegionalId))
                    {
                        mensagens.Add("Já existe uma conta corrente vigente para esta regional ou unidade escolar.");
                    }

                    // Verifica se já existe a descricao cadastrada
                    if (this.PossuiOutroCadastradoPor(contexto, contaCorrente.Conta, contaCorrente.Banco, contaCorrente.Agencia, contaCorrente.ContaCorrenteId))
                    {
                        mensagens.Add("Já existe uma conta corrente cadastrada com este banco / conta / agência.");
                    }

                    //Verifica se a data de inicio está intercalada com outro
                    if (this.PossuiDataInicioEmOutroIntervaloPor(contexto, contaCorrente.Censo, contaCorrente.RegionalId, contaCorrente.DataInicio.Value, contaCorrente.ContaCorrenteId))
                    {
                        mensagens.Add("DATA INÍCIO não pode estar dentro do intervalo de outra conta corrente dessa escola ou regional.");
                    }

                    //Verifica se não possui data de fim
                    if (contaCorrente.DataFim != null && contaCorrente.DataFim > DateTime.MinValue)
                    {
                        //Verifica se a data de inicio está intercalada com outro
                        if (this.PossuiDataFimEmOutroIntervaloPor(contexto, contaCorrente.Censo, contaCorrente.RegionalId, Convert.ToDateTime(contaCorrente.DataFim), contaCorrente.ContaCorrenteId))
                        {
                            mensagens.Add("DATA FIM não pode estar dentro do intervalo de outra conta corrente dessa escola ou regional.");
                        }

                        //Verifica se as datas de inicio e de fim estão intercalada com outro
                        if (this.PossuiOutraIntercaladaPor(contexto, contaCorrente.Censo, contaCorrente.RegionalId, contaCorrente.DataInicio.Value, Convert.ToDateTime(contaCorrente.DataFim), contaCorrente.ContaCorrenteId))
                        {
                            mensagens.Add("DATA INÍCIO E FIM não podem intercalar com outra conta corrente desta escola ou regional.");
                        }
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

        private bool PossuiOutraIntercaladaPor(DataContext ctx, string censo, int? regional, DateTime dataInicio, DateTime dataFim, int contaCorrenteId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                            FROM   PrestacaoContas.CONTACORRENTE  (NOLOCK)
                            WHERE ((@CENSO is not null and  CENSO = @CENSO )
                                  OR (@REGIONAL is not null and  REGIONALID = @REGIONAL ) )                                                                           
                                    AND CONTACORRENTEID <> @CONTACORRENTEID
                                    AND @DATAINICIO <= CONVERT(DATE, DATAINICIO) 
                                    AND @DATAFIM >= CONVERT(DATE, CONVERT(DATETIME, ISNULL(DATAFIM, GETDATE()))) ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@REGIONAL", SqlDbType.Bit, regional);
            contextQuery.Parameters.Add("@CONTACORRENTEID", SqlDbType.Int, contaCorrenteId);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, dataInicio.Date);
            contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, dataFim.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiDataInicioEmOutroIntervaloPor(DataContext ctx, string censo, int? regional, DateTime data, int contaCorrenteID)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*)
 
                                FROM  [PrestacaoContas].[CONTACORRENTE]  (NOLOCK)
                                WHERE ((@CENSO is not null and  CENSO = @CENSO )
                                  OR (@REGIONAL is not null and  REGIONALID = @REGIONAL ) )                                                                            
                                    AND CONTACORRENTEID <> @CONTACORRENTEID
	                                AND @DATA BETWEEN DATAINICIO AND 
			                                CONVERT(DATE, CONVERT(DATETIME, ISNULL(DATAFIM, GETDATE())) ) ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@REGIONAL", SqlDbType.Bit, regional);
            contextQuery.Parameters.Add("@CONTACORRENTEID", SqlDbType.Int, contaCorrenteID);
            contextQuery.Parameters.Add("@DATA", SqlDbType.Date, data.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiDataFimEmOutroIntervaloPor(DataContext ctx, string censo, int? regional, DateTime data, int contaCorrenteId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                        FROM   [PrestacaoContas].[CONTACORRENTE] 
                        WHERE  ((@CENSO is not null and  CENSO = @CENSO )
                                  OR (@REGIONAL is not null and  REGIONALID = @REGIONAL ) )      AND CONTACORRENTEID <> @CONTACORRENTEID
                                AND @DATA BETWEEN 
                                    CONVERT(DATE, CONVERT(DATETIME, DATAINICIO) + 1) AND CONVERT( 
                                    DATE, CONVERT(DATETIME, ISNULL(DATAFIM, GETDATE())))  ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@REGIONAL", SqlDbType.Bit, regional);
            contextQuery.Parameters.Add("@CONTACORRENTEID", SqlDbType.Int, contaCorrenteId);
            contextQuery.Parameters.Add("@DATA", SqlDbType.Date, data.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public void Insere(Entidades.ContaCorrente contaCorrente)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                this.Insere(contexto, contaCorrente);
            }
            catch (Exception ex)
            {
                if (contexto != null)
                {
                    contexto.Abandon();
                }
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    if (Convert.ToString(ex.Message).Contains("ERRO:"))
                    {
                        mensagem = ex.Message.Replace("ERRO: ", string.Empty);
                    }
                    else
                    {
                        mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine,
                            Convert.ToString(ex.Message));
                    }
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

        private void Insere(DataContext contexto, Entidades.ContaCorrente contaCorrente)
        {
            //DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();

            try
            {




                contextQuery.Command = @" INSERT INTO PrestacaoContas.CONTACORRENTE
                                           (REGIONALID
                                           ,CENSO
                                           ,BANCO
                                           ,AGENCIA
                                           ,CONTA                                           
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO, DATAINICIO, DATAFIM)
                                     VALUES
	                                       (@REGIONALID
                                           ,@CENSO
                                           ,@BANCO
                                           ,@AGENCIA
                                           ,@CONTA                                           
                                           ,@USUARIOID
                                           ,@DATACADASTRO
                                           ,@DATAALTERACAO, @DATAINICIO, @DATAFIM) 

                         SELECT IDENT_CURRENT('PrestacaoContas.CONTACORRENTE') ";


                contextQuery.Parameters.Add("@REGIONALID", SqlDbType.Int, contaCorrente.RegionalId);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, contaCorrente.Censo);
                contextQuery.Parameters.Add("@BANCO", SqlDbType.VarChar, contaCorrente.Banco);
                contextQuery.Parameters.Add("@AGENCIA", SqlDbType.VarChar, contaCorrente.Agencia);
                contextQuery.Parameters.Add("@CONTA", SqlDbType.VarChar, contaCorrente.Conta);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, contaCorrente.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, contaCorrente.DataInicio);

                if (contaCorrente.DataFim != null && contaCorrente.DataFim > DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, contaCorrente.DataFim);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, DBNull.Value);
                }

                contaCorrente.ContaCorrenteId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));

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

        public void Atualiza(Entidades.ContaCorrente contaCorrente)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            try
            {
                this.Atualiza(contexto, contaCorrente);
            }
            catch (Exception ex)
            {
                if (contexto != null)
                {
                    contexto.Abandon();
                }
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    if (Convert.ToString(ex.Message).Contains("ERRO:"))
                    {
                        mensagem = ex.Message.Replace("ERRO: ", string.Empty);
                    }
                    else
                    {
                        mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine,
                            Convert.ToString(ex.Message));
                    }
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

        private void Atualiza(DataContext contexto, Entidades.ContaCorrente contaCorrente)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PrestacaoContas.CONTACORRENTE
                                       SET
									    BANCO = @BANCO
										,CONTA = @CONTA
										,AGENCIA = @AGENCIA
										,DATAINICIO = @DATAINICIO
										,DATAFIM = @DATAFIM 
                                        ,USUARIOID = @USUARIOID
                                        ,DATAALTERACAO = @DATAALTERACAO
                                     WHERE CONTACORRENTEID = @CONTACORRENTEID ";

            contextQuery.Parameters.Add("@CONTACORRENTEID", SqlDbType.Int, contaCorrente.ContaCorrenteId);
            contextQuery.Parameters.Add("@BANCO", SqlDbType.VarChar, contaCorrente.Banco);
            contextQuery.Parameters.Add("@CONTA", SqlDbType.VarChar, contaCorrente.Conta);
            contextQuery.Parameters.Add("@AGENCIA", SqlDbType.VarChar, contaCorrente.Agencia);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, contaCorrente.DataInicio);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, contaCorrente.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            if (contaCorrente.DataFim != null && contaCorrente.DataFim > DateTime.MinValue)
            {
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, contaCorrente.DataFim);
            }
            else
            {
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, DBNull.Value);
            }

            contexto.ApplyModifications(contextQuery);
        }

        public ValidacaoDados ValidaRemocao(int contaCorrenteId, string censo, int? regionalId)
        {
            List<string> mensagens = new List<string>();
            AnaliseContaCorrente rnAnaliseContaCorrente = new AnaliseContaCorrente();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
           {
               Valido = false
           };

            if (contaCorrenteId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se ja foi utilizado 
                    if (rnAnaliseContaCorrente.PossuiContaCorrentePor(contexto, contaCorrenteId))
                    {
                        mensagens.Add("A conta corrrente não pode ser excluida pois foi analisada.");
                    }

                    if (ehContaVigentePor(contexto, contaCorrenteId, censo, regionalId))
                    {
                        mensagens.Add("A conta corrrente não pode ser excluida pois está vigente.");
                    }

                }
                catch
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw;
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
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void Remove(int contaCorrenteId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE PrestacaoContas.CONTACORRENTE
                            WHERE  CONTACORRENTEID = @CONTACORRENTEID  ";

                contextQuery.Parameters.Add("@CONTACORRENTEID", SqlDbType.Int, contaCorrenteId);

                ctx.ApplyModifications(contextQuery);
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

        public DataTable ObtemDataFimPor(int? contaCorrenteId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder sql = new StringBuilder();
            DataTable dt = null;

            try
            {
                sql.Append(@"  SELECT DATAFIM
                                FROM PrestacaoContas.CONTACORRENTE cc (NOLOCK)	                                
	                                    INNER JOIN  HADES.DBO.BANCOS B (NOLOCK) 
					                                ON CONVERT(INT, CC.BANCO) = CONVERT(INT, B.BANCO)
	                                    LEFT JOIN  HADES.DBO.AGENCIAS A (NOLOCK) 
					                                ON CONVERT(INT, CC.AGENCIA) = CONVERT(INT, A.AGENCIA) 
                                                        AND CONVERT(INT, CC.BANCO) = CONVERT(INT, A.BANCO) ");


                sql.Append(@" WHERE CONTACORRENTEID = @CONTACORRENTEID ");
                contextQuery.Parameters.Add("@CONTACORRENTEID", SqlDbType.Int, contaCorrenteId);

                contextQuery.Command = sql.ToString();

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                if (contexto != null)
                {
                    contexto.Abandon();
                }
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    if (Convert.ToString(ex.Message).Contains("ERRO:"))
                    {
                        mensagem = ex.Message.Replace("ERRO: ", string.Empty);
                    }
                    else
                    {
                        mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine,
                            Convert.ToString(ex.Message));
                    }
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

        public bool PossuiContaVigentePor(DataContext contexto, int contaCorrenteId, string censo, int? regional)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"SELECT COUNT(*) 
                        FROM   [PrestacaoContas].[CONTACORRENTE] 
                        WHERE  ((@CENSO IS NOT NULL AND  CENSO = @CENSO )
                                  OR (@REGIONAL IS NOT NULL AND  REGIONALID = @REGIONAL ) )      
                                AND DATAFIM IS NULL  
                                AND CONTACORRENTEID <> @CONTACORRENTEID ";


            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@REGIONAL", SqlDbType.Int, regional);
            contextQuery.Parameters.Add("@CONTACORRENTEID", SqlDbType.Int, contaCorrenteId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool ehContaVigentePor(DataContext contexto, int contaCorrenteId, string censo, int? regional)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"SELECT COUNT(*) 
                        FROM   [PrestacaoContas].[CONTACORRENTE] 
                        WHERE  ((@CENSO IS NOT NULL AND  CENSO = @CENSO )
                                  OR (@REGIONAL IS NOT NULL AND  REGIONALID = @REGIONAL ) )      
                                AND (DATAFIM IS NULL OR DATAFIM > GETDATE())
                                AND CONTACORRENTEID = @CONTACORRENTEID ";


            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@REGIONAL", SqlDbType.Int, regional);
            contextQuery.Parameters.Add("@CONTACORRENTEID", SqlDbType.Int, contaCorrenteId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }        

        public int? ObtemContaCorrenteIdPor(string banco, string agencia, string conta)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            
            try
            {
                return ObtemContaCorrenteIdPor(contexto, banco, agencia, conta);
            }
            catch (Exception ex)
            {
                if (contexto != null)
                {
                    contexto.Abandon();
                }
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    if (Convert.ToString(ex.Message).Contains("ERRO:"))
                    {
                        mensagem = ex.Message.Replace("ERRO: ", string.Empty);
                    }
                    else
                    {
                        mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine,
                            Convert.ToString(ex.Message));
                    }
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
        }

        private int? ObtemContaCorrenteIdPor(DataContext contexto, string banco, string agencia, string conta)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT CONTACORRENTEID
                                            FROM PRESTACAOCONTAS.CONTACORRENTE (NOLOCK)
                                            WHERE CONTA = @CONTA
                                                    AND BANCO = @BANCO
                                                    AND AGENCIA = @AGENCIA ";

            contextQuery.Parameters.Add("@CONTA", SqlDbType.VarChar, conta);
            contextQuery.Parameters.Add("@BANCO", SqlDbType.VarChar, banco);
            contextQuery.Parameters.Add("@AGENCIA", SqlDbType.VarChar, agencia);

            return contexto.GetReturnValue<int?>(contextQuery);
        }

        public IList<Entidades.ContaCorrente> RetornaApenasAsContasCorrentesExistentes(IList<string> unidadesDeEnsino)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return RetornaApenasAsContasCorrentesExistentes(contexto, unidadesDeEnsino);
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
        }

        public IList<Entidades.ContaCorrente> RetornaApenasAsContasCorrentesExistentes(DataContext contexto, IList<string> unidadesDeEnsino)
        {
            ContextQuery contextQuery = new ContextQuery();

            var stringUnidadesDeEnsino = unidadesDeEnsino
                    .Select(s => "'" + s.Trim() + "'")
                    .Aggregate((c, n) => c + "," + n);

            contextQuery.Command = @" SELECT * FROM [LYCEUM].[PrestacaoContas].[ContaCorrente] CC (NOLOCK) WHERE CC.CENSO in (" + stringUnidadesDeEnsino + ") ";

            return contexto.TryToBindEntities<Entidades.ContaCorrente>(contextQuery).ToList();
        }
    }
}
