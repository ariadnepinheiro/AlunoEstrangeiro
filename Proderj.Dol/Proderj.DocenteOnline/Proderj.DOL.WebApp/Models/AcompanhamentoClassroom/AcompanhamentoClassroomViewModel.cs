using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proderj.DOL.WebApp.Models
{
    public class AcompanhamentoClassroomViewModel: ViewModelPadrao
    {
        public AcompanhamentoClassroomViewModel(DocenteLogadoBindModel docenteLogadoModelo) : base(docenteLogadoModelo)
        {
            DadosGerais = new DadosGeraisDocenteViewModel(docenteLogadoModelo);
            Turmas = new HashSet<DadosTurmaDocenteViewModel>();
            UltimosAcessos = new HashSet<DateTime>();
        }

        public DadosGeraisDocenteViewModel DadosGerais { get; set; }

        public ICollection<DadosTurmaDocenteViewModel> Turmas { get; set; }
        public ICollection<DateTime> UltimosAcessos { get; set; }
    }
}