using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Proderj.DOL.WebApp.Models;
using Resources;
using AutoMapper;
using Proderj.DOL.Service;
using Proderj.DOL.Exception;
using Proderj.Foundation.Common;
using System.Text;
using Proderj.DOL.Domain;
using log4net;

namespace Proderj.DOL.WebApp.Controllers
{
    public class LancamentoNotasController : ControllerPadrao
    {
        private readonly ITurmaService turmaServico;
        private readonly IDisciplinaService disciplinaServico;
        private readonly ILancamentoNotasService lancamentoDeNotasServico;
        private readonly IRespostaCurriculoMinimoService curriculoMinimoServico;
        private readonly ISubPeriodoLetivoService subPeriodoLetivoServico;
        private readonly ISolicitacaoAlteracaoNotasService solicitacaoAlteracaoNotaServico;
        private readonly IItemTabelaService itemTabelaServico;
        private readonly IMaterialEstudoService materialEstudoServico;
        private readonly ITurmaMaterialEstudoService turmaMaterialEstudoServico;
        private ILog logger; 

        public LancamentoNotasController(
            ILancamentoNotasService lancamentoDeNotasServico,
            ISubPeriodoLetivoService subPeriodoLetivoServico,
            IRespostaCurriculoMinimoService curriculoMinimoServico,
            IDisciplinaService disciplinaServico,
            ISolicitacaoAlteracaoNotasService solicitacaoAlteracaoNotaServico,
            IItemTabelaService itemTabelaServico,
            ITurmaService turmaServico,
            IMaterialEstudoService materialEstudoServico,
            ITurmaMaterialEstudoService turmaMaterialEstudoServico
            )
        {        
            logger = log4net.LogManager.GetLogger("FileAppender");

            this.lancamentoDeNotasServico = lancamentoDeNotasServico;
            this.subPeriodoLetivoServico = subPeriodoLetivoServico;
            this.curriculoMinimoServico = curriculoMinimoServico;
            this.disciplinaServico = disciplinaServico;
            this.solicitacaoAlteracaoNotaServico = solicitacaoAlteracaoNotaServico;
            this.turmaServico = turmaServico;
            this.itemTabelaServico = itemTabelaServico;
            this.materialEstudoServico = materialEstudoServico;
            this.turmaMaterialEstudoServico = turmaMaterialEstudoServico;
        }

        private LancamentoNotasListaViewModel CarregaModeloLista(DocenteLogadoBindModel docenteLogadoModelo, LancamentoNotasListaRequestModel lancaNotasModelo)
        {
            return CarregaModeloLista(docenteLogadoModelo, lancaNotasModelo, null);
        }

        private LancamentoNotasListaViewModel CarregaModeloLista(DocenteLogadoBindModel docenteLogadoModelo,
            LancamentoNotasListaRequestModel lancaNotasModelo, bool ehConsolidado)
        {
            return CarregaModeloLista(docenteLogadoModelo, lancaNotasModelo, null, ehConsolidado);
        }

