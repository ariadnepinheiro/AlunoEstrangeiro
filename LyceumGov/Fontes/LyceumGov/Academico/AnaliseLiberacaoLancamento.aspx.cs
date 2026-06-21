using System;
using System.Linq;
using System.Web;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Web;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Academico
{
    using Techne.Lyceum.RN.DTOs;
    using System.Collections.Generic;
    using System.Data;

    [
     NavUrl("~/Basico/AnaliseLiberacaoLancamento.aspx"),
      ControlText("AnaliseLiberacaoLancamento"),
      Title("Análise Liberação Lançamento de Notas"),
    ]
    public partial class AnaliseLiberacaoLancamento : TPage
    {
        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
        }
        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {

            TituloGrid(grdAnaliseLiberacaoNotas, "Análise Liberação Lançamento de Notas");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    pnGrid.Visible = false;
                    CarregarDadosDrop(ddlPeriodo.ID);

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdAnaliseLiberacaoNotas);
        }

        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        protected void grdAnaliseLiberacaoNotas_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAnaliseLiberacaoNotas);
        }

        public object Listar(object unidade_ens, object regional, object status, object ano, object periodo)
        {
            QueryTable qt = null;

            var status_sel = String.IsNullOrEmpty((string)status) ? "Aguardando" : ((string)status != "Todas" ? (string)status : string.Empty);

            if (regional != null && ano != null && periodo != null && unidade_ens != null)
            {            
               qt = RN.SolicitacaoAlteracaoNota.ListarPorCenso(unidade_ens.ToString(), status_sel, ano.ToString(), periodo.ToString());
            }
            else
                return null;


            return qt;
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                    return;
                lblMensagem.Text = string.Empty;
                pnGrid.Visible = false;
              
                if (!tseRegional.IsValidDBValue)
                {
                    lblMensagem.Text = "Regional não cadastrada.";
                    return;
                }
                
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidade_Ensino_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                    return;

                if (!tseUnidade_Ensino.DBValue.IsNull && tseUnidade_Ensino.IsValidDBValue)
                {
                    tseRegional.Value = tseUnidade_Ensino["id_regional"];
                    tseRegional_Changed(sender, args);
                    grdAnaliseLiberacaoNotas.Visible = true;
                    lblMensagem.Text = string.Empty;
                    pnGrid.Visible = true;
                    pnlBotoes.Visible = true;

                }
                
                if (!tseUnidade_Ensino.IsValidDBValue)
                {
                    grdAnaliseLiberacaoNotas.Visible = false;
                    lblMensagem.Text = "Unidade Administrativa não cadastrada.";
                    pnGrid.Visible = false;
                }
                
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdAnaliseLiberacaoNotas_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            if (Session["Mensagem"] == null)
                e.Properties["cpMessage"] = String.Empty;
            else
            {
                e.Properties["cpMessage"] = Session["Mensagem"].ToString();
                Session["Mensagem"] = null;
            }
        }

        protected void btnAprovar_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedItems = this.grdAnaliseLiberacaoNotas
                    .GetSelectedFieldValues("ID_SOLICITACAO_ALTERACAO_NOTA")
                    .Select(x => new StatusSolicitacaoAlteracaoNota
                    {
                        IdSolicitacaoAlteracaoNota = int.Parse(x.ToString()),
                        MatriculaAprovador = User.Identity.Name.ToString(),
                    })
                    .ToList();

                if (selectedItems.Count == 0)
                {
                    lblMensagem.Text = "Para Aprovar um pedido é necessário selecionar pelo menos 1(uma) solicitação.";
                    return;
                }
                else
                {
                    var retorno = SolicitacaoAlteracaoNota.Aprovar(selectedItems);

                    if (retorno != null)
                    {
                        if (!retorno.Ok)
                            throw new Exception(retorno.Errors.ToString());
                    }
                    else
                    {
                        lblMensagem.Text = "Pedido(s) aprovado(s) com sucesso.";
                        odsAnaliseLiberacaoLancamento.Select();
                        odsAnaliseLiberacaoLancamento.DataBind();
                        grdAnaliseLiberacaoNotas.DataBind();
                        this.grdAnaliseLiberacaoNotas.Selection.UnselectAll();
                    }
                }
            }

            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnReprovar_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedItems = this.grdAnaliseLiberacaoNotas
                   .GetSelectedFieldValues("ID_SOLICITACAO_ALTERACAO_NOTA")
                   .Select(x => new StatusSolicitacaoAlteracaoNota
                   {
                       IdSolicitacaoAlteracaoNota = int.Parse(x.ToString()),
                       MatriculaAprovador = User.Identity.Name.ToString(),

                   })
                   .ToList();

                if (selectedItems.Count == 0)
                {
                    lblMensagem.Text = "Para Aprovar um pedido é necessário selecionar pelo menos 1(uma) solicitação.";
                    return;
                }
                else
                {
                    var retorno = SolicitacaoAlteracaoNota.Reprovar(selectedItems);

                    if (retorno != null)
                    {
                        if (!retorno.Ok)
                            throw new Exception(retorno.Errors.ToString());
                    }
                    else
                    {
                        lblMensagem.Text = "Pedido(s) reprovado(s) com sucesso.";
                        odsAnaliseLiberacaoLancamento.Select();
                        odsAnaliseLiberacaoLancamento.DataBind();
                        grdAnaliseLiberacaoNotas.DataBind();
                        this.grdAnaliseLiberacaoNotas.Selection.UnselectAll();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tseRegional.ResetValue();
                tseUnidade_Ensino.ResetValue();
                pnGrid.Visible = false;
                
                if (!ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    CarregarDadosDrop(ddlPeriodo.ID);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }
        protected void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbStatus.SelectedValue != "Aguardando")
                {
                    pnlBotoes.Visible = false;
                }
                else
                {
                    pnlBotoes.Visible = true;
                }

                grdAnaliseLiberacaoNotas.DataBind();
                this.grdAnaliseLiberacaoNotas.Selection.UnselectAll();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private DataTable CarregarDadosDrop(string idDrop)
        {
            DataTable dadosDrop = null;

            try
            {
                switch (idDrop.ToUpper())
                {

                    case "DDLPERIODO":
                        {
                            string ano = string.Empty;

                            if (ddlAno.Items.Count > 0)
                                ano = ddlAno.SelectedValue;
                            else
                                ano = DateTime.Now.Year.ToString();

                            if (!string.IsNullOrEmpty(ano))
                            {
                                //dadosDrop = RN.PeriodoLetivo.ConsultarPeriodo(ano);

                                dadosDrop = RN.SubperiodoLetivo.ListarPeriodo(ano);

                                List<DropDownList> listaDrop = new List<DropDownList>();

                                CarregarDropDownList(ddlPeriodo, dadosDrop, listaDrop, null);

                            }

                            break;
                        }

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

            return dadosDrop;
        }

        private void CarregarDropDownList(DropDownList drop, DataTable data, List<DropDownList> listaDrop, string defaultValue)
        {
            drop.DataSource = data;
            drop.DataBind();

            if (drop.Items.Count == 0)
            {
                ListItem itemVazio = new ListItem("<Lista Vazia>", "");
                drop.Items.Add(itemVazio);
            }
            else
            {
                try
                {
                    if (!string.IsNullOrEmpty(defaultValue))
                        drop.SelectedValue = defaultValue;
                }
                catch (ArgumentOutOfRangeException)
                {
                    drop.ClearSelection();
                }
            }

            if (listaDrop != null)
            {
                foreach (DropDownList dropDependente in listaDrop)
                {
                    dropDependente.Items.Clear();
                    dropDependente.DataBind();
                }
            }
        }



    }
}