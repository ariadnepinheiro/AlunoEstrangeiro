using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web.SessionState;
using System.Web.UI.HtmlControls;
using Techne.Controls;
using Techne.Web;
using Techne.Data;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxClasses;
using Techne.Lyceum.RN.Util;
using System.Globalization;



namespace Techne.Lyceum.Net.Academico
{
    [
NavUrl("~/Academico/LicencaAluno.aspx"),
ControlText("LicencaAluno"),
Title("Controle de Faltas Justificadas"),
]

    public partial class LicencaAluno : TPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdLicenca, "Faltas Justificadas");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdLicenca);
        }

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

        protected void tseAluno_Changed(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                lblMensagemFixa.Visible = false;
                hdnDataInicio.Value = string.Empty;
                hdnDataFim.Value = string.Empty;

                if (!tseAluno.DBValue.IsNull)
                {
                    if (tseAluno.IsValidDBValue)
                    {
                        grdLicenca.Visible = true;
                        lblMensagemFixa.Visible = true;

                        odsDisciplinas.SelectParameters[0].DefaultValue = tseAluno.DBValue.ToString();
                        odsTurma.SelectParameters[0].DefaultValue = tseAluno.DBValue.ToString();
                        odsAnoSemestre.SelectParameters[0].DefaultValue = tseAluno.DBValue.ToString();

                        RN.PeriodoLetivo rnPeriodoLetivo = new Techne.Lyceum.RN.PeriodoLetivo();

                        string ano = string.Empty;
                        string semestre = string.Empty;


                        DataView view = (DataView)odsAnoSemestre.Select();
                        for (int i = 0; i < view.Count; i++)
                        {
                            ano = view[i]["Ano"].ToString();
                            semestre = view[i]["Semestre"].ToString();
                        }

                        var datas = rnPeriodoLetivo.ObtemDataInicioFimAulaPor(Convert.ToInt32(ano), Convert.ToInt32(semestre));

                        hdnDataInicio.Value = datas[0];
                        hdnDataFim.Value = datas[1];
                    }
                    else
                    {
                        grdLicenca.Visible = false;
                        lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    grdLicenca.Visible = false;
                    lblMensagem.Text = "Favor consultar um aluno.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void FillCombo(DevExpress.Web.ASPxEditors.ASPxComboBox cmbTurma, string disciplina)
        {
            if (string.IsNullOrEmpty(disciplina)) return;

            cmbTurma.Items.Clear();
            if (disciplina == "-1")
            {
                cmbTurma.Items.Add("Todas");

            }
            else
            {
                Lyceum.CR.Ly_matricula dados = ObterDados(disciplina);

                foreach (Lyceum.CR.Ly_matricula.Row linha in dados.Rows)
                {
                    ListEditItem item = new ListEditItem();

                    item.Text = linha.Turma;
                    item.Value = linha.Turma;

                    cmbTurma.Items.Add(item);

                }
            }
        }

        private Lyceum.CR.Ly_matricula ObterDados(string disciplina)
        {

            Lyceum.CR.Ly_matricula dados = new Techne.Lyceum.CR.Ly_matricula();

            odsMatricula.SelectParameters[0].DefaultValue = tseAluno.DBValue.ToString();
            odsMatricula.SelectParameters[1].DefaultValue = disciplina;
            DataView view = (DataView)odsMatricula.Select();
            for (int i = 0; i < view.Count; i++)
            {
                Lyceum.CR.Ly_matricula.Row linha = dados.NewRow();

                linha.Disciplina = disciplina;
                linha.Aluno = tseAluno.DBValue.ToString();
                linha.Turma = Convert.ToString(view[i]["Turma"]);
                linha.Ano = Convert.ToInt32(view[i]["Ano"]);
                linha.Semestre = Convert.ToInt32(view[i]["Semestre"]);

                dados.Rows.Add(linha);
            }
            return dados;
        }

        private void cmbTurma_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillCombo(source as DevExpress.Web.ASPxEditors.ASPxComboBox, e.Parameter);
        }

        public object Lista(DbObject tseAluno)
        {
            QueryTable qt = null;

            if (!tseAluno.IsNull)
            {
                qt = RN.AlunoLicenca.ConsultarLicencas(tseAluno.ToString());
            }
            return qt;
        }
        protected void grdLicenca_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdLicenca.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "id_aluno_licenca" ||
                    (e.Column.FieldName) == "aluno" ||
                    (e.Column.FieldName) == "disciplina" ||
                    (e.Column.FieldName) == "turma" ||
                    (e.Column.FieldName) == "ano" ||
                    (e.Column.FieldName) == "semestre")
                    e.Editor.ClientEnabled = true;
                e.Editor.ReadOnly = false;
            }
            else if (grdLicenca.IsEditing)
            {
                if ((e.Column.FieldName) == "id_aluno_licenca" ||
                    (e.Column.FieldName) == "aluno" ||
                    (e.Column.FieldName) == "disciplina" ||
                    (e.Column.FieldName) == "turma" ||
                    (e.Column.FieldName) == "ano" ||
                    (e.Column.FieldName) == "semestre")
                {
                    e.Editor.ClientEnabled = false;
                    e.Editor.ReadOnly = true;
                }               
            }

            // filtrando uma combo da grid pela outra
            if (grdLicenca.IsNewRowEditing && e.Column.FieldName == "turma")
            {
                DevExpress.Web.ASPxEditors.ASPxComboBox combo = e.Editor as DevExpress.Web.ASPxEditors.ASPxComboBox;
                combo.Callback += new CallbackEventHandlerBase(cmbTurma_OnCallback);
            }
            

            if (e.Column.FieldName == "dt_inicio" || e.Column.FieldName == "dt_fim")
            {
                if (grdLicenca.IsNewRowEditing)
                {                    
                    // filtrando uma combo da grid pela outra
                    if (e.Column.FieldName == "dt_inicio")
                    {
                        (e.Editor as ASPxDateEdit).MinDate = Convert.ToDateTime(hdnDataInicio.Value);
                        (e.Editor as ASPxDateEdit).MaxDate = Convert.ToDateTime(hdnDataFim.Value);
                    }

                    if (e.Column.FieldName == "dt_fim")
                    {
                        (e.Editor as ASPxDateEdit).MinDate = Convert.ToDateTime(hdnDataInicio.Value);
                        (e.Editor as ASPxDateEdit).MaxDate = Convert.ToDateTime(hdnDataFim.Value);
                    }
                }
            }

            
        }

        public void Delete(decimal id_aluno_licenca) { }

        public void Insert(DbObject tseAluno, string disciplina, string turma, decimal ano, decimal semestre, DateTime dt_inicio, DateTime dt_fim, string descricao, Int32 id_aluno_licenca, string aluno, int justificativafaltaid, string observacao, string usuarioId) { }
        public void Insert(object disciplina, object turma, object ano, object semestre, object justificativafaltaid, object observacao, object dt_inicio, object dt_fim) { }
        public void Update(string disciplina, string turma, decimal ano, decimal semestre, DateTime dt_inicio, DateTime dt_fim, string descricao, string aluno, decimal id_aluno_licenca, int justificativafaltaid, string observacao, string usuarioId) { }
        public void Update(object disciplina, object turma, object ano, object semestre, object justificativafaltaid, object observacao, object dt_inicio, object dt_fim, object id_aluno_licenca) { }

        protected void grdLicenca_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdLicenca);
        }

        protected void grdLicenca_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdLicenca.Settings.ShowFilterRow = false;
        }

        protected void grdLicenca_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdLicenca.Settings.ShowFilterRow = false;
        }


        protected void odsLicenca_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Entidades.LyAlunoLicenca dados = new Techne.Lyceum.RN.Entidades.LyAlunoLicenca();
            RN.AlunoLicenca rnAlunoLicenca = new Techne.Lyceum.RN.AlunoLicenca();
            RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();


            dados.Aluno = tseAluno.DBValue.ToString();
            dados.Disciplina = e.InputParameters["disciplina"] != null ? e.InputParameters["disciplina"].ToString() : null;
            dados.Turma = e.InputParameters["turma"] != null ? e.InputParameters["turma"].ToString() : null;
            dados.Ano = e.InputParameters["ano"] != null ? Convert.ToDecimal(e.InputParameters["ano"]) : -1;
            dados.Semestre = e.InputParameters["semestre"] != null ? Convert.ToDecimal(e.InputParameters["semestre"]) : -1;
            dados.Observacao = e.InputParameters["observacao"] != null ? e.InputParameters["observacao"].ToString() : null;
            dados.DtInicio = e.InputParameters["dt_inicio"] != null ? Convert.ToDateTime(e.InputParameters["dt_inicio"]) : DateTime.MinValue;
            dados.DtFim = e.InputParameters["dt_fim"] != null ? Convert.ToDateTime(e.InputParameters["dt_fim"]) : DateTime.MinValue;
            dados.JustificativaFaltaId = e.InputParameters["JustificativaFaltaId"] != null ? Convert.ToInt32(e.InputParameters["JustificativaFaltaId"]) : -1;
            dados.UsuarioId = User.Identity.Name;
            hdnDataMatricula.Value = rnMatricula.ObtemDataMatriculaEnturmacaoPor(Convert.ToInt32(dados.Ano), Convert.ToInt32(dados.Semestre), dados.Turma, tseAluno.DBValue.ToString()).ToShortDateString();


            validacao = rnAlunoLicenca.Valida(dados, dados.Disciplina == "-1" ? true : false, true, Convert.ToDateTime(hdnDataMatricula.Value));

            if (validacao.Valido)
            {
                if (dados.Disciplina == "-1" || dados.Disciplina.IsNullOrEmptyOrWhiteSpace())
                {
                    rnAlunoLicenca.InsereTodos(dados);
                }
                else
                {
                    rnAlunoLicenca.Insere(dados);
                }
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdLicenca.DataBind();
        }

        protected void odsLicenca_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Entidades.LyAlunoLicenca dados = new Techne.Lyceum.RN.Entidades.LyAlunoLicenca();
            RN.AlunoLicenca rnAlunoLicenca = new Techne.Lyceum.RN.AlunoLicenca();
            RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();

            dados.IdAlunoLicenca = e.InputParameters["id_aluno_licenca"] != null ? Convert.ToInt32(e.InputParameters["id_aluno_licenca"]) : -1;
            dados.Aluno = tseAluno.DBValue.ToString();
            dados.Disciplina = e.InputParameters["disciplina"] != null ? e.InputParameters["disciplina"].ToString() : null;
            dados.Turma = e.InputParameters["turma"] != null ? e.InputParameters["turma"].ToString() : null;
            dados.Ano = e.InputParameters["ano"] != null ? Convert.ToDecimal(e.InputParameters["ano"]) : -1;
            dados.Semestre = e.InputParameters["semestre"] != null ? Convert.ToDecimal(e.InputParameters["semestre"]) : -1;
            dados.Observacao = e.InputParameters["observacao"] != null ? e.InputParameters["observacao"].ToString() : null;
            dados.DtInicio = e.InputParameters["dt_inicio"] != null ? Convert.ToDateTime(e.InputParameters["dt_inicio"]) : DateTime.MinValue;
            dados.DtFim = e.InputParameters["dt_fim"] != null ? Convert.ToDateTime(e.InputParameters["dt_fim"]) : DateTime.MinValue;
            dados.JustificativaFaltaId = e.InputParameters["JustificativaFaltaId"] != null ? Convert.ToInt32(e.InputParameters["JustificativaFaltaId"]) : -1;
            dados.UsuarioId = User.Identity.Name;
            hdnDataMatricula.Value = rnMatricula.ObtemDataMatriculaEnturmacaoPor(Convert.ToInt32(dados.Ano), Convert.ToInt32(dados.Semestre), dados.Turma, tseAluno.DBValue.ToString()).ToShortDateString();


            validacao = rnAlunoLicenca.Valida(dados, dados.Disciplina == "-1" ? true : false, false,Convert.ToDateTime(hdnDataMatricula.Value));

            if (validacao.Valido)
            {
                rnAlunoLicenca.Atualiza(dados);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdLicenca.DataBind();
        }

        protected void odsLicenca_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Entidades.LyAlunoLicenca aluno = new Techne.Lyceum.RN.Entidades.LyAlunoLicenca();
            RN.AlunoLicenca rnAlunoLicenca = new Techne.Lyceum.RN.AlunoLicenca();
            int licencaId = 0;
            licencaId = Convert.ToInt32(e.InputParameters["id_aluno_licenca"]);

            validacao = rnAlunoLicenca.ValidaRemocao(licencaId);

            if (validacao.Valido)
            {
                rnAlunoLicenca.Remove(licencaId);
                grdLicenca.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
