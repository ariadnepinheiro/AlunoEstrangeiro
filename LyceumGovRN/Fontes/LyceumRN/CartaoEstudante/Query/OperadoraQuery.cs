using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Entity;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class OperadoraQuery: QueryBase<OperadoraQuery>
    {
        OperadoraQuery() {
            NomeTabela = "Lyceum.CartaoEstudante.Operadora";
        }

        public Operadora ObtemOperadoraPor(string nomeOperadora)
        {
            string SELECT_OPERADORA = "SELECT * FROM " + NomeTabela + " WHERE UPPER(NOMEOPERADORA) = UPPER(@NOMEOPERADORA)";

            Operadora operadora = ObtemPor<Operadora>(
                SELECT_OPERADORA,
                nomeOperadora
            );

            return operadora;
        }

        public bool ExisteOperadora(int codigoOperadora)
        {
            string SELECT_OPERADORA = "SELECT 1 FROM " + NomeTabela + " WHERE OPERADORAID = @OPERADORAID";
            return Possui(SELECT_OPERADORA, codigoOperadora);            
        }
    }
}
