using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.GestaoRede.Entidades
{
    [AtributoTabela("GestaoRede.REGIAOFINANCEIRA", Nome = "GestaoRede.REGIAOFINANCEIRA")]
    public class RegiaoFinanceira : IEntity
    {
        [AtributoCampo(Nome = "REGIAOFINANCEIRAID")]
        public int RegiaoFinanceiraId { get; set; }

        [AtributoCampo(Nome = "DESCRICAO")]
        public string Descricao { get; set; }

        [AtributoCampo(Nome = "CODIGOCG")]
        public string CodigoCg { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
