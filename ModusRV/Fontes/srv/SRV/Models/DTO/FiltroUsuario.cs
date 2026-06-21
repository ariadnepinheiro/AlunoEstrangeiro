using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using SRV.Models.Domain;

namespace SRV.Models.DTO
{
    public class FiltroUsuario
    {
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "Matrícula")]
        public string Matricula { get; set; }

        [Display(Name = "Perfil")]
        public IEnumerable Perfis { get; set; }
        public Perfil Perfil { get; set; }

        public Paging<Usuario> Usuarios { get; set; }
    }
}