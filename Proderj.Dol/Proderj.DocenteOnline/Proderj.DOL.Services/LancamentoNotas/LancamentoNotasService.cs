using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.DOL.Exception;
using Proderj.DOL.Repository;
using Proderj.DOL.Domain;
using Proderj.Foundation.Common;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Service;

namespace Proderj.DOL.Service
{
    public class LancamentoNotasService : ILancamentoNotasService
    {
        private const float PERCENTUAL_MINIMO_PRESENCA = 40;

        private ILancamentoNotasRepository repositorioLancamentoNotas;
        private INotaRepository repositorioNota;
        private IFaltaRepository repositorioFalta;
        private ILogNotaRepository repositorioLogNota;
        private IFrequenciaRepository repositorioFrequencia;
        private IProtocoloNotaRepository repositorioProtocolo;
        private IDeclaracaoSemNotaRepository repositorioDeclaracaoSemNota;
        private IDisciplinaRepository repositorioDisciplina;
        private ILancamentoNotasConsolidadoRepository repositorioLancamentoNotasConsolidado;
        private INotaConsolidadoRepository repositorioNotaConsolidado;

        public LancamentoNotasService(ILancamentoNotasRepository repositorioLancamentoNotas, INotaRepository repositorioNota,
            IFaltaRepository repositorioFalta, ILogNotaRepository repositorioLogNota, IFrequenciaRepository repositorioFrequencia,
            IProtocoloNotaRepository repositorioProtocolo, IDeclaracaoSemNotaRepository repositorioDeclaracaoSemNota, 
            IDisciplinaRepository repositorioDisciplina, ILancamentoNotasConsolidadoRepository repositorioLancamentoNotasConsolidado,
            INotaConsolidadoRepository repositorioNotaConsolidado)
        {
            this.repositorioLancamentoNotas = repositorioLancamentoNotas;
            this.repositorioNota = repositorioNota;
            this.repositorioFalta = repositorioFalta;
            this.repositorioLogNota = repositorioLogNota;
            this.repositorioFrequencia = repositorioFrequencia;
            this.repositorioProtocolo = repositorioProtocolo;
            this.repositorioDeclaracaoSemNota = repositorioDeclaracaoSemNota;
            this.repositorioDisciplina = repositorioDisciplina;
            this.repositorioLancamentoNotasConsolidado = repositorioLancamentoNotasConsolidado;
            this.repositorioNotaConsolidado = repositorioNotaConsolidado;
        }

        public void VerificaPermissaoParaLancarNota(long numeroFuncionarioDocente, DTOLancamentoNotasSolicitacao dtoSolicitacaoParaLancamento)
        {
            //Consumir o serviço de turma

            var fabricaServico = new NinjectFactoryBase<NinjectModuloServico>();
            SelecaoTurmasService servicoDeTurma = fabricaServico.Obtem<SelecaoTurmasService>();

            List<DTOSelecaoTurmas> listaTurmasComAcesso = servicoDeTurma.EnumeraSelecaoTurmasPor(numeroFuncionarioDocente).ToList();

            //Verifica se os dados de solicitação de lançamento de notas existem no escopo deste docente
            DTOSelecaoTurmas turmaComAcesso = listaTurmasComAcesso.FirstOrDefault(turmas =>
                                            turmas.Serie == dtoSolicitacaoParaLancamento.Serie &&
                                            turmas.Semestre == dtoSolicitacaoParaLancamento.Periodo &&
                                            turmas.Ano == dtoSolicitacaoParaLancamento.Ano &&
                                            turmas.Curso == dtoSolicitacaoParaLancamento.CodigoCurso &&
                                            turmas.Disciplina == dtoSolicitacaoParaLancamento.CodigoDisciplina &&
                                            turmas.Modalidade == dtoSolicitacaoParaLancamento.CodigoModalidade &&
                                            turmas.UnidadeEnsino == dtoSolicitacaoParaLancamento.CodigoUnidadeEnsino &&
                                            turmas.Turma == dtoSolicitacaoParaLancamento.CodigoTurma &&
                                            turmas.Tipo == dtoSolicitacaoParaLancamento.TipoCurso
                                            );

            //Se tiver acesso, verificar se ainda pode lançar nota...
            if (turmaComAcesso == null)
            {
                throw new LancamentoNotasException(LancamentoNotasException.TipoEnum.AcessoNegadoDocente_A_TurmaEDisciplina);
            }

            if (!turmaComAcesso.ValidoParaLancamento)
            {
                var excecao = new LancamentoNotasException(LancamentoNotasException.TipoEnum.TurmaEDisciplinaInvalidaParaLancamento)
                {
                    CodigoTurma = dtoSolicitacaoParaLancamento.CodigoTurma
                };

                throw excecao;
            }
        }

        public bool ExisteNotaPendenteParaLancamentoEmBimestreAnteriorAoAtualPor(short ano, short periodo, short subperiodoAtual, string disciplina, string turma)
        {
            return repositorioLancamentoNotas.ExisteNotaPendenteParaLancamentoEmBimestreAnteriorAoAtualPor(ano, periodo, subperiodoAtual, disciplina, turma);
        }

