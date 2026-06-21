using System.ComponentModel.DataAnnotations;

namespace Proderj.DOL.WebApp.Models
{
    public class RedefineSenhaViewModel: ViewModelPadrao
    {
        [Required]
        public string IdFuncional { get; set; }

        [Required]
        public string Codigo { get; set; }
    }
}