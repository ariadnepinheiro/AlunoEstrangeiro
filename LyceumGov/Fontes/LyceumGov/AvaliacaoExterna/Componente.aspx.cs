using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Matriculas;
using Techne.Web;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.AvaliacaoExterna
{
    [NavUrl("~/AvaliacaoExterna/Componente.aspx")]
    [ControlText("Componentes")]
    [Title("Componentes")]
    public partial class Componente : TPage
    {
        public object Lista()
        {
            RN.AvaliacaoExterna.Componente rnComponente = new Techne.Lyceum.RN.AvaliacaoExterna.Componente();

            return rnComponente.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object COMPONENTEID) { }
        public void Delete(object COMPONENTEID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdComponente, "Componentes");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcessoGrid();
        }


        protected void ControlaAcessoGrid()
        {
            if (grdComponente != null)
            {
                if (!Permission.AllowDelete && !Permission.AllowInsert && !Permission.AllowUpdate)
                {
                    grdComponente.Columns[""].Visible = false;
                }
            }

            foreach (GridViewColumn col in grdComponente.Columns)
            {
                if (col is GridViewCommandColumn)
                {
                    if (((GridViewCommandColumn)col).CustomButtons["cmdHabilidade"] != null)
                    {
                        ((GridViewCommandColumn)col).CustomButtons["cmdHabilidade"].Visibility =
                            Permission.AllowInsert ? GridViewCustomButtonVisibility.AllDataRows : GridViewCustomButtonVisibility.Invisible;
                    }                  
                }
            }

            ControlaAcesso(grdComponente);
        }

        protected void grdComponente_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdComponente);
        }

        protected void grdComponente_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        { 
            Dictionary<string, string> qryPars = new Dictionary<string, string>();
            var row = grdComponente.GetRowValues(e.VisibleIndex, "COMPONENTEID");
            qryPars = new Dictionary<string, string>();
            qryPars.Add("componenteId", Convert.ToString(row));
            string queryString = TPage.CodificaQueryString(qryPars);

            switch (e.ButtonID)
            {
                case "cmdHabilidade":
                    Response.RedirectLocation = string.Format("Habilidade.aspx?{0}", queryString);
                    break;

                default:
                    break;
            }            
        }

        protected void grdComponente_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdComponente.Settings.ShowFilterRow = false;
        }

        protected void grdComponente_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdComponente.Settings.ShowFilterRow = false;
        }

        protected void grdComponente_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.AvaliacaoExterna.Entidades.Componente componente = new Techne.Lyceum.RN.AvaliacaoExterna.Entidades.Componente();
            RN.AvaliacaoExterna.Componente rnComponente = new Techne.Lyceum.RN.AvaliacaoExterna.Componente();

            componente.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            componente.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            componente.UsuarioId = User.Identity.Name;

            validacao = rnComponente.Valida(componente, true);

            if (validacao.Valido)
            {
                rnComponente.Insere(componente);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdComponente.DataBind();

        }

        protected void grdComponente_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.AvaliacaoExterna.Entidades.Componente componente = new Techne.Lyceum.RN.AvaliacaoExterna.Entidades.Componente();
            RN.AvaliacaoExterna.Componente rnComponente = new Techne.Lyceum.RN.AvaliacaoExterna.Componente();

            componente.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            componente.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;            
            componente.UsuarioId = User.Identity.Name;
            componente.ComponenteId = Convert.ToInt32(e.Keys["COMPONENTEID"]);


            validacao = rnComponente.Valida(componente, true);

            if (validacao.Valido)
            {
                rnComponente.Atualiza(componente);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdComponente.DataBind();
        }

        protected void grdComponente_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.AvaliacaoExterna.Componente rnComponente = new Techne.Lyceum.RN.AvaliacaoExterna.Componente();
            int componenteId = 0;

            componenteId = Convert.ToInt32(e.Keys["COMPONENTEID"]);

            validacao = rnComponente.ValidaRemocao(componenteId);

            if (validacao.Valido)
            {
                rnComponente.Remove(componenteId);
                grdComponente.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
