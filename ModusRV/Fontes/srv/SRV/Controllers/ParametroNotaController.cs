using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Models.DTO;
using SRV.Models.Service;
using SRV.Common.Extension;
using SRV.Filters;
using SRV.Models.Domain;
using SRV.Common.Exceptions;

namespace SRV.Controllers
{
    public class ParametroNotaController : BaseController
    {
        //
        // GET: /ParametroNota/

        [CustomAuthorize(Roles = "Administrador, Secretaria")]
        public ActionResult Index()
        {
            CadastroParametroNota cadastro = new CadastroParametroNota();

            cadastro = InicializaTelaCadastro(cadastro);

            return View(cadastro);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador, Secretaria")]
        public ActionResult Index(int? idModalidade)
        {
            CadastroParametroNota cadastro = new CadastroParametroNota();

            try
            {
                if (idModalidade != null)
                {
                    cadastro.IdModalidade = idModalidade;

                    //Busca os valores já existentes
                    ParametroNotaService parametroNotaService = new ParametroNotaService(ModelState);

                    cadastro.Values = new List<ParametroNotaItem>();

                    // Converte os valores de "ParametroCurva" para "ParametroCurvaItem"
                    foreach (ParametroNota item in parametroNotaService.List(idModalidade.Value, UsuarioLogado.Ciclo))
                    {
                        cadastro.Values.Add(new ParametroNotaItem() { ParametroNota = item, Existia = true });
                    }

                    //Carrega todas as notas
                    NotaService notaService = new NotaService(ModelState);
                    cadastro.Notas = notaService.List(UsuarioLogado.Ciclo);

                    //Carrega os indicadores do tipo elegibilidade
                    IndicadorService indicadorService = new IndicadorService(ModelState);
                    cadastro.Indicadores = indicadorService.ListByTipo(TipoIndicador.Elegibilidade);
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
        public ActionResult Edit(int idModalidade)
        {
            CadastroParametroNota cadastro = new CadastroParametroNota();

            try
            {
                cadastro.IdModalidade = idModalidade;

                //Busca os valores já existentes
                ParametroNotaService parametroNotaService = new ParametroNotaService(ModelState);
                
                cadastro.Values = new List<ParametroNotaItem>();

                // Converte os valores de "ParametroCurva" para "ParametroCurvaItem"
                foreach (ParametroNota item in parametroNotaService.List(idModalidade, UsuarioLogado.Ciclo))
                {
                    cadastro.Values.Add(new ParametroNotaItem() { ParametroNota = item, Existia = true });
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
        public ActionResult Edit(CadastroParametroNota cadastro)
        {
            try
            {
                ParametroNotaService parametroNotaService = new ParametroNotaService(ModelState);

                parametroNotaService.Save(cadastro.NewValues, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Parâmetros salvos com sucesso");

                    return RedirectToAction("Edit", new { idModalidade = cadastro.IdModalidade.Value });
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao salvar parâmetros", ModelState);
            }

            // Recupera os dados informados
            cadastro.Values = new List<ParametroNotaItem>();

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

        private CadastroParametroNota InicializaTelaCadastro(CadastroParametroNota cadastro)
        {
            ModalidadeService modalidadeService = new ModalidadeService(ModelState);
            cadastro.Modalidades = modalidadeService.List().ToSelectList<Modalidade>(o => o.IdModalidade.Value, o => o.DesModalidade);

            return cadastro;
        }

        private CadastroParametroNota InicializaTelaEdicao(CadastroParametroNota cadastro)
        {
            ModalidadeService modalidadeService = new ModalidadeService(ModelState);
            cadastro.Modalidades = modalidadeService.List().ToSelectList<Modalidade>(o => o.IdModalidade.Value, o => o.DesModalidade);

            //Carrega todas as notas
            NotaService notaService = new NotaService(ModelState);
            cadastro.Notas = notaService.List(UsuarioLogado.Ciclo);

            //Carrega os indicadores do tipo elegibilidade
            IndicadorService indicadorService = new IndicadorService(ModelState);
            cadastro.Indicadores = indicadorService.ListByTipo(TipoIndicador.Elegibilidade);

            return cadastro;
        }
    }
}
