using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Data;
using Techne.Web;
using Techne.Lyceum.CR;
using DevExpress.Web.ASPxGridView;
using System.Collections.ObjectModel;
using DevExpress.Web.ASPxEditors;
using System.Drawing;
using DevExpress.Data;
using System.Data;
using Techne.Lyceum.RN;
using System.Globalization;
using System.Threading;
using System.IO;
using Techne.Lyceum.Net.Modulos;

namespace Techne.Lyceum.Net.Academico
{
    [
       NavUrl("~/Academico/PrototipoNotasTurma.aspx"),
       ControlText("PrototipoNotasTurma"),
       Title("Protótipo de Notas por Turma"),
    ]

    public partial class PrototipoNotasTurma : TPage
    {
        private static CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
        private static TextInfo TextInfo = cultureInfo.TextInfo;

        private const String SIT_MATRICULA_MATRICULADO = "Matriculado";
        private Boolean CRIPTOGRAFIA_QUERY_STRING = true;

        private Boolean PERMISSION { get { return Permission.AllowDelete && Permission.AllowInsert && Permission.AllowUpdate; } }

        protected void Page_Init(object sender, EventArgs e)
        {
            LyceumMaster mp = (LyceumMaster)Master;
            mp.habilitaLoading = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                decimal? grade_id = 30986;
                    //ObtemGradeID();
                if (grade_id.HasValue)
                    CarregarDadosIniciais(grade_id.Value);
                else
                {
                    pnlDadosTurma.Visible = false;
                    pnlDisciplina.Visible = false;
                    pnlGridMatriculas.Visible = false;
                    Response.Redirect("~/Academico/ListarNotasTurma.aspx");
                }
            }
            else
            {
                CarregarDadosGrid();
                btnSalvar.Visible = false;
                btnFecharNotas.Visible = false;
                grdMatriculas.Visible = false;
            }
            pnlGridMatriculas.Visible = tseDisciplina.IsValidDBValue && !tseDisciplina.DBValue.IsNull;
            //        btnSalvar.Visible = tseDisciplina.IsValidDBValue && !tseDisciplina.DBValue.IsNull;
            lblMensagem.Visible = false;

            //Boolean padacessReadOnly = !PERMISSION;
            //if (padacessReadOnly)
            //{
            //    btnSalvar.Visible = false;
            //    btnSalvar.Enabled = false;
            //}
        }

        #region Carregamento de dados na tela (recebido o valor de GRADE_ID pela QueryString)

