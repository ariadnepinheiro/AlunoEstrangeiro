using System;
using System.Web;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Controls;
using Techne.Data;
using Techne.Lyceum.RN.Util;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
    using Techne.Lyceum.RN.Entidades;

    [
     NavUrl("~/Basico/SolicitacaoHabilitacao.aspx"),
      ControlText("SolicitacaoHabilitacao"),
      Title("Solicitação de Habilitação"),
    ]

    public partial class SolicitacaoHabilitacao : TPage
    {
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

        protected string CodFuncao
        {
            get
            {
                return ViewState["codFuncao"].ToString();
            }
            set
            {
                ViewState["codFuncao"] = value;
            }
        }

        protected bool EmAula
        {
            get
            {
                return (bool)ViewState["emAula"];
            }
            set
            {
                ViewState["emAula"] = value;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {

            TituloGrid(grdSolHabilitacao, "Solicitações de Habilitação");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                btnSalvar.Attributes.Add("onClick", "javascript:return confirm('Declaro que a documentação do docente foi analisada e validada pela Inspeção Escolar e arquivada na Diretoria Regional.');");
                if (!IsPostBack)
                {
                    ListItem itemVazio = new ListItem("Selecione", string.Empty);

                    if (!RN.Usuarios.UsuarioPrivilegiado(HttpContext.Current.User.Identity.Name))
                    {
                        tseUnidadeAdministrativa.SqlWhere = " uuf.USUARIO = '" + HttpContext.Current.User.Identity.Name + "'";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdSolHabilitacao);
        }

        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        protected void grdSolHabilitacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdSolHabilitacao);
        }

        public object Listar(object setor, object status)
        {
            QueryTable qt = null;

            var status_sel = String.IsNullOrEmpty((string)status) ? null : (string)status;
            qt = RN.SolicitacaoHabilitacaoDocente.ListarPorUA(setor.ToString(), status_sel);
            return qt;
        }

        protected void tseUnidadeAdministrativa_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                Limpar();
                hdnUnidadeAdministrativa.Value = string.Empty;

                if (tseUnidadeAdministrativa.IsValidDBValue && !tseUnidadeAdministrativa.DBValue.IsNull)
                {
                    grdSolHabilitacao.Visible = true;
                    pnPrincipal.Visible = true;
                    pnGrid.Visible = true;

                    //Busca codigo do setor da undiade administrativa
                    hdnUnidadeAdministrativa.Value = tseUnidadeAdministrativa["SETOR"].ToString();
                }
                else if (!tseUnidadeAdministrativa.DBValue.IsNull)
                {
                    grdSolHabilitacao.Visible = false;
                    pnPrincipal.Visible = false;
                    lblMensagem.Text = "Unidade Administrativa não cadastrada.";
                    pnGrid.Visible = false;
                }
                else
                {
                    grdSolHabilitacao.Visible = false;
                    pnPrincipal.Visible = false;
                    lblMensagem.Text = "Favor consultar uma unidade administrativa.";
                    pnGrid.Visible = false;
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseDocente_Changed(object sender, EventArgs e)
        {
            try
            {
                if (tseDocente.IsValidDBValue & !tseDocente.DBValue.IsNull)
                {
                    QueryTable qt = null;

                    qt = RN.Docentes.ConsultarDadosDocente(tseDocente["num_func"].ToString());

                    if (qt == null || qt.Rows.Count == 0)
                        return;

                    SimpleRow rowDocente = qt.Rows[0];

                    txtCargo.Text = Convert.ToString(rowDocente["cargo"]);

                    if (Convert.ToString(rowDocente["em_aula"]) == "SIM")
                    {
                        txtFuncao.Text = Convert.ToString(rowDocente["funcao"]) + " - Regente";
                        this.EmAula = true;
                    }
                    else
                    {
                        txtFuncao.Text = Convert.ToString(rowDocente["funcao"]);
                        this.EmAula = false;
                    }

                    txtDiscIngresso.Text = Convert.ToString(rowDocente["disciplina"]);
                    this.CodFuncao = Convert.ToString(rowDocente["codFuncao"]);

                    QueryTable qtSituacao = null;

                    qtSituacao = RN.LicencaDocente.ConsultarLicencas(int.Parse(tseDocente["num_func"].ToString()));

                    if (qtSituacao.Rows.Count > 0)
                    {
                        SimpleRow rowSituacao = qtSituacao.Rows[0];
                        txtSituacao.Text = Convert.ToString(rowSituacao["MOTIVO_LICENCA"]);

                        var situacao = txtSituacao.Text.Trim(); //situação do professor
                        if (!string.IsNullOrEmpty(situacao)) //verificar se o prof esta em situação ou nao
                        {
                            var inicio = Convert.ToString(rowSituacao["dtini"]); //inicio da situação do professor
                            var fim = Convert.ToString(rowSituacao["dtfim"]); //fim da situação do professor

                            //se estiver com licença vigente, afastamento não pode se cadastrar
                            lblMensagem.Text = string.Format("Docente possui situação de {0} no período de {1} à {2}. Por este motivo a solicitação não poderá ser efetuada.",
                                situacao,
                                inicio,
                                fim);
                            Limpar();
                        }
                    }
                    else
                    {
                        txtSituacao.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void Limpar()
        {
            tseDocente.ResetValue();
            txtCargo.Text = string.Empty;
            txtFuncao.Text = string.Empty;
            txtSituacao.Text = string.Empty;
            txtDiscIngresso.Text = string.Empty;
            ddlCategoriaCurso.ClearSelection();
            tseAgrupamento.ResetValue();
            cblHabilitar.ClearSelection();
            TSDocenteSubst.ResetValue();
            TSDocenteSubst.Enabled = false;
            rblTipoSubstituicao.Enabled = false;
            rblTipoSubstituicao.ClearSelection();
            pnlDocenteSubs.Visible = false;
            pnlTipoSubs.Visible = false;
            this.CodFuncao = string.Empty;
            this.EmAula = false;
        }

        private void CarregarDropDownList(DropDownList drop, QueryTable data, string defaultValue)
        {
            drop.Items.Clear();
            drop.DataSource = data;
            drop.DataBind();

            if (drop.Items.Count == 0)
            {
                ListItem itemVazio = new ListItem("Lista Vazia", string.Empty);
                drop.Items.Add(itemVazio);
            }
            else
            {
                ListItem item = new ListItem("Selecione", string.Empty);
                drop.Items.Add(item);
                drop.SelectedValue = "";
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Session["Mensagem"] = null;

                if (!tseDocente.IsValidDBValue || tseDocente.DBValue.IsNull)
                {
                    lblMensagem.Text = "Favor selecionar Docente.";
                    return;
                }

                if (!tseUnidade_Ensino.IsValidDBValue || tseUnidade_Ensino.DBValue.IsNull)
                {
                    lblMensagem.Text = "Favor selecionar Unidade de Ensino.";
                    return;
                }

                if (string.IsNullOrEmpty(ddlCategoriaCurso.SelectedValue))
                {
                    lblMensagem.Text = "Favor selecionar o segmento de atuação.";
                    return;
                }

                if (!tseAgrupamento.IsValidDBValue || tseAgrupamento.DBValue.IsNull)
                {
                    lblMensagem.Text = "Favor selecionar Disciplina para Habilitar.";
                    return;
                }
                if (string.IsNullOrEmpty(cblHabilitar.SelectedValue))
                {
                    lblMensagem.Text = "Favor selecionar a opção para Habilitar.";
                    return;
                }
                if (cblHabilitar.Items[0].Selected)
                {
                    if (this.CodFuncao == "108" || this.CodFuncao == "109" || this.CodFuncao == "10001")
                    {
                        if (this.EmAula)
                        {
                            if (string.IsNullOrEmpty(rblTipoSubstituicao.SelectedValue))
                            {
                                lblMensagem.Text = "Favor selecionar a Tipo de Substituição.";
                                return;
                            }

                            if (rblTipoSubstituicao.SelectedValue != "CT")
                            {
                                if (!TSDocenteSubst.IsValidDBValue || TSDocenteSubst.DBValue.IsNull)
                                {
                                    lblMensagem.Text = "Favor selecionar a Matrícula que irá substituir.";
                                    return;
                                }
                            }
                        }
                    }
                }

                TceSolicitacaoHabilitacaoDocente TSHD = new TceSolicitacaoHabilitacaoDocente();
                var validacao = new ValidacaoDados();

                TSHD.UnidadeEns = tseUnidade_Ensino.DBValue.ToString();
                TSHD.NumFunc = Convert.ToDecimal(tseDocente["num_func"]);
                TSHD.SegmentoAtuacao = ddlCategoriaCurso.SelectedValue;
                TSHD.Agrupamento = tseAgrupamento.DBValue.ToString();

                TSHD.HabilitacaoMatricula = false;
                TSHD.NumFuncSubstituido = null;
                TSHD.TipoSubstituicao = string.Empty;

                if (cblHabilitar.Items[0].Selected)
                {
                    TSHD.HabilitacaoMatricula = true;

                    if (this.CodFuncao == "108" || this.CodFuncao == "109" || this.CodFuncao == "10001")
                    {
                        if (this.EmAula)
                        {
                            TSHD.TipoSubstituicao = rblTipoSubstituicao.SelectedValue;
                            if (rblTipoSubstituicao.SelectedValue != "CT")
                            {
                                TSHD.NumFuncSubstituido = Convert.ToDecimal(TSDocenteSubst["num_func"]);
                            }
                        }
                    }
                }

                if (cblHabilitar.Items[1].Selected)
                {
                    TSHD.HabilitacaoGLP = true;
                }
                else
                {
                    TSHD.HabilitacaoGLP = false;
                }

                validacao = RN.SolicitacaoHabilitacaoDocente.Validar(TSHD);

                if (validacao.Valido)
                {
                    var retorno = RN.SolicitacaoHabilitacaoDocente.Incluir(TSHD);

                    if (retorno != null)
                    {
                        if (!retorno.Ok)
                        {
                            lblMensagem.Text = retorno.Errors.ToString();
                        }
                        else
                        {
                            lblMensagem.Text = "Pedido incluído com sucesso.";
                            Limpar();
                            odsSolicitacaoHabilitacao.Select();
                            odsSolicitacaoHabilitacao.DataBind();
                            grdSolHabilitacao.DataBind();
                        }
                    }
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdSolHabilitacao_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            if (Session["Mensagem"] == null)
                e.Properties["cpMessage"] = String.Empty;
            else
            {
                e.Properties["cpMessage"] = Session["Mensagem"].ToString();
                Session["Mensagem"] = null;
            }
        }

        public void Delete(object ID_SOLICITACAO_HABILITACAO_DOCENTE) { }

        protected void odsSolicitacaoHabilitacao_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            RN.RetValue retorno = null;
            int id_solicitacao = int.Parse(e.InputParameters["ID_SOLICITACAO_HABILITACAO_DOCENTE"].ToString());
            retorno = RN.SolicitacaoHabilitacaoDocente.Remover(id_solicitacao);
            if (retorno != null)
            {
                if (!retorno.Ok)
                    throw new Exception(retorno.Errors.ToString());
            }
            else
            {
                odsSolicitacaoHabilitacao.Select();
                odsSolicitacaoHabilitacao.DataBind();
                grdSolHabilitacao.DataBind();
            }
        }

        protected void tseAgrupamento_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (tseAgrupamento.IsValidDBValue && !tseAgrupamento.DBValue.IsNull)
                {
                    lblMensagem.Text = string.Empty;
                }
                else if (!tseAgrupamento.DBValue.IsNull)
                {
                    lblMensagem.Text = "Grupo não cadastrado.";
                }
                else
                {
                    lblMensagem.Text = "Favor consultar um grupo.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(cmbStatus.SelectedValue))
                {
                    odsSolicitacaoHabilitacao.Select();
                    odsSolicitacaoHabilitacao.DataBind();
                    grdSolHabilitacao.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void cblHabilitar_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                TSDocenteSubst.Enabled = false;
                TSDocenteSubst.ResetValue();
                rblTipoSubstituicao.Enabled = false;
                rblTipoSubstituicao.ClearSelection();
                pnlTipoSubs.Visible = false;
                pnlDocenteSubs.Visible = false;
                if (cblHabilitar.Items[0].Selected)
                {
                    //verificar se eh doc II
                    if (this.CodFuncao == "108" || this.CodFuncao == "109" || this.CodFuncao == "10001")
                    {
                        if (this.EmAula)
                        {
                            rblTipoSubstituicao.Enabled = true;
                            pnlTipoSubs.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rblTipoSubstituicao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                TSDocenteSubst.Enabled = false;
                TSDocenteSubst.ResetValue();
                pnlDocenteSubs.Visible = false;

                if (!string.IsNullOrEmpty(rblTipoSubstituicao.SelectedValue))
                {
                    if (rblTipoSubstituicao.SelectedValue != "CT")
                    {
                        pnlDocenteSubs.Visible = true;
                        TSDocenteSubst.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdSolHabilitacao_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            var status = grdSolHabilitacao.GetRowValues(e.VisibleIndex, "STATUS").ToString();
            var isVisible = status == "Aguardando" && e.ButtonType == ColumnCommandButtonType.Delete;

            e.Visible = isVisible;
        }
    }
}
