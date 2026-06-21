using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN;

namespace Techne.Lyceum.Net.Academico
{
    public partial class GoogleEducation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RN.RecursosHumanos.GoogleEducation rnGoogleEducation = new Techne.Lyceum.RN.RecursosHumanos.GoogleEducation();
            RN.RecursosHumanos.Entidades.GoogleEducation googleEducation = new Techne.Lyceum.RN.RecursosHumanos.Entidades.GoogleEducation();           

            try
            {
                string matricula = User.Identity.Name;
                lblMensagem.Text = string.Empty;
                lblEmail.Text = string.Empty;
                googleEducation = rnGoogleEducation.ObtemPor(matricula);

                if (googleEducation.GoogleEducationId > 0)
                {
                    lblEmail.Text = googleEducation.Email;
                }
                else
                {                    
                    lblMensagem.Text = "Login não encontrado, caso possua uma matricula ativa entre em contato com a direção de sua escola";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
