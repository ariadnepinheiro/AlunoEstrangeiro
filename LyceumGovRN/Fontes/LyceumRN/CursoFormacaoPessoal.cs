using System;

namespace Techne.Lyceum.RN
{
    using System.Data;
    using Seeduc.Infra.Data;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Lyceum.RN.Util;

    public class CursoFormacaoPessoal : RNBase
    {
        public static TceCursoFormacaoPessoal Carregar(int idCursoFormacaoPessoal)
        {
            try
            {
                var cpf = new TceCursoFormacaoPessoal();

                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = "SELECT * FROM TCE_CURSO_FORMACAO_PESSOAL WHERE ID_CURSO_FORMACAO_PESSOAL = @ID "
                    };
                    contextQuery.Parameters.Add("@ID", idCursoFormacaoPessoal);

                    using (var reader = ctx.GetDataReader(contextQuery))
                    {
                        while (reader.Read())
                        {
                            cpf.IdCursoFormacaoPessoal = (int)reader["ID_CURSO_FORMACAO_PESSOAL"];
                            cpf.IdAreaFormacaoPessoal = (int)reader["ID_AREA_FORMACAO_PESSOAL"];
                            cpf.Curso = (string)reader["CURSO"];
                            cpf.Grau = (string)reader["GRAU"];
                        }
                    }
                    return cpf;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static int Inserir(TceCursoFormacaoPessoal cursoformacaopessoal)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"INSERT INTO TCE_CURSO_FORMACAO_PESSOAL(ID_AREA_FORMACAO_PESSOAL,CURSO,GRAU,MATRICULA) 
                                VALUES (@area, @curso, @grau, @matricula) "
                    };
                    contextQuery.Parameters.Add("@area", cursoformacaopessoal.IdAreaFormacaoPessoal);
                    contextQuery.Parameters.Add("@curso", cursoformacaopessoal.Curso);
                    contextQuery.Parameters.Add("@grau", cursoformacaopessoal.Grau);
                    contextQuery.Parameters.Add("@matricula", cursoformacaopessoal.Matricula);

                    return ctx.ApplyModifications(contextQuery);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static int Alterar(TceCursoFormacaoPessoal cursoformacaopessoal)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"UPDATE TCE_CURSO_FORMACAO_PESSOAL SET 
                                 CURSO = @curso,
                                 GRAU = @grau,
                                 MATRICULA = @matricula,
                                 DT_ALTERACAO = getdate()
                                 WHERE ID_CURSO_FORMACAO_PESSOAL = @ID "
                    };
                    contextQuery.Parameters.Add("@curso", cursoformacaopessoal.Curso);
                    contextQuery.Parameters.Add("@grau", cursoformacaopessoal.Grau);
                    contextQuery.Parameters.Add("@ID", cursoformacaopessoal.IdCursoFormacaoPessoal);
                    contextQuery.Parameters.Add("@matricula", cursoformacaopessoal.Matricula);

                    return ctx.ApplyModifications(contextQuery);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static DataTable Listar(int idAreaFormacaoPessoal)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT *
                                                 FROM dbo.TCE_CURSO_FORMACAO_PESSOAL C
                                                inner join TCE_AREA_FORMACAO_PESSOAL A on C.ID_AREA_FORMACAO_PESSOAL=A.ID_AREA_FORMACAO_PESSOAL
                                                WHERE c.ID_AREA_FORMACAO_PESSOAL = @ID_AREA_FORMACAO_PESSOAL
                                                ORDER BY AREA,CURSO"
                };

                contextQuery.Parameters.Add("@ID_AREA_FORMACAO_PESSOAL", idAreaFormacaoPessoal);
                              
                return ctx.GetDataTable(contextQuery);
            }
        }

        public static int Remover(int idCursoFormacaoPessoal)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = "DELETE FROM TCE_CURSO_FORMACAO_PESSOAL WHERE ID_CURSO_FORMACAO_PESSOAL = @ID "
                    };
                    contextQuery.Parameters.Add("@ID", idCursoFormacaoPessoal);

                    return ctx.ApplyModifications(contextQuery);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static ValidacaoDados Validar(TceCursoFormacaoPessoal cursoformacaopessoal)
        {
            var validacao = new ValidacaoDados();
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = "SELECT 1 FROM [TCE_CURSO_FORMACAO_PESSOAL] WHERE  CURSO= @curso and ID_CURSO_FORMACAO_PESSOAL <> @ID_CURSO_FORMACAO_PESSOAL";
                contextQuery.Parameters.Add("@curso", cursoformacaopessoal.Curso);
                contextQuery.Parameters.Add("@ID_CURSO_FORMACAO_PESSOAL", cursoformacaopessoal.IdCursoFormacaoPessoal);
                               
                object obj = ctx.GetReturnValue(contextQuery);

                if (obj == null)
                {
                    validacao.Valido = true;
                }
                else
                {
                    validacao.Valido = false;
                    validacao.Mensagem = "Já existe um curso com este nome.";
                }
            }

            return validacao;
        }

        public static DataTable ListarCursoArea()
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT AREA + ' - ' + CURSO AS AREA_CURSO, CAST(c.ID_AREA_FORMACAO_PESSOAL AS VARCHAR) + '-' + CAST(ID_CURSO_FORMACAO_PESSOAL AS VARCHAR) AS CODIGO
                                                 FROM dbo.TCE_CURSO_FORMACAO_PESSOAL C
                                                inner join TCE_AREA_FORMACAO_PESSOAL A on C.ID_AREA_FORMACAO_PESSOAL=A.ID_AREA_FORMACAO_PESSOAL
                                                ORDER BY AREA,CURSO"
                };

               return ctx.GetDataTable(contextQuery);
            }
        }
    }
}
