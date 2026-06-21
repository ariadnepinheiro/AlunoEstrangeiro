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

namespace Techne.Lyceum.Net.Protocolo
{
    [NavUrl("~/Protocolo/RecursosRecebidos.aspx")]
    [ControlText("RecursosRecebidos")]
    [Title("Recursos Recebidos")]

    public partial class RecursosRecebidos : TPage
    {
        public object ListarProtocolo(object id_regional, object unidade_ens)
        {
            RN.Protocolo.ProtocoloPrestacao rnProtocoloPrestacao = new Techne.Lyceum.RN.Protocolo.ProtocoloPrestacao();

            var regional = id_regional.ToString();
            var unidade = unidade_ens.ToString();

            if (!regional.IsNullOrEmptyOrWhiteSpace() || !unidade.IsNullOrEmptyOrWhiteSpace())
            {
                return rnProtocoloPrestacao.ListaProtocoloComUltimaSituacaoPor((!regional.IsNullOrEmptyOrWhiteSpace()) ? Convert.ToInt32(regional) : 0, (!unidade.IsNullOrEmptyOrWhiteSpace()) ? unidade.ToString() : string.Empty);
            }

            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                RN.Perfil rnPerfil = new Techne.Lyceum.RN.Perfil();

                if (!Page.IsPostBack)
                {
                    hdnPerfilCoord.Value = string.Empty;

                    if (rnPerfil.PossuiPerfilCoordenadorProtocoloPor(User.Identity.Name))
                    {
                        hdnPerfilCoord.Value = "S";
                    }

                    LimparTela();
                    lblCNPJ.Text = string.Empty;
                }
                grdPrestacao.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnSalvarPrestacao, AcaoControle.novo);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdPrestacao, "Protocolo de Recursos Recebidos");
            ControlaAcessoGrid();
            ControlarVisibilidadeControle();
        }
        private void ControlarVisibilidadeControle()
        {
            ControlaAcesso(btnSalvarPrestacao, AcaoControle.novo);
        }
        private void LimparTela()
        {
            txtPrestacaoID.Text = string.Empty;
            ddlPrograma.Items.Clear();
            ddlAno.ClearSelection();
            ddlInicialProcesso.ClearSelection();
            rblSemestre.ClearSelection();
            ddlTipoProtocolo.ClearSelection();
            txtProcesso.Text = string.Empty;
            txtFolha.Text = string.Empty;
            dtProcesso.Text = string.Empty;
            btnCancelarAtualizacao.Visible = false;
            btnSalvarPrestacao.Text = "Incluir Protocolo";
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
                lblCNPJ.Text = string.Empty;
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
                lblCNPJ.Text = string.Empty;
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
                lblCNPJ.Text = string.Empty;
                pnlDados.Visible = false;
                pnlGrid.Visible = false;

                if (!this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (this.tseUnidadeResponsavel.IsValidDBValue)
                    {
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
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

        protected void btnPesquisar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                lblCNPJ.Text = string.Empty;
                LimparTela();

                if ((this.tseUnidadeResponsavel.IsValidDBValue && !this.tseUnidadeResponsavel.DBValue.IsNull) || (this.tseRegional.IsValidDBValue && !this.tseRegional.DBValue.IsNull))
                {
                    CarregaAno();
                    CarregaTipoProtocolo();
                    pnlDados.Visible = true;
                    pnlGrid.Visible = true;
                    lblCNPJ.Text = (this.tseUnidadeResponsavel.IsValidDBValue) ? tseUnidadeResponsavel["cgc"].ToString().AplicarMascaraCNPJ() : string.Empty;
                    odsProtocolo.DataBind();
                }
                else
                {
                    lblMensagem.Text = "Para efetuar a busca é necessario selecionar uma Regional e/ou Unidade de Ensino.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        private void CarregaAno()
        {
            try
            {
                ddlAno.Items.Clear();
                ddlAno.DataSource = RN.PeriodoLetivo.ListarAnos();
                ddlAno.Items.Insert(0, new ListItem("Selecione", string.Empty));
                ddlAno.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaTipoProtocolo()
        {
            RN.Protocolo.TipoProtocolo rnTipoProtocolo = new Techne.Lyceum.RN.Protocolo.TipoProtocolo();
            try
            {
                ddlTipoProtocolo.Items.Clear();
                ddlTipoProtocolo.DataSource = rnTipoProtocolo.ListaTipoProtocoloAtivo();
                ddlTipoProtocolo.Items.Insert(0, new ListItem("Selecione", string.Empty));
                ddlTipoProtocolo.DataBind();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void ControlaAcessoGrid()
        {
            foreach (GridViewColumn col in grdPrestacao.Columns)
            {
                if (col is GridViewCommandColumn)
                {
                    if (((GridViewCommandColumn)col).CustomButtons["btnEditar"] != null)
                    {
                        ((GridViewCommandColumn)col).CustomButtons["btnEditar"].Visibility =
                            Permission.AllowInsert ? GridViewCustomButtonVisibility.AllDataRows : GridViewCustomButtonVisibility.Invisible;
                    }
                    if (((GridViewCommandColumn)col).CustomButtons["btnDeletar"] != null)
                    {
                        ((GridViewCommandColumn)col).CustomButtons["btnDeletar"].Visibility =
                            Permission.AllowDelete ? GridViewCustomButtonVisibility.AllDataRows : GridViewCustomButtonVisibility.Invisible;
                    }
                }
            }

            ControlaAcesso(grdPrestacao);
        }
        protected void grdPrestacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdPrestacao);
        }

        protected void grdPrestacao_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            btnCancelarAtualizacao.Visible = false;

            string ano = grdPrestacao.GetRowValues(e.VisibleIndex, "ANO").ToString();
            string temporalidade = grdPrestacao.GetRowValues(e.VisibleIndex, "TEMPORALIDADE").ToString();
            string unidadeEnsinoid = grdPrestacao.GetRowValues(e.VisibleIndex, "UNIDADEENSINOID").ToString();
            string programa = grdPrestacao.GetRowValues(e.VisibleIndex, "PROGRAMAPROTOCOLOID").ToString();
            string regionalid = grdPrestacao.GetRowValues(e.VisibleIndex, "REGIONALID").ToString();
            string processo = grdPrestacao.GetRowValues(e.VisibleIndex, "PROCESSO").ToString();
            string numeroFolhas = grdPrestacao.GetRowValues(e.VisibleIndex, "NUMEROFOLHAS").ToString();
            string situacaoProtocoloId = grdPrestacao.GetRowValues(e.VisibleIndex, "SITUACAOPROTOCOLOID").ToString();
            string tipoProtocoloId = grdPrestacao.GetRowValues(e.VisibleIndex, "TIPOPROTOCOLOID").ToString();
            string dataProcesso = grdPrestacao.GetRowValues(e.VisibleIndex, "DATAPROCESSO").ToString();
            string protocoloPrestacaoId = grdPrestacao.GetRowValues(e.VisibleIndex, "PROTOCOLOPRESTACAOID").ToString();

            if (e.ButtonID == "btnEditar")
            {
                LimparTela();
                txtPrestacaoID.Text = protocoloPrestacaoId;
                if (ddlAno.Items.FindByValue(ano.ToString()) != null)
                {
                    ddlAno.SelectedValue = ano;
                }
                rblSemestre.SelectedValue = temporalidade;
                ddlTipoProtocolo.SelectedValue = tipoProtocoloId.ToString();
                CarregaPrograma(Convert.ToInt32(ddlTipoProtocolo.SelectedValue));
                ddlPrograma.SelectedValue = programa.ToString();
                ddlPrograma.Enabled = true;
                string[] inicial = processo.Split('-');
                ddlInicialProcesso.SelectedValue = inicial[0].Length == 1 ? inicial[0] + "-03/" : inicial[0] + "-";
                txtProcesso.Text = processo.Substring(ddlInicialProcesso.SelectedValue.Length, (processo.Length - (ddlInicialProcesso.SelectedValue.Length)));
                txtFolha.Text = numeroFolhas;

                if (!dataProcesso.IsNullOrEmptyOrWhiteSpace())
                {
                    dtProcesso.Date = Convert.ToDateTime(dataProcesso);
                }
                btnSalvarPrestacao.Text = "Atualizar Protocolo";
                btnCancelarAtualizacao.Visible = true;
            }
            if (e.ButtonID == "btnDeletar")
            {
                txtPrestacaoID.Text = protocoloPrestacaoId;
                popup.ShowOnPageLoad = true;
            }
        }
        protected void grdPrestacao_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            RN.Perfil rnPerfil = new Techne.Lyceum.RN.Perfil();

            if (e.VisibleIndex == -1) return;

            if (e.CellType == GridViewTableCommandCellType.Filter)
            {
                e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                return;
            }

            string situacao = Convert.ToString(grdPrestacao.GetRowValues(e.VisibleIndex, "SITUACAO")); //!= DBNull.Value ? (int)grdPrestacao.GetRowValues(e.VisibleIndex, "SITUACAOPROTOCOLOID") : 0;

            if (e.ButtonID == "btnDeletar")
            {
                e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                if (situacao != null)
                {
                    if ((situacao.ToString() == "Aguardando Análise" && Permission.AllowDelete) || hdnPerfilCoord.Value == "S")
                    {
                        e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.True;
                    }
                }
            }
        }

        protected void btnSalvarPrestacao_Click(object sender, EventArgs e)
        {
            try
            {
                RN.Protocolo.ProtocoloPrestacao rnProtocoloPrestacao = new Techne.Lyceum.RN.Protocolo.ProtocoloPrestacao();
                bool cadastro = false;

                var prestacao = new RN.Protocolo.Entidades.ProtocoloPrestacao
                {
                    Ano = !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(this.ddlAno.SelectedValue) : -1,
                    Temporalidade = !rblSemestre.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? this.rblSemestre.SelectedValue : null,
                    UnidadeEnsinoId = (this.tseUnidadeResponsavel.IsValidDBValue && !this.tseUnidadeResponsavel.DBValue.IsNull) ? tseUnidadeResponsavel.DBValue.ToString() : null,
                    RegionalId = (this.tseRegional.IsValidDBValue && !this.tseRegional.DBValue.IsNull) ? Convert.ToInt32(tseRegional.DBValue.ToString()) : -1,
                    Processo = !txtProcesso.Text.Trim().IsNullOrEmptyOrWhiteSpace() && !ddlInicialProcesso.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlInicialProcesso.SelectedValue + txtProcesso.Text.Trim() : null,
                    NumeroFolhas = !txtFolha.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtFolha.Text.Trim()) : ((!ddlInicialProcesso.SelectedValue.IsNullOrEmptyOrWhiteSpace() && ddlInicialProcesso.SelectedValue == "SEI-") ? (int?)null : -1),
                    TipoProtocoloId = !ddlTipoProtocolo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlTipoProtocolo.SelectedValue) : -1,
                    ProgramaProtocoloId = !ddlPrograma.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlPrograma.SelectedValue) : -1,
                    DataProcesso = !string.IsNullOrEmpty(dtProcesso.Text.Trim()) ? dtProcesso.Date : DateTime.MinValue,
                    UsuarioId = User.Identity.Name,
                    UsuarioCadastroId = User.Identity.Name,
                    ProtocoloPrestacaoId = !txtPrestacaoID.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtPrestacaoID.Text) : 0,
                };
                cadastro = prestacao.ProtocoloPrestacaoId == 0;

                var validacao = rnProtocoloPrestacao.Valida(prestacao, cadastro, (!ddlInicialProcesso.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlInicialProcesso.SelectedValue : null));

                if (validacao.Valido)
                {
                    if (cadastro)
                    {
                        rnProtocoloPrestacao.Insere(prestacao);
                        txtPrestacaoID.Text = prestacao.ProtocoloPrestacaoId.ToString();
                    }
                    else
                    {
                        rnProtocoloPrestacao.Atualiza(prestacao);
                    }

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup",
                        "alert('Protocolo " + (cadastro ? " inserido " : " atualizado ") + "com sucesso.');", true);

                    LimparTela();
                    btnSalvarPrestacao.Text = "Incluir Protocolo";
                    grdPrestacao.DataBind();
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

        protected void btnCancelarAtualizacao_Click(object sender, EventArgs e)
        {
            try
            {
                LimparTela();
                btnSalvarPrestacao.Text = "Incluir Protocolo";
                grdPrestacao.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            RN.Protocolo.ProtocoloPrestacao rnProtocoloPrestacao = new Techne.Lyceum.RN.Protocolo.ProtocoloPrestacao();
            ValidacaoDados validacao = new ValidacaoDados();

            try
            {
                int idProtocoloPrestacao = Convert.ToInt32(txtPrestacaoID.Text);

                if (hdnPerfilCoord.Value == "S")
                {
                    rnProtocoloPrestacao.RemoveCoordenador(idProtocoloPrestacao);
                }
                else
                {
                    validacao = rnProtocoloPrestacao.ValidaRemocao(idProtocoloPrestacao);

                    if (validacao.Valido)
                    {
                        rnProtocoloPrestacao.Remove(idProtocoloPrestacao);
                        
                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        return;
                    }
                }

                LimparTela();
                grdPrestacao.DataBind();
                lblMensagem.Text = "Protocolo excluído com sucesso.";
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
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

        protected void ddlTipoProtocolo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlPrograma.Enabled = false;
                if (!ddlTipoProtocolo.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    CarregaPrograma(Convert.ToInt32(ddlTipoProtocolo.SelectedValue));
                    ddlPrograma.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
