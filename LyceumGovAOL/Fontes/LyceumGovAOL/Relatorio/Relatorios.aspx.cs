namespace Techne.Lyceum.Net.Relatorio
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using Microsoft.Reporting.WebForms;
    using Techne.Lyceum.RN.Relatorios;
    using Techne.Web;

    [NavUrl("~/Relatorio/Relatorios.aspx"), ControlText("Relatório"), Title("Relatório")]
    public partial class Relatorios : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var relatorio = this.Request.QueryString["report"];
            var gruporelat = this.Request.QueryString["grp"];
            var folderRs = ConfigurationManager.AppSettings["FolderRS"];

            try
            {
                var serverPath = ConfigurationManager.AppSettings["ServerPath"];
                var user = ConfigurationManager.AppSettings["CredentialUser"];
                var pwd = ConfigurationManager.AppSettings["CredentialPwd"];
                var domain = ConfigurationManager.AppSettings["CredentialDomain"];

                if (!this.Page.IsPostBack)
                {
                    this.rptViewer.ProcessingMode = ProcessingMode.Remote;
                    this.rptViewer.ServerReport.ReportServerUrl = new Uri(serverPath);
                    this.rptViewer.ServerReport.ReportPath = @"/" + folderRs + "/" + gruporelat + "/" + relatorio;

                    var key = new TechneReportCredentials(user, pwd, domain);

                    this.rptViewer.ServerReport.ReportServerCredentials = key;

                    if (relatorio == "rsaolboletim")
                    {
                        var paramList = new List<ReportParameter>();
                        var boletim = this.Request.QueryString["value"].Split('|');

                        paramList.Add(new ReportParameter("P_ALUNO", boletim[0], false));
                        paramList.Add(new ReportParameter("P_SUBPERIODO", boletim[1], false));
                        paramList.Add(new ReportParameter("P_ANO", boletim[2], false));
                        paramList.Add(new ReportParameter("P_SEMESTRE", boletim[3], false));

                        this.rptViewer.ServerReport.SetParameters(paramList);
                    }

                    if (relatorio == "RAolBoletimInd")
                    {
                        var paramList = new List<ReportParameter>();
                        var boletim = this.Request.QueryString["value"].Split('|');

                        paramList.Add(new ReportParameter("P_ALUNO", boletim[0], false));
                        paramList.Add(new ReportParameter("P_ANO", boletim[1], false));
                        paramList.Add(new ReportParameter("P_SEMESTRE", boletim[2], false));

                        this.rptViewer.ServerReport.SetParameters(paramList);
                    }
                }
            }
            catch (Exception ex)
            {
                var msgError = ex.Message;
                var i = msgError.IndexOf("rsItemNotFound", 0);

                if (i > 0)
                {
                    this.lblMsg.Text = "O relatório " + "/" + folderRs + "/" + gruporelat + "/" + relatorio + " não pode ser localizado no servidor de relatórios.";
                    this.rptViewer.Visible = false;

                    return;
                }
                
                throw ex;
            }

            this.rptViewer.Visible = true;
        }
    }
}