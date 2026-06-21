using System.Web.Mvc;
using Proderj.DOL.WebApp.Models;
using Resources;
using Proderj.DOL.Service;

namespace Proderj.DOL.WebApp.Controllers
{
    public class CodigoArmazemDoLivroController : ControllerPadrao
    {
        private readonly IARMAZEM_LIVRO_2019Service armazemServico;

        public CodigoArmazemDoLivroController(IARMAZEM_LIVRO_2019Service armazemServico)
        {
            this.armazemServico = armazemServico;
        }

        [LogadoComTermoAceito]
        public ActionResult Inicial(DocenteLogadoBindModel modeloDocenteLogado)
        {
            ActionResult viewSaida = null;

            try
            {
                CodigoArmazemDoLivroViewModel modelo = new CodigoArmazemDoLivroViewModel(modeloDocenteLogado);
                modelo.TituloDaPagina = Recurso.CodigoArmazemDoLivroInicial_TituloPagina;
                modelo.CabecalhoModelo.TituloCabecalho = Recurso.CodigoArmazemDoLivroInicial_TituloPagina;
                modelo.CabecalhoModelo.LinkAjuda = Url.Action("CodigoArmazemDoLivro", "Ajuda");
                modelo.Codigo = armazemServico.ObtemCodigoPor(modeloDocenteLogado.Matricula);

                viewSaida = View(modelo);
            }
            catch (System.Exception excecao)
            {
                viewSaida = View("ErroInesperado", excecao);
            }

            return viewSaida;
        }
    }
}
