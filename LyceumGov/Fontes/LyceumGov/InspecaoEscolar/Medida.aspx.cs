using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.InspecaoEscolar
{
    [
          NavUrl("~/InspecaoEscolar/Medida.aspx"),
          ControlText("Medida"),
          Title("Medida")
      ]
    public partial class Medida : TPage
    {
        public object Lista()
        {
            RN.InspecaoEscolar.Medida rnMedida = new Techne.Lyceum.RN.InspecaoEscolar.Medida();

            return rnMedida.ListaMedida();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object MEDIDAID) { }
        public void Delete(object MEDIDAID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMedida, "Medida");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdMedida);
        }

        protected void grdMedida_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMedida.Settings.ShowFilterRow = false;
        }

        protected void grdMedida_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdMedida.Settings.ShowFilterRow = false;
        }

        protected void grdMedida_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.InspecaoEscolar.Entidades.Medida medida = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.Medida();
            RN.InspecaoEscolar.Medida rnMedida = new RN.InspecaoEscolar.Medida();

            medida.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            medida.Ativo = e.NewValues["ATIVO"] == null ? false : true;
            medida.UsuarioId = User.Identity.Name;

            validacao = rnMedida.Valida(medida, true);

            if (validacao.Valido)
            {
                rnMedida.Insere(medida);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMedida.DataBind();

        }

        protected void grdMedida_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.InspecaoEscolar.Entidades.Medida medida = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.Medida();
            RN.InspecaoEscolar.Medida rnMedida = new RN.InspecaoEscolar.Medida();

            medida.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            medida.Ativo = e.NewValues["ATIVO"] == null ? false : true;
            medida.MedidaId = Convert.ToInt32(e.Keys["MEDIDAID"]);
            medida.UsuarioId = User.Identity.Name;

            validacao = rnMedida.Valida(medida, true);

            if (validacao.Valido)
            {
                rnMedida.Atualiza(medida);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMedida.DataBind();
        }

        protected void grdMedida_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.InspecaoEscolar.Medida rnMedida = new RN.InspecaoEscolar.Medida();
            int medidaId = 0;

            medidaId = Convert.ToInt32(e.Keys["MEDIDAID"]);

            validacao = rnMedida.ValidaRemocao(medidaId);

            if (validacao.Valido)
            {
                rnMedida.Remove(medidaId);
                grdMedida.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }           
        }
    }
}
