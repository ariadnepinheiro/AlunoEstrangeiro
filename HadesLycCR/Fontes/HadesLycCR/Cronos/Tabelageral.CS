// HadesLyc vHades1.7
// Gerado por:
//   cesar em 25/10/2010 às 15:06h
//   CronosRN, Version=1.1.3950.27138, Culture=neutral, PublicKeyToken=4a25cea0e390af6e
// Último load por cesar em 18/10/2010 às 14:51:54 do banco SQLServer (Cronos@lyceumgov1:HadCronosAuditoria)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Reflection;

using Techne.Data;
using Techne.Library;
using Techne.Library.MultiLang;

namespace Techne.HadesLyc.CR {
  public class TabelaGeral : TDataSet {
    private const string DataSetName_Def = "TabelaGeral";
    private static ConstructorInfo ctorTCustom = null;
    private CustomBase tcustom = null;
    #region Variáveis private para as tabelas deste dataset
    private Hd_numeracao pvTabHd_numeracao;
    private Hd_sistema pvTabHd_sistema;
    private Hd_tabela pvTabHd_tabela;
    private Hd_tabelaitem pvTabHd_tabelaitem;
    #endregion

    static TabelaGeral() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("TabelaGeral");
      }
      catch{}
    }

    public TabelaGeral() : base() {
      DataSetName = DataSetName_Def;

      pvTabHd_numeracao = new Hd_numeracao();
      pvTabHd_sistema = new Hd_sistema();
      pvTabHd_tabela = new Hd_tabela();
      pvTabHd_tabelaitem = new Hd_tabelaitem();

      Tables.Add(pvTabHd_numeracao);
      Tables.Add(pvTabHd_sistema);
      Tables.Add(pvTabHd_tabela);
      Tables.Add(pvTabHd_tabelaitem);

      CreateRelations();

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
    }


    [DefaultValue(DataSetName_Def)]
    public new string DataSetName {
      get { return base.DataSetName; }
      set { base.DataSetName = value == null ? string.Empty : value; }
    }

    private void CreateRelations() {
      Relations.Add("fk_tabela_sistema",
                    new DataColumn[] { pvTabHd_sistema.Columns["sis"] },
                    new DataColumn[] { pvTabHd_tabela.Columns["sis"] }, false);
      Relations.Add("fk_tabelaitem_tabela",
                    new DataColumn[] { pvTabHd_tabela.Columns["tabela"] },
                    new DataColumn[] { pvTabHd_tabelaitem.Columns["tabela"] }, false);
    }

    public override TConnection CreateConnection() { return Config.CreateConnection(); }

    public override TConnectionWritable CreateWritableConnection(TPermission permission) { return Config.CreateWritableConnection(permission); }

    public virtual string Put(TConnectionWritable connection) {
      connection.Open(true);
      try {
        if(tcustom != null)
          return tcustom.Put(this, connection);

        else {
          DataRow row;
          pvTabHd_numeracao.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_numeracao); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabHd_sistema.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_sistema); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabHd_tabela.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_tabela); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabHd_tabelaitem.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_tabelaitem); if(row != null) return DataLib.RowErrorsToString(row);
          return string.Empty;
        }
      }
      catch {
        connection.Rollback();
        throw;
      }
      finally {
        if(connection.State == ConnectionState.Open) connection.Close();
      }
    }


    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new DataRelationCollection Relations { get { return base.Relations; } }


    #region Propriedades para acesso às tabelas
    [
      Browsable(false),
    ]
    public Hd_numeracao Hd_numeracao { get { return pvTabHd_numeracao; } }

    [
      Browsable(false),
    ]
    public Hd_sistema Hd_sistema { get { return pvTabHd_sistema; } }

    [
      Browsable(false),
    ]
    public Hd_tabela Hd_tabela { get { return pvTabHd_tabela; } }

    [
      Browsable(false),
    ]
    public Hd_tabelaitem Hd_tabelaitem { get { return pvTabHd_tabelaitem; } }

    #endregion

    public abstract class CustomBase {
      public abstract string Put(Techne.HadesLyc.CR.TabelaGeral ds, TConnectionWritable connection);
    }

  }


  public partial class Hd_tabela : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_tabela() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_tabela");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_tabela");
      }
      catch{}
    }

    public Hd_tabela() {
      pvColTabela = new ColumnTabela(); Columns.Add(pvColTabela);
      pvColSis = new ColumnSis(); Columns.Add(pvColSis);
      pvColDescr = new ColumnDescr(); Columns.Add(pvColDescr);
      pvColTaminfo = new ColumnTaminfo(); Columns.Add(pvColTaminfo);
      pvColTipo = new ColumnTipo(); Columns.Add(pvColTipo);
      pvColNome_tabela = new ColumnNome_tabela(); Columns.Add(pvColNome_tabela);
      pvColColuna_tabela = new ColumnColuna_tabela(); Columns.Add(pvColColuna_tabela);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_tabela");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @tabela) {
      Row row = NewRow();

      row.Tabela = @tabela;

      return row;
    }

    public new Row[] Select() { return Select(string.Empty); }

    public new Row[] Select(string filterExpression) { return Select(filterExpression, string.Empty); }

    public new Row[] Select(string filterExpression, string sort) { return Select(filterExpression, sort, DataViewRowState.CurrentRows); }

    public new Row[] Select(string filterExpression, string sort, DataViewRowState recordStates) {
      DataRow[] rows = base.Select(filterExpression, sort, recordStates);
      Row[] result = new Row[rows.Length];
      rows.CopyTo(result, 0);
      return result;
    }


    [Browsable(false)]
    public override sealed string ConnectionString { get { return Config.ConnectionString; } }

    [Browsable(false)]
    public override sealed string MainTableName {
      get { return "HD_TABELA"; }
    }

    protected override sealed string Name { get { return "Tabela Geral"; } }

    [Browsable(false)]
    public override sealed bool ReadOnly {
      get { return false; }
    }

    [Browsable(false)]
    public new RowCollection Rows { get { return (RowCollection)base.Rows; } }


    protected override sealed DataRow NewRowFromBuilder(DataRowBuilder builder) {
      return new Row(builder);
    }

    protected override sealed void SetPrimaryKey() {
      SetPrimaryKey(new DataColumn[] { pvColTabela });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_tabela_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_tabela_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_tabela_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_tabela_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkTabela", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "TABELA", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("tabela", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TABELA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("sis", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SIS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("descr", OleDbType.VarChar, 260, ParameterDirection.InputOutput, true, 0, 0, "DESCR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("taminfo", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "TAMINFO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("tipo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TIPO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("nome_tabela", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NOME_TABELA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("coluna_tabela", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "COLUNA_TABELA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

      switch(operation) {
      case CommandOperation.Insert: cm.Parameters.Add(new TParameter("tipoComando", OleDbType.VarChar, 10, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Current, "Insert")); break;
      case CommandOperation.Update: cm.Parameters.Add(new TParameter("tipoComando", OleDbType.VarChar, 10, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Current, "Update")); break;
      case CommandOperation.Delete: cm.Parameters.Add(new TParameter("tipoComando", OleDbType.VarChar, 10, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Current, "Delete")); break;
      }
      cm.Parameters.Add(new TParameter("erro", OleDbType.VarChar, 1024, ParameterDirection.Output, true, 0, 0, string.Empty, DataRowVersion.Default, DBNull.Value));

      return cm;
    }

    /// <summary>
    /// Filtra o array de colunas retornando somente as colunas pertencentes ao DataTable.
    /// Devolve StringCollection para que seja possível utilizar o método Contains().
    /// </summary>
    /// <param name="toLower">As colunas serão devolvidas em minúsculas</param>
    /// <param name="tabPrincipal">Somente colunas da tabela principal</param>
    private static StringCollection FilterColumns(string[] columns, bool toLower, bool tabPrincipal) {
      StringCollection result = new StringCollection();

      if(columns != null)
        foreach(string column in columns) {
          string lower = column.ToLower();
          if(lower == "tabela") { result.Add(toLower ? "tabela" : column); continue; }
          if(lower == "sis") { result.Add(toLower ? "sis" : column); continue; }
          if(lower == "descr") { result.Add(toLower ? "descr" : column); continue; }
          if(lower == "taminfo") { result.Add(toLower ? "taminfo" : column); continue; }
          if(lower == "tipo") { result.Add(toLower ? "tipo" : column); continue; }
          if(lower == "nome_tabela") { result.Add(toLower ? "nome_tabela" : column); continue; }
          if(lower == "coluna_tabela") { result.Add(toLower ? "coluna_tabela" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "tabela", "sis", "descr", "taminfo", "tipo", "nome_tabela", "coluna_tabela" }
                          : new string[] { "TABELA", "SIS", "DESCR", "TAMINFO", "TIPO", "NOME_TABELA", "COLUNA_TABELA" });
        else
          result.AddRange(toLower
                          ? new string[] { "tabela", "sis", "descr", "taminfo", "tipo", "nome_tabela", "coluna_tabela" }
                          : new string[] { "TABELA", "SIS", "DESCR", "TAMINFO", "TIPO", "NOME_TABELA", "COLUNA_TABELA" });

      return result;
    }


    protected override sealed string PreOperationInternal(TDataRowUpdateEventArgs args) {
      string error = string.Empty;

      if(custom != null)
        switch(args.CommandOperation) {
          case CommandOperation.Insert: error = custom.PreInsert((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          case CommandOperation.Update: error = custom.PreUpdate((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          case CommandOperation.Delete: error = custom.PreDelete((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          case CommandOperation.Undelete: error = custom.PreUndelete((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          default: throw new NotImplementedException();
        }

      if(error != null && error.Length == 0 && tcustom != null)
        switch(args.CommandOperation) {
          case CommandOperation.Insert: error = tcustom.PreInsert((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          case CommandOperation.Update: error = tcustom.PreUpdate((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          case CommandOperation.Delete: error = tcustom.PreDelete((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          case CommandOperation.Undelete: error = tcustom.PreUndelete((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          default: throw new NotImplementedException();
        }

      if(error == null) throw new InvalidOperationException("Entry-points não podem devolver null. Devolva string.Empty para indicar inexistência de erros.");
      return error;
    }

    protected override sealed string PostOperationInternal(TDataRowUpdateEventArgs args) {
      string error = string.Empty;

      if(custom != null)
        switch(args.CommandOperation) {
          case CommandOperation.Insert: error = custom.PostInsert((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          case CommandOperation.Update: error = custom.PostUpdate((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          case CommandOperation.Delete: error = custom.PostDelete((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          case CommandOperation.Undelete: error = custom.PostUndelete((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          default: throw new NotImplementedException();
        }

      if(error != null && error.Length == 0 && tcustom != null)
        switch(args.CommandOperation) {
          case CommandOperation.Insert: error = tcustom.PostInsert((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          case CommandOperation.Update: error = tcustom.PostUpdate((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          case CommandOperation.Delete: error = tcustom.PostDelete((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          case CommandOperation.Undelete: error = tcustom.PostUndelete((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          default: throw new NotImplementedException();
        }

      if(error == null) throw new InvalidOperationException("Entry-points não podem devolver null. Devolva string.Empty para indicar inexistência de erros.");
      return error;
    }


    public override sealed int Get(TConnection connection,
                                   string[] columns,
                                   string where, DbObject[] whereValues,
                                   string order,
                                   int startRecord,
                                   int maxRecords,
                                   bool createPrimaryKey,
                                   bool getDeletedRecords,
                                   bool rowCountOnly) {
      if(where == null) throw new ArgumentNullException("where");
      if(order == null) throw new ArgumentNullException("order");
      TConnection cn = connection != null ? connection : Config.CreateConnection();
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      where = where.Trim();

      int count;

      StringCollection scColumns = FilterColumns(columns, true, false);

      #region Build Select
      ArrayList list = new ArrayList();
      list.AddRange(new string[] {
        "hd_tabela.tabela"
      });
      if(scColumns.Contains("sis")) list.Add("hd_tabela.sis");
      if(scColumns.Contains("descr")) list.Add("hd_tabela.descr");
      if(scColumns.Contains("taminfo")) list.Add("hd_tabela.taminfo");
      if(scColumns.Contains("tipo")) list.Add("hd_tabela.tipo");
      if(scColumns.Contains("nome_tabela")) list.Add("hd_tabela.nome_tabela");
      if(scColumns.Contains("coluna_tabela")) list.Add("hd_tabela.coluna_tabela");
      string select,columnList,orderby;
      if (rowCountOnly)
      {
        columnList = "count(*)";
        orderby = "";
      }
      else
      {
        columnList = StrLib.EnumerableToStr(list, ", ");
        orderby = (order.Trim().Length == 0 ? string.Empty : " ORDER BY " + order);
      }
      select =
        "SELECT " + columnList + " " +
          "FROM hd_tabela " +
          (where.Length == 0 ? string.Empty : " WHERE " + where) +
          orderby;

      #endregion
      if (rowCountOnly)
      {
        #region RowCountOnly
        using(TCommand cm = cn.CreateCommand(select, whereValues)) 
        {
          try 
          {
            count = (int)(decimal)cm.ExecuteScalar();
          }
          catch(Exception exc)
          {
            throw new Exception(exc.Message + " " +
              "Select statement: [" + select + "], " +
              "Parameters: [" + StrLib.EnumerableToStr(whereValues) + "]");
          }
        }
        #endregion
      }
      else
      {
        #region Fill Datatable
        using(TCommand cm = cn.CreateCommand(select, whereValues)) {
          TDataAdapter da = new TDataAdapter(cm);
          try {
            DataSet ds = BaseDataSet;
            bool fakeds = false;
            if(ds == null) {
              fakeds = true;
              ds = new DataSet();
              ds.Tables.Add(this);
            }
            count = da.Fill(ds, startRecord, maxRecords, "hd_tabela");
            if(fakeds) ds.Tables.Remove(this);
          }
          catch(Exception exc) {
            throw new Exception(exc.Message + " " +
                                "Select statement: [" + select + "], " +
                                "Parameters: [" + StrLib.EnumerableToStr(whereValues) + "]");
          }
        }

        if(createPrimaryKey) SetPrimaryKey();

        // Cria os nomes (caption) nas colunas da tabela do DataSet
        // de acordo com o idioma informado no construtor da classe.
        foreach(DataColumn dataColumn in Columns) {
          TDataColumn c = dataColumn as TDataColumn;
          if(c != null && scColumns.Contains(c.ColumnName.ToLower()))
            c.ReadOnly = c.IsAux;
        }
        #endregion
      }

      return count;
    }

    public override sealed void Put(TConnectionWritable connection, DataRow[] dataRows) {
      if(connection == null) throw new ArgumentNullException();

      TDataAdapter da; {
        da = new TDataAdapter();
        da.TableMappings.Add("Table", "Hd_tabela");
        da.ContinueUpdateOnError = true;
        da.RowUpdating += new TDataRowUpdateEventHandler(da_RowUpdating);
        da.RowUpdated += new TDataRowUpdateEventHandler(da_RowUpdated);

        da.InsertCommand = CreateCommand(connection, CommandOperation.Insert);
        da.UpdateCommand = CreateCommand(connection, CommandOperation.Update);
        da.DeleteCommand = CreateCommand(connection, CommandOperation.Delete);
      }



      Put(da, connection, dataRows);
    }

    public void ValidaConvEmptyStrToNull(ref DataRow dr)
    {
      if (dr != null)
      {
            if (dr["TABELA"] != null && string.IsNullOrEmpty(dr["TABELA"].ToString()))
              dr["TABELA"] = DBNull.Value;
            if (dr["SIS"] != null && string.IsNullOrEmpty(dr["SIS"].ToString()))
              dr["SIS"] = DBNull.Value;
            if (dr["DESCR"] != null && string.IsNullOrEmpty(dr["DESCR"].ToString()))
              dr["DESCR"] = DBNull.Value;
            if (dr["TAMINFO"] != null && string.IsNullOrEmpty(dr["TAMINFO"].ToString()))
              dr["TAMINFO"] = DBNull.Value;
            if (dr["TIPO"] != null && string.IsNullOrEmpty(dr["TIPO"].ToString()))
              dr["TIPO"] = DBNull.Value;
            if (dr["NOME_TABELA"] != null && string.IsNullOrEmpty(dr["NOME_TABELA"].ToString()))
              dr["NOME_TABELA"] = DBNull.Value;
            if (dr["COLUNA_TABELA"] != null && string.IsNullOrEmpty(dr["COLUNA_TABELA"].ToString()))
              dr["COLUNA_TABELA"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["TABELA"] == null || string.IsNullOrEmpty(dr["TABELA"].ToString()))
            {
              return "Favor informar a tabela.";
            }

            if (dr["TABELA"] != null && !string.IsNullOrEmpty(dr["TABELA"].ToString()))
            {
              if (dr["TABELA"].ToString().Length > 20)
              {
                return "A tabela deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["SIS"] != null && !string.IsNullOrEmpty(dr["SIS"].ToString()))
            {
              if (dr["SIS"].ToString().Length > 12)
              {
                return "O sistema deve possuir no máximo 12 caracteres.";
              }
            }

            if (dr["DESCR"] != null && !string.IsNullOrEmpty(dr["DESCR"].ToString()))
            {
              if (dr["DESCR"].ToString().Length > 160)
              {
                return "A descrição deve possuir no máximo 160 caracteres.";
              }
            }

            if (dr["TAMINFO"] != null && !string.IsNullOrEmpty(dr["TAMINFO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["TAMINFO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["TAMINFO"]).ToString("0.############").Split(',')[0].Length > 2) || (Convert.ToDecimal(dr["TAMINFO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["TAMINFO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O tamanho máximo da informação deve ser um valor inteiro com 2 dígitos no máximo.";
              }

              if (dr["TAMINFO"] != null && !string.IsNullOrEmpty(dr["TAMINFO"].ToString()))
              {
                if (Convert.ToDecimal(dr["TAMINFO"]) < (1))
                {
                  return "O tamanho máximo da informação deve ser maior ou igual a (1).";
                }
              }
            }

            if (dr["TIPO"] != null && !string.IsNullOrEmpty(dr["TIPO"].ToString()))
            {
              if (dr["TIPO"].ToString().Length > 1)
              {
                return "O tipo deve possuir no máximo 1 caractere.";
              }

              if (dr["TIPO"] != null && !string.IsNullOrEmpty(dr["TIPO"].ToString()))
              {
                List<object> lstTIPO = new List<object>(new object[]{"N", "A"});
                if(!lstTIPO.Contains(dr["TIPO"].ToString()))
                {
                  return "Os valores permitidos para o tipo são ''N'' ou ''A''.";
                }
              }
            }

            if (dr["NOME_TABELA"] != null && !string.IsNullOrEmpty(dr["NOME_TABELA"].ToString()))
            {
              if (dr["NOME_TABELA"].ToString().Length > 50)
              {
                return "O nome da tabela deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["COLUNA_TABELA"] != null && !string.IsNullOrEmpty(dr["COLUNA_TABELA"].ToString()))
            {
              if (dr["COLUNA_TABELA"].ToString().Length > 50)
              {
                return "A coluna deve possuir no máximo 50 caracteres.";
              }
            }
      }
      return "";
    }

    protected override string PreInsert(DataRow row, TConnectionWritable connection)
    {
      ValidaConvEmptyStrToNull(ref row);
      return ValidaCampos(row);
    }

    protected override string PreUpdate(DataRow row, TConnectionWritable connection)
    {
      ValidaConvEmptyStrToNull(ref row);
      return ValidaCampos(row);
    }

    public static Hd_tabela Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_tabela Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_tabela tab = new Hd_tabela();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_tabela tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @tabela, string @sis, string @descr, decimal? @taminfo, string @tipo, string @nome_tabela, 
        string @coluna_tabela
      ) {
        return (Row)Add(new object[] {
          @tabela, @sis, @descr, @taminfo, @tipo, @nome_tabela, @coluna_tabela
        });
      }

      public Row Find(
        string @tabela
      ) {
        return (Row)Find(new object[] {
          @tabela
        });
      }


      public new Row this[int index] { get { return (Row)base[index]; } }

      public void Add(Row row) { base.Add(row); }

      public void InsertAt(Row row, int pos) { base.InsertAt(row, pos); }

      public void Remove(Row row) { base.Remove(row); }

    }

    public sealed class Row : TDataRow {
      private OriginalValues original;

      internal Row(DataRowBuilder rb) : base(rb) {
        this.original = new OriginalValues(this);
      }


      /// <summary>
      /// TABELA: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Tabela { get { return (string)this["tabela"]; } set { this["tabela"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SIS: VARCHAR(12)
      /// </summary>
      public string Sis { get { return (string)this["sis"]; } set { this["sis"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCR: VARCHAR(160)
      /// </summary>
      public string Descr { get { return (string)this["descr"]; } set { this["descr"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TAMINFO: NUMERIC(2)
      /// </summary>
      public decimal? Taminfo { get { return (decimal?)this["taminfo"]; } set { this["taminfo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TIPO: CHAR(1)
      /// </summary>
      public string Tipo { get { return (string)this["tipo"]; } set { this["tipo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME_TABELA: VARCHAR(50)
      /// </summary>
      public string Nome_tabela { get { return (string)this["nome_tabela"]; } set { this["nome_tabela"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// COLUNA_TABELA: VARCHAR(50)
      /// </summary>
      public string Coluna_tabela { get { return (string)this["coluna_tabela"]; } set { this["coluna_tabela"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @tabela) {
        return Insert(connection,
                      @tabela,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @tabela,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_tabela(),
          new string[] { "tabela" },
          new object[] { @tabela },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkTabela,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_tabela(),
          new string[] { "tabela" },
          new object[] { pkTabela },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkTabela) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_tabela(),
          new string[] { "tabela" },
          new object[] { pkTabela }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkTabela) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_tabela(),
          new string[] { "tabela" },
          new object[] { pkTabela }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Tabela { get { return (string)row["tabela", DataRowVersion.Original]; } }
        public string Sis { get { return (string)row["sis", DataRowVersion.Original]; } }
        public string Descr { get { return (string)row["descr", DataRowVersion.Original]; } }
        public decimal? Taminfo { get { return (decimal?)row["taminfo", DataRowVersion.Original]; } }
        public string Tipo { get { return (string)row["tipo", DataRowVersion.Original]; } }
        public string Nome_tabela { get { return (string)row["nome_tabela", DataRowVersion.Original]; } }
        public string Coluna_tabela { get { return (string)row["coluna_tabela", DataRowVersion.Original]; } }

      }

    }

    public abstract class CustomBase {
      public virtual string PreInsert(Row row, TConnectionWritable cn) { return string.Empty; }
      public virtual string PreUpdate(Row row, TConnectionWritable cn) { return string.Empty; }
      public virtual string PreDelete(Row row, TConnectionWritable cn) { return string.Empty; }
      public virtual string PreUndelete(Row row, TConnectionWritable cn) { return string.Empty; }
      public virtual string PostInsert(Row row, TConnectionWritable cn) { return string.Empty; }
      public virtual string PostUpdate(Row row, TConnectionWritable cn) { return string.Empty; }
      public virtual string PostDelete(Row row, TConnectionWritable cn) { return string.Empty; }
      public virtual string PostUndelete(Row row, TConnectionWritable cn) { return string.Empty; }
    }

    #region Columns private fields and classes
    #region Coluna: TABELA
    private ColumnTabela pvColTabela;

    public sealed class ColumnTabela : TDataColumn {

      public ColumnTabela() {
        this.ColumnName = "tabela";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "tabela";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_tabela.tabela"; } }
      public override string OriginalCol { get { return "tabela"; } }
      public override string RefCol { get { return "HD_TABELA.TABELA"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 20; } }
      public override string SqlServerType { get { return "VARCHAR(20)"; } }

    }
    #endregion

    #region Coluna: SIS
    private ColumnSis pvColSis;

    public sealed class ColumnSis : TDataColumn {

      public ColumnSis() {
        this.ColumnName = "sis";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "sistema";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_tabela.sis"; } }
      public override string OriginalCol { get { return "sis"; } }
      public override string RefCol { get { return "HD_TABELA.SIS"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 12; } }
      public override string SqlServerType { get { return "VARCHAR(12)"; } }

    }
    #endregion

    #region Coluna: DESCR
    private ColumnDescr pvColDescr;

    public sealed class ColumnDescr : TDataColumn {

      public ColumnDescr() {
        this.ColumnName = "descr";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "descrição";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_tabela.descr"; } }
      public override string OriginalCol { get { return "descr"; } }
      public override string RefCol { get { return "HD_TABELA.DESCR"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 160; } }
      public override string SqlServerType { get { return "VARCHAR(160)"; } }

    }
    #endregion

    #region Coluna: TAMINFO
    private ColumnTaminfo pvColTaminfo;

    public sealed class ColumnTaminfo : TDataColumn {

      public ColumnTaminfo() {
        this.ColumnName = "taminfo";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "tamanho máximo da informação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_tabela.taminfo"; } }
      public override string OriginalCol { get { return "taminfo"; } }
      public override string RefCol { get { return "HD_TABELA.TAMINFO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 3; } }
      public override int Precision { get { return 2; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(2)"; } }

      public override object MinValue { get { return 1; } }

    }
    #endregion

    #region Coluna: TIPO
    private ColumnTipo pvColTipo;

    public sealed class ColumnTipo : TDataColumn {

      public ColumnTipo() {
        this.ColumnName = "tipo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "tipo";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_tabela.tipo"; } }
      public override string OriginalCol { get { return "tipo"; } }
      public override string RefCol { get { return "HD_TABELA.TIPO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 1; } }
      public override string SqlServerType { get { return "CHAR(1)"; } }

    }
    #endregion

    #region Coluna: NOME_TABELA
    private ColumnNome_tabela pvColNome_tabela;

    public sealed class ColumnNome_tabela : TDataColumn {

      public ColumnNome_tabela() {
        this.ColumnName = "nome_tabela";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "nome da tabela";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_tabela.nome_tabela"; } }
      public override string OriginalCol { get { return "nome_tabela"; } }
      public override string RefCol { get { return "HD_TABELA.NOME_TABELA"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 50; } }
      public override string SqlServerType { get { return "VARCHAR(50)"; } }

    }
    #endregion

    #region Coluna: COLUNA_TABELA
    private ColumnColuna_tabela pvColColuna_tabela;

    public sealed class ColumnColuna_tabela : TDataColumn {

      public ColumnColuna_tabela() {
        this.ColumnName = "coluna_tabela";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "coluna";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_tabela.coluna_tabela"; } }
      public override string OriginalCol { get { return "coluna_tabela"; } }
      public override string RefCol { get { return "HD_TABELA.COLUNA_TABELA"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 50; } }
      public override string SqlServerType { get { return "VARCHAR(50)"; } }

    }
    #endregion
    #endregion
  }

  public partial class Hd_tabelaitem : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_tabelaitem() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_tabelaitem");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_tabelaitem");
      }
      catch{}
    }

    public Hd_tabelaitem() {
      pvColTabela = new ColumnTabela(); Columns.Add(pvColTabela);
      pvColItem = new ColumnItem(); Columns.Add(pvColItem);
      pvColDescr = new ColumnDescr(); Columns.Add(pvColDescr);
      pvColSis = new ColumnSis(); Columns.Add(pvColSis);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_tabelaitem");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @tabela, string @item) {
      Row row = NewRow();

      row.Tabela = @tabela;
      row.Item = @item;

      return row;
    }

    public new Row[] Select() { return Select(string.Empty); }

    public new Row[] Select(string filterExpression) { return Select(filterExpression, string.Empty); }

    public new Row[] Select(string filterExpression, string sort) { return Select(filterExpression, sort, DataViewRowState.CurrentRows); }

    public new Row[] Select(string filterExpression, string sort, DataViewRowState recordStates) {
      DataRow[] rows = base.Select(filterExpression, sort, recordStates);
      Row[] result = new Row[rows.Length];
      rows.CopyTo(result, 0);
      return result;
    }


    [Browsable(false)]
    public override sealed string ConnectionString { get { return Config.ConnectionString; } }

    [Browsable(false)]
    public override sealed string MainTableName {
      get { return "HD_TABELAITEM"; }
    }

    protected override sealed string Name { get { return "Itens da Tabela Geral"; } }

    [Browsable(false)]
    public override sealed bool ReadOnly {
      get { return false; }
    }

    [Browsable(false)]
    public new RowCollection Rows { get { return (RowCollection)base.Rows; } }


    protected override sealed DataRow NewRowFromBuilder(DataRowBuilder builder) {
      return new Row(builder);
    }

    protected override sealed void SetPrimaryKey() {
      SetPrimaryKey(new DataColumn[] { pvColTabela, pvColItem });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_tabelaitem_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_tabelaitem_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_tabelaitem_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_tabelaitem_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkTabela", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "TABELA", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkItem", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "ITEM", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("tabela", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TABELA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("item", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "ITEM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("descr", OleDbType.VarChar, 280, ParameterDirection.InputOutput, true, 0, 0, "DESCR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

      switch(operation) {
      case CommandOperation.Insert: cm.Parameters.Add(new TParameter("tipoComando", OleDbType.VarChar, 10, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Current, "Insert")); break;
      case CommandOperation.Update: cm.Parameters.Add(new TParameter("tipoComando", OleDbType.VarChar, 10, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Current, "Update")); break;
      case CommandOperation.Delete: cm.Parameters.Add(new TParameter("tipoComando", OleDbType.VarChar, 10, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Current, "Delete")); break;
      }
      cm.Parameters.Add(new TParameter("erro", OleDbType.VarChar, 1024, ParameterDirection.Output, true, 0, 0, string.Empty, DataRowVersion.Default, DBNull.Value));

      return cm;
    }

    /// <summary>
    /// Filtra o array de colunas retornando somente as colunas pertencentes ao DataTable.
    /// Devolve StringCollection para que seja possível utilizar o método Contains().
    /// </summary>
    /// <param name="toLower">As colunas serão devolvidas em minúsculas</param>
    /// <param name="tabPrincipal">Somente colunas da tabela principal</param>
    private static StringCollection FilterColumns(string[] columns, bool toLower, bool tabPrincipal) {
      StringCollection result = new StringCollection();

      if(columns != null)
        foreach(string column in columns) {
          string lower = column.ToLower();
          if(lower == "tabela") { result.Add(toLower ? "tabela" : column); continue; }
          if(lower == "item") { result.Add(toLower ? "item" : column); continue; }
          if(lower == "descr") { result.Add(toLower ? "descr" : column); continue; }
          if(!tabPrincipal) {
            if(lower == "sis") { result.Add(toLower ? "sis" : column); continue; }
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "tabela", "item", "descr" }
                          : new string[] { "TABELA", "ITEM", "DESCR" });
        else
          result.AddRange(toLower
                          ? new string[] { "tabela", "item", "descr", "sis" }
                          : new string[] { "TABELA", "ITEM", "DESCR", "SIS" });

      return result;
    }


    protected override sealed string PreOperationInternal(TDataRowUpdateEventArgs args) {
      string error = string.Empty;

      if(custom != null)
        switch(args.CommandOperation) {
          case CommandOperation.Insert: error = custom.PreInsert((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          case CommandOperation.Update: error = custom.PreUpdate((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          case CommandOperation.Delete: error = custom.PreDelete((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          case CommandOperation.Undelete: error = custom.PreUndelete((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          default: throw new NotImplementedException();
        }

      if(error != null && error.Length == 0 && tcustom != null)
        switch(args.CommandOperation) {
          case CommandOperation.Insert: error = tcustom.PreInsert((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          case CommandOperation.Update: error = tcustom.PreUpdate((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          case CommandOperation.Delete: error = tcustom.PreDelete((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          case CommandOperation.Undelete: error = tcustom.PreUndelete((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          default: throw new NotImplementedException();
        }

      if(error == null) throw new InvalidOperationException("Entry-points não podem devolver null. Devolva string.Empty para indicar inexistência de erros.");
      return error;
    }

    protected override sealed string PostOperationInternal(TDataRowUpdateEventArgs args) {
      string error = string.Empty;

      if(custom != null)
        switch(args.CommandOperation) {
          case CommandOperation.Insert: error = custom.PostInsert((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          case CommandOperation.Update: error = custom.PostUpdate((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          case CommandOperation.Delete: error = custom.PostDelete((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          case CommandOperation.Undelete: error = custom.PostUndelete((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          default: throw new NotImplementedException();
        }

      if(error != null && error.Length == 0 && tcustom != null)
        switch(args.CommandOperation) {
          case CommandOperation.Insert: error = tcustom.PostInsert((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          case CommandOperation.Update: error = tcustom.PostUpdate((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          case CommandOperation.Delete: error = tcustom.PostDelete((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          case CommandOperation.Undelete: error = tcustom.PostUndelete((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
          default: throw new NotImplementedException();
        }

      if(error == null) throw new InvalidOperationException("Entry-points não podem devolver null. Devolva string.Empty para indicar inexistência de erros.");
      return error;
    }


    public override sealed int Get(TConnection connection,
                                   string[] columns,
                                   string where, DbObject[] whereValues,
                                   string order,
                                   int startRecord,
                                   int maxRecords,
                                   bool createPrimaryKey,
                                   bool getDeletedRecords,
                                   bool rowCountOnly) {
      if(where == null) throw new ArgumentNullException("where");
      if(order == null) throw new ArgumentNullException("order");
      TConnection cn = connection != null ? connection : Config.CreateConnection();
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      where = where.Trim();

      int count;

      StringCollection scColumns = FilterColumns(columns, true, false);

      #region Build Select
      ArrayList list = new ArrayList();
      list.AddRange(new string[] {
        "hd_tabelaitem.tabela", "hd_tabelaitem.item"
      });
      if(scColumns.Contains("descr")) list.Add("hd_tabelaitem.descr");
      if(scColumns.Contains("sis")) list.Add("hd_tabela.sis");
      string select,columnList,orderby;
      if (rowCountOnly)
      {
        columnList = "count(*)";
        orderby = "";
      }
      else
      {
        columnList = StrLib.EnumerableToStr(list, ", ");
        orderby = (order.Trim().Length == 0 ? string.Empty : " ORDER BY " + order);
      }
      select =
        "SELECT " + columnList + " " +
          "FROM hd_tabelaitem " +
               "INNER JOIN hd_tabela " +
                 "ON hd_tabela.tabela = hd_tabelaitem.tabela " +
          (where.Length == 0 ? string.Empty : " WHERE " + where) +
          orderby;

      #endregion
      if (rowCountOnly)
      {
        #region RowCountOnly
        using(TCommand cm = cn.CreateCommand(select, whereValues)) 
        {
          try 
          {
            count = (int)(decimal)cm.ExecuteScalar();
          }
          catch(Exception exc)
          {
            throw new Exception(exc.Message + " " +
              "Select statement: [" + select + "], " +
              "Parameters: [" + StrLib.EnumerableToStr(whereValues) + "]");
          }
        }
        #endregion
      }
      else
      {
        #region Fill Datatable
        using(TCommand cm = cn.CreateCommand(select, whereValues)) {
          TDataAdapter da = new TDataAdapter(cm);
          try {
            DataSet ds = BaseDataSet;
            bool fakeds = false;
            if(ds == null) {
              fakeds = true;
              ds = new DataSet();
              ds.Tables.Add(this);
            }
            count = da.Fill(ds, startRecord, maxRecords, "hd_tabelaitem");
            if(fakeds) ds.Tables.Remove(this);
          }
          catch(Exception exc) {
            throw new Exception(exc.Message + " " +
                                "Select statement: [" + select + "], " +
                                "Parameters: [" + StrLib.EnumerableToStr(whereValues) + "]");
          }
        }

        if(createPrimaryKey) SetPrimaryKey();

        // Cria os nomes (caption) nas colunas da tabela do DataSet
        // de acordo com o idioma informado no construtor da classe.
        foreach(DataColumn dataColumn in Columns) {
          TDataColumn c = dataColumn as TDataColumn;
          if(c != null && scColumns.Contains(c.ColumnName.ToLower()))
            c.ReadOnly = c.IsAux;
        }
        #endregion
      }

      return count;
    }

    public override sealed void Put(TConnectionWritable connection, DataRow[] dataRows) {
      if(connection == null) throw new ArgumentNullException();

      TDataAdapter da; {
        da = new TDataAdapter();
        da.TableMappings.Add("Table", "Hd_tabelaitem");
        da.ContinueUpdateOnError = true;
        da.RowUpdating += new TDataRowUpdateEventHandler(da_RowUpdating);
        da.RowUpdated += new TDataRowUpdateEventHandler(da_RowUpdated);

        da.InsertCommand = CreateCommand(connection, CommandOperation.Insert);
        da.UpdateCommand = CreateCommand(connection, CommandOperation.Update);
        da.DeleteCommand = CreateCommand(connection, CommandOperation.Delete);
      }



      Put(da, connection, dataRows);
    }

    public void ValidaConvEmptyStrToNull(ref DataRow dr)
    {
      if (dr != null)
      {
            if (dr["TABELA"] != null && string.IsNullOrEmpty(dr["TABELA"].ToString()))
              dr["TABELA"] = DBNull.Value;
            if (dr["ITEM"] != null && string.IsNullOrEmpty(dr["ITEM"].ToString()))
              dr["ITEM"] = DBNull.Value;
            if (dr["DESCR"] != null && string.IsNullOrEmpty(dr["DESCR"].ToString()))
              dr["DESCR"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["TABELA"] == null || string.IsNullOrEmpty(dr["TABELA"].ToString()))
            {
              return "Favor informar a tabela.";
            }

            if (dr["TABELA"] != null && !string.IsNullOrEmpty(dr["TABELA"].ToString()))
            {
              if (dr["TABELA"].ToString().Length > 20)
              {
                return "A tabela deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["ITEM"] == null || string.IsNullOrEmpty(dr["ITEM"].ToString()))
            {
              return "Favor informar o item.";
            }

            if (dr["ITEM"] != null && !string.IsNullOrEmpty(dr["ITEM"].ToString()))
            {
              if (dr["ITEM"].ToString().Length > 40)
              {
                return "O item deve possuir no máximo 40 caracteres.";
              }
            }

            if (dr["DESCR"] != null && !string.IsNullOrEmpty(dr["DESCR"].ToString()))
            {
              if (dr["DESCR"].ToString().Length > 180)
              {
                return "A descrição deve possuir no máximo 180 caracteres.";
              }
            }
      }
      return "";
    }

    protected override string PreInsert(DataRow row, TConnectionWritable connection)
    {
      ValidaConvEmptyStrToNull(ref row);
      return ValidaCampos(row);
    }

    protected override string PreUpdate(DataRow row, TConnectionWritable connection)
    {
      ValidaConvEmptyStrToNull(ref row);
      return ValidaCampos(row);
    }

    public static Hd_tabelaitem Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_tabelaitem Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_tabelaitem tab = new Hd_tabelaitem();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_tabelaitem tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @tabela, string @item, string @descr
      ) {
        return (Row)Add(new object[] {
          @tabela, @item, @descr
        });
      }

      public Row Find(
        string @tabela, string @item
      ) {
        return (Row)Find(new object[] {
          @tabela, @item
        });
      }


      public new Row this[int index] { get { return (Row)base[index]; } }

      public void Add(Row row) { base.Add(row); }

      public void InsertAt(Row row, int pos) { base.InsertAt(row, pos); }

      public void Remove(Row row) { base.Remove(row); }

    }

    public sealed class Row : TDataRow {
      private OriginalValues original;

      internal Row(DataRowBuilder rb) : base(rb) {
        this.original = new OriginalValues(this);
      }


      /// <summary>
      /// TABELA: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Tabela { get { return (string)this["tabela"]; } set { this["tabela"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ITEM: VARCHAR(40), Primary Key, Not Null
      /// </summary>
      public string Item { get { return (string)this["item"]; } set { this["item"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCR: VARCHAR(180)
      /// </summary>
      public string Descr { get { return (string)this["descr"]; } set { this["descr"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SIS: VARCHAR(12)
      /// </summary>
      public string Sis { get { return (string)this["sis"]; } set { this["sis"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @tabela, string @item) {
        return Insert(connection,
                      @tabela, @item,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @tabela, string @item,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_tabelaitem(),
          new string[] { "tabela", "item" },
          new object[] { @tabela, @item },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkTabela, string pkItem,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_tabelaitem(),
          new string[] { "tabela", "item" },
          new object[] { pkTabela, pkItem },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkTabela, string pkItem) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_tabelaitem(),
          new string[] { "tabela", "item" },
          new object[] { pkTabela, pkItem }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkTabela, string pkItem) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_tabelaitem(),
          new string[] { "tabela", "item" },
          new object[] { pkTabela, pkItem }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Tabela { get { return (string)row["tabela", DataRowVersion.Original]; } }
        public string Item { get { return (string)row["item", DataRowVersion.Original]; } }
        public string Descr { get { return (string)row["descr", DataRowVersion.Original]; } }
        public string Sis { get { return (string)row["sis", DataRowVersion.Original]; } }

      }

    }

    public abstract class CustomBase {
      public virtual string PreInsert(Row row, TConnectionWritable cn) { return string.Empty; }
      public virtual string PreUpdate(Row row, TConnectionWritable cn) { return string.Empty; }
      public virtual string PreDelete(Row row, TConnectionWritable cn) { return string.Empty; }
      public virtual string PreUndelete(Row row, TConnectionWritable cn) { return string.Empty; }
      public virtual string PostInsert(Row row, TConnectionWritable cn) { return string.Empty; }
      public virtual string PostUpdate(Row row, TConnectionWritable cn) { return string.Empty; }
      public virtual string PostDelete(Row row, TConnectionWritable cn) { return string.Empty; }
      public virtual string PostUndelete(Row row, TConnectionWritable cn) { return string.Empty; }
    }

    #region Columns private fields and classes
    #region Coluna: TABELA
    private ColumnTabela pvColTabela;

    public sealed class ColumnTabela : TDataColumn {

      public ColumnTabela() {
        this.ColumnName = "tabela";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "tabela";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_tabelaitem.tabela"; } }
      public override string OriginalCol { get { return "tabela"; } }
      public override string RefCol { get { return "HD_TABELAITEM.TABELA"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 20; } }
      public override string SqlServerType { get { return "VARCHAR(20)"; } }

    }
    #endregion

    #region Coluna: ITEM
    private ColumnItem pvColItem;

    public sealed class ColumnItem : TDataColumn {

      public ColumnItem() {
        this.ColumnName = "item";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "item";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_tabelaitem.item"; } }
      public override string OriginalCol { get { return "item"; } }
      public override string RefCol { get { return "HD_TABELAITEM.ITEM"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 40; } }
      public override string SqlServerType { get { return "VARCHAR(40)"; } }

    }
    #endregion

    #region Coluna: DESCR
    private ColumnDescr pvColDescr;

    public sealed class ColumnDescr : TDataColumn {

      public ColumnDescr() {
        this.ColumnName = "descr";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "descrição";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_tabelaitem.descr"; } }
      public override string OriginalCol { get { return "descr"; } }
      public override string RefCol { get { return "HD_TABELAITEM.DESCR"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 180; } }
      public override string SqlServerType { get { return "VARCHAR(180)"; } }

    }
    #endregion

    #region Coluna: SIS
    private ColumnSis pvColSis;

    public sealed class ColumnSis : TDataColumn {

      public ColumnSis() {
        this.ColumnName = "sis";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "sistema";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_tabela.sis"; } }
      public override string OriginalCol { get { return "sis"; } }
      public override string RefCol { get { return "HD_TABELA.SIS"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 12; } }
      public override string SqlServerType { get { return "VARCHAR(12)"; } }

    }
    #endregion
    #endregion
  }
}

