using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Matriculas.Entidades
{
    [AtributoTabela("Matricula.MOTIVOENCAMINHAMENTOESPECIAL", Nome = "Matricula.MOTIVOENCAMINHAMENTOESPECIAL")]
    public class MotivoEncaminhamentoEspecial : IEntity
    {
        [AtributoCampo(Nome = "MOTIVOENCAMINHAMENTOESPECIALID")]
        public int MotivoEncaminhamentoEspecialId { get; set; }

        public string Descricao { get; set; }

        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
