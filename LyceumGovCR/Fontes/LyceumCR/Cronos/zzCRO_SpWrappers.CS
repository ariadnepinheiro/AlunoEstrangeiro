// Lyceum vDesenvSP
// Gerado por:
//   ercastro em 15/12/2008 às 16:39h
//   CronosRN, Version=1.1.3271.29361, Culture=neutral, PublicKeyToken=4a25cea0e390af6e
// Último load por ercastro em 10/12/2008 às 14:28:46 do banco SQLServer (sa@techne30:LyceumDesenvSP)

using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;

using Techne.Data;
using Techne.Library;
using Techne.Web;

namespace Techne.Lyceum.RN {
  public sealed class SPCliente {
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

      cm.Parameters.Add(new TParameter("p_tipo", OleDbType.VarChar, 20, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Current, DbObject.ToDbObject(p_tipo)));
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

    public static ErrorList s_VALIDA_OBRIG_DADOSCAD(
      ref string p_VALIDA_DT_NASC, ref string p_VALIDA_SEXO, ref string p_VALIDA_EST_CIVIL, 
      ref string p_VALIDA_RG, ref string p_VALIDA_CERT
    ) {
      TConnectionWritable cn = Config.CreateWritableConnection();
      cn.Transacao = "SP: s_VALIDA_OBRIG_DADOSCAD";
      cn.Open();
      try {
        s_VALIDA_OBRIG_DADOSCAD(cn, ref p_VALIDA_DT_NASC, ref p_VALIDA_SEXO, ref p_VALIDA_EST_CIVIL, 
                                    ref p_VALIDA_RG, ref p_VALIDA_CERT);
        return cn.GetErrors();
      }
      finally {
        if(cn.State == ConnectionState.Open) cn.Close();
      }
    }

    public static bool s_VALIDA_OBRIG_DADOSCAD(
      TConnectionWritable connection,
      ref string p_VALIDA_DT_NASC, ref string p_VALIDA_SEXO, ref string p_VALIDA_EST_CIVIL, 
      ref string p_VALIDA_RG, ref string p_VALIDA_CERT
    ) {
      if(connection == null) throw new ArgumentNullException();

      TCommand cm = connection.CreateCommand();
      cm.CommandText = "s_VALIDA_OBRIG_DADOSCAD";
      cm.CommandType = CommandType.StoredProcedure;

      cm.Parameters.Add(new TParameter("p_VALIDA_DT_NASC", OleDbType.Char, 1, ParameterDirection.InputOutput, true, 0, 0, string.Empty, DataRowVersion.Current, DbObject.ToDbObject(p_VALIDA_DT_NASC)));
      cm.Parameters.Add(new TParameter("p_VALIDA_SEXO", OleDbType.Char, 1, ParameterDirection.InputOutput, true, 0, 0, string.Empty, DataRowVersion.Current, DbObject.ToDbObject(p_VALIDA_SEXO)));
      cm.Parameters.Add(new TParameter("p_VALIDA_EST_CIVIL", OleDbType.Char, 1, ParameterDirection.InputOutput, true, 0, 0, string.Empty, DataRowVersion.Current, DbObject.ToDbObject(p_VALIDA_EST_CIVIL)));
      cm.Parameters.Add(new TParameter("p_VALIDA_RG", OleDbType.Char, 1, ParameterDirection.InputOutput, true, 0, 0, string.Empty, DataRowVersion.Current, DbObject.ToDbObject(p_VALIDA_RG)));
      cm.Parameters.Add(new TParameter("p_VALIDA_CERT", OleDbType.Char, 1, ParameterDirection.InputOutput, true, 0, 0, string.Empty, DataRowVersion.Current, DbObject.ToDbObject(p_VALIDA_CERT)));

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

      p_VALIDA_DT_NASC = (string)cm.Parameters["p_VALIDA_DT_NASC"].Value;
      p_VALIDA_SEXO = (string)cm.Parameters["p_VALIDA_SEXO"].Value;
      p_VALIDA_EST_CIVIL = (string)cm.Parameters["p_VALIDA_EST_CIVIL"].Value;
      p_VALIDA_RG = (string)cm.Parameters["p_VALIDA_RG"].Value;
      p_VALIDA_CERT = (string)cm.Parameters["p_VALIDA_CERT"].Value;

      return success;
    }

  }

}

