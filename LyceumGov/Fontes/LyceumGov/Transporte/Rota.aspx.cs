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
    [NavUrl("~/Transporte/Rota.aspx")]
    [ControlText("Rota de Transporte")]
    [Title("Rota de Transporte")]
    public partial class Rota : TPage
    {
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        public object ListarRotaAlunoVolta(object trajetoVolta)
        {
            RN.Transporte.RotaAluno rnRotaAluno = new Techne.Lyceum.RN.Transporte.RotaAluno();

            var volta = trajetoVolta != null ? trajetoVolta.ToString() : null;

            if (!string.IsNullOrEmpty(volta))
            {
                return rnRotaAluno.ListaPor(Convert.ToInt32(trajetoVolta));
            }
            return null;
        }

        public object ListarRotaAlunoIda(object trajetoIda)
        {
            RN.Transporte.RotaAluno rnRotaAluno = new Techne.Lyceum.RN.Transporte.RotaAluno();

            var ida = trajetoIda != null ? trajetoIda.ToString() : null;

            if (!string.IsNullOrEmpty(ida))
            {
                return rnRotaAluno.ListaPor(Convert.ToInt32(trajetoIda));
            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (_tipoOperacao.Equals(TipoOperacao.Novo))
                {
                    tseUnidadeFiltro.Mode = ControlMode.View;
                    tseUnidadeFiltro.Enabled = false;
                    tseRota.Mode = ControlMode.View;
                    tseRota.Enabled = false;
                }

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

            TituloGrid(grdPontoEmbarqueIda, "Ida");
            TituloGrid(grdPontoEmbarqueVolta, "Volta");
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnExcluir, AcaoControle.excluir);
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(btnIncluirAssociacao, AcaoControle.editar);
            ControlaAcesso(btnNovaAssociacaoAluno, AcaoControle.novo);
            ControlaAcesso(btnIncluirEmbarque, AcaoControle.editar);

            ControlarTSearchs();
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

        public enum TipoOperacao
        {
            Novo,
            Cancelar,
            Consultar,
            Inicial,
            Alterar,
            Excluir,
            Sucesso,
            NovaAssociacao,
            CancelarAssociacao,
            SucessoAssociacao,
            NovoEmbarque,
            CancelarEmbarque,
            AlterarEmbarque,
            SucessoEmbarque
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

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                var sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (sessao != null)
                {
                    if (!this.tseRegional.DBValue.IsNull)
                    {
                        if (this.tseRegional.IsValidDBValue)
                        {
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

                if (!this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (this.tseUnidadeResponsavel.IsValidDBValue)
                    {
                        //Buscar região financeira e cgc
                        lblRegiaoFinanceira.Text = Convert.ToString(this.tseUnidadeResponsavel["regiaofinanceira"]);
                        lblCnpj.Text = Convert.ToString(this.tseUnidadeResponsavel["cgc"]);

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
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ControlarTipoOperacao()
        {
            RN.Transporte.Rota rnRota = new Techne.Lyceum.RN.Transporte.Rota();

            switch (this._tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        tseRota.ResetValue();
                        pcRota.TabPages[1].Enabled = false;
                        pcRota.TabPages[2].Enabled = false;
                        pnAbas.Visible = false;
                        tseRota.Mode = Techne.Controls.ControlMode.Edit;
                        pnlNovaAssociacao.Visible = false;
                        pnlNovoEmbarque.Visible = false;
                        LimpaDados();
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, btnExcluir, btnDesabilitar };
                        ControlarVisibilidadeControle(controles);
                        
                        //Verifica se rota não esta ativa
                        if (!this.tseRota.IsValidDBValue || this.tseRota.DBValue.IsNull || !rnRota.EhAtivaPor(Convert.ToInt32(this.tseRota["rotaid"])))
                        {
                            btnDesabilitar.Visible = false;
                        }

                        pnAbas.Visible = true;
                        tseRota.Mode = ControlMode.Edit;
                        tseRota.Enabled = true;
                        tseUnidadeFiltro.Mode = ControlMode.Edit;
                        tseUnidadeFiltro.Enabled = true;
                        pnlDados.Visible = true;
                        pcRota.TabPages[1].Enabled = true;
                        pcRota.TabPages[2].Enabled = true;
                        pnlPrestadorIda.Visible = true;
                        pnlPrestadorVolta.Visible = true;
                        pnlPrimeiroEmbarqueIda.Visible = false;
                        pnlPrimeiroEmbarqueVolta.Visible = false;
                        tseAluno.ResetValue();

                        this.odsPontoEmbarqueIda.Select();
                        this.odsPontoEmbarqueIda.DataBind();
                        this.grdPontoEmbarqueIda.DataBind();
                        this.odsPontoEmbarqueVolta.Select();
                        this.odsPontoEmbarqueVolta.DataBind();
                        this.grdPontoEmbarqueVolta.DataBind();
                        this.odsRotaAlunoIda.Select();
                        this.odsRotaAlunoIda.DataBind();
                        this.grdRotaAlunoIda.DataBind();
                        this.odsRotaAlunoVolta.Select();
                        this.odsRotaAlunoVolta.DataBind();
                        this.grdRotaAlunoVolta.DataBind();

                        grdRotaAlunoVolta.Columns[0].Visible = false;
                        grdRotaAlunoIda.Columns[0].Visible = false;
                        grdPontoEmbarqueVolta.Columns[0].Visible = false;
                        grdPontoEmbarqueIda.Columns[0].Visible = false;

                        DesabilitaCampos();
                        break;
                    }
                case TipoOperacao.Novo:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnIncluir };
                        ControlarVisibilidadeControle(controles);
                        tseRota.ResetValue();
                        tseRota.Enabled = false;
                        tseRota.Mode = ControlMode.View;
                        tseUnidadeFiltro.ResetValue();
                        tseUnidadeFiltro.Enabled = false;
                        tseUnidadeFiltro.Mode = ControlMode.View;
                        pnAbas.Visible = true;
                        pnlDados.Visible = true;
                        chkAtivo.Checked = true;
                        chkAtivo.Enabled = false;
                        pnlPrestadorIda.Visible = false;
                        pnlPrestadorVolta.Visible = false;
                        pnlPrimeiroEmbarqueIda.Visible = true;
                        pnlPrimeiroEmbarqueVolta.Visible = true;
                        CarregaTurno();
                        CarregaTipoCalculoPagamento();
                        CarregaTipoContratacao();
                        LimpaDados();
                        lblSituacao.Text = "Aguardando Aprovação";
                        pcRota.ActiveTabIndex = 0;
                        pcRota.TabPages[1].Enabled = false;
                        pcRota.TabPages[2].Enabled = false;
                        this.HabilitaCampos();
                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, btnExcluir, btnDesabilitar };
                        ControlarVisibilidadeControle(controles);

                        //Verifica se rota não esta ativa
                        if (!this.tseRota.IsValidDBValue || this.tseRota.DBValue.IsNull || !rnRota.EhAtivaPor(Convert.ToInt32(this.tseRota["rotaid"])))
                        {
                            btnDesabilitar.Visible = false;
                        }

                        pcRota.ActiveTabIndex = 0;
                        pnlDados.Visible = true;
                        pnlPrestadorIda.Visible = true;
                        pnlPrestadorVolta.Visible = true;
                        pnlPrimeiroEmbarqueIda.Visible = false;
                        pnlPrimeiroEmbarqueVolta.Visible = false;
                        pcRota.TabPages[1].Enabled = true;
                        pcRota.TabPages[2].Enabled = true;
                        CarregaTurno();
                        CarregaTipoCalculoPagamento();
                        CarregaTipoContratacao();
                        LimpaDados();
                        pnAbas.Visible = true;
                        tseUnidadeFiltro.Enabled = true;
                        tseRota.Enabled = true;
                        pnlNovaAssociacao.Visible = false;
                        pnlNovoEmbarque.Visible = false;
                        tseUnidadeResponsavel.Enabled = false;
                        tseUnidadeResponsavel.Mode = ControlMode.View;
                        hdnPrimeiroCodMunicipioIda.Value = string.Empty;
                        hdnPrimeiroCodMunicipioVolta.Value = string.Empty;
                        CarregaDadosRota();

                        grdPontoEmbarqueVolta.Columns[0].Visible = false;
                        grdPontoEmbarqueIda.Columns[0].Visible = false;
                        grdRotaAlunoVolta.Columns[0].Visible = false;
                        grdRotaAlunoIda.Columns[0].Visible = false;

                        DesabilitaCampos();
                        break;
                    }
                case TipoOperacao.Cancelar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        pnlDados.Visible = false;
                        pnAbas.Visible = false;
                        tseUnidadeFiltro.ResetValue();
                        tseUnidadeFiltro.Mode = ControlMode.Edit;
                        tseUnidadeFiltro.ReadOnly = false;
                        tseUnidadeFiltro.Enabled = true;
                        tseRota.ResetValue();
                        tseRota.Mode = ControlMode.Edit;
                        tseRota.ReadOnly = false;
                        tseRota.Enabled = true;

                        grdPontoEmbarqueIda.DataSource = null;
                        grdPontoEmbarqueIda.DataBind();
                        grdPontoEmbarqueIda.CancelEdit();
                        grdPontoEmbarqueVolta.DataSource = null;
                        grdPontoEmbarqueVolta.DataBind();
                        grdPontoEmbarqueVolta.CancelEdit();
                        grdRotaAlunoIda.DataSource = null;
                        grdRotaAlunoIda.DataBind();
                        grdRotaAlunoIda.CancelEdit();
                        grdRotaAlunoVolta.DataSource = null;
                        grdRotaAlunoVolta.DataBind();
                        grdRotaAlunoVolta.CancelEdit();
                        pnlNovaAssociacao.Visible = false;
                        LimparDadosAssociacao();
                        pnlNovoEmbarque.Visible = false;
                        LimpaDadosPontoEmbarque();
                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        tseRota.Enabled = false;
                        HabilitaCampos();
                        pcRota.ActiveTabIndex = 0;

                        ImageButton[] controles = new ImageButton[] { btnCancel, btnIncluir, btnNovaAssociacaoAluno, btnNovoEmbarque };
                        ControlarVisibilidadeControle(controles);

                        pnlNovaAssociacao.Visible = false;
                        LimparDadosAssociacao();
                        pnlDadosNovaAssociacao.Visible = false;
                        pnlNovoEmbarque.Visible = false;
                        LimpaDadosPontoEmbarque();
                        pnlDadosEmbarque.Visible = false;
                        ddlTurno.Enabled = false;
                        ddlTipoCalculoPagamento.Enabled = false;

                        grdPontoEmbarqueVolta.Columns[0].Visible = true;
                        grdPontoEmbarqueIda.Columns[0].Visible = true;
                        grdRotaAlunoVolta.Columns[0].Visible = true;
                        grdRotaAlunoIda.Columns[0].Visible = true;

                        break;
                    }
                case TipoOperacao.Excluir:
                    {
                        ValidacaoDados validacao = new ValidacaoDados();
                        if (this.tseRota.IsValidDBValue && !this.tseRota.DBValue.IsNull)
                        {
                            validacao = rnRota.ValidaRemocao(Convert.ToInt32(this.tseRota["rotaid"]));

                            if (validacao.Valido)
                            {
                                rnRota.Remove(Convert.ToInt32(this.tseRota["rotaid"]));
                                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Rota excluída com sucesso.');", true);

                                _tipoOperacao = TipoOperacao.Inicial;
                                ControlarTipoOperacao();
                            }
                            else
                            {
                                lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                            }
                        }
                        break;
                    }
                case TipoOperacao.NovaAssociacao:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancelAssociacaoAluno, btnIncluirAssociacao };
                        ControlarVisibilidadeControle(controles);

                        rblTrajetoAssociacao.ClearSelection();
                        pnlNovaAssociacao.Visible = true;
                        btnIncluirAssociacao.Visible = true;
                        btnCancelAssociacaoAluno.Visible = true;
                        DesabilitaCamposDadosGerais();


                        break;
                    }
                case TipoOperacao.CancelarAssociacao:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnIncluir, btnNovaAssociacaoAluno };
                        ControlarVisibilidadeControle(controles);
                        LimparDadosAssociacao();
                        rblTrajetoAssociacao.ClearSelection();
                        pnlDadosNovaAssociacao.Visible = false;
                        pnlNovaAssociacao.Visible = false;
                        HabilitaCampos();
                        break;
                    }
                case TipoOperacao.SucessoAssociacao:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnIncluir, btnNovaAssociacaoAluno };
                        ControlarVisibilidadeControle(controles);
                        grdRotaAlunoIda.DataBind();
                        grdRotaAlunoVolta.DataBind();
                        LimparDadosAssociacao();
                        rblTrajetoAssociacao.ClearSelection();
                        pnlDadosNovaAssociacao.Visible = false;
                        pnlNovaAssociacao.Visible = false;
                        HabilitaCampos();
                        break;
                    }
                case TipoOperacao.NovoEmbarque:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancelaEmbarque, btnIncluirEmbarque };
                        ControlarVisibilidadeControle(controles);

                        rblTrajetoEmbarque.ClearSelection();
                        pnlNovoEmbarque.Visible = true;
                        DesabilitaCamposDadosGerais();
                        rblTrajetoEmbarque.Enabled = true;

                        break;
                    }
                case TipoOperacao.CancelarEmbarque:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnIncluir, btnNovoEmbarque };
                        ControlarVisibilidadeControle(controles);
                        LimpaDadosPontoEmbarque();
                        rblTrajetoEmbarque.ClearSelection();
                        pnlDadosEmbarque.Visible = false;
                        pnlNovoEmbarque.Visible = false;
                        HabilitaCampos();
                        break;
                    }
                case TipoOperacao.SucessoEmbarque:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnIncluir, btnNovoEmbarque };
                        ControlarVisibilidadeControle(controles);
                        grdPontoEmbarqueIda.DataBind();
                        grdPontoEmbarqueVolta.DataBind();
                        LimpaDadosPontoEmbarque();
                        rblTrajetoEmbarque.ClearSelection();
                        pnlDadosEmbarque.Visible = false;
                        pnlNovoEmbarque.Visible = false;
                        HabilitaCampos();
                        break;
                    }
                case TipoOperacao.AlterarEmbarque:
                    {
                        ImageButton[] controles = new ImageButton[] { btnIncluirEmbarque, btnCancelaEmbarque };
                        ControlarVisibilidadeControle(controles);
                        rblTrajetoEmbarque.Enabled = false;

                        break;
                    }
            }
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
                        tseRegional.Mode = ControlMode.View;
                        tseRegional.Enabled = false;
                        tseMunicipio.Mode = ControlMode.View;
                        tseMunicipio.Enabled = false;
                        tseUnidadeResponsavel.Mode = ControlMode.View;
                        tseUnidadeResponsavel.Enabled = false;
                        break;
                    }
                case TipoOperacao.Novo:
                    {
                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        tsePrestadorIda.Mode = ControlMode.View;
                        tsePrestadorIda.Enabled = false;
                        tseCondutorIda.Mode = ControlMode.View;
                        tseCondutorIda.Enabled = false;
                        tseVeiculoIda.Mode = ControlMode.View;
                        tseVeiculoIda.Enabled = false;

                        tsePrestadorVolta.Mode = ControlMode.View;
                        tsePrestadorVolta.Enabled = false;
                        tseCondutorVolta.Mode = ControlMode.View;
                        tseCondutorVolta.Enabled = false;
                        tseVeiculoVolta.Mode = ControlMode.View;
                        tseVeiculoVolta.Enabled = false;

                        tseRegional.Mode = ControlMode.View;
                        tseRegional.Enabled = false;
                        tseMunicipio.Mode = ControlMode.View;
                        tseMunicipio.Enabled = false;
                        tseUnidadeResponsavel.Mode = ControlMode.View;
                        tseUnidadeResponsavel.Enabled = false;
                        break;
                    }
                case TipoOperacao.Cancelar:
                    {


                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        tsePrestadorIda.Mode = ControlMode.Edit;
                        tsePrestadorIda.Enabled = true;
                        tseCondutorIda.Mode = ControlMode.Edit;
                        tseCondutorIda.Enabled = true;
                        tseVeiculoIda.Mode = ControlMode.Edit;
                        tseVeiculoIda.Enabled = true;

                        tsePrestadorVolta.Mode = ControlMode.Edit;
                        tsePrestadorVolta.Enabled = true;
                        tseCondutorVolta.Mode = ControlMode.Edit;
                        tseCondutorVolta.Enabled = true;
                        tseVeiculoVolta.Mode = ControlMode.Edit;
                        tseVeiculoVolta.Enabled = true;

                        tseRegional.Mode = ControlMode.View;
                        tseRegional.Enabled = false;
                        tseMunicipio.Mode = ControlMode.View;
                        tseMunicipio.Enabled = false;
                        tseUnidadeResponsavel.Mode = ControlMode.View;
                        tseUnidadeResponsavel.Enabled = false;

                        tseAluno.Enabled = true;
                        break;
                    }
                case TipoOperacao.Excluir:
                    {
                        tseRegional.Mode = ControlMode.View;
                        tseRegional.Enabled = false;
                        tseMunicipio.Mode = ControlMode.View;
                        tseMunicipio.Enabled = false;
                        tseUnidadeResponsavel.Mode = ControlMode.View;
                        tseUnidadeResponsavel.Enabled = false;
                        break;
                    }
                case TipoOperacao.NovaAssociacao:
                    {
                        tsePrestadorIda.Mode = ControlMode.View;
                        tsePrestadorIda.Enabled = false;
                        tseCondutorIda.Mode = ControlMode.View;
                        tseCondutorIda.Enabled = false;
                        tseVeiculoIda.Mode = ControlMode.View;
                        tseVeiculoIda.Enabled = false;

                        tsePrestadorVolta.Mode = ControlMode.View;
                        tsePrestadorVolta.Enabled = false;
                        tseCondutorVolta.Mode = ControlMode.View;
                        tseCondutorVolta.Enabled = false;
                        tseVeiculoVolta.Mode = ControlMode.View;
                        tseVeiculoVolta.Enabled = false;

                        tseRegional.Mode = ControlMode.View;
                        tseRegional.Enabled = false;
                        tseMunicipio.Mode = ControlMode.View;
                        tseMunicipio.Enabled = false;
                        tseUnidadeResponsavel.Mode = ControlMode.View;
                        tseUnidadeResponsavel.Enabled = false;
                        break;
                    }
                case TipoOperacao.CancelarAssociacao:
                    {
                        tsePrestadorIda.Mode = ControlMode.Edit;
                        tsePrestadorIda.Enabled = true;
                        tseCondutorIda.Mode = ControlMode.Edit;
                        tseCondutorIda.Enabled = true;
                        tseVeiculoIda.Mode = ControlMode.Edit;
                        tseVeiculoIda.Enabled = true;

                        tsePrestadorVolta.Mode = ControlMode.Edit;
                        tsePrestadorVolta.Enabled = true;
                        tseCondutorVolta.Mode = ControlMode.Edit;
                        tseCondutorVolta.Enabled = true;
                        tseVeiculoVolta.Mode = ControlMode.Edit;
                        tseVeiculoVolta.Enabled = true;

                        tseRegional.Mode = ControlMode.View;
                        tseRegional.Enabled = false;
                        tseMunicipio.Mode = ControlMode.View;
                        tseMunicipio.Enabled = false;
                        tseUnidadeResponsavel.Mode = ControlMode.View;
                        tseUnidadeResponsavel.Enabled = false;
                        break;
                    }
                case TipoOperacao.SucessoAssociacao:
                    {
                        tsePrestadorIda.Mode = ControlMode.Edit;
                        tsePrestadorIda.Enabled = true;
                        tseCondutorIda.Mode = ControlMode.Edit;
                        tseCondutorIda.Enabled = true;
                        tseVeiculoIda.Mode = ControlMode.Edit;
                        tseVeiculoIda.Enabled = true;

                        tsePrestadorVolta.Mode = ControlMode.Edit;
                        tsePrestadorVolta.Enabled = true;
                        tseCondutorVolta.Mode = ControlMode.Edit;
                        tseCondutorVolta.Enabled = true;
                        tseVeiculoVolta.Mode = ControlMode.Edit;
                        tseVeiculoVolta.Enabled = true;

                        tseRegional.Mode = ControlMode.View;
                        tseRegional.Enabled = false;
                        tseMunicipio.Mode = ControlMode.View;
                        tseMunicipio.Enabled = false;
                        tseUnidadeResponsavel.Mode = ControlMode.View;
                        tseUnidadeResponsavel.Enabled = false;
                        break;
                    }
                case TipoOperacao.NovoEmbarque:
                    {
                        tsePrestadorIda.Mode = ControlMode.View;
                        tsePrestadorIda.Enabled = false;
                        tseCondutorIda.Mode = ControlMode.View;
                        tseCondutorIda.Enabled = false;
                        tseVeiculoIda.Mode = ControlMode.View;
                        tseVeiculoIda.Enabled = false;

                        tsePrestadorVolta.Mode = ControlMode.View;
                        tsePrestadorVolta.Enabled = false;
                        tseCondutorVolta.Mode = ControlMode.View;
                        tseCondutorVolta.Enabled = false;
                        tseVeiculoVolta.Mode = ControlMode.View;
                        tseVeiculoVolta.Enabled = false;

                        tseRegional.Mode = ControlMode.View;
                        tseRegional.Enabled = false;
                        tseMunicipio.Mode = ControlMode.View;
                        tseMunicipio.Enabled = false;
                        tseUnidadeResponsavel.Mode = ControlMode.View;
                        tseUnidadeResponsavel.Enabled = false;
                        break;
                    }
                case TipoOperacao.CancelarEmbarque:
                    {
                        tsePrestadorIda.Mode = ControlMode.Edit;
                        tsePrestadorIda.Enabled = true;
                        tseCondutorIda.Mode = ControlMode.Edit;
                        tseCondutorIda.Enabled = true;
                        tseVeiculoIda.Mode = ControlMode.Edit;
                        tseVeiculoIda.Enabled = true;

                        tsePrestadorVolta.Mode = ControlMode.Edit;
                        tsePrestadorVolta.Enabled = true;
                        tseCondutorVolta.Mode = ControlMode.Edit;
                        tseCondutorVolta.Enabled = true;
                        tseVeiculoVolta.Mode = ControlMode.Edit;
                        tseVeiculoVolta.Enabled = true;

                        tseRegional.Mode = ControlMode.View;
                        tseRegional.Enabled = false;
                        tseMunicipio.Mode = ControlMode.View;
                        tseMunicipio.Enabled = false;
                        tseUnidadeResponsavel.Mode = ControlMode.View;
                        tseUnidadeResponsavel.Enabled = false;
                        break;
                    }
                case TipoOperacao.SucessoEmbarque:
                    {
                        tsePrestadorIda.Mode = ControlMode.Edit;
                        tsePrestadorIda.Enabled = true;
                        tseCondutorIda.Mode = ControlMode.Edit;
                        tseCondutorIda.Enabled = true;
                        tseVeiculoIda.Mode = ControlMode.Edit;
                        tseVeiculoIda.Enabled = true;

                        tsePrestadorVolta.Mode = ControlMode.Edit;
                        tsePrestadorVolta.Enabled = true;
                        tseCondutorVolta.Mode = ControlMode.Edit;
                        tseCondutorVolta.Enabled = true;
                        tseVeiculoVolta.Mode = ControlMode.Edit;
                        tseVeiculoVolta.Enabled = true;

                        tseRegional.Mode = ControlMode.View;
                        tseRegional.Enabled = false;
                        tseMunicipio.Mode = ControlMode.View;
                        tseMunicipio.Enabled = false;
                        tseUnidadeResponsavel.Mode = ControlMode.View;
                        tseUnidadeResponsavel.Enabled = false;
                        break;
                    }
            }
        }

        private void CarregaDadosRota()
        {
            RN.Transporte.Rota rnRota = new Techne.Lyceum.RN.Transporte.Rota();
            RN.Transporte.Condutor rnCondutor = new Techne.Lyceum.RN.Transporte.Condutor();
            RN.Transporte.Veiculo rnVeiculo = new Techne.Lyceum.RN.Transporte.Veiculo();
            RN.Transporte.Entidades.Condutor condutorIda = new Techne.Lyceum.RN.Transporte.Entidades.Condutor();
            RN.Transporte.Entidades.Condutor condutorVolta = new Techne.Lyceum.RN.Transporte.Entidades.Condutor();
            RN.Transporte.Entidades.Veiculo veiculoIda = new Techne.Lyceum.RN.Transporte.Entidades.Veiculo();
            RN.Transporte.Entidades.Veiculo veiculoVolta = new Techne.Lyceum.RN.Transporte.Entidades.Veiculo();
            DadosRota dadosRota = new DadosRota();
            int rotaId = 0;

            try
            {
                //Busca id selecionado
                rotaId = (this.tseRota.IsValidDBValue && !this.tseRota.DBValue.IsNull) ? Convert.ToInt32(this.tseRota["rotaid"]) : -1;

                //Verifica se é novo ou alteração
                if (rotaId > 0)
                {
                    //Busca dados da rota
                    dadosRota = rnRota.ObtemDadosRotaPor(rotaId);

                    tseUnidadeResponsavel.DBValue = dadosRota.Censo;
                    tseUnidadeResponsavel_Changed(null, null);
                    lblCodigo.Text = dadosRota.Codigo;
                    lblSituacao.Text = dadosRota.Situacao;
                    ddlTipoCalculoPagamento.SelectedValue = Convert.ToString(dadosRota.TipoCalculoPagamentoId);
                    ddlTurno.SelectedValue = dadosRota.Turno;
                    chkAtivo.Checked = dadosRota.Ativo;
                    if (chkAtivo.Checked)
                    {
                        chkAtivo.Enabled = false;
                    }

                    hdnRotaTrajetoIdIda.Value = Convert.ToString(dadosRota.RotaTrajetoIdIda);
                    ddlTipoContratacaoIda.SelectedValue = Convert.ToString(dadosRota.TipoContratacaoIdIda);
                    ddlTipoContratacaoIda_SelectedIndexChanged(null, null);
                    txtValorRotaIda.Text = Convert.ToString(dadosRota.ValorRotaIda);
                    txtQuantidadeKmIda.Text = Convert.ToString(dadosRota.QuantidadeKmIda);
                    txtTempoIda.Text = dadosRota.TempoIda != 0 ? Convert.ToString(dadosRota.TempoIda) : string.Empty;
                    if (dadosRota.PrestadorIdIda > 0)
                    {
                        tsePrestadorIda.DBValue = Convert.ToString(dadosRota.PrestadorIdIda);

                        //Busca dados condutor e veiculo
                        condutorIda = rnCondutor.ObtemPor(dadosRota.CondutorIdIda);
                        veiculoIda = rnVeiculo.ObtemPor(dadosRota.VeiculoIdIda);
                        tseCondutorIda.DBValue = condutorIda.Cpf;
                        tseVeiculoIda.DBValue = veiculoIda.Placa;
                    }

                    hdnRotaTrajetoIdVolta.Value = Convert.ToString(dadosRota.RotaTrajetoIdVolta);
                    ddlTipoContratacaoVolta.SelectedValue = Convert.ToString(dadosRota.TipoContratacaoIdVolta);
                    ddlTipoContratacaoVolta_SelectedIndexChanged(null, null);
                    txtValorRotaVolta.Text = Convert.ToString(dadosRota.ValorRotaVolta);
                    txtQuantidadeKmVolta.Text = Convert.ToString(dadosRota.QuantidadeKmVolta);
                    txtTempoVolta.Text = dadosRota.TempoVolta != 0 ? Convert.ToString(dadosRota.TempoVolta) : string.Empty;
                    if (dadosRota.PrestadorIdVolta > 0)
                    {
                        tsePrestadorVolta.DBValue = Convert.ToString(dadosRota.PrestadorIdVolta);

                        //Busca dados condutor e veiculo
                        condutorVolta = rnCondutor.ObtemPor(dadosRota.CondutorIdVolta);
                        veiculoVolta = rnVeiculo.ObtemPor(dadosRota.VeiculoIdVolta);
                        tseCondutorVolta.DBValue = condutorVolta.Cpf;
                        tseVeiculoVolta.DBValue = veiculoVolta.Placa;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public object ListarPontoEmbarqueIda(object rotaTrajetoIdIda)
        {
            RN.Transporte.PontoEmbarque rnPontoEmbarque = new Techne.Lyceum.RN.Transporte.PontoEmbarque();

            var ida = rotaTrajetoIdIda != null ? rotaTrajetoIdIda.ToString() : null;

            if (!string.IsNullOrEmpty(ida))
            {
                return rnPontoEmbarque.ListaPor(Convert.ToInt32(rotaTrajetoIdIda));
            }
            return null;
        }

        protected void btnNovoEmbarque_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.NovoEmbarque;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnIncluirEmbarque_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Transporte.Entidades.PontoEmbarque pontoEmbarque = new Techne.Lyceum.RN.Transporte.Entidades.PontoEmbarque();
                RN.Transporte.PontoEmbarque rnPontoEmbarque = new Techne.Lyceum.RN.Transporte.PontoEmbarque();
                string mensagem = string.Empty;

                pontoEmbarque.Cep = !txtCep.Text.IsNullOrEmptyOrWhiteSpace() ? txtCep.Text.Trim() : null;
                pontoEmbarque.Bairro = !txtBairro.Text.IsNullOrEmptyOrWhiteSpace() ? txtBairro.Text.Trim().ToUpper() : null;
                pontoEmbarque.Latitude = !txtLatitude.Text.IsNullOrEmptyOrWhiteSpace() ? txtLatitude.Text.Trim() : null;
                pontoEmbarque.Longitude = !txtLongitude.Text.IsNullOrEmptyOrWhiteSpace() ? txtLongitude.Text.Trim() : null;
                pontoEmbarque.Municipio = !hdnCodMunicipio.Value.IsNullOrEmptyOrWhiteSpace() ? hdnCodMunicipio.Value.Trim() : null;
                pontoEmbarque.Logradouro = !txtEndereco.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndereco.Text.Trim().ToUpper() : null;
                pontoEmbarque.Numero = !txtEndNum.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndNum.Text.Trim() : null;
                pontoEmbarque.UsuarioId = User.Identity.Name;
                pontoEmbarque.PontoEmbarqueId = hdnPontoEmbarque.Value.IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt32(hdnPontoEmbarque.Value);

                pontoEmbarque.Primeiro = chkPrimeiro.Checked;

                if (hdnPontoEmbarque.Value.IsNullOrEmptyOrWhiteSpace())
                {
                    if (!rblTrajetoEmbarque.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                    {
                        pontoEmbarque.RotaTrajetoId = rblTrajetoEmbarque.SelectedValue == "Ida" ? Convert.ToInt32(hdnRotaTrajetoIdIda.Value) : Convert.ToInt32(hdnRotaTrajetoIdVolta.Value);
                    }
                }
                else
                {
                    pontoEmbarque.RotaTrajetoId = Convert.ToInt32(hdnRotaTrajetoId.Value);
                }

                int rotaId = (this.tseRota.IsValidDBValue && !this.tseRota.DBValue.IsNull) ? Convert.ToInt32(tseRota["rotaId"]) : -1;

                validacao = rnPontoEmbarque.Valida(pontoEmbarque, rotaId, pontoEmbarque.PontoEmbarqueId == 0 ? true : false);

                if (validacao.Valido)
                {
                    if (pontoEmbarque.PontoEmbarqueId == 0)
                    {
                        rnPontoEmbarque.Insere(pontoEmbarque);
                        mensagem = "Ponto de embarque inserido com sucesso.";
                    }
                    else
                    {
                        rnPontoEmbarque.Atualiza(pontoEmbarque);
                        mensagem = "Ponto de embarque atualizado com sucesso.";
                    }

                    lblMensagem.Text = mensagem;

                    _tipoOperacao = TipoOperacao.SucessoEmbarque;
                    ControlarTipoOperacao();

                    var script = @"alert('" + lblMensagem.Text + @"');";
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCancelaEmbarque_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.CancelarEmbarque;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rblTrajetoEmbarque_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LimpaDadosPontoEmbarque();

                if (!rblTrajetoEmbarque.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    pnlDadosEmbarque.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public object ListarPontoEmbarqueVolta(object rotaTrajetoIdVolta)
        {
            RN.Transporte.PontoEmbarque rnPontoEmbarque = new Techne.Lyceum.RN.Transporte.PontoEmbarque();

            var volta = rotaTrajetoIdVolta != null ? rotaTrajetoIdVolta.ToString() : null;

            if (!string.IsNullOrEmpty(volta))
            {
                return rnPontoEmbarque.ListaPor(Convert.ToInt32(rotaTrajetoIdVolta));
            }
            return null;
        }

        protected void grdPontoEmbarqueIda_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdPontoEmbarqueIda.Settings.ShowFilterRow = false;
        }

        protected void grdPontoEmbarqueVolta_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdPontoEmbarqueVolta.Settings.ShowFilterRow = false;
        }

        protected void grdPontoEmbarqueIda_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdPontoEmbarqueIda.Settings.ShowFilterRow = false;
        }

        protected void grdPontoEmbarqueVolta_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdPontoEmbarqueVolta.Settings.ShowFilterRow = false;
        }

        protected void grdPontoEmbarqueIda_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdPontoEmbarqueIda);
        }

        protected void grdPontoEmbarqueVolta_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdPontoEmbarqueVolta);
        }

        protected void grdPontoEmbarqueIda_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.PontoEmbarque rnPontoEmbarque = new Techne.Lyceum.RN.Transporte.PontoEmbarque();
            int pontoEmbarqueId = 0;
            int rotaId = (this.tseRota.IsValidDBValue && !this.tseRota.DBValue.IsNull) ? Convert.ToInt32(tseRota["rotaId"]) : -1;

            var primeiro = Convert.ToBoolean(grdPontoEmbarqueIda.GetRowValuesByKeyValue(e.Keys[0], "PRIMEIRO"));

            pontoEmbarqueId = Convert.ToInt32(e.Keys["PONTOEMBARQUEID"]);

            validacao = rnPontoEmbarque.ValidaRemocao(pontoEmbarqueId, rotaId, primeiro);

            if (validacao.Valido)
            {
                rnPontoEmbarque.Remove(pontoEmbarqueId);
                grdPontoEmbarqueIda.DataBind();
            }
            else
            {
                e.Cancel = true;
                lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
            }
        }

        protected void grdPontoEmbarqueVolta_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.PontoEmbarque rnPontoEmbarque = new Techne.Lyceum.RN.Transporte.PontoEmbarque();
            int pontoEmbarqueId = 0;
            int rotaId = (this.tseRota.IsValidDBValue && !this.tseRota.DBValue.IsNull) ? Convert.ToInt32(tseRota["rotaId"]) : -1;

            var primeiro = Convert.ToBoolean(grdPontoEmbarqueVolta.GetRowValuesByKeyValue(e.Keys[0], "PRIMEIRO"));

            pontoEmbarqueId = Convert.ToInt32(e.Keys["PONTOEMBARQUEID"]);

            validacao = rnPontoEmbarque.ValidaRemocao(pontoEmbarqueId, rotaId, primeiro);

            if (validacao.Valido)
            {
                rnPontoEmbarque.Remove(pontoEmbarqueId);
                grdPontoEmbarqueVolta.DataBind();
            }
            else
            {
                e.Cancel = true;
                lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
            }
        }

        public void DeleteEmbarqueVolta(object PONTOEMBARQUEID)
        { }

        public void DeleteEmbarqueIda(object PONTOEMBARQUEID)
        { }

        private void LimpaDadosPontoEmbarque()
        {
            txtCep.Text = string.Empty;
            txtEstado.Value = string.Empty;
            txtLatitude.Text = string.Empty;
            txtLongitude.Text = string.Empty;
            txtMunicipio.Text = string.Empty;
            txtEndereco.Text = string.Empty;
            txtEndNum.Text = string.Empty;
            txtBairro.Text = string.Empty;
            hdnPontoEmbarque.Value = string.Empty;
            hdnRotaTrajetoId.Value = string.Empty;
            chkPrimeiro.Checked = false;

        }

        protected void grdPontoEmbarqueIda_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            if (e.ButtonID == "EditarIda")
            {
                pnlNovoEmbarque.Visible = true;
                pnlDadosEmbarque.Visible = true;
                LimpaDadosPontoEmbarque();

                string idPontoEmbarque = grdPontoEmbarqueIda.GetRowValues(e.VisibleIndex, "PONTOEMBARQUEID").ToString();
                string cep = grdPontoEmbarqueIda.GetRowValues(e.VisibleIndex, "CEP").ToString();
                string bairro = grdPontoEmbarqueIda.GetRowValues(e.VisibleIndex, "BAIRRO").ToString();
                string latitude = grdPontoEmbarqueIda.GetRowValues(e.VisibleIndex, "LATITUDE").ToString();
                string longitude = grdPontoEmbarqueIda.GetRowValues(e.VisibleIndex, "LONGITUDE").ToString();
                string municipio = grdPontoEmbarqueIda.GetRowValues(e.VisibleIndex, "MUNICIPIO").ToString();
                string logradouro = grdPontoEmbarqueIda.GetRowValues(e.VisibleIndex, "LOGRADOURO").ToString();
                string numero = grdPontoEmbarqueIda.GetRowValues(e.VisibleIndex, "NUMERO").ToString();
                string estado = grdPontoEmbarqueIda.GetRowValues(e.VisibleIndex, "ESTADO").ToString();
                string descMunicipio = grdPontoEmbarqueIda.GetRowValues(e.VisibleIndex, "DESCRICAOMUNICIPIO").ToString();
                string ida = grdPontoEmbarqueIda.GetRowValues(e.VisibleIndex, "IDA").ToString();
                string rotaTrajetoId = grdPontoEmbarqueIda.GetRowValues(e.VisibleIndex, "ROTATRAJETOID").ToString();
                string primeiro = grdPontoEmbarqueIda.GetRowValues(e.VisibleIndex, "PRIMEIRO").ToString();

                hdnPontoEmbarque.Value = idPontoEmbarque;
                hdnRotaTrajetoId.Value = rotaTrajetoId;
                txtCep.Text = cep;
                txtEndereco.Text = logradouro;
                txtEndNum.Text = numero;
                txtMunicipio.Text = descMunicipio;
                txtEstado.Value = estado;
                txtBairro.Text = bairro;
                hdnCodMunicipio.Value = municipio;
                txtLatitude.Text = latitude;
                txtLongitude.Text = longitude;
                rblTrajetoEmbarque.SelectedValue = Convert.ToBoolean(ida) == true ? "Ida" : "Volta";
                chkPrimeiro.Checked = Convert.ToBoolean(primeiro);

                _tipoOperacao = TipoOperacao.AlterarEmbarque;
                ControlarTipoOperacao();
            }
        }

        protected void grdPontoEmbarqueVolta_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            if (e.ButtonID == "EditarVolta")
            {
                pnlNovoEmbarque.Visible = true;
                pnlDadosEmbarque.Visible = true;
                LimpaDadosPontoEmbarque();

                string idPontoEmbarque = grdPontoEmbarqueVolta.GetRowValues(e.VisibleIndex, "PONTOEMBARQUEID").ToString();
                string cep = grdPontoEmbarqueVolta.GetRowValues(e.VisibleIndex, "CEP").ToString();
                string bairro = grdPontoEmbarqueVolta.GetRowValues(e.VisibleIndex, "BAIRRO").ToString();
                string latitude = grdPontoEmbarqueVolta.GetRowValues(e.VisibleIndex, "LATITUDE").ToString();
                string longitude = grdPontoEmbarqueVolta.GetRowValues(e.VisibleIndex, "LONGITUDE").ToString();
                string municipio = grdPontoEmbarqueVolta.GetRowValues(e.VisibleIndex, "MUNICIPIO").ToString();
                string logradouro = grdPontoEmbarqueVolta.GetRowValues(e.VisibleIndex, "LOGRADOURO").ToString();
                string numero = grdPontoEmbarqueVolta.GetRowValues(e.VisibleIndex, "NUMERO").ToString();
                string estado = grdPontoEmbarqueVolta.GetRowValues(e.VisibleIndex, "ESTADO").ToString();
                string descMunicipio = grdPontoEmbarqueVolta.GetRowValues(e.VisibleIndex, "DESCRICAOMUNICIPIO").ToString();
                string ida = grdPontoEmbarqueVolta.GetRowValues(e.VisibleIndex, "IDA").ToString();
                string rotaTrajetoId = grdPontoEmbarqueVolta.GetRowValues(e.VisibleIndex, "ROTATRAJETOID").ToString();
                string primeiro = grdPontoEmbarqueVolta.GetRowValues(e.VisibleIndex, "PRIMEIRO").ToString();


                hdnPontoEmbarque.Value = idPontoEmbarque;
                hdnRotaTrajetoId.Value = rotaTrajetoId;
                txtCep.Text = cep;
                txtEndereco.Text = logradouro;
                txtEndNum.Text = numero;
                txtMunicipio.Text = descMunicipio;
                txtEstado.Value = estado;
                txtBairro.Text = bairro;
                hdnCodMunicipio.Value = municipio;
                txtLatitude.Text = latitude;
                txtLongitude.Text = longitude;
                rblTrajetoEmbarque.SelectedValue = Convert.ToBoolean(ida) == true ? "Ida" : "Volta";
                chkPrimeiro.Checked = Convert.ToBoolean(primeiro);

                _tipoOperacao = TipoOperacao.AlterarEmbarque;
                ControlarTipoOperacao();
            }
        }

        private void LimpaDados()
        {
            tseRegional.ResetValue();
            tseMunicipio.ResetValue();
            tseUnidadeResponsavel.ResetValue();
            lblCnpj.Text = string.Empty;
            lblRegiaoFinanceira.Text = string.Empty;
            lblCodigo.Text = string.Empty;
            lblSituacao.Text = string.Empty;
            chkAtivo.Checked = true;
            chkAtivo.Enabled = false;
            ddlTipoCalculoPagamento.ClearSelection();
            ddlTurno.ClearSelection();

            ddlTipoContratacaoIda.ClearSelection();
            txtValorRotaIda.Text = string.Empty;
            txtQuantidadeKmIda.Text = string.Empty;
            txtTempoIda.Text = string.Empty;
            tsePrestadorIda.ResetValue();
            tseCondutorIda.ResetValue();
            tseVeiculoIda.ResetValue();
            txtPrimeiroCepIda.Text = string.Empty;
            txtPrimeiroEstadoIda.Value = string.Empty;
            hdnPrimeiroCodMunicipioIda.Value = string.Empty;
            txtPrimeiroMunicipioIda.Text = string.Empty;
            txtPrimeiroEnderecoIda.Text = string.Empty;
            txtPrimeiroEndNumIda.Text = string.Empty;
            txtPrimeiroBairroIda.Text = string.Empty;
            txtPrimeiraLatitudeIda.Text = string.Empty;
            txtPrimeiroLongitudeIda.Text = string.Empty;

            ddlTipoContratacaoVolta.ClearSelection();
            txtValorRotaVolta.Text = string.Empty;
            txtQuantidadeKmVolta.Text = string.Empty;
            txtTempoVolta.Text = string.Empty;
            tsePrestadorVolta.ResetValue();
            tseCondutorVolta.ResetValue();
            tseVeiculoVolta.ResetValue();
            txtPrimeiroCepVolta.Text = string.Empty;
            txtPrimeiroEstadoVolta.Value = string.Empty;
            hdnPrimeiroCodMunicipioVolta.Value = string.Empty;
            txtPrimeiroMunicipioVolta.Text = string.Empty;
            txtPrimeiroEnderecoVolta.Text = string.Empty;
            txtPrimeiroEndNumVolta.Text = string.Empty;
            txtPrimeiroBairroVolta.Text = string.Empty;
            txtPrimeiraLatitudeVolta.Text = string.Empty;
            txtPrimeiroLongitudeVolta.Text = string.Empty;

            txtCep.Text = string.Empty;
            txtEstado.Value = string.Empty;
            txtLatitude.Text = string.Empty;
            txtLongitude.Text = string.Empty;
            txtMunicipio.Text = string.Empty;
            txtEndereco.Text = string.Empty;
            txtEndNum.Text = string.Empty;
            txtBairro.Text = string.Empty;
            hdnPontoEmbarque.Value = string.Empty;
            hdnRotaTrajetoId.Value = string.Empty;

            tseAluno.ResetValue();
            dtDataInicioNova.Text = string.Empty;
            dtDataFimNova.Text = string.Empty;

        }

        private void DesabilitaCampos()
        {
            tseRegional.Mode = ControlMode.View;
            tseMunicipio.Mode = ControlMode.View;
            tseUnidadeResponsavel.Mode = ControlMode.View;
            chkAtivo.Enabled = false;
            ddlTipoCalculoPagamento.Enabled = false;
            ddlTurno.Enabled = false;

            ddlTipoContratacaoIda.Enabled = false;
            txtValorRotaIda.Enabled = false;
            txtQuantidadeKmIda.Enabled = false;
            txtTempoIda.Enabled = false;
            tsePrestadorIda.Mode = ControlMode.View;
            tseCondutorIda.Mode = ControlMode.View;
            tseVeiculoIda.Mode = ControlMode.View;
            txtPrimeiroCepIda.Enabled = false;
            txtPrimeiroMunicipioIda.Enabled = false;
            txtPrimeiroEnderecoIda.Enabled = false;
            txtPrimeiroEndNumIda.Enabled = false;
            txtPrimeiroBairroIda.Enabled = false;
            txtPrimeiraLatitudeIda.Enabled = false;
            txtPrimeiroLongitudeIda.Enabled = false;

            ddlTipoContratacaoVolta.Enabled = false;
            txtValorRotaVolta.Enabled = false;
            txtQuantidadeKmVolta.Enabled = false;
            txtTempoVolta.Enabled = false;
            tsePrestadorVolta.Mode = ControlMode.View;
            tseCondutorVolta.Mode = ControlMode.View;
            tseVeiculoVolta.Mode = ControlMode.View;
            txtPrimeiroCepVolta.Enabled = false;
            txtPrimeiroMunicipioVolta.Enabled = false;
            txtPrimeiroEnderecoVolta.Enabled = false;
            txtPrimeiroEndNumVolta.Enabled = false;
            txtPrimeiroBairroVolta.Enabled = false;
            txtPrimeiraLatitudeVolta.Enabled = false;
            txtPrimeiroLongitudeVolta.Enabled = false;

            txtCep.Enabled = false;
            txtLatitude.Enabled = false;
            txtLongitude.Enabled = false;
            txtMunicipio.Enabled = false;
            txtEndereco.Enabled = false;
            txtEndNum.Enabled = false;
            txtBairro.Enabled = false;

            tseAluno.Enabled = false;
            dtDataInicioNova.Enabled = false;
            dtDataFimNova.Enabled = false;
        }

        private void DesabilitaCamposDadosGerais()
        {

            chkAtivo.Enabled = false;
            ddlTipoCalculoPagamento.Enabled = false;
            ddlTurno.Enabled = false;

            ddlTipoContratacaoIda.Enabled = false;
            txtValorRotaIda.Enabled = false;
            txtQuantidadeKmIda.Enabled = false;
            txtTempoIda.Enabled = false;
            tsePrestadorIda.Mode = ControlMode.View;
            tseCondutorIda.Mode = ControlMode.View;
            tseVeiculoIda.Mode = ControlMode.View;
            txtPrimeiroCepIda.Enabled = false;
            txtPrimeiroMunicipioIda.Enabled = false;
            txtPrimeiroEnderecoIda.Enabled = false;
            txtPrimeiroEndNumIda.Enabled = false;
            txtPrimeiroBairroIda.Enabled = false;
            txtPrimeiraLatitudeIda.Enabled = false;
            txtPrimeiroLongitudeIda.Enabled = false;

            ddlTipoContratacaoVolta.Enabled = false;
            txtValorRotaVolta.Enabled = false;
            txtQuantidadeKmVolta.Enabled = false;
            txtTempoVolta.Enabled = false;
            tsePrestadorVolta.Mode = ControlMode.View;
            tseCondutorVolta.Mode = ControlMode.View;
            tseVeiculoVolta.Mode = ControlMode.View;
            txtPrimeiroCepVolta.Enabled = false;
            txtPrimeiroMunicipioVolta.Enabled = false;
            txtPrimeiroEnderecoVolta.Enabled = false;
            txtPrimeiroEndNumVolta.Enabled = false;
            txtPrimeiroBairroVolta.Enabled = false;
            txtPrimeiraLatitudeVolta.Enabled = false;
            txtPrimeiroLongitudeVolta.Enabled = false;
        }

        private void HabilitaCampos()
        {
            tseRegional.Mode = ControlMode.Edit;
            tseMunicipio.Mode = ControlMode.Edit;
            tseUnidadeResponsavel.Mode = ControlMode.Edit;

            if (chkAtivo.Checked)
            {
                chkAtivo.Enabled = false;
            }
            else
            {
                chkAtivo.Enabled = true;
            }

            ddlTipoCalculoPagamento.Enabled = true;
            ddlTurno.Enabled = true;

            ddlTipoContratacaoIda.Enabled = true;
            txtValorRotaIda.Enabled = true;
            txtQuantidadeKmIda.Enabled = true;
            txtTempoIda.Enabled = true;
            tsePrestadorIda.Mode = ControlMode.Edit;
            tseCondutorIda.Mode = ControlMode.Edit;
            tseVeiculoIda.Mode = ControlMode.Edit;
            txtPrimeiroCepIda.Enabled = true;
            txtPrimeiroMunicipioIda.Enabled = true;
            txtPrimeiroEnderecoIda.Enabled = true;
            txtPrimeiroEndNumIda.Enabled = true;
            txtPrimeiroBairroIda.Enabled = true;
            txtPrimeiraLatitudeIda.Enabled = true;
            txtPrimeiroLongitudeIda.Enabled = true;

            ddlTipoContratacaoVolta.Enabled = true;
            txtValorRotaVolta.Enabled = true;
            txtQuantidadeKmVolta.Enabled = true;
            txtTempoVolta.Enabled = true;
            tsePrestadorVolta.Mode = ControlMode.Edit;
            tseCondutorVolta.Mode = ControlMode.Edit;
            tseVeiculoVolta.Mode = ControlMode.Edit;
            txtPrimeiroCepVolta.Enabled = true;
            txtPrimeiroMunicipioVolta.Enabled = true;
            txtPrimeiroEnderecoVolta.Enabled = true;
            txtPrimeiroEndNumVolta.Enabled = true;
            txtPrimeiroBairroVolta.Enabled = true;
            txtPrimeiraLatitudeVolta.Enabled = true;
            txtPrimeiroLongitudeVolta.Enabled = true;

            txtCep.Enabled = true;
            txtLatitude.Enabled = true;
            txtLongitude.Enabled = true;
            txtMunicipio.Enabled = true;
            txtEndereco.Enabled = true;
            txtEndNum.Enabled = true;
            txtBairro.Enabled = true;

            tseAluno.Enabled = true;
            dtDataInicioNova.Enabled = true;
            dtDataFimNova.Enabled = true;
        }

        protected void btnIncluir_Click(object sender, ImageClickEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Rota rnRota = new Techne.Lyceum.RN.Transporte.Rota();
            DadosRotaCadastro dadosRotaCadastro = new DadosRotaCadastro();
            DadosRota dadosRotaAtualizacao = new DadosRota();
            int rotaId = 0;
            string mensagem = string.Empty;

            try
            {
                //Busca id selecionado
                rotaId = (this.tseRota.IsValidDBValue && !this.tseRota.DBValue.IsNull) ? Convert.ToInt32(this.tseRota["rotaid"]) : -1;

                //Verifica se é novo ou alteração
                if (rotaId <= 0)
                {
                    //Monta dto para cadastro
                    dadosRotaCadastro.Censo = (this.tseUnidadeResponsavel.IsValidDBValue && !this.tseUnidadeResponsavel.DBValue.IsNull) ? Convert.ToString(tseUnidadeResponsavel.DBValue.ToString()) : string.Empty;
                    dadosRotaCadastro.Turno = ddlTurno.SelectedValue;
                    dadosRotaCadastro.TipoCalculoPagamentoId = ddlTipoCalculoPagamento.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToInt32(ddlTipoCalculoPagamento.SelectedValue);
                    dadosRotaCadastro.UsuarioResponsavel = User.Identity.Name;
                    dadosRotaCadastro.Ativo = chkAtivo.Checked;

                    //Ida
                    dadosRotaCadastro.TipoContratacaoIdIda = ddlTipoContratacaoIda.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToInt32(ddlTipoContratacaoIda.SelectedValue);
                    dadosRotaCadastro.ValorRotaIda = txtValorRotaIda.Text.IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToDecimal(txtValorRotaIda.Text);
                    dadosRotaCadastro.QuantidadeKmIda = txtQuantidadeKmIda.Text.IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToDecimal(txtQuantidadeKmIda.Text);
                    dadosRotaCadastro.TempoIda = txtTempoIda.Text.IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(txtTempoIda.Text);
                    dadosRotaCadastro.PrimeiroCepIda = txtPrimeiroCepIda.Text.IsNullOrEmptyOrWhiteSpace() ? string.Empty : Convert.ToString(txtPrimeiroCepIda.Text);
                    dadosRotaCadastro.PrimeiroLogradouroIda = txtPrimeiroEnderecoIda.Text.IsNullOrEmptyOrWhiteSpace() ? string.Empty : Convert.ToString(txtPrimeiroEnderecoIda.Text).ToUpper();
                    dadosRotaCadastro.PrimeiroNumeroIda = txtPrimeiroEndNumIda.Text.IsNullOrEmptyOrWhiteSpace() ? string.Empty : Convert.ToString(txtPrimeiroEndNumIda.Text).ToUpper();
                    dadosRotaCadastro.PrimeiroBairroIda = txtPrimeiroBairroIda.Text.IsNullOrEmptyOrWhiteSpace() ? string.Empty : Convert.ToString(txtPrimeiroBairroIda.Text).ToUpper();
                    dadosRotaCadastro.PrimeiroMunicipioIda = hdnPrimeiroCodMunicipioIda.Value.IsNullOrEmptyOrWhiteSpace() ? string.Empty : Convert.ToString(hdnPrimeiroCodMunicipioIda.Value);
                    dadosRotaCadastro.PrimeiroLatitudeIda = txtPrimeiraLatitudeIda.Text.IsNullOrEmptyOrWhiteSpace() ? string.Empty : Convert.ToString(txtPrimeiraLatitudeIda.Text);
                    dadosRotaCadastro.PrimeiroLongitudeIda = txtPrimeiroLongitudeIda.Text.IsNullOrEmptyOrWhiteSpace() ? string.Empty : Convert.ToString(txtPrimeiroLongitudeIda.Text);

                    //Volta
                    dadosRotaCadastro.TipoContratacaoIdVolta = ddlTipoContratacaoVolta.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToInt32(ddlTipoContratacaoVolta.SelectedValue);
                    dadosRotaCadastro.ValorRotaVolta = txtValorRotaVolta.Text.IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToDecimal(txtValorRotaVolta.Text);
                    dadosRotaCadastro.QuantidadeKmVolta = txtQuantidadeKmVolta.Text.IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToDecimal(txtQuantidadeKmVolta.Text);
                    dadosRotaCadastro.TempoVolta = txtTempoVolta.Text.IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(txtTempoVolta.Text);
                    dadosRotaCadastro.PrimeiroCepVolta = txtPrimeiroCepVolta.Text.IsNullOrEmptyOrWhiteSpace() ? string.Empty : Convert.ToString(txtPrimeiroCepVolta.Text);
                    dadosRotaCadastro.PrimeiroLogradouroVolta = txtPrimeiroEnderecoVolta.Text.IsNullOrEmptyOrWhiteSpace() ? string.Empty : Convert.ToString(txtPrimeiroEnderecoVolta.Text).ToUpper();
                    dadosRotaCadastro.PrimeiroNumeroVolta = txtPrimeiroEndNumVolta.Text.IsNullOrEmptyOrWhiteSpace() ? string.Empty : Convert.ToString(txtPrimeiroEndNumVolta.Text).ToUpper();
                    dadosRotaCadastro.PrimeiroBairroVolta = txtPrimeiroBairroVolta.Text.IsNullOrEmptyOrWhiteSpace() ? string.Empty : Convert.ToString(txtPrimeiroBairroVolta.Text).ToUpper();
                    dadosRotaCadastro.PrimeiroMunicipioVolta = hdnPrimeiroCodMunicipioVolta.Value.IsNullOrEmptyOrWhiteSpace() ? string.Empty : Convert.ToString(hdnPrimeiroCodMunicipioVolta.Value);
                    dadosRotaCadastro.PrimeiroLatitudeVolta = txtPrimeiraLatitudeVolta.Text.IsNullOrEmptyOrWhiteSpace() ? string.Empty : Convert.ToString(txtPrimeiraLatitudeVolta.Text);
                    dadosRotaCadastro.PrimeiroLongitudeVolta = txtPrimeiroLongitudeVolta.Text.IsNullOrEmptyOrWhiteSpace() ? string.Empty : Convert.ToString(txtPrimeiroLongitudeVolta.Text);
                }
                else
                {
                    //Monta dto para alteração
                    dadosRotaAtualizacao.RotaId = rotaId;
                    dadosRotaAtualizacao.RotaTrajetoIdIda = Convert.ToInt32(hdnRotaTrajetoIdIda.Value);
                    dadosRotaAtualizacao.RotaTrajetoIdVolta = Convert.ToInt32(hdnRotaTrajetoIdVolta.Value);

                    //Monta dto para cadastro
                    dadosRotaAtualizacao.Censo = (this.tseUnidadeResponsavel.IsValidDBValue && !this.tseUnidadeResponsavel.DBValue.IsNull) ? Convert.ToString(tseUnidadeResponsavel.DBValue.ToString()) : string.Empty;
                    dadosRotaAtualizacao.Turno = ddlTurno.SelectedValue;
                    dadosRotaAtualizacao.TipoCalculoPagamentoId = ddlTipoCalculoPagamento.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToInt32(ddlTipoCalculoPagamento.SelectedValue);
                    dadosRotaAtualizacao.UsuarioResponsavel = User.Identity.Name;
                    dadosRotaAtualizacao.Ativo = chkAtivo.Checked;

                    //Ida
                    dadosRotaAtualizacao.TipoContratacaoIdIda = ddlTipoContratacaoIda.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToInt32(ddlTipoContratacaoIda.SelectedValue);
                    dadosRotaAtualizacao.ValorRotaIda = txtValorRotaIda.Text.IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToDecimal(txtValorRotaIda.Text);
                    dadosRotaAtualizacao.QuantidadeKmIda = txtQuantidadeKmIda.Text.IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToDecimal(txtQuantidadeKmIda.Text);
                    dadosRotaAtualizacao.TempoIda = txtTempoIda.Text.IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(txtTempoIda.Text);
                    dadosRotaAtualizacao.CondutorIdIda = (this.tseCondutorIda.IsValidDBValue && !this.tseCondutorIda.DBValue.IsNull) ? Convert.ToInt32(tseCondutorIda["condutorid"]) : -1;
                    dadosRotaAtualizacao.PrestadorIdIda = (this.tsePrestadorIda.IsValidDBValue && !this.tsePrestadorIda.DBValue.IsNull) ? Convert.ToInt32(tsePrestadorIda.DBValue.ToString()) : -1;
                    dadosRotaAtualizacao.VeiculoIdIda = (this.tseVeiculoIda.IsValidDBValue && !this.tseVeiculoIda.DBValue.IsNull) ? Convert.ToInt32(tseVeiculoIda["veiculoid"]) : -1;
                    dadosRotaAtualizacao.Ativo = chkAtivo.Checked;

                    //Volta
                    dadosRotaAtualizacao.TipoContratacaoIdVolta = ddlTipoContratacaoVolta.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToInt32(ddlTipoContratacaoVolta.SelectedValue);
                    dadosRotaAtualizacao.ValorRotaVolta = txtValorRotaVolta.Text.IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToDecimal(txtValorRotaVolta.Text);
                    dadosRotaAtualizacao.QuantidadeKmVolta = txtQuantidadeKmVolta.Text.IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToDecimal(txtQuantidadeKmVolta.Text);
                    dadosRotaAtualizacao.TempoVolta = txtTempoVolta.Text.IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(txtTempoVolta.Text);
                    dadosRotaAtualizacao.CondutorIdVolta = (this.tseCondutorVolta.IsValidDBValue && !this.tseCondutorVolta.DBValue.IsNull) ? Convert.ToInt32(tseCondutorVolta["condutorid"]) : -1;
                    dadosRotaAtualizacao.PrestadorIdVolta = (this.tsePrestadorVolta.IsValidDBValue && !this.tsePrestadorVolta.DBValue.IsNull) ? Convert.ToInt32(tsePrestadorVolta.DBValue.ToString()) : -1;
                    dadosRotaAtualizacao.VeiculoIdVolta = (this.tseVeiculoVolta.IsValidDBValue && !this.tseVeiculoVolta.DBValue.IsNull) ? Convert.ToInt32(tseVeiculoVolta["veiculoid"]) : -1;
                    dadosRotaAtualizacao.Ativo = chkAtivo.Checked;
                }

                if (rotaId <= 0)
                {
                    validacao = rnRota.ValidaInsercao(dadosRotaCadastro);
                }
                else
                {
                    validacao = rnRota.ValidaAtualizacao(dadosRotaAtualizacao);
                }

                if (validacao.Valido)
                {
                    if (rotaId <= 0)
                    {
                        rnRota.Insere(dadosRotaCadastro);
                        mensagem = "Rota incluído com sucesso.";

                        tseRota.ResetValue();
                        tseRota.DBValue = dadosRotaCadastro.Codigo;
                        lblCodigo.Text = dadosRotaCadastro.Codigo;
                        hdnRotaTrajetoIdIda.Value = dadosRotaCadastro.RotaTrajetoIdIda.ToString();
                        hdnRotaTrajetoIdVolta.Value = dadosRotaCadastro.RotaTrajetoIdVolta.ToString();
                    }
                    else
                    {
                        rnRota.Atualiza(dadosRotaAtualizacao);
                        mensagem = "Rota atualizado com sucesso.";
                    }

                    lblMensagem.Text = mensagem;

                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();

                    var script = @"alert('" + lblMensagem.Text + @"');";
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
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

        private void CarregaTurno()
        {
            RN.Turno rnTurno = new RN.Turno();

            try
            {
                ListItem item = new ListItem("Selecione", string.Empty);

                ddlTurno.Items.Clear();
                ddlTurno.DataSource = rnTurno.ListaTurnosRotaTransportePor();
                ddlTurno.DataBind();
                ddlTurno.Items.Insert(0, item);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaTipoContratacao()
        {
            RN.Transporte.TipoContratacao rnTipoContratacao = new Techne.Lyceum.RN.Transporte.TipoContratacao();

            try
            {
                DataTable resultado = rnTipoContratacao.ListaAtivo();
                ListItem item = new ListItem("Selecione", string.Empty);

                ddlTipoContratacaoIda.Items.Clear();
                ddlTipoContratacaoIda.DataSource = resultado;
                ddlTipoContratacaoIda.DataBind();
                ddlTipoContratacaoIda.Items.Insert(0, item);

                ddlTipoContratacaoVolta.Items.Clear();
                ddlTipoContratacaoVolta.DataSource = resultado;
                ddlTipoContratacaoVolta.DataBind();
                ddlTipoContratacaoVolta.Items.Insert(0, item);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaTipoCalculoPagamento()
        {
            RN.Transporte.TipoCalculoPagamento rnTipoCalculoPagamento = new Techne.Lyceum.RN.Transporte.TipoCalculoPagamento();
            try
            {
                ListItem item = new ListItem("Selecione", string.Empty);

                ddlTipoCalculoPagamento.Items.Clear();
                ddlTipoCalculoPagamento.DataSource = rnTipoCalculoPagamento.ListaAtivo();
                ddlTipoCalculoPagamento.DataBind();
                ddlTipoCalculoPagamento.Items.Insert(0, item);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
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

            ControlaAcesso(btnIncluir, AcaoControle.editar);
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(btnDesabilitar, AcaoControle.novo);
            ControlaAcesso(btnExcluir, AcaoControle.excluir);
            ControlaAcesso(btnIncluirAssociacao, AcaoControle.editar);
            ControlaAcesso(btnNovaAssociacaoAluno, AcaoControle.editar);
            ControlaAcesso(btnIncluirEmbarque, AcaoControle.editar);
            ControlaAcesso(btnNovoEmbarque, AcaoControle.editar);
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnEditar.Visible = false;
            btnNovo.Visible = false;
            btnIncluir.Visible = false;
            btnDesabilitar.Visible = false;
            btnIncluirAssociacao.Visible = false;
            btnNovaAssociacaoAluno.Visible = false;
            btnCancelAssociacaoAluno.Visible = false;
            btnExcluir.Visible = false;
            btnIncluirEmbarque.Visible = false;
            btnNovoEmbarque.Visible = false;
            btnCancelaEmbarque.Visible = false;
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

        protected void btnExcluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this._tipoOperacao = TipoOperacao.Excluir;
                this.ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnDesabilitar_Click(object sender, ImageClickEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Rota rnRota = new Techne.Lyceum.RN.Transporte.Rota();

            try
            {
                if (this.tseRota.DBValue.IsNull)
                {
                    this.lblMensagem.Text = "Favor consultar uma rota.";
                    return;
                }

                if (!this.tseRota.IsValidDBValue)
                {
                    this.lblMensagem.Text = "Rota não cadastrado.";
                    return;
                }

                int rotaId = Convert.ToInt32(this.tseRota["rotaid"]);
                validacao = rnRota.ValidaDesativacao(rotaId, User.Identity.Name);

                if (validacao.Valido)
                {
                    rnRota.Desativa(rotaId, User.Identity.Name);
                    _tipoOperacao = TipoOperacao.Consultar;
                    ControlarTipoOperacao();
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

        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Alterar;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rblTrajetoAssociacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LimparDadosAssociacao();

                if (!rblTrajetoAssociacao.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    pnlDadosNovaAssociacao.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                if (!tseAluno.DBValue.IsNull)
                {
                    if (tseAluno.IsValidDBValue)
                    {
                        lblMensagem.Text = string.Empty;
                    }
                    else
                    {
                        lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNovaAssociacaoAluno_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.NovaAssociacao;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnIncluirAssociacao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Transporte.Entidades.RotaAluno rotaAluno = new Techne.Lyceum.RN.Transporte.Entidades.RotaAluno();
                RN.Transporte.RotaAluno rnRotaAluno = new Techne.Lyceum.RN.Transporte.RotaAluno();

                rotaAluno.Aluno = (this.tseAluno.IsValidDBValue && !this.tseAluno.DBValue.IsNull) ? tseAluno.DBValue.ToString() : null;
                rotaAluno.DataInicio = !dtDataInicioNova.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataInicioNova.Date : DateTime.MinValue;
                rotaAluno.DataFim = !dtDataFimNova.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataFimNova.Date : DateTime.MinValue;
                rotaAluno.UsuarioId = User.Identity.Name;
                rotaAluno.RotaAlunoId = 0;

                if (!rblTrajetoAssociacao.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    rotaAluno.RotaTrajetoId = rblTrajetoAssociacao.SelectedValue == "Ida" ? Convert.ToInt32(hdnRotaTrajetoIdIda.Value) : Convert.ToInt32(hdnRotaTrajetoIdVolta.Value);
                }

                int tipoCalculo = !ddlTipoCalculoPagamento.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlTipoCalculoPagamento.SelectedValue) : -1;
                int pessoa = (this.tseAluno.IsValidDBValue && !this.tseAluno.DBValue.IsNull) ? Convert.ToInt32(tseAluno["pessoa"]) : -1;
                int rotaId = (this.tseRota.IsValidDBValue && !this.tseRota.DBValue.IsNull) ? Convert.ToInt32(tseRota["rotaId"]) : -1;
                string turno = !ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTurno.SelectedValue : null;

                validacao = rnRotaAluno.Valida(rotaAluno, tipoCalculo, turno, rotaId, true, pessoa, tseUnidadeResponsavel.DBValue.ToString());

                if (validacao.Valido)
                {
                    rnRotaAluno.Insere(rotaAluno);

                    _tipoOperacao = TipoOperacao.SucessoAssociacao;
                    ControlarTipoOperacao();

                    lblMensagem.Text = "Associação incluída com sucesso.";

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup",
                     "alert('Associação incluída com sucesso.');", true);
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCancelAssociacaoAluno_Click(object sender, EventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.CancelarAssociacao;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimparDadosAssociacao()
        {
            tseAluno.ResetValue();
            dtDataInicioNova.Text = string.Empty;
            dtDataFimNova.Text = string.Empty;
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

        protected void grdRotaAlunoIda_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Entidades.RotaAluno rotaAluno = new Techne.Lyceum.RN.Transporte.Entidades.RotaAluno();
            RN.Transporte.RotaAluno rnRotaAluno = new Techne.Lyceum.RN.Transporte.RotaAluno();


            rotaAluno.Aluno = e.NewValues["ALUNO"] != null ? Convert.ToString(e.NewValues["ALUNO"]) : null;
            rotaAluno.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            rotaAluno.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            rotaAluno.RotaTrajetoId = Convert.ToInt32(grdRotaAlunoIda.GetRowValuesByKeyValue(e.Keys[0], "ROTATRAJETOID"));
            rotaAluno.UsuarioId = User.Identity.Name;
            rotaAluno.RotaAlunoId = Convert.ToInt32(e.Keys["ROTAALUNOID"]);

            int tipoCalculo = !ddlTipoCalculoPagamento.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlTipoCalculoPagamento.SelectedValue) : -1;
            int pessoa = Convert.ToInt32(grdRotaAlunoIda.GetRowValuesByKeyValue(e.Keys[0], "PESSOA"));
            int rotaId = (this.tseRota.IsValidDBValue && !this.tseRota.DBValue.IsNull) ? Convert.ToInt32(tseRota["rotaId"]) : -1;
            string turno = !ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTurno.SelectedValue : null;

            validacao = rnRotaAluno.Valida(rotaAluno, tipoCalculo, turno, rotaId, false, pessoa, tseUnidadeResponsavel.DBValue.ToString());

            if (validacao.Valido)
            {
                rnRotaAluno.Atualiza(rotaAluno);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdRotaAlunoIda.DataBind();
        }

        public void UpdateRotaAlunoIda(object ALUNO, object NOME, object DATAINICIO, object DATAFIM, object ROTAALUNOID)
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

        protected void grdRotaAlunoVolta_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Entidades.RotaAluno rotaAluno = new Techne.Lyceum.RN.Transporte.Entidades.RotaAluno();
            RN.Transporte.RotaAluno rnRotaAluno = new Techne.Lyceum.RN.Transporte.RotaAluno();


            rotaAluno.Aluno = e.NewValues["ALUNO"] != null ? Convert.ToString(e.NewValues["ALUNO"]) : null;
            rotaAluno.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            rotaAluno.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            rotaAluno.RotaTrajetoId = Convert.ToInt32(grdRotaAlunoVolta.GetRowValuesByKeyValue(e.Keys[0], "ROTATRAJETOID"));
            rotaAluno.UsuarioId = User.Identity.Name;
            rotaAluno.RotaAlunoId = Convert.ToInt32(e.Keys["ROTAALUNOID"]);

            int tipoCalculo = !ddlTipoCalculoPagamento.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlTipoCalculoPagamento.SelectedValue) : -1;
            int pessoa = Convert.ToInt32(grdRotaAlunoVolta.GetRowValuesByKeyValue(e.Keys[0], "PESSOA"));
            int rotaId = (this.tseRota.IsValidDBValue && !this.tseRota.DBValue.IsNull) ? Convert.ToInt32(tseRota["rotaId"]) : -1;
            string turno = !ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTurno.SelectedValue : null;

            validacao = rnRotaAluno.Valida(rotaAluno, tipoCalculo, turno, rotaId, false, pessoa, tseUnidadeResponsavel.DBValue.ToString());

            if (validacao.Valido)
            {
                rnRotaAluno.Atualiza(rotaAluno);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdRotaAlunoVolta.DataBind();
        }

        public void UpdateRotaAlunoVolta(object ALUNO, object NOME, object DATAINICIO, object DATAFIM, object ROTAALUNOID)
        { }
        protected void ddlTipoContratacaoIda_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!ddlTipoContratacaoIda.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (ddlTipoContratacaoIda.SelectedValue == "1") // ALUGUEL
                    {
                        lblValorRotaIda.Text = "Valor diária:*";
                    }
                    else
                    {
                        lblValorRotaIda.Text = "Valor unitário:*";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlTipoContratacaoVolta_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!ddlTipoContratacaoVolta.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (ddlTipoContratacaoVolta.SelectedValue == "1") // ALUGUEL
                    {
                        lblValorRotaVolta.Text = "Valor diária:*";
                    }
                    else
                    {
                        lblValorRotaVolta.Text = "Valor unitário:*";
                    }
                }
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
                _tipoOperacao = TipoOperacao.Inicial;

                var sessao = RN.SessaoUsuario.GetSessaoUsuario();

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
    }
}
