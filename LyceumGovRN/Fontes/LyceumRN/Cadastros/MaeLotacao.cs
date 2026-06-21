using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Cadastros.DTOs;
using System.Data.SqlClient;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Web;

namespace Techne.Lyceum.RN.Cadastros
{
    public class MaeLotacao
    {
        public bool PossuiOutraLotacaoAbertaPor(DataContext contexto, int maeInscricaoId, string censo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                        FROM Cadastros.MAE_LOTACAO (NOLOCK)
                                        WHERE MAE_INSCRICAOID = @MAE_INSCRICAOID
											  AND CENSO <> @CENSO
											  AND (DATAFIM IS NULL OR DATAFIM > GETDATE()) ";

            contextQuery.Parameters.Add("@MAE_INSCRICAOID", SqlDbType.Int, maeInscricaoId);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiLotacaoPor(DataContext contexto, int maeInscricaoId, string censo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                        FROM Cadastros.MAE_LOTACAO (NOLOCK)
                                        WHERE MAE_INSCRICAOID = @MAE_INSCRICAOID
                                              AND CENSO = @CENSO ";

            contextQuery.Parameters.Add("@MAE_INSCRICAOID", SqlDbType.Int, maeInscricaoId);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;

        }

        public DataTable ListaLotacaoPor(string cpf)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT l.MAE_INSCRICAOID,
												L.MAE_LOTACAOID,
                                                CENSO ,
									            UE.NOME_COMP as ESCOLA ,
									            DATAINICIO ,
									            DATAFIM ,
									            l.BANCO,
									            B.NOME AS BANCODESCRICAO,
									            l.AGENCIA ,
									            A.NOME as AGENCIADESCRICAO,
									            CONTACORRENTE,
												L.MAE_MOTIVODESLIGAMENTOID,
												M.DESCRICAO AS MOTIVODESLIGAMENTO,
												L.DESCRICAOOUTROS,
												CASE
													WHEN (SELECT COUNT(1) 
														FROM HADES..HD_USUARIO 
														WHERE PRIVILEGIADO = 's'
														AND USUARIO = @USUARIOID ) > 0 THEN 1
													WHEN (SELECT COUNT(1) 
														FROM LY_USUARIO_UNIDADE_FIS UU
														WHERE USUARIO = @USUARIOID
														AND UU.UNIDADE_FIS = CENSO ) > 0 THEN 1
													ELSE 0
												END PODEALTERAR
							            FROM Cadastros.MAE_INSCRICAO I (NOLOCK)
												INNER JOIN	Cadastros.MAE_LOTACAO L (NOLOCK)
													  ON  I.MAE_INSCRICAOID = L.MAE_INSCRICAOID
									            INNER JOIN LY_UNIDADE_ENSINO UE 
											            ON L.CENSO = UE.UNIDADE_ENS
									            LEFT JOIN  HADES.DBO.BANCOS B (NOLOCK) 
											            ON CONVERT(INT, L.BANCO) = CONVERT(INT, B.BANCO)
									            LEFT JOIN  HADES.DBO.AGENCIAS A (NOLOCK) 
											            ON CONVERT(VARCHAR, L.AGENCIA) = CONVERT(VARCHAR, A.AGENCIA) 
											            AND CONVERT(INT, L.BANCO) = CONVERT(INT, A.BANCO)
												LEFT JOIN Cadastros.MAE_MOTIVODESLIGAMENTO M
														ON M.MAE_MOTIVODESLIGAMENTOID = L.MAE_MOTIVODESLIGAMENTOID
							            WHERE CPF = @CPF
                                        ORDER BY DATAINICIO DESC ";

                contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, cpf);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, HttpContext.Current.User.Identity.Name);

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

        public ValidacaoDados ValidaDadosBancarios(MaeDadosBancarios dados, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            MaeInscricao rnMaeInscricao = new MaeInscricao();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dados == null)
            {
                return validacaoDados;
            }

