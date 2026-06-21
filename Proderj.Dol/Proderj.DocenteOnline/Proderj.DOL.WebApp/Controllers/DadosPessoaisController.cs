using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proderj.DOL.Exception;
using Proderj.DOL.Service;
using Proderj.DOL.WebApp.Models;
using Resources;

namespace Proderj.DOL.WebApp.Controllers
{
    public class DadosPessoaisController : ControllerPadrao
    {
        private readonly ICadastroGlpService cadastroGplServico;
        private readonly IUnidadeEnsinoService unidadeEnsinoServico;
        private readonly IPessoaService pessoaServico;
        private readonly IRegionalService regionalServico;
        private readonly IHD_PAISService paisService;
        private readonly ITCE_LOGRADOUROService logradouroService;
        private readonly ITCE_MUNICIPIOService municipioService;
        private readonly ILY_PESSOAService pessoaService;

        public DadosPessoaisController(
            ICadastroGlpService cadastroGplServico, 
            IUnidadeEnsinoService unidadeEnsinoServico, 
            IPessoaService pessoaServico, 
            IRegionalService regionalServico, 
            IHD_PAISService paisService,
            ITCE_LOGRADOUROService logradouroService,
            ITCE_MUNICIPIOService municipioService,
            ILY_PESSOAService pessoaService
        )
        {
            this.cadastroGplServico = cadastroGplServico;
            this.unidadeEnsinoServico = unidadeEnsinoServico;
            this.pessoaServico = pessoaServico;
            this.regionalServico = regionalServico;
            this.paisService = paisService;
            this.logradouroService = logradouroService;
            this.municipioService = municipioService;
            this.pessoaService = pessoaService;
        }

        [LogadoComTermoAceito]
        public ActionResult Inicial(DocenteLogadoBindModel modeloDocenteLogado)
        {
            ActionResult viewSaida = null;
            try
            {
                var listaDiaSemana = new List<DiaDaSemanaEnum>(new[] 
                {
                    DiaDaSemanaEnum.Segunda,
                    DiaDaSemanaEnum.Terca,
                    DiaDaSemanaEnum.Quarta,
                    DiaDaSemanaEnum.Quinta,
                    DiaDaSemanaEnum.Sexta,
                    DiaDaSemanaEnum.Sabado
                })
                .ConvertAll(diaSemana =>
                    (IDTOItemCombo)new DTOItemCombo()
                    {
                        Codigo = ((int)diaSemana).ToString(),
                        Descricao = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int)diaSemana - 1]
                    }
                );

                var modelo = new DadosPessoaisInicialViewModel(modeloDocenteLogado)
                {
                    DadosPessoais = new DadosPessoaisViewModel
                    {
                        TituloDaPagina = Recurso.DadosPessoaisInicial_TituloPagina,
                        ListaDisciplina = cadastroGplServico.ListaDisciplinasPor(),
                        ListaDiaSemana = listaDiaSemana,
                        NumeroTelefoneDocente = cadastroGplServico.ObtemDocenteComTelefonePor(modeloDocenteLogado.Matricula).Telefone,
                        ListaRegional = regionalServico.ListaRegionais(),
                        DropDownListaPais = paisService.Lista()
                            .Where(q => q.Pais != "0000000000")
                            .Select(s => new SelectListItem()
                            {
                                Text = s.Nome,
                                Value = s.Pais
                            })
                            .ToList(),
                    },
                };
                modelo.CabecalhoModelo.TituloCabecalho = Recurso.DadosPessoaisInicial_TituloPagina;
                modelo.CabecalhoModelo.LinkAjuda = Url.Action("DadosPessoais", "Ajuda");

                viewSaida = View(modelo);
            }
            catch (System.Exception excecao)
            {
                viewSaida = View("ErroInesperado", excecao);
            }
            return viewSaida;
        }

        [LogadoComTermoAceito]
        public JsonResult ListaPais()
        {
            return Json(paisService.Lista());
        }

        [LogadoComTermoAceito]
        public JsonResult ListaUF()
        {
            return Json(municipioService.ListaUF());
        }

        [LogadoComTermoAceito]
        public JsonResult ObtemLogradouroPor(string cep) 
        { 
            return Json(logradouroService.ObtemLogradouroPor(cep));
        }

        [LogadoComTermoAceito]
        public JsonResult DicionarioMunicipioPor(string uf)
        {
            return Json(municipioService.ListaMunicipioPor(uf)
                .Select(s => new
                {
                    s.ID_MUNICIPIO,
                    s.NOME
                }));
        }

        [LogadoComTermoAceito]
        public JsonResult ObtemUFPor(string municipio)
        {
            return Json(municipioService.ObtemUFPor(municipio));
        }

        [LogadoComTermoAceito]
        public JsonResult ObtemPessoaPor(int? pessoa)
        {
            return Json(pessoaService.ObtemPor(pessoa));
        }

        [LogadoComTermoAceito]
        public JsonResult ObtemPessoaPorMatricula(string matricula)
        {
            return Json(pessoaService.ObtemPor(matricula));
        }

        [LogadoComTermoAceito]
        public JsonResult Atualiza(DocenteLogadoBindModel modeloDocenteLogado, DTOAtualizaPessoa dto)
        {
            dto.USUARIOID = modeloDocenteLogado.Matricula;
            dto.DATAALTERACAO = DateTime.Now;
            return Json(pessoaService.Atualiza(dto));
        }
    }

}
