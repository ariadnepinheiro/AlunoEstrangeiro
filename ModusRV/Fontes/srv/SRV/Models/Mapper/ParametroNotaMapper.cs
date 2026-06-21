using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;

namespace SRV.Models.Mapper
{
    public class ParametroNotaMapper : BaseMapper<ParametroNota>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT id_modalidade, id_ano_referencia,
                            id_nota, nm_percentual, fl_valor_meta,
                            id_indicador
                       FROM rv_parametro_nota
                      WHERE id_modalidade = @idModalidade
                        AND id_ano_referencia = @idAnoReferencia
                        AND id_nota = @idNota
                        AND id_indicador = @idIndicador";
        }

        protected override string QueryListObjects()
        {
            return @"SELECT id_modalidade, id_ano_referencia,
                            id_nota, nm_percentual, fl_valor_meta,
                            id_indicador
                       FROM rv_parametro_nota
                      WHERE id_modalidade = @idModalidade
                        AND id_ano_referencia = @idAnoReferencia";
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_parametro_nota
                                   (id_modalidade,
                                    id_ano_referencia,
                                    id_nota,
                                    nm_percentual,
                                    fl_valor_meta,
                                    id_indicador)
                            VALUES (@idModalidade,
                                    @idAnoReferencia,
                                    @idNota,
                                    @percentual,
                                    @valorMeta,
                                    @idIndicador)";
        }

        private string QueryUpdate()
        {
            return @"UPDATE rv_parametro_nota
                        SET nm_percentual = @percentual,
                            fl_valor_meta = @valorMeta
                      WHERE id_modalidade = @idModalidade
                        AND id_ano_referencia = @idAnoReferencia
                        AND id_nota = @idNota
                        AND id_indicador = @idIndicador";
        }

        public override ParametroNota LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            ParametroNota parametroNota = new ParametroNota();

            parametroNota.Percentual = (decimal)reader["nm_percentual"];
            parametroNota.ValorMeta = reader["fl_valor_meta"].ToString().Equals("S") ? true : false;

            parametroNota.Modalidade = new Modalidade();
            parametroNota.Modalidade.IdModalidade = (int)reader["id_modalidade"];

            parametroNota.AnoReferencia = new AnoReferencia();
            parametroNota.AnoReferencia.IdAnoReferencia = (int)reader["id_ano_referencia"];

            parametroNota.Nota = new Nota();
            parametroNota.Nota.IdNota = (int)reader["id_nota"];

            parametroNota.Indicador = new Indicador();
            parametroNota.Indicador.IdIndicador = (int)reader["id_indicador"];

            return parametroNota;
        }

        public ParametroNota Find(int idModalidade, int idAnoReferencia, int idNota, int idIndicador)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idModalidade", idModalidade);
            param.Add("idAnoReferencia", idAnoReferencia);
            param.Add("idNota", idNota);
            param.Add("idIndicador", idIndicador);

            return FindObject(QueryFindObject(), param);
        }

        public IList<ParametroNota> List(int idModalidade, int idAnoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idModalidade", idModalidade);
            param.Add("idAnoReferencia", idAnoReferencia);

            return ListObjects(QueryListObjects(), param);
        }

        public void Insert(ParametroNota parametroNota)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idModalidade", parametroNota.Modalidade.IdModalidade);
            param.Add("idAnoReferencia", parametroNota.AnoReferencia.IdAnoReferencia);
            param.Add("idNota", parametroNota.Nota.IdNota);
            param.Add("percentual", parametroNota.Percentual);
            param.Add("valorMeta", parametroNota.ValorMeta ? "S" : "N");
            param.Add("idIndicador", parametroNota.Indicador.IdIndicador);

            InsertObject(QueryInsert(), param);
        }

        public void Update(ParametroNota parametroNota)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idModalidade", parametroNota.Modalidade.IdModalidade);
            param.Add("idAnoReferencia", parametroNota.AnoReferencia.IdAnoReferencia);
            param.Add("idNota", parametroNota.Nota.IdNota);
            param.Add("percentual", parametroNota.Percentual);
            param.Add("valorMeta", parametroNota.ValorMeta ? "S" : "N");
            param.Add("idIndicador", parametroNota.Indicador.IdIndicador);

            UpdateObject(QueryUpdate(), param);
        }
    }
}