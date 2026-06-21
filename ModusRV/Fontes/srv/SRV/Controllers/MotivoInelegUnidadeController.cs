using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Models.Service;
using SRV.Models.DTO;

namespace SRV.Controllers
{
    public class MotivoInelegUnidadeController : BaseController
    {
        //
        // GET: /MotivoInelegibilidade/

        public ActionResult Index(int idUnidade, int idAnoReferencia)
        {
            MotivoInelegUnidadeService motivoInelegService = new MotivoInelegUnidadeService();

            MotivoInelegUnidade motivo = motivoInelegService.Find(idUnidade, idAnoReferencia);

            return View(motivo);
        }

    }
}
