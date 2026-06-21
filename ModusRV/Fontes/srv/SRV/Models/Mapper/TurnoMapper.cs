using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;

namespace SRV.Models.Mapper
{
	public class TurnoMapper : BaseMapper<Turno>
	{
		protected override string QueryFindObject()
		{
			return @"SELECT ID_TURNO, FL_CODIGO_TURNO, DES_NOME_TURNO, FL_GRUPO_TURNO 
					   FROM RV_TURNO
					  WHERE FL_CODIGO_TURNO = @CodigoTurno";
		}

		public Turno Find(string codigoTurno)
		{
			Dictionary<string, object> param = new Dictionary<string, object>();

			param.Add("CodigoTurno", codigoTurno);

			return FindObject(QueryFindObject(), param);
		}

		protected override string QueryListObjects()
		{
			throw new NotImplementedException();
		}

		public override Turno LoadObject(System.Data.SqlClient.SqlDataReader reader)
		{
			Turno turno = new Turno();

			turno.IdTurno = int.Parse(reader["ID_TURNO"].ToString());
			turno.Codigo = (CodigoTurno)Enum.Parse(typeof(CodigoTurno), reader["FL_CODIGO_TURNO"].ToString());
			turno.Nome = reader["DES_NOME_TURNO"].ToString();
			turno.Grupo = (GrupoTurno)Enum.Parse(typeof(GrupoTurno), reader["FL_GRUPO_TURNO"].ToString());
			
			return turno;
		}

		private string QueryValida()
		{
			return @"SELECT COUNT(*)
                       FROM rv_turno
                      WHERE FL_CODIGO_TURNO = @CodigoTurno";
		}

		public bool Valida(string codigoTurno)
		{
			Dictionary<string, object> param = new Dictionary<string, object>();

			param.Add("CodigoTurno", codigoTurno);

			short count = Convert.ToInt16(ExecuteScalarQuery(QueryValida(), param));

			return count > 0 ? true : false;
		}
	}
}
