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
         NavUrl("~/Ocorrencia/Meio.aspx"),
         ControlText("Meio"),
         Title("Meio")
     ]
    public partial class Meio : TPage
    {
        public object Lista()
        {
            RN.Ocorrencias.Meio rnMeio = new Techne.Lyceum.RN.Ocorrencias.Meio();

            return rnMeio.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO, object ORDEM) { }
        public void Update(object DESCRICAO, object ATIVO, object ORDEM, object MEIOID) { }
        public void Delete(object MEIOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMeio, "Meio");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdMeio);
        }

        protected void grdMeio_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdMeio);
        }		
        
        protected void grdMeio_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMeio.Settings.ShowFilterRow = false;
        }

        protected void grdMeio_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdMeio.Settings.ShowFilterRow = false;
        }

        protected void grdMeio_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Ocorrencias.Entidades.Meio meio = new Techne.Lyceum.RN.Ocorrencias.Entidades.Meio();
            RN.Ocorrencias.Meio rnMeio = new RN.Ocorrencias.Meio();

            meio.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            meio.Ordem = e.NewValues["ORDEM"] != null ? Convert.ToInt32(e.NewValues["ORDEM"]) : -1;
            meio.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            meio.UsuarioId = User.Identity.Name;

            validacao = rnMeio.Valida(meio, true);

            if (validacao.Valido)
            {
                rnMeio.Insere(meio);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMeio.DataBind();

        }

        protected void grdMeio_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Ocorrencias.Entidades.Meio meio = new Techne.Lyceum.RN.Ocorrencias.Entidades.Meio();
            RN.Ocorrencias.Meio rnMeio = new RN.Ocorrencias.Meio();

            meio.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            meio.Ordem = e.NewValues["ORDEM"] != null ? Convert.ToInt32(e.NewValues["ORDEM"]) : -1;
            meio.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            meio.MeioId = Convert.ToInt32(e.Keys["MEIOID"]);
            meio.UsuarioId = User.Identity.Name;

            validacao = rnMeio.Valida(meio, true);

            if (validacao.Valido)
            {
                rnMeio.Atualiza(meio);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMeio.DataBind();
        }

        protected void grdMeio_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Ocorrencias.Meio rnMeio = new RN.Ocorrencias.Meio();
            int MeioId = 0;

            MeioId = Convert.ToInt32(e.Keys["MEIOID"]);

            validacao = rnMeio.ValidaRemocao(MeioId);

            if (validacao.Valido)
            {
                rnMeio.Remove(MeioId);
                grdMeio.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

      
       
    }
}
