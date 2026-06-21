using System;
using System.Web;
using System.Web.SessionState;
using Techne.Lyceum.RN.Util;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Techne.Lyceum.Net.Cadastros
{
    public class SignPDF : IHttpHandler, IRequiresSessionState
    {
        private readonly Techne.Lyceum.RN.Cadastros.MaeFormularioBancoArquivo rnMaeFormularioBancoArquivo = new Techne.Lyceum.RN.Cadastros.MaeFormularioBancoArquivo();

        private IList<PDFSigner> _PDFSigners
        {
            get
            {
                IList<PDFSigner> result = HttpContext.Current.Session["Techne.Lyceum.Net.Cadastros.CreatePDF.PDFSigners"] as IList<PDFSigner>;
                result = result ?? new List<PDFSigner>();
                HttpContext.Current.Session["Techne.Lyceum.Net.Cadastros.CreatePDF.PDFSigners"] = result;
                return result;
            }
        }

        private int Id
        {
            get
            {
                int result;
                int.TryParse(HttpContext.Current.Request.Form["Id"] ?? "0", out result);
                return result;
            }
        }

        private string HashAssinado
        {
            get
            {
                return (HttpContext.Current.Request.Form["HashAssinado"] as string);
            }
        }

        private int MaeInscricaoId 
        {
            get
            {
                int result;
                int.TryParse(HttpContext.Current.Request.Form["MaeInscricaoId"] ?? "0", out result);
                return result;
            }
        }
        
        public void ProcessRequest(HttpContext context)
        {
            if (Id == 0)
            {
                context.Response.Write("É necessário fornecer o parâmetro \"Id\" na query string");
                context.Response.End();
                return;
            }

            if (MaeInscricaoId == 0)
            {
                context.Response.Write("É necessário fornecer o parâmetro \"MaeInscricaoId\" na query string");
                context.Response.End();
                return;
            }

            PDFSigner pdfSigner = _PDFSigners.FirstOrDefault(q => q.GetHashCode() == Id);
            if (pdfSigner == null)
            {
                context.Response.Write("O Id \"" + Id + "\" não foi localizado");
                context.Response.End();
                return;
            }

            byte[] pdfbytes;
            if (!HashAssinado.IsNullOrEmptyOrWhiteSpace())
                pdfbytes = pdfSigner.SignPDFToMemory(Convert.FromBase64String(HashAssinado));
            else
                pdfbytes = pdfSigner.GetOriginalBytes();

            //var chaveArquivo = pdfSigner.Tag;

            pdfSigner.Dispose();
            
            _PDFSigners.Remove(pdfSigner);

            var maeFormularioBancoArquivo = new RN.Cadastros.Entidades.MaeFormularioBancoArquivo();
            maeFormularioBancoArquivo.MaeInscricaoId = MaeInscricaoId;
            maeFormularioBancoArquivo.Arquivo = pdfbytes;
            maeFormularioBancoArquivo.ChaveArquivo = Guid.NewGuid().ToString();
            maeFormularioBancoArquivo.NomeArquivo = "FormularioMAE.pdf";
            maeFormularioBancoArquivo.TipoArquivo = "application/pdf";
            maeFormularioBancoArquivo.UsuarioId = context.User.Identity.Name;
            maeFormularioBancoArquivo.DataCadastro = DateTime.Now;
            maeFormularioBancoArquivo.DataAlteracao = DateTime.Now;

            var validacao = rnMaeFormularioBancoArquivo.Valida(maeFormularioBancoArquivo);
            if (!validacao.Valido)
            {
                context.Response.Write("Erro ao gravar o PDF");
                context.Response.End();
            }

            rnMaeFormularioBancoArquivo.Atualiza(maeFormularioBancoArquivo);

            context.Response.Write("Sucesso!");
            context.Response.End();

            //HttpContext.Current.Response.ContentType = "application/pdf";
            //HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=FormularioMAE.pdf");
            //HttpContext.Current.Response.BinaryWrite(pdfbytes);
            //HttpContext.Current.Response.Flush();
            //HttpContext.Current.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
