using System;
using System.Web.Mvc;
using Proderj.DOL.Exception;
using Proderj.DOL.Service;
using Proderj.DOL.WebApp.Models;
using Proderj.Foundation.Framework.Web.Seguranca;
using Resources;
using System.Configuration;
using Proderj.DOL.WebApp.Models.Captcha;
using System.Windows.Forms;
using System.Linq;

using System.IO;
using System.Security.Cryptography;

using AutoMapper;
using Proderj.DOL.Domain;
using Proderj.DOL.WebApp.Models;
using Resources;
using System.Text;


namespace Proderj.DOL.WebApp.Controllers
{
	public class LoginController : ControllerPadrao
    {
		private readonly ILoginService loginService;
    	private readonly ITermoCompromissoDocenteService termoCompromissoService;

        private static readonly byte[] Key = Encoding.UTF8.GetBytes("d$s&T!20%@22@*V`");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("HR$2pIjHR$2pIj12"); 

		public LoginController(ILoginService loginService, ITermoCompromissoDocenteService termoCompromissoService)
		{
			this.loginService = loginService;
			this.termoCompromissoService = termoCompromissoService;
		}

    	//
        // GET: /Login/

        public ActionResult Inicial()
        {
        	var modelo = new LoginInicialViewModel
        	             	{
        	             		LoginEntraModelo = new LoginLogaRequestModel(),
        	             		TituloDaPagina = "Login"
        	             	};

        	modelo.CabecalhoModelo.BotaoAjudaHabilitado = false;
        	modelo.CabecalhoModelo.BotaoSairHabilitado = false;
        	modelo.CabecalhoModelo.BotaoInicioHabilitado = false;
        	modelo.CabecalhoModelo.TituloCabecalho = Recurso.LoginInicial_TituloPagina;
            return View(modelo);
        }

		public ActionResult Loga()
		{
			return RedirectToAction("Inicial");
		}

