using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.Text;
using SRV.Models.DTO;

namespace SRV.Models.Mapper
{
    public class DenunciaAvaliacaoExternaMapper: BaseMapper<DenunciaAvaliacaoExterna>
    {
        protected override string QueryFindObject()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT 
                            s.id_servidor, s.des_nome_servidor
                        FROM [rv_denuncia_avaliacao_externa] dae
                            INNER JOIN [rv_servidor] s 
                                ON dae.id_servidor = s.id_servidor
                        WHERE s.des_nome_servidor LIKE @desNomeServidor
                            AND dae.id_servidor = @idServidor");

            return sql.ToString();
        }

        protected override string QueryListObjects()
        {
            throw new NotImplementedException();
        }

        private string QueryListObjectsByAnoServidorUnidade()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"SELECT dae.id_denuncia_avaliacao_externa, dae.id_ano_referencia, 
                           dae.id_unidade_administrativa, dae.id_avaliacao_externa, dae.id_servidor, 
                           ua.des_unidade_administrativa, ae.des_avaliacao_externa, 
                           s.des_nome_servidor, dae.des_motivo_denuncia
                         FROM rv_denuncia_avaliacao_externa dae
                            INNER JOIN rv_unidade_administrativa ua ON
                                ua.id_unidade_administrativa = dae.id_unidade_administrativa
                            INNER JOIN rv_avaliacao_externa ae ON
                                ae.id_avaliacao_externa = dae.id_avaliacao_externa
                            INNER JOIN rv_servidor s ON
                                s.id_servidor = dae.id_servidor
                         WHERE
                            dae.id_ano_referencia = @idAnoReferencia AND 
                            dae.id_servidor = @idServidor AND 
                            dae.id_unidade_administrativa = @idUnidade");

