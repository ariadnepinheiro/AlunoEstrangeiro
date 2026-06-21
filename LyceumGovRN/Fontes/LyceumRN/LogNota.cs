namespace Techne.Lyceum.RN
{
    using System;
    using Seeduc.Infra.Data;
    using Techne.Lyceum.RN.Entidades;

    public class LogNota
    {
        internal static ContextQuery Inserir(LyNota nota, string usuario, string conceito, string recuperacaoParalela, string semAvaliacao, string justificativa, int operacao)
        {
            var contextQuery = new ContextQuery
                               {
                                   Command = @"INSERT  INTO dbo.LY_LOG_NOTA
                                                       (
                                                        ALUNO,
                                                        DISCIPLINA,
                                                        TURMA,
                                                        ANO,
                                                        SEMESTRE,
                                                        PROVA,
                                                        VALOR_ANTERIOR,
                                                        VALOR_ATUAL,
                                                        RECUPERACAO_PARALELA_ANTERIOR,
                                                        RECUPERACAO_PARALELA_ATUAL,
                                                        SEM_AVALIACAO_ANTERIOR,
                                                        SEM_AVALIACAO_ATUAL,
                                                        JUSTIFICATIVA_ANTERIOR,
                                                        JUSTIFICATIVA_ATUAL,
                                                        USUARIO,
                                                        DATA_OPERACAO,
                                                        OPERACAO
                                                       )
                                               VALUES  (
                                                        @ALUNO,
                                                        @DISCIPLINA,
                                                        @TURMA,
                                                        @ANO,
                                                        @SEMESTRE,
                                                        @PROVA,
                                                        @VALOR_ANTERIOR,
                                                        @VALOR_ATUAL,
                                                        @RECUPERACAO_PARALELA_ANTERIOR,
                                                        @RECUPERACAO_PARALELA_ATUAL,
                                                        @SEM_AVALIACAO_ANTERIOR,
                                                        @SEM_AVALIACAO_ATUAL,
                                                        @JUSTIFICATIVA_ANTERIOR,
                                                        @JUSTIFICATIVA_ATUAL,
                                                        @USUARIO,
                                                        @DATA_OPERACAO,
                                                        @OPERACAO
                                                        )"
                               };

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, nota.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", TechneDbType.T_CODIGO, nota.Disciplina);
            contextQuery.Parameters.Add("@TURMA", nota.Turma);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, nota.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, nota.Semestre);
            contextQuery.Parameters.Add("@PROVA", TechneDbType.T_PROVA10, nota.Prova);
            contextQuery.Parameters.Add("@VALOR_ANTERIOR", TechneDbType.T_ALFASMALL, conceito);
            contextQuery.Parameters.Add("@VALOR_ATUAL", TechneDbType.T_ALFASMALL, nota.Conceito);
            contextQuery.Parameters.Add("@RECUPERACAO_PARALELA_ANTERIOR", recuperacaoParalela);
            contextQuery.Parameters.Add("@RECUPERACAO_PARALELA_ATUAL", nota.RecuperacaoParalela);
            contextQuery.Parameters.Add("@SEM_AVALIACAO_ANTERIOR", semAvaliacao);
            contextQuery.Parameters.Add("@SEM_AVALIACAO_ATUAL", nota.SemAvaliacao);
            contextQuery.Parameters.Add("@JUSTIFICATIVA_ANTERIOR", justificativa);
            contextQuery.Parameters.Add("@JUSTIFICATIVA_ATUAL", nota.Justificativa);
            contextQuery.Parameters.Add("@USUARIO", TechneDbType.T_ALFALARGE, usuario);
            contextQuery.Parameters.Add("@DATA_OPERACAO", TechneDbType.T_DATA, DateTime.Now);
            contextQuery.Parameters.Add("@OPERACAO", operacao);

            return contextQuery;
        }
    }
}