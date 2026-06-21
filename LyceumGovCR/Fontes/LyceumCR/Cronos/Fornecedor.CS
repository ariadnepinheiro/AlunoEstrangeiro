// Lyceum vSeeduc1.7
// Gerado por:
//   cesar em 25/10/2010 às 15:18h
//   CronosRN, Version=1.1.3950.27138, Culture=neutral, PublicKeyToken=4a25cea0e390af6e
// Último load por marcelo.ris em 21/10/2010 às 15:32:54 do banco SQLServer (Cronos@Lyceumgov1:LycCronosAuditoria)

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

namespace Techne.Lyceum.CR {
  public class Fornecedor : TDataSet {
    private const string DataSetName_Def = "Fornecedor";
    private static ConstructorInfo ctorTCustom = null;
    private CustomBase tcustom = null;
    #region Variáveis private para as tabelas deste dataset
    private Ly_sm_cotacoes pvTabLy_sm_cotacoes;
    private Ly_sm_fornecedor pvTabLy_sm_fornecedor;
    private Ly_sm_fotoitem pvTabLy_sm_fotoitem;
    private Ly_sm_h_fornecedor pvTabLy_sm_h_fornecedor;
    private Ly_sm_itens pvTabLy_sm_itens;
    private Ly_sm_itensfornecidos pvTabLy_sm_itensfornecidos;
    private Ly_sm_marcas pvTabLy_sm_marcas;
    private Ly_sm_municipiosatendid pvTabLy_sm_municipiosatendid;
    private Ly_sm_precositem pvTabLy_sm_precositem;
    private Ly_sm_precosmercado pvTabLy_sm_precosmercado;
    private Ly_sm_unidmed pvTabLy_sm_unidmed;
    #endregion

