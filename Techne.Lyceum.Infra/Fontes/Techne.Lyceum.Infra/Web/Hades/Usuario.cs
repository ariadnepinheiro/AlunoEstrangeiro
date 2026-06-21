using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Techne.Web.Hades
{
    internal class Usuario
    {
        internal static HdUsuario BuscaUsuario(string usuario)
        {
            HdUsuario u = null;
            var sql = "select * from hd_usuario (NOLOCK) where usuario=? && habilitado='S'";
            var dt = HadesUtil.ExecuteQuery(sql, new object[] { usuario });
            if (dt.Rows.Count > 0)
            {
                u = new HdUsuario();
                u.Usuario = dt.Rows[0]["USUARIO"] as string;
                u.Email = dt.Rows[0]["EMAIL"] as string;
                u.Nome = dt.Rows[0]["NOME"] as string;
                u.Habilitado = "S".Equals(string.Empty + dt.Rows[0]["NOME"], StringComparison.InvariantCultureIgnoreCase);
            }

            return u;
        }

        internal static string HdPal(string text)
        {
            var b = _HdPal(text);

            if (b.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                return Encoding.GetEncoding(1252).GetString(b);
            }
        }

        internal static bool UsuarioValido(string usuario, string senha)
        {
            var sql = "select usuario, senha, habilitado from hd_usuario (NOLOCK) where usuario = ?";
            var dt = HadesUtil.ExecuteQuery(sql, new object[] { usuario });
            if (dt == null || dt.Rows.Count == 0)
            {
                return false;
            }

            var habilitado = string.Empty + dt.Rows[0]["habilitado"];
            var pwdCodificada = HdPal(senha);
            var pwdUsuario = string.Empty + dt.Rows[0]["senha"];
            return habilitado.Equals("S", StringComparison.InvariantCultureIgnoreCase) && (pwdCodificada == pwdUsuario);
        }

        private static byte[] _HdPal(string text)
        {
            const string h = "TECHNELYCEUM";
            if (text == null || text == string.Empty)
            {
                return new byte[0];
            }

            if (text.Length > 30)
            {
                text = text.Substring(0, 30);
            }
            else
            {
                text = text.PadRight(30, ' ');
            }

            var WSenha = new byte[30];
            byte WOffset = 0;
            var j = 0;

            var enc = Encoding.GetEncoding(1252);
            for (var i = 0; i < 30; i++)
            {
                var a = enc.GetBytes(text.Substring(i, 1))[0];
                var b = enc.GetBytes(h.Substring(j, 1))[0];
                var cod = Convert.ToByte((a + WOffset + b) & 255);
                WSenha[i] = cod;
                WOffset = Convert.ToByte((WOffset + b) & 255);
                if (++j >= h.Length)
                {
                    j = 0;
                }
            }

            return WSenha;
        }
    }
}