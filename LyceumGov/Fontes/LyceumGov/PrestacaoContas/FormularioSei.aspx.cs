using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using System.Data;
using Techne.Web;
using DevExpress.Web.ASPxGridView;
using System.Web.UI.HtmlControls;
using System.Text;
using DevExpress.Web.ASPxEditors;
using Techne.Controls;
using System.Threading;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.css;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.pipeline.html;
using iTextSharp.tool.xml.pipeline.end;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [
     NavUrl("~/PrestacaoContas/FormularioSei.aspx"),
      ControlText("FormularioSei"),
      Title("Formulário Sei"),
    ]
    public partial class FormularioSei : TPage
    {
        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnImportar, AcaoControle.novo);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
        }
        protected void tseUnidadeResponsavel_Changed(object sender, EventArgs args)
        {
            try
            {
                RN.PrestacaoContas.ImportacaoSei rnImportacaoSei = new Techne.Lyceum.RN.PrestacaoContas.ImportacaoSei();
                lblMensagem.Text = string.Empty;
                lblUltimaGeracao.Text = string.Empty;
                DateTime data = new DateTime();


                if ((!tsePeriodoReferencia.DBValue.IsNull && tsePeriodoReferencia.IsValidDBValue) && (!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue))
                {
                    data = rnImportacaoSei.ObtemUltimaImportacaoPor(Convert.ToInt32(tsePeriodoReferencia.DBValue), Convert.ToString(tseUnidadeResponsavel.DBValue));

                    if (data != DateTime.MinValue)
                    {
                        lblUltimaGeracao.Text = "SEI gerado em " + data.ToShortDateString();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }
        protected void tsePeriodoReferencia_Changed(object sender, EventArgs args)
        {
            try
            {
                RN.PrestacaoContas.ImportacaoSei rnImportacaoSei = new Techne.Lyceum.RN.PrestacaoContas.ImportacaoSei();
                lblMensagem.Text = string.Empty;
                lblUltimaGeracao.Text = string.Empty;
                DateTime data = new DateTime();


                if ((!tsePeriodoReferencia.DBValue.IsNull && tsePeriodoReferencia.IsValidDBValue) && (!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue))
                {
                    data = rnImportacaoSei.ObtemUltimaImportacaoPor(Convert.ToInt32(tsePeriodoReferencia.DBValue), Convert.ToString(tseUnidadeResponsavel.DBValue));

                    if (data != DateTime.MinValue)
                    {
                        lblUltimaGeracao.Text = "SEI gerado em " + data.ToShortDateString();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        public ElementList ParseToElementList(string html, string css)
        {
            // CSS
            ICSSResolver cssResolver = new StyleAttrCSSResolver();
            if (css != null)
            {
                ICssFile cssFile = XMLWorkerHelper.GetCSS(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(css)));
                cssResolver.AddCss(cssFile);
            }

            // HTML
            CssAppliers cssAppliers = new CssAppliersImpl(FontFactory.FontImp);
            HtmlPipelineContext htmlContext = new HtmlPipelineContext(cssAppliers);
            htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());
            htmlContext.AutoBookmark(false);

            // Pipelines
            ElementList elements = new ElementList();
            ElementHandlerPipeline end = new ElementHandlerPipeline(elements, null);
            HtmlPipeline htmlPipeline = new HtmlPipeline(htmlContext, end);
            CssResolverPipeline cssPipeline = new CssResolverPipeline(cssResolver, htmlPipeline);

            // XML Worker
            XMLWorker worker = new XMLWorker(cssPipeline, true);
            XMLParser p = new XMLParser(worker);
            p.Parse(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(html)));

            return elements;
        }

        protected void btnImportar_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
            RN.PrestacaoContas.FormularioSei rnFormularioSei = new Techne.Lyceum.RN.PrestacaoContas.FormularioSei();
            RN.Util.ExportaPdf exportaPdf = new ExportaPdf();
            ValidacaoDados validacao = new ValidacaoDados();
            string nomeArquivo = "FormularioSei1";
            iTextSharp.text.Document docPdf = null;

            try
            {
                string censo = !tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue ? tseUnidadeResponsavel.DBValue.ToString() : string.Empty;
                int periodoReferenciaId = !tsePeriodoReferencia.DBValue.IsNull && tsePeriodoReferencia.IsValidDBValue ? Convert.ToInt32(tsePeriodoReferencia.DBValue) : -1;

                validacao = rnFormularioSei.Valida(censo, periodoReferenciaId);

                if (validacao.Valido)
                {
                    CarregaFormularioI(censo, periodoReferenciaId);
                    CarregaFormularioII(censo, periodoReferenciaId);
                    CarregaFormularioIII(censo, periodoReferenciaId);
                   // CarregaFormularioIV(censo, periodoReferenciaId);
                    CarregaFormularioV(censo, periodoReferenciaId);

                    //Cria arquivo com div
                    StringBuilder html = new StringBuilder();
                    StringWriter stringWriter = new StringWriter(html);
                    HtmlTextWriter writer = new HtmlTextWriter(stringWriter);
                    divPrincipal.Visible = true;
                    divPrincipal.RenderControl(writer);

                    try
                    {
                        HttpResponse response = HttpContext.Current.Response;
                        response.ContentType = "application/pdf";
                        response.AddHeader("content-disposition", string.Format("attachment;filename={0}.pdf", nomeArquivo));
                        response.Cache.SetCacheability(HttpCacheability.NoCache);

                        //Cria papel com suas propriedades
                        iTextSharp.text.Rectangle papel;

                        papel = PageSize.A4;

                        //Cria documento
                        docPdf = new iTextSharp.text.Document(papel, 10, 10, 10, 0);

                        //Monta pdf
                        PdfWriter.GetInstance(docPdf, response.OutputStream);
                        docPdf.Open();

                        //Formulario1
                        StringBuilder html1 = new StringBuilder();
                        StringWriter stringWriter1 = new StringWriter(html1);
                        HtmlTextWriter writer1 = new HtmlTextWriter(stringWriter1);
                        divPrincipal.Visible = true;
                        divPrincipal.RenderControl(writer1);
                        var parsedHtmlElements1 = ParseToElementList(html1.ToString(), null);

                        foreach (var htmlElement in parsedHtmlElements1)
                        {
                            docPdf.Add(htmlElement as IElement);
                        }

                        docPdf.NewPage(); //adiciona uma nova pagina
                        divPrincipal.Visible = false;

                        //Formulario2
                        StringBuilder html2 = new StringBuilder();
                        StringWriter stringWriter2 = new StringWriter(html2);
                        HtmlTextWriter writer2 = new HtmlTextWriter(stringWriter2);
                        divFormularioSeiII.Visible = true;
                        divFormularioSeiII.RenderControl(writer2);
                        var parsedHtmlElements2 = ParseToElementList(html2.ToString(), null);

                        foreach (var htmlElement in parsedHtmlElements2)
                        {
                            docPdf.Add(htmlElement as IElement);
                        }
                        docPdf.NewPage(); //adiciona uma nova pagina
                        divFormularioSeiII.Visible = false;


                        //Formulario3
                        StringBuilder html3 = new StringBuilder();
                        StringWriter stringWriter3 = new StringWriter(html3);
                        HtmlTextWriter writer3 = new HtmlTextWriter(stringWriter3);
                        divFormularioSeiIII.Visible = true;
                        divFormularioSeiIII.RenderControl(writer3);
                        var parsedHtmlElements3 = ParseToElementList(html3.ToString(), null);

                        foreach (var htmlElement in parsedHtmlElements3)
                        {
                            docPdf.Add(htmlElement as IElement);
                        }
                        docPdf.NewPage(); //adiciona uma nova pagina
                        divFormularioSeiIII.Visible = false;

                        //Formulario4

                        List<RN.PrestacaoContas.DTOs.DadosFormulario2> dadosFormulario2C = new List<RN.PrestacaoContas.DTOs.DadosFormulario2>();
                        dadosFormulario2C = CarregaFormularioIV(censo, periodoReferenciaId);

                        foreach (RN.PrestacaoContas.DTOs.DadosFormulario2 dado in dadosFormulario2C)
                        {
                            MontaFormularioIV(dado);

                            StringBuilder html4 = new StringBuilder();
                            StringWriter stringWriter4 = new StringWriter(html4);
                            HtmlTextWriter writer4 = new HtmlTextWriter(stringWriter4);
                            divformularioSeiIV.Visible = true;
                            divformularioSeiIV.RenderControl(writer4);
                            var parsedHtmlElements4 = ParseToElementList(html4.ToString(), null);

                            foreach (var htmlElement in parsedHtmlElements4)
                            {
                                docPdf.Add(htmlElement as IElement);
                            }
                            docPdf.NewPage(); //adiciona uma nova pagina
                            divformularioSeiIV.Visible = false;
                        }

                        //Formulario5
                        StringBuilder html5 = new StringBuilder();
                        StringWriter stringWriter5 = new StringWriter(html5);
                        HtmlTextWriter writer5 = new HtmlTextWriter(stringWriter5);
                        divFormularioSeiV.Visible = true;
                        divFormularioSeiV.RenderControl(writer5);
                        var parsedHtmlElements5 = ParseToElementList(html5.ToString(), null);

                        foreach (var htmlElement in parsedHtmlElements5)
                        {
                            docPdf.Add(htmlElement as IElement);
                        }
                        docPdf.NewPage(); //adiciona uma nova pagina
                        divFormularioSeiV.Visible = false;

                        //Formulario6
                        List<RN.PrestacaoContas.DTOs.DadosFormularioFornecedor> dadosFornecedores = new List<RN.PrestacaoContas.DTOs.DadosFormularioFornecedor>();
                        dadosFornecedores = CarregaFormularioVI(censo, periodoReferenciaId);

                        foreach (RN.PrestacaoContas.DTOs.DadosFormularioFornecedor dado in dadosFornecedores)
                        {
                            MontaFormularioVI(dado.DiretoriaRegional, dado.MunicipioAtendidos, dado.Cnpj, dado.RazaoSocial, dado.InscricaoEstadual, dado.InscricaoMunicipal, dado.Endereco, dado.ComplementoEndereco, dado.Bairro, dado.UF, dado.Municipio, dado.Cep, dado.CaixaPostal, dado.DDD, dado.Telefone, dado.Email, dado.Nome, dado.Cpf);

                            StringBuilder html6 = new StringBuilder();
                            StringWriter stringWriter6 = new StringWriter(html6);
                            HtmlTextWriter writer6 = new HtmlTextWriter(stringWriter6);
                            divFormularioSeiVI.Visible = true;
                            divFormularioSeiVI.RenderControl(writer6);
                            var parsedHtmlElements6 = ParseToElementList(html6.ToString(), null);



                            foreach (var htmlElement in parsedHtmlElements6)
                            {
                                docPdf.Add(htmlElement as IElement);
                            }
                            divFormularioSeiVI.Visible = false;
                            docPdf.NewPage(); //adiciona uma nova pagina
                        }

                        docPdf.Close();

                        //Grava geração de formulario
                        rnFormularioSei.Insere(censo, periodoReferenciaId, User.Identity.Name);
                    }
                    catch (Exception ex)
                    {
                        string mensagem = string.Format("Erro ao gerar pdf : {0}", ex.Message);
                        throw new Exception(mensagem);
                    }

                    divPrincipal.Visible = false;

                    CarregaFormularioI(censo, periodoReferenciaId);
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                if (docPdf != null)
                {
                    docPdf.Close();
                }
                lblMensagem.Text = string.Empty;
                lblMensagem.Text = ex.Message;
            }
        }



        public void CarregaFormularioI(string censo, int periodoReferenciaId)
        {
            try
            {       

                RN.PrestacaoContas.DTOs.DadosFormulario1 formulario1 = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosFormulario1();
                RN.PrestacaoContas.FormularioSei rnFormularioSei = new Techne.Lyceum.RN.PrestacaoContas.FormularioSei();

                //Busca dados do formulario
                formulario1 = rnFormularioSei.ObtemDadosFormulario1Por(censo, periodoReferenciaId);

                lblNomeAae.Text = formulario1.NomeAae;
                lblCnpj.Text = formulario1.Cnpj;
                lblEndereco.Text = formulario1.Endereco;
                lblComplemento.Text = formulario1.Complemento;
                lblBairro.Text = formulario1.Bairro;
                lblMunicipio.Text = formulario1.Municipio;
                lblCep.Text = formulario1.Cep;
                lblDdd.Text = formulario1.Ddd;
                lblTelefone.Text = formulario1.Telefone;
                lblEmailInstitucional.Text = formulario1.EmailInstituicional;
                lblRegional.Text = formulario1.DiretoriaRegional;
                lblFundamental.Text = formulario1.FundamentalModalidade;
                lblFundamentalNumeroAlunos.Text = formulario1.FundamentalNumeroAlunos;
                lblFundamentalNumeroTurnos.Text = formulario1.FundamentalNumeroTurnos;
                lblFundamentalIntegralSim.Text = formulario1.FundamentalHorarioIntegralSim;
                lblFundamentalIntegralNao.Text = formulario1.FundamentalHorarioIntegralNao;
                lblMedio.Text = formulario1.MedioModalidade;
                lblMedioNumeroAlunos.Text = formulario1.MedioNumeroAlunos;
                lblMedioNumeroTurnos.Text = formulario1.MedioNumeroTurnos;
                lblMedioIntegralSim.Text = formulario1.MedioHorarioIntegralSim;
                lblMedioIntegralNao.Text = formulario1.MedioHorarioIntegralNao;
                lblEja.Text = formulario1.EjaModalidade;
                lblEjaNumeroAlunos.Text = formulario1.EjaNumeroAlunos;
                lblEjaNumeroTurnos.Text = formulario1.EjaNumeroTurnos;
                lblEjaIntegralSim.Text = formulario1.EjaHorarioIntegralSim;
                lblEjaIntegralNao.Text = formulario1.EjaHorarioIntegralNao;
                lblEducacaoEspecial.Text = formulario1.EducacaoEspecialModalidade;
                lblEducacaoEspecialNumeroAlunos.Text = formulario1.EducacaoEspecialNumeroAlunos;
                lblEducacaoEspecialNumeroTurnos.Text = formulario1.EducacaoEspecialNumeroTurnos;
                lblEducacaoEspecialIntegralSim.Text = formulario1.EducacaoEspecialHorarioIntegralSim;
                lblEducacaoEspecialIntegralNao.Text = formulario1.EducacaoEspecialHorarioIntegralNao;
                lblDiretorNome.Text = formulario1.DiretorFimNome;
                lblDiretorCpf.Text = formulario1.DiretorFimCpf;
                lblDiretorMatricula.Text = formulario1.DiretorFimMatricula;
                lblDiretorId.Text = formulario1.DiretorFimIdFuncional;
                lblDiretorDataDo.Text = formulario1.DiretorFimDataDO;
                lblTesoureiroNome.Text = formulario1.TesoureiroFimNome;
                lblTesoureiroCpf.Text = formulario1.TesoureiroFimCpf;
                lblTesoureiroMatricula.Text = formulario1.TesoureiroFimMatricula;
                lblTesoureiroId.Text = formulario1.TesoureiroFimIdFuncional;

                //Inicio Periodo

                lblDiretorNomeInicio.Text = formulario1.DiretorInicioNome;
                lblDiretorCpfInicio.Text = formulario1.DiretorInicioCpf;
                lblDiretorMatriculaInicio.Text = formulario1.DiretorInicioMatricula;
                lblDiretorIdInicio.Text = formulario1.DiretorInicioIdFuncional;
                lblDiretorDataDoInicio.Text = formulario1.DiretorInicioDataDO;
                lblTesoureiroNomeInicio.Text = formulario1.TesoureiroInicioNome;
                lblTesoureiroCpfInicio.Text = formulario1.TesoureiroInicioCpf;
                lblTesoureiroMatriculaInicio.Text = formulario1.TesoureiroInicioMatricula;
                lblTesoureiroIdInicio.Text = formulario1.TesoureiroInicioIdFuncional;


                lblBanco.Text = formulario1.Banco;
                lblAgencia.Text = formulario1.Agencia;
                lblContaCorrente.Text = formulario1.ContaCorrente;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public void CarregaFormularioII(string censo, int periodoReferenciaId)
        {
            string html = string.Empty;
            RN.PrestacaoContas.DTOs.DadosFormulario2 formulario = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosFormulario2();
            RN.PrestacaoContas.FormularioSei rnFormularioSei = new Techne.Lyceum.RN.PrestacaoContas.FormularioSei();

            try
            {
                //Busca dados do formulario
                formulario = rnFormularioSei.ObtemDadosFormulario2APor(censo, periodoReferenciaId);

                //Adiciona campos fixos
                lblfrm2PeriodoPrestacao.Text = formulario.PeriodoPrestacao;
                lblfrm2SaldoAnterior.Text = formulario.SaldoAnterior;
                lblfrm2RepasseRecebido.Text = formulario.RepassesRecebidos;
                lblfrm2CreditosDebitos.Text = formulario.CreditosDebitos;
                lblfrm2SaldoInicial.Text = formulario.SaldoInicial;
                lblfrm2SaldoFinal.Text = formulario.SaldoFinal;

                //Adiciona cabeçalho da tabela de despesas
                html = "<table border=\"1px\" width=\"100%\" style=\"font-weight: normal; font-family: Calibri;";
                html += " font-size: 12px; border-spacing: 0; border-collapse: collapse; border: 1px solid #000000\">";
                html += "      <tr style=\"height: 20px\">";
                html += "         <td style=\"width: 130px\">";
                html += "             &nbsp;5 - Evento";
                html += "          </td>";
                html += "         <td>";
                html += "             &nbsp;6 - Fornecedor/Beneficiário";
                html += "         </td>";
                html += "         <td style=\"width: 100px\">";
                html += "             &nbsp;7 - Documento Fiscal";
                html += "       </td>";
                html += "        <td style=\"width: 100px\">";
                html += "            &nbsp;8 - Dt. Pgto.";
                html += "       </td>";
                html += "        <td style=\"width: 100px\">";
                html += "            &nbsp;9 - Valor Em R$";
                html += "       </td>";
                html += "   </tr> ";

                //Adiciona itens de despesas
                foreach (RN.PrestacaoContas.DTOs.DadosDespesa item in formulario.Despesas)
                {
                    html += "      <tr style=\"height: 20px\">";
                    html += "         <td>";
                    html += "             &nbsp;" + item.Evento;
                    html += "          </td>";
                    html += "         <td>";
                    html += "             &nbsp;" + item.FornecedorBeneficiario;
                    html += "         </td>";
                    html += "         <td>";
                    html += "             &nbsp;" + item.DocumentoFiscal;
                    html += "       </td>";
                    html += "        <td>";
                    html += "            &nbsp;" + item.DataPagamento.ToString("dd/MM/yyyy");
                    html += "       </td>";
                    html += "        <td>";
                    html += "            &nbsp;" + item.Valor;
                    html += "       </td>";
                    html += "   </tr> ";
                }

                //Fim da tabela
                html += "  <tr style=\"height: 20px\">";
                html += "          <td colspan='4'>";
                html += "           &nbsp;9 - Total de Despesas";
                html += "      </td>";
                html += "     <td style=\"background-color: Yellow;\">";
                html += "          &nbsp;&nbsp;" + formulario.TotalDespesas;
                html += "     </td>";
                html += "   </tr>";
                html += " </table>";

                //Adicina html no div da tela
                divGridIIA.InnerHtml = html;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public void CarregaFormularioIII(string censo, int periodoReferenciaId)
        {
            string html = string.Empty;
            RN.PrestacaoContas.DTOs.DadosFormulario2 formulario = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosFormulario2();
            RN.PrestacaoContas.FormularioSei rnFormularioSei = new Techne.Lyceum.RN.PrestacaoContas.FormularioSei();

            try
            {
                //Busca dados do formulario
                formulario = rnFormularioSei.ObtemDadosFormulario2BPor(censo, periodoReferenciaId);

                //Adiciona campos fixos
                lblfrm3PeriodoPrestacao.Text = formulario.PeriodoPrestacao;
                lblfrm3SaldoAnterior.Text = formulario.SaldoAnterior;
                lblfrm3RepasseRecebido.Text = formulario.RepassesRecebidos;
                lblfrm3CreditosDebitos.Text = formulario.CreditosDebitos;
                lblfrm3SaldoInicial.Text = formulario.SaldoInicial;
                lblfrm3SaldoFinal.Text = formulario.SaldoFinalComRendimento;

                //Adiciona cabeçalho da tabela de despesas
                html = "<table border=\"1px\" width=\"100%\" style=\"font-weight: normal; font-family: Calibri;";
                html += " font-size: 12px; border-spacing: 0; border-collapse: collapse; border: 1px solid #000000\">";
                html += "      <tr style=\"height: 20px\">";
                html += "         <td style=\"width: 130px\">";
                html += "             &nbsp;5 - Evento";
                html += "          </td>";
                html += "         <td>";
                html += "             &nbsp;6 - Fornecedor/Beneficiário";
                html += "         </td>";
                html += "         <td style=\"width: 100px\">";
                html += "             &nbsp;7 - Documento Fiscal";
                html += "       </td>";
                html += "        <td style=\"width: 100px\">";
                html += "            &nbsp;8 - Dt. Pgto.";
                html += "       </td>";
                html += "        <td style=\"width: 100px\">";
                html += "            &nbsp;9 - Valor Em R$";
                html += "       </td>";
                html += "   </tr> ";

                //Adiciona itens de despesas
                foreach (RN.PrestacaoContas.DTOs.DadosDespesa item in formulario.Despesas)
                {
                    html += "      <tr style=\"height: 20px\">";
                    html += "         <td>";
                    html += "             &nbsp;" + item.Evento;
                    html += "          </td>";
                    html += "         <td>";
                    html += "             &nbsp;" + item.FornecedorBeneficiario;
                    html += "         </td>";
                    html += "         <td>";
                    html += "             &nbsp;" + item.DocumentoFiscal;
                    html += "       </td>";
                    html += "        <td>";
                    html += "            &nbsp;" + item.DataPagamento.ToString("dd/MM/yyyy");
                    html += "       </td>";
                    html += "        <td>";
                    html += "            &nbsp;" + item.Valor;
                    html += "       </td>";
                    html += "   </tr> ";
                }

                //Fim da tabela
                html += "  <tr style=\"height: 20px\">";
                html += "          <td colspan='4'>";
                html += "           &nbsp;9 - Total de Despesas";
                html += "      </td>";
                html += "     <td style=\"background-color: Yellow;\">";
                html += "          &nbsp;&nbsp;" + formulario.TotalDespesas;
                html += "     </td>";
                html += "   </tr>";
                html += "  <tr style=\"height: 20px\">";
                html += "          <td colspan='4'>";
                html += "           &nbsp;10 - Total de Pequenas Despesas";
                html += "      </td>";
                html += "     <td style=\"background-color: Yellow;\">";
                html += "          &nbsp;&nbsp;" + formulario.TotalPequenasDespesas;
                html += "     </td>";
                html += "   </tr>";
                html += " </table>";

                //Adicina htMl no div da tela
                divGridIIB.InnerHtml = html; 
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public List<RN.PrestacaoContas.DTOs.DadosFormulario2> CarregaFormularioIV(string censo, int periodoReferenciaId)
        {
           
            RN.PrestacaoContas.FormularioSei rnFormularioSei = new Techne.Lyceum.RN.PrestacaoContas.FormularioSei();
            List<RN.PrestacaoContas.DTOs.DadosFormulario2> lista = new List<Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosFormulario2>();
            try
            {
                //Busca dados do formulario
                lista = rnFormularioSei.ObtemDadosFormulario2CPor(censo, periodoReferenciaId);
                              

                return lista;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                return null;
            }
        }

        public void MontaFormularioIV(RN.PrestacaoContas.DTOs.DadosFormulario2 formulario)
        {
            string html = string.Empty;

            //Adiciona campos fixos

            html = "";
            html += "<table width=\"100%\" style=\"font-family: Calibri; font-size: 13px; font-weight: bold; border: 1px solid #000000\">";
            html += "            <tr>";
            html += "                <td style=\"height: 15px\" colspan=\"3\">";
            html += "                    &nbsp;";
            html += "                </td>";
            html += "            </tr>";
            html += "            <tr>";
            html += "                <td align=\"center\" colspan=\"3\">";
            html += "                    FORMULÁRIO II - C DA Resolução SEEDUC Nº 5.722 DE 18 DE FEVEREIRO DE 2019";
            html += "                </td>";
            html += "            </tr>";
            html += "            <tr>";
            html += "               <td align=\"center\" colspan=\"3\">";
            html += "                    RELAÇÃO DOS RECURSOS RECEBIDO PELA SECRETARIA DE ESTADO DE EDUCAÇÃO E DESPESAS REALIZADAS";
            html += "                    PELA AAE NO PROJETO <u>" + formulario.PlanoTrabalho.ToUpper();
            html += "               </u> </td>";
            html += "            </tr>";
            html += "            <tr>";
            html += "                <td style=\"height: 25px\" colspan=\"3\">";
            html += "                    &nbsp;";
            html += "                </td>";
            html += "            </tr>";
            html += "            <tr>";
            html += "                <td style=\"width: 10px\">";
            html += "                    &nbsp;";
            html += "                 </td>";
            html += "                 <td>";
            html += "                     BLOCO 1- DADOS DAS RECEITAS RECEBIDAS";
            html += "                 </td>";
            html += "                 <td style=\"width: 10px\">";
            html += "                     &nbsp;";
            html += "                 </td>";
            html += "             </tr>";
            html += " <tr>";
            html += "                <td style=\"width: 10px\">";
            html += "                    &nbsp;";
            html += "                </td>";
            html += "                <td>";
            html += "                    <table border=\"1px\" width=\"100%\" style=\"font-weight: normal; font-family: Calibri;";
            html += "                        font-size: 12px; border-spacing: 0; border-collapse: collapse; border: 1px solid #000000\">";
            html += "                        <tr style=\"height: 20px\">";
            html += "                            <td>";
            html += "                                &nbsp;1- Período da Prestação de Contas:";
            html += "                            </td>";
            html += "                            <td style=\"background-color: Yellow;\">";
            html += "                                &nbsp;&nbsp;" + formulario.PeriodoPrestacao;
            html += "                            </td>";
            html += "                        </tr>";
            html += "                        <tr style=\"height: 20px\">";
            html += "                            <td>";
            html += "                                &nbsp;2 - Saldo Anterior:";
            html += "                            </td>";
            html += "                            <td style=\"background-color: Yellow;\">";
            html += "                                &nbsp;&nbsp;" + formulario.SaldoAnterior;
            html += "                            </td>";
            html += "                        </tr>";
            html += "                        <tr style=\"height: 20px\">";
            html += "                            <td>";
            html += "                                &nbsp;3 - Repasses Recebidos:";
            html += "                            </td>";
            html += "                            <td style=\"background-color: Yellow;\">";
            html += "                                &nbsp;&nbsp;" + formulario.RepassesRecebidos;
            html += "                            </td>";
            html += "                        </tr>";
            html += "                        <tr style=\"height: 20px\">";
            html += "                            <td>";
            html += "                                &nbsp;3.1 - Créditos/Débitos:";
            html += "                            </td>";
            html += "                            <td style=\"background-color: Yellow;\">";
            html += "                                &nbsp;&nbsp;" + formulario.CreditosDebitos;
            html += "                            </td>";
            html += "                        </tr>";
            html += "                        <tr style=\"height: 20px\">";
            html += "                            <td>";
            html += "                                &nbsp;4 - Saldo Inicial (2 + 3 + 3.1):";
            html += "                            </td>";
            html += "                            <td style=\"background-color: Yellow;\">";
            html += "                                &nbsp;&nbsp;" + formulario.SaldoInicial;
            html += "                            </td>";
            html += "                        </tr>";
            html += "                    </table>";
            html += "                </td>";
            html += "            </tr>";

            html += "<tr>";
            html += "                <td style=\"height: 25px\" colspan=\"3\">";
            html += "                    &nbsp;";
            html += "                </td>";
            html += "            </tr>";


            html += "<tr>";
            html += "                <td style=\"width: 10px\">";
            html += "                    &nbsp;";
            html += "                </td>";
            html += "                <td>";
            html += "                    BLOCO 2 - DADOS DAS DESPESAS REALIZADAS";
            html += "                </td>";
            html += "                <td style=\"width: 10px\">";
            html += "                    &nbsp;";
            html += "                </td>";
            html += "            </tr>";

            html += "<tr>";
            html += "                <td style=\"width: 10px\">";
            html += "                    &nbsp;";
            html += "                </td>";
            html += "                <td>";
   			

            //Adiciona cabeçalho da tabela de despesas
            html += "<table border=\"1px\" width=\"100%\" style=\"font-weight: normal; font-family: Calibri;";
            html += " font-size: 12px; border-spacing: 0; border-collapse: collapse; border: 1px solid #000000\">";
            html += "      <tr style=\"height: 20px\">";
            html += "         <td style=\"width: 130px\">";
            html += "             &nbsp;5 - Evento";
            html += "          </td>";
            html += "         <td>";
            html += "             &nbsp;6 - Fornecedor/Beneficiário";
            html += "         </td>";
            html += "         <td style=\"width: 100px\">";
            html += "             &nbsp;7 - Documento Fiscal";
            html += "       </td>";
            html += "        <td style=\"width: 100px\">";
            html += "            &nbsp;8 - Dt. Pgto.";
            html += "       </td>";
            html += "        <td style=\"width: 100px\">";
            html += "            &nbsp;9 - Valor Em R$";
            html += "       </td>";
            html += "   </tr> ";

            //Adiciona itens de despesas
            foreach (RN.PrestacaoContas.DTOs.DadosDespesa item in formulario.Despesas)
            {
                html += "      <tr style=\"height: 20px\">";
                html += "         <td>";
                html += "             &nbsp;" + item.Evento;
                html += "          </td>";
                html += "         <td>";
                html += "             &nbsp;" + item.FornecedorBeneficiario;
                html += "         </td>";
                html += "         <td>";
                html += "             &nbsp;" + item.DocumentoFiscal;
                html += "       </td>";
                html += "        <td>";
                html += "            &nbsp;" + item.DataPagamento.ToString("dd/MM/yyyy");
                html += "       </td>";
                html += "        <td>";
                html += "            &nbsp;" + item.Valor;
                html += "       </td>";
                html += "   </tr> ";
            }

            //Fim da tabela
            html += "  <tr style=\"height: 20px\">";
            html += "          <td colspan='4'>";
            html += "           &nbsp;9 - Total de Despesas";
            html += "      </td>";
            html += "     <td style=\"background-color: Yellow;\">";
            html += "          &nbsp;&nbsp;" + formulario.TotalDespesas;
            html += "     </td>";
            html += "   </tr>";
            html += " </table>";

            html += "				 </td>";
            html += "               <td style=\"width: 10px\">";
            html += "                    &nbsp;";
            html += "                </td>";
            html += "            </tr>";

            html += "<tr>";
            html += "                <td style=\"height: 25px\" colspan=\"4\">";
            html += "                    &nbsp;";
            html += "                </td>";
            html += "            </tr>";
            html += "            <tr>";
            html += "                <td style=\"width: 10px\">";
            html += "                    &nbsp;";
            html += "                </td>";
            html += "                <td>";
            html += "                    BLOCO 3- SALDO FINAL";
            html += "                </td>";
            html += "                <td style=\"width: 10px\">";
            html += "                    &nbsp;";
            html += "                </td>";
            html += "            </tr>";
            html += "            <tr>";
            html += "                <td style=\"width: 10px\">";
            html += "                    &nbsp;";
            html += "                </td>";
            html += "                <td>";
            html += "                    <table border=\"1px\" width=\"100%\" style=\"font-weight: normal; font-family: Calibri;";
            html += "                        font-size: 12px; border-spacing: 0; border-collapse: collapse; border: 1px solid #000000\">";
            html += "                        <tr style=\"height: 20px\">";
            html += "                            <td>";
            html += "                                &nbsp;10 - Saldo Final (4 &#8211; 9)";
            html += "                            </td>";
            html += "                            <td style=\"background-color: Yellow;\">";
            html += "                                &nbsp;&nbsp;" + formulario.SaldoFinal;
            html += "                            </td>";
            html += "                        </tr>";
            html += "                    </table>";
            html += "                </td>";
            html += "                <td style=\"width: 10px\">";
            html += "                    &nbsp;";
            html += "                </td>";
            html += "            </tr>";
            html += "            <tr>";
            html += "                <td style=\"height: 15px\" colspan=\"4\">";
            html += "                    &nbsp;";
            html += "                </td>";
            html += "            </tr>";
            html += "            </table>";

            //Adicina html no div da tela
            divGridIIC.InnerHtml = html;
        }

        public void CarregaFormularioV(string censo, int periodoReferenciaId)
        {
            try
            {
                RN.PrestacaoContas.DTOs.DadosFormulario5 formulario = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosFormulario5();
                RN.PrestacaoContas.FormularioSei rnFormularioSei = new Techne.Lyceum.RN.PrestacaoContas.FormularioSei();

                //Busca dados do formulario
                formulario = rnFormularioSei.ObtemDadosFormulario5Por(censo, periodoReferenciaId);

                lblfrm5PeriodoPrestacao.Text = formulario.PeriodoPrestacao;
                lblfrm5MerendaSaldoAnterior.Text = formulario.MerendaSaldoAnterior;
                lblfrm5MerendaRepasses.Text = formulario.MerendaRepasses;
                lblfrm5MerendaRendimentos.Text = formulario.MerendaRendimentos;
                lblfrm5MerendaCreditosDebitos.Text = formulario.MerendaCreditosDebitos;
                lblfrm5MerendaDevolucoes.Text = formulario.MerendaDevolucoes;
                lblfrm5MerendaDespesas.Text = formulario.MerendaDespesas;
                lblfrm5MerendaSaldoFinal.Text = formulario.MerendaSaldoFinal;
                lblfrm5ManutencaoSaldoAnterior.Text = formulario.ManutencaoSaldoAnterior;
                lblfrm5ManutencaoRepasses.Text = formulario.ManutencaoRepasses;
                lblfrm5ManutencaoCreditosDebitos.Text = formulario.ManutencaoCreditosDebitos;
                lblfrm5ManutencaoRendimentos.Text = formulario.ManutencaoRendimentos;
                lblfrm5ManutencaoDevolucoes.Text = formulario.ManutencaoDevolucoes;
                lblfrm5ManutencaoDespesas.Text = formulario.ManutencaoDespesas;
                lblfrm5ManutencaoSaldoFinal.Text = formulario.ManutencaoSaldoFinal;
                lblfrm5OutrosProjetosSaldoAnterior.Text = formulario.OutrosProjetosSaldoAnterior;
                lblfrm5OutrosProjetosRepasses.Text = formulario.OutrosProjetosRepasses;
                lblfrm5OutrosProjetosCreditosDebitos.Text = formulario.OutrosProjetosCreditosDebitos;
                lblfrm5OutrosProjetosRendimentos.Text = formulario.OutrosProjetosRendimentos;
                lblfrm5OutrosProjetosDevolucoes.Text = formulario.OutrosProjetosDevolucoes;
                lblfrm5OutrosProjetosDespesas.Text = formulario.OutrosProjetosDespesas;
                lblfrm5OutrosProjetosSaldoFinal.Text = formulario.OutrosProjetosSaldoFinal;
                lblfrm5TotalSaldoAnterior.Text = formulario.TotalSaldoAnterior;
                lblfrm5TotalRepasses.Text = formulario.TotalRepasses;
                lblfrm5TotalCreditosDebitos.Text = formulario.TotalCreditosDebitos;
                lblfrm5TotalRendimentos.Text = formulario.TotalRendimentos;
                lblfrm5TotalDevolucoes.Text = formulario.TotalDevolucoes;
                lblfrm5TotalDespesas.Text = formulario.TotalDespesas;
                lblfrm5TotalSaldoFinal.Text = formulario.TotalSaldoFinal;

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public List<RN.PrestacaoContas.DTOs.DadosFormularioFornecedor> CarregaFormularioVI(string censo, int periodoReferenciaId)
        {
            try
            {
                List<RN.PrestacaoContas.DTOs.DadosFormularioFornecedor> formulario = new List<Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosFormularioFornecedor>();

                RN.PrestacaoContas.FormularioSei rnFormularioSei = new Techne.Lyceum.RN.PrestacaoContas.FormularioSei();

                //Busca dados do formulario
                formulario = rnFormularioSei.ObtemDadosFormularioFornecedorPor(censo, periodoReferenciaId);


                return formulario;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
            return null;
        }

        public void MontaFormularioVI(String DiretoriaRegional, String MunicipioAtendidos, String Cnpj, String RazaoSocial, String InscricaoEstadual, String InscricaoMunicipal, String Endereco, String ComplementoEndereco, String Bairro, String UF, String Municipio, String Cep, String CaixaPostal, String DDD, String Telefone, String Email, String Nome, String Cpf)
        {
            lblfrm6DiretoriaRegional.Text = DiretoriaRegional;
            lblfrm6MunicipioAtendidos.Text = MunicipioAtendidos;
            lblfrm6Cnpj.Text = Cnpj;
            lblfrm6RazaoSocial.Text = RazaoSocial;
            lblfrm6InscricaoEstadual.Text = InscricaoEstadual;
            lblfrm6InscricaoMunicipal.Text = InscricaoMunicipal;
            lblfrm6Endereco.Text = Endereco;
            lblfrm6ComplementoEndereco.Text = ComplementoEndereco;
            lblfrm6Bairro.Text = Bairro;
            lblfrm6UF.Text = UF;
            lblfrm6Municipio.Text = Municipio;
            lblfrm6Cep.Text = Cep;
            lblfrm6CaixaPostal.Text = CaixaPostal;
            lblfrm6DDD.Text = DDD;
            lblfrm6Telefone.Text = Telefone;
            lblfrm6Email.Text = Email;
            lblfrm6Nome.Text = Nome;
            lblfrm6Cpf.Text = Cpf;
        }

    }

}


