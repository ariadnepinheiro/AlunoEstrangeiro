using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using SRV.Common.Extension;
using SRV.Models.DTO;
using System.Text;

namespace SRV.Models.Mapper
{
    public class NivelEnsinoMapper : BaseMapper<NivelEnsino>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT ne.id_nivel_ensino, ne.des_nivel_ensino,
                            m.id_modalidade, m.des_modalidade
                       FROM rv_nivel_ensino ne,
                            rv_modalidade m
                      WHERE ne.id_modalidade = m.id_modalidade
                        AND ne.id_nivel_ensino = @idNivelEnsino
                        AND ne.id_modalidade = @idModalidade";
        }

        protected override string QueryListObjects()
        {
            return @"SELECT ne.id_nivel_ensino, ne.des_nivel_ensino,
                            m.id_modalidade, m.des_modalidade
                       FROM rv_nivel_ensino ne,
                            rv_modalidade m
                      WHERE ne.id_modalidade = m.id_modalidade
                      ORDER BY ne.des_nivel_ensino, m.des_modalidade";
        }

        private string QueryListByModalidade()
        {
            return @"SELECT id_nivel_ensino, des_nivel_ensino
                       FROM rv_nivel_ensino
                      WHERE id_modalidade = @idModalidade
                      ORDER BY des_nivel_ensino";
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_nivel_ensino
                               (des_nivel_ensino,
                                id_modalidade)
                        VALUES (@desNivelEnsino,
                                @idModalidade)";
        }

        private string QueryUpdate()
        {
            return @"UPDATE rv_nivel_ensino
                        SET des_nivel_ensino = @desNivelEnsino
                      WHERE id_nivel_ensino = @idNivelEnsino
                        AND id_modalidade = @idModalidade";
        }

        private string QueryDelete()
        {
            return @"DELETE FROM rv_nivel_ensino
                           WHERE id_nivel_ensino = @idNivelEnsino
                             AND id_modalidade = @idModalidade";
        }

        private string QueryFindByDescNivelEnsinoIdModalidade()
        {
            return @"SELECT TOP 1 id_nivel_ensino
                             FROM rv_nivel_ensino
                            WHERE des_nivel_ensino = @desNivelEnsino
                              AND id_modalidade = @idModalidade";
        }

        private string QueryExisteNivelEnsino()
        {
            return @"SELECT COUNT(*)
                       FROM rv_nivel_ensino
                      WHERE id_nivel_ensino = @idNivelEnsino
                        AND id_modalidade = @idModalidade";
        }

        private string QueryValidaNivelEnsino(int? idNivelEnsino)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT COUNT(*)
                           FROM rv_nivel_ensino
                          WHERE des_nivel_ensino = @desNivelEnsino
                            AND id_modalidade = @idModalidade");

            if (idNivelEnsino != null)
                sql.Append(" AND id_nivel_ensino <> @idNivelEnsino");

            return sql.ToString();
        }

        public override NivelEnsino LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            NivelEnsino nivelEnsino = new NivelEnsino();

            nivelEnsino.IdNivelEnsino = (int)reader["id_nivel_ensino"];
            nivelEnsino.DesNivelEnsino = (string)reader["des_nivel_ensino"];

            nivelEnsino.Modalidade = new Modalidade();
            nivelEnsino.Modalidade.IdModalidade = (int)reader["id_modalidade"];
            nivelEnsino.Modalidade.DesModalidade = (string)reader["des_modalidade"];

            return nivelEnsino;
        }

        public NivelEnsino LoadObjectSimple(System.Data.SqlClient.SqlDataReader reader)
        {
            NivelEnsino nivelEnsino = new NivelEnsino();

            nivelEnsino.IdNivelEnsino = (int)reader["id_nivel_ensino"];
            nivelEnsino.DesNivelEnsino = (string)reader["des_nivel_ensino"];

            return nivelEnsino;
        }

        public NivelEnsino Find(int idNivelEnsino, int idModalidade)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idNivelEnsino", idNivelEnsino);
            param.Add("idModalidade", idModalidade);

            return FindObject(QueryFindObject(), param);
        }

        public IList<NivelEnsino> List()
        {
            return ListObjects();
        }

        public IList<NivelEnsino> ListByModalidade(int idModalidade)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idModalidade", idModalidade);

            return ListObjects(QueryListByModalidade(), param, LoadObjectSimple);
        }

        public NivelEnsino Insert(NivelEnsino nivelEnsino)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("desNivelEnsino", nivelEnsino.DesNivelEnsino.Trim().ToUpper());
            param.Add("idModalidade", nivelEnsino.Modalidade.IdModalidade);

            nivelEnsino.IdNivelEnsino = InsertObjectWithIdentity(QueryInsert(), param);

            return nivelEnsino;
        }

        public void Update(NivelEnsino nivelEnsino)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idNivelEnsino", nivelEnsino.IdNivelEnsino);
            param.Add("desNivelEnsino", nivelEnsino.DesNivelEnsino.Trim().ToUpper());
            param.Add("idModalidade", nivelEnsino.Modalidade.IdModalidade);

            UpdateObject(QueryUpdate(), param);
        }

        public void Delete(int idNivelEnsino, int idModalidade)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idNivelEnsino", idNivelEnsino);
            param.Add("idModalidade", idModalidade);

            DeleteObject(QueryDelete(), param);
        }

        public bool ExisteNivelEnsino(int idNivelEnsino, int idModalidade)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idNivelEnsino", idNivelEnsino);
            param.Add("idModalidade", idModalidade);

            short cont = Convert.ToInt16(ExecuteScalarQuery(QueryExisteNivelEnsino(), param));

            return cont > 0 ? true : false;
        }

        public int? FindByModalidadeDesc(int idModalidade, string DesNivelEnsino)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("DesNivelEnsino", DesNivelEnsino);
            param.Add("idModalidade", idModalidade);

            return (int?)ExecuteScalarQuery(QueryFindByDescNivelEnsinoIdModalidade(), param);
        }

        public bool ValidaNivelEnsino(NivelEnsino nivelEnsino)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            if (nivelEnsino.IdNivelEnsino != null)
                param.Add("idNivelEnsino", nivelEnsino.IdNivelEnsino);

            param.Add("desNivelEnsino", nivelEnsino.DesNivelEnsino.Trim().ToUpper());
            param.Add("idModalidade", nivelEnsino.Modalidade.IdModalidade);

            short cont = Convert.ToInt16(ExecuteScalarQuery(QueryValidaNivelEnsino(nivelEnsino.IdNivelEnsino), param));

            return cont > 0 ? false : true;
        }
    }
}