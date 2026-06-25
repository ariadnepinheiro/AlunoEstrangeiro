using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using Image = System.Drawing.Image;

namespace Techne.Lyceum.Net.Certificacao
{
    [NavUrl("~/Certificacao/CertificadoEscolar.aspx"),
    ControlText("Certificado ou Diploma Escolar"),
    Title("Certificado ou Diploma Escolar"),]

    public partial class CertificadoEscolar : TPage
    {
        private string tipoDocumentoID;
        private string tipoConclusaoID;
        private string matriculaAluno;
        private string pessoa;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    this.listarTipos();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnGerarDocumento, AcaoControle.novo);
        }

        protected void btCancelar_Click(object sender, EventArgs e)
        {
            tseAluno.ResetValue();
            EstadoInicial();
        }

        public void EstadoInicial()
        {
            ddlTipoConclusao.ClearSelection();
            ddlTipoDocumento.ClearSelection();
            pnDadosAluno.Visible = false;
            lblMensagem.Text = string.Empty;
            btnGerarDocumento.Visible = false;
            btnCancelar.Visible = false;
        }

        private void listarTipos()
        {
            ddlTipoConclusao.DataSource = RN.Certificacao.HistoricoEscolar.listarTipoConclusao();
            ddlTipoConclusao.DataBind();
            ddlTipoConclusao.Items.Insert(0, new ListItem("Selecione", "-1"));
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            tipoConclusaoID = ddlTipoConclusao.SelectedValue;
            tipoDocumentoID = ddlTipoDocumento.SelectedValue;
            matriculaAluno = Convert.ToString(tseAluno.DBValue);
            pessoa = tseAluno["Pessoa"].ToString();
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                RN.Certificacao.HistoricoEscolar rnHistorico = new RN.Certificacao.HistoricoEscolar();
                validacao = rnHistorico.ValidaHistorico(pessoa, Convert.ToInt32(tipoConclusaoID), Convert.ToInt32(tipoDocumentoID), string.Empty);

                if (validacao.Valido)
                {
                    // regra de autorização para gerar documento
                    if (rnHistorico.ValidaAutorizacao(matriculaAluno, Convert.ToInt32(tipoConclusaoID), Convert.ToInt32(tipoDocumentoID)))
                    {
                        pnDadosAluno.Visible = true;
                        //carregar dados do aluno
                        this.carregarDadosAlunoPor(matriculaAluno);

                        btnGerarDocumento.Visible = true;
                        btnCancelar.Visible = true;
                    }
                    else
                    {
                        lblMensagem.Text = "Este aluno ainda não possui autorização para gerar o documento solicitado.";
                    }
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        private void CarregaFoto(LyFotoPessoa dadosFotoPessoa)
        {
            if (dadosFotoPessoa == null)
            {
                bimgFotoPessoa.EmptyImage.Url = "~/Images/semfoto.jpg";
                bimgFotoPessoa.EmptyImage.AlternateText = "Sem foto";
                bimgFotoPessoa.ContentBytes = null;
            }
            else
            {
                try
                {
                    // Tenta carregar array de bytes em objeto Image. 
                    // Em caso de exceção, a foto está em formato inválido
                    Image.FromStream(new MemoryStream(dadosFotoPessoa.Foto));
                    bimgFotoPessoa.ContentBytes = dadosFotoPessoa.Foto;
                }
                catch
                {
                    bimgFotoPessoa.EmptyImage.Url = "~/Images/fotoinvalida.jpg";
                    bimgFotoPessoa.EmptyImage.AlternateText = "Foto inválida";
                    bimgFotoPessoa.ContentBytes = null;
                }
            }
        }

        private void carregarDadosAlunoPor(string matricula)
        {
            RN.Aluno rnAluno = new RN.Aluno();
            RN.DTOs.DadosFichaAluno dadosAluno = new Techne.Lyceum.RN.DTOs.DadosFichaAluno();

            try
            {
                //Busca dados do aluno passando o parâmetro matrícula
                dadosAluno = rnAluno.ObtemFichaAlunoPor(matricula);

                //Buscar dados da matrícula deste aluno a partir do momento em que ele seleciona o tipo

                if (!string.IsNullOrEmpty(dadosAluno.AlunoMatricula))
                {
                    pnDadosAluno.Visible = true;

                    //Dados pessoais
                    lblNome.Text = dadosAluno.NomeAluno.Trim();
                    lblDataNascimento.Text = dadosAluno.DataNascimento.ToString("dd/MM/yyyy");
                    lblSexo.Text = (dadosAluno.Sexo == "M" || dadosAluno.Sexo == "Masculino" ? "Masculino" : "Feminino");
                    lblQtdeFilhos.Text = (!string.IsNullOrEmpty(dadosAluno.QuantidadeFilhos.ToString()) ? dadosAluno.QuantidadeFilhos.ToString() : "Não informado");
                    lblTipoSanguineo.Text = (!string.IsNullOrEmpty(dadosAluno.TipoSanguineo.ToString()) ? dadosAluno.TipoSanguineo : "Não declarado");
                    lblEtnia.Text = (!string.IsNullOrEmpty(dadosAluno.Etnia.ToString()) ? dadosAluno.Etnia : "Não declarado");
                    lblEstadoCivil.Text = (!string.IsNullOrEmpty(dadosAluno.EstadoCivil.ToString()) ? dadosAluno.EstadoCivil : "Não declarado");
                    lblNacionalidade.Text = (!string.IsNullOrEmpty(dadosAluno.Nacionalidade.ToString()) ? dadosAluno.Nacionalidade : "Não declarado");

                    if (!string.IsNullOrEmpty(dadosAluno.Naturalidade) && dadosAluno.Naturalidade.Trim() != "/")
                    {
                        // Brasileiro: dados vêm normalmente do ObtemFichaAlunoPor
                        lblNaturalidade.Text = dadosAluno.Naturalidade;
                        lblUFNasc.Text = dadosAluno.UfNascimento;
                        lblPaisNasc.Text = (!string.IsNullOrEmpty(dadosAluno.PaisNascimento) ? dadosAluno.PaisNascimento : "Não declarado");
                    }
                    else
                    {
                        try
                        {
                            DataTable dtNatEst = rnAluno.ObtemNaturalidadeEstrangeiraPor(matricula);
                            if (dtNatEst != null && dtNatEst.Rows.Count > 0)
                            {
                                string munExt     = dtNatEst.Rows[0]["NOME_MUNICIPIO"].ToString();
                                string ufExt = dtNatEst.Rows[0]["NOME_ESTADO"].ToString();
                                string paisExt    = dtNatEst.Rows[0]["NOME_PAIS"].ToString();

                                // Sempre mostra país de nascimento
                                lblPaisNasc.Text = RN.Util.Utils.Capitaliza(paisExt);

                                // Brasileiro nascido fora do Brasil: Município - Nome do Estado - País
                                if (!string.IsNullOrEmpty(dadosAluno.Nacionalidade) &&
                                    dadosAluno.Nacionalidade.Trim().ToUpper() == "BRASILEIRA")
                                {
                                    lblNaturalidade.Text = RN.Util.Utils.Capitaliza(munExt + " / " + ufExt + " - " + paisExt);
                                    lblUFNasc.Text = ufExt;
                                }
                                // Exclusivamente estrangeiro: Município - País (sem estado)
                                else
                                {
                                    lblNaturalidade.Text = RN.Util.Utils.Capitaliza(munExt + " - " + paisExt);
                                    lblUFNasc.Text = string.Empty;
                                }
                            }
                            else
                            {
                                lblNaturalidade.Text = "Não informado";
                                lblUFNasc.Text       = string.Empty;
                                lblPaisNasc.Text     = "Não declarado";
                            }
                        }
                        catch
                        {
                            lblNaturalidade.Text = "Não informado";
                            lblUFNasc.Text       = string.Empty;
                            lblPaisNasc.Text     = "Não declarado";
                        }
                    }

                    lblCredo.Text = dadosAluno.Credo;
                    lblNecEspecial.Text = dadosAluno.NecessidadeEspecial;
                    lblMatricula.Text = dadosAluno.AlunoMatricula;
                    lblDataSit.Text = dadosAluno.DataSituacao.ToString();

                    this.CarregaFoto(dadosAluno.Foto);

                    //Dados Filiação
                    lblNomeMae.Text = dadosAluno.NomeMae;
                    lblCPFMae.Text = dadosAluno.CPFMae.AplicarMascaraCPF();

                    lblNomePai.Text = dadosAluno.NomePai;
                    lblCPFPai.Text = dadosAluno.CPFPai.AplicarMascaraCPF();

                    lblRespLegal.Text = dadosAluno.ResponsavelLegal;
                    lblNomeOutros.Text = dadosAluno.NomeOutros;
                    lblCPFOutros.Text = dadosAluno.CpfOutros.AplicarMascaraCPF();

                    lblTelMae.Text = dadosAluno.TelMae.AplicarMascaraTelefoneComDDD();
                    lblTelPai.Text = dadosAluno.TelPai.AplicarMascaraTelefoneComDDD();
                    lblTelResp.Text = dadosAluno.TelResponsavel.AplicarMascaraTelefoneComDDD();

                    //Endereço
                    lblEndereco.Text = dadosAluno.Endereco;
                    lblNumero.Text = dadosAluno.NumeroEndereco;
                    lblComplemento.Text = dadosAluno.ComplementoEndereco;
                    lblBairro.Text = dadosAluno.BairroEndereco;
                    lblMunicipio.Text = dadosAluno.MunicipioCartorio;
                    lblEstado.Text = dadosAluno.EstadoEndereco;
                    lblCEP.Text = dadosAluno.CepEndereco;
                    lblLocalizacao.Text = dadosAluno.LocalizacaoEndereco;

                    //Contatos
                    lblContatoTelefone.Text = dadosAluno.Telefone.AplicarMascaraTelefoneComDDD();
                    lblContatoCelular.Text = dadosAluno.Celular.AplicarMascaraCelularComDDD();
                    lblContatoEmail.Text = dadosAluno.Email;

                    btnGerarDocumento.Visible = true;
                    btnCancelar.Visible = true;
                }
                else
                {
                    //limpar a tela
                    pnDadosAluno.Visible = false;
                    btnGerarDocumento.Visible = false;
                    tseAluno.ResetValue();
                    ddlTipoConclusao.ClearSelection();
                    lblMensagem.Text = "Aluno não encontrado.";
                    return;
                }
            }
            catch (Exception ex)
            {
                this.lblMensagem.Text = ex.Message;
            }
        }

        protected void btnGerarDocumento_Click(object sender, EventArgs e)
        {
            switch (Convert.ToInt32(ddlTipoDocumento.SelectedValue))
            {
                case 3:
                    GerarDocumento(RN.Certificacao.TipoDocumento.CERTIFICADO_ESCOLAR);
                    break;

                case 4:
                    GerarDocumento(RN.Certificacao.TipoDocumento.DIPLOMA);
                    break;

                default:
                    lblMensagem.Text = "OPÇÃO INVÁLIDA";
                    break;
            }
        }

        public void GerarDocumento(int tipoDocumentoID)
        {
            matriculaAluno = Convert.ToString(tseAluno.DBValue);
            tipoConclusaoID = ddlTipoConclusao.SelectedValue;

            string usuario = User.Identity.Name;
            ExportaPdf pdf = new ExportaPdf();
            string html = string.Empty;
            string tipoArquivo = string.Empty;

            if (tipoDocumentoID == 3)
                tipoArquivo = "CERTIFICADO_ESCOLAR";
            else
                tipoArquivo = "DIPLOMA_ESCOLAR";

            lblMensagem.Text = string.Empty;

            RN.Certificacao.CertificadoEscolar rnCertificadoEscolar = new Techne.Lyceum.RN.Certificacao.CertificadoEscolar();
            try
            {
                if (rnCertificadoEscolar.PossuiAutorizacao(matriculaAluno, Convert.ToInt32(tipoConclusaoID), tipoDocumentoID))
                {
                    string cssText = File.ReadAllText(HttpContext.Current.Server.MapPath("Css/CertificadoEscolar.css"));
                    string certificado = rnCertificadoEscolar.GerarCertificadoDiplomaEscolarCertidao(matriculaAluno, Convert.ToInt32(tipoConclusaoID), usuario, tipoDocumentoID);

                    if (certificado.Substring(2, 6) != "<html>")
                    {
                        lblMensagem.Text = certificado;
                        lblMensagem.Focus();
                    }
                    else
                    {
                        pdf.ExportaHtmlCssPor(certificado, tipoArquivo + "_" + matriculaAluno + "_" + DateTime.Now.ToShortDateString().Replace("/", "_"), cssText);
                    }
                }
                else
                {
                    lblMensagem.Text = "A emissão do documento solicitado ainda não foi autorizada.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = (ex.Message);
            }
        }

        protected void tseAluno_TextChange(object sender, EventArgs e)
        {
            EstadoInicial();
        }

        //#region ControlaVisibilidade

        //public enum TipoOperacao
        //{
        //    Novo,
        //    Cancelar,
        //    Inicial,
        //    Consultar,
        //    Sucesso,
        //    Editar,
        //    Finalizar
        //}

        //private TipoOperacao _tipoOperacao
        //{
        //    get
        //    {
        //        if (ViewState["_tipoOperacao"] != null)
        //        {
        //            if (ViewState["_tipoOperacao"] is TipoOperacao)
        //            {
        //                return (TipoOperacao)ViewState["_tipoOperacao"];
        //            }
        //        }
        //
        //        return TipoOperacao.Inicial;
        //    }
        //
        //    set
        //    {
        //        ViewState["_tipoOperacao"] = value;
        //    }
        //}

        //private void RetiraVisibilidadeBotao()
        //{
        //    btnGerarDocumento.Visible = false;
        //}

        //private void ControlarVisibilidadeControle(ImageButton[] imgBotoes, Button[] botoes)
        //{
        //    RetiraVisibilidadeBotao();
        //
        //    foreach (var img in imgBotoes)
        //    {
        //        img.Visible = true;
        //    }
        //    foreach (var botao in botoes)
        //    {
        //        botao.Visible = true;
        //    }
        //}

        //private void ControlarTipoOperacao()
        //{
        //    switch (this._tipoOperacao)
        //    {
        //        case TipoOperacao.Inicial:
        //            {
        //                btnGerar.Visible = false;
        //                grdMeusAlunos.Visible = false;
        //                break;
        //            }
        //        case TipoOperacao.Consultar:
        //            {
        //                grdMeusAlunos.Visible = true;
        //                Button[] controles = new Button[] { btnGerar };
        //                ImageButton[] imgControles = new ImageButton[] { };
        //                ControlarVisibilidadeControle(imgControles, controles);
        //                ControlaAcesso(btnGerar, AcaoControle.novo);
        //                break;
        //            }
        //    }
        //}

        //#endregion
    }
}
