using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Entidades
{
    public class LyGrupoHabilitacaoDoc : IEntity
    {
        public decimal NumFunc { get; set; }

        public string Agrupamento { get; set; }

        public string Provisorio { get; set; }

        public DateTime? DtLimite { get; set; }

        public DateTime StampAtualizacao { get; set; }

        //HABILITAÇÃO MATRÍCULA
        public string Campo01 { get; set; }

        //HABILITAÇÃO GLP
        public string Campo02 { get; set; }

        public string AgrupamentoIngresso { get; set; }

        public string Documentacao { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }
    }
}
