using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Configuration;
using System.Net;
using System.IO;

namespace LyceumGovWebService
{
    /// <summary>
    /// Summary description for ServicoAluno
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ServicoAluno : System.Web.Services.WebService
    {
        [WebMethod]
        public string SalvarAluno()
        {
            int lote = 0;
            Int64 registros = 0;
            int total = 0;
            try
            {
                string url = ConfigurationManager.AppSettings["urlAcessoSalvarAluno"];
                string ip = ConfigurationManager.AppSettings["ipAcessoSalvarAluno"];
                string xml = ConfigurationManager.AppSettings["urlXML"];
                string savePath = ConfigurationManager.AppSettings["savePath"];
                string quantidade = ConfigurationManager.AppSettings["quantidadeAlunos"];
                string baseTotal = ConfigurationManager.AppSettings["baseTotal"];

                XmlTextReader xmlTr1 = new XmlTextReader(url + "ip=" + ip + "&passo=" + 1);

                HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)HttpWebRequest.Create(url + "?ip=" + ip + "&passo=" + 1 + "&basetotal=" + baseTotal);
                httpWebRequest.Timeout = 1800000;
                HttpWebResponse httpWebResponseInstance = (HttpWebResponse)httpWebRequest.GetResponse();
                using (XmlReader reader = XmlReader.Create(httpWebResponseInstance.GetResponseStream()))
                {
                    XmlDocument xmlDoc1 = new XmlDocument();
                    xmlDoc1.Load(reader);
                                        
                    lote = Convert.ToInt32(xmlDoc1.GetElementsByTagName("numerolote")[0].InnerText);
                    registros = Convert.ToInt32(xmlDoc1.GetElementsByTagName("quantidaderegistros")[0].InnerText);
                }

                Techne.Lyceum.RN.ServicoAluno.GravarAluno(lote, 0, registros.ToString());
                
                while (total < registros)
                {
                    HttpWebRequest httpWebRequest1 = (System.Net.HttpWebRequest)HttpWebRequest.Create(url + "ip=" + ip + "&passo=" + 2 + "&numerolote=" + lote + "&numeroinicial=" + (total + 1));
                    HttpWebResponse httpWebResponseInstance1 = (HttpWebResponse)httpWebRequest1.GetResponse();

                    HttpWebRequest httpWebRequestx = (System.Net.HttpWebRequest)HttpWebRequest.Create(xml + lote + ".xml");
                    httpWebRequestx.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
                        HttpWebResponse httpWebResponseInstancex = (HttpWebResponse)httpWebRequestx.GetResponse();
                        //XmlTextReader xmlTr = new XmlTextReader(xml + lote + ".xml");

                        using (XmlReader xmlTr = XmlReader.Create(httpWebResponseInstancex.GetResponseStream()))
                        {
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.Load(xmlTr);
                            try
                            {
                                xmlDoc.Save(savePath + "\\" + "dados" + lote + "-" + (total + 1) + ".xml");
                            }
                            catch(Exception exS)
                            { throw exS; }

                            string matricula = string.Empty, flagEnderecoConflitante = string.Empty, flagDadosCelularPaiConflitante = string.Empty,
                                flagDadosCelularMaeConflitante = string.Empty, flagDadosCelularResponsavelConflitante = string.Empty, fotoAluno = string.Empty,
                                alunoDddCelular = string.Empty, alunoCelular = string.Empty;

                            for (int i = 0; i < xmlDoc.GetElementsByTagName("aluno").Count; i++)
                            {
                                matricula = xmlDoc.GetElementsByTagName("matricula_nova")[i].InnerText;
                                alunoDddCelular = xmlDoc.GetElementsByTagName("aluno_ddd_celular")[i].InnerText;
                                alunoCelular = xmlDoc.GetElementsByTagName("aluno_celular")[i].InnerText;
                                flagEnderecoConflitante = xmlDoc.GetElementsByTagName("flag_endereco_conflitante")[i].InnerText;
                                flagDadosCelularPaiConflitante = xmlDoc.GetElementsByTagName("flag_dados_celular_pai_conflitante")[i].InnerText;
                                flagDadosCelularMaeConflitante = xmlDoc.GetElementsByTagName("flag_dados_celular_mae_conflitante")[i].InnerText;
                                flagDadosCelularResponsavelConflitante = xmlDoc.GetElementsByTagName("flag_dados_celular_responsavel_conflitante")[i].InnerText;
                                fotoAluno = xmlDoc.GetElementsByTagName("foto_aluno")[i].InnerText;
                                total++;
                                Techne.Lyceum.RN.ServicoAluno.GravarAluno(lote, total, matricula, alunoDddCelular, alunoCelular, flagEnderecoConflitante, flagDadosCelularPaiConflitante, flagDadosCelularMaeConflitante, flagDadosCelularResponsavelConflitante, fotoAluno);
                            }
                            
                        }
                }
            }
            catch (Exception ex)
            {
                return ex.Message + ". Registro: " + total;
            }

            return registros + " foram lidos a partir da requisição. Lote: " + lote;
            
        }
    }
}