            return sql.ToString();
        }

        public DenunciaAvaliacaoExterna LoadObjectByAnoServidorUnidade(System.Data.SqlClient.SqlDataReader reader)
        {
            DenunciaAvaliacaoExterna denunciaAvaliacaoExterna = new DenunciaAvaliacaoExterna();

            denunciaAvaliacaoExterna.IdDenunciaAvaliacaoExterna = Convert.ToInt32(reader["id_denuncia_avaliacao_externa"]);
            denunciaAvaliacaoExterna.DesMotivoDenuncia = reader["des_motivo_denuncia"].ToString();
            
            denunciaAvaliacaoExterna.AnoReferencia = new AnoReferencia();
            denunciaAvaliacaoExterna.AnoReferencia.IdAnoReferencia = Convert.ToInt32(reader["id_ano_referencia"]);

            denunciaAvaliacaoExterna.UnidadeAdministrativa = new UnidadeAdministrativa();
            denunciaAvaliacaoExterna.UnidadeAdministrativa.IdUnidadeAdministrativa = Convert.ToInt32(reader["id_unidade_administrativa"]);
            denunciaAvaliacaoExterna.UnidadeAdministrativa.DesUnidadeAdministrativa = reader["des_unidade_administrativa"].ToString();

            denunciaAvaliacaoExterna.Servidor = new Servidor();
            denunciaAvaliacaoExterna.Servidor.IdServidor = Convert.ToInt32(reader["id_servidor"]);
            denunciaAvaliacaoExterna.Servidor.DesNomeServidor = reader["des_nome_servidor"].ToString();

            denunciaAvaliacaoExterna.AvaliacaoExterna = new AvaliacaoExterna();
            denunciaAvaliacaoExterna.AvaliacaoExterna.IdAvaliacaoExterna = Convert.ToInt32(reader["id_avaliacao_externa"]);
            denunciaAvaliacaoExterna.AvaliacaoExterna.DesAvaliacaoExterna = reader["des_avaliacao_externa"].ToString();

            return denunciaAvaliacaoExterna;
        }

        public override DenunciaAvaliacaoExterna LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            DenunciaAvaliacaoExterna denunciaAvaliacaoExterna = new DenunciaAvaliacaoExterna();

            denunciaAvaliacaoExterna.Servidor = new Servidor();
            denunciaAvaliacaoExterna.Servidor.IdServidor = Convert.ToInt32(reader["id_servidor"]);
            denunciaAvaliacaoExterna.Servidor.DesNomeServidor = reader["des_nome_servidor"].ToString();

            return denunciaAvaliacaoExterna;
        }

        private string QueryList(FiltroDenunciaAvaliacaoExterna filtro)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT 
                            s.id_servidor, s.des_nome_servidor
                        FROM [rv_denuncia_avaliacao_externa] dae
                            INNER JOIN [rv_servidor] s 
                                ON dae.id_servidor = s.id_servidor");

            if (filtro.IdServidor != null)
            {
                sql.Append(" WHERE dae.id_servidor = @idServidor");

                if (filtro.DesNomeServidor != null)
                    sql.Append(" AND s.des_nome_servidor LIKE @desNomeServidor");
            }
            else if (filtro.DesNomeServidor != null)
            {
                sql.Append(" WHERE s.des_nome_servidor LIKE @desNomeServidor");
            }

            return sql.ToString();
        }

        private string QueryFindServidor()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT 
                            s.id_servidor, s.des_nome_servidor
                        FROM [rv_denuncia_avaliacao_externa] dae
                            INNER JOIN [rv_servidor] s 
                                ON dae.id_servidor = s.id_servidor
                        WHERE dae.id_servidor = @idServidor");

            return sql.ToString();
        }

        public IList<DenunciaAvaliacaoExterna> List()
        {
            return ListObjects();
        }

        public Paging<DenunciaAvaliacaoExterna> List(FiltroDenunciaAvaliacaoExterna filtro, int currentPage, int pageSize)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            if (filtro.IdServidor != null)
                param.Add("idServidor", filtro.IdServidor);

            if (filtro.DesNomeServidor != null)
                param.Add("desNomeServidor", "%" + filtro.DesNomeServidor + "%");

            return ListPagingObjects(QueryList(filtro), param, LoadObject, currentPage, pageSize);
        }

        public IList<DenunciaAvaliacaoExterna> ListBy(int idAnoReferencia, int idServidor, int idUnidade)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAnoReferencia", idAnoReferencia);
            param.Add("idServidor", idServidor);
            param.Add("idUnidade", idUnidade);

            return ListObjects(QueryListObjectsByAnoServidorUnidade(), param, LoadObjectByAnoServidorUnidade);
        }

        public DenunciaAvaliacaoExterna Find(int idServidor)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idServidor", idServidor);

            return FindObject(QueryFindServidor(), param, LoadObject);
        }

        public DenunciaAvaliacaoExterna Find(int idServidor, string desNomeServidor)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idServidor", idServidor);
            param.Add("desNomeServidor", "%" + desNomeServidor + "%");

            return FindObject(QueryFindObject(), param, LoadObject);
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_denuncia_avaliacao_externa(
						id_ano_referencia, 
						id_unidade_administrativa, 
						id_servidor, 
						id_avaliacao_externa,
						des_motivo_denuncia
					)
					VALUES (
						@idAnoReferencia, 
						@idUnidadeAdministrativa, 
						@idServidor, 
						@idAvaliacaoExterna,
						@desMotivoDenuncia
					)";
        }

        private string QueryDelete()
        {
            return @"DELETE FROM rv_denuncia_avaliacao_externa WHERE id_servidor = @idServidor";
        }

        private string QueryDeleteAllRows()
        {
            return @"DELETE FROM rv_denuncia_avaliacao_externa";
        }

        public void Insert(DenunciaAvaliacaoExterna denunciaProvaAvaliacaoExterna)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAnoReferencia", denunciaProvaAvaliacaoExterna.AnoReferencia.IdAnoReferencia);
            param.Add("idUnidadeAdministrativa", denunciaProvaAvaliacaoExterna.UnidadeAdministrativa.IdUnidadeAdministrativa);
            param.Add("idServidor", denunciaProvaAvaliacaoExterna.Servidor.IdServidor);
            param.Add("idAvaliacaoExterna", denunciaProvaAvaliacaoExterna.AvaliacaoExterna.IdAvaliacaoExterna);
            param.Add("desMotivoDenuncia", denunciaProvaAvaliacaoExterna.DesMotivoDenuncia);

            InsertObject(QueryInsert(), param);
        }

        public void Delete(int idServidor)
        {
            Dictionary<string, int> param = new Dictionary<string, int>();

            param.Add("idServidor", idServidor);

            DeleteObject(QueryDelete(), param);
        }

        public void DeleteAllRows()
        {
            DeleteObject(QueryDeleteAllRows(), null);
        }
    }
}