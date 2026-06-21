using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Interconectividade
{
    [
        NavUrl("~/Interconectividade/Operadora.aspx"),
        ControlText("Operadora"),
        Title("Operadora")
    ]
    public partial class Operadora : TPage
    {
        public object Lista()
        {
            RN.FiscalizacaoLink.Operadora rnOperadora = new Techne.Lyceum.RN.FiscalizacaoLink.Operadora();

            return rnOperadora.Lista();

        }

        public void Insert(object DESCRICAO, object CNPJOPERADORA, object ATIVO) { }
        public void Update(object DESCRICAO, object CNPJOPERADORA, object ATIVO, object OPERADORAID) { }
        public void Delete(object OPERADORAID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdOperadora, "Operadora");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdOperadora);
        }

        protected void grdOperadora_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdOperadora.Settings.ShowFilterRow = false;
        }

        protected void grdOperadora_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdOperadora.Settings.ShowFilterRow = false;
        }

        protected void grdOperadora_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.Entidades.Operadora operadora = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.Operadora();
            RN.FiscalizacaoLink.Operadora rnOperadora = new RN.FiscalizacaoLink.Operadora();

            operadora.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            operadora.CnpjOperadora = e.NewValues["CNPJOPERADORA"] != null ? e.NewValues["CNPJOPERADORA"].ToString().Trim().ToUpper() : null;
            operadora.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            operadora.UsuarioId = User.Identity.Name;

            validacao = rnOperadora.Valida(operadora, true);

            if (validacao.Valido)
            {
                rnOperadora.Insere(operadora);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdOperadora.DataBind();

        }

        protected void grdOperadora_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.Entidades.Operadora operadora = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.Operadora();
            RN.FiscalizacaoLink.Operadora rnOperadora = new RN.FiscalizacaoLink.Operadora();

            operadora.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            operadora.CnpjOperadora = e.NewValues["CNPJOPERADORA"] != null ? e.NewValues["CNPJOPERADORA"].ToString().Trim().ToUpper() : null;
            operadora.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            operadora.OperadoraId = Convert.ToInt32(e.Keys["OPERADORAID"]);
            operadora.UsuarioId = User.Identity.Name;

            validacao = rnOperadora.Valida(operadora, true);

            if (validacao.Valido)
            {
                rnOperadora.Atualiza(operadora);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdOperadora.DataBind();
        }

        protected void grdOperadora_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.Operadora rnOperadora = new RN.FiscalizacaoLink.Operadora();
            int operadoraId = 0;

            operadoraId = Convert.ToInt32(e.Keys["OPERADORAID"]);

            validacao = rnOperadora.ValidaRemocao(operadoraId);

            if (validacao.Valido)
            {
                rnOperadora.Remove(operadoraId);
                grdOperadora.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
