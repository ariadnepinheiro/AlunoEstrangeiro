using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace SRV.Models.DTO
{
    public class CadastroUsuario
    {
        public IEnumerable Regionais { get; set; }

        public IEnumerable UnidadesAdministrativas { get; set; }

        public Usuario Usuario { get; set; }

        [Display(Name = "Perfil")]
        public IEnumerable Perfis { get; set; }

        public Perfil PerfilAtual { get; set; }
    }
}