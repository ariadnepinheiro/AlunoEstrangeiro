using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;
using Proderj.DOL.Service;

namespace Proderj.DOL.WebApp.Models
{
	public class DocenteLogadoModelBinder : IModelBinder
	{
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			IPrincipal docentePrincipal = controllerContext.HttpContext.User;
			if (docentePrincipal is DTODocenteLogadoPrincipal)
			{
				DTODocenteLogadoPrincipal docentePrincipalConcreto = (docentePrincipal as DTODocenteLogadoPrincipal);
				var modeloDocenteLogado = new DocenteLogadoBindModel(docentePrincipalConcreto);
				return modeloDocenteLogado;

			}
			else
			{
				throw new ApplicationException("O usuário não está logado");
			}
		}
	}
}
