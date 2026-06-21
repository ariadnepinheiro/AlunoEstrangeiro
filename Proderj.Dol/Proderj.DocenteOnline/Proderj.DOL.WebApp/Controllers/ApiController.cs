using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Proderj.DOL.WebApp.Models;
using Resources;
using AutoMapper;
using Proderj.DOL.Service;
using Proderj.DOL.Exception;
using Proderj.Foundation.Common;
using System.Text;
using Proderj.DOL.Domain;
using System.Configuration;

namespace Proderj.DOL.WebApp.Controllers

{
    public class ApiController : ControllerPadrao
    {
        private readonly IApiService ApiServico;
        private readonly ILoginService loginService;

        public ApiController(IApiService ApiServico,
                             ILoginService loginService
            )
        {
            this.ApiServico = ApiServico;
            this.loginService = loginService;
        }

        [HttpPost]
        public JsonResult ReceberDados(string sistemas, string chaveCriptografada)
        {
            if (string.IsNullOrEmpty(sistemas) || string.IsNullOrEmpty(chaveCriptografada))
            {
                return Json(new { mensagem = "Parâmetros inválidos." });
            }

            // Processar os dados conforme necessário
            return Json(new { mensagem = "Dados recebidos com sucesso." });
        }

        [HttpGet]
        public ActionResult LancamentoFrequencia(DocenteLogadoBindModel modeloDocenteLogado)
        {
            var caminhoDol = System.Configuration.ConfigurationManager.AppSettings["caminhoDol"];
            var duracaoDoTokenEmSegundos = int.Parse(System.Configuration.ConfigurationManager.AppSettings["duracaoDoTokenEmSegundos"]);

            String crp = this.ApiServico.EnviaUsuario(modeloDocenteLogado.IdFuncional+"/"+modeloDocenteLogado.Vinculo,modeloDocenteLogado.Senha, duracaoDoTokenEmSegundos);

            return Content("<form action='" + caminhoDol + "/Token' id='theForm' method='post'><input type='hidden' name='crp' value='" + crp + "' /><input type='hidden' name='returnUrl' value='/LancamentoFrequencia' /></form><script>document.getElementById('theForm').submit();</script>");

            //return Redirect(caminhoDol+"/Api/LancamentoFrequencia?crp=" + crp);
        }

        public ActionResult LancamentoNotas(DocenteLogadoBindModel modeloDocenteLogado)
        {
            var caminhoDol = System.Configuration.ConfigurationManager.AppSettings["caminhoDol"];
            var duracaoDoTokenEmSegundos = int.Parse(System.Configuration.ConfigurationManager.AppSettings["duracaoDoTokenEmSegundos"]);

            String crp = this.ApiServico.EnviaUsuario(modeloDocenteLogado.IdFuncional + "/" + modeloDocenteLogado.Vinculo, modeloDocenteLogado.Senha, duracaoDoTokenEmSegundos);

            return Content("<form action='" + caminhoDol + "/Token' id='theForm' method='post'><input type='hidden' name='crp' value='" + crp + "' /><input type='hidden' name='returnUrl' value='/LancamentoNotas' /></form><script>document.getElementById('theForm').submit();</script>");
        }

        [HttpGet]
        public ActionResult CadastroGLP(DocenteLogadoBindModel modeloDocenteLogado)
        {
            var caminhoDol = ConfigurationManager.AppSettings["caminhoDol"];
            var duracaoDoTokenEmSegundos = int.Parse(ConfigurationManager.AppSettings["duracaoDoTokenEmSegundos"]);

            string crp = ApiServico.EnviaUsuario(modeloDocenteLogado.IdFuncional + "/" + modeloDocenteLogado.Vinculo,modeloDocenteLogado.Senha,duracaoDoTokenEmSegundos);

            return Content(
                "<form action='" + caminhoDol + "/Token' method='post' id='theForm'>" + "<input type='hidden' name='crp' value='" + crp + "' />" + "<input type='hidden' name='returnUrl' value='/CadastroGLP' />" + "</form>" + "<script>document.getElementById('theForm').submit();</script>"
            );
        }

        [HttpGet]
        public ActionResult ProtocoloNota(DocenteLogadoBindModel modeloDocenteLogado)
        {
            var caminhoDol = ConfigurationManager.AppSettings["caminhoDol"];
            var duracaoDoTokenEmSegundos = int.Parse(ConfigurationManager.AppSettings["duracaoDoTokenEmSegundos"]);

            string crp = ApiServico.EnviaUsuario(modeloDocenteLogado.IdFuncional + "/" + modeloDocenteLogado.Vinculo, modeloDocenteLogado.Senha, duracaoDoTokenEmSegundos);

            return Content(
                "<form action='" + caminhoDol + "/Token' method='post' id='theForm'>" + "<input type='hidden' name='crp' value='" + crp + "' />" + "<input type='hidden' name='returnUrl' value='/ProtocoloNota' />" + "</form>" + "<script>document.getElementById('theForm').submit();</script>"
            );
        }

