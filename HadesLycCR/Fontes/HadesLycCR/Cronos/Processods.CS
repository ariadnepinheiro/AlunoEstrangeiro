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
  public class ProcessoDS : TDataSet {
    private const string DataSetName_Def = "ProcessoDS";
    private static ConstructorInfo ctorTCustom = null;
    private CustomBase tcustom = null;
    #region Variáveis private para as tabelas deste dataset
    private Hd_grupo_processos pvTabHd_grupo_processos;
    private Hd_parametro_processo pvTabHd_parametro_processo;
    private Hd_processo pvTabHd_processo;
    #endregion

    static ProcessoDS() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("ProcessoDS");
      }
      catch{}
    }

    public ProcessoDS() : base() {
      DataSetName = DataSetName_Def;

      pvTabHd_grupo_processos = new Hd_grupo_processos();
      pvTabHd_parametro_processo = new Hd_parametro_processo();
      pvTabHd_processo = new Hd_processo();

      Tables.Add(pvTabHd_grupo_processos);
      Tables.Add(pvTabHd_parametro_processo);
      Tables.Add(pvTabHd_processo);

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
          pvTabHd_grupo_processos.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_grupo_processos); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabHd_parametro_processo.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_parametro_processo); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabHd_processo.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_processo); if(row != null) return DataLib.RowErrorsToString(row);
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
    public Hd_grupo_processos Hd_grupo_processos { get { return pvTabHd_grupo_processos; } }

    [
      Browsable(false),
    ]
    public Hd_parametro_processo Hd_parametro_processo { get { return pvTabHd_parametro_processo; } }

    [
      Browsable(false),
    ]
    public Hd_processo Hd_processo { get { return pvTabHd_processo; } }

    #endregion

    public abstract class CustomBase {
      public abstract string Put(Techne.HadesLyc.CR.ProcessoDS ds, TConnectionWritable connection);
    }

  }


  public partial class Hd_grupo_processos : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_grupo_processos() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_grupo_processos");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_grupo_processos");
      }
      catch{}
    }

    public Hd_grupo_processos() {
      pvColSis = new ColumnSis(); Columns.Add(pvColSis);
      pvColGrupoproc = new ColumnGrupoproc(); Columns.Add(pvColGrupoproc);
      pvColDescricao = new ColumnDescricao(); Columns.Add(pvColDescricao);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_grupo_processos");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @sis, string @grupoproc) {
      Row row = NewRow();

      row.Sis = @sis;
      row.Grupoproc = @grupoproc;

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
      get { return "HD_GRUPO_PROCESSOS"; }
    }

    protected override sealed string Name { get { return "Grupos de processos"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColSis, pvColGrupoproc });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_grupo_processos_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_grupo_processos_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_grupo_processos_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_grupo_processos_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkSis", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "SIS", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkGrupoproc", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "GRUPOPROC", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("sis", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SIS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("grupoproc", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "GRUPOPROC", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("descricao", OleDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "DESCRICAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "grupoproc") { result.Add(toLower ? "grupoproc" : column); continue; }
          if(lower == "descricao") { result.Add(toLower ? "descricao" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "sis", "grupoproc", "descricao" }
                          : new string[] { "SIS", "GRUPOPROC", "DESCRICAO" });
        else
          result.AddRange(toLower
                          ? new string[] { "sis", "grupoproc", "descricao" }
                          : new string[] { "SIS", "GRUPOPROC", "DESCRICAO" });

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
        "hd_grupo_processos.sis", "hd_grupo_processos.grupoproc"
      });
      if(scColumns.Contains("descricao")) list.Add("hd_grupo_processos.descricao");
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
          "FROM hd_grupo_processos " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_grupo_processos");
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
        da.TableMappings.Add("Table", "Hd_grupo_processos");
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
            if (dr["GRUPOPROC"] != null && string.IsNullOrEmpty(dr["GRUPOPROC"].ToString()))
              dr["GRUPOPROC"] = DBNull.Value;
            if (dr["DESCRICAO"] != null && string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
              dr["DESCRICAO"] = DBNull.Value;
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

            if (dr["GRUPOPROC"] == null || string.IsNullOrEmpty(dr["GRUPOPROC"].ToString()))
            {
              return "Favor informar o Grupoproc.";
            }

            if (dr["GRUPOPROC"] != null && !string.IsNullOrEmpty(dr["GRUPOPROC"].ToString()))
            {
              if (dr["GRUPOPROC"].ToString().Length > 50)
              {
                return "O Grupoproc deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["DESCRICAO"] != null && !string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              if (dr["DESCRICAO"].ToString().Length > 200)
              {
                return "O Descricao deve possuir no máximo 200 caracteres.";
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

    public static Hd_grupo_processos Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_grupo_processos Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_grupo_processos tab = new Hd_grupo_processos();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_grupo_processos tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @sis, string @grupoproc, string @descricao
      ) {
        return (Row)Add(new object[] {
          @sis, @grupoproc, @descricao
        });
      }

      public Row Find(
        string @sis, string @grupoproc
      ) {
        return (Row)Find(new object[] {
          @sis, @grupoproc
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
      /// GRUPOPROC: VARCHAR(50), Primary Key, Not Null
      /// </summary>
      public string Grupoproc { get { return (string)this["grupoproc"]; } set { this["grupoproc"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO: VARCHAR(200)
      /// </summary>
      public string Descricao { get { return (string)this["descricao"]; } set { this["descricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @sis, string @grupoproc) {
        return Insert(connection,
                      @sis, @grupoproc,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @sis, string @grupoproc,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_grupo_processos(),
          new string[] { "sis", "grupoproc" },
          new object[] { @sis, @grupoproc },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkSis, string pkGrupoproc,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_grupo_processos(),
          new string[] { "sis", "grupoproc" },
          new object[] { pkSis, pkGrupoproc },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkSis, string pkGrupoproc) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_grupo_processos(),
          new string[] { "sis", "grupoproc" },
          new object[] { pkSis, pkGrupoproc }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkSis, string pkGrupoproc) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_grupo_processos(),
          new string[] { "sis", "grupoproc" },
          new object[] { pkSis, pkGrupoproc }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Sis { get { return (string)row["sis", DataRowVersion.Original]; } }
        public string Grupoproc { get { return (string)row["grupoproc", DataRowVersion.Original]; } }
        public string Descricao { get { return (string)row["descricao", DataRowVersion.Original]; } }

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
      public override string FullCol { get { return "hd_grupo_processos.sis"; } }
      public override string OriginalCol { get { return "sis"; } }
      public override string RefCol { get { return "HD_GRUPO_PROCESSOS.SIS"; } }
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

    #region Coluna: GRUPOPROC
    private ColumnGrupoproc pvColGrupoproc;

    public sealed class ColumnGrupoproc : TDataColumn {

      public ColumnGrupoproc() {
        this.ColumnName = "grupoproc";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_grupo_processos.grupoproc"; } }
      public override string OriginalCol { get { return "grupoproc"; } }
      public override string RefCol { get { return "HD_GRUPO_PROCESSOS.GRUPOPROC"; } }
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
      public override string FullCol { get { return "hd_grupo_processos.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "HD_GRUPO_PROCESSOS.DESCRICAO"; } }
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

  public partial class Hd_parametro_processo : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_parametro_processo() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_parametro_processo");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_parametro_processo");
      }
      catch{}
    }

    public Hd_parametro_processo() {
      pvColSis = new ColumnSis(); Columns.Add(pvColSis);
      pvColProcesso = new ColumnProcesso(); Columns.Add(pvColProcesso);
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
      SetTableName("hd_parametro_processo");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @sis, string @processo, string @parametro, decimal? @ordem, 
                      decimal? @tipo, string @obrigatorio) {
      Row row = NewRow();

      row.Sis = @sis;
      row.Processo = @processo;
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
      get { return "HD_PARAMETRO_PROCESSO"; }
    }

    protected override sealed string Name { get { return "Parâmetros dos processos"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColSis, pvColProcesso, pvColParametro });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_parametro_processo_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_parametro_processo_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_parametro_processo_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_parametro_processo_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkSis", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "SIS", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkProcesso", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "PROCESSO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkParametro", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "PARAMETRO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("sis", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SIS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("processo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PROCESSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("parametro", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PARAMETRO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("ordem", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ORDEM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("tipo", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "TIPO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("descricao", OleDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "DESCRICAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("obrigatorio", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "OBRIGATORIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("valor", OleDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "VALOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("sql_texto", OleDbType.VarChar, 2100, ParameterDirection.InputOutput, true, 0, 0, "SQL_TEXTO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
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
          if(lower == "processo") { result.Add(toLower ? "processo" : column); continue; }
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
                          ? new string[] { "sis", "processo", "parametro", "ordem", "tipo", "descricao", "obrigatorio", 
                                           "valor", "sql_texto", "col_valor", "col_descr", "nomes_colunas" }
                          : new string[] { "SIS", "PROCESSO", "PARAMETRO", "ORDEM", "TIPO", "DESCRICAO", "OBRIGATORIO", 
                                           "VALOR", "SQL_TEXTO", "COL_VALOR", "COL_DESCR", "NOMES_COLUNAS" });
        else
          result.AddRange(toLower
                          ? new string[] { "sis", "processo", "parametro", "ordem", "tipo", "descricao", "obrigatorio", 
                                           "valor", "sql_texto", "col_valor", "col_descr", "nomes_colunas" }
                          : new string[] { "SIS", "PROCESSO", "PARAMETRO", "ORDEM", "TIPO", "DESCRICAO", "OBRIGATORIO", 
                                           "VALOR", "SQL_TEXTO", "COL_VALOR", "COL_DESCR", "NOMES_COLUNAS" });

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
        "hd_parametro_processo.sis", "hd_parametro_processo.processo", "hd_parametro_processo.parametro"
      });
      if(scColumns.Contains("ordem")) list.Add("hd_parametro_processo.ordem");
      if(scColumns.Contains("tipo")) list.Add("hd_parametro_processo.tipo");
      if(scColumns.Contains("descricao")) list.Add("hd_parametro_processo.descricao");
      if(scColumns.Contains("obrigatorio")) list.Add("hd_parametro_processo.obrigatorio");
      if(scColumns.Contains("valor")) list.Add("hd_parametro_processo.valor");
      if(scColumns.Contains("sql_texto")) list.Add("hd_parametro_processo.sql_texto");
      if(scColumns.Contains("col_valor")) list.Add("hd_parametro_processo.col_valor");
      if(scColumns.Contains("col_descr")) list.Add("hd_parametro_processo.col_descr");
      if(scColumns.Contains("nomes_colunas")) list.Add("hd_parametro_processo.nomes_colunas");
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
          "FROM hd_parametro_processo " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_parametro_processo");
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
        da.TableMappings.Add("Table", "Hd_parametro_processo");
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
            if (dr["PROCESSO"] != null && string.IsNullOrEmpty(dr["PROCESSO"].ToString()))
              dr["PROCESSO"] = DBNull.Value;
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

            if (dr["PROCESSO"] == null || string.IsNullOrEmpty(dr["PROCESSO"].ToString()))
            {
              return "Favor informar o Processo.";
            }

            if (dr["PROCESSO"] != null && !string.IsNullOrEmpty(dr["PROCESSO"].ToString()))
            {
              if (dr["PROCESSO"].ToString().Length > 50)
              {
                return "O Processo deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["PARAMETRO"] == null || string.IsNullOrEmpty(dr["PARAMETRO"].ToString()))
            {
              return "Favor informar o Parametro.";
            }

            if (dr["PARAMETRO"] != null && !string.IsNullOrEmpty(dr["PARAMETRO"].ToString()))
            {
              if (dr["PARAMETRO"].ToString().Length > 20)
              {
                return "O Parametro deve possuir no máximo 20 caracteres.";
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
              if (dr["SQL_TEXTO"].ToString().Length > 2000)
              {
                return "O Sql_texto deve possuir no máximo 2000 caracteres.";
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

    public static Hd_parametro_processo Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_parametro_processo Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_parametro_processo tab = new Hd_parametro_processo();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_parametro_processo tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @sis, string @processo, string @parametro, decimal? @ordem, decimal? @tipo, 
        string @descricao, string @obrigatorio, string @valor, string @sql_texto, string @col_valor, 
        string @col_descr, string @nomes_colunas
      ) {
        return (Row)Add(new object[] {
          @sis, @processo, @parametro, @ordem, @tipo, @descricao, @obrigatorio, @valor, @sql_texto, 
          @col_valor, @col_descr, @nomes_colunas
        });
      }

      public Row Find(
        string @sis, string @processo, string @parametro
      ) {
        return (Row)Find(new object[] {
          @sis, @processo, @parametro
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
      /// PROCESSO: VARCHAR(50), Primary Key, Not Null
      /// </summary>
      public string Processo { get { return (string)this["processo"]; } set { this["processo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PARAMETRO: VARCHAR(20), Primary Key, Not Null
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
      /// SQL_TEXTO: VARCHAR(2000)
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
                               string @sis, string @processo, string @parametro, decimal? @ordem, decimal? @tipo, 
                               string @obrigatorio) {
        return Insert(connection,
                      @sis, @processo, @parametro, @ordem, @tipo, @obrigatorio,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @sis, string @processo, string @parametro, decimal? @ordem, decimal? @tipo, 
                               string @obrigatorio,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_parametro_processo(),
          new string[] { "sis", "processo", "parametro", "ordem", "tipo", "obrigatorio" },
          new object[] { @sis, @processo, @parametro, @ordem, @tipo, @obrigatorio },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkSis, string pkProcesso, string pkParametro,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_parametro_processo(),
          new string[] { "sis", "processo", "parametro" },
          new object[] { pkSis, pkProcesso, pkParametro },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkSis, string pkProcesso, string pkParametro) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_parametro_processo(),
          new string[] { "sis", "processo", "parametro" },
          new object[] { pkSis, pkProcesso, pkParametro }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkSis, string pkProcesso, string pkParametro) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_parametro_processo(),
          new string[] { "sis", "processo", "parametro" },
          new object[] { pkSis, pkProcesso, pkParametro }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Sis { get { return (string)row["sis", DataRowVersion.Original]; } }
        public string Processo { get { return (string)row["processo", DataRowVersion.Original]; } }
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
      public override string FullCol { get { return "hd_parametro_processo.sis"; } }
      public override string OriginalCol { get { return "sis"; } }
      public override string RefCol { get { return "HD_PARAMETRO_PROCESSO.SIS"; } }
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

    #region Coluna: PROCESSO
    private ColumnProcesso pvColProcesso;

    public sealed class ColumnProcesso : TDataColumn {

      public ColumnProcesso() {
        this.ColumnName = "processo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_parametro_processo.processo"; } }
      public override string OriginalCol { get { return "processo"; } }
      public override string RefCol { get { return "HD_PARAMETRO_PROCESSO.PROCESSO"; } }
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
      public override string FullCol { get { return "hd_parametro_processo.parametro"; } }
      public override string OriginalCol { get { return "parametro"; } }
      public override string RefCol { get { return "HD_PARAMETRO_PROCESSO.PARAMETRO"; } }
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
      public override string FullCol { get { return "hd_parametro_processo.ordem"; } }
      public override string OriginalCol { get { return "ordem"; } }
      public override string RefCol { get { return "HD_PARAMETRO_PROCESSO.ORDEM"; } }
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
      public override string FullCol { get { return "hd_parametro_processo.tipo"; } }
      public override string OriginalCol { get { return "tipo"; } }
      public override string RefCol { get { return "HD_PARAMETRO_PROCESSO.TIPO"; } }
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
      public override string FullCol { get { return "hd_parametro_processo.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "HD_PARAMETRO_PROCESSO.DESCRICAO"; } }
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
      public override string FullCol { get { return "hd_parametro_processo.obrigatorio"; } }
      public override string OriginalCol { get { return "obrigatorio"; } }
      public override string RefCol { get { return "HD_PARAMETRO_PROCESSO.OBRIGATORIO"; } }
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
      public override string FullCol { get { return "hd_parametro_processo.valor"; } }
      public override string OriginalCol { get { return "valor"; } }
      public override string RefCol { get { return "HD_PARAMETRO_PROCESSO.VALOR"; } }
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
      public override string FullCol { get { return "hd_parametro_processo.sql_texto"; } }
      public override string OriginalCol { get { return "sql_texto"; } }
      public override string RefCol { get { return "HD_PARAMETRO_PROCESSO.SQL_TEXTO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 2000; } }
      public override string SqlServerType { get { return "VARCHAR(2000)"; } }

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
      public override string FullCol { get { return "hd_parametro_processo.col_valor"; } }
      public override string OriginalCol { get { return "col_valor"; } }
      public override string RefCol { get { return "HD_PARAMETRO_PROCESSO.COL_VALOR"; } }
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
      public override string FullCol { get { return "hd_parametro_processo.col_descr"; } }
      public override string OriginalCol { get { return "col_descr"; } }
      public override string RefCol { get { return "HD_PARAMETRO_PROCESSO.COL_DESCR"; } }
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
      public override string FullCol { get { return "hd_parametro_processo.nomes_colunas"; } }
      public override string OriginalCol { get { return "nomes_colunas"; } }
      public override string RefCol { get { return "HD_PARAMETRO_PROCESSO.NOMES_COLUNAS"; } }
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

  public partial class Hd_processo : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_processo() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_processo");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_processo");
      }
      catch{}
    }

    public Hd_processo() {
      pvColSis = new ColumnSis(); Columns.Add(pvColSis);
      pvColProcesso = new ColumnProcesso(); Columns.Add(pvColProcesso);
      pvColGrupoproc = new ColumnGrupoproc(); Columns.Add(pvColGrupoproc);
      pvColDescricao = new ColumnDescricao(); Columns.Add(pvColDescricao);
      pvColArquivo = new ColumnArquivo(); Columns.Add(pvColArquivo);
      pvColAuditar = new ColumnAuditar(); Columns.Add(pvColAuditar);
      pvColReprocessavel = new ColumnReprocessavel(); Columns.Add(pvColReprocessavel);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_processo");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @sis, string @processo, string @auditar, string @reprocessavel) {
      Row row = NewRow();

      row.Sis = @sis;
      row.Processo = @processo;
      row.Auditar = @auditar;
      row.Reprocessavel = @reprocessavel;

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
      get { return "HD_PROCESSO"; }
    }

    protected override sealed string Name { get { return "Processos"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColSis, pvColProcesso });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_processo_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_processo_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_processo_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_processo_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkSis", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "SIS", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkProcesso", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "PROCESSO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("sis", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SIS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("processo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PROCESSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("grupoproc", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "GRUPOPROC", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("descricao", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "DESCRICAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("arquivo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "ARQUIVO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("auditar", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "AUDITAR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("reprocessavel", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "REPROCESSAVEL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));

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
          if(lower == "processo") { result.Add(toLower ? "processo" : column); continue; }
          if(lower == "grupoproc") { result.Add(toLower ? "grupoproc" : column); continue; }
          if(lower == "descricao") { result.Add(toLower ? "descricao" : column); continue; }
          if(lower == "arquivo") { result.Add(toLower ? "arquivo" : column); continue; }
          if(lower == "auditar") { result.Add(toLower ? "auditar" : column); continue; }
          if(lower == "reprocessavel") { result.Add(toLower ? "reprocessavel" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "sis", "processo", "grupoproc", "descricao", "arquivo", "auditar", 
                                           "reprocessavel" }
                          : new string[] { "SIS", "PROCESSO", "GRUPOPROC", "DESCRICAO", "ARQUIVO", "AUDITAR", 
                                           "REPROCESSAVEL" });
        else
          result.AddRange(toLower
                          ? new string[] { "sis", "processo", "grupoproc", "descricao", "arquivo", "auditar", 
                                           "reprocessavel" }
                          : new string[] { "SIS", "PROCESSO", "GRUPOPROC", "DESCRICAO", "ARQUIVO", "AUDITAR", 
                                           "REPROCESSAVEL" });

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
        "hd_processo.sis", "hd_processo.processo"
      });
      if(scColumns.Contains("grupoproc")) list.Add("hd_processo.grupoproc");
      if(scColumns.Contains("descricao")) list.Add("hd_processo.descricao");
      if(scColumns.Contains("arquivo")) list.Add("hd_processo.arquivo");
      if(scColumns.Contains("auditar")) list.Add("hd_processo.auditar");
      if(scColumns.Contains("reprocessavel")) list.Add("hd_processo.reprocessavel");
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
          "FROM hd_processo " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_processo");
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
        da.TableMappings.Add("Table", "Hd_processo");
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
            if (dr["PROCESSO"] != null && string.IsNullOrEmpty(dr["PROCESSO"].ToString()))
              dr["PROCESSO"] = DBNull.Value;
            if (dr["GRUPOPROC"] != null && string.IsNullOrEmpty(dr["GRUPOPROC"].ToString()))
              dr["GRUPOPROC"] = DBNull.Value;
            if (dr["DESCRICAO"] != null && string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
              dr["DESCRICAO"] = DBNull.Value;
            if (dr["ARQUIVO"] != null && string.IsNullOrEmpty(dr["ARQUIVO"].ToString()))
              dr["ARQUIVO"] = DBNull.Value;
            if (dr["AUDITAR"] != null && string.IsNullOrEmpty(dr["AUDITAR"].ToString()))
              dr["AUDITAR"] = DBNull.Value;
            if (dr["REPROCESSAVEL"] != null && string.IsNullOrEmpty(dr["REPROCESSAVEL"].ToString()))
              dr["REPROCESSAVEL"] = DBNull.Value;
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

            if (dr["PROCESSO"] == null || string.IsNullOrEmpty(dr["PROCESSO"].ToString()))
            {
              return "Favor informar o Processo.";
            }

            if (dr["PROCESSO"] != null && !string.IsNullOrEmpty(dr["PROCESSO"].ToString()))
            {
              if (dr["PROCESSO"].ToString().Length > 50)
              {
                return "O Processo deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["GRUPOPROC"] != null && !string.IsNullOrEmpty(dr["GRUPOPROC"].ToString()))
            {
              if (dr["GRUPOPROC"].ToString().Length > 50)
              {
                return "O Grupoproc deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["DESCRICAO"] != null && !string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              if (dr["DESCRICAO"].ToString().Length > 100)
              {
                return "O Descricao deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["ARQUIVO"] != null && !string.IsNullOrEmpty(dr["ARQUIVO"].ToString()))
            {
              if (dr["ARQUIVO"].ToString().Length > 100)
              {
                return "O Arquivo deve possuir no máximo 100 caracteres.";
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

            if (dr["REPROCESSAVEL"] == null || string.IsNullOrEmpty(dr["REPROCESSAVEL"].ToString()))
            {
              return "Favor informar o Reprocessavel.";
            }

            if (dr["REPROCESSAVEL"] != null && !string.IsNullOrEmpty(dr["REPROCESSAVEL"].ToString()))
            {
              if (dr["REPROCESSAVEL"].ToString().Length > 1)
              {
                return "O Reprocessavel deve possuir no máximo 1 caractere.";
              }

              if (dr["REPROCESSAVEL"] != null && !string.IsNullOrEmpty(dr["REPROCESSAVEL"].ToString()))
              {
                List<object> lstREPROCESSAVEL = new List<object>(new object[]{"N", "S"});
                if(!lstREPROCESSAVEL.Contains(dr["REPROCESSAVEL"].ToString()))
                {
                  return "Os valores permitidos para o Reprocessavel são ''N'' ou ''S''.";
                }
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

    public static Hd_processo Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_processo Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_processo tab = new Hd_processo();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_processo tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @sis, string @processo, string @grupoproc, string @descricao, string @arquivo, 
        string @auditar, string @reprocessavel
      ) {
        return (Row)Add(new object[] {
          @sis, @processo, @grupoproc, @descricao, @arquivo, @auditar, @reprocessavel
        });
      }

      public Row Find(
        string @sis, string @processo
      ) {
        return (Row)Find(new object[] {
          @sis, @processo
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
      /// PROCESSO: VARCHAR(50), Primary Key, Not Null
      /// </summary>
      public string Processo { get { return (string)this["processo"]; } set { this["processo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// GRUPOPROC: VARCHAR(50)
      /// </summary>
      public string Grupoproc { get { return (string)this["grupoproc"]; } set { this["grupoproc"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO: VARCHAR(100)
      /// </summary>
      public string Descricao { get { return (string)this["descricao"]; } set { this["descricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ARQUIVO: VARCHAR(100)
      /// </summary>
      public string Arquivo { get { return (string)this["arquivo"]; } set { this["arquivo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// AUDITAR: CHAR(1), Not Null
      /// </summary>
      public string Auditar { get { return (string)this["auditar"]; } set { this["auditar"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// REPROCESSAVEL: CHAR(1), Not Null
      /// </summary>
      public string Reprocessavel { get { return (string)this["reprocessavel"]; } set { this["reprocessavel"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @sis, string @processo, string @auditar, string @reprocessavel) {
        return Insert(connection,
                      @sis, @processo, @auditar, @reprocessavel,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @sis, string @processo, string @auditar, string @reprocessavel,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_processo(),
          new string[] { "sis", "processo", "auditar", "reprocessavel" },
          new object[] { @sis, @processo, @auditar, @reprocessavel },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkSis, string pkProcesso,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_processo(),
          new string[] { "sis", "processo" },
          new object[] { pkSis, pkProcesso },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkSis, string pkProcesso) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_processo(),
          new string[] { "sis", "processo" },
          new object[] { pkSis, pkProcesso }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkSis, string pkProcesso) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_processo(),
          new string[] { "sis", "processo" },
          new object[] { pkSis, pkProcesso }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Sis { get { return (string)row["sis", DataRowVersion.Original]; } }
        public string Processo { get { return (string)row["processo", DataRowVersion.Original]; } }
        public string Grupoproc { get { return (string)row["grupoproc", DataRowVersion.Original]; } }
        public string Descricao { get { return (string)row["descricao", DataRowVersion.Original]; } }
        public string Arquivo { get { return (string)row["arquivo", DataRowVersion.Original]; } }
        public string Auditar { get { return (string)row["auditar", DataRowVersion.Original]; } }
        public string Reprocessavel { get { return (string)row["reprocessavel", DataRowVersion.Original]; } }

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
      public override string FullCol { get { return "hd_processo.sis"; } }
      public override string OriginalCol { get { return "sis"; } }
      public override string RefCol { get { return "HD_PROCESSO.SIS"; } }
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

    #region Coluna: PROCESSO
    private ColumnProcesso pvColProcesso;

    public sealed class ColumnProcesso : TDataColumn {

      public ColumnProcesso() {
        this.ColumnName = "processo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_processo.processo"; } }
      public override string OriginalCol { get { return "processo"; } }
      public override string RefCol { get { return "HD_PROCESSO.PROCESSO"; } }
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

    #region Coluna: GRUPOPROC
    private ColumnGrupoproc pvColGrupoproc;

    public sealed class ColumnGrupoproc : TDataColumn {

      public ColumnGrupoproc() {
        this.ColumnName = "grupoproc";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_processo.grupoproc"; } }
      public override string OriginalCol { get { return "grupoproc"; } }
      public override string RefCol { get { return "HD_PROCESSO.GRUPOPROC"; } }
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
      public override string FullCol { get { return "hd_processo.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "HD_PROCESSO.DESCRICAO"; } }
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
      public override string FullCol { get { return "hd_processo.arquivo"; } }
      public override string OriginalCol { get { return "arquivo"; } }
      public override string RefCol { get { return "HD_PROCESSO.ARQUIVO"; } }
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
      public override string FullCol { get { return "hd_processo.auditar"; } }
      public override string OriginalCol { get { return "auditar"; } }
      public override string RefCol { get { return "HD_PROCESSO.AUDITAR"; } }
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

    #region Coluna: REPROCESSAVEL
    private ColumnReprocessavel pvColReprocessavel;

    public sealed class ColumnReprocessavel : TDataColumn {

      public ColumnReprocessavel() {
        this.ColumnName = "reprocessavel";
        this.DataType = typeof(string);
        this.DefaultValue = "S";

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_processo.reprocessavel"; } }
      public override string OriginalCol { get { return "reprocessavel"; } }
      public override string RefCol { get { return "HD_PROCESSO.REPROCESSAVEL"; } }
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
    #endregion
  }
}

