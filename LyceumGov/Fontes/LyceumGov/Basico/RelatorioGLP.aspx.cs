using System;
using System.Web;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using System.Linq;
using Microsoft.Reporting.WebForms;
using System.Net;
using System.Security.Principal;
using System.Configuration;
using Techne.Lyceum.RN.Relatorios;
using System.Collections.Generic;

namespace Techne.Lyceum.Net.Basico
{
    [
     NavUrl("~/Basico/RelatorioGLP.aspx"),
      ControlText("RelatorioGLP"),
      Title("Relatório Geral de Disponibilidade"),
    ]
    public partial class RelatorioGLP : TPage
    {
        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            ddlRegional.SelectedIndex = 0;
            ddlMunicipio.SelectedIndex = 0;
            ddlEscola.SelectedIndex = 0;
            ddlAno.SelectedIndex = 0;
            
            chkModalidadeMedio.Checked = false;
            chkModalidadeFundamental.Checked = false;
          
            ddlDisciplina.SelectedIndex = 0;

            chkTurnoManha.Checked = false;
            chkTurnoTarde.Checked = false;
            chkTurnoNoite.Checked = false;

            rblLotacao.ClearSelection();
            rbIngresso.ClearSelection();          

            chkSem65Horas.Checked = false;
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            EmEdicao = true;

            AbrirRelatorio(rvwDisponibilidade, "GLP_Disp_CH_DP");
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            EmEdicao = false;
        }

        public object ListaRegional()
        {
            
            var lista = new RN.UnidadeEnsino()
                .ListaRegionais(User.Identity.Name)
                .Select(s => new { s.ID_REGIONAL, s.REGIONAL })
                .OrderBy(o => o.REGIONAL)
                .ToList();

            lista.Insert(0, new { ID_REGIONAL = 0, REGIONAL = "-- TODAS --" });

            return lista;
        }

        public object ListaMunicipio(int? ID_REGIONAL)
        {
            if (ID_REGIONAL.HasValue && ID_REGIONAL.Value == 0)
                ID_REGIONAL = null;

            var lista = new RN.UnidadeEnsino()
                .ListaMunicipios(ID_REGIONAL, User.Identity.Name)
                .Select(s => new { s.CODIGO, s.NOME })
                .OrderBy(o => o.NOME)
                .ToList();

            lista.Insert(0, new { CODIGO = "", NOME = "-- TODOS --" });

            return lista;
        }

        public object ListaEscola(int? ID_REGIONAL, string MUNICIPIO)
        {
            if (ID_REGIONAL.HasValue && ID_REGIONAL.Value == 0)
                ID_REGIONAL = null;

            if (string.IsNullOrEmpty(MUNICIPIO))
                MUNICIPIO = null;

            var lista = new RN.UnidadeEnsino()
                .Lista(ID_REGIONAL, MUNICIPIO, User.Identity.Name)
                .Select(s => new { s.UnidadeEns, s.NomeComp })
                .OrderBy(o => o.NomeComp)
                .ToList();

            lista.Insert(0, new { UnidadeEns = "", NomeComp = "-- TODAS --" });

            return lista;
        }

        public object ListaAno()
        {
            return RN.PeriodoLetivo.ListarAnos();
        }

        public object ListaDisciplina()
        {
            RN.GrupoHabilitacao rnGrupoHabilitacao = new Techne.Lyceum.RN.GrupoHabilitacao();

            var lista = rnGrupoHabilitacao.ListaGrupoHabilitacaoAtivo();
            var row = lista.NewRow();
            lista.Rows.InsertAt(row, 0);
            return lista;
        }

        public bool EmEdicao
        {
            get
            {
                return plaReport.Visible;
            }

            set
            {
                plaFiltro.Visible = !value;
                plaReport.Visible = value;
            }
        }