            if (dados.MaeInscricaoId <= 0)
            {
                mensagens.Add("Campo INSCRIÇÃO é obrigatório.");
            }
            if (!cadastro)
            {
                if (dados.MaeLotacaoId <= 0)
                {
                    mensagens.Add("Campo CODIGO DA LOTAÇÃO é obrigatório.");
                }
            }
            else
            {
                if (dados.Censo.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo CENSO é obrigatório.");
                }
            }

            if (dados.DataInicio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INÍCIO é obrigatório.");
            }
            else
            {
                if (dados.DataFim != null && dados.DataFim != DateTime.MinValue)
                {
                    if (dados.DataInicio > dados.DataFim)
                    {
                        mensagens.Add("A DATA INÍCIO não pode ser maior que a DATA FIM.");
                    }

                    if (dados.MaeMotivoDesligamentoId == null || dados.MaeMotivoDesligamentoId <= 0)
                    {
                        mensagens.Add("Campo MOTIVO DESLIGAMENTO é obrigatório.");
                    }
                    else
                    {
                        if (dados.MaeMotivoDesligamentoId == 1)
                        {
                            if (dados.DescricaoOutros.IsNullOrEmptyOrWhiteSpace())
                            {
                                mensagens.Add("Campo DESCRICAO MOTIVO DESLIGAMENTO é obrigatório para o motivo OUTROS.");
                            }
                        }
                        else
                        {
                            dados.DescricaoOutros = null;
                        }
                    }
                }
                else
                {
                    dados.MaeMotivoDesligamentoId = null;
                    dados.DescricaoOutros = null;
                }
            }

            if (dados.Banco.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo BANCO é obrigatório.");
            }

            if (dados.Agencia.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo AGÊNCIA é obrigatório.");
            }

            if (dados.ContaCorrente.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CONTA CORRENTE é obrigatório.");
            }

