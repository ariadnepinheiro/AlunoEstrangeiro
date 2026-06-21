using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;


namespace Techne.Lyceum.Net.Ocorrencia
{
    [
         NavUrl("~/Ocorrencia/MotivoCancelamento.aspx"),
         ControlText("MotivoCancelamento"),
         Title("Motivo Cancelamento")
     ]
    public partial class MotivoCancelamento : TPage
    {
        public object Lista()
        {
            RN.Ocorrencias.MotivoCancelamento rnMotivoCancelamento = new Techne.Lyceum.RN.Ocorrencias.MotivoCancelamento();

            return rnMotivoCancelamento.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object MOTIVOCANCELAMENTOID) { }
        public void Delete(object MOTIVOCANCELAMENTOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMotivoCancelamento, "Motivo Cancelamento");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdMotivoCancelamento);
        }

        protected void grdMotivoCancelamento_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdMotivoCancelamento);
        }		

        protected void grdMotivoCancelamento_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMotivoCancelamento.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoCancelamento_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdMotivoCancelamento.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoCancelamento_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Ocorrencias.Entidades.MotivoCancelamento motivo = new Techne.Lyceum.RN.Ocorrencias.Entidades.MotivoCancelamento();
            RN.Ocorrencias.MotivoCancelamento rnMotivoCancelamento = new RN.Ocorrencias.MotivoCancelamento();

            motivo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivo.UsuarioId = User.Identity.Name;

            validacao = rnMotivoCancelamento.Valida(motivo, true);

            if (validacao.Valido)
            {
                rnMotivoCancelamento.Insere(motivo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoCancelamento.DataBind();

        }

        protected void grdMotivoCancelamento_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Ocorrencias.Entidades.MotivoCancelamento motivo = new Techne.Lyceum.RN.Ocorrencias.Entidades.MotivoCancelamento();
            RN.Ocorrencias.MotivoCancelamento rnMotivoCancelamento = new RN.Ocorrencias.MotivoCancelamento();

            motivo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivo.MotivoCancelamentoId = Convert.ToInt32(e.Keys["MOTIVOCANCELAMENTOID"]);
            motivo.UsuarioId = User.Identity.Name;

            validacao = rnMotivoCancelamento.Valida(motivo, true);

            if (validacao.Valido)
            {
                rnMotivoCancelamento.Atualiza(motivo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoCancelamento.DataBind();
        }

        protected void grdMotivoCancelamento_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Ocorrencias.MotivoCancelamento rnMotivoCancelamento = new RN.Ocorrencias.MotivoCancelamento();
            int motivoId = 0;

            motivoId = Convert.ToInt32(e.Keys["MOTIVOCANCELAMENTOID"]);

            validacao = rnMotivoCancelamento.ValidaRemocao(motivoId);

            if (validacao.Valido)
            {
                rnMotivoCancelamento.Remove(motivoId);
                grdMotivoCancelamento.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

       

    }
}
