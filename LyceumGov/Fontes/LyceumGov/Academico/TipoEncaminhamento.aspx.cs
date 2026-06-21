using System;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/TipoEncaminhamento.aspx")]
    [ControlText("Encaminhamentos Realizados pela UE")]
    [Title("Encaminhamentos Realizados pela UE")]

    public partial class TipoEncaminhamento : TPage
    {
        public object Lista()
        {
            RN.Turmas.TipoEncaminhamento rnTipoEncaminhamento = new Techne.Lyceum.RN.Turmas.TipoEncaminhamento();

            return rnTipoEncaminhamento.Lista();
        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object TIPOENCAMINHAMENTOID) { }
        public void Delete(object TIPOENCAMINHAMENTOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTipoEncaminhamento, "Encaminhamentos Realizados pela Unidade Escolar");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTipoEncaminhamento);
        }

        protected void grdTipoEncaminhamento_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTipoEncaminhamento);
        }

        protected void grdTipoEncaminhamento_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTipoEncaminhamento.Settings.ShowFilterRow = false;
        }

        protected void grdTipoEncaminhamento_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdTipoEncaminhamento.Settings.ShowFilterRow = false;
        }

        protected void grdTipoEncaminhamento_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Turmas.Entidades.TipoEncaminhamento medida = new Techne.Lyceum.RN.Turmas.Entidades.TipoEncaminhamento();
            RN.Turmas.TipoEncaminhamento rnTipoEncaminhamento = new Techne.Lyceum.RN.Turmas.TipoEncaminhamento();

            medida.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            medida.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            medida.UsuarioId = User.Identity.Name;

            validacao = rnTipoEncaminhamento.Valida(medida, true);

            if (validacao.Valido)
            {
                rnTipoEncaminhamento.Insere(medida);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoEncaminhamento.DataBind();

        }

        protected void grdTipoEncaminhamento_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Turmas.Entidades.TipoEncaminhamento medida = new Techne.Lyceum.RN.Turmas.Entidades.TipoEncaminhamento();
            RN.Turmas.TipoEncaminhamento rnTipoEncaminhamento = new Techne.Lyceum.RN.Turmas.TipoEncaminhamento();

            medida.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            medida.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            medida.UsuarioId = User.Identity.Name;
            medida.TipoEncaminhamentoId = Convert.ToInt32(e.Keys["TIPOENCAMINHAMENTOID"]);

            validacao = rnTipoEncaminhamento.Valida(medida, true);

            if (validacao.Valido)
            {
                rnTipoEncaminhamento.Atualiza(medida);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoEncaminhamento.DataBind();
        }

        protected void grdTipoEncaminhamento_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Turmas.TipoEncaminhamento rnTipoEncaminhamento = new Techne.Lyceum.RN.Turmas.TipoEncaminhamento();
            int Id = 0;

            Id = Convert.ToInt32(e.Keys["TIPOENCAMINHAMENTOID"]);

            validacao = rnTipoEncaminhamento.ValidaRemocao(Id);

            if (validacao.Valido)
            {
                rnTipoEncaminhamento.Remove(Id);
                grdTipoEncaminhamento.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

    }
}
