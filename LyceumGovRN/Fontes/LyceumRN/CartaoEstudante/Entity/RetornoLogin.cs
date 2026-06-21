using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    [AtributoTabela("RETORNOLOGIN", Nome = "RETORNOLOGIN")]
    class RetornoLogin: IEntity
    {
        [AtributoCampo(Nome = "RETORNOLOGINID")]
        public int RetornoLoginId { get; set; }

        [AtributoCampo(Nome = "OPERADORAID")]
        public int OperadoraId { get; set; }
                
        public string Aluno { get; set; }

        [AtributoCampo(Nome = "IDBENEFICIARIO")]
        public int IdBeneficiario { get; set; }

        [AtributoCampo(Nome = "DATACONFIRMACAOALUNO")]
        public DateTime DataConfirmacaoAluno { get; set; }

        [AtributoCampo(Nome = "LOGINOPERADORA")]
        public string LoginOperadora { get; set; }

        public RetornoLogin() { }
        public RetornoLogin(int idBeneficiario, string aluno, string loginOperadora, DateTime dataConfirmacaoAluno, int operadoraId)
        {
            IdBeneficiario = idBeneficiario;
            Aluno = aluno;
            LoginOperadora = loginOperadora;
            DataConfirmacaoAluno = dataConfirmacaoAluno;
            OperadoraId = operadoraId;
        }
    }
}
