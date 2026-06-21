using System;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Web.Security;
using Proderj.Foundation.Framework.Web.Seguranca;

using Proderj.DOL.Exception;
using Proderj.DOL.Repository;
using Proderj.DOL.Domain;
using System.Text;
using System.Web;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;


using Proderj.Foundation.Framework;
using Proderj.Foundation.Common;
using System.Collections;


namespace Proderj.DOL.Service
{
    public class LoginService : ILoginService
    {
        private IDocenteRepository repositorioDeDocente;
        private IPessoaRepository repositorioDePessoa;
        private ILoginRepository repositorioDeLogin;
        private ITermoCompromissoDocenteRepository repositorioDeTermoAceite;
        private IDadosGeraisDocenteRepository repositorioDadosGerais;

        public LoginService(ILoginRepository repositorioDeLogin, IDocenteRepository repositorioDeDocente, IPessoaRepository repositorioDePessoa, ITermoCompromissoDocenteRepository repositorioDeTermoAceite, IDadosGeraisDocenteRepository repositorioDadosGerais)
    	{
    		this.repositorioDeLogin = repositorioDeLogin;
    		this.repositorioDeDocente = repositorioDeDocente;
            this.repositorioDePessoa = repositorioDePessoa;
            this.repositorioDeTermoAceite = repositorioDeTermoAceite;
            this.repositorioDadosGerais = repositorioDadosGerais;
    	}
        
        public string VerificaNumFunc(string vinculo, long idfuncional)
        {
            String mtr = repositorioDePessoa.BuscaNunFunc(vinculo, idfuncional);

            return mtr;
        }

        public List<string> VerificaIdVinculoFunc(string cpf)
        {
            List<string> mtr = repositorioDePessoa.BuscaIdVinculo(cpf);

            return mtr;
        }

        public DTODocenteLogado VerificaLogin(string pessoapar, string senha, string captcha, string idfuncional, string vinculo)
    	{
            string captchaGerado    = string.Empty;
            
            if (HttpContext.Current.Request.Cookies["captcha"] != null)
            {
                captchaGerado = HttpContext.Current.Request.Cookies["captcha"].Value;
            }

            if (captchaGerado != captcha)
            {
                throw new LoginException(LoginException.TipoEnum.CaptchaNaoConfere);
            }

            if (string.IsNullOrWhiteSpace(pessoapar))
            {
                throw new LoginException(LoginException.TipoEnum.DocenteInexistente);
            } 

            Docente docente = repositorioDeDocente.ObtemPorPessoa(pessoapar); //VW_LY_PESSOA não tem o idfuncional nessa view
            bool docenteInexistente = (docente == null);

			if (docenteInexistente)
			{
				throw new LoginException(LoginException.TipoEnum.DocenteInexistente);
			}           

            if (docente.Pessoa == null)
            {
                throw new LoginException(LoginException.TipoEnum.PessoaInexistente);
            }

            Pessoa pessoa = repositorioDePessoa.ObtemPor(docente.Pessoa.Id);

            if (pessoa == null)
            {
                throw new LoginException(LoginException.TipoEnum.PessoaInexistente);
            }

    		string senhaCriptografada = CriptografaSenha(senha);
			
			//Aceita a senha criptografada ou nao no repositorio
			if (docente.SenhaDocente != senhaCriptografada && docente.SenhaDocente != senha)
			{
				throw new LoginException(LoginException.TipoEnum.SenhaIncorreta);
			}

    		var dtoDocente = new DTODocenteLogado
    		                 	{
    		                 		Email = docente.Pessoa.EmailInterno,
                                    Nome = pessoa.NomeCompleto,
    		                 		Matricula = docente.Matricula,
                                    IdFuncional = idfuncional,
                                    Senha = senha,
                                    Vinculo = vinculo,
    		                 		NumeroFuncionario = docente.NumeroFuncionario
    		                 	};

			if (docente.SenhaAlterada == "S")
			{
				dtoDocente.AlteracaoDeSenhaNecessaria = true;
			}
			else
			{
				//dtoDocente = VerificaSePrecisaAceitarTermo(dtoDocente, matricula);
                dtoDocente = VerificaSePrecisaAceitarTermoPesssoa(dtoDocente, idfuncional);
			}

    		return dtoDocente;
    	}

