using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Turmas.Entidades
{
    [AtributoTabela("Turma.DIASEMAULA", Nome = "Turma.DIASEMAULA")]
    public class DiaSemAula : IEntity
    {
        [AtributoCampo(Nome = "DIASEMAULAID")]
        public int DiaSemAulaId { get; set; }

        public string Censo { get; set; }

        public DateTime Data { get; set; }

        [AtributoCampo(Nome = "PROCESSOSEI")]
        public string ProcessoSei { get; set; }

        [AtributoCampo(Nome = "MOTIVODIASEMAULAID")]
        public int MotivoDiaSemAulaId { get; set; }

        public string Justificativa { get; set; }

        [AtributoCampo(Nome = "DATAREPOSICAO")]
        public DateTime DataReposicao { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}