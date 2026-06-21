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
using Techne.Controls;
using Techne.Lyceum.RN.DTOs;

namespace Techne.Lyceum.Net.Matricula
{
    [NavUrl("~/Matricula/ConvocacaoRegres.aspx")]
    [ControlText("Convocar Matricula Especial - Regres")]
    [Title("Convocar Matricula Especial - Regres")]

    public partial class ConvocacaoRegres : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                if (!IsPostBack)
                {                    
                    CarregaAno();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaAno()
        {
            ddlAno.Items.Clear();
            ListItem item = new ListItem("Selecione", string.Empty);
            ddlAno.DataSource = RN.PeriodoLetivo.ListarAnos();
            ddlAno.DataBind();
            ddlAno.Items.Insert(0, item);
        }

        private void CarregaTurno()
        {
            try
            {
                RN.ControleVaga rnControleVaga = new ControleVaga();

                if (!ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && (tseCurso.IsValidDBValue && !tseCurso.DBValue.IsNull))
                {
                    ddlTurno.Items.Clear();
                    ListItem item = new ListItem("Selecione", string.Empty);
                    ddlTurno.DataSource = rnControleVaga.ListaTurnoMatriculaRegresPor(Convert.ToInt32(ddlAno.SelectedValue), tseCurso.DBValue.ToString());
                    ddlTurno.DataBind();
                    ddlTurno.Items.Insert(0, item);
                }
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
                pnlDados.Visible = false;
                pnlAviso.Visible = false;
                if (!string.IsNullOrEmpty(ddlAno.SelectedValue))
                {
                    
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                RN.Matriculas.MatriculaEspecial rnMatriculas = new Techne.Lyceum.RN.Matriculas.MatriculaEspecial();
                pnlDados.Visible = false;
                pnlAviso.Visible = false;

                if (!ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && (tseCurso.IsValidDBValue && !tseCurso.DBValue.IsNull))
                {
                    lblVagasDisponiveis.Text = rnMatriculas.RetornaSaldoPor(tseCurso.DBValue.ToString(), ddlTurno.SelectedValue, Convert.ToInt32(ddlAno.SelectedValue)).ToString();

                    lblTotalFila.Text = rnMatriculas.RetornaQuantidadeFilaPor(tseCurso.DBValue.ToString(), ddlTurno.SelectedValue, Convert.ToInt32(ddlAno.SelectedValue)).ToString();

                    pnlDados.Visible = true;
                }
                else
                {
                    lblMensagem.Text = "Para efetuar a busca é necessário informar o Ano, a Disciplina e o  Turno.";
                }
                                
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void tseCurso_Changed(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                pnlDados.Visible = false;
                pnlAviso.Visible = false;
                if (tseCurso.IsValidDBValue && !tseCurso.DBValue.IsNull)
                {
                    CarregaTurno();

                }
                else if (!tseCurso.DBValue.IsNull)
                {
                    lblMensagem.Text = "Escolaridade não cadastrada.";
                }
                else
                {
                    lblMensagem.Text = "Favor consultar uma escolaridade.";

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void btnConvocar_Click(object sender, EventArgs e)
        {
            try
            {
                RN.Matriculas.MatriculaEspecial rnMatriculas = new Techne.Lyceum.RN.Matriculas.MatriculaEspecial();
                ValidacaoDados validacao = new ValidacaoDados();
                string aviso;

                validacao = rnMatriculas.ValidaConvocacao(tseCurso.DBValue.ToString(), ddlTurno.SelectedValue, Convert.ToInt32(ddlAno.SelectedValue), User.Identity.Name);

                if (validacao.Valido)
                {
                    rnMatriculas.Convoca(tseCurso.DBValue.ToString(), ddlTurno.SelectedValue, Convert.ToInt32(ddlAno.SelectedValue), User.Identity.Name, out aviso);

                    lblVagasDisponiveis.Text = rnMatriculas.RetornaSaldoPor(tseCurso.DBValue.ToString(), ddlTurno.SelectedValue, Convert.ToInt32(ddlAno.SelectedValue)).ToString();

                    lblTotalFila.Text = rnMatriculas.RetornaQuantidadeFilaPor(tseCurso.DBValue.ToString(), ddlTurno.SelectedValue, Convert.ToInt32(ddlAno.SelectedValue)).ToString();


                    if (!aviso.IsNullOrEmptyOrWhiteSpace())
                    {
                        lblAviso.Text = aviso.Replace(Environment.NewLine, "<br />");
                        pnlAviso.Visible = true;
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
