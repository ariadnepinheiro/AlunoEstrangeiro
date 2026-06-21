using System;
using System.Data;
using System.Globalization;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN
{
    public class Instituicao : RNBase
    {
        //Excluir
        public static RetValue Excluir(string outra_faculdade)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            try
            {
                Ly_instituicao.Row.Delete(connection, outra_faculdade);

                retorno = VerificarErro(connection.GetErrors());

                if (retorno != null && !retorno.Ok)
                {
                    connection.Rollback();
                    return retorno;
                }

                return new RetValue(true, "Instituição excluída com sucesso.", null);
            }
            finally
            {
                connection.Close();
            }            
        }


        //Consultar
        public static Ly_instituicao.Row Consultar(string outra_faculdade)
        {
            Ly_instituicao.Row consulta = Ly_instituicao.QueryFirstRow(Config.CreateWritableConnection(), "outra_faculdade = ?", outra_faculdade);
            return consulta;
        }

        //Incluir
        public static RetValue Incluir(Ly_instituicao dt)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                if (dt != null)
                {
                    if (dt.Rows != null)
                    {
                        Ly_instituicao.Row.Insert(connection, dt.Rows[0].Outra_faculdade, dt.Rows[0].Nome_comp, dt.Rows[0].Local_vest, dt.Rows[0].Tipo_inst, "Endereco, End_num, Municipio, Cep, End_compl, Bairro, Tipo_Origem", dt.Rows[0].Endereco, dt.Rows[0].End_num, dt.Rows[0].Municipio, dt.Rows[0].Cep, dt.Rows[0].End_compl, dt.Rows[0].Bairro,dt.Rows[0].Tipo_origem);

                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }

                        retorno = new RetValue(true, "Instituição incluída com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        //Alterar
        public static RetValue Alterar(Ly_instituicao dt)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                if (dt != null)
                {
                    if (dt.Rows != null)
                    {
                        ColunasTable colunas = MontarParametros(dt);

                        Ly_instituicao.Row.Update(connection, dt.Rows[0].Outra_faculdade, colunas.Colunas, colunas.ValorColuna);

                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }

                        retorno = new RetValue(true, "Instituição alterada com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        public static string GeraOutraFaculdade()
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            decimal ordem;
            string ordem_s;

            try
            {
                ordem_s = Convert.ToString(TCommand.ExecuteScalar(connection, "Select max(CONVERT(NUMERIC,outra_faculdade)) From LY_INSTITUICAO"));
                if (!string.IsNullOrEmpty(ordem_s))
                    ordem = Convert.ToDecimal(ordem_s);
                else
                    ordem = 0;
            }
            finally
            {
                connection.Close();
            }

            return Convert.ToString(ordem + 1);
        }

        public static DataTable ConsultarInstituicao()
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT *
                             FROM LY_INSTITUICAO 
                            ORDER BY NOME_COMP"
                };

                return ctx.GetDataTable(contextQuery);
            }
        }

        public bool ExisteInstituicao(string outraFaculdade)
        {
            var sql = @"select top 1 1 from LY_INSTITUICAO WHERE OUTRA_FACULDADE = ?";

            var retorno = ExecutarFuncao(sql, outraFaculdade);

            return retorno == 1;
        }
    }
}