        public void AbrirRelatorio(ReportViewer reportViewer, string relatorio)
        {
            var serverPath = ConfigurationManager.AppSettings["ServerPath"];
            var user = ConfigurationManager.AppSettings["CredentialUser"];
            var pwd = ConfigurationManager.AppSettings["CredentialPwd"];
            var domain = ConfigurationManager.AppSettings["CredentialDomain"];

            reportViewer.ProcessingMode = ProcessingMode.Remote;

            ServerReport serverReport = reportViewer.ServerReport;

            serverReport.ReportServerUrl = new Uri(serverPath);
            serverReport.ReportServerCredentials = new TechneReportCredentials(user, pwd, domain);
            serverReport.ReportPath = "/LYCEUM/QHI/" + relatorio;

            var paramList = new List<ReportParameter>();

            var usuario = RN.Usuarios.BuscaNome(User.Identity.Name);

            var modalidade = new Func<string>(() =>
            {
                var checkboxes = new CheckBox[] { chkModalidadeMedio, chkModalidadeFundamental };
                string value = null;

                foreach (var chk in checkboxes)
                    if (chk.Checked)
                        value += ((value ?? "").Length > 0 ? "," : "") + chk.Attributes["data-value"];

                return value;
            }).Invoke();
                       
            var turno = new Func<string>(() =>
            {
                var checkboxes = new CheckBox[] { chkTurnoManha, chkTurnoTarde, chkTurnoNoite };
                string value = null;

                foreach (var chk in checkboxes)
                    if (chk.Checked)
                        value += ((value ?? "").Length > 0 ? "," : "") + chk.Attributes["data-value"];

                return value;
            }).Invoke();            

            var lotadoMesmaEscola = new Func<string>(() =>
            {
                if (rblLotacao.SelectedValue == "MesmaEscola")
                {
                    return "1";
                }
                else
                {
                    return null;
                }
            }).Invoke();

            var lotadoMesmoMunicipio = new Func<string>(() =>
            {
                if (rblLotacao.SelectedValue == "MesmoMunicipio")
                {
                    return "1";
                }
                else
                {
                    return null;
                }
            }).Invoke();

            var lotadoMesmaRegional = new Func<string>(() =>
            {
                if (rblLotacao.SelectedValue == "MesmaRegional")
                {
                    return "1";
                }
                else
                {
                    return null;
                }
            }).Invoke();

            var lotadoOutraRegional = new Func<string>(() =>
            {
                if (rblLotacao.SelectedValue == "OutraRegional")
                {
                    return "1";
                }
                else
                {
                    return null;
                }
            }).Invoke();

            var agrupamentoIngresso = new Func<string>(() =>
            {
                if (rbIngresso.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    return null;
                }
                else
                {
                    return rbIngresso.SelectedValue;
                } 
            }).Invoke();

            var sem65Horas = chkSem65Horas.Checked ? "1" : "0";

            paramList.Add(new ReportParameter("usuario", usuario, false));
            paramList.Add(new ReportParameter("REGIONAL", (ddlRegional.SelectedValue != "0" ? ddlRegional.SelectedValue : null), false));
            paramList.Add(new ReportParameter("MUNICIPIO", (ddlMunicipio.SelectedValue != "" ? ddlMunicipio.SelectedValue : null), false));
            paramList.Add(new ReportParameter("UNIDADE_ENS", (ddlEscola.SelectedValue != "" ? ddlEscola.SelectedValue : null), false));
            paramList.Add(new ReportParameter("ANO", ddlAno.SelectedValue != "" ? ddlAno.SelectedValue : null, false));
            paramList.Add(new ReportParameter("MODALIDADE", modalidade, false));            
            paramList.Add(new ReportParameter("AGRUPAMENTO", (ddlDisciplina.SelectedValue != "" ? ddlDisciplina.SelectedValue : null), false));
            paramList.Add(new ReportParameter("TURNO", turno, false));
            paramList.Add(new ReportParameter("MESMA_UNIDADE_ENS", lotadoMesmaEscola, false));
            paramList.Add(new ReportParameter("MESMO_MUNICIPIO", lotadoMesmoMunicipio, false));
            paramList.Add(new ReportParameter("MESMA_REGIONAL", lotadoMesmaRegional, false));
            paramList.Add(new ReportParameter("OUTRA_REGIONAL", lotadoOutraRegional, false));
            paramList.Add(new ReportParameter("AGRUPAMENTO_INGRESSO", agrupamentoIngresso, false));
            paramList.Add(new ReportParameter("SEM_65HORAS", sem65Horas, false));

            serverReport.SetParameters(paramList);

            serverReport.Refresh();
        }
    }
}