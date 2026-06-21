using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Proderj.DOL.Domain;
using Proderj.DOL.Service;
using Proderj.DOL.WebApp.Models;
using Resources;
using System.Text;
using System.IO;
using System.Security.Cryptography;
//using Newtonsoft.Json;


namespace Proderj.DOL.WebApp.Controllers
{
    public class DadosDocenteController : ControllerPadrao
    {
        private readonly IDadosDocenteService dadosDocenteServico;
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("SecretariadeEducaçãoDOLVersão2024");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("InicializacaoCriptograficaSeeduc"); 

        public DadosDocenteController(IDadosDocenteService dadosDocenteServico)
        {

            this.dadosDocenteServico = dadosDocenteServico;

        }



        [LogadoComTermoAceito]
        public ActionResult Inicial(DocenteLogadoBindModel docenteLogadoBindModel)
        {
            DadosDocenteViewModel dadosDocenteModel = new DadosDocenteViewModel(docenteLogadoBindModel);
            dadosDocenteModel.TituloDaPagina = Recurso.DadosDocente_TituloPagina;
            dadosDocenteModel.CabecalhoModelo.TituloCabecalho = Recurso.DadosDocente_TituloPagina.ToUpper();

            DateTime dataAtual = DateTime.Now;
            DateTime dataInicioCampanha, dataFimCampanha;
            bool bloqueiaDadosDocentesEmCampanha = false;


            DateTime.TryParse(System.Configuration.ConfigurationManager.AppSettings["DataInicioCampanha"].ToString(), out dataInicioCampanha);
            DateTime.TryParse(System.Configuration.ConfigurationManager.AppSettings["DataFimCampanha"].ToString(), out dataFimCampanha);
            Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings["BloqueiaDadosDocentesEmCampanha"].ToString(), out bloqueiaDadosDocentesEmCampanha);

            dadosDocenteModel.EhPeriodoCampanhaLancamentoNotas = dataAtual >= dataInicioCampanha && dataAtual <= dataFimCampanha;
            dadosDocenteModel.BloqueiaDadosDocentesEmCampanha = bloqueiaDadosDocentesEmCampanha;

            if (dadosDocenteModel.EhPeriodoCampanhaLancamentoNotas && dadosDocenteModel.BloqueiaDadosDocentesEmCampanha)
            {
                ViewData["MensagemBloqueioCampanha"] = String.Format(@"Não é possível exibir informações de cadastro durante campanha de lançamento de notas.<br/>
                Tente novamente em {0}", dataFimCampanha.AddDays(1).ToString("dd/MM/yyyy"));
            }
            else
            {
                /*
                 * Foi necessário especificar o tipo dentro do "ConstructUsing", a fim de evitar o erro de ambiguidade entre as chamadas:
                 * - ConstructUsing(Func<AutoMapper.ResolutionContext, TDestination> ctor) 
                 * e
                 * - ConstructUsing(Func<TSource, TDestination> ctor) 
                 * */
                // TODO: Por questões de performance e referência de uso, toda definição de mapeamento deverá ser colocada no Application_Start (em Global.asax.cs), posteriormente.
                Mapper.CreateMap<DadosGeraisDocenteDTO, DadosGeraisDocenteViewModel>()
                    .ConstructUsing(
                        (Func<DadosGeraisDocenteDTO, DadosGeraisDocenteViewModel>)
                        (x => new DadosGeraisDocenteViewModel(docenteLogadoBindModel))
                    );

                DadosGeraisDocenteDTO docente = dadosDocenteServico.ObtemPor(docenteLogadoBindModel.Matricula);
                dadosDocenteModel.DadosGerais = Mapper.Map<DadosGeraisDocenteDTO, DadosGeraisDocenteViewModel>(docente);

                List<DadosFormacaoDocenteDTO> graduacoes = dadosDocenteServico.ListaFormacaoPor(docenteLogadoBindModel.Matricula, TipoFormacaoEnum.Graduacao);
                dadosDocenteModel.Graduacoes = Mapper.Map<List<DadosFormacaoDocenteDTO>, List<DadosFormacaoDocenteViewModel>>(graduacoes);

                List<DadosFormacaoDocenteDTO> posGraduacoes = dadosDocenteServico.ListaFormacaoPor(docenteLogadoBindModel.Matricula, TipoFormacaoEnum.PosGraduacao);
                dadosDocenteModel.PosGraduacoes = Mapper.Map<List<DadosFormacaoDocenteDTO>, List<DadosFormacaoDocenteViewModel>>(posGraduacoes);

                List<DadosCapacitacaoDocenteDTO> capacitacoes = dadosDocenteServico.ListaCapacitacaoPor(docenteLogadoBindModel.Matricula);
                dadosDocenteModel.Capacitacoes = Mapper.Map<List<DadosCapacitacaoDocenteDTO>, List<DadosCapacitacaoDocenteViewModel>>(capacitacoes);
            }

            return View(dadosDocenteModel);
        }

        [System.Web.Services.WebMethod]
        [LogadoComTermoAceito]
        public string ObtemDataHoraParaImpressao()
        {
            return DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }
    }
}
