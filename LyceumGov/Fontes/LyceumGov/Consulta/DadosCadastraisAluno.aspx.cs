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

namespace Techne.Lyceum.Net.Consulta
{
    [NavUrl("~/Consulta/DadosCadastraisAluno.aspx"),
    ControlText("Dados Cadastrais do Aluno"),
    Title("Dados Cadastrais do Aluno")]
    public partial class DadosCadastraisAluno : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;            
        }

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            try
            {
                pnDadosAluno.Visible = false;
                LimpaDadosAluno();

                if (!tseAluno.DBValue.IsNull)
                {
                    if (tseAluno.IsValidDBValue)
                    {
                        lblMensagem.Text = string.Empty;
                        pnDadosAluno.Visible = true;
                        this.CarregaDadosAluno(Convert.ToString(tseAluno.DBValue));
                    }
                    else
                    {
                        lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Favor preencher o campo Aluno.";
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

        private void CarregaDadosAluno(string aluno)
        {
            try
            {
                RN.DTOs.DadosFichaAluno dadosFichaConfirmacao;
                RN.Aluno rnAluno = new Aluno();
                if (aluno != string.Empty)
                {
                    dadosFichaConfirmacao = rnAluno.ObtemFichaAlunoPor(aluno);

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
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimpaDadosAluno()
        {
            CarregaFoto(null);
            lblNome.Text = string.Empty;
            lblDataNascimento.Text = string.Empty;
            lblSexo.Text = string.Empty;
            lblQtdeFilhos.Text = string.Empty;
            lblTipoSanguineo.Text = string.Empty;
            lblEtnia.Text = string.Empty;
            lblEstadoCivil.Text = string.Empty;
            lblPaisNasc.Text = string.Empty;
            lblNacionalidade.Text = string.Empty;
            lblUFNasc.Text = string.Empty;
            lblNaturalidade.Text = string.Empty;
            lblCredo.Text = string.Empty;
            lblNecEspecial.Text = string.Empty;
            lblMatricula.Text = string.Empty;
            lblNomeMae.Text = string.Empty;
            lblMaeFalecida.Text = string.Empty;
            lblCPFMae.Text = string.Empty;
            lblNomePai.Text = string.Empty;
            lblPaiFalecido.Text = string.Empty;
            lblCPFPai.Text = string.Empty;
            lblRespLegal.Text = string.Empty;
            lblNomeOutros.Text = string.Empty;
            lblCPFOutros.Text = string.Empty;
            lblTelMae.Text = string.Empty;
            lblTelPai.Text = string.Empty;
            lblTelResp.Text = string.Empty;
            lblEndereco.Text = string.Empty;
            lblNumero.Text = string.Empty;
            lblComplemento.Text = string.Empty;
            lblBairro.Text = string.Empty;
            lblMunicipio.Text = string.Empty;
            lblEstado.Text = string.Empty;
            lblCEP.Text = string.Empty;
            lblLocalizacao.Text = string.Empty;
            lblContatoTelefone.Text = string.Empty;
            lblContatoCelular.Text = string.Empty;
            lblContatoEmail.Text = string.Empty;
            lblCPFDocumento.Text = string.Empty;
            lblTipoDocumento.Text = string.Empty;
            lblNumeroDocumento.Text = string.Empty;
            lblComplIdent.Text = string.Empty;
            lblEstadoDocumento.Text = string.Empty;
            lblOrgaoEmissor.Text = string.Empty;
            lblDtExpedicaoDocumento.Text = string.Empty;
            lblINEP.Text = string.Empty;
            lblNIS.Text = string.Empty;
            lblTipoCertidao.Text = string.Empty;
            lblCertidaoCivil.Text = string.Empty;
            lblUFCartorio.Text = string.Empty;
            lblMunicipioCartorio.Text = string.Empty;
            lblNomeCartorio.Text = string.Empty;
            lblLivro.Text = string.Empty;
            lblFolha.Text = string.Empty;
            lblTermo.Text = string.Empty;
            lblDtEmissaoCertidao.Text = string.Empty;
            lblMatriculaCertidao.Text = string.Empty;
        }
    }
}
