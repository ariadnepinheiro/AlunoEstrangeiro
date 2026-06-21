using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.Net.Basico;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxGridView;
using Techne.Web;
using DevExpress.Web.ASPxClasses;

namespace Techne.Lyceum.Net.Cadastros
{
    [NavUrl("~/Cadastros/MaeAnalise.aspx")]
    [ControlText("MaeAnalise")]
    [Title("MAE Análise")]
    public partial class MaeAnalise : TPage
    {

        public object Listar(object unidade_ens)
        {
            RN.Cadastros.MaeInscricao rnMaeInscricao = new Techne.Lyceum.RN.Cadastros.MaeInscricao();

            var unidade = unidade_ens != null ? unidade_ens.ToString() : null;


            if (!unidade.IsNullOrEmptyOrWhiteSpace())
            {
                return rnMaeInscricao.ListaInscricaoParaAnalisePor(unidade);

            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.lblMensagem.Text = string.Empty;

                if (!this.IsPostBack)
                {
                    if (Request.QueryString.Keys.Count > 0)
                    {

                        if (Request.QueryString["ChaveConfirmacao"] != null)
                        {
                            byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["ChaveConfirmacao"]);
                            string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                            LimparCampos();

                            string censo = Convert.ToString(decodedText);

                            PreencherTela(censo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdAnalise, string.Empty);
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {

            ControlaAcesso(grdAnalise);
            ControlaAcessoGrid();
        }

        protected void ControlaAcessoGrid()
        {
            foreach (GridViewColumn col in grdAnalise.Columns)
            {
                if (col is GridViewCommandColumn)
                {
                    if (((GridViewCommandColumn)col).CustomButtons["btnConfirmar"] != null)
                        ((GridViewCommandColumn)col).CustomButtons["btnConfirmar"].Visibility =
                            (Permission.AllowUpdate || Permission.AllowInsert) ? GridViewCustomButtonVisibility.AllDataRows : GridViewCustomButtonVisibility.Invisible;
                }
            }

        }

        protected void grdAnalise_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAnalise);
            ControlaAcessoGrid();
        }

        protected void hplVisualizarDados_Click(object sender, EventArgs e)
        {
            try
            {
                var script = string.Empty;
                var cpf = (sender as LinkButton).CommandArgument.ToString();

                if (!cpf.IsNullOrEmptyOrWhiteSpace())
                {
                    byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(cpf);

                    Response.Redirect("MaeCadastro.aspx?ChaveConfirmacao=" + Convert.ToBase64String(bytesToEncode));

                   
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdAnalise_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {

            if (e.VisibleIndex == -1) return;

            if (e.CellType == GridViewTableCommandCellType.Filter)
            {
                e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                return;
            }

            var habilitado = (string)grdAnalise.GetRowValues(e.VisibleIndex, "HABILITADO");

            e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;

            if (Permission.AllowInsert || Permission.AllowUpdate)
            {
                e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.True;

                if (!string.IsNullOrEmpty(habilitado)
                   && habilitado == "Sim")
                {
                    e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                }
            }
        }

        protected void grdAnalise_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            int maeInscricaoId = Convert.ToInt32(grdAnalise.GetRowValues(e.VisibleIndex, "MAE_INSCRICAOID"));

            string queryString = string.Empty;

            if (e.ButtonID == "btnConfirmar")
            {
                string cpf = grdAnalise.GetRowValues(e.VisibleIndex, "CPF") != null ? grdAnalise.GetRowValues(e.VisibleIndex, "CPF").ToString() : null;
                string nome = grdAnalise.GetRowValues(e.VisibleIndex, "NOME").ToString();
                queryString = MontarQueryStringConfirmacao(maeInscricaoId, cpf, nome, tseUnidadeResponsavel.DBValue.ToString(), tseUnidadeResponsavel["nome_comp"].ToString());

                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                Response.Redirect("ConfirmaMae.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
            }

        }

        private string MontarQueryStringConfirmacao(int maeInscricaoId, string Cpf,string Nome, string Censo, string Unidade)
        {
            string queryString = string.Empty;
            queryString += "MaeInscricaoId=" + maeInscricaoId;
            queryString += "&CPF=" + Cpf;            
            queryString += "&Nome=" + Nome;
            queryString += "&Censo=" + Censo;
            queryString += "&Unidade=" + Unidade;


            return queryString;
        }   

        private void PreencherTela(string censo)
        {

            tseUnidadeResponsavel.DBValue = censo;
            tseUnidadeResponsavel_Changed(null, null);            

        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                var sessao = SessaoUsuario.GetSessaoUsuario();
                tseUnidadeResponsavel.ResetValue();
                this.pnGrid.Visible = false;
                this.pnGrid.Visible = false;

                if (sessao != null)
                {
                    if (!this.tseMunicipio.DBValue.IsNull)
                    {
                        if (this.tseMunicipio.IsValidDBValue)
                        {
                            sessao.Municipio = Convert.ToString(this.tseMunicipio.DBValue);

                            sessao.Escola = string.Empty;
                            this.tseUnidadeResponsavel.ResetValue();
                        }
                        else
                        {
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                        }
                    }
                    else
                    {
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeResponsavel_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                var sessao = SessaoUsuario.GetSessaoUsuario();
                this.pnGrid.Visible = false;


                if (!this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (this.tseUnidadeResponsavel.IsValidDBValue)
                    {
                        if (!this.tseUnidadeResponsavel["unidade_ens"].IsNull)
                        {
                            sessao.Escola = Convert.ToString(this.tseUnidadeResponsavel.DBValue);
                            this.tseMunicipio.Value = this.tseUnidadeResponsavel["municipio"];

                            this.pnGrid.Visible = true;
                            odsAnalise.Select();
                            grdAnalise.DataBind();
                        }
                    }
                    else
                    {
                        if (sessao != null)
                        {
                            sessao.Escola = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Coordenadoria = string.Empty;
                        }


                        this.lblMensagem.Text = "Unidade de Ensino não cadastrada.";

                    }
                }
                else
                {
                    if (sessao != null)
                    {
                        sessao.Escola = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Coordenadoria = string.Empty;
                    }

                    this.lblMensagem.Text = "Favor consultar uma unidade de ensino.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimparCampos()
        {
            this.lblMensagem.Text = string.Empty;
            this.tseMunicipio.ResetValue();
            this.tseUnidadeResponsavel.ResetValue();

            this.pnGrid.Visible = false;

        }

    }
}