        public DTODocenteLogado VerificaLoginApi(string pessoapar, string senha, string idfuncional, string vinculo)
        {
          
            if (pessoapar == null)
            {
                throw new LoginException(LoginException.TipoEnum.DocenteInexistente);
            }

            Docente docente = repositorioDeDocente.ObtemPorPessoa(pessoapar); //VW_LY_PESSOA não tem o idfuncional nessa view
            bool docenteInexistente = (docente == null);

            if (docenteInexistente)
            {
                throw new LoginException(LoginException.TipoEnum.DocenteInexistente);
            }

            if (docente.Pessoa == null)
            {
                throw new LoginException(LoginException.TipoEnum.PessoaInexistente);
            }

            Pessoa pessoa = repositorioDePessoa.ObtemPor(docente.Pessoa.Id);

            if (pessoa == null)
            {
                throw new LoginException(LoginException.TipoEnum.PessoaInexistente);
            }

            string senhaCriptografada = CriptografaSenha(senha);

            //Aceita a senha criptografada ou nao no repositorio
            if (docente.SenhaDocente != senhaCriptografada && docente.SenhaDocente != senha)
            {
                throw new LoginException(LoginException.TipoEnum.SenhaIncorreta);
            }

            var dtoDocente = new DTODocenteLogado
            {
                Email = docente.Pessoa.EmailInterno,
                Nome = pessoa.NomeCompleto,
                Matricula = docente.Matricula,
                IdFuncional = idfuncional,
                Senha = senha,
                Vinculo = vinculo,
                NumeroFuncionario = docente.NumeroFuncionario
            };

            if (docente.SenhaAlterada == "S")
            {
                dtoDocente.AlteracaoDeSenhaNecessaria = true;
            }
            else
            {
                //dtoDocente = VerificaSePrecisaAceitarTermo(dtoDocente, matricula);
                dtoDocente = VerificaSePrecisaAceitarTermoPesssoa(dtoDocente, idfuncional);
            }

            return dtoDocente;
        }

		private DTODocenteLogado VerificaSePrecisaAceitarTermo(DTODocenteLogado dtoDocenteLogadoPrecarregado, string matricula)
		{
            repositorioDeLogin.Inclui(TipoSistema.DOL, matricula, DateTime.Now);

			bool aceitouTermo = (repositorioDeTermoAceite.ObtemTermoNaoAceitoMaisRecentePor(matricula) == null);

			dtoDocenteLogadoPrecarregado.AceitouTermoDeAceite = aceitouTermo;

			return dtoDocenteLogadoPrecarregado;
		}


        private DTODocenteLogado VerificaSePrecisaAceitarTermoPesssoa(DTODocenteLogado dtoDocenteLogadoPrecarregado, string idfuncional)
		{
            repositorioDeLogin.Inclui(TipoSistema.DOL, idfuncional, DateTime.Now);

            bool aceitouTermo = (repositorioDeTermoAceite.ObtemTermoNaoAceitoMaisRecentePorIdFuncional(idfuncional) == null);

			dtoDocenteLogadoPrecarregado.AceitouTermoDeAceite = aceitouTermo;

			return dtoDocenteLogadoPrecarregado;
		}	
        

    	private string CriptografaSenha(string senha)
    	{

    		byte[] bytes = Encoding.ASCII.GetBytes(senha);
    		var sha1Managed = new SHA1Managed();
    		byte[] inArray = sha1Managed.ComputeHash(bytes);

			var sb = new StringBuilder();
			foreach (byte b in inArray)
				sb.Append(b.ToString("X2"));

			string hexString = sb.ToString();

    		return hexString;
		}

