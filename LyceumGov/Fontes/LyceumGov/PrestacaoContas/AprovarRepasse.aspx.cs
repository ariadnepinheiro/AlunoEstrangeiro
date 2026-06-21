using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxTabControl;
using DevExpress.Web.Data;
using Techne.Lyceum.RN.PrestacaoContas;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using Techne.Controls;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [NavUrl("~/PrestacaoContas/AprovarRepasse.aspx"), ControlText("Análise de Lançamentos de Repasses"), Title("Análise de Lançamentos de Repasses")]
    public partial class AprovarRepasse : TPage
    {
        private readonly RN.PrestacaoContas.ItemPlanilhaOrcamentaria rnItemPlanilhaOrcamentaria = new RN.PrestacaoContas.ItemPlanilhaOrcamentaria();
        private readonly RN.PrestacaoContas.LancamentoRepasse rnLancamentoRepasse = new RN.PrestacaoContas.LancamentoRepasse();
        private readonly RN.PrestacaoContas.MotivoReprovacaoLancamentoRepasse rnMotivoReprovacaoLancamentoRepasse = new RN.PrestacaoContas.MotivoReprovacaoLancamentoRepasse();
        private readonly RN.PrestacaoContas.AnaliseRepasse rnAnaliseRepasse = new RN.PrestacaoContas.AnaliseRepasse();

        protected void Page_Load(object sender, EventArgs e)
        {
           
             try
            {
                lblMensagem.Text = string.Empty;
                

                if (!IsPostBack)
                {
                }
            }
             catch (Exception ex)
             {
                 lblMensagem.Text = ex.Message;
             }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            ControlaAcesso(grdLancamentoRepasse, AcaoControle.editar, "btnEditar");
            TituloGrid(grdLancamentoRepasse, string.Empty);
            Button[] controles = new Button[] { btnSalvarAprovacao };
            ControlarVisibilidadeControle(controles);
           
        }

        private void ControlarVisibilidadeControle(Button[] botoes)
        {
            btnSalvarAprovacao.Visible = false;
            if (Permission.AllowUpdate)
            {
                btnSalvarAprovacao.Visible = true;
            }
           
        }

        private void RetiraVisibilidadeBotao()
        {
            btnSalvarAprovacao.Visible = false;
            
        }

     

        protected void btnSalvarAprovacao_Click(object sender, EventArgs e)
        {
            try
            {
                var ars = new List<RN.PrestacaoContas.Entidades.AnaliseRepasse>();

                for (var i = 0; i < grdLancamentoRepasse.VisibleRowCount; i++)
                {
                    /*
                    Alterado por Felipe R. Gomes em 03/08/2023
                    
                    Conforme orientação do Rodrigo via Skype nesse mesmo dia (msg particular),
                    desabilitar essa regra específica para repasses importados do SEFAZ, pois
                    no integrador os repasses já deveriam vir com APROVADO = 1, e não vieram.
                    */
                    //if (grdLancamentoRepasse.GetRowValues(i, "WSREPASSESEFAZID") != DBNull.Value)
                    //    continue;

                    var ar = new RN.PrestacaoContas.Entidades.AnaliseRepasse();

                    ar.AnaliseRepasseId = Convert.ToInt32(grdLancamentoRepasse.GetRowValues(i, "ANALISEREPASSEID") != DBNull.Value ? grdLancamentoRepasse.GetRowValues(i, "ANALISEREPASSEID") : 0);
                    ar.LancamentoRepasseId = Convert.ToInt32(grdLancamentoRepasse.GetRowValues(i, "LANCAMENTOREPASSEID"));
                    ar.Aprovado = radAprovarTodos.Checked ? true : false;
                    ar.MotivoReprovacaoLancamentoRepasseId = !cmbMotivoReprovacao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? int.Parse(cmbMotivoReprovacao.SelectedValue) : (int?)null;
                    ar.UsuarioId = User.Identity.Name;

                    var validacaoDados = rnAnaliseRepasse.Valida(ar);
                    if (validacaoDados.Valido)
                        ars.Add(ar);
                    else
                    {
                        lblMensagem.Text = validacaoDados.Mensagem;
                        return;
                    }
                }

                rnAnaliseRepasse.SalvaLista(ars);

                grdLancamentoRepasse.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdLancamentoRepasse_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            var temWsRepasseSefazId = grdLancamentoRepasse.GetRowValues(e.VisibleIndex, "WSREPASSESEFAZID") != DBNull.Value;

            if (e.ButtonType == ColumnCommandButtonType.Edit)
                e.Visible = !temWsRepasseSefazId;
        }

        protected void grdLancamentoRepasse_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            try
            {
                MotivoReprovacaoLancamentoRepasse rnMotivo = new MotivoReprovacaoLancamentoRepasse();
                ValidacaoDados validacao = new ValidacaoDados();

                var analiseRepasseId = grdLancamentoRepasse.GetRowValues(grdLancamentoRepasse.EditingRowVisibleIndex, "ANALISEREPASSEID");

                var ar = new RN.PrestacaoContas.Entidades.AnaliseRepasse();
                int result = -1;

                if (e.NewValues["MOTIVOREPROVACAO"] != null)
                {
                    if (!Int32.TryParse(e.NewValues["MOTIVOREPROVACAO"].ToString(), out result))
                    {
                        result = rnMotivo.ObtemIdMotivoReprovacaoPor(Convert.ToString(e.NewValues["MOTIVOREPROVACAO"]));
                    }
                }

                ar.AnaliseRepasseId = Convert.ToInt32(analiseRepasseId != DBNull.Value ? analiseRepasseId : 0);
                ar.LancamentoRepasseId = Convert.ToInt32(grdLancamentoRepasse.GetRowValues(grdLancamentoRepasse.EditingRowVisibleIndex, "LANCAMENTOREPASSEID"));
                ar.Aprovado = Convert.ToBoolean((grdLancamentoRepasse.FindEditRowCellTemplateControl(grdLancamentoRepasse.Columns["ACAO"] as GridViewDataColumn, "rblAcao") as ASPxRadioButtonList).Value);
                ar.MotivoReprovacaoLancamentoRepasseId = ar.Aprovado ? (int?)null : (result > 0 ? result : (int?)null);
                ar.UsuarioId = User.Identity.Name;

                validacao = rnAnaliseRepasse.Valida(ar);

                if (validacao.Valido)
                {
                    rnAnaliseRepasse.Salva(ar);
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }

                grdLancamentoRepasse.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void plaAprovarRepasse_PreRender(object sender, EventArgs e)
        {
            var self = sender as PlaceHolder;
            if (!self.Visible)
                return;

            var itemPlanilhaOrcamentariaId = tseParcelaProgramacaoOrcamentaria.Value != null ? (int?)Convert.ToInt32(tseParcelaProgramacaoOrcamentaria.Value) : (int?)null;
            if (!itemPlanilhaOrcamentariaId.HasValue)
                return;

            using (var totalizadores = rnLancamentoRepasse.ListaSomaValorLancRepasseItemPlanilhaOrc((int)itemPlanilhaOrcamentariaId))
            {
                if (totalizadores.Rows.Count > 0)
                {
                    lblValorTotalRepasse.Text = string.Format("{0:N2}", totalizadores.Rows[0]["SOMALANCREPASSE"]);
                    lblValorTotalParcela.Text = string.Format("{0:N2}", totalizadores.Rows[0]["SOMAITEMPLAORC"]);
                }
                else
                {
                    lblValorTotalRepasse.Text = string.Empty;
                    lblValorTotalParcela.Text = string.Empty;
                }
            }

            radAprovarTodos.Checked = false;
            radReprovarTodos.Checked = false;
        }

        public DataTable ListaMotivoReprovacaoLancamentoRepasse()
        {
            var lista = rnMotivoReprovacaoLancamentoRepasse.ListaAtivoPor();
            var newRow = lista.NewRow();
            lista.Rows.InsertAt(newRow, 0);
            return lista;
        }

        public DataTable ListaLancamentoRepasse(decimal itemPlanilhaOrcamentariaId)
        {
            return rnLancamentoRepasse.ListaPor((int)itemPlanilhaOrcamentariaId);
        }

        public void UpdateLancamentoRepasse(object MOTIVOREPROVACAO, object LANCAMENTOREPASSEID)
        {
        }

        protected void tseParcelaProgramacaoOrcamentaria_Changed(object sender, ChangedEventArgs e)
        {
            try
            {
                var mensagens = new List<string>();


                if ((tsePeriodoPrestacaoContas.Value ?? "").ToString().IsNullOrEmptyOrWhiteSpace())
                    mensagens.Add("PERÍODO DE PRESTAÇÃO: Preenchimento obrigatório");

                if ((tseProgramacaoOrcamentaria.Value ?? "").ToString().IsNullOrEmptyOrWhiteSpace())
                    mensagens.Add("PROGRAMAÇÃO ORÇAMENTÁRIA: Preenchimento obrigatório");

                if ((tseParcelaProgramacaoOrcamentaria.Value ?? "").ToString().IsNullOrEmptyOrWhiteSpace())
                    mensagens.Add("PARCELA DA PROGRAMAÇÃO ORÇAMENTÁRIA: Preenchimento obrigatório");

                if (mensagens.Any())
                {
                    this.plaAprovarRepasse.Visible = false;

                    lblMesAnoReferencia.Text = String.Empty;
                    lblRegiaoFinanceira.Text = String.Empty;
                    lblFonteRecursos.Text = String.Empty;

                    lblMensagem.Text = mensagens.Aggregate((i, j) => i + "<br />" + j);
                    return;
                }

                var itemPlanilhaOrcamentaria = rnItemPlanilhaOrcamentaria.ObtemPor(int.Parse((tseParcelaProgramacaoOrcamentaria.Value ?? "").ToString()));

                if (itemPlanilhaOrcamentaria.Rows.Count > 0)
                {
                    var row = itemPlanilhaOrcamentaria.Rows[0];
                    lblMesAnoReferencia.Text = Convert.ToString(row["REFERENCIA"]) + " / " + Convert.ToString(row["ANO"]);
                    lblRegiaoFinanceira.Text = Convert.ToString(row["REGIAOFINANCEIRA_DESCRICAO"]);
                    lblFonteRecursos.Text = Convert.ToString(row["CODIGOSEFAZ"]) + " - " + Convert.ToString(row["FONTERECURSO_DESCRICAO"]);
                    grdLancamentoRepasse.DataBind();
                }
                else
                {
                    lblMesAnoReferencia.Text = String.Empty;
                    lblRegiaoFinanceira.Text = String.Empty;
                    lblFonteRecursos.Text = String.Empty;
                }

                plaAprovarRepasse.Visible = true;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tsePeriodoPrestacaoContas_Changed(object sender, ChangedEventArgs args)
        {
            if (!tsePeriodoPrestacaoContas.DBValue.IsNull)
            {
                var ano = rnItemPlanilhaOrcamentaria.PesquisaAnoPrestacao(tsePeriodoPrestacaoContas.DBValue.ToString() );
                tseProgramacaoOrcamentaria.ResetValue();
                tseParcelaProgramacaoOrcamentaria.ResetValue();
                tseProgramacaoOrcamentaria.SqlWhere = " ano = '" + ano + "'";
                tseParcelaProgramacaoOrcamentaria.SqlWhere = " planilhaorcamentariaid = 0";
            }
            tseProgramacaoOrcamentaria.DataBind();
            tseParcelaProgramacaoOrcamentaria.DataBind();
        }

        protected void tseProgramacaoOrcamentaria_Changed(object sender, ChangedEventArgs args)
        {
             try
            {
                var mensagens = new List<string>();
            
                 if ((tsePeriodoPrestacaoContas.Value ?? "").ToString().IsNullOrEmptyOrWhiteSpace())
                     mensagens.Add("PERÍODO DE PRESTAÇÃO: Preenchimento obrigatório");

                 if (mensagens.Any())
                 {
                     this.plaAprovarRepasse.Visible = false;

                     lblMesAnoReferencia.Text = String.Empty;
                     lblRegiaoFinanceira.Text = String.Empty;
                     lblFonteRecursos.Text = String.Empty;

                     lblMensagem.Text = mensagens.Aggregate((i, j) => i + "<br />" + j);
                     return;
                 }
                 
                 if (!tsePeriodoPrestacaoContas.DBValue.IsNull)
                    {
                        var retwhere = rnItemPlanilhaOrcamentaria.ObtemIntervaloPagamento(tsePeriodoPrestacaoContas.DBValue.ToString());
                        tseParcelaProgramacaoOrcamentaria.ResetValue();
                        tseParcelaProgramacaoOrcamentaria.SqlWhere = retwhere + " AND planilhaorcamentariaid = '" + tseProgramacaoOrcamentaria.DBValue + "'";
                    }
                    tseProgramacaoOrcamentaria.DataBind();
             }
             catch (Exception ex)
             {
                 lblMensagem.Text = ex.Message;
             }
        }
    }
}
