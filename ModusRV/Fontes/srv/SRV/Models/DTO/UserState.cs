using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using SRV.Models.Domain;

namespace SRV.Models.DTO
{
    [Serializable]
    public class UserState
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public bool AlterarSenha { get; set; }
        public Perfil Perfil { get; set; }
        public int Ciclo { get; set; }
        public string IPCliente { get; set; }

        public MenuItem Menu { get; set; }

    }
}