        public bool PodeLancarNotaNaTurma(string disciplina, string turma, short ano, short periodo, short subperiodo)
        {
            var fabricaServico = new NinjectFactoryBase<NinjectModuloServico>();
            TurmaService servicoDeProva = fabricaServico.Obtem<TurmaService>();

            DTOProvaTurma dtoProvaTurma = servicoDeProva.ObtemProvaDaTurmaPor(disciplina, turma, ano, periodo, subperiodo);

            bool existeProvaParaEstaTurma = (dtoProvaTurma != null) && dtoProvaTurma.TipoProva != null;

            return existeProvaParaEstaTurma;
        }

        public IList<DTOItemJustificativa> ListarItemJustificativa()
        {
            return Enum
                .GetValues(typeof(MotivoSemNotaEnum))
                .Cast<MotivoSemNotaEnum>()
                .Select(i => new DTOItemJustificativa
                {
                    Codigo = (short)i,
                    Descricao = i.GetDescription()
                })
                .ToList();
        }

        /// <summary>
        /// Lista as notas lançadas.
        /// </summary>
        /// <param name="disciplina"></param>
        /// <param name="turma"></param>
        /// <param name="ano"></param>
        /// <param name="periodo"></param>
        /// <param name="subperiodo"></param>
        /// <returns></returns>
        public List<DTOItemLancamentoNotaFrequenciaAluno> ListaLancamentoNotaFrequenciaAlunoPor(string disciplina, string turma, short ano, short periodo, short subperiodo)
        {

            List<LancamentoNotas> listaLancamentos = repositorioLancamentoNotas.EnumeraLancamentosPor(disciplina, turma, ano, periodo, subperiodo).ToList();
            if (listaLancamentos.Count == 0)
                throw new LancamentoNotasException(LancamentoNotasException.TipoEnum.NaoHaAlunosMatriculados);

            List<DTOItemLancamentoNotaFrequenciaAluno> listaDtoLancamentoNotaFrequenciaAluno = new List<DTOItemLancamentoNotaFrequenciaAluno>();

            foreach (var lancamento in listaLancamentos)
            {
                var l = new DTOItemLancamentoNotaFrequenciaAluno();
                l.Id = lancamento.NotaId;
                l.NotaProva = lancamento.MediaNota;
                l.Faltas = lancamento.Faltas;
                l.Situacao = lancamento.SituacaoMatricula;
                l.DescricaoSituacao = lancamento.DescricaoSituacao;
                l.RecuperacaoParalela = lancamento.RecuperacaoPararela;
                l.SemAvaliacao = lancamento.SemAvaliacao;
                l.Nome = lancamento.NomeCompleto;
                l.CodigoJustificativa = lancamento.MotivoSemNota.HasValue ? lancamento.MotivoSemNota.ToString() : null;

                if (lancamento.MediaNota == null && !lancamento.SemAvaliacao && lancamento.PossuiLicenca) // lançamento novo
                {
                    l.SemAvaliacao = true;
                    l.CodigoJustificativa = ((int)MotivoSemNotaEnum.AfastamentoMedicoMaternidadeServicoMilitar).ToString();
                }

                l.SomenteLeitura = (lancamento.SituacaoMatricula != "Matriculado" ||
                                    !String.IsNullOrEmpty(lancamento.Formula));
                l.Codigo = lancamento.MatriculaAluno;
                l.Nota = lancamento.NotaProva;
                l.NotaRecuperacao = lancamento.NotaRecuperacao;
                l.PossuiLicenca = lancamento.PossuiLicenca;
                l.ExibeMensagemAfastamentoMedico = !lancamento.PossuiLicenca;
                l.PresencaMinima = PERCENTUAL_MINIMO_PRESENCA;

                listaDtoLancamentoNotaFrequenciaAluno.Add(l);
            }

            return listaDtoLancamentoNotaFrequenciaAluno;
        }

        #region Salvamento de Lancamento de Notas

        public List<DTOItemLancamentoNotaFrequenciaAluno> AtualizaEVerificaListaDeAlunosExistentesComAlunosEnviadosPeloProfessor(List<DTOItemLancamentoNotaFrequenciaAluno> alunosDaTurma, List<DTOItemSalvaNotaFrequenciaAluno> alunosEnviadosPeloProfessor)
        {
            //Se houver qualquer aluno enviado pelo professor que não exista na turma, caracterizará violação de negócio
            List<string> listaMensagemPorAlunoInvalido = new List<string>();

            //para o professor nao perder os dados após salvar
            foreach (DTOItemSalvaNotaFrequenciaAluno dtoAlunoEnviadoProfessor in alunosEnviadosPeloProfessor)
            {

                DTOItemLancamentoNotaFrequenciaAluno dtoAlunoDoBanco =
                    alunosDaTurma.FirstOrDefault(
                        aluno => aluno.Codigo == dtoAlunoEnviadoProfessor.Codigo);

                if (dtoAlunoDoBanco != null)
                {
                    dtoAlunoDoBanco.Nota = dtoAlunoEnviadoProfessor.Nota;
                    dtoAlunoDoBanco.Faltas = dtoAlunoEnviadoProfessor.Faltas;
                    dtoAlunoDoBanco.SemAvaliacao = dtoAlunoEnviadoProfessor.SemAvaliacao;
                    dtoAlunoDoBanco.RecuperacaoParalela = dtoAlunoEnviadoProfessor.RecuperacaoParalela;
                    dtoAlunoDoBanco.CodigoJustificativa = dtoAlunoEnviadoProfessor.CodigoJustificativa;
                }
                else
                {
                    //Aluno que o professor está enviando nao existe na base
                    var excecaoLancamentoAluno = new LancamentoNotasException(LancamentoNotasException.TipoEnum.AlunoInvalidoParaLancamento) { NomeAluno = dtoAlunoEnviadoProfessor.Codigo };
                    listaMensagemPorAlunoInvalido.Add(excecaoLancamentoAluno.Message);
                }
            }

            if (listaMensagemPorAlunoInvalido.Count > 0)
            {
                string mensagemExcecaoAgrupada = String.Join("\n", listaMensagemPorAlunoInvalido.ToArray());
                throw new LancamentoNotasException(mensagemExcecaoAgrupada);
            }

            return alunosDaTurma;
        }


