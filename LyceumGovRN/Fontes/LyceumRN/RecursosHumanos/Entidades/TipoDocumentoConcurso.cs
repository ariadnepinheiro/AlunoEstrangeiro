using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.RecursosHumanos.Entidades
{
    [AtributoTabela("RecursosHumanos.TIPODOCUMENTOCONCURSO", Nome = "RecursosHumanos.TIPODOCUMENTOCONCURSO")]
    public class TipoDocumentoConcurso : IEntity
    {
        [AtributoCampo(Nome = "TIPODOCUMENTOCONCURSOID")]
        public int TipoDocumentoConcursoId { get; set; }

        [AtributoCampo(Nome = "TIPODOCUMENTOID")]
        public int TipoDocumentoId { get; set; }

        public string Concurso { get; set; }

        public bool Anexo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
