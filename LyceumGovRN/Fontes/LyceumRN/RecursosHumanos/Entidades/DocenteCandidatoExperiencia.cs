using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.RecursosHumanos.Entidades
{
    [AtributoTabela("RecursosHumanos.DOCENTECANDIDATOEXPERIENCIA", Nome = "RecursosHumanos.DOCENTECANDIDATOEXPERIENCIA")]
    public class DocenteCandidatoExperiencia : IEntity
    {
        [AtributoCampo(Nome = "DOCENTECANDIDATOEXPERIENCIAID")]
        public int DocenteCandidatoExperienciaId { get; set; }

        [AtributoCampo(Nome = "DOCENTECANDIDATOID")]
        public int DocenteCandidatoId { get; set; }

        [AtributoCampo(Nome = "CONCURSO")]
        public string Concurso { get; set; }

        [AtributoCampo(Nome = "EXPERIENCIA")]
        public string Experiencia { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}