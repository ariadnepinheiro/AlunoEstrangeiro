using System.Web.Mvc;
using Proderj.DOL.WebApp.Models;
using Resources;
using Proderj.DOL.Service;

namespace Proderj.DOL.WebApp.Controllers
{
    public class ResultadoAvaliacaoController : ControllerPadrao
    {
        [LogadoComTermoAceito]
        public ActionResult Inicial(DocenteLogadoBindModel modeloDocenteLogado)
        {
            ActionResult viewSaida = null;

            try
            {
                ResultadoAvaliacaoViewModel modelo = new ResultadoAvaliacaoViewModel(modeloDocenteLogado);
                modelo.TituloDaPagina = Recurso.ResultadoAvaliacao_TituloPagina;
                modelo.CabecalhoModelo.TituloCabecalho = Recurso.ResultadoAvaliacao_TituloPagina;
                modelo.CabecalhoModelo.LinkAjuda = Url.Action("ResultadoAvaliacao", "Ajuda");

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
