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
  public class Biblioteca : TDataSet {
    private const string DataSetName_Def = "Biblioteca";
    private static ConstructorInfo ctorTCustom = null;
    private CustomBase tcustom = null;
    #region Variáveis private para as tabelas deste dataset
    private Ly_bib_assunto_cdd pvTabLy_bib_assunto_cdd;
    private Ly_bib_autor pvTabLy_bib_autor;
    private Ly_bib_autor_titulo pvTabLy_bib_autor_titulo;
    private Ly_bib_biblioteca pvTabLy_bib_biblioteca;
    private Ly_bib_configuracoes pvTabLy_bib_configuracoes;
    private Ly_bib_editora pvTabLy_bib_editora;
    private Ly_bib_emprestimo pvTabLy_bib_emprestimo;
    private Ly_bib_localizacao pvTabLy_bib_localizacao;
    private Ly_bib_material pvTabLy_bib_material;
    private Ly_bib_participante pvTabLy_bib_participante;
    private Ly_bib_penalidade pvTabLy_bib_penalidade;
    private Ly_bib_projeto pvTabLy_bib_projeto;
    private Ly_bib_reserva pvTabLy_bib_reserva;
    private Ly_bib_sugestao pvTabLy_bib_sugestao;
    private Ly_bib_tipo_material pvTabLy_bib_tipo_material;
    private Ly_bib_titulo pvTabLy_bib_titulo;
    private Ly_bib_usuario pvTabLy_bib_usuario;
    #endregion

    static Biblioteca() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Biblioteca");
      }
      catch{}
    }

    public Biblioteca() : base() {
      DataSetName = DataSetName_Def;

      pvTabLy_bib_assunto_cdd = new Ly_bib_assunto_cdd();
      pvTabLy_bib_autor = new Ly_bib_autor();
      pvTabLy_bib_autor_titulo = new Ly_bib_autor_titulo();
      pvTabLy_bib_biblioteca = new Ly_bib_biblioteca();
      pvTabLy_bib_configuracoes = new Ly_bib_configuracoes();
      pvTabLy_bib_editora = new Ly_bib_editora();
      pvTabLy_bib_emprestimo = new Ly_bib_emprestimo();
      pvTabLy_bib_localizacao = new Ly_bib_localizacao();
      pvTabLy_bib_material = new Ly_bib_material();
      pvTabLy_bib_participante = new Ly_bib_participante();
      pvTabLy_bib_penalidade = new Ly_bib_penalidade();
      pvTabLy_bib_projeto = new Ly_bib_projeto();
      pvTabLy_bib_reserva = new Ly_bib_reserva();
      pvTabLy_bib_sugestao = new Ly_bib_sugestao();
      pvTabLy_bib_tipo_material = new Ly_bib_tipo_material();
      pvTabLy_bib_titulo = new Ly_bib_titulo();
      pvTabLy_bib_usuario = new Ly_bib_usuario();

      Tables.Add(pvTabLy_bib_assunto_cdd);
      Tables.Add(pvTabLy_bib_autor);
      Tables.Add(pvTabLy_bib_autor_titulo);
      Tables.Add(pvTabLy_bib_biblioteca);
      Tables.Add(pvTabLy_bib_configuracoes);
      Tables.Add(pvTabLy_bib_editora);
      Tables.Add(pvTabLy_bib_emprestimo);
      Tables.Add(pvTabLy_bib_localizacao);
      Tables.Add(pvTabLy_bib_material);
      Tables.Add(pvTabLy_bib_participante);
      Tables.Add(pvTabLy_bib_penalidade);
      Tables.Add(pvTabLy_bib_projeto);
      Tables.Add(pvTabLy_bib_reserva);
      Tables.Add(pvTabLy_bib_sugestao);
      Tables.Add(pvTabLy_bib_tipo_material);
      Tables.Add(pvTabLy_bib_titulo);
      Tables.Add(pvTabLy_bib_usuario);

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
          pvTabLy_bib_assunto_cdd.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_bib_assunto_cdd); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_bib_autor.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_bib_autor); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_bib_autor_titulo.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_bib_autor_titulo); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_bib_biblioteca.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_bib_biblioteca); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_bib_configuracoes.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_bib_configuracoes); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_bib_editora.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_bib_editora); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_bib_emprestimo.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_bib_emprestimo); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_bib_localizacao.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_bib_localizacao); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_bib_material.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_bib_material); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_bib_participante.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_bib_participante); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_bib_penalidade.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_bib_penalidade); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_bib_projeto.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_bib_projeto); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_bib_reserva.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_bib_reserva); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_bib_sugestao.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_bib_sugestao); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_bib_tipo_material.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_bib_tipo_material); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_bib_titulo.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_bib_titulo); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_bib_usuario.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_bib_usuario); if(row != null) return DataLib.RowErrorsToString(row);
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
    public Ly_bib_assunto_cdd Ly_bib_assunto_cdd { get { return pvTabLy_bib_assunto_cdd; } }

    [
      Browsable(false),
    ]
    public Ly_bib_autor Ly_bib_autor { get { return pvTabLy_bib_autor; } }

    [
      Browsable(false),
    ]
    public Ly_bib_autor_titulo Ly_bib_autor_titulo { get { return pvTabLy_bib_autor_titulo; } }

    [
      Browsable(false),
    ]
    public Ly_bib_biblioteca Ly_bib_biblioteca { get { return pvTabLy_bib_biblioteca; } }

    [
      Browsable(false),
    ]
    public Ly_bib_configuracoes Ly_bib_configuracoes { get { return pvTabLy_bib_configuracoes; } }

    [
      Browsable(false),
    ]
    public Ly_bib_editora Ly_bib_editora { get { return pvTabLy_bib_editora; } }

    [
      Browsable(false),
    ]
    public Ly_bib_emprestimo Ly_bib_emprestimo { get { return pvTabLy_bib_emprestimo; } }

    [
      Browsable(false),
    ]
    public Ly_bib_localizacao Ly_bib_localizacao { get { return pvTabLy_bib_localizacao; } }

    [
      Browsable(false),
    ]
    public Ly_bib_material Ly_bib_material { get { return pvTabLy_bib_material; } }

    [
      Browsable(false),
    ]
    public Ly_bib_participante Ly_bib_participante { get { return pvTabLy_bib_participante; } }

    [
      Browsable(false),
    ]
    public Ly_bib_penalidade Ly_bib_penalidade { get { return pvTabLy_bib_penalidade; } }

    [
      Browsable(false),
    ]
    public Ly_bib_projeto Ly_bib_projeto { get { return pvTabLy_bib_projeto; } }

    [
      Browsable(false),
    ]
    public Ly_bib_reserva Ly_bib_reserva { get { return pvTabLy_bib_reserva; } }

    [
      Browsable(false),
    ]
    public Ly_bib_sugestao Ly_bib_sugestao { get { return pvTabLy_bib_sugestao; } }

    [
      Browsable(false),
    ]
    public Ly_bib_tipo_material Ly_bib_tipo_material { get { return pvTabLy_bib_tipo_material; } }

    [
      Browsable(false),
    ]
    public Ly_bib_titulo Ly_bib_titulo { get { return pvTabLy_bib_titulo; } }

    [
      Browsable(false),
    ]
    public Ly_bib_usuario Ly_bib_usuario { get { return pvTabLy_bib_usuario; } }

    #endregion

    public abstract class CustomBase {
      public abstract string Put(Techne.Lyceum.CR.Biblioteca ds, TConnectionWritable connection);
    }

  }


  public partial class Ly_bib_assunto_cdd : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_bib_assunto_cdd() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_bib_assunto_cdd");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_bib_assunto_cdd");
      }
      catch{}
    }

    public Ly_bib_assunto_cdd() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColCodigo_cdd = new ColumnCodigo_cdd(); Columns.Add(pvColCodigo_cdd);
      pvColDescricao_cdd = new ColumnDescricao_cdd(); Columns.Add(pvColDescricao_cdd);
      pvColId_bib_assunto_cdd_pai = new ColumnId_bib_assunto_cdd_pai(); Columns.Add(pvColId_bib_assunto_cdd_pai);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_bib_assunto_cdd");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @codigo_cdd) {
      Row row = NewRow();

      row.Codigo_cdd = @codigo_cdd;

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
      get { return "LY_BIB_ASSUNTO_CDD"; }
    }

    protected override sealed string Name { get { return "Assunto CDD"; } }

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
        case CommandOperation.Select: cm.CommandText = "zLy_bib_assunto_cdd_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_bib_assunto_cdd_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_bib_assunto_cdd_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_bib_assunto_cdd_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("codigo_cdd", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CODIGO_CDD", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("descricao_cdd", OleDbType.VarChar, 600, ParameterDirection.InputOutput, true, 0, 0, "DESCRICAO_CDD", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("id_bib_assunto_cdd_pai", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_BIB_ASSUNTO_CDD_PAI", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "codigo_cdd") { result.Add(toLower ? "codigo_cdd" : column); continue; }
          if(lower == "descricao_cdd") { result.Add(toLower ? "descricao_cdd" : column); continue; }
          if(lower == "id_bib_assunto_cdd_pai") { result.Add(toLower ? "id_bib_assunto_cdd_pai" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "codigo_cdd", "descricao_cdd", "id_bib_assunto_cdd_pai" }
                          : new string[] { "ID", "CODIGO_CDD", "DESCRICAO_CDD", "ID_BIB_ASSUNTO_CDD_PAI" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "codigo_cdd", "descricao_cdd", "id_bib_assunto_cdd_pai" }
                          : new string[] { "ID", "CODIGO_CDD", "DESCRICAO_CDD", "ID_BIB_ASSUNTO_CDD_PAI" });

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
        "ly_bib_assunto_cdd.id"
      });
      if(scColumns.Contains("codigo_cdd")) list.Add("ly_bib_assunto_cdd.codigo_cdd");
      if(scColumns.Contains("descricao_cdd")) list.Add("ly_bib_assunto_cdd.descricao_cdd");
      if(scColumns.Contains("id_bib_assunto_cdd_pai")) list.Add("ly_bib_assunto_cdd.id_bib_assunto_cdd_pai");
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
          "FROM ly_bib_assunto_cdd " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_bib_assunto_cdd");
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
        da.TableMappings.Add("Table", "Ly_bib_assunto_cdd");
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
            if (dr["CODIGO_CDD"] != null && string.IsNullOrEmpty(dr["CODIGO_CDD"].ToString()))
              dr["CODIGO_CDD"] = DBNull.Value;
            if (dr["DESCRICAO_CDD"] != null && string.IsNullOrEmpty(dr["DESCRICAO_CDD"].ToString()))
              dr["DESCRICAO_CDD"] = DBNull.Value;
            if (dr["ID_BIB_ASSUNTO_CDD_PAI"] != null && string.IsNullOrEmpty(dr["ID_BIB_ASSUNTO_CDD_PAI"].ToString()))
              dr["ID_BIB_ASSUNTO_CDD_PAI"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["CODIGO_CDD"] == null || string.IsNullOrEmpty(dr["CODIGO_CDD"].ToString()))
            {
              return "Favor informar o código CDD.";
            }

            if (dr["CODIGO_CDD"] != null && !string.IsNullOrEmpty(dr["CODIGO_CDD"].ToString()))
            {
              if (dr["CODIGO_CDD"].ToString().Length > 50)
              {
                return "O código CDD deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["DESCRICAO_CDD"] != null && !string.IsNullOrEmpty(dr["DESCRICAO_CDD"].ToString()))
            {
              if (dr["DESCRICAO_CDD"].ToString().Length > 500)
              {
                return "A descrição deve possuir no máximo 500 caracteres.";
              }
            }

            if (dr["ID_BIB_ASSUNTO_CDD_PAI"] != null && !string.IsNullOrEmpty(dr["ID_BIB_ASSUNTO_CDD_PAI"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_BIB_ASSUNTO_CDD_PAI"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_ASSUNTO_CDD_PAI"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_BIB_ASSUNTO_CDD_PAI"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_ASSUNTO_CDD_PAI"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O assunto deve ser um valor inteiro com 14 dígitos no máximo.";
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

    public static Ly_bib_assunto_cdd Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_bib_assunto_cdd Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_bib_assunto_cdd tab = new Ly_bib_assunto_cdd();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_bib_assunto_cdd tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, string @codigo_cdd, string @descricao_cdd, decimal? @id_bib_assunto_cdd_pai
      ) {
        return (Row)Add(new object[] {
          @id, @codigo_cdd, @descricao_cdd, @id_bib_assunto_cdd_pai
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
      /// CODIGO_CDD: VARCHAR(50), Not Null
      /// </summary>
      public string Codigo_cdd { get { return (string)this["codigo_cdd"]; } set { this["codigo_cdd"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO_CDD: VARCHAR(500)
      /// </summary>
      public string Descricao_cdd { get { return (string)this["descricao_cdd"]; } set { this["descricao_cdd"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ID_BIB_ASSUNTO_CDD_PAI: NUMERIC(14)
      /// </summary>
      public decimal? Id_bib_assunto_cdd_pai { get { return (decimal?)this["id_bib_assunto_cdd_pai"]; } set { this["id_bib_assunto_cdd_pai"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @codigo_cdd) {
        return Insert(connection,
                      @codigo_cdd,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @codigo_cdd,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_bib_assunto_cdd(),
          new string[] { "codigo_cdd" },
          new object[] { @codigo_cdd },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_bib_assunto_cdd(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_bib_assunto_cdd(),
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
          connection, new Techne.Lyceum.CR.Ly_bib_assunto_cdd(),
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
        public string Codigo_cdd { get { return (string)row["codigo_cdd", DataRowVersion.Original]; } }
        public string Descricao_cdd { get { return (string)row["descricao_cdd", DataRowVersion.Original]; } }
        public decimal? Id_bib_assunto_cdd_pai { get { return (decimal?)row["id_bib_assunto_cdd_pai", DataRowVersion.Original]; } }

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


      protected override string GetName(){return "identificador";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_assunto_cdd.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_BIB_ASSUNTO_CDD.ID"; } }
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

    #region Coluna: CODIGO_CDD
    private ColumnCodigo_cdd pvColCodigo_cdd;

    public sealed class ColumnCodigo_cdd : TDataColumn {

      public ColumnCodigo_cdd() {
        this.ColumnName = "codigo_cdd";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "código CDD";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_assunto_cdd.codigo_cdd"; } }
      public override string OriginalCol { get { return "codigo_cdd"; } }
      public override string RefCol { get { return "LY_BIB_ASSUNTO_CDD.CODIGO_CDD"; } }
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

    #region Coluna: DESCRICAO_CDD
    private ColumnDescricao_cdd pvColDescricao_cdd;

    public sealed class ColumnDescricao_cdd : TDataColumn {

      public ColumnDescricao_cdd() {
        this.ColumnName = "descricao_cdd";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "descrição";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_assunto_cdd.descricao_cdd"; } }
      public override string OriginalCol { get { return "descricao_cdd"; } }
      public override string RefCol { get { return "LY_BIB_ASSUNTO_CDD.DESCRICAO_CDD"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 500; } }
      public override string SqlServerType { get { return "VARCHAR(500)"; } }

    }
    #endregion

    #region Coluna: ID_BIB_ASSUNTO_CDD_PAI
    private ColumnId_bib_assunto_cdd_pai pvColId_bib_assunto_cdd_pai;

    public sealed class ColumnId_bib_assunto_cdd_pai : TDataColumn {

      public ColumnId_bib_assunto_cdd_pai() {
        this.ColumnName = "id_bib_assunto_cdd_pai";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "assunto";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_assunto_cdd.id_bib_assunto_cdd_pai"; } }
      public override string OriginalCol { get { return "id_bib_assunto_cdd_pai"; } }
      public override string RefCol { get { return "LY_BIB_ASSUNTO_CDD.ID_BIB_ASSUNTO_CDD_PAI"; } }
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
    #endregion
  }

  public partial class Ly_bib_autor : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_bib_autor() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_bib_autor");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_bib_autor");
      }
      catch{}
    }

    public Ly_bib_autor() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColNome_autor = new ColumnNome_autor(); Columns.Add(pvColNome_autor);
      pvColAno_nasc_autor = new ColumnAno_nasc_autor(); Columns.Add(pvColAno_nasc_autor);
      pvColLocal_nasc_autor = new ColumnLocal_nasc_autor(); Columns.Add(pvColLocal_nasc_autor);
      pvColBiografia_autor = new ColumnBiografia_autor(); Columns.Add(pvColBiografia_autor);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_bib_autor");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @nome_autor) {
      Row row = NewRow();

      row.Nome_autor = @nome_autor;

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
      get { return "LY_BIB_AUTOR"; }
    }

    protected override sealed string Name { get { return "Autor"; } }

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
        case CommandOperation.Select: cm.CommandText = "zLy_bib_autor_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_bib_autor_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_bib_autor_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_bib_autor_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("nome_autor", OleDbType.VarChar, 600, ParameterDirection.InputOutput, true, 0, 0, "NOME_AUTOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("ano_nasc_autor", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ANO_NASC_AUTOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("local_nasc_autor", OleDbType.VarChar, 600, ParameterDirection.InputOutput, true, 0, 0, "LOCAL_NASC_AUTOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("biografia_autor", OleDbType.VarChar, 600, ParameterDirection.InputOutput, true, 0, 0, "BIOGRAFIA_AUTOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "nome_autor") { result.Add(toLower ? "nome_autor" : column); continue; }
          if(lower == "ano_nasc_autor") { result.Add(toLower ? "ano_nasc_autor" : column); continue; }
          if(lower == "local_nasc_autor") { result.Add(toLower ? "local_nasc_autor" : column); continue; }
          if(lower == "biografia_autor") { result.Add(toLower ? "biografia_autor" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "nome_autor", "ano_nasc_autor", "local_nasc_autor", "biografia_autor" }
                          : new string[] { "ID", "NOME_AUTOR", "ANO_NASC_AUTOR", "LOCAL_NASC_AUTOR", "BIOGRAFIA_AUTOR" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "nome_autor", "ano_nasc_autor", "local_nasc_autor", "biografia_autor" }
                          : new string[] { "ID", "NOME_AUTOR", "ANO_NASC_AUTOR", "LOCAL_NASC_AUTOR", "BIOGRAFIA_AUTOR" });

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
        "ly_bib_autor.id"
      });
      if(scColumns.Contains("nome_autor")) list.Add("ly_bib_autor.nome_autor");
      if(scColumns.Contains("ano_nasc_autor")) list.Add("ly_bib_autor.ano_nasc_autor");
      if(scColumns.Contains("local_nasc_autor")) list.Add("ly_bib_autor.local_nasc_autor");
      if(scColumns.Contains("biografia_autor")) list.Add("ly_bib_autor.biografia_autor");
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
          "FROM ly_bib_autor " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_bib_autor");
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
        da.TableMappings.Add("Table", "Ly_bib_autor");
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
            if (dr["NOME_AUTOR"] != null && string.IsNullOrEmpty(dr["NOME_AUTOR"].ToString()))
              dr["NOME_AUTOR"] = DBNull.Value;
            if (dr["ANO_NASC_AUTOR"] != null && string.IsNullOrEmpty(dr["ANO_NASC_AUTOR"].ToString()))
              dr["ANO_NASC_AUTOR"] = DBNull.Value;
            if (dr["LOCAL_NASC_AUTOR"] != null && string.IsNullOrEmpty(dr["LOCAL_NASC_AUTOR"].ToString()))
              dr["LOCAL_NASC_AUTOR"] = DBNull.Value;
            if (dr["BIOGRAFIA_AUTOR"] != null && string.IsNullOrEmpty(dr["BIOGRAFIA_AUTOR"].ToString()))
              dr["BIOGRAFIA_AUTOR"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["NOME_AUTOR"] == null || string.IsNullOrEmpty(dr["NOME_AUTOR"].ToString()))
            {
              return "Favor informar o nome.";
            }

            if (dr["NOME_AUTOR"] != null && !string.IsNullOrEmpty(dr["NOME_AUTOR"].ToString()))
            {
              if (dr["NOME_AUTOR"].ToString().Length > 500)
              {
                return "O nome deve possuir no máximo 500 caracteres.";
              }
            }

            if (dr["ANO_NASC_AUTOR"] != null && !string.IsNullOrEmpty(dr["ANO_NASC_AUTOR"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ANO_NASC_AUTOR"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ANO_NASC_AUTOR"]).ToString("0.############").Split(',')[0].Length > 4) || (Convert.ToDecimal(dr["ANO_NASC_AUTOR"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ANO_NASC_AUTOR"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O ano de nascimento deve ser um valor inteiro com 4 dígitos no máximo.";
              }
            }

            if (dr["LOCAL_NASC_AUTOR"] != null && !string.IsNullOrEmpty(dr["LOCAL_NASC_AUTOR"].ToString()))
            {
              if (dr["LOCAL_NASC_AUTOR"].ToString().Length > 500)
              {
                return "O local de nascimento deve possuir no máximo 500 caracteres.";
              }
            }

            if (dr["BIOGRAFIA_AUTOR"] != null && !string.IsNullOrEmpty(dr["BIOGRAFIA_AUTOR"].ToString()))
            {
              if (dr["BIOGRAFIA_AUTOR"].ToString().Length > 500)
              {
                return "A biografia deve possuir no máximo 500 caracteres.";
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

    public static Ly_bib_autor Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_bib_autor Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_bib_autor tab = new Ly_bib_autor();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_bib_autor tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, string @nome_autor, decimal? @ano_nasc_autor, string @local_nasc_autor, 
        string @biografia_autor
      ) {
        return (Row)Add(new object[] {
          @id, @nome_autor, @ano_nasc_autor, @local_nasc_autor, @biografia_autor
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
      /// NOME_AUTOR: VARCHAR(500), Not Null
      /// </summary>
      public string Nome_autor { get { return (string)this["nome_autor"]; } set { this["nome_autor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ANO_NASC_AUTOR: NUMERIC(4)
      /// </summary>
      public decimal? Ano_nasc_autor { get { return (decimal?)this["ano_nasc_autor"]; } set { this["ano_nasc_autor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// LOCAL_NASC_AUTOR: VARCHAR(500)
      /// </summary>
      public string Local_nasc_autor { get { return (string)this["local_nasc_autor"]; } set { this["local_nasc_autor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// BIOGRAFIA_AUTOR: VARCHAR(500)
      /// </summary>
      public string Biografia_autor { get { return (string)this["biografia_autor"]; } set { this["biografia_autor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @nome_autor) {
        return Insert(connection,
                      @nome_autor,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @nome_autor,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_bib_autor(),
          new string[] { "nome_autor" },
          new object[] { @nome_autor },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_bib_autor(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_bib_autor(),
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
          connection, new Techne.Lyceum.CR.Ly_bib_autor(),
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
        public string Nome_autor { get { return (string)row["nome_autor", DataRowVersion.Original]; } }
        public decimal? Ano_nasc_autor { get { return (decimal?)row["ano_nasc_autor", DataRowVersion.Original]; } }
        public string Local_nasc_autor { get { return (string)row["local_nasc_autor", DataRowVersion.Original]; } }
        public string Biografia_autor { get { return (string)row["biografia_autor", DataRowVersion.Original]; } }

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


      protected override string GetName(){return "identificador";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_autor.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_BIB_AUTOR.ID"; } }
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

    #region Coluna: NOME_AUTOR
    private ColumnNome_autor pvColNome_autor;

    public sealed class ColumnNome_autor : TDataColumn {

      public ColumnNome_autor() {
        this.ColumnName = "nome_autor";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "nome";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_autor.nome_autor"; } }
      public override string OriginalCol { get { return "nome_autor"; } }
      public override string RefCol { get { return "LY_BIB_AUTOR.NOME_AUTOR"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 500; } }
      public override string SqlServerType { get { return "VARCHAR(500)"; } }

    }
    #endregion

    #region Coluna: ANO_NASC_AUTOR
    private ColumnAno_nasc_autor pvColAno_nasc_autor;

    public sealed class ColumnAno_nasc_autor : TDataColumn {

      public ColumnAno_nasc_autor() {
        this.ColumnName = "ano_nasc_autor";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "ano de nascimento";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_autor.ano_nasc_autor"; } }
      public override string OriginalCol { get { return "ano_nasc_autor"; } }
      public override string RefCol { get { return "LY_BIB_AUTOR.ANO_NASC_AUTOR"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 5; } }
      public override int Precision { get { return 4; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(4)"; } }

    }
    #endregion

    #region Coluna: LOCAL_NASC_AUTOR
    private ColumnLocal_nasc_autor pvColLocal_nasc_autor;

    public sealed class ColumnLocal_nasc_autor : TDataColumn {

      public ColumnLocal_nasc_autor() {
        this.ColumnName = "local_nasc_autor";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "local de nascimento";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_autor.local_nasc_autor"; } }
      public override string OriginalCol { get { return "local_nasc_autor"; } }
      public override string RefCol { get { return "LY_BIB_AUTOR.LOCAL_NASC_AUTOR"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 500; } }
      public override string SqlServerType { get { return "VARCHAR(500)"; } }

    }
    #endregion

    #region Coluna: BIOGRAFIA_AUTOR
    private ColumnBiografia_autor pvColBiografia_autor;

    public sealed class ColumnBiografia_autor : TDataColumn {

      public ColumnBiografia_autor() {
        this.ColumnName = "biografia_autor";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "biografia";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_autor.biografia_autor"; } }
      public override string OriginalCol { get { return "biografia_autor"; } }
      public override string RefCol { get { return "LY_BIB_AUTOR.BIOGRAFIA_AUTOR"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 500; } }
      public override string SqlServerType { get { return "VARCHAR(500)"; } }

    }
    #endregion
    #endregion
  }

  public partial class Ly_bib_autor_titulo : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_bib_autor_titulo() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_bib_autor_titulo");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_bib_autor_titulo");
      }
      catch{}
    }

    public Ly_bib_autor_titulo() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColId_bib_autor = new ColumnId_bib_autor(); Columns.Add(pvColId_bib_autor);
      pvColId_bib_titulo = new ColumnId_bib_titulo(); Columns.Add(pvColId_bib_titulo);
      pvColAno_nasc_autor = new ColumnAno_nasc_autor(); Columns.Add(pvColAno_nasc_autor);
      pvColLocal_nasc_autor = new ColumnLocal_nasc_autor(); Columns.Add(pvColLocal_nasc_autor);
      pvColNome_autor = new ColumnNome_autor(); Columns.Add(pvColNome_autor);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_bib_autor_titulo");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(decimal? @id_bib_autor, decimal? @id_bib_titulo) {
      Row row = NewRow();

      row.Id_bib_autor = @id_bib_autor;
      row.Id_bib_titulo = @id_bib_titulo;

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
      get { return "LY_BIB_AUTOR_TITULO"; }
    }

    protected override sealed string Name { get { return "Autor do Título"; } }

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
        case CommandOperation.Select: cm.CommandText = "zLy_bib_autor_titulo_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_bib_autor_titulo_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_bib_autor_titulo_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_bib_autor_titulo_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("id_bib_autor", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_BIB_AUTOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("id_bib_titulo", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_BIB_TITULO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "id_bib_autor") { result.Add(toLower ? "id_bib_autor" : column); continue; }
          if(lower == "id_bib_titulo") { result.Add(toLower ? "id_bib_titulo" : column); continue; }
          if(!tabPrincipal) {
            if(lower == "ano_nasc_autor") { result.Add(toLower ? "ano_nasc_autor" : column); continue; }
            if(lower == "local_nasc_autor") { result.Add(toLower ? "local_nasc_autor" : column); continue; }
            if(lower == "nome_autor") { result.Add(toLower ? "nome_autor" : column); continue; }
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "id_bib_autor", "id_bib_titulo" }
                          : new string[] { "ID", "ID_BIB_AUTOR", "ID_BIB_TITULO" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "id_bib_autor", "id_bib_titulo", "ano_nasc_autor", "local_nasc_autor", 
                                           "nome_autor" }
                          : new string[] { "ID", "ID_BIB_AUTOR", "ID_BIB_TITULO", "ANO_NASC_AUTOR", "LOCAL_NASC_AUTOR", 
                                           "NOME_AUTOR" });

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
        "ly_bib_autor_titulo.id"
      });
      if(scColumns.Contains("id_bib_autor")) list.Add("ly_bib_autor_titulo.id_bib_autor");
      if(scColumns.Contains("id_bib_titulo")) list.Add("ly_bib_autor_titulo.id_bib_titulo");
      if(scColumns.Contains("ano_nasc_autor")) list.Add("ly_bib_autor.ano_nasc_autor");
      if(scColumns.Contains("local_nasc_autor")) list.Add("ly_bib_autor.local_nasc_autor");
      if(scColumns.Contains("nome_autor")) list.Add("ly_bib_autor.nome_autor");
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
          "FROM ly_bib_autor_titulo " +
               "INNER JOIN ly_bib_autor " +
                 "ON ly_bib_autor.id = ly_bib_autor_titulo.id_bib_autor " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_bib_autor_titulo");
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
        da.TableMappings.Add("Table", "Ly_bib_autor_titulo");
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
            if (dr["ID_BIB_AUTOR"] != null && string.IsNullOrEmpty(dr["ID_BIB_AUTOR"].ToString()))
              dr["ID_BIB_AUTOR"] = DBNull.Value;
            if (dr["ID_BIB_TITULO"] != null && string.IsNullOrEmpty(dr["ID_BIB_TITULO"].ToString()))
              dr["ID_BIB_TITULO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["ID_BIB_AUTOR"] == null || string.IsNullOrEmpty(dr["ID_BIB_AUTOR"].ToString()))
            {
              return "Favor informar o autor.";
            }

            if (dr["ID_BIB_AUTOR"] != null && !string.IsNullOrEmpty(dr["ID_BIB_AUTOR"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_BIB_AUTOR"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_AUTOR"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_BIB_AUTOR"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_AUTOR"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O autor deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["ID_BIB_TITULO"] == null || string.IsNullOrEmpty(dr["ID_BIB_TITULO"].ToString()))
            {
              return "Favor informar o título.";
            }

            if (dr["ID_BIB_TITULO"] != null && !string.IsNullOrEmpty(dr["ID_BIB_TITULO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_BIB_TITULO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_TITULO"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_BIB_TITULO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_TITULO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O título deve ser um valor inteiro com 14 dígitos no máximo.";
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

    public static Ly_bib_autor_titulo Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_bib_autor_titulo Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_bib_autor_titulo tab = new Ly_bib_autor_titulo();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_bib_autor_titulo tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, decimal? @id_bib_autor, decimal? @id_bib_titulo
      ) {
        return (Row)Add(new object[] {
          @id, @id_bib_autor, @id_bib_titulo
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
      /// ID_BIB_AUTOR: NUMERIC(14), Not Null
      /// </summary>
      public decimal? Id_bib_autor { get { return (decimal?)this["id_bib_autor"]; } set { this["id_bib_autor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ID_BIB_TITULO: NUMERIC(14), Not Null
      /// </summary>
      public decimal? Id_bib_titulo { get { return (decimal?)this["id_bib_titulo"]; } set { this["id_bib_titulo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ANO_NASC_AUTOR: NUMERIC(4)
      /// </summary>
      public decimal? Ano_nasc_autor { get { return (decimal?)this["ano_nasc_autor"]; } set { this["ano_nasc_autor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// LOCAL_NASC_AUTOR: VARCHAR(500)
      /// </summary>
      public string Local_nasc_autor { get { return (string)this["local_nasc_autor"]; } set { this["local_nasc_autor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME_AUTOR: VARCHAR(500), Not Null
      /// </summary>
      public string Nome_autor { get { return (string)this["nome_autor"]; } set { this["nome_autor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               decimal? @id_bib_autor, decimal? @id_bib_titulo) {
        return Insert(connection,
                      @id_bib_autor, @id_bib_titulo,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               decimal? @id_bib_autor, decimal? @id_bib_titulo,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_bib_autor_titulo(),
          new string[] { "id_bib_autor", "id_bib_titulo" },
          new object[] { @id_bib_autor, @id_bib_titulo },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_bib_autor_titulo(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_bib_autor_titulo(),
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
          connection, new Techne.Lyceum.CR.Ly_bib_autor_titulo(),
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
        public decimal? Id_bib_autor { get { return (decimal?)row["id_bib_autor", DataRowVersion.Original]; } }
        public decimal? Id_bib_titulo { get { return (decimal?)row["id_bib_titulo", DataRowVersion.Original]; } }
        public decimal? Ano_nasc_autor { get { return (decimal?)row["ano_nasc_autor", DataRowVersion.Original]; } }
        public string Local_nasc_autor { get { return (string)row["local_nasc_autor", DataRowVersion.Original]; } }
        public string Nome_autor { get { return (string)row["nome_autor", DataRowVersion.Original]; } }

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


      protected override string GetName(){return "identificador";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_autor_titulo.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_BIB_AUTOR_TITULO.ID"; } }
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

    #region Coluna: ID_BIB_AUTOR
    private ColumnId_bib_autor pvColId_bib_autor;

    public sealed class ColumnId_bib_autor : TDataColumn {

      public ColumnId_bib_autor() {
        this.ColumnName = "id_bib_autor";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "autor";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_autor_titulo.id_bib_autor"; } }
      public override string OriginalCol { get { return "id_bib_autor"; } }
      public override string RefCol { get { return "LY_BIB_AUTOR_TITULO.ID_BIB_AUTOR"; } }
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

    #region Coluna: ID_BIB_TITULO
    private ColumnId_bib_titulo pvColId_bib_titulo;

    public sealed class ColumnId_bib_titulo : TDataColumn {

      public ColumnId_bib_titulo() {
        this.ColumnName = "id_bib_titulo";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "título";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_autor_titulo.id_bib_titulo"; } }
      public override string OriginalCol { get { return "id_bib_titulo"; } }
      public override string RefCol { get { return "LY_BIB_AUTOR_TITULO.ID_BIB_TITULO"; } }
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

    #region Coluna: ANO_NASC_AUTOR
    private ColumnAno_nasc_autor pvColAno_nasc_autor;

    public sealed class ColumnAno_nasc_autor : TDataColumn {

      public ColumnAno_nasc_autor() {
        this.ColumnName = "ano_nasc_autor";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "ano de nascimento";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_autor.ano_nasc_autor"; } }
      public override string OriginalCol { get { return "ano_nasc_autor"; } }
      public override string RefCol { get { return "LY_BIB_AUTOR.ANO_NASC_AUTOR"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 5; } }
      public override int Precision { get { return 4; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(4)"; } }

    }
    #endregion

    #region Coluna: LOCAL_NASC_AUTOR
    private ColumnLocal_nasc_autor pvColLocal_nasc_autor;

    public sealed class ColumnLocal_nasc_autor : TDataColumn {

      public ColumnLocal_nasc_autor() {
        this.ColumnName = "local_nasc_autor";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "local de nascimento";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_autor.local_nasc_autor"; } }
      public override string OriginalCol { get { return "local_nasc_autor"; } }
      public override string RefCol { get { return "LY_BIB_AUTOR.LOCAL_NASC_AUTOR"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 500; } }
      public override string SqlServerType { get { return "VARCHAR(500)"; } }

    }
    #endregion

    #region Coluna: NOME_AUTOR
    private ColumnNome_autor pvColNome_autor;

    public sealed class ColumnNome_autor : TDataColumn {

      public ColumnNome_autor() {
        this.ColumnName = "nome_autor";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "nome";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_autor.nome_autor"; } }
      public override string OriginalCol { get { return "nome_autor"; } }
      public override string RefCol { get { return "LY_BIB_AUTOR.NOME_AUTOR"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 500; } }
      public override string SqlServerType { get { return "VARCHAR(500)"; } }

    }
    #endregion
    #endregion
  }

  public partial class Ly_bib_biblioteca : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_bib_biblioteca() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_bib_biblioteca");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_bib_biblioteca");
      }
      catch{}
    }

    public Ly_bib_biblioteca() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColNome_bib = new ColumnNome_bib(); Columns.Add(pvColNome_bib);
      pvColResp_bib = new ColumnResp_bib(); Columns.Add(pvColResp_bib);
      pvColHorario_func = new ColumnHorario_func(); Columns.Add(pvColHorario_func);
      pvColTipo_bib = new ColumnTipo_bib(); Columns.Add(pvColTipo_bib);
      pvColFone_bib = new ColumnFone_bib(); Columns.Add(pvColFone_bib);
      pvColFax_bib = new ColumnFax_bib(); Columns.Add(pvColFax_bib);
      pvColCelular_bib = new ColumnCelular_bib(); Columns.Add(pvColCelular_bib);
      pvColEmail = new ColumnEmail(); Columns.Add(pvColEmail);
      pvColUnid_fisica = new ColumnUnid_fisica(); Columns.Add(pvColUnid_fisica);
      pvColDependencia = new ColumnDependencia(); Columns.Add(pvColDependencia);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_bib_biblioteca");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @nome_bib, decimal? @resp_bib, string @horario_func, string @tipo_bib, 
                      string @unid_fisica, string @dependencia) {
      Row row = NewRow();

      row.Nome_bib = @nome_bib;
      row.Resp_bib = @resp_bib;
      row.Horario_func = @horario_func;
      row.Tipo_bib = @tipo_bib;
      row.Unid_fisica = @unid_fisica;
      row.Dependencia = @dependencia;

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
      get { return "LY_BIB_BIBLIOTECA"; }
    }

    protected override sealed string Name { get { return "Biblioteca"; } }

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
        case CommandOperation.Select: cm.CommandText = "zLy_bib_biblioteca_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_bib_biblioteca_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_bib_biblioteca_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_bib_biblioteca_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("nome_bib", OleDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "NOME_BIB", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("resp_bib", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "RESP_BIB", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("horario_func", OleDbType.VarChar, 600, ParameterDirection.InputOutput, true, 0, 0, "HORARIO_FUNC", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("tipo_bib", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TIPO_BIB", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("fone_bib", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 25, 10, "FONE_BIB", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("fax_bib", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 25, 10, "FAX_BIB", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("celular_bib", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 25, 10, "CELULAR_BIB", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("email", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "EMAIL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("unid_fisica", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "UNID_FISICA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("dependencia", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "DEPENDENCIA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "nome_bib") { result.Add(toLower ? "nome_bib" : column); continue; }
          if(lower == "resp_bib") { result.Add(toLower ? "resp_bib" : column); continue; }
          if(lower == "horario_func") { result.Add(toLower ? "horario_func" : column); continue; }
          if(lower == "tipo_bib") { result.Add(toLower ? "tipo_bib" : column); continue; }
          if(lower == "fone_bib") { result.Add(toLower ? "fone_bib" : column); continue; }
          if(lower == "fax_bib") { result.Add(toLower ? "fax_bib" : column); continue; }
          if(lower == "celular_bib") { result.Add(toLower ? "celular_bib" : column); continue; }
          if(lower == "email") { result.Add(toLower ? "email" : column); continue; }
          if(lower == "unid_fisica") { result.Add(toLower ? "unid_fisica" : column); continue; }
          if(lower == "dependencia") { result.Add(toLower ? "dependencia" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "nome_bib", "resp_bib", "horario_func", "tipo_bib", "fone_bib", 
                                           "fax_bib", "celular_bib", "email", "unid_fisica", "dependencia" }
                          : new string[] { "ID", "NOME_BIB", "RESP_BIB", "HORARIO_FUNC", "TIPO_BIB", "FONE_BIB", 
                                           "FAX_BIB", "CELULAR_BIB", "EMAIL", "UNID_FISICA", "DEPENDENCIA" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "nome_bib", "resp_bib", "horario_func", "tipo_bib", "fone_bib", 
                                           "fax_bib", "celular_bib", "email", "unid_fisica", "dependencia" }
                          : new string[] { "ID", "NOME_BIB", "RESP_BIB", "HORARIO_FUNC", "TIPO_BIB", "FONE_BIB", 
                                           "FAX_BIB", "CELULAR_BIB", "EMAIL", "UNID_FISICA", "DEPENDENCIA" });

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
        "ly_bib_biblioteca.id"
      });
      if(scColumns.Contains("nome_bib")) list.Add("ly_bib_biblioteca.nome_bib");
      if(scColumns.Contains("resp_bib")) list.Add("ly_bib_biblioteca.resp_bib");
      if(scColumns.Contains("horario_func")) list.Add("ly_bib_biblioteca.horario_func");
      if(scColumns.Contains("tipo_bib")) list.Add("ly_bib_biblioteca.tipo_bib");
      if(scColumns.Contains("fone_bib")) list.Add("ly_bib_biblioteca.fone_bib");
      if(scColumns.Contains("fax_bib")) list.Add("ly_bib_biblioteca.fax_bib");
      if(scColumns.Contains("celular_bib")) list.Add("ly_bib_biblioteca.celular_bib");
      if(scColumns.Contains("email")) list.Add("ly_bib_biblioteca.email");
      if(scColumns.Contains("unid_fisica")) list.Add("ly_bib_biblioteca.unid_fisica");
      if(scColumns.Contains("dependencia")) list.Add("ly_bib_biblioteca.dependencia");
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
          "FROM ly_bib_biblioteca " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_bib_biblioteca");
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
        da.TableMappings.Add("Table", "Ly_bib_biblioteca");
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
            if (dr["NOME_BIB"] != null && string.IsNullOrEmpty(dr["NOME_BIB"].ToString()))
              dr["NOME_BIB"] = DBNull.Value;
            if (dr["RESP_BIB"] != null && string.IsNullOrEmpty(dr["RESP_BIB"].ToString()))
              dr["RESP_BIB"] = DBNull.Value;
            if (dr["HORARIO_FUNC"] != null && string.IsNullOrEmpty(dr["HORARIO_FUNC"].ToString()))
              dr["HORARIO_FUNC"] = DBNull.Value;
            if (dr["TIPO_BIB"] != null && string.IsNullOrEmpty(dr["TIPO_BIB"].ToString()))
              dr["TIPO_BIB"] = DBNull.Value;
            if (dr["FONE_BIB"] != null && string.IsNullOrEmpty(dr["FONE_BIB"].ToString()))
              dr["FONE_BIB"] = DBNull.Value;
            if (dr["FAX_BIB"] != null && string.IsNullOrEmpty(dr["FAX_BIB"].ToString()))
              dr["FAX_BIB"] = DBNull.Value;
            if (dr["CELULAR_BIB"] != null && string.IsNullOrEmpty(dr["CELULAR_BIB"].ToString()))
              dr["CELULAR_BIB"] = DBNull.Value;
            if (dr["EMAIL"] != null && string.IsNullOrEmpty(dr["EMAIL"].ToString()))
              dr["EMAIL"] = DBNull.Value;
            if (dr["UNID_FISICA"] != null && string.IsNullOrEmpty(dr["UNID_FISICA"].ToString()))
              dr["UNID_FISICA"] = DBNull.Value;
            if (dr["DEPENDENCIA"] != null && string.IsNullOrEmpty(dr["DEPENDENCIA"].ToString()))
              dr["DEPENDENCIA"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["NOME_BIB"] == null || string.IsNullOrEmpty(dr["NOME_BIB"].ToString()))
            {
              return "Favor informar o nome.";
            }

            if (dr["NOME_BIB"] != null && !string.IsNullOrEmpty(dr["NOME_BIB"].ToString()))
            {
              if (dr["NOME_BIB"].ToString().Length > 200)
              {
                return "O nome deve possuir no máximo 200 caracteres.";
              }
            }

            if (dr["RESP_BIB"] == null || string.IsNullOrEmpty(dr["RESP_BIB"].ToString()))
            {
              return "Favor informar o responsável.";
            }

            if (dr["RESP_BIB"] != null && !string.IsNullOrEmpty(dr["RESP_BIB"].ToString()))
            {
              if ((Convert.ToDecimal(dr["RESP_BIB"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["RESP_BIB"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["RESP_BIB"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["RESP_BIB"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O responsável deve ser um valor inteiro com 10 dígitos no máximo.";
              }
            }

            if (dr["HORARIO_FUNC"] == null || string.IsNullOrEmpty(dr["HORARIO_FUNC"].ToString()))
            {
              return "Favor informar o horário de funcionamento.";
            }

            if (dr["HORARIO_FUNC"] != null && !string.IsNullOrEmpty(dr["HORARIO_FUNC"].ToString()))
            {
              if (dr["HORARIO_FUNC"].ToString().Length > 500)
              {
                return "O horário de funcionamento deve possuir no máximo 500 caracteres.";
              }
            }

            if (dr["TIPO_BIB"] == null || string.IsNullOrEmpty(dr["TIPO_BIB"].ToString()))
            {
              return "Favor informar o tipo.";
            }

            if (dr["TIPO_BIB"] != null && !string.IsNullOrEmpty(dr["TIPO_BIB"].ToString()))
            {
              if (dr["TIPO_BIB"].ToString().Length > 20)
              {
                return "O tipo deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["FONE_BIB"] != null && !string.IsNullOrEmpty(dr["FONE_BIB"].ToString()))
            {
              if ((Convert.ToDecimal(dr["FONE_BIB"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["FONE_BIB"]).ToString("0.############").Split(',')[0].Length > 15) || (Convert.ToDecimal(dr["FONE_BIB"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["FONE_BIB"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O telefone deve ser um valor inteiro com 15 dígitos no máximo.";
              }
            }

            if (dr["FAX_BIB"] != null && !string.IsNullOrEmpty(dr["FAX_BIB"].ToString()))
            {
              if ((Convert.ToDecimal(dr["FAX_BIB"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["FAX_BIB"]).ToString("0.############").Split(',')[0].Length > 15) || (Convert.ToDecimal(dr["FAX_BIB"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["FAX_BIB"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O fax deve ser um valor inteiro com 15 dígitos no máximo.";
              }
            }

            if (dr["CELULAR_BIB"] != null && !string.IsNullOrEmpty(dr["CELULAR_BIB"].ToString()))
            {
              if ((Convert.ToDecimal(dr["CELULAR_BIB"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["CELULAR_BIB"]).ToString("0.############").Split(',')[0].Length > 15) || (Convert.ToDecimal(dr["CELULAR_BIB"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["CELULAR_BIB"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O celular deve ser um valor inteiro com 15 dígitos no máximo.";
              }
            }

            if (dr["EMAIL"] != null && !string.IsNullOrEmpty(dr["EMAIL"].ToString()))
            {
              if (dr["EMAIL"].ToString().Length > 50)
              {
                return "O e-mail deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["UNID_FISICA"] == null || string.IsNullOrEmpty(dr["UNID_FISICA"].ToString()))
            {
              return "Favor informar a unidade física.";
            }

            if (dr["UNID_FISICA"] != null && !string.IsNullOrEmpty(dr["UNID_FISICA"].ToString()))
            {
              if (dr["UNID_FISICA"].ToString().Length > 20)
              {
                return "A unidade física deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["DEPENDENCIA"] == null || string.IsNullOrEmpty(dr["DEPENDENCIA"].ToString()))
            {
              return "Favor informar a dependência.";
            }

            if (dr["DEPENDENCIA"] != null && !string.IsNullOrEmpty(dr["DEPENDENCIA"].ToString()))
            {
              if (dr["DEPENDENCIA"].ToString().Length > 20)
              {
                return "A dependência deve possuir no máximo 20 caracteres.";
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

    public static Ly_bib_biblioteca Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_bib_biblioteca Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_bib_biblioteca tab = new Ly_bib_biblioteca();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_bib_biblioteca tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, string @nome_bib, decimal? @resp_bib, string @horario_func, string @tipo_bib, 
        decimal? @fone_bib, decimal? @fax_bib, decimal? @celular_bib, string @email, string @unid_fisica, 
        string @dependencia
      ) {
        return (Row)Add(new object[] {
          @id, @nome_bib, @resp_bib, @horario_func, @tipo_bib, @fone_bib, @fax_bib, @celular_bib, 
          @email, @unid_fisica, @dependencia
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
      /// NOME_BIB: VARCHAR(200), Not Null
      /// </summary>
      public string Nome_bib { get { return (string)this["nome_bib"]; } set { this["nome_bib"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// RESP_BIB: NUMERIC(10), Not Null
      /// </summary>
      public decimal? Resp_bib { get { return (decimal?)this["resp_bib"]; } set { this["resp_bib"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// HORARIO_FUNC: VARCHAR(500), Not Null
      /// </summary>
      public string Horario_func { get { return (string)this["horario_func"]; } set { this["horario_func"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TIPO_BIB: VARCHAR(20), Not Null
      /// </summary>
      public string Tipo_bib { get { return (string)this["tipo_bib"]; } set { this["tipo_bib"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// FONE_BIB: NUMERIC(15)
      /// </summary>
      public decimal? Fone_bib { get { return (decimal?)this["fone_bib"]; } set { this["fone_bib"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// FAX_BIB: NUMERIC(15)
      /// </summary>
      public decimal? Fax_bib { get { return (decimal?)this["fax_bib"]; } set { this["fax_bib"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CELULAR_BIB: NUMERIC(15)
      /// </summary>
      public decimal? Celular_bib { get { return (decimal?)this["celular_bib"]; } set { this["celular_bib"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// EMAIL: VARCHAR(50)
      /// </summary>
      public string Email { get { return (string)this["email"]; } set { this["email"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// UNID_FISICA: VARCHAR(20), Not Null
      /// </summary>
      public string Unid_fisica { get { return (string)this["unid_fisica"]; } set { this["unid_fisica"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DEPENDENCIA: VARCHAR(20), Not Null
      /// </summary>
      public string Dependencia { get { return (string)this["dependencia"]; } set { this["dependencia"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @nome_bib, decimal? @resp_bib, string @horario_func, string @tipo_bib, string @unid_fisica, 
                               string @dependencia) {
        return Insert(connection,
                      @nome_bib, @resp_bib, @horario_func, @tipo_bib, @unid_fisica, @dependencia,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @nome_bib, decimal? @resp_bib, string @horario_func, string @tipo_bib, string @unid_fisica, 
                               string @dependencia,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_bib_biblioteca(),
          new string[] { "nome_bib", "resp_bib", "horario_func", "tipo_bib", "unid_fisica", 
                         "dependencia" },
          new object[] { @nome_bib, @resp_bib, @horario_func, @tipo_bib, @unid_fisica, @dependencia },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_bib_biblioteca(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_bib_biblioteca(),
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
          connection, new Techne.Lyceum.CR.Ly_bib_biblioteca(),
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
        public string Nome_bib { get { return (string)row["nome_bib", DataRowVersion.Original]; } }
        public decimal? Resp_bib { get { return (decimal?)row["resp_bib", DataRowVersion.Original]; } }
        public string Horario_func { get { return (string)row["horario_func", DataRowVersion.Original]; } }
        public string Tipo_bib { get { return (string)row["tipo_bib", DataRowVersion.Original]; } }
        public decimal? Fone_bib { get { return (decimal?)row["fone_bib", DataRowVersion.Original]; } }
        public decimal? Fax_bib { get { return (decimal?)row["fax_bib", DataRowVersion.Original]; } }
        public decimal? Celular_bib { get { return (decimal?)row["celular_bib", DataRowVersion.Original]; } }
        public string Email { get { return (string)row["email", DataRowVersion.Original]; } }
        public string Unid_fisica { get { return (string)row["unid_fisica", DataRowVersion.Original]; } }
        public string Dependencia { get { return (string)row["dependencia", DataRowVersion.Original]; } }

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


      protected override string GetName(){return "identificador";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_biblioteca.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_BIB_BIBLIOTECA.ID"; } }
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

    #region Coluna: NOME_BIB
    private ColumnNome_bib pvColNome_bib;

    public sealed class ColumnNome_bib : TDataColumn {

      public ColumnNome_bib() {
        this.ColumnName = "nome_bib";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "nome";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_biblioteca.nome_bib"; } }
      public override string OriginalCol { get { return "nome_bib"; } }
      public override string RefCol { get { return "LY_BIB_BIBLIOTECA.NOME_BIB"; } }
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

    #region Coluna: RESP_BIB
    private ColumnResp_bib pvColResp_bib;

    public sealed class ColumnResp_bib : TDataColumn {

      public ColumnResp_bib() {
        this.ColumnName = "resp_bib";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "responsável";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_biblioteca.resp_bib"; } }
      public override string OriginalCol { get { return "resp_bib"; } }
      public override string RefCol { get { return "LY_BIB_BIBLIOTECA.RESP_BIB"; } }
      public override bool PrimaryKey { get { return false; } }
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

    #region Coluna: HORARIO_FUNC
    private ColumnHorario_func pvColHorario_func;

    public sealed class ColumnHorario_func : TDataColumn {

      public ColumnHorario_func() {
        this.ColumnName = "horario_func";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "horário de funcionamento";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_biblioteca.horario_func"; } }
      public override string OriginalCol { get { return "horario_func"; } }
      public override string RefCol { get { return "LY_BIB_BIBLIOTECA.HORARIO_FUNC"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 500; } }
      public override string SqlServerType { get { return "VARCHAR(500)"; } }

    }
    #endregion

    #region Coluna: TIPO_BIB
    private ColumnTipo_bib pvColTipo_bib;

    public sealed class ColumnTipo_bib : TDataColumn {

      public ColumnTipo_bib() {
        this.ColumnName = "tipo_bib";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "tipo";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_biblioteca.tipo_bib"; } }
      public override string OriginalCol { get { return "tipo_bib"; } }
      public override string RefCol { get { return "LY_BIB_BIBLIOTECA.TIPO_BIB"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 20; } }
      public override string SqlServerType { get { return "VARCHAR(20)"; } }

    }
    #endregion

    #region Coluna: FONE_BIB
    private ColumnFone_bib pvColFone_bib;

    public sealed class ColumnFone_bib : TDataColumn {

      public ColumnFone_bib() {
        this.ColumnName = "fone_bib";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "telefone";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_biblioteca.fone_bib"; } }
      public override string OriginalCol { get { return "fone_bib"; } }
      public override string RefCol { get { return "LY_BIB_BIBLIOTECA.FONE_BIB"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 16; } }
      public override int Precision { get { return 15; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(15)"; } }

    }
    #endregion

    #region Coluna: FAX_BIB
    private ColumnFax_bib pvColFax_bib;

    public sealed class ColumnFax_bib : TDataColumn {

      public ColumnFax_bib() {
        this.ColumnName = "fax_bib";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "fax";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_biblioteca.fax_bib"; } }
      public override string OriginalCol { get { return "fax_bib"; } }
      public override string RefCol { get { return "LY_BIB_BIBLIOTECA.FAX_BIB"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 16; } }
      public override int Precision { get { return 15; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(15)"; } }

    }
    #endregion

    #region Coluna: CELULAR_BIB
    private ColumnCelular_bib pvColCelular_bib;

    public sealed class ColumnCelular_bib : TDataColumn {

      public ColumnCelular_bib() {
        this.ColumnName = "celular_bib";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "celular";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_biblioteca.celular_bib"; } }
      public override string OriginalCol { get { return "celular_bib"; } }
      public override string RefCol { get { return "LY_BIB_BIBLIOTECA.CELULAR_BIB"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 16; } }
      public override int Precision { get { return 15; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(15)"; } }

    }
    #endregion

    #region Coluna: EMAIL
    private ColumnEmail pvColEmail;

    public sealed class ColumnEmail : TDataColumn {

      public ColumnEmail() {
        this.ColumnName = "email";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "e-mail";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_biblioteca.email"; } }
      public override string OriginalCol { get { return "email"; } }
      public override string RefCol { get { return "LY_BIB_BIBLIOTECA.EMAIL"; } }
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

    #region Coluna: UNID_FISICA
    private ColumnUnid_fisica pvColUnid_fisica;

    public sealed class ColumnUnid_fisica : TDataColumn {

      public ColumnUnid_fisica() {
        this.ColumnName = "unid_fisica";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "unidade física";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_biblioteca.unid_fisica"; } }
      public override string OriginalCol { get { return "unid_fisica"; } }
      public override string RefCol { get { return "LY_BIB_BIBLIOTECA.UNID_FISICA"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 20; } }
      public override string SqlServerType { get { return "VARCHAR(20)"; } }

    }
    #endregion

    #region Coluna: DEPENDENCIA
    private ColumnDependencia pvColDependencia;

    public sealed class ColumnDependencia : TDataColumn {

      public ColumnDependencia() {
        this.ColumnName = "dependencia";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "dependência";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_biblioteca.dependencia"; } }
      public override string OriginalCol { get { return "dependencia"; } }
      public override string RefCol { get { return "LY_BIB_BIBLIOTECA.DEPENDENCIA"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 20; } }
      public override string SqlServerType { get { return "VARCHAR(20)"; } }

    }
    #endregion
    #endregion
  }

  public partial class Ly_bib_configuracoes : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_bib_configuracoes() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_bib_configuracoes");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_bib_configuracoes");
      }
      catch{}
    }

    public Ly_bib_configuracoes() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColId_bib_biblioteca = new ColumnId_bib_biblioteca(); Columns.Add(pvColId_bib_biblioteca);
      pvColId_bib_tipo_material = new ColumnId_bib_tipo_material(); Columns.Add(pvColId_bib_tipo_material);
      pvColPrazo = new ColumnPrazo(); Columns.Add(pvColPrazo);
      pvColValor_dia_multa = new ColumnValor_dia_multa(); Columns.Add(pvColValor_dia_multa);
      pvColValor_fixo_multa = new ColumnValor_fixo_multa(); Columns.Add(pvColValor_fixo_multa);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_bib_configuracoes");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(decimal? @id_bib_biblioteca, decimal? @id_bib_tipo_material, decimal? @prazo) {
      Row row = NewRow();

      row.Id_bib_biblioteca = @id_bib_biblioteca;
      row.Id_bib_tipo_material = @id_bib_tipo_material;
      row.Prazo = @prazo;

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
      get { return "LY_BIB_CONFIGURACOES"; }
    }

    protected override sealed string Name { get { return "Configurações"; } }

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
        case CommandOperation.Select: cm.CommandText = "zLy_bib_configuracoes_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_bib_configuracoes_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_bib_configuracoes_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_bib_configuracoes_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("id_bib_biblioteca", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_BIB_BIBLIOTECA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("id_bib_tipo_material", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_BIB_TIPO_MATERIAL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("prazo", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "PRAZO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("valor_dia_multa", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "VALOR_DIA_MULTA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("valor_fixo_multa", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "VALOR_FIXO_MULTA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "id_bib_biblioteca") { result.Add(toLower ? "id_bib_biblioteca" : column); continue; }
          if(lower == "id_bib_tipo_material") { result.Add(toLower ? "id_bib_tipo_material" : column); continue; }
          if(lower == "prazo") { result.Add(toLower ? "prazo" : column); continue; }
          if(lower == "valor_dia_multa") { result.Add(toLower ? "valor_dia_multa" : column); continue; }
          if(lower == "valor_fixo_multa") { result.Add(toLower ? "valor_fixo_multa" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "id_bib_biblioteca", "id_bib_tipo_material", "prazo", "valor_dia_multa", 
                                           "valor_fixo_multa" }
                          : new string[] { "ID", "ID_BIB_BIBLIOTECA", "ID_BIB_TIPO_MATERIAL", "PRAZO", "VALOR_DIA_MULTA", 
                                           "VALOR_FIXO_MULTA" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "id_bib_biblioteca", "id_bib_tipo_material", "prazo", "valor_dia_multa", 
                                           "valor_fixo_multa" }
                          : new string[] { "ID", "ID_BIB_BIBLIOTECA", "ID_BIB_TIPO_MATERIAL", "PRAZO", "VALOR_DIA_MULTA", 
                                           "VALOR_FIXO_MULTA" });

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
        "ly_bib_configuracoes.id"
      });
      if(scColumns.Contains("id_bib_biblioteca")) list.Add("ly_bib_configuracoes.id_bib_biblioteca");
      if(scColumns.Contains("id_bib_tipo_material")) list.Add("ly_bib_configuracoes.id_bib_tipo_material");
      if(scColumns.Contains("prazo")) list.Add("ly_bib_configuracoes.prazo");
      if(scColumns.Contains("valor_dia_multa")) list.Add("ly_bib_configuracoes.valor_dia_multa");
      if(scColumns.Contains("valor_fixo_multa")) list.Add("ly_bib_configuracoes.valor_fixo_multa");
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
          "FROM ly_bib_configuracoes " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_bib_configuracoes");
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
        da.TableMappings.Add("Table", "Ly_bib_configuracoes");
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
            if (dr["ID_BIB_BIBLIOTECA"] != null && string.IsNullOrEmpty(dr["ID_BIB_BIBLIOTECA"].ToString()))
              dr["ID_BIB_BIBLIOTECA"] = DBNull.Value;
            if (dr["ID_BIB_TIPO_MATERIAL"] != null && string.IsNullOrEmpty(dr["ID_BIB_TIPO_MATERIAL"].ToString()))
              dr["ID_BIB_TIPO_MATERIAL"] = DBNull.Value;
            if (dr["PRAZO"] != null && string.IsNullOrEmpty(dr["PRAZO"].ToString()))
              dr["PRAZO"] = DBNull.Value;
            if (dr["VALOR_DIA_MULTA"] != null && string.IsNullOrEmpty(dr["VALOR_DIA_MULTA"].ToString()))
              dr["VALOR_DIA_MULTA"] = DBNull.Value;
            if (dr["VALOR_FIXO_MULTA"] != null && string.IsNullOrEmpty(dr["VALOR_FIXO_MULTA"].ToString()))
              dr["VALOR_FIXO_MULTA"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["ID_BIB_BIBLIOTECA"] == null || string.IsNullOrEmpty(dr["ID_BIB_BIBLIOTECA"].ToString()))
            {
              return "Favor informar a biblioteca.";
            }

            if (dr["ID_BIB_BIBLIOTECA"] != null && !string.IsNullOrEmpty(dr["ID_BIB_BIBLIOTECA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_BIB_BIBLIOTECA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_BIBLIOTECA"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_BIB_BIBLIOTECA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_BIBLIOTECA"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A biblioteca deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["ID_BIB_TIPO_MATERIAL"] == null || string.IsNullOrEmpty(dr["ID_BIB_TIPO_MATERIAL"].ToString()))
            {
              return "Favor informar o tipo de material.";
            }

            if (dr["ID_BIB_TIPO_MATERIAL"] != null && !string.IsNullOrEmpty(dr["ID_BIB_TIPO_MATERIAL"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_BIB_TIPO_MATERIAL"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_TIPO_MATERIAL"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_BIB_TIPO_MATERIAL"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_TIPO_MATERIAL"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O tipo de material deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["PRAZO"] == null || string.IsNullOrEmpty(dr["PRAZO"].ToString()))
            {
              return "Favor informar o prazo.";
            }

            if (dr["PRAZO"] != null && !string.IsNullOrEmpty(dr["PRAZO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["PRAZO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PRAZO"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["PRAZO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PRAZO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O prazo deve ser um valor inteiro com 10 dígitos no máximo.";
              }
            }

            if (dr["VALOR_DIA_MULTA"] != null && !string.IsNullOrEmpty(dr["VALOR_DIA_MULTA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["VALOR_DIA_MULTA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["VALOR_DIA_MULTA"]).ToString("0.############").Split(',')[0].Length > 2) || (Convert.ToDecimal(dr["VALOR_DIA_MULTA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["VALOR_DIA_MULTA"]).ToString("0.############").Split(',')[1].Length > 2))
              {
                return "O valor diário da multa deve ser um valor numérico com, no máximo, 2 dígitos na parte inteira e 2 casas decimal.";
              }
            }

            if (dr["VALOR_FIXO_MULTA"] != null && !string.IsNullOrEmpty(dr["VALOR_FIXO_MULTA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["VALOR_FIXO_MULTA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["VALOR_FIXO_MULTA"]).ToString("0.############").Split(',')[0].Length > 2) || (Convert.ToDecimal(dr["VALOR_FIXO_MULTA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["VALOR_FIXO_MULTA"]).ToString("0.############").Split(',')[1].Length > 2))
              {
                return "O valor fixo da multa deve ser um valor numérico com, no máximo, 2 dígitos na parte inteira e 2 casas decimal.";
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

    public static Ly_bib_configuracoes Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_bib_configuracoes Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_bib_configuracoes tab = new Ly_bib_configuracoes();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_bib_configuracoes tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, decimal? @id_bib_biblioteca, decimal? @id_bib_tipo_material, decimal? @prazo, 
        decimal? @valor_dia_multa, decimal? @valor_fixo_multa
      ) {
        return (Row)Add(new object[] {
          @id, @id_bib_biblioteca, @id_bib_tipo_material, @prazo, @valor_dia_multa, @valor_fixo_multa
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
      /// ID_BIB_BIBLIOTECA: NUMERIC(14), Not Null
      /// </summary>
      public decimal? Id_bib_biblioteca { get { return (decimal?)this["id_bib_biblioteca"]; } set { this["id_bib_biblioteca"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ID_BIB_TIPO_MATERIAL: NUMERIC(14), Not Null
      /// </summary>
      public decimal? Id_bib_tipo_material { get { return (decimal?)this["id_bib_tipo_material"]; } set { this["id_bib_tipo_material"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PRAZO: NUMERIC(10), Not Null
      /// </summary>
      public decimal? Prazo { get { return (decimal?)this["prazo"]; } set { this["prazo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// VALOR_DIA_MULTA: NUMERIC(4, 2)
      /// </summary>
      public decimal? Valor_dia_multa { get { return (decimal?)this["valor_dia_multa"]; } set { this["valor_dia_multa"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// VALOR_FIXO_MULTA: NUMERIC(4, 2)
      /// </summary>
      public decimal? Valor_fixo_multa { get { return (decimal?)this["valor_fixo_multa"]; } set { this["valor_fixo_multa"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               decimal? @id_bib_biblioteca, decimal? @id_bib_tipo_material, decimal? @prazo) {
        return Insert(connection,
                      @id_bib_biblioteca, @id_bib_tipo_material, @prazo,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               decimal? @id_bib_biblioteca, decimal? @id_bib_tipo_material, decimal? @prazo,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_bib_configuracoes(),
          new string[] { "id_bib_biblioteca", "id_bib_tipo_material", "prazo" },
          new object[] { @id_bib_biblioteca, @id_bib_tipo_material, @prazo },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_bib_configuracoes(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_bib_configuracoes(),
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
          connection, new Techne.Lyceum.CR.Ly_bib_configuracoes(),
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
        public decimal? Id_bib_biblioteca { get { return (decimal?)row["id_bib_biblioteca", DataRowVersion.Original]; } }
        public decimal? Id_bib_tipo_material { get { return (decimal?)row["id_bib_tipo_material", DataRowVersion.Original]; } }
        public decimal? Prazo { get { return (decimal?)row["prazo", DataRowVersion.Original]; } }
        public decimal? Valor_dia_multa { get { return (decimal?)row["valor_dia_multa", DataRowVersion.Original]; } }
        public decimal? Valor_fixo_multa { get { return (decimal?)row["valor_fixo_multa", DataRowVersion.Original]; } }

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


      protected override string GetName(){return "identificador";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_configuracoes.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_BIB_CONFIGURACOES.ID"; } }
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

    #region Coluna: ID_BIB_BIBLIOTECA
    private ColumnId_bib_biblioteca pvColId_bib_biblioteca;

    public sealed class ColumnId_bib_biblioteca : TDataColumn {

      public ColumnId_bib_biblioteca() {
        this.ColumnName = "id_bib_biblioteca";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "biblioteca";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_configuracoes.id_bib_biblioteca"; } }
      public override string OriginalCol { get { return "id_bib_biblioteca"; } }
      public override string RefCol { get { return "LY_BIB_CONFIGURACOES.ID_BIB_BIBLIOTECA"; } }
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

    #region Coluna: ID_BIB_TIPO_MATERIAL
    private ColumnId_bib_tipo_material pvColId_bib_tipo_material;

    public sealed class ColumnId_bib_tipo_material : TDataColumn {

      public ColumnId_bib_tipo_material() {
        this.ColumnName = "id_bib_tipo_material";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "tipo de material";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_configuracoes.id_bib_tipo_material"; } }
      public override string OriginalCol { get { return "id_bib_tipo_material"; } }
      public override string RefCol { get { return "LY_BIB_CONFIGURACOES.ID_BIB_TIPO_MATERIAL"; } }
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

    #region Coluna: PRAZO
    private ColumnPrazo pvColPrazo;

    public sealed class ColumnPrazo : TDataColumn {

      public ColumnPrazo() {
        this.ColumnName = "prazo";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "prazo";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_configuracoes.prazo"; } }
      public override string OriginalCol { get { return "prazo"; } }
      public override string RefCol { get { return "LY_BIB_CONFIGURACOES.PRAZO"; } }
      public override bool PrimaryKey { get { return false; } }
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

    #region Coluna: VALOR_DIA_MULTA
    private ColumnValor_dia_multa pvColValor_dia_multa;

    public sealed class ColumnValor_dia_multa : TDataColumn {

      public ColumnValor_dia_multa() {
        this.ColumnName = "valor_dia_multa";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "valor diário da multa";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_configuracoes.valor_dia_multa"; } }
      public override string OriginalCol { get { return "valor_dia_multa"; } }
      public override string RefCol { get { return "LY_BIB_CONFIGURACOES.VALOR_DIA_MULTA"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 6; } }
      public override int Precision { get { return 4; } }
      public override int Scale { get { return 2; } }
      public override string SqlServerType { get { return "NUMERIC(4, 2)"; } }

    }
    #endregion

    #region Coluna: VALOR_FIXO_MULTA
    private ColumnValor_fixo_multa pvColValor_fixo_multa;

    public sealed class ColumnValor_fixo_multa : TDataColumn {

      public ColumnValor_fixo_multa() {
        this.ColumnName = "valor_fixo_multa";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "valor fixo da multa";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_configuracoes.valor_fixo_multa"; } }
      public override string OriginalCol { get { return "valor_fixo_multa"; } }
      public override string RefCol { get { return "LY_BIB_CONFIGURACOES.VALOR_FIXO_MULTA"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 6; } }
      public override int Precision { get { return 4; } }
      public override int Scale { get { return 2; } }
      public override string SqlServerType { get { return "NUMERIC(4, 2)"; } }

    }
    #endregion
    #endregion
  }

  public partial class Ly_bib_editora : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_bib_editora() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_bib_editora");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_bib_editora");
      }
      catch{}
    }

    public Ly_bib_editora() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColSigla_editora = new ColumnSigla_editora(); Columns.Add(pvColSigla_editora);
      pvColNome_editora = new ColumnNome_editora(); Columns.Add(pvColNome_editora);
      pvColObs_editora = new ColumnObs_editora(); Columns.Add(pvColObs_editora);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_bib_editora");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @sigla_editora, string @nome_editora) {
      Row row = NewRow();

      row.Sigla_editora = @sigla_editora;
      row.Nome_editora = @nome_editora;

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
      get { return "LY_BIB_EDITORA"; }
    }

    protected override sealed string Name { get { return "Editora"; } }

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
        case CommandOperation.Select: cm.CommandText = "zLy_bib_editora_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_bib_editora_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_bib_editora_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_bib_editora_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("sigla_editora", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SIGLA_EDITORA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("nome_editora", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NOME_EDITORA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("obs_editora", OleDbType.VarChar, 600, ParameterDirection.InputOutput, true, 0, 0, "OBS_EDITORA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "sigla_editora") { result.Add(toLower ? "sigla_editora" : column); continue; }
          if(lower == "nome_editora") { result.Add(toLower ? "nome_editora" : column); continue; }
          if(lower == "obs_editora") { result.Add(toLower ? "obs_editora" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "sigla_editora", "nome_editora", "obs_editora" }
                          : new string[] { "ID", "SIGLA_EDITORA", "NOME_EDITORA", "OBS_EDITORA" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "sigla_editora", "nome_editora", "obs_editora" }
                          : new string[] { "ID", "SIGLA_EDITORA", "NOME_EDITORA", "OBS_EDITORA" });

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
        "ly_bib_editora.id"
      });
      if(scColumns.Contains("sigla_editora")) list.Add("ly_bib_editora.sigla_editora");
      if(scColumns.Contains("nome_editora")) list.Add("ly_bib_editora.nome_editora");
      if(scColumns.Contains("obs_editora")) list.Add("ly_bib_editora.obs_editora");
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
          "FROM ly_bib_editora " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_bib_editora");
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
        da.TableMappings.Add("Table", "Ly_bib_editora");
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
            if (dr["SIGLA_EDITORA"] != null && string.IsNullOrEmpty(dr["SIGLA_EDITORA"].ToString()))
              dr["SIGLA_EDITORA"] = DBNull.Value;
            if (dr["NOME_EDITORA"] != null && string.IsNullOrEmpty(dr["NOME_EDITORA"].ToString()))
              dr["NOME_EDITORA"] = DBNull.Value;
            if (dr["OBS_EDITORA"] != null && string.IsNullOrEmpty(dr["OBS_EDITORA"].ToString()))
              dr["OBS_EDITORA"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["SIGLA_EDITORA"] == null || string.IsNullOrEmpty(dr["SIGLA_EDITORA"].ToString()))
            {
              return "Favor informar a sigla.";
            }

            if (dr["SIGLA_EDITORA"] != null && !string.IsNullOrEmpty(dr["SIGLA_EDITORA"].ToString()))
            {
              if (dr["SIGLA_EDITORA"].ToString().Length > 50)
              {
                return "A sigla deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["NOME_EDITORA"] == null || string.IsNullOrEmpty(dr["NOME_EDITORA"].ToString()))
            {
              return "Favor informar o nome.";
            }

            if (dr["NOME_EDITORA"] != null && !string.IsNullOrEmpty(dr["NOME_EDITORA"].ToString()))
            {
              if (dr["NOME_EDITORA"].ToString().Length > 100)
              {
                return "O nome deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["OBS_EDITORA"] != null && !string.IsNullOrEmpty(dr["OBS_EDITORA"].ToString()))
            {
              if (dr["OBS_EDITORA"].ToString().Length > 500)
              {
                return "A observação deve possuir no máximo 500 caracteres.";
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

    public static Ly_bib_editora Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_bib_editora Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_bib_editora tab = new Ly_bib_editora();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_bib_editora tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, string @sigla_editora, string @nome_editora, string @obs_editora
      ) {
        return (Row)Add(new object[] {
          @id, @sigla_editora, @nome_editora, @obs_editora
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
      /// SIGLA_EDITORA: VARCHAR(50), Not Null
      /// </summary>
      public string Sigla_editora { get { return (string)this["sigla_editora"]; } set { this["sigla_editora"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME_EDITORA: VARCHAR(100), Not Null
      /// </summary>
      public string Nome_editora { get { return (string)this["nome_editora"]; } set { this["nome_editora"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// OBS_EDITORA: VARCHAR(500)
      /// </summary>
      public string Obs_editora { get { return (string)this["obs_editora"]; } set { this["obs_editora"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @sigla_editora, string @nome_editora) {
        return Insert(connection,
                      @sigla_editora, @nome_editora,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @sigla_editora, string @nome_editora,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_bib_editora(),
          new string[] { "sigla_editora", "nome_editora" },
          new object[] { @sigla_editora, @nome_editora },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_bib_editora(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_bib_editora(),
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
          connection, new Techne.Lyceum.CR.Ly_bib_editora(),
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
        public string Sigla_editora { get { return (string)row["sigla_editora", DataRowVersion.Original]; } }
        public string Nome_editora { get { return (string)row["nome_editora", DataRowVersion.Original]; } }
        public string Obs_editora { get { return (string)row["obs_editora", DataRowVersion.Original]; } }

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


      protected override string GetName(){return "identificador";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_editora.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_BIB_EDITORA.ID"; } }
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

    #region Coluna: SIGLA_EDITORA
    private ColumnSigla_editora pvColSigla_editora;

    public sealed class ColumnSigla_editora : TDataColumn {

      public ColumnSigla_editora() {
        this.ColumnName = "sigla_editora";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "sigla";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_editora.sigla_editora"; } }
      public override string OriginalCol { get { return "sigla_editora"; } }
      public override string RefCol { get { return "LY_BIB_EDITORA.SIGLA_EDITORA"; } }
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

    #region Coluna: NOME_EDITORA
    private ColumnNome_editora pvColNome_editora;

    public sealed class ColumnNome_editora : TDataColumn {

      public ColumnNome_editora() {
        this.ColumnName = "nome_editora";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "nome";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_editora.nome_editora"; } }
      public override string OriginalCol { get { return "nome_editora"; } }
      public override string RefCol { get { return "LY_BIB_EDITORA.NOME_EDITORA"; } }
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

    #region Coluna: OBS_EDITORA
    private ColumnObs_editora pvColObs_editora;

    public sealed class ColumnObs_editora : TDataColumn {

      public ColumnObs_editora() {
        this.ColumnName = "obs_editora";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "observação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_editora.obs_editora"; } }
      public override string OriginalCol { get { return "obs_editora"; } }
      public override string RefCol { get { return "LY_BIB_EDITORA.OBS_EDITORA"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 500; } }
      public override string SqlServerType { get { return "VARCHAR(500)"; } }

    }
    #endregion
    #endregion
  }

  public partial class Ly_bib_emprestimo : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_bib_emprestimo() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_bib_emprestimo");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_bib_emprestimo");
      }
      catch{}
    }

    public Ly_bib_emprestimo() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColId_bib_usuario = new ColumnId_bib_usuario(); Columns.Add(pvColId_bib_usuario);
      pvColId_bib_material = new ColumnId_bib_material(); Columns.Add(pvColId_bib_material);
      pvColDt_emprestimo = new ColumnDt_emprestimo(); Columns.Add(pvColDt_emprestimo);
      pvColDt_prev_devolucao = new ColumnDt_prev_devolucao(); Columns.Add(pvColDt_prev_devolucao);
      pvColDt_devolucao = new ColumnDt_devolucao(); Columns.Add(pvColDt_devolucao);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_bib_emprestimo");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(decimal? @id_bib_usuario, decimal? @id_bib_material, DateTime? @dt_emprestimo, 
                      DateTime? @dt_prev_devolucao) {
      Row row = NewRow();

      row.Id_bib_usuario = @id_bib_usuario;
      row.Id_bib_material = @id_bib_material;
      row.Dt_emprestimo = @dt_emprestimo;
      row.Dt_prev_devolucao = @dt_prev_devolucao;

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
      get { return "LY_BIB_EMPRESTIMO"; }
    }

    protected override sealed string Name { get { return "Empréstimo"; } }

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
        case CommandOperation.Select: cm.CommandText = "zLy_bib_emprestimo_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_bib_emprestimo_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_bib_emprestimo_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_bib_emprestimo_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("id_bib_usuario", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_BIB_USUARIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("id_bib_material", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_BIB_MATERIAL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("dt_emprestimo", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_EMPRESTIMO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_prev_devolucao", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_PREV_DEVOLUCAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_devolucao", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_DEVOLUCAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));

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
          if(lower == "id_bib_usuario") { result.Add(toLower ? "id_bib_usuario" : column); continue; }
          if(lower == "id_bib_material") { result.Add(toLower ? "id_bib_material" : column); continue; }
          if(lower == "dt_emprestimo") { result.Add(toLower ? "dt_emprestimo" : column); continue; }
          if(lower == "dt_prev_devolucao") { result.Add(toLower ? "dt_prev_devolucao" : column); continue; }
          if(lower == "dt_devolucao") { result.Add(toLower ? "dt_devolucao" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "id_bib_usuario", "id_bib_material", "dt_emprestimo", "dt_prev_devolucao", 
                                           "dt_devolucao" }
                          : new string[] { "ID", "ID_BIB_USUARIO", "ID_BIB_MATERIAL", "DT_EMPRESTIMO", "DT_PREV_DEVOLUCAO", 
                                           "DT_DEVOLUCAO" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "id_bib_usuario", "id_bib_material", "dt_emprestimo", "dt_prev_devolucao", 
                                           "dt_devolucao" }
                          : new string[] { "ID", "ID_BIB_USUARIO", "ID_BIB_MATERIAL", "DT_EMPRESTIMO", "DT_PREV_DEVOLUCAO", 
                                           "DT_DEVOLUCAO" });

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
        "ly_bib_emprestimo.id"
      });
      if(scColumns.Contains("id_bib_usuario")) list.Add("ly_bib_emprestimo.id_bib_usuario");
      if(scColumns.Contains("id_bib_material")) list.Add("ly_bib_emprestimo.id_bib_material");
      if(scColumns.Contains("dt_emprestimo")) list.Add("ly_bib_emprestimo.dt_emprestimo");
      if(scColumns.Contains("dt_prev_devolucao")) list.Add("ly_bib_emprestimo.dt_prev_devolucao");
      if(scColumns.Contains("dt_devolucao")) list.Add("ly_bib_emprestimo.dt_devolucao");
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
          "FROM ly_bib_emprestimo " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_bib_emprestimo");
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
        da.TableMappings.Add("Table", "Ly_bib_emprestimo");
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
            if (dr["ID_BIB_USUARIO"] != null && string.IsNullOrEmpty(dr["ID_BIB_USUARIO"].ToString()))
              dr["ID_BIB_USUARIO"] = DBNull.Value;
            if (dr["ID_BIB_MATERIAL"] != null && string.IsNullOrEmpty(dr["ID_BIB_MATERIAL"].ToString()))
              dr["ID_BIB_MATERIAL"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["ID_BIB_USUARIO"] == null || string.IsNullOrEmpty(dr["ID_BIB_USUARIO"].ToString()))
            {
              return "Favor informar o usuário.";
            }

            if (dr["ID_BIB_USUARIO"] != null && !string.IsNullOrEmpty(dr["ID_BIB_USUARIO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_BIB_USUARIO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_USUARIO"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_BIB_USUARIO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_USUARIO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O usuário deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["ID_BIB_MATERIAL"] == null || string.IsNullOrEmpty(dr["ID_BIB_MATERIAL"].ToString()))
            {
              return "Favor informar o material.";
            }

            if (dr["ID_BIB_MATERIAL"] != null && !string.IsNullOrEmpty(dr["ID_BIB_MATERIAL"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_BIB_MATERIAL"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_MATERIAL"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_BIB_MATERIAL"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_MATERIAL"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O material deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["DT_EMPRESTIMO"] == null || string.IsNullOrEmpty(dr["DT_EMPRESTIMO"].ToString()))
            {
              return "Favor informar a data do empréstimo.";
            }

            if (dr["DT_EMPRESTIMO"] != null && dr["DT_EMPRESTIMO"].ToString() != "")
            {
              dr["DT_EMPRESTIMO"] = Convert.ToDateTime(dr["DT_EMPRESTIMO"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_PREV_DEVOLUCAO"] == null || string.IsNullOrEmpty(dr["DT_PREV_DEVOLUCAO"].ToString()))
            {
              return "Favor informar a data da previsão da devolução.";
            }

            if (dr["DT_PREV_DEVOLUCAO"] != null && dr["DT_PREV_DEVOLUCAO"].ToString() != "")
            {
              dr["DT_PREV_DEVOLUCAO"] = Convert.ToDateTime(dr["DT_PREV_DEVOLUCAO"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_DEVOLUCAO"] != null && dr["DT_DEVOLUCAO"].ToString() != "")
            {
              dr["DT_DEVOLUCAO"] = Convert.ToDateTime(dr["DT_DEVOLUCAO"]).ToString("dd/MM/yyyy HH:mm:ss");
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

    public static Ly_bib_emprestimo Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_bib_emprestimo Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_bib_emprestimo tab = new Ly_bib_emprestimo();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_bib_emprestimo tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, decimal? @id_bib_usuario, decimal? @id_bib_material, DateTime? @dt_emprestimo, 
        DateTime? @dt_prev_devolucao, DateTime? @dt_devolucao
      ) {
        return (Row)Add(new object[] {
          @id, @id_bib_usuario, @id_bib_material, @dt_emprestimo, @dt_prev_devolucao, @dt_devolucao
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
      /// ID_BIB_USUARIO: NUMERIC(14), Not Null
      /// </summary>
      public decimal? Id_bib_usuario { get { return (decimal?)this["id_bib_usuario"]; } set { this["id_bib_usuario"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ID_BIB_MATERIAL: NUMERIC(14), Not Null
      /// </summary>
      public decimal? Id_bib_material { get { return (decimal?)this["id_bib_material"]; } set { this["id_bib_material"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_EMPRESTIMO: DATETIME, Not Null
      /// </summary>
      public DateTime? Dt_emprestimo { get { return (DateTime?)this["dt_emprestimo"]; } set { this["dt_emprestimo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_PREV_DEVOLUCAO: DATETIME, Not Null
      /// </summary>
      public DateTime? Dt_prev_devolucao { get { return (DateTime?)this["dt_prev_devolucao"]; } set { this["dt_prev_devolucao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_DEVOLUCAO: DATETIME
      /// </summary>
      public DateTime? Dt_devolucao { get { return (DateTime?)this["dt_devolucao"]; } set { this["dt_devolucao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               decimal? @id_bib_usuario, decimal? @id_bib_material, DateTime? @dt_emprestimo, DateTime? @dt_prev_devolucao) {
        return Insert(connection,
                      @id_bib_usuario, @id_bib_material, @dt_emprestimo, @dt_prev_devolucao,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               decimal? @id_bib_usuario, decimal? @id_bib_material, DateTime? @dt_emprestimo, DateTime? @dt_prev_devolucao,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_bib_emprestimo(),
          new string[] { "id_bib_usuario", "id_bib_material", "dt_emprestimo", "dt_prev_devolucao" },
          new object[] { @id_bib_usuario, @id_bib_material, @dt_emprestimo, @dt_prev_devolucao },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_bib_emprestimo(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_bib_emprestimo(),
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
          connection, new Techne.Lyceum.CR.Ly_bib_emprestimo(),
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
        public decimal? Id_bib_usuario { get { return (decimal?)row["id_bib_usuario", DataRowVersion.Original]; } }
        public decimal? Id_bib_material { get { return (decimal?)row["id_bib_material", DataRowVersion.Original]; } }
        public DateTime? Dt_emprestimo { get { return (DateTime?)row["dt_emprestimo", DataRowVersion.Original]; } }
        public DateTime? Dt_prev_devolucao { get { return (DateTime?)row["dt_prev_devolucao", DataRowVersion.Original]; } }
        public DateTime? Dt_devolucao { get { return (DateTime?)row["dt_devolucao", DataRowVersion.Original]; } }

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


      protected override string GetName(){return "identificador";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_emprestimo.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_BIB_EMPRESTIMO.ID"; } }
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

    #region Coluna: ID_BIB_USUARIO
    private ColumnId_bib_usuario pvColId_bib_usuario;

    public sealed class ColumnId_bib_usuario : TDataColumn {

      public ColumnId_bib_usuario() {
        this.ColumnName = "id_bib_usuario";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "usuário";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_emprestimo.id_bib_usuario"; } }
      public override string OriginalCol { get { return "id_bib_usuario"; } }
      public override string RefCol { get { return "LY_BIB_EMPRESTIMO.ID_BIB_USUARIO"; } }
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

    #region Coluna: ID_BIB_MATERIAL
    private ColumnId_bib_material pvColId_bib_material;

    public sealed class ColumnId_bib_material : TDataColumn {

      public ColumnId_bib_material() {
        this.ColumnName = "id_bib_material";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "material";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_emprestimo.id_bib_material"; } }
      public override string OriginalCol { get { return "id_bib_material"; } }
      public override string RefCol { get { return "LY_BIB_EMPRESTIMO.ID_BIB_MATERIAL"; } }
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

    #region Coluna: DT_EMPRESTIMO
    private ColumnDt_emprestimo pvColDt_emprestimo;

    public sealed class ColumnDt_emprestimo : TDataColumn {

      public ColumnDt_emprestimo() {
        this.ColumnName = "dt_emprestimo";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data do empréstimo";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_emprestimo.dt_emprestimo"; } }
      public override string OriginalCol { get { return "dt_emprestimo"; } }
      public override string RefCol { get { return "LY_BIB_EMPRESTIMO.DT_EMPRESTIMO"; } }
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

    #region Coluna: DT_PREV_DEVOLUCAO
    private ColumnDt_prev_devolucao pvColDt_prev_devolucao;

    public sealed class ColumnDt_prev_devolucao : TDataColumn {

      public ColumnDt_prev_devolucao() {
        this.ColumnName = "dt_prev_devolucao";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data da previsão da devolução";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_emprestimo.dt_prev_devolucao"; } }
      public override string OriginalCol { get { return "dt_prev_devolucao"; } }
      public override string RefCol { get { return "LY_BIB_EMPRESTIMO.DT_PREV_DEVOLUCAO"; } }
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

    #region Coluna: DT_DEVOLUCAO
    private ColumnDt_devolucao pvColDt_devolucao;

    public sealed class ColumnDt_devolucao : TDataColumn {

      public ColumnDt_devolucao() {
        this.ColumnName = "dt_devolucao";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data da devolução";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_emprestimo.dt_devolucao"; } }
      public override string OriginalCol { get { return "dt_devolucao"; } }
      public override string RefCol { get { return "LY_BIB_EMPRESTIMO.DT_DEVOLUCAO"; } }
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

  public partial class Ly_bib_localizacao : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_bib_localizacao() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_bib_localizacao");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_bib_localizacao");
      }
      catch{}
    }

    public Ly_bib_localizacao() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColSigla_local = new ColumnSigla_local(); Columns.Add(pvColSigla_local);
      pvColDescr_local = new ColumnDescr_local(); Columns.Add(pvColDescr_local);
      pvColId_bib_biblioteca = new ColumnId_bib_biblioteca(); Columns.Add(pvColId_bib_biblioteca);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_bib_localizacao");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(decimal? @id_bib_biblioteca) {
      Row row = NewRow();

      row.Id_bib_biblioteca = @id_bib_biblioteca;

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
      get { return "LY_BIB_LOCALIZACAO"; }
    }

    protected override sealed string Name { get { return "Localização"; } }

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
        case CommandOperation.Select: cm.CommandText = "zLy_bib_localizacao_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_bib_localizacao_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_bib_localizacao_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_bib_localizacao_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("sigla_local", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SIGLA_LOCAL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("descr_local", OleDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "DESCR_LOCAL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("id_bib_biblioteca", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_BIB_BIBLIOTECA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "sigla_local") { result.Add(toLower ? "sigla_local" : column); continue; }
          if(lower == "descr_local") { result.Add(toLower ? "descr_local" : column); continue; }
          if(lower == "id_bib_biblioteca") { result.Add(toLower ? "id_bib_biblioteca" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "sigla_local", "descr_local", "id_bib_biblioteca" }
                          : new string[] { "ID", "SIGLA_LOCAL", "DESCR_LOCAL", "ID_BIB_BIBLIOTECA" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "sigla_local", "descr_local", "id_bib_biblioteca" }
                          : new string[] { "ID", "SIGLA_LOCAL", "DESCR_LOCAL", "ID_BIB_BIBLIOTECA" });

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
        "ly_bib_localizacao.id"
      });
      if(scColumns.Contains("sigla_local")) list.Add("ly_bib_localizacao.sigla_local");
      if(scColumns.Contains("descr_local")) list.Add("ly_bib_localizacao.descr_local");
      if(scColumns.Contains("id_bib_biblioteca")) list.Add("ly_bib_localizacao.id_bib_biblioteca");
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
          "FROM ly_bib_localizacao " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_bib_localizacao");
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
        da.TableMappings.Add("Table", "Ly_bib_localizacao");
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
            if (dr["SIGLA_LOCAL"] != null && string.IsNullOrEmpty(dr["SIGLA_LOCAL"].ToString()))
              dr["SIGLA_LOCAL"] = DBNull.Value;
            if (dr["DESCR_LOCAL"] != null && string.IsNullOrEmpty(dr["DESCR_LOCAL"].ToString()))
              dr["DESCR_LOCAL"] = DBNull.Value;
            if (dr["ID_BIB_BIBLIOTECA"] != null && string.IsNullOrEmpty(dr["ID_BIB_BIBLIOTECA"].ToString()))
              dr["ID_BIB_BIBLIOTECA"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["SIGLA_LOCAL"] != null && !string.IsNullOrEmpty(dr["SIGLA_LOCAL"].ToString()))
            {
              if (dr["SIGLA_LOCAL"].ToString().Length > 50)
              {
                return "A sigla deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["DESCR_LOCAL"] != null && !string.IsNullOrEmpty(dr["DESCR_LOCAL"].ToString()))
            {
              if (dr["DESCR_LOCAL"].ToString().Length > 200)
              {
                return "A descrição deve possuir no máximo 200 caracteres.";
              }
            }

            if (dr["ID_BIB_BIBLIOTECA"] == null || string.IsNullOrEmpty(dr["ID_BIB_BIBLIOTECA"].ToString()))
            {
              return "Favor informar a biblioteca.";
            }

            if (dr["ID_BIB_BIBLIOTECA"] != null && !string.IsNullOrEmpty(dr["ID_BIB_BIBLIOTECA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_BIB_BIBLIOTECA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_BIBLIOTECA"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_BIB_BIBLIOTECA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_BIBLIOTECA"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A biblioteca deve ser um valor inteiro com 14 dígitos no máximo.";
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

    public static Ly_bib_localizacao Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_bib_localizacao Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_bib_localizacao tab = new Ly_bib_localizacao();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_bib_localizacao tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, string @sigla_local, string @descr_local, decimal? @id_bib_biblioteca
      ) {
        return (Row)Add(new object[] {
          @id, @sigla_local, @descr_local, @id_bib_biblioteca
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
      /// SIGLA_LOCAL: VARCHAR(50)
      /// </summary>
      public string Sigla_local { get { return (string)this["sigla_local"]; } set { this["sigla_local"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCR_LOCAL: VARCHAR(200)
      /// </summary>
      public string Descr_local { get { return (string)this["descr_local"]; } set { this["descr_local"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ID_BIB_BIBLIOTECA: NUMERIC(14), Not Null
      /// </summary>
      public decimal? Id_bib_biblioteca { get { return (decimal?)this["id_bib_biblioteca"]; } set { this["id_bib_biblioteca"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               decimal? @id_bib_biblioteca) {
        return Insert(connection,
                      @id_bib_biblioteca,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               decimal? @id_bib_biblioteca,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_bib_localizacao(),
          new string[] { "id_bib_biblioteca" },
          new object[] { @id_bib_biblioteca },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_bib_localizacao(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_bib_localizacao(),
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
          connection, new Techne.Lyceum.CR.Ly_bib_localizacao(),
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
        public string Sigla_local { get { return (string)row["sigla_local", DataRowVersion.Original]; } }
        public string Descr_local { get { return (string)row["descr_local", DataRowVersion.Original]; } }
        public decimal? Id_bib_biblioteca { get { return (decimal?)row["id_bib_biblioteca", DataRowVersion.Original]; } }

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


      protected override string GetName(){return "identificador";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_localizacao.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_BIB_LOCALIZACAO.ID"; } }
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

    #region Coluna: SIGLA_LOCAL
    private ColumnSigla_local pvColSigla_local;

    public sealed class ColumnSigla_local : TDataColumn {

      public ColumnSigla_local() {
        this.ColumnName = "sigla_local";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "sigla";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_localizacao.sigla_local"; } }
      public override string OriginalCol { get { return "sigla_local"; } }
      public override string RefCol { get { return "LY_BIB_LOCALIZACAO.SIGLA_LOCAL"; } }
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

    #region Coluna: DESCR_LOCAL
    private ColumnDescr_local pvColDescr_local;

    public sealed class ColumnDescr_local : TDataColumn {

      public ColumnDescr_local() {
        this.ColumnName = "descr_local";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "descrição";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_localizacao.descr_local"; } }
      public override string OriginalCol { get { return "descr_local"; } }
      public override string RefCol { get { return "LY_BIB_LOCALIZACAO.DESCR_LOCAL"; } }
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

    #region Coluna: ID_BIB_BIBLIOTECA
    private ColumnId_bib_biblioteca pvColId_bib_biblioteca;

    public sealed class ColumnId_bib_biblioteca : TDataColumn {

      public ColumnId_bib_biblioteca() {
        this.ColumnName = "id_bib_biblioteca";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "biblioteca";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_localizacao.id_bib_biblioteca"; } }
      public override string OriginalCol { get { return "id_bib_biblioteca"; } }
      public override string RefCol { get { return "LY_BIB_LOCALIZACAO.ID_BIB_BIBLIOTECA"; } }
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
    #endregion
  }

  public partial class Ly_bib_material : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_bib_material() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_bib_material");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_bib_material");
      }
      catch{}
    }

    public Ly_bib_material() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColCod_barras_material = new ColumnCod_barras_material(); Columns.Add(pvColCod_barras_material);
      pvColDt_aquisicao = new ColumnDt_aquisicao(); Columns.Add(pvColDt_aquisicao);
      pvColForma_aquisicao = new ColumnForma_aquisicao(); Columns.Add(pvColForma_aquisicao);
      pvColDt_baixa = new ColumnDt_baixa(); Columns.Add(pvColDt_baixa);
      pvColMotivo_baixa = new ColumnMotivo_baixa(); Columns.Add(pvColMotivo_baixa);
      pvColObs_baixa = new ColumnObs_baixa(); Columns.Add(pvColObs_baixa);
      pvColId_bib_localizacao = new ColumnId_bib_localizacao(); Columns.Add(pvColId_bib_localizacao);
      pvColId_bib_titulo = new ColumnId_bib_titulo(); Columns.Add(pvColId_bib_titulo);
      pvColNum_registro = new ColumnNum_registro(); Columns.Add(pvColNum_registro);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_bib_material");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(decimal? @cod_barras_material, decimal? @num_registro) {
      Row row = NewRow();

      row.Cod_barras_material = @cod_barras_material;
      row.Num_registro = @num_registro;

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
      get { return "LY_BIB_MATERIAL"; }
    }

    protected override sealed string Name { get { return "Material"; } }

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
        case CommandOperation.Select: cm.CommandText = "zLy_bib_material_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_bib_material_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_bib_material_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_bib_material_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("cod_barras_material", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "COD_BARRAS_MATERIAL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("dt_aquisicao", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_AQUISICAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("forma_aquisicao", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "FORMA_AQUISICAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("dt_baixa", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_BAIXA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("motivo_baixa", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "MOTIVO_BAIXA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("obs_baixa", OleDbType.VarChar, 600, ParameterDirection.InputOutput, true, 0, 0, "OBS_BAIXA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("id_bib_localizacao", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_BIB_LOCALIZACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("id_bib_titulo", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_BIB_TITULO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("num_registro", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "NUM_REGISTRO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "cod_barras_material") { result.Add(toLower ? "cod_barras_material" : column); continue; }
          if(lower == "dt_aquisicao") { result.Add(toLower ? "dt_aquisicao" : column); continue; }
          if(lower == "forma_aquisicao") { result.Add(toLower ? "forma_aquisicao" : column); continue; }
          if(lower == "dt_baixa") { result.Add(toLower ? "dt_baixa" : column); continue; }
          if(lower == "motivo_baixa") { result.Add(toLower ? "motivo_baixa" : column); continue; }
          if(lower == "obs_baixa") { result.Add(toLower ? "obs_baixa" : column); continue; }
          if(lower == "id_bib_localizacao") { result.Add(toLower ? "id_bib_localizacao" : column); continue; }
          if(lower == "id_bib_titulo") { result.Add(toLower ? "id_bib_titulo" : column); continue; }
          if(lower == "num_registro") { result.Add(toLower ? "num_registro" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "cod_barras_material", "dt_aquisicao", "forma_aquisicao", 
                                           "dt_baixa", "motivo_baixa", "obs_baixa", "id_bib_localizacao", "id_bib_titulo", "num_registro" }
                          : new string[] { "ID", "COD_BARRAS_MATERIAL", "DT_AQUISICAO", "FORMA_AQUISICAO", 
                                           "DT_BAIXA", "MOTIVO_BAIXA", "OBS_BAIXA", "ID_BIB_LOCALIZACAO", "ID_BIB_TITULO", "NUM_REGISTRO" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "cod_barras_material", "dt_aquisicao", "forma_aquisicao", 
                                           "dt_baixa", "motivo_baixa", "obs_baixa", "id_bib_localizacao", "id_bib_titulo", "num_registro" }
                          : new string[] { "ID", "COD_BARRAS_MATERIAL", "DT_AQUISICAO", "FORMA_AQUISICAO", 
                                           "DT_BAIXA", "MOTIVO_BAIXA", "OBS_BAIXA", "ID_BIB_LOCALIZACAO", "ID_BIB_TITULO", "NUM_REGISTRO" });

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
        "ly_bib_material.id"
      });
      if(scColumns.Contains("cod_barras_material")) list.Add("ly_bib_material.cod_barras_material");
      if(scColumns.Contains("dt_aquisicao")) list.Add("ly_bib_material.dt_aquisicao");
      if(scColumns.Contains("forma_aquisicao")) list.Add("ly_bib_material.forma_aquisicao");
      if(scColumns.Contains("dt_baixa")) list.Add("ly_bib_material.dt_baixa");
      if(scColumns.Contains("motivo_baixa")) list.Add("ly_bib_material.motivo_baixa");
      if(scColumns.Contains("obs_baixa")) list.Add("ly_bib_material.obs_baixa");
      if(scColumns.Contains("id_bib_localizacao")) list.Add("ly_bib_material.id_bib_localizacao");
      if(scColumns.Contains("id_bib_titulo")) list.Add("ly_bib_material.id_bib_titulo");
      if(scColumns.Contains("num_registro")) list.Add("ly_bib_material.num_registro");
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
          "FROM ly_bib_material " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_bib_material");
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
        da.TableMappings.Add("Table", "Ly_bib_material");
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
            if (dr["COD_BARRAS_MATERIAL"] != null && string.IsNullOrEmpty(dr["COD_BARRAS_MATERIAL"].ToString()))
              dr["COD_BARRAS_MATERIAL"] = DBNull.Value;
            if (dr["FORMA_AQUISICAO"] != null && string.IsNullOrEmpty(dr["FORMA_AQUISICAO"].ToString()))
              dr["FORMA_AQUISICAO"] = DBNull.Value;
            if (dr["MOTIVO_BAIXA"] != null && string.IsNullOrEmpty(dr["MOTIVO_BAIXA"].ToString()))
              dr["MOTIVO_BAIXA"] = DBNull.Value;
            if (dr["OBS_BAIXA"] != null && string.IsNullOrEmpty(dr["OBS_BAIXA"].ToString()))
              dr["OBS_BAIXA"] = DBNull.Value;
            if (dr["ID_BIB_LOCALIZACAO"] != null && string.IsNullOrEmpty(dr["ID_BIB_LOCALIZACAO"].ToString()))
              dr["ID_BIB_LOCALIZACAO"] = DBNull.Value;
            if (dr["ID_BIB_TITULO"] != null && string.IsNullOrEmpty(dr["ID_BIB_TITULO"].ToString()))
              dr["ID_BIB_TITULO"] = DBNull.Value;
            if (dr["NUM_REGISTRO"] != null && string.IsNullOrEmpty(dr["NUM_REGISTRO"].ToString()))
              dr["NUM_REGISTRO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["COD_BARRAS_MATERIAL"] == null || string.IsNullOrEmpty(dr["COD_BARRAS_MATERIAL"].ToString()))
            {
              return "Favor informar o código de barras.";
            }

            if (dr["COD_BARRAS_MATERIAL"] != null && !string.IsNullOrEmpty(dr["COD_BARRAS_MATERIAL"].ToString()))
            {
              if ((Convert.ToDecimal(dr["COD_BARRAS_MATERIAL"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["COD_BARRAS_MATERIAL"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["COD_BARRAS_MATERIAL"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["COD_BARRAS_MATERIAL"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O código de barras deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["DT_AQUISICAO"] != null && dr["DT_AQUISICAO"].ToString() != "")
            {
              dr["DT_AQUISICAO"] = Convert.ToDateTime(dr["DT_AQUISICAO"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["FORMA_AQUISICAO"] != null && !string.IsNullOrEmpty(dr["FORMA_AQUISICAO"].ToString()))
            {
              if (dr["FORMA_AQUISICAO"].ToString().Length > 50)
              {
                return "A forma de aquisição deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["DT_BAIXA"] != null && dr["DT_BAIXA"].ToString() != "")
            {
              dr["DT_BAIXA"] = Convert.ToDateTime(dr["DT_BAIXA"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["MOTIVO_BAIXA"] != null && !string.IsNullOrEmpty(dr["MOTIVO_BAIXA"].ToString()))
            {
              if (dr["MOTIVO_BAIXA"].ToString().Length > 50)
              {
                return "O motivo da baixa deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["OBS_BAIXA"] != null && !string.IsNullOrEmpty(dr["OBS_BAIXA"].ToString()))
            {
              if (dr["OBS_BAIXA"].ToString().Length > 500)
              {
                return "A observação da baixa deve possuir no máximo 500 caracteres.";
              }
            }

            if (dr["ID_BIB_LOCALIZACAO"] != null && !string.IsNullOrEmpty(dr["ID_BIB_LOCALIZACAO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_BIB_LOCALIZACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_LOCALIZACAO"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_BIB_LOCALIZACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_LOCALIZACAO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A localização deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["ID_BIB_TITULO"] != null && !string.IsNullOrEmpty(dr["ID_BIB_TITULO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_BIB_TITULO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_TITULO"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_BIB_TITULO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_TITULO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O título deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["NUM_REGISTRO"] == null || string.IsNullOrEmpty(dr["NUM_REGISTRO"].ToString()))
            {
              return "Favor informar o número do registro.";
            }

            if (dr["NUM_REGISTRO"] != null && !string.IsNullOrEmpty(dr["NUM_REGISTRO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["NUM_REGISTRO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["NUM_REGISTRO"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["NUM_REGISTRO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["NUM_REGISTRO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O número do registro deve ser um valor inteiro com 10 dígitos no máximo.";
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

    public static Ly_bib_material Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_bib_material Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_bib_material tab = new Ly_bib_material();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_bib_material tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, decimal? @cod_barras_material, DateTime? @dt_aquisicao, string @forma_aquisicao, 
        DateTime? @dt_baixa, string @motivo_baixa, string @obs_baixa, decimal? @id_bib_localizacao, 
        decimal? @id_bib_titulo, decimal? @num_registro
      ) {
        return (Row)Add(new object[] {
          @id, @cod_barras_material, @dt_aquisicao, @forma_aquisicao, @dt_baixa, @motivo_baixa, 
          @obs_baixa, @id_bib_localizacao, @id_bib_titulo, @num_registro
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
      /// COD_BARRAS_MATERIAL: NUMERIC(14), Not Null
      /// </summary>
      public decimal? Cod_barras_material { get { return (decimal?)this["cod_barras_material"]; } set { this["cod_barras_material"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_AQUISICAO: DATETIME
      /// </summary>
      public DateTime? Dt_aquisicao { get { return (DateTime?)this["dt_aquisicao"]; } set { this["dt_aquisicao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// FORMA_AQUISICAO: VARCHAR(50)
      /// </summary>
      public string Forma_aquisicao { get { return (string)this["forma_aquisicao"]; } set { this["forma_aquisicao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_BAIXA: DATETIME
      /// </summary>
      public DateTime? Dt_baixa { get { return (DateTime?)this["dt_baixa"]; } set { this["dt_baixa"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// MOTIVO_BAIXA: VARCHAR(50)
      /// </summary>
      public string Motivo_baixa { get { return (string)this["motivo_baixa"]; } set { this["motivo_baixa"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// OBS_BAIXA: VARCHAR(500)
      /// </summary>
      public string Obs_baixa { get { return (string)this["obs_baixa"]; } set { this["obs_baixa"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ID_BIB_LOCALIZACAO: NUMERIC(14)
      /// </summary>
      public decimal? Id_bib_localizacao { get { return (decimal?)this["id_bib_localizacao"]; } set { this["id_bib_localizacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ID_BIB_TITULO: NUMERIC(14)
      /// </summary>
      public decimal? Id_bib_titulo { get { return (decimal?)this["id_bib_titulo"]; } set { this["id_bib_titulo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NUM_REGISTRO: NUMERIC(10), Not Null
      /// </summary>
      public decimal? Num_registro { get { return (decimal?)this["num_registro"]; } set { this["num_registro"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               decimal? @cod_barras_material, decimal? @num_registro) {
        return Insert(connection,
                      @cod_barras_material, @num_registro,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               decimal? @cod_barras_material, decimal? @num_registro,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_bib_material(),
          new string[] { "cod_barras_material", "num_registro" },
          new object[] { @cod_barras_material, @num_registro },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_bib_material(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_bib_material(),
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
          connection, new Techne.Lyceum.CR.Ly_bib_material(),
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
        public decimal? Cod_barras_material { get { return (decimal?)row["cod_barras_material", DataRowVersion.Original]; } }
        public DateTime? Dt_aquisicao { get { return (DateTime?)row["dt_aquisicao", DataRowVersion.Original]; } }
        public string Forma_aquisicao { get { return (string)row["forma_aquisicao", DataRowVersion.Original]; } }
        public DateTime? Dt_baixa { get { return (DateTime?)row["dt_baixa", DataRowVersion.Original]; } }
        public string Motivo_baixa { get { return (string)row["motivo_baixa", DataRowVersion.Original]; } }
        public string Obs_baixa { get { return (string)row["obs_baixa", DataRowVersion.Original]; } }
        public decimal? Id_bib_localizacao { get { return (decimal?)row["id_bib_localizacao", DataRowVersion.Original]; } }
        public decimal? Id_bib_titulo { get { return (decimal?)row["id_bib_titulo", DataRowVersion.Original]; } }
        public decimal? Num_registro { get { return (decimal?)row["num_registro", DataRowVersion.Original]; } }

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


      protected override string GetName(){return "identificador";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_material.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_BIB_MATERIAL.ID"; } }
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

    #region Coluna: COD_BARRAS_MATERIAL
    private ColumnCod_barras_material pvColCod_barras_material;

    public sealed class ColumnCod_barras_material : TDataColumn {

      public ColumnCod_barras_material() {
        this.ColumnName = "cod_barras_material";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "código de barras";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_material.cod_barras_material"; } }
      public override string OriginalCol { get { return "cod_barras_material"; } }
      public override string RefCol { get { return "LY_BIB_MATERIAL.COD_BARRAS_MATERIAL"; } }
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

    #region Coluna: DT_AQUISICAO
    private ColumnDt_aquisicao pvColDt_aquisicao;

    public sealed class ColumnDt_aquisicao : TDataColumn {

      public ColumnDt_aquisicao() {
        this.ColumnName = "dt_aquisicao";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data da aquisição";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_material.dt_aquisicao"; } }
      public override string OriginalCol { get { return "dt_aquisicao"; } }
      public override string RefCol { get { return "LY_BIB_MATERIAL.DT_AQUISICAO"; } }
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

    #region Coluna: FORMA_AQUISICAO
    private ColumnForma_aquisicao pvColForma_aquisicao;

    public sealed class ColumnForma_aquisicao : TDataColumn {

      public ColumnForma_aquisicao() {
        this.ColumnName = "forma_aquisicao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "forma de aquisição";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_material.forma_aquisicao"; } }
      public override string OriginalCol { get { return "forma_aquisicao"; } }
      public override string RefCol { get { return "LY_BIB_MATERIAL.FORMA_AQUISICAO"; } }
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

    #region Coluna: DT_BAIXA
    private ColumnDt_baixa pvColDt_baixa;

    public sealed class ColumnDt_baixa : TDataColumn {

      public ColumnDt_baixa() {
        this.ColumnName = "dt_baixa";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data da baixa";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_material.dt_baixa"; } }
      public override string OriginalCol { get { return "dt_baixa"; } }
      public override string RefCol { get { return "LY_BIB_MATERIAL.DT_BAIXA"; } }
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

    #region Coluna: MOTIVO_BAIXA
    private ColumnMotivo_baixa pvColMotivo_baixa;

    public sealed class ColumnMotivo_baixa : TDataColumn {

      public ColumnMotivo_baixa() {
        this.ColumnName = "motivo_baixa";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "motivo da baixa";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_material.motivo_baixa"; } }
      public override string OriginalCol { get { return "motivo_baixa"; } }
      public override string RefCol { get { return "LY_BIB_MATERIAL.MOTIVO_BAIXA"; } }
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

    #region Coluna: OBS_BAIXA
    private ColumnObs_baixa pvColObs_baixa;

    public sealed class ColumnObs_baixa : TDataColumn {

      public ColumnObs_baixa() {
        this.ColumnName = "obs_baixa";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "observação da baixa";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_material.obs_baixa"; } }
      public override string OriginalCol { get { return "obs_baixa"; } }
      public override string RefCol { get { return "LY_BIB_MATERIAL.OBS_BAIXA"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 500; } }
      public override string SqlServerType { get { return "VARCHAR(500)"; } }

    }
    #endregion

    #region Coluna: ID_BIB_LOCALIZACAO
    private ColumnId_bib_localizacao pvColId_bib_localizacao;

    public sealed class ColumnId_bib_localizacao : TDataColumn {

      public ColumnId_bib_localizacao() {
        this.ColumnName = "id_bib_localizacao";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "localização";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_material.id_bib_localizacao"; } }
      public override string OriginalCol { get { return "id_bib_localizacao"; } }
      public override string RefCol { get { return "LY_BIB_MATERIAL.ID_BIB_LOCALIZACAO"; } }
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

    #region Coluna: ID_BIB_TITULO
    private ColumnId_bib_titulo pvColId_bib_titulo;

    public sealed class ColumnId_bib_titulo : TDataColumn {

      public ColumnId_bib_titulo() {
        this.ColumnName = "id_bib_titulo";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "título";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_material.id_bib_titulo"; } }
      public override string OriginalCol { get { return "id_bib_titulo"; } }
      public override string RefCol { get { return "LY_BIB_MATERIAL.ID_BIB_TITULO"; } }
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

    #region Coluna: NUM_REGISTRO
    private ColumnNum_registro pvColNum_registro;

    public sealed class ColumnNum_registro : TDataColumn {

      public ColumnNum_registro() {
        this.ColumnName = "num_registro";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "número do registro";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_material.num_registro"; } }
      public override string OriginalCol { get { return "num_registro"; } }
      public override string RefCol { get { return "LY_BIB_MATERIAL.NUM_REGISTRO"; } }
      public override bool PrimaryKey { get { return false; } }
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
    #endregion
  }

  public partial class Ly_bib_participante : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_bib_participante() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_bib_participante");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_bib_participante");
      }
      catch{}
    }

    public Ly_bib_participante() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColNome = new ColumnNome(); Columns.Add(pvColNome);
      pvColEmail = new ColumnEmail(); Columns.Add(pvColEmail);
      pvColId_bib_projeto = new ColumnId_bib_projeto(); Columns.Add(pvColId_bib_projeto);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_bib_participante");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @nome, decimal? @id_bib_projeto) {
      Row row = NewRow();

      row.Nome = @nome;
      row.Id_bib_projeto = @id_bib_projeto;

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
      get { return "LY_BIB_PARTICIPANTE"; }
    }

    protected override sealed string Name { get { return "Participante"; } }

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
        case CommandOperation.Select: cm.CommandText = "zLy_bib_participante_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_bib_participante_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_bib_participante_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_bib_participante_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("nome", OleDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "NOME", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("email", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "EMAIL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("id_bib_projeto", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_BIB_PROJETO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "email") { result.Add(toLower ? "email" : column); continue; }
          if(lower == "id_bib_projeto") { result.Add(toLower ? "id_bib_projeto" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "nome", "email", "id_bib_projeto" }
                          : new string[] { "ID", "NOME", "EMAIL", "ID_BIB_PROJETO" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "nome", "email", "id_bib_projeto" }
                          : new string[] { "ID", "NOME", "EMAIL", "ID_BIB_PROJETO" });

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
        "ly_bib_participante.id"
      });
      if(scColumns.Contains("nome")) list.Add("ly_bib_participante.nome");
      if(scColumns.Contains("email")) list.Add("ly_bib_participante.email");
      if(scColumns.Contains("id_bib_projeto")) list.Add("ly_bib_participante.id_bib_projeto");
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
          "FROM ly_bib_participante " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_bib_participante");
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
        da.TableMappings.Add("Table", "Ly_bib_participante");
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
            if (dr["EMAIL"] != null && string.IsNullOrEmpty(dr["EMAIL"].ToString()))
              dr["EMAIL"] = DBNull.Value;
            if (dr["ID_BIB_PROJETO"] != null && string.IsNullOrEmpty(dr["ID_BIB_PROJETO"].ToString()))
              dr["ID_BIB_PROJETO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["NOME"] == null || string.IsNullOrEmpty(dr["NOME"].ToString()))
            {
              return "Favor informar o nome do participante.";
            }

            if (dr["NOME"] != null && !string.IsNullOrEmpty(dr["NOME"].ToString()))
            {
              if (dr["NOME"].ToString().Length > 200)
              {
                return "O nome do participante deve possuir no máximo 200 caracteres.";
              }
            }

            if (dr["EMAIL"] != null && !string.IsNullOrEmpty(dr["EMAIL"].ToString()))
            {
              if (dr["EMAIL"].ToString().Length > 50)
              {
                return "O email deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["ID_BIB_PROJETO"] == null || string.IsNullOrEmpty(dr["ID_BIB_PROJETO"].ToString()))
            {
              return "Favor informar o projeto.";
            }

            if (dr["ID_BIB_PROJETO"] != null && !string.IsNullOrEmpty(dr["ID_BIB_PROJETO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_BIB_PROJETO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_PROJETO"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_BIB_PROJETO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_PROJETO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O projeto deve ser um valor inteiro com 14 dígitos no máximo.";
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

    public static Ly_bib_participante Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_bib_participante Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_bib_participante tab = new Ly_bib_participante();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_bib_participante tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, string @nome, string @email, decimal? @id_bib_projeto
      ) {
        return (Row)Add(new object[] {
          @id, @nome, @email, @id_bib_projeto
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
      /// EMAIL: VARCHAR(50)
      /// </summary>
      public string Email { get { return (string)this["email"]; } set { this["email"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ID_BIB_PROJETO: NUMERIC(14), Not Null
      /// </summary>
      public decimal? Id_bib_projeto { get { return (decimal?)this["id_bib_projeto"]; } set { this["id_bib_projeto"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @nome, decimal? @id_bib_projeto) {
        return Insert(connection,
                      @nome, @id_bib_projeto,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @nome, decimal? @id_bib_projeto,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_bib_participante(),
          new string[] { "nome", "id_bib_projeto" },
          new object[] { @nome, @id_bib_projeto },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_bib_participante(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_bib_participante(),
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
          connection, new Techne.Lyceum.CR.Ly_bib_participante(),
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
        public string Email { get { return (string)row["email", DataRowVersion.Original]; } }
        public decimal? Id_bib_projeto { get { return (decimal?)row["id_bib_projeto", DataRowVersion.Original]; } }

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


      protected override string GetName(){return "identificador";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_participante.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_BIB_PARTICIPANTE.ID"; } }
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


      protected override string GetName(){return "nome do participante";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_participante.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "LY_BIB_PARTICIPANTE.NOME"; } }
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
      public override string FullCol { get { return "ly_bib_participante.email"; } }
      public override string OriginalCol { get { return "email"; } }
      public override string RefCol { get { return "LY_BIB_PARTICIPANTE.EMAIL"; } }
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

    #region Coluna: ID_BIB_PROJETO
    private ColumnId_bib_projeto pvColId_bib_projeto;

    public sealed class ColumnId_bib_projeto : TDataColumn {

      public ColumnId_bib_projeto() {
        this.ColumnName = "id_bib_projeto";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "projeto";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_participante.id_bib_projeto"; } }
      public override string OriginalCol { get { return "id_bib_projeto"; } }
      public override string RefCol { get { return "LY_BIB_PARTICIPANTE.ID_BIB_PROJETO"; } }
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
    #endregion
  }

  public partial class Ly_bib_penalidade : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_bib_penalidade() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_bib_penalidade");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_bib_penalidade");
      }
      catch{}
    }

    public Ly_bib_penalidade() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColId_bib_usuario = new ColumnId_bib_usuario(); Columns.Add(pvColId_bib_usuario);
      pvColMotivo = new ColumnMotivo(); Columns.Add(pvColMotivo);
      pvColDt_penalidade = new ColumnDt_penalidade(); Columns.Add(pvColDt_penalidade);
      pvColDt_baixa = new ColumnDt_baixa(); Columns.Add(pvColDt_baixa);
      pvColId_bib_emprestimo = new ColumnId_bib_emprestimo(); Columns.Add(pvColId_bib_emprestimo);
      pvColValor = new ColumnValor(); Columns.Add(pvColValor);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_bib_penalidade");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(decimal? @id_bib_usuario, string @motivo, DateTime? @dt_penalidade) {
      Row row = NewRow();

      row.Id_bib_usuario = @id_bib_usuario;
      row.Motivo = @motivo;
      row.Dt_penalidade = @dt_penalidade;

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
      get { return "LY_BIB_PENALIDADE"; }
    }

    protected override sealed string Name { get { return "Penalidade"; } }

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
        case CommandOperation.Select: cm.CommandText = "zLy_bib_penalidade_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_bib_penalidade_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_bib_penalidade_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_bib_penalidade_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("id_bib_usuario", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_BIB_USUARIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("motivo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "MOTIVO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("dt_penalidade", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_PENALIDADE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_baixa", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_BAIXA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("id_bib_emprestimo", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_BIB_EMPRESTIMO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("valor", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "VALOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "id_bib_usuario") { result.Add(toLower ? "id_bib_usuario" : column); continue; }
          if(lower == "motivo") { result.Add(toLower ? "motivo" : column); continue; }
          if(lower == "dt_penalidade") { result.Add(toLower ? "dt_penalidade" : column); continue; }
          if(lower == "dt_baixa") { result.Add(toLower ? "dt_baixa" : column); continue; }
          if(lower == "id_bib_emprestimo") { result.Add(toLower ? "id_bib_emprestimo" : column); continue; }
          if(lower == "valor") { result.Add(toLower ? "valor" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "id_bib_usuario", "motivo", "dt_penalidade", "dt_baixa", "id_bib_emprestimo", 
                                           "valor" }
                          : new string[] { "ID", "ID_BIB_USUARIO", "MOTIVO", "DT_PENALIDADE", "DT_BAIXA", "ID_BIB_EMPRESTIMO", 
                                           "VALOR" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "id_bib_usuario", "motivo", "dt_penalidade", "dt_baixa", "id_bib_emprestimo", 
                                           "valor" }
                          : new string[] { "ID", "ID_BIB_USUARIO", "MOTIVO", "DT_PENALIDADE", "DT_BAIXA", "ID_BIB_EMPRESTIMO", 
                                           "VALOR" });

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
        "ly_bib_penalidade.id"
      });
      if(scColumns.Contains("id_bib_usuario")) list.Add("ly_bib_penalidade.id_bib_usuario");
      if(scColumns.Contains("motivo")) list.Add("ly_bib_penalidade.motivo");
      if(scColumns.Contains("dt_penalidade")) list.Add("ly_bib_penalidade.dt_penalidade");
      if(scColumns.Contains("dt_baixa")) list.Add("ly_bib_penalidade.dt_baixa");
      if(scColumns.Contains("id_bib_emprestimo")) list.Add("ly_bib_penalidade.id_bib_emprestimo");
      if(scColumns.Contains("valor")) list.Add("ly_bib_penalidade.valor");
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
          "FROM ly_bib_penalidade " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_bib_penalidade");
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
        da.TableMappings.Add("Table", "Ly_bib_penalidade");
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
            if (dr["ID_BIB_USUARIO"] != null && string.IsNullOrEmpty(dr["ID_BIB_USUARIO"].ToString()))
              dr["ID_BIB_USUARIO"] = DBNull.Value;
            if (dr["MOTIVO"] != null && string.IsNullOrEmpty(dr["MOTIVO"].ToString()))
              dr["MOTIVO"] = DBNull.Value;
            if (dr["ID_BIB_EMPRESTIMO"] != null && string.IsNullOrEmpty(dr["ID_BIB_EMPRESTIMO"].ToString()))
              dr["ID_BIB_EMPRESTIMO"] = DBNull.Value;
            if (dr["VALOR"] != null && string.IsNullOrEmpty(dr["VALOR"].ToString()))
              dr["VALOR"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["ID_BIB_USUARIO"] == null || string.IsNullOrEmpty(dr["ID_BIB_USUARIO"].ToString()))
            {
              return "Favor informar o usuário.";
            }

            if (dr["ID_BIB_USUARIO"] != null && !string.IsNullOrEmpty(dr["ID_BIB_USUARIO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_BIB_USUARIO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_USUARIO"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_BIB_USUARIO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_USUARIO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O usuário deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["MOTIVO"] == null || string.IsNullOrEmpty(dr["MOTIVO"].ToString()))
            {
              return "Favor informar o motivo.";
            }

            if (dr["MOTIVO"] != null && !string.IsNullOrEmpty(dr["MOTIVO"].ToString()))
            {
              if (dr["MOTIVO"].ToString().Length > 50)
              {
                return "O motivo deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["DT_PENALIDADE"] == null || string.IsNullOrEmpty(dr["DT_PENALIDADE"].ToString()))
            {
              return "Favor informar a data da penalidade.";
            }

            if (dr["DT_PENALIDADE"] != null && dr["DT_PENALIDADE"].ToString() != "")
            {
              dr["DT_PENALIDADE"] = Convert.ToDateTime(dr["DT_PENALIDADE"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_BAIXA"] != null && dr["DT_BAIXA"].ToString() != "")
            {
              dr["DT_BAIXA"] = Convert.ToDateTime(dr["DT_BAIXA"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["ID_BIB_EMPRESTIMO"] != null && !string.IsNullOrEmpty(dr["ID_BIB_EMPRESTIMO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_BIB_EMPRESTIMO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_EMPRESTIMO"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_BIB_EMPRESTIMO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_EMPRESTIMO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O nome do participante deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["VALOR"] != null && !string.IsNullOrEmpty(dr["VALOR"].ToString()))
            {
              if ((Convert.ToDecimal(dr["VALOR"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["VALOR"]).ToString("0.############").Split(',')[0].Length > 2) || (Convert.ToDecimal(dr["VALOR"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["VALOR"]).ToString("0.############").Split(',')[1].Length > 2))
              {
                return "O valor deve ser um valor numérico com, no máximo, 2 dígitos na parte inteira e 2 casas decimal.";
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

    public static Ly_bib_penalidade Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_bib_penalidade Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_bib_penalidade tab = new Ly_bib_penalidade();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_bib_penalidade tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, decimal? @id_bib_usuario, string @motivo, DateTime? @dt_penalidade, 
        DateTime? @dt_baixa, decimal? @id_bib_emprestimo, decimal? @valor
      ) {
        return (Row)Add(new object[] {
          @id, @id_bib_usuario, @motivo, @dt_penalidade, @dt_baixa, @id_bib_emprestimo, @valor
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
      /// ID_BIB_USUARIO: NUMERIC(14), Not Null
      /// </summary>
      public decimal? Id_bib_usuario { get { return (decimal?)this["id_bib_usuario"]; } set { this["id_bib_usuario"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// MOTIVO: VARCHAR(50), Not Null
      /// </summary>
      public string Motivo { get { return (string)this["motivo"]; } set { this["motivo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_PENALIDADE: DATETIME, Not Null
      /// </summary>
      public DateTime? Dt_penalidade { get { return (DateTime?)this["dt_penalidade"]; } set { this["dt_penalidade"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_BAIXA: DATETIME
      /// </summary>
      public DateTime? Dt_baixa { get { return (DateTime?)this["dt_baixa"]; } set { this["dt_baixa"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ID_BIB_EMPRESTIMO: NUMERIC(14)
      /// </summary>
      public decimal? Id_bib_emprestimo { get { return (decimal?)this["id_bib_emprestimo"]; } set { this["id_bib_emprestimo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// VALOR: NUMERIC(4, 2)
      /// </summary>
      public decimal? Valor { get { return (decimal?)this["valor"]; } set { this["valor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               decimal? @id_bib_usuario, string @motivo, DateTime? @dt_penalidade) {
        return Insert(connection,
                      @id_bib_usuario, @motivo, @dt_penalidade,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               decimal? @id_bib_usuario, string @motivo, DateTime? @dt_penalidade,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_bib_penalidade(),
          new string[] { "id_bib_usuario", "motivo", "dt_penalidade" },
          new object[] { @id_bib_usuario, @motivo, @dt_penalidade },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_bib_penalidade(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_bib_penalidade(),
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
          connection, new Techne.Lyceum.CR.Ly_bib_penalidade(),
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
        public decimal? Id_bib_usuario { get { return (decimal?)row["id_bib_usuario", DataRowVersion.Original]; } }
        public string Motivo { get { return (string)row["motivo", DataRowVersion.Original]; } }
        public DateTime? Dt_penalidade { get { return (DateTime?)row["dt_penalidade", DataRowVersion.Original]; } }
        public DateTime? Dt_baixa { get { return (DateTime?)row["dt_baixa", DataRowVersion.Original]; } }
        public decimal? Id_bib_emprestimo { get { return (decimal?)row["id_bib_emprestimo", DataRowVersion.Original]; } }
        public decimal? Valor { get { return (decimal?)row["valor", DataRowVersion.Original]; } }

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


      protected override string GetName(){return "identificador";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_penalidade.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_BIB_PENALIDADE.ID"; } }
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

    #region Coluna: ID_BIB_USUARIO
    private ColumnId_bib_usuario pvColId_bib_usuario;

    public sealed class ColumnId_bib_usuario : TDataColumn {

      public ColumnId_bib_usuario() {
        this.ColumnName = "id_bib_usuario";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "usuário";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_penalidade.id_bib_usuario"; } }
      public override string OriginalCol { get { return "id_bib_usuario"; } }
      public override string RefCol { get { return "LY_BIB_PENALIDADE.ID_BIB_USUARIO"; } }
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

    #region Coluna: MOTIVO
    private ColumnMotivo pvColMotivo;

    public sealed class ColumnMotivo : TDataColumn {

      public ColumnMotivo() {
        this.ColumnName = "motivo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "motivo";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_penalidade.motivo"; } }
      public override string OriginalCol { get { return "motivo"; } }
      public override string RefCol { get { return "LY_BIB_PENALIDADE.MOTIVO"; } }
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

    #region Coluna: DT_PENALIDADE
    private ColumnDt_penalidade pvColDt_penalidade;

    public sealed class ColumnDt_penalidade : TDataColumn {

      public ColumnDt_penalidade() {
        this.ColumnName = "dt_penalidade";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data da penalidade";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_penalidade.dt_penalidade"; } }
      public override string OriginalCol { get { return "dt_penalidade"; } }
      public override string RefCol { get { return "LY_BIB_PENALIDADE.DT_PENALIDADE"; } }
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

    #region Coluna: DT_BAIXA
    private ColumnDt_baixa pvColDt_baixa;

    public sealed class ColumnDt_baixa : TDataColumn {

      public ColumnDt_baixa() {
        this.ColumnName = "dt_baixa";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data da baixa";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_penalidade.dt_baixa"; } }
      public override string OriginalCol { get { return "dt_baixa"; } }
      public override string RefCol { get { return "LY_BIB_PENALIDADE.DT_BAIXA"; } }
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

    #region Coluna: ID_BIB_EMPRESTIMO
    private ColumnId_bib_emprestimo pvColId_bib_emprestimo;

    public sealed class ColumnId_bib_emprestimo : TDataColumn {

      public ColumnId_bib_emprestimo() {
        this.ColumnName = "id_bib_emprestimo";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "nome do participante";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_penalidade.id_bib_emprestimo"; } }
      public override string OriginalCol { get { return "id_bib_emprestimo"; } }
      public override string RefCol { get { return "LY_BIB_PENALIDADE.ID_BIB_EMPRESTIMO"; } }
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

    #region Coluna: VALOR
    private ColumnValor pvColValor;

    public sealed class ColumnValor : TDataColumn {

      public ColumnValor() {
        this.ColumnName = "valor";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "valor";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_penalidade.valor"; } }
      public override string OriginalCol { get { return "valor"; } }
      public override string RefCol { get { return "LY_BIB_PENALIDADE.VALOR"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 6; } }
      public override int Precision { get { return 4; } }
      public override int Scale { get { return 2; } }
      public override string SqlServerType { get { return "NUMERIC(4, 2)"; } }

    }
    #endregion
    #endregion
  }

  public partial class Ly_bib_projeto : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_bib_projeto() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_bib_projeto");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_bib_projeto");
      }
      catch{}
    }

    public Ly_bib_projeto() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColNome_projeto = new ColumnNome_projeto(); Columns.Add(pvColNome_projeto);
      pvColDt_inicial = new ColumnDt_inicial(); Columns.Add(pvColDt_inicial);
      pvColDt_final = new ColumnDt_final(); Columns.Add(pvColDt_final);
      pvColResponsavel = new ColumnResponsavel(); Columns.Add(pvColResponsavel);
      pvColObjetivo = new ColumnObjetivo(); Columns.Add(pvColObjetivo);
      pvColResultado = new ColumnResultado(); Columns.Add(pvColResultado);
      pvColId_bib_biblioteca = new ColumnId_bib_biblioteca(); Columns.Add(pvColId_bib_biblioteca);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_bib_projeto");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @nome_projeto, decimal? @id_bib_biblioteca) {
      Row row = NewRow();

      row.Nome_projeto = @nome_projeto;
      row.Id_bib_biblioteca = @id_bib_biblioteca;

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
      get { return "LY_BIB_PROJETO"; }
    }

    protected override sealed string Name { get { return "Projeto"; } }

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
        case CommandOperation.Select: cm.CommandText = "zLy_bib_projeto_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_bib_projeto_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_bib_projeto_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_bib_projeto_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("nome_projeto", OleDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "NOME_PROJETO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("dt_inicial", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_INICIAL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_final", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_FINAL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("responsavel", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "RESPONSAVEL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("objetivo", OleDbType.VarChar, 600, ParameterDirection.InputOutput, true, 0, 0, "OBJETIVO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("resultado", OleDbType.VarChar, 600, ParameterDirection.InputOutput, true, 0, 0, "RESULTADO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("id_bib_biblioteca", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_BIB_BIBLIOTECA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "nome_projeto") { result.Add(toLower ? "nome_projeto" : column); continue; }
          if(lower == "dt_inicial") { result.Add(toLower ? "dt_inicial" : column); continue; }
          if(lower == "dt_final") { result.Add(toLower ? "dt_final" : column); continue; }
          if(lower == "responsavel") { result.Add(toLower ? "responsavel" : column); continue; }
          if(lower == "objetivo") { result.Add(toLower ? "objetivo" : column); continue; }
          if(lower == "resultado") { result.Add(toLower ? "resultado" : column); continue; }
          if(lower == "id_bib_biblioteca") { result.Add(toLower ? "id_bib_biblioteca" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "nome_projeto", "dt_inicial", "dt_final", "responsavel", "objetivo", 
                                           "resultado", "id_bib_biblioteca" }
                          : new string[] { "ID", "NOME_PROJETO", "DT_INICIAL", "DT_FINAL", "RESPONSAVEL", "OBJETIVO", 
                                           "RESULTADO", "ID_BIB_BIBLIOTECA" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "nome_projeto", "dt_inicial", "dt_final", "responsavel", "objetivo", 
                                           "resultado", "id_bib_biblioteca" }
                          : new string[] { "ID", "NOME_PROJETO", "DT_INICIAL", "DT_FINAL", "RESPONSAVEL", "OBJETIVO", 
                                           "RESULTADO", "ID_BIB_BIBLIOTECA" });

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
        "ly_bib_projeto.id"
      });
      if(scColumns.Contains("nome_projeto")) list.Add("ly_bib_projeto.nome_projeto");
      if(scColumns.Contains("dt_inicial")) list.Add("ly_bib_projeto.dt_inicial");
      if(scColumns.Contains("dt_final")) list.Add("ly_bib_projeto.dt_final");
      if(scColumns.Contains("responsavel")) list.Add("ly_bib_projeto.responsavel");
      if(scColumns.Contains("objetivo")) list.Add("ly_bib_projeto.objetivo");
      if(scColumns.Contains("resultado")) list.Add("ly_bib_projeto.resultado");
      if(scColumns.Contains("id_bib_biblioteca")) list.Add("ly_bib_projeto.id_bib_biblioteca");
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
          "FROM ly_bib_projeto " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_bib_projeto");
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
        da.TableMappings.Add("Table", "Ly_bib_projeto");
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
            if (dr["NOME_PROJETO"] != null && string.IsNullOrEmpty(dr["NOME_PROJETO"].ToString()))
              dr["NOME_PROJETO"] = DBNull.Value;
            if (dr["RESPONSAVEL"] != null && string.IsNullOrEmpty(dr["RESPONSAVEL"].ToString()))
              dr["RESPONSAVEL"] = DBNull.Value;
            if (dr["OBJETIVO"] != null && string.IsNullOrEmpty(dr["OBJETIVO"].ToString()))
              dr["OBJETIVO"] = DBNull.Value;
            if (dr["RESULTADO"] != null && string.IsNullOrEmpty(dr["RESULTADO"].ToString()))
              dr["RESULTADO"] = DBNull.Value;
            if (dr["ID_BIB_BIBLIOTECA"] != null && string.IsNullOrEmpty(dr["ID_BIB_BIBLIOTECA"].ToString()))
              dr["ID_BIB_BIBLIOTECA"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["NOME_PROJETO"] == null || string.IsNullOrEmpty(dr["NOME_PROJETO"].ToString()))
            {
              return "Favor informar o nome da disciplina / projeto.";
            }

            if (dr["NOME_PROJETO"] != null && !string.IsNullOrEmpty(dr["NOME_PROJETO"].ToString()))
            {
              if (dr["NOME_PROJETO"].ToString().Length > 200)
              {
                return "O nome da disciplina / projeto deve possuir no máximo 200 caracteres.";
              }
            }

            if (dr["DT_INICIAL"] != null && dr["DT_INICIAL"].ToString() != "")
            {
              dr["DT_INICIAL"] = Convert.ToDateTime(dr["DT_INICIAL"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_FINAL"] != null && dr["DT_FINAL"].ToString() != "")
            {
              dr["DT_FINAL"] = Convert.ToDateTime(dr["DT_FINAL"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["RESPONSAVEL"] != null && !string.IsNullOrEmpty(dr["RESPONSAVEL"].ToString()))
            {
              if (dr["RESPONSAVEL"].ToString().Length > 100)
              {
                return "O responsável deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["OBJETIVO"] != null && !string.IsNullOrEmpty(dr["OBJETIVO"].ToString()))
            {
              if (dr["OBJETIVO"].ToString().Length > 500)
              {
                return "O objetivo deve possuir no máximo 500 caracteres.";
              }
            }

            if (dr["RESULTADO"] != null && !string.IsNullOrEmpty(dr["RESULTADO"].ToString()))
            {
              if (dr["RESULTADO"].ToString().Length > 500)
              {
                return "O resultado deve possuir no máximo 500 caracteres.";
              }
            }

            if (dr["ID_BIB_BIBLIOTECA"] == null || string.IsNullOrEmpty(dr["ID_BIB_BIBLIOTECA"].ToString()))
            {
              return "Favor informar a biblioteca.";
            }

            if (dr["ID_BIB_BIBLIOTECA"] != null && !string.IsNullOrEmpty(dr["ID_BIB_BIBLIOTECA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_BIB_BIBLIOTECA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_BIBLIOTECA"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_BIB_BIBLIOTECA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_BIBLIOTECA"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A biblioteca deve ser um valor inteiro com 14 dígitos no máximo.";
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

    public static Ly_bib_projeto Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_bib_projeto Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_bib_projeto tab = new Ly_bib_projeto();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_bib_projeto tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, string @nome_projeto, DateTime? @dt_inicial, DateTime? @dt_final, string @responsavel, 
        string @objetivo, string @resultado, decimal? @id_bib_biblioteca
      ) {
        return (Row)Add(new object[] {
          @id, @nome_projeto, @dt_inicial, @dt_final, @responsavel, @objetivo, @resultado, 
          @id_bib_biblioteca
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
      /// NOME_PROJETO: VARCHAR(200), Not Null
      /// </summary>
      public string Nome_projeto { get { return (string)this["nome_projeto"]; } set { this["nome_projeto"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_INICIAL: DATETIME
      /// </summary>
      public DateTime? Dt_inicial { get { return (DateTime?)this["dt_inicial"]; } set { this["dt_inicial"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_FINAL: DATETIME
      /// </summary>
      public DateTime? Dt_final { get { return (DateTime?)this["dt_final"]; } set { this["dt_final"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// RESPONSAVEL: VARCHAR(100)
      /// </summary>
      public string Responsavel { get { return (string)this["responsavel"]; } set { this["responsavel"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// OBJETIVO: VARCHAR(500)
      /// </summary>
      public string Objetivo { get { return (string)this["objetivo"]; } set { this["objetivo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// RESULTADO: VARCHAR(500)
      /// </summary>
      public string Resultado { get { return (string)this["resultado"]; } set { this["resultado"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ID_BIB_BIBLIOTECA: NUMERIC(14), Not Null
      /// </summary>
      public decimal? Id_bib_biblioteca { get { return (decimal?)this["id_bib_biblioteca"]; } set { this["id_bib_biblioteca"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @nome_projeto, decimal? @id_bib_biblioteca) {
        return Insert(connection,
                      @nome_projeto, @id_bib_biblioteca,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @nome_projeto, decimal? @id_bib_biblioteca,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_bib_projeto(),
          new string[] { "nome_projeto", "id_bib_biblioteca" },
          new object[] { @nome_projeto, @id_bib_biblioteca },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_bib_projeto(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_bib_projeto(),
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
          connection, new Techne.Lyceum.CR.Ly_bib_projeto(),
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
        public string Nome_projeto { get { return (string)row["nome_projeto", DataRowVersion.Original]; } }
        public DateTime? Dt_inicial { get { return (DateTime?)row["dt_inicial", DataRowVersion.Original]; } }
        public DateTime? Dt_final { get { return (DateTime?)row["dt_final", DataRowVersion.Original]; } }
        public string Responsavel { get { return (string)row["responsavel", DataRowVersion.Original]; } }
        public string Objetivo { get { return (string)row["objetivo", DataRowVersion.Original]; } }
        public string Resultado { get { return (string)row["resultado", DataRowVersion.Original]; } }
        public decimal? Id_bib_biblioteca { get { return (decimal?)row["id_bib_biblioteca", DataRowVersion.Original]; } }

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


      protected override string GetName(){return "identificador";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_projeto.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_BIB_PROJETO.ID"; } }
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

    #region Coluna: NOME_PROJETO
    private ColumnNome_projeto pvColNome_projeto;

    public sealed class ColumnNome_projeto : TDataColumn {

      public ColumnNome_projeto() {
        this.ColumnName = "nome_projeto";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "nome da disciplina / projeto";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_projeto.nome_projeto"; } }
      public override string OriginalCol { get { return "nome_projeto"; } }
      public override string RefCol { get { return "LY_BIB_PROJETO.NOME_PROJETO"; } }
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

    #region Coluna: DT_INICIAL
    private ColumnDt_inicial pvColDt_inicial;

    public sealed class ColumnDt_inicial : TDataColumn {

      public ColumnDt_inicial() {
        this.ColumnName = "dt_inicial";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data inicial";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_projeto.dt_inicial"; } }
      public override string OriginalCol { get { return "dt_inicial"; } }
      public override string RefCol { get { return "LY_BIB_PROJETO.DT_INICIAL"; } }
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

    #region Coluna: DT_FINAL
    private ColumnDt_final pvColDt_final;

    public sealed class ColumnDt_final : TDataColumn {

      public ColumnDt_final() {
        this.ColumnName = "dt_final";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data final";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_projeto.dt_final"; } }
      public override string OriginalCol { get { return "dt_final"; } }
      public override string RefCol { get { return "LY_BIB_PROJETO.DT_FINAL"; } }
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

    #region Coluna: RESPONSAVEL
    private ColumnResponsavel pvColResponsavel;

    public sealed class ColumnResponsavel : TDataColumn {

      public ColumnResponsavel() {
        this.ColumnName = "responsavel";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "responsável";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_projeto.responsavel"; } }
      public override string OriginalCol { get { return "responsavel"; } }
      public override string RefCol { get { return "LY_BIB_PROJETO.RESPONSAVEL"; } }
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

    #region Coluna: OBJETIVO
    private ColumnObjetivo pvColObjetivo;

    public sealed class ColumnObjetivo : TDataColumn {

      public ColumnObjetivo() {
        this.ColumnName = "objetivo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "objetivo";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_projeto.objetivo"; } }
      public override string OriginalCol { get { return "objetivo"; } }
      public override string RefCol { get { return "LY_BIB_PROJETO.OBJETIVO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 500; } }
      public override string SqlServerType { get { return "VARCHAR(500)"; } }

    }
    #endregion

    #region Coluna: RESULTADO
    private ColumnResultado pvColResultado;

    public sealed class ColumnResultado : TDataColumn {

      public ColumnResultado() {
        this.ColumnName = "resultado";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "resultado";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_projeto.resultado"; } }
      public override string OriginalCol { get { return "resultado"; } }
      public override string RefCol { get { return "LY_BIB_PROJETO.RESULTADO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 500; } }
      public override string SqlServerType { get { return "VARCHAR(500)"; } }

    }
    #endregion

    #region Coluna: ID_BIB_BIBLIOTECA
    private ColumnId_bib_biblioteca pvColId_bib_biblioteca;

    public sealed class ColumnId_bib_biblioteca : TDataColumn {

      public ColumnId_bib_biblioteca() {
        this.ColumnName = "id_bib_biblioteca";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "biblioteca";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_projeto.id_bib_biblioteca"; } }
      public override string OriginalCol { get { return "id_bib_biblioteca"; } }
      public override string RefCol { get { return "LY_BIB_PROJETO.ID_BIB_BIBLIOTECA"; } }
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
    #endregion
  }

  public partial class Ly_bib_reserva : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_bib_reserva() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_bib_reserva");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_bib_reserva");
      }
      catch{}
    }

    public Ly_bib_reserva() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColId_bib_usuario = new ColumnId_bib_usuario(); Columns.Add(pvColId_bib_usuario);
      pvColId_bib_material = new ColumnId_bib_material(); Columns.Add(pvColId_bib_material);
      pvColDt_validade = new ColumnDt_validade(); Columns.Add(pvColDt_validade);
      pvColDt_reserva = new ColumnDt_reserva(); Columns.Add(pvColDt_reserva);
      pvColId_bib_titulo = new ColumnId_bib_titulo(); Columns.Add(pvColId_bib_titulo);
      pvColLocal = new ColumnLocal(); Columns.Add(pvColLocal);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_bib_reserva");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(decimal? @id_bib_usuario, DateTime? @dt_reserva) {
      Row row = NewRow();

      row.Id_bib_usuario = @id_bib_usuario;
      row.Dt_reserva = @dt_reserva;

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
      get { return "LY_BIB_RESERVA"; }
    }

    protected override sealed string Name { get { return "Reserva"; } }

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
        case CommandOperation.Select: cm.CommandText = "zLy_bib_reserva_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_bib_reserva_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_bib_reserva_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_bib_reserva_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("id_bib_usuario", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_BIB_USUARIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("id_bib_material", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_BIB_MATERIAL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("dt_validade", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_VALIDADE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_reserva", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_RESERVA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("id_bib_titulo", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_BIB_TITULO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("local", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "LOCAL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "id_bib_usuario") { result.Add(toLower ? "id_bib_usuario" : column); continue; }
          if(lower == "id_bib_material") { result.Add(toLower ? "id_bib_material" : column); continue; }
          if(lower == "dt_validade") { result.Add(toLower ? "dt_validade" : column); continue; }
          if(lower == "dt_reserva") { result.Add(toLower ? "dt_reserva" : column); continue; }
          if(lower == "id_bib_titulo") { result.Add(toLower ? "id_bib_titulo" : column); continue; }
          if(lower == "local") { result.Add(toLower ? "local" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "id_bib_usuario", "id_bib_material", "dt_validade", "dt_reserva", 
                                           "id_bib_titulo", "local" }
                          : new string[] { "ID", "ID_BIB_USUARIO", "ID_BIB_MATERIAL", "DT_VALIDADE", "DT_RESERVA", 
                                           "ID_BIB_TITULO", "LOCAL" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "id_bib_usuario", "id_bib_material", "dt_validade", "dt_reserva", 
                                           "id_bib_titulo", "local" }
                          : new string[] { "ID", "ID_BIB_USUARIO", "ID_BIB_MATERIAL", "DT_VALIDADE", "DT_RESERVA", 
                                           "ID_BIB_TITULO", "LOCAL" });

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
        "ly_bib_reserva.id"
      });
      if(scColumns.Contains("id_bib_usuario")) list.Add("ly_bib_reserva.id_bib_usuario");
      if(scColumns.Contains("id_bib_material")) list.Add("ly_bib_reserva.id_bib_material");
      if(scColumns.Contains("dt_validade")) list.Add("ly_bib_reserva.dt_validade");
      if(scColumns.Contains("dt_reserva")) list.Add("ly_bib_reserva.dt_reserva");
      if(scColumns.Contains("id_bib_titulo")) list.Add("ly_bib_reserva.id_bib_titulo");
      if(scColumns.Contains("local")) list.Add("ly_bib_reserva.local");
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
          "FROM ly_bib_reserva " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_bib_reserva");
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
        da.TableMappings.Add("Table", "Ly_bib_reserva");
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
            if (dr["ID_BIB_USUARIO"] != null && string.IsNullOrEmpty(dr["ID_BIB_USUARIO"].ToString()))
              dr["ID_BIB_USUARIO"] = DBNull.Value;
            if (dr["ID_BIB_MATERIAL"] != null && string.IsNullOrEmpty(dr["ID_BIB_MATERIAL"].ToString()))
              dr["ID_BIB_MATERIAL"] = DBNull.Value;
            if (dr["ID_BIB_TITULO"] != null && string.IsNullOrEmpty(dr["ID_BIB_TITULO"].ToString()))
              dr["ID_BIB_TITULO"] = DBNull.Value;
            if (dr["LOCAL"] != null && string.IsNullOrEmpty(dr["LOCAL"].ToString()))
              dr["LOCAL"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["ID_BIB_USUARIO"] == null || string.IsNullOrEmpty(dr["ID_BIB_USUARIO"].ToString()))
            {
              return "Favor informar o usuário.";
            }

            if (dr["ID_BIB_USUARIO"] != null && !string.IsNullOrEmpty(dr["ID_BIB_USUARIO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_BIB_USUARIO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_USUARIO"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_BIB_USUARIO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_USUARIO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O usuário deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["ID_BIB_MATERIAL"] != null && !string.IsNullOrEmpty(dr["ID_BIB_MATERIAL"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_BIB_MATERIAL"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_MATERIAL"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_BIB_MATERIAL"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_MATERIAL"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O material deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["DT_VALIDADE"] != null && dr["DT_VALIDADE"].ToString() != "")
            {
              dr["DT_VALIDADE"] = Convert.ToDateTime(dr["DT_VALIDADE"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_RESERVA"] == null || string.IsNullOrEmpty(dr["DT_RESERVA"].ToString()))
            {
              return "Favor informar a data da reserva.";
            }

            if (dr["DT_RESERVA"] != null && dr["DT_RESERVA"].ToString() != "")
            {
              dr["DT_RESERVA"] = Convert.ToDateTime(dr["DT_RESERVA"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["ID_BIB_TITULO"] != null && !string.IsNullOrEmpty(dr["ID_BIB_TITULO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_BIB_TITULO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_TITULO"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_BIB_TITULO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_TITULO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O título deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["LOCAL"] != null && !string.IsNullOrEmpty(dr["LOCAL"].ToString()))
            {
              if ((Convert.ToDecimal(dr["LOCAL"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["LOCAL"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["LOCAL"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["LOCAL"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O local deve ser um valor inteiro com 14 dígitos no máximo.";
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

    public static Ly_bib_reserva Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_bib_reserva Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_bib_reserva tab = new Ly_bib_reserva();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_bib_reserva tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, decimal? @id_bib_usuario, decimal? @id_bib_material, DateTime? @dt_validade, 
        DateTime? @dt_reserva, decimal? @id_bib_titulo, decimal? @local
      ) {
        return (Row)Add(new object[] {
          @id, @id_bib_usuario, @id_bib_material, @dt_validade, @dt_reserva, @id_bib_titulo, 
          @local
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
      /// ID_BIB_USUARIO: NUMERIC(14), Not Null
      /// </summary>
      public decimal? Id_bib_usuario { get { return (decimal?)this["id_bib_usuario"]; } set { this["id_bib_usuario"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ID_BIB_MATERIAL: NUMERIC(14)
      /// </summary>
      public decimal? Id_bib_material { get { return (decimal?)this["id_bib_material"]; } set { this["id_bib_material"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_VALIDADE: DATETIME
      /// </summary>
      public DateTime? Dt_validade { get { return (DateTime?)this["dt_validade"]; } set { this["dt_validade"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_RESERVA: DATETIME, Not Null
      /// </summary>
      public DateTime? Dt_reserva { get { return (DateTime?)this["dt_reserva"]; } set { this["dt_reserva"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ID_BIB_TITULO: NUMERIC(14)
      /// </summary>
      public decimal? Id_bib_titulo { get { return (decimal?)this["id_bib_titulo"]; } set { this["id_bib_titulo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// LOCAL: NUMERIC(14)
      /// </summary>
      public decimal? Local { get { return (decimal?)this["local"]; } set { this["local"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               decimal? @id_bib_usuario, DateTime? @dt_reserva) {
        return Insert(connection,
                      @id_bib_usuario, @dt_reserva,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               decimal? @id_bib_usuario, DateTime? @dt_reserva,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_bib_reserva(),
          new string[] { "id_bib_usuario", "dt_reserva" },
          new object[] { @id_bib_usuario, @dt_reserva },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_bib_reserva(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_bib_reserva(),
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
          connection, new Techne.Lyceum.CR.Ly_bib_reserva(),
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
        public decimal? Id_bib_usuario { get { return (decimal?)row["id_bib_usuario", DataRowVersion.Original]; } }
        public decimal? Id_bib_material { get { return (decimal?)row["id_bib_material", DataRowVersion.Original]; } }
        public DateTime? Dt_validade { get { return (DateTime?)row["dt_validade", DataRowVersion.Original]; } }
        public DateTime? Dt_reserva { get { return (DateTime?)row["dt_reserva", DataRowVersion.Original]; } }
        public decimal? Id_bib_titulo { get { return (decimal?)row["id_bib_titulo", DataRowVersion.Original]; } }
        public decimal? Local { get { return (decimal?)row["local", DataRowVersion.Original]; } }

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


      protected override string GetName(){return "identificador";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_reserva.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_BIB_RESERVA.ID"; } }
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

    #region Coluna: ID_BIB_USUARIO
    private ColumnId_bib_usuario pvColId_bib_usuario;

    public sealed class ColumnId_bib_usuario : TDataColumn {

      public ColumnId_bib_usuario() {
        this.ColumnName = "id_bib_usuario";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "usuário";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_reserva.id_bib_usuario"; } }
      public override string OriginalCol { get { return "id_bib_usuario"; } }
      public override string RefCol { get { return "LY_BIB_RESERVA.ID_BIB_USUARIO"; } }
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

    #region Coluna: ID_BIB_MATERIAL
    private ColumnId_bib_material pvColId_bib_material;

    public sealed class ColumnId_bib_material : TDataColumn {

      public ColumnId_bib_material() {
        this.ColumnName = "id_bib_material";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "material";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_reserva.id_bib_material"; } }
      public override string OriginalCol { get { return "id_bib_material"; } }
      public override string RefCol { get { return "LY_BIB_RESERVA.ID_BIB_MATERIAL"; } }
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

    #region Coluna: DT_VALIDADE
    private ColumnDt_validade pvColDt_validade;

    public sealed class ColumnDt_validade : TDataColumn {

      public ColumnDt_validade() {
        this.ColumnName = "dt_validade";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data da validade";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_reserva.dt_validade"; } }
      public override string OriginalCol { get { return "dt_validade"; } }
      public override string RefCol { get { return "LY_BIB_RESERVA.DT_VALIDADE"; } }
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

    #region Coluna: DT_RESERVA
    private ColumnDt_reserva pvColDt_reserva;

    public sealed class ColumnDt_reserva : TDataColumn {

      public ColumnDt_reserva() {
        this.ColumnName = "dt_reserva";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data da reserva";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_reserva.dt_reserva"; } }
      public override string OriginalCol { get { return "dt_reserva"; } }
      public override string RefCol { get { return "LY_BIB_RESERVA.DT_RESERVA"; } }
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

    #region Coluna: ID_BIB_TITULO
    private ColumnId_bib_titulo pvColId_bib_titulo;

    public sealed class ColumnId_bib_titulo : TDataColumn {

      public ColumnId_bib_titulo() {
        this.ColumnName = "id_bib_titulo";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "título";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_reserva.id_bib_titulo"; } }
      public override string OriginalCol { get { return "id_bib_titulo"; } }
      public override string RefCol { get { return "LY_BIB_RESERVA.ID_BIB_TITULO"; } }
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

    #region Coluna: LOCAL
    private ColumnLocal pvColLocal;

    public sealed class ColumnLocal : TDataColumn {

      public ColumnLocal() {
        this.ColumnName = "local";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "local";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_reserva.local"; } }
      public override string OriginalCol { get { return "local"; } }
      public override string RefCol { get { return "LY_BIB_RESERVA.LOCAL"; } }
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
    #endregion
  }

  public partial class Ly_bib_sugestao : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_bib_sugestao() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_bib_sugestao");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_bib_sugestao");
      }
      catch{}
    }

    public Ly_bib_sugestao() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColTitulo = new ColumnTitulo(); Columns.Add(pvColTitulo);
      pvColAutor = new ColumnAutor(); Columns.Add(pvColAutor);
      pvColEditora = new ColumnEditora(); Columns.Add(pvColEditora);
      pvColAno = new ColumnAno(); Columns.Add(pvColAno);
      pvColObservacoes = new ColumnObservacoes(); Columns.Add(pvColObservacoes);
      pvColId_bib_usuario = new ColumnId_bib_usuario(); Columns.Add(pvColId_bib_usuario);
      pvColData = new ColumnData(); Columns.Add(pvColData);
      pvColId_bib_biblioteca = new ColumnId_bib_biblioteca(); Columns.Add(pvColId_bib_biblioteca);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_bib_sugestao");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @titulo, string @autor, decimal? @id_bib_usuario, DateTime? @data, 
                      decimal? @id_bib_biblioteca) {
      Row row = NewRow();

      row.Titulo = @titulo;
      row.Autor = @autor;
      row.Id_bib_usuario = @id_bib_usuario;
      row.Data = @data;
      row.Id_bib_biblioteca = @id_bib_biblioteca;

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
      get { return "LY_BIB_SUGESTAO"; }
    }

    protected override sealed string Name { get { return "Sugestão"; } }

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
        case CommandOperation.Select: cm.CommandText = "zLy_bib_sugestao_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_bib_sugestao_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_bib_sugestao_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_bib_sugestao_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("titulo", OleDbType.VarChar, 600, ParameterDirection.InputOutput, true, 0, 0, "TITULO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("autor", OleDbType.VarChar, 600, ParameterDirection.InputOutput, true, 0, 0, "AUTOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("editora", OleDbType.VarChar, 600, ParameterDirection.InputOutput, true, 0, 0, "EDITORA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("ano", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ANO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("observacoes", OleDbType.VarChar, 600, ParameterDirection.InputOutput, true, 0, 0, "OBSERVACOES", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("id_bib_usuario", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_BIB_USUARIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("data", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DATA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("id_bib_biblioteca", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_BIB_BIBLIOTECA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "titulo") { result.Add(toLower ? "titulo" : column); continue; }
          if(lower == "autor") { result.Add(toLower ? "autor" : column); continue; }
          if(lower == "editora") { result.Add(toLower ? "editora" : column); continue; }
          if(lower == "ano") { result.Add(toLower ? "ano" : column); continue; }
          if(lower == "observacoes") { result.Add(toLower ? "observacoes" : column); continue; }
          if(lower == "id_bib_usuario") { result.Add(toLower ? "id_bib_usuario" : column); continue; }
          if(lower == "data") { result.Add(toLower ? "data" : column); continue; }
          if(lower == "id_bib_biblioteca") { result.Add(toLower ? "id_bib_biblioteca" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "titulo", "autor", "editora", "ano", "observacoes", "id_bib_usuario", 
                                           "data", "id_bib_biblioteca" }
                          : new string[] { "ID", "TITULO", "AUTOR", "EDITORA", "ANO", "OBSERVACOES", "ID_BIB_USUARIO", 
                                           "DATA", "ID_BIB_BIBLIOTECA" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "titulo", "autor", "editora", "ano", "observacoes", "id_bib_usuario", 
                                           "data", "id_bib_biblioteca" }
                          : new string[] { "ID", "TITULO", "AUTOR", "EDITORA", "ANO", "OBSERVACOES", "ID_BIB_USUARIO", 
                                           "DATA", "ID_BIB_BIBLIOTECA" });

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
        "ly_bib_sugestao.id"
      });
      if(scColumns.Contains("titulo")) list.Add("ly_bib_sugestao.titulo");
      if(scColumns.Contains("autor")) list.Add("ly_bib_sugestao.autor");
      if(scColumns.Contains("editora")) list.Add("ly_bib_sugestao.editora");
      if(scColumns.Contains("ano")) list.Add("ly_bib_sugestao.ano");
      if(scColumns.Contains("observacoes")) list.Add("ly_bib_sugestao.observacoes");
      if(scColumns.Contains("id_bib_usuario")) list.Add("ly_bib_sugestao.id_bib_usuario");
      if(scColumns.Contains("data")) list.Add("ly_bib_sugestao.data");
      if(scColumns.Contains("id_bib_biblioteca")) list.Add("ly_bib_sugestao.id_bib_biblioteca");
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
          "FROM ly_bib_sugestao " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_bib_sugestao");
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
        da.TableMappings.Add("Table", "Ly_bib_sugestao");
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
            if (dr["TITULO"] != null && string.IsNullOrEmpty(dr["TITULO"].ToString()))
              dr["TITULO"] = DBNull.Value;
            if (dr["AUTOR"] != null && string.IsNullOrEmpty(dr["AUTOR"].ToString()))
              dr["AUTOR"] = DBNull.Value;
            if (dr["EDITORA"] != null && string.IsNullOrEmpty(dr["EDITORA"].ToString()))
              dr["EDITORA"] = DBNull.Value;
            if (dr["ANO"] != null && string.IsNullOrEmpty(dr["ANO"].ToString()))
              dr["ANO"] = DBNull.Value;
            if (dr["OBSERVACOES"] != null && string.IsNullOrEmpty(dr["OBSERVACOES"].ToString()))
              dr["OBSERVACOES"] = DBNull.Value;
            if (dr["ID_BIB_USUARIO"] != null && string.IsNullOrEmpty(dr["ID_BIB_USUARIO"].ToString()))
              dr["ID_BIB_USUARIO"] = DBNull.Value;
            if (dr["ID_BIB_BIBLIOTECA"] != null && string.IsNullOrEmpty(dr["ID_BIB_BIBLIOTECA"].ToString()))
              dr["ID_BIB_BIBLIOTECA"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["TITULO"] == null || string.IsNullOrEmpty(dr["TITULO"].ToString()))
            {
              return "Favor informar o título.";
            }

            if (dr["TITULO"] != null && !string.IsNullOrEmpty(dr["TITULO"].ToString()))
            {
              if (dr["TITULO"].ToString().Length > 500)
              {
                return "O título deve possuir no máximo 500 caracteres.";
              }
            }

            if (dr["AUTOR"] == null || string.IsNullOrEmpty(dr["AUTOR"].ToString()))
            {
              return "Favor informar o autor.";
            }

            if (dr["AUTOR"] != null && !string.IsNullOrEmpty(dr["AUTOR"].ToString()))
            {
              if (dr["AUTOR"].ToString().Length > 500)
              {
                return "O autor deve possuir no máximo 500 caracteres.";
              }
            }

            if (dr["EDITORA"] != null && !string.IsNullOrEmpty(dr["EDITORA"].ToString()))
            {
              if (dr["EDITORA"].ToString().Length > 500)
              {
                return "A editora deve possuir no máximo 500 caracteres.";
              }
            }

            if (dr["ANO"] != null && !string.IsNullOrEmpty(dr["ANO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ANO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ANO"]).ToString("0.############").Split(',')[0].Length > 4) || (Convert.ToDecimal(dr["ANO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ANO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O ano deve ser um valor inteiro com 4 dígitos no máximo.";
              }
            }

            if (dr["OBSERVACOES"] != null && !string.IsNullOrEmpty(dr["OBSERVACOES"].ToString()))
            {
              if (dr["OBSERVACOES"].ToString().Length > 500)
              {
                return "As observações devem possuir no máximo 500 caracteres.";
              }
            }

            if (dr["ID_BIB_USUARIO"] == null || string.IsNullOrEmpty(dr["ID_BIB_USUARIO"].ToString()))
            {
              return "Favor informar o usuário.";
            }

            if (dr["ID_BIB_USUARIO"] != null && !string.IsNullOrEmpty(dr["ID_BIB_USUARIO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_BIB_USUARIO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_USUARIO"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_BIB_USUARIO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_USUARIO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O usuário deve ser um valor inteiro com 14 dígitos no máximo.";
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

            if (dr["ID_BIB_BIBLIOTECA"] == null || string.IsNullOrEmpty(dr["ID_BIB_BIBLIOTECA"].ToString()))
            {
              return "Favor informar a biblioteca.";
            }

            if (dr["ID_BIB_BIBLIOTECA"] != null && !string.IsNullOrEmpty(dr["ID_BIB_BIBLIOTECA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_BIB_BIBLIOTECA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_BIBLIOTECA"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_BIB_BIBLIOTECA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_BIBLIOTECA"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A biblioteca deve ser um valor inteiro com 14 dígitos no máximo.";
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

    public static Ly_bib_sugestao Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_bib_sugestao Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_bib_sugestao tab = new Ly_bib_sugestao();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_bib_sugestao tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, string @titulo, string @autor, string @editora, decimal? @ano, string @observacoes, 
        decimal? @id_bib_usuario, DateTime? @data, decimal? @id_bib_biblioteca
      ) {
        return (Row)Add(new object[] {
          @id, @titulo, @autor, @editora, @ano, @observacoes, @id_bib_usuario, @data, @id_bib_biblioteca
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
      /// TITULO: VARCHAR(500), Not Null
      /// </summary>
      public string Titulo { get { return (string)this["titulo"]; } set { this["titulo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// AUTOR: VARCHAR(500), Not Null
      /// </summary>
      public string Autor { get { return (string)this["autor"]; } set { this["autor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// EDITORA: VARCHAR(500)
      /// </summary>
      public string Editora { get { return (string)this["editora"]; } set { this["editora"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ANO: NUMERIC(4)
      /// </summary>
      public decimal? Ano { get { return (decimal?)this["ano"]; } set { this["ano"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// OBSERVACOES: VARCHAR(500)
      /// </summary>
      public string Observacoes { get { return (string)this["observacoes"]; } set { this["observacoes"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ID_BIB_USUARIO: NUMERIC(14), Not Null
      /// </summary>
      public decimal? Id_bib_usuario { get { return (decimal?)this["id_bib_usuario"]; } set { this["id_bib_usuario"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DATA: DATETIME, Not Null
      /// </summary>
      public DateTime? Data { get { return (DateTime?)this["data"]; } set { this["data"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ID_BIB_BIBLIOTECA: NUMERIC(14), Not Null
      /// </summary>
      public decimal? Id_bib_biblioteca { get { return (decimal?)this["id_bib_biblioteca"]; } set { this["id_bib_biblioteca"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @titulo, string @autor, decimal? @id_bib_usuario, DateTime? @data, decimal? @id_bib_biblioteca) {
        return Insert(connection,
                      @titulo, @autor, @id_bib_usuario, @data, @id_bib_biblioteca,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @titulo, string @autor, decimal? @id_bib_usuario, DateTime? @data, decimal? @id_bib_biblioteca,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_bib_sugestao(),
          new string[] { "titulo", "autor", "id_bib_usuario", "data", "id_bib_biblioteca" },
          new object[] { @titulo, @autor, @id_bib_usuario, @data, @id_bib_biblioteca },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_bib_sugestao(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_bib_sugestao(),
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
          connection, new Techne.Lyceum.CR.Ly_bib_sugestao(),
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
        public string Titulo { get { return (string)row["titulo", DataRowVersion.Original]; } }
        public string Autor { get { return (string)row["autor", DataRowVersion.Original]; } }
        public string Editora { get { return (string)row["editora", DataRowVersion.Original]; } }
        public decimal? Ano { get { return (decimal?)row["ano", DataRowVersion.Original]; } }
        public string Observacoes { get { return (string)row["observacoes", DataRowVersion.Original]; } }
        public decimal? Id_bib_usuario { get { return (decimal?)row["id_bib_usuario", DataRowVersion.Original]; } }
        public DateTime? Data { get { return (DateTime?)row["data", DataRowVersion.Original]; } }
        public decimal? Id_bib_biblioteca { get { return (decimal?)row["id_bib_biblioteca", DataRowVersion.Original]; } }

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


      protected override string GetName(){return "identificador";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_sugestao.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_BIB_SUGESTAO.ID"; } }
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

    #region Coluna: TITULO
    private ColumnTitulo pvColTitulo;

    public sealed class ColumnTitulo : TDataColumn {

      public ColumnTitulo() {
        this.ColumnName = "titulo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "título";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_sugestao.titulo"; } }
      public override string OriginalCol { get { return "titulo"; } }
      public override string RefCol { get { return "LY_BIB_SUGESTAO.TITULO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 500; } }
      public override string SqlServerType { get { return "VARCHAR(500)"; } }

    }
    #endregion

    #region Coluna: AUTOR
    private ColumnAutor pvColAutor;

    public sealed class ColumnAutor : TDataColumn {

      public ColumnAutor() {
        this.ColumnName = "autor";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "autor";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_sugestao.autor"; } }
      public override string OriginalCol { get { return "autor"; } }
      public override string RefCol { get { return "LY_BIB_SUGESTAO.AUTOR"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 500; } }
      public override string SqlServerType { get { return "VARCHAR(500)"; } }

    }
    #endregion

    #region Coluna: EDITORA
    private ColumnEditora pvColEditora;

    public sealed class ColumnEditora : TDataColumn {

      public ColumnEditora() {
        this.ColumnName = "editora";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "editora";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_sugestao.editora"; } }
      public override string OriginalCol { get { return "editora"; } }
      public override string RefCol { get { return "LY_BIB_SUGESTAO.EDITORA"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 500; } }
      public override string SqlServerType { get { return "VARCHAR(500)"; } }

    }
    #endregion

    #region Coluna: ANO
    private ColumnAno pvColAno;

    public sealed class ColumnAno : TDataColumn {

      public ColumnAno() {
        this.ColumnName = "ano";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "ano";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_sugestao.ano"; } }
      public override string OriginalCol { get { return "ano"; } }
      public override string RefCol { get { return "LY_BIB_SUGESTAO.ANO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 5; } }
      public override int Precision { get { return 4; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(4)"; } }

    }
    #endregion

    #region Coluna: OBSERVACOES
    private ColumnObservacoes pvColObservacoes;

    public sealed class ColumnObservacoes : TDataColumn {

      public ColumnObservacoes() {
        this.ColumnName = "observacoes";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "observações";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_sugestao.observacoes"; } }
      public override string OriginalCol { get { return "observacoes"; } }
      public override string RefCol { get { return "LY_BIB_SUGESTAO.OBSERVACOES"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 500; } }
      public override string SqlServerType { get { return "VARCHAR(500)"; } }

    }
    #endregion

    #region Coluna: ID_BIB_USUARIO
    private ColumnId_bib_usuario pvColId_bib_usuario;

    public sealed class ColumnId_bib_usuario : TDataColumn {

      public ColumnId_bib_usuario() {
        this.ColumnName = "id_bib_usuario";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "usuário";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_sugestao.id_bib_usuario"; } }
      public override string OriginalCol { get { return "id_bib_usuario"; } }
      public override string RefCol { get { return "LY_BIB_SUGESTAO.ID_BIB_USUARIO"; } }
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

    #region Coluna: DATA
    private ColumnData pvColData;

    public sealed class ColumnData : TDataColumn {

      public ColumnData() {
        this.ColumnName = "data";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_sugestao.data"; } }
      public override string OriginalCol { get { return "data"; } }
      public override string RefCol { get { return "LY_BIB_SUGESTAO.DATA"; } }
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

    #region Coluna: ID_BIB_BIBLIOTECA
    private ColumnId_bib_biblioteca pvColId_bib_biblioteca;

    public sealed class ColumnId_bib_biblioteca : TDataColumn {

      public ColumnId_bib_biblioteca() {
        this.ColumnName = "id_bib_biblioteca";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "biblioteca";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_sugestao.id_bib_biblioteca"; } }
      public override string OriginalCol { get { return "id_bib_biblioteca"; } }
      public override string RefCol { get { return "LY_BIB_SUGESTAO.ID_BIB_BIBLIOTECA"; } }
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
    #endregion
  }

  public partial class Ly_bib_tipo_material : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_bib_tipo_material() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_bib_tipo_material");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_bib_tipo_material");
      }
      catch{}
    }

    public Ly_bib_tipo_material() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColSigla = new ColumnSigla(); Columns.Add(pvColSigla);
      pvColDescricao = new ColumnDescricao(); Columns.Add(pvColDescricao);
      pvColImagem = new ColumnImagem(); Columns.Add(pvColImagem);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_bib_tipo_material");

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
      get { return "LY_BIB_TIPO_MATERIAL"; }
    }

    protected override sealed string Name { get { return "Tipo de Material"; } }

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
        case CommandOperation.Select: cm.CommandText = "zLy_bib_tipo_material_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_bib_tipo_material_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_bib_tipo_material_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_bib_tipo_material_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("sigla", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SIGLA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("descricao", OleDbType.VarChar, 600, ParameterDirection.InputOutput, true, 0, 0, "DESCRICAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("imagem", OleDbType.VarBinary, 0, ParameterDirection.Input, true, 0, 0, "IMAGEM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("5F5F554E4445465F4C4F4E475241575F5F")));

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
          if(lower == "imagem") { result.Add(toLower ? "imagem" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "sigla", "descricao", "imagem" }
                          : new string[] { "ID", "SIGLA", "DESCRICAO", "IMAGEM" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "sigla", "descricao", "imagem" }
                          : new string[] { "ID", "SIGLA", "DESCRICAO", "IMAGEM" });

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
        "ly_bib_tipo_material.id"
      });
      if(scColumns.Contains("sigla")) list.Add("ly_bib_tipo_material.sigla");
      if(scColumns.Contains("descricao")) list.Add("ly_bib_tipo_material.descricao");
      if(scColumns.Contains("imagem")) list.Add("ly_bib_tipo_material.imagem");
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
          "FROM ly_bib_tipo_material " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_bib_tipo_material");
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
        da.TableMappings.Add("Table", "Ly_bib_tipo_material");
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
              if (dr["DESCRICAO"].ToString().Length > 500)
              {
                return "A descrição deve possuir no máximo 500 caracteres.";
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

    public static Ly_bib_tipo_material Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_bib_tipo_material Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_bib_tipo_material tab = new Ly_bib_tipo_material();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_bib_tipo_material tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, string @sigla, string @descricao, byte[] @imagem
      ) {
        return (Row)Add(new object[] {
          @id, @sigla, @descricao, @imagem
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
      /// DESCRICAO: VARCHAR(500)
      /// </summary>
      public string Descricao { get { return (string)this["descricao"]; } set { this["descricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// IMAGEM: IMAGE
      /// </summary>
      public byte[] Imagem { get { return (byte[])this["imagem"]; } set { this["imagem"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


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
          connection, new Techne.Lyceum.CR.Ly_bib_tipo_material(),
          new string[] { "sigla" },
          new object[] { @sigla },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_bib_tipo_material(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_bib_tipo_material(),
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
          connection, new Techne.Lyceum.CR.Ly_bib_tipo_material(),
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
        public byte[] Imagem { get { return (byte[])row["imagem", DataRowVersion.Original]; } }

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


      protected override string GetName(){return "identificador";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_tipo_material.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_BIB_TIPO_MATERIAL.ID"; } }
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
      public override string FullCol { get { return "ly_bib_tipo_material.sigla"; } }
      public override string OriginalCol { get { return "sigla"; } }
      public override string RefCol { get { return "LY_BIB_TIPO_MATERIAL.SIGLA"; } }
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
      public override string FullCol { get { return "ly_bib_tipo_material.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "LY_BIB_TIPO_MATERIAL.DESCRICAO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 500; } }
      public override string SqlServerType { get { return "VARCHAR(500)"; } }

    }
    #endregion

    #region Coluna: IMAGEM
    private ColumnImagem pvColImagem;

    public sealed class ColumnImagem : TDataColumn {

      public ColumnImagem() {
        this.ColumnName = "imagem";
        this.DataType = typeof(byte[]);

      }


      protected override string GetName(){return "imagem";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_tipo_material.imagem"; } }
      public override string OriginalCol { get { return "imagem"; } }
      public override string RefCol { get { return "LY_BIB_TIPO_MATERIAL.IMAGEM"; } }
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

  public partial class Ly_bib_titulo : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_bib_titulo() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_bib_titulo");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_bib_titulo");
      }
      catch{}
    }

    public Ly_bib_titulo() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColTitulo = new ColumnTitulo(); Columns.Add(pvColTitulo);
      pvColIsbn = new ColumnIsbn(); Columns.Add(pvColIsbn);
      pvColEdicao = new ColumnEdicao(); Columns.Add(pvColEdicao);
      pvColAno = new ColumnAno(); Columns.Add(pvColAno);
      pvColArea = new ColumnArea(); Columns.Add(pvColArea);
      pvColTema = new ColumnTema(); Columns.Add(pvColTema);
      pvColGenero = new ColumnGenero(); Columns.Add(pvColGenero);
      pvColId_bib_editora = new ColumnId_bib_editora(); Columns.Add(pvColId_bib_editora);
      pvColId_bib_tipo_material = new ColumnId_bib_tipo_material(); Columns.Add(pvColId_bib_tipo_material);
      pvColId_bib_assunto_cdd = new ColumnId_bib_assunto_cdd(); Columns.Add(pvColId_bib_assunto_cdd);
      pvColImagem = new ColumnImagem(); Columns.Add(pvColImagem);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_bib_titulo");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
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
      get { return "LY_BIB_TITULO"; }
    }

    protected override sealed string Name { get { return "Título"; } }

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
        case CommandOperation.Select: cm.CommandText = "zLy_bib_titulo_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_bib_titulo_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_bib_titulo_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_bib_titulo_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("titulo", OleDbType.VarChar, 600, ParameterDirection.InputOutput, true, 0, 0, "TITULO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("isbn", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 28, 10, "ISBN", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("edicao", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "EDICAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("ano", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ANO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("area", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "AREA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("tema", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TEMA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("genero", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "GENERO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("id_bib_editora", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_BIB_EDITORA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("id_bib_tipo_material", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_BIB_TIPO_MATERIAL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("id_bib_assunto_cdd", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_BIB_ASSUNTO_CDD", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("imagem", OleDbType.VarBinary, 0, ParameterDirection.Input, true, 0, 0, "IMAGEM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("5F5F554E4445465F4C4F4E475241575F5F")));

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
          if(lower == "titulo") { result.Add(toLower ? "titulo" : column); continue; }
          if(lower == "isbn") { result.Add(toLower ? "isbn" : column); continue; }
          if(lower == "edicao") { result.Add(toLower ? "edicao" : column); continue; }
          if(lower == "ano") { result.Add(toLower ? "ano" : column); continue; }
          if(lower == "area") { result.Add(toLower ? "area" : column); continue; }
          if(lower == "tema") { result.Add(toLower ? "tema" : column); continue; }
          if(lower == "genero") { result.Add(toLower ? "genero" : column); continue; }
          if(lower == "id_bib_editora") { result.Add(toLower ? "id_bib_editora" : column); continue; }
          if(lower == "id_bib_tipo_material") { result.Add(toLower ? "id_bib_tipo_material" : column); continue; }
          if(lower == "id_bib_assunto_cdd") { result.Add(toLower ? "id_bib_assunto_cdd" : column); continue; }
          if(lower == "imagem") { result.Add(toLower ? "imagem" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "titulo", "isbn", "edicao", "ano", "area", "tema", "genero", 
                                           "id_bib_editora", "id_bib_tipo_material", "id_bib_assunto_cdd", "imagem" }
                          : new string[] { "ID", "TITULO", "ISBN", "EDICAO", "ANO", "AREA", "TEMA", "GENERO", 
                                           "ID_BIB_EDITORA", "ID_BIB_TIPO_MATERIAL", "ID_BIB_ASSUNTO_CDD", "IMAGEM" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "titulo", "isbn", "edicao", "ano", "area", "tema", "genero", 
                                           "id_bib_editora", "id_bib_tipo_material", "id_bib_assunto_cdd", "imagem" }
                          : new string[] { "ID", "TITULO", "ISBN", "EDICAO", "ANO", "AREA", "TEMA", "GENERO", 
                                           "ID_BIB_EDITORA", "ID_BIB_TIPO_MATERIAL", "ID_BIB_ASSUNTO_CDD", "IMAGEM" });

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
        "ly_bib_titulo.id"
      });
      if(scColumns.Contains("titulo")) list.Add("ly_bib_titulo.titulo");
      if(scColumns.Contains("isbn")) list.Add("ly_bib_titulo.isbn");
      if(scColumns.Contains("edicao")) list.Add("ly_bib_titulo.edicao");
      if(scColumns.Contains("ano")) list.Add("ly_bib_titulo.ano");
      if(scColumns.Contains("area")) list.Add("ly_bib_titulo.area");
      if(scColumns.Contains("tema")) list.Add("ly_bib_titulo.tema");
      if(scColumns.Contains("genero")) list.Add("ly_bib_titulo.genero");
      if(scColumns.Contains("id_bib_editora")) list.Add("ly_bib_titulo.id_bib_editora");
      if(scColumns.Contains("id_bib_tipo_material")) list.Add("ly_bib_titulo.id_bib_tipo_material");
      if(scColumns.Contains("id_bib_assunto_cdd")) list.Add("ly_bib_titulo.id_bib_assunto_cdd");
      if(scColumns.Contains("imagem")) list.Add("ly_bib_titulo.imagem");
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
          "FROM ly_bib_titulo " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_bib_titulo");
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
        da.TableMappings.Add("Table", "Ly_bib_titulo");
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
            if (dr["TITULO"] != null && string.IsNullOrEmpty(dr["TITULO"].ToString()))
              dr["TITULO"] = DBNull.Value;
            if (dr["ISBN"] != null && string.IsNullOrEmpty(dr["ISBN"].ToString()))
              dr["ISBN"] = DBNull.Value;
            if (dr["EDICAO"] != null && string.IsNullOrEmpty(dr["EDICAO"].ToString()))
              dr["EDICAO"] = DBNull.Value;
            if (dr["ANO"] != null && string.IsNullOrEmpty(dr["ANO"].ToString()))
              dr["ANO"] = DBNull.Value;
            if (dr["AREA"] != null && string.IsNullOrEmpty(dr["AREA"].ToString()))
              dr["AREA"] = DBNull.Value;
            if (dr["TEMA"] != null && string.IsNullOrEmpty(dr["TEMA"].ToString()))
              dr["TEMA"] = DBNull.Value;
            if (dr["GENERO"] != null && string.IsNullOrEmpty(dr["GENERO"].ToString()))
              dr["GENERO"] = DBNull.Value;
            if (dr["ID_BIB_EDITORA"] != null && string.IsNullOrEmpty(dr["ID_BIB_EDITORA"].ToString()))
              dr["ID_BIB_EDITORA"] = DBNull.Value;
            if (dr["ID_BIB_TIPO_MATERIAL"] != null && string.IsNullOrEmpty(dr["ID_BIB_TIPO_MATERIAL"].ToString()))
              dr["ID_BIB_TIPO_MATERIAL"] = DBNull.Value;
            if (dr["ID_BIB_ASSUNTO_CDD"] != null && string.IsNullOrEmpty(dr["ID_BIB_ASSUNTO_CDD"].ToString()))
              dr["ID_BIB_ASSUNTO_CDD"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["TITULO"] != null && !string.IsNullOrEmpty(dr["TITULO"].ToString()))
            {
              if (dr["TITULO"].ToString().Length > 500)
              {
                return "O título deve possuir no máximo 500 caracteres.";
              }
            }

            if (dr["ISBN"] != null && !string.IsNullOrEmpty(dr["ISBN"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ISBN"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ISBN"]).ToString("0.############").Split(',')[0].Length > 18) || (Convert.ToDecimal(dr["ISBN"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ISBN"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O isbn deve ser um valor inteiro com 18 dígitos no máximo.";
              }
            }

            if (dr["EDICAO"] != null && !string.IsNullOrEmpty(dr["EDICAO"].ToString()))
            {
              if (dr["EDICAO"].ToString().Length > 50)
              {
                return "A edição deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["ANO"] != null && !string.IsNullOrEmpty(dr["ANO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ANO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ANO"]).ToString("0.############").Split(',')[0].Length > 4) || (Convert.ToDecimal(dr["ANO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ANO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O ano deve ser um valor inteiro com 4 dígitos no máximo.";
              }
            }

            if (dr["AREA"] != null && !string.IsNullOrEmpty(dr["AREA"].ToString()))
            {
              if (dr["AREA"].ToString().Length > 50)
              {
                return "A área deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["TEMA"] != null && !string.IsNullOrEmpty(dr["TEMA"].ToString()))
            {
              if (dr["TEMA"].ToString().Length > 50)
              {
                return "O tema deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["GENERO"] != null && !string.IsNullOrEmpty(dr["GENERO"].ToString()))
            {
              if (dr["GENERO"].ToString().Length > 50)
              {
                return "O gênero deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["ID_BIB_EDITORA"] != null && !string.IsNullOrEmpty(dr["ID_BIB_EDITORA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_BIB_EDITORA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_EDITORA"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_BIB_EDITORA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_EDITORA"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A editora deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["ID_BIB_TIPO_MATERIAL"] != null && !string.IsNullOrEmpty(dr["ID_BIB_TIPO_MATERIAL"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_BIB_TIPO_MATERIAL"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_TIPO_MATERIAL"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_BIB_TIPO_MATERIAL"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_TIPO_MATERIAL"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O tipo de material deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["ID_BIB_ASSUNTO_CDD"] != null && !string.IsNullOrEmpty(dr["ID_BIB_ASSUNTO_CDD"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_BIB_ASSUNTO_CDD"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_ASSUNTO_CDD"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_BIB_ASSUNTO_CDD"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_ASSUNTO_CDD"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O assunto cdd deve ser um valor inteiro com 14 dígitos no máximo.";
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

    public static Ly_bib_titulo Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_bib_titulo Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_bib_titulo tab = new Ly_bib_titulo();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_bib_titulo tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, string @titulo, decimal? @isbn, string @edicao, decimal? @ano, string @area, 
        string @tema, string @genero, decimal? @id_bib_editora, decimal? @id_bib_tipo_material, 
        decimal? @id_bib_assunto_cdd, byte[] @imagem
      ) {
        return (Row)Add(new object[] {
          @id, @titulo, @isbn, @edicao, @ano, @area, @tema, @genero, @id_bib_editora, @id_bib_tipo_material, 
          @id_bib_assunto_cdd, @imagem
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
      /// TITULO: VARCHAR(500)
      /// </summary>
      public string Titulo { get { return (string)this["titulo"]; } set { this["titulo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ISBN: NUMERIC(18)
      /// </summary>
      public decimal? Isbn { get { return (decimal?)this["isbn"]; } set { this["isbn"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// EDICAO: VARCHAR(50)
      /// </summary>
      public string Edicao { get { return (string)this["edicao"]; } set { this["edicao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ANO: NUMERIC(4)
      /// </summary>
      public decimal? Ano { get { return (decimal?)this["ano"]; } set { this["ano"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// AREA: VARCHAR(50)
      /// </summary>
      public string Area { get { return (string)this["area"]; } set { this["area"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TEMA: VARCHAR(50)
      /// </summary>
      public string Tema { get { return (string)this["tema"]; } set { this["tema"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// GENERO: VARCHAR(50)
      /// </summary>
      public string Genero { get { return (string)this["genero"]; } set { this["genero"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ID_BIB_EDITORA: NUMERIC(14)
      /// </summary>
      public decimal? Id_bib_editora { get { return (decimal?)this["id_bib_editora"]; } set { this["id_bib_editora"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ID_BIB_TIPO_MATERIAL: NUMERIC(14)
      /// </summary>
      public decimal? Id_bib_tipo_material { get { return (decimal?)this["id_bib_tipo_material"]; } set { this["id_bib_tipo_material"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ID_BIB_ASSUNTO_CDD: NUMERIC(14)
      /// </summary>
      public decimal? Id_bib_assunto_cdd { get { return (decimal?)this["id_bib_assunto_cdd"]; } set { this["id_bib_assunto_cdd"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// IMAGEM: IMAGE
      /// </summary>
      public byte[] Imagem { get { return (byte[])this["imagem"]; } set { this["imagem"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection) {
        return Insert(connection,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_bib_titulo(),
          new string[0], new object[0],
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_bib_titulo(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_bib_titulo(),
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
          connection, new Techne.Lyceum.CR.Ly_bib_titulo(),
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
        public string Titulo { get { return (string)row["titulo", DataRowVersion.Original]; } }
        public decimal? Isbn { get { return (decimal?)row["isbn", DataRowVersion.Original]; } }
        public string Edicao { get { return (string)row["edicao", DataRowVersion.Original]; } }
        public decimal? Ano { get { return (decimal?)row["ano", DataRowVersion.Original]; } }
        public string Area { get { return (string)row["area", DataRowVersion.Original]; } }
        public string Tema { get { return (string)row["tema", DataRowVersion.Original]; } }
        public string Genero { get { return (string)row["genero", DataRowVersion.Original]; } }
        public decimal? Id_bib_editora { get { return (decimal?)row["id_bib_editora", DataRowVersion.Original]; } }
        public decimal? Id_bib_tipo_material { get { return (decimal?)row["id_bib_tipo_material", DataRowVersion.Original]; } }
        public decimal? Id_bib_assunto_cdd { get { return (decimal?)row["id_bib_assunto_cdd", DataRowVersion.Original]; } }
        public byte[] Imagem { get { return (byte[])row["imagem", DataRowVersion.Original]; } }

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


      protected override string GetName(){return "identificador";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_titulo.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_BIB_TITULO.ID"; } }
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

    #region Coluna: TITULO
    private ColumnTitulo pvColTitulo;

    public sealed class ColumnTitulo : TDataColumn {

      public ColumnTitulo() {
        this.ColumnName = "titulo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "título";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_titulo.titulo"; } }
      public override string OriginalCol { get { return "titulo"; } }
      public override string RefCol { get { return "LY_BIB_TITULO.TITULO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 500; } }
      public override string SqlServerType { get { return "VARCHAR(500)"; } }

    }
    #endregion

    #region Coluna: ISBN
    private ColumnIsbn pvColIsbn;

    public sealed class ColumnIsbn : TDataColumn {

      public ColumnIsbn() {
        this.ColumnName = "isbn";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "isbn";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_titulo.isbn"; } }
      public override string OriginalCol { get { return "isbn"; } }
      public override string RefCol { get { return "LY_BIB_TITULO.ISBN"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 19; } }
      public override int Precision { get { return 18; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(18)"; } }

    }
    #endregion

    #region Coluna: EDICAO
    private ColumnEdicao pvColEdicao;

    public sealed class ColumnEdicao : TDataColumn {

      public ColumnEdicao() {
        this.ColumnName = "edicao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "edição";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_titulo.edicao"; } }
      public override string OriginalCol { get { return "edicao"; } }
      public override string RefCol { get { return "LY_BIB_TITULO.EDICAO"; } }
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

    #region Coluna: ANO
    private ColumnAno pvColAno;

    public sealed class ColumnAno : TDataColumn {

      public ColumnAno() {
        this.ColumnName = "ano";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "ano";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_titulo.ano"; } }
      public override string OriginalCol { get { return "ano"; } }
      public override string RefCol { get { return "LY_BIB_TITULO.ANO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 5; } }
      public override int Precision { get { return 4; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(4)"; } }

    }
    #endregion

    #region Coluna: AREA
    private ColumnArea pvColArea;

    public sealed class ColumnArea : TDataColumn {

      public ColumnArea() {
        this.ColumnName = "area";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "área";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_titulo.area"; } }
      public override string OriginalCol { get { return "area"; } }
      public override string RefCol { get { return "LY_BIB_TITULO.AREA"; } }
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

    #region Coluna: TEMA
    private ColumnTema pvColTema;

    public sealed class ColumnTema : TDataColumn {

      public ColumnTema() {
        this.ColumnName = "tema";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "tema";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_titulo.tema"; } }
      public override string OriginalCol { get { return "tema"; } }
      public override string RefCol { get { return "LY_BIB_TITULO.TEMA"; } }
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

    #region Coluna: GENERO
    private ColumnGenero pvColGenero;

    public sealed class ColumnGenero : TDataColumn {

      public ColumnGenero() {
        this.ColumnName = "genero";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "gênero";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_titulo.genero"; } }
      public override string OriginalCol { get { return "genero"; } }
      public override string RefCol { get { return "LY_BIB_TITULO.GENERO"; } }
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

    #region Coluna: ID_BIB_EDITORA
    private ColumnId_bib_editora pvColId_bib_editora;

    public sealed class ColumnId_bib_editora : TDataColumn {

      public ColumnId_bib_editora() {
        this.ColumnName = "id_bib_editora";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "editora";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_titulo.id_bib_editora"; } }
      public override string OriginalCol { get { return "id_bib_editora"; } }
      public override string RefCol { get { return "LY_BIB_TITULO.ID_BIB_EDITORA"; } }
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

    #region Coluna: ID_BIB_TIPO_MATERIAL
    private ColumnId_bib_tipo_material pvColId_bib_tipo_material;

    public sealed class ColumnId_bib_tipo_material : TDataColumn {

      public ColumnId_bib_tipo_material() {
        this.ColumnName = "id_bib_tipo_material";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "tipo de material";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_titulo.id_bib_tipo_material"; } }
      public override string OriginalCol { get { return "id_bib_tipo_material"; } }
      public override string RefCol { get { return "LY_BIB_TITULO.ID_BIB_TIPO_MATERIAL"; } }
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

    #region Coluna: ID_BIB_ASSUNTO_CDD
    private ColumnId_bib_assunto_cdd pvColId_bib_assunto_cdd;

    public sealed class ColumnId_bib_assunto_cdd : TDataColumn {

      public ColumnId_bib_assunto_cdd() {
        this.ColumnName = "id_bib_assunto_cdd";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "assunto cdd";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_titulo.id_bib_assunto_cdd"; } }
      public override string OriginalCol { get { return "id_bib_assunto_cdd"; } }
      public override string RefCol { get { return "LY_BIB_TITULO.ID_BIB_ASSUNTO_CDD"; } }
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

    #region Coluna: IMAGEM
    private ColumnImagem pvColImagem;

    public sealed class ColumnImagem : TDataColumn {

      public ColumnImagem() {
        this.ColumnName = "imagem";
        this.DataType = typeof(byte[]);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_titulo.imagem"; } }
      public override string OriginalCol { get { return "imagem"; } }
      public override string RefCol { get { return "LY_BIB_TITULO.IMAGEM"; } }
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

  public partial class Ly_bib_usuario : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_bib_usuario() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_bib_usuario");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_bib_usuario");
      }
      catch{}
    }

    public Ly_bib_usuario() {
      pvColId = new ColumnId(); Columns.Add(pvColId);
      pvColPessoa = new ColumnPessoa(); Columns.Add(pvColPessoa);
      pvColId_bib_biblioteca = new ColumnId_bib_biblioteca(); Columns.Add(pvColId_bib_biblioteca);
      pvColTipo = new ColumnTipo(); Columns.Add(pvColTipo);
      pvColNome_compl04 = new ColumnNome_compl04(); Columns.Add(pvColNome_compl04);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_bib_usuario");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(decimal? @pessoa, decimal? @id_bib_biblioteca, string @tipo) {
      Row row = NewRow();

      row.Pessoa = @pessoa;
      row.Id_bib_biblioteca = @id_bib_biblioteca;
      row.Tipo = @tipo;

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
      get { return "LY_BIB_USUARIO"; }
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
      SetPrimaryKey(new DataColumn[] { pvColId });
      pvColId.AllowDBNull = true;
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_bib_usuario_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_bib_usuario_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_bib_usuario_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_bib_usuario_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("pessoa", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "PESSOA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("id_bib_biblioteca", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 10, "ID_BIB_BIBLIOTECA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
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
          if(lower == "id") { result.Add(toLower ? "id" : column); continue; }
          if(lower == "pessoa") { result.Add(toLower ? "pessoa" : column); continue; }
          if(lower == "id_bib_biblioteca") { result.Add(toLower ? "id_bib_biblioteca" : column); continue; }
          if(lower == "tipo") { result.Add(toLower ? "tipo" : column); continue; }
          if(!tabPrincipal) {
            if(lower == "nome_compl04") { result.Add(toLower ? "nome_compl04" : column); continue; }
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id", "pessoa", "id_bib_biblioteca", "tipo" }
                          : new string[] { "ID", "PESSOA", "ID_BIB_BIBLIOTECA", "TIPO" });
        else
          result.AddRange(toLower
                          ? new string[] { "id", "pessoa", "id_bib_biblioteca", "tipo", "nome_compl04" }
                          : new string[] { "ID", "PESSOA", "ID_BIB_BIBLIOTECA", "TIPO", "NOME_COMPL04" });

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
        "ly_bib_usuario.id"
      });
      if(scColumns.Contains("pessoa")) list.Add("ly_bib_usuario.pessoa");
      if(scColumns.Contains("id_bib_biblioteca")) list.Add("ly_bib_usuario.id_bib_biblioteca");
      if(scColumns.Contains("tipo")) list.Add("ly_bib_usuario.tipo");
      if(scColumns.Contains("nome_compl04")) list.Add("ly_pessoa.nome_compl nome_compl04");
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
          "FROM ly_bib_usuario " +
               "INNER JOIN ly_pessoa " +
                 "ON ly_pessoa.pessoa = ly_bib_usuario.pessoa " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_bib_usuario");
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
        da.TableMappings.Add("Table", "Ly_bib_usuario");
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
            if (dr["PESSOA"] != null && string.IsNullOrEmpty(dr["PESSOA"].ToString()))
              dr["PESSOA"] = DBNull.Value;
            if (dr["ID_BIB_BIBLIOTECA"] != null && string.IsNullOrEmpty(dr["ID_BIB_BIBLIOTECA"].ToString()))
              dr["ID_BIB_BIBLIOTECA"] = DBNull.Value;
            if (dr["TIPO"] != null && string.IsNullOrEmpty(dr["TIPO"].ToString()))
              dr["TIPO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["PESSOA"] == null || string.IsNullOrEmpty(dr["PESSOA"].ToString()))
            {
              return "Favor informar a pessoa.";
            }

            if (dr["PESSOA"] != null && !string.IsNullOrEmpty(dr["PESSOA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["PESSOA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PESSOA"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["PESSOA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PESSOA"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A pessoa deve ser um valor inteiro com 10 dígitos no máximo.";
              }
            }

            if (dr["ID_BIB_BIBLIOTECA"] == null || string.IsNullOrEmpty(dr["ID_BIB_BIBLIOTECA"].ToString()))
            {
              return "Favor informar a biblioteca.";
            }

            if (dr["ID_BIB_BIBLIOTECA"] != null && !string.IsNullOrEmpty(dr["ID_BIB_BIBLIOTECA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_BIB_BIBLIOTECA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_BIBLIOTECA"]).ToString("0.############").Split(',')[0].Length > 14) || (Convert.ToDecimal(dr["ID_BIB_BIBLIOTECA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_BIB_BIBLIOTECA"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A biblioteca deve ser um valor inteiro com 14 dígitos no máximo.";
              }
            }

            if (dr["TIPO"] == null || string.IsNullOrEmpty(dr["TIPO"].ToString()))
            {
              return "Favor informar o tipo.";
            }

            if (dr["TIPO"] != null && !string.IsNullOrEmpty(dr["TIPO"].ToString()))
            {
              if (dr["TIPO"].ToString().Length > 20)
              {
                return "O tipo deve possuir no máximo 20 caracteres.";
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

    public static Ly_bib_usuario Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_bib_usuario Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_bib_usuario tab = new Ly_bib_usuario();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_bib_usuario tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id, decimal? @pessoa, decimal? @id_bib_biblioteca, string @tipo
      ) {
        return (Row)Add(new object[] {
          @id, @pessoa, @id_bib_biblioteca, @tipo
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
      /// PESSOA: NUMERIC(10), Not Null
      /// </summary>
      public decimal? Pessoa { get { return (decimal?)this["pessoa"]; } set { this["pessoa"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ID_BIB_BIBLIOTECA: NUMERIC(14), Not Null
      /// </summary>
      public decimal? Id_bib_biblioteca { get { return (decimal?)this["id_bib_biblioteca"]; } set { this["id_bib_biblioteca"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TIPO: VARCHAR(20), Not Null
      /// </summary>
      public string Tipo { get { return (string)this["tipo"]; } set { this["tipo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME_COMPL: VARCHAR(100), Not Null
      /// </summary>
      public string Nome_compl04 { get { return (string)this["nome_compl04"]; } set { this["nome_compl04"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               decimal? @pessoa, decimal? @id_bib_biblioteca, string @tipo) {
        return Insert(connection,
                      @pessoa, @id_bib_biblioteca, @tipo,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               decimal? @pessoa, decimal? @id_bib_biblioteca, string @tipo,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_bib_usuario(),
          new string[] { "pessoa", "id_bib_biblioteca", "tipo" },
          new object[] { @pessoa, @id_bib_biblioteca, @tipo },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_bib_usuario(),
          new string[] { "id" },
          new object[] { pkId },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_bib_usuario(),
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
          connection, new Techne.Lyceum.CR.Ly_bib_usuario(),
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
        public decimal? Pessoa { get { return (decimal?)row["pessoa", DataRowVersion.Original]; } }
        public decimal? Id_bib_biblioteca { get { return (decimal?)row["id_bib_biblioteca", DataRowVersion.Original]; } }
        public string Tipo { get { return (string)row["tipo", DataRowVersion.Original]; } }
        public string Nome_compl04 { get { return (string)row["nome_compl04", DataRowVersion.Original]; } }

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


      protected override string GetName(){return "identificador";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_usuario.id"; } }
      public override string OriginalCol { get { return "id"; } }
      public override string RefCol { get { return "LY_BIB_USUARIO.ID"; } }
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

    #region Coluna: PESSOA
    private ColumnPessoa pvColPessoa;

    public sealed class ColumnPessoa : TDataColumn {

      public ColumnPessoa() {
        this.ColumnName = "pessoa";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "pessoa";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_usuario.pessoa"; } }
      public override string OriginalCol { get { return "pessoa"; } }
      public override string RefCol { get { return "LY_BIB_USUARIO.PESSOA"; } }
      public override bool PrimaryKey { get { return false; } }
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

    #region Coluna: ID_BIB_BIBLIOTECA
    private ColumnId_bib_biblioteca pvColId_bib_biblioteca;

    public sealed class ColumnId_bib_biblioteca : TDataColumn {

      public ColumnId_bib_biblioteca() {
        this.ColumnName = "id_bib_biblioteca";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "biblioteca";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_bib_usuario.id_bib_biblioteca"; } }
      public override string OriginalCol { get { return "id_bib_biblioteca"; } }
      public override string RefCol { get { return "LY_BIB_USUARIO.ID_BIB_BIBLIOTECA"; } }
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
      public override string FullCol { get { return "ly_bib_usuario.tipo"; } }
      public override string OriginalCol { get { return "tipo"; } }
      public override string RefCol { get { return "LY_BIB_USUARIO.TIPO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 20; } }
      public override string SqlServerType { get { return "VARCHAR(20)"; } }

    }
    #endregion

    #region Coluna: NOME_COMPL04
    private ColumnNome_compl04 pvColNome_compl04;

    public sealed class ColumnNome_compl04 : TDataColumn {

      public ColumnNome_compl04() {
        this.ColumnName = "nome_compl04";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "nome completo";}
      protected override string GetCaption(){return "Nome Completo";}
      protected override string GetDescricao(){return "Nome Completo";}
      public override string FullCol { get { return "ly_pessoa.nome_compl"; } }
      public override string OriginalCol { get { return "nome_compl"; } }
      public override string RefCol { get { return "LY_PESSOA.NOME_COMPL"; } }
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
    #endregion
  }
}

