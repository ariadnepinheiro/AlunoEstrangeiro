using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Common;

namespace Proderj.DOL.Service
{
	public class DTOProtocoloNota
	{
		public string Codigo
		{
			get
			{
				if (Id == 0 || Tipo.IsNullOrEmpty())
				{
					return string.Empty;
				}

				return "{0}{1}{2}{3}{4}/{5}{6}".Fmt
				(
					this.Ano,
					this.Periodo,
					this.CodigoTurma,
					this.CodigoDisciplina,
                    this.IdFuncional,
					this.Tipo,
					this.Id.ToString().PadLeft(9, '0')
				);
			}
		}

		public int Id;

        public string IdFuncional { get; set; }

		public string CodigoDisciplina { get; set; }

		public string CodigoTurma { get; set; }

		public short Ano { get; set; }

		public short Periodo { get; set; }

		public short SubPeriodo { get; set; }

		public String Tipo { get; set; }

	}
}
