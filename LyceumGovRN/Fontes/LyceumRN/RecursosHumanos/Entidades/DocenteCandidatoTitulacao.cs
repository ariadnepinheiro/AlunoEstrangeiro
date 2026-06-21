using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.RecursosHumanos.Entidades
{
    [AtributoTabela("RecursosHumanos.DOCENTECANDIDATOTITULACAO", Nome = "RecursosHumanos.DOCENTECANDIDATOTITULACAO")]
    public class DocenteCandidatoTitulacao : IEntity
    {
        [AtributoCampo(Nome = "DOCENTECANDIDATOTITULACAOID")]
        public int DocenteCandidatoTitulacaoId { get; set; }

        [AtributoCampo(Nome = "DOCENTECANDIDATOID")]
        public int DocenteCandidatoId { get; set; }

        [AtributoCampo(Nome = "CONCURSO")]
        public string Concurso { get; set; }

        [AtributoCampo(Nome = "TITULACAO")]
        public string Titulacao { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
