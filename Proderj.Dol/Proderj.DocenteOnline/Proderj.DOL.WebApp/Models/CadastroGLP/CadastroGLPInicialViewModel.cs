using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proderj.DOL.WebApp.Models
{
	public class CadastroGLPInicialViewModel : ViewModelPadrao
	{
		public CadastroGLPInicialViewModel(DocenteLogadoBindModel modeloDocenteLogado) : base(modeloDocenteLogado)
		{
		
		}

        public DadosPessoaisViewModel DadosPessoais { get; set; }

		public List<Service.DTOCoordenadoria> ListaCoordenadoria { get; set; }

		public List<Service.DTOCadastroGlp_Disciplina> ListaDisciplina { get; set; }

		public List<IDTOItemCombo> ListaDiaSemana { get; set; }

		public string NumeroTelefoneDocente { get; set; }

        public List<Service.DTORegional> ListaRegional { get; set; }
	}
}