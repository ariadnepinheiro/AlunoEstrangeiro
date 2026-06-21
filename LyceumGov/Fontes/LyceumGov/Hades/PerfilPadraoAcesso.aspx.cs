using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Web;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.Net.Hades
{
    [
    NavUrl("~/Hades/PerfilPadraoAcesso.aspx"),
    ControlText("PerfilPadraoAcesso"),
    Title("Perfil de Acesso"),
]
    public partial class PerfilPadraoAcesso : TPage
    {
        public object Listar()
        {
               return RN.Perfil.Listar();

        }
        protected void Page_Init(object sender, EventArgs e)
        {

            TituloGrid(grdPerfil, "Perfil de Acesso");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;

            if (!IsPostBack)
            { }
        }
        protected void grdPerfil_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdPerfil);
        }
        protected void grdPerfil_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdPerfil.Settings.ShowFilterRow = false;

        }

        protected void grdPerfil_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdPerfil.Settings.ShowFilterRow = false;
        }
        protected void grdPerfil_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdPerfil.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "id_perfil")
                    e.Editor.Enabled = true;
            }
            else if (grdPerfil.IsEditing)
            {
                if ((e.Column.FieldName) == "id_perfil")
                    e.Editor.Enabled = false;
            }

        }
        public void Insert(object ID_PERFIL,object DESCRICAO)
        {
        }
        public void Update(object ID_PERFIL, object DESCRICAO)
        {
        }
        public void Delete(object ID_PERFIL)
        {
        }
        protected void odsPerfil_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var id = e.InputParameters["ID_PERFIL"].ToString();

            var validacao = RN.Perfil.ValidarRemover(int.Parse(id));

            if (validacao.Valido)
            {
                RN.Perfil.Remover(int.Parse(id));
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void odsPerfil_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            ///retirado
            //var perfil = new TcePerfil
            //                    {
            //                        IdPerfil = int.Parse(e.InputParameters["ID_PERFIL"].ToString()),
            //                        Descricao = e.InputParameters["DESCRICAO"].ToString(),
            //                        Matricula = this.User.Identity.Name
            //                    };

            // var validacao = RN.Perfil.Validar(perfil);

            // if (validacao.Valido)
            // {
            //     RN.Perfil.Alterar(perfil);
            // }
            // else
            // {
            //     throw new Exception(validacao.Mensagem);
            // }

            throw new Exception("Não é possivel editar perfis, apenas adicionar e remover.");
        }
        protected void odsPerfil_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {

            var perfil = new TcePerfil
            {
                Descricao = e.InputParameters["DESCRICAO"].ToString(),
                Matricula = this.User.Identity.Name
            };

            var validacao = RN.Perfil.Validar(perfil);

            if (validacao.Valido)
            {
                RN.Perfil.Inserir(perfil);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }
    }
}
