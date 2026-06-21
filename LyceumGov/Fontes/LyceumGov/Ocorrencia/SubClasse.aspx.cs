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
using Techne.Controls;


namespace Techne.Lyceum.Net.Ocorrencia
{
    [
         NavUrl("~/Ocorrencia/SubClasse.aspx"),
         ControlText("SubClasse"),
         Title("SubClasse")
     ]
    public partial class SubClasse : TPage
    {
        public object Lista( object classeId)
        {
            RN.Ocorrencias.SubClasse rnClasseSub = new Techne.Lyceum.RN.Ocorrencias.SubClasse();

            string id = Convert.ToString(classeId);

            if (!id.IsNullOrEmptyOrWhiteSpace())
            {
                return rnClasseSub.ListaPor(Convert.ToInt32(classeId));
            }
            return null;

        }

        public void Insert(object DESCRICAO, object ATIVO, object ORDEM) { }
        public void Update(object DESCRICAO, object ATIVO, object ORDEM, object SUBCLASSEID) { }
        public void Delete(object SUBCLASSEID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdSubClasse, "SubClasse");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdSubClasse);
        }

        protected void grdSubClasse_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdSubClasse);
        }		
        
        protected void grdSubClasse_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdSubClasse.Settings.ShowFilterRow = false;
        }

        protected void grdSubClasse_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdSubClasse.Settings.ShowFilterRow = false;
        }

        protected void grdSubClasse_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Ocorrencias.Entidades.SubClasse subClasse = new Techne.Lyceum.RN.Ocorrencias.Entidades.SubClasse();
            RN.Ocorrencias.SubClasse rnClasse = new RN.Ocorrencias.SubClasse();

            subClasse.ClasseId = !tseClasse.DBValue.IsNull && tseClasse.IsValidDBValue ? Convert.ToInt32(tseClasse.DBValue) : -1;
            
            if (subClasse.ClasseId <= 0)
            {
                e.Cancel = true;
                throw new Exception("Para adicionar uma subclasse selecione uma classe acima.");
            }
            else
            {
                subClasse.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
                subClasse.Ordem = e.NewValues["ORDEM"] != null ? Convert.ToInt32(e.NewValues["ORDEM"]) : -1;
                subClasse.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
                subClasse.UsuarioId = User.Identity.Name;
                validacao = rnClasse.Valida(subClasse, true);

                if (validacao.Valido)
                {
                    rnClasse.Insere(subClasse);
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }
            }

            grdSubClasse.DataBind();

        }

        protected void grdSubClasse_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Ocorrencias.Entidades.SubClasse subClasse = new Techne.Lyceum.RN.Ocorrencias.Entidades.SubClasse();
            RN.Ocorrencias.SubClasse rnClasse = new RN.Ocorrencias.SubClasse();

            subClasse.ClasseId = !tseClasse.DBValue.IsNull && tseClasse.IsValidDBValue ? Convert.ToInt32(tseClasse.DBValue) : -1;
            subClasse.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            subClasse.Ordem = e.NewValues["ORDEM"] != null ? Convert.ToInt32(e.NewValues["ORDEM"]) : -1;
            subClasse.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            subClasse.SubClasseId = Convert.ToInt32(e.Keys["SUBCLASSEID"]);
            subClasse.UsuarioId = User.Identity.Name;

            validacao = rnClasse.Valida(subClasse, true);

            if (validacao.Valido)
            {
                rnClasse.Atualiza(subClasse);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdSubClasse.DataBind();
        }

        protected void grdSubClasse_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Ocorrencias.SubClasse rnClasse = new RN.Ocorrencias.SubClasse();
            int classeId = 0;

            classeId = Convert.ToInt32(e.Keys["SUBCLASSEID"]);

            validacao = rnClasse.ValidaRemocao(classeId);

            if (validacao.Valido)
            {
                rnClasse.Remove(classeId);
                grdSubClasse.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        protected void tseClasse_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                lblMensagem.Text = string.Empty;

                if (!tseClasse.DBValue.IsNull)
                {
                    if (!tseClasse.IsValidDBValue)
                    {

                        lblMensagem.Text = "Classe não cadastrada (favor verificar).";
                    }

                }
                else
                {

                    lblMensagem.Text = "Classe não cadastrada (favor verificar).";
                }


            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
       
    }
}
