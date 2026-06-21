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
  public class Numeracao : TDataSet {
    private const string DataSetName_Def = "Numeracao";
    private static ConstructorInfo ctorTCustom = null;
    private CustomBase tcustom = null;
    #region Variáveis private para as tabelas deste dataset
    private Hd_numeracao pvTabHd_numeracao;
    #endregion

    static Numeracao() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Numeracao");
      }
      catch{}
    }

    public Numeracao() : base() {
      DataSetName = DataSetName_Def;

      pvTabHd_numeracao = new Hd_numeracao();

      Tables.Add(pvTabHd_numeracao);

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
          pvTabHd_numeracao.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_numeracao); if(row != null) return DataLib.RowErrorsToString(row);
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

    #endregion

    public abstract class CustomBase {
      public abstract string Put(Techne.HadesLyc.CR.Numeracao ds, TConnectionWritable connection);
    }

  }


  public partial class Hd_numeracao : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_numeracao() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_numeracao");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_numeracao");
      }
      catch{}
    }

    public Hd_numeracao() {
      pvColTipo = new ColumnTipo(); Columns.Add(pvColTipo);
      pvColBase = new ColumnBase(); Columns.Add(pvColBase);
      pvColLastnum = new ColumnLastnum(); Columns.Add(pvColLastnum);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_numeracao");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @tipo, string @base, decimal? @lastnum) {
      Row row = NewRow();

      row.Tipo = @tipo;
      row.Base = @base;
      row.Lastnum = @lastnum;

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
      get { return "HD_NUMERACAO"; }
    }

    protected override sealed string Name { get { return "Numeração"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColTipo, pvColBase });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_numeracao_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_numeracao_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_numeracao_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_numeracao_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkTipo", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "TIPO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkBase", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "BASE", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("tipo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TIPO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("base", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "BASE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("lastnum", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 22, 10, "LASTNUM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "tipo") { result.Add(toLower ? "tipo" : column); continue; }
          if(lower == "base") { result.Add(toLower ? "base" : column); continue; }
          if(lower == "lastnum") { result.Add(toLower ? "lastnum" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "tipo", "base", "lastnum" }
                          : new string[] { "TIPO", "BASE", "LASTNUM" });
        else
          result.AddRange(toLower
                          ? new string[] { "tipo", "base", "lastnum" }
                          : new string[] { "TIPO", "BASE", "LASTNUM" });

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
        "hd_numeracao.tipo", "hd_numeracao.base"
      });
      if(scColumns.Contains("lastnum")) list.Add("hd_numeracao.lastnum");
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
          "FROM hd_numeracao " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_numeracao");
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
        da.TableMappings.Add("Table", "Hd_numeracao");
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
            if (dr["TIPO"] != null && string.IsNullOrEmpty(dr["TIPO"].ToString()))
              dr["TIPO"] = DBNull.Value;
            if (dr["BASE"] != null && string.IsNullOrEmpty(dr["BASE"].ToString()))
              dr["BASE"] = DBNull.Value;
            if (dr["LASTNUM"] != null && string.IsNullOrEmpty(dr["LASTNUM"].ToString()))
              dr["LASTNUM"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["TIPO"] == null || string.IsNullOrEmpty(dr["TIPO"].ToString()))
            {
              return "Favor informar o Tipo.";
            }

            if (dr["TIPO"] != null && !string.IsNullOrEmpty(dr["TIPO"].ToString()))
            {
              if (dr["TIPO"].ToString().Length > 20)
              {
                return "O Tipo deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["BASE"] == null || string.IsNullOrEmpty(dr["BASE"].ToString()))
            {
              return "Favor informar o Base.";
            }

            if (dr["BASE"] != null && !string.IsNullOrEmpty(dr["BASE"].ToString()))
            {
              if (dr["BASE"].ToString().Length > 28)
              {
                return "O Base deve possuir no máximo 28 caracteres.";
              }
            }

            if (dr["LASTNUM"] == null || string.IsNullOrEmpty(dr["LASTNUM"].ToString()))
            {
              return "Favor informar o Lastnum.";
            }

            if (dr["LASTNUM"] != null && !string.IsNullOrEmpty(dr["LASTNUM"].ToString()))
            {
              if ((Convert.ToDecimal(dr["LASTNUM"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["LASTNUM"]).ToString("0.############").Split(',')[0].Length > 12) || (Convert.ToDecimal(dr["LASTNUM"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["LASTNUM"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Lastnum deve ser um valor inteiro com 12 dígitos no máximo.";
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

    public static Hd_numeracao Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_numeracao Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_numeracao tab = new Hd_numeracao();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_numeracao tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @tipo, string @base, decimal? @lastnum
      ) {
        return (Row)Add(new object[] {
          @tipo, @base, @lastnum
        });
      }

      public Row Find(
        string @tipo, string @base
      ) {
        return (Row)Find(new object[] {
          @tipo, @base
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
      /// TIPO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Tipo { get { return (string)this["tipo"]; } set { this["tipo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// BASE: VARCHAR(28), Primary Key, Not Null
      /// </summary>
      public string Base { get { return (string)this["base"]; } set { this["base"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// LASTNUM: NUMERIC(12), Not Null
      /// </summary>
      public decimal? Lastnum { get { return (decimal?)this["lastnum"]; } set { this["lastnum"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @tipo, string @base, decimal? @lastnum) {
        return Insert(connection,
                      @tipo, @base, @lastnum,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @tipo, string @base, decimal? @lastnum,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_numeracao(),
          new string[] { "tipo", "base", "lastnum" },
          new object[] { @tipo, @base, @lastnum },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkTipo, string pkBase,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_numeracao(),
          new string[] { "tipo", "base" },
          new object[] { pkTipo, pkBase },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkTipo, string pkBase) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_numeracao(),
          new string[] { "tipo", "base" },
          new object[] { pkTipo, pkBase }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkTipo, string pkBase) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_numeracao(),
          new string[] { "tipo", "base" },
          new object[] { pkTipo, pkBase }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Tipo { get { return (string)row["tipo", DataRowVersion.Original]; } }
        public string Base { get { return (string)row["base", DataRowVersion.Original]; } }
        public decimal? Lastnum { get { return (decimal?)row["lastnum", DataRowVersion.Original]; } }

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
    #region Coluna: TIPO
    private ColumnTipo pvColTipo;

    public sealed class ColumnTipo : TDataColumn {

      public ColumnTipo() {
        this.ColumnName = "tipo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_numeracao.tipo"; } }
      public override string OriginalCol { get { return "tipo"; } }
      public override string RefCol { get { return "HD_NUMERACAO.TIPO"; } }
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

    #region Coluna: BASE
    private ColumnBase pvColBase;

    public sealed class ColumnBase : TDataColumn {

      public ColumnBase() {
        this.ColumnName = "base";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_numeracao.base"; } }
      public override string OriginalCol { get { return "base"; } }
      public override string RefCol { get { return "HD_NUMERACAO.BASE"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 28; } }
      public override string SqlServerType { get { return "VARCHAR(28)"; } }

    }
    #endregion

    #region Coluna: LASTNUM
    private ColumnLastnum pvColLastnum;

    public sealed class ColumnLastnum : TDataColumn {

      public ColumnLastnum() {
        this.ColumnName = "lastnum";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_numeracao.lastnum"; } }
      public override string OriginalCol { get { return "lastnum"; } }
      public override string RefCol { get { return "HD_NUMERACAO.LASTNUM"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 13; } }
      public override int Precision { get { return 12; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(12)"; } }

    }
    #endregion
    #endregion
  }
}

