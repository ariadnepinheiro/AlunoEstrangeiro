using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using Microsoft.Reporting.WebForms;
using Proderj.DOL.Service;
using Proderj.Foundation.Framework;
using Proderj.Foundation.Framework.Web.Seguranca;
using System.Web.UI.WebControls;
using Proderj.DOL.WebApp.Models;
using Resources;

namespace Proderj.DOL.WebApp
{
    public partial class Relatorios : System.Web.UI.Page
    {
        protected CabecalhoViewModel cabecalhoModelo = new CabecalhoViewModel
                                                        {
                                                            BotaoAjudaHabilitado = false,
                                                            BotaoInicioHabilitado = false,
                                                            BotaoSairHabilitado = true
                                                        };

        protected bool EhPeriodoCampanhaLancamentoNotas { get; private set; }        

        protected void Page_Init()
        {
            //throw new System.Exception("Xabu");
            EnableViewState = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (User == null)
                Response.Redirect("~/");

            if (!(User is DTODocenteLogadoPrincipal))
                Response.Redirect("~/");

            DateTime dataAtual = DateTime.Now;
            DateTime dataInicioCampanha, dataFimCampanha;
            bool bloqueiaConsultaQHIEmCampanha = false;

            DateTime.TryParse(System.Configuration.ConfigurationManager.AppSettings["DataInicioCampanha"].ToString(), out dataInicioCampanha);            
            DateTime.TryParse(System.Configuration.ConfigurationManager.AppSettings["DataFimCampanha"].ToString(), out dataFimCampanha);
            Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings["BloqueiaConsultaQHIEmCampanha"].ToString(), out bloqueiaConsultaQHIEmCampanha);

            this.EhPeriodoCampanhaLancamentoNotas = dataAtual >= dataInicioCampanha && dataAtual <= dataFimCampanha;

            // Nome do relatório cadastrado no banco de dados Hades. (HD_RELATORIO.RELATORIO)
            string relatorio = Request.QueryString["relatorio"];
            // Grupo ao qual pertence o relatório. (HD_GRUPO_RELATORIOS.GRUPORELAT)
            string grupo = Request.QueryString["grupo"];
            // Pasta do RS aonde estão instalados os relatórios.

