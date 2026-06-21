using System;
using DevExpress.Web.ASPxTabControl;
using Techne.Controls;
using Techne.Web;
using System.Data;
using DevExpress.Web.ASPxGridView;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.DTOs;

namespace Techne.Lyceum.Net.Patrimonio
{
    [NavUrl("~/Patrimonio/BaixaDeBensEmLote.aspx"), ControlText("Baixa de Bens em Lote"), Title("Baixa de Bens em Lote")]

    public partial class BaixaDeBensEmLote : TPage
    {
        public DataTable DataTableSelecionado
        {
            get
            {
                /*
                Como o data table está sendo armazenada em uma variável de sessão,
                então para que não haja conflitos entre abas de navegador em uma
                mesma sessão, foi gerado um Guid para cada instância da página.
                
                Esse Guid será usado para nomear a key da variável de sessão
                que armazena o data table. Dessa forma, cada aba de navegador que
                estiver com essa página aberta terá sua própria variável de sessão,
                sem que uma aba interfira com a outra.
                
                Obs.: foi tentado substituir o Session por ViewState. Seria mais apropriado
                se não fosse pelo fato de o link "excluir" do registro da tabela não
                fizesse uma chamada Ajax para excluir o registro do data table ao invés de um
                Postback. Somente o PostBack atualiza o ViewState. Sessions podem ser 
                atualizadas tanto por um PostBack quanto por uma chamada Ajax.
                 */

                if (string.IsNullOrEmpty(hidPageInstance.Value))
                    hidPageInstance.Value = Guid.NewGuid().ToString();

                DataTable dt = Session["BaixaDeBensEmLote-" + hidPageInstance.Value] as DataTable;

                if (dt == null)
                {
                    dt = new DataTable();

                    dt.Columns.Add(new DataColumn("BEMID", typeof(string)));
                    dt.Columns.Add(new DataColumn("NUMERO", typeof(string)));
                    dt.Columns.Add(new DataColumn("DESCRICAO", typeof(string)));
                    dt.Columns.Add(new DataColumn("CONTA", typeof(string)));
                    dt.Columns.Add(new DataColumn("CLASSIFICACAO", typeof(string)));
                    dt.Columns.Add(new DataColumn("ESTADOCONSERVACAO", typeof(string)));
                    dt.Columns.Add(new DataColumn("VALORCOMSIGLA", typeof(string)));
                    dt.Columns.Add(new DataColumn("DATAAQUISICAO", typeof(string)));
                    dt.Columns.Add(new DataColumn("DATAINCORPORACAO", typeof(string)));

                    Session["BaixaDeBensEmLote-" + hidPageInstance.Value] = dt;
                }

                return dt;
            }
        }

        public object Lista(object setor, object conta)
        {
            RN.Patrimonio.Bem rnBem = new Techne.Lyceum.RN.Patrimonio.Bem();

