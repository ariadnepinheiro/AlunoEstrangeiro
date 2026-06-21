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
  public class Menu : TDataSet {
    private const string DataSetName_Def = "Menu";
    private static ConstructorInfo ctorTCustom = null;
    private CustomBase tcustom = null;
    #region Variáveis private para as tabelas deste dataset
    private Hd_menu pvTabHd_menu;
    private Hd_menuitem pvTabHd_menuitem;
    private Hd_sistema pvTabHd_sistema;
    private Hd_transacao pvTabHd_transacao;
    #endregion

    static Menu() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Menu");
      }
      catch{}
    }

    public Menu() : base() {
      DataSetName = DataSetName_Def;

      pvTabHd_menu = new Hd_menu();
      pvTabHd_menuitem = new Hd_menuitem();
      pvTabHd_sistema = new Hd_sistema();
      pvTabHd_transacao = new Hd_transacao();

      Tables.Add(pvTabHd_menu);
      Tables.Add(pvTabHd_menuitem);
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
      Relations.Add("fk_menu_sistema",
                    new DataColumn[] { pvTabHd_sistema.Columns["sis"] },
                    new DataColumn[] { pvTabHd_menu.Columns["sis"] }, false);
      Relations.Add("fk_menuitem_menu",
                    new DataColumn[] { pvTabHd_menu.Columns["menu"] },
                    new DataColumn[] { pvTabHd_menuitem.Columns["menu"] }, false);
      Relations.Add("fk_menuitem_transacao",
                    new DataColumn[] { pvTabHd_transacao.Columns["sis"], pvTabHd_transacao.Columns["trans"] },
                    new DataColumn[] { pvTabHd_menuitem.Columns["sis"], pvTabHd_menuitem.Columns["trans"] }, false);
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
          pvTabHd_menu.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_menu); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabHd_menuitem.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_menuitem); if(row != null) return DataLib.RowErrorsToString(row);
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
    public Hd_menu Hd_menu { get { return pvTabHd_menu; } }

    [
      Browsable(false),
    ]
    public Hd_menuitem Hd_menuitem { get { return pvTabHd_menuitem; } }

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
      public abstract string Put(Techne.HadesLyc.CR.Menu ds, TConnectionWritable connection);
    }

  }


  public partial class Hd_menu : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_menu() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_menu");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_menu");
      }
      catch{}
    }

    public Hd_menu() {
      pvColMenu = new ColumnMenu(); Columns.Add(pvColMenu);
      pvColSis = new ColumnSis(); Columns.Add(pvColSis);
      pvColDescricao = new ColumnDescricao(); Columns.Add(pvColDescricao);
      pvColTipo = new ColumnTipo(); Columns.Add(pvColTipo);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_menu");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @menu, string @sis) {
      Row row = NewRow();

      row.Menu = @menu;
      row.Sis = @sis;

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
      get { return "HD_MENU"; }
    }

    protected override sealed string Name { get { return "Menus"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColMenu });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_menu_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_menu_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_menu_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_menu_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkMenu", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "MENU", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("menu", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "MENU", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("sis", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SIS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("descricao", OleDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "DESCRICAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("tipo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TIPO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "menu") { result.Add(toLower ? "menu" : column); continue; }
          if(lower == "sis") { result.Add(toLower ? "sis" : column); continue; }
          if(lower == "descricao") { result.Add(toLower ? "descricao" : column); continue; }
          if(lower == "tipo") { result.Add(toLower ? "tipo" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "menu", "sis", "descricao", "tipo" }
                          : new string[] { "MENU", "SIS", "DESCRICAO", "TIPO" });
        else
          result.AddRange(toLower
                          ? new string[] { "menu", "sis", "descricao", "tipo" }
                          : new string[] { "MENU", "SIS", "DESCRICAO", "TIPO" });

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
        "hd_menu.menu"
      });
      if(scColumns.Contains("sis")) list.Add("hd_menu.sis");
      if(scColumns.Contains("descricao")) list.Add("hd_menu.descricao");
      if(scColumns.Contains("tipo")) list.Add("hd_menu.tipo");
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
          "FROM hd_menu " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_menu");
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
        da.TableMappings.Add("Table", "Hd_menu");
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
            if (dr["MENU"] != null && string.IsNullOrEmpty(dr["MENU"].ToString()))
              dr["MENU"] = DBNull.Value;
            if (dr["SIS"] != null && string.IsNullOrEmpty(dr["SIS"].ToString()))
              dr["SIS"] = DBNull.Value;
            if (dr["DESCRICAO"] != null && string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
              dr["DESCRICAO"] = DBNull.Value;
            if (dr["TIPO"] != null && string.IsNullOrEmpty(dr["TIPO"].ToString()))
              dr["TIPO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["MENU"] == null || string.IsNullOrEmpty(dr["MENU"].ToString()))
            {
              return "Favor informar o Menu.";
            }

            if (dr["MENU"] != null && !string.IsNullOrEmpty(dr["MENU"].ToString()))
            {
              if (dr["MENU"].ToString().Length > 20)
              {
                return "O Menu deve possuir no máximo 20 caracteres.";
              }
            }

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

            if (dr["DESCRICAO"] != null && !string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              if (dr["DESCRICAO"].ToString().Length > 200)
              {
                return "O Descricao deve possuir no máximo 200 caracteres.";
              }
            }

            if (dr["TIPO"] != null && !string.IsNullOrEmpty(dr["TIPO"].ToString()))
            {
              if (dr["TIPO"].ToString().Length > 10)
              {
                return "O Tipo deve possuir no máximo 10 caracteres.";
              }

              if (dr["TIPO"] != null && !string.IsNullOrEmpty(dr["TIPO"].ToString()))
              {
                List<object> lstTIPO = new List<object>(new object[]{"Web", "Windows"});
                if(!lstTIPO.Contains(dr["TIPO"].ToString()))
                {
                  return "Os valores permitidos para o Tipo são ''Web'' ou ''Windows''.";
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

    public static Hd_menu Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_menu Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_menu tab = new Hd_menu();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_menu tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @menu, string @sis, string @descricao, string @tipo
      ) {
        return (Row)Add(new object[] {
          @menu, @sis, @descricao, @tipo
        });
      }

      public Row Find(
        string @menu
      ) {
        return (Row)Find(new object[] {
          @menu
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
      /// MENU: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Menu { get { return (string)this["menu"]; } set { this["menu"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SIS: VARCHAR(12), Not Null
      /// </summary>
      public string Sis { get { return (string)this["sis"]; } set { this["sis"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO: VARCHAR(200)
      /// </summary>
      public string Descricao { get { return (string)this["descricao"]; } set { this["descricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TIPO: VARCHAR(10)
      /// </summary>
      public string Tipo { get { return (string)this["tipo"]; } set { this["tipo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @menu, string @sis) {
        return Insert(connection,
                      @menu, @sis,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @menu, string @sis,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_menu(),
          new string[] { "menu", "sis" },
          new object[] { @menu, @sis },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkMenu,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_menu(),
          new string[] { "menu" },
          new object[] { pkMenu },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkMenu) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_menu(),
          new string[] { "menu" },
          new object[] { pkMenu }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkMenu) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_menu(),
          new string[] { "menu" },
          new object[] { pkMenu }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Menu { get { return (string)row["menu", DataRowVersion.Original]; } }
        public string Sis { get { return (string)row["sis", DataRowVersion.Original]; } }
        public string Descricao { get { return (string)row["descricao", DataRowVersion.Original]; } }
        public string Tipo { get { return (string)row["tipo", DataRowVersion.Original]; } }

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
    #region Coluna: MENU
    private ColumnMenu pvColMenu;

    public sealed class ColumnMenu : TDataColumn {

      public ColumnMenu() {
        this.ColumnName = "menu";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_menu.menu"; } }
      public override string OriginalCol { get { return "menu"; } }
      public override string RefCol { get { return "HD_MENU.MENU"; } }
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


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_menu.sis"; } }
      public override string OriginalCol { get { return "sis"; } }
      public override string RefCol { get { return "HD_MENU.SIS"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 12; } }
      public override string SqlServerType { get { return "VARCHAR(12)"; } }

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
      public override string FullCol { get { return "hd_menu.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "HD_MENU.DESCRICAO"; } }
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
      public override string FullCol { get { return "hd_menu.tipo"; } }
      public override string OriginalCol { get { return "tipo"; } }
      public override string RefCol { get { return "HD_MENU.TIPO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 10; } }
      public override string SqlServerType { get { return "VARCHAR(10)"; } }

    }
    #endregion
    #endregion
  }

  public partial class Hd_menuitem : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_menuitem() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_menuitem");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_menuitem");
      }
      catch{}
    }

    public Hd_menuitem() {
      pvColMenu = new ColumnMenu(); Columns.Add(pvColMenu);
      pvColMenuitem = new ColumnMenuitem(); Columns.Add(pvColMenuitem);
      pvColMenuitempai = new ColumnMenuitempai(); Columns.Add(pvColMenuitempai);
      pvColSis = new ColumnSis(); Columns.Add(pvColSis);
      pvColTrans = new ColumnTrans(); Columns.Add(pvColTrans);
      pvColOrdem = new ColumnOrdem(); Columns.Add(pvColOrdem);
      pvColTexto = new ColumnTexto(); Columns.Add(pvColTexto);
      pvColImagem = new ColumnImagem(); Columns.Add(pvColImagem);
      pvColTipo = new ColumnTipo(); Columns.Add(pvColTipo);
      pvColSubtipo = new ColumnSubtipo(); Columns.Add(pvColSubtipo);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_menuitem");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @menu, decimal? @menuitem, decimal? @ordem) {
      Row row = NewRow();

      row.Menu = @menu;
      row.Menuitem = @menuitem;
      row.Ordem = @ordem;

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
      get { return "HD_MENUITEM"; }
    }

    protected override sealed string Name { get { return "Itens dos menus"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColMenu, pvColMenuitem });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_menuitem_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_menuitem_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_menuitem_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_menuitem_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkMenu", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "MENU", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkMenuitem", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "MENUITEM", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("menu", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "MENU", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("menuitem", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "MENUITEM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("menuitempai", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "MENUITEMPAI", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("sis", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SIS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("trans", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TRANS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("ordem", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ORDEM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("texto", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TEXTO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("imagem", OleDbType.VarChar, 356, ParameterDirection.InputOutput, true, 0, 0, "IMAGEM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("tipo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TIPO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("subtipo", OleDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "SUBTIPO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "menu") { result.Add(toLower ? "menu" : column); continue; }
          if(lower == "menuitem") { result.Add(toLower ? "menuitem" : column); continue; }
          if(lower == "menuitempai") { result.Add(toLower ? "menuitempai" : column); continue; }
          if(lower == "sis") { result.Add(toLower ? "sis" : column); continue; }
          if(lower == "trans") { result.Add(toLower ? "trans" : column); continue; }
          if(lower == "ordem") { result.Add(toLower ? "ordem" : column); continue; }
          if(lower == "texto") { result.Add(toLower ? "texto" : column); continue; }
          if(lower == "imagem") { result.Add(toLower ? "imagem" : column); continue; }
          if(lower == "tipo") { result.Add(toLower ? "tipo" : column); continue; }
          if(lower == "subtipo") { result.Add(toLower ? "subtipo" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "menu", "menuitem", "menuitempai", "sis", "trans", "ordem", "texto", 
                                           "imagem", "tipo", "subtipo" }
                          : new string[] { "MENU", "MENUITEM", "MENUITEMPAI", "SIS", "TRANS", "ORDEM", "TEXTO", 
                                           "IMAGEM", "TIPO", "SUBTIPO" });
        else
          result.AddRange(toLower
                          ? new string[] { "menu", "menuitem", "menuitempai", "sis", "trans", "ordem", "texto", 
                                           "imagem", "tipo", "subtipo" }
                          : new string[] { "MENU", "MENUITEM", "MENUITEMPAI", "SIS", "TRANS", "ORDEM", "TEXTO", 
                                           "IMAGEM", "TIPO", "SUBTIPO" });

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
        "hd_menuitem.menu", "hd_menuitem.menuitem"
      });
      if(scColumns.Contains("menuitempai")) list.Add("hd_menuitem.menuitempai");
      if(scColumns.Contains("sis")) list.Add("hd_menuitem.sis");
      if(scColumns.Contains("trans")) list.Add("hd_menuitem.trans");
      if(scColumns.Contains("ordem")) list.Add("hd_menuitem.ordem");
      if(scColumns.Contains("texto")) list.Add("hd_menuitem.texto");
      if(scColumns.Contains("imagem")) list.Add("hd_menuitem.imagem");
      if(scColumns.Contains("tipo")) list.Add("hd_menuitem.tipo");
      if(scColumns.Contains("subtipo")) list.Add("hd_menuitem.subtipo");
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
          "FROM hd_menuitem " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_menuitem");
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
        da.TableMappings.Add("Table", "Hd_menuitem");
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
            if (dr["MENU"] != null && string.IsNullOrEmpty(dr["MENU"].ToString()))
              dr["MENU"] = DBNull.Value;
            if (dr["MENUITEM"] != null && string.IsNullOrEmpty(dr["MENUITEM"].ToString()))
              dr["MENUITEM"] = DBNull.Value;
            if (dr["MENUITEMPAI"] != null && string.IsNullOrEmpty(dr["MENUITEMPAI"].ToString()))
              dr["MENUITEMPAI"] = DBNull.Value;
            if (dr["SIS"] != null && string.IsNullOrEmpty(dr["SIS"].ToString()))
              dr["SIS"] = DBNull.Value;
            if (dr["TRANS"] != null && string.IsNullOrEmpty(dr["TRANS"].ToString()))
              dr["TRANS"] = DBNull.Value;
            if (dr["ORDEM"] != null && string.IsNullOrEmpty(dr["ORDEM"].ToString()))
              dr["ORDEM"] = DBNull.Value;
            if (dr["TEXTO"] != null && string.IsNullOrEmpty(dr["TEXTO"].ToString()))
              dr["TEXTO"] = DBNull.Value;
            if (dr["IMAGEM"] != null && string.IsNullOrEmpty(dr["IMAGEM"].ToString()))
              dr["IMAGEM"] = DBNull.Value;
            if (dr["TIPO"] != null && string.IsNullOrEmpty(dr["TIPO"].ToString()))
              dr["TIPO"] = DBNull.Value;
            if (dr["SUBTIPO"] != null && string.IsNullOrEmpty(dr["SUBTIPO"].ToString()))
              dr["SUBTIPO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["MENU"] == null || string.IsNullOrEmpty(dr["MENU"].ToString()))
            {
              return "Favor informar o menu.";
            }

            if (dr["MENU"] != null && !string.IsNullOrEmpty(dr["MENU"].ToString()))
            {
              if (dr["MENU"].ToString().Length > 20)
              {
                return "O menu deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["MENUITEM"] == null || string.IsNullOrEmpty(dr["MENUITEM"].ToString()))
            {
              return "Favor informar o item do menu.";
            }

            if (dr["MENUITEM"] != null && !string.IsNullOrEmpty(dr["MENUITEM"].ToString()))
            {
              if ((Convert.ToDecimal(dr["MENUITEM"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["MENUITEM"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["MENUITEM"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["MENUITEM"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O item do menu deve ser um valor inteiro com 10 dígitos no máximo.";
              }
            }

            if (dr["MENUITEMPAI"] != null && !string.IsNullOrEmpty(dr["MENUITEMPAI"].ToString()))
            {
              if ((Convert.ToDecimal(dr["MENUITEMPAI"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["MENUITEMPAI"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["MENUITEMPAI"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["MENUITEMPAI"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O item pai do menu deve ser um valor inteiro com 10 dígitos no máximo.";
              }
            }

            if (dr["SIS"] != null && !string.IsNullOrEmpty(dr["SIS"].ToString()))
            {
              if (dr["SIS"].ToString().Length > 12)
              {
                return "O sistema deve possuir no máximo 12 caracteres.";
              }
            }

            if (dr["TRANS"] != null && !string.IsNullOrEmpty(dr["TRANS"].ToString()))
            {
              if (dr["TRANS"].ToString().Length > 100)
              {
                return "A transação deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["ORDEM"] == null || string.IsNullOrEmpty(dr["ORDEM"].ToString()))
            {
              return "Favor informar a ordem.";
            }

            if (dr["ORDEM"] != null && !string.IsNullOrEmpty(dr["ORDEM"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ORDEM"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ORDEM"]).ToString("0.############").Split(',')[0].Length > 2) || (Convert.ToDecimal(dr["ORDEM"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ORDEM"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A ordem deve ser um valor inteiro com 2 dígitos no máximo.";
              }

              if (dr["ORDEM"] != null && !string.IsNullOrEmpty(dr["ORDEM"].ToString()))
              {
                if (Convert.ToDecimal(dr["ORDEM"]) < (1))
                {
                  return "A ordem deve ser maior ou igual a (1).";
                }
              }
            }

            if (dr["TEXTO"] != null && !string.IsNullOrEmpty(dr["TEXTO"].ToString()))
            {
              if (dr["TEXTO"].ToString().Length > 40)
              {
                return "O texto deve possuir no máximo 40 caracteres.";
              }
            }

            if (dr["IMAGEM"] != null && !string.IsNullOrEmpty(dr["IMAGEM"].ToString()))
            {
              if (dr["IMAGEM"].ToString().Length > 256)
              {
                return "A imagem deve possuir no máximo 256 caracteres.";
              }
            }

            if (dr["TIPO"] != null && !string.IsNullOrEmpty(dr["TIPO"].ToString()))
            {
              if (dr["TIPO"].ToString().Length > 50)
              {
                return "O tipo deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["SUBTIPO"] != null && !string.IsNullOrEmpty(dr["SUBTIPO"].ToString()))
            {
              if (dr["SUBTIPO"].ToString().Length > 200)
              {
                return "O subtipo deve possuir no máximo 200 caracteres.";
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

    public static Hd_menuitem Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_menuitem Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_menuitem tab = new Hd_menuitem();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_menuitem tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @menu, decimal? @menuitem, decimal? @menuitempai, string @sis, string @trans, 
        decimal? @ordem, string @texto, string @imagem, string @tipo, string @subtipo
      ) {
        return (Row)Add(new object[] {
          @menu, @menuitem, @menuitempai, @sis, @trans, @ordem, @texto, @imagem, @tipo, @subtipo
        });
      }

      public Row Find(
        string @menu, decimal? @menuitem
      ) {
        return (Row)Find(new object[] {
          @menu, @menuitem
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
      /// MENU: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Menu { get { return (string)this["menu"]; } set { this["menu"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// MENUITEM: NUMERIC(10), Primary Key, Not Null
      /// </summary>
      public decimal? Menuitem { get { return (decimal?)this["menuitem"]; } set { this["menuitem"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// MENUITEMPAI: NUMERIC(10)
      /// </summary>
      public decimal? Menuitempai { get { return (decimal?)this["menuitempai"]; } set { this["menuitempai"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SIS: VARCHAR(12)
      /// </summary>
      public string Sis { get { return (string)this["sis"]; } set { this["sis"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TRANS: VARCHAR(100)
      /// </summary>
      public string Trans { get { return (string)this["trans"]; } set { this["trans"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ORDEM: NUMERIC(2), Not Null
      /// </summary>
      public decimal? Ordem { get { return (decimal?)this["ordem"]; } set { this["ordem"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TEXTO: VARCHAR(40)
      /// </summary>
      public string Texto { get { return (string)this["texto"]; } set { this["texto"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// IMAGEM: VARCHAR(256)
      /// </summary>
      public string Imagem { get { return (string)this["imagem"]; } set { this["imagem"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TIPO: VARCHAR(50)
      /// </summary>
      public string Tipo { get { return (string)this["tipo"]; } set { this["tipo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SUBTIPO: VARCHAR(200)
      /// </summary>
      public string Subtipo { get { return (string)this["subtipo"]; } set { this["subtipo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @menu, decimal? @menuitem, decimal? @ordem) {
        return Insert(connection,
                      @menu, @menuitem, @ordem,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @menu, decimal? @menuitem, decimal? @ordem,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_menuitem(),
          new string[] { "menu", "menuitem", "ordem" },
          new object[] { @menu, @menuitem, @ordem },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkMenu, decimal? pkMenuitem,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_menuitem(),
          new string[] { "menu", "menuitem" },
          new object[] { pkMenu, pkMenuitem },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkMenu, decimal? pkMenuitem) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_menuitem(),
          new string[] { "menu", "menuitem" },
          new object[] { pkMenu, pkMenuitem }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkMenu, decimal? pkMenuitem) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_menuitem(),
          new string[] { "menu", "menuitem" },
          new object[] { pkMenu, pkMenuitem }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Menu { get { return (string)row["menu", DataRowVersion.Original]; } }
        public decimal? Menuitem { get { return (decimal?)row["menuitem", DataRowVersion.Original]; } }
        public decimal? Menuitempai { get { return (decimal?)row["menuitempai", DataRowVersion.Original]; } }
        public string Sis { get { return (string)row["sis", DataRowVersion.Original]; } }
        public string Trans { get { return (string)row["trans", DataRowVersion.Original]; } }
        public decimal? Ordem { get { return (decimal?)row["ordem", DataRowVersion.Original]; } }
        public string Texto { get { return (string)row["texto", DataRowVersion.Original]; } }
        public string Imagem { get { return (string)row["imagem", DataRowVersion.Original]; } }
        public string Tipo { get { return (string)row["tipo", DataRowVersion.Original]; } }
        public string Subtipo { get { return (string)row["subtipo", DataRowVersion.Original]; } }

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
    #region Coluna: MENU
    private ColumnMenu pvColMenu;

    public sealed class ColumnMenu : TDataColumn {

      public ColumnMenu() {
        this.ColumnName = "menu";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "menu";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_menuitem.menu"; } }
      public override string OriginalCol { get { return "menu"; } }
      public override string RefCol { get { return "HD_MENUITEM.MENU"; } }
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

    #region Coluna: MENUITEM
    private ColumnMenuitem pvColMenuitem;

    public sealed class ColumnMenuitem : TDataColumn {

      public ColumnMenuitem() {
        this.ColumnName = "menuitem";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "item do menu";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_menuitem.menuitem"; } }
      public override string OriginalCol { get { return "menuitem"; } }
      public override string RefCol { get { return "HD_MENUITEM.MENUITEM"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 11; } }
      public override int Precision { get { return 10; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(10)"; } }

    }
    #endregion

    #region Coluna: MENUITEMPAI
    private ColumnMenuitempai pvColMenuitempai;

    public sealed class ColumnMenuitempai : TDataColumn {

      public ColumnMenuitempai() {
        this.ColumnName = "menuitempai";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "item pai do menu";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_menuitem.menuitempai"; } }
      public override string OriginalCol { get { return "menuitempai"; } }
      public override string RefCol { get { return "HD_MENUITEM.MENUITEMPAI"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 11; } }
      public override int Precision { get { return 10; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(10)"; } }

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
      public override string FullCol { get { return "hd_menuitem.sis"; } }
      public override string OriginalCol { get { return "sis"; } }
      public override string RefCol { get { return "HD_MENUITEM.SIS"; } }
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
      public override string FullCol { get { return "hd_menuitem.trans"; } }
      public override string OriginalCol { get { return "trans"; } }
      public override string RefCol { get { return "HD_MENUITEM.TRANS"; } }
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

    #region Coluna: ORDEM
    private ColumnOrdem pvColOrdem;

    public sealed class ColumnOrdem : TDataColumn {

      public ColumnOrdem() {
        this.ColumnName = "ordem";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "ordem";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_menuitem.ordem"; } }
      public override string OriginalCol { get { return "ordem"; } }
      public override string RefCol { get { return "HD_MENUITEM.ORDEM"; } }
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

    #region Coluna: TEXTO
    private ColumnTexto pvColTexto;

    public sealed class ColumnTexto : TDataColumn {

      public ColumnTexto() {
        this.ColumnName = "texto";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "texto";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_menuitem.texto"; } }
      public override string OriginalCol { get { return "texto"; } }
      public override string RefCol { get { return "HD_MENUITEM.TEXTO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 40; } }
      public override string SqlServerType { get { return "VARCHAR(40)"; } }

    }
    #endregion

    #region Coluna: IMAGEM
    private ColumnImagem pvColImagem;

    public sealed class ColumnImagem : TDataColumn {

      public ColumnImagem() {
        this.ColumnName = "imagem";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "imagem";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_menuitem.imagem"; } }
      public override string OriginalCol { get { return "imagem"; } }
      public override string RefCol { get { return "HD_MENUITEM.IMAGEM"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 256; } }
      public override string SqlServerType { get { return "VARCHAR(256)"; } }

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
      public override string FullCol { get { return "hd_menuitem.tipo"; } }
      public override string OriginalCol { get { return "tipo"; } }
      public override string RefCol { get { return "HD_MENUITEM.TIPO"; } }
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

    #region Coluna: SUBTIPO
    private ColumnSubtipo pvColSubtipo;

    public sealed class ColumnSubtipo : TDataColumn {

      public ColumnSubtipo() {
        this.ColumnName = "subtipo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "subtipo";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_menuitem.subtipo"; } }
      public override string OriginalCol { get { return "subtipo"; } }
      public override string RefCol { get { return "HD_MENUITEM.SUBTIPO"; } }
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
}