        private static string ExtrairSenhaDoUserData(string userData)
        {
            var ini = userData.IndexOf("&Senha=") + 7;
            var fim = userData.IndexOf("&IdFuncional=");
            var senha = userData.Substring(ini, fim - ini);
            return senha;
        }

		public static IPrincipal ObtemDocenteLogadoPrincipalPor(string informacaoCookieDeAutenticacao)
		{
            bool cookieDeAutenticacaoEncontrado = !String.IsNullOrEmpty(informacaoCookieDeAutenticacao);


			DTODocenteLogadoPrincipal dtoUsuarioPrincipalConcreto;
			
			if (cookieDeAutenticacaoEncontrado)
            {
                IFormsAuthenticationBase formsBase = new FormsAuthenticationBase();
                var ticketDeAutenticacao = formsBase.DescriptografarTicket(informacaoCookieDeAutenticacao);

                IAutenticacaoSerializador autenticador = new AutenticacaoSerializadorPadrao();

                /* BUG !!!
                O método Deserializa está com problemas para entender caracteres especiais de QueryString.
                Dessa forma, se a senha possuir quaisquer caracteres que sejam confundidos com os caracteres
                especiais de uma QueryString, estes serão suprimidos e a senha não refletirá o que foi
                digitado pelo usuário.
                Com isso, a autenticação do DocenteOnline (.NET 8) irá falhar porque a senha chegará errada lá.
                Logo, foi necessário fazer uma forma alternativa para obter a senha que está no UserData, usando
                substring entre o nome dos parâmetros Senha e IdFuncional, considerando que estes parâmetros nunca
                mudarão de posição.
                */
                DTODocenteLogado dtoUsuarioConcreto = autenticador.Deserializa<DTODocenteLogado>(ticketDeAutenticacao.UserData);
                var senhaObtidaViaSubstring = ExtrairSenhaDoUserData(ticketDeAutenticacao.UserData);
                dtoUsuarioConcreto.Senha = senhaObtidaViaSubstring;

                dtoUsuarioPrincipalConcreto = new DTODocenteLogadoPrincipal(dtoUsuarioConcreto);
            }
			else
			{
				//Caso nao encontre cookie de autenticacao retorna nulo para o usuário.
				dtoUsuarioPrincipalConcreto = null;
			}
			return dtoUsuarioPrincipalConcreto;
		}

		#region ILoginService Members

		public void DesconectaDocenteForms() {
			var autenticador = new AutenticacaoForms();
			autenticador.DesconectarDoFormsAuthentication();
		}

		public void AutenticaDocenteForms(DTODocenteLogado dtoDocenteLogado, short minutosParaExpirarAutenticacao)
		{
			var autenticador = new AutenticacaoForms();
			IAutenticavel dtoDocenteAutenticavel = ObtemDocenteLogadoAutenticavelPor(dtoDocenteLogado);
			autenticador.AutenticaNoFormsAuthentication(dtoDocenteAutenticavel, DateTime.Now, DateTime.Now.AddMinutes(minutosParaExpirarAutenticacao));
		}

		private IAutenticavel ObtemDocenteLogadoAutenticavelPor(DTODocenteLogado dtoDocenteLogado)
		{
			return new DTODocenteLogadoAutenticavel(dtoDocenteLogado.Matricula)
			{
				Nome = dtoDocenteLogado.Nome,
				Email = dtoDocenteLogado.Email,
                Senha = dtoDocenteLogado.Senha,
                IdFuncional = dtoDocenteLogado.IdFuncional,
                Vinculo = dtoDocenteLogado.Vinculo,
				NumeroFuncionario = dtoDocenteLogado.NumeroFuncionario,
				AceitouTermoDeAceite = dtoDocenteLogado.AceitouTermoDeAceite
			};
		}

