using System.Web.Mvc;
using SRV.Filters;
using System;
using System.IO;
using System.Configuration;
using SRV.Models.Service;
using System.Web;
using SRV.Models.Domain;
using SRV.Common.Exceptions;
using SRV.Models.DTO;
using System.Xml;


namespace SRV.Controllers
{
    //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    [CustomAuthorize]
    [LoadManagerContext]
    public abstract class BaseController : Controller
    {
        protected UserState UsuarioLogado { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            UsuarioLogado = (UserState)Session["user"];
        }


        /// <summary>
        /// Action utilizada para verificar se a sessão está ativa.
        /// Deve ser realizada uma requisição ajax para essa action, e caso a sessão
        /// estaja encerrada irá entrar no tratamento padrão para requisições ajax,
        /// exibindo um alert na tela
        /// </summary>
        /// <returns></returns>
        public JsonResult ValidaSessao()
        {
            return Json("Usuário autenticado", JsonRequestBehavior.AllowGet);
        }

        public string GetClientIP()
        {
            string ipAddress = String.IsNullOrEmpty(Request.Headers.Get("x-forwarded-for"))
                ? Request.UserHostAddress : Request.Headers.Get("x-forwarded-for");

            if (ipAddress.Contains(","))
                ipAddress = ipAddress.Split(',')[0];

            return ipAddress;
        }

        protected void UploadFile(HttpPostedFileBase fileUpload, TipoImportacao tipoImportacao)
        {
            if (fileUpload == null || fileUpload.ContentLength == 0)
            {
                throw new Exception("Arquivo não existe ou está vazio");
            }

            string fileExtension = Path.GetExtension(fileUpload.FileName);

            if (fileExtension != ".csv")
                throw new Exception("Formato de arquivo não suportado. Favor usar formato \'*.csv\'");
            
            ArquivoImportacao arquivoImportacao = new ArquivoImportacao();

            arquivoImportacao.TipoImportacao = tipoImportacao;
            arquivoImportacao.DesArquivoOriginal = Path.GetFileName(fileUpload.FileName);
            arquivoImportacao.DesArquivo = BuildFilename(tipoImportacao);

            Usuario usuario = new Usuario();
            usuario.Id = UsuarioLogado.Id;

            arquivoImportacao.UsuarioUpload = usuario;


            string pathUpload = ConfigurationManager.AppSettings["PathUpload"];

            Directory.CreateDirectory(pathUpload);

            fileUpload.SaveAs(Path.Combine(pathUpload, arquivoImportacao.DesArquivo));

            ArquivoImportacaoService arquivoImportacaoService = new ArquivoImportacaoService();
            arquivoImportacaoService.Insert(arquivoImportacao);

        }

        private string BuildFilename(TipoImportacao tipoImportacao)
        {
            return String.Format("{0}_{1}.csv", tipoImportacao.ToString(), String.Format("{0:yyyy_MM_dd_HHmmss}", DateTime.Now));
        }


    }
}
