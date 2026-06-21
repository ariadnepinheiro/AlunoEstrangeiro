using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.Net.Basico
{
    public partial class FichaImplantacao : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                decimal NumFunc;
                if (!IsPostBack)
                {
                    if (Request.QueryString.Keys.Count > 0)
                    {
                        var decodedBytes = Convert.FromBase64String(this.Request.QueryString["Chave"]);
                        var decodedText = Encoding.UTF8.GetString(decodedBytes);

                        NumFunc = Convert.ToDecimal(decodedText.Substring(decodedText.LastIndexOf('=') + 1));

                        if (NumFunc != 0)
                        {
                            this.CarregaDadosDocente(NumFunc);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        private void CarregaDadosDocente(decimal NumFunc)
        {
            try
            {

                Techne.Lyceum.RN.DTOs.FichaImplantacaoDocente dadosDocente = new Techne.Lyceum.RN.DTOs.FichaImplantacaoDocente();
                RN.Docentes rnDocentes = new Techne.Lyceum.RN.Docentes();

                dadosDocente = rnDocentes.ObtemDadosFichaDocentePor(NumFunc);

                if (dadosDocente.DocenteId != 0)
                {
                    lblMatricula.Text = dadosDocente.Matricula;
                    if (dadosDocente.IdFuncional > 0)
                    {
                        lblIDFuncionalVinculo.Text = dadosDocente.IdFuncional.ToString() + " - " + dadosDocente.Vinculo.ToString();
                    }
                    lblNome.Text = dadosDocente.NomeCompleto;
                    lblUA.Text = dadosDocente.UnidadeAdministrativa;
                    lblMunicipioLotacao.Text = dadosDocente.MunicipioLotacao;
                    lblDataAdmissao.Text = dadosDocente.DataAdmissao.ToShortDateString();
                    if (!string.IsNullOrEmpty(dadosDocente.MatriculaAcumulucao))
                    {
                        lblAcumulacao.Text = "SIM";
                        lblMatriculaAcumulacao.Text = dadosDocente.MatriculaAcumulucao;
                        lblOrgaoAcumulacao.Text = dadosDocente.OrgaoAcumulucao;
                        lblNumProcessoAcumulacao.Text = dadosDocente.ProcessoAcumulucao;
                    }
                    else
                    {
                        lblAcumulacao.Text = "NÃO";
                    }
                    lblDisciplina.Text = dadosDocente.DisciplinaIngresso;
                    lblDataNascimento.Text = dadosDocente.DataNascimento.ToShortDateString();
                    lblSexo.Text = dadosDocente.Sexo;
                    lblCorRaca.Text = dadosDocente.Raca;
                    lblEstadoCivil.Text = dadosDocente.EstadoCivil;
                    lblNacionalidade.Text = dadosDocente.Nacionalidade;
                    lblNaturalidade.Text = dadosDocente.Naturalidade;
                    lblNomePai.Text = dadosDocente.NomePai;
                    lblNomeMae.Text = dadosDocente.NomeMae;
                    lblEndereco.Text = dadosDocente.Endereco;
                    lblNumero.Text = dadosDocente.Numero;
                    lblComplemento.Text = dadosDocente.Complemento;
                    lblBairro.Text = dadosDocente.Bairro;
                    lblMunicipio.Text = dadosDocente.Municipio;
                    lblEstado.Text = dadosDocente.Estado;
                    lblCEP.Text = dadosDocente.Cep;
                    lblNumeroDocumento.Text = dadosDocente.Identidade;
                    lblOrgaoIdentidade.Text = dadosDocente.OrgaoIdentidade;
                    lblDataExpedicao.Text = dadosDocente.DataExpedicao.ToShortDateString();
                    lblUFDoc.Text = dadosDocente.UFIdentidade;
                    if (!string.IsNullOrEmpty(dadosDocente.TituloEleitor))
                    {
                        lblTituloEleitor.Text = dadosDocente.TituloEleitor + "/" + dadosDocente.ZonaTitulo + "/" + dadosDocente.SecaoTitulo;
                    }
                    if (!string.IsNullOrEmpty(dadosDocente.Certificado))
                    {
                        lblCertificado.Text = dadosDocente.Certificado + "/" + dadosDocente.CategoriaCertificado + "/" + dadosDocente.SerieCertificado;
                    }
                    lblCPF.Text = dadosDocente.Cpf;
                    LBLPISPASEP.Text = dadosDocente.Pis;
                    lblCTPS.Text = dadosDocente.Ctps;
                    lblSerieCtps.Text = dadosDocente.SerieCtps;
                    lblAnoConcurso.Text = dadosDocente.AnoConcurso;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
