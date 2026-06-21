using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Pedagogico.Entidades
{
    [AtributoTabela("Pedagogico.TRILHAAPRENDIZAGEM_ESCOLA", Nome = "Pedagogico.TRILHAAPRENDIZAGEM_ESCOLA")]
    public class TrilhaAprendizagemEscola : IEntity
    {
        [AtributoCampo(Nome = "TRILHAAPRENDIZAGEM_ESCOLAID")]
        public int TrilhaAprendizagemEscolaId { get; set; }

        public int Ano { get; set; }

        [AtributoCampo(Nome = "CURSO")]
        public string Curso { get; set; }

        [AtributoCampo(Nome = "TURNO")]
        public string Turno { get; set; }

        public string Censo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
