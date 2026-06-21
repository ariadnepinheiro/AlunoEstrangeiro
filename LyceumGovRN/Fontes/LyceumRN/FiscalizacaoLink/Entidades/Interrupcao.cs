using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.FiscalizacaoLink.Entidades
{
    [AtributoTabela("FiscalizacaoLink.INTERRUPCAO", Nome = "FiscalizacaoLink.INTERRUPCAO")]
    public class Interrupcao : IEntity
    {
        [AtributoCampo(Nome = "INTERRUPCAOID")]
        public int InterrupcaoId { get; set; }

        [AtributoCampo(Nome = "AVALIACAOID")]
        public int AvaliacaoId { get; set; }

        [AtributoCampo(Nome = "MOTIVOINTERRUPCAOID")]
        public int MotivoInterrupcaoId { get; set; }

        [AtributoCampo(Nome = "MOTIVOCOMPLEMENTO")]
        public string MotivoComplemento { get; set; }

        public string Chamado { get; set; }

        [AtributoCampo(Nome = "DATAINTERRUPCAO")]
        public DateTime DataInterrupcao { get; set; }

        [AtributoCampo(Nome = "DATAREESTABELECIMENTO")]
        public DateTime? DataReestabelecimento { get; set; }

        [AtributoCampo(Nome = "TIPOPROBLEMA")]
        public string TipoProblema { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
