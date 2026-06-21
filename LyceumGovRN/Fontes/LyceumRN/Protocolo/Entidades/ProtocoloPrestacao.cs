using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Protocolo.Entidades
{
    [AtributoTabela("Protocolo.ProtocoloPrestacao", Nome = "Protocolo.ProtocoloPrestacao")]
    public class ProtocoloPrestacao: IEntity
    {
        [AtributoCampo(Nome = "PROTOCOLOPRESTACAOID")]
        public int ProtocoloPrestacaoId { get; set; }

        public int Ano { get; set; }

        public int? Semestre { get; set; }

        [AtributoCampo(Nome = "TEMPORALIDADE")]
        public string Temporalidade { get; set; }

        [AtributoCampo(Nome = "UNIDADEENSINOID")]
        public string UnidadeEnsinoId { get; set; }

        [AtributoCampo(Nome = "REGIONALID")]
        public int? RegionalId { get; set; }

        public string Processo { get; set; }

        [AtributoCampo(Nome = "NUMEROFOLHAS")]
        public int? NumeroFolhas { get; set; }

        [AtributoCampo(Nome = "TIPOPROTOCOLOID")]
        public int TipoProtocoloId { get; set; }

        [AtributoCampo(Nome = "PROGRAMAPROTOCOLOID")]
        public int? ProgramaProtocoloId { get; set; }

        public string Observacao { get; set; }

        [AtributoCampo(Nome = "DATAPROCESSO")]
        public DateTime DataProcesso { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "USUARIOCADASTROID")]
        public string UsuarioCadastroId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
