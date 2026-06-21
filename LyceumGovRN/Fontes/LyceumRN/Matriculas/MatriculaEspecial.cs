using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.DTOs;

namespace Techne.Lyceum.RN.Matriculas
{
    public class MatriculaEspecial
    {
        public int RetornaQuantidadeUtilizadaPor(string disciplina, string turno, int ano)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return RetornaQuantidadeUtilizadaPor(contexto, disciplina, turno, ano);
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

        public int RetornaSaldoPor(string disciplina, string turno, int ano)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            int retorno = 0;

            try
            {
                int liberadas = this.RetornaQuantidadeLiberadaPor(contexto, disciplina, turno, ano);
                int utilizadas = this.RetornaQuantidadeUtilizadaPor(contexto, disciplina, turno, ano);
                retorno = liberadas - utilizadas;

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

        private int RetornaQuantidadeLiberadaPor(DataContext contexto, string disciplina, string turno, int ano)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                int periodo = 0;
                string censo = "33183554"; //Escola onde serão lançadas as disciplinas
                int serie = 3; //Serie onde serão lançadas as disciplinas

                contextQuery.Command = @" SELECT VAGAS_LIBERADAS
                        FROM    dbo.TCE_CONTROLE_VAGA (NOLOCK)
                        WHERE   ANO = @ANO
								AND PERIODO = @PERIODO 
								AND CENSO = @CENSO
								AND CURSO = @CURSO
								AND SERIE = @SERIE
								AND TURNO = @TURNO ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, disciplina);
                contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);
                contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["VAGAS_LIBERADAS"]);
                }

                return retorno;
            }            
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }                
            }
        }

        private int RetornaQuantidadeUtilizadaPor(DataContext contexto, string disciplina, string turno, int ano)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT COUNT(1) UTILIZADAS
                            FROM MATRICULA.MATRICULAESPECIALDISCIPLINA d
								INNER JOIN MATRICULA.MATRICULAESPECIAL M 
										ON D.MATRICULAESPECIALID = M.MATRICULAESPECIALID
                            WHERE d.DISCIPLINA = @DISCIPLINA
								AND d.TURNO = @TURNO
                                AND M.ANO = @ANO
                                AND d.DATACONVOCACAO IS NOT NULL ";

                contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, disciplina);
                contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["UTILIZADAS"]);
                }

                return retorno;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public int RetornaQuantidadeFilaPor(string disciplina, string turno, int ano)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.RetornaQuantidadeFilaPor(contexto, disciplina, turno, ano);
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

        private int RetornaQuantidadeFilaPor(DataContext contexto, string disciplina, string turno, int ano)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT COUNT(1) PENDENTES
                            FROM MATRICULA.MATRICULAESPECIALDISCIPLINA d
								INNER JOIN MATRICULA.MATRICULAESPECIAL M 
										ON D.MATRICULAESPECIALID = M.MATRICULAESPECIALID
                            WHERE d.DISCIPLINA = @DISCIPLINA
								AND d.TURNO = @TURNO
                                AND m.ANO = @ANO
                                AND d.DATACONVOCACAO IS NULL  ";

                contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, disciplina);
                contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["PENDENTES"]);
                }

                return retorno;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public ValidacaoDados ValidaConvocacao(string disciplina, string turno, int ano, string usuarioId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Matriculas.EncaminhamentoEspecial rnEncaminhamentoEspecial = new EncaminhamentoEspecial();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (disciplina.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DISCIPLINA é obrigatório.");
            }

            if (turno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TURNO é obrigatório.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se possui vaga
                    int liberadas = this.RetornaQuantidadeLiberadaPor(contexto, disciplina, turno, ano);
                    int utilizadas = this.RetornaQuantidadeUtilizadaPor(contexto, disciplina, turno, ano);

                    //Verifica se ainda existe vaga
                    if (liberadas <= utilizadas)
                    {
                        mensagens.Add("Não existem mais vagas para a DISCIPLINA / TURNO.");
                    }

                    //Verifica se possui pessoas para convocar
                    if (this.RetornaQuantidadeFilaPor(contexto, disciplina, turno, ano) == 0)
                    {
                        mensagens.Add("Não existem candidatos para convocar na DISCIPLINA / TURNO.");
                    }
                }
                catch
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw;
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

        public void Convoca(string disciplina, string turno, int ano, string usuarioId, out string aviso)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            List<DTOs.DadosCandidatoMatriculaEspecial> candidatosConvocados = new List<Techne.Lyceum.RN.DTOs.DadosCandidatoMatriculaEspecial>();
            RN.Matriculas.MatriculaEspecialDisciplina rnMatriculaEspecialDisciplina = new MatriculaEspecialDisciplina();
            RN.Matriculas.ConvocacaoDisciplinaSemEmail rnConvocacaoDisciplinaSemEmail = new ConvocacaoDisciplinaSemEmail();
            RN.DTOs.DadosEmail dadosEmail = new DadosEmail();
            RN.Turno rnTurno = new Turno();
            int vagas = 0;
            int alunosConvocados = 0;
            bool emailEnviado = false;
            aviso = string.Empty;
           // List<string> listaAvisos = new List<string>();
            List<string> listaConvocados = new List<string>();
            List<string> listaEmailsNaoEnviados = new List<string>();

            try
            {
                //Obtem quantidade vagas disponiveis
                vagas = this.RetornaQuantidadeLiberadaPor(contexto, disciplina, turno, ano) - this.RetornaQuantidadeUtilizadaPor(contexto, disciplina, turno, ano);

                //Busca descricao do turno
                string descricaoturno = rnTurno.RetornaDescricaoTurno(turno);

                //Buscar os primeiros alunos não convocados
                candidatosConvocados = this.ListaNaoConvocadosPor(contexto, vagas, disciplina, turno, ano);

                //Convoca
                foreach (DTOs.DadosCandidatoMatriculaEspecial candidato in candidatosConvocados)
                {
                    int liberadas = this.RetornaQuantidadeLiberadaPor(contexto, disciplina, turno, ano);
                    int utilizadas = this.RetornaQuantidadeUtilizadaPor(contexto, disciplina, turno, ano);

                    //Verifica se ainda existe vaga
                    if (liberadas <= utilizadas)
                    {
                        break;
                        //continue;
                    }
                    
                    alunosConvocados++;
                    candidato.Convocado = true;

                    //Atualiza dados da convocacao
                    rnMatriculaEspecialDisciplina.AtualizaConvocacao(contexto, candidato.MatriculaEspecialDisciplinaId, usuarioId);

                    listaConvocados.Add(string.Format(". {0} - Nome: {1}, Email Educacional: {2}, Email Pessoal: {3}, Telefone {4}",
                            Convert.ToString(candidato.Aluno),
                            candidato.Nome,
                            candidato.EmailGoogle,
                            candidato.Email,
                            candidato.Telefone
                        ));
                }

                aviso = string.Format("Foram Convocado(s) {0} candidato(s): <br/>", alunosConvocados);
                aviso += listaConvocados.Aggregate((x, y) => x + "<br/>" + y);               

                //Fecha contexto com convocaçoes
                contexto.Dispose();

                //Envia email para quem foi convocado
                foreach (DTOs.DadosCandidatoMatriculaEspecial candidato in candidatosConvocados.Where(x => x.Convocado == true))
                {
                    //Caso o aluno possua EmailGoogle utilizar, caso nao possua utilizar email pessoal
                    string email = candidato.EmailGoogle.IsNullOrEmptyOrWhiteSpace() ? candidato.Email : candidato.EmailGoogle;

                    //Monta email 
                    dadosEmail.Destinatario = email;
                    dadosEmail.Remetente = System.Configuration.ConfigurationManager.AppSettings["EmailMatriculaFase3"].ToString();
                    dadosEmail.Login = System.Configuration.ConfigurationManager.AppSettings["EmailMatriculaFase3_Login"].ToString();
                    dadosEmail.Senha = System.Configuration.ConfigurationManager.AppSettings["EmailMatriculaFase3_Senha"].ToString();
                    dadosEmail.Assunto = "Convocação Matrícula - Turmas Reforço";
                    dadosEmail.Texto = string.Format(@"<br />{0}
                                            <br />Você foi convocado para a turma de reforço de {1} no turno {2}.
                                            <br />Para ter acesso gratuito às turmas, a partir do dia 19/04/2021, acesse o Google Classroom através do Aplicativo <b>Applique-se</b> e faça seu login com email educacional: {3}. 
                                            <br />A senha do primeiro acesso é sua data de nascimento, caso já tenha trocado sua senha inicial e não se lembre, acesse o link https://sites.google.com/educa.rj.gov.br/aluno/e-mail-e-troca-de-senha para consulta.
                                            <br />
                                            <br />Seeduc", candidato.Nome, candidato.NomeDisciplina, candidato.NomeTurno, candidato.EmailGoogle);

                    //Tentar Enviar e-mail
                    try
                    {
                        //Envia e-mail
                        RN.Util.Email.Envia(dadosEmail);
                        emailEnviado = true;
                    }
                    catch (Exception)
                    {
                        emailEnviado = false;
                    }

                    //Verifica se não foi possivel enviar o e-mail
                    if (!emailEnviado)
                    {
                        listaEmailsNaoEnviados.Add(string.Format(". {0} - Nome: {1}, Email Educacional: {2}, Email Pessoal: {3}, Telefone {4}",
                            Convert.ToString(candidato.Aluno),
                            candidato.Nome,
                            candidato.EmailGoogle,
                            candidato.Email,
                            candidato.Telefone
                        ));

                        //Insere registro de convocacao de disciplina sem email
                        rnConvocacaoDisciplinaSemEmail.Insere(candidato.MatriculaEspecialDisciplinaId, candidato.MatriculaEspecialId, usuarioId);
                    }
                }

                if (listaEmailsNaoEnviados.Count > 0)
                {
                    aviso += "<br/><br/><u>Houveram falhas no envio do email, favor entrar em contado com o(s) candidato(s):</u><br/>";
                    aviso += listaEmailsNaoEnviados.Aggregate((x, y) => x + "<br/>" + y);
                }
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                contexto.Dispose();
                throw ex;
            }
        }

        private List<DTOs.DadosCandidatoMatriculaEspecial> ListaNaoConvocadosPor(DataContext contexto, int limite, string disciplina, string turno, int ano)
        {
            List<DTOs.DadosCandidatoMatriculaEspecial> candidatosConvocados = new List<DadosCandidatoMatriculaEspecial>();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader dataReader = null;

            try
            {
                contextQuery.Command = @" SELECT TOP(@TOP) D.MATRICULAESPECIALDISCIPLINAID, 
								            D.MATRICULAESPECIALID,
								            A.ALUNO, 
								            P.NOME_COMPL,
								            P.E_MAIL AS EMAIL, 
								            GE.EMAIL as EMAILGOOGLE,
								            ISNULL(P.CELULAR, P.FONE) AS TELEFONE,
								            d.DISCIPLINA,
								            C.NOME AS NOMEDISCIPLINA,
								            d.TURNO,
								            T.DESCRICAO AS NOMETURNO
                                        FROM MATRICULA.MATRICULAESPECIALDISCIPLINA d
								            INNER JOIN MATRICULA.MATRICULAESPECIAL M ON D.MATRICULAESPECIALID = M.MATRICULAESPECIALID
								            INNER JOIN LY_CURSO C (NOLOCK) ON d.DISCIPLINA = C.CURSO
								            INNER JOIN LY_TURNO T (NOLOCK) ON T.TURNO = D.TURNO
								            INNER JOIN LY_ALUNO A (NOLOCK) ON M.ALUNO = A.ALUNO
								            LEFT JOIN RecursosHumanos.GOOGLEEDUCATION GE (nolock) on A.ALUNO = GE.ALUNO
								            INNER JOIN LY_PESSOA P (NOLOCK) ON A.PESSOA = P.PESSOA
                                        WHERE d.DISCIPLINA = @DISCIPLINA
								            AND d.TURNO = @TURNO
                                            AND m.ANO = @ANO
                                            AND d.DATACONVOCACAO IS NULL
                                        ORDER BY D.MATRICULAESPECIALDISCIPLINAID ";

                contextQuery.Parameters.Add("@TOP", SqlDbType.Int, limite);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, disciplina);
                contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);

                dataReader = contexto.GetDataReader(contextQuery);

                while (dataReader.Read())
                {
                    DadosCandidatoMatriculaEspecial dados = new DadosCandidatoMatriculaEspecial();

                    dados.MatriculaEspecialDisciplinaId = Convert.ToInt32(dataReader["MATRICULAESPECIALDISCIPLINAID"]);
                    dados.MatriculaEspecialId = Convert.ToInt32(dataReader["MATRICULAESPECIALID"]);
                    dados.Aluno = Convert.ToString(dataReader["ALUNO"]);
                    dados.Nome = Convert.ToString(dataReader["NOME_COMPL"]);
                    dados.Email = Convert.ToString(dataReader["EMAIL"]);
                    dados.EmailGoogle = Convert.ToString(dataReader["EMAILGOOGLE"]);
                    dados.Telefone = Convert.ToString(dataReader["TELEFONE"]);
                    dados.Disciplina = Convert.ToString(dataReader["DISCIPLINA"]);
                    dados.NomeDisciplina = Convert.ToString(dataReader["NOMEDISCIPLINA"]);
                    dados.Turno = Convert.ToString(dataReader["TURNO"]);
                    dados.NomeTurno = Convert.ToString(dataReader["NOMETURNO"]);

                    candidatosConvocados.Add(dados);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
            }
            return candidatosConvocados;
        }
    }
}