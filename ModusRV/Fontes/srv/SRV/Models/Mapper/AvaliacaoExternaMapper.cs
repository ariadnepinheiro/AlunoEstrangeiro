using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;

namespace SRV.Models.Mapper
{
    public class AvaliacaoExternaMapper : BaseMapper<AvaliacaoExterna>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT id_avaliacao_externa, des_avaliacao_externa, des_periodo_avaliacao
                       FROM rv_avaliacao_externa
                      WHERE id_avaliacao_externa = @idAvaliacaoExterna";
        }

        protected override string QueryListObjects()
        {
            return @"SELECT id_avaliacao_externa, des_avaliacao_externa, des_periodo_avaliacao
                       FROM rv_avaliacao_externa
                      ORDER BY des_avaliacao_externa, des_periodo_avaliacao";
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_avaliacao_externa
                                   (id_avaliacao_externa,
                                    des_avaliacao_externa,
                                    des_periodo_avaliacao)
                            VALUES (@idAvaliacaoExterna,
                                    @desAvaliacaoExterna,
                                    @desPeriodo)";
        }

        private string QueryUpdate()
        {
            return @"UPDATE rv_avaliacao_externa
                        SET des_avaliacao_externa = @desAvaliacaoExterna,
                            des_periodo_avaliacao = @desPeriodo
                      WHERE id_avaliacao_externa = @idAvaliacaoExterna";
        }

        private string QueryDelete()
        {
            return @"DELETE FROM rv_avaliacao_externa
                           WHERE id_avaliacao_externa = @idAvaliacaoExterna";
        }

        private string QueryValida()
        {
            return @"SELECT COUNT(*)
                       FROM rv_avaliacao_externa
                      WHERE id_avaliacao_externa = @idAvaliacaoExterna";
        }

        public override AvaliacaoExterna LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            AvaliacaoExterna avaliacaoExterna = new AvaliacaoExterna();

            avaliacaoExterna.IdAvaliacaoExterna = (int)reader["id_avaliacao_externa"];
            avaliacaoExterna.DesAvaliacaoExterna = (string)reader["des_avaliacao_externa"];
            avaliacaoExterna.DesPeriodoAvaliacao = (string)reader["des_periodo_avaliacao"];

            return avaliacaoExterna;
        }

        public AvaliacaoExterna Find(int idAvaliacaoExterna)
        {
            return FindObject("idAvaliacaoExterna", idAvaliacaoExterna);
        }

        public IList<AvaliacaoExterna> List()
        {
            return ListObjects();
        }

        public AvaliacaoExterna Insert(AvaliacaoExterna avaliacaoExterna)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAvaliacaoExterna", avaliacaoExterna.IdAvaliacaoExterna);
            param.Add("desAvaliacaoExterna", avaliacaoExterna.DesAvaliacaoExterna.ToUpper());
            param.Add("desPeriodo", avaliacaoExterna.DesPeriodoAvaliacao.ToUpper());

            InsertObject(QueryInsert(), param);
            
            return avaliacaoExterna;
        }

        public void Update(AvaliacaoExterna avaliacaoExterna)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAvaliacaoExterna", avaliacaoExterna.IdAvaliacaoExterna);
            param.Add("desAvaliacaoExterna", avaliacaoExterna.DesAvaliacaoExterna.ToUpper());
            param.Add("desPeriodo", avaliacaoExterna.DesPeriodoAvaliacao.ToUpper());

            UpdateObject(QueryUpdate(), param);
        }

        public void Delete(int idAvaliacaoExterna)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAvaliacaoExterna", idAvaliacaoExterna);
            
            DeleteObject(QueryDelete(), param);
        }

        public bool Valida(int idAvaliacaoExterna)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("IdAvaliacaoExterna", idAvaliacaoExterna);

            short cont = Convert.ToInt16(ExecuteScalarQuery(QueryValida(), param));

            return cont > 0 ? true : false;
        }
    }
}