using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Techne.Lyceum.RN.Util
{
    public class Log
    {    
        private static Object thisLock = new Object();

        private static bool ativado = true;
        public static bool Ativado { get { return ativado; } set { ativado = value; } }

        public static void Adicionar(params String[] mensagens)
        {
            if (Ativado)
            {
                lock (thisLock)
                {
                    String agora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    StringBuilder str = new StringBuilder();
                    for (int i = 0; i < mensagens.Length; i++)
                        str.AppendLine(agora + "\t" + mensagens[i]);

                    string strFile = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/logErro-" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
                    //File.AppendAllText(DateTime.Now.ToString("ddMMyyyy") + Constantes.CAMINHO_LOG, str.ToString());

                    //Se arquivo não existir
                    if (!File.Exists(strFile))
                    {
                        //Criar o arquivo, 
                        //Estou usando o using para fazer o Dispose automático do arquivo após criá-lo.
                        using (FileStream fs = File.Create(strFile)) { }
                    }

                    File.AppendAllText(strFile, str.ToString());
                }
            }
        }

        public static void Adicionar(string diretorio, string extensao, params String[] mensagens)
        {
            Adicionar(diretorio, extensao, String.Empty, mensagens);
        } 

        public static void Adicionar(string diretorio, string extensao, string titulo, params String[] mensagens)
        {
            if (Ativado)
            {
                lock (thisLock)
                {
                    String agora = DateTime.Now.ToString("HH:mm:ss");
                    StringBuilder str = new StringBuilder();
                    string path = System.Web.HttpContext.Current.Server.MapPath(diretorio + DateTime.Now.ToString("yyyyMMdd") + "." + extensao); 
                    
                    str.AppendLine(titulo);

                    for (int i = 0; i < mensagens.Length; i++)
                        str.AppendLine(agora + "\t" + mensagens[i]);
                    File.AppendAllText(path, str.ToString());
                }
            }
        }        
    }

    public class LogItem
    {
        public DateTime Data { get; set; }        
    }
}
