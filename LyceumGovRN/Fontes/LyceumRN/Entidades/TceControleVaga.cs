using System;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceControleVaga : IEntity
    {
        public int IdControleVaga { get; set; }

        public string Censo { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        public string Curso { get; set; }

        public int Serie { get; set; }

        public string Turno { get; set; }

        public int VagasLiberadas { get; set; }

        public int VagasContinuidade { get; set; }

        public int VagasNovas { get; set; }

        public string Matricula { get; set; }

        [AtributoCampo(Nome = "PARTICIPAMATRICULAFACIL")]
        public bool ParticipaMatriculaFacil { get; set; }

        [AtributoCampo(Nome = "VISUALIZAVAGA")]
        public bool VisualizaVaga { get; set; }

        [AtributoCampo(Nome = "VAGAPLANEJADA")]
        public int? VagaPlanejada { get; set; }

        [AtributoCampo(Nome = "OFERECEVAGAFASE1")]
        public bool OfereceVagaFase1 { get; set; }

        [AtributoCampo(Nome = "PARALISAMATRICULAFACIL")]
        public bool ParalisaMatriculaFacil { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }
    }
}
