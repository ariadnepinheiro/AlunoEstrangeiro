using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using System.Collections;
using Seeduc.Infra.Mapper;

namespace Techne.Lyceum.RN
{
    public class DisciplinaMultipla
    {
        public DataTable ListaDisciplinaMultiplaPor(string disciplina)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable disciplinasMultiplas = null;

            try
            {
                contextQuery.Command = @" SELECT  m.disciplina ,
                                        m.disciplina_multipla ,
                                        d.nome_compl AS NOME_DISCIPLINA_MULTIPLA,
                                        d.HORAS_AULA
                                FROM    LY_DISCIPLINA_MULTIPLA m
                                        INNER JOIN dbo.LY_DISCIPLINA d ON m.DISCIPLINA_MULTIPLA = d.DISCIPLINA
                                WHERE   m.DISCIPLINA = @DISCIPLINA ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

                disciplinasMultiplas = ctx.GetDataTable(contextQuery);
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

            return disciplinasMultiplas;
        }

        public bool EhDisciplinaMultiplaCadastradaPor(string disciplina, string disciplinaMultipla)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                FROM    LY_DISCIPLINA_MULTIPLA
                            WHERE   DISCIPLINA = @DISCIPLINA
                                    AND DISCIPLINA_MULTIPLA = @DISCIPLINA_MULTIPLA ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@DISCIPLINA_MULTIPLA", disciplinaMultipla);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    existe = true;
                }

                return existe;
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

        public bool PossuiMultiplaPor(string disciplina)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                    FROM  LY_DISCIPLINA_MULTIPLA
                            WHERE   DISCIPLINA = @DISCIPLINA ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    existe = true;
                }

                return existe;
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

        public bool ExisteDisciplinaMultiplaPor(string disciplina)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                    FROM  LY_DISCIPLINA_MULTIPLA
                            WHERE   DISCIPLINA = @DISCIPLINA
                                    OR DISCIPLINA_MULTIPLA = @DISCIPLINA ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    existe = true;
                }

                return existe;
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

        public ValidacaoDados Valida(LyDisciplinaMultipla disciplinaMultipla)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (disciplinaMultipla == null)
            {
                return validacaoDados;
            }

            //Verifica campos obrigatorios
            if (string.IsNullOrEmpty(disciplinaMultipla.Disciplina))
            {
                mensagens.Add("O CÓDIGO da disciplina é de preenchimento obrigatório.");
            }

            if (string.IsNullOrEmpty(disciplinaMultipla.DisciplinaMultipla))
            {
                mensagens.Add("O CÓDIGO da disciplina multipla é de preenchimento obrigatório.");
            }            

            if (mensagens.Count == 0)
            {
                if (disciplinaMultipla.Disciplina == disciplinaMultipla.DisciplinaMultipla)
                {
                    mensagens.Add("A disciplina não pode ser multipla dela mesma.");
                }

                //Verificar se exsite o conjunto não cadastrada
                if (EhDisciplinaMultiplaCadastradaPor(disciplinaMultipla.Disciplina, disciplinaMultipla.DisciplinaMultipla))
                {
                    mensagens.Add("Esta disciplina Multipla já foi cadastrada para esta disciplina.");
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

        public ValidacaoDados ValidaRemocao(LyDisciplinaMultipla disciplinaMultipla)
        {
            List<string> mensagens = new List<string>();
            RN.Turma rnTurma = new Turma();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (disciplinaMultipla == null)
            {
                return validacaoDados;
            }

            //Verifica campos obrigatorios
            if (string.IsNullOrEmpty(disciplinaMultipla.Disciplina))
            {
                mensagens.Add("O CÓDIGO da disciplina é de preenchimento obrigatório.");
            }

            if (string.IsNullOrEmpty(disciplinaMultipla.DisciplinaMultipla))
            {
                mensagens.Add("O CÓDIGO da disciplina multipla é de preenchimento obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                //Verificar se existe turma utilizando o conjunto
                if (rnTurma.ExisteTurmaPor(disciplinaMultipla.Disciplina, disciplinaMultipla.DisciplinaMultipla))
                {
                    mensagens.Add("Não é possível remover a disciplina multipla pois existe Turma cadastrada com esta Disciplina multipla.");
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

        public void Insere(LyDisciplinaMultipla disciplinaMultipla)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT  INTO DBO.LY_DISCIPLINA_MULTIPLA
                                        ( DISCIPLINA ,
                                          DISCIPLINA_MULTIPLA 
                                        )
                                VALUES  ( @DISCIPLINA ,
                                          @DISCIPLINA_MULTIPLA
                                        ) ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplinaMultipla.Disciplina);
                contextQuery.Parameters.Add("@DISCIPLINA_MULTIPLA", disciplinaMultipla.DisciplinaMultipla);

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

        public void Remove(LyDisciplinaMultipla disciplinaMultipla)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE  dbo.LY_DISCIPLINA_MULTIPLA
                                        WHERE   DISCIPLINA = @DISCIPLINA
                                                AND DISCIPLINA_MULTIPLA = @DISCIPLINA_MULTIPLA ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplinaMultipla.Disciplina);
                contextQuery.Parameters.Add("@DISCIPLINA_MULTIPLA", disciplinaMultipla.DisciplinaMultipla);

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

        public LyDisciplinaMultipla Bind(IDictionary chaves, IDictionary valores)
        {
            return new LyDisciplinaMultipla
           {
               Disciplina = chaves == null ? Convert.ToString(valores["disciplina"]) : chaves.Contains("CompositeKey") ? Convert.ToString(chaves["CompositeKey"]).Split(';')[0] : string.Empty,
               DisciplinaMultipla = chaves == null ? Convert.ToString(valores["disciplina_multipla"]) : chaves.Contains("CompositeKey") ? Convert.ToString(chaves["CompositeKey"]).Split(';')[1] : string.Empty,

           };
        }
    }
}
