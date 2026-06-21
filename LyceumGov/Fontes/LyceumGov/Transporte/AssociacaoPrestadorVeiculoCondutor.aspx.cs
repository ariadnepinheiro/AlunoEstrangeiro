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
using Techne.Controls;

namespace Techne.Lyceum.Net.Transporte
{
    [NavUrl("~/Transporte/AssociacaoPrestadorVeiculoCondutor.aspx")]
    [ControlText("AssociacaoPrestadorVeiculoCondutor")]
    [Title("Associação de Veículos")]

    public partial class AssociacaoPrestadorVeiculoCondutor : TPage
    {
        public enum TipoOperacao
        {
            Novo,
            Cancelar,
            Consultar,
            Inicial,
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
                        tsePrestador.ResetValue();
                        tseVeiculo.ResetValue();
                        tseCondutor.ResetValue();
                        pnlGrid.Visible = false;
                        grdAssociacoes.DataSource = null;
                        grdAssociacoes.DataBind();
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);

                        grdAssociacoes.DataBind();
                        tseCondutor.ResetValue();
                        tseVeiculo.ResetValue();
                        pnlDados.Visible = false;
                        break;
                    }
                case TipoOperacao.Novo:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnIncluir };
                        ControlarVisibilidadeControle(controles);
                        pnlDados.Visible = true;
                        tseVeiculo.ResetValue();
                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        tseVeiculo.ResetValue();
                        pnlDados.Visible = false;
                        pnlGrid.Visible = true;
                        break;
                    }
                case TipoOperacao.Cancelar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        pnlDados.Visible = false;
                        break;
                    }
            }
        }


        public object ListarAssociacao(object prestadorId)
        {
            RN.Transporte.Veiculo rnVeiculo = new Techne.Lyceum.RN.Transporte.Veiculo();

            if (!string.IsNullOrEmpty(prestadorId.ToString()))
            {
                return rnVeiculo.ListaPrestadorCondutorVeiculoPor(Convert.ToInt32(prestadorId));
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
            TituloGrid(grdAssociacoes, "Associação de Veículos");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnIncluir, AcaoControle.novo);
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(grdAssociacoes);
            ControlaAcessoGrid();
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
            btnIncluir.Visible = false;
        }
        protected void btnIncluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                try
                {
                    ValidacaoDados validacao = new ValidacaoDados();
                    RN.Transporte.Veiculo rnVeiculo = new Techne.Lyceum.RN.Transporte.Veiculo();
                    int prestadorId, veiculoId, condutorId = 0;

                    prestadorId = (this.tsePrestador.IsValidDBValue && !this.tsePrestador.DBValue.IsNull) ? Convert.ToInt32(tsePrestador.DBValue.ToString()) : -1;
                    condutorId = (this.tseCondutor.IsValidDBValue && !this.tseCondutor.DBValue.IsNull) ? Convert.ToInt32(tseCondutor["condutorid"]) : -1;
                    veiculoId = (this.tseVeiculo.IsValidDBValue && !this.tseVeiculo.DBValue.IsNull) ? Convert.ToInt32(tseVeiculo["veiculoid"]) : -1;

                    validacao = rnVeiculo.ValidaPrestadorCondutorVeiculo(prestadorId, condutorId, veiculoId, User.Identity.Name);


                    if (validacao.Valido)
                    {
                        rnVeiculo.InserePrestadorCondutorVeiculo(prestadorId, condutorId, veiculoId, User.Identity.Name);
                        lblMensagem.Text = "Associação realizada com sucesso.";

                        _tipoOperacao = TipoOperacao.Sucesso;
                        ControlarTipoOperacao();

                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup",
                          "alert('Associação realizada com sucesso.');", true);

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
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tsePrestador_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (!this.tsePrestador.DBValue.IsNull)
                {
                    if (!this.tsePrestador.IsValidDBValue)
                    {
                        this._tipoOperacao = TipoOperacao.Inicial;
                        this.lblMensagem.Text = "Prestador não cadastrado.";
                    }
                    else
                    {
                        this._tipoOperacao = TipoOperacao.Consultar;
                    }
                }
                else
                {
                    this._tipoOperacao = TipoOperacao.Inicial;
                    this.lblMensagem.Text = "Favor consultar um prestador.";
                }

                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseCondutor_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                tseVeiculo.ResetValue();

                if (!this.tseCondutor.DBValue.IsNull)
                {
                    if (!this.tseCondutor.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Condutor não cadastrado.";
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar um condutor.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseVeiculo_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (!this.tseVeiculo.DBValue.IsNull)
                {
                    if (!this.tseVeiculo.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Veículo não cadastrado.";
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar um veículo.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdAssociacoes_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAssociacoes);
            ControlaAcessoGrid();
        }
        public void Delete(object CompositeKey)
        {
        }
        protected void grdAssociacoes_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                var prestadorId = Convert.ToString(e.GetListSourceFieldValue("PRESTADORID"));
                var veiculoId = Convert.ToString(e.GetListSourceFieldValue("VEICULOID"));
                var condutorId = Convert.ToString(e.GetListSourceFieldValue("CONDUTORID"));
                e.Value = prestadorId + ";" + veiculoId + ";" + condutorId;
            }
        }
        protected void grdAssociacoes_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            int prestadorId, veiculoId, condutorId = 0;
            RN.Transporte.Veiculo rnVeiculo = new Techne.Lyceum.RN.Transporte.Veiculo();
            string[] chaves = e.Keys["CompositeKey"].ToString().Split(';');

            prestadorId = int.Parse(chaves[0]);
            veiculoId = int.Parse(chaves[1]);
            condutorId = int.Parse(chaves[2]);

            validacao = rnVeiculo.ValidaRemocaoPrestadorCondutorVeiculo(prestadorId, condutorId, veiculoId, User.Identity.Name);

            if (validacao.Valido)
            {
                rnVeiculo.RemovePrestadorCondutorVeiculo(prestadorId, condutorId, veiculoId, User.Identity.Name);
                e.Cancel = true;
                this.grdAssociacoes.CancelEdit();
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void grdAssociacoes_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                int prestadorId, veiculoId, condutorId = 0;
                RN.Transporte.Veiculo rnVeiculo = new Techne.Lyceum.RN.Transporte.Veiculo();

                if (e.ButtonID == "btnExcluir")
                {
                    string[] chaves = grdAssociacoes.GetRowValues(e.VisibleIndex, "CompositeKey").ToString().Split(';');
                    prestadorId = int.Parse(chaves[0]);
                    veiculoId = int.Parse(chaves[1]);
                    condutorId = int.Parse(chaves[2]);

                    validacao = rnVeiculo.ValidaRemocaoPrestadorCondutorVeiculo(prestadorId, condutorId, veiculoId, User.Identity.Name);

                    if (validacao.Valido)
                    {
                        rnVeiculo.RemovePrestadorCondutorVeiculo(prestadorId, condutorId, veiculoId, User.Identity.Name);

                        odsAssociacoes.Select();
                        odsAssociacoes.DataBind();
                        grdAssociacoes.DataBind();
                        lblMensagem.Text = string.Empty;
                    }
                    else
                    {
                        throw new Exception(validacao.Mensagem);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ControlaAcessoGrid()
        {
            foreach (GridViewColumn col in grdAssociacoes.Columns)
            {
                if (col is GridViewCommandColumn)
                {
                    if (((GridViewCommandColumn)col).CustomButtons["btnExcluir"] != null)
                        ((GridViewCommandColumn)col).CustomButtons["btnExcluir"].Visibility =
                            Permission.AllowDelete ? GridViewCustomButtonVisibility.AllDataRows : GridViewCustomButtonVisibility.Invisible;
                }
            }

        }
    }
}
