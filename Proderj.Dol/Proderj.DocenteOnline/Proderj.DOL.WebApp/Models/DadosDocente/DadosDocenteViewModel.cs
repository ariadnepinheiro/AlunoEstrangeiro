using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proderj.DOL.WebApp.Models
{
    public class DadosDocenteViewModel: ViewModelPadrao
    {
        public DadosDocenteViewModel(DocenteLogadoBindModel docenteLogadoModelo)
            : base(docenteLogadoModelo)
        {
            DadosGerais = new DadosGeraisDocenteViewModel(docenteLogadoModelo);
        }

        public DadosGeraisDocenteViewModel DadosGerais { get; set; }
        public List<DadosFormacaoDocenteViewModel> Graduacoes { get; set; }
        public List<DadosFormacaoDocenteViewModel> PosGraduacoes { get; set; }
        public List<DadosCapacitacaoDocenteViewModel> Capacitacoes { get; set; }

        public bool EhPeriodoCampanhaLancamentoNotas { get; set; }
        public bool BloqueiaDadosDocentesEmCampanha { get; set; }

        public string MensagemInicial { get { return Resources.Recurso.DadosDocente_MensagemInicial; } }
    }
}