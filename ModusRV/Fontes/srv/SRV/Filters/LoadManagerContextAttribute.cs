using System.Web.Mvc;
using SRV.Models.DTO;
using SRV.Common;
using System;

namespace SRV.Filters
{
    public class LoadManagerContextAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            UserState user = filterContext.Controller.ViewBag.UsuarioLogado;

            if (user != null)
            {
                string controllerName = filterContext.Controller.GetType().Name;
                controllerName = controllerName.Substring(0, controllerName.Length - "Controller".Length);

                filterContext.Controller.ViewBag.CaminhoPao = Menu.GetCaminhoPao(user.Menu, controllerName);

                AuthorizeControllerAttribute[] att = (AuthorizeControllerAttribute[])filterContext.Controller.GetType().GetCustomAttributes(typeof(AuthorizeControllerAttribute), false);

                if (att.Length > 0)
                    controllerName = att[0].Controller;
            }
        }
    }
}