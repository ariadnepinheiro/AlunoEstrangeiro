using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using System.Data;

namespace Techne.Lyceum.RN.Turmas
{
    public class AlunosAusentes
    {
        public List<Entidades.AlunosAusentes> ListaAlunosAusentes(string censo, int ano, int periodo, string turno, DateTime dataLancamento)
        {
            List<Entidades.AlunosAusentes> lista = new List<Techne.Lyceum.RN.Turmas.Entidades.AlunosAusentes>();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT A.ALUNOSAUSENTESID,
		                                        T.FACULDADE,
		                                        T.TURMA, 
		                                        T.ANO, 
		                                        T.SEMESTRE, 
		                                        CONVERT(DATE, @DATALANCAMENTO) DATALANCAMENTO, 
		                                        CASE 
			                                        WHEN A.ALUNOSAUSENTESID IS NULL THEN (SELECT COUNT(distinct M.ALUNO) FROM LY_MATRICULA M 
													                                        where T.ANO = M.ANO 
													                                        AND T.SEMESTRE = M.SEMESTRE 
													                                        AND T.TURMA = M.TURMA 
													                                        AND M.SIT_MATRICULA = 'Matriculado'
                                                                                            AND ( M.DEPENDENCIA IS NULL
                                                                                              OR M.DEPENDENCIA = 'N'
                                                                                            ))
			                                        ELSE A.QUANTIDADEMATRICULADOS
		                                        END QUANTIDADEMATRICULADOS, 
		                                        A.QUANTIDADEPRESENTES, 
                                                A.QUANTIDADEAMPARADOS,
                                                A.QUANTIDADEAFASTAMENTOSCOVID,
		                                        A.DATAALTERACAO,
		                                        A.DATACADASTRO,
		                                        A.USUARIOID
                                        FROM LY_TURMA T (NOLOCK)
	                                        LEFT JOIN Turma.ALUNOSAUSENTES A (NOLOCK) 
			                                        ON T.TURMA = A.TURMA
			                                        AND T.ANO = A.ANO
			                                        AND T.SEMESTRE = A.PERIODO
			                                        AND T.FACULDADE = A.CENSO
			                                        AND CONVERT(DATE, A.DATALANCAMENTO) = CONVERT(DATE, @DATALANCAMENTO)
                                        WHERE T.FACULDADE = @CENSO
	                                        AND T.ANO = @ANO
	                                        AND T.SEMESTRE = @PERIODO
	                                        AND T.TURNO = @TURNO
	                                        AND T.SIT_TURMA = 'Aberta' ";

                contextQuery.Parameters.Add("@DATALANCAMENTO", SqlDbType.DateTime, dataLancamento.Date);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    Entidades.AlunosAusentes alunoAusente = new Techne.Lyceum.RN.Turmas.Entidades.AlunosAusentes();
                    alunoAusente.AlunosAusentesId = reader["ALUNOSAUSENTESID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ALUNOSAUSENTESID"]);
                    alunoAusente.Censo = Convert.ToString(reader["FACULDADE"]);
                    alunoAusente.Ano = Convert.ToInt32(reader["ANO"]);
                    alunoAusente.Periodo = Convert.ToInt32(reader["SEMESTRE"]);
                    alunoAusente.Turma = Convert.ToString(reader["TURMA"]);
                    if (reader["DATALANCAMENTO"] != DBNull.Value)
                    {
                        alunoAusente.DataLancamento = Convert.ToDateTime(reader["DATALANCAMENTO"]);
                    }
                    alunoAusente.QuantidadeMatriculados = reader["QUANTIDADEMATRICULADOS"] == DBNull.Value ? 0 : Convert.ToInt32(reader["QUANTIDADEMATRICULADOS"]);

                    if (reader["ALUNOSAUSENTESID"] != DBNull.Value)
                    {
                        alunoAusente.QuantidadePresentes = Convert.ToInt32(reader["QUANTIDADEPRESENTES"]);
                        alunoAusente.QuantidadeAmparados = Convert.ToInt32(reader["QUANTIDADEAMPARADOS"]);
                        alunoAusente.QuantidadeAfastamentosCovid = Convert.ToInt32(reader["QUANTIDADEAFASTAMENTOSCOVID"]);
                        alunoAusente.UsuarioID = Convert.ToString(reader["USUARIOID"]);
                        alunoAusente.DataCadastro = Convert.ToDateTime(reader["DATACADASTRO"]);
                        alunoAusente.DataAlteracao = Convert.ToDateTime(reader["DATAALTERACAO"]);
                    }
                    else
                    {
                        alunoAusente.QuantidadePresentes = null;
                        alunoAusente.QuantidadeAmparados = null;
                        alunoAusente.QuantidadeAfastamentosCovid = null;
                    }

