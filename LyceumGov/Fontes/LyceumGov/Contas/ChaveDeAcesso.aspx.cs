using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using System.Data;
using DevExpress.Web.ASPxGridView;
using Techne.Web;
using DevExpress.Web.ASPxTabControl;
using System.Text;

namespace Techne.Lyceum.Net.Contas
{
    [NavUrl("~/Contas/ChaveDeAcesso.aspx")]
    [ControlText("Chave de Acesso - Nota Fiscal")]
    [Title("Chave de Acesso - Nota Fiscal")]

    public partial class ChaveDeAcesso : TPage
    {
        public enum TipoOperacao
        {
            Novo,
            Cancelar,
            Inicial,
            Consultar,
            Sucesso
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

        private void ControlarTipoOperacao()
        {
            switch (this._tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { };
                        ControlarVisibilidadeControle(controles);
                        pnlDados.Visible = false;
                        pnlGrid.Visible = false;
                        LimparTela();
                        tseUnidadeResponsavel.ResetValue();
                        tseMunicipio.ResetValue();
                        tseRegional.ResetValue();
                        break;

                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);

                        grdNotaFiscal.DataBind();
                        LimparTela();
                        pnlDados.Visible = false;
                        break;
                    }
                case TipoOperacao.Novo:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);
                        pnlDados.Visible = true;
                        LimparTela();

                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        LimparTela();
                        pnlGrid.Visible = true;

