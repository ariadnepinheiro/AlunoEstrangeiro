using System.Web.Mvc;
using Proderj.DOL.WebApp.Models;
using Resources;
using Proderj.DOL.Service;

namespace Proderj.DOL.WebApp.Controllers
{
    public class AnaliseRendimentoController : ControllerPadrao
    {
        [LogadoComTermoAceito]
        public ActionResult Inicial(DocenteLogadoBindModel modeloDocenteLogado)
        {
            ActionResult viewSaida = null;

            try
            {
                AnaliseRendimentoViewModel modelo = new AnaliseRendimentoViewModel(modeloDocenteLogado);
                modelo.TituloDaPagina = Recurso.AnaliseRendimento_TituloPagina;
                modelo.CabecalhoModelo.TituloCabecalho = Recurso.AnaliseRendimento_TituloPagina;
                modelo.CabecalhoModelo.LinkAjuda = Url.Action("AnaliseRendimento", "Ajuda");

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