        private LancamentoNotasListaViewModel CarregaModeloLista(DocenteLogadoBindModel docenteLogadoModelo,
            LancamentoNotasListaRequestModel lancaNotasModelo, short? bimestreSelecionado, bool ehConsolidado = false)
        {
            var fabricaDeController = (NinjectControllerFactory)ControllerBuilder.Current.GetControllerFactory();
            var selecaoTurmaController = (SelecaoTurmasController)fabricaDeController.CriaController(typeof(SelecaoTurmasController));

            LancamentoNotasListaViewModel modelo = new LancamentoNotasListaViewModel();

            modelo = new LancamentoNotasListaViewModel
            {
                TituloDaPagina = Recurso.LancamentoNotasLista_TituloPagina,
                Ano = lancaNotasModelo.Ano,
                Periodo = lancaNotasModelo.Periodo,
                CodigoCurso = lancaNotasModelo.CodigoCurso,
                CodigoTurma = lancaNotasModelo.CodigoTurma,
                CodigoDisciplina = lancaNotasModelo.CodigoDisciplina,
                CodigoUnidadeEnsino = lancaNotasModelo.CodigoUnidadeEnsino,
                CodigoModalidade = lancaNotasModelo.CodigoModalidade,
                TipoCurso = lancaNotasModelo.TipoCurso,
                Serie = lancaNotasModelo.Serie
            };





            //modelo.ListaTurmaMaterialEstudo = turmaMaterialEstudoServico.ObtemTurmaMatEstPor(modelo.CodigoTurma, modelo.Ano, modelo.Periodo, modelo.CodigoDisciplina, modelo.BimestreSelecionado);
            //modelo.ListaMaterialEstudo = materialEstudoServico.ObtemPor(docenteLogadoModelo.Matricula);
            modelo.CabecalhoModelo.TituloCabecalho = Recurso.LancamentoNotasLista_TituloPagina;
            modelo.CabecalhoModelo.LinkAjuda = Url.Action("LancamentoNotas", "Ajuda");

            modelo.TurmaSelecionadaModelo = selecaoTurmaController.ObtemTurmaSelecionadaViewModelPor(lancaNotasModelo, docenteLogadoModelo);
            modelo.BimestresHabilitados = subPeriodoLetivoServico.EnumeraPor(lancaNotasModelo.Ano, lancaNotasModelo.Periodo).ToList();

            if (!bimestreSelecionado.HasValue)
            {
                //Verifica se existe bimestre informado na solicitação
                if (lancaNotasModelo.Subperiodo.HasValue)
                {
                    modelo.BimestreSelecionado = lancaNotasModelo.Subperiodo.Value;
                }
                else
                {
                    //Senao tiver, pega o mais recente/atual.
                    modelo.BimestreSelecionado = subPeriodoLetivoServico.ObtemAtualParaLancamentoDeNotasPor(lancaNotasModelo.Ano, lancaNotasModelo.Periodo);
                }
            }
            else
            {
                //verifica se o bimestre seleciona está contido nos bimestre permitidos
                if (!modelo.BimestresHabilitados.Any(subperiodo => subperiodo.SubPeriodo == bimestreSelecionado.Value))
                {
                    throw new LancamentoNotasException(Recurso.LancamentoNotasLista_ExceptionBimestreSelecionadoInvalido);
                }

                modelo.BimestreSelecionado = bimestreSelecionado.Value;
            }

            modelo.DisciplinaFrequenciaNota = lancamentoDeNotasServico.DisciplinaFrequenciaNota(lancaNotasModelo.CodigoDisciplina);

            //Obtendo configuração de notas para a disciplina
            modelo.DadosConfiguracaoNotaDisciplina = disciplinaServico.ObtemConfiguracaoNotaPor(lancaNotasModelo.CodigoDisciplina);

            //
            modelo.ListaTurmaMaterialEstudo = turmaMaterialEstudoServico.ObtemTurmaMatEstPor(modelo.CodigoTurma, modelo.Ano, modelo.Periodo, modelo.CodigoDisciplina, modelo.BimestreSelecionado);

            modelo.ListaMaterialEstudo = materialEstudoServico.ObtemPor(docenteLogadoModelo.Matricula);

            #region Lançamento de Notas Consolidado
            if (lancaNotasModelo.ExibeConsolidado)
            {
                modelo.DadosConsolidados = lancamentoDeNotasServico.ObtemLancamentoNotasConsolidado(
                    lancaNotasModelo.CodigoDisciplina, lancaNotasModelo.CodigoTurma, lancaNotasModelo.Ano, lancaNotasModelo.Periodo);
            }
            #endregion
            #region Lançamento de notas
            else
            {
                modelo.ListaItemJustificativa = lancamentoDeNotasServico.ListarItemJustificativa();
                modelo.ExisteBimestreAnteriorPendenteDeLancamento =
                    lancamentoDeNotasServico.ExisteNotaPendenteParaLancamentoEmBimestreAnteriorAoAtualPor(
                        lancaNotasModelo.Ano, lancaNotasModelo.Periodo, modelo.BimestreSelecionado, lancaNotasModelo.CodigoDisciplina, lancaNotasModelo.CodigoTurma
                    );

                var dtoVerificacaoStatusCurriculoMinimo = new DTORespostaCurriculoMinimo_ObtemStatusPreenchimentoPor
                {
                    Ano = lancaNotasModelo.Ano,
                    CodigoCurso = lancaNotasModelo.CodigoCurso,
                    CodigoDisciplina = lancaNotasModelo.CodigoDisciplina,
                    CodigoModalidade = lancaNotasModelo.CodigoModalidade,
                    CodigoTurma = lancaNotasModelo.CodigoTurma,
                    MatriculaDocente = docenteLogadoModelo.Matricula,
                    Periodo = lancaNotasModelo.Periodo,
                    Serie = lancaNotasModelo.Serie,
                    Subperiodo = modelo.BimestreSelecionado,
                    TipoCurso = lancaNotasModelo.TipoCurso
                };



                //Verifica e prepara dados de currículo minimo
                modelo.DadosPreenchimentoCurriculoMinimo = curriculoMinimoServico.ObtemStatusPreenchimentoPor(dtoVerificacaoStatusCurriculoMinimo);

                ///Retirado pois curriculo minimo agor é lançado na frequencia
                modelo.HabilitaCurriculoMinimo = false;
                //modelo.HabilitaCurriculoMinimo = modelo.DadosPreenchimentoCurriculoMinimo.StatusPreenchimento !=
                //                                 StatusPreenchimentoCurriculoMinimoPorTurmaEnum.NaoSeAplica;

                //if (modelo.DadosPreenchimentoCurriculoMinimo.StatusPreenchimento == StatusPreenchimentoCurriculoMinimoPorTurmaEnum.Pendente)
                //{
                //    modelo.MensagemStatusCurriculoMinimo =
                //        Recurso.LancamentoNotasLista_MensagemCurriculoMinimoPendente;
                //}
                //else if (modelo.DadosPreenchimentoCurriculoMinimo.StatusPreenchimento == StatusPreenchimentoCurriculoMinimoPorTurmaEnum.Parcial)
                //{
                //    modelo.MensagemStatusCurriculoMinimo =
                //        Recurso.LancamentoNotasLista_MensagemCurriculoMinimoParcial
                //            .Fmt(
                //                modelo.DadosPreenchimentoCurriculoMinimo.PercentualPreenchimentoParcial,
                //                modelo.DadosPreenchimentoCurriculoMinimo.DataUltimoPreenchimento.Value.ToString("dd/MM/yyyy"),
                //                modelo.DadosPreenchimentoCurriculoMinimo.DataUltimoPreenchimento.Value.ToString("HH'h'mm")

                //            );

                //}
                //else if (modelo.DadosPreenchimentoCurriculoMinimo.StatusPreenchimento == StatusPreenchimentoCurriculoMinimoPorTurmaEnum.Completo)
                //{
                //    modelo.MensagemStatusCurriculoMinimo =
                //        Recurso.LancamentoNotasLista_MensagemCurriculoMinimoCompleto
                //            .Fmt(
                //                modelo.DadosPreenchimentoCurriculoMinimo.DataUltimoPreenchimento.Value.ToString("dd/MM/yyyy"),
                //                modelo.DadosPreenchimentoCurriculoMinimo.DataUltimoPreenchimento.Value.ToString("HH'h'mm")
                //            );
                //}

                //Verifica se é pra habilitar o modo de edição
                modelo.HabilitaEdicaoDeNotas = lancamentoDeNotasServico.PodeLancarNotaNaTurma(lancaNotasModelo.CodigoDisciplina, lancaNotasModelo.CodigoTurma, lancaNotasModelo.Ano, lancaNotasModelo.Periodo, modelo.BimestreSelecionado);
                modelo.DadosFrequenciaTurma = turmaServico.ObtemFrequenciaDaTurmaPor(lancaNotasModelo.CodigoDisciplina, lancaNotasModelo.CodigoTurma, lancaNotasModelo.Ano, lancaNotasModelo.Periodo, modelo.BimestreSelecionado);

                //Se existir dados de frequencia pra turma é habilitada a opção de edição destes
                modelo.HabilitaEdicaoAulasPrevistasEDadas = modelo.DadosFrequenciaTurma != null;
                modelo.HabilitaEdicaoDeFaltas = modelo.DadosFrequenciaTurma != null;

                //Obtendo configuração de notas para a disciplina
                modelo.DadosConfiguracaoNotaDisciplina = disciplinaServico.ObtemConfiguracaoNotaPor(lancaNotasModelo.CodigoDisciplina);

                //Lista de alunos para lancamento de nota
                modelo.ListaItemLancamentoNotaFrequenciaAluno = lancamentoDeNotasServico.ListaLancamentoNotaFrequenciaAlunoPor(lancaNotasModelo.CodigoDisciplina, lancaNotasModelo.CodigoTurma, lancaNotasModelo.Ano, lancaNotasModelo.Periodo, modelo.BimestreSelecionado);

                bool subPeriodoAtivo = subPeriodoLetivoServico.EhAtivoParaLancamentoDeNotas(lancaNotasModelo.Ano, lancaNotasModelo.Periodo, modelo.BimestreSelecionado);
                bool existeSolicitacaoAlteracao = false;

                DTOSolicitacaoAlteracaoNotas_ConsultaTurma dtoConsultaSolicitacao = null;

                if (!subPeriodoAtivo)
                {
                    dtoConsultaSolicitacao = new DTOSolicitacaoAlteracaoNotas_ConsultaTurma
                    {
                        Ano = lancaNotasModelo.Ano,
                        CodigoDisciplina = lancaNotasModelo.CodigoDisciplina,
                        CodigoTurma = lancaNotasModelo.CodigoTurma,
                        CodigoUnidadeEnsino = lancaNotasModelo.CodigoUnidadeEnsino,
                        NumeroFuncionarioDocente = docenteLogadoModelo.NumeroFuncionario,
                        SubPeriodo = modelo.BimestreSelecionado
                    };

                    existeSolicitacaoAlteracao = solicitacaoAlteracaoNotaServico.ExisteSolicitacaoAlteracaoNotaValidaEAprovada(dtoConsultaSolicitacao);
                }

                modelo.HabilitaLancamentoNotas = (subPeriodoAtivo || existeSolicitacaoAlteracao);

                if (modelo.HabilitaLancamentoNotas)
                {
                    //Para lançamentos liberados
                    if (modelo.DadosConfiguracaoNotaDisciplina.TemNota == "S" && !modelo.HabilitaEdicaoDeNotas)
                    {
                        //Caso disciplina possua nota e não exista prova, desabilita lançamento
                        modelo.HabilitaLancamentoNotas = false;
                    }
                    if (modelo.DadosConfiguracaoNotaDisciplina.TemFrequencia == "S" && !modelo.HabilitaEdicaoDeFaltas)
                    {
                        //Caso disciplina possua falta e não exista frequencia, desabilita lançamento
                        modelo.HabilitaLancamentoNotas = false;
                    }
                }

                if (!modelo.HabilitaLancamentoNotas)
                {
                    //dtoConsultaSolicitacao será sempre preenchido quando entrar nesta condição (false + false)

                    //Verifica se o subperiodo é antigo
                    bool ehSubperiodoAntigo = subPeriodoLetivoServico.EhAntigoParaLancamentoDeNotas(lancaNotasModelo.Ano, lancaNotasModelo.Periodo, modelo.BimestreSelecionado);

                    if (ehSubperiodoAntigo)
                    {
                        //Verificar se ja há solicitação de lançamento de notas
                        DateTime? dataUltimaSolicitacaoNota =
                            solicitacaoAlteracaoNotaServico.ObtemDataDaSolicitacaoAlteracaoNotaAguardandoAprovacao(dtoConsultaSolicitacao);

                        //Se não houver, habilita a possibilidade de solicitação
                        modelo.HabilitaSolicitacaoDeLancamento = (dataUltimaSolicitacaoNota == null);

                        if (dataUltimaSolicitacaoNota != null)
                        {
                            modelo.MensagemSolicitacaoAlteracaoNotasExistente =
                                String.Format(Recurso.LancamentoNotasLista_MensagemSolicitacaoAlteracaoNotasExistente,
                                              dataUltimaSolicitacaoNota.Value.ToString("dd/MM/yyyy"),
                                              dataUltimaSolicitacaoNota.Value.ToString("HH:mm:ss"));
                        }
                    }
                    else
                    {
                        modelo.HabilitaSolicitacaoDeLancamento = false;
                    }
                }

                modelo.MensagemFrequenciaNotaFalta = lancamentoDeNotasServico.MensagemFrequenciaNotaFalta(lancaNotasModelo.CodigoDisciplina);
            }
            #endregion


            return modelo;
        }

