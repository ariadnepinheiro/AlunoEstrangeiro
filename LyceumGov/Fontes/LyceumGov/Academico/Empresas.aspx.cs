using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Data;
using Techne.Web;

namespace Techne.Lyceum.Net.Academico
{
    [
        NavUrl("~/Academico/Empresas.aspx"),
            ControlText("Empresas"),
            Title("Empresas"),
    ]

    public partial class Empresas : TPage
    {

        public static string GetUrl()
        {
            #region Código gerado Techne
            return
                Techne.Web.Navigation.GetNavigation(
                    System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        #region Web Form Designer gnerated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
        }
        #endregion

        #region :: Enumerador e ViewState do Tipo de Operação ::
        public enum TipoOperacao
        { 
            Iniciar,
            Consultar,
            Novo,
            Alterar,
            Excluir,
            Cancelar,
            Sucesso
        }

        private TipoOperacao _tipoOperacao
        {
            get { return (TipoOperacao)ViewState["_tipoOperacao"]; }
            set { ViewState["_tipoOperacao"] = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = String.Empty;

            if (!IsPostBack)
            {
                _tipoOperacao = TipoOperacao.Iniciar;
                ControlarTipoOperacao();
            }
        }

        protected void tseEmpresa_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            if (tseEmpresa.IsValidDBValue == true && !tseEmpresa.DBValue.IsNull)
            {
                LimparTela();

                ImageButton[] botoes = new ImageButton[] { btnNovo, btnAlterar, btnExcluir };
                VisibilidadeBotoes(botoes); 

                _tipoOperacao = TipoOperacao.Consultar;
                ControlarTipoOperacao();
            }
            else
            {
                if (tseEmpresa.DBValue.IsNull)
                {
                    lblMensagem.Text = "Empresa não cadastrada.";
                    _tipoOperacao = TipoOperacao.Iniciar;
                    ControlarTipoOperacao();
                }
                else
                {
                    lblMensagem.Text = "Favor consultar uma empresa.";
                }
            }
        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            if (!tseMunicipio.DBValue.IsNull && tseMunicipio.IsValidDBValue)
            {
                txtEstado.Text = Convert.ToString(tseMunicipio["uf_sigla"]);
            }
        }

        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Novo;
            ControlarTipoOperacao();
        }

        protected void btnAlterar_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Alterar;
            ControlarTipoOperacao();
        }

        protected void btnExcluir_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Excluir;
            ControlarTipoOperacao();
        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            Techne.Lyceum.RN.RetValue retorno = null;

            Dictionary<String, String> dadosEmpresa = new Dictionary<String, String>();
            ObterDados(dadosEmpresa);

            if (dadosEmpresa["CNPJ"] != null && dadosEmpresa["CNPJ"] != String.Empty)
            {
                bool CNPJValido = Techne.Lyceum.RN.Validacao.ValidaCnpj(dadosEmpresa["CNPJ"]);
                if (CNPJValido != true)
                {
                    lblMensagem.Text = "CNPJ inválido!";
                    return;
                }
            }
            else
            {
                lblMensagem.Text = "CNPJ: Preenchimento obrigatório!";
                return;
            }

            //if (!string.IsNullOrEmpty(txtInscricaoMunicipal.Text))
            //{
            //    decimal i = 0;
            //    if (!decimal.TryParse(txtInscricaoMunicipal.Text, out i))
            //    {
            //        lblMensagem.Text = "Só é possível inserir número no campo Inscrição Municipal.";
            //        return;
            //    }
            //}

            //if (!string.IsNullOrEmpty(txtInscricaoEstadual.Text))
            //{
            //    decimal i = 0;
            //    if (!decimal.TryParse(txtInscricaoEstadual.Text, out i))
            //    {
            //        lblMensagem.Text = "Só é possível inserir número no campo Inscrição Estadual.";
            //        return;
            //    }
            //}