        private decimal? ObtemGradeID()
        {
            try
            {
                if (!CRIPTOGRAFIA_QUERY_STRING)
                    return Convert.ToDecimal(Request.QueryString["Chave"]);
            }
            catch { }

            try
            {
                String gradeID_encrypted = Request.QueryString["Chave"];
                byte[] bytes = Convert.FromBase64String(gradeID_encrypted);
                String gradeID_str = System.Text.Encoding.UTF8.GetString(bytes);

                decimal grade_id;
                if (decimal.TryParse(gradeID_str.Replace("grade_id=", ""), out grade_id))
                    return grade_id;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        private void CarregarDadosIniciais(decimal grade_id)
        {
            RN.Curso rnCurso = new Curso();
            Ly_grade_serie.Row rowGrade = RN.GradeSerie.Consultar(grade_id);
            if (rowGrade == null)
                CarregarDadosIniciais(null, null, null);
            else
            {

                Ly_unidade_ensino.Row rowUnidade = RN.UnidadeEnsino.Consultar(rowGrade.Unidade_responsavel); 
                string tituloCurso = rnCurso.ObtemTituloPor(rowGrade.Curso);
                CarregarDadosIniciais(rowGrade, rowUnidade, tituloCurso);
            }
        }

        private void CarregarDadosIniciais(Ly_grade_serie.Row rowGradeSerie, Ly_unidade_ensino.Row rowUnidade, string tituloCurso)
        {
            hdnGradeID.Value = rowGradeSerie.Grade_id.ToString();

            //tbUnidadeEnsino.Text = "C.E. IGNACIO AZEVEDO DO AMARAL";
            //tbTurma.Text = "CN-1001-180193";
            //tbAno.Text = "2009";
            //tbPeriodo.Text = "1";
            //tbEscolaridade.Text = "CURSO NORMAL";
            //tbMatrizCurricular.Text = "0003.31-2009";
            //tbAnoEscolar.Text = "1";

            tbUnidadeEnsino.Text = rowUnidade != null ? rowUnidade.Nome_comp : "";
            tbTurma.Text = rowGradeSerie != null ? rowGradeSerie.Grade : "";
            tbAno.Text = rowGradeSerie != null ? rowGradeSerie.Ano.Value.ToString() : "";
            tbPeriodo.Text = rowGradeSerie != null ? rowGradeSerie.Semestre.Value.ToString() : "";
            tbEscolaridade.Text = tituloCurso != null ? tituloCurso : "";
            tbMatrizCurricular.Text = rowGradeSerie != null ? rowGradeSerie.Curriculo : "";
            tbAnoEscolar.Text = rowGradeSerie != null ? rowGradeSerie.Serie.Value.ToString() : "";

            Ly_turno.Row turnoRow = RN.Turno.Consultar(rowGradeSerie.Turno);
            tbTurno.Text = turnoRow != null ? TextInfo.ToTitleCase(turnoRow.Descricao) : rowGradeSerie.Turno;

            tseDisciplina.SqlWhere = "grade_id = " + hdnGradeID.Value + " and turma = '" + RN.RNBase.MudarAspas(tbTurma.Text) + "'";

            ddlPeriodoEscolar.DataSource = RN.SubperiodoLetivo.ConsultarSubPeriodosLetivosTodos(rowGradeSerie.Ano, rowGradeSerie.Semestre);
            ddlPeriodoEscolar.DataBind();
        }

        #endregion


        protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
        {
            CarregarDadosGrid();
        }

        private Ly_prova.Row[] ConsultarProvas()
        {
            String disciplina = tseDisciplina.DBValue.ToString(); ;
            String turma = tbTurma.Text;
            decimal ano = Convert.ToDecimal(tbAno.Text);
            decimal periodo = Convert.ToDecimal(tbPeriodo.Text);
            decimal subperiodo;
            Boolean subperiodoOK = Decimal.TryParse(ddlPeriodoEscolar.SelectedValue, out subperiodo);
            return RN.ProvaTurma.ConsultarProvas(disciplina, turma, ano, periodo, subperiodoOK ? (decimal?)subperiodo : null);
        }

        private void CarregarDadosGrid()
        {
            if (!tseDisciplina.IsValidDBValue)
                return;

            //Recupera dados da tela para montagem da grid
            String disciplina = tseDisciplina.DBValue.ToString(); ;
            String turma = tbTurma.Text;
            decimal ano = Convert.ToDecimal(tbAno.Text);
            decimal periodo = Convert.ToDecimal(tbPeriodo.Text);
            decimal subperiodo;
            Boolean subperiodoOK = Decimal.TryParse(ddlPeriodoEscolar.SelectedValue, out subperiodo);

            //Recupera dados da disciplina e carrega hidden fields (valores acessados via javascript para validação numérica)
            RN.Disciplina.NotasDisciplina dadosNotas = RN.Disciplina.ConsultarDisciplinaConceitos(tseDisciplina.DBValue.ToString());
            hdnGrupoNota.Value = dadosNotas.GrupoNota;
            hdnNCasasDec.Value = dadosNotas.CasasDecimais.ToString();

            //Limpa grid
            TituloGrid(grdMatriculas, "Notas da Turma");
            grdMatriculas.Columns.Clear();
            grdMatriculas.DataSource = null;

            //Adiciona colunas Default
            GridViewCommandColumn commandColumn = new GridViewCommandColumn { Caption = "Foto", Visible = true };
            commandColumn.SelectButton.Visible = true;
            commandColumn.ButtonType = ButtonType.Image;
            commandColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            commandColumn.SelectButton.Image.Url = "~/Images/bt_foto.png";

            //            grdMatriculas.Columns.Add(new GridViewDataTextColumn { Caption = "Matrícula", FieldName = "aluno", Name = "aluno", UnboundType = UnboundColumnType.String });
            grdMatriculas.Columns.Add(new GridViewDataTextColumn { Caption = "Nº", FieldName = "num_chamada", Name = "num_chamada", UnboundType = UnboundColumnType.Decimal });
            grdMatriculas.Columns.Add(new GridViewDataTextColumn { Caption = "Nome", FieldName = "nome_compl", Name = "nome_compl", UnboundType = UnboundColumnType.String });
            grdMatriculas.Columns.Add(commandColumn);
            grdMatriculas.Columns.Add(new GridViewDataTextColumn { Caption = "Sit. Matrícula", FieldName = "sit_matricula", Name = "sit_matricula", UnboundType = UnboundColumnType.String });

            foreach (GridViewColumn c in grdMatriculas.Columns)
                c.FixedStyle = GridViewColumnFixedStyle.Left;

            grdMatriculas.Columns["num_chamada"].CellStyle.HorizontalAlign = HorizontalAlign.Center;
            grdMatriculas.Columns["num_chamada"].FixedStyle = GridViewColumnFixedStyle.Left;
            grdMatriculas.Columns["num_chamada"].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            grdMatriculas.Columns["sit_matricula"].CellStyle.HorizontalAlign = HorizontalAlign.Center;
            grdMatriculas.Columns["sit_matricula"].FixedStyle = GridViewColumnFixedStyle.Left;
            grdMatriculas.Columns["sit_matricula"].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;

            //Adiciona colunas em branco e desabilitadas
            for (int i = 0; i < 6; i++)
            {
                GridViewDataColumn c = new GridViewDataColumn();
                c.Caption = "Avaliação";
                c.FieldName = "prova";
                c.Name = "prova";
                c.ToolTip = "Escolha um instrumento de avaliação.";
                c.UnboundType = UnboundColumnType.String;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;

                Boolean contemFormula = true;
                    //!String.IsNullOrEmpty(prova.Formula);
                Boolean naoPossuiGrupoNota = true;
                    //String.IsNullOrEmpty(dadosNotas.GrupoNota);

                //Se possui GrupoNota, exibe DropDownList com conceitos
                //Se não possui GrupoNota, exibe TextBox para edição das notas numéricas
                //Se contém fórmula, exibe TextBox somente leitura
                if (naoPossuiGrupoNota || contemFormula)
                    c.DataItemTemplate = new TextBoxNotaTemplate();
                else
                    c.DataItemTemplate = new DropDownListConceitoTemplate();
                grdMatriculas.Columns.Add(c);
            }


            //Adiciona colunas de acordo com as provas cadastradas para a disciplina
            Ly_prova.Row[] provas = ConsultarProvas();

            foreach (Ly_prova.Row prova in provas)
            {
                GridViewDataColumn c = new GridViewDataColumn();
                c.Caption = prova.Prova;
                c.FieldName = prova.Prova;
                c.Name = prova.Prova;
                c.ToolTip = prova.Nome + (String.IsNullOrEmpty(prova.Formula) ? "\nNota máxima: " + prova.Nota_max : "\nFórmula: " + prova.Formula);
                c.UnboundType = UnboundColumnType.String;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;

                Boolean contemFormula = !String.IsNullOrEmpty(prova.Formula);
                Boolean naoPossuiGrupoNota = String.IsNullOrEmpty(dadosNotas.GrupoNota);

                //Se possui GrupoNota, exibe DropDownList com conceitos
                //Se não possui GrupoNota, exibe TextBox para edição das notas numéricas
                //Se contém fórmula, exibe TextBox somente leitura
                if (naoPossuiGrupoNota || contemFormula)
                    c.DataItemTemplate = new TextBoxNotaTemplate();
                else
                    c.DataItemTemplate = new DropDownListConceitoTemplate();
                grdMatriculas.Columns.Add(c);
            }

            //DataBind dos dados   
            QueryTable qtMatriculas = RN.Matricula.ConsultarMatriculas(disciplina, turma, ano, periodo, subperiodoOK ? (decimal?)subperiodo : null);
            if (qtMatriculas.Rows.Count > 0)
            {
                grdMatriculas.Visible = true;
                grdMatriculas.DataSource = qtMatriculas;
                grdMatriculas.DataBind();
                btnSalvar.Visible = PERMISSION && provas.Count() > 0;

                int cont_provas = RN.ProvaTurma.ContaProvas(turma, disciplina, Convert.ToInt16(ano), Convert.ToInt16(periodo), subperiodo);
                decimal cont_notas = RN.ProvaTurma.SomaNotaMax(turma, disciplina, Convert.ToInt16(ano), Convert.ToInt16(periodo), subperiodo);

                btnFecharNotas.Visible = PERMISSION && cont_provas > 2 && cont_notas == 10;
            }
            else
            {
                ExibirMensagem("Não há alunos matriculados nesta turma.");
                grdMatriculas.Visible = false;
                btnSalvar.Visible = false;
                btnFecharNotas.Visible = false;
            }
        }

        protected void grdMatriculas_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (!grdMatriculas.Visible) return;

            //Verifica a situação da matrícula do aluno (notas editáveis apenas se sit_matricula = "Matriculado"
            String sit_matricula = (String)grdMatriculas.GetRowValues(e.VisibleIndex, "sit_matricula");
            String disciplina = tseDisciplina.DBValue.ToString(); ;
            String turma = tbTurma.Text;
            decimal ano = String.IsNullOrEmpty(tbAno.Text) ? 0 : Convert.ToDecimal(tbAno.Text);
            decimal periodo = String.IsNullOrEmpty(tbPeriodo.Text) ? 0 : Convert.ToDecimal(tbPeriodo.Text);
            decimal subperiodo = String.IsNullOrEmpty(ddlPeriodoEscolar.SelectedValue) ? -1 : Decimal.Parse(ddlPeriodoEscolar.SelectedValue);

            foreach (GridViewColumn column_tmp in grdMatriculas.Columns)
            {
                GridViewDataColumn column = null;
                if (column_tmp is GridViewDataColumn)
                    column = (GridViewDataColumn)column_tmp;
                else
                    continue;

                GridViewDataItemTemplateContainer container = null;
                foreach (Control o in e.Row.Cells[column.Index].Controls)
                    if (o is GridViewDataItemTemplateContainer)
                        container = (GridViewDataItemTemplateContainer)o;

                if (container == null) continue;

                String prova = column.FieldName;
                String controlID = "gridControl_" + column.FieldName;
                int rowIndex = e.VisibleIndex;
                int columnIndex = column.Index;

                Ly_prova.Row provaRow = RN.ProvaTurma.ConsultarProva(prova, disciplina, turma, ano, periodo);
                Boolean contemFormula = true;
                Boolean somenteLeitura = !sit_matricula.Equals(SIT_MATRICULA_MATRICULADO) || contemFormula || !PERMISSION;

                //Verifica o tipo do controle no Template da célula
                if (column.DataItemTemplate is TextBoxNotaTemplate)
                {
                    TextBoxNotaTemplate template = (TextBoxNotaTemplate)column.DataItemTemplate;
                    if (container.Controls[0] is TextBox)
                    {
                        TextBox tb = (TextBox)container.Controls[0];
                        tb.ID = controlID;
                        tb.ReadOnly = somenteLeitura;
                        if (somenteLeitura) tb.BackColor = Color.Gainsboro;
                        tb.Text = HttpUtility.HtmlDecode(container.Text).Trim();
                        tb.Width = Unit.Pixel(35);
                        tb.Attributes.Add("style", "text-align:center");
                        tb.Attributes.Add("rowIndex", rowIndex.ToString());
                        tb.Attributes.Add("columnIndex", columnIndex.ToString());
                        tb.Attributes.Add("navegar", somenteLeitura ? "false" : "true");
                        tb.Attributes.Add("validar", somenteLeitura ? "false" : "true");
                        //tb.Attributes.Add("notaMax", provaRow.Nota_max);
                        if (somenteLeitura)
                            tb.TabIndex = -1;
                    }
                }
                else if (column.DataItemTemplate is DropDownListConceitoTemplate)
                {
                    DropDownListConceitoTemplate template = (DropDownListConceitoTemplate)column.DataItemTemplate;
                    if (container.Controls[0] is DropDownList)
                    {
                        DropDownList ddl = (DropDownList)container.Controls[0];
                        ddl.ID = controlID;
                        ddl.Enabled = !somenteLeitura;
                        ddl.Text = HttpUtility.HtmlDecode(container.Text).Trim();

                        ddl.Items.Add("");
                        QueryTable conceitos = RN.Conceito.ConsultarPorGrupo(hdnGrupoNota.Value);
                        foreach (SimpleRow row in conceitos.Rows)
                            ddl.Items.Add(row["conceito"].ToString());
                    }
                }
            }
        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            Boolean padacessReadOnly = !(Permission.AllowDelete && Permission.AllowInsert && Permission.AllowUpdate);
            if (padacessReadOnly) return;

            var provas = ConsultarProvas().OrderBy(p => p.Ordem);

            String disciplina = tseDisciplina.DBValue.ToString();
            String turma = tbTurma.Text;
            decimal ano = Convert.ToDecimal(tbAno.Text);
            decimal semestre = Convert.ToDecimal(tbPeriodo.Text);

            //Percorre as provas da disciplina, e obtém os dados da coluna da grid referente à prova
            foreach (Ly_prova.Row prova in provas)
            {
                String controlID = "gridControl_" + prova.Prova;

                //Executa caso a Prova não possua fórmula (insere os conceitos digitados na tela)
                if (String.IsNullOrEmpty(prova.Formula))
                {
                    //Montagem de coleção <Aluno,Conceito>
                    Dictionary<String, String> alunosConceitos = new Dictionary<String, String>();
                    for (int rowIndex = 0; rowIndex < grdMatriculas.VisibleRowCount; rowIndex++)
                    {
                        String aluno = grdMatriculas.GetRowValues(rowIndex, "aluno").ToString();
                        String sit_matricula = grdMatriculas.GetRowValues(rowIndex, "sit_matricula").ToString();
                        if (aluno != null && sit_matricula.Equals("Matriculado"))
                        {
                            GridViewDataColumn col = (GridViewDataColumn)grdMatriculas.Columns[prova.Prova];
                            Control c = grdMatriculas.FindRowCellTemplateControl(rowIndex, col, controlID);
                            String conceito = null;
                            if (c is TextBox)
                                conceito = ((TextBox)c).Text;
                            else if (c is DropDownList)
                                conceito = ((DropDownList)c).Text;
                            if (!String.IsNullOrEmpty(conceito))
                                alunosConceitos.Add(aluno, conceito);
                        }
                    }
                    //Insere as notas
                    RetValue ret = RN.Nota.AtualizarNotas(alunosConceitos, disciplina, turma, ano, semestre, prova.Prova);
                    if (ret != null && !ret.Ok)
                        ExibirMensagem(ret.Errors.ToString());
                }
                //Executa caso a Prova possua fórmula (processa as fórmulas e insere)
                //else
                //{
                //    List<String> alunos = new List<String>();
                //    for (int rowIndex = 0; rowIndex < grdMatriculas.VisibleRowCount; rowIndex++)
                //    {
                //        String aluno = grdMatriculas.GetRowValues(rowIndex, "aluno").ToString();
                //        String sit_matricula = grdMatriculas.GetRowValues(rowIndex, "sit_matricula").ToString();
                //        if (sit_matricula.Equals("Matriculado"))
                //            alunos.Add(aluno);
                //    }

                //    //Processa as notas
                //    RetValue ret = RN.Nota.ProcessarNotas(alunos, disciplina, turma, ano, semestre, prova.Prova);
                //    if (ret != null && !ret.Ok)
                //        ExibirMensagem(ret.Errors.ToString());
                //}
            }
            CarregarDadosGrid();
        }

        protected void btnFecharNotas_Click(object sender, ImageClickEventArgs e)
        {
            Boolean padacessReadOnly = !(Permission.AllowDelete && Permission.AllowInsert && Permission.AllowUpdate);
            if (padacessReadOnly) return;

            var provas = ConsultarProvas().OrderBy(p => p.Ordem);

            String disciplina = tseDisciplina.DBValue.ToString();
            String turma = tbTurma.Text;
            decimal ano = Convert.ToDecimal(tbAno.Text);
            decimal semestre = Convert.ToDecimal(tbPeriodo.Text);

            //Percorre as provas da disciplina, e obtém os dados da coluna da grid referente à prova
            foreach (Ly_prova.Row prova in provas)
            {
                String controlID = "gridControl_" + prova.Prova;

                //Executa caso a Prova possua fórmula (processa as fórmulas e insere)
                if (!String.IsNullOrEmpty(prova.Formula))
                {
                    List<String> alunos = new List<String>();
                    for (int rowIndex = 0; rowIndex < grdMatriculas.VisibleRowCount; rowIndex++)
                    {
                        String aluno = grdMatriculas.GetRowValues(rowIndex, "aluno").ToString();
                        String sit_matricula = grdMatriculas.GetRowValues(rowIndex, "sit_matricula").ToString();
                        if (sit_matricula.Equals("Matriculado"))
                            alunos.Add(aluno);
                    }

                    //Processa as notas
                    RetValue ret = RN.Nota.ProcessarNotas(alunos, disciplina, turma, ano, semestre, prova.Prova);
                    if (ret != null && !ret.Ok)
                        ExibirMensagem(ret.Errors.ToString());
                }
            }
            CarregarDadosGrid();
        }

        private void ExibirMensagem(String mensagem)
        {
            lblMensagem.Visible = !String.IsNullOrEmpty(mensagem);
            lblMensagem.Text = mensagem + "<br/>";
        }

        #region PopUp

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdMatriculas.PageIndex * grdMatriculas.SettingsPager.PageSize;
            int selectedRow = -1;
            for (int i = 0; i < grdMatriculas.VisibleRowCount; i++)
            {
                if (grdMatriculas.Selection.IsRowSelected(startIndexOnPage + i))
                {
                    selectedRow = startIndexOnPage + i;
                    break;
                }
            }
            grdMatriculas.Selection.UnselectAll();
            return selectedRow;
        }

