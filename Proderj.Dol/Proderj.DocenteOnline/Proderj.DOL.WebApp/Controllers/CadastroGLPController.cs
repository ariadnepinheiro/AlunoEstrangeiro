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
    public class CadastroGLPController : ControllerPadrao
    {
        private readonly ICadastroGlpService cadastroGplServico;
        private readonly IUnidadeEnsinoService unidadeEnsinoServico;
        private readonly IPessoaService pessoaServico;
        private readonly IRegionalService regionalServico;
        private readonly ILY_UNIDADE_ENSINOService ueServico;
        private readonly IDISPONIBILIDADEGLPService disponibilidadeGlpServico;
        private readonly IHD_PAISService paisService;

        public CadastroGLPController(
            ICadastroGlpService cadastroGplServico, 
            IUnidadeEnsinoService unidadeEnsinoServico, 
            IPessoaService pessoaServico, 
            IRegionalService regionalServico, 
            ILY_UNIDADE_ENSINOService ueServico,
            IDISPONIBILIDADEGLPService disponibilidadeGlpServico,
            IHD_PAISService paisService
        )
        {
            this.cadastroGplServico = cadastroGplServico;
            this.unidadeEnsinoServico = unidadeEnsinoServico;
            this.pessoaServico = pessoaServico;
            this.regionalServico = regionalServico;
            this.ueServico = ueServico;
            this.disponibilidadeGlpServico = disponibilidadeGlpServico;
            this.paisService = paisService;
        }

        [LogadoComTermoAceito]
        public ActionResult Inicial(DocenteLogadoBindModel modeloDocenteLogado)
        {
            ActionResult viewSaida = null;
            try
            {
                var listaDiaSemana = new List<DiaDaSemanaEnum>(new[] {
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
                                                                            });


                var modelo = new CadastroGLPInicialViewModel(modeloDocenteLogado)
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
                    TituloDaPagina = Recurso.CadastroGLPInicial_TituloPagina,
                    ListaDisciplina = cadastroGplServico.ListaDisciplinasPor(),
                    ListaDiaSemana = listaDiaSemana,
                    NumeroTelefoneDocente = cadastroGplServico.ObtemDocenteComTelefonePor(modeloDocenteLogado.Matricula).Telefone,
                };
                modelo.CabecalhoModelo.TituloCabecalho = Recurso.CadastroGLPInicial_TituloPagina;
                modelo.CabecalhoModelo.LinkAjuda = Url.Action("CadastroGLP", "Ajuda");

                viewSaida = View(modelo);
            }
            catch (System.Exception excecao)
            {
                viewSaida = View("ErroInesperado", excecao);
            }
            return viewSaida;
        }

        [LogadoComTermoAceito]
        public PartialViewResult ListaDisponibilidade(DocenteLogadoBindModel modeloDocenteLogado)
        {
            return PartialView(disponibilidadeGlpServico.ListaPor(modeloDocenteLogado.NumeroFuncionario, DateTime.Now.Year));
        }

        [LogadoComTermoAceitoJson]
        public JsonResult ListaRegional()
        {
            var lista = regionalServico.ListaRegionais().OrderBy(o => o.Descricao).ToList();

            return Json(new
            {
                Sucesso = true,
                Combo = lista,
            });
        }

        [LogadoComTermoAceitoJson]
        public JsonResult ListaMunicipioRegional(int codigoRegional)
        {
            try
            {
                List<DTOUnidadeEnsino> listaUnidadeDeEnsino = unidadeEnsinoServico.ListaPor(codigoRegional).OrderBy(o => o.NomeMunicipio).ToList();
                return Json(
                        new
                        {
                            Sucesso = true,
                            Combo = listaUnidadeDeEnsino.Select(unidade => new
                            {
                                Codigo = unidade.CodigoMunicipio,
                                Descricao = unidade.SiglaUF + " - " + unidade.NomeMunicipio
                            })
                        });

            }
            catch (System.Exception excecao)
            {
                return JsonErro(excecao.Message);
            }
        }

        public JsonResult ListaMunicipioPor(int id_regional)
        {
            try
            {
                IList<DTOListaMUNICIPIOPorID_REGIONAL> lista = ueServico.ListaMunicipioPor(id_regional).OrderBy(o => o.MUNICIPIO).ToList();
                return Json(new
                {
                    Sucesso = true,
                    Combo = lista.Select(unidade => new
                    {
                        Codigo = unidade.ID_MUNICIPIO,
                        Descricao = unidade.UF + " - " + unidade.MUNICIPIO
                    })
                });

            }
            catch (System.Exception excecao)
            {
                return JsonErro(excecao.Message);
            }
        }

        [LogadoComTermoAceitoJson]
        public JsonResult ListaUnidadeEnsinoPor(int? id_regional, string municipio)
        {
            return Json(new
            {
                Sucesso = true,
                Combo = ueServico.ListaPor(id_regional, municipio),
            });
        }

        [LogadoComTermoAceitoJson]
        public JsonResult ListaDisciplinaPor(DocenteLogadoBindModel modeloDocenteLogado)
        {
            var lista = cadastroGplServico.ListaDisciplinasPor(Convert.ToInt32(modeloDocenteLogado.NumeroFuncionario)).OrderBy(o => o.Descricao).ToList();

            return Json(new
            {
                Sucesso = true,
                Combo = lista,
            });
        }

        [LogadoComTermoAceitoJson]
        public JsonResult ObtemRegionalEMunicipioPor(string unidadeEnsino)
        {
            return Json((object)ueServico.ObtemRegionalEMunicipioPor(unidadeEnsino));
        }

        [HttpPost]
        [LogadoComTermoAceitoJson]
        [ValidateAntiForgeryToken]
        public JsonResult IncluiDisponibilidade(DocenteLogadoBindModel modeloDocenteLogado, IncluiDisponibilidadeRequestModel modeloSolicitacao)
        {
            JsonResult jsonSaida = null;

            if (ModelState.IsValid)
            {
                try
                {
                    string[] UEs;
                    if (modeloSolicitacao.CodigoUE == "1")
                        UEs = ueServico.ListaCodigoPor(modeloSolicitacao.CodigoRegional, modeloSolicitacao.CodigoMunicipio).ToArray();
                    else
                        UEs = new string[] { modeloSolicitacao.CodigoUE };

                    var dto = new DTOIncluiDISPONIBILIDADEGLP
                    {
                        AGRUPAMENTO = modeloSolicitacao.CodigoDisciplina,
                        ANO = DateTime.Now.Year,
                        DIASEMANA = modeloSolicitacao.CodigoDiaSemana.ToList(),
                        MODALIDADE = modeloSolicitacao.CodigoModalidade.ToList(),
                        NUM_FUNC = Convert.ToInt32(modeloDocenteLogado.NumeroFuncionario),
                        TURNO = modeloSolicitacao.CodigoTurno.ToList(),
                        UNIDADE_ENS = UEs, 
                        USUARIOID = modeloDocenteLogado.Matricula,
                        DATACADASTRO = DateTime.Now,
                    };

                    disponibilidadeGlpServico.Inclui(dto);

                    jsonSaida = Json(new { Sucesso = true, Mensagem = "Disponibilidade incluída com sucesso." });
                }
                catch (CadastroGlpListaException excecaoLista)
                {
                    foreach (CadastroGlpException excecao in excecaoLista.ListaExcecoes)
                    {
                        ModelState.AddModelError(excecao.TipoDeErro, excecao.Message);
                    }
                }
                catch (System.Exception excecao)
                {
                    jsonSaida = JsonErro(excecao.Message);
                }
            }

            if (jsonSaida == null)
            {
                jsonSaida = JsonErro(ModelState);
            }

            return jsonSaida;
        }

        [HttpPost]
        [LogadoComTermoAceitoJson]
        public JsonResult ExcluiDisponibilidade(DocenteLogadoBindModel modeloDocenteLogado, ExcluiDisponibilidadeRequestModel modeloSolicitacao)
        {
            JsonResult jsonSaida = null;

            if (ModelState.IsValid)
            {
                try
                {
                    disponibilidadeGlpServico.Exclui(modeloSolicitacao.DISPONIBILIDADEGLPID, modeloSolicitacao.UNIDADE_ENS);
                    jsonSaida = Json(new { 
                        Sucesso = true,
                        Mensagem = "Exclusão de disponibilidade realizada com sucesso!",
                    });
                }
                catch (System.Exception excecao)
                {
                    jsonSaida = JsonErro(excecao.Message);
                }
            }

            if (jsonSaida == null)
            {
                jsonSaida = JsonErro(ModelState);
            }

            return jsonSaida;
        }
    }
}
