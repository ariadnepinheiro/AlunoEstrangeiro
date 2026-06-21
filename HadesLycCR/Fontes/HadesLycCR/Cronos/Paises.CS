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
  public class Paises : TDataSet {
    private const string DataSetName_Def = "Paises";
    private static ConstructorInfo ctorTCustom = null;
    private CustomBase tcustom = null;
    #region Variáveis private para as tabelas deste dataset
    private Hd_municipio_estrangeiro pvTabHd_municipio_estrangeiro;
    private Hd_pais pvTabHd_pais;
    #endregion

    static Paises() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Paises");
      }
      catch{}
    }

    public Paises() : base() {
      DataSetName = DataSetName_Def;

      pvTabHd_municipio_estrangeiro = new Hd_municipio_estrangeiro();
      pvTabHd_pais = new Hd_pais();

      Tables.Add(pvTabHd_municipio_estrangeiro);
      Tables.Add(pvTabHd_pais);

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
          pvTabHd_municipio_estrangeiro.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_municipio_estrangeiro); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabHd_pais.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_pais); if(row != null) return DataLib.RowErrorsToString(row);
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
    public Hd_municipio_estrangeiro Hd_municipio_estrangeiro { get { return pvTabHd_municipio_estrangeiro; } }

    [
      Browsable(false),
    ]
    public Hd_pais Hd_pais { get { return pvTabHd_pais; } }

    #endregion

    public abstract class CustomBase {
      public abstract string Put(Techne.HadesLyc.CR.Paises ds, TConnectionWritable connection);
    }

  }


  public partial class Hd_municipio_estrangeiro : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_municipio_estrangeiro() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_municipio_estrangeiro");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_municipio_estrangeiro");
      }
      catch{}
    }

    public Hd_municipio_estrangeiro() {
      pvColMunicipio_estrangeiro = new ColumnMunicipio_estrangeiro(); Columns.Add(pvColMunicipio_estrangeiro);
      pvColPais = new ColumnPais(); Columns.Add(pvColPais);
      pvColNome = new ColumnNome(); Columns.Add(pvColNome);
      pvColEstado = new ColumnEstado(); Columns.Add(pvColEstado);
      pvColCep = new ColumnCep(); Columns.Add(pvColCep);
      pvColNome_estado = new ColumnNome_estado(); Columns.Add(pvColNome_estado);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_municipio_estrangeiro");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @municipio_estrangeiro, string @pais, string @nome) {
      Row row = NewRow();

      row.Municipio_estrangeiro = @municipio_estrangeiro;
      row.Pais = @pais;
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
      get { return "HD_MUNICIPIO_ESTRANGEIRO"; }
    }

    protected override sealed string Name { get { return "Municípios estrangeiros"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColMunicipio_estrangeiro });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_municipio_estrangeir_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_municipio_estrangeir_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_municipio_estrangeir_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_municipio_estrangeir_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkMunicipio_estrangeiro", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "MUNICIPIO_ESTRANGEIRO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("municipio_estrangeiro", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "MUNICIPIO_ESTRANGEIRO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("pais", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PAIS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("nome", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NOME", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("estado", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "ESTADO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("cep", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CEP", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("nome_estado", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NOME_ESTADO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "municipio_estrangeiro") { result.Add(toLower ? "municipio_estrangeiro" : column); continue; }
          if(lower == "pais") { result.Add(toLower ? "pais" : column); continue; }
          if(lower == "nome") { result.Add(toLower ? "nome" : column); continue; }
          if(lower == "estado") { result.Add(toLower ? "estado" : column); continue; }
          if(lower == "cep") { result.Add(toLower ? "cep" : column); continue; }
          if(lower == "nome_estado") { result.Add(toLower ? "nome_estado" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "municipio_estrangeiro", "pais", "nome", "estado", "cep", "nome_estado" }
                          : new string[] { "MUNICIPIO_ESTRANGEIRO", "PAIS", "NOME", "ESTADO", "CEP", "NOME_ESTADO" });
        else
          result.AddRange(toLower
                          ? new string[] { "municipio_estrangeiro", "pais", "nome", "estado", "cep", "nome_estado" }
                          : new string[] { "MUNICIPIO_ESTRANGEIRO", "PAIS", "NOME", "ESTADO", "CEP", "NOME_ESTADO" });

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
        "hd_municipio_estrangeiro.municipio_estrangeiro"
      });
      if(scColumns.Contains("pais")) list.Add("hd_municipio_estrangeiro.pais");
      if(scColumns.Contains("nome")) list.Add("hd_municipio_estrangeiro.nome");
      if(scColumns.Contains("estado")) list.Add("hd_municipio_estrangeiro.estado");
      if(scColumns.Contains("cep")) list.Add("hd_municipio_estrangeiro.cep");
      if(scColumns.Contains("nome_estado")) list.Add("hd_municipio_estrangeiro.nome_estado");
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
          "FROM hd_municipio_estrangeiro " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_municipio_estrangeiro");
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
        da.TableMappings.Add("Table", "Hd_municipio_estrangeiro");
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
            if (dr["MUNICIPIO_ESTRANGEIRO"] != null && string.IsNullOrEmpty(dr["MUNICIPIO_ESTRANGEIRO"].ToString()))
              dr["MUNICIPIO_ESTRANGEIRO"] = DBNull.Value;
            if (dr["PAIS"] != null && string.IsNullOrEmpty(dr["PAIS"].ToString()))
              dr["PAIS"] = DBNull.Value;
            if (dr["NOME"] != null && string.IsNullOrEmpty(dr["NOME"].ToString()))
              dr["NOME"] = DBNull.Value;
            if (dr["ESTADO"] != null && string.IsNullOrEmpty(dr["ESTADO"].ToString()))
              dr["ESTADO"] = DBNull.Value;
            if (dr["CEP"] != null && string.IsNullOrEmpty(dr["CEP"].ToString()))
              dr["CEP"] = DBNull.Value;
            if (dr["NOME_ESTADO"] != null && string.IsNullOrEmpty(dr["NOME_ESTADO"].ToString()))
              dr["NOME_ESTADO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["MUNICIPIO_ESTRANGEIRO"] == null || string.IsNullOrEmpty(dr["MUNICIPIO_ESTRANGEIRO"].ToString()))
            {
              return "Favor informar o Municipio_estrangeiro.";
            }

            if (dr["MUNICIPIO_ESTRANGEIRO"] != null && !string.IsNullOrEmpty(dr["MUNICIPIO_ESTRANGEIRO"].ToString()))
            {
              if (dr["MUNICIPIO_ESTRANGEIRO"].ToString().Length > 10)
              {
                return "O Municipio_estrangeiro deve possuir no máximo 10 caracteres.";
              }
            }

            if (dr["PAIS"] == null || string.IsNullOrEmpty(dr["PAIS"].ToString()))
            {
              return "Favor informar o Pais.";
            }

            if (dr["PAIS"] != null && !string.IsNullOrEmpty(dr["PAIS"].ToString()))
            {
              if (dr["PAIS"].ToString().Length > 10)
              {
                return "O Pais deve possuir no máximo 10 caracteres.";
              }
            }

            if (dr["NOME"] == null || string.IsNullOrEmpty(dr["NOME"].ToString()))
            {
              return "Favor informar o Nome.";
            }

            if (dr["NOME"] != null && !string.IsNullOrEmpty(dr["NOME"].ToString()))
            {
              if (dr["NOME"].ToString().Length > 50)
              {
                return "O Nome deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["ESTADO"] != null && !string.IsNullOrEmpty(dr["ESTADO"].ToString()))
            {
              if (dr["ESTADO"].ToString().Length > 20)
              {
                return "O Estado deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["CEP"] != null && !string.IsNullOrEmpty(dr["CEP"].ToString()))
            {
              if (dr["CEP"].ToString().Length > 10)
              {
                return "O Cep deve possuir no máximo 10 caracteres.";
              }
            }

            if (dr["NOME_ESTADO"] != null && !string.IsNullOrEmpty(dr["NOME_ESTADO"].ToString()))
            {
              if (dr["NOME_ESTADO"].ToString().Length > 50)
              {
                return "O Nome_estado deve possuir no máximo 50 caracteres.";
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

    public static Hd_municipio_estrangeiro Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_municipio_estrangeiro Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_municipio_estrangeiro tab = new Hd_municipio_estrangeiro();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_municipio_estrangeiro tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @municipio_estrangeiro, string @pais, string @nome, string @estado, string @cep, 
        string @nome_estado
      ) {
        return (Row)Add(new object[] {
          @municipio_estrangeiro, @pais, @nome, @estado, @cep, @nome_estado
        });
      }

      public Row Find(
        string @municipio_estrangeiro
      ) {
        return (Row)Find(new object[] {
          @municipio_estrangeiro
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
      /// MUNICIPIO_ESTRANGEIRO: VARCHAR(10), Primary Key, Not Null
      /// </summary>
      public string Municipio_estrangeiro { get { return (string)this["municipio_estrangeiro"]; } set { this["municipio_estrangeiro"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PAIS: VARCHAR(10), Not Null
      /// </summary>
      public string Pais { get { return (string)this["pais"]; } set { this["pais"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(50), Not Null
      /// </summary>
      public string Nome { get { return (string)this["nome"]; } set { this["nome"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ESTADO: VARCHAR(20)
      /// </summary>
      public string Estado { get { return (string)this["estado"]; } set { this["estado"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CEP: VARCHAR(10)
      /// </summary>
      public string Cep { get { return (string)this["cep"]; } set { this["cep"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME_ESTADO: VARCHAR(50)
      /// </summary>
      public string Nome_estado { get { return (string)this["nome_estado"]; } set { this["nome_estado"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @municipio_estrangeiro, string @pais, string @nome) {
        return Insert(connection,
                      @municipio_estrangeiro, @pais, @nome,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @municipio_estrangeiro, string @pais, string @nome,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_municipio_estrangeiro(),
          new string[] { "municipio_estrangeiro", "pais", "nome" },
          new object[] { @municipio_estrangeiro, @pais, @nome },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkMunicipio_estrangeiro,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_municipio_estrangeiro(),
          new string[] { "municipio_estrangeiro" },
          new object[] { pkMunicipio_estrangeiro },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkMunicipio_estrangeiro) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_municipio_estrangeiro(),
          new string[] { "municipio_estrangeiro" },
          new object[] { pkMunicipio_estrangeiro }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkMunicipio_estrangeiro) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_municipio_estrangeiro(),
          new string[] { "municipio_estrangeiro" },
          new object[] { pkMunicipio_estrangeiro }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Municipio_estrangeiro { get { return (string)row["municipio_estrangeiro", DataRowVersion.Original]; } }
        public string Pais { get { return (string)row["pais", DataRowVersion.Original]; } }
        public string Nome { get { return (string)row["nome", DataRowVersion.Original]; } }
        public string Estado { get { return (string)row["estado", DataRowVersion.Original]; } }
        public string Cep { get { return (string)row["cep", DataRowVersion.Original]; } }
        public string Nome_estado { get { return (string)row["nome_estado", DataRowVersion.Original]; } }

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
    #region Coluna: MUNICIPIO_ESTRANGEIRO
    private ColumnMunicipio_estrangeiro pvColMunicipio_estrangeiro;

    public sealed class ColumnMunicipio_estrangeiro : TDataColumn {

      public ColumnMunicipio_estrangeiro() {
        this.ColumnName = "municipio_estrangeiro";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_municipio_estrangeiro.municipio_estrangeiro"; } }
      public override string OriginalCol { get { return "municipio_estrangeiro"; } }
      public override string RefCol { get { return "HD_MUNICIPIO_ESTRANGEIRO.MUNICIPIO_ESTRANGEIRO"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 10; } }
      public override string SqlServerType { get { return "VARCHAR(10)"; } }

    }
    #endregion

    #region Coluna: PAIS
    private ColumnPais pvColPais;

    public sealed class ColumnPais : TDataColumn {

      public ColumnPais() {
        this.ColumnName = "pais";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_municipio_estrangeiro.pais"; } }
      public override string OriginalCol { get { return "pais"; } }
      public override string RefCol { get { return "HD_MUNICIPIO_ESTRANGEIRO.PAIS"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 10; } }
      public override string SqlServerType { get { return "VARCHAR(10)"; } }

    }
    #endregion

    #region Coluna: NOME
    private ColumnNome pvColNome;

    public sealed class ColumnNome : TDataColumn {

      public ColumnNome() {
        this.ColumnName = "nome";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_municipio_estrangeiro.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "HD_MUNICIPIO_ESTRANGEIRO.NOME"; } }
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

    #region Coluna: ESTADO
    private ColumnEstado pvColEstado;

    public sealed class ColumnEstado : TDataColumn {

      public ColumnEstado() {
        this.ColumnName = "estado";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_municipio_estrangeiro.estado"; } }
      public override string OriginalCol { get { return "estado"; } }
      public override string RefCol { get { return "HD_MUNICIPIO_ESTRANGEIRO.ESTADO"; } }
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

    #region Coluna: CEP
    private ColumnCep pvColCep;

    public sealed class ColumnCep : TDataColumn {

      public ColumnCep() {
        this.ColumnName = "cep";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_municipio_estrangeiro.cep"; } }
      public override string OriginalCol { get { return "cep"; } }
      public override string RefCol { get { return "HD_MUNICIPIO_ESTRANGEIRO.CEP"; } }
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

    #region Coluna: NOME_ESTADO
    private ColumnNome_estado pvColNome_estado;

    public sealed class ColumnNome_estado : TDataColumn {

      public ColumnNome_estado() {
        this.ColumnName = "nome_estado";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_municipio_estrangeiro.nome_estado"; } }
      public override string OriginalCol { get { return "nome_estado"; } }
      public override string RefCol { get { return "HD_MUNICIPIO_ESTRANGEIRO.NOME_ESTADO"; } }
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

  public partial class Hd_pais : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_pais() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_pais");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_pais");
      }
      catch{}
    }

    public Hd_pais() {
      pvColPais = new ColumnPais(); Columns.Add(pvColPais);
      pvColNome = new ColumnNome(); Columns.Add(pvColNome);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_pais");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @pais, string @nome) {
      Row row = NewRow();

      row.Pais = @pais;
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
      get { return "HD_PAIS"; }
    }

    protected override sealed string Name { get { return "País"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColPais });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_pais_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_pais_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_pais_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_pais_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkPais", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "PAIS", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("pais", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PAIS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
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
          if(lower == "pais") { result.Add(toLower ? "pais" : column); continue; }
          if(lower == "nome") { result.Add(toLower ? "nome" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "pais", "nome" }
                          : new string[] { "PAIS", "NOME" });
        else
          result.AddRange(toLower
                          ? new string[] { "pais", "nome" }
                          : new string[] { "PAIS", "NOME" });

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
        "hd_pais.pais"
      });
      if(scColumns.Contains("nome")) list.Add("hd_pais.nome");
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
          "FROM hd_pais " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_pais");
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
        da.TableMappings.Add("Table", "Hd_pais");
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
            if (dr["PAIS"] != null && string.IsNullOrEmpty(dr["PAIS"].ToString()))
              dr["PAIS"] = DBNull.Value;
            if (dr["NOME"] != null && string.IsNullOrEmpty(dr["NOME"].ToString()))
              dr["NOME"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["PAIS"] == null || string.IsNullOrEmpty(dr["PAIS"].ToString()))
            {
              return "Favor informar o Pais.";
            }

            if (dr["PAIS"] != null && !string.IsNullOrEmpty(dr["PAIS"].ToString()))
            {
              if (dr["PAIS"].ToString().Length > 10)
              {
                return "O Pais deve possuir no máximo 10 caracteres.";
              }
            }

            if (dr["NOME"] == null || string.IsNullOrEmpty(dr["NOME"].ToString()))
            {
              return "Favor informar o Nome.";
            }

            if (dr["NOME"] != null && !string.IsNullOrEmpty(dr["NOME"].ToString()))
            {
              if (dr["NOME"].ToString().Length > 50)
              {
                return "O Nome deve possuir no máximo 50 caracteres.";
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

    public static Hd_pais Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_pais Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_pais tab = new Hd_pais();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_pais tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @pais, string @nome
      ) {
        return (Row)Add(new object[] {
          @pais, @nome
        });
      }

      public Row Find(
        string @pais
      ) {
        return (Row)Find(new object[] {
          @pais
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
      /// PAIS: VARCHAR(10), Primary Key, Not Null
      /// </summary>
      public string Pais { get { return (string)this["pais"]; } set { this["pais"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(50), Not Null
      /// </summary>
      public string Nome { get { return (string)this["nome"]; } set { this["nome"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @pais, string @nome) {
        return Insert(connection,
                      @pais, @nome,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @pais, string @nome,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_pais(),
          new string[] { "pais", "nome" },
          new object[] { @pais, @nome },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkPais,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_pais(),
          new string[] { "pais" },
          new object[] { pkPais },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkPais) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_pais(),
          new string[] { "pais" },
          new object[] { pkPais }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkPais) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_pais(),
          new string[] { "pais" },
          new object[] { pkPais }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Pais { get { return (string)row["pais", DataRowVersion.Original]; } }
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
    #region Coluna: PAIS
    private ColumnPais pvColPais;

    public sealed class ColumnPais : TDataColumn {

      public ColumnPais() {
        this.ColumnName = "pais";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_pais.pais"; } }
      public override string OriginalCol { get { return "pais"; } }
      public override string RefCol { get { return "HD_PAIS.PAIS"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 10; } }
      public override string SqlServerType { get { return "VARCHAR(10)"; } }

    }
    #endregion

    #region Coluna: NOME
    private ColumnNome pvColNome;

    public sealed class ColumnNome : TDataColumn {

      public ColumnNome() {
        this.ColumnName = "nome";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_pais.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "HD_PAIS.NOME"; } }
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