        public string RedefineSenha(string matricula, string captcha)
        {
            if (string.IsNullOrWhiteSpace(matricula))
                return "A matrícula não foi informada.";

            string aux = matricula;
            var palavras = aux.Split('/');

            // valida se veio no formato correto
            if (palavras.Length < 2)
                return "Não é possível resetar a senha: o ID informado não é válido ou não pertence a um docente.";

            var idpessoaencontrada = VerificaNumFunc(palavras[1], Convert.ToInt64(palavras[0]));
            
            string captchaGerado = string.Empty;
            if (HttpContext.Current.Request.Cookies["captcha"] != null)
                captchaGerado = HttpContext.Current.Request.Cookies["captcha"].Value;

            if (captchaGerado != captcha)
                return "O código fornecido não está correto.";

            if (idpessoaencontrada == null || idpessoaencontrada == String.Empty)
                return "Não é possível resetar a senha: o ID informado não é válido ou não pertence a um docente.";

            var docente = repositorioDeDocente.ObtemPorPessoa(idpessoaencontrada);
            var dadosGeraisDocente = repositorioDadosGerais.ObtemPorPessoa(idpessoaencontrada);

            if (docente == null)
                return "O usuário \"" + matricula + "\" não existe.";

            if (dadosGeraisDocente == null)
                return "Não foi possível localizar os dados do usuário \"" + matricula + "\".";


            if (string.IsNullOrEmpty(dadosGeraisDocente.EmailGoogle) && string.IsNullOrEmpty(dadosGeraisDocente.EmailInterno))
                return "O usuário \"" + matricula + "\" não possui e-mail Google cadastrado no formato \"@prof.educa.rj.gov.br\" nem e-mail interno cadastrado no formato \"@prof.educacao.rj.gov.br\". Por gentileza, solicite a redefinição de senha através do whatsapp (21)2380-9199.";

            //if (docente.SenhaAlterada == "S")
            //    return "O usuário \"" + matricula + "\" já solicitou redefinição de senha. Se não recebeu o e-mail com a senha temporária, solicite novamente pelo sistema https://suporteti.educacao.rj.gov.br, cujo acesso é feito através de seu e-mail institucional.";

            var senha = RandomString(8);
            var senhaCriptografada = CriptografaSenha(senha);
            repositorioDeDocente.RedefineSenha(senhaCriptografada, docente.Matricula);

            if (!dadosGeraisDocente.EmailGoogle.IsNullOrEmpty())
                EnviaEmailRedefinicaoSenha(dadosGeraisDocente.EmailGoogle, docente.Pessoa.NomeCompleto, matricula, senha);

            if (!dadosGeraisDocente.EmailInterno.IsNullOrEmpty())
                EnviaEmailRedefinicaoSenha(dadosGeraisDocente.EmailInterno, docente.Pessoa.NomeCompleto, matricula, senha);

            var dtoDocente = new DTODocenteLogado()
            {
                Email = dadosGeraisDocente.EmailGoogle,
                Nome = docente.Pessoa.NomeCompleto,
                Matricula = docente.Matricula,
                IdFuncional = palavras[1],
                Vinculo = palavras[0],
                NumeroFuncionario = docente.NumeroFuncionario,
            };

            var msgEnvioEmail = string.Empty;

            if (!dadosGeraisDocente.EmailGoogle.IsNullOrEmpty())
                msgEnvioEmail += (msgEnvioEmail != string.Empty ? ", " : "") + "\"" + EmailMask(dadosGeraisDocente.EmailGoogle) + "\"";
                
            if (!dadosGeraisDocente.EmailInterno.IsNullOrEmpty())
                msgEnvioEmail += (msgEnvioEmail != string.Empty ? ", " : "") + "\"" + EmailMask(dadosGeraisDocente.EmailInterno) + "\"";

            msgEnvioEmail = "A nova senha foi enviada para o e-mail: " + msgEnvioEmail;

            return msgEnvioEmail;
        }