        [HttpGet]
        public ActionResult DadosDocente(DocenteLogadoBindModel modeloDocenteLogado)
        {
            var caminhoDol = ConfigurationManager.AppSettings["caminhoDol"];
            var duracaoDoTokenEmSegundos = int.Parse(ConfigurationManager.AppSettings["duracaoDoTokenEmSegundos"]);

            string crp = ApiServico.EnviaUsuario(modeloDocenteLogado.IdFuncional + "/" + modeloDocenteLogado.Vinculo, modeloDocenteLogado.Senha, duracaoDoTokenEmSegundos);

            return Content(
                "<form action='" + caminhoDol + "/Token' method='post' id='theForm'>" + "<input type='hidden' name='crp' value='" + crp + "' />" + "<input type='hidden' name='returnUrl' value='/DadosDocente' />" + "</form>" + "<script>document.getElementById('theForm').submit();</script>"
            );
        }

        [HttpGet]
        public ActionResult DadosPessoais(DocenteLogadoBindModel modeloDocenteLogado)
        {
            var caminhoDol = ConfigurationManager.AppSettings["caminhoDol"];
            var duracaoDoTokenEmSegundos = int.Parse(ConfigurationManager.AppSettings["duracaoDoTokenEmSegundos"]);

            string crp = ApiServico.EnviaUsuario(modeloDocenteLogado.IdFuncional + "/" + modeloDocenteLogado.Vinculo, modeloDocenteLogado.Senha, duracaoDoTokenEmSegundos);

            return Content(
                "<form action='" + caminhoDol + "/Token' method='post' id='theForm'>" + "<input type='hidden' name='crp' value='" + crp + "' />" + "<input type='hidden' name='returnUrl' value='/DadosPessoais' />" + "</form>" + "<script>document.getElementById('theForm').submit();</script>"
            );
        }

        [HttpGet]
        public ActionResult Relatorios(DocenteLogadoBindModel modeloDocenteLogado)
        {
            var caminhoDol = ConfigurationManager.AppSettings["caminhoDol"];
            var duracaoDoTokenEmSegundos = int.Parse(ConfigurationManager.AppSettings["duracaoDoTokenEmSegundos"]);

            string crp = ApiServico.EnviaUsuario(modeloDocenteLogado.IdFuncional + "/" + modeloDocenteLogado.Vinculo, modeloDocenteLogado.Senha, duracaoDoTokenEmSegundos);

            return Content(
                "<form action='" + caminhoDol + "/Token' method='post' id='theForm'>" + "<input type='hidden' name='crp' value='" + crp + "' />" + "<input type='hidden' name='returnUrl' value='/Relatorio?relatorio=chdocenteonline&grupo=dol' />" + "</form>" + "<script>document.getElementById('theForm').submit();</script>"
            );
        }

        [HttpGet]
        public ActionResult AlteraSenha(DocenteLogadoBindModel modeloDocenteLogado)
        {
            var caminhoDol = ConfigurationManager.AppSettings["caminhoDol"];
            var duracaoDoTokenEmSegundos = int.Parse(ConfigurationManager.AppSettings["duracaoDoTokenEmSegundos"]);

            string crp = ApiServico.EnviaUsuario(modeloDocenteLogado.IdFuncional + "/" + modeloDocenteLogado.Vinculo, modeloDocenteLogado.Senha, duracaoDoTokenEmSegundos);

            return Content(
                "<form action='" + caminhoDol + "/Token' method='post' id='theForm'>" + "<input type='hidden' name='crp' value='" + crp + "' />" + "<input type='hidden' name='returnUrl' value='/AlteraSenha' />" + "</form>" + "<script>document.getElementById('theForm').submit();</script>"
            );
        }

