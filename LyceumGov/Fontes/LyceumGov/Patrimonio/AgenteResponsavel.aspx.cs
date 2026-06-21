using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using System.Data;
using Techne.Web;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Patrimonio
{
    [NavUrl("~/Patrimonio/AgenteResponsavel.aspx"),
ControlText("Designar Agente Responsável"),
Title("Designar Agente Responsável"),]

    public partial class AgenteResponsavel : TPage
    {
        //public object Lista(object setor)
        //{
        //    RN.Patrimonio.AgenteResponsavel rnAgenteResponsavel = new Techne.Lyceum.RN.Patrimonio.AgenteResponsavel();

        //    if (!string.IsNullOrEmpty(setor.ToString()))
        //    {
        //        return rnAgenteResponsavel.ListaPor(setor.ToString());
        //    }
        //    return null;
        //}

        public object Lista(object setor)
        {
            RN.Patrimonio.AgenteResponsavel rnAgenteResponsavel = new Techne.Lyceum.RN.Patrimonio.AgenteResponsavel();
            RN.Setores rnSetor = new Setores();

            if (!string.IsNullOrEmpty(setor.ToString()))
            {
                string codigoSetor = rnSetor.ObtemSetorPor(setor.ToString());
                return rnAgenteResponsavel.ListaPor(codigoSetor);
            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdAgente, "Designar Responsável");
        }

        protected void tseServidor_Changed(object sender, EventArgs args)
        {

            TSearch tseServidor = (TSearch)grdAgente.FindEditFormTemplateControl("tseServidor");
            if (!tseServidor.DBValue.IsNull)
            {
                if (tseServidor.IsValidDBValue)
                {

                    DevExpress.Web.ASPxEditors.ASPxLabel lblFuncao = grdAgente.FindEditFormTemplateControl("lblFuncao") as DevExpress.Web.ASPxEditors.ASPxLabel;

                    lblFuncao.Text = string.Empty;

                    if (tseServidor != null)
                    {
                        lblFuncao.Text = tseServidor["funcaodesc"].ToString();
                    }
                }
                else
                {
                    lblMensagem.Text = "Matrícula não encontrada.";
                    tseServidor.DBValue = string.Empty;
                    tseServidor.Text = string.Empty;

                }
            }
        }
        protected void tseUnidadeAdministrativa_Changed(object sender, EventArgs args)
        {
            if (this.Page.IsCallback)
            {
                return;
            }

            try
            {
                grdAgente.Visible = false;
                if (!this.tseUnidadeAdministrativa.DBValue.IsNull)
                {
                    if (this.tseUnidadeAdministrativa.IsValidDBValue)
                    {
                        grdAgente.Visible = true;
                    }
                    else
                    {
                        lblMensagem.Text = "Unidade administrativa não cadastrada.";

                    }
                }
                else
                {
                    lblMensagem.Text = "Unidade administrativa não cadastrada.";

                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public void Delete(object AGENTERESPONSAVELID) { }


        public void Insert(object DATAPUBLICACAONOMEACAO, object DATADISPENSA, object DATAPUBLICACAODISPENSA, object MATRICULA, object DATANOMEACAO) { }

        public void Insert(object DATANOMEACAO, object DATAPUBLICACAONOMEACAO, object DATADISPENSA, object DATAPUBLICACAODISPENSA, object MATRICULA, object FUNCAODESCRICAO) { }

        public void Update(object DATAPUBLICACAONOMEACAO, object DATADISPENSA, object DATAPUBLICACAODISPENSA, object MATRICULA, object DATANOMEACAO, object AGENTERESPONSAVELID) { }

        public void Update(object DATANOMEACAO, object DATAPUBLICACAONOMEACAO, object DATADISPENSA, object DATAPUBLICACAODISPENSA, object MATRICULA, object FUNCAODESCRICAO, object AGENTERESPONSAVELID) { }

        protected void grdAgente_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAgente);
        }
        protected void grdAgente_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            tseUnidadeAdministrativa.Enabled = true;
        }
     
        protected void grdAgente_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Patrimonio.AgenteResponsavel rnAgenteResponsavel = new Techne.Lyceum.RN.Patrimonio.AgenteResponsavel();
                lblMensagem.Text = string.Empty;

                rnAgenteResponsavel.Remove(Convert.ToInt32(e.Keys["AGENTERESPONSAVELID"]));
                grdAgente.DataBind();
                tseUnidadeAdministrativa.Enabled = true;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }
        protected void grdAgente_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Patrimonio.Entidades.AgenteResponsavel agente = new Techne.Lyceum.RN.Patrimonio.Entidades.AgenteResponsavel();
                RN.Patrimonio.AgenteResponsavel rnAgenteResponsavel = new Techne.Lyceum.RN.Patrimonio.AgenteResponsavel();

                agente.Matricula = e.NewValues["MATRICULA"] != null ? e.NewValues["MATRICULA"].ToString().Trim().ToUpper() : null;
                agente.DataNomeacao = e.NewValues["DATANOMEACAO"] != null ? Convert.ToDateTime(e.NewValues["DATANOMEACAO"]) : DateTime.MinValue;
                agente.DataPublicacaoNomeacao = e.NewValues["DATAPUBLICACAONOMEACAO"] != null ? Convert.ToDateTime(e.NewValues["DATAPUBLICACAONOMEACAO"]) : (DateTime?)null;
                agente.UsuarioId = User.Identity.Name;
                //agente.Setor = !tseUnidadeAdministrativa.DBValue.IsNull && tseUnidadeAdministrativa.IsValidDBValue ? tseUnidadeAdministrativa.DBValue.ToString() : null;
                agente.Setor = !tseUnidadeAdministrativa.DBValue.IsNull && tseUnidadeAdministrativa.IsValidDBValue ? tseUnidadeAdministrativa["setor"].ToString() : null;
                
                validacao = rnAgenteResponsavel.Valida(agente, true);

                if (validacao.Valido)
                {
                    rnAgenteResponsavel.Insere(agente);
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }

                grdAgente.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        
        }

        protected void grdAgente_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Patrimonio.Entidades.AgenteResponsavel agente = new Techne.Lyceum.RN.Patrimonio.Entidades.AgenteResponsavel();
                RN.Patrimonio.AgenteResponsavel rnAgenteResponsavel = new Techne.Lyceum.RN.Patrimonio.AgenteResponsavel();

                agente.Matricula = e.NewValues["MATRICULA"] != null ? e.NewValues["MATRICULA"].ToString().Trim().ToUpper() : null;
                agente.DataNomeacao = e.NewValues["DATANOMEACAO"] != null ? Convert.ToDateTime(e.NewValues["DATANOMEACAO"]) : DateTime.MinValue;
                agente.DataPublicacaoNomeacao = e.NewValues["DATAPUBLICACAONOMEACAO"] != null ? Convert.ToDateTime(e.NewValues["DATAPUBLICACAONOMEACAO"]) : (DateTime?)null;
                agente.DataDispensa = e.NewValues["DATADISPENSA"] != null ? Convert.ToDateTime(e.NewValues["DATADISPENSA"]) : (DateTime?)null;
                agente.DataPublicacaoDispensa = e.NewValues["DATAPUBLICACAODISPENSA"] != null ? Convert.ToDateTime(e.NewValues["DATAPUBLICACAODISPENSA"]) : (DateTime?)null;
                agente.UsuarioId = User.Identity.Name;
                agente.Setor = !tseUnidadeAdministrativa.DBValue.IsNull && tseUnidadeAdministrativa.IsValidDBValue ? tseUnidadeAdministrativa["setor"].ToString() : null;
                agente.AgenteResponsavelId = Convert.ToInt32(e.Keys["AGENTERESPONSAVELID"]);

                validacao = rnAgenteResponsavel.Valida(agente, false);

                if (validacao.Valido)
                {
                    rnAgenteResponsavel.Atualiza(agente);
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }

                grdAgente.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        
        }

        protected void grdAgente_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdAgente.Settings.ShowFilterRow = false;
        }

        protected void grdAgente_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (grdAgente.IsEditing && !grdAgente.IsNewRowEditing)
            {
                string matricula = (string)grdAgente.GetRowValues(e.VisibleIndex, "MATRICULA");
                TSearch tseServidor = (TSearch)grdAgente.FindEditFormTemplateControl("tseServidor");
                if (tseServidor != null)
                {
                    tseServidor.Enabled = false;
                    tseServidor.ReadOnly = true;
                }
            }
        }

        protected void grdAgente_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdAgente.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "DATADISPENSA")
                {
                    e.Editor.ClientEnabled = false;
                }
                
                if ((e.Column.FieldName) == "DATAPUBLICACAODISPENSA")
                {
                    e.Editor.ClientEnabled = false;
                }
            }
            else if (grdAgente.IsEditing)
            {                
                //if ((e.Column.FieldName) == "DATANOMEACAO")
                //{                    
                //    e.Editor.ClientEnabled = false;
                //}

                //if ((e.Column.FieldName) == "DATAPUBLICACAONOMEACAO")
                //{
                //    e.Editor.ClientEnabled = false;
                //}
            }
        }
    }
}
