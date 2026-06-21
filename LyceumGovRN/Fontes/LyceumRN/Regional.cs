using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class Regional : RNBase
    {
        public DataTable RetornarRegionalPor(int polo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = new DataTable();

            try
            {
                contextQuery.Command = @"SELECT POLO, REGIONAL FROM REGIONAL WHERE POLO = @POLO";

                contextQuery.Parameters.Add("@POLO", polo);

                dt = ctx.GetDataTable(contextQuery);

            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();

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
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;
        }

        public static TceRegional Carregar(int idRegional)
        {
            var regional = new TceRegional();

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT  ID_REGIONAL, REGIONAL, R.CEP, R.MUNICIPIO, LOGRADOURO, NUMERO,
                                        COMPLEMENTO, BAIRRO, MATRICULA, DT_CADASTRO, DT_ALTERACAO,M.NOME,M.UF_SIGLA
                                FROM    TCE_REGIONAL R
                                LEFT JOIN MUNICIPIO M ON M.CODIGO=R.MUNICIPIO
                                WHERE   ID_REGIONAL = @ID_REGIONAL "
                };
                contextQuery.Parameters.Add("@ID_REGIONAL", idRegional);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    if (reader.Read())
                    {
                        regional.IdRegional = idRegional;
                        regional.Regional = Convert.ToString(reader["REGIONAL"]);
                        regional.Cep = Convert.ToString(reader["CEP"]);
                        regional.Municipio = Convert.ToString(reader["MUNICIPIO"]);
                        regional.Logradouro = Convert.ToString(reader["LOGRADOURO"]);
                        regional.Numero = Convert.ToString(reader["NUMERO"]);
                        regional.Complemento = Convert.ToString(reader["COMPLEMENTO"]);
                        regional.Bairro = Convert.ToString(reader["BAIRRO"]);
                        regional.Matricula = Convert.ToString(reader["MATRICULA"]);
                        regional.DtCadastro = Convert.ToDateTime(reader["DT_CADASTRO"]);
                        regional.NomeMunicipio = Convert.ToString(reader["NOME"]);
                        regional.Uf  = Convert.ToString(reader["UF_SIGLA"]);
                        
                        if (reader["DT_ALTERACAO"] != DBNull.Value)
                        {
                            regional.DtAlteracao = Convert.ToDateTime(reader["DT_ALTERACAO"]);
                        }
                    }
                }
                return regional;
            }
        }

        public static DataTable Listar()
        {
            var contextQuery = new ContextQuery(
                @" SELECT * from TCE_REGIONAL R
                    LEFT JOIN MUNICIPIO M ON M.CODIGO=R.MUNICIPIO
                    order by REGIONAL");

            return Consultar(contextQuery);
        }

        public static DataTable Listar(int regional)
        {
            var contextQuery = new ContextQuery(
                @" SELECT * from TCE_REGIONAL R
                    LEFT JOIN MUNICIPIO M ON M.CODIGO=R.MUNICIPIO
                    WHERE ID_REGIONAL = @ID_REGIONAL
                    order by REGIONAL");
            contextQuery.Parameters.Add("@ID_REGIONAL", regional);

            return Consultar(contextQuery);
        }

        public static ValidacaoDados Validar(TceRegional regional)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (regional == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(regional.Regional))
            {
                mensagens.Add("O campo NOME DA REGIONAL é obrigatório!");
            }          

            if (string.IsNullOrEmpty(regional.Cep))
            {
                mensagens.Add("O campo CEP é obrigatório!");
            }

            if (!string.IsNullOrEmpty(regional.Cep))
            {
                var cep = Utils.RetirarMascara(regional.Cep);

                if (!Validacao.ValidarCEP(cep))
                {
                    mensagens.Add("CEP inválido! Este CEP não foi encontrado em nossa base.");
                }
            }

            if (string.IsNullOrEmpty(regional.Municipio))
            {
                mensagens.Add("O campo MUNICÍPIO é obrigatório!");
            }

            if (string.IsNullOrEmpty(regional.Logradouro))
            {
                mensagens.Add("O campo LOGRADOURO é obrigatório!");
            }

            if (string.IsNullOrEmpty(regional.Numero))
            {
                mensagens.Add("O campo NÚMERO é obrigatório!");
            }

            if (mensagens.Count == 0 && regional.IdRegional <= 0)
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    var contextQuery = new ContextQuery(
                        @"SELECT  1
                            FROM    TCE_REGIONAL
                            WHERE   REGIONAL = @REGIONAL
                                    ");

                    contextQuery.Parameters.Add("@REGIONAL", regional.Regional);
                    
                    var obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("Já existe uma DIRETORIA REGIONAL cadastrada com este mesmo nome.");
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public static void Inserir(TceRegional regional)
        {
            var contextQuery = new ContextQuery(
                @" INSERT  INTO TCE_REGIONAL ( REGIONAL, CEP, MUNICIPIO, LOGRADOURO, NUMERO,
                                                COMPLEMENTO, BAIRRO, MATRICULA )
                    VALUES  ( @REGIONAL, @CEP, @MUNICIPIO, @LOGRADOURO,  @NUMERO,@COMPLEMENTO,
                              @BAIRRO, @MATRICULA ) ");

            contextQuery.Parameters.Add("@REGIONAL", regional.Regional);
            contextQuery.Parameters.Add("@CEP", regional.Cep);
            contextQuery.Parameters.Add("@MUNICIPIO", regional.Municipio);
            contextQuery.Parameters.Add("@LOGRADOURO", regional.Logradouro);
            contextQuery.Parameters.Add("@NUMERO", regional.Numero);
            contextQuery.Parameters.Add("@COMPLEMENTO", regional.Complemento);
            contextQuery.Parameters.Add("@BAIRRO", regional.Bairro);
            contextQuery.Parameters.Add("@MATRICULA", regional.Matricula);

            ExecutarAlteracao(contextQuery);
        }

        public static void Alterar(TceRegional regional)
        {
            var contextQuery = new ContextQuery(
            @" UPDATE  TCE_REGIONAL
                SET     REGIONAL = @REGIONAL,                         
                        CEP = @CEP,
                        MUNICIPIO = @MUNICIPIO, 
                        LOGRADOURO = @LOGRADOURO,
                        NUMERO = @NUMERO,
                        COMPLEMENTO = @COMPLEMENTO, 
                        BAIRRO = @BAIRRO, 
                        MATRICULA = @MATRICULA,
                        DT_ALTERACAO = GETDATE()
                WHERE   ID_REGIONAL = @ID_REGIONAL ");

            contextQuery.Parameters.Add("@ID_REGIONAL", regional.IdRegional);
            contextQuery.Parameters.Add("@REGIONAL", regional.Regional);
            contextQuery.Parameters.Add("@CEP", regional.Cep);
            contextQuery.Parameters.Add("@MUNICIPIO", regional.Municipio);
            contextQuery.Parameters.Add("@LOGRADOURO", regional.Logradouro);
            contextQuery.Parameters.Add("@NUMERO", regional.Numero);
            contextQuery.Parameters.Add("@COMPLEMENTO", regional.Complemento);
            contextQuery.Parameters.Add("@BAIRRO", regional.Bairro);
            contextQuery.Parameters.Add("@MATRICULA", regional.Matricula);

            ExecutarAlteracao(contextQuery);
        }

        public static ValidacaoDados ValidarRemover(int id)
        {
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (id == 0)
            {
                return validacaoDados;
            }

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT  1
                    FROM    dbo.LY_UNIDADE_ENSINO
                    WHERE   ID_REGIONAL = @ID_REGIONAL");

                contextQuery.Parameters.Add("@ID_REGIONAL", id);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    validacaoDados.Mensagem = "Não é permitido realizar a exclusão desta regional, pois ela está sendo utilizada.";
                }
            }

            if (string.IsNullOrEmpty(validacaoDados.Mensagem))
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public static void Remover(int id)
        {
            if (id < 1)
            {
                return;
            }

            var contextQuery = new ContextQuery(
                @"DELETE  TCE_REGIONAL
                    WHERE   ID_REGIONAL = @ID_REGIONAL");

            contextQuery.Parameters.Add("@ID_REGIONAL", id);

            ExecutarAlteracao(contextQuery);
        }

        public DataTable ListaRegionalPor(string municipio)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable regionais = null;

            try
            {


                contextQuery.Command = @" SELECT DISTINCT ID_REGIONAL                                   
                            FROM VW_UNIDADE_ENSINO_SITUACAO 
                            WHERE MUNICIPIO=@MUNICIPIO
                            AND ID_REGIONAL IS NOT NULL";

                contextQuery.Parameters.Add("@MUNICIPIO", municipio);

                regionais = contexto.GetDataTable(contextQuery);

            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }

            return regionais;
        }

        public bool EhMunicipioPertencentePor(int regional, string municipio)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool pertence = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                        FROM    VW_UNIDADE_ENSINO_SITUACAO
                                        WHERE   SITUACAO = 'ESTADUAL'
                                                AND ID_REGIONAL = @ID_REGIONAL
                                                AND MUNICIPIO = @MUNICIPIO ";

                contextQuery.Parameters.Add("@ID_REGIONAL", regional);
                contextQuery.Parameters.Add("@MUNICIPIO", municipio);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    pertence = true;
                }

                return pertence;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
                ctx.Dispose();
            }
        }
    }
}