        [LogadoComTermoAceito]
        public ActionResult Inicial(DocenteLogadoBindModel docenteLogadoModelo)
        {
            return RedirectToAction("Lista", "SelecaoTurmas", new { nomeController = "LancamentoNotas" });
        }

        public ActionResult Lista()
        {
            //Se tentar carregar o lancamento de notas via GET, redireciona para a seleção de turmas
            return RedirectToAction("Lista", "SelecaoTurmas", new { nomeController = "LancamentoNotas" });
        }

        [HttpPost]
        [LogadoComTermoAceito]
        public ActionResult Lista(DocenteLogadoBindModel docenteLogadoModelo, LancamentoNotasListaRequestModel lancaNotasSolicitacaoModelo)
        {


            //Sempre que carrega o lançamento de notas, renova o tempo de login em mais 1 hora.
            var fabricaDeController = (NinjectControllerFactory)ControllerBuilder.Current.GetControllerFactory();
            var loginController = (LoginController)fabricaDeController.CriaController(typeof(LoginController));
            loginController.ReautenticaDocenteComExtensaoDoTempo(docenteLogadoModelo);

            ViewResult viewSaida = null;
            string codigoTurma = lancaNotasSolicitacaoModelo.CodigoTurma;
            LancamentoNotasListaViewModel modeloLancaNotas;

            //Verifica se todos os dados do modelo estão preenchidos...
            if (ModelState.IsValid)
            {
                //Mapper.CreateMap<LancamentoNotasListaRequestModel, DTOLancamentoNotasSolicitacao>();
                DTOLancamentoNotasSolicitacao dtoSolicitacao = Mapper.Map<LancamentoNotasListaRequestModel, DTOLancamentoNotasSolicitacao>(lancaNotasSolicitacaoModelo);

                //Verifica se o professor tem acesso a essa turma/disciplina
                try
                {
                    if (!lancaNotasSolicitacaoModelo.ExibeConsolidado)
                    {
                        lancamentoDeNotasServico.VerificaPermissaoParaLancarNota(docenteLogadoModelo.NumeroFuncionario, dtoSolicitacao);
                        //Se correr tudo bem...

                        modeloLancaNotas = CarregaModeloLista(docenteLogadoModelo, lancaNotasSolicitacaoModelo);
                        //Carrega o modelo de lançamento de notas para a turma.
                    }
                    else
                    {
                        modeloLancaNotas = CarregaModeloLista(docenteLogadoModelo, lancaNotasSolicitacaoModelo, true);
                    }

                    modeloLancaNotas.CabecalhoModelo.DocenteLogadoModelo = docenteLogadoModelo;
                    modeloLancaNotas.ExibeConsolidado = lancaNotasSolicitacaoModelo.ExibeConsolidado;

                    viewSaida = View("Lista", modeloLancaNotas);
                }
                catch (LancamentoNotasException lnex)
                {
                    ModelState.AddModelError(lnex.TipoDeErro, lnex.Message);
                }
                catch (SubPeriodoLetivoException subperiodoException)
                {
                    ModelState.AddModelError(subperiodoException.TipoDeErro, subperiodoException.Message);
                }
                catch (System.Exception ex)
                {
                    viewSaida = View("ErroInesperado", ex);

                    logger.Error("Erro - " + ex.ToString());
                }
            }

            if (viewSaida == null)
            {
                var selecaoTurmaController = (SelecaoTurmasController)fabricaDeController.CriaController(typeof(SelecaoTurmasController));

                try
                {
                    viewSaida = selecaoTurmaController.Lista(docenteLogadoModelo, "LancamentoNotas", this, lancaNotasSolicitacaoModelo.CodigoTurma);
                }
                catch (System.Exception ex)
                {
                    logger.Error("Erro - " + ex.ToString());
                }

            }

            return viewSaida;
        }

