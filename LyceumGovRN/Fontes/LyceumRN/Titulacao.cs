using System;
using Techne.Data;

namespace Techne.Lyceum.RN
{
    public class Titulacao : RNBase
    {
        /// <summary>
        /// Verifica se existe titulação informada.
        /// </summary>
        /// <param name="titulacao">titulação</param>
        /// <returns>lista de possíveis titulações</returns>
        public static QueryTable VerificarTitulacao(string titulacao)
        {
            string sql = " SELECT top 1 1 FROM LY_TITULACAO  WHERE TITULACAO = ? ";
            return RNBase.Consultar(sql, titulacao);
        }

    }
}
