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
     Title("Relatório")]
    public partial class Relatorios : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Nome do relatório cadastrado no banco de dados Hades. (HD_RELATORIO.RELATORIO)
            var relatorio = this.Request.QueryString["report"];

            // Grupo ao qual pertence o relatório. (HD_GRUPO_RELATORIOS.GRUPORELAT)
            var gruporelat = this.Request.QueryString["grp"];

            // Pasta do RS aonde estão instalados os relatórios.
            var folderRs = ConfigurationManager.AppSettings["FolderRS"];

            var parametro = this.Request.QueryString["param"];

            string hadesUsu = null;
            string errorMsg = null;

            try
            {
                if (!RN.Relatorio.TemPermissao(relatorio, gruporelat, this.GetNTUser(), ref hadesUsu, ref errorMsg))
                {
                    this.lblMsg.Text = errorMsg;

                    return;
                }

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

                    if (relatorio == "rsconcursodocente")
                    {
                        var paramList = new List<ReportParameter>();

                        paramList.Add(new ReportParameter("P_CANDIDATO", this.QueryStringDecodificada["candidato"], false));
                        paramList.Add(new ReportParameter("P_CONCURSO", this.QueryStringDecodificada["concurso"], false));

                        this.rptViewer.ServerReport.SetParameters(paramList);
                    }
					else if (relatorio == "rsrescisaoealteracao")
					{
						var paramList = new List<ReportParameter>();

						paramList.Add(new ReportParameter("TIPO_RELATORIO", this.QueryStringDecodificada["tipo"], false));
						paramList.Add(new ReportParameter("P_CANDIDATO", this.QueryStringDecodificada["candidato"], false));
						paramList.Add(new ReportParameter("P_CONCURSO", this.QueryStringDecodificada["concurso"], false));

						this.rptViewer.ServerReport.SetParameters(paramList);
					}
					else if (relatorio == "rspropcontrtemp")
					{
						var paramList = new List<ReportParameter>();

						paramList.Add(new ReportParameter("MATRICULA", this.QueryStringDecodificada["matricula"], false));
						paramList.Add(new ReportParameter("CONCURSO", this.QueryStringDecodificada["concurso"], false));

						this.rptViewer.ServerReport.SetParameters(paramList);
					}
					else if (relatorio == "rspropcontrtempinterno")
					{
						var paramList = new List<ReportParameter>();

						paramList.Add(new ReportParameter("CANDIDATO", this.QueryStringDecodificada["candidato"], false));
						paramList.Add(new ReportParameter("CONCURSO", this.QueryStringDecodificada["concurso"], false));

						this.rptViewer.ServerReport.SetParameters(paramList);
					}
					else if (relatorio == "rsconvocacaocorreios" ||
							 relatorio == "rsconvocacaocoordenadoria")
					{
						var paramList = new List<ReportParameter>();

						paramList.Add(new ReportParameter("P_CONCURSO", this.QueryStringDecodificada["concurso"], false));
						paramList.Add(new ReportParameter("P_NUCLEO", this.QueryStringDecodificada["nucleo"], false));
						//paramList.Add(new ReportParameter("P_CATEGORIA", this.QueryStringDecodificada["categoria"], false));
						paramList.Add(new ReportParameter("P_DISCIPLINA", this.QueryStringDecodificada["disciplina"], false));
						paramList.Add(new ReportParameter("P_DT_APRESENTACAO", this.QueryStringDecodificada["dt_apresentacao"], false));

						this.rptViewer.ServerReport.SetParameters(paramList);
					}
					else if (parametro == "diretor")
					{
						var paramList = new List<ReportParameter>();

						paramList.Add(new ReportParameter("SERVMAT", RN.Usuarios.ObterMatricula(this.User.Identity.Name), false));
						paramList.Add(new ReportParameter("usuario", this.User.Identity.Name, false));

						this.rptViewer.ServerReport.SetParameters(paramList);
					}
                    else if (relatorio == "Pendencia_CHTC")
                    {
                        var paramList = new List<ReportParameter>();
                        paramList.Add(new ReportParameter("setor", this.QueryStringDecodificada["setor"], false));
                        paramList.Add(new ReportParameter("escola", this.QueryStringDecodificada["escola"], false));

                        this.rptViewer.ServerReport.SetParameters(paramList);
                    }
					else if (parametro == "turma")
					{
						var paramList = new List<ReportParameter>();
						var valores = RN.Coordenadoria.ObterCoordenadoriaUsuario(this.User.Identity.Name);

						if (valores.Length != 2)
						{
							return;
						}

						paramList.Add(new ReportParameter("carencia", "3", false));
						paramList.Add(new ReportParameter("coord", valores.Split('|')[0], false));
						paramList.Add(new ReportParameter("municipio", (string)null, false));
						paramList.Add(new ReportParameter("escola", valores.Split('|')[1], false));
						paramList.Add(new ReportParameter("usuario", this.User.Identity.Name, false));

						this.rptViewer.ServerReport.SetParameters(paramList);
					}
					else if (parametro == "escola")
					{
						var paramList = new List<ReportParameter>();
						var valores = RN.Coordenadoria.ObterCoordenadoriaUsuario(this.User.Identity.Name);

						if (valores.Length < 2)
						{
							return;
						}

						paramList.Add(new ReportParameter("coord", valores.Split('|')[0], false));
						paramList.Add(new ReportParameter("municipio", (string)null, false));
						paramList.Add(new ReportParameter("escola", valores.Split('|')[1], false));
						paramList.Add(new ReportParameter("usuario", this.User.Identity.Name, false));

						this.rptViewer.ServerReport.SetParameters(paramList);
					}
					else if (relatorio == "RelLogEncerramentoVagas")
					{
						var paramList = new List<ReportParameter>();

						paramList.Add(new ReportParameter("ANO", this.QueryStringDecodificada["ano"], false));
						paramList.Add(new ReportParameter("PERIODO", this.QueryStringDecodificada["periodo"], false));

						this.rptViewer.ServerReport.SetParameters(paramList);
					}
					else if (relatorio == "FichaDeMatriculaFinal")
					{
						var paramList = new List<ReportParameter>();

						paramList.Add(new ReportParameter("Id_Confirmacao_Matricula", this.QueryStringDecodificada["Id_Confirmacao_Matricula"], false));

						this.rptViewer.ServerReport.SetParameters(paramList);
					}
                    else if (relatorio == "Filipeta" || relatorio == "FilipetaConsolidada" || relatorio == "FilipetaConsolidadaHistorico")
					{
						List<ReportParameter> paramList = new List<ReportParameter>();
						paramList.Add(new ReportParameter("matricula", QueryStringDecodificada["matricula"], false));
                        paramList.Add(new ReportParameter("idvinculo", QueryStringDecodificada["idvinculo"], false));
						paramList.Add(new ReportParameter("disciplina", QueryStringDecodificada["disciplina"], false));
						paramList.Add(new ReportParameter("turma", QueryStringDecodificada["turma"], false));
						paramList.Add(new ReportParameter("ano", QueryStringDecodificada["ano"], false));
						paramList.Add(new ReportParameter("periodo", QueryStringDecodificada["periodo"], false));
						paramList.Add(new ReportParameter("subperiodo", QueryStringDecodificada["subperiodo"], false));
						paramList.Add(new ReportParameter("nome", QueryStringDecodificada["nome"], false));
						paramList.Add(new ReportParameter("escola", QueryStringDecodificada["escola"], false));						
						paramList.Add(new ReportParameter("nomedisciplina", QueryStringDecodificada["nomedisciplina"], false));
                        if (relatorio == "Filipeta")
                        {
                            paramList.Add(new ReportParameter("semestre", QueryStringDecodificada["semestre"], false));
                            paramList.Add(new ReportParameter("protocolo", QueryStringDecodificada["protocolo"], false));
                        }
						rptViewer.ServerReport.SetParameters(paramList);
					}
                    else if (relatorio == "DesalocacaoMigracaoPorProfessor")
                    {
                        var paramList = new List<ReportParameter>();

                        paramList.Add(new ReportParameter("IDVINCULO", this.QueryStringDecodificada["IDVINCULO"], false));
                        paramList.Add(new ReportParameter("usuario", this.User.Identity.Name, false));

                        this.rptViewer.ServerReport.SetParameters(paramList);
                    }
                    else if (relatorio == "RelCertificadoConcluinte")
                    {
                        var paramList = new List<ReportParameter>();

                        paramList.Add(new ReportParameter("cpf", this.QueryStringDecodificada["cpf"], false));

                        this.rptViewer.ServerReport.SetParameters(paramList);
                    }
                    else if (relatorio == "RelDiplomaConcluinte")
                    {
                        var paramList = new List<ReportParameter>();

                        paramList.Add(new ReportParameter("cpf", this.QueryStringDecodificada["cpf"], false));

                        this.rptViewer.ServerReport.SetParameters(paramList);
                    }
					else if (parametro == "empty")
					{
						var paramList = new List<ReportParameter>();

						paramList.Add(new ReportParameter("usuario", this.User.Identity.Name, false));

						this.rptViewer.ServerReport.SetParameters(paramList);
					}
                }
            }
            catch (Exception ex)
            {
                var msgError = ex.Message;

                if (msgError.IndexOf("rsItemNotFound", 0) > 0)
                {
                    this.lblMsg.Text = "O relatório " + "/" + folderRs + "/" + gruporelat + "/" + relatorio + " não pode ser localizado no servidor de relatórios.";
                    this.rptViewer.Visible = false;

                    return;
                }

                throw ex;
            }

            this.rptViewer.Visible = true;
        }

        private string GetNTUser()
        {
            var userName = HttpContext.Current.User.Identity.Name;

            if (userName == null)
            {
                throw new Exception("Nao foi possível obter nome do usuário, verifique as configurações do aplicativo");
            }

            return userName;
        }
    }
}