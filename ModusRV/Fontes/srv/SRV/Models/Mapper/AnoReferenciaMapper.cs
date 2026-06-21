using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;

namespace SRV.Models.Mapper
{
    public class AnoReferenciaMapper : BaseMapper<AnoReferencia>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT id_ano_referencia, dt_inicio_periodoletivo, 
                            dt_fim_periodoletivo, nome_proc_calc
                       FROM rv_ano_referencia 
                      WHERE id_ano_referencia = @idAnoReferencia";
        }

        protected override string QueryListObjects()
        {
            return "SELECT * FROM rv_ano_referencia ORDER BY id_ano_referencia";
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_ano_referencia
                                   (id_ano_referencia,
                                    dt_inicio_periodoletivo,
                                    dt_fim_periodoletivo,
                                    nome_proc_calc)
                            VALUES (@idAnoReferencia,
                                    @dtInicio,
                                    @dtFim,
                                    @nomeProc)";
        }

        private string QueryUpdate()
        {
            return @"UPDATE rv_ano_referencia
                        SET dt_inicio_periodoletivo = @dtInicio,
                            dt_fim_periodoletivo = @dtFim,
                            nome_proc_calc = @nomeProc
                      WHERE id_ano_referencia = @idAnoReferencia";
        }

        private string QueryDelete()
        {
            return @"DELETE FROM rv_ano_referencia
                           WHERE id_ano_referencia = @idAnoReferencia";
        }

        private string QueryValidaInsert()
        {
            return @"SELECT COUNT(*)
                       FROM rv_ano_referencia
                      WHERE id_ano_referencia = @idAnoReferencia";
        }

        public override AnoReferencia LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            AnoReferencia anoReferencia = new AnoReferencia();

            anoReferencia.IdAnoReferencia = (int)reader["id_ano_referencia"];
            anoReferencia.DtInicioPeriodoLetivo = (DateTime)reader["dt_inicio_periodoletivo"];
            anoReferencia.DtFimPeriodoLetivo = (DateTime)reader["dt_fim_periodoletivo"];
            anoReferencia.NomeProcCalculo = (string)reader["nome_proc_calc"];

            return anoReferencia;
        }

        public AnoReferencia Find(int idAnoReferencia)
        {
            return FindObject("idAnoReferencia", idAnoReferencia);
        }

        public IList<AnoReferencia> List()
        {
            return ListObjects();
        }

        public AnoReferencia Insert(AnoReferencia anoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAnoReferencia", anoReferencia.IdAnoReferencia);
            param.Add("dtInicio", anoReferencia.DtInicioPeriodoLetivo);
            param.Add("dtFim", anoReferencia.DtFimPeriodoLetivo);
            param.Add("nomeProc", anoReferencia.NomeProcCalculo.ToUpper());

            InsertObject(QueryInsert(), param);

            return anoReferencia;
        }

        public void Update(AnoReferencia anoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAnoReferencia", anoReferencia.IdAnoReferencia);
            param.Add("dtInicio", anoReferencia.DtInicioPeriodoLetivo);
            param.Add("dtFim", anoReferencia.DtFimPeriodoLetivo);
            param.Add("nomeProc", anoReferencia.NomeProcCalculo.ToUpper());

            UpdateObject(QueryUpdate(), param);
        }

        public void Delete(int idAnoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAnoReferencia", idAnoReferencia);

            DeleteObject(QueryDelete(), param);
        }

        public bool ValidaInsert(int idAnoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAnoReferencia", idAnoReferencia);

            short cont = Convert.ToInt16(ExecuteScalarQuery(QueryValidaInsert(), param));

            return cont > 0 ? false : true;
        }
    }
}