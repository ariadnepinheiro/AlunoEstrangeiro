using System;
using System.Data;
using System.Globalization;
using Seeduc.Infra.Data;
using Techne.Data;
using Techne.Library;
using Techne.HadesLyc.CR;
using System.Web.Security;

namespace Techne.Lyceum.RN
{
    public class Senha : RNBase
    {
        public static RetValue AlterarSenha(Hd_usuario.Row linha)
        {
            TConnectionWritable connection = Techne.HadesLyc.Config.CreateWritableConnection();
            connection.Open(true);
            RetValue retorno = null;
            try
            {
                if (linha != null)
                {
                    Hd_usuario.Row.Update(connection, linha.Usuario, "Data_alteracao_senha, Senha", linha.Data_alteracao_senha, linha.Senha);
                    retorno = VerificarErro(connection.GetErrors());
                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                    retorno = new RetValue(true, "Senha alterada com sucesso.", null);
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        public static Hd_usuario.Row Consultar(string usuario)
        {
            TConnectionWritable connection = Techne.HadesLyc.Config.CreateWritableConnection();
            connection.Open(true);
            try
            {
                Hd_usuario.Row dadosUsuario = Hd_usuario.QueryFirstRow(connection, "usuario = ?", usuario);

                return dadosUsuario;
            }
            finally
            {
                connection.Close();
            }
        }

        public static RetValue AlterarSenha(Hd_usuario.Row rowUsuario, TConnectionWritable connection)
        {
            RetValue retorno = null;

            if (rowUsuario != null)
            {
                QueryTable qt = new QueryTable(@"declare @senha varchar(30)
                                                set @senha = dbo.CRYPT(?)
                                                select @senha senha");
                qt.Query(Config.CreateWritableConnection(), rowUsuario.Senha);
                string senha;
                if (qt.Rows.Count > 0)
                    senha = qt.Rows[0]["senha"].ToString();
                else
                    senha = rowUsuario.Senha;
                //ColunasTable colunas = MontarParametros(dtusuario);
                Hd_usuario.Row.Update(connection, rowUsuario.Usuario, "Data_alteracao_senha, Senha", DateTime.Today, senha);

                retorno = VerificarErro(connection.GetErrors());

                if (retorno != null && !retorno.Ok)
                {
                    connection.Rollback();
                    return retorno;
                }
                retorno = new RetValue(true, "Senha alterada com sucesso.", null);
            }

            return retorno;
        }

        public static Hd_usuario.Row Consultar(string usuario, TConnectionWritable connection)
        {

            Hd_usuario.Row dadosUsuario = Hd_usuario.QueryFirstRow(connection, "usuario = ?", usuario);

            return dadosUsuario;
        }

        public static string GerarSenhaNumerica(int valorMaximo)
        {
            var r = new Random(DateTime.Now.Millisecond);
            var senha = r.Next(0, valorMaximo);
            return senha.ToString().PadLeft(6, '0');
        }
    }
}

