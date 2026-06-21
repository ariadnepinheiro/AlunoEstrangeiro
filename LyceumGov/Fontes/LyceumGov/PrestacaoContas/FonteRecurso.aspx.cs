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
         NavUrl("~/PrestacaoContas/FonteRecurso.aspx"),
         ControlText("FonteRecurso"),
         Title("Fonte Recurso")
     ]
    public partial class FonteRecurso : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.FonteRecurso rnFonteRecurso = new Techne.Lyceum.RN.PrestacaoContas.FonteRecurso();

            return rnFonteRecurso.Lista();

        }

        public void Update(object CODIGOSEFAZ, object DESCRICAO, object DATAINICIO,object DATAFIM, object FONTERECURSOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdFonteRecurso, "Fonte Recurso");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdFonteRecurso);
        }

        protected void grdFonteRecurso_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdFonteRecurso);
        }		

        protected void grdFonteRecurso_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdFonteRecurso.Settings.ShowFilterRow = false;
        }

        protected void grdFonteRecurso_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdFonteRecurso.Settings.ShowFilterRow = false;
        }

        protected void grdFonteRecurso_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdFonteRecurso.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "DESCRICAO")
                {
                    e.Editor.ReadOnly = false;
                }
            }
            else if (grdFonteRecurso.IsEditing)
            {

                if ((e.Column.FieldName) == "DESCRICAO")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.ClientEnabled = false;
                }              
            }
        }


        protected void grdFonteRecurso_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.FonteRecurso fonteRecurso = new Techne.Lyceum.RN.PrestacaoContas.Entidades.FonteRecurso();
            RN.PrestacaoContas.FonteRecurso rnFonteRecurso = new RN.PrestacaoContas.FonteRecurso();

            fonteRecurso.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            fonteRecurso.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            fonteRecurso.FonteRecursoId = Convert.ToInt32(e.Keys["FONTERECURSOID"]);
            fonteRecurso.UsuarioId = User.Identity.Name;

            validacao = rnFonteRecurso.ValidaAtualizaDatas(fonteRecurso.FonteRecursoId, fonteRecurso.DataInicio, fonteRecurso.DataFim, fonteRecurso.UsuarioId);

            if (validacao.Valido)
            {
                rnFonteRecurso.AtualizaDatas(fonteRecurso.FonteRecursoId, fonteRecurso.DataInicio, fonteRecurso.DataFim, fonteRecurso.UsuarioId);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdFonteRecurso.DataBind();
        }



    }
}
