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


namespace Techne.Lyceum.Net.PrestacaoContas
{
    [
         NavUrl("~/PrestacaoContas/DeclaracaoAAE.aspx"),
         ControlText("DeclaracaoAAE"),
         Title("Declaração AAE")
     ]
    public partial class DeclaracaoAAE : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.DeclaracaoAae rnDeclaracaoAAE = new Techne.Lyceum.RN.PrestacaoContas.DeclaracaoAae();

            return rnDeclaracaoAAE.Lista();

        }

        public void Insert(object DESCRICAO,object PERIODICIDADE, object DATAINICIO,object DATAFIM, object OBRIGATORIO) { }
        public void Update(object DESCRICAO, object PERIODICIDADE, object DATAINICIO, object DATAFIM, object OBRIGATORIO, object DECLARACAOAAEID) { }
        public void Delete(object DECLARACAOAAEID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdDeclaracaoAAE, "Declaração AAE");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdDeclaracaoAAE);
        }

        protected void grdDeclaracaoAAE_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdDeclaracaoAAE);
        }		

        protected void grdDeclaracaoAAE_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdDeclaracaoAAE.Settings.ShowFilterRow = false;
        }

        protected void grdDeclaracaoAAE_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["OBRIGATORIO"] = true;
            grdDeclaracaoAAE.Settings.ShowFilterRow = false;
        }

        protected void grdDeclaracaoAAE_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.DeclaracaoAae declaracao = new Techne.Lyceum.RN.PrestacaoContas.Entidades.DeclaracaoAae();
            RN.PrestacaoContas.DeclaracaoAae rnDeclaracaoAAE = new RN.PrestacaoContas.DeclaracaoAae();

            declaracao.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            declaracao.Periodicidade = e.NewValues["PERIODICIDADE"] != null ? Convert.ToInt32(e.NewValues["PERIODICIDADE"]) : -1;
            declaracao.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            declaracao.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            declaracao.Obrigatorio = (e.NewValues["OBRIGATORIO"] == null || Convert.ToBoolean(e.NewValues["OBRIGATORIO"]) == false) ? false : true;
            declaracao.UsuarioId = User.Identity.Name;

            validacao = rnDeclaracaoAAE.Valida(declaracao, true);

            if (validacao.Valido)
            {
                rnDeclaracaoAAE.Insere(declaracao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdDeclaracaoAAE.DataBind();

        }

        protected void grdDeclaracaoAAE_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.DeclaracaoAae declaracao = new Techne.Lyceum.RN.PrestacaoContas.Entidades.DeclaracaoAae();
            RN.PrestacaoContas.DeclaracaoAae rnDeclaracaoAAE = new RN.PrestacaoContas.DeclaracaoAae();

            declaracao.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            declaracao.Periodicidade = e.NewValues["PERIODICIDADE"] != null ? Convert.ToInt32(e.NewValues["PERIODICIDADE"]) : -1;
            declaracao.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            declaracao.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            declaracao.Obrigatorio = (e.NewValues["OBRIGATORIO"] == null || Convert.ToBoolean(e.NewValues["OBRIGATORIO"]) == false) ? false : true;
            declaracao.DeclaracaoAaeId = Convert.ToInt32(e.Keys["DECLARACAOAAEID"]);
            declaracao.UsuarioId = User.Identity.Name;

            validacao = rnDeclaracaoAAE.Valida(declaracao, true);

            if (validacao.Valido)
            {
                rnDeclaracaoAAE.Atualiza(declaracao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdDeclaracaoAAE.DataBind();
        }

        protected void grdDeclaracaoAAE_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.DeclaracaoAae rnDeclaracaoAAE = new RN.PrestacaoContas.DeclaracaoAae();
            int declaracaoId = 0;

            declaracaoId = Convert.ToInt32(e.Keys["DECLARACAOAAEID"]);

            validacao = rnDeclaracaoAAE.ValidaRemocao(declaracaoId);

            if (validacao.Valido)
            {
                rnDeclaracaoAAE.Remove(declaracaoId);
                grdDeclaracaoAAE.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }


    }
}
