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
     NavUrl("~/Ocorrencia/Tratamento.aspx"),
     ControlText("Tratamento"),
     Title("Tratamento")
 ]
    public partial class Tratamento : TPage
    {
        public object Lista()
        {
            RN.Ocorrencias.Tratamento rnTratamento = new Techne.Lyceum.RN.Ocorrencias.Tratamento();

            return rnTratamento.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO, object ORDEM) { }
        public void Update(object DESCRICAO, object ATIVO, object ORDEM, object TratamentoID) { }
        public void Delete(object TratamentoID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTratamento, "Tratamento");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTratamento);
        }

        protected void grdTratamento_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTratamento);
        }

        protected void grdTratamento_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTratamento.Settings.ShowFilterRow = false;
        }

        protected void grdTratamento_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdTratamento.Settings.ShowFilterRow = false;
        }

        protected void grdTratamento_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Ocorrencias.Entidades.Tratamento tratamento = new Techne.Lyceum.RN.Ocorrencias.Entidades.Tratamento();
            RN.Ocorrencias.Tratamento rnTratamento = new RN.Ocorrencias.Tratamento();

            tratamento.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim() : null;
            tratamento.Ordem = e.NewValues["ORDEM"] != null ? Convert.ToInt32(e.NewValues["ORDEM"]) : -1;
            tratamento.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            tratamento.UsuarioId = User.Identity.Name;

            validacao = rnTratamento.Valida(tratamento, true);

            if (validacao.Valido)
            {
                rnTratamento.Insere(tratamento);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTratamento.DataBind();

        }

        protected void grdTratamento_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Ocorrencias.Entidades.Tratamento tratamento = new Techne.Lyceum.RN.Ocorrencias.Entidades.Tratamento();
            RN.Ocorrencias.Tratamento rnTratamento = new RN.Ocorrencias.Tratamento();

            tratamento.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim() : null;
            tratamento.Ordem = e.NewValues["ORDEM"] != null ? Convert.ToInt32(e.NewValues["ORDEM"]) : -1;
            tratamento.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            tratamento.TratamentoId = Convert.ToInt32(e.Keys["TRATAMENTOID"]);
            tratamento.UsuarioId = User.Identity.Name;

            validacao = rnTratamento.Valida(tratamento, true);

            if (validacao.Valido)
            {
                rnTratamento.Atualiza(tratamento);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTratamento.DataBind();
        }

        protected void grdTratamento_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Ocorrencias.Tratamento rnTratamento = new RN.Ocorrencias.Tratamento();
            int TratamentoId = 0;

            TratamentoId = Convert.ToInt32(e.Keys["TRATAMENTOID"]);

            validacao = rnTratamento.ValidaRemocao(TratamentoId);

            if (validacao.Valido)
            {
                rnTratamento.Remove(TratamentoId);
                grdTratamento.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }



    }
}
