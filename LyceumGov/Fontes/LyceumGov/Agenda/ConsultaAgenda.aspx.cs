using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Web;

namespace Techne.Lyceum.Net.Agenda
{
    [NavUrl("~/Agenda/ConsultaAgenda.aspx"), ControlText("ConsultaAgenda"), Title("Agenda de eventos")]
    public partial class ConsultaAgenda : TPage
    {
        #region PROPRIEDADES DE PÁGINA

        string periodo;
        public string Periodo
        {
            get { return periodo; }
            set { periodo = value; }
        }

        RN.Agenda.Agenda agenda = new Techne.Lyceum.RN.Agenda.Agenda();
        public RN.Agenda.Agenda Agenda
        {
            get { return agenda; }
            set { agenda = value; }
        }

        List<string> mensagens = new List<string>();

        public List<string> Mensagens
        {
            get { return mensagens; }
            set { mensagens = value; }
        }

        #endregion

        #region EVENTOS DE PÁGINA

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarControle(ddlAno.ID);
                if (Request.QueryString["retorno"] == "R")
                {
                    RecuperaDadosPagina(Session["ssDDLANO"].ToString(), Session["ssCHKVALUE"].ToString());
                }
            }
            if (cblperiodo.SelectedValue != "" && ddlAno.SelectedValue != "")
            {
                lblMensagem.Text = "";
                List<string> msg = new List<string>();
                msg = PreencheTelaDados();
                if (msg.Count != 0)
                {
                    ApresentaMensagemErro(msg);
                }
            }
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarControle(cblperiodo.ID);
        }

        protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
        {
            lblMensagem.Text = "";
            List<string> msg = new List<string>();
            msg = PreencheTelaDados();
            if (msg.Count != 0)
            {
                ApresentaMensagemErro(msg);
            }
        }

        protected void SelecionaRow(object sender, EventArgs e)
        {
            Session["SSAGENDAID"] = (sender as ImageButton).CommandArgument.ToString();
            Session["SSANO"] = ddlAno.SelectedValue;
            Response.Redirect("~/Agenda/AgendaDadosGerais.aspx");
        }

        #endregion

        #region MÉTODOS
        /// <summary>
        /// Carrega dados em sessão, dos valores da pesquisa para persistêcnia da busca 
        /// </summary>
        /// <param name="ddlValue"></param>
        /// <param name="ChkBoxValue"></param>
        private void CarregaSessaoPagina(string ddlValue, string ChkBoxValue)
        {
            Session.Add("ssDDLANO", ddlValue);
            Session.Add("ssCHKVALUE", ChkBoxValue);
        }
        /// <summary>
        /// Recupera a tela no retorno do botão voltar da página AgendaDadosGerais
        /// </summary>
        /// <param name="ddlValue"></param>
        /// <param name="ChkBoxValue"></param>
        private void RecuperaDadosPagina(string ddlValue, string ChkBoxValue)
        {
            string[] valor = ChkBoxValue.Split(',');
            ddlAno.SelectedValue = ddlValue;
            CarregarControle(cblperiodo.ID);
            for (int i = 0; i < valor.Count(); i++)
            {
                if (Convert.ToInt32(valor[i]) == i)
                {
                    cblperiodo.Items[Convert.ToInt32(valor[i])].Selected = true;
                }
            }
            List<string> msg = new List<string>();
            msg = PreencheTelaDados();
            if (msg.Count != 0)
            {
                ApresentaMensagemErro(msg);
            }
        }
        /// <summary>
        /// Preenche a tela com dados
        /// </summary>
        private List<string> PreencheTelaDados()
        {
            Mensagens = ValidaCampos(Mensagens);
            if (Mensagens.Count == 0)
            {
                Periodo = RecuperaPeriodos(cblperiodo);
                DataTable DadosAgenda = Agenda.ConsultaAgendaPorAnoEPeriodo(Convert.ToInt32(ddlAno.SelectedValue), Periodo);
                Mensagens = ValidarRetorno(DadosAgenda, Mensagens);
                CarregaSessaoPagina(ddlAno.SelectedValue, Periodo);
                if (Mensagens.Count == 0)
                {
                    PopulaGridView(DadosAgenda);
                    this.grdConsultaAgenda.SettingsText.Title = "Agenda de Eventos Ano " + ddlAno.SelectedValue + " Período " + Periodo.Replace(",", " - ") + "";
                    grdConsultaAgenda.Visible = true;
                    if (DadosAgenda.Rows.Count == 0)
                    {
                        Mensagens.Add("Não há dados para esta pesquisa");
                    }
                }
            }
            return Mensagens;
        }
        /// <summary>
        /// Carrega controles de página a partir do ID
        /// </summary>
        /// <param name="idDrop"></param>
        private void CarregarControle(string idDrop)
        {
            switch (idDrop.ToUpper())
            {
                case "DDLANO":
                    {

                        ddlAno.DataSource = RN.PeriodoLetivo.ConsultarAno();
                        ddlAno.DataBind();
                        ddlAno.DataTextField = "ANO";
                        ddlAno.DataValueField = "ANO";
                        ListItem item = new ListItem("<selecione>", "");
                        ddlAno.Items.Insert(0, item);
                        break;
                    }

                case "CBLPERIODO":
                    {
                        cblperiodo.DataSource = RN.PeriodoLetivo.ConsultarPeriodo(ddlAno.SelectedValue);
                        cblperiodo.DataBind();
                        cblperiodo.DataTextField = "PERIODO";
                        cblperiodo.DataValueField = "ID_REDUZIDA";
                        break;
                    }
                default:
                    break;
            }
        }
        /// <summary>
        /// Validação de Retorno do banco
        /// </summary>
        /// <param name="DadosAgenda"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public List<string> ValidarRetorno(DataTable DadosAgenda, List<string> ErrMsg)
        {
            Periodo = RecuperaPeriodos(cblperiodo);
            if (DadosAgenda.Rows.Count == 0)
            {
                ErrMsg.Add("Não existe agenda para o Ano/Período informado.");
                grdConsultaAgenda.Visible = false;
            }
            return ErrMsg;
        }
        /// <summary>
        /// Validação de Campos de tela
        /// </summary>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        private List<string> ValidaCampos(List<string> ErrMsg)
        {
            if (ddlAno.SelectedItem.Value == "")
            {
                ErrMsg.Add("Ano não informado.");
                grdConsultaAgenda.Visible = false;
            }
            if (cblperiodo.SelectedIndex < 0)
            {
                ErrMsg.Add("Período não informado.");
                grdConsultaAgenda.Visible = false;
            }

            return ErrMsg;
        }
        /// <summary>
        /// Popula a GridView
        /// </summary>
        /// <param name="DadosAgenda"></param>
        private void PopulaGridView(DataTable DadosAgenda)
        {
            if (cblperiodo.SelectedValue != "" && ddlAno.SelectedValue != "")
            {
                grdConsultaAgenda.DataSource = DadosAgenda;
                grdConsultaAgenda.DataBind();
            }
            else
            {
                grdConsultaAgenda.Visible = false;
            }
        }
        /// <summary>
        /// Apresenta as mensagens de erro na tela
        /// </summary>
        /// <param name="erros"></param>
        private void ApresentaMensagemErro(List<string> erros)
        {
            grdConsultaAgenda.Visible = false;
            string htmlErro = string.Empty;
            foreach (var item in erros)
            {
                htmlErro += item.ToString() + "<br/>";
            }
            lblMensagem.Text = htmlErro;
        }
        /// <summary>
        /// Recupera periodos concatenados provenientes da cblPeriodos
        /// </summary>
        /// <returns>string</returns>
        private string RecuperaPeriodos(CheckBoxList chkLst)
        {
            string per = string.Empty;

            for (int i = 0; i < chkLst.Items.Count; i++)
            {
                if (chkLst.Items[i].Selected == true)
                {
                    per += chkLst.Items[i].Value + ",";
                }
            }
            if (per != string.Empty)
            {
                per = per.Remove(per.Length - 1);
            }
            return per;
        }

        #endregion

    }
}
