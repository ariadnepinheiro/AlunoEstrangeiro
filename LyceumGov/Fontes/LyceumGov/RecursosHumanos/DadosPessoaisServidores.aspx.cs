using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Configuration;


namespace Techne.Lyceum.Net.RecursosHumanos
{
    [NavUrl("~/RecursosHumanos/DadosPessoaisServidores.aspx")]
    [ControlText("DadosPessoaisServidores")]
    [Title("Dados Pessoais Servidores")]


    public partial class DadosPessoaisServidores : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    CarregaCombo();

                    tseUsuario.DBValue = User.Identity.Name;

                    var dadosPessoa = Pessoa.Carregar(Convert.ToInt32(tseUsuario["pessoa"].ToString()));

                    if (dadosPessoa == null)
                    {
                        lblMensagem.Text = "Pessoa não cadastrada.";
                    }
                    else
                    {
                        LimparTela();

                        CarregaDadosPessoa(dadosPessoa);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            ControlarEnderecoPais();
            tseUsuario.Enabled = false;
        }

        protected void tseMunicipio_Changed(object sender, EventArgs e)
        {
            if (!tseMunicipio.DBValue.IsNull && tseMunicipio.IsValidDBValue)
            {
                txtEstado.Value = tseMunicipio["uf_sigla"].ToString();
            }
        }

        protected void tseNaturalidade_Changed(object sender, EventArgs e)
        {
            if (!tseNaturalidade.DBValue.IsNull && tseNaturalidade.IsValidDBValue)
            {
                txtEstadoNaturalidade.Value = tseNaturalidade["uf_sigla"].ToString();
            }
        }

