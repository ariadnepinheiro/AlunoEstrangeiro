using System;
using System.Configuration;
using System.Web;
using Microsoft.Reporting.WebForms;
using Techne.Lyceum.RN.Relatorios;

namespace webReportsHost
{
    public partial class _BIPageViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request.QueryString["rel"] != null)
            {
                string relatorio = HttpUtility.UrlDecode(Request.QueryString["rel"]);

                rptViewer.ProcessingMode = ProcessingMode.Remote;

                rptViewer.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ServerPath"]);
                rptViewer.ServerReport.ReportPath = "/RelatoriosGerenciais/" + relatorio;
                rptViewer.ServerReport.ReportServerCredentials = new TechneReportCredentials(ConfigurationManager.AppSettings["CredentialUser"], ConfigurationManager.AppSettings["CredentialPwd"], ConfigurationManager.AppSettings["CredentialDomain"]);
            }    
        }
    }
}