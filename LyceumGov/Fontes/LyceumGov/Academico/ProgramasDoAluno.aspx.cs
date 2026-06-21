using System;
using DevExpress.Web.ASPxEditors;
using Techne.Web;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/ProgramasDoAluno.aspx"),
    ControlText("ProgramasDoAluno"),
    Title("Programas Sociais do Aluno"),]
    public partial class ProgramasDoAluno : TPage
    {
        #region Código Padrão Techne
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
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_RenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdProgramasAluno);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdProgramasAluno, "Programas Sociais do Aluno");
        }

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            if (tseAluno.IsValidDBValue && !tseAluno.DBValue.IsNull)
                grdProgramasAluno.Visible = true;
            else
                grdProgramasAluno.Visible = false;
        }

        protected void grdProgramasAluno_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdProgramasAluno);       
        }

        protected void grdProgramasAluno_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["aluno"] = tseAluno.Value;
        }

        protected void grdProgramasAluno_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if ((e.Column.FieldName) == "dt_inicio")
            {
                (e.Editor as ASPxDateEdit).MaxDate = DateTime.Now;
            }
        }

        protected void grdProgramasAluno_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {

            grdProgramasAluno.Settings.ShowFilterRow = false;
        }

        protected void grdProgramasAluno_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {

            grdProgramasAluno.Settings.ShowFilterRow = false;
        }

        protected void grdProgramasAluno_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (e.NewValues["id_unidade_ensino_programas"] != null)
            {
                if (e.NewValues["id_unidade_ensino_programas"].ToString() == "")
                {
                    e.RowError = "Favor informar o programa da unidade de ensino.";
                }
            }
            else
            {
                e.RowError = "Favor informar o programa da unidade de ensino.";
            }

            if (grdProgramasAluno.IsNewRowEditing)
            {
                if (RN.ProgramaAluno.ExisteAlunoPrograma(tseAluno.Value.ToString(), e.NewValues["id_unidade_ensino_programas"]))
                {
					e.RowError = "Já existe um programa social com os dados especificados para este aluno.";
                }
            }
            else if (grdProgramasAluno.IsEditing)
            {
                if (RN.ProgramaAluno.ExisteAlunoPrograma(tseAluno.Value.ToString(), e.NewValues["id_unidade_ensino_programas"], e.Keys["id_aluno_programas"]))
                {
					e.RowError = "Já existe um programa social com os dados especificados para este aluno.";
                }
            }

            DateTime dataInicio = Convert.ToDateTime(e.NewValues["dt_inicio"]);
            DateTime dataFinal = Convert.ToDateTime(e.NewValues["dt_fim"]);

            if (dataFinal < dataInicio)
            {
                e.RowError = "A data final deve ser maior que a data de início.";
            }


        }

        protected void grdProgramasAluno_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditFormEventArgs e)
        {
            TSearchBox tseProgramaUnidade = (TSearchBox)grdProgramasAluno.FindEditFormTemplateControl("tseProgramaUnidade");
            if (tseProgramaUnidade != null)
                tseProgramaUnidade.SqlWhere = "a.aluno = '" + Convert.ToString(tseAluno.Value) + "' and ano_validade = convert(int,CONVERT(varchar(4),getdate(),112))";
        }
    }
}
