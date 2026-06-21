using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proderj.DOL.WebApp.Models
{
	public class ControllerPadrao : Controller
	{
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            var controller = requestContext.RouteData.Values["controller"];
            var controllers = new string[] 
            {
                "AcompanhamentoClassroom",
                "Api",
                "AvaliacaoCurriculoMinimo",
                "CadastroGLP",
                "CodigoArmazemDoLivro",
                "DadosDocente",
                "DadosPessoais",
                "LancamentoNotas",
                "ProtocoloNota",
                "RespostaCurriculoMinimo",
                "SelecaoTurmas",
            };

            if (StaticMethods.AlteracaoDeSenhaNecessaria && controllers.Contains(controller))
            {
                throw new System.Exception("Alteração de senha é obrigatória");
            }

            base.Initialize(requestContext);
        }
				
		protected internal virtual PrerequisitoValidoResult GeraViewPrerequisitoValido(string viewName, object model)
		{
			if (model != null)
			{
				ViewData.Model = model;
			}

			return new PrerequisitoValidoResult(viewName, model)
			{
				ViewName = viewName,
				ViewData = ViewData,
				TempData = TempData
			};
		}

		protected internal virtual PrerequisitoInvalidoResult GeraViewPrerequisitoInvalido(string viewName, object model)
		{
			if (model != null)
			{
				ViewData.Model = model;
			}

			return new PrerequisitoInvalidoResult(viewName, model)
			{
				ViewName = viewName,
				ViewData = ViewData,
				TempData = TempData
			};
		}

		protected internal virtual JsonResult JsonErro(string mensagem)
		{
			return Json(new {Sucesso = false, Mensagem = mensagem});
		}

		internal JsonResult JsonErro(ModelStateDictionary modelState)
		{
			JsonResult resultado = null;
			//Converter ModelState em dados via JSON
			if (!modelState.IsValid)
			{
				var listaMensagemErro = new List<string>();
				foreach (var itemModelState in modelState.Values.Where(valores => valores.Errors.Count > 0))
				{
					foreach (var erro in itemModelState.Errors)
					{
						listaMensagemErro.Add(erro.ErrorMessage);
					}
				}

				resultado = Json(new
				            	{
				            		Sucesso = false,
				            		ListaMensagem = listaMensagemErro
				            	});
			}
			else
			{
				resultado = JsonErro("O modelo é valido mas foi tratado como inválido");
			}

			return resultado;
		}
	}
}