using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Drawing.Imaging;

namespace Proderj.DOL.WebApp.Models.Captcha
{
    public class GeraChave : ActionResult
    {
        //TODO: ver onde vai ficar esta classe

        public string _captchaText;

        public GeraChave(string captchaText)
        {
            _captchaText = captchaText;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            Captcha c = new Captcha();
            c.Chave = _captchaText;

            HttpContextBase cb = context.HttpContext;

            cb.Response.Clear();
            cb.Response.ContentType = "image/jpeg";
            c.Imagem.Save(cb.Response.OutputStream, ImageFormat.Jpeg);
            c.Dispose();
        }
    }

}