using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proderj.DOL.WebApp.Models;
using Resources;

namespace Proderj.DOL.WebApp.Controllers
{
	public class AjudaController : ControllerPadrao
    {
        //
		// GET: /Ajuda/CurriculoMinimo
		[LogadoComTermoAceito]
        public ActionResult CurriculoMinimo()
        {
			var modeloAjuda = new AjudaViewModel
			                  	{
			                  		TituloDaPagina = Recurso.AjudaCurriculoMinimo_TituloPagina
			                  	};
            return View(modeloAjuda);
        }

		[LogadoComTermoAceito]
		public ActionResult CadastroGLP()
		{
			var modeloAjuda = new AjudaViewModel
			{
				TituloDaPagina = Recurso.AjudaCadastroGLP_TituloPagina
			};
			return View(modeloAjuda);
		}

		[LogadoComTermoAceito]
		public ActionResult SelecaoTurmas()
		{
			var modeloAjuda = new AjudaViewModel
			{
				TituloDaPagina = Recurso.AjudaSelecaoTurmas_TituloPagina
			};
			return View(modeloAjuda);
		}

		[LogadoComTermoAceito]
		public ActionResult LancamentoNotas()
		{
			var modeloAjuda = new AjudaViewModel
			{
				TituloDaPagina = Recurso.AjudaLancamentoNotas_TituloPagina
			};
			return View(modeloAjuda);
		}

		[LogadoComTermoAceito]
		public ActionResult ProtocoloNota()
		{
			var modeloAjuda = new AjudaViewModel
			{
				TituloDaPagina = Recurso.AjudaProtocoloNota_TituloPagina
			};
			return View(modeloAjuda);
		}

        [LogadoComTermoAceito]
        public ActionResult DadosPessoais()
        {
            var modeloAjuda = new AjudaViewModel
            {
                TituloDaPagina = Recurso.AjudaDadosPessoais_TituloPagina
            };
            return View(modeloAjuda);
        }

	}
}
