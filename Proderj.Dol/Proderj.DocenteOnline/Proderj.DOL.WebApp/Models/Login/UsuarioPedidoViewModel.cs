using System.ComponentModel.DataAnnotations;

namespace Proderj.DOL.WebApp.Models
{
    public class UsuarioPedidoViewModel: ViewModelPadrao
    {
        [Required]
        public string Mensagem { get; set; }
    }
}