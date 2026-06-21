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
    public class CardapioEletiva
    {
        public DTOs.DadosCardapio ObtemPor(string censo, int ano, int periodo, string curso, int serie)
        {
            DTOs.DadosCardapio dadosCardapio = new DTOs.DadosCardapio();
            CardapioEletivaDisciplina rnCardapioEletivaDisciplina = new CardapioEletivaDisciplina();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            List<Entidades.CardapioEletivaDisciplina> disciplinas = new List<Entidades.CardapioEletivaDisciplina>();

            try
            {
                //Busca dados principais
                dadosCardapio = this.ObtemPor(contexto, censo, ano, periodo, curso, serie);

                if (dadosCardapio.CardapioEletivaId > 0)
                {
                    disciplinas = rnCardapioEletivaDisciplina.ObtemListaPor(contexto, dadosCardapio.CardapioEletivaId);

                    //Alimenta opção por turno
                    var disciplinasManha = disciplinas.Where(x => x.Turno == "M").ToList();
                    if (disciplinasManha.Count > 0)
                    {
                        dadosCardapio.DisciplinaManha1 = disciplinasManha[0].Disciplina;
                        dadosCardapio.DisciplinaManha2 = disciplinasManha[1].Disciplina;
                    }

                    var disciplinasTarde = disciplinas.Where(x => x.Turno == "T").ToList();
                    if (disciplinasTarde.Count > 0)
                    {
                        dadosCardapio.DisciplinaTarde1 = disciplinasTarde[0].Disciplina;
                        dadosCardapio.DisciplinaTarde2 = disciplinasTarde[1].Disciplina;
                    }

                    var disciplinasNoite = disciplinas.Where(x => x.Turno == "N").ToList();
                    if (disciplinasNoite.Count > 0)
                    {
                        dadosCardapio.DisciplinaNoite1 = disciplinasNoite[0].Disciplina;
                        dadosCardapio.DisciplinaNoite2 = disciplinasNoite[1].Disciplina;
                    }

                    var disciplinasIntegral = disciplinas.Where(x => x.Turno == "I").ToList();
                    if (disciplinasIntegral.Count > 0)
                    {
                        dadosCardapio.DisciplinaIntegral1 = disciplinasIntegral[0].Disciplina;
                        dadosCardapio.DisciplinaIntegral2 = disciplinasIntegral[1].Disciplina;
                    }

                    var disciplinasAmpliado = disciplinas.Where(x => x.Turno == "A").ToList();
                    if (disciplinasAmpliado.Count > 0)
                    {
                        dadosCardapio.DisciplinaAmpliado1 = disciplinasAmpliado[0].Disciplina;
                        dadosCardapio.DisciplinaAmpliado2 = disciplinasAmpliado[1].Disciplina;
                    }
                }

                return dadosCardapio;
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

        private DTOs.DadosCardapio ObtemPor(DataContext contexto, string censo, int ano, int periodo, string curso, int serie)
        {
            DTOs.DadosCardapio dadosCardapio = new DTOs.DadosCardapio();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  CARDAPIOELETIVAID, 
		                                        CENSO, 
		                                        ANO, 
		                                        PERIODO, 
		                                        CURSO, 
		                                        SERIE, 
		                                        VALIDADO, 
		                                        USUARIOVALIDACAO, 
												UV.NOME AS NOMEUSUARIOVALIDACAO,
		                                        DATAVALIDACAO, 
		                                        FINALIZADO, 
		                                        USUARIOFINALIZACAO,
												UF.NOME AS NOMEUSUARIOFINALIZACAOO, 
		                                        DATAFINALIZACAO, 
		                                        USUARIOID, 
		                                        DATACADASTRO, 
		                                        DATAALTERACAO
                                        FROM Turma.CARDAPIOELETIVA C
											 LEFT JOIN HADES..HD_USUARIO UV ON C.USUARIOVALIDACAO = UV.USUARIO
											 LEFT JOIN HADES..HD_USUARIO UF ON C.USUARIOFINALIZACAO = UF.USUARIO
                                        WHERE CENSO = @CENSO
		                                        AND ANO = @ANO 
		                                        AND PERIODO = @PERIODO
		                                        AND CURSO = @CURSO
		                                        AND SERIE = @SERIE ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
                contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosCardapio.Censo = Convert.ToString(reader["CENSO"]);
                    dadosCardapio.Ano = Convert.ToInt32(reader["ANO"]);
                    dadosCardapio.Periodo = Convert.ToInt32(reader["PERIODO"]);
                    dadosCardapio.Curso = Convert.ToString(reader["CURSO"]);
                    dadosCardapio.Serie = Convert.ToInt32(reader["SERIE"]);
                    dadosCardapio.CardapioEletivaId = Convert.ToInt32(reader["CARDAPIOELETIVAID"]);
                    dadosCardapio.Validado = Convert.ToBoolean(reader["VALIDADO"]);

                    if (dadosCardapio.Validado)
                    {
                        dadosCardapio.UsuarioValidacao = Convert.ToString(reader["USUARIOVALIDACAO"]);
                        dadosCardapio.DataValidacao = Convert.ToDateTime(reader["DATAVALIDACAO"]);
                        dadosCardapio.NomeUsuarioValidacao = Convert.ToString(reader["NOMEUSUARIOVALIDACAO"]);
                    }

                    dadosCardapio.Finalizado = Convert.ToBoolean(reader["FINALIZADO"]);

                    if (dadosCardapio.Finalizado)
                    {
                        dadosCardapio.UsuarioFinalizacao = Convert.ToString(reader["USUARIOFINALIZACAO"]);
                        dadosCardapio.DataFinalizacao = Convert.ToDateTime(reader["DATAFINALIZACAO"]);
                        dadosCardapio.NomeUsuarioFinalizacao = Convert.ToString(reader["NOMEUSUARIOFINALIZACAOO"]);
                    }
                }

                return dadosCardapio;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public ValidacaoDados Valida(DTOs.DadosCardapio dadosCardapio)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Turma rnTurma = new Turma();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosCardapio == null)
            {
                return validacaoDados;
            }

            if (dadosCardapio.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE DE ENSINO é obrigatório.");
            }

            if (dadosCardapio.Ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (dadosCardapio.Periodo < 0)
            {
                mensagens.Add("Campo PERIODO é obrigatório.");
            }

            if (dadosCardapio.Curso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CURSO é obrigatório.");
            }

            if (dadosCardapio.Serie <= 0)
            {
                mensagens.Add("Campo SERIE é obrigatório.");
            }

            if (!dadosCardapio.DisciplinaManha1.IsNullOrEmptyOrWhiteSpace() || !dadosCardapio.DisciplinaManha2.IsNullOrEmptyOrWhiteSpace())
            {
                if (dadosCardapio.DisciplinaManha1.IsNullOrEmptyOrWhiteSpace() || dadosCardapio.DisciplinaManha2.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Falta(m) 1 eletiva(s) do turno manhã.");
                }
                else if (dadosCardapio.DisciplinaManha1 == dadosCardapio.DisciplinaManha2)
                {
                    mensagens.Add("As eletivas do turno manhã não podem ser iguais.");
                }
            }

            if (!dadosCardapio.DisciplinaTarde1.IsNullOrEmptyOrWhiteSpace() || !dadosCardapio.DisciplinaTarde2.IsNullOrEmptyOrWhiteSpace())
            {
                if (dadosCardapio.DisciplinaTarde1.IsNullOrEmptyOrWhiteSpace() || dadosCardapio.DisciplinaTarde2.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Falta(m) 1 eletiva(s) do turno tarde.");
                }
                else if (dadosCardapio.DisciplinaTarde1 == dadosCardapio.DisciplinaTarde2)
                {
                    mensagens.Add("As eletivas do turno tarde não podem ser iguais.");
                }
            }

            if (!dadosCardapio.DisciplinaNoite1.IsNullOrEmptyOrWhiteSpace() || !dadosCardapio.DisciplinaNoite2.IsNullOrEmptyOrWhiteSpace())
            {
                if (dadosCardapio.DisciplinaNoite1.IsNullOrEmptyOrWhiteSpace() || dadosCardapio.DisciplinaNoite2.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Falta(m) 1 eletiva(s) do turno noite.");
                }
                else if (dadosCardapio.DisciplinaNoite1 == dadosCardapio.DisciplinaNoite2)
                {
                    mensagens.Add("As eletivas do turno noite não podem ser iguais.");
                }
            }

            if (!dadosCardapio.DisciplinaIntegral1.IsNullOrEmptyOrWhiteSpace() || !dadosCardapio.DisciplinaIntegral2.IsNullOrEmptyOrWhiteSpace())
            {
                if (dadosCardapio.DisciplinaIntegral1.IsNullOrEmptyOrWhiteSpace() || dadosCardapio.DisciplinaIntegral2.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Falta(m) 1 eletiva(s) do turno integral.");
                }
                else if (dadosCardapio.DisciplinaIntegral1 == dadosCardapio.DisciplinaIntegral2)
                {
                    mensagens.Add("As eletivas do turno integral não podem ser iguais.");
                }
            }

            if (!dadosCardapio.DisciplinaAmpliado1.IsNullOrEmptyOrWhiteSpace() || !dadosCardapio.DisciplinaAmpliado2.IsNullOrEmptyOrWhiteSpace())
            {
                if (dadosCardapio.DisciplinaAmpliado1.IsNullOrEmptyOrWhiteSpace() || dadosCardapio.DisciplinaAmpliado2.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Falta(m) 1 eletiva(s) do turno ampliado.");
                }
                else if (dadosCardapio.DisciplinaAmpliado1 == dadosCardapio.DisciplinaAmpliado2)
                {
                    mensagens.Add("As eletivas do turno ampliado não podem ser iguais.");
                }
            }

            if (dadosCardapio.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Busca os turnos em que a escola, curso, serie podem ser habilitados
                    var turnos = rnTurma.ListaTurnoPor(contexto, dadosCardapio.Censo, dadosCardapio.Ano, dadosCardapio.Periodo, dadosCardapio.Curso, dadosCardapio.Serie);

                    //Verifica se algum turno habilitado nao foi informado
                    if (turnos.Contains("M") && dadosCardapio.DisciplinaManha1.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Falta(m) 2 eletiva(s) do turno manhã.");
                    }

                    if (turnos.Contains("T") && dadosCardapio.DisciplinaTarde1.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Falta(m) 2 eletiva(s) do turno tarde.");
                    }

                    if (turnos.Contains("N") && dadosCardapio.DisciplinaNoite1.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Falta(m) 2 eletiva(s) do turno noite.");
                    }

                    if (turnos.Contains("I") && dadosCardapio.DisciplinaIntegral1.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Falta(m) 2 eletiva(s) do turno integral.");
                    }

                    if (turnos.Contains("A") && dadosCardapio.DisciplinaAmpliado1.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Falta(m) 2 eletiva(s) do turno ampliado.");
                    }

                    //Verifica se algum turno nao habilitado foi informado
                    if (!turnos.Contains("M") && !dadosCardapio.DisciplinaManha1.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("O turno manhã não está habilitado.");
                    }

                    if (!turnos.Contains("T") && !dadosCardapio.DisciplinaTarde1.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("O turno tarde não está habilitado.");
                    }

                    if (!turnos.Contains("N") && !dadosCardapio.DisciplinaNoite1.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("O noite manhã não está habilitado.");
                    }

                    if (!turnos.Contains("I") && !dadosCardapio.DisciplinaIntegral1.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("O turno integral não está habilitado.");
                    }

                    if (!turnos.Contains("A") && !dadosCardapio.DisciplinaAmpliado1.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("O turno ampliado não está habilitado.");
                    }

                    //Verifica se o carpadio ja foi validado
                    if (this.EhValidadoPor(contexto, dadosCardapio.Censo, dadosCardapio.Ano, dadosCardapio.Periodo, dadosCardapio.Curso, dadosCardapio.Serie))
                    {
                        mensagens.Add("Não é permitido alterar este censo / ano / período / curso / série, pois ele já foi validado.");
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

        public void Salva(DTOs.DadosCardapio dadosCardapio)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            CardapioEletivaDisciplina rnCardapioEletivaDisciplina = new CardapioEletivaDisciplina();
            int id = 0;

            try
            {
                //Verifica se já existe cadastro
                id = this.ObtemIdPor(contexto, dadosCardapio.Censo, dadosCardapio.Ano, dadosCardapio.Periodo, dadosCardapio.Curso, dadosCardapio.Serie);

                if (id > 0)
                {
                    //Atualiza data e usuario da ultima alteracao
                    dadosCardapio.CardapioEletivaId = id;

                    this.AtualizaUltimaAcao(contexto, dadosCardapio);

                    //Remove todas as disciplinas anteriores
                    rnCardapioEletivaDisciplina.RemoveTodas(contexto, dadosCardapio.CardapioEletivaId);

                    //Insere disciplinas
                    if (!dadosCardapio.DisciplinaManha1.IsNullOrEmptyOrWhiteSpace())
                    {
                        rnCardapioEletivaDisciplina.Insere(contexto, dadosCardapio.CardapioEletivaId, "M", dadosCardapio.DisciplinaManha1, dadosCardapio.UsuarioId);
                        rnCardapioEletivaDisciplina.Insere(contexto, dadosCardapio.CardapioEletivaId, "M", dadosCardapio.DisciplinaManha2, dadosCardapio.UsuarioId);
                    }

                    if (!dadosCardapio.DisciplinaTarde1.IsNullOrEmptyOrWhiteSpace())
                    {
                        rnCardapioEletivaDisciplina.Insere(contexto, dadosCardapio.CardapioEletivaId, "T", dadosCardapio.DisciplinaTarde1, dadosCardapio.UsuarioId);
                        rnCardapioEletivaDisciplina.Insere(contexto, dadosCardapio.CardapioEletivaId, "T", dadosCardapio.DisciplinaTarde2, dadosCardapio.UsuarioId);
                    }

                    if (!dadosCardapio.DisciplinaNoite1.IsNullOrEmptyOrWhiteSpace())
                    {
                        rnCardapioEletivaDisciplina.Insere(contexto, dadosCardapio.CardapioEletivaId, "N", dadosCardapio.DisciplinaNoite1, dadosCardapio.UsuarioId);
                        rnCardapioEletivaDisciplina.Insere(contexto, dadosCardapio.CardapioEletivaId, "N", dadosCardapio.DisciplinaNoite2, dadosCardapio.UsuarioId);
                    }

                    if (!dadosCardapio.DisciplinaIntegral1.IsNullOrEmptyOrWhiteSpace())
                    {
                        rnCardapioEletivaDisciplina.Insere(contexto, dadosCardapio.CardapioEletivaId, "I", dadosCardapio.DisciplinaIntegral1, dadosCardapio.UsuarioId);
                        rnCardapioEletivaDisciplina.Insere(contexto, dadosCardapio.CardapioEletivaId, "I", dadosCardapio.DisciplinaIntegral2, dadosCardapio.UsuarioId);
                    }

                    if (!dadosCardapio.DisciplinaAmpliado1.IsNullOrEmptyOrWhiteSpace())
                    {
                        rnCardapioEletivaDisciplina.Insere(contexto, dadosCardapio.CardapioEletivaId, "A", dadosCardapio.DisciplinaAmpliado1, dadosCardapio.UsuarioId);
                        rnCardapioEletivaDisciplina.Insere(contexto, dadosCardapio.CardapioEletivaId, "A", dadosCardapio.DisciplinaAmpliado2, dadosCardapio.UsuarioId);
                    }
                }
                else
                {
                    this.Insere(contexto, dadosCardapio);

                    //Insere disciplinas
                    if (!dadosCardapio.DisciplinaManha1.IsNullOrEmptyOrWhiteSpace())
                    {
                        rnCardapioEletivaDisciplina.Insere(contexto, dadosCardapio.CardapioEletivaId, "M", dadosCardapio.DisciplinaManha1, dadosCardapio.UsuarioId);
                        rnCardapioEletivaDisciplina.Insere(contexto, dadosCardapio.CardapioEletivaId, "M", dadosCardapio.DisciplinaManha2, dadosCardapio.UsuarioId);
                    }

                    if (!dadosCardapio.DisciplinaTarde1.IsNullOrEmptyOrWhiteSpace())
                    {
                        rnCardapioEletivaDisciplina.Insere(contexto, dadosCardapio.CardapioEletivaId, "T", dadosCardapio.DisciplinaTarde1, dadosCardapio.UsuarioId);
                        rnCardapioEletivaDisciplina.Insere(contexto, dadosCardapio.CardapioEletivaId, "T", dadosCardapio.DisciplinaTarde2, dadosCardapio.UsuarioId);
                    }

                    if (!dadosCardapio.DisciplinaNoite1.IsNullOrEmptyOrWhiteSpace())
                    {
                        rnCardapioEletivaDisciplina.Insere(contexto, dadosCardapio.CardapioEletivaId, "N", dadosCardapio.DisciplinaNoite1, dadosCardapio.UsuarioId);
                        rnCardapioEletivaDisciplina.Insere(contexto, dadosCardapio.CardapioEletivaId, "N", dadosCardapio.DisciplinaNoite2, dadosCardapio.UsuarioId);
                    }

                    if (!dadosCardapio.DisciplinaIntegral1.IsNullOrEmptyOrWhiteSpace())
                    {
                        rnCardapioEletivaDisciplina.Insere(contexto, dadosCardapio.CardapioEletivaId, "I", dadosCardapio.DisciplinaIntegral1, dadosCardapio.UsuarioId);
                        rnCardapioEletivaDisciplina.Insere(contexto, dadosCardapio.CardapioEletivaId, "I", dadosCardapio.DisciplinaIntegral2, dadosCardapio.UsuarioId);
                    }

                    if (!dadosCardapio.DisciplinaAmpliado1.IsNullOrEmptyOrWhiteSpace())
                    {
                        rnCardapioEletivaDisciplina.Insere(contexto, dadosCardapio.CardapioEletivaId, "A", dadosCardapio.DisciplinaAmpliado1, dadosCardapio.UsuarioId);
                        rnCardapioEletivaDisciplina.Insere(contexto, dadosCardapio.CardapioEletivaId, "A", dadosCardapio.DisciplinaAmpliado2, dadosCardapio.UsuarioId);
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

        private bool ExistePor(DataContext contexto, string censo, int ano, int periodo, string curso, int serie)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT  COUNT(1)
                                        FROM Turma.CARDAPIOELETIVA C
                                        WHERE CENSO = @CENSO
		                                        AND ANO = @ANO 
		                                        AND PERIODO = @PERIODO
		                                        AND CURSO = @CURSO
		                                        AND SERIE = @SERIE ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
            contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private int ObtemQuantidadePor(DataContext contexto, string censo, int ano, int periodo, string curso, int serie, string turno)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT  COUNT(1) AS QTDE
                                        FROM Turma.CARDAPIOELETIVA C
												INNER JOIN Turma.CARDAPIOELETIVADISCIPLINA d on C.CARDAPIOELETIVAID = d.CARDAPIOELETIVAID
                                        WHERE CENSO = @CENSO
		                                        AND ANO = @ANO 
		                                        AND PERIODO = @PERIODO
		                                        AND CURSO = @CURSO
		                                        AND SERIE = @SERIE
												AND D.TURNO = @TURNO ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
                contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);
                contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["QTDE"]);
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

        private int ObtemIdPor(DataContext contexto, string censo, int ano, int periodo, string curso, int serie)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT  CARDAPIOELETIVAID
                                        FROM Turma.CARDAPIOELETIVA C
                                        WHERE CENSO = @CENSO
		                                        AND ANO = @ANO 
		                                        AND PERIODO = @PERIODO
		                                        AND CURSO = @CURSO
		                                        AND SERIE = @SERIE ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
                contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["CARDAPIOELETIVAID"]);
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

        private bool EhValidadoPor(DataContext contexto, string censo, int ano, int periodo, string curso, int serie)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT  COUNT(1)
                                        FROM Turma.CARDAPIOELETIVA
                                        WHERE CENSO = @CENSO
		                                        AND ANO = @ANO 
		                                        AND PERIODO = @PERIODO
		                                        AND CURSO = @CURSO
		                                        AND SERIE = @SERIE
												AND VALIDADO = 1 ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
            contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool EhFinalizadoPor(DataContext contexto, string censo, int ano, int periodo, string curso, int serie)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT  COUNT(1)
                                        FROM Turma.CARDAPIOELETIVA
                                        WHERE CENSO = @CENSO
		                                        AND ANO = @ANO 
		                                        AND PERIODO = @PERIODO
		                                        AND CURSO = @CURSO
		                                        AND SERIE = @SERIE
												AND FINALIZADO = 1 ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
            contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private void Insere(DataContext contexto, DTOs.DadosCardapio dadosCardapio)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Turma.CARDAPIOELETIVA
                                           (CENSO
                                           ,ANO
                                           ,PERIODO
                                           ,CURSO
                                           ,SERIE
                                           ,VALIDADO
                                           ,FINALIZADO
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@CENSO, 
                                           @ANO, 
                                           @PERIODO, 
                                           @CURSO, 
                                           @SERIE, 
                                           @VALIDADO, 
                                           @FINALIZADO, 
                                           @USUARIOID, 
                                           @DATACADASTRO, 
                                           @DATAALTERACAO)

                                 SELECT IDENT_CURRENT('Turma.CARDAPIOELETIVA') ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, dadosCardapio.Censo);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, dadosCardapio.Ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, dadosCardapio.Periodo);
            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, dadosCardapio.Curso);
            contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, dadosCardapio.Serie);
            contextQuery.Parameters.Add("@VALIDADO", SqlDbType.Bit, false);
            contextQuery.Parameters.Add("@FINALIZADO", SqlDbType.Bit, false);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dadosCardapio.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            dadosCardapio.CardapioEletivaId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        private void AtualizaUltimaAcao(DataContext contexto, DTOs.DadosCardapio dadosCardapio)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Turma.CARDAPIOELETIVA
                                   SET USUARIOID = @USUARIOID,
                                      DATAALTERACAO = @DATAALTERACAO
                                WHERE CARDAPIOELETIVAID = @CARDAPIOELETIVAID ";

            contextQuery.Parameters.Add("@CARDAPIOELETIVAID", SqlDbType.Int, dadosCardapio.CardapioEletivaId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dadosCardapio.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public ValidacaoDados ValidaValidacao(string censo, int ano, int periodo, string curso, int serie, string usuario)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Perfil rnPerfil = new Perfil();
            RN.Turma rnTurma = new Turma();
            RN.Turno rnTurno = new Turno();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE DE ENSINO é obrigatório.");
            }

            if (ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (periodo < 0)
            {
                mensagens.Add("Campo PERIODO é obrigatório.");
            }

            if (curso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CURSO é obrigatório.");
            }

            if (serie <= 0)
            {
                mensagens.Add("Campo SÉRIE é obrigatório.");
            }

            if (usuario.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já foi salva
                    if (!this.ExistePor(contexto, censo, ano, periodo, curso, serie))
                    {
                        mensagens.Add("Este censo / ano / período / curso / série ainda não foi salvo.");
                    }
                    else
                    {
                        //Busca os turnos em que a escola, curso, serie podem ser habilitados
                        var turnos = rnTurma.ListaTurnoPor(contexto, censo, ano, periodo, curso, serie);
                        foreach (string turno in turnos)
                        {
                            //Verifica se foram indicadas 2 opções por turno
                            int quantidade = this.ObtemQuantidadePor(contexto, censo, ano, periodo, curso, serie, turno);
                            if (quantidade < 2)
                            {
                                mensagens.Add(string.Format("Falta(m) {0} eletiva(s) do turno {1}.", (2 - quantidade).ToString(), rnTurno.RetornaDescricaoTurno(turno)));
                            }
                        }
                    }

                    //Verifica se o carpadio ja foi validado
                    if (this.EhValidadoPor(contexto, censo, ano, periodo, curso, serie))
                    {
                        mensagens.Add("Este censo / ano / período / curso / série já foi validado.");
                    }

                    //Verifica se o usuario possui perfil para a alteraçao
                    if (!rnPerfil.PossuiPerfilValidaCardapioPor(contexto, usuario))
                    {
                        mensagens.Add("Este usuário não possui perfil para Validar o cardápio.");
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

        public void SalvaValidacao(string censo, int ano, int periodo, string curso, int serie, string usuario)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Turma.CARDAPIOELETIVA
                                   SET VALIDADO = @VALIDADO,
									  USUARIOVALIDACAO = @USUARIOID,
									  DATAVALIDACAO = @DATAALTERACAO,
									  USUARIOID = @USUARIOID,
                                      DATAALTERACAO = @DATAALTERACAO
                                WHERE CENSO = @CENSO
	                                AND ANO = @ANO 
	                                AND PERIODO = @PERIODO
	                                AND CURSO = @CURSO
	                                AND SERIE = @SERIE ";

                contextQuery.Parameters.Add("@VALIDADO", SqlDbType.Bit, true);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
                contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuario);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                contexto.ApplyModifications(contextQuery);
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

        public ValidacaoDados ValidaFinalizacao(string censo, int ano, int periodo, string curso, int serie, string usuario)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Perfil rnPerfil = new Perfil();
            RN.Turma rnTurma = new Turma();
            RN.Turno rnTurno = new Turno();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE DE ENSINO é obrigatório.");
            }

            if (ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (periodo < 0)
            {
                mensagens.Add("Campo PERIODO é obrigatório.");
            }

            if (curso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CURSO é obrigatório.");
            }

            if (serie <= 0)
            {
                mensagens.Add("Campo SÉRIE é obrigatório.");
            }

            if (usuario.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já foi salva
                    if (!this.ExistePor(contexto, censo, ano, periodo, curso, serie))
                    {
                        mensagens.Add("Este censo / ano / período / curso / série ainda não foi salvo.");
                    }
                    else
                    {
                        //Busca os turnos em que a escola, curso, serie podem ser habilitados
                        var turnos = rnTurma.ListaTurnoPor(contexto, censo, ano, periodo, curso, serie);
                        foreach (string turno in turnos)
                        {
                            //Verifica se foram indicadas 2 opções por turno
                            int quantidade = this.ObtemQuantidadePor(contexto, censo, ano, periodo, curso, serie, turno);
                            if (quantidade < 2)
                            {
                                mensagens.Add(string.Format("Falta(m) {0} eletiva(s) do turno {1}.", (2 - quantidade).ToString(), rnTurno.RetornaDescricaoTurno(turno)));
                            }
                        }
                    }

                    //Verifica se o carpadio ja foi validado
                    if (!this.EhValidadoPor(contexto, censo, ano, periodo, curso, serie))
                    {
                        mensagens.Add("Este censo / ano / período / curso / série ainda não foi validado.");
                    }

                    //Verifica se já foi finalizado
                    if (this.EhFinalizadoPor(contexto, censo, ano, periodo, curso, serie))
                    {
                        mensagens.Add("Este censo / ano / período / curso / série já foi finalizado.");
                    }

                    //Verifica se o usuario possui perfil para a alteraçao
                    if (!rnPerfil.PossuiPerfilFinalizaCardapioPor(contexto, usuario))
                    {
                        mensagens.Add("Este usuário não possui perfil para finalizar o cardápio.");
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

        public void SalvaFinalizacao(string censo, int ano, int periodo, string curso, int serie, string usuario)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Turma.CARDAPIOELETIVA
                                   SET FINALIZADO = @FINALIZADO,
									  USUARIOFINALIZACAO = @USUARIOID,
									  DATAFINALIZACAO = @DATAALTERACAO,
									  USUARIOID = @USUARIOID,
                                      DATAALTERACAO = @DATAALTERACAO
                                WHERE CENSO = @CENSO
	                                AND ANO = @ANO 
	                                AND PERIODO = @PERIODO
	                                AND CURSO = @CURSO
	                                AND SERIE = @SERIE ";

                contextQuery.Parameters.Add("@FINALIZADO", SqlDbType.Bit, true);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
                contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuario);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                contexto.ApplyModifications(contextQuery);
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

        public ValidacaoDados ValidaLimpar(string censo, int ano, int periodo, string curso, int serie, string usuario)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Perfil rnPerfil = new Perfil();
            RN.Matricula rnMatricula = new Matricula();
            RN.AulaDocente rnAulaDocente = new AulaDocente();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE DE ENSINO é obrigatório.");
            }

            if (ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (periodo < 0)
            {
                mensagens.Add("Campo PERIODO é obrigatório.");
            }

            if (curso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CURSO é obrigatório.");
            }

            if (serie <= 0)
            {
                mensagens.Add("Campo SERIE é obrigatório.");
            }

            if (usuario.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já foi salva
                    if (!this.ExistePor(contexto, censo, ano, periodo, curso, serie))
                    {
                        mensagens.Add("Este censo / ano / período / curso / série ainda nao foi salvo.");
                    }

                    //Verifica se o carpadio ja foi validado
                    if (!this.EhValidadoPor(contexto, censo, ano, periodo, curso, serie))
                    {
                        mensagens.Add("Este censo / ano / período / curso / série ainda nao foi validado.");
                    }

                    //Verifica se existem alunos em turmas eletivas do censo, ano, periodo, curso, serie
                    if (rnMatricula.PossuiMatriculaEletiva(contexto, censo, ano, periodo, curso, serie))
                    {
                        mensagens.Add("Este censo / ano / período / curso / série possui alunos enturmados.");
                    }

                    //Verifica se existem professores com alocacao ativa em turmas eletivas do censo, ano, periodo, curso, serie
                    if (rnAulaDocente.PossuiDocentesEmAulaAtivaEletiva(contexto, censo, ano, periodo, curso, serie))
                    {
                        mensagens.Add("Este censo / ano / período / curso / série possui docentes com lotação ativa.");
                    }

                    //Verifica se o usuario possui perfil para a alteraçao
                    if (!rnPerfil.PossuiPerfilFinalizaCardapioPor(contexto, usuario))
                    {
                        mensagens.Add("Este usuario não possui perfil para limpar o cardápio.");
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

        public void SalvaLimpar(string censo, int ano, int periodo, string curso, int serie, string usuario)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.Turma rnTurma = new Turma();

            try
            {
                //Limpa finalização do caradapio
                this.Limpa(contexto, censo, ano, periodo, curso, serie, usuario);

                //Limpa distribuição (disciplina multipla)
                rnTurma.LimpaMultiplaEletiva(contexto, censo, ano, periodo, curso, serie);
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

        private void Limpa(DataContext contexto, string censo, int ano, int periodo, string curso, int serie, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Turma.CARDAPIOELETIVA
                                   SET VALIDADO = @VALIDADO,
									  USUARIOVALIDACAO = null,
									  DATAVALIDACAO = null,
                                      FINALIZADO = @FINALIZADO,
									  USUARIOFINALIZACAO = null,
									  DATAFINALIZACAO = null,
									  USUARIOID = @USUARIOID,
                                      DATAALTERACAO = @DATAALTERACAO
                                WHERE CENSO = @CENSO
	                                AND ANO = @ANO 
	                                AND PERIODO = @PERIODO
	                                AND CURSO = @CURSO
	                                AND SERIE = @SERIE ";

            contextQuery.Parameters.Add("@VALIDADO", SqlDbType.Bit, false);
            contextQuery.Parameters.Add("@FINALIZADO", SqlDbType.Bit, false);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
            contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuario);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public DataTable ListaDisciplinasEletivasPor(decimal ano, decimal semestre, string curso, string turno, int serie, string censo, int grupo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            DataTable dt = null;

            try
            {
                if (grupo == 3)
                {
                    //Para o grupo 3 busca disciplinas adicionadas no cardapio
                    dt = this.ListaDisciplinasCadastradasPor(contexto, ano, semestre, curso, turno, serie, censo);
                }
                else
                {
                    //Para os outros grupos verifica disciplinas multiplas da matriz
                    dt = this.ListaDisciplinasEletivasGrupoPor(contexto, ano, semestre, turno, serie, grupo);
                }
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

        private DataTable ListaDisciplinasCadastradasPor(DataContext contexto, decimal ano, decimal semestre, string curso, string turno, int serie, string censo)
        {
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT CD.DISCIPLINA, D.NOME
                                        FROM Turma.CARDAPIOELETIVA C
                                        INNER JOIN Turma.CARDAPIOELETIVADISCIPLINA CD ON C.CARDAPIOELETIVAID = CD.CARDAPIOELETIVAID
                                        INNER JOIN LY_DISCIPLINA D ON D.DISCIPLINA = CD.DISCIPLINA
                                        WHERE   ANO = @ANO
                                                AND PERIODO = @SEMESTRE
		                                        AND CURSO = @CURSO
		                                        AND TURNO = @TURNO
		                                        AND SERIE = @SERIE
		                                        AND CENSO = @CENSO
		                                        AND GRUPO = 3
		                                        AND FINALIZADO = 1 ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@SERIE", serie);
                contextQuery.Parameters.Add("@CENSO", censo);

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

        private DataTable ListaDisciplinasEletivasGrupoPor(DataContext contexto, decimal ano, decimal semestre, string turno, int serie, int grupo)
        {
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  DISTINCT D2.DISCIPLINA, D2.NOME
                                        FROM    LY_CURRICULO CUR
		                                        INNER JOIN LY_GRADE G ON G.CURSO = CUR.CURSO
                                                                                AND G.SERIE_IDEAL = @SERIE
                                                                                AND G.CURRICULO = CUR.CURRICULO
                                                                                AND G.TURNO = CUR.TURNO
		                                        INNER JOIN LY_DISCIPLINA D ON G.DISCIPLINA = D.DISCIPLINA
		                                        inner join LY_DISCIPLINA_MULTIPLA dm on D.DISCIPLINA = dm.DISCIPLINA
		                                        INNER JOIN LY_DISCIPLINA D2 ON dm.DISCIPLINA_MULTIPLA = D2.DISCIPLINA
                                        WHERE     CUR.SEM_INI = @SEMESTRE
                                                AND CUR.ANO_INI = @ANO
                                                AND CUR.CURSO = '9999.80'
                                                AND CUR.TURNO = @TURNO
                                                AND ( CUR.DT_EXTINCAO IS NULL
                                                        OR CUR.DT_EXTINCAO > GETDATE()
                                                    )
		                                        AND D.GRUPO = @GRUPO ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@SERIE", serie);
                contextQuery.Parameters.Add("@GRUPO", grupo);

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
    }
}
