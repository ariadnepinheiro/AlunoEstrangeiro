using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Data;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxTabControl;
using Techne.Controls;
using Techne.Lyceum.RN.InspecaoEscolar.DTOs;
using Techne.Lyceum.RN.Util;
using Techne.Web;


namespace Techne.Lyceum.Net.InspecaoEscolar
{
    [NavUrl("~/InspecaoEscolar/ReabeturaRT.aspx"),
 ControlText("Relatório de Trabalho de Infraestrutura"),
 Title("Relatório de Trabalho de Infraestrutura"),]

    public partial class ReaberturaRT : TPage
    {
        public enum TipoOperacao
        {

            Cancelar,
            Inicial,
            Consultar,
            Reabrir
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

        private void RetiraVisibilidadeBotao()
        {
            btnReabrir.Visible = false;
        }

        private void ControlarVisibilidadeControle(ImageButton[] imgBotoes, Button[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (var img in imgBotoes)
            {
                img.Visible = true;
            }
            foreach (var botao in botoes)
            {
                botao.Visible = true;
            }
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
        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            try
            {
                ControlaAcesso(btnReabrir, AcaoControle.novo);

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void tseCampanha_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                // pnlReabertura.Visible = false;

                if (!this.tseCampanha.DBValue.IsNull)
                {
                    if (!this.tseCampanha.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Campanha não cadastrada.";

                    }
                    else
                    {
                        if (!this.tseUnidade.DBValue.IsNull && this.tseUnidade.IsValidDBValue)
                        {
                            _tipoOperacao = TipoOperacao.Consultar;
                            ControlarTipoOperacao();
                        }
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar uma campanha.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidade_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                tseCampanha.ResetValue();

                //pnlReabertura.Visible = false;

                if (!this.tseUnidade.DBValue.IsNull)
                {
                    if (!this.tseUnidade.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Unidade de Ensino não cadastrada.";
                    }
                    else
                    {
                        if (!this.tseCampanha.DBValue.IsNull && this.tseCampanha.IsValidDBValue)
                        {
                            _tipoOperacao = TipoOperacao.Consultar;
                            ControlarTipoOperacao();
                        }
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

        protected void btnReabrir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (!rblReabrir.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (rblReabrir.SelectedValue == "1")
                    {
                        _tipoOperacao = TipoOperacao.Reabrir;
                        ControlarTipoOperacao();
                    }
                    else
                    {
                        _tipoOperacao = TipoOperacao.Inicial;
                        ControlarTipoOperacao();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        private void ControlarTipoOperacao()
        {
            switch (this._tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] imgControles = new ImageButton[] { };
                        Button[] controles = new Button[] { };
                        ControlarVisibilidadeControle(imgControles, controles);
                        pnlReabertura.Visible = false;
                        tseUnidade.Mode = ControlMode.Edit;
                        tseCampanha.Mode = ControlMode.Edit;
                        tseUnidade.ResetValue();
                        tseCampanha.ResetValue();
                        rblReabrir.ClearSelection();
                        hdnDataFinalizacao.Value = string.Empty;
                        hdnFinalizado.Value = string.Empty;
                        hdnCampanhaEscolaId.Value = string.Empty;
                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        RN.InspecaoEscolar.CampanhaEscola rnCampanhaEscola = new Techne.Lyceum.RN.InspecaoEscolar.CampanhaEscola();
                        RN.InspecaoEscolar.Entidades.CampanhaEscola campanhaEscola = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.CampanhaEscola();
                        ImageButton[] imgControles = new ImageButton[] { };
                        Button[] controles = new Button[] { };
                        imgControles = new ImageButton[] { btnReabrir };

                        hdnDataFinalizacao.Value = string.Empty;
                        hdnFinalizado.Value = string.Empty;
                        hdnCampanhaEscolaId.Value = string.Empty;
                        rblReabrir.ClearSelection();
                        pnlReabertura.Visible = false;

                        campanhaEscola = rnCampanhaEscola.ObtemPor(Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                        hdnCampanhaEscolaId.Value = campanhaEscola.CampanhaEscolaId == null ? string.Empty : campanhaEscola.CampanhaEscolaId.ToString();

                        if (campanhaEscola.CampanhaEscolaId != 0)
                        {

                            if (campanhaEscola.Finalizado == null)
                            {
                                imgControles = new ImageButton[] { };
                                lblMensagem.Text = "Relatório de Trabalho de Infraestrutura ainda não foi finalizado.";
                            }
                            else
                            {
                                hdnFinalizado.Value = campanhaEscola.Finalizado != null ? (campanhaEscola.Finalizado.Value ? "S" : "N") : string.Empty;
                                hdnDataFinalizacao.Value = campanhaEscola.DataFinalizacao != null ? campanhaEscola.DataFinalizacao.Value.ToShortDateString() : string.Empty;
                                pnlReabertura.Visible = true;
                            }


                        }
                        else
                        {
                            imgControles = new ImageButton[] { };
                            tseUnidade.Mode = ControlMode.Edit;
                            tseCampanha.Mode = ControlMode.Edit;
                            lblMensagem.Text = "Esta unidade escolar ainda não iniciou o preenchimento do RT.";
                        }

                        ControlarVisibilidadeControle(imgControles, controles);


                        break;
                    }

                case TipoOperacao.Reabrir:
                    {
                        RN.InspecaoEscolar.CampanhaEscola rnCampanhaEscola = new Techne.Lyceum.RN.InspecaoEscolar.CampanhaEscola();
                        RN.InspecaoEscolar.Entidades.CampanhaEscola campanhaEscola = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.CampanhaEscola();
                        ValidacaoDados validacao = new ValidacaoDados();

                        ImageButton[] imgControles = new ImageButton[] { };
                        Button[] controles = new Button[] { };
                        imgControles = new ImageButton[] { };

                        ControlarVisibilidadeControle(imgControles, controles);

                        campanhaEscola.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                        campanhaEscola.CampanhaId = (!this.tseCampanha.DBValue.IsNull && this.tseCampanha.IsValidDBValue) ? Convert.ToInt32(tseCampanha.DBValue) : -1;
                        campanhaEscola.DataFinalizacao = (DateTime?)null;
                        campanhaEscola.Finalizado = (bool?)null;
                        campanhaEscola.Unidade_Ens = (!this.tseUnidade.DBValue.IsNull && this.tseUnidade.IsValidDBValue) ? tseUnidade.DBValue.ToString() : null;
                        campanhaEscola.Aceito = (bool?)null;
                        campanhaEscola.DataAceite = (DateTime?)null;
                        campanhaEscola.UsuarioAceiteId = null;
                        campanhaEscola.UsuarioId = User.Identity.Name;

                        validacao = rnCampanhaEscola.ValidaReabertura(campanhaEscola, false);

                        if (validacao.Valido)
                        {
                            rnCampanhaEscola.Reabri(campanhaEscola);
                           
                            _tipoOperacao = TipoOperacao.Inicial;
                            ControlarTipoOperacao();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Reabertura RT", "alert('Reabertura realizada com sucesso.')", true);
                        }
                        else
                        {
                            imgControles = new ImageButton[] { btnReabrir };

                            lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        }
                        ControlarVisibilidadeControle(imgControles, controles);

                        break;
                    }

            }
        }
    }
}
