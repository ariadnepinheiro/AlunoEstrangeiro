using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proderj.DOL.WebApp.Models
{
    public class CodigoArmazemDoLivroViewModel: ViewModelPadrao
    {
        public CodigoArmazemDoLivroViewModel(DocenteLogadoBindModel docenteLogadoModelo)
            : base(docenteLogadoModelo)
        { }

        public string Codigo { get; set; }
    }
}