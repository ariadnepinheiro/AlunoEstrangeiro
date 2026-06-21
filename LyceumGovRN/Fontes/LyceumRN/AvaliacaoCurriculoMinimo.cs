namespace Techne.Lyceum.RN
{
    using System.Data;
    using Seeduc.Infra.Data;
    using Techne.Lyceum.RN.Entidades;
    using System;
    using Techne.Lyceum.RN.Util;

    public class AvaliacaoCurriculoMinimo
    {
        public static TceAvaliacaoCurriculoMinimo Carregar(int IdAvaliacaoCurriculoMinimo)
        {
            try
            {
                TceAvaliacaoCurriculoMinimo ACM = new TceAvaliacaoCurriculoMinimo();

                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = "SELECT * FROM TCE_AVALIACAO_CM WHERE ID_AVALIACAO_CM = @ID "
                    };
                    contextQuery.Parameters.Add("@ID", IdAvaliacaoCurriculoMinimo);

                    using (var reader = ctx.GetDataReader(contextQuery))
                    {
                        while (reader.Read())
                        {
                            ACM.IdAvaliacaoCurriculoMinimo = (int)reader["ID_AVALIACAO_CM"];
                            ACM.Ano = Convert.ToInt32((decimal)reader["ANO"]);
                            ACM.Periodo = Convert.ToInt32((decimal)reader["PERIODO"]);
                            ACM.Subperiodo = Convert.ToInt32((decimal)reader["SUBPERIODO"]);
                            ACM.Ordem = (int)reader["ORDEM"];
                            ACM.Avaliacao = (string)reader["AVALIACAO"];
                            ACM.Habilitado = (bool)reader["HABILITADO"]; 

                        }
                    }
                    return ACM;

                }
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public static int Inserir(TceAvaliacaoCurriculoMinimo avaliacaoCurriculoMinimo)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" INSERT INTO TCE_AVALIACAO_CM
                            (ANO, PERIODO, SUBPERIODO, ORDEM, AVALIACAO, HABILITADO, MATRICULA)
                            VALUES
                            (@ANO, @PERIODO, @SUBPERIODO, @ORDEM, @AVALIACAO, @HABILITADO, @MATRICULA) "
                    };
                    contextQuery.Parameters.Add("@ANO", avaliacaoCurriculoMinimo.Ano);
                    contextQuery.Parameters.Add("@PERIODO", avaliacaoCurriculoMinimo.Periodo);
                    contextQuery.Parameters.Add("@SUBPERIODO", avaliacaoCurriculoMinimo.Subperiodo);
                    contextQuery.Parameters.Add("@ORDEM", avaliacaoCurriculoMinimo.Ordem);
                    contextQuery.Parameters.Add("@AVALIACAO", avaliacaoCurriculoMinimo.Avaliacao);
                    contextQuery.Parameters.Add("@HABILITADO", avaliacaoCurriculoMinimo.Habilitado);
                    contextQuery.Parameters.Add("@MATRICULA", avaliacaoCurriculoMinimo.Matricula);

                    return ctx.ApplyModifications(contextQuery);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static int Alterar(TceAvaliacaoCurriculoMinimo avaliacaoCurriculoMinimo)
        {
            //Ver quais dados podem ser alterados
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" UPDATE TCE_AVALIACAO_CM
                        SET ANO = @ANO, 
                        PERIODO = @PERIODO, 
                        SUBPERIODO = @SUBPERIODO, 
                        ORDEM = @ORDEM, 
                        AVALIACAO = @AVALIACAO, 
                        HABILITADO = @HABILITADO, 
                        MATRICULA = @MATRICULA,
                        DT_ALTERACAO = GETDATE()
                        WHERE ID_AVALIACAO_CM = @ID "
                    };

                    contextQuery.Parameters.Add("@ID", avaliacaoCurriculoMinimo.IdAvaliacaoCurriculoMinimo);
                    contextQuery.Parameters.Add("@ANO", avaliacaoCurriculoMinimo.Ano);
                    contextQuery.Parameters.Add("@PERIODO", avaliacaoCurriculoMinimo.Periodo);
                    contextQuery.Parameters.Add("@SUBPERIODO", avaliacaoCurriculoMinimo.Subperiodo);
                    contextQuery.Parameters.Add("@ORDEM", avaliacaoCurriculoMinimo.Ordem);
                    contextQuery.Parameters.Add("@AVALIACAO", avaliacaoCurriculoMinimo.Avaliacao);
                    contextQuery.Parameters.Add("@HABILITADO", avaliacaoCurriculoMinimo.Habilitado);
                    contextQuery.Parameters.Add("@MATRICULA", avaliacaoCurriculoMinimo.Matricula);

                    return ctx.ApplyModifications(contextQuery);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static int Remover(int idCAvaliacaoCurriculoMinimo)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = "DELETE FROM TCE_AVALIACAO_CM WHERE ID_AVALIACAO_CM = @ID "
                    };
                    contextQuery.Parameters.Add("@ID", idCAvaliacaoCurriculoMinimo);

                    return ctx.ApplyModifications(contextQuery);
                }
            }

            catch (Exception e)
            {
                throw e;
            }
        }
        
        public static DataTable Listar(decimal ano, decimal periodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" select * from TCE_AVALIACAO_CM 
                            WHERE ANO = @ANO
                            AND PERIODO = @PERIODO
                            "
                };

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static ValidacaoDados Validar(TceAvaliacaoCurriculoMinimo avaliacaoCurriculoMinimo)
        {
            var validacao = new ValidacaoDados();
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @"SELECT 1 FROM [TCE_AVALIACAO_CM] WHERE  
                                    ANO = @ANO
                                    AND PERIODO =  @PERIODO
                                    AND SUBPERIODO = @SUBPERIODO
                                     and (AVALIACAO = @AVALIACAO
                                    or ORDEM = @ORDEM)";

                if (avaliacaoCurriculoMinimo.IdAvaliacaoCurriculoMinimo != 0)
                    contextQuery.Command += " AND ID_AVALIACAO_CM <> @ID_AVALIACAO_CM ";

                contextQuery.Parameters.Add("@ID_AVALIACAO_CM", avaliacaoCurriculoMinimo.IdAvaliacaoCurriculoMinimo);
                contextQuery.Parameters.Add("@AVALIACAO", avaliacaoCurriculoMinimo.Avaliacao);
                contextQuery.Parameters.Add("@ORDEM", avaliacaoCurriculoMinimo.Ordem);
                contextQuery.Parameters.Add("@ANO", avaliacaoCurriculoMinimo.Ano);
                contextQuery.Parameters.Add("@PERIODO", avaliacaoCurriculoMinimo.Periodo);
                contextQuery.Parameters.Add("@SUBPERIODO", avaliacaoCurriculoMinimo.Subperiodo);


                object obj = ctx.GetReturnValue(contextQuery);

                if (obj == null)
                {
                    validacao.Valido = true;
                }
                else
                {
                    validacao.Valido = false;
                    validacao.Mensagem = "Já existe uma Avaliação com este nome/ordem.";

                }
            }

            return validacao;
        }

        public static ValidacaoDados ValidarExclusao(TceAvaliacaoCurriculoMinimo avaliacaoCurriculoMinimo)
        {
            var validacao = new ValidacaoDados();
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @"SELECT DISTINCT 1  FROM   [dbo].[TCE_AVALIACAO_CM_DOCENTE] 
                                        WHERE  ID_AVALIACAO_CM= @ID ";
                contextQuery.Parameters.Add("@ID", avaliacaoCurriculoMinimo.IdAvaliacaoCurriculoMinimo);


                object obj = ctx.GetReturnValue(contextQuery);

                if (obj == null)
                {
                    validacao.Valido = true;
                }
                else
                {
                    validacao.Valido = false;
                    validacao.Mensagem = "Esta avaliação não pode ser excluída devido existir currículo mínimo vinculado.";

                }
            }

            return validacao;
        }
        public static DataTable ListarHabilitadas(decimal ano, decimal periodo,decimal subperiodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" select * from TCE_AVALIACAO_CM 
                            WHERE ANO = @ANO
                            AND PERIODO = @PERIODO
                            AND SUBPERIODO = @SUBPERIODO
                            AND HABILITADO = 1
                            "
                };

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@SUBPERIODO", subperiodo);

                return ctx.GetDataTable(contextQuery);
            }
        }

    }
}