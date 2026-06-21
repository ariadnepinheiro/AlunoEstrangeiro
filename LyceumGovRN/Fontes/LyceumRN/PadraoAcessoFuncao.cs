using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Data;
using Techne.Lyceum.CR;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN
{
    public class PadraoAcessoFuncao : RNBase
    {
        public static QueryTable ListarPadacesFuncao(DbObject padaces)
        {
            string sql = "Select f.funcao, f.descricao, pf.padaces from LY_PADACES_FUNCAO pf inner join ly_funcao f on f.funcao = pf.funcao where pf.padaces = ?";
            return Consultar(sql, padaces);
        }

        public static RetValue Inserir(Ly_padaces_funcao.Row rowPadacesFuncao)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();

            try
            {
                connection.Open(true);

                Ly_padaces_funcao.Row.Insert(connection, rowPadacesFuncao.Padaces, rowPadacesFuncao.Funcao);

                retorno = VerificarErro(connection.GetErrors());

                return retorno;
            }
            catch (Exception ex)
            {
                connection.Rollback();
                return new RetValue(false, "", new Techne.Library.ErrorList(ex.Message));
            }
            finally
            {
                connection.Close();
            }
        }

        public static RetValue Alterar(Ly_padaces_funcao.Row rowPadacesFuncao)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();

            try
            {
                connection.Open(true);

                Ly_padaces_funcao.Row.Update(connection, rowPadacesFuncao.Padaces, rowPadacesFuncao.Funcao, "");

                retorno = VerificarErro(connection.GetErrors());

                return retorno;
            }
            catch (Exception ex)
            {
                connection.Rollback();
                return new RetValue(false, "", new Techne.Library.ErrorList(ex.Message));
            }
            finally
            {
                connection.Close();
            }
        }

        public static RetValue Excluir(Ly_padaces_funcao.Row rowPadacesFuncao)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();

            try
            {
                connection.Open(true);

                Ly_padaces_funcao.Row.Delete(connection, rowPadacesFuncao.Padaces, rowPadacesFuncao.Funcao);

                retorno = VerificarErro(connection.GetErrors());

                return retorno;
            }
            catch (Exception ex)
            {
                connection.Rollback();
                return new RetValue(false, "", new Techne.Library.ErrorList(ex.Message));
            }
            finally
            {
                connection.Close();
            }
        }

        public static void Insert(string padaces, string funcao) { }
        public static void Delete(string padaces, string funcao) { }
        public static void Update(string padaces, string funcao) { }


        public static bool VerificaPodeExcluirFuncao(string padaces, string funcao)
        {
            string sql = "select 1 from Ly_padaces_funcao where PADACES = ? and funcao = ? ";

            int retorno = ExecutarFuncao(sql, padaces, funcao);

            if (retorno == 1)
                return true;//tem acesso a transação
            else
                return false;
        }


        public static string ConsultaFuncao(string matricula)
        {
            string sql = @"SELECT top 1 funcao from ly_lotacao WHERE matricula = ? 
            and (data_desativacao is null
            OR convert(date,data_desativacao) > convert(date,GetDate()))";
            return ConsultarCampo(sql, matricula);
        }

        public bool PossuiAcessoExcluirFuncaoPor(DataContext ctx, string usuario, string funcao)
        {
            bool possui = false;

            ContextQuery contextQuery = new ContextQuery
            {
                Command = @" SELECT Count(*) 
                                FROM   PADUSUARIO PU 
                                       INNER JOIN LY_PADACES_FUNCAO PF 
                                               ON PF.PADACES = PU.PADACES 
                                WHERE  USUARIO = @USUARIO 
                                       AND FUNCAO = @FUNCAO "
            };

            contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuario);
            contextQuery.Parameters.Add("@FUNCAO", SqlDbType.VarChar, funcao);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }
    }
}
