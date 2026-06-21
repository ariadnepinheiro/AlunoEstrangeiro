using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;

namespace SRV.Models.Mapper
{
    public class ParametroCurvaMapper : BaseMapper<ParametroCurva>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT id_ano_referencia, id_nota, nm_qtd_vencimento,
                            id_grupo_funcao, id_tipo_unidadm
                       FROM rv_parametro_curva_premiacao
                      WHERE id_ano_referencia = @idAnoReferencia
                        AND id_nota = @idNota
                        AND id_grupo_funcao = @idGrupoFuncao
                        AND id_tipo_unidadm = @idTipoUnidadeAdm";
        }

        protected override string QueryListObjects()
        {
            return @"SELECT id_ano_referencia, id_nota, nm_qtd_vencimento,
                            id_grupo_funcao, id_tipo_unidadm
                       FROM rv_parametro_curva_premiacao
                      WHERE id_tipo_unidadm = @idTipoUnidadeAdm
                        AND id_ano_referencia = @idAnoReferencia";
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_parametro_curva_premiacao
                                   (id_ano_referencia,
                                    id_nota,
                                    nm_qtd_vencimento,
                                    id_grupo_funcao,
                                    id_tipo_unidadm)
                            VALUES (@idAnoReferencia,
                                    @idNota,
                                    @qtdeVencimento,
                                    @idGrupoFuncao,
                                    @idTipoUnidadeAdm)";
        }

        private string QueryUpdate()
        {
            return @"UPDATE rv_parametro_curva_premiacao
                        SET nm_qtd_vencimento = @qtdeVencimento
                      WHERE id_ano_referencia = @idAnoReferencia
                        AND id_nota = @idNota
                        AND id_grupo_funcao = @idGrupoFuncao
                        AND id_tipo_unidadm = @idTipoUnidadeAdm";
        }

        public override ParametroCurva LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            ParametroCurva parametroCurva = new ParametroCurva();

            parametroCurva.QuantidadeVencimento = (decimal)reader["nm_qtd_vencimento"];

            parametroCurva.AnoReferencia = new AnoReferencia();
            parametroCurva.AnoReferencia.IdAnoReferencia = (int)reader["id_ano_referencia"];

            parametroCurva.Nota = new Nota();
            parametroCurva.Nota.IdNota = (int)reader["id_nota"];

            parametroCurva.GrupoFuncao = new GrupoFuncao();
            parametroCurva.GrupoFuncao.IdGrupoFuncao = (int)reader["id_grupo_funcao"];

            parametroCurva.TipoUnidadeAdministrativa = new TipoUnidadeAdministrativa();
            parametroCurva.TipoUnidadeAdministrativa.IdTipoUnidAdm = (int)reader["id_tipo_unidadm"];

            return parametroCurva;
        }

        public ParametroCurva Find(int idAnoReferencia, int idNota, int idGrupoFuncao, int idTipoUnidadeAdm)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAnoReferencia", idAnoReferencia);
            param.Add("idNota", idNota);
            param.Add("idGrupoFuncao", idGrupoFuncao);
            param.Add("idTipoUnidadeAdm", idTipoUnidadeAdm);

            return FindObject(QueryFindObject(), param);
        }

        public IList<ParametroCurva> List(int idTipoUnidadeAdm, int idAnoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idTipoUnidadeAdm", idTipoUnidadeAdm);
            param.Add("idAnoReferencia", idAnoReferencia);

            return ListObjects(QueryListObjects(), param);
        }

        public void Insert(ParametroCurva parametroCurva)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAnoReferencia", parametroCurva.AnoReferencia.IdAnoReferencia);
            param.Add("idNota", parametroCurva.Nota.IdNota);
            param.Add("qtdeVencimento", parametroCurva.QuantidadeVencimento);
            param.Add("idGrupoFuncao", parametroCurva.GrupoFuncao.IdGrupoFuncao);
            param.Add("idTipoUnidadeAdm", parametroCurva.TipoUnidadeAdministrativa.IdTipoUnidAdm);

            InsertObject(QueryInsert(), param);
        }

        public void Update(ParametroCurva parametroCurva)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAnoReferencia", parametroCurva.AnoReferencia.IdAnoReferencia);
            param.Add("idNota", parametroCurva.Nota.IdNota);
            param.Add("qtdeVencimento", parametroCurva.QuantidadeVencimento);
            param.Add("idGrupoFuncao", parametroCurva.GrupoFuncao.IdGrupoFuncao);
            param.Add("idTipoUnidadeAdm", parametroCurva.TipoUnidadeAdministrativa.IdTipoUnidAdm);

            UpdateObject(QueryUpdate(), param);
        }
    }
}