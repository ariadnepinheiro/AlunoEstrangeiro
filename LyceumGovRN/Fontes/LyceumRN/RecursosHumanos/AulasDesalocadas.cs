using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class AulasDesalocadas
    {
        public void Insere(DataContext contexto, DTO.DadosTurmaAlocacao dadosTurmaAlocacao, int docenteCandidatoId, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO RecursosHumanos.AULASDESALOCADAS
                                           (DOCENTECANDIDATOID
                                           ,NUM_FUNC
                                           ,TIPO_AULA
                                           ,ANO
                                           ,SEMESTRE
                                           ,TURMA
                                           ,DISCIPLINA
                                           ,TURNO
                                           ,FACULDADE
                                           ,DIA_SEMANA
                                           ,AULA
                                           ,DATA_INICIO
                                           ,USUARIOID
                                           ,DATACADASTRO)
                                     VALUES
                                           (@DOCENTECANDIDATOID,
                                           @NUM_FUNC, 
                                           @TIPO_AULA, 
                                           @ANO, 
                                           @SEMESTRE, 
                                           @TURMA, 
                                           @DISCIPLINA, 
                                           @TURNO, 
                                           @FACULDADE, 
                                           @DIA_SEMANA, 
                                           @AULA, 
                                           @DATA_INICIO, 
                                           @USUARIOID, 
                                           @DATACADASTRO ) ";

            contextQuery.Parameters.Add("@DOCENTECANDIDATOID", SqlDbType.Int, docenteCandidatoId);
            contextQuery.Parameters.Add("@NUM_FUNC", SqlDbType.Int, dadosTurmaAlocacao.NumFuncAnterior);
            contextQuery.Parameters.Add("@TIPO_AULA", SqlDbType.VarChar, dadosTurmaAlocacao.TipoAula);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, dadosTurmaAlocacao.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, dadosTurmaAlocacao.Semestre);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, dadosTurmaAlocacao.Turma);
            contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, dadosTurmaAlocacao.Disciplina);
            contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, dadosTurmaAlocacao.Turno);
            contextQuery.Parameters.Add("@FACULDADE", SqlDbType.VarChar, dadosTurmaAlocacao.Faculdade);
            contextQuery.Parameters.Add("@DIA_SEMANA", SqlDbType.Int, dadosTurmaAlocacao.DiaSemana);
            contextQuery.Parameters.Add("@AULA", SqlDbType.Int, dadosTurmaAlocacao.Aula);
            contextQuery.Parameters.Add("@DATA_INICIO", SqlDbType.DateTime, dadosTurmaAlocacao.DataInicio);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
