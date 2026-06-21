using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.CartaoEstudante.Entity;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class AlunoQuery : QueryBase<AlunoQuery>
    {
        private const string TABELA_ALUNO = "Lyceum.dbo.LY_ALUNO";

        AlunoQuery() { }

        public bool ExisteAluno(string matricula)
        {
            try
            {
                string consulta = "select 1 FROM " + TABELA_ALUNO + " WHERE ALUNO = @MATRICULA";
                return Possui(consulta, matricula);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        public string ObtemMunicipioDaEscolaDo(string aluno)
        {
            try
            {
                string consulta = @" SELECT UE.MUNICIPIO
                        FROM  LY_ALUNO A (NOLOCK)  
	                          JOIN LY_PESSOA P (NOLOCK)   ON  P.PESSOA = A.PESSOA   
	                          JOIN LY_UNIDADE_ENSINO UE (NOLOCK)  ON  UE.UNIDADE_ENS = A.UNIDADE_ENSINO  
                        where aluno = @ALUNO ";

                return ObtemValorSimples<string>(consulta, aluno);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
