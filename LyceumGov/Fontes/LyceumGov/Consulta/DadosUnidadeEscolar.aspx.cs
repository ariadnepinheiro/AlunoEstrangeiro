using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxGridView;
using Techne.Web;

namespace Techne.Lyceum.Net.Consulta
{
    [NavUrl("~/Consulta/DadosUnidadeEscolar.aspx"), ControlText("Dados Unidade Escolar"), Title("Dados Unidade Escolar")]
    public partial class DadosUnidadeEscolar : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    pnlGeral.Visible = false;
                    LimpaTela();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {           
            TituloGrid(grdEquipe, "Equipe Técnico-Pedagógica");
        }
        protected void LimpaTela()
        {
            lblCenso.Text = string.Empty;
            lblNomeUnidade.Text = string.Empty;
            lblDiretoriaRegional.Text = string.Empty;
            lblNucleo.Text = string.Empty;
            lblSetor.Text = string.Empty;
            lblDependenciaAdministrativa.Text = string.Empty;
            lblEmail.Text = string.Empty;
            lblCGC.Text = string.Empty;
            lblClassificacao.Text = string.Empty;
            lblFone.Text = string.Empty;
            lblFone2.Text = string.Empty;
            lblFax.Text = string.Empty;
            lblLocalFuncionamento.Text = string.Empty;
            lblLocalizacaoUF.Text = string.Empty;
            chkAreaAssentamento.Checked = false;
            chkTerraIndigena.Checked = false;
            chkSustentavel.Checked = false;
            chkQuilombos.Checked = false;
            chkNaoSeAplica.Checked = false;
            lblCEPUF.Text = string.Empty;
            lblMunicipioUF.Text = string.Empty;
            lblLogradouroUF.Text = string.Empty;
            lblNumeroEndUF.Text = string.Empty;
            lblEstadoUF.Text = string.Empty;
            lblComplementoUF.Text = string.Empty;
            lblBairro.Text = string.Empty;
            lblDistritoUF.Text = string.Empty;
        }
        protected void tseUnidade_Changed(object sender, EventArgs e)
        {
            try
            {
                RN.UnidadeEnsino rnUnidadeEnsino = new Techne.Lyceum.RN.UnidadeEnsino();
                RN.UnidadeFisica rnUnidadeFisica = new Techne.Lyceum.RN.UnidadeFisica();
                RN.DTOs.UnidadeCaracteristicasFisicas infoFisicas = new UnidadeCaracteristicasFisicas();
                RN.DTOs.UnidadeInformacoesGerais infoGerais = new UnidadeInformacoesGerais();

                LimpaTela();
                pnlGeral.Visible = false;

                if (tseUnidade.IsValidDBValue && !tseUnidade.DBValue.IsNull)
                {
                    pnlGeral.Visible = true;
                    infoGerais = rnUnidadeEnsino.ObtemInformacoesGeraisPor(tseUnidade.DBValue.ToString());

                    if (!infoGerais.Censo.IsNullOrEmptyOrWhiteSpace())
                    {
                        lblCenso.Text = infoGerais.Censo;
                        lblNomeUnidade.Text = !infoGerais.NomeUnidade.IsNullOrEmptyOrWhiteSpace() ? infoGerais.NomeUnidade : string.Empty;
                        lblDiretoriaRegional.Text = !infoGerais.NomeRegional.IsNullOrEmptyOrWhiteSpace() ? infoGerais.NomeRegional : string.Empty;
                        lblNucleo.Text = !infoGerais.NomeCoordenadoria.IsNullOrEmptyOrWhiteSpace() ? infoGerais.NomeCoordenadoria : string.Empty;
                        lblSetor.Text = !infoGerais.UnidadeAdministrativa.IsNullOrEmptyOrWhiteSpace() ? infoGerais.UnidadeAdministrativa : string.Empty;
                        lblClassificacao.Text = !infoGerais.Classificacao.IsNullOrEmptyOrWhiteSpace() ? infoGerais.Classificacao : string.Empty;
                        lblSituacaoFuncionamento.Text = !infoGerais.SituacaoFuncionamento.IsNullOrEmptyOrWhiteSpace() ? infoGerais.SituacaoFuncionamento : string.Empty;

                        lblCEPUF.Text = !infoGerais.Cep.IsNullOrEmptyOrWhiteSpace() ? infoGerais.Cep : string.Empty;
                        lblLogradouroUF.Text = !infoGerais.Endereco.IsNullOrEmptyOrWhiteSpace() ? infoGerais.Endereco : string.Empty;
                        lblBairro.Text = !infoGerais.EnderecoBairro.IsNullOrEmptyOrWhiteSpace() ? infoGerais.EnderecoBairro : string.Empty;
                        lblNumeroEndUF.Text = !infoGerais.EnderecoNumero.IsNullOrEmptyOrWhiteSpace() ? infoGerais.EnderecoNumero : string.Empty;
                        lblComplementoUF.Text = !infoGerais.EnderecoComplemento.IsNullOrEmptyOrWhiteSpace() ? infoGerais.EnderecoComplemento : string.Empty;
                        lblMunicipioUF.Text = !infoGerais.MunicipioDescricao.IsNullOrEmptyOrWhiteSpace() ? infoGerais.MunicipioDescricao : string.Empty;
                        lblEstadoUF.Text = !infoGerais.UF.IsNullOrEmptyOrWhiteSpace() ? infoGerais.UF : string.Empty;
                        lblDistritoUF.Text = !infoGerais.Distrito.IsNullOrEmptyOrWhiteSpace() ? infoGerais.Distrito : string.Empty;
                        
                    }

                    infoFisicas = rnUnidadeFisica.ObtemCaracteristicasFisicasPor(tseUnidade.DBValue.ToString());

                    if (!infoFisicas.UnidadeFisica.IsNullOrEmptyOrWhiteSpace())
                    {
                        lblDependenciaAdministrativa.Text = !infoFisicas.DependenciaAdministrativa.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.DependenciaAdministrativa : string.Empty;
                        lblEmail.Text = !infoFisicas.Email.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.Email : string.Empty;
                        lblCGC.Text = !infoFisicas.Cnpj.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.Cnpj.ToString().AplicarMascaraCNPJ() : string.Empty;
                        lblFone.Text = !infoFisicas.Telefone1.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.Telefone1.AplicarMascaraCelularComDDD() : string.Empty;
                        lblFone2.Text = !infoFisicas.Telefone2.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.Telefone2.AplicarMascaraCelularComDDD() : string.Empty;
                        lblFax.Text = !infoFisicas.Fax.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.Fax.AplicarMascaraCelularComDDD() : string.Empty;
                                                                      
                        lblLocalFuncionamento.Text = !infoFisicas.LocalFuncionamento.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.LocalFuncionamento : string.Empty;
                        lblLocalizacaoUF.Text = !infoFisicas.FormaOcupacaoLocalizacao.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.FormaOcupacaoLocalizacao : string.Empty;
                        chkAreaAssentamento.Checked = !infoFisicas.AreaAssentamento.IsNullOrEmptyOrWhiteSpace() ? (infoFisicas.AreaAssentamento == "S" ? true : false) : false;
                        chkTerraIndigena.Checked = !infoFisicas.TerraIndigena.IsNullOrEmptyOrWhiteSpace() ? (infoFisicas.TerraIndigena == "S" ? true : false) : false;
                        chkSustentavel.Checked = !infoFisicas.UnidadeSustentavel.IsNullOrEmptyOrWhiteSpace() ? (infoFisicas.UnidadeSustentavel == "S" ? true : false) : false;
                        chkQuilombos.Checked = !infoFisicas.AreaQuilombo.IsNullOrEmptyOrWhiteSpace() ? (infoFisicas.AreaQuilombo == "S" ? true : false) : false;
                        chkNaoSeAplica.Checked = (!chkAreaAssentamento.Checked && !chkTerraIndigena.Checked && !chkSustentavel.Checked && !chkQuilombos.Checked) ? true : false;
                        

                    }
                }
                else
                {
                    if (!tseUnidade.DBValue.IsNull)
                    {
                        lblMensagem.Text = "Unidade de Ensino não cadastrada.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void grdEquipe_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "fone" && e.Value != null)
            {
                decimal fone;

                if (decimal.TryParse(e.Value.ToString().Replace(" ", string.Empty), out fone))
                {
                    e.DisplayText = string.Format("{0:(00)0000-0000}", fone);
                }
                else
                {
                    e.DisplayText = string.Empty;
                }
            }

            if (e.Column.FieldName == "celular" && e.Value != null)
            {
                decimal celular;

                if (decimal.TryParse(e.Value.ToString().Replace(" ", string.Empty), out celular))
                {
                    if (e.Value.ToString().Replace(" ", string.Empty).Length == 10)
                    {
                        e.DisplayText = string.Format("{0:(00)0000-0000}", celular);
                    }
                    if (e.Value.ToString().Replace(" ", string.Empty).Length == 11)
                    {
                        e.DisplayText = string.Format("{0:(00)00000-0000}", celular);
                    }

                }
                else
                {
                    e.DisplayText = string.Empty;
                }
            }
        }
    }
}
