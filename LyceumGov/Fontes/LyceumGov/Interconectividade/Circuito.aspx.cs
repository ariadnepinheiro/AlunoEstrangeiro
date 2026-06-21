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
using DevExpress.Web.ASPxEditors;
namespace Techne.Lyceum.Net.Interconectividade
{
    [NavUrl("~/Interconectividade/Circuito.aspx")]
    [ControlText("Contrato")]
    [Title("Contrato")]

    public partial class Circuito : TPage
    {
        public object ListaCircuito(object contrato)
        {
            RN.FiscalizacaoLink.CircuitoSetor rnCircuitoSetor = new Techne.Lyceum.RN.FiscalizacaoLink.CircuitoSetor();

            if (contrato != null)
            {
                return rnCircuitoSetor.ListaPor(Convert.ToInt32(contrato));
            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString.Keys.Count > 0)
                    {
                        byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                        string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                        tseUA.DBValue = ObterDadosQueryString(decodedText);
                        tseUA.Mode = ControlMode.View;
                        tseUA_Changed(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {

            TituloGrid(grdCircuito, string.Empty);
        }
        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            tseUA.Mode = ControlMode.View;
        }
        private string ObterDadosQueryString(string queryString)
        {
            string[] listaDados = queryString.Split('&');
            string setor = null;


            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("setor=") >= 0)
                    setor = dados.Substring(dados.LastIndexOf('=') + 1);

            }

            return setor;
        }

        protected void tseUA_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                tseContratoCircuito.ResetValue();

                if (!this.tseUA.DBValue.IsNull)
                {
                    if (!this.tseUA.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Unidade Administrativa não cadastrada.";
                    }
                    else
                    {
                        tseContratoCircuito.ResetValue();
                        tseContratoCircuito.SqlWhere = " SETORID = '" + tseUA.DBValue + "'";
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


        protected void tseContratoCircuito_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                RN.FiscalizacaoLink.Entidades.ContratoSetor contratoSetor = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.ContratoSetor();
                RN.FiscalizacaoLink.ContratoSetor rnContratoSetor = new Techne.Lyceum.RN.FiscalizacaoLink.ContratoSetor();
                // LimparTela();


                if (!this.tseContratoCircuito.DBValue.IsNull)
                {
                    if (!this.tseContratoCircuito.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Contrato não cadastrado.";
                    }
                    else
                    {

                        grdCircuito.DataBind();
                        hdnContratoSetorId.Value = Convert.ToString(tseContratoCircuito["contratosetorid"]);
                        grdCircuito.Visible = true;
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

        protected void grdCircuito_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdCircuito);
        }

        protected void grdCircuito_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdCircuito.Settings.ShowFilterRow = false;
        }

        protected void grdCircuito_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdCircuito.Settings.ShowFilterRow = false;
        }

