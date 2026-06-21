using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using System.Collections.Specialized;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using System.Data;
using Seeduc.Infra.Helpers;

namespace Techne.Lyceum.Net.InspecaoEscolar
{
    [NavUrl("~/InspecaoEscolar/Assunto.aspx"),
    ControlText("Assuntoo"),
    Title("Assunto"),]

    public partial class Assunto : TPage
    {
        public void Insert(object ASSUNTO, object ORDEM, object TIPOASSUNTOID, object ACAODEDIRECAO) { }
        public void Insert(object ASSUNTO, object ORDEM, object TIPOASSUNTOID, object ACAODEDIRECAO, object RESTRICAO) { }

        public void Update(object ASSUNTO, object ORDEM, object TIPOASSUNTOID, object ACAODEDIRECAO, object RESTRICAO, object ASSUNTOID) { }

        public void Delete(object ASSUNTOID) { }

        private readonly RN.InspecaoEscolar.Campanha CampanhaRN;
        private readonly RN.InspecaoEscolar.Grupo GrupoRN;
        private readonly RN.InspecaoEscolar.TipoAssunto TipoAssuntoRN;
        private readonly RN.InspecaoEscolar.Assunto AssuntoRN;

        private RN.InspecaoEscolar.Entidades.Assunto AssuntoDados;


