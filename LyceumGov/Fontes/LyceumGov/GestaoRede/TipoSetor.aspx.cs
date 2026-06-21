using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.GestaoRede
{
    [
        NavUrl("~/GestaoRede/TipoSetor.aspx"),
        ControlText("Tipo Setor"),
        Title("Tipo Setor")
    ]
    public partial class TipoSetor : TPage
    {
        public object Lista()
        {
            RN.GestaoRede.TipoSetor rnTipoSetor = new Techne.Lyceum.RN.GestaoRede.TipoSetor();

            return rnTipoSetor.ListaTipoSetor();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object TIPOSETORID) { }
        public void Delete(object MEDIDAID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTipoSetor, "Tipo Setor");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTipoSetor);
        }

        protected void grdTipoSetor_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTipoSetor.Settings.ShowFilterRow = false;
        }

        protected void grdTipoSetor_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdTipoSetor.Settings.ShowFilterRow = false;
        }

        protected void grdTipoSetor_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.GestaoRede.Entidades.TipoSetor tipoSetor = new Techne.Lyceum.RN.GestaoRede.Entidades.TipoSetor();
            RN.GestaoRede.TipoSetor rnTipoSetor = new Techne.Lyceum.RN.GestaoRede.TipoSetor();

            tipoSetor.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipoSetor.Ativo = e.NewValues["ATIVO"] == null ? false : true;
            tipoSetor.UsuarioId = User.Identity.Name;

            validacao = rnTipoSetor.Valida(tipoSetor, true);

            if (validacao.Valido)
            {
                rnTipoSetor.Insere(tipoSetor);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoSetor.DataBind();

        }

        protected void grdTipoSetor_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.GestaoRede.Entidades.TipoSetor tipoSetor = new Techne.Lyceum.RN.GestaoRede.Entidades.TipoSetor();
            RN.GestaoRede.TipoSetor rnTipoSetor = new Techne.Lyceum.RN.GestaoRede.TipoSetor();

            tipoSetor.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipoSetor.TipoSetorId = Convert.ToInt32(e.Keys["TIPOSETORID"]);
            tipoSetor.Ativo = e.NewValues["ATIVO"] == null ? false : true;
            tipoSetor.UsuarioId = User.Identity.Name;

            validacao = rnTipoSetor.Valida(tipoSetor, true);

            if (validacao.Valido)
            {
                rnTipoSetor.Atualiza(tipoSetor);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoSetor.DataBind();
        }

        protected void grdTipoSetor_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.GestaoRede.TipoSetor rnTipoSetor = new Techne.Lyceum.RN.GestaoRede.TipoSetor();
            int tipoSetorId = 0;

            tipoSetorId = Convert.ToInt32(e.Keys["TIPOSETORID"]);

            validacao = rnTipoSetor.ValidaRemocao(tipoSetorId);

            if (validacao.Valido)
            {
                rnTipoSetor.Remove(tipoSetorId);
                grdTipoSetor.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}