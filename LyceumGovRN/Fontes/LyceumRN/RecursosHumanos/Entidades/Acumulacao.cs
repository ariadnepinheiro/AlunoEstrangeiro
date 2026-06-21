using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.RecursosHumanos.Entidades
{
    public class Acumulacao : IEntity
    {
        [AtributoCampo(Nome = "ACUMULACAOID")]
        public int AcumulacaoId { get; set; }

        [AtributoCampo(Nome = "DOCENTEID")]
        public decimal DocenteId { get; set; }

        public string Orgao { get; set; }

        [AtributoCampo(Nome = "MATRICULAORGAO")]
        public string MatriculaOrgao { get; set; }

        [AtributoCampo(Nome = "NUMEROPROCESSO")]
        public string NumeroProcesso { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
