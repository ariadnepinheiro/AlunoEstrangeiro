using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Data;
using Techne.Lyceum.CR;
using System.Web.UI.HtmlControls;
using System.Drawing;
namespace Techne.Lyceum.Net.Relatorio
{
    [
    NavUrl("~/Relatorio/RelatorioTurma.aspx"),
    ControlText("RelatorioTurma"),
    Title("Relatório da Turma"),
    ]
    public partial class RelatorioTurma : TPage
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

        #region Propriedade
        private static String COR_GLP = "#C8FFBF";
        private static String COR_GLP_00000000 = "#C8FFBF";
        private static String COR_GLP_99999999 = "MediumSeaGreen";

        private Techne.Lyceum.RN.Turma.DadosTurma ObjetoTurma
        {
            get { return (Techne.Lyceum.RN.Turma.DadosTurma)ViewState["ObjetoTurma"]; }
            set { ViewState["ObjetoTurma"] = value; }
        }
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Keys.Count > 0)
                CarregarDadosTurma();
        }
        #endregion

        #region Metodos
        private void CarregarDadosTurma()
        {
            RN.Turma rnTurma = new Techne.Lyceum.RN.Turma();
            byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
            string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
            ObterDadosQueryString(decodedText);

            if (ObjetoTurma != null)
            {   
                ObjetoTurma.Grade = rnTurma.ObtemTurmaPor(Convert.ToDecimal(ObjetoTurma.Grade_ID));
                ObterCurriculo();

                if (!string.IsNullOrEmpty(ObjetoTurma.Ano))
                    lblValorAno.Text = ObjetoTurma.Ano;
                if (!string.IsNullOrEmpty(ObjetoTurma.Periodo))
                    lblValorPeriodo.Text = ObjetoTurma.Periodo;
                if (!string.IsNullOrEmpty(ObjetoTurma.Nucleo))
                {
                    Ly_nucleo.Row linhaNucleo = RN.Coordenadoria.Consultar(ObjetoTurma.Nucleo);
                    if (linhaNucleo != null && !string.IsNullOrEmpty(linhaNucleo.Descricao))
                        lblValorCoordenadoria.Text = linhaNucleo.Descricao;
                }
                if (!string.IsNullOrEmpty(ObjetoTurma.UnidadeResponsavel))
                    lblValorUnidadeResponsavel.Text = RN.UnidadeEnsino.ConsultarPorUnidade(ObjetoTurma.UnidadeResponsavel);

                if (!string.IsNullOrEmpty(ObjetoTurma.Grade))
                    lblTurmaValor.Text = ObjetoTurma.Grade;

                if (!string.IsNullOrEmpty(ObjetoTurma.Turno))
                    lblTurnoValor.Text = RN.Turno.Consultar(ObjetoTurma.Turno).Descricao;
            }
            PopularTable();
            ControlarCorPostCelula();
        }

        private void ControlarCorPostCelula()
        {
            foreach (TableRow tr in tQuadroHorario.Rows.Cast<TableRow>()
                .Where(tr => tr.TableSection == TableRowSection.TableBody))
            {
                //if (tr.TableSection == TableRowSection.TableBody)
                //{
                foreach (TableCell td in tr.Cells)
                {
                    if (td.Controls.Count > 3)
                    {
                        Label txtDocente = null;
                        Label txtDisciplina = null;

                        //verifica se o controle de indice 0 é um textbox
                        if (td.Controls[0] is Label)
                            //cast do textbox de docente encontrado na célula (representado pelo indice 0)
                            txtDocente = (Label)td.Controls[0];

                        if (td.Controls[2] is Label)
                            txtDisciplina = (Label)td.Controls[2];

                        ControlarCorControleCelula(txtDocente, txtDisciplina);
                    }
                }
                //}
            }
        }

        private void ControlarCorControleCelula(Label txtDocente, Label txtDisciplina)
        {
            if (txtDocente != null && txtDisciplina != null)
            {
                //obtém valor do campo texto caso seja um postback
                string valorDocente = txtDocente.Text;

                if (!string.IsNullOrEmpty(valorDocente))
                {
                    //caso a cor esteja definida como GLP não entrará na condição pois já foi definido este valor quando foi preenchido seu valor
                    if (txtDocente.Style["background-color"] != COR_GLP && txtDocente.Style["background-color"] != COR_GLP_00000000 && txtDocente.Style["background-color"] != COR_GLP_99999999)
                    {
                        txtDocente.Style.Clear();
                        txtDisciplina.Style.Clear();

                        if (valorDocente.Contains("00000000"))
                        {
                            txtDocente.Style.Add("background-color", "#FFFD80");
                            txtDocente.Style.Add("color", "black");

                            txtDisciplina.Style.Add("background-color", "#FFFD80");
                            txtDisciplina.Style.Add("color", "black");

                            (txtDocente.Parent as TableCell).BackColor = Color.FromArgb(255, 255, 204);
                        }
                        else if (valorDocente.Contains("99999999"))
                        {
                            txtDocente.Style.Add("background-color", "#EAEA00");
                            txtDocente.Style.Add("color", "black");

                            txtDisciplina.Style.Add("background-color", "#EAEA00");
                            txtDisciplina.Style.Add("color", "black");

                            (txtDocente.Parent as TableCell).BackColor = Color.FromArgb(234, 234, 0);
                        }
                        else if (valorDocente.Contains("66666666"))
                        {
                            txtDocente.Style.Add("background-color", "#000080");
                            txtDocente.Style.Add("color", "white");

                            txtDisciplina.Style.Add("background-color", "#000080");
                            txtDisciplina.Style.Add("color", "white");

                            (txtDocente.Parent as TableCell).BackColor = Color.FromArgb(0, 0, 128);
                        }
                        else if (valorDocente.Contains("88888888") || valorDocente.Contains("11111111")
                            || valorDocente.Contains("22222222") || valorDocente.Contains("44444444"))
                        {
                            //txtDocente.Style.Add("background-color", "#FF9B54");
                            txtDocente.Style.Add("background-color", "#D7D7D7");
                            txtDocente.Style.Add("color", "black");

                            //txtDisciplina.Style.Add("background-color", "#FF9B54");
                            txtDisciplina.Style.Add("background-color", "#D7D7D7");
                            txtDisciplina.Style.Add("color", "black");

                            (txtDocente.Parent as TableCell).BackColor = Color.FromArgb(215, 215, 215);
                        }
                        else if (valorDocente.Contains("55555555") || valorDocente.Contains("77777777"))
                        {
                            txtDocente.Style.Add("background-color", "LightGray");
                            txtDocente.Style.Add("color", "black");

                            txtDisciplina.Style.Add("background-color", "LightGray");
                            txtDisciplina.Style.Add("color", "black");

                            (txtDocente.Parent as TableCell).BackColor = Color.LightGray;
                        }
                        else if (valorDocente.Substring(0, 2) == "33")
                        {
                            txtDocente.Style.Add("background-color", "#0F6BFF");
                            txtDocente.Style.Add("color", "white");

                            txtDisciplina.Style.Add("background-color", "#0F6BFF");
                            txtDisciplina.Style.Add("color", "white");

                            (txtDocente.Parent as TableCell).BackColor = Color.FromArgb(15, 107, 255);
                        }
                        else if (valorDocente.Substring(0, 2) == "55")
                        {
                            txtDocente.Style.Add("background-color", "#7FC9FF");
                            txtDocente.Style.Add("color", "black");

                            txtDisciplina.Style.Add("background-color", "#7FC9FF");
                            txtDisciplina.Style.Add("color", "black");

                            (txtDocente.Parent as TableCell).BackColor = Color.FromArgb(127, 201, 255);
                        }
                    }
                    // else
                    //     (txtDocente.Parent as TableCell).BackColor = Color.FromArgb(200, 255, 191);
                }
                else // se a célula estiver vazia será definido o estilo padrão para os controles
                {
                    txtDocente.Style.Clear();

                    //txtDocente.Style.Add("background-color", "white");
                    //txtDocente.Style.Add("color", "black");

                    txtDisciplina.Style.Clear();

                    (txtDocente.Parent as TableCell).BackColor = Color.Empty;
                    //txtDisciplina.Style.Add("background-color", "white");
                    //txtDisciplina.Style.Add("color", "black");
                }
            }
        }

        private void ObterCurriculo()
        {
            if (ObjetoTurma == null)
                ObjetoTurma = new Techne.Lyceum.RN.Turma.DadosTurma();

            if (!string.IsNullOrEmpty(ObjetoTurma.Grade) && !string.IsNullOrEmpty(ObjetoTurma.Ano) && !string.IsNullOrEmpty(ObjetoTurma.Periodo))
            {
                Ly_turma dtTurma = RN.Turma.Consultar(ObjetoTurma.Grade, ObjetoTurma.Ano, ObjetoTurma.Periodo);
                if (dtTurma != null && dtTurma.Rows.Count > 0)
                    ObjetoTurma.Curriculo = Convert.ToString(dtTurma.Rows[0]["CURRICULO"]);
            }
        }

        private void ObterDadosQueryString(string queryString)
        {
            ObjetoTurma = new Techne.Lyceum.RN.Turma.DadosTurma();
            string[] listaDados = queryString.Split('&');

            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("ano") >= 0)
                    ObjetoTurma.Ano = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("semestre") >= 0)
                    ObjetoTurma.Periodo = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("turno") >= 0)
                    ObjetoTurma.Turno = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("curso") >= 0)
                    ObjetoTurma.Curso = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("unidadeResponsavel") >= 0)
                    ObjetoTurma.UnidadeResponsavel = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("prefixoUnidadeResponsavel") >= 0)
                    ObjetoTurma.MnemonicoUnidadeResponsavel = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("gradeId") >= 0)
                    ObjetoTurma.Grade_ID = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("serie") >= 0)
                    ObjetoTurma.Serie = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("curriculo") >= 0)
                    ObjetoTurma.Curriculo = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("faculdade") >= 0)
                    ObjetoTurma.Faculdade = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("nucleo") >= 0)
                    ObjetoTurma.Nucleo = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("municipio") >= 0)
                    ObjetoTurma.Municipio = dados.Substring(dados.LastIndexOf('=') + 1);
            }
        }

        private void PopularTable()
        {
            try
            {
                //verifica se o objeto de turma não está nulo
                if (ObjetoTurma != null)
                {
                    //verifica se existe série no objeto de turma
                    if (!string.IsNullOrEmpty(ObjetoTurma.Serie))
                    {
                        decimal serie = 0M;
                        //tenta converter o valor da série no objeto da turma para decimal
                        if (Decimal.TryParse(ObjetoTurma.Serie, out serie))
                        {
                            QueryTable qt = RN.HorarioOperacional.Consultar(ObjetoTurma.Faculdade, ObjetoTurma.Turno, ObjetoTurma.Curso, ObjetoTurma.Curriculo, serie);
                            QueryTable qtAulaDocente = null;

                            qtAulaDocente = RN.Turma.ConsultarAulaDocente(ObjetoTurma.Turno, ObjetoTurma.Faculdade, ObjetoTurma.Grade, Convert.ToDecimal(ObjetoTurma.Ano), Convert.ToDecimal(ObjetoTurma.Periodo));

                            TableRow tr = null;

                            if (qt != null)
                            {
                                for (int i = 0; i < qt.Rows.Count; i++)
                                {
                                    SimpleRow linha = qt.Rows[i];
                                    //tr = new TableRow();
                                    //tr.Height = Unit.Pixel(7);
                                    //TableCell td = new TableCell();
                                    //td.CssClass = "bordaHorario";
                                    //tr.Cells.Add(td);

                                    //td = new TableCell();
                                    //td.ColumnSpan = 7;
                                    //td.CssClass = string.Empty;
                                    //tr.Cells.Add(td);

                                    //tQuadroHorario.Rows.Add(tr);

                                    tr = new TableRow();
                                    tr.BackColor = i % 2 == 0 ? Color.White : Color.FromArgb(245, 245, 245);

                                    string valorIdentificador = Convert.ToString(linha["aula"]) + "_" + String.Format("{0:HH:mm}", Convert.ToDateTime(linha["horaini_aula"])).Replace(":", "_") + "_" + String.Format("{0:HH:mm}", Convert.ToDateTime(linha["horafim_aula"])).Replace(":", "_");

                                    TableCell tdHorario = new TableCell();

                                    tdHorario.Width = Unit.Percentage(10);
                                    tdHorario.CssClass = "bordaHorario";

                                    HtmlInputHidden hAula = new HtmlInputHidden();
                                    hAula.ID = "hAula_" + valorIdentificador;
                                    hAula.Value = Convert.ToString(linha["aula"]);
                                    tdHorario.Controls.Add(hAula);

                                    tdHorario.Controls.Add(new LiteralControl(String.Format("{0:HH:mm}", Convert.ToDateTime(linha["horaini_aula"])) + " / " + String.Format("{0:HH:mm}", Convert.ToDateTime(linha["horafim_aula"]))));

                                    tr.Cells.Add(tdHorario);

                                    string valor = Convert.ToString(linha["aula"]) + "|" + String.Format("{0:HH:mm}", Convert.ToDateTime(linha["horaini_aula"])) + "|" + String.Format("{0:HH:mm}", Convert.ToDateTime(linha["horafim_aula"]));

                                    MontarCelula("txtDocenteSegunda_" + valorIdentificador, "txtDisciplinaSegunda_" + valorIdentificador, true, valor, tr);
                                    MontarCelula("txtDocenteTerca_" + valorIdentificador, "txtDisciplinaTerca_" + valorIdentificador, true, valor, tr);
                                    MontarCelula("txtDocenteQuarta_" + valorIdentificador, "txtDisciplinaQuarta_" + valorIdentificador, true, valor, tr);
                                    MontarCelula("txtDocenteQuinta_" + valorIdentificador, "txtDisciplinaQuinta_" + valorIdentificador, true, valor, tr);
                                    MontarCelula("txtDocenteSexta_" + valorIdentificador, "txtDisciplinaSexta_" + valorIdentificador, true, valor, tr);
                                    MontarCelula("txtDocenteSabado_" + valorIdentificador, "txtDisciplinaSabado_" + valorIdentificador, true, valor, tr);

                                    tQuadroHorario.Rows.Add(tr);

                                    MontarCelulaDados(qtAulaDocente, Convert.ToString(linha["aula"]), valorIdentificador, tr);
                                }

                                tr = new TableRow();
                                for (int i = 0; i < 7; i++)
                                    tr.Cells.Add(new TableCell { CssClass = "bordaFundo1" });
                                tQuadroHorario.Rows.Add(tr);

                                tr = new TableRow();
                                for (int i = 0; i < 7; i++)
                                    tr.Cells.Add(new TableCell { CssClass = "bordaFundo2" });

                                tQuadroHorario.Rows.Add(tr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void MontarCelula(string idTextBoxDocente, string idTextBoxDisciplina, bool eventoBorda, string valor, TableRow tr)
        {
            TableCell td = new TableCell();
            //verifica se adiciona o evendo de click na célula para pintar de vermelho sua borda
            if (eventoBorda)
            {
                //           td.CssClass = "bordaBranca";
                //           td.Attributes.Add("onclick", "HiLite(this);");
                //           td.Attributes.Add("onmouseover", "DadosCelula(this);");
                td.Width = Unit.Percentage(12.5);
                Label txtDocente = new Label();
                txtDocente.Attributes.Add("class", "txtInput");
                txtDocente.ID = idTextBoxDocente;
                txtDocente.Attributes.Add("readonly", "readonly");

                //TXTDOCENTE Control index 0
                td.Controls.Add(txtDocente);

                //LITERALCONTROL (<br>) Control index 1
                td.Controls.Add(new LiteralControl("<br>"));

                Label txtDisciplina = new Label();
                txtDisciplina.Attributes.Add("readonly", "readonly");
                txtDisciplina.Attributes.Add("class", "txtInput");
                txtDisciplina.ID = idTextBoxDisciplina;

                //TXTDISCIPLINA Control index 2
                td.Controls.Add(txtDisciplina);

                System.Web.UI.WebControls.TextBox txtCamposAula = new System.Web.UI.WebControls.TextBox();
                txtCamposAula.ID = "txtCamposAula_" + txtDocente.ID;
                txtCamposAula.Text = valor;
                txtCamposAula.Visible = false;
                //TXTCAMPOSAULA Control index 3
                td.Controls.Add(txtCamposAula);

                HtmlInputHidden hCodigoDocente = new HtmlInputHidden();
                hCodigoDocente.ID = "txtCodigoDocente_" + txtDocente.ID;
                //HTMLINPUTHIDDEN (hCodigoDocente) Control index 4
                td.Controls.Add(hCodigoDocente);

                HtmlInputHidden hCodigoDisciplina = new HtmlInputHidden();
                hCodigoDisciplina.ID = "txtCodigoDisciplina_" + txtDisciplina.ID;
                //HTMLINPUTHIDDEN (hCodigoDisciplina) Control index 5
                td.Controls.Add(hCodigoDisciplina);
                tr.HorizontalAlign = HorizontalAlign.Center;
                //adiciona célula na linha
                tr.Cells.Add(td);

                //Nova célula com o imagem que possibilita copiar/colar/recortar valores
                //TableCell tdImagem = new TableCell();

                ////adiciona o controle somente se o tipo de operação for diferente de consulta e exclusão
                //if (_tipoOperacao != TipoOperacao.ConsultarRetornaDados && _tipoOperacao != TipoOperacao.Excluir)
                //{
                //    HtmlImage img = new HtmlImage();

                //    //                  if (_tipoOperacao != TipoOperacao.Excluir && _tipoOperacao != TipoOperacao.Consultar)
                //    //                      img.Attributes.Add("onclick", "DisplayTip(this, 0, 0, '" + RN.RNBase.MudarAspas(txtDocente.ClientID) + "', '" + RN.RNBase.MudarAspas(txtDisciplina.ClientID) + "')");

                //    HtmlGenericControl div = new HtmlGenericControl("div");
                //    div.Attributes.Add("class", "PopUp");
                //    div.Attributes.Add("style", "display:block;");

                //    tdImagem.Controls.Add(div);

                //    img.Attributes.Add("class", "Imagem");
                //    img.Src = "~/Images/select_list.gif";
                //    tdImagem.VerticalAlign = VerticalAlign.Top;
                //    tdImagem.HorizontalAlign = HorizontalAlign.Left;
                //    tdImagem.Controls.Add(img);
                //}

                //adiciona célula na linha
                //tr.Cells.Add(tdImagem);
            }
            else
            {
                td.Width = Unit.Percentage(10);
                string aula = valor.Split(' ')[0];

                HtmlInputHidden hAula = new HtmlInputHidden();
                hAula.ID = "hAula_" + aula;
                hAula.Value = aula;
                td.Controls.Add(hAula);

                td.Controls.Add(new LiteralControl(valor.Substring(aula.Length + 1)));
                tr.Cells.Add(td);
            }
        }

        private void MontarCelulaDados(QueryTable qtAulaDocente, string aula, string valorIdentificador, TableRow tr)
        {
            string sql = "aula = " + aula;
            SimpleRow[] dadosAula = qtAulaDocente.Select(sql);

            if (dadosAula != null && dadosAula.Length > 0)
            {
                foreach (SimpleRow linhaDadosAula in dadosAula)
                {
                    int diaSemana = Convert.ToInt32(linhaDadosAula["DIA_SEMANA"]);
                    string tipo_aula = string.Empty;

                    if (!linhaDadosAula["tipo_aula"].IsNull)
                        tipo_aula = Convert.ToString(linhaDadosAula["tipo_aula"]);

                    String nomeDocente = Convert.ToString(linhaDadosAula["NOME_DOCENTE"]);
                    String nomeDisciplina = linhaDadosAula["NOME_DISCIPLINA"].ToString();
                    String numFunc = Convert.ToString(linhaDadosAula["NUM_FUNC"]);
                    String disciplina = Convert.ToString(linhaDadosAula["DISCIPLINA"]);

                    String turno = Convert.ToString(linhaDadosAula["turno"]);
                    String faculdade = Convert.ToString(linhaDadosAula["faculdade"]);
                    String turma = Convert.ToString(linhaDadosAula["turma"]);
                    String ano = Convert.ToString(linhaDadosAula["ano"]);
                    String semestre = Convert.ToString(linhaDadosAula["semestre"]);

                    if (tipo_aula == "GLP")
                    {
                        QueryTable qtAnterior = new QueryTable(
                        @"select ad.num_func, d.matricula from ly_aula_docente ad
                        inner join ly_docente d on d.num_func = ad.num_func
                        where 
                        ad.num_func <> ? AND ad.turno = ? AND ad.faculdade = ? AND ad.dia_semana = ? AND ad.aula = ? AND ad.disciplina = ? AND ad.turma = ? AND ad.ano = ? AND ad.semestre = ?                        
                        order by ad.stamp_atualizacao desc");
                        qtAnterior.Query(Config.CreateConnection(), numFunc, turno, faculdade, diaSemana, aula,
                            disciplina, turma, ano, semestre);
                        var anteriores = qtAnterior.Rows.Cast<SimpleRow>();
                        string matriculaAnterior = anteriores.Count() > 0 ? anteriores.First()["matricula"].ToString() : "";
                        if (!String.IsNullOrEmpty(matriculaAnterior))
                            tipo_aula += "_" + matriculaAnterior;
                    }

                    if (diaSemana == 2)
                        PreencherValorCelula(tr, "txtDocenteSegunda_" + valorIdentificador, nomeDocente, "txtDisciplinaSegunda_" + valorIdentificador, nomeDisciplina, numFunc, disciplina, tipo_aula);
                    else if (diaSemana == 3)
                        PreencherValorCelula(tr, "txtDocenteTerca_" + valorIdentificador, nomeDocente, "txtDisciplinaTerca_" + valorIdentificador, nomeDisciplina, numFunc, disciplina, tipo_aula);
                    else if (diaSemana == 4)
                        PreencherValorCelula(tr, "txtDocenteQuarta_" + valorIdentificador, nomeDocente, "txtDisciplinaQuarta_" + valorIdentificador, nomeDisciplina, numFunc, disciplina, tipo_aula);
                    else if (diaSemana == 5)
                        PreencherValorCelula(tr, "txtDocenteQuinta_" + valorIdentificador, nomeDocente, "txtDisciplinaQuinta_" + valorIdentificador, nomeDisciplina, numFunc, disciplina, tipo_aula);
                    else if (diaSemana == 6)
                        PreencherValorCelula(tr, "txtDocenteSexta_" + valorIdentificador, nomeDocente, "txtDisciplinaSexta_" + valorIdentificador, nomeDisciplina, numFunc, disciplina, tipo_aula);
                    else if (diaSemana == 7)
                        PreencherValorCelula(tr, "txtDocenteSabado_" + valorIdentificador, nomeDocente, "txtDisciplinaSabado_" + valorIdentificador, nomeDisciplina, numFunc, disciplina, tipo_aula);
                }
            }
        }

        private void PreencherValorCelula(TableRow tr, string idTextBoxDocente, string valorTextBoxDocente, string idTextBoxDisciplina, string valorTextBoxDisciplina, string num_func, string disciplina, string tipo_aula)
        {
            if (tr != null && tr.Cells.Count > 0)
            {
                foreach (TableCell td in tr.Cells)
                {
                    //verifica se não é a primeira célula que contém somente um controle
                    if (td.Controls.Count > 1)
                    {
                        Label txtDocente = null;
                        Label txtDisciplina = null;
                        if (td.FindControl(idTextBoxDocente) != null && td.FindControl(idTextBoxDocente) is Label)
                        {
                            txtDocente = (Label)td.FindControl(idTextBoxDocente);
                            if (txtDocente != null)
                                txtDocente.Text = valorTextBoxDocente.Split('-')[0].Trim() + "<br/>" +
                                    valorTextBoxDocente.Substring(valorTextBoxDocente.IndexOf(" - ") + 3).Trim();
                        }

                        if (td.FindControl(idTextBoxDisciplina) != null && td.FindControl(idTextBoxDisciplina) is Label)
                        {
                            txtDisciplina = (Label)td.FindControl(idTextBoxDisciplina);
                            if (txtDisciplina != null)
                                txtDisciplina.Text = valorTextBoxDisciplina.Trim();
                        }

                        if (txtDocente != null && txtDisciplina != null)
                        {
                            if (!string.IsNullOrEmpty(tipo_aula))
                            {
                                if (tipo_aula.ToUpper() == "GLP")
                                {
                                    txtDocente.Style.Clear();
                                    txtDisciplina.Style.Clear();

                                    txtDocente.Style.Add("background-color", COR_GLP);
                                    txtDocente.Style.Add("color", "black");

                                    txtDisciplina.Style.Add("background-color", COR_GLP);
                                    txtDisciplina.Style.Add("color", "black");

                                    (txtDocente.Parent as TableCell).BackColor = Color.FromName(COR_GLP);
                                }
                                else if (tipo_aula.ToUpper() == "GLP_00000000")
                                {
                                    txtDocente.Style.Clear();
                                    txtDisciplina.Style.Clear();

                                    txtDocente.Style.Add("background-color", COR_GLP_00000000);
                                    txtDocente.Style.Add("color", "black");

                                    txtDisciplina.Style.Add("background-color", COR_GLP_00000000);
                                    txtDisciplina.Style.Add("color", "black");

                                    (txtDocente.Parent as TableCell).BackColor = Color.FromName(COR_GLP_00000000);
                                }
                                else if (tipo_aula.ToUpper() == "GLP_99999999")
                                {
                                    txtDocente.Style.Clear();
                                    txtDisciplina.Style.Clear();

                                    txtDocente.Style.Add("background-color", COR_GLP_99999999);
                                    txtDocente.Style.Add("color", "black");

                                    txtDisciplina.Style.Add("background-color", COR_GLP_99999999);
                                    txtDisciplina.Style.Add("color", "black");

                                    (txtDocente.Parent as TableCell).BackColor = Color.FromName(COR_GLP_99999999);
                                }
                            }
                        }

                        if (td.FindControl("txtCodigoDocente_" + idTextBoxDocente) != null &&
                            td.FindControl("txtCodigoDocente_" + idTextBoxDocente) is HtmlInputHidden)
                        {
                            HtmlInputHidden codigoDocente = (HtmlInputHidden)td.FindControl("txtCodigoDocente_" + idTextBoxDocente);
                            if (codigoDocente != null)
                                codigoDocente.Value = num_func;
                        }

                        if (td.FindControl("txtCodigoDisciplina_" + idTextBoxDisciplina) != null &&
                            td.FindControl("txtCodigoDisciplina_" + idTextBoxDisciplina) is HtmlInputHidden)
                        {
                            HtmlInputHidden codigoDisciplina = (HtmlInputHidden)td.FindControl("txtCodigoDisciplina_" + idTextBoxDisciplina);
                            if (codigoDisciplina != null)
                                codigoDisciplina.Value = disciplina;
                        }
                    }
                }
            }
        }
        #endregion
    }
}