            if (_tipoOperacao == TipoOperacao.Novo)
            {
                retorno = Techne.Lyceum.RN.Empresas.Inserir(dadosEmpresa); ;
            }

            if (_tipoOperacao == TipoOperacao.Alterar)
            {
                retorno = Techne.Lyceum.RN.Empresas.Alterar(dadosEmpresa);
            }

            if (retorno != null)
            {
                if (!retorno.Ok)
                {
                    lblMensagem.Text = retorno.Errors.ToString();
                    lblMensagem.Text = lblMensagem.Text + " " + retorno.Message.ToString();
                }
                else
                {
                    lblMensagem.Text = retorno.Message;
                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();
                }
            }
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Cancelar;
            ControlarTipoOperacao();
        }

        #region :: Controlar as Operações ::
        private void ControlarTipoOperacao()
        {
            switch (_tipoOperacao)
            { 
                case TipoOperacao.Iniciar:
                {
                    ImageButton[] botoes = new ImageButton[] { btnNovo };
                    VisibilidadeBotoes(botoes); 
                    
                    tseEmpresa.ResetValue();

                    DesabilitarCampos();
                    pnlEmpresa.Visible = false;

                    break;
                }

                case TipoOperacao.Consultar:
                {                    
                    lblMensagem.Text = String.Empty;

                    PreencherDados();

                    DesabilitarCampos();
                    pnlEmpresa.Visible = true;

                    break;
                }

                case TipoOperacao.Novo:
                {
                    LimparTela();
                    lblMensagem.Text = String.Empty;

                    ImageButton[] botoes = new ImageButton[] { btnSalvar, btnCancelar };
                    VisibilidadeBotoes(botoes);

                    HabilitarCampos();
                    pnlEmpresa.Visible = true;

                    break;
                }

                case TipoOperacao.Alterar:
                {
                    ImageButton[] botoes = new ImageButton[] { btnSalvar, btnCancelar };
                    VisibilidadeBotoes(botoes);

                    HabilitarCampos();
                    pnlEmpresa.Visible = true;
                    txtEmpresa.ReadOnly = true;

                    break;
                }

                case TipoOperacao.Excluir:
                {
                    Techne.Lyceum.RN.RetValue retorno = null;

                    retorno = Techne.Lyceum.RN.Empresas.Excluir(Convert.ToString(txtEmpresa.Text));

                    if (retorno != null)
                    {
                        if (!retorno.Ok)
                        {
                            lblMensagem.Text = "Existe Relação de Empresa/Unidade Física cadastrada para esta Empresa.";
                        }
                        else
                        {
                            lblMensagem.Text = retorno.Message;
                            _tipoOperacao = TipoOperacao.Iniciar;
                            ControlarTipoOperacao();
                        }
                    }
                    
                    break;
                }

                case TipoOperacao.Cancelar:
                {
                    LimparTela();

                    if (!tseEmpresa.DBValue.IsNull)
                    {
                        ImageButton[] botoes = new ImageButton[] { btnNovo, btnAlterar, btnExcluir };
                        VisibilidadeBotoes(botoes);

                        _tipoOperacao = TipoOperacao.Consultar;
                        ControlarTipoOperacao();
                    }
                    else
                    {
                        _tipoOperacao = TipoOperacao.Iniciar;
                        ControlarTipoOperacao();
                    }

                    break;
                }

                case TipoOperacao.Sucesso:
                {
                    ImageButton[] botoes = new ImageButton[] {btnNovo, btnAlterar, btnExcluir };
                    VisibilidadeBotoes(botoes);

                    tseEmpresa.Value = txtEmpresa.Text;

                    break;
                }
            }
        }
        #endregion

        #region :: Controles de Visibilidade dos Botões ::
        private void VisibilidadeBotoes(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotoes();

            foreach (ImageButton botao in botoes)
            {
                botao.Visible = true;
            }

            ControlaAcessoBotoes();
        }

        private void RetiraVisibilidadeBotoes()
        {
            btnNovo.Visible = false;
            btnAlterar.Visible = false;
            btnExcluir.Visible = false;
            btnSalvar.Visible = false;
            btnCancelar.Visible = false;
        }

        private void ControlaAcessoBotoes()
        {
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(btnAlterar, AcaoControle.editar);
            ControlaAcesso(btnExcluir, AcaoControle.excluir);
        }        
        #endregion

        #region :: Habilitar, Desabilitar e Limpar Tela ::
        private void LimparTela()
        {
            txtEmpresa.Text = String.Empty;
            txtRazaoSocial.Text = String.Empty;
            txtNome.Text = String.Empty;
            txtCEP.Text = String.Empty;
            txtEstado.Text = String.Empty;
            txtEndereco.Text = String.Empty;
            txtNumero.Text = String.Empty;
            txtComplemento.Text = String.Empty;
            txtBairro.Text = String.Empty;
            txtCNPJ.Text = String.Empty;
            txtInscricaoMunicipal.Text = String.Empty;
            txtInscricaoEstadual.Text = String.Empty;
            txtPorte.Text = String.Empty;
            txtRamo.Text = String.Empty;
            txtAtividade.Text = String.Empty;
            txtNumeroEmpregados.Text = String.Empty;
            txtTipoCapital.Text = String.Empty;

            tsCEP.ResetValue();
            tseMunicipio.ResetValue();
        }

        private void HabilitarCampos()
        {
            txtEmpresa.ReadOnly = false;
            txtRazaoSocial.ReadOnly = false;
            txtNome.ReadOnly = false;
            txtCEP.ReadOnly = false;
            //txtEstado.ReadOnly = true;
            txtEndereco.ReadOnly = false;
            txtNumero.ReadOnly = false;
            txtComplemento.ReadOnly = false;
            txtBairro.ReadOnly = false;
            txtCNPJ.ReadOnly = false;
            txtInscricaoMunicipal.ReadOnly = false;
            txtInscricaoEstadual.ReadOnly = false;
            txtPorte.ReadOnly = false;
            txtRamo.ReadOnly = false;
            txtAtividade.ReadOnly = false;
            txtNumeroEmpregados.ReadOnly = false;
            txtTipoCapital.ReadOnly = false;

            tsCEP.ReadOnly = false;
            tseMunicipio.ReadOnly = false;
        }

        private void DesabilitarCampos()
        {
            txtEmpresa.ReadOnly = true;
            txtRazaoSocial.ReadOnly = true;
            txtNome.ReadOnly = true;
            txtCEP.ReadOnly = true;
            txtEstado.ReadOnly = true;
            txtEndereco.ReadOnly = true;
            txtNumero.ReadOnly = true;
            txtComplemento.ReadOnly = true;
            txtBairro.ReadOnly = true;
            txtCNPJ.ReadOnly = true;
            txtInscricaoMunicipal.ReadOnly = true;
            txtInscricaoEstadual.ReadOnly = true;
            txtPorte.ReadOnly = true;
            txtRamo.ReadOnly = true;
            txtAtividade.ReadOnly = true;
            txtNumeroEmpregados.ReadOnly = true;
            txtTipoCapital.ReadOnly = true;

            tsCEP.ReadOnly = true;
            tseMunicipio.ReadOnly = true;
        }
        #endregion

        #region :: Obter e Preencher Dados ::
        private void PreencherDados()
        {
            if (tseEmpresa != null && tseEmpresa.Value != null)
            {
                QueryTable qtEmpresas = RN.Empresas.Consultar(Convert.ToString(tseEmpresa.Value));

                if (qtEmpresas.Rows.Count > 0)
                {
                    txtEmpresa.Text = Convert.ToString(qtEmpresas.Rows[0]["empresa"]);
                    txtRazaoSocial.Text = Convert.ToString(qtEmpresas.Rows[0]["razao_social"]);
                    txtNome.Text = Convert.ToString(qtEmpresas.Rows[0]["nome"]);
                    txtCEP.Text = Convert.ToString(qtEmpresas.Rows[0]["cep"]);
                    txtEndereco.Text = Convert.ToString(qtEmpresas.Rows[0]["endereco"]);
                    txtNumero.Text = Convert.ToString(qtEmpresas.Rows[0]["end_num"]);
                    txtComplemento.Text = Convert.ToString(qtEmpresas.Rows[0]["end_compl"]);
                    txtBairro.Text = Convert.ToString(qtEmpresas.Rows[0]["bairro"]);
                    txtCNPJ.Text = Convert.ToString(qtEmpresas.Rows[0]["cnpj"]);
                    txtInscricaoMunicipal.Text = Convert.ToString(qtEmpresas.Rows[0]["inscr_municipal"]);
                    txtInscricaoEstadual.Text = Convert.ToString(qtEmpresas.Rows[0]["inscr_estadual"]);
                    txtPorte.Text = Convert.ToString(qtEmpresas.Rows[0]["porte"]);
                    txtRamo.Text = Convert.ToString(qtEmpresas.Rows[0]["ramo"]);
                    txtAtividade.Text = Convert.ToString(qtEmpresas.Rows[0]["atividade"]);
                    txtNumeroEmpregados.Text = Convert.ToString(qtEmpresas.Rows[0]["num_empregados"]);
                    txtTipoCapital.Text = Convert.ToString(qtEmpresas.Rows[0]["tipo_capital"]);

                    tseMunicipio.DBValue = Convert.ToString(qtEmpresas.Rows[0]["municipio"]);                    
                    if (!tseMunicipio.DBValue.IsNull && tseMunicipio.IsValidDBValue)
                    {
                        txtEstado.Text = Convert.ToString(qtEmpresas.Rows[0]["uf_sigla"]);
                    }
                }
            }
        }

        private void ObterDados(Dictionary<String, String> dadosEmpresa)
        {
            dadosEmpresa.Add("Empresa", Convert.ToString(txtEmpresa.Text));
            dadosEmpresa.Add("RazaoSocial", Convert.ToString(txtRazaoSocial.Text));
            dadosEmpresa.Add("Nome", Convert.ToString(txtNome.Text));
            dadosEmpresa.Add("CEP", Convert.ToString(txtCEP.Text));
            dadosEmpresa.Add("Municipio", Convert.ToString(tseMunicipio.DBValue));
            dadosEmpresa.Add("Estado", Convert.ToString(txtEstado.Text));
            dadosEmpresa.Add("Endereco", Convert.ToString(txtEndereco.Text));
            dadosEmpresa.Add("Numero", Convert.ToString(txtNumero.Text));
            dadosEmpresa.Add("Complemento", Convert.ToString(txtComplemento.Text));
            dadosEmpresa.Add("Bairro", Convert.ToString(txtBairro.Text));
            dadosEmpresa.Add("CNPJ", Convert.ToString(txtCNPJ.Text));
            dadosEmpresa.Add("InscricaoMunicipal", Convert.ToString(txtInscricaoMunicipal.Text));
            dadosEmpresa.Add("InscricaoEstadual", Convert.ToString(txtInscricaoEstadual.Text));
            dadosEmpresa.Add("Porte", Convert.ToString(txtPorte.Text));
            dadosEmpresa.Add("Ramo", Convert.ToString(txtRamo.Text));
            dadosEmpresa.Add("Atividade", Convert.ToString(txtAtividade.Text));
            dadosEmpresa.Add("NumeroEmpregados", Convert.ToString(txtNumeroEmpregados.Text));
            dadosEmpresa.Add("TipoCapital", Convert.ToString(txtTipoCapital.Text));
        }
        #endregion
    }
}
