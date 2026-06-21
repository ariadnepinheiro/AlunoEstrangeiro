using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Transporte.Entidades
{
    [AtributoTabela("Transporte.ROTAALUNO", Nome = "Transporte.ROTAALUNO")]
    public class RotaAluno : IEntity
    {
        [AtributoCampo(Nome = "RotaAlunoID")]
        public int RotaAlunoId { get; set; }

        [AtributoCampo(Nome = "ROTATRAJETOID")]
        public int RotaTrajetoId { get; set; }

        public string Aluno { get; set; }

        [AtributoCampo(Nome = "DATAINICIO")]
        public DateTime DataInicio { get; set; }

        [AtributoCampo(Nome = "DATAFIM")]
        public DateTime DataFim { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; } 
    }
}
