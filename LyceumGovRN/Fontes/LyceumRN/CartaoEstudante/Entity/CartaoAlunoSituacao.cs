using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    class CartaoAlunoSituacao: IEntity
    {
        [AtributoCampo(Nome = "CARTAO__ALUNOSITUACAOID")]
        public int CartaoAlunoSituacaoId { get; set; }

        [AtributoCampo(Nome = "CARTAO__ALUNOID")]
        public int CartaoAlunoId { get; set; }

        [AtributoCampo(Nome = "SITUACAO")]
        public string Situacao { get; set; }

        [AtributoCampo(Nome = "DATASITUACAO")]
        public DateTime DataSituacao { get; set; }
    }
}