        public void VerificaPermissaoParaSalvarNota(DTOLancamentoNotasSalvamento dtoSolicitacaoParaLancamento, DTOProvaParaLancamento dtoProvaParaLancamento)
        {
            Disciplina disciplina = repositorioDisciplina.ObtemPorChavePrimaria(dtoSolicitacaoParaLancamento.CodigoDisciplina);
            //Se houver qualquer aluno enviado pelo professor que não exista na turma, caracterizará violação de negócio
            var lancamentoNotasExceptionLista = new List<LancamentoNotasException>();

            if (dtoSolicitacaoParaLancamento.DadosFrequenciaTurma.CodigoFrequencia.IsNullOrEmpty())
            {
                if (!dtoSolicitacaoParaLancamento.DadosFrequenciaTurma.AulasPrevistas.HasValue)
                {
                    var excecaoLancamentoAluno = new LancamentoNotasException(LancamentoNotasException.TipoEnum.NaoHaAulasPrevistas);
                    lancamentoNotasExceptionLista.Add(excecaoLancamentoAluno);
                }
                else if (dtoSolicitacaoParaLancamento.DadosFrequenciaTurma.AulasPrevistas.Value < 0)
                {
                    var excecaoLancamentoAluno = new LancamentoNotasException(LancamentoNotasException.TipoEnum.AulasPrevistasMenorQueZero);
                    lancamentoNotasExceptionLista.Add(excecaoLancamentoAluno);
                }

                if (!dtoSolicitacaoParaLancamento.DadosFrequenciaTurma.AulasDadas.HasValue)
                {
                    var excecaoLancamentoAluno = new LancamentoNotasException(LancamentoNotasException.TipoEnum.NaoHaAulasDadas);
                    lancamentoNotasExceptionLista.Add(excecaoLancamentoAluno);
                }
                else if (dtoSolicitacaoParaLancamento.DadosFrequenciaTurma.AulasPrevistas.Value < 0)
                {
                    var excecaoLancamentoAluno = new LancamentoNotasException(LancamentoNotasException.TipoEnum.AulasDadasMenorQueZero);
                    lancamentoNotasExceptionLista.Add(excecaoLancamentoAluno);
                }
            }
            if ((disciplina.TemNota == "S") && (dtoProvaParaLancamento == null))
            {
                var excecaoLancamentoAluno = new LancamentoNotasException(LancamentoNotasException.TipoEnum.SemProvaDaTurmaParaLancamento);
                lancamentoNotasExceptionLista.Add(excecaoLancamentoAluno);
            }

            bool jaPossuiDisciplinaNaoPossuiAulasDadasException = false;

            foreach (DTOItemSalvaNotaFrequenciaAluno itemLancamento in dtoSolicitacaoParaLancamento.ListaItemLancamentoNotaFrequenciaAluno)
            {
                if ((itemLancamento.Codigo.IsNullOrEmpty()))
                {
                    var excecaoLancamentoAluno = new LancamentoNotasException(LancamentoNotasException.TipoEnum.AlunoNaoMatriculado);
                    lancamentoNotasExceptionLista.Add(excecaoLancamentoAluno);
                }

                if (!itemLancamento.NotaProva.HasValue && !itemLancamento.SemAvaliacao && disciplina.TemNota == "S")
                {
                    var excecaoLancamentoAluno = new LancamentoNotasException(LancamentoNotasException.TipoEnum.NotaNaoLancadaATodosOsAlunos);
                    lancamentoNotasExceptionLista.Add(excecaoLancamentoAluno);
                }

                if (itemLancamento.SemAvaliacao && itemLancamento.CodigoJustificativa == null)
                {
                    var excecaoLancamentoAluno = new LancamentoNotasException(LancamentoNotasException.TipoEnum.JustificativaNaoPreenchido)
                    {
                        NomeAluno = itemLancamento.Nome
                    };

                    lancamentoNotasExceptionLista.Add(excecaoLancamentoAluno);
                }

                if (itemLancamento.RecuperacaoParalela && itemLancamento.NotaRecuperacao == null)
                {
                    var excecaoLancamentoAluno = new LancamentoNotasException(LancamentoNotasException.TipoEnum.NotaRecuperacaoNaoPreenchido);
                    lancamentoNotasExceptionLista.Add(excecaoLancamentoAluno);
                }

                if (!dtoSolicitacaoParaLancamento.DadosFrequenciaTurma.CodigoFrequencia.IsNullOrEmpty())
                {
                    if (!itemLancamento.Faltas.HasValue && disciplina.TemFrequencia == "S")
                    {
                        var excecaoLancamentoAluno = new LancamentoNotasException(LancamentoNotasException.TipoEnum.FaltaNaoLancadaATodosOsAlunos);
                        lancamentoNotasExceptionLista.Add(excecaoLancamentoAluno);
                    }

                    if (itemLancamento.Faltas != null)
                    {
                        if (itemLancamento.NotaProva.HasValue 
                            && (!dtoSolicitacaoParaLancamento.DadosFrequenciaTurma.AulasDadas.HasValue 
                                || dtoSolicitacaoParaLancamento.DadosFrequenciaTurma.AulasDadas.Value == 0) 
                            && disciplina.TemNota == "S"
                            && !jaPossuiDisciplinaNaoPossuiAulasDadasException)
                        {
                            var excecaoLancamentoAluno = new LancamentoNotasException(LancamentoNotasException.TipoEnum.DisciplinaNaoPossuiAulasDadas);
                            lancamentoNotasExceptionLista.Add(excecaoLancamentoAluno);

                            jaPossuiDisciplinaNaoPossuiAulasDadasException = true;
                        }
                        if (dtoSolicitacaoParaLancamento.DadosFrequenciaTurma.AulasDadas != null)
                        {
                            if (itemLancamento.Faltas.Value > dtoSolicitacaoParaLancamento.DadosFrequenciaTurma.AulasDadas.Value && disciplina.TemFrequencia == "S")
                            {
                                var excecaoLancamentoAluno = new LancamentoNotasException(LancamentoNotasException.TipoEnum.FaltasMaiorQueAulasDadas)
                                {
                                    NomeAluno = itemLancamento.Nome
                                };

                                lancamentoNotasExceptionLista.Add(excecaoLancamentoAluno);
                            }
                        }

                        if (itemLancamento.Faltas.Value < 0 && disciplina.TemFrequencia == "S")
                        {
                            var excecaoLancamentoAluno = new LancamentoNotasException(LancamentoNotasException.TipoEnum.FaltasMenorQueZero)
                            {
                                NomeAluno = itemLancamento.Nome
                            };

                            lancamentoNotasExceptionLista.Add(excecaoLancamentoAluno);
                        }
                        if (dtoSolicitacaoParaLancamento.DadosFrequenciaTurma.AulasDadas != null)
                        {
                            var pp = 100 - CalcularPercentualPresenca(dtoSolicitacaoParaLancamento.DadosFrequenciaTurma.AulasDadas.Value, itemLancamento.Faltas.Value);

                            if (pp > PERCENTUAL_MINIMO_PRESENCA && ((int)MotivoSemNotaEnum.Outros).ToString().Equals(itemLancamento.CodigoJustificativa))
                            {
                                var excecaoLancamentoAluno = new LancamentoNotasException(LancamentoNotasException.TipoEnum.JustificativaOutrasAcimaPresencaMinima)
                                {
                                    NomeAluno = itemLancamento.Nome
                                };

                                lancamentoNotasExceptionLista.Add(excecaoLancamentoAluno);
                            }

                            if (itemLancamento.Faltas.Value == dtoSolicitacaoParaLancamento.DadosFrequenciaTurma.AulasDadas && itemLancamento.NotaProva.HasValue && disciplina.TemFrequencia == "S" && disciplina.TemNota == "S")
                            {
                                var excecaoLancamentoAluno = new LancamentoNotasException(LancamentoNotasException.TipoEnum.AlunoSemFrequenciaComAvaliacao)
                                {
                                    NomeAluno = itemLancamento.Nome
                                };

                                lancamentoNotasExceptionLista.Add(excecaoLancamentoAluno);
                            }


                            if (itemLancamento.SemAvaliacao && itemLancamento.Faltas.Value == 0 &&
                                    dtoSolicitacaoParaLancamento.DadosFrequenciaTurma.AulasDadas > 0 && disciplina.TemFrequencia == "S")
                            {
                                var excecaoLancamentoAluno = new LancamentoNotasException(LancamentoNotasException.TipoEnum.AlunoComFrequenciaMaximaSemAvalicao)
                                {
                                    NomeAluno = itemLancamento.Nome
                                };

                                lancamentoNotasExceptionLista.Add(excecaoLancamentoAluno);
                            }
                        }
                    }
                    if (dtoProvaParaLancamento != null)
                    {
                        if (itemLancamento.NotaProva > Convert.ToDecimal(dtoProvaParaLancamento.NotaMaxima.Replace('.',',')) && disciplina.TemNota == "S")
                        {
                            var excecaoLancamentoAluno = new LancamentoNotasException(LancamentoNotasException.TipoEnum.NaoPermitiNotaAcimaNotaMaxima)
                            {
                                NomeAluno = itemLancamento.Nome
                            };

                            lancamentoNotasExceptionLista.Add(excecaoLancamentoAluno);
                        }
                    }
                    if (itemLancamento.NotaProva != null)
                    {
                        if (!itemLancamento.SemAvaliacao && itemLancamento.NotaProva.Value >= 5 && itemLancamento.RecuperacaoParalela && disciplina.TemNota == "S")
                        {
                            var excecaoLancamentoAluno = new LancamentoNotasException(LancamentoNotasException.TipoEnum.NaoHaRecuperacaoNotaAzul)
                            {
                                NomeAluno = itemLancamento.Nome
                            };

                            lancamentoNotasExceptionLista.Add(excecaoLancamentoAluno);
                        }
                    }
                    if (itemLancamento.NotaRecuperacao != null)
                    {
                        if (itemLancamento.NotaRecuperacao > Convert.ToDecimal(dtoProvaParaLancamento.NotaMaxima.Replace('.', ',')) && disciplina.TemNota == "S")
                        {
                            var excecaoLancamentoAluno = new LancamentoNotasException(LancamentoNotasException.TipoEnum.NaoPermitiNotaAcimaNotaMaxima)
                            {
                                NomeAluno = itemLancamento.Nome
                            };

                            lancamentoNotasExceptionLista.Add(excecaoLancamentoAluno);
                        }
                    }
                }
            }

            if (lancamentoNotasExceptionLista.Count > 0)
            {
                throw new LancamentoNotasExceptionList(lancamentoNotasExceptionLista);
            }
        }