    static Fornecedor() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Fornecedor");
      }
      catch{}
    }

    public Fornecedor() : base() {
      DataSetName = DataSetName_Def;

      pvTabLy_sm_cotacoes = new Ly_sm_cotacoes();
      pvTabLy_sm_fornecedor = new Ly_sm_fornecedor();
      pvTabLy_sm_fotoitem = new Ly_sm_fotoitem();
      pvTabLy_sm_h_fornecedor = new Ly_sm_h_fornecedor();
      pvTabLy_sm_itens = new Ly_sm_itens();
      pvTabLy_sm_itensfornecidos = new Ly_sm_itensfornecidos();
      pvTabLy_sm_marcas = new Ly_sm_marcas();
      pvTabLy_sm_municipiosatendid = new Ly_sm_municipiosatendid();
      pvTabLy_sm_precositem = new Ly_sm_precositem();
      pvTabLy_sm_precosmercado = new Ly_sm_precosmercado();
      pvTabLy_sm_unidmed = new Ly_sm_unidmed();

      Tables.Add(pvTabLy_sm_cotacoes);
      Tables.Add(pvTabLy_sm_fornecedor);
      Tables.Add(pvTabLy_sm_fotoitem);
      Tables.Add(pvTabLy_sm_h_fornecedor);
      Tables.Add(pvTabLy_sm_itens);
      Tables.Add(pvTabLy_sm_itensfornecidos);
      Tables.Add(pvTabLy_sm_marcas);
      Tables.Add(pvTabLy_sm_municipiosatendid);
      Tables.Add(pvTabLy_sm_precositem);
      Tables.Add(pvTabLy_sm_precosmercado);
      Tables.Add(pvTabLy_sm_unidmed);

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
          pvTabLy_sm_cotacoes.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_sm_cotacoes); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_sm_fornecedor.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_sm_fornecedor); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_sm_fotoitem.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_sm_fotoitem); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_sm_h_fornecedor.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_sm_h_fornecedor); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_sm_itens.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_sm_itens); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_sm_itensfornecidos.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_sm_itensfornecidos); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_sm_marcas.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_sm_marcas); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_sm_municipiosatendid.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_sm_municipiosatendid); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_sm_precositem.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_sm_precositem); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_sm_precosmercado.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_sm_precosmercado); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_sm_unidmed.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_sm_unidmed); if(row != null) return DataLib.RowErrorsToString(row);
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
    public Ly_sm_cotacoes Ly_sm_cotacoes { get { return pvTabLy_sm_cotacoes; } }

    [
      Browsable(false),
    ]
    public Ly_sm_fornecedor Ly_sm_fornecedor { get { return pvTabLy_sm_fornecedor; } }

    [
      Browsable(false),
    ]
    public Ly_sm_fotoitem Ly_sm_fotoitem { get { return pvTabLy_sm_fotoitem; } }

    [
      Browsable(false),
    ]
    public Ly_sm_h_fornecedor Ly_sm_h_fornecedor { get { return pvTabLy_sm_h_fornecedor; } }

    [
      Browsable(false),
    ]
    public Ly_sm_itens Ly_sm_itens { get { return pvTabLy_sm_itens; } }

    [
      Browsable(false),
    ]
    public Ly_sm_itensfornecidos Ly_sm_itensfornecidos { get { return pvTabLy_sm_itensfornecidos; } }

    [
      Browsable(false),
    ]
    public Ly_sm_marcas Ly_sm_marcas { get { return pvTabLy_sm_marcas; } }

    [
      Browsable(false),
    ]
    public Ly_sm_municipiosatendid Ly_sm_municipiosatendid { get { return pvTabLy_sm_municipiosatendid; } }

    [
      Browsable(false),
    ]
    public Ly_sm_precositem Ly_sm_precositem { get { return pvTabLy_sm_precositem; } }

    [
      Browsable(false),
    ]
    public Ly_sm_precosmercado Ly_sm_precosmercado { get { return pvTabLy_sm_precosmercado; } }

    [
      Browsable(false),
    ]
    public Ly_sm_unidmed Ly_sm_unidmed { get { return pvTabLy_sm_unidmed; } }

    #endregion

    public abstract class CustomBase {
      public abstract string Put(Techne.Lyceum.CR.Fornecedor ds, TConnectionWritable connection);
    }

  }


  public partial class Ly_sm_cotacoes : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_sm_cotacoes() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_sm_cotacoes");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_sm_cotacoes");
      }
      catch{}
    }

    public Ly_sm_cotacoes() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColIditem = new ColumnIditem(); Columns.Add(pvColIditem);
      pvColDescitem = new ColumnDescitem(); Columns.Add(pvColDescitem);
      pvColData = new ColumnData(); Columns.Add(pvColData);
      pvColPreco = new ColumnPreco(); Columns.Add(pvColPreco);
      pvColOrigem = new ColumnOrigem(); Columns.Add(pvColOrigem);
      pvColUsuario = new ColumnUsuario(); Columns.Add(pvColUsuario);
      pvColIdfornec = new ColumnIdfornec(); Columns.Add(pvColIdfornec);
      pvColIditemdesp = new ColumnIditemdesp(); Columns.Add(pvColIditemdesp);
      pvColMarca = new ColumnMarca(); Columns.Add(pvColMarca);
      pvColNome_fornecedor = new ColumnNome_fornecedor(); Columns.Add(pvColNome_fornecedor);
      pvColItemid_item = new ColumnItemid_item(); Columns.Add(pvColItemid_item);
      pvColNome_item = new ColumnNome_item(); Columns.Add(pvColNome_item);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_sm_cotacoes");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(decimal? @iditem, DateTime? @data, decimal? @preco, string @origem) {
      Row row = NewRow();

      row.Iditem = @iditem;
      row.Data = @data;
      row.Preco = @preco;
      row.Origem = @origem;

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
      get { return "LY_SM_COTACOES"; }
    }

    protected override sealed string Name { get { return "Cotação"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColId });
      pvColId.AllowDBNull = true;
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_sm_cotacoes_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_sm_cotacoes_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_sm_cotacoes_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_sm_cotacoes_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("iditem", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "IDITEM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("descitem", OleDbType.VarChar, 2100, ParameterDirection.InputOutput, true, 0, 0, "DESCITEM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("data", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DATA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("preco", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "PRECO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("origem", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "ORIGEM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("usuario", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "USUARIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("idfornec", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "IDFORNEC", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("iditemdesp", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "IDITEMDESP", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("marca", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "MARCA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "id") { result.Add(toLower ? "id" : column); continue; }
          if(lower == "iditem") { result.Add(toLower ? "iditem" : column); continue; }
          if(lower == "descitem") { result.Add(toLower ? "descitem" : column); continue; }
          if(lower == "data") { result.Add(toLower ? "data" : column); continue; }
          if(lower == "preco") { result.Add(toLower ? "preco" : column); continue; }
          if(lower == "origem") { result.Add(toLower ? "origem" : column); continue; }
          if(lower == "usuario") { result.Add(toLower ? "usuario" : column); continue; }
          if(lower == "idfornec") { result.Add(toLower ? "idfornec" : column); continue; }
          if(lower == "iditemdesp") { result.Add(toLower ? "iditemdesp" : column); continue; }
          if(lower == "marca") { result.Add(toLower ? "marca" : column); continue; }
          if(!tabPrincipal) {
            if(lower == "nome_fornecedor") { result.Add(toLower ? "nome_fornecedor" : column); continue; }
            if(lower == "itemid_item") { result.Add(toLower ? "itemid_item" : column); continue; }
            if(lower == "nome_item") { result.Add(toLower ? "nome_item" : column); continue; }
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "iditem", "descitem", "data", "preco", "origem", "usuario", 
                                           "idfornec", "iditemdesp", "marca" }
                          : new string[] { "ID", "IDITEM", "DESCITEM", "DATA", "PRECO", "ORIGEM", "USUARIO", 
                                           "IDFORNEC", "IDITEMDESP", "MARCA" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "iditem", "descitem", "data", "preco", "origem", "usuario", 
                                           "idfornec", "iditemdesp", "marca", "nome_fornecedor", "itemid_item", "nome_item" }
                          : new string[] { "ID", "IDITEM", "DESCITEM", "DATA", "PRECO", "ORIGEM", "USUARIO", 
                                           "IDFORNEC", "IDITEMDESP", "MARCA", "NOME_FORNECEDOR", "ITEMID_ITEM", "NOME_ITEM" });

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
        "ly_sm_cotacoes.id"
      });
      if(scColumns.Contains("iditem")) list.Add("ly_sm_cotacoes.iditem");
      if(scColumns.Contains("descitem")) list.Add("ly_sm_cotacoes.descitem");
      if(scColumns.Contains("data")) list.Add("ly_sm_cotacoes.data");
      if(scColumns.Contains("preco")) list.Add("ly_sm_cotacoes.preco");
      if(scColumns.Contains("origem")) list.Add("ly_sm_cotacoes.origem");
      if(scColumns.Contains("usuario")) list.Add("ly_sm_cotacoes.usuario");
      if(scColumns.Contains("idfornec")) list.Add("ly_sm_cotacoes.idfornec");
      if(scColumns.Contains("iditemdesp")) list.Add("ly_sm_cotacoes.iditemdesp");
      if(scColumns.Contains("marca")) list.Add("ly_sm_cotacoes.marca");
      if(scColumns.Contains("nome_fornecedor")) list.Add("ly_sm_fornecedor.nome nome_fornecedor");
      if(scColumns.Contains("itemid_item")) list.Add("ly_sm_itens.itemid itemid_item");
      if(scColumns.Contains("nome_item")) list.Add("ly_sm_itens.nome nome_item");
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
          "FROM ly_sm_cotacoes " +
               "LEFT JOIN ly_sm_fornecedor " +
                 "ON ly_sm_fornecedor.id = ly_sm_cotacoes.idfornec " +
               "INNER JOIN ly_sm_itens " +
                 "ON ly_sm_itens.id = ly_sm_cotacoes.iditem " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_sm_cotacoes");
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
        da.TableMappings.Add("Table", "Ly_sm_cotacoes");
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
            if (dr["IDITEM"] != null && string.IsNullOrEmpty(dr["IDITEM"].ToString()))
              dr["IDITEM"] = DBNull.Value;
            if (dr["DESCITEM"] != null && string.IsNullOrEmpty(dr["DESCITEM"].ToString()))
              dr["DESCITEM"] = DBNull.Value;
            if (dr["PRECO"] != null && string.IsNullOrEmpty(dr["PRECO"].ToString()))
              dr["PRECO"] = DBNull.Value;
            if (dr["ORIGEM"] != null && string.IsNullOrEmpty(dr["ORIGEM"].ToString()))
              dr["ORIGEM"] = DBNull.Value;
            if (dr["USUARIO"] != null && string.IsNullOrEmpty(dr["USUARIO"].ToString()))
              dr["USUARIO"] = DBNull.Value;
            if (dr["IDFORNEC"] != null && string.IsNullOrEmpty(dr["IDFORNEC"].ToString()))
              dr["IDFORNEC"] = DBNull.Value;
            if (dr["IDITEMDESP"] != null && string.IsNullOrEmpty(dr["IDITEMDESP"].ToString()))
              dr["IDITEMDESP"] = DBNull.Value;
            if (dr["MARCA"] != null && string.IsNullOrEmpty(dr["MARCA"].ToString()))
              dr["MARCA"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["IDITEM"] == null || string.IsNullOrEmpty(dr["IDITEM"].ToString()))
            {
              return "Favor informar o item.";
            }

            if (dr["IDITEM"] != null && !string.IsNullOrEmpty(dr["IDITEM"].ToString()))
            {
              if ((Convert.ToDecimal(dr["IDITEM"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["IDITEM"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["IDITEM"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["IDITEM"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O item deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["DESCITEM"] != null && !string.IsNullOrEmpty(dr["DESCITEM"].ToString()))
            {
              if (dr["DESCITEM"].ToString().Length > 2000)
              {
                return "A descrição do item deve possuir no máximo 2000 caracteres.";
              }
            }

            if (dr["DATA"] == null || string.IsNullOrEmpty(dr["DATA"].ToString()))
            {
              return "Favor informar a data.";
            }

            if (dr["DATA"] != null && dr["DATA"].ToString() != "")
            {
              dr["DATA"] = Convert.ToDateTime(dr["DATA"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["PRECO"] == null || string.IsNullOrEmpty(dr["PRECO"].ToString()))
            {
              return "Favor informar o preço.";
            }

            if (dr["PRECO"] != null && !string.IsNullOrEmpty(dr["PRECO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["PRECO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PRECO"]).ToString("0.############").Split(',')[0].Length > 8) || (Convert.ToDecimal(dr["PRECO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PRECO"]).ToString("0.############").Split(',')[1].Length > 2))
              {
                return "O preço deve ser um valor numérico com, no máximo, 8 dígitos na parte inteira e 2 casas decimal.";
              }
            }

            if (dr["ORIGEM"] == null || string.IsNullOrEmpty(dr["ORIGEM"].ToString()))
            {
              return "Favor informar a origem.";
            }

            if (dr["ORIGEM"] != null && !string.IsNullOrEmpty(dr["ORIGEM"].ToString()))
            {
              if (dr["ORIGEM"].ToString().Length > 40)
              {
                return "A origem deve possuir no máximo 40 caracteres.";
              }
            }

            if (dr["USUARIO"] != null && !string.IsNullOrEmpty(dr["USUARIO"].ToString()))
            {
              if (dr["USUARIO"].ToString().Length > 15)
              {
                return "O usuário deve possuir no máximo 15 caracteres.";
              }
            }

            if (dr["IDFORNEC"] != null && !string.IsNullOrEmpty(dr["IDFORNEC"].ToString()))
            {
              if ((Convert.ToDecimal(dr["IDFORNEC"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["IDFORNEC"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["IDFORNEC"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["IDFORNEC"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O fornecedor deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["IDITEMDESP"] != null && !string.IsNullOrEmpty(dr["IDITEMDESP"].ToString()))
            {
              if ((Convert.ToDecimal(dr["IDITEMDESP"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["IDITEMDESP"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["IDITEMDESP"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["IDITEMDESP"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O item de despesa deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["MARCA"] != null && !string.IsNullOrEmpty(dr["MARCA"].ToString()))
            {
              if (dr["MARCA"].ToString().Length > 50)
              {
                return "A marca deve possuir no máximo 50 caracteres.";
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

    public static Ly_sm_cotacoes Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_sm_cotacoes Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_sm_cotacoes tab = new Ly_sm_cotacoes();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_sm_cotacoes tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, decimal? @iditem, string @descitem, DateTime? @data, decimal? @preco, 
        string @origem, string @usuario, decimal? @idfornec, decimal? @iditemdesp, string @marca
      ) {
        return (Row)Add(new object[] {
          @id, @iditem, @descitem, @data, @preco, @origem, @usuario, @idfornec, @iditemdesp, 
          @marca
        });
      }

      public Row Find(
        decimal? @id
      ) {
        return (Row)Find(new object[] {
          @id
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
      /// ID: NUMERIC(14), Primary Key, AutoNumber, Not Null
      /// </summary>
      public decimal? Id { get { return (decimal?)this["id"]; } set { this["id"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// IDITEM: NUMERIC(14), Not Null
      /// </summary>
      public decimal? Iditem { get { return (decimal?)this["iditem"]; } set { this["iditem"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCITEM: VARCHAR(2000)
      /// </summary>
      public string Descitem { get { return (string)this["descitem"]; } set { this["descitem"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DATA: DATETIME, Not Null
      /// </summary>
      public DateTime? Data { get { return (DateTime?)this["data"]; } set { this["data"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PRECO: NUMERIC(10, 2), Not Null
      /// </summary>
      public decimal? Preco { get { return (decimal?)this["preco"]; } set { this["preco"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ORIGEM: VARCHAR(40), Not Null
      /// </summary>
      public string Origem { get { return (string)this["origem"]; } set { this["origem"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// USUARIO: VARCHAR(15)
      /// </summary>
      public string Usuario { get { return (string)this["usuario"]; } set { this["usuario"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// IDFORNEC: NUMERIC(14)
      /// </summary>
      public decimal? Idfornec { get { return (decimal?)this["idfornec"]; } set { this["idfornec"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// IDITEMDESP: NUMERIC(14)
      /// </summary>
      public decimal? Iditemdesp { get { return (decimal?)this["iditemdesp"]; } set { this["iditemdesp"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// MARCA: VARCHAR(50)
      /// </summary>
      public string Marca { get { return (string)this["marca"]; } set { this["marca"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(200), Not Null
      /// </summary>
      public string Nome_fornecedor { get { return (string)this["nome_fornecedor"]; } set { this["nome_fornecedor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ITEMID: VARCHAR(100), Not Null
      /// </summary>
      public string Itemid_item { get { return (string)this["itemid_item"]; } set { this["itemid_item"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(200), Not Null
      /// </summary>
      public string Nome_item { get { return (string)this["nome_item"]; } set { this["nome_item"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               decimal? @iditem, DateTime? @data, decimal? @preco, string @origem) {
        return Insert(connection,
                      @iditem, @data, @preco, @origem,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               decimal? @iditem, DateTime? @data, decimal? @preco, string @origem,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_sm_cotacoes(),
          new string[] { "iditem", "data", "preco", "origem" },
          new object[] { @iditem, @data, @preco, @origem },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_sm_cotacoes(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_sm_cotacoes(),
          new string[] { "id" },
          new object[] { pkId }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              decimal? pkId) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_sm_cotacoes(),
          new string[] { "id" },
          new object[] { pkId }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public decimal? Id { get { return (decimal?)row["id", DataRowVersion.Original]; } }
        public decimal? Iditem { get { return (decimal?)row["iditem", DataRowVersion.Original]; } }
        public string Descitem { get { return (string)row["descitem", DataRowVersion.Original]; } }
        public DateTime? Data { get { return (DateTime?)row["data", DataRowVersion.Original]; } }
        public decimal? Preco { get { return (decimal?)row["preco", DataRowVersion.Original]; } }
        public string Origem { get { return (string)row["origem", DataRowVersion.Original]; } }
        public string Usuario { get { return (string)row["usuario", DataRowVersion.Original]; } }
        public decimal? Idfornec { get { return (decimal?)row["idfornec", DataRowVersion.Original]; } }
        public decimal? Iditemdesp { get { return (decimal?)row["iditemdesp", DataRowVersion.Original]; } }
        public string Marca { get { return (string)row["marca", DataRowVersion.Original]; } }
        public string Nome_fornecedor { get { return (string)row["nome_fornecedor", DataRowVersion.Original]; } }
        public string Itemid_item { get { return (string)row["itemid_item", DataRowVersion.Original]; } }
        public string Nome_item { get { return (string)row["nome_item", DataRowVersion.Original]; } }

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
    #region Coluna: ID
    private ColumnId pvColId;

    public sealed class ColumnId : TDataColumn {

      public ColumnId() {
        this.ColumnName = "id";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "id";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_cotacoes.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_SM_COTACOES.ID"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 15; } }
      public override int Precision { get { return 14; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(14)"; } }

    }
    #endregion

    #region Coluna: IDITEM
    private ColumnIditem pvColIditem;

    public sealed class ColumnIditem : TDataColumn {

      public ColumnIditem() {
        this.ColumnName = "iditem";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "item";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_cotacoes.iditem"; } }
      public override string OriginalCol { get { return "iditem"; } }
      public override string RefCol { get { return "LY_SM_COTACOES.IDITEM"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 15; } }
      public override int Precision { get { return 14; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(14)"; } }

    }
    #endregion

    #region Coluna: DESCITEM
    private ColumnDescitem pvColDescitem;

    public sealed class ColumnDescitem : TDataColumn {

      public ColumnDescitem() {
        this.ColumnName = "descitem";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "descrição do item";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_cotacoes.descitem"; } }
      public override string OriginalCol { get { return "descitem"; } }
      public override string RefCol { get { return "LY_SM_COTACOES.DESCITEM"; } }
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

    #region Coluna: DATA
    private ColumnData pvColData;

    public sealed class ColumnData : TDataColumn {

      public ColumnData() {
        this.ColumnName = "data";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data";}
      protected override string GetCaption(){return "Data";}
      protected override string GetDescricao(){return "Data";}
      public override string FullCol { get { return "ly_sm_cotacoes.data"; } }
      public override string OriginalCol { get { return "data"; } }
      public override string RefCol { get { return "LY_SM_COTACOES.DATA"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 10; } }
      public override string SqlServerType { get { return "DATETIME"; } }

    }
    #endregion

    #region Coluna: PRECO
    private ColumnPreco pvColPreco;

    public sealed class ColumnPreco : TDataColumn {

      public ColumnPreco() {
        this.ColumnName = "preco";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "preço";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_cotacoes.preco"; } }
      public override string OriginalCol { get { return "preco"; } }
      public override string RefCol { get { return "LY_SM_COTACOES.PRECO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 12; } }
      public override int Precision { get { return 10; } }
      public override int Scale { get { return 2; } }
      public override string SqlServerType { get { return "NUMERIC(10, 2)"; } }

    }
    #endregion

    #region Coluna: ORIGEM
    private ColumnOrigem pvColOrigem;

    public sealed class ColumnOrigem : TDataColumn {

      public ColumnOrigem() {
        this.ColumnName = "origem";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "origem";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_cotacoes.origem"; } }
      public override string OriginalCol { get { return "origem"; } }
      public override string RefCol { get { return "LY_SM_COTACOES.ORIGEM"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 40; } }
      public override string SqlServerType { get { return "VARCHAR(40)"; } }

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
      public override string FullCol { get { return "ly_sm_cotacoes.usuario"; } }
      public override string OriginalCol { get { return "usuario"; } }
      public override string RefCol { get { return "LY_SM_COTACOES.USUARIO"; } }
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

    #region Coluna: IDFORNEC
    private ColumnIdfornec pvColIdfornec;

    public sealed class ColumnIdfornec : TDataColumn {

      public ColumnIdfornec() {
        this.ColumnName = "idfornec";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "fornecedor";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_cotacoes.idfornec"; } }
      public override string OriginalCol { get { return "idfornec"; } }
      public override string RefCol { get { return "LY_SM_COTACOES.IDFORNEC"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 15; } }
      public override int Precision { get { return 14; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(14)"; } }

    }
    #endregion

    #region Coluna: IDITEMDESP
    private ColumnIditemdesp pvColIditemdesp;

    public sealed class ColumnIditemdesp : TDataColumn {

      public ColumnIditemdesp() {
        this.ColumnName = "iditemdesp";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "item de despesa";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_cotacoes.iditemdesp"; } }
      public override string OriginalCol { get { return "iditemdesp"; } }
      public override string RefCol { get { return "LY_SM_COTACOES.IDITEMDESP"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 15; } }
      public override int Precision { get { return 14; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(14)"; } }

    }
    #endregion

    #region Coluna: MARCA
    private ColumnMarca pvColMarca;

    public sealed class ColumnMarca : TDataColumn {

      public ColumnMarca() {
        this.ColumnName = "marca";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "marca";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_cotacoes.marca"; } }
      public override string OriginalCol { get { return "marca"; } }
      public override string RefCol { get { return "LY_SM_COTACOES.MARCA"; } }
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

    #region Coluna: NOME_FORNECEDOR
    private ColumnNome_fornecedor pvColNome_fornecedor;

    public sealed class ColumnNome_fornecedor : TDataColumn {

      public ColumnNome_fornecedor() {
        this.ColumnName = "nome_fornecedor";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "nome";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.NOME"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 200; } }
      public override string SqlServerType { get { return "VARCHAR(200)"; } }

    }
    #endregion

    #region Coluna: ITEMID_ITEM
    private ColumnItemid_item pvColItemid_item;

    public sealed class ColumnItemid_item : TDataColumn {

      public ColumnItemid_item() {
        this.ColumnName = "itemid_item";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "código";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_itens.itemid"; } }
      public override string OriginalCol { get { return "itemid"; } }
      public override string RefCol { get { return "LY_SM_ITENS.ITEMID"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 100; } }
      public override string SqlServerType { get { return "VARCHAR(100)"; } }

    }
    #endregion

    #region Coluna: NOME_ITEM
    private ColumnNome_item pvColNome_item;

    public sealed class ColumnNome_item : TDataColumn {

      public ColumnNome_item() {
        this.ColumnName = "nome_item";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "nome";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_itens.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "LY_SM_ITENS.NOME"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 200; } }
      public override string SqlServerType { get { return "VARCHAR(200)"; } }

    }
    #endregion
    #endregion
  }

  public partial class Ly_sm_fornecedor : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_sm_fornecedor() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_sm_fornecedor");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_sm_fornecedor");
      }
      catch{}
    }

    public Ly_sm_fornecedor() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColNome = new ColumnNome(); Columns.Add(pvColNome);
      pvColCnpj = new ColumnCnpj(); Columns.Add(pvColCnpj);
      pvColCpf = new ColumnCpf(); Columns.Add(pvColCpf);
      pvColInscrest = new ColumnInscrest(); Columns.Add(pvColInscrest);
      pvColInscrmunic = new ColumnInscrmunic(); Columns.Add(pvColInscrmunic);
      pvColNum_origem = new ColumnNum_origem(); Columns.Add(pvColNum_origem);
      pvColEnd_mail = new ColumnEnd_mail(); Columns.Add(pvColEnd_mail);
      pvColEnd_web = new ColumnEnd_web(); Columns.Add(pvColEnd_web);
      pvColHabilitado = new ColumnHabilitado(); Columns.Add(pvColHabilitado);
      pvColMotivo_hab_inab = new ColumnMotivo_hab_inab(); Columns.Add(pvColMotivo_hab_inab);
      pvColAtividade = new ColumnAtividade(); Columns.Add(pvColAtividade);
      pvColTipo_servico = new ColumnTipo_servico(); Columns.Add(pvColTipo_servico);
      pvColCoordenadoria = new ColumnCoordenadoria(); Columns.Add(pvColCoordenadoria);
      pvColRepreslegal = new ColumnRepreslegal(); Columns.Add(pvColRepreslegal);
      pvColCpf_represlegal = new ColumnCpf_represlegal(); Columns.Add(pvColCpf_represlegal);
      pvColEndereco = new ColumnEndereco(); Columns.Add(pvColEndereco);
      pvColEnd_num = new ColumnEnd_num(); Columns.Add(pvColEnd_num);
      pvColEnd_compl = new ColumnEnd_compl(); Columns.Add(pvColEnd_compl);
      pvColEnd_municipio = new ColumnEnd_municipio(); Columns.Add(pvColEnd_municipio);
      pvColEnd_pais = new ColumnEnd_pais(); Columns.Add(pvColEnd_pais);
      pvColEnd_cep = new ColumnEnd_cep(); Columns.Add(pvColEnd_cep);
      pvColEnd_bairro = new ColumnEnd_bairro(); Columns.Add(pvColEnd_bairro);
      pvColCaixa_postal = new ColumnCaixa_postal(); Columns.Add(pvColCaixa_postal);
      pvColFone = new ColumnFone(); Columns.Add(pvColFone);
      pvColFax = new ColumnFax(); Columns.Add(pvColFax);
      pvColCelular = new ColumnCelular(); Columns.Add(pvColCelular);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_sm_fornecedor");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @nome, string @habilitado, string @tipo_servico) {
      Row row = NewRow();

      row.Nome = @nome;
      row.Habilitado = @habilitado;
      row.Tipo_servico = @tipo_servico;

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
      get { return "LY_SM_FORNECEDOR"; }
    }

    protected override sealed string Name { get { return "Fornecedor"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColId });
      pvColId.AllowDBNull = true;
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_sm_fornecedor_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_sm_fornecedor_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_sm_fornecedor_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_sm_fornecedor_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("nome", OleDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "NOME", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("cnpj", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CNPJ", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("cpf", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CPF", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("inscrest", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "INSCREST", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("inscrmunic", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "INSCRMUNIC", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("num_origem", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NUM_ORIGEM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("end_mail", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "END_MAIL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("end_web", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "END_WEB", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("habilitado", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "HABILITADO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("motivo_hab_inab", OleDbType.VarChar, 2100, ParameterDirection.InputOutput, true, 0, 0, "MOTIVO_HAB_INAB", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("atividade", OleDbType.VarChar, 400, ParameterDirection.InputOutput, true, 0, 0, "ATIVIDADE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("tipo_servico", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TIPO_SERVICO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("coordenadoria", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "COORDENADORIA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("represlegal", OleDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "REPRESLEGAL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("cpf_represlegal", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CPF_REPRESLEGAL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("endereco", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "ENDERECO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("end_num", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "END_NUM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("end_compl", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "END_COMPL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("end_municipio", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "END_MUNICIPIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("end_pais", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "END_PAIS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("end_cep", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "END_CEP", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("end_bairro", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "END_BAIRRO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("caixa_postal", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CAIXA_POSTAL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("fone", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "FONE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("fax", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "FAX", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("celular", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CELULAR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "id") { result.Add(toLower ? "id" : column); continue; }
          if(lower == "nome") { result.Add(toLower ? "nome" : column); continue; }
          if(lower == "cnpj") { result.Add(toLower ? "cnpj" : column); continue; }
          if(lower == "cpf") { result.Add(toLower ? "cpf" : column); continue; }
          if(lower == "inscrest") { result.Add(toLower ? "inscrest" : column); continue; }
          if(lower == "inscrmunic") { result.Add(toLower ? "inscrmunic" : column); continue; }
          if(lower == "num_origem") { result.Add(toLower ? "num_origem" : column); continue; }
          if(lower == "end_mail") { result.Add(toLower ? "end_mail" : column); continue; }
          if(lower == "end_web") { result.Add(toLower ? "end_web" : column); continue; }
          if(lower == "habilitado") { result.Add(toLower ? "habilitado" : column); continue; }
          if(lower == "motivo_hab_inab") { result.Add(toLower ? "motivo_hab_inab" : column); continue; }
          if(lower == "atividade") { result.Add(toLower ? "atividade" : column); continue; }
          if(lower == "tipo_servico") { result.Add(toLower ? "tipo_servico" : column); continue; }
          if(lower == "coordenadoria") { result.Add(toLower ? "coordenadoria" : column); continue; }
          if(lower == "represlegal") { result.Add(toLower ? "represlegal" : column); continue; }
          if(lower == "cpf_represlegal") { result.Add(toLower ? "cpf_represlegal" : column); continue; }
          if(lower == "endereco") { result.Add(toLower ? "endereco" : column); continue; }
          if(lower == "end_num") { result.Add(toLower ? "end_num" : column); continue; }
          if(lower == "end_compl") { result.Add(toLower ? "end_compl" : column); continue; }
          if(lower == "end_municipio") { result.Add(toLower ? "end_municipio" : column); continue; }
          if(lower == "end_pais") { result.Add(toLower ? "end_pais" : column); continue; }
          if(lower == "end_cep") { result.Add(toLower ? "end_cep" : column); continue; }
          if(lower == "end_bairro") { result.Add(toLower ? "end_bairro" : column); continue; }
          if(lower == "caixa_postal") { result.Add(toLower ? "caixa_postal" : column); continue; }
          if(lower == "fone") { result.Add(toLower ? "fone" : column); continue; }
          if(lower == "fax") { result.Add(toLower ? "fax" : column); continue; }
          if(lower == "celular") { result.Add(toLower ? "celular" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "nome", "cnpj", "cpf", "inscrest", "inscrmunic", "num_origem", 
                                           "end_mail", "end_web", "habilitado", "motivo_hab_inab", "atividade", "tipo_servico", 
                                           "coordenadoria", "represlegal", "cpf_represlegal", "endereco", "end_num", "end_compl", 
                                           "end_municipio", "end_pais", "end_cep", "end_bairro", "caixa_postal", "fone", "fax", 
                                           "celular" }
                          : new string[] { "ID", "NOME", "CNPJ", "CPF", "INSCREST", "INSCRMUNIC", "NUM_ORIGEM", 
                                           "END_MAIL", "END_WEB", "HABILITADO", "MOTIVO_HAB_INAB", "ATIVIDADE", "TIPO_SERVICO", 
                                           "COORDENADORIA", "REPRESLEGAL", "CPF_REPRESLEGAL", "ENDERECO", "END_NUM", "END_COMPL", 
                                           "END_MUNICIPIO", "END_PAIS", "END_CEP", "END_BAIRRO", "CAIXA_POSTAL", "FONE", "FAX", 
                                           "CELULAR" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "nome", "cnpj", "cpf", "inscrest", "inscrmunic", "num_origem", 
                                           "end_mail", "end_web", "habilitado", "motivo_hab_inab", "atividade", "tipo_servico", 
                                           "coordenadoria", "represlegal", "cpf_represlegal", "endereco", "end_num", "end_compl", 
                                           "end_municipio", "end_pais", "end_cep", "end_bairro", "caixa_postal", "fone", "fax", 
                                           "celular" }
                          : new string[] { "ID", "NOME", "CNPJ", "CPF", "INSCREST", "INSCRMUNIC", "NUM_ORIGEM", 
                                           "END_MAIL", "END_WEB", "HABILITADO", "MOTIVO_HAB_INAB", "ATIVIDADE", "TIPO_SERVICO", 
                                           "COORDENADORIA", "REPRESLEGAL", "CPF_REPRESLEGAL", "ENDERECO", "END_NUM", "END_COMPL", 
                                           "END_MUNICIPIO", "END_PAIS", "END_CEP", "END_BAIRRO", "CAIXA_POSTAL", "FONE", "FAX", 
                                           "CELULAR" });

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
        "ly_sm_fornecedor.id"
      });
      if(scColumns.Contains("nome")) list.Add("ly_sm_fornecedor.nome");
      if(scColumns.Contains("cnpj")) list.Add("ly_sm_fornecedor.cnpj");
      if(scColumns.Contains("cpf")) list.Add("ly_sm_fornecedor.cpf");
      if(scColumns.Contains("inscrest")) list.Add("ly_sm_fornecedor.inscrest");
      if(scColumns.Contains("inscrmunic")) list.Add("ly_sm_fornecedor.inscrmunic");
      if(scColumns.Contains("num_origem")) list.Add("ly_sm_fornecedor.num_origem");
      if(scColumns.Contains("end_mail")) list.Add("ly_sm_fornecedor.end_mail");
      if(scColumns.Contains("end_web")) list.Add("ly_sm_fornecedor.end_web");
      if(scColumns.Contains("habilitado")) list.Add("ly_sm_fornecedor.habilitado");
      if(scColumns.Contains("motivo_hab_inab")) list.Add("ly_sm_fornecedor.motivo_hab_inab");
      if(scColumns.Contains("atividade")) list.Add("ly_sm_fornecedor.atividade");
      if(scColumns.Contains("tipo_servico")) list.Add("ly_sm_fornecedor.tipo_servico");
      if(scColumns.Contains("coordenadoria")) list.Add("ly_sm_fornecedor.coordenadoria");
      if(scColumns.Contains("represlegal")) list.Add("ly_sm_fornecedor.represlegal");
      if(scColumns.Contains("cpf_represlegal")) list.Add("ly_sm_fornecedor.cpf_represlegal");
      if(scColumns.Contains("endereco")) list.Add("ly_sm_fornecedor.endereco");
      if(scColumns.Contains("end_num")) list.Add("ly_sm_fornecedor.end_num");
      if(scColumns.Contains("end_compl")) list.Add("ly_sm_fornecedor.end_compl");
      if(scColumns.Contains("end_municipio")) list.Add("ly_sm_fornecedor.end_municipio");
      if(scColumns.Contains("end_pais")) list.Add("ly_sm_fornecedor.end_pais");
      if(scColumns.Contains("end_cep")) list.Add("ly_sm_fornecedor.end_cep");
      if(scColumns.Contains("end_bairro")) list.Add("ly_sm_fornecedor.end_bairro");
      if(scColumns.Contains("caixa_postal")) list.Add("ly_sm_fornecedor.caixa_postal");
      if(scColumns.Contains("fone")) list.Add("ly_sm_fornecedor.fone");
      if(scColumns.Contains("fax")) list.Add("ly_sm_fornecedor.fax");
      if(scColumns.Contains("celular")) list.Add("ly_sm_fornecedor.celular");
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
          "FROM ly_sm_fornecedor " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_sm_fornecedor");
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
        da.TableMappings.Add("Table", "Ly_sm_fornecedor");
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
            if (dr["NOME"] != null && string.IsNullOrEmpty(dr["NOME"].ToString()))
              dr["NOME"] = DBNull.Value;
            if (dr["CNPJ"] != null && string.IsNullOrEmpty(dr["CNPJ"].ToString()))
              dr["CNPJ"] = DBNull.Value;
            if (dr["CPF"] != null && string.IsNullOrEmpty(dr["CPF"].ToString()))
              dr["CPF"] = DBNull.Value;
            if (dr["INSCREST"] != null && string.IsNullOrEmpty(dr["INSCREST"].ToString()))
              dr["INSCREST"] = DBNull.Value;
            if (dr["INSCRMUNIC"] != null && string.IsNullOrEmpty(dr["INSCRMUNIC"].ToString()))
              dr["INSCRMUNIC"] = DBNull.Value;
            if (dr["NUM_ORIGEM"] != null && string.IsNullOrEmpty(dr["NUM_ORIGEM"].ToString()))
              dr["NUM_ORIGEM"] = DBNull.Value;
            if (dr["END_MAIL"] != null && string.IsNullOrEmpty(dr["END_MAIL"].ToString()))
              dr["END_MAIL"] = DBNull.Value;
            if (dr["END_WEB"] != null && string.IsNullOrEmpty(dr["END_WEB"].ToString()))
              dr["END_WEB"] = DBNull.Value;
            if (dr["HABILITADO"] != null && string.IsNullOrEmpty(dr["HABILITADO"].ToString()))
              dr["HABILITADO"] = DBNull.Value;
            if (dr["MOTIVO_HAB_INAB"] != null && string.IsNullOrEmpty(dr["MOTIVO_HAB_INAB"].ToString()))
              dr["MOTIVO_HAB_INAB"] = DBNull.Value;
            if (dr["ATIVIDADE"] != null && string.IsNullOrEmpty(dr["ATIVIDADE"].ToString()))
              dr["ATIVIDADE"] = DBNull.Value;
            if (dr["TIPO_SERVICO"] != null && string.IsNullOrEmpty(dr["TIPO_SERVICO"].ToString()))
              dr["TIPO_SERVICO"] = DBNull.Value;
            if (dr["COORDENADORIA"] != null && string.IsNullOrEmpty(dr["COORDENADORIA"].ToString()))
              dr["COORDENADORIA"] = DBNull.Value;
            if (dr["REPRESLEGAL"] != null && string.IsNullOrEmpty(dr["REPRESLEGAL"].ToString()))
              dr["REPRESLEGAL"] = DBNull.Value;
            if (dr["CPF_REPRESLEGAL"] != null && string.IsNullOrEmpty(dr["CPF_REPRESLEGAL"].ToString()))
              dr["CPF_REPRESLEGAL"] = DBNull.Value;
            if (dr["ENDERECO"] != null && string.IsNullOrEmpty(dr["ENDERECO"].ToString()))
              dr["ENDERECO"] = DBNull.Value;
            if (dr["END_NUM"] != null && string.IsNullOrEmpty(dr["END_NUM"].ToString()))
              dr["END_NUM"] = DBNull.Value;
            if (dr["END_COMPL"] != null && string.IsNullOrEmpty(dr["END_COMPL"].ToString()))
              dr["END_COMPL"] = DBNull.Value;
            if (dr["END_MUNICIPIO"] != null && string.IsNullOrEmpty(dr["END_MUNICIPIO"].ToString()))
              dr["END_MUNICIPIO"] = DBNull.Value;
            if (dr["END_PAIS"] != null && string.IsNullOrEmpty(dr["END_PAIS"].ToString()))
              dr["END_PAIS"] = DBNull.Value;
            if (dr["END_CEP"] != null && string.IsNullOrEmpty(dr["END_CEP"].ToString()))
              dr["END_CEP"] = DBNull.Value;
            if (dr["END_BAIRRO"] != null && string.IsNullOrEmpty(dr["END_BAIRRO"].ToString()))
              dr["END_BAIRRO"] = DBNull.Value;
            if (dr["CAIXA_POSTAL"] != null && string.IsNullOrEmpty(dr["CAIXA_POSTAL"].ToString()))
              dr["CAIXA_POSTAL"] = DBNull.Value;
            if (dr["FONE"] != null && string.IsNullOrEmpty(dr["FONE"].ToString()))
              dr["FONE"] = DBNull.Value;
            if (dr["FAX"] != null && string.IsNullOrEmpty(dr["FAX"].ToString()))
              dr["FAX"] = DBNull.Value;
            if (dr["CELULAR"] != null && string.IsNullOrEmpty(dr["CELULAR"].ToString()))
              dr["CELULAR"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

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

            if (dr["CNPJ"] != null && !string.IsNullOrEmpty(dr["CNPJ"].ToString()))
            {
              if (dr["CNPJ"].ToString().Length > 19)
              {
                return "O cnpj deve possuir no máximo 19 caracteres.";
              }
            }

            if (dr["CPF"] != null && !string.IsNullOrEmpty(dr["CPF"].ToString()))
            {
              if (dr["CPF"].ToString().Length > 14)
              {
                return "O cpf deve possuir no máximo 14 caracteres.";
              }
            }

            if (dr["INSCREST"] != null && !string.IsNullOrEmpty(dr["INSCREST"].ToString()))
            {
              if (dr["INSCREST"].ToString().Length > 15)
              {
                return "A inscrição estadual deve possuir no máximo 15 caracteres.";
              }
            }

            if (dr["INSCRMUNIC"] != null && !string.IsNullOrEmpty(dr["INSCRMUNIC"].ToString()))
            {
              if (dr["INSCRMUNIC"].ToString().Length > 15)
              {
                return "A inscrição municipal deve possuir no máximo 15 caracteres.";
              }
            }

            if (dr["NUM_ORIGEM"] != null && !string.IsNullOrEmpty(dr["NUM_ORIGEM"].ToString()))
            {
              if (dr["NUM_ORIGEM"].ToString().Length > 50)
              {
                return "O número de origem deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["END_MAIL"] != null && !string.IsNullOrEmpty(dr["END_MAIL"].ToString()))
            {
              if (dr["END_MAIL"].ToString().Length > 100)
              {
                return "O email deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["END_WEB"] != null && !string.IsNullOrEmpty(dr["END_WEB"].ToString()))
            {
              if (dr["END_WEB"].ToString().Length > 100)
              {
                return "O site web deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["HABILITADO"] == null || string.IsNullOrEmpty(dr["HABILITADO"].ToString()))
            {
              return "Favor informar a habilitação.";
            }

            if (dr["HABILITADO"] != null && !string.IsNullOrEmpty(dr["HABILITADO"].ToString()))
            {
              if (dr["HABILITADO"].ToString().Length > 1)
              {
                return "A habilitação deve possuir no máximo 1 caractere.";
              }
            }

            if (dr["MOTIVO_HAB_INAB"] != null && !string.IsNullOrEmpty(dr["MOTIVO_HAB_INAB"].ToString()))
            {
              if (dr["MOTIVO_HAB_INAB"].ToString().Length > 2000)
              {
                return "O motivo para desabilitação deve possuir no máximo 2000 caracteres.";
              }
            }

            if (dr["ATIVIDADE"] != null && !string.IsNullOrEmpty(dr["ATIVIDADE"].ToString()))
            {
              if (dr["ATIVIDADE"].ToString().Length > 300)
              {
                return "A atividade deve possuir no máximo 300 caracteres.";
              }
            }

            if (dr["TIPO_SERVICO"] == null || string.IsNullOrEmpty(dr["TIPO_SERVICO"].ToString()))
            {
              return "Favor informar o tipo de serviço.";
            }

            if (dr["TIPO_SERVICO"] != null && !string.IsNullOrEmpty(dr["TIPO_SERVICO"].ToString()))
            {
              if (dr["TIPO_SERVICO"].ToString().Length > 40)
              {
                return "O tipo de serviço deve possuir no máximo 40 caracteres.";
              }
            }

            if (dr["COORDENADORIA"] != null && !string.IsNullOrEmpty(dr["COORDENADORIA"].ToString()))
            {
              if (dr["COORDENADORIA"].ToString().Length > 20)
              {
                return "A coordenadoria deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["REPRESLEGAL"] != null && !string.IsNullOrEmpty(dr["REPRESLEGAL"].ToString()))
            {
              if (dr["REPRESLEGAL"].ToString().Length > 200)
              {
                return "O representante legal deve possuir no máximo 200 caracteres.";
              }
            }

            if (dr["CPF_REPRESLEGAL"] != null && !string.IsNullOrEmpty(dr["CPF_REPRESLEGAL"].ToString()))
            {
              if (dr["CPF_REPRESLEGAL"].ToString().Length > 14)
              {
                return "O cpf do representante legal deve possuir no máximo 14 caracteres.";
              }
            }

            if (dr["ENDERECO"] != null && !string.IsNullOrEmpty(dr["ENDERECO"].ToString()))
            {
              if (dr["ENDERECO"].ToString().Length > 50)
              {
                return "O endereco deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["END_NUM"] != null && !string.IsNullOrEmpty(dr["END_NUM"].ToString()))
            {
              if (dr["END_NUM"].ToString().Length > 15)
              {
                return "O número deve possuir no máximo 15 caracteres.";
              }
            }

            if (dr["END_COMPL"] != null && !string.IsNullOrEmpty(dr["END_COMPL"].ToString()))
            {
              if (dr["END_COMPL"].ToString().Length > 50)
              {
                return "O complemento deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["END_MUNICIPIO"] != null && !string.IsNullOrEmpty(dr["END_MUNICIPIO"].ToString()))
            {
              if (dr["END_MUNICIPIO"].ToString().Length > 20)
              {
                return "O município deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["END_PAIS"] != null && !string.IsNullOrEmpty(dr["END_PAIS"].ToString()))
            {
              if (dr["END_PAIS"].ToString().Length > 50)
              {
                return "O país deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["END_CEP"] != null && !string.IsNullOrEmpty(dr["END_CEP"].ToString()))
            {
              if (dr["END_CEP"].ToString().Length > 9)
              {
                return "O cep deve possuir no máximo 9 caracteres.";
              }
            }

            if (dr["END_BAIRRO"] != null && !string.IsNullOrEmpty(dr["END_BAIRRO"].ToString()))
            {
              if (dr["END_BAIRRO"].ToString().Length > 50)
              {
                return "O bairro deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["CAIXA_POSTAL"] != null && !string.IsNullOrEmpty(dr["CAIXA_POSTAL"].ToString()))
            {
              if (dr["CAIXA_POSTAL"].ToString().Length > 50)
              {
                return "A caixa postal deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["FONE"] != null && !string.IsNullOrEmpty(dr["FONE"].ToString()))
            {
              if (dr["FONE"].ToString().Length > 30)
              {
                return "O telefone deve possuir no máximo 30 caracteres.";
              }
            }

            if (dr["FAX"] != null && !string.IsNullOrEmpty(dr["FAX"].ToString()))
            {
              if (dr["FAX"].ToString().Length > 30)
              {
                return "O fax deve possuir no máximo 30 caracteres.";
              }
            }

            if (dr["CELULAR"] != null && !string.IsNullOrEmpty(dr["CELULAR"].ToString()))
            {
              if (dr["CELULAR"].ToString().Length > 30)
              {
                return "O celular deve possuir no máximo 30 caracteres.";
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

    public static Ly_sm_fornecedor Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_sm_fornecedor Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_sm_fornecedor tab = new Ly_sm_fornecedor();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_sm_fornecedor tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, string @nome, string @cnpj, string @cpf, string @inscrest, string @inscrmunic, 
        string @num_origem, string @end_mail, string @end_web, string @habilitado, string @motivo_hab_inab, 
        string @atividade, string @tipo_servico, string @coordenadoria, string @represlegal, 
        string @cpf_represlegal, string @endereco, string @end_num, string @end_compl, string @end_municipio, 
        string @end_pais, string @end_cep, string @end_bairro, string @caixa_postal, string @fone, 
        string @fax, string @celular
      ) {
        return (Row)Add(new object[] {
          @id, @nome, @cnpj, @cpf, @inscrest, @inscrmunic, @num_origem, @end_mail, @end_web, 
          @habilitado, @motivo_hab_inab, @atividade, @tipo_servico, @coordenadoria, @represlegal, 
          @cpf_represlegal, @endereco, @end_num, @end_compl, @end_municipio, @end_pais, @end_cep, 
          @end_bairro, @caixa_postal, @fone, @fax, @celular
        });
      }

      public Row Find(
        decimal? @id
      ) {
        return (Row)Find(new object[] {
          @id
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
      /// ID: NUMERIC(14), Primary Key, AutoNumber, Not Null
      /// </summary>
      public decimal? Id { get { return (decimal?)this["id"]; } set { this["id"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(200), Not Null
      /// </summary>
      public string Nome { get { return (string)this["nome"]; } set { this["nome"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CNPJ: VARCHAR(19)
      /// </summary>
      public string Cnpj { get { return (string)this["cnpj"]; } set { this["cnpj"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CPF: VARCHAR(14)
      /// </summary>
      public string Cpf { get { return (string)this["cpf"]; } set { this["cpf"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// INSCREST: VARCHAR(15)
      /// </summary>
      public string Inscrest { get { return (string)this["inscrest"]; } set { this["inscrest"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// INSCRMUNIC: VARCHAR(15)
      /// </summary>
      public string Inscrmunic { get { return (string)this["inscrmunic"]; } set { this["inscrmunic"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NUM_ORIGEM: VARCHAR(50)
      /// </summary>
      public string Num_origem { get { return (string)this["num_origem"]; } set { this["num_origem"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// END_MAIL: VARCHAR(100)
      /// </summary>
      public string End_mail { get { return (string)this["end_mail"]; } set { this["end_mail"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// END_WEB: VARCHAR(100)
      /// </summary>
      public string End_web { get { return (string)this["end_web"]; } set { this["end_web"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// HABILITADO: CHAR(1), Not Null
      /// </summary>
      public string Habilitado { get { return (string)this["habilitado"]; } set { this["habilitado"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// MOTIVO_HAB_INAB: VARCHAR(2000)
      /// </summary>
      public string Motivo_hab_inab { get { return (string)this["motivo_hab_inab"]; } set { this["motivo_hab_inab"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ATIVIDADE: VARCHAR(300)
      /// </summary>
      public string Atividade { get { return (string)this["atividade"]; } set { this["atividade"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TIPO_SERVICO: VARCHAR(40), Not Null
      /// </summary>
      public string Tipo_servico { get { return (string)this["tipo_servico"]; } set { this["tipo_servico"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// COORDENADORIA: VARCHAR(20)
      /// </summary>
      public string Coordenadoria { get { return (string)this["coordenadoria"]; } set { this["coordenadoria"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// REPRESLEGAL: VARCHAR(200)
      /// </summary>
      public string Represlegal { get { return (string)this["represlegal"]; } set { this["represlegal"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CPF_REPRESLEGAL: VARCHAR(14)
      /// </summary>
      public string Cpf_represlegal { get { return (string)this["cpf_represlegal"]; } set { this["cpf_represlegal"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ENDERECO: VARCHAR(50)
      /// </summary>
      public string Endereco { get { return (string)this["endereco"]; } set { this["endereco"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// END_NUM: VARCHAR(15)
      /// </summary>
      public string End_num { get { return (string)this["end_num"]; } set { this["end_num"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// END_COMPL: VARCHAR(50)
      /// </summary>
      public string End_compl { get { return (string)this["end_compl"]; } set { this["end_compl"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// END_MUNICIPIO: VARCHAR(20)
      /// </summary>
      public string End_municipio { get { return (string)this["end_municipio"]; } set { this["end_municipio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// END_PAIS: VARCHAR(50)
      /// </summary>
      public string End_pais { get { return (string)this["end_pais"]; } set { this["end_pais"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// END_CEP: VARCHAR(9)
      /// </summary>
      public string End_cep { get { return (string)this["end_cep"]; } set { this["end_cep"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// END_BAIRRO: VARCHAR(50)
      /// </summary>
      public string End_bairro { get { return (string)this["end_bairro"]; } set { this["end_bairro"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CAIXA_POSTAL: VARCHAR(50)
      /// </summary>
      public string Caixa_postal { get { return (string)this["caixa_postal"]; } set { this["caixa_postal"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// FONE: VARCHAR(30)
      /// </summary>
      public string Fone { get { return (string)this["fone"]; } set { this["fone"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// FAX: VARCHAR(30)
      /// </summary>
      public string Fax { get { return (string)this["fax"]; } set { this["fax"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CELULAR: VARCHAR(30)
      /// </summary>
      public string Celular { get { return (string)this["celular"]; } set { this["celular"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @nome, string @habilitado, string @tipo_servico) {
        return Insert(connection,
                      @nome, @habilitado, @tipo_servico,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @nome, string @habilitado, string @tipo_servico,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_sm_fornecedor(),
          new string[] { "nome", "habilitado", "tipo_servico" },
          new object[] { @nome, @habilitado, @tipo_servico },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_sm_fornecedor(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_sm_fornecedor(),
          new string[] { "id" },
          new object[] { pkId }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              decimal? pkId) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_sm_fornecedor(),
          new string[] { "id" },
          new object[] { pkId }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public decimal? Id { get { return (decimal?)row["id", DataRowVersion.Original]; } }
        public string Nome { get { return (string)row["nome", DataRowVersion.Original]; } }
        public string Cnpj { get { return (string)row["cnpj", DataRowVersion.Original]; } }
        public string Cpf { get { return (string)row["cpf", DataRowVersion.Original]; } }
        public string Inscrest { get { return (string)row["inscrest", DataRowVersion.Original]; } }
        public string Inscrmunic { get { return (string)row["inscrmunic", DataRowVersion.Original]; } }
        public string Num_origem { get { return (string)row["num_origem", DataRowVersion.Original]; } }
        public string End_mail { get { return (string)row["end_mail", DataRowVersion.Original]; } }
        public string End_web { get { return (string)row["end_web", DataRowVersion.Original]; } }
        public string Habilitado { get { return (string)row["habilitado", DataRowVersion.Original]; } }
        public string Motivo_hab_inab { get { return (string)row["motivo_hab_inab", DataRowVersion.Original]; } }
        public string Atividade { get { return (string)row["atividade", DataRowVersion.Original]; } }
        public string Tipo_servico { get { return (string)row["tipo_servico", DataRowVersion.Original]; } }
        public string Coordenadoria { get { return (string)row["coordenadoria", DataRowVersion.Original]; } }
        public string Represlegal { get { return (string)row["represlegal", DataRowVersion.Original]; } }
        public string Cpf_represlegal { get { return (string)row["cpf_represlegal", DataRowVersion.Original]; } }
        public string Endereco { get { return (string)row["endereco", DataRowVersion.Original]; } }
        public string End_num { get { return (string)row["end_num", DataRowVersion.Original]; } }
        public string End_compl { get { return (string)row["end_compl", DataRowVersion.Original]; } }
        public string End_municipio { get { return (string)row["end_municipio", DataRowVersion.Original]; } }
        public string End_pais { get { return (string)row["end_pais", DataRowVersion.Original]; } }
        public string End_cep { get { return (string)row["end_cep", DataRowVersion.Original]; } }
        public string End_bairro { get { return (string)row["end_bairro", DataRowVersion.Original]; } }
        public string Caixa_postal { get { return (string)row["caixa_postal", DataRowVersion.Original]; } }
        public string Fone { get { return (string)row["fone", DataRowVersion.Original]; } }
        public string Fax { get { return (string)row["fax", DataRowVersion.Original]; } }
        public string Celular { get { return (string)row["celular", DataRowVersion.Original]; } }

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
    #region Coluna: ID
    private ColumnId pvColId;

    public sealed class ColumnId : TDataColumn {

      public ColumnId() {
        this.ColumnName = "id";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "id";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.ID"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 15; } }
      public override int Precision { get { return 14; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(14)"; } }

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
      public override string FullCol { get { return "ly_sm_fornecedor.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.NOME"; } }
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

    #region Coluna: CNPJ
    private ColumnCnpj pvColCnpj;

    public sealed class ColumnCnpj : TDataColumn {

      public ColumnCnpj() {
        this.ColumnName = "cnpj";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "cnpj";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.cnpj"; } }
      public override string OriginalCol { get { return "cnpj"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.CNPJ"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 19; } }
      public override string SqlServerType { get { return "VARCHAR(19)"; } }

    }
    #endregion

    #region Coluna: CPF
    private ColumnCpf pvColCpf;

    public sealed class ColumnCpf : TDataColumn {

      public ColumnCpf() {
        this.ColumnName = "cpf";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "cpf";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.cpf"; } }
      public override string OriginalCol { get { return "cpf"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.CPF"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 14; } }
      public override string SqlServerType { get { return "VARCHAR(14)"; } }

    }
    #endregion

    #region Coluna: INSCREST
    private ColumnInscrest pvColInscrest;

    public sealed class ColumnInscrest : TDataColumn {

      public ColumnInscrest() {
        this.ColumnName = "inscrest";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "inscrição estadual";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.inscrest"; } }
      public override string OriginalCol { get { return "inscrest"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.INSCREST"; } }
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

    #region Coluna: INSCRMUNIC
    private ColumnInscrmunic pvColInscrmunic;

    public sealed class ColumnInscrmunic : TDataColumn {

      public ColumnInscrmunic() {
        this.ColumnName = "inscrmunic";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "inscrição municipal";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.inscrmunic"; } }
      public override string OriginalCol { get { return "inscrmunic"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.INSCRMUNIC"; } }
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

    #region Coluna: NUM_ORIGEM
    private ColumnNum_origem pvColNum_origem;

    public sealed class ColumnNum_origem : TDataColumn {

      public ColumnNum_origem() {
        this.ColumnName = "num_origem";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "número de origem";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.num_origem"; } }
      public override string OriginalCol { get { return "num_origem"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.NUM_ORIGEM"; } }
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

    #region Coluna: END_MAIL
    private ColumnEnd_mail pvColEnd_mail;

    public sealed class ColumnEnd_mail : TDataColumn {

      public ColumnEnd_mail() {
        this.ColumnName = "end_mail";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "email";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.end_mail"; } }
      public override string OriginalCol { get { return "end_mail"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.END_MAIL"; } }
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

    #region Coluna: END_WEB
    private ColumnEnd_web pvColEnd_web;

    public sealed class ColumnEnd_web : TDataColumn {

      public ColumnEnd_web() {
        this.ColumnName = "end_web";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "site web";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.end_web"; } }
      public override string OriginalCol { get { return "end_web"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.END_WEB"; } }
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

    #region Coluna: HABILITADO
    private ColumnHabilitado pvColHabilitado;

    public sealed class ColumnHabilitado : TDataColumn {

      public ColumnHabilitado() {
        this.ColumnName = "habilitado";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "habilitação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.habilitado"; } }
      public override string OriginalCol { get { return "habilitado"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.HABILITADO"; } }
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

    #region Coluna: MOTIVO_HAB_INAB
    private ColumnMotivo_hab_inab pvColMotivo_hab_inab;

    public sealed class ColumnMotivo_hab_inab : TDataColumn {

      public ColumnMotivo_hab_inab() {
        this.ColumnName = "motivo_hab_inab";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "motivo para desabilitação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.motivo_hab_inab"; } }
      public override string OriginalCol { get { return "motivo_hab_inab"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.MOTIVO_HAB_INAB"; } }
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

    #region Coluna: ATIVIDADE
    private ColumnAtividade pvColAtividade;

    public sealed class ColumnAtividade : TDataColumn {

      public ColumnAtividade() {
        this.ColumnName = "atividade";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "atividade";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.atividade"; } }
      public override string OriginalCol { get { return "atividade"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.ATIVIDADE"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 300; } }
      public override string SqlServerType { get { return "VARCHAR(300)"; } }

    }
    #endregion

    #region Coluna: TIPO_SERVICO
    private ColumnTipo_servico pvColTipo_servico;

    public sealed class ColumnTipo_servico : TDataColumn {

      public ColumnTipo_servico() {
        this.ColumnName = "tipo_servico";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "tipo de serviço";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.tipo_servico"; } }
      public override string OriginalCol { get { return "tipo_servico"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.TIPO_SERVICO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 40; } }
      public override string SqlServerType { get { return "VARCHAR(40)"; } }

    }
    #endregion

    #region Coluna: COORDENADORIA
    private ColumnCoordenadoria pvColCoordenadoria;

    public sealed class ColumnCoordenadoria : TDataColumn {

      public ColumnCoordenadoria() {
        this.ColumnName = "coordenadoria";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "coordenadoria";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.coordenadoria"; } }
      public override string OriginalCol { get { return "coordenadoria"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.COORDENADORIA"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 20; } }
      public override string SqlServerType { get { return "VARCHAR(20)"; } }

    }
    #endregion

    #region Coluna: REPRESLEGAL
    private ColumnRepreslegal pvColRepreslegal;

    public sealed class ColumnRepreslegal : TDataColumn {

      public ColumnRepreslegal() {
        this.ColumnName = "represlegal";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "representante legal";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.represlegal"; } }
      public override string OriginalCol { get { return "represlegal"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.REPRESLEGAL"; } }
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

    #region Coluna: CPF_REPRESLEGAL
    private ColumnCpf_represlegal pvColCpf_represlegal;

    public sealed class ColumnCpf_represlegal : TDataColumn {

      public ColumnCpf_represlegal() {
        this.ColumnName = "cpf_represlegal";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "cpf do representante legal";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.cpf_represlegal"; } }
      public override string OriginalCol { get { return "cpf_represlegal"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.CPF_REPRESLEGAL"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 14; } }
      public override string SqlServerType { get { return "VARCHAR(14)"; } }

    }
    #endregion

    #region Coluna: ENDERECO
    private ColumnEndereco pvColEndereco;

    public sealed class ColumnEndereco : TDataColumn {

      public ColumnEndereco() {
        this.ColumnName = "endereco";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "endereco";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.endereco"; } }
      public override string OriginalCol { get { return "endereco"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.ENDERECO"; } }
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

    #region Coluna: END_NUM
    private ColumnEnd_num pvColEnd_num;

    public sealed class ColumnEnd_num : TDataColumn {

      public ColumnEnd_num() {
        this.ColumnName = "end_num";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "número";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.end_num"; } }
      public override string OriginalCol { get { return "end_num"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.END_NUM"; } }
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

    #region Coluna: END_COMPL
    private ColumnEnd_compl pvColEnd_compl;

    public sealed class ColumnEnd_compl : TDataColumn {

      public ColumnEnd_compl() {
        this.ColumnName = "end_compl";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "complemento";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.end_compl"; } }
      public override string OriginalCol { get { return "end_compl"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.END_COMPL"; } }
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

    #region Coluna: END_MUNICIPIO
    private ColumnEnd_municipio pvColEnd_municipio;

    public sealed class ColumnEnd_municipio : TDataColumn {

      public ColumnEnd_municipio() {
        this.ColumnName = "end_municipio";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "município";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.end_municipio"; } }
      public override string OriginalCol { get { return "end_municipio"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.END_MUNICIPIO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 20; } }
      public override string SqlServerType { get { return "VARCHAR(20)"; } }

    }
    #endregion

    #region Coluna: END_PAIS
    private ColumnEnd_pais pvColEnd_pais;

    public sealed class ColumnEnd_pais : TDataColumn {

      public ColumnEnd_pais() {
        this.ColumnName = "end_pais";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "país";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.end_pais"; } }
      public override string OriginalCol { get { return "end_pais"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.END_PAIS"; } }
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

    #region Coluna: END_CEP
    private ColumnEnd_cep pvColEnd_cep;

    public sealed class ColumnEnd_cep : TDataColumn {

      public ColumnEnd_cep() {
        this.ColumnName = "end_cep";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "cep";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.end_cep"; } }
      public override string OriginalCol { get { return "end_cep"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.END_CEP"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 9; } }
      public override string SqlServerType { get { return "VARCHAR(9)"; } }

    }
    #endregion

    #region Coluna: END_BAIRRO
    private ColumnEnd_bairro pvColEnd_bairro;

    public sealed class ColumnEnd_bairro : TDataColumn {

      public ColumnEnd_bairro() {
        this.ColumnName = "end_bairro";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "bairro";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.end_bairro"; } }
      public override string OriginalCol { get { return "end_bairro"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.END_BAIRRO"; } }
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

    #region Coluna: CAIXA_POSTAL
    private ColumnCaixa_postal pvColCaixa_postal;

    public sealed class ColumnCaixa_postal : TDataColumn {

      public ColumnCaixa_postal() {
        this.ColumnName = "caixa_postal";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "caixa postal";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.caixa_postal"; } }
      public override string OriginalCol { get { return "caixa_postal"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.CAIXA_POSTAL"; } }
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

    #region Coluna: FONE
    private ColumnFone pvColFone;

    public sealed class ColumnFone : TDataColumn {

      public ColumnFone() {
        this.ColumnName = "fone";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "telefone";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.fone"; } }
      public override string OriginalCol { get { return "fone"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.FONE"; } }
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

    #region Coluna: FAX
    private ColumnFax pvColFax;

    public sealed class ColumnFax : TDataColumn {

      public ColumnFax() {
        this.ColumnName = "fax";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "fax";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.fax"; } }
      public override string OriginalCol { get { return "fax"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.FAX"; } }
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

    #region Coluna: CELULAR
    private ColumnCelular pvColCelular;

    public sealed class ColumnCelular : TDataColumn {

      public ColumnCelular() {
        this.ColumnName = "celular";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "celular";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.celular"; } }
      public override string OriginalCol { get { return "celular"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.CELULAR"; } }
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

  public partial class Ly_sm_fotoitem : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_sm_fotoitem() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_sm_fotoitem");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_sm_fotoitem");
      }
      catch{}
    }

    public Ly_sm_fotoitem() {
      pvColIditem = new ColumnIditem(); Columns.Add(pvColIditem);
      pvColFotoitem = new ColumnFotoitem(); Columns.Add(pvColFotoitem);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_sm_fotoitem");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(decimal? @iditem) {
      Row row = NewRow();

      row.Iditem = @iditem;

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
      get { return "LY_SM_FOTOITEM"; }
    }

    protected override sealed string Name { get { return "Foto do Item"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColIditem });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_sm_fotoitem_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_sm_fotoitem_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_sm_fotoitem_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_sm_fotoitem_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkIditem", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "IDItem", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("iditem", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "IDItem", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("fotoitem", OleDbType.VarBinary, 0, ParameterDirection.Input, true, 0, 0, "FotoItem", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("5F5F554E4445465F4C4F4E475241575F5F")));

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
          if(lower == "iditem") { result.Add(toLower ? "iditem" : column); continue; }
          if(lower == "fotoitem") { result.Add(toLower ? "fotoitem" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "iditem", "fotoitem" }
                          : new string[] { "IDItem", "FotoItem" });
        else
          result.AddRange(toLower
                          ? new string[] { "iditem", "fotoitem" }
                          : new string[] { "IDItem", "FotoItem" });

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
        "ly_sm_fotoitem.iditem"
      });
      if(scColumns.Contains("fotoitem")) list.Add("ly_sm_fotoitem.fotoitem");
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
          "FROM ly_sm_fotoitem " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_sm_fotoitem");
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
        da.TableMappings.Add("Table", "Ly_sm_fotoitem");
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
            if (dr["IDItem"] != null && string.IsNullOrEmpty(dr["IDItem"].ToString()))
              dr["IDItem"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["IDItem"] == null || string.IsNullOrEmpty(dr["IDItem"].ToString()))
            {
              return "Favor informar o id.";
            }

            if (dr["IDItem"] != null && !string.IsNullOrEmpty(dr["IDItem"].ToString()))
            {
              if ((Convert.ToDecimal(dr["IDItem"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["IDItem"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["IDItem"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["IDItem"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O id deve ser um valor inteiro com 14 dígitos no máximo.";
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

    public static Ly_sm_fotoitem Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_sm_fotoitem Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_sm_fotoitem tab = new Ly_sm_fotoitem();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_sm_fotoitem tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @iditem, byte[] @fotoitem
      ) {
        return (Row)Add(new object[] {
          @iditem, @fotoitem
        });
      }

      public Row Find(
        decimal? @iditem
      ) {
        return (Row)Find(new object[] {
          @iditem
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
      /// IDItem: NUMERIC(14), Primary Key, Not Null
      /// </summary>
      public decimal? Iditem { get { return (decimal?)this["iditem"]; } set { this["iditem"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// FotoItem: IMAGE
      /// </summary>
      public byte[] Fotoitem { get { return (byte[])this["fotoitem"]; } set { this["fotoitem"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               decimal? @iditem) {
        return Insert(connection,
                      @iditem,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               decimal? @iditem,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_sm_fotoitem(),
          new string[] { "iditem" },
          new object[] { @iditem },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkIditem,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_sm_fotoitem(),
          new string[] { "iditem" },
          new object[] { pkIditem },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkIditem) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_sm_fotoitem(),
          new string[] { "iditem" },
          new object[] { pkIditem }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              decimal? pkIditem) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_sm_fotoitem(),
          new string[] { "iditem" },
          new object[] { pkIditem }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public decimal? Iditem { get { return (decimal?)row["iditem", DataRowVersion.Original]; } }
        public byte[] Fotoitem { get { return (byte[])row["fotoitem", DataRowVersion.Original]; } }

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
    #region Coluna: IDItem
    private ColumnIditem pvColIditem;

    public sealed class ColumnIditem : TDataColumn {

      public ColumnIditem() {
        this.ColumnName = "iditem";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "id";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fotoitem.iditem"; } }
      public override string OriginalCol { get { return "iditem"; } }
      public override string RefCol { get { return "LY_SM_FOTOITEM.IDItem"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 15; } }
      public override int Precision { get { return 14; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(14)"; } }

    }
    #endregion

    #region Coluna: FotoItem
    private ColumnFotoitem pvColFotoitem;

    public sealed class ColumnFotoitem : TDataColumn {

      public ColumnFotoitem() {
        this.ColumnName = "fotoitem";
        this.DataType = typeof(byte[]);

      }


      protected override string GetName(){return "foto";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fotoitem.fotoitem"; } }
      public override string OriginalCol { get { return "fotoitem"; } }
      public override string RefCol { get { return "LY_SM_FOTOITEM.FotoItem"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 0; } }
      public override string SqlServerType { get { return "IMAGE"; } }

    }
    #endregion
    #endregion
  }

  public partial class Ly_sm_h_fornecedor : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_sm_h_fornecedor() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_sm_h_fornecedor");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_sm_h_fornecedor");
      }
      catch{}
    }

    public Ly_sm_h_fornecedor() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColId_fornecedor = new ColumnId_fornecedor(); Columns.Add(pvColId_fornecedor);
      pvColNome = new ColumnNome(); Columns.Add(pvColNome);
      pvColCnpj = new ColumnCnpj(); Columns.Add(pvColCnpj);
      pvColCpf = new ColumnCpf(); Columns.Add(pvColCpf);
      pvColInscrest = new ColumnInscrest(); Columns.Add(pvColInscrest);
      pvColInscrmunic = new ColumnInscrmunic(); Columns.Add(pvColInscrmunic);
      pvColNum_origem = new ColumnNum_origem(); Columns.Add(pvColNum_origem);
      pvColEnd_mail = new ColumnEnd_mail(); Columns.Add(pvColEnd_mail);
      pvColEnd_web = new ColumnEnd_web(); Columns.Add(pvColEnd_web);
      pvColHabilitado = new ColumnHabilitado(); Columns.Add(pvColHabilitado);
      pvColMotivo_hab_inab = new ColumnMotivo_hab_inab(); Columns.Add(pvColMotivo_hab_inab);
      pvColAtividade = new ColumnAtividade(); Columns.Add(pvColAtividade);
      pvColTipo_servico = new ColumnTipo_servico(); Columns.Add(pvColTipo_servico);
      pvColCoordenadoria = new ColumnCoordenadoria(); Columns.Add(pvColCoordenadoria);
      pvColRepreslegal = new ColumnRepreslegal(); Columns.Add(pvColRepreslegal);
      pvColCpf_represlegal = new ColumnCpf_represlegal(); Columns.Add(pvColCpf_represlegal);
      pvColEndereco = new ColumnEndereco(); Columns.Add(pvColEndereco);
      pvColEnd_num = new ColumnEnd_num(); Columns.Add(pvColEnd_num);
      pvColEnd_compl = new ColumnEnd_compl(); Columns.Add(pvColEnd_compl);
      pvColEnd_municipio = new ColumnEnd_municipio(); Columns.Add(pvColEnd_municipio);
      pvColEnd_pais = new ColumnEnd_pais(); Columns.Add(pvColEnd_pais);
      pvColEnd_cep = new ColumnEnd_cep(); Columns.Add(pvColEnd_cep);
      pvColEnd_bairro = new ColumnEnd_bairro(); Columns.Add(pvColEnd_bairro);
      pvColCaixa_postal = new ColumnCaixa_postal(); Columns.Add(pvColCaixa_postal);
      pvColFone = new ColumnFone(); Columns.Add(pvColFone);
      pvColFax = new ColumnFax(); Columns.Add(pvColFax);
      pvColData_alteracao = new ColumnData_alteracao(); Columns.Add(pvColData_alteracao);
      pvColUsuario = new ColumnUsuario(); Columns.Add(pvColUsuario);
      pvColCelular = new ColumnCelular(); Columns.Add(pvColCelular);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_sm_h_fornecedor");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(decimal? @id_fornecedor, string @nome, string @habilitado, string @tipo_servico) {
      Row row = NewRow();

      row.Id_fornecedor = @id_fornecedor;
      row.Nome = @nome;
      row.Habilitado = @habilitado;
      row.Tipo_servico = @tipo_servico;

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
      get { return "LY_SM_H_FORNECEDOR"; }
    }

    protected override sealed string Name { get { return "Histórico de Fornecedores"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColId });
      pvColId.AllowDBNull = true;
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_sm_h_fornecedor_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_sm_h_fornecedor_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_sm_h_fornecedor_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_sm_h_fornecedor_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("id_fornecedor", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_FORNECEDOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("nome", OleDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "NOME", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("cnpj", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CNPJ", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("cpf", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CPF", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("inscrest", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "INSCREST", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("inscrmunic", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "INSCRMUNIC", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("num_origem", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NUM_ORIGEM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("end_mail", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "END_MAIL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("end_web", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "END_WEB", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("habilitado", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "HABILITADO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("motivo_hab_inab", OleDbType.VarChar, 2100, ParameterDirection.InputOutput, true, 0, 0, "MOTIVO_HAB_INAB", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("atividade", OleDbType.VarChar, 400, ParameterDirection.InputOutput, true, 0, 0, "ATIVIDADE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("tipo_servico", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TIPO_SERVICO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("coordenadoria", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "COORDENADORIA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("represlegal", OleDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "REPRESLEGAL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("cpf_represlegal", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CPF_REPRESLEGAL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("endereco", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "ENDERECO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("end_num", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "END_NUM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("end_compl", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "END_COMPL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("end_municipio", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "END_MUNICIPIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("end_pais", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "END_PAIS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("end_cep", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "END_CEP", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("end_bairro", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "END_BAIRRO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("caixa_postal", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CAIXA_POSTAL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("fone", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "FONE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("fax", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "FAX", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("data_alteracao", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DATA_ALTERACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("usuario", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "USUARIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("celular", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CELULAR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "id") { result.Add(toLower ? "id" : column); continue; }
          if(lower == "id_fornecedor") { result.Add(toLower ? "id_fornecedor" : column); continue; }
          if(lower == "nome") { result.Add(toLower ? "nome" : column); continue; }
          if(lower == "cnpj") { result.Add(toLower ? "cnpj" : column); continue; }
          if(lower == "cpf") { result.Add(toLower ? "cpf" : column); continue; }
          if(lower == "inscrest") { result.Add(toLower ? "inscrest" : column); continue; }
          if(lower == "inscrmunic") { result.Add(toLower ? "inscrmunic" : column); continue; }
          if(lower == "num_origem") { result.Add(toLower ? "num_origem" : column); continue; }
          if(lower == "end_mail") { result.Add(toLower ? "end_mail" : column); continue; }
          if(lower == "end_web") { result.Add(toLower ? "end_web" : column); continue; }
          if(lower == "habilitado") { result.Add(toLower ? "habilitado" : column); continue; }
          if(lower == "motivo_hab_inab") { result.Add(toLower ? "motivo_hab_inab" : column); continue; }
          if(lower == "atividade") { result.Add(toLower ? "atividade" : column); continue; }
          if(lower == "tipo_servico") { result.Add(toLower ? "tipo_servico" : column); continue; }
          if(lower == "coordenadoria") { result.Add(toLower ? "coordenadoria" : column); continue; }
          if(lower == "represlegal") { result.Add(toLower ? "represlegal" : column); continue; }
          if(lower == "cpf_represlegal") { result.Add(toLower ? "cpf_represlegal" : column); continue; }
          if(lower == "endereco") { result.Add(toLower ? "endereco" : column); continue; }
          if(lower == "end_num") { result.Add(toLower ? "end_num" : column); continue; }
          if(lower == "end_compl") { result.Add(toLower ? "end_compl" : column); continue; }
          if(lower == "end_municipio") { result.Add(toLower ? "end_municipio" : column); continue; }
          if(lower == "end_pais") { result.Add(toLower ? "end_pais" : column); continue; }
          if(lower == "end_cep") { result.Add(toLower ? "end_cep" : column); continue; }
          if(lower == "end_bairro") { result.Add(toLower ? "end_bairro" : column); continue; }
          if(lower == "caixa_postal") { result.Add(toLower ? "caixa_postal" : column); continue; }
          if(lower == "fone") { result.Add(toLower ? "fone" : column); continue; }
          if(lower == "fax") { result.Add(toLower ? "fax" : column); continue; }
          if(lower == "data_alteracao") { result.Add(toLower ? "data_alteracao" : column); continue; }
          if(lower == "usuario") { result.Add(toLower ? "usuario" : column); continue; }
          if(lower == "celular") { result.Add(toLower ? "celular" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "id_fornecedor", "nome", "cnpj", "cpf", "inscrest", "inscrmunic", 
                                           "num_origem", "end_mail", "end_web", "habilitado", "motivo_hab_inab", "atividade", 
                                           "tipo_servico", "coordenadoria", "represlegal", "cpf_represlegal", "endereco", "end_num", 
                                           "end_compl", "end_municipio", "end_pais", "end_cep", "end_bairro", "caixa_postal", 
                                           "fone", "fax", "data_alteracao", "usuario", "celular" }
                          : new string[] { "ID", "ID_FORNECEDOR", "NOME", "CNPJ", "CPF", "INSCREST", "INSCRMUNIC", 
                                           "NUM_ORIGEM", "END_MAIL", "END_WEB", "HABILITADO", "MOTIVO_HAB_INAB", "ATIVIDADE", 
                                           "TIPO_SERVICO", "COORDENADORIA", "REPRESLEGAL", "CPF_REPRESLEGAL", "ENDERECO", "END_NUM", 
                                           "END_COMPL", "END_MUNICIPIO", "END_PAIS", "END_CEP", "END_BAIRRO", "CAIXA_POSTAL", 
                                           "FONE", "FAX", "DATA_ALTERACAO", "USUARIO", "CELULAR" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "id_fornecedor", "nome", "cnpj", "cpf", "inscrest", "inscrmunic", 
                                           "num_origem", "end_mail", "end_web", "habilitado", "motivo_hab_inab", "atividade", 
                                           "tipo_servico", "coordenadoria", "represlegal", "cpf_represlegal", "endereco", "end_num", 
                                           "end_compl", "end_municipio", "end_pais", "end_cep", "end_bairro", "caixa_postal", 
                                           "fone", "fax", "data_alteracao", "usuario", "celular" }
                          : new string[] { "ID", "ID_FORNECEDOR", "NOME", "CNPJ", "CPF", "INSCREST", "INSCRMUNIC", 
                                           "NUM_ORIGEM", "END_MAIL", "END_WEB", "HABILITADO", "MOTIVO_HAB_INAB", "ATIVIDADE", 
                                           "TIPO_SERVICO", "COORDENADORIA", "REPRESLEGAL", "CPF_REPRESLEGAL", "ENDERECO", "END_NUM", 
                                           "END_COMPL", "END_MUNICIPIO", "END_PAIS", "END_CEP", "END_BAIRRO", "CAIXA_POSTAL", 
                                           "FONE", "FAX", "DATA_ALTERACAO", "USUARIO", "CELULAR" });

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
        "ly_sm_h_fornecedor.id"
      });
      if(scColumns.Contains("id_fornecedor")) list.Add("ly_sm_h_fornecedor.id_fornecedor");
      if(scColumns.Contains("nome")) list.Add("ly_sm_h_fornecedor.nome");
      if(scColumns.Contains("cnpj")) list.Add("ly_sm_h_fornecedor.cnpj");
      if(scColumns.Contains("cpf")) list.Add("ly_sm_h_fornecedor.cpf");
      if(scColumns.Contains("inscrest")) list.Add("ly_sm_h_fornecedor.inscrest");
      if(scColumns.Contains("inscrmunic")) list.Add("ly_sm_h_fornecedor.inscrmunic");
      if(scColumns.Contains("num_origem")) list.Add("ly_sm_h_fornecedor.num_origem");
      if(scColumns.Contains("end_mail")) list.Add("ly_sm_h_fornecedor.end_mail");
      if(scColumns.Contains("end_web")) list.Add("ly_sm_h_fornecedor.end_web");
      if(scColumns.Contains("habilitado")) list.Add("ly_sm_h_fornecedor.habilitado");
      if(scColumns.Contains("motivo_hab_inab")) list.Add("ly_sm_h_fornecedor.motivo_hab_inab");
      if(scColumns.Contains("atividade")) list.Add("ly_sm_h_fornecedor.atividade");
      if(scColumns.Contains("tipo_servico")) list.Add("ly_sm_h_fornecedor.tipo_servico");
      if(scColumns.Contains("coordenadoria")) list.Add("ly_sm_h_fornecedor.coordenadoria");
      if(scColumns.Contains("represlegal")) list.Add("ly_sm_h_fornecedor.represlegal");
      if(scColumns.Contains("cpf_represlegal")) list.Add("ly_sm_h_fornecedor.cpf_represlegal");
      if(scColumns.Contains("endereco")) list.Add("ly_sm_h_fornecedor.endereco");
      if(scColumns.Contains("end_num")) list.Add("ly_sm_h_fornecedor.end_num");
      if(scColumns.Contains("end_compl")) list.Add("ly_sm_h_fornecedor.end_compl");
      if(scColumns.Contains("end_municipio")) list.Add("ly_sm_h_fornecedor.end_municipio");
      if(scColumns.Contains("end_pais")) list.Add("ly_sm_h_fornecedor.end_pais");
      if(scColumns.Contains("end_cep")) list.Add("ly_sm_h_fornecedor.end_cep");
      if(scColumns.Contains("end_bairro")) list.Add("ly_sm_h_fornecedor.end_bairro");
      if(scColumns.Contains("caixa_postal")) list.Add("ly_sm_h_fornecedor.caixa_postal");
      if(scColumns.Contains("fone")) list.Add("ly_sm_h_fornecedor.fone");
      if(scColumns.Contains("fax")) list.Add("ly_sm_h_fornecedor.fax");
      if(scColumns.Contains("data_alteracao")) list.Add("ly_sm_h_fornecedor.data_alteracao");
      if(scColumns.Contains("usuario")) list.Add("ly_sm_h_fornecedor.usuario");
      if(scColumns.Contains("celular")) list.Add("ly_sm_h_fornecedor.celular");
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
          "FROM ly_sm_h_fornecedor " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_sm_h_fornecedor");
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
        da.TableMappings.Add("Table", "Ly_sm_h_fornecedor");
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
            if (dr["ID_FORNECEDOR"] != null && string.IsNullOrEmpty(dr["ID_FORNECEDOR"].ToString()))
              dr["ID_FORNECEDOR"] = DBNull.Value;
            if (dr["NOME"] != null && string.IsNullOrEmpty(dr["NOME"].ToString()))
              dr["NOME"] = DBNull.Value;
            if (dr["CNPJ"] != null && string.IsNullOrEmpty(dr["CNPJ"].ToString()))
              dr["CNPJ"] = DBNull.Value;
            if (dr["CPF"] != null && string.IsNullOrEmpty(dr["CPF"].ToString()))
              dr["CPF"] = DBNull.Value;
            if (dr["INSCREST"] != null && string.IsNullOrEmpty(dr["INSCREST"].ToString()))
              dr["INSCREST"] = DBNull.Value;
            if (dr["INSCRMUNIC"] != null && string.IsNullOrEmpty(dr["INSCRMUNIC"].ToString()))
              dr["INSCRMUNIC"] = DBNull.Value;
            if (dr["NUM_ORIGEM"] != null && string.IsNullOrEmpty(dr["NUM_ORIGEM"].ToString()))
              dr["NUM_ORIGEM"] = DBNull.Value;
            if (dr["END_MAIL"] != null && string.IsNullOrEmpty(dr["END_MAIL"].ToString()))
              dr["END_MAIL"] = DBNull.Value;
            if (dr["END_WEB"] != null && string.IsNullOrEmpty(dr["END_WEB"].ToString()))
              dr["END_WEB"] = DBNull.Value;
            if (dr["HABILITADO"] != null && string.IsNullOrEmpty(dr["HABILITADO"].ToString()))
              dr["HABILITADO"] = DBNull.Value;
            if (dr["MOTIVO_HAB_INAB"] != null && string.IsNullOrEmpty(dr["MOTIVO_HAB_INAB"].ToString()))
              dr["MOTIVO_HAB_INAB"] = DBNull.Value;
            if (dr["ATIVIDADE"] != null && string.IsNullOrEmpty(dr["ATIVIDADE"].ToString()))
              dr["ATIVIDADE"] = DBNull.Value;
            if (dr["TIPO_SERVICO"] != null && string.IsNullOrEmpty(dr["TIPO_SERVICO"].ToString()))
              dr["TIPO_SERVICO"] = DBNull.Value;
            if (dr["COORDENADORIA"] != null && string.IsNullOrEmpty(dr["COORDENADORIA"].ToString()))
              dr["COORDENADORIA"] = DBNull.Value;
            if (dr["REPRESLEGAL"] != null && string.IsNullOrEmpty(dr["REPRESLEGAL"].ToString()))
              dr["REPRESLEGAL"] = DBNull.Value;
            if (dr["CPF_REPRESLEGAL"] != null && string.IsNullOrEmpty(dr["CPF_REPRESLEGAL"].ToString()))
              dr["CPF_REPRESLEGAL"] = DBNull.Value;
            if (dr["ENDERECO"] != null && string.IsNullOrEmpty(dr["ENDERECO"].ToString()))
              dr["ENDERECO"] = DBNull.Value;
            if (dr["END_NUM"] != null && string.IsNullOrEmpty(dr["END_NUM"].ToString()))
              dr["END_NUM"] = DBNull.Value;
            if (dr["END_COMPL"] != null && string.IsNullOrEmpty(dr["END_COMPL"].ToString()))
              dr["END_COMPL"] = DBNull.Value;
            if (dr["END_MUNICIPIO"] != null && string.IsNullOrEmpty(dr["END_MUNICIPIO"].ToString()))
              dr["END_MUNICIPIO"] = DBNull.Value;
            if (dr["END_PAIS"] != null && string.IsNullOrEmpty(dr["END_PAIS"].ToString()))
              dr["END_PAIS"] = DBNull.Value;
            if (dr["END_CEP"] != null && string.IsNullOrEmpty(dr["END_CEP"].ToString()))
              dr["END_CEP"] = DBNull.Value;
            if (dr["END_BAIRRO"] != null && string.IsNullOrEmpty(dr["END_BAIRRO"].ToString()))
              dr["END_BAIRRO"] = DBNull.Value;
            if (dr["CAIXA_POSTAL"] != null && string.IsNullOrEmpty(dr["CAIXA_POSTAL"].ToString()))
              dr["CAIXA_POSTAL"] = DBNull.Value;
            if (dr["FONE"] != null && string.IsNullOrEmpty(dr["FONE"].ToString()))
              dr["FONE"] = DBNull.Value;
            if (dr["FAX"] != null && string.IsNullOrEmpty(dr["FAX"].ToString()))
              dr["FAX"] = DBNull.Value;
            if (dr["USUARIO"] != null && string.IsNullOrEmpty(dr["USUARIO"].ToString()))
              dr["USUARIO"] = DBNull.Value;
            if (dr["CELULAR"] != null && string.IsNullOrEmpty(dr["CELULAR"].ToString()))
              dr["CELULAR"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["ID_FORNECEDOR"] == null || string.IsNullOrEmpty(dr["ID_FORNECEDOR"].ToString()))
            {
              return "Favor informar o fornecedor.";
            }

            if (dr["ID_FORNECEDOR"] != null && !string.IsNullOrEmpty(dr["ID_FORNECEDOR"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_FORNECEDOR"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_FORNECEDOR"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_FORNECEDOR"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_FORNECEDOR"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O fornecedor deve ser um valor inteiro com 14 dígitos no máximo.";
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

            if (dr["CNPJ"] != null && !string.IsNullOrEmpty(dr["CNPJ"].ToString()))
            {
              if (dr["CNPJ"].ToString().Length > 19)
              {
                return "O cnpj deve possuir no máximo 19 caracteres.";
              }
            }

            if (dr["CPF"] != null && !string.IsNullOrEmpty(dr["CPF"].ToString()))
            {
              if (dr["CPF"].ToString().Length > 14)
              {
                return "O cpf deve possuir no máximo 14 caracteres.";
              }
            }

            if (dr["INSCREST"] != null && !string.IsNullOrEmpty(dr["INSCREST"].ToString()))
            {
              if (dr["INSCREST"].ToString().Length > 15)
              {
                return "A inscrição estadual deve possuir no máximo 15 caracteres.";
              }
            }

            if (dr["INSCRMUNIC"] != null && !string.IsNullOrEmpty(dr["INSCRMUNIC"].ToString()))
            {
              if (dr["INSCRMUNIC"].ToString().Length > 15)
              {
                return "A inscrição municipal deve possuir no máximo 15 caracteres.";
              }
            }

            if (dr["NUM_ORIGEM"] != null && !string.IsNullOrEmpty(dr["NUM_ORIGEM"].ToString()))
            {
              if (dr["NUM_ORIGEM"].ToString().Length > 50)
              {
                return "O número do origem deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["END_MAIL"] != null && !string.IsNullOrEmpty(dr["END_MAIL"].ToString()))
            {
              if (dr["END_MAIL"].ToString().Length > 100)
              {
                return "O e-mail deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["END_WEB"] != null && !string.IsNullOrEmpty(dr["END_WEB"].ToString()))
            {
              if (dr["END_WEB"].ToString().Length > 100)
              {
                return "O site web deve possuir no máximo 100 caracteres.";
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
            }

            if (dr["MOTIVO_HAB_INAB"] != null && !string.IsNullOrEmpty(dr["MOTIVO_HAB_INAB"].ToString()))
            {
              if (dr["MOTIVO_HAB_INAB"].ToString().Length > 2000)
              {
                return "O motivo para desabilitação deve possuir no máximo 2000 caracteres.";
              }
            }

            if (dr["ATIVIDADE"] != null && !string.IsNullOrEmpty(dr["ATIVIDADE"].ToString()))
            {
              if (dr["ATIVIDADE"].ToString().Length > 300)
              {
                return "A atividade deve possuir no máximo 300 caracteres.";
              }
            }

            if (dr["TIPO_SERVICO"] == null || string.IsNullOrEmpty(dr["TIPO_SERVICO"].ToString()))
            {
              return "Favor informar o tipo de serviço.";
            }

            if (dr["TIPO_SERVICO"] != null && !string.IsNullOrEmpty(dr["TIPO_SERVICO"].ToString()))
            {
              if (dr["TIPO_SERVICO"].ToString().Length > 40)
              {
                return "O tipo de serviço deve possuir no máximo 40 caracteres.";
              }
            }

            if (dr["COORDENADORIA"] != null && !string.IsNullOrEmpty(dr["COORDENADORIA"].ToString()))
            {
              if (dr["COORDENADORIA"].ToString().Length > 20)
              {
                return "A coordenadoria deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["REPRESLEGAL"] != null && !string.IsNullOrEmpty(dr["REPRESLEGAL"].ToString()))
            {
              if (dr["REPRESLEGAL"].ToString().Length > 200)
              {
                return "O representante legal deve possuir no máximo 200 caracteres.";
              }
            }

            if (dr["CPF_REPRESLEGAL"] != null && !string.IsNullOrEmpty(dr["CPF_REPRESLEGAL"].ToString()))
            {
              if (dr["CPF_REPRESLEGAL"].ToString().Length > 14)
              {
                return "O cpf do responsável legal deve possuir no máximo 14 caracteres.";
              }
            }

            if (dr["ENDERECO"] != null && !string.IsNullOrEmpty(dr["ENDERECO"].ToString()))
            {
              if (dr["ENDERECO"].ToString().Length > 50)
              {
                return "O endereço deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["END_NUM"] != null && !string.IsNullOrEmpty(dr["END_NUM"].ToString()))
            {
              if (dr["END_NUM"].ToString().Length > 15)
              {
                return "O número deve possuir no máximo 15 caracteres.";
              }
            }

            if (dr["END_COMPL"] != null && !string.IsNullOrEmpty(dr["END_COMPL"].ToString()))
            {
              if (dr["END_COMPL"].ToString().Length > 50)
              {
                return "O complemento deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["END_MUNICIPIO"] != null && !string.IsNullOrEmpty(dr["END_MUNICIPIO"].ToString()))
            {
              if (dr["END_MUNICIPIO"].ToString().Length > 20)
              {
                return "O município deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["END_PAIS"] != null && !string.IsNullOrEmpty(dr["END_PAIS"].ToString()))
            {
              if (dr["END_PAIS"].ToString().Length > 50)
              {
                return "O país deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["END_CEP"] != null && !string.IsNullOrEmpty(dr["END_CEP"].ToString()))
            {
              if (dr["END_CEP"].ToString().Length > 9)
              {
                return "O cep deve possuir no máximo 9 caracteres.";
              }
            }

            if (dr["END_BAIRRO"] != null && !string.IsNullOrEmpty(dr["END_BAIRRO"].ToString()))
            {
              if (dr["END_BAIRRO"].ToString().Length > 50)
              {
                return "O bairro deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["CAIXA_POSTAL"] != null && !string.IsNullOrEmpty(dr["CAIXA_POSTAL"].ToString()))
            {
              if (dr["CAIXA_POSTAL"].ToString().Length > 50)
              {
                return "A caixa postal deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["FONE"] != null && !string.IsNullOrEmpty(dr["FONE"].ToString()))
            {
              if (dr["FONE"].ToString().Length > 30)
              {
                return "O telefone deve possuir no máximo 30 caracteres.";
              }
            }

            if (dr["FAX"] != null && !string.IsNullOrEmpty(dr["FAX"].ToString()))
            {
              if (dr["FAX"].ToString().Length > 30)
              {
                return "O fax deve possuir no máximo 30 caracteres.";
              }
            }

            if (dr["DATA_ALTERACAO"] != null && dr["DATA_ALTERACAO"].ToString() != "")
            {
              dr["DATA_ALTERACAO"] = Convert.ToDateTime(dr["DATA_ALTERACAO"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["USUARIO"] != null && !string.IsNullOrEmpty(dr["USUARIO"].ToString()))
            {
              if (dr["USUARIO"].ToString().Length > 15)
              {
                return "O usuário deve possuir no máximo 15 caracteres.";
              }
            }

            if (dr["CELULAR"] != null && !string.IsNullOrEmpty(dr["CELULAR"].ToString()))
            {
              if (dr["CELULAR"].ToString().Length > 30)
              {
                return "O celular deve possuir no máximo 30 caracteres.";
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

    public static Ly_sm_h_fornecedor Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_sm_h_fornecedor Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_sm_h_fornecedor tab = new Ly_sm_h_fornecedor();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_sm_h_fornecedor tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, decimal? @id_fornecedor, string @nome, string @cnpj, string @cpf, string @inscrest, 
        string @inscrmunic, string @num_origem, string @end_mail, string @end_web, string @habilitado, 
        string @motivo_hab_inab, string @atividade, string @tipo_servico, string @coordenadoria, 
        string @represlegal, string @cpf_represlegal, string @endereco, string @end_num, 
        string @end_compl, string @end_municipio, string @end_pais, string @end_cep, string @end_bairro, 
        string @caixa_postal, string @fone, string @fax, DateTime? @data_alteracao, string @usuario, 
        string @celular
      ) {
        return (Row)Add(new object[] {
          @id, @id_fornecedor, @nome, @cnpj, @cpf, @inscrest, @inscrmunic, @num_origem, @end_mail, 
          @end_web, @habilitado, @motivo_hab_inab, @atividade, @tipo_servico, @coordenadoria, 
          @represlegal, @cpf_represlegal, @endereco, @end_num, @end_compl, @end_municipio, 
          @end_pais, @end_cep, @end_bairro, @caixa_postal, @fone, @fax, @data_alteracao, @usuario, 
          @celular
        });
      }

      public Row Find(
        decimal? @id
      ) {
        return (Row)Find(new object[] {
          @id
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
      /// ID: NUMERIC(14), Primary Key, AutoNumber, Not Null
      /// </summary>
      public decimal? Id { get { return (decimal?)this["id"]; } set { this["id"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ID_FORNECEDOR: NUMERIC(14), Not Null
      /// </summary>
      public decimal? Id_fornecedor { get { return (decimal?)this["id_fornecedor"]; } set { this["id_fornecedor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(200), Not Null
      /// </summary>
      public string Nome { get { return (string)this["nome"]; } set { this["nome"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CNPJ: VARCHAR(19)
      /// </summary>
      public string Cnpj { get { return (string)this["cnpj"]; } set { this["cnpj"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CPF: VARCHAR(14)
      /// </summary>
      public string Cpf { get { return (string)this["cpf"]; } set { this["cpf"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// INSCREST: VARCHAR(15)
      /// </summary>
      public string Inscrest { get { return (string)this["inscrest"]; } set { this["inscrest"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// INSCRMUNIC: VARCHAR(15)
      /// </summary>
      public string Inscrmunic { get { return (string)this["inscrmunic"]; } set { this["inscrmunic"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NUM_ORIGEM: VARCHAR(50)
      /// </summary>
      public string Num_origem { get { return (string)this["num_origem"]; } set { this["num_origem"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// END_MAIL: VARCHAR(100)
      /// </summary>
      public string End_mail { get { return (string)this["end_mail"]; } set { this["end_mail"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// END_WEB: VARCHAR(100)
      /// </summary>
      public string End_web { get { return (string)this["end_web"]; } set { this["end_web"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// HABILITADO: CHAR(1), Not Null
      /// </summary>
      public string Habilitado { get { return (string)this["habilitado"]; } set { this["habilitado"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// MOTIVO_HAB_INAB: VARCHAR(2000)
      /// </summary>
      public string Motivo_hab_inab { get { return (string)this["motivo_hab_inab"]; } set { this["motivo_hab_inab"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ATIVIDADE: VARCHAR(300)
      /// </summary>
      public string Atividade { get { return (string)this["atividade"]; } set { this["atividade"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TIPO_SERVICO: VARCHAR(40), Not Null
      /// </summary>
      public string Tipo_servico { get { return (string)this["tipo_servico"]; } set { this["tipo_servico"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// COORDENADORIA: VARCHAR(20)
      /// </summary>
      public string Coordenadoria { get { return (string)this["coordenadoria"]; } set { this["coordenadoria"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// REPRESLEGAL: VARCHAR(200)
      /// </summary>
      public string Represlegal { get { return (string)this["represlegal"]; } set { this["represlegal"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CPF_REPRESLEGAL: VARCHAR(14)
      /// </summary>
      public string Cpf_represlegal { get { return (string)this["cpf_represlegal"]; } set { this["cpf_represlegal"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ENDERECO: VARCHAR(50)
      /// </summary>
      public string Endereco { get { return (string)this["endereco"]; } set { this["endereco"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// END_NUM: VARCHAR(15)
      /// </summary>
      public string End_num { get { return (string)this["end_num"]; } set { this["end_num"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// END_COMPL: VARCHAR(50)
      /// </summary>
      public string End_compl { get { return (string)this["end_compl"]; } set { this["end_compl"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// END_MUNICIPIO: VARCHAR(20)
      /// </summary>
      public string End_municipio { get { return (string)this["end_municipio"]; } set { this["end_municipio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// END_PAIS: VARCHAR(50)
      /// </summary>
      public string End_pais { get { return (string)this["end_pais"]; } set { this["end_pais"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// END_CEP: VARCHAR(9)
      /// </summary>
      public string End_cep { get { return (string)this["end_cep"]; } set { this["end_cep"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// END_BAIRRO: VARCHAR(50)
      /// </summary>
      public string End_bairro { get { return (string)this["end_bairro"]; } set { this["end_bairro"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CAIXA_POSTAL: VARCHAR(50)
      /// </summary>
      public string Caixa_postal { get { return (string)this["caixa_postal"]; } set { this["caixa_postal"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// FONE: VARCHAR(30)
      /// </summary>
      public string Fone { get { return (string)this["fone"]; } set { this["fone"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// FAX: VARCHAR(30)
      /// </summary>
      public string Fax { get { return (string)this["fax"]; } set { this["fax"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DATA_ALTERACAO: DATETIME
      /// </summary>
      public DateTime? Data_alteracao { get { return (DateTime?)this["data_alteracao"]; } set { this["data_alteracao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// USUARIO: VARCHAR(15)
      /// </summary>
      public string Usuario { get { return (string)this["usuario"]; } set { this["usuario"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CELULAR: VARCHAR(30)
      /// </summary>
      public string Celular { get { return (string)this["celular"]; } set { this["celular"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               decimal? @id_fornecedor, string @nome, string @habilitado, string @tipo_servico) {
        return Insert(connection,
                      @id_fornecedor, @nome, @habilitado, @tipo_servico,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               decimal? @id_fornecedor, string @nome, string @habilitado, string @tipo_servico,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_sm_h_fornecedor(),
          new string[] { "id_fornecedor", "nome", "habilitado", "tipo_servico" },
          new object[] { @id_fornecedor, @nome, @habilitado, @tipo_servico },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_sm_h_fornecedor(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_sm_h_fornecedor(),
          new string[] { "id" },
          new object[] { pkId }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              decimal? pkId) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_sm_h_fornecedor(),
          new string[] { "id" },
          new object[] { pkId }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public decimal? Id { get { return (decimal?)row["id", DataRowVersion.Original]; } }
        public decimal? Id_fornecedor { get { return (decimal?)row["id_fornecedor", DataRowVersion.Original]; } }
        public string Nome { get { return (string)row["nome", DataRowVersion.Original]; } }
        public string Cnpj { get { return (string)row["cnpj", DataRowVersion.Original]; } }
        public string Cpf { get { return (string)row["cpf", DataRowVersion.Original]; } }
        public string Inscrest { get { return (string)row["inscrest", DataRowVersion.Original]; } }
        public string Inscrmunic { get { return (string)row["inscrmunic", DataRowVersion.Original]; } }
        public string Num_origem { get { return (string)row["num_origem", DataRowVersion.Original]; } }
        public string End_mail { get { return (string)row["end_mail", DataRowVersion.Original]; } }
        public string End_web { get { return (string)row["end_web", DataRowVersion.Original]; } }
        public string Habilitado { get { return (string)row["habilitado", DataRowVersion.Original]; } }
        public string Motivo_hab_inab { get { return (string)row["motivo_hab_inab", DataRowVersion.Original]; } }
        public string Atividade { get { return (string)row["atividade", DataRowVersion.Original]; } }
        public string Tipo_servico { get { return (string)row["tipo_servico", DataRowVersion.Original]; } }
        public string Coordenadoria { get { return (string)row["coordenadoria", DataRowVersion.Original]; } }
        public string Represlegal { get { return (string)row["represlegal", DataRowVersion.Original]; } }
        public string Cpf_represlegal { get { return (string)row["cpf_represlegal", DataRowVersion.Original]; } }
        public string Endereco { get { return (string)row["endereco", DataRowVersion.Original]; } }
        public string End_num { get { return (string)row["end_num", DataRowVersion.Original]; } }
        public string End_compl { get { return (string)row["end_compl", DataRowVersion.Original]; } }
        public string End_municipio { get { return (string)row["end_municipio", DataRowVersion.Original]; } }
        public string End_pais { get { return (string)row["end_pais", DataRowVersion.Original]; } }
        public string End_cep { get { return (string)row["end_cep", DataRowVersion.Original]; } }
        public string End_bairro { get { return (string)row["end_bairro", DataRowVersion.Original]; } }
        public string Caixa_postal { get { return (string)row["caixa_postal", DataRowVersion.Original]; } }
        public string Fone { get { return (string)row["fone", DataRowVersion.Original]; } }
        public string Fax { get { return (string)row["fax", DataRowVersion.Original]; } }
        public DateTime? Data_alteracao { get { return (DateTime?)row["data_alteracao", DataRowVersion.Original]; } }
        public string Usuario { get { return (string)row["usuario", DataRowVersion.Original]; } }
        public string Celular { get { return (string)row["celular", DataRowVersion.Original]; } }

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
    #region Coluna: ID
    private ColumnId pvColId;

    public sealed class ColumnId : TDataColumn {

      public ColumnId() {
        this.ColumnName = "id";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "id";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.ID"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 15; } }
      public override int Precision { get { return 14; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(14)"; } }

    }
    #endregion

    #region Coluna: ID_FORNECEDOR
    private ColumnId_fornecedor pvColId_fornecedor;

    public sealed class ColumnId_fornecedor : TDataColumn {

      public ColumnId_fornecedor() {
        this.ColumnName = "id_fornecedor";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "fornecedor";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.id_fornecedor"; } }
      public override string OriginalCol { get { return "id_fornecedor"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.ID_FORNECEDOR"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 15; } }
      public override int Precision { get { return 14; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(14)"; } }

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
      public override string FullCol { get { return "ly_sm_h_fornecedor.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.NOME"; } }
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

    #region Coluna: CNPJ
    private ColumnCnpj pvColCnpj;

    public sealed class ColumnCnpj : TDataColumn {

      public ColumnCnpj() {
        this.ColumnName = "cnpj";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "cnpj";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.cnpj"; } }
      public override string OriginalCol { get { return "cnpj"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.CNPJ"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 19; } }
      public override string SqlServerType { get { return "VARCHAR(19)"; } }

    }
    #endregion

    #region Coluna: CPF
    private ColumnCpf pvColCpf;

    public sealed class ColumnCpf : TDataColumn {

      public ColumnCpf() {
        this.ColumnName = "cpf";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "cpf";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.cpf"; } }
      public override string OriginalCol { get { return "cpf"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.CPF"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 14; } }
      public override string SqlServerType { get { return "VARCHAR(14)"; } }

    }
    #endregion

    #region Coluna: INSCREST
    private ColumnInscrest pvColInscrest;

    public sealed class ColumnInscrest : TDataColumn {

      public ColumnInscrest() {
        this.ColumnName = "inscrest";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "inscrição estadual";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.inscrest"; } }
      public override string OriginalCol { get { return "inscrest"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.INSCREST"; } }
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

    #region Coluna: INSCRMUNIC
    private ColumnInscrmunic pvColInscrmunic;

    public sealed class ColumnInscrmunic : TDataColumn {

      public ColumnInscrmunic() {
        this.ColumnName = "inscrmunic";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "inscrição municipal";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.inscrmunic"; } }
      public override string OriginalCol { get { return "inscrmunic"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.INSCRMUNIC"; } }
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

    #region Coluna: NUM_ORIGEM
    private ColumnNum_origem pvColNum_origem;

    public sealed class ColumnNum_origem : TDataColumn {

      public ColumnNum_origem() {
        this.ColumnName = "num_origem";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "número do origem";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.num_origem"; } }
      public override string OriginalCol { get { return "num_origem"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.NUM_ORIGEM"; } }
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

    #region Coluna: END_MAIL
    private ColumnEnd_mail pvColEnd_mail;

    public sealed class ColumnEnd_mail : TDataColumn {

      public ColumnEnd_mail() {
        this.ColumnName = "end_mail";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "e-mail";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.end_mail"; } }
      public override string OriginalCol { get { return "end_mail"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.END_MAIL"; } }
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

    #region Coluna: END_WEB
    private ColumnEnd_web pvColEnd_web;

    public sealed class ColumnEnd_web : TDataColumn {

      public ColumnEnd_web() {
        this.ColumnName = "end_web";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "site web";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.end_web"; } }
      public override string OriginalCol { get { return "end_web"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.END_WEB"; } }
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
      public override string FullCol { get { return "ly_sm_h_fornecedor.habilitado"; } }
      public override string OriginalCol { get { return "habilitado"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.HABILITADO"; } }
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

    #region Coluna: MOTIVO_HAB_INAB
    private ColumnMotivo_hab_inab pvColMotivo_hab_inab;

    public sealed class ColumnMotivo_hab_inab : TDataColumn {

      public ColumnMotivo_hab_inab() {
        this.ColumnName = "motivo_hab_inab";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "motivo para desabilitação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.motivo_hab_inab"; } }
      public override string OriginalCol { get { return "motivo_hab_inab"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.MOTIVO_HAB_INAB"; } }
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

    #region Coluna: ATIVIDADE
    private ColumnAtividade pvColAtividade;

    public sealed class ColumnAtividade : TDataColumn {

      public ColumnAtividade() {
        this.ColumnName = "atividade";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "atividade";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.atividade"; } }
      public override string OriginalCol { get { return "atividade"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.ATIVIDADE"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 300; } }
      public override string SqlServerType { get { return "VARCHAR(300)"; } }

    }
    #endregion

    #region Coluna: TIPO_SERVICO
    private ColumnTipo_servico pvColTipo_servico;

    public sealed class ColumnTipo_servico : TDataColumn {

      public ColumnTipo_servico() {
        this.ColumnName = "tipo_servico";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "tipo de serviço";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.tipo_servico"; } }
      public override string OriginalCol { get { return "tipo_servico"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.TIPO_SERVICO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 40; } }
      public override string SqlServerType { get { return "VARCHAR(40)"; } }

    }
    #endregion

    #region Coluna: COORDENADORIA
    private ColumnCoordenadoria pvColCoordenadoria;

    public sealed class ColumnCoordenadoria : TDataColumn {

      public ColumnCoordenadoria() {
        this.ColumnName = "coordenadoria";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "coordenadoria";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.coordenadoria"; } }
      public override string OriginalCol { get { return "coordenadoria"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.COORDENADORIA"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 20; } }
      public override string SqlServerType { get { return "VARCHAR(20)"; } }

    }
    #endregion

    #region Coluna: REPRESLEGAL
    private ColumnRepreslegal pvColRepreslegal;

    public sealed class ColumnRepreslegal : TDataColumn {

      public ColumnRepreslegal() {
        this.ColumnName = "represlegal";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "representante legal";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.represlegal"; } }
      public override string OriginalCol { get { return "represlegal"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.REPRESLEGAL"; } }
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

    #region Coluna: CPF_REPRESLEGAL
    private ColumnCpf_represlegal pvColCpf_represlegal;

    public sealed class ColumnCpf_represlegal : TDataColumn {

      public ColumnCpf_represlegal() {
        this.ColumnName = "cpf_represlegal";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "cpf do responsável legal";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.cpf_represlegal"; } }
      public override string OriginalCol { get { return "cpf_represlegal"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.CPF_REPRESLEGAL"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 14; } }
      public override string SqlServerType { get { return "VARCHAR(14)"; } }

    }
    #endregion

    #region Coluna: ENDERECO
    private ColumnEndereco pvColEndereco;

    public sealed class ColumnEndereco : TDataColumn {

      public ColumnEndereco() {
        this.ColumnName = "endereco";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "endereço";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.endereco"; } }
      public override string OriginalCol { get { return "endereco"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.ENDERECO"; } }
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

    #region Coluna: END_NUM
    private ColumnEnd_num pvColEnd_num;

    public sealed class ColumnEnd_num : TDataColumn {

      public ColumnEnd_num() {
        this.ColumnName = "end_num";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "número";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.end_num"; } }
      public override string OriginalCol { get { return "end_num"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.END_NUM"; } }
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

    #region Coluna: END_COMPL
    private ColumnEnd_compl pvColEnd_compl;

    public sealed class ColumnEnd_compl : TDataColumn {

      public ColumnEnd_compl() {
        this.ColumnName = "end_compl";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "complemento";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.end_compl"; } }
      public override string OriginalCol { get { return "end_compl"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.END_COMPL"; } }
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

    #region Coluna: END_MUNICIPIO
    private ColumnEnd_municipio pvColEnd_municipio;

    public sealed class ColumnEnd_municipio : TDataColumn {

      public ColumnEnd_municipio() {
        this.ColumnName = "end_municipio";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "município";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.end_municipio"; } }
      public override string OriginalCol { get { return "end_municipio"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.END_MUNICIPIO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 20; } }
      public override string SqlServerType { get { return "VARCHAR(20)"; } }

    }
    #endregion

    #region Coluna: END_PAIS
    private ColumnEnd_pais pvColEnd_pais;

    public sealed class ColumnEnd_pais : TDataColumn {

      public ColumnEnd_pais() {
        this.ColumnName = "end_pais";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "país";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.end_pais"; } }
      public override string OriginalCol { get { return "end_pais"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.END_PAIS"; } }
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

    #region Coluna: END_CEP
    private ColumnEnd_cep pvColEnd_cep;

    public sealed class ColumnEnd_cep : TDataColumn {

      public ColumnEnd_cep() {
        this.ColumnName = "end_cep";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "cep";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.end_cep"; } }
      public override string OriginalCol { get { return "end_cep"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.END_CEP"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 9; } }
      public override string SqlServerType { get { return "VARCHAR(9)"; } }

    }
    #endregion

    #region Coluna: END_BAIRRO
    private ColumnEnd_bairro pvColEnd_bairro;

    public sealed class ColumnEnd_bairro : TDataColumn {

      public ColumnEnd_bairro() {
        this.ColumnName = "end_bairro";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "bairro";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.end_bairro"; } }
      public override string OriginalCol { get { return "end_bairro"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.END_BAIRRO"; } }
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

    #region Coluna: CAIXA_POSTAL
    private ColumnCaixa_postal pvColCaixa_postal;

    public sealed class ColumnCaixa_postal : TDataColumn {

      public ColumnCaixa_postal() {
        this.ColumnName = "caixa_postal";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "caixa postal";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.caixa_postal"; } }
      public override string OriginalCol { get { return "caixa_postal"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.CAIXA_POSTAL"; } }
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

    #region Coluna: FONE
    private ColumnFone pvColFone;

    public sealed class ColumnFone : TDataColumn {

      public ColumnFone() {
        this.ColumnName = "fone";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "telefone";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.fone"; } }
      public override string OriginalCol { get { return "fone"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.FONE"; } }
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

    #region Coluna: FAX
    private ColumnFax pvColFax;

    public sealed class ColumnFax : TDataColumn {

      public ColumnFax() {
        this.ColumnName = "fax";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "fax";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.fax"; } }
      public override string OriginalCol { get { return "fax"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.FAX"; } }
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

    #region Coluna: DATA_ALTERACAO
    private ColumnData_alteracao pvColData_alteracao;

    public sealed class ColumnData_alteracao : TDataColumn {

      public ColumnData_alteracao() {
        this.ColumnName = "data_alteracao";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data de alteração";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.data_alteracao"; } }
      public override string OriginalCol { get { return "data_alteracao"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.DATA_ALTERACAO"; } }
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
      public override string FullCol { get { return "ly_sm_h_fornecedor.usuario"; } }
      public override string OriginalCol { get { return "usuario"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.USUARIO"; } }
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

    #region Coluna: CELULAR
    private ColumnCelular pvColCelular;

    public sealed class ColumnCelular : TDataColumn {

      public ColumnCelular() {
        this.ColumnName = "celular";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "celular";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_h_fornecedor.celular"; } }
      public override string OriginalCol { get { return "celular"; } }
      public override string RefCol { get { return "LY_SM_H_FORNECEDOR.CELULAR"; } }
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

  public partial class Ly_sm_itens : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_sm_itens() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_sm_itens");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_sm_itens");
      }
      catch{}
    }

    public Ly_sm_itens() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColItemid = new ColumnItemid(); Columns.Add(pvColItemid);
      pvColNome = new ColumnNome(); Columns.Add(pvColNome);
      pvColEspecif = new ColumnEspecif(); Columns.Add(pvColEspecif);
      pvColTipo_patr = new ColumnTipo_patr(); Columns.Add(pvColTipo_patr);
      pvColNum_antigo = new ColumnNum_antigo(); Columns.Add(pvColNum_antigo);
      pvColFlg_desuso = new ColumnFlg_desuso(); Columns.Add(pvColFlg_desuso);
      pvColUnidade = new ColumnUnidade(); Columns.Add(pvColUnidade);
      pvColItempai = new ColumnItempai(); Columns.Add(pvColItempai);
      pvColFlg_pnae = new ColumnFlg_pnae(); Columns.Add(pvColFlg_pnae);
      pvColDesconto = new ColumnDesconto(); Columns.Add(pvColDesconto);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_sm_itens");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @itemid, string @nome, string @flg_desuso, string @flg_pnae) {
      Row row = NewRow();

      row.Itemid = @itemid;
      row.Nome = @nome;
      row.Flg_desuso = @flg_desuso;
      row.Flg_pnae = @flg_pnae;

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
      get { return "LY_SM_ITENS"; }
    }

    protected override sealed string Name { get { return "Item"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColId });
      pvColId.AllowDBNull = true;
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_sm_itens_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_sm_itens_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_sm_itens_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_sm_itens_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("itemid", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "ITEMID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("nome", OleDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "NOME", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("especif", OleDbType.VarChar, 2100, ParameterDirection.InputOutput, true, 0, 0, "ESPECIF", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("tipo_patr", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TIPO_PATR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("num_antigo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NUM_ANTIGO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("flg_desuso", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "FLG_DESUSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("unidade", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "UNIDADE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("itempai", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ITEMPAI", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("flg_pnae", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "FLG_PNAE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("desconto", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "DESCONTO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));

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
          if(lower == "id") { result.Add(toLower ? "id" : column); continue; }
          if(lower == "itemid") { result.Add(toLower ? "itemid" : column); continue; }
          if(lower == "nome") { result.Add(toLower ? "nome" : column); continue; }
          if(lower == "especif") { result.Add(toLower ? "especif" : column); continue; }
          if(lower == "tipo_patr") { result.Add(toLower ? "tipo_patr" : column); continue; }
          if(lower == "num_antigo") { result.Add(toLower ? "num_antigo" : column); continue; }
          if(lower == "flg_desuso") { result.Add(toLower ? "flg_desuso" : column); continue; }
          if(lower == "unidade") { result.Add(toLower ? "unidade" : column); continue; }
          if(lower == "itempai") { result.Add(toLower ? "itempai" : column); continue; }
          if(lower == "flg_pnae") { result.Add(toLower ? "flg_pnae" : column); continue; }
          if(lower == "desconto") { result.Add(toLower ? "desconto" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "itemid", "nome", "especif", "tipo_patr", "num_antigo", "flg_desuso", 
                                           "unidade", "itempai", "flg_pnae", "desconto" }
                          : new string[] { "ID", "ITEMID", "NOME", "ESPECIF", "TIPO_PATR", "NUM_ANTIGO", "FLG_DESUSO", 
                                           "UNIDADE", "ITEMPAI", "FLG_PNAE", "DESCONTO" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "itemid", "nome", "especif", "tipo_patr", "num_antigo", "flg_desuso", 
                                           "unidade", "itempai", "flg_pnae", "desconto" }
                          : new string[] { "ID", "ITEMID", "NOME", "ESPECIF", "TIPO_PATR", "NUM_ANTIGO", "FLG_DESUSO", 
                                           "UNIDADE", "ITEMPAI", "FLG_PNAE", "DESCONTO" });

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
        "ly_sm_itens.id"
      });
      if(scColumns.Contains("itemid")) list.Add("ly_sm_itens.itemid");
      if(scColumns.Contains("nome")) list.Add("ly_sm_itens.nome");
      if(scColumns.Contains("especif")) list.Add("ly_sm_itens.especif");
      if(scColumns.Contains("tipo_patr")) list.Add("ly_sm_itens.tipo_patr");
      if(scColumns.Contains("num_antigo")) list.Add("ly_sm_itens.num_antigo");
      if(scColumns.Contains("flg_desuso")) list.Add("ly_sm_itens.flg_desuso");
      if(scColumns.Contains("unidade")) list.Add("ly_sm_itens.unidade");
      if(scColumns.Contains("itempai")) list.Add("ly_sm_itens.itempai");
      if(scColumns.Contains("flg_pnae")) list.Add("ly_sm_itens.flg_pnae");
      if(scColumns.Contains("desconto")) list.Add("ly_sm_itens.desconto");
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
          "FROM ly_sm_itens " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_sm_itens");
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
        da.TableMappings.Add("Table", "Ly_sm_itens");
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
            if (dr["ITEMID"] != null && string.IsNullOrEmpty(dr["ITEMID"].ToString()))
              dr["ITEMID"] = DBNull.Value;
            if (dr["NOME"] != null && string.IsNullOrEmpty(dr["NOME"].ToString()))
              dr["NOME"] = DBNull.Value;
            if (dr["ESPECIF"] != null && string.IsNullOrEmpty(dr["ESPECIF"].ToString()))
              dr["ESPECIF"] = DBNull.Value;
            if (dr["TIPO_PATR"] != null && string.IsNullOrEmpty(dr["TIPO_PATR"].ToString()))
              dr["TIPO_PATR"] = DBNull.Value;
            if (dr["NUM_ANTIGO"] != null && string.IsNullOrEmpty(dr["NUM_ANTIGO"].ToString()))
              dr["NUM_ANTIGO"] = DBNull.Value;
            if (dr["FLG_DESUSO"] != null && string.IsNullOrEmpty(dr["FLG_DESUSO"].ToString()))
              dr["FLG_DESUSO"] = DBNull.Value;
            if (dr["UNIDADE"] != null && string.IsNullOrEmpty(dr["UNIDADE"].ToString()))
              dr["UNIDADE"] = DBNull.Value;
            if (dr["ITEMPAI"] != null && string.IsNullOrEmpty(dr["ITEMPAI"].ToString()))
              dr["ITEMPAI"] = DBNull.Value;
            if (dr["FLG_PNAE"] != null && string.IsNullOrEmpty(dr["FLG_PNAE"].ToString()))
              dr["FLG_PNAE"] = DBNull.Value;
            if (dr["DESCONTO"] != null && string.IsNullOrEmpty(dr["DESCONTO"].ToString()))
              dr["DESCONTO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["ITEMID"] == null || string.IsNullOrEmpty(dr["ITEMID"].ToString()))
            {
              return "Favor informar o código.";
            }

            if (dr["ITEMID"] != null && !string.IsNullOrEmpty(dr["ITEMID"].ToString()))
            {
              if (dr["ITEMID"].ToString().Length > 100)
              {
                return "O código deve possuir no máximo 100 caracteres.";
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

            if (dr["ESPECIF"] != null && !string.IsNullOrEmpty(dr["ESPECIF"].ToString()))
            {
              if (dr["ESPECIF"].ToString().Length > 2000)
              {
                return "A especificação deve possuir no máximo 2000 caracteres.";
              }
            }

            if (dr["TIPO_PATR"] != null && !string.IsNullOrEmpty(dr["TIPO_PATR"].ToString()))
            {
              if (dr["TIPO_PATR"].ToString().Length > 40)
              {
                return "O tipo de patrimônio deve possuir no máximo 40 caracteres.";
              }
            }

            if (dr["NUM_ANTIGO"] != null && !string.IsNullOrEmpty(dr["NUM_ANTIGO"].ToString()))
            {
              if (dr["NUM_ANTIGO"].ToString().Length > 100)
              {
                return "O número antigo deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["FLG_DESUSO"] == null || string.IsNullOrEmpty(dr["FLG_DESUSO"].ToString()))
            {
              return "Favor informar o desuso.";
            }

            if (dr["FLG_DESUSO"] != null && !string.IsNullOrEmpty(dr["FLG_DESUSO"].ToString()))
            {
              if (dr["FLG_DESUSO"].ToString().Length > 1)
              {
                return "O desuso deve possuir no máximo 1 caractere.";
              }
            }

            if (dr["UNIDADE"] != null && !string.IsNullOrEmpty(dr["UNIDADE"].ToString()))
            {
              if ((Convert.ToDecimal(dr["UNIDADE"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["UNIDADE"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["UNIDADE"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["UNIDADE"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A unidade deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["ITEMPAI"] != null && !string.IsNullOrEmpty(dr["ITEMPAI"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ITEMPAI"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ITEMPAI"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ITEMPAI"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ITEMPAI"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O item pai deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["FLG_PNAE"] == null || string.IsNullOrEmpty(dr["FLG_PNAE"].ToString()))
            {
              return "Favor informar o pnae.";
            }

            if (dr["FLG_PNAE"] != null && !string.IsNullOrEmpty(dr["FLG_PNAE"].ToString()))
            {
              if (dr["FLG_PNAE"].ToString().Length > 1)
              {
                return "O pnae deve possuir no máximo 1 caractere.";
              }
            }

            if (dr["DESCONTO"] != null && !string.IsNullOrEmpty(dr["DESCONTO"].ToString()))
            {
              if (dr["DESCONTO"].ToString().Length > 1)
              {
                return "O desconto deve possuir no máximo 1 caractere.";
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

    public static Ly_sm_itens Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_sm_itens Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_sm_itens tab = new Ly_sm_itens();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_sm_itens tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, string @itemid, string @nome, string @especif, string @tipo_patr, string @num_antigo, 
        string @flg_desuso, decimal? @unidade, decimal? @itempai, string @flg_pnae, string @desconto
      ) {
        return (Row)Add(new object[] {
          @id, @itemid, @nome, @especif, @tipo_patr, @num_antigo, @flg_desuso, @unidade, @itempai, 
          @flg_pnae, @desconto
        });
      }

      public Row Find(
        decimal? @id
      ) {
        return (Row)Find(new object[] {
          @id
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
      /// ID: NUMERIC(14), Primary Key, AutoNumber, Not Null
      /// </summary>
      public decimal? Id { get { return (decimal?)this["id"]; } set { this["id"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ITEMID: VARCHAR(100), Not Null
      /// </summary>
      public string Itemid { get { return (string)this["itemid"]; } set { this["itemid"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(200), Not Null
      /// </summary>
      public string Nome { get { return (string)this["nome"]; } set { this["nome"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ESPECIF: VARCHAR(2000)
      /// </summary>
      public string Especif { get { return (string)this["especif"]; } set { this["especif"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TIPO_PATR: VARCHAR(40)
      /// </summary>
      public string Tipo_patr { get { return (string)this["tipo_patr"]; } set { this["tipo_patr"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NUM_ANTIGO: VARCHAR(100)
      /// </summary>
      public string Num_antigo { get { return (string)this["num_antigo"]; } set { this["num_antigo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// FLG_DESUSO: VARCHAR(1), Not Null
      /// </summary>
      public string Flg_desuso { get { return (string)this["flg_desuso"]; } set { this["flg_desuso"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// UNIDADE: NUMERIC(14)
      /// </summary>
      public decimal? Unidade { get { return (decimal?)this["unidade"]; } set { this["unidade"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ITEMPAI: NUMERIC(14)
      /// </summary>
      public decimal? Itempai { get { return (decimal?)this["itempai"]; } set { this["itempai"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// FLG_PNAE: CHAR(1), Not Null
      /// </summary>
      public string Flg_pnae { get { return (string)this["flg_pnae"]; } set { this["flg_pnae"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCONTO: CHAR(1)
      /// </summary>
      public string Desconto { get { return (string)this["desconto"]; } set { this["desconto"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @itemid, string @nome, string @flg_desuso, string @flg_pnae) {
        return Insert(connection,
                      @itemid, @nome, @flg_desuso, @flg_pnae,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @itemid, string @nome, string @flg_desuso, string @flg_pnae,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_sm_itens(),
          new string[] { "itemid", "nome", "flg_desuso", "flg_pnae" },
          new object[] { @itemid, @nome, @flg_desuso, @flg_pnae },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_sm_itens(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_sm_itens(),
          new string[] { "id" },
          new object[] { pkId }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              decimal? pkId) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_sm_itens(),
          new string[] { "id" },
          new object[] { pkId }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public decimal? Id { get { return (decimal?)row["id", DataRowVersion.Original]; } }
        public string Itemid { get { return (string)row["itemid", DataRowVersion.Original]; } }
        public string Nome { get { return (string)row["nome", DataRowVersion.Original]; } }
        public string Especif { get { return (string)row["especif", DataRowVersion.Original]; } }
        public string Tipo_patr { get { return (string)row["tipo_patr", DataRowVersion.Original]; } }
        public string Num_antigo { get { return (string)row["num_antigo", DataRowVersion.Original]; } }
        public string Flg_desuso { get { return (string)row["flg_desuso", DataRowVersion.Original]; } }
        public decimal? Unidade { get { return (decimal?)row["unidade", DataRowVersion.Original]; } }
        public decimal? Itempai { get { return (decimal?)row["itempai", DataRowVersion.Original]; } }
        public string Flg_pnae { get { return (string)row["flg_pnae", DataRowVersion.Original]; } }
        public string Desconto { get { return (string)row["desconto", DataRowVersion.Original]; } }

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
    #region Coluna: ID
    private ColumnId pvColId;

    public sealed class ColumnId : TDataColumn {

      public ColumnId() {
        this.ColumnName = "id";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "id";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_itens.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_SM_ITENS.ID"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 15; } }
      public override int Precision { get { return 14; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(14)"; } }

    }
    #endregion

    #region Coluna: ITEMID
    private ColumnItemid pvColItemid;

    public sealed class ColumnItemid : TDataColumn {

      public ColumnItemid() {
        this.ColumnName = "itemid";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "código";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_itens.itemid"; } }
      public override string OriginalCol { get { return "itemid"; } }
      public override string RefCol { get { return "LY_SM_ITENS.ITEMID"; } }
      public override bool PrimaryKey { get { return false; } }
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
      public override string FullCol { get { return "ly_sm_itens.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "LY_SM_ITENS.NOME"; } }
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

    #region Coluna: ESPECIF
    private ColumnEspecif pvColEspecif;

    public sealed class ColumnEspecif : TDataColumn {

      public ColumnEspecif() {
        this.ColumnName = "especif";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "especificação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_itens.especif"; } }
      public override string OriginalCol { get { return "especif"; } }
      public override string RefCol { get { return "LY_SM_ITENS.ESPECIF"; } }
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

    #region Coluna: TIPO_PATR
    private ColumnTipo_patr pvColTipo_patr;

    public sealed class ColumnTipo_patr : TDataColumn {

      public ColumnTipo_patr() {
        this.ColumnName = "tipo_patr";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "tipo de patrimônio";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_itens.tipo_patr"; } }
      public override string OriginalCol { get { return "tipo_patr"; } }
      public override string RefCol { get { return "LY_SM_ITENS.TIPO_PATR"; } }
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

    #region Coluna: NUM_ANTIGO
    private ColumnNum_antigo pvColNum_antigo;

    public sealed class ColumnNum_antigo : TDataColumn {

      public ColumnNum_antigo() {
        this.ColumnName = "num_antigo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "número antigo";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_itens.num_antigo"; } }
      public override string OriginalCol { get { return "num_antigo"; } }
      public override string RefCol { get { return "LY_SM_ITENS.NUM_ANTIGO"; } }
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

    #region Coluna: FLG_DESUSO
    private ColumnFlg_desuso pvColFlg_desuso;

    public sealed class ColumnFlg_desuso : TDataColumn {

      public ColumnFlg_desuso() {
        this.ColumnName = "flg_desuso";
        this.DataType = typeof(string);
        this.DefaultValue = "N";

      }


      protected override string GetName(){return "desuso";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_itens.flg_desuso"; } }
      public override string OriginalCol { get { return "flg_desuso"; } }
      public override string RefCol { get { return "LY_SM_ITENS.FLG_DESUSO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 1; } }
      public override string SqlServerType { get { return "VARCHAR(1)"; } }

    }
    #endregion

    #region Coluna: UNIDADE
    private ColumnUnidade pvColUnidade;

    public sealed class ColumnUnidade : TDataColumn {

      public ColumnUnidade() {
        this.ColumnName = "unidade";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "unidade";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_itens.unidade"; } }
      public override string OriginalCol { get { return "unidade"; } }
      public override string RefCol { get { return "LY_SM_ITENS.UNIDADE"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 15; } }
      public override int Precision { get { return 14; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(14)"; } }

    }
    #endregion

    #region Coluna: ITEMPAI
    private ColumnItempai pvColItempai;

    public sealed class ColumnItempai : TDataColumn {

      public ColumnItempai() {
        this.ColumnName = "itempai";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "item pai";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_itens.itempai"; } }
      public override string OriginalCol { get { return "itempai"; } }
      public override string RefCol { get { return "LY_SM_ITENS.ITEMPAI"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 15; } }
      public override int Precision { get { return 14; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(14)"; } }

    }
    #endregion

    #region Coluna: FLG_PNAE
    private ColumnFlg_pnae pvColFlg_pnae;

    public sealed class ColumnFlg_pnae : TDataColumn {

      public ColumnFlg_pnae() {
        this.ColumnName = "flg_pnae";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "pnae";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_itens.flg_pnae"; } }
      public override string OriginalCol { get { return "flg_pnae"; } }
      public override string RefCol { get { return "LY_SM_ITENS.FLG_PNAE"; } }
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

    #region Coluna: DESCONTO
    private ColumnDesconto pvColDesconto;

    public sealed class ColumnDesconto : TDataColumn {

      public ColumnDesconto() {
        this.ColumnName = "desconto";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "desconto";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_itens.desconto"; } }
      public override string OriginalCol { get { return "desconto"; } }
      public override string RefCol { get { return "LY_SM_ITENS.DESCONTO"; } }
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
    #endregion
  }

  public partial class Ly_sm_itensfornecidos : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_sm_itensfornecidos() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_sm_itensfornecidos");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_sm_itensfornecidos");
      }
      catch{}
    }

    public Ly_sm_itensfornecidos() {
      pvColId_fornecedor = new ColumnId_fornecedor(); Columns.Add(pvColId_fornecedor);
      pvColId_item = new ColumnId_item(); Columns.Add(pvColId_item);
      pvColItemid_item = new ColumnItemid_item(); Columns.Add(pvColItemid_item);
      pvColNome_item = new ColumnNome_item(); Columns.Add(pvColNome_item);
      pvColNome_fornecedor = new ColumnNome_fornecedor(); Columns.Add(pvColNome_fornecedor);
      pvColCnpj_fornecedor = new ColumnCnpj_fornecedor(); Columns.Add(pvColCnpj_fornecedor);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_sm_itensfornecidos");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(decimal? @id_fornecedor, decimal? @id_item) {
      Row row = NewRow();

      row.Id_fornecedor = @id_fornecedor;
      row.Id_item = @id_item;

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
      get { return "LY_SM_ITENSFORNECIDOS"; }
    }

    protected override sealed string Name { get { return "Item Fornecido"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColId_fornecedor, pvColId_item });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_sm_itensfornecidos_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_sm_itensfornecidos_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_sm_itensfornecidos_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_sm_itensfornecidos_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId_fornecedor", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID_FORNECEDOR", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkId_item", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID_ITEM", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id_fornecedor", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_FORNECEDOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("id_item", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_ITEM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "id_fornecedor") { result.Add(toLower ? "id_fornecedor" : column); continue; }
          if(lower == "id_item") { result.Add(toLower ? "id_item" : column); continue; }
          if(!tabPrincipal) {
            if(lower == "itemid_item") { result.Add(toLower ? "itemid_item" : column); continue; }
            if(lower == "nome_item") { result.Add(toLower ? "nome_item" : column); continue; }
            if(lower == "nome_fornecedor") { result.Add(toLower ? "nome_fornecedor" : column); continue; }
            if(lower == "cnpj_fornecedor") { result.Add(toLower ? "cnpj_fornecedor" : column); continue; }
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id_fornecedor", "id_item" }
                          : new string[] { "ID_FORNECEDOR", "ID_ITEM" });
        else
          result.AddRange(toLower
                          ? new string[] { "id_fornecedor", "id_item", "itemid_item", "nome_item", "nome_fornecedor", 
                                           "cnpj_fornecedor" }
                          : new string[] { "ID_FORNECEDOR", "ID_ITEM", "ITEMID_ITEM", "NOME_ITEM", "NOME_FORNECEDOR", 
                                           "CNPJ_FORNECEDOR" });

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
        "ly_sm_itensfornecidos.id_fornecedor", "ly_sm_itensfornecidos.id_item"
      });
      if(scColumns.Contains("itemid_item")) list.Add("ly_sm_itens.itemid itemid_item");
      if(scColumns.Contains("nome_item")) list.Add("ly_sm_itens.nome nome_item");
      if(scColumns.Contains("nome_fornecedor")) list.Add("ly_sm_fornecedor.nome nome_fornecedor");
      if(scColumns.Contains("cnpj_fornecedor")) list.Add("ly_sm_fornecedor.cnpj cnpj_fornecedor");
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
          "FROM ly_sm_itensfornecidos " +
               "INNER JOIN ly_sm_fornecedor " +
                 "ON ly_sm_fornecedor.id = ly_sm_itensfornecidos.id_fornecedor " +
               "INNER JOIN ly_sm_itens " +
                 "ON ly_sm_itens.id = ly_sm_itensfornecidos.id_item " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_sm_itensfornecidos");
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
        da.TableMappings.Add("Table", "Ly_sm_itensfornecidos");
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
            if (dr["ID_FORNECEDOR"] != null && string.IsNullOrEmpty(dr["ID_FORNECEDOR"].ToString()))
              dr["ID_FORNECEDOR"] = DBNull.Value;
            if (dr["ID_ITEM"] != null && string.IsNullOrEmpty(dr["ID_ITEM"].ToString()))
              dr["ID_ITEM"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["ID_FORNECEDOR"] == null || string.IsNullOrEmpty(dr["ID_FORNECEDOR"].ToString()))
            {
              return "Favor informar o fornecedor.";
            }

            if (dr["ID_FORNECEDOR"] != null && !string.IsNullOrEmpty(dr["ID_FORNECEDOR"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_FORNECEDOR"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_FORNECEDOR"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_FORNECEDOR"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_FORNECEDOR"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O fornecedor deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["ID_ITEM"] == null || string.IsNullOrEmpty(dr["ID_ITEM"].ToString()))
            {
              return "Favor informar o item.";
            }

            if (dr["ID_ITEM"] != null && !string.IsNullOrEmpty(dr["ID_ITEM"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_ITEM"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_ITEM"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_ITEM"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_ITEM"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O item deve ser um valor inteiro com 14 dígitos no máximo.";
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

    public static Ly_sm_itensfornecidos Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_sm_itensfornecidos Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_sm_itensfornecidos tab = new Ly_sm_itensfornecidos();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_sm_itensfornecidos tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id_fornecedor, decimal? @id_item
      ) {
        return (Row)Add(new object[] {
          @id_fornecedor, @id_item
        });
      }

      public Row Find(
        decimal? @id_fornecedor, decimal? @id_item
      ) {
        return (Row)Find(new object[] {
          @id_fornecedor, @id_item
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
      /// ID_FORNECEDOR: NUMERIC(14), Primary Key, Not Null
      /// </summary>
      public decimal? Id_fornecedor { get { return (decimal?)this["id_fornecedor"]; } set { this["id_fornecedor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ID_ITEM: NUMERIC(14), Primary Key, Not Null
      /// </summary>
      public decimal? Id_item { get { return (decimal?)this["id_item"]; } set { this["id_item"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ITEMID: VARCHAR(100), Not Null
      /// </summary>
      public string Itemid_item { get { return (string)this["itemid_item"]; } set { this["itemid_item"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(200), Not Null
      /// </summary>
      public string Nome_item { get { return (string)this["nome_item"]; } set { this["nome_item"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(200), Not Null
      /// </summary>
      public string Nome_fornecedor { get { return (string)this["nome_fornecedor"]; } set { this["nome_fornecedor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CNPJ: VARCHAR(19)
      /// </summary>
      public string Cnpj_fornecedor { get { return (string)this["cnpj_fornecedor"]; } set { this["cnpj_fornecedor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               decimal? @id_fornecedor, decimal? @id_item) {
        return Insert(connection,
                      @id_fornecedor, @id_item,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               decimal? @id_fornecedor, decimal? @id_item,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_sm_itensfornecidos(),
          new string[] { "id_fornecedor", "id_item" },
          new object[] { @id_fornecedor, @id_item },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId_fornecedor, decimal? pkId_item,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_sm_itensfornecidos(),
          new string[] { "id_fornecedor", "id_item" },
          new object[] { pkId_fornecedor, pkId_item },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId_fornecedor, decimal? pkId_item) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_sm_itensfornecidos(),
          new string[] { "id_fornecedor", "id_item" },
          new object[] { pkId_fornecedor, pkId_item }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              decimal? pkId_fornecedor, decimal? pkId_item) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_sm_itensfornecidos(),
          new string[] { "id_fornecedor", "id_item" },
          new object[] { pkId_fornecedor, pkId_item }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public decimal? Id_fornecedor { get { return (decimal?)row["id_fornecedor", DataRowVersion.Original]; } }
        public decimal? Id_item { get { return (decimal?)row["id_item", DataRowVersion.Original]; } }
        public string Itemid_item { get { return (string)row["itemid_item", DataRowVersion.Original]; } }
        public string Nome_item { get { return (string)row["nome_item", DataRowVersion.Original]; } }
        public string Nome_fornecedor { get { return (string)row["nome_fornecedor", DataRowVersion.Original]; } }
        public string Cnpj_fornecedor { get { return (string)row["cnpj_fornecedor", DataRowVersion.Original]; } }

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
    #region Coluna: ID_FORNECEDOR
    private ColumnId_fornecedor pvColId_fornecedor;

    public sealed class ColumnId_fornecedor : TDataColumn {

      public ColumnId_fornecedor() {
        this.ColumnName = "id_fornecedor";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "fornecedor";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_itensfornecidos.id_fornecedor"; } }
      public override string OriginalCol { get { return "id_fornecedor"; } }
      public override string RefCol { get { return "LY_SM_ITENSFORNECIDOS.ID_FORNECEDOR"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 15; } }
      public override int Precision { get { return 14; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(14)"; } }

    }
    #endregion

    #region Coluna: ID_ITEM
    private ColumnId_item pvColId_item;

    public sealed class ColumnId_item : TDataColumn {

      public ColumnId_item() {
        this.ColumnName = "id_item";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "item";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_itensfornecidos.id_item"; } }
      public override string OriginalCol { get { return "id_item"; } }
      public override string RefCol { get { return "LY_SM_ITENSFORNECIDOS.ID_ITEM"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 15; } }
      public override int Precision { get { return 14; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(14)"; } }

    }
    #endregion

    #region Coluna: ITEMID_ITEM
    private ColumnItemid_item pvColItemid_item;

    public sealed class ColumnItemid_item : TDataColumn {

      public ColumnItemid_item() {
        this.ColumnName = "itemid_item";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "código";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_itens.itemid"; } }
      public override string OriginalCol { get { return "itemid"; } }
      public override string RefCol { get { return "LY_SM_ITENS.ITEMID"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 100; } }
      public override string SqlServerType { get { return "VARCHAR(100)"; } }

    }
    #endregion

    #region Coluna: NOME_ITEM
    private ColumnNome_item pvColNome_item;

    public sealed class ColumnNome_item : TDataColumn {

      public ColumnNome_item() {
        this.ColumnName = "nome_item";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "nome";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_itens.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "LY_SM_ITENS.NOME"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 200; } }
      public override string SqlServerType { get { return "VARCHAR(200)"; } }

    }
    #endregion

    #region Coluna: NOME_FORNECEDOR
    private ColumnNome_fornecedor pvColNome_fornecedor;

    public sealed class ColumnNome_fornecedor : TDataColumn {

      public ColumnNome_fornecedor() {
        this.ColumnName = "nome_fornecedor";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "nome";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.NOME"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 200; } }
      public override string SqlServerType { get { return "VARCHAR(200)"; } }

    }
    #endregion

    #region Coluna: CNPJ_FORNECEDOR
    private ColumnCnpj_fornecedor pvColCnpj_fornecedor;

    public sealed class ColumnCnpj_fornecedor : TDataColumn {

      public ColumnCnpj_fornecedor() {
        this.ColumnName = "cnpj_fornecedor";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "cnpj";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.cnpj"; } }
      public override string OriginalCol { get { return "cnpj"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.CNPJ"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 19; } }
      public override string SqlServerType { get { return "VARCHAR(19)"; } }

    }
    #endregion
    #endregion
  }

  public partial class Ly_sm_marcas : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_sm_marcas() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_sm_marcas");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_sm_marcas");
      }
      catch{}
    }

    public Ly_sm_marcas() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColSigla = new ColumnSigla(); Columns.Add(pvColSigla);
      pvColDescricao = new ColumnDescricao(); Columns.Add(pvColDescricao);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_sm_marcas");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @sigla) {
      Row row = NewRow();

      row.Sigla = @sigla;

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
      get { return "LY_SM_MARCAS"; }
    }

    protected override sealed string Name { get { return "Marca"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColId });
      pvColId.AllowDBNull = true;
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_sm_marcas_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_sm_marcas_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_sm_marcas_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_sm_marcas_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("sigla", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SIGLA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
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
          if(lower == "id") { result.Add(toLower ? "id" : column); continue; }
          if(lower == "sigla") { result.Add(toLower ? "sigla" : column); continue; }
          if(lower == "descricao") { result.Add(toLower ? "descricao" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "sigla", "descricao" }
                          : new string[] { "ID", "SIGLA", "DESCRICAO" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "sigla", "descricao" }
                          : new string[] { "ID", "SIGLA", "DESCRICAO" });

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
        "ly_sm_marcas.id"
      });
      if(scColumns.Contains("sigla")) list.Add("ly_sm_marcas.sigla");
      if(scColumns.Contains("descricao")) list.Add("ly_sm_marcas.descricao");
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
          "FROM ly_sm_marcas " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_sm_marcas");
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
        da.TableMappings.Add("Table", "Ly_sm_marcas");
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
            if (dr["SIGLA"] != null && string.IsNullOrEmpty(dr["SIGLA"].ToString()))
              dr["SIGLA"] = DBNull.Value;
            if (dr["DESCRICAO"] != null && string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
              dr["DESCRICAO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["SIGLA"] == null || string.IsNullOrEmpty(dr["SIGLA"].ToString()))
            {
              return "Favor informar a sigla.";
            }

            if (dr["SIGLA"] != null && !string.IsNullOrEmpty(dr["SIGLA"].ToString()))
            {
              if (dr["SIGLA"].ToString().Length > 50)
              {
                return "A sigla deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["DESCRICAO"] != null && !string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              if (dr["DESCRICAO"].ToString().Length > 200)
              {
                return "A descrição deve possuir no máximo 200 caracteres.";
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

    public static Ly_sm_marcas Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_sm_marcas Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_sm_marcas tab = new Ly_sm_marcas();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_sm_marcas tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, string @sigla, string @descricao
      ) {
        return (Row)Add(new object[] {
          @id, @sigla, @descricao
        });
      }

      public Row Find(
        decimal? @id
      ) {
        return (Row)Find(new object[] {
          @id
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
      /// ID: NUMERIC(14), Primary Key, AutoNumber, Not Null
      /// </summary>
      public decimal? Id { get { return (decimal?)this["id"]; } set { this["id"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SIGLA: VARCHAR(50), Not Null
      /// </summary>
      public string Sigla { get { return (string)this["sigla"]; } set { this["sigla"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO: VARCHAR(200)
      /// </summary>
      public string Descricao { get { return (string)this["descricao"]; } set { this["descricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @sigla) {
        return Insert(connection,
                      @sigla,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @sigla,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_sm_marcas(),
          new string[] { "sigla" },
          new object[] { @sigla },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_sm_marcas(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_sm_marcas(),
          new string[] { "id" },
          new object[] { pkId }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              decimal? pkId) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_sm_marcas(),
          new string[] { "id" },
          new object[] { pkId }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public decimal? Id { get { return (decimal?)row["id", DataRowVersion.Original]; } }
        public string Sigla { get { return (string)row["sigla", DataRowVersion.Original]; } }
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
    #region Coluna: ID
    private ColumnId pvColId;

    public sealed class ColumnId : TDataColumn {

      public ColumnId() {
        this.ColumnName = "id";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "id";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_marcas.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_SM_MARCAS.ID"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 15; } }
      public override int Precision { get { return 14; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(14)"; } }

    }
    #endregion

    #region Coluna: SIGLA
    private ColumnSigla pvColSigla;

    public sealed class ColumnSigla : TDataColumn {

      public ColumnSigla() {
        this.ColumnName = "sigla";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "sigla";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_marcas.sigla"; } }
      public override string OriginalCol { get { return "sigla"; } }
      public override string RefCol { get { return "LY_SM_MARCAS.SIGLA"; } }
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

    #region Coluna: DESCRICAO
    private ColumnDescricao pvColDescricao;

    public sealed class ColumnDescricao : TDataColumn {

      public ColumnDescricao() {
        this.ColumnName = "descricao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "descrição";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_marcas.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "LY_SM_MARCAS.DESCRICAO"; } }
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

  public partial class Ly_sm_municipiosatendid : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_sm_municipiosatendid() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_sm_municipiosatendid");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_sm_municipiosatendid");
      }
      catch{}
    }

    public Ly_sm_municipiosatendid() {
      pvColId_fornecedor = new ColumnId_fornecedor(); Columns.Add(pvColId_fornecedor);
      pvColMunicipio = new ColumnMunicipio(); Columns.Add(pvColMunicipio);
      pvColNome_fornecedor = new ColumnNome_fornecedor(); Columns.Add(pvColNome_fornecedor);
      pvColCnpj_fornecedor = new ColumnCnpj_fornecedor(); Columns.Add(pvColCnpj_fornecedor);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_sm_municipiosatendid");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(decimal? @id_fornecedor, string @municipio) {
      Row row = NewRow();

      row.Id_fornecedor = @id_fornecedor;
      row.Municipio = @municipio;

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
      get { return "LY_SM_MUNICIPIOSATENDID"; }
    }

    protected override sealed string Name { get { return "Município Atendido"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColId_fornecedor, pvColMunicipio });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_sm_municipiosatendid_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_sm_municipiosatendid_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_sm_municipiosatendid_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_sm_municipiosatendid_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId_fornecedor", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID_FORNECEDOR", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkMunicipio", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "MUNICIPIO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id_fornecedor", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_FORNECEDOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("municipio", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "MUNICIPIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "id_fornecedor") { result.Add(toLower ? "id_fornecedor" : column); continue; }
          if(lower == "municipio") { result.Add(toLower ? "municipio" : column); continue; }
          if(!tabPrincipal) {
            if(lower == "nome_fornecedor") { result.Add(toLower ? "nome_fornecedor" : column); continue; }
            if(lower == "cnpj_fornecedor") { result.Add(toLower ? "cnpj_fornecedor" : column); continue; }
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id_fornecedor", "municipio" }
                          : new string[] { "ID_FORNECEDOR", "MUNICIPIO" });
        else
          result.AddRange(toLower
                          ? new string[] { "id_fornecedor", "municipio", "nome_fornecedor", "cnpj_fornecedor" }
                          : new string[] { "ID_FORNECEDOR", "MUNICIPIO", "NOME_FORNECEDOR", "CNPJ_FORNECEDOR" });

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
        "ly_sm_municipiosatendid.id_fornecedor", "ly_sm_municipiosatendid.municipio"
      });
      if(scColumns.Contains("nome_fornecedor")) list.Add("ly_sm_fornecedor.nome nome_fornecedor");
      if(scColumns.Contains("cnpj_fornecedor")) list.Add("ly_sm_fornecedor.cnpj cnpj_fornecedor");
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
          "FROM ly_sm_municipiosatendid " +
               "INNER JOIN ly_sm_fornecedor " +
                 "ON ly_sm_fornecedor.id = ly_sm_municipiosatendid.id_fornecedor " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_sm_municipiosatendid");
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
        da.TableMappings.Add("Table", "Ly_sm_municipiosatendid");
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
            if (dr["ID_FORNECEDOR"] != null && string.IsNullOrEmpty(dr["ID_FORNECEDOR"].ToString()))
              dr["ID_FORNECEDOR"] = DBNull.Value;
            if (dr["MUNICIPIO"] != null && string.IsNullOrEmpty(dr["MUNICIPIO"].ToString()))
              dr["MUNICIPIO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["ID_FORNECEDOR"] == null || string.IsNullOrEmpty(dr["ID_FORNECEDOR"].ToString()))
            {
              return "Favor informar o fornecedor.";
            }

            if (dr["ID_FORNECEDOR"] != null && !string.IsNullOrEmpty(dr["ID_FORNECEDOR"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_FORNECEDOR"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_FORNECEDOR"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_FORNECEDOR"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_FORNECEDOR"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O fornecedor deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["MUNICIPIO"] == null || string.IsNullOrEmpty(dr["MUNICIPIO"].ToString()))
            {
              return "Favor informar o município.";
            }

            if (dr["MUNICIPIO"] != null && !string.IsNullOrEmpty(dr["MUNICIPIO"].ToString()))
            {
              if (dr["MUNICIPIO"].ToString().Length > 50)
              {
                return "O município deve possuir no máximo 50 caracteres.";
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

    public static Ly_sm_municipiosatendid Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_sm_municipiosatendid Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_sm_municipiosatendid tab = new Ly_sm_municipiosatendid();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_sm_municipiosatendid tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id_fornecedor, string @municipio
      ) {
        return (Row)Add(new object[] {
          @id_fornecedor, @municipio
        });
      }

      public Row Find(
        decimal? @id_fornecedor, string @municipio
      ) {
        return (Row)Find(new object[] {
          @id_fornecedor, @municipio
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
      /// ID_FORNECEDOR: NUMERIC(14), Primary Key, Not Null
      /// </summary>
      public decimal? Id_fornecedor { get { return (decimal?)this["id_fornecedor"]; } set { this["id_fornecedor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// MUNICIPIO: VARCHAR(50), Primary Key, Not Null
      /// </summary>
      public string Municipio { get { return (string)this["municipio"]; } set { this["municipio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(200), Not Null
      /// </summary>
      public string Nome_fornecedor { get { return (string)this["nome_fornecedor"]; } set { this["nome_fornecedor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CNPJ: VARCHAR(19)
      /// </summary>
      public string Cnpj_fornecedor { get { return (string)this["cnpj_fornecedor"]; } set { this["cnpj_fornecedor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               decimal? @id_fornecedor, string @municipio) {
        return Insert(connection,
                      @id_fornecedor, @municipio,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               decimal? @id_fornecedor, string @municipio,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_sm_municipiosatendid(),
          new string[] { "id_fornecedor", "municipio" },
          new object[] { @id_fornecedor, @municipio },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId_fornecedor, string pkMunicipio,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_sm_municipiosatendid(),
          new string[] { "id_fornecedor", "municipio" },
          new object[] { pkId_fornecedor, pkMunicipio },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId_fornecedor, string pkMunicipio) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_sm_municipiosatendid(),
          new string[] { "id_fornecedor", "municipio" },
          new object[] { pkId_fornecedor, pkMunicipio }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              decimal? pkId_fornecedor, string pkMunicipio) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_sm_municipiosatendid(),
          new string[] { "id_fornecedor", "municipio" },
          new object[] { pkId_fornecedor, pkMunicipio }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public decimal? Id_fornecedor { get { return (decimal?)row["id_fornecedor", DataRowVersion.Original]; } }
        public string Municipio { get { return (string)row["municipio", DataRowVersion.Original]; } }
        public string Nome_fornecedor { get { return (string)row["nome_fornecedor", DataRowVersion.Original]; } }
        public string Cnpj_fornecedor { get { return (string)row["cnpj_fornecedor", DataRowVersion.Original]; } }

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
    #region Coluna: ID_FORNECEDOR
    private ColumnId_fornecedor pvColId_fornecedor;

    public sealed class ColumnId_fornecedor : TDataColumn {

      public ColumnId_fornecedor() {
        this.ColumnName = "id_fornecedor";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "fornecedor";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_municipiosatendid.id_fornecedor"; } }
      public override string OriginalCol { get { return "id_fornecedor"; } }
      public override string RefCol { get { return "LY_SM_MUNICIPIOSATENDID.ID_FORNECEDOR"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 15; } }
      public override int Precision { get { return 14; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(14)"; } }

    }
    #endregion

    #region Coluna: MUNICIPIO
    private ColumnMunicipio pvColMunicipio;

    public sealed class ColumnMunicipio : TDataColumn {

      public ColumnMunicipio() {
        this.ColumnName = "municipio";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "município";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_municipiosatendid.municipio"; } }
      public override string OriginalCol { get { return "municipio"; } }
      public override string RefCol { get { return "LY_SM_MUNICIPIOSATENDID.MUNICIPIO"; } }
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

    #region Coluna: NOME_FORNECEDOR
    private ColumnNome_fornecedor pvColNome_fornecedor;

    public sealed class ColumnNome_fornecedor : TDataColumn {

      public ColumnNome_fornecedor() {
        this.ColumnName = "nome_fornecedor";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "nome";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.NOME"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 200; } }
      public override string SqlServerType { get { return "VARCHAR(200)"; } }

    }
    #endregion

    #region Coluna: CNPJ_FORNECEDOR
    private ColumnCnpj_fornecedor pvColCnpj_fornecedor;

    public sealed class ColumnCnpj_fornecedor : TDataColumn {

      public ColumnCnpj_fornecedor() {
        this.ColumnName = "cnpj_fornecedor";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "cnpj";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_fornecedor.cnpj"; } }
      public override string OriginalCol { get { return "cnpj"; } }
      public override string RefCol { get { return "LY_SM_FORNECEDOR.CNPJ"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 19; } }
      public override string SqlServerType { get { return "VARCHAR(19)"; } }

    }
    #endregion
    #endregion
  }

  public partial class Ly_sm_precositem : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_sm_precositem() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_sm_precositem");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_sm_precositem");
      }
      catch{}
    }

    public Ly_sm_precositem() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColIdprecosmercado = new ColumnIdprecosmercado(); Columns.Add(pvColIdprecosmercado);
      pvColIditem = new ColumnIditem(); Columns.Add(pvColIditem);
      pvColPreco = new ColumnPreco(); Columns.Add(pvColPreco);
      pvColItemid = new ColumnItemid(); Columns.Add(pvColItemid);
      pvColNome = new ColumnNome(); Columns.Add(pvColNome);
      pvColUnidade = new ColumnUnidade(); Columns.Add(pvColUnidade);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_sm_precositem");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(decimal? @idprecosmercado, decimal? @iditem) {
      Row row = NewRow();

      row.Idprecosmercado = @idprecosmercado;
      row.Iditem = @iditem;

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
      get { return "LY_SM_PRECOSITEM"; }
    }

    protected override sealed string Name { get { return "Preço de Mercado"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColId });
      pvColId.AllowDBNull = true;
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_sm_precositem_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_sm_precositem_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_sm_precositem_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_sm_precositem_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("idprecosmercado", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "IDPRECOSMERCADO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("iditem", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "IDITEM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("preco", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 21, 10, "PRECO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "id") { result.Add(toLower ? "id" : column); continue; }
          if(lower == "idprecosmercado") { result.Add(toLower ? "idprecosmercado" : column); continue; }
          if(lower == "iditem") { result.Add(toLower ? "iditem" : column); continue; }
          if(lower == "preco") { result.Add(toLower ? "preco" : column); continue; }
          if(!tabPrincipal) {
            if(lower == "itemid") { result.Add(toLower ? "itemid" : column); continue; }
            if(lower == "nome") { result.Add(toLower ? "nome" : column); continue; }
            if(lower == "unidade") { result.Add(toLower ? "unidade" : column); continue; }
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "idprecosmercado", "iditem", "preco" }
                          : new string[] { "ID", "IDPRECOSMERCADO", "IDITEM", "PRECO" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "idprecosmercado", "iditem", "preco", "itemid", "nome", "unidade" }
                          : new string[] { "ID", "IDPRECOSMERCADO", "IDITEM", "PRECO", "ITEMID", "NOME", "UNIDADE" });

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
        "ly_sm_precositem.id"
      });
      if(scColumns.Contains("idprecosmercado")) list.Add("ly_sm_precositem.idprecosmercado");
      if(scColumns.Contains("iditem")) list.Add("ly_sm_precositem.iditem");
      if(scColumns.Contains("preco")) list.Add("ly_sm_precositem.preco");
      if(scColumns.Contains("itemid")) list.Add("ly_sm_itens.itemid");
      if(scColumns.Contains("nome")) list.Add("ly_sm_itens.nome");
      if(scColumns.Contains("unidade")) list.Add("ly_sm_unidmed.unidade");
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
          "FROM ly_sm_precositem " +
               "INNER JOIN ly_sm_itens " +
                          "LEFT JOIN ly_sm_unidmed " +
                            "ON ly_sm_unidmed.id = ly_sm_itens.unidade " +
                 "ON ly_sm_itens.id = ly_sm_precositem.iditem " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_sm_precositem");
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
        da.TableMappings.Add("Table", "Ly_sm_precositem");
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
            if (dr["IDPRECOSMERCADO"] != null && string.IsNullOrEmpty(dr["IDPRECOSMERCADO"].ToString()))
              dr["IDPRECOSMERCADO"] = DBNull.Value;
            if (dr["IDITEM"] != null && string.IsNullOrEmpty(dr["IDITEM"].ToString()))
              dr["IDITEM"] = DBNull.Value;
            if (dr["PRECO"] != null && string.IsNullOrEmpty(dr["PRECO"].ToString()))
              dr["PRECO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["IDPRECOSMERCADO"] == null || string.IsNullOrEmpty(dr["IDPRECOSMERCADO"].ToString()))
            {
              return "Favor informar a tabela de preços.";
            }

            if (dr["IDPRECOSMERCADO"] != null && !string.IsNullOrEmpty(dr["IDPRECOSMERCADO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["IDPRECOSMERCADO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["IDPRECOSMERCADO"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["IDPRECOSMERCADO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["IDPRECOSMERCADO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A tabela de preços deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["IDITEM"] == null || string.IsNullOrEmpty(dr["IDITEM"].ToString()))
            {
              return "Favor informar o item.";
            }

            if (dr["IDITEM"] != null && !string.IsNullOrEmpty(dr["IDITEM"].ToString()))
            {
              if ((Convert.ToDecimal(dr["IDITEM"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["IDITEM"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["IDITEM"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["IDITEM"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O item deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["PRECO"] != null && !string.IsNullOrEmpty(dr["PRECO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["PRECO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PRECO"]).ToString("0.############").Split(',')[0].Length > 9) || (Convert.ToDecimal(dr["PRECO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PRECO"]).ToString("0.############").Split(',')[1].Length > 2))
              {
                return "O preço deve ser um valor numérico com, no máximo, 9 dígitos na parte inteira e 2 casas decimal.";
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

    public static Ly_sm_precositem Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_sm_precositem Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_sm_precositem tab = new Ly_sm_precositem();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_sm_precositem tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, decimal? @idprecosmercado, decimal? @iditem, decimal? @preco
      ) {
        return (Row)Add(new object[] {
          @id, @idprecosmercado, @iditem, @preco
        });
      }

      public Row Find(
        decimal? @id
      ) {
        return (Row)Find(new object[] {
          @id
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
      /// ID: NUMERIC(14), Primary Key, AutoNumber, Not Null
      /// </summary>
      public decimal? Id { get { return (decimal?)this["id"]; } set { this["id"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// IDPRECOSMERCADO: NUMERIC(14), Not Null
      /// </summary>
      public decimal? Idprecosmercado { get { return (decimal?)this["idprecosmercado"]; } set { this["idprecosmercado"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// IDITEM: NUMERIC(14), Not Null
      /// </summary>
      public decimal? Iditem { get { return (decimal?)this["iditem"]; } set { this["iditem"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PRECO: NUMERIC(11, 2)
      /// </summary>
      public decimal? Preco { get { return (decimal?)this["preco"]; } set { this["preco"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ITEMID: VARCHAR(100), Not Null
      /// </summary>
      public string Itemid { get { return (string)this["itemid"]; } set { this["itemid"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(200), Not Null
      /// </summary>
      public string Nome { get { return (string)this["nome"]; } set { this["nome"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// UNIDADE: VARCHAR(15), Not Null
      /// </summary>
      public string Unidade { get { return (string)this["unidade"]; } set { this["unidade"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               decimal? @idprecosmercado, decimal? @iditem) {
        return Insert(connection,
                      @idprecosmercado, @iditem,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               decimal? @idprecosmercado, decimal? @iditem,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_sm_precositem(),
          new string[] { "idprecosmercado", "iditem" },
          new object[] { @idprecosmercado, @iditem },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_sm_precositem(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_sm_precositem(),
          new string[] { "id" },
          new object[] { pkId }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              decimal? pkId) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_sm_precositem(),
          new string[] { "id" },
          new object[] { pkId }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public decimal? Id { get { return (decimal?)row["id", DataRowVersion.Original]; } }
        public decimal? Idprecosmercado { get { return (decimal?)row["idprecosmercado", DataRowVersion.Original]; } }
        public decimal? Iditem { get { return (decimal?)row["iditem", DataRowVersion.Original]; } }
        public decimal? Preco { get { return (decimal?)row["preco", DataRowVersion.Original]; } }
        public string Itemid { get { return (string)row["itemid", DataRowVersion.Original]; } }
        public string Nome { get { return (string)row["nome", DataRowVersion.Original]; } }
        public string Unidade { get { return (string)row["unidade", DataRowVersion.Original]; } }

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
    #region Coluna: ID
    private ColumnId pvColId;

    public sealed class ColumnId : TDataColumn {

      public ColumnId() {
        this.ColumnName = "id";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "id";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_precositem.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_SM_PRECOSITEM.ID"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 15; } }
      public override int Precision { get { return 14; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(14)"; } }

    }
    #endregion

    #region Coluna: IDPRECOSMERCADO
    private ColumnIdprecosmercado pvColIdprecosmercado;

    public sealed class ColumnIdprecosmercado : TDataColumn {

      public ColumnIdprecosmercado() {
        this.ColumnName = "idprecosmercado";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "tabela de preços";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_precositem.idprecosmercado"; } }
      public override string OriginalCol { get { return "idprecosmercado"; } }
      public override string RefCol { get { return "LY_SM_PRECOSITEM.IDPRECOSMERCADO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 15; } }
      public override int Precision { get { return 14; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(14)"; } }

    }
    #endregion

    #region Coluna: IDITEM
    private ColumnIditem pvColIditem;

    public sealed class ColumnIditem : TDataColumn {

      public ColumnIditem() {
        this.ColumnName = "iditem";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "item";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_precositem.iditem"; } }
      public override string OriginalCol { get { return "iditem"; } }
      public override string RefCol { get { return "LY_SM_PRECOSITEM.IDITEM"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 15; } }
      public override int Precision { get { return 14; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(14)"; } }

    }
    #endregion

    #region Coluna: PRECO
    private ColumnPreco pvColPreco;

    public sealed class ColumnPreco : TDataColumn {

      public ColumnPreco() {
        this.ColumnName = "preco";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "preço";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_precositem.preco"; } }
      public override string OriginalCol { get { return "preco"; } }
      public override string RefCol { get { return "LY_SM_PRECOSITEM.PRECO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 13; } }
      public override int Precision { get { return 11; } }
      public override int Scale { get { return 2; } }
      public override string SqlServerType { get { return "NUMERIC(11, 2)"; } }

    }
    #endregion

    #region Coluna: ITEMID
    private ColumnItemid pvColItemid;

    public sealed class ColumnItemid : TDataColumn {

      public ColumnItemid() {
        this.ColumnName = "itemid";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "código";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_itens.itemid"; } }
      public override string OriginalCol { get { return "itemid"; } }
      public override string RefCol { get { return "LY_SM_ITENS.ITEMID"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
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
      public override string FullCol { get { return "ly_sm_itens.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "LY_SM_ITENS.NOME"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 200; } }
      public override string SqlServerType { get { return "VARCHAR(200)"; } }

    }
    #endregion

    #region Coluna: UNIDADE
    private ColumnUnidade pvColUnidade;

    public sealed class ColumnUnidade : TDataColumn {

      public ColumnUnidade() {
        this.ColumnName = "unidade";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "sigla";}
      protected override string GetCaption(){return "Sigla";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_unidmed.unidade"; } }
      public override string OriginalCol { get { return "unidade"; } }
      public override string RefCol { get { return "LY_SM_UNIDMED.UNIDADE"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 15; } }
      public override string SqlServerType { get { return "VARCHAR(15)"; } }

    }
    #endregion
    #endregion
  }

  public partial class Ly_sm_precosmercado : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_sm_precosmercado() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_sm_precosmercado");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_sm_precosmercado");
      }
      catch{}
    }

    public Ly_sm_precosmercado() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColDataini = new ColumnDataini(); Columns.Add(pvColDataini);
      pvColDatafim = new ColumnDatafim(); Columns.Add(pvColDatafim);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_sm_precosmercado");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(DateTime? @dataini) {
      Row row = NewRow();

      row.Dataini = @dataini;

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
      get { return "LY_SM_PRECOSMERCADO"; }
    }

    protected override sealed string Name { get { return "Tabela de Preços de Mercado"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColId });
      pvColId.AllowDBNull = true;
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_sm_precosmercado_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_sm_precosmercado_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_sm_precosmercado_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_sm_precosmercado_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("dataini", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DATAINI", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("datafim", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DATAFIM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));

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
          if(lower == "id") { result.Add(toLower ? "id" : column); continue; }
          if(lower == "dataini") { result.Add(toLower ? "dataini" : column); continue; }
          if(lower == "datafim") { result.Add(toLower ? "datafim" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "dataini", "datafim" }
                          : new string[] { "ID", "DATAINI", "DATAFIM" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "dataini", "datafim" }
                          : new string[] { "ID", "DATAINI", "DATAFIM" });

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
        "ly_sm_precosmercado.id"
      });
      if(scColumns.Contains("dataini")) list.Add("ly_sm_precosmercado.dataini");
      if(scColumns.Contains("datafim")) list.Add("ly_sm_precosmercado.datafim");
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
          "FROM ly_sm_precosmercado " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_sm_precosmercado");
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
        da.TableMappings.Add("Table", "Ly_sm_precosmercado");
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
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["DATAINI"] == null || string.IsNullOrEmpty(dr["DATAINI"].ToString()))
            {
              return "Favor informar a data de início da validade.";
            }

            if (dr["DATAINI"] != null && dr["DATAINI"].ToString() != "")
            {
              dr["DATAINI"] = Convert.ToDateTime(dr["DATAINI"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DATAFIM"] != null && dr["DATAFIM"].ToString() != "")
            {
              dr["DATAFIM"] = Convert.ToDateTime(dr["DATAFIM"]).ToString("dd/MM/yyyy HH:mm:ss");
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

    public static Ly_sm_precosmercado Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_sm_precosmercado Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_sm_precosmercado tab = new Ly_sm_precosmercado();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_sm_precosmercado tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, DateTime? @dataini, DateTime? @datafim
      ) {
        return (Row)Add(new object[] {
          @id, @dataini, @datafim
        });
      }

      public Row Find(
        decimal? @id
      ) {
        return (Row)Find(new object[] {
          @id
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
      /// ID: NUMERIC(14), Primary Key, AutoNumber, Not Null
      /// </summary>
      public decimal? Id { get { return (decimal?)this["id"]; } set { this["id"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DATAINI: DATETIME, Not Null
      /// </summary>
      public DateTime? Dataini { get { return (DateTime?)this["dataini"]; } set { this["dataini"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DATAFIM: DATETIME
      /// </summary>
      public DateTime? Datafim { get { return (DateTime?)this["datafim"]; } set { this["datafim"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               DateTime? @dataini) {
        return Insert(connection,
                      @dataini,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               DateTime? @dataini,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_sm_precosmercado(),
          new string[] { "dataini" },
          new object[] { @dataini },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_sm_precosmercado(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_sm_precosmercado(),
          new string[] { "id" },
          new object[] { pkId }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              decimal? pkId) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_sm_precosmercado(),
          new string[] { "id" },
          new object[] { pkId }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public decimal? Id { get { return (decimal?)row["id", DataRowVersion.Original]; } }
        public DateTime? Dataini { get { return (DateTime?)row["dataini", DataRowVersion.Original]; } }
        public DateTime? Datafim { get { return (DateTime?)row["datafim", DataRowVersion.Original]; } }

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
    #region Coluna: ID
    private ColumnId pvColId;

    public sealed class ColumnId : TDataColumn {

      public ColumnId() {
        this.ColumnName = "id";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "id";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_precosmercado.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_SM_PRECOSMERCADO.ID"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 15; } }
      public override int Precision { get { return 14; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(14)"; } }

    }
    #endregion

    #region Coluna: DATAINI
    private ColumnDataini pvColDataini;

    public sealed class ColumnDataini : TDataColumn {

      public ColumnDataini() {
        this.ColumnName = "dataini";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data de início da validade";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_precosmercado.dataini"; } }
      public override string OriginalCol { get { return "dataini"; } }
      public override string RefCol { get { return "LY_SM_PRECOSMERCADO.DATAINI"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 10; } }
      public override string SqlServerType { get { return "DATETIME"; } }

    }
    #endregion

    #region Coluna: DATAFIM
    private ColumnDatafim pvColDatafim;

    public sealed class ColumnDatafim : TDataColumn {

      public ColumnDatafim() {
        this.ColumnName = "datafim";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data de término da validade";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_precosmercado.datafim"; } }
      public override string OriginalCol { get { return "datafim"; } }
      public override string RefCol { get { return "LY_SM_PRECOSMERCADO.DATAFIM"; } }
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
    #endregion
  }

  public partial class Ly_sm_unidmed : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_sm_unidmed() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_sm_unidmed");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_sm_unidmed");
      }
      catch{}
    }

    public Ly_sm_unidmed() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColUnidade = new ColumnUnidade(); Columns.Add(pvColUnidade);
      pvColNome = new ColumnNome(); Columns.Add(pvColNome);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_sm_unidmed");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @unidade, string @nome) {
      Row row = NewRow();

      row.Unidade = @unidade;
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
      get { return "LY_SM_UNIDMED"; }
    }

    protected override sealed string Name { get { return "Unidade de Medida"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColId });
      pvColId.AllowDBNull = true;
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_sm_unidmed_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_sm_unidmed_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_sm_unidmed_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_sm_unidmed_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("unidade", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "UNIDADE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("nome", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NOME", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "id") { result.Add(toLower ? "id" : column); continue; }
          if(lower == "unidade") { result.Add(toLower ? "unidade" : column); continue; }
          if(lower == "nome") { result.Add(toLower ? "nome" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "unidade", "nome" }
                          : new string[] { "ID", "UNIDADE", "NOME" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "unidade", "nome" }
                          : new string[] { "ID", "UNIDADE", "NOME" });

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
        "ly_sm_unidmed.id"
      });
      if(scColumns.Contains("unidade")) list.Add("ly_sm_unidmed.unidade");
      if(scColumns.Contains("nome")) list.Add("ly_sm_unidmed.nome");
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
          "FROM ly_sm_unidmed " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_sm_unidmed");
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
        da.TableMappings.Add("Table", "Ly_sm_unidmed");
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
            if (dr["UNIDADE"] != null && string.IsNullOrEmpty(dr["UNIDADE"].ToString()))
              dr["UNIDADE"] = DBNull.Value;
            if (dr["NOME"] != null && string.IsNullOrEmpty(dr["NOME"].ToString()))
              dr["NOME"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["UNIDADE"] == null || string.IsNullOrEmpty(dr["UNIDADE"].ToString()))
            {
              return "Favor informar a sigla.";
            }

            if (dr["UNIDADE"] != null && !string.IsNullOrEmpty(dr["UNIDADE"].ToString()))
            {
              if (dr["UNIDADE"].ToString().Length > 15)
              {
                return "A sigla deve possuir no máximo 15 caracteres.";
              }
            }

            if (dr["NOME"] == null || string.IsNullOrEmpty(dr["NOME"].ToString()))
            {
              return "Favor informar a descrição.";
            }

            if (dr["NOME"] != null && !string.IsNullOrEmpty(dr["NOME"].ToString()))
            {
              if (dr["NOME"].ToString().Length > 50)
              {
                return "A descrição deve possuir no máximo 50 caracteres.";
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

    public static Ly_sm_unidmed Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_sm_unidmed Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_sm_unidmed tab = new Ly_sm_unidmed();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_sm_unidmed tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, string @unidade, string @nome
      ) {
        return (Row)Add(new object[] {
          @id, @unidade, @nome
        });
      }

      public Row Find(
        decimal? @id
      ) {
        return (Row)Find(new object[] {
          @id
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
      /// ID: NUMERIC(14), Primary Key, AutoNumber, Not Null
      /// </summary>
      public decimal? Id { get { return (decimal?)this["id"]; } set { this["id"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// UNIDADE: VARCHAR(15), Not Null
      /// </summary>
      public string Unidade { get { return (string)this["unidade"]; } set { this["unidade"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(50), Not Null
      /// </summary>
      public string Nome { get { return (string)this["nome"]; } set { this["nome"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @unidade, string @nome) {
        return Insert(connection,
                      @unidade, @nome,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @unidade, string @nome,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_sm_unidmed(),
          new string[] { "unidade", "nome" },
          new object[] { @unidade, @nome },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_sm_unidmed(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_sm_unidmed(),
          new string[] { "id" },
          new object[] { pkId }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              decimal? pkId) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_sm_unidmed(),
          new string[] { "id" },
          new object[] { pkId }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public decimal? Id { get { return (decimal?)row["id", DataRowVersion.Original]; } }
        public string Unidade { get { return (string)row["unidade", DataRowVersion.Original]; } }
        public string Nome { get { return (string)row["nome", DataRowVersion.Original]; } }

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
    #region Coluna: ID
    private ColumnId pvColId;

    public sealed class ColumnId : TDataColumn {

      public ColumnId() {
        this.ColumnName = "id";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "id";}
      protected override string GetCaption(){return "ID";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_unidmed.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_SM_UNIDMED.ID"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 15; } }
      public override int Precision { get { return 14; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(14)"; } }

    }
    #endregion

    #region Coluna: UNIDADE
    private ColumnUnidade pvColUnidade;

    public sealed class ColumnUnidade : TDataColumn {

      public ColumnUnidade() {
        this.ColumnName = "unidade";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "sigla";}
      protected override string GetCaption(){return "Sigla";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_unidmed.unidade"; } }
      public override string OriginalCol { get { return "unidade"; } }
      public override string RefCol { get { return "LY_SM_UNIDMED.UNIDADE"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 15; } }
      public override string SqlServerType { get { return "VARCHAR(15)"; } }

    }
    #endregion

    #region Coluna: NOME
    private ColumnNome pvColNome;

    public sealed class ColumnNome : TDataColumn {

      public ColumnNome() {
        this.ColumnName = "nome";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "descrição";}
      protected override string GetCaption(){return "Descrição";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_sm_unidmed.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "LY_SM_UNIDMED.NOME"; } }
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
    #endregion
  }
}

