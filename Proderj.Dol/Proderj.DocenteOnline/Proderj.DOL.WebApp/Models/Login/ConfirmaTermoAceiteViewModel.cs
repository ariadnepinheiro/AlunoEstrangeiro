using System.ComponentModel.DataAnnotations;

namespace Proderj.DOL.WebApp.Models
{
	public class ConfirmaTermoAceiteViewModel : ViewModelPadrao
	{
		[Required]
		public bool? AceitouTermo { get; set; }
		
		[Required]
		public short Ano { get; set; }

		[Required]
		public int Codigo { get; set; }

		public string Arquivo { get; set; }
	}
}