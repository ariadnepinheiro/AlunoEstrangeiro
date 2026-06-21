using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using System.Data;
using Techne.Lyceum.RN.Util;
using Techne.Controls;
using Seeduc.Infra.Data;
using Techne.Web;
using Seeduc.Infra.Helpers;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [
        NavUrl("~/PrestacaoContas/AprovarContaCorrente.aspx"),
        ControlText("AprovarContaCorrente"),
        Title("Aprovar Conta Corrente")
    ]
    public partial class AprovarContaCorrente : TPage
    {
   

        public enum TipoOperacao
        {
            Novo,
            Alterar,
            Consultar,
            Inicial,
            Sucesso,
            Excluir,
            Desativar
        }

        private TipoOperacao _tipoOperacao
        {
            get
            {
                if (ViewState["_tipoOperacao"] != null)
                {
                    if (ViewState["_tipoOperacao"] is TipoOperacao)
                    {
                        return (TipoOperacao)ViewState["_tipoOperacao"];
                    }
                }

                return TipoOperacao.Inicial;
            }

            set
            {
                ViewState["_tipoOperacao"] = value;
            }
        }

        public void Delete(object CONTACORRENTEID) { }

       

        public object ListaDados(object filtro, object tipo)
        {
            RN.PrestacaoContas.AnaliseContaCorrente rnAnaliseContaCorrente = new Techne.Lyceum.RN.PrestacaoContas.AnaliseContaCorrente();

            if (filtro != null && tipo != null)
            {
                if (!string.IsNullOrEmpty(filtro.ToString()) && !string.IsNullOrEmpty(tipo.ToString()))
                {
                    return rnAnaliseContaCorrente.ListaAnaliseContaCorrentePor(filtro.ToString(), tipo.ToString(), User.Identity.Name);
                }
            }
            
            return null;
        }

        
        public object ListaMotivo()
        {
            RN.PrestacaoContas.MotivoReprovacaoContaCorrente rnMotivoReprovacaoContaCorrente = new Techne.Lyceum.RN.PrestacaoContas.MotivoReprovacaoContaCorrente();

            return rnMotivoReprovacaoContaCorrente.ListaAtivoPor();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    _tipoOperacao = TipoOperacao.Inicial;
                    ControlarTipoOperacao();
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdAprovarContaCorrente, "");
            grdAprovarContaCorrente.SettingsText.CommandEdit = "Confirma a desativação/reativação da conta?";
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcessoGrid();
        }


        protected void ControlaAcessoGrid()
        {
            if (grdAprovarContaCorrente != null)
            {
                if (!Permission.AllowDelete && !Permission.AllowInsert && !Permission.AllowUpdate)
                {
                    grdAprovarContaCorrente.Columns[""].Visible = false;
                }
            }
            ControlaAcesso(grdAprovarContaCorrente, AcaoControle.editar, "btnReprovar");
            ControlaAcesso(grdAprovarContaCorrente, AcaoControle.editar, "btnAceitar");            
            ControlaAcesso(grdAprovarContaCorrente);
        }


      
        protected void rblTipoFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {           

              
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Consultar;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (!rblTipoFiltro.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (rblTipoFiltro.SelectedValue == "R")
                    {
                    
                    }
                    else
                    {
                    }
                }
                else
                {
                    lblMensagem.Text = "Para uma Nova Conta Corrente é necessário a escolha de um filtro.";
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ControlarTipoOperacao()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                      
                        LimpaDados();
                       
                        ControlarTSearchs();
                        break;
                    }
                case TipoOperacao.Novo:
                    {
                       
                        LimpaDados();
                    
                        ControlarTSearchs();
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { };
                     
                        ControlarTSearchs();
                        break;
                    }

                case TipoOperacao.Alterar:
                    {
                       
                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                       
                        LimpaDados();
                        ControlarTSearchs();
                        break;
                    }

            }
        }

        protected void tseBanco_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseAgencia_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseAgencia_Changed(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

               
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseBanco_Changed(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }              
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeResponsavel_Changed(object sender, ChangedEventArgs args)
        {
            if (Page.IsCallback)
            {
                return;
            }
            try
            {
           

                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseRegional_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }
                lblMensagem.Text = string.Empty;

             

                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimpaDados()
        {
            hdnContaCorrente.Value = string.Empty;

        }

        private void ControlarTSearchs()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {

                        break;
                    }
                case TipoOperacao.Sucesso:
                    {

                        break;
                    }

                case TipoOperacao.Novo:
                    {

                        break;
                    }

                case TipoOperacao.Alterar:
                    {


                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                

                        break;
                    }
            }
        }
        protected void tseRegional_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseUnidadeResponsavel_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdAprovarContaCorrente.PageIndex * grdAprovarContaCorrente.SettingsPager.PageSize;
            for (int i = 0; i < grdAprovarContaCorrente.VisibleRowCount; i++)
            {
                if (grdAprovarContaCorrente.Selection.IsRowSelected(startIndexOnPage + i))
                {
                    return startIndexOnPage + i;
                }
            }
            return -1;
        }
        protected void grdAprovarContaCorrente_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdAprovarContaCorrente.Settings.ShowFilterRow = false;
        }

        protected void grdAprovarContaCorrente_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdAprovarContaCorrente.Settings.ShowFilterRow = false;
        }

        protected void grdAprovarContaCorrente_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAprovarContaCorrente);
        }

        public void Update(object DESCRICAO, object ATIVO, object FINALIDADEID) { }



        protected void btConfirma_Click(object sender, EventArgs e)
        {
            try
            {
                RN.PrestacaoContas.Entidades.AnaliseContaCorrente analiseContaCorrente = new Techne.Lyceum.RN.PrestacaoContas.Entidades.AnaliseContaCorrente();
                RN.PrestacaoContas.AnaliseContaCorrente rnAnaliseContaCorrente = new RN.PrestacaoContas.AnaliseContaCorrente();

                analiseContaCorrente.ContaCorrenteId = Convert.ToInt32(hID.Text);
                analiseContaCorrente.MotivoReprovacaoContaCorrenteId = Convert.ToInt32(cmbMotivo.SelectedValue);
                analiseContaCorrente.UsuarioAprovacaoId = User.Identity.Name;

                ValidacaoDados validacao = new ValidacaoDados();
                validacao = rnAnaliseContaCorrente.Valida(analiseContaCorrente, false);

                if (validacao.Valido)
                {
                    analiseContaCorrente.Aprovado = true;
                    if (rblSituacao.SelectedValue == "Pendente")
                    {
                        analiseContaCorrente.Aprovado = false;
                        rnAnaliseContaCorrente.Insere(analiseContaCorrente);
                        string mensagem = "Conta Corrente Reprovada com sucesso.";

                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('" + mensagem + ".');", true);
                    }
                    else 
                    {
                        rnAnaliseContaCorrente.Atualiza(analiseContaCorrente);
                    }
                    pucConfirmar.ShowOnPageLoad = false;
                    odsContaCorrente.Select();
                    odsContaCorrente.DataBind();
                    grdAprovarContaCorrente.DataBind();
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem;
                    odsContaCorrente.Select();
                    odsContaCorrente.DataBind();
                    grdAprovarContaCorrente.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void grdAprovarContaCorrente_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            RN.PrestacaoContas.Entidades.AnaliseContaCorrente analiseContaCorrente = new Techne.Lyceum.RN.PrestacaoContas.Entidades.AnaliseContaCorrente();
            RN.PrestacaoContas.AnaliseContaCorrente rnAnaliseContaCorrente = new RN.PrestacaoContas.AnaliseContaCorrente();


            analiseContaCorrente.ContaCorrenteId = Convert.ToInt32(grdAprovarContaCorrente.GetRowValues(e.VisibleIndex, "CONTACORRENTEID").ToString());
            analiseContaCorrente.MotivoReprovacaoContaCorrenteId = Convert.ToInt32(cmbMotivo.SelectedValue);
            analiseContaCorrente.UsuarioAprovacaoId = User.Identity.Name;

            string contaCorrenteId = grdAprovarContaCorrente.GetRowValues(e.VisibleIndex, "CONTACORRENTEID").ToString();

            if (e.ButtonID == "btnReprovar")
            {
                pucConfirmar.ShowOnPageLoad = true;
                hID.Text = contaCorrenteId;
                analiseContaCorrente.Aprovado = false;

            }
            else if (e.ButtonID == "btnAceitar")
            {
                try
                {
                    ValidacaoDados validacao = new ValidacaoDados();
                    //Chamar metodo valida       
                    validacao = rnAnaliseContaCorrente.Valida(analiseContaCorrente, false);

                    if (validacao.Valido)
                    {
                        analiseContaCorrente.Aprovado = true;
                        if (rblSituacao.SelectedValue == "Pendente")
                        {
                            analiseContaCorrente.MotivoReprovacaoContaCorrenteId = null;
                            rnAnaliseContaCorrente.Insere(analiseContaCorrente);
                            string mensagem = "Conta Corrente Aprovada com sucesso.";

                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('" + mensagem + ".');", true);
                        }
                        else 
                        {
                            rnAnaliseContaCorrente.Atualiza(analiseContaCorrente);
                            string mensagem = "Conta Corrente atualizado com sucesso.";

                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('" + mensagem + ".');", true);
                        }

                        odsContaCorrente.Select();
                        odsContaCorrente.DataBind();
                        grdAprovarContaCorrente.DataBind();
                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem;
                        odsContaCorrente.Select();
                        odsContaCorrente.DataBind();
                        grdAprovarContaCorrente.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    lblMensagem.Text = ex.Message;
                }
            }
        }

        protected void grdAprovarContaCorrente_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;

            if (e.CellType == GridViewTableCommandCellType.Filter)
            {
                e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                return;
            }
            if (e.ButtonID == "btnAceitar")
            {
                if (rblSituacao.SelectedValue != "Pendente")
                {
                    e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                    return;
                }
            }

            if (e.ButtonID == "btnReprovar")
            {
                if (rblSituacao.SelectedValue != "Pendente")
                {
                    e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                    return;
                }
            }
        }
    }
}
