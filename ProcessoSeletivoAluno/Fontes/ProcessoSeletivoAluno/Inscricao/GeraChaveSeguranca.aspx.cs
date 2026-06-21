using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Techne.Lyceum.Net.Inscricao
{
    public partial class GeraChaveSeguranca : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            //Gera Captcha
            RN.Captcha rnCaptcha = new Techne.Lyceum.RN.Captcha();
            rnCaptcha.GeraCaptcha();
        }
    }
}
