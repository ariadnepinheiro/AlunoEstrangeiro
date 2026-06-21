using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.AvaliacaoExterna
{
    public class TranscricaoTurma
    {
        public bool PossuiEtapaPor(DataContext contexto, int etapaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(1) 
                                        FROM  AvaliacaoExterna.TRANSCRICAOTURMA (NOLOCK) 
                                        WHERE  ETAPAID = @ETAPAID ";

            contextQuery.Parameters.Add("@ETAPAID", SqlDbType.Int, etapaId);


            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiTranscricaoFinalizadaPor(DataContext contexto, int etapaId, string turma, int ano, int semestre)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(1) 
                                        FROM  AVALIACAOEXTERNA.TRANSCRICAOTURMA (NOLOCK) 
                                        WHERE  ETAPAID = @ETAPAID
										    AND TURMA = @TURMA
										    AND ANO = @ANO
										    AND SEMESTRE = @SEMESTRE
										    AND DATAFINALIZACAO IS NOT NULL ";

            contextQuery.Parameters.Add("@ETAPAID", SqlDbType.Int, etapaId);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, semestre);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void ReabreTranscriacao(DataContext ctx, int etapaId, string turma, int ano, int semestre)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"UPDATE AVALIACAOEXTERNA.TRANSCRICAOTURMA 
                                      SET   DATAFINALIZACAO = null,
	                                        DATAALTERACAO = @DATAALTERACAO
                                      WHERE ETAPAID = @ETAPAID
	                                        AND TURMA = @TURMA
	                                        AND ANO = @ANO
	                                        AND SEMESTRE = @SEMESTRE ";

            contextQuery.Parameters.Add("@ETAPAID", SqlDbType.Int, etapaId);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, semestre);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            ctx.ApplyModifications(contextQuery);
        }

        public int ObtemIdPor(int etapaId, string turma, int ano, int semestre)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.ObtemIdPor(contexto, etapaId, turma, ano, semestre);
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

        private int ObtemIdPor(DataContext contexto, int etapaId, string turma, int ano, int semestre)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;

            try
            {


                contextQuery.Command = @" SELECT TRANSCRICAOTURMAID
                                        FROM [AVALIACAOEXTERNA].[TRANSCRICAOTURMA] (NOLOCK)
                                        WHERE ETAPAID = @ETAPAID
											AND TURMA = @TURMA
											AND ANO = @ANO
											AND SEMESTRE = @SEMESTRE ";

                contextQuery.Parameters.Add("@ETAPAID", SqlDbType.Int, etapaId);
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, semestre);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["TRANSCRICAOTURMAID"]);
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

        public ValidacaoDados Valida(Entidades.TranscricaoTurma transcricao, IList<Entidades.AlunoParticipante> alunos, IList<Entidades.Resposta> respostas)
        {
            RN.AvaliacaoExterna.AlunoParticipante rnAlunoParticipante = new AlunoParticipante();
            RN.AvaliacaoExterna.Etapa rnEtapa = new Etapa();
            RN.AvaliacaoExterna.Resposta rnResposta = new Resposta();
            RN.AvaliacaoExterna.Questao rnQuestao = new Questao();
            RN.AvaliacaoExterna.Entidades.Etapa etapa = new Techne.Lyceum.RN.AvaliacaoExterna.Entidades.Etapa();
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (transcricao == null)
            {
                return validacaoDados;
            }

            if (alunos == null || !alunos.Any())
            {
                mensagens.Add("Transcrição não pode ser salva em uma turma sem alunos.");
            }

            if (respostas == null || !respostas.Any())
            {
                mensagens.Add("Transcrição não pode ser salva em uma turma sem questões.");
            }

            //Valida campos obrigaotorios da transcricao
            if (transcricao.EtapaId <= 0)
            {
                mensagens.Add("Campo ETAPA é obrigatorio.");
            }

            if (transcricao.Ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatorio.");
            }

            if (transcricao.Semestre < 0)
            {
                mensagens.Add("Campo SEMESTRE é obrigatorio.");
            }

            if (transcricao.Turma.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TURMA é obrigatorio.");
            }

            if (transcricao.UsuarioID.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatorio.");
            }

            //Apenas deve ser incluido na lista de alunos para salvar os alunos que possuem alguma informação
            if (alunos.Any(q => q.SituacaoParticipanteId <= 0 && q.DataParticipacao == null))
            {
                mensagens.Add("Existem alunos sem informação.");
            }
            else
            {
                //Valida campos obrigatorios do aluno
                foreach (Entidades.AlunoParticipante aluno in alunos)
                {
                    if (aluno.Aluno.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Existe aluno sem codigo de aluno.");
                    }
                    else
                    {
                        if (aluno.ProvaId <= 0)
                        {
                            mensagens.Add(string.Format("o aluno {0} está sem Codigo da prova.", aluno.Aluno));
                        }

                        if (aluno.ComponenteId <= 0)
                        {
                            mensagens.Add(string.Format("o aluno {0} está sem Codigo da componente.", aluno.Aluno));
                        }

                        if (aluno.SituacaoParticipanteId <= 0 && aluno.DataParticipacao != null)
                        {
                            mensagens.Add(string.Format("Para informar a data de participação do aluno {0} é necessário informar sua situação de participação.", aluno.Aluno));
                        }
                        else
                        {
                            //Verificar se o aluno esta com situacao 1 - PRESENTE
                            if (aluno.SituacaoParticipanteId == 1)
                            {
                                if (aluno.DataParticipacao == null || aluno.DataParticipacao == DateTime.MinValue)
                                {
                                    mensagens.Add(string.Format("A data de participação do aluno {0} é obrigatória pois ele está com situação Presente.", aluno.Aluno));
                                }
                            }
                            else
                            {
                                if (aluno.DataParticipacao != null)
                                {
                                    mensagens.Add(string.Format("A data de participação do aluno {0} apenas pode ser informada caso ele esteja com situação Presente.", aluno.Aluno));
                                }
                            }
                        }

                        if (aluno.UsuarioID.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add(string.Format("A resposta do aluno {0} está sem usuario.", aluno.Aluno));
                        }
                    }
                }

                //Verifica se existe na lista 2x o mesmo aluno / componente / prova
                if (alunos.GroupBy(x => new { x.Aluno, x.ProvaId, x.ComponenteId }).Where(g => g.Count() > 1).Any())
                {
                    mensagens.Add("A lista de alunos possui duplicidade de aluno / componente / prova.");
                }
            }

            //Apenas repostas preenchidas devem ser salvas
            if (respostas.Any(q => q.resposta == null) || respostas.Any(q => q.Aluno.IsNullOrEmptyOrWhiteSpace()))
            {
                mensagens.Add("Existem respostas sem informação.");
            }
            else
            {
                //Valida campos obrigatorios da reposta
                foreach (Entidades.Resposta resposta in respostas)
                {
                    if (resposta.Aluno.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Existe resposta sem codigo de aluno.");
                    }
                    else
                    {
                        if (resposta.QuestaoId <= 0)
                        {
                            mensagens.Add(string.Format("A resposta do aluno {0} está sem Codigo da questão.", resposta.Aluno));
                        }

                        if (resposta.UsuarioID.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add(string.Format("A resposta do aluno {0} está sem usuario.", resposta.Aluno));
                        }
                    }
                }

                //Verifica se existe na lista 2x o mesmo aluno / questao
                if (respostas.GroupBy(x => new { x.Aluno, x.QuestaoId }).Where(g => g.Count() > 1).Any())
                {
                    mensagens.Add("A lista de respostas possui duplicidade de aluno / questao.");
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se a turma está finalizada
                    if (this.TranscricaoFinalizada(contexto, transcricao.EtapaId, transcricao.Turma, transcricao.Ano, transcricao.Semestre))
                    {
                        mensagens.Add("A transcrição não pode ser salva porque já está finalizada.");
                    }
                    else
                    {
                        //Busca dados da etapa da transcricao
                        etapa = rnEtapa.ObtemPor(contexto, transcricao.EtapaId);

                        //Verifica se todos os participantes sao da mesma prova da etapa
                        if (alunos.Where(x => x.ProvaId != etapa.ProvaId).Count() > 0)
                        {
                            mensagens.Add("Existem alunos com codigo de prova incorreto.");
                        }

                        //Busca codigo da transcrição
                        int transcricaoId = this.ObtemIdPor(contexto, transcricao.EtapaId, transcricao.Turma, transcricao.Ano, transcricao.Semestre);

                        //Verifica se não veio codigo da tela
                        if (transcricao.TranscricaoTurmaId <= 0)
                        {
                            transcricao.TranscricaoTurmaId = transcricaoId;
                        }
                        else if (transcricao.TranscricaoTurmaId != transcricaoId)
                        {
                            //Caso venha da tela valida com a informação do banco
                            mensagens.Add("O codigo da transcrição está incorreto.");
                        }

                        foreach (Entidades.AlunoParticipante aluno in alunos)
                        {
                            //Busca aluno participante para o aluno, prova, componenteId
                            int alunoId = rnAlunoParticipante.ObtemIdPor(contexto, aluno.Aluno, aluno.ProvaId, aluno.ComponenteId);

                            //Verifica se não veio codigo da tela
                            if (aluno.AlunoParticipanteId <= 0)
                            {
                                aluno.AlunoParticipanteId = alunoId;
                            }
                            else if (aluno.AlunoParticipanteId != alunoId)
                            {
                                //Caso venha da tela valida com a informação do banco
                                mensagens.Add(string.Format("O codigo do aluno {0} está incorreto.", aluno.Aluno));
                            }

                            ///O campo de Data de Participação deve respeitar os limites de início e fim da realização da prova.                              
                            if (aluno.DataParticipacao != null)
                            {
                                if (Convert.ToDateTime(aluno.DataParticipacao).Date < etapa.InicioRealizacao.Date 
                                    || Convert.ToDateTime(aluno.DataParticipacao).Date > etapa.FimRealizacao.Date)
                                {
                                    mensagens.Add(string.Format("A data de participação do aluno {0} deve estar entre o inicio ({1}) e o fim ({2}) da realização da prova.", aluno.Aluno, etapa.InicioRealizacao.ToString("dd/MM/yyyy"), etapa.FimRealizacao.ToString("dd/MM/yyyy")));
                                }
                            }
                        }

                        foreach (Entidades.Resposta resposta in respostas)
                        {
                            //Busca aluno participante para o aluno, prova, componenteId
                            int respostaId = rnResposta.ObtemIdPor(contexto, resposta.Aluno, resposta.QuestaoId);

                            //Verifica se não veio codigo da tela
                            if (resposta.RespostaId <= 0)
                            {
                                resposta.RespostaId = respostaId;
                            }
                            else if (resposta.RespostaId != respostaId)
                            {
                                //Caso venha da tela valida com a informação do banco
                                mensagens.Add(string.Format("O codigo da resposta do aluno {0} está incorreto.", resposta.Aluno));
                            }

                            //Busca codigo da prova da questao
                            int provaId = rnQuestao.RetornaProvaIdPor(contexto, resposta.QuestaoId);

                            //Verifica se todos as respostas sao da mesma prova da etapa
                            if (etapa.ProvaId != provaId)
                            {
                                mensagens.Add("Existem respostas com codigo de prova incorreto.");
                            }

                            //Busca componente da questao
                            int componenteId = rnQuestao.RetornaConponentePor(contexto, resposta.QuestaoId);

                            //Busca dados da participacao do eluno no componente
                            Entidades.AlunoParticipante aluno = alunos.Where(x => x.Aluno == resposta.Aluno && x.ComponenteId == componenteId).FirstOrDefault();

                            if (aluno == null || aluno.Aluno.IsNullOrEmptyOrWhiteSpace())
                            {
                                mensagens.Add(string.Format("Para lançar a resposta do aluno {0} será necessários informar sua situação e data de particpação no componente da questão.", resposta.Aluno));
                            }
                            else
                            {
                                //Verificar se o aluno não esta com situacao 1 - PRESENTE
                                if (aluno.SituacaoParticipanteId != 1 && (respostas.Any(x => x.Aluno == aluno.Aluno) && aluno.ComponenteId == componenteId))
                                {
                                    mensagens.Add(string.Format("As respostas do aluno {0} apenas podem ser informadas caso ele esteja com situação Presente no componente.", aluno.Aluno));
                                }
                            }
                        }
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

        private void LimpaAnteriores(DataContext ctx, List<string> alunosTotal, int provaId)
        {
            if (!alunosTotal.Any())
                return;

            ContextQuery contextQuery = new ContextQuery();

            var alunosTotalFormatado = "'" + alunosTotal.Aggregate((a, b) => a + "', '" + b) + "'";
            contextQuery.Command = String.Format(@"
                DELETE FROM AvaliacaoExterna.ALUNOPARTICIPANTE WHERE ALUNO IN ({0}) AND PROVAID = @PROVAID;
                DELETE FROM AvaliacaoExterna.RESPOSTA WHERE ALUNO IN ({0}) AND QUESTAOID IN (SELECT QUESTAOID FROM AvaliacaoExterna.QUESTAO WHERE PROVAID = @PROVAID);
            ", alunosTotalFormatado);
            contextQuery.Parameters.Add("@PROVAID", SqlDbType.VarChar, provaId);
            ctx.ApplyModifications(contextQuery);
        }

        public void Salva(Entidades.TranscricaoTurma transcricao, IList<Entidades.AlunoParticipante> alunos, IList<Entidades.Resposta> respostas, List<string> alunosTotal)
        {
            RN.AvaliacaoExterna.AlunoParticipante rnAlunoParticipante = new AlunoParticipante();
            RN.AvaliacaoExterna.Resposta rnResposta = new Resposta();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Salva transcrição
                this.SalvaTranscricao(ctx, transcricao);

                //Limpa as informaçoes anteriores
                var provaId = alunos.First().ProvaId; //tem que verificar o que fazer se não tiver alunos na array
                this.LimpaAnteriores(ctx, alunosTotal, provaId);

                //Salva participantes
                foreach (Entidades.AlunoParticipante a in alunos)
                    rnAlunoParticipante.SalvaAlunoParticipante(ctx, a);

                //Salva respostas
                foreach (Entidades.Resposta r in respostas)
                    rnResposta.SalvaResposta(ctx, r);
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

        public ValidacaoDados ValidaFinalizacao(Entidades.TranscricaoTurma transcricao, IList<Entidades.AlunoParticipante> alunos, IList<Entidades.Resposta> respostas)
        {
            RN.AvaliacaoExterna.AlunoParticipante rnAlunoParticipante = new AlunoParticipante();
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.AvaliacaoExterna.Etapa rnEtapa = new Etapa();
            RN.AvaliacaoExterna.Questao rnQuestao = new Questao();
            RN.AvaliacaoExterna.Componente rnComponente = new Componente();
            RN.AvaliacaoExterna.Entidades.Etapa etapa = new Techne.Lyceum.RN.AvaliacaoExterna.Entidades.Etapa();
            IList<RN.AvaliacaoExterna.Entidades.Questao> questoes = new List<Techne.Lyceum.RN.AvaliacaoExterna.Entidades.Questao>();
            IList<RN.AvaliacaoExterna.Entidades.Componente> componentes = new List<RN.AvaliacaoExterna.Entidades.Componente>();
            DataTable alunosBase = new DataTable();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (transcricao == null)
            {
                return validacaoDados;
            }

            //Chama metodo que faz as validaçoes comuns
            validacaoDados = this.Valida(transcricao, alunos, respostas);

            //Caso passe das validões comuns, fazer validaçoes especificas para finalizar
            validacaoDados.Valido = false;
            if (mensagens.Count == 0)
            {
                if (alunos.Any(q => q.SituacaoParticipanteId == 0))
                {
                    mensagens.Add("A finalização só pode ser feita se todos os alunos estiverem com o campo SITUAÇÃO PARTICIPANTE preenchido.");
                }

                if (alunos.Any(q => new int[] { 1, 5 }.Contains(q.SituacaoParticipanteId) && !q.DataParticipacao.HasValue))
                {
                    mensagens.Add("A finalização só pode ser feita se todos os alunos PRESENTES estiverem com o campo DATA DE PARTICIPAÇÃO preenchido.");
                }

                //return new ValidacaoDados { Valido = true };
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Busca dados da etapa da transcricao
                    etapa = rnEtapa.ObtemPor(transcricao.EtapaId);

                    //Busca todos os alunos da Turma
                    //Verifica se eh o ano atual
                    if (transcricao.Ano == DateTime.Now.Year)
                    {
                        //Para ano atual busca na matricula
                        alunosBase = rnAlunoParticipante.ListaAlunoMatriculaPor(contexto, etapa.ProvaId, transcricao.Turma, transcricao.Semestre, transcricao.Ano);
                    }
                    else
                    {
                        //Para anos anteriores busca na historico
                        alunosBase = rnAlunoParticipante.ListaAlunoHistoricoPor(contexto, etapa.ProvaId, transcricao.Turma, transcricao.Semestre, transcricao.Ano);
                    }

                    //Busca todas as questoes da prova
                    questoes = rnQuestao.CarregaQuestoes(contexto, etapa.ProvaId);

                    //Busca todos os componentes da prova
                    componentes = rnComponente.CarregaPorProva(contexto, etapa.ProvaId);

                    //Para finalizar todos os alunos da turma precisam ter situacao em todos os componentes da prova
                    foreach (DataRow item in alunosBase.Rows)
                    {
                        string aluno = Convert.ToString(item["ALUNO"]);

                        //Verifica se tem registro para todos os componentes da prova
                        foreach (Entidades.Componente componente in componentes)
                        {
                            if (!alunos.Any(x => x.Aluno == aluno && x.ComponenteId == componente.ComponenteId))
                            {
                                mensagens.Add(string.Format("O Aluno {0} não possui situação para o componente {1}.", aluno, componente.Descricao));
                            }
                        }

                        //para finalizar todos os alunos da turma precisam ter resposta em todos os questoes da prova
                        foreach (Entidades.Questao questao in questoes)
                        {
                            //Busca componente da questao
                            int componenteId = rnQuestao.RetornaConponentePor(contexto, questao.QuestaoId);

                            //Busca dados da participacao do eluno no componente
                            Entidades.AlunoParticipante alunoParticipante = alunos.Where(x => x.Aluno == aluno && x.ComponenteId == componenteId).First();

                            //Verifica se o aluno estava presente
                            if (alunoParticipante.SituacaoParticipanteId == 1)
                            {
                                if (!respostas.Any(x => x.Aluno == aluno && x.QuestaoId == questao.QuestaoId))
                                {
                                    mensagens.Add(string.Format("O Aluno {0} não possui resposta para a questao {1}.", aluno, questao.Numero));
                                }
                            }
                        }
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

        private void SalvaTranscricao(DataContext ctx, Entidades.TranscricaoTurma transcricao)
        {
            //Verifica se já existe
            if (transcricao.TranscricaoTurmaId > 0)
            {
                this.Atualiza(ctx, transcricao);
            }
            else
            {
                this.Insere(ctx, transcricao);
            }
        }

        private void Insere(DataContext contexto, Entidades.TranscricaoTurma transcricao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO AvaliacaoExterna.TRANSCRICAOTURMA
                                           (ETAPAID
                                           ,ANO
                                           ,SEMESTRE
                                           ,TURMA
                                           ,DATAINICIO
                                           ,DATAFINALIZACAO
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@ETAPAID, 
                                           @ANO, 
                                           @SEMESTRE, 
                                           @TURMA, 
                                           @DATAINICIO, 
                                           @DATAFINALIZACAO, 
                                           @USUARIOID, 
                                           @DATACADASTRO, 
                                           @DATAALTERACAO)

                                SELECT IDENT_CURRENT('AvaliacaoExterna.TRANSCRICAOTURMA')  ";

            contextQuery.Parameters.Add("@ETAPAID", SqlDbType.Int, transcricao.EtapaId);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, transcricao.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, transcricao.Semestre);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, transcricao.Turma);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAFINALIZACAO", SqlDbType.DateTime, transcricao.DataFinalizacao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, transcricao.UsuarioID);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            transcricao.TranscricaoTurmaId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        private void Atualiza(DataContext contexto, Entidades.TranscricaoTurma transcricao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE AvaliacaoExterna.TRANSCRICAOTURMA 
                                    SET DATAFINALIZACAO = @DATAFINALIZACAO,
	                                    USUARIOID = @USUARIOID,
	                                    DATAALTERACAO = @DATAALTERACAO
                                    WHERE TRANSCRICAOTURMAID = @TRANSCRICAOTURMAID ";

            contextQuery.Parameters.Add("@TRANSCRICAOTURMAID", SqlDbType.Int, transcricao.TranscricaoTurmaId);
            contextQuery.Parameters.Add("@DATAFINALIZACAO", SqlDbType.DateTime, transcricao.DataFinalizacao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, transcricao.UsuarioID);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        //        public bool TranscricaoExiste(DataContext ctx, int etapaId, string turma)
        //        {
        //            if (ctx == null)
        //                ctx = DataContextBuilder.FromLyceum.UsingNoLock();

        //            ContextQuery contextQuery = new ContextQuery();

        //            contextQuery.Command = @" SELECT COUNT(0) 
        //                                        FROM AVALIACAOEXTERNA.TRANSCRICAOTURMA
        //                                        WHERE ETAPAID = @ETAPAID 
        //	                                        AND TURMA = @TURMA
        //	                                        AND ANO = @ANO
        //	                                        AND SEMESTRE = @SEMESTRE ";

        //            contextQuery.Parameters.Add("@ETAPAID", SqlDbType.Int, etapaId);
        //            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
        //            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, etapaId);
        //            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, etapaId);

        //            return ctx.GetReturnValue<int>(contextQuery) > 0;
        //        }

        public bool TranscricaoExiste(int etapaId, string turma, int ano, int semestre)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(0) 
                                        FROM AVALIACAOEXTERNA.TRANSCRICAOTURMA
                                        WHERE ETAPAID = @ETAPAID 
	                                        AND TURMA = @TURMA
	                                        AND ANO = @ANO
	                                        AND SEMESTRE = @SEMESTRE ";

                contextQuery.Parameters.Add("@ETAPAID", SqlDbType.Int, etapaId);
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, semestre);

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

        public bool TranscricaoFinalizada(int etapaId, string turma, int ano, int semestre)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.TranscricaoFinalizada(contexto, etapaId, turma, ano, semestre);
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

        private bool TranscricaoFinalizada(DataContext ctx, int etapaId, string turma, int ano, int semestre)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT COUNT(0) 
                                        FROM AVALIACAOEXTERNA.TRANSCRICAOTURMA 
                                        WHERE ETAPAID = @ETAPAID 
	                                        AND TURMA = @TURMA 
	                                        AND ANO = @ANO
	                                        AND SEMESTRE = @SEMESTRE
	                                        AND DATAFINALIZACAO IS NOT NULL ";

            contextQuery.Parameters.Add("@ETAPAID", SqlDbType.Int, etapaId);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, semestre);

            return ctx.GetReturnValue<int>(contextQuery) > 0;
        }
    }
}
