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
         NavUrl("~/Ocorrencia/Classe.aspx"),
         ControlText("Classe"),
         Title("Classe")
     ]
    public partial class Classe : TPage
    {
        public object Lista()
        {
            RN.Ocorrencias.Classe rnClasse = new Techne.Lyceum.RN.Ocorrencias.Classe();

            return rnClasse.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO,object ORDEM) { }
        public void Update(object DESCRICAO, object ATIVO,object ORDEM, object CLASSEID) { }
        public void Delete(object CLASSEID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdClasse, "Classe");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdClasse);
        }

        protected void grdClasse_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdClasse);
        }		
        
        protected void grdClasse_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdClasse.Settings.ShowFilterRow = false;
        }

        protected void grdClasse_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdClasse.Settings.ShowFilterRow = false;
        }

        protected void grdClasse_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Ocorrencias.Entidades.Classe classe = new Techne.Lyceum.RN.Ocorrencias.Entidades.Classe();
            RN.Ocorrencias.Classe rnClasse = new RN.Ocorrencias.Classe();

            classe.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            classe.Ordem = e.NewValues["ORDEM"] != null ? Convert.ToInt32(e.NewValues["ORDEM"]) : -1;
            classe.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            classe.UsuarioId = User.Identity.Name;

            validacao = rnClasse.Valida(classe, true);

            if (validacao.Valido)
            {
                rnClasse.Insere(classe);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdClasse.DataBind();

        }

        protected void grdClasse_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Ocorrencias.Entidades.Classe classe = new Techne.Lyceum.RN.Ocorrencias.Entidades.Classe();
            RN.Ocorrencias.Classe rnClasse = new RN.Ocorrencias.Classe();

            classe.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            classe.Ordem = e.NewValues["ORDEM"] != null ? Convert.ToInt32(e.NewValues["ORDEM"]) : -1;
            classe.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            classe.ClasseId = Convert.ToInt32(e.Keys["CLASSEID"]);
            classe.UsuarioId = User.Identity.Name;

            validacao = rnClasse.Valida(classe, true);

            if (validacao.Valido)
            {
                rnClasse.Atualiza(classe);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdClasse.DataBind();
        }

        protected void grdClasse_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Ocorrencias.Classe rnClasse = new RN.Ocorrencias.Classe();
            int classeId = 0;

            classeId = Convert.ToInt32(e.Keys["CLASSEID"]);

            validacao = rnClasse.ValidaRemocao(classeId);

            if (validacao.Valido)
            {
                rnClasse.Remove(classeId);
                grdClasse.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

      
       
    }
}
