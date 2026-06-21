using System;
using System.Web;
using System.Web.UI;
using Techne.HadesLyc.CR;
using Techne.Web;

namespace Techne.Lyceum.Net.Hades
{
    [NavUrl("~/Hades/AlteracaoSenha.aspx"),
    ControlText("AlteracaoSenha"),
    Title("Alteração de Senha")]
    public partial class AlteracaoSenha : TPage
    {
        #region Propriedades e Enumeradores
        public enum TipoOperacao
        {
            Alterar,
            Confirmar
        }

        private TipoOperacao _tipoOperacao
        {
            get { return (TipoOperacao)ViewState["_tipoOperacao"]; }
            set { ViewState["_tipoOperacao"] = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = Page.Title + " - " + GetPageTitle();
            lblMensagem.Text = string.Empty;

            if (!IsPostBack)
            {
                _tipoOperacao = TipoOperacao.Alterar;
                ControlarTipoOperacao();
            }

        }
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }
        #region Web Form Designer generated code

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
        }
        #endregion

        private void ControlarTipoOperacao()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Alterar:
                    {
                        string usuario = HttpContext.Current.User.Identity.Name.ToString();
                        Hd_usuario.Row dadosUsuario = null;
                        dadosUsuario = Lyceum.RN.Senha.Consultar(usuario);
                        CarregarDados(dadosUsuario);
                        
                        txtSenhaAntiga.Text = string.Empty;
                        txtNovaSenha.Text = string.Empty;
                        txtConfirmaSenha.Text = string.Empty;
                        dtAlteracao.Date = DateTime.Now;
                        
                        break;
                    }

                case TipoOperacao.Confirmar:
                    {
                        lblMensagem.Text = string.Empty;
                        bool valida = ConfereSenhas();

                        if (valida)
                        {
                            RN.RetValue retorno = null;
                            Hd_usuario.Row linha = new Hd_usuario().NewRow();
                            linha.Usuario = txtUsuario.Text;
                            string senha_cripty = RN.RNBase.HdPal(txtNovaSenha.Text);
                            linha.Data_alteracao_senha = DateTime.Now;
                            linha.Senha = senha_cripty;

                            retorno = Lyceum.RN.Senha.AlterarSenha(linha);

                            if (retorno != null)
                            {
                                if (!retorno.Ok)
                                {
                                    lblMensagem.Text = retorno.Errors.ToString();
                                    _tipoOperacao = TipoOperacao.Alterar;
                                    ControlarTipoOperacao();
                                }
                                else
                                {
                                    lblMensagem.Text = retorno.Message;
                                    _tipoOperacao = TipoOperacao.Alterar;
                                    ControlarTipoOperacao();
                                }
                            }
                        }
                        else
                        {
                            _tipoOperacao = TipoOperacao.Alterar;
                            ControlarTipoOperacao();
                        }

                        break;
                    }
            }
        }

        #region Eventos Botões
        protected void btConfirmar_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Confirmar;
            ControlarTipoOperacao();
        }
        #endregion



        private void CarregarDados(Hd_usuario.Row dadosUsuario)
        {
            txtNome.Text = dadosUsuario.Nome;
            txtUsuario.Text = dadosUsuario.Usuario;
            lblPrivilediado.Text = dadosUsuario.Privilegiado;
            lblHabilitado.Text = dadosUsuario.Habilitado;
        }


        private bool ConfereSenhas()
        {
            string usuario = HttpContext.Current.User.Identity.Name.ToString();
            Hd_usuario.Row dadosUsuario = null;
            dadosUsuario = Lyceum.RN.Senha.Consultar(usuario);
            
            string senha = dadosUsuario.Senha;

            string senha_cripty = RN.RNBase.HdPal(txtSenhaAntiga.Text);

            if (senha != senha_cripty)
            {
                lblMensagem.Text = "A senha antiga está incorreta. Favor informá-la novamente.";
                txtSenhaAntiga.Text = string.Empty;
                return false;
            }

            if (txtNovaSenha.Text != txtConfirmaSenha.Text)
            {
                lblMensagem.Text = "Nova senha e confirmação de nova senha estão diferentes. Favor informá-las novamente.";
                txtNovaSenha.Text = txtConfirmaSenha.Text = string.Empty;
                return false;
            }

            if (txtNovaSenha.Text == txtSenhaAntiga.Text)
            {
                lblMensagem.Text = "Nova senha não pode ser igual a senha antiga. Favor escolher outra senha.";
                txtNovaSenha.Text = txtConfirmaSenha.Text = string.Empty;
                return false;
            }

            return true;

        }


    }//fim da classe
}//fim do namespace