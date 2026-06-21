using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Interconectividade
{
    [
       NavUrl("~/Interconectividade/Tecnologia.aspx"),
       ControlText("Tecnologia"),
       Title("Tecnologia")
   ]
    public partial class Tecnologia : TPage
    {
        public object Lista()
        {
            RN.FiscalizacaoLink.Tecnologia rnTecnologia = new Techne.Lyceum.RN.FiscalizacaoLink.Tecnologia();

            return rnTecnologia.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object TecnologiaID) { }
        public void Delete(object TecnologiaID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTecnologia, "Tecnologia");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTecnologia);
        }

        protected void grdTecnologia_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTecnologia.Settings.ShowFilterRow = false;
        }

        protected void grdTecnologia_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdTecnologia.Settings.ShowFilterRow = false;
        }

        protected void grdTecnologia_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.Entidades.Tecnologia tecnologia = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.Tecnologia();
            RN.FiscalizacaoLink.Tecnologia rnTecnologia = new RN.FiscalizacaoLink.Tecnologia();

            tecnologia.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tecnologia.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            tecnologia.UsuarioId = User.Identity.Name;

            validacao = rnTecnologia.Valida(tecnologia, true);

            if (validacao.Valido)
            {
                rnTecnologia.Insere(tecnologia);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTecnologia.DataBind();

        }

        protected void grdTecnologia_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.Entidades.Tecnologia tecnologia = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.Tecnologia();
            RN.FiscalizacaoLink.Tecnologia rnTecnologia = new RN.FiscalizacaoLink.Tecnologia();

            tecnologia.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tecnologia.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            tecnologia.TecnologiaId = Convert.ToInt32(e.Keys["TECNOLOGIAID"]);
            tecnologia.UsuarioId = User.Identity.Name;

            validacao = rnTecnologia.Valida(tecnologia, true);

            if (validacao.Valido)
            {
                rnTecnologia.Atualiza(tecnologia);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTecnologia.DataBind();
        }

        protected void grdTecnologia_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.Tecnologia rnTecnologia = new RN.FiscalizacaoLink.Tecnologia();
            int tecnologiaId = 0;

            tecnologiaId = Convert.ToInt32(e.Keys["TECNOLOGIAID"]);

            validacao = rnTecnologia.ValidaRemocao(tecnologiaId);

            if (validacao.Valido)
            {
                rnTecnologia.Remove(tecnologiaId);
                grdTecnologia.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
