using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.DTOs;
using Seeduc.Infra.Data;
using Seeduc.Infra.Extensions;
using Seeduc.Infra.Validation;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Techne.Lyceum.RN.Servicos;
using Techne.Data;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN
{
    public class FotoPessoa : RNBase
    {
        public static bool ExisteFoto(decimal pessoa)
        {
            var cn = Config.CreateConnection();
            var qt = new QueryTable("select pessoa from ly_foto_pessoa where pessoa = ?");

            qt.Query(cn, pessoa);

            if (qt.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }

        public static RetValue Incluir(TConnectionWritable connection, Ly_foto_pessoa dtFotoPessoa)
        {
            RetValue retorno = null;

            if (dtFotoPessoa != null)
            {
                if (dtFotoPessoa.Rows != null)
                {
                    Ly_foto_pessoa.Row.Insert(connection, dtFotoPessoa.Rows[0].Foto, dtFotoPessoa.Rows[0].Pessoa);

                    retorno = VerificarErro(connection.GetErrors());
                }
            }

            return retorno;
        }

        public static RetValue Excluir(TConnectionWritable connection, Ly_foto_pessoa.Row dadosFotoPessoa)
        {
            // Consulta o datatable para obter todos os docentes com o código informado
            var dtFotoPessoa = Ly_foto_pessoa.Query(connection, "pessoa = ?", dadosFotoPessoa.Pessoa);

            // Verifica se o datatable do docente não é nulo
            if (dtFotoPessoa != null)
            {
                // Verifica se existem linhas no datatable do docente
                if (dtFotoPessoa.Rows != null)
                {
                    foreach (Ly_foto_pessoa.Row linhaFotoPessoa in dtFotoPessoa.Rows)
                    {
                        // Marca que alinha deve ser excluida
                        linhaFotoPessoa.Delete();
                    }

                    // Chamar Put para efetuar a exclusao
                    dtFotoPessoa.Put(connection);
                }
            }

            return VerificarErro(connection.GetErrors());
        }

        public static RetValue Excluir(TConnectionWritable connection, decimal? pessoa)
        {
            // Consulta o datatable para obter todos os docentes com o código informado
            var dtFotoPessoa = Ly_foto_pessoa.Query(connection, "pessoa = ?", pessoa);

            // Verifica se o datatable do docente não é nulo
            if (dtFotoPessoa != null)
            {
                // Verifica se existem linhas no datatable do docente
                if (dtFotoPessoa.Rows != null)
                {
                    foreach (Ly_foto_pessoa.Row linhaFotoPessoa in dtFotoPessoa.Rows)
                    {
                        // Marca que alinha deve ser excluida
                        linhaFotoPessoa.Delete();
                    }

                    // Chamar Put para efetuar a exclusao
                    dtFotoPessoa.Put(connection);
                }
            }

            return VerificarErro(connection.GetErrors());
        }

        public static Ly_foto_pessoa.Row Consultar(string pessoa)
        {
            var connection = Config.CreateConnection();

            connection.Open();

            try
            {
                var parametros = new object[] { pessoa };

                return Ly_foto_pessoa.QueryFirstRow(connection, "pessoa = ?", parametros);
            }
            finally
            {
                connection.Close();
            }
        }       

        public static string ValidaFoto(byte[] foto)
        {
            var ms = new MemoryStream(foto);
            var imagem = System.Drawing.Image.FromStream(ms);
            List<string> mensagens = new List<string>();

            if (foto != null)
            {

                if (!System.Drawing.Imaging.ImageFormat.Jpeg.Equals(imagem.RawFormat))
                {
                    mensagens.Add("Formato da FOTO Inválido!");
                }

                //Verifica tamanho do arquivo - Todas as fotos devem ter entre 8 e 32Kb
                int tamanhoByte = Buffer.ByteLength(foto);
                if (tamanhoByte < 8192 || tamanhoByte > 32768)
                {
                    mensagens.Add("A foto devem ter tamanho entre 8 e 32Kb.");
                }

                // As fotos precisam ser quadradas, com no máximo 400px de largura.
                int width = imagem.Width;
                int height = imagem.Height;
                if (width != height)
                {
                    mensagens.Add("A foto precisa ser quadrada.");
                }
                else if (width > 400)
                {
                    mensagens.Add("A foto precisa ter no máximo 400px de largura.");
                }

                if (mensagens.Count > 0)
                {
                    return mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
                }
            }

            return string.Empty;
        }

        public static string ValidaFoto(decimal pessoa)
        {
            var foto = SelecionarFoto(pessoa);

            if (foto == null)
            {
                return "Aluno não possui foto.";
            }

            return ValidaFoto(foto);
        }

        public static bool VerificarFoto(decimal pessoa)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                                       {
                                           Command =
                                               @"SELECT    1
                              FROM      LY_FOTO_PESSOA
                              WHERE     PESSOA = @PESSOA"
                                       };
                contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);

                object obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    return true;
                }
            }
            return false;
        }

        private static byte[] SelecionarFoto(decimal pessoa)
        {
            var foto = ExecutarFuncaoScalar("select foto from ly_foto_pessoa fp where fp.pessoa  = ?", pessoa);

            if (!foto.IsNull)
            {
                return (byte[])foto;
            }

            return null;
        }

        public static int Inserir(LyFotoPessoa foto, DataContext ctx)
        {
            try
            {
                var contextQuery = new ContextQuery
                                       {
                                           Command =
                                               @"INSERT INTO LY_FOTO_PESSOA(Pessoa,Foto) 
                         VALUES (@Pessoa,@Foto) "
                                       };
                contextQuery.Parameters.Add("@Pessoa", TechneDbType.T_NUMERO, foto.Pessoa);
                contextQuery.Parameters.Add("@Foto", SqlDbType.Image, foto.Foto);


                return ctx.ApplyModifications(contextQuery);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static int Alterar(LyFotoPessoa foto, DataContext ctx)
        {
            try
            {
                var contextQuery = new ContextQuery
                                       {
                                           Command =
                                               @"UPDATE LY_FOTO_PESSOA SET 
                                FOTO =@Foto
                          WHERE PESSOA = @Pessoa "
                                       };

                contextQuery.Parameters.Add("@Pessoa", TechneDbType.T_NUMERO, foto.Pessoa);
                contextQuery.Parameters.Add("@Foto", SqlDbType.Image, foto.Foto);

                return ctx.ApplyModifications(contextQuery);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static LyFotoPessoa Carregar(int idPessoa)
        {
            var foto = new LyFotoPessoa();

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery
                                       {
                                           Command =
                                               @"SELECT  foto
                                FROM    ly_foto_pessoa fp
                                WHERE   fp.pessoa = @pessoa "
                                       };
                contextQuery.Parameters.Add("@pessoa", idPessoa);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    if (reader.Read())
                    {
                        foto.Pessoa = idPessoa;
                        foto.Foto = (byte[])(reader["foto"]);
                    }
                }
                return foto;
            }
        }

        public static int Remover(decimal idPessoa, DataContext ctx)
        {
            try
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @"DELETE  FROM LY_FOTO_PESSOA
                            WHERE   PESSOA = @PESSOA "
                };
                contextQuery.Parameters.Add("@Pessoa", TechneDbType.T_NUMERO, idPessoa);

                return ctx.ApplyModifications(contextQuery);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}