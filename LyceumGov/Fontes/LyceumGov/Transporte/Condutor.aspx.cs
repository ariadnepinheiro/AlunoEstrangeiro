using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Transporte
{
    [NavUrl("~/Transporte/Condutor.aspx")]
    [ControlText("Condutor")]
    [Title("Condutor")]

    public partial class Condutor : TPage
    {
        public object ListarCondutor()
        {
            RN.Transporte.Condutor rnCondutor = new Techne.Lyceum.RN.Transporte.Condutor();

            return rnCondutor.Lista();
        }


        public void Update(object CPF, object NOME, object NUMEROCNH, object ATIVO, object DATAVALIDADECNH,object CATEGORIA, object CONDUTORID) { }
        public void Insert(object CPF, object NOME, object NUMEROCNH, object ATIVO, object DATAVALIDADECNH,object CATEGORIA) { }        
        public void Delete(object CONDUTORID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdCondutor, "Condutor");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdCondutor);
        }

        protected void grdCondutor_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdCondutor.Settings.ShowFilterRow = false;
        }

        protected void grdCondutor_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdCondutor.Settings.ShowFilterRow = false;
        }

        protected void grdCondutor_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdCondutor.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "ATIVO")
                {
                    e.Editor.Value = true;
                }
            }
        }
        protected void grdCondutor_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "CPF" && e.Value != null)
            {

                e.DisplayText = string.Format(@"{0:000\.000\.000\-00}", e.Value);
                //else
                //    e.DisplayText = "";
            }
        }
        protected void grdCondutor_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Entidades.Condutor condutor = new Techne.Lyceum.RN.Transporte.Entidades.Condutor();
            RN.Transporte.Condutor rnCondutor = new Techne.Lyceum.RN.Transporte.Condutor();

            condutor.CondutorId = e.NewValues["CONDUTORID"] != null ? Convert.ToInt32(e.NewValues["CONDUTORID"]) : -1;
            condutor.Cpf = e.NewValues["CPF"] != null ? e.NewValues["CPF"].ToString().RetirarMascaraCPF() : null;
            condutor.Nome = e.NewValues["NOME"] != null ? e.NewValues["NOME"].ToString().Trim().ToUpper() : null;
            condutor.NumeroCnh = e.NewValues["NUMEROCNH"] != null ? e.NewValues["NUMEROCNH"].ToString() : null;
            condutor.Categoria = e.NewValues["CATEGORIA"] != null ? e.NewValues["CATEGORIA"].ToString() : null;
            condutor.DataValidadeCnh = e.NewValues["DATAVALIDADECNH"] != null ? Convert.ToDateTime(e.NewValues["DATAVALIDADECNH"]) : DateTime.MinValue;
            condutor.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            condutor.UsuarioId = User.Identity.Name;
            
            validacao = rnCondutor.Valida(condutor, true);

            if (validacao.Valido)
            {
                rnCondutor.Insere(condutor);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdCondutor.DataBind();

        }

        protected void grdCondutor_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Entidades.Condutor condutor = new Techne.Lyceum.RN.Transporte.Entidades.Condutor();
            RN.Transporte.Condutor rnCondutor = new Techne.Lyceum.RN.Transporte.Condutor();

            condutor.Cpf = e.NewValues["CPF"] != null ? e.NewValues["CPF"].ToString().RetirarMascaraCPF() : null;
            condutor.Nome = e.NewValues["NOME"] != null ? e.NewValues["NOME"].ToString().Trim().ToUpper() : null;
            condutor.NumeroCnh = e.NewValues["NUMEROCNH"] != null ? e.NewValues["NUMEROCNH"].ToString() : null;
            condutor.DataValidadeCnh = e.NewValues["DATAVALIDADECNH"] != null ? Convert.ToDateTime(e.NewValues["DATAVALIDADECNH"]) : DateTime.MinValue;
            condutor.Categoria = e.NewValues["CATEGORIA"] != null ? e.NewValues["CATEGORIA"].ToString() : null;
            condutor.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            condutor.UsuarioId = User.Identity.Name;
            condutor.CondutorId = Convert.ToInt32(e.Keys["CONDUTORID"]);

            validacao = rnCondutor.Valida(condutor, true);

            if (validacao.Valido)
            {
                rnCondutor.Atualiza(condutor);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdCondutor.DataBind();
        }

        protected void grdCondutor_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Condutor rnCondutor = new Techne.Lyceum.RN.Transporte.Condutor();
            int condutorId = 0;

            condutorId = Convert.ToInt32(e.Keys["CONDUTORID"]);

            validacao = rnCondutor.ValidaRemocao(condutorId);

            if (validacao.Valido)
            {
                rnCondutor.Remove(condutorId);
                grdCondutor.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