        protected void grdMatriculas_SelectionChanged(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                return;

            pucInfoAluno.ShowOnPageLoad = true;
            object matricula = grdMatriculas.GetRowValues(GetSelectedRowOnTheCurrentPage(), "aluno");

            if (matricula == null)
                return;

            Ly_aluno.Row rowAluno = RN.Aluno.ConsultarAluno(matricula.ToString());
            if (rowAluno != null)
            {
                if (rowAluno.Pessoa.HasValue)
                {
                    Ly_foto_pessoa.Row rowFoto = RN.FotoPessoa.Consultar(rowAluno.Pessoa.Value.ToString());
                    if (rowFoto == null)
                    {
                        bimgFotoPessoa.EmptyImage.Url = "~/Images/semfoto.jpg";
                        bimgFotoPessoa.EmptyImage.AlternateText = "sem foto";
                        bimgFotoPessoa.ContentBytes = null;
                    }
                    else
                    {
                        try
                        {
                            //Tenta carregar array de bytes em objeto Image. 
                            //Em caso de exceção, a foto está em formato inválido
                            Bitmap.FromStream(new MemoryStream(rowFoto.Foto));
                            bimgFotoPessoa.ContentBytes = rowFoto.Foto;
                        }
                        catch
                        {
                            bimgFotoPessoa.EmptyImage.Url = "~/Images/fotoinvalida.jpg";
                            bimgFotoPessoa.EmptyImage.AlternateText = "foto inválida";
                            bimgFotoPessoa.ContentBytes = null;
                        }
                    }
                }

                String naoCadastrado = "não cadastrado";

                Ly_pessoa.Row rowPessoa = Pessoa.Consultar(rowAluno.Pessoa.Value.ToString());
                lblNome.Text = String.IsNullOrEmpty(rowPessoa.Nome_compl) ? naoCadastrado : rowPessoa.Nome_compl;
                lblNomePai.Text = String.IsNullOrEmpty(rowPessoa.Nome_pai) ? naoCadastrado : rowPessoa.Nome_pai;
                lblNomeMae.Text = String.IsNullOrEmpty(rowPessoa.Nome_mae) ? naoCadastrado : rowPessoa.Nome_mae;

                String[] emails = new String[] { 
                    rowAluno.E_mail_interno, 
                    rowPessoa.E_mail_interno,
                    rowPessoa.E_mail};

                var emails_notnull = emails.Where(em => !String.IsNullOrEmpty(em));
                if (emails_notnull.Count() > 0)
                {
                    hlEmail.Text = emails_notnull.First();
                    hlEmail.NavigateUrl = "mailto:" + emails_notnull.First();
                    hlEmail.Visible = true;
                    lblEmail.Visible = false;
                }
                else
                {
                    hlEmail.Visible = false;
                    lblEmail.Visible = true;
                }
            }
            else
            {
                bimgFotoPessoa.EmptyImage.Url = "~/Images/semfoto.jpg";
                bimgFotoPessoa.EmptyImage.AlternateText = "sem foto";
                bimgFotoPessoa.ContentBytes = null;
            }
            //grdMatriculas.DataBind();
            grdMatriculas.Visible = true;
        }

        #endregion

        protected void grdMatriculas_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            grdMatriculas.Visible = true;
            btnSalvar.Click += new ImageClickEventHandler(btnSalvar_Click);
            btnFecharNotas.Click += new ImageClickEventHandler(btnFecharNotas_Click);
        }

        public class TextBoxNotaTemplate : ITemplate
        {
            public void InstantiateIn(Control container)
            {
                TextBox tb = new TextBox();
                container.Controls.Add(tb);
            }
        }

        public class DropDownListConceitoTemplate : ITemplate
        {
            public void InstantiateIn(Control container)
            {
                DropDownList cb = new DropDownList();
                container.Controls.Add(cb);
            }
        }

        public class LabelTemplate : ITemplate
        {
            public void InstantiateIn(Control container)
            {
                Label lbl = new Label();
                lbl.Width = Unit.Pixel(35);
                container.Controls.Add(lbl);
            }
        }
    }
}