        public string PedidoUsuario(string cpf, string captcha)
        {
            string captchaGerado = string.Empty;
            if (HttpContext.Current.Request.Cookies["captcha"] != null)
                captchaGerado = HttpContext.Current.Request.Cookies["captcha"].Value;

            if (captchaGerado != captcha)
                return "O código fornecido não está correto.";

            if (cpf.IsNullOrEmpty())
                return "CPF é obrigatorio.";

            List<string> usuarios = VerificaIdVinculoFunc(cpf);
            string nome;

            if (usuarios.Count == 0)
                return "O CPF \"" + cpf + "\" não possui ID Funcional/Vínculo habilitado.";

            List<string> emails  = repositorioDadosGerais.ObtemEmailsPor(cpf, out nome);

            if (emails.Count == 0)
                return "O CPF \"" + cpf + "\" não possui e-mail Google cadastrado no formato \"@prof.educa.rj.gov.br\" nem e-mail interno cadastrado no formato \"@prof.educacao.rj.gov.br\". Por gentileza, solicite o ID Funcional/Vínculo através do whatsapp (21)2380-9199.";

            var msgEnvioEmail = string.Empty;

            foreach (var email in emails)
            {
                EnviaEmailPedidoUsuario(email, nome, usuarios, emails);
                msgEnvioEmail += (msgEnvioEmail != string.Empty ? ", " : "") + "\"" + EmailMask(email) + "\"";
            }           


            msgEnvioEmail = "O ID Funcional/Vínculo foi enviado para o e-mail: " + msgEnvioEmail;

            return msgEnvioEmail;
        }

        public DTODocenteLogado VerificaAlteraSenha(string matricula, string vinculo, string idfuncional, string senhaAtual, string senhaNova, string senhaNovaConfirmacao)
        {
            Docente docente = repositorioDeDocente.ObtemPor(matricula);
            Pessoa pessoa = repositorioDePessoa.ObtemPor(docente.Pessoa.Id);
            DTODocenteLogado dtoDocente = null;
            string senhaCriptografada = string.Empty;

            if (docente == null)
            {
                throw new LoginException(LoginException.TipoEnum.DocenteInexistente);
            }

            senhaCriptografada = CriptografaSenha(senhaAtual);

            // Como, em produção, pode haver caso da senha não estar criptografada, este caso deve ser considerado também.
            if (senhaCriptografada != docente.SenhaDocente && senhaAtual != docente.SenhaDocente)
            {
                throw new LoginException(LoginException.TipoEnum.SenhaAtualIncorreta);
            }

            if (string.IsNullOrEmpty(senhaNova) || senhaNova.Length < 6)
            {
                throw new LoginException(LoginException.TipoEnum.SenhaCurta);
            }

            if (senhaNova != senhaNovaConfirmacao)
            {
                throw new LoginException(LoginException.TipoEnum.SenhasNaoConferem);
            }

            senhaCriptografada = CriptografaSenha(senhaNova);

            repositorioDeDocente.AlteraSenha(senhaCriptografada, matricula);

            dtoDocente = new DTODocenteLogado()
            {
                Email = docente.Pessoa.EmailInterno,
                Nome = pessoa.NomeCompleto,
                Matricula = docente.Matricula,
                IdFuncional = idfuncional,
                Vinculo = vinculo,
                NumeroFuncionario = docente.NumeroFuncionario,
                Senha = senhaNova,
            };

            if (docente.SenhaAlterada == "S")
            {
                dtoDocente.AlteracaoDeSenhaNecessaria = true;
            }
            else
            {
                dtoDocente = VerificaSePrecisaAceitarTermo(dtoDocente, matricula);
            }

            return dtoDocente;
        }

