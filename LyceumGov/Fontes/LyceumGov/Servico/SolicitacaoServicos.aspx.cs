using System;
using System.Data;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.Data;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Web;
using System.Collections.Generic;
using System.Web.UI;
using DevExpress.Web.ASPxEditors;

namespace Techne.Lyceum.Net.Servico
{
    [NavUrl("~/Servico/SolicitacaoServicos.aspx"),
     ControlText("SolicitacaoServicos"),
     Title("Solicitações / Consulta de Andamento")]
    public partial class SolicitacaoServicos : TPage
    {
        //private string usuarioLogado;
        //private decimal? num_func_usuarioLogado;

        protected void Servico_Callback(object source, CallbackEventArgsBase e)
        {
            FillMotivoCombo(source as ASPxComboBox, e.Parameter);
        }

        protected void FillMotivoCombo(ASPxComboBox source, string servico)
        {
            if (string.IsNullOrEmpty(servico)) return;

            source.Items.Clear();
            this.tdsMotivos.SqlWhereParameters[0].DefaultValue = (servico == "1VIACARTAO" ? "1" : "0");
            DataView view = (DataView)tdsMotivos.Select();
            for (int i = 0; i < view.Count; i++)
            {
                source.Items.Add(new ListEditItem((string)view[i][2], (string)view[i][1]));
            }

        }

        protected readonly string UpdateError = string.Empty;

        public string Controle
        {
            get
            {
                return (string)this.ViewState["controle"];
            }

            set
            {
                this.ViewState["controle"] = value;
            }
        }

        public static string GetUrl()
        {
            return Navigation.GetNavigation(System.Reflection.MethodBase.GetCurrentMethod()).GetUrl(new object[] { });
        }

        public void Delete(decimal solicitacao)
        {
        }

        public void Insert(DbObject tseAluno, string servico, string obs, string operadora)
        {
        }

        public object Listar(DbObject tseAluno)
        {
            QueryTable qt = null;

            if (!tseAluno.IsNull)
            {
                qt = RN.Servico.ConsultarServicos(tseAluno.ToString());
            }

            return qt;
        }

        public object Listar2(decimal solicit, decimal item, string servico)
        {
            QueryTable qt = null;

            if (solicit > 0 && item > 0)
            {
                qt = RN.Servico.ConsultarAndamentos(solicit, item, servico);
            }

            return qt;
        }

        public void Update()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdServicos, "Solicitações");
            TituloGrid(this.grdAndamento, "Andamento");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var valores = this.ObtemValores();

            ASPxWebControl.RegisterBaseScript(this.Page);

            if (!this.IsPostBack && !this.IsCallback)
            {
                this.grdServicos.FocusedRowIndex = 0;
            }
            else
            {
                this.grdServicos.FocusedRowIndex = this.grdServicos.FocusedRowIndex;

                var solicit = Convert.ToDecimal(valores[0]);
                var item = Convert.ToDecimal(valores[1]);
                var servico = Convert.ToString(valores[2]);

                if (solicit > 0 && item > 0 && !string.IsNullOrEmpty(servico))
                {
                    this.odsAndamentos.SelectParameters[0].DefaultValue = valores[0].ToString();
                    this.odsAndamentos.SelectParameters[1].DefaultValue = valores[1].ToString();
                    this.odsAndamentos.SelectParameters[2].DefaultValue = valores[2].ToString();
                    this.lblServico.Text = "Solicitação selecionada: " + valores[0];
                }
                else
                {
                    this.lblServico.Text = "Solicitação selecionada: ";
                }

                this.odsAndamentos.Select();
                this.grdAndamento.DataBind();
            }

            if (!this.grdServicos.IsNewRowEditing && !this.grdServicos.IsEditing)
            {
                this.grdAndamento.Columns[string.Empty].Visible = true;
            }

