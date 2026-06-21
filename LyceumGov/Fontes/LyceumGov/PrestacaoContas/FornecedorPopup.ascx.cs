using System;
using System.Data;
using System.Web.UI;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using System.Web;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    public partial class FornecedorPopup : System.Web.UI.UserControl
    {
        private readonly RN.PrestacaoContas.DocumentosFornecedor rnDocumentosFornecedor = new RN.PrestacaoContas.DocumentosFornecedor();

        public int FornecedorId { get; set; }

        protected void Page_Init()
        {
            TPage.TituloGrid(this.grdDocumentos, string.Empty);
        }

        protected void Page_Load()
        {
            try
            {
                if (FornecedorId > 0 && (ViewState["FornecedorId"] as int?) != FornecedorId)
                {
                    ViewState["FornecedorId"] = FornecedorId;
                    var lista = rnDocumentosFornecedor.ListaDocumentosPrestesAExpirar(FornecedorId, DateTime.Now.AddDays(15));
                    grdDocumentos.DataSource = lista;
                    grdDocumentos.DataBind();
                    
                    if (lista.Rows.Count > 0)
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