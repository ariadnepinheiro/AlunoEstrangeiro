using System;
using Techne.Data;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN.Custom
{
	public class Ly_documentos_ingressoCustom : Ly_documentos_ingresso.CustomBase
	{
		public override string PreInsert(Ly_documentos_ingresso.Row row, TConnectionWritable cn)
		{
            row.Doc = RN.DocumentosRequeridos.GeraNumeroDocumento();
			return string.Empty;
		}
	}
}
