using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/OfertaEscolar.aspx")]
    [ControlText("OfertaEscolar")]
    [Title("Oferta Escolar")]

    public partial class OfertaEscolar : TPage
    {
        public object Lista(object censo, object ano)
        {
            RN.Pedagogico.TrilhaAprendizagemEscola rnTrilhaAprendizagemEscola = new Techne.Lyceum.RN.Pedagogico.TrilhaAprendizagemEscola();

            if (censo.ToString() != string.Empty && ano != null)
            {
                return rnTrilhaAprendizagemEscola.ListaPor(Convert.ToString(censo),Convert.ToInt32(ano));
            }
            return null;

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                DateTime dtInicio = Convert.ToDateTime(System.Configuration.ConfigurationSettings.AppSettings["DataInicioOfertaEscola"]);
                DateTime dtFim = Convert.ToDateTime(System.Configuration.ConfigurationSettings.AppSettings["DataFimOfertaEscola"]);

                if (!this.IsPostBack)
                {
                    CarregaAno();
                    pnlGrid.Visible = false;
                    pnAbaNovo.Visible = false;
                    tseMunicipio.ResetValue();
                    tseUnidadeResponsavel.ResetValue();
                    tseModalidade.ResetValue();

                    if (DateTime.Now < dtInicio || DateTime.Now > dtFim)
                    {
                        lblMensagem.Text = string.Format("O periodo para escolha de trilhas pela escola é de {0} até {1}.", dtInicio.ToString("dd/MM/yyyy HH:mm"), dtFim.ToString("dd/MM/yyyy HH:mm"));
                        pnGeral.Visible = false;
                        return;
                    }
                    else
                    {
                        pnGeral.Visible = true;
                    }

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTrilha, string.Empty);
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {

            ControlaAcesso(grdTrilha);
            ControlaAcesso(grdTrilha, AcaoControle.excluir, "btnExcluir");
            ControlaAcesso(btnSalvarOferta, AcaoControle.novo);
            ControlaAcesso(btnSim, AcaoControle.excluir);
            ControlaAcesso(btnNao, AcaoControle.excluir);
            AcessoGrid();
        }

        private void CarregaAno()
        {
            ddlAno.Items.Clear();
            ListItem item = new ListItem("Selecione", string.Empty);
            ddlAno.DataSource = RN.PeriodoLetivo.ListarAnos();
            ddlAno.DataBind();
            ddlAno.Items.Insert(0, item);
        }

        protected void HabilitaPnlNovo(object sender, EventArgs e)
        {
            try
            {
                pnAbaNovo.Visible = true;
                LimpaCampos();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimpaCampos()
        {
            tseTrilha.ResetValue();
            tseItinerario.ResetValue();
            tseModalidade.ResetValue();
            ddlTurno.Items.Clear();
            hdnIdTrilha.Value = string.Empty;
            hdnCurso.Value = string.Empty;
            hdnTurno.Value = string.Empty;
        }

        protected void tseModalidade_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                tseItinerario.ResetValue();
                tseTrilha.ResetValue();
                ddlTurno.Items.Clear();


                if (!this.tseModalidade.DBValue.IsNull)
                {
                    if (this.tseModalidade.IsValidDBValue)
                    {
                        if (!tseUnidadeResponsavel.DBValue.IsNull)
                        {
                            CarregaTurno();
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Modalidade não encontrada.";
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
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }
                pnlGrid.Visible = false;
                pnAbaNovo.Visible = false;
                LimpaCampos();

                var sessao = SessaoUsuario.GetSessaoUsuario();
                tseUnidadeResponsavel.ResetValue();


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

        protected void tseUnidadeResponsavel_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                RN.Pedagogico.TrilhaAprendizagemEscola rnTrilhaAprendizagemEscola = new Techne.Lyceum.RN.Pedagogico.TrilhaAprendizagemEscola();
                RN.Pedagogico.TrilhaAprendizagemEscolaFinalizada rnTrilhaAprendizagemEscolaFinalizada = new Techne.Lyceum.RN.Pedagogico.TrilhaAprendizagemEscolaFinalizada();
                string mensagem;
                var sessao = SessaoUsuario.GetSessaoUsuario();
                pnlGrid.Visible = false;
                pnAbaNovo.Visible = false;
                LimpaCampos();
                btnFinalizar.Visible = false;
                lblMensagem.Text = string.Empty;

                if (!this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (this.tseUnidadeResponsavel.IsValidDBValue)
                    {
                        if (!this.tseUnidadeResponsavel["unidade_ens"].IsNull)
                        {
                            sessao.Escola = Convert.ToString(this.tseUnidadeResponsavel.DBValue);
                            this.tseMunicipio.Value = this.tseUnidadeResponsavel["municipio"];

                            if (rnTrilhaAprendizagemEscola.PodeParticiparPor(tseUnidadeResponsavel.DBValue.ToString(), Convert.ToInt32(ddlAno.SelectedValue)))
                            {                               
                                pnlGrid.Visible = true;

                                if (rnTrilhaAprendizagemEscolaFinalizada.PossuiFinalizacaoPor(tseUnidadeResponsavel.DBValue.ToString(), Convert.ToInt32(ddlAno.SelectedValue), out mensagem))
                                {
                                    lblMensagem.Text = mensagem;
                                    btnFinalizar.Visible = false;
                                    grdTrilha.Columns[0].Visible = false;
                                }
                                else
                                {
                                    btnFinalizar.Visible = true;
                                    grdTrilha.Columns[0].Visible = true;
                                }
                            }
                            else
                            {
                                lblMensagem.Text = "Prezado(a) Diretor(a), <br> Sua Unidade Escolar não participa deste processo, por não ocorrer a oferta das Modalidades: Educação de Jovens e Adultos (EJA- Módulo III) e Ensino Médio Regular em horário parcial (2ª série).";
                                return;
                            }
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

        private void CarregaTurno()
        {
            ListItem item = new ListItem("Selecione", string.Empty);
            RN.Turma rnTurma = new Turma();

            ddlTurno.Items.Clear();
            ddlTurno.DataSource = rnTurma.ListaTurnosOfertaPor(Convert.ToInt32(ddlAno.SelectedValue), tseUnidadeResponsavel.DBValue.ToString(), tseModalidade.DBValue.ToString());
            ddlTurno.DataBind();
            ddlTurno.Items.Insert(0, item);
        }

        protected void btnSalvarOferta_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Pedagogico.TrilhaAprendizagemEscola rnTrilhaAprendizagemEscola = new Techne.Lyceum.RN.Pedagogico.TrilhaAprendizagemEscola();
                RN.Pedagogico.Entidades.TrilhaAprendizagemEscola trilha = new Techne.Lyceum.RN.Pedagogico.Entidades.TrilhaAprendizagemEscola();
                string modalidade;
                int categoriaId;

                trilha.Ano = !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlAno.SelectedValue) : -1;
                trilha.Curso = (!this.tseTrilha.DBValue.IsNull && this.tseTrilha.IsValidDBValue) ? tseTrilha.DBValue.ToString() : null;
                trilha.Turno = !ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTurno.SelectedValue : null;
                trilha.Censo = (!this.tseUnidadeResponsavel.DBValue.IsNull && this.tseUnidadeResponsavel.IsValidDBValue) ? Convert.ToString(tseUnidadeResponsavel.DBValue) : null;
                trilha.UsuarioId = User.Identity.Name;
                modalidade = (!this.tseModalidade.DBValue.IsNull && this.tseModalidade.IsValidDBValue) ? Convert.ToString(tseModalidade.DBValue) : null;

                categoriaId = (!this.tseItinerario.DBValue.IsNull && this.tseItinerario.IsValidDBValue) ? Convert.ToInt32(tseItinerario["CATEGORIAITINERARIOFORMATIVOID"].ToString()) : -1;

                validacao = rnTrilhaAprendizagemEscola.Valida(trilha, modalidade, categoriaId);

                if (validacao.Valido)
                {
                    rnTrilhaAprendizagemEscola.Insere(trilha);

                    grdTrilha.DataBind();
                    LimpaCampos();
                    pnAbaNovo.Visible = false;

                    lblMensagem.Text = "Oferta cadastrada com sucesso.";
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

        protected void grdTrilha_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTrilha.Settings.ShowFilterRow = false;
        }

        protected void grdTrilha_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdTrilha.Settings.ShowFilterRow = false;
        }

        protected void grdTrilha_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTrilha);
            ControlaAcesso(grdTrilha, AcaoControle.excluir, "btnExcluir");
            ControlaAcesso(btnSalvarOferta, AcaoControle.novo);
            AcessoGrid();
        }

        protected void grdTrilha_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            try
            {
                if (e.ButtonID == "btnExcluir")
                {
                    hdnIdTrilha.Value = Convert.ToString(grdTrilha.GetRowValues(e.VisibleIndex, "TRILHAAPRENDIZAGEM_ESCOLAID"));
                    hdnCurso.Value = Convert.ToString(grdTrilha.GetRowValues(e.VisibleIndex, "CURSO"));

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);
                }
            }
            catch (Exception ex)
            {
                this.pucConfirmarTrilha.ShowOnPageLoad = false;
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSim_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Pedagogico.TrilhaAprendizagemEscola rnTrilhaAprendizagemEscola = new Techne.Lyceum.RN.Pedagogico.TrilhaAprendizagemEscola();

                int id = 0;

                id = Convert.ToInt32(hdnIdTrilha.Value);

                validacao = rnTrilhaAprendizagemEscola.ValidaRemocao(id, tseUnidadeResponsavel.DBValue.ToString(), hdnCurso.Value, Convert.ToInt32(ddlAno.SelectedValue));

                if (validacao.Valido)
                {
                    rnTrilhaAprendizagemEscola.Remove(id);
                    grdTrilha.DataBind();
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    grdTrilha.CancelEdit();
                }

                this.pucConfirmarTrilha.ShowOnPageLoad = false;
            }

            catch (Exception ex)
            {
                this.pucConfirmarTrilha.ShowOnPageLoad = false;
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNao_Click(object sender, EventArgs e)
        {
            this.pucConfirmarTrilha.ShowOnPageLoad = false;
            grdTrilha.CancelEdit();
        }

        protected void AcessoGrid()
        {
            if (grdTrilha != null)
            {
                HtmlInputImage img = (HtmlInputImage)grdTrilha.FindHeaderTemplateControl(grdTrilha.Columns[""], "btnNovoGridTrilha");


                if (img != null)
                {
                    img.Visible = Permission.AllowInsert;
                }
            }
        }

         protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tseUnidadeResponsavel.ResetValue();
                pnlGrid.Visible = false;
                pnAbaNovo.Visible = false;
                LimpaCampos();
                

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

         protected void btnFinalizar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Pedagogico.TrilhaAprendizagemEscolaFinalizada rnTrilhaAprendizagemEscolaFinalizada = new Techne.Lyceum.RN.Pedagogico.TrilhaAprendizagemEscolaFinalizada();
                RN.Pedagogico.Entidades.TrilhaAprendizagemEscolaFinalizada trilhaAprendizagemEscolaFinalizada = new Techne.Lyceum.RN.Pedagogico.Entidades.TrilhaAprendizagemEscolaFinalizada();

                LimpaCampos();
                pnAbaNovo.Visible = false;


                trilhaAprendizagemEscolaFinalizada.Ano = !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlAno.SelectedValue) : -1;
                trilhaAprendizagemEscolaFinalizada.Censo = (tseUnidadeResponsavel.IsValidDBValue && !tseUnidadeResponsavel.DBValue.IsNull) ? tseUnidadeResponsavel.DBValue.ToString() : null;
                trilhaAprendizagemEscolaFinalizada.DataFinalizacao = DateTime.Now;
                trilhaAprendizagemEscolaFinalizada.UsuarioId = User.Identity.Name;


                validacao = rnTrilhaAprendizagemEscolaFinalizada.ValidaFinalizacao(trilhaAprendizagemEscolaFinalizada);

                if (validacao.Valido)
                {
                    rnTrilhaAprendizagemEscolaFinalizada.Finaliza(trilhaAprendizagemEscolaFinalizada);

                    lblMensagem.Text = "Finalização realizada com sucesso.";

                    string mensagemFinalizacao = string.Empty;
                    if (rnTrilhaAprendizagemEscolaFinalizada.PossuiFinalizacaoPor(tseUnidadeResponsavel.DBValue.ToString(), Convert.ToInt32(ddlAno.SelectedValue), out mensagemFinalizacao))
                    {
                        this.lblMensagem.Text = mensagemFinalizacao;
                        btnFinalizar.Visible = false;
                        grdTrilha.Columns[0].Visible = false;
                    }
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
