using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.ACOMPANHAMENTONOTA", Nome = "PrestacaoContas.ACOMPANHAMENTONOTA")]
    public class AcompanhamentoNota : IEntity
    {
        [AtributoCampo(Nome = "ACOMPANHAMENTONOTAID")]
        public int AcompanhamentoNotaId { get; set; }

        [AtributoCampo(Nome = "CENSO")]
        public string Censo { get; set; }

        [AtributoCampo(Nome = "PROCESSO")]
        public string Processo { get; set; }

        [AtributoCampo(Nome = "DATAPROCESSO")]
        public DateTime DataProcesso { get; set; }

        [AtributoCampo(Nome = "CHAVEACESSO")]
        public string ChaveAcesso { get; set; }

        [AtributoCampo(Nome = "VALIDO")]
        public bool Valido { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
