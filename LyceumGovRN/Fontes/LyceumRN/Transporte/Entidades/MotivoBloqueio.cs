using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Transporte.Entidades
{
    [AtributoTabela("Transporte.MOTIVOBLOQUEIO", Nome = "Transporte.MOTIVOBLOQUEIO")]
    public class MotivoBloqueio : IEntity
    {
        [AtributoCampo(Nome = "MOTIVOBLOQUEIOID")]
        public int MotivoBloqueioId { get; set; }

        public string Descricao { get; set; }

        public int Tipo { get; set; }

        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}