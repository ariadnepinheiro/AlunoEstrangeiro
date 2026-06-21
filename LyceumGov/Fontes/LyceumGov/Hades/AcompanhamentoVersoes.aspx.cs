namespace Techne.Lyceum.Net.Hades
{
    using System;
    using DevExpress.Web.ASPxGridView;
    using DevExpress.Web.Data;
    using Techne.Lyceum.RN;
    using Techne.Web;

    [NavUrl("~/Hades/AcompanhamentoVersoes.aspx")]
    [ControlText("Acompanhamento das Versões")]
    [Title("Acompanhamento das Versões")]
    public partial class AcompanhamentoVersoes : TPage
    {
        public object Listar()
        {
            return Versao.Listar();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdVersoes, "Versões");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void grdVersoes_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs asPxGridViewAfterPerformCallbackEventArgs)
        {
            this.ControlaAcesso(this.grdVersoes);
        }

        protected void grdVersoes_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            var idVersao = Convert.ToInt32(e.Values["ID_VERSAO"]);

            Versao.Remover(idVersao);

            e.Cancel = true;
            this.grdVersoes.CancelEdit();
        }

        protected void grdVersoes_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            var versao = Versao.Bind(null, e.NewValues);

            versao.Usuario = User.Identity.Name;

            Versao.Inserir(versao);

            e.Cancel = true;
            this.grdVersoes.CancelEdit();
        }

        protected void grdVersoes_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            var versao = Versao.Bind(e.Keys, e.NewValues);

            versao.Usuario = User.Identity.Name;

            Versao.Atualizar(versao);

            e.Cancel = true;
            this.grdVersoes.CancelEdit();
        }

        protected void grdVersoes_RowValidating(object sender, ASPxDataValidationEventArgs e)
        {
            var versao = Versao.Bind(e.Keys, e.NewValues);
            var validacao = Versao.Validar(versao);

            if (!validacao.Valido)
            {
                e.RowError = validacao.Mensagem;
            }
        }
    }
}