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


namespace Techne.Lyceum.Net.AvaliacaoExterna
{
    [NavUrl("~/AvaliacaoExterna/StatusParticipacao.aspx")]
    [ControlText("Status Participação")]
    [Title("Status Participação")]

    public partial class StatusParticipacao : TPage
    {
        public object Lista()
        {
            RN.AvaliacaoExterna.SituacaoParticipante rnSituacaoParticipante = new Techne.Lyceum.RN.AvaliacaoExterna.SituacaoParticipante();

            return rnSituacaoParticipante.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object SITUACAOPARTICIPANTEID) { }
        public void Delete(object SITUACAOPARTICIPANTEID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdStatusParticipacao, "Status Participante");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdStatusParticipacao);
        }

        protected void grdStatusParticipacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdStatusParticipacao);
        }

        protected void grdStatusParticipacao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdStatusParticipacao.Settings.ShowFilterRow = false;
        }

        protected void grdStatusParticipacao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdStatusParticipacao.Settings.ShowFilterRow = false;
        }

        protected void grdStatusParticipacao_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.AvaliacaoExterna.Entidades.SituacaoParticipante situacaoParticipante = new Techne.Lyceum.RN.AvaliacaoExterna.Entidades.SituacaoParticipante();
            RN.AvaliacaoExterna.SituacaoParticipante rnSituacaoParticipante = new Techne.Lyceum.RN.AvaliacaoExterna.SituacaoParticipante();

            situacaoParticipante.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            situacaoParticipante.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            situacaoParticipante.UsuarioId = User.Identity.Name;

            validacao = rnSituacaoParticipante.Valida(situacaoParticipante, true);

            if (validacao.Valido)
            {
                rnSituacaoParticipante.Insere(situacaoParticipante);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdStatusParticipacao.DataBind();

        }

        protected void grdStatusParticipacao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.AvaliacaoExterna.Entidades.SituacaoParticipante situacaoParticipante = new Techne.Lyceum.RN.AvaliacaoExterna.Entidades.SituacaoParticipante();
            RN.AvaliacaoExterna.SituacaoParticipante rnSituacaoParticipante = new Techne.Lyceum.RN.AvaliacaoExterna.SituacaoParticipante();

            situacaoParticipante.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            situacaoParticipante.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            situacaoParticipante.UsuarioId = User.Identity.Name;
            situacaoParticipante.SituacaoParticipanteId = Convert.ToInt32(e.Keys["SITUACAOPARTICIPANTEID"]);


            validacao = rnSituacaoParticipante.Valida(situacaoParticipante, true);

            if (validacao.Valido)
            {
                rnSituacaoParticipante.Atualiza(situacaoParticipante);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdStatusParticipacao.DataBind();
        }

        protected void grdStatusParticipacao_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.AvaliacaoExterna.SituacaoParticipante rnSituacaoParticipante = new Techne.Lyceum.RN.AvaliacaoExterna.SituacaoParticipante();
            int situacaoParticipanteId = 0;

            situacaoParticipanteId = Convert.ToInt32(e.Keys["SITUACAOPARTICIPANTEID"]);

            validacao = rnSituacaoParticipante.ValidaRemocao(situacaoParticipanteId);

            if (validacao.Valido)
            {
                rnSituacaoParticipante.Remove(situacaoParticipanteId);
                grdStatusParticipacao.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
