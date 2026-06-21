using System.Web;
using System.IO;
using System.Threading;
using System;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.css;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.pipeline.html;
using iTextSharp.tool.xml.pipeline.end;

namespace Techne.Lyceum.RN.Util
{
    public class ExportaPdf
    {
        private void ExportaHtmlSimplesPor(string html, string nomeArquivo, bool paisagem)
        {
            try
            {
                //NOTA: Apenas tags de html simples são reconhecidas!

                //Tratar componentes não reconhecidos
                html = html.Replace("<span", "<label");
                html = html.Replace("/span", "/label");
                html = html.Replace("<br >", "<br />");

                HttpResponse response = HttpContext.Current.Response;
                response.ContentType = "application/pdf";
                response.AddHeader("content-disposition", string.Format("attachment;filename={0}.pdf", nomeArquivo));
                response.Cache.SetCacheability(HttpCacheability.NoCache);

                //Cria papel com suas propriedades
                iTextSharp.text.Rectangle papel;

                if (paisagem)
                {
                    papel = PageSize.A4.Rotate();
                }
                else
                {
                    papel = PageSize.A4;
                }

                //Cria documento
                iTextSharp.text.Document docPdf = new iTextSharp.text.Document(papel, 10, 10, 10, 0);

                //Monta pdf
                PdfWriter.GetInstance(docPdf, response.OutputStream);
                docPdf.Open();

                var parsedHtmlElements = HTMLWorker.ParseToList(new StringReader(html), null);
                foreach (var htmlElement in parsedHtmlElements)
                {
                    docPdf.Add(htmlElement as IElement);
                }

                docPdf.Close();
                //response.Write(docPdf);
                //response.End();
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("Erro ao gerar pdf : {0}", ex.Message);
                throw new Exception(mensagem);
            }
        }

        private void ExportaHtmlCssPor(string html, string nomeArquivo, bool paisagem, string cssText)
        {
            try
            {
                HttpResponse response = HttpContext.Current.Response;
                response.ContentType = "application/pdf";
                response.AddHeader("content-disposition", string.Format("attachment;filename={0}.pdf", nomeArquivo));
                response.Cache.SetCacheability(HttpCacheability.NoCache);

                //Cria papel com suas propriedades
                iTextSharp.text.Rectangle papel;

                if (paisagem)
                {
                    papel = PageSize.A4.Rotate();
                }
                else
                {
                    papel = PageSize.A4;
                }

                //Verifica se existe css
                if (cssText.IsNullOrEmptyOrWhiteSpace())
                {
                    cssText = null;
                }

                //Cria documento
                iTextSharp.text.Document docPdf = new iTextSharp.text.Document(papel, 10, 10, 10, 0);

                //Monta pdf
                PdfWriter.GetInstance(docPdf, response.OutputStream);
                docPdf.Open();

                //var parsedHtmlElements = XMLWorkerHelper.ParseToElementList(html, cssText);
                var parsedHtmlElements = ParseToElementList(html, cssText);

                foreach (var htmlElement in parsedHtmlElements)
                {
                    docPdf.Add(htmlElement as IElement);
                }

                docPdf.Close();
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("Erro ao gerar pdf : {0}", ex.Message);
                throw new Exception(mensagem);
            }
        }

        public void gerapdf(string html, string nomeArquivo, bool paisagem, string cssText)
        {
            try
            {
                HttpResponse response = HttpContext.Current.Response;
                response.ContentType = "application/pdf";
                response.AddHeader("content-disposition", string.Format("attachment;filename={0}.pdf", nomeArquivo));
                response.Cache.SetCacheability(HttpCacheability.NoCache);

                //Cria papel com suas propriedades
                iTextSharp.text.Rectangle papel;

                if (paisagem)
                {
                    papel = PageSize.A4.Rotate();
                }
                else
                {
                    papel = PageSize.A4;
                }

                //Verifica se existe css
                if (cssText.IsNullOrEmptyOrWhiteSpace())
                {
                    cssText = null;
                }

                //Cria documento
                iTextSharp.text.Document docPdf = new iTextSharp.text.Document(papel, 10, 10, 10, 0);

                //Monta pdf
                PdfWriter.GetInstance(docPdf, response.OutputStream);
                docPdf.Open();

                //var parsedHtmlElements = XMLWorkerHelper.ParseToElementList(html, cssText);
                var parsedHtmlElements = ParseToElementList(html, cssText);

                foreach (var htmlElement in parsedHtmlElements)
                {
                    docPdf.Add(htmlElement as IElement);
                }

                docPdf.Close();



                //return docPdf;
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("Erro ao gerar pdf : {0}", ex.Message);
                throw new Exception(mensagem);
            }
        }

