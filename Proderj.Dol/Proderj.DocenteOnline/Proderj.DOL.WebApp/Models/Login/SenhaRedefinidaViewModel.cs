using System.ComponentModel.DataAnnotations;

namespace Proderj.DOL.WebApp.Models
{
    public class SenhaRedefinidaViewModel: ViewModelPadrao
    {
        [Required]
        public string Mensagem { get; set; }
    }
}