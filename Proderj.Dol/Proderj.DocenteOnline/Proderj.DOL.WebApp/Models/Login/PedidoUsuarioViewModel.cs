using System.ComponentModel.DataAnnotations;

namespace Proderj.DOL.WebApp.Models
{
    public class PedidoUsuarioViewModel : ViewModelPadrao
    {
        [Required]
        public string Cpf { get; set; }

        [Required]
        public string Codigo { get; set; }
    }
}