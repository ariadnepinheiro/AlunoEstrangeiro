using System;
using System.Web.UI;
using DevExpress.Web.ASPxGridView;
using Techne.Web;
using Techne.Lyceum.RN;
using Techne.Data;

namespace Techne.Lyceum.Net.Academico
{
    [
        NavUrl("~/Academico/MatriculasDuplicadas.aspx"),
        ControlText("MatriculasDuplicadas"),
        Title("Matrículas Duplicadas"),
    ]
    public partial class MatriculasDuplicadas : TPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdAluno, "Alunos");
            TituloGrid(grdTurmas, "Turmas");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ASPxGridView.RegisterBaseScript(Page);
            string aluno = ObtemAluno();
            if (!IsPostBack && !IsCallback)
            {
                if (Session["unidadeDuplicados"] != null)
                {
                    txtUnidade.Text = Session["unidadeDuplicados"].ToString();
                }

                grdAluno.FocusedRowIndex = -1;
            }
            else
            {
                grdAluno.FocusedRowIndex = grdAluno.FocusedRowIndex;

                odsTurmas.Select();
                grdTurmas.DataBind();
            }
        }

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

        #region Seleção da grid
        protected void grdTabela_FocusedRowChanged(object sender, EventArgs e)
        {
            string aluno = ObtemAluno();

            if (!IsPostBack && !IsCallback)
            {
                grdAluno.FocusedRowIndex = -1;
                odsTurmas.Select();
                odsTurmas.DataBind();
                grdAluno.DataBind();
                grdTurmas.DataBind();

            }
            else
            {
                grdAluno.FocusedRowIndex = ((DevExpress.Web.ASPxGridView.ASPxGridView)sender).FocusedRowIndex;

                odsTurmas.Select();
                odsTurmas.DataBind();
                grdTurmas.DataBind();
                grdTurmas.ExpandAll();
            }
            lblTabela.Text = "Aluno selecionado: " + aluno;
            txtAluno.Text = aluno;
        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdAluno.PageIndex * grdAluno.SettingsPager.PageSize;
            for (int i = 0; i < grdAluno.VisibleRowCount; i++)
            {
                if (grdAluno.FocusedRowIndex == startIndexOnPage + i)
                    return startIndexOnPage + i;
            }
            return -1;
        }

        private string ObtemAluno()
        {
            //obtém o indice atual da seleção
            int curPageSelection = GetSelectedRowOnTheCurrentPage();
            string aluno;

            aluno = (string)grdAluno.GetRowValues(grdAluno.FocusedRowIndex, "aluno");

            return aluno;
        }
        #endregion

        public void Deletar(string aluno, string turma, decimal ano, decimal semestre) 
        {
            TConnectionWritable conn = Config.CreateWritableConnection();
            try
            {
                conn.Open(true);
                TCommand.ExecuteNonQuery(conn, "DELETE Ly_nota WHERE Aluno = ? AND Turma = ? AND Ano = ? AND Semestre = ?", aluno, turma, ano, semestre);
                TCommand.ExecuteNonQuery(conn, "DELETE ly_falta WHERE Aluno = ? AND Turma = ? AND Ano = ? AND periodo = ?", aluno, turma, ano, semestre);
                TCommand.ExecuteNonQuery(conn, "DELETE ly_matricula WHERE Aluno = ? AND Turma = ? AND Ano = ? AND Semestre = ?", aluno, turma, ano, semestre);
                TCommand.ExecuteNonQuery(conn, @"delete ly_matgrade where ALUNO = ?
                and grade_id in 
                (select distinct GRADE_ID from LY_GRADE_TURMA 
                where TURMA = ? and ANO = ? and SEMESTRE = ?)
                and SIT_MATGRADE = 'Matriculado'", aluno, turma, ano, semestre);
                TCommand.ExecuteNonQuery(conn, @"insert into ly_matricula_duplicada values (?,?,?,?)", aluno, turma, ano, semestre);

                //verificar e retornar erros

            }
            catch (Exception)
            {
                conn.Rollback();
            }
            finally
            {
                conn.Close();
            }
        }

        public static QueryTable ConsultarAlunos(string unidade)
        {
            if (string.IsNullOrEmpty(unidade))
                return null;

            Techne.Data.TConnection connection = Config.CreateConnection();
            connection.Open();
            QueryTable qt = null;
            string sql = @"select distinct m.aluno,  PE.NOME_COMPL as nome from ly_matricula m inner join LY_ALUNO a
                        on a.ALUNO = m.ALUNO INNER JOIN LY_PESSOA PE ON PE.PESSOA = A.PESSOA
                        where m.SIT_MATRICULA = 'Matriculado' and a.UNIDADE_ENSINO = ?
                        group by m.aluno, m.DISCIPLINA, m.ANO, m.SEMESTRE, PE.NOME_COMPL
                        having COUNT(1) > 1";
            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, unidade);
            }
            finally
            {
                connection.Close();
            }
            return qt;
        }

        public static QueryTable ConsultarTurmas(string aluno)
        {
            if (string.IsNullOrEmpty(aluno))
                return null;
            Techne.Data.TConnection connection = Config.CreateConnection();
            connection.Open();
            QueryTable qt = null;
            string sql = @"select distinct aluno, turma, ano, semestre from ly_matricula where sit_matricula = 'Matriculado' and aluno = ?";
            try
            {
                qt = new QueryTable(sql);
                qt.Query(connection, aluno);
            }
            finally
            {
                connection.Close();
            }
            return qt;
        }


        protected void odsTurmas_Deleted(object sender, System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs e)
        {
            grdAluno.FocusedRowIndex = -1;

            lblTabela.Text = string.Empty;
            txtAluno.Text = string.Empty;

            odsAlunos.Select();
            odsTurmas.Select();

            grdAluno.DataBind();
            grdTurmas.DataBind();

        }



    }
}


