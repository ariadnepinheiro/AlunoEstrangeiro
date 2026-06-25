using System;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Web;
using System.Data;
using DevExpress.Web.ASPxEditors;
using System.Collections.Generic;
using Techne.Lyceum.CR;
using System.Linq;
using Techne.Library;

namespace Techne.Lyceum.Net.Basico
{
    [
     NavUrl("~/Basico/AnaliseGLP.aspx"),
      ControlText("AnaliseGLP"),
      Title("Análise de Pedido GLP"),
    ]

    public partial class AnaliseGLP : TPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdDocenteFuncaoGLP, "Análise de Pedidos GLP");
            TituloGrid(grdTurmasPedido, string.Empty);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
            pucConfirmar.ShowOnPageLoad = false;
            divBotoes.Visible = grdDocenteFuncaoGLP.VisibleRowCount > 0;
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdDocenteFuncaoGLP);
            ControlaAcesso(btnAceitarTodos, AcaoControle.editar);
            ControlaAcesso(btnReprovarTodos, AcaoControle.editar);
        }

        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
        }
        #endregion

        protected void grdDocenteFuncaoGLP_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdDocenteFuncaoGLP);
        }

        public object Listar(DbObject id_regional, DbObject unidade_ensino)
        {
            DataTable qt = null;
            RN.DocenteGLP rnDocenteGLP = new DocenteGLP();

            if (!id_regional.IsNull && !String.IsNullOrEmpty(Convert.ToString(id_regional)))
            {
                qt = rnDocenteGLP.ListaAnaliseDocenteGLPPor(Convert.ToInt32(id_regional), Convert.ToString(unidade_ensino));
            }

            return qt;
        }

        public object ListarTurmaPedido(object id_docente_funcao_glp)
        {
            RN.RecursosHumanos.DocenteFuncaoGlpTurma rnDocenteFuncaoGlpTurma = new Techne.Lyceum.RN.RecursosHumanos.DocenteFuncaoGlpTurma();

            var id = id_docente_funcao_glp != null ? id_docente_funcao_glp.ToString() : null;

            if (!string.IsNullOrEmpty(id))
            {
                return rnDocenteFuncaoGlpTurma.ListaPor(Convert.ToInt32(id));
            }
            return null;
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            pnlAnaliseGLP.Visible = false;

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
                            tseUnidade_Ensino.ResetValue();
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
                        tseUnidade_Ensino.ResetValue();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            pnlAnaliseGLP.Visible = false;

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
                            tseUnidade_Ensino.ResetValue();
                        }
                    }
                    else
                    {
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                        tseUnidade_Ensino.ResetValue();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidade_Ensino_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            pnlAnaliseGLP.Visible = false;

            try
            {
                if (Page.IsCallback)
                    return;


                var sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if ((!tseUnidade_Ensino.DBValue.IsNull && tseUnidade_Ensino.IsValidDBValue) && (tseMunicipio.DBValue.IsNull || tseRegional.DBValue.IsNull))
                {
                    tseRegional.Value = tseUnidade_Ensino["id_regional"];
                    tseMunicipio.Value = tseUnidade_Ensino["municipio"];

                    sessao.Regional = Convert.ToString(tseRegional.DBValue);
                    sessao.Municipio = Convert.ToString(tseMunicipio.DBValue);
                    sessao.Escola = Convert.ToString(tseUnidade_Ensino.DBValue);

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdDocenteFuncaoGLP_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            if (Session["Mensagem"] == null)
                e.Properties["cpMessage"] = String.Empty;
            else
            {
                e.Properties["cpMessage"] = Session["Mensagem"].ToString();
                Session["Mensagem"] = null;
            }
        }

        protected void btConfirma_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hID.Text)) return;

            RN.DocenteGLP rnDocenteGLP = new RN.DocenteGLP();
            string[] idValues = hID.Text.Split(';');
            string[] qtdValues = hQtde.Text.Split(';');
            string motivo = cmbMotivo.SelectedValue;

            try
            {
                for (int i = 0; i < idValues.Length; i++)
                {
                    if (i >= qtdValues.Length) break;

                    decimal id = Convert.ToDecimal(idValues[i]);
                    decimal qtde = Convert.ToDecimal(qtdValues[i]);

                    if (rnDocenteGLP.ehSolicitacaoPendente(id))
                    {
                        var retorno = RN.DocenteGLP.ReprovarGLPEmAnalise(id, qtde, motivo, User.Identity.Name);

                        if (retorno != null && !retorno.Ok)
                        {
                            throw new Exception("Erro no item {id}: {retorno.Errors}");
                        }

                        // Acumulamos a mensagem se necessário, ou pegamos a última
                        lblMensagem.Text = retorno.Message ?? "Processado com sucesso.";
                    }
                    else
                    {
                        lblMensagem.Text = "Algumas solicitações já haviam sido analisadas.";
                    }
                }

                odsDocenteFuncaoGLP.Select();
                grdDocenteFuncaoGLP.DataBind();
                pucConfirmar.ShowOnPageLoad = false; // Fecha o popup após concluir
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Erro: " + ex.Message;
            }
        }

        protected void btnPesquisar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            pnlAnaliseGLP.Visible = false;

            try
            {
                if (tseRegional.IsValidDBValue && !tseRegional.DBValue.IsNull)
                {
                    DocenteGLP.RecalcularCHLivreGLP(Convert.ToString(tseRegional.Value), Convert.ToString(tseUnidade_Ensino.Value));
                    grdDocenteFuncaoGLP.Visible = true;
                    grdDocenteFuncaoGLP.DataBind();
                    grdDocenteFuncaoGLP.Selection.UnselectAll();

                    pnlAnaliseGLP.Visible = true;
                    divBotoes.Visible = grdDocenteFuncaoGLP.VisibleRowCount > 0;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnDetalhes_Command(object sender, CommandEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(e.CommandArgument);
                string codigoSolicitacao = grdDocenteFuncaoGLP.GetRowValuesByKeyValue(id, "ID_DOCENTE_FUNCAO_GLP").ToString();

                txtRow.Value = codigoSolicitacao;

                pucTurmaPedido.ShowOnPageLoad = true;

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

            divBotoes.Visible = grdDocenteFuncaoGLP.VisibleRowCount > 0;
        }

        protected void grdDocenteFuncaoGLP_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "SEGMENTO")
            {
                string valor = e.Value.ToString();
                if (!string.IsNullOrEmpty(valor))
                {
                    if (valor.Contains("DOC II"))
                    {
                        e.DisplayText = "Ensino Fundamental Anos Iniciais";
                    }
                    else if (valor.Contains("DOC I"))
                    {
                        e.DisplayText = "Ensino Fundamental Anos Finais / Ensino Médio";
                    }
                }
            }
        }

        /// <summary>
        /// Método responsável por selecionar os checkboxs da grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkSelecionarTodos_CheckedChanged(object sender, EventArgs e)
        {
            ASPxCheckBox chk = (ASPxCheckBox)sender;

            if (chk.Checked)
            {
                grdDocenteFuncaoGLP.Selection.SelectAll();
            }
            else
            {
                grdDocenteFuncaoGLP.Selection.UnselectAll();
            }
        }

        protected void btnAcaoTodos_Command(object sender, CommandEventArgs e)
        {
            try
            {
                string acao = e.CommandArgument.ToString(); // "A" para Aceitar, "R" para Reprovar

                // Lista de IDs das linhas selecionadas
                List<object> keyValues = grdDocenteFuncaoGLP.GetSelectedFieldValues("ID_DOCENTE_FUNCAO_GLP");

                if (keyValues.Count == 0)
                {
                    lblMensagem.Text = "Selecione ao menos um item para aprovar ou reprovar.";
                }
                else
                {
                    AprovarReprovarSelecionados(acao, keyValues);

                    //Atualiza o grid após a ação
                    grdDocenteFuncaoGLP.DataBind();
                    grdDocenteFuncaoGLP.Selection.UnselectAll();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

            divBotoes.Visible = grdDocenteFuncaoGLP.VisibleRowCount > 0;
        }

        private void AprovarReprovarSelecionados(string acao, List<object> selecionados)
        {
            lblMensagem.ForeColor = System.Drawing.Color.Red;

            RN.DocenteGLP rnDocenteGLP = new DocenteGLP();
            List<string> lista = selecionados.ConvertAll(x => x.ToString());
            try
            {
                if (acao == "R")
                {
                    pucConfirmar.ShowOnPageLoad = true;

                    string ids = string.Join(";", lista.ToArray());
                    hID.Text = ids;

                    hQtde.Text = string.Join(";", lista.Select(key =>
                        grdDocenteFuncaoGLP.GetRowValuesByKeyValue(key, "GLP_SOLICITADA").ToString() ?? ""
                    ).ToArray()).ToString();
                }
                else if (acao == "A")
                {
                    string msgCompleta = string.Empty;
                    int qtdSucesso = 0;
                    int qtdSemCarencia = 0;
                    int qtdFalha = 0;


                    RetValue retorno = null;

                    foreach (object key in selecionados)
                    {
                        try
                        {
                            decimal idSelecionado = Convert.ToDecimal(key);
                            Ly_docente_funcao_glp.Row row = new Ly_docente_funcao_glp().NewRow();

                            row.Id_docente_funcao_glp = Convert.ToDecimal(idSelecionado);
                            row.Matricula = grdDocenteFuncaoGLP.GetRowValuesByKeyValue(idSelecionado, new string[] { "MATRICULA" }).ToString();
                            row.Funcao_glp = grdDocenteFuncaoGLP.GetRowValuesByKeyValue(idSelecionado, new string[] { "FUNCAO_GLP" }).ToString();
                            row.Unidade_ens = grdDocenteFuncaoGLP.GetRowValuesByKeyValue(idSelecionado, new string[] { "UNIDADE_ENS" }).ToString();
                            row.Agrupamento = grdDocenteFuncaoGLP.GetRowValuesByKeyValue(idSelecionado, new string[] { "AGRUPAMENTO" }).ToString();
                            row.Glp_solicitada = Convert.ToDecimal(grdDocenteFuncaoGLP.GetRowValuesByKeyValue(idSelecionado, new string[] { "GLP_SOLICITADA" }));
                            row.Ano = DateTime.Today.Year;
                            row.Mes = DateTime.Today.Month;
                            row.Data = DateTime.Today;
                            row.Usuarioanaliseid = User.Identity.Name;
                            row.DataAnalise = DateTime.Today;

                            if (rnDocenteGLP.ehSolicitacaoPendente(Convert.ToDecimal(idSelecionado)))
                            {
                                retorno = RN.DocenteGLP.AceitarGLP(row);
                                if (retorno != null)
                                {
                                    if (!retorno.Ok)
                                    {
                                        qtdSemCarencia++;
                                    }
                                    else
                                    {
                                        qtdSucesso++;

                                    }
                                }
                            }
                            else
                            {
                                retorno = new RetValue(false, "", new ErrorList("Esta solicitação já foi analisada."));
                            }
                        }
                        catch (Exception)
                        {
                            qtdFalha++;
                        }
                    }

                    // Atualiza o grid apenas UMA VEZ após o loop todo
                    odsDocenteFuncaoGLP.Select();
                    odsDocenteFuncaoGLP.DataBind();
                    grdDocenteFuncaoGLP.DataBind();

                    //"Esta solicitação já se encontra analisada.";
                    msgCompleta = "<br /><b>Resumo do Processamento de GLP:</b><br /><br />";

                    // Corrigido os emojis de alerta/erro e adicionado espaçamento antes do número
                    msgCompleta += string.Format("Pedidos selecionados: {0}<br />", selecionados.Count);
                    msgCompleta += string.Format("Pedidos de GLP aprovada(s): {0}<br />", qtdSucesso);
                    msgCompleta += string.Format("Disciplina sem carência disponível: {0}<br />", qtdSemCarencia);

                    msgCompleta += string.Format("Falha(s): {0}<br /><br />", qtdFalha);

                    lblMensagem.Text = msgCompleta;

                    if (!retorno.Ok)
                    {
                        lblMensagem.Text += "<br /><br />" + retorno.Message;
                    }

                    if (qtdSemCarencia == 0 && qtdFalha == 0)
                    {
                        lblMensagem.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblMensagem.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text += "<br /><br />" + ex.Message;
            }


            divBotoes.Visible = grdDocenteFuncaoGLP.VisibleRowCount > 0;
        }
    }
}


