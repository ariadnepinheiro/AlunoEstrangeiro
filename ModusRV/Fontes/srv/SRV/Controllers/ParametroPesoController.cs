using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Filters;
using SRV.Models.DTO;
using SRV.Models.Service;
using SRV.Common.Exceptions;
using SRV.Common.Extension;
using SRV.Models.Domain;

namespace SRV.Controllers
{
    public class ParametroPesoController : BaseController
    {
        //
        // GET: /ParametroPeso/

        [CustomAuthorize(Roles = "Administrador, Secretaria")]
        public ActionResult Index()
        {
            CadastroParametroPeso cadastro = new CadastroParametroPeso();

            cadastro = InicializaTelaCadastro(cadastro);

            return View(cadastro);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador, Secretaria")]
        public ActionResult Index(int? idModalidade, int? idTipoUnidadeAdm)
        {
            CadastroParametroPeso cadastro = new CadastroParametroPeso();

            try
            {
                if (idModalidade != null && idTipoUnidadeAdm != null)
                {
                    cadastro.IdModalidade = idModalidade;
                    cadastro.IdTipoUnidadeAdm = idTipoUnidadeAdm;

                    //Busca os valores já existentes
                    ParametroPesoService parametroPesoService = new ParametroPesoService(ModelState);
                    cadastro.Values = parametroPesoService.List(idModalidade.Value, idTipoUnidadeAdm.Value, UsuarioLogado.Ciclo);

                    //Carrega os grupos de função
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
        public ActionResult Edit(int? idModalidade, int? idTipoUnidadeAdm)
        {
            if (idModalidade == null || idTipoUnidadeAdm == null)
                return RedirectToAction("Index");

            CadastroParametroPeso cadastro = new CadastroParametroPeso();

            try
            {
                cadastro.IdModalidade = idModalidade;
                cadastro.IdTipoUnidadeAdm = idTipoUnidadeAdm;

                //Busca os valores já existentes
                ParametroPesoService parametroPesoService = new ParametroPesoService(ModelState);
                cadastro.Values = parametroPesoService.List(idModalidade.Value, idTipoUnidadeAdm.Value, UsuarioLogado.Ciclo);
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
        public ActionResult Edit(CadastroParametroPeso cadastro)
        {
            try
            {
                ParametroPesoService parametroPesoService = new ParametroPesoService(ModelState);

                parametroPesoService.Save(cadastro, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Parâmetros salvos com sucesso");

                    return RedirectToAction("Edit", new { idModalidade = cadastro.IdModalidade.Value, idTipoUnidadeAdm = cadastro.IdTipoUnidadeAdm.Value });
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao salvar parâmetros", ModelState);
            }

            // Recupera os dados informados
            cadastro.Values = new List<ParametroPeso>();

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

        private CadastroParametroPeso InicializaTelaCadastro(CadastroParametroPeso cadastro)
        {
            ModalidadeService modalidadeService = new ModalidadeService(ModelState);
            cadastro.Modalidades = modalidadeService.List().ToSelectList<Modalidade>(o => o.IdModalidade.Value, o => o.DesModalidade);

            TipoUnidadeAdministrativaService tipoUnidadeAdministrativaService = new TipoUnidadeAdministrativaService(ModelState);
            cadastro.TiposUnidadesAdm = tipoUnidadeAdministrativaService.List().ToSelectList<TipoUnidadeAdministrativa>(o => o.IdTipoUnidAdm.Value, o => o.DesTipoUnidAdm);

            return cadastro;
        }

        private CadastroParametroPeso InicializaTelaEdicao(CadastroParametroPeso cadastro)
        {
            ModalidadeService modalidadeService = new ModalidadeService(ModelState);
            cadastro.Modalidades = modalidadeService.List().ToSelectList<Modalidade>(o => o.IdModalidade.Value, o => o.DesModalidade);

            TipoUnidadeAdministrativaService tipoUnidadeAdministrativaService = new TipoUnidadeAdministrativaService(ModelState);
            cadastro.TiposUnidadesAdm = tipoUnidadeAdministrativaService.List().ToSelectList<TipoUnidadeAdministrativa>(o => o.IdTipoUnidAdm.Value, o => o.DesTipoUnidAdm);

            //Carrega os grupos de função
            GrupoFuncaoService grupoFuncaoService = new GrupoFuncaoService(ModelState);
            cadastro.Grupos = grupoFuncaoService.List();

            return cadastro;
        }
    }
}
