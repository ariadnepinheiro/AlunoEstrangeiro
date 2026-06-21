using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using System.Data;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Consulta
{
    [NavUrl("~/Consulta/HistorioEscolarAluno.aspx"),
    ControlText("Histórico Escolar do Aluno"),
    Title("Histórico Escolar do Aluno")]
    public partial class HistorioEscolarAluno : TPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdHistorico, "Histórico Escolar do Aluno");
            TituloGrid(grdDisciplina, "Disciplinas");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
        }

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            try
            {
                pnDadosAluno.Visible = false;
                LimpaTela();

                if (!tseAluno.DBValue.IsNull)
                {
                    if (tseAluno.IsValidDBValue)
                    {
                        lblMensagem.Text = string.Empty;
                        pnDadosAluno.Visible = true;
                        this.CarregaDadosAluno(Convert.ToString(tseAluno.DBValue));
                    }
                    else
                    {
                        lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Favor preencher o campo Aluno.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaDadosAluno(string aluno)
        {
            RN.Aluno rnAluno = new RN.Aluno();
            RN.DTOs.DadosAlunoPessoa dadosAluno = new Techne.Lyceum.RN.DTOs.DadosAlunoPessoa();

            pnHistorico.Visible = false;

            try
            {
                //Busca dados do aluno
                dadosAluno = rnAluno.ObtemDadosAlunoPessoaPor(aluno);

                //Preenche Dados Aluno
                if (!string.IsNullOrEmpty(dadosAluno.Aluno))
                {
                    //Dados do aluno                   
                    lblMatricula.Text = dadosAluno.Aluno;
                    lblNome.Text = dadosAluno.Nome;
                    lblMae.Text = dadosAluno.NomeMae;
                    lblStatus.Text = dadosAluno.SitAluno;

                    if (dadosAluno.DataNascimento != null && dadosAluno.DataNascimento != DateTime.MinValue)
                    {
                        lblDataNascimento.Text = Convert.ToDateTime(dadosAluno.DataNascimento).ToString("dd/MM/yyyy");
                    }

                    pnHistorico.Visible = true;
                }
                else
                {
                    this.LimpaTela();
                    this.lblMensagem.Text = "Aluno não encontrado.";
                    return;
                }
            }
            catch (Exception ex)
            {
                this.lblMensagem.Text = ex.Message;
            }
        }

        public object ListaTurmasHistorico(object aluno)
        {
            RN.SituacaoFinalAluno rnSituacaoFinalAluno = new Techne.Lyceum.RN.SituacaoFinalAluno();

            if (!string.IsNullOrEmpty(Convert.ToString(aluno)))
            {
                return rnSituacaoFinalAluno.ObtemListaPor(Convert.ToString(aluno));
            }
            return null;
        }

        protected void grdHistorico_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            RN.HistMatricula rnHistMatricula = new Techne.Lyceum.RN.HistMatricula();

            try
            {
                if (e.ButtonID == "btnDisciplinas")
                {
                    pucDisciplinas.ShowOnPageLoad = true;

                    // Buscar campos;  
                    string aluno = Convert.ToString(grdHistorico.GetRowValues(e.VisibleIndex, "ALUNO"));
                    int ano = Convert.ToInt32(grdHistorico.GetRowValues(e.VisibleIndex, "ANO"));
                    int periodo = Convert.ToInt32(grdHistorico.GetRowValues(e.VisibleIndex, "SEMESTRE"));
                    string turma = Convert.ToString(grdHistorico.GetRowValues(e.VisibleIndex, "TURMA"));

                    //Atualiza grid
                    this.grdDisciplina.DataSource = rnHistMatricula.ListaDisciplinasPor(aluno, ano, periodo, turma);
                    this.grdDisciplina.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                pucDisciplinas.ShowOnPageLoad = false;
            }
        }

        private void LimpaTela()
        {
            lblMatricula.Text = string.Empty;
            lblNome.Text = string.Empty;
            lblMae.Text = string.Empty;
            lblDataNascimento.Text = string.Empty;
            lblStatus.Text = string.Empty;
        }
    }
}
