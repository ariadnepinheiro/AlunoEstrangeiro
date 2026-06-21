using System;
using Techne.Lyceum.RN.CartaoEstudante.Query;
using Techne.Lyceum.RN.CartaoEstudante.DTO;
using Techne.Lyceum.RN.CartaoEstudante.Entity;
using System.Text;
using System.Collections.Generic;

namespace Techne.Lyceum.RN.CartaoEstudante.Service
{
    public class RegistrosRetornoService: SingletonBase<RegistrosRetornoService>
    {
        private static readonly RegistrosRetornoQuery registrosRetornoQuery = RegistrosRetornoQuery.Instancia;
        private static readonly RetornoQuery retornoQuery = RetornoQuery.Instancia;
        private static readonly RetornoCriticaQuery retornoCriticaQuery = RetornoCriticaQuery.Instancia;
        private static readonly CriticaQuery criticaQuery = CriticaQuery.Instancia;
        private static readonly OperadoraQuery operadoraQuery = OperadoraQuery.Instancia;
        public static readonly string NOMEPROCESSO_REGISTROS_RETORNO = "REGISTROSRETORNO";

        RegistrosRetornoService() { }

        public long[] ListaSolicitacoesRioCard()
        {
            Operadora operadora = operadoraQuery.ObtemOperadoraPor("RIOCARD");
            return registrosRetornoQuery.ListaSolicitacoes(operadora.OperadoraId);
        }

        public void Processa(RegistrosRetornoResponseDTO response)
        {           
            Retorno retorno = new Retorno();
            Operadora operadora = operadoraQuery.ObtemOperadoraPor("RIOCARD");
            IList<TipoRetornoCritica> criticas = new List<TipoRetornoCritica>();
            bool possuiRetornoInvalido = false;

            try
            {
                retorno.RemessaId = response.IdRemessa;
                retorno.OperadoraId = operadora.OperadoraId;
                retorno.IdBeneficiario = response.IdBeneficiario;
                retorno.SituacaoProcessamento = response.StProc;

                if (response.DtProc.HasValue)
                    retorno.DataProcessamento = response.DtProc.Value;

                if (retorno.SituacaoProcessamento != criticaQuery.Erro_Nao_Identificado_Na_Aplicacao)
                {
                    ValidaCampos(response);

                    if (response.Criticas.Count > 0)
                    {
                        foreach (var critica in response.Criticas)
                        {
                            criticas.Add(
                                new TipoRetornoCritica
                                {
                                    CodigoCritica = critica.Codigo
                                }
                            );
                        }
                    }
                }
                else
                    possuiRetornoInvalido = retornoQuery.PossuiRetornoPor(
                        retorno.RemessaId, 
                        criticaQuery.Erro_Nao_Identificado_Na_Aplicacao
                    );

                if (!possuiRetornoInvalido)
                    registrosRetornoQuery.Insere(retorno, criticas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ValidaCampos(RegistrosRetornoResponseDTO response)
        {
            string msgPadrao = "O campo \'{0}\' é obrigatório!";
            StringBuilder criticasNaoCadastradas = new StringBuilder();

            try
            {
                if (response.IdRemessa == default(long)) 
                    throw new Exception(string.Format(msgPadrao, "IDENTIFICADOR DA REMESSA"));

                if (string.IsNullOrEmpty(response.StProc))
                    throw new Exception(string.Format(msgPadrao, "SITUAÇAO DE PROCESSAMENTO"));                    

                if ((response.StProc == "2") && (response.Criticas.Count == 0))
                    throw new Exception("Para esta situação de processamento, uma crítica deve ser informada!");

                string strComma = ", ";

                foreach (CriticaDTO critica in response.Criticas)
                {
                    if (criticaQuery.ObtemCriticaPor(critica.Codigo) == null)
                    {
                        if (criticasNaoCadastradas.Length > 0)
                            criticasNaoCadastradas.Append(strComma);

                        criticasNaoCadastradas.Append(critica.Codigo);
                    }   
                }

                if (criticasNaoCadastradas.Length > 0)
                {
                    throw new Exception(
                        string.Format(
                            "Os seguintes códigos de críticas não foram localizados no banco de dados: {0}",
                            criticasNaoCadastradas.ToString()
                        )
                    );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
