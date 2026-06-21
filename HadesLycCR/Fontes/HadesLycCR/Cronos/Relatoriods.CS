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
  public class RelatorioDS : TDataSet {
    private const string DataSetName_Def = "RelatorioDS";
    private static ConstructorInfo ctorTCustom = null;
    private CustomBase tcustom = null;
    #region Variáveis private para as tabelas deste dataset
    private Hd_grupo_relatorios pvTabHd_grupo_relatorios;
    private Hd_padrel pvTabHd_padrel;
    private Hd_parametro_relatorio pvTabHd_parametro_relatorio;
    private Hd_relatorio pvTabHd_relatorio;
    #endregion

    static RelatorioDS() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("RelatorioDS");
      }
      catch{}
    }

    public RelatorioDS() : base() {
      DataSetName = DataSetName_Def;

      pvTabHd_grupo_relatorios = new Hd_grupo_relatorios();
      pvTabHd_padrel = new Hd_padrel();
      pvTabHd_parametro_relatorio = new Hd_parametro_relatorio();
      pvTabHd_relatorio = new Hd_relatorio();

      Tables.Add(pvTabHd_grupo_relatorios);
      Tables.Add(pvTabHd_padrel);
      Tables.Add(pvTabHd_parametro_relatorio);
      Tables.Add(pvTabHd_relatorio);

      CreateRelations();

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
    }


    [DefaultValue(DataSetName_Def)]
    public new string DataSetName {
      get { return base.DataSetName; }
      set { base.DataSetName = value == null ? string.Empty : value; }
    }

    private void CreateRelations() {
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
          pvTabHd_grupo_relatorios.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_grupo_relatorios); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabHd_padrel.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_padrel); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabHd_parametro_relatorio.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_parametro_relatorio); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabHd_relatorio.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_relatorio); if(row != null) return DataLib.RowErrorsToString(row);
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
    public Hd_grupo_relatorios Hd_grupo_relatorios { get { return pvTabHd_grupo_relatorios; } }

    [
      Browsable(false),
    ]
    public Hd_padrel Hd_padrel { get { return pvTabHd_padrel; } }

    [
      Browsable(false),
    ]
    public Hd_parametro_relatorio Hd_parametro_relatorio { get { return pvTabHd_parametro_relatorio; } }

    [
      Browsable(false),
    ]
    public Hd_relatorio Hd_relatorio { get { return pvTabHd_relatorio; } }

    #endregion

    public abstract class CustomBase {
      public abstract string Put(Techne.HadesLyc.CR.RelatorioDS ds, TConnectionWritable connection);
    }

  }


  public partial class Hd_grupo_relatorios : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_grupo_relatorios() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_grupo_relatorios");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_grupo_relatorios");
      }
      catch{}
    }

    public Hd_grupo_relatorios() {
      pvColSis = new ColumnSis(); Columns.Add(pvColSis);
      pvColGruporelat = new ColumnGruporelat(); Columns.Add(pvColGruporelat);
      pvColDescricao = new ColumnDescricao(); Columns.Add(pvColDescricao);
      pvColArquivo = new ColumnArquivo(); Columns.Add(pvColArquivo);
      pvColMetodo = new ColumnMetodo(); Columns.Add(pvColMetodo);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_grupo_relatorios");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @sis, string @gruporelat, string @descricao) {
      Row row = NewRow();

      row.Sis = @sis;
      row.Gruporelat = @gruporelat;
      row.Descricao = @descricao;

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
      get { return "HD_GRUPO_RELATORIOS"; }
    }

    protected override sealed string Name { get { return "Grupos de relatórios"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColSis, pvColGruporelat });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_grupo_relatorios_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_grupo_relatorios_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_grupo_relatorios_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_grupo_relatorios_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkSis", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "SIS", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkGruporelat", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "GRUPORELAT", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("sis", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SIS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("gruporelat", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "GRUPORELAT", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("descricao", OleDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "DESCRICAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("arquivo", OleDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "ARQUIVO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("metodo", OleDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "METODO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "sis") { result.Add(toLower ? "sis" : column); continue; }
          if(lower == "gruporelat") { result.Add(toLower ? "gruporelat" : column); continue; }
          if(lower == "descricao") { result.Add(toLower ? "descricao" : column); continue; }
          if(lower == "arquivo") { result.Add(toLower ? "arquivo" : column); continue; }
          if(lower == "metodo") { result.Add(toLower ? "metodo" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "sis", "gruporelat", "descricao", "arquivo", "metodo" }
                          : new string[] { "SIS", "GRUPORELAT", "DESCRICAO", "ARQUIVO", "METODO" });
        else
          result.AddRange(toLower
                          ? new string[] { "sis", "gruporelat", "descricao", "arquivo", "metodo" }
                          : new string[] { "SIS", "GRUPORELAT", "DESCRICAO", "ARQUIVO", "METODO" });

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
        "hd_grupo_relatorios.sis", "hd_grupo_relatorios.gruporelat"
      });
      if(scColumns.Contains("descricao")) list.Add("hd_grupo_relatorios.descricao");
      if(scColumns.Contains("arquivo")) list.Add("hd_grupo_relatorios.arquivo");
      if(scColumns.Contains("metodo")) list.Add("hd_grupo_relatorios.metodo");
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
          "FROM hd_grupo_relatorios " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_grupo_relatorios");
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
        da.TableMappings.Add("Table", "Hd_grupo_relatorios");
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
            if (dr["SIS"] != null && string.IsNullOrEmpty(dr["SIS"].ToString()))
              dr["SIS"] = DBNull.Value;
            if (dr["GRUPORELAT"] != null && string.IsNullOrEmpty(dr["GRUPORELAT"].ToString()))
              dr["GRUPORELAT"] = DBNull.Value;
            if (dr["DESCRICAO"] != null && string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
              dr["DESCRICAO"] = DBNull.Value;
            if (dr["ARQUIVO"] != null && string.IsNullOrEmpty(dr["ARQUIVO"].ToString()))
              dr["ARQUIVO"] = DBNull.Value;
            if (dr["METODO"] != null && string.IsNullOrEmpty(dr["METODO"].ToString()))
              dr["METODO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["SIS"] == null || string.IsNullOrEmpty(dr["SIS"].ToString()))
            {
              return "Favor informar o Sis.";
            }

            if (dr["SIS"] != null && !string.IsNullOrEmpty(dr["SIS"].ToString()))
            {
              if (dr["SIS"].ToString().Length > 12)
              {
                return "O Sis deve possuir no máximo 12 caracteres.";
              }
            }

            if (dr["GRUPORELAT"] == null || string.IsNullOrEmpty(dr["GRUPORELAT"].ToString()))
            {
              return "Favor informar o Gruporelat.";
            }

            if (dr["GRUPORELAT"] != null && !string.IsNullOrEmpty(dr["GRUPORELAT"].ToString()))
            {
              if (dr["GRUPORELAT"].ToString().Length > 50)
              {
                return "O Gruporelat deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["DESCRICAO"] == null || string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              return "Favor informar o Descricao.";
            }

            if (dr["DESCRICAO"] != null && !string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              if (dr["DESCRICAO"].ToString().Length > 200)
              {
                return "O Descricao deve possuir no máximo 200 caracteres.";
              }
            }

            if (dr["ARQUIVO"] != null && !string.IsNullOrEmpty(dr["ARQUIVO"].ToString()))
            {
              if (dr["ARQUIVO"].ToString().Length > 200)
              {
                return "O Arquivo deve possuir no máximo 200 caracteres.";
              }
            }

            if (dr["METODO"] != null && !string.IsNullOrEmpty(dr["METODO"].ToString()))
            {
              if (dr["METODO"].ToString().Length > 200)
              {
                return "O Metodo deve possuir no máximo 200 caracteres.";
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

    public static Hd_grupo_relatorios Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_grupo_relatorios Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_grupo_relatorios tab = new Hd_grupo_relatorios();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_grupo_relatorios tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @sis, string @gruporelat, string @descricao, string @arquivo, string @metodo
      ) {
        return (Row)Add(new object[] {
          @sis, @gruporelat, @descricao, @arquivo, @metodo
        });
      }

      public Row Find(
        string @sis, string @gruporelat
      ) {
        return (Row)Find(new object[] {
          @sis, @gruporelat
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
      /// SIS: VARCHAR(12), Primary Key, Not Null
      /// </summary>
      public string Sis { get { return (string)this["sis"]; } set { this["sis"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// GRUPORELAT: VARCHAR(50), Primary Key, Not Null
      /// </summary>
      public string Gruporelat { get { return (string)this["gruporelat"]; } set { this["gruporelat"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO: VARCHAR(200), Not Null
      /// </summary>
      public string Descricao { get { return (string)this["descricao"]; } set { this["descricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ARQUIVO: VARCHAR(200)
      /// </summary>
      public string Arquivo { get { return (string)this["arquivo"]; } set { this["arquivo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// METODO : VARCHAR(200)
      /// </summary>
      public string Metodo { get { return (string)this["metodo"]; } set { this["metodo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @sis, string @gruporelat, string @descricao) {
        return Insert(connection,
                      @sis, @gruporelat, @descricao,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @sis, string @gruporelat, string @descricao,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_grupo_relatorios(),
          new string[] { "sis", "gruporelat", "descricao" },
          new object[] { @sis, @gruporelat, @descricao },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkSis, string pkGruporelat,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_grupo_relatorios(),
          new string[] { "sis", "gruporelat" },
          new object[] { pkSis, pkGruporelat },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkSis, string pkGruporelat) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_grupo_relatorios(),
          new string[] { "sis", "gruporelat" },
          new object[] { pkSis, pkGruporelat }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkSis, string pkGruporelat) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_grupo_relatorios(),
          new string[] { "sis", "gruporelat" },
          new object[] { pkSis, pkGruporelat }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Sis { get { return (string)row["sis", DataRowVersion.Original]; } }
        public string Gruporelat { get { return (string)row["gruporelat", DataRowVersion.Original]; } }
        public string Descricao { get { return (string)row["descricao", DataRowVersion.Original]; } }
        public string Arquivo { get { return (string)row["arquivo", DataRowVersion.Original]; } }
        public string Metodo { get { return (string)row["metodo", DataRowVersion.Original]; } }

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
    #region Coluna: SIS
    private ColumnSis pvColSis;

    public sealed class ColumnSis : TDataColumn {

      public ColumnSis() {
        this.ColumnName = "sis";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_grupo_relatorios.sis"; } }
      public override string OriginalCol { get { return "sis"; } }
      public override string RefCol { get { return "HD_GRUPO_RELATORIOS.SIS"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 12; } }
      public override string SqlServerType { get { return "VARCHAR(12)"; } }

    }
    #endregion

    #region Coluna: GRUPORELAT
    private ColumnGruporelat pvColGruporelat;

    public sealed class ColumnGruporelat : TDataColumn {

      public ColumnGruporelat() {
        this.ColumnName = "gruporelat";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_grupo_relatorios.gruporelat"; } }
      public override string OriginalCol { get { return "gruporelat"; } }
      public override string RefCol { get { return "HD_GRUPO_RELATORIOS.GRUPORELAT"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 50; } }
      public override string SqlServerType { get { return "VARCHAR(50)"; } }

    }
    #endregion

    #region Coluna: DESCRICAO
    private ColumnDescricao pvColDescricao;

    public sealed class ColumnDescricao : TDataColumn {

      public ColumnDescricao() {
        this.ColumnName = "descricao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_grupo_relatorios.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "HD_GRUPO_RELATORIOS.DESCRICAO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 200; } }
      public override string SqlServerType { get { return "VARCHAR(200)"; } }

    }
    #endregion

    #region Coluna: ARQUIVO
    private ColumnArquivo pvColArquivo;

    public sealed class ColumnArquivo : TDataColumn {

      public ColumnArquivo() {
        this.ColumnName = "arquivo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_grupo_relatorios.arquivo"; } }
      public override string OriginalCol { get { return "arquivo"; } }
      public override string RefCol { get { return "HD_GRUPO_RELATORIOS.ARQUIVO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 200; } }
      public override string SqlServerType { get { return "VARCHAR(200)"; } }

    }
    #endregion

    #region Coluna: METODO
    private ColumnMetodo pvColMetodo;

    public sealed class ColumnMetodo : TDataColumn {

      public ColumnMetodo() {
        this.ColumnName = "metodo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_grupo_relatorios.metodo"; } }
      public override string OriginalCol { get { return "metodo"; } }
      public override string RefCol { get { return "HD_GRUPO_RELATORIOS.METODO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 200; } }
      public override string SqlServerType { get { return "VARCHAR(200)"; } }

    }
    #endregion
    #endregion
  }

  public partial class Hd_padrel : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_padrel() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_padrel");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_padrel");
      }
      catch{}
    }

    public Hd_padrel() {
      pvColPadaces = new ColumnPadaces(); Columns.Add(pvColPadaces);
      pvColSis = new ColumnSis(); Columns.Add(pvColSis);
      pvColGruporelat = new ColumnGruporelat(); Columns.Add(pvColGruporelat);
      pvColRelatorio = new ColumnRelatorio(); Columns.Add(pvColRelatorio);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_padrel");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @padaces, string @sis, string @gruporelat, string @relatorio) {
      Row row = NewRow();

      row.Padaces = @padaces;
      row.Sis = @sis;
      row.Gruporelat = @gruporelat;
      row.Relatorio = @relatorio;

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
      get { return "HD_PADREL"; }
    }

    protected override sealed string Name { get { return "Padrão de acesso e seus relatórios"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColPadaces, pvColSis, pvColGruporelat, pvColRelatorio });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_padrel_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_padrel_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_padrel_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_padrel_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkPadaces", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "PADACES", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkSis", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "SIS", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkGruporelat", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "GRUPORELAT", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkRelatorio", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "RELATORIO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("padaces", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PADACES", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("sis", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SIS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("gruporelat", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "GRUPORELAT", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("relatorio", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "RELATORIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "padaces") { result.Add(toLower ? "padaces" : column); continue; }
          if(lower == "sis") { result.Add(toLower ? "sis" : column); continue; }
          if(lower == "gruporelat") { result.Add(toLower ? "gruporelat" : column); continue; }
          if(lower == "relatorio") { result.Add(toLower ? "relatorio" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "padaces", "sis", "gruporelat", "relatorio" }
                          : new string[] { "PADACES", "SIS", "GRUPORELAT", "RELATORIO" });
        else
          result.AddRange(toLower
                          ? new string[] { "padaces", "sis", "gruporelat", "relatorio" }
                          : new string[] { "PADACES", "SIS", "GRUPORELAT", "RELATORIO" });

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
        "hd_padrel.padaces", "hd_padrel.sis", "hd_padrel.gruporelat", "hd_padrel.relatorio"
      });
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
          "FROM hd_padrel " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_padrel");
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
        da.TableMappings.Add("Table", "Hd_padrel");
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
            if (dr["PADACES"] != null && string.IsNullOrEmpty(dr["PADACES"].ToString()))
              dr["PADACES"] = DBNull.Value;
            if (dr["SIS"] != null && string.IsNullOrEmpty(dr["SIS"].ToString()))
              dr["SIS"] = DBNull.Value;
            if (dr["GRUPORELAT"] != null && string.IsNullOrEmpty(dr["GRUPORELAT"].ToString()))
              dr["GRUPORELAT"] = DBNull.Value;
            if (dr["RELATORIO"] != null && string.IsNullOrEmpty(dr["RELATORIO"].ToString()))
              dr["RELATORIO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["PADACES"] == null || string.IsNullOrEmpty(dr["PADACES"].ToString()))
            {
              return "Favor informar o padrão de acesso.";
            }

            if (dr["PADACES"] != null && !string.IsNullOrEmpty(dr["PADACES"].ToString()))
            {
              if (dr["PADACES"].ToString().Length > 14)
              {
                return "O padrão de acesso deve possuir no máximo 14 caracteres.";
              }
            }

            if (dr["SIS"] == null || string.IsNullOrEmpty(dr["SIS"].ToString()))
            {
              return "Favor informar o sistema.";
            }

            if (dr["SIS"] != null && !string.IsNullOrEmpty(dr["SIS"].ToString()))
            {
              if (dr["SIS"].ToString().Length > 12)
              {
                return "O sistema deve possuir no máximo 12 caracteres.";
              }
            }

            if (dr["GRUPORELAT"] == null || string.IsNullOrEmpty(dr["GRUPORELAT"].ToString()))
            {
              return "Favor informar o grupo relatório.";
            }

            if (dr["GRUPORELAT"] != null && !string.IsNullOrEmpty(dr["GRUPORELAT"].ToString()))
            {
              if (dr["GRUPORELAT"].ToString().Length > 50)
              {
                return "O grupo relatório deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["RELATORIO"] == null || string.IsNullOrEmpty(dr["RELATORIO"].ToString()))
            {
              return "Favor informar o relatório.";
            }

            if (dr["RELATORIO"] != null && !string.IsNullOrEmpty(dr["RELATORIO"].ToString()))
            {
              if (dr["RELATORIO"].ToString().Length > 50)
              {
                return "O relatório deve possuir no máximo 50 caracteres.";
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

    public static Hd_padrel Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_padrel Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_padrel tab = new Hd_padrel();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_padrel tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @padaces, string @sis, string @gruporelat, string @relatorio
      ) {
        return (Row)Add(new object[] {
          @padaces, @sis, @gruporelat, @relatorio
        });
      }

      public Row Find(
        string @padaces, string @sis, string @gruporelat, string @relatorio
      ) {
        return (Row)Find(new object[] {
          @padaces, @sis, @gruporelat, @relatorio
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
      /// PADACES: VARCHAR(14), Primary Key, Not Null
      /// </summary>
      public string Padaces { get { return (string)this["padaces"]; } set { this["padaces"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SIS: VARCHAR(12), Primary Key, Not Null
      /// </summary>
      public string Sis { get { return (string)this["sis"]; } set { this["sis"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// GRUPORELAT: VARCHAR(50), Primary Key, Not Null
      /// </summary>
      public string Gruporelat { get { return (string)this["gruporelat"]; } set { this["gruporelat"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// RELATORIO: VARCHAR(50), Primary Key, Not Null
      /// </summary>
      public string Relatorio { get { return (string)this["relatorio"]; } set { this["relatorio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @padaces, string @sis, string @gruporelat, string @relatorio) {
        return Insert(connection,
                      @padaces, @sis, @gruporelat, @relatorio,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @padaces, string @sis, string @gruporelat, string @relatorio,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_padrel(),
          new string[] { "padaces", "sis", "gruporelat", "relatorio" },
          new object[] { @padaces, @sis, @gruporelat, @relatorio },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkPadaces, string pkSis, string pkGruporelat, string pkRelatorio,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_padrel(),
          new string[] { "padaces", "sis", "gruporelat", "relatorio" },
          new object[] { pkPadaces, pkSis, pkGruporelat, pkRelatorio },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkPadaces, string pkSis, string pkGruporelat, string pkRelatorio) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_padrel(),
          new string[] { "padaces", "sis", "gruporelat", "relatorio" },
          new object[] { pkPadaces, pkSis, pkGruporelat, pkRelatorio }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkPadaces, string pkSis, string pkGruporelat, string pkRelatorio) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_padrel(),
          new string[] { "padaces", "sis", "gruporelat", "relatorio" },
          new object[] { pkPadaces, pkSis, pkGruporelat, pkRelatorio }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Padaces { get { return (string)row["padaces", DataRowVersion.Original]; } }
        public string Sis { get { return (string)row["sis", DataRowVersion.Original]; } }
        public string Gruporelat { get { return (string)row["gruporelat", DataRowVersion.Original]; } }
        public string Relatorio { get { return (string)row["relatorio", DataRowVersion.Original]; } }

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
    #region Coluna: PADACES
    private ColumnPadaces pvColPadaces;

    public sealed class ColumnPadaces : TDataColumn {

      public ColumnPadaces() {
        this.ColumnName = "padaces";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "padrão de acesso";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_padrel.padaces"; } }
      public override string OriginalCol { get { return "padaces"; } }
      public override string RefCol { get { return "HD_PADREL.PADACES"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 14; } }
      public override string SqlServerType { get { return "VARCHAR(14)"; } }

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
      public override string FullCol { get { return "hd_padrel.sis"; } }
      public override string OriginalCol { get { return "sis"; } }
      public override string RefCol { get { return "HD_PADREL.SIS"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 12; } }
      public override string SqlServerType { get { return "VARCHAR(12)"; } }

    }
    #endregion

    #region Coluna: GRUPORELAT
    private ColumnGruporelat pvColGruporelat;

    public sealed class ColumnGruporelat : TDataColumn {

      public ColumnGruporelat() {
        this.ColumnName = "gruporelat";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "grupo relatório";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_padrel.gruporelat"; } }
      public override string OriginalCol { get { return "gruporelat"; } }
      public override string RefCol { get { return "HD_PADREL.GRUPORELAT"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 50; } }
      public override string SqlServerType { get { return "VARCHAR(50)"; } }

    }
    #endregion

    #region Coluna: RELATORIO
    private ColumnRelatorio pvColRelatorio;

    public sealed class ColumnRelatorio : TDataColumn {

      public ColumnRelatorio() {
        this.ColumnName = "relatorio";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "relatório";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_padrel.relatorio"; } }
      public override string OriginalCol { get { return "relatorio"; } }
      public override string RefCol { get { return "HD_PADREL.RELATORIO"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 50; } }
      public override string SqlServerType { get { return "VARCHAR(50)"; } }

    }
    #endregion
    #endregion
  }

  public partial class Hd_parametro_relatorio : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_parametro_relatorio() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_parametro_relatorio");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_parametro_relatorio");
      }
      catch{}
    }

    public Hd_parametro_relatorio() {
      pvColSis = new ColumnSis(); Columns.Add(pvColSis);
      pvColGruporelat = new ColumnGruporelat(); Columns.Add(pvColGruporelat);
      pvColRelatorio = new ColumnRelatorio(); Columns.Add(pvColRelatorio);
      pvColParametro = new ColumnParametro(); Columns.Add(pvColParametro);
      pvColOrdem = new ColumnOrdem(); Columns.Add(pvColOrdem);
      pvColTipo = new ColumnTipo(); Columns.Add(pvColTipo);
      pvColDescricao = new ColumnDescricao(); Columns.Add(pvColDescricao);
      pvColObrigatorio = new ColumnObrigatorio(); Columns.Add(pvColObrigatorio);
      pvColValor = new ColumnValor(); Columns.Add(pvColValor);
      pvColSql_texto = new ColumnSql_texto(); Columns.Add(pvColSql_texto);
      pvColCol_valor = new ColumnCol_valor(); Columns.Add(pvColCol_valor);
      pvColCol_descr = new ColumnCol_descr(); Columns.Add(pvColCol_descr);
      pvColNomes_colunas = new ColumnNomes_colunas(); Columns.Add(pvColNomes_colunas);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_parametro_relatorio");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @sis, string @gruporelat, string @relatorio, string @parametro, 
                      decimal? @ordem, decimal? @tipo, string @obrigatorio) {
      Row row = NewRow();

      row.Sis = @sis;
      row.Gruporelat = @gruporelat;
      row.Relatorio = @relatorio;
      row.Parametro = @parametro;
      row.Ordem = @ordem;
      row.Tipo = @tipo;
      row.Obrigatorio = @obrigatorio;

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
      get { return "HD_PARAMETRO_RELATORIO"; }
    }

    protected override sealed string Name { get { return "Parâmetros dos relatórios"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColSis, pvColGruporelat, pvColRelatorio, pvColParametro });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_parametro_relatorio_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_parametro_relatorio_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_parametro_relatorio_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_parametro_relatorio_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkSis", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "SIS", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkGruporelat", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "GRUPORELAT", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkRelatorio", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "RELATORIO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkParametro", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "PARAMETRO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("sis", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SIS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("gruporelat", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "GRUPORELAT", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("relatorio", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "RELATORIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("parametro", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PARAMETRO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("ordem", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ORDEM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("tipo", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "TIPO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("descricao", OleDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "DESCRICAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("obrigatorio", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "OBRIGATORIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("valor", OleDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "VALOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("sql_texto", OleDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "SQL_TEXTO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("col_valor", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "COL_VALOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("col_descr", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "COL_DESCR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("nomes_colunas", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NOMES_COLUNAS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "sis") { result.Add(toLower ? "sis" : column); continue; }
          if(lower == "gruporelat") { result.Add(toLower ? "gruporelat" : column); continue; }
          if(lower == "relatorio") { result.Add(toLower ? "relatorio" : column); continue; }
          if(lower == "parametro") { result.Add(toLower ? "parametro" : column); continue; }
          if(lower == "ordem") { result.Add(toLower ? "ordem" : column); continue; }
          if(lower == "tipo") { result.Add(toLower ? "tipo" : column); continue; }
          if(lower == "descricao") { result.Add(toLower ? "descricao" : column); continue; }
          if(lower == "obrigatorio") { result.Add(toLower ? "obrigatorio" : column); continue; }
          if(lower == "valor") { result.Add(toLower ? "valor" : column); continue; }
          if(lower == "sql_texto") { result.Add(toLower ? "sql_texto" : column); continue; }
          if(lower == "col_valor") { result.Add(toLower ? "col_valor" : column); continue; }
          if(lower == "col_descr") { result.Add(toLower ? "col_descr" : column); continue; }
          if(lower == "nomes_colunas") { result.Add(toLower ? "nomes_colunas" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "sis", "gruporelat", "relatorio", "parametro", "ordem", "tipo", 
                                           "descricao", "obrigatorio", "valor", "sql_texto", "col_valor", "col_descr", "nomes_colunas" }
                          : new string[] { "SIS", "GRUPORELAT", "RELATORIO", "PARAMETRO", "ORDEM", "TIPO", 
                                           "DESCRICAO", "OBRIGATORIO", "VALOR", "SQL_TEXTO", "COL_VALOR", "COL_DESCR", "NOMES_COLUNAS" });
        else
          result.AddRange(toLower
                          ? new string[] { "sis", "gruporelat", "relatorio", "parametro", "ordem", "tipo", 
                                           "descricao", "obrigatorio", "valor", "sql_texto", "col_valor", "col_descr", "nomes_colunas" }
                          : new string[] { "SIS", "GRUPORELAT", "RELATORIO", "PARAMETRO", "ORDEM", "TIPO", 
                                           "DESCRICAO", "OBRIGATORIO", "VALOR", "SQL_TEXTO", "COL_VALOR", "COL_DESCR", "NOMES_COLUNAS" });

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
        "hd_parametro_relatorio.sis", "hd_parametro_relatorio.gruporelat", "hd_parametro_relatorio.relatorio", 
        "hd_parametro_relatorio.parametro"
      });
      if(scColumns.Contains("ordem")) list.Add("hd_parametro_relatorio.ordem");
      if(scColumns.Contains("tipo")) list.Add("hd_parametro_relatorio.tipo");
      if(scColumns.Contains("descricao")) list.Add("hd_parametro_relatorio.descricao");
      if(scColumns.Contains("obrigatorio")) list.Add("hd_parametro_relatorio.obrigatorio");
      if(scColumns.Contains("valor")) list.Add("hd_parametro_relatorio.valor");
      if(scColumns.Contains("sql_texto")) list.Add("hd_parametro_relatorio.sql_texto");
      if(scColumns.Contains("col_valor")) list.Add("hd_parametro_relatorio.col_valor");
      if(scColumns.Contains("col_descr")) list.Add("hd_parametro_relatorio.col_descr");
      if(scColumns.Contains("nomes_colunas")) list.Add("hd_parametro_relatorio.nomes_colunas");
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
          "FROM hd_parametro_relatorio " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_parametro_relatorio");
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
        da.TableMappings.Add("Table", "Hd_parametro_relatorio");
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
            if (dr["SIS"] != null && string.IsNullOrEmpty(dr["SIS"].ToString()))
              dr["SIS"] = DBNull.Value;
            if (dr["GRUPORELAT"] != null && string.IsNullOrEmpty(dr["GRUPORELAT"].ToString()))
              dr["GRUPORELAT"] = DBNull.Value;
            if (dr["RELATORIO"] != null && string.IsNullOrEmpty(dr["RELATORIO"].ToString()))
              dr["RELATORIO"] = DBNull.Value;
            if (dr["PARAMETRO"] != null && string.IsNullOrEmpty(dr["PARAMETRO"].ToString()))
              dr["PARAMETRO"] = DBNull.Value;
            if (dr["ORDEM"] != null && string.IsNullOrEmpty(dr["ORDEM"].ToString()))
              dr["ORDEM"] = DBNull.Value;
            if (dr["TIPO"] != null && string.IsNullOrEmpty(dr["TIPO"].ToString()))
              dr["TIPO"] = DBNull.Value;
            if (dr["DESCRICAO"] != null && string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
              dr["DESCRICAO"] = DBNull.Value;
            if (dr["OBRIGATORIO"] != null && string.IsNullOrEmpty(dr["OBRIGATORIO"].ToString()))
              dr["OBRIGATORIO"] = DBNull.Value;
            if (dr["VALOR"] != null && string.IsNullOrEmpty(dr["VALOR"].ToString()))
              dr["VALOR"] = DBNull.Value;
            if (dr["SQL_TEXTO"] != null && string.IsNullOrEmpty(dr["SQL_TEXTO"].ToString()))
              dr["SQL_TEXTO"] = DBNull.Value;
            if (dr["COL_VALOR"] != null && string.IsNullOrEmpty(dr["COL_VALOR"].ToString()))
              dr["COL_VALOR"] = DBNull.Value;
            if (dr["COL_DESCR"] != null && string.IsNullOrEmpty(dr["COL_DESCR"].ToString()))
              dr["COL_DESCR"] = DBNull.Value;
            if (dr["NOMES_COLUNAS"] != null && string.IsNullOrEmpty(dr["NOMES_COLUNAS"].ToString()))
              dr["NOMES_COLUNAS"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["SIS"] == null || string.IsNullOrEmpty(dr["SIS"].ToString()))
            {
              return "Favor informar o Sis.";
            }

            if (dr["SIS"] != null && !string.IsNullOrEmpty(dr["SIS"].ToString()))
            {
              if (dr["SIS"].ToString().Length > 12)
              {
                return "O Sis deve possuir no máximo 12 caracteres.";
              }
            }

            if (dr["GRUPORELAT"] == null || string.IsNullOrEmpty(dr["GRUPORELAT"].ToString()))
            {
              return "Favor informar o Gruporelat.";
            }

            if (dr["GRUPORELAT"] != null && !string.IsNullOrEmpty(dr["GRUPORELAT"].ToString()))
            {
              if (dr["GRUPORELAT"].ToString().Length > 50)
              {
                return "O Gruporelat deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["RELATORIO"] == null || string.IsNullOrEmpty(dr["RELATORIO"].ToString()))
            {
              return "Favor informar o Relatorio.";
            }

            if (dr["RELATORIO"] != null && !string.IsNullOrEmpty(dr["RELATORIO"].ToString()))
            {
              if (dr["RELATORIO"].ToString().Length > 50)
              {
                return "O Relatorio deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["PARAMETRO"] == null || string.IsNullOrEmpty(dr["PARAMETRO"].ToString()))
            {
              return "Favor informar o Parametro.";
            }

            if (dr["PARAMETRO"] != null && !string.IsNullOrEmpty(dr["PARAMETRO"].ToString()))
            {
              if (dr["PARAMETRO"].ToString().Length > 50)
              {
                return "O Parametro deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["ORDEM"] == null || string.IsNullOrEmpty(dr["ORDEM"].ToString()))
            {
              return "Favor informar o Ordem.";
            }

            if (dr["ORDEM"] != null && !string.IsNullOrEmpty(dr["ORDEM"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ORDEM"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ORDEM"]).ToString("0.############").Split(',')[0].Length > 2) || (Convert.ToDecimal(dr["ORDEM"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ORDEM"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Ordem deve ser um valor inteiro com 2 dígitos no máximo.";
              }

              if (dr["ORDEM"] != null && !string.IsNullOrEmpty(dr["ORDEM"].ToString()))
              {
                if (Convert.ToDecimal(dr["ORDEM"]) < (1))
                {
                  return "O Ordem deve ser maior ou igual a (1).";
                }
              }
            }

            if (dr["TIPO"] == null || string.IsNullOrEmpty(dr["TIPO"].ToString()))
            {
              return "Favor informar o Tipo.";
            }

            if (dr["TIPO"] != null && !string.IsNullOrEmpty(dr["TIPO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["TIPO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["TIPO"]).ToString("0.############").Split(',')[0].Length > 2) || (Convert.ToDecimal(dr["TIPO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["TIPO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Tipo deve ser um valor inteiro com 2 dígitos no máximo.";
              }

              if (dr["TIPO"] != null && !string.IsNullOrEmpty(dr["TIPO"].ToString()))
              {
                if (Convert.ToDecimal(dr["TIPO"]) < (0))
                {
                  return "O Tipo deve ser maior ou igual a (0).";
                }
              }

              if (dr["TIPO"] != null && !string.IsNullOrEmpty(dr["TIPO"].ToString()))
              {
                if (Convert.ToDecimal(dr["TIPO"]) > (15))
                {
                  return "O Tipo deve ser menor ou igual a (15).";
                }
              }
            }

            if (dr["DESCRICAO"] != null && !string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              if (dr["DESCRICAO"].ToString().Length > 200)
              {
                return "O Descricao deve possuir no máximo 200 caracteres.";
              }
            }

            if (dr["OBRIGATORIO"] == null || string.IsNullOrEmpty(dr["OBRIGATORIO"].ToString()))
            {
              return "Favor informar o Obrigatorio.";
            }

            if (dr["OBRIGATORIO"] != null && !string.IsNullOrEmpty(dr["OBRIGATORIO"].ToString()))
            {
              if (dr["OBRIGATORIO"].ToString().Length > 1)
              {
                return "O Obrigatorio deve possuir no máximo 1 caractere.";
              }

              if (dr["OBRIGATORIO"] != null && !string.IsNullOrEmpty(dr["OBRIGATORIO"].ToString()))
              {
                List<object> lstOBRIGATORIO = new List<object>(new object[]{"N", "S"});
                if(!lstOBRIGATORIO.Contains(dr["OBRIGATORIO"].ToString()))
                {
                  return "Os valores permitidos para o Obrigatorio são ''N'' ou ''S''.";
                }
              }
            }

            if (dr["VALOR"] != null && !string.IsNullOrEmpty(dr["VALOR"].ToString()))
            {
              if (dr["VALOR"].ToString().Length > 200)
              {
                return "O Valor deve possuir no máximo 200 caracteres.";
              }
            }

            if (dr["SQL_TEXTO"] != null && !string.IsNullOrEmpty(dr["SQL_TEXTO"].ToString()))
            {
              if (dr["SQL_TEXTO"].ToString().Length > 200)
              {
                return "O Sql_texto deve possuir no máximo 200 caracteres.";
              }
            }

            if (dr["COL_VALOR"] != null && !string.IsNullOrEmpty(dr["COL_VALOR"].ToString()))
            {
              if (dr["COL_VALOR"].ToString().Length > 50)
              {
                return "O Col_valor deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["COL_DESCR"] != null && !string.IsNullOrEmpty(dr["COL_DESCR"].ToString()))
            {
              if (dr["COL_DESCR"].ToString().Length > 50)
              {
                return "O Col_descr deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["NOMES_COLUNAS"] != null && !string.IsNullOrEmpty(dr["NOMES_COLUNAS"].ToString()))
            {
              if (dr["NOMES_COLUNAS"].ToString().Length > 100)
              {
                return "O Nomes_colunas deve possuir no máximo 100 caracteres.";
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

    public static Hd_parametro_relatorio Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_parametro_relatorio Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_parametro_relatorio tab = new Hd_parametro_relatorio();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_parametro_relatorio tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @sis, string @gruporelat, string @relatorio, string @parametro, decimal? @ordem, 
        decimal? @tipo, string @descricao, string @obrigatorio, string @valor, string @sql_texto, 
        string @col_valor, string @col_descr, string @nomes_colunas
      ) {
        return (Row)Add(new object[] {
          @sis, @gruporelat, @relatorio, @parametro, @ordem, @tipo, @descricao, @obrigatorio, 
          @valor, @sql_texto, @col_valor, @col_descr, @nomes_colunas
        });
      }

      public Row Find(
        string @sis, string @gruporelat, string @relatorio, string @parametro
      ) {
        return (Row)Find(new object[] {
          @sis, @gruporelat, @relatorio, @parametro
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
      /// SIS: VARCHAR(12), Primary Key, Not Null
      /// </summary>
      public string Sis { get { return (string)this["sis"]; } set { this["sis"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// GRUPORELAT: VARCHAR(50), Primary Key, Not Null
      /// </summary>
      public string Gruporelat { get { return (string)this["gruporelat"]; } set { this["gruporelat"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// RELATORIO: VARCHAR(50), Primary Key, Not Null
      /// </summary>
      public string Relatorio { get { return (string)this["relatorio"]; } set { this["relatorio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PARAMETRO: VARCHAR(50), Primary Key, Not Null
      /// </summary>
      public string Parametro { get { return (string)this["parametro"]; } set { this["parametro"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ORDEM: NUMERIC(2), Not Null
      /// </summary>
      public decimal? Ordem { get { return (decimal?)this["ordem"]; } set { this["ordem"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TIPO: NUMERIC(2), Not Null
      /// </summary>
      public decimal? Tipo { get { return (decimal?)this["tipo"]; } set { this["tipo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO: VARCHAR(200)
      /// </summary>
      public string Descricao { get { return (string)this["descricao"]; } set { this["descricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// OBRIGATORIO: CHAR(1), Not Null
      /// </summary>
      public string Obrigatorio { get { return (string)this["obrigatorio"]; } set { this["obrigatorio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// VALOR: VARCHAR(200)
      /// </summary>
      public string Valor { get { return (string)this["valor"]; } set { this["valor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SQL_TEXTO: VARCHAR(200)
      /// </summary>
      public string Sql_texto { get { return (string)this["sql_texto"]; } set { this["sql_texto"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// COL_VALOR: VARCHAR(50)
      /// </summary>
      public string Col_valor { get { return (string)this["col_valor"]; } set { this["col_valor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// COL_DESCR: VARCHAR(50)
      /// </summary>
      public string Col_descr { get { return (string)this["col_descr"]; } set { this["col_descr"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOMES_COLUNAS: VARCHAR(100)
      /// </summary>
      public string Nomes_colunas { get { return (string)this["nomes_colunas"]; } set { this["nomes_colunas"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @sis, string @gruporelat, string @relatorio, string @parametro, decimal? @ordem, 
                               decimal? @tipo, string @obrigatorio) {
        return Insert(connection,
                      @sis, @gruporelat, @relatorio, @parametro, @ordem, @tipo, @obrigatorio,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @sis, string @gruporelat, string @relatorio, string @parametro, decimal? @ordem, 
                               decimal? @tipo, string @obrigatorio,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_parametro_relatorio(),
          new string[] { "sis", "gruporelat", "relatorio", "parametro", "ordem", "tipo", "obrigatorio" },
          new object[] { @sis, @gruporelat, @relatorio, @parametro, @ordem, @tipo, @obrigatorio },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkSis, string pkGruporelat, string pkRelatorio, string pkParametro,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_parametro_relatorio(),
          new string[] { "sis", "gruporelat", "relatorio", "parametro" },
          new object[] { pkSis, pkGruporelat, pkRelatorio, pkParametro },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkSis, string pkGruporelat, string pkRelatorio, string pkParametro) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_parametro_relatorio(),
          new string[] { "sis", "gruporelat", "relatorio", "parametro" },
          new object[] { pkSis, pkGruporelat, pkRelatorio, pkParametro }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkSis, string pkGruporelat, string pkRelatorio, string pkParametro) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_parametro_relatorio(),
          new string[] { "sis", "gruporelat", "relatorio", "parametro" },
          new object[] { pkSis, pkGruporelat, pkRelatorio, pkParametro }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Sis { get { return (string)row["sis", DataRowVersion.Original]; } }
        public string Gruporelat { get { return (string)row["gruporelat", DataRowVersion.Original]; } }
        public string Relatorio { get { return (string)row["relatorio", DataRowVersion.Original]; } }
        public string Parametro { get { return (string)row["parametro", DataRowVersion.Original]; } }
        public decimal? Ordem { get { return (decimal?)row["ordem", DataRowVersion.Original]; } }
        public decimal? Tipo { get { return (decimal?)row["tipo", DataRowVersion.Original]; } }
        public string Descricao { get { return (string)row["descricao", DataRowVersion.Original]; } }
        public string Obrigatorio { get { return (string)row["obrigatorio", DataRowVersion.Original]; } }
        public string Valor { get { return (string)row["valor", DataRowVersion.Original]; } }
        public string Sql_texto { get { return (string)row["sql_texto", DataRowVersion.Original]; } }
        public string Col_valor { get { return (string)row["col_valor", DataRowVersion.Original]; } }
        public string Col_descr { get { return (string)row["col_descr", DataRowVersion.Original]; } }
        public string Nomes_colunas { get { return (string)row["nomes_colunas", DataRowVersion.Original]; } }

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
    #region Coluna: SIS
    private ColumnSis pvColSis;

    public sealed class ColumnSis : TDataColumn {

      public ColumnSis() {
        this.ColumnName = "sis";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_parametro_relatorio.sis"; } }
      public override string OriginalCol { get { return "sis"; } }
      public override string RefCol { get { return "HD_PARAMETRO_RELATORIO.SIS"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 12; } }
      public override string SqlServerType { get { return "VARCHAR(12)"; } }

    }
    #endregion

    #region Coluna: GRUPORELAT
    private ColumnGruporelat pvColGruporelat;

    public sealed class ColumnGruporelat : TDataColumn {

      public ColumnGruporelat() {
        this.ColumnName = "gruporelat";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_parametro_relatorio.gruporelat"; } }
      public override string OriginalCol { get { return "gruporelat"; } }
      public override string RefCol { get { return "HD_PARAMETRO_RELATORIO.GRUPORELAT"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 50; } }
      public override string SqlServerType { get { return "VARCHAR(50)"; } }

    }
    #endregion

    #region Coluna: RELATORIO
    private ColumnRelatorio pvColRelatorio;

    public sealed class ColumnRelatorio : TDataColumn {

      public ColumnRelatorio() {
        this.ColumnName = "relatorio";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_parametro_relatorio.relatorio"; } }
      public override string OriginalCol { get { return "relatorio"; } }
      public override string RefCol { get { return "HD_PARAMETRO_RELATORIO.RELATORIO"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 50; } }
      public override string SqlServerType { get { return "VARCHAR(50)"; } }

    }
    #endregion

    #region Coluna: PARAMETRO
    private ColumnParametro pvColParametro;

    public sealed class ColumnParametro : TDataColumn {

      public ColumnParametro() {
        this.ColumnName = "parametro";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_parametro_relatorio.parametro"; } }
      public override string OriginalCol { get { return "parametro"; } }
      public override string RefCol { get { return "HD_PARAMETRO_RELATORIO.PARAMETRO"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 50; } }
      public override string SqlServerType { get { return "VARCHAR(50)"; } }

    }
    #endregion

    #region Coluna: ORDEM
    private ColumnOrdem pvColOrdem;

    public sealed class ColumnOrdem : TDataColumn {

      public ColumnOrdem() {
        this.ColumnName = "ordem";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_parametro_relatorio.ordem"; } }
      public override string OriginalCol { get { return "ordem"; } }
      public override string RefCol { get { return "HD_PARAMETRO_RELATORIO.ORDEM"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
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
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_parametro_relatorio.tipo"; } }
      public override string OriginalCol { get { return "tipo"; } }
      public override string RefCol { get { return "HD_PARAMETRO_RELATORIO.TIPO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 3; } }
      public override int Precision { get { return 2; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(2)"; } }

      public override object MinValue { get { return 0; } }

      public override object MaxValue { get { return 15; } }

    }
    #endregion

    #region Coluna: DESCRICAO
    private ColumnDescricao pvColDescricao;

    public sealed class ColumnDescricao : TDataColumn {

      public ColumnDescricao() {
        this.ColumnName = "descricao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_parametro_relatorio.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "HD_PARAMETRO_RELATORIO.DESCRICAO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 200; } }
      public override string SqlServerType { get { return "VARCHAR(200)"; } }

    }
    #endregion

    #region Coluna: OBRIGATORIO
    private ColumnObrigatorio pvColObrigatorio;

    public sealed class ColumnObrigatorio : TDataColumn {

      public ColumnObrigatorio() {
        this.ColumnName = "obrigatorio";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_parametro_relatorio.obrigatorio"; } }
      public override string OriginalCol { get { return "obrigatorio"; } }
      public override string RefCol { get { return "HD_PARAMETRO_RELATORIO.OBRIGATORIO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 1; } }
      public override string SqlServerType { get { return "CHAR(1)"; } }

    }
    #endregion

    #region Coluna: VALOR
    private ColumnValor pvColValor;

    public sealed class ColumnValor : TDataColumn {

      public ColumnValor() {
        this.ColumnName = "valor";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_parametro_relatorio.valor"; } }
      public override string OriginalCol { get { return "valor"; } }
      public override string RefCol { get { return "HD_PARAMETRO_RELATORIO.VALOR"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 200; } }
      public override string SqlServerType { get { return "VARCHAR(200)"; } }

    }
    #endregion

    #region Coluna: SQL_TEXTO
    private ColumnSql_texto pvColSql_texto;

    public sealed class ColumnSql_texto : TDataColumn {

      public ColumnSql_texto() {
        this.ColumnName = "sql_texto";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_parametro_relatorio.sql_texto"; } }
      public override string OriginalCol { get { return "sql_texto"; } }
      public override string RefCol { get { return "HD_PARAMETRO_RELATORIO.SQL_TEXTO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 200; } }
      public override string SqlServerType { get { return "VARCHAR(200)"; } }

    }
    #endregion

    #region Coluna: COL_VALOR
    private ColumnCol_valor pvColCol_valor;

    public sealed class ColumnCol_valor : TDataColumn {

      public ColumnCol_valor() {
        this.ColumnName = "col_valor";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_parametro_relatorio.col_valor"; } }
      public override string OriginalCol { get { return "col_valor"; } }
      public override string RefCol { get { return "HD_PARAMETRO_RELATORIO.COL_VALOR"; } }
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

    #region Coluna: COL_DESCR
    private ColumnCol_descr pvColCol_descr;

    public sealed class ColumnCol_descr : TDataColumn {

      public ColumnCol_descr() {
        this.ColumnName = "col_descr";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_parametro_relatorio.col_descr"; } }
      public override string OriginalCol { get { return "col_descr"; } }
      public override string RefCol { get { return "HD_PARAMETRO_RELATORIO.COL_DESCR"; } }
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

    #region Coluna: NOMES_COLUNAS
    private ColumnNomes_colunas pvColNomes_colunas;

    public sealed class ColumnNomes_colunas : TDataColumn {

      public ColumnNomes_colunas() {
        this.ColumnName = "nomes_colunas";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_parametro_relatorio.nomes_colunas"; } }
      public override string OriginalCol { get { return "nomes_colunas"; } }
      public override string RefCol { get { return "HD_PARAMETRO_RELATORIO.NOMES_COLUNAS"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 100; } }
      public override string SqlServerType { get { return "VARCHAR(100)"; } }

    }
    #endregion
    #endregion
  }

  public partial class Hd_relatorio : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_relatorio() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_relatorio");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_relatorio");
      }
      catch{}
    }

    public Hd_relatorio() {
      pvColSis = new ColumnSis(); Columns.Add(pvColSis);
      pvColGruporelat = new ColumnGruporelat(); Columns.Add(pvColGruporelat);
      pvColRelatorio = new ColumnRelatorio(); Columns.Add(pvColRelatorio);
      pvColDescricao = new ColumnDescricao(); Columns.Add(pvColDescricao);
      pvColArquivo = new ColumnArquivo(); Columns.Add(pvColArquivo);
      pvColAuditar = new ColumnAuditar(); Columns.Add(pvColAuditar);
      pvColFuncao = new ColumnFuncao(); Columns.Add(pvColFuncao);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_relatorio");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @sis, string @gruporelat, string @relatorio, string @auditar) {
      Row row = NewRow();

      row.Sis = @sis;
      row.Gruporelat = @gruporelat;
      row.Relatorio = @relatorio;
      row.Auditar = @auditar;

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
      get { return "HD_RELATORIO"; }
    }

    protected override sealed string Name { get { return "Relatórios"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColSis, pvColGruporelat, pvColRelatorio });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_relatorio_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_relatorio_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_relatorio_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_relatorio_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkSis", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "SIS", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkGruporelat", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "GRUPORELAT", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkRelatorio", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "RELATORIO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("sis", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SIS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("gruporelat", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "GRUPORELAT", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("relatorio", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "RELATORIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("descricao", OleDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "DESCRICAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("arquivo", OleDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "ARQUIVO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("auditar", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "AUDITAR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("funcao", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "FUNCAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "sis") { result.Add(toLower ? "sis" : column); continue; }
          if(lower == "gruporelat") { result.Add(toLower ? "gruporelat" : column); continue; }
          if(lower == "relatorio") { result.Add(toLower ? "relatorio" : column); continue; }
          if(lower == "descricao") { result.Add(toLower ? "descricao" : column); continue; }
          if(lower == "arquivo") { result.Add(toLower ? "arquivo" : column); continue; }
          if(lower == "auditar") { result.Add(toLower ? "auditar" : column); continue; }
          if(lower == "funcao") { result.Add(toLower ? "funcao" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "sis", "gruporelat", "relatorio", "descricao", "arquivo", "auditar", 
                                           "funcao" }
                          : new string[] { "SIS", "GRUPORELAT", "RELATORIO", "DESCRICAO", "ARQUIVO", "AUDITAR", 
                                           "FUNCAO" });
        else
          result.AddRange(toLower
                          ? new string[] { "sis", "gruporelat", "relatorio", "descricao", "arquivo", "auditar", 
                                           "funcao" }
                          : new string[] { "SIS", "GRUPORELAT", "RELATORIO", "DESCRICAO", "ARQUIVO", "AUDITAR", 
                                           "FUNCAO" });

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
        "hd_relatorio.sis", "hd_relatorio.gruporelat", "hd_relatorio.relatorio"
      });
      if(scColumns.Contains("descricao")) list.Add("hd_relatorio.descricao");
      if(scColumns.Contains("arquivo")) list.Add("hd_relatorio.arquivo");
      if(scColumns.Contains("auditar")) list.Add("hd_relatorio.auditar");
      if(scColumns.Contains("funcao")) list.Add("hd_relatorio.funcao");
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
          "FROM hd_relatorio " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_relatorio");
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
        da.TableMappings.Add("Table", "Hd_relatorio");
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
            if (dr["SIS"] != null && string.IsNullOrEmpty(dr["SIS"].ToString()))
              dr["SIS"] = DBNull.Value;
            if (dr["GRUPORELAT"] != null && string.IsNullOrEmpty(dr["GRUPORELAT"].ToString()))
              dr["GRUPORELAT"] = DBNull.Value;
            if (dr["RELATORIO"] != null && string.IsNullOrEmpty(dr["RELATORIO"].ToString()))
              dr["RELATORIO"] = DBNull.Value;
            if (dr["DESCRICAO"] != null && string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
              dr["DESCRICAO"] = DBNull.Value;
            if (dr["ARQUIVO"] != null && string.IsNullOrEmpty(dr["ARQUIVO"].ToString()))
              dr["ARQUIVO"] = DBNull.Value;
            if (dr["AUDITAR"] != null && string.IsNullOrEmpty(dr["AUDITAR"].ToString()))
              dr["AUDITAR"] = DBNull.Value;
            if (dr["FUNCAO"] != null && string.IsNullOrEmpty(dr["FUNCAO"].ToString()))
              dr["FUNCAO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["SIS"] == null || string.IsNullOrEmpty(dr["SIS"].ToString()))
            {
              return "Favor informar o Sis.";
            }

            if (dr["SIS"] != null && !string.IsNullOrEmpty(dr["SIS"].ToString()))
            {
              if (dr["SIS"].ToString().Length > 12)
              {
                return "O Sis deve possuir no máximo 12 caracteres.";
              }
            }

            if (dr["GRUPORELAT"] == null || string.IsNullOrEmpty(dr["GRUPORELAT"].ToString()))
            {
              return "Favor informar o Gruporelat.";
            }

            if (dr["GRUPORELAT"] != null && !string.IsNullOrEmpty(dr["GRUPORELAT"].ToString()))
            {
              if (dr["GRUPORELAT"].ToString().Length > 50)
              {
                return "O Gruporelat deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["RELATORIO"] == null || string.IsNullOrEmpty(dr["RELATORIO"].ToString()))
            {
              return "Favor informar o Relatorio.";
            }

            if (dr["RELATORIO"] != null && !string.IsNullOrEmpty(dr["RELATORIO"].ToString()))
            {
              if (dr["RELATORIO"].ToString().Length > 50)
              {
                return "O Relatorio deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["DESCRICAO"] != null && !string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              if (dr["DESCRICAO"].ToString().Length > 200)
              {
                return "O Descricao deve possuir no máximo 200 caracteres.";
              }
            }

            if (dr["ARQUIVO"] != null && !string.IsNullOrEmpty(dr["ARQUIVO"].ToString()))
            {
              if (dr["ARQUIVO"].ToString().Length > 200)
              {
                return "O Arquivo deve possuir no máximo 200 caracteres.";
              }
            }

            if (dr["AUDITAR"] == null || string.IsNullOrEmpty(dr["AUDITAR"].ToString()))
            {
              return "Favor informar o Auditar.";
            }

            if (dr["AUDITAR"] != null && !string.IsNullOrEmpty(dr["AUDITAR"].ToString()))
            {
              if (dr["AUDITAR"].ToString().Length > 1)
              {
                return "O Auditar deve possuir no máximo 1 caractere.";
              }

              if (dr["AUDITAR"] != null && !string.IsNullOrEmpty(dr["AUDITAR"].ToString()))
              {
                List<object> lstAUDITAR = new List<object>(new object[]{"N", "S"});
                if(!lstAUDITAR.Contains(dr["AUDITAR"].ToString()))
                {
                  return "Os valores permitidos para o Auditar são ''N'' ou ''S''.";
                }
              }
            }

            if (dr["FUNCAO"] != null && !string.IsNullOrEmpty(dr["FUNCAO"].ToString()))
            {
              if (dr["FUNCAO"].ToString().Length > 50)
              {
                return "O Funcao deve possuir no máximo 50 caracteres.";
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

    public static Hd_relatorio Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_relatorio Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_relatorio tab = new Hd_relatorio();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_relatorio tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @sis, string @gruporelat, string @relatorio, string @descricao, string @arquivo, 
        string @auditar, string @funcao
      ) {
        return (Row)Add(new object[] {
          @sis, @gruporelat, @relatorio, @descricao, @arquivo, @auditar, @funcao
        });
      }

      public Row Find(
        string @sis, string @gruporelat, string @relatorio
      ) {
        return (Row)Find(new object[] {
          @sis, @gruporelat, @relatorio
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
      /// SIS: VARCHAR(12), Primary Key, Not Null
      /// </summary>
      public string Sis { get { return (string)this["sis"]; } set { this["sis"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// GRUPORELAT: VARCHAR(50), Primary Key, Not Null
      /// </summary>
      public string Gruporelat { get { return (string)this["gruporelat"]; } set { this["gruporelat"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// RELATORIO: VARCHAR(50), Primary Key, Not Null
      /// </summary>
      public string Relatorio { get { return (string)this["relatorio"]; } set { this["relatorio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO: VARCHAR(200)
      /// </summary>
      public string Descricao { get { return (string)this["descricao"]; } set { this["descricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ARQUIVO: VARCHAR(200)
      /// </summary>
      public string Arquivo { get { return (string)this["arquivo"]; } set { this["arquivo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// AUDITAR: CHAR(1), Not Null
      /// </summary>
      public string Auditar { get { return (string)this["auditar"]; } set { this["auditar"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// FUNCAO: VARCHAR(50)
      /// </summary>
      public string Funcao { get { return (string)this["funcao"]; } set { this["funcao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @sis, string @gruporelat, string @relatorio, string @auditar) {
        return Insert(connection,
                      @sis, @gruporelat, @relatorio, @auditar,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @sis, string @gruporelat, string @relatorio, string @auditar,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_relatorio(),
          new string[] { "sis", "gruporelat", "relatorio", "auditar" },
          new object[] { @sis, @gruporelat, @relatorio, @auditar },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkSis, string pkGruporelat, string pkRelatorio,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_relatorio(),
          new string[] { "sis", "gruporelat", "relatorio" },
          new object[] { pkSis, pkGruporelat, pkRelatorio },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkSis, string pkGruporelat, string pkRelatorio) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_relatorio(),
          new string[] { "sis", "gruporelat", "relatorio" },
          new object[] { pkSis, pkGruporelat, pkRelatorio }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkSis, string pkGruporelat, string pkRelatorio) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_relatorio(),
          new string[] { "sis", "gruporelat", "relatorio" },
          new object[] { pkSis, pkGruporelat, pkRelatorio }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Sis { get { return (string)row["sis", DataRowVersion.Original]; } }
        public string Gruporelat { get { return (string)row["gruporelat", DataRowVersion.Original]; } }
        public string Relatorio { get { return (string)row["relatorio", DataRowVersion.Original]; } }
        public string Descricao { get { return (string)row["descricao", DataRowVersion.Original]; } }
        public string Arquivo { get { return (string)row["arquivo", DataRowVersion.Original]; } }
        public string Auditar { get { return (string)row["auditar", DataRowVersion.Original]; } }
        public string Funcao { get { return (string)row["funcao", DataRowVersion.Original]; } }

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
    #region Coluna: SIS
    private ColumnSis pvColSis;

    public sealed class ColumnSis : TDataColumn {

      public ColumnSis() {
        this.ColumnName = "sis";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_relatorio.sis"; } }
      public override string OriginalCol { get { return "sis"; } }
      public override string RefCol { get { return "HD_RELATORIO.SIS"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 12; } }
      public override string SqlServerType { get { return "VARCHAR(12)"; } }

    }
    #endregion

    #region Coluna: GRUPORELAT
    private ColumnGruporelat pvColGruporelat;

    public sealed class ColumnGruporelat : TDataColumn {

      public ColumnGruporelat() {
        this.ColumnName = "gruporelat";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_relatorio.gruporelat"; } }
      public override string OriginalCol { get { return "gruporelat"; } }
      public override string RefCol { get { return "HD_RELATORIO.GRUPORELAT"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 50; } }
      public override string SqlServerType { get { return "VARCHAR(50)"; } }

    }
    #endregion

    #region Coluna: RELATORIO
    private ColumnRelatorio pvColRelatorio;

    public sealed class ColumnRelatorio : TDataColumn {

      public ColumnRelatorio() {
        this.ColumnName = "relatorio";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_relatorio.relatorio"; } }
      public override string OriginalCol { get { return "relatorio"; } }
      public override string RefCol { get { return "HD_RELATORIO.RELATORIO"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 50; } }
      public override string SqlServerType { get { return "VARCHAR(50)"; } }

    }
    #endregion

    #region Coluna: DESCRICAO
    private ColumnDescricao pvColDescricao;

    public sealed class ColumnDescricao : TDataColumn {

      public ColumnDescricao() {
        this.ColumnName = "descricao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_relatorio.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "HD_RELATORIO.DESCRICAO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 200; } }
      public override string SqlServerType { get { return "VARCHAR(200)"; } }

    }
    #endregion

    #region Coluna: ARQUIVO
    private ColumnArquivo pvColArquivo;

    public sealed class ColumnArquivo : TDataColumn {

      public ColumnArquivo() {
        this.ColumnName = "arquivo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_relatorio.arquivo"; } }
      public override string OriginalCol { get { return "arquivo"; } }
      public override string RefCol { get { return "HD_RELATORIO.ARQUIVO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 200; } }
      public override string SqlServerType { get { return "VARCHAR(200)"; } }

    }
    #endregion

    #region Coluna: AUDITAR
    private ColumnAuditar pvColAuditar;

    public sealed class ColumnAuditar : TDataColumn {

      public ColumnAuditar() {
        this.ColumnName = "auditar";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_relatorio.auditar"; } }
      public override string OriginalCol { get { return "auditar"; } }
      public override string RefCol { get { return "HD_RELATORIO.AUDITAR"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 1; } }
      public override string SqlServerType { get { return "CHAR(1)"; } }

    }
    #endregion

    #region Coluna: FUNCAO
    private ColumnFuncao pvColFuncao;

    public sealed class ColumnFuncao : TDataColumn {

      public ColumnFuncao() {
        this.ColumnName = "funcao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_relatorio.funcao"; } }
      public override string OriginalCol { get { return "funcao"; } }
      public override string RefCol { get { return "HD_RELATORIO.FUNCAO"; } }
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
}

