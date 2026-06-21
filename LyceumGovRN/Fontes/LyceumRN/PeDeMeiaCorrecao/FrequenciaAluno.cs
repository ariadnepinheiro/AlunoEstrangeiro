using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using System.Data;

namespace Techne.Lyceum.RN.PeDeMeiaCorrecao
{
    public class FrequenciaAluno
    {
        public void Insere(DataContext contexto, Entidades.FrequenciaAluno frequenciaAluno)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO PeDeMeiaCorrecao.FREQUENCIAALUNO
                                           (FREQUENCIATURMAID
                                           ,ALUNO
                                           ,DATAAUSENCIA                                        
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@FREQUENCIATURMAID,                                          
                                           @ALUNO,
                                           @DATAAUSENCIA,                                        
                                           @USUARIOID, 
                                           @DATACADASTRO, 
                                           @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@FREQUENCIATURMAID", SqlDbType.Int, frequenciaAluno.FrequenciaTurmaId);
            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, frequenciaAluno.Aluno);
            contextQuery.Parameters.Add("@DATAAUSENCIA", SqlDbType.DateTime, frequenciaAluno.DataAusencia.Date);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, frequenciaAluno.UsuarioID);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void RemovePorTurma(DataContext contexto, int frequenciaTurmaId, List<string> alunos)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = string.Format(@" DELETE PeDeMeiaCorrecao.FREQUENCIAALUNO
                                      WHERE FREQUENCIATURMAID = @FREQUENCIATURMAID 
                                            AND ALUNO IN ('{0}')", alunos.Aggregate((x, y) => x + "','" + y));

            contextQuery.Parameters.Add("@FREQUENCIATURMAID", SqlDbType.Int, frequenciaTurmaId);

            contexto.ApplyModifications(contextQuery);
        }

        public void RemovePorAluno(DataContext contexto, int ano, int periodo, int mes, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE A
                                    FROM PeDeMeiaCorrecao.FREQUENCIATURMA T 
		                                    INNER JOIN PeDeMeiaCorrecao.FREQUENCIAALUNO A ON T.FREQUENCIATURMAID = A.FREQUENCIATURMAID
                                    WHERE ALUNO = @ALUNO
	                                        AND ANO = @ANO
	                                        AND PERIODO = @PERIODO
	                                        AND MESREFERENCIA = @MES  ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@MES", SqlDbType.Int, mes);
            contexto.ApplyModifications(contextQuery);
        }
    }
}
