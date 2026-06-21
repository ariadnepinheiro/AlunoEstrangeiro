using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    class CartaoAluno: IEntity
    {
        [AtributoCampo(Nome = "CARTAO__ALUNOID")]
        public int CartaoAlunoId { get; set; }

        [AtributoCampo(Nome = "ALUNO")]
        public string Aluno { get; set; }

        [AtributoCampo(Nome = "CARTAOID")]        
        public int CartaoId { get; set; }
    }
}