        public void VerificaNotasEFaltasParaAtualizacaoNotas(string matricula, int? aulasDadas, int? aulasPrevistas, List<DTOFalta> dtoFaltas, List<DTONota> dtoNotas, DTOProtocoloNota dtoProtocolaNota)
        {
            Disciplina disciplina = repositorioDisciplina.ObtemPorChavePrimaria(dtoProtocolaNota.CodigoDisciplina);
            Frequencia frequencia = repositorioFrequencia.ObtemFrequenciaPor(
                dtoProtocolaNota.CodigoDisciplina, dtoProtocolaNota.CodigoTurma, dtoProtocolaNota.Ano, dtoProtocolaNota.Periodo, dtoProtocolaNota.SubPeriodo);

            if (disciplina.TemNota == "S")
            {
                if (dtoNotas == null || dtoNotas.Count == 0
                            || dtoNotas.Select(x => new { x.TipoProva, x.CodigoTurma, x.Ano, x.Periodo, x.CodigoDisciplina }).Distinct().Count() != 1)
                {
                    throw new LancamentoNotasException(LancamentoNotasException.TipoEnum.NaoHaNotasEnviadas);
                }
            }

            if (disciplina.TemFrequencia == "S")
            {
                if (!aulasDadas.HasValue || !aulasPrevistas.HasValue)
                {
                    throw new LancamentoNotasException(LancamentoNotasException.TipoEnum.DadosCompementaresNaoAtualizadosCorretamente);
                }
                else if (aulasDadas == 0 || aulasPrevistas == 0)
                {
                    throw new LancamentoNotasException(LancamentoNotasException.TipoEnum.DadosCompementaresNaoAtualizadosCorretamente);
                }
                else if (frequencia == null)
                {
                    throw new LancamentoNotasException(LancamentoNotasException.TipoEnum.DisciplinaNaoPossuiFrequenciaNoPeriodo);
                }
                if (dtoFaltas == null || dtoFaltas.Count == 0
                            || dtoFaltas.Select(x => new { x.CodigoFrequencia, x.CodigoTurma, x.Ano, x.Periodo, x.CodigoDisciplina }).Distinct().Count() != 1)
                {
                    throw new LancamentoNotasException(LancamentoNotasException.TipoEnum.NaoHaFaltasEnviadas);
                }
            }
            if (matricula.IsNullOrEmpty() || dtoProtocolaNota == null)
            {
                throw new LancamentoNotasException(LancamentoNotasException.TipoEnum.SemMatriculaOUProtocolo);
            }
        }

