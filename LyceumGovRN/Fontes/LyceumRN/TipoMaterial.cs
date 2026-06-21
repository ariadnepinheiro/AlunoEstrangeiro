using System;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN
{
    public class TipoMaterial : RNBase
    {
        public static RetValue AtualizarTipoMaterial(Ly_bib_tipo_material.Row dtTipoMaterial)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                if (dtTipoMaterial != null)
                {
                    Ly_bib_tipo_material.Row.Update(connection, dtTipoMaterial.Id, "sigla, descricao, imagem", dtTipoMaterial.Sigla, dtTipoMaterial.Descricao, dtTipoMaterial.Imagem);

                    retorno = VerificarErro(connection.GetErrors());

                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                }
                return new RetValue(true, "Tipo do material alterado com sucesso.", null);
            }
            catch (Exception e)
            {
                connection.Rollback();
                return new RetValue(false, "", new ErrorList(e.Message.ToString()));
            }
            finally
            {
                connection.Close();
            }
        }

        public static RetValue AtualizarTipoMaterialSemImagem(Ly_bib_tipo_material.Row dtTipoMaterial)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                if (dtTipoMaterial != null)
                {
                    Ly_bib_tipo_material.Row.Update(connection, dtTipoMaterial.Id, "sigla, descricao", dtTipoMaterial.Sigla, dtTipoMaterial.Descricao);

                    retorno = VerificarErro(connection.GetErrors());

                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                }
                return new RetValue(true, "Tipo do material alterado com sucesso.", null);
            }
            catch (Exception e)
            {
                connection.Rollback();
                return new RetValue(false, "", new ErrorList(e.Message.ToString()));
            }
            finally
            {
                connection.Close();
            }
        }

        public static RetValue IncluirTipoMaterial(Ly_bib_tipo_material.Row dtTipoMaterial)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                if (dtTipoMaterial != null)
                {
                    Ly_bib_tipo_material.Row.Insert(connection, dtTipoMaterial.Sigla, "sigla, descricao, imagem", dtTipoMaterial.Sigla, dtTipoMaterial.Descricao, dtTipoMaterial.Imagem);

                    retorno = VerificarErro(connection.GetErrors());

                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }   
                }
                return new RetValue(true, "Tipo do material incluído com sucesso.", null);
            }
            catch (Exception e)
            {
                connection.Rollback();
                return new RetValue(false, "", new ErrorList(e.Message.ToString()));
            }
            finally
            {
                connection.Close();
            }
        }

        public static RetValue IncluirTipoMaterialSemImagem(Ly_bib_tipo_material.Row dtTipoMaterial)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                if (dtTipoMaterial != null)
                {
                    Ly_bib_tipo_material.Row.Insert(connection, dtTipoMaterial.Sigla, "sigla, descricao", dtTipoMaterial.Sigla, dtTipoMaterial.Descricao);

                    retorno = VerificarErro(connection.GetErrors());

                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                }
                return new RetValue(true, "Tipo do material incluído com sucesso.", null);
            }
            catch (Exception e)
            {
                connection.Rollback();
                return new RetValue(false, "", new ErrorList(e.Message.ToString()));
            }
            finally
            {
                connection.Close();
            }
        }

        public static RetValue ExcluirTipoMaterial(Ly_bib_tipo_material.Row dtTipoMaterial)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            try
            {
                if (dtTipoMaterial != null)
                {
                    Ly_bib_tipo_material.Row.Delete(connection, dtTipoMaterial.Id);

                    retorno = VerificarErro(connection.GetErrors());

                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                }
                return new RetValue(true, "Tipo do material excluído com sucesso.", null);
            }
            catch (Exception e)
            {
                connection.Rollback();
                return new RetValue(false, "", new ErrorList(e.Message.ToString()));
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultarTipoMaterial()
        {
            string sql = @"select 
                    id,
                    sigla,
                    descricao,
                    imagem
                    from LY_BIB_TIPO_MATERIAL";
            return RNBase.Consultar(sql);
        }

        public static SimpleRow Consultar(string id)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                object[] parametros = new object[] { id };

                SimpleRow row = SimpleRow.QueryFirstRow(connection, "SELECT * FROM LY_BIB_TIPO_MATERIAL WHERE ID = ?", id);
                return row;
            }
            finally
            {
                connection.Close();
            }
        }

        public static string VerificaExisteSigla(string sigla)
        {
            TConnection connection = Techne.Lyceum.Config.CreateConnection();
            connection.Open();
            QueryTable qt = null;

            try
            {
                qt = new QueryTable("select sigla from Ly_bib_tipo_material where sigla=?");
                qt.Query(connection, sigla);
                if (qt.Rows.Count > 0)
                {
                    return "Já existe esta sigla cadastrada.";
                }
            }
            finally
            {
                connection.Close();
            }
            return "";
        }

        public static string VerificaExisteSigla_Update(string sigla, decimal? id)
        {
            TConnection connection = Techne.Lyceum.Config.CreateConnection();
            connection.Open();
            QueryTable qt = null;

            try
            {
                qt = new QueryTable("select sigla from Ly_bib_tipo_material where sigla = ? and id <> ?");
                qt.Query(connection, sigla, id);
                if (qt.Rows.Count > 0)
                {
                    return "Já existe esta sigla cadastrada.";
                }
            }
            finally
            {
                connection.Close();
            }
            return "";
        }
    }
}
