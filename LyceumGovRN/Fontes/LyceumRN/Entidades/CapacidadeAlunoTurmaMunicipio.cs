using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    [AtributoTabela("CAPACIDADEALUNOTURMAMUNICIPIO", Nome = "CAPACIDADEALUNOTURMAMUNICIPIO")]
    public class CapacidadeAlunoTurmaMunicipio : IEntity
    {
        [AtributoCampo(Nome = "CAPACIDADEALUNOTURMAMUNICIPIOID")]
        public int CapacidadeAlunoTurmaMunicipioId { get; set; }

        [AtributoCampo(Nome = "MUNICIPIOID")]
        public string MunicipioId { get; set; }

        [AtributoCampo(Nome = "ANO")]
        public decimal Ano { get; set; }

        [AtributoCampo(Nome = "PERIODO")]
        public decimal Periodo { get; set; }

        [AtributoCampo(Nome = "TIPO")]
        public int Tipo { get; set; }

        [AtributoCampo(Nome = "CAPACIDADE")]
        public int Capacidade { get; set; }

        [AtributoCampo(Nome = "MATRICULA")]
        public string Matricula { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }

        public CapacidadeAlunoTurmaMunicipio() { }
    }
}
