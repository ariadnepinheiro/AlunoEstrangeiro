using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Models.DTO;
using SRV.Models.Service;
using SRV.Models.Domain;
using SRV.Filters;
using SRV.Common;
using SRV.Common.Extension;
using SRV.Common.Exceptions;


namespace SRV.Controllers
{
    public class UsuarioController : BaseController
    {
        //
        // GET: /Usuario/

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Index()
        {
            FiltroUsuario filtro = new FiltroUsuario();

            try
            {
                filtro = InicializaTelaFiltro(filtro);
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao carregar tela", ModelState);
            }

            return View(filtro);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Index(FiltroUsuario filtro, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioService usuarioService = new UsuarioService(ModelState);

                    filtro.Usuarios = usuarioService.List(filtro, page ?? 1, Constants.gridPageSize);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha na consulta", ModelState);
            }

            filtro = InicializaTelaFiltro(filtro);

            return View(filtro);
        }


        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            CadastroUsuario cadastro = new CadastroUsuario();
            cadastro.Usuario = new Usuario();

            cadastro = InicializaTelaCadastro(cadastro);
            return View(cadastro);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create(CadastroUsuario cadastro)
        {
            try
            {
                UsuarioService usuarioService = new UsuarioService(ModelState);
                usuarioService.Insert(cadastro.Usuario, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Usuário salvo com sucesso");

                    return RedirectToAction("Edit", new { id = cadastro.Usuario.Id });
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao salvar usuário", ModelState);
            }

            cadastro = InicializaTelaCadastro(cadastro);
            return View(cadastro);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(int id)
        {
            CadastroUsuario cadastro = new CadastroUsuario();

            try
            {
                UsuarioService usuarioService = new UsuarioService(ModelState);
                cadastro.Usuario = usuarioService.Find(id);
                cadastro.PerfilAtual = cadastro.Usuario.Perfil.Value;

                if (cadastro.Usuario.Perfil == Perfil.Regional)
                {
                    UnidadeAdministrativaService unidadeAdministrativaService = new UnidadeAdministrativaService();
                    var listaRegional = unidadeAdministrativaService.ListRegional(2012, UsuarioLogado);
                    cadastro.Regionais = listaRegional.ToSelectList<UnidadeAdministrativa>(o => o.IdUnidadeAdministrativa.Value, o => o.DesUnidadeAdministrativa);
                }
                else if (cadastro.Usuario.Perfil == Perfil.Escola)
                {
                    UnidadeAdministrativaService unidadeAdministrativaService = new UnidadeAdministrativaService();
                    var listaRegional = unidadeAdministrativaService.ListRegional(2012, UsuarioLogado);
                    cadastro.Regionais = listaRegional.ToSelectList<UnidadeAdministrativa>(o => o.IdUnidadeAdministrativa.Value, o => o.DesUnidadeAdministrativa);
                    var listaEscolas = unidadeAdministrativaService.List(2012, cadastro.Usuario.IdRegional.Value, UsuarioLogado);
                    cadastro.UnidadesAdministrativas = listaEscolas.ToSelectList<UnidadeAdministrativa>(o => o.IdUnidadeAdministrativa.Value, o => o.DesUnidadeAdministrativa);
                }

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao carregar tela", ModelState);
            }
            cadastro = InicializaTelaCadastro(cadastro);
            return View(cadastro);
        }


        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(CadastroUsuario cadastro)
        {
            try
            {
                UsuarioService usuarioService = new UsuarioService(ModelState);

                usuarioService.Update(cadastro.Usuario, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Usuário alterado com sucesso");

                    return RedirectToAction("Edit", new { id = cadastro.Usuario.Id });
                }

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao alterar usuário", ModelState);
            }

            cadastro = InicializaTelaCadastro(cadastro);
            return View(cadastro);
        }

        private FiltroUsuario InicializaTelaFiltro(FiltroUsuario filtro)
        {
            filtro.Perfis = Perfil.Vazio.ToSelectList();

            return filtro;
        }

        private CadastroUsuario InicializaTelaCadastro(CadastroUsuario cadastro)
        {

            IList<Perfil> perfis = new List<Perfil>();
            
            if (cadastro.Usuario.Id == null)
            {
                cadastro.Usuario.Ativo = true;

                perfis.Add(Perfil.Administrador);
                perfis.Add(Perfil.Secretaria);
                perfis.Add(Perfil.Regional);
            }
            else if (cadastro.PerfilAtual == Perfil.Administrador || cadastro.PerfilAtual == Perfil.Secretaria)
            {
                perfis.Add(Perfil.Administrador);
                perfis.Add(Perfil.Secretaria);
                perfis.Add(Perfil.Regional);
            }
            else
            {
                perfis.Add(Perfil.Secretaria);
                perfis.Add(Perfil.Servidor);
                perfis.Add(Perfil.Escola);
                perfis.Add(Perfil.Regional);
            }

            cadastro.Perfis = perfis.ToSelectList<Perfil>(o => o.ToString(), o => o.ToString());


            return cadastro;
        }

    }
}