        private void CarregaDadosPessoa(LyPessoa dadosPessoa)
        {
            RN.FlPessoa rnFlPessoa = new FlPessoa();

            if (!string.IsNullOrEmpty(dadosPessoa.Pais_nasc))
            {
                string descricaoPais = RN.Endereco.ObterPais(dadosPessoa.Pais_nasc);

                //verifica se valor não é Brasil
                if (descricaoPais.ToUpper() != "BRASIL")
                {
                    //obtém o municipio estrangeiro
                    SimpleRow sr = RN.Endereco.ObterMunicipioEstrangeiro(dadosPessoa.Municipio_nasc);

                    //verifica se a função retornou algum valor para a simplerow
                    if (sr != null)
                    {
                        //preenche os dados obtidos de municipio estrangeiro
                        if (!sr["nome"].IsNull)
                            txtMunicipioNaturalidade.Text = Convert.ToString(sr["nome"]);

                        if (!sr["nome_estado"].IsNull)
                            txtEstadoNaturalidade.Value = Convert.ToString(sr["nome_estado"]);
                    }
                }
                else //se for Brasil
                {
                    //verifica se existe valor para municipio
                    if (!string.IsNullOrEmpty(dadosPessoa.Municipio_nasc))
                    {
                        //preenche os dados nos controles da tela
                        tseNaturalidade.DBValue = dadosPessoa.Municipio_nasc;
                        //obtém a UF de acordo com o codigo do municipío
                        txtEstadoNaturalidade.Value = RN.Endereco.ObterUFMunicipio(dadosPessoa.Municipio_nasc);

                        if (!tseNaturalidade.IsValidDBValue)
                        {
                            tseNaturalidade.DBValue = string.Empty;
                        }
                    }
                    else
                    {
                        tseNaturalidade.ResetValue();
                        txtEstadoNaturalidade.Value = string.Empty;
                    }
                }
            }

            txtNomeMae.Text = dadosPessoa.NomeMae;
            txtNomePai.Text = dadosPessoa.NomePai;

            chkMaeNaoDeclarada.Checked = dadosPessoa.NomeMae == chkMaeNaoDeclarada.Text.ToUpper();
            chkPaiNaoDeclarado.Checked = dadosPessoa.NomePai == chkPaiNaoDeclarado.Text.ToUpper();

            if (chkMaeNaoDeclarada.Checked)
            {
                txtNomeMae.ReadOnly = true;
            }

            if (chkPaiNaoDeclarado.Checked)
            {
                txtNomePai.ReadOnly = true;
            }

            //verifica se retornou valor para pais
            if (!string.IsNullOrEmpty(dadosPessoa.End_pais))
            {
                string descricaoPais = RN.Endereco.ObterPais(dadosPessoa.End_pais);

                //verifica se valor não é Brasil
                if (descricaoPais.ToUpper() != "BRASIL" && dadosPessoa.End_pais.ToUpper() != "BRASIL")
                {
                    //obtém o municipio estrangeiro
                    SimpleRow sr = RN.Endereco.ObterMunicipioEstrangeiro(dadosPessoa.End_municipio);

                    //verifica se a função retornou algum valor para a simplerow
                    if (sr != null)
                    {
                        //preenche os dados obtidos de municipio estrangeiro
                        if (!sr["nome"].IsNull)
                            txtMunicipio.Text = Convert.ToString(sr["nome"]);

                        if (!sr["nome_estado"].IsNull)
                            txtEstado.Value = Convert.ToString(sr["nome_estado"]);
                    }
                }
                else //se for Brasil
                {
                    //verifica se existe valor para municipio
                    if (!string.IsNullOrEmpty(dadosPessoa.End_municipio))
                    {
                        //preenche os dados nos controles da tela
                        tseMunicipio.DBValue = dadosPessoa.End_municipio;
                        //obtém a UF de acordo com o codigo do municipío
                        txtEstado.Value = RN.Endereco.ObterUFMunicipio(dadosPessoa.End_municipio);

                        if (!tseMunicipio.IsValidDBValue)
                        {
                            tseMunicipio.DBValue = string.Empty;
                        }
                    }
                    else
                    {
                        tseMunicipio.ResetValue();
                        txtEstado.Value = string.Empty;
                    }
                }
            }


            hdnPessoa.Value = dadosPessoa.Pessoa.ToString();
            txtNomeSocial.Text = dadosPessoa.Nome_social;
            txtNomeCompl.Text = dadosPessoa.Nome_compl;
            txtCEP.Text = dadosPessoa.Cep;
            txtEndereco.Text = dadosPessoa.Endereco;
            txtEndNum.Text = dadosPessoa.End_num;
            txtEndCompl.Text = dadosPessoa.End_compl;
            txtBairro.Text = dadosPessoa.Bairro;
            ddlRaca.SelectedValue = dadosPessoa.Etnia;
            ddlPaisNasc.SelectedValue = dadosPessoa.Pais_nasc;
            ddlPais.SelectedValue = dadosPessoa.End_pais;

            Int64 result;
            if (Int64.TryParse(dadosPessoa.Fone, out result))
                txtFone.Text = string.Format("{0:(00)0000-0000}", result);
            else
                txtFone.Text = dadosPessoa.Fone;


            long resultado;
            if (long.TryParse(dadosPessoa.Celular, out resultado))
            {
                if (dadosPessoa.Celular.Length == 10)
                {
                    txtCelular.Text = string.Format("{0:(00)0000-0000}", resultado);
                }
                else
                {
                    txtCelular.Text = string.Format("{0:(00)00000-0000}", resultado);
                }
            }
            else
            {
                txtCelular.Text = dadosPessoa.Celular;
            }


            txtEmailInstitucional.Text = !dadosPessoa.E_mail_interno.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.E_mail_interno : string.Empty;
            txtEmail.Text = !dadosPessoa.E_mail.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.E_mail : string.Empty;
            txtEmailGoogle.Text = !dadosPessoa.E_mail_google.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.E_mail_google : string.Empty;


            PreencherDadoCombo(ddlNecessidadeEspecial, Convert.ToString(dadosPessoa.NecessidadeEspecialId));


            if (!string.IsNullOrEmpty(dadosPessoa.Est_civil))
            {
                if (ddlEstadoCivil.Items.FindByValue(dadosPessoa.Est_civil) != null)
                {
                    ddlEstadoCivil.SelectedValue = dadosPessoa.Est_civil;
                }
                else
                {
                    ddlEstadoCivil.SelectedValue = string.Empty;
                }
            }

            if (!string.IsNullOrEmpty(dadosPessoa.Nacionalidade))
            {
                if (ddlNacionalidade.Items.FindByValue(dadosPessoa.Nacionalidade) != null)
                {
                    ddlNacionalidade.SelectedValue = dadosPessoa.Nacionalidade;
                }
            }

            if (dadosPessoa.Dt_nasc.HasValue)
                dteDtNasc.Date = dadosPessoa.Dt_nasc.Value;

            if (!string.IsNullOrEmpty(dadosPessoa.Sexo))
            {
                if (rblSexo.Items.FindByValue(dadosPessoa.Sexo) != null)
                {
                    rblSexo.Text = dadosPessoa.Sexo;
                }
            }


            chkAreaAssentamento.Checked = !dadosPessoa.AreaAssentamento.IsNullOrEmptyOrWhiteSpace() ? (dadosPessoa.AreaAssentamento == "S" ? true : false) : false;
            chkTerraIndigena.Checked = !dadosPessoa.TerraIndigena.IsNullOrEmptyOrWhiteSpace() ? (dadosPessoa.TerraIndigena == "S" ? true : false) : false;
            chkQuilombos.Checked = !dadosPessoa.AreaQuilombos.IsNullOrEmptyOrWhiteSpace() ? (dadosPessoa.AreaQuilombos == "S" ? true : false) : false;
            chkNaoSeAplica.Checked = (!chkAreaAssentamento.Checked && !chkTerraIndigena.Checked && !chkQuilombos.Checked) ? true : false;

            chkNaoSeAplica_CheckedChanged(null, null);

            //Busca Zona Residencial
            string zonaResidencial = rnFlPessoa.ObtemZonaResidencialPor(dadosPessoa.Pessoa);
            if (!zonaResidencial.IsNullOrEmptyOrWhiteSpace())
            {
                rblLocalizacaoUF.SelectedValue = zonaResidencial;
            }

            ddlPovoIndigena.Visible = false;
            lblPovo.Visible = false;
            ddlPovoIndigena.ClearSelection();
            if (ddlRaca.SelectedValue == "Índigena")
            {
                CarregaPovoIndigena();
                ddlPovoIndigena.Visible = true;
                lblPovo.Visible = true;

                string povoIndigena = rnFlPessoa.ObtemPovoIndigenaPor(dadosPessoa.Pessoa);
                if (!povoIndigena.IsNullOrEmptyOrWhiteSpace())
                {
                    ddlPovoIndigena.SelectedValue = povoIndigena;
                }
            }
        }

        protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimparEndereco();
        }

        protected void ddlPaisNasc_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimparEnderecoNascimento();
        }

        private void LimparEndereco()
        {
            txtMunicipio.Text = string.Empty;
            tseMunicipio.ResetValue();
            txtEstado.Value = string.Empty;
            txtEndereco.Text = string.Empty;
            txtCEP.Text = string.Empty;
            txtEndCompl.Text = string.Empty;
            txtEndNum.Text = string.Empty;
            txtBairro.Text = string.Empty;
            chkAreaAssentamento.Checked = false;
            chkTerraIndigena.Checked = false;
            chkQuilombos.Checked = false;
            chkNaoSeAplica.Checked = false;
            rblLocalizacaoUF.ClearSelection();

        }

        private void LimparEnderecoNascimento()
        {
            txtMunicipioNaturalidade.Text = string.Empty;
            tseNaturalidade.ResetValue();
            txtEstadoNaturalidade.Value = string.Empty;
        }


        protected void CarregaCombo()
        {
            CarregaEstadoCivil();
            CarregaPais();
            CarregaPaisNasc();
            CarregaNacionalidade();
            CarregaNecessidadeEspecial();
            CarregaEtnia();
        }

        protected void LimparTela()
        {
            hdnPessoa.Value = string.Empty;
            txtNomeCompl.Text = string.Empty;
            txtNomeSocial.Text = string.Empty;
            dteDtNasc.Text = string.Empty;
            rblSexo.ClearSelection();
            ddlNecessidadeEspecial.ClearSelection();
            ddlPais.ClearSelection();
            ddlRaca.ClearSelection();
            ddlEstadoCivil.ClearSelection();
            ddlNacionalidade.ClearSelection();
            tseNaturalidade.ResetValue();
            txtEstado.Value = string.Empty;
            txtNomeMae.Text = string.Empty;
            txtNomePai.Text = string.Empty;
            chkMaeNaoDeclarada.Checked = false;
            chkPaiNaoDeclarado.Checked = false;

            LimparEndereco();
            LimparEnderecoNascimento();

            txtFone.Text = string.Empty;
            txtCelular.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtEmailInstitucional.Text = string.Empty;
            txtEmailGoogle.Text = string.Empty;

        }


        private void CarregaNecessidadeEspecial()
        {
            RN.NecessidadeEspecial.NecessidadeEspecial rnNecessidadeEspecial = new RN.NecessidadeEspecial.NecessidadeEspecial();
            ListItem itemVazio = new ListItem("Selecione", string.Empty);

            ddlNecessidadeEspecial.Items.Clear();
            ddlNecessidadeEspecial.DataSource = rnNecessidadeEspecial.ListaNecessidadeEspecialAtiva();
            ddlNecessidadeEspecial.DataBind();
            ddlNecessidadeEspecial.Items.Insert(0, itemVazio);
        }

        //Carrega combo "Cor/Raça"
        private void CarregaEtnia()
        {
            RN.Etnia rnEtnia = new Etnia();
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlRaca.Items.Clear();
            ddlRaca.DataSource = rnEtnia.ListaEtniaAtiva();
            ddlRaca.DataBind();
            ddlRaca.Items.Insert(0, item);
        }

        private void CarregaNacionalidade()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlNacionalidade.Items.Clear();
            ddlNacionalidade.DataSource = RN.Basico.ConsultarNacionalidade();
            ddlNacionalidade.DataBind();
            ddlNacionalidade.Items.Insert(0, item);
        }

        private void CarregaPaisNasc()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlPaisNasc.Items.Clear();
            ddlPaisNasc.DataSource = RN.Basico.ConsultarPais();
            ddlPaisNasc.DataBind();
            ddlPaisNasc.Items.Insert(0, item);
        }

        private void CarregaEstadoCivil()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlEstadoCivil.Items.Clear();
            ddlEstadoCivil.DataSource = RN.Basico.ConsultaItemTabelaValDescr("Estado civil");
            ddlEstadoCivil.DataBind();
            ddlEstadoCivil.Items.Insert(0, item);
        }

        private void CarregaPais()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlPais.Items.Clear();
            ddlPais.DataSource = RN.Basico.ConsultarPais();
            ddlPais.DataBind();
            ddlPais.Items.Insert(0, item);
        }

        protected void chkMaeNaoDeclarada_CheckedChanged(object sender, EventArgs e)
        {
            txtNomeMae.ReadOnly = false;
            txtNomeMae.Text = string.Empty;
            if (chkMaeNaoDeclarada.Checked)
            {
                txtNomeMae.Text = chkMaeNaoDeclarada.Text.ToUpper();
                txtNomeMae.ReadOnly = true;
            }
        }

        protected void chkPaiNaoDeclarado_CheckedChanged(object sender, EventArgs e)
        {
            txtNomePai.ReadOnly = false;
            txtNomePai.Text = string.Empty;
            if (chkPaiNaoDeclarado.Checked)
            {
                txtNomePai.Text = chkPaiNaoDeclarado.Text.ToUpper();
                txtNomePai.ReadOnly = true;
            }
        }

        protected void chkNaoSeAplica_CheckedChanged(object sender, EventArgs e)
        {
            ValidaLocalizacaoDiferenciada();
        }

        private void ValidaLocalizacaoDiferenciada()
        {
            if (chkNaoSeAplica.Checked)
            {
                chkQuilombos.Checked = !chkNaoSeAplica.Checked;
                chkAreaAssentamento.Checked = !chkNaoSeAplica.Checked;
                chkTerraIndigena.Checked = !chkNaoSeAplica.Checked;

                chkQuilombos.Enabled = !chkNaoSeAplica.Checked;
                chkAreaAssentamento.Enabled = !chkNaoSeAplica.Checked;
                chkTerraIndigena.Enabled = !chkNaoSeAplica.Checked;
            }
            else
            {
                HabilitaLocalizacaoDiferenciada();
            }
        }

        private void HabilitaLocalizacaoDiferenciada()
        {
            if (!chkNaoSeAplica.Checked)
            {
                Util.Utils.HabilitaDesabilitaControlesWeb(
                    new WebControl[] {
                        chkQuilombos, chkTerraIndigena, chkAreaAssentamento
                    }, true
                );
            }

            chkNaoSeAplica.Enabled = true;
        }

        private void ControlarEnderecoPais()
        {
            if (ddlPais.SelectedItem != null)
            {
                //código 0 = BRASIL
                if (ddlPais.SelectedItem.Text.ToUpper() != "BRASIL")
                {
                    tsCEP.ShowButton = false;
                    txtCEP.MaxLength = 9;

                    txtMunicipio.Visible = true;
                    tseMunicipio.Visible = false;
                    txtEstado.Attributes.Remove("readonly");
                }
                else
                {
                    tsCEP.ShowButton = true;
                    txtCEP.MaxLength = 8;

                    txtMunicipio.Visible = false;
                    tseMunicipio.Visible = true;

                    txtEstado.Attributes.Add("readonly", "readonly");
                }

                if (ddlPaisNasc.SelectedItem != null)
                {
                    if (ddlPaisNasc.SelectedItem.Text.ToUpper() != "BRASIL")
                    {
                        txtMunicipioNaturalidade.Visible = true;

                        tseNaturalidade.Visible = false;
                        txtEstadoNaturalidade.Attributes.Remove("readonly");
                    }
                    else
                    {
                        txtMunicipioNaturalidade.Visible = false;
                        tseNaturalidade.Visible = true;

                        txtEstadoNaturalidade.Attributes.Add("readonly", "readonly");
                    }
                }
            }
        }

        protected void ddlRaca_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlPovoIndigena.Visible = false;
                lblPovo.Visible = false;
                ddlPovoIndigena.ClearSelection();

                if (ddlRaca.SelectedValue == "Índigena")
                {
                    CarregaPovoIndigena();
                    ddlPovoIndigena.Visible = true;
                    lblPovo.Visible = true;
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaPovoIndigena()
        {

            RN.RecursosHumanos.PovoIndigena rnPovoIndigena = new RN.RecursosHumanos.PovoIndigena();
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlPovoIndigena.Items.Clear();
            ddlPovoIndigena.DataSource = rnPovoIndigena.ListaAtivoPor();
            ddlPovoIndigena.DataBind();
            ddlPovoIndigena.Items.Insert(0, item);

        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.VinculoLy rnVinculo = new VinculoLy();
            string zonaResidencial = null;
            string naturalidade = string.Empty;
            string municipioEstrangeiro = string.Empty;
            string mensagem = string.Empty;

            try
            {
                if (!ddlPaisNasc.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (ddlPaisNasc.SelectedItem.Text.ToUpper() == "BRASIL")
                    {
                        naturalidade = (!tseNaturalidade.DBValue.IsNull && tseNaturalidade.IsValidDBValue) ? Convert.ToString(tseNaturalidade.DBValue) : null;
                    }
                    else
                    {
                        // obtém o municipio estrangeiro
                        SimpleRow sr = Endereco.ObterCodigoMunicipioEstrangeiro(txtMunicipioNaturalidade.Text.Trim());

                        //verifica se a função retornou algum valor para a simplerow
                        if (sr != null)
                        {
                            //preenche os dados obtidos de municipio estrangeiro
                            if (!sr["municipio_estrangeiro"].IsNull)
                            {
                                municipioEstrangeiro = Convert.ToString(sr["municipio_estrangeiro"]);
                            }
                        }

                        naturalidade = !municipioEstrangeiro.IsNullOrEmptyOrWhiteSpace() ? municipioEstrangeiro : null;


                        if (naturalidade.IsNullOrEmptyOrWhiteSpace())
                        {
                            lblMensagem.Text = "Naturalidade não encontrada. Favor verificar.";
                            return;
                        }
                    }
                }
               
      
                var pessoa = new LyPessoa
                    {
                        Pessoa = !string.IsNullOrEmpty(hdnPessoa.Value) ? Convert.ToDecimal(hdnPessoa.Value) : 0m,
                        Nome_compl = !txtNomeCompl.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeCompl.Text.Trim().ToUpper() : null,
                        Nome_social = !txtNomeSocial.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeSocial.Text.Trim().ToUpper() : null,
                        Endereco = !txtEndereco.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndereco.Text.Trim().ToUpper() : null,
                        End_num = !txtEndNum.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndNum.Text.Trim().ToUpper() : null,
                        End_municipio = (!tseMunicipio.DBValue.IsNull && tseMunicipio.IsValidDBValue) ? Convert.ToString(tseMunicipio.DBValue) : null,
                        Cep = !txtCEP.Text.IsNullOrEmptyOrWhiteSpace() ? txtCEP.Text.RetirarCaracteres() : null,
                        Pais_nasc = !ddlPaisNasc.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlPaisNasc.SelectedValue : null,
                        Nacionalidade = !ddlNacionalidade.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlNacionalidade.SelectedValue : null,
                        Municipio_nasc = !naturalidade.IsNullOrEmptyOrWhiteSpace() ? naturalidade : null,
                        Etnia = !ddlRaca.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlRaca.SelectedValue : null,
                        End_pais = !ddlPais.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlPais.SelectedValue : null,
                        NecessidadeEspecialId = !ddlNecessidadeEspecial.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlNecessidadeEspecial.SelectedValue) : (int?)null,
                        Est_civil = !ddlEstadoCivil.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlEstadoCivil.SelectedValue : null,
                        Sexo = !string.IsNullOrEmpty(rblSexo.Text)
                                               ? Convert.ToString(rblSexo.Text)
                                               : string.Empty,
                        End_compl = !txtEndCompl.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndCompl.Text.Trim() : null,
                        Bairro = !txtBairro.Text.IsNullOrEmptyOrWhiteSpace() ? txtBairro.Text : null,
                        Fone = !txtFone.Text.IsNullOrEmptyOrWhiteSpace() ? txtFone.Text.Trim() : null,
                        Celular = !txtCelular.Text.IsNullOrEmptyOrWhiteSpace() ? txtCelular.Text.Trim() : null,
                        E_mail_interno = !txtEmailInstitucional.Text.IsNullOrEmptyOrWhiteSpace() ? txtEmailInstitucional.Text.Trim() : null,
                        E_mail = !txtEmail.Text.IsNullOrEmptyOrWhiteSpace() ? txtEmail.Text.Trim() : null,
                        NomeMae = !txtNomeMae.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeMae.Text.Trim().ToUpper() : null,
                        NomePai = !txtNomePai.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomePai.Text.Trim().ToUpper() : null,
                        Dt_nasc = !string.IsNullOrEmpty(dteDtNasc.Text) ? dteDtNasc.Date : (DateTime?)null,
                        AreaAssentamento = chkAreaAssentamento.Checked ? "S" : "N",
                        TerraIndigena = chkTerraIndigena.Checked ? "S" : "N",
                        AreaQuilombos = chkQuilombos.Checked ? "S" : "N",
                        UsuarioId = User.Identity.Name
                    };


                long resultado;

                if (long.TryParse(txtCelular.Text.Trim().RetirarMascaraTelefone(), out resultado))
                {
                    if (txtCelular.Text.Trim().RetirarMascaraTelefone().Length == 10)
                    {
                        pessoa.Celular = string.Format("{0:(00)0000-0000}", resultado);
                    }
                    if (txtCelular.Text.Trim().RetirarMascaraTelefone().Length == 11)
                    {
                        pessoa.Celular = string.Format("{0:(00)00000-0000}", resultado);
                    }
                    txtCelular.Text = pessoa.Celular;
                }
                else
                {
                    pessoa.Celular = null;
                }

                zonaResidencial = !rblLocalizacaoUF.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblLocalizacaoUF.SelectedValue : null;

                validacao = rnVinculo.ValidaDadosPessoaisServidor(pessoa, (chkNaoSeAplica.Checked ? "S" : "N"), zonaResidencial, ddlPovoIndigena.SelectedValue);

                if (validacao.Valido)
                {
                    rnVinculo.AtualizaDadosPessoaisServidor(pessoa, zonaResidencial, ddlPovoIndigena.SelectedValue);
                    mensagem = "Servidor atualizado com sucesso.";

                    var script = @"alert('" + mensagem + @"');";

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


    }
}
