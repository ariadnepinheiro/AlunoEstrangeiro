// HadesLyc vProducao
// Gerado por:
//   victor.quintana em 20/04/2009 às 13:32h
//   CronosRN, Version=1.1.3385.29867, Culture=neutral, PublicKeyToken=4a25cea0e390af6e
// Último load por leo em 01/12/2008 às 17:29:58 do banco SQLServer (Cronos@Techne17\Usuarios_SQL:HadCronosProducao)

using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;

using Techne.Data;
using Techne.Library;
using Techne.Web;

namespace Techne.HadesLyc.RN {
  public sealed class Acadêmico {
    public static ErrorList GET_NUMERO(
      string p_tipo, decimal? p_base, ref decimal? p_lastnum
    ) {
      TConnectionWritable cn = Config.CreateWritableConnection();
      cn.Transacao = "SP: GET_NUMERO";
      cn.Open();
      try {
        GET_NUMERO(cn, p_tipo, p_base, ref p_lastnum);
        return cn.GetErrors();
      }
      finally {
        if(cn.State == ConnectionState.Open) cn.Close();
      }
    }

    public static bool GET_NUMERO(
      TConnectionWritable connection,
      string p_tipo, decimal? p_base, ref decimal? p_lastnum
    ) {
      if(connection == null) throw new ArgumentNullException();

      TCommand cm = connection.CreateCommand();
      cm.CommandText = "GET_NUMERO";
      cm.CommandType = CommandType.StoredProcedure;

      cm.Parameters.Add(new TParameter("p_tipo", OleDbType.VarChar, 10, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Current, DbObject.ToDbObject(p_tipo)));
      cm.Parameters.Add(new TParameter("p_base", OleDbType.Numeric, 0, ParameterDirection.Input, true, 28, 0, string.Empty, DataRowVersion.Current, DbObject.ToDbObject(p_base)));
      cm.Parameters.Add(new TParameter("p_lastnum", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 12, 0, string.Empty, DataRowVersion.Current, DbObject.ToDbObject(p_lastnum)));

      bool success;
      connection.Open(true);
      try {
        int errorCount = connection.ErrorCount;
        cm.ExecuteNonQuery();
        success = connection.ErrorCount == errorCount;
        if(!success) connection.Rollback();
      }
      catch {
        if(connection.State == ConnectionState.Open) connection.Rollback();
        throw;
      }
      finally {
        if(connection.State == ConnectionState.Open) connection.Close();
      }

      p_lastnum = (decimal?)cm.Parameters["p_lastnum"].Value;

      return success;
    }

  }

}

