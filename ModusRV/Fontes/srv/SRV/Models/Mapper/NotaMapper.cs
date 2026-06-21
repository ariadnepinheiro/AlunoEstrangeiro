using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;

namespace SRV.Models.Mapper
{
    public class NotaMapper : BaseMapper<Nota>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT id_nota, des_nota, id_ano_referencia
                       FROM rv_nota
                      WHERE id_nota = @idNota";
        }

        protected override string QueryListObjects()
        {
            return @"SELECT id_nota, des_nota, id_ano_referencia
                       FROM rv_nota
                      WHERE id_ano_referencia = @ciclo
                      ORDER BY des_nota, id_ano_referencia";
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_nota
                               (des_nota,
                                id_ano_referencia)
                        VALUES (@desNota,
                                @idAnoReferencia)";
        }

        private string QueryUpdate()
        {
            return @"UPDATE rv_nota
                        SET des_nota = @desNota
                      WHERE id_nota = @idNota";
        }

        private string QueryDelete()
        {
            return @"DELETE FROM rv_nota
                           WHERE id_nota = @idNota";
        }

        public override Nota LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            Nota nota = new Nota();

            nota.IdNota = (int)reader["id_nota"];
            nota.DesNota = (decimal)reader["des_nota"];

            nota.AnoReferencia = new AnoReferencia();
            nota.AnoReferencia.IdAnoReferencia = (int)reader["id_ano_referencia"];

            return nota;
        }

        public Nota Find(int idNota)
        {
            return FindObject("idNota", idNota);
        }

        public IList<Nota> List(int ciclo)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("ciclo", ciclo);

            return ListObjects(QueryListObjects(), param);
        }

        public Nota Insert(Nota nota)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("desNota", nota.DesNota);
            param.Add("idAnoReferencia", nota.AnoReferencia.IdAnoReferencia);

            nota.IdNota = InsertObjectWithIdentity(QueryInsert(), param);

            return nota;
        }

        public void Update(Nota nota)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idNota", nota.IdNota);
            param.Add("desNota", nota.DesNota);

            UpdateObject(QueryUpdate(), param);
        }

        public void Delete(int idNota)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idNota", idNota);

            DeleteObject(QueryDelete(), param);
        }
    }
}