        public List<DTONotaSalva> ListaNotasPreviamenteSalvasPor(short ano, short periodo, string codigoTurma, string codigoDisciplina, string tipoProva)
        {
            List<Nota> listaNotasSalvas = repositorioNota.EnumeraPor(ano, periodo, codigoTurma, codigoDisciplina, tipoProva).ToList();

            List<DTONotaSalva> listaDtoNota = listaNotasSalvas.ConvertAll(
                nota => new DTONotaSalva
                {
                    CodigoAluno = nota.Aluno,
                    Nota = nota.Conceito.To<Decimal>(),
                    CodigoJustificativa = nota.MotivoSemNota.HasValue ? nota.MotivoSemNota.ToString() : null,
                    RecuperacaoParalela = (nota.RecuperacaoParalela == 'S'),
                    SemAvaliacao = (nota.SemAvaliacao == 'S'),
                }
            );

            return listaDtoNota;
        }

        public List<DTOFaltaSalva> ListaFaltasPreviamenteSalvasPor(short ano, short periodo, string codigoTurma, string codigoDisciplina, string codigoFrequencia)
        {
            List<Falta> listaFaltasSalvas = repositorioFalta.EnumeraPor(ano, periodo, codigoTurma, codigoDisciplina, codigoFrequencia).ToList();

            List<DTOFaltaSalva> listaDtoFalta = listaFaltasSalvas.ConvertAll(
                falta => new DTOFaltaSalva
                {
                    CodigoAluno = falta.Aluno,
                    Faltas = falta.QuantFaltas.To<short>()
                }
            );

            return listaDtoFalta;
        }

