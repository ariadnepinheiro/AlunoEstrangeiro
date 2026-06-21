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
    [NavUrl("~/InspecaoEscolar/Consideracoes.aspx"),
   ControlText("Considerações"),
   Title("Considerações"),]

    public partial class Consideracoes : TPage
    {
        public enum TipoOperacao
        {
           
            Cancelar,
            Inicial,
            Consultar,            
            Aceite
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
            btnAceitar.Visible = false;
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
                ControlaAcesso(btnAceitar, AcaoControle.novo);

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

                pnlConsideracoes.Visible = false;

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

                pnlConsideracoes.Visible = false;

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

        protected void btnAceitar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Aceite;
                ControlarTipoOperacao();
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
                        pnlConsideracoes.Visible = false;
                        tseUnidade.Mode = ControlMode.Edit;
                        tseCampanha.Mode = ControlMode.Edit;
                        txtConsideracoes.Text = string.Empty;
                        rblAceite.ClearSelection();
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
                        imgControles = new ImageButton[] { btnAceitar };

                        hdnDataFinalizacao.Value = string.Empty;
                        hdnFinalizado.Value = string.Empty;
                        hdnCampanhaEscolaId.Value = string.Empty;
                        rblAceite.ClearSelection();
                        txtConsideracoes.Text = string.Empty;
                        pnlConsideracoes.Visible = false;

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
                                pnlConsideracoes.Visible = true;
                            }


                            txtConsideracoes.Text = !campanhaEscola.ConsideracaoFinal.IsNullOrEmptyOrWhiteSpace() ? campanhaEscola.ConsideracaoFinal.Trim() : string.Empty;

                            if (campanhaEscola.Aceito != null)
                            {
                                rblAceite.SelectedValue = campanhaEscola.Aceito == true ? "1" : "0";
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

                case TipoOperacao.Aceite:
                    {
                        RN.InspecaoEscolar.CampanhaEscola rnCampanhaEscola = new Techne.Lyceum.RN.InspecaoEscolar.CampanhaEscola();
                        RN.InspecaoEscolar.Entidades.CampanhaEscola campanhaEscola = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.CampanhaEscola();
                        ValidacaoDados validacao = new ValidacaoDados();

                        ImageButton[] imgControles = new ImageButton[] { };
                        Button[] controles = new Button[] { };
                        imgControles = new ImageButton[] { btnAceitar };

                        ControlarVisibilidadeControle(imgControles, controles);
                        
                        campanhaEscola.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                        campanhaEscola.CampanhaId = (!this.tseCampanha.DBValue.IsNull && this.tseCampanha.IsValidDBValue) ? Convert.ToInt32(tseCampanha.DBValue) : -1;
                        campanhaEscola.DataFinalizacao = !hdnDataFinalizacao.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(hdnDataFinalizacao.Value) : (DateTime?)null;
                        campanhaEscola.Finalizado = !hdnFinalizado.Value.IsNullOrEmptyOrWhiteSpace() ? (hdnFinalizado.Value == "S" ? true : false) : (bool?)null;
                        campanhaEscola.Unidade_Ens = (!this.tseUnidade.DBValue.IsNull && this.tseUnidade.IsValidDBValue) ? tseUnidade.DBValue.ToString() : null;
                        campanhaEscola.Aceito = !rblAceite.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblAceite.SelectedValue == "1" ? true : false ) : (bool?)null;
                        campanhaEscola.ConsideracaoFinal = !txtConsideracoes.Text.IsNullOrEmptyOrWhiteSpace() ? txtConsideracoes.Text.Trim() : null;
                        campanhaEscola.DataAceite = DateTime.Now;
                        campanhaEscola.UsuarioAceiteId = User.Identity.Name;

                        if (campanhaEscola.Aceito != null)
                        {
                            if (!campanhaEscola.Aceito.Value)
                            {
                                campanhaEscola.DataFinalizacao = null;
                                campanhaEscola.Finalizado = null;
                            }
                        }

                        validacao = rnCampanhaEscola.ValidaFinalizacao(campanhaEscola, false);

                        if (validacao.Valido)
                        {
                            rnCampanhaEscola.Aceita(campanhaEscola);
                            imgControles = new ImageButton[] {btnAceitar };
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Considerações RT", "alert('Considerações realizada com sucesso.')", true);
                        }
                        else
                        {
                            lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        }
                        ControlarVisibilidadeControle(imgControles, controles);

                        break;
                    }

            }
        }
    }
}
