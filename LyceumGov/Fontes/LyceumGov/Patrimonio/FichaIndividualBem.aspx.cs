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
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;
using Techne.Controls;

namespace Techne.Lyceum.Net.Patrimonio
{
    [NavUrl("~/Patrimonio/FichaIndividualBem.aspx"), ControlText("Inventário Individual"), Title("Inventário Individual")]
    public partial class FichaIndividualBem : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                if (!this.IsPostBack)
                {
                    divPrincipal.Visible = false;
                    pnlImprimir.Visible = false;
                    LimparCampos();
                }
                CarregaBem();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public String ListaSetor(object setor)
        {
            RN.Patrimonio.AgenteResponsavel rnAgenteResponsavel = new Techne.Lyceum.RN.Patrimonio.AgenteResponsavel();
            RN.Setores rnSetor = new Setores();

            if (!string.IsNullOrEmpty(setor.ToString()))
            {
                string codigoSetor = rnSetor.ObtemSetorPor(setor.ToString());
                return codigoSetor;
            }
            return null;
        }

        private void CarregaBem()
        {
            string table = string.Empty;
            Techne.Library.Sql.Structure.SqlSelectColumns coluna = new Techne.Library.Sql.Structure.SqlSelectColumns();
            Techne.Library.Sql.Structure.SqlSelect sqlSelect;
                       

            if (!this.tseUA.DBValue.IsNull && this.tseUA.IsValidDBValue)
            {

                var Setor = ListaSetor(tseUA.DBValue.ToString());                

                table = " Patrimonio.VW_BEMSETOR ";
                tseBem.SqlWhere = " SETOR in ( '" + tseUA.DBValue.ToString() + "' , '" + Setor + "' )";
                //tseBem.SqlWhere = " SETOR = '" + tseUA.DBValue.ToString() + "'";
            }

            coluna.Add("numero");
            coluna.Add("descricao");
            coluna.Add("bemid ");
            coluna.Add("numeroformatado");

            sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);

            tseBem.SqlSelect = sqlSelect;
            tseBem.DataBind();
        }

        protected void tseUA_Changed(object sender, EventArgs args)
        {
            if (Page.IsCallback)
            {
                return;
            }
            try
            {
                pnlImprimir.Visible = false;
                divPrincipal.Visible = false;
                tseBem.ResetValue();

                if (!this.tseUA.DBValue.IsNull)
                {
                    if (!this.tseUA.IsValidDBValue)                   
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
        protected void tseBem_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                RN.Patrimonio.Bem rnBem = new Techne.Lyceum.RN.Patrimonio.Bem();
                RN.DTOs.DadosFichaIndividualBem dadosBem = new Techne.Lyceum.RN.DTOs.DadosFichaIndividualBem();
                LimparCampos();
                divPrincipal.Visible = false;
                pnlImprimir.Visible = false;

                if (!this.tseBem.DBValue.IsNull)
                {
                    if (this.tseBem.IsValidDBValue)
                    {
                        var Setor = ListaSetor(tseUA.DBValue.ToString());
                        dadosBem = rnBem.ObtemFichaIndividualBem(tseUA.DBValue.ToString(), tseBem.DBValue.ToString(), Setor);

                        if (dadosBem.BemId > 0)
                        {
                            divPrincipal.Visible = true;
                            lblSetor.InnerHtml = dadosBem.UnidadeAdministrativa;
                            lblOrgao.InnerHtml = dadosBem.UnidadeAdministrativa.ToUpper();
                            lblIdentificacao.InnerHtml = dadosBem.Descricao;
                            lblInventario.InnerHtml = dadosBem.Numero;
                            lblClassificacao.InnerHtml = dadosBem.Conta;
                            lblDataIncorporacao.InnerHtml = String.Format("{0:dd/MM/yyyy}", dadosBem.DataIncorporacao);
                            lblOperacao.InnerHtml = dadosBem.Operacao;
                            lblDocumento.InnerHtml = dadosBem.DocumentoHabil;
                            lblHistorico.InnerHtml = dadosBem.Historico;
                            lblValor.InnerHtml = dadosBem.Sigla + " " + string.Format("{0:N2}", dadosBem.ValorInicial);

                            lblRodape.InnerHtml = String.Format("{0} {1} {2} {3}", dadosBem.UnidadeAdministrativa, dadosBem.EnderecoUnidadeAdministrativa, dadosBem.TelefoneUnidadeAdministrativa, dadosBem.EmailUnidadeAdministrativa);

                            pnlImprimir.Visible = true;
                        }

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

        private void LimparCampos()
        {
            lblSetor.InnerHtml = string.Empty;
            lblOrgao.InnerHtml = string.Empty;
            lblIdentificacao.InnerHtml = string.Empty;
            lblInventario.InnerHtml = string.Empty;
            lblDataIncorporacao.InnerHtml = string.Empty;
            lblOperacao.InnerHtml = string.Empty;
            lblDocumento.InnerHtml = string.Empty;
            lblHistorico.InnerHtml = string.Empty;
            lblValor.InnerHtml = string.Empty;
            lblRodape.InnerHtml = string.Empty; 

        }

        protected void btnExportarPDF_Click(object sender, ImageClickEventArgs e)
        {
            RN.Util.ExportaPdf exportaPdf = new ExportaPdf();

            try
            {
                //Verifica se dados para exportar já estão montados na tela
                if (divPrincipal.Visible)
                {
                    Image1.Src = HttpContext.Current.Server.MapPath("~/Images/logo_govrj.jpg");
                    Image1.Align = "center";
                    //Cria arquivo com div
                    StringBuilder html = new StringBuilder();
                    StringWriter stringWriter = new StringWriter(html);
                    HtmlTextWriter writer = new HtmlTextWriter(stringWriter);
                    divPrincipal.RenderControl(writer);
                    exportaPdf.ExportaHtmlSimplesOrientacaoPaisagemPor(html.ToString(), "INVENTARIOINDIVIDUAL_" + tseBem.DBValue.ToString());
                }
                else
                {
                    lblMensagem.Text = "Não existem dados à serem exportados.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
