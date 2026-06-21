using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.Net.Basico;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxGridView;
using Techne.Web;
using DevExpress.Web.ASPxClasses;
using System.Data;
using System.Threading;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/AlunosRetornoAula.aspx"), ControlText("AlunosRetornoAula"), Title("Controle de Frequência - Retorno Presencial")]

    public partial class AlunosRetornoAula : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.lblMensagem.Text = string.Empty;
                

                if (!this.IsPostBack)
                {
                    hdnErro.Value = string.Empty;
                    ImageButton[] controles = new ImageButton[] { };
                    ControlarVisibilidadeControle(controles);
                    dvTurmas.Visible = false;
                    CarregaAno();
                    
                    
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            btnSalvar.Visible = false;

            foreach (var botao in botoes)
            {
                botao.Visible = true;
            }

            ControlaAcesso(btnSalvar, AcaoControle.editar);

        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            try
            {
                if ((!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue) &&
                            !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() &&
                            !ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace() &&
                            !ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace() &&
                            !dtAula.Text.IsNullOrEmptyOrWhiteSpace() && hdnErro.Value.IsNullOrEmptyOrWhiteSpace())
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

        private void CarregaTurno()
        {
            RN.Turno rnTurno = new Techne.Lyceum.RN.Turno();
            try
            {
                ddlTurno.Items.Clear();
                ddlTurno.DataSource = rnTurno.ListaTurnosAtivosPor(tseUnidadeResponsavel.DBValue.ToString(), Convert.ToInt32(ddlAno.SelectedValue));
                ddlTurno.Items.Insert(0, new ListItem("Selecione", string.Empty));
                ddlTurno.DataBind();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaAno()
        {
            RN.PeriodoLetivo rnPeriodoLetivo = new Techne.Lyceum.RN.PeriodoLetivo();

            ddlAno.Items.Clear();
            ListItem item = new ListItem("Selecione", string.Empty);
            ddlAno.DataSource = rnPeriodoLetivo.ListaAnoAberto();
            ddlAno.DataBind();
            ddlAno.Items.Insert(0, item);
        }


        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                hdnErro.Value = string.Empty;
                dvTurmas.Visible = false;
                this.ddlPeriodo.Items.Clear();
                ddlTurno.Items.Clear();
                dtAula.Text = string.Empty;
                this.lblMensagem.Text = string.Empty;
                ImageButton[] controles = new ImageButton[] { };

                if (!string.IsNullOrEmpty(ddlAno.SelectedValue))
                {
                    ListItem item = new ListItem("Selecione", string.Empty);
                    this.ddlPeriodo.DataSource = RN.PeriodoLetivo.ListarPeriodo(this.ddlAno.SelectedValue);
                    this.ddlPeriodo.DataBind();
                    this.ddlPeriodo.Items.Insert(0, item);
                   
                }

                ControlarVisibilidadeControle(controles);

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
                hdnErro.Value = string.Empty;
                dvTurmas.Visible = false;
                this.lblMensagem.Text = string.Empty;
                ImageButton[] controles = new ImageButton[] { };
                RN.PeriodoLetivo rnPeriodoLetivo = new Techne.Lyceum.RN.PeriodoLetivo();

                if (!string.IsNullOrEmpty(ddlPeriodo.SelectedValue))
                {
                    CarregaTurno();

                     string[] dataInicioFim = new string[2];

                    //Busca datas de inicio e fim do ano letivo
                    dataInicioFim = rnPeriodoLetivo.ObtemDataInicioFimAulaPor(Convert.ToInt32(ddlAno.SelectedValue),Convert.ToInt32(ddlPeriodo.SelectedValue));
                    

                    dtAula.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                    if (Convert.ToInt32(ddlAno.SelectedValue) == 2021)
                    {
                        dtAula.MinDate = new DateTime(2021, 10, 25);
                        dtAula.Enabled = true;
                    }
                    else
                    {
                        if (DateTime.Now.Date >= Convert.ToDateTime(dataInicioFim[0]))
                        {
                            dtAula.MinDate = new DateTime(Convert.ToDateTime(dataInicioFim[0]).Year, Convert.ToDateTime(dataInicioFim[0]).Month, Convert.ToDateTime(dataInicioFim[0]).Day);
                            dtAula.Enabled = true;
                        }
                        else
                        {
                            dtAula.Enabled = false;
                        }
                    }
                }
                ControlarVisibilidadeControle(controles);


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
                dvTurmas.Visible = false;
                this.lblMensagem.Text = string.Empty;
                ImageButton[] controles = new ImageButton[] { };
                hdnErro.Value = string.Empty;

                if (!dtAula.Text.IsNullOrEmptyOrWhiteSpace())
                {
                    if (!string.IsNullOrEmpty(ddlTurno.SelectedValue))
                    {
                        controles = new ImageButton[] { btnSalvar };

                        dvTurmas.Visible = true;
                    }
                }
                else
                {
                    lblMensagem.Text = "Favor selecionar uma data.";
                    ddlTurno.ClearSelection();
                }

                ControlarVisibilidadeControle(controles);

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimparCampos()
        {

            ddlAno.ClearSelection();
            ddlPeriodo.ClearSelection();
            ddlTurno.Items.Clear();
            dtAula.Text = string.Empty;

        }

        protected void tseUnidadeResponsavel_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }
                hdnErro.Value = string.Empty;
                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
                LimparCampos();
                dvTurmas.Visible = false;
                lblMensagem.Text = string.Empty;
                ImageButton[] controles = new ImageButton[] { };

                if (!tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (tseUnidadeResponsavel.IsValidDBValue)
                    {
                        if (sessao != null)
                        {
                            sessao.Escola = Convert.ToString(tseUnidadeResponsavel.DBValue);

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

                        lblMensagem.Text = "Unidade de Ensino não cadastrada.";

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


                    lblMensagem.Text = "Favor consultar uma unidade de ensino.";

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

                dtTurmas = rnTurma.ListaTurmaAbertaRetornoPresencialPor(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), ddlTurno.SelectedValue, tseUnidadeResponsavel.DBValue.ToString());

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
            List<RN.Turmas.Entidades.AlunosAusentes> lsTurmas = new List<Techne.Lyceum.RN.Turmas.Entidades.AlunosAusentes>();

            try
            {
                RN.Turmas.AlunosAusentes rnAlunosAusentes = new Techne.Lyceum.RN.Turmas.AlunosAusentes();
                List<Techne.Lyceum.RN.DTOs.DadosDistruicaoEletivas> lista = new List<Techne.Lyceum.RN.DTOs.DadosDistruicaoEletivas>();

                RN.DTOs.DadosDistruicaoEletivas dados = new Techne.Lyceum.RN.DTOs.DadosDistruicaoEletivas();


                lsTurmas = rnAlunosAusentes.ListaAlunosAusentes(tseUnidadeResponsavel.DBValue.ToString(), Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), ddlTurno.SelectedValue, Convert.ToDateTime(dtAula.Text));

                foreach (RepeaterItem item in rpTurmas.Items)
                {
                    Label lblTurma = (Label)item.FindControl("lblTurma");

                    TextBox txtMatriculados = (TextBox)item.FindControl("txtMatriculados");
                    TextBox txtPresente = (TextBox)item.FindControl("txtPresente");
                    TextBox txtAusente = (TextBox)item.FindControl("txtAusente");
                    TextBox txtCasosAmparo = (TextBox)item.FindControl("txtCasosAmparo");
                    TextBox txtCovid = (TextBox)item.FindControl("txtCovid");
                    Label lblFrequencia = (Label)item.FindControl("lblFrequencia");
                    Label lblInfrequencia = (Label)item.FindControl("lblInfrequencia");
                    Label lblAmparo = (Label)item.FindControl("lblAmparo");
                    Label lblCovid = (Label)item.FindControl("lblCovid");

                    if (lsTurmas.Count > 0)
                    {
                        decimal matriculados = lsTurmas.Find(x => x.Turma == lblTurma.Text).QuantidadeMatriculados;
                        decimal presentes = lsTurmas.Find(x => x.Turma == lblTurma.Text).QuantidadePresentes == null ? -1 : lsTurmas.Find(x => x.Turma == lblTurma.Text).QuantidadePresentes.Value;
                        decimal amparados = lsTurmas.Find(x => x.Turma == lblTurma.Text).QuantidadeAmparados == null ? -1 : lsTurmas.Find(x => x.Turma == lblTurma.Text).QuantidadeAmparados.Value;
                        decimal afastamentos = lsTurmas.Find(x => x.Turma == lblTurma.Text).QuantidadeAfastamentosCovid == null ? -1 : lsTurmas.Find(x => x.Turma == lblTurma.Text).QuantidadeAfastamentosCovid.Value;


                        txtMatriculados.Text = matriculados.ToString();
                        txtPresente.Text = presentes == -1 ? string.Empty : presentes.ToString();
                        txtAusente.Text = presentes == -1 ? string.Empty : (matriculados - (presentes + amparados + afastamentos)).ToString();
                        txtCasosAmparo.Text = amparados == -1 ? "0" : amparados.ToString();
                        txtCovid.Text = afastamentos == -1 ? "0" : afastamentos.ToString();

                        lblInfrequencia.Text = matriculados == 0 || presentes == -1 ? "-" : (((matriculados - (presentes + amparados + afastamentos)) / matriculados) * 100).ToString("0.00");
                        lblFrequencia.Text = matriculados == 0 || presentes == -1 ? "-" : ((presentes / matriculados) * 100).ToString("0.00");
                        lblAmparo.Text = matriculados == 0 ? "-" : (amparados == -1 ? "0.00" : ((amparados / matriculados) * 100).ToString("0.00"));
                        lblCovid.Text = matriculados == 0 ? "-" : (afastamentos == -1 ? "0.00" : ((afastamentos / matriculados) * 100).ToString("0.00"));

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
                RN.Turmas.AlunosAusentes rnAlunosAusentes = new Techne.Lyceum.RN.Turmas.AlunosAusentes();
                List<RN.Turmas.Entidades.AlunosAusentes> lsTurmas = new List<RN.Turmas.Entidades.AlunosAusentes>();
                RN.Turmas.Entidades.AlunosAusentes dados = new Techne.Lyceum.RN.Turmas.Entidades.AlunosAusentes();

                foreach (RepeaterItem item in rpTurmas.Items)
                {
                    dados = new Techne.Lyceum.RN.Turmas.Entidades.AlunosAusentes();

                    Label lblTurma = (Label)item.FindControl("lblTurma");
                    TextBox txtMatriculados = (TextBox)item.FindControl("txtMatriculados");
                    TextBox txtPresente = (TextBox)item.FindControl("txtPresente");
                    TextBox txtCasosAmparo = (TextBox)item.FindControl("txtCasosAmparo");
                    TextBox txtCovid = (TextBox)item.FindControl("txtCovid");

                    dados.Censo = (!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue) ? tseUnidadeResponsavel.DBValue.ToString() : null;
                    dados.Ano = !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlAno.SelectedValue) : -1;
                    dados.Periodo = !ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlPeriodo.SelectedValue) : -1;
                    dados.Turma = !lblTurma.Text.IsNullOrEmptyOrWhiteSpace() ? lblTurma.Text : null;
                    dados.QuantidadeMatriculados = !txtMatriculados.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtMatriculados.Text) : -1;
                    dados.QuantidadePresentes = !txtPresente.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtPresente.Text) : -1;
                    dados.QuantidadeAmparados = !txtCasosAmparo.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtCasosAmparo.Text) : -1;
                    dados.QuantidadeAfastamentosCovid = !txtCovid.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtCovid.Text) : -1;
                    dados.DataLancamento = !dtAula.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(dtAula.Text) : DateTime.MinValue;
                    dados.UsuarioID = User.Identity.Name;
                    dados.Turno = !ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTurno.SelectedValue.ToString() : null;
                    dados.IdRegional = (!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue) ? Convert.ToInt32(tseUnidadeResponsavel["id_regional"]) : -1;

                    lsTurmas.Add(dados);
                }

                validacao = rnAlunosAusentes.Valida(lsTurmas);

                if (validacao.Valido)
                {
                    rnAlunosAusentes.Salva(lsTurmas);

                    lblMensagem.Text = "Controle de Frequência salvo com sucesso";
                    hdnErro.Value = string.Empty;
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    hdnErro.Value = "Erro";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                hdnErro.Value = "Erro";
            }
        }

        protected void dtAula_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                ImageButton[] controles = new ImageButton[] { };
                hdnErro.Value = string.Empty;
                this.lblMensagem.Text = string.Empty;
                dvTurmas.Visible = false;
                ddlTurno.ClearSelection();
                ControlarVisibilidadeControle(controles);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
