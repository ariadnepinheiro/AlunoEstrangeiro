using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using Microsoft.Reporting.WebForms;
using Techne.Lyceum.RN.Relatorios;

namespace webReportsHost
{
    public partial class _PageViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Nome do relatório cadastrado no banco de dados Hades. (HD_RELATORIO.RELATORIO)
            string relatorio = Request.QueryString["report"];
            // Grupo ao qual pertence o relatório. (HD_GRUPO_RELATORIOS.GRUPORELAT)
            string gruporelat = Request.QueryString["grp"];
            // Pasta do RS aonde estão instalados os relatórios.
            string FolderRS = ConfigurationManager.AppSettings["FolderRS"];

            string HadesUsu = null;
            string ErrorMsg = null;

            //relatorio = "HistMatricula";
            //gruporelat = "Academico";

            //relatorio = "TesteAtestMatr";
            //gruporelat = "Oracle";

            try
            {

                if (!Techne.Lyceum.RN.Relatorio.TemPermissao(relatorio, gruporelat, getNTUser(), ref HadesUsu, ref ErrorMsg))
                {
                    lblMsg.Text = ErrorMsg;
                    return;
                }

                string ServerPath = ConfigurationManager.AppSettings["ServerPath"];
                string user = ConfigurationManager.AppSettings["CredentialUser"];
                string pwd = ConfigurationManager.AppSettings["CredentialPwd"];
                string domain = ConfigurationManager.AppSettings["CredentialDomain"];

                if (!Page.IsPostBack)
                {
                    rptViewer.ProcessingMode = ProcessingMode.Remote;
                    rptViewer.ServerReport.ReportServerUrl = new Uri(ServerPath);
                    rptViewer.ServerReport.ReportPath = @"/" + FolderRS + "/" + gruporelat + "/" + relatorio;
                    IReportServerCredentials key = new TechneReportCredentials(user, pwd, domain);
                    rptViewer.ServerReport.ReportServerCredentials = key;
                    List<ReportParameter> paramList = new List<ReportParameter>();
                    paramList.Add(new ReportParameter("usuario", User.Identity.Name, true));

                    foreach (var parameter in Request.QueryString.AllKeys)
                    {
                        if (parameter != "report"
                            && parameter != "grp")
                        {
                            paramList.Add(new ReportParameter(parameter, Request.QueryString[parameter], true));
                        }
                    }

                    rptViewer.ServerReport.SetParameters(paramList);
                }
            }
            catch (Exception exc)
            {
                string msgError = exc.Message;
                int J = msgError.IndexOf("rsItemNotFound", 0);
                if (J > 0)
                {
                    lblMsg.Text = "O relatório " + "/" + FolderRS + "/" + gruporelat + "/" + relatorio + " não pode ser localizado no servidor de relatórios.";
                    rptViewer.Visible = false;
                    return;
                }
                else
                    throw exc;
            }
            rptViewer.Visible = true;

        }

        private string getNTUser()
        {
            string userName = HttpContext.Current.User.Identity.Name.ToString();
            if (userName == null)
                throw new Exception("Nao foi possivel obter nome do usuário, verifique as configurações do aplicativo");

            return userName;
        }
    }
}