		#endregion

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
                                <img src=""{{URL_ROOT}}/Imagens/email-top-docente.png"">
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
                                <p> Para efetuar login na Migração da carga horária do Professor Docente I - 18 horas para 30 horas, é necessário aguardar o prazo de 1 hora para liberação do acesso.</p>
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
                                                <img src=""{{URL_ROOT}}/Imagens/email-fb.png"">
                                            </td>
                                            <td width=""60"">
                                                <img src=""{{URL_ROOT}}/Imagens/email-twitter.png"">
                                            </td>
                                            <td width=""60"">
                                                <img src=""{{URL_ROOT}}/Imagens/email-instagram.png"">
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

                var emailObject = new EmailApi.EmailDTO
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

                if (!string.IsNullOrWhiteSpace(bcc) && !string.IsNullOrWhiteSpace(bccName))
                    emailObject.Message.Bcc.Add(new EmailApi.EmailDTO.MessageDTO.MailAddressDTO { Address = bcc, Name = bccName });

                var emailApiResult = rnEmailApi.EmailApiSend(emailObject);
            }
            catch (System.Exception ex)
            {
                throw ex;
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
                var fromName = "ID Funcional/Vínculo Conexão Educação - Docente Online";
                var bcc = System.Configuration.ConfigurationManager.AppSettings["EmailRedefinicaoSenha_Bcc"];
                var bccName = System.Configuration.ConfigurationManager.AppSettings["EmailRedefinicaoSenha_BccName"];
                var userName = System.Configuration.ConfigurationManager.AppSettings["EmailRedefinicaoSenha_UserName"];
                var password = System.Configuration.ConfigurationManager.AppSettings["EmailRedefinicaoSenha_Password"];
                var subject = "ID Funcional/Vínculo Conexão Educação - Docente Online";
                var urlRoot = System.Configuration.ConfigurationManager.AppSettings["EmailRedefinicaoSenha_UrlRoot"];

                StringBuilder msg = new StringBuilder();

                msg.Append(@"                
                <table cellpadding=""0"" cellspacing=""0"" width=""600"" border=""0"" style=""border:1px solid gray;"">
                    <tbody>
                        <tr>
                            <td colspan=""2"" height=""120"" style=""border-bottom:1px solid gray;"">
                                <img src=""{{URL_ROOT}}/Imagens/email-top-docente.png"">
                            </td>
                        </tr>
                        <tr>
                            <td colspan=""2"" height=""35"" align=""center"" valign=""middle"" bgcolor=""#e0eaf1"">
                                <p>Prezado(a), {{NOME}}</p>
                            </td>
                        </tr>
                        <tr>
                            <td colspan=""2"" height=""45"" style="""" align=""center"" bgcolor=""#e0eaf1"">Seu(s) ID Funcional/Vínculo(s) para acessar o Docente Online:</td>
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
                            <td colspan=""2"" height=""45"" style="""" align=""center"" bgcolor=""#e0eaf1""><i>IMPORTANTE: Cada ID Funcional/Vínculo tem acesso de acordo com sua lotação.</i></td>
                        </tr>
                        <tr>
                            <td colspan=""2"" height=""45"" style="""" align=""center"" bgcolor=""#e0eaf1"">Seu(s) e-mail(s) cadastrado(s) no Conexão:</td>
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
                                                <img src=""{{URL_ROOT}}/Imagens/email-fb.png"">
                                            </td>
                                            <td width=""60"">
                                                <img src=""{{URL_ROOT}}/Imagens/email-twitter.png"">
                                            </td>
                                            <td width=""60"">
                                                <img src=""{{URL_ROOT}}/Imagens/email-instagram.png"">
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

                var emailObject = new EmailApi.EmailDTO
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

                if (!string.IsNullOrWhiteSpace(bcc) && !string.IsNullOrWhiteSpace(bccName))
                    emailObject.Message.Bcc.Add(new EmailApi.EmailDTO.MessageDTO.MailAddressDTO { Address = bcc, Name = bccName });

                var emailApiResult = rnEmailApi.EmailApiSend(emailObject);
            }
            catch (System.Exception ex)
            {
                throw ex;
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