        protected void grdCircuito_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdCircuito.IsNewRowEditing)
            {
                if (this.tseContratoCircuito.DBValue.IsNull || !this.tseContratoCircuito.IsValidDBValue || tseContratoCircuito.DBValue.ToString().IsNullOrEmptyOrWhiteSpace())
                {
                    throw new Exception("Selecione o Contrato");
                }

                if ((e.Column.FieldName) == "CONTRATOSETORID")
                {
                    e.Editor.Enabled = true;
                }
                if ((e.Column.FieldName) == "TECNOLOGIAID")
                {
                    e.Editor.ReadOnly = false;
                }
                if ((e.Column.FieldName) == "VELOCIDADEID")
                {
                    e.Editor.ReadOnly = false;
                }
                if ((e.Column.FieldName) == "DESIGNACAO")
                {
                    e.Editor.ReadOnly = false;
                }
            }
            else if (grdCircuito.IsEditing)
            {
                if ((e.Column.FieldName) == "CONTRATOSETORID")
                {
                    e.Editor.Enabled = false;
                }
                if ((e.Column.FieldName) == "TECNOLOGIAID")
                {
                    e.Editor.ReadOnly = false;
                }
                if ((e.Column.FieldName) == "VELOCIDADEID")
                {
                    e.Editor.ReadOnly = false;
                }
                if ((e.Column.FieldName) == "DESIGNACAO")
                {
                    e.Editor.ReadOnly = false;
                }
            }
        }

        public void Insert(object TECNOLOGIAID, object VELOCIDADEID, object DESIGNACAO) { }

        public void Update(object TECNOLOGIAID, object VELOCIDADEID, object DESIGNACAO, object CIRCUITOSETORID)
        {
        }
        public void Delete(object CIRCUITOSETORID) { }


        protected void grdCircuito_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {

            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.CircuitoSetor rnCircuitoSetor = new Techne.Lyceum.RN.FiscalizacaoLink.CircuitoSetor();

            RN.FiscalizacaoLink.Entidades.CircuitoSetor circuito = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.CircuitoSetor
            {
                ContratoSetorId = !hdnContratoSetorId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnContratoSetorId.Value) : 0,
                VelocidadeId = e.NewValues["VELOCIDADEID"] != null ? Convert.ToInt32(e.NewValues["VELOCIDADEID"].ToString()) : 0,
                TecnologiaId = e.NewValues["TECNOLOGIAID"] != null ? Convert.ToInt32(e.NewValues["TECNOLOGIAID"].ToString()) : 0,
                Designacao = e.NewValues["DESIGNACAO"] != null ? e.NewValues["DESIGNACAO"].ToString() : null,
                Inicio = e.NewValues["INICIO"] != null ? Convert.ToDateTime(e.NewValues["INICIO"]) : (DateTime?)null,
                Fim = e.NewValues["FIM"] != null ? Convert.ToDateTime(e.NewValues["FIM"]) : (DateTime?)null,
                QuantidadeMeses = e.NewValues["QUANTIDADEMESES"] != null ? Convert.ToInt32(e.NewValues["QUANTIDADEMESES"].ToString()) : -1,
                CustoMensal = e.NewValues["CUSTOMENSAL"] != null ? Convert.ToDecimal(e.NewValues["CUSTOMENSAL"].ToString()) : (Decimal?)null,
                UsuarioId = this.User.Identity.Name
            };

            validacao = rnCircuitoSetor.Valida(circuito, true,tseUA.DBValue.ToString());

            if (validacao.Valido)
            {
                rnCircuitoSetor.Insere(circuito);
                e.Cancel = true;
                this.grdCircuito.CancelEdit();
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void grdCircuito_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {

            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.CircuitoSetor rnCircuitoSetor = new Techne.Lyceum.RN.FiscalizacaoLink.CircuitoSetor();

            RN.FiscalizacaoLink.Entidades.CircuitoSetor circuito = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.CircuitoSetor
            {
                ContratoSetorId = !hdnContratoSetorId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnContratoSetorId.Value) : 0,
                CircuitoSetorId = Convert.ToInt32(e.Keys["CIRCUITOSETORID"]),
                VelocidadeId = e.NewValues["VELOCIDADEID"] != null ? Convert.ToInt32(e.NewValues["VELOCIDADEID"].ToString()) : 0,
                TecnologiaId = e.NewValues["TECNOLOGIAID"] != null ? Convert.ToInt32(e.NewValues["TECNOLOGIAID"].ToString()) : 0,
                Designacao = e.NewValues["DESIGNACAO"] != null ? e.NewValues["DESIGNACAO"].ToString() : null,
                Inicio = e.NewValues["INICIO"] != null ? Convert.ToDateTime(e.NewValues["INICIO"]) : (DateTime?)null,
                Fim = e.NewValues["FIM"] != null ? Convert.ToDateTime(e.NewValues["FIM"]) : (DateTime?)null,
                QuantidadeMeses = e.NewValues["QUANTIDADEMESES"] != null ? Convert.ToInt32(e.NewValues["QUANTIDADEMESES"].ToString()) : -1,
                CustoMensal = e.NewValues["CUSTOMENSAL"] != null ? Convert.ToDecimal(e.NewValues["CUSTOMENSAL"].ToString()) : (Decimal?)null,

                UsuarioId = this.User.Identity.Name
            };

            validacao = rnCircuitoSetor.Valida(circuito, false, tseUA.DBValue.ToString());

            if (validacao.Valido)
            {
                rnCircuitoSetor.Atualiza(circuito);
                e.Cancel = true;
                this.grdCircuito.CancelEdit();
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void grdCircuito_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.CircuitoSetor rnCircuitoSetor = new Techne.Lyceum.RN.FiscalizacaoLink.CircuitoSetor();
            int circuitoSetorId = 0;

            circuitoSetorId = Convert.ToInt32(e.Keys["CIRCUITOSETORID"]);

            validacao = rnCircuitoSetor.ValidaRemocao(circuitoSetorId);

            if (validacao.Valido)
            {
                rnCircuitoSetor.Remove(circuitoSetorId);
                grdCircuito.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
        protected void grdCircuito_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (this.tseContratoCircuito.DBValue.IsNull || !this.tseContratoCircuito.IsValidDBValue || tseContratoCircuito.DBValue.ToString().IsNullOrEmptyOrWhiteSpace() )
            {
                if (e.ButtonType == ColumnCommandButtonType.New)
                {
                    e.Visible = false;
                }
            }
        }


        protected void pcContrato_TabClick(object source, TabControlCancelEventArgs e)
        {
            try
            {
                this.lblMensagem.Text = string.Empty;

                string queryString = "setor=" + tseUA.DBValue;

                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                if (Convert.ToInt16(e.Tab.Index) == 0)
                {
                    Response.Redirect("Contrato.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string queryString = "setor=" + tseUA.DBValue;

                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                Response.Redirect("Contrato.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


    }
}
