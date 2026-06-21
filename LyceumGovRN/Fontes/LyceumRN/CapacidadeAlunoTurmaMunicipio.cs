using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    /// <summary>
    /// 
    /// </summary>
    public class CapacidadeAlunoTurmaMunicipio : RNBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public static Entidades.CapacidadeAlunoTurmaMunicipio RetornaCapacidaDeAlunoTurmaPor(int pId)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {
                var contextQuery =
                    new ContextQuery(
                        @"
                        SELECT capacidadealunoturmamunicipioid,
                               ano,
                               periodo,
                               capacidade,
                               matricula,
                               datacadastro,
                               dataalteracao,
                               tipo,
                               municipioid
                        FROM   capacidadealunoturmamunicipio
                        WHERE  capacidadealunoturmamunicipioid = pId");

                contextQuery.Parameters.Add("@pId", pId);

                return ctx.TryToBindEntity<Entidades.CapacidadeAlunoTurmaMunicipio>(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pObj"></param>
        /// <returns></returns>
        public static Entidades.CapacidadeAlunoTurmaMunicipio RetornaPorExemplo(Entidades.CapacidadeAlunoTurmaMunicipio pObj)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {
                var contextQuery =
                    new ContextQuery(
                        @"
                        SELECT capacidadealunoturmamunicipioid,
                               ano,
                               periodo,
                               capacidade,
                               matricula,
                               datacadastro,
                               dataalteracao,
                               tipo,
                               municipioid
                        FROM   capacidadealunoturmamunicipio
                        WHERE  capacidadealunoturmamunicipioid = pId");

                contextQuery.Parameters.Add("@pId", pObj.CapacidadeAlunoTurmaMunicipioId);

                return ctx.TryToBindEntity<Entidades.CapacidadeAlunoTurmaMunicipio>(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ICollection<Entidades.CapacidadeAlunoTurmaMunicipio> RetornaTodos()
        {
            var ctx = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {
                var contextQuery =
                    new ContextQuery(
                        @"
                        SELECT capacidadealunoturmamunicipioid,
                               ano,
                               periodo,
                               capacidade,
                               matricula,
                               datacadastro,
                               dataalteracao,
                               tipo,
                               municipioid
                        FROM   capacidadealunoturmamunicipio");

                return ctx.TryToBindEntities<Entidades.CapacidadeAlunoTurmaMunicipio>(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pObj"></param>
        /// <returns></returns>
        public static void Grava(Entidades.CapacidadeAlunoTurmaMunicipio pObj)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" 
                                INSERT INTO capacidadealunoturmamunicipio
                                            (ano,
                                             periodo,
                                             capacidade,
                                             matricula,
                                             datacadastro,
                                             dataalteracao,
                                             tipo,
                                             municipioid)
                                VALUES      (@ANO,
                                             @PERIODO,
                                             @CAPACIDADE,
                                             @MATRICULA,
                                             GETDATE(),
                                             GETDATE(),
                                             @TIPO,
                                             @MUNICIPIOID)  "
                };

                contextQuery.Parameters.Add("@ANO", pObj.Ano);
                contextQuery.Parameters.Add("@PERIODO", pObj.Periodo);
                contextQuery.Parameters.Add("@CAPACIDADE", pObj.Capacidade);
                contextQuery.Parameters.Add("@MATRICULA", pObj.Matricula);
                contextQuery.Parameters.Add("@TIPO", pObj.Tipo);
                contextQuery.Parameters.Add("@MUNICIPIOID", pObj.MunicipioId);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pObj"></param>
        public static void Atualiza(Entidades.CapacidadeAlunoTurmaMunicipio pObj)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"
                                UPDATE capacidadealunoturmamunicipio
                                SET    capacidade = @CAPACIDADE,
                                       matricula = @MATRICULA,
                                       dataalteracao = Getdate()
                                WHERE  capacidadealunoturmamunicipioid = @Id AND municipioid = @MUNICIPIOID AND tipo = @TIPO"
                };

                //contextQuery.Parameters.Add("@ANO", pObj.Ano);
                //contextQuery.Parameters.Add("@PERIODO", pObj.Periodo);
                contextQuery.Parameters.Add("@CAPACIDADE", pObj.Capacidade);
                contextQuery.Parameters.Add("@MATRICULA", pObj.Matricula);
                contextQuery.Parameters.Add("@TIPO", pObj.Tipo);
                contextQuery.Parameters.Add("@MUNICIPIOID", pObj.MunicipioId);
                contextQuery.Parameters.Add("@Id", pObj.CapacidadeAlunoTurmaMunicipioId);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pObj"></param>
        public static void Remove(Entidades.CapacidadeAlunoTurmaMunicipio pObj)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"
                                DELETE FROM capacidadealunoturmamunicipio
                                WHERE  capacidadealunoturmamunicipioid = @pId "
                };

                contextQuery.Parameters.Add("@pId", pObj.CapacidadeAlunoTurmaMunicipioId);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pId"></param>
        public static void Remove(int pId)
        {
            if (pId < 1)
            {
                return;
            }

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" DELETE from dbo.capacidadealunoturmamunicipio
                                where CAPACIDADEALUNOTURMAMUNICIPIOID = @pId "
                    };

                    contextQuery.Parameters.Add("@pId", pId);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception ex)
                {
                    ctx.Abandon();
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                       Environment.NewLine, Convert.ToString(ex.Message));
                    throw new Exception(mensagem);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ano"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public static DataTable RetornarCapacidadeAlunoMunicipioPor(decimal ano, decimal periodo)
        {
            DataTable objDt = null;
            var ctx = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"  
                                SELECT CONVERT(VARCHAR(4), CT.ano) + '/'
                                       + CONVERT(VARCHAR(1), CT.periodo)        AS ANOPERIODO,
                                       MU.uf_sigla                              AS UFSIGLA,
                                       MU.nome                                  AS NOME,
                                       CASE CT.tipo
                                         WHEN 0 THEN 'MÍNIMA'
                                         WHEN 1 THEN 'MÁXIMA'
                                       END                                      AS TIPO,
                                       CT.capacidade                            AS CAPACIDADE,
                                       CONVERT(VARCHAR(10), dataalteracao, 103) AS DATAALTERACAO,
                                       CT.capacidadealunoturmamunicipioid       AS CAPACIDADEID,
                                       mu.CODIGO								AS CODMUNICIPIO
                                FROM   dbo.capacidadealunoturmamunicipio CT
                                       INNER JOIN dbo.municipio MU
                                               ON ct.municipioid = mu.codigo
                                WHERE  CT.ano = @ANO
                                       AND CT.periodo = @PERIODO
                                ORDER  BY CT.ano,
                                          CT.periodo  "
                };

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                objDt = new DataTable();
                objDt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }

            return objDt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ano"></param>
        /// <param name="periodo"></param>
        /// <param name="censo"></param>
        /// <returns></returns>
        public Entidades.CapacidadeAlunoTurmaMunicipio RetornaCapacidaMaximaDeAlunoTurmaMunicipioPor(int ano, int periodo, string censo)
        {
            Entidades.CapacidadeAlunoTurmaMunicipio objCapacidadeAlunoTurmaMunicipio = null;

            var ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;

            try
            {
                var contextQuery =
                    new ContextQuery(
                        @"SELECT M.*
                        FROM   capacidadealunoturmamunicipio M
                               LEFT OUTER JOIN ly_unidade_ensino U
                                      ON M.municipioid = U.municipio
                        WHERE  M.ano = @ANO
                               AND M.periodo = @PERIODO
                               AND U.unidade_ens = @CENSO
                               AND M.TIPO = 1 ");

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CENSO", censo);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    objCapacidadeAlunoTurmaMunicipio = new Entidades.CapacidadeAlunoTurmaMunicipio
                    {
                        CapacidadeAlunoTurmaMunicipioId = !String.IsNullOrEmpty(reader["CAPACIDADEALUNOTURMAMUNICIPIOID"].ToString()) ? Convert.ToInt32(reader["CAPACIDADEALUNOTURMAMUNICIPIOID"]) : default(int),
                        MunicipioId = !String.IsNullOrEmpty(reader["MUNICIPIOID"].ToString()) ? Convert.ToString(reader["MUNICIPIOID"]) : default(string),
                        Ano = !String.IsNullOrEmpty(reader["ANO"].ToString()) ? Convert.ToDecimal(reader["ANO"]) : default(decimal),
                        Periodo = !String.IsNullOrEmpty(reader["PERIODO"].ToString()) ? Convert.ToDecimal(reader["PERIODO"]) : default(decimal),
                        Tipo = !String.IsNullOrEmpty(reader["PERIODO"].ToString()) ? Convert.ToInt32(reader["PERIODO"]) : default(int),
                        Capacidade = !String.IsNullOrEmpty(reader["CAPACIDADE"].ToString()) ? Convert.ToInt32(reader["CAPACIDADE"]) : default(int),
                        Matricula = !String.IsNullOrEmpty(reader["MATRICULA"].ToString()) ? Convert.ToString(reader["MATRICULA"]) : default(string),
                        DataCadastro = !String.IsNullOrEmpty(reader["DATACADASTRO"].ToString()) ? Convert.ToDateTime(reader["DATACADASTRO"]) : default(DateTime),
                        DataAlteracao = !String.IsNullOrEmpty(reader["DATAALTERACAO"].ToString()) ? Convert.ToDateTime(reader["DATAALTERACAO"]) : default(DateTime),
                    };
                }

                return objCapacidadeAlunoTurmaMunicipio;
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacidaDeAlunoTurmaMunicipio"></param>
        /// <returns></returns>
        public static ValidacaoDados Validar(Entidades.CapacidadeAlunoTurmaMunicipio capacidaDeAlunoTurmaMunicipio)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (capacidaDeAlunoTurmaMunicipio == null)
            {
                return validacaoDados;
            }

            if (capacidaDeAlunoTurmaMunicipio.Ano <= 0)
            {
                mensagens.Add("Campos Ano é obrigatório.");
            }

            if (capacidaDeAlunoTurmaMunicipio.Periodo < 0)
            {
                mensagens.Add("Campos Periodo é obrigatório.");
            }

            if (capacidaDeAlunoTurmaMunicipio.Capacidade <= 0)
            {
                mensagens.Add("Campos Capacidade não pode estar zerado.");
            }

            if (capacidaDeAlunoTurmaMunicipio.Tipo < 0)
            {
                mensagens.Add("Campos Tipo é obrigatório.");
            }

            if (string.IsNullOrEmpty(capacidaDeAlunoTurmaMunicipio.Matricula)
                || (!string.IsNullOrEmpty(capacidaDeAlunoTurmaMunicipio.Matricula)
                    && capacidaDeAlunoTurmaMunicipio.Matricula.Length > 12))
            {
                mensagens.Add("O campo Matricula é obrigatório com o máximo de 12 caracteres!");
            }

            int capacidadeMaxima = 0, capacidadeMinima = 0;

            //Verifica se capacidade é do tipo máximo
            if (capacidaDeAlunoTurmaMunicipio.Tipo == 1)
            {
                capacidadeMaxima = capacidaDeAlunoTurmaMunicipio.Capacidade;
                capacidadeMinima = RetornaCapacidadePor(capacidaDeAlunoTurmaMunicipio, 0);
            }

            //Verifica se capacidade é do tipo mínima
            if (capacidaDeAlunoTurmaMunicipio.Tipo == 0)
            {
                capacidadeMinima = capacidaDeAlunoTurmaMunicipio.Capacidade;
                capacidadeMaxima = RetornaCapacidadePor(capacidaDeAlunoTurmaMunicipio, 1);
            }

            if (capacidadeMinima > capacidadeMaxima && capacidadeMaxima > 0)
            {
                mensagens.Add("Capacidade Máxima não podem ser menor do que a Mínima.");
            }

            if (mensagens.Count == 0)
            {
                var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                var contextQuery = new ContextQuery
                {
                    //Verifica já existe o ano / periodo / curso cadastrados
                    Command =
                        @"
                            SELECT 1
                            FROM   dbo.capacidadealunoturmamunicipio CP
                            WHERE  ano = @ANO
                                   AND CP.periodo = @PERIODO
                                   AND CP.municipioid = @MUNICIPIOID
                                   AND CP.tipo = @TIPO
                                   AND CP.capacidadealunoturmamunicipioid <>
                                       @CAPACIDADEALUNOTURMAMUNICIPIOID  "
                };

                contextQuery.Parameters.Add("@ANO", capacidaDeAlunoTurmaMunicipio.Ano);
                contextQuery.Parameters.Add("@PERIODO", capacidaDeAlunoTurmaMunicipio.Periodo);
                contextQuery.Parameters.Add("@MUNICIPIOID", capacidaDeAlunoTurmaMunicipio.MunicipioId);
                contextQuery.Parameters.Add("@TIPO", capacidaDeAlunoTurmaMunicipio.Tipo);
                contextQuery.Parameters.Add("@CAPACIDADEALUNOTURMAMUNICIPIOID", capacidaDeAlunoTurmaMunicipio.CapacidadeAlunoTurmaMunicipioId);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    mensagens.Add("Município já cadastrado anteriormente para este ano / periodo.");
                }

                //Verifica ser é alteração
                if (capacidaDeAlunoTurmaMunicipio.CapacidadeAlunoTurmaMunicipioId > 0)
                {
                    RN.CapacidadeAlunoTurmaMunicipio rnCapacidadeAlunoTurmaMunicipio = new CapacidadeAlunoTurmaMunicipio();

                    //Verifica se a capacidade é para o tipo maximo
                    if (capacidaDeAlunoTurmaMunicipio.Tipo == 1)
                    { 
                        if (rnCapacidadeAlunoTurmaMunicipio.ExisteTurmaSuperiorPor(capacidaDeAlunoTurmaMunicipio.Ano, capacidaDeAlunoTurmaMunicipio.Periodo, capacidaDeAlunoTurmaMunicipio.MunicipioId, capacidaDeAlunoTurmaMunicipio.Capacidade))
                        {
                            mensagens.Add("Não será possivel alterar, pois existem turmas com capacidade superior.");
                        }
                    }
                    else
                    {
                        //Caso sejá do tipo minimo
                        if (rnCapacidadeAlunoTurmaMunicipio.ExisteTurmaInferiorPor(capacidaDeAlunoTurmaMunicipio.Ano, capacidaDeAlunoTurmaMunicipio.Periodo, capacidaDeAlunoTurmaMunicipio.MunicipioId, capacidaDeAlunoTurmaMunicipio.Capacidade))
                        {
                            mensagens.Add("Não será possivel alterar, pois existem turmas com capacidade inferior.");
                        }
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        private bool ExisteTurmaSuperiorPor(decimal ano, decimal periodo, string municipio, int capacidade)
        {	
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   LY_TURMA T (NOLOCK) 
                                               INNER JOIN LY_UNIDADE_ENSINO E (NOLOCK) 
                                                       ON T.FACULDADE = E.UNIDADE_ENS 
                                               INNER JOIN LY_DEPENDENCIA DE (NOLOCK) 
                                                       ON E.UNIDADE_ENS = DE.FACULDADE 
                                                          AND T.DEPENDENCIA = DE.DEPENDENCIA 
                                        WHERE  T.ANO = @ANO
                                               AND T.SEMESTRE = @SEMESTRE 
                                               AND E.MUNICIPIO = @MUNICIPIO 
                                               AND T.NUM_ALUNOS > @CAPACIDADE
                                                ";

                contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
                contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, periodo);
                contextQuery.Parameters.Add("@MUNICIPIO", municipio);
                contextQuery.Parameters.Add("@CAPACIDADE", TechneDbType.T_NUMERO, capacidade);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
                contexto.Dispose();
            }
        }

        private bool ExisteTurmaInferiorPor(decimal ano, decimal periodo, string municipio, int capacidade)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   LY_TURMA T (NOLOCK) 
                                               INNER JOIN LY_UNIDADE_ENSINO E (NOLOCK) 
                                                       ON T.FACULDADE = E.UNIDADE_ENS 
                                               INNER JOIN LY_DEPENDENCIA DE (NOLOCK) 
                                                       ON E.UNIDADE_ENS = DE.FACULDADE 
                                                          AND T.DEPENDENCIA = DE.DEPENDENCIA 
                                        WHERE  T.ANO = @ANO
                                               AND T.SEMESTRE = @SEMESTRE 
                                               AND E.MUNICIPIO = @MUNICIPIO 
                                               AND T.NUM_ALUNOS < @CAPACIDADE
                                                ";

                contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
                contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, periodo);
                contextQuery.Parameters.Add("@MUNICIPIO", municipio);
                contextQuery.Parameters.Add("@CAPACIDADE", TechneDbType.T_NUMERO, capacidade);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
                contexto.Dispose();
            }
        }

        private static int RetornaCapacidadePor(Techne.Lyceum.RN.Entidades.CapacidadeAlunoTurmaMunicipio capacidaDeAlunoTurmaMunicipio, int tipo)
        {
            int retorno = int.MinValue;
            Entidades.CapacidadeAlunoTurmaMunicipio objCapacidadeAlunoTurmaMunicipio = null;
            SqlDataReader reader = null;

            var ctx = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {
                var contextQuery =
                    new ContextQuery(
                        @"
                        SELECT capacidadealunoturmamunicipioid,
                               ano,
                               periodo,
                               capacidade,
                               matricula,
                               datacadastro,
                               dataalteracao,
                               tipo,
                               municipioid
                        FROM   capacidadealunoturmamunicipio
                        WHERE  ano = @ANO
                        AND periodo = @PERIODO
                        AND municipioid = @MUNICIPIOID
                        AND tipo = @TIPO ");

                contextQuery.Parameters.Add("@ANO", capacidaDeAlunoTurmaMunicipio.Ano);
                contextQuery.Parameters.Add("@PERIODO", capacidaDeAlunoTurmaMunicipio.Periodo);
                contextQuery.Parameters.Add("@MUNICIPIOID", capacidaDeAlunoTurmaMunicipio.MunicipioId);
                contextQuery.Parameters.Add("@TIPO", tipo);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    objCapacidadeAlunoTurmaMunicipio = new Entidades.CapacidadeAlunoTurmaMunicipio
                    {
                        CapacidadeAlunoTurmaMunicipioId = !String.IsNullOrEmpty(reader["CAPACIDADEALUNOTURMAMUNICIPIOID"].ToString()) ? Convert.ToInt32(reader["CAPACIDADEALUNOTURMAMUNICIPIOID"]) : default(int),
                        MunicipioId = !String.IsNullOrEmpty(reader["MUNICIPIOID"].ToString()) ? Convert.ToString(reader["MUNICIPIOID"]) : default(string),
                        Ano = !String.IsNullOrEmpty(reader["ANO"].ToString()) ? Convert.ToDecimal(reader["ANO"]) : default(decimal),
                        Periodo = !String.IsNullOrEmpty(reader["PERIODO"].ToString()) ? Convert.ToDecimal(reader["PERIODO"]) : default(decimal),
                        Tipo = !String.IsNullOrEmpty(reader["PERIODO"].ToString()) ? Convert.ToInt32(reader["PERIODO"]) : default(int),
                        Capacidade = !String.IsNullOrEmpty(reader["CAPACIDADE"].ToString()) ? Convert.ToInt32(reader["CAPACIDADE"]) : default(int),
                        Matricula = !String.IsNullOrEmpty(reader["MATRICULA"].ToString()) ? Convert.ToString(reader["MATRICULA"]) : default(string),
                        DataCadastro = !String.IsNullOrEmpty(reader["DATACADASTRO"].ToString()) ? Convert.ToDateTime(reader["DATACADASTRO"]) : default(DateTime),
                        DataAlteracao = !String.IsNullOrEmpty(reader["DATAALTERACAO"].ToString()) ? Convert.ToDateTime(reader["DATAALTERACAO"]) : default(DateTime),
                    };
                }

                if (objCapacidadeAlunoTurmaMunicipio != null)
                {
                    retorno = objCapacidadeAlunoTurmaMunicipio.Capacidade;
                }

                return retorno;
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

        public static DataTable ListarAnoPeriodoReplicacao(int ano, int periodo)
        {
            DataTable objDt = null;
            var ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            var anoperiodo = string.Format("{0}/{1}",
                Convert.ToString(ano), Convert.ToString(periodo));

            try
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"  
                                 SELECT DISTINCT CONVERT(VARCHAR(4), CT.ano) + '/'
                                                + CONVERT(VARCHAR(1), CT.periodo) AS ANOPERIODO,
                                                CT.ano,
                                                CT.periodo
                                FROM   dbo.capacidadealunoturmamunicipio CT
                                WHERE  ( CONVERT(VARCHAR(4), CT.ano) + '/'
                                         + CONVERT(VARCHAR(1), CT.periodo) ) <> @ANOPERIODO
                                ORDER  BY CT.ano,
                                          CT.periodo  "
                };

                contextQuery.Parameters.Add("@ANOPERIODO", anoperiodo);

                objDt = new DataTable();
                objDt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }

            return objDt;
        }

        public static void Replicar(Techne.Lyceum.RN.DTOs.DadosReplicacaoCapacidadeTurma capac)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" 
                                 INSERT INTO capacidadealunoturmamunicipio
                                            (ano,
                                             periodo,
                                             capacidade,
                                             matricula,
                                             datacadastro,
                                             dataalteracao,
                                             tipo,
                                             municipioid)
                                SELECT @ANO,
                                       @PERIODO,
                                       CI.capacidade,
                                       @MATRICULA,
                                       Getdate(),
                                       Getdate(),
                                       ci.tipo,
                                       ci.municipioid
                                FROM   dbo.capacidadealunoturmamunicipio CI
                                WHERE  ano = @ANOREPLICACAO
                                       AND periodo = @PERIODOREPLICACAO
                                       AND NOT EXISTS (SELECT 1
                                                       FROM   capacidadealunoturmamunicipio CF
                                                       WHERE  CF.municipioid = ci.municipioid
                                                              AND cf.tipo = ci.tipo
                                                              AND ano = @ANO
                                                              AND periodo = @PERIODO)"
                };

                contextQuery.Parameters.Add("@ANO", capac.Ano);
                contextQuery.Parameters.Add("@PERIODO", capac.Periodo);
                contextQuery.Parameters.Add("@MATRICULA", capac.Matricula);
                contextQuery.Parameters.Add("@ANOREPLICACAO", capac.AnoReplicacao);
                contextQuery.Parameters.Add("@PERIODOREPLICACAO", capac.PeriodoReplicacao);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                    Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public int RetornaCapacidadePor(int ano, int periodo, string municipio)
        {
            int capacidade = -1;
            SqlDataReader reader = null;

            var ctx = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {
                var contextQuery =
                    new ContextQuery(
                        @"
                        SELECT                             
                               CAPACIDADE
                              
                        FROM   CAPACIDADEALUNOTURMAMUNICIPIO
                        WHERE  ano = @ANO
                        AND periodo = @PERIODO
                        AND municipioid = @MUNICIPIOID
                        AND tipo = 1 ");

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@MUNICIPIOID", municipio);

                reader = ctx.GetDataReader(contextQuery);

                if (reader.Read())
                {
                    capacidade = Convert.ToInt32(reader["CAPACIDADE"]);
                }

                return capacidade;
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }
    }
}
