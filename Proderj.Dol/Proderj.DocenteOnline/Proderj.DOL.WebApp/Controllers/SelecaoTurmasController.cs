using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proderj.DOL.WebApp.Models;
using Proderj.DOL.Service;
using Proderj.Foundation.Framework;
using Resources;
using log4net;

namespace Proderj.DOL.WebApp.Controllers
{
    public class SelecaoTurmasController : ControllerPadrao
    {
        private readonly ISelecaoTurmasService selecaoTurmasService;
        private readonly IDisciplinaService disciplinaServico;
        private readonly IUnidadeEnsinoService unidadeEnsinoServico;
        private readonly ILog logger = log4net.LogManager.GetLogger("FileAppender");

        public SelecaoTurmasController(ISelecaoTurmasService selecaoTurmasService, IDisciplinaService disciplinaServico, IUnidadeEnsinoService unidadeEnsinoServico)
        {
            this.selecaoTurmasService = selecaoTurmasService;
            this.disciplinaServico = disciplinaServico;
            this.unidadeEnsinoServico = unidadeEnsinoServico;
        }

        public SelecaoTurmasTurmaSelecionadaViewModel ObtemTurmaSelecionadaViewModelPor(ISelecaoTurmasTurmaSelecionadaRequestModel solicitacaoModelo, DocenteLogadoBindModel docenteLogadoModelo)
        {
            SelecaoTurmasTurmaSelecionadaViewModel modeloTurmaSelecionada = new SelecaoTurmasTurmaSelecionadaViewModel();

            try
            {
                modeloTurmaSelecionada = new SelecaoTurmasTurmaSelecionadaViewModel
                {
                    Ano = solicitacaoModelo.Ano,
                    CodigoTurma = solicitacaoModelo.CodigoTurma,
                    DescricaoDisciplina = disciplinaServico.ObtemDescricaoDisciplinaPor(solicitacaoModelo.CodigoDisciplina),
                    DescricaoUnidadeEnsino = unidadeEnsinoServico.ObtemDescricaoPor(solicitacaoModelo.CodigoUnidadeEnsino),
                    MatriculaDocente = docenteLogadoModelo.Matricula,
                    IdFuncional = docenteLogadoModelo.IdFuncional,
                    Vinculo = docenteLogadoModelo.Vinculo,
                    NomeDocente = docenteLogadoModelo.Nome,
                    Periodo = solicitacaoModelo.Periodo
                };
            }
            catch (System.Exception ex)
            {
                logger.Error("Erro - " + ex.ToString());

            }

            return modeloTurmaSelecionada;
        }

        [LogadoComTermoAceito]
        public ViewMontadaResult Lista(DocenteLogadoBindModel docenteLogadoModelo, string nomeController, ControllerPadrao controllerOrigem, string codigoTurma)
        {
            //Testar quando o professor nao tiver nenhuma turma..
            try
            {
                ModelStateDictionary modelState = null;
                var modelo = new SelecaoTurmasListaViewModel(docenteLogadoModelo);
                modelo.ListaSelecaoTurma = selecaoTurmasService.EnumeraSelecaoTurmasPor(docenteLogadoModelo.NumeroFuncionario).ToList();
                modelo.CodigoTurmaErro = codigoTurma ?? String.Empty;
                if (Url != null)
                {
                    modelState = ModelState;
                    modelo.AcaoPost = Url.Action("Lista", nomeController);
                    modelo.CabecalhoModelo.LinkAjuda = Url.Action("SelecaoTurmas", "Ajuda");
                }
                else if (Url == null && controllerOrigem != null)
                {
                    modelState = controllerOrigem.ModelState;
                    modelo.AcaoPost = controllerOrigem.Url.Action("Lista", nomeController);
                    modelo.CabecalhoModelo.LinkAjuda = controllerOrigem.Url.Action("SelecaoTurmas", "Ajuda");
                }

                if (nomeController == "RespostaCurriculoMinimo")
                {
                    modelo.TituloDaPagina = Recurso.RespostaCurriculoMinimoSelecaoTurmas_TituloPagina;
                    modelo.CabecalhoModelo.TituloCabecalho = Recurso.RespostaCurriculoMinimoSelecaoTurmas_TituloPagina;
                    modelo.MensagemInicialTela = Recurso.RespostaCurriculoMinimoSelecaoTurmas_MensagemInicial;
                }
                else
                {
                    modelo.TituloDaPagina = Recurso.LancamentoNotasSelecaoTurma_TituloPagina;
                    modelo.CabecalhoModelo.TituloCabecalho = Recurso.LancamentoNotasSelecaoTurma_TituloPagina;
                    modelo.MensagemInicialTela = Recurso.SelecaoTurmasLista_MensagemInicial;
                }
                return new ViewMontadaResult("../SelecaoTurmas/Lista", modelo, modelState);
            }
            catch (System.Exception excecao)
            {
                logger.Error("Erro - " + excecao.ToString());

                return new ViewMontadaResult("ErroInesperado", excecao, new ModelStateDictionary());
            }
        }
    }
}