        [HttpPost]
        [LogadoComTermoAceito]
        public ActionResult SolicitaReaberturaBimestre(DocenteLogadoBindModel docenteLogadoModelo, LancamentoNotasSolicitaReaberturaBimestreRequestModel solicitacaoReabertura)
        {
            ActionResult viewRetorno;

            //Mapper.CreateMap<LancamentoNotasSolicitaReaberturaBimestreRequestModel, LancamentoNotasListaRequestModel>();
            LancamentoNotasListaRequestModel lancamentoNotaSolicitacao = Mapper.Map<LancamentoNotasSolicitaReaberturaBimestreRequestModel, LancamentoNotasListaRequestModel>(solicitacaoReabertura);

            LancamentoNotasListaViewModel lancamentoNotaListaModelo = new LancamentoNotasListaViewModel();

            try
            {
                lancamentoNotaListaModelo = CarregaModeloLista(docenteLogadoModelo, lancamentoNotaSolicitacao);
                //Mapper.CreateMap<LancamentoNotasSolicitaReaberturaBimestreRequestModel, DTOSolicitacaoAlteracaoNotas_ConsultaTurma>();
                DTOSolicitacaoAlteracaoNotas_ConsultaTurma dtoConsultaSolicitacao = Mapper.Map<LancamentoNotasSolicitaReaberturaBimestreRequestModel, DTOSolicitacaoAlteracaoNotas_ConsultaTurma>(solicitacaoReabertura);

                dtoConsultaSolicitacao.NumeroFuncionarioDocente = docenteLogadoModelo.NumeroFuncionario;
                //Verificar se ja há solicitação de lançamento de notas
                DateTime? dataUltimaSolicitacaoNota =
                        solicitacaoAlteracaoNotaServico.ObtemDataDaSolicitacaoAlteracaoNotaAguardandoAprovacao(dtoConsultaSolicitacao);

                //Se não houver, habilita a possibilidade de solicitação
                if (dataUltimaSolicitacaoNota == null)
                {
                    //Mapper.CreateMap<LancamentoNotasSolicitaReaberturaBimestreRequestModel, DTOSolicitacaoReabertura>();
                    DTOSolicitacaoReabertura dtoSolicitacao = Mapper.Map<LancamentoNotasSolicitaReaberturaBimestreRequestModel, DTOSolicitacaoReabertura>(solicitacaoReabertura);

                    dtoSolicitacao.NumeroFuncionario = docenteLogadoModelo.NumeroFuncionario;

                    solicitacaoAlteracaoNotaServico.InsereSolicitacaoReabertura(dtoSolicitacao);
                    lancamentoNotaListaModelo.SolicitadoReaberturaLancamento = true;
                }
                else
                {
                    ModelState.AddModelError("Solicitação reabertura", Recurso.LancamentoNotasLista_SolicitacaoReaberturaIndisponivel);
                    lancamentoNotaListaModelo.CodigoTurmaErro = solicitacaoReabertura.CodigoTurma;
                }

                viewRetorno = View("Lista", lancamentoNotaListaModelo);

            }
            catch (System.Exception ex)
            {
                viewRetorno = View("ErroInesperado", ex);
                logger.Error("Erro - " + ex.ToString());
            }

            return viewRetorno;
        }

