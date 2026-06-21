using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using System.Collections.Specialized;

namespace Techne.Lyceum.Net.InspecaoEscolar
{
    [NavUrl("~/InspecaoEscolar/Grupo.aspx"),
  ControlText("Grupo"),
  Title("Grupo"),]

    public partial class Grupo : TPage
    {
        public void Insert(object CAMPANHAID, object DESCRICAO, object ORDEM) { }
        public void Update(object GRUPOID, object DESCRICAO, object ORDEM, object CAMPANHAID) { }
        public void Delete(object GRUPOID) { }


        private readonly RN.InspecaoEscolar.Campanha CampanhaRN;
        private readonly RN.InspecaoEscolar.Grupo GrupoRN;
        private readonly RN.InspecaoEscolar.TipoAssunto TipoAssuntoRN;

        private RN.InspecaoEscolar.Entidades.Grupo GrupoDados;
        private RN.InspecaoEscolar.Entidades.Assunto AssuntoDados;

        private ValidacaoDados validacao;

        public Grupo()
        {
            CampanhaRN = new Techne.Lyceum.RN.InspecaoEscolar.Campanha();
            GrupoRN = new Techne.Lyceum.RN.InspecaoEscolar.Grupo();
            TipoAssuntoRN = new Techne.Lyceum.RN.InspecaoEscolar.TipoAssunto();

            GrupoDados = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.Grupo();
            AssuntoDados = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.Assunto();

            validacao = new ValidacaoDados();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            { }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                    
            }

        }

        public object ListaCampanha() // será chamado pelo ods para listar as campanhas
        {
            return CampanhaRN.ListarCampanha_Grupo();
        }


        public object ListaGrupo()
        {
            return GrupoRN.ListarGrupo();
        }

        public object ListaAssunto()
        {
            return TipoAssuntoRN.ListarTipoAssunto();
        }


        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdGrupo, "Grupo");

        }


        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdGrupo);
        }

        protected void grdGrupo_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdGrupo.Settings.ShowFilterRow = false;
        }

        protected void grdGrupo_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdGrupo.Settings.ShowFilterRow = false;
        }

        protected void grdGrupo_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdGrupo.IsNewRowEditing) // regra para não poder editar
            {
                if ((e.Column.FieldName) == "GRUPOID")
                {
                    e.Editor.ReadOnly = false;
                }

            }
            else if (grdGrupo.IsEditing)
            {
                if ((e.Column.FieldName) == "GRUPOID")
                {
                    e.Editor.ReadOnly = true;
                }
            }
        }

        protected void grdGrupo_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {

            try
            {
                PegaDadosnaTela(e.NewValues);

                validacao = GrupoRN.ValidaInsercaoAtualizacaoGrupo(GrupoDados);

                if (validacao.Valido)
                {
                    GrupoRN.Insere(GrupoDados);
                    grdGrupo.DataBind();

                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        protected void grdGrupo_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                int grupoid = Convert.ToInt32(e.Keys["GRUPOID"]);

                PegaDadosnaTela(e.NewValues, grupoid);
                validacao = GrupoRN.ValidaAtualizacaoGrupo(GrupoDados);

                if (validacao.Valido)
                {
                    GrupoRN.Atualiza(GrupoDados);
                    grdGrupo.DataBind();

                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void grdGrupo_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {

                int grupoID, campanhaID;

                grupoID = Convert.ToInt32(e.Values["GRUPOID"]);
                campanhaID = Convert.ToInt32(e.Values["CAMPANHAID"]);
                validacao = GrupoRN.ValidaRemocaoGrupo(grupoID);


                if (validacao.Valido)
                {
                    if (!(grupoID == null) || !(campanhaID == null))
                    {
                        GrupoRN.Remover(grupoID, campanhaID);
                        grdGrupo.DataBind();
                    }
                    else
                    {
                        e.Cancel = true;
                        new Exception("O grupo ou a campanha, precisam ser selecionados.");
                    }

                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region Utilidades

        private void PegaDadosnaTela(OrderedDictionary od, int grupoid)
        {
            GrupoDados.Descricao = od["DESCRICAO"] != null ? od["DESCRICAO"].ToString() : string.Empty;
            GrupoDados.Ordem = od["ORDEM"] != null ? Convert.ToInt32(od["ORDEM"]) : 0;
            GrupoDados.CampanhaId = od["CAMPANHAID"] != null ? Convert.ToInt32(od["CAMPANHAID"]) : 0;
            GrupoDados.UsuarioId = User.Identity.Name;
            GrupoDados.GrupoId = grupoid;

        }
        private void PegaDadosnaTela(OrderedDictionary od)
        {
            GrupoDados.Descricao = od["DESCRICAO"] != null ? od["DESCRICAO"].ToString() : string.Empty;
            GrupoDados.Ordem = od["ORDEM"] != null ? Convert.ToInt32(od["ORDEM"]) : 0;
            GrupoDados.CampanhaId = od["CAMPANHAID"] != null ? Convert.ToInt32(od["CAMPANHAID"]) : 0;
            GrupoDados.UsuarioId = User.Identity.Name;
            GrupoDados.GrupoId = od["GRUPOID"] != null ? Convert.ToInt32(od["GRUPOID"]) : 0;
        }
        #endregion
    }


}
