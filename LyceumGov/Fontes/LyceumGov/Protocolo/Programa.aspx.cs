using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Protocolo
{
    [
           NavUrl("~/Protocolo/Programa.aspx"),
           ControlText("Programa"),
           Title("Programa")
       ]

    public partial class Programa : TPage
    {
        public object ListaPrograma()
        {
            RN.Protocolo.ProgramaProtocolo rnPrograma = new Techne.Lyceum.RN.Protocolo.ProgramaProtocolo();

            return rnPrograma.ListaProgramaProtocolo();
        }

        public object ListaTipoPrograma()
        {
            RN.Protocolo.TipoProtocolo rnTipoProtocolo = new Techne.Lyceum.RN.Protocolo.TipoProtocolo();

            return rnTipoProtocolo.ListaTipoProtocoloAtivo();
        }

        public void Insert(object TIPOPROTOCOLOID, object DESCRICAO, object ATIVO) { }
        public void Update(object TIPOPROTOCOLOID, object DESCRICAO, object ATIVO, object PROGRAMAPROTOCOLOID) { }
        public void Delete(object PROGRAMAPROTOCOLOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdPrograma, "Programa");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdPrograma);
        }

        protected void grdPrograma_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdPrograma);
        }

        protected void grdPrograma_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdPrograma.Settings.ShowFilterRow = false;
        }

        protected void grdPrograma_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdPrograma.Settings.ShowFilterRow = false;
        }

        protected void grdPrograma_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdPrograma.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "TIPOPROTOCOLOID")
                {
                    e.Editor.ReadOnly = false;
                }

            }
            else if (grdPrograma.IsEditing)
            {
                if ((e.Column.FieldName) == "TIPOPROTOCOLOID")
                {
                    e.Editor.ReadOnly = true;
                }
            }
        }

        protected void grdPrograma_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Protocolo.Entidades.ProgramaProtocolo ProgramaProtocolo = new Techne.Lyceum.RN.Protocolo.Entidades.ProgramaProtocolo();
            RN.Protocolo.ProgramaProtocolo rnProgramaProtocolo = new RN.Protocolo.ProgramaProtocolo();

            ProgramaProtocolo.TipoProtocoloId = e.NewValues["TIPOPROTOCOLOID"] != null ? Convert.ToInt32(e.NewValues["TIPOPROTOCOLOID"]) : 0;
            ProgramaProtocolo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            ProgramaProtocolo.Ativo = e.NewValues["ATIVO"] == null ? false : true;
            ProgramaProtocolo.UsuarioId = User.Identity.Name;

            validacao = rnProgramaProtocolo.Valida(ProgramaProtocolo, true);

            if (validacao.Valido)
            {
                rnProgramaProtocolo.Insere(ProgramaProtocolo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdPrograma.DataBind();
        }

        protected void grdPrograma_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Protocolo.Entidades.ProgramaProtocolo ProgramaProtocolo = new Techne.Lyceum.RN.Protocolo.Entidades.ProgramaProtocolo();
            RN.Protocolo.ProgramaProtocolo rnProgramaProtocolo = new RN.Protocolo.ProgramaProtocolo();

            ProgramaProtocolo.TipoProtocoloId = e.NewValues["TIPOPROTOCOLOID"] != null ? Convert.ToInt32(e.NewValues["TIPOPROTOCOLOID"]) : 0;
            ProgramaProtocolo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            ProgramaProtocolo.Ativo = e.NewValues["ATIVO"] == null ? false : Convert.ToBoolean(e.NewValues["ATIVO"]);
            ProgramaProtocolo.ProgramaProtocoloId = Convert.ToInt32(e.Keys["PROGRAMAPROTOCOLOID"]);
            ProgramaProtocolo.UsuarioId = User.Identity.Name;

            validacao = rnProgramaProtocolo.Valida(ProgramaProtocolo, false);

            if (validacao.Valido)
            {
                rnProgramaProtocolo.Atualiza(ProgramaProtocolo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdPrograma.DataBind();
        }

        protected void grdPrograma_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Protocolo.ProgramaProtocolo rnProgramaProtocolo = new RN.Protocolo.ProgramaProtocolo();
            int ProgramaProtocoloId = 0;

            ProgramaProtocoloId = Convert.ToInt32(e.Keys["PROGRAMAPROTOCOLOID"]);

            validacao = rnProgramaProtocolo.ValidaRemocao(ProgramaProtocoloId);

            if (validacao.Valido)
            {
                rnProgramaProtocolo.Remove(ProgramaProtocoloId);
                grdPrograma.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