        [HttpPost]
        [LogadoComTermoAceito]
        public ActionResult Salva(DocenteLogadoBindModel docenteLogadoModelo, LancamentoNotasSalvaRequestModel notasEnviadasPeloProfessorModelo)
        {
            ActionResult viewRetorno = null;
            LancamentoNotasListaViewModel modeloNotaTurmaErro = null;
            List<DTONotaSalva> dtoNotasSalvas = null;
            List<DTOFaltaSalva> dtoFaltasSalvas = null;
            DTOLancamentoAtualizacaoAulas dtoLancamentoAtualizacao = null;
            Disciplina disciplina = lancamentoDeNotasServico.DisciplinaFrequenciaNota(notasEnviadasPeloProfessorModelo.CodigoDisciplina);

            var materialEstudo = Request.Form["materialEstudo"];

            if (string.IsNullOrWhiteSpace(materialEstudo))
            {
                ModelState.AddModelError("MaterialEstudoNaoPreenchido", "Preencha material estudo.");
            }

            try
            {
                if (notasEnviadasPeloProfessorModelo.DadosFrequenciaTurma == null)
                {
                    ModelState.AddModelError("Frequencia", @"Dados referentes a turma (aulas dadas e/ou aulas presvistas e/ou código da frequencia) não encontrados.");
                }

                if (ModelState.IsValid)
                {

                    var lstMaterialEstudos = materialEstudoServico.ObtemIds();
                    //var modelo = turmaMaterialEstudoServico.Obtem(Convert.ToInt32(materialEstudo[0].ToString()), notasEnviadasPeloProfessorModelo.Subperiodo, notasEnviadasPeloProfessorModelo.CodigoDisciplina, notasEnviadasPeloProfessorModelo.Periodo.ToString(), notasEnviadasPeloProfessorModelo.Ano.ToString(), notasEnviadasPeloProfessorModelo.CodigoTurma.ToString());
                    //var modelo = turmaMaterialEstudoServico.Obtem(Convert.ToInt32(materialEstudo[0].ToString()), notasEnviadasPeloProfessorModelo.Subperiodo, notasEnviadasPeloProfessorModelo.CodigoDisciplina, notasEnviadasPeloProfessorModelo.Periodo.ToString(), notasEnviadasPeloProfessorModelo.Ano.ToString(), notasEnviadasPeloProfessorModelo.CodigoTurma.ToString());

                    var dtoTurmaMaterialEstudo = new DTOTurmaMaterialEstudo
                    {
                        Ano = notasEnviadasPeloProfessorModelo.Ano.ToString(),
                        Disciplina = notasEnviadasPeloProfessorModelo.CodigoDisciplina,
                        Semestre = notasEnviadasPeloProfessorModelo.Periodo.ToString(),
                        Subperiodo = notasEnviadasPeloProfessorModelo.Subperiodo,
                        Turma = notasEnviadasPeloProfessorModelo.CodigoTurma.ToString(),
                    };

                    turmaMaterialEstudoServico.Gravar(materialEstudo, dtoTurmaMaterialEstudo, lstMaterialEstudos, docenteLogadoModelo.IdFuncional);// User.Identity.Name

                    //Mapper.CreateMap<LancamentoNotasSalvaRequestModel, DTOLancamentoNotasSolicitacao>();
                    DTOLancamentoNotasSolicitacao dtoSolicitacao = Mapper.Map<LancamentoNotasSalvaRequestModel, DTOLancamentoNotasSolicitacao>(notasEnviadasPeloProfessorModelo);

                    lancamentoDeNotasServico.VerificaPermissaoParaLancarNota(docenteLogadoModelo.NumeroFuncionario, dtoSolicitacao);

                    DTOProvaParaLancamento dtoProvaParaLancamento = turmaServico.ObtemProvaDaTurmaParaLancamentoPor(notasEnviadasPeloProfessorModelo.CodigoDisciplina, notasEnviadasPeloProfessorModelo.CodigoTurma,
                        notasEnviadasPeloProfessorModelo.Ano, notasEnviadasPeloProfessorModelo.Periodo, notasEnviadasPeloProfessorModelo.Subperiodo);

                    //Mapper.CreateMap<LancamentoNotasSalvaRequestModel, DTOLancamentoNotasSalvamento>();
                    DTOLancamentoNotasSalvamento dtoSolicitacaoParaSalvamento = Mapper.Map<LancamentoNotasSalvaRequestModel, DTOLancamentoNotasSalvamento>(notasEnviadasPeloProfessorModelo);

                    lancamentoDeNotasServico.VerificaPermissaoParaSalvarNota(dtoSolicitacaoParaSalvamento, dtoProvaParaLancamento);

                    var dtoFaltas = new List<DTOFalta>();
                    var dtoNotas = new List<DTONota>();

                    foreach (DTOItemSalvaNotaFrequenciaAluno dtoItemLancamentoNotaFrequenciaAluno in notasEnviadasPeloProfessorModelo.ListaItemLancamentoNotaFrequenciaAluno)
                    {
                        if (notasEnviadasPeloProfessorModelo.DadosFrequenciaTurma != null)
                        {
                            if (!notasEnviadasPeloProfessorModelo.DadosFrequenciaTurma.CodigoFrequencia.IsNullOrEmpty())
                            {
                                dtoFaltas.Add(MontaDtoFalta(notasEnviadasPeloProfessorModelo, dtoItemLancamentoNotaFrequenciaAluno));
                            }
                        }
                        if (disciplina.TemNota == "S")
                        {
                            dtoNotas.Add(MontaDtoNota(notasEnviadasPeloProfessorModelo, dtoProvaParaLancamento, dtoItemLancamentoNotaFrequenciaAluno));
                        }
                    }

                    DTOProtocoloNota dtoProtocoloNota = MontaDtoProtocoloNota(docenteLogadoModelo.IdFuncional + "/" + docenteLogadoModelo.Vinculo, notasEnviadasPeloProfessorModelo);

                    lancamentoDeNotasServico.VerificaNotasEFaltasParaAtualizacaoNotas(docenteLogadoModelo.IdFuncional + "/" + docenteLogadoModelo.Vinculo, notasEnviadasPeloProfessorModelo.DadosFrequenciaTurma.AulasDadas,
                        notasEnviadasPeloProfessorModelo.DadosFrequenciaTurma.AulasPrevistas, dtoFaltas, dtoNotas, dtoProtocoloNota);
                    if (disciplina.TemNota == "S")
                    {
                        dtoNotasSalvas = lancamentoDeNotasServico.ListaNotasPreviamenteSalvasPor(notasEnviadasPeloProfessorModelo.Ano,
                                notasEnviadasPeloProfessorModelo.Periodo, notasEnviadasPeloProfessorModelo.CodigoTurma,
                                notasEnviadasPeloProfessorModelo.CodigoDisciplina, dtoProvaParaLancamento.TipoProva);
                    }
                    if (disciplina.TemFrequencia == "S")
                    {
                        dtoFaltasSalvas = lancamentoDeNotasServico.ListaFaltasPreviamenteSalvasPor(notasEnviadasPeloProfessorModelo.Ano,
                                    notasEnviadasPeloProfessorModelo.Periodo, notasEnviadasPeloProfessorModelo.CodigoTurma,
                                    notasEnviadasPeloProfessorModelo.CodigoDisciplina, notasEnviadasPeloProfessorModelo.DadosFrequenciaTurma.CodigoFrequencia);
                    }
                    string tipoProva = null;
                    if (dtoProvaParaLancamento != null)
                    {
                        tipoProva = dtoProvaParaLancamento.TipoProva;
                    }
                    dtoLancamentoAtualizacao = MontaDtoLancamentoAtualizacao(notasEnviadasPeloProfessorModelo, tipoProva);

                    lancamentoDeNotasServico.ProcessaNotasFaltasProtocolo(docenteLogadoModelo.IdFuncional + "/" + docenteLogadoModelo.Vinculo, dtoFaltas, dtoNotas, ref dtoProtocoloNota,
                                                     dtoNotasSalvas, dtoFaltasSalvas, dtoLancamentoAtualizacao);//docenteLogadoModelo.Matricula

                    bool existeNotaPendente = lancamentoDeNotasServico.ExisteNotaPendenteParaLancamentoEmBimestreAnteriorAoAtualPor(notasEnviadasPeloProfessorModelo.Ano,
                        notasEnviadasPeloProfessorModelo.Periodo, notasEnviadasPeloProfessorModelo.Subperiodo,
                        notasEnviadasPeloProfessorModelo.CodigoDisciplina, notasEnviadasPeloProfessorModelo.CodigoTurma);

                    LancamentoNotasListaRequestModel lancamentoNotaSolicitacao = MontaLancamentoNotasSolicitacaoViewModel(notasEnviadasPeloProfessorModelo);

                    LancamentoNotasListaViewModel lancamentoInicialNota = CarregaModeloLista(docenteLogadoModelo, lancamentoNotaSolicitacao);

                    ComplementaLancamentoNotasInicial(dtoProtocoloNota, existeNotaPendente, lancamentoInicialNota);



                    viewRetorno = View("Lista", lancamentoInicialNota);
                }
                else
                {
                    modeloNotaTurmaErro = CarregaModeloLancaNotaTurmaAposErro(docenteLogadoModelo, notasEnviadasPeloProfessorModelo);
                }
            }
            catch (LancamentoNotasException excecaoLancamento)
            {
                ModelState.AddModelError(excecaoLancamento.TipoDeErro, excecaoLancamento.Message);

            }
            catch (LancamentoNotasExceptionList excecaoLancamentoLista)
            {
                excecaoLancamentoLista.ListaExcecoes.ExecuteForEach(x =>
                {
                    ModelState.AddModelError(x.TipoDeErro, x.Message);
                });
            }
            catch (SubPeriodoLetivoException subperiodoException)
            {
                ModelState.AddModelError(subperiodoException.TipoDeErro, subperiodoException.Message);
            }
            catch (System.Exception ex)
            {
                viewRetorno = View("ErroInesperado", ex);
                logger.Error("Erro - " + ex.ToString());
            }


            if (viewRetorno == null)
            {
                modeloNotaTurmaErro = CarregaModeloLancaNotaTurmaAposErro(docenteLogadoModelo, notasEnviadasPeloProfessorModelo);
                modeloNotaTurmaErro.CodigoTurmaErro = notasEnviadasPeloProfessorModelo.CodigoTurma;
                viewRetorno = View("Lista", modeloNotaTurmaErro);
            }


            return viewRetorno;
        }

