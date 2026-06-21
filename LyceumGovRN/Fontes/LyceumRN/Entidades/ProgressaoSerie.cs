using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Entidades
{
    [AtributoTabela("PROGRESSAOSERIE", Nome = "PROGRESSAOSERIE")]
    public class ProgressaoSerie : IEntity
    {
        [AtributoCampo(Nome = "PROGRESSAOSERIEID")]
        public int ProgressaoSerieId { get; set; }

        [AtributoCampo(Nome = "CURSOID")]
        public string CursoId { get; set; }

        [AtributoCampo(Nome = "SERIEID")]
        public int SerieId { get; set; }

        [AtributoCampo(Nome = "PROXIMOCURSOID")]
        public string ProximoCursoId { get; set; }

        [AtributoCampo(Nome = "PROXIMOSERIEID")]
        public int ProximoSerieId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "MATRICULA")]
        public string Matricula { get; set; }
    }
}
