using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using System.Data;
using DevExpress.Web.ASPxGridView;
using Techne.Web;
using DevExpress.Web.ASPxTabControl;
using Techne.Controls;

namespace Techne.Lyceum.Net.Interconectividade
{
    [NavUrl("~/Interconectividade/ChamadoAnatel.aspx"), ControlText("Chamado Anatel"), Title("Chamado Anatel")]
    public partial class ChamadoAnatel : TPage
    {
        public object Lista(object circuitoSetor)
        {
            RN.FiscalizacaoLink.ChamadoAnatel rnChamadoAnatel = new Techne.Lyceum.RN.FiscalizacaoLink.ChamadoAnatel();

            if (!string.IsNullOrEmpty(circuitoSetor.ToString()))
            {
                return rnChamadoAnatel.ListaPor(Convert.ToInt32(circuitoSetor));
            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdChamadoAnatel, string.Empty);
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdChamadoAnatel);
        }

        protected void grdChamadoAnatel_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdChamadoAnatel);
        }

        protected void grdChamadoAnatel_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdChamadoAnatel.Settings.ShowFilterRow = false;
        }

        protected void grdChamadoAnatel_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdChamadoAnatel.Settings.ShowFilterRow = false;
        }

        protected void grdChamadoAnatel_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdChamadoAnatel.IsNewRowEditing)
            {
                //if ((e.Column.FieldName) == "CHAMADOANATELID")
                //{
                //    e.Editor.Enabled = true;
                //}
                //if ((e.Column.FieldName) == "NUMEROOPERADORA")
                //{
                //    e.Editor.ReadOnly = false;
                //}
                //if ((e.Column.FieldName) == "DATAOPERADORA")
                //{
                //    e.Editor.ReadOnly = false;
                //}
                //if ((e.Column.FieldName) == "NUMEROANATEL")
                //{
                //    e.Editor.ReadOnly = false;
                //}
                //if ((e.Column.FieldName) == "DATAANATEL")
                //{
                //    e.Editor.ReadOnly = false;
                //}
                //if ((e.Column.FieldName) == "DATARESOLUCAO")
                //{
                //    e.Editor.ReadOnly = false;
                //}
                //if ((e.Column.FieldName) == "SEVERIDADE")
                //{
                //    e.Editor.ReadOnly = false;
                //}
            }
            else if (grdChamadoAnatel.IsEditing)
            {
                if ((e.Column.FieldName) == "CHAMADOANATELID")
                {
                    e.Editor.Enabled = false;
                }
                if ((e.Column.FieldName) == "NUMEROOPERADORA")
                {
                    e.Editor.ReadOnly = false;
                }
                if ((e.Column.FieldName) == "DATAOPERADORA")
                {
                    e.Editor.ReadOnly = false;
                }
                if ((e.Column.FieldName) == "NUMEROANATEL")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.Enabled = false;
                }
                if ((e.Column.FieldName) == "DATAANATEL")
                {
                    e.Editor.ReadOnly = false;
                }
                if ((e.Column.FieldName) == "DATARESOLUCAO")
                {
                    e.Editor.ReadOnly = false;
                }
                if ((e.Column.FieldName) == "SEVERIDADE")
                {
                    e.Editor.ReadOnly = false;
                }
            }
        }
                
        public void Insert(object NUMEROOPERADORA, object DATAOPERADORA, object NUMEROANATEL, object DATAANATEL, object DATARESOLUCAO, object SEVERIDADE)
        {
        }

        public void Update(object CIRCUITOSETORID, object NUMEROOPERADORA, object DATAOPERADORA, object NUMEROANATEL, object DATAANATEL, object DATARESOLUCAO, object SEVERIDADE, object CHAMADOANATELID)
        {
        }

        public void Delete(object CHAMADOANATELID)
        { }

        protected void grdChamadoAnatel_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.Entidades.ChamadoAnatel chamadoAnatel = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.ChamadoAnatel();
            RN.FiscalizacaoLink.ChamadoAnatel rnChamadoAnatel = new Techne.Lyceum.RN.FiscalizacaoLink.ChamadoAnatel();


            chamadoAnatel.CircuitoSetorId = (this.tseCircuito.IsValidDBValue && !this.tseCircuito.DBValue.IsNull) ? Convert.ToInt32(tseCircuito.DBValue) : -1;
            chamadoAnatel.NumeroOperadora = e.NewValues["NUMEROOPERADORA"] != null ? e.NewValues["NUMEROOPERADORA"].ToString() : null;
            chamadoAnatel.DataOperadora = e.NewValues["DATAOPERADORA"] != null ? Convert.ToDateTime(e.NewValues["DATAOPERADORA"]) : DateTime.MinValue;
            chamadoAnatel.NumeroAnatel = e.NewValues["NUMEROANATEL"] != null ? e.NewValues["NUMEROANATEL"].ToString() : null;
            chamadoAnatel.DataAnatel = e.NewValues["DATAANATEL"] != null ? Convert.ToDateTime(e.NewValues["DATAANATEL"]) : DateTime.MinValue;
            chamadoAnatel.DataResolucao = e.NewValues["DATARESOLUCAO"] != null ? Convert.ToDateTime(e.NewValues["DATARESOLUCAO"]) : (DateTime?)null;
            chamadoAnatel.Severidade = e.NewValues["SEVERIDADE"] != null ? e.NewValues["SEVERIDADE"].ToString() : null;
            chamadoAnatel.UsuarioId = User.Identity.Name;


            validacao = rnChamadoAnatel.Valida(chamadoAnatel, true);

            if (validacao.Valido)
            {
                rnChamadoAnatel.Insere(chamadoAnatel);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdChamadoAnatel.DataBind();

        }

        protected void grdChamadoAnatel_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {

            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.ChamadoAnatel rnChamadoAnatel = new Techne.Lyceum.RN.FiscalizacaoLink.ChamadoAnatel();

            var chamadoId = ((ASPxGridView)sender).GetRowValuesByKeyValue(e.Keys[0], "CHAMADOANATELID");
            var circuitoSetorId = ((ASPxGridView)sender).GetRowValuesByKeyValue(e.Keys[0], "CIRCUITOSETORID");
            var numeroAnatel = ((ASPxGridView)sender).GetRowValuesByKeyValue(e.Keys[0], "NUMEROANATEL");

            RN.FiscalizacaoLink.Entidades.ChamadoAnatel chamadoAnatel = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.ChamadoAnatel
            {
                ChamadoAnatelId = Convert.ToInt32(chamadoId),
                CircuitoSetorId = Convert.ToInt32(circuitoSetorId),
                NumeroOperadora = e.NewValues["NUMEROOPERADORA"] != null ? e.NewValues["NUMEROOPERADORA"].ToString() : null,
                DataOperadora = e.NewValues["DATAOPERADORA"] != null ? Convert.ToDateTime(e.NewValues["DATAOPERADORA"]) : DateTime.MinValue,
                NumeroAnatel = numeroAnatel.ToString(),
                DataAnatel = e.NewValues["DATAANATEL"] != null ? Convert.ToDateTime(e.NewValues["DATAANATEL"]) : DateTime.MinValue,
                DataResolucao = e.NewValues["DATARESOLUCAO"] != null ? Convert.ToDateTime(e.NewValues["DATARESOLUCAO"]) : (DateTime?)null,
                Severidade = e.NewValues["SEVERIDADE"] != null ? e.NewValues["SEVERIDADE"].ToString() : null,
                UsuarioId = this.User.Identity.Name
            };

            validacao = rnChamadoAnatel.Valida(chamadoAnatel, false);

            if (validacao.Valido)
            {
                rnChamadoAnatel.Atualiza(chamadoAnatel);
                e.Cancel = true;
                this.grdChamadoAnatel.CancelEdit();
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void grdChamadoAnatel_Init(object sender, EventArgs e)
        {
            ASPxGridView gridView = sender as ASPxGridView;
            gridView.JSProperties["cpShowDeleteConfirmBox"] = false;
        }

        protected void grdChamadoAnatel_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.ChamadoAnatel rnChamadoAnatel = new Techne.Lyceum.RN.FiscalizacaoLink.ChamadoAnatel();
            var id = ((ASPxGridView)sender).GetRowValuesByKeyValue(e.Keys[0], "CHAMADOANATELID");
            var data = ((ASPxGridView)sender).GetRowValuesByKeyValue(e.Keys[0], "DATARESOLUCAO");
            Session["idChamadoAnatel"] = null;
            DateTime? dataResolucao = !data.ToString().IsNullOrEmptyOrWhiteSpace()  ? Convert.ToDateTime(data) : (DateTime?)null;

            validacao = rnChamadoAnatel.ValidaRemocao(Convert.ToInt32(id), dataResolucao);

            if (validacao.Valido)
            {
                rnChamadoAnatel.Remove(Convert.ToInt32(id));
                e.Cancel = true;
                this.grdChamadoAnatel.CancelEdit();
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void tseContrato_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                tseCircuito.ResetValue();
                grdChamadoAnatel.Visible = false;

                if (!this.tseContrato.DBValue.IsNull)
                {
                    if (!this.tseContrato.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Contrato não cadastrado.";
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar um contrato.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseCircuito_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                grdChamadoAnatel.Visible = false;

                if (!this.tseCircuito.DBValue.IsNull)
                {
                    if (!this.tseCircuito.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Circuito não cadastrado.";
                    }
                    else
                    {
                        grdChamadoAnatel.Visible = true;
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar um circuito.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeAdministrativa_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                tseCircuito.ResetValue();
                tseContrato.ResetValue();
                grdChamadoAnatel.Visible = false;

                if (!this.tseUnidadeAdministrativa.DBValue.IsNull)
                {
                    if (!this.tseUnidadeAdministrativa.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Unidade Administrativa não cadastrada.";
                    }

                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar uma unidade administrativa.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            tseUnidadeAdministrativa.ResetValue();
            tseContrato.ResetValue();
            tseCircuito.ResetValue();
            grdChamadoAnatel.Visible = false;
            grdChamadoAnatel.DataBind();
        }
    }
}
