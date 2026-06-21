using System;
using System.Data;
using System.Text;
using System.Web.UI;
using Techne.Lyceum.RN.Agenda;

namespace Techne.Lyceum.Net.ProcessoSeletivoAluno
{
    public partial class Edital : TPage
    {
        Sessao.CandidatoProcessoSeletivoSessao sessaoCandidato = new Sessao.CandidatoProcessoSeletivoSessao();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                ExibeEdital();
        }

        public void ExibeEdital()
        {
            string div = string.Empty;

            DataTable dtEdital = ProcessoSeletivo.ListaEdital();

            if (dtEdital.Rows.Count > 0)
            {
                sessaoCandidato.AgendaID = Convert.ToInt32(dtEdital.Rows[0]["AGENDAID"]);
                sessaoCandidato.ProcessoSeletivoID = Convert.ToInt32(dtEdital.Rows[0]["PROCESSOSELETIVOID"]);

                div += "<table style='width: 100%;'>";
                // Converter o byte[] para String
                byte[] dBytes = dtEdital.Rows[0]["EDITAL"] as byte[]; // seu array de bytes.
                string str; // String que irá receber a conversão
                Encoding enc = Encoding.GetEncoding("iso-8859-1");
                str = enc.GetString(dBytes);
                div += "<tr> <td>";
                div += str;
                div += "</td> </tr>";
                div += "</table>";
            }
            else
            {
                trSemProcessoSeletivoPeriodoInscricao.Visible = true;
                trEdital.Visible = false;
            }

            divEdital.InnerHtml = div;
        }

        protected void btnEdital_Click(object sender, EventArgs e)
        {
            if (SeChecado.Value == "True")
            {
                sessaoCandidato.CandidatoAcordoEdital = true;

                Response.Redirect("Identificacao.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('Favor aceitar as condições descritas no edital.')", true);
            }
        }
    }
}
