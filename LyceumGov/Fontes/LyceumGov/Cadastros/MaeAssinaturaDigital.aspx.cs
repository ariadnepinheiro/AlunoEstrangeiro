using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using DevExpress.Web.ASPxGridView;
using Techne.Lyceum.RN;
using System.Data;
using System.Configuration;

namespace Techne.Lyceum.Net.Cadastros
{
    [NavUrl("~/Cadastros/MaeAssinaturaDigital.aspx")]
    [ControlText("MaeAssinaturaDigital")]
    [Title("MAE Assinatura Digital")]
    public partial class MaeAssinaturaDigital : TPage
    {
        private readonly RN.Cadastros.MaeFormularioBancoArquivo rnMaeFormularioBancoArquivo = new RN.Cadastros.MaeFormularioBancoArquivo();
        private readonly RN.Cadastros.MaeInscricao rnMaeInscricao = new RN.Cadastros.MaeInscricao();
        
        public object Listar(object unidade_ens)
        {
            var unidade = unidade_ens != null ? unidade_ens.ToString() : null;
            if (!unidade.IsNullOrEmptyOrWhiteSpace())
                return rnMaeInscricao.ListaInscricaoPor(unidade);
            return null;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdAssinaturaDigital, string.Empty);
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdAssinaturaDigital);
            ControlaAcessoGrid();

            txtQRCode.Text = ConfigurationManager.AppSettings["MaeQRCode"];
            txtURLBusca.Text = ConfigurationManager.AppSettings["MaeURLBusca"];
        }

        protected void ControlaAcessoGrid()
        {
            foreach (GridViewColumn col in grdAssinaturaDigital.Columns)
            {
                if (col is GridViewCommandColumn)
                {
                    if (((GridViewCommandColumn)col).CustomButtons["btnConfirmar"] != null)
                        ((GridViewCommandColumn)col).CustomButtons["btnConfirmar"].Visibility =
                            (Permission.AllowUpdate || Permission.AllowInsert) ? GridViewCustomButtonVisibility.AllDataRows : GridViewCustomButtonVisibility.Invisible;
                }
            }

        }

        protected void grdAssinaturaDigital_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAssinaturaDigital);
            ControlaAcessoGrid();
        }

        protected void grdAssinaturaDigital_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {

            if (e.VisibleIndex == -1) return;

            if (e.CellType == GridViewTableCommandCellType.Filter)
            {
                e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                return;
            }

            var habilitado = (string)grdAssinaturaDigital.GetRowValues(e.VisibleIndex, "HABILITADO");

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

        protected void grdAssinaturaDigital_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            //int maeInscricaoId = Convert.ToInt32(grdAssinaturaDigital.GetRowValues(e.VisibleIndex, "MAE_INSCRICAOID"));

            //string queryString = string.Empty;

            //if (e.ButtonID == "btnConfirmar")
            //{
            //    string cpf = grdAssinaturaDigital.GetRowValues(e.VisibleIndex, "CPF") != null ? grdAssinaturaDigital.GetRowValues(e.VisibleIndex, "CPF").ToString() : null;
            //    string nome = grdAssinaturaDigital.GetRowValues(e.VisibleIndex, "NOME").ToString();
            //    queryString = MontarQueryStringConfirmacao(maeInscricaoId, cpf, nome, tseUnidadeResponsavel.DBValue.ToString(), tseUnidadeResponsavel["nome_comp"].ToString());

            //    byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

            //    Response.Redirect("ConfirmaMae.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
            //}

        }

        //private string MontarQueryStringConfirmacao(int maeInscricaoId, string Cpf, string Nome, string Censo, string Unidade)
        //{
        //    string queryString = string.Empty;
        //    queryString += "MaeInscricaoId=" + maeInscricaoId;
        //    queryString += "&CPF=" + Cpf;
        //    queryString += "&Nome=" + Nome;
        //    queryString += "&Censo=" + Censo;
        //    queryString += "&Unidade=" + Unidade;


        //    return queryString;
        //}

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
                //this.pnGrid.Visible = false;
                //this.pnGrid.Visible = false;

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
                //this.pnGrid.Visible = false;


                if (!this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (this.tseUnidadeResponsavel.IsValidDBValue)
                    {
                        if (!this.tseUnidadeResponsavel["unidade_ens"].IsNull)
                        {
                            sessao.Escola = Convert.ToString(this.tseUnidadeResponsavel.DBValue);
                            this.tseMunicipio.Value = this.tseUnidadeResponsavel["municipio"];

                            //this.pnGrid.Visible = true;
                            odsAssinaturaDigital.Select();
                            grdAssinaturaDigital.DataBind();
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

            //this.pnGrid.Visible = false;
        }

        protected void btnVisualizar_Command(object sender, CommandEventArgs e)
        {
            try
            {
                var tabela = "MaeFormularioBancoArquivo";
                string embed = string.Empty;
                bimgArquivo.Visible = false;
                ltEmbed.Visible = false;
                DataTable dados = new DataTable();

                string[] chave = e.CommandArgument.ToString().Split(new char[] { ',' });

                if (!chave[1].ToString().IsNullOrEmptyOrWhiteSpace())
                {
                    pucVisualizarArquivo.ShowOnPageLoad = true;

                    if (chave[1].ToString() == "application/pdf")
                    {
                        embed = " <object data=\"{0}{1}\"";
                        embed += "type=\"application/pdf\" width=\"100%\" height=\"100%\">";
                        embed += "<iframe   src=\"{0}{1}\"  width=\"100%\"   height=\"100%\"";
                        embed += "style=\"border: none;\">    <p>Your browser does not support PDFs.";
                        embed += "<a href=\"{0}{1}\">Download the PDF</a>.</p>";
                        embed += "</iframe></object>";
                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/Cadastros/FileCS.ashx?Tabela=" + tabela + "&Id="), chave[0].ToString());
                        ltEmbed.Visible = true;
                        pucVisualizarArquivo.Width = Unit.Pixel(880);
                        pucVisualizarArquivo.Height = Unit.Pixel(580);
                    }
                    else
                    {
                        pucVisualizarArquivo.Width = Unit.Pixel(350);
                        pucVisualizarArquivo.Height = Unit.Pixel(350);
                        bimgArquivo.ContentBytes = rnMaeFormularioBancoArquivo.ObtemArquivoPor(Convert.ToInt32(chave[0]));
                        bimgArquivo.Visible = true;
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Não existe documento para visualização');", true);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvarURLBusca_Click(object sender, EventArgs e)
        {
            ConfigurationManager.AppSettings["MaeQRCode"] = txtQRCode.Text;
            ConfigurationManager.AppSettings["MaeURLBusca"] = txtURLBusca.Text;
        }
    }
}
