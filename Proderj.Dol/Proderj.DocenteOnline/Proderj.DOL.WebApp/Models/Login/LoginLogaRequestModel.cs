using System.ComponentModel.DataAnnotations;
using Proderj.Foundation.Framework.Web;
using Resources;

namespace Proderj.DOL.WebApp.Models
{
	public class LoginLogaRequestModel : ViewModelPadrao
	{
		[Required]
		public string IdFuncional { get; set; }

		[Required]
		public string Senha { get; set; }

        [Required]
        public string Codigo { get; set; }        
	}
}
