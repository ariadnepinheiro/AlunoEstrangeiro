using System;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Entidades
{
    [AtributoTabela("CAPACIDADEALUNOTURMA", Nome = "CAPACIDADEALUNOTURMA")]
    public class CapacidaDeAlunoTurma : IEntity
    {
        [AtributoCampo(Nome = "CAPACIDADEALUNOTURMAID")]
        public int CapacidaDeAlunoTurmaId { get; set; }

        [AtributoCampo(Nome = "CURSOID")]
        public string CursoId { get; set; }
       
        public decimal Ano { get; set; }

        public decimal Periodo { get; set; }

        [AtributoCampo(Nome = "CAPACIDADEMINIMA")]
        public int CapacidadeMinima { get; set; }

        [AtributoCampo(Nome = "CAPACIDADEMAXIMA")]
        public int CapacidadeMaxima { get; set; }

        public string Matricula { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
