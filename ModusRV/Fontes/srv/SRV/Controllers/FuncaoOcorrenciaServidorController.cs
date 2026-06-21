using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Models.DTO;
using SRV.Models.Service;
using SRV.Models.Domain;
using SRV.Common;
using SRV.Common.Extension;
using SRV.Common.Exceptions;

namespace SRV.Controllers
{
    public class FuncaoOcorrenciaServidorController : BaseController
    {
        //
        // GET: /FuncaoOcorrenciaServidor/

        public ActionResult Index()
        {
			FiltroFuncaoOcorrenciaServidor filtro = new FiltroFuncaoOcorrenciaServidor();

			ViewBag.TelaFiltrada = false;

			if (Session["FiltroFuncaoOcorrenciaServidor"] != null)
			{
				filtro = (FiltroFuncaoOcorrenciaServidor)Session["FiltroFuncaoOcorrenciaServidor"];
				return Index(filtro, null, null);
			}

			return View(filtro);
        }

		[HttpPost]
		public ActionResult Index(FiltroFuncaoOcorrenciaServidor filtro, int? pageFuncao, int? pageOcorrencia)
		{
			try
			{
				if (filtro.IdServidor != null)
				{
					Session["FiltroFuncaoOcorrenciaServidor"] = filtro;

					OcorrenciaServidorService ocorrenciaServidorService = new OcorrenciaServidorService();
					FuncaoServidorService funcaoServidorService			= new FuncaoServidorService();
					CalculoRVService calculoRVService					= new CalculoRVService();
										
					FiltroFuncaoServidor filtroFuncoes = new FiltroFuncaoServidor
					{
						FuncoesServidores = filtro.FuncoesServidores,
						IdServidor = filtro.IdServidor,
						NomeServidor = filtro.NomeServidor
					};

					FiltroOcorrenciaServidor filtroOcorrencias = new FiltroOcorrenciaServidor
					{
						OcorrenciasServidores = filtro.OcorrenciasServidores,
						IdServidor = filtro.IdServidor,
						NomeServidor = filtro.NomeServidor
					};
					
					filtroFuncoes.IdAnoReferencia = UsuarioLogado.Ciclo;
					filtro.FuncoesServidores = funcaoServidorService.List(filtroFuncoes, pageFuncao ?? 1, Constants.gridPageSize);
					filtro.FuncoesServidores.Title = "Funções do Servidor (Alocações)";

					filtro.OcorrenciasServidores = ocorrenciaServidorService.List(filtroOcorrencias, pageOcorrencia ?? 1, Constants.gridPageSize);
					filtro.OcorrenciasServidores.Title = "Ocorrências do Servidor (Afastamentos)";

					funcaoServidorService.PreencherExtratoAlocacao(filtro);
					ocorrenciaServidorService.PreencherExtratoAfastamento(filtro, UsuarioLogado);

					calculoRVService.PreencherCoeficienteServidor(filtro, UsuarioLogado);

					ViewBag.TelaFiltrada = true;
				}
				else
				{
					throw new BusinessException("Deve ser informada uma matrícula de servidor.");
				}
			}
			catch (Exception e)
			{
				ViewBag.TelaFiltrada = false;
				ExceptionHandler.Execute(e, "Falha na consulta", ModelState);
			}

			return View(filtro);
		}

    }
}