        public byte[] gerapdfstream(string html, bool paisagem, string cssText)
        {
            try
            {
                //Cria papel com suas propriedades
                iTextSharp.text.Rectangle papel;

                if (paisagem)
                {
                    papel = PageSize.A4.Rotate();
                }
                else
                {
                    papel = PageSize.A4;
                }

                //Verifica se existe css
                if (cssText.IsNullOrEmptyOrWhiteSpace())
                {
                    cssText = null;
                }

                //Cria documento
                iTextSharp.text.Document docPdf = new iTextSharp.text.Document(papel, 10, 10, 10, 0);

                //Monta pdf
                using (var ms = new MemoryStream())
                {
                    PdfWriter.GetInstance(docPdf, ms);
                    docPdf.Open();
                    var parsedHtmlElements = ParseToElementList(html, cssText);

                    foreach (var htmlElement in parsedHtmlElements)
                    {
                        docPdf.Add(htmlElement as IElement);

                    }



                    docPdf.Close();

                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("Erro ao gerar pdf : {0}", ex.Message);
                throw new Exception(mensagem);
            }
        }

        public void gerapdf2(string html, string nomeArquivo, bool paisagem, string cssText)
        {
            try
            {
                HttpResponse response = HttpContext.Current.Response;
                response.ContentType = "application/pdf";
                response.AddHeader("content-disposition", string.Format("attachment;filename={0}.pdf", nomeArquivo));
                response.Cache.SetCacheability(HttpCacheability.NoCache);

                //Cria papel com suas propriedades
                iTextSharp.text.Rectangle papel;

                if (paisagem)
                {
                    papel = PageSize.A4.Rotate();
                }
                else
                {
                    papel = PageSize.A4;
                }

                //Verifica se existe css
                if (cssText.IsNullOrEmptyOrWhiteSpace())
                {
                    cssText = null;
                }

                //Cria documento
                iTextSharp.text.Document docPdf = new iTextSharp.text.Document(papel, 10, 10, 10, 0);

                //Monta pdf
                PdfWriter.GetInstance(docPdf, response.OutputStream);
                docPdf.Open();




                //var parsedHtmlElements = XMLWorkerHelper.ParseToElementList(html, cssText);
                var parsedHtmlElements = ParseToElementList(html, cssText);

                foreach (var htmlElement in parsedHtmlElements)
                {
                    docPdf.Add(htmlElement as IElement);
                }

                docPdf.Close();


                // return docPdf;
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("Erro ao gerar pdf : {0}", ex.Message);
                throw new Exception(mensagem);
            }
        }
        //Metodo XMLWorkerHelper.ParseToElementList, reescrito para utilizar Encoding.UTF8

        public ElementList ParseToElementList(string html, string css)
        {
            // CSS
            ICSSResolver cssResolver = new StyleAttrCSSResolver();
            if (css != null)
            {
                ICssFile cssFile = XMLWorkerHelper.GetCSS(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(css)));
                cssResolver.AddCss(cssFile);
            }

            // HTML
            CssAppliers cssAppliers = new CssAppliersImpl(FontFactory.FontImp);
            HtmlPipelineContext htmlContext = new HtmlPipelineContext(cssAppliers);
            htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());
            htmlContext.AutoBookmark(false);

            // Pipelines
            ElementList elements = new ElementList();
            ElementHandlerPipeline end = new ElementHandlerPipeline(elements, null);
            HtmlPipeline htmlPipeline = new HtmlPipeline(htmlContext, end);
            CssResolverPipeline cssPipeline = new CssResolverPipeline(cssResolver, htmlPipeline);

            // XML Worker
            XMLWorker worker = new XMLWorker(cssPipeline, true);
            XMLParser p = new XMLParser(worker);
            p.Parse(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(html)));

            return elements;
        }

        public void ExportaHtmlCssOrientacaoPaisagemPor(string html, string nomeArquivo, string cssText)
        {
            this.ExportaHtmlCssPor(html, nomeArquivo, true, cssText);
        }

        public void ExportaHtmlCssPor(string html, string nomeArquivo, string cssText)
        {
            this.ExportaHtmlCssPor(html, nomeArquivo, false, cssText);
        }

        public void ExportaHtmlSimplesPor(string html, string nomeArquivo)
        {
            this.ExportaHtmlSimplesPor(html, nomeArquivo, false);
        }

        public void ExportaHtmlSimplesOrientacaoPaisagemPor(string html, string nomeArquivo)
        {
            this.ExportaHtmlSimplesPor(html, nomeArquivo, true);
        }

        public void gerapdfStreamDownload(string html, string nomeArquivo, bool paisagem, string cssText)
        {
            byte[] pdfBytes = gerapdfstream(html, paisagem, cssText);

            HttpResponse response = HttpContext.Current.Response;
            response.ContentType = "application/pdf";
            response.AddHeader("content-disposition", string.Format("attachment;filename={0}.pdf", nomeArquivo));
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.BinaryWrite(pdfBytes);
            response.End();
        }
    }
}