using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Patrimonio
{
    [
 NavUrl("~/Patrimonio/Moeda.aspx"),
 ControlText("Moeda"),
 Title("Moeda")
]
    public partial class Moeda : TPage
    {
        public object Lista()
        {
            RN.Patrimonio.Moeda rnMoeda = new Techne.Lyceum.RN.Patrimonio.Moeda();

            return rnMoeda.Lista();
        }

        public void Insert(object DESCRICAO, object DATAINICIO, object DATAFIM, object SIGLA, object FATOR) { }
        public void Update(object DESCRICAO, object DATAINICIO, object DATAFIM, object SIGLA, object FATOR ,object MOEDAID) { }
        public void Delete(object MOEDAID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMoeda, "Moeda");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdMoeda);
        }

        protected void grdMoeda_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdMoeda);
        }

        protected void grdMoeda_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMoeda.Settings.ShowFilterRow = false;
        }

        protected void grdMoeda_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdMoeda.Settings.ShowFilterRow = false;
        }

        protected void grdMoeda_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Patrimonio.Entidades.Moeda moeda = new Techne.Lyceum.RN.Patrimonio.Entidades.Moeda();
            RN.Patrimonio.Moeda rnMoeda = new Techne.Lyceum.RN.Patrimonio.Moeda();

            moeda.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            moeda.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"].ToString()) : DateTime.MinValue;
            moeda.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"].ToString()) : DateTime.MinValue;
            moeda.Sigla = e.NewValues["SIGLA"] != null ? e.NewValues["SIGLA"].ToString().Trim().ToUpper() : null;
            moeda.Fator = e.NewValues["FATOR"] != null ? Convert.ToInt32(e.NewValues["FATOR"]) : -1;
            moeda.UsuarioId = User.Identity.Name;

            validacao = rnMoeda.Valida(moeda, true);

            if (validacao.Valido)
            {
                rnMoeda.Insere(moeda);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMoeda.DataBind();

        }

        protected void grdMoeda_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Patrimonio.Entidades.Moeda moeda = new Techne.Lyceum.RN.Patrimonio.Entidades.Moeda();
            RN.Patrimonio.Moeda rnMoeda = new Techne.Lyceum.RN.Patrimonio.Moeda();

            moeda.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            moeda.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"].ToString()) : DateTime.MinValue;
            moeda.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"].ToString()) : DateTime.MinValue;
            moeda.Sigla = e.NewValues["SIGLA"] != null ? e.NewValues["SIGLA"].ToString().Trim().ToUpper() : null;
            moeda.Fator = e.NewValues["FATOR"] != null ? Convert.ToInt32(e.NewValues["FATOR"]) : -1;
            moeda.UsuarioId = User.Identity.Name;
            moeda.MoedaId = Convert.ToInt32(e.Keys["MOEDAID"]);


            validacao = rnMoeda.Valida(moeda, true);

            if (validacao.Valido)
            {
                rnMoeda.Atualiza(moeda);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMoeda.DataBind();
        }

        protected void grdMoeda_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados(); 
            RN.Patrimonio.Moeda rnMoeda = new Techne.Lyceum.RN.Patrimonio.Moeda();
            int moedaId = 0;

            moedaId = Convert.ToInt32(e.Keys["MOEDAID"]);

            validacao = rnMoeda.ValidaRemocao(moedaId);

            if (validacao.Valido)
            {
                rnMoeda.Remove(moedaId);
                grdMoeda.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
