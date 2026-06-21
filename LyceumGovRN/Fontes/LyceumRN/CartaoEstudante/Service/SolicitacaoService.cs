using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Query;
using Techne.Lyceum.RN.CartaoEstudante.Entity;
using Techne.Lyceum.RN.CartaoEstudante.DTO;
using Techne.Lyceum.RN.CartaoEstudante.Enum;
using Techne.Lyceum.RN.CartaoEstudante.Domain;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.CartaoEstudante.Service
{
    public class SolicitacaoService : SingletonBase<SolicitacaoService>
    {
        private static readonly SolicitacaoQuery solicitacaoQuery = SolicitacaoQuery.Instancia;
        private static readonly AlunoQuery alunoQuery = AlunoQuery.Instancia;

        SolicitacaoService() { }

        public SolicitacaoManualResponseDTO GeraSolicitacaoManual(SolicitacaoManualRequestDTO solicitacaoDto, bool forcouGeracao)
        {
            SolicitacaoManualResponseDTO responseDto = new SolicitacaoManualResponseDTO() { Inseriu = false };
            string errorMsg;
            Entity.Solicitacao solicitacao;
            responseDto.DadosValidos = ValidaPreenchimento(solicitacaoDto, out errorMsg);

            if (responseDto.DadosValidos)
            {
                solicitacao = PreencheEntityPor(solicitacaoDto);

                if (solicitacaoQuery.PossuiCondicoesDeGeracaoEssenciais(solicitacao))
                {
                    if (forcouGeracao)
                    {                        
                        solicitacaoQuery.Insere(solicitacao);
                        responseDto.Inseriu = true;
                    }
                    else
                    {
                        if (solicitacaoQuery.PossuiCondicoesDeGeracaoDesconsideraveis(solicitacao))
                        {
                            solicitacaoQuery.Insere(solicitacao);
                            responseDto.Inseriu = true;
                        }
                        else
                        {
                            responseDto.MensagemErro = "A matrícula não se enquadra na regra para geração de remessa. Deseja forçar a geração da remessa?";
                            responseDto.PodeForcarGeracao = true;
                        }
                    }
                }
                else
                {
                    responseDto.MensagemErro = "A matrícula não atende aos requisitos necessários. A solicitação não poderá ser inserida.";
                    responseDto.PodeForcarGeracao = false;
                }

            }
            else
            {
                responseDto.MensagemErro = errorMsg;
            }
            return responseDto;
        }

        private Entity.Solicitacao PreencheEntityPor(SolicitacaoManualRequestDTO solicitacaoDto)
        {
            Entity.Solicitacao solicitacaoEntity = new Entity.Solicitacao();
            solicitacaoEntity.Aluno = solicitacaoDto.Aluno;
            solicitacaoEntity.OperadoraId = solicitacaoDto.OperadoraId;
            solicitacaoEntity.TipoSolicitacaoId = solicitacaoDto.TipoSolicitacaoId;
            solicitacaoEntity.Usuario = solicitacaoDto.Usuario;
            solicitacaoEntity.Observacao = solicitacaoDto.Motivo;
            solicitacaoEntity.MunicipioId = alunoQuery.ObtemMunicipioDaEscolaDo(solicitacaoDto.Aluno);
            solicitacaoEntity.Situacao = Convert.ToInt32(SituacaoSolicitacaoEnum.Criada);
            
            return solicitacaoEntity;
        }

        public bool ValidaPreenchimento(SolicitacaoManualRequestDTO solicitacao, out string errorMsg)
        {
            errorMsg = String.Empty;

            if (String.IsNullOrEmpty(solicitacao.Aluno))
                errorMsg += "Matrícula inválida. ";

            if (!System.Enum.IsDefined(typeof(OperadoraEnum), solicitacao.OperadoraId))
                errorMsg += "Operadora inválida. ";

            if (!System.Enum.IsDefined(typeof(TipoSolicitacaoEnum), solicitacao.TipoSolicitacaoId))
                errorMsg += "Tipo de Solicitação inválida. ";

            if (solicitacao.Motivo.Length > 1000)
                errorMsg += "Motivo excedeu a quantidade máxima de 1000 caracteres. ";

            return String.IsNullOrEmpty(errorMsg);
        }

        public List<SolicitacaoDTO> ListaSolicitacoesPor(string aluno)
        {
            return solicitacaoQuery.ListaSolicitacoesPor(aluno).ToList<SolicitacaoDTO>();                                               
        }
    }
}
