using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.Turmas
{
    public class FrequenciaDiariaAlunoFalta
    {
        public bool PossuiPor(DataContext contexto, int frequenciaDiariaId, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(1)
                                    FROM Turma.FREQUENCIADIARIA_ALUNOFALTA
	                                    WHERE ALUNO = @ALUNO
	                                    AND FREQUENCIADIARIAID = @FREQUENCIADIARIAID";

            contextQuery.Parameters.Add("@FREQUENCIADIARIAID", SqlDbType.Int, frequenciaDiariaId);
            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public void Remove(DataContext contexto, string censo, DateTime dataFrequencia)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE FA
								FROM Turma.FREQUENCIADIARIA F
									INNER JOIN Turma.FREQUENCIADIARIA_ALUNOFALTA FA ON F.FREQUENCIADIARIAID = FA.FREQUENCIADIARIAID
								WHERE FACULDADE = @CENSO
									AND DATAFREQUENCIA = @DATAFREQUENCIA ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@DATAFREQUENCIA", SqlDbType.DateTime, dataFrequencia.Date);

            contexto.ApplyModifications(contextQuery);
        }

        public void DesativaPor(DataContext contexto, int frequenciaDiariaId, List<string> alunos, string usuarioId)
        {
            string matriculas = string.Empty;

            if (alunos.Count() > 0)
            {
                matriculas = alunos.Aggregate((x, y) => x + "', '" + y);
            }

            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = string.Format(@" UPDATE Turma.FREQUENCIADIARIA_ALUNOFALTA
                                SET ATIVO = 0,
                                    USUARIOID = @USUARIOID,
                                    NUMFUNCLANCAMENTO = NULL,
                                    DATAALTERACAO = @DATAALTERACAO
                            WHERE FREQUENCIADIARIAID = @FREQUENCIADIARIAID
                                AND ALUNO NOT IN ('{0}') ", matriculas);

            contextQuery.Parameters.Add("@FREQUENCIADIARIAID", SqlDbType.Int, frequenciaDiariaId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtivaPor(DataContext contexto, Entidades.FrequenciaDiariaAlunoFalta frequenciaDiariaAlunoFalta)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Turma.FREQUENCIADIARIA_ALUNOFALTA
                                SET ATIVO = 1,
                                    USUARIOID = @USUARIOID,
                                    NUMFUNCLANCAMENTO = NULL,
                                    DATAALTERACAO = @DATAALTERACAO
                            WHERE FREQUENCIADIARIAID = @FREQUENCIADIARIAID
                                AND ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@FREQUENCIADIARIAID", SqlDbType.Int, frequenciaDiariaAlunoFalta.FrequenciaDiariaId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, frequenciaDiariaAlunoFalta.UsuarioId);
            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, frequenciaDiariaAlunoFalta.Aluno);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void Insere(DataContext contexto, Entidades.FrequenciaDiariaAlunoFalta frequenciaDiariaAlunoFalta)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Turma.FREQUENCIADIARIA_ALUNOFALTA
                                           (FREQUENCIADIARIAID
                                           ,ALUNO
                                           ,ATIVO
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@FREQUENCIADIARIAID, 
                                           @ALUNO, 
                                           @ATIVO, 
                                           @USUARIOID, 
                                           @DATACADASTRO,
                                           @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@FREQUENCIADIARIAID", SqlDbType.Int, frequenciaDiariaAlunoFalta.FrequenciaDiariaId);
            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, frequenciaDiariaAlunoFalta.Aluno);
            contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, frequenciaDiariaAlunoFalta.Ativo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, frequenciaDiariaAlunoFalta.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