                    lista.Add(alunoAusente);
                }

                return lista;
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }

        public ValidacaoDados Valida(List<Entidades.AlunosAusentes> alunosAusentes)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (alunosAusentes == null)
            {
                return validacaoDados;
            }

            if (alunosAusentes.Count == 0)
            {
                mensagens.Add("É obrigatório informar ao menos 1 turma.");
            }
            else
            {
                foreach (Entidades.AlunosAusentes aluno in alunosAusentes)
                {
                    if (aluno.Censo.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo UNIDADE DE ENSINO é obrigatório.");
                    }

                    if (aluno.Ano <= 0)
                    {
                        mensagens.Add("Campo ANO é obrigatório.");
                    }

                    if (aluno.Periodo < 0)
                    {
                        mensagens.Add("Campo PERIODO é obrigatório.");
                    }

                    if (aluno.Turma.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo TURMA é obrigatório.");
                    }

                    if (aluno.DataLancamento == DateTime.MinValue)
                    {
                        mensagens.Add("Campo DATA LANÇAMENTO é obrigatório.");
                    }

                    if (aluno.QuantidadeMatriculados < 0)
                    {
                        mensagens.Add(aluno.Turma + " - Campo QUANTIDADE MATRICULADOS é obrigatório.");
                    }

                    if (aluno.QuantidadePresentes == null || aluno.QuantidadePresentes < 0)
                    {
                        mensagens.Add(aluno.Turma + " - Campo QUANTIDADE PRESENTES é obrigatório.");
                    }

                    if (aluno.QuantidadeAmparados == null || aluno.QuantidadeAmparados < 0)
                    {
                        mensagens.Add(aluno.Turma + " - Campo QUANTIDADE AMPARADOS é obrigatório.");
                    }

                    if (aluno.QuantidadeAfastamentosCovid == null || aluno.QuantidadeAfastamentosCovid < 0)
                    {
                        mensagens.Add(aluno.Turma + " - Campo QUANTIDADE AFASTAMENTOS COVID é obrigatório.");
                    }

                    if (aluno.IdRegional != 5)
                    {

                        if (aluno.Turno == "M" && DateTime.Now.Hour > 14)
                        {
                            mensagens.Add("Para o Turno da Manhã só é permitido lançamento até às 14 horas.");
                        }

                        if ((aluno.Turno == "T" || aluno.Turno == "I") && DateTime.Now.Hour > 19)
                        {
                            mensagens.Add("Para o Turno da Tarde ou Integral só é permitido lançamento até às 19 horas.");
                        }

                        if (aluno.Turno == "N" && (DateTime.Now.Date == aluno.DataLancamento.Date && DateTime.Now.Hour < 17))
                        {
                            mensagens.Add("Para o Turno da Noite só é permitido lançamento após às 17 horas.");
                        }
                    }
                    else
                    {
                        if ((aluno.Turno == "M" || aluno.Turno == "T") && DateTime.Now.Hour > 19)
                        {
                            mensagens.Add("Para o Turno da Manhã ou Tarde só é permitido lançamento até às 19 horas.");
                        }
                    }

                    if (aluno.UsuarioID.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo USUARIO é obrigatório.");
                    }

                    if (mensagens.Count == 0)
                    {
                        if ((aluno.QuantidadePresentes + aluno.QuantidadeAmparados + aluno.QuantidadeAfastamentosCovid) > aluno.QuantidadeMatriculados)
                        {
                            mensagens.Add(aluno.Turma + " - A soma dos campo QUANTIDADE PRESENTES + AMPARADOS + AFASTAMENTOS COVID não deve ser maior que a quantidade de matriculados.");
                        }
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
    
        public void Salva(List<Entidades.AlunosAusentes> alunosAusentes)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            try
            {
                foreach (Entidades.AlunosAusentes aluno in alunosAusentes)
                {
                    //Verifica se já existe cadastro
                    aluno.AlunosAusentesId = this.ObtemIdPor(contexto, aluno.Censo, aluno.Ano, aluno.Periodo, aluno.Turma, aluno.DataLancamento);

                    if (aluno.AlunosAusentesId > 0)
                    {
                        this.Atualiza(contexto, aluno);
                    }
                    else
                    {
                        this.Insere(contexto, aluno);
                    }
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

        private int ObtemIdPor(DataContext contexto, string censo, int ano, int periodo, string turma, DateTime dataLancamento)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" 	SELECT ALUNOSAUSENTESID
	                                        FROM  Turma.ALUNOSAUSENTES A (NOLOCK) 
	                                        WHERE A.TURMA = @TURMA
			                                        AND A.ANO = @ANO
			                                        AND A.PERIODO = @PERIODO
			                                        AND A.CENSO = @CENSO
			                                        AND CONVERT(DATE, A.DATALANCAMENTO) = CONVERT(DATE, @DATALANCAMENTO) ";

                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@DATALANCAMENTO", SqlDbType.DateTime, dataLancamento); 

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["ALUNOSAUSENTESID"]);
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

        private void Insere(DataContext contexto, Entidades.AlunosAusentes alunoAusente)
        { 
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Turma.ALUNOSAUSENTES
                                           (CENSO
                                           ,ANO
                                           ,PERIODO
                                           ,TURMA
                                           ,DATALANCAMENTO
                                           ,QUANTIDADEMATRICULADOS
                                           ,QUANTIDADEPRESENTES
                                           ,QUANTIDADEAMPARADOS
                                           ,QUANTIDADEAFASTAMENTOSCOVID
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@CENSO,
                                           @ANO, 
                                           @PERIODO, 
                                           @TURMA,
                                           @DATALANCAMENTO, 
                                           @QUANTIDADEMATRICULADOS,
                                           @QUANTIDADEPRESENTES, 
                                           @QUANTIDADEAMPARADOS, 
                                           @QUANTIDADEAFASTAMENTOSCOVID,
                                           @USUARIOID, 
                                           @DATACADASTRO, 
                                           @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, alunoAusente.Censo);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, alunoAusente.Ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, alunoAusente.Periodo);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, alunoAusente.Turma);
            contextQuery.Parameters.Add("@DATALANCAMENTO", SqlDbType.DateTime, alunoAusente.DataLancamento.Date);
            contextQuery.Parameters.Add("@QUANTIDADEMATRICULADOS", SqlDbType.Int, alunoAusente.QuantidadeMatriculados);
            contextQuery.Parameters.Add("@QUANTIDADEPRESENTES", SqlDbType.Int, alunoAusente.QuantidadePresentes);
            contextQuery.Parameters.Add("@QUANTIDADEAMPARADOS", SqlDbType.Int, alunoAusente.QuantidadeAmparados);
            contextQuery.Parameters.Add("@QUANTIDADEAFASTAMENTOSCOVID", SqlDbType.Int, alunoAusente.QuantidadeAfastamentosCovid);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, alunoAusente.UsuarioID);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now); 

            contexto.ApplyModifications(contextQuery);
        }

        private void Atualiza(DataContext contexto, Entidades.AlunosAusentes alunoAusente)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Turma.ALUNOSAUSENTES
                                        SET QUANTIDADEMATRICULADOS = @QUANTIDADEMATRICULADOS,
                                            QUANTIDADEPRESENTES = @QUANTIDADEPRESENTES,
                                            QUANTIDADEAMPARADOS = @QUANTIDADEAMPARADOS,
                                            QUANTIDADEAFASTAMENTOSCOVID = @QUANTIDADEAFASTAMENTOSCOVID,
                                            USUARIOID = @USUARIOID,
		                                    DATAALTERACAO = @DATAALTERACAO
                                    WHERE ALUNOSAUSENTESID = @ALUNOSAUSENTESID ";

            contextQuery.Parameters.Add("@ALUNOSAUSENTESID", SqlDbType.Int, alunoAusente.AlunosAusentesId);
            contextQuery.Parameters.Add("@QUANTIDADEMATRICULADOS", SqlDbType.Int, alunoAusente.QuantidadeMatriculados);
            contextQuery.Parameters.Add("@QUANTIDADEPRESENTES", SqlDbType.Int, alunoAusente.QuantidadePresentes);
            contextQuery.Parameters.Add("@QUANTIDADEAMPARADOS", SqlDbType.Int, alunoAusente.QuantidadeAmparados);
            contextQuery.Parameters.Add("@QUANTIDADEAFASTAMENTOSCOVID", SqlDbType.Int, alunoAusente.QuantidadeAfastamentosCovid);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, alunoAusente.UsuarioID);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now); 

            contexto.ApplyModifications(contextQuery);
        }
    }
}
