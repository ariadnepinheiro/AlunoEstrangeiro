using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.DTOs;
using System.Data.SqlClient;
using System.Data;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN
{
    public class HCursosConcl : RNBase
    {
        public ValidacaoDados ValidaReabertura(DadosReabertura dadosReabertura, decimal pessoa)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            int idade = 0;
            RN.Aluno rnAluno = new RN.Aluno();
            RN.EncerramentoAluno rnEncerramentoAluno = new RN.EncerramentoAluno();
            RN.RestricaoIdadeSerie rnRestricaoIdadeSerie = new RestricaoIdadeSerie();
            Aluno.DadosAluno dadosAluno = new Aluno.DadosAluno();
            RN.ControleVaga rnControleVaga = new ControleVaga();
            RN.Matriculas.PessoaAluno rnPessoaAluno = new Techne.Lyceum.RN.Matriculas.PessoaAluno();
            TceRestricaoIdadeSerie restricao = new TceRestricaoIdadeSerie();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosReabertura == null)
            {
                return validacaoDados;
            }

            //Campos obrigatorios da tela
            if (dadosReabertura.Aluno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo ALUNO é de preenchimento obrigatório.");
            }

            if (dadosReabertura.MotivoEncerramento.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo MOTIVO DE ENCERRAMENTO é de preenchimento obrigatório.");
            }
            else if (dadosReabertura.MotivoReabertura.IsNullOrEmptyOrWhiteSpace() && dadosReabertura.MotivoEncerramento == "PROV_DES")
            {
                mensagens.Add("O campo MOTIVO DE REABERTURA é de preenchimento obrigatório para encerramento como PROVÁVEL DESISTÊNCIA.");
            }

            if (dadosReabertura.DataReabertura == DateTime.MinValue)
            {
                mensagens.Add("O campo DATA DE REABERTURA é de preenchimento obrigatório.");
            }
            else if (dadosReabertura.DataReabertura.Date > DateTime.Now.Date)
            {
                mensagens.Add("O campo DATA DE REABERTURA não pode ser maior que a data atual.");
            }

            if (dadosReabertura.AnoReabertura <= 0)
            {
                mensagens.Add("O campo ANO DE REABERTURA é de preenchimento obrigatório.");
            }

            if (dadosReabertura.PeriodoReabertura < 0)
            {
                mensagens.Add("O campo PERÍODO DE REABERTURA é de preenchimento obrigatório.");
            }

            if (dadosReabertura.CursoReabertura.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo CURSO DE REABERTURA é de preenchimento obrigatório.");
            }

            if (dadosReabertura.TurnoReabertura.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo TURNO DE REABERTURA é de preenchimento obrigatório.");
            }

            if (dadosReabertura.SerieReabertura <= 0)
            {
                mensagens.Add("O campo SERIE DE REABERTURA é de preenchimento obrigatório.");
            }

            //Campos obrigatorios alimentados internamente pela tela
            if (dadosReabertura.DataNascimento == DateTime.MinValue)
            {
                mensagens.Add("O campo DATA DE NASCIMENTO é de preenchimento obrigatório.");
            }

            if (dadosReabertura.DataEncerramento == DateTime.MinValue)
            {
                mensagens.Add("O campo DATA DE ENCERRAMENTO é de preenchimento obrigatório.");
            }
            else if (dadosReabertura.DataEncerramento != DateTime.MinValue)
            {
                if (dadosReabertura.DataReabertura.Date <= dadosReabertura.DataEncerramento.Date)
                {
                    mensagens.Add("O campo DATA DE REABERTURA não pode ser menor ou igual a DATA DE ENCERRAMENTO.");
                }
            }

            if (dadosReabertura.AnoEncerramento <= 0)
            {
                mensagens.Add("O campo ANO DE ENCERRAMENTO é de preenchimento obrigatório.");
            }

            if (dadosReabertura.PeriodoEncerramento < 0)
            {
                mensagens.Add("O campo PERÍODO DE ENCERRAMENTO é de preenchimento obrigatório.");
            }

            if (dadosReabertura.CurriculoReabertura.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("MATRIZ CURRICULAR DE REABERTURA não identificada.");
            }

            if (dadosReabertura.CursoAtual.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo CURSO ATUAL DO ALUNO é de preenchimento obrigatório.");
            }

            if (dadosReabertura.TurnoAtual.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo TURNO ATUAL DO ALUNO é de preenchimento obrigatório.");
            }

            if (dadosReabertura.SerieAtual <= 0)
            {
                mensagens.Add("O campo SERIE ATUAL DO ALUNO é de preenchimento obrigatório.");
            }

            if (dadosReabertura.CurriculoAtual.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("MATRIZ CURRICULAR ATUAL DO ALUNO não identificada.");
            }

            if (dadosReabertura.TipoVaga.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo TIPO VAGA é de preenchimento obrigatório.");
            }

            if (dadosReabertura.UnidadeEnsino.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo UNIDADE DE ENSINO é de preenchimento obrigatório.");
            }

            if (dadosReabertura.UsuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo USUARIO RESPONSAVEL é de preenchimento obrigatório.");
            }

            if (mensagens.Count == 0)
            {

                //Verifica se unidade/ano/periodo/curso/serie/turno está participando da 3ºª fase
                if (rnControleVaga.PartipaMatriculaFacilPor(dadosReabertura.UnidadeEnsino, Convert.ToInt32(dadosReabertura.AnoReabertura), Convert.ToInt32(dadosReabertura.PeriodoReabertura), dadosReabertura.CursoReabertura, Convert.ToInt32(dadosReabertura.SerieReabertura), dadosReabertura.TurnoReabertura))
                {
                    mensagens.Add("Não será possível realizar a reabertura, pois o curso/série está participando do Matrícula Fácil.");
                }

                string matricula = rnPessoaAluno.ObtemOutraPessoaAlunoPor(pessoa, dadosReabertura.Aluno);

                if (!matricula.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Não será possível realizar a reabertura, pois a matrícula de número " + matricula + " consta como correta do aluno.");
                }

                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Busca dados do aluno
                    dadosAluno = rnAluno.ObtemDadosAluno(dadosReabertura.Aluno);

                    //Verifica se existe outro aluno ativo com mesmo nome / mae / data Nascimento
                    if (rnAluno.PossuiOutroAlunoAtivoPor(contexto, dadosAluno.Nome_compl, dadosAluno.NomeMae, dadosReabertura.DataNascimento, dadosReabertura.Aluno))
                    {
                        mensagens.Add("Não será possível realizar a reabertura, pois já existe outro aluno ativo com mesmo nome/data de nascimento/nome da mãe.");
                    }

                    //Verifica se existe outro aluno ativo com mesmo cpf
                    if (rnAluno.PossuiOutroCPFAtivoPor(contexto, dadosAluno.Cpf, dadosReabertura.Aluno))
                    {
                        mensagens.Add("Não será possível realizar a reabertura, pois já existe outro aluno ativo com mesmo cpf.");
                    }

                    //Verifica idade do aluno                                        
                    idade = Utils.CalcularIdade(dadosReabertura.DataNascimento);
                    restricao = rnRestricaoIdadeSerie.CarregaRestricaoPor(contexto, dadosReabertura.CursoReabertura, dadosReabertura.SerieReabertura);

                    //Verifica se o aluno possui necessidade especial
                    if (!rnAluno.PossuiNecessidadeEspecialPor(contexto, dadosReabertura.Aluno))
                    {
                        //Para Alunos sem necessidades Especiais Verificar restrição de idade minima e maxima
                        if (idade < restricao.IdadeMinima || idade > restricao.IdadeMaxima)
                        {
                            mensagens.Add(string.Format("Para o curso selecionado é permitido cadastrar alunos entre {0} e {1} anos. Favor verificar a DATA DE NASCIMENTO!",
                                restricao.IdadeMinima,
                                restricao.IdadeMaxima));
                        }
                    }
                    else
                    {
                        //Para Alunos com necessidades Especiais Verificar restrição de idade minima
                        if (idade < restricao.IdadeMinima)
                        {
                            mensagens.Add(string.Format("Para o curso selecionado não é permitido cadastrar alunos com necessidade especial com menos de {0} anos. Favor verificar a DATA DE NASCIMENTO!",
                                 restricao.IdadeMinima));
                        }
                    }

                    int vagasLiberadas = 0;
                    int vagasUtilizadas = 0;

                    //Verificar se tem vaga no curso / serie / turno / ano / semestre
                    vagasLiberadas = rnControleVaga.ObtemVagasLiberadasTotalPor(contexto,
                        dadosReabertura.UnidadeEnsino,
                        dadosReabertura.AnoReabertura,
                        dadosReabertura.PeriodoReabertura,
                        dadosReabertura.SerieReabertura,
                        dadosReabertura.CursoReabertura,
                        dadosReabertura.TurnoReabertura);

                    vagasUtilizadas = rnControleVaga.ObtemVagasUtilizadasTotalPor(contexto,
                        dadosReabertura.UnidadeEnsino,
                        dadosReabertura.AnoReabertura,
                        dadosReabertura.PeriodoReabertura,
                        dadosReabertura.SerieReabertura,
                        dadosReabertura.CursoReabertura,
                        dadosReabertura.TurnoReabertura);

                    if (vagasLiberadas <= vagasUtilizadas)
                    {
                        mensagens.Add("Não será possível realizar a reabertura, pois não existem vagas disponíveis para o curso/série e turno!");
                    }

                    //Verificar se existe linha com os dados para ser reaberta                    
                    if (!rnEncerramentoAluno.PossuiEncerramentoParaReabertura(contexto, dadosReabertura.Aluno))
                    {
                        mensagens.Add("Não será possível realizar a reabertura, pois não foi encontrado um registro de encerramento com os dados necessários.");
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

        public void ReabreAlunoPreCadastro(DataContext contexto, string aluno, int ano, int periodo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_H_CURSOS_CONCL
	                                    SET DT_REABERTURA = @DATAATUAL,
		                                    MOTIVOREABERTURA = @MOTIVOREABERTURA,
		                                    DT_ULTALT = @DATAATUAL,
		                                    OBSERVACAO = @OBSERVACAO
	                                    FROM LY_H_CURSOS_CONCL C
	                                    WHERE C.DT_REABERTURA IS NULL
			                                    AND ALUNO = @ALUNO  ";

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
            contextQuery.Parameters.Add("@DATAATUAL", TechneDbType.T_DATA, DateTime.Now);
            contextQuery.Parameters.Add("@MOTIVOREABERTURA", SqlDbType.VarChar, "Reabertura pela confirmação de Inscrição");
            contextQuery.Parameters.Add("@OBSERVACAO", TechneDbType.T_ALFAEXTRALARGE, string.Format("Reabertura {0}/{1}", ano.ToString(), periodo.ToString()));

            contexto.ApplyModifications(contextQuery);
        }

        public void ReabreAlunoDuplicidade(DataContext contexto, string aluno, int ano, int periodo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_H_CURSOS_CONCL
	                                    SET DT_REABERTURA = @DATAATUAL,
		                                    MOTIVOREABERTURA = @MOTIVOREABERTURA,
		                                    DT_ULTALT = @DATAATUAL,
		                                    OBSERVACAO = @OBSERVACAO
	                                    FROM LY_H_CURSOS_CONCL C
	                                    WHERE C.DT_REABERTURA IS NULL
			                                    AND ALUNO = @ALUNO  ";

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
            contextQuery.Parameters.Add("@DATAATUAL", TechneDbType.T_DATA, DateTime.Now);
            contextQuery.Parameters.Add("@MOTIVOREABERTURA", SqlDbType.VarChar, "Reabertura por tratamento de duplicidade");
            contextQuery.Parameters.Add("@OBSERVACAO", TechneDbType.T_ALFAEXTRALARGE, string.Format("Reabertura {0}/{1}", ano.ToString(), periodo.ToString()));

            contexto.ApplyModifications(contextQuery);
        }

        public void Reabre(DadosReabertura dadosReabertura)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.Aluno rnAluno = new Aluno();
            RN.Carteirinha rnCarteirinha = new Carteirinha();
            RN.Matricula rnMatricula = new Matricula();
            RN.GradeTurma rnGradeTurma = new GradeTurma();
            DateTime dataHoraReabertura;
            string observacao = string.Empty;
            bool utilizaTransporte = false;
            TceConfirmacaoMatricula confirmacaoMatricula = new TceConfirmacaoMatricula();
            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            RN.Matgrade rnMatgrade = new Matgrade();
            bool matriculaAtivada = false;
            string turma = string.Empty;
            decimal gradeId = 0;

            try
            {
                //Monta data reabertura com hora atual, e observacao
                dataHoraReabertura = new DateTime(dadosReabertura.DataReabertura.Year, dadosReabertura.DataReabertura.Month, dadosReabertura.DataReabertura.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                observacao = "Reabertura " + dadosReabertura.AnoReabertura.ToString() + "/" + dadosReabertura.PeriodoReabertura.ToString();

                //Atualizar dados do aluno, curso / turno / serie / curriculo / situação
                rnAluno.AtivaAtualizaAlunoPor(contexto, dadosReabertura.CursoReabertura, dadosReabertura.TurnoReabertura, dadosReabertura.CurriculoReabertura, dadosReabertura.SerieReabertura, dadosReabertura.Aluno);

                //Atualiza dados da reabertura de acordo com aluno e dados do encerramento
                this.AtualizaDadosReaberturaEspecial(contexto, dataHoraReabertura, dadosReabertura.MotivoReabertura, DateTime.Now, observacao, dadosReabertura.Aluno);

                //Verifica se possui marcação no campo gratuidade
                utilizaTransporte = Aluno.ExisteUtilizaTransporte(contexto, dadosReabertura.Aluno);

                //Caso exista marcação no campo gratuidade desmarcar
                if (utilizaTransporte)
                {
                    rnAluno.RetiraUsoGratuidade(contexto, dadosReabertura.Aluno);
                }

                if (rnCarteirinha.PossuiCarteirinhaPor(contexto, dadosReabertura.Aluno))
                {
                    rnCarteirinha.AtivaCarteirinha(contexto, dadosReabertura.Aluno, dataHoraReabertura);
                }

                //Fernanda Reina: o restante já tem que validar no curso novo
                turma = rnMatricula.ObtemTurmaPrincipalCanceladaPor(contexto, dadosReabertura.Aluno, dadosReabertura.AnoReabertura, dadosReabertura.PeriodoReabertura, dadosReabertura.CursoReabertura, dadosReabertura.TurnoReabertura, dadosReabertura.CurriculoReabertura, dadosReabertura.SerieReabertura, dadosReabertura.UnidadeEnsino);

                if (!turma.IsNullOrEmptyOrWhiteSpace())
                {
                    int vagas = RN.Turma.RetornaVagas(dadosReabertura.AnoReabertura, dadosReabertura.PeriodoReabertura, turma);

                    if (vagas > 0)
                    {
                        rnMatricula.ReativaMatriculaPrincipal(contexto, dadosReabertura.Aluno, dataHoraReabertura, dadosReabertura.AnoReabertura, dadosReabertura.PeriodoReabertura, turma);

                        if (rnMatricula.PossuiMatriculaAtivaNaTurmaPorAluno(contexto, dadosReabertura.Aluno, turma, dadosReabertura.AnoReabertura, dadosReabertura.PeriodoReabertura))
                        {
                            matriculaAtivada = true;

                            //Busca gradeId
                            gradeId = Convert.ToDecimal(rnGradeTurma.ObterGradeId(contexto, dadosReabertura.AnoReabertura, dadosReabertura.PeriodoReabertura, turma));

                            if (gradeId > 0)
                            {
                                //Insere matgrade e Cancela demais
                                rnMatgrade.InsereMatgradePrincipal(contexto, dadosReabertura.Aluno, gradeId);
                            }
                        }
                    }
                }

                //Monta Confirmação de Matricula para Inclusao/Alteração
                confirmacaoMatricula = new TceConfirmacaoMatricula
                {
                    Aluno = dadosReabertura.Aluno,
                    Censo = dadosReabertura.UnidadeEnsino,
                    Ano = dadosReabertura.AnoReabertura,
                    Periodo = dadosReabertura.PeriodoReabertura,
                    Curso = dadosReabertura.CursoReabertura,
                    Turno = dadosReabertura.TurnoReabertura,
                    Curriculo = dadosReabertura.CurriculoReabertura,
                    Serie = dadosReabertura.SerieReabertura,
                    EnsinoReligioso = dadosReabertura.EnsinoReligioso,
                    LinguaEstrangeiraFacultativa = dadosReabertura.LinguaEstrangeira,
                    ProjetoAutonomia = false,
                    Matricula = dadosReabertura.UsuarioResponsavel,
                    TipoVagaOcupada = dadosReabertura.TipoVaga,
                    Status = matriculaAtivada ? ConfirmacaoMatricula.Confirmado : ConfirmacaoMatricula.Pendente
                };

                //Atualiza todas as possiveis renovações ativas ou pendentes do aluno para não confirmado
                rnConfirmacaoMatricula.AtualizaStatusNaoConfirmadoPor(contexto, confirmacaoMatricula.Aluno, Convert.ToInt32(confirmacaoMatricula.Ano), Convert.ToInt32(confirmacaoMatricula.Periodo), confirmacaoMatricula.Matricula);

                int idConfirmacao = rnConfirmacaoMatricula.ObtemIdConfirmacaoMatriculaPor(confirmacaoMatricula.Aluno, confirmacaoMatricula.Ano, confirmacaoMatricula.Periodo, confirmacaoMatricula.Censo, confirmacaoMatricula.Curso, confirmacaoMatricula.Serie, confirmacaoMatricula.Turno);

                if (idConfirmacao > 0)
                {
                    confirmacaoMatricula.IdConfirmacaoMatricula = idConfirmacao;
                    rnConfirmacaoMatricula.Atualiza(contexto, confirmacaoMatricula);
                }
                else
                {
                    RN.ConfirmacaoMatricula.Inserir(confirmacaoMatricula, contexto);
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

        public DTOs.DadosEncerramentoAluno ObtemDadosAlunoEncerramentoPor(string aluno)
        {
            DTOs.DadosEncerramentoAluno dados = new DTOs.DadosEncerramentoAluno();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"   SELECT A.ALUNO,
                                                   H.ANO_ENCERRAMENTO,
                                                   H.SEM_ENCERRAMENTO,
                                                   A.CURSO,
                                                   C.NOME       AS NOME_CURSO,
                                                   A.TURNO,
                                                   T.DESCRICAO AS NOME_TURNO,
                                                   A.CURRICULO,
                                                   A.UNIDADE_ENSINO,
                                                   UE.NOME_COMP AS NOME_ESCOLA,
                                                   A.SERIE,
                                                   S.DESCRICAO  AS NOME_SERIE,
                                                   H.DT_ENCERRAMENTO,
                                                   H.MOTIVO,
                                                   MS.DESCRICAO AS DESC_MOTIVO,
                                                   A.SIT_ALUNO
                                            FROM   LY_H_CURSOS_CONCL H (NOLOCK)
                                                   INNER JOIN LY_ALUNO A (NOLOCK)
                                                           ON H.ALUNO = A.ALUNO
                                                   INNER JOIN LY_CURSO C
                                                           ON C.CURSO = A.CURSO
                                                   INNER JOIN LY_SERIE S
                                                           ON S.SERIE = A.SERIE AND C.CURSO=S.CURSO AND S.CURRICULO=A.CURRICULO
                                                   INNER JOIN LY_UNIDADE_ENSINO UE
                                                           ON UE.UNIDADE_ENS = A.UNIDADE_ENSINO
                                                   INNER JOIN LY_TURNO T 
                                                           ON T.TURNO = A.TURNO
                                                   INNER JOIN LY_MOTIVOSAIDA MS ON H.MOTIVO = MS.MOTIVOSAIDA
                                            WHERE  A.ALUNO = @ALUNO    
                                            AND H.DT_REABERTURA IS NULL";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dados.Aluno = Convert.ToString(reader["ALUNO"]);
                    dados.Ano = Convert.ToInt32(reader["ANO_ENCERRAMENTO"]);
                    dados.Periodo = Convert.ToInt32(reader["SEM_ENCERRAMENTO"]);
                    dados.CursoAtual = Convert.ToString(reader["CURSO"]);
                    dados.NomeCursoAtual = Convert.ToString(reader["NOME_CURSO"]);
                    dados.TurnoAtual = Convert.ToString(reader["TURNO"]);
                    dados.NomeTurnoAtual = Convert.ToString(reader["NOME_TURNO"]);
                    dados.CurriculoAtual = Convert.ToString(reader["CURRICULO"]);
                    dados.UnidadeEnsinoAtual = Convert.ToString(reader["UNIDADE_ENSINO"]);
                    dados.NomeUnidadeEnsinoAtual = Convert.ToString(reader["NOME_ESCOLA"]);
                    dados.SerieAtual = Convert.ToDecimal(reader["SERIE"]);
                    dados.NomeSerieAtual = Convert.ToString(reader["NOME_SERIE"]);
                    dados.Situacao = Convert.ToString(reader["SIT_ALUNO"]);
                    dados.Motivo = Convert.ToString(reader["MOTIVO"]);
                    dados.MotivoDescricao = Convert.ToString(reader["DESC_MOTIVO"]);
                    if (reader["DT_ENCERRAMENTO"] != DBNull.Value)
                    {
                        dados.DtEncerramento = Convert.ToDateTime(reader["DT_ENCERRAMENTO"]);
                    }

                }

                return dados;
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

        private void AtualizaDadosReaberturaEspecial(DataContext contexto, DateTime dataReabertura, string motivo, DateTime dataAtualizacao, string observacao, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_H_CURSOS_CONCL 
                                SET    DT_REABERTURA = @DT_REABERTURA,
                                       MOTIVOREABERTURA = @MOTIVOREABERTURA, 
                                       DT_ULTALT = @DT_ULTALT, 
                                       OBSERVACAO = @OBSERVACAO 
                                WHERE  ALUNO = @ALUNO 
                                       AND DT_REABERTURA IS NULL  ";

            contextQuery.Parameters.Add("@DT_REABERTURA", TechneDbType.T_DATA, dataReabertura);
            contextQuery.Parameters.Add("@MOTIVOREABERTURA", SqlDbType.VarChar, motivo);
            contextQuery.Parameters.Add("@DT_ULTALT", TechneDbType.T_DATA, dataAtualizacao);
            contextQuery.Parameters.Add("@OBSERVACAO", TechneDbType.T_ALFAEXTRALARGE, observacao);
            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);

            contexto.ApplyModifications(contextQuery);
        }

        public bool ExistePor(DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @"  SELECT COUNT(*) 
                                FROM LY_H_CURSOS_CONCL (NOLOCK)
                                WHERE ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", aluno);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public void EncerraAlunoPessoaDuplicadaPor(DataContext contexto, string matricula, decimal semestre)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO LY_H_CURSOS_CONCL
                                                ( [CURSO]
                                                ,[TURNO]
                                                ,[CURRICULO]
                                                ,[ALUNO]
                                                ,[DT_ENCERRAMENTO]
                                                ,[MOTIVO]
                                                ,[ANO_INGRESSO]
                                                ,[SEM_INGRESSO]
                                                ,[ANO_ENCERRAMENTO]
                                                ,[SEM_ENCERRAMENTO]
                                                ,[CAUSA_ENCERR]
                                                ,[DT_INSERCAO]
                                                ,[DT_ULTALT]) 
                                  SELECT  [CURSO]
                                                ,[TURNO]
                                                ,[CURRICULO]
                                                ,[ALUNO]
                                                ,@DT_ENCERRAMENTO
                                                ,'DUPLIC_SIS'
                                                ,[ANO_INGRESSO]
                                                ,[SEM_INGRESSO]
                                                ,@ANO_ENCERRAMENTO
                                                ,@SEM_ENCERRAMENTO
                                                ,'OUTROS'
                                                ,@DT_INSERCAO
                                                ,@DT_ULTALT
                                    FROM   LY_ALUNO
                                    WHERE  ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", matricula);
            contextQuery.Parameters.Add("@ANO_ENCERRAMENTO", DateTime.Now.Year);
            contextQuery.Parameters.Add("@DT_ULTALT", DateTime.Now);
            contextQuery.Parameters.Add("@DT_INSERCAO", DateTime.Now);
            contextQuery.Parameters.Add("@DT_ENCERRAMENTO", DateTime.Now);
            contextQuery.Parameters.Add("@SEM_ENCERRAMENTO", semestre);

            contexto.ApplyModifications(contextQuery);
        }

        public void Insere(DataContext contexto, Ly_h_cursos_concl dtEncerramento)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO dbo.LY_H_CURSOS_CONCL
                                   (CURSO
                                   ,TURNO
                                   ,CURRICULO
                                   ,ALUNO
                                   ,DT_ENCERRAMENTO
                                   ,MOTIVO
                                   ,OUTRA_FACULDADE
                                   ,ANO_INGRESSO
                                   ,SEM_INGRESSO
                                   ,DT_COLACAO
                                   ,DT_DIPLOMA
                                   ,DT_REABERTURA
                                   ,ANO_ENCERRAMENTO
                                   ,SEM_ENCERRAMENTO
                                   ,CAUSA_ENCERR
                                   ,DIPLOMA_FACULDADE
                                   ,REGISTRO
                                   ,DT_REGISTRO
                                   ,NOME_DIPLOMA
                                   ,PROCESSO
                                   ,LIVRO
                                   ,FOLHAS
                                   ,NOME_REITOR
                                   ,DT_RETIRA_DIP
                                   ,NUM_OFICIO
                                   ,DT_OFICIO
                                   ,OBSERVACAO
                                   ,SIT_DIPLOMA
                                   ,DT_INSERCAO
                                   ,DT_ULTALT
                                   ,MOTIVO_INVALIDACAO
                                   ,DT_CONFECCAO
                                   ,DT_ASSIN_ALUNO
                                   ,DT_ENVIO_INST_EXT
                                   ,MOTIVOREABERTURA)
                             VALUES
                                   (@CURSO,
                                   @TURNO, 
                                   @CURRICULO, 
                                   @ALUNO, 
                                   @DT_ENCERRAMENTO, 
                                   @MOTIVO, 
                                   @OUTRA_FACULDADE, 
                                   @ANO_INGRESSO, 
                                   @SEM_INGRESSO, 
                                   @DT_COLACAO, 
                                   @DT_DIPLOMA, 
                                   @DT_REABERTURA, 
                                   @ANO_ENCERRAMENTO, 
                                   @SEM_ENCERRAMENTO, 
                                   @CAUSA_ENCERR, 
                                   @DIPLOMA_FACULDADE, 
                                   @REGISTRO, 
                                   @DT_REGISTRO, 
                                   @NOME_DIPLOMA, 
                                   @PROCESSO, 
                                   @LIVRO, 
                                   @FOLHAS, 
                                   @NOME_REITOR, 
                                   @DT_RETIRA_DIP, 
                                   @NUM_OFICIO, 
                                   @DT_OFICIO, 
                                   @OBSERVACAO, 
                                   @SIT_DIPLOMA,
                                   @DT_INSERCAO, 
                                   @DT_ULTALT, 
                                   @MOTIVO_INVALIDACAO, 
                                   @DT_CONFECCAO,
                                   @DT_ASSIN_ALUNO, 
                                   @DT_ENVIO_INST_EXT, 
                                   @MOTIVOREABERTURA ) ";

            contextQuery.Parameters.Add("@CURSO", TechneDbType.T_CODIGO, dtEncerramento.Rows[0].Curso);
            contextQuery.Parameters.Add("@TURNO", TechneDbType.T_CODIGO, dtEncerramento.Rows[0].Turno);
            contextQuery.Parameters.Add("@CURRICULO", TechneDbType.T_CODIGO, dtEncerramento.Rows[0].Curriculo);
            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, dtEncerramento.Rows[0].Aluno);
            contextQuery.Parameters.Add("@DT_ENCERRAMENTO", TechneDbType.T_DATA, dtEncerramento.Rows[0].Dt_encerramento);
            contextQuery.Parameters.Add("@MOTIVO", TechneDbType.T_CODIGO, dtEncerramento.Rows[0].Motivo);
            contextQuery.Parameters.Add("@OUTRA_FACULDADE", TechneDbType.T_CODIGO, dtEncerramento.Rows[0].Outra_faculdade);
            contextQuery.Parameters.Add("@ANO_INGRESSO", TechneDbType.T_ANO, dtEncerramento.Rows[0].Ano_ingresso);
            contextQuery.Parameters.Add("@SEM_INGRESSO", TechneDbType.T_SEMESTRE2, dtEncerramento.Rows[0].Sem_ingresso);
            contextQuery.Parameters.Add("@DT_COLACAO", TechneDbType.T_DATA, dtEncerramento.Rows[0].Dt_colacao);
            contextQuery.Parameters.Add("@DT_DIPLOMA", TechneDbType.T_DATA, dtEncerramento.Rows[0].Dt_diploma);
            contextQuery.Parameters.Add("@DT_REABERTURA", TechneDbType.T_DATA, dtEncerramento.Rows[0].Dt_reabertura);
            contextQuery.Parameters.Add("@ANO_ENCERRAMENTO", TechneDbType.T_ANO, dtEncerramento.Rows[0].Ano_encerramento);
            contextQuery.Parameters.Add("@SEM_ENCERRAMENTO", TechneDbType.T_SEMESTRE2, dtEncerramento.Rows[0].Sem_encerramento);
            contextQuery.Parameters.Add("@CAUSA_ENCERR", TechneDbType.T_CODIGO, dtEncerramento.Rows[0].Causa_encerr);
            contextQuery.Parameters.Add("@DIPLOMA_FACULDADE", TechneDbType.T_CODIGO, dtEncerramento.Rows[0].Diploma_faculdade);
            contextQuery.Parameters.Add("@REGISTRO", TechneDbType.T_CODIGO, dtEncerramento.Rows[0].Registro);
            contextQuery.Parameters.Add("@DT_REGISTRO", TechneDbType.T_DATA, dtEncerramento.Rows[0].Dt_registro);
            contextQuery.Parameters.Add("@NOME_DIPLOMA", TechneDbType.T_ALFAMEDIUM, dtEncerramento.Rows[0].Nome_diploma);
            contextQuery.Parameters.Add("@PROCESSO", TechneDbType.T_CODIGO, dtEncerramento.Rows[0].Processo);
            contextQuery.Parameters.Add("@LIVRO", TechneDbType.T_CODIGO, dtEncerramento.Rows[0].Livro);
            contextQuery.Parameters.Add("@FOLHAS", TechneDbType.T_CODIGO, dtEncerramento.Rows[0].Folhas);
            contextQuery.Parameters.Add("@NOME_REITOR", TechneDbType.T_ALFALARGE, dtEncerramento.Rows[0].Nome_reitor);
            contextQuery.Parameters.Add("@DT_RETIRA_DIP", TechneDbType.T_DATA, dtEncerramento.Rows[0].Dt_retira_dip);
            contextQuery.Parameters.Add("@NUM_OFICIO", TechneDbType.T_NUMERO_GRANDE, dtEncerramento.Rows[0].Num_oficio);
            contextQuery.Parameters.Add("@DT_OFICIO", TechneDbType.T_DATA, dtEncerramento.Rows[0].Dt_oficio);
            contextQuery.Parameters.Add("@OBSERVACAO", TechneDbType.T_ALFAEXTRALARGE, dtEncerramento.Rows[0].Observacao);
            contextQuery.Parameters.Add("@SIT_DIPLOMA", SqlDbType.VarChar, dtEncerramento.Rows[0].Sit_diploma);
            contextQuery.Parameters.Add("@DT_INSERCAO", TechneDbType.T_DATA, dtEncerramento.Rows[0].Dt_insercao);
            contextQuery.Parameters.Add("@DT_ULTALT", TechneDbType.T_DATA, dtEncerramento.Rows[0].Dt_ultalt);
            contextQuery.Parameters.Add("@MOTIVO_INVALIDACAO", TechneDbType.T_ALFAEXTRALARGE, dtEncerramento.Rows[0].Motivo_invalidacao);
            contextQuery.Parameters.Add("@DT_CONFECCAO", TechneDbType.T_DATA, dtEncerramento.Rows[0].Dt_confeccao);
            contextQuery.Parameters.Add("@DT_ASSIN_ALUNO", TechneDbType.T_DATA, dtEncerramento.Rows[0].Dt_assin_aluno);
            contextQuery.Parameters.Add("@DT_ENVIO_INST_EXT", TechneDbType.T_DATA, dtEncerramento.Rows[0].Dt_envio_inst_ext);
            contextQuery.Parameters.Add("@MOTIVOREABERTURA", SqlDbType.VarChar, dtEncerramento.Rows[0].Motivoreabertura);

            contexto.ApplyModifications(contextQuery);
        }

        public void InsereSuspensao(DataContext contexto, int historicoSuspensaoId, int ano, int periodo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO dbo.LY_H_CURSOS_CONCL 
                                        (CURSO, 
                                         TURNO, 
                                         CURRICULO, 
                                         ALUNO, 
                                         DT_ENCERRAMENTO, 
                                         MOTIVO, 
                                         OUTRA_FACULDADE, 
                                         ANO_INGRESSO, 
                                         SEM_INGRESSO, 
                                         CAUSA_ENCERR, 
                                         ANO_ENCERRAMENTO, 
                                         SEM_ENCERRAMENTO, 
                                         DT_ULTALT, 
                                         DT_INSERCAO) 
                            SELECT CURSO, 
                                   TURNO, 
                                   CURRICULO, 
                                   h.ALUNO, 
                                   Getdate(), 
                                   'SUSPENSAO', 
                                   NULL, 
                                   ANO_INGRESSO, 
                                   SEM_INGRESSO, 
                                   'SUSP_26DIASOUMAIS', 
                                   @ANO, 
                                   @SEMESTRE, 
                                   Getdate(), 
                                   Getdate() 
                            FROM   Turma.HISTORICOSUSPENSAO h
	                            inner join LY_ALUNO a on h.ALUNO = a.ALUNO
                            where HISTORICOSUSPENSAOID = @HISTORICOSUSPENSAOID ";

            contextQuery.Parameters.Add("@HISTORICOSUSPENSAOID", SqlDbType.Int, historicoSuspensaoId);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, periodo);

            contexto.ApplyModifications(contextQuery);
        }

        public DateTime? ObtemDataMotivoConclusaoPor(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            DateTime? data = null;
            DataTable dt = null;
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"SELECT DISTINCT PL.DT_FIM_AULA
                            FROM   LY_ALUNO A
                            INNER JOIN LY_H_CURSOS_CONCL CC ON CC.ALUNO = A.ALUNO AND MOTIVO = 'CONCLUSAO'
							INNER JOIN LY_PERIODO_LETIVO PL ON PL.ANO = CC.ANO_ENCERRAMENTO AND PL.PERIODO=CC.SEM_ENCERRAMENTO
                            WHERE a.ALUNO = @ALUNO ";
            try
            {
                contextQuery.Parameters.Add("@ALUNO", aluno);

                dt = ctx.GetDataTable(contextQuery);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        data = Convert.ToDateTime(dt.Rows[0]["DT_FIM_AULA"]);
                    }
                }


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
                ctx.Dispose();
            }

            return data;
        }
    }
}
