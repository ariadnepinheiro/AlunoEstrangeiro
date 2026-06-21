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
using Techne.Lyceum.RN.Util;


namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/FichaRenovacao.aspx"), ControlText("FichaRenovacao"), Title("Ficha de Renovação")]
    public partial class FichaRenovacao : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int idRenovacao = 0;

                if (!IsPostBack)
                {
                    if (Request.QueryString.Keys.Count > 0)
                    {
                        var decodedBytes = Convert.FromBase64String(this.Request.QueryString["Chave"]);
                        var decodedText = Encoding.UTF8.GetString(decodedBytes);

                        idRenovacao = Convert.ToInt32(decodedText.Substring(decodedText.LastIndexOf('=') + 1));

                        if (idRenovacao != 0)
                        {
                            CarregaModais();
                            this.CarregaDadosAlunoRenovacao(idRenovacao);
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

        private void CarregaDadosAlunoRenovacao(int idRenovacao)
        {
            try
            {
                RN.DTOs.DadosFichaRenovacao dadosFichaRenovacao;
                RN.RenovacaoMatricula.Renovacao rnRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Renovacao();

                if (idRenovacao != 0)
                {
                    dadosFichaRenovacao = rnRenovacao.ObtemFichaRenovacaoAlunoPor(idRenovacao);

                    CarregaFoto(dadosFichaRenovacao.Foto);

                    lblNome.Text = dadosFichaRenovacao.NomeAluno;
                    lblDataNascimento.Text = dadosFichaRenovacao.DataNascimento.ToString("dd/MM/yyyy");
                    lblSexo.Text = dadosFichaRenovacao.Sexo;
                    lblQtdeFilhos.Text = dadosFichaRenovacao.QuantidadeFilhos.ToString();
                    lblTipoSanguineo.Text = dadosFichaRenovacao.TipoSanguineo;
                    lblEtnia.Text = dadosFichaRenovacao.Etnia;
                    lblEstadoCivil.Text = dadosFichaRenovacao.EstadoCivil;
                    lblPaisNasc.Text = dadosFichaRenovacao.PaisNascimento;
                    lblNacionalidade.Text = dadosFichaRenovacao.Nacionalidade;
                    lblUFNasc.Text = dadosFichaRenovacao.UfNascimento;
                    lblNaturalidade.Text = dadosFichaRenovacao.Naturalidade;
                    lblCredo.Text = dadosFichaRenovacao.Credo;
                    lblNecEspecial.Text = dadosFichaRenovacao.NecessidadeEspecial;
                    lblMatricula.Text = dadosFichaRenovacao.AlunoMatricula;
                    lblDataSit.Text = dadosFichaRenovacao.DataSituacao != null ? Convert.ToDateTime(dadosFichaRenovacao.DataSituacao).ToString("dd/MM/yyyy") : string.Empty;
                    lblHoraSit.Text = dadosFichaRenovacao.DataSituacao != null ? Convert.ToDateTime(dadosFichaRenovacao.DataSituacao).ToString("HH:mm") : string.Empty;

                    lblNomeMae.Text = dadosFichaRenovacao.NomeMae;
                    lblMaeFalecida.Text = dadosFichaRenovacao.FalecidaMae;
                    lblCPFMae.Text = dadosFichaRenovacao.CPFMae;

                    lblNomePai.Text = dadosFichaRenovacao.NomePai;
                    lblPaiFalecido.Text = dadosFichaRenovacao.FalecidoPai;
                    lblCPFPai.Text = dadosFichaRenovacao.CPFPai;

                    lblRespLegal.Text = dadosFichaRenovacao.ResponsavelLegal;
                    lblNomeOutros.Text = dadosFichaRenovacao.NomeOutros;
                    lblCPFOutros.Text = dadosFichaRenovacao.CpfOutros;

                    lblTelMae.Text = dadosFichaRenovacao.TelMae;
                    lblTelPai.Text = dadosFichaRenovacao.TelPai;
                    lblTelResp.Text = dadosFichaRenovacao.TelResponsavel;

                    lblEndereco.Text = dadosFichaRenovacao.Endereco;
                    lblNumero.Text = dadosFichaRenovacao.NumeroEndereco;
                    lblComplemento.Text = dadosFichaRenovacao.ComplementoEndereco;
                    lblBairro.Text = dadosFichaRenovacao.BairroEndereco;
                    lblMunicipio.Text = dadosFichaRenovacao.MunicipioEndereco;
                    lblEstado.Text = dadosFichaRenovacao.EstadoEndereco;
                    lblCEP.Text = dadosFichaRenovacao.CepEndereco;
                    lblLocalizacao.Text = dadosFichaRenovacao.LocalizacaoEndereco;

                    lblContatoTelefone.Text = dadosFichaRenovacao.Telefone;
                    lblContatoCelular.Text = dadosFichaRenovacao.Celular;
                    lblContatoEmail.Text = dadosFichaRenovacao.Email;

                    lblNumDocDetran.Text = dadosFichaRenovacao.NumeroRGDetran;
                    lblDataRGDetran.Text = dadosFichaRenovacao.DataExpedicaoRGDetran != null ? Convert.ToDateTime(dadosFichaRenovacao.DataExpedicaoRGDetran).ToString("dd/MM/yyyy") : string.Empty;

                    lblCPFDocumento.Text = dadosFichaRenovacao.Cpf;
                    lblTipoDocumento.Text = dadosFichaRenovacao.TipoDocumento;
                    lblNumeroDocumento.Text = dadosFichaRenovacao.NumeroDocumento;
                    lblComplIdent.Text = dadosFichaRenovacao.ComplementoIdentidade;
                    lblEstadoDocumento.Text = dadosFichaRenovacao.EstadoDocumento;
                    lblOrgaoEmissor.Text = dadosFichaRenovacao.OrgaoEmissorDocumento;
                    lblDtExpedicaoDocumento.Text = dadosFichaRenovacao.DataExpedicaoDocumento != null ? Convert.ToDateTime(dadosFichaRenovacao.DataExpedicaoDocumento).ToString("dd/MM/yyyy") : string.Empty;

                    lblINEP.Text = dadosFichaRenovacao.Inep;
                    lblNIS.Text = dadosFichaRenovacao.Nis;

                    lblTipoCertidao.Text = dadosFichaRenovacao.TipoCertidao;
                    lblCertidaoCivil.Text = dadosFichaRenovacao.CertidaoCivil;
                    lblUFCartorio.Text = dadosFichaRenovacao.UfCartorio;
                    lblMunicipioCartorio.Text = dadosFichaRenovacao.MunicipioCartorio;
                    lblNomeCartorio.Text = dadosFichaRenovacao.NomeCartorio;
                    lblLivro.Text = dadosFichaRenovacao.Livro;
                    lblFolha.Text = dadosFichaRenovacao.Folha;
                    lblTermo.Text = dadosFichaRenovacao.NumeroTermo;
                    lblDtEmissaoCertidao.Text = dadosFichaRenovacao.DataEmissaoCertidao != null ? Convert.ToDateTime(dadosFichaRenovacao.DataEmissaoCertidao).ToString("dd/MM/yyyy") : string.Empty; 
                    lblMatriculaCertidao.Text = dadosFichaRenovacao.MatriculaCertidao;

                    lblUtilizaTransporte.Text = dadosFichaRenovacao.UtilizaTransporte;
                    lblPoderResponsavel.Text = dadosFichaRenovacao.PoderResponsavel;

                    if (!string.IsNullOrEmpty(dadosFichaRenovacao.Modais))
                    {
                        string[] modais = dadosFichaRenovacao.Modais.Split(';');
                        foreach (String str in modais)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                chkModais.Items.FindByValue(str).Selected = true;
                            }
                        }
                    }

                    lblMatriculaRenovacaoViaAluno.Text = lblMatriculaRenovacaoViaUnidade.Text = dadosFichaRenovacao.AlunoMatricula;
                    lblNomeViaAluno.Text = lblNomeViaUnidade.Text = dadosFichaRenovacao.NomeAluno;
                    lblAnoLetivoViaAluno.Text = lblAnoLetivoViaUnidade.Text = dadosFichaRenovacao.AnoLetivo.ToString();
                    lblPeriodoLetivoViaAluno.Text = lblPeriodoLetivoViaUnidade.Text = dadosFichaRenovacao.PeriodoLetivo.ToString();
                    lblUnidadeViaAluno.Text = lblUnidadeViaUnidade.Text = dadosFichaRenovacao.UnidadeEnsino;
                    lblCensoViaAluno.Text = lblCensoViaUnidade.Text = dadosFichaRenovacao.Censo;
                    lblModalidadeViaAluno.Text = lblModalidadeViaUnidade.Text = dadosFichaRenovacao.Modalidade;
                    lblCursoViaAluno.Text = lblCursoViaUnidade.Text = dadosFichaRenovacao.Curso;
                    lblNomeCursoViaAluno.Text = lblNomeCursoViaUnidade.Text = dadosFichaRenovacao.CursoDescricao;
                    lblSerieViaAluno.Text = lblSerieViaUnidade.Text = dadosFichaRenovacao.Serie.ToString();
                    lblTurnoViaAluno.Text = lblTurnoViaUnidade.Text = dadosFichaRenovacao.Turno;
                    lblDataSugeridaViaAluno.Text = lblDataSugeridaViaUnidade.Text = dadosFichaRenovacao.DataSugerida != null ? Convert.ToDateTime(dadosFichaRenovacao.DataSugerida).ToString("dd/MM/yyyy") : string.Empty;
                    lblEnsReligiosoViaAluno.Text = lblEnsReligiosoViaUnidade.Text = dadosFichaRenovacao.EnsinoReligioso;
                    lblOptativaViaAluno.Text = lblOptativaViaUnidade.Text = dadosFichaRenovacao.LinguaEstrangueira;
                    lblSituacaoViaAluno.Text = lblSituacaoViaUnidade.Text = ((RN.RenovacaoMatricula.Entidades.SituacaoRenovacao)Enum.Parse(typeof(RN.RenovacaoMatricula.Entidades.SituacaoRenovacao), Convert.ToString(dadosFichaRenovacao.Situacao))).GetStringValue();
//dadosFichaRenovacao.Situacao;
                    lblDataSituacaoViaAluno.Text = lblDataSituacaoViaUnidade.Text = dadosFichaRenovacao.DataSituacao != null ? Convert.ToDateTime(dadosFichaRenovacao.DataSituacao).ToString("dd/MM/yyyy") : string.Empty;
                    lblHoraSituacaoViaAluno.Text = lblHoraSituacaoViaUnidade.Text = dadosFichaRenovacao.DataSituacao != null ? Convert.ToDateTime(dadosFichaRenovacao.DataSituacao).ToString("HH:mm") : string.Empty;

                    lblMatriculaServidorViaAluno.Text = lblMatriculaServidorViaUnidade.Text = dadosFichaRenovacao.UsuarioResponsavel;
                    lblNomeServidorViaAluno.Text = lblNomeServidorViaUnidade.Text = dadosFichaRenovacao.UsuarioResponsavelNome;

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
