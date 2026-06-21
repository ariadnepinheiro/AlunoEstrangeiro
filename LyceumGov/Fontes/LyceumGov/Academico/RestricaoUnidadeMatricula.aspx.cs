using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using Techne.Web;
using Techne.Lyceum.Net.Modulos;
using System.Linq;
using Techne.Controls;
using System.Data;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Util;
using System.Web;
using System.Web.UI;


namespace Techne.Lyceum.Net.Academico
{
    [
         NavUrl("~/Academico/RestricaoUnidadeMatricula.aspx"),
         ControlText("RestricaoUnidadeMatricula"),
         Title("Restrição/Terminalidade"),
     ]
    public partial class RestricaoUnidadeMatricula : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {                   
                    limpaDadosFiltro();
                    ddlAno.Items.Clear();
                    ddlAno.DataSource = RN.CtvAgendaConfTurnoVaga.ListarAnos();
                    ddlAno.DataBind();
                    ListItem ls = new ListItem("Selecione", string.Empty);
                    ddlAno.Items.Insert(0, ls);

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdRestricao, "Restrição / Terminalidade");
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnCriarRestricao, AcaoControle.novo);
            ControlaAcesso(btnExcluir, AcaoControle.excluir);
        }

        protected void grdRestricao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdRestricao);

        }

        protected void grdRestricao_PageIndexChanged(object sender, EventArgs e)
        {
            carregaGrid();
        }

        protected void chkRegional_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkRegional.Checked)
                {
                    rblTipoFiltroRegional.Visible = true;
                }
                else
                {
                    rblTipoFiltroRegional.Visible = false;
                    rblTipoFiltroRegional.ClearSelection();
                    pnlRegional.Visible = false;
                    tseRegional.SqlWhere = "";
                    tseRegional.ResetValue();
                }
                updatePanel3.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkMunicipio_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string regional = string.Empty;
                if (chkMunicipio.Checked)
                {
                    rblTipoFiltroMunicipio.Visible = true;
                }
                else
                {
                    rblTipoFiltroMunicipio.Visible = false;
                    rblTipoFiltroMunicipio.ClearSelection();
                    pnlMunicipio.Visible = false;
                    tseMunicipio.SqlWhere = " id_regional IS NOT NULL";
                    tseMunicipio.ResetValue();

                    if (!tseRegional.DBValue.IsNull)
                    {
                        regional = tseRegional.DBValue.ToString();
                        tseRegional.SqlWhere = "";
                        tseRegional.ResetValue();
                        tseRegional.DBValue = regional;
                    }

                }
                updatePanel3.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkUnidadeEnsino_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkUnidadeEnsino.Checked)
                {
                    rblTipoFiltroUnidade.Visible = true;
                }
                else
                {
                    rblTipoFiltroUnidade.Visible = false;
                    rblTipoFiltroUnidade.ClearSelection();
                    pnlUnidade.Visible = false;
                    tseUnidade.SqlWhere = " SITUACAO = 'ESTADUAL'";
                    tseUnidade.ResetValue();
                }
                updatePanel3.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkCurso_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkCurso.Checked)
                {
                    rblTipoFiltroCurso.Visible = true;
                }
                else
                {
                    rblTipoFiltroCurso.Visible = false;
                    rblTipoFiltroCurso.ClearSelection();
                    pnlCurso.Visible = false;
                    tseCurso.SqlWhere = "";
                    tseCurso.ResetValue();
                    chkSerie.Visible = false;
                    ddlSerie.Items.Clear();
                    ddlSerie.Visible = false;
                    rblTipoFiltroSerie.Visible = false;
                    rblTipoFiltroSerie.ClearSelection();
                    chkSerie.Checked = false;
                }

                updatePanel3.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkSerie_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkSerie.Checked)
                {
                    rblTipoFiltroSerie.Visible = true;
                }
                else
                {
                    rblTipoFiltroSerie.Visible = false;
                    rblTipoFiltroSerie.ClearSelection();
                    ddlSerie.Visible = false;
                    ddlSerie.Items.Clear();
                }
                updatePanel3.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rblTipoFiltroRegional_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rblTipoFiltroRegional.SelectedValue == "porRegionais")
                {
                    tseRegional.SqlWhere = "";
                    tseRegional.ResetValue();
                    pnlRegional.Visible = true;
                }
                else if (rblTipoFiltroRegional.SelectedValue == "todasRegionais")
                {
                    tseRegional.SqlWhere = "";
                    tseRegional.ResetValue();
                    pnlRegional.Visible = false;
                }
                else
                {
                    pnlRegional.Visible = false;
                    tseRegional.SqlWhere = "";
                    tseRegional.ResetValue();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rblTipoFiltroMunicipio_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rblTipoFiltroMunicipio.SelectedValue == "porMunicipio")
                {
                    pnlMunicipio.Visible = true;
                    if (rblTipoFiltroRegional.Items[1].Selected)//Por Regional
                    {
                        if (!tseRegional.DBValue.IsNull)
                        {
                            tseMunicipio.SqlWhere = " id_regional IS NOT NULL and id_regional='" + tseRegional.DBValue.ToString() + "'";
                        }
                        else
                        {
                            tseRegional.SqlWhere = "";
                            tseMunicipio.ResetValue();
                            tseMunicipio.SqlWhere = " id_regional IS NOT NULL";

                        }
                    }
                    else
                    {
                        tseRegional.SqlWhere = "";
                        tseMunicipio.ResetValue();
                        tseMunicipio.SqlWhere = " id_regional IS NOT NULL";
                    }
                }
                else
                {
                    pnlMunicipio.Visible = false;
                    tseRegional.SqlWhere = "";
                    tseMunicipio.ResetValue();
                    tseMunicipio.SqlWhere = " id_regional IS NOT NULL";

                    if (!string.IsNullOrEmpty(rblTipoFiltroCurso.SelectedValue))//Por Curso
                    {
                        tseCurso.SqlWhere = "";
                        tseCurso.ResetValue();
                        rblTipoFiltroCurso.ClearSelection();
                        rblTipoFiltroSerie.ClearSelection();
                        ddlSerie.Items.Clear();
                        ddlSerie.Visible = false;
                        pnlCurso.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rblTipoFiltroUnidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rblTipoFiltroUnidade.SelectedValue == "porUnidade")
                {
                    pnlUnidade.Visible = true;

                    if (tseRegional.IsValidDBValue && tseMunicipio.IsValidDBValue)
                    {
                        if (!string.IsNullOrEmpty(tseMunicipio.DBValue.ToString()) && !string.IsNullOrEmpty(tseRegional.DBValue.ToString()))
                        {
                            tseUnidade.SqlWhere = "";
                            tseUnidade.SqlWhere = " situacao = 'ESTADUAL' and id_regional='" + tseRegional.DBValue.ToString() + "' and municipio='" + tseMunicipio.DBValue.ToString() + "'";
                        }
                    }
                    if (tseRegional.IsValidDBValue && string.IsNullOrEmpty(tseMunicipio.DBValue.ToString()))
                    {
                        if (!string.IsNullOrEmpty(tseRegional.DBValue.ToString()))
                        {
                            tseUnidade.SqlWhere = "";
                            tseUnidade.SqlWhere = " situacao = 'ESTADUAL' and id_regional='" + tseRegional.DBValue.ToString() + "' ";
                        }
                    }

                    if (tseMunicipio.IsValidDBValue && string.IsNullOrEmpty(tseRegional.DBValue.ToString()))
                    {
                        if (!string.IsNullOrEmpty(tseMunicipio.DBValue.ToString()))
                        {
                            tseUnidade.SqlWhere = "";
                            tseUnidade.SqlWhere = " situacao = 'ESTADUAL' and municipio='" + tseMunicipio.DBValue.ToString() + "' ";
                        }
                    }

                }
                else
                {
                    pnlUnidade.Visible = false;
                    tseUnidade.ResetValue();

                    if (!string.IsNullOrEmpty(rblTipoFiltroCurso.SelectedValue))//Por Curso
                    {
                        tseCurso.SqlWhere = "";
                        tseCurso.ResetValue();
                        rblTipoFiltroCurso.ClearSelection();
                        rblTipoFiltroSerie.ClearSelection();
                        ddlSerie.Items.Clear();
                        ddlSerie.Visible = false;
                        pnlCurso.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rblTipoFiltroCurso_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tseCurso.SqlWhere = "";
                if (rblTipoFiltroCurso.SelectedValue == "porCurso")
                {
                    if (string.IsNullOrEmpty(ddlAno.SelectedValue) || string.IsNullOrEmpty(ddlPeriodo.SelectedValue))
                    {
                        lblMensagem.Text = "O campo ANO/PERÍODO é de preenchimento obrigatório.";
                        rblTipoFiltroCurso.ClearSelection();
                        return;
                    }

                    pnlCurso.Visible = true;
                    chkSerie.Visible = true;
                    tseCurso.SqlWhere = "";
                    tseCurso.SqlWhere = " a.ano = " + ddlAno.SelectedValue + " and a.periodo = " + ddlPeriodo.SelectedValue;

                    if (rblTipoFiltroUnidade.Items[1].Selected)//Por Escola
                    {
                        tseCurso.SqlWhere = tseCurso.SqlWhere + " and uec.UNIDADE_ENS = '" + tseUnidade.DBValue.ToString() + "'";
                    }
                    if (rblTipoFiltroMunicipio.Items[1].Selected)//Por Municipio
                    {
                        tseCurso.SqlWhere = tseCurso.SqlWhere + " and municipio='" + tseMunicipio.DBValue.ToString() + "'";
                    }
                    if (rblTipoFiltroRegional.Items[1].Selected)//Por Regional
                    {
                        tseCurso.SqlWhere = tseCurso.SqlWhere + " and id_regional='" + tseRegional.DBValue.ToString() + "'";
                    }
                }
                else
                {
                    pnlCurso.Visible = false;
                    tseCurso.SqlWhere = "";
                    tseCurso.ResetValue();
                    chkSerie.Visible = false;
                    ddlSerie.Items.Clear();
                    ddlSerie.Visible = false;
                    rblTipoFiltroSerie.Visible = false;
                    chkSerie.Checked = false;
                    rblTipoFiltroSerie.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rblTipoFiltroSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rblTipoFiltroSerie.SelectedValue == "porSerie")
                {
                    ddlSerie.Visible = true;

                    preencheSerie();
                }
                else
                {
                    ddlSerie.Visible = false;
                    ddlSerie.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseRegional_Changed(object sender, ChangedEventArgs args)
        {
            string municipio = string.Empty;
            try
            {
                tseRegional.Visible = true;
                if (Page.IsCallback)
                {
                    return;
                }

                if (!this.tseRegional.DBValue.IsNull)
                {
                    if (this.tseRegional.IsValidDBValue)
                    {
                        if (!tseMunicipio.DBValue.IsNull)
                        {
                            municipio = tseMunicipio.DBValue.ToString();
                        }
                        tseMunicipio.SqlWhere = " id_regional IS NOT NULL";
                        tseMunicipio.ResetValue();
                        tseUnidade.SqlWhere = " SITUACAO = 'ESTADUAL'";
                        tseUnidade.ResetValue();
                        tseCurso.SqlWhere = "";
                        tseCurso.ResetValue();
                        rblTipoFiltroCurso.ClearSelection();
                        rblTipoFiltroSerie.ClearSelection();
                        ddlSerie.Items.Clear();
                        pnlCurso.Visible = false;
                        ddlSerie.Visible = false;

                        if (rblTipoFiltroMunicipio.Items[1].Selected)//Por Municipio
                        {

                            tseMunicipio.SqlWhere = "id_regional IS NOT NULL  and id_regional='" + tseRegional.DBValue.ToString() + "'";

                            if (!string.IsNullOrEmpty(municipio))
                            {
                                tseMunicipio.DBValue = municipio;
                            }
                        }
                        if (rblTipoFiltroUnidade.Items[1].Selected)//Por Escola
                        {
                            tseUnidade.SqlWhere = " situacao = 'ESTADUAL' and id_regional='" + tseRegional.DBValue.ToString() + "'";
                        }

                    }
                }
                updatePanel3.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseMunicipio_Changed(object sender, Controls.ChangedEventArgs args)
        {
            RN.Regional rnRegional = new Techne.Lyceum.RN.Regional();
            DataTable dt = new DataTable();

            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                if (!this.tseMunicipio.DBValue.IsNull)
                {
                    if (this.tseMunicipio.IsValidDBValue)
                    {
                        tseUnidade.SqlWhere = " SITUACAO = 'ESTADUAL'";
                        tseUnidade.ResetValue();
                        tseCurso.SqlWhere = "";
                        tseCurso.ResetValue();
                        rblTipoFiltroUnidade.ClearSelection();
                        rblTipoFiltroCurso.ClearSelection();
                        rblTipoFiltroSerie.ClearSelection();
                        ddlSerie.Items.Clear();
                        pnlCurso.Visible = false;
                        pnlUnidade.Visible = false;
                        ddlSerie.Visible = false;
                        rblTipoFiltroSerie.Visible = false;
                        chkSerie.Checked = false;
                        chkSerie.Visible = false;

                        dt = rnRegional.ListaRegionalPor(tseMunicipio.DBValue.ToString());

                        if (dt.Rows.Count > 1)
                        {
                            tseRegional.SqlWhere = " u.id_regional IS NOT NULL and u.municipio ='" + tseMunicipio.DBValue.ToString() + "'";
                            pnlRegional.Visible = true;
                            chkRegional.Checked = true;
                            rblTipoFiltroRegional.Visible = true;
                            rblTipoFiltroRegional.Items[1].Selected = true;
                        }
                        if (dt.Rows.Count == 1 && (tseRegional.DBValue.IsNull))
                        {
                            tseRegional.SqlWhere = "";
                            tseRegional.ResetValue();
                            tseRegional.SqlWhere = " u.id_regional IS NOT NULL ";
                            tseRegional.DBValue = Convert.ToString(dt.Rows[0]["id_regional"]);
                            pnlRegional.Visible = true;
                            chkRegional.Checked = true;
                            rblTipoFiltroRegional.Visible = true;
                            rblTipoFiltroRegional.Items[1].Selected = true;
                        }
                    }
                }
                updatePanel3.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidade_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                    return;


                if (!this.tseUnidade.DBValue.IsNull)
                {
                    if (this.tseUnidade.IsValidDBValue)
                    {
                        tseCurso.SqlWhere = "";
                        tseCurso.ResetValue();
                        rblTipoFiltroCurso.ClearSelection();
                        rblTipoFiltroSerie.ClearSelection();
                        ddlSerie.Items.Clear();
                        ddlSerie.Visible = false;
                        pnlCurso.Visible = false;

                        if (tseRegional.DBValue.IsNull)
                        {
                            tseRegional.SqlWhere = "";
                            tseRegional.ResetValue();
                            tseRegional.DBValue = tseUnidade["id_regional"].ToString();
                            pnlRegional.Visible = true;
                            chkRegional.Checked = true;
                            rblTipoFiltroRegional.Visible = true;
                            rblTipoFiltroRegional.Items[1].Selected = true;
                        }
                        if (tseMunicipio.DBValue.IsNull)
                        {
                            tseMunicipio.SqlWhere = " id_regional IS NOT NULL";
                            tseMunicipio.ResetValue();
                            tseMunicipio.DBValue = tseUnidade["municipio"].ToString();
                            pnlMunicipio.Visible = true;
                            chkMunicipio.Checked = true;
                            rblTipoFiltroMunicipio.Visible = true;
                            rblTipoFiltroMunicipio.Items[1].Selected = true;
                        }
                    }
                }
                updatePanel3.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseCurso_Changed(object sender, ChangedEventArgs args)
        {
            RN.Serie rnSerie = new Techne.Lyceum.RN.Serie();
            try
            {
                if (Page.IsCallback)
                    return;

                if (!this.tseCurso.DBValue.IsNull)
                {
                    if (this.tseCurso.IsValidDBValue)
                    {
                        if (rblTipoFiltroSerie.Items[1].Selected)//Por Serie
                        {
                            preencheSerie();
                        }
                    }
                }
                updatePanel3.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                limpaDadosFiltro();
                ddlPeriodo.Items.Clear();

                if (!string.IsNullOrEmpty(ddlAno.SelectedValue))
                {
                    ddlPeriodo.DataSource = RN.CtvAgendaConfTurnoVaga.ListarPeriodo(Convert.ToInt32(this.ddlAno.SelectedValue));
                    ddlPeriodo.DataBind();
                    ListItem ls = new ListItem("Selecione", string.Empty);
                    ddlPeriodo.Items.Insert(0, ls);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                limpaDadosFiltro();

                if (!string.IsNullOrEmpty(ddlPeriodo.SelectedValue) && !string.IsNullOrEmpty(ddlAno.SelectedValue))
                {
                    carregaGrid();
                    if (grdRestricao.VisibleRowCount > 0)
                    {
                        btnExcluir.Visible = true;
                    }

                    else
                    {
                        btnExcluir.Visible = false;
                    }
                }

                updatePanel3.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void ddlSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
            updatePanel3.Update();
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            List<string> mensagens = new List<string>();
            List<TceCtvRestricao> listaRestricao = new List<TceCtvRestricao>();
            Button tipoBotao = new Button();
            ValidacaoDados validacao = new ValidacaoDados();
            CtvRestricao rnCtvRestricao = new CtvRestricao();
            RN.DTOs.ResumoRestricoesParaCadastro resumoRestricao = new ResumoRestricoesParaCadastro();

            try
            {
                tipoBotao = (Button)sender;
                if (string.IsNullOrEmpty(ddlAno.SelectedValue))
                {
                    mensagens.Add("O campo ANO é de preenchimento obrigatório.");
                }
                if (string.IsNullOrEmpty(ddlPeriodo.SelectedValue))
                {
                    mensagens.Add("O campo PERIODO é de preenchimento obrigatório.");
                }
                if (!(chkRegional.Checked) && !(chkMunicipio.Checked) && !(chkUnidadeEnsino.Checked) && !(chkCurso.Checked) && !chkTerminalidade.Checked)
                {
                    mensagens.Add("Para efetuar a busca é necessário escolher pelo menos 1(um) filtro.");
                }
                if ((chkRegional.Checked) && (string.IsNullOrEmpty(rblTipoFiltroRegional.SelectedValue)))
                {
                    mensagens.Add("Para Regional é necessario escolher uma das opções Todos ou Por Regional.");
                }
                if ((chkMunicipio.Checked) && (string.IsNullOrEmpty(rblTipoFiltroMunicipio.SelectedValue)))
                {
                    mensagens.Add("Para Município é necessario escolher uma das opções Todos ou Por Município.");
                }
                if ((chkUnidadeEnsino.Checked) && (string.IsNullOrEmpty(rblTipoFiltroUnidade.SelectedValue)))
                {
                    mensagens.Add("Para Unidade de Ensino é necessario escolher uma das opções Todos ou Por Unidade.");
                }
                if ((chkCurso.Checked) && (string.IsNullOrEmpty(rblTipoFiltroCurso.SelectedValue)))
                {
                    mensagens.Add("Para Curso é necessario escolher uma das opções Todos ou Por Curso.");
                }
                if ((chkSerie.Checked) && (string.IsNullOrEmpty(rblTipoFiltroSerie.SelectedValue)))
                {
                    mensagens.Add("Para Serie é necessario escolher uma das opções Todos ou Por Serie.");
                }
                if ((chkRegional.Checked) && ((rblTipoFiltroRegional.Items[1].Selected)) && tseRegional.DBValue.IsNull)
                {
                    mensagens.Add("É necessario escolher uma Regional.");
                }
                if ((chkMunicipio.Checked) && ((rblTipoFiltroMunicipio.Items[1].Selected)) && tseMunicipio.DBValue.IsNull)
                {
                    mensagens.Add("É necessario escolher um Município.");
                }
                if ((chkUnidadeEnsino.Checked) && ((rblTipoFiltroUnidade.Items[1].Selected)) && tseUnidade.DBValue.IsNull)
                {
                    mensagens.Add("É necessario escolher uma Unidade de Ensino.");
                }
                if ((chkCurso.Checked) && ((rblTipoFiltroCurso.Items[1].Selected)) && tseCurso.DBValue.IsNull)
                {
                    mensagens.Add("É necessario escolher um Curso.");
                }
                if ((chkSerie.Checked) && ((rblTipoFiltroSerie.Items[1].Selected)) && (string.IsNullOrEmpty(ddlSerie.SelectedValue)))
                {
                    mensagens.Add("É necessario escolher uma Série.");
                }
                if (mensagens.Count > 0)
                {
                    lblMensagem.Text = mensagens.Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />");
                }
                else
                {
                    if (tipoBotao.Text == "Buscar")
                    {
                        carregaGrid();
                        if (grdRestricao.VisibleRowCount > 0)
                        {
                            btnExcluir.Visible = true;
                        }
                        else
                        {
                            btnExcluir.Visible = false;
                        }
                    }

                    if (tipoBotao.Text == "Criar Restrição")
                    {
                        validacao = rnCtvRestricao.ValidaEMontaListaRestricaoPor(montaFiltro(), User.Identity.Name, out listaRestricao);
                        if (validacao.Valido)
                        {
                            resumoRestricao = rnCtvRestricao.ObtemResumoParaCadastroPor(listaRestricao);

                            lblTotalRegional.Text = resumoRestricao.Regionais.ToString();
                            lblTotalMunicipio.Text = resumoRestricao.Municipios.ToString();
                            lblTotalUnidade.Text = resumoRestricao.UnidadesEnsino.ToString();
                            lblTotalCurso.Text = resumoRestricao.Cursos.ToString();
                            lblTotalSerie.Text = resumoRestricao.Series.ToString();

                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "abrirPopup();", true);
                        }
                        else
                        {
                            lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        }
                    }
                }
                updatePanel3.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            CtvRestricao rnCtvRestricao = new CtvRestricao();
            List<TceCtvRestricao> listaRestricao = new List<TceCtvRestricao>();
            try
            {
                listaRestricao = rnCtvRestricao.MontaListaRestricoesNaoCadastradasPor(montaFiltro(), User.Identity.Name);
                rnCtvRestricao.Insere(listaRestricao);

                lblMensagem.Text = "Restrição/Terminalidade incluída com sucesso.";
                carregaGrid();
                if (grdRestricao.VisibleRowCount > 0)
                {
                    btnExcluir.Visible = true;
                }

                else
                {
                    btnExcluir.Visible = false;
                }

                updatePanel3.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            CtvRestricao rnCtvRestricao = new CtvRestricao();
            List<int> selectedItems = new List<int>();
            try
            {
                selectedItems = this.grdRestricao
                    .GetSelectedFieldValues("ID_RESTRICAO")
                     .Select(x => int.Parse(x.ToString()))
                   .ToList();

                if (selectedItems.Count() > 0)
                {
                    rnCtvRestricao.Remove(selectedItems);

                    lblMensagem.Text = "Pedido(s) excluído(s) com sucesso.";

                    carregaGrid();
                    if (grdRestricao.VisibleRowCount > 0)
                    {
                        btnExcluir.Visible = true;
                    }

                    else
                    {
                        btnExcluir.Visible = false;
                    }
                    grdRestricao.Selection.UnselectAll();
                }
                else
                {
                    lblMensagem.Text = "Para excluir é necessário selecionar pelo menos uma restrição.";
                }

                updatePanel3.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void preencheSerie()
        {
            RN.CtvAgendaConfTurnoVaga rnCtvAgendaConfTurnoVaga = new CtvAgendaConfTurnoVaga();
            int ano = -1;
            int periodo = -1;
            string curso = string.Empty;
            string unidadeEnsino = string.Empty;
            string municipio = string.Empty;
            string regional = string.Empty;

            try
            {
                ddlSerie.Items.Clear();

                if (!string.IsNullOrEmpty(ddlAno.SelectedValue))
                {
                    ano = Convert.ToInt32(ddlAno.SelectedValue);
                }

                if (!string.IsNullOrEmpty(ddlPeriodo.SelectedValue))
                {
                    periodo = Convert.ToInt32(ddlPeriodo.SelectedValue);
                }

                if (!tseCurso.DBValue.IsNull)
                {
                    curso = tseCurso.DBValue.ToString();
                }

                if (!tseUnidade.DBValue.IsNull)
                {
                    unidadeEnsino = tseUnidade.DBValue.ToString();
                }

                if (!tseMunicipio.DBValue.IsNull)
                {
                    municipio = tseMunicipio.DBValue.ToString();
                }

                if (!tseRegional.DBValue.IsNull)
                {
                    regional = tseRegional.DBValue.ToString();
                }

                ddlSerie.DataSource = rnCtvAgendaConfTurnoVaga.ObtemListaSeriesComAgendaPor(ano, periodo, curso, unidadeEnsino, municipio, regional);
                ddlSerie.DataBind();
                ListItem ls = new ListItem("Selecione", string.Empty);
                ddlSerie.Items.Insert(0, ls);
                updatePanel3.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void carregaGrid()
        {
            try
            {
                if (ddlPeriodo.SelectedValue != "Selecione" && !string.IsNullOrEmpty(ddlAno.SelectedValue))
                {
                    CtvRestricao rnCtvRestricao = new CtvRestricao();
                    FiltroRestricaoTerminalidade filtroRestricao = new FiltroRestricaoTerminalidade();

                    filtroRestricao = montaFiltro();

                    grdRestricao.DataSource = rnCtvRestricao.ListaPor(filtroRestricao);
                    grdRestricao.DataBind();
                    pnlRestricao.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private FiltroRestricaoTerminalidade montaFiltro()
        {
            FiltroRestricaoTerminalidade filtro = new FiltroRestricaoTerminalidade();
            try
            {
                filtro.Ano = Convert.ToInt32(ddlAno.SelectedValue);
                filtro.Periodo = Convert.ToInt32(ddlPeriodo.SelectedValue);
                if (!tseRegional.DBValue.IsNull)
                {
                    filtro.PorRegional = true;
                    filtro.Regional = tseRegional.DBValue.ToString();
                }
                if (!tseMunicipio.DBValue.IsNull)
                {
                    filtro.PorMunicipio = true;
                    filtro.Municipio = tseMunicipio.DBValue.ToString();
                }
                if (!tseUnidade.DBValue.IsNull)
                {
                    filtro.PorUnidadeEnsino = true;
                    filtro.UnidadeEnsino = tseUnidade.DBValue.ToString();
                }
                if (!tseCurso.DBValue.IsNull)
                {
                    filtro.PorCurso = true;
                    filtro.Curso = tseCurso.DBValue.ToString();
                }
                if (!string.IsNullOrEmpty(ddlSerie.SelectedValue))
                {
                    filtro.PorSerie = true;
                    filtro.Serie = Convert.ToInt32(ddlSerie.SelectedValue);
                }
                else
                {

                    filtro.Serie = -1;
                }

                filtro.Terminalidade = chkTerminalidade.Checked;

                return filtro;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                return null;
            }
        }
        private void limpaDadosFiltro()
        {
            chkRegional.Checked = false;
            rblTipoFiltroRegional.ClearSelection();
            rblTipoFiltroRegional.Visible = false;
            tseRegional.SqlWhere = "";
            tseRegional.ResetValue();
            pnlRegional.Visible = false;

            chkMunicipio.Checked = false;
            rblTipoFiltroMunicipio.ClearSelection();
            rblTipoFiltroMunicipio.Visible = false;
            tseMunicipio.SqlWhere = " id_regional IS NOT NULL";
            tseMunicipio.ResetValue();
            pnlMunicipio.Visible = false;

            chkUnidadeEnsino.Checked = false;
            rblTipoFiltroUnidade.ClearSelection();
            rblTipoFiltroUnidade.Visible = false;
            tseUnidade.SqlWhere = " SITUACAO = 'ESTADUAL'";
            tseUnidade.ResetValue();
            pnlUnidade.Visible = false;

            chkCurso.Checked = false;
            rblTipoFiltroCurso.ClearSelection();
            rblTipoFiltroCurso.Visible = false;
            tseCurso.SqlWhere = "";
            tseCurso.ResetValue();
            pnlCurso.Visible = false;

            chkSerie.Checked = false;
            rblTipoFiltroSerie.ClearSelection();
            rblTipoFiltroSerie.Visible = false;
            ddlSerie.Items.Clear();
            ddlSerie.Visible = false;

            chkTerminalidade.Checked = false;
            grdRestricao.DataSource = null;
            grdRestricao.DataBind();
            pnlRestricao.Visible = false;
            btnExcluir.Visible = false;
        }
    }
}
