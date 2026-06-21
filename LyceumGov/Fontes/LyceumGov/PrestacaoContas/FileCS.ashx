<%@ WebHandler Language="C#" Class="FileCS" %>

using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
public class FileCS : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        int id = int.Parse(context.Request.QueryString["Id"]);
        string tabelaArquivo = Convert.ToString(context.Request.QueryString["Tabela"]);
        string tipoArquivo = Convert.ToString(context.Request.QueryString["TipoArquivo"]); 
        byte[] bytes = (byte[])carregaImagem(id, tabelaArquivo);
        if (bytes == null)
            return;
        
        context.Response.Buffer = true;
        context.Response.Charset = "UTF-8";
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

        if (string.IsNullOrEmpty(tipoArquivo))
            context.Response.ContentType = "application/pdf";
        else
            context.Response.ContentType = tipoArquivo;
        
        context.Response.BinaryWrite(bytes);
        context.Response.Flush();
        context.Response.End();
    }

    private byte[] carregaImagem(int id, string tabelaArquivo)
    {
        try
        {
            byte[] fileBytes = null;
            
            switch (tabelaArquivo)
            {
                case "FornecedorDocumentoArquivo":
                    {
                        Techne.Lyceum.RN.PrestacaoContas.FornecedorDocumentoArquivo rnFornecedorDocumentoArquivo = new Techne.Lyceum.RN.PrestacaoContas.FornecedorDocumentoArquivo();
                        fileBytes = rnFornecedorDocumentoArquivo.ObtemArquivoPor(id);
                        break;
                    }
                case "OperacaoDocumentos":
                    {
                        Techne.Lyceum.RN.PrestacaoContas.OperacaoDocumentos rnOperacaoDocumentos = new Techne.Lyceum.RN.PrestacaoContas.OperacaoDocumentos();
                        fileBytes = rnOperacaoDocumentos.ObtemArquivoPor(id);
                        break;
                    }                    
                case "DeclaracaoFiscalArquivo":
                    {
                        Techne.Lyceum.RN.PrestacaoContas.DeclaracaoFiscalArquivo rnDeclaracaoFiscalArquivo = new Techne.Lyceum.RN.PrestacaoContas.DeclaracaoFiscalArquivo();
                        fileBytes = rnDeclaracaoFiscalArquivo.ObtemArquivoPor(id);
                        break;
                    }

                case "ArquivoAae":
                    {
                        Techne.Lyceum.RN.PrestacaoContas.ArquivoAae rnArquivoAae = new Techne.Lyceum.RN.PrestacaoContas.ArquivoAae();
                        fileBytes = rnArquivoAae.ObtemArquivoPor(id);
                        break;
                    }

                case "ExtratoBancarioArquivo":
                    {
                        Techne.Lyceum.RN.PrestacaoContas.ExtratoBancarioArquivo rnExtratoBancarioArquivo = new Techne.Lyceum.RN.PrestacaoContas.ExtratoBancarioArquivo();
                        fileBytes = rnExtratoBancarioArquivo.ObtemArquivoPor(id);
                        break;
                    }

                case "ExigenciaExtratoArquivo":
                    {
                        Techne.Lyceum.RN.PrestacaoContas.ExigenciaExtratoArquivo rnExigenciaExtratoArquivo = new Techne.Lyceum.RN.PrestacaoContas.ExigenciaExtratoArquivo();
                        fileBytes = rnExigenciaExtratoArquivo.ObtemArquivoPor(id);
                        break;
                    }
                case "OperacaoExigenciaArquivo":
                    {
                        Techne.Lyceum.RN.PrestacaoContas.OperacaoExigenciaArquivo rnOperacaoExigenciaArquivo = new Techne.Lyceum.RN.PrestacaoContas.OperacaoExigenciaArquivo();
                        fileBytes = rnOperacaoExigenciaArquivo.ObtemArquivoPor(id);
                        break;
                    }
                case "ExigenciaEventoArquivo":
                    {
                        Techne.Lyceum.RN.PrestacaoContas.ExigenciaEventoArquivo rnExigenciaEventoArquivo = new Techne.Lyceum.RN.PrestacaoContas.ExigenciaEventoArquivo();
                        fileBytes = rnExigenciaEventoArquivo.ObtemArquivoPor(id);
                        break;
                    }

                case "ExigenciaEventoArquivo2":
                    {
                        Techne.Lyceum.RN.PrestacaoContas.ExigenciaEventoArquivo rnExigenciaEventoArquivo = new Techne.Lyceum.RN.PrestacaoContas.ExigenciaEventoArquivo();
                        fileBytes = rnExigenciaEventoArquivo.ObtemArquivoPorId(id);
                        break;
                    }
                    
                case "AplicacaoFinanceiraComprovanteArquivo":
                    {
                        Techne.Lyceum.RN.PrestacaoContas.AplicacaoFinanceiraComprovanteArquivo rnAplicacaoFinanceiraComprovanteArquivo = new Techne.Lyceum.RN.PrestacaoContas.AplicacaoFinanceiraComprovanteArquivo();
                        fileBytes = rnAplicacaoFinanceiraComprovanteArquivo.ObtemArquivoPor(id);
                        break;
                    }
                    
                case "OrcamentoArquivo":
                    {
                        Techne.Lyceum.RN.PrestacaoContas.OrcamentoArquivo rnOrcamentoArquivo = new Techne.Lyceum.RN.PrestacaoContas.OrcamentoArquivo();
                        fileBytes = rnOrcamentoArquivo.ObtemArquivoPor(id);
                        break;
                    }

                case "EventoNotaFiscalArquivo":
                    {
                        Techne.Lyceum.RN.PrestacaoContas.EventoNotaFiscalArquivo rnEventoNotaFiscalArquivo = new Techne.Lyceum.RN.PrestacaoContas.EventoNotaFiscalArquivo();
                        fileBytes = rnEventoNotaFiscalArquivo.ObtemArquivoPor(id);
                        break;
                    }

                case "ComprovantePagamentoArquivo":
                    {
                        Techne.Lyceum.RN.PrestacaoContas.ComprovantePagamentoArquivo rnComprovantePagamentoArquivo = new Techne.Lyceum.RN.PrestacaoContas.ComprovantePagamentoArquivo();
                        fileBytes = rnComprovantePagamentoArquivo.ObtemArquivoPor(id);
                        break;
                    }
                    
                case "EvidenciaArquivo":
                    {
                        Techne.Lyceum.RN.PrestacaoContas.EvidenciaArquivo rnEvidenciaArquivo = new Techne.Lyceum.RN.PrestacaoContas.EvidenciaArquivo();
                        fileBytes = rnEvidenciaArquivo.ObtemArquivoPor(id);
                        break;
                    }
                case "DocenteCandidatoArquivo":
                    {
                        Techne.Lyceum.RN.RecursosHumanos.DocenteCandidatoArquivo rnDocenteCandidatoArquivo = new Techne.Lyceum.RN.RecursosHumanos.DocenteCandidatoArquivo();
                        fileBytes = rnDocenteCandidatoArquivo.ObtemDocumentoPor(id);
                        break;
                    }                               
                default:
                    break;
            }

            return fileBytes;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
   
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}