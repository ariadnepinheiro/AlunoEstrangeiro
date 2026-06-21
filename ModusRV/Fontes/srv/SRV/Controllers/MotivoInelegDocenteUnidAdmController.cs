using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Models.Service;
using SRV.Models.DTO;

namespace SRV.Controllers
{
    public class MotivoInelegDocenteUnidAdmController : BaseController
    {
        public ActionResult Index(int idUnidade, int idAnoReferencia, int? idServidor)
        {
            MotivoInelegDocenteUnidAdmService motivoInelegUnidAdmService = new MotivoInelegDocenteUnidAdmService();

            MotivoInelegDocenteUnidAdm motivo = motivoInelegUnidAdmService.Find(idUnidade, idAnoReferencia, idServidor);

            return View(motivo);
        }
    }
}