        public void AtualizaOuRemoveOuInsereNota(List<DTONota> dtoNotas, List<DTONotaSalva> dtoNotasSalvas, string matricula)
        {
           
            foreach (DTONota dtoNota in dtoNotas)
            {
                DTONotaSalva dtoNotaSalva = dtoNotasSalvas.Where(n => n.CodigoAluno.Trim() == dtoNota.CodigoAluno.Trim()).FirstOrDefault();

                LogNota logNota = MontaLogNota(matricula, dtoNota, dtoNotaSalva, (dtoNotaSalva == null));
                Disciplina disciplina = repositorioDisciplina.ObtemPorChavePrimaria(dtoNota.CodigoDisciplina);
                if (disciplina.TemNota == "S")
                {
                    if (dtoNotaSalva != null)
                    {
                        if (dtoNotaSalva.Nota != dtoNota.NotaProva
                                || dtoNotaSalva.RecuperacaoParalela != dtoNota.RecuperacaoParalela
                                || dtoNotaSalva.SemAvaliacao != dtoNota.SemAvaliacao
                                || dtoNotaSalva.CodigoJustificativa != dtoNota.CodigoJustificativa
                                || dtoNotaSalva.NotaRecuperacao != dtoNota.NotaRecuperacao)
                        {
                            logNota.Operacao = 2;
                            repositorioLogNota.IncluiAuditada(logNota);

                            Nota nota = MontaNota(dtoNota);
                            repositorioNota.AtualizaNota(nota);
                            InsereDeclaracao(nota, matricula);
                        }
                    }
                    else
                    {
                        logNota.Operacao = 1;
                        repositorioLogNota.IncluiAuditada(logNota);

                        Nota nota = MontaNota(dtoNota);
                        repositorioNota.IncluiAuditada(nota);

                        InsereDeclaracao(nota, matricula);
                    }
                }
            }
        }

        public void AtualizarLancamentoEAtualizarAulas(DTOLancamentoAtualizacaoAulas dtoLancamentoAtualizacao)
        {
            Disciplina disciplina = repositorioDisciplina.ObtemPorChavePrimaria(dtoLancamentoAtualizacao.CodigoDisciplina);

            if (disciplina.TemNota == "S")
            {
                repositorioLancamentoNotas.AtualizaFlagLancamentoCompletoPor(dtoLancamentoAtualizacao.Ano, dtoLancamentoAtualizacao.Periodo,
                    dtoLancamentoAtualizacao.CodigoTurma, dtoLancamentoAtualizacao.CodigoDisciplina, dtoLancamentoAtualizacao.TipoProva);
            }
            if (disciplina.TemFrequencia == "S")
            {
                repositorioFrequencia.AtualizaFrequencia(dtoLancamentoAtualizacao.AulasDadas.ToString(), dtoLancamentoAtualizacao.AulasPrevistas.ToString(),
                    dtoLancamentoAtualizacao.CodigoDisciplina, dtoLancamentoAtualizacao.CodigoTurma, dtoLancamentoAtualizacao.Ano,
                    dtoLancamentoAtualizacao.Periodo, dtoLancamentoAtualizacao.CodigoFrequencia);
            }

        }

        public void InsereDeclaracao(Nota nota, string matricula)
        {
            if (nota.SemAvaliacao.HasValue && nota.SemAvaliacao.Value == 'S')
            {
                var declaracao = new DeclaracaoSemNota();
                declaracao.TipoDeclaracaoSemNota = nota.MotivoSemNota.Value;
                declaracao.Matricula = matricula;
                declaracao.DataCadastro = DateTime.Now;
                declaracao.NotaId = nota.Id;
                repositorioDeclaracaoSemNota.IncluiAuditada(declaracao);
            }
        }

        public void AtualizaOuInsereFalta(List<DTOFalta> dtoFaltas, List<DTOFaltaSalva> dtoFaltasSalvas)
        {
            foreach (DTOFalta dtoFalta in dtoFaltas)
            {
                DTOFaltaSalva dtoFaltaSalva = dtoFaltasSalvas.Where(n => n.CodigoAluno.Trim() == dtoFalta.CodigoAluno.Trim()).FirstOrDefault();
                Disciplina disciplina = repositorioDisciplina.ObtemPorChavePrimaria(dtoFalta.CodigoDisciplina);
                if (disciplina.TemFrequencia == "S")
                {
                    if (dtoFaltaSalva != null)
                    {
                        if (!dtoFalta.Faltas.HasValue)
                        {
                            throw new LancamentoNotasException(LancamentoNotasException.TipoEnum.FaltaNaoLancadaATodosOsAlunos);
                        }
                        else if (dtoFaltaSalva.Faltas != dtoFalta.Faltas)
                        {
                            repositorioFalta.Atualiza(dtoFalta.Faltas.To<Double>(), dtoFalta.CodigoAluno, dtoFalta.CodigoDisciplina, dtoFalta.CodigoTurma,
                                dtoFalta.Ano, dtoFalta.Periodo, dtoFalta.CodigoFrequencia);
                        }
                    }
                    else if (dtoFalta.Faltas.HasValue)
                    {
                        Falta falta = MontaFalta(dtoFalta);

                        repositorioFalta.IncluiAuditada(falta);
                    }
                }
            }
        }



