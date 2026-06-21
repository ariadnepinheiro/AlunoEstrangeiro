using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.NecessidadeEspecial.Entidades
{
    [AtributoTabela("NecessidadeEspecial.AlunoRecursoNecessidadeEspecial", Nome = "NecessidadeEspecial.AlunoRecursoNecessidadeEspecial")]
    public class AlunoRecursoNecessidadeEspecial : IEntity
    {
        [AtributoCampo(Nome = "ALUNORECURSONECESSIDADEESPECIALID")]
        public int AlunoRecursoNecessidadeEspecialId { get; set; }

        [AtributoCampo(Nome = "TIPORECURSONECESSIDADEESPECIALID")]
        public int TipoRecursoNecessidadeEspecialId { get; set; }

        [AtributoCampo(Nome = "ALUNOID")]
        public string AlunoId { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
