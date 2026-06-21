namespace Techne.Lyceum.Net.Seguranca
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Web.UI;

    public partial class GeraChaveSeguranca : Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            //Gera Captcha
            RN.Captcha rnCaptcha = new Techne.Lyceum.RN.Captcha();            
            rnCaptcha.GeraCaptcha();
        }
    }
}