        [Authorize]
        [LogadoComTermoAceito]
        public ActionResult GeraFilipeta()
        {
            return null;
        }

        private void ComplementaLancamentoNotasInicial(DTOProtocoloNota dtoProtocoloNota, bool existeNotaPendente, LancamentoNotasListaViewModel lancamentoInicialNota)
        {
            try
            {
                lancamentoInicialNota.CodigoFilipeta = dtoProtocoloNota.Codigo;
                lancamentoInicialNota.LancamentoPersistido = true;

                var mensagemProtocolo = new StringBuilder();

                //if (!existeNotaPendente)
                //	mensagemProtocolo.AppendLine("Notas atualizadas com sucesso.");

                mensagemProtocolo.Append(String.Format(Recurso.LancamentoNotasLista_MensagemSucessoLancamento, dtoProtocoloNota.Codigo));

                lancamentoInicialNota.MensagemLancamentoPersistido = mensagemProtocolo.ToString();
            }
            catch (System.Exception ex)
            {
                logger.Error("Erro - " + ex.ToString());
            }

        }

        private LancamentoNotasListaRequestModel MontaLancamentoNotasSolicitacaoViewModel(LancamentoNotasSalvaRequestModel notasEnviadasPeloProfessorModelo)
        {
            LancamentoNotasListaRequestModel lancamento = new LancamentoNotasListaRequestModel();

            lancamento = new LancamentoNotasListaRequestModel
            {
                Ano = notasEnviadasPeloProfessorModelo.Ano,
                CodigoCurso = notasEnviadasPeloProfessorModelo.CodigoCurso,
                CodigoDisciplina = notasEnviadasPeloProfessorModelo.CodigoDisciplina,
                CodigoModalidade = notasEnviadasPeloProfessorModelo.CodigoModalidade,
                CodigoTurma = notasEnviadasPeloProfessorModelo.CodigoTurma,
                CodigoUnidadeEnsino = notasEnviadasPeloProfessorModelo.CodigoUnidadeEnsino,
                Periodo = notasEnviadasPeloProfessorModelo.Periodo,
                Serie = notasEnviadasPeloProfessorModelo.Serie,
                Subperiodo = notasEnviadasPeloProfessorModelo.Subperiodo,
                TipoCurso = notasEnviadasPeloProfessorModelo.TipoCurso

            };


            return lancamento;
        }

