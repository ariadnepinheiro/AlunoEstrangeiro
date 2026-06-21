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
  public class Sistema : TDataSet {
    private const string DataSetName_Def = "Sistema";
    private static ConstructorInfo ctorTCustom = null;
    private CustomBase tcustom = null;
    #region Variáveis private para as tabelas deste dataset
    private Hd_modulo pvTabHd_modulo;
    private Hd_padtrans pvTabHd_padtrans;
    private Hd_sistema pvTabHd_sistema;
    private Hd_transacao pvTabHd_transacao;
    #endregion

    static Sistema() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Sistema");
      }
      catch{}
    }

    public Sistema() : base() {
      DataSetName = DataSetName_Def;

      pvTabHd_modulo = new Hd_modulo();
      pvTabHd_padtrans = new Hd_padtrans();
      pvTabHd_sistema = new Hd_sistema();
      pvTabHd_transacao = new Hd_transacao();

      Tables.Add(pvTabHd_modulo);
      Tables.Add(pvTabHd_padtrans);
      Tables.Add(pvTabHd_sistema);
      Tables.Add(pvTabHd_transacao);

      CreateRelations();

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
    }


    [DefaultValue(DataSetName_Def)]
    public new string DataSetName {
      get { return base.DataSetName; }
      set { base.DataSetName = value == null ? string.Empty : value; }
    }

    private void CreateRelations() {
      Relations.Add("fk_modulo_sistema",
                    new DataColumn[] { pvTabHd_sistema.Columns["sis"] },
                    new DataColumn[] { pvTabHd_modulo.Columns["sis"] }, false);
      Relations.Add("fk_transacao_modulo",
                    new DataColumn[] { pvTabHd_modulo.Columns["sis"], pvTabHd_modulo.Columns["modulo"] },
                    new DataColumn[] { pvTabHd_transacao.Columns["sis"], pvTabHd_transacao.Columns["modulo"] }, false);
      Relations.Add("fk_transacao_sistema",
                    new DataColumn[] { pvTabHd_sistema.Columns["sis"] },
                    new DataColumn[] { pvTabHd_transacao.Columns["sis"] }, false);
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
          pvTabHd_modulo.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_modulo); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabHd_padtrans.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_padtrans); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabHd_sistema.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_sistema); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabHd_transacao.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_transacao); if(row != null) return DataLib.RowErrorsToString(row);
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
    public Hd_modulo Hd_modulo { get { return pvTabHd_modulo; } }

    [
      Browsable(false),
    ]
    public Hd_padtrans Hd_padtrans { get { return pvTabHd_padtrans; } }

    [
      Browsable(false),
    ]
    public Hd_sistema Hd_sistema { get { return pvTabHd_sistema; } }

    [
      Browsable(false),
    ]
    public Hd_transacao Hd_transacao { get { return pvTabHd_transacao; } }

    #endregion

    public abstract class CustomBase {
      public abstract string Put(Techne.HadesLyc.CR.Sistema ds, TConnectionWritable connection);
    }

  }


  public partial class Hd_modulo : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_modulo() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_modulo");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_modulo");
      }
      catch{}
    }

    public Hd_modulo() {
      pvColSis = new ColumnSis(); Columns.Add(pvColSis);
      pvColModulo = new ColumnModulo(); Columns.Add(pvColModulo);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_modulo");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @sis, string @modulo) {
      Row row = NewRow();

      row.Sis = @sis;
      row.Modulo = @modulo;

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
      get { return "HD_MODULO"; }
    }

    protected override sealed string Name { get { return "Módulos"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColSis, pvColModulo });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_modulo_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_modulo_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_modulo_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_modulo_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkSis", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "SIS", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkModulo", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "MODULO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("sis", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SIS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("modulo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "MODULO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "modulo") { result.Add(toLower ? "modulo" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "sis", "modulo" }
                          : new string[] { "SIS", "MODULO" });
        else
          result.AddRange(toLower
                          ? new string[] { "sis", "modulo" }
                          : new string[] { "SIS", "MODULO" });

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
        "hd_modulo.sis", "hd_modulo.modulo"
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
          "FROM hd_modulo " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_modulo");
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
        da.TableMappings.Add("Table", "Hd_modulo");
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
            if (dr["MODULO"] != null && string.IsNullOrEmpty(dr["MODULO"].ToString()))
              dr["MODULO"] = DBNull.Value;
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

            if (dr["MODULO"] == null || string.IsNullOrEmpty(dr["MODULO"].ToString()))
            {
              return "Favor informar o Modulo.";
            }

            if (dr["MODULO"] != null && !string.IsNullOrEmpty(dr["MODULO"].ToString()))
            {
              if (dr["MODULO"].ToString().Length > 30)
              {
                return "O Modulo deve possuir no máximo 30 caracteres.";
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

    public static Hd_modulo Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_modulo Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_modulo tab = new Hd_modulo();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_modulo tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @sis, string @modulo
      ) {
        return (Row)Add(new object[] {
          @sis, @modulo
        });
      }

      public Row Find(
        string @sis, string @modulo
      ) {
        return (Row)Find(new object[] {
          @sis, @modulo
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
      /// MODULO: VARCHAR(30), Primary Key, Not Null
      /// </summary>
      public string Modulo { get { return (string)this["modulo"]; } set { this["modulo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @sis, string @modulo) {
        return Insert(connection,
                      @sis, @modulo,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @sis, string @modulo,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_modulo(),
          new string[] { "sis", "modulo" },
          new object[] { @sis, @modulo },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkSis, string pkModulo,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_modulo(),
          new string[] { "sis", "modulo" },
          new object[] { pkSis, pkModulo },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkSis, string pkModulo) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_modulo(),
          new string[] { "sis", "modulo" },
          new object[] { pkSis, pkModulo }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkSis, string pkModulo) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_modulo(),
          new string[] { "sis", "modulo" },
          new object[] { pkSis, pkModulo }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Sis { get { return (string)row["sis", DataRowVersion.Original]; } }
        public string Modulo { get { return (string)row["modulo", DataRowVersion.Original]; } }

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
      public override string FullCol { get { return "hd_modulo.sis"; } }
      public override string OriginalCol { get { return "sis"; } }
      public override string RefCol { get { return "HD_MODULO.SIS"; } }
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

    #region Coluna: MODULO
    private ColumnModulo pvColModulo;

    public sealed class ColumnModulo : TDataColumn {

      public ColumnModulo() {
        this.ColumnName = "modulo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_modulo.modulo"; } }
      public override string OriginalCol { get { return "modulo"; } }
      public override string RefCol { get { return "HD_MODULO.MODULO"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 30; } }
      public override string SqlServerType { get { return "VARCHAR(30)"; } }

    }
    #endregion
    #endregion
  }

  public partial class Hd_padtrans : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_padtrans() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_padtrans");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_padtrans");
      }
      catch{}
    }

    public Hd_padtrans() {
      pvColPadaces = new ColumnPadaces(); Columns.Add(pvColPadaces);
      pvColSis = new ColumnSis(); Columns.Add(pvColSis);
      pvColTrans = new ColumnTrans(); Columns.Add(pvColTrans);
      pvColPodealt = new ColumnPodealt(); Columns.Add(pvColPodealt);
      pvColPodecad = new ColumnPodecad(); Columns.Add(pvColPodecad);
      pvColPoderem = new ColumnPoderem(); Columns.Add(pvColPoderem);
      pvColItemmenu = new ColumnItemmenu(); Columns.Add(pvColItemmenu);
      pvColTrans02 = new ColumnTrans02(); Columns.Add(pvColTrans02);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_padtrans");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @padaces, string @sis, string @trans) {
      Row row = NewRow();

      row.Padaces = @padaces;
      row.Sis = @sis;
      row.Trans = @trans;

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
      get { return "HD_PADTRANS"; }
    }

    protected override sealed string Name { get { return "Padrão de acesso e suas transações"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColPadaces, pvColSis, pvColTrans });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_padtrans_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_padtrans_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_padtrans_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_padtrans_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkPadaces", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "PADACES", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkSis", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "SIS", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkTrans", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "TRANS", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("padaces", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PADACES", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("sis", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SIS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("trans", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TRANS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("podealt", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PODEALT", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("podecad", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PODECAD", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("poderem", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PODEREM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));

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
          if(lower == "trans") { result.Add(toLower ? "trans" : column); continue; }
          if(lower == "podealt") { result.Add(toLower ? "podealt" : column); continue; }
          if(lower == "podecad") { result.Add(toLower ? "podecad" : column); continue; }
          if(lower == "poderem") { result.Add(toLower ? "poderem" : column); continue; }
          if(!tabPrincipal) {
            if(lower == "itemmenu") { result.Add(toLower ? "itemmenu" : column); continue; }
            if(lower == "trans02") { result.Add(toLower ? "trans02" : column); continue; }
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "padaces", "sis", "trans", "podealt", "podecad", "poderem" }
                          : new string[] { "PADACES", "SIS", "TRANS", "PODEALT", "PODECAD", "PODEREM" });
        else
          result.AddRange(toLower
                          ? new string[] { "padaces", "sis", "trans", "podealt", "podecad", "poderem", "itemmenu", 
                                           "trans02" }
                          : new string[] { "PADACES", "SIS", "TRANS", "PODEALT", "PODECAD", "PODEREM", "ITEMMENU", 
                                           "TRANS02" });

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
        "hd_padtrans.padaces", "hd_padtrans.sis", "hd_padtrans.trans"
      });
      if(scColumns.Contains("podealt")) list.Add("hd_padtrans.podealt");
      if(scColumns.Contains("podecad")) list.Add("hd_padtrans.podecad");
      if(scColumns.Contains("poderem")) list.Add("hd_padtrans.poderem");
      if(scColumns.Contains("itemmenu")) list.Add("hd_transacao.itemmenu");
      if(scColumns.Contains("trans02")) list.Add("hd_transacao.trans trans02");
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
          "FROM hd_padtrans " +
               "INNER JOIN hd_transacao " +
                 "ON hd_transacao.sis = hd_padtrans.sis AND " +
                    "hd_transacao.trans = hd_padtrans.trans " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_padtrans");
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
        da.TableMappings.Add("Table", "Hd_padtrans");
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
            if (dr["TRANS"] != null && string.IsNullOrEmpty(dr["TRANS"].ToString()))
              dr["TRANS"] = DBNull.Value;
            if (dr["PODEALT"] != null && string.IsNullOrEmpty(dr["PODEALT"].ToString()))
              dr["PODEALT"] = DBNull.Value;
            if (dr["PODECAD"] != null && string.IsNullOrEmpty(dr["PODECAD"].ToString()))
              dr["PODECAD"] = DBNull.Value;
            if (dr["PODEREM"] != null && string.IsNullOrEmpty(dr["PODEREM"].ToString()))
              dr["PODEREM"] = DBNull.Value;
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

            if (dr["TRANS"] == null || string.IsNullOrEmpty(dr["TRANS"].ToString()))
            {
              return "Favor informar a transação.";
            }

            if (dr["TRANS"] != null && !string.IsNullOrEmpty(dr["TRANS"].ToString()))
            {
              if (dr["TRANS"].ToString().Length > 100)
              {
                return "A transação deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["PODEALT"] != null && !string.IsNullOrEmpty(dr["PODEALT"].ToString()))
            {
              if (dr["PODEALT"].ToString().Length > 1)
              {
                return "O pode alterar? deve possuir no máximo 1 caractere.";
              }

              if (dr["PODEALT"] != null && !string.IsNullOrEmpty(dr["PODEALT"].ToString()))
              {
                List<object> lstPODEALT = new List<object>(new object[]{"N", "S"});
                if(!lstPODEALT.Contains(dr["PODEALT"].ToString()))
                {
                  return "Os valores permitidos para o pode alterar? são ''N'' ou ''S''.";
                }
              }
            }

            if (dr["PODECAD"] != null && !string.IsNullOrEmpty(dr["PODECAD"].ToString()))
            {
              if (dr["PODECAD"].ToString().Length > 1)
              {
                return "O pode cadastrar? deve possuir no máximo 1 caractere.";
              }

              if (dr["PODECAD"] != null && !string.IsNullOrEmpty(dr["PODECAD"].ToString()))
              {
                List<object> lstPODECAD = new List<object>(new object[]{"N", "S"});
                if(!lstPODECAD.Contains(dr["PODECAD"].ToString()))
                {
                  return "Os valores permitidos para o pode cadastrar? são ''N'' ou ''S''.";
                }
              }
            }

            if (dr["PODEREM"] != null && !string.IsNullOrEmpty(dr["PODEREM"].ToString()))
            {
              if (dr["PODEREM"].ToString().Length > 1)
              {
                return "O pode remover? deve possuir no máximo 1 caractere.";
              }

              if (dr["PODEREM"] != null && !string.IsNullOrEmpty(dr["PODEREM"].ToString()))
              {
                List<object> lstPODEREM = new List<object>(new object[]{"N", "S"});
                if(!lstPODEREM.Contains(dr["PODEREM"].ToString()))
                {
                  return "Os valores permitidos para o pode remover? são ''N'' ou ''S''.";
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

    public static Hd_padtrans Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_padtrans Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_padtrans tab = new Hd_padtrans();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_padtrans tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @padaces, string @sis, string @trans, string @podealt, string @podecad, string @poderem
      ) {
        return (Row)Add(new object[] {
          @padaces, @sis, @trans, @podealt, @podecad, @poderem
        });
      }

      public Row Find(
        string @padaces, string @sis, string @trans
      ) {
        return (Row)Find(new object[] {
          @padaces, @sis, @trans
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
      /// TRANS: VARCHAR(100), Primary Key, Not Null
      /// </summary>
      public string Trans { get { return (string)this["trans"]; } set { this["trans"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PODEALT: CHAR(1)
      /// </summary>
      public string Podealt { get { return (string)this["podealt"]; } set { this["podealt"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PODECAD: CHAR(1)
      /// </summary>
      public string Podecad { get { return (string)this["podecad"]; } set { this["podecad"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PODEREM: CHAR(1)
      /// </summary>
      public string Poderem { get { return (string)this["poderem"]; } set { this["poderem"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ITEMMENU: VARCHAR(100)
      /// </summary>
      public string Itemmenu { get { return (string)this["itemmenu"]; } set { this["itemmenu"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TRANS: VARCHAR(100), Primary Key, Not Null
      /// </summary>
      public string Trans02 { get { return (string)this["trans02"]; } set { this["trans02"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @padaces, string @sis, string @trans) {
        return Insert(connection,
                      @padaces, @sis, @trans,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @padaces, string @sis, string @trans,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_padtrans(),
          new string[] { "padaces", "sis", "trans" },
          new object[] { @padaces, @sis, @trans },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkPadaces, string pkSis, string pkTrans,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_padtrans(),
          new string[] { "padaces", "sis", "trans" },
          new object[] { pkPadaces, pkSis, pkTrans },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkPadaces, string pkSis, string pkTrans) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_padtrans(),
          new string[] { "padaces", "sis", "trans" },
          new object[] { pkPadaces, pkSis, pkTrans }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkPadaces, string pkSis, string pkTrans) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_padtrans(),
          new string[] { "padaces", "sis", "trans" },
          new object[] { pkPadaces, pkSis, pkTrans }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Padaces { get { return (string)row["padaces", DataRowVersion.Original]; } }
        public string Sis { get { return (string)row["sis", DataRowVersion.Original]; } }
        public string Trans { get { return (string)row["trans", DataRowVersion.Original]; } }
        public string Podealt { get { return (string)row["podealt", DataRowVersion.Original]; } }
        public string Podecad { get { return (string)row["podecad", DataRowVersion.Original]; } }
        public string Poderem { get { return (string)row["poderem", DataRowVersion.Original]; } }
        public string Itemmenu { get { return (string)row["itemmenu", DataRowVersion.Original]; } }
        public string Trans02 { get { return (string)row["trans02", DataRowVersion.Original]; } }

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
      public override string FullCol { get { return "hd_padtrans.padaces"; } }
      public override string OriginalCol { get { return "padaces"; } }
      public override string RefCol { get { return "HD_PADTRANS.PADACES"; } }
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
      public override string FullCol { get { return "hd_padtrans.sis"; } }
      public override string OriginalCol { get { return "sis"; } }
      public override string RefCol { get { return "HD_PADTRANS.SIS"; } }
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

    #region Coluna: TRANS
    private ColumnTrans pvColTrans;

    public sealed class ColumnTrans : TDataColumn {

      public ColumnTrans() {
        this.ColumnName = "trans";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "transação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_padtrans.trans"; } }
      public override string OriginalCol { get { return "trans"; } }
      public override string RefCol { get { return "HD_PADTRANS.TRANS"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 100; } }
      public override string SqlServerType { get { return "VARCHAR(100)"; } }

    }
    #endregion

    #region Coluna: PODEALT
    private ColumnPodealt pvColPodealt;

    public sealed class ColumnPodealt : TDataColumn {

      public ColumnPodealt() {
        this.ColumnName = "podealt";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "pode alterar?";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_padtrans.podealt"; } }
      public override string OriginalCol { get { return "podealt"; } }
      public override string RefCol { get { return "HD_PADTRANS.PODEALT"; } }
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

    #region Coluna: PODECAD
    private ColumnPodecad pvColPodecad;

    public sealed class ColumnPodecad : TDataColumn {

      public ColumnPodecad() {
        this.ColumnName = "podecad";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "pode cadastrar?";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_padtrans.podecad"; } }
      public override string OriginalCol { get { return "podecad"; } }
      public override string RefCol { get { return "HD_PADTRANS.PODECAD"; } }
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

    #region Coluna: PODEREM
    private ColumnPoderem pvColPoderem;

    public sealed class ColumnPoderem : TDataColumn {

      public ColumnPoderem() {
        this.ColumnName = "poderem";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "pode remover?";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_padtrans.poderem"; } }
      public override string OriginalCol { get { return "poderem"; } }
      public override string RefCol { get { return "HD_PADTRANS.PODEREM"; } }
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

    #region Coluna: ITEMMENU
    private ColumnItemmenu pvColItemmenu;

    public sealed class ColumnItemmenu : TDataColumn {

      public ColumnItemmenu() {
        this.ColumnName = "itemmenu";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "item do menu";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_transacao.itemmenu"; } }
      public override string OriginalCol { get { return "itemmenu"; } }
      public override string RefCol { get { return "HD_TRANSACAO.ITEMMENU"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 100; } }
      public override string SqlServerType { get { return "VARCHAR(100)"; } }

    }
    #endregion

    #region Coluna: TRANS02
    private ColumnTrans02 pvColTrans02;

    public sealed class ColumnTrans02 : TDataColumn {

      public ColumnTrans02() {
        this.ColumnName = "trans02";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "transação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_transacao.trans"; } }
      public override string OriginalCol { get { return "trans"; } }
      public override string RefCol { get { return "HD_TRANSACAO.TRANS"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 100; } }
      public override string SqlServerType { get { return "VARCHAR(100)"; } }

    }
    #endregion
    #endregion
  }

  public partial class Hd_sistema : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_sistema() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_sistema");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_sistema");
      }
      catch{}
    }

    public Hd_sistema() {
      pvColSis = new ColumnSis(); Columns.Add(pvColSis);
      pvColNome = new ColumnNome(); Columns.Add(pvColNome);
      pvColVersao = new ColumnVersao(); Columns.Add(pvColVersao);
      pvColTake = new ColumnTake(); Columns.Add(pvColTake);
      pvColHabilitado = new ColumnHabilitado(); Columns.Add(pvColHabilitado);
      pvColUrl = new ColumnUrl(); Columns.Add(pvColUrl);
      pvColP = new ColumnP(); Columns.Add(pvColP);
      pvColU = new ColumnU(); Columns.Add(pvColU);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_sistema");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @sis, string @nome) {
      Row row = NewRow();

      row.Sis = @sis;
      row.Nome = @nome;

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
      get { return "HD_SISTEMA"; }
    }

    protected override sealed string Name { get { return "Sistemas"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColSis });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_sistema_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_sistema_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_sistema_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_sistema_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkSis", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "SIS", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("sis", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SIS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("nome", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NOME", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("versao", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "VERSAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("take", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "TAKE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("habilitado", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "HABILITADO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("url", OleDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "URL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("p", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "P", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("u", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "U", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "nome") { result.Add(toLower ? "nome" : column); continue; }
          if(lower == "versao") { result.Add(toLower ? "versao" : column); continue; }
          if(lower == "take") { result.Add(toLower ? "take" : column); continue; }
          if(lower == "habilitado") { result.Add(toLower ? "habilitado" : column); continue; }
          if(lower == "url") { result.Add(toLower ? "url" : column); continue; }
          if(lower == "p") { result.Add(toLower ? "p" : column); continue; }
          if(lower == "u") { result.Add(toLower ? "u" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "sis", "nome", "versao", "take", "habilitado", "url", "p", "u" }
                          : new string[] { "SIS", "NOME", "VERSAO", "TAKE", "HABILITADO", "URL", "P", "U" });
        else
          result.AddRange(toLower
                          ? new string[] { "sis", "nome", "versao", "take", "habilitado", "url", "p", "u" }
                          : new string[] { "SIS", "NOME", "VERSAO", "TAKE", "HABILITADO", "URL", "P", "U" });

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
        "hd_sistema.sis"
      });
      if(scColumns.Contains("nome")) list.Add("hd_sistema.nome");
      if(scColumns.Contains("versao")) list.Add("hd_sistema.versao");
      if(scColumns.Contains("take")) list.Add("hd_sistema.take");
      if(scColumns.Contains("habilitado")) list.Add("hd_sistema.habilitado");
      if(scColumns.Contains("url")) list.Add("hd_sistema.url");
      if(scColumns.Contains("p")) list.Add("hd_sistema.p");
      if(scColumns.Contains("u")) list.Add("hd_sistema.u");
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
          "FROM hd_sistema " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_sistema");
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
        da.TableMappings.Add("Table", "Hd_sistema");
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
            if (dr["NOME"] != null && string.IsNullOrEmpty(dr["NOME"].ToString()))
              dr["NOME"] = DBNull.Value;
            if (dr["VERSAO"] != null && string.IsNullOrEmpty(dr["VERSAO"].ToString()))
              dr["VERSAO"] = DBNull.Value;
            if (dr["HABILITADO"] != null && string.IsNullOrEmpty(dr["HABILITADO"].ToString()))
              dr["HABILITADO"] = DBNull.Value;
            if (dr["URL"] != null && string.IsNullOrEmpty(dr["URL"].ToString()))
              dr["URL"] = DBNull.Value;
            if (dr["P"] != null && string.IsNullOrEmpty(dr["P"].ToString()))
              dr["P"] = DBNull.Value;
            if (dr["U"] != null && string.IsNullOrEmpty(dr["U"].ToString()))
              dr["U"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

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

            if (dr["NOME"] == null || string.IsNullOrEmpty(dr["NOME"].ToString()))
            {
              return "Favor informar o nome do sistema.";
            }

            if (dr["NOME"] != null && !string.IsNullOrEmpty(dr["NOME"].ToString()))
            {
              if (dr["NOME"].ToString().Length > 50)
              {
                return "O nome do sistema deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["VERSAO"] != null && !string.IsNullOrEmpty(dr["VERSAO"].ToString()))
            {
              if (dr["VERSAO"].ToString().Length > 11)
              {
                return "A versão deve possuir no máximo 11 caracteres.";
              }
            }

            if (dr["TAKE"] != null && dr["TAKE"].ToString() != "")
            {
              dr["TAKE"] = Convert.ToDateTime(dr["TAKE"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["HABILITADO"] != null && !string.IsNullOrEmpty(dr["HABILITADO"].ToString()))
            {
              if (dr["HABILITADO"].ToString().Length > 1)
              {
                return "O habilitado deve possuir no máximo 1 caractere.";
              }

              if (dr["HABILITADO"] != null && !string.IsNullOrEmpty(dr["HABILITADO"].ToString()))
              {
                List<object> lstHABILITADO = new List<object>(new object[]{"N", "S"});
                if(!lstHABILITADO.Contains(dr["HABILITADO"].ToString()))
                {
                  return "Os valores permitidos para o habilitado são ''N'' ou ''S''.";
                }
              }
            }

            if (dr["URL"] != null && !string.IsNullOrEmpty(dr["URL"].ToString()))
            {
              if (dr["URL"].ToString().Length > 200)
              {
                return "O Url deve possuir no máximo 200 caracteres.";
              }
            }

            if (dr["P"] != null && !string.IsNullOrEmpty(dr["P"].ToString()))
            {
              if (dr["P"].ToString().Length > 30)
              {
                return "O P deve possuir no máximo 30 caracteres.";
              }
            }

            if (dr["U"] != null && !string.IsNullOrEmpty(dr["U"].ToString()))
            {
              if (dr["U"].ToString().Length > 30)
              {
                return "O U deve possuir no máximo 30 caracteres.";
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

    public static Hd_sistema Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_sistema Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_sistema tab = new Hd_sistema();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_sistema tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @sis, string @nome, string @versao, DateTime? @take, string @habilitado, string @url, 
        string @p, string @u
      ) {
        return (Row)Add(new object[] {
          @sis, @nome, @versao, @take, @habilitado, @url, @p, @u
        });
      }

      public Row Find(
        string @sis
      ) {
        return (Row)Find(new object[] {
          @sis
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
      /// NOME: VARCHAR(50), Not Null
      /// </summary>
      public string Nome { get { return (string)this["nome"]; } set { this["nome"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// VERSAO: VARCHAR(11)
      /// </summary>
      public string Versao { get { return (string)this["versao"]; } set { this["versao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TAKE: DATETIME
      /// </summary>
      public DateTime? Take { get { return (DateTime?)this["take"]; } set { this["take"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// HABILITADO: CHAR(1)
      /// </summary>
      public string Habilitado { get { return (string)this["habilitado"]; } set { this["habilitado"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// URL: VARCHAR(200)
      /// </summary>
      public string Url { get { return (string)this["url"]; } set { this["url"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// P: VARCHAR(30)
      /// </summary>
      public string P { get { return (string)this["p"]; } set { this["p"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// U: VARCHAR(30)
      /// </summary>
      public string U { get { return (string)this["u"]; } set { this["u"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @sis, string @nome) {
        return Insert(connection,
                      @sis, @nome,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @sis, string @nome,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_sistema(),
          new string[] { "sis", "nome" },
          new object[] { @sis, @nome },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkSis,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_sistema(),
          new string[] { "sis" },
          new object[] { pkSis },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkSis) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_sistema(),
          new string[] { "sis" },
          new object[] { pkSis }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkSis) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_sistema(),
          new string[] { "sis" },
          new object[] { pkSis }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Sis { get { return (string)row["sis", DataRowVersion.Original]; } }
        public string Nome { get { return (string)row["nome", DataRowVersion.Original]; } }
        public string Versao { get { return (string)row["versao", DataRowVersion.Original]; } }
        public DateTime? Take { get { return (DateTime?)row["take", DataRowVersion.Original]; } }
        public string Habilitado { get { return (string)row["habilitado", DataRowVersion.Original]; } }
        public string Url { get { return (string)row["url", DataRowVersion.Original]; } }
        public string P { get { return (string)row["p", DataRowVersion.Original]; } }
        public string U { get { return (string)row["u", DataRowVersion.Original]; } }

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


      protected override string GetName(){return "sistema";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_sistema.sis"; } }
      public override string OriginalCol { get { return "sis"; } }
      public override string RefCol { get { return "HD_SISTEMA.SIS"; } }
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

    #region Coluna: NOME
    private ColumnNome pvColNome;

    public sealed class ColumnNome : TDataColumn {

      public ColumnNome() {
        this.ColumnName = "nome";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "nome do sistema";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_sistema.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "HD_SISTEMA.NOME"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 50; } }
      public override string SqlServerType { get { return "VARCHAR(50)"; } }

    }
    #endregion

    #region Coluna: VERSAO
    private ColumnVersao pvColVersao;

    public sealed class ColumnVersao : TDataColumn {

      public ColumnVersao() {
        this.ColumnName = "versao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "versão";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_sistema.versao"; } }
      public override string OriginalCol { get { return "versao"; } }
      public override string RefCol { get { return "HD_SISTEMA.VERSAO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 11; } }
      public override string SqlServerType { get { return "VARCHAR(11)"; } }

    }
    #endregion

    #region Coluna: TAKE
    private ColumnTake pvColTake;

    public sealed class ColumnTake : TDataColumn {

      public ColumnTake() {
        this.ColumnName = "take";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_sistema.take"; } }
      public override string OriginalCol { get { return "take"; } }
      public override string RefCol { get { return "HD_SISTEMA.TAKE"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 10; } }
      public override string SqlServerType { get { return "DATETIME"; } }

    }
    #endregion

    #region Coluna: HABILITADO
    private ColumnHabilitado pvColHabilitado;

    public sealed class ColumnHabilitado : TDataColumn {

      public ColumnHabilitado() {
        this.ColumnName = "habilitado";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "habilitado";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_sistema.habilitado"; } }
      public override string OriginalCol { get { return "habilitado"; } }
      public override string RefCol { get { return "HD_SISTEMA.HABILITADO"; } }
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

    #region Coluna: URL
    private ColumnUrl pvColUrl;

    public sealed class ColumnUrl : TDataColumn {

      public ColumnUrl() {
        this.ColumnName = "url";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_sistema.url"; } }
      public override string OriginalCol { get { return "url"; } }
      public override string RefCol { get { return "HD_SISTEMA.URL"; } }
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

    #region Coluna: P
    private ColumnP pvColP;

    public sealed class ColumnP : TDataColumn {

      public ColumnP() {
        this.ColumnName = "p";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_sistema.p"; } }
      public override string OriginalCol { get { return "p"; } }
      public override string RefCol { get { return "HD_SISTEMA.P"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 30; } }
      public override string SqlServerType { get { return "VARCHAR(30)"; } }

    }
    #endregion

    #region Coluna: U
    private ColumnU pvColU;

    public sealed class ColumnU : TDataColumn {

      public ColumnU() {
        this.ColumnName = "u";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_sistema.u"; } }
      public override string OriginalCol { get { return "u"; } }
      public override string RefCol { get { return "HD_SISTEMA.U"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 30; } }
      public override string SqlServerType { get { return "VARCHAR(30)"; } }

    }
    #endregion
    #endregion
  }

  public partial class Hd_transacao : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_transacao() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_transacao");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_transacao");
      }
      catch{}
    }

    public Hd_transacao() {
      pvColSis = new ColumnSis(); Columns.Add(pvColSis);
      pvColTrans = new ColumnTrans(); Columns.Add(pvColTrans);
      pvColNome = new ColumnNome(); Columns.Add(pvColNome);
      pvColPublica = new ColumnPublica(); Columns.Add(pvColPublica);
      pvColAcessoanonimo = new ColumnAcessoanonimo(); Columns.Add(pvColAcessoanonimo);
      pvColPaginaweb = new ColumnPaginaweb(); Columns.Add(pvColPaginaweb);
      pvColForm = new ColumnForm(); Columns.Add(pvColForm);
      pvColItemmenu = new ColumnItemmenu(); Columns.Add(pvColItemmenu);
      pvColAuditar = new ColumnAuditar(); Columns.Add(pvColAuditar);
      pvColModulo = new ColumnModulo(); Columns.Add(pvColModulo);
      pvColNomesis = new ColumnNomesis(); Columns.Add(pvColNomesis);
      pvColVersao = new ColumnVersao(); Columns.Add(pvColVersao);
      pvColTake = new ColumnTake(); Columns.Add(pvColTake);
      pvColHabilitado = new ColumnHabilitado(); Columns.Add(pvColHabilitado);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_transacao");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @sis, string @trans) {
      Row row = NewRow();

      row.Sis = @sis;
      row.Trans = @trans;

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
      get { return "HD_TRANSACAO"; }
    }

    protected override sealed string Name { get { return "Transações"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColSis, pvColTrans });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_transacao_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_transacao_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_transacao_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_transacao_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkSis", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "SIS", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkTrans", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "TRANS", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("sis", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SIS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("trans", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TRANS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("nome", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NOME", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("publica", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PUBLICA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("acessoanonimo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "ACESSOANONIMO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("paginaweb", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PAGINAWEB", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("form", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "FORM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("itemmenu", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "ITEMMENU", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("auditar", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "AUDITAR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("modulo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "MODULO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "trans") { result.Add(toLower ? "trans" : column); continue; }
          if(lower == "nome") { result.Add(toLower ? "nome" : column); continue; }
          if(lower == "publica") { result.Add(toLower ? "publica" : column); continue; }
          if(lower == "acessoanonimo") { result.Add(toLower ? "acessoanonimo" : column); continue; }
          if(lower == "paginaweb") { result.Add(toLower ? "paginaweb" : column); continue; }
          if(lower == "form") { result.Add(toLower ? "form" : column); continue; }
          if(lower == "itemmenu") { result.Add(toLower ? "itemmenu" : column); continue; }
          if(lower == "auditar") { result.Add(toLower ? "auditar" : column); continue; }
          if(lower == "modulo") { result.Add(toLower ? "modulo" : column); continue; }
          if(!tabPrincipal) {
            if(lower == "nomesis") { result.Add(toLower ? "nomesis" : column); continue; }
            if(lower == "versao") { result.Add(toLower ? "versao" : column); continue; }
            if(lower == "take") { result.Add(toLower ? "take" : column); continue; }
            if(lower == "habilitado") { result.Add(toLower ? "habilitado" : column); continue; }
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "sis", "trans", "nome", "publica", "acessoanonimo", "paginaweb", 
                                           "form", "itemmenu", "auditar", "modulo" }
                          : new string[] { "SIS", "TRANS", "NOME", "PUBLICA", "ACESSOANONIMO", "PAGINAWEB", 
                                           "FORM", "ITEMMENU", "AUDITAR", "MODULO" });
        else
          result.AddRange(toLower
                          ? new string[] { "sis", "trans", "nome", "publica", "acessoanonimo", "paginaweb", 
                                           "form", "itemmenu", "auditar", "modulo", "nomesis", "versao", "take", "habilitado" }
                          : new string[] { "SIS", "TRANS", "NOME", "PUBLICA", "ACESSOANONIMO", "PAGINAWEB", 
                                           "FORM", "ITEMMENU", "AUDITAR", "MODULO", "NOMESIS", "VERSAO", "TAKE", "HABILITADO" });

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
        "hd_transacao.sis", "hd_transacao.trans"
      });
      if(scColumns.Contains("nome")) list.Add("hd_transacao.nome");
      if(scColumns.Contains("publica")) list.Add("hd_transacao.publica");
      if(scColumns.Contains("acessoanonimo")) list.Add("hd_transacao.acessoanonimo");
      if(scColumns.Contains("paginaweb")) list.Add("hd_transacao.paginaweb");
      if(scColumns.Contains("form")) list.Add("hd_transacao.form");
      if(scColumns.Contains("itemmenu")) list.Add("hd_transacao.itemmenu");
      if(scColumns.Contains("auditar")) list.Add("hd_transacao.auditar");
      if(scColumns.Contains("modulo")) list.Add("hd_transacao.modulo");
      if(scColumns.Contains("nomesis")) list.Add("hd_sistema.nome nomesis");
      if(scColumns.Contains("versao")) list.Add("hd_sistema.versao");
      if(scColumns.Contains("take")) list.Add("hd_sistema.take");
      if(scColumns.Contains("habilitado")) list.Add("hd_sistema.habilitado");
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
          "FROM hd_transacao " +
               "INNER JOIN hd_sistema " +
                 "ON hd_sistema.sis = hd_transacao.sis " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_transacao");
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
        da.TableMappings.Add("Table", "Hd_transacao");
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
            if (dr["TRANS"] != null && string.IsNullOrEmpty(dr["TRANS"].ToString()))
              dr["TRANS"] = DBNull.Value;
            if (dr["NOME"] != null && string.IsNullOrEmpty(dr["NOME"].ToString()))
              dr["NOME"] = DBNull.Value;
            if (dr["PUBLICA"] != null && string.IsNullOrEmpty(dr["PUBLICA"].ToString()))
              dr["PUBLICA"] = DBNull.Value;
            if (dr["ACESSOANONIMO"] != null && string.IsNullOrEmpty(dr["ACESSOANONIMO"].ToString()))
              dr["ACESSOANONIMO"] = DBNull.Value;
            if (dr["PAGINAWEB"] != null && string.IsNullOrEmpty(dr["PAGINAWEB"].ToString()))
              dr["PAGINAWEB"] = DBNull.Value;
            if (dr["FORM"] != null && string.IsNullOrEmpty(dr["FORM"].ToString()))
              dr["FORM"] = DBNull.Value;
            if (dr["ITEMMENU"] != null && string.IsNullOrEmpty(dr["ITEMMENU"].ToString()))
              dr["ITEMMENU"] = DBNull.Value;
            if (dr["AUDITAR"] != null && string.IsNullOrEmpty(dr["AUDITAR"].ToString()))
              dr["AUDITAR"] = DBNull.Value;
            if (dr["MODULO"] != null && string.IsNullOrEmpty(dr["MODULO"].ToString()))
              dr["MODULO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

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

            if (dr["TRANS"] == null || string.IsNullOrEmpty(dr["TRANS"].ToString()))
            {
              return "Favor informar a transação.";
            }

            if (dr["TRANS"] != null && !string.IsNullOrEmpty(dr["TRANS"].ToString()))
            {
              if (dr["TRANS"].ToString().Length > 100)
              {
                return "A transação deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["NOME"] != null && !string.IsNullOrEmpty(dr["NOME"].ToString()))
            {
              if (dr["NOME"].ToString().Length > 100)
              {
                return "O nome deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["PUBLICA"] != null && !string.IsNullOrEmpty(dr["PUBLICA"].ToString()))
            {
              if (dr["PUBLICA"].ToString().Length > 1)
              {
                return "A pública deve possuir no máximo 1 caractere.";
              }

              if (dr["PUBLICA"] != null && !string.IsNullOrEmpty(dr["PUBLICA"].ToString()))
              {
                List<object> lstPUBLICA = new List<object>(new object[]{"N", "S"});
                if(!lstPUBLICA.Contains(dr["PUBLICA"].ToString()))
                {
                  return "Os valores permitidos para a pública são ''N'' ou ''S''.";
                }
              }
            }

            if (dr["ACESSOANONIMO"] != null && !string.IsNullOrEmpty(dr["ACESSOANONIMO"].ToString()))
            {
              if (dr["ACESSOANONIMO"].ToString().Length > 1)
              {
                return "O acesso anônimo deve possuir no máximo 1 caractere.";
              }

              if (dr["ACESSOANONIMO"] != null && !string.IsNullOrEmpty(dr["ACESSOANONIMO"].ToString()))
              {
                List<object> lstACESSOANONIMO = new List<object>(new object[]{"N", "S"});
                if(!lstACESSOANONIMO.Contains(dr["ACESSOANONIMO"].ToString()))
                {
                  return "Os valores permitidos para o acesso anônimo são ''N'' ou ''S''.";
                }
              }
            }

            if (dr["PAGINAWEB"] != null && !string.IsNullOrEmpty(dr["PAGINAWEB"].ToString()))
            {
              if (dr["PAGINAWEB"].ToString().Length > 100)
              {
                return "A página da web deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["FORM"] != null && !string.IsNullOrEmpty(dr["FORM"].ToString()))
            {
              if (dr["FORM"].ToString().Length > 8)
              {
                return "O Form deve possuir no máximo 8 caracteres.";
              }
            }

            if (dr["ITEMMENU"] != null && !string.IsNullOrEmpty(dr["ITEMMENU"].ToString()))
            {
              if (dr["ITEMMENU"].ToString().Length > 100)
              {
                return "O item do menu deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["AUDITAR"] != null && !string.IsNullOrEmpty(dr["AUDITAR"].ToString()))
            {
              if (dr["AUDITAR"].ToString().Length > 1)
              {
                return "A auditoria deve possuir no máximo 1 caractere.";
              }

              if (dr["AUDITAR"] != null && !string.IsNullOrEmpty(dr["AUDITAR"].ToString()))
              {
                List<object> lstAUDITAR = new List<object>(new object[]{"N", "S"});
                if(!lstAUDITAR.Contains(dr["AUDITAR"].ToString()))
                {
                  return "Os valores permitidos para a auditoria são ''N'' ou ''S''.";
                }
              }
            }

            if (dr["MODULO"] != null && !string.IsNullOrEmpty(dr["MODULO"].ToString()))
            {
              if (dr["MODULO"].ToString().Length > 30)
              {
                return "O módulo deve possuir no máximo 30 caracteres.";
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

    public static Hd_transacao Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_transacao Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_transacao tab = new Hd_transacao();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_transacao tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @sis, string @trans, string @nome, string @publica, string @acessoanonimo, 
        string @paginaweb, string @form, string @itemmenu, string @auditar, string @modulo
      ) {
        return (Row)Add(new object[] {
          @sis, @trans, @nome, @publica, @acessoanonimo, @paginaweb, @form, @itemmenu, @auditar, 
          @modulo
        });
      }

      public Row Find(
        string @sis, string @trans
      ) {
        return (Row)Find(new object[] {
          @sis, @trans
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
      /// TRANS: VARCHAR(100), Primary Key, Not Null
      /// </summary>
      public string Trans { get { return (string)this["trans"]; } set { this["trans"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(100)
      /// </summary>
      public string Nome { get { return (string)this["nome"]; } set { this["nome"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PUBLICA: CHAR(1)
      /// </summary>
      public string Publica { get { return (string)this["publica"]; } set { this["publica"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ACESSOANONIMO: CHAR(1)
      /// </summary>
      public string Acessoanonimo { get { return (string)this["acessoanonimo"]; } set { this["acessoanonimo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PAGINAWEB: VARCHAR(100)
      /// </summary>
      public string Paginaweb { get { return (string)this["paginaweb"]; } set { this["paginaweb"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// FORM: VARCHAR(8)
      /// </summary>
      public string Form { get { return (string)this["form"]; } set { this["form"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ITEMMENU: VARCHAR(100)
      /// </summary>
      public string Itemmenu { get { return (string)this["itemmenu"]; } set { this["itemmenu"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// AUDITAR: CHAR(1)
      /// </summary>
      public string Auditar { get { return (string)this["auditar"]; } set { this["auditar"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// MODULO: VARCHAR(30)
      /// </summary>
      public string Modulo { get { return (string)this["modulo"]; } set { this["modulo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(50), Not Null
      /// </summary>
      public string Nomesis { get { return (string)this["nomesis"]; } set { this["nomesis"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// VERSAO: VARCHAR(11)
      /// </summary>
      public string Versao { get { return (string)this["versao"]; } set { this["versao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TAKE: DATETIME
      /// </summary>
      public DateTime? Take { get { return (DateTime?)this["take"]; } set { this["take"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// HABILITADO: CHAR(1)
      /// </summary>
      public string Habilitado { get { return (string)this["habilitado"]; } set { this["habilitado"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @sis, string @trans) {
        return Insert(connection,
                      @sis, @trans,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @sis, string @trans,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_transacao(),
          new string[] { "sis", "trans" },
          new object[] { @sis, @trans },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkSis, string pkTrans,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_transacao(),
          new string[] { "sis", "trans" },
          new object[] { pkSis, pkTrans },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkSis, string pkTrans) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_transacao(),
          new string[] { "sis", "trans" },
          new object[] { pkSis, pkTrans }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkSis, string pkTrans) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_transacao(),
          new string[] { "sis", "trans" },
          new object[] { pkSis, pkTrans }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Sis { get { return (string)row["sis", DataRowVersion.Original]; } }
        public string Trans { get { return (string)row["trans", DataRowVersion.Original]; } }
        public string Nome { get { return (string)row["nome", DataRowVersion.Original]; } }
        public string Publica { get { return (string)row["publica", DataRowVersion.Original]; } }
        public string Acessoanonimo { get { return (string)row["acessoanonimo", DataRowVersion.Original]; } }
        public string Paginaweb { get { return (string)row["paginaweb", DataRowVersion.Original]; } }
        public string Form { get { return (string)row["form", DataRowVersion.Original]; } }
        public string Itemmenu { get { return (string)row["itemmenu", DataRowVersion.Original]; } }
        public string Auditar { get { return (string)row["auditar", DataRowVersion.Original]; } }
        public string Modulo { get { return (string)row["modulo", DataRowVersion.Original]; } }
        public string Nomesis { get { return (string)row["nomesis", DataRowVersion.Original]; } }
        public string Versao { get { return (string)row["versao", DataRowVersion.Original]; } }
        public DateTime? Take { get { return (DateTime?)row["take", DataRowVersion.Original]; } }
        public string Habilitado { get { return (string)row["habilitado", DataRowVersion.Original]; } }

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


      protected override string GetName(){return "sistema";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_transacao.sis"; } }
      public override string OriginalCol { get { return "sis"; } }
      public override string RefCol { get { return "HD_TRANSACAO.SIS"; } }
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

    #region Coluna: TRANS
    private ColumnTrans pvColTrans;

    public sealed class ColumnTrans : TDataColumn {

      public ColumnTrans() {
        this.ColumnName = "trans";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "transação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_transacao.trans"; } }
      public override string OriginalCol { get { return "trans"; } }
      public override string RefCol { get { return "HD_TRANSACAO.TRANS"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 100; } }
      public override string SqlServerType { get { return "VARCHAR(100)"; } }

    }
    #endregion

    #region Coluna: NOME
    private ColumnNome pvColNome;

    public sealed class ColumnNome : TDataColumn {

      public ColumnNome() {
        this.ColumnName = "nome";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "nome";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_transacao.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "HD_TRANSACAO.NOME"; } }
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

    #region Coluna: PUBLICA
    private ColumnPublica pvColPublica;

    public sealed class ColumnPublica : TDataColumn {

      public ColumnPublica() {
        this.ColumnName = "publica";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "pública";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_transacao.publica"; } }
      public override string OriginalCol { get { return "publica"; } }
      public override string RefCol { get { return "HD_TRANSACAO.PUBLICA"; } }
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

    #region Coluna: ACESSOANONIMO
    private ColumnAcessoanonimo pvColAcessoanonimo;

    public sealed class ColumnAcessoanonimo : TDataColumn {

      public ColumnAcessoanonimo() {
        this.ColumnName = "acessoanonimo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "acesso anônimo";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_transacao.acessoanonimo"; } }
      public override string OriginalCol { get { return "acessoanonimo"; } }
      public override string RefCol { get { return "HD_TRANSACAO.ACESSOANONIMO"; } }
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

    #region Coluna: PAGINAWEB
    private ColumnPaginaweb pvColPaginaweb;

    public sealed class ColumnPaginaweb : TDataColumn {

      public ColumnPaginaweb() {
        this.ColumnName = "paginaweb";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "página da web";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_transacao.paginaweb"; } }
      public override string OriginalCol { get { return "paginaweb"; } }
      public override string RefCol { get { return "HD_TRANSACAO.PAGINAWEB"; } }
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

    #region Coluna: FORM
    private ColumnForm pvColForm;

    public sealed class ColumnForm : TDataColumn {

      public ColumnForm() {
        this.ColumnName = "form";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_transacao.form"; } }
      public override string OriginalCol { get { return "form"; } }
      public override string RefCol { get { return "HD_TRANSACAO.FORM"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 8; } }
      public override string SqlServerType { get { return "VARCHAR(8)"; } }

    }
    #endregion

    #region Coluna: ITEMMENU
    private ColumnItemmenu pvColItemmenu;

    public sealed class ColumnItemmenu : TDataColumn {

      public ColumnItemmenu() {
        this.ColumnName = "itemmenu";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "item do menu";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_transacao.itemmenu"; } }
      public override string OriginalCol { get { return "itemmenu"; } }
      public override string RefCol { get { return "HD_TRANSACAO.ITEMMENU"; } }
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


      protected override string GetName(){return "auditoria";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_transacao.auditar"; } }
      public override string OriginalCol { get { return "auditar"; } }
      public override string RefCol { get { return "HD_TRANSACAO.AUDITAR"; } }
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

    #region Coluna: MODULO
    private ColumnModulo pvColModulo;

    public sealed class ColumnModulo : TDataColumn {

      public ColumnModulo() {
        this.ColumnName = "modulo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "módulo";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_transacao.modulo"; } }
      public override string OriginalCol { get { return "modulo"; } }
      public override string RefCol { get { return "HD_TRANSACAO.MODULO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 30; } }
      public override string SqlServerType { get { return "VARCHAR(30)"; } }

    }
    #endregion

    #region Coluna: NOMESIS
    private ColumnNomesis pvColNomesis;

    public sealed class ColumnNomesis : TDataColumn {

      public ColumnNomesis() {
        this.ColumnName = "nomesis";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "nome do sistema";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_sistema.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "HD_SISTEMA.NOME"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 50; } }
      public override string SqlServerType { get { return "VARCHAR(50)"; } }

    }
    #endregion

    #region Coluna: VERSAO
    private ColumnVersao pvColVersao;

    public sealed class ColumnVersao : TDataColumn {

      public ColumnVersao() {
        this.ColumnName = "versao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "versão";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_sistema.versao"; } }
      public override string OriginalCol { get { return "versao"; } }
      public override string RefCol { get { return "HD_SISTEMA.VERSAO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 11; } }
      public override string SqlServerType { get { return "VARCHAR(11)"; } }

    }
    #endregion

    #region Coluna: TAKE
    private ColumnTake pvColTake;

    public sealed class ColumnTake : TDataColumn {

      public ColumnTake() {
        this.ColumnName = "take";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_sistema.take"; } }
      public override string OriginalCol { get { return "take"; } }
      public override string RefCol { get { return "HD_SISTEMA.TAKE"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 10; } }
      public override string SqlServerType { get { return "DATETIME"; } }

    }
    #endregion

    #region Coluna: HABILITADO
    private ColumnHabilitado pvColHabilitado;

    public sealed class ColumnHabilitado : TDataColumn {

      public ColumnHabilitado() {
        this.ColumnName = "habilitado";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "habilitado";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_sistema.habilitado"; } }
      public override string OriginalCol { get { return "habilitado"; } }
      public override string RefCol { get { return "HD_SISTEMA.HABILITADO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 1; } }
      public override string SqlServerType { get { return "CHAR(1)"; } }

    }
    #endregion
    #endregion
  }
}

