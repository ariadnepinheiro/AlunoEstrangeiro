using System.Web.Mvc;

namespace Proderj.DOL.WebApp.Models
{
	/// <summary>
	/// Garante que o usuário só terá acesso à ação se o mesmo tiver aceitado o termo de aceite, 
	/// caso contrario será redirecionado automaticamente para a aceitação deste termo se estiver 
	/// logado no sistema.
	/// </summary>
	public class LogadoComTermoAceitoJson : LogadoComTermoAceito
	{
		//protected override bool AuthorizeCore(HttpContextBase httpContext)
		//{
		//	bool enviarParaAceiteDeTermo;
		//	return AutorizacaoValida(httpContext, out enviarParaAceiteDeTermo);
		//}

		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			bool enviarParaAceiteDeTermo;
			bool autorizacaoValida = AutorizacaoValida(filterContext.RequestContext.HttpContext, out enviarParaAceiteDeTermo);
			if (!autorizacaoValida && enviarParaAceiteDeTermo)
			{
				filterContext.Result =
					new JsonResult
						{
							Data = new { Sucesso = false, Mensagem = "Termo de aceite não foi confirmado. Por favor, faça o login novamente."}
						};
			}
			else if (!autorizacaoValida)
			{
				filterContext.Result =
					new JsonResult
						{
							Data = new { Sucesso = false, Mensagem = "Sessão expirada. Por favor, faça o login novamente" }
						};
			}
			else
				base.OnAuthorization(filterContext);
		}
	}
}