            if (!this.grdAndamento.IsNewRowEditing && !this.grdAndamento.IsEditing)
            {
                this.grdServicos.Columns[string.Empty].Visible = true;
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            this.ControlaAcesso(this.grdServicos);
            this.ControlaAcesso(this.grdAndamento);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            var valores = this.ObtemValores();
            var solicit = Convert.ToDecimal(valores[0]);
            var item = Convert.ToDecimal(valores[1]);
            var servico = Convert.ToString(valores[2]);
            var comentario = this.txtComentario.Text;

            if (solicit > 0 && item > 0 && !string.IsNullOrEmpty(servico))
            {
                var usuario = this.User.Identity.Name;
                var retorno = RN.Servico.CancelarAndamento(solicit, item, servico, usuario, comentario);

                if (retorno != null)
                {
                    if (!retorno.Ok)
                    {
                        this.lblMensagem2.Text = retorno.Errors.ToString();
                    }
                    else
                    {
                        this.lblMensagem2.Text = retorno.Message;
                        this.odsAndamentos.SelectParameters[0].DefaultValue = valores[0].ToString();
                        this.odsAndamentos.SelectParameters[1].DefaultValue = valores[1].ToString();
                        this.odsAndamentos.SelectParameters[2].DefaultValue = valores[2].ToString();
                        this.odsAndamentos.Select();
                        this.odsAndamentos.DataBind();
                        this.grdAndamento.DataBind();
                        this.odsSolicitacoes.Select();
                        this.odsSolicitacoes.DataBind();
                        this.grdServicos.DataBind();
                    }
                }
            }
            else
            {
                this.lblMensagem2.Text = "Selecione uma solicitação.";
            }
        }

        protected void btnDar_Click(object sender, EventArgs e)
        {
            var valores = this.ObtemValores();
            var solicit = Convert.ToDecimal(valores[0]);
            var item = Convert.ToDecimal(valores[1]);
            var servico = Convert.ToString(valores[2]);

            if (solicit > 0 && item > 0 && !string.IsNullOrEmpty(servico))
            {
                var usuario = this.User.Identity.Name;
                var retorno = RN.Servico.DarAndamento(solicit, item, servico, usuario);

                if (retorno != null)
                {
                    if (!retorno.Ok)
                    {
                        this.lblMensagem2.Text = retorno.Errors.ToString();
                    }
                    else
                    {
                        this.lblMensagem2.Text = retorno.Message;

                        this.odsAndamentos.SelectParameters[0].DefaultValue = valores[0].ToString();
                        this.odsAndamentos.SelectParameters[1].DefaultValue = valores[1].ToString();
                        this.odsAndamentos.SelectParameters[2].DefaultValue = valores[2].ToString();

                        this.odsAndamentos.Select();
                        this.odsAndamentos.DataBind();

                        this.grdAndamento.DataBind();

                        this.odsSolicitacoes.Select();
                        this.odsSolicitacoes.DataBind();

                        this.grdServicos.DataBind();
                    }
                }
            }
            else
            {
                this.lblMensagem2.Text = "Selecione uma solicitação.";
            }
        }

        protected void grdAndamento_CancelRowEditing(object sender, ASPxStartRowEditingEventArgs e)
        {
            this.grdServicos.Columns[string.Empty].Visible = true;
        }

