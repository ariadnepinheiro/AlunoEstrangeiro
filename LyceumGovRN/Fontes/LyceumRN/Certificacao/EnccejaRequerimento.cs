using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Certificacao
{
    public class EnccejaRequerimento
    {
        public bool PossuiUnidadeCertificadoraPor(DataContext contexto, int unidadeCertificadoraId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM CertificacaoEscolar.ENCCEJAREQUERIMENTO (NOLOCK)
                                    WHERE UNIDADECERTIFICADORAID = @UNIDADECERTIFICADORAID ";

            contextQuery.Parameters.Add("@UNIDADECERTIFICADORAID", SqlDbType.Int, unidadeCertificadoraId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiMotivoIndeferidoPor(DataContext contexto, int motivoIndeferidoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM CertificacaoEscolar.ENCCEJAREQUERIMENTO (NOLOCK)
                                    WHERE MOTIVOINDEFERIDOID = @MOTIVOINDEFERIDOID ";

            contextQuery.Parameters.Add("@MOTIVOINDEFERIDOID", SqlDbType.Int, motivoIndeferidoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ListaEnccejaAlunoPor(string nome, string cpf, string numeroProtocolo, string statusSolicitacao, string usuario, int? unidadeCertificadorId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                if (!string.IsNullOrEmpty(statusSolicitacao))
                {
                    sql.Append(@"  		                                    
SELECT DISTINCT EA.ENCCEJAALUNOID,
		TC.DESCRICAO AS TIPOCERTIFICACAO,
		ER.ENCCEJAREQUERIMENTOID,
        CASE 
            WHEN (SELECT COUNT(1) AS POSSUI
				from CertificacaoEscolar.ENCCEJAREQUERIMENTO r
				inner join CertificacaoEscolar.ENCCEJAALUNO a on r.ENCCEJAALUNOID = a.ENCCEJAALUNOID
				where SITUACAOENCCEJAREQUERIMENTOid = 3	--Entregue
				and a.CPF = EA.CPF
				and r.TIPOCERTIFICACAOID =  ER.TIPOCERTIFICACAOID
                and r.ENCCEJAREQUERIMENTOid < er.ENCCEJAREQUERIMENTOID ) = 1 THEN 'Sim' 
            else '' 
		END AS 'POSSUISEGUNDAVIA',
		ER.PROTOCOLO,
		EA.NOME,
		ER.ANO,
		EA.NOMEPAI,
		EA.NOMEMAE,
		EA.CPF,
		EA.RG,
		EA.DATANASCIMENTO,
		EA.CELULAR, 
		EA.FIXOCELULAR, 
		EA.EMAIL,
		ER.DATASOLICITACAO,
		ER.SITUACAOENCCEJAREQUERIMENTOID,
		ER.MOTIVOINDEFERIDOID, 
		MI.DESCRICAO as MOTIVOINDEFERIDODESCRICAO,
		ER.DATAVERIFICACAO,
		'' as ENTREGUE,
		ER.DATAENTREGA,
		EA.ENDERECO AS LOGRADOURO,
		EA.BAIRRO AS BAIRRO,
		EA.NUMERO AS NUMERO,
		EA.COMPLEMENTO AS COMPLEMENTO,
		EA.MUNICIPIO AS MUNICIPIO,		                                      	                                       
		UN.UNIDADECERTIFICADORAID,
		UN.DESCRICAO AS UNIDADE,
		UN.ENDERECO AS ENDERECOUNIDADE,
		UN.NUMERO AS NUMEROUNIDADE,
		UN.COMPLEMENTO AS COMPLEMENTOUNIDADE,
		UN.MUNICIPIO AS MUNICIPIOUNIDADE
FROM [CERTIFICACAOESCOLAR].[ENCCEJAALUNO] EA (NOLOCK)	
		INNER JOIN [CERTIFICACAOESCOLAR].[ENCCEJAREQUERIMENTO] ER (NOLOCK)
				ON EA.ENCCEJAALUNOID = ER.ENCCEJAALUNOID
		LEFT JOIN [CERTIFICACAOESCOLAR].TIPOCERTIFICACAO TC (NOLOCK)
				ON TC.TIPOCERTIFICACAOID = ER.TIPOCERTIFICACAOID
		INNER JOIN [CERTIFICACAOESCOLAR].UNIDADECERTIFICADORA UN (NOLOCK)
				ON ER.UNIDADECERTIFICADORAID = UN.UNIDADECERTIFICADORAID
		INNER JOIN [CERTIFICACAOESCOLAR].[USUARIOUNIDADECERTIFICADORA] UUN (NOLOCK)
				ON UN.UNIDADECERTIFICADORAID = UUN.UNIDADECERTIFICADORAID				
		INNER JOIN [CERTIFICACAOESCOLAR].[SITUACAOENCCEJAREQUERIMENTO] SER (NOLOCK) 
				ON SER.SITUACAOENCCEJAREQUERIMENTOID = ER.SITUACAOENCCEJAREQUERIMENTOID		
		LEFT JOIN [CERTIFICACAOESCOLAR].[MOTIVOINDEFERIDO] MI (NOLOCK) 
				ON MI.MOTIVOINDEFERIDOID = ER.MOTIVOINDEFERIDOID		                            
WHERE UUN.USUARIO = @USUARIO 
                                         ");

                    if (Convert.ToInt32(statusSolicitacao) != 0)
                    {
                        sql.Append(@" AND ER.SITUACAOENCCEJAREQUERIMENTOID = @SITUACAOENCCEJAREQUERIMENTOID
                                     ");
                        contextQuery.Parameters.Add("@SITUACAOENCCEJAREQUERIMENTOID", SqlDbType.VarChar, statusSolicitacao);
                    }

                    if (unidadeCertificadorId != null && unidadeCertificadorId > 0)
                    {
                        sql.Append(@" AND UN.UNIDADECERTIFICADORAID = @UNIDADECERTIFICADORAID
                                    ");
                        contextQuery.Parameters.Add("@UNIDADECERTIFICADORAID", SqlDbType.Int, unidadeCertificadorId);
                    }

                    if (!string.IsNullOrEmpty(nome))
                    {
                        sql.Append(@" AND EA.NOME = @NOME 
                                    ");
                        contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, nome);
                    }

                    if (!string.IsNullOrEmpty(cpf))
                    {
                        sql.Append(@" AND EA.CPF = @CPF 
                                     ");
                        contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, cpf.Replace(".", "").Replace("-", "").ToString());
                    }

                    if (!string.IsNullOrEmpty(numeroProtocolo))
                    {
                        sql.Append(@" AND ER.PROTOCOLO = @PROTOCOLO 
                                    ");
                        contextQuery.Parameters.Add("@PROTOCOLO", SqlDbType.VarChar, numeroProtocolo);
                    }

                    sql.Append(@"ORDER BY ER.DATASOLICITACAO DESC ");

                    contextQuery.Command = sql.ToString();
                    contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuario);

                    dt = ctx.GetDataTable(contextQuery);
                }
            }
            catch (Exception ex)
            {
                ctx.Abandon();
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
                ctx.Dispose();
            }

            return dt;
        }

        public DataTable ListaEnccejaAlunoPor(string nome, string cpf, string numeroProtocolo, string statusSolicitacao, string usuario, int? unidadeCertificadorId, DateTime? dataIni, DateTime? dataFim)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                if (!string.IsNullOrEmpty(statusSolicitacao))
                {
                    sql.Append(@"  		                                    
SELECT DISTINCT EA.ENCCEJAALUNOID,
		TC.DESCRICAO AS TIPOCERTIFICACAO,
		ER.ENCCEJAREQUERIMENTOID,
        CASE 
            WHEN (SELECT COUNT(1) AS POSSUI
				from CertificacaoEscolar.ENCCEJAREQUERIMENTO r
				inner join CertificacaoEscolar.ENCCEJAALUNO a on r.ENCCEJAALUNOID = a.ENCCEJAALUNOID
				where SITUACAOENCCEJAREQUERIMENTOid = 3	--Entregue
				and a.CPF = EA.CPF
				and r.TIPOCERTIFICACAOID =  ER.TIPOCERTIFICACAOID
                and r.ENCCEJAREQUERIMENTOid < er.ENCCEJAREQUERIMENTOID ) = 1 THEN 'Sim' 
            else '' 
		END AS 'POSSUISEGUNDAVIA',
		ER.PROTOCOLO,
		EA.NOME,
		ER.ANO,
		EA.NOMEPAI,
		EA.NOMEMAE,
		EA.CPF,
		EA.RG,
		EA.DATANASCIMENTO,
		EA.CELULAR, 
		EA.FIXOCELULAR, 
		EA.EMAIL,
		ER.DATASOLICITACAO,
		ER.SITUACAOENCCEJAREQUERIMENTOID,
		ER.MOTIVOINDEFERIDOID, 
		MI.DESCRICAO as MOTIVOINDEFERIDODESCRICAO,
		ER.DATAVERIFICACAO,
		'' as ENTREGUE,
		ER.DATAENTREGA,
		EA.ENDERECO AS LOGRADOURO,
		EA.BAIRRO AS BAIRRO,
		EA.NUMERO AS NUMERO,
		EA.COMPLEMENTO AS COMPLEMENTO,
		EA.MUNICIPIO AS MUNICIPIO,		                                      	                                       
		UN.UNIDADECERTIFICADORAID,
		UN.DESCRICAO AS UNIDADE,
		UN.ENDERECO AS ENDERECOUNIDADE,
		UN.NUMERO AS NUMEROUNIDADE,
		UN.COMPLEMENTO AS COMPLEMENTOUNIDADE,
		UN.BAIRRO AS BAIRROUNIDADE,
		UN_MUN.NOME AS MUNICIPIOUNIDADE
FROM [CERTIFICACAOESCOLAR].[ENCCEJAALUNO] EA (NOLOCK)	
		INNER JOIN [CERTIFICACAOESCOLAR].[ENCCEJAREQUERIMENTO] ER (NOLOCK)
				ON EA.ENCCEJAALUNOID = ER.ENCCEJAALUNOID
		LEFT JOIN [CERTIFICACAOESCOLAR].TIPOCERTIFICACAO TC (NOLOCK)
				ON TC.TIPOCERTIFICACAOID = ER.TIPOCERTIFICACAOID
		INNER JOIN [CERTIFICACAOESCOLAR].UNIDADECERTIFICADORA UN (NOLOCK)
				ON ER.UNIDADECERTIFICADORAID = UN.UNIDADECERTIFICADORAID
		INNER JOIN MUNICIPIO UN_MUN (NOLOCK) 
				ON UN_MUN.CODIGO = UN.MUNICIPIO
		INNER JOIN [CERTIFICACAOESCOLAR].[USUARIOUNIDADECERTIFICADORA] UUN (NOLOCK)
				ON UN.UNIDADECERTIFICADORAID = UUN.UNIDADECERTIFICADORAID				
		INNER JOIN [CERTIFICACAOESCOLAR].[SITUACAOENCCEJAREQUERIMENTO] SER (NOLOCK) 
				ON SER.SITUACAOENCCEJAREQUERIMENTOID = ER.SITUACAOENCCEJAREQUERIMENTOID		
		LEFT JOIN [CERTIFICACAOESCOLAR].[MOTIVOINDEFERIDO] MI (NOLOCK) 
				ON MI.MOTIVOINDEFERIDOID = ER.MOTIVOINDEFERIDOID		                            
WHERE UUN.USUARIO = @USUARIO 
                                         ");

                    if (Convert.ToInt32(statusSolicitacao) != 0)
                    {
                        sql.Append(@" AND ER.SITUACAOENCCEJAREQUERIMENTOID = @SITUACAOENCCEJAREQUERIMENTOID
                                     ");
                        contextQuery.Parameters.Add("@SITUACAOENCCEJAREQUERIMENTOID", SqlDbType.VarChar, statusSolicitacao);
                    }

                    if (unidadeCertificadorId != null && unidadeCertificadorId > 0)
                    {
                        sql.Append(@" AND UN.UNIDADECERTIFICADORAID = @UNIDADECERTIFICADORAID
                                    ");
                        contextQuery.Parameters.Add("@UNIDADECERTIFICADORAID", SqlDbType.Int, unidadeCertificadorId);
                    }

                    if (!string.IsNullOrEmpty(nome))
                    {
                        sql.Append(@" AND EA.NOME = @NOME 
                                    ");
                        contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, nome);
                    }

                    if (!string.IsNullOrEmpty(cpf))
                    {
                        sql.Append(@" AND EA.CPF = @CPF 
                                     ");
                        contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, cpf.Replace(".", "").Replace("-", "").ToString());
                    }

                    if (!string.IsNullOrEmpty(numeroProtocolo))
                    {
                        sql.Append(@" AND ER.PROTOCOLO = @PROTOCOLO 
                                    ");
                        contextQuery.Parameters.Add("@PROTOCOLO", SqlDbType.VarChar, numeroProtocolo);
                    }

                    if (dataIni.HasValue)
                    {
                        sql.Append(@" AND ER.DATASOLICITACAO >= @DATAINI ");
                        contextQuery.Parameters.Add("@DATAINI", SqlDbType.DateTime, Convert.ToDateTime(dataIni.Value.ToString("yyyy-MM-dd 00:00:00")));
                    }

                    if (dataFim.HasValue)
                    {
                        sql.Append(@" AND ER.DATASOLICITACAO <= @DATAFIM ");
                        contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, Convert.ToDateTime(dataFim.Value.ToString("yyyy-MM-dd 23:59:59")));
                    }

                    sql.Append(@"ORDER BY ER.DATASOLICITACAO DESC ");

                    contextQuery.Command = sql.ToString();
                    contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuario);

                    dt = ctx.GetDataTable(contextQuery);
                }
            }
            catch (Exception ex)
            {
                ctx.Abandon();
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
                ctx.Dispose();
            }

            return dt;
        }

        private int ObtemSituacaoPor(DataContext contexto, int enccejaRequerimentoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" select SITUACAOENCCEJAREQUERIMENTOID
                                        from CertificacaoEscolar.ENCCEJAREQUERIMENTO
                                        WHERE ENCCEJAREQUERIMENTOID = @ENCCEJAREQUERIMENTOID";

                contextQuery.Parameters.Add("@ENCCEJAREQUERIMENTOID", SqlDbType.Int, enccejaRequerimentoId);

                return contexto.GetReturnValue<int>(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
                throw;
            }
            finally
            {
                contexto.Dispose();
            }
        }

        public ValidacaoDados Valida(DTOs.PainelCertificacao painelCertificacao)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (painelCertificacao.SituacaoEnccejaRequerimentoId <= 0)
            {
                mensagens.Add("Situação é obrigatória.");
            }
            else if (painelCertificacao.SituacaoEnccejaRequerimentoId == 1)
            {
                mensagens.Add("Situação é obrigatória. Marque: Emitido ou Indeferido.");
            }

            if (painelCertificacao.DataVerificacao.Date == DateTime.MinValue.Date)
            {
                mensagens.Add("Data da Situação é obrigatória.");
            }
            else
            {
                if (painelCertificacao.DataVerificacao.Date < painelCertificacao.DataSolicitacao.Date)
                {
                    string descricaoSituacao = "";
                    if (painelCertificacao.SituacaoEnccejaRequerimentoId == 2)
                        descricaoSituacao = "Data de Emissão";
                    else if (painelCertificacao.SituacaoEnccejaRequerimentoId == 4)
                        descricaoSituacao = "Data de Indeferimento";
                    else
                        descricaoSituacao = "Data da Situação";

                    mensagens.Add(String.Format("A {0} não pode ser menor do que a Data de Solicitação.", descricaoSituacao));
                }

                if (painelCertificacao.DataVerificacao.Date > DateTime.Now.Date)
                {
                    string descricaoSituacao = "";
                    if (painelCertificacao.SituacaoEnccejaRequerimentoId == 2)
                        descricaoSituacao = "Data de Emissão";
                    else if (painelCertificacao.SituacaoEnccejaRequerimentoId == 4)
                        descricaoSituacao = "Data de Indeferimento";
                    else
                        descricaoSituacao = "Data da Situação";

                    mensagens.Add(String.Format("A {0} não pode ser maior do que hoje.", descricaoSituacao));
                }
            }

            if (painelCertificacao.SituacaoEnccejaRequerimentoId == 3)
            {
                if (painelCertificacao.DataEntrega.Date == DateTime.MinValue.Date)
                {
                    mensagens.Add("Data de Entrega é obrigatória.");
                }
                else
                {
                    if (!painelCertificacao.Entregue)
                    {
                        mensagens.Add("Data de Entrega apenas pode ser informada para requerimento com marcação Entregue.");
                    }

                    if (painelCertificacao.DataEntrega.Date < painelCertificacao.DataSolicitacao.Date)
                    {
                        mensagens.Add("Data de Entrega não pode ser menor do que a Data de Solicitação.");
                    }

                    if (painelCertificacao.DataEntrega.Date < painelCertificacao.DataVerificacao.Date)
                    {
                        mensagens.Add("Data de Entrega não pode ser menor do que a Data de Emissão.");
                    }

                    if (painelCertificacao.DataEntrega.Date > DateTime.Now.Date)
                    {
                        mensagens.Add("Data de Entrega não pode ser maior do que a data de hoje.");
                    }
                }
            }
            else
            {
                if (painelCertificacao.DataEntrega.Date != DateTime.MinValue.Date)
                {
                    mensagens.Add("Data de Entrega apenas pode ser informada para a situação Entregue.");
                }
            }

            if (painelCertificacao.SituacaoEnccejaRequerimentoId == 4 && painelCertificacao.MotivoIndeferidoId == (int?)null)
            {
                mensagens.Add("É necessário informar o Motivo do Indeferimento.");
            }

            if (painelCertificacao.SituacaoEnccejaRequerimentoId != 4 && painelCertificacao.MotivoIndeferidoId > 0)
            {
                mensagens.Add("Não é possivel informar o Motivo do Indeferimento caso o requerimento não esteja com situação Indeferido.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                    var situacao = ObtemSituacaoPor(contexto, painelCertificacao.EnccejaRequerimentoId);

                    if (painelCertificacao.SituacaoEnccejaRequerimentoId == 4 && situacao != 1)
                    {
                        mensagens.Add("Somente podem ser indeferidas solicitações em que o status da solicitação estiverem em andamento");
                    }

                    if (painelCertificacao.SituacaoEnccejaRequerimentoId == 2 && situacao != 1)
                    {
                        mensagens.Add("Somente podem ser emitidas solicitações em que o status da solicitação estiverem em andamento");
                    }

                    if (painelCertificacao.SituacaoEnccejaRequerimentoId == 3 && situacao != 2)
                    {
                        mensagens.Add("Só é permitido marcar a opção entregue para os certificados com status emitido");
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

        public void Atualiza(DTOs.PainelCertificacao painelCertificacao)
        {
            DataContext contextoData = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {

                contextQuery.Command = @" UPDATE CertificacaoEscolar.ENCCEJAREQUERIMENTO
                                        SET    SITUACAOENCCEJAREQUERIMENTOID = @SITUACAOENCCEJAREQUERIMENTOID,                                                
                                               MOTIVOINDEFERIDOID = @MOTIVOINDEFERIDOID, 
                                               DATAVERIFICACAO = @DATAVERIFICACAO,
                                               DATAENTREGA = @DATAENTREGA, 
                                               USUARIOVERIFICACAO = @USUARIOVERIFICACAO  
                                        WHERE  ENCCEJAREQUERIMENTOID = @ENCCEJAREQUERIMENTOID ";


                contextQuery.Parameters.Add("@ENCCEJAREQUERIMENTOID", SqlDbType.Int, painelCertificacao.EnccejaRequerimentoId);
                contextQuery.Parameters.Add("@SITUACAOENCCEJAREQUERIMENTOID", SqlDbType.Int, painelCertificacao.SituacaoEnccejaRequerimentoId);
                contextQuery.Parameters.Add("@MOTIVOINDEFERIDOID", SqlDbType.Int, painelCertificacao.MotivoIndeferidoId != 0 ? painelCertificacao.MotivoIndeferidoId : (object)DBNull.Value);
                contextQuery.Parameters.Add("@DATAVERIFICACAO", SqlDbType.DateTime, painelCertificacao.DataVerificacao);
                contextQuery.Parameters.Add("@DATAENTREGA", SqlDbType.DateTime, painelCertificacao.DataEntrega != DateTime.MinValue ? painelCertificacao.DataEntrega : (object)DBNull.Value);
                contextQuery.Parameters.Add("@USUARIOVERIFICACAO", SqlDbType.VarChar, painelCertificacao.UsuarioId);

                contextoData.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contextoData.Abandon();
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
                contextoData.Dispose();
            }

        }

        public bool PossuiTipoCertificacaoPor(DataContext contexto, int tipoCertificacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM CertificacaoEscolar.ENCCEJAREQUERIMENTO (NOLOCK)
                                    WHERE TIPOCERTIFICACAOID = @TIPOCERTIFICACAOID ";

            contextQuery.Parameters.Add("@TIPOCERTIFICACAOID", SqlDbType.Int, tipoCertificacaoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

    }
}
