using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.RecursosHumanos.Entidades
{
    [AtributoTabela("RecursosHumanos.DOCENTECANDIDATO", Nome = "RecursosHumanos.DOCENTECANDIDATO")]
    public class DocenteCandidato : IEntity
    {
        [AtributoCampo(Nome = "DOCENTECANDIDATOID")]
        public int DocenteCandidatoId { get; set; }

        [AtributoCampo(Nome = "ACUMULACAOID")]
        public int NumFunc { get; set; }

        [AtributoCampo(Nome = "CONCURSO")]
        public string Concurso { get; set; }

        [AtributoCampo(Nome = "QTDEANOSGLP")]
        public int QtdeAnosGlp { get; set; }

        [AtributoCampo(Nome = "ACUMULACAO")]
        public bool Acumulacao { get; set; }

        [AtributoCampo(Nome = "UTILIZARUBRICA")]
        public bool UtilizaRubrica { get; set; }

        [AtributoCampo(Nome = "ID_REGIONAL")]
        public int? IdRegional { get; set; }

        [AtributoCampo(Nome = "MUNICIPIO")]
        public string Municipio { get; set; }

        [AtributoCampo(Nome = "SEDE")]
        public string Sede { get; set; }

        [AtributoCampo(Nome = "DISCIPLINAINGRESSO")]
        public string DisciplinaIngresso { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }       
    }
}
