using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;

namespace Techne.Lyceum.Net.ProcessoSeletivo
{
    [
     NavUrl("~/ProcessoSeletivo/ReimpressaoProposta.aspx"),
      ControlText("ReimpressaoProposta"),
      Title("Reimpressão da Proposta de Contrato Temporário"),
    ]


    public partial class ReimpressaoProposta : TPage
    {
        #region Código Padrão Techne

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

        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                RN.CandidatoDocente rnCandidatoDocente = new Techne.Lyceum.RN.CandidatoDocente();
                if (tseConcurso.IsValidDBValue && !tseConcurso.DBValue.IsNull && tseInscricao.IsValidDBValue && !tseInscricao.DBValue.IsNull)
                {
                    if (rnCandidatoDocente.EhMatriculaDocente(tseConcurso.DBValue.ToString(), tseInscricao.DBValue.ToString()))
                    {
                        IDictionary<string, string> pares = new Dictionary<string, string>();
                        pares.Add("candidato", tseInscricao.DBValue.ToString());
                        pares.Add("concurso", tseConcurso.DBValue.ToString());
                        btnImprimir.Attributes.Add("onclick", @"javascript:window.open('../Relatorio/Relatorios.aspx?report=rspropcontrtempinterno&grp=processoseletivo&" + TPage.CodificaQueryString(pares) + "','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes'); return false;");
                    }
                    else
                    {
                        lblMensagem.Text = "Proposta não encontrada.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseConcurso_Changed(object sender, EventArgs e)
        {
            try
            {
                if (!tseConcurso.IsValidDBValue || tseConcurso.DBValue.IsNull)
                {
                    lblMensagem.Text = "Favor consultar processo seletivo válido.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseInscricao_Changed(object sender, EventArgs e)
        {
            try
            {
                if (!tseInscricao.IsValidDBValue || tseInscricao.DBValue.IsNull)
                {
                    lblMensagem.Text = "Favor consultar um número de inscrição válido.";
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

    }
}









