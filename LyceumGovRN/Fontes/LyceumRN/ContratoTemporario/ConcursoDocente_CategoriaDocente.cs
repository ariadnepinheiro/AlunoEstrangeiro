using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Util;
using System.Data;

namespace Techne.Lyceum.RN.ContratoTemporario
{
    public class ConcursoDocente_CategoriaDocente
    {
        public DataTable ConsultaCargosConcurso(string concurso)
        {

            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"
												SELECT CDCAT.CONCURSO_DOCENTE__CATEGORIA_DOCENTEID, CATD.CATEGORIA, CATD.NOME FROM LY_CATEGORIA_DOCENTE CATD
												INNER JOIN ContratoTemporario.CONCURSO_DOCENTE__CATEGORIA_DOCENTE CDCAT 
												ON CATD.CATEGORIA = CDCAT.CATEGORIAID
												INNER JOIN LY_CONCURSO_DOCENTE CD ON CD.CONCURSO = CDCAT.CONCURSOID
												where CD.CONCURSO = @CONCURSO";

                contextQuery.Parameters.Add("@CONCURSO", concurso);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
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
                ctx.Dispose();
            }

            return dt;
        }

        public static int InserirCargosConcurso(string concurso, string categoria)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            int ret = int.MinValue;

            try
            {
                contextQuery.Command = @"INSERT INTO ContratoTemporario.CONCURSO_DOCENTE__CATEGORIA_DOCENTE
																(CONCURSOID,
																CATEGORIAID)
															VALUES
																(@CONCURSOID,
																@CATEGORIAID)";

                contextQuery.Parameters.Add("@CONCURSOID", concurso);
                contextQuery.Parameters.Add("@CATEGORIAID", categoria);

                ret = ctx.ApplyModifications(contextQuery);
            }
            catch (SqlException exSql)
            {
                if (exSql.Number == 2601) // Cannot insert duplicate key row in object error
                {
                    exSql.Data.Add("error", "O cargo já foi cadastrado para esse processo seletivo");
                    throw exSql;
                }
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
                ctx.Dispose();
            }

            return ret;
        }

        public bool PossuiCategoriaPor(DataContext contexto, string categoria)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                    FROM ContratoTemporario.CONCURSO_DOCENTE__CATEGORIA_DOCENTE
                                    WHERE CATEGORIAID = @CATEGORIAID ";

            contextQuery.Parameters.Add("@CATEGORIAID", SqlDbType.VarChar, categoria);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void ExcluirCargosConcurso(string concurso, string categoria)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"DELETE FROM ContratoTemporario.CONCURSO_DOCENTE__CATEGORIA_DOCENTE 
										WHERE CONCURSOID = @CONCURSOID and CATEGORIAID = @CATEGORIAID ";

                contextQuery.Parameters.Add("@CONCURSOID", concurso);
                contextQuery.Parameters.Add("@CATEGORIAID", categoria);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public ValidacaoDados Valida(string concurso, string strCategoria, string cargaHoraria)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (string.IsNullOrEmpty(strCategoria))
            {
                mensagens.Add("Campo CARGO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                //Verifica se ja existe registro no banco com mesma carga horaria para este concurso
                if (EhCargaHorariaCadastradaPor(concurso, cargaHoraria))
                {
                    mensagens.Add("Já existe um cargo com a mesma carga horária.");
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

        private static bool EhCargaHorariaCadastradaPor(string concurso, string strCargaHoraria)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool participa = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(1) 
                                        FROM   LY_CONCURSO_DOCENTE CD (NOLOCK) 
                                               INNER JOIN CONTRATOTEMPORARIO.CONCURSO_DOCENTE__CATEGORIA_DOCENTE CDC (NOLOCK) 
                                                       ON CD.CONCURSO = CDC.CONCURSOID 
                                               INNER JOIN LY_CATEGORIA_DOCENTE CATD (NOLOCK) 
                                                       ON CATD.CATEGORIA = CDC.CATEGORIAID 
                                               INNER JOIN RECURSOSHUMANOS.CH_AGRUPAMENTOCARGO CH (NOLOCK) 
                                                       ON CATD.FUNCAO = CH.FUNCAO 
                                                          AND CATD.AGRUPAMENTOCARGOSID = CH.AGRUPAMENTOCARGOSID 
                                        WHERE  CD.CONCURSO = @CONCURSO 
                                               AND CH.CARGAHORARIAREGENCIA = @CH_SEMANAL_EFETIVA ";

                contextQuery.Parameters.Add("@CONCURSO", concurso);
                contextQuery.Parameters.Add("@CH_SEMANAL_EFETIVA", strCargaHoraria);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    participa = true;
                }

                return participa;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
                ctx.Dispose();
            }
        }
    }
}
