using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Filters;
using SRV.Models.DTO;
using SRV.Common.Exceptions;
using SRV.Common.Extension;
using SRV.Models.Service;
using SRV.Models.Domain;

namespace SRV.Controllers
{
    public class ParametroCurvaController : BaseController
    {
        //
        // GET: /ParametroCurva/

        [CustomAuthorize(Roles = "Administrador, Secretaria")]
        public ActionResult Index()
        {
            CadastroParametroCurva cadastro = new CadastroParametroCurva();

            cadastro = InicializaTelaCadastro(cadastro);

            return View(cadastro);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador, Secretaria")]
        public ActionResult Index(int? idTipoUnidadeAdm)
        {
            CadastroParametroCurva cadastro = new CadastroParametroCurva();

            try
            {
                if (idTipoUnidadeAdm != null)
                {
                    cadastro.IdTipoUnidadeAdm = idTipoUnidadeAdm;

                    //Busca os valores já existentes
                    ParametroCurvaService parametroCurvaService = new ParametroCurvaService(ModelState);
                    
                    cadastro.Values = new List<ParametroCurvaItem>();

                    // Converte os valores de "ParametroCurva" para "ParametroCurvaItem"
                    foreach(ParametroCurva item in parametroCurvaService.List(idTipoUnidadeAdm.Value, UsuarioLogado.Ciclo))
                    {
                        cadastro.Values.Add(new ParametroCurvaItem() { ParametroCurva = item, Existia = true });
                    }

                    //Carrega todas as notas
                    NotaService notaService = new NotaService(ModelState);
                    cadastro.Notas = notaService.List(UsuarioLogado.Ciclo);

                    //Carrega todos os grupos de função
                    GrupoFuncaoService grupoFuncaoService = new GrupoFuncaoService(ModelState);
                    cadastro.Grupos = grupoFuncaoService.List();
                }

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha na consulta", ModelState);
            }

            cadastro = InicializaTelaCadastro(cadastro);

            return View(cadastro);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(int idTipoUnidadeAdm)
        {
            CadastroParametroCurva cadastro = new CadastroParametroCurva();

            try
            {
                cadastro.IdTipoUnidadeAdm = idTipoUnidadeAdm;

                //Busca os valores já existentes
                ParametroCurvaService parametroCurvaService = new ParametroCurvaService(ModelState);

                cadastro.Values = new List<ParametroCurvaItem>();

                // Converte os valores de "ParametroCurva" para "ParametroCurvaItem"
                foreach(ParametroCurva item in parametroCurvaService.List(idTipoUnidadeAdm, UsuarioLogado.Ciclo))
                {
                    cadastro.Values.Add(new ParametroCurvaItem() { ParametroCurva = item, Existia = true });
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha na consulta", ModelState);
            }

            cadastro = InicializaTelaEdicao(cadastro);

            return View("Index", cadastro);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(CadastroParametroCurva cadastro)
        {
            try
            {
                ParametroCurvaService parametroCurvaService = new ParametroCurvaService(ModelState);

                parametroCurvaService.Save(cadastro.NewValues, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Parâmetros salvos com sucesso");

                    return RedirectToAction("Edit", new { idTipoUnidadeAdm = cadastro.IdTipoUnidadeAdm.Value });
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao salvar parâmetros", ModelState);
            }

            // Recupera os dados informados
            cadastro.Values = new List<ParametroCurvaItem>();

            for (short i = 0; i < cadastro.NewValues.Length; i++)
            {
                for (short j = 0; j < cadastro.NewValues[i].Length; j++)
                {
                    cadastro.Values.Add(cadastro.NewValues[i][j]);
                }
            }

            cadastro = InicializaTelaEdicao(cadastro);

            return View("Index", cadastro);
        }

        private CadastroParametroCurva InicializaTelaCadastro(CadastroParametroCurva cadastro)
        {
            TipoUnidadeAdministrativaService tipoUnidadeAdministrativaService = new TipoUnidadeAdministrativaService(ModelState);
            cadastro.TiposUnidadesAdm = tipoUnidadeAdministrativaService.List().ToSelectList<TipoUnidadeAdministrativa>(o => o.IdTipoUnidAdm.Value, o => o.DesTipoUnidAdm);

            return cadastro;
        }

        private CadastroParametroCurva InicializaTelaEdicao(CadastroParametroCurva cadastro)
        {
            TipoUnidadeAdministrativaService tipoUnidadeAdministrativaService = new TipoUnidadeAdministrativaService(ModelState);
            cadastro.TiposUnidadesAdm = tipoUnidadeAdministrativaService.List().ToSelectList<TipoUnidadeAdministrativa>(o => o.IdTipoUnidAdm.Value, o => o.DesTipoUnidAdm);

            //Carrega todas as notas
            NotaService notaService = new NotaService(ModelState);
            cadastro.Notas = notaService.List(UsuarioLogado.Ciclo);

            //Carrega todos os grupos de função
            GrupoFuncaoService grupoFuncaoService = new GrupoFuncaoService(ModelState);
            cadastro.Grupos = grupoFuncaoService.List();

            return cadastro;
        }
    }
}
