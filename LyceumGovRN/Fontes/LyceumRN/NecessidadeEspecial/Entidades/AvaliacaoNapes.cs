using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.NecessidadeEspecial.Entidades
{
    [AtributoTabela("NecessidadeEspecial.AvaliacaoNapes", Nome = "NecessidadeEspecial.AvaliacaoNapes")]
    public class AvaliacaoNapes : IEntity
    {
        [AtributoCampo(Nome = "AVALIACAONAPESID")]
        public int AvaliacaoNapesId { get; set; }

        [AtributoCampo(Nome = "ALUNOID")]
        public string AlunoId { get; set; }

        [AtributoCampo(Nome = "TIPORECURSONECESSIDADEESPECIALID")]
        public int TipoRecursoNecessidadeEspecialId { get; set; }

        [AtributoCampo(Nome = "NECESSITARECURSO")]
        public bool? NecessitaRecurso { get; set; }

        [AtributoCampo(Nome = "JUSTIFICATIVA")]
        public string Justificativa { get; set; }

        [AtributoCampo(Nome = "TRANSITORIO")]
        public bool? Transitorio { get; set; }

        [AtributoCampo(Nome = "DATAINICIO")]
        public DateTime? DataInicio { get; set; }

        [AtributoCampo(Nome = "DATAFIM")]
        public DateTime? DataFim { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