        protected void grdAndamento_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
        }

        protected void grdAndamento_RowDeleted(object sender, ASPxDataDeletedEventArgs e)
        {
            this.grdServicos.Columns[string.Empty].Visible = true;
        }

        protected void grdAndamento_RowInserted(object sender, ASPxDataInsertedEventArgs e)
        {
            this.grdServicos.Columns[string.Empty].Visible = true;
        }

        protected void grdAndamento_RowUpdated(object sender, ASPxDataUpdatedEventArgs e)
        {
            this.grdServicos.Columns[string.Empty].Visible = true;
        }

        protected void grdAndamento_StartRowEditing(object sender, ASPxStartRowEditingEventArgs e)
        {
            this.grdAndamento.Settings.ShowFilterRow = false;
            this.grdServicos.Columns[string.Empty].Visible = false;
        }

        protected void grdServicos_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            this.ControlaAcesso(this.grdServicos);
        }

        protected void grdServicos_CancelRowEditing(object sender, ASPxStartRowEditingEventArgs e)
        {
            this.grdAndamento.Columns[string.Empty].Visible = true;
        }

        protected void grdServicos_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpUpdateError"] = this.UpdateError;
        }

        protected void grdServicos_FocusedRowChanged(object sender, EventArgs e)
        {
            this.grdServicos.FocusedRowIndex = ((ASPxGridView)sender).FocusedRowIndex;

            var valores = this.ObtemValores();

            this.txtServico.Text = valores[2].ToString();

            this.odsAndamentos.SelectParameters[0].DefaultValue = valores[0].ToString();
            this.odsAndamentos.SelectParameters[1].DefaultValue = valores[1].ToString();
            this.odsAndamentos.SelectParameters[2].DefaultValue = valores[2].ToString();

            this.grdAndamento.ExpandAll();

            this.odsAndamentos.Select();

            this.grdAndamento.DataBind();

            this.grdServicos.CancelEdit();

            if (valores[0].ToString() != "0")
            {
                this.lblServico.Text = "Solicitação selecionada: " + valores[0];
            }
            else
            {
                this.lblServico.Text = "Solicitação selecionada: ";
            }
        }

        protected void grdServicos_InitNewRow(object sender, ASPxDataInitNewRowEventArgs e)
        {

            this.grdServicos.Settings.ShowFilterRow = false;

            this.grdAndamento.Columns[string.Empty].Visible = false;
            this.grdAndamento.CancelEdit();

            this.lblMensagem2.Text = string.Empty;
        }

        protected void grdServicos_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            var aluno = this.tseAluno.DBValue.ToString();

            if (grdServicos.IsNewRowEditing)
            {
                bool verificaMunicipioSalineiras = RN.Servico.VerificaMunicipioSalineiras(aluno);

                if (e.Column.FieldName == "operadora")
                {
                    e.Editor.Visible = verificaMunicipioSalineiras;
                    e.Column.Visible = verificaMunicipioSalineiras;
                    HabilitaDesabilita(this.grdServicos, "lblSalineiras", verificaMunicipioSalineiras, typeof(Label));
                }
            }

            if (!grdServicos.IsEditing || e.Column.FieldName != "obs") return;

            DevExpress.Web.ASPxEditors.ASPxComboBox combo = e.Editor as DevExpress.Web.ASPxEditors.ASPxComboBox;

            combo.Callback += new CallbackEventHandlerBase(Servico_Callback);
        }

        protected void grdServicos_RowDeleted(object sender, ASPxDataDeletedEventArgs e)
        {
            this.odsAndamentos.SelectParameters[0].DefaultValue = "0";
            this.odsAndamentos.SelectParameters[1].DefaultValue = "0";
            this.odsAndamentos.SelectParameters[2].DefaultValue = "0";

            this.lblServico.Text = "Solicitação selecionada: ";

            this.odsAndamentos.Select();

            this.grdAndamento.DataBind();

            this.grdAndamento.Columns[string.Empty].Visible = true;
            this.lblMensagem2.Text = string.Empty;
            this.grdServicos.FocusedRowIndex = -1;
        }

        protected void grdServicos_RowInserted(object sender, ASPxDataInsertedEventArgs e)
        {
            var valores = this.ObtemValores();

            this.txtServico.Text = valores[2].ToString();

            this.odsAndamentos.SelectParameters[0].DefaultValue = valores[0].ToString();
            this.odsAndamentos.SelectParameters[1].DefaultValue = valores[1].ToString();
            this.odsAndamentos.SelectParameters[2].DefaultValue = valores[2].ToString();

            this.odsAndamentos.Select();

            this.grdAndamento.DataBind();

            if (valores[0].ToString() != "0")
            {
                this.lblServico.Text = "Solicitação selecionada: " + valores[0];
            }
            else
            {
                this.lblServico.Text = "Solicitação selecionada: ";
            }

            this.grdAndamento.Columns[string.Empty].Visible = true;
            this.lblMensagem2.Text = string.Empty;
            this.grdServicos.FocusedRowIndex = 0;
        }

        protected void grdServicos_RowUpdated(object sender, ASPxDataUpdatedEventArgs e)
        {
            this.grdAndamento.Columns[string.Empty].Visible = true;
            this.grdServicos.FocusedRowIndex = 0;
        }

        protected void grdServicos_RowValidating(object sender, ASPxDataValidationEventArgs e)
        {
            if (e.NewValues["servico"] != null
                && e.NewValues["servico"].ToString() != string.Empty)
            {
                var aluno = this.tseAluno.DBValue.ToString();
                var pessoa = this.tseAluno["pessoa"].ToString();
                var errosNoCadastro = Aluno.ValidaDadosParaCartao(aluno, pessoa);

                //Validação para saber se o aluno é riocard
                if (Convert.ToString(e.NewValues["operadora"]) != "SIM")
                {
                    e.RowError = "Solicitações para a Riocard deverão ser feitas pelo aluno diretamente no site da operadora.";

                    return;
                }


                if (!string.IsNullOrEmpty(errosNoCadastro))
                {
                    e.RowError = errosNoCadastro;

                    return;
                }

                // Validações para a primeira via
                if (e.NewValues["servico"].ToString().ToUpper() == RN.Servico.PrimeiraVia)
                {
                    if (RN.Servico.ExistePrimeiraViaCartao(aluno))
                    {
                        e.RowError = "Já existe uma solicitação de primeira via concluída.";

                        return;
                    }

                    if (RN.Servico.ExisteSolicitacaoPendente(aluno, RN.Servico.PrimeiraVia))
                    {
                        e.RowError = "Já existe uma solicitação de primeira via pendente.";

                        return;
                    }
                }

                // Validações para a segunda via
                if (e.NewValues["servico"].ToString().ToUpper() == RN.Servico.SegundaVia)
                {
                    // Verifica se já possui primeira via
                    if (!RN.Servico.ExistePrimeiraViaCartao(aluno))
                    {
                        e.RowError = "Não é possível solicitar segunda via do cartão sem ter uma solicitação de primeira via ou se a solicitação de primeira via não estiver concluída.";

                        return;
                    }

                    if (RN.Servico.ExisteSolicitacaoPendente(aluno, RN.Servico.SegundaVia))
                    {
                        e.RowError = "Já existe uma solicitação de segunda via pendente.";

                        return;
                    }
                }
            }
        }

        protected void grdServicos_StartRowEditing(object sender, ASPxStartRowEditingEventArgs e)
        {
            this.grdServicos.Settings.ShowFilterRow = false;
            this.grdAndamento.Columns[string.Empty].Visible = false;
            this.grdAndamento.CancelEdit();
        }

        protected void grdServicos_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            ((ASPxGridView)sender).Columns["operadora"].Visible = RN.Servico.VerificaMunicipioSalineiras(tseAluno.DBValue.ToString());
            (((ASPxGridView)sender).Columns["operadora"] as GridViewDataComboBoxColumn).PropertiesComboBox.ValidationSettings.RequiredField.IsRequired = RN.Servico.VerificaMunicipioSalineiras(tseAluno.DBValue.ToString());
        }

        protected void odsSolicitacoes_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var id = Convert.ToDecimal(e.InputParameters["solicitacao"]);
            var retorno = RN.Servico.Excluir(id);

            if (retorno != null)
            {
                if (!retorno.Ok)
                {
                    this.lblMensagem2.Text = string.Empty;
                    throw new Exception("Não foi possível excluir a solicitação.\n" + retorno.Errors);
                }
            }
        }

        public class OperadoraCartao
        {
            public string OperadoraId { get; set; }
            public string OperadoraDs { get; set; }
        }

        public IEnumerable<OperadoraCartao> ListarOperadoras()
        {
            var listaOperadoras = new List<OperadoraCartao>();

            listaOperadoras.Add(new OperadoraCartao { OperadoraDs = "SIM", OperadoraId = "SIM" });
            listaOperadoras.Add(new OperadoraCartao { OperadoraDs = "NÃO", OperadoraId = "NÃO" });

            return listaOperadoras;
        }

        protected void odsSolicitacoes_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var solicitacao = string.Empty;
            var aluno = e.InputParameters["tseAluno"].ToString();
            var servico = e.InputParameters["servico"].ToString();
            var obs = e.InputParameters["obs"].ToString();
            var operadoraId = e.InputParameters["operadora"] == null ? "1" : e.InputParameters["operadora"].ToString() == "SIM" ? "2" : "1";

            var retorno = RN.Servico.IncluirSolicitacao(aluno, servico, obs, ref solicitacao, operadoraId, User.Identity.Name);

            this.tdsMotivos.SqlWhereParameters[0].DefaultValue = null;
            tdsMotivos.DataBind();

            if (retorno != null)
            {
                if (!retorno.Ok)
                {
                    throw new Exception(retorno.Errors.ToString());
                }
            }

            this.lblMensagem2.Text = string.Empty;

            this.lblServico.Text = "Solicitação selecionada: " + solicitacao;
            this.txtServico.Text = servico;

            this.odsSolicitacoes.Select();
            this.odsSolicitacoes.DataBind();

            this.grdServicos.DataBind();

            this.odsAndamentos.SelectParameters[0].DefaultValue = "0";
            this.odsAndamentos.SelectParameters[1].DefaultValue = "0";
            this.odsAndamentos.SelectParameters[2].DefaultValue = "0";

            this.odsAndamentos.Select();

            this.grdAndamento.DataBind();
        }

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            this.grdAndamento.CancelEdit();
            this.grdServicos.CancelEdit();
            lblMensagem.Text = string.Empty;

            if (!this.tseAluno.DBValue.IsNull)
            {
                if (this.tseAluno.IsValidDBValue)
                {
                    if (!Aluno.ExisteUtilizaTransporte(tseAluno.DBValue.ToString()))
                    {
                        lblMensagem.Text = "É permitido solicitar cartão do estudante somente para alunos que possuem a marcação \"Sim\" no item \"Utiliza Transporte?\" disponível na Tela de cadastro do aluno.";
                        uppSolicitacao.Visible = false;
                        return;
                    }
                    uppSolicitacao.Visible = true;

                    this.grdServicos.FocusedRowIndex = 0;
                    this.lblMensagem2.Text = string.Empty;
                    this.pnGrid.Visible = true;

                    var valores = this.ObtemValores();
                    var solicit = Convert.ToDecimal(valores[0]);
                    var item = Convert.ToDecimal(valores[1]);
                    var servico = Convert.ToString(valores[2]);

                    if (solicit > 0 && item > 0 && !string.IsNullOrEmpty(servico))
                    {
                        this.odsAndamentos.SelectParameters[0].DefaultValue = valores[0].ToString();
                        this.odsAndamentos.SelectParameters[1].DefaultValue = valores[1].ToString();
                        this.odsAndamentos.SelectParameters[2].DefaultValue = valores[2].ToString();
                        this.lblServico.Text = "Solicitação selecionada: " + valores[0];
                    }
                    else
                    {
                        this.odsAndamentos.SelectParameters[0].DefaultValue = "0";
                        this.odsAndamentos.SelectParameters[1].DefaultValue = "0";
                        this.odsAndamentos.SelectParameters[2].DefaultValue = "0";
                        this.lblServico.Text = "Solicitação selecionada: ";
                    }

                    this.odsAndamentos.Select();
                    this.grdAndamento.DataBind();
                }
                else
                {
                    this.lblMensagem2.Text = "Aluno não possui pré-requisitos necessários para solicitação de serviços (favor verificar).";
                    this.pnGrid.Visible = false;
                }
            }
            else
            {
                this.lblMensagem2.Text = "Aluno não possui pré-requisitos necessários para solicitação de serviços (favor verificar).";
                this.pnGrid.Visible = false;
            }
        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            var startIndexOnPage = this.grdServicos.PageIndex * this.grdServicos.SettingsPager.PageSize;

            for (var i = 0; i < this.grdServicos.VisibleRowCount; i++)
            {
                if (this.grdServicos.FocusedRowIndex == startIndexOnPage + i)
                {
                    return startIndexOnPage + i;
                }
            }

            return -1;
        }

        private void InitializeComponent()
        {
        }

        private object[] ObtemValores()
        {
            // obtém o indice atual da seleção
            var index = this.GetSelectedRowOnTheCurrentPage();

            var solicit = Convert.ToDecimal(this.grdServicos.GetRowValues(this.grdServicos.FocusedRowIndex, "solicitacao"));
            var item = Convert.ToDecimal(this.grdServicos.GetRowValues(this.grdServicos.FocusedRowIndex, "item"));
            var servico = Convert.ToString(this.grdServicos.GetRowValues(this.grdServicos.FocusedRowIndex, "servico"));

            return new object[] { solicit, item, servico };
        }

        protected void HabilitaDesabilita(Control parent, string objeto, bool visivel, Type tipo)
        {
            foreach (Control ctrControl in parent.Controls)
            {
                if (object.ReferenceEquals(ctrControl.GetType(), tipo))
                {
                    if ((ctrControl).ID == objeto)
                    {
                        (ctrControl).Visible = visivel;
                    }
                }
                if (ctrControl.Controls.Count > 0)
                {
                    HabilitaDesabilita(ctrControl, objeto, visivel, tipo);
                }
            }
        }
    }
}