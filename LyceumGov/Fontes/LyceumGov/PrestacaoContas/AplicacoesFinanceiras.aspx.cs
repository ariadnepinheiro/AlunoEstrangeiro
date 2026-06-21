using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxGridView;
using Seeduc.Infra.Data;
using Seeduc.Infra.Helpers;
using Techne.Lyceum.RN.PrestacaoContas.DTOs;
using Techne.Web;
using Techne.Lyceum.RN;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [NavUrl("~/PrestacaoContas/AplicacoesFinanceiras.aspx"), ControlText("AplicacoesFinanceiras"), Title("Poupança")]
    public partial class AplicacoesFinanceiras : TPage
    {
    
        public object ListaDados(object censo, object extratoBancario)
        {
            DataTable dados = new DataTable();

            RN.PrestacaoContas.AplicacaoFinanceira rnAplicacaoFinanceira = new RN.PrestacaoContas.AplicacaoFinanceira();

            if (!String.IsNullOrEmpty(censo.ToString()) && String.IsNullOrEmpty(extratoBancario.ToString()))
            {
                dados = rnAplicacaoFinanceira.ListaDados(censo);

            }
            else if (!String.IsNullOrEmpty(extratoBancario.ToString()))
            {
                dados = rnAplicacaoFinanceira.ListaDadosPor(censo.ToString(), Convert.ToInt32(extratoBancario));
            }


            return dados;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    LimpaControles();
                    pnlDados.Visible = false;
                    btnCancel.Visible = false;

                    if (Request.QueryString.Keys.Count > 0)
                    {
                        byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                        string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

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
            TituloGrid(grdAplicacoesFinanceiras, "");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdAplicacoesFinanceiras);
            ControlaAcesso(btnSalvar, AcaoControle.editar);
            ControlaAcesso(btnNovo, AcaoControle.novo);
        }

        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                LimpaControles();
                pnlDados.Visible = true;
                btnNovo.Visible = false;
                btnCancel.Visible = true;
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
                LimpaControles();
                pnlDados.Visible = false;
                btnNovo.Visible = true;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                var dadosInformacaoAdcionalAEE = new RN.PrestacaoContas.DTOs.DadosUnidadeAae();
                ValidacaoDados validacao = new ValidacaoDados();
                string mensagem = string.Empty;

                RN.PrestacaoContas.Entidades.AplicacaoFinanceira aplicacaoFinanceira = new Techne.Lyceum.RN.PrestacaoContas.Entidades.AplicacaoFinanceira();
                RN.PrestacaoContas.AplicacaoFinanceira rnAplicacaoFinanceira = new Techne.Lyceum.RN.PrestacaoContas.AplicacaoFinanceira();

                RN.PrestacaoContas.Entidades.AplicacaoFinanceiraComprovanteArquivo aplicacaoFinanceiraComprovanteArquivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.AplicacaoFinanceiraComprovanteArquivo();
               
                aplicacaoFinanceira.AplicacaoFinanceiraId = !String.IsNullOrEmpty(hdnAplicacaoFinanceiraId.Value) ? Convert.ToInt32(hdnAplicacaoFinanceiraId.Value) : 0;
                aplicacaoFinanceira.Censo = !String.IsNullOrEmpty(Convert.ToString(tseUnidadeResponsavelDados.DBValue)) ? Convert.ToString(tseUnidadeResponsavelDados.DBValue) : null;
                aplicacaoFinanceira.ExtratoBancarioId = !String.IsNullOrEmpty(Convert.ToString(tseExtratoBancarioDados.DBValue)) ? Convert.ToInt32(tseExtratoBancarioDados.DBValue) : 0;
                aplicacaoFinanceira.Valor = !String.IsNullOrEmpty(txtValorAplicacao.Text) ? Convert.ToDecimal(txtValorAplicacao.Text) : -1;
                aplicacaoFinanceira.Justificativa = !String.IsNullOrEmpty(txtJustificativa.Text) ? txtJustificativa.Text : null;
                aplicacaoFinanceira.UsuarioId = User.Identity.Name;

                byte[] imageBytes = new byte[FileUpload2.PostedFile.InputStream.Length + 1];
                FileUpload2.PostedFile.InputStream.Read(imageBytes, 0, imageBytes.Length);
                aplicacaoFinanceiraComprovanteArquivo.Arquivo = imageBytes;

                aplicacaoFinanceiraComprovanteArquivo.NomeArquivo = FileUpload2.PostedFile.FileName;
                aplicacaoFinanceiraComprovanteArquivo.TipoArquivo = FileUpload2.PostedFile.ContentType;
                aplicacaoFinanceiraComprovanteArquivo.ChaveArquivo = Guid.NewGuid().ToString();
                aplicacaoFinanceiraComprovanteArquivo.AplicacaoFinanceiraId = !String.IsNullOrEmpty(hdnAplicacaoFinanceiraId.Value) ? Convert.ToInt32(hdnAplicacaoFinanceiraId.Value) : 0;
                aplicacaoFinanceiraComprovanteArquivo.AplicacaoFinanceiraComprovanteArquivoId = !String.IsNullOrEmpty(hdnAplicacaoFinanceiraComprovanteArquivoId.Value) ? Convert.ToInt32(hdnAplicacaoFinanceiraComprovanteArquivoId.Value) : 0;
                aplicacaoFinanceiraComprovanteArquivo.UsuarioId = User.Identity.Name;
                
                validacao = rnAplicacaoFinanceira.Valida(aplicacaoFinanceira,aplicacaoFinanceiraComprovanteArquivo, aplicacaoFinanceira.AplicacaoFinanceiraId == 0 ? true : false);

                if (validacao.Valido)
                {
                    if (aplicacaoFinanceira.AplicacaoFinanceiraId == 0)
                    {
                        rnAplicacaoFinanceira.Insere(aplicacaoFinanceira, aplicacaoFinanceiraComprovanteArquivo);
                        mensagem = "Aplicação Financeira inserida com sucesso.";
                    }
                    else
                    {
                        rnAplicacaoFinanceira.Atualiza(aplicacaoFinanceira, aplicacaoFinanceiraComprovanteArquivo);
                        mensagem = "Aplicação Financeira atualizada com sucesso.";
                    }

                    if (tseUnidadeResponsavelDados.DBValue != tseUnidadeResponsavel.DBValue)
                    {
                        tseUnidadeResponsavel.DBValue = tseUnidadeResponsavelDados.DBValue;
                        tseExtratoBancario.ResetValue();
                    }


                    odsAplicacoesFinanceiras.Select();
                    odsAplicacoesFinanceiras.DataBind();
                    grdAplicacoesFinanceiras.DataBind();

                    LimpaControles();
                    pnlDados.Visible = false;
                    btnNovo.Visible = true;
                    btnCancel.Visible = false;

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('" + mensagem + ".');", true);
                    lblMensagem.Text = mensagem;

                    }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void LimpaControles()
        {
            tseUnidadeResponsavelDados.ResetValue();
            tseExtratoBancarioDados.ResetValue();
            txtValorAplicacao.Text = String.Empty;
            txtJustificativa.Text = String.Empty;
        }


        protected void btnVisualizar_Command(object sender, CommandEventArgs e)
        {
            try
            {
                string embed = string.Empty;
                bimgArquivo.Visible = false;
                ltEmbed.Visible = false;
                DataTable dados = new DataTable();
                RN.PrestacaoContas.AplicacaoFinanceiraComprovanteArquivo rnDeclaracaoFiscalArquivo = new Techne.Lyceum.RN.PrestacaoContas.AplicacaoFinanceiraComprovanteArquivo();

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
                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/PrestacaoContas/FileCS.ashx?Tabela=AplicacaoFinanceiraComprovanteArquivo&Id="), chave[0].ToString());
                        ltEmbed.Visible = true;
                        pucVisualizarArquivo.Width = Unit.Pixel(880);
                        pucVisualizarArquivo.Height = Unit.Pixel(580);
                    }
                    else
                    {
                        pucVisualizarArquivo.Width = Unit.Pixel(350);
                        pucVisualizarArquivo.Height = Unit.Pixel(350);
                        bimgArquivo.ContentBytes = rnDeclaracaoFiscalArquivo.ObtemArquivoPor(Convert.ToInt32(chave[0]));
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
             

        protected void tseUnidadeResponsavel_Changed(object sender, EventArgs args)
        {
            try
            {
                var sessao = SessaoUsuario.GetSessaoUsuario();
               
                this.pnlDados.Visible = false;


                if (!this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (this.tseUnidadeResponsavel.IsValidDBValue)
                    {
                        if (!this.tseUnidadeResponsavel["unidade_ens"].IsNull)
                        {
                            sessao.Escola = Convert.ToString(this.tseUnidadeResponsavel.DBValue);
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

        protected void tseUnidadeResponsavelDados_Changed(object sender, EventArgs args)
        {
            try
            {             


                if (!this.tseUnidadeResponsavelDados.DBValue.IsNull)
                {
                    if (!this.tseUnidadeResponsavelDados.IsValidDBValue)
                    {                      
                        this.lblMensagem.Text = "Unidade de Ensino não cadastrada.";
                    }
                }
                else
                {                    

                    this.lblMensagem.Text = "Favor consultar uma unidade de ensino.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdAplicacoesFinanceiras_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {

            string censo = grdAplicacoesFinanceiras.GetRowValues(e.VisibleIndex, "CENSO").ToString();
            string valor = grdAplicacoesFinanceiras.GetRowValues(e.VisibleIndex, "VALOR").ToString();
            string justificativa = grdAplicacoesFinanceiras.GetRowValues(e.VisibleIndex, "JUSTIFICATIVA").ToString();
            string extratoBancarioId = grdAplicacoesFinanceiras.GetRowValues(e.VisibleIndex, "EXTRATOBANCARIOID").ToString();
            string aplicacaoFinanceiraId = grdAplicacoesFinanceiras.GetRowValues(e.VisibleIndex, "APLICACAOFINANCEIRAID").ToString();
            string aplicacaoFinanceiraComprovanteArquivoId = grdAplicacoesFinanceiras.GetRowValues(e.VisibleIndex, "APLICACAOFINANCEIRACOMPROVANTEARQUIVOID") != null ? grdAplicacoesFinanceiras.GetRowValues(e.VisibleIndex, "APLICACAOFINANCEIRACOMPROVANTEARQUIVOID").ToString() : null;

            LimpaControles();
            btnCancel.Visible = true;
            pnlDados.Visible = true;

            tseUnidadeResponsavelDados.DBValue = censo;
            tseExtratoBancarioDados.DBValue = Convert.ToInt32(extratoBancarioId);
            txtValorAplicacao.Text = string.Format("{0:N2}", valor); 
            txtJustificativa.Text = justificativa;

            if (e.ButtonID == "btnEditarCustom")
            {
                hdnAplicacaoFinanceiraId.Value = aplicacaoFinanceiraId;
                hdnAplicacaoFinanceiraComprovanteArquivoId.Value = aplicacaoFinanceiraComprovanteArquivoId;

            }
       
            if (e.ButtonID == "btnVizualizar")
            {

            }

        }  

        protected void grdAplicacoesFinanceiras_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;

            if (e.CellType == GridViewTableCommandCellType.Filter)
            {
                e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                return;
            }

            if (e.ButtonID == "btnEditarCustom")
            {
                if (Permission.AllowUpdate)
                {
                    e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.True;
                }
            }
        }

        protected void btnDetalhes_Command(object sender, CommandEventArgs e)
        {
            try
            {

                string[] chave = e.CommandArgument.ToString().Split(new char[] { ',' });

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        public void Delete(object APLICACAOFINANCEIRAID) { }

        protected void grdAplicacoesFinanceiras_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {

                ValidacaoDados validacao = new ValidacaoDados();
                RN.PrestacaoContas.AplicacaoFinanceira rnAplicacaoFinanceira = new RN.PrestacaoContas.AplicacaoFinanceira();

                int aplicacaoFinanceiraId = Convert.ToInt32(e.Keys["APLICACAOFINANCEIRAID"]);

                validacao = rnAplicacaoFinanceira.ValidaRemocao(aplicacaoFinanceiraId, User.Identity.Name, Convert.ToInt32(e.Values["ANO"]), Convert.ToInt32(e.Values["MES"]), Convert.ToString(e.Values["CENSO"]));

                if (validacao.Valido)
                {
                    rnAplicacaoFinanceira.Remove(aplicacaoFinanceiraId, User.Identity.Name);
                    grdAplicacoesFinanceiras.DataBind();
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
             
        }



        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            if (String.IsNullOrEmpty(Convert.ToString(tseUnidadeResponsavel.DBValue)))
            {
                lblMensagem.Text = " Selecione Unidade de Ensino ";
                return;
            }

            try
            {
                odsAplicacoesFinanceiras.Select();
                odsAplicacoesFinanceiras.DataBind();
                grdAplicacoesFinanceiras.DataBind();


            
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void grdAplicacoesFinanceiras_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "VALOR")
            {
                var valor = e.GetListSourceFieldValue("VALOR");

                var valorFormatado = string.Format("{0:N2}", valor);

                e.Value = valorFormatado;
            }
        }
       

    }
}
