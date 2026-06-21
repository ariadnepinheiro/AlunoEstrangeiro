using System;
using System.Data;
using System.Globalization;
using Techne.Data;
using Techne.Library;
using Techne.HadesLyc.CR;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class Setores : RNBase
    {
        public void Insere(Hd_setor dtSetor)
        {
            DataContext contexto = DataContextBuilder.FromHades.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            Hd_setor.Row dadosSetor = dtSetor.Rows[0];

            try
            {
                contextQuery.Command = @" INSERT INTO dbo.HD_SETOR
                                       (SETOR
                                       ,NOME
                                       ,SETORPAI
                                       ,TIPO_SETOR
                                       ,DTINI
                                       ,DTFIM
                                       ,NUMEROLOG
                                       ,COMPLEMENTO
                                       ,FONE
                                       ,FAX
                                       ,ATIVO
                                       ,MUNICIPIO
                                       ,BAIRRO
                                       ,LOGRADOURO
                                       ,TRECHO_LOGR
                                       ,CNPJ
                                       ,PAIS
                                       ,CEP
                                       ,STAMP_ATUALIZACAO
                                       ,OBSERVACAO
                                       ,NOVOSETOR)
                                 VALUES
                                       (@SETOR, 
                                       @NOME, 
                                       @SETORPAI, 
                                       @TIPO_SETOR, 
                                       @DTINI, 
                                       @DTFIM, 
                                       @NUMEROLOG, 
                                       @COMPLEMENTO, 
                                       @FONE, 
                                       @FAX, 
                                       @ATIVO, 
                                       @MUNICIPIO, 
                                       @BAIRRO, 
                                       @LOGRADOURO, 
                                       @TRECHO_LOGR, 
                                       @CNPJ, 
                                       @PAIS, 
                                       @CEP, 
                                       @STAMP_ATUALIZACAO, 
                                       @OBSERVACAO, 
                                       @NOVOSETOR) ";

                contextQuery.Parameters.Add("@SETOR", dadosSetor.Setor);
                contextQuery.Parameters.Add("@NOME", dadosSetor.Nome);
                contextQuery.Parameters.Add("@SETORPAI", dadosSetor.Setorpai);
                contextQuery.Parameters.Add("@TIPO_SETOR", dadosSetor.Tipo_setor);
                contextQuery.Parameters.Add("@DTINI", dadosSetor.Dtini);
                contextQuery.Parameters.Add("@DTFIM", dadosSetor.Dtfim);
                contextQuery.Parameters.Add("@NUMEROLOG", dadosSetor.Numerolog);
                contextQuery.Parameters.Add("@COMPLEMENTO", dadosSetor.Complemento);
                contextQuery.Parameters.Add("@FONE", dadosSetor.Fone);
                contextQuery.Parameters.Add("@FAX", dadosSetor.Fax);
                contextQuery.Parameters.Add("@ATIVO", dadosSetor.Ativo);
                contextQuery.Parameters.Add("@MUNICIPIO", dadosSetor.Municipio);
                contextQuery.Parameters.Add("@BAIRRO", dadosSetor.Bairro);
                contextQuery.Parameters.Add("@LOGRADOURO", dadosSetor.Logradouro);
                contextQuery.Parameters.Add("@TRECHO_LOGR", dadosSetor.Trecho_logr);
                contextQuery.Parameters.Add("@CNPJ", dadosSetor.Cnpj);
                contextQuery.Parameters.Add("@PAIS", dadosSetor.Pais);
                contextQuery.Parameters.Add("@CEP", dadosSetor.Cep);
                contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", DateTime.Now);
                contextQuery.Parameters.Add("@OBSERVACAO", dadosSetor.Observacao);
                contextQuery.Parameters.Add("@NOVOSETOR", dadosSetor.novosetor);

                contexto.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
        }

        public static int ObtemProximoCodigo()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT MAX(CONVERT(DECIMAL,S.SETOR)) + 1 AS PROXIMOSETOR
                                          FROM HADES..HD_SETOR S
                                          WHERE S.SETOR LIKE '2%' 
                                             and S.SETOR not in ('292351','292331','292001','253100','240004')--Cadastros fora da ordem ";

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["PROXIMOSETOR"]);
                }

                return retorno;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }

        public static RetValue Incluir(Hd_setor dtSetor)
        {
            TConnectionWritable connection = Techne.HadesLyc.Config.CreateWritableConnection();
            connection.Open(true);

            RetValue retorno = null;
            try
            {
                if (dtSetor != null)
                {
                    if (dtSetor.Rows != null)
                    {
                        //Verifica se foi informada uma UA Antiga para ser utilizada como setor
                        if (dtSetor.Rows[0].Setor.ToString().IsNullOrEmptyOrWhiteSpace())
                        {
                            //Caso não tenha busca proxima UA
                            int setor = ObtemProximoCodigo();

                            //Verifica se a ua escolhida ja foi utilizada
                            if (ExisteSetor(Convert.ToString(setor)))
                            {
                                //Busca proxima outra UA
                                setor = setor + 1;
                            }

                            dtSetor.Rows[0].Setor = Convert.ToString(setor);
                        }

                        ColunasTable colunas = MontarParametros(dtSetor.Columns, dtSetor.Rows[0]);
                        Hd_setor.Row.Insert(connection, dtSetor.Rows[0].Setor, dtSetor.Rows[0].Nome, colunas.Colunas, colunas.ValorColuna);

                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }

                        retorno = new RetValue(true, "Registro incluido com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }

        public static RetValue IncluirLogradouro(string logradouro, string municipio)
        {
            TConnectionWritable connection = Techne.HadesLyc.Config.CreateWritableConnection();
            connection.Open(true);
            string codigo = null;
            decimal cod = 0;

            RetValue retorno = new RetValue(false, null, null);
            try
            {
                QueryTable cod_logradouro = ConsultarCodigoLogradouro();
                if (cod_logradouro.Rows.Count > 0)
                {
                    string dados = cod_logradouro.Rows[0].ToString();
                    char[] parametros = new char[] { ':' };
                    string[] dadosLogradouro = dados.Split(parametros, 2, StringSplitOptions.None);
                    if (dadosLogradouro[1].ToString() != " ")
                        cod = Convert.ToDecimal(dadosLogradouro[1]);
                    //int cod = Convert.ToInt16(cod_logradouro.Rows[0].ToString()) + 1;
                    cod = cod + 1;
                    codigo = cod.ToString();
                }
                else
                {
                    retorno = new RetValue(false, "Erro", null);
                    //connection.Close();
                    return retorno;
                }
                Hd_logradouro.Row.Insert(connection, municipio, codigo, "081", logradouro, "S");

                retorno = VerificarErro(connection.GetErrors());

                if (retorno != null && !retorno.Ok)
                {
                    connection.Rollback();
                    return retorno;
                }

                retorno = new RetValue(true, codigo, null);
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }

        public static RetValue IncluirBairro(string bairro, string municipio)
        {
            TConnectionWritable connection = Techne.HadesLyc.Config.CreateWritableConnection();
            connection.Open(true);
            string codigo = null;
            decimal cod = 0;

            RetValue retorno = new RetValue(false, null, null);
            try
            {
                QueryTable cod_bairro = ConsultarCodigoBairro();
                if (cod_bairro.Rows.Count > 0)
                {
                    string dados = cod_bairro.Rows[0].ToString();
                    char[] parametros = new char[] { ':' };
                    string[] dadosBairro = dados.Split(parametros, 2, StringSplitOptions.None);
                    if (dadosBairro[1].ToString() != " ")
                        cod = Convert.ToDecimal(dadosBairro[1]);
                    //int cod = Convert.ToInt16(cod_logradouro.Rows[0].ToString()) + 1;
                    cod = cod + 1;
                    //cod = Convert.ToInt16(cod_bairro.Rows[0].ToString()) + 1;
                    codigo = cod.ToString();
                }
                else
                {
                    retorno = new RetValue(false, "Erro", null);
                    //connection.Close();
                    return retorno;
                }
                Hd_bairro.Row.Insert(connection, municipio, codigo, bairro);

                retorno = VerificarErro(connection.GetErrors());

                if (retorno != null && !retorno.Ok)
                {
                    connection.Rollback();
                    return retorno;
                }

                retorno = new RetValue(true, codigo, null);
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }


        public static RetValue Alterar(Hd_setor dtSetor)
        {
            RetValue retorno = null;
            //string endereco = null;
            //string bairro = null;

            TConnectionWritable connection = Techne.HadesLyc.Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                if (dtSetor != null)
                {
                    if (dtSetor.Rows != null)
                    {
                        //Hd_setor.Row dadosSetor = dtSetor.Rows[0];
                        //if (!string.IsNullOrEmpty(dadosSetor.Logradouro.ToString()))
                        //{
                        //    if (!string.IsNullOrEmpty(dadosSetor.Municipio.ToString()))
                        //    {
                        //        endereco = ConsultarLogradouro(permission, dadosSetor.Logradouro.ToString(), dadosSetor.Municipio.ToString());
                        //        if (endereco == "Erro")
                        //        {
                        //            retorno = new RetValue(false, "Erro no endereço", null);
                        //            return retorno;
                        //        }
                        //        else
                        //        {
                        //            if (!string.IsNullOrEmpty(endereco))
                        //            {
                        //                string dados = endereco;
                        //                char[] parametros = new char[] { ':' };
                        //                string[] dadosEndereco = dados.Split(parametros, 2, StringSplitOptions.None);
                        //                if (dadosEndereco.Length > 1 && !string.IsNullOrEmpty(dadosEndereco[1].ToString()))
                        //                    endereco = dadosEndereco[1].ToString().Trim();
                        //                else
                        //                    endereco = dadosEndereco[0].ToString().Trim();
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        dadosSetor.Municipio = "00000000";
                        //        endereco = ConsultarLogradouro(permission, dadosSetor.Logradouro.ToString(), dadosSetor.Municipio.ToString());
                        //        if (endereco == "Erro")
                        //        {
                        //            retorno = new RetValue(false, "Erro no endereço", null);
                        //            return retorno;
                        //        }
                        //        else
                        //        {
                        //            if (!string.IsNullOrEmpty(endereco))
                        //            {
                        //                string dados = endereco;
                        //                char[] parametros = new char[] { ':' };
                        //                string[] dadosEndereco = dados.Split(parametros, 2, StringSplitOptions.None);
                        //                if (dadosEndereco.Length > 1 && !string.IsNullOrEmpty(dadosEndereco[1].ToString()))
                        //                    endereco = dadosEndereco[1].ToString().Trim();
                        //                else
                        //                    endereco = dadosEndereco[0].ToString().Trim();
                        //            }
                        //        }
                        //    }
                        //}
                        //if (!string.IsNullOrEmpty(dadosSetor.Bairro.ToString()))
                        //{
                        //    if (!string.IsNullOrEmpty(dadosSetor.Municipio.ToString()))
                        //    {
                        //        bairro = ConsultarBairro(permission, dadosSetor.Bairro.ToString(), dadosSetor.Municipio.ToString());
                        //        if (bairro == "Erro")
                        //        {
                        //            retorno = new RetValue(false, "Erro no bairro", null);
                        //            return retorno;
                        //        }
                        //        else
                        //        {
                        //            if (!string.IsNullOrEmpty(bairro))
                        //            {
                        //                string dados = bairro;
                        //                char[] parametros = new char[] { ':' };
                        //                string[] dadosBairro = dados.Split(parametros, 2, StringSplitOptions.None);
                        //                if (dadosBairro.Length > 1 && !string.IsNullOrEmpty(dadosBairro[1].ToString()))
                        //                    bairro = dadosBairro[1].ToString().Trim();
                        //                else
                        //                    bairro = dadosBairro[0].ToString().Trim();
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        dadosSetor.Municipio = "00000000";
                        //        bairro = ConsultarBairro(permission, dadosSetor.Bairro.ToString(), dadosSetor.Municipio.ToString());
                        //        if (bairro == "Erro")
                        //        {
                        //            retorno = new RetValue(false, "Erro no bairro", null);
                        //            return retorno;
                        //        }
                        //        else
                        //        {
                        //            if (!string.IsNullOrEmpty(bairro))
                        //            {
                        //                string dados = bairro;
                        //                char[] parametros = new char[] { ':' };
                        //                string[] dadosBairro = dados.Split(parametros, 2, StringSplitOptions.None);
                        //                if (dadosBairro.Length > 1 && !string.IsNullOrEmpty(dadosBairro[1].ToString()))
                        //                    bairro = dadosBairro[1].ToString().Trim();
                        //                else
                        //                    bairro = dadosBairro[0].ToString().Trim();
                        //            }
                        //        }
                        //    }
                        //}
                        //dtSetor.Rows[0].Logradouro = endereco;
                        //dtSetor.Rows[0].Bairro = bairro;

                        ColunasTable colunas = MontarParametros(dtSetor.Columns, dtSetor.Rows[0]);

                        Hd_setor.Row.Update(connection, dtSetor.Rows[0].Setor, colunas.Colunas, colunas.ValorColuna);

                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }

                        retorno = new RetValue(true, "Registro alterado com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }


        public static Hd_setor.Row Consultar(string setor)
        {
            TConnectionWritable connection = Techne.HadesLyc.Config.CreateWritableConnection();

            connection.Open(true);
            try
            {
                Hd_setor.Row linha = Hd_setor.QueryFirstRow(connection, "setor= ?", setor) != null ? Hd_setor.QueryFirstRow(connection, "setor= ?", setor) : Hd_setor.QueryFirstRow(connection, "novosetor= ?", setor);

                return linha;
            }
            finally
            {
                connection.Close();
            }
        }



        public static QueryTable Consultar()
        {
            TConnectionWritable connection = Techne.HadesLyc.Config.CreateWritableConnection();
            connection.Open();
            string sql = "select setor, nome, setorpai from hd_setor ";

            QueryTable qt = null;
            qt = new QueryTable(sql);
            try
            {
                qt.Query(connection);
            }
            finally
            {
                connection.Close();
            }
            return qt;
        }

        public static string ConsultarLogradouro(string logradouro, string municipio)
        {
            TConnection connection = Techne.HadesLyc.Config.CreateConnection();
            connection.Open();
            string sql = "select logradouro from hd_logradouro where nome = ? AND municipio =? ";
            string cod_logradouro = null;

            QueryTable qt = null;
            qt = new QueryTable(sql);
            try
            {
                qt.Query(connection, logradouro, municipio);
                if (qt.Rows.Count <= 0)
                {
                    RetValue insercao = IncluirLogradouro(logradouro, municipio);
                    if (insercao.Ok == true && insercao.Message != null)
                    {
                        cod_logradouro = insercao.Message;
                    }
                    else if (insercao.Ok == false && insercao.Message == "Erro")
                    {
                        //connection.Close();
                        return "Erro";
                    }
                }
                else if (qt.Rows.Count > 0)
                {
                    cod_logradouro = qt.Rows[0].ToString();
                }
            }
            finally
            {
                connection.Close();
            }
            return cod_logradouro;
        }

        public static QueryTable ConsultarCodigoLogradouro()
        {
            TConnection connection = Techne.HadesLyc.Config.CreateConnection();

            connection.Open();
            QueryTable qt = null;

            string sql = "SELECT MAX(logradouro) AS COD_LOGRADOURO FROM hd_logradouro";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection);
            }
            finally
            {
                connection.Close();
            }


            return qt;
        }

        public static string ConsultarBairro(string bairro, string municipio)
        {
            TConnection connection = Techne.HadesLyc.Config.CreateConnection();
            connection.Open();
            string sql = "select bairro from hd_bairro where nome = ? AND municipio =? ";
            string cod_bairro = null;

            QueryTable qt = null;
            qt = new QueryTable(sql);
            try
            {
                qt.Query(connection, bairro, municipio);
                if (qt.Rows.Count <= 0)
                {
                    RetValue insercao = IncluirBairro(bairro, municipio);
                    if (insercao.Ok == true && insercao.Message != null)
                    {
                        cod_bairro = insercao.Message;
                    }
                    else if (insercao.Ok == false && insercao.Message == "Erro")
                    {
                        //connection.Close();
                        return "Erro";
                    }
                }
                else if (qt.Rows.Count > 0)
                {
                    cod_bairro = qt.Rows[0].ToString();
                }
            }
            finally
            {
                connection.Close();
            }
            return cod_bairro;
        }

        public static QueryTable ConsultarCodigoBairro()
        {
            TConnection connection = Techne.HadesLyc.Config.CreateConnection();

            connection.Open();
            QueryTable qt = null;

            string sql = "SELECT MAX(bairro) AS COD_BAIRRO FROM hd_bairro";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection);
            }
            finally
            {
                connection.Close();
            }


            return qt;
        }


        public static RetValue Excluir(string setor)
        {
            TConnectionWritable connection = Techne.HadesLyc.Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                Hd_setor dtSetor = Hd_setor.Query(connection, "setor = ?", setor);

                if (dtSetor != null)
                {
                    if (dtSetor.Rows != null)
                    {
                        foreach (Hd_setor.Row linha in dtSetor.Rows)
                        {
                            linha.Delete();
                        }

                        dtSetor.Put(connection);
                        retorno = VerificarErro(dtSetor);

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }

                        retorno = new RetValue(true, "U.A. removida com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }


        public static string ConsultaSetorUniEns(string setor)
        {
            string sql = "select unidade_ens from LY_UNIDADE_ENSINO where SETOR =  ? ";
            return ConsultarCampo(sql, setor);
        }

        public static string ConsultaSetorNucleo(string setor)
        {
            string sql = "select nucleo from LY_NUCLEO where SETOR =  ? ";
            return ConsultarCampo(sql, setor);
        }

        public static string ConsultaNucleoUniEns(string uniens)
        {

            string sql = "select nucleo from LY_UNIDADE_ENSINO where UNIDADE_ENS =  ? ";
            return ConsultarCampo(sql, uniens);

        }

        public static string ConsultaNucleoUniFis(string uniens)
        {

            string sql = "select unidade_fis from LY_UNIDADES_ASSOCIADAS where UNIDADE_ENS =  ? ";
            return ConsultarCampo(sql, uniens);

        }


        public static QueryTable ConsultarContato(string setor)
        {
            string sql = @"select L.MATRICULA, P.NOME_COMPL, P.FONE, P.CELULAR, P.E_MAIL, F.descricao from ly_lotacao L 
                inner join ly_funcao F ON F.FUNCAO = L.FUNCAO 
                INNER JOIN ly_pessoa P ON P.PESSOA = L.PESSOA 
                WHERE
                F.CAMPO_03 = 'S'
                AND L.DATA_NOMEACAO <= convert(date,GetDate()) AND (L.DATA_DESATIVACAO is null OR convert(date,L.data_desativacao) > convert(date,GetDate()))
                AND L.SETOR = ? ";

            return Consultar(sql, setor);
        }        

        public static bool ExisteSetor(string setor)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(1) 
                                            FROM HADES.dbo.HD_SETOR  
                                            where SETOR = @SETOR ";

                contextQuery.Parameters.Add("@SETOR", setor); 

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
        }

        public bool ExisteNovaUaPor(string novaUa, string setor)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(1) 
                                        FROM HADES.DBO.HD_SETOR (NOLOCK)
                                        WHERE NOVOSETOR = @NOVOSETOR
                                            AND SETOR <> @SETOR ";

                contextQuery.Parameters.Add("@NOVOSETOR", SqlDbType.VarChar, novaUa);
                contextQuery.Parameters.Add("@SETOR", SqlDbType.VarChar, setor);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
        }

        public bool EhSetorRegionalPor(string setor)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM TCE_REGIONAL 
                                        WHERE SETOR = @SETOR ";

                contextQuery.Parameters.Add("@SETOR", setor);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
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
        public string ObtemTipoSetorPor(string setor)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            string tipoSetor = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT CASE 
                                                     WHEN R.SETORID IS NOT NULL THEN 'REGIONAL' 
                                                     WHEN N.SETOR IS NOT NULL THEN 'COORDENADORIA' 
                                                     ELSE '' 
                                                   END TIPOSETOR 
                                            FROM   HADES..HD_SETOR S 
                                                   LEFT JOIN HADES..REGIONAL__SETOR R 
                                                          ON S.SETOR = R.SETORID 
                                                   LEFT JOIN LY_NUCLEO N 
                                                          ON N.SETOR = S.SETOR 
                                            WHERE  S.SETOR = @SETOR ";

                contextQuery.Parameters.Add("@SETOR", setor);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    if (reader["TIPOSETOR"] != DBNull.Value)
                    {
                        tipoSetor = Convert.ToString(reader["TIPOSETOR"]);
                    }
                }

                return tipoSetor;
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

        public DataTable ObtemDadosPor(string setor)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;


            try
            {

                contextQuery.Command = @"  SELECT T.SETOR,
                                                   T.NOME,
                                                   T.ENDERECO,
                                                   T.E_MAIL AS EMAIL,
                                                   T.FONE AS TELEFONE
                                            FROM  (SELECT S.SETOR,
                                                          UE.NOME_COMP                           AS NOME,
                                                          UE.ENDERECO + ISNULL(UE.END_COMPL, '') + ' - ' + ISNULL(UE.BAIRRO,'') AS ENDERECO,
                                                          UE.E_MAIL,
                                                          UE.FONE
                                                   FROM   HADES.DBO.HD_SETOR S
                                                          INNER JOIN LY_UNIDADE_ENSINO UE
                                                                  ON UE.SETOR = S.SETOR
                                                   UNION ALL
                                                   SELECT S.SETOR,
                                                          R.REGIONAL   AS NOME,
                                                          R.LOGRADOURO + ' - ' + ISNULL(R.BAIRRO,'') AS ENDERECO,
                                                          ''           AS EMAIL,
                                                          ''           AS TELEFONE
                                                   FROM   HADES.DBO.HD_SETOR S
														  inner join HADES.DBO.REGIONAL__SETOR rs 
																  on s.SETOR = rs.SETORID
                                                          INNER JOIN TCE_REGIONAL R
                                                                  ON R.ID_REGIONAL = rs.REGIONALID) AS T
                                                   WHERE  T.SETOR = @SETOR ";

                contextQuery.Parameters.Add("@SETOR", SqlDbType.VarChar, setor);

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

        public string ObtemSetorAtualPor(string setor)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string setorAtual = string.Empty;


            try
            {

                contextQuery.Command = @"  SELECT UA_ATUAL FROM hades..VW_SETOR
                                                   WHERE SETOR = @SETOR ";

                contextQuery.Parameters.Add("@SETOR", SqlDbType.VarChar, setor);

                setorAtual = contexto.GetReturnValue<string>(contextQuery);

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

            return setorAtual;
        }

        public string ObtemSetorPor(string setorAtual)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string setor = string.Empty;

            try
            {

                contextQuery.Command = @" SELECT SETOR FROM hades..VW_SETOR
                                                   WHERE UA_ATUAL = @UA_ATUAL ";

                contextQuery.Parameters.Add("@UA_ATUAL", SqlDbType.VarChar, setorAtual);

                setor = contexto.GetReturnValue<string>(contextQuery);

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

            return setor;
        }

        public bool PossuiSetorPor(DataContext contexto, string tipoSetor)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM hades..HD_SETOR
                                    where TIPO_SETOR = @TIPO_SETOR ";

            contextQuery.Parameters.Add("@TIPO_SETOR", tipoSetor);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public string ObtemRegionalPor(string setor)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            string regional = string.Empty;

            try
            {
                contextQuery.Command = @"  select DISTINCT  ISNULL(UE.ID_REGIONAL,RUA.ID_REGIONAL),R.REGIONAL,S.SETOR
                                             from HADES..HD_SETOR S
                                             LEFT JOIN LY_UNIDADE_ENSINO UE ON UE.SETOR=S.SETOR
                                             LEFT JOIN APOLLO..REGIONAIS_UAS RUA ON RUA.SETOR = S.SETOR
                                             INNER JOIN TCE_REGIONAL R ON R.ID_REGIONAL = ISNULL(UE.ID_REGIONAL,RUA.ID_REGIONAL)
                                             WHERE S.SETOR = @SETOR ";

                contextQuery.Parameters.Add("@SETOR", setor);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    if (reader["REGIONAL"] != DBNull.Value)
                    {
                        regional = Convert.ToString(reader["REGIONAL"]);
                    }
                }

                return regional;
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

    }
}
