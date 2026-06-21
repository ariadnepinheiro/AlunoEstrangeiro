using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Query;

namespace Techne.Lyceum.RN.CartaoEstudante.Service
{
    public class AlunoService: SingletonBase<AlunoService>
    {
        private static readonly AlunoQuery alunoQuery = AlunoQuery.Instancia;

        AlunoService() { }

        public bool ExisteAluno(string aluno)
		{
            return alunoQuery.ExisteAluno(aluno);
		}
    }
}
