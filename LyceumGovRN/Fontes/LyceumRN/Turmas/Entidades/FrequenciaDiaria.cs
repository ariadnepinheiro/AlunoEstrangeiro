using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Turmas.Entidades
{
    [AtributoTabela("Turma.FREQUENCIADIARIA", Nome = "Turma.FREQUENCIADIARIA")]
    public class FrequenciaDiaria: IEntity
    {
        [AtributoCampo(Nome = "FREQUENCIADIARIAID")]
        public int FrequenciaDiariaId { get; set; }

        public decimal Ano { get; set; }

        public decimal Semestre { get; set; }

        public string Turma { get; set; }

        public string Disciplina { get; set; }

        public decimal Aula { get; set; }

        public string Faculdade { get; set; }

        [AtributoCampo(Nome = "DIA_SEMANA")]
        public decimal DiaSemana { get; set; }

        public string Turno { get; set; }

        [AtributoCampo(Nome = "DATAFREQUENCIA")]
        public DateTime DataFrequencia { get; set; }

        [AtributoCampo(Nome = "DATAREPOSICAO")]
        public DateTime? DataReposicao { get; set; }

        [AtributoCampo(Nome = "NUMFUNCLANCAMENTO")]
        public decimal? NumFuncLancamento { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
