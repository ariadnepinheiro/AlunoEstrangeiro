using Seeduc.Infra.Data;
using System.Data;
using System;
using System.Collections.Generic;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Entidades;
using System.Linq;

namespace Techne.Lyceum.RN
{
    public class CursoDuracao : RNBase
    {
        public bool PossuiCursoPor(DataContext ctx, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                FROM TCE_CURSO_DURACAO
                                WHERE CURSO = @CURSO ";

            contextQuery.Parameters.Add("@CURSO", curso);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ListaPor(int ano, string curso, string turno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable duracoes = null;

            try
            {
                contextQuery.Command = @" SELECT DURACAO
                                         FROM   TCE_CURSO_DURACAO
                                         WHERE  ANO = @ANO
                                                AND CURSO = @CURSO
                                                AND TURNO = @TURNO ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@TURNO", turno);

                duracoes = ctx.GetDataTable(contextQuery);
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

            return duracoes;
        }

        public DataTable ListaPor(string curso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable duracoes = null;

            try
            {
                contextQuery.Command = @" SELECT  ID_CURSO_DURACAO ,
                                                    DURACAO ,
                                                    TURNO ,
                                                    ANO
                                            FROM    TCE_CURSO_DURACAO
                                            WHERE   CURSO = @CURSO
                                            ORDER BY ANO DESC ,
                                                    TURNO ";

                contextQuery.Parameters.Add("@CURSO", curso);

                duracoes = ctx.GetDataTable(contextQuery);
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

            return duracoes;
        }

        public bool EhDuracaoCadastradaPor(int idCursoDuracao, int ano, string curso, string turno, int duracao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool cadastrada = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                            FROM    TCE_CURSO_DURACAO
                            WHERE   ANO = @ANO
                                    AND CURSO = @CURSO
                                    AND TURNO = @TURNO
                                    AND DURACAO = @DURACAO
                                    AND ID_CURSO_DURACAO <> @ID_CURSO_DURACAO ";

                contextQuery.Parameters.Add("@ID_CURSO_DURACAO", idCursoDuracao);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@DURACAO", duracao);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    cadastrada = true;
                }

                return cadastrada;
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

        public bool EhDuracaoCadastradaPor(int ano, string curso, string turno, int duracao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool cadastrada = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                            FROM    TCE_CURSO_DURACAO
                            WHERE   ANO = @ANO
                                    AND CURSO = @CURSO
                                    AND TURNO = @TURNO
                                    AND DURACAO = @DURACAO ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@DURACAO", duracao);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    cadastrada = true;
                }

                return cadastrada;
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

        public bool EhDuracaoUtilizadaPor(int idCursoDuracao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool utilizada = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                    FROM    LY_HOR_OPER HO
                                            INNER JOIN DBO.TCE_CURSO_DURACAO CD ON HO.CURSO = CD.CURSO
                                                                                   AND HO.TURNO = CD.TURNO
                                    WHERE   ( HO.DURACAO_AULA = CD.DURACAO
                                              OR HO.DURACAO_AULA IS NULL
                                              AND DATEDIFF(MINUTE, HO.HORAINI_AULA, HORAFIM_AULA) = CD.DURACAO
                                            )
                                            AND YEAR(HO.STAMP_ATUALIZACAO) = CD.ANO
                                            AND CD.ID_CURSO_DURACAO = @ID_CURSO_DURACAO ";

                contextQuery.Parameters.Add("@ID_CURSO_DURACAO", idCursoDuracao);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    utilizada = true;
                }

                return utilizada;
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

        public ValidacaoDados Valida(TceCursoDuracao duracao)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            //Verifica campos obrigatórios gerais
            if (duracao.Ano <= 0)
            {
                mensagens.Add("O ANO é obrigatório.");
            }
            else
            {
                if (duracao.Ano.ToString().Length != 4)
                {
                    mensagens.Add("O ANO deve ser composto de 4 dígitos.");
                }
            }

            if (string.IsNullOrEmpty(duracao.Turno))
            {
                mensagens.Add("O TURNO é obrigatório.");
            }

            if (string.IsNullOrEmpty(duracao.Curso))
            {
                mensagens.Add("O CURSO é obrigatório.");
            }

            if (string.IsNullOrEmpty(duracao.Matricula))
            {
                mensagens.Add("A MATRICULA é obrigatória.");
            }

            if (duracao.Duracao <= 0)
            {
                mensagens.Add("A DURAÇÃO é obrigatória.");
            }
            else
            {
                if (duracao.Duracao.ToString().Length > 2)
                {
                    mensagens.Add("A DURAÇÃO deve ser composta de no máximo 2 dígitos.");
                }
            }            

            if (mensagens.Count == 0)
            {
                //Verifica se ja existe registro no banco com mesmo ano / curso / turno / duracao
                if (EhDuracaoCadastradaPor(duracao.IdCursoDuracao, duracao.Ano, duracao.Curso, duracao.Turno, duracao.Duracao))
                {
                    mensagens.Add("Já existe esta duração cadastra para o ano / curso / turno.");
                }

                //Verifica se é alteração
                if (duracao.IdCursoDuracao > 0)
                {
                    //verifica se esta sendo utilizado por algum horario operacional  
                    if (EhDuracaoUtilizadaPor(duracao.IdCursoDuracao))
                    {
                        mensagens.Add("A duração não pode ser alterada, pois já foi utilizada por um horário operacional.");
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

        public ValidacaoDados ValidaRemocao(int idCursoDuracao)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (idCursoDuracao <= 0)
            {
                mensagens.Add("O campo ID_CURSO_DURACAO não foi encontrado.");
            }

            if (mensagens.Count == 0)
            {
                //verifica se esta sendo utilizado por algum horario operacional  
                if (EhDuracaoUtilizadaPor(idCursoDuracao))
                {
                    mensagens.Add("A duração não pode ser excluída, pois já foi utilizada por um horário operacional.");
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

        public void Insere(TceCursoDuracao duracao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO dbo.TCE_CURSO_DURACAO
                                            ( ANO ,
                                              TURNO ,
                                              CURSO ,
                                              DURACAO ,
                                              MATRICULA ,
                                              DT_CADASTRO
                                            )
                                    VALUES  ( @ANO , 
                                              @TURNO , 
                                              @CURSO , 
                                              @DURACAO , 
                                              @MATRICULA , 
                                              @DT_CADASTRO  
                                            ) ";

                contextQuery.Parameters.Add("@ANO", duracao.Ano);
                contextQuery.Parameters.Add("@TURNO", duracao.Turno);
                contextQuery.Parameters.Add("@CURSO", duracao.Curso);
                contextQuery.Parameters.Add("@DURACAO", duracao.Duracao);
                contextQuery.Parameters.Add("@MATRICULA", duracao.Matricula);
                contextQuery.Parameters.Add("@DT_CADASTRO", DateTime.Now);

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

        public void Altera(TceCursoDuracao duracao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE  TCE_CURSO_DURACAO
                            SET     DURACAO = @DURACAO ,
                                    MATRICULA = @MATRICULA ,
                                    DT_CADASTRO = @DT_CADASTRO
                            WHERE   ID_CURSO_DURACAO = @ID_CURSO_DURACAO ";

                contextQuery.Parameters.Add("@ID_CURSO_DURACAO", duracao.IdCursoDuracao);
                contextQuery.Parameters.Add("@DURACAO", duracao.Duracao);
                contextQuery.Parameters.Add("@MATRICULA", duracao.Matricula);
                contextQuery.Parameters.Add("@DT_CADASTRO", DateTime.Now);

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

        public void Remove(int idCursoDuracao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE  dbo.TCE_CURSO_DURACAO
                            WHERE   ID_CURSO_DURACAO = @ID_CURSO_DURACAO ";

                contextQuery.Parameters.Add("@ID_CURSO_DURACAO", idCursoDuracao);

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
