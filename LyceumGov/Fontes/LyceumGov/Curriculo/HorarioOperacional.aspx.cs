using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using Techne.Web;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.Data;
using Techne.Data;
using DevExpress.Web.ASPxGridView;
using Techne.Lyceum.RN;
using System.Collections.Generic;
using System.Linq;
using Techne.Lyceum.RN.Util;
using Techne.Controls;

namespace Techne.Lyceum.Net.Curriculo
{
    [
         NavUrl("~/Curriculo/HorarioOperacional.aspx"),
         ControlText("HorarioOperacional"),
         Title("Horário Operacional"),
     ]

    public partial class HorarioOperacional : TPage
    {
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdHorarioOperacional, "Horários");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!tseUnidadeEnsino.IsValidDBValue || tseUnidadeEnsino.DBValue.IsNull)
                {
                    tseUnidadeFisica.ResetValue();
                    tseUnidadeFisica.Enabled = false;
                }

                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    DesabilitaControles();
                    if (Request.QueryString.Keys.Count > 0)
                    {
                        byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                        string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                        ObterDadosQueryString(decodedText);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdHorarioOperacional);
            ControlaAcesso(btnSalvar, AcaoControle.editar);
            ControlaAcesso(btnExcluir, AcaoControle.excluir);
        }

        private void ObterDadosQueryString(string queryString)
        {
            string[] listaDados = queryString.Split('&');
            string _grade = string.Empty;
            CursoDuracao rnCursoDuracao = new CursoDuracao();
            QueryTable qt;

            try
            {
                foreach (string dados in listaDados)
                {
                    if (dados.IndexOf("grade_id") >= 0)
                        _grade = dados.Substring(dados.LastIndexOf('=') + 1);
                }

                qt = RN.Serie.ConsultaFiltrosHorOper(_grade);

                if (qt.Rows.Count > 0)
                {
                    PreencherDadosSession();
                    tseCurso.DBValue = qt.Rows[0]["curso"];
                    CarregaTurno(tseUnidadeEnsino.DBValue.ToString(), tseCurso.DBValue.ToString());
                    ddlTurno.SelectedValue = qt.Rows[0]["turno"].ToString();
                    ddlTurno_SelectedIndexChanged(null, null);
                    ddlCurriculo.SelectedValue = qt.Rows[0]["curriculo"].ToString() + "|" + qt.Rows[0]["ano"].ToString();
                    ddlCurriculo_SelectedIndexChanged(null, null);
                    ddlSerie.SelectedValue = qt.Rows[0]["serie"].ToString();
                    ddlSerie_SelectedIndexChanged(null, null);
                    tseUnidadeEnsino.DBValue = qt.Rows[0]["unidade_responsavel"];
                    tseUnidadeFisica.DBValue = qt.Rows[0]["faculdade"];

                    Pesquisar();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void PreencherDadosSession()
        {
            RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
            try
            {
                if (sessao != null)
                {
                    if (!string.IsNullOrEmpty(sessao.Regional))
                    {
                        tseRegional.DBValue = sessao.Regional;
                        if (!tseRegional.IsValidDBValue)
                        {
                            tseRegional.Msg = string.Empty;
                            tseRegional.ResetValue();
                        }
                    }
                    if (!string.IsNullOrEmpty(sessao.Municipio))
                    {
                        tseMunicipio.DBValue = sessao.Municipio;
                        if (!tseMunicipio.IsValidDBValue)
                        {
                            tseMunicipio.Msg = string.Empty;
                            tseMunicipio.ResetValue();
                        }
                    }
                    if (!string.IsNullOrEmpty(sessao.Escola))
                    {
                        tseUnidadeEnsino.DBValue = sessao.Escola;

                        if (!tseUnidadeEnsino.IsValidDBValue)
                        {
                            tseUnidadeEnsino.Msg = string.Empty;
                            tseUnidadeEnsino.ResetValue();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected string FormataHora(object hora)
        {
            string horaFormatada = null;
            DateTime dataHora;
            try
            {
                if (DateTime.TryParse(Convert.ToString(hora), out dataHora))
                {
                    horaFormatada = dataHora.ToString("HH:mm");
                }

                return horaFormatada;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                return horaFormatada;
            }
        }

        private void Pesquisar()
        {
            DateTime horaini_aulaGridAux = DateTime.MinValue;
            DateTime horafim_aulaGridAux = DateTime.MinValue;
            DataRowView drv;
            TimeSpan ts = new TimeSpan();
            DateTime horaini_aulaGrid;
            DateTime horafim_aulaGrid;
            bool achou = false;
            RN.HorarioOperacional rnHorarioOperacional = new Techne.Lyceum.RN.HorarioOperacional();

            try
            {
                string[] curriculo = ddlCurriculo.SelectedValue.Split('|');
                lblDuracao.Visible = true;
                ddlDuracao.Visible = true;
                ddlDuracao.ClearSelection();

                CarregaGrid();
                grdHorarioOperacional.CancelEdit();

                for (int i = 0; i < grdHorarioOperacional.VisibleRowCount; i++)
                {
                    drv = (DataRowView)grdHorarioOperacional.GetRow(i);
                    DateTime.TryParse(drv["HORAINI_AULA"] == null ? string.Empty : drv["HORAINI_AULA"].ToString(), out horaini_aulaGridAux);
                    DateTime.TryParse(drv["HORAFIM_AULA"] == null ? string.Empty : drv["HORAFIM_AULA"].ToString(), out horafim_aulaGridAux);
                    horaini_aulaGrid = new DateTime(1899, 12, 30, horaini_aulaGridAux.Hour, horaini_aulaGridAux.Minute, 0);
                    horafim_aulaGrid = new DateTime(1900, 12, 30, horafim_aulaGridAux.Hour, horafim_aulaGridAux.Minute, 0);

                    ts = horafim_aulaGrid.Subtract(horaini_aulaGrid);

                    if (ts.Hours == 1)
                    {
                        ddlDuracao.SelectedValue = "60";
                        achou = true;
                    }

                    if (ts.Minutes != 0)
                    {
                        foreach (ListItem item in ddlDuracao.Items)
                        {
                            if (item.Value != "")
                            {
                                if (ts.Minutes == int.Parse(item.Value))
                                {
                                    achou = true;
                                }
                            }
                        }
                        if (!achou)
                        {
                            lblMensagem.Text =
                                "Horário Operacional com duração errada. Favor verificar.";
                            ddlDuracao.Enabled = true;
                            ddlDuracao.SelectedValue = "";
                            btnExcluir.Visible = true;
                            return;
                        }
                        else
                        {
                            ddlDuracao.SelectedValue = ts.Minutes.ToString();
                            break;
                        }
                    }
                }

                if (achou)
                {
                    btnExcluir.Visible = true;
                }
                else
                {
                    btnExcluir.Visible = false;
                }

                if (rnHorarioOperacional.ExisteHoraAulaPor(tseUnidadeEnsino.DBValue.ToString(), ddlTurno.SelectedValue, tseCurso.DBValue.ToString(), curriculo[0], Convert.ToDecimal(ddlSerie.SelectedValue)))
                {
                    ddlDuracao.Enabled = false;
                }
                else
                {
                    ddlDuracao.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
            try
            {
                grdHorarioOperacional.CancelEdit();
                grdHorarioOperacional.Visible = false;
                btnSalvar.Visible = false;
                btnExcluir.Visible = false;
                lblDuracao.Visible = false;
                ddlDuracao.Visible = false;
                tseMunicipio.ResetValue();
                tseUnidadeEnsino.ResetValue();
                tseUnidadeFisica.ResetValue();
                tseCurso.ResetValue();
                ddlTurno.Items.Clear();
                ddlCurriculo.Items.Clear();
                ddlSerie.Items.Clear();

                if (sessao != null)
                {
                    if (!tseRegional.DBValue.IsNull)
                    {
                        if (tseRegional.IsValidDBValue)
                        {
                            sessao.Regional = Convert.ToString(tseRegional.DBValue);
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;

                            tseUnidadeEnsino.ResetValue();
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
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
            try
            {
                grdHorarioOperacional.CancelEdit();
                grdHorarioOperacional.Visible = false;
                btnSalvar.Visible = false;
                btnExcluir.Visible = false;
                lblDuracao.Visible = false;
                ddlDuracao.Visible = false;
                tseUnidadeEnsino.ResetValue();
                tseUnidadeFisica.ResetValue();
                tseCurso.ResetValue();
                ddlTurno.Items.Clear();
                ddlCurriculo.Items.Clear();
                ddlSerie.Items.Clear();

                if (sessao != null)
                {
                    if (!tseMunicipio.DBValue.IsNull)
                    {
                        if (tseMunicipio.IsValidDBValue)
                        {
                            sessao.Municipio = Convert.ToString(tseMunicipio.DBValue);

                            sessao.Escola = string.Empty;
                            tseUnidadeEnsino.ResetValue();
                        }
                        else
                        {
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                        }
                    }
                    else
                    {
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeEnsino_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
            try
            {
                grdHorarioOperacional.CancelEdit();
                grdHorarioOperacional.Visible = false;
                btnSalvar.Visible = false;
                btnExcluir.Visible = false;
                lblDuracao.Visible = false;
                ddlDuracao.Visible = false;
                tseCurso.ResetValue();
                ddlTurno.Items.Clear();
                ddlCurriculo.Items.Clear();
                ddlSerie.Items.Clear();

                if (!tseUnidadeEnsino.DBValue.IsNull && tseUnidadeEnsino.IsValidDBValue)
                {
                    tseRegional.Value = tseUnidadeEnsino["id_regional"];
                    tseMunicipio.Value = tseUnidadeEnsino["municipio"];

                    if (sessao != null)
                    {
                        sessao.Escola = Convert.ToString(tseUnidadeEnsino.DBValue);
                        sessao.Regional = Convert.ToString(tseRegional.DBValue);
                        sessao.Municipio = tseMunicipio.DBValue.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeFisica_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                grdHorarioOperacional.CancelEdit();
                grdHorarioOperacional.Visible = false;
                btnSalvar.Visible = false;
                btnExcluir.Visible = false;
                lblDuracao.Visible = false;
                ddlDuracao.Visible = false;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseCurso_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                grdHorarioOperacional.CancelEdit();
                grdHorarioOperacional.Visible = false;
                btnSalvar.Visible = false;
                btnExcluir.Visible = false;
                lblDuracao.Visible = false;
                ddlDuracao.Visible = false;
                ddlTurno.Items.Clear();
                ddlCurriculo.Items.Clear();
                ddlSerie.Items.Clear();

                if (!this.tseCurso.DBValue.IsNull)
                {
                    CarregaTurno(tseUnidadeEnsino.DBValue.ToString(), tseCurso.DBValue.ToString());
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                grdHorarioOperacional.CancelEdit();
                grdHorarioOperacional.Visible = false;
                btnSalvar.Visible = false;
                btnExcluir.Visible = false;
                ddlCurriculo.Items.Clear();
                ddlSerie.Items.Clear();
                ddlDuracao.Items.Clear();

                if (!string.IsNullOrEmpty(ddlTurno.SelectedValue))
                {
                    CarregaCurriculo(tseCurso.DBValue.ToString(), ddlTurno.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlCurriculo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CursoDuracao rnCursoDuracao = new CursoDuracao();
                grdHorarioOperacional.CancelEdit();
                grdHorarioOperacional.Visible = false;
                btnSalvar.Visible = false;
                btnExcluir.Visible = false;
                ddlSerie.Items.Clear();
                lblDuracao.Visible = false;
                ddlDuracao.Visible = false;
                ddlDuracao.Items.Clear();

                string[] curriculo = ddlCurriculo.SelectedValue.Split('|');

                if (!this.tseCurso.DBValue.IsNull && !string.IsNullOrEmpty(ddlTurno.SelectedValue) && !string.IsNullOrEmpty(curriculo[0]))
                {
                    CarregaSerie(tseCurso.DBValue.ToString(), ddlTurno.SelectedValue, curriculo[0]);

                    this.ddlDuracao.DataSource = rnCursoDuracao.ListaPor(int.Parse(curriculo[1]), tseCurso.DBValue.ToString(),
                                                                                         ddlTurno.SelectedValue);

                    ListItem ls = new ListItem("Selecione", "");
                    ddlDuracao.Items.Insert(0, ls);
                    this.ddlDuracao.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                grdHorarioOperacional.CancelEdit();
                grdHorarioOperacional.Visible = false;
                btnSalvar.Visible = false;
                btnExcluir.Visible = false;
                lblDuracao.Visible = false;
                ddlDuracao.Visible = false;

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnPesquisar_Click(object sender, ImageClickEventArgs e)
        {
            Pesquisar();
        }

        protected void ddlDuracao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(ddlDuracao.SelectedValue))
                {
                    grdHorarioOperacional.Visible = true;
                    btnSalvar.Visible = true;
                }
                else
                {
                    grdHorarioOperacional.Visible = false;
                    btnSalvar.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            List<RN.Entidades.LyHorOper> listaHorarios = new List<RN.Entidades.LyHorOper>();
            RN.HorarioOperacional rnHorarioOperacional = new Techne.Lyceum.RN.HorarioOperacional();
            ASPxTextBox txtHoraIni = new ASPxTextBox();
            ASPxTextBox txtHoraFim = new ASPxTextBox();
            RN.Entidades.LyHorOper horarios;
            DateTime dateIniAux = DateTime.MinValue;
            DateTime dateFimAux = DateTime.MinValue;
            DateTime dateini = DateTime.MinValue;
            DateTime datefim = DateTime.MinValue;
            ValidacaoDados validacao = new ValidacaoDados();

            try
            {
                string[] curriculo = ddlCurriculo.SelectedValue.Split('|');

                for (int i = 0; i < grdHorarioOperacional.VisibleRowCount; i++)
                {
                    dateini = DateTime.MinValue;
                    datefim = DateTime.MinValue;

                    txtHoraIni = (grdHorarioOperacional.FindRowCellTemplateControl(i, (GridViewDataColumn)grdHorarioOperacional.Columns["HORAINI_AULA"], "txtBox") as ASPxTextBox);
                    txtHoraFim = (grdHorarioOperacional.FindRowCellTemplateControl(i, (GridViewDataColumn)grdHorarioOperacional.Columns["HORAFIM_AULA"], "txtBoxFim") as ASPxTextBox);

                    if (DateTime.TryParse(txtHoraIni.Text, out dateIniAux))
                    {
                        dateini = new DateTime(1899, 12, 30, dateIniAux.Hour, dateIniAux.Minute, 0);
                    }
                    if (DateTime.TryParse(txtHoraFim.Text, out dateFimAux))
                    {
                        datefim = new DateTime(1899, 12, 30, dateFimAux.Hour, dateFimAux.Minute, 0);
                    }

                    horarios = new RN.Entidades.LyHorOper
                    {
                        Aula = Convert.ToDecimal(grdHorarioOperacional.GetRowValues(i, "AULA")) == 0 ? -1 : (Convert.ToDecimal(grdHorarioOperacional.GetRowValues(i, "AULA"))),
                        Curriculo = Convert.ToString(curriculo[0]),
                        Curso = Convert.ToString(tseCurso.DBValue),
                        DiaSemana = Convert.ToDecimal(grdHorarioOperacional.GetRowValues(i, "DIA_SEMANA").ToString()),
                        DuracaoAula = string.IsNullOrEmpty(ddlDuracao.SelectedValue) ? -1 : Convert.ToDecimal(ddlDuracao.SelectedValue),
                        Faculdade = Convert.ToString(tseUnidadeEnsino.DBValue),
                        HorainiAula = dateini,
                        HorafimAula = datefim,
                        Ordem = Convert.ToDecimal(grdHorarioOperacional.GetRowValues(i, "ORDEM")),
                        Serie = string.IsNullOrEmpty(ddlSerie.SelectedValue) ? -1 : Convert.ToDecimal(ddlSerie.SelectedValue),
                        Turno = Convert.ToString(ddlTurno.SelectedValue),
                    };

                    listaHorarios.Add(horarios);
                }

                validacao = rnHorarioOperacional.Valida(listaHorarios, Convert.ToInt32(curriculo[1]));

                if (validacao.Valido)
                {
                    rnHorarioOperacional.Salva(listaHorarios);
                    Pesquisar();
                    lblMensagem.Text = "Horário atualizado com sucesso.";
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

        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            RN.HorarioOperacional rnHorarioOperacional = new Techne.Lyceum.RN.HorarioOperacional();
            string faculdade = string.Empty;
            string turno = string.Empty;
            string curso = string.Empty;
            //string curriculo = string.Empty;
            decimal serie = -1;
            ValidacaoDados validacao = new ValidacaoDados();

            try
            {
                string[] curriculo = ddlCurriculo.SelectedValue.Split('|');

                faculdade = Convert.ToString(tseUnidadeEnsino.DBValue);
                turno = ddlTurno.SelectedValue;
                curso = Convert.ToString(tseCurso.DBValue);
                serie = string.IsNullOrEmpty(ddlSerie.SelectedValue) ? -1 : Convert.ToDecimal(ddlSerie.SelectedValue);

                validacao = rnHorarioOperacional.ValidaRemocao(faculdade, turno, curso, curriculo[0], serie);

                if (validacao.Valido)
                {
                    rnHorarioOperacional.Remove(faculdade, turno, curso, curriculo[0], serie);
                    Pesquisar();
                    lblMensagem.Text = "Horário excluido com sucesso.";
                    ddlDuracao.Visible = false;
                    lblDuracao.Visible = false;
                    grdHorarioOperacional.CancelEdit();
                    grdHorarioOperacional.Visible = false;
                    btnSalvar.Visible = false;
                    btnExcluir.Visible = false;
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

        private void CarregaGrid()
        {
            RN.HorarioOperacional rnHorarioOperacional = new Techne.Lyceum.RN.HorarioOperacional();
            try
            {
                if (tseUnidadeFisica.IsValidDBValue && !tseUnidadeFisica.DBValue.IsNull
                && tseCurso.IsValidDBValue && !tseCurso.DBValue.IsNull
                && !string.IsNullOrEmpty(ddlTurno.SelectedValue)
                && !string.IsNullOrEmpty(ddlCurriculo.SelectedValue)
                && !string.IsNullOrEmpty(ddlSerie.SelectedValue))
                {
                    string[] curriculo = ddlCurriculo.SelectedValue.Split('|');
                    grdHorarioOperacional.Visible = true;
                    btnSalvar.Visible = true;
                    btnExcluir.Visible = true;
                    lblDuracao.Visible = true;
                    ddlDuracao.Visible = true;

                    grdHorarioOperacional.DataSource = rnHorarioOperacional.ListaPor(tseUnidadeFisica.DBValue.ToString(), tseCurso.DBValue.ToString(), ddlTurno.SelectedValue, curriculo[0], ddlSerie.SelectedValue);
                    grdHorarioOperacional.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaTurno(string unidadeEnsino, string curso)
        {
            try
            {
                ddlTurno.DataSource = RN.Turno.ConsultarPorUnidadeEnsinoECurso(unidadeEnsino, curso);
                ddlTurno.DataBind();
                ListItem ls = new ListItem("Selecione", "");
                ddlTurno.Items.Insert(0, ls);

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaCurriculo(string curso, string turno)
        {
            RN.Curriculo rnCurriculo = new Techne.Lyceum.RN.Curriculo();
            try
            {
                ddlCurriculo.DataSource = rnCurriculo.ListaCurriculoHorarioOperacionalPor(curso, turno);
                ddlCurriculo.DataBind();
                ListItem ls = new ListItem("Selecione", "");
                ddlCurriculo.Items.Insert(0, ls);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaSerie(string curso, string turno, string curriculo)
        {
            RN.Serie rnSerie = new Techne.Lyceum.RN.Serie();
            try
            {
                ddlSerie.DataSource = rnSerie.ListaSeriePor(curso, turno, curriculo);
                ddlSerie.DataBind();
                ListItem ls = new ListItem("Selecione", "");
                ddlSerie.Items.Insert(0, ls);

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        private void DesabilitaControles()
        {
            tseRegional.Mode = ControlMode.View;
            tseMunicipio.Mode = ControlMode.View;
            tseUnidadeEnsino.Mode = ControlMode.View;
            tseUnidadeFisica.Mode = ControlMode.View;
            tseCurso.Mode = ControlMode.View;
            ddlTurno.Enabled = false;
            ddlCurriculo.Enabled = false;
            ddlSerie.Enabled = false;

        }

        protected void btnVoltarTurma_Click(object sender, EventArgs e)
        {
            string chaveOrigem = Request.QueryString["ChaveOrigem"];
            Response.Redirect("Turma.aspx?Chave=" + chaveOrigem);
        }
    }
}

