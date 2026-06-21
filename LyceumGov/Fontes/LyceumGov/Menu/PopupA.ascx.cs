using System;
using System.Data;
using System.Web.UI;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using System.Web;
using System.Collections.Generic;
using System.Reflection;
using Techne.Web;
using System.Configuration;
using System.Collections.Specialized;
using DevExpress.Web.ASPxGridView;
using Techne.Controls;
using Techne.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Collections;

namespace Techne.Lyceum.Net.Menu
{
    public partial class PopupA : System.Web.UI.UserControl
    {

        protected void Page_Load()
        {
            try
            {
                var usuario = HttpContext.Current.User.Identity.Name;

                if (!IsPostBack)
                {
                    if (DateTime.Now.Date >= Convert.ToDateTime("2025-10-20") && DateTime.Now.Date <= Convert.ToDateTime("2025-11-07"))
                    {
                        int dia = DateTime.Now.Day;

                        switch (dia)
                        {
                            case 20:
                                imgSaeb.ImageUrl = "~/Images/2010_FALTAM8.png";
                                break;
                            case 21:
                                imgSaeb.ImageUrl = "~/Images/2110_FALTAM7.png";
                                break;
                            case 22:
                                imgSaeb.ImageUrl = "~/Images/2210_FALTAM6.png";
                                break;
                            case 23:
                                imgSaeb.ImageUrl = "~/Images/2310_FALTAM5.png";
                                break;
                            case 24:
                                imgSaeb.ImageUrl = "~/Images/2410_FALTAM4.png";
                                break;
                            case 25:
                                imgSaeb.ImageUrl = "~/Images/2510_FALTAM3.png";
                                break;
                            case 26:
                                imgSaeb.ImageUrl = "~/Images/2610_FALTAM2.png";
                                break;
                            case 27:
                                imgSaeb.ImageUrl = "~/Images/2710_FALTA1.png";
                                break;                            
                            default:
                                imgSaeb.ImageUrl = "~/Images/2810_HOJE.png";
                                break;
                        } 
                    }                   

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "window.setTimeout(function() { pcPopup.Show(); }, 1000);", true);

                }
            }

            catch (Exception ex)
            {
                throw;
            }
        }
    }
}