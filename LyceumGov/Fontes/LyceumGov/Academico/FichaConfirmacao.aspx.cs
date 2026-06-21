using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN;
using System.Text;
using Techne.Web;
using Techne.Data;
using Techne.Lyceum.RN.Entidades;
using Image = System.Drawing.Image;
using System.IO;


namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/FichaConfirmacao.aspx"), ControlText("FichaConfirmacao"), Title("Ficha de Confirmação")]
    public partial class FichaConfirmacao : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int idConfirmacao = 0;

                if (!IsPostBack)
                {
                    if (Request.QueryString.Keys.Count > 0)
                    {
                        var decodedBytes = Convert.FromBase64String(this.Request.QueryString["Chave"]);
                        var decodedText = Encoding.UTF8.GetString(decodedBytes);

                        idConfirmacao = Convert.ToInt32(decodedText.Substring(decodedText.LastIndexOf('=') + 1));

                        if (idConfirmacao != 0)
                        {
                            CarregaModais();
                            this.CarregaDadosAlunoConfirmacao(idConfirmacao);
                        }
                    }
                
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

        private void CarregaDadosAlunoConfirmacao(int idConfirmacao)
        {
            try
            {
                RN.DTOs.DadosFichaConfirmacao dadosFichaConfirmacao;
                ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();

                if (idConfirmacao != 0)
                {
                    dadosFichaConfirmacao = rnConfirmacaoMatricula.ObtemFichaConfirmacaoAlunoPor(idConfirmacao);

                    CarregaFoto(dadosFichaConfirmacao.Foto);

                    lblNome.Text = dadosFichaConfirmacao.NomeAluno;
                    lblDataNascimento.Text = dadosFichaConfirmacao.DataNascimento.ToString("dd/MM/yyyy");
                    lblSexo.Text = dadosFichaConfirmacao.Sexo;
                    lblQtdeFilhos.Text = dadosFichaConfirmacao.QuantidadeFilhos.ToString();
                    lblTipoSanguineo.Text = dadosFichaConfirmacao.TipoSanguineo;
                    lblEtnia.Text = dadosFichaConfirmacao.Etnia;
                    lblEstadoCivil.Text = dadosFichaConfirmacao.EstadoCivil;
                    lblPaisNasc.Text = dadosFichaConfirmacao.PaisNascimento;
                    lblNacionalidade.Text = dadosFichaConfirmacao.Nacionalidade;
                    lblUFNasc.Text = dadosFichaConfirmacao.UfNascimento;
                    lblNaturalidade.Text = dadosFichaConfirmacao.Naturalidade;
                    lblCredo.Text = dadosFichaConfirmacao.Credo;
                    lblNecEspecial.Text = dadosFichaConfirmacao.NecessidadeEspecial;
                    lblMatricula.Text = dadosFichaConfirmacao.AlunoMatricula;
                    lblDataSit.Text = dadosFichaConfirmacao.DataSituacao != null ? Convert.ToDateTime(dadosFichaConfirmacao.DataSituacao).ToString("dd/MM/yyyy") : string.Empty;
                    lblHoraSit.Text = dadosFichaConfirmacao.DataSituacao != null ? Convert.ToDateTime(dadosFichaConfirmacao.DataSituacao).ToString("HH:mm") : string.Empty;

                    lblNomeMae.Text = dadosFichaConfirmacao.NomeMae;
                    lblMaeFalecida.Text = dadosFichaConfirmacao.FalecidaMae;
                    lblCPFMae.Text = dadosFichaConfirmacao.CPFMae;

                    lblNomePai.Text = dadosFichaConfirmacao.NomePai;
                    lblPaiFalecido.Text = dadosFichaConfirmacao.FalecidoPai;
                    lblCPFPai.Text = dadosFichaConfirmacao.CPFPai;

                    lblRespLegal.Text = dadosFichaConfirmacao.ResponsavelLegal;
                    lblNomeOutros.Text = dadosFichaConfirmacao.NomeOutros;
                    lblCPFOutros.Text = dadosFichaConfirmacao.CpfOutros;

                    lblTelMae.Text = dadosFichaConfirmacao.TelMae;
                    lblTelPai.Text = dadosFichaConfirmacao.TelPai;
                    lblTelResp.Text = dadosFichaConfirmacao.TelResponsavel;

                    lblEndereco.Text = dadosFichaConfirmacao.Endereco;
                    lblNumero.Text = dadosFichaConfirmacao.NumeroEndereco;
                    lblComplemento.Text = dadosFichaConfirmacao.ComplementoEndereco;
                    lblBairro.Text = dadosFichaConfirmacao.BairroEndereco;
                    lblMunicipio.Text = dadosFichaConfirmacao.MunicipioEndereco;
                    lblEstado.Text = dadosFichaConfirmacao.EstadoEndereco;
                    lblCEP.Text = dadosFichaConfirmacao.CepEndereco;
                    lblLocalizacao.Text = dadosFichaConfirmacao.LocalizacaoEndereco;

                    lblContatoTelefone.Text = dadosFichaConfirmacao.Telefone;
                    lblContatoCelular.Text = dadosFichaConfirmacao.Celular;
                    lblContatoEmail.Text = dadosFichaConfirmacao.Email;

                    lblNumDocDetran.Text = dadosFichaConfirmacao.NumeroRGDetran;
                    lblDataRGDetran.Text = dadosFichaConfirmacao.DataExpedicaoRGDetran != null ? Convert.ToDateTime(dadosFichaConfirmacao.DataExpedicaoRGDetran).ToString("dd/MM/yyyy") : string.Empty;

                    lblCPFDocumento.Text = dadosFichaConfirmacao.Cpf;
                    lblTipoDocumento.Text = dadosFichaConfirmacao.TipoDocumento;
                    lblNumeroDocumento.Text = dadosFichaConfirmacao.NumeroDocumento;
                    lblComplIdent.Text = dadosFichaConfirmacao.ComplementoIdentidade;
                    lblEstadoDocumento.Text = dadosFichaConfirmacao.EstadoDocumento;
                    lblOrgaoEmissor.Text = dadosFichaConfirmacao.OrgaoEmissorDocumento;
                    lblDtExpedicaoDocumento.Text = dadosFichaConfirmacao.DataExpedicaoDocumento != null ? Convert.ToDateTime(dadosFichaConfirmacao.DataExpedicaoDocumento).ToString("dd/MM/yyyy") : string.Empty;

                    lblINEP.Text = dadosFichaConfirmacao.Inep;
                    lblNIS.Text = dadosFichaConfirmacao.Nis;

                    lblTipoCertidao.Text = dadosFichaConfirmacao.TipoCertidao;
                    lblCertidaoCivil.Text = dadosFichaConfirmacao.CertidaoCivil;
                    lblUFCartorio.Text = dadosFichaConfirmacao.UfCartorio;
                    lblMunicipioCartorio.Text = dadosFichaConfirmacao.MunicipioCartorio;
                    lblNomeCartorio.Text = dadosFichaConfirmacao.NomeCartorio;
                    lblLivro.Text = dadosFichaConfirmacao.Livro;
                    lblFolha.Text = dadosFichaConfirmacao.Folha;
                    lblTermo.Text = dadosFichaConfirmacao.NumeroTermo;
                    lblDtEmissaoCertidao.Text = dadosFichaConfirmacao.DataEmissaoCertidao != null ? Convert.ToDateTime(dadosFichaConfirmacao.DataEmissaoCertidao).ToString("dd/MM/yyyy") : string.Empty; 
                    lblMatriculaCertidao.Text = dadosFichaConfirmacao.MatriculaCertidao;

                    lblUtilizaTransporte.Text = dadosFichaConfirmacao.UtilizaTransporte;
                    lblPoderResponsavel.Text = dadosFichaConfirmacao.PoderResponsavel;

                    if (!string.IsNullOrEmpty(dadosFichaConfirmacao.Modais))
                    {
                        string[] modais = dadosFichaConfirmacao.Modais.Split(';');
                        foreach (String str in modais)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                chkModais.Items.FindByValue(str).Selected = true;
                            }
                        }
                    }

                    lblMatriculaRenovacaoViaAluno.Text = lblMatriculaRenovacaoViaUnidade.Text = dadosFichaConfirmacao.AlunoMatricula;
                    lblNomeViaAluno.Text = lblNomeViaUnidade.Text = dadosFichaConfirmacao.NomeAluno;
                    lblAnoLetivoViaAluno.Text = lblAnoLetivoViaUnidade.Text = dadosFichaConfirmacao.AnoLetivo.ToString();
                    lblPeriodoLetivoViaAluno.Text = lblPeriodoLetivoViaUnidade.Text = dadosFichaConfirmacao.PeriodoLetivo.ToString();
                    lblUnidadeViaAluno.Text = lblUnidadeViaUnidade.Text = dadosFichaConfirmacao.UnidadeEnsino;
                    lblCensoViaAluno.Text = lblCensoViaUnidade.Text = dadosFichaConfirmacao.Censo;
                    lblModalidadeViaAluno.Text = lblModalidadeViaUnidade.Text = dadosFichaConfirmacao.Modalidade;
                    lblCursoViaAluno.Text = lblCursoViaUnidade.Text = dadosFichaConfirmacao.Curso;
                    lblNomeCursoViaAluno.Text = lblNomeCursoViaUnidade.Text = dadosFichaConfirmacao.CursoDescricao;
                    lblSerieViaAluno.Text = lblSerieViaUnidade.Text = dadosFichaConfirmacao.Serie.ToString();
                    lblTurnoViaAluno.Text = lblTurnoViaUnidade.Text = dadosFichaConfirmacao.Turno;
                    lblDataSugeridaViaAluno.Text = lblDataSugeridaViaUnidade.Text = dadosFichaConfirmacao.DataSugerida != null ? Convert.ToDateTime(dadosFichaConfirmacao.DataSugerida).ToString("dd/MM/yyyy") : string.Empty;
                    lblEnsReligiosoViaAluno.Text = lblEnsReligiosoViaUnidade.Text = dadosFichaConfirmacao.EnsinoReligioso;
                    lblOptativaViaAluno.Text = lblOptativaViaUnidade.Text = dadosFichaConfirmacao.LinguaEstrangueira;
                    lblSituacaoViaAluno.Text = lblSituacaoViaUnidade.Text = dadosFichaConfirmacao.Situacao;
                    lblDataSituacaoViaAluno.Text = lblDataSituacaoViaUnidade.Text = dadosFichaConfirmacao.DataSituacao != null ? Convert.ToDateTime(dadosFichaConfirmacao.DataSituacao).ToString("dd/MM/yyyy") : string.Empty;
                    lblHoraSituacaoViaAluno.Text = lblHoraSituacaoViaUnidade.Text = dadosFichaConfirmacao.DataSituacao != null ? Convert.ToDateTime(dadosFichaConfirmacao.DataSituacao).ToString("HH:mm") : string.Empty;

                    lblMatriculaServidorViaAluno.Text = lblMatriculaServidorViaUnidade.Text = dadosFichaConfirmacao.UsuarioResponsavel;
                    lblNomeServidorViaAluno.Text = lblNomeServidorViaUnidade.Text = dadosFichaConfirmacao.UsuarioResponsavelNome;

                }                
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaModais()
        {
            QueryTable dados = null;

            dados = RN.Basico.ConsultaItemTabelaValDescr("TransporteModal");
            if (dados != null)
            {
                chkModais.Items.Clear();
                chkModais.DataSource = dados;
                chkModais.DataBind();
            }
        }
    }
}
