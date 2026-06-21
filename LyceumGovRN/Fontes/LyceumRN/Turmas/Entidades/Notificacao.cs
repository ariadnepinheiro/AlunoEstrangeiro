using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Turmas.Entidades
{
    [AtributoTabela("Turma.NOTIFICACAO", Nome = "Turma.NOTIFICACAO")]
    public class Notificacao : IEntity
    {
        [AtributoCampo(Nome = "NOTIFICACAOID")]
        public int NotificacaoId { get; set; }

        public string Aluno { get; set; }

        public int Ano { get; set; }

        public int Sequencial { get; set; }

        [AtributoCampo(Nome = "QUANTIDADEFALTAS")]
        public int QuantidadeFaltas { get; set; }

        [AtributoCampo(Nome = "DATAINICIOFALTAS")]
        public DateTime DataInicioFaltas { get; set; }

        [AtributoCampo(Nome = "NUMEROFICAI")]
        public string NumeroFicai { get; set; }

        [AtributoCampo(Nome = "NUMEROFAMI")]
        public string NumeroFami { get; set; }

        [AtributoCampo(Nome = "DATACOMUNICACAO")]
        public DateTime DataComunicacao { get; set; }

        [AtributoCampo(Nome = "OBSERVACAO")]
        public string Observacao { get; set; }

        [AtributoCampo(Nome = "FORMACONTATOID_1")]
        public int? FormaContatoId1 { get; set; }

        [AtributoCampo(Nome = "DATACONTATO_1")]
        public DateTime? DataContato1 { get; set; }

        [AtributoCampo(Nome = "FORMACONTATOID_2")]
        public int? FormaContatoId2 { get; set; }

        [AtributoCampo(Nome = "DATACONTATO_2")]
        public DateTime? DataContato2 { get; set; }

        [AtributoCampo(Nome = "FORMACONTATOID_3")]
        public int? FormaContatoId3 { get; set; }

        [AtributoCampo(Nome = "DATACONTATO_3")]
        public DateTime? DataContato3 { get; set; }

        [AtributoCampo(Nome = "SITUACAOFAMILIARID")]
        public int? SituacaoFamiliarId { get; set; }

        [AtributoCampo(Nome = "ALEGACAO")]
        public string Alegacao { get; set; }

        [AtributoCampo(Nome = "TIPOENCAMINHAMENTOID")]
        public int? TipoEncaminhamentoId { get; set; }

        [AtributoCampo(Nome = "EQUIPAMENTOUSADO")]
        public string EquipamentoUsado { get; set; }

        [AtributoCampo(Nome = "DATARETORNO")]
        public DateTime? DataRetorno { get; set; }

        [AtributoCampo(Nome = "DATAENCAMINHAMENTOESCOLA")]
        public DateTime? DataEncaminhamentoEscola { get; set; }

        [AtributoCampo(Nome = "PROTOCOLOCONSELHO")]
        public string ProtocoloConselho { get; set; }

        [AtributoCampo(Nome = "MEDIDACONSELHOTUTELARID")]
        public int? MedidasConselhoTutelarId { get; set; }

        [AtributoCampo(Nome = "DATAENCAMINHAMENTOCONSELHO")]
        public DateTime? DataEncaminhamentoConselho { get; set; }

        [AtributoCampo(Nome = "NOMECONSELHEIRO")]
        public string NomeConselheiro { get; set; }

        [AtributoCampo(Nome = "MEDIDAMPRJID")]
        public int? MedidaMPRJId { get; set; }

        [AtributoCampo(Nome = "DATAENCAMINHAMENTOMPRJ")]
        public DateTime? DataEncaminhamentoMprj { get; set; }

        [AtributoCampo(Nome = "PROMOTOR")]
        public string Promotor { get; set; }

        [AtributoCampo(Nome = "ENCAMINHAMENTOSREALIZADO")]
        public string EncaminhamentosRealizado { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
