using System;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Web;
using System.Data;

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
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdDocenteFuncaoGLP);
            ControlaAcesso(grdDocenteFuncaoGLP, AcaoControle.editar, "btnReprovar");
            ControlaAcesso(grdDocenteFuncaoGLP, AcaoControle.editar, "btnAceitar");
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
            RetValue retorno = null;
            RN.DocenteGLP rnDocenteGLP = new DocenteGLP();

            try
            {
                if (rnDocenteGLP.ehSolicitacaoPendente(Convert.ToDecimal(hID.Text)))
                {
                    retorno = RN.DocenteGLP.ReprovarGLPEmAnalise(Convert.ToDecimal(hID.Text), Convert.ToDecimal(hQtde.Text), Convert.ToString(cmbMotivo.SelectedValue),User.Identity.Name);
                    if (retorno != null)
                    {
                        if (!retorno.Ok)
                        {
                            throw new Exception(retorno.Errors.ToString());
                        }
                        else
                        {
                            lblMensagem.Text = retorno.Message;
                            odsDocenteFuncaoGLP.Select();
                            odsDocenteFuncaoGLP.DataBind();
                            grdDocenteFuncaoGLP.DataBind();
                        }
                    }
                }
                else
                {
                    lblMensagem.Text = "Esta solicitação já se encontra analisada.";
                    odsDocenteFuncaoGLP.Select();
                    odsDocenteFuncaoGLP.DataBind();
                    grdDocenteFuncaoGLP.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }     

        protected void grdDocenteFuncaoGLP_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            RN.DocenteGLP rnDocenteGLP = new DocenteGLP();
            Techne.Lyceum.CR.Ly_docente_funcao_glp.Row row = new Techne.Lyceum.CR.Ly_docente_funcao_glp().NewRow();
            row.Id_docente_funcao_glp = Convert.ToDecimal(grdDocenteFuncaoGLP.GetRowValues(e.VisibleIndex, "ID_DOCENTE_FUNCAO_GLP"));
            row.Matricula = Convert.ToString(grdDocenteFuncaoGLP.GetRowValues(e.VisibleIndex, "MATRICULA"));
            row.Funcao_glp = Convert.ToString(grdDocenteFuncaoGLP.GetRowValues(e.VisibleIndex, "FUNCAO_GLP"));
            row.Unidade_ens = Convert.ToString(grdDocenteFuncaoGLP.GetRowValues(e.VisibleIndex, "UNIDADE_ENS"));
            row.Agrupamento = Convert.ToString(grdDocenteFuncaoGLP.GetRowValues(e.VisibleIndex, "AGRUPAMENTO"));
            row.Glp_solicitada = Convert.ToInt32(grdDocenteFuncaoGLP.GetRowValues(e.VisibleIndex, "GLP_SOLICITADA"));
            row.Ano = DateTime.Today.Year;
            row.Mes = DateTime.Today.Month;
            row.Data = DateTime.Today;
            row.Usuarioanaliseid = User.Identity.Name;
            row.DataAnalise = DateTime.Today;
            

            RetValue retorno = null;

            if (e.ButtonID == "btnReprovar")
            {
                pucConfirmar.ShowOnPageLoad = true;
                hID.Text = Convert.ToString(row.Id_docente_funcao_glp);
                hQtde.Text = Convert.ToString(row.Glp_solicitada);
            }
            else if (e.ButtonID == "btnAceitar")
            {
                try
                {
                    if (rnDocenteGLP.ehSolicitacaoPendente(Convert.ToDecimal(row.Id_docente_funcao_glp)))
                    {
                        retorno = RN.DocenteGLP.AceitarGLP(row);
                        if (retorno != null)
                        {
                            if (!retorno.Ok)
                            {
                                throw new Exception(retorno.Errors.ToString());
                            }
                            else
                            {
                                lblMensagem.Text = retorno.Message;
                                odsDocenteFuncaoGLP.Select();
                                odsDocenteFuncaoGLP.DataBind();
                                grdDocenteFuncaoGLP.DataBind();
                            }
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Esta solicitação já se encontra analisada.";
                        odsDocenteFuncaoGLP.Select();
                        odsDocenteFuncaoGLP.DataBind();
                        grdDocenteFuncaoGLP.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    lblMensagem.Text = ex.Message;
                }
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
                    pnlAnaliseGLP.Visible = true;
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
    }
}


