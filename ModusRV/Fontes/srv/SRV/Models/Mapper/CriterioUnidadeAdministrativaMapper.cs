using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.Text;
using SRV.Models.DTO;
using SRV.Common.Extension;

namespace SRV.Models.Mapper
{
    public class CriterioUnidadeAdministrativaMapper : BaseMapper<CriterioUnidadeAdministrativa>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT cua.id_unidade_administrativa, cua.id_ano_referencia,
                            cua.nm_perc_curriculo_minimo, cua.nm_perc_lancamento_nota, 
                            cua.nm_nota_ige
                       FROM rv_criterio_unidadm cua
                      WHERE cua.id_unidade_administrativa = @idUnidadeAdministrativa
                        AND cua.id_ano_referencia = @idAnoReferencia";
        }

        protected override string QueryListObjects()
        {
            throw new NotImplementedException();
        }

        private string QueryList(FiltroCriterioUnidadeAdministrativa filtro)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@" SELECT ua.id_unidade_administrativa, ua.des_unidade_administrativa,
                                 cua.id_ano_referencia, cua.nm_perc_curriculo_minimo,
                                 cua.nm_perc_lancamento_nota, cua.nm_nota_ige
                            FROM rv_criterio_unidadm cua,
                                 rv_unidade_administrativa ua
                           WHERE cua.id_unidade_administrativa = ua.id_unidade_administrativa");

            if (filtro.IdUnidadeAdministrativa != null)
                sql.Append(" AND ua.id_unidade_administrativa = @idUnidadeAdministrativa");

            sql.Append(@"    AND cua.id_ano_referencia = @idAnoReferencia
                           ORDER BY ua.des_unidade_administrativa, cua.id_ano_referencia");

            return sql.ToString();
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_criterio_unidadm
                                   (id_unidade_administrativa,
                                    id_ano_referencia,
                                    nm_perc_curriculo_minimo,
                                    nm_perc_lancamento_nota,
                                    nm_nota_ige)
                            VALUES (@idUnidadeAdministrativa,
                                    @idAnoReferencia,
                                    @percCurriculoMinimo,
                                    @percLancamentoNota,
                                    @notaIge)";
        }

        private string QueryUpdate()
        {
            return @"UPDATE rv_criterio_unidadm
                        SET nm_perc_curriculo_minimo = @percCurriculoMinimo,
                            nm_perc_lancamento_nota = @percLancamentoNota,
                            nm_nota_ige = @notaIge
                      WHERE id_unidade_administrativa = @idUnidadeAdministrativa
                        AND id_ano_referencia = @idAnoReferencia";
        }

        private string QueryDelete()
        {
            return @"DELETE FROM rv_criterio_unidadm
                           WHERE id_unidade_administrativa = @idUnidadeAdministrativa
                             AND id_ano_referencia = @idAnoReferencia";
        }

        private string QueryExisteCriterioUnidadeAdministrativa()
        {
            return @"SELECT COUNT(*)
                       FROM rv_criterio_unidadm
                      WHERE id_unidade_administrativa = @idUnidadeAdministrativa
                        AND id_ano_referencia = @idAnoReferencia";
        }

        public override CriterioUnidadeAdministrativa LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            CriterioUnidadeAdministrativa criterioUnidadeAdministrativa = new CriterioUnidadeAdministrativa();

            criterioUnidadeAdministrativa.UnidadeAdministrativa = new UnidadeAdministrativa();
            criterioUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa = Convert.ToInt32(reader["id_unidade_administrativa"]);

            if (reader.HasColumn("des_unidade_administrativa"))
                criterioUnidadeAdministrativa.UnidadeAdministrativa.DesUnidadeAdministrativa = (string)reader["des_unidade_administrativa"];

            criterioUnidadeAdministrativa.AnoReferencia = new AnoReferencia();
            criterioUnidadeAdministrativa.AnoReferencia.IdAnoReferencia = (int)reader["id_ano_referencia"];

            criterioUnidadeAdministrativa.PerCurriculoMinimo = (decimal?)reader["nm_perc_curriculo_minimo"];
            criterioUnidadeAdministrativa.PercLancamentoNota = (decimal?)reader["nm_perc_lancamento_nota"];
            
            if(reader["nm_nota_ige"] != DBNull.Value)
                criterioUnidadeAdministrativa.NotaIge = Convert.ToDecimal(reader["nm_nota_ige"]);

            return criterioUnidadeAdministrativa;
        }

        public CriterioUnidadeAdministrativa Find(int idUnidadeAdministrativa, int idAnoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", idUnidadeAdministrativa);
            param.Add("idAnoReferencia", idAnoReferencia);

            return FindObject(QueryFindObject(), param);
        }

        public Paging<CriterioUnidadeAdministrativa> List(FiltroCriterioUnidadeAdministrativa filtro, int currentPage, int pageSize)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            if (filtro.IdUnidadeAdministrativa != null)
                param.Add("idUnidadeAdministrativa", filtro.IdUnidadeAdministrativa);

            param.Add("idAnoReferencia", filtro.IdAnoReferencia);

            return ListPagingObjects(QueryList(filtro), param, LoadObject, currentPage, pageSize);
        }

        public void Insert(CriterioUnidadeAdministrativa criterioUnidadeAdministrativa)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", criterioUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa);
            param.Add("idAnoReferencia", criterioUnidadeAdministrativa.AnoReferencia.IdAnoReferencia);
            param.Add("percCurriculoMinimo", criterioUnidadeAdministrativa.PerCurriculoMinimo);
            param.Add("percLancamentoNota", criterioUnidadeAdministrativa.PercLancamentoNota);
            param.Add("notaIge", criterioUnidadeAdministrativa.NotaIge);

            InsertObject(QueryInsert(), param);
        }

        public void Update(CriterioUnidadeAdministrativa criterioUnidadeAdministrativa)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", criterioUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa);
            param.Add("idAnoReferencia", criterioUnidadeAdministrativa.AnoReferencia.IdAnoReferencia);
            param.Add("percCurriculoMinimo", criterioUnidadeAdministrativa.PerCurriculoMinimo);
            param.Add("percLancamentoNota", criterioUnidadeAdministrativa.PercLancamentoNota);
            param.Add("notaIge", criterioUnidadeAdministrativa.NotaIge);

            UpdateObject(QueryUpdate(), param);
        }

        public void Delete(int idUnidadeAdministrativa, int idAnoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", idUnidadeAdministrativa);
            param.Add("idAnoReferencia", idAnoReferencia);

            DeleteObject(QueryDelete(), param);
        }

        public bool ExisteCriterioUnidadeAdministrativa(int idUnidadeAdministrativa, int idAnoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", idUnidadeAdministrativa);
            param.Add("idAnoReferencia", idAnoReferencia);

            short cont = Convert.ToInt16(ExecuteScalarQuery(QueryExisteCriterioUnidadeAdministrativa(), param));

            return cont > 0 ? true : false;
        }
    }
}