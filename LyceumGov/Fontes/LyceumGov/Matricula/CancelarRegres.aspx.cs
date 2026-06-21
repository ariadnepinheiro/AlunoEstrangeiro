using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Matriculas;
using Techne.Web;

namespace Techne.Lyceum.Net.Matricula
{
    [NavUrl("~/Matricula/CancelarRegres.aspx")]
    [ControlText("Cancelar Matricula Especial - Regres")]
    [Title("Cancelar Matricula Especial - Regres")]

    public partial class CancelarRegres : TPage
    {
        public object Lista(object ano, object aluno)
        {
            RN.Matriculas.MatriculaEspecialDisciplina rnMatriculas = new Techne.Lyceum.RN.Matriculas.MatriculaEspecialDisciplina();

            if (!string.IsNullOrEmpty(aluno.ToString()) && !string.IsNullOrEmpty(ano.ToString()))
            {
                return rnMatriculas.ObtemListaPor(Convert.ToInt32(ano), aluno.ToString());
            }
            return null;
        }

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

        public void Delete(object MATRICULAESPECIALDISCIPLINAID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdCancelamento, "Cancelamento Aluno/Disciplina - Regres");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdCancelamento);
        }

        protected void grdCancelamento_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Matriculas.MatriculaEspecialDisciplina rnMatriculas = new Techne.Lyceum.RN.Matriculas.MatriculaEspecialDisciplina();
            int id = 0;

            id = Convert.ToInt32(e.Keys["MATRICULAESPECIALDISCIPLINAID"]);

            validacao = rnMatriculas.ValidaRemocao(id);

            if (validacao.Valido)
            {
                rnMatriculas.Remove(id);
                grdCancelamento.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }
              lblMensagem.Text = string.Empty;
                if (!tseAluno.DBValue.IsNull)
                {
                    if (tseAluno.IsValidDBValue)
                    {
                        if (ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                        {
                            lblMensagem.Text = "Para pesquisar é necessário selecionar o Ano.";
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";                  
                }           

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
