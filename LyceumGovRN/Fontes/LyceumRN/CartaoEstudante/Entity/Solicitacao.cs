using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    public class Solicitacao : IEntity
    {
        [AtributoCampo(Nome = "SOLICITACAOID")]
        public int SolicitacaoId { get; set; }

        [AtributoCampo(Nome = "OPERADORAID")]
        public int OperadoraId { get; set; }

        [AtributoCampo(Nome = "MUNICIPIOID")]
        public string MunicipioId { get; set; }

        [AtributoCampo(Nome = "TIPOSOLICITACAOID")]
        public int TipoSolicitacaoId { get; set; }

        public string Aluno { get; set; }

        [AtributoCampo(Nome = "DATASOLICITACAO")]
        public string DataSolicitacao { get; set; }

        public string Observacao{ get; set; }

        public string Usuario { get; set; }

        public int Situacao { get; set; }
    }
}