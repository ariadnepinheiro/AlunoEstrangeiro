using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;

namespace SRV.Models.DTO
{
    public class ParametroNotaItem
    {
        public ParametroNota ParametroNota { get; set; }

        /// <summary>
        /// Controle para saber se o registro já existia na tabela de parâmetro
        /// </summary>
        public bool Existia { get; set; }
    }
}