                        break;
                    }
                case TipoOperacao.Cancelar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };

                        ControlarVisibilidadeControle(controles);
                        pnlDados.Visible = false;
                        LimparTela();
                        grdNotaFiscal.DataBind();
                        break;
                    }
            }
        }


        public object Listar(object unidade_ens)
        {
            RN.NotaFiscal.AcompanhamentoNota rnAcompanhamentoNota = new Techne.Lyceum.RN.NotaFiscal.AcompanhamentoNota();

            var unidade = unidade_ens.ToString();

            if (!unidade.IsNullOrEmptyOrWhiteSpace())
            {
                return rnAcompanhamentoNota.ListaPor(unidade.ToString());
            }

            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!Page.IsPostBack)
                {
                    this._tipoOperacao = TipoOperacao.Inicial;
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
            TituloGrid(grdNotaFiscal, string.Empty);
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdNotaFiscal);
            ControlaAcesso(btnSalvar, AcaoControle.novo);
            ControlaAcesso(btnNovo, AcaoControle.novo);
        }
    
        private void LimparTela()
        {
            txtProcesso.Text = string.Empty;
            txtCodigoAcesso.Text = string.Empty;
            dtProcesso.Text = string.Empty;
            rblValido.ClearSelection();
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                var sessao = RN.SessaoUsuario.GetSessaoUsuario();
                LimparTela();

                pnlDados.Visible = false;
                pnlGrid.Visible = false;

                if (sessao != null)
                {
                    if (!this.tseRegional.DBValue.IsNull)
                    {
                        if (this.tseRegional.IsValidDBValue)
                        {
                            LimparTela();
                            sessao.Regional = Convert.ToString(tseRegional.DBValue);
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;

                            tseMunicipio.ResetValue();
                            tseUnidadeResponsavel.ResetValue();
                        }
                        else
                        {
                            sessao.Regional = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                        }
                    }
                    else
                    {
                        sessao.Regional = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                        tseMunicipio.ResetValue();
                        tseUnidadeResponsavel.ResetValue();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                var sessao = RN.SessaoUsuario.GetSessaoUsuario();
                LimparTela();

                pnlDados.Visible = false;
                pnlGrid.Visible = false;

                if (sessao != null)
                {
                    if (!this.tseMunicipio.DBValue.IsNull)
                    {
                        if (this.tseMunicipio.IsValidDBValue)
                        {
                            sessao.Municipio = Convert.ToString(tseMunicipio.DBValue);
                            sessao.Escola = string.Empty;
                        }
                        else
                        {
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                            tseUnidadeResponsavel.ResetValue();
                        }
                    }
                    else
                    {
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                        tseUnidadeResponsavel.ResetValue();
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
                if (this.Page.IsCallback)
                {
                    return;
                }
                var sessao = RN.SessaoUsuario.GetSessaoUsuario();
                LimparTela();

                pnlDados.Visible = false;
                pnlGrid.Visible = false;

                if (!this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (this.tseUnidadeResponsavel.IsValidDBValue)
                    {
                        this._tipoOperacao = TipoOperacao.Consultar;

                        if (!this.tseUnidadeResponsavel["unidade_ens"].IsNull)
                        {
                            this.tseRegional.Value = this.tseUnidadeResponsavel["id_regional"];
                            this.tseMunicipio.Value = this.tseUnidadeResponsavel["municipio"];
                        }

                        if (sessao != null)
                        {
                            sessao.Escola = Convert.ToString(this.tseUnidadeResponsavel.DBValue);
                            sessao.Regional = Convert.ToString(this.tseRegional.DBValue);
                            sessao.Municipio = Convert.ToString(this.tseMunicipio.DBValue);
                        }
                    }
                    else
                    {
                        this._tipoOperacao = TipoOperacao.Inicial;

                        lblMensagem.Text = "Unidade de ensino não encontrada.";
                        if (sessao != null)
                        {
                            sessao.Escola = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Regional = string.Empty;
                        }
                    }
                }
                else
                {
                    this._tipoOperacao = TipoOperacao.Inicial;

                    if (sessao != null)
                    {
                        sessao.Escola = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Regional = string.Empty;
                    }
                }

                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

        public void Update(object CHAVEACESSO, object PROCESSO, object DATAPROCESSO, object VALIDO, object DATACADASTRO, object NOME, object ACOMPANHAMENTONOTAID) { }


        public void Delete(object ACOMPANHAMENTONOTAID) { }

        protected void grdNotaFiscal_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdNotaFiscal);
        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                RN.NotaFiscal.AcompanhamentoNota rnAcompanhamentoNota = new Techne.Lyceum.RN.NotaFiscal.AcompanhamentoNota();

                string msgChaveDuplicada = string.Empty;
                string mensagem = string.Empty;
                StringBuilder mensagemFormatada = new StringBuilder();

                var acompanhamento = new RN.NotaFiscal.Entidades.AcompanhamentoNota
                {
                    Censo = (this.tseUnidadeResponsavel.IsValidDBValue && !this.tseUnidadeResponsavel.DBValue.IsNull) ? tseUnidadeResponsavel.DBValue.ToString() : null,
                    Processo = !txtProcesso.Text.Trim().IsNullOrEmptyOrWhiteSpace() ? txtProcesso.Text.Trim() : null,
                    ChaveAcesso = !txtCodigoAcesso.Text.IsNullOrEmptyOrWhiteSpace() ? txtCodigoAcesso.Text.Trim() : null,
                    Valido = !rblValido.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblValido.SelectedValue == "1" ? true : false) : (Boolean?)null,
                    DataProcesso = !string.IsNullOrEmpty(dtProcesso.Text.Trim()) ? dtProcesso.Date : DateTime.MinValue,
                    UsuarioId = User.Identity.Name,
                };

                var validacao = rnAcompanhamentoNota.Valida(acompanhamento, true);

                if (validacao.Valido)
                {
                    rnAcompanhamentoNota.Insere(acompanhamento, out msgChaveDuplicada);
                    mensagemFormatada.Append("Chave de Acesso incluído com sucesso. <br />");

                    if (!msgChaveDuplicada.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagemFormatada.Append(msgChaveDuplicada + " <br />");
                    }

                    lblMensagem.Text = mensagemFormatada.ToString();

                    //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup",
                    //    "alert('" + mensagemFormatada.ToString().Replace("<br />", "\n") + "');", true);


                    this._tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();
                }
                else
                {
                    this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }

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
                _tipoOperacao = TipoOperacao.Novo;
                ControlarTipoOperacao();
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
                _tipoOperacao = TipoOperacao.Cancelar;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (var botao in botoes)
            {
                botao.Visible = true;
            }
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnNovo.Visible = false;
            btnSalvar.Visible = false;
        }


        protected void grdNotaFiscal_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.NotaFiscal.AcompanhamentoNota rnAcompanhamentoNota = new Techne.Lyceum.RN.NotaFiscal.AcompanhamentoNota();
                RN.NotaFiscal.Entidades.AcompanhamentoNota acompanhamento = new Techne.Lyceum.RN.NotaFiscal.Entidades.AcompanhamentoNota();
                string msgChaveDuplicada = string.Empty;
                string mensagem = string.Empty;
                StringBuilder mensagemFormatada = new StringBuilder();

                acompanhamento.Censo = (this.tseUnidadeResponsavel.IsValidDBValue && !this.tseUnidadeResponsavel.DBValue.IsNull) ? tseUnidadeResponsavel.DBValue.ToString() : null;
                acompanhamento.ChaveAcesso = e.NewValues["CHAVEACESSO"] != null ? e.NewValues["CHAVEACESSO"].ToString().Trim().ToUpper() : null;
                acompanhamento.Processo = e.NewValues["PROCESSO"] != null ? e.NewValues["PROCESSO"].ToString() : null;
                acompanhamento.DataProcesso = e.NewValues["DATAPROCESSO"] != null ? Convert.ToDateTime(e.NewValues["DATAPROCESSO"]) : DateTime.MinValue;
                acompanhamento.Valido = (e.NewValues["VALIDO"] == null || Convert.ToBoolean(e.NewValues["VALIDO"]) == false) ? false : true;
                acompanhamento.UsuarioId = User.Identity.Name;
                acompanhamento.AcompanhamentoNotaId = Convert.ToInt32(e.Keys["ACOMPANHAMENTONOTAID"]);

                validacao = rnAcompanhamentoNota.Valida(acompanhamento, false);

                if (validacao.Valido)
                {
                    rnAcompanhamentoNota.Atualiza(acompanhamento, out msgChaveDuplicada);


                    mensagemFormatada.Append("Chave de Acesso atualizada com sucesso.");

                    if (!msgChaveDuplicada.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagemFormatada.Append(msgChaveDuplicada);
                    }

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup",
                       "alert('" + mensagemFormatada.ToString() + "');", true);

                    //grdNotaFiscal.DataBind();
                    this._tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();
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


        protected void grdNotaFiscal_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.NotaFiscal.AcompanhamentoNota rnAcompanhamentoNota = new Techne.Lyceum.RN.NotaFiscal.AcompanhamentoNota();
                RN.NotaFiscal.Entidades.AcompanhamentoNota acompanhamento = new Techne.Lyceum.RN.NotaFiscal.Entidades.AcompanhamentoNota();

                int acompanhamentoNotaId = 0;

                acompanhamentoNotaId = Convert.ToInt32(e.Keys["ACOMPANHAMENTONOTAID"]);

                validacao = rnAcompanhamentoNota.ValidaRemocao(acompanhamentoNotaId);

                if (validacao.Valido)
                {
                    rnAcompanhamentoNota.Remove(acompanhamentoNotaId);
                    grdNotaFiscal.DataBind();
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

    }
}
