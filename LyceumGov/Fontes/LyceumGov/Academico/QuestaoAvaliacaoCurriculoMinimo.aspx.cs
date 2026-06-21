using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using Techne.Web;

namespace Techne.Lyceum.Net.Academico
{
    using Techne.Lyceum.RN.Entidades;
    [
NavUrl("~/Academico/QuestaoAvaliacaoCurriculoMinimo.aspx"),
 ControlText("QuestaoAvaliacaoCurriculoMinimo"),
 Title("Questões para Avaliação Currículo Minimo"),
]
    public partial class QuestaoAvaliacaoCurriculoMinimo : TPage
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

        protected void Page_Init(object sender, EventArgs e)
        {

            TituloGrid(grdAvaliacao, "Avaliação Currículo Mínimo");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
            ValidarCampos();
            if (!IsPostBack)
            {
                ddlAno.DataSource = RN.SubperiodoLetivo.ListarAnosPeriodo();
                ddlAno.Items.Insert(0, "Selecione");
                ddlAno.DataBind();
            }
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdAvaliacao);
        }

        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {

            ddlBimestre.Items.Clear();
            txtOrdem.Text = string.Empty;
            txtQuestao.Text = string.Empty;


            if (ddlAno.SelectedValue != "Selecione")
            {
                ddlBimestre.DataSource = RN.SubperiodoLetivo.ListarBimestresAvaliacao(ddlAno.SelectedValue.Substring(0, 4), ddlAno.SelectedValue.Substring(7, 1));
                ddlBimestre.Items.Insert(0, "Selecione");
                ddlBimestre.DataBind();

                //odsAvaliacao.Select();
                //odsAvaliacao.DataBind();
                grdAvaliacao.DataBind();
            }

        }

        protected void btnSalvarAvaliacao_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarAvaliacao())
                {
                    TceAvaliacaoCurriculoMinimo TACM = new TceAvaliacaoCurriculoMinimo();
                    var validacao = new ValidacaoDados();

                    TACM.Ano = int.Parse(ddlAno.SelectedValue.Substring(0, 4).ToString());
                    TACM.Periodo = int.Parse(ddlAno.SelectedValue.Substring(7, 1).ToString());
                    TACM.Subperiodo = int.Parse(ddlBimestre.SelectedValue.ToString());
                    TACM.Avaliacao = txtQuestao.Text.Trim();
                    TACM.Ordem = int.Parse(txtOrdem.Text.Trim());
                    TACM.Habilitado = rblStatus.SelectedValue == "1" ? true : false; 
                    TACM.Matricula = User.Identity.Name.ToString();


                    validacao = RN.AvaliacaoCurriculoMinimo.Validar(TACM);

                    if (validacao.Valido)
                    {
                        if (RN.AvaliacaoCurriculoMinimo.Inserir(TACM) > 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Questão para avaliação incluída com sucesso.');", true);

                            LimparCampos(); 

                           odsAvaliacao.Select();
                           odsAvaliacao.DataBind();
                           grdAvaliacao.DataBind();

                        }

                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "ERRO:" + ex.Message;
            }
        }

        private bool ValidarAvaliacao()
        {

            if (ddlAno.SelectedValue == "Selecione")
            {
                lblMensagem.Text = "Favor selecionar o ano.";
                ddlAno.Focus();
                return false;
            }
            if (ddlBimestre.SelectedValue == "Selecione")
            {
                lblMensagem.Text = "Favor selecionar o Bimestre.";
                ddlBimestre.Focus();
                return false;
            }
          
            if (string.IsNullOrEmpty(txtOrdem.Text.Trim()))
            {
                lblMensagem.Text = "Favor digitar a Ordem.";
                txtOrdem.Focus();
                return false;
            }
            if (txtOrdem.Text.Trim() == "0")
            {
                lblMensagem.Text = "O campo Ordem não pode ser igual a zero(0).";
                txtOrdem.Focus();
                return false;
            }          
           

            if (string.IsNullOrEmpty(txtQuestao.Text.Trim()))
            {
                lblMensagem.Text = "Favor digitar a Questão.";
                txtQuestao.Focus();
                return false;
            }
            return true;
        }

        private void ValidarCampos()
        {
            txtOrdem.Attributes.Add("onkeyPress", "return onlyNumbers(event, this);");
            txtOrdem.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
            txtOrdem.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");

        }

        public object Listar(object ano)
        {
            if (ano.ToString() != "Selecione" )
                return RN.AvaliacaoCurriculoMinimo.Listar(decimal.Parse(ano.ToString().Substring(0, 4)), decimal.Parse(ano.ToString().Substring(7, 1))); ;

            return null;
        }
        protected void grdAvaliacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAvaliacao);
        }

        protected void grdAvaliacao_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdAvaliacao.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "ID_AVALIACAO_CM")
                    e.Editor.Enabled = true;
                if ((e.Column.FieldName) == "ANO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "PERIODO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "SUBPERIODO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "DT_CADASTRO")
                    e.Editor.ReadOnly = true;

            }
            else if (grdAvaliacao.IsEditing)
            {
                if ((e.Column.FieldName) == "ID_AVALIACAO_CM")
                    e.Editor.Enabled = false;
                if ((e.Column.FieldName) == "ANO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "PERIODO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "SUBPERIODO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "DT_CADASTRO")
                    e.Editor.ReadOnly = true;

            }

        }

        protected void grdAvaliacao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdAvaliacao.Settings.ShowFilterRow = false;
        }

        protected void grdAvaliacao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdAvaliacao.Settings.ShowFilterRow = false;
        }

        protected void grdAvaliacao_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            lblMensagem.Text = string.Empty;

            if (string.IsNullOrEmpty(Convert.ToString(e.NewValues["ORDEM"])))
            {
                e.RowError = "Favor informar a Ordem.";
            }
            if (string.IsNullOrEmpty(Convert.ToString(e.NewValues["AVALIACAO"])))
            {
                e.RowError = "Favor informar a Avaliação.";
            }

            ASPxGridView grd = (ASPxGridView)sender;
            var TACM = RN.AvaliacaoCurriculoMinimo.Carregar(int.Parse(e.Keys[0].ToString()));

            if (grd != null && grd.IsNewRowEditing == true)
            {
                if (!RN.AvaliacaoCurriculoMinimo.Validar(TACM).Valido)
                {
                    e.RowError = "Questão já existente.";
                }

            }
        }

        public void Delete(object ID_AVALIACAO_CM)
        {
        }

        public void Update(object ANO, object  SUBPERIODO, object  ORDEM, object  AVALIACAO, object  HABILITADO, object  ID_AVALIACAO_CM)
        {
        }

        protected void odsAvaliacao_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var validacao = new ValidacaoDados();
            string id = e.InputParameters["ID_AVALIACAO_CM"].ToString();

            var TACM = RN.AvaliacaoCurriculoMinimo.Carregar(int.Parse(id));

            validacao = RN.AvaliacaoCurriculoMinimo.ValidarExclusao(TACM);

            if (!validacao.Valido)
            {
                throw new Exception(validacao.Mensagem.ToString());
            }
            else
            {
                if (RN.AvaliacaoCurriculoMinimo.Remover(int.Parse(id)) > 0)
                {

                }
            }
        }

        protected void odsAvaliacao_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var validacao = new ValidacaoDados();

            var TACM = new TceAvaliacaoCurriculoMinimo
            {
                IdAvaliacaoCurriculoMinimo = Convert.ToInt32(e.InputParameters["ID_AVALIACAO_CM"]),
                Ano = Convert.ToInt32(e.InputParameters["ANO"]),
                Periodo = Convert.ToInt32(e.InputParameters["PERIODO"]),
                Subperiodo = Convert.ToInt32(e.InputParameters["SUBPERIODO"]),
                Ordem = Convert.ToInt32(e.InputParameters["ORDEM"]),
                Avaliacao = e.InputParameters["AVALIACAO"].ToString(),
                Habilitado = e.InputParameters["HABILITADO"].ToString() == "True" ? true : false, 
                Matricula = User.Identity.Name
               
            };


            validacao = RN.AvaliacaoCurriculoMinimo.Validar(TACM);

            if (!validacao.Valido)
            {
                throw new Exception(validacao.Mensagem.ToString());
            }
            else
            {
                if (RN.AvaliacaoCurriculoMinimo.Alterar(TACM) > 0)
                {

                }
            }
        }

        private void LimparCampos()
        {
            lblMensagem.Text = string.Empty;
            txtQuestao.Text = string.Empty;
            txtOrdem.Text = string.Empty;
            ddlBimestre.ClearSelection();
            
        }

        protected void grdAvaliacao_AutoFilterCellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "HABILITADO")
            {
                DevExpress.Web.ASPxEditors.ASPxComboBox combo = e.Editor as DevExpress.Web.ASPxEditors.ASPxComboBox;
                combo.Items.Clear();
                DevExpress.Web.ASPxGridView.GridViewDataCheckColumn check = e.Column as DevExpress.Web.ASPxGridView.GridViewDataCheckColumn;
                combo.ValueType = check.PropertiesCheckEdit.ValueType;
                combo.Items.Add(string.Empty, null);
                combo.Items.Add("Marcado", check.PropertiesCheckEdit.ValueChecked);
                combo.Items.Add("Desmarcado", check.PropertiesCheckEdit.ValueUnchecked);
            }

        }


    }
}
