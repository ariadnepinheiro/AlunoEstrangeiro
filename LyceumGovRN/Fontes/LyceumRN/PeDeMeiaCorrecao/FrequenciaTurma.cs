using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using System.Data;

namespace Techne.Lyceum.RN.PeDeMeiaCorrecao
{
    public class FrequenciaTurma
    {
        public string RetornaDadosLancamento(int ano, int periodo, string turma, int mes)
        {
            string retorno = string.Empty;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            string dados = string.Empty;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT CONVERT(VARCHAR,T.DATAALTERACAO,103) + ' por ' + T.USUARIOID + ' - ' + U.NOME as DADOS
                                FROM PeDeMeiaCorrecao.FREQUENCIATURMA T
		                                LEFT JOIN HADES..HD_USUARIO U ON T.USUARIOID = U.USUARIO
                                WHERE TURMA = @TURMA
	                                    AND ANO = @ANO
	                                    AND PERIODO = @PERIODO
	                                    AND MESREFERENCIA = @MES "
                };

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
                contextQuery.Parameters.Add("@MES", SqlDbType.Int, mes);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dados = Convert.ToString(reader["DADOS"]);
                }

                return dados;
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

        public ValidacaoDados Valida(Entidades.FrequenciaTurma frequenciaTurma, List<Entidades.FrequenciaAluno> frequenciaAluno, List<string> alunos, bool historico)
        {
            List<string> mensagens = new List<string>();
            RN.Matricula rnMatricula = new Matricula();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (frequenciaAluno == null)
            {
                return validacaoDados;
            }

            if (frequenciaTurma.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE DE ENSINO é obrigatório.");
            }

            if (frequenciaTurma.Ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (frequenciaTurma.Periodo < 0)
            {
                mensagens.Add("Campo PERIODO é obrigatório.");
            }

            if (frequenciaTurma.MesReferencia < 0)
            {
                mensagens.Add("Campo MES REFERÊNCIA é obrigatório.");
            }

            if (frequenciaTurma.Turma.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TURMA é obrigatório.");
            }

            if (frequenciaTurma.UsuarioID.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO é obrigatório.");
            }

            if (alunos.Count == 0)
            {
                mensagens.Add("Não existem alunos para lançamento.");
            }
            else
            {
                List<string> freq = frequenciaAluno.Select(x => x.Aluno).Distinct().ToList();

                if (freq.Count > alunos.Count)
                {
                    mensagens.Add("Existem mais faltas que alunos .");
                }
            }

            if (frequenciaAluno.Count > 0)
            {
                foreach (Entidades.FrequenciaAluno aluno in frequenciaAluno)
                {
                    if (aluno.Aluno.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo ALUNO é obrigatório.");
                    }

                    if (aluno.DataAusencia == DateTime.MinValue)
                    {
                        mensagens.Add("Campo DATA AUSÊNCIA é obrigatório.");
                    }

                    if (aluno.UsuarioID.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo USUÁRIO é obrigatório.");
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

        public void Salva(Entidades.FrequenciaTurma frequenciaTurma, List<Entidades.FrequenciaAluno> frequenciaAluno, List<string> alunos)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            FrequenciaAluno rnFrequenciaAluno = new FrequenciaAluno();
            try
            {
                //Verifica se já possui frequenciaTurma
                int id = this.ObtemIdPor(contexto, frequenciaTurma);
                if (id > 0)
                {
                    frequenciaTurma.FrequenciaTurmaId = id;
                    this.Atualiza(contexto, frequenciaTurma);
                }
                else
                {
                    this.Insere(contexto, frequenciaTurma);
                }

                //Remove todos os alunos da turma
                rnFrequenciaAluno.RemovePorTurma(contexto, frequenciaTurma.FrequenciaTurmaId, alunos);

                foreach (var aluno in frequenciaAluno.Select(x => x.Aluno).Distinct())
                {
                    //Remove o aluno caso ainda exista (em outra turma)
                    rnFrequenciaAluno.RemovePorAluno(contexto, frequenciaTurma.Ano, frequenciaTurma.Periodo, frequenciaTurma.MesReferencia, aluno);
                }

                //Insere cada ausencia lançada
                foreach (Entidades.FrequenciaAluno aluno in frequenciaAluno)
                {
                    aluno.FrequenciaTurmaId = frequenciaTurma.FrequenciaTurmaId;
                    rnFrequenciaAluno.Insere(contexto, aluno);
                }
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

        public int ObtemIdPor(DataContext contexto, Entidades.FrequenciaTurma frequenciaTurma)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT FREQUENCIATURMAID
                            FROM PeDeMeiaCorrecao.FREQUENCIATURMA
                            WHERE TURMA = @TURMA
	                            AND ANO = @ANO
	                            AND PERIODO = @PERIODO
	                            AND MESREFERENCIA = @MES ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, frequenciaTurma.Ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, frequenciaTurma.Periodo);
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, frequenciaTurma.Turma);
                contextQuery.Parameters.Add("@MES", SqlDbType.Int, frequenciaTurma.MesReferencia);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["FREQUENCIATURMAID"]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return retorno;
        }

        private void Insere(DataContext contexto, Entidades.FrequenciaTurma frequenciaTurma)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO PeDeMeiaCorrecao.FREQUENCIATURMA
                                           (CENSO
                                           ,ANO
                                           ,PERIODO
                                           ,TURMA
                                           ,MESREFERENCIA                                   
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@CENSO,
                                           @ANO, 
                                           @PERIODO, 
                                           @TURMA,
                                           @MESREFERENCIA,                                                                         
                                           @USUARIOID, 
                                           @DATACADASTRO, 
                                           @DATAALTERACAO) 
                                    
                                    SELECT IDENT_CURRENT('PeDeMeiaCorrecao.FREQUENCIATURMA') ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, frequenciaTurma.Censo);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, frequenciaTurma.Ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, frequenciaTurma.Periodo);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, frequenciaTurma.Turma);
            contextQuery.Parameters.Add("@MESREFERENCIA", SqlDbType.Int, frequenciaTurma.MesReferencia);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, frequenciaTurma.UsuarioID);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            frequenciaTurma.FrequenciaTurmaId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        private void Atualiza(DataContext contexto, Entidades.FrequenciaTurma frequenciaTurma)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PeDeMeiaCorrecao.FREQUENCIATURMA
                                       SET USUARIOID = @USUARIOID, 
                                          DATAALTERACAO = @DATAALTERACAO
                                     WHERE FREQUENCIATURMAID = @FREQUENCIATURMAID";

            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, frequenciaTurma.UsuarioID);
            contextQuery.Parameters.Add("@FREQUENCIATURMAID", SqlDbType.Int, frequenciaTurma.FrequenciaTurmaId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
