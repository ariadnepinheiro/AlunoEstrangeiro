using System;
using System.Collections.Generic;
using System.Text;
using Techne.HadesLyc.CR;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;


namespace Techne.HadesLyc.RN.Custom
{
    public class Hd_agenciaCustom : Techne.HadesLyc.CR.Hd_agencia.CustomBase
    {
        public override string PreDelete(Hd_agencia.Row row, TConnectionWritable cn)
        {
            string erro = PodeRemover(row.Banco, row.Agencia, false);
            if (!string.IsNullOrEmpty(erro))
                return erro;

            return base.PreDelete(row, cn);
        }

        public override string PreUpdate(Hd_agencia.Row row, TConnectionWritable cn)
        {
            if (row.Original.Banco != row.Banco || row.Original.Agencia != row.Agencia)
            {
                string erro = PodeRemover(row.Original.Banco, row.Original.Agencia,true);
                if (!string.IsNullOrEmpty(erro))
                    return erro;
            }
            return base.PreUpdate(row, cn);
        }

        private string PodeRemover(decimal? banco, string agencia, bool alterando)
        {
            //abre uma conexão com o Lyceum e verifica se alguma tabela
            // aponta para esta agência
            //tabelas checadas:
            //LY_DOCENTE
            TConnection connection = Techne.Lyceum.Config.CreateConnection();
            connection.Open();
            QueryTable qt = null;

            try
            {

                qt = new QueryTable("select count(*) from LY_DOCENTE where banco=? and agencia=?");
                qt.Query(connection, banco, agencia);
                if (qt == null || qt.Rows.Count == 0 || Convert.ToDecimal(qt.Rows[0][0]) > 0)
                {
                    if (alterando)
                        return "Não é permitido alterar o número desta agência. Existem docentes com contas bancárias cadastradas nela.";
                    else
                        return "Não é permitido remover esta agência do cadastro. Existem docentes com contas bancárias cadastradas nela.";
                }

            }
            finally
            {
                connection.Close();
            }
            return "";
        }
    }
}
