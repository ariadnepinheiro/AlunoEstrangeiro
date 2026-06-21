namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections.Generic;
    using Seeduc.Infra.Data;
    using Techne.Lyceum.RN.Entidades;

    public class ProtocoloNota : RNBase
    {
        public static void Inserir(DataContext dataContext, TceProtocoloNota protocoloNota)
        {
            protocoloNota.Tipo = "T";

            var contextQuery = new ContextQuery
                {
                    Command = @"INSERT INTO dbo.TCE_PROTOCOLO_NOTA
                                        (
                                         DISCIPLINA,
                                         NOME_DISCIPLINA,
                                         TURMA,
                                         ANO,
                                         PERIODO,
                                         SUBPERIODO,
                                         MATRICULA,
                                         TIPO
                                        )
                                VALUES  (
                                         @DISCIPLINA,
                                         @NOME_DISCIPLINA,
                                         @TURMA,
                                         @ANO,
                                         @PERIODO,
                                         @SUBPERIODO,
                                         @MATRICULA,
                                         @TIPO
                                        );
                                SELECT SCOPE_IDENTITY();"
                };

            contextQuery.Parameters.Add("@DISCIPLINA", protocoloNota.Disciplina);
            contextQuery.Parameters.Add("@NOME_DISCIPLINA", protocoloNota.NomeDisciplina);
            contextQuery.Parameters.Add("@TURMA", protocoloNota.Turma);
            contextQuery.Parameters.Add("@ANO", protocoloNota.Ano);
            contextQuery.Parameters.Add("@PERIODO", protocoloNota.Periodo);
            contextQuery.Parameters.Add("@SUBPERIODO", protocoloNota.Subperiodo);
            contextQuery.Parameters.Add("@MATRICULA", protocoloNota.Matricula);
            contextQuery.Parameters.Add("@TIPO", protocoloNota.Tipo);

            protocoloNota.IdProtocoloNota = Convert.ToInt32(dataContext.GetReturnValue(contextQuery));
        }

        public static ICollection<TceProtocoloNota> Listar(string matricula)
        {
            if (string.IsNullOrEmpty(matricula))
            {
                return null;
            }

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery
                    {
                        Command = @"SELECT  *
                                    FROM    TCE_PROTOCOLO_NOTA
                                    WHERE   MATRICULA = @MATRICULA
                                    ORDER BY DT_CADASTRO DESC"
                    };

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                return ctx.TryToBindEntities<TceProtocoloNota>(contextQuery);
            }
        }
    }
}