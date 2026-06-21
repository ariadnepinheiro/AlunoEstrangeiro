using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using System.Data.SqlTypes;
using System.Globalization;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [
    NavUrl("~/PrestacaoContas/SaldoInicialPlanoTrabalho.aspx"),
    ControlText("SaldoInicialPlanoTrabalho"),
    Title("Saldo Inicial"),
    ]
    public partial class SaldoInicialPlanoTrabalho : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Techne.Lyceum.RN.Perfil rnPerfil = new Techne.Lyceum.RN.Perfil();

                lblMensagem.Text = string.Empty;


                if (!IsPostBack)
                {
                    _tipoOperacao = TipoOperacao.Inicial;
                    ControlarTipoOperacao();
                    btnZerarSaldo.Visible = false;

                    if (rnPerfil.PossuiPerfilSaldoInicialTotalPor(User.Identity.Name) || RN.Usuarios.UsuarioPrivilegiado(User.Identity.Name))
                    {
                        hdnPodeZerar.Value = "true";
                    }
                    else
                    {
                        hdnPodeZerar.Value = "false";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public enum TipoOperacao
        {
            Novo,
            Alterar,
            Consultar,
            Editar,
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

        public object Lista()
        {
            RN.PrestacaoContas.Finalidade rnFinalidade = new Techne.Lyceum.RN.PrestacaoContas.Finalidade();

            return rnFinalidade.Lista();

        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {

        }

        public void LimpaControles()
        {
            tseUnidadeResponsavel.ResetValue();
            tsePlanoTrabalho.ResetValue();
            txtValorInicial.Text = "";
            txtDataReferencia.Text = "";
        }

        protected void tseUnidadeResponsavel_Changed(object sender, EventArgs args)
        {
            try
            {
                divDados.Visible = false;
                if (Page.IsCallback)
                {
                    return;
                }

                if (!tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (tseUnidadeResponsavel.IsValidDBValue)
                    {
                        this._tipoOperacao = TipoOperacao.Consultar;
                        lblMensagem.Text = string.Empty;
                    }
                    else
                    {
                        this._tipoOperacao = TipoOperacao.Inicial;

                        lblMensagem.Text = "";
                    }
                }
                else
                {
                    lblMensagem.Text = "";
                    _tipoOperacao = TipoOperacao.Inicial;
                }


                ControlarTipoOperacao();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tsePlanoTrabalho_Changed(object sender, EventArgs args)
        {
            RN.PrestacaoContas.SaldoInicial rnSaldoInicial = new Techne.Lyceum.RN.PrestacaoContas.SaldoInicial();
            RN.PrestacaoContas.Entidades.SaldoInicial saldoInicial = new Techne.Lyceum.RN.PrestacaoContas.Entidades.SaldoInicial();
            RN.PrestacaoContas.PlanilhaOrcamentaria rnPlanilhaOrcamentaria = new Techne.Lyceum.RN.PrestacaoContas.PlanilhaOrcamentaria();
            
            try
            {
                lblMensagem.Text = string.Empty;
                divDados.Visible = false;
                btnZerarSaldo.Visible = false;
                if (Page.IsCallback)
                {
                    return;
                }
                hdnSaldoInicialId.Value = string.Empty;

                if (!tsePlanoTrabalho.DBValue.IsNull)
                {
                    if (tsePlanoTrabalho.IsValidDBValue)
                    {
                        divDados.Visible = true;

                        var dados = rnSaldoInicial.ObtemSaldoInicialPor(Convert.ToString(tseUnidadeResponsavel.DBValue), Convert.ToInt32(tsePlanoTrabalho.DBValue));

                        if (dados.SaldoInicialID != null)
                        {
                            if (dados.SaldoInicialID.Value != null)
                            {
                                hdnSaldoInicialId.Value = Convert.ToString(dados.SaldoInicialID.Value);

                                if (Convert.ToBoolean(hdnPodeZerar.Value))
                                {
                                    btnZerarSaldo.Visible = true;
                                }
                            }
                        }


                        if (dados.ValorInicial != null)
                        {
                            txtValorInicial.Text = Convert.ToString(dados.ValorInicial.Value);

                        }

                        if (dados.DataReferenciaVinculo == DateTime.MinValue)
                        {
                            txtDataReferencia.Text = string.Empty;
                        }
                        else
                        {
                            txtDataReferencia.Text = dados.DataReferenciaVinculo.ToString("dd/MM/yyyy");
                        }

                        var possui = rnPlanilhaOrcamentaria.PossuiPlanilhaOrcamentariaPor(Convert.ToInt32(tsePlanoTrabalho.DBValue), tseUnidadeResponsavel.DBValue.ToString());

                        if (!possui)
                        {
                            if (txtDataReferencia.Text.IsNullOrEmptyOrWhiteSpace() && hdnSaldoInicialId.Value.IsNullOrEmptyOrWhiteSpace())
                            {
                                this._tipoOperacao = TipoOperacao.Novo;
                            }
                            else
                            {
                                this._tipoOperacao = TipoOperacao.Editar;
                            }
                        }
                        else
                        {
                            if (Convert.ToBoolean(hdnPodeZerar.Value))
                            {
                                this._tipoOperacao = TipoOperacao.Editar;
                            }
                            else
                            {
                                this._tipoOperacao = TipoOperacao.Consultar;
                            }
                        }


                    }
                    else
                    {
                        this._tipoOperacao = TipoOperacao.Inicial;
                        lblMensagem.Text = "Projeto/Programa não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Projeto/Programa não ativo ou não cadastrado (favor verificar).";
                    _tipoOperacao = TipoOperacao.Inicial;
                }


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

            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnNovo, AcaoControle.novo);

        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnEditar.Visible = false;
            btnNovo.Visible = false;
            btnSalvar.Visible = false;

        }

        private void ControlarTipoOperacao()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { };

                        tsePlanoTrabalho.ResetValue();
                        tseUnidadeResponsavel.ResetValue();
                        divDados.Visible = false;
                        ControlarVisibilidadeControle(controles);

                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnEditar };
                        ControlarVisibilidadeControle(controles);
                        txtValorInicial.Enabled = false;
                        txtDataReferencia.Enabled = false;
                        if (Convert.ToBoolean(hdnPodeZerar.Value))
                        {
                            btnZerarSaldo.Visible = true;
                        }

                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        ImageButton[] controles = new ImageButton[] { };
                        ControlarVisibilidadeControle(controles);
                        txtValorInicial.Enabled = false;
                        txtDataReferencia.Enabled = false;
                        break;

                    }
                case TipoOperacao.Editar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnEditar };
                        ControlarVisibilidadeControle(controles);
                        break;

                    }
                case TipoOperacao.Alterar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);
                        divDados.Visible = true;
                        txtValorInicial.Enabled = true;
                        txtDataReferencia.Enabled = true;
                        lblMensagem.Text = string.Empty;

                        break;

                    }
                case TipoOperacao.Novo:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        lblMensagem.Text = string.Empty;
                        txtDataReferencia.Text = string.Empty;
                        txtValorInicial.Text = string.Empty;
                        divDados.Visible = false;
                        break;

                    }

            }
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Inicial;
                ControlarTipoOperacao();
                LimpaControles();

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
                _tipoOperacao = TipoOperacao.Alterar;
                ControlarTipoOperacao();

                tseUnidadeResponsavel.Enabled = true;
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

        protected void btnZerarSaldo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.pucConfirmar.ShowOnPageLoad = true;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Consultar()
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

        protected void btnSalvar_Click(object sender, EventArgs e)
        {

            RN.PrestacaoContas.SaldoInicial rnSaldoInicial = new Techne.Lyceum.RN.PrestacaoContas.SaldoInicial();
            RN.PrestacaoContas.Entidades.SaldoInicial saldoInicial = new Techne.Lyceum.RN.PrestacaoContas.Entidades.SaldoInicial();

            var dadosInformacaoAdcionalAEE = new RN.PrestacaoContas.DTOs.DadosUnidadeAae();
            ValidacaoDados validacao = new ValidacaoDados();
            string mensagem = string.Empty;
            try
            {

                DateTime temp = new DateTime();
                if (!String.IsNullOrEmpty(txtDataReferencia.Text))
                {
                    var resultado = DateTime.TryParse(txtDataReferencia.Text, out temp);
                    if (resultado)
                    {
                        saldoInicial.DataReferenciaCalculo = !String.IsNullOrEmpty(txtDataReferencia.Text) ? Convert.ToDateTime(txtDataReferencia.Text) : SqlDateTime.MinValue.Value;
                    }
                    else
                    {
                        saldoInicial.DataInvalida = true;
                    }

                }
                else
                {
                    saldoInicial.DataReferenciaCalculo = SqlDateTime.MinValue.Value;
                }

                decimal valorInicial;
                if (!decimal.TryParse(this.txtValorInicial.Text, out valorInicial))
                {
                    lblMensagem.Text = "Saldo Inicial inválido, favor informar o saldo inicial de 0 à 9999999999999.99 .";
                    return;
                }

                saldoInicial.SaldoInicialId = !String.IsNullOrEmpty(hdnSaldoInicialId.Value) ? Convert.ToInt32(hdnSaldoInicialId.Value) : 0;
                saldoInicial.PlanoTrabalhoId = !String.IsNullOrEmpty(tsePlanoTrabalho.DBValue.ToString()) ? Convert.ToInt32(tsePlanoTrabalho.DBValue) : 0;
                saldoInicial.Censo = !String.IsNullOrEmpty(tseUnidadeResponsavel.DBValue.ToString()) ? Convert.ToString(tseUnidadeResponsavel.DBValue) : "";
                saldoInicial.ValorInicial = !String.IsNullOrEmpty(txtValorInicial.Text) ? Convert.ToDecimal(txtValorInicial.Text) : 0;
                saldoInicial.UsuarioId = User.Identity.Name;
                bool cadastro = hdnSaldoInicialId.Value.IsNullOrEmptyOrWhiteSpace();

                validacao = rnSaldoInicial.Valida(saldoInicial, cadastro);

                if (validacao.Valido)
                {
                    if (cadastro)
                    {
                        rnSaldoInicial.Insere(saldoInicial);
                        hdnSaldoInicialId.Value = saldoInicial.SaldoInicialId.ToString();
                        mensagem = "Saldo Inicial inserido com sucesso.";
                    }
                    else
                    {
                        rnSaldoInicial.Atualiza(saldoInicial);
                        mensagem = "Saldo Inicial alterado com sucesso.";
                    }

                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('" + mensagem + ".');", true);
                    lblMensagem.Text = mensagem;

                    _tipoOperacao = TipoOperacao.Sucesso;
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

        protected void btnSim_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.PrestacaoContas.SaldoInicial rnSaldoInicial = new Techne.Lyceum.RN.PrestacaoContas.SaldoInicial();

                validacao = rnSaldoInicial.ValidaRemocao(tseUnidadeResponsavel.DBValue.ToString(), Convert.ToInt32(tsePlanoTrabalho.DBValue), User.Identity.Name);

                if (validacao.Valido)
                {
                    rnSaldoInicial.Remove(tseUnidadeResponsavel.DBValue.ToString(), Convert.ToInt32(tsePlanoTrabalho.DBValue));

                    btnZerarSaldo.Visible = false;
                    hdnSaldoInicialId.Value = string.Empty;

                    this._tipoOperacao = TipoOperacao.Novo;
                    ControlarTipoOperacao();

                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");

                }

                this.pucConfirmar.ShowOnPageLoad = false;
            }

            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNao_Click(object sender, EventArgs e)
        {
            this.pucConfirmar.ShowOnPageLoad = false;

        }


    }
}