            if (!String.IsNullOrWhiteSpace(relatorio)
                && relatorio.ToLower().Equals("chdocenteonline") 
                && !String.IsNullOrWhiteSpace(grupo)
                && grupo.ToLower().Equals("dol") 
                && EhPeriodoCampanhaLancamentoNotas
                && bloqueiaConsultaQHIEmCampanha)
            {
                rptViewer.Visible = false;
                lblMsg.Text = String.Format(@"Não é possível exibir informações de alocação de QHI durante campanha de lançamento de notas.<br/>
                Tente novamente em {0}", dataFimCampanha.AddDays(1).ToString("dd/MM/yyyy")); 
            }
            else
            {
                string pastaReport = ConfigurationManager.AppSettings["ReportServices_Pasta"];
                string caminhoServidor = ConfigurationManager.AppSettings["ReportServices_CaminhoServidor"];
                string usuario = ConfigurationManager.AppSettings["ReportServices_Credencial_Usuario"];
                string senha = ConfigurationManager.AppSettings["ReportServices_Credencial_Senha"];
                string dominio = ConfigurationManager.AppSettings["ReportServices_Credencial_Dominio"];


                try
                {
                    if (!Page.IsPostBack)
                    {
                        rptViewer.ShowPrintButton = true;
                        rptViewer.ProcessingMode = ProcessingMode.Remote;
                        rptViewer.ServerReport.ReportServerUrl = new Uri(caminhoServidor);
                        rptViewer.ServerReport.ReportPath = @"/" + pastaReport + "/" + grupo + "/" + relatorio;
                        rptViewer.ServerReport.ReportServerCredentials = (IReportServerCredentials)new ReportCredentials(usuario, senha, dominio);


                        NameValueCollection queryStringDecodificada = null;

                        if (Request.QueryString["Chave"] != null)
                        {
                            queryStringDecodificada = QueryStringDecode.DecodificaQueryString(Request.QueryString["Chave"]);

                            var turma = Convert.ToString(queryStringDecodificada["turma"]).Replace("##", "&");

                            if (relatorio.ToLower() == "filipeta")
                            {
                                List<ReportParameter> paramList = new List<ReportParameter>
							                                  	{
							                                  		new ReportParameter("matricula", queryStringDecodificada["matricula"], false),
							                                  		new ReportParameter("disciplina", queryStringDecodificada["disciplina"],
							                                  		                    false),
							                                  		new ReportParameter("turma", turma, false),
							                                  		new ReportParameter("ano", queryStringDecodificada["ano"], false),
                                                                    new ReportParameter("idvinculo", queryStringDecodificada["idvinculo"], false),
							                                  		new ReportParameter("periodo", queryStringDecodificada["periodo"], false),
							                                  		new ReportParameter("subperiodo", queryStringDecodificada["subperiodo"],
							                                  		                    false),
							                                  		new ReportParameter("nome", queryStringDecodificada["nome"], false),
							                                  		new ReportParameter("escola", queryStringDecodificada["escola"], false),
							                                  		new ReportParameter("semestre", queryStringDecodificada["semestre"], false),
							                                  		new ReportParameter("nomedisciplina",
							                                  		                    queryStringDecodificada["nomedisciplina"], false),
							                                  		new ReportParameter("protocolo", Convert.ToString(queryStringDecodificada["protocolo"]).Replace("##", "&") , false)
							                                  	};
                                rptViewer.ServerReport.SetParameters(paramList);

                                cabecalhoModelo.TituloCabecalho = Recurso.LancamentoNotasLista_TituloPagina;
                            }

                            else if (relatorio == "CurriculoMinimo")
                            {
                                List<ReportParameter> paramList = new List<ReportParameter>();
                                paramList.Add(new ReportParameter("matricula", queryStringDecodificada["matricula"], false));
                                paramList.Add(new ReportParameter("disciplina", queryStringDecodificada["disciplina"], false));
                                paramList.Add(new ReportParameter("turma", turma, false));
                                paramList.Add(new ReportParameter("ano", queryStringDecodificada["ano"], false));
                                paramList.Add(new ReportParameter("periodo", queryStringDecodificada["periodo"], false));
                                paramList.Add(new ReportParameter("subperiodo", queryStringDecodificada["subperiodo"], false));
                                paramList.Add(new ReportParameter("curso", queryStringDecodificada["curso"], false));
                                paramList.Add(new ReportParameter("modalidade", queryStringDecodificada["modalidade"], false));
                                paramList.Add(new ReportParameter("tipoCurso", queryStringDecodificada["tipoCurso"], false));
                                paramList.Add(new ReportParameter("serie", queryStringDecodificada["serie"], false));
                                paramList.Add(new ReportParameter("nome", queryStringDecodificada["nome"], false));
                                paramList.Add(new ReportParameter("escola", queryStringDecodificada["escola"], false));
                                paramList.Add(new ReportParameter("nomedisciplina", queryStringDecodificada["nomedisciplina"],
                                                                  false));
                                rptViewer.ServerReport.SetParameters(paramList);

                                cabecalhoModelo.TituloCabecalho = Recurso.RespostaCurriculoMinimoLista_TituloPagina;
                            }
                            else if (relatorio.ToLower() == "filipetaconsolidada")
                            {
                                List<ReportParameter> paramList = new List<ReportParameter>
							                                  	{
							                                  		
							                                  		new ReportParameter("disciplina", queryStringDecodificada["disciplina"],
							                                  		                    false),
							                                  		new ReportParameter("turma", turma, false),
							                                  		new ReportParameter("ano", queryStringDecodificada["ano"], false),
							                                  		new ReportParameter("periodo", queryStringDecodificada["periodo"], false),
                                                                     new ReportParameter("idvinculo", queryStringDecodificada["idvinculo"], false),
							                                  		new ReportParameter("escola", queryStringDecodificada["escola"], false),
                                                                    new ReportParameter("nomedisciplina",
							                                  		                    queryStringDecodificada["nomedisciplina"], false),
                                                                                        new ReportParameter("nome", queryStringDecodificada["nome"], false),
                                                                                         new ReportParameter("matricula", queryStringDecodificada["matricula"], false),
                                                                    new ReportParameter("subperiodo", queryStringDecodificada["subperiodo"],
							                                  		                    false)

							                                  	};
                                rptViewer.ServerReport.SetParameters(paramList);

                                cabecalhoModelo.TituloCabecalho = Recurso.LancamentoNotasLista_TituloPagina;
                            }
                        }
                        else
                        {
                            if (relatorio == "chdocenteonline")
                            {
                                var dtoDocenteLogado = (DTODocenteLogadoPrincipal)HttpContext.Current.User;
                                List<ReportParameter> paramList = new List<ReportParameter>();
                                paramList.Add(new ReportParameter("matricula", dtoDocenteLogado.Matricula, true));
                                paramList.Add(new ReportParameter("usuario", dtoDocenteLogado.Matricula, true));
                                paramList.Add(new ReportParameter("IDVINCULO", dtoDocenteLogado.IdFuncional + "/" + dtoDocenteLogado.Vinculo, true));
                                rptViewer.ServerReport.SetParameters(paramList);

                                cabecalhoModelo.TituloCabecalho = Recurso.ConsultaAlocacaoQHI_TituloPagina;
                            }

                            if (relatorio == "PlanoDeAula")
                            {
                                var dtoDocenteLogado = (DTODocenteLogadoPrincipal)HttpContext.Current.User;
                                List<ReportParameter> paramList = new List<ReportParameter>();
                                paramList.Add(new ReportParameter("ano", Request.QueryString["ano"], true));
                                paramList.Add(new ReportParameter("mes", Request.QueryString["mes"], true));
                                paramList.Add(new ReportParameter("disciplina", Request.QueryString["disciplina"], true));
                                paramList.Add(new ReportParameter("semestre", Request.QueryString["semestre"], true));
                                paramList.Add(new ReportParameter("turma", Request.QueryString["turma"], true));
                                rptViewer.ServerReport.SetParameters(paramList);

                                cabecalhoModelo.TituloCabecalho = Recurso.ConsultaAlocacaoQHI_TituloPagina;
                            }

                            if (relatorio == "DiarioClasse")
                            {
                                var dtoDocenteLogado = (DTODocenteLogadoPrincipal)HttpContext.Current.User;
                                List<ReportParameter> paramList = new List<ReportParameter>();
                                paramList.Add(new ReportParameter("ano", Request.QueryString["ano"], true));
                                paramList.Add(new ReportParameter("mes", Request.QueryString["mes"], true));
                                paramList.Add(new ReportParameter("disciplina", Request.QueryString["disciplina"], true));
                                paramList.Add(new ReportParameter("semestre", Request.QueryString["semestre"], true));
                                paramList.Add(new ReportParameter("turma", Request.QueryString["turma"], true));
                                rptViewer.ServerReport.SetParameters(paramList);

                                cabecalhoModelo.TituloCabecalho = Recurso.ConsultaAlocacaoQHI_TituloPagina;
                            }

                            if (relatorio == "DiarioClasseTrimestre")
                            {
                                var dtoDocenteLogado = (DTODocenteLogadoPrincipal)HttpContext.Current.User;
                                List<ReportParameter> paramList = new List<ReportParameter>();
                                paramList.Add(new ReportParameter("ano", Request.QueryString["ano"], true));

                                if (Request.QueryString["subperiodo"] != null)
                                    paramList.Add(new ReportParameter("Subperiodo", Request.QueryString["subperiodo"], true));
                                
                                paramList.Add(new ReportParameter("disciplina", Request.QueryString["disciplina"], true));
                                paramList.Add(new ReportParameter("semestre", Request.QueryString["semestre"], true));
                                paramList.Add(new ReportParameter("turma", Request.QueryString["turma"], true));
                                rptViewer.ServerReport.SetParameters(paramList);

                                cabecalhoModelo.TituloCabecalho = Recurso.ConsultaAlocacaoQHI_TituloPagina;
                            }
                        }
                    }
                }
                catch (System.Exception excecao)
                {
                    string msgError = excecao.Message;
                    bool itemNaoEncontrado = msgError.IndexOf("rsItemNotFound", 0) > 0;

                    if (itemNaoEncontrado)
                    {
                        lblMsg.Text = String.Concat(@"O relatório ", '/', pastaReport, '/', grupo, '/', relatorio, " não pode ser localizado no servidor de relatórios.");

                        rptViewer.Visible = false;
                        return;
                    }
                    throw;
                }
                rptViewer.Visible = true;
            }
        }

        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //	Page.Controls.Add(new Label {Text = "mf" });
        //}
    }
}