            if (dados.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se a inscricao foi habilitada
                    if (!rnMaeInscricao.EhHabilitadoPor(contexto, dados.MaeInscricaoId))
                    {
                        mensagens.Add("Esta inscrição não pode ser alterada pois nao foi habilitada.");
                    }


                    if (dados.DataFim == null)
                    {
                        if (PossuiLotacaoSemDataFimPor(contexto, dados.MaeInscricaoId))
                        {
                            mensagens.Add("Já existe uma LOTAÇÃO SEM DATA FIM para essa inscrição.");
                        }
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
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void AtualizaDadosBancarios(MaeDadosBancarios dados)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Cadastros.MAE_LOTACAO
	                                        SET DATAFIM = @DATAFIM,
		                                        BANCO = @BANCO,
		                                        AGENCIA = @AGENCIA,
		                                        CONTACORRENTE = @CONTACORRENTE,
		                                        MAE_MOTIVODESLIGAMENTOID = @MAE_MOTIVODESLIGAMENTOID,
		                                        DESCRICAOOUTROS = @DESCRICAOOUTROS,
		                                        USUARIOID = @USUARIOID,
		                                        DATAALTERACAO = @DATAALTERACAO
                                        WHERE MAE_LOTACAOID = @MAE_LOTACAOID ";

                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, dados.DataFim == null || dados.DataFim == DateTime.MinValue ? (object)DBNull.Value : dados.DataFim);
                contextQuery.Parameters.Add("@BANCO", SqlDbType.VarChar, dados.Banco);
                contextQuery.Parameters.Add("@AGENCIA", SqlDbType.VarChar, dados.Agencia);
                contextQuery.Parameters.Add("@CONTACORRENTE", SqlDbType.VarChar, dados.ContaCorrente);
                contextQuery.Parameters.Add("@MAE_MOTIVODESLIGAMENTOID", SqlDbType.Int, dados.MaeMotivoDesligamentoId == null || dados.MaeMotivoDesligamentoId <= 0 ? (object)DBNull.Value : dados.MaeMotivoDesligamentoId);
                contextQuery.Parameters.Add("@DESCRICAOOUTROS", SqlDbType.VarChar, dados.DescricaoOutros);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dados.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@MAE_LOTACAOID", SqlDbType.Int, dados.MaeLotacaoId);

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

        public bool PossuiMotivoDesligamentoPor(DataContext contexto, int maeMotivoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM Cadastros.MAE_LOTACAO (NOLOCK)
                                        WHERE MAE_MOTIVODESLIGAMENTOID = @MAE_MOTIVODESLIGAMENTOID ";

            contextQuery.Parameters.Add("@MAE_MOTIVODESLIGAMENTOID", SqlDbType.Int, maeMotivoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void InsereDadosBancarios(MaeDadosBancarios dados)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {

                contextQuery.Command = @" INSERT INTO Cadastros.MAE_LOTACAO
                                           (MAE_INSCRICAOID
                                           ,CENSO
                                           ,DATAINICIO  
                                           ,DATAFIM 
		                                   ,BANCO
		                                   ,AGENCIA
		                                   ,CONTACORRENTE
		                                   ,MAE_MOTIVODESLIGAMENTOID
		                                   ,DESCRICAOOUTROS
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@MAE_INSCRICAOID, 
                                           @CENSO, 
                                           @DATAINICIO,
                                           @DATAFIM, 
                                           @BANCO,  
                                           @AGENCIA,  
                                           @CONTACORRENTE,
		                                   @MAE_MOTIVODESLIGAMENTOID,
		                                   @DESCRICAOOUTROS,
                                           @USUARIOID, 
                                           @DATACADASTRO, 
                                           @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@MAE_INSCRICAOID", SqlDbType.Int, dados.MaeInscricaoId);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, dados.Censo);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, dados.DataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, dados.DataFim);
                contextQuery.Parameters.Add("@BANCO", SqlDbType.VarChar, dados.Banco);
                contextQuery.Parameters.Add("@AGENCIA", SqlDbType.VarChar, dados.Agencia);
                contextQuery.Parameters.Add("@CONTACORRENTE", SqlDbType.VarChar, dados.ContaCorrente);
                contextQuery.Parameters.Add("@MAE_MOTIVODESLIGAMENTOID", SqlDbType.Int, dados.MaeMotivoDesligamentoId == null || dados.MaeMotivoDesligamentoId <= 0 ? (object)DBNull.Value : dados.MaeMotivoDesligamentoId);
                contextQuery.Parameters.Add("@DESCRICAOOUTROS", SqlDbType.VarChar, dados.DescricaoOutros);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dados.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

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

        public void Insere(DataContext contexto, MaeDadosAnalise dados)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Cadastros.MAE_LOTACAO
                                           (MAE_INSCRICAOID
                                           ,CENSO
                                           ,DATAINICIO           
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@MAE_INSCRICAOID, 
                                           @CENSO, 
                                           @DATAINICIO,      
                                           @USUARIOID, 
                                           @DATACADASTRO, 
                                           @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@MAE_INSCRICAOID", SqlDbType.Int, dados.MaeInscricaoId);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, dados.Censo);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, dados.DataInicio);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dados.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void Atualiza(DataContext contexto, MaeDadosAnalise dados)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Cadastros.MAE_LOTACAO
	                                        SET DATAINICIO = @DATAINICIO,
                                                USUARIOID = @USUARIOID,
                                                DATACADASTRO = @DATACADASTRO, 
                                                DATAALTERACAO = @DATAALTERACAO
                                        WHERE MAE_INSCRICAOID = @MAE_INSCRICAOID
                                              AND CENSO = @CENSO ";

            contextQuery.Parameters.Add("@MAE_INSCRICAOID", SqlDbType.Int, dados.MaeInscricaoId);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, dados.Censo);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, dados.DataInicio);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dados.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public bool PossuiLotacaoSemDataFimPor(DataContext contexto, int maeInscricaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM Cadastros.MAE_LOTACAO (NOLOCK)
                                        WHERE MAE_INSCRICAOID = @MAE_INSCRICAOID
                                              AND DATAFIM IS NULL ";

            contextQuery.Parameters.Add("@MAE_INSCRICAOID", SqlDbType.Int, maeInscricaoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

    }
}
