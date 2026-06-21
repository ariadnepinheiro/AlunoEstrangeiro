using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Controls;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxTabControl;
using System.Data;
using Techne.Lyceum.RN.DTOs;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Transporte
{
    [NavUrl("~/Transporte/DesvinculaAlunoRota.aspx")]
    [ControlText("Desvincular Aluno da Rota")]
    [Title("Desvincular Aluno da Rota")]
    public partial class DesvinculaAlunoRota : TPage
    {
        public enum TipoOperacao
        {           
            Inicial,           
            Consultar,
            Cancelar,
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

        public object ListarRotaAlunoVolta(object rotaTrajeto)
        {
            RN.Transporte.RotaAluno rnRotaAluno = new Techne.Lyceum.RN.Transporte.RotaAluno();

            var rota = rotaTrajeto != null ? rotaTrajeto.ToString() : null;

            if (!string.IsNullOrEmpty(rota))
            {
                return rnRotaAluno.ListaVoltaPor(Convert.ToInt32(rota));
            }
            return null;
        }

        public object ListarRotaAlunoIda(object rotaTrajeto)
        {
            RN.Transporte.RotaAluno rnRotaAluno = new Techne.Lyceum.RN.Transporte.RotaAluno();

            var rota = rotaTrajeto != null ? rotaTrajeto.ToString() : null;

            if (!string.IsNullOrEmpty(rota))
            {
                return rnRotaAluno.ListaIdaPor(Convert.ToInt32(rota));
            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;               

                if (!IsPostBack)
                {
                    tseUnidadeFiltro.ResetValue();
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
            TituloGrid(grdRotaAlunoIda, "Ida");
            TituloGrid(grdRotaAlunoVolta, "Volta");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcessoGrid();         
        }

        protected void ControlaAcessoGrid()
        {
            if (grdRotaAlunoIda != null || grdRotaAlunoVolta != null)
            {
                if (!Permission.AllowDelete)
                {
                    grdRotaAlunoIda.Columns[""].Visible = false;
                    grdRotaAlunoVolta.Columns[""].Visible = false;
                }
            }

            ControlaAcesso(grdRotaAlunoIda);
            ControlaAcesso(grdRotaAlunoVolta);
        }

        private void ControlarTipoOperacao()
        {
            RN.Transporte.Rota rnRota = new Techne.Lyceum.RN.Transporte.Rota();

            switch (this._tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] {  };
                        ControlarVisibilidadeControle(controles);                        
                        tseRota.ResetValue();
                        pnAbas.Visible = false;
                        pcRota.TabPages[0].Enabled = false;
                        pcRota.TabPages[1].Enabled = false;                      
                        tseRota.Mode = Techne.Controls.ControlMode.Edit;
                        grdRotaAlunoIda.DataSource = null;
                        grdRotaAlunoIda.DataBind();
                        grdRotaAlunoVolta.DataSource = null;
                        grdRotaAlunoVolta.DataBind();
                        break;
                    }              
              
                case TipoOperacao.Consultar:
                    {
                        ImageButton[] controles = new ImageButton[] {  };
                        ControlarVisibilidadeControle(controles);
                        pcRota.ActiveTabIndex = 0;
                        pnlDados.Visible = true;
                        pcRota.TabPages[0].Enabled = true;
                        pcRota.TabPages[1].Enabled = true;
                        pnAbas.Visible = true;
                       
                        tseUnidadeFiltro.Enabled = true;
                        tseRota.Enabled = true;   
                        hdnRotaId.Value = Convert.ToString(tseRota["rotaid"]);

                        break;
                    }
                case TipoOperacao.Cancelar:
                    {
                        ImageButton[] controles = new ImageButton[] {  };
                        ControlarVisibilidadeControle(controles);
                        pnlDados.Visible = false;

                        tseUnidadeFiltro.ResetValue();
                        tseUnidadeFiltro.Mode = ControlMode.Edit;
                        tseUnidadeFiltro.ReadOnly = false;
                        tseUnidadeFiltro.Enabled = true;
                        tseRota.ResetValue();
                        tseRota.Mode = ControlMode.Edit;
                        tseRota.ReadOnly = false;
                        tseRota.Enabled = true;

                        grdRotaAlunoIda.DataSource = null;
                        grdRotaAlunoIda.DataBind();
                        grdRotaAlunoIda.CancelEdit();
                        grdRotaAlunoVolta.DataSource = null;
                        grdRotaAlunoVolta.DataBind();
                        grdRotaAlunoVolta.CancelEdit();
                       
                        break;
                    }
                
                 
            }
        }

        private void ControlarVisibilidadeControle(ImageButton[] imgBotoes)
        {
            RetiraVisibilidadeBotao();

            if (imgBotoes != null)
            {
                foreach (ImageButton botao in imgBotoes)
                {
                    botao.Visible = true;
                }
            }

        }
        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
        }
        protected void tseRota_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (!this.tseRota.DBValue.IsNull)
                {
                    if (!this.tseRota.IsValidDBValue)
                    {
                        this._tipoOperacao = TipoOperacao.Inicial;
                        this.lblMensagem.Text = "Rota não cadastrado.";
                    }
                    else
                    {
                        this._tipoOperacao = TipoOperacao.Consultar;
                    }
                }
                else
                {
                    this._tipoOperacao = TipoOperacao.Inicial;
                    this.lblMensagem.Text = "Favor consultar uma rota.";
                }

                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeFiltro_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }
              
                var sessao = RN.SessaoUsuario.GetSessaoUsuario();
                this._tipoOperacao = TipoOperacao.Inicial;
                if (!this.tseUnidadeFiltro.DBValue.IsNull)
                {
                    if (this.tseUnidadeFiltro.IsValidDBValue)
                    {
                        
                        if (sessao != null)
                        {
                            sessao.Escola = Convert.ToString(this.tseUnidadeFiltro.DBValue);
                        }
                    }
                    else
                    {
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
                    if (sessao != null)
                    {
                        sessao.Escola = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Regional = string.Empty;
                    }
                    lblMensagem.Text = "Favor consultar uma unidade de ensino.";

                }
                ControlarTipoOperacao();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

        protected void grdRotaAlunoIda_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdRotaAlunoIda);
        }

        protected void grdRotaAlunoIda_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdRotaAlunoIda.Settings.ShowFilterRow = false;
        }

        protected void grdRotaAlunoIda_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdRotaAlunoIda.Settings.ShowFilterRow = false;
        }

        protected void grdRotaAlunoIda_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {            
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.RotaAluno rnRotaAluno = new Techne.Lyceum.RN.Transporte.RotaAluno();
            int rotaAlunoId = 0;
            int rotaId = (this.tseRota.IsValidDBValue && !this.tseRota.DBValue.IsNull) ? Convert.ToInt32(tseRota["rotaId"]) : -1;

            rotaAlunoId = Convert.ToInt32(e.Keys["ROTAALUNOID"]);

            validacao = rnRotaAluno.ValidaRemocao(rotaId, rotaAlunoId);

            if (validacao.Valido)
            {
                rnRotaAluno.Remove(rotaAlunoId);
                grdRotaAlunoIda.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        public void DeleteIda(object ROTAALUNOID)
        { }

        protected void grdRotaAlunoVolta_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdRotaAlunoVolta);
        }

        protected void grdRotaAlunoVolta_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdRotaAlunoVolta.Settings.ShowFilterRow = false;
        }

        protected void grdRotaAlunoVolta_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdRotaAlunoVolta.Settings.ShowFilterRow = false;
        }

        protected void grdRotaAlunoVolta_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.RotaAluno rnRotaAluno = new Techne.Lyceum.RN.Transporte.RotaAluno();
            int rotaAlunoId = 0;
            int rotaId = (this.tseRota.IsValidDBValue && !this.tseRota.DBValue.IsNull) ? Convert.ToInt32(tseRota["rotaId"]) : -1;

            rotaAlunoId = Convert.ToInt32(e.Keys["ROTAALUNOID"]);

            validacao = rnRotaAluno.ValidaRemocao(rotaId, rotaAlunoId);

            if (validacao.Valido)
            {
                rnRotaAluno.Remove(rotaAlunoId);
                grdRotaAlunoVolta.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        public void DeleteVolta(object ROTAALUNOID)
        { }

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

    }
}
