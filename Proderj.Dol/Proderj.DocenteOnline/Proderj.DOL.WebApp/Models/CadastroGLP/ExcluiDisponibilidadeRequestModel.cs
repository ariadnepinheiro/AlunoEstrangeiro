using System.ComponentModel.DataAnnotations;

namespace Proderj.DOL.WebApp.Models
{
	public class ExcluiDisponibilidadeRequestModel
	{
        [Required(ErrorMessage = "Informe a DISPONIBILIDADE.")]
        public int DISPONIBILIDADEGLPID { get; set; }

        [Required(ErrorMessage = "Informe a UNIDADE ESCOLAR.")]
        public string UNIDADE_ENS { get; set; }
	}
}
