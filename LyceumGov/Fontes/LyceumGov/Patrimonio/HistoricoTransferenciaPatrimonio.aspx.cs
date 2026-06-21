using System;
using System.Web;
using DevExpress.Web.ASPxTabControl;
using Techne.Controls;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using System.Text;
using System.Data;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.DTOs.Agenda;
using DevExpress.Web.ASPxGridView;
using System.Collections.Generic;
using System.Linq;
using Techne.Data;

namespace Techne.Lyceum.Net.Patrimonio
{
    [NavUrl("~/Patrimonio/HistoricoTransferenciaPatrimonio.aspx"), ControlText("Histórico Transferência Patrimônio"), Title("Histórico Transferência Patrimônio")]

    public partial class HistoricoTransferenciaPatrimonio : TPage
    {
        public object Lista(object bem)
        {
            RN.Patrimonio.TransferenciaItem rnTransferenciaItem = new Techne.Lyceum.RN.Patrimonio.TransferenciaItem();

            if (bem != null)
            {
                return rnTransferenciaItem.ListaHistoricoTransferenciaPor(Convert.ToInt32(bem));
            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                if (!this.IsPostBack)
                {
                    hdnBem.Value = string.Empty;
                    pnlGrid.Visible = false;
                }
                CarregaBem();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdHistorico, string.Empty);
        }

        protected void pcTransferencia_TabClick(object source, TabControlCancelEventArgs e)
        {
            if (e.Tab.Index == 0)
            {
                this.Server.Transfer("SolicitacaoTransferenciaPatrimonio.aspx");
            }

            if (e.Tab.Index == 1)
            {
                this.Server.Transfer("AcompanhamentoTransferenciaPatrimonio.aspx");
            }

            if (e.Tab.Index == 2)
            {
                this.Server.Transfer("HistoricoTransferenciaPatrimonio.aspx");
            }
        }

        protected void tseUA_Changed(object sender, EventArgs args)
        {
            if (Page.IsCallback)
            {
                return;
            }
            try
            {
                tseClassificacao.ResetValue();
                tseBem.ResetValue();
                pnlGrid.Visible = false;
                hdnBem.Value = string.Empty;
                if (!this.tseUA.DBValue.IsNull)
                {
                    if (this.tseUA.IsValidDBValue)
                    {
                        grdHistorico.DataBind();
                    }
                    else
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

        protected void tseClassificacao_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                tseBem.ResetValue();
                hdnBem.Value = string.Empty;
                if (!this.tseClassificacao.DBValue.IsNull)
                {
                    if (this.tseClassificacao.IsValidDBValue)
                    {
                        CarregaBem();
                        grdHistorico.DataBind();
                    }
                    else
                    {
                        this.lblMensagem.Text = "Classificação não cadastrada.";
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar uma classificacao.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaBem()
        {
            try
            {
                string table = string.Empty;
                Techne.Library.Sql.Structure.SqlSelectColumns coluna = new Techne.Library.Sql.Structure.SqlSelectColumns();
                Techne.Library.Sql.Structure.SqlSelect sqlSelect;

                if ((!this.tseClassificacao.DBValue.IsNull && this.tseClassificacao.IsValidDBValue) && (!this.tseUA.DBValue.IsNull && this.tseUA.IsValidDBValue))
                {
                    table = " Patrimonio.VW_BEMSETOR ";
                    tseBem.SqlWhere = " classificacaoid = " + Convert.ToInt32(tseClassificacao["classificacaoid"].ToString()) + " AND SETOR = '" + tseUA["setor"].ToString() + "'";
                }

                if ((this.tseClassificacao.DBValue.IsNull && !this.tseClassificacao.IsValidDBValue) && (!this.tseUA.DBValue.IsNull && this.tseUA.IsValidDBValue))
                {
                    table = " Patrimonio.VW_BEMSETOR ";
                    //tseBem.SqlWhere = " SETOR = '" + tseUA.DBValue.ToString() + "'";
                    tseBem.SqlWhere = " SETOR = '" + tseUA["setor"].ToString() + "'";                    
                }

                coluna.Add("numero");
                coluna.Add("descricao");
                coluna.Add("bemid ");
                coluna.Add("numeroformatado");

                sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);

                tseBem.SqlSelect = sqlSelect;
                tseBem.DataBind();
            }
            catch (Exception ex)
            {                
                throw ex;
            }           
        }

        protected void tseBem_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                hdnBem.Value = string.Empty;
                if (!this.tseBem.DBValue.IsNull)
                {
                    if (this.tseBem.IsValidDBValue)
                    {
                        hdnBem.Value = tseBem["bemid"].ToString();
                        odsHistorico.Select();
                        odsHistorico.DataBind();
                        grdHistorico.DataBind();
                        pnlGrid.Visible = true;
                    }
                    else
                    {
                        this.lblMensagem.Text = "Bem não cadastrado.";
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar um bem.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdHistorico_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "VALORFORMAT")
            {
                var sigla = e.GetListSourceFieldValue("SIGLA");
                var valor = e.GetListSourceFieldValue("VALOR");

                var valorFormatado = string.Format("{0:N2}", valor);

                e.Value = sigla + " " + valorFormatado;
            }
        }
    }
}
