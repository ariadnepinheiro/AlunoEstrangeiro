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
  public class Bancos : TDataSet {
    private const string DataSetName_Def = "Bancos";
    private static ConstructorInfo ctorTCustom = null;
    private CustomBase tcustom = null;
    #region Variáveis private para as tabelas deste dataset
    private Hd_agencia pvTabHd_agencia;
    private Hd_banco pvTabHd_banco;
    #endregion

    static Bancos() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Bancos");
      }
      catch{}
    }

    public Bancos() : base() {
      DataSetName = DataSetName_Def;

      pvTabHd_agencia = new Hd_agencia();
      pvTabHd_banco = new Hd_banco();

      Tables.Add(pvTabHd_agencia);
      Tables.Add(pvTabHd_banco);

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
          pvTabHd_agencia.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_agencia); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabHd_banco.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_banco); if(row != null) return DataLib.RowErrorsToString(row);
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
    public Hd_agencia Hd_agencia { get { return pvTabHd_agencia; } }

    [
      Browsable(false),
    ]
    public Hd_banco Hd_banco { get { return pvTabHd_banco; } }

    #endregion

    public abstract class CustomBase {
      public abstract string Put(Techne.HadesLyc.CR.Bancos ds, TConnectionWritable connection);
    }

  }


  public partial class Hd_agencia : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_agencia() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_agencia");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_agencia");
      }
      catch{}
    }

    public Hd_agencia() {
      pvColAgencia = new ColumnAgencia(); Columns.Add(pvColAgencia);
      pvColBanco = new ColumnBanco(); Columns.Add(pvColBanco);
      pvColNome = new ColumnNome(); Columns.Add(pvColNome);
      pvColMunicipio = new ColumnMunicipio(); Columns.Add(pvColMunicipio);
      pvColBairro = new ColumnBairro(); Columns.Add(pvColBairro);
      pvColUf = new ColumnUf(); Columns.Add(pvColUf);
      pvColEndereco = new ColumnEndereco(); Columns.Add(pvColEndereco);
      pvColContato = new ColumnContato(); Columns.Add(pvColContato);
      pvColCargo = new ColumnCargo(); Columns.Add(pvColCargo);
      pvColTelefone = new ColumnTelefone(); Columns.Add(pvColTelefone);
      pvColCep = new ColumnCep(); Columns.Add(pvColCep);
      pvColNome_municipio = new ColumnNome_municipio(); Columns.Add(pvColNome_municipio);
      pvColNome_bairro = new ColumnNome_bairro(); Columns.Add(pvColNome_bairro);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_agencia");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @agencia, decimal? @banco, string @nome) {
      Row row = NewRow();

      row.Agencia = @agencia;
      row.Banco = @banco;
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
      get { return "HD_AGENCIA"; }
    }

    protected override sealed string Name { get { return "Agência"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColAgencia, pvColBanco });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_agencia_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_agencia_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_agencia_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_agencia_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkAgencia", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "AGENCIA", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkBanco", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "BANCO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("agencia", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "AGENCIA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("banco", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "BANCO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("nome", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NOME", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("municipio", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "MUNICIPIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("bairro", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "BAIRRO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("uf", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "UF", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("endereco", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "ENDERECO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("contato", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CONTATO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("cargo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CARGO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("telefone", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TELEFONE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("cep", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CEP", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "agencia") { result.Add(toLower ? "agencia" : column); continue; }
          if(lower == "banco") { result.Add(toLower ? "banco" : column); continue; }
          if(lower == "nome") { result.Add(toLower ? "nome" : column); continue; }
          if(lower == "municipio") { result.Add(toLower ? "municipio" : column); continue; }
          if(lower == "bairro") { result.Add(toLower ? "bairro" : column); continue; }
          if(lower == "uf") { result.Add(toLower ? "uf" : column); continue; }
          if(lower == "endereco") { result.Add(toLower ? "endereco" : column); continue; }
          if(lower == "contato") { result.Add(toLower ? "contato" : column); continue; }
          if(lower == "cargo") { result.Add(toLower ? "cargo" : column); continue; }
          if(lower == "telefone") { result.Add(toLower ? "telefone" : column); continue; }
          if(lower == "cep") { result.Add(toLower ? "cep" : column); continue; }
          if(!tabPrincipal) {
            if(lower == "nome_municipio") { result.Add(toLower ? "nome_municipio" : column); continue; }
            if(lower == "nome_bairro") { result.Add(toLower ? "nome_bairro" : column); continue; }
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "agencia", "banco", "nome", "municipio", "bairro", "uf", "endereco", 
                                           "contato", "cargo", "telefone", "cep" }
                          : new string[] { "AGENCIA", "BANCO", "NOME", "MUNICIPIO", "BAIRRO", "UF", "ENDERECO", 
                                           "CONTATO", "CARGO", "TELEFONE", "CEP" });
        else
          result.AddRange(toLower
                          ? new string[] { "agencia", "banco", "nome", "municipio", "bairro", "uf", "endereco", 
                                           "contato", "cargo", "telefone", "cep", "nome_municipio", "nome_bairro" }
                          : new string[] { "AGENCIA", "BANCO", "NOME", "MUNICIPIO", "BAIRRO", "UF", "ENDERECO", 
                                           "CONTATO", "CARGO", "TELEFONE", "CEP", "NOME_MUNICIPIO", "NOME_BAIRRO" });

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
        "hd_agencia.agencia", "hd_agencia.banco"
      });
      if(scColumns.Contains("nome")) list.Add("hd_agencia.nome");
      if(scColumns.Contains("municipio")) list.Add("hd_agencia.municipio");
      if(scColumns.Contains("bairro")) list.Add("hd_agencia.bairro");
      if(scColumns.Contains("uf")) list.Add("hd_agencia.uf");
      if(scColumns.Contains("endereco")) list.Add("hd_agencia.endereco");
      if(scColumns.Contains("contato")) list.Add("hd_agencia.contato");
      if(scColumns.Contains("cargo")) list.Add("hd_agencia.cargo");
      if(scColumns.Contains("telefone")) list.Add("hd_agencia.telefone");
      if(scColumns.Contains("cep")) list.Add("hd_agencia.cep");
      if(scColumns.Contains("nome_municipio")) list.Add("hd_municipio.nome nome_municipio");
      if(scColumns.Contains("nome_bairro")) list.Add("hd_bairro.nome nome_bairro");
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
          "FROM hd_agencia " +
               "LEFT JOIN hd_bairro " +
                 "ON hd_bairro.bairro = hd_agencia.bairro AND " +
                    "hd_bairro.municipio = hd_agencia.municipio " +
               "LEFT JOIN hd_municipio " +
                 "ON hd_municipio.municipio = hd_agencia.municipio " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_agencia");
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
        da.TableMappings.Add("Table", "Hd_agencia");
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
            if (dr["AGENCIA"] != null && string.IsNullOrEmpty(dr["AGENCIA"].ToString()))
              dr["AGENCIA"] = DBNull.Value;
            if (dr["BANCO"] != null && string.IsNullOrEmpty(dr["BANCO"].ToString()))
              dr["BANCO"] = DBNull.Value;
            if (dr["NOME"] != null && string.IsNullOrEmpty(dr["NOME"].ToString()))
              dr["NOME"] = DBNull.Value;
            if (dr["MUNICIPIO"] != null && string.IsNullOrEmpty(dr["MUNICIPIO"].ToString()))
              dr["MUNICIPIO"] = DBNull.Value;
            if (dr["BAIRRO"] != null && string.IsNullOrEmpty(dr["BAIRRO"].ToString()))
              dr["BAIRRO"] = DBNull.Value;
            if (dr["UF"] != null && string.IsNullOrEmpty(dr["UF"].ToString()))
              dr["UF"] = DBNull.Value;
            if (dr["ENDERECO"] != null && string.IsNullOrEmpty(dr["ENDERECO"].ToString()))
              dr["ENDERECO"] = DBNull.Value;
            if (dr["CONTATO"] != null && string.IsNullOrEmpty(dr["CONTATO"].ToString()))
              dr["CONTATO"] = DBNull.Value;
            if (dr["CARGO"] != null && string.IsNullOrEmpty(dr["CARGO"].ToString()))
              dr["CARGO"] = DBNull.Value;
            if (dr["TELEFONE"] != null && string.IsNullOrEmpty(dr["TELEFONE"].ToString()))
              dr["TELEFONE"] = DBNull.Value;
            if (dr["CEP"] != null && string.IsNullOrEmpty(dr["CEP"].ToString()))
              dr["CEP"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["AGENCIA"] == null || string.IsNullOrEmpty(dr["AGENCIA"].ToString()))
            {
              return "Favor informar o número da agência.";
            }

            if (dr["AGENCIA"] != null && !string.IsNullOrEmpty(dr["AGENCIA"].ToString()))
            {
              if (dr["AGENCIA"].ToString().Length > 7)
              {
                return "O número da agência deve possuir no máximo 7 caracteres.";
              }
            }

            if (dr["BANCO"] == null || string.IsNullOrEmpty(dr["BANCO"].ToString()))
            {
              return "Favor informar o número do banco.";
            }

            if (dr["BANCO"] != null && !string.IsNullOrEmpty(dr["BANCO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["BANCO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["BANCO"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["BANCO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["BANCO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O número do banco deve ser um valor inteiro com 3 dígitos no máximo.";
              }

              if (dr["BANCO"] != null && !string.IsNullOrEmpty(dr["BANCO"].ToString()))
              {
                if (Convert.ToDecimal(dr["BANCO"]) < (1))
                {
                  return "O número do banco deve ser maior ou igual a (1).";
                }
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

            if (dr["MUNICIPIO"] != null && !string.IsNullOrEmpty(dr["MUNICIPIO"].ToString()))
            {
              if (dr["MUNICIPIO"].ToString().Length > 10)
              {
                return "O Município deve possuir no máximo 10 caracteres.";
              }
            }

            if (dr["BAIRRO"] != null && !string.IsNullOrEmpty(dr["BAIRRO"].ToString()))
            {
              if (dr["BAIRRO"].ToString().Length > 10)
              {
                return "O bairro deve possuir no máximo 10 caracteres.";
              }
            }

            if (dr["UF"] != null && !string.IsNullOrEmpty(dr["UF"].ToString()))
            {
              if (dr["UF"].ToString().Length > 2)
              {
                return "O estado deve possuir no máximo 2 caracteres.";
              }
            }

            if (dr["ENDERECO"] != null && !string.IsNullOrEmpty(dr["ENDERECO"].ToString()))
            {
              if (dr["ENDERECO"].ToString().Length > 60)
              {
                return "O Endereço deve possuir no máximo 60 caracteres.";
              }
            }

            if (dr["CONTATO"] != null && !string.IsNullOrEmpty(dr["CONTATO"].ToString()))
            {
              if (dr["CONTATO"].ToString().Length > 60)
              {
                return "O Contato deve possuir no máximo 60 caracteres.";
              }
            }

            if (dr["CARGO"] != null && !string.IsNullOrEmpty(dr["CARGO"].ToString()))
            {
              if (dr["CARGO"].ToString().Length > 12)
              {
                return "O cargo deve possuir no máximo 12 caracteres.";
              }

              if (dr["CARGO"] != null && !string.IsNullOrEmpty(dr["CARGO"].ToString()))
              {
                List<object> lstCARGO = new List<object>(new object[]{"Outros", "Caixa", "Subgerente", "Gerente"});
                if(!lstCARGO.Contains(dr["CARGO"].ToString()))
                {
                  return "Os valores permitidos para o cargo são ''Outros'', ''Caixa'', ''Subgerente'' ou ''Gerente''.";
                }
              }
            }

            if (dr["TELEFONE"] != null && !string.IsNullOrEmpty(dr["TELEFONE"].ToString()))
            {
              if (dr["TELEFONE"].ToString().Length > 50)
              {
                return "O telefone deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["CEP"] != null && !string.IsNullOrEmpty(dr["CEP"].ToString()))
            {
              if (dr["CEP"].ToString().Length > 8)
              {
                return "O cep deve possuir no máximo 8 caracteres.";
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

    public static Hd_agencia Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_agencia Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_agencia tab = new Hd_agencia();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_agencia tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @agencia, decimal? @banco, string @nome, string @municipio, string @bairro, 
        string @uf, string @endereco, string @contato, string @cargo, string @telefone, string @cep
      ) {
        return (Row)Add(new object[] {
          @agencia, @banco, @nome, @municipio, @bairro, @uf, @endereco, @contato, @cargo, @telefone, 
          @cep
        });
      }

      public Row Find(
        string @agencia, decimal? @banco
      ) {
        return (Row)Find(new object[] {
          @agencia, @banco
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
      /// AGENCIA: VARCHAR(7), Primary Key, Not Null
      /// </summary>
      public string Agencia { get { return (string)this["agencia"]; } set { this["agencia"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// BANCO: NUMERIC(3), Primary Key, Not Null
      /// </summary>
      public decimal? Banco { get { return (decimal?)this["banco"]; } set { this["banco"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(50), Not Null
      /// </summary>
      public string Nome { get { return (string)this["nome"]; } set { this["nome"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// MUNICIPIO: VARCHAR(10)
      /// </summary>
      public string Municipio { get { return (string)this["municipio"]; } set { this["municipio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// BAIRRO: VARCHAR(10)
      /// </summary>
      public string Bairro { get { return (string)this["bairro"]; } set { this["bairro"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// UF: CHAR(2)
      /// </summary>
      public string Uf { get { return (string)this["uf"]; } set { this["uf"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ENDERECO: VARCHAR(60)
      /// </summary>
      public string Endereco { get { return (string)this["endereco"]; } set { this["endereco"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CONTATO: VARCHAR(60)
      /// </summary>
      public string Contato { get { return (string)this["contato"]; } set { this["contato"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CARGO: VARCHAR(12)
      /// </summary>
      public string Cargo { get { return (string)this["cargo"]; } set { this["cargo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TELEFONE: VARCHAR(50)
      /// </summary>
      public string Telefone { get { return (string)this["telefone"]; } set { this["telefone"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CEP: VARCHAR(8)
      /// </summary>
      public string Cep { get { return (string)this["cep"]; } set { this["cep"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(50), Not Null
      /// </summary>
      public string Nome_municipio { get { return (string)this["nome_municipio"]; } set { this["nome_municipio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(50), Not Null
      /// </summary>
      public string Nome_bairro { get { return (string)this["nome_bairro"]; } set { this["nome_bairro"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @agencia, decimal? @banco, string @nome) {
        return Insert(connection,
                      @agencia, @banco, @nome,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @agencia, decimal? @banco, string @nome,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_agencia(),
          new string[] { "agencia", "banco", "nome" },
          new object[] { @agencia, @banco, @nome },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkAgencia, decimal? pkBanco,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_agencia(),
          new string[] { "agencia", "banco" },
          new object[] { pkAgencia, pkBanco },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkAgencia, decimal? pkBanco) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_agencia(),
          new string[] { "agencia", "banco" },
          new object[] { pkAgencia, pkBanco }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkAgencia, decimal? pkBanco) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_agencia(),
          new string[] { "agencia", "banco" },
          new object[] { pkAgencia, pkBanco }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Agencia { get { return (string)row["agencia", DataRowVersion.Original]; } }
        public decimal? Banco { get { return (decimal?)row["banco", DataRowVersion.Original]; } }
        public string Nome { get { return (string)row["nome", DataRowVersion.Original]; } }
        public string Municipio { get { return (string)row["municipio", DataRowVersion.Original]; } }
        public string Bairro { get { return (string)row["bairro", DataRowVersion.Original]; } }
        public string Uf { get { return (string)row["uf", DataRowVersion.Original]; } }
        public string Endereco { get { return (string)row["endereco", DataRowVersion.Original]; } }
        public string Contato { get { return (string)row["contato", DataRowVersion.Original]; } }
        public string Cargo { get { return (string)row["cargo", DataRowVersion.Original]; } }
        public string Telefone { get { return (string)row["telefone", DataRowVersion.Original]; } }
        public string Cep { get { return (string)row["cep", DataRowVersion.Original]; } }
        public string Nome_municipio { get { return (string)row["nome_municipio", DataRowVersion.Original]; } }
        public string Nome_bairro { get { return (string)row["nome_bairro", DataRowVersion.Original]; } }

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
    #region Coluna: AGENCIA
    private ColumnAgencia pvColAgencia;

    public sealed class ColumnAgencia : TDataColumn {

      public ColumnAgencia() {
        this.ColumnName = "agencia";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "número da agência";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_agencia.agencia"; } }
      public override string OriginalCol { get { return "agencia"; } }
      public override string RefCol { get { return "HD_AGENCIA.AGENCIA"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 7; } }
      public override string SqlServerType { get { return "VARCHAR(7)"; } }

    }
    #endregion

    #region Coluna: BANCO
    private ColumnBanco pvColBanco;

    public sealed class ColumnBanco : TDataColumn {

      public ColumnBanco() {
        this.ColumnName = "banco";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "número do banco";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_agencia.banco"; } }
      public override string OriginalCol { get { return "banco"; } }
      public override string RefCol { get { return "HD_AGENCIA.BANCO"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 4; } }
      public override int Precision { get { return 3; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(3)"; } }

      public override object MinValue { get { return 1; } }

    }
    #endregion

    #region Coluna: NOME
    private ColumnNome pvColNome;

    public sealed class ColumnNome : TDataColumn {

      public ColumnNome() {
        this.ColumnName = "nome";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Nome";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_agencia.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "HD_AGENCIA.NOME"; } }
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

    #region Coluna: MUNICIPIO
    private ColumnMunicipio pvColMunicipio;

    public sealed class ColumnMunicipio : TDataColumn {

      public ColumnMunicipio() {
        this.ColumnName = "municipio";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Município";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_agencia.municipio"; } }
      public override string OriginalCol { get { return "municipio"; } }
      public override string RefCol { get { return "HD_AGENCIA.MUNICIPIO"; } }
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
      public override string FullCol { get { return "hd_agencia.bairro"; } }
      public override string OriginalCol { get { return "bairro"; } }
      public override string RefCol { get { return "HD_AGENCIA.BAIRRO"; } }
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

    #region Coluna: UF
    private ColumnUf pvColUf;

    public sealed class ColumnUf : TDataColumn {

      public ColumnUf() {
        this.ColumnName = "uf";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "estado";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_agencia.uf"; } }
      public override string OriginalCol { get { return "uf"; } }
      public override string RefCol { get { return "HD_AGENCIA.UF"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 2; } }
      public override string SqlServerType { get { return "CHAR(2)"; } }

    }
    #endregion

    #region Coluna: ENDERECO
    private ColumnEndereco pvColEndereco;

    public sealed class ColumnEndereco : TDataColumn {

      public ColumnEndereco() {
        this.ColumnName = "endereco";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Endereço";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_agencia.endereco"; } }
      public override string OriginalCol { get { return "endereco"; } }
      public override string RefCol { get { return "HD_AGENCIA.ENDERECO"; } }
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

    #region Coluna: CONTATO
    private ColumnContato pvColContato;

    public sealed class ColumnContato : TDataColumn {

      public ColumnContato() {
        this.ColumnName = "contato";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Contato";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_agencia.contato"; } }
      public override string OriginalCol { get { return "contato"; } }
      public override string RefCol { get { return "HD_AGENCIA.CONTATO"; } }
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

    #region Coluna: CARGO
    private ColumnCargo pvColCargo;

    public sealed class ColumnCargo : TDataColumn {

      public ColumnCargo() {
        this.ColumnName = "cargo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "cargo";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_agencia.cargo"; } }
      public override string OriginalCol { get { return "cargo"; } }
      public override string RefCol { get { return "HD_AGENCIA.CARGO"; } }
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

    #region Coluna: TELEFONE
    private ColumnTelefone pvColTelefone;

    public sealed class ColumnTelefone : TDataColumn {

      public ColumnTelefone() {
        this.ColumnName = "telefone";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "telefone";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_agencia.telefone"; } }
      public override string OriginalCol { get { return "telefone"; } }
      public override string RefCol { get { return "HD_AGENCIA.TELEFONE"; } }
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

    #region Coluna: CEP
    private ColumnCep pvColCep;

    public sealed class ColumnCep : TDataColumn {

      public ColumnCep() {
        this.ColumnName = "cep";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "cep";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_agencia.cep"; } }
      public override string OriginalCol { get { return "cep"; } }
      public override string RefCol { get { return "HD_AGENCIA.CEP"; } }
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

    #region Coluna: NOME_MUNICIPIO
    private ColumnNome_municipio pvColNome_municipio;

    public sealed class ColumnNome_municipio : TDataColumn {

      public ColumnNome_municipio() {
        this.ColumnName = "nome_municipio";
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

    #region Coluna: NOME_BAIRRO
    private ColumnNome_bairro pvColNome_bairro;

    public sealed class ColumnNome_bairro : TDataColumn {

      public ColumnNome_bairro() {
        this.ColumnName = "nome_bairro";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "nome do bairro";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_bairro.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "HD_BAIRRO.NOME"; } }
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

  public partial class Hd_banco : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Hd_banco() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_banco");
        ctorCustom = CRCustomLib.GetCustomConstructor("Hd_banco");
      }
      catch{}
    }

    public Hd_banco() {
      pvColBanco = new ColumnBanco(); Columns.Add(pvColBanco);
      pvColNome = new ColumnNome(); Columns.Add(pvColNome);
      pvColMaskag = new ColumnMaskag(); Columns.Add(pvColMaskag);
      pvColMaskconta = new ColumnMaskconta(); Columns.Add(pvColMaskconta);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("hd_banco");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(decimal? @banco, string @nome) {
      Row row = NewRow();

      row.Banco = @banco;
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
      get { return "HD_BANCO"; }
    }

    protected override sealed string Name { get { return "Banco"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColBanco });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zHd_banco_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zHd_banco_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zHd_banco_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zHd_banco_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkBanco", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "BANCO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("banco", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "BANCO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("nome", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NOME", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("maskag", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "MASKAG", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("maskconta", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "MASKCONTA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "banco") { result.Add(toLower ? "banco" : column); continue; }
          if(lower == "nome") { result.Add(toLower ? "nome" : column); continue; }
          if(lower == "maskag") { result.Add(toLower ? "maskag" : column); continue; }
          if(lower == "maskconta") { result.Add(toLower ? "maskconta" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "banco", "nome", "maskag", "maskconta" }
                          : new string[] { "BANCO", "NOME", "MASKAG", "MASKCONTA" });
        else
          result.AddRange(toLower
                          ? new string[] { "banco", "nome", "maskag", "maskconta" }
                          : new string[] { "BANCO", "NOME", "MASKAG", "MASKCONTA" });

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
        "hd_banco.banco"
      });
      if(scColumns.Contains("nome")) list.Add("hd_banco.nome");
      if(scColumns.Contains("maskag")) list.Add("hd_banco.maskag");
      if(scColumns.Contains("maskconta")) list.Add("hd_banco.maskconta");
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
          "FROM hd_banco " +
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
            count = da.Fill(ds, startRecord, maxRecords, "hd_banco");
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
        da.TableMappings.Add("Table", "Hd_banco");
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
            if (dr["BANCO"] != null && string.IsNullOrEmpty(dr["BANCO"].ToString()))
              dr["BANCO"] = DBNull.Value;
            if (dr["NOME"] != null && string.IsNullOrEmpty(dr["NOME"].ToString()))
              dr["NOME"] = DBNull.Value;
            if (dr["MASKAG"] != null && string.IsNullOrEmpty(dr["MASKAG"].ToString()))
              dr["MASKAG"] = DBNull.Value;
            if (dr["MASKCONTA"] != null && string.IsNullOrEmpty(dr["MASKCONTA"].ToString()))
              dr["MASKCONTA"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["BANCO"] == null || string.IsNullOrEmpty(dr["BANCO"].ToString()))
            {
              return "Favor informar o número do banco.";
            }

            if (dr["BANCO"] != null && !string.IsNullOrEmpty(dr["BANCO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["BANCO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["BANCO"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["BANCO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["BANCO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O número do banco deve ser um valor inteiro com 3 dígitos no máximo.";
              }

              if (dr["BANCO"] != null && !string.IsNullOrEmpty(dr["BANCO"].ToString()))
              {
                if (Convert.ToDecimal(dr["BANCO"]) < (1))
                {
                  return "O número do banco deve ser maior ou igual a (1).";
                }
              }
            }

            if (dr["NOME"] == null || string.IsNullOrEmpty(dr["NOME"].ToString()))
            {
              return "Favor informar o nome.";
            }

            if (dr["NOME"] != null && !string.IsNullOrEmpty(dr["NOME"].ToString()))
            {
              if (dr["NOME"].ToString().Length > 100)
              {
                return "O nome deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["MASKAG"] != null && !string.IsNullOrEmpty(dr["MASKAG"].ToString()))
            {
              if (dr["MASKAG"].ToString().Length > 7)
              {
                return "A máscara do número de agência deve possuir no máximo 7 caracteres.";
              }
            }

            if (dr["MASKCONTA"] != null && !string.IsNullOrEmpty(dr["MASKCONTA"].ToString()))
            {
              if (dr["MASKCONTA"].ToString().Length > 15)
              {
                return "A máscara do número de conta deve possuir no máximo 15 caracteres.";
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

    public static Hd_banco Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Hd_banco Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Hd_banco tab = new Hd_banco();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Hd_banco tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @banco, string @nome, string @maskag, string @maskconta
      ) {
        return (Row)Add(new object[] {
          @banco, @nome, @maskag, @maskconta
        });
      }

      public Row Find(
        decimal? @banco
      ) {
        return (Row)Find(new object[] {
          @banco
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
      /// BANCO: NUMERIC(3), Primary Key, Not Null
      /// </summary>
      public decimal? Banco { get { return (decimal?)this["banco"]; } set { this["banco"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(100), Not Null
      /// </summary>
      public string Nome { get { return (string)this["nome"]; } set { this["nome"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// MASKAG: VARCHAR(7)
      /// </summary>
      public string Maskag { get { return (string)this["maskag"]; } set { this["maskag"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// MASKCONTA: VARCHAR(15)
      /// </summary>
      public string Maskconta { get { return (string)this["maskconta"]; } set { this["maskconta"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               decimal? @banco, string @nome) {
        return Insert(connection,
                      @banco, @nome,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               decimal? @banco, string @nome,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.HadesLyc.CR.Hd_banco(),
          new string[] { "banco", "nome" },
          new object[] { @banco, @nome },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkBanco,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.HadesLyc.CR.Hd_banco(),
          new string[] { "banco" },
          new object[] { pkBanco },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkBanco) {
        return Delete(
          connection, new Techne.HadesLyc.CR.Hd_banco(),
          new string[] { "banco" },
          new object[] { pkBanco }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              decimal? pkBanco) {
        return (Row)Query(
          connection, new Techne.HadesLyc.CR.Hd_banco(),
          new string[] { "banco" },
          new object[] { pkBanco }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public decimal? Banco { get { return (decimal?)row["banco", DataRowVersion.Original]; } }
        public string Nome { get { return (string)row["nome", DataRowVersion.Original]; } }
        public string Maskag { get { return (string)row["maskag", DataRowVersion.Original]; } }
        public string Maskconta { get { return (string)row["maskconta", DataRowVersion.Original]; } }

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
    #region Coluna: BANCO
    private ColumnBanco pvColBanco;

    public sealed class ColumnBanco : TDataColumn {

      public ColumnBanco() {
        this.ColumnName = "banco";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "número do banco";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_banco.banco"; } }
      public override string OriginalCol { get { return "banco"; } }
      public override string RefCol { get { return "HD_BANCO.BANCO"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 4; } }
      public override int Precision { get { return 3; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(3)"; } }

      public override object MinValue { get { return 1; } }

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
      public override string FullCol { get { return "hd_banco.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "HD_BANCO.NOME"; } }
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

    #region Coluna: MASKAG
    private ColumnMaskag pvColMaskag;

    public sealed class ColumnMaskag : TDataColumn {

      public ColumnMaskag() {
        this.ColumnName = "maskag";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "máscara do número de agência";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_banco.maskag"; } }
      public override string OriginalCol { get { return "maskag"; } }
      public override string RefCol { get { return "HD_BANCO.MASKAG"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 7; } }
      public override string SqlServerType { get { return "VARCHAR(7)"; } }

    }
    #endregion

    #region Coluna: MASKCONTA
    private ColumnMaskconta pvColMaskconta;

    public sealed class ColumnMaskconta : TDataColumn {

      public ColumnMaskconta() {
        this.ColumnName = "maskconta";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "máscara do número de conta";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "hd_banco.maskconta"; } }
      public override string OriginalCol { get { return "maskconta"; } }
      public override string RefCol { get { return "HD_BANCO.MASKCONTA"; } }
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
    #endregion
  }
}

