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
         NavUrl("~/PrestacaoContas/ProgramaTrabalho.aspx"),
         ControlText("ProgramaTrabalho"),
         Title("Programa de Trabalho")
     ]
    public partial class ProgramaTrabalho : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.ProgramaTrabalho rnProgramaTrabalho = new Techne.Lyceum.RN.PrestacaoContas.ProgramaTrabalho();

            return rnProgramaTrabalho.Lista();

        }

        public void Update(object DESCRICAO, object PT, object PTRES, object UO, object ATIVO, object PROGRAMATRABALHOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdProgramaTrabalho, "Programa de Trabalho");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdProgramaTrabalho);
        }

        protected void grdProgramaTrabalho_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdProgramaTrabalho);
        }		

        protected void grdProgramaTrabalho_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdProgramaTrabalho.Settings.ShowFilterRow = false;
        }

        protected void grdProgramaTrabalho_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdProgramaTrabalho.Settings.ShowFilterRow = false;
        }

        protected void grdProgramaTrabalho_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.ProgramaTrabalho programa = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ProgramaTrabalho();
            RN.PrestacaoContas.ProgramaTrabalho rnProgramaTrabalho = new RN.PrestacaoContas.ProgramaTrabalho();

            programa.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            programa.ProgramaTrabalhoId = Convert.ToInt32(e.Keys["PROGRAMATRABALHOID"]);
            programa.UsuarioId = User.Identity.Name;

            validacao = rnProgramaTrabalho.ValidaAtualizaAtivo(programa.ProgramaTrabalhoId,programa.Ativo,programa.UsuarioId);

            if (validacao.Valido)
            {
                rnProgramaTrabalho.AtualizaAtivo(programa.ProgramaTrabalhoId, programa.Ativo, programa.UsuarioId);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdProgramaTrabalho.DataBind();
        }



    }
}
