using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using SRV.Common.Logging;

namespace SRV.Models.Domain
{
    public enum Perfil
    {
        [DescriptionAttribute("")]
        [Display(Name = " ")]
        Vazio = 0,

        [DescriptionAttribute("Administrador")]
        [Display(Name = "Administrador")]
        Administrador = 1,

        [DescriptionAttribute("Secretaria")]
        [Display(Name = "Secretaria")]
        Secretaria = 2,

        [DescriptionAttribute("Regional")]
        [Display(Name = "Regional")]
        Regional = 3,

        [DescriptionAttribute("Escola")]
        [Display(Name = "Escola")]
        Escola = 4,
        
        [DescriptionAttribute("Servidor")]
        [Display(Name = "Servidor")]
        Servidor = 5
    }

    public class Usuario
    {
        [PrimaryKey]
        [Display(Name = "Código Usuário")]
        public int? Id { get; set; }

        [Display(Name = "Login / Matrícula")]
        [StringLength(50, ErrorMessage = "Campo não pode ter mais de {1} caracteres")]
        public string Login { get; set; }

        [Display(Name = "Nome")]
        [StringLength(70, ErrorMessage = "Campo não pode ter mais de {1} caracteres")]
        public string Nome { get; set; }

        [Display(Name = "E-mail")]
        [RegularExpression("^[A-Za-z0-9_\\-\\.]+@[A-Za-z0-9_\\-\\.]{2,}\\.[A-Za-z0-9]{2,}(\\.[A-Za-z0-9])?$", ErrorMessage = "Email Inválido")]
        [StringLength(50, ErrorMessage = "Campo não pode ter mais de {1} caracteres")]
        public string EmailUsuario { get; set; }

        [Display(Name = "CPF")]
        public string CPF { get; set; }

        [Display(Name = "Perfil")]
        public Perfil? Perfil { get; set; }

        public bool AlterarSenha { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; }

        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [Display(Name = "Unidade Administrativa")]
        public int? IdUnidadeAdministrativa { get; set; }

        [Display(Name = "Regional")]
        public int? IdRegional { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            if (((Usuario)obj).Id == this.Id &&
                ((Usuario)obj).Login == this.Login &&
                ((Usuario)obj).Nome == this.Nome &&
                ((Usuario)obj).EmailUsuario == this.EmailUsuario &&
                ((Usuario)obj).CPF == this.CPF &&
                ((Usuario)obj).Perfil == this.Perfil &&
                ((Usuario)obj).AlterarSenha == this.AlterarSenha &&
                ((Usuario)obj).Ativo == this.Ativo &&
                ((Usuario)obj).Senha == this.Senha)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return Id == null ? 0 : Id.GetHashCode();
        }
    }
}