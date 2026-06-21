using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Proderj.DOL.Service;
using System.IO;
using System.Security.Cryptography;

using AutoMapper;
using Proderj.DOL.Domain;
using Proderj.DOL.WebApp.Models;
using Resources;
using System.Text;



namespace Proderj.DOL.WebApp.Models
{
	/// <summary>
	/// Garante que o usuário só terá acesso à ação se o mesmo tiver aceitado o termo de aceite, 
	/// caso contrario será redirecionado automaticamente para a aceitação deste termo se estiver 
	/// logado no sistema.
	/// </summary>
	public class LogadoComTermoAceito : AuthorizeAttribute
	{
		//protected override bool AuthorizeCore(HttpContextBase httpContext)
		//{
		//	bool enviarParaAceiteDeTermo;
		//	return AutorizacaoValida(httpContext, out enviarParaAceiteDeTermo);
		//}

        //private static readonly byte[] Key = Encoding.UTF8.GetBytes("g(KMDu(EEw63.*V`"); 
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("d$s&T!20%@22@*V`");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("HR$2pIjHR$2pIj12"); 

		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			bool enviarParaAceiteDeTermo;

 			bool autorizacaoValida = AutorizacaoValida(filterContext.RequestContext.HttpContext, out enviarParaAceiteDeTermo);
			if (!autorizacaoValida && enviarParaAceiteDeTermo)
			{
				filterContext.Result =
					new RedirectResult(new UrlHelper(filterContext.RequestContext).Action("ConfirmaTermoAceite", "Login"));
			}
			else
			{
				base.OnAuthorization(filterContext);
			}
		}

  		protected bool AutorizacaoValida(HttpContextBase httpContext, out bool enviarParaAceiteDeTermo)
		{
			bool autorizar = false;
			enviarParaAceiteDeTermo = false;

			if (httpContext.User.Identity.IsAuthenticated)
			{
				IPrincipal docentePrincipal = httpContext.User;
				if (docentePrincipal is DTODocenteLogadoPrincipal)
				{
					DTODocenteLogadoPrincipal docentePrincipalConcreto = (docentePrincipal as DTODocenteLogadoPrincipal);
					if (docentePrincipalConcreto.AceitouTermoDeAceite)
						autorizar = true;
					else
						enviarParaAceiteDeTermo = true;
				}
			}
			return autorizar;
		}
	}
}