        public void ProcessaNotasFaltasProtocolo(string matricula, List<DTOFalta> dtoFaltas, List<DTONota> dtoNotas, ref DTOProtocoloNota dtoProtocolaNota, List<DTONotaSalva> dtoNotasSalvas, List<DTOFaltaSalva> dtoFaltasSalvas, DTOLancamentoAtualizacaoAulas dtoLancamentoAtualizacao)
        {
            Disciplina disciplina = repositorioDisciplina.ObtemPorChavePrimaria(dtoLancamentoAtualizacao.CodigoDisciplina);
            try
            {
                repositorioFalta.InicializaTransacao();

                if (disciplina.TemNota == "S")
                {
                    AtualizaOuRemoveOuInsereNota(dtoNotas, dtoNotasSalvas, matricula);
                }

                AtualizarLancamentoEAtualizarAulas(dtoLancamentoAtualizacao);

                if (disciplina.TemFrequencia == "S")
                {
                    AtualizaOuInsereFalta(dtoFaltas, dtoFaltasSalvas);
                }

                dtoProtocolaNota.Id = InsereProtocolo(dtoProtocolaNota);

                repositorioFalta.FinalizaTransacao();
            }
            catch (LancamentoNotasException)
            {
                repositorioFalta.TransacaoRollback();
                throw;
            }
            catch (System.Exception)
            {
                repositorioFalta.TransacaoRollback();
                throw;
            }
        }

        public int InsereProtocolo(DTOProtocoloNota dtoProtocolaNota)
        {
            Protocolo protocolo = MontaProtocolo(dtoProtocolaNota);

            repositorioProtocolo.Inclui(protocolo);

            return protocolo.Id;
        }

        private Falta MontaFalta(DTOFalta dtoFalta)
        {
            var falta = new Falta
            {
                Aluno = dtoFalta.CodigoAluno,
                Ano = dtoFalta.Ano,
                Disciplina = dtoFalta.CodigoDisciplina,
                Frequencia = dtoFalta.CodigoFrequencia,
                QuantFaltas = dtoFalta.Faltas.To<Double>(),
                Semestre = dtoFalta.Periodo,
                Turma = dtoFalta.CodigoTurma
            };
            return falta;
        }

        private Nota MontaNota(DTONota dtoNota)
        {
            Nota nota = new Nota();
            nota.Id = dtoNota.Id;
            nota.Compareceu = 'S';
            nota.DataProva = DateTime.Today;
            nota.NotaProva = dtoNota.NotaProva;
            nota.Ordem = dtoNota.Ordem;
            nota.Formulario = 1;
            nota.RecuperacaoParalela = dtoNota.RecuperacaoParalela ? 'S' : 'N';
            nota.SemAvaliacao = dtoNota.SemAvaliacao ? 'S' : 'N';
            nota.Justificativa = dtoNota.SemAvaliacao ? "Justificadas" : null;
            nota.Aluno = dtoNota.CodigoAluno;
            nota.Disciplina = dtoNota.CodigoDisciplina;
            nota.Turma = dtoNota.CodigoTurma;
            nota.Ano = dtoNota.Ano;
            nota.Semestre = dtoNota.Periodo;
            nota.TipoProva = dtoNota.TipoProva;
            nota.Conceito = dtoNota.Nota != null ? dtoNota.Nota.ToString() : null;
            nota.NotaRecuperacao = dtoNota.NotaRecuperacao;
            nota.MotivoSemNota = String.IsNullOrEmpty(dtoNota.CodigoJustificativa) ? default(short?) : Convert.ToInt16(dtoNota.CodigoJustificativa);

            return nota;
        }

        private LogNota MontaLogNota(string matricula, DTONota dtoNota, DTONotaSalva dtoNotaSalva, bool ehNova)
        {
            var logNota = new LogNota
            {
                Aluno = dtoNota.CodigoAluno,
                Disciplina = dtoNota.CodigoDisciplina,
                Turma = dtoNota.CodigoTurma,
                Ano = dtoNota.Ano,
                Semestre = dtoNota.Periodo,
                Prova = dtoNota.TipoProva,
                ValorAnterior = !ehNova ? dtoNotaSalva.Nota.ToString() : String.Empty,
                ValorAtual = dtoNota.Nota.ToString(),
                RecuperacaoParalelaAnterior = !ehNova ? (dtoNotaSalva.RecuperacaoParalela ? "S" : "N") : String.Empty,
                RecuperacaoParalelaAtual = dtoNota.RecuperacaoParalela ? "S" : "N",
                SemAvaliacaoAnterior = !ehNova ? (dtoNotaSalva.SemAvaliacao ? "S" : "N") : String.Empty,
                SemAvaliacaoAtual = dtoNota.SemAvaliacao ? "S" : "N",
                JustificativaAnterior = !ehNova ? dtoNotaSalva.CodigoJustificativa : String.Empty,
                JustificativaAtual = dtoNota.CodigoJustificativa,
                Usuario = matricula,
                DataOperacao = DateTime.Now
            };
            return logNota;
        }

