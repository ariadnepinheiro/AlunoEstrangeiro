using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.RecursosHumanos.Entidades
{

    [AtributoTabela("RecursosHumanos.DocenteCandidatoGLP", Nome = "RecursosHumanos.DocenteCandidatoGLP")]
    public class DocenteCandidatoGLP : IEntity
    {
        [AtributoCampo(Nome = "DOCENTECANDIDATOGLPID")]
        public int DocenteCandidatoGlpId { get; set; }

        [AtributoCampo(Nome = "DOCENTECANDIDATOID")]
        public int DocenteCandidatoId { get; set; }

        [AtributoCampo(Nome = "ANO")]
        public int Ano { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
