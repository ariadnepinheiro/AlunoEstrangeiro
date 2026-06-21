using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Matriculas;
using Techne.Web;
using DevExpress.Web.ASPxGridView;


namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/JustificativaFalta.aspx")]
    [ControlText("JustificativaFalta")]
    [Title("Motivo Falta")]

    public partial class JustificativaFalta : TPage
    {
        public object Lista()
        {
            RN.RecursosHumanos.JustificativaFalta rnJustificativaFalta = new Techne.Lyceum.RN.RecursosHumanos.JustificativaFalta();

            return rnJustificativaFalta.Lista();

        }

        public void Insert(object DESCRICAO, object LEIAMPARO, object CASOESPECIFICO, object ATIVO) { }
        public void Update(object DESCRICAO, object LEIAMPARO, object CASOESPECIFICO, object ATIVO, object JUSTIFICATIVAFALTAID) { }
        public void Delete(object JUSTIFICATIVAFALTAID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdJustificativaFalta, "Motivo Falta");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdJustificativaFalta);
        }

        protected void grdJustificativaFalta_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdJustificativaFalta);
        }

        protected void grdJustificativaFalta_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdJustificativaFalta.Settings.ShowFilterRow = false;
        }

        protected void grdJustificativaFalta_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdJustificativaFalta.Settings.ShowFilterRow = false;
        }

        protected void grdJustificativaFalta_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.Entidades.JustificativaFalta motivo = new Techne.Lyceum.RN.RecursosHumanos.Entidades.JustificativaFalta();
            RN.RecursosHumanos.JustificativaFalta rnJustificativaFalta = new Techne.Lyceum.RN.RecursosHumanos.JustificativaFalta();

            motivo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivo.LeiAmparo = e.NewValues["LEIAMPARO"] != null ? e.NewValues["LEIAMPARO"].ToString().Trim().ToUpper() : null;
            motivo.CasoEspecifico = (e.NewValues["CASOESPECIFICO"] == null || Convert.ToBoolean(e.NewValues["CASOESPECIFICO"]) == false) ? false : true;
            motivo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivo.UsuarioId = User.Identity.Name;

            validacao = rnJustificativaFalta.Valida(motivo, true);

            if (validacao.Valido)
            {
                rnJustificativaFalta.Insere(motivo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdJustificativaFalta.DataBind();

        }

        protected void grdJustificativaFalta_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.Entidades.JustificativaFalta motivo = new Techne.Lyceum.RN.RecursosHumanos.Entidades.JustificativaFalta();
            RN.RecursosHumanos.JustificativaFalta rnJustificativaFalta = new Techne.Lyceum.RN.RecursosHumanos.JustificativaFalta();

            motivo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivo.LeiAmparo = e.NewValues["LEIAMPARO"] != null ? e.NewValues["LEIAMPARO"].ToString().Trim().ToUpper() : null;
            motivo.CasoEspecifico = (e.NewValues["CASOESPECIFICO"] == null || Convert.ToBoolean(e.NewValues["CASOESPECIFICO"]) == false) ? false : true;
            motivo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivo.UsuarioId = User.Identity.Name;
            motivo.JustificativaFaltaId = Convert.ToInt32(e.Keys["JUSTIFICATIVAFALTAID"]);


            validacao = rnJustificativaFalta.Valida(motivo, true);

            if (validacao.Valido)
            {
                rnJustificativaFalta.Atualiza(motivo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdJustificativaFalta.DataBind();
        }

        protected void grdJustificativaFalta_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.JustificativaFalta rnJustificativaFalta = new Techne.Lyceum.RN.RecursosHumanos.JustificativaFalta();
            int motivoId = 0;

            motivoId = Convert.ToInt32(e.Keys["JUSTIFICATIVAFALTAID"]);

            validacao = rnJustificativaFalta.ValidaRemocao(motivoId);

            if (validacao.Valido)
            {
                rnJustificativaFalta.Remove(motivoId);
                grdJustificativaFalta.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

    }
}
