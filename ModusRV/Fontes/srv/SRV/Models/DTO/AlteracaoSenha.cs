using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SRV.Models.DTO
{
    public class AlteracaoSenha
    {
        [Display(Name = "Senha atual")]
        [Required(ErrorMessage = "Senha atual obrigatória")]
        public string SenhaAtual { get; set; }

        [Display(Name = "Nova senha")]
        [Required(ErrorMessage = "Nova senha obrigatória")]
        [RegularExpression("(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9]).{7,10}", ErrorMessage = "Senha deve conter letras maiúsculas, minúsculas, números e ter entre 7 e 10 caracteres")]
        public string NovaSenha { get; set; }

        [Display(Name = "Confirmação da senha")]
        [Required(ErrorMessage = "Confirmação obrigatória")]
        [Compare("NovaSenha", ErrorMessage = "Nova senha não confere")]
        public string ConfirmacaoSenha { get; set; }
    }
}