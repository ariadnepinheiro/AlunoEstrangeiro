using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Patrimonio.Entidades
{
    [AtributoTabela("Patrimonio.BEMVALOR", Nome = "Patrimonio.BEMVALOR")]
    public class BemValor : IEntity
    {
        [AtributoCampo(Nome = "BEMVALORID")]
        public int BemValorId { get; set; }

        [AtributoCampo(Nome = "BEMID")]
        public int BemId { get; set; }

        [AtributoCampo(Nome = "MOEDAID")]
        public int MoedaId { get; set; }

        [AtributoCampo(Nome = "ESTADOCONSERVACAOID")]
        public int EstadoconservacaoId { get; set; }

        public decimal Valor { get; set; }

        [AtributoCampo(Nome = "VIDAUTIL")]
        public int VidaUtil { get; set; }

        [AtributoCampo(Nome = "DATAINICIO")]
        public DateTime DataInicio { get; set; }

        [AtributoCampo(Nome = "DATAFIM")]
        public DateTime? DataFim { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
