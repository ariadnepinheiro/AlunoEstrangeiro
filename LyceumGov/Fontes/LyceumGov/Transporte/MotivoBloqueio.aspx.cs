using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Transporte
{
    [NavUrl("~/Transporte/MotivoBloqueio.aspx")]
    [ControlText("Motivo Bloqueio")]
    [Title("Motivo Bloqueio")]

    public partial class MotivoBloqueio : TPage
    {
        public object Lista()
        {
            RN.Transporte.MotivoBloqueio rnMotivoBloqueio = new Techne.Lyceum.RN.Transporte.MotivoBloqueio();

            return rnMotivoBloqueio.Lista();

        }

        public void Insert(object DESCRICAO,object TIPO, object ATIVO) { }
        public void Update(object DESCRICAO, object TIPO, object ATIVO, object MOTIVOBLOQUEIOID) { }
        public void Delete(object MOTIVOBLOQUEIOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMotivoBloqueio, "Motivo do Bloqueio");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdMotivoBloqueio);
        }

        protected void grdMotivoBloqueio_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMotivoBloqueio.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoBloqueio_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdMotivoBloqueio.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoBloqueio_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Entidades.MotivoBloqueio motivoBloqueio = new Techne.Lyceum.RN.Transporte.Entidades.MotivoBloqueio();
            RN.Transporte.MotivoBloqueio rnMotivoBloqueio = new Techne.Lyceum.RN.Transporte.MotivoBloqueio();

            motivoBloqueio.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivoBloqueio.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivoBloqueio.Tipo = e.NewValues["TIPO"] != null ? Convert.ToInt32(e.NewValues["TIPO"]) : -1;
            motivoBloqueio.UsuarioId = User.Identity.Name;

            validacao = rnMotivoBloqueio.Valida(motivoBloqueio, true);

            if (validacao.Valido)
            {
                rnMotivoBloqueio.Insere(motivoBloqueio);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoBloqueio.DataBind();

        }

        protected void grdMotivoBloqueio_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Entidades.MotivoBloqueio motivoBloqueio = new Techne.Lyceum.RN.Transporte.Entidades.MotivoBloqueio();
            RN.Transporte.MotivoBloqueio rnMotivoBloqueio = new Techne.Lyceum.RN.Transporte.MotivoBloqueio();

            motivoBloqueio.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivoBloqueio.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivoBloqueio.Tipo = e.NewValues["TIPO"] != null ? Convert.ToInt32(e.NewValues["TIPO"]) : -1;
            motivoBloqueio.UsuarioId = User.Identity.Name;
            motivoBloqueio.MotivoBloqueioId = Convert.ToInt32(e.Keys["MOTIVOBLOQUEIOID"]);


            validacao = rnMotivoBloqueio.Valida(motivoBloqueio, true);

            if (validacao.Valido)
            {
                rnMotivoBloqueio.Atualiza(motivoBloqueio);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoBloqueio.DataBind();
        }

        protected void grdMotivoBloqueio_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.MotivoBloqueio rnMotivoBloqueio = new Techne.Lyceum.RN.Transporte.MotivoBloqueio();
            int motivoBloqueioId = 0;

            motivoBloqueioId = Convert.ToInt32(e.Keys["MOTIVOBLOQUEIOID"]);

            validacao = rnMotivoBloqueio.ValidaRemocao(motivoBloqueioId);

            if (validacao.Valido)
            {
                rnMotivoBloqueio.Remove(motivoBloqueioId);
                grdMotivoBloqueio.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