            if (!string.IsNullOrEmpty(setor.ToString()) && !string.IsNullOrEmpty(conta.ToString()))
            {
                return rnBem.ListaPatrimonioAtivoPor(setor.ToString(), conta.ToString());
            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                lblMensagemBaixa.Text = string.Empty;

                if (!this.IsPostBack)
                {
                    pnlGrid.Visible = false;
                    CarregaMotivoBaixa();
                }

                grdSelecionado.DataSource = DataTableSelecionado;
                grdSelecionado.DataBind();
                AtualizaQtdItensSelecionados();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void pcTransferencia_TabClick(object source, TabControlCancelEventArgs e)
        {
            if (e.Tab.Index == 0)
            {
                this.Server.Transfer("BaixaDeBensEmLote.aspx");
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
            TituloGrid(grdSelecionado, "Lista de Bens para Baixa em Lote");
        }

        private void ControlarVisibilidadeControle()
        {
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
                pnlGrid.Visible = false;
                if (!this.tseUACedente.DBValue.IsNull)
                {
                    if (this.tseUACedente.IsValidDBValue)
                    {
                        odsPatrimonio.Select();
                        odsPatrimonio.DataBind();
                        grdPatrimonio.DataBind();
                        grdPatrimonio.Selection.UnselectAll();
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

        protected void btnAdicionar_Click(object sender, EventArgs e)
        {
            try
            {
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
                        listaBemId.Add(id);
                }


                foreach (object[] item in fieldValues)
                    if (!listaBemId.Contains(Convert.ToInt32(item[0])))
                        DataTableSelecionado.Rows.Add(item[0].ToString(), item[1].ToString(), item[2].ToString(), item[3].ToString(), item[4].ToString(), item[5].ToString(), item[6].ToString(), item[7].ToString(), item[8].ToString());

                grdSelecionado.DataBind();
                AtualizaQtdItensSelecionados();
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
            var IDBEM = Convert.ToDecimal(grdSelecionado.GetRowValues(e.VisibleIndex, "BEMID"));

            if (e.ButtonID == "btnExcluir")
            {
                try
                {
                    DataRow[] dadosLinha = DataTableSelecionado.Select("BEMID = '" + IDBEM + "'");
                    DataTableSelecionado.Rows.Remove(dadosLinha[0]);
                    grdSelecionado.DataSource = DataTableSelecionado;
                    grdSelecionado.DataBind();
                    AtualizaQtdItensSelecionados();
                }
                catch (Exception ex)
                {
                    Session["Mensagem"] = ex.Message;
                }
            }
        }

        private void CarregaMotivoBaixa()
        {
            RN.Patrimonio.MotivoBaixa rnMotivoBaixa = new Techne.Lyceum.RN.Patrimonio.MotivoBaixa();
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlMotivoBaixa.Items.Clear();
            ddlMotivoBaixa.DataSource = rnMotivoBaixa.ListaMotivoBaixaAtivo();
            ddlMotivoBaixa.DataBind();
            ddlMotivoBaixa.Items.Insert(0, item);
        }

        protected void ddlMotivoBaixa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtProcesso.Text = string.Empty;
                lblBoletim.Visible = false;
                txtBoletimOcorrencia.Visible = false;
                lblCNPJ.Visible = false;
                txtCNPJ.Visible = false;
                lblPrefeitura.Visible = false;
                txtPrefeituraInstituicao.Visible = false;
                txtBoletimOcorrencia.Text = string.Empty;
                txtCNPJ.Text = string.Empty;
                txtPrefeituraInstituicao.Text = string.Empty;
                txtObservacao.Text = string.Empty;

                if (!ddlMotivoBaixa.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (ddlMotivoBaixa.SelectedValue == "2")
                    {
                        lblBoletim.Visible = true;
                        txtBoletimOcorrencia.Visible = true;
                    }
                    if (ddlMotivoBaixa.SelectedValue == "3")
                    {
                        lblCNPJ.Visible = true;
                        txtCNPJ.Visible = true;
                        lblPrefeitura.Visible = true;
                        txtPrefeituraInstituicao.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnEfetuarBaixa_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Patrimonio.Bem rnBem = new Techne.Lyceum.RN.Patrimonio.Bem();
                RN.DTOs.DadosBaixaBemPatrimonial dadosBaixa = new DadosBaixaBemPatrimonial();
                string mensagem = string.Empty;
                int qtdSucesso = 0;
                int qtdErro = 0;

                if (DataTableSelecionado.Rows.Count == 0)
                {
                    mensagem += (!string.IsNullOrEmpty(mensagem) ? Environment.NewLine : "");
                    mensagem += "A lista de Bens para Baixa em Lote está vazia. Não foi dada a baixa de nenhum bem.";
                }

                if (ddlMotivoBaixa.SelectedIndex < 1)
                {
                    mensagem += (!string.IsNullOrEmpty(mensagem) ? Environment.NewLine : "");
                    mensagem += "Campo MOTIVO BAIXA é obrigatório.";
                }

                if (ddlProcessoPrefixo.SelectedIndex < 1)
                {
                    mensagem += (!string.IsNullOrEmpty(mensagem) ? Environment.NewLine : "");
                    mensagem += "Campo PREFIXO do PROCESSO é obrigatório.";
                }

                if (txtProcesso.Text.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagem += (!string.IsNullOrEmpty(mensagem) ? Environment.NewLine : "");
                    mensagem += "Campo PROCESSO é obrigatório.";
                }

                if (dtDataBaixa.Value == null)
                {
                    mensagem += (!string.IsNullOrEmpty(mensagem) ? Environment.NewLine : "");
                    mensagem += "Campo DATA DA BAIXA é obrigatório.";
                }

                if (!mensagem.IsNullOrEmptyOrWhiteSpace())
                {
                    lblMensagem.Text = mensagem.Replace(Environment.NewLine, "<br />") + "<br /><br />";
                    return;
                }

                mensagem = string.Empty;
                foreach (DataRow row in DataTableSelecionado.Rows)
                {
                    mensagem += (!string.IsNullOrEmpty(mensagem) ? Environment.NewLine : "");
                    mensagem += row["NUMERO"].ToString() + " -> ";

                    try
                    {
                        
                        dadosBaixa.BemId = row["BEMID"] != DBNull.Value ? Convert.ToInt32(row["BEMID"].ToString()) : -1;
                        var dadosBemPatrimonial = rnBem.ObtemDadosBemPatrimonialPor(dadosBaixa.BemId);
                        if (dadosBemPatrimonial == null || dadosBemPatrimonial.BemId == -1)
                        {
                            mensagem += "ERRO: Não foi possível obter os dados desse bem patrimonial.";
                            continue;
                        }

                        dadosBaixa.Baixa = true;
                        dadosBaixa.MotivoBaixaId = !ddlMotivoBaixa.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlMotivoBaixa.SelectedValue) : -1;
                        dadosBaixa.ProcessoBaixa = !txtProcesso.Text.IsNullOrEmptyOrWhiteSpace() ? txtProcesso.Text.Trim() : null;
                        dadosBaixa.BoletimOcorrencia = !txtBoletimOcorrencia.Text.IsNullOrEmptyOrWhiteSpace() ? txtBoletimOcorrencia.Text.Trim() : null;
                        dadosBaixa.CnpjInstituicaoDestino = !txtCNPJ.Text.IsNullOrEmptyOrWhiteSpace() ? txtCNPJ.Text.Trim().RetirarMascaraCNPJ() : null;
                        dadosBaixa.InstituicaoDestino = !txtPrefeituraInstituicao.Text.IsNullOrEmptyOrWhiteSpace() ? txtPrefeituraInstituicao.Text.Trim() : null;
                        dadosBaixa.JustificativaBaixa = !txtObservacao.Text.IsNullOrEmptyOrWhiteSpace() ? txtObservacao.Text.Trim() : null;
                        dadosBaixa.DataBaixa = !dtDataBaixa.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataBaixa.Date : DateTime.MinValue;
                        dadosBaixa.UsuarioId = User.Identity.Name;
                        dadosBaixa.PrefixoProcesso = !ddlProcessoPrefixo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlProcessoPrefixo.SelectedValue : null;

                        validacao = rnBem.ValidaBaixa(dadosBaixa, dadosBemPatrimonial.DataAquisicao, dadosBemPatrimonial.DataIncorporacao, dadosBemPatrimonial.Setor);

                        if (validacao.Valido)
                        {
                            rnBem.Baixa(dadosBaixa);

                            if (dadosBaixa.MotivoBaixaId == (int)RN.Patrimonio.MotivoBaixa.EnumMotivoBaixa.Subtraido)
                                mensagem += "SUCESSO: Baixa realizada. Será aberta uma sindicância para averiguação.";
                            else
                                mensagem += "SUCESSO: Baixa realizada.";

                            qtdSucesso++;
                        }
                        else
                        {
                            mensagem += "ERRO: " + validacao.Mensagem.Replace(Environment.NewLine, "; ");
                            qtdErro++;
                        }
                    }
                    catch (Exception rowEx)
                    {
                        mensagem += "ERRO: " + rowEx.Message;
                        qtdErro++;
                    }
                }

                //reset dos campos do formulário de dados da baixa
                grdPatrimonio.DataBind();
                DataTableSelecionado.Rows.Clear();
                grdSelecionado.DataBind();
                AtualizaQtdItensSelecionados();
                ddlMotivoBaixa.SelectedIndex = -1;
                ddlProcessoPrefixo.SelectedIndex = -1;
                txtProcesso.Text = string.Empty;
                dtDataBaixa.Value = null;
                txtObservacao.Text = string.Empty;

                //exibição do sucesso e/ou erro
                if (qtdSucesso > 0)                
                    lblMensagem.Text += qtdSucesso + " bens foram baixados com sucesso<br />";

                if (qtdErro > 0)
                    lblMensagem.Text += qtdErro + " bens retornaram erro ao dar a baixa<br />";

                lblMensagem.Text += "<br />";
                lblMensagemBaixa.Text = mensagem.Replace(Environment.NewLine, "<br />") + "<br /><br />";
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagemBaixa.Text = ex.Message;
            }
        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            //reset das tsearchs
            tseUACedente.Value = null;
            tseClassificacao.Value = null;

            //reset dos grids
            odsPatrimonio.SelectParameters["setor"].DefaultValue = null;
            odsPatrimonio.SelectParameters["conta"].DefaultValue = null;
            odsPatrimonio.DataBind();

            //reset dos campos do formulário de dados da baixa
            DataTableSelecionado.Rows.Clear();
            grdSelecionado.DataBind();
            AtualizaQtdItensSelecionados();
            ddlMotivoBaixa.SelectedIndex = -1;
            ddlProcessoPrefixo.SelectedIndex = -1;
            txtProcesso.Text = string.Empty;
            dtDataBaixa.Value = null;
            txtObservacao.Text = string.Empty;

            //esconder o painel
            pnlGrid.Visible = false;
        }

        private void AtualizaQtdItensSelecionados()
        {
            var qtdItens = grdSelecionado.VisibleRowCount;
            lblMensagemSelecionado.Text = "Existe(m) " + qtdItens + " item(s) na Lista de Bens abaixo.";
            plaMensagemSelecionado.Visible = qtdItens > 0;
        }
    }
}