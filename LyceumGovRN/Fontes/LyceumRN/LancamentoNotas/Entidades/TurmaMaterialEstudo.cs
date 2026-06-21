using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.LancamentoNotas.Entidades
{
    [AtributoTabela("LancamentoNotas.TURMA_MATERIALESTUDO", Nome = "LancamentoNotas.TURMA_MATERIALESTUDO")]
    public class TurmaMaterialEstudo : IEntity
    {
        [AtributoCampo(Nome = "TURMA_MATERIALESTUDOID")]
        public int TurmaMaterialEstudoId { get; set; }

        public int Ano { get; set; }

        public int Semestre { get; set; }

        public string Turma { get; set; }

        public string Disciplina { get; set; }

        public int Subperiodo { get; set; }

        [AtributoCampo(Nome = "MATERIALESTUDOID")]
        public int MaterialEstudoId { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
