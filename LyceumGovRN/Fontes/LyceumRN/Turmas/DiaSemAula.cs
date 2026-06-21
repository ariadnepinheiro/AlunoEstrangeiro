using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.Turmas
{
    public class DiaSemAula
    {
        public bool PossuiMotivoDiaSemAulaPor(DataContext contexto, int motivoDiaSemAulaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Turma.DIASEMAULA (NOLOCK)
                                    WHERE MOTIVODIASEMAULAID = @MOTIVODIASEMAULAID ";

            contextQuery.Parameters.Add("@MOTIVODIASEMAULAID", SqlDbType.Int, motivoDiaSemAulaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ListaPor(string censo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"SELECT DIASEMAULAID,
	                   DATA,
	                   'TODOS' AS TURNO,
	                   'TODOS' AS TURMA,
	                   'TODOS' AS COMPONENTECURRICULAR,
	                   'TODOS' AS TEMPOAULA,
	                   SA.PROCESSOSEI,
	                   SA.MOTIVODIASEMAULAID,
	                   M.DESCRICAO AS MOTIVO,
	                   JUSTIFICATIVA,
	                   DATAREPOSICAO
                FROM Turma.DIASEMAULA SA
	                INNER JOIN Turma.MOTIVODIASEMAULA M ON SA.MOTIVODIASEMAULAID = M.MOTIVODIASEMAULAID
                where CENSO = @CENSO ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.Int, censo);

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

        public ValidacaoDados Valida(Entidades.DiaSemAula diaSemAula, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            Matriculas.DiasNaoLetivos rnDiasNaoLetivos = new Techne.Lyceum.RN.Matriculas.DiasNaoLetivos();
            RN.UnidadeEnsino rnUnidadeEnsino = new UnidadeEnsino();  
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (diaSemAula == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (diaSemAula.DiaSemAulaId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (diaSemAula.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE DE ENSINO é obrigatório.");
            }

            if (diaSemAula.Data == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA é obrigatório.");
            }
            else
            {
                if (diaSemAula.Data > DateTime.Now.Date)
                {
                    mensagens.Add("Campo DATA SEM AULA não pode ser maior que a data atual.");
                }

                //Verificar se não é domingo (0)
                if ((int)diaSemAula.Data.DayOfWeek == 0)
                {
                    mensagens.Add("A DATA SEM AULA não pode ser um domingo.");
                }
            }

            if (diaSemAula.ProcessoSei.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo PROCESSO SEI é obrigatório.");
            }
            else if (diaSemAula.ProcessoSei.Length > 500)
            {
                mensagens.Add("Campo PROCESSO SEI deve ter no máximo 500 caracteres.");
            }

            if (diaSemAula.MotivoDiaSemAulaId <= 0)
            {
                mensagens.Add("Campo MOTIVO é obrigatório.");
            }

            if (diaSemAula.Justificativa.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo JUSTIFICATIVA é obrigatório.");
            }
            else if (diaSemAula.Justificativa.Length > 500)
            {
                mensagens.Add("Campo JUSTIFICATIVA deve ter no máximo 500 caracteres.");
            }

            if (diaSemAula.DataReposicao == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA REPOSIÇÃO é obrigatório.");
            }
            else
            {
                if (diaSemAula.DataReposicao != null && Convert.ToDateTime(diaSemAula.DataReposicao).Date <= diaSemAula.Data)
                {
                    mensagens.Add("Campo DATA REPOSIÇÃO deve ser maior que a DATA SEM AULA.");
                }

                //RN11.	Data Reposição Edição
                //O sistema deve permitir a edição do “DIA SEM AULA” até às 0h do dia da “Data Reposição”.
                //Após essa data não poderá mais ser editado.
                if (DateTime.Now.Date >= diaSemAula.DataReposicao)
                {
                    mensagens.Add("A Edição/Cadastro do DIA SEM AULA deve ser até às 0h do dia da DATA REPOSIÇÃO");
                }

                //Verificar se é sabado (6)
                if ((int)diaSemAula.DataReposicao.DayOfWeek != 6)
                {
                    mensagens.Add("A reposição da aula deverá ocorrer somente aos sábados.");
                }
            }

            if (diaSemAula.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSAVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já existe 
                    if (this.PossuiOutroCadastradoPor(contexto, diaSemAula.Data, diaSemAula.Censo, diaSemAula.DiaSemAulaId))
                    {
                        mensagens.Add("Esta DATA SEM AULA já foi cadastrada para esta UNIDADE DE ENSINO.");
                    }

                    //Busca municipio da escola
                    string municipio = rnUnidadeEnsino.RetornaMunicipioPor(contexto, diaSemAula.Censo);

                    //Verifica se data sem aula é um Dias nao letivos
                    if(rnDiasNaoLetivos.PossuiDiaNaoLetivoPor(contexto, diaSemAula.Data, municipio))
                    {
                        mensagens.Add("A DATA SEM AULA não pode ser uma data cadastrada como dia não letivo.");
                    }

                    //Verifica se data reposicao é um Dias nao letivos
                    if (rnDiasNaoLetivos.PossuiDiaNaoLetivoPor(contexto, diaSemAula.DataReposicao, municipio))
                    {
                        mensagens.Add("A DATA REPOSIÇÃO não pode ser uma data cadastrada como dia não letivo.");
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
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        private bool PossuiOutroCadastradoPor(DataContext contexto, DateTime data, string censo, int diaSemAulaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1)
                                    FROM Turma.DIASEMAULA
                                    WHERE [DATA] = @DATA
		                                    AND CENSO = @CENSO
		                                    AND DIASEMAULAID <> @DIASEMAULAID ";

            contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, data.Date);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@DIASEMAULAID", SqlDbType.Int, diaSemAulaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.DiaSemAula diaSemAula)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            FrequenciaDiariaAlunoFalta rnFrequenciaDiariaAlunoFalta = new FrequenciaDiariaAlunoFalta();
            FrequenciaDiaria rnFrequenciaDiaria = new FrequenciaDiaria();
            CompetenciaHabilidadeDocente rnCompetenciaHabilidadeDocente = new CompetenciaHabilidadeDocente();
            FrequenciaPlanoDeAula rnFrequenciaPlanoDeAula = new FrequenciaPlanoDeAula();

            try
            {
                //Insere
                this.Insere(contexto, diaSemAula);

                //RN10.	Data Reposição
                //O sistema deve, ao salvar a data da reposição do “DIA SEM AULA”, limpar toda e qualquer informação da base de dados que por ventura tem sido cadastrada indevidamente no dia em que não houve aula.
                //O “DIA SEM AULA” agendado para reposição pelo Diretor/Adjunto deve receber as informações APENAS na data estipulada para esse fim.
                //Limpar lançamentos de frequencia no censo / data (CURRICULO / PLANO / FREQUENCIA / ALUNO)

                //deleta plano de aula
                rnFrequenciaPlanoDeAula.RemovePor(contexto, diaSemAula.Censo, diaSemAula.Data);

                //Insere curriculo minimo  no log
                rnCompetenciaHabilidadeDocente.GeraLog(contexto, diaSemAula.Censo, diaSemAula.Data);

                //Deleta itens do curriculo minimo
                rnCompetenciaHabilidadeDocente.Remove(contexto, diaSemAula.Censo, diaSemAula.Data);

                //Deleta alunos com falta
                rnFrequenciaDiariaAlunoFalta.Remove(contexto, diaSemAula.Censo, diaSemAula.Data);

                //Deleta frequencia
                rnFrequenciaDiaria.Remove(contexto, diaSemAula.Censo, diaSemAula.Data);
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

        public void Insere(DataContext ctx, Entidades.DiaSemAula diaSemAula)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Turma.DIASEMAULA
                                               (CENSO
                                               ,[DATA]
                                               ,PROCESSOSEI
                                               ,MOTIVODIASEMAULAID
                                               ,JUSTIFICATIVA
                                               ,DATAREPOSICAO
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@CENSO,
                                               @DATA,
                                               @PROCESSOSEI, 
                                               @MOTIVODIASEMAULAID,
                                               @JUSTIFICATIVA, 
                                               @DATAREPOSICAO, 
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO) ";


            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, diaSemAula.Censo);
            contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, diaSemAula.Data);
            contextQuery.Parameters.Add("@PROCESSOSEI", SqlDbType.VarChar, diaSemAula.ProcessoSei);
            contextQuery.Parameters.Add("@MOTIVODIASEMAULAID", SqlDbType.Int, diaSemAula.MotivoDiaSemAulaId);
            contextQuery.Parameters.Add("@JUSTIFICATIVA", SqlDbType.VarChar, diaSemAula.Justificativa);
            contextQuery.Parameters.Add("@DATAREPOSICAO", SqlDbType.DateTime, diaSemAula.DataReposicao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, diaSemAula.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            ctx.ApplyModifications(contextQuery);
        }

        public void Atualiza(Entidades.DiaSemAula diaSemAula)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            FrequenciaDiaria rnFrequenciaDiaria = new FrequenciaDiaria();

            try
            {
                this.Atualiza(ctx, diaSemAula);

                rnFrequenciaDiaria.AtualizaDataReposicao(ctx, diaSemAula.Censo, diaSemAula.Data, diaSemAula.DataReposicao);
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

        public void Atualiza(DataContext contexto, Entidades.DiaSemAula diaSemAula)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Turma.DIASEMAULA
                                          SET PROCESSOSEI = @PROCESSOSEI,
                                              MOTIVODIASEMAULAID = @MOTIVODIASEMAULAID,
                                              JUSTIFICATIVA = @JUSTIFICATIVA,
                                              DATAREPOSICAO = @DATAREPOSICAO,
                                              USUARIOID = @USUARIOID,
                                              DATAALTERACAO = @DATAALTERACAO
                                         WHERE DIASEMAULAID = @DIASEMAULAID ";

            contextQuery.Parameters.Add("@DIASEMAULAID", SqlDbType.Int, diaSemAula.DiaSemAulaId);
            contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, diaSemAula.Data);
            contextQuery.Parameters.Add("@PROCESSOSEI", SqlDbType.VarChar, diaSemAula.ProcessoSei);
            contextQuery.Parameters.Add("@MOTIVODIASEMAULAID", SqlDbType.Int, diaSemAula.MotivoDiaSemAulaId);
            contextQuery.Parameters.Add("@JUSTIFICATIVA", SqlDbType.VarChar, diaSemAula.Justificativa);
            contextQuery.Parameters.Add("@DATAREPOSICAO", SqlDbType.DateTime, diaSemAula.DataReposicao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, diaSemAula.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public ValidacaoDados ValidaRemocao(int diaSemAulaId, string censo, DateTime data)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (diaSemAulaId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE DE ENSINO é obrigatório.");
            }

            if (data == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA é obrigatório.");
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

        public void Remove(int diaSemAulaId, string censo, DateTime data)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            FrequenciaDiaria rnFrequenciaDiaria = new FrequenciaDiaria();

            try
            {
                //Insere
                this.Remove(contexto, diaSemAulaId);
            
                //Limpar a data de reposição
                rnFrequenciaDiaria.AtualizaDataReposicao(contexto, censo, data, null);
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

        public void Remove(DataContext ctx, int diaSemAulaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE Turma.DIASEMAULA
                            WHERE  DIASEMAULAID = @DIASEMAULAID  ";

            contextQuery.Parameters.Add("@DIASEMAULAID", SqlDbType.Int, diaSemAulaId);

            ctx.ApplyModifications(contextQuery);
        }

        public DateTime ObtemDataReposicaoPor(DataContext contexto, DateTime data, string censo)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            DateTime retorno = DateTime.MinValue;

            try
            {
                contextQuery.Command = @" SELECT DATAREPOSICAO
                                    FROM Turma.DIASEMAULA
                                    WHERE [DATA] = @DATA
		                                    AND CENSO = @CENSO ";

                contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, data.Date);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToDateTime(reader["DATAREPOSICAO"]);
                }

                return retorno;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public string VerificaDiaSemAulaPor(DateTime data, string censo, out bool aberto)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            string retorno = string.Empty;
            aberto = true;

            try
            {
                //Busca data de reposição - caso seja dia é um dia sem aula
                DateTime dataReposicao = this.ObtemDataReposicaoPor(contexto, data, censo);

                if (dataReposicao != DateTime.MinValue)
                {
                    if (DateTime.Now < dataReposicao)
                    {
                        aberto = false;
                        retorno = string.Format("O dia Sem Aula ficará indisponível para lançamento. Favor retornar no dia cadastrado para a reposição, {0}.", dataReposicao.ToString("dd/MM/yyyy"));
                    }
                    else
                    {
                        retorno = string.Format("Lançamento de Frequência no dia {0} referente à reposição do dia {1} (Dia Sem Aula).", dataReposicao.ToString("dd/MM/yyyy"), data.ToString("dd/MM/yyyy"));
                    }
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
    }
}