    	[HttpPost]
		public ActionResult Loga(LoginLogaRequestModel modelo)
		{
            var retornoPadrao = new Func<ViewResult>(() => 
            {
                var modeloInicial = new LoginInicialViewModel { LoginEntraModelo = modelo };
                modeloInicial.CabecalhoModelo.BotaoInicioHabilitado = false;
                modeloInicial.CabecalhoModelo.BotaoSairHabilitado = false;
                modeloInicial.CabecalhoModelo.BotaoAjudaHabilitado = false;
                modeloInicial.CabecalhoModelo.TituloCabecalho = Recurso.LoginInicial_TituloPagina;

                //Limpa Captcha sempre que houver qualquer erro
                modeloInicial.LoginEntraModelo.Codigo = null;
                
                return View("Inicial", modeloInicial);
            });

			ActionResult acaoResultante = null;
            DTODocenteLogado dtoDocenteLogado = null;
            short tempoMaximoLancamento = 60;
            try
            {
                if (!ModelState.IsValid)
                    return retornoPadrao();

                if (modelo.IdFuncional == null)
                {
                    ModelState.AddModelError("", "Id Funcional / Vínculo não pode ficar em branco");
                    return retornoPadrao();
                }

                if (!modelo.IdFuncional.Contains("/"))
                {
                    ModelState.AddModelError("", "É necessário informar o vínculo");
                    return retornoPadrao();
                }

                string aux = modelo.IdFuncional;
                var palavras = aux.Split('/');

                if (!palavras[0].All(q => char.IsDigit(q)))
                {
                    ModelState.AddModelError("", "ID Funcional não pode conter caracteres não-numéricos");
                    return retornoPadrao();
                }

                if (!palavras[1].All(q => char.IsDigit(q)))
                {
                    ModelState.AddModelError("", "Vínculo não pode conter caracteres não-numéricos");
                    return retornoPadrao();
                }

                var matricula = loginService.VerificaNumFunc(palavras[1], Convert.ToInt64(palavras[0]));// na verdade tras o campo num_func
                Session["IdVinculo"] = aux;

                dtoDocenteLogado = loginService.VerificaLogin(matricula, modelo.Senha, modelo.Codigo, palavras[0], palavras[1]);

                if (ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"] != null)
                {
                    tempoMaximoLancamento = short.Parse(ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"]);
                }

                loginService.AutenticaDocenteForms(dtoDocenteLogado, tempoMaximoLancamento);

                /*
                !!! GAMBIARRA !!!
                Feita por Felipe Ribeiro Gomes em 19/05/2022
                    
                Quem programou a "AlteracaoDeSenhaNecessaria" não previu que
                o usuário pudesse logar e depois digitar a URL de uma página
                qualquer no navegador.
                    
                Por isso, pensei em colocar o valor desse atributo em uma variável
                que pudesse ser vista por toda a aplicação, para impedir que o
                usuário possa driblar essa falha de segurança.
                    */

                StaticMethods.AlteracaoDeSenhaNecessaria = dtoDocenteLogado.AlteracaoDeSenhaNecessaria;

                /* !!! FIM DA GAMBIARRA !!! */

                if (dtoDocenteLogado.AlteracaoDeSenhaNecessaria)
                {
                    acaoResultante = RedirectToAction("AlteraSenha");
                }
                else
                {
                    if (!dtoDocenteLogado.AceitouTermoDeAceite)
                    {
                        acaoResultante = RedirectToAction("ConfirmaTermoAceite");
                    }
                    else
                    {
                        return RedirectToAction("Apresentacao");
                    }
                }
            }
            catch (LoginException excecaoDeLogin)
            {
                ModelState.AddModelError(excecaoDeLogin.TipoDeExcecao, excecaoDeLogin.Message);
            }
            catch (System.Exception excecaoDesconhecida)
            {
                acaoResultante = View("ErroInesperado", excecaoDesconhecida);
            }

			if (acaoResultante == null)
			{
				var modeloInicial = new LoginInicialViewModel {LoginEntraModelo = modelo};
				modeloInicial.CabecalhoModelo.BotaoInicioHabilitado = false;
				modeloInicial.CabecalhoModelo.BotaoSairHabilitado = false;
				modeloInicial.CabecalhoModelo.BotaoAjudaHabilitado = false;
				modeloInicial.CabecalhoModelo.TituloCabecalho = Recurso.LoginInicial_TituloPagina;

				acaoResultante = View("Inicial", modeloInicial);
			}

			return acaoResultante;
		}

        public static string Decrypt(string cipherText)
        {
            using (var aes = System.Security.Cryptography.Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream(Convert.FromBase64String(cipherText.Replace(' ', '+'))))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }

        public GeraChave GetCaptcha()
        {
            string captchaText = Captcha.GeraChaveSeguranca();
            System.Web.HttpCookie captchaCookie = new System.Web.HttpCookie("captcha", captchaText);
            HttpContext.Response.Cookies.Add(captchaCookie);
            return new GeraChave(captchaText);
        }

        //TODO: ver como fazer para passar valores sem utilizar js
        public ActionResult TrocaImagem(LoginLogaRequestModel modelo)
        {
            ActionResult acaoResultante = null;

            var modeloInicial = new LoginInicialViewModel { LoginEntraModelo = modelo };
            modeloInicial.CabecalhoModelo.BotaoInicioHabilitado = false;
            modeloInicial.CabecalhoModelo.BotaoSairHabilitado = false;
            modeloInicial.CabecalhoModelo.BotaoAjudaHabilitado = false;
            modeloInicial.CabecalhoModelo.TituloCabecalho = Recurso.LoginInicial_TituloPagina;

            //Limpa Captcha
            modeloInicial.LoginEntraModelo.Codigo = null;

            acaoResultante=  View("Inicial", modeloInicial);

            return acaoResultante;
        }

		private void ReautenticaDocenteAceiteTermo(DocenteLogadoBindModel docenteLogado)
		{
			//Reautentica o usuario com o Aceite termo OK nos Dados
			var dtoDocenteLogado = new DTODocenteLogado
			{
				AceitouTermoDeAceite = true,
				AlteracaoDeSenhaNecessaria = false,
				Email = docenteLogado.Email,
                Senha = docenteLogado.Senha,
				Matricula = docenteLogado.Matricula,
				Nome = docenteLogado.Nome,
				NumeroFuncionario = docenteLogado.NumeroFuncionario
			};

			//Obtem o tempo configurado para o lancamento de notas.
			short tempoMaximoLancamento = 60;
			if (ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"] != null)
			{
				tempoMaximoLancamento = short.Parse(ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"]);
			}

			loginService.AutenticaDocenteForms(dtoDocenteLogado, tempoMaximoLancamento);
		}

		[NonAction]
		public void ReautenticaDocenteComExtensaoDoTempo(DocenteLogadoBindModel docenteLogado)
		{
			//Reautentica o usuario com o Aceite termo OK nos Dados
			var dtoDocenteLogado = new DTODocenteLogado
			{
				AceitouTermoDeAceite = docenteLogado.AceitouTermoDeAceite,
				AlteracaoDeSenhaNecessaria = false,
				Email = docenteLogado.Email,
                Senha = docenteLogado.Senha,
				Matricula = docenteLogado.Matricula,
                IdFuncional = docenteLogado.IdFuncional,
                Vinculo = docenteLogado.Vinculo,
				Nome = docenteLogado.Nome,
				NumeroFuncionario = docenteLogado.NumeroFuncionario
			};

			//Obtem o tempo configurado para o lancamento de notas.
			short tempoMaximoLancamento = 60;
			if (ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"] != null)
			{
				tempoMaximoLancamento = short.Parse(ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"]);
			}

			loginService.AutenticaDocenteForms(dtoDocenteLogado, tempoMaximoLancamento);
		}

    	[Authorize]
		public ActionResult ConfirmaTermoAceite(DocenteLogadoBindModel docenteLogado)
		{
            if(docenteLogado.AceitouTermoDeAceite)
                return RedirectToAction("Apresentacao");

			DTOTermoAceiteExibicao dtoTermo = termoCompromissoService.ObtemTermoNaoAceitoMaisRecentePor(docenteLogado.Matricula);
			if (dtoTermo == null && !docenteLogado.AceitouTermoDeAceite)
			{
				//Redefine os dados de Aceite do termo para verdadeiro
				ReautenticaDocenteAceiteTermo(docenteLogado);

				return RedirectToAction("Apresentacao");
			}				

			var modeloConfirmaTermoAceite = new ConfirmaTermoAceiteViewModel
			                                	{
			                                		NomeDaPagina = Recurso.LoginConfirmaTermoAceite_TituloPagina,
													Ano = dtoTermo.Ano,
													Codigo = dtoTermo.Codigo,
													Arquivo = dtoTermo.Arquivo
			                                	};
			modeloConfirmaTermoAceite.CabecalhoModelo.DocenteLogadoModelo = docenteLogado;
    		modeloConfirmaTermoAceite.CabecalhoModelo.BotaoInicioHabilitado = false;
    		modeloConfirmaTermoAceite.CabecalhoModelo.BotaoAjudaHabilitado = false;
    		modeloConfirmaTermoAceite.CabecalhoModelo.TituloCabecalho = Recurso.LoginConfirmaTermoAceite_TituloPagina;

			return View(modeloConfirmaTermoAceite);
		}

		[HttpPost]
		public ActionResult ConfirmaTermoAceite(DocenteLogadoBindModel docenteLogado, ConfirmaTermoAceiteViewModel confirmaTermoModelo)
		{
			if (ModelState.IsValid)
			{
				try {
				var dtoAceiteInclusao = new DTOTermoAceiteInclusao
				                        	{
				                        		Ano = confirmaTermoModelo.Ano,
				                        		DataAceite = DateTime.Now,
				                        		IdTermo = confirmaTermoModelo.Codigo,
				                        		Matricula = docenteLogado.Matricula,
				                        		IP = Request.UserHostAddress
				                        	};

					
					termoCompromissoService.IncluiAceiteDeTermo(dtoAceiteInclusao);

					//Redefine os dados de Aceite do termo para verdadeiro
					ReautenticaDocenteAceiteTermo(docenteLogado);

					return RedirectToAction("Apresentacao");
				}
				catch(System.Exception excecao)
				{
					return View("ErroInesperado", excecao);
				}
			}
			return View(confirmaTermoModelo);
		}

		[LogadoComTermoAceito]
		public ActionResult Apresentacao(DocenteLogadoBindModel modeloDocenteLogado)
		{
			var modelo = new LoginApresentacaoViewModel(modeloDocenteLogado);
			modelo.TituloDaPagina = Recurso.LoginApresentacao_TituloPagina;
			modelo.CabecalhoModelo.TituloCabecalho = Recurso.LoginApresentacao_TituloPagina;
			modelo.CabecalhoModelo.BotaoInicioHabilitado = false;
			modelo.CabecalhoModelo.BotaoAjudaHabilitado = false;
			return View(modelo);
		}

    	[Authorize]
		public ActionResult Desloga() {
			//Desloga o usuário
			loginService.DesconectaDocenteForms();

			//Redireciona para a tela principal
			return RedirectToAction("Inicial");
		}

        public ActionResult RedefineSenha()
		{
            var modelo = new RedefineSenhaViewModel();

            modelo.CabecalhoModelo.BotaoAjudaHabilitado = false;
            modelo.CabecalhoModelo.BotaoSairHabilitado = false;
            modelo.CabecalhoModelo.BotaoInicioHabilitado = false;
            modelo.CabecalhoModelo.TituloCabecalho = "Esqueci minha Senha";

            return View(modelo);
		}

        public ActionResult PedidoUsuario()
        {
            var modelo = new PedidoUsuarioViewModel();

            modelo.CabecalhoModelo.BotaoAjudaHabilitado = false;
            modelo.CabecalhoModelo.BotaoSairHabilitado = false;
            modelo.CabecalhoModelo.BotaoInicioHabilitado = false;
            modelo.CabecalhoModelo.TituloCabecalho = "Esqueci meu ID Funcional/Vínculo";

            return View(modelo);
        }

        [HttpPost]
        public ActionResult RedefineSenha(RedefineSenhaViewModel modelo)
        {
            var result = loginService.RedefineSenha(modelo.IdFuncional, modelo.Codigo);
            var senhaRedefinidaViewModel = new SenhaRedefinidaViewModel { Mensagem = result };
            return View("SenhaRedefinida", senhaRedefinidaViewModel);
        }

        [HttpPost]
        public ActionResult PedidoUsuario(PedidoUsuarioViewModel modelo)
        {
            var result = loginService.PedidoUsuario(modelo.Cpf, modelo.Codigo);
            var usuarioPedidoViewModel = new UsuarioPedidoViewModel { Mensagem = result };
            return View("UsuarioPedido", usuarioPedidoViewModel);
        }

		public ActionResult ErroGeral()
		{
			return View("ErroGeral");
		}

        [Authorize]
        public ActionResult AlteraSenha(DocenteLogadoBindModel docenteLogado)
        {
            var modelo = new AlteraSenhaViewModel();

            modelo.TituloDaPagina = Recurso.LoginAlteraSenha_TituloPagina;
            //modelo.CabecalhoModelo.DocenteLogadoModelo = docenteLogado;
            modelo.CabecalhoModelo.TituloCabecalho = Recurso.LoginAlteraSenha_TituloPagina;
            modelo.CabecalhoModelo.BotaoInicioHabilitado = false;
            modelo.CabecalhoModelo.BotaoAjudaHabilitado = false;
            modelo.Matricula = docenteLogado.Matricula;
            modelo.IdFuncional = docenteLogado.IdFuncional;
            modelo.Vinculo = docenteLogado.Vinculo;

            return View(modelo);
        }

        [HttpPost]
        public ActionResult AlteraSenha(AlteraSenhaViewModel alteraSenhaModelo)
        {
            ActionResult acaoResultante = null;

            if (ModelState.IsValid)
            {
                try
                {
                    DTODocenteLogado dtoDocenteLogado = loginService.VerificaAlteraSenha(
                        alteraSenhaModelo.Matricula,
                        alteraSenhaModelo.Vinculo ,
                        alteraSenhaModelo.IdFuncional, 
                        alteraSenhaModelo.SenhaAtual,
                        alteraSenhaModelo.SenhaNova,
                        alteraSenhaModelo.SenhaNovaConfirmacao
                    );

                    short tempoMaximoLancamento = 60;
                    if (ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"] != null)
                    {
                        tempoMaximoLancamento = short.Parse(ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"]);
                    }

                    loginService.AutenticaDocenteForms(dtoDocenteLogado, tempoMaximoLancamento);

                    /*
                    !!! GAMBIARRA !!!
                    Feita por Felipe Ribeiro Gomes em 19/05/2022
                    
                    Quem programou a "AlteracaoDeSenhaNecessaria" não previu que
                    o usuário pudesse logar e depois digitar a URL de uma página
                    qualquer no navegador.
                    
                    Por isso, pensei em colocar o valor desse atributo em uma variável
                    que pudesse ser vista por toda a aplicação, para impedir que o
                    usuário possa driblar essa falha de segurança.
                     */

                    StaticMethods.AlteracaoDeSenhaNecessaria = dtoDocenteLogado.AlteracaoDeSenhaNecessaria;

                    /* !!! FIM DA GAMBIARRA !!! */
                    
                    if (!dtoDocenteLogado.AceitouTermoDeAceite)
                    {
                        acaoResultante = RedirectToAction("ConfirmaTermoAceite");
                    }
                    else
                    {
                        return RedirectToAction("Apresentacao");
                    }
                }
                catch (LoginException excecaoDeLogin)
                {
                    ModelState.AddModelError(excecaoDeLogin.TipoDeExcecao, excecaoDeLogin.Message);
                }
                catch (System.Exception excecaoDesconhecida)
                {
                    acaoResultante = View("ErroInesperado", excecaoDesconhecida);
                }
            }

            if (acaoResultante == null)
            {
                var modelo = new AlteraSenhaViewModel();

                modelo.TituloDaPagina = Recurso.LoginAlteraSenha_TituloPagina;
                modelo.CabecalhoModelo.TituloCabecalho = Recurso.LoginAlteraSenha_TituloPagina;
                modelo.CabecalhoModelo.BotaoInicioHabilitado = false;
                modelo.CabecalhoModelo.BotaoAjudaHabilitado = false;
                modelo.Matricula = alteraSenhaModelo.Matricula;
                modelo.IdFuncional = alteraSenhaModelo.IdFuncional;
                modelo.Vinculo = alteraSenhaModelo.Vinculo;

                acaoResultante = View("AlteraSenha", modelo);
            }

            return acaoResultante;
        }
    }
}
