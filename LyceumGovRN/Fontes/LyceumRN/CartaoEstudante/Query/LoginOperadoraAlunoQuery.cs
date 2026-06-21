using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Entity;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class LoginOperadoraAlunoQuery: QueryBase<LoginOperadoraAlunoQuery>
    {
        private const string TABELA_LOGIN_OPERADORA_ALUNO = "Lyceum.CartaoEstudante.LOGINOPERADORAALUNO";

        LoginOperadoraAlunoQuery() { }

        public LoginOperadoraAluno ObtemPor(string aluno)
        {
            string query = @"SELECT * FROM " + TABELA_LOGIN_OPERADORA_ALUNO + " WHERE ALUNO = @ALUNO";
            LoginOperadoraAluno loginOperadoraAluno = ObtemPor<LoginOperadoraAluno>(query, aluno);

            return loginOperadoraAluno;
        }
    }
}
