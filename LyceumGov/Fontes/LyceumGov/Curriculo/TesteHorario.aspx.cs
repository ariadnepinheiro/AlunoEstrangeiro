using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Web.SessionState;
using System.Web.UI.HtmlControls;
using Techne.Data;
using Techne.Web;
using Techne.Lyceum.CR;
using System.Web.UI.MobileControls;
using Techne.Lyceum.RN;
using System.IO;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxGridView;


namespace Techne.Lyceum.Net.Curriculo
{
    [NavUrl("~/Curriculo/TesteHorario.aspx"),
     ControlText("TesteHorario"),
     Title("TesteHorario"),]


    public partial class TesteHorario : TPage
    {


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Botao_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTurma.Text))
            {
                string dadosTurma = txtTurma.Text;
                string queryString = MontarQueryString(dadosTurma);

                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);
                Response.Redirect("HorariosDocentePorTurma.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
            }
        }

        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

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

        private void RedirecionarPagina()
        {
            HttpContext.Current.Items.Add("chave", "1000");

            Server.Transfer("HorariosDocentePorTurma.aspx");
        }

        private string MontarQueryString(string dadosTurma)
        {
            string queryString = string.Empty;

            queryString = dadosTurma;

            return queryString;
        }
    }
}