        [HttpGet]
        public ActionResult GetDadosDocente(string crp)
        {
            if (string.IsNullOrEmpty(crp))
            {
                return Json(new { mensagem = "Parâmetros inválidos." });
            }
            short tempoMaximoLancamento = 60;
            DTODocenteLogado dtoDocenteLogado = this.ApiServico.ValidaUsuario(crp);

            if (ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"] != null)
            {
                tempoMaximoLancamento = short.Parse(ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"]);
            }

            loginService.AutenticaDocenteForms(dtoDocenteLogado, tempoMaximoLancamento);

            return RedirectToAction("Inicial", "DadosDocente", new { nomeController = "DadosDocente" });
        }

    
        [HttpGet]
        public ActionResult GetLancamentoNotas(string crp)
        {
            if (string.IsNullOrEmpty(crp))
            {
                return Json(new { mensagem = "Parâmetros inválidos." });
            }
            short tempoMaximoLancamento = 60;
            DTODocenteLogado dtoDocenteLogado = this.ApiServico.ValidaUsuario(crp);

            if (ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"] != null)
            {
                tempoMaximoLancamento = short.Parse(ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"]);
            }

            loginService.AutenticaDocenteForms(dtoDocenteLogado, tempoMaximoLancamento);

            return RedirectToAction("Lista", "SelecaoTurmas", new { nomeController = "LancamentoNotas" });
        }

        [HttpGet]
        public ActionResult GetCadastroGLP(string crp)
        {
            if (string.IsNullOrEmpty(crp))
            {
                return Json(new { mensagem = "Parâmetros inválidos." });
            }
            short tempoMaximoLancamento = 60;
            DTODocenteLogado dtoDocenteLogado = this.ApiServico.ValidaUsuario(crp);

            if (ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"] != null)
            {
                tempoMaximoLancamento = short.Parse(ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"]);
            }

            loginService.AutenticaDocenteForms(dtoDocenteLogado, tempoMaximoLancamento);

            return RedirectToAction("Inicial", "CadastroGLP", new { nomeController = "CadastroGLP" });
        }

        [HttpGet]
        public ActionResult GetRelatorios(string crp)
        {
            if (string.IsNullOrEmpty(crp))
            {
                return Json(new { mensagem = "Parâmetros inválidos." });
            }
            short tempoMaximoLancamento = 60;
            DTODocenteLogado dtoDocenteLogado = this.ApiServico.ValidaUsuario(crp);

            if (ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"] != null)
            {
                tempoMaximoLancamento = short.Parse(ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"]);
            }

            loginService.AutenticaDocenteForms(dtoDocenteLogado, tempoMaximoLancamento);

            string url = "http://localhost:60444/Relatorios.aspx?relatorio=chdocenteonline&grupo=dol";
            string script = "<script>window.open('" + url + "', '', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes');window.close();</script>";
            return Content(script, "text/html");

            //return RedirectToAction("Lista", "SelecaoTurmas", new { nomeController = "RespostaCurriculoMinimo" });
        }
        [HttpGet]
        public ActionResult GetProtocoloNota(string crp)
        {
            if (string.IsNullOrEmpty(crp))
            {
                return Json(new { mensagem = "Parâmetros inválidos." });
            }
            short tempoMaximoLancamento = 60;
            DTODocenteLogado dtoDocenteLogado = this.ApiServico.ValidaUsuario(crp);

            if (ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"] != null)
            {
                tempoMaximoLancamento = short.Parse(ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"]);
            }

            loginService.AutenticaDocenteForms(dtoDocenteLogado, tempoMaximoLancamento);

            return RedirectToAction("Lista", "ProtocoloNota", new { nomeController = "ProtocoloNota" });
        }
        [HttpGet]
        public ActionResult GetDadosPessoais(string crp)
        {
            if (string.IsNullOrEmpty(crp))
            {
                return Json(new { mensagem = "Parâmetros inválidos." });
            }
            short tempoMaximoLancamento = 60;
            DTODocenteLogado dtoDocenteLogado = this.ApiServico.ValidaUsuario(crp);

            if (ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"] != null)
            {
                tempoMaximoLancamento = short.Parse(ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"]);
            }

            loginService.AutenticaDocenteForms(dtoDocenteLogado, tempoMaximoLancamento);

            return RedirectToAction("Inicial", "DadosPessoais", new { nomeController = "DadosPessoais" });
        }
        [HttpGet]
        public ActionResult GetResultadoAvaliacao(string crp)
        {
            if (string.IsNullOrEmpty(crp))
            {
                return Json(new { mensagem = "Parâmetros inválidos." });
            }
            short tempoMaximoLancamento = 60;
            DTODocenteLogado dtoDocenteLogado = this.ApiServico.ValidaUsuario(crp);

            if (ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"] != null)
            {
                tempoMaximoLancamento = short.Parse(ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"]);
            }

            loginService.AutenticaDocenteForms(dtoDocenteLogado, tempoMaximoLancamento);

            return RedirectToAction("Inicial", "ResultadoAvaliacao", new { nomeController = "ResultadoAvaliacao" });
        }
        [HttpGet]
        public ActionResult GetAnaliseRendimento(string crp)
        {
            if (string.IsNullOrEmpty(crp))
            {
                return Json(new { mensagem = "Parâmetros inválidos." });
            }
            short tempoMaximoLancamento = 60;
            DTODocenteLogado dtoDocenteLogado = this.ApiServico.ValidaUsuario(crp);

            if (ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"] != null)
            {
                tempoMaximoLancamento = short.Parse(ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"]);
            }

            loginService.AutenticaDocenteForms(dtoDocenteLogado, tempoMaximoLancamento);

            return RedirectToAction("Inicial", "AnaliseRendimento", new { nomeController = "AnaliseRendimento" });
        }

    }

    public class DadosRequest
    {
        public string Sistemas { get; set; }
        public string ChaveCriptografada { get; set; }
    }
}

