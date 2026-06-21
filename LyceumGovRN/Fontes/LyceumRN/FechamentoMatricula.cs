using System.Web;
using System.Linq;

namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Techne.Data;
    using Techne.Library;
    using Techne.Lyceum.CR;
    using Seeduc.Infra.Data;
    using System.Data;
    using Techne.Lyceum.RN.DTOs;
    using System.Collections.ObjectModel;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Lyceum.RN.Util;

    public class FechamentoMatricula : RNBase
    {
        public const string Aprovado = "Aprovado";

        public const string AprovadoComDependencia = "Aprovado Com Dep";

        public const string AprovadoPorConselho = "Aprovado Conselho";

        public const string Cancelado = "Cancelado";

        public const string Dispensado = "Dispensado";

        public const string Inconcluido = "Inconcluido";

        public const string ReprovadoPorFrequencia = "Rep Freq";

        public const string ReprovadoPorNota = "Rep Nota";

        public const string Trancado = "Trancado";

        public const string Promovido = "Promovido";

        public const string Retido = "Retido";

        public void AprovaAlunos(Turma.DadosTurma dadosTurma, List<string> alunos, string matricula, decimal proximoAno, decimal proximoPeriodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            List<DadosDisciplinaParaFechamento> listaDisciplinas = new List<DadosDisciplinaParaFechamento>();
            Disciplina rnDisciplina = new Disciplina();
            ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();

            try
            {
                string situacaoFinal = Aprovado;

                //Lista as disciplinas da turma
                listaDisciplinas = rnDisciplina.ObtemDisciplinasParaFechamentoPor(Convert.ToInt32(dadosTurma.Grade_ID));

                foreach (var aluno in alunos)
                {
                    //Ao finalizar o aluno gerar confirmação de matricula atraves da renovação
                    rnConfirmacaoMatricula.GeraConfirmacaoMatriculaDeRenovacaoPor(ctx, dadosTurma, situacaoFinal, aluno, proximoAno, proximoPeriodo, matricula);

                    this.FinalizaAluno(ctx, dadosTurma, aluno, null, matricula, situacaoFinal, listaDisciplinas, proximoAno, proximoPeriodo);
                }
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();

                //Trata mensagens de erros de validacao no processo de enturmacao
                if (Convert.ToString(ex.Message).Contains("ERRO_VALIDACAO:"))
                {
                    mensagem = Convert.ToString(ex.Message).Replace("ERRO_VALIDACAO:", string.Empty);
                }
                else
                {
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
                }

                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public void AprovaMatriculaDependenciaAlunos(Turma.DadosTurma dadosTurma, List<string[]> alunosDisciplinas, string matricula)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            DadosDisciplinaParaFechamento dadosDisciplina = new DadosDisciplinaParaFechamento();
            Disciplina rnDisciplina = new Disciplina();
            string aluno = string.Empty;
            string disciplina = string.Empty;

            try
            {
                string situacaoFinal = Aprovado;

                foreach (var alunoDisciplina in alunosDisciplinas)
                {
                    //Pega aluno
                    aluno = alunoDisciplina[0].ToString();

                    //Pega disciplina
                    disciplina = alunoDisciplina[1].ToString();

                    //Carregar dados dados disciplina
                    dadosDisciplina = rnDisciplina.ObtemDisciplinaParaFechamento(disciplina);

                    this.FinalizaAlunoPorDisciplinaDeDependencia(ctx, dadosTurma, aluno, matricula, situacaoFinal, dadosDisciplina);
                }
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

        public void AprovaMatriculaEletivaAlunos(Turma.DadosTurma dadosTurma, List<string[]> alunosDisciplinas, string matricula)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            DadosDisciplinaParaFechamento dadosDisciplina = new DadosDisciplinaParaFechamento();
            Disciplina rnDisciplina = new Disciplina();
            string aluno = string.Empty;
            string disciplina = string.Empty;

            try
            {
                string situacaoFinal = Aprovado;

                foreach (var alunoDisciplina in alunosDisciplinas)
                {
                    //Pega aluno
                    aluno = alunoDisciplina[0].ToString();

                    //Pega disciplina
                    disciplina = alunoDisciplina[1].ToString();

                    //Carregar dados dados disciplina
                    dadosDisciplina = rnDisciplina.ObtemDisciplinaParaFechamento(disciplina);

                    this.FinalizaAlunoPorDisciplinaEletiva(ctx, dadosTurma, aluno, matricula, situacaoFinal, dadosDisciplina);
                }
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


        public void ReprovaMatriculaDependenciaAlunos(Turma.DadosTurma dadosTurma, List<string[]> alunosDisciplinas, string matricula)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            DadosDisciplinaParaFechamento dadosDisciplina = new DadosDisciplinaParaFechamento();
            Disciplina rnDisciplina = new Disciplina();
            string aluno = string.Empty;
            string disciplina = string.Empty;

            try
            {
                string situacaoFinal = ReprovadoPorNota;

                foreach (var alunoDisciplina in alunosDisciplinas)
                {
                    //Pega aluno
                    aluno = alunoDisciplina[0].ToString();

                    //Pega disciplina
                    disciplina = alunoDisciplina[1].ToString();

                    //Carregar dados dados disciplina
                    dadosDisciplina = rnDisciplina.ObtemDisciplinaParaFechamento(disciplina);

                    this.FinalizaAlunoPorDisciplinaDeDependencia(ctx, dadosTurma, aluno, matricula, situacaoFinal, dadosDisciplina);
                }
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

        public void AprovaAlunosComEnturmacao(Turma.DadosTurma dadosTurma, List<string> alunos, string anoDest, string periodoDest, string cursoDest, string turnoDest, string curriculoDest, string serieDest, string turmaDest, string matricula, string unidadeDest)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            List<DadosDisciplinaParaFechamento> listaDisciplinas = new List<DadosDisciplinaParaFechamento>();
            Disciplina rnDisciplina = new Disciplina();
            TceTransferencia transferencia = new TceTransferencia();
            TceTransferenciaOrigem transferenciaOrigem = new TceTransferenciaOrigem();
            TceTransferenciaDestino transferenciaDestino = new TceTransferenciaDestino();
            Transferencia rnTransferencia = new Transferencia();
            Matricula rnMatricula = new Matricula();
            Aluno rnAluno = new Aluno();
            RN.Aluno.DadosAluno dadosAluno = new Aluno.DadosAluno();
            ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            int vagas = 0;

            int tipoAprovacao = 2; //1 = Reprovado; 2 = Aprovado

            try
            {
                //VERIFICA SE A TURMA DESTINO TEM VAGA
                vagas = RN.Turma.RetornaVagas(Convert.ToInt32(anoDest), Convert.ToInt32(periodoDest), turmaDest);

                if (vagas < alunos.Count)
                {
                    throw new Exception("ERRO_VALIDACAO:A capacidade da turma desejada não comporta mais alunos.");
                }

                string situacaoFinal = Aprovado;

                //Lista as disciplinas da turma
                listaDisciplinas = rnDisciplina.ObtemDisciplinasParaFechamentoPor(Convert.ToInt32(dadosTurma.Grade_ID));

                foreach (string aluno in alunos)
                {
                    if (rnMatricula.EhMatriculaConcomitante(aluno, Convert.ToInt32(dadosTurma.Ano), Convert.ToInt32(dadosTurma.Periodo), dadosTurma.Grade))
                    {
                        dadosAluno = rnAluno.ObtemDadosAluno(aluno);

                        throw new Exception(String.Format("ERRO_VALIDACAO:O aluno: {0} - {1} está cursando Educação Profissional Concomitante nesta turma, por isso não pode ser enturmado.", dadosAluno.Aluno, dadosAluno.Nome_compl));
                    }
                    //Ao finalizar o aluno gerar confirmação de matricula atraves da renovação
                    rnConfirmacaoMatricula.GeraConfirmacaoMatriculaDeRenovacaoComEnturmacaoPor(ctx, dadosTurma, situacaoFinal, aluno, Convert.ToDecimal(anoDest), Convert.ToDecimal(periodoDest), cursoDest, turnoDest, curriculoDest, Convert.ToDecimal(serieDest), unidadeDest, matricula);

                    this.FinalizaAluno(ctx, dadosTurma, aluno, null, matricula, situacaoFinal, listaDisciplinas, Convert.ToDecimal(anoDest), Convert.ToDecimal(periodoDest));

                    //Enturmacao
                    if (dadosTurma.UnidadeResponsavel == unidadeDest)
                    {
                        //Se a enturmacao for para a mesma unidade de ensino
                        this.MatriculaAluno(ctx, aluno, anoDest, periodoDest, cursoDest, turnoDest, curriculoDest, serieDest, turmaDest, unidadeDest, tipoAprovacao, matricula, dadosTurma.Periodo);
                    }
                    else
                    {
                        //Caso troque de escola criar transferencia
                        GeraTransferencia(ctx, dadosTurma, aluno, anoDest, periodoDest, cursoDest, turnoDest, curriculoDest, serieDest, turmaDest, matricula, unidadeDest);
                    }
                }
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();

                //Trata mensagens de erros de validacao no processo de enturmacao
                if (Convert.ToString(ex.Message).Contains("ERRO_VALIDACAO:"))
                {
                    mensagem = Convert.ToString(ex.Message).Replace("ERRO_VALIDACAO:", string.Empty);
                }
                else
                {
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
                }

                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public void GeraTransferencia(DataContext ctx, Turma.DadosTurma dadosTurma, string aluno, string anoDest, string periodoDest, string cursoDest, string turnoDest, string curriculoDest, string serieDest, string turmaDest, string matricula, string unidadeDest)
        {
            RN.ControleVaga rnControleVaga = new ControleVaga();
            TceConfirmacaoMatricula confirmacao = new TceConfirmacaoMatricula();
            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            TceTransferencia transferencia = new TceTransferencia();
            TceTransferenciaOrigem transferenciaOrigem = new TceTransferenciaOrigem();
            TceTransferenciaDestino transferenciaDestino = new TceTransferenciaDestino();
            Transferencia rnTransferencia = new Transferencia();
            RenovacaoMatricula.Renovacao rnRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Renovacao();
            RenovacaoMatricula.Entidades.Renovacao renovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Entidades.Renovacao();
            DataTable dtRenovacao = new DataTable();
            int vagasLiberadas = 0;
            int vagasUtilizadas = 0;

            //TRANSFERENCIA
            transferencia.Aluno = aluno;
            transferencia.Motivo = "Outros";
            transferencia.MatriculaSolicitante = matricula;
            transferencia.Status = Transferencia.Pendente;

            //Destino
            transferenciaDestino.Turno = turnoDest;
            transferenciaDestino.Turma = turmaDest;
            transferenciaDestino.TipoCurso = null; //rever
            transferenciaDestino.Ano = Convert.ToInt32(anoDest);
            transferenciaDestino.Periodo = Convert.ToInt32(periodoDest);
            transferenciaDestino.Serie = Convert.ToInt32(serieDest);
            transferenciaDestino.Curso = cursoDest;
            transferenciaDestino.Censo = unidadeDest;
            transferenciaDestino.UnidadeFisica = unidadeDest;
            transferenciaDestino.Curriculo = curriculoDest;

            //Origem
            transferenciaOrigem.Censo = dadosTurma.UnidadeResponsavel;
            transferenciaOrigem.UnidadeFisica = dadosTurma.UnidadeResponsavel;
            transferenciaOrigem.Turno = dadosTurma.Turno.ToString();
            transferenciaOrigem.Curriculo = dadosTurma.Curriculo;
            transferenciaOrigem.Turma = dadosTurma.Grade;
            transferenciaOrigem.Ano = Convert.ToInt32(dadosTurma.Ano.ToString());
            transferenciaOrigem.Periodo = Convert.ToInt32(dadosTurma.Periodo);
            transferenciaOrigem.Serie = Convert.ToInt32(dadosTurma.Serie);
            transferenciaOrigem.Curso = dadosTurma.Curso.ToString();

            //Verificar se o aluno já possui confirmação confirmada para a nova escola
            confirmacao = rnConfirmacaoMatricula.ObtemConfirmacaoAtivaPor(transferencia.Aluno, transferenciaDestino.Ano, transferenciaDestino.Periodo, transferenciaDestino.Serie, transferenciaDestino.Turno, transferenciaDestino.Curso, transferenciaDestino.Censo);

            if (confirmacao.IdConfirmacaoMatricula <= 0)
            {
                //Caso não exista validar vagas disponíveis para a serie ou modalidade ou turno pretendido
                vagasLiberadas = 0;
                vagasUtilizadas = 0;

                //Verificar se tem vaga no curso / serie / turno / ano / semestre
                vagasLiberadas = rnControleVaga.ObtemVagasLiberadasTotalPor(ctx,
                    transferenciaDestino.Censo,
                    transferenciaDestino.Ano,
                    transferenciaDestino.Periodo,
                    transferenciaDestino.Serie,
                    transferenciaDestino.Curso,
                    transferenciaDestino.Turno);

                vagasUtilizadas = rnControleVaga.ObtemVagasUtilizadasTotalPor(ctx,
                   transferenciaDestino.Censo,
                   transferenciaDestino.Ano,
                   transferenciaDestino.Periodo,
                   transferenciaDestino.Serie,
                   transferenciaDestino.Curso,
                   transferenciaDestino.Turno);

                if (vagasLiberadas <= vagasUtilizadas)
                {
                    throw new Exception("ERRO_VALIDACAO:Não existem vagas de disponíveis para a escola / curso / serie / turno pretendidos.");
                }


                renovacao = rnRenovacao.ObtemRenovacoesMatriculasPor(transferencia.Aluno, transferenciaDestino.Censo, transferenciaDestino.Curso, transferenciaDestino.Ano, transferenciaDestino.Periodo, transferenciaDestino.Serie.ToString(), transferenciaDestino.Turno);

                if (renovacao.RenovacaoId > 0)
                {
                    transferenciaDestino.EnsinoReligioso = renovacao.EnsinoReligioso;
                    transferenciaDestino.LinguaEstrangeiraFacultativa = renovacao.LinguaEstrangeira;
                }
            }
            else
            {
                transferenciaDestino.EnsinoReligioso = confirmacao.EnsinoReligioso;
                transferenciaDestino.LinguaEstrangeiraFacultativa = confirmacao.LinguaEstrangeiraFacultativa;
            }

            //Insere transferencia
            rnTransferencia.InsereFechamentoMatricula(ctx, transferencia, transferenciaDestino, transferenciaOrigem);
        }

        public void AprovaAlunosComDependencia(Turma.DadosTurma dadosTurma, List<string> alunos, List<string> disciplinasDependencia, string matricula, decimal proximoAno, decimal proximoPeriodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            List<DadosDisciplinaParaFechamento> listaDisciplinas = new List<DadosDisciplinaParaFechamento>();
            Disciplina rnDisciplina = new Disciplina();
            ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();

            try
            {
                string situacaoFinal = AprovadoComDependencia;

                //Lista as disciplinas da turma
                listaDisciplinas = rnDisciplina.ObtemDisciplinasParaFechamentoPor(Convert.ToInt32(dadosTurma.Grade_ID));

                foreach (var aluno in alunos)
                {
                    //Ao finalizar o aluno gerar confirmação de matricula atraves da renovação
                    //rnConfirmacaoMatricula.GeraConfirmacaoMatriculaDeRenovacaoPor(ctx, dadosTurma, situacaoFinal, aluno, proximoAno, proximoPeriodo, matricula);

                    this.FinalizaAluno(ctx, dadosTurma, aluno, disciplinasDependencia, matricula, situacaoFinal, listaDisciplinas, proximoAno, proximoPeriodo);
                }
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();

                //Trata mensagens de erros de validacao no processo de enturmacao
                if (Convert.ToString(ex.Message).Contains("ERRO_VALIDACAO:"))
                {
                    mensagem = Convert.ToString(ex.Message).Replace("ERRO_VALIDACAO:", string.Empty);
                }
                else
                {
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
                }

                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public void AprovaAlunosComDependenciaComEnturmacao(Turma.DadosTurma dadosTurma, List<string> alunos, string anoDest, string periodoDest, string cursoDest, string turnoDest, string curriculoDest, string serieDest, string turmaDest, List<string> disciplinasDependencia, string matricula, string unidadeDest)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            List<DadosDisciplinaParaFechamento> listaDisciplinas = new List<DadosDisciplinaParaFechamento>();
            Disciplina rnDisciplina = new Disciplina();
            TceTransferencia transferencia = new TceTransferencia();
            TceTransferenciaOrigem transferenciaOrigem = new TceTransferenciaOrigem();
            TceTransferenciaDestino transferenciaDestino = new TceTransferenciaDestino();
            Transferencia rnTransferencia = new Transferencia();
            ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            int vagas = 0;

            int tipoAprovacao = 2; //1 = Reprovado; 2 = Aprovado

            try
            {
                //VERIFICA SE A TURMA DESTINO TEM VAGA
                vagas = RN.Turma.RetornaVagas(Convert.ToInt32(anoDest), Convert.ToInt32(periodoDest), turmaDest);

                if (vagas < alunos.Count)
                {
                    throw new Exception("ERRO_VALIDACAO:A capacidade da turma desejada não comporta mais alunos.");
                }

                string situacaoFinal = AprovadoComDependencia;

                //Lista as disciplinas da turma
                listaDisciplinas = rnDisciplina.ObtemDisciplinasParaFechamentoPor(Convert.ToInt32(dadosTurma.Grade_ID));

                foreach (string aluno in alunos)
                {
                    //Ao finalizar o aluno gerar confirmação de matricula atraves da renovação
                    rnConfirmacaoMatricula.GeraConfirmacaoMatriculaDeRenovacaoComEnturmacaoPor(ctx, dadosTurma, situacaoFinal, aluno, Convert.ToDecimal(anoDest), Convert.ToDecimal(periodoDest), cursoDest, turnoDest, curriculoDest, Convert.ToDecimal(serieDest), unidadeDest, matricula);

                    this.FinalizaAluno(ctx, dadosTurma, aluno, disciplinasDependencia, matricula, situacaoFinal, listaDisciplinas, Convert.ToDecimal(anoDest), Convert.ToDecimal(periodoDest));

                    //Enturmacao
                    if (dadosTurma.UnidadeResponsavel == unidadeDest)
                    {
                        //Se a enturmacao for para a mesma unidade de ensino
                        this.MatriculaAluno(ctx, aluno, anoDest, periodoDest, cursoDest, turnoDest, curriculoDest, serieDest, turmaDest, unidadeDest, tipoAprovacao, matricula, dadosTurma.Periodo);
                    }
                    else
                    {
                        //Caso troque de escola criar transferencia
                        GeraTransferencia(ctx, dadosTurma, aluno, anoDest, periodoDest, cursoDest, turnoDest, curriculoDest, serieDest, turmaDest, matricula, unidadeDest);
                    }
                }
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();

                //Trata mensagens de erros de validacao no processo de enturmacao
                if (Convert.ToString(ex.Message).Contains("ERRO_VALIDACAO:"))
                {
                    mensagem = Convert.ToString(ex.Message).Replace("ERRO_VALIDACAO:", string.Empty);
                }
                else
                {
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
                }

                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public void ReprovaAlunosPorNota(Turma.DadosTurma dadosTurma, List<string> alunos, string matricula, decimal proximoAno, decimal proximoPeriodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            List<DadosDisciplinaParaFechamento> listaDisciplinas = new List<DadosDisciplinaParaFechamento>();
            Disciplina rnDisciplina = new Disciplina();
            ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();

            try
            {
                string situacaoFinal = ReprovadoPorNota;

                //Lista as disciplinas da turma
                listaDisciplinas = rnDisciplina.ObtemDisciplinasParaFechamentoPor(Convert.ToInt32(dadosTurma.Grade_ID));

                foreach (var aluno in alunos)
                {
                    //Ao finalizar o aluno gerar confirmação de matricula atraves da renovação
                    rnConfirmacaoMatricula.GeraConfirmacaoMatriculaDeRenovacaoPor(ctx, dadosTurma, situacaoFinal, aluno, proximoAno, proximoPeriodo, matricula);

                    this.FinalizaAluno(ctx, dadosTurma, aluno, null, matricula, situacaoFinal, listaDisciplinas, proximoAno, proximoPeriodo);
                }
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();

                //Trata mensagens de erros de validacao no processo de enturmacao
                if (Convert.ToString(ex.Message).Contains("ERRO_VALIDACAO:"))
                {
                    mensagem = Convert.ToString(ex.Message).Replace("ERRO_VALIDACAO:", string.Empty);
                }
                else
                {
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
                }

                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public void ReprovaAlunosPorNotaComEnturmacao(Turma.DadosTurma dadosTurma, List<string> alunos, string anoDest, string periodoDest, string cursoDest, string turnoDest, string curriculoDest, string serieDest, string turmaDest, string matricula, string unidadeDest)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            List<DadosDisciplinaParaFechamento> listaDisciplinas = new List<DadosDisciplinaParaFechamento>();
            Disciplina rnDisciplina = new Disciplina();
            TceTransferencia transferencia = new TceTransferencia();
            TceTransferenciaOrigem transferenciaOrigem = new TceTransferenciaOrigem();
            TceTransferenciaDestino transferenciaDestino = new TceTransferenciaDestino();
            Transferencia rnTransferencia = new Transferencia();
            int vagas = 0;
            ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            int tipoAprovacao = 1; //1 = Reprovado; 2 = Aprovado

            try
            {
                //VERIFICA SE A TURMA DESTINO TEM VAGA
                vagas = RN.Turma.RetornaVagas(Convert.ToInt32(anoDest), Convert.ToInt32(periodoDest), turmaDest);

                if (vagas < alunos.Count)
                {
                    throw new Exception("ERRO_VALIDACAO:A capacidade da turma desejada não comporta mais alunos.");
                }

                string situacaoFinal = ReprovadoPorNota;

                //Lista as disciplinas da turma
                listaDisciplinas = rnDisciplina.ObtemDisciplinasParaFechamentoPor(Convert.ToInt32(dadosTurma.Grade_ID));

                foreach (string aluno in alunos)
                {
                    //Ao finalizar o aluno gerar confirmação de matricula atraves da renovação
                    rnConfirmacaoMatricula.GeraConfirmacaoMatriculaDeRenovacaoComEnturmacaoPor(ctx, dadosTurma, situacaoFinal, aluno, Convert.ToDecimal(anoDest), Convert.ToDecimal(periodoDest), cursoDest, turnoDest, curriculoDest, Convert.ToDecimal(serieDest), unidadeDest, matricula);

                    this.FinalizaAluno(ctx, dadosTurma, aluno, null, matricula, situacaoFinal, listaDisciplinas, Convert.ToDecimal(anoDest), Convert.ToDecimal(periodoDest));

                    //Enturmacao
                    if (dadosTurma.UnidadeResponsavel == unidadeDest)
                    {
                        //Se a enturmacao for para a mesma unidade de ensino
                        this.MatriculaAluno(ctx, aluno, anoDest, periodoDest, cursoDest, turnoDest, curriculoDest, serieDest, turmaDest, unidadeDest, tipoAprovacao, matricula, dadosTurma.Periodo);
                    }
                    else
                    {
                        //Caso troque de escola criar transferencia
                        GeraTransferencia(ctx, dadosTurma, aluno, anoDest, periodoDest, cursoDest, turnoDest, curriculoDest, serieDest, turmaDest, matricula, unidadeDest);
                    }
                }
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();

                //Trata mensagens de erros de validacao no processo de enturmacao
                if (Convert.ToString(ex.Message).Contains("ERRO_VALIDACAO:"))
                {
                    mensagem = Convert.ToString(ex.Message).Replace("ERRO_VALIDACAO:", string.Empty);
                }
                else
                {
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
                }

                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public void ReprovaAlunosPorFrequencia(Turma.DadosTurma dadosTurma, List<string> alunos, string matricula, decimal proximoAno, decimal proximoPeriodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            List<DadosDisciplinaParaFechamento> listaDisciplinas = new List<DadosDisciplinaParaFechamento>();
            Disciplina rnDisciplina = new Disciplina();
            ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();

            try
            {
                string situacaoFinal = ReprovadoPorFrequencia;

                //Lista as disciplinas da turma
                listaDisciplinas = rnDisciplina.ObtemDisciplinasParaFechamentoPor(Convert.ToInt32(dadosTurma.Grade_ID));

                foreach (var aluno in alunos)
                {
                    //Ao finalizar o aluno gerar confirmação de matricula atraves da renovação
                    rnConfirmacaoMatricula.GeraConfirmacaoMatriculaDeRenovacaoPor(ctx, dadosTurma, situacaoFinal, aluno, proximoAno, proximoPeriodo, matricula);

                    this.FinalizaAluno(ctx, dadosTurma, aluno, null, matricula, situacaoFinal, listaDisciplinas, proximoAno, proximoPeriodo);
                }
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();

                //Trata mensagens de erros de validacao no processo de enturmacao
                if (Convert.ToString(ex.Message).Contains("ERRO_VALIDACAO:"))
                {
                    mensagem = Convert.ToString(ex.Message).Replace("ERRO_VALIDACAO:", string.Empty);
                }
                else
                {
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
                }

                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public void ReprovaAlunosPorFrequenciaComEnturmacao(Turma.DadosTurma dadosTurma, List<string> alunos, string anoDest, string periodoDest, string cursoDest, string turnoDest, string curriculoDest, string serieDest, string turmaDest, string matricula, string unidadeDest)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            List<DadosDisciplinaParaFechamento> listaDisciplinas = new List<DadosDisciplinaParaFechamento>();
            Disciplina rnDisciplina = new Disciplina();
            TceTransferencia transferencia = new TceTransferencia();
            TceTransferenciaOrigem transferenciaOrigem = new TceTransferenciaOrigem();
            TceTransferenciaDestino transferenciaDestino = new TceTransferenciaDestino();
            Transferencia rnTransferencia = new Transferencia();
            int vagas = 0;
            ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            int tipoAprovacao = 1; //1 = Reprovado; 2 = Aprovado

            try
            {
                //VERIFICA SE A TURMA DESTINO TEM VAGA
                vagas = RN.Turma.RetornaVagas(Convert.ToInt32(anoDest), Convert.ToInt32(periodoDest), turmaDest);

                if (vagas < alunos.Count)
                {
                    throw new Exception("ERRO_VALIDACAO:A capacidade da turma desejada não comporta mais alunos.");
                }

                string situacaoFinal = ReprovadoPorFrequencia;

                //Lista as disciplinas da turma
                listaDisciplinas = rnDisciplina.ObtemDisciplinasParaFechamentoPor(Convert.ToInt32(dadosTurma.Grade_ID));

                foreach (string aluno in alunos)
                {
                    //Ao finalizar o aluno gerar confirmação de matricula atraves da renovação
                    rnConfirmacaoMatricula.GeraConfirmacaoMatriculaDeRenovacaoComEnturmacaoPor(ctx, dadosTurma, situacaoFinal, aluno, Convert.ToDecimal(anoDest), Convert.ToDecimal(periodoDest), cursoDest, turnoDest, curriculoDest, Convert.ToDecimal(serieDest), unidadeDest, matricula);

                    this.FinalizaAluno(ctx, dadosTurma, aluno, null, matricula, situacaoFinal, listaDisciplinas, Convert.ToDecimal(anoDest), Convert.ToDecimal(periodoDest));

                    //Enturmacao
                    if (dadosTurma.UnidadeResponsavel == unidadeDest)
                    {
                        //Se a enturmacao for para a mesma unidade de ensino
                        this.MatriculaAluno(ctx, aluno, anoDest, periodoDest, cursoDest, turnoDest, curriculoDest, serieDest, turmaDest, unidadeDest, tipoAprovacao, matricula, dadosTurma.Periodo);
                    }
                    else
                    {
                        //Caso troque de escola criar transferencia
                        GeraTransferencia(ctx, dadosTurma, aluno, anoDest, periodoDest, cursoDest, turnoDest, curriculoDest, serieDest, turmaDest, matricula, unidadeDest);
                    }
                }
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();

                //Trata mensagens de erros de validacao no processo de enturmacao
                if (Convert.ToString(ex.Message).Contains("ERRO_VALIDACAO:"))
                {
                    mensagem = Convert.ToString(ex.Message).Replace("ERRO_VALIDACAO:", string.Empty);
                }
                else
                {
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
                }

                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        private void AtualizaSituacaoFinalAluno(DataContext ctx, Turma.DadosTurma dadosTurma, string aluno, string situacaoFinal, string matricula)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();
                decimal frequenciaGlobal = 0;

                //Busca frequencia global do aluno
                frequenciaGlobal = HistMatricula.VerificaFrequenciaGlobal(aluno, int.Parse(dadosTurma.Ano), int.Parse(dadosTurma.Periodo), dadosTurma.Grade);

                contextQuery.Command = @" INSERT  INTO dbo.TCE_SITUACAO_FINAL_ALUNO
                                ( ALUNO ,
                                  ANO ,
                                  PERIODO ,
                                  TURMA ,
                                  SITUACAO_FINAL ,
                                  FREQUENCIA_GLOBAL ,
                                  MATRICULA ,
                                  DT_CADASTRO        
                                )
                        VALUES  ( @ALUNO ,
                                  @ANO ,
                                  @PERIODO ,
                                  @TURMA ,
                                  @SITUACAO_FINAL ,
                                  @FREQUENCIA_GLOBAL ,
                                  @MATRICULA ,
                                  GETDATE()
                                ) ";

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", dadosTurma.Ano);
                contextQuery.Parameters.Add("@PERIODO", dadosTurma.Periodo);
                contextQuery.Parameters.Add("@TURMA", dadosTurma.Grade);
                contextQuery.Parameters.Add("@SITUACAO_FINAL", situacaoFinal);
                contextQuery.Parameters.Add("@FREQUENCIA_GLOBAL", frequenciaGlobal);
                contextQuery.Parameters.Add("@MATRICULA", matricula);

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
        }

        private void FinalizaAluno(DataContext ctx, Turma.DadosTurma dadosTurma, string aluno, List<string> disciplinasDependencia, string matricula, string situacaoFinal, List<DadosDisciplinaParaFechamento> listaDisciplinas, decimal proximoAno, decimal proximoPeriodo)
        {
            try
            {
                HistFaculdade rnHistFaculdade = new HistFaculdade();
                HistMatricula rnHistMatricula = new HistMatricula();
                int ordem = 0;
                Matricula rnMatricula = new Matricula();
                Turma rnTurma = new Turma();
                Matgrade rnMatGrade = new Matgrade();
                ICollection<LyMatGrade> listaMatGrade = new List<LyMatGrade>();
                LyHistMatGrade histMatGrade = new LyHistMatGrade();
                HistMatGrade rnHistMatGrade = new HistMatGrade();
                Instituicao rnInstituicao = new Instituicao();
                ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
                RN.SituacaoFinalAluno rnSituacaoFinalAluno = new SituacaoFinalAluno();
                string situacaoMatGrade = string.Empty;
                string nomeAluno = string.Empty;

                //Para cursos mais educacao (9999.92) e turmas optativa reforço, não serão geradas situação final
                //NOTA: ver se Dependenciairá utilizar este metodo, se sim tratar.
                if (dadosTurma.Curso != "9999.92" && dadosTurma.OptativaReforco == "N" && dadosTurma.Eletiva == "N")
                {
                    if (rnSituacaoFinalAluno.PossuiSituacaoFinal(aluno, Convert.ToInt32(dadosTurma.Ano), Convert.ToInt32(dadosTurma.Periodo), dadosTurma.Grade))
                    {
                        nomeAluno = RN.Aluno.ConsultarNome(aluno);

                        throw new Exception(String.Format("ERRO_VALIDACAO:O(a) aluno(a) {0} - {1} não pode ser finalizado pois já possui situação final para a turma {2}.", aluno, nomeAluno, dadosTurma.Grade));
                    }
                }

                //Busca ordem ideal para aquele aluno / ano / periodo / turma
                ordem = rnHistMatricula.ObtemOrdemIdeal(aluno, Convert.ToInt32(dadosTurma.Ano), Convert.ToInt32(dadosTurma.Periodo), dadosTurma.Grade);

                if (rnInstituicao.ExisteInstituicao(dadosTurma.UnidadeResponsavel))
                {
                    //Insere na tabela HistFaculdade
                    LyHistFaculdade histFaculdade = new LyHistFaculdade
                    {
                        Aluno = aluno,
                        Ordem = ordem,
                        OutraFaculdade = dadosTurma.UnidadeResponsavel
                    };
                    rnHistFaculdade.Insere(ctx, histFaculdade);
                }
                else
                {
                    throw new Exception(String.Format("ERRO_VALIDACAO:A unidade de ensino {0} - {1} não está liberada para fechamento do período letivo. Favor entrar em contato com a Central de Relacionamentos, informando o código do censo da sua unidade e o motivo 'Cadastro de Instituições'.", dadosTurma.UnidadeResponsavel, dadosTurma.NomeUnidade));
                }

                foreach (DadosDisciplinaParaFechamento disciplina in listaDisciplinas)
                {
                    this.FinalizaDisciplina(ctx, disciplina, ordem, dadosTurma, aluno, situacaoFinal, disciplinasDependencia, matricula);
                }

                // Verifica se ainda possui outros alunos matriculados na turma                    
                if (!rnMatricula.PossuiMatriculaAtivaNaTurma(dadosTurma.Grade, Convert.ToDecimal(dadosTurma.Ano), Convert.ToDecimal(dadosTurma.Periodo)))
                {
                    //Caso não exista mais alunos, muda a situação da turma para finalizada.
                    rnTurma.FinalizaTurma(ctx, dadosTurma.Grade, Convert.ToDecimal(dadosTurma.Ano), Convert.ToDecimal(dadosTurma.Periodo));
                }

                //Lista os registros da matgrade do aluno / turma
                listaMatGrade = rnMatGrade.ObtemListaPor(aluno, Convert.ToDecimal(dadosTurma.Ano), Convert.ToDecimal(dadosTurma.Periodo), dadosTurma.Curso, dadosTurma.Turno, dadosTurma.Curriculo, dadosTurma.Serie, dadosTurma.Grade);

                foreach (LyMatGrade item in listaMatGrade)
                {
                    //Ajusta situação para tabela matgrade                    
                    if (item.SitMatgrade == "Matriculado")
                    {
                        if (situacaoFinal.Contains("Rep"))
                        {
                            situacaoMatGrade = "Reprovado";
                        }
                        else if (situacaoFinal.Contains("Dep"))
                        {
                            situacaoMatGrade = "Aprovado";
                        }
                        else
                        {
                            situacaoMatGrade = situacaoFinal;
                        }
                    }
                    else
                    {
                        situacaoMatGrade = item.SitMatgrade;
                    }

                    //Monta entidade histMatGrade
                    histMatGrade.Aluno = item.Aluno;
                    histMatGrade.Grade_Id = item.GradeId;
                    histMatGrade.DtUltalt = item.DtUltalt.ToString("MM/dd/yyyy HH:mm:ss.fff");
                    histMatGrade.Num_chamada = item.NumChamada;
                    histMatGrade.Sit_Matgrade = situacaoMatGrade;

                    // verifica se existe historico cadastrado                   
                    if (rnHistMatGrade.PossuiCadastradoPor(item.Aluno, item.GradeId, item.DtUltalt))
                    {
                        //atualiza
                        rnHistMatGrade.Altera(ctx, histMatGrade);
                    }
                    else
                    {
                        // insere
                        rnHistMatGrade.Insere(ctx, histMatGrade);
                    }

                    // deleta matgrade
                    rnMatGrade.Remove(ctx, item.Aluno, item.GradeId, item.DtUltalt);

                    
                }

                if (dadosTurma.Curso != "9999.92" && dadosTurma.OptativaReforco == "N" && dadosTurma.Eletiva == "N")
                {
                    this.AtualizaSituacaoFinalAluno(ctx, dadosTurma, aluno, situacaoFinal, matricula);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FinalizaAlunoPorDisciplinaDeDependencia(DataContext ctx, Turma.DadosTurma dadosTurma, string aluno, string matricula, string situacaoFinal, DadosDisciplinaParaFechamento disciplina)
        {
            try
            {
                HistFaculdade rnHistFaculdade = new HistFaculdade();
                HistMatricula rnHistMatricula = new HistMatricula();
                int ordem = 0;
                Matricula rnMatricula = new Matricula();
                Turma rnTurma = new Turma();
                Matgrade rnMatGrade = new Matgrade();
                ICollection<LyMatGrade> listaMatGrade = new List<LyMatGrade>();
                LyHistMatGrade histMatGrade = new LyHistMatGrade();
                HistMatGrade rnHistMatGrade = new HistMatGrade();
                string situacaoMatGrade = string.Empty;
                Instituicao rnInstituicao = new Instituicao();

                //Busca ordem ideal para aquele aluno / ano / periodo / turma
                ordem = rnHistMatricula.ObtemOrdemIdeal(aluno, Convert.ToInt32(dadosTurma.Ano), Convert.ToInt32(dadosTurma.Periodo), dadosTurma.Grade);

                if (rnInstituicao.ExisteInstituicao(dadosTurma.UnidadeResponsavel))
                {

                    //Insere na tabela HistFaculdade
                    LyHistFaculdade histFaculdade = new LyHistFaculdade
                    {
                        Aluno = aluno,
                        Ordem = ordem,
                        OutraFaculdade = dadosTurma.UnidadeResponsavel
                    };
                    rnHistFaculdade.Insere(ctx, histFaculdade);
                }
                else
                {
                    throw new Exception(String.Format("ERRO_VALIDACAO:A unidade de ensino {0} - {1} não está liberada para fechamento do período letivo. Favor entrar em contato com a Central de Relacionamentos, informando o código do censo da sua unidade e o motivo 'Cadastro de Instituições'.", dadosTurma.UnidadeResponsavel, dadosTurma.NomeUnidade));
                }

                this.FinalizaDisciplina(ctx, disciplina, ordem, dadosTurma, aluno, situacaoFinal, null, matricula);

                // Verifica se ainda possui outros alunos matriculados na turma                    
                if (!rnMatricula.PossuiMatriculaAtivaNaTurma(dadosTurma.Grade, Convert.ToDecimal(dadosTurma.Ano), Convert.ToDecimal(dadosTurma.Periodo)))
                {
                    //Caso não exista mais alunos, muda a situação da turma para finalizada.
                    rnTurma.FinalizaTurma(ctx, dadosTurma.Grade, Convert.ToDecimal(dadosTurma.Ano), Convert.ToDecimal(dadosTurma.Periodo));
                }

                //Verifica se nao sobrou disciplina na turma do aluno, para atualizar matgrade
                if (!rnMatricula.PossuiMatriculaAtivaNaTurmaPorAluno(ctx, aluno, dadosTurma.Grade, Convert.ToDecimal(dadosTurma.Ano), Convert.ToDecimal(dadosTurma.Periodo)))
                {
                    //Lista os registros da matgrade do aluno / turma
                    listaMatGrade = rnMatGrade.ObtemListaPor(aluno, Convert.ToDecimal(dadosTurma.Ano), Convert.ToDecimal(dadosTurma.Periodo), dadosTurma.Curso, dadosTurma.Turno, dadosTurma.Curriculo, dadosTurma.Serie, dadosTurma.Grade);

                    foreach (LyMatGrade item in listaMatGrade)
                    {
                        //Ajusta situação para tabela matgrade                    
                        if (item.SitMatgrade == "Matriculado")
                        {
                            if (situacaoFinal.Contains("Rep"))
                            {
                                situacaoMatGrade = "Reprovado";
                            }
                            else
                            {
                                situacaoMatGrade = situacaoFinal;
                            }
                        }
                        else
                        {
                            situacaoMatGrade = item.SitMatgrade;
                        }

                        //Monta entidade histMatGrade
                        histMatGrade.Aluno = item.Aluno;
                        histMatGrade.Grade_Id = item.GradeId;
                        histMatGrade.DtUltalt = item.DtUltalt.ToString("MM/dd/yyyy HH:mm:ss.fff");
                        histMatGrade.Num_chamada = item.NumChamada;
                        histMatGrade.Sit_Matgrade = situacaoMatGrade;

                        // verifica se existe historico cadastrado                   
                        if (rnHistMatGrade.PossuiCadastradoPor(item.Aluno, item.GradeId, item.DtUltalt))
                        {
                            //atualiza
                            rnHistMatGrade.Altera(ctx, histMatGrade);
                        }
                        else
                        {
                            // insere
                            rnHistMatGrade.Insere(ctx, histMatGrade);
                        }

                        // deleta matgrade
                        rnMatGrade.Remove(ctx, item.Aluno, item.GradeId, item.DtUltalt);
                    }
                }
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
        }

        private void FinalizaAlunoPorDisciplinaEletiva(DataContext ctx, Turma.DadosTurma dadosTurma, string aluno, string matricula, string situacaoFinal, DadosDisciplinaParaFechamento disciplina)
        {
            try
            {
                HistFaculdade rnHistFaculdade = new HistFaculdade();
                HistMatricula rnHistMatricula = new HistMatricula();
                int ordem = 0;
                Matricula rnMatricula = new Matricula();
                Turma rnTurma = new Turma();
                Matgrade rnMatGrade = new Matgrade();
                ICollection<LyMatGrade> listaMatGrade = new List<LyMatGrade>();
                LyHistMatGrade histMatGrade = new LyHistMatGrade();
                HistMatGrade rnHistMatGrade = new HistMatGrade();
                string situacaoMatGrade = string.Empty;
                Instituicao rnInstituicao = new Instituicao();

                //Busca ordem ideal para aquele aluno / ano / periodo / turma
                ordem = rnHistMatricula.ObtemOrdemIdeal(aluno, Convert.ToInt32(dadosTurma.Ano), Convert.ToInt32(dadosTurma.Periodo), dadosTurma.Grade);

                if (rnInstituicao.ExisteInstituicao(dadosTurma.UnidadeResponsavel))
                {
                    //Insere na tabela HistFaculdade
                    LyHistFaculdade histFaculdade = new LyHistFaculdade
                    {
                        Aluno = aluno,
                        Ordem = ordem,
                        OutraFaculdade = dadosTurma.UnidadeResponsavel
                    };
                    rnHistFaculdade.Insere(ctx, histFaculdade);
                }
                else
                {
                    throw new Exception(String.Format("ERRO_VALIDACAO:A unidade de ensino {0} - {1} não está liberada para fechamento do período letivo. Favor entrar em contato com a Central de Relacionamentos, informando o código do censo da sua unidade e o motivo 'Cadastro de Instituições'.", dadosTurma.UnidadeResponsavel, dadosTurma.NomeUnidade));
                }

                this.FinalizaDisciplina(ctx, disciplina, ordem, dadosTurma, aluno, situacaoFinal, null, matricula);

                // Verifica se ainda possui outros alunos matriculados na turma                    
                if (!rnMatricula.PossuiMatriculaAtivaNaTurma(dadosTurma.Grade, Convert.ToDecimal(dadosTurma.Ano), Convert.ToDecimal(dadosTurma.Periodo)))
                {
                    //Caso não exista mais alunos, muda a situação da turma para finalizada.
                    rnTurma.FinalizaTurma(ctx, dadosTurma.Grade, Convert.ToDecimal(dadosTurma.Ano), Convert.ToDecimal(dadosTurma.Periodo));
                }

                //Verifica se nao sobrou disciplina na turma do aluno, para atualizar matgrade
                if (!rnMatricula.PossuiMatriculaAtivaNaTurmaPorAluno(ctx, aluno, dadosTurma.Grade, Convert.ToDecimal(dadosTurma.Ano), Convert.ToDecimal(dadosTurma.Periodo)))
                {
                    //Lista os registros da matgrade do aluno / turma
                    listaMatGrade = rnMatGrade.ObtemListaPor(aluno, Convert.ToDecimal(dadosTurma.Ano), Convert.ToDecimal(dadosTurma.Periodo), dadosTurma.Curso, dadosTurma.Turno, dadosTurma.Curriculo, dadosTurma.Serie, dadosTurma.Grade);

                    foreach (LyMatGrade item in listaMatGrade)
                    {
                        //Ajusta situação para tabela matgrade                    
                        if (item.SitMatgrade == "Matriculado")
                        {
                            if (situacaoFinal.Contains("Rep"))
                            {
                                situacaoMatGrade = "Reprovado";
                            }
                            else
                            {
                                situacaoMatGrade = situacaoFinal;
                            }
                        }
                        else
                        {
                            situacaoMatGrade = item.SitMatgrade;
                        }

                        //Monta entidade histMatGrade
                        histMatGrade.Aluno = item.Aluno;
                        histMatGrade.Grade_Id = item.GradeId;
                        histMatGrade.DtUltalt = item.DtUltalt.ToString("MM/dd/yyyy HH:mm:ss.fff");
                        histMatGrade.Num_chamada = item.NumChamada;
                        histMatGrade.Sit_Matgrade = situacaoMatGrade;

                        // verifica se existe historico cadastrado                   
                        if (rnHistMatGrade.PossuiCadastradoPor(item.Aluno, item.GradeId, item.DtUltalt))
                        {
                            //atualiza
                            rnHistMatGrade.Altera(ctx, histMatGrade);
                        }
                        else
                        {
                            // insere
                            rnHistMatGrade.Insere(ctx, histMatGrade);
                        }

                        // deleta matgrade
                        rnMatGrade.Remove(ctx, item.Aluno, item.GradeId, item.DtUltalt);
                    }
                }
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
        }


        private void FinalizaDisciplina(DataContext ctx, DadosDisciplinaParaFechamento disciplina, int ordem, Turma.DadosTurma dadosTurma, string aluno, string situacaoFinal, List<string> dependencias, string matriculaResponsavel)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();
                LyMatricula matricula = new LyMatricula();
                Matricula rnMatricula = new Matricula();
                LyHistMatricula histMatricula = new LyHistMatricula();
                HistMatricula rnHistMatricula = new HistMatricula();
                Nota rnNota = new Nota();
                string situacaoDisciplina = situacaoFinal;
                int bimComNotas = 0;
                int bimComFaltas = 0;
                decimal notaFinal = 0;
                DadosFaltasAulas dadosFalta = new DadosFaltasAulas();
                NotaHistmatr rnNotaHistmatr = new NotaHistmatr();
                FaltaHistmatr rnFaltaHistmatr = new FaltaHistmatr();
                Falta rnFalta = new Falta();
                AulaDocente rnAulaDocente = new AulaDocente();
                List<decimal> listaDocentes = new List<decimal>();
                HistoricoDocente rnHistoricoDocente = new HistoricoDocente();
                LyHistoricoDocente historicoDocente = new LyHistoricoDocente();
                DeclaracaoSemNota rnDeclaracaoSemNota = new DeclaracaoSemNota();
                RN.PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();

                //Carrega a matricula do aluno para aquela disciplina / turma / ano / periodo
                matricula = rnMatricula.ObtemMatriculaPor(aluno, Convert.ToInt32(dadosTurma.Ano), Convert.ToInt32(dadosTurma.Periodo), dadosTurma.Grade, disciplina.Disciplina);

                //Caso o aluno não possui aquela disciplina vai para o proximo aluno 
                if (string.IsNullOrEmpty(matricula.Aluno))
                {
                    return;
                }

                //Para alunos aprovados com Dependencia inciar situacao da disciplina como aprovado 
                if (situacaoFinal == AprovadoComDependencia)
                {
                    situacaoDisciplina = Aprovado;
                }
                //Verifica se o aluno ficou reprovado nesta disciplina, caso esteja altera situação da disciplina
                if (dependencias != null && dependencias.Contains(disciplina.Disciplina))
                {
                    situacaoDisciplina = ReprovadoPorNota;
                }

                //Monta entidade da tabela ly_histMatricula
                histMatricula.Aluno = aluno;
                histMatricula.Ordem = ordem;
                histMatricula.Ano = Convert.ToDecimal(dadosTurma.Ano);
                histMatricula.Semestre = Convert.ToDecimal(dadosTurma.Periodo);
                histMatricula.Disciplina = disciplina.Disciplina;
                histMatricula.Turma = dadosTurma.Grade;
                histMatricula.DtInicio = dadosTurma.dtInicio;
                histMatricula.DtFim = dadosTurma.dtFim;
                histMatricula.OptativaReforco = dadosTurma.OptativaReforco;
                histMatricula.Creditos = disciplina.Creditos;

                if (matricula.SitMatricula == RN.Matricula.Matriculado)
                {
                    histMatricula.SituacaoHist = situacaoDisciplina;
                }
                else
                {
                    histMatricula.SituacaoHist = matricula.SitMatricula;
                }

                histMatricula.HorasAula = disciplina.HorasAula + disciplina.HorasLab + disciplina.HorasAtiv + disciplina.HorasEstagio;
                histMatricula.Serie = Convert.ToDecimal(dadosTurma.Serie);
                histMatricula.UnidadeEnsino = dadosTurma.UnidadeResponsavel;
                histMatricula.Matricula = matriculaResponsavel;
                histMatricula.NivelPresenca = "Presencial";

                bimComNotas = RN.Nota.RetornaBimestesLançados(histMatricula.Aluno, histMatricula.Ano,
                                                                  histMatricula.Semestre, histMatricula.Turma,
                                                                  histMatricula.Disciplina);
                bimComFaltas = RN.Falta.RetornaBimestesLançados(histMatricula.Aluno, histMatricula.Ano,
                                                                  histMatricula.Semestre, histMatricula.Turma,
                                                                  histMatricula.Disciplina);


                var totalSubPeriodo = rnPeriodoLetivo.ObtemTotalSubPeriodo(Convert.ToInt32(histMatricula.Ano), Convert.ToInt32(histMatricula.Semestre));


                if (bimComNotas == totalSubPeriodo && bimComFaltas == totalSubPeriodo)
                {
                    notaFinal = RN.Nota.CalculaNotaFinal(histMatricula.Aluno, histMatricula.Ano,
                                                                  histMatricula.Semestre, histMatricula.Turma,
                                                                  histMatricula.Disciplina);
                    histMatricula.NotaFinal = notaFinal.ToString();

                    dadosFalta = RN.Falta.CalculaFaltasAulas(histMatricula.Aluno,
                                                                 histMatricula.Ano,
                                                                 histMatricula.Semestre,
                                                                 histMatricula.Turma,
                                                                 histMatricula.Disciplina);
                    histMatricula.AulasDadas = Convert.ToDecimal(dadosFalta.AulasDadas);
                    histMatricula.FaltaFinal = Convert.ToInt32(dadosFalta.FaltasFinal);
                    histMatricula.AulasPrevistas = Convert.ToDecimal(dadosFalta.AulasPrevistas);
                }

                //Verifica se a matricula já existe, para inserir ou alterar
                if (rnHistMatricula.PossuiHistoricoMatriculaPor(aluno, ordem, Convert.ToDecimal(dadosTurma.Ano), Convert.ToDecimal(dadosTurma.Periodo), disciplina.Disciplina))
                {
                    //Carrega parte de matriculas especiais para atualizar
                    histMatricula.Dependencia = matricula.Dependencia;
                    histMatricula.SerieReferencia = matricula.SerieReferencia;
                    histMatricula.DisciplinaReferencia = matricula.DisciplinaReferencia;
                    histMatricula.Concomitante = matricula.Concomitante;
                    histMatricula.EducEspecial = matricula.EducEspecial;
                    histMatricula.MaisEducacao = matricula.MaisEducacao;

                    rnHistMatricula.AlteraPorMatriculaParaFechamento(ctx, histMatricula);
                }
                else
                {
                    rnHistMatricula.InserePorMatriculaParaFechamento(ctx, histMatricula);
                }

                //Grava notas no histórico
                rnNotaHistmatr.InserePorMatriculaParaFechamento(ctx, histMatricula);

                //Deleta as declaraçoes sem nota
                rnDeclaracaoSemNota.Remove(ctx, histMatricula.Aluno, histMatricula.Disciplina, histMatricula.Turma, histMatricula.Ano, histMatricula.Semestre);

                //Deleta notas do aluno
                rnNota.RemovePorMatriculaParaFechamento(ctx, histMatricula.Aluno, histMatricula.Disciplina, histMatricula.Turma, histMatricula.Ano, histMatricula.Semestre);

                //Grava faltas no histórico
                rnFaltaHistmatr.InserePorMatriculaParaFechamento(ctx, histMatricula);

                //Deleta faltas do aluno
                rnFalta.RemovePorMatriculaParaFechamento(ctx, histMatricula.Aluno, histMatricula.Disciplina, histMatricula.Turma, histMatricula.Ano, histMatricula.Semestre);

                //Lista os docentes que tem aula para aquela disciplina, turma, ano, semestre
                listaDocentes = rnAulaDocente.ObtemDocentesEmAulaPor(histMatricula.Disciplina, histMatricula.Turma, histMatricula.Ano, histMatricula.Semestre, histMatricula.Aluno);

                foreach (decimal item in listaDocentes)
                {
                    // Insere cada docente no histórico de docentes (LY_HISTORICO_DOCENTE), caso nao exista
                    if (!rnHistoricoDocente.PossuiHistoricoCadastradoPor(histMatricula.Aluno, histMatricula.Ordem, histMatricula.Ano, histMatricula.Semestre, histMatricula.Disciplina, Convert.ToDecimal(item)))
                    {
                        historicoDocente.Aluno = histMatricula.Aluno;
                        historicoDocente.Ano = histMatricula.Ano;
                        historicoDocente.Disciplina = histMatricula.Disciplina;
                        historicoDocente.NumFunc = Convert.ToDecimal(item);
                        historicoDocente.Ordem = histMatricula.Ordem;
                        historicoDocente.Periodo = histMatricula.Semestre;

                        rnHistoricoDocente.Insere(ctx, historicoDocente);
                    }
                }

                // Remove os registros da matrícula da disciplina / turma já transferida para o histórico.
                rnMatricula.RemoveParaFechamento(ctx, matricula);
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
        }

        public ValidacaoDados ValidaDependencia(List<string> alunos, int totalDisciplinaSelecionadas, int qtdeDependenciaPermitida)
        {
            DataContext contexto = null;
            List<string> mensagens = new List<string>();
            DataTable dtDependencia = new DataTable();

            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (alunos.Count == 0)
            {
                return validacaoDados;
            }

            try
            {
                mensagens.Add("Favor verificar o(s) aluno(s) abaixo:");

                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                foreach (string aluno in alunos)
                {
                    dtDependencia = null;
                    dtDependencia = ObtemDependenciaPor(contexto, aluno);
                    if (dtDependencia.Rows.Count > 0)
                    {
                        if ((Convert.ToInt32(dtDependencia.Rows[0]["QUANTIDADE_DEPENDENCIAS_ABERTAS"]) + totalDisciplinaSelecionadas) > qtdeDependenciaPermitida)
                        {
                            mensagens.Add(dtDependencia.Rows[0]["NOME_COMPL"] + " possui " + dtDependencia.Rows[0]["QUANTIDADE_DEPENDENCIAS_ABERTAS"].ToString() + " dependência(s) a cumprir e " + totalDisciplinaSelecionadas + " dependência(s) selecionada(s).");
                        }
                    }
                }

                if (mensagens.Count > 1)
                {
                    validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
                }
                else
                {
                    validacaoDados.Valido = true;
                }

            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }

            return validacaoDados;
        }

        public DataTable ObtemDependenciaPor(DataContext ctx, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            DataTable dtDependencia = new DataTable();

            contextQuery.Command = @" SELECT A.ALUNO, PE.NOME_COMPL, QUANTIDADE_DEPENDENCIAS_ABERTAS
                        FROM    fechamentoano.VW_DEPENDENCIASABERTASPORALUNO D
                        INNER JOIN LY_ALUNO A ON A.ALUNO=D.ALUNO  
                        INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA             
                                WHERE D.ALUNO = @ALUNO";

            contextQuery.Parameters.Add("@ALUNO", aluno);

            dtDependencia = ctx.GetDataTable(contextQuery);

            return dtDependencia;
        }



        public static bool ExisteHistoricoDocente(TConnectionWritable cn, string aluno, decimal? ordem, decimal? ano, decimal? periodo, string disciplina, decimal? num_func)
        {
            var retorno = ExecutarFuncao(
                "SELECT 1 FROM LY_HISTORICO_DOCENTE  (nolock) WHERE ALUNO = ? and ORDEM = ? and ANO = ? and PERIODO = ? and DISCIPLINA = ? and NUM_FUNC = ?",
                cn,
                aluno,
                ordem,
                ano,
                periodo,
                disciplina,
                num_func);

            return retorno == 1;
        }

        public static bool ExisteHistoricoMatricula(TConnectionWritable cn, string aluno, decimal? ordem, decimal? ano, decimal? semestre, string disciplina)
        {
            var retorno = ExecutarFuncao(
                "SELECT 1 FROM Ly_histmatricula  (nolock) WHERE ALUNO = ? and ORDEM = ? and ANO = ? and SEMESTRE = ? and DISCIPLINA = ?",
                cn,
                aluno,
                ordem,
                ano,
                semestre,
                disciplina);

            return retorno == 1;
        }

        public static DataTable ListarAlunosMatriculados(string ano, string semestre, string curso, string turno, string curriculo, string serie, string turma)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable alunos = null;

            try
            {
                contextQuery.Command = @"     DECLARE @PER_PERIODO TABLE ( PERIODO INT )

                                              IF ( @SEMESTRE = 1 ) 
	                                            BEGIN
		                                            INSERT  @PER_PERIODO
				                                            ( PERIODO )
		                                            VALUES  ( 2 )                               
	                                            END
                                              ELSE 
	                                            BEGIN
		                                            INSERT  @PER_PERIODO
				                                            ( PERIODO )
		                                            VALUES  ( 0 )
		                                            INSERT  @PER_PERIODO
				                                            ( PERIODO )
		                                            VALUES  ( 1 )
	                                            END

                                     SELECT DISTINCT
                                        a.aluno ,
                                        m.num_chamada ,
                                        PE.nome_compl ,
                                        m.sit_matricula,
										c.curso + ' - ' + C.NOME AS CURSO
                                    FROM  LY_MATRICULA M ( NOLOCK )
                                    INNER JOIN LY_ALUNO A ( NOLOCK ) ON A.ALUNO = M.ALUNO
                                    INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA
                                    INNER JOIN LY_TURMA T ( NOLOCK ) ON T.DISCIPLINA = M.DISCIPLINA
                                                                                AND T.TURMA = M.TURMA
                                                                                AND T.ANO = M.ANO
                                                                                AND T.SEMESTRE = M.SEMESTRE

                                    INNER JOIN LY_PERIODO_LETIVO PL ON PL.ANO = M.ANO AND PL.PERIODO = M.SEMESTRE
									LEFT JOIN TCE_CONFIRMACAO_MATRICULA (NOLOCK) CM ON CM.ALUNO = M.ALUNO
																						AND CM.ANO = PL.PER_ANO
																						AND CM.PERIODO IN (SELECT PERIODO FROM @PER_PERIODO)
                                                                                        AND CM.CENSO = T.FACULDADE
																						AND CM.STATUS = 'Confirmado'  
									LEFT join LY_CURSO C (NOLOCK) ON C.CURSO = CM.CURSO
                                    WHERE  SIT_MATRICULA = 'Matriculado'
                                        AND ISNULL(M.DEPENDENCIA, 'N') = 'N'
                                        AND ISNULL(T.ELETIVA, 'N') = 'N'
                                        AND T.ANO = @ANO
                                        AND T.SEMESTRE = @SEMESTRE
                                        AND T.CURSO = @CURSO
                                        AND T.TURNO = @TURNO
                                        AND T.CURRICULO = @CURRICULO
                                        AND T.SERIE = @SERIE
                                        AND T.TURMA = @TURMA
                                    ORDER BY PE.NOME_COMPL  ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURRICULO", curriculo);
                contextQuery.Parameters.Add("@SERIE", Convert.ToDecimal(serie));
                contextQuery.Parameters.Add("@TURMA", turma);

                alunos = ctx.GetDataTable(contextQuery);
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

            return alunos;
        }

        public static DataTable ListarHistoricoAlunos(string ano, string semestre, string curso, string turno, string curriculo, string serie, string turma)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable alunos = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                h.aluno ,                               
                                PE.nome_compl ,
                                f.SITUACAO_FINAL AS situacao_hist
                        FROM    LY_HISTMATRICULA h ( NOLOCK )
                                INNER JOIN LY_ALUNO a ( NOLOCK ) ON h.ALUNO = a.ALUNO
                                INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA
                                INNER JOIN LY_TURMA t ( NOLOCK ) ON t.DISCIPLINA = h.DISCIPLINA
                                                                    AND t.TURMA = h.TURMA
                                                                    AND t.ANO = h.ANO
                                                                    AND t.SEMESTRE = h.SEMESTRE
                                INNER JOIN dbo.TCE_SITUACAO_FINAL_ALUNO f ( NOLOCK ) ON h.ALUNO = f.ALUNO
                                                                                      AND h.ANO = f.ANO
                                                                                      AND h.TURMA = f.TURMA
                                                                                      AND h.SEMESTRE = f.PERIODO
                        WHERE   f.ano = @ANO
                                AND f.PERIODO = @SEMESTRE
                                AND t.curso = @CURSO
                                AND t.TURNO = @TURNO
                                AND t.CURRICULO = @CURRICULO
                                AND t.SERIE = @SERIE
                                AND t.TURMA = @TURMA
                        ORDER BY PE.NOME_COMPL ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURRICULO", curriculo);
                contextQuery.Parameters.Add("@SERIE", Convert.ToDecimal(serie));
                contextQuery.Parameters.Add("@TURMA", turma);

                alunos = ctx.GetDataTable(contextQuery);
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

            return alunos;
        }

        private void MatriculaAluno(DataContext ctx, string aluno, string ano, string periodo, string curso, string turno, string curriculo, string serie, string turma, string unidadeDest, int tipoAprovacao, string matriculaResponsavel, string periodoOrigem)
        {
            try
            {
                LyMatricula matricula = new LyMatricula();
                LyMatGrade matgrade = new LyMatGrade();
                Matricula rnMatricula = new Matricula();
                Aluno rnAluno = new Aluno();
                LyAluno lyAluno = new LyAluno();
                decimal gradeId = 0;

                //Monta entidade da matricula
                matricula.Aluno = aluno;
                matricula.Ano = Convert.ToDecimal(ano);
                matricula.Semestre = Convert.ToDecimal(periodo);
                matricula.CobrancaSep = "N";
                matricula.SitMatricula = RN.Matricula.Matriculado;
                matricula.Turma = turma;
                matricula.Matricula = matriculaResponsavel;

                //Somente serão enturmadas matriculas regulares pela tela de fechamento
                matricula.Dependencia = "N";
                matricula.Concomitante = "N";
                matricula.EducEspecial = "N";
                matricula.MaisEducacao = "N";

                //Busca gradeId para enturmação
                gradeId = Convert.ToDecimal(GradeSerie.ObterGradeId(Convert.ToDecimal(ano), Convert.ToDecimal(periodo), curso, curriculo, Convert.ToDecimal(serie), turma));

                //Monta entidade da matgrade
                matgrade.Aluno = aluno;
                matgrade.DtUltalt = DateTime.Now;
                matgrade.SitMatgrade = "Matriculado";
                matgrade.GradeId = gradeId;

                rnMatricula.IncluiMatriculaFechamento(ctx, matricula, matgrade, tipoAprovacao, matriculaResponsavel, periodoOrigem);

                //Monta entidade da aluno
                lyAluno.Aluno = aluno;
                lyAluno.Curso = curso;
                lyAluno.Turno = turno;
                lyAluno.Serie = Convert.ToDecimal(serie);
                lyAluno.Curriculo = curriculo;

                //Atualiza dados do aluno
                rnAluno.AtualizaMatriculaFechamento(ctx, lyAluno);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public static ICollection<DadosAlunoFechamento> ListarAlunosMatriculadosCursosEspeciais(string ano, string alunos, string turmaFechamento)
        {
            ICollection<DadosAlunoFechamento> listaAlunos = new Collection<DadosAlunoFechamento>();

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                           @"SELECT DISTINCT
                            m.ALUNO ,
                            PE.NOME_COMPL ,
                            m.TURMA
                    FROM    dbo.LY_MATRICULA m
                            INNER JOIN dbo.LY_TURMA t ON m.TURMA = t.TURMA
                                                         AND m.ANO = t.ANO
                                                         AND m.SEMESTRE = t.SEMESTRE
                                                         AND m.DISCIPLINA = t.DISCIPLINA
                            INNER JOIN dbo.LY_ALUNO a ON a.ALUNO = m.ALUNO
                            INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA
                    WHERE   M.ANO = @ANO
                            AND SIT_MATRICULA = 'Matriculado'
                            AND (M.TURMA <> @TURMA_FECHAMENTO --VERRIFICA MATRICULAS ESPECIAIS EM OUTRAS TURMAS
                                    AND (OPTATIVAREFORCO <> 'N'
                                        OR ISNULL(M.DEPENDENCIA, 'N') <> 'N'
                                        OR ISNULL(M.MAIS_EDUCACAO, 'N') <> 'N'
                                        OR ISNULL(M.EDUC_ESPECIAL, 'N') <> 'N'
                                        OR ISNULL(T.ELETIVA, 'N') <> 'N'
                                        OR ISNULL(M.CONCOMITANTE, 'N') <> 'N')
                                OR (M.TURMA = @TURMA_FECHAMENTO AND ISNULL(T.ELETIVA, 'N') <> 'N') --OU ELETIVA NA MSM TURMA
                            )

                            AND m.ALUNO IN (" + alunos + ")"
                };

                contextQuery.Command += "ORDER BY m.ALUNO , PE.NOME_COMPL ,m.TURMA";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@TURMA_FECHAMENTO", turmaFechamento);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    while (reader.Read())
                    {
                        var fechamento = new DadosAlunoFechamento
                        {
                            Aluno = Convert.ToString(reader["ALUNO"]),
                            NomeAluno = Convert.ToString(reader["NOME_COMPL"]),
                            Turma = Convert.ToString(reader["TURMA"])
                        };

                        listaAlunos.Add(fechamento);
                    }
                }
            }

            return listaAlunos;
        }

        public static QueryTable ListarAlunosProgressaoParcial(string ano, string semestre, string curso, string turno, string curriculo, string serie, string turma)
        {
            var qt = Consultar(
                @"select Distinct a.aluno,
                m.num_chamada,
                PE.nome_compl,
                mg.sit_matgrade,
                m.DISCIPLINA,
                d.NOME AS nomeDisciplina,
                (a.aluno + '_' + m.disciplina) AS CHAVE

                from 
                LY_MATGRADE mg 
                inner join LY_GRADE_SERIE gs on gs.GRADE_ID = mg.GRADE_ID
                inner join LY_ALUNO a on a.ALUNO = mg.ALUNO
                INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA
                inner join ly_grade_turma gt on mg.grade_id = gt.grade_id 
                inner join ly_matricula m on m.ALUNO = mg.ALUNO and gt.disciplina = m.disciplina and gt.turma = m.turma and gt.ano = m.ano and gt.semestre = m.semestre 
                inner join ly_disciplina d on m.disciplina = d.disciplina
                where SIT_MATRICULA = 'Matriculado'
                AND m.DEPENDENCIA='S'
                and gs.ANO = ?
                and gs.SEMESTRE = ?
                and gs.CURSO = ?
                and gs.TURNO = ?
                and gs.CURRICULO = ?
                and gs.SERIE = ?
                and gs.GRADE = ?
                order by PE.NOME_COMPL",
                Convert.ToDecimal(ano),
                Convert.ToDecimal(semestre),
                curso,
                turno,
                curriculo,
                Convert.ToDecimal(serie),
                turma);

            return qt;
        }

        public static QueryTable ListarAlunosEletivas(string ano, string semestre, string turma)
        {
            var qt = Consultar(@"select Distinct a.aluno,
                m.num_chamada,
                PE.nome_compl,
                mg.sit_matgrade,
                d.DISCIPLINA,
                d.NOME AS nomeDisciplina,
                (a.aluno + '_' + m.disciplina) AS CHAVE,
                T.DISCIPLINA AS DISCIPLINAORIGINAL
 from LY_MATGRADE mg 
                inner join LY_GRADE_SERIE gs on gs.GRADE_ID = mg.GRADE_ID
                inner join LY_ALUNO a on a.ALUNO = mg.ALUNO
                INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA
                inner join ly_grade_turma gt on mg.grade_id = gt.grade_id 
                inner join ly_matricula m on m.ALUNO = mg.ALUNO and gt.disciplina = m.disciplina and gt.turma = m.turma and gt.ano = m.ano and gt.semestre = m.semestre 
                inner join LY_TURMA t on m.ANO = t.ANO and m.SEMESTRE = t.SEMESTRE and m.TURMA = t.TURMA and m.DISCIPLINA = t.DISCIPLINA
				inner join ly_disciplina d on isnull(t.DISCIPLINA_MULTIPLA, t.disciplina) = d.disciplina
                  where SIT_MATRICULA = 'Matriculado'
                AND isnull(t.ELETIVA, 'n') ='S'
                and m.ANO = ?
                and m.SEMESTRE = ?
                and m.TURMA = ?
                order by PE.NOME_COMPL     ",
                Convert.ToDecimal(ano),
                Convert.ToDecimal(semestre),
                turma);

            return qt;
        }

        public static ICollection<DadosAlunoFechamento> ListarAlunosMatriculadosConcomitante(string ano, string alunos, string turmaFechamento, string semestre)
        {
            ICollection<DadosAlunoFechamento> listaAlunos = new Collection<DadosAlunoFechamento>();

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                           @"SELECT DISTINCT
                            m.ALUNO ,
                            PE.NOME_COMPL ,
                            m.TURMA
                    FROM    dbo.LY_MATRICULA m
                            INNER JOIN dbo.LY_TURMA t ON m.TURMA = t.TURMA
                                                         AND m.ANO = t.ANO
                                                         AND m.SEMESTRE = t.SEMESTRE
                                                         AND m.DISCIPLINA = t.DISCIPLINA
                            INNER JOIN dbo.LY_ALUNO a ON a.ALUNO = m.ALUNO
                            INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA
                    WHERE   M.ANO = @ANO
                            AND SIT_MATRICULA = 'Matriculado'
                            AND m.tURMA = @TURMA_FECHAMENTO
                            AND m.SEMESTRE = @SEMESTRE
                            AND  ISNULL(m.CONCOMITANTE, 'N') <> 'N'
                            AND m.ALUNO IN (" + alunos + ")"
                };

                contextQuery.Command += "ORDER BY m.ALUNO , PE.NOME_COMPL ,m.TURMA";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@TURMA_FECHAMENTO", turmaFechamento);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    while (reader.Read())
                    {
                        var fechamento = new DadosAlunoFechamento
                        {
                            Aluno = Convert.ToString(reader["ALUNO"]),
                            NomeAluno = Convert.ToString(reader["NOME_COMPL"]),
                            Turma = Convert.ToString(reader["TURMA"])
                        };

                        listaAlunos.Add(fechamento);
                    }
                }
            }

            return listaAlunos;
        }


        public void PromoveAlunosComEnturmacao(Turma.DadosTurma dadosTurma, List<string> alunos, string anoDest, string periodoDest, string cursoDest, string turnoDest, string curriculoDest, string serieDest, string turmaDest, string matricula, string unidadeDest)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            List<DadosDisciplinaParaFechamento> listaDisciplinas = new List<DadosDisciplinaParaFechamento>();
            Disciplina rnDisciplina = new Disciplina();
            TceTransferencia transferencia = new TceTransferencia();
            TceTransferenciaOrigem transferenciaOrigem = new TceTransferenciaOrigem();
            TceTransferenciaDestino transferenciaDestino = new TceTransferenciaDestino();
            Transferencia rnTransferencia = new Transferencia();
            Matricula rnMatricula = new Matricula();
            Aluno rnAluno = new Aluno();
            RN.Aluno.DadosAluno dadosAluno = new Aluno.DadosAluno();
            ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            int vagas = 0;

            int tipoAprovacao = 2; //1 = Reprovado; 2 = Aprovado

            try
            {
                //VERIFICA SE A TURMA DESTINO TEM VAGA
                vagas = RN.Turma.RetornaVagas(Convert.ToInt32(anoDest), Convert.ToInt32(periodoDest), turmaDest);

                if (vagas < alunos.Count)
                {
                    throw new Exception("ERRO_VALIDACAO:A capacidade da turma desejada não comporta mais alunos.");
                }

                string situacaoFinal = Promovido;

                //Lista as disciplinas da turma
                listaDisciplinas = rnDisciplina.ObtemDisciplinasParaFechamentoPor(Convert.ToInt32(dadosTurma.Grade_ID));

                foreach (string aluno in alunos)
                {
                    if (rnMatricula.EhMatriculaConcomitante(aluno, Convert.ToInt32(dadosTurma.Ano), Convert.ToInt32(dadosTurma.Periodo), dadosTurma.Grade))
                    {
                        dadosAluno = rnAluno.ObtemDadosAluno(aluno);

                        throw new Exception(String.Format("ERRO_VALIDACAO:O aluno: {0} - {1} está cursando Educação Profissional Concomitante nesta turma, por isso não pode ser enturmado.", dadosAluno.Aluno, dadosAluno.Nome_compl));
                    }
                    //Ao finalizar o aluno gerar confirmação de matricula atraves da renovação
                    rnConfirmacaoMatricula.GeraConfirmacaoMatriculaDeRenovacaoComEnturmacaoPor(ctx, dadosTurma, situacaoFinal, aluno, Convert.ToDecimal(anoDest), Convert.ToDecimal(periodoDest), cursoDest, turnoDest, curriculoDest, Convert.ToDecimal(serieDest), unidadeDest, matricula);

                    this.FinalizaAluno(ctx, dadosTurma, aluno, null, matricula, situacaoFinal, listaDisciplinas, Convert.ToDecimal(anoDest), Convert.ToDecimal(periodoDest));

                    //Enturmacao
                    if (dadosTurma.UnidadeResponsavel == unidadeDest)
                    {
                        //Se a enturmacao for para a mesma unidade de ensino
                        this.MatriculaAluno(ctx, aluno, anoDest, periodoDest, cursoDest, turnoDest, curriculoDest, serieDest, turmaDest, unidadeDest, tipoAprovacao, matricula, dadosTurma.Periodo);
                    }
                    else
                    {
                        //Caso troque de escola criar transferencia
                        GeraTransferencia(ctx, dadosTurma, aluno, anoDest, periodoDest, cursoDest, turnoDest, curriculoDest, serieDest, turmaDest, matricula, unidadeDest);
                    }
                }
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();

                //Trata mensagens de erros de validacao no processo de enturmacao
                if (Convert.ToString(ex.Message).Contains("ERRO_VALIDACAO:"))
                {
                    mensagem = Convert.ToString(ex.Message).Replace("ERRO_VALIDACAO:", string.Empty);
                }
                else
                {
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
                }

                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public void PromoveAlunos(Turma.DadosTurma dadosTurma, List<string> alunos, string matricula, decimal proximoAno, decimal proximoPeriodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            List<DadosDisciplinaParaFechamento> listaDisciplinas = new List<DadosDisciplinaParaFechamento>();
            Disciplina rnDisciplina = new Disciplina();
            ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();

            try
            {
                string situacaoFinal = Promovido;

                //Lista as disciplinas da turma
                listaDisciplinas = rnDisciplina.ObtemDisciplinasParaFechamentoPor(Convert.ToInt32(dadosTurma.Grade_ID));

                foreach (var aluno in alunos)
                {
                    //Ao finalizar o aluno gerar confirmação de matricula atraves da renovação
                    rnConfirmacaoMatricula.GeraConfirmacaoMatriculaDeRenovacaoPor(ctx, dadosTurma, situacaoFinal, aluno, proximoAno, proximoPeriodo, matricula);

                    this.FinalizaAluno(ctx, dadosTurma, aluno, null, matricula, situacaoFinal, listaDisciplinas, proximoAno, proximoPeriodo);
                }
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();

                //Trata mensagens de erros de validacao no processo de enturmacao
                if (Convert.ToString(ex.Message).Contains("ERRO_VALIDACAO:"))
                {
                    mensagem = Convert.ToString(ex.Message).Replace("ERRO_VALIDACAO:", string.Empty);
                }
                else
                {
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
                }

                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }
        public void RetemAlunosPorNotaComEnturmacao(Turma.DadosTurma dadosTurma, List<string> alunos, string anoDest, string periodoDest, string cursoDest, string turnoDest, string curriculoDest, string serieDest, string turmaDest, string matricula, string unidadeDest)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            List<DadosDisciplinaParaFechamento> listaDisciplinas = new List<DadosDisciplinaParaFechamento>();
            Disciplina rnDisciplina = new Disciplina();
            TceTransferencia transferencia = new TceTransferencia();
            TceTransferenciaOrigem transferenciaOrigem = new TceTransferenciaOrigem();
            TceTransferenciaDestino transferenciaDestino = new TceTransferenciaDestino();
            Transferencia rnTransferencia = new Transferencia();
            int vagas = 0;
            ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            int tipoAprovacao = 1; //1 = Reprovado; 2 = Aprovado

            try
            {
                //VERIFICA SE A TURMA DESTINO TEM VAGA
                vagas = RN.Turma.RetornaVagas(Convert.ToInt32(anoDest), Convert.ToInt32(periodoDest), turmaDest);

                if (vagas < alunos.Count)
                {
                    throw new Exception("ERRO_VALIDACAO:A capacidade da turma desejada não comporta mais alunos.");
                }

                string situacaoFinal = Retido;

                //Lista as disciplinas da turma
                listaDisciplinas = rnDisciplina.ObtemDisciplinasParaFechamentoPor(Convert.ToInt32(dadosTurma.Grade_ID));

                foreach (string aluno in alunos)
                {
                    //Ao finalizar o aluno gerar confirmação de matricula atraves da renovação
                    rnConfirmacaoMatricula.GeraConfirmacaoMatriculaDeRenovacaoComEnturmacaoPor(ctx, dadosTurma, situacaoFinal, aluno, Convert.ToDecimal(anoDest), Convert.ToDecimal(periodoDest), cursoDest, turnoDest, curriculoDest, Convert.ToDecimal(serieDest), unidadeDest, matricula);

                    this.FinalizaAluno(ctx, dadosTurma, aluno, null, matricula, situacaoFinal, listaDisciplinas, Convert.ToDecimal(anoDest), Convert.ToDecimal(periodoDest));

                    //Enturmacao
                    if (dadosTurma.UnidadeResponsavel == unidadeDest)
                    {
                        //Se a enturmacao for para a mesma unidade de ensino
                        this.MatriculaAluno(ctx, aluno, anoDest, periodoDest, cursoDest, turnoDest, curriculoDest, serieDest, turmaDest, unidadeDest, tipoAprovacao, matricula, dadosTurma.Periodo);
                    }
                    else
                    {
                        //Caso troque de escola criar transferencia
                        GeraTransferencia(ctx, dadosTurma, aluno, anoDest, periodoDest, cursoDest, turnoDest, curriculoDest, serieDest, turmaDest, matricula, unidadeDest);
                    }
                }
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();

                //Trata mensagens de erros de validacao no processo de enturmacao
                if (Convert.ToString(ex.Message).Contains("ERRO_VALIDACAO:"))
                {
                    mensagem = Convert.ToString(ex.Message).Replace("ERRO_VALIDACAO:", string.Empty);
                }
                else
                {
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
                }

                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public void RetemAlunos(Turma.DadosTurma dadosTurma, List<string> alunos, string matricula, decimal proximoAno, decimal proximoPeriodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            List<DadosDisciplinaParaFechamento> listaDisciplinas = new List<DadosDisciplinaParaFechamento>();
            Disciplina rnDisciplina = new Disciplina();
            ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();

            try
            {
                string situacaoFinal = Retido;

                //Lista as disciplinas da turma
                listaDisciplinas = rnDisciplina.ObtemDisciplinasParaFechamentoPor(Convert.ToInt32(dadosTurma.Grade_ID));

                foreach (var aluno in alunos)
                {
                    //Ao finalizar o aluno gerar confirmação de matricula atraves da renovação
                    rnConfirmacaoMatricula.GeraConfirmacaoMatriculaDeRenovacaoPor(ctx, dadosTurma, situacaoFinal, aluno, proximoAno, proximoPeriodo, matricula);

                    this.FinalizaAluno(ctx, dadosTurma, aluno, null, matricula, situacaoFinal, listaDisciplinas, proximoAno, proximoPeriodo);
                }
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();

                //Trata mensagens de erros de validacao no processo de enturmacao
                if (Convert.ToString(ex.Message).Contains("ERRO_VALIDACAO:"))
                {
                    mensagem = Convert.ToString(ex.Message).Replace("ERRO_VALIDACAO:", string.Empty);
                }
                else
                {
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
                }

                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public int ObtemDependenciasPermitidasPor(string curso, int serie)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            int qtdeDEP = 0;

            try
            {

                contextQuery.Command = @" SELECT  QTDE_REPROV
                                FROM    EXTRACOES..CURSO_PARTICIPANTES_FECH_2025 GS (NOLOCK)
                                WHERE   CURSO = @CURSO
                                        AND SERIE = @SERIE";

                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@SERIE", serie);

                qtdeDEP = ctx.GetReturnValue(contextQuery) == null ? 0 : ctx.GetReturnValue<int>(contextQuery); 

                return qtdeDEP;
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