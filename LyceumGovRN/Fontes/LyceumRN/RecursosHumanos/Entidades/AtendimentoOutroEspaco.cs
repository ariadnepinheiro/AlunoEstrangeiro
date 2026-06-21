using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.RecursosHumanos.Entidades
{
    [AtributoTabela("RecursosHumanos.ATENDIMENTOOUTROESPACO", Nome = "RecursosHumanos.ATENDIMENTOOUTROESPACO")]
    public class AtendimentoOutroEspaco : IEntity
    {
        [AtributoCampo(Nome = "ATENDIMENTOOUTROESPACOID")]
        public int AtendimentoOutroEspacoId { get; set; }

        public string Aluno { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        public string Turma { get; set; }

        public string Censo { get; set; }

        [AtributoCampo(Nome = "DATAINICIO")]
        public DateTime DataInicio { get; set; }

        [AtributoCampo(Nome = "DATAFIM")]
        public DateTime DataFim { get; set; }

        public string Tipo { get; set; }

        public bool? Laudo { get; set; }

        public bool? Requerimento { get; set; }

        public bool? PlanoEspecial { get; set; }

        [AtributoCampo(Nome = "NUMEROSEI")]
        public string NumeroSei { get; set; }

        public bool? Prorrogacao { get; set; }

        public string Descricao { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
