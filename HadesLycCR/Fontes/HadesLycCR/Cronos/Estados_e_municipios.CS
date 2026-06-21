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
  public class Estados_e_Municipios : TDataSet {
    private const string DataSetName_Def = "Estados_e_Municipios";
    private static ConstructorInfo ctorTCustom = null;
    private CustomBase tcustom = null;
    #region Variáveis private para as tabelas deste dataset
    private Hd_bairro pvTabHd_bairro;
    private Hd_logradouro pvTabHd_logradouro;
    private Hd_municipio pvTabHd_municipio;
    private Hd_tipo_logradouro pvTabHd_tipo_logradouro;
    private Hd_trecho_logradouro pvTabHd_trecho_logradouro;
    private Hd_uf pvTabHd_uf;
    #endregion

    static Estados_e_Municipios() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Estados_e_Municipios");
      }
      catch{}
    }

    public Estados_e_Municipios() : base() {
      DataSetName = DataSetName_Def;

      pvTabHd_bairro = new Hd_bairro();
      pvTabHd_logradouro = new Hd_logradouro();
      pvTabHd_municipio = new Hd_municipio();
      pvTabHd_tipo_logradouro = new Hd_tipo_logradouro();
      pvTabHd_trecho_logradouro = new Hd_trecho_logradouro();
      pvTabHd_uf = new Hd_uf();

      Tables.Add(pvTabHd_bairro);
      Tables.Add(pvTabHd_logradouro);
      Tables.Add(pvTabHd_municipio);
      Tables.Add(pvTabHd_tipo_logradouro);
      Tables.Add(pvTabHd_trecho_logradouro);
      Tables.Add(pvTabHd_uf);

      CreateRelations();

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
    }


    [DefaultValue(DataSetName_Def)]
    public new string DataSetName {
      get { return base.DataSetName; }
      set { base.DataSetName = value == null ? string.Empty : value; }
    }

    private void CreateRelations() {
      Relations.Add("fk_bairro_municipio",
                    new DataColumn[] { pvTabHd_municipio.Columns["municipio"] },
                    new DataColumn[] { pvTabHd_bairro.Columns["municipio"] }, false);
      Relations.Add("fk_logradouro_municipio",
                    new DataColumn[] { pvTabHd_municipio.Columns["municipio"] },
                    new DataColumn[] { pvTabHd_logradouro.Columns["municipio"] }, false);
      Relations.Add("fk_municipio_uf",
                    new DataColumn[] { pvTabHd_uf.Columns["uf"] },
                    new DataColumn[] { pvTabHd_municipio.Columns["uf"] }, false);
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
          pvTabHd_bairro.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_bairro); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabHd_logradouro.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_logradouro); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabHd_municipio.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_municipio); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabHd_tipo_logradouro.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_tipo_logradouro); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabHd_trecho_logradouro.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_trecho_logradouro); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabHd_uf.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_uf); if(row != null) return DataLib.RowErrorsToString(row);
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
    public Hd_bairro Hd_bairro { get { return pvTabHd_bairro; } }

    [
      Browsable(false),
    ]
    public Hd_logradouro Hd_logradouro { get { return pvTabHd_logradouro; } }

    [
      Browsable(false),
    ]
    public Hd_municipio Hd_municipio { get { return pvTabHd_municipio; } }

    [
      Browsable(false),
    ]
    public Hd_tipo_logradouro Hd_tipo_logradouro { get { return pvTabHd_tipo_logradouro; } }

    [
      Browsable(false),
    ]
    public Hd_trecho_logradouro Hd_trecho_logradouro { get { return pvTabHd_trecho_logradouro; } }

    [
      Browsable(false),
    ]
    public Hd_uf Hd_uf { get { return pvTabHd_uf; } }

    #endregion

    public abstract class CustomBase {
      public abstract string Put(Techne.HadesLyc.CR.Estados_e_Municipios ds, TConnectionWritable connection);
    }

  }


  public partial class Hd_bairro : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_bairro() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_bairro");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_bairro");
      }
      catch{}
    }

    public Hd_bairro() {
      pvColMunicipio = new ColumnMunicipio(); Columns.Add(pvColMunicipio);
      pvColBairro = new ColumnBairro(); Columns.Add(pvColBairro);
      pvColNome = new ColumnNome(); Columns.Add(pvColNome);
      pvColIx_buscatexto = new ColumnIx_buscatexto(); Columns.Add(pvColIx_buscatexto);
      pvColMunicipio_nome = new ColumnMunicipio_nome(); Columns.Add(pvColMunicipio_nome);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_bairro");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @municipio, string @bairro, string @nome) {
      Row row = NewRow();

      row.Municipio = @municipio;
      row.Bairro = @bairro;
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
      get { return "HD_BAIRRO"; }
    }

    protected override sealed string Name { get { return "Bairros"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColMunicipio, pvColBairro });
      pvColIx_buscatexto.AllowDBNull = true;
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_bairro_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_bairro_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_bairro_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_bairro_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkMunicipio", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "MUNICIPIO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkBairro", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "BAIRRO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("municipio", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "MUNICIPIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("bairro", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "BAIRRO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("nome", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NOME", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("ix_buscatexto", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 28, 10, "IX_BUSCATEXTO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "municipio") { result.Add(toLower ? "municipio" : column); continue; }
          if(lower == "bairro") { result.Add(toLower ? "bairro" : column); continue; }
          if(lower == "nome") { result.Add(toLower ? "nome" : column); continue; }
          if(lower == "ix_buscatexto") { result.Add(toLower ? "ix_buscatexto" : column); continue; }
          if(!tabPrincipal) {
            if(lower == "municipio_nome") { result.Add(toLower ? "municipio_nome" : column); continue; }
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "municipio", "bairro", "nome", "ix_buscatexto" }
                          : new string[] { "MUNICIPIO", "BAIRRO", "NOME", "IX_BUSCATEXTO" });
        else
          result.AddRange(toLower
                          ? new string[] { "municipio", "bairro", "nome", "ix_buscatexto", "municipio_nome" }
                          : new string[] { "MUNICIPIO", "BAIRRO", "NOME", "IX_BUSCATEXTO", "MUNICIPIO_NOME" });

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
        "hd_bairro.municipio", "hd_bairro.bairro"
      });
      if(scColumns.Contains("nome")) list.Add("hd_bairro.nome");
      if(scColumns.Contains("ix_buscatexto")) list.Add("hd_bairro.ix_buscatexto");
      if(scColumns.Contains("municipio_nome")) list.Add("hd_municipio.nome municipio_nome");
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
          "FROM hd_bairro " +
               "INNER JOIN hd_municipio " +
                 "ON hd_municipio.municipio = hd_bairro.municipio " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_bairro");
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
        da.TableMappings.Add("Table", "Hd_bairro");
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
            if (dr["MUNICIPIO"] != null && string.IsNullOrEmpty(dr["MUNICIPIO"].ToString()))
              dr["MUNICIPIO"] = DBNull.Value;
            if (dr["BAIRRO"] != null && string.IsNullOrEmpty(dr["BAIRRO"].ToString()))
              dr["BAIRRO"] = DBNull.Value;
            if (dr["NOME"] != null && string.IsNullOrEmpty(dr["NOME"].ToString()))
              dr["NOME"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["MUNICIPIO"] == null || string.IsNullOrEmpty(dr["MUNICIPIO"].ToString()))
            {
              return "Favor informar o município.";
            }

            if (dr["MUNICIPIO"] != null && !string.IsNullOrEmpty(dr["MUNICIPIO"].ToString()))
            {
              if (dr["MUNICIPIO"].ToString().Length > 10)
              {
                return "O município deve possuir no máximo 10 caracteres.";
              }
            }

            if (dr["BAIRRO"] == null || string.IsNullOrEmpty(dr["BAIRRO"].ToString()))
            {
              return "Favor informar o bairro.";
            }

            if (dr["BAIRRO"] != null && !string.IsNullOrEmpty(dr["BAIRRO"].ToString()))
            {
              if (dr["BAIRRO"].ToString().Length > 10)
              {
                return "O bairro deve possuir no máximo 10 caracteres.";
              }
            }

            if (dr["NOME"] == null || string.IsNullOrEmpty(dr["NOME"].ToString()))
            {
              return "Favor informar o nome do bairro.";
            }

            if (dr["NOME"] != null && !string.IsNullOrEmpty(dr["NOME"].ToString()))
            {
              if (dr["NOME"].ToString().Length > 50)
              {
                return "O nome do bairro deve possuir no máximo 50 caracteres.";
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

    public static Hd_bairro Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_bairro Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_bairro tab = new Hd_bairro();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_bairro tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @municipio, string @bairro, string @nome, decimal? @ix_buscatexto
      ) {
        return (Row)Add(new object[] {
          @municipio, @bairro, @nome, @ix_buscatexto
        });
      }

      public Row Find(
        string @municipio, string @bairro
      ) {
        return (Row)Find(new object[] {
          @municipio, @bairro
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
      /// MUNICIPIO: VARCHAR(10), Primary Key, Not Null
      /// </summary>
      public string Municipio { get { return (string)this["municipio"]; } set { this["municipio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// BAIRRO: VARCHAR(10), Primary Key, Not Null
      /// </summary>
      public string Bairro { get { return (string)this["bairro"]; } set { this["bairro"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(50), Not Null
      /// </summary>
      public string Nome { get { return (string)this["nome"]; } set { this["nome"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// IX_BUSCATEXTO: NUMERIC(18), AutoNumber, Not Null
      /// </summary>
      public decimal? Ix_buscatexto { get { return (decimal?)this["ix_buscatexto"]; } set { this["ix_buscatexto"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(50), Not Null
      /// </summary>
      public string Municipio_nome { get { return (string)this["municipio_nome"]; } set { this["municipio_nome"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @municipio, string @bairro, string @nome) {
        return Insert(connection,
                      @municipio, @bairro, @nome,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @municipio, string @bairro, string @nome,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_bairro(),
          new string[] { "municipio", "bairro", "nome" },
          new object[] { @municipio, @bairro, @nome },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkMunicipio, string pkBairro,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_bairro(),
          new string[] { "municipio", "bairro" },
          new object[] { pkMunicipio, pkBairro },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkMunicipio, string pkBairro) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_bairro(),
          new string[] { "municipio", "bairro" },
          new object[] { pkMunicipio, pkBairro }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkMunicipio, string pkBairro) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_bairro(),
          new string[] { "municipio", "bairro" },
          new object[] { pkMunicipio, pkBairro }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Municipio { get { return (string)row["municipio", DataRowVersion.Original]; } }
        public string Bairro { get { return (string)row["bairro", DataRowVersion.Original]; } }
        public string Nome { get { return (string)row["nome", DataRowVersion.Original]; } }
        public decimal? Ix_buscatexto { get { return (decimal?)row["ix_buscatexto", DataRowVersion.Original]; } }
        public string Municipio_nome { get { return (string)row["municipio_nome", DataRowVersion.Original]; } }

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
      public override string FullCol { get { return "hd_bairro.municipio"; } }
      public override string OriginalCol { get { return "municipio"; } }
      public override string RefCol { get { return "HD_BAIRRO.MUNICIPIO"; } }
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

    #region Coluna: BAIRRO
    private ColumnBairro pvColBairro;

    public sealed class ColumnBairro : TDataColumn {

      public ColumnBairro() {
        this.ColumnName = "bairro";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "bairro";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_bairro.bairro"; } }
      public override string OriginalCol { get { return "bairro"; } }
      public override string RefCol { get { return "HD_BAIRRO.BAIRRO"; } }
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


      protected override string GetName(){return "nome do bairro";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_bairro.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "HD_BAIRRO.NOME"; } }
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

    #region Coluna: IX_BUSCATEXTO
    private ColumnIx_buscatexto pvColIx_buscatexto;

    public sealed class ColumnIx_buscatexto : TDataColumn {

      public ColumnIx_buscatexto() {
        this.ColumnName = "ix_buscatexto";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_bairro.ix_buscatexto"; } }
      public override string OriginalCol { get { return "ix_buscatexto"; } }
      public override string RefCol { get { return "HD_BAIRRO.IX_BUSCATEXTO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 19; } }
      public override int Precision { get { return 18; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(18)"; } }

    }
    #endregion

    #region Coluna: MUNICIPIO_NOME
    private ColumnMunicipio_nome pvColMunicipio_nome;

    public sealed class ColumnMunicipio_nome : TDataColumn {

      public ColumnMunicipio_nome() {
        this.ColumnName = "municipio_nome";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_municipio.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "HD_MUNICIPIO.NOME"; } }
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

  public partial class Hd_logradouro : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_logradouro() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_logradouro");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_logradouro");
      }
      catch{}
    }

    public Hd_logradouro() {
      pvColMunicipio = new ColumnMunicipio(); Columns.Add(pvColMunicipio);
      pvColLogradouro = new ColumnLogradouro(); Columns.Add(pvColLogradouro);
      pvColTipo_logr = new ColumnTipo_logr(); Columns.Add(pvColTipo_logr);
      pvColNome = new ColumnNome(); Columns.Add(pvColNome);
      pvColOficial = new ColumnOficial(); Columns.Add(pvColOficial);
      pvColSinonimos = new ColumnSinonimos(); Columns.Add(pvColSinonimos);
      pvColIx_buscatexto = new ColumnIx_buscatexto(); Columns.Add(pvColIx_buscatexto);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_logradouro");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @municipio, string @logradouro, string @tipo_logr, string @nome, 
                      string @oficial) {
      Row row = NewRow();

      row.Municipio = @municipio;
      row.Logradouro = @logradouro;
      row.Tipo_logr = @tipo_logr;
      row.Nome = @nome;
      row.Oficial = @oficial;

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
      get { return "HD_LOGRADOURO"; }
    }

    protected override sealed string Name { get { return "Logradouros"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColMunicipio, pvColLogradouro });
      pvColIx_buscatexto.AllowDBNull = true;
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_logradouro_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_logradouro_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_logradouro_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_logradouro_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkMunicipio", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "MUNICIPIO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkLogradouro", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "LOGRADOURO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("municipio", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "MUNICIPIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("logradouro", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "LOGRADOURO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("tipo_logr", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TIPO_LOGR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("nome", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NOME", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("oficial", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "OFICIAL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("sinonimos", OleDbType.VarChar, 400, ParameterDirection.InputOutput, true, 0, 0, "SINONIMOS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("ix_buscatexto", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 28, 10, "IX_BUSCATEXTO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "municipio") { result.Add(toLower ? "municipio" : column); continue; }
          if(lower == "logradouro") { result.Add(toLower ? "logradouro" : column); continue; }
          if(lower == "tipo_logr") { result.Add(toLower ? "tipo_logr" : column); continue; }
          if(lower == "nome") { result.Add(toLower ? "nome" : column); continue; }
          if(lower == "oficial") { result.Add(toLower ? "oficial" : column); continue; }
          if(lower == "sinonimos") { result.Add(toLower ? "sinonimos" : column); continue; }
          if(lower == "ix_buscatexto") { result.Add(toLower ? "ix_buscatexto" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "municipio", "logradouro", "tipo_logr", "nome", "oficial", "sinonimos", 
                                           "ix_buscatexto" }
                          : new string[] { "MUNICIPIO", "LOGRADOURO", "TIPO_LOGR", "NOME", "OFICIAL", "SINONIMOS", 
                                           "IX_BUSCATEXTO" });
        else
          result.AddRange(toLower
                          ? new string[] { "municipio", "logradouro", "tipo_logr", "nome", "oficial", "sinonimos", 
                                           "ix_buscatexto" }
                          : new string[] { "MUNICIPIO", "LOGRADOURO", "TIPO_LOGR", "NOME", "OFICIAL", "SINONIMOS", 
                                           "IX_BUSCATEXTO" });

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
        "hd_logradouro.municipio", "hd_logradouro.logradouro"
      });
      if(scColumns.Contains("tipo_logr")) list.Add("hd_logradouro.tipo_logr");
      if(scColumns.Contains("nome")) list.Add("hd_logradouro.nome");
      if(scColumns.Contains("oficial")) list.Add("hd_logradouro.oficial");
      if(scColumns.Contains("sinonimos")) list.Add("hd_logradouro.sinonimos");
      if(scColumns.Contains("ix_buscatexto")) list.Add("hd_logradouro.ix_buscatexto");
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
          "FROM hd_logradouro " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_logradouro");
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
        da.TableMappings.Add("Table", "Hd_logradouro");
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
            if (dr["MUNICIPIO"] != null && string.IsNullOrEmpty(dr["MUNICIPIO"].ToString()))
              dr["MUNICIPIO"] = DBNull.Value;
            if (dr["LOGRADOURO"] != null && string.IsNullOrEmpty(dr["LOGRADOURO"].ToString()))
              dr["LOGRADOURO"] = DBNull.Value;
            if (dr["TIPO_LOGR"] != null && string.IsNullOrEmpty(dr["TIPO_LOGR"].ToString()))
              dr["TIPO_LOGR"] = DBNull.Value;
            if (dr["NOME"] != null && string.IsNullOrEmpty(dr["NOME"].ToString()))
              dr["NOME"] = DBNull.Value;
            if (dr["OFICIAL"] != null && string.IsNullOrEmpty(dr["OFICIAL"].ToString()))
              dr["OFICIAL"] = DBNull.Value;
            if (dr["SINONIMOS"] != null && string.IsNullOrEmpty(dr["SINONIMOS"].ToString()))
              dr["SINONIMOS"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["MUNICIPIO"] == null || string.IsNullOrEmpty(dr["MUNICIPIO"].ToString()))
            {
              return "Favor informar o município.";
            }

            if (dr["MUNICIPIO"] != null && !string.IsNullOrEmpty(dr["MUNICIPIO"].ToString()))
            {
              if (dr["MUNICIPIO"].ToString().Length > 10)
              {
                return "O município deve possuir no máximo 10 caracteres.";
              }
            }

            if (dr["LOGRADOURO"] == null || string.IsNullOrEmpty(dr["LOGRADOURO"].ToString()))
            {
              return "Favor informar o logradouro.";
            }

            if (dr["LOGRADOURO"] != null && !string.IsNullOrEmpty(dr["LOGRADOURO"].ToString()))
            {
              if (dr["LOGRADOURO"].ToString().Length > 10)
              {
                return "O logradouro deve possuir no máximo 10 caracteres.";
              }
            }

            if (dr["TIPO_LOGR"] == null || string.IsNullOrEmpty(dr["TIPO_LOGR"].ToString()))
            {
              return "Favor informar o tipo.";
            }

            if (dr["TIPO_LOGR"] != null && !string.IsNullOrEmpty(dr["TIPO_LOGR"].ToString()))
            {
              if (dr["TIPO_LOGR"].ToString().Length > 5)
              {
                return "O tipo deve possuir no máximo 5 caracteres.";
              }
            }

            if (dr["NOME"] == null || string.IsNullOrEmpty(dr["NOME"].ToString()))
            {
              return "Favor informar o nome do logradouro.";
            }

            if (dr["NOME"] != null && !string.IsNullOrEmpty(dr["NOME"].ToString()))
            {
              if (dr["NOME"].ToString().Length > 100)
              {
                return "O nome do logradouro deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["OFICIAL"] == null || string.IsNullOrEmpty(dr["OFICIAL"].ToString()))
            {
              return "Favor informar o Oficial.";
            }

            if (dr["OFICIAL"] != null && !string.IsNullOrEmpty(dr["OFICIAL"].ToString()))
            {
              if (dr["OFICIAL"].ToString().Length > 1)
              {
                return "O Oficial deve possuir no máximo 1 caractere.";
              }

              if (dr["OFICIAL"] != null && !string.IsNullOrEmpty(dr["OFICIAL"].ToString()))
              {
                List<object> lstOFICIAL = new List<object>(new object[]{"N", "S"});
                if(!lstOFICIAL.Contains(dr["OFICIAL"].ToString()))
                {
                  return "Os valores permitidos para o Oficial são ''N'' ou ''S''.";
                }
              }
            }

            if (dr["SINONIMOS"] != null && !string.IsNullOrEmpty(dr["SINONIMOS"].ToString()))
            {
              if (dr["SINONIMOS"].ToString().Length > 300)
              {
                return "O Sinonimos deve possuir no máximo 300 caracteres.";
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

    public static Hd_logradouro Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_logradouro Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_logradouro tab = new Hd_logradouro();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_logradouro tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @municipio, string @logradouro, string @tipo_logr, string @nome, string @oficial, 
        string @sinonimos, decimal? @ix_buscatexto
      ) {
        return (Row)Add(new object[] {
          @municipio, @logradouro, @tipo_logr, @nome, @oficial, @sinonimos, @ix_buscatexto
        });
      }

      public Row Find(
        string @municipio, string @logradouro
      ) {
        return (Row)Find(new object[] {
          @municipio, @logradouro
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
      /// MUNICIPIO: VARCHAR(10), Primary Key, Not Null
      /// </summary>
      public string Municipio { get { return (string)this["municipio"]; } set { this["municipio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// LOGRADOURO: VARCHAR(10), Primary Key, Not Null
      /// </summary>
      public string Logradouro { get { return (string)this["logradouro"]; } set { this["logradouro"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TIPO_LOGR: VARCHAR(5), Not Null
      /// </summary>
      public string Tipo_logr { get { return (string)this["tipo_logr"]; } set { this["tipo_logr"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(100), Not Null
      /// </summary>
      public string Nome { get { return (string)this["nome"]; } set { this["nome"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// OFICIAL: CHAR(1), Not Null
      /// </summary>
      public string Oficial { get { return (string)this["oficial"]; } set { this["oficial"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SINONIMOS: VARCHAR(300)
      /// </summary>
      public string Sinonimos { get { return (string)this["sinonimos"]; } set { this["sinonimos"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// IX_BUSCATEXTO: NUMERIC(18), AutoNumber, Not Null
      /// </summary>
      public decimal? Ix_buscatexto { get { return (decimal?)this["ix_buscatexto"]; } set { this["ix_buscatexto"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @municipio, string @logradouro, string @tipo_logr, string @nome, string @oficial) {
        return Insert(connection,
                      @municipio, @logradouro, @tipo_logr, @nome, @oficial,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @municipio, string @logradouro, string @tipo_logr, string @nome, string @oficial,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_logradouro(),
          new string[] { "municipio", "logradouro", "tipo_logr", "nome", "oficial" },
          new object[] { @municipio, @logradouro, @tipo_logr, @nome, @oficial },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkMunicipio, string pkLogradouro,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_logradouro(),
          new string[] { "municipio", "logradouro" },
          new object[] { pkMunicipio, pkLogradouro },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkMunicipio, string pkLogradouro) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_logradouro(),
          new string[] { "municipio", "logradouro" },
          new object[] { pkMunicipio, pkLogradouro }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkMunicipio, string pkLogradouro) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_logradouro(),
          new string[] { "municipio", "logradouro" },
          new object[] { pkMunicipio, pkLogradouro }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Municipio { get { return (string)row["municipio", DataRowVersion.Original]; } }
        public string Logradouro { get { return (string)row["logradouro", DataRowVersion.Original]; } }
        public string Tipo_logr { get { return (string)row["tipo_logr", DataRowVersion.Original]; } }
        public string Nome { get { return (string)row["nome", DataRowVersion.Original]; } }
        public string Oficial { get { return (string)row["oficial", DataRowVersion.Original]; } }
        public string Sinonimos { get { return (string)row["sinonimos", DataRowVersion.Original]; } }
        public decimal? Ix_buscatexto { get { return (decimal?)row["ix_buscatexto", DataRowVersion.Original]; } }

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
      public override string FullCol { get { return "hd_logradouro.municipio"; } }
      public override string OriginalCol { get { return "municipio"; } }
      public override string RefCol { get { return "HD_LOGRADOURO.MUNICIPIO"; } }
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

    #region Coluna: LOGRADOURO
    private ColumnLogradouro pvColLogradouro;

    public sealed class ColumnLogradouro : TDataColumn {

      public ColumnLogradouro() {
        this.ColumnName = "logradouro";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "logradouro";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_logradouro.logradouro"; } }
      public override string OriginalCol { get { return "logradouro"; } }
      public override string RefCol { get { return "HD_LOGRADOURO.LOGRADOURO"; } }
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

    #region Coluna: TIPO_LOGR
    private ColumnTipo_logr pvColTipo_logr;

    public sealed class ColumnTipo_logr : TDataColumn {

      public ColumnTipo_logr() {
        this.ColumnName = "tipo_logr";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "tipo";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_logradouro.tipo_logr"; } }
      public override string OriginalCol { get { return "tipo_logr"; } }
      public override string RefCol { get { return "HD_LOGRADOURO.TIPO_LOGR"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 5; } }
      public override string SqlServerType { get { return "VARCHAR(5)"; } }

    }
    #endregion

    #region Coluna: NOME
    private ColumnNome pvColNome;

    public sealed class ColumnNome : TDataColumn {

      public ColumnNome() {
        this.ColumnName = "nome";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "nome do logradouro";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_logradouro.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "HD_LOGRADOURO.NOME"; } }
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

    #region Coluna: OFICIAL
    private ColumnOficial pvColOficial;

    public sealed class ColumnOficial : TDataColumn {

      public ColumnOficial() {
        this.ColumnName = "oficial";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_logradouro.oficial"; } }
      public override string OriginalCol { get { return "oficial"; } }
      public override string RefCol { get { return "HD_LOGRADOURO.OFICIAL"; } }
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

    #region Coluna: SINONIMOS
    private ColumnSinonimos pvColSinonimos;

    public sealed class ColumnSinonimos : TDataColumn {

      public ColumnSinonimos() {
        this.ColumnName = "sinonimos";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_logradouro.sinonimos"; } }
      public override string OriginalCol { get { return "sinonimos"; } }
      public override string RefCol { get { return "HD_LOGRADOURO.SINONIMOS"; } }
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

    #region Coluna: IX_BUSCATEXTO
    private ColumnIx_buscatexto pvColIx_buscatexto;

    public sealed class ColumnIx_buscatexto : TDataColumn {

      public ColumnIx_buscatexto() {
        this.ColumnName = "ix_buscatexto";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_logradouro.ix_buscatexto"; } }
      public override string OriginalCol { get { return "ix_buscatexto"; } }
      public override string RefCol { get { return "HD_LOGRADOURO.IX_BUSCATEXTO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 19; } }
      public override int Precision { get { return 18; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(18)"; } }

    }
    #endregion
    #endregion
  }

  public partial class Hd_municipio : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_municipio() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_municipio");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_municipio");
      }
      catch{}
    }

    public Hd_municipio() {
      pvColMunicipio = new ColumnMunicipio(); Columns.Add(pvColMunicipio);
      pvColUf = new ColumnUf(); Columns.Add(pvColUf);
      pvColNome = new ColumnNome(); Columns.Add(pvColNome);
      pvColVerbairro = new ColumnVerbairro(); Columns.Add(pvColVerbairro);
      pvColVerlog = new ColumnVerlog(); Columns.Add(pvColVerlog);
      pvColCep = new ColumnCep(); Columns.Add(pvColCep);
      pvColDesvinc_bairro_logr = new ColumnDesvinc_bairro_logr(); Columns.Add(pvColDesvinc_bairro_logr);
      pvColCodigo_ibge = new ColumnCodigo_ibge(); Columns.Add(pvColCodigo_ibge);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_municipio");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @municipio, string @uf, string @nome) {
      Row row = NewRow();

      row.Municipio = @municipio;
      row.Uf = @uf;
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
      get { return "HD_MUNICIPIO"; }
    }

    protected override sealed string Name { get { return "Municípios"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColMunicipio });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_municipio_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_municipio_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_municipio_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_municipio_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkMunicipio", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "MUNICIPIO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("municipio", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "MUNICIPIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("uf", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "UF", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("nome", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NOME", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("verbairro", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "VERBAIRRO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("verlog", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "VERLOG", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("cep", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CEP", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("desvinc_bairro_logr", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "DESVINC_BAIRRO_LOGR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("codigo_ibge", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CODIGO_IBGE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "municipio") { result.Add(toLower ? "municipio" : column); continue; }
          if(lower == "uf") { result.Add(toLower ? "uf" : column); continue; }
          if(lower == "nome") { result.Add(toLower ? "nome" : column); continue; }
          if(lower == "verbairro") { result.Add(toLower ? "verbairro" : column); continue; }
          if(lower == "verlog") { result.Add(toLower ? "verlog" : column); continue; }
          if(lower == "cep") { result.Add(toLower ? "cep" : column); continue; }
          if(lower == "desvinc_bairro_logr") { result.Add(toLower ? "desvinc_bairro_logr" : column); continue; }
          if(lower == "codigo_ibge") { result.Add(toLower ? "codigo_ibge" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "municipio", "uf", "nome", "verbairro", "verlog", "cep", "desvinc_bairro_logr", 
                                           "codigo_ibge" }
                          : new string[] { "MUNICIPIO", "UF", "NOME", "VERBAIRRO", "VERLOG", "CEP", "DESVINC_BAIRRO_LOGR", 
                                           "CODIGO_IBGE" });
        else
          result.AddRange(toLower
                          ? new string[] { "municipio", "uf", "nome", "verbairro", "verlog", "cep", "desvinc_bairro_logr", 
                                           "codigo_ibge" }
                          : new string[] { "MUNICIPIO", "UF", "NOME", "VERBAIRRO", "VERLOG", "CEP", "DESVINC_BAIRRO_LOGR", 
                                           "CODIGO_IBGE" });

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
        "hd_municipio.municipio"
      });
      if(scColumns.Contains("uf")) list.Add("hd_municipio.uf");
      if(scColumns.Contains("nome")) list.Add("hd_municipio.nome");
      if(scColumns.Contains("verbairro")) list.Add("hd_municipio.verbairro");
      if(scColumns.Contains("verlog")) list.Add("hd_municipio.verlog");
      if(scColumns.Contains("cep")) list.Add("hd_municipio.cep");
      if(scColumns.Contains("desvinc_bairro_logr")) list.Add("hd_municipio.desvinc_bairro_logr");
      if(scColumns.Contains("codigo_ibge")) list.Add("hd_municipio.codigo_ibge");
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
          "FROM hd_municipio " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_municipio");
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
        da.TableMappings.Add("Table", "Hd_municipio");
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
            if (dr["MUNICIPIO"] != null && string.IsNullOrEmpty(dr["MUNICIPIO"].ToString()))
              dr["MUNICIPIO"] = DBNull.Value;
            if (dr["UF"] != null && string.IsNullOrEmpty(dr["UF"].ToString()))
              dr["UF"] = DBNull.Value;
            if (dr["NOME"] != null && string.IsNullOrEmpty(dr["NOME"].ToString()))
              dr["NOME"] = DBNull.Value;
            if (dr["VERBAIRRO"] != null && string.IsNullOrEmpty(dr["VERBAIRRO"].ToString()))
              dr["VERBAIRRO"] = DBNull.Value;
            if (dr["VERLOG"] != null && string.IsNullOrEmpty(dr["VERLOG"].ToString()))
              dr["VERLOG"] = DBNull.Value;
            if (dr["CEP"] != null && string.IsNullOrEmpty(dr["CEP"].ToString()))
              dr["CEP"] = DBNull.Value;
            if (dr["DESVINC_BAIRRO_LOGR"] != null && string.IsNullOrEmpty(dr["DESVINC_BAIRRO_LOGR"].ToString()))
              dr["DESVINC_BAIRRO_LOGR"] = DBNull.Value;
            if (dr["CODIGO_IBGE"] != null && string.IsNullOrEmpty(dr["CODIGO_IBGE"].ToString()))
              dr["CODIGO_IBGE"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["MUNICIPIO"] == null || string.IsNullOrEmpty(dr["MUNICIPIO"].ToString()))
            {
              return "Favor informar o Municipio.";
            }

            if (dr["MUNICIPIO"] != null && !string.IsNullOrEmpty(dr["MUNICIPIO"].ToString()))
            {
              if (dr["MUNICIPIO"].ToString().Length > 10)
              {
                return "O Municipio deve possuir no máximo 10 caracteres.";
              }
            }

            if (dr["UF"] == null || string.IsNullOrEmpty(dr["UF"].ToString()))
            {
              return "Favor informar o Uf.";
            }

            if (dr["UF"] != null && !string.IsNullOrEmpty(dr["UF"].ToString()))
            {
              if (dr["UF"].ToString().Length > 2)
              {
                return "O Uf deve possuir no máximo 2 caracteres.";
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

            if (dr["VERBAIRRO"] != null && !string.IsNullOrEmpty(dr["VERBAIRRO"].ToString()))
            {
              if (dr["VERBAIRRO"].ToString().Length > 1)
              {
                return "O Verbairro deve possuir no máximo 1 caractere.";
              }

              if (dr["VERBAIRRO"] != null && !string.IsNullOrEmpty(dr["VERBAIRRO"].ToString()))
              {
                List<object> lstVERBAIRRO = new List<object>(new object[]{"N", "S"});
                if(!lstVERBAIRRO.Contains(dr["VERBAIRRO"].ToString()))
                {
                  return "Os valores permitidos para o Verbairro são ''N'' ou ''S''.";
                }
              }
            }

            if (dr["VERLOG"] != null && !string.IsNullOrEmpty(dr["VERLOG"].ToString()))
            {
              if (dr["VERLOG"].ToString().Length > 1)
              {
                return "O Verlog deve possuir no máximo 1 caractere.";
              }

              if (dr["VERLOG"] != null && !string.IsNullOrEmpty(dr["VERLOG"].ToString()))
              {
                List<object> lstVERLOG = new List<object>(new object[]{"N", "S"});
                if(!lstVERLOG.Contains(dr["VERLOG"].ToString()))
                {
                  return "Os valores permitidos para o Verlog são ''N'' ou ''S''.";
                }
              }
            }

            if (dr["CEP"] != null && !string.IsNullOrEmpty(dr["CEP"].ToString()))
            {
              if (dr["CEP"].ToString().Length > 8)
              {
                return "O Cep deve possuir no máximo 8 caracteres.";
              }
            }

            if (dr["DESVINC_BAIRRO_LOGR"] != null && !string.IsNullOrEmpty(dr["DESVINC_BAIRRO_LOGR"].ToString()))
            {
              if (dr["DESVINC_BAIRRO_LOGR"].ToString().Length > 1)
              {
                return "O Desvinc_bairro_logr deve possuir no máximo 1 caractere.";
              }

              if (dr["DESVINC_BAIRRO_LOGR"] != null && !string.IsNullOrEmpty(dr["DESVINC_BAIRRO_LOGR"].ToString()))
              {
                List<object> lstDESVINC_BAIRRO_LOGR = new List<object>(new object[]{"N", "S"});
                if(!lstDESVINC_BAIRRO_LOGR.Contains(dr["DESVINC_BAIRRO_LOGR"].ToString()))
                {
                  return "Os valores permitidos para o Desvinc_bairro_logr são ''N'' ou ''S''.";
                }
              }
            }

            if (dr["CODIGO_IBGE"] != null && !string.IsNullOrEmpty(dr["CODIGO_IBGE"].ToString()))
            {
              if (dr["CODIGO_IBGE"].ToString().Length > 20)
              {
                return "O Codigo_ibge deve possuir no máximo 20 caracteres.";
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

    public static Hd_municipio Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_municipio Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_municipio tab = new Hd_municipio();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_municipio tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @municipio, string @uf, string @nome, string @verbairro, string @verlog, string @cep, 
        string @desvinc_bairro_logr, string @codigo_ibge
      ) {
        return (Row)Add(new object[] {
          @municipio, @uf, @nome, @verbairro, @verlog, @cep, @desvinc_bairro_logr, @codigo_ibge
        });
      }

      public Row Find(
        string @municipio
      ) {
        return (Row)Find(new object[] {
          @municipio
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
      /// MUNICIPIO: VARCHAR(10), Primary Key, Not Null
      /// </summary>
      public string Municipio { get { return (string)this["municipio"]; } set { this["municipio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// UF: CHAR(2), Not Null
      /// </summary>
      public string Uf { get { return (string)this["uf"]; } set { this["uf"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(50), Not Null
      /// </summary>
      public string Nome { get { return (string)this["nome"]; } set { this["nome"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// VERBAIRRO: CHAR(1)
      /// </summary>
      public string Verbairro { get { return (string)this["verbairro"]; } set { this["verbairro"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// VERLOG: CHAR(1)
      /// </summary>
      public string Verlog { get { return (string)this["verlog"]; } set { this["verlog"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CEP: VARCHAR(8)
      /// </summary>
      public string Cep { get { return (string)this["cep"]; } set { this["cep"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESVINC_BAIRRO_LOGR: CHAR(1)
      /// </summary>
      public string Desvinc_bairro_logr { get { return (string)this["desvinc_bairro_logr"]; } set { this["desvinc_bairro_logr"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CODIGO_IBGE: VARCHAR(20)
      /// </summary>
      public string Codigo_ibge { get { return (string)this["codigo_ibge"]; } set { this["codigo_ibge"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @municipio, string @uf, string @nome) {
        return Insert(connection,
                      @municipio, @uf, @nome,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @municipio, string @uf, string @nome,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_municipio(),
          new string[] { "municipio", "uf", "nome" },
          new object[] { @municipio, @uf, @nome },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkMunicipio,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_municipio(),
          new string[] { "municipio" },
          new object[] { pkMunicipio },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkMunicipio) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_municipio(),
          new string[] { "municipio" },
          new object[] { pkMunicipio }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkMunicipio) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_municipio(),
          new string[] { "municipio" },
          new object[] { pkMunicipio }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Municipio { get { return (string)row["municipio", DataRowVersion.Original]; } }
        public string Uf { get { return (string)row["uf", DataRowVersion.Original]; } }
        public string Nome { get { return (string)row["nome", DataRowVersion.Original]; } }
        public string Verbairro { get { return (string)row["verbairro", DataRowVersion.Original]; } }
        public string Verlog { get { return (string)row["verlog", DataRowVersion.Original]; } }
        public string Cep { get { return (string)row["cep", DataRowVersion.Original]; } }
        public string Desvinc_bairro_logr { get { return (string)row["desvinc_bairro_logr", DataRowVersion.Original]; } }
        public string Codigo_ibge { get { return (string)row["codigo_ibge", DataRowVersion.Original]; } }

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
    #region Coluna: MUNICIPIO
    private ColumnMunicipio pvColMunicipio;

    public sealed class ColumnMunicipio : TDataColumn {

      public ColumnMunicipio() {
        this.ColumnName = "municipio";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_municipio.municipio"; } }
      public override string OriginalCol { get { return "municipio"; } }
      public override string RefCol { get { return "HD_MUNICIPIO.MUNICIPIO"; } }
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

    #region Coluna: UF
    private ColumnUf pvColUf;

    public sealed class ColumnUf : TDataColumn {

      public ColumnUf() {
        this.ColumnName = "uf";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_municipio.uf"; } }
      public override string OriginalCol { get { return "uf"; } }
      public override string RefCol { get { return "HD_MUNICIPIO.UF"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 2; } }
      public override string SqlServerType { get { return "CHAR(2)"; } }

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
      public override string FullCol { get { return "hd_municipio.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "HD_MUNICIPIO.NOME"; } }
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

    #region Coluna: VERBAIRRO
    private ColumnVerbairro pvColVerbairro;

    public sealed class ColumnVerbairro : TDataColumn {

      public ColumnVerbairro() {
        this.ColumnName = "verbairro";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_municipio.verbairro"; } }
      public override string OriginalCol { get { return "verbairro"; } }
      public override string RefCol { get { return "HD_MUNICIPIO.VERBAIRRO"; } }
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

    #region Coluna: VERLOG
    private ColumnVerlog pvColVerlog;

    public sealed class ColumnVerlog : TDataColumn {

      public ColumnVerlog() {
        this.ColumnName = "verlog";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_municipio.verlog"; } }
      public override string OriginalCol { get { return "verlog"; } }
      public override string RefCol { get { return "HD_MUNICIPIO.VERLOG"; } }
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
      public override string FullCol { get { return "hd_municipio.cep"; } }
      public override string OriginalCol { get { return "cep"; } }
      public override string RefCol { get { return "HD_MUNICIPIO.CEP"; } }
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

    #region Coluna: DESVINC_BAIRRO_LOGR
    private ColumnDesvinc_bairro_logr pvColDesvinc_bairro_logr;

    public sealed class ColumnDesvinc_bairro_logr : TDataColumn {

      public ColumnDesvinc_bairro_logr() {
        this.ColumnName = "desvinc_bairro_logr";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_municipio.desvinc_bairro_logr"; } }
      public override string OriginalCol { get { return "desvinc_bairro_logr"; } }
      public override string RefCol { get { return "HD_MUNICIPIO.DESVINC_BAIRRO_LOGR"; } }
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

    #region Coluna: CODIGO_IBGE
    private ColumnCodigo_ibge pvColCodigo_ibge;

    public sealed class ColumnCodigo_ibge : TDataColumn {

      public ColumnCodigo_ibge() {
        this.ColumnName = "codigo_ibge";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_municipio.codigo_ibge"; } }
      public override string OriginalCol { get { return "codigo_ibge"; } }
      public override string RefCol { get { return "HD_MUNICIPIO.CODIGO_IBGE"; } }
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
    #endregion
  }

  public partial class Hd_tipo_logradouro : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_tipo_logradouro() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_tipo_logradouro");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_tipo_logradouro");
      }
      catch{}
    }

    public Hd_tipo_logradouro() {
      pvColTipo_logr = new ColumnTipo_logr(); Columns.Add(pvColTipo_logr);
      pvColDescricao = new ColumnDescricao(); Columns.Add(pvColDescricao);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_tipo_logradouro");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @tipo_logr, string @descricao) {
      Row row = NewRow();

      row.Tipo_logr = @tipo_logr;
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
      get { return "HD_TIPO_LOGRADOURO"; }
    }

    protected override sealed string Name { get { return "Tipos de logradouro"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColTipo_logr });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_tipo_logradouro_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_tipo_logradouro_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_tipo_logradouro_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_tipo_logradouro_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkTipo_logr", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "TIPO_LOGR", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("tipo_logr", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TIPO_LOGR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
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
          if(lower == "tipo_logr") { result.Add(toLower ? "tipo_logr" : column); continue; }
          if(lower == "descricao") { result.Add(toLower ? "descricao" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "tipo_logr", "descricao" }
                          : new string[] { "TIPO_LOGR", "DESCRICAO" });
        else
          result.AddRange(toLower
                          ? new string[] { "tipo_logr", "descricao" }
                          : new string[] { "TIPO_LOGR", "DESCRICAO" });

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
        "hd_tipo_logradouro.tipo_logr"
      });
      if(scColumns.Contains("descricao")) list.Add("hd_tipo_logradouro.descricao");
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
          "FROM hd_tipo_logradouro " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_tipo_logradouro");
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
        da.TableMappings.Add("Table", "Hd_tipo_logradouro");
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
            if (dr["TIPO_LOGR"] != null && string.IsNullOrEmpty(dr["TIPO_LOGR"].ToString()))
              dr["TIPO_LOGR"] = DBNull.Value;
            if (dr["DESCRICAO"] != null && string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
              dr["DESCRICAO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["TIPO_LOGR"] == null || string.IsNullOrEmpty(dr["TIPO_LOGR"].ToString()))
            {
              return "Favor informar o Tipo_logr.";
            }

            if (dr["TIPO_LOGR"] != null && !string.IsNullOrEmpty(dr["TIPO_LOGR"].ToString()))
            {
              if (dr["TIPO_LOGR"].ToString().Length > 5)
              {
                return "O Tipo_logr deve possuir no máximo 5 caracteres.";
              }
            }

            if (dr["DESCRICAO"] == null || string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              return "Favor informar o Descricao.";
            }

            if (dr["DESCRICAO"] != null && !string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              if (dr["DESCRICAO"].ToString().Length > 15)
              {
                return "O Descricao deve possuir no máximo 15 caracteres.";
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

    public static Hd_tipo_logradouro Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_tipo_logradouro Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_tipo_logradouro tab = new Hd_tipo_logradouro();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_tipo_logradouro tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @tipo_logr, string @descricao
      ) {
        return (Row)Add(new object[] {
          @tipo_logr, @descricao
        });
      }

      public Row Find(
        string @tipo_logr
      ) {
        return (Row)Find(new object[] {
          @tipo_logr
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
      /// TIPO_LOGR: VARCHAR(5), Primary Key, Not Null
      /// </summary>
      public string Tipo_logr { get { return (string)this["tipo_logr"]; } set { this["tipo_logr"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO: VARCHAR(15), Not Null
      /// </summary>
      public string Descricao { get { return (string)this["descricao"]; } set { this["descricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @tipo_logr, string @descricao) {
        return Insert(connection,
                      @tipo_logr, @descricao,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @tipo_logr, string @descricao,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_tipo_logradouro(),
          new string[] { "tipo_logr", "descricao" },
          new object[] { @tipo_logr, @descricao },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkTipo_logr,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_tipo_logradouro(),
          new string[] { "tipo_logr" },
          new object[] { pkTipo_logr },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkTipo_logr) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_tipo_logradouro(),
          new string[] { "tipo_logr" },
          new object[] { pkTipo_logr }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkTipo_logr) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_tipo_logradouro(),
          new string[] { "tipo_logr" },
          new object[] { pkTipo_logr }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Tipo_logr { get { return (string)row["tipo_logr", DataRowVersion.Original]; } }
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
    #region Coluna: TIPO_LOGR
    private ColumnTipo_logr pvColTipo_logr;

    public sealed class ColumnTipo_logr : TDataColumn {

      public ColumnTipo_logr() {
        this.ColumnName = "tipo_logr";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_tipo_logradouro.tipo_logr"; } }
      public override string OriginalCol { get { return "tipo_logr"; } }
      public override string RefCol { get { return "HD_TIPO_LOGRADOURO.TIPO_LOGR"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 5; } }
      public override string SqlServerType { get { return "VARCHAR(5)"; } }

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
      public override string FullCol { get { return "hd_tipo_logradouro.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "HD_TIPO_LOGRADOURO.DESCRICAO"; } }
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
    #endregion
  }

  public partial class Hd_trecho_logradouro : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_trecho_logradouro() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_trecho_logradouro");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_trecho_logradouro");
      }
      catch{}
    }

    public Hd_trecho_logradouro() {
      pvColLogradouro = new ColumnLogradouro(); Columns.Add(pvColLogradouro);
      pvColMunicipio = new ColumnMunicipio(); Columns.Add(pvColMunicipio);
      pvColTrecho_logr = new ColumnTrecho_logr(); Columns.Add(pvColTrecho_logr);
      pvColNum_inicio = new ColumnNum_inicio(); Columns.Add(pvColNum_inicio);
      pvColNum_fim = new ColumnNum_fim(); Columns.Add(pvColNum_fim);
      pvColLado = new ColumnLado(); Columns.Add(pvColLado);
      pvColCep = new ColumnCep(); Columns.Add(pvColCep);
      pvColBairro = new ColumnBairro(); Columns.Add(pvColBairro);
      pvColVernumvalido = new ColumnVernumvalido(); Columns.Add(pvColVernumvalido);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_trecho_logradouro");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @logradouro, string @municipio, string @trecho_logr, string @vernumvalido) {
      Row row = NewRow();

      row.Logradouro = @logradouro;
      row.Municipio = @municipio;
      row.Trecho_logr = @trecho_logr;
      row.Vernumvalido = @vernumvalido;

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
      get { return "HD_TRECHO_LOGRADOURO"; }
    }

    protected override sealed string Name { get { return "Trechos de logradouros"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColLogradouro, pvColMunicipio, pvColTrecho_logr });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_trecho_logradouro_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_trecho_logradouro_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_trecho_logradouro_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_trecho_logradouro_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkLogradouro", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "LOGRADOURO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkMunicipio", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "MUNICIPIO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkTrecho_logr", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "TRECHO_LOGR", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("logradouro", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "LOGRADOURO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("municipio", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "MUNICIPIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("trecho_logr", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TRECHO_LOGR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("num_inicio", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "NUM_INICIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("num_fim", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "NUM_FIM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("lado", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "LADO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("cep", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CEP", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("bairro", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "BAIRRO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("vernumvalido", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "VERNUMVALIDO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));

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
          if(lower == "logradouro") { result.Add(toLower ? "logradouro" : column); continue; }
          if(lower == "municipio") { result.Add(toLower ? "municipio" : column); continue; }
          if(lower == "trecho_logr") { result.Add(toLower ? "trecho_logr" : column); continue; }
          if(lower == "num_inicio") { result.Add(toLower ? "num_inicio" : column); continue; }
          if(lower == "num_fim") { result.Add(toLower ? "num_fim" : column); continue; }
          if(lower == "lado") { result.Add(toLower ? "lado" : column); continue; }
          if(lower == "cep") { result.Add(toLower ? "cep" : column); continue; }
          if(lower == "bairro") { result.Add(toLower ? "bairro" : column); continue; }
          if(lower == "vernumvalido") { result.Add(toLower ? "vernumvalido" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "logradouro", "municipio", "trecho_logr", "num_inicio", "num_fim", 
                                           "lado", "cep", "bairro", "vernumvalido" }
                          : new string[] { "LOGRADOURO", "MUNICIPIO", "TRECHO_LOGR", "NUM_INICIO", "NUM_FIM", 
                                           "LADO", "CEP", "BAIRRO", "VERNUMVALIDO" });
        else
          result.AddRange(toLower
                          ? new string[] { "logradouro", "municipio", "trecho_logr", "num_inicio", "num_fim", 
                                           "lado", "cep", "bairro", "vernumvalido" }
                          : new string[] { "LOGRADOURO", "MUNICIPIO", "TRECHO_LOGR", "NUM_INICIO", "NUM_FIM", 
                                           "LADO", "CEP", "BAIRRO", "VERNUMVALIDO" });

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
        "hd_trecho_logradouro.logradouro", "hd_trecho_logradouro.municipio", "hd_trecho_logradouro.trecho_logr"
      });
      if(scColumns.Contains("num_inicio")) list.Add("hd_trecho_logradouro.num_inicio");
      if(scColumns.Contains("num_fim")) list.Add("hd_trecho_logradouro.num_fim");
      if(scColumns.Contains("lado")) list.Add("hd_trecho_logradouro.lado");
      if(scColumns.Contains("cep")) list.Add("hd_trecho_logradouro.cep");
      if(scColumns.Contains("bairro")) list.Add("hd_trecho_logradouro.bairro");
      if(scColumns.Contains("vernumvalido")) list.Add("hd_trecho_logradouro.vernumvalido");
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
          "FROM hd_trecho_logradouro " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_trecho_logradouro");
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
        da.TableMappings.Add("Table", "Hd_trecho_logradouro");
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
            if (dr["LOGRADOURO"] != null && string.IsNullOrEmpty(dr["LOGRADOURO"].ToString()))
              dr["LOGRADOURO"] = DBNull.Value;
            if (dr["MUNICIPIO"] != null && string.IsNullOrEmpty(dr["MUNICIPIO"].ToString()))
              dr["MUNICIPIO"] = DBNull.Value;
            if (dr["TRECHO_LOGR"] != null && string.IsNullOrEmpty(dr["TRECHO_LOGR"].ToString()))
              dr["TRECHO_LOGR"] = DBNull.Value;
            if (dr["NUM_INICIO"] != null && string.IsNullOrEmpty(dr["NUM_INICIO"].ToString()))
              dr["NUM_INICIO"] = DBNull.Value;
            if (dr["NUM_FIM"] != null && string.IsNullOrEmpty(dr["NUM_FIM"].ToString()))
              dr["NUM_FIM"] = DBNull.Value;
            if (dr["LADO"] != null && string.IsNullOrEmpty(dr["LADO"].ToString()))
              dr["LADO"] = DBNull.Value;
            if (dr["CEP"] != null && string.IsNullOrEmpty(dr["CEP"].ToString()))
              dr["CEP"] = DBNull.Value;
            if (dr["BAIRRO"] != null && string.IsNullOrEmpty(dr["BAIRRO"].ToString()))
              dr["BAIRRO"] = DBNull.Value;
            if (dr["VERNUMVALIDO"] != null && string.IsNullOrEmpty(dr["VERNUMVALIDO"].ToString()))
              dr["VERNUMVALIDO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["LOGRADOURO"] == null || string.IsNullOrEmpty(dr["LOGRADOURO"].ToString()))
            {
              return "Favor informar o Logradouro.";
            }

            if (dr["LOGRADOURO"] != null && !string.IsNullOrEmpty(dr["LOGRADOURO"].ToString()))
            {
              if (dr["LOGRADOURO"].ToString().Length > 10)
              {
                return "O Logradouro deve possuir no máximo 10 caracteres.";
              }
            }

            if (dr["MUNICIPIO"] == null || string.IsNullOrEmpty(dr["MUNICIPIO"].ToString()))
            {
              return "Favor informar o Municipio.";
            }

            if (dr["MUNICIPIO"] != null && !string.IsNullOrEmpty(dr["MUNICIPIO"].ToString()))
            {
              if (dr["MUNICIPIO"].ToString().Length > 10)
              {
                return "O Municipio deve possuir no máximo 10 caracteres.";
              }
            }

            if (dr["TRECHO_LOGR"] == null || string.IsNullOrEmpty(dr["TRECHO_LOGR"].ToString()))
            {
              return "Favor informar o Trecho_logr.";
            }

            if (dr["TRECHO_LOGR"] != null && !string.IsNullOrEmpty(dr["TRECHO_LOGR"].ToString()))
            {
              if (dr["TRECHO_LOGR"].ToString().Length > 10)
              {
                return "O Trecho_logr deve possuir no máximo 10 caracteres.";
              }
            }

            if (dr["NUM_INICIO"] != null && !string.IsNullOrEmpty(dr["NUM_INICIO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["NUM_INICIO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["NUM_INICIO"]).ToString("0.############").Split(',')[0].Length > 6) || (Convert.ToDecimal(dr["NUM_INICIO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["NUM_INICIO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Num_inicio deve ser um valor inteiro com 6 dígitos no máximo.";
              }

              if (dr["NUM_INICIO"] != null && !string.IsNullOrEmpty(dr["NUM_INICIO"].ToString()))
              {
                if (Convert.ToDecimal(dr["NUM_INICIO"]) < (0))
                {
                  return "O Num_inicio deve ser maior ou igual a (0).";
                }
              }
            }

            if (dr["NUM_FIM"] != null && !string.IsNullOrEmpty(dr["NUM_FIM"].ToString()))
            {
              if ((Convert.ToDecimal(dr["NUM_FIM"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["NUM_FIM"]).ToString("0.############").Split(',')[0].Length > 6) || (Convert.ToDecimal(dr["NUM_FIM"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["NUM_FIM"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Num_fim deve ser um valor inteiro com 6 dígitos no máximo.";
              }

              if (dr["NUM_FIM"] != null && !string.IsNullOrEmpty(dr["NUM_FIM"].ToString()))
              {
                if (Convert.ToDecimal(dr["NUM_FIM"]) < (0))
                {
                  return "O Num_fim deve ser maior ou igual a (0).";
                }
              }
            }

            if (dr["LADO"] != null && !string.IsNullOrEmpty(dr["LADO"].ToString()))
            {
              if (dr["LADO"].ToString().Length > 1)
              {
                return "O Lado deve possuir no máximo 1 caractere.";
              }

              if (dr["LADO"] != null && !string.IsNullOrEmpty(dr["LADO"].ToString()))
              {
                List<object> lstLADO = new List<object>(new object[]{"P", "I"});
                if(!lstLADO.Contains(dr["LADO"].ToString()))
                {
                  return "Os valores permitidos para o Lado são ''P'' ou ''I''.";
                }
              }
            }

            if (dr["CEP"] != null && !string.IsNullOrEmpty(dr["CEP"].ToString()))
            {
              if (dr["CEP"].ToString().Length > 8)
              {
                return "O Cep deve possuir no máximo 8 caracteres.";
              }
            }

            if (dr["BAIRRO"] != null && !string.IsNullOrEmpty(dr["BAIRRO"].ToString()))
            {
              if (dr["BAIRRO"].ToString().Length > 10)
              {
                return "O Bairro deve possuir no máximo 10 caracteres.";
              }
            }

            if (dr["VERNUMVALIDO"] == null || string.IsNullOrEmpty(dr["VERNUMVALIDO"].ToString()))
            {
              return "Favor informar o Vernumvalido.";
            }

            if (dr["VERNUMVALIDO"] != null && !string.IsNullOrEmpty(dr["VERNUMVALIDO"].ToString()))
            {
              if (dr["VERNUMVALIDO"].ToString().Length > 1)
              {
                return "O Vernumvalido deve possuir no máximo 1 caractere.";
              }

              if (dr["VERNUMVALIDO"] != null && !string.IsNullOrEmpty(dr["VERNUMVALIDO"].ToString()))
              {
                List<object> lstVERNUMVALIDO = new List<object>(new object[]{"N", "S"});
                if(!lstVERNUMVALIDO.Contains(dr["VERNUMVALIDO"].ToString()))
                {
                  return "Os valores permitidos para o Vernumvalido são ''N'' ou ''S''.";
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

    public static Hd_trecho_logradouro Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_trecho_logradouro Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_trecho_logradouro tab = new Hd_trecho_logradouro();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_trecho_logradouro tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @logradouro, string @municipio, string @trecho_logr, decimal? @num_inicio, 
        decimal? @num_fim, string @lado, string @cep, string @bairro, string @vernumvalido
      ) {
        return (Row)Add(new object[] {
          @logradouro, @municipio, @trecho_logr, @num_inicio, @num_fim, @lado, @cep, @bairro, 
          @vernumvalido
        });
      }

      public Row Find(
        string @logradouro, string @municipio, string @trecho_logr
      ) {
        return (Row)Find(new object[] {
          @logradouro, @municipio, @trecho_logr
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
      /// LOGRADOURO: VARCHAR(10), Primary Key, Not Null
      /// </summary>
      public string Logradouro { get { return (string)this["logradouro"]; } set { this["logradouro"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// MUNICIPIO: VARCHAR(10), Primary Key, Not Null
      /// </summary>
      public string Municipio { get { return (string)this["municipio"]; } set { this["municipio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TRECHO_LOGR: VARCHAR(10), Primary Key, Not Null
      /// </summary>
      public string Trecho_logr { get { return (string)this["trecho_logr"]; } set { this["trecho_logr"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NUM_INICIO: NUMERIC(6)
      /// </summary>
      public decimal? Num_inicio { get { return (decimal?)this["num_inicio"]; } set { this["num_inicio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NUM_FIM: NUMERIC(6)
      /// </summary>
      public decimal? Num_fim { get { return (decimal?)this["num_fim"]; } set { this["num_fim"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// LADO: CHAR(1)
      /// </summary>
      public string Lado { get { return (string)this["lado"]; } set { this["lado"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CEP: VARCHAR(8)
      /// </summary>
      public string Cep { get { return (string)this["cep"]; } set { this["cep"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// BAIRRO: VARCHAR(10)
      /// </summary>
      public string Bairro { get { return (string)this["bairro"]; } set { this["bairro"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// VERNUMVALIDO: CHAR(1), Not Null
      /// </summary>
      public string Vernumvalido { get { return (string)this["vernumvalido"]; } set { this["vernumvalido"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @logradouro, string @municipio, string @trecho_logr, string @vernumvalido) {
        return Insert(connection,
                      @logradouro, @municipio, @trecho_logr, @vernumvalido,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @logradouro, string @municipio, string @trecho_logr, string @vernumvalido,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_trecho_logradouro(),
          new string[] { "logradouro", "municipio", "trecho_logr", "vernumvalido" },
          new object[] { @logradouro, @municipio, @trecho_logr, @vernumvalido },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkLogradouro, string pkMunicipio, string pkTrecho_logr,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_trecho_logradouro(),
          new string[] { "logradouro", "municipio", "trecho_logr" },
          new object[] { pkLogradouro, pkMunicipio, pkTrecho_logr },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkLogradouro, string pkMunicipio, string pkTrecho_logr) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_trecho_logradouro(),
          new string[] { "logradouro", "municipio", "trecho_logr" },
          new object[] { pkLogradouro, pkMunicipio, pkTrecho_logr }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkLogradouro, string pkMunicipio, string pkTrecho_logr) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_trecho_logradouro(),
          new string[] { "logradouro", "municipio", "trecho_logr" },
          new object[] { pkLogradouro, pkMunicipio, pkTrecho_logr }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Logradouro { get { return (string)row["logradouro", DataRowVersion.Original]; } }
        public string Municipio { get { return (string)row["municipio", DataRowVersion.Original]; } }
        public string Trecho_logr { get { return (string)row["trecho_logr", DataRowVersion.Original]; } }
        public decimal? Num_inicio { get { return (decimal?)row["num_inicio", DataRowVersion.Original]; } }
        public decimal? Num_fim { get { return (decimal?)row["num_fim", DataRowVersion.Original]; } }
        public string Lado { get { return (string)row["lado", DataRowVersion.Original]; } }
        public string Cep { get { return (string)row["cep", DataRowVersion.Original]; } }
        public string Bairro { get { return (string)row["bairro", DataRowVersion.Original]; } }
        public string Vernumvalido { get { return (string)row["vernumvalido", DataRowVersion.Original]; } }

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
    #region Coluna: LOGRADOURO
    private ColumnLogradouro pvColLogradouro;

    public sealed class ColumnLogradouro : TDataColumn {

      public ColumnLogradouro() {
        this.ColumnName = "logradouro";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_trecho_logradouro.logradouro"; } }
      public override string OriginalCol { get { return "logradouro"; } }
      public override string RefCol { get { return "HD_TRECHO_LOGRADOURO.LOGRADOURO"; } }
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

    #region Coluna: MUNICIPIO
    private ColumnMunicipio pvColMunicipio;

    public sealed class ColumnMunicipio : TDataColumn {

      public ColumnMunicipio() {
        this.ColumnName = "municipio";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_trecho_logradouro.municipio"; } }
      public override string OriginalCol { get { return "municipio"; } }
      public override string RefCol { get { return "HD_TRECHO_LOGRADOURO.MUNICIPIO"; } }
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

    #region Coluna: TRECHO_LOGR
    private ColumnTrecho_logr pvColTrecho_logr;

    public sealed class ColumnTrecho_logr : TDataColumn {

      public ColumnTrecho_logr() {
        this.ColumnName = "trecho_logr";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_trecho_logradouro.trecho_logr"; } }
      public override string OriginalCol { get { return "trecho_logr"; } }
      public override string RefCol { get { return "HD_TRECHO_LOGRADOURO.TRECHO_LOGR"; } }
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

    #region Coluna: NUM_INICIO
    private ColumnNum_inicio pvColNum_inicio;

    public sealed class ColumnNum_inicio : TDataColumn {

      public ColumnNum_inicio() {
        this.ColumnName = "num_inicio";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_trecho_logradouro.num_inicio"; } }
      public override string OriginalCol { get { return "num_inicio"; } }
      public override string RefCol { get { return "HD_TRECHO_LOGRADOURO.NUM_INICIO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 7; } }
      public override int Precision { get { return 6; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(6)"; } }

      public override object MinValue { get { return 0; } }

    }
    #endregion

    #region Coluna: NUM_FIM
    private ColumnNum_fim pvColNum_fim;

    public sealed class ColumnNum_fim : TDataColumn {

      public ColumnNum_fim() {
        this.ColumnName = "num_fim";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_trecho_logradouro.num_fim"; } }
      public override string OriginalCol { get { return "num_fim"; } }
      public override string RefCol { get { return "HD_TRECHO_LOGRADOURO.NUM_FIM"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 7; } }
      public override int Precision { get { return 6; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(6)"; } }

      public override object MinValue { get { return 0; } }

    }
    #endregion

    #region Coluna: LADO
    private ColumnLado pvColLado;

    public sealed class ColumnLado : TDataColumn {

      public ColumnLado() {
        this.ColumnName = "lado";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_trecho_logradouro.lado"; } }
      public override string OriginalCol { get { return "lado"; } }
      public override string RefCol { get { return "HD_TRECHO_LOGRADOURO.LADO"; } }
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
      public override string FullCol { get { return "hd_trecho_logradouro.cep"; } }
      public override string OriginalCol { get { return "cep"; } }
      public override string RefCol { get { return "HD_TRECHO_LOGRADOURO.CEP"; } }
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

    #region Coluna: BAIRRO
    private ColumnBairro pvColBairro;

    public sealed class ColumnBairro : TDataColumn {

      public ColumnBairro() {
        this.ColumnName = "bairro";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_trecho_logradouro.bairro"; } }
      public override string OriginalCol { get { return "bairro"; } }
      public override string RefCol { get { return "HD_TRECHO_LOGRADOURO.BAIRRO"; } }
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

    #region Coluna: VERNUMVALIDO
    private ColumnVernumvalido pvColVernumvalido;

    public sealed class ColumnVernumvalido : TDataColumn {

      public ColumnVernumvalido() {
        this.ColumnName = "vernumvalido";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_trecho_logradouro.vernumvalido"; } }
      public override string OriginalCol { get { return "vernumvalido"; } }
      public override string RefCol { get { return "HD_TRECHO_LOGRADOURO.VERNUMVALIDO"; } }
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

  public partial class Hd_uf : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_uf() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_uf");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_uf");
      }
      catch{}
    }

    public Hd_uf() {
      pvColUf = new ColumnUf(); Columns.Add(pvColUf);
      pvColNome = new ColumnNome(); Columns.Add(pvColNome);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_uf");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @uf, string @nome) {
      Row row = NewRow();

      row.Uf = @uf;
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
      get { return "HD_UF"; }
    }

    protected override sealed string Name { get { return "Estados do Brasil"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColUf });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_uf_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_uf_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_uf_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_uf_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkUf", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "UF", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("uf", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "UF", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
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
          if(lower == "uf") { result.Add(toLower ? "uf" : column); continue; }
          if(lower == "nome") { result.Add(toLower ? "nome" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "uf", "nome" }
                          : new string[] { "UF", "NOME" });
        else
          result.AddRange(toLower
                          ? new string[] { "uf", "nome" }
                          : new string[] { "UF", "NOME" });

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
        "hd_uf.uf"
      });
      if(scColumns.Contains("nome")) list.Add("hd_uf.nome");
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
          "FROM hd_uf " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_uf");
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
        da.TableMappings.Add("Table", "Hd_uf");
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
            if (dr["UF"] != null && string.IsNullOrEmpty(dr["UF"].ToString()))
              dr["UF"] = DBNull.Value;
            if (dr["NOME"] != null && string.IsNullOrEmpty(dr["NOME"].ToString()))
              dr["NOME"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["UF"] == null || string.IsNullOrEmpty(dr["UF"].ToString()))
            {
              return "Favor informar o Uf.";
            }

            if (dr["UF"] != null && !string.IsNullOrEmpty(dr["UF"].ToString()))
            {
              if (dr["UF"].ToString().Length > 2)
              {
                return "O Uf deve possuir no máximo 2 caracteres.";
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

    public static Hd_uf Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_uf Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_uf tab = new Hd_uf();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_uf tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @uf, string @nome
      ) {
        return (Row)Add(new object[] {
          @uf, @nome
        });
      }

      public Row Find(
        string @uf
      ) {
        return (Row)Find(new object[] {
          @uf
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
      /// UF: CHAR(2), Primary Key, Not Null
      /// </summary>
      public string Uf { get { return (string)this["uf"]; } set { this["uf"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(50), Not Null
      /// </summary>
      public string Nome { get { return (string)this["nome"]; } set { this["nome"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @uf, string @nome) {
        return Insert(connection,
                      @uf, @nome,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @uf, string @nome,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_uf(),
          new string[] { "uf", "nome" },
          new object[] { @uf, @nome },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkUf,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_uf(),
          new string[] { "uf" },
          new object[] { pkUf },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkUf) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_uf(),
          new string[] { "uf" },
          new object[] { pkUf }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkUf) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_uf(),
          new string[] { "uf" },
          new object[] { pkUf }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Uf { get { return (string)row["uf", DataRowVersion.Original]; } }
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
    #region Coluna: UF
    private ColumnUf pvColUf;

    public sealed class ColumnUf : TDataColumn {

      public ColumnUf() {
        this.ColumnName = "uf";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_uf.uf"; } }
      public override string OriginalCol { get { return "uf"; } }
      public override string RefCol { get { return "HD_UF.UF"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 2; } }
      public override string SqlServerType { get { return "CHAR(2)"; } }

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
      public override string FullCol { get { return "hd_uf.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "HD_UF.NOME"; } }
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

