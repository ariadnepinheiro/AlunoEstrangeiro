using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Techne.Lyceum.RN.Util
{
    public class Download
    {
        /// <summary>
        /// Realiza o download automático do arquivo
        /// </summary>
        /// <param name="nomeArquivo"></param>
        /// <param name="ArrayArquivo"></param>
        public void RealizaDownload(string nomeArquivo, byte[] ArrayArquivo, Page pg)
        {
            long fileSize = (long)ArrayArquivo.Length;

            pg.Response.Clear();
            pg.Response.ContentType = "application/octet-stream";
            pg.Response.BufferOutput = true;

            pg.Response.AddHeader("Pragma", "public");
            pg.Response.AddHeader("Expires", "0");
            pg.Response.AddHeader("Cache-Control", "must-revalidate, post-check=0, pre-check=0");
            pg.Response.AddHeader("Cache-Control", "public");
            pg.Response.AddHeader("Content-Description", "File Transfer");
            pg.Response.AddHeader("Content-type", "application/octet-stream");
            pg.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + nomeArquivo + "\"");
            pg.Response.AddHeader("Content-Transfer-Encoding", "binary");
            pg.Response.AddHeader("Content-Length", fileSize.ToString());

            pg.Response.BinaryWrite(ArrayArquivo);
            pg.Response.Flush();
            pg.Response.Close();
        }
    }
}
