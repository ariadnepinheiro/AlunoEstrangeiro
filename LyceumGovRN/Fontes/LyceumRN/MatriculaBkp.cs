using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN
{
    public class MatriculaBkp
    {
        public void InserePorTurma(DataContext ctx, string aluno, decimal ano, decimal periodo, string turma, string motivoBkp)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"INSERT INTO LY_MATRICULA_BKP 
                                            (ALUNO, 
                                             DISCIPLINA, 
                                             TURMA, 
                                             ANO, 
                                             SEMESTRE, 
                                             SIT_MATRICULA, 
                                             DT_ULTALT, 
                                             DT_INSERCAO, 
                                             DT_MATRICULA, 
                                             NUM_CHAMADA, 
                                             STAMP_ATUALIZACAO, 
                                             COMENTARIO, 
                                             DT_EXCLUSAO) 
                                SELECT ALUNO, 
                                       DISCIPLINA, 
                                       TURMA, 
                                       ANO, 
                                       SEMESTRE, 
                                       SIT_MATRICULA, 
                                       DT_ULTALT, 
                                       DT_INSERCAO, 
                                       DT_MATRICULA, 
                                       NUM_CHAMADA, 
                                       STAMP_ATUALIZACAO, 
                                       @MOTIVOBKP, 
                                       GETDATE() 
                                FROM   LY_MATRICULA 
                                WHERE  ALUNO = @ALUNO 
                                       AND TURMA = @TURMA 
                                       AND ANO = @ANO 
                                       AND SEMESTRE = @SEMESTRE  ";

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@MOTIVOBKP", motivoBkp);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
        }
    }
}
