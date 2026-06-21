using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN;
using System.Data;

namespace Techne.Lyceum.Net.Academico
{
    public partial class AcompanhamentoClassroom : System.Web.UI.Page
    {
        public List<DateTime> _acessos = new List<DateTime>();

        protected void Page_Load(object sender, EventArgs e)
        {
            RN.RecursosHumanos.GoogleEducation rnGoogleEducation = new Techne.Lyceum.RN.RecursosHumanos.GoogleEducation();
            DataTable dtAcessos = new DataTable();

            try
            {
                dtAcessos = rnGoogleEducation.ObtemAcessosPor(User.Identity.Name);
                foreach (DataRow dr in dtAcessos.Rows)
                    _acessos.Add(Convert.ToDateTime(dr["LOGINTIME"]));
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
