using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.GestaoRede.Entidades
{
    [AtributoTabela("GestaoRede.SUPERINTENDENCIA", Nome = "GestaoRede.SUPERINTENDENCIA")]
    public class Superintendencia : IEntity
    {
        [AtributoCampo(Nome = "SUPERINTENDENCIAID")]
        public int SuperintendenciaId { get; set; }

        [AtributoCampo(Nome = "SUBSECRETARIAID")]
        public int SubsecretariaId { get; set; }

        [AtributoCampo(Nome = "DESCRICAO")]
        public string Descricao { get; set; }

        [AtributoCampo(Nome = "SETOR")]
        public string Setor { get; set; }

        [AtributoCampo(Nome = "ATIVO")]
        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}