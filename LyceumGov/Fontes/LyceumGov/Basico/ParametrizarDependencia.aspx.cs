using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/ParametrizarDependencia.aspx"), ControlText("ParametrizarDependencia"), Title("Parametrizar Dependencia")]
    public partial class ParametrizarDependencia : TPage
    {
        //estacia as propriedades da camada RN
        RN.ParametrizarDependencia Propriedades = new RN.ParametrizarDependencia();
        protected void Page_Load(object sender, EventArgs e)
        {
            string table = string.Empty;
            Techne.Library.Sql.Structure.SqlSelectColumns coluna = new Techne.Library.Sql.Structure.SqlSelectColumns();
            table = "ly_curso";
            coluna.Add("curso");
            coluna.Add("nome");
            Techne.Library.Sql.Structure.SqlSelect sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);
            tseUnidade_Ensino_Destino.Argument = "nome";
            tseUnidade_Ensino_Destino.Key = "curso";
            tseUnidade_Ensino_Destino.SqlOrder = "nome";
            tseUnidade_Ensino_Destino.SqlSelect = sqlSelect;
            tseUnidade_Ensino_Destino.SqlWhere = "";
            tseUnidade_Ensino_Destino.DataBind();
            tseUnidade_Ensino_Destino.Enabled = true;
            grdTipoDependencia.Enabled = false;
            if(tseUnidade_Ensino_Destino.Value != null)
            {
                grdTipoDependencia.Enabled = true;
            }
        }
        
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdTipoDependencia, "Tipo Dependência");
        }

        protected void grdTipoDependencia_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["IDCURSO"] = tseUnidade_Ensino_Destino.Value;
        }

        protected void grdTipoDependencia_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {

            if (grdTipoDependencia.IsNewRowEditing)
            {
                if (e.Column.FieldName == "LYTIPODEPENDENCIAID")
                    e.Editor.Enabled = true;
            }
            else
            {
                if (grdTipoDependencia.IsEditing)
                {

                    if (e.Column.FieldName == "LYTIPODEPENDENCIAID")
                        e.Editor.Enabled = true;
               }
            }
        }

        protected void grdTipoDependencia_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string idcurso = Convert.ToString(e.GetListSourceFieldValue("IDCURSO"));
                string dependencia = Convert.ToString(e.GetListSourceFieldValue("LYTIPODEPENDENCIAID"));
                e.Value = idcurso + "/" + dependencia;
            }
        }

        protected void ObTipoDependencia_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var Tipodepend = new LyCursoLyTipoDependencia
            {
                ID_CURSO = Convert.ToString(tseUnidade_Ensino_Destino.Value),
                DEPENDENCIA = Convert.ToString(e.InputParameters["LYTIPODEPENDENCIAID"]),
            };

            RN.ParametrizarDependencia.Inserir(Tipodepend);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('Operação realizada com sucesso!')", true);
        }

        public static void InsertObTipoDependencia(object LYTIPODEPENDENCIAID)
        {

        }

        public object ListarDependenciaPorCurso(object curso)
        {
            var ID = curso.ToString();

            return RN.ParametrizarDependencia.ListarDependenciaPorCurso(ID);
        }
        
        protected void grdTipoDependencia_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
           
        }

        protected void grdTipoDependencia_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Keys.Clear();
            e.Keys.Add("IDCURSO", tseUnidade_Ensino_Destino.Value);
            e.Keys.Add("LYTIPODEPENDENCIAID", e.Values["LYTIPODEPENDENCIAID"]);
        }

        protected void ObTipoDependencia_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var sala = new LyCursoLyTipoDependencia
            {

                ID_CURSO = Convert.ToString(tseUnidade_Ensino_Destino.Value),
                DEPENDENCIA = e.InputParameters["LYTIPODEPENDENCIAID"].ToString(),
            };

            var validacao = RN.ParametrizarDependencia.Validar(sala);

            if (validacao.Valido)
            {
                RN.ParametrizarDependencia.Remover(sala.ID_CURSO, sala.DEPENDENCIA);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        public void DeleteObTipoDependencia(object IDCURSO, object LYTIPODEPENDENCIAID)
        {
        }
    }
}
