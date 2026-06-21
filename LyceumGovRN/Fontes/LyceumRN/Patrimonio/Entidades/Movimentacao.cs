using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Patrimonio.Entidades
{
    [AtributoTabela("Patrimonio.MOVIMENTACAO", Nome = "Patrimonio.MOVIMENTACAO")]
    public class Movimentacao : IEntity
    {
        [AtributoCampo(Nome = "MOVIMENTACAOID")]
        public int MovimentacaoId { get; set; }

        [AtributoCampo(Nome = "BEMVALORID")]
        public int BemId { get; set; }

        public int Numero { get; set; }

        public string Setor { get; set; }

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
