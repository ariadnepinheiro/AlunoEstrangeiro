using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Techne.Web;
using DevExpress.Web.ASPxGridView;
using Techne.Lyceum.RN.DTOs;

namespace Techne.Lyceum.Net.Basico
{
    [
    NavUrl("~/Basico/HabilitaNecessidadeEspecialProcesso.aspx"),
     ControlText("Habilitar Necessidade Especial"),
     Title("Habilitar Necessidade Especial"),
   ]
    public partial class HabilitaNecessidadeEspecialProcesso : TPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdNecessidade, "Necessidade Especial");

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;

            if (!IsPostBack)
            {

            }
        }

        protected void tseProcesso_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }
                if (!this.tseProcesso.DBValue.IsNull)
                {
                    if (this.tseProcesso.IsValidDBValue)
                    {
                        pnlGrid.Visible = true;
                    }
                    else
                    {
                        pnlGrid.Visible = false;
                    }
                }
                else
                {
                    pnlGrid.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
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

        protected void grdNecessidade_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdNecessidade);
        }

        protected void grdNecessidade_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdNecessidade.Settings.ShowFilterRow = false;
        }

        protected void odsNecessidade_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            RN.NecessidadeEspecial.NecessidadeEspecial rnNecessidadeEspecial = new Techne.Lyceum.RN.NecessidadeEspecial.NecessidadeEspecial();

            var necessidade = new DadosNecessidadeEspecialFiltroProcesso
            {
                NecessidadeEspecialId = Convert.ToInt32(e.InputParameters["NECESSIDADEESPECIALID"]),
                FiltroProcessoDescricao = Convert.ToString(tseProcesso["DESCRICAO"]),
                FiltroProcessoId = Convert.ToInt32(tseProcesso.DBValue.ToString()),
                Habilitado =  Convert.ToBoolean(e.InputParameters["HABILITADO"]),
                NecessidadeEspecialDescricao = Convert.ToString(e.InputParameters["DESCRICAO"]),

            };

            var validacao = rnNecessidadeEspecial.ValidaNecessidadeEspecialFiltroProcesso(necessidade);

            if (validacao.Valido)
            {
                rnNecessidadeEspecial.SalvaNecessidadeEspecialFiltroProcesso(necessidade);

                e.Cancel = true;
                this.grdNecessidade.CancelEdit();
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        public void Update( object ITEM, object DESCRICAO, object HABILITADO,object NECESSIDADEESPECIALID)
        {
        }
    }
}
