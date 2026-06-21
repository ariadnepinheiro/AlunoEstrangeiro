using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Query;
using Techne.Lyceum.RN.CartaoEstudante.DTO;
using Techne.Lyceum.RN.CartaoEstudante.Entity;

namespace Techne.Lyceum.RN.CartaoEstudante.Service
{
    public class CartaoService: SingletonBase<CartaoService>
    {
        private static readonly CartaoQuery duplicidadeQuery = CartaoQuery.Instancia;
        private static readonly ControleProcessamentoQuery controleProcessamentoQuery = ControleProcessamentoQuery.Instancia;
        private static readonly RetornoCartaoQuery retornoCartaoQuery = RetornoCartaoQuery.Instancia;
        private static readonly LogControleProcessamentoQuery logControleProcessamentoQuery = LogControleProcessamentoQuery.Instancia;
        private static readonly OperadoraQuery operadoraQuery = OperadoraQuery.Instancia;
        private static readonly TipoSituacaoCartaoQuery tipoSituacaoCartaoQuery = TipoSituacaoCartaoQuery.Instancia;
        private static readonly TipoCancelamentoQuery tipoCancelamentoQuery = TipoCancelamentoQuery.Instancia;
        public static readonly string NOMEPROCESSO_CARTAO = "CARTAO";

        CartaoService() { }

		public DadosPaginadosRequestDTO ObtemDadosPaginadosRequest()
        {
            DadosPaginadosRequestDTO dadosRequest = new DadosPaginadosRequestDTO();
            ControleProcessamento controleProcessamento =
                controleProcessamentoQuery.ObtemUltimaAtualizacaoPor(NOMEPROCESSO_CARTAO);

            dadosRequest.DtAtualizacaoInicial = controleProcessamento.DataInicioMovimento.Value;
            dadosRequest.DtAtualizacaoFinal = controleProcessamento.DataFimMovimento.Value;

            return dadosRequest;
        }

        public void Processa(RegistroCartaoDTO registroDTO)
        {
            RetornoCartao retornoCartao;
            TipoSituacaoCartao tipoSituacaoCartao;
            TipoCancelamento tipoCancelamento = null;
            Operadora operadora;

            try
            {
                ValidaCampos(registroDTO);

                tipoSituacaoCartao = tipoSituacaoCartaoQuery.ObtemTipoSituacaoCartaoPor(registroDTO.Cartao.StatusCartao);
                operadora = operadoraQuery.ObtemOperadoraPor("RIOCARD");

                if (registroDTO.Cartao.StatusCartao == "H")
                {
                    tipoCancelamento = tipoCancelamentoQuery.ObtemTipoCancelamentoPor(registroDTO.Cartao.CodCancel);

                    if (tipoCancelamento.Codigo == default(int))
                        throw new Exception(string.Format("Código de cancelamento \"{0}\" não encontrado!", registroDTO.Cartao.CodCancel));
                }

                retornoCartao = new RetornoCartao
                {
                    TipoSituacaoCartaoId = tipoSituacaoCartao.TipoSituacaoCartaoId,
                    OperadoraID = operadora.OperadoraId,
                    TipoCancelamentoId = (tipoCancelamento != null) ? (int?)tipoCancelamento.TipoCancelamentoId : null,
                    Aluno = registroDTO.Matricula.Matricula,
                    NumeroChip = registroDTO.Cartao.NrChip,
                    NumeroCartao = registroDTO.Cartao.NrRiocard,
                    NumeroLote = Convert.ToInt32(registroDTO.Cartao.NrLoteEntrega),
                    DataImpressao = registroDTO.Cartao.DtImpr,
                    IdBeneficiario = Convert.ToInt32(registroDTO.IdBeneficiario),
                    LocalImpressao = registroDTO.Cartao.LocalImpressao,
                    DataEntregaLote = registroDTO.Cartao.DtEntregaLote,
                    DataConfirmacaoEntrega = registroDTO.Cartao.DataConfirmacaoAluno
                };

                retornoCartaoQuery.InsereRetornoCartao(retornoCartao);
            }
            catch (Exception ex)
            {                  
                throw ex;
            }
        }

        private void ValidaCampos(RegistroCartaoDTO registroDTO)
        {
            string msgPadrao = "O campo \'{0}\' é obrigatório!";
            StringBuilder cartoesComErro = new StringBuilder();

            try
            {
                if (registroDTO.NumeroRegistro == default(long))
                    throw new Exception(string.Format(msgPadrao, "NÚMERO SEQUENCIAL DO REGISTRO"));

                if (registroDTO.Matricula != null)
                {
                    if (string.IsNullOrEmpty(registroDTO.Matricula.Matricula))
                        throw new Exception(string.Format(msgPadrao, "MATRÍCULA"));
                }

                if (string.IsNullOrEmpty(registroDTO.IdBeneficiario))
                    throw new Exception(string.Format(msgPadrao, "IDENTIFICADOR DO BENEFICIÁRIO"));

                if (registroDTO.Cartao != null)
                {
                    if (string.IsNullOrEmpty(registroDTO.Cartao.NrRiocard))
                        throw new Exception(string.Format(msgPadrao, "NÚMERO DO CARTÃO"));

                    if (string.IsNullOrEmpty(registroDTO.Cartao.StatusCartao))
                        throw new Exception(string.Format(msgPadrao, "STATUS DO CARTÃO"));

                    if ((registroDTO.Cartao.StatusCartao == "H") && (registroDTO.Cartao.CodCancel == default(int)))
                        throw new Exception("Para este status do cartão, um código de cancelamento deve ser informado!");
                }
                else
                {
                    throw new Exception(string.Format(msgPadrao, "CARTÃO"));
                }
            }
            catch (Exception ex)
            {                   
                throw ex;
            }
        }
    }
}