        private LancamentoNotasListaViewModel CarregaModeloLancaNotaTurmaAposErro(DocenteLogadoBindModel docenteLogadoModelo, LancamentoNotasSalvaRequestModel notasEnviadasPeloProfessorModelo)
        {
            LancamentoNotasListaRequestModel lancaNotasModelo = new LancamentoNotasListaRequestModel();
            LancamentoNotasListaViewModel modeloNotasParaCarregamentoDaTela = new LancamentoNotasListaViewModel();

            lancaNotasModelo = new LancamentoNotasListaRequestModel
            {
                Ano = notasEnviadasPeloProfessorModelo.Ano,
                CodigoCurso = notasEnviadasPeloProfessorModelo.CodigoCurso,
                CodigoDisciplina = notasEnviadasPeloProfessorModelo.CodigoDisciplina,
                CodigoModalidade = notasEnviadasPeloProfessorModelo.CodigoModalidade,
                CodigoTurma = notasEnviadasPeloProfessorModelo.CodigoTurma,
                CodigoUnidadeEnsino = notasEnviadasPeloProfessorModelo.CodigoUnidadeEnsino,
                Periodo = notasEnviadasPeloProfessorModelo.Periodo,
                Serie = notasEnviadasPeloProfessorModelo.Serie,
                Subperiodo = notasEnviadasPeloProfessorModelo.Subperiodo,
                TipoCurso = notasEnviadasPeloProfessorModelo.TipoCurso
            };

            modeloNotasParaCarregamentoDaTela = CarregaModeloLista(docenteLogadoModelo, lancaNotasModelo, notasEnviadasPeloProfessorModelo.Subperiodo);

            //Carrega as informações vindas do banco com os dados enviados pelo professor 
            //para o professor nao perder os dados após salvar
            foreach (DTOItemSalvaNotaFrequenciaAluno dtoAlunoEnviadoProfessor in notasEnviadasPeloProfessorModelo.ListaItemLancamentoNotaFrequenciaAluno)
            {

                DTOItemLancamentoNotaFrequenciaAluno dtoAlunoDoBanco =
                    modeloNotasParaCarregamentoDaTela.ListaItemLancamentoNotaFrequenciaAluno.FirstOrDefault(
                        aluno => aluno.Codigo == dtoAlunoEnviadoProfessor.Codigo);

                if (dtoAlunoDoBanco != null)
                {
                    dtoAlunoDoBanco.Nota = dtoAlunoEnviadoProfessor.Nota;
                    dtoAlunoDoBanco.Faltas = dtoAlunoEnviadoProfessor.Faltas;
                    dtoAlunoDoBanco.SemAvaliacao = dtoAlunoEnviadoProfessor.SemAvaliacao;
                    dtoAlunoDoBanco.RecuperacaoParalela = dtoAlunoEnviadoProfessor.RecuperacaoParalela;
                    dtoAlunoDoBanco.CodigoJustificativa = dtoAlunoEnviadoProfessor.CodigoJustificativa;
                    dtoAlunoDoBanco.NotaProva = dtoAlunoEnviadoProfessor.NotaProva;
                    dtoAlunoDoBanco.NotaRecuperacao = dtoAlunoEnviadoProfessor.NotaRecuperacao;
                }
                else
                {
                    //Aluno que o professor está enviando nao existe na base
                    ModelState.AddModelError("AlunoInvalido", @"Aluno informado inexistente");
                }
            }


            return modeloNotasParaCarregamentoDaTela;
        }

