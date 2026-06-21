using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proderj.DOL.WebApp.Models
{
	public class DadosPessoaisViewModel : ViewModelPadrao
	{
        public List<Service.DTOCoordenadoria> ListaCoordenadoria { get; set; }

		public List<Service.DTOCadastroGlp_Disciplina> ListaDisciplina { get; set; }

		public List<IDTOItemCombo> ListaDiaSemana { get; set; }

		public string NumeroTelefoneDocente { get; set; }

        public List<Service.DTORegional> ListaRegional { get; set; }

        public List<Service.DTOHD_PAIS> ListaPais { get; set; }

        public IEnumerable<SelectListItem> DropDownListaPais { get; set; }
	}
}