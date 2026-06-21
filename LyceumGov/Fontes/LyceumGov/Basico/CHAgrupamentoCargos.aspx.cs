using System;
using DevExpress.Web.ASPxGridView;
using Techne.Web;
using System.Web.UI;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/CHAgrupamentoCargos.aspx"),
      ControlText("CHAgrupamentoCargos"),
      Title("CH Agrupamento dos Cargos do Docente"),]
    public partial class CHAgrupamentoCargos : TPage
    {
        public object Lista()
        {
            RN.RecursosHumanos.ChAgrupamentoCargo rnChAgrupamentoCargo = new Techne.Lyceum.RN.RecursosHumanos.ChAgrupamentoCargo();

            return rnChAgrupamentoCargo.Lista();

        }
        public object ListaGrupo()
        {
            RN.RecursosHumanos.AgrupamentoCargos rnAgrupamentoCargos = new Techne.Lyceum.RN.RecursosHumanos.AgrupamentoCargos();

            return rnAgrupamentoCargos.ListaAtivo();

        }
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdCHAgrupamento, string.Empty);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdCHAgrupamento);
        }

        protected void grdCHAgrupamento_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdCHAgrupamento.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "CHAVE")
                {
                    e.Editor.Enabled = true;
                }
            }
            else if (grdCHAgrupamento.IsEditing)
            {
                if ((e.Column.FieldName) == "CHAVE")
                {
                    e.Editor.Enabled = false;
                }

                if ((e.Column.FieldName) == "CARGAHORARIAGRUPO")
                {
                    e.Editor.Enabled = false;
                }               
            }
        }

        protected void grdCHAgrupamento_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdCHAgrupamento);
        }

        protected void grdCHAgrupamento_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdCHAgrupamento.Settings.ShowFilterRow = false;
        }

        protected void grdCHAgrupamento_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdCHAgrupamento.Settings.ShowFilterRow = false;
        }

        protected void grdCHAgrupamento_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.Entidades.ChAgrupamentoCargo chAgrupamento = new Techne.Lyceum.RN.RecursosHumanos.Entidades.ChAgrupamentoCargo();
            RN.RecursosHumanos.ChAgrupamentoCargo rnChAgrupamentoCargo = new Techne.Lyceum.RN.RecursosHumanos.ChAgrupamentoCargo();
            int chGrupo = 0;

            chAgrupamento.AgrupamentoCargosId = e.NewValues["CHAVE"] != null ? Convert.ToInt32(e.NewValues["CHAVE"].ToString().Split('_')[0]) : -1;
            chAgrupamento.CargaHorariaComplementacao = e.NewValues["CARGAHORARIACOMPLEMENTACAO"] != null ? Convert.ToInt32(e.NewValues["CARGAHORARIACOMPLEMENTACAO"]) : -1;
            chAgrupamento.CargaHorariaPlanejamento = e.NewValues["CARGAHORARIAPLANEJAMENTO"] != null ? Convert.ToInt32(e.NewValues["CARGAHORARIAPLANEJAMENTO"]) : -1;
            chAgrupamento.CargaHorariaRegencia = e.NewValues["CARGAHORARIAREGENCIA"] != null ? Convert.ToInt32(e.NewValues["CARGAHORARIAREGENCIA"]) : -1;
            chAgrupamento.Funcao = e.NewValues["FUNCAO"] != null ? e.NewValues["FUNCAO"].ToString().Trim() : null;
            chAgrupamento.UsuarioId = User.Identity.Name;
            chGrupo = e.NewValues["CHAVE"] != null ? Convert.ToInt32(e.NewValues["CHAVE"].ToString().Split('_')[1]) : -1;

            validacao = rnChAgrupamentoCargo.Valida(chAgrupamento, chGrupo, true);

            if (validacao.Valido)
            {
                rnChAgrupamentoCargo.Insere(chAgrupamento);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdCHAgrupamento.DataBind();

        }

        protected void grdCHAgrupamento_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.Entidades.ChAgrupamentoCargo chAgrupamento = new Techne.Lyceum.RN.RecursosHumanos.Entidades.ChAgrupamentoCargo();
            RN.RecursosHumanos.ChAgrupamentoCargo rnChAgrupamentoCargo = new Techne.Lyceum.RN.RecursosHumanos.ChAgrupamentoCargo();
            int chGrupo = 0;

            chAgrupamento.CargaHorariaComplementacao = e.NewValues["CARGAHORARIACOMPLEMENTACAO"] != null ? Convert.ToInt32(e.NewValues["CARGAHORARIACOMPLEMENTACAO"]) : -1;
            chAgrupamento.CargaHorariaPlanejamento = e.NewValues["CARGAHORARIAPLANEJAMENTO"] != null ? Convert.ToInt32(e.NewValues["CARGAHORARIAPLANEJAMENTO"]) : -1;
            chAgrupamento.CargaHorariaRegencia = e.NewValues["CARGAHORARIAREGENCIA"] != null ? Convert.ToInt32(e.NewValues["CARGAHORARIAREGENCIA"]) : -1;
            chAgrupamento.Funcao = e.NewValues["FUNCAO"] != null ? e.NewValues["FUNCAO"].ToString().Trim() : null;
            chAgrupamento.UsuarioId = User.Identity.Name;
            chAgrupamento.ChAgrupamentoCargoId = Convert.ToInt32(e.Keys["CH_AGRUPAMENTOCARGOID"]);
            var chave = grdCHAgrupamento.GetRowValuesByKeyValue(e.Keys[0], "CHAVE");
            chAgrupamento.AgrupamentoCargosId = chave != null ? Convert.ToInt32(chave.ToString().Split('_')[0]) : -1;
            chGrupo = Convert.ToInt32(chave.ToString().Split('_')[1]);

            
            validacao = rnChAgrupamentoCargo.Valida(chAgrupamento, chGrupo, false);

            if (validacao.Valido)
            {
                rnChAgrupamentoCargo.Atualiza(chAgrupamento);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdCHAgrupamento.DataBind();
        }

        protected void grdCHAgrupamento_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.ChAgrupamentoCargo rnChAgrupamentoCargo = new Techne.Lyceum.RN.RecursosHumanos.ChAgrupamentoCargo();
            int chAgrupamentoId = 0;

            chAgrupamentoId = Convert.ToInt32(e.Keys["CH_AGRUPAMENTOCARGOID"]);

            validacao = rnChAgrupamentoCargo.ValidaRemocao(chAgrupamentoId);

            if (validacao.Valido)
            {
                rnChAgrupamentoCargo.Remove(chAgrupamentoId);
                grdCHAgrupamento.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }


        public void Insert(object CHAVE, object CARGAHORARIACOMPLEMENTACAO, object CARGAHORARIAREGENCIA, object CARGAHORARIAPLANEJAMENTO, object TOTAL, object FUNCAO)
        { }
        

        public void Update(object CARGAHORARIACOMPLEMENTACAO, object CARGAHORARIAREGENCIA, object CARGAHORARIAPLANEJAMENTO, object TOTAL, object FUNCAO, object CH_AGRUPAMENTOCARGOID)
        { }

        public void Delete(object CH_AGRUPAMENTOCARGOID)
        { }
    }
}

