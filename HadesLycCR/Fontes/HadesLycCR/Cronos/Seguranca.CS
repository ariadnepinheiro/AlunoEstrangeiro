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
  public class Seguranca : TDataSet {
    private const string DataSetName_Def = "Seguranca";
    private static ConstructorInfo ctorTCustom = null;
    private CustomBase tcustom = null;
    #region Variáveis private para as tabelas deste dataset
    private Hd_padaces pvTabHd_padaces;
    private Hd_padtrans pvTabHd_padtrans;
    private Hd_padusuario pvTabHd_padusuario;
    private Hd_transacao pvTabHd_transacao;
    private Hd_usuario pvTabHd_usuario;
    #endregion

    static Seguranca() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Seguranca");
      }
      catch{}
    }

    public Seguranca() : base() {
      DataSetName = DataSetName_Def;

      pvTabHd_padaces = new Hd_padaces();
      pvTabHd_padtrans = new Hd_padtrans();
      pvTabHd_padusuario = new Hd_padusuario();
      pvTabHd_transacao = new Hd_transacao();
      pvTabHd_usuario = new Hd_usuario();

      Tables.Add(pvTabHd_padaces);
      Tables.Add(pvTabHd_padtrans);
      Tables.Add(pvTabHd_padusuario);
      Tables.Add(pvTabHd_transacao);
      Tables.Add(pvTabHd_usuario);

      CreateRelations();

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
    }


    [DefaultValue(DataSetName_Def)]
    public new string DataSetName {
      get { return base.DataSetName; }
      set { base.DataSetName = value == null ? string.Empty : value; }
    }

    private void CreateRelations() {
      Relations.Add("fk_padusuario_padaces",
                    new DataColumn[] { pvTabHd_padaces.Columns["padaces"] },
                    new DataColumn[] { pvTabHd_padusuario.Columns["padaces"] }, false);
      Relations.Add("fk_padusuario_usuario",
                    new DataColumn[] { pvTabHd_usuario.Columns["usuario"] },
                    new DataColumn[] { pvTabHd_padusuario.Columns["usuario"] }, false);
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
          pvTabHd_padaces.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_padaces); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabHd_padtrans.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_padtrans); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabHd_padusuario.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_padusuario); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabHd_transacao.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_transacao); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabHd_usuario.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_usuario); if(row != null) return DataLib.RowErrorsToString(row);
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
    public Hd_padaces Hd_padaces { get { return pvTabHd_padaces; } }

    [
      Browsable(false),
    ]
    public Hd_padtrans Hd_padtrans { get { return pvTabHd_padtrans; } }

    [
      Browsable(false),
    ]
    public Hd_padusuario Hd_padusuario { get { return pvTabHd_padusuario; } }

    [
      Browsable(false),
    ]
    public Hd_transacao Hd_transacao { get { return pvTabHd_transacao; } }

    [
      Browsable(false),
    ]
    public Hd_usuario Hd_usuario { get { return pvTabHd_usuario; } }

    #endregion

    public abstract class CustomBase {
      public abstract string Put(Techne.HadesLyc.CR.Seguranca ds, TConnectionWritable connection);
    }

  }

  public partial class Hd_padaces : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_padaces() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_padaces");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_padaces");
      }
      catch{}
    }

    public Hd_padaces() {
      pvColPadaces = new ColumnPadaces(); Columns.Add(pvColPadaces);
      pvColNome = new ColumnNome(); Columns.Add(pvColNome);
      pvColPagina_inicial = new ColumnPagina_inicial(); Columns.Add(pvColPagina_inicial);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_padaces");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @padaces) {
      Row row = NewRow();

      row.Padaces = @padaces;

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
      get { return "HD_PADACES"; }
    }

    protected override sealed string Name { get { return "Padrões de acesso"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColPadaces });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_padaces_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_padaces_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_padaces_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_padaces_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkPadaces", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "PADACES", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("padaces", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PADACES", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("nome", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NOME", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("pagina_inicial", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PAGINA_INICIAL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "nome") { result.Add(toLower ? "nome" : column); continue; }
          if(lower == "pagina_inicial") { result.Add(toLower ? "pagina_inicial" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "padaces", "nome", "pagina_inicial" }
                          : new string[] { "PADACES", "NOME", "PAGINA_INICIAL" });
        else
          result.AddRange(toLower
                          ? new string[] { "padaces", "nome", "pagina_inicial" }
                          : new string[] { "PADACES", "NOME", "PAGINA_INICIAL" });

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
        "hd_padaces.padaces"
      });
      if(scColumns.Contains("nome")) list.Add("hd_padaces.nome");
      if(scColumns.Contains("pagina_inicial")) list.Add("hd_padaces.pagina_inicial");
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
          "FROM hd_padaces " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_padaces");
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
        da.TableMappings.Add("Table", "Hd_padaces");
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
            if (dr["NOME"] != null && string.IsNullOrEmpty(dr["NOME"].ToString()))
              dr["NOME"] = DBNull.Value;
            if (dr["PAGINA_INICIAL"] != null && string.IsNullOrEmpty(dr["PAGINA_INICIAL"].ToString()))
              dr["PAGINA_INICIAL"] = DBNull.Value;
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

            if (dr["NOME"] != null && !string.IsNullOrEmpty(dr["NOME"].ToString()))
            {
              if (dr["NOME"].ToString().Length > 50)
              {
                return "O nome deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["PAGINA_INICIAL"] != null && !string.IsNullOrEmpty(dr["PAGINA_INICIAL"].ToString()))
            {
              if (dr["PAGINA_INICIAL"].ToString().Length > 100)
              {
                return "A página inicial deve possuir no máximo 100 caracteres.";
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

    public static Hd_padaces Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_padaces Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_padaces tab = new Hd_padaces();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_padaces tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @padaces, string @nome, string @pagina_inicial
      ) {
        return (Row)Add(new object[] {
          @padaces, @nome, @pagina_inicial
        });
      }

      public Row Find(
        string @padaces
      ) {
        return (Row)Find(new object[] {
          @padaces
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
      /// NOME: VARCHAR(50)
      /// </summary>
      public string Nome { get { return (string)this["nome"]; } set { this["nome"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PAGINA_INICIAL: VARCHAR(100)
      /// </summary>
      public string Pagina_inicial { get { return (string)this["pagina_inicial"]; } set { this["pagina_inicial"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @padaces) {
        return Insert(connection,
                      @padaces,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @padaces,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_padaces(),
          new string[] { "padaces" },
          new object[] { @padaces },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkPadaces,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_padaces(),
          new string[] { "padaces" },
          new object[] { pkPadaces },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkPadaces) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_padaces(),
          new string[] { "padaces" },
          new object[] { pkPadaces }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkPadaces) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_padaces(),
          new string[] { "padaces" },
          new object[] { pkPadaces }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Padaces { get { return (string)row["padaces", DataRowVersion.Original]; } }
        public string Nome { get { return (string)row["nome", DataRowVersion.Original]; } }
        public string Pagina_inicial { get { return (string)row["pagina_inicial", DataRowVersion.Original]; } }

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
      public override string FullCol { get { return "hd_padaces.padaces"; } }
      public override string OriginalCol { get { return "padaces"; } }
      public override string RefCol { get { return "HD_PADACES.PADACES"; } }
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
      public override string FullCol { get { return "hd_padaces.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "HD_PADACES.NOME"; } }
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

    #region Coluna: PAGINA_INICIAL
    private ColumnPagina_inicial pvColPagina_inicial;

    public sealed class ColumnPagina_inicial : TDataColumn {

      public ColumnPagina_inicial() {
        this.ColumnName = "pagina_inicial";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "página inicial";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_padaces.pagina_inicial"; } }
      public override string OriginalCol { get { return "pagina_inicial"; } }
      public override string RefCol { get { return "HD_PADACES.PAGINA_INICIAL"; } }
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

  public partial class Hd_padusuario : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_padusuario() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_padusuario");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_padusuario");
      }
      catch{}
    }

    public Hd_padusuario() {
      pvColPadaces = new ColumnPadaces(); Columns.Add(pvColPadaces);
      pvColUsuario = new ColumnUsuario(); Columns.Add(pvColUsuario);
      pvColNome02 = new ColumnNome02(); Columns.Add(pvColNome02);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_padusuario");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @padaces, string @usuario) {
      Row row = NewRow();

      row.Padaces = @padaces;
      row.Usuario = @usuario;

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
      get { return "HD_PADUSUARIO"; }
    }

    protected override sealed string Name { get { return "Padrão de acesso e seus usuários"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColPadaces, pvColUsuario });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_padusuario_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_padusuario_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_padusuario_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_padusuario_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkPadaces", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "PADACES", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkUsuario", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "USUARIO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("padaces", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PADACES", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("usuario", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "USUARIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "usuario") { result.Add(toLower ? "usuario" : column); continue; }
          if(!tabPrincipal) {
            if(lower == "nome02") { result.Add(toLower ? "nome02" : column); continue; }
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "padaces", "usuario" }
                          : new string[] { "PADACES", "USUARIO" });
        else
          result.AddRange(toLower
                          ? new string[] { "padaces", "usuario", "nome02" }
                          : new string[] { "PADACES", "USUARIO", "NOME02" });

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
        "hd_padusuario.padaces", "hd_padusuario.usuario"
      });
      if(scColumns.Contains("nome02")) list.Add("hd_usuario.nome nome02");
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
          "FROM hd_padusuario " +
               "INNER JOIN hd_usuario " +
                 "ON hd_usuario.usuario = hd_padusuario.usuario " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_padusuario");
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
        da.TableMappings.Add("Table", "Hd_padusuario");
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
            if (dr["USUARIO"] != null && string.IsNullOrEmpty(dr["USUARIO"].ToString()))
              dr["USUARIO"] = DBNull.Value;
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

            if (dr["USUARIO"] == null || string.IsNullOrEmpty(dr["USUARIO"].ToString()))
            {
              return "Favor informar o usuário.";
            }

            if (dr["USUARIO"] != null && !string.IsNullOrEmpty(dr["USUARIO"].ToString()))
            {
              if (dr["USUARIO"].ToString().Length > 15)
              {
                return "O usuário deve possuir no máximo 15 caracteres.";
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

    public static Hd_padusuario Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_padusuario Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_padusuario tab = new Hd_padusuario();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_padusuario tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @padaces, string @usuario
      ) {
        return (Row)Add(new object[] {
          @padaces, @usuario
        });
      }

      public Row Find(
        string @padaces, string @usuario
      ) {
        return (Row)Find(new object[] {
          @padaces, @usuario
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
      /// USUARIO: VARCHAR(15), Primary Key, Not Null
      /// </summary>
      public string Usuario { get { return (string)this["usuario"]; } set { this["usuario"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(50), Not Null
      /// </summary>
      public string Nome02 { get { return (string)this["nome02"]; } set { this["nome02"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @padaces, string @usuario) {
        return Insert(connection,
                      @padaces, @usuario,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @padaces, string @usuario,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_padusuario(),
          new string[] { "padaces", "usuario" },
          new object[] { @padaces, @usuario },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkPadaces, string pkUsuario,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_padusuario(),
          new string[] { "padaces", "usuario" },
          new object[] { pkPadaces, pkUsuario },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkPadaces, string pkUsuario) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_padusuario(),
          new string[] { "padaces", "usuario" },
          new object[] { pkPadaces, pkUsuario }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkPadaces, string pkUsuario) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_padusuario(),
          new string[] { "padaces", "usuario" },
          new object[] { pkPadaces, pkUsuario }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Padaces { get { return (string)row["padaces", DataRowVersion.Original]; } }
        public string Usuario { get { return (string)row["usuario", DataRowVersion.Original]; } }
        public string Nome02 { get { return (string)row["nome02", DataRowVersion.Original]; } }

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
      public override string FullCol { get { return "hd_padusuario.padaces"; } }
      public override string OriginalCol { get { return "padaces"; } }
      public override string RefCol { get { return "HD_PADUSUARIO.PADACES"; } }
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

    #region Coluna: USUARIO
    private ColumnUsuario pvColUsuario;

    public sealed class ColumnUsuario : TDataColumn {

      public ColumnUsuario() {
        this.ColumnName = "usuario";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "usuário";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_padusuario.usuario"; } }
      public override string OriginalCol { get { return "usuario"; } }
      public override string RefCol { get { return "HD_PADUSUARIO.USUARIO"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 15; } }
      public override string SqlServerType { get { return "VARCHAR(15)"; } }

    }
    #endregion

    #region Coluna: NOME02
    private ColumnNome02 pvColNome02;

    public sealed class ColumnNome02 : TDataColumn {

      public ColumnNome02() {
        this.ColumnName = "nome02";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "nome";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_usuario.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "HD_USUARIO.NOME"; } }
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
    #endregion
  }

  public partial class Hd_usuario : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_usuario() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_usuario");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_usuario");
      }
      catch{}
    }

    public Hd_usuario() {
      pvColUsuario = new ColumnUsuario(); Columns.Add(pvColUsuario);
      pvColSis = new ColumnSis(); Columns.Add(pvColSis);
      pvColNome = new ColumnNome(); Columns.Add(pvColNome);
      pvColGrupousu = new ColumnGrupousu(); Columns.Add(pvColGrupousu);
      pvColSenha = new ColumnSenha(); Columns.Add(pvColSenha);
      pvColPrivilegiado = new ColumnPrivilegiado(); Columns.Add(pvColPrivilegiado);
      pvColEmail = new ColumnEmail(); Columns.Add(pvColEmail);
      pvColMatricula = new ColumnMatricula(); Columns.Add(pvColMatricula);
      pvColSetor = new ColumnSetor(); Columns.Add(pvColSetor);
      pvColHabilitado = new ColumnHabilitado(); Columns.Add(pvColHabilitado);
      pvColMotivo_desabilitado = new ColumnMotivo_desabilitado(); Columns.Add(pvColMotivo_desabilitado);
      pvColData_alteracao_senha = new ColumnData_alteracao_senha(); Columns.Add(pvColData_alteracao_senha);
      pvColAlterar_senha = new ColumnAlterar_senha(); Columns.Add(pvColAlterar_senha);
      pvColSenhas_anteriores = new ColumnSenhas_anteriores(); Columns.Add(pvColSenhas_anteriores);
      pvColSenha_temp = new ColumnSenha_temp(); Columns.Add(pvColSenha_temp);
      pvColUltimo_login = new ColumnUltimo_login(); Columns.Add(pvColUltimo_login);
      pvColTentativas_incorretas = new ColumnTentativas_incorretas(); Columns.Add(pvColTentativas_incorretas);
      pvColWinusuario = new ColumnWinusuario(); Columns.Add(pvColWinusuario);
      pvColPessoa = new ColumnPessoa(); Columns.Add(pvColPessoa);
      pvColPriv_unidade_ens = new ColumnPriv_unidade_ens(); Columns.Add(pvColPriv_unidade_ens);
      pvColMat_cadastro = new ColumnMat_cadastro(); Columns.Add(pvColMat_cadastro);
      pvColDt_cadastro = new ColumnDt_cadastro(); Columns.Add(pvColDt_cadastro);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_usuario");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @usuario, string @nome, string @privilegiado, string @habilitado) {
      Row row = NewRow();

      row.Usuario = @usuario;
      row.Nome = @nome;
      row.Privilegiado = @privilegiado;
      row.Habilitado = @habilitado;

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
      get { return "HD_USUARIO"; }
    }

    protected override sealed string Name { get { return "Usuário"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColUsuario });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_usuario_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_usuario_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_usuario_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_usuario_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkUsuario", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "USUARIO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("usuario", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "USUARIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("sis", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SIS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("nome", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NOME", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("grupousu", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "GRUPOUSU", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("senha", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SENHA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("privilegiado", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PRIVILEGIADO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("email", OleDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "EMAIL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("matricula", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "MATRICULA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("setor", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SETOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("habilitado", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "HABILITADO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("motivo_desabilitado", OleDbType.VarChar, 2100, ParameterDirection.InputOutput, true, 0, 0, "MOTIVO_DESABILITADO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("data_alteracao_senha", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DATA_ALTERACAO_SENHA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("alterar_senha", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "ALTERAR_SENHA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("senhas_anteriores", OleDbType.VarChar, 1600, ParameterDirection.InputOutput, true, 0, 0, "SENHAS_ANTERIORES", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("senha_temp", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SENHA_TEMP", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("ultimo_login", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "ULTIMO_LOGIN", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("tentativas_incorretas", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "TENTATIVAS_INCORRETAS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("winusuario", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "WINUSUARIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("pessoa", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 22, 10, "PESSOA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("priv_unidade_ens", OleDbType.Char, 1, ParameterDirection.InputOutput, true, 0, 0, "PRIV_UNIDADE_ENS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("mat_cadastro", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "MAT_CADASTRO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("dt_cadastro", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_CADASTRO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      
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
          if(lower == "usuario") { result.Add(toLower ? "usuario" : column); continue; }
          if(lower == "sis") { result.Add(toLower ? "sis" : column); continue; }
          if(lower == "nome") { result.Add(toLower ? "nome" : column); continue; }
          if(lower == "grupousu") { result.Add(toLower ? "grupousu" : column); continue; }
          if(lower == "senha") { result.Add(toLower ? "senha" : column); continue; }
          if(lower == "privilegiado") { result.Add(toLower ? "privilegiado" : column); continue; }
          if(lower == "email") { result.Add(toLower ? "email" : column); continue; }
          if(lower == "matricula") { result.Add(toLower ? "matricula" : column); continue; }
          if(lower == "setor") { result.Add(toLower ? "setor" : column); continue; }
          if(lower == "habilitado") { result.Add(toLower ? "habilitado" : column); continue; }
          if(lower == "motivo_desabilitado") { result.Add(toLower ? "motivo_desabilitado" : column); continue; }
          if(lower == "data_alteracao_senha") { result.Add(toLower ? "data_alteracao_senha" : column); continue; }
          if(lower == "alterar_senha") { result.Add(toLower ? "alterar_senha" : column); continue; }
          if(lower == "senhas_anteriores") { result.Add(toLower ? "senhas_anteriores" : column); continue; }
          if(lower == "senha_temp") { result.Add(toLower ? "senha_temp" : column); continue; }
          if(lower == "ultimo_login") { result.Add(toLower ? "ultimo_login" : column); continue; }
          if(lower == "tentativas_incorretas") { result.Add(toLower ? "tentativas_incorretas" : column); continue; }
          if(lower == "winusuario") { result.Add(toLower ? "winusuario" : column); continue; }
          if(lower == "pessoa") { result.Add(toLower ? "pessoa" : column); continue; }
          if(lower == "priv_unidade_ens") { result.Add(toLower ? "priv_unidade_ens" : column); continue; }
          if(lower == "mat_cadastro") { result.Add(toLower ? "mat_cadastro" : column); continue; }
          if(lower == "dt_cadastro") { result.Add(toLower ? "dt_cadastro" : column); continue; }
            
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "usuario", "sis", "nome", "grupousu", "senha", "privilegiado", "email", 
                                           "matricula", "setor", "habilitado", "motivo_desabilitado", "data_alteracao_senha", 
                                           "alterar_senha", "senhas_anteriores", "senha_temp", "ultimo_login", "tentativas_incorretas", 
                                           "winusuario", "pessoa", "priv_unidade_ens", "mat_cadastro", "dt_cadastro" }
                          : new string[] { "USUARIO", "SIS", "NOME", "GRUPOUSU", "SENHA", "PRIVILEGIADO", "EMAIL", 
                                           "MATRICULA", "SETOR", "HABILITADO", "MOTIVO_DESABILITADO", "DATA_ALTERACAO_SENHA", 
                                           "ALTERAR_SENHA", "SENHAS_ANTERIORES", "SENHA_TEMP", "ULTIMO_LOGIN", "TENTATIVAS_INCORRETAS", 
                                           "WINUSUARIO", "PESSOA", "PRIV_UNIDADE_ENS", "MAT_CADASTRO", "DT_CADASTRO" });
        else
          result.AddRange(toLower
                          ? new string[] { "usuario", "sis", "nome", "grupousu", "senha", "privilegiado", "email", 
                                           "matricula", "setor", "habilitado", "motivo_desabilitado", "data_alteracao_senha", 
                                           "alterar_senha", "senhas_anteriores", "senha_temp", "ultimo_login", "tentativas_incorretas", 
                                           "winusuario", "pessoa", "priv_unidade_ens", "mat_cadastro", "dt_cadastro" }
                          : new string[] { "USUARIO", "SIS", "NOME", "GRUPOUSU", "SENHA", "PRIVILEGIADO", "EMAIL", 
                                           "MATRICULA", "SETOR", "HABILITADO", "MOTIVO_DESABILITADO", "DATA_ALTERACAO_SENHA", 
                                           "ALTERAR_SENHA", "SENHAS_ANTERIORES", "SENHA_TEMP", "ULTIMO_LOGIN", "TENTATIVAS_INCORRETAS", 
                                           "WINUSUARIO", "PESSOA", "PRIV_UNIDADE_ENS", "MAT_CADASTRO", "DT_CADASTRO" });

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
        "hd_usuario.usuario"
      });
      if(scColumns.Contains("sis")) list.Add("hd_usuario.sis");
      if(scColumns.Contains("nome")) list.Add("hd_usuario.nome");
      if(scColumns.Contains("grupousu")) list.Add("hd_usuario.grupousu");
      if(scColumns.Contains("senha")) list.Add("hd_usuario.senha");
      if(scColumns.Contains("privilegiado")) list.Add("hd_usuario.privilegiado");
      if(scColumns.Contains("email")) list.Add("hd_usuario.email");
      if(scColumns.Contains("matricula")) list.Add("hd_usuario.matricula");
      if(scColumns.Contains("setor")) list.Add("hd_usuario.setor");
      if(scColumns.Contains("habilitado")) list.Add("hd_usuario.habilitado");
      if(scColumns.Contains("motivo_desabilitado")) list.Add("hd_usuario.motivo_desabilitado");
      if(scColumns.Contains("data_alteracao_senha")) list.Add("hd_usuario.data_alteracao_senha");
      if(scColumns.Contains("alterar_senha")) list.Add("hd_usuario.alterar_senha");
      if(scColumns.Contains("senhas_anteriores")) list.Add("hd_usuario.senhas_anteriores");
      if(scColumns.Contains("senha_temp")) list.Add("hd_usuario.senha_temp");
      if(scColumns.Contains("ultimo_login")) list.Add("hd_usuario.ultimo_login");
      if(scColumns.Contains("tentativas_incorretas")) list.Add("hd_usuario.tentativas_incorretas");
      if(scColumns.Contains("winusuario")) list.Add("hd_usuario.winusuario");
      if(scColumns.Contains("pessoa")) list.Add("hd_usuario.pessoa");
      if(scColumns.Contains("priv_unidade_ens")) list.Add("hd_usuario.priv_unidade_ens");
      if(scColumns.Contains("mat_cadastro")) list.Add("hd_usuario.mat_cadastro");
      if(scColumns.Contains("dt_cadastro")) list.Add("hd_usuario.dt_cadastro");
        
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
          "FROM hd_usuario " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_usuario");
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
        da.TableMappings.Add("Table", "Hd_usuario");
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
            if (dr["USUARIO"] != null && string.IsNullOrEmpty(dr["USUARIO"].ToString()))
              dr["USUARIO"] = DBNull.Value;
            if (dr["SIS"] != null && string.IsNullOrEmpty(dr["SIS"].ToString()))
              dr["SIS"] = DBNull.Value;
            if (dr["NOME"] != null && string.IsNullOrEmpty(dr["NOME"].ToString()))
              dr["NOME"] = DBNull.Value;
            if (dr["GRUPOUSU"] != null && string.IsNullOrEmpty(dr["GRUPOUSU"].ToString()))
              dr["GRUPOUSU"] = DBNull.Value;
            if (dr["SENHA"] != null && string.IsNullOrEmpty(dr["SENHA"].ToString()))
              dr["SENHA"] = DBNull.Value;
            if (dr["PRIVILEGIADO"] != null && string.IsNullOrEmpty(dr["PRIVILEGIADO"].ToString()))
              dr["PRIVILEGIADO"] = DBNull.Value;
            if (dr["EMAIL"] != null && string.IsNullOrEmpty(dr["EMAIL"].ToString()))
              dr["EMAIL"] = DBNull.Value;
            if (dr["MATRICULA"] != null && string.IsNullOrEmpty(dr["MATRICULA"].ToString()))
              dr["MATRICULA"] = DBNull.Value;
            if (dr["SETOR"] != null && string.IsNullOrEmpty(dr["SETOR"].ToString()))
              dr["SETOR"] = DBNull.Value;
            if (dr["HABILITADO"] != null && string.IsNullOrEmpty(dr["HABILITADO"].ToString()))
              dr["HABILITADO"] = DBNull.Value;
            if (dr["MOTIVO_DESABILITADO"] != null && string.IsNullOrEmpty(dr["MOTIVO_DESABILITADO"].ToString()))
              dr["MOTIVO_DESABILITADO"] = DBNull.Value;
            if (dr["ALTERAR_SENHA"] != null && string.IsNullOrEmpty(dr["ALTERAR_SENHA"].ToString()))
              dr["ALTERAR_SENHA"] = DBNull.Value;
            if (dr["SENHAS_ANTERIORES"] != null && string.IsNullOrEmpty(dr["SENHAS_ANTERIORES"].ToString()))
              dr["SENHAS_ANTERIORES"] = DBNull.Value;
            if (dr["SENHA_TEMP"] != null && string.IsNullOrEmpty(dr["SENHA_TEMP"].ToString()))
              dr["SENHA_TEMP"] = DBNull.Value;
            if (dr["TENTATIVAS_INCORRETAS"] != null && string.IsNullOrEmpty(dr["TENTATIVAS_INCORRETAS"].ToString()))
              dr["TENTATIVAS_INCORRETAS"] = DBNull.Value;
            if (dr["WINUSUARIO"] != null && string.IsNullOrEmpty(dr["WINUSUARIO"].ToString()))
              dr["WINUSUARIO"] = DBNull.Value;
            if (dr["PESSOA"] != null && string.IsNullOrEmpty(dr["PESSOA"].ToString()))
              dr["PESSOA"] = DBNull.Value;
            if (dr["PRIV_UNIDADE_ENS"] != null && string.IsNullOrEmpty(dr["PRIV_UNIDADE_ENS"].ToString()))
              dr["PRIV_UNIDADE_ENS"] = DBNull.Value;
            if (dr["MAT_CADASTRO"] != null && string.IsNullOrEmpty(dr["MAT_CADASTRO"].ToString()))
              dr["MAT_CADASTRO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["USUARIO"] == null || string.IsNullOrEmpty(dr["USUARIO"].ToString()))
            {
              return "Favor informar o usuário.";
            }

            if (dr["USUARIO"] != null && !string.IsNullOrEmpty(dr["USUARIO"].ToString()))
            {
              if (dr["USUARIO"].ToString().Length > 15)
              {
                return "O usuário deve possuir no máximo 15 caracteres.";
              }
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
              return "Favor informar o nome.";
            }

            if (dr["NOME"] != null && !string.IsNullOrEmpty(dr["NOME"].ToString()))
            {
              if (dr["NOME"].ToString().Length > 200)
              {
                return "O nome deve possuir no máximo 200 caracteres.";
              }
            }

            if (dr["GRUPOUSU"] != null && !string.IsNullOrEmpty(dr["GRUPOUSU"].ToString()))
            {
              if (dr["GRUPOUSU"].ToString().Length > 30)
              {
                return "O grupo de usuário deve possuir no máximo 30 caracteres.";
              }
            }

            if (dr["SENHA"] != null && !string.IsNullOrEmpty(dr["SENHA"].ToString()))
            {
              if (dr["SENHA"].ToString().Length > 30)
              {
                return "A senha deve possuir no máximo 30 caracteres.";
              }
            }

            if (dr["PRIVILEGIADO"] == null || string.IsNullOrEmpty(dr["PRIVILEGIADO"].ToString()))
            {
              return "Favor informar o privilegiado.";
            }

            if (dr["PRIVILEGIADO"] != null && !string.IsNullOrEmpty(dr["PRIVILEGIADO"].ToString()))
            {
              if (dr["PRIVILEGIADO"].ToString().Length > 1)
              {
                return "O privilegiado deve possuir no máximo 1 caractere.";
              }

              if (dr["PRIVILEGIADO"] != null && !string.IsNullOrEmpty(dr["PRIVILEGIADO"].ToString()))
              {
                List<object> lstPRIVILEGIADO = new List<object>(new object[]{"N", "S"});
                if(!lstPRIVILEGIADO.Contains(dr["PRIVILEGIADO"].ToString()))
                {
                  return "Os valores permitidos para o privilegiado são ''N'' ou ''S''.";
                }
              }
            }

            if (dr["EMAIL"] != null && !string.IsNullOrEmpty(dr["EMAIL"].ToString()))
            {
              if (dr["EMAIL"].ToString().Length > 200)
              {
                return "O email deve possuir no máximo 200 caracteres.";
              }
            }

            if (dr["MATRICULA"] != null && !string.IsNullOrEmpty(dr["MATRICULA"].ToString()))
            {
              if (dr["MATRICULA"].ToString().Length > 10)
              {
                return "A matrícula deve possuir no máximo 10 caracteres.";
              }
            }

            if (dr["SETOR"] != null && !string.IsNullOrEmpty(dr["SETOR"].ToString()))
            {
              if (dr["SETOR"].ToString().Length > 15)
              {
                return "A unidade administrativa deve possuir no máximo 15 caracteres.";
              }
            }

            if (dr["HABILITADO"] == null || string.IsNullOrEmpty(dr["HABILITADO"].ToString()))
            {
              return "Favor informar o habilitado.";
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

            if (dr["MOTIVO_DESABILITADO"] != null && !string.IsNullOrEmpty(dr["MOTIVO_DESABILITADO"].ToString()))
            {
              if (dr["MOTIVO_DESABILITADO"].ToString().Length > 2000)
              {
                return "O motivo da desabilitação deve possuir no máximo 2000 caracteres.";
              }
            }

            if (dr["DATA_ALTERACAO_SENHA"] != null && dr["DATA_ALTERACAO_SENHA"].ToString() != "")
            {
              dr["DATA_ALTERACAO_SENHA"] = Convert.ToDateTime(dr["DATA_ALTERACAO_SENHA"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["ALTERAR_SENHA"] != null && !string.IsNullOrEmpty(dr["ALTERAR_SENHA"].ToString()))
            {
              if (dr["ALTERAR_SENHA"].ToString().Length > 1)
              {
                return "O alterar senha deve possuir no máximo 1 caractere.";
              }

              if (dr["ALTERAR_SENHA"] != null && !string.IsNullOrEmpty(dr["ALTERAR_SENHA"].ToString()))
              {
                List<object> lstALTERAR_SENHA = new List<object>(new object[]{"N", "S"});
                if(!lstALTERAR_SENHA.Contains(dr["ALTERAR_SENHA"].ToString()))
                {
                  return "Os valores permitidos para o alterar senha são ''N'' ou ''S''.";
                }
              }
            }

            if (dr["SENHAS_ANTERIORES"] != null && !string.IsNullOrEmpty(dr["SENHAS_ANTERIORES"].ToString()))
            {
              if (dr["SENHAS_ANTERIORES"].ToString().Length > 1500)
              {
                return "As senhas anteriores devem possuir no máximo 1500 caracteres.";
              }
            }

            if (dr["SENHA_TEMP"] != null && !string.IsNullOrEmpty(dr["SENHA_TEMP"].ToString()))
            {
              if (dr["SENHA_TEMP"].ToString().Length > 60)
              {
                return "A senha  temporária deve possuir no máximo 60 caracteres.";
              }
            }

            if (dr["ULTIMO_LOGIN"] != null && dr["ULTIMO_LOGIN"].ToString() != "")
            {
              dr["ULTIMO_LOGIN"] = Convert.ToDateTime(dr["ULTIMO_LOGIN"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["TENTATIVAS_INCORRETAS"] != null && !string.IsNullOrEmpty(dr["TENTATIVAS_INCORRETAS"].ToString()))
            {
              if ((Convert.ToDecimal(dr["TENTATIVAS_INCORRETAS"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["TENTATIVAS_INCORRETAS"]).ToString("0.############").Split(',')[0].Length > 5) || (Convert.ToDecimal(dr["TENTATIVAS_INCORRETAS"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["TENTATIVAS_INCORRETAS"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "As tentativas incorretas deve ser um valor inteiro com 5 dígitos no máximo.";
              }
            }

            if (dr["WINUSUARIO"] != null && !string.IsNullOrEmpty(dr["WINUSUARIO"].ToString()))
            {
              if (dr["WINUSUARIO"].ToString().Length > 100)
              {
                return "O usuário do windows deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["PESSOA"] != null && !string.IsNullOrEmpty(dr["PESSOA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["PESSOA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PESSOA"]).ToString("0.############").Split(',')[0].Length > 12) || (Convert.ToDecimal(dr["PESSOA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PESSOA"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O código da pessoa deve ser um valor inteiro com 12 dígitos no máximo.";
              }
            }

            if (dr["PRIV_UNIDADE_ENS"] != null && !string.IsNullOrEmpty(dr["PRIV_UNIDADE_ENS"].ToString()))
            {
                if (dr["PRIV_UNIDADE_ENS"].ToString().Length > 1)
                {
                    return "O privilegiado unidade de ensino deve possuir no máximo 1 caractere.";
                }

                if (dr["PRIV_UNIDADE_ENS"] != null && !string.IsNullOrEmpty(dr["PRIV_UNIDADE_ENS"].ToString()))
                {
                    List<object> lstPRIVILEGIADO = new List<object>(new object[] { "N", "S" });
                    if (!lstPRIVILEGIADO.Contains(dr["PRIV_UNIDADE_ENS"].ToString()))
                    {
                        return "Os valores permitidos para o privilegiado unidade de ensino são ''N'' ou ''S''.";
                    }
                }
            }

            if (dr["MAT_CADASTRO"] != null && !string.IsNullOrEmpty(dr["MAT_CADASTRO"].ToString()))
            {
              if (dr["MAT_CADASTRO"].ToString().Length > 8)
              {
                return "A matrícula deve possuir no máximo 8 caracteres.";
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

    public static Hd_usuario Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_usuario Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_usuario tab = new Hd_usuario();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_usuario tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }
        

      public Row Add(
        string @usuario, string @sis, string @nome, string @grupousu, string @senha, string @privilegiado, 
        string @email, string @matricula, string @setor, string @habilitado, string @motivo_desabilitado, 
        DateTime? @data_alteracao_senha, string @alterar_senha, string @senhas_anteriores, 
        string @senha_temp, DateTime? @ultimo_login, decimal? @tentativas_incorretas, string @winusuario,
        decimal? @pessoa, string @priv_unidade_ens, string @mat_cadastro, DateTime? @dt_cadastro
      ) {
        return (Row)Add(new object[] {
          @usuario, @sis, @nome, @grupousu, @senha, @privilegiado, @email, @matricula, @setor, 
          @habilitado, @motivo_desabilitado, @data_alteracao_senha, @alterar_senha, @senhas_anteriores, 
          @senha_temp, @ultimo_login, @tentativas_incorretas, @winusuario, @pessoa, @priv_unidade_ens,
          @mat_cadastro, @dt_cadastro
        });
      }

      public Row Find(
        string @usuario
      ) {
        return (Row)Find(new object[] {
          @usuario
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
      /// USUARIO: VARCHAR(15), Primary Key, Not Null
      /// </summary>
      public string Usuario { get { return (string)this["usuario"]; } set { this["usuario"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SIS: VARCHAR(12)
      /// </summary>
      public string Sis { get { return (string)this["sis"]; } set { this["sis"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(50), Not Null
      /// </summary>
      public string Nome { get { return (string)this["nome"]; } set { this["nome"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// GRUPOUSU: VARCHAR(30)
      /// </summary>
      public string Grupousu { get { return (string)this["grupousu"]; } set { this["grupousu"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SENHA: VARCHAR(30)
      /// </summary>
      public string Senha { get { return (string)this["senha"]; } set { this["senha"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PRIVILEGIADO: CHAR(1), Not Null
      /// </summary>
      public string Privilegiado { get { return (string)this["privilegiado"]; } set { this["privilegiado"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// EMAIL: VARCHAR(200)
      /// </summary>
      public string Email { get { return (string)this["email"]; } set { this["email"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// MATRICULA: VARCHAR(10)
      /// </summary>
      public string Matricula { get { return (string)this["matricula"]; } set { this["matricula"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SETOR: VARCHAR(15)
      /// </summary>
      public string Setor { get { return (string)this["setor"]; } set { this["setor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// HABILITADO: CHAR(1), Not Null
      /// </summary>
      public string Habilitado { get { return (string)this["habilitado"]; } set { this["habilitado"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// MOTIVO_DESABILITADO: VARCHAR(2000)
      /// </summary>
      public string Motivo_desabilitado { get { return (string)this["motivo_desabilitado"]; } set { this["motivo_desabilitado"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DATA_ALTERACAO_SENHA: DATETIME
      /// </summary>
      public DateTime? Data_alteracao_senha { get { return (DateTime?)this["data_alteracao_senha"]; } set { this["data_alteracao_senha"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ALTERAR_SENHA: CHAR(1)
      /// </summary>
      public string Alterar_senha { get { return (string)this["alterar_senha"]; } set { this["alterar_senha"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SENHAS_ANTERIORES: VARCHAR(1500)
      /// </summary>
      public string Senhas_anteriores { get { return (string)this["senhas_anteriores"]; } set { this["senhas_anteriores"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SENHA_TEMP: VARCHAR(60)
      /// </summary>
      public string Senha_temp { get { return (string)this["senha_temp"]; } set { this["senha_temp"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ULTIMO_LOGIN: DATETIME
      /// </summary>
      public DateTime? Ultimo_login { get { return (DateTime?)this["ultimo_login"]; } set { this["ultimo_login"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TENTATIVAS_INCORRETAS: NUMERIC(5)
      /// </summary>
      public decimal? Tentativas_incorretas { get { return (decimal?)this["tentativas_incorretas"]; } set { this["tentativas_incorretas"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// WINUSUARIO: VARCHAR(100)
      /// </summary>
      public string Winusuario { get { return (string)this["winusuario"]; } set { this["winusuario"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PESSOA: NUMERIC(12)
      /// </summary>
      public decimal? Pessoa { get { return (decimal?)this["pessoa"]; } set { this["pessoa"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PRIV_UNIDADE_ENS: CHAR(1)
      /// </summary>
      public string Priv_unidade_ens { get { return (string)this["priv_unidade_ens"]; } set { this["priv_unidade_ens"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// MAT_CADASTRO: VARCHAR(8)
      /// </summary>
      public string Mat_cadastro { get { return (string)this["mat_cadastro"]; } set { this["mat_cadastro"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_CADASTRO: DATETIME
      /// </summary>
      public DateTime? Dt_cadastro { get { return (DateTime?)this["@dt_cadastro"]; } set { this["@dt_cadastro"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }   

      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @usuario, string @nome, string @privilegiado, string @habilitado) {
        return Insert(connection,
                      @usuario, @nome, @privilegiado, @habilitado,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @usuario, string @nome, string @privilegiado, string @habilitado,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_usuario(),
          new string[] { "usuario", "nome", "privilegiado", "habilitado" },
          new object[] { @usuario, @nome, @privilegiado, @habilitado },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkUsuario,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_usuario(),
          new string[] { "usuario" },
          new object[] { pkUsuario },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkUsuario) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_usuario(),
          new string[] { "usuario" },
          new object[] { pkUsuario }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkUsuario) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_usuario(),
          new string[] { "usuario" },
          new object[] { pkUsuario }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Usuario { get { return (string)row["usuario", DataRowVersion.Original]; } }
        public string Sis { get { return (string)row["sis", DataRowVersion.Original]; } }
        public string Nome { get { return (string)row["nome", DataRowVersion.Original]; } }
        public string Grupousu { get { return (string)row["grupousu", DataRowVersion.Original]; } }
        public string Senha { get { return (string)row["senha", DataRowVersion.Original]; } }
        public string Privilegiado { get { return (string)row["privilegiado", DataRowVersion.Original]; } }
        public string Email { get { return (string)row["email", DataRowVersion.Original]; } }
        public string Matricula { get { return (string)row["matricula", DataRowVersion.Original]; } }
        public string Setor { get { return (string)row["setor", DataRowVersion.Original]; } }
        public string Habilitado { get { return (string)row["habilitado", DataRowVersion.Original]; } }
        public string Motivo_desabilitado { get { return (string)row["motivo_desabilitado", DataRowVersion.Original]; } }
        public DateTime? Data_alteracao_senha { get { return (DateTime?)row["data_alteracao_senha", DataRowVersion.Original]; } }
        public string Alterar_senha { get { return (string)row["alterar_senha", DataRowVersion.Original]; } }
        public string Senhas_anteriores { get { return (string)row["senhas_anteriores", DataRowVersion.Original]; } }
        public string Senha_temp { get { return (string)row["senha_temp", DataRowVersion.Original]; } }
        public DateTime? Ultimo_login { get { return (DateTime?)row["ultimo_login", DataRowVersion.Original]; } }
        public decimal? Tentativas_incorretas { get { return (decimal?)row["tentativas_incorretas", DataRowVersion.Original]; } }
        public string Winusuario { get { return (string)row["winusuario", DataRowVersion.Original]; } }
        public decimal? Pessoa { get { return (decimal?)row["pessoa", DataRowVersion.Original]; } }
        public string Priv_unidade_ens { get { return (string)row["priv_unidade_ens", DataRowVersion.Original]; } }
        public string Mat_cadastro { get { return (string)row["mat_cadastro", DataRowVersion.Original]; } }
        public DateTime? Dt_cadastro { get { return (DateTime?)row["dt_cadastro", DataRowVersion.Original]; } }
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
    #region Coluna: USUARIO
    private ColumnUsuario pvColUsuario;

    public sealed class ColumnUsuario : TDataColumn {

      public ColumnUsuario() {
        this.ColumnName = "usuario";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "usuário";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_usuario.usuario"; } }
      public override string OriginalCol { get { return "usuario"; } }
      public override string RefCol { get { return "HD_USUARIO.USUARIO"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 15; } }
      public override string SqlServerType { get { return "VARCHAR(15)"; } }

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
      public override string FullCol { get { return "hd_usuario.sis"; } }
      public override string OriginalCol { get { return "sis"; } }
      public override string RefCol { get { return "HD_USUARIO.SIS"; } }
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
      public override string FullCol { get { return "hd_usuario.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "HD_USUARIO.NOME"; } }
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

    #region Coluna: GRUPOUSU
    private ColumnGrupousu pvColGrupousu;

    public sealed class ColumnGrupousu : TDataColumn {

      public ColumnGrupousu() {
        this.ColumnName = "grupousu";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "grupo de usuário";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_usuario.grupousu"; } }
      public override string OriginalCol { get { return "grupousu"; } }
      public override string RefCol { get { return "HD_USUARIO.GRUPOUSU"; } }
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

    #region Coluna: SENHA
    private ColumnSenha pvColSenha;

    public sealed class ColumnSenha : TDataColumn {

      public ColumnSenha() {
        this.ColumnName = "senha";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "senha";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_usuario.senha"; } }
      public override string OriginalCol { get { return "senha"; } }
      public override string RefCol { get { return "HD_USUARIO.SENHA"; } }
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

    #region Coluna: PRIVILEGIADO
    private ColumnPrivilegiado pvColPrivilegiado;

    public sealed class ColumnPrivilegiado : TDataColumn {

      public ColumnPrivilegiado() {
        this.ColumnName = "privilegiado";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "privilegiado";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_usuario.privilegiado"; } }
      public override string OriginalCol { get { return "privilegiado"; } }
      public override string RefCol { get { return "HD_USUARIO.PRIVILEGIADO"; } }
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

    #region Coluna: EMAIL
    private ColumnEmail pvColEmail;

    public sealed class ColumnEmail : TDataColumn {

      public ColumnEmail() {
        this.ColumnName = "email";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "email";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_usuario.email"; } }
      public override string OriginalCol { get { return "email"; } }
      public override string RefCol { get { return "HD_USUARIO.EMAIL"; } }
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

    #region Coluna: MATRICULA
    private ColumnMatricula pvColMatricula;

    public sealed class ColumnMatricula : TDataColumn {

      public ColumnMatricula() {
        this.ColumnName = "matricula";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "matrícula";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_usuario.matricula"; } }
      public override string OriginalCol { get { return "matricula"; } }
      public override string RefCol { get { return "HD_USUARIO.MATRICULA"; } }
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

    #region Coluna: SETOR
    private ColumnSetor pvColSetor;

    public sealed class ColumnSetor : TDataColumn {

      public ColumnSetor() {
        this.ColumnName = "setor";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "unidade administrativa";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_usuario.setor"; } }
      public override string OriginalCol { get { return "setor"; } }
      public override string RefCol { get { return "HD_USUARIO.SETOR"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 15; } }
      public override string SqlServerType { get { return "VARCHAR(15)"; } }

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
      public override string FullCol { get { return "hd_usuario.habilitado"; } }
      public override string OriginalCol { get { return "habilitado"; } }
      public override string RefCol { get { return "HD_USUARIO.HABILITADO"; } }
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

    #region Coluna: MOTIVO_DESABILITADO
    private ColumnMotivo_desabilitado pvColMotivo_desabilitado;

    public sealed class ColumnMotivo_desabilitado : TDataColumn {

      public ColumnMotivo_desabilitado() {
        this.ColumnName = "motivo_desabilitado";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "motivo da desabilitação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_usuario.motivo_desabilitado"; } }
      public override string OriginalCol { get { return "motivo_desabilitado"; } }
      public override string RefCol { get { return "HD_USUARIO.MOTIVO_DESABILITADO"; } }
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

    #region Coluna: DATA_ALTERACAO_SENHA
    private ColumnData_alteracao_senha pvColData_alteracao_senha;

    public sealed class ColumnData_alteracao_senha : TDataColumn {

      public ColumnData_alteracao_senha() {
        this.ColumnName = "data_alteracao_senha";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data da alteração da senha";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_usuario.data_alteracao_senha"; } }
      public override string OriginalCol { get { return "data_alteracao_senha"; } }
      public override string RefCol { get { return "HD_USUARIO.DATA_ALTERACAO_SENHA"; } }
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

    #region Coluna: ALTERAR_SENHA
    private ColumnAlterar_senha pvColAlterar_senha;

    public sealed class ColumnAlterar_senha : TDataColumn {

      public ColumnAlterar_senha() {
        this.ColumnName = "alterar_senha";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "alterar senha";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_usuario.alterar_senha"; } }
      public override string OriginalCol { get { return "alterar_senha"; } }
      public override string RefCol { get { return "HD_USUARIO.ALTERAR_SENHA"; } }
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

    #region Coluna: SENHAS_ANTERIORES
    private ColumnSenhas_anteriores pvColSenhas_anteriores;

    public sealed class ColumnSenhas_anteriores : TDataColumn {

      public ColumnSenhas_anteriores() {
        this.ColumnName = "senhas_anteriores";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "senhas anteriores";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_usuario.senhas_anteriores"; } }
      public override string OriginalCol { get { return "senhas_anteriores"; } }
      public override string RefCol { get { return "HD_USUARIO.SENHAS_ANTERIORES"; } }
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

    #region Coluna: SENHA_TEMP
    private ColumnSenha_temp pvColSenha_temp;

    public sealed class ColumnSenha_temp : TDataColumn {

      public ColumnSenha_temp() {
        this.ColumnName = "senha_temp";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "senha  temporária";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_usuario.senha_temp"; } }
      public override string OriginalCol { get { return "senha_temp"; } }
      public override string RefCol { get { return "HD_USUARIO.SENHA_TEMP"; } }
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

    #region Coluna: ULTIMO_LOGIN
    private ColumnUltimo_login pvColUltimo_login;

    public sealed class ColumnUltimo_login : TDataColumn {

      public ColumnUltimo_login() {
        this.ColumnName = "ultimo_login";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "último login";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_usuario.ultimo_login"; } }
      public override string OriginalCol { get { return "ultimo_login"; } }
      public override string RefCol { get { return "HD_USUARIO.ULTIMO_LOGIN"; } }
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

    #region Coluna: TENTATIVAS_INCORRETAS
    private ColumnTentativas_incorretas pvColTentativas_incorretas;

    public sealed class ColumnTentativas_incorretas : TDataColumn {

      public ColumnTentativas_incorretas() {
        this.ColumnName = "tentativas_incorretas";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "tentativas incorretas";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_usuario.tentativas_incorretas"; } }
      public override string OriginalCol { get { return "tentativas_incorretas"; } }
      public override string RefCol { get { return "HD_USUARIO.TENTATIVAS_INCORRETAS"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 6; } }
      public override int Precision { get { return 5; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(5)"; } }

    }
    #endregion

    #region Coluna: WINUSUARIO
    private ColumnWinusuario pvColWinusuario;

    public sealed class ColumnWinusuario : TDataColumn {

      public ColumnWinusuario() {
        this.ColumnName = "winusuario";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "usuário do windows";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_usuario.winusuario"; } }
      public override string OriginalCol { get { return "winusuario"; } }
      public override string RefCol { get { return "HD_USUARIO.WINUSUARIO"; } }
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

    #region Coluna: PESSOA
    private ColumnPessoa pvColPessoa;

    public sealed class ColumnPessoa : TDataColumn {

      public ColumnPessoa() {
        this.ColumnName = "pessoa";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "código da pessoa";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_usuario.pessoa"; } }
      public override string OriginalCol { get { return "pessoa"; } }
      public override string RefCol { get { return "HD_USUARIO.PESSOA"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 13; } }
      public override int Precision { get { return 12; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(12)"; } }

    }
    #endregion

    #region Coluna: PRIV_UNIDADE_ENS
    private ColumnPriv_unidade_ens pvColPriv_unidade_ens;

    public sealed class ColumnPriv_unidade_ens : TDataColumn
    {

        public ColumnPriv_unidade_ens()
        {
            this.ColumnName = "priv_unidade_ens";
            this.DataType = typeof(string);

        }


        protected override string GetName() { return "priv_unidade_ens"; }
        protected override string GetCaption() { return ""; }
        protected override string GetDescricao() { return ""; }
        public override string FullCol { get { return "hd_usuario.priv_unidade_ens"; } }
        public override string OriginalCol { get { return "priv_unidade_ens"; } }
        public override string RefCol { get { return "HD_USUARIO.PRIV_UNIDADE_ENS"; } }
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

    #region Coluna: MAT_CADASTRO
    private ColumnMat_cadastro pvColMat_cadastro;

    public sealed class ColumnMat_cadastro : TDataColumn
    {

        public ColumnMat_cadastro()
        {
            this.ColumnName = "mat_cadastro";
            this.DataType = typeof(string);

        }


        protected override string GetName() { return "mat_cadastro"; }
        protected override string GetCaption() { return ""; }
        protected override string GetDescricao() { return ""; }
        public override string FullCol { get { return "hd_usuario.mat_cadastro"; } }
        public override string OriginalCol { get { return "mat_cadastro"; } }
        public override string RefCol { get { return "HD_USUARIO.MAT_CADASTRO"; } }
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

    #region Coluna: DT_CADASTRO
    private ColumnDt_cadastro pvColDt_cadastro;

    public sealed class ColumnDt_cadastro : TDataColumn {

      public ColumnDt_cadastro() {
        this.ColumnName = "dt_cadastro";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_usuario.dt_cadastro"; } }
      public override string OriginalCol { get { return "dt_cadastro"; } }
      public override string RefCol { get { return "HD_USUARIO.DT_CADASTRO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override string SqlServerType { get { return "DATETIME"; } }

    }
    #endregion
    #endregion
  }
}

