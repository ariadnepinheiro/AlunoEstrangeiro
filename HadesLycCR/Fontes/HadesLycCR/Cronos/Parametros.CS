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
  public class Parametros : TDataSet {
    private const string DataSetName_Def = "Parametros";
    private static ConstructorInfo ctorTCustom = null;
    private CustomBase tcustom = null;
    #region Variáveis private para as tabelas deste dataset
    private Hd_grupo_parametros_hades pvTabHd_grupo_parametros_hades;
    private Hd_parametro_hades pvTabHd_parametro_hades;
    #endregion

    static Parametros() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Parametros");
      }
      catch{}
    }

    public Parametros() : base() {
      DataSetName = DataSetName_Def;

      pvTabHd_grupo_parametros_hades = new Hd_grupo_parametros_hades();
      pvTabHd_parametro_hades = new Hd_parametro_hades();

      Tables.Add(pvTabHd_grupo_parametros_hades);
      Tables.Add(pvTabHd_parametro_hades);

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
          pvTabHd_grupo_parametros_hades.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_grupo_parametros_hades); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabHd_parametro_hades.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_parametro_hades); if(row != null) return DataLib.RowErrorsToString(row);
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
    public Hd_grupo_parametros_hades Hd_grupo_parametros_hades { get { return pvTabHd_grupo_parametros_hades; } }

    [
      Browsable(false),
    ]
    public Hd_parametro_hades Hd_parametro_hades { get { return pvTabHd_parametro_hades; } }

    #endregion

    public abstract class CustomBase {
      public abstract string Put(Techne.HadesLyc.CR.Parametros ds, TConnectionWritable connection);
    }

  }


  public partial class Hd_grupo_parametros_hades : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_grupo_parametros_hades() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_grupo_parametros_hades");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_grupo_parametros_hades");
      }
      catch{}
    }

    public Hd_grupo_parametros_hades() {
      pvColSis = new ColumnSis(); Columns.Add(pvColSis);
      pvColGrupo = new ColumnGrupo(); Columns.Add(pvColGrupo);
      pvColDescricao = new ColumnDescricao(); Columns.Add(pvColDescricao);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_grupo_parametros_hades");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @sis, string @grupo) {
      Row row = NewRow();

      row.Sis = @sis;
      row.Grupo = @grupo;

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
      get { return "HD_GRUPO_PARAMETROS_HADES"; }
    }

    protected override sealed string Name { get { return "Grupos de parâmetros"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColSis, pvColGrupo });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_grupo_parametros_had_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_grupo_parametros_had_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_grupo_parametros_had_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_grupo_parametros_had_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkSis", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "SIS", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkGrupo", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "GRUPO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("sis", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SIS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("grupo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "GRUPO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("descricao", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "DESCRICAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "grupo") { result.Add(toLower ? "grupo" : column); continue; }
          if(lower == "descricao") { result.Add(toLower ? "descricao" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "sis", "grupo", "descricao" }
                          : new string[] { "SIS", "GRUPO", "DESCRICAO" });
        else
          result.AddRange(toLower
                          ? new string[] { "sis", "grupo", "descricao" }
                          : new string[] { "SIS", "GRUPO", "DESCRICAO" });

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
        "hd_grupo_parametros_hades.sis", "hd_grupo_parametros_hades.grupo"
      });
      if(scColumns.Contains("descricao")) list.Add("hd_grupo_parametros_hades.descricao");
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
          "FROM hd_grupo_parametros_hades " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_grupo_parametros_hades");
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
        da.TableMappings.Add("Table", "Hd_grupo_parametros_hades");
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
            if (dr["GRUPO"] != null && string.IsNullOrEmpty(dr["GRUPO"].ToString()))
              dr["GRUPO"] = DBNull.Value;
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

            if (dr["GRUPO"] == null || string.IsNullOrEmpty(dr["GRUPO"].ToString()))
            {
              return "Favor informar o Grupo.";
            }

            if (dr["GRUPO"] != null && !string.IsNullOrEmpty(dr["GRUPO"].ToString()))
            {
              if (dr["GRUPO"].ToString().Length > 50)
              {
                return "O Grupo deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["DESCRICAO"] != null && !string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              if (dr["DESCRICAO"].ToString().Length > 60)
              {
                return "O Descricao deve possuir no máximo 60 caracteres.";
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

    public static Hd_grupo_parametros_hades Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_grupo_parametros_hades Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_grupo_parametros_hades tab = new Hd_grupo_parametros_hades();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_grupo_parametros_hades tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @sis, string @grupo, string @descricao
      ) {
        return (Row)Add(new object[] {
          @sis, @grupo, @descricao
        });
      }

      public Row Find(
        string @sis, string @grupo
      ) {
        return (Row)Find(new object[] {
          @sis, @grupo
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
      /// GRUPO: VARCHAR(50), Primary Key, Not Null
      /// </summary>
      public string Grupo { get { return (string)this["grupo"]; } set { this["grupo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO: VARCHAR(60)
      /// </summary>
      public string Descricao { get { return (string)this["descricao"]; } set { this["descricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @sis, string @grupo) {
        return Insert(connection,
                      @sis, @grupo,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @sis, string @grupo,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_grupo_parametros_hades(),
          new string[] { "sis", "grupo" },
          new object[] { @sis, @grupo },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkSis, string pkGrupo,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_grupo_parametros_hades(),
          new string[] { "sis", "grupo" },
          new object[] { pkSis, pkGrupo },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkSis, string pkGrupo) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_grupo_parametros_hades(),
          new string[] { "sis", "grupo" },
          new object[] { pkSis, pkGrupo }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkSis, string pkGrupo) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_grupo_parametros_hades(),
          new string[] { "sis", "grupo" },
          new object[] { pkSis, pkGrupo }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Sis { get { return (string)row["sis", DataRowVersion.Original]; } }
        public string Grupo { get { return (string)row["grupo", DataRowVersion.Original]; } }
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
      public override string FullCol { get { return "hd_grupo_parametros_hades.sis"; } }
      public override string OriginalCol { get { return "sis"; } }
      public override string RefCol { get { return "HD_GRUPO_PARAMETROS_HADES.SIS"; } }
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

    #region Coluna: GRUPO
    private ColumnGrupo pvColGrupo;

    public sealed class ColumnGrupo : TDataColumn {

      public ColumnGrupo() {
        this.ColumnName = "grupo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_grupo_parametros_hades.grupo"; } }
      public override string OriginalCol { get { return "grupo"; } }
      public override string RefCol { get { return "HD_GRUPO_PARAMETROS_HADES.GRUPO"; } }
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
      public override string FullCol { get { return "hd_grupo_parametros_hades.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "HD_GRUPO_PARAMETROS_HADES.DESCRICAO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 60; } }
      public override string SqlServerType { get { return "VARCHAR(60)"; } }

    }
    #endregion
    #endregion
  }

  public partial class Hd_parametro_hades : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_parametro_hades() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_parametro_hades");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_parametro_hades");
      }
      catch{}
    }

    public Hd_parametro_hades() {
      pvColSis = new ColumnSis(); Columns.Add(pvColSis);
      pvColGrupo = new ColumnGrupo(); Columns.Add(pvColGrupo);
      pvColOpcao = new ColumnOpcao(); Columns.Add(pvColOpcao);
      pvColDescricao = new ColumnDescricao(); Columns.Add(pvColDescricao);
      pvColValor = new ColumnValor(); Columns.Add(pvColValor);
      pvColTipo_valor = new ColumnTipo_valor(); Columns.Add(pvColTipo_valor);
      pvColTamanho = new ColumnTamanho(); Columns.Add(pvColTamanho);
      pvColSql_lista = new ColumnSql_lista(); Columns.Add(pvColSql_lista);
      pvColCol_valor = new ColumnCol_valor(); Columns.Add(pvColCol_valor);
      pvColCol_descr = new ColumnCol_descr(); Columns.Add(pvColCol_descr);
      pvColValmin = new ColumnValmin(); Columns.Add(pvColValmin);
      pvColValmax = new ColumnValmax(); Columns.Add(pvColValmax);
      pvColValor_default = new ColumnValor_default(); Columns.Add(pvColValor_default);
      pvColTipo_alt = new ColumnTipo_alt(); Columns.Add(pvColTipo_alt);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_parametro_hades");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @sis, string @grupo, string @opcao, string @tipo_valor, 
                      decimal? @tamanho, decimal? @tipo_alt) {
      Row row = NewRow();

      row.Sis = @sis;
      row.Grupo = @grupo;
      row.Opcao = @opcao;
      row.Tipo_valor = @tipo_valor;
      row.Tamanho = @tamanho;
      row.Tipo_alt = @tipo_alt;

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
      get { return "HD_PARAMETRO_HADES"; }
    }

    protected override sealed string Name { get { return "Parâmetros do Hades"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColSis, pvColGrupo, pvColOpcao });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_parametro_hades_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_parametro_hades_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_parametro_hades_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_parametro_hades_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkSis", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "SIS", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkGrupo", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "GRUPO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkOpcao", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "OPCAO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("sis", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SIS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("grupo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "GRUPO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("opcao", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "OPCAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("descricao", OleDbType.VarChar, 1600, ParameterDirection.InputOutput, true, 0, 0, "DESCRICAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("valor", OleDbType.VarChar, 2100, ParameterDirection.InputOutput, true, 0, 0, "VALOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("tipo_valor", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TIPO_VALOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("tamanho", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "TAMANHO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("sql_lista", OleDbType.VarChar, 2100, ParameterDirection.InputOutput, true, 0, 0, "SQL_LISTA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("col_valor", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "COL_VALOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("col_descr", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "COL_DESCR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("valmin", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 38, 30, "VALMIN", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("valmax", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 38, 30, "VALMAX", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("valor_default", OleDbType.VarChar, 2100, ParameterDirection.InputOutput, true, 0, 0, "VALOR_DEFAULT", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("tipo_alt", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "TIPO_ALT", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "grupo") { result.Add(toLower ? "grupo" : column); continue; }
          if(lower == "opcao") { result.Add(toLower ? "opcao" : column); continue; }
          if(lower == "descricao") { result.Add(toLower ? "descricao" : column); continue; }
          if(lower == "valor") { result.Add(toLower ? "valor" : column); continue; }
          if(lower == "tipo_valor") { result.Add(toLower ? "tipo_valor" : column); continue; }
          if(lower == "tamanho") { result.Add(toLower ? "tamanho" : column); continue; }
          if(lower == "sql_lista") { result.Add(toLower ? "sql_lista" : column); continue; }
          if(lower == "col_valor") { result.Add(toLower ? "col_valor" : column); continue; }
          if(lower == "col_descr") { result.Add(toLower ? "col_descr" : column); continue; }
          if(lower == "valmin") { result.Add(toLower ? "valmin" : column); continue; }
          if(lower == "valmax") { result.Add(toLower ? "valmax" : column); continue; }
          if(lower == "valor_default") { result.Add(toLower ? "valor_default" : column); continue; }
          if(lower == "tipo_alt") { result.Add(toLower ? "tipo_alt" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "sis", "grupo", "opcao", "descricao", "valor", "tipo_valor", "tamanho", 
                                           "sql_lista", "col_valor", "col_descr", "valmin", "valmax", "valor_default", "tipo_alt" }
                          : new string[] { "SIS", "GRUPO", "OPCAO", "DESCRICAO", "VALOR", "TIPO_VALOR", "TAMANHO", 
                                           "SQL_LISTA", "COL_VALOR", "COL_DESCR", "VALMIN", "VALMAX", "VALOR_DEFAULT", "TIPO_ALT" });
        else
          result.AddRange(toLower
                          ? new string[] { "sis", "grupo", "opcao", "descricao", "valor", "tipo_valor", "tamanho", 
                                           "sql_lista", "col_valor", "col_descr", "valmin", "valmax", "valor_default", "tipo_alt" }
                          : new string[] { "SIS", "GRUPO", "OPCAO", "DESCRICAO", "VALOR", "TIPO_VALOR", "TAMANHO", 
                                           "SQL_LISTA", "COL_VALOR", "COL_DESCR", "VALMIN", "VALMAX", "VALOR_DEFAULT", "TIPO_ALT" });

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
        "hd_parametro_hades.sis", "hd_parametro_hades.grupo", "hd_parametro_hades.opcao"
      });
      if(scColumns.Contains("descricao")) list.Add("hd_parametro_hades.descricao");
      if(scColumns.Contains("valor")) list.Add("hd_parametro_hades.valor");
      if(scColumns.Contains("tipo_valor")) list.Add("hd_parametro_hades.tipo_valor");
      if(scColumns.Contains("tamanho")) list.Add("hd_parametro_hades.tamanho");
      if(scColumns.Contains("sql_lista")) list.Add("hd_parametro_hades.sql_lista");
      if(scColumns.Contains("col_valor")) list.Add("hd_parametro_hades.col_valor");
      if(scColumns.Contains("col_descr")) list.Add("hd_parametro_hades.col_descr");
      if(scColumns.Contains("valmin")) list.Add("hd_parametro_hades.valmin");
      if(scColumns.Contains("valmax")) list.Add("hd_parametro_hades.valmax");
      if(scColumns.Contains("valor_default")) list.Add("hd_parametro_hades.valor_default");
      if(scColumns.Contains("tipo_alt")) list.Add("hd_parametro_hades.tipo_alt");
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
          "FROM hd_parametro_hades " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_parametro_hades");
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
        da.TableMappings.Add("Table", "Hd_parametro_hades");
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
            if (dr["GRUPO"] != null && string.IsNullOrEmpty(dr["GRUPO"].ToString()))
              dr["GRUPO"] = DBNull.Value;
            if (dr["OPCAO"] != null && string.IsNullOrEmpty(dr["OPCAO"].ToString()))
              dr["OPCAO"] = DBNull.Value;
            if (dr["DESCRICAO"] != null && string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
              dr["DESCRICAO"] = DBNull.Value;
            if (dr["VALOR"] != null && string.IsNullOrEmpty(dr["VALOR"].ToString()))
              dr["VALOR"] = DBNull.Value;
            if (dr["TIPO_VALOR"] != null && string.IsNullOrEmpty(dr["TIPO_VALOR"].ToString()))
              dr["TIPO_VALOR"] = DBNull.Value;
            if (dr["TAMANHO"] != null && string.IsNullOrEmpty(dr["TAMANHO"].ToString()))
              dr["TAMANHO"] = DBNull.Value;
            if (dr["SQL_LISTA"] != null && string.IsNullOrEmpty(dr["SQL_LISTA"].ToString()))
              dr["SQL_LISTA"] = DBNull.Value;
            if (dr["COL_VALOR"] != null && string.IsNullOrEmpty(dr["COL_VALOR"].ToString()))
              dr["COL_VALOR"] = DBNull.Value;
            if (dr["COL_DESCR"] != null && string.IsNullOrEmpty(dr["COL_DESCR"].ToString()))
              dr["COL_DESCR"] = DBNull.Value;
            if (dr["VALMIN"] != null && string.IsNullOrEmpty(dr["VALMIN"].ToString()))
              dr["VALMIN"] = DBNull.Value;
            if (dr["VALMAX"] != null && string.IsNullOrEmpty(dr["VALMAX"].ToString()))
              dr["VALMAX"] = DBNull.Value;
            if (dr["VALOR_DEFAULT"] != null && string.IsNullOrEmpty(dr["VALOR_DEFAULT"].ToString()))
              dr["VALOR_DEFAULT"] = DBNull.Value;
            if (dr["TIPO_ALT"] != null && string.IsNullOrEmpty(dr["TIPO_ALT"].ToString()))
              dr["TIPO_ALT"] = DBNull.Value;
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

            if (dr["GRUPO"] == null || string.IsNullOrEmpty(dr["GRUPO"].ToString()))
            {
              return "Favor informar o Grupo.";
            }

            if (dr["GRUPO"] != null && !string.IsNullOrEmpty(dr["GRUPO"].ToString()))
            {
              if (dr["GRUPO"].ToString().Length > 50)
              {
                return "O Grupo deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["OPCAO"] == null || string.IsNullOrEmpty(dr["OPCAO"].ToString()))
            {
              return "Favor informar o Opcao.";
            }

            if (dr["OPCAO"] != null && !string.IsNullOrEmpty(dr["OPCAO"].ToString()))
            {
              if (dr["OPCAO"].ToString().Length > 30)
              {
                return "O Opcao deve possuir no máximo 30 caracteres.";
              }
            }

            if (dr["DESCRICAO"] != null && !string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              if (dr["DESCRICAO"].ToString().Length > 1500)
              {
                return "O Descricao deve possuir no máximo 1500 caracteres.";
              }
            }

            if (dr["VALOR"] != null && !string.IsNullOrEmpty(dr["VALOR"].ToString()))
            {
              if (dr["VALOR"].ToString().Length > 2000)
              {
                return "O Valor deve possuir no máximo 2000 caracteres.";
              }
            }

            if (dr["TIPO_VALOR"] == null || string.IsNullOrEmpty(dr["TIPO_VALOR"].ToString()))
            {
              return "Favor informar o Tipo_valor.";
            }

            if (dr["TIPO_VALOR"] != null && !string.IsNullOrEmpty(dr["TIPO_VALOR"].ToString()))
            {
              if (dr["TIPO_VALOR"].ToString().Length > 1)
              {
                return "O Tipo_valor deve possuir no máximo 1 caractere.";
              }

              if (dr["TIPO_VALOR"] != null && !string.IsNullOrEmpty(dr["TIPO_VALOR"].ToString()))
              {
                List<object> lstTIPO_VALOR = new List<object>(new object[]{"D", "V", "N"});
                if(!lstTIPO_VALOR.Contains(dr["TIPO_VALOR"].ToString()))
                {
                  return "Os valores permitidos para o Tipo_valor são ''D'', ''V'' ou ''N''.";
                }
              }
            }

            if (dr["TAMANHO"] == null || string.IsNullOrEmpty(dr["TAMANHO"].ToString()))
            {
              return "Favor informar o Tamanho.";
            }

            if (dr["TAMANHO"] != null && !string.IsNullOrEmpty(dr["TAMANHO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["TAMANHO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["TAMANHO"]).ToString("0.############").Split(',')[0].Length > 4) || (Convert.ToDecimal(dr["TAMANHO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["TAMANHO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Tamanho deve ser um valor inteiro com 4 dígitos no máximo.";
              }

              if (dr["TAMANHO"] != null && !string.IsNullOrEmpty(dr["TAMANHO"].ToString()))
              {
                if (Convert.ToDecimal(dr["TAMANHO"]) < (1))
                {
                  return "O Tamanho deve ser maior ou igual a (1).";
                }
              }
            }

            if (dr["SQL_LISTA"] != null && !string.IsNullOrEmpty(dr["SQL_LISTA"].ToString()))
            {
              if (dr["SQL_LISTA"].ToString().Length > 2000)
              {
                return "O Sql_lista deve possuir no máximo 2000 caracteres.";
              }
            }

            if (dr["COL_VALOR"] != null && !string.IsNullOrEmpty(dr["COL_VALOR"].ToString()))
            {
              if (dr["COL_VALOR"].ToString().Length > 30)
              {
                return "O Col_valor deve possuir no máximo 30 caracteres.";
              }
            }

            if (dr["COL_DESCR"] != null && !string.IsNullOrEmpty(dr["COL_DESCR"].ToString()))
            {
              if (dr["COL_DESCR"].ToString().Length > 30)
              {
                return "O Col_descr deve possuir no máximo 30 caracteres.";
              }
            }

            if (dr["VALMIN"] != null && !string.IsNullOrEmpty(dr["VALMIN"].ToString()))
            {
              if ((Convert.ToDecimal(dr["VALMIN"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["VALMIN"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["VALMIN"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["VALMIN"]).ToString("0.############").Split(',')[1].Length > 25))
              {
                return "O Valmin deve ser um valor numérico com, no máximo, 3 dígitos na parte inteira e 25 casas decimal.";
              }
            }

            if (dr["VALMAX"] != null && !string.IsNullOrEmpty(dr["VALMAX"].ToString()))
            {
              if ((Convert.ToDecimal(dr["VALMAX"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["VALMAX"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["VALMAX"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["VALMAX"]).ToString("0.############").Split(',')[1].Length > 25))
              {
                return "O Valmax deve ser um valor numérico com, no máximo, 3 dígitos na parte inteira e 25 casas decimal.";
              }
            }

            if (dr["VALOR_DEFAULT"] != null && !string.IsNullOrEmpty(dr["VALOR_DEFAULT"].ToString()))
            {
              if (dr["VALOR_DEFAULT"].ToString().Length > 2000)
              {
                return "O Valor_default deve possuir no máximo 2000 caracteres.";
              }
            }

            if (dr["TIPO_ALT"] == null || string.IsNullOrEmpty(dr["TIPO_ALT"].ToString()))
            {
              return "Favor informar o Tipo_alt.";
            }

            if (dr["TIPO_ALT"] != null && !string.IsNullOrEmpty(dr["TIPO_ALT"].ToString()))
            {
              if ((Convert.ToDecimal(dr["TIPO_ALT"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["TIPO_ALT"]).ToString("0.############").Split(',')[0].Length > 1) || (Convert.ToDecimal(dr["TIPO_ALT"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["TIPO_ALT"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Tipo_alt deve ser um valor inteiro com 1 dígito no máximo.";
              }

              if (dr["TIPO_ALT"] != null && !string.IsNullOrEmpty(dr["TIPO_ALT"].ToString()))
              {
                List<object> lstTIPO_ALT = new List<object>(new object[]{(2), (1), (0)});
                if(!lstTIPO_ALT.Contains(dr["TIPO_ALT"].ToString()))
                {
                  return "Os valores permitidos para o Tipo_alt são (2), (1) ou (0).";
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

    public static Hd_parametro_hades Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_parametro_hades Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_parametro_hades tab = new Hd_parametro_hades();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_parametro_hades tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @sis, string @grupo, string @opcao, string @descricao, string @valor, string @tipo_valor, 
        decimal? @tamanho, string @sql_lista, string @col_valor, string @col_descr, decimal? @valmin, 
        decimal? @valmax, string @valor_default, decimal? @tipo_alt
      ) {
        return (Row)Add(new object[] {
          @sis, @grupo, @opcao, @descricao, @valor, @tipo_valor, @tamanho, @sql_lista, @col_valor, 
          @col_descr, @valmin, @valmax, @valor_default, @tipo_alt
        });
      }

      public Row Find(
        string @sis, string @grupo, string @opcao
      ) {
        return (Row)Find(new object[] {
          @sis, @grupo, @opcao
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
      /// GRUPO: VARCHAR(50), Primary Key, Not Null
      /// </summary>
      public string Grupo { get { return (string)this["grupo"]; } set { this["grupo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// OPCAO: VARCHAR(30), Primary Key, Not Null
      /// </summary>
      public string Opcao { get { return (string)this["opcao"]; } set { this["opcao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO: VARCHAR(1500)
      /// </summary>
      public string Descricao { get { return (string)this["descricao"]; } set { this["descricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// VALOR: VARCHAR(2000)
      /// </summary>
      public string Valor { get { return (string)this["valor"]; } set { this["valor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TIPO_VALOR: CHAR(1), Not Null
      /// </summary>
      public string Tipo_valor { get { return (string)this["tipo_valor"]; } set { this["tipo_valor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TAMANHO: NUMERIC(4), Not Null
      /// </summary>
      public decimal? Tamanho { get { return (decimal?)this["tamanho"]; } set { this["tamanho"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SQL_LISTA: VARCHAR(2000)
      /// </summary>
      public string Sql_lista { get { return (string)this["sql_lista"]; } set { this["sql_lista"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// COL_VALOR: VARCHAR(30)
      /// </summary>
      public string Col_valor { get { return (string)this["col_valor"]; } set { this["col_valor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// COL_DESCR: VARCHAR(30)
      /// </summary>
      public string Col_descr { get { return (string)this["col_descr"]; } set { this["col_descr"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// VALMIN: NUMERIC(28, 25)
      /// </summary>
      public decimal? Valmin { get { return (decimal?)this["valmin"]; } set { this["valmin"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// VALMAX: NUMERIC(28, 25)
      /// </summary>
      public decimal? Valmax { get { return (decimal?)this["valmax"]; } set { this["valmax"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// VALOR_DEFAULT: VARCHAR(2000)
      /// </summary>
      public string Valor_default { get { return (string)this["valor_default"]; } set { this["valor_default"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TIPO_ALT: NUMERIC(1), Not Null
      /// </summary>
      public decimal? Tipo_alt { get { return (decimal?)this["tipo_alt"]; } set { this["tipo_alt"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @sis, string @grupo, string @opcao, string @tipo_valor, decimal? @tamanho, 
                               decimal? @tipo_alt) {
        return Insert(connection,
                      @sis, @grupo, @opcao, @tipo_valor, @tamanho, @tipo_alt,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @sis, string @grupo, string @opcao, string @tipo_valor, decimal? @tamanho, 
                               decimal? @tipo_alt,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_parametro_hades(),
          new string[] { "sis", "grupo", "opcao", "tipo_valor", "tamanho", "tipo_alt" },
          new object[] { @sis, @grupo, @opcao, @tipo_valor, @tamanho, @tipo_alt },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkSis, string pkGrupo, string pkOpcao,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_parametro_hades(),
          new string[] { "sis", "grupo", "opcao" },
          new object[] { pkSis, pkGrupo, pkOpcao },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkSis, string pkGrupo, string pkOpcao) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_parametro_hades(),
          new string[] { "sis", "grupo", "opcao" },
          new object[] { pkSis, pkGrupo, pkOpcao }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkSis, string pkGrupo, string pkOpcao) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_parametro_hades(),
          new string[] { "sis", "grupo", "opcao" },
          new object[] { pkSis, pkGrupo, pkOpcao }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Sis { get { return (string)row["sis", DataRowVersion.Original]; } }
        public string Grupo { get { return (string)row["grupo", DataRowVersion.Original]; } }
        public string Opcao { get { return (string)row["opcao", DataRowVersion.Original]; } }
        public string Descricao { get { return (string)row["descricao", DataRowVersion.Original]; } }
        public string Valor { get { return (string)row["valor", DataRowVersion.Original]; } }
        public string Tipo_valor { get { return (string)row["tipo_valor", DataRowVersion.Original]; } }
        public decimal? Tamanho { get { return (decimal?)row["tamanho", DataRowVersion.Original]; } }
        public string Sql_lista { get { return (string)row["sql_lista", DataRowVersion.Original]; } }
        public string Col_valor { get { return (string)row["col_valor", DataRowVersion.Original]; } }
        public string Col_descr { get { return (string)row["col_descr", DataRowVersion.Original]; } }
        public decimal? Valmin { get { return (decimal?)row["valmin", DataRowVersion.Original]; } }
        public decimal? Valmax { get { return (decimal?)row["valmax", DataRowVersion.Original]; } }
        public string Valor_default { get { return (string)row["valor_default", DataRowVersion.Original]; } }
        public decimal? Tipo_alt { get { return (decimal?)row["tipo_alt", DataRowVersion.Original]; } }

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
      public override string FullCol { get { return "hd_parametro_hades.sis"; } }
      public override string OriginalCol { get { return "sis"; } }
      public override string RefCol { get { return "HD_PARAMETRO_HADES.SIS"; } }
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

    #region Coluna: GRUPO
    private ColumnGrupo pvColGrupo;

    public sealed class ColumnGrupo : TDataColumn {

      public ColumnGrupo() {
        this.ColumnName = "grupo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_parametro_hades.grupo"; } }
      public override string OriginalCol { get { return "grupo"; } }
      public override string RefCol { get { return "HD_PARAMETRO_HADES.GRUPO"; } }
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

    #region Coluna: OPCAO
    private ColumnOpcao pvColOpcao;

    public sealed class ColumnOpcao : TDataColumn {

      public ColumnOpcao() {
        this.ColumnName = "opcao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_parametro_hades.opcao"; } }
      public override string OriginalCol { get { return "opcao"; } }
      public override string RefCol { get { return "HD_PARAMETRO_HADES.OPCAO"; } }
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
      public override string FullCol { get { return "hd_parametro_hades.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "HD_PARAMETRO_HADES.DESCRICAO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 1500; } }
      public override string SqlServerType { get { return "VARCHAR(1500)"; } }

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
      public override string FullCol { get { return "hd_parametro_hades.valor"; } }
      public override string OriginalCol { get { return "valor"; } }
      public override string RefCol { get { return "HD_PARAMETRO_HADES.VALOR"; } }
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

    #region Coluna: TIPO_VALOR
    private ColumnTipo_valor pvColTipo_valor;

    public sealed class ColumnTipo_valor : TDataColumn {

      public ColumnTipo_valor() {
        this.ColumnName = "tipo_valor";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_parametro_hades.tipo_valor"; } }
      public override string OriginalCol { get { return "tipo_valor"; } }
      public override string RefCol { get { return "HD_PARAMETRO_HADES.TIPO_VALOR"; } }
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

    #region Coluna: TAMANHO
    private ColumnTamanho pvColTamanho;

    public sealed class ColumnTamanho : TDataColumn {

      public ColumnTamanho() {
        this.ColumnName = "tamanho";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_parametro_hades.tamanho"; } }
      public override string OriginalCol { get { return "tamanho"; } }
      public override string RefCol { get { return "HD_PARAMETRO_HADES.TAMANHO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 5; } }
      public override int Precision { get { return 4; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(4)"; } }

      public override object MinValue { get { return 1; } }

    }
    #endregion

    #region Coluna: SQL_LISTA
    private ColumnSql_lista pvColSql_lista;

    public sealed class ColumnSql_lista : TDataColumn {

      public ColumnSql_lista() {
        this.ColumnName = "sql_lista";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_parametro_hades.sql_lista"; } }
      public override string OriginalCol { get { return "sql_lista"; } }
      public override string RefCol { get { return "HD_PARAMETRO_HADES.SQL_LISTA"; } }
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
      public override string FullCol { get { return "hd_parametro_hades.col_valor"; } }
      public override string OriginalCol { get { return "col_valor"; } }
      public override string RefCol { get { return "HD_PARAMETRO_HADES.COL_VALOR"; } }
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
      public override string FullCol { get { return "hd_parametro_hades.col_descr"; } }
      public override string OriginalCol { get { return "col_descr"; } }
      public override string RefCol { get { return "HD_PARAMETRO_HADES.COL_DESCR"; } }
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

    #region Coluna: VALMIN
    private ColumnValmin pvColValmin;

    public sealed class ColumnValmin : TDataColumn {

      public ColumnValmin() {
        this.ColumnName = "valmin";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_parametro_hades.valmin"; } }
      public override string OriginalCol { get { return "valmin"; } }
      public override string RefCol { get { return "HD_PARAMETRO_HADES.VALMIN"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 30; } }
      public override int Precision { get { return 28; } }
      public override int Scale { get { return 25; } }
      public override string SqlServerType { get { return "NUMERIC(28, 25)"; } }

    }
    #endregion

    #region Coluna: VALMAX
    private ColumnValmax pvColValmax;

    public sealed class ColumnValmax : TDataColumn {

      public ColumnValmax() {
        this.ColumnName = "valmax";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_parametro_hades.valmax"; } }
      public override string OriginalCol { get { return "valmax"; } }
      public override string RefCol { get { return "HD_PARAMETRO_HADES.VALMAX"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 30; } }
      public override int Precision { get { return 28; } }
      public override int Scale { get { return 25; } }
      public override string SqlServerType { get { return "NUMERIC(28, 25)"; } }

    }
    #endregion

    #region Coluna: VALOR_DEFAULT
    private ColumnValor_default pvColValor_default;

    public sealed class ColumnValor_default : TDataColumn {

      public ColumnValor_default() {
        this.ColumnName = "valor_default";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_parametro_hades.valor_default"; } }
      public override string OriginalCol { get { return "valor_default"; } }
      public override string RefCol { get { return "HD_PARAMETRO_HADES.VALOR_DEFAULT"; } }
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

    #region Coluna: TIPO_ALT
    private ColumnTipo_alt pvColTipo_alt;

    public sealed class ColumnTipo_alt : TDataColumn {

      public ColumnTipo_alt() {
        this.ColumnName = "tipo_alt";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_parametro_hades.tipo_alt"; } }
      public override string OriginalCol { get { return "tipo_alt"; } }
      public override string RefCol { get { return "HD_PARAMETRO_HADES.TIPO_ALT"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 2; } }
      public override int Precision { get { return 1; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(1)"; } }

    }
    #endregion
    #endregion
  }
}

