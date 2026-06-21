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
        byte[] bytes = (byte[])carregaImagem(id, tabelaArquivo);
        
        context.Response.Buffer = true;
        context.Response.Charset = "UTF-8";
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        context.Response.ContentType = "application/pdf";
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
                case "certificacaoescolar.ENCCEJAALUNO":
                    {
                        Techne.Lyceum.RN.Certificacao.EnccejaDocumentoArquivo rnEnccejaDocumentoArquivo = new Techne.Lyceum.RN.Certificacao.EnccejaDocumentoArquivo();
                        fileBytes = rnEnccejaDocumentoArquivo.ObtemArquivoPor(id);
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