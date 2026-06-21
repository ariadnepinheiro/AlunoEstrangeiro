using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.Text;

namespace SRV.Models.Mapper
{
    public class ModalidadeMapper : BaseMapper<Modalidade>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT id_modalidade, des_modalidade, sigla_modalidade
                       FROM rv_modalidade
                      WHERE id_modalidade = @idModalidade";
        }

        protected override string QueryListObjects()
        {
            return @"SELECT id_modalidade, des_modalidade, sigla_modalidade
                       FROM rv_modalidade
                      ORDER BY des_modalidade, sigla_modalidade";
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_modalidade
                               (des_modalidade,
                                sigla_modalidade)
                        VALUES (@desModalidade,
                                @siglaModalidade)";
        }

        private string QueryUpdate()
        {
            return @"UPDATE rv_modalidade
                        SET des_modalidade = @desModalidade,
                            sigla_modalidade = @siglaModalidade
                      WHERE id_modalidade = @idModalidade";
        }

        private string QueryDelete()
        {
            return @"DELETE FROM rv_modalidade
                           WHERE id_modalidade = @idModalidade";
        }

        private string QueryFindBySigla()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@" SELECT TOP 1 id_modalidade
                                  FROM rv_modalidade
                                 WHERE sigla_modalidade = @siglaModalidade");

            return sql.ToString();
        }

        private string QueryValidaSigla(int? idModalidade)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@" SELECT COUNT(*)
                            FROM rv_modalidade
                           WHERE sigla_modalidade = @siglaModalidade");

            if (idModalidade != null)
                sql.Append(" AND id_modalidade <> @idModalidade");

            return sql.ToString();
        }

        private string QueryExisteModalidade()
        {
            return @"SELECT COUNT(*)
                       FROM rv_modalidade
                      WHERE id_modalidade = @idModalidade";
        }

        public override Modalidade LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            Modalidade modalidade = new Modalidade();

            modalidade.IdModalidade = (int)reader["id_modalidade"];
            modalidade.DesModalidade = (string)reader["des_modalidade"];
            modalidade.SiglaModalidade = (string)reader["sigla_modalidade"];

            return modalidade;
        }

        public Modalidade Find(int idModalidade)
        {
            return FindObject("idModalidade", idModalidade);
        }

        public IList<Modalidade> List()
        {
            return ListObjects();
        }

        public Modalidade Insert(Modalidade modalidade)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("desModalidade", modalidade.DesModalidade.ToUpper());
            param.Add("siglaModalidade", modalidade.SiglaModalidade.ToUpper());

            modalidade.IdModalidade = InsertObjectWithIdentity(QueryInsert(), param);

            return modalidade;
        }

        public void Update(Modalidade modalidade)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idModalidade", modalidade.IdModalidade);
            param.Add("desModalidade", modalidade.DesModalidade.ToUpper());
            param.Add("siglaModalidade", modalidade.SiglaModalidade.ToUpper());

            UpdateObject(QueryUpdate(), param);
        }

        public void Delete(int idModalidade)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idModalidade", idModalidade);

            DeleteObject(QueryDelete(), param);
        }

        public bool ValidaSigla(Modalidade modalidade)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            if(modalidade.IdModalidade != null)
                param.Add("idModalidade", modalidade.IdModalidade);

            param.Add("siglaModalidade", modalidade.SiglaModalidade.ToUpper());

            short cont = Convert.ToInt16(ExecuteScalarQuery(QueryValidaSigla(modalidade.IdModalidade), param));

            return cont > 0 ? false : true;
        }

        public bool ExisteModalidade(int idModalidade)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idModalidade", idModalidade);

            short cont = Convert.ToInt16(ExecuteScalarQuery(QueryExisteModalidade(), param));

            return cont > 0 ? true : false;
        }

        public int? FindBySigla(string Sigla)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("siglaModalidade", Sigla.ToUpper());

            return (int?)ExecuteScalarQuery(QueryFindBySigla(), param);
        }
    }
}