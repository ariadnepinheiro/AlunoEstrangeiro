using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Matriculas.Entidades
{
    [AtributoTabela("Matricula.INSCRICAOALUNO", Nome = "Matricula.INSCRICAOALUNO")]
    public class InscricaoAluno : IEntity
    {
        [AtributoCampo(Nome = "INSCRICAOALUNOID")]
        public int InscricaoAlunoId { get; set; }

        [AtributoCampo(Nome = "PRECADASTROALUNOID")]
        public int PreCadastroAlunoId { get; set; }

        [AtributoCampo(Nome = "ANO")]
        public int Ano { get; set; }

        [AtributoCampo(Nome = "PERIODO")]
        public int Periodo { get; set; }

        [AtributoCampo(Nome = "NUMEROINSCRICAO")]
        public int? NumeroInscricao { get; set; }

        [AtributoCampo(Nome = "PARTICIPOUFASE1")]
        public bool? ParticipouFase1 { get; set; }

        [AtributoCampo(Nome = "ALOCADOFASE1")]
        public bool? AlocadoFase1 { get; set; }

        [AtributoCampo(Nome = "CONFIRMADOFASE1")]
        public bool? ConfirmadoFase1 { get; set; }

        [AtributoCampo(Nome = "DESALOCADOFASE1")]
        public bool? DesalocadoFase1 { get; set; }

        [AtributoCampo(Nome = "PARTICIPOUFASE2")]
        public bool? ParticipouFase2 { get; set; }

        [AtributoCampo(Nome = "ALOCADOFASE2")]
        public bool? AlocadoFase2 { get; set; }

        [AtributoCampo(Nome = "CONFIRMADOFASE2")]
        public bool? ConfirmadoFase2 { get; set; }

        [AtributoCampo(Nome = "ENCAMINHAMENTOESPECIAL")]
        public bool? EncaminhamentoEspecial { get; set; }

        [AtributoCampo(Nome = "ALUNO")]
        public string Aluno { get; set; }
        
        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
