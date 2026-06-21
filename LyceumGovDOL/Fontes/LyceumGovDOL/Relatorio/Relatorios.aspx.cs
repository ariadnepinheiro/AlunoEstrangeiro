using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using Microsoft.Reporting.WebForms;
using Techne.Lyceum.RN.Relatorios;
using Techne.Web;

namespace Techne.Lyceum.Net.Relatorio
{
    [NavUrl("~/Relatorio/Relatorios.aspx"),
      ControlText("Relatório"),
      Title("Relatório"),]
    public partial class Relatorios : TPage
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            string relatorio = Request.QueryString["report"];
            if (relatorio == "rscandidatos" || relatorio == "rsconcursodocente")
                MasterPageFile = "~/Modulos/LyceumConexaoMaster.Master";
            if (relatorio == "rshorariodocente")
                MasterPageFile = "~/Modulos/LyceumMaster.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            // Nome do relatório cadastrado no banco de dados Hades. (HD_RELATORIO.RELATORIO)
            string relatorio = Request.QueryString["report"];
            // Grupo ao qual pertence o relatório. (HD_GRUPO_RELATORIOS.GRUPORELAT)
            string gruporelat = Request.QueryString["grp"];
            // Pasta do RS aonde estão instalados os relatórios.
            string FolderRS = ConfigurationManager.AppSettings["FolderRS"];

            try
            {

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

                    if (relatorio == "rscandidatos")
                    {
                        List<ReportParameter> paramList = new List<ReportParameter>();
                        paramList.Add(new ReportParameter("P_CONCURSO", QueryStringDecodificada["concurso"], false));
                        paramList.Add(new ReportParameter("P_NUCLEO", QueryStringDecodificada["nucleo"], false));
                        paramList.Add(new ReportParameter("P_CATEGORIA", QueryStringDecodificada["categoria"], false));
                        paramList.Add(new ReportParameter("P_FUNCAO", QueryStringDecodificada["disciplina"], false));
                        if (!String.IsNullOrEmpty(QueryStringDecodificada["candidato"]))
                            paramList.Add(new ReportParameter("P_CANDIDATO", QueryStringDecodificada["candidato"], false));
                        rptViewer.ServerReport.SetParameters(paramList);
                    }
                    else if (relatorio == "rshorariodocente")
                    {
                        List<ReportParameter> paramList = new List<ReportParameter>();
                        string matricula = Request.QueryString["matricula"];
                        paramList.Add(new ReportParameter("P_MATRICULA", matricula, false));
                        rptViewer.ServerReport.SetParameters(paramList);
                    }
                    else if (relatorio == "rsconcursodocente")
                    {
                        List<ReportParameter> paramList = new List<ReportParameter>();
                        paramList.Add(new ReportParameter("P_CANDIDATO", QueryStringDecodificada["candidato"], false));
                        paramList.Add(new ReportParameter("P_CONCURSO", QueryStringDecodificada["concurso"], false));
                        rptViewer.ServerReport.SetParameters(paramList);
                    }
                    else if (relatorio == "rsconcursodocenteexterno")
                    {
                        List<ReportParameter> paramList = new List<ReportParameter>();
                        paramList.Add(new ReportParameter("P_CANDIDATO", QueryStringDecodificada["candidato"], false));
                        paramList.Add(new ReportParameter("P_CONCURSO", QueryStringDecodificada["concurso"], false));
                        rptViewer.ShowPrintButton = true;
                        rptViewer.ShowExportControls = true;                        
                        rptViewer.ServerReport.SetParameters(paramList);
                    }
                    else if (relatorio == "CurriculoMinimo")
                    {
                        List<ReportParameter> paramList = new List<ReportParameter>();
                        paramList.Add(new ReportParameter("matricula", QueryStringDecodificada["matricula"], false));
                        paramList.Add(new ReportParameter("disciplina", QueryStringDecodificada["disciplina"], false));
                        paramList.Add(new ReportParameter("turma", QueryStringDecodificada["turma"], false));
                        paramList.Add(new ReportParameter("ano", QueryStringDecodificada["ano"], false));
                        paramList.Add(new ReportParameter("periodo", QueryStringDecodificada["periodo"], false));
                        paramList.Add(new ReportParameter("subperiodo", QueryStringDecodificada["subperiodo"], false));
                        paramList.Add(new ReportParameter("curso", QueryStringDecodificada["curso"], false));
                        paramList.Add(new ReportParameter("modalidade", QueryStringDecodificada["modalidade"], false));
                        paramList.Add(new ReportParameter("tipoCurso", QueryStringDecodificada["tipoCurso"], false));
                        paramList.Add(new ReportParameter("serie", QueryStringDecodificada["serie"], false));
                        paramList.Add(new ReportParameter("nome", QueryStringDecodificada["nome"], false));
                        paramList.Add(new ReportParameter("escola", QueryStringDecodificada["escola"], false));
                        paramList.Add(new ReportParameter("nomedisciplina", QueryStringDecodificada["nomedisciplina"], false));
                        rptViewer.ServerReport.SetParameters(paramList);
                    }
                    else if (relatorio == "Filipeta")
                    {
                        List<ReportParameter> paramList = new List<ReportParameter>();
                        paramList.Add(new ReportParameter("matricula", QueryStringDecodificada["matricula"], false));
                        paramList.Add(new ReportParameter("disciplina", QueryStringDecodificada["disciplina"], false));
                        paramList.Add(new ReportParameter("turma", QueryStringDecodificada["turma"], false));
                        paramList.Add(new ReportParameter("ano", QueryStringDecodificada["ano"], false));
                        paramList.Add(new ReportParameter("periodo", QueryStringDecodificada["periodo"], false));
                        paramList.Add(new ReportParameter("subperiodo", QueryStringDecodificada["subperiodo"], false));
                        paramList.Add(new ReportParameter("nome", QueryStringDecodificada["nome"], false));
                        paramList.Add(new ReportParameter("escola", QueryStringDecodificada["escola"], false));
                        paramList.Add(new ReportParameter("semestre", QueryStringDecodificada["semestre"], false));
                        paramList.Add(new ReportParameter("nomedisciplina", QueryStringDecodificada["nomedisciplina"], false));
                        paramList.Add(new ReportParameter("protocolo", QueryStringDecodificada["protocolo"], false));
                        rptViewer.ServerReport.SetParameters(paramList);
                    }

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
                throw new Exception("Nao foi possível obter nome do usuário, verifique as configurações do aplicativo");

            return userName;
        }
    }
}
