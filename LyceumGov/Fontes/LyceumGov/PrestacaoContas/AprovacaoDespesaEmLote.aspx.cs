using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using System.Web.Caching;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using DevExpress.Data.Filtering;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [NavUrl("~/PrestacaoContas/AprovacaoDespesaEmLote.aspx")]
    [ControlText("Validação de despesas em lote")]
    [Title("Validação de despesas em lote")]
    public partial class AprovacaoDespesaEmLote : TPage
    {
        private readonly Techne.Lyceum.RN.PrestacaoContas.Evento rnEvento = new Techne.Lyceum.RN.PrestacaoContas.Evento();

        protected void Page_Init()
        {
            if (IsPostBack)
                return;

            TituloGrid(grdDespesas, "Despesas Enviadas para Análise");
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdDespesas);
            ControlaAcesso(btnAprovarSelecionadas, AcaoControle.editar);
            ControlaAcesso(btnAprovarTodas, AcaoControle.editar);
        }

        #region Eventos das TSearches de busca

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }
                
                var sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (sessao != null)
                {
                    if (!this.tseRegional.DBValue.IsNull)
                    {
                        if (this.tseRegional.IsValidDBValue)
                        {
                            sessao.Regional = Convert.ToString(tseRegional.DBValue);
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;

                            tseMunicipio.ResetValue();
                            tseUnidadeResponsavel.ResetValue();
                        }
                        else
                        {
                            sessao.Regional = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                        }
                    }
                    else
                    {
                        sessao.Regional = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                        tseMunicipio.ResetValue();
                        tseUnidadeResponsavel.ResetValue();
                    }
                }
            }
            catch (Exception ex)
            {
                Mensagem = ex.Message;
            }
        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                var sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (sessao != null)
                {
                    if (!this.tseMunicipio.DBValue.IsNull)
                    {
                        if (this.tseMunicipio.IsValidDBValue)
                        {
                            sessao.Municipio = Convert.ToString(tseMunicipio.DBValue);
                            sessao.Escola = string.Empty;
                        }
                        else
                        {
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                            tseUnidadeResponsavel.ResetValue();
                        }
                    }
                    else
                    {
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                        tseUnidadeResponsavel.ResetValue();
                    }
                }
            }
            catch (Exception ex)
            {
                Mensagem = ex.Message;
            }
        }

        protected void tseUnidadeResponsavel_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }
                
                var sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (!this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (this.tseUnidadeResponsavel.IsValidDBValue)
                    {
                        if (!this.tseUnidadeResponsavel["unidade_ens"].IsNull)
                        {
                            this.tseRegional.Value = this.tseUnidadeResponsavel["id_regional"];
                            this.tseMunicipio.Value = this.tseUnidadeResponsavel["municipio"];
                        }

                        if (sessao != null)
                        {
                            sessao.Escola = Convert.ToString(this.tseUnidadeResponsavel.DBValue);
                            sessao.Regional = Convert.ToString(this.tseRegional.DBValue);
                            sessao.Municipio = Convert.ToString(this.tseMunicipio.DBValue);
                        }

                        //plaGrid.Visible = false;
                        plaGrid.Style.Add("display", "none");
                        upGrid.Update();
                    }
                    else
                    {
                        Mensagem = "Unidade de ensino não encontrada.";
                        
                        if (sessao != null)
                        {
                            sessao.Escola = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Regional = string.Empty;
                        }
                    }
                }
                else
                {
                    if (sessao != null)
                    {
                        sessao.Escola = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Regional = string.Empty;
                    }
                    
                    Mensagem = "Favor consultar uma unidade de ensino.";
                }
            }
            catch (Exception ex)
            {
                Mensagem = ex.Message;
            }
        }

        #endregion

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                //plaGrid.Visible = false;
                plaGrid.Style.Add("display", "none");

                var mensagens = new List<string>();

                if (tseUnidadeResponsavel.Value == null || !tseUnidadeResponsavel.IsValidDBValue)
                    mensagens.Add("UNIDADE DE ENSINO: Seleção obrigatória");

                if (mensagens.Any())
                {
                    Mensagem = mensagens.Distinct().Aggregate((x, y) => x + "<br />" + y);
                    return;
                }

                odsEvento.SelectParameters["censo"].DefaultValue = Convert.ToString(tseUnidadeResponsavel.Value);

                grdDespesas.FilterExpression = string.Empty;
                grdDespesas.Selection.UnselectAll();
                grdDespesas.DataBind();
                lblQtd.Text = grdDespesas.VisibleRowCount.ToString();
                cmbFilterMode.SelectedIndex = 0;

                upGrid.Update();

                //plaGrid.Visible = true;
                plaGrid.Style.Add("display", "block");
            }
            catch (Exception ex)
            {
                Mensagem = ex.Message;
            }
        }

        protected void btnAprovarSelecionadas_Click(object sender, EventArgs e)
        {
            try
            {
                var aprovarDespesa = new Func<int, int, ValidacaoDados>((eventoId, finalidadeId) =>
                {
                    var validacao = rnEvento.ValidaFinalizacao(eventoId, finalidadeId, User.Identity.Name, false);

                    if (validacao.Valido)
                        rnEvento.Finaliza(eventoId, User.Identity.Name);

                    return validacao;
                });
                var mensagens = new List<string>();
                var linhasSelecionadas = grdDespesas.GetSelectedFieldValues("EVENTOID", "FINALIDADEID", "NUMEROEVENTO");

                if (linhasSelecionadas.Any())
                {
                    foreach (object[] sel in linhasSelecionadas)
                    {
                        var resultado = aprovarDespesa(Convert.ToInt32(sel[0]), Convert.ToInt32(sel[1]));
                        if (resultado.Valido)
                        {
                            var mensagem = string.Format("{0}: Despesa aprovada com sucesso!", sel[2]);
                            mensagens.Add(mensagem);
                        }
                        else
                        {
                            var mensagem = string.Format("{0}: {1}", sel[2], resultado.Mensagem);
                            mensagens.Add(mensagem);
                        }
                    }
                }
                else
                {
                    mensagens.Add("É preciso selecionar uma despesa para poder aprová-la");
                }

                grdDespesas.DataBind();
                grdDespesas.FilterExpression = string.Empty;
                grdDespesas.Selection.UnselectAll();
                lblQtd.Text = grdDespesas.VisibleRowCount.ToString();
                cmbFilterMode.SelectedIndex = 0;

                upGrid.Update();

                Mensagem = mensagens.Aggregate((c, n) => c + "<br />" + n);
            }
            catch (Exception ex)
            {
                Mensagem = ex.Message;
            }
        }

        protected void btnAprovarTodas_Click(object sender, EventArgs e)
        {
            grdDespesas.Selection.SelectAll();
            btnAprovarSelecionadas_Click(sender, e);
        }

        protected void grdDespesas_SelectionChanged(object sender, EventArgs e)
        {
            lblQtdSel.Text = grdDespesas.Selection.Count.ToString();
        }

        protected void grdDespesas_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            ASPxGridView gridView = (ASPxGridView)sender;
            switch (e.Parameters)
            {
                case "All":
                    gridView.FilterExpression = string.Empty;
                    break;
                case "Selected":
                    FilterOutNonSelectedRows(gridView, true);
                    break;
                case "UnSelected":
                    FilterOutNonSelectedRows(gridView, false);
                    break;
            }
            gridView.DataBind();
        }

        protected void FilterOutNonSelectedRows(ASPxGridView gridView, bool selectedRows)
        {
            CriteriaOperator selectionCriteria = new InOperator(gridView.KeyFieldName, gridView.GetSelectedFieldValues(gridView.KeyFieldName));
            if (!selectedRows)
                selectionCriteria = selectionCriteria.Not();
            gridView.FilterExpression = (GroupOperator.Combine(GroupOperatorType.And, selectionCriteria)).ToString();
        }

        private string Mensagem
        {
            get
            {
                return lblMensagem.Text;
            }
            set
            {
                lblMensagem.Text = value;
                plaMensagem.Visible = (lblMensagem.Text != string.Empty);
            }
        }

        public object ListaDespesa(string censo)
        {
            return rnEvento.ListaEventoParaAnalisePor(censo, null, null, null);
        }
    }
}