        private DTOLancamentoAtualizacaoAulas MontaDtoLancamentoAtualizacao(LancamentoNotasSalvaRequestModel notasEnviadasPeloProfessorModelo, String tipoProva)
        {
            DTOLancamentoAtualizacaoAulas dtoLancamentoAtualizacao = new DTOLancamentoAtualizacaoAulas();

            dtoLancamentoAtualizacao = new DTOLancamentoAtualizacaoAulas
            {
                Ano = notasEnviadasPeloProfessorModelo.Ano,
                AulasDadas = notasEnviadasPeloProfessorModelo.DadosFrequenciaTurma.AulasDadas,
                AulasPrevistas = notasEnviadasPeloProfessorModelo.DadosFrequenciaTurma.AulasPrevistas,
                CodigoDisciplina = notasEnviadasPeloProfessorModelo.CodigoDisciplina,
                CodigoFrequencia = notasEnviadasPeloProfessorModelo.DadosFrequenciaTurma.CodigoFrequencia,
                CodigoTurma = notasEnviadasPeloProfessorModelo.CodigoTurma,
                Periodo = notasEnviadasPeloProfessorModelo.Periodo,
                TipoProva = tipoProva
            };



            return dtoLancamentoAtualizacao;
        }

        private DTOProtocoloNota MontaDtoProtocoloNota(string MatriculadocenteLogadoModelo, LancamentoNotasSalvaRequestModel notasEnviadasPeloProfessorModelo)
        {
            return new DTOProtocoloNota
            {
                Ano = notasEnviadasPeloProfessorModelo.Ano,
                CodigoDisciplina = notasEnviadasPeloProfessorModelo.CodigoDisciplina,
                IdFuncional = MatriculadocenteLogadoModelo,
                Periodo = notasEnviadasPeloProfessorModelo.Periodo,
                SubPeriodo = notasEnviadasPeloProfessorModelo.Subperiodo,
                CodigoTurma = notasEnviadasPeloProfessorModelo.CodigoTurma,
                Tipo = "T"
            };
        }

        private DTONota MontaDtoNota(LancamentoNotasSalvaRequestModel notasEnviadasPeloProfessorModelo, DTOProvaParaLancamento dtoProvaParaLancamento, DTOItemSalvaNotaFrequenciaAluno dtoItemLancamentoNotaFrequenciaAluno)
        {
            DTONota dtoNota = new DTONota();

            dtoNota.Id = dtoItemLancamentoNotaFrequenciaAluno.Id;
            dtoNota.Ano = notasEnviadasPeloProfessorModelo.Ano;
            dtoNota.CodigoDisciplina = notasEnviadasPeloProfessorModelo.CodigoDisciplina;
            dtoNota.CodigoTurma = notasEnviadasPeloProfessorModelo.CodigoTurma;
            dtoNota.CodigoAluno = dtoItemLancamentoNotaFrequenciaAluno.Codigo;
            dtoNota.Periodo = notasEnviadasPeloProfessorModelo.Periodo;
            dtoNota.NotaProva = dtoItemLancamentoNotaFrequenciaAluno.SemAvaliacao ? null : dtoItemLancamentoNotaFrequenciaAluno.NotaProva;
            dtoNota.TipoProva = dtoProvaParaLancamento.TipoProva;
            dtoNota.Ordem = dtoProvaParaLancamento.Ordem;
            dtoNota.RecuperacaoParalela = dtoItemLancamentoNotaFrequenciaAluno.SemAvaliacao ? false : dtoItemLancamentoNotaFrequenciaAluno.RecuperacaoParalela;
            dtoNota.SemAvaliacao = dtoItemLancamentoNotaFrequenciaAluno.SemAvaliacao;
            dtoNota.CodigoJustificativa = dtoItemLancamentoNotaFrequenciaAluno.CodigoJustificativa;
            dtoNota.NotaRecuperacao = dtoItemLancamentoNotaFrequenciaAluno.SemAvaliacao ? null : dtoItemLancamentoNotaFrequenciaAluno.NotaRecuperacao;

            decimal? nota = null;

            if (!dtoItemLancamentoNotaFrequenciaAluno.SemAvaliacao)
            {
                if (dtoItemLancamentoNotaFrequenciaAluno.NotaProva == null)
                {
                    nota = 0;
                }
                else
                {
                    nota = dtoItemLancamentoNotaFrequenciaAluno.NotaProva;
                }

                var notaRecuperacao = dtoItemLancamentoNotaFrequenciaAluno.NotaRecuperacao == null ? 0 : dtoItemLancamentoNotaFrequenciaAluno.NotaRecuperacao;
                if (nota < notaRecuperacao)
                {
                    nota = notaRecuperacao;
                }
            }

            dtoNota.Nota = nota;



            return dtoNota;
        }

        private DTOFalta MontaDtoFalta(LancamentoNotasSalvaRequestModel notasEnviadasPeloProfessorModelo, DTOItemSalvaNotaFrequenciaAluno dtoItemLancamentoNotaFrequenciaAluno)
        {
            return new DTOFalta
                 {
                     Ano = notasEnviadasPeloProfessorModelo.Ano,
                     CodigoDisciplina = notasEnviadasPeloProfessorModelo.CodigoDisciplina,
                     CodigoTurma = notasEnviadasPeloProfessorModelo.CodigoTurma,
                     CodigoAluno = dtoItemLancamentoNotaFrequenciaAluno.Codigo,
                     Periodo = notasEnviadasPeloProfessorModelo.Periodo,
                     Faltas = dtoItemLancamentoNotaFrequenciaAluno.Faltas,
                     CodigoFrequencia = notasEnviadasPeloProfessorModelo.DadosFrequenciaTurma.CodigoFrequencia
                 };

        }
    }
}
