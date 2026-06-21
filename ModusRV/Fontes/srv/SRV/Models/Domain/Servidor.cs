using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SRV.Models.Domain
{
    public class Servidor
    {
		[Display(Name = "Matrícula")]
		public int IdServidor { get; set; }

		[Display(Name = "Servidor")]
        public string DesNomeServidor { get; set; }

        public long DesCpfServidor { get; set; }

        public bool? Elegivel { get; set; }

		public string IdFuncional { get; set; }

		public int? Vinculo { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            if (((Servidor)obj).IdServidor == this.IdServidor &&
                ((Servidor)obj).DesNomeServidor == this.DesNomeServidor &&
                ((Servidor)obj).DesCpfServidor == this.DesCpfServidor &&
                ((Servidor)obj).Elegivel == this.Elegivel)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return IdServidor == 0 ? 0 : IdServidor.GetHashCode();
        }

        public override string ToString()
        {
            return IdServidor.ToString();
        }
    }
}