namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Seeduc.Infra.Data;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Lyceum.RN.Util;

    public class Municipio : RNBase
    {
        public static DataTable Listar(int ano)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT * FROM    MUNICIPIO");

                return ctx.GetDataTable(contextQuery);
            }
        }
        public static bool ValidarMunicipio(string municipio)
        {
            var sql = @"SELECT  1
                        FROM    MUNICIPIO 
                        WHERE   CODIGO = ?";

            var retorno = ExecutarFuncao(sql, municipio);

            return retorno == 1;
        }

        public static DataTable ListarUF()
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT distinct UF_SIGLA FROM    MUNICIPIO WHERE UF_SIGLA <> '00' order by UF_SIGLA");

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarMunicipioUF(string UF)
        {
            var contextQuery = new ContextQuery
                        (@" SELECT distinct CODIGO, NOME FROM    MUNICIPIO where UF_SIGLA =@UF_SIGLA "
                            );
            contextQuery.Parameters.Add("@UF_SIGLA", UF);

            return Consultar(contextQuery);
        }

        public static DataTable ListarMunicipioUFLimitrofe(string UF, string MunicipioOrigem)
        {
            var contextQuery = new ContextQuery
                        (@" SELECT distinct CODIGO, NOME FROM    MUNICIPIO where UF_SIGLA =@UF_SIGLA AND CODIGO <> @CODIGO"
                            );
            contextQuery.Parameters.Add("@UF_SIGLA", UF);
            contextQuery.Parameters.Add("@CODIGO", MunicipioOrigem);

            return Consultar(contextQuery);
        }

        public static DataTable ListarMunicipio(string UF)
        {
            var contextQuery = new ContextQuery
                        (@" SELECT distinct CODIGO, NOME FROM    MUNICIPIO where UF_SIGLA =@UF_SIGLA "
                            );
            contextQuery.Parameters.Add("@UF_SIGLA", UF);

            return Consultar(contextQuery);
        }

        public static TceMunicipio ObtemPor(string codigoMunicipio)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(String.Format(@"SELECT * FROM MUNICIPIO WHERE CODIGO = '{0}'",codigoMunicipio));

                return ctx.TryToBindEntity<TceMunicipio>(contextQuery);
            }
        }

        public static TceMunicipio ObtemPorDePara(string codigoSeeduc)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(String.Format(@"select m.* 
                                                                    from DEPARA_MUNIC dm (nolock)    
	                                                                     inner join MUNICIPIO m (nolock)
			                                                                    on dm.COD_LYCEUM = m.CODIGO
                                                                    where COD_SEEDUC = '{0}'", codigoSeeduc));
                return ctx.TryToBindEntity<TceMunicipio>(contextQuery);
            }
        }

        public DataTable ListaPor(string uf)
        {
            DataContext contexto = DataContextBuilder.FromHades.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT distinct CODIGO, 
                                            NOME 
                                            FROM    MUNICIPIO 
                                            where UF_SIGLA =@UF_SIGLA
                                            ORDER BY NOME ";

                contextQuery.Parameters.Add("@UF_SIGLA", SqlDbType.VarChar, uf);

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }

            return dt;
        }

        public static DataTable ListarTodos()
        {
            var contextQuery = new ContextQuery(
                @"SELECT CODIGO, NOME 
          FROM MUNICIPIO
          ORDER BY NOME");

            return Consultar(contextQuery);
        }
    }
}