        private ValidacaoDados validacao;


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ListarAno();
                    DropDownList[] dropdown = new DropDownList[] { ddlSemestre, ddlTituloCampanha, ddlGrupo };
                    LimpaDdl(dropdown);

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdAssunto);
        }

        #region DataSource

        public Assunto()
        {
            CampanhaRN = new Techne.Lyceum.RN.InspecaoEscolar.Campanha();
            GrupoRN = new Techne.Lyceum.RN.InspecaoEscolar.Grupo();
            AssuntoRN = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();
            TipoAssuntoRN = new Techne.Lyceum.RN.InspecaoEscolar.TipoAssunto();

            AssuntoDados = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.Assunto();

            validacao = new ValidacaoDados();

        }

        public object ListaTipoAssunto()
        {
            var lista = TipoAssuntoRN.ListarTipoAssunto();

            DataRow newRow = lista.NewRow();
            newRow["TIPOASSUNTO"] = "Selecione";
            newRow["TIPOASSUNTOID"] = -1;
            lista.Rows.InsertAt(newRow, 0);

            return lista;


        }

        public object ListaAssuntoPai(int grupoID)
        {           
            var lista = AssuntoRN.ListarAssuntoPai(grupoID);

            DataRow newRow = lista.NewRow();
            newRow["ASSUNTOPAI"] = "Vazio";
            newRow["ASSUNTOPAIID"] = -1;
            lista.Rows.InsertAt(newRow, 0);
            return lista;
        }

        #endregion

        #region Métodos_Grid

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdAssunto, "Assunto");

        }
        protected void grdAssunto_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAssunto);
        }
      
        protected void grdAssunto_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdAssunto.Settings.ShowFilterRow = false;
        }

        protected void grdAssunto_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdAssunto.Settings.ShowFilterRow = false;
        }

        protected void grdAssunto_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "RESTRICAO")
            {
                var grid = (DevExpress.Web.ASPxGridView.ASPxGridView)sender;

                if (grid.IsNewRowEditing)
                {
                    e.Editor.ClientVisible = false;
                    return;
                }

                object tipoAssunto = grid.GetRowValues(grid.EditingRowVisibleIndex, "TIPOASSUNTOID");

                if (tipoAssunto == null || Convert.ToInt32(tipoAssunto) != 4)
                {
                    e.Editor.ClientVisible = false;
                }
            }
        }

        protected void grdAssunto_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            //if (!this.grdAssunto.Visible
            //    || this.grdAssunto.VisibleRowCount == 0)
            //{
            //    return;
            //}
            //var tipoAssunto = (string)grdAssunto.GetRowValues(e.VisibleIndex, "TIPOASSUNTOID");
            //var ddlRestricao = DevExpressHelper.GetControl<DropDownList>(this.grdAssunto, e.VisibleIndex, "RESTRICAO", "ddlRETRICAO");

            //ddlRestricao.Visible = false;

            ////Verifica se é descritivo
            //if (tipoAssunto == "4")
            //{
            //    ddlRestricao.Visible = true;
            //}
        }

        protected void grdAssunto_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "RESTRICAO")
            {
                object tipoAssunto = e.GetValue("TIPOASSUNTOID");

                if (tipoAssunto != null && Convert.ToInt32(tipoAssunto) != 4)
                {
                    e.Cell.Text = ""; 
                    e.Cell.Visible = false; 
                }
            }
        }

        protected void grdAssunto_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                int assuntoId = Convert.ToInt32(e.Keys["ASSUNTOID"]);


                if (assuntoId == 0)
                { throw new Exception("Erro ao obter o ASSUNTO."); }


                validacao = AssuntoRN.ValidaRemocaoAssunto(assuntoId);

                if (validacao.Valido)
                {
                    AssuntoRN.Remover(assuntoId);
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

        protected void grdAssunto_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {

            try
            {
                int campanhaid = Convert.ToInt32(ddlTituloCampanha.SelectedValue);
                int assuntoId = 0;
                int grupoId = Convert.ToInt32(ddlGrupo.SelectedIndex > 0 ? ddlGrupo.SelectedValue : "0");

                if (grupoId == 0)
                { throw new Exception("Erro ao obter o Grupo."); }

                //pegar os dados do panel
                PegaDadosnaTela(e.NewValues, assuntoId, grupoId);

                validacao = AssuntoRN.Valida(AssuntoDados, Convert.ToInt32(ddlGrupo.SelectedItem.Value), campanhaid);
                //validar os dados e depois fazer o insert

                if (validacao.Valido)
                {
                    AssuntoRN.Insere(AssuntoDados);
                    
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

        protected void grdAssunto_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                int campanhaid = Convert.ToInt32(ddlTituloCampanha.SelectedValue);
                int assuntoId = Convert.ToInt32(e.Keys["ASSUNTOID"]);
                int grupoId = Convert.ToInt32(ddlGrupo.SelectedIndex > 0 ? ddlGrupo.SelectedValue : "0");

                if (grupoId == 0 || assuntoId == 0)
                { throw new Exception("Erro ao obter o Grupo."); }

                PegaDadosnaTela(e.NewValues, assuntoId, grupoId);

                validacao = AssuntoRN.Valida(AssuntoDados, grupoId, campanhaid);

                if (validacao.Valido)
                {
                    AssuntoRN.Atualiza(AssuntoDados);
                 
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

        #endregion


        #region Métodos_Panel

        public void ListarAno()
        {

            ddlAno.DataSource = CampanhaRN.ListarAno();
            ddlAno.DataBind();
            ddlAno.Items.Insert(0, new ListItem("Selecione", "-1"));

        }

        public void ListarSemestre(int ano)
        {
            if (ano == -1)
            {
                DropDownList[] dropdown = new DropDownList[] { ddlSemestre, ddlTituloCampanha, ddlGrupo };
                LimpaDdl(dropdown);
            }
            else
            {
                ddlSemestre.DataSource = CampanhaRN.ListarSemestreporAno(ano);
                ddlSemestre.DataBind();
                ddlSemestre.Items.Insert(0, new ListItem("Selecione", "-1"));
            }


        }

        public void ListarCampanhaporSemestreAno(int ano, int semestre)
        {
            if (ano == -1 || semestre == -1)
            {
                DropDownList[] dropdown = new DropDownList[] { ddlTituloCampanha, ddlGrupo };
                LimpaDdl(dropdown);
            }
            else
            {
                ddlTituloCampanha.DataSource = CampanhaRN.ListarCampanha(ano, semestre);
                ddlTituloCampanha.DataBind();
                ddlTituloCampanha.Items.Insert(0, new ListItem("Selecione", "-1"));
            }

        }

        public void ListaGrupoporCampanha(int campanhaId)
        {

            if (campanhaId == -1)
            {
                DropDownList[] dropdown = new DropDownList[] { ddlGrupo };
                LimpaDdl(dropdown);
            }

            else
            {

                ddlGrupo.DataSource = GrupoRN.ListarGrupoporCampanha(campanhaId);
                ddlGrupo.DataBind();
                ddlGrupo.Items.Insert(0, new ListItem("Selecione", "-1"));
            }           
        }

        public object ListaAssunto(int grupoId)
        {
            DataTable assunto = new DataTable();
            assunto = null;
            try
            {             
                if (grupoId == -1)
                {                   
                    return assunto;
                }
                else
                {
                    assunto = AssuntoRN.ListarAssunto(grupoId);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);                
            }
            return assunto;
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListarSemestre(Convert.ToInt32(ddlAno.SelectedItem.Value));

            DropDownList[] dropdown = new DropDownList[] { ddlTituloCampanha, ddlGrupo };
            LimpaDdl(dropdown);
        }

        protected void ddlSemestre_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListarCampanhaporSemestreAno(Convert.ToInt32(ddlAno.SelectedValue),
           Convert.ToInt32(ddlSemestre.SelectedValue));

        }

        protected void ddlTituloCampanha_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListaGrupoporCampanha(Convert.ToInt32(ddlTituloCampanha.SelectedItem.Value));
        }

        protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {            
            grdAssunto.DataBind();
            grdAssunto.Visible = true;
          
        }

        #endregion


        #region Utilidades


        public void limpaGrid()
        {
            this.grdAssunto.DataSource = null;
            this.grdAssunto.Visible = false;           
        }

        public void LimpaDdl(DropDownList[] ddl)
        {
            // passarei o nome do ddl que

            foreach (var nomeddl in ddl)
            {
                nomeddl.DataSource = null;
                nomeddl.DataBind();
                nomeddl.Items.Clear();
              
            }
            limpaGrid();

        }

        protected bool VerificarCheck(object valor)
        {
            if (valor is DBNull)
            {
                return false;
            }

            if (valor is string)
            {
                return (string)valor == "1";
            }

            if (valor is bool)
            {
                return (bool)valor;
            }
            return false;
        }

        protected bool VerificarAssuntoPai(object valor)
        {
            if (valor is DBNull)
            {
                return false;
            }

            if (valor is string)
            {
                return (string)valor == "1";
            }

            if (valor is bool)
            {
                return (bool)valor;
            }
            return false;
        }

        private void PegaDadosnaTela(OrderedDictionary od, int assuntoId, int grupoID)
        {
            try
            {
                ASPxComboBox IDPAIASSUNTOID;

                AssuntoDados.AssuntoId = assuntoId;
                AssuntoDados.Descricao = od["ASSUNTO"] != null ? od["ASSUNTO"].ToString() : string.Empty;
                AssuntoDados.Ordem = od["ORDEM"] != null ? Convert.ToInt32(od["ORDEM"]) : (int?)null;
                AssuntoDados.GrupoId = grupoID; 
                AssuntoDados.TipoAssuntoId = Convert.ToInt32((od["TIPOASSUNTOID"] ?? "0").ToString());
                AssuntoDados.Restricao = Convert.ToInt32((od["RESTRICAO"] ?? "0").ToString());

                AssuntoDados.AcaodeDirecao = Convert.ToBoolean(od["ACAODEDIRECAO"]);

                IDPAIASSUNTOID = grdAssunto.FindEditRowCellTemplateControl((GridViewDataColumn)grdAssunto.Columns["PAI_ASSUNTO"], "ddlIDPAIASSUNTO") as ASPxComboBox;

                if (IDPAIASSUNTOID.SelectedItem == null || IDPAIASSUNTOID.SelectedItem.Value.ToString() == "-1")
                {
                    AssuntoDados.IdPaiAssuntoId = null;
                }
                else
                {
                    AssuntoDados.IdPaiAssuntoId = Convert.ToInt32(IDPAIASSUNTOID.SelectedItem.Value);


                }

                AssuntoDados.UsuarioId = User.Identity.Name;
                
                var chkEntrega = DevExpressHelper.GetControl<CheckBox>(this.grdAssunto, 0, "ACAODEDIRECAO", "chkacaodedirecao");
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " PegaDadosnaTela ");
            }          

        }

        #endregion
    }
}
