using System;
using System.Data;
using System.Globalization;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using System.Linq;
using Techne.Lyceum.RN.DTOs;

namespace Techne.Lyceum.RN
{
    public class CepRN : RNBase
    {
        public DataTable ConsultarPorMunicipio(string idMunicipio)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"
                        SELECT TOP 1
                               ID_LOGRADOURO,
                               ID_MUNICIPIO,
                               CEP,
                               NOME,
                               BAIRRO
                        FROM   HADES.DBO.TCE_LOGRADOURO (NOLOCK)
                        WHERE  ID_MUNICIPIO = @ID_MUNICIPIO
                    "
                };

                contextQuery.Parameters.Add("@ID_MUNICIPIO", TechneDbType.T_CODIGO, idMunicipio);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public DataTable ListarLogradouros()
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"
                SELECT 
                       ID_LOGRADOURO,
                       ID_MUNICIPIO,
                       CEP,
                       NOME,
                       BAIRRO
                FROM HADES.DBO.TCE_LOGRADOURO (NOLOCK)
            "
                };

                return ctx.GetDataTable(contextQuery);
            }
        }

        public DataTable ListarLogradourosPorMunicipio(string idMunicipio)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"
                SELECT 
                   L.ID_LOGRADOURO,
                   M.NOME AS MUNICIPIO,
                   L.CEP,
                   L.NOME,
                   L.BAIRRO
            FROM HADES.DBO.TCE_LOGRADOURO L (NOLOCK)
            INNER JOIN LYCEUM.DBO.MUNICIPIO M (NOLOCK)
                ON L.ID_MUNICIPIO = M.CODIGO
            WHERE L.ID_MUNICIPIO = @ID_MUNICIPIO
            "
                };

                contextQuery.Parameters.Add("@ID_MUNICIPIO", TechneDbType.T_CODIGO, idMunicipio);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public void AdicionarCep(string cep, string idMunicipio, string nome, string bairro)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"
                INSERT INTO HADES.DBO.TCE_LOGRADOURO (ID_MUNICIPIO, NOME, BAIRRO, CEP)
                    VALUES (@ID_MUNICIPIO, @NOME, @BAIRRO, @CEP);
            "
                };

                contextQuery.Parameters.Add("@ID_MUNICIPIO", TechneDbType.T_CODIGO, idMunicipio);
                contextQuery.Parameters.Add("@NOME", TechneDbType.T_CODIGO, nome);
                contextQuery.Parameters.Add("@BAIRRO", TechneDbType.T_CODIGO, bairro);
                contextQuery.Parameters.Add("@CEP", TechneDbType.T_CODIGO, cep);

                ctx.GetDataTable(contextQuery);
            }
        }

        public void ExcluirCep(string idLogradouro)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"
                DELETE FROM HADES.DBO.TCE_LOGRADOURO
                    WHERE ID_LOGRADOURO = @ID_LOGRADOURO;
            "
                };

                contextQuery.Parameters.Add("@ID_LOGRADOURO", TechneDbType.T_CODIGO, idLogradouro);

                ctx.GetDataTable(contextQuery);
            }
        }

        public bool CepExiste(string cep, string municipio, string bairro)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"
                SELECT 1
                FROM HADES.DBO.TCE_LOGRADOURO (NOLOCK)
                WHERE CEP = @CEP AND ID_MUNICIPIO = @MUNICIPIO AND BAIRRO = @BAIRRO
            "
                };

                contextQuery.Parameters.Add("@CEP", TechneDbType.T_CODIGO, cep);
                contextQuery.Parameters.Add("@MUNICIPIO", TechneDbType.T_CODIGO, municipio);
                contextQuery.Parameters.Add("@BAIRRO", TechneDbType.T_CODIGO, bairro);

                DataTable dt = ctx.GetDataTable(contextQuery);

                return dt.Rows.Count > 0;
            }
        }
    }
}