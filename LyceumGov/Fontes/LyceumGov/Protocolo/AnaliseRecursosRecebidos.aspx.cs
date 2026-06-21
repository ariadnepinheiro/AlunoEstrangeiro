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
using DevExpress.Web.ASPxClasses;

namespace Techne.Lyceum.Net.Protocolo
{
    [NavUrl("~/Protocolo/AnaliseRecursosRecebidos.aspx")]
    [ControlText("AnaliseRecursosRecebidos")]
    [Title("Análise Recursos Recebidos")]

    public partial class AnaliseRecursosRecebidos : TPage
    {
        public enum TipoOperacao
        {
            Novo,
            Alterar,
            Consultar,
            Inicial,
            Sucesso,
            CancelarAnalise
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
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        LimpaTela();
                        CarregaSituacao();
                        CarregarDadosProtocolo();
                        pnlAnalise.Visible = false;
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        LimparDadosAnalise();
                        ddlPrograma.Enabled = false;
                        txtFolha.Enabled = false;
                        txtObservacao.Enabled = false;
                        pnlAnalise.Visible = false;
                        grdAnalise.DataBind();
                        break;
                    }
                case TipoOperacao.Novo:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);
                        pnlAnalise.Visible = true;
                        CarregaPrograma(Convert.ToInt32(lblTipoProtocoloId.Text));
                        ddlPrograma.SelectedValue = ddlPrograma.Items.FindByValue(lblProgramaProtocoloId.Text) != null ? lblProgramaProtocoloId.Text : string.Empty;
                        ddlPrograma.Enabled = false;
                        txtObservacao.Enabled = true;
                        txtFolha.Enabled = true;
                        LimparDadosAnalise();
                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel };
                        ControlarVisibilidadeControle(controles);
                        DesabilitaCampos();
                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);
                        pnlAnalise.Visible = true;
                        break;
                    }
                case TipoOperacao.CancelarAnalise:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        LimparDadosAnalise();
                        CarregaPrograma(Convert.ToInt32(lblTipoProtocoloId.Text));
                        ddlPrograma.SelectedValue = ddlPrograma.Items.FindByValue(lblProgramaProtocoloId.Text) != null ? lblProgramaProtocoloId.Text : string.Empty;
                        ddlPrograma.Enabled = false;
                        txtObservacao.Enabled = false;
                        txtFolha.Enabled = false;
                        pnlAnalise.Visible = false;
                        grdAnalise.DataBind();
                        break;
                    }

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

        protected void DesabilitaCampos()
        {
            ddlPrograma.Enabled = false;
            txtObservacao.Enabled = false;
            txtFolha.Enabled = false;
            ddlSituacao.Enabled = false;
            data.Enabled = false;
            txtDescricao.Enabled = false;
            tseRevisor.Enabled = false;
            tseAnalisador.Enabled = false;
        }

        protected void HabilitaCampos()
        {
            ddlPrograma.Enabled = false;
            txtObservacao.Enabled = true;
            txtFolha.Enabled = true;
            ddlSituacao.Enabled = true;
            data.Enabled = true;
            txtDescricao.Enabled = true;
            tseRevisor.Enabled = true;
            tseAnalisador.Enabled = true;
        }

        public object ListarProtocoloAnalise(object idProtocolo)
        {
            RN.Protocolo.Analise rnAnalise = new Techne.Lyceum.RN.Protocolo.Analise();

            if (idProtocolo != null)
            {
                return rnAnalise.ListaAnalisePor(Convert.ToInt32(idProtocolo.ToString()));
            }

            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;

            if (!Page.IsPostBack)
            {
                if (Request.QueryString.Keys.Count > 0)
                {
                    _tipoOperacao = TipoOperacao.Inicial;
                    ControlarTipoOperacao();
                }
                else
                {
                    ASPxWebControl.RedirectOnCallback("ListarAnaliseRecursoRecebido.aspx");
                }
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdAnalise, "Análise Protocolo de Recursos Recebidos");
            ControlaAcessoGrid();
        }

        protected void ControlaAcessoGrid()
        {
            foreach (GridViewColumn col in grdAnalise.Columns)
            {
                if (col is GridViewCommandColumn)
                {
                    if (((GridViewCommandColumn)col).CustomButtons["btnEditarAnalise"] != null)
                        ((GridViewCommandColumn)col).CustomButtons["btnEditarAnalise"].Visibility =
                            Permission.AllowUpdate ? GridViewCustomButtonVisibility.AllDataRows : GridViewCustomButtonVisibility.Invisible;
                }
            }

            ControlaAcesso(grdAnalise);
        }

        private void CarregaPrograma(int tipoProtocoloId)
        {
            RN.Protocolo.ProgramaProtocolo rnProgramaProtocolo = new Techne.Lyceum.RN.Protocolo.ProgramaProtocolo();
            try
            {
                ddlPrograma.Items.Clear();
                if (tipoProtocoloId > 0)
                {
                    ddlPrograma.DataSource = rnProgramaProtocolo.ListaProgramaProtocoloAtivo(tipoProtocoloId);
                    ddlPrograma.Items.Insert(0, new ListItem("Selecione", string.Empty));
                    ddlPrograma.DataBind();
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaSituacao()
        {
            RN.Protocolo.SituacaoProtocolo rnSituacaoProtocolo = new Techne.Lyceum.RN.Protocolo.SituacaoProtocolo();
            try
            {
                ddlSituacao.Items.Clear();
                ddlSituacao.DataSource = rnSituacaoProtocolo.ListaSituacaoProtocoloAtiva();
                ddlSituacao.Items.Insert(0, new ListItem("Selecione", string.Empty));
                ddlSituacao.DataBind();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregarDadosProtocolo()
        {
            try
            {
                RN.DTOs.DadosAnaliseProtocolo dadosProtocolo = new Techne.Lyceum.RN.DTOs.DadosAnaliseProtocolo();
                byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                dadosProtocolo = ObterDadosQueryString(decodedText);

                if (dadosProtocolo.ProtocoloPrestacaoId > 0)
                {
                    lblIdProtocolo.Text = dadosProtocolo.ProtocoloPrestacaoId.ToString();
                    lblCNPJ.Text = (!dadosProtocolo.UnidadeEnsino.IsNullOrEmptyOrWhiteSpace() ? dadosProtocolo.Cnpj.AplicarMascaraCNPJ() : string.Empty);
                    lblCNPJ.Visible = !lblCNPJ.Text.IsNullOrEmptyOrWhiteSpace();
                    lblCNPJTexto.Visible = !lblCNPJ.Text.IsNullOrEmptyOrWhiteSpace();
                    lblAno.Text = dadosProtocolo.Ano.ToString();
                    lblSemestre.Text = dadosProtocolo.Semestre.ToString();
                    lblNumeroProcesso.Text = dadosProtocolo.Processo;
                    lblDataProcesso.Text = dadosProtocolo.DataProcesso.Date.ToShortDateString();
                    txtFolha.Text =  dadosProtocolo.NumeroFolhas != null ? dadosProtocolo.NumeroFolhas.ToString() : string.Empty;
                    lblTipo.Text = dadosProtocolo.TipoProtocolo;
                    lblTipoProtocoloId.Text = dadosProtocolo.TipoProtocoloId.ToString();
                    lblProgramaProtocoloId.Text = dadosProtocolo.ProgramaProtocoloId.ToString();
                    CarregaPrograma(dadosProtocolo.TipoProtocoloId);
                    ddlPrograma.SelectedValue = dadosProtocolo.ProgramaProtocoloId > 0 ? dadosProtocolo.ProgramaProtocoloId.ToString() : string.Empty;
                    txtObservacao.Text = dadosProtocolo.Observacao;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private RN.DTOs.DadosAnaliseProtocolo ObterDadosQueryString(string queryString)
        {
            RN.DTOs.DadosAnaliseProtocolo dadosProtocolo = new RN.DTOs.DadosAnaliseProtocolo();
            RN.Protocolo.ProtocoloPrestacao rnProtocoloPrestacao = new Techne.Lyceum.RN.Protocolo.ProtocoloPrestacao();

            string[] listaDados = queryString.Split('&');

            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("ProtocoloPrestacaoId") >= 0)
                {
                    dadosProtocolo.ProtocoloPrestacaoId = Convert.ToInt32(dados.Substring(dados.LastIndexOf('=') + 1));
                }
            }

            if (dadosProtocolo.ProtocoloPrestacaoId > 0)
            {
                dadosProtocolo = rnProtocoloPrestacao.ObtemDadosAnaliseProtocoloPor(dadosProtocolo.ProtocoloPrestacaoId);

            }

            return dadosProtocolo;
        }

        private void LimpaTela()
        {
            lblIdProtocolo.Text = string.Empty;
            lblCNPJ.Text = string.Empty;
            lblAno.Text = string.Empty;
            lblSemestre.Text = string.Empty;
            lblNumeroProcesso.Text = string.Empty;
            txtFolha.Text = string.Empty;
            lblDataProcesso.Text = string.Empty;
            lblTipo.Text = string.Empty;
            lblTipoProtocoloId.Text = string.Empty;
            lblProgramaProtocoloId.Text = string.Empty;
            ddlPrograma.Items.Clear();
            txtObservacao.Text = string.Empty;
            LimparDadosAnalise();
        }

        private void LimparDadosAnalise()
        {
            lblAnaliseId.Text = string.Empty;
            ddlSituacao.ClearSelection();
            data.Text = string.Empty;
            txtDescricao.Text = string.Empty;
            tseRevisor.ResetValue();
            tseAnalisador.ResetValue();
        }

        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Novo;
            ControlarTipoOperacao();
        }

        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Alterar;
            ControlarTipoOperacao();
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            string queryString = "voltar";
            byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);
            Response.Redirect("ListarRecursosRecebidos.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
        }

        protected void btnSalvarAnalise_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                RN.Protocolo.Analise rnAnalise = new Techne.Lyceum.RN.Protocolo.Analise();
                bool cadastro = false;
                int programaId = 0;
                int? numeroFolhas = 0;
                string observacao = string.Empty;

                var analise = new RN.Protocolo.Entidades.Analise
                {
                    AnaliseId = !lblAnaliseId.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(lblAnaliseId.Text) : 0,
                    SituacaoProtocoloId = !ddlSituacao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlSituacao.SelectedValue) : -1,
                    Descricao = !txtDescricao.Text.Trim().IsNullOrEmptyOrWhiteSpace() ? txtDescricao.Text.Trim() : null,
                    UsuarioSistema = User.Identity.Name,
                    DataSituacao = !string.IsNullOrEmpty(data.Text.Trim()) ? data.Date : DateTime.MinValue,
                    ProtocoloPrestacaoId = !lblIdProtocolo.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(lblIdProtocolo.Text) : 0,
                    UsuarioAnalisador =  !tseAnalisador.DBValue.IsNull ? Convert.ToString(tseAnalisador["matricula"]) : null,
                    UsuarioRevisor = !tseRevisor.DBValue.IsNull ? Convert.ToString(tseRevisor["matricula"]) : null,

                };
                programaId = !ddlPrograma.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlPrograma.SelectedValue) : -1;
                observacao = !txtObservacao.Text.Trim().IsNullOrEmptyOrWhiteSpace() ? txtObservacao.Text.Trim() : null;
                cadastro = analise.AnaliseId == 0;
                string[] inicial = lblNumeroProcesso.Text.Split('-');
                var inicialProcesso = inicial[0].Length == 1 ? inicial[0] + "-03/" : inicial[0] + "-";


                numeroFolhas = !txtFolha.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtFolha.Text.Trim()) : ((inicialProcesso.ToString() == "SEI-") ? (int?)null : -1);

                var validacao = rnAnalise.Valida(analise, numeroFolhas, observacao, Convert.ToDateTime(lblDataProcesso.Text), cadastro);

                if (validacao.Valido)
                {
                    if (cadastro)
                    {
                        rnAnalise.Insere(analise, numeroFolhas, observacao);
                        lblAnaliseId.Text = analise.AnaliseId.ToString();
                    }
                    else
                    {
                        rnAnalise.Atualiza(analise, numeroFolhas, observacao);
                    }

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup",
                        "alert('Análise " + (cadastro ? " inserida " : " atualizada ") + "com sucesso.');", true);


                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();
                    ddlPrograma.SelectedValue = programaId > 0 ? programaId.ToString() : string.Empty;
                    lblProgramaProtocoloId.Text = programaId > 0 ? programaId.ToString() : string.Empty;
                    txtObservacao.Text = observacao;
                    txtFolha.Text = numeroFolhas.ToString();
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

        protected void btnCancelarAnalise_Click(object sender, EventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.CancelarAnalise;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdAnalise_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAnalise);
        }

        protected void grdAnalise_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            string analiseId = grdAnalise.GetRowValues(e.VisibleIndex, "ANALISEID").ToString();
            string programaId = grdAnalise.GetRowValues(e.VisibleIndex, "PROGRAMAPROTOCOLOID").ToString();
            string numeroFolhas = grdAnalise.GetRowValues(e.VisibleIndex, "NUMEROFOLHAS").ToString();
            string observacao = grdAnalise.GetRowValues(e.VisibleIndex, "OBSERVACAO").ToString();
            string situacaoprotocoloid = grdAnalise.GetRowValues(e.VisibleIndex, "SITUACAOPROTOCOLOID").ToString();
            string datasituacao = grdAnalise.GetRowValues(e.VisibleIndex, "DATASITUACAO").ToString();
            string descricao = grdAnalise.GetRowValues(e.VisibleIndex, "DESCRICAO").ToString();
            string usuarioAnalisador = grdAnalise.GetRowValues(e.VisibleIndex, "USUARIOANALISADOR").ToString();
            string usuarioRevisor = grdAnalise.GetRowValues(e.VisibleIndex, "USUARIOREVISOR").ToString();
            RN.Docentes rnDocente = new Techne.Lyceum.RN.Docentes(); 

            if (e.ButtonID == "btnEditarAnalise")
            {
                LimparDadosAnalise();
                lblAnaliseId.Text = analiseId;
                ddlPrograma.SelectedValue = ddlPrograma.Items.FindByValue(programaId) != null ? programaId : string.Empty;
                ddlPrograma.Enabled = false;
                txtObservacao.Enabled = true;
                txtFolha.Enabled = true;
                txtObservacao.Text = observacao;
                txtFolha.Text = numeroFolhas;
                ddlSituacao.SelectedValue = ddlSituacao.Items.FindByValue(situacaoprotocoloid) != null ? situacaoprotocoloid : string.Empty;
                txtDescricao.Text = descricao.Trim();
                
                //Busca IdVinculo
                string analisador = rnDocente.ObtemIdVinculoMatriculaPor(usuarioAnalisador);
                string revisor = rnDocente.ObtemIdVinculoMatriculaPor(usuarioRevisor);
                tseAnalisador.DBValue = analisador;
                tseRevisor.DBValue = revisor;
                if (!datasituacao.IsNullOrEmptyOrWhiteSpace())
                {
                    data.Date = Convert.ToDateTime(datasituacao);
                }

                _tipoOperacao = TipoOperacao.Alterar;
                ControlarTipoOperacao();
            }
        }
        protected void tseAnalisador_Changed(object sender, EventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }
                if (!this.tseAnalisador.DBValue.IsNull)
                {
                    if (this.tseAnalisador.IsValidDBValue)
                    {

                    }
                    else
                    {
                        lblMensagem.Text = "Analisador não encontrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Analisador não encontrado (favor verificar).";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void tseRevisor_Changed(object sender, EventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }
                if (!this.tseRevisor.DBValue.IsNull)
                {
                    if (this.tseRevisor.IsValidDBValue)
                    {

                    }
                    else
                    {
                        lblMensagem.Text = "Revisor não encontrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Revisor não encontrado (favor verificar).";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
