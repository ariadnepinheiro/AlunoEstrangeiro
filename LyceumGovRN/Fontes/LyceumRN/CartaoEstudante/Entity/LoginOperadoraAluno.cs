using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    [AtributoTabela("LOGINOPERADORAALUNO", Nome = "LOGINOPERADORAALUNO")]
    public class LoginOperadoraAluno: IEntity
    {
        [AtributoCampo(Nome = "LOGINOPERADORAALUNOID")]
        public int LoginOperadoraAlunoId { get; set; }

        [AtributoCampo(Nome = "OPERADORAID")]
        public int OperadoraId { get; set; }
                
        public string Aluno { get; set; }

        [AtributoCampo(Nome = "IDBENEFICIARIO")]
        public int IdBeneficiario { get; set; }

        [AtributoCampo(Nome = "DATAATUALIZACAOOPERADORA")]
        public DateTime DataAtualizacaoOperadora { get; set; }

        [AtributoCampo(Nome = "LOGINOPERADORA")]
        public string LoginOperadora { get; set; }

        [AtributoCampo(Nome = "DATAINCLUSAO")]
        public DateTime DataInclusao { get; set; }

        public LoginOperadoraAluno() { }
    }
}