        private Protocolo MontaProtocolo(DTOProtocoloNota dtoProtocolaNota)
        {
            var fabricaServico = new NinjectFactoryBase<NinjectModuloServico>();
            DisciplinaService servicoDeDisciplina = fabricaServico.Obtem<DisciplinaService>();

            Protocolo protocolo = new Protocolo
            {
                Ano = dtoProtocolaNota.Ano,
                DataCadastro = System.DateTime.Now,
                Disciplina = dtoProtocolaNota.CodigoDisciplina,
                IdFuncional = dtoProtocolaNota.IdFuncional,
                NomeDisciplina = servicoDeDisciplina.ObtemDescricaoDisciplinaPor(dtoProtocolaNota.CodigoDisciplina),
                Periodo = dtoProtocolaNota.Periodo,
                SubPeriodo = dtoProtocolaNota.SubPeriodo,
                Tipo = !dtoProtocolaNota.Tipo.IsNullOrEmpty() ? dtoProtocolaNota.Tipo : String.Empty,
                Turma = dtoProtocolaNota.CodigoTurma
            };
            return protocolo;
        }

        private float CalcularPercentualPresenca(short aulasDadas, short numeroFaltas)
        {
            return (numeroFaltas / (float)aulasDadas) * 100f;
        }

        public string MensagemFrequenciaNotaFalta(string CodigoDisciplina)
        {
            string Mensagem = null;
            Disciplina disciplina = repositorioDisciplina.ObtemPorChavePrimaria(CodigoDisciplina);
            if (disciplina.TemNota == "N" && disciplina.TemFrequencia == "S")
            {
                Mensagem = "Está disciplina não possui lançamento de notas.";//LancamentoNotasException.TipoEnum.NaoPossuiLancamentoNotas.ToString();
            }
            else if (disciplina.TemNota == "S" && disciplina.TemFrequencia == "N")
            {
                Mensagem = "Está disciplina não possui lançamento de faltas, 'Aulas previstas' e 'Aulas dadas'.";// LancamentoNotasException.TipoEnum.NaoPossuiLancamentoFaltas.ToString();
            }
            else if (disciplina.TemNota == "N" && disciplina.TemFrequencia == "N")
            {
                Mensagem = "Está disciplina não possui lançamento de notas, faltas, 'Aulas previstas' e 'Aulas dadas'.";//LancamentoNotasException.TipoEnum.NaoPossuiLancamentoNotasFaltas.ToString();
            }
            else
            {
                Mensagem = "";
            }

            return Mensagem;
        }
        public Disciplina DisciplinaFrequenciaNota(string CodigoDisciplina)
        {
            Disciplina disciplina = repositorioDisciplina.ObtemPorChavePrimaria(CodigoDisciplina);
            Disciplina disciplinaFreqNota = new Disciplina();
            disciplinaFreqNota.TemFrequencia = disciplina.TemFrequencia;
            disciplinaFreqNota.TemNota = disciplina.TemNota;
            return disciplinaFreqNota;
        }
        #endregion

        public DTOLancamentoNotasConsolidado ObtemLancamentoNotasConsolidado(string disciplina, string turma, short ano, short periodo)
        {
            DTOLancamentoNotasConsolidado dtoLancamentoNotasConsolidado = new DTOLancamentoNotasConsolidado();
            Frequencia freq = repositorioFrequencia.ObtemTotalAulasConsolidado(disciplina, turma, ano, periodo);

            if (freq != null)
            {
                dtoLancamentoNotasConsolidado.TotalAulasDadas = freq.AulasDadas;
                dtoLancamentoNotasConsolidado.TotalAulasPrevistas = freq.AulasPrevistas;
            }

            if (periodo == 0)
            {
                if (ano >= 2025)
                {
                    dtoLancamentoNotasConsolidado.TotalBimestresAtivos = 3;
                }
                else
                {
                    dtoLancamentoNotasConsolidado.TotalBimestresAtivos = 4;
                }
            }
            else if (periodo == 1 || periodo == 2)
            {
                dtoLancamentoNotasConsolidado.TotalBimestresAtivos = 2;
            }

            List<NotaConsolidado> mediaTurma = repositorioNotaConsolidado.EnumeraPor(disciplina, turma, ano, periodo).ToList();

            foreach (NotaConsolidado n in mediaTurma)
            {
                dtoLancamentoNotasConsolidado.MediaTurma.Add(n.SubPeriodo, n.Media);
            }

            List<LancamentoNotasConsolidado> lancamentoNotasConsolidado = repositorioLancamentoNotasConsolidado.EnumeraLancamentosNotasConsolidado(
                disciplina, turma, ano, periodo
            )
            .ToList();

            foreach (var item in lancamentoNotasConsolidado)
            {
                var dtoNotaFrequenciaAcumulada =
                    new DTONotaFrequenciaConsolidado
                    {
                        NomeAluno = item.NomeCompleto,
                        SituacaoMatriculado = (item.SituacaoMatricula == "Matriculado"),
                        Notas = new List<decimal?>() {
                            item.Nota1, item.Nota2, item.Nota3, item.Nota4
                        },
                        Faltas = new List<short?>() {
                            item.Falta1, item.Falta2, item.Falta3, item.Falta4
                        },
                        NotasAcumuladas = item.NotasAcumuladas,
                        FaltasAcumuladas = item.FaltasAcumuladas,
                        PercentualFrequenciaAcumulada = item.PercentualFrequenciaAcumulada
                    };

                dtoLancamentoNotasConsolidado.NotasFrequenciasConsolidadas.Add(dtoNotaFrequenciaAcumulada);
            }

            return dtoLancamentoNotasConsolidado;
        }
    }
}
