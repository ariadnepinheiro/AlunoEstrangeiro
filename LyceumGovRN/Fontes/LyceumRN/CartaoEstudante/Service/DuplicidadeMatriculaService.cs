using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.DTO;
using Techne.Lyceum.RN.CartaoEstudante.Entity;
using Techne.Lyceum.RN.CartaoEstudante.Query;
using Techne.Lyceum.RN.CartaoEstudante.DTO.Filter;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.CartaoEstudante.Service
{
    public class DuplicidadeMatriculaService: SingletonBase<DuplicidadeMatriculaService>
    {
        private static readonly DuplicidadeQuery duplicidadeQuery = DuplicidadeQuery.Instancia;
        private static readonly ControleProcessamentoQuery controleProcessamentoQuery = ControleProcessamentoQuery.Instancia;
        private static readonly LogControleProcessamentoQuery logControleProcessamentoQuery = LogControleProcessamentoQuery.Instancia;
        private static readonly OperadoraQuery operadoraQuery = OperadoraQuery.Instancia;
        private static readonly AlunoQuery alunoQuery = AlunoQuery.Instancia;
        public static readonly string NOMEPROCESSO_DUPLICIDADE = "DUPLICIDADESMATRICULA";
		
		DuplicidadeMatriculaService() { }

        public DadosPaginadosRequestDTO ObtemDadosPaginadosRequest()
        {
            DadosPaginadosRequestDTO dadosRequest = new DadosPaginadosRequestDTO();
            ControleProcessamento controleProcessamento =
                controleProcessamentoQuery.ObtemUltimaAtualizacaoPor(NOMEPROCESSO_DUPLICIDADE);

            dadosRequest.DtAtualizacaoInicial = controleProcessamento.DataInicioMovimento.Value;
            dadosRequest.DtAtualizacaoFinal = controleProcessamento.DataFimMovimento.Value;

            return dadosRequest;
        }

        public string obtemIdBeneficiario(string aluno)
        {
            try
            {
                return duplicidadeQuery.ObtemIdBeneficiario(aluno)
                              .ToString();
            }
            catch (NullReferenceException)
            {
                throw new Exception(string.Format("Não existem Matriculas duplicadas para os parâmetros de pesquisa informados."));
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            
        }


        public List<DuplicidadeDTO> ListaDuplicidades(MatriculaDuplicadaFilterDTO filtro)
        {
            return duplicidadeQuery.ObtemDuplicidades(filtro.Aluno, filtro.UnidadeEnsino, filtro.Regional)
                               .ToList<DuplicidadeDTO>();
        }

        public void Processa(RegistroDuplicidadeMatriculaDTO registroDTO)
        {
            Operadora operadora;
            Duplicidade duplicidade;

            try
            {
                ValidaCampos(registroDTO);

                operadora = operadoraQuery.ObtemOperadoraPor("RIOCARD");

                foreach (var matricula in registroDTO.Matriculas)
                {
                    duplicidade = new Duplicidade
                    {
                        IdBeneficiario = Convert.ToInt32(registroDTO.IdBeneficiario),
                        NumeroRegistro = registroDTO.NumeroRegistro,
                        Aluno = matricula.Matricula,
                        FlagMatriculaPrincipal = matricula.FlagMatriculaPrincipal,
                        DataFlagMatriculaPrincipal = matricula.DtFlagMatriculaPrincipal,
                        DataAtualizacao = registroDTO.DtAtualizacao,
                        OperadoraId = operadora.OperadoraId
                    };

                    duplicidadeQuery.InsereDuplicidade(duplicidade);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ValidaCampos(RegistroDuplicidadeMatriculaDTO registroDTO)
        {
            string msgPadrao = "O campo \'{0}\' é obrigatório!";
            StringBuilder matriculasComErro = new StringBuilder();

            try
            {
                if (registroDTO.NumeroRegistro == default(long))
                    throw new Exception(string.Format(msgPadrao, "NÚMERO SEQUENCIAL DO REGISTRO"));

                if (string.IsNullOrEmpty(registroDTO.IdBeneficiario))
                    throw new Exception(string.Format(msgPadrao, "IDENTIFICADOR DO BENEFICIÁRIO"));

                if (registroDTO.DtAtualizacao == default(DateTime))
                    throw new Exception(string.Format(msgPadrao, "DATA E HORA DA ÚLTIMA ATUALIZAÇÃO"));

                string strComma = ", ";

                foreach (var matricula in registroDTO.Matriculas)
                {
                    if ((string.IsNullOrEmpty(matricula.Matricula))
                        || (string.IsNullOrEmpty(matricula.FlagMatriculaPrincipal))
                        || ((matricula.FlagMatriculaPrincipal != "S") && (matricula.FlagMatriculaPrincipal != "N"))
                        || (matricula.DtFlagMatriculaPrincipal == default(DateTime)))
                    {   
                        if (matriculasComErro.Length > 0)
                            matriculasComErro.Append(strComma);

                        matriculasComErro.Append(matricula.Matricula);
                    }   
                }

                if (matriculasComErro.Length > 0)
                {
                    throw new Exception(
                        string.Format(
                            "As seguintes matrículas se encontram com erro: {0}",
                            matriculasComErro.ToString()
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
