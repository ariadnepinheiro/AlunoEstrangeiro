using System;
using Techne.Lyceum.RN.CartaoEstudante.Query;
using Techne.Lyceum.RN.CartaoEstudante.DTO;
using Techne.Lyceum.RN.CartaoEstudante.DTO.Filter;
using Techne.Lyceum.RN.CartaoEstudante.Entity;
using Techne.Lyceum.RN.CartaoEstudante.Domain;
using System.Text;
using System.Collections.Generic;
using Techne.Lyceum.RN.Util;


namespace Techne.Lyceum.RN.CartaoEstudante.Service
{
    public class RemessaService : SingletonBase<RemessaService>
    {
        private static readonly RemessaQuery remessaQuery = RemessaQuery.Instancia;
        private static readonly LogRemessaService logRemessaService = LogRemessaService.Instancia;
        private static readonly LogLoteRemessaService logLoteRemessaService = LogLoteRemessaService.Instancia;
        private static readonly LoteRemessaService loteRemessaService = LoteRemessaService.Instancia;

        RemessaService() { }

        /// <summary>
        /// Lista as informações de envio e retorno de remessas.
        /// </summary>
        /// <param name="filtro">Dto contendo os parâmetros de filtro (aluno, unidade de ensino, data de envio, existencia de crítica).</param>
        /// <returns>Lista de AcompanhamentoRemessaDTO's contendo alem dos dados básicos da remessa, o status de processamento e a data de envio.</returns>
        public List<AcompanhamentoRemessaDTO> ListaProcessamento(AcompanhamentoRemessaFilterDTO filtro)
        //public System.Data.DataTable ListaProcessamento(AcompanhamentoRemessaFilterDTO filtro)
        {
            return remessaQuery.ListaProcessamento(filtro.Aluno, filtro.UnidadeEnsino, filtro.DataEnvioInicio, filtro.DataEnvioFim, filtro.TipoSituacaoProcessamento, filtro.Municipio, filtro.IdRegional)
                               .ToList<AcompanhamentoRemessaDTO>();
        }

        public RemessaDTO ObtemDetalhesRemessaPor(int remessaId)
        {
            RemessaDTO remessaDTO;

            Remessa remessa = remessaQuery.ObtemRemessaPor(remessaId);
            remessaDTO = PreencheRemessaDTO(remessa);
            return remessaDTO;
        }

        private RemessaDTO PreencheRemessaDTO(Remessa remessa)
        {
            RemessaDTO dto = null;
            if (remessa != null)
            {
                dto = new RemessaDTO();
                dto.RemessaId = remessa.RemessaId;
                dto.SolicitacaoId = remessa.SolicitacaoId;
                dto.OperadoraId = remessa.OperadoraId;
                dto.LoteRemessaId = remessa.LoteRemessaId;
                dto.DataInclusao = remessa.DataInclusao;
                dto.MatriculaAluno = remessa.Aluno;
                dto.NomeAluno= remessa.NomeCompl;
                dto.DataNascimento = remessa.DtNasc;
                dto.NomePai = remessa.NomePai;
                dto.NomeMae = remessa.NomeMae;
                dto.Cpf = remessa.Cpf;
                dto.NumeroRG = remessa.RgNum;
                dto.UFRg = remessa.RgUF;
                dto.OrgaoEmissorRG = remessa.RgEmissor;
                dto.DataExpedicaoRG = remessa.RgDtExp;
                dto.Cep = remessa.Cep;
                dto.TipoLogradouroEndereco = remessa.EndTpLogradouro;
                dto.Endereco = remessa.Endereco;
                dto.NumeroEndereco = remessa.EndNum;
                dto.ComplementoEndereco = remessa.EndCompl;
                dto.Bairro = remessa.Bairro;
                dto.EndMunicipio = remessa.EndMunicipio;
                dto.UnidadeEnsino = remessa.UnidadeEns;
                dto.Foto = remessa.Foto;
                dto.Gratuidade = remessa.Gratuidade;
                dto.ModalTrem = remessa.ModalTrem;
                dto.ModalOnibus = remessa.ModalOnibus;
                dto.ModalMetro = remessa.ModalMetro;
                dto.ModalBarcas = remessa.ModalBarcas;
                dto.DataUltimaAtualizacao = remessa.StampAtualizacao;
                dto.Turno = remessa.Turno;
                dto.Turma = remessa.Turma;
                dto.Serie = remessa.Serie;
                dto.EmailInterno = remessa.EmailInterno;
                dto.LoginRioCard = remessa.LoginRioCard;                
                dto.DataEnvioLoteRemessa = logLoteRemessaService.ObtemPrimeiraDataEnvioPor(remessa.LoteRemessaId);
                dto.NomeLoteRemessa = loteRemessaService.ObtemNomeLotePor(remessa.LoteRemessaId);
            }

            return dto;
        }
    }
}