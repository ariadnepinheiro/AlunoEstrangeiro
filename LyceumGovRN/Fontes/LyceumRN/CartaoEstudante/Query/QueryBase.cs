using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.Data;
using System.Text.RegularExpressions;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    /// <summary>
    /// Classe responsável por encapsular os métodos de execução das query's, 
    /// a fim de evitar repetição de código.
    /// </summary>
    abstract class QueryBase<T> : SingletonBase<T>
    {
        private const string REGEX_PARAMS = @"(?<param>@\w*)";
        private static readonly Regex getParamRegex = new Regex(REGEX_PARAMS, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private const string MSG_ERRO_PADRAO = "Falha de Acesso ao Banco de Dados. " +
            "Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.";
        protected static string NomeTabela { get; set; } 

        protected QueryBase() {
            Techne.Data.ConnectionList.Current = new HadesConnectionList();
        }

        /// <summary>
        /// A ser usado em instruções SELECT onde somente será retornado um único objeto.
        /// </summary>
        /// <remarks>A lista de parâmetros deve estar na mesma ordem que os parâmetros no SQL.</remarks>
        /// <typeparam name="T">Objeto IEntity que representa a tabela</typeparam>
        /// <param name="query">SQL a ser executada</param>
        /// <param name="paramValues">Valores dos parâmetros do SQL.</param>
        /// <typeparam name="paramValues">Array de object</typeparam>
        /// <returns>Retorna um único objeto do tipo informado por parâmetro.</returns>
        public T ObtemPor<T>(string query, params object[] paramValues) where T : class, IEntity, new()
        {
            T tipo;
            DataContext dataContext = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = query;

                VerificaEAtribuiParametros(query, paramValues, contextQuery);

                tipo = dataContext.TryToBindEntity<T>(contextQuery);
            }
            catch (ContextQueryException ctxEx)
            {
                throw GetDefautException(
                    new Exception(ctxEx.Message, ctxEx)
                );
            }
            catch (Exception ex)
            {
                throw GetDefautException(ex);
            }
            finally
            {
                dataContext.Dispose();
            }

            return tipo;
        }

        protected static void VerificaEAtribuiParametros(string query, object[] paramValues, ContextQuery contextQuery)
        {
            var parameters = getParamRegex.Matches(query);

            if (parameters.Count != paramValues.Length)
                throw new Exception("Erro ao ler os parâmetros.");

            for (int i = 0; i < parameters.Count; i++)
            {
                contextQuery.Parameters.Add(parameters[i].ToString(), paramValues[i]);
            }
        }

        public List<T> ListaTodos<T>() where T : class, IEntity, new()
        {
            string SELECT_ALL = "SELECT * FROM " + NomeTabela;

            List<T> list;
            DataContext dataContext = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = SELECT_ALL;
                list = dataContext.TryToBindEntities<T>(contextQuery).ToList();
            }
            catch (ContextQueryException ctxEx)
            {
                throw GetDefautException(
                    new Exception(ctxEx.Message, ctxEx)
                );
            }
            catch (Exception ex)
            {
                throw GetDefautException(ex);
            }
            finally
            {
                dataContext.Dispose();
            }

            return list;
        }

        /// <summary>
        /// A ser usado em instruções SELECT onde será retornado uma lista de objetos.
        /// </summary>
        /// <remarks>A lista de parâmetros deve estar na mesma ordem que os parâmetros no SQL.</remarks>
        /// <typeparam name="T">Objeto IEntity que representa a tabela</typeparam>
        /// <param name="query">SQL a ser executada</param>
        /// <param name="paramValues">Valores dos parâmetros do SQL.</param>
        /// <typeparam name="paramValues">Array de object</typeparam>
        /// <returns>Retorna uma lista de objetos do tipo informado por parâmetro.</returns>
        public List<T> ListaPor<T>(string query, params object[] paramValues) where T : class, IEntity, new()
        {
            List<T> list;
            DataContext dataContext = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = query;

                VerificaEAtribuiParametros(query, paramValues, contextQuery);

                list = dataContext.TryToBindEntities<T>(contextQuery).ToList();
            }
            catch (ContextQueryException ctxEx)
            {
                throw GetDefautException(
                    new Exception(ctxEx.Message, ctxEx)
                );
            }
            catch (Exception ex)
            {
                throw GetDefautException(ex);
            }
            finally
            {
                dataContext.Dispose();
            }

            return list;
        }

        /// <summary>
        /// A ser usado em instruções SELECT onde será retornado um número identificando se existe ou não o objeto.
        /// </summary>
        /// <remarks>A lista de parâmetros deve estar na mesma ordem que os parâmetros no SQL.</remarks>
        /// <example>SQL: SELECT 1 FROM TABELA WHERE CAMPO = @CAMPO</example>
        /// <typeparam name="T">Objeto IEntity que representa a tabela</typeparam>
        /// <param name="query">SQL a ser executada</param>
        /// <param name="paramValues">Valores dos parâmetros do SQL.</param>
        /// <typeparam name="paramValues">Array de object</typeparam>
        /// <returns>Retorna uma lista de objetos do tipo informado por parâmetro.</returns>
        public bool Possui(string query, params object[] paramValues)
        {
            bool possui = false;

            DataContext dataContext = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = query;

                VerificaEAtribuiParametros(query, paramValues, contextQuery);

                possui = ((dataContext.GetReturnValue<int?>(contextQuery) ?? 0) > 0);
            }
            catch (ContextQueryException ctxEx)
            {
                throw GetDefautException(
                    new Exception(ctxEx.Message, ctxEx)
                );
            }
            catch (Exception ex)
            {
                throw GetDefautException(ex);
            }
            finally
            {
                dataContext.Dispose();
            }

            return possui;
        }

        /// <summary>
        /// A ser usado em instruções INSERT, onde é necessário obter o registro gerado.
        /// </summary>
        /// <remarks>A lista de parâmetros deve estar na mesma ordem que os parâmetros no SQL.</remarks>
        /// <param name="query">SQL a ser executada</param>
        /// <param name="queryRetornoIdentity">SQL para retornar o último registro inserido</param>
        /// <param name="paramValues">Valores dos parâmetros do SQL.</param>
        /// <typeparam name="paramValues">Array de object</typeparam>
        /// <returns>Retorna um objeto do tipo informado por parâmetro (IEntity).</returns>
        public T AplicarModificacoes<T>(string query, string queryRetornoIdentity, params object[] paramValues) where T : class, IEntity, new()
        {
            DataContext dataContext = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            ContextQuery contextQueryReturn = new ContextQuery();
            T obj = default(T);

            try
            {
                contextQuery.Command = query;
                contextQueryReturn.Command = queryRetornoIdentity;

                VerificaEAtribuiParametros(query, paramValues, contextQuery);

                obj = dataContext.ApplyModifications<T>(contextQuery, contextQueryReturn);
            }
            catch (ContextQueryException ctxEx)
            {
                dataContext.Abandon();

                throw GetDefautException(
                    new Exception(ctxEx.Message, ctxEx)
                );
            }
            catch (Exception ex)
            {
                dataContext.Abandon();

                throw GetDefautException(ex);
            }
            finally
            {
                dataContext.Dispose();
            }

            return obj;
        }

        /// <summary>
        /// A ser usado em instruções INSERT, UPDATE E DELETE.
        /// </summary>
        /// <remarks>A lista de parâmetros deve estar na mesma ordem que os parâmetros no SQL.</remarks>
        /// <param name="query">SQL a ser executada</param>
        /// <param name="paramValues">Valores dos parâmetros do SQL.</param>
        /// <typeparam name="paramValues">Array de object</typeparam>
        public void AplicarModificacoes(string query, params object[] paramValues)
		{
            DataContext dataContext = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = query;

                VerificaEAtribuiParametros(query, paramValues, contextQuery);

                dataContext.ApplyModifications(contextQuery);
            }
            catch (ContextQueryException ctxEx)
            {
                dataContext.Abandon();

                throw GetDefautException(
                    new Exception(ctxEx.Message, ctxEx)
                );
            }
            catch (Exception ex)
            {
                dataContext.Abandon();

                throw GetDefautException(ex);
            }
            finally
            {
                dataContext.Dispose();
            }
		}

        /// <summary>
        /// A ser usado em instruções INSERT, UPDATE E DELETE a serem executadas em lote.
        /// </summary>
        /// <param name="queries">SQLs a serem executadas numa mesma transação</param>
        /// <typeparam name="queries">Array de ContextQuery</typeparam>
        public void AplicarModificacoesEmLote(ContextQuery[] queries)
		{
            DataContext dataContext = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                foreach (var sql in queries)
                {
                    if (sql != null)
                        dataContext.ApplyModifications(sql);
                }
            }
            catch (ContextQueryException ctxEx)
            {
                dataContext.Abandon();

                throw GetDefautException(
                    new Exception(ctxEx.Message, ctxEx)
                );
            }
            catch (Exception ex)
            {
                dataContext.Abandon();

                throw GetDefautException(ex);
            }
            finally
            {
                dataContext.Dispose();
            }	
		}
        
        /// <summary>
        /// A ser usado em instruções SELECT onde será retornado uma lista de objetos primitivos.
        /// </summary>
        /// <remarks>A lista de parâmetros deve estar na mesma ordem que os parâmetros no SQL.</remarks>
        /// <typeparam name="T">Tipo primitivo a ser retornado na lista</typeparam>
        /// <param name="query">SQL a ser executada</param>
        /// <param name="paramValues">Valores dos parâmetros do SQL.</param>
        /// <typeparam name="paramValues">Array de object</typeparam>
        /// <returns>Retorna uma lista de objetos do tipo primitivo</returns>
        public List<T> ListaPorDe<T>(string query, params object[] paramValues)
        {
            List<T> objs = new List<T>();

            DataContext dataContext = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = query;

                VerificaEAtribuiParametros(query, paramValues, contextQuery);

                objs = dataContext.TryToBind<T>(contextQuery).ToList<T>();
            }
            catch (ContextQueryException ctxEx)
            {
                throw GetDefautException(
                    new Exception(ctxEx.Message, ctxEx)
                );
            }
            catch (Exception ex)
            {
                throw GetDefautException(ex);
            }
            finally
            {
                dataContext.Dispose();
            }

            return objs;
        }

        /// <summary>
        /// A ser usado em instruções SELECT onde será retornado um único valor.
        /// </summary>
        /// <remarks>Ideal para retorno de funções agregadas, tais como COUNT, SUM, AVG, etc.</remarks>
        /// <typeparam name="T">Tipo primitivo a ser retornado na lista</typeparam>
        /// <param name="query">SQL a ser executada</param>
        /// <param name="paramValues">Valores dos parâmetros do SQL.</param>
        /// <typeparam name="paramValues">Array de object</typeparam>
        /// <returns>Tipo primitivo</returns>
        public T ObtemValorSimples<T>(string query, params object[] paramValues)
        {
            T valor;

            DataContext dataContext = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = query;

                VerificaEAtribuiParametros(query, paramValues, contextQuery);

                valor = dataContext.GetReturnValue<T>(contextQuery);
            }
            catch (ContextQueryException ctxEx)
            {
                throw GetDefautException(
                    new Exception(ctxEx.Message, ctxEx)
                );
            }
            catch (Exception ex)
            {
                throw GetDefautException(ex);
            }
            finally
            {
                dataContext.Dispose();
            }

            return valor;
        }

        /// <summary>
        /// A ser usado em instruções SELECT com a cláusula join onde será retornado um DataTable.
        /// </summary>
        /// <remarks>A lista de parâmetros deve estar na mesma ordem que os parâmetros no SQL.</remarks>
        /// <param name="query">SQL a ser executada</param>
        /// <param name="paramValues">Valores dos parâmetros do SQL.</param>
        /// <typeparam name="paramValues">Array de object</typeparam>
        /// <returns>Retorna um DataTable.</returns>
        public System.Data.DataTable ObterDataTable(string query, params object[] paramValues)
        {
            System.Data.DataTable dt;
            DataContext dataContext = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = query;

                var parameters = getParamRegex.Matches(query);

                if (parameters.Count != paramValues.Length)
                    throw new Exception("Erro ao ler os parâmetros.");

                for (int i = 0; i < parameters.Count; i++)
                {
                    contextQuery.Parameters.Add(parameters[i].ToString(), paramValues[i]);
                }

                dt = dataContext.GetDataTable(contextQuery);
            }
            catch (ContextQueryException ctxEx)
            {
                throw GetDefautException(
                    new Exception(ctxEx.Message, ctxEx)
                );
            }
            catch (Exception ex)
            {
                throw GetDefautException(ex);
            }
            finally
            {
                dataContext.Dispose();
            }

            return dt;
        }

        static Exception GetDefautException(Exception ex)
        {
            string msgErro = string.Empty;

            if (!Convert.ToString(ex.Message).Contains(MSG_ERRO_PADRAO))
            {                            
                msgErro = string.Format("{0}\nErro gerado: {1}", MSG_ERRO_PADRAO, ex.Message);

                if (ex.InnerException != null)
                    string.Concat(msgErro, "\nErro interno: " + ex.InnerException.Message);
            }
            else
            {
                msgErro = ex.Message;
            }

            return new Exception(msgErro);
        }

        static string GetSelectPadrao()
        {
            return "SELECT * FROM " + NomeTabela;
        }
    }
}