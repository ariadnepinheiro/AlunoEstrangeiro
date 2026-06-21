namespace Techne.Lyceum.Net.Seguranca
{
    using System;
    using System.Reflection;
    using System.Web.UI;
    using Techne.Lyceum.RN;
    using Techne.Web;
    using System.Web;
    using Techne.Lyceum.RN.Util;
    using System.Collections.Generic;
    using System.Linq;
using System.Text.RegularExpressions;
    using System.Text;

    [
    NavUrl("~/Seguranca/Identificacao.aspx"),
    Title("Login")
    ]
    public partial class Identificacao : TPage
    {
        protected System.Web.UI.WebControls.Label PageUser;
        protected System.Web.UI.WebControls.Label PageDate;

        public static string GetUrl()
        {
            #region Código gerado Techne
            return Navigation.GetNavigation(MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblErroSenha.Text = string.Empty;
            lblErroSenhaAlteracao.Text = string.Empty;
            cMsg.Text = string.Empty;
        }

        protected void bt_Entrar_Click(object sender, ImageClickEventArgs e)
        {
            if (VerificaPreenchimeto())
            {
                Autentica(cUsuario.Text, cSenha.Text);
            }
            else
            {
                this.txtChave.Text = string.Empty;
            }
        }

        public bool VerificaPreenchimeto()
        {
            bool retorno = true;

            if (string.IsNullOrEmpty(cUsuario.Text))
            {
                this.lblErroSenha.Text = "O campo USUÁRIO é de preenchimento obrigatório.</br>";
                retorno = false;
            }

            if (string.IsNullOrEmpty(cSenha.Text))
            {
                this.lblErroSenha.Text = lblErroSenha.Text + "O campo SENHA é de preenchimento obrigatório.</br>";
                retorno = false;
            }

            if (string.IsNullOrEmpty(txtChave.Text))
            {
                this.lblErroSenha.Text = lblErroSenha.Text + "O CÓDIGO DA IMAGEM é de preenchimento obrigatório.</br>";
                retorno = false;
            }
            else
            {
                string captchaGerado = string.Empty;
                if (HttpContext.Current.Response.Cookies["CaptchaValue"] != null)
                {
                    captchaGerado = HttpContext.Current.Request.Cookies["CaptchaValue"].Value;
                }

                // Valida Captcha
                if (this.txtChave.Text != captchaGerado)
                {
                    this.lblErroSenha.Text = lblErroSenha.Text + "Código digitado incorreto. Digite-o novamente.";
                    retorno = false;
                }
            }

            return retorno;
        }

        public bool VerificaPreenchimetoRedefinicaoSenha()
        {
            bool retorno = true;

            if (string.IsNullOrEmpty(cUsuarioRS.Text))
            {
                this.lblErroSenhaRS.Text = "O campo USUÁRIO é de preenchimento obrigatório.</br>";
                retorno = false;
            }

            if (string.IsNullOrEmpty(txtChaveRS.Text))
            {
                this.lblErroSenhaRS.Text = lblErroSenhaRS.Text + "O CÓDIGO DA IMAGEM é de preenchimento obrigatório.</br>";
                retorno = false;
            }
            else
            {
                string captchaGerado = string.Empty;
                if (HttpContext.Current.Response.Cookies["CaptchaValue"] != null)
                {
                    captchaGerado = HttpContext.Current.Request.Cookies["CaptchaValue"].Value;
                }

                // Valida Captcha
                if (this.txtChaveRS.Text != captchaGerado)
                {
                    this.lblErroSenhaRS.Text = lblErroSenhaRS.Text + "Código digitado incorreto. Digite-o novamente.";
                    retorno = false;
                }
            }

            return retorno;
        }

        public bool VerificaPreenchimetoPedidoUsuario()
        {
            bool retorno = true;

            if (string.IsNullOrEmpty(cUsuarioCPF.Text))
            {
                this.lblErroSenhaUSU.Text = "O campo CPF é de preenchimento obrigatório.</br>";
                retorno = false;
            }

            if (string.IsNullOrEmpty(txtChaveUSU.Text))
            {
                this.lblErroSenhaUSU.Text = lblErroSenhaUSU.Text + "O CÓDIGO DA IMAGEM é de preenchimento obrigatório.</br>";
                retorno = false;
            }
            else
            {
                string captchaGerado = string.Empty;
                if (HttpContext.Current.Response.Cookies["CaptchaValue"] != null)
                {
                    captchaGerado = HttpContext.Current.Request.Cookies["CaptchaValue"].Value;
                }

                // Valida Captcha
                if (this.txtChaveUSU.Text != captchaGerado)
                {
                    this.lblErroSenhaUSU.Text = lblErroSenhaUSU.Text + "Código digitado incorreto. Digite-o novamente.";
                    retorno = false;
                }
            }

            return retorno;
        }

        private void SetPanelLoginVisibility(bool visible)
        {
            PanelLogin.Visible = visible;
            PanelPassword.Visible = !visible;
            PanelPedidoRedefinicaoSenha.Visible = false;
            PanelPedidoUsuario.Visible = false;
        }

        private void Autentica(string usuario, string senha)
        {
            var msg = string.Empty;
            var result = TechneAuthentication.Authenticate(usuario, senha, out msg);

            if (result == TechneAuthenticationResult.OK)
            {
                AcessoUsuario.AtualizaUltimoAcesso("GESTAO", usuario);

                var pagina_inicial = RN.Identificacao.ConsultarPaginaInicial(usuario);

                // verifica se usuário é diretor e redireciona para correção de matrículas duplicadas
                //var matricula = Servidores.ObterMatriculaUsuario(usuario);
                //var funcaoUsuario = PadraoAcessoFuncao.ConsultaFuncao(matricula);
                var email = Usuarios.BuscaEmail(usuario);

                Session["email"] = email;

                Session["unidadeDuplicados"] = null;

                if (!string.IsNullOrEmpty(pagina_inicial))
                {
                    Response.Redirect(pagina_inicial);
                }
                else
                {
                    Response.Redirect("../Default.aspx");
                }
            }
            else if (result == TechneAuthenticationResult.ChangePassword)
            {
                cMsg.Text = string.Empty;
                ViewState.Add("USUARIO.ID", usuario);
                lblUsuarioSenha.Text = usuario;
                SetPanelLoginVisibility(false);
                lblMsgAlteracao.Text = msg + "<br>Digite uma nova senha";
            }
            else
            {
                cSenha.Focus();
                this.txtChave.Text = string.Empty;
                cMsg.Text = msg;
            }
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
            this.DisabledNavigationKeys = NavigationKey.Backspace;
        }

        private void InitializeComponent()
        {
            cUsuario.Focus();
        }
        #endregion

        protected bool ValidaNovaSenha()
        {
            if (string.IsNullOrEmpty(cUsuario.Text))
            {
                lblErroSenhaAlteracao.Text = "O usuario é de preenchimento obrigatorio.";
                return false;
            }

            if (string.IsNullOrEmpty(txtSenhaAtual.Text))
            {
                lblErroSenhaAlteracao.Text = "A senha atual é de preenchimento obrigatorio.";
                return false;
            }

            if (string.IsNullOrEmpty(txtSenhaNova1.Text))
            {
                lblErroSenhaAlteracao.Text = "A senha nova é de preenchimento obrigatorio.";
                return false;
            }

            if (txtSenhaNova1.Text.Length < 6)
            {
                lblErroSenhaAlteracao.Text = "Favor inserir uma senha de, no mínimo, 6 dígitos.";
                return false;
            }

            if (txtSenhaNova1.Text == txtSenhaAtual.Text)
            {
                lblErroSenhaAlteracao.Text = "A senha nova deve ser diferente da senha atual.";
                return false;
            }

            if (string.IsNullOrEmpty(txtSenhaNova2.Text))
            {
                lblErroSenhaAlteracao.Text = "A confirmação da senha nova é de preenchimento obrigatorio.";
                return false;
            }

            if (txtSenhaNova1.Text != txtSenhaNova2.Text)
            {
                lblErroSenhaAlteracao.Text = "As senhas novas digitadas não conferem.";
                return false;
            }

            return true;
        }

        protected void ib_ok_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (ViewState["USUARIO.ID"] == null)
            {
                SetPanelLoginVisibility(true);
            }
            else if (ValidaNovaSenha())
            {
                string msg;
                TechneAuthentication.ChangePassword(cUsuario.Text, txtSenhaAtual.Text, txtSenhaNova1.Text, out msg);
                if (msg != null)
                {
                    lblErroSenhaAlteracao.Text = msg;
                }
                else
                {
                    Autentica(cUsuario.Text, txtSenhaNova1.Text);
                }
            }
        }

        protected void ib_cancelar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SetPanelLoginVisibility(true);
        }

        protected void lnkPedidoRedefinicaoSenha_Click(object sender, EventArgs e)
        {
            PanelPedidoRedefinicaoSenha.Visible = true;
            PanelLogin.Visible = false;
            PanelPassword.Visible = false;
            PanelPedidoUsuario.Visible = false;

            cUsuarioRS.Text = string.Empty;
            txtChaveRS.Text = string.Empty;
            divRedefinicaoSenha.Visible = true;
            lblErroSenhaRS.Text = string.Empty;
            PnlAviso.Visible = false;
        }

        protected void lnkPedidoUsuario_Click(object sender, EventArgs e)
        {
            PanelPedidoRedefinicaoSenha.Visible = false;
            PanelLogin.Visible = false;
            PanelPassword.Visible = false;
            PanelPedidoUsuario.Visible = true;           

            cUsuarioCPF.Text = string.Empty;
            txtChaveUSU.Text = string.Empty;
            divPedidoUsuario.Visible = true;
            lblErroSenhaUSU.Text = string.Empty;
        }

        protected void lnkRetornarLogin_Click(object sender, EventArgs e)
        {
            SetPanelLoginVisibility(true);
        }

        protected void bt_EntrarRS_Click(object sender, EventArgs e)
        {
            lblErroSenhaRS.Text = string.Empty;

            if (VerificaPreenchimetoRedefinicaoSenha())
            {
                RN.Usuarios rnUsuarios = new Techne.Lyceum.RN.Usuarios();

                var pessoa = Lyceum.RN.Pessoa.ObtemPessoaPorUsuario(cUsuarioRS.Text);
                var usuario = Lyceum.RN.Usuarios.Consultar(cUsuarioRS.Text);
                var novaSenha = RandomString(8);

                if (pessoa.Pessoa == 0)
                {
                    divRedefinicaoSenha.Visible = false;
                    lblErroSenhaRS.Text = "O usuário \"" + cUsuarioRS.Text + "\" não existe.";
                    return;
                }

                if (pessoa.E_mail_interno.IsNullOrEmptyOrWhiteSpace() && pessoa.E_mail_google.IsNullOrEmptyOrWhiteSpace())
                {
                    divRedefinicaoSenha.Visible = false;
                    lblErroSenhaRS.Text = "O usuário \"" + cUsuarioRS.Text + "\" não possui e-mail interno nem e-mail google cadastrado. Por gentileza, solicitar a redefinição de senha pelo whatsapp (21)2380-9199.";
                    return;
                }

                //if (usuario.Alterar_senha == "S")
                //{
                //    divRedefinicaoSenha.Visible = false;
                //    lblErroSenhaRS.Text = "O usuário \"" + cUsuarioRS.Text + "\" já solicitou redefinição de senha. Se não recebeu o e-mail com a senha temporária, solicite novamente pelo sistema https://suporteti.educacao.rj.gov.br, cujo acesso é feito através de seu e-mail institucional.";
                //    return;
                //}

                rnUsuarios.AlteraSenhaUsuario(cUsuarioRS.Text, novaSenha);
                
                if (!pessoa.E_mail_interno.IsNullOrEmptyOrWhiteSpace())
                    EnviaEmailRedefinicaoSenha(pessoa.E_mail_interno, pessoa.Nome_compl, cUsuario.Text, novaSenha);

                if (!pessoa.E_mail_google.IsNullOrEmptyOrWhiteSpace())
                EnviaEmailRedefinicaoSenha(pessoa.E_mail_google, pessoa.Nome_compl, cUsuario.Text, novaSenha);

                divRedefinicaoSenha.Visible = false;

                lblErroSenhaRS.Text = string.Empty;

                if (!pessoa.E_mail_interno.IsNullOrEmptyOrWhiteSpace())
                    lblErroSenhaRS.Text += (lblErroSenhaRS.Text != string.Empty ? ", " : "") + "\"" + EmailMask(pessoa.E_mail_interno) + "\"";

                if (!pessoa.E_mail_google.IsNullOrEmptyOrWhiteSpace())
                    lblErroSenhaRS.Text += (lblErroSenhaRS.Text != string.Empty ? ", " : "") + "\"" + EmailMask(pessoa.E_mail_google) + "\"";

                lblErroSenhaRS.Text = "A nova senha foi enviada para os e-mails : " + lblErroSenhaRS.Text;
                PnlAviso.Visible = true;


            }
            else
            {
                txtChaveRS.Text = string.Empty;
            }
        }

        protected void bt_EntrarUSU_Click(object sender, EventArgs e)
        {
            RN.Pessoa rnPessoa = new Pessoa();
            lblErroSenhaUSU.Text = string.Empty;

            if (VerificaPreenchimetoPedidoUsuario())
            {
                RN.Usuarios rnUsuarios = new Techne.Lyceum.RN.Usuarios();
                RN.Entidades.LyPessoa pessoa = rnPessoa.ObtemPessoaPor(cUsuarioCPF.Text);
                List<string> usuarios = rnUsuarios.ListaUsuarioPor(pessoa.Pessoa);

                if (pessoa.Pessoa == 0) 
                {
                    divRedefinicaoSenha.Visible = false;
                    lblErroSenhaUSU.Text = "CPF \"" + cUsuarioCPF.Text + "\" não cadastrado.";
                    return;
                }

                if (usuarios.Count == 0)
                {
                    divRedefinicaoSenha.Visible = false;
                    lblErroSenhaUSU.Text = "O CPF \"" + cUsuarioCPF.Text + "\" não possui usuário habilitado. Por gentileza, abra um chamado pelo sistema https://suporteti.educacao.rj.gov.br, cujo acesso é feito através de seu e-mail institucional.";
                    return;
                }

                if (pessoa.E_mail_interno.IsNullOrEmptyOrWhiteSpace() && pessoa.E_mail_google.IsNullOrEmptyOrWhiteSpace())
                {
                    divRedefinicaoSenha.Visible = false;
                    lblErroSenhaUSU.Text = "O CPF \"" + cUsuarioCPF.Text + "\" não possui e-mail interno nem e-mail google cadastrado. Por gentileza, acionar através do whatsapp (21)2380-9199.";
                    return;
                }

                List<string> email = new List<string>();
                if (!pessoa.E_mail_interno.IsNullOrEmptyOrWhiteSpace())
                {
                    email.Add(pessoa.E_mail_interno);
                }
                if (!pessoa.E_mail_google.IsNullOrEmptyOrWhiteSpace())
                {
                    email.Add(pessoa.E_mail_google);
                }

                if (!pessoa.E_mail_interno.IsNullOrEmptyOrWhiteSpace())
                    EnviaEmailPedidoUsuario(pessoa.E_mail_interno, pessoa.Nome_compl, usuarios, email);

                if (!pessoa.E_mail_google.IsNullOrEmptyOrWhiteSpace())
                    EnviaEmailPedidoUsuario(pessoa.E_mail_google, pessoa.Nome_compl, usuarios, email); 

                divPedidoUsuario.Visible = false;

                lblErroSenhaUSU.Text = string.Empty;

                if (!pessoa.E_mail_interno.IsNullOrEmptyOrWhiteSpace())
                    lblErroSenhaUSU.Text += (lblErroSenhaUSU.Text != string.Empty ? ", " : "") + "\"" + EmailMask(pessoa.E_mail_interno) + "\"";

                if (!pessoa.E_mail_google.IsNullOrEmptyOrWhiteSpace())
                    lblErroSenhaUSU.Text += (lblErroSenhaUSU.Text != string.Empty ? ", " : "") + "\"" + EmailMask(pessoa.E_mail_google) + "\"";

                lblErroSenhaUSU.Text = "O usuário foi enviado para os e-mails : " + lblErroSenhaUSU.Text;
                          
            }

            else
            {
                txtChaveUSU.Text = string.Empty;
            }
        }

        private void EnviaEmailRedefinicaoSenha(string email, string nome, string usuario, string novaSenha)
        {
            EmailApi rnEmailApi = new EmailApi();

            try
            {
                var host = System.Configuration.ConfigurationManager.AppSettings["EmailApi_Host"];
                var port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EmailApi_Port"]);

                var from = System.Configuration.ConfigurationManager.AppSettings["EmailRedefinicaoSenha_From"];
                var fromName = System.Configuration.ConfigurationManager.AppSettings["EmailRedefinicaoSenha_FromName"];
                var bcc = System.Configuration.ConfigurationManager.AppSettings["EmailRedefinicaoSenha_Bcc"];
                var bccName = System.Configuration.ConfigurationManager.AppSettings["EmailRedefinicaoSenha_BccName"];
                var userName = System.Configuration.ConfigurationManager.AppSettings["EmailRedefinicaoSenha_UserName"];
                var password = System.Configuration.ConfigurationManager.AppSettings["EmailRedefinicaoSenha_Password"];
                var subject = System.Configuration.ConfigurationManager.AppSettings["EmailRedefinicaoSenha_Subject"];
                var urlRoot = System.Configuration.ConfigurationManager.AppSettings["EmailRedefinicaoSenha_UrlRoot"];

                string emailMessage = @"
                
                <table cellpadding=""0"" cellspacing=""0"" width=""600"" border=""0"" style=""border:1px solid gray;"">
                    <tbody>
                        <tr>
                            <td colspan=""3"" height=""120"" style=""border-bottom:1px solid gray;"">
                                <img src=""{{URL_ROOT}}/images/email-top-conexao.png"">
                            </td>
                        </tr>
                        <tr>
                            <td colspan=""3"" height=""35"" align=""center"" valign=""middle"" bgcolor=""#e0eaf1"">
                                <p>Prezado(a), {{NOME}}</p>
                            </td>
                        </tr>
                        <tr>
                            <td colspan=""3"" height=""45"" style="""" align=""center"" bgcolor=""#e0eaf1"">A sua senha foi redefinida para:</td>
                        </tr>
                        <tr>
                            <td colspan=""3"" style=""text-align:center;font-weight:bold;font-size:16px;"" bgcolor=""#e0eaf1"">{{NOVA_SENHA}}</td>
                        </tr>
                        <tr>
                            <td colspan=""3"" height=""70"" align=""center"" bgcolor=""#e0eaf1"">
                                <p> Atenciosamente, SUPTI </p>
                            </td>
                        </tr>
                        <tr>
                            <td height=""140"" align=""center"" bgcolor=""#343f64"" width=""180"" style=""border-top:1px solid gray;"">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td colspan=""3"" align=""center"" valign=""bottom"" height=""40"" style=""color:white;font-size:small;""><b>Siga nossas redes sociais</b></td>
                                        </tr>
                                        <tr>
                                            <td width=""60"" height=""100"">
                                                <img src=""{{URL_ROOT}}/images/email-fb.png"">
                                            </td>
                                            <td width=""60"">
                                                <img src=""{{URL_ROOT}}/images/email-twitter.png"">
                                            </td>
                                            <td width=""60"">
                                                <img src=""{{URL_ROOT}}/images/email-instagram.png"">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                            <td align=""center"" bgcolor=""#343f64"" width=""180"" style=""border-top:1px solid gray;border-left:1px solid gray;"">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td colspan=""3"" align=""center"" valign=""bottom"" height=""40"" style=""color:white;font-size:small;""><b>Contate-nos</b></td>
                                        </tr>
                                        <tr>
                                            <td colspan=""3"" align=""center"" height=""100"" style=""color:white;font-size:small;"">
                                                <p>Tel: (21) 2380-9199</p>
                                                <p>Email: suporteti@educacao.rj.gov.br</p>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                            <td align=""center"" bgcolor=""#343f64"" width=""180"" style=""border-top:1px solid gray;border-left:1px solid gray;"">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td colspan=""3"" align=""center"" valign=""bottom"" height=""40"" style=""color:white;font-size:small;""><b>Endereço</b></td>
                                        </tr>
                                        <tr>
                                            <td colspan=""3"" align=""center"" height=""100"" style=""color:white;font-size:small;"">
                                                <p>Rua Joaquim Palhares, 40</p>
                                                <p>Cidade Nova, Rio de Janeiro - RJ</p>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>

                ";

                emailMessage = emailMessage
                    .Replace("{{URL_ROOT}}", urlRoot)
                    .Replace("{{NOME}}", nome)
                    .Replace("{{USUARIO}}", usuario)
                    .Replace("{{NOVA_SENHA}}", novaSenha)
                ;

                var emailObject = new RN.Util.EmailApi.EmailDTO
                {
                    Smtp = new EmailApi.EmailDTO.SmtpDTO
                    {
                        Host = host,
                        Port = port,
                        UserName = userName,
                        Password = password,
                        EnableSSL = true,
                    },
                    Message = new EmailApi.EmailDTO.MessageDTO
                    {
                        From = new EmailApi.EmailDTO.MessageDTO.MailAddressDTO
                        {
                            Address = from,
                            Name = fromName
                        },
                        To = new List<EmailApi.EmailDTO.MessageDTO.MailAddressDTO>
                        {
                            new EmailApi.EmailDTO.MessageDTO.MailAddressDTO { Address = email, Name = nome },
                        },
                        Subject = subject,
                        Body = emailMessage,
                        IsBodyHtml = true,
                    },
                };

                if (!bcc.IsNullOrEmptyOrWhiteSpace() && !bccName.IsNullOrEmptyOrWhiteSpace())
                    emailObject.Message.Bcc.Add(new EmailApi.EmailDTO.MessageDTO.MailAddressDTO { Address = bcc, Name = bccName });

                var emailApiResult = rnEmailApi.EmailApiSend(emailObject);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void EnviaEmailPedidoUsuario(string email, string nome, List<string> usuarios, List<string> emails)
        {
            EmailApi rnEmailApi = new EmailApi();

            try
            {
                var host = System.Configuration.ConfigurationManager.AppSettings["EmailApi_Host"];
                var port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EmailApi_Port"]);

                var from = System.Configuration.ConfigurationManager.AppSettings["EmailRedefinicaoSenha_From"];
                var fromName = "Usuário Conexão Educação - Gestão";
                var bcc = System.Configuration.ConfigurationManager.AppSettings["EmailRedefinicaoSenha_Bcc"];
                var bccName = System.Configuration.ConfigurationManager.AppSettings["EmailRedefinicaoSenha_BccName"];
                var userName = System.Configuration.ConfigurationManager.AppSettings["EmailRedefinicaoSenha_UserName"];
                var password = System.Configuration.ConfigurationManager.AppSettings["EmailRedefinicaoSenha_Password"];
                var subject = "Usuário Conexão Educação - Gestão";
                var urlRoot = System.Configuration.ConfigurationManager.AppSettings["EmailRedefinicaoSenha_UrlRoot"];
                
                StringBuilder msg = new StringBuilder();

                msg.Append(@"                
                <table cellpadding=""0"" cellspacing=""0"" width=""600"" border=""0"" style=""border:1px solid gray;"">
                    <tbody>
                        <tr>
                            <td colspan=""2"" height=""120"" style=""border-bottom:1px solid gray;"">
                                <img src=""{{URL_ROOT}}/images/email-top-conexao.png"">
                            </td>
                        </tr>
                        <tr>
                            <td colspan=""2"" height=""35"" align=""center"" valign=""middle"" bgcolor=""#e0eaf1"">
                                <p>Prezado(a), {{NOME}}</p>
                            </td>
                        </tr>
                        <tr>
                            <td colspan=""2"" height=""45"" style="""" align=""center"" bgcolor=""#e0eaf1"">Seu(s) usuário(s) para acessar o Conexão Gestão:</td>
                        </tr> ");

                foreach (var item in usuarios)
                {
                     msg.Append(string.Format(@"  
                        <tr>
                            <td colspan=""2"" style=""text-align:center;font-weight:bold;font-size:16px;"" bgcolor=""#e0eaf1"">{0}</td>
                        </tr> ", item));
                }

                 msg.Append(@"  
                        <tr>
                            <td colspan=""2"" height=""45"" style="""" align=""center"" bgcolor=""#e0eaf1""><i>IMPORTANTE: Cada usuário tem acesso de acordo com sua função de lotação.</i></td>
                        </tr>
                        <tr>
                            <td colspan=""2"" height=""45"" style="""" align=""center"" bgcolor=""#e0eaf1"">Seu(s) e-mail(s) cadastrado(s) no Conexão Gestão:</td>
                        </tr>");

                 foreach (var item in emails)
                 {
                     msg.Append(string.Format(@"  
                        <tr>
                            <td colspan=""2"" style=""text-align:center;font-weight:bold;font-size:16px;"" bgcolor=""#e0eaf1"">{0}</td>
                        </tr> ", item));
                 }

                 msg.Append(@"                          
                        <tr>
                            <td colspan=""2"" height=""70"" align=""center"" bgcolor=""#e0eaf1"">
                                <p> Atenciosamente, SUPTI </p>
                            </td>
                        </tr>
                        <tr>
                            <td height=""140"" align=""center"" bgcolor=""#343f64"" width=""180"" style=""border-top:1px solid gray;"">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td colspan=""3"" align=""center"" valign=""bottom"" height=""40"" style=""color:white;font-size:small;""><b>Siga nossas redes sociais</b></td>
                                        </tr>
                                        <tr>
                                            <td width=""60"" height=""100"">
                                                <img src=""{{URL_ROOT}}/images/email-fb.png"">
                                            </td>
                                            <td width=""60"">
                                                <img src=""{{URL_ROOT}}/images/email-twitter.png"">
                                            </td>
                                            <td width=""60"">
                                                <img src=""{{URL_ROOT}}/images/email-instagram.png"">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                            <td align=""center"" bgcolor=""#343f64"" width=""180"" style=""border-top:1px solid gray;border-left:1px solid gray;"">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td colspan=""3"" align=""center"" valign=""bottom"" height=""40"" style=""color:white;font-size:small;""><b>Contate-nos</b></td>
                                        </tr>
                                        <tr>
                                            <td colspan=""3"" align=""center"" height=""100"" style=""color:white;font-size:small;"">
                                                <p>Tel: (21) 2380-9199</p>
                                                <p>Email: suporteti@educacao.rj.gov.br</p>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>                            
                        </tr>
                    </tbody>
                </table>

                ");

                string emailMessage = msg.ToString();

                emailMessage = emailMessage
                    .Replace("{{URL_ROOT}}", urlRoot)
                    .Replace("{{NOME}}", nome)
                ;

                var emailObject = new RN.Util.EmailApi.EmailDTO
                {
                    Smtp = new EmailApi.EmailDTO.SmtpDTO
                    {
                        Host = host,
                        Port = port,
                        UserName = userName,
                        Password = password,
                        EnableSSL = true,
                    },
                    Message = new EmailApi.EmailDTO.MessageDTO
                    {
                        From = new EmailApi.EmailDTO.MessageDTO.MailAddressDTO
                        {
                            Address = from,
                            Name = fromName
                        },
                        To = new List<EmailApi.EmailDTO.MessageDTO.MailAddressDTO>
                        {
                            new EmailApi.EmailDTO.MessageDTO.MailAddressDTO { Address = email, Name = nome },
                        },
                        Subject = subject,
                        Body = emailMessage,
                        IsBodyHtml = true,
                    },
                };

                if (!bcc.IsNullOrEmptyOrWhiteSpace() && !bccName.IsNullOrEmptyOrWhiteSpace())
                    emailObject.Message.Bcc.Add(new EmailApi.EmailDTO.MessageDTO.MailAddressDTO { Address = bcc, Name = bccName });

                var emailApiResult = rnEmailApi.EmailApiSend(emailObject);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private string EmailMask(string email)
        {
	        return Regex.Replace(email, @"(?<=[\w]{1})[\w-\._\+%]*(?=[\w]{1}@)", m => new string('*', m.Length));
        }
    }
}