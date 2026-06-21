using System;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/FormaContato.aspx")]
    [ControlText("Formas de Contato com a Família")]
    [Title("Formas de Contato com a Família")]

    public partial class FormaContato : TPage
    {
        public object Lista()
        {
            RN.Turmas.FormaContato rnFormaContato = new Techne.Lyceum.RN.Turmas.FormaContato();

            return rnFormaContato.Lista();
        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object FORMACONTATOID) { }
        public void Delete(object FORMACONTATOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdFormaContato, "Cadastro de Formas de Contato com a Família/Rede de Apoio do Aluno");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdFormaContato);
        }

        protected void grdFormaContato_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdFormaContato);
        }

        protected void grdFormaContato_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdFormaContato.Settings.ShowFilterRow = false;
        }

        protected void grdFormaContato_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdFormaContato.Settings.ShowFilterRow = false;
        }

        protected void grdFormaContato_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Turmas.Entidades.FormaContato medida = new Techne.Lyceum.RN.Turmas.Entidades.FormaContato();
            RN.Turmas.FormaContato rnFormaContato = new Techne.Lyceum.RN.Turmas.FormaContato();

            medida.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            medida.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            medida.UsuarioId = User.Identity.Name;

            validacao = rnFormaContato.Valida(medida, true);

            if (validacao.Valido)
            {
                rnFormaContato.Insere(medida);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdFormaContato.DataBind();

        }

        protected void grdFormaContato_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Turmas.Entidades.FormaContato medida = new Techne.Lyceum.RN.Turmas.Entidades.FormaContato();
            RN.Turmas.FormaContato rnFormaContato = new Techne.Lyceum.RN.Turmas.FormaContato();

            medida.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            medida.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            medida.UsuarioId = User.Identity.Name;
            medida.FormaContatoId = Convert.ToInt32(e.Keys["FORMACONTATOID"]);

            validacao = rnFormaContato.Valida(medida, true);

            if (validacao.Valido)
            {
                rnFormaContato.Atualiza(medida);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdFormaContato.DataBind();
        }

        protected void grdFormaContato_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Turmas.FormaContato rnFormaContato = new Techne.Lyceum.RN.Turmas.FormaContato();
            int Id = 0;

            Id = Convert.ToInt32(e.Keys["FORMACONTATOID"]);

            validacao = rnFormaContato.ValidaRemocao(Id);

            if (validacao.Valido)
            {
                rnFormaContato.Remove(Id);
                grdFormaContato.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

    }
}
