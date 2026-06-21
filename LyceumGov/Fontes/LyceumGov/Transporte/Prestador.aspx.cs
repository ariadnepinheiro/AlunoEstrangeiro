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
    [NavUrl("~/Transporte/Prestador.aspx")]
    [ControlText("Prestador")]
    [Title("Prestador")]

    public partial class Prestador : TPage
    {
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
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
            RN.Transporte.Entidades.Prestador prestador = new Techne.Lyceum.RN.Transporte.Entidades.Prestador();
            RN.Transporte.Prestador rnPrestador = new Techne.Lyceum.RN.Transporte.Prestador();

            switch (this._tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles,null);
                        tsePrestador.ResetValue();
                        tseCondutor.ResetValue();
                        // pnlGrid.Visible = false;
                        grdCondutor.DataSource = null;
                        grdCondutor.DataBind();
                        pcPrestador.TabPages[1].Enabled = false;
                        pcPrestador.TabPages[2].Enabled = false;
                        pnlNovaVigencia.Visible = false;
                        pnAbas.Visible = false;
                        tsePrestador.Mode = Techne.Controls.ControlMode.Edit;
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, btnExcluir };
                        ControlarVisibilidadeControle(controles,null);
                        pnAbas.Visible = true;
                        grdCondutor.DataBind();
                        tseCondutor.ResetValue();
                        grdCondutor.DataBind();
                        pnlDados.Visible = true;
                        pcPrestador.TabPages[1].Enabled = true;
                        pcPrestador.TabPages[2].Enabled = true;

                        pnlNovaVigencia.Visible = false;
                        this.odsPrestadorVigencia.Select();
                        this.odsPrestadorVigencia.DataBind();
                        this.grdPrestadorVigencia.DataBind();
                        DesabilitaCampos();
                        grdCondutor.Columns[0].Visible = false;
                        grdPrestadorVigencia.Columns[0].Visible = false;
                        break;
                    }
                case TipoOperacao.Novo:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnIncluir };
                        ControlarVisibilidadeControle(controles,null);
                        tsePrestador.ResetValue();
                        tsePrestador.Enabled = false;
                        tsePrestador.Mode = ControlMode.View;
                        pnAbas.Visible = true;
                        pnlDados.Visible = true;
                        pnlNovaVigencia.Visible = true;
                        LimparDadosPrestador();
                        LimparDadosNovaVigencia();
                        pcPrestador.ActiveTabIndex = 0;
                        pcPrestador.TabPages[1].Enabled = false;
                        pcPrestador.TabPages[2].Enabled = false;
                        ControlarDocumento("CNPJ", String.Empty);
                        this.HabilitaCampos();
                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        var controles = new[] { btnNovo, btnEditar, btnExcluir };
                        ControlarVisibilidadeControle(controles, null);
                        pcPrestador.ActiveTabIndex = 0;
                        pnlDados.Visible = false;
                        pnlNovaVigencia.Visible = false;
                        LimparDadosPrestador();
                        LimparDadosNovaVigencia();
                        LimparDadosPrestadorVigencia();
                        tseCondutor.ResetValue();
                        pnAbas.Visible = true;
                        tsePrestador.Enabled = true;

                        prestador = rnPrestador.ObtemPor(Convert.ToInt32(tsePrestador.DBValue.ToString()));

                        if (prestador.PrestadorId > 0)
                        {
                            if (!prestador.Cnpj.IsNullOrEmptyOrWhiteSpace())
                            {
                                txtCNPJ.Text = prestador.Cnpj.ToString().AplicarMascaraCNPJ();
                            }
                            else
                            {
                                txtCPF.Text = prestador.Cpf.ToString().AplicarMascaraCPF();
                            }
                            txtNome.Text = !prestador.Nome.IsNullOrEmptyOrWhiteSpace() ? prestador.Nome.Trim() : string.Empty;

                            long resultado;

                            if (long.TryParse(prestador.Telefone.Trim().RetirarMascaraTelefone(), out resultado))
                            {
                                if (prestador.Telefone.Trim().RetirarMascaraTelefone().Length == 10)
                                {
                                    txtFone.Text = string.Format("{0:(00)0000-0000}", resultado);
                                }
                                if (prestador.Telefone.Trim().RetirarMascaraTelefone().Length == 11)
                                {
                                    txtFone.Text = string.Format("{0:(00)00000-0000}", resultado);
                                }
                            }

                            grdCondutor.DataBind();
                            pnlDados.Visible = true;
                            pcPrestador.TabPages[1].Enabled = true;
                            pcPrestador.TabPages[2].Enabled = true;
                        }
                        ControlarDocumento(txtCNPJ.Text, txtCPF.Text);
                        DesabilitaCampos();
                        grdCondutor.Columns[0].Visible = false;
                        grdPrestadorVigencia.Columns[0].Visible = false;
                        break;
                    }
                case TipoOperacao.Cancelar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles,null);
                        pnlDados.Visible = false;
                        pnlNovaVigencia.Visible = false;
                        pnAbas.Visible = false;
                        tsePrestador.ResetValue();
                        tsePrestador.Mode = ControlMode.Edit;
                        tsePrestador.ReadOnly = false;
                        tsePrestador.Enabled = true;
                        grdCondutor.DataSource = null;
                        grdCondutor.DataBind();
                        grdPrestadorVigencia.DataSource = null;
                        grdPrestadorVigencia.DataBind();
                        grdPrestadorVigencia.CancelEdit();
                        grdCondutor.CancelEdit();
                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        tsePrestador.Enabled = false;
                        HabilitaCampos();
                        pcPrestador.ActiveTabIndex = 0;

                        ImageButton[] controles = new ImageButton[] { btnCancel, btnIncluir };
                        Button[] controlesButton = new[] { btnIncluirCondutor, btnIncluirVigencia, btnCancelarCondutor, btnCancelarVigencia };
                        ControlarVisibilidadeControle(controles, controlesButton);
                        ControlarDocumento(txtCNPJ.Text, txtCPF.Text);
                        grdCondutor.Columns[0].Visible = true;
                        grdPrestadorVigencia.Columns[0].Visible = true;
                        break;
                    }
                case TipoOperacao.Excluir:
                    {
                        ValidacaoDados validacao = new ValidacaoDados();
                        if (this.tsePrestador.IsValidDBValue && !this.tsePrestador.DBValue.IsNull)
                        {
                            validacao = rnPrestador.ValidaRemocao(Convert.ToInt32(tsePrestador.DBValue));

                            if (validacao.Valido)
                            {
                                rnPrestador.Remove(Convert.ToInt32(tsePrestador.DBValue));
                                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup",
                                                                               "alert('Prestador excluído com sucesso.');", true);

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
            }

        }

        public object ListarCondutor(object prestadorId)
        {
            RN.Transporte.Prestador rnPrestador = new Techne.Lyceum.RN.Transporte.Prestador();

            if (!string.IsNullOrEmpty(prestadorId.ToString()))
            {
                return rnPrestador.ListaPrestadorCondutorPor(Convert.ToInt32(prestadorId));
            }
            return null;
        }

        public object ListarVigenciaPrestador(object prestadorId)
        {
            RN.Transporte.PrestadorVigencia rnPrestadorVigencia = new Techne.Lyceum.RN.Transporte.PrestadorVigencia();

            if (!string.IsNullOrEmpty(prestadorId.ToString()))
            {
                return rnPrestadorVigencia.ListaPor(Convert.ToInt32(prestadorId));
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

                    if (_tipoOperacao.Equals(TipoOperacao.Novo))
                    {
                        tsePrestador.Mode = ControlMode.View;
                        tsePrestador.Enabled = false;
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
            TituloGrid(grdPrestadorVigencia, "Vigência");
            TituloGrid(grdCondutor, "Condutor(es) Vinculado(s)");
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnExcluir, AcaoControle.excluir);
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(btnIncluirCondutor, AcaoControle.editar);
            ControlaAcesso(btnIncluirVigencia, AcaoControle.editar);
        }

        protected void pcPrestador_TabClick(object source, TabControlCancelEventArgs e)
        {
            this.lblMensagem.Text = string.Empty;

            if (e.Tab.Name == "")
            {
                pnlNovaVigencia.Visible = false;

            }
            else if (e.Tab.Name == "Vínculo Unidade Escolar")
            {
                LimparDadosPrestadorVigencia();
            }
            else if (e.Tab.Name == "Condutor")
            {
                tseCondutor.ResetValue();
                pnlCondutor.Visible = true;
            }

        }

        private void ControlarVisibilidadeControle(ImageButton[] imgBotoes, Button[] botoes)
        {
            RetiraVisibilidadeBotao();

            if (imgBotoes != null)
            {
                foreach (ImageButton botao in imgBotoes)
                {
                    botao.Visible = true;
                }
            }

            if (botoes != null)
            {
                foreach (Button botao in botoes)
                {
                    botao.Visible = true;
                }
            }

            ControlaAcesso(btnIncluir, AcaoControle.editar);
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(btnIncluirCondutor, AcaoControle.editar);
            ControlaAcesso(btnIncluirVigencia, AcaoControle.editar);
            ControlaAcesso(btnExcluir, AcaoControle.excluir);
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnEditar.Visible = false;
            btnNovo.Visible = false;
            btnIncluir.Visible = false;
            btnIncluirCondutor.Visible = false;
            btnIncluirVigencia.Visible = false;
            btnCancelarCondutor.Visible = false;
            btnCancelarVigencia.Visible = false;
            btnExcluir.Visible = false;

        }          

        protected void rblCNPJ_CPF_SelectedIndexChanged(object sender, EventArgs e)
        {
            String CNPJ = String.Empty;
            String CPF = String.Empty;

            if (rblCNPJ_CPF.SelectedItem.Value == "CNPJ")
            {
                CNPJ = rblCNPJ_CPF.SelectedItem.Value;
                txtCPF.Text = String.Empty;
            }

            if (rblCNPJ_CPF.SelectedItem.Value == "CPF")
            {
                CPF = rblCNPJ_CPF.SelectedItem.Value;
                txtCNPJ.Text = String.Empty;

            }

            ControlarDocumento(CNPJ, CPF);
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
        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Alterar;
            ControlarTipoOperacao();
        }

        private void LimparDadosPrestador()
        {
            rblCNPJ_CPF.ClearSelection();
            txtCNPJ.Text = string.Empty;
            txtCPF.Text = string.Empty;
            txtFone.Text = string.Empty;
            txtNome.Text = string.Empty;
        }

        private void LimparDadosNovaVigencia()
        {
            tseRegionalNova.ResetValue();
            tseMunicipioNova.ResetValue();
            tseUnidadeResponsavelNova.ResetValue();
            dtDataInicioNova.Text = string.Empty;
            dtDataFimNova.Text = string.Empty;
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
                        //pnlAvaliacao.Visible = true;
                        //grdAvaliacao.DataBind();
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
                lblMensagem.Visible = true;
            }
        }

        protected void tseCondutor_Changed(object sender, ChangedEventArgs args)
        {
            try
            {

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

        protected void tseRegionalNova_Changed(object sender, Techne.Controls.ChangedEventArgs args)
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
                    if (!this.tseRegionalNova.DBValue.IsNull)
                    {
                        if (this.tseRegionalNova.IsValidDBValue)
                        {
                            sessao.Regional = Convert.ToString(tseRegionalNova.DBValue);
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;

                            tseMunicipioNova.ResetValue();
                            tseUnidadeResponsavelNova.ResetValue();
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
                        tseMunicipioNova.ResetValue();
                        tseUnidadeResponsavelNova.ResetValue();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

        protected void tseMunicipioNova_Changed(object sender, Techne.Controls.ChangedEventArgs args)
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
                    if (!this.tseMunicipioNova.DBValue.IsNull)
                    {
                        if (this.tseMunicipioNova.IsValidDBValue)
                        {
                            sessao.Municipio = Convert.ToString(tseMunicipioNova.DBValue);
                            sessao.Escola = string.Empty;
                        }
                        else
                        {
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                            tseUnidadeResponsavelNova.ResetValue();
                        }
                    }
                    else
                    {
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                        tseUnidadeResponsavelNova.ResetValue();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeResponsavelNova_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                var sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (!this.tseUnidadeResponsavelNova.DBValue.IsNull)
                {
                    if (this.tseUnidadeResponsavelNova.IsValidDBValue)
                    {

                        if (!this.tseUnidadeResponsavelNova["unidade_ens"].IsNull)
                        {
                            this.tseRegionalNova.Value = this.tseUnidadeResponsavelNova["id_regional"];
                            this.tseMunicipioNova.Value = this.tseUnidadeResponsavelNova["municipio"];
                        }

                        if (sessao != null)
                        {
                            sessao.Escola = Convert.ToString(this.tseUnidadeResponsavelNova.DBValue);
                            sessao.Regional = Convert.ToString(this.tseRegionalNova.DBValue);
                            sessao.Municipio = Convert.ToString(this.tseMunicipioNova.DBValue);
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
                lblMensagem.Visible = true;
            }
        }

        protected void btnIncluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Transporte.Prestador rnPrestador = new Techne.Lyceum.RN.Transporte.Prestador();
                RN.Transporte.Entidades.Prestador prestador = new Techne.Lyceum.RN.Transporte.Entidades.Prestador();
                RN.Transporte.Entidades.PrestadorVigencia prestadorVigencia = new Techne.Lyceum.RN.Transporte.Entidades.PrestadorVigencia();
                bool pessoaJuridica = false;
                string mensagem = string.Empty;

                prestador.PrestadorId = (this.tsePrestador.IsValidDBValue && !this.tsePrestador.DBValue.IsNull) ? Convert.ToInt32(tsePrestador.DBValue.ToString()) : -1;
                if (!rblCNPJ_CPF.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (rblCNPJ_CPF.SelectedValue == "CPF")
                    {
                        prestador.Cpf = !txtCPF.Text.IsNullOrEmptyOrWhiteSpace() ? txtCPF.Text.RetirarMascaraCPF() : null;
                    }
                    else
                    {
                        prestador.Cnpj = !txtCNPJ.Text.IsNullOrEmptyOrWhiteSpace() ? txtCNPJ.Text.RetirarMascaraCNPJ() : null;
                        pessoaJuridica = true;
                    }
                }
               
                prestador.Nome = !txtNome.Text.Trim().IsNullOrEmptyOrWhiteSpace() ? txtNome.Text.Trim().ToUpper() : null;
                prestador.Telefone = !txtFone.Text.Trim().IsNullOrEmptyOrWhiteSpace() ? txtFone.Text.Trim() : null;
                prestador.UsuarioId = User.Identity.Name;
                
                if (prestador.PrestadorId == -1)
                {
                    prestador.Ativo = true;
                    prestadorVigencia.Censo = (this.tseUnidadeResponsavelNova.IsValidDBValue && !this.tseUnidadeResponsavelNova.DBValue.IsNull) ? tseUnidadeResponsavelNova.DBValue.ToString() : null;
                    prestadorVigencia.DataInicio = !dtDataInicioNova.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataInicioNova.Date : DateTime.MinValue;
                    prestadorVigencia.DataFim = !dtDataFimNova.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataFimNova.Date : DateTime.MinValue;

                    validacao = rnPrestador.ValidaInsercao(prestador, pessoaJuridica, prestadorVigencia);
                }
                else
                {
                    validacao = rnPrestador.ValidaAlteracao(prestador, pessoaJuridica);
                }

                if (validacao.Valido)
                {
                    if (prestador.PrestadorId == -1)
                    {
                        rnPrestador.Insere(prestador, prestadorVigencia);
                        mensagem = "Prestador incluído com sucesso.";

                        tsePrestador.ResetValue();
                        tsePrestador.DBValue = prestador.PrestadorId.ToString();
                    }
                    else
                    {
                        rnPrestador.Atualiza(prestador);
                        mensagem = "Prestador atualizado com sucesso.";
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

        private void LimparDadosPrestadorVigencia()
        {
            tseRegional.ResetValue();
            tseMunicipio.ResetValue();
            tseUnidadeResponsavel.ResetValue();
            dtDataInicio.Text = string.Empty;
            dtDataFim.Text = string.Empty;
        }

        protected void grdCondutor_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdCondutor);
        }

        protected void grdCondutor_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdCondutor.Settings.ShowFilterRow = false;
        }

        protected void grdCondutor_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdCondutor.Settings.ShowFilterRow = false;
        }

        protected void grdCondutor_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "CPF" && e.Value != null)
            {

                e.DisplayText = string.Format(@"{0:000\.000\.000\-00}", e.Value);

            }
        }

        protected void grdCondutor_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                var prestadorId = Convert.ToString(e.GetListSourceFieldValue("PRESTADORID"));                
                var condutorId = Convert.ToString(e.GetListSourceFieldValue("CONDUTORID"));
                e.Value = prestadorId + ";" + condutorId;
            }
        }

        protected void grdCondutor_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Prestador rnPrestador = new Techne.Lyceum.RN.Transporte.Prestador();
            int condutorId, prestadorId = 0;

            string[] chaves = e.Keys["CompositeKey"].ToString().Split(';');

            prestadorId = int.Parse(chaves[0]);
            condutorId = int.Parse(chaves[1]);


            validacao = rnPrestador.ValidaRemocaoPrestadorCondutor(prestadorId,condutorId,User.Identity.Name);

            if (validacao.Valido)
            {
                rnPrestador.RemovePrestadorCondutor(prestadorId, condutorId, User.Identity.Name);
                grdCondutor.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        private void ControlarDocumento(String CNPJ, String CPF)
        {
            bool documento = ((!String.IsNullOrEmpty(CNPJ)) ? true : (String.IsNullOrEmpty(CPF) ? true : false));
            lblCNPJ.Enabled = documento;
            txtCNPJ.Enabled = documento;
            lblCNPJ.Visible = documento;
            txtCNPJ.Visible = documento;
            rblCNPJ_CPF.Items[0].Selected = documento;

            lblCPF.Enabled = !documento;
            txtCPF.Enabled = !documento;
            lblCPF.Visible = !documento;
            txtCPF.Visible = !documento;
            rblCNPJ_CPF.Items[1].Selected = !documento;
        }

        protected void btnIncluirCondutor_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Transporte.Prestador rnPrestador = new Techne.Lyceum.RN.Transporte.Prestador();
                int prestadorId, condutorId = 0;

                prestadorId = (this.tsePrestador.IsValidDBValue && !this.tsePrestador.DBValue.IsNull) ? Convert.ToInt32(tsePrestador.DBValue.ToString()) : -1;
                condutorId = (this.tseCondutor.IsValidDBValue && !this.tseCondutor.DBValue.IsNull) ? Convert.ToInt32(tseCondutor["condutorid"]) : -1;

                validacao = rnPrestador.ValidaPrestadorCondutor(prestadorId, condutorId, User.Identity.Name);

                if (validacao.Valido)
                {
                    rnPrestador.InserePrestadorCondutor(prestadorId, condutorId, User.Identity.Name);
                    lblMensagem.Text = "Condutor associado com sucesso.";

                    tseCondutor.ResetValue();
                    grdCondutor.DataBind();

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup",
                      "alert('Condutor associado com sucesso.');", true);

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
        protected void btnCancelarCondutor_Click(object sender, EventArgs e)
        {
            try
            {
                tseCondutor.ResetValue();
                grdCondutor.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void btnIncluirVigencia_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Transporte.Entidades.PrestadorVigencia prestadorVigencia = new Techne.Lyceum.RN.Transporte.Entidades.PrestadorVigencia();
                RN.Transporte.PrestadorVigencia rnPrestadorVigencia = new Techne.Lyceum.RN.Transporte.PrestadorVigencia();

                prestadorVigencia.Censo = (this.tseUnidadeResponsavel.IsValidDBValue && !this.tseUnidadeResponsavel.DBValue.IsNull) ? tseUnidadeResponsavel.DBValue.ToString() : null;
                prestadorVigencia.DataInicio = !string.IsNullOrEmpty(dtDataInicio.Text) ? dtDataInicio.Date : DateTime.MinValue;
                prestadorVigencia.DataFim = !string.IsNullOrEmpty(dtDataFim.Text) ? dtDataFim.Date : DateTime.MinValue;
                prestadorVigencia.UsuarioId = User.Identity.Name;
                prestadorVigencia.PrestadorVigenciaId = 0;
                prestadorVigencia.PrestadorId = (this.tsePrestador.IsValidDBValue && !this.tsePrestador.DBValue.IsNull) ? Convert.ToInt32(tsePrestador.DBValue.ToString()) : -1;

                validacao = rnPrestadorVigencia.Valida(prestadorVigencia, true);

                if (validacao.Valido)
                {
                    rnPrestadorVigencia.Insere(prestadorVigencia);
                    grdPrestadorVigencia.DataBind();

                    LimparDadosPrestadorVigencia();

                    lblMensagem.Text = "Vigência incluída com sucesso.";

                     this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup",
                      "alert('Vigência incluída com sucesso.');", true);
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
        protected void btnCancelarVigencia_Click(object sender, EventArgs e)
        {
            try
            {
                LimparDadosPrestadorVigencia();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdPrestadorVigencia_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdPrestadorVigencia);
        }

        protected void grdPrestadorVigencia_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdPrestadorVigencia.Settings.ShowFilterRow = false;
        }

        protected void grdPrestadorVigencia_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdPrestadorVigencia.Settings.ShowFilterRow = false;
        }

        //protected void grdPrestador_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        //{
        //    if (grdPrestador.IsEditing)
        //    {

        //        if ((e.Column.FieldName) == "CNPJCPF")
        //        {
        //            e.Editor.ReadOnly = true;
        //            e.Editor.ClientEnabled = false;
        //        }
        //        else if ((e.Column.FieldName) == "NOME")
        //        {
        //            e.Editor.ReadOnly = true;
        //            e.Editor.ClientEnabled = false;
        //        }
        //        else if ((e.Column.FieldName) == "MOTIVOBLOQUEIO")
        //        {
        //            e.Editor.ReadOnly = true;
        //            e.Editor.ClientEnabled = false;
        //        }
        //        else if ((e.Column.FieldName) == "DATABLOQUEIO")
        //        {
        //            e.Editor.ReadOnly = true;
        //            e.Editor.ClientEnabled = false;
        //        }

        //    }
        //}

        protected void grdPrestadorVigencia_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "CNPJCPF" && e.Value != null)
            {
                if (e.Value.ToString().Length == 11)
                {
                    e.DisplayText = string.Format(@"{0:000\.000\.000\-00}", e.Value);
                }
                else
                {
                    e.DisplayText = string.Format(@"{0:00\.000\.000\.0000\-00}", e.Value);
                }
            }
        }

        protected void grdPrestadorVigencia_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            //var dataDesbloqueio = grdPrestadorVigencia.GetRowValues(e.VisibleIndex, "DATADESBLOQUEIO").ToString();

            //if (!string.IsNullOrEmpty(dataDesbloqueio))
            //{
            //    if (Convert.ToDateTime(dataDesbloqueio) < DateTime.Now.Date)
            //    {
            //        if (e.ButtonType == ColumnCommandButtonType.Edit)
            //        {
            //            e.Visible = false;
            //        }
            //    }
            //}
        }

        protected void grdPrestadorVigencia_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Entidades.PrestadorVigencia prestadorVigencia = new Techne.Lyceum.RN.Transporte.Entidades.PrestadorVigencia();
            RN.Transporte.PrestadorVigencia rnPrestadorVigencia = new Techne.Lyceum.RN.Transporte.PrestadorVigencia();

            prestadorVigencia.PrestadorId = (this.tsePrestador.IsValidDBValue && !this.tsePrestador.DBValue.IsNull) ? Convert.ToInt32(tsePrestador.DBValue.ToString()) : -1;
            prestadorVigencia.Censo = e.NewValues["CENSO"] != null ? Convert.ToString(e.NewValues["CENSO"]) : null;
            prestadorVigencia.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            prestadorVigencia.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            prestadorVigencia.UsuarioId = User.Identity.Name;
            prestadorVigencia.PrestadorVigenciaId = Convert.ToInt32(e.Keys["PRESTADORVIGENCIAID"]);

            validacao = rnPrestadorVigencia.Valida(prestadorVigencia, false);

            if (validacao.Valido)
            {
                rnPrestadorVigencia.Atualiza(prestadorVigencia);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdPrestadorVigencia.DataBind();
        }

        protected void grdPrestadorVigencia_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Entidades.PrestadorVigencia prestadorVigencia = new Techne.Lyceum.RN.Transporte.Entidades.PrestadorVigencia();
            RN.Transporte.PrestadorVigencia rnPrestadorVigencia = new Techne.Lyceum.RN.Transporte.PrestadorVigencia();

            int prestadorVigenciaId = 0;

            prestadorVigenciaId = Convert.ToInt32(e.Keys["PRESTADORVIGENCIAID"]);
            string censo = e.Values["CENSO"].ToString().Trim();

            validacao = rnPrestadorVigencia.ValidaRemocao(prestadorVigenciaId, User.Identity.Name, censo);

            if (validacao.Valido)
            {
                rnPrestadorVigencia.Remove(prestadorVigenciaId);
                grdPrestadorVigencia.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        public void UpdatePrestador(object CENSO ,object REGIONAL, object MUNICIPIODESCRICAO,object ESCOLA,object DATAINICIO, object DATAFIM, object PRESTADORVIGENCIAID)
        { }
        
        public void DeletePrestador(object PRESTADORVIGENCIAID)
        { }

        protected void DesabilitaCampos()
        {
            rblCNPJ_CPF.Enabled = false;
            txtCNPJ.Enabled = false;
            txtCPF.Enabled = false;
            txtNome.Enabled = false;
            txtFone.Enabled = false;
            tseCondutor.Mode = ControlMode.View;
            tseCondutor.ReadOnly = true;
            tseCondutor.Enabled = false;
            tseRegional.Mode = ControlMode.View;
            tseRegional.ReadOnly = true;
            tseRegional.Enabled = false;
            tseUnidadeResponsavel.Mode = ControlMode.View;
            tseUnidadeResponsavel.ReadOnly = true;
            tseUnidadeResponsavel.Enabled = false;
            tseMunicipio.Mode = ControlMode.View;
            tseMunicipio.ReadOnly = true;
            tseMunicipio.Enabled = false;
            dtDataFim.Enabled = false;
            dtDataInicio.Enabled = false;

        }

        protected void HabilitaCampos()
        {
            rblCNPJ_CPF.Enabled = true;
            txtCNPJ.Enabled = true;
            txtCPF.Enabled = true;
            txtNome.Enabled = true;
            txtFone.Enabled = true;
            tseCondutor.Mode = ControlMode.Edit;
            tseCondutor.ReadOnly = false;
            tseCondutor.Enabled = true;
            tseRegional.Mode = ControlMode.Edit;
            tseRegional.ReadOnly = false;
            tseRegional.Enabled = true;
            tseUnidadeResponsavel.Mode = ControlMode.Edit;
            tseUnidadeResponsavel.ReadOnly = false;
            tseUnidadeResponsavel.Enabled = true;
            tseMunicipio.Mode = ControlMode.Edit;
            tseMunicipio.ReadOnly = false;
            tseMunicipio.Enabled = true;
            dtDataFim.Enabled = true;
            dtDataInicio.Enabled = true;
        }


        public void Delete(object CompositeKey)
        { }
    }
}
