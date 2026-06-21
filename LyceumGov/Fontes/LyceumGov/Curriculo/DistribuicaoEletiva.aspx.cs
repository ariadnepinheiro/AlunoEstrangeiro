using System;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.Data;
using Techne.Controls;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using Techne.Web;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using System.Web.UI;
using System.Collections.Generic;
using System.Data;


namespace Techne.Lyceum.Net.Curriculo
{
    [NavUrl("~/Curriculo/DistribuicaoEletiva.aspx"), ControlText("DistribuicaoEletiva"), Title("Distribuição de Eletiva")]

    public partial class DistribuicaoEletiva : TPage
    {
      
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.lblMensagem.Text = string.Empty;
                RN.PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();

                if (!this.IsPostBack)
                {
                    ImageButton[] controles = new ImageButton[] { };
                    ControlarVisibilidadeControle(controles);

                   
                        if (!rnPeriodoLetivo.EhPeriodoDistribuicaoEletiva())
                        {
                            this.lblMensagem.Text = "O período para lançamento da Distribuição de Eletiva encerrado ou ainda não iniciou. Verifique!";
                            pnGeral.Visible = false;
                            divEdit.Visible = false;
                            return;
                        }
                        else
                        {
                            pnGeral.Visible = true;
                            divEdit.Visible = true;
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
            try
            {
                if ((!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue) &&
                            (!tseAno.DBValue.IsNull && tseAno.IsValidDBValue) &&
                            (!tsePeriodo.DBValue.IsNull && tsePeriodo.IsValidDBValue) &&
                            (!tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue) &&
                            (!tseTurno.DBValue.IsNull && tseTurno.IsValidDBValue))
                {
                    CriaTurmas();
                    ManterTurma();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (var botao in botoes)
            {
                botao.Visible = true;
            }

            ControlaAcesso(btnSalvar, AcaoControle.editar);

        }

        private void RetiraVisibilidadeBotao()
        {
            btnSalvar.Visible = false;

        }

        protected void tseMunicipio_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;

                tseAno.ResetValue();
                tsePeriodo.ResetValue();
                tseCurso.ResetValue();
                tseTurno.ResetValue();

                dvTurmas.Visible = false;
                this.lblMensagem.Text = string.Empty;
                ImageButton[] controles = new ImageButton[] { };
                ControlarVisibilidadeControle(controles);

                if (this.Page.IsCallback)
                {
                    return;
                }

                var sessao = SessaoUsuario.GetSessaoUsuario();

                if (sessao != null)
                {
                    if (!this.tseMunicipio.DBValue.IsNull)
                    {
                        if (this.tseMunicipio.IsValidDBValue)
                        {
                            sessao.Municipio = Convert.ToString(this.tseMunicipio.DBValue);

                            sessao.Escola = string.Empty;
                            this.tseUnidadeResponsavel.ResetValue();
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

        protected void tseUnidadeResponsavel_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;

                dvTurmas.Visible = false;
                this.lblMensagem.Text = string.Empty;

                ImageButton[] controles = new ImageButton[] { };
                ControlarVisibilidadeControle(controles);


                tseAno.ResetValue();
                tsePeriodo.ResetValue();
                tseCurso.ResetValue();
                tseTurno.ResetValue();

                var sessao = SessaoUsuario.GetSessaoUsuario();

                if (!this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (this.tseUnidadeResponsavel.IsValidDBValue)
                    {
                        if (!this.tseUnidadeResponsavel["unidade_ens"].IsNull)
                        {
                            sessao.Escola = Convert.ToString(this.tseUnidadeResponsavel.DBValue);
                            this.tseMunicipio.Value = this.tseUnidadeResponsavel["municipio"];
                        }

                        this.lblMensagem.Text = string.Empty;

                    }
                    else
                    {
                        if (sessao != null)
                        {
                            sessao.Escola = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Coordenadoria = string.Empty;
                        }


                        this.lblMensagem.Text = "Unidade de Ensino não cadastrada.";

                    }
                }
                else
                {
                    if (sessao != null)
                    {
                        sessao.Escola = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Coordenadoria = string.Empty;
                    }


                    this.lblMensagem.Text = "Favor consultar uma unidade de ensino.";

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseAno_Changed(object sender, ChangedEventArgs args)
        {
            try
            {

                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;

                dvTurmas.Visible = false;
                this.lblMensagem.Text = string.Empty;
                ImageButton[] controles = new ImageButton[] { };
                ControlarVisibilidadeControle(controles);

                tsePeriodo.ResetValue();
                tseCurso.ResetValue();
                tseTurno.ResetValue();
                this.lblMensagem.Text = string.Empty;

                var sessao = SessaoUsuario.GetSessaoUsuario();

                if (!this.tseAno.DBValue.IsNull)
                {
                    if (!this.tseAno.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Ano não cadastrado.";

                    }
                }
                else
                {


                    this.lblMensagem.Text = "Favor consultar um ano.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void tsePeriodo_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;
                dvTurmas.Visible = false;
                this.lblMensagem.Text = string.Empty;
                ImageButton[] controles = new ImageButton[] { };
                ControlarVisibilidadeControle(controles);
               
                tseCurso.ResetValue();
                tseTurno.ResetValue();
                this.lblMensagem.Text = string.Empty;

                if (!this.tsePeriodo.DBValue.IsNull)
                {
                    if (!this.tsePeriodo.IsValidDBValue)
                    {

                        this.lblMensagem.Text = "Período não cadastrado.";

                    }
                    
                }
                else
                {

                    this.lblMensagem.Text = "Favor consultar um Período.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void tseCurso_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;
                dvTurmas.Visible = false;
                this.lblMensagem.Text = string.Empty;
                ImageButton[] controles = new ImageButton[] { };
                ControlarVisibilidadeControle(controles);
                tseTurno.ResetValue();

                if (!this.tseCurso.DBValue.IsNull)
                {
                    if (!this.tseCurso.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Curso não cadastrado.";
                    }
                }
                else
                {

                    this.lblMensagem.Text = "Favor consultar um Curso.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseTurno_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;
                dvTurmas.Visible = false;
                this.lblMensagem.Text = string.Empty;
                ImageButton[] controles = new ImageButton[] { };
                RN.PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();

                if (!this.tseTurno.DBValue.IsNull)
                {
                    if (this.tseTurno.IsValidDBValue)
                    {                     

                        if ((!tseAno.DBValue.IsNull && tseAno.IsValidDBValue) && (!tsePeriodo.DBValue.IsNull && tsePeriodo.IsValidDBValue))
                        {
                            dvTurmas.Visible = true;

                            if (!rnPeriodoLetivo.EhPeriodoDistribuicaoEletivaPor(Convert.ToInt32(tseAno.DBValue), Convert.ToInt32(tsePeriodo.DBValue)))
                            {
                                this.lblMensagem.Text = "O período para lançamento da Distribuição de Eletiva encerrado ou ainda não iniciou. Verifique!";
                               
                            }
                            else
                            {
                                controles = new ImageButton[] { btnSalvar };
                            }
                        }                        

                    }
                    else
                    {

                        this.lblMensagem.Text = "Turno não cadastrado.";

                    }
                }
                else
                {

                    this.lblMensagem.Text = "Favor consultar um Turno.";
                }

                ControlarVisibilidadeControle(controles);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public void CriaTurmas()
        {

            try
            {

                RN.Turma rnTurma = new Techne.Lyceum.RN.Turma();
                DataTable dtTurmas = new DataTable();
                RN.Turmas.CardapioEletiva rnCardapioEletiva = new Techne.Lyceum.RN.Turmas.CardapioEletiva();

                dtTurmas = rnTurma.ListaTurmaRegularOfereceEletivaPor(Convert.ToDecimal(tseAno.DBValue), Convert.ToDecimal(tsePeriodo.DBValue), tseCurso.DBValue.ToString(), tseTurno.DBValue.ToString(), tseUnidadeResponsavel.DBValue.ToString());

                rpTurmas.DataSource = dtTurmas;
                rpTurmas.DataBind();


            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ManterTurma()
        {
            List<RN.DTOs.DadosDistruicaoEletivas> lsEletivas = new List<Techne.Lyceum.RN.DTOs.DadosDistruicaoEletivas>();

            try
            {
                RN.Turma rnTurma = new Techne.Lyceum.RN.Turma();
                List<Techne.Lyceum.RN.DTOs.DadosDistruicaoEletivas> lista = new List<Techne.Lyceum.RN.DTOs.DadosDistruicaoEletivas>();

                RN.DTOs.DadosDistruicaoEletivas dados = new Techne.Lyceum.RN.DTOs.DadosDistruicaoEletivas();


                lsEletivas = rnTurma.ObtemListaDistribuicaoEletivaPor(Convert.ToDecimal(tseAno.DBValue), Convert.ToDecimal(tsePeriodo.DBValue), tseCurso.DBValue.ToString(), tseTurno.DBValue.ToString(), tseUnidadeResponsavel.DBValue.ToString());

                foreach (RepeaterItem item in rpTurmas.Items)
                {
                    Label lblTurma = (Label)item.FindControl("lblTurma");
                    HiddenField hdnReferencia = (HiddenField)item.FindControl("hdnReferencia");

                    DropDownList ddlgrupo1 = (DropDownList)item.FindControl("ddlgrupo1");
                    DropDownList ddlgrupo2 = (DropDownList)item.FindControl("ddlgrupo2");
                    DropDownList ddlgrupo3 = (DropDownList)item.FindControl("ddlgrupo3");

                    if (lsEletivas.Count > 0)
                    {
                        ddlgrupo1.SelectedValue = lsEletivas.Find(x => x.TurmaReferencia == lblTurma.Text && (hdnReferencia.Value == "1" ? true : false) == x.Referencia).DisciplinaGrupo1;
                        ddlgrupo2.SelectedValue = lsEletivas.Find(x => x.TurmaReferencia == lblTurma.Text && (hdnReferencia.Value == "1" ? true : false) == x.Referencia).DisciplinaGrupo2;
                        ddlgrupo3.SelectedValue = lsEletivas.Find(x => x.TurmaReferencia == lblTurma.Text && (hdnReferencia.Value == "1" ? true : false) == x.Referencia).DisciplinaGrupo3;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rpTurmas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            try
            {

                RN.Turmas.CardapioEletiva rnCardapioEletiva = new Techne.Lyceum.RN.Turmas.CardapioEletiva();

                if ((item.ItemType == ListItemType.Item) ||

                    (item.ItemType == ListItemType.AlternatingItem))
                {

                    HiddenField hdnSerie = (HiddenField)item.FindControl("hdnSerie");
                    HiddenField hdnReferencia = (HiddenField)item.FindControl("hdnReferencia");

                    if (hdnSerie != null)
                    {
                        ListItem itemSelecione1 = new ListItem("Selecione G1", string.Empty);
                        ListItem itemSelecione2 = new ListItem("Selecione G2", string.Empty);
                        ListItem itemSelecione3 = new ListItem("Selecione G3", string.Empty);
                        Label lblTurmaReferencia = (Label)item.FindControl("lblTurmaReferencia");

                        DropDownList ddlGrupo1 = (DropDownList)item.FindControl("ddlGrupo1");
                        DropDownList ddlGrupo2 = (DropDownList)item.FindControl("ddlGrupo2");
                        DropDownList ddlGrupo3 = (DropDownList)item.FindControl("ddlGrupo3");

                        ddlGrupo1.DataSource = rnCardapioEletiva.ListaDisciplinasEletivasPor(Convert.ToDecimal(tseAno.DBValue), Convert.ToDecimal(tsePeriodo.DBValue), tseCurso.DBValue.ToString(), tseTurno.DBValue.ToString(), Convert.ToInt32(hdnSerie.Value), tseUnidadeResponsavel.DBValue.ToString(), 1);
                        ddlGrupo2.DataSource = rnCardapioEletiva.ListaDisciplinasEletivasPor(Convert.ToDecimal(tseAno.DBValue), Convert.ToDecimal(tsePeriodo.DBValue), tseCurso.DBValue.ToString(), tseTurno.DBValue.ToString(), Convert.ToInt32(hdnSerie.Value), tseUnidadeResponsavel.DBValue.ToString(), 2);
                        ddlGrupo3.DataSource = rnCardapioEletiva.ListaDisciplinasEletivasPor(Convert.ToDecimal(tseAno.DBValue), Convert.ToDecimal(tsePeriodo.DBValue), tseCurso.DBValue.ToString(), tseTurno.DBValue.ToString(), Convert.ToInt32(hdnSerie.Value), tseUnidadeResponsavel.DBValue.ToString(), 3);

                        ddlGrupo1.DataBind();
                        ddlGrupo2.DataBind();
                        ddlGrupo3.DataBind();

                        ddlGrupo1.Items.Insert(0, itemSelecione1);
                        ddlGrupo2.Items.Insert(0, itemSelecione2);
                        ddlGrupo3.Items.Insert(0, itemSelecione3);

                        if (hdnReferencia.Value == "0")
                        {
                            lblTurmaReferencia.Text = string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                lblMensagem.Text = ex.Message;
            }
        }


        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.lblMensagem.Text = string.Empty;
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Turma rnTurma = new Techne.Lyceum.RN.Turma();
                List<RN.DTOs.DadosDistruicaoEletivas> lsEletivas = new List<Techne.Lyceum.RN.DTOs.DadosDistruicaoEletivas>();
                List<RN.DTOs.DadosDistruicaoEletivas> lsEletivasCompleta = new List<Techne.Lyceum.RN.DTOs.DadosDistruicaoEletivas>();
                RN.DTOs.DadosDistruicaoEletivas dados = new Techne.Lyceum.RN.DTOs.DadosDistruicaoEletivas();
                RN.DTOs.DadosDistruicaoEletivas dadosBase = new Techne.Lyceum.RN.DTOs.DadosDistruicaoEletivas(); 
                
                foreach (RepeaterItem item in rpTurmas.Items)
                {
                    dados = new Techne.Lyceum.RN.DTOs.DadosDistruicaoEletivas();

                    Label lblTurma = (Label)item.FindControl("lblTurma");
                    HiddenField hdnSerie = (HiddenField)item.FindControl("hdnSerie");
                    HiddenField hdnReferencia = (HiddenField)item.FindControl("hdnReferencia");
                    DropDownList ddlGrupo1 = (DropDownList)item.FindControl("ddlGrupo1");
                    DropDownList ddlGrupo2 = (DropDownList)item.FindControl("ddlGrupo2");
                    DropDownList ddlGrupo3 = (DropDownList)item.FindControl("ddlGrupo3");
                    Label lblTurmaReferencia = (Label)item.FindControl("lblTurmaReferencia");

                    dados.Ano = (!tseAno.DBValue.IsNull && tseAno.IsValidDBValue) ? Convert.ToInt32(tseAno.DBValue) : -1;
                    dados.Semestre = (!tsePeriodo.DBValue.IsNull && tsePeriodo.IsValidDBValue) ? Convert.ToInt32(tsePeriodo.DBValue) : -1;
                    dados.TurmaReferencia = !lblTurma.Text.IsNullOrEmptyOrWhiteSpace() ? lblTurma.Text : null;
                    dados.Serie = !hdnSerie.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnSerie.Value) : -1;
                    dados.DisciplinaGrupo1 = !ddlGrupo1.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlGrupo1.SelectedValue : null;
                    dados.DisciplinaGrupo2 = !ddlGrupo2.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlGrupo2.SelectedValue : null;
                    dados.DisciplinaGrupo3 = !ddlGrupo3.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlGrupo3.SelectedValue : null;
                    dados.Referencia = !hdnReferencia.Value.IsNullOrEmptyOrWhiteSpace() && hdnReferencia.Value == "1" ? true : false;
                    dados.DescricaoTurmaReferencia = !hdnReferencia.Value.IsNullOrEmptyOrWhiteSpace() && hdnReferencia.Value == "1" ? lblTurmaReferencia.Text : lblTurma.Text;

                    dadosBase = rnTurma.ObtemDistribuicaoEletivaPor(dados.Ano, dados.Semestre, dados.TurmaReferencia, dados.Referencia.Value);

                    if (dados.DisciplinaGrupo1 != dadosBase.DisciplinaGrupo1 || dados.DisciplinaGrupo2 != dadosBase.DisciplinaGrupo2 || dados.DisciplinaGrupo3 != dadosBase.DisciplinaGrupo3)
                    {
                        lsEletivas.Add(dados);
                    }
                    lsEletivasCompleta.Add(dados);
                }

                validacao = rnTurma.ValidaDistribuicaoEletiva(lsEletivas, lsEletivasCompleta, User.Identity.Name);

                if (validacao.Valido)
                {
                    rnTurma.SalvaDistribuicaoEletiva(lsEletivas, User.Identity.Name);

                    lblMensagem.Text = "Distribuição de Eletiva salva com sucesso";
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }



    }
}
