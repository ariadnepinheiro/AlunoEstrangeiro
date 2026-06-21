using System;
using System.Linq;
using System.Web;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Web;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/CancelaRenovacaoMatricula.aspx"), ControlText("CancelaRenovacaoMatricula"), Title("Cancela Renovação Matrícula")]

    public partial class CancelaRenovacaoMatricula : TPage
    {
        public object Lista(object aluno,object ano, object periodo)
        {
            RN.RenovacaoMatricula.Renovacao rnRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Renovacao();

            if (aluno != null && ano != null && periodo != null)
            {
                return rnRenovacao.ListaRenovacaoPor(aluno.ToString(), Convert.ToInt32(ano), Convert.ToInt32(periodo));
            }

            return null;
        }

        protected void Page_Init(object sender, EventArgs e)
        {

            TituloGrid(grdRenovacaoMatricula, "Renovação de Matrícula");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    CarregaAno();
                    ddlPeriodo.ClearSelection();
                    tseAluno.ResetValue();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdRenovacaoMatricula);
        }

        private void CarregaAno()
        {
            ListItem listItem;
            RN.PeriodoLetivo rnPeriodoLetivo = new Techne.Lyceum.RN.PeriodoLetivo();

            try
            {
                ddlAno.Items.Clear();
                ddlAno.DataSource = rnPeriodoLetivo.ListaAnoFuturo();
                ddlAno.DataBind();
                listItem = new ListItem("Selecione", string.Empty);
                ddlAno.Items.Insert(0, listItem);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs args)
        {
            try
            {
                pnRenovacao.Visible = true;
                ddlPeriodo.Items.Clear();

                if (!string.IsNullOrEmpty(ddlAno.SelectedValue))
                {
                    CarregaPeriodo(Convert.ToInt32(ddlAno.SelectedValue));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaPeriodo(int ano)
        {
            ListItem listItem;
            RN.PeriodoLetivo rnPeriodoLetivo = new Techne.Lyceum.RN.PeriodoLetivo();

            try
            {
                ddlPeriodo.Items.Clear();
                ddlPeriodo.DataSource = rnPeriodoLetivo.ListaPeriodoFuturo(ano);
                ddlPeriodo.DataBind();
                listItem = new ListItem("Selecione", string.Empty);
                ddlPeriodo.Items.Insert(0, listItem);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            try
            {
                pnRenovacao.Visible = false;
                if (Page.IsCallback)
                {
                    return;
                }

                if (!tseAluno.DBValue.IsNull)
                {
                    if (tseAluno.IsValidDBValue)
                    {
                        lblMensagem.Text = string.Empty;

                        if (!ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                        {
                            pnRenovacao.Visible = true;                        
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

        protected void grdRenovacaoMatricula_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdRenovacaoMatricula);
        }

        protected bool VerificarCheck(object valor)
        {
            if (valor is DBNull)
            {
                return false;
            }

            if (valor is string)
            {
                return (string)valor == "1";
            }

            if (valor is bool)
            {
                return (bool)valor;
            }
            return false;
        }

        protected void grdRenovacaoMatricula_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;

            var status = grdRenovacaoMatricula.GetRowValues(e.VisibleIndex, "COD_SITUACAO_RENOVACAOID");

            e.Button.Visibility = GridViewCustomButtonVisibility.AllDataRows;

            if (Convert.ToInt32(status) == 2)
            {
                e.Button.Visibility = GridViewCustomButtonVisibility.Invisible;
            }
        }

        protected void grdRenovacaoMatricula_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            RN.RenovacaoMatricula.Renovacao rnRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Renovacao();
            ValidacaoDados validacao = new ValidacaoDados();

            if (e.ButtonID == "btnCancelar")
            {
                try
                {
                    var renovacaoId = Convert.ToInt32(grdRenovacaoMatricula.GetRowValues(e.VisibleIndex, "RENOVACAOID"));

                    validacao = rnRenovacao.ValidaCancelaRenovacao(tseAluno.DBValue.ToString(), renovacaoId, Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), User.Identity.Name);

                    if (validacao.Valido)
                    {
                        rnRenovacao.CancelaRenovacao(renovacaoId, User.Identity.Name);
                        grdRenovacaoMatricula.DataBind();
                        lblMensagem.Text = "Renovação cancelada com sucesso.";
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
}
