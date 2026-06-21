using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.FiscalizacaoLink.Entidades
{
    [AtributoTabela("FiscalizacaoLink.AVALIACAO", Nome = "FiscalizacaoLink.AVALIACAO")]
    public class Avaliacao : IEntity
    {
        [AtributoCampo(Nome = "AVALIACAOID")]
        public int AvaliacaoId { get; set; }

        [AtributoCampo(Nome = "CIRCUITOSETORID")]
        public int CircuitoSetorId { get; set; }

        [AtributoCampo(Nome = "SETORID")]
        public string SetorId { get; set; }

        public int Ano { get; set; }

        public int Mes { get; set; }

        public bool? Interrupcao { get; set; }

        [AtributoCampo(Nome = "ENVIOFATURAMENTO")]
        public bool EnvioFaturamento { get; set; }

        [AtributoCampo(Nome = "DATAENVIOFATURAMENTO")]
        public DateTime? DataEnvioFaturamento { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
