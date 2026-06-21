namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Library;
    using Techne.Lyceum.RN.DTOs;
    using Techne.Lyceum.RN.Entidades;
    using Util;
    using System.Data;
    using Seeduc.Infra.Data;

    public class AreaFormacaoPessoal : RNBase
    {
        public static TceAreaFormacaoPessoal Carregar(int IdAreaFormacaoPessoal)
        {
            try
            {
                TceAreaFormacaoPessoal AFP = new TceAreaFormacaoPessoal();

                using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = "SELECT * FROM TCE_AREA_FORMACAO_PESSOAL WHERE ID_AREA_FORMACAO_PESSOAL = @ID "
                    };
                    contextQuery.Parameters.Add("@ID", IdAreaFormacaoPessoal);

                    using (var reader = ctx.GetDataReader(contextQuery))
                    {
                        while (reader.Read())
                        {
                            AFP.IdAreaFormacaoPessoal = (int)reader["ID_AREA_FORMACAO_PESSOAL"];
                            AFP.Area = (string)reader["AREA"];
                        }
                    }
                    return AFP;                  

                }
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public static int Inserir(TceAreaFormacaoPessoal areaformacaopessoal)
        {
            try
            {

                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = "INSERT INTO TCE_AREA_FORMACAO_PESSOAL(AREA,MATRICULA) VALUES (@area, @matricula) "
                    };
                    contextQuery.Parameters.Add("@area", areaformacaopessoal.Area);
                    contextQuery.Parameters.Add("@matricula", areaformacaopessoal.Matricula);

                    return ctx.ApplyModifications(contextQuery);

                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public static int Alterar(TceAreaFormacaoPessoal areaformacaopessoal)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"UPDATE TCE_AREA_FORMACAO_PESSOAL SET 
                                 AREA = @area,
                                 MATRICULA = @matricula,
                                 DT_ALTERACAO = getdate()
                                 WHERE ID_AREA_FORMACAO_PESSOAL = @ID "
                    };
                    contextQuery.Parameters.Add("@area", areaformacaopessoal.Area);
                    contextQuery.Parameters.Add("@ID", areaformacaopessoal.IdAreaFormacaoPessoal);
                    contextQuery.Parameters.Add("@matricula", areaformacaopessoal.Matricula);

                   return ctx.ApplyModifications(contextQuery);


                }
             }
            catch(Exception e)
            {
                throw e;
            }
        }

        public static DataTable Listar()
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT *
                                                 FROM TCE_AREA_FORMACAO_PESSOAL
                                             order by AREA"
                };

                 return ctx.GetDataTable(contextQuery);
            }
        }

        public static int Remover(int idAreaFormacaoPessoal)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = "DELETE FROM TCE_AREA_FORMACAO_PESSOAL WHERE ID_AREA_FORMACAO_PESSOAL = @ID "
                    };
                    contextQuery.Parameters.Add("@ID", idAreaFormacaoPessoal);

                    return ctx.ApplyModifications(contextQuery);



                }
            }
             
            catch(Exception e)
            {
                throw e;
            }
        }

        public static DataTable ListarAreas()
        {
            var dataTable = new DataTable();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = "SELECT ID_AREA_FORMACAO_PESSOAL,AREA  FROM TCE_AREA_FORMACAO_PESSOAL ORDER BY AREA";
 
                dataTable = ctx.GetDataTable(contextQuery);
            }

            return dataTable;
        }

        public DataTable ObtemListaAreas()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable areas = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT ID_AREA_FORMACAO_PESSOAL,
                                                            AREA
                                            FROM   TCE_AREA_FORMACAO_PESSOAL C
                                              ORDER BY AREA      ";

                areas = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
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
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return areas;
        }

        public static ValidacaoDados Validar(TceAreaFormacaoPessoal areaformacaopessoal)
        {
            var validacao = new ValidacaoDados();
             using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = "SELECT DISTINCT 1  FROM   [dbo].[TCE_AREA_FORMACAO_PESSOAL] WHERE  AREA= @area ";
                contextQuery.Parameters.Add("@area", areaformacaopessoal.Area);

                 
                 object obj = ctx.GetReturnValue(contextQuery);

                 if (obj == null)
                 { 
                     validacao.Valido = true;
                 }
                 else
                 {
                     validacao.Valido = false;
                     validacao.Mensagem = "Já existe uma área com este nome.";

                 }
            }

            return validacao;
        }

        public static ValidacaoDados ValidarExclusao(TceAreaFormacaoPessoal areaformacaopessoal)
        {
            var validacao = new ValidacaoDados();
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = "SELECT DISTINCT 1  FROM   [dbo].[TCE_CURSO_FORMACAO_PESSOAL] WHERE  ID_AREA_FORMACAO_PESSOAL= @area ";
                contextQuery.Parameters.Add("@area", areaformacaopessoal.IdAreaFormacaoPessoal);


                object obj = ctx.GetReturnValue(contextQuery);

                if (obj == null)
                {
                    validacao.Valido = true;
                }
                else
                {
                    validacao.Valido = false;
                    validacao.Mensagem = "Esta área não pode ser excluída devido existir cursos vinculados.";

                }
            }

            return validacao;
        }


    }
}
