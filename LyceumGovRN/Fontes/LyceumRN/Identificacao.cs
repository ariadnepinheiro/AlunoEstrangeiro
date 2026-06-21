using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN
{
    public class Identificacao : RNBase
    {
        public static string ConsultarPadaces(string usuario)
        {
            string sql = "select PADACES from PADUSUARIO where USUARIO = ? ";
            return ConsultarCampo(sql, usuario);
        }

        public static string ConsultarPaginaInicial(string usuario)
        {
            string padaces = ConsultarPadaces(usuario);
            if (!string.IsNullOrEmpty(padaces))
            {
                string sql = "select pagina_inicial from hd_padaces where padaces = ? ";
                return ConsultarCampoHades(sql, padaces);
            }
            return null;
        }

    }
}
