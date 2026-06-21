using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using System.Data;

namespace Techne.Lyceum.Net.Basico
{
    using Techne.Lyceum.RN.Entidades;
    [
        NavUrl("~/Basico/CursosFormacaoPessoal.aspx"),
         ControlText("CursosFormacaoPessoal"),
         Title("Cursos Formação Pessoal"),
        ]

    public partial class CursosFormacaoPessoal : TPage
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
            TituloGrid(grdArea, "Áreas ");
            TituloGrid(grdCurso, "Cursos");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                if (!IsPostBack)
                {
                    ddlArea.Items.Clear();
                    ddlArea.DataSource = RN.AreaFormacaoPessoal.ListarAreas();
                    ddlArea.Items.Insert(0, "Selecione");
                    ddlArea.DataBind();

                    ddlGrauCurso.Items.Clear();
                    ddlGrauCurso.DataSource = RN.Basico.ConsultaItemTabelaValDescr("GrauCursoFormacao");
                    ddlGrauCurso.Items.Insert(0, "Selecione");
                    ddlGrauCurso.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ValidarCampos()
        {
            txtArea.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
            txtArea.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");

            txtCurso.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
            txtCurso.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");
        }
        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdArea);
            ControlaAcesso(grdCurso);
        }

        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        protected void btnSalvarArea_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarArea())
                {

                    TceAreaFormacaoPessoal TAFP = new TceAreaFormacaoPessoal();
                    var validacao = new ValidacaoDados();

                    TAFP.Area = txtArea.Text.Trim();
                    TAFP.Matricula = User.Identity.Name.ToString();


                    validacao = RN.AreaFormacaoPessoal.Validar(TAFP);

                    if (validacao.Valido)
                    {
                        if (RN.AreaFormacaoPessoal.Inserir(TAFP) > 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Área incluída com sucesso.');", true);
                            txtArea.Text = string.Empty;

                            ddlArea.Items.Clear();
                            ddlArea.DataSource = RN.AreaFormacaoPessoal.ListarAreas();
                            ddlArea.Items.Insert(0, "Selecione");
                            ddlArea.DataBind();

                            odsArea.Select();
                            odsArea.DataBind();
                            grdArea.DataBind();

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
                lblMensagem.Text = ex.Message;
            }
        }

        private bool ValidarArea()
        {
            if (string.IsNullOrEmpty(txtArea.Text.Trim()))
            {
                lblMensagem.Text = "Favor digitar a Área.";
                txtArea.Focus();
                return false;
            }
            if (txtArea.Text.Trim().Length > 100)
            {
                lblMensagem.Text = "O campo área deve ter 100 caracteres.";
                txtArea.Focus();
                return false;
            }

            return true;
        }

        protected void grdArea_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdArea);
        }

        protected void grdArea_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdArea.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "ID_AREA_FORMACAO_PESSOAL")
                    e.Editor.Enabled = true;

            }
            else if (grdArea.IsEditing)
            {
                if ((e.Column.FieldName) == "ID_AREA_FORMACAO_PESSOAL")
                    e.Editor.Enabled = false;
            }

        }

        protected void grdArea_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdArea.Settings.ShowFilterRow = false;
        }

        protected void grdArea_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdArea.Settings.ShowFilterRow = false;
        }

        protected void grdArea_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            lblMensagem.Text = string.Empty;
            ASPxGridView grd = (ASPxGridView)sender;
            var TAFP = RN.AreaFormacaoPessoal.Carregar(int.Parse(e.Keys[0].ToString()));

            if (grd != null && grd.IsNewRowEditing == true)
            {
                if (!RN.AreaFormacaoPessoal.Validar(TAFP).Valido)
                {
                    e.RowError = "Área já existente.";
                }

            }
        }

        public object Listar()
        {
            DataTable qt = new DataTable();
            qt = RN.AreaFormacaoPessoal.Listar();

            return qt;
        }

        public void Delete(object ID_AREA_FORMACAO_PESSOAL)
        {
        }

        public void Update(object AREA, object ID_AREA_FORMACAO_PESSOAL)
        {
        }

        protected void odsArea_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var validacao = new ValidacaoDados();
            string id = e.InputParameters["ID_AREA_FORMACAO_PESSOAL"].ToString();

            var TAFP = RN.AreaFormacaoPessoal.Carregar(int.Parse(id));

            validacao = RN.AreaFormacaoPessoal.ValidarExclusao(TAFP);

            if (!validacao.Valido)
            {
                throw new Exception(validacao.Mensagem.ToString());
            }
            else
            {
                if (RN.AreaFormacaoPessoal.Remover(int.Parse(id)) > 0)
                {

                    //throw new Exception("Área excluída com sucesso.");
                }
            }
        }

        protected void odsArea_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var validacao = new ValidacaoDados();

            var TAFP = new TceAreaFormacaoPessoal
            {
                Area = e.InputParameters["AREA"].ToString(),
                IdAreaFormacaoPessoal = int.Parse(e.InputParameters["ID_AREA_FORMACAO_PESSOAL"].ToString()),
                Matricula = User.Identity.Name
            };


            validacao = RN.AreaFormacaoPessoal.Validar(TAFP);

            if (!validacao.Valido)
            {
                throw new Exception(validacao.Mensagem.ToString());
            }
            else
            {
                if (RN.AreaFormacaoPessoal.Alterar(TAFP) > 0)
                {

                    //throw new Exception("Área excluída com sucesso.");
                }
            }
        }


        protected void ods_Update(object sender, ObjectDataSourceStatusEventArgs e)
        {
            ddlArea.Items.Clear();
            ddlArea.DataSource = RN.AreaFormacaoPessoal.ListarAreas();
            ddlArea.Items.Insert(0, "Selecione");
            ddlArea.DataBind();
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlArea.SelectedValue != "Selecione")
                {
                    PreencheGrid(grdCurso, RN.CursoFormacaoPessoal.Listar(int.Parse(ddlArea.SelectedValue)), lblMensagem);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        public void PreencheGrid(ASPxGridView gv, DataTable dt, Label lblErro)
        {
            try
            {
                gv.DataSource = dt;
                gv.DataBind();

            }
            catch (Exception ex)
            {
                lblErro.Text = "ERRO:" + ex.Message;
            }
        }

        protected void btnSalvarCurso_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCurso())
                {

                    TceCursoFormacaoPessoal TCFP = new TceCursoFormacaoPessoal();
                    var validacao = new ValidacaoDados();

                    TCFP.IdAreaFormacaoPessoal = int.Parse(ddlArea.SelectedValue);
                    TCFP.Curso = txtCurso.Text.Trim();
                    TCFP.Grau = ddlGrauCurso.SelectedValue;
                    TCFP.Matricula = User.Identity.Name.ToString();


                    validacao = RN.CursoFormacaoPessoal.Validar(TCFP);

                    if (validacao.Valido)
                    {
                        if (RN.CursoFormacaoPessoal.Inserir(TCFP) > 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Curso incluído com sucesso.');", true);
                            txtCurso.Text = string.Empty;
                            ddlGrauCurso.ClearSelection();

                            odsCursoArea.Select();
                            odsCursoArea.DataBind();
                            grdCurso.DataBind();

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
                lblMensagem.Text = ex.Message;
            }
        }

        private bool ValidarCurso()
        {
            if (ddlArea.SelectedValue == "Selecione")
            {
                lblMensagem.Text = "Favor selecionar a Área do Curso.";
                ddlArea.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtCurso.Text.Trim()))
            {
                lblMensagem.Text = "Favor digitar o Nome do Curso.";
                txtCurso.Focus();
                return false;
            }
            if (ddlGrauCurso.SelectedValue == "Selecione")
            {
                lblMensagem.Text = "Favor selecionar o Grau.";
                ddlGrauCurso.Focus();
                return false;
            }
            if (txtCurso.Text.Trim().Length > 100)
            {
                lblMensagem.Text = "O campo Curso deve ter 100 caracteres.";
                txtCurso.Focus();
                return false;
            }
            return true;
        }


        protected void grdCurso_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdCurso);
        }

        protected void grdCurso_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdCurso.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "ID_CURSO_FORMACAO_PESSOAL")
                    e.Editor.Enabled = true;
                if ((e.Column.FieldName) == "ID_AREA_FORMACAO_PESSOAL")
                    e.Editor.Enabled = false;
                if ((e.Column.FieldName) == "AREA")
                    e.Editor.Enabled = false;


            }
            else if (grdCurso.IsEditing)
            {
                if ((e.Column.FieldName) == "ID_CURSO_FORMACAO_PESSOAL")
                    e.Editor.Enabled = false;
                if ((e.Column.FieldName) == "ID_AREA_FORMACAO_PESSOAL")
                    e.Editor.Enabled = false;
                if ((e.Column.FieldName) == "AREA")
                    e.Editor.Enabled = false;

            }

        }

        protected void grdCurso_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdCurso.Settings.ShowFilterRow = false;
        }

        protected void grdCurso_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdCurso.Settings.ShowFilterRow = false;
        }

        protected void grdCurso_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            ASPxGridView grd = (ASPxGridView)sender;
            var TCFP = RN.CursoFormacaoPessoal.Carregar(int.Parse(e.Keys[0].ToString()));

            if (grd != null && grd.IsNewRowEditing == true)
            {
                if (!RN.CursoFormacaoPessoal.Validar(TCFP).Valido)
                {
                    e.RowError = "Curso já existente.";
                }

            }
        }

        public object ListarCursoArea(object ID_AREA_FORMACAO_PESSOAL)
        {
            if (ID_AREA_FORMACAO_PESSOAL.ToString() != "Selecione")
                return RN.CursoFormacaoPessoal.Listar(int.Parse(ID_AREA_FORMACAO_PESSOAL.ToString()));


            return null;
        }

        public void DeleteCursoArea(object ID_CURSO_FORMACAO_PESSOAL)
        {
        }

        public void UpdateCursoArea(object CURSO, object GRAU, object ID_CURSO_FORMACAO_PESSOAL, object AREA)
        {
        }
        public void UpdateCursoArea(object CURSO, object GRAU, object ID_CURSO_FORMACAO_PESSOAL)
        {
        }

        protected void odsCursoArea_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var validacao = new ValidacaoDados();

            var TCFP = new TceCursoFormacaoPessoal
            {
                IdCursoFormacaoPessoal = int.Parse(e.InputParameters["ID_CURSO_FORMACAO_PESSOAL"].ToString()),
                Curso = e.InputParameters["CURSO"].ToString(),
                Grau = e.InputParameters["GRAU"].ToString(),
                Matricula = User.Identity.Name
            };

            validacao = RN.CursoFormacaoPessoal.Validar(TCFP);
            if (validacao.Valido)
            {

                if (RN.CursoFormacaoPessoal.Alterar(TCFP) > 0)
                {
                    //lblMensagem.Text = "Área incluída com sucesso.";
                }
            }
            else
            {
                throw new Exception(validacao.Mensagem.ToString());
            }
        }

        protected void odsCursoArea_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (RN.CursoFormacaoPessoal.Remover(int.Parse(e.InputParameters["ID_CURSO_FORMACAO_PESSOAL"].ToString())) > 0)
            {
                //lblMensagem.Text = "Área incluída com sucesso.";

            }
        }

        protected void ddlArea_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (ddlArea.SelectedValue != "Selecione")
            {
                txtCurso.Text = string.Empty;
                ddlGrauCurso.ClearSelection();
            }
        }

        protected void pcAreaCurso_TabClick(object source, DevExpress.Web.ASPxTabControl.TabControlCancelEventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                ddlGrauCurso.ClearSelection();

                ddlArea.Items.Clear();
                ddlArea.DataSource = RN.AreaFormacaoPessoal.ListarAreas();
                ddlArea.Items.Insert(0, "Selecione");
                ddlArea.DataBind();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

    }
}
