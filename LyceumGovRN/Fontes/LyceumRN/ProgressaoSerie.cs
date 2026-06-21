using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN
{
    public class ProgressaoSerie
    {
        public bool PossuiCursoPor(DataContext ctx, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                FROM  DBO.PROGRESSAOSERIE
                                WHERE CURSOID = @CURSO ";

            contextQuery.Parameters.Add("@CURSO", curso);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public static RN.Entidades.ProgressaoSerie Carregar(string cursoAtual, int serieAtual)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery =
                    new ContextQuery(
                        @" SELECT *
                            FROM PROGRESSAOSERIE
                            WHERE CURSOID = @CURSOID
                            AND SERIEID = @SERIEID
                            ");

                contextQuery.Parameters.Add("@CURSOID", cursoAtual);
                contextQuery.Parameters.Add("@SERIEID", serieAtual);

                return ctx.TryToBindEntity<RN.Entidades.ProgressaoSerie>(contextQuery);
            }
        }

        public bool EhCursoProximoCurso(string cursoOrigem, string CursoDestino)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool resultado = false;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT COUNT(*)
                                FROM    PROGRESSAOSERIE
                                WHERE   PROXIMOCURSOID = @PROXIMOCURSOID
                                        AND CURSOID = @CURSOID "
                };

                contextQuery.Parameters.Add("@PROXIMOCURSOID", CursoDestino);
                contextQuery.Parameters.Add("@CURSOID", cursoOrigem);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    resultado = true;
                }

                return resultado;
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }
    }
}
