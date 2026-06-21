using System;
using System.Collections.Generic;
using Proderj.DOL.Service;
using Resources;

namespace Proderj.DOL.WebApp.Models
{
	public class SelecaoTurmasListaViewModel : ViewModelPadrao
	{
		public SelecaoTurmasListaViewModel(DocenteLogadoBindModel docenteLogadoModelo) 
			: base(docenteLogadoModelo)
		{ }

		public string CodigoTurmaErro { get; set; }
		public string MensagemInicialTela { get; set; }
		public List<DTOSelecaoTurmas> ListaSelecaoTurma { get; set; }
		public string AcaoPost { get; set; }
		public string MensagemSumario { 
			get
			{
                return String.Format(Recurso.LancamentoNotasLista_MensagemSumario, CodigoTurmaErro);
			} 
		}
	}
}