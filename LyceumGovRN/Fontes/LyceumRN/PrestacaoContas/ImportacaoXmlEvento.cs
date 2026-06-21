using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class ImportacaoXmlEvento
    {
        public DataTable ListaItensXmlPor(int eventoId, string censo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT X.IMPORTACAOXMLEVENTOID,
	                                           X.EVENTOID,
	                                           X.DESCRICAO AS ITEM,
	                                           ISNULL(P.NCM, X.NCM) AS NCM,
                                               UM.UNIDADEMEDIDAID,
	                                           UM.DESCRICAO AS UNIDADEMEDIDA,
	                                           CONVERT(VARCHAR, P.CODIGOFGV) + ' - '+ P.NOME AS CODIGOFGV,
                                               P.CODIGOFGV as CODIGO_FGV,
	                                           X.QUANTIDADE,
	                                           x.VALORUNITARIO,
	                                           (X.QUANTIDADE * X.VALORUNITARIO) AS VALORPAGO,
	                                           VM.VALORMAXIMO AS VALORFGV,
	                                           CASE
			                                        WHEN VM.VALORMAXIMO IS NOT NULL THEN  (VM.VALORMAXIMO - X.VALORUNITARIO) * -1
			                                        ELSE NULL
		                                       END DIFERENCA,
	                                           CASE
			                                        WHEN VM.VALORMAXIMO IS NOT NULL THEN  convert(decimal(20,2), ((VM.VALORMAXIMO - X.VALORUNITARIO) / VM.VALORMAXIMO) * 100 * -1)
			                                        ELSE NULL
		                                        END PORCENTAGEMDIFERENCA,
		                                        CASE
			                                        WHEN x.DATAANALISE IS NOT NULL AND P.PRODUTOSERVICOID IS NOT NULL THEN  'Permitido'
			                                        when x.DATAANALISE IS NOT NULL AND P.PRODUTOSERVICOID IS NULL THEN  'Não Permitido'
			                                        ELSE 'Não Analisado'
		                                        END NAOPERMITIDO
                                        FROM PrestacaoContas.IMPORTACAOXMLEVENTO x (nolock)
	                                        inner join PrestacaoContas.EVENTO E (NOLOCK)
		                                        ON x.EVENTOID = E.EVENTOID
	                                        LEFT JOIN PrestacaoContas.PRODUTOSERVICO P (NOLOCK)
		                                        ON X.PRODUTOSERVICOID = P.PRODUTOSERVICOID
	                                        LEFT JOIN PrestacaoContas.UNIDADEMEDIDA UM (nolock)
		                                        on um.UNIDADEMEDIDAID = P.UNIDADEMEDIDAID
	                                        LEFT JOIN PrestacaoContas.PRODUTOSERVICOVALORMAXIMO vm (nolock)
		                                        ON VM.PRODUTOSERVICOID = P.PRODUTOSERVICOID
			                                        AND REGIAOFGVID = (SELECT REGIAOFGVID 
								                                        FROM PrestacaoContas.REGIAOFGV__MUNICIPIO rm
									                                        inner join LY_UNIDADE_ENSINO ue on rm.MUNICIPIOID = ue.MUNICIPIO
								                                        where ue.UNIDADE_ENS = @CENSO)
			                                        AND MONTH(e.DATAPAGAMENTO) BETWEEN MONTH(VM.DATAINICIO) AND MONTH(VM.DATAFIM)
                                        WHERE x.EVENTOID = @EVENTOID 
                                        ORDER BY X.DESCRICAO";

                contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);
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

        public void RemoveTodos(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  DELETE PrestacaoContas.IMPORTACAOXMLEVENTO
                                       WHERE EVENTOID = @EVENTOID ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

            contexto.ApplyModifications(contextQuery);
        }

        public void Insere(DataContext contexto, Entidades.ImportacaoXmlEvento importacaoXmlEvento)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO PrestacaoContas.IMPORTACAOXMLEVENTO
                                           (EVENTOID
                                           ,PRODUTOSERVICOID
                                           ,NUMEROITEM
                                           ,DESCRICAO
                                           ,NCM
                                           ,QUANTIDADE
                                           ,VALORUNITARIO
                                           ,DATAIMPORTACAO           
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@EVENTOID, 
                                           @PRODUTOSERVICOID,
                                           @NUMEROITEM, 
                                           @DESCRICAO, 
                                           @NCM, 
                                           @QUANTIDADE, 
                                           @VALORUNITARIO, 
                                           @DATAIMPORTACAO,           
                                           @USUARIOID, 
                                           @DATACADASTRO,
                                           @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, importacaoXmlEvento.EventoId);
            contextQuery.Parameters.Add("@PRODUTOSERVICOID", SqlDbType.Int, importacaoXmlEvento.ProdutoServicoId);
            contextQuery.Parameters.Add("@NUMEROITEM", SqlDbType.VarChar, importacaoXmlEvento.NumeroItem);
            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, importacaoXmlEvento.Descricao);
            contextQuery.Parameters.Add("@NCM", SqlDbType.VarChar, importacaoXmlEvento.Ncm);
            contextQuery.Parameters.Add("@QUANTIDADE", SqlDbType.Int, importacaoXmlEvento.Quantidade);
            contextQuery.Parameters.Add("@VALORUNITARIO", SqlDbType.Decimal, importacaoXmlEvento.ValorUnitario);
            contextQuery.Parameters.Add("@DATAIMPORTACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, importacaoXmlEvento.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public bool PossuiItensNaoAnalisadosPor(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" 	SELECT COUNT(1) 
                                    FROM PrestacaoContas.IMPORTACAOXMLEVENTO (NOLOCK)
                                    WHERE EVENTOID = @EVENTOID
										AND DATAANALISE IS NULL ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public ValidacaoDados ValidaAnalise(int importacaoXmlEventoId, string ncm, int unidadeMedidaId, string codigoFgv, string usuarioId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ProdutoServico rnProdutoServico = new ProdutoServico();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (importacaoXmlEventoId <= 0)
            {
                mensagens.Add("Campo CÓDIGO é obrigatório.");
            }

            if (ncm.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NCM é obrigatório.");
            }

            if (unidadeMedidaId <= 0)
            {
                mensagens.Add("Campo UNIDADE DE MEDIDA é obrigatório.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se foi encontrado codigo fgv
                    if (!codigoFgv.IsNullOrEmptyOrWhiteSpace())
                    {
                        //Verifica se o codigo fgv é permitido para o ncm
                        if (!rnProdutoServico.EhCodigoFgvValidoPor(contexto, ncm, unidadeMedidaId, codigoFgv))
                        {
                            mensagens.Add("Este CODIGO FGV não é permitido para esse ncm / unidade de medida.");
                        }

                        //Busca codigo ProdutoServiço id pelo codigoFgv e unidadeMedidaId
                        int? produtoServicoId = rnProdutoServico.ObtemProdutoServicoIdPor(contexto, codigoFgv, unidadeMedidaId);

                        //Verifica se existe produto para o codigo FGV e unidade de medida
                        if (produtoServicoId == null || produtoServicoId <= 0)
                        {
                            mensagens.Add("Erro ao buscar o codigo pra o FGV e unidade de medida.");
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
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void SalvaAnalise(int importacaoXmlEventoId, string ncm, int unidadeMedidaId, string codigoFgv, string usuarioId)
        {
            ProdutoServico rnProdutoServico = new ProdutoServico();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            int? produtoServicoId = null;

            try
            {
                //Verifica se foi encontrado codigo fgv
                if (!codigoFgv.IsNullOrEmptyOrWhiteSpace())
                {
                    //Busca codigo ProdutoServiço id pelo codigoFgv e unidadeMedidaId
                    produtoServicoId = rnProdutoServico.ObtemProdutoServicoIdPor(contexto, codigoFgv, unidadeMedidaId);
                }

                //Atualiza campos do XML
                this.AtualizaDadosProduto(contexto, importacaoXmlEventoId, ncm, produtoServicoId, usuarioId);
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

        private void AtualizaDadosProduto(DataContext contexto, int importacaoXmlEventoId, string ncm, int? produtoServicoId, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  UPDATE PrestacaoContas.IMPORTACAOXMLEVENTO
                                       SET NCM = @NCM,
                                           PRODUTOSERVICOID = @PRODUTOSERVICOID,
                                           USUARIOANALISADOR = @USUARIOANALISADOR,
                                           DATAANALISE = @DATAANALISE
                                    WHERE IMPORTACAOXMLEVENTOID = @IMPORTACAOXMLEVENTOID ";

            contextQuery.Parameters.Add("@IMPORTACAOXMLEVENTOID", SqlDbType.Int, importacaoXmlEventoId);
            contextQuery.Parameters.Add("@NCM", SqlDbType.VarChar, ncm);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuario);
            contextQuery.Parameters.Add("@USUARIOANALISADOR", SqlDbType.VarChar, usuario);
            contextQuery.Parameters.Add("@DATAANALISE", SqlDbType.DateTime, DateTime.Now);

            //Verifica se é um item permitido (que teve seus dados encontados
            if (produtoServicoId != null && produtoServicoId > 0)
            {
                contextQuery.Parameters.Add("@PRODUTOSERVICOID", SqlDbType.Int, produtoServicoId);
            }
            else
            {
                contextQuery.Parameters.Add("@PRODUTOSERVICOID", SqlDbType.Int, DBNull.Value);
            }

            contexto.ApplyModifications(contextQuery);
        }
    }
}
