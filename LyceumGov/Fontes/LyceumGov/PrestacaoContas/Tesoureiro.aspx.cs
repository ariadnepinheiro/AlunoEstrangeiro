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

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [
          NavUrl("~/PrestacaoContas/Tesoureiro.aspx"),
          ControlText("Tesoureiro"),
          Title("Tesoureiro")
      ]
    public partial class Tesoureiro : TPage
    {
        public enum TipoOperacao
        {
            Novo,
            Excluir,
            Alterar,
            Inicial,
            Sucesso,
            BuscaTesoureiro,
            Consultar
        }

        private TipoOperacao _tipoOperacao
        {
            get { return (TipoOperacao)ViewState["_tipoOperacao"]; }
            set { ViewState["_tipoOperacao"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    _tipoOperacao = TipoOperacao.Inicial;
                    ControlarTipoOperacao();
                }
               
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }     
       

        protected void LimparTela()
        {
            hdnTesoureiroId.Value = string.Empty;
            tseMunicipio.ResetValue();
            txtNomeComplPessoa.Text = string.Empty;
            txtEnderecoPessoa.Text = string.Empty;
            txtEndNumPessoa.Text = string.Empty;
            txtEndCompl.Text = string.Empty;
            txtRG.Text = string.Empty;
            txtMunicipio.Text = string.Empty;
            txtEstado.Value = string.Empty;
            txtBairro.Text = string.Empty;
            txtCEP.Text = string.Empty;
            txtTelefone.Text = string.Empty;            
            txtEmail.Text = string.Empty;
        }


        protected void BloquearDesbloquearEdicao(bool ativado)
        {
            try
            {
                txtNomeComplPessoa.Enabled = ativado;
                txtCEP.Enabled = ativado;
                tsCEP.Enabled = ativado;
                tseMunicipio.Enabled = ativado;
                txtEnderecoPessoa.Enabled = ativado;
                txtEndNumPessoa.Enabled = ativado;
                txtEndCompl.Enabled = ativado;
                txtBairro.Enabled = ativado; ;
                txtTelefone.Enabled = ativado;
                txtEmail.Enabled = ativado;
                txtRG.Enabled = ativado;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PreencheDadosTesoureiro(RN.PrestacaoContas.Entidades.Tesoureiro dadosTesoureiro)
        {
            List<int> listaTipoTesoureiro = new List<int>();

            txtCPF.Text = dadosTesoureiro.Cpf.AplicarMascaraCPF();
            txtRG.Text = dadosTesoureiro.Rg.Trim();
            hdnTesoureiroId.Value = dadosTesoureiro.TesoureiroId.ToString();
            txtNomeComplPessoa.Text = dadosTesoureiro.Nome;  
            txtCEP.Text = dadosTesoureiro.Cep;
            tseMunicipio.DBValue = dadosTesoureiro.MunicipioId;
            tseMunicipio_Changed(null, null);
            txtEnderecoPessoa.Text = dadosTesoureiro.Endereco;
            txtEndNumPessoa.Text = dadosTesoureiro.Numero;
            txtEndCompl.Text = dadosTesoureiro.Complemento;
            txtBairro.Text = dadosTesoureiro.Bairro;
           
            long resultadoFixoCelular;
            var fixoCelular = dadosTesoureiro.Telefone.RetirarMascaraTelefone();

            if (long.TryParse(fixoCelular, out resultadoFixoCelular))
            {
                if (fixoCelular.Length == 10)
                {
                    txtTelefone.Text = string.Format("{0:(00)0000-0000}", resultadoFixoCelular);
                }
                else if (fixoCelular.Length == 11)
                {
                    txtTelefone.Text = string.Format("{0:(00)00000-0000}", resultadoFixoCelular);
                }
                else
                {
                    txtTelefone.Text = resultadoFixoCelular.ToString();
                }
            }


            txtEmail.Text = dadosTesoureiro.Email;
           
        }

        protected void tseTesoureiro_Changed(object sender, EventArgs args)
        {
            if (this.Page.IsCallback)
            {
                return;
            }

            try
            {
                RN.PrestacaoContas.Entidades.Tesoureiro dadosTesoureiro = new Techne.Lyceum.RN.PrestacaoContas.Entidades.Tesoureiro();
                RN.PrestacaoContas.Tesoureiro rnTesoureiro = new Techne.Lyceum.RN.PrestacaoContas.Tesoureiro();

                LimparTela();
                if (!this.tseTesoureiro.DBValue.IsNull)
                {
                    if (this.tseTesoureiro.IsValidDBValue)
                    {
                        dadosTesoureiro = rnTesoureiro.ObtemTesoureiroPor(tseTesoureiro.DBValue.ToString());

                        if (dadosTesoureiro.TesoureiroId > 0)
                        {
                            PreencheDadosTesoureiro(dadosTesoureiro);

                            _tipoOperacao = TipoOperacao.Consultar;
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Tesoureiro não cadastrado.";
                        _tipoOperacao = TipoOperacao.Inicial;
                    }
                }
                else
                {
                    lblMensagem.Text = "Tesoureiro não cadastrado.";
                    _tipoOperacao = TipoOperacao.Inicial;
                }
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }       

        protected void tseMunicipio_Changed(object sender, EventArgs args)
        {
            if (tseMunicipio.IsValidDBValue
                && !tseMunicipio.DBValue.IsNull)
            {
                txtEstado.Value = Convert.ToString(tseMunicipio["uf_sigla"]);
            }
        }

        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Novo;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.PrestacaoContas.Tesoureiro rnTesoureiro = new Techne.Lyceum.RN.PrestacaoContas.Tesoureiro();
                RN.PrestacaoContas.Entidades.Tesoureiro dadosTesoureiro = new Techne.Lyceum.RN.PrestacaoContas.Entidades.Tesoureiro();
                List<int> tipoTesoureiro = new List<int>();
                bool cadastro = false;

                dadosTesoureiro.Cpf = !txtCPF.Text.IsNullOrEmptyOrWhiteSpace() ? this.txtCPF.Text.RetirarMascaraCPF() : null;
                dadosTesoureiro.Rg = !txtRG.Text.IsNullOrEmptyOrWhiteSpace() ? this.txtRG.Text.RetirarMascaraCPF() : null;
                dadosTesoureiro.Nome = !txtNomeComplPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeComplPessoa.Text.Trim() : null;
                dadosTesoureiro.Cep = !txtCEP.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToString(txtCEP.Text.Replace("-", String.Empty).Replace(".", String.Empty)) : null;
                dadosTesoureiro.Endereco = !txtEnderecoPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? txtEnderecoPessoa.Text.Trim() : null;
                dadosTesoureiro.Numero = !txtEndNumPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? this.txtEndNumPessoa.Text : null;
                dadosTesoureiro.Bairro = !txtBairro.Text.IsNullOrEmptyOrWhiteSpace() ? txtBairro.Text.Trim() : null;
                dadosTesoureiro.Complemento = !txtEndCompl.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndCompl.Text.Trim() : null;
                dadosTesoureiro.Email = !txtEmail.Text.IsNullOrEmptyOrWhiteSpace() ? txtEmail.Text.Trim() : null;
                dadosTesoureiro.Telefone = !txtTelefone.Text.IsNullOrEmptyOrWhiteSpace() ? txtTelefone.Text.Trim().RetirarMascaraTelefone() : null;                
                dadosTesoureiro.TesoureiroId = !hdnTesoureiroId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnTesoureiroId.Value) : 0;
                dadosTesoureiro.UsuarioId = User.Identity.Name;
                dadosTesoureiro.MunicipioId = !tseMunicipio.DBValue.IsNull ? tseMunicipio.DBValue.ToString() : null;

                cadastro = dadosTesoureiro.TesoureiroId == 0;
                validacao = rnTesoureiro.Valida(dadosTesoureiro, cadastro);

                if (validacao.Valido)
                {
                    if (dadosTesoureiro.TesoureiroId == 0)
                    {
                        rnTesoureiro.Insere(dadosTesoureiro);
                        tseTesoureiro.ResetValue();
                        tseTesoureiro.DBValue = dadosTesoureiro.Cpf;
                        hdnTesoureiroId.Value = dadosTesoureiro.TesoureiroId.ToString();
                    }
                    else
                    {
                        rnTesoureiro.Atualiza(dadosTesoureiro);
                    }

                    lblMensagem.Text = "Tesoureiro " + (cadastro ? "incluído" : "alterado") + " com sucesso.";
                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();
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

        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Alterar;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                tseTesoureiro.ResetValue();
                _tipoOperacao = TipoOperacao.Inicial;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnExcluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.PrestacaoContas.Tesoureiro rnTesoureiro = new Techne.Lyceum.RN.PrestacaoContas.Tesoureiro();

                if (!hdnTesoureiroId.Value.IsNullOrEmptyOrWhiteSpace())
                {
                    validacao = rnTesoureiro.ValidaRemocao(Convert.ToInt32(hdnTesoureiroId.Value));

                    if (validacao.Valido)
                    {
                        rnTesoureiro.Remove(Convert.ToInt32(hdnTesoureiroId.Value));
                        _tipoOperacao = TipoOperacao.Excluir;
                        ControlarTipoOperacao();
                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        return;
                    }
                }
                else
                {
                    lblMensagem.Text = "Tesoureiro não encontrado.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ControlarTipoOperacao()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        divDados.Visible = false;
                        tseTesoureiro.Enabled = true;
                        LimparTela();
                        break;
                    }

                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, btnExcluir };
                        ControlarVisibilidadeControle(controles);
                        tseTesoureiro.Enabled = true;
                        DesabilitaCampos();
                        break;
                    }

                case TipoOperacao.Novo:
                    {
                        LimparTela();
                        txtCPF.Text = string.Empty;                
                        tseTesoureiro.ResetValue();
                        tseTesoureiro.Enabled = false;
                        divDados.Visible = true;
                        var controles = new[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);
                        HabilitaCampos();

                        break;
                    }
                case TipoOperacao.Excluir:
                    {
                        
                        btnExcluir.Visible = false;
                        lblMensagem.Text = "Tesoureiro removido com sucesso.";

                        _tipoOperacao = TipoOperacao.Inicial;
                        ControlarTipoOperacao();

                        break;
                    }
                case TipoOperacao.BuscaTesoureiro:
                    {
                        LimparTela();                      
                        HabilitaCampos();

                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, btnExcluir }; 
                        ControlarVisibilidadeControle(controles);
                        DesabilitaCampos();
                        divDados.Visible = true;
                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        var controles = new[] { btnCancel, btnSalvar };
                        tseTesoureiro.Enabled = false; 
                        ControlarVisibilidadeControle(controles);
                        HabilitaCampos();

                        break;
                    }
            }
        }
      
        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (ImageButton botao in botoes)
            {
                botao.Visible = true;
            }
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnExcluir, AcaoControle.excluir);
            ControlaAcesso(btnNovo, AcaoControle.novo);
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnEditar.Visible = false;
            btnExcluir.Visible = false;
            btnNovo.Visible = false;
            btnSalvar.Visible = false;
        }

        protected void DesabilitaCampos()
        {
            txtMunicipio.ReadOnly = true;
            tseMunicipio.Enabled = false;
            txtEstado.Attributes.Add("readonly", "readonly");
            txtNomeComplPessoa.Enabled = false;
            txtTelefone.Enabled = false;            
            txtEmail.Enabled = false; ;
            txtCPF.Enabled = false;
            txtRG.Enabled = false;
            txtCEP.Enabled = false;
            tsCEP.ShowButton = false;

            txtEndCompl.Enabled = false;
            txtEndNumPessoa.Enabled = false;
            txtEnderecoPessoa.Enabled = false;
            txtBairro.Enabled = false;
        }

        protected void HabilitaCampos()
        {
            txtMunicipio.ReadOnly = false;
            tseMunicipio.Enabled = true;
            txtEstado.Attributes.Add("readonly", "readonly");
            txtNomeComplPessoa.Enabled = true;

            txtTelefone.Enabled = true;            
            txtEmail.Enabled = true;
            txtCPF.Enabled = true;
            txtRG.Enabled = true;
            txtCEP.Enabled = true;
            tsCEP.ShowButton = true;

            txtEndCompl.Enabled = true;
            txtEndNumPessoa.Enabled = true;
            txtEnderecoPessoa.Enabled = true;
            txtBairro.Enabled = true;
        }
    }
}
