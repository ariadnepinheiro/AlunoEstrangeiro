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
using System.Web.UI.WebControls;

namespace Techne.Lyceum.Net.Patrimonio
{
    [NavUrl("~/Patrimonio/SolicitacaoTransferenciaPatrimonio.aspx"), ControlText("Transferência de Bem"), Title("Transferência de Bem")]

    public partial class SolicitacaoTransferenciaPatrimonio : TPage
    {


        public object Lista(object setor, object conta)
        {
            RN.Patrimonio.Bem rnBem = new Techne.Lyceum.RN.Patrimonio.Bem();

            if (!string.IsNullOrEmpty(setor.ToString()) && !string.IsNullOrEmpty(conta.ToString()))
            {
                return rnBem.ListaPatrimonioAtivoPor(setor.ToString(), conta.ToString());
            }
            return null;
        }

        //public object ListaSelecionado()
        //{
        //    Session["grid"]
        //    return null;
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                if (!this.IsPostBack)
                {
                    Session["grid"] = null;
                    pnlGrid.Visible = false;
                    InicializaGrid();
                }
                else
                {
                    grdSelecionado.DataSource = Session["grid"];
                    grdSelecionado.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }      

        private void InicializaGrid()
        {

            DataTable dt = new DataTable();

            DataRow dr = null;

            dt.Columns.Add(new DataColumn("BEMID", typeof(string)));
            dt.Columns.Add(new DataColumn("NUMERO", typeof(string)));
            dt.Columns.Add(new DataColumn("DESCRICAO", typeof(string)));
            dt.Columns.Add(new DataColumn("CONTA", typeof(string)));
            dt.Columns.Add(new DataColumn("CLASSIFICACAO", typeof(string)));
            dt.Columns.Add(new DataColumn("ESTADOCONSERVACAO", typeof(string)));
            dt.Columns.Add(new DataColumn("VALORCOMSIGLA", typeof(string)));
            dt.Columns.Add(new DataColumn("DATAAQUISICAO", typeof(string)));
            dt.Columns.Add(new DataColumn("DATAINCORPORACAO", typeof(string)));

            dr = dt.NewRow();

            Session["grid"] = dt;

            grdSelecionado.DataSource = dt;
            grdSelecionado.DataBind();

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

        protected void Page_Init(object sender, EventArgs e)
        {
            ControlarVisibilidadeControle();
            TituloGrid(grdPatrimonio, "Patrimônio");
            TituloGrid(grdSelecionado, "Lista de Transferência");
        }

        private void ControlarVisibilidadeControle()
        {
            ControlaAcesso(btnSalvar, AcaoControle.novo);
            ControlaAcesso(btnAdicionar, AcaoControle.novo);
        }

        protected void tseUACedente_Changed(object sender, EventArgs args)
        {
            if (Page.IsCallback)
            {
                return;
            }
            try
            {
                tseClassificacao.ResetValue();
                tseUADestinataria.ResetValue();
                pnlGrid.Visible = false;
                if (!this.tseUACedente.DBValue.IsNull)
                {
                    if (this.tseUACedente.IsValidDBValue)
                    {
                        odsPatrimonio.Select();
                        odsPatrimonio.DataBind();
                        grdPatrimonio.DataBind();
                        grdPatrimonio.Selection.UnselectAll();
                        InicializaGrid();
                    }
                    else
                    {
                        this.lblMensagem.Text = "Unidade Administrativa Cedente não cadastrada.";
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar uma unidade administrativa cedente.";
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
                tseUADestinataria.ResetValue();

                if (!this.tseClassificacao.DBValue.IsNull)
                {
                    if (this.tseClassificacao.IsValidDBValue)
                    {
                        odsPatrimonio.Select();
                        odsPatrimonio.DataBind();
                        grdPatrimonio.DataBind();
                        pnlGrid.Visible = true;
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

        protected void tseUADestinataria_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (!this.tseUADestinataria.DBValue.IsNull)
                {
                    if (!this.tseUADestinataria.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Unidade Administrativa Destinatária não cadastrada.";
                    }
                    else
                    {


                        grdSelecionado.DataSource = null;
                        grdSelecionado.DataBind();

                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar uma unidade administrativa destinatária.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                var validacao = new ValidacaoDados();
                var rnTransferencia = new Techne.Lyceum.RN.Patrimonio.Transferencia();
                var rnTransferenciaItem = new Techne.Lyceum.RN.Patrimonio.TransferenciaItem();
                var transferencia = new Techne.Lyceum.RN.Patrimonio.Entidades.Transferencia();
                var validItems = new List<string[]>();
                var invalidItems = new List<string[]>();

                lblMensagem.Text = string.Empty;

                if (this.chkAtesto.Checked == false)
                {
                    lblMensagem.Text = "Não é possivel realizar a solicitação sem a entrega da declaração de transferência.";
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Não é possivel realizar a solicitação sem a entrega da declaração de transferência.');", true);
                    return;
                }

                if (grdSelecionado.VisibleRowCount == 0)
                {
                    lblMensagem.Text = "Para solicitar uma transferência é necessário selecionar pelo menos um patrimônio.";
                    return;
                }

                transferencia.SetorOrigem = !tseUACedente.DBValue.IsNull && tseUACedente.IsValidDBValue ? tseUACedente["setor"].ToString() : null;
                transferencia.SetorDestino = !tseUADestinataria.DBValue.IsNull && tseUADestinataria.IsValidDBValue ? tseUADestinataria["setor"].ToString() : null;
                transferencia.UsuarioSolicitanteId = User.Identity.Name;

                for (var rowIndex = 0; rowIndex < this.grdSelecionado.VisibleRowCount; rowIndex++)
                {
                    var id = Convert.ToInt32(grdSelecionado.GetRowValues(rowIndex, "BEMID"));
                    var numero = grdSelecionado.GetRowValues(rowIndex, "NUMERO").ToString();
                    if (id > 0)
                    {
                        validacao = rnTransferencia.ValidaSolicitacao(transferencia, id);
                        if (validacao.Valido)
                            validItems.Add(new string[] { id.ToString(), numero });
                        else
                            invalidItems.Add(new string[] { id.ToString(), numero + ": " + validacao.Mensagem });
                    }
                }

                if (invalidItems.Count() > 0)
                {
                    lblMensagem.Text += !lblMensagem.Text.IsNullOrEmptyOrWhiteSpace() ? "<br />" : "";
                    lblMensagem.Text += invalidItems.Select(s => s[1].Replace(Environment.NewLine, " ; ")).Aggregate((c, n) => c + "<br />" + n);
                }

                if (validItems.Count() > 0)
                {
                    rnTransferencia.SolicitaTransferencia(transferencia, validItems.Select(s => Convert.ToInt32(s[0])).ToList());

                    odsPatrimonio.Select();
                    odsPatrimonio.DataBind();
                    grdPatrimonio.DataBind();
                    grdPatrimonio.Selection.UnselectAll();
                    InicializaGrid();

                    lblMensagem.Text += !lblMensagem.Text.IsNullOrEmptyOrWhiteSpace() ? "<br />" : "";
                    lblMensagem.Text += validItems.Select(s => s[1] + ": Transferência realizada com sucesso.").Aggregate((c, n) => c + "<br />" + n);

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Solicitação(ões) realizada(s) com sucesso. Lote nº " + transferencia.TransferenciaId + " .');", true);
                    lblMensagem.Text = "Solicitação(ões) realizada(s) com sucesso. Lote nº " + transferencia.TransferenciaId + ".<br /><br />" + lblMensagem.Text;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnAdicionar_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtCurrentTable = (DataTable)Session["grid"];
                List<object> fieldValues = grdPatrimonio.GetSelectedFieldValues(new string[] { "BEMID", "NUMERO", "DESCRICAO", "CONTA", "CLASSIFICACAO", "ESTADOCONSERVACAO", "VALORCOMSIGLA", "DATAAQUISICAO", "DATAINCORPORACAO" });

                if (fieldValues.Count() == 0)
                {
                    lblMensagem.Text = "Para adicionar solicitações de transferência é necessário selecionar pelo menos um patrimônio.";
                    return;
                }

                List<int> listaBemId = new List<int>();

                for (var rowIndex = 0; rowIndex < this.grdSelecionado.VisibleRowCount; rowIndex++)
                {
                    int id = Convert.ToInt32(grdSelecionado.GetRowValues(rowIndex, "BEMID"));

                    if (!listaBemId.Contains(id))
                    {
                        listaBemId.Add(id);
                    }
                }


                foreach (object[] item in fieldValues)
                {
                    if (!listaBemId.Contains(Convert.ToInt32(item[0])))
                    {
                        dtCurrentTable.Rows.Add(item[0].ToString(), item[1].ToString(), item[2].ToString(), item[3].ToString(), item[4].ToString(), item[5].ToString(), item[6].ToString(), item[7].ToString(), item[8].ToString());

                        Session["grid"] = dtCurrentTable;
                    }
                }

                //var x = (from r in dtCurrentTable.AsEnumerable()
                //         select r["BEMID"]).Distinct().ToList();


                DataView view = new DataView(dtCurrentTable);
                DataTable distinctValues = view.ToTable(true, "BEMID", "NUMERO", "DESCRICAO", "CONTA", "CLASSIFICACAO", "ESTADOCONSERVACAO", "VALORCOMSIGLA", "DATAAQUISICAO", "DATAINCORPORACAO");

                grdSelecionado.DataSource = distinctValues;
                grdSelecionado.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void grdPatrimonio_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "VALOR")
            {
                var sigla = e.GetListSourceFieldValue("SIGLA");
                var valor = e.GetListSourceFieldValue("VALOR");

                e.Value = sigla + " " + valor;
            }
        }
        protected void grdSelecionado_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            DataTable dtCurrentTable = (DataTable)Session["grid"];
            var IDBEM = Convert.ToDecimal(grdSelecionado.GetRowValues(e.VisibleIndex, "BEMID"));

            if (e.ButtonID == "btnExcluir")
            {
                try
                {
                    dtCurrentTable.AcceptChanges();
                   
                    DataRow[] dadosLinha = dtCurrentTable.Select("BEMID = " + IDBEM);

                    dtCurrentTable.Rows.Remove(dadosLinha[0]);

                    dtCurrentTable.AcceptChanges();

                    Session["grid"] = dtCurrentTable;

                    grdSelecionado.DataSource = dtCurrentTable;
                    grdSelecionado.DataBind();
                }
                catch (Exception ex)
                {
                    Session["Mensagem"] = ex.Message;
                }
            }
        }
    }
}
