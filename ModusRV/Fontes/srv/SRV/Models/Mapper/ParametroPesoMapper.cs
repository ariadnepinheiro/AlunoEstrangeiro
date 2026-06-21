using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;

namespace SRV.Models.Mapper
{
    public class ParametroPesoMapper : BaseMapper<ParametroPeso>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT id_parametro_peso ,id_ano_referencia,
                            id_modalidade ,id_indicador, id_tipo_unidadm,
                            fl_tem_ige ,nm_valor_peso ,id_grupo_funcao
                       FROM rv_parametro_peso
                      WHERE id_parametro_peso = @idParametroPeso";
        }

        protected override string QueryListObjects()
        {
            return @"SELECT id_parametro_peso ,id_ano_referencia,
                            id_modalidade ,id_indicador, id_tipo_unidadm,
                            fl_tem_ige ,nm_valor_peso ,id_grupo_funcao
                       FROM rv_parametro_peso
                      WHERE id_modalidade = @idModalidade
                        AND id_tipo_unidadm = @idTipoUnidadeAdm
                        AND id_ano_referencia = @idAnoReferencia";
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_parametro_peso
                               (id_ano_referencia,
                                id_modalidade,
                                id_indicador,
                                id_tipo_unidadm,
                                fl_tem_ige,
                                nm_valor_peso,
                                id_grupo_funcao)
                        VALUES (@idAnoReferencia,
                                @idModalidade,
                                @idIndicador,
                                @idTipoUnidadeAdm,
                                @temIGE,
                                @valorPeso,
                                @idGrupoFuncao)";
        }

        private string QueryUpdate()
        {
            return @"UPDATE rv_parametro_peso
                        SET nm_valor_peso = @valorPeso
                      WHERE id_parametro_peso = @idParametroPeso";
        }

        public override ParametroPeso LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            ParametroPeso parametroPeso = new ParametroPeso();

            if(reader["id_parametro_peso"] != DBNull.Value)
                parametroPeso.IdParametroPeso = (int)reader["id_parametro_peso"];

            parametroPeso.TemIGE = reader["fl_tem_ige"].ToString().Equals("S") ? true : false;
            parametroPeso.ValorPeso = (decimal)reader["nm_valor_peso"];

            parametroPeso.AnoReferencia = new AnoReferencia();
            parametroPeso.AnoReferencia.IdAnoReferencia = (int)reader["id_ano_referencia"];

            parametroPeso.Modalidade = new Modalidade();
            parametroPeso.Modalidade.IdModalidade= (int)reader["id_modalidade"];

            parametroPeso.Indicador = new Indicador();
            parametroPeso.Indicador.IdIndicador = (int)reader["id_indicador"];

            parametroPeso.TipoUnidadeAdministrativa = new TipoUnidadeAdministrativa();
            parametroPeso.TipoUnidadeAdministrativa.IdTipoUnidAdm = (int)reader["id_tipo_unidadm"];

            parametroPeso.GrupoFuncao = new GrupoFuncao();
            parametroPeso.GrupoFuncao.IdGrupoFuncao = (int)reader["id_grupo_funcao"];

            return parametroPeso;
        }

        public ParametroPeso Find(int idParametroPeso)
        {
            return FindObject("idParametroPeso", idParametroPeso);
        }

        public IList<ParametroPeso> List(int idModalidade, int idTipoUnidadeAdm, int idAnoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idModalidade", idModalidade);
            param.Add("idTipoUnidadeAdm", idTipoUnidadeAdm);
            param.Add("idAnoReferencia", idAnoReferencia);

            return ListObjects(QueryListObjects(), param);
        }

        public ParametroPeso Insert(ParametroPeso parametroPeso)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAnoReferencia", parametroPeso.AnoReferencia.IdAnoReferencia);
            param.Add("idModalidade", parametroPeso.Modalidade.IdModalidade);
            param.Add("idIndicador", parametroPeso.Indicador.IdIndicador);
            param.Add("idTipoUnidadeAdm", parametroPeso.TipoUnidadeAdministrativa.IdTipoUnidAdm);
            param.Add("temIGE", parametroPeso.TemIGE ? "S" : "N");
            param.Add("valorPeso", parametroPeso.ValorPeso);
            param.Add("idGrupoFuncao", parametroPeso.GrupoFuncao.IdGrupoFuncao);

            parametroPeso.IdParametroPeso = InsertObjectWithIdentity(QueryInsert(), param);

            return parametroPeso;
        }

        public void Update(ParametroPeso parametroPeso)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idParametroPeso", parametroPeso.IdParametroPeso);
            param.Add("valorPeso", parametroPeso.ValorPeso);

            UpdateObject(QueryUpdate(), param);
        }
    }
}