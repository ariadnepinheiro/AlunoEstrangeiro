using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    public class TipoSolicitacao : IEntity
    {
        [AtributoCampo(Nome = "TIPOSOLICITACAOID")]
        public int TipoSolicitacaoId { get; set; }

        public string Descricao { get; set; }

        [AtributoCampo(Nome = "CODSOLICITACAO")]
        public string CodigoSolicitacao { get; set; }

        [AtributoCampo(Nome = "FLAGGERASEGUNDAVIA")]
        public string FlagSegundaVia { get; set; }
    }
}