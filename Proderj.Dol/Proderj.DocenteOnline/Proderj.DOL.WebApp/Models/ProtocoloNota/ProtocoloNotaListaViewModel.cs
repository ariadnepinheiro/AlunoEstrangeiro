using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Proderj.DOL.Service;

namespace Proderj.DOL.WebApp.Models.ProtocoloNota
{
	public class ProtocoloNotaListaViewModel : ViewModelPadrao
    {
        public ProtocoloNotaListaViewModel() { }
		public ProtocoloNotaListaViewModel(DocenteLogadoBindModel modeloDocenteLogado) : base(modeloDocenteLogado) { }

		public  List<DTOProtocoloNotaComData> ListaProtocolo { get; set; }
		public  int RegistrosPorPagina { get; set; }
        public List<DTOPeriodoLetivo> ListaAno { get; set; }

	}
}