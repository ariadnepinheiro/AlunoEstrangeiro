using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using Techne.Web;

namespace Techne.Lyceum.Net.Matricula
{
    [
      NavUrl("~/Matricula/CadastroCandidato.aspx"),
       ControlText("Cadastro de Candidato"),
       Title("Cadastro de Candidato"),
  ]

    public partial class CadastroCandidato : TPage
    {
        public object ListarOpcaoIrmaoForaRede(object inscricaoAlunoId)
        {
            RN.Matriculas.OpcaoInscricao opcao = new Techne.Lyceum.RN.Matriculas.OpcaoInscricao();

            var inscricao = inscricaoAlunoId != null ? inscricaoAlunoId.ToString() : null;

            if (!inscricao.IsNullOrEmptyOrWhiteSpace())
            {
                return opcao.ListaPor(Convert.ToInt32(inscricao));

            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                RN.Matriculas.InscricaoAluno inscricao = new Techne.Lyceum.RN.Matriculas.InscricaoAluno();
                RN.DTOs.DadosCandidato dados = new Techne.Lyceum.RN.DTOs.DadosCandidato();

                if (!IsPostBack)
                {
                    divPrincipal.Visible = false;
                    LimpaDados();
                    if (Request.QueryString.Keys.Count > 0)
                    {
                        if (Request.QueryString["Chave"] != null)
                        {
                            byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["chave"]);
                            string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                            dados = inscricao.ObtemDadosCandidatoPor(Convert.ToInt32(decodedText));

                            if (dados.InscricaoAlunoId > 0)
                            {
                                PreencherDados(dados);
                                divPrincipal.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Para visualizar os dados é necessario escolher um candidato.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdOpcoesIrmaoForaRede, "Lista de Escolas escolhidas pelo seu irmão");
        }

        private void PreencherDados(RN.DTOs.DadosCandidato dados)
        {
            hdnInscricao.Value = dados.InscricaoAlunoId.ToString();
            txtNumeroInscricao.Text = dados.NumeroInscricao.ToString();
            txtNomeCompl.Text = dados.Nome;

            if (dados.DataNascimento.HasValue)
            {
                txtDtNascimento.Text = dados.DataNascimento.Value.ToString("dd/MM/yyyy");
            }

            txtSexo.Text = dados.Sexo;
            txtEstadoCivil.Text = dados.EstadoCivil;
            txtRedeOrigem.Text = dados.RedeOrigem;
            txtNacionalidade.Text = dados.Nacionalidade;
            txtUFNascimento.Text = dados.UfNascimento;
            txtMunicipioNascimento.Text = dados.DescricaoMunicipioNascimento;
            txtNomePai.Text = dados.NomePai;
            txtNomeMae.Text = dados.NomeMae;
            chkNaoDeclarMae.Checked = dados.DeclaroAusenciaMae;
            chkNaoDeclarPai.Checked = dados.DeclaroAusenciaPai;

            Int64 resultadoCEP;
            var cep = !dados.Cep.IsNullOrEmptyOrWhiteSpace() ? dados.Cep.Replace("-", "") : string.Empty;
            if (Int64.TryParse(cep, out resultadoCEP))
            {
                if (resultadoCEP != 0)
                {
                    txtCEP.Text = string.Format(@"{0:00000-000}", resultadoCEP);
                }
                else
                {
                    txtCEP.Text = resultadoCEP.ToString();
                }
            }

            txtLogradouro.Text = dados.Endereco;
            txtUFEndereco.Text = dados.UfEndereco;
            txtNumero.Text = dados.NumeroEndereco;
            txtComplemento.Text = dados.ComplementoEndereco;
            txtMunicipioEndereco.Text = dados.DescricaoMunicipioEndereco;
            txtBairro.Text = dados.Bairro;
            Int64 resultadoCPF;

            if (Int64.TryParse(dados.Cpf.RetirarMascaraCPF(), out resultadoCPF))
            {
                if (resultadoCPF != 0)
                {
                    txtCPF.Text = string.Format(@"{0:000\.000\.000-00}", resultadoCPF);
                }
                else
                {
                    txtCPF.Text = resultadoCPF.ToString();
                }
            }

            txtRgNumero.Text = dados.NumeroRG;
            txtRgEmissor.Text = dados.OrgaoRG;
            txtRgUf.Text = dados.UFRG;

            rblModeloCertidao.SelectedValue = dados.ModeloCertidao;
            rblTipoCertidao.SelectedValue = dados.TipoCertidao;

            if (rblModeloCertidao.SelectedValue == "Modelo Novo")
            {
                pnlNovo.Visible = true;
                pnlAntigo.Visible = false;
            }
            else if (rblModeloCertidao.SelectedValue == "Modelo Antigo")
            {
                pnlNovo.Visible = false;
                pnlAntigo.Visible = true;
            }
            else
            {
                pnlNovo.Visible = false;
                pnlAntigo.Visible = false;
            }

            txtDOC_CertNasc_Numero.Text = dados.TermoCertidao;
            txtDOC_CertNasc_Folha.Text = dados.FolhaCertidao;
            txtDOC_CertNasc_Livro.Text = dados.LivroCertidao;
            txtNumMatriculaCertidao.Text = dados.MatriculaCertidao;
            txtDeficiencia.Text = dados.DescricaoNecessidadeEspecial;
            rblResponsavel.SelectedValue = dados.Responsavel;
            txtResponsavel.Text = dados.ResponsavelNome;

            Int64 resultadoCPFResponsavel;

            if (Int64.TryParse(dados.ResponsavelCpf.RetirarMascaraCPF(), out resultadoCPFResponsavel))
            {
                if (resultadoCPFResponsavel != 0)
                {
                    txtCPFResponsavel.Text = string.Format(@"{0:000\.000\.000-00}", resultadoCPFResponsavel);
                }
                else
                {
                    txtCPFResponsavel.Text = resultadoCPFResponsavel.ToString();
                }
            }

            txtRgResponsavelNumero.Text = dados.ResponsavelNumeroRG;
            txtRgResponsavelEmissor.Text = dados.ResposanvelEmissorRG;
            txtRgResponsavelUf.Text = dados.ResposanvelUFRG;

            long resultadoTelResponsavel;
            var telResponsavel = dados.ResponsavelFone.RetirarMascaraTelefone();

            if (long.TryParse(telResponsavel, out resultadoTelResponsavel))
            {
                if (telResponsavel.Length == 10)
                {
                    txtCelularResponsavel.Text = string.Format("{0:(00)0000-0000}", resultadoTelResponsavel);
                }
                else if (telResponsavel.Length == 11)
                {
                    txtCelularResponsavel.Text = string.Format("{0:(00)00000-0000}", resultadoTelResponsavel);
                }
                else
                {
                    txtCelularResponsavel.Text = resultadoTelResponsavel.ToString();
                }
            }

            pnlIrmaoForaRede.Visible = false;
            pnlIrmaoRede.Visible = false;
            pnlNaoPossuiIrmao.Visible = false;

            if (dados.IrmaoNumeroInscricao == (int?)null && dados.IrmaoMatricula.IsNullOrEmptyOrWhiteSpace())
            {
                pnlNaoPossuiIrmao.Visible = true;
                chkNaoPossuiIrmao.Checked = true;
            }

            if (dados.IrmaoNumeroInscricao != (int?)null)
            {
                pnlIrmaoForaRede.Visible = true;
                chkIrmaoForaRede.Checked = true;
                txtInscricaoIrmao.Text = dados.IrmaoNumeroInscricao > 0 ? dados.IrmaoNumeroInscricao.ToString() : string.Empty;
                txtNomeIrmaoForaRede.Text = !dados.DadosIrmao.NomeCompl.IsNullOrEmptyOrWhiteSpace() ? dados.DadosIrmao.NomeCompl : string.Empty;
                txtDataNascIrmaoForaRede.Text = dados.DadosIrmao.DataNascimento.HasValue ? dados.DadosIrmao.DataNascimento.Value.ToShortDateString() : string.Empty;
                hdnIdIrmao.Value = dados.IrmaoIdInscricao > 0 ? dados.IrmaoIdInscricao.ToString() : string.Empty;

            }

            if (!dados.IrmaoMatricula.IsNullOrEmptyOrWhiteSpace())
            {
                pnlIrmaoRede.Visible = true;
                chkIrmaoRede.Checked = true;
                txtMatriculaIrmao.Text = !dados.IrmaoMatricula.IsNullOrEmptyOrWhiteSpace() ? dados.IrmaoMatricula : string.Empty;
                txtNomeIrmaoRede.Text = !dados.DadosIrmao.NomeCompl.IsNullOrEmptyOrWhiteSpace() ? dados.DadosIrmao.NomeCompl : string.Empty;
                txtDataNascIrmaoRede.Text = dados.DadosIrmao.DataNascimento.HasValue ? dados.DadosIrmao.DataNascimento.Value.ToShortDateString() : string.Empty;
                txtUEIrmaoRede.Text = !dados.DadosIrmao.EscolaAtual.IsNullOrEmptyOrWhiteSpace() ? dados.DadosIrmao.EscolaAtual : string.Empty;
                txtCursoIrmaoRede.Text = !dados.DadosIrmao.CursoDescricaoAtual.IsNullOrEmptyOrWhiteSpace() ? dados.DadosIrmao.CursoDescricaoAtual : string.Empty;
                txtSerieIrmaoRede.Text = dados.DadosIrmao.SerieAtual > 0 ? dados.DadosIrmao.SerieAtual.ToString() : string.Empty;
                txtTurnoIrmaoRede.Text = !dados.DadosIrmao.TurnoDescricaoAtual.IsNullOrEmptyOrWhiteSpace() ? dados.DadosIrmao.TurnoDescricaoAtual : string.Empty;
            }           

        }
        private void LimpaDados()
        {
            hdnInscricao.Value = string.Empty;
            txtNumeroInscricao.Text = string.Empty;
            txtNomeCompl.Text = string.Empty;
            txtDtNascimento.Text = string.Empty;
            txtSexo.Text = string.Empty;
            txtEstadoCivil.Text = string.Empty;
            txtRedeOrigem.Text = string.Empty;
            txtNacionalidade.Text = string.Empty;
            txtUFNascimento.Text = string.Empty;
            txtMunicipioNascimento.Text = string.Empty;
            txtNomePai.Text = string.Empty;
            txtNomeMae.Text = string.Empty;
            chkNaoDeclarMae.Checked = false;
            chkNaoDeclarPai.Checked = false;
            txtCEP.Text = string.Empty;
            txtLogradouro.Text = string.Empty;
            txtUFEndereco.Text = string.Empty;
            txtNumero.Text = string.Empty;
            txtComplemento.Text = string.Empty;
            txtMunicipioEndereco.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtCPF.Text = string.Empty;
            txtRgNumero.Text = string.Empty;
            txtRgEmissor.Text = string.Empty;
            txtRgUf.Text = string.Empty;
            rblModeloCertidao.Text = string.Empty;
            rblTipoCertidao.Text = string.Empty;
            txtDOC_CertNasc_Numero.Text = string.Empty;
            txtDOC_CertNasc_Folha.Text = string.Empty;
            txtDOC_CertNasc_Livro.Text = string.Empty;
            txtNumMatriculaCertidao.Text = string.Empty;
            txtDeficiencia.Text = string.Empty;
            rblResponsavel.Text = string.Empty;
            txtResponsavel.Text = string.Empty;
            txtCPFResponsavel.Text = string.Empty;
            txtRgResponsavelNumero.Text = string.Empty;
            txtRgResponsavelEmissor.Text = string.Empty;
            txtRgResponsavelUf.Text = string.Empty;
            txtCelularResponsavel.Text = string.Empty;

            hdnIdIrmao.Value = string.Empty;
            chkNaoPossuiIrmao.Checked = false;
            chkIrmaoForaRede.Checked = false;
            chkIrmaoRede.Checked = false;
            txtMatriculaIrmao.Text = string.Empty;
            txtInscricaoIrmao.Text = string.Empty;
            txtNomeIrmaoForaRede.Text = string.Empty;
            txtNomeIrmaoRede.Text = string.Empty;
            txtDataNascIrmaoForaRede.Text = string.Empty;
            txtDataNascIrmaoRede.Text = string.Empty;
            txtUEIrmaoRede.Text = string.Empty;
            txtCursoIrmaoRede.Text = string.Empty;
            txtSerieIrmaoRede.Text = string.Empty;
            txtTurnoIrmaoRede.Text = string.Empty;

        }
    }
}
