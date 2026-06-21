using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Pedagogico
{
    public class PeriodoConfirmacaoCurso
    {
        public bool PossuiPeriodoConfirmacaoPor(DataContext ctx, int periodoConfirmacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM Pedagogico.PERIODOCONFIRMACAOCURSO
                                WHERE PERIODOCONFIRMACAOID = @PERIODOCONFIRMACAOID ";

            contextQuery.Parameters.Add("@PERIODOCONFIRMACAOID", SqlDbType.Int, periodoConfirmacaoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ListaPor(int periodoConfirmacaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  PERIODOCONFIRMACAOCURSOID, 
											PERIODOCONFIRMACAOID, 
											C.CURSO, 
											C.NOME AS NOMECURSO,
											PC.SERIE
                                        FROM Pedagogico.PERIODOCONFIRMACAOCURSO PC (NOLOCK)
											INNER JOIN LY_CURSO C (NOLOCK) ON PC.CURSO = C.CURSO
										WHERE PERIODOCONFIRMACAOID = @PERIODOCONFIRMACAOID
                                        ORDER BY NOMECURSO, SERIE";

                contextQuery.Parameters.Add("@PERIODOCONFIRMACAOID", SqlDbType.Int, periodoConfirmacaoId);

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
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
                contexto.Dispose();
            }

            return dt;
        }

        public ValidacaoDados Valida(Entidades.PeriodoConfirmacaoCurso periodoConfirmacaoCurso, int ano, int periodo, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Curriculo rnCurriculo = new Curriculo();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (periodoConfirmacaoCurso == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (periodoConfirmacaoCurso.PeriodoConfirmacaoCursoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (periodoConfirmacaoCurso.PeriodoConfirmacaoId <= 0)
            {
                mensagens.Add("Campo ANO/PERIODO é obrigatório.");
            }

            if (ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (periodo < 0)
            {
                mensagens.Add("Campo PERIODO é obrigatório.");
            }

            if (periodoConfirmacaoCurso.Curso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CURSO é obrigatório.");
            }

            if (periodoConfirmacaoCurso.Serie <= 0)
            {
                mensagens.Add("Campo SÉRIE é obrigatório.");
            }

            if (periodoConfirmacaoCurso.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já existe o curso / serie cadastrado para o ano / periodo 
                    if (this.PossuiOutroCursoSerieCadastradoPor(contexto, periodoConfirmacaoCurso.Curso, periodoConfirmacaoCurso.Serie, periodoConfirmacaoCurso.PeriodoConfirmacaoId, periodoConfirmacaoCurso.PeriodoConfirmacaoCursoId))
                    {
                        mensagens.Add("Este CURSO/SÉRIE já foi cadastrado para este ANO/PERIODO.");
                    }                  
                }
                catch (Exception ex)
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
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

        private bool PossuiOutroCursoSerieCadastradoPor(DataContext ctx, string curso, int serie, int periodoConfirmacaoId, int periodoConfirmacaoCursoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM Pedagogico.PERIODOCONFIRMACAOCURSO (NOLOCK)
                                WHERE CURSO = @CURSO
                                    AND SERIE = @SERIE
									AND PERIODOCONFIRMACAOID = @PERIODOCONFIRMACAOID 
	                                AND PERIODOCONFIRMACAOCURSOID <> @PERIODOCONFIRMACAOCURSOID ";

            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
            contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);
            contextQuery.Parameters.Add("@PERIODOCONFIRMACAOID", SqlDbType.Int, periodoConfirmacaoId);
            contextQuery.Parameters.Add("@PERIODOCONFIRMACAOCURSOID", SqlDbType.Int, periodoConfirmacaoCursoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.PeriodoConfirmacaoCurso periodoConfirmacaoCurso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO Pedagogico.PERIODOCONFIRMACAOCURSO
                                               (PERIODOCONFIRMACAOID,
                                               CURSO,
                                               SERIE,
                                               USUARIOID,
                                               DATACADASTRO,
                                               DATAALTERACAO)
                                         VALUES
                                               (@PERIODOCONFIRMACAOID,
                                               @CURSO,
                                               @SERIE,
                                               @USUARIOID,
                                               @DATACADASTRO,
                                               @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@PERIODOCONFIRMACAOID", SqlDbType.Int, periodoConfirmacaoCurso.PeriodoConfirmacaoId);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, periodoConfirmacaoCurso.Curso);
                contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, periodoConfirmacaoCurso.Serie);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, periodoConfirmacaoCurso.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                ctx.ApplyModifications(contextQuery);
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

        public ValidacaoDados ValidaRemocao(int periodoConfirmacaoCursoId)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (periodoConfirmacaoCursoId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
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

        public void Remove(int periodoConfirmacaoCursoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Pedagogico.PERIODOCONFIRMACAOCURSO
                                         WHERE  PERIODOCONFIRMACAOCURSOID = @PERIODOCONFIRMACAOCURSOID  ";

                contextQuery.Parameters.Add("@PERIODOCONFIRMACAOCURSOID", SqlDbType.Int, periodoConfirmacaoCursoId);

                ctx.ApplyModifications(contextQuery);
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
