using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace Techne.Lyceum.RN.Util
{
   public class ExportaCsv
    {
        /// <summary>
        /// Exporta os dados do objeto System.Data.DataTable para arquivo CSV retornando um objeto Stream carregado
        /// </summary>
        /// <param name="dt">DataTable carregado</param>
        /// <param name="separador">Caractere separador dos valores</param>
        /// <returns></returns>
        public Stream ExportaDataTableCSV(DataTable dt, string separador)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter sw = new StreamWriter(stream, Encoding.UTF8);

            int iColCount = dt.Columns.Count;

            for (int i = 0; i < iColCount; i++)
            {
                sw.Write(dt.Columns[i]);
                if (i < iColCount - 1)
                {
                    sw.Write(separador);
                }
            }

            sw.Write(sw.NewLine);

            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        sw.Write(dr[i].ToString());
                    }
                    if (i < iColCount - 1)
                    {
                        sw.Write(separador);
                    }
                }
                sw.Write(sw.NewLine);
            }

            sw.Flush();
            return stream;
        }
    }
}
