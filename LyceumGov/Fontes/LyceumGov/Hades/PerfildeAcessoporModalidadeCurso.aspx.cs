using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Entidades;
using Techne.Web;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.Data;

namespace Techne.Lyceum.Net.Hades
{
    [
        NavUrl("~/Hades/PerfildeAcessoporModalidadeCurso.aspx"),
         ControlText("Modalidade por Perfil de Acesso"),
         Title("Modalidade por Perfil de Acesso"),
    ]
    public partial class PerfildeAcessoporModalidadeCurso : TPage
    {
        public object Listar(object id_perfil)
        {
                if (id_perfil != null && !string.IsNullOrEmpty(id_perfil.ToString()))
                    return RN.PerfilModalidade.Listar(int.Parse(id_perfil.ToString()));

                return null;
            
        }
        protected void Page_Init(object sender, EventArgs e)
        {

            TituloGrid(grdPerfilModalidade, "Modalidades");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;

        }
        protected void grdPerfilModalidade_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdPerfilModalidade);
        }
        protected void grdPerfilModalidade_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdPerfilModalidade.Settings.ShowFilterRow = false;
        }

        protected void grdPerfilModalidade_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdPerfilModalidade.Settings.ShowFilterRow = false;
        }
        protected void grdPerfilModalidade_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdPerfilModalidade.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "PERFILMODALIDADEID")
                    e.Editor.Enabled = true;
            }
            else if (grdPerfilModalidade.IsEditing)
            {
                if ((e.Column.FieldName) == "PERFILMODALIDADEID")
                    e.Editor.Enabled = false;
                if ((e.Column.FieldName) == "MODALIDADE")
                    e.Editor.ReadOnly = true;
            }
        }


        protected void grdPerfilModalidade_RowValidating(object sender, ASPxDataValidationEventArgs e)
        {
            this.lblMensagem.Text = string.Empty;

            if (e.NewValues["MODALIDADE_DESCRICAO"] == null)
            {
                e.RowError = "Selecione a Modalidade.";
                return;
            }
        }

        public void Delete(object PERFILMODALIDADEID)
        {
        }

        public void Insert(object MODALIDADE_DESCRICAO)
        {
        }

        protected void odsPadraoPerfil_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var id = e.InputParameters["PERFILMODALIDADEID"].ToString();
           
            RN.PerfilModalidade.Remover(int.Parse(id));
           
        }

        protected void odsPadraoPerfil_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {

            var perfil = new RN.Entidades.PerfilModalidade
            {
                ModalidadeId = e.InputParameters["MODALIDADE_DESCRICAO"].ToString(),
                PerfilId = Convert.ToInt32(tsePerfil.DBValue),
                Matricula = this.User.Identity.Name

            };

            var validacao = RN.PerfilModalidade.Validar(perfil);

            if (validacao.Valido)
            {
                RN.PerfilModalidade.Inserir(perfil);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }
    }
}
