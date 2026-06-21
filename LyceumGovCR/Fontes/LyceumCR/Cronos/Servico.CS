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
  public class Servico : TDataSet {
    private const string DataSetName_Def = "Servico";
    private static ConstructorInfo ctorTCustom = null;
    private CustomBase tcustom = null;
    #region Variáveis private para as tabelas deste dataset
    private Ly_andamento pvTabLy_andamento;
    private Ly_correspondencia pvTabLy_correspondencia;
    private Ly_correspondencia_docs pvTabLy_correspondencia_docs;
    private Ly_documentos_necessarios pvTabLy_documentos_necessarios;
    private Ly_fluxo_de_andamento pvTabLy_fluxo_de_andamento;
    private Ly_itens_solicit_serv pvTabLy_itens_solicit_serv;
    private Ly_solicitacao_serv pvTabLy_solicitacao_serv;
    private Ly_tabela_servicos pvTabLy_tabela_servicos;
    private Serv_solicitacao pvTabServ_solicitacao;
    private Vw_andamento_bt pvTabVw_andamento_bt;
    #endregion

    static Servico() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Servico");
      }
      catch{}
    }

    public Servico() : base() {
      DataSetName = DataSetName_Def;

      pvTabLy_andamento = new Ly_andamento();
      pvTabLy_correspondencia = new Ly_correspondencia();
      pvTabLy_correspondencia_docs = new Ly_correspondencia_docs();
      pvTabLy_documentos_necessarios = new Ly_documentos_necessarios();
      pvTabLy_fluxo_de_andamento = new Ly_fluxo_de_andamento();
      pvTabLy_itens_solicit_serv = new Ly_itens_solicit_serv();
      pvTabLy_solicitacao_serv = new Ly_solicitacao_serv();
      pvTabLy_tabela_servicos = new Ly_tabela_servicos();
      pvTabServ_solicitacao = new Serv_solicitacao();
      pvTabVw_andamento_bt = new Vw_andamento_bt();

      Tables.Add(pvTabLy_andamento);
      Tables.Add(pvTabLy_correspondencia);
      Tables.Add(pvTabLy_correspondencia_docs);
      Tables.Add(pvTabLy_documentos_necessarios);
      Tables.Add(pvTabLy_fluxo_de_andamento);
      Tables.Add(pvTabLy_itens_solicit_serv);
      Tables.Add(pvTabLy_solicitacao_serv);
      Tables.Add(pvTabLy_tabela_servicos);
      Tables.Add(pvTabServ_solicitacao);
      Tables.Add(pvTabVw_andamento_bt);

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
          pvTabLy_andamento.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_andamento); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_correspondencia.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_correspondencia); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_correspondencia_docs.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_correspondencia_docs); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_documentos_necessarios.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_documentos_necessarios); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_fluxo_de_andamento.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_fluxo_de_andamento); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_itens_solicit_serv.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_itens_solicit_serv); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_solicitacao_serv.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_solicitacao_serv); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_tabela_servicos.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_tabela_servicos); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabServ_solicitacao.Put(connection); row = DataLib.FirstRowWithError(pvTabServ_solicitacao); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabVw_andamento_bt.Put(connection); row = DataLib.FirstRowWithError(pvTabVw_andamento_bt); if(row != null) return DataLib.RowErrorsToString(row);
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
    public Ly_andamento Ly_andamento { get { return pvTabLy_andamento; } }

    [
      Browsable(false),
    ]
    public Ly_correspondencia Ly_correspondencia { get { return pvTabLy_correspondencia; } }

    [
      Browsable(false),
    ]
    public Ly_correspondencia_docs Ly_correspondencia_docs { get { return pvTabLy_correspondencia_docs; } }

    [
      Browsable(false),
    ]
    public Ly_documentos_necessarios Ly_documentos_necessarios { get { return pvTabLy_documentos_necessarios; } }

    [
      Browsable(false),
    ]
    public Ly_fluxo_de_andamento Ly_fluxo_de_andamento { get { return pvTabLy_fluxo_de_andamento; } }

    [
      Browsable(false),
    ]
    public Ly_itens_solicit_serv Ly_itens_solicit_serv { get { return pvTabLy_itens_solicit_serv; } }

    [
      Browsable(false),
    ]
    public Ly_solicitacao_serv Ly_solicitacao_serv { get { return pvTabLy_solicitacao_serv; } }

    [
      Browsable(false),
    ]
    public Ly_tabela_servicos Ly_tabela_servicos { get { return pvTabLy_tabela_servicos; } }

    [
      Browsable(false),
    ]
    public Serv_solicitacao Serv_solicitacao { get { return pvTabServ_solicitacao; } }

    [
      Browsable(false),
    ]
    public Vw_andamento_bt Vw_andamento_bt { get { return pvTabVw_andamento_bt; } }

    #endregion

    public abstract class CustomBase {
      public abstract string Put(Techne.Lyceum.CR.Servico ds, TConnectionWritable connection);
    }

  }


  public partial class Ly_andamento : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_andamento() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_andamento");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_andamento");
      }
      catch{}
    }

    public Ly_andamento() {
      pvColAndamento = new ColumnAndamento(); Columns.Add(pvColAndamento);
      pvColServico = new ColumnServico(); Columns.Add(pvColServico);
      pvColPasso = new ColumnPasso(); Columns.Add(pvColPasso);
      pvColSolicitacao = new ColumnSolicitacao(); Columns.Add(pvColSolicitacao);
      pvColItem_solicitacao = new ColumnItem_solicitacao(); Columns.Add(pvColItem_solicitacao);
      pvColComentario = new ColumnComentario(); Columns.Add(pvColComentario);
      pvColData = new ColumnData(); Columns.Add(pvColData);
      pvColStatus = new ColumnStatus(); Columns.Add(pvColStatus);
      pvColSetor = new ColumnSetor(); Columns.Add(pvColSetor);
      pvColUsuario = new ColumnUsuario(); Columns.Add(pvColUsuario);
      pvColProximo_setor = new ColumnProximo_setor(); Columns.Add(pvColProximo_setor);
      pvColProximo_usuario = new ColumnProximo_usuario(); Columns.Add(pvColProximo_usuario);
      pvColAtendimento = new ColumnAtendimento(); Columns.Add(pvColAtendimento);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_andamento");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @servico, decimal? @passo, decimal? @solicitacao, decimal? @item_solicitacao, 
                      DateTime? @data, string @setor) {
      Row row = NewRow();

      row.Servico = @servico;
      row.Passo = @passo;
      row.Solicitacao = @solicitacao;
      row.Item_solicitacao = @item_solicitacao;
      row.Data = @data;
      row.Setor = @setor;

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
      get { return "LY_ANDAMENTO"; }
    }

    protected override sealed string Name { get { return "Andamento"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColAndamento });
      pvColAndamento.AllowDBNull = true;
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_andamento_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_andamento_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_andamento_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_andamento_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkAndamento", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ANDAMENTO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("andamento", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ANDAMENTO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("servico", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SERVICO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("passo", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "PASSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("solicitacao", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "SOLICITACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("item_solicitacao", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ITEM_SOLICITACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("comentario", OleDbType.VarChar, 2100, ParameterDirection.InputOutput, true, 0, 0, "COMENTARIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("data", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DATA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("status", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "STATUS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("setor", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SETOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("usuario", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "USUARIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("proximo_setor", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PROXIMO_SETOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("proximo_usuario", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PROXIMO_USUARIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("atendimento", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ATENDIMENTO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "andamento") { result.Add(toLower ? "andamento" : column); continue; }
          if(lower == "servico") { result.Add(toLower ? "servico" : column); continue; }
          if(lower == "passo") { result.Add(toLower ? "passo" : column); continue; }
          if(lower == "solicitacao") { result.Add(toLower ? "solicitacao" : column); continue; }
          if(lower == "item_solicitacao") { result.Add(toLower ? "item_solicitacao" : column); continue; }
          if(lower == "comentario") { result.Add(toLower ? "comentario" : column); continue; }
          if(lower == "data") { result.Add(toLower ? "data" : column); continue; }
          if(lower == "status") { result.Add(toLower ? "status" : column); continue; }
          if(lower == "setor") { result.Add(toLower ? "setor" : column); continue; }
          if(lower == "usuario") { result.Add(toLower ? "usuario" : column); continue; }
          if(lower == "proximo_setor") { result.Add(toLower ? "proximo_setor" : column); continue; }
          if(lower == "proximo_usuario") { result.Add(toLower ? "proximo_usuario" : column); continue; }
          if(lower == "atendimento") { result.Add(toLower ? "atendimento" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "andamento", "servico", "passo", "solicitacao", "item_solicitacao", 
                                           "comentario", "data", "status", "setor", "usuario", "proximo_setor", "proximo_usuario", 
                                           "atendimento" }
                          : new string[] { "ANDAMENTO", "SERVICO", "PASSO", "SOLICITACAO", "ITEM_SOLICITACAO", 
                                           "COMENTARIO", "DATA", "STATUS", "SETOR", "USUARIO", "PROXIMO_SETOR", "PROXIMO_USUARIO", 
                                           "ATENDIMENTO" });
        else
          result.AddRange(toLower
                          ? new string[] { "andamento", "servico", "passo", "solicitacao", "item_solicitacao", 
                                           "comentario", "data", "status", "setor", "usuario", "proximo_setor", "proximo_usuario", 
                                           "atendimento" }
                          : new string[] { "ANDAMENTO", "SERVICO", "PASSO", "SOLICITACAO", "ITEM_SOLICITACAO", 
                                           "COMENTARIO", "DATA", "STATUS", "SETOR", "USUARIO", "PROXIMO_SETOR", "PROXIMO_USUARIO", 
                                           "ATENDIMENTO" });

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
        "ly_andamento.andamento"
      });
      if(scColumns.Contains("servico")) list.Add("ly_andamento.servico");
      if(scColumns.Contains("passo")) list.Add("ly_andamento.passo");
      if(scColumns.Contains("solicitacao")) list.Add("ly_andamento.solicitacao");
      if(scColumns.Contains("item_solicitacao")) list.Add("ly_andamento.item_solicitacao");
      if(scColumns.Contains("comentario")) list.Add("ly_andamento.comentario");
      if(scColumns.Contains("data")) list.Add("ly_andamento.data");
      if(scColumns.Contains("status")) list.Add("ly_andamento.status");
      if(scColumns.Contains("setor")) list.Add("ly_andamento.setor");
      if(scColumns.Contains("usuario")) list.Add("ly_andamento.usuario");
      if(scColumns.Contains("proximo_setor")) list.Add("ly_andamento.proximo_setor");
      if(scColumns.Contains("proximo_usuario")) list.Add("ly_andamento.proximo_usuario");
      if(scColumns.Contains("atendimento")) list.Add("ly_andamento.atendimento");
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
          "FROM ly_andamento " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_andamento");
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
        da.TableMappings.Add("Table", "Ly_andamento");
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
            if (dr["SERVICO"] != null && string.IsNullOrEmpty(dr["SERVICO"].ToString()))
              dr["SERVICO"] = DBNull.Value;
            if (dr["PASSO"] != null && string.IsNullOrEmpty(dr["PASSO"].ToString()))
              dr["PASSO"] = DBNull.Value;
            if (dr["SOLICITACAO"] != null && string.IsNullOrEmpty(dr["SOLICITACAO"].ToString()))
              dr["SOLICITACAO"] = DBNull.Value;
            if (dr["ITEM_SOLICITACAO"] != null && string.IsNullOrEmpty(dr["ITEM_SOLICITACAO"].ToString()))
              dr["ITEM_SOLICITACAO"] = DBNull.Value;
            if (dr["COMENTARIO"] != null && string.IsNullOrEmpty(dr["COMENTARIO"].ToString()))
              dr["COMENTARIO"] = DBNull.Value;
            if (dr["STATUS"] != null && string.IsNullOrEmpty(dr["STATUS"].ToString()))
              dr["STATUS"] = DBNull.Value;
            if (dr["SETOR"] != null && string.IsNullOrEmpty(dr["SETOR"].ToString()))
              dr["SETOR"] = DBNull.Value;
            if (dr["USUARIO"] != null && string.IsNullOrEmpty(dr["USUARIO"].ToString()))
              dr["USUARIO"] = DBNull.Value;
            if (dr["PROXIMO_SETOR"] != null && string.IsNullOrEmpty(dr["PROXIMO_SETOR"].ToString()))
              dr["PROXIMO_SETOR"] = DBNull.Value;
            if (dr["PROXIMO_USUARIO"] != null && string.IsNullOrEmpty(dr["PROXIMO_USUARIO"].ToString()))
              dr["PROXIMO_USUARIO"] = DBNull.Value;
            if (dr["ATENDIMENTO"] != null && string.IsNullOrEmpty(dr["ATENDIMENTO"].ToString()))
              dr["ATENDIMENTO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["SERVICO"] == null || string.IsNullOrEmpty(dr["SERVICO"].ToString()))
            {
              return "Favor informar o Serviço.";
            }

            if (dr["SERVICO"] != null && !string.IsNullOrEmpty(dr["SERVICO"].ToString()))
            {
              if (dr["SERVICO"].ToString().Length > 20)
              {
                return "O Serviço deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["PASSO"] == null || string.IsNullOrEmpty(dr["PASSO"].ToString()))
            {
              return "Favor informar o Passo.";
            }

            if (dr["PASSO"] != null && !string.IsNullOrEmpty(dr["PASSO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["PASSO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PASSO"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["PASSO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PASSO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Passo deve ser um valor inteiro com 3 dígitos no máximo.";
              }
            }

            if (dr["SOLICITACAO"] == null || string.IsNullOrEmpty(dr["SOLICITACAO"].ToString()))
            {
              return "Favor informar o Solicitação.";
            }

            if (dr["SOLICITACAO"] != null && !string.IsNullOrEmpty(dr["SOLICITACAO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["SOLICITACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["SOLICITACAO"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["SOLICITACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["SOLICITACAO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Solicitação deve ser um valor inteiro com 10 dígitos no máximo.";
              }
            }

            if (dr["ITEM_SOLICITACAO"] == null || string.IsNullOrEmpty(dr["ITEM_SOLICITACAO"].ToString()))
            {
              return "Favor informar o Item de Solicitação.";
            }

            if (dr["ITEM_SOLICITACAO"] != null && !string.IsNullOrEmpty(dr["ITEM_SOLICITACAO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ITEM_SOLICITACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ITEM_SOLICITACAO"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["ITEM_SOLICITACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ITEM_SOLICITACAO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Item de Solicitação deve ser um valor inteiro com 10 dígitos no máximo.";
              }
            }

            if (dr["COMENTARIO"] != null && !string.IsNullOrEmpty(dr["COMENTARIO"].ToString()))
            {
              if (dr["COMENTARIO"].ToString().Length > 2000)
              {
                return "O Comentário deve possuir no máximo 2000 caracteres.";
              }
            }

            if (dr["DATA"] == null || string.IsNullOrEmpty(dr["DATA"].ToString()))
            {
              return "Favor informar o Data.";
            }

            if (dr["DATA"] != null && dr["DATA"].ToString() != "")
            {
              dr["DATA"] = Convert.ToDateTime(dr["DATA"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["STATUS"] != null && !string.IsNullOrEmpty(dr["STATUS"].ToString()))
            {
              if (dr["STATUS"].ToString().Length > 15)
              {
                return "O Status deve possuir no máximo 15 caracteres.";
              }

              if (dr["STATUS"] != null && !string.IsNullOrEmpty(dr["STATUS"].ToString()))
              {
                List<object> lstSTATUS = new List<object>(new object[]{"Executado", "Atendido", "Indeferido", "Deferido", "Entregue", "Cancelado", "Envio Corresp.", "Receb.Corresp."});
                if(!lstSTATUS.Contains(dr["STATUS"].ToString()))
                {
                  return "Os valores permitidos para o Status são ''Executado'', ''Atendido'', ''Indeferido'', ''Deferido'', ''Entregue'', ''Cancelado'', ''Envio Corresp.'' ou ''Receb.Corresp.''.";
                }
              }
            }

            if (dr["SETOR"] == null || string.IsNullOrEmpty(dr["SETOR"].ToString()))
            {
              return "Favor informar o Setor.";
            }

            if (dr["SETOR"] != null && !string.IsNullOrEmpty(dr["SETOR"].ToString()))
            {
              if (dr["SETOR"].ToString().Length > 15)
              {
                return "O Setor deve possuir no máximo 15 caracteres.";
              }
            }

            if (dr["USUARIO"] != null && !string.IsNullOrEmpty(dr["USUARIO"].ToString()))
            {
              if (dr["USUARIO"].ToString().Length > 15)
              {
                return "O Usuário deve possuir no máximo 15 caracteres.";
              }
            }

            if (dr["PROXIMO_SETOR"] != null && !string.IsNullOrEmpty(dr["PROXIMO_SETOR"].ToString()))
            {
              if (dr["PROXIMO_SETOR"].ToString().Length > 15)
              {
                return "O Próximo Setor deve possuir no máximo 15 caracteres.";
              }
            }

            if (dr["PROXIMO_USUARIO"] != null && !string.IsNullOrEmpty(dr["PROXIMO_USUARIO"].ToString()))
            {
              if (dr["PROXIMO_USUARIO"].ToString().Length > 15)
              {
                return "O Próximo Usuário deve possuir no máximo 15 caracteres.";
              }
            }

            if (dr["ATENDIMENTO"] != null && !string.IsNullOrEmpty(dr["ATENDIMENTO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ATENDIMENTO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ATENDIMENTO"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["ATENDIMENTO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ATENDIMENTO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Atendimento deve ser um valor inteiro com 10 dígitos no máximo.";
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

    public static Ly_andamento Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_andamento Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_andamento tab = new Ly_andamento();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_andamento tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @andamento, string @servico, decimal? @passo, decimal? @solicitacao, decimal? @item_solicitacao, 
        string @comentario, DateTime? @data, string @status, string @setor, string @usuario, 
        string @proximo_setor, string @proximo_usuario, decimal? @atendimento
      ) {
        return (Row)Add(new object[] {
          @andamento, @servico, @passo, @solicitacao, @item_solicitacao, @comentario, @data, 
          @status, @setor, @usuario, @proximo_setor, @proximo_usuario, @atendimento
        });
      }

      public Row Find(
        decimal? @andamento
      ) {
        return (Row)Find(new object[] {
          @andamento
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
      /// ANDAMENTO: NUMERIC(10), Primary Key, AutoNumber, Not Null
      /// </summary>
      public decimal? Andamento { get { return (decimal?)this["andamento"]; } set { this["andamento"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SERVICO: VARCHAR(20), Not Null
      /// </summary>
      public string Servico { get { return (string)this["servico"]; } set { this["servico"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PASSO: NUMERIC(3), Not Null
      /// </summary>
      public decimal? Passo { get { return (decimal?)this["passo"]; } set { this["passo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SOLICITACAO: NUMERIC(10), Not Null
      /// </summary>
      public decimal? Solicitacao { get { return (decimal?)this["solicitacao"]; } set { this["solicitacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ITEM_SOLICITACAO: NUMERIC(10), Not Null
      /// </summary>
      public decimal? Item_solicitacao { get { return (decimal?)this["item_solicitacao"]; } set { this["item_solicitacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// COMENTARIO: VARCHAR(2000)
      /// </summary>
      public string Comentario { get { return (string)this["comentario"]; } set { this["comentario"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DATA: DATETIME, Not Null
      /// </summary>
      public DateTime? Data { get { return (DateTime?)this["data"]; } set { this["data"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// STATUS: VARCHAR(15)
      /// </summary>
      public string Status { get { return (string)this["status"]; } set { this["status"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SETOR: VARCHAR(15), Not Null
      /// </summary>
      public string Setor { get { return (string)this["setor"]; } set { this["setor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// USUARIO: VARCHAR(15)
      /// </summary>
      public string Usuario { get { return (string)this["usuario"]; } set { this["usuario"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PROXIMO_SETOR: VARCHAR(15)
      /// </summary>
      public string Proximo_setor { get { return (string)this["proximo_setor"]; } set { this["proximo_setor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PROXIMO_USUARIO: VARCHAR(15)
      /// </summary>
      public string Proximo_usuario { get { return (string)this["proximo_usuario"]; } set { this["proximo_usuario"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ATENDIMENTO: NUMERIC(10)
      /// </summary>
      public decimal? Atendimento { get { return (decimal?)this["atendimento"]; } set { this["atendimento"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @servico, decimal? @passo, decimal? @solicitacao, decimal? @item_solicitacao, 
                               DateTime? @data, string @setor) {
        return Insert(connection,
                      @servico, @passo, @solicitacao, @item_solicitacao, @data, @setor,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @servico, decimal? @passo, decimal? @solicitacao, decimal? @item_solicitacao, 
                               DateTime? @data, string @setor,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_andamento(),
          new string[] { "servico", "passo", "solicitacao", "item_solicitacao", "data", "setor" },
          new object[] { @servico, @passo, @solicitacao, @item_solicitacao, @data, @setor },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkAndamento,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_andamento(),
          new string[] { "andamento" },
          new object[] { pkAndamento },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkAndamento) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_andamento(),
          new string[] { "andamento" },
          new object[] { pkAndamento }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              decimal? pkAndamento) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_andamento(),
          new string[] { "andamento" },
          new object[] { pkAndamento }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public decimal? Andamento { get { return (decimal?)row["andamento", DataRowVersion.Original]; } }
        public string Servico { get { return (string)row["servico", DataRowVersion.Original]; } }
        public decimal? Passo { get { return (decimal?)row["passo", DataRowVersion.Original]; } }
        public decimal? Solicitacao { get { return (decimal?)row["solicitacao", DataRowVersion.Original]; } }
        public decimal? Item_solicitacao { get { return (decimal?)row["item_solicitacao", DataRowVersion.Original]; } }
        public string Comentario { get { return (string)row["comentario", DataRowVersion.Original]; } }
        public DateTime? Data { get { return (DateTime?)row["data", DataRowVersion.Original]; } }
        public string Status { get { return (string)row["status", DataRowVersion.Original]; } }
        public string Setor { get { return (string)row["setor", DataRowVersion.Original]; } }
        public string Usuario { get { return (string)row["usuario", DataRowVersion.Original]; } }
        public string Proximo_setor { get { return (string)row["proximo_setor", DataRowVersion.Original]; } }
        public string Proximo_usuario { get { return (string)row["proximo_usuario", DataRowVersion.Original]; } }
        public decimal? Atendimento { get { return (decimal?)row["atendimento", DataRowVersion.Original]; } }

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
    #region Coluna: ANDAMENTO
    private ColumnAndamento pvColAndamento;

    public sealed class ColumnAndamento : TDataColumn {

      public ColumnAndamento() {
        this.ColumnName = "andamento";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_andamento.andamento"; } }
      public override string OriginalCol { get { return "andamento"; } }
      public override string RefCol { get { return "LY_ANDAMENTO.ANDAMENTO"; } }
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

    #region Coluna: SERVICO
    private ColumnServico pvColServico;

    public sealed class ColumnServico : TDataColumn {

      public ColumnServico() {
        this.ColumnName = "servico";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Serviço";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_andamento.servico"; } }
      public override string OriginalCol { get { return "servico"; } }
      public override string RefCol { get { return "LY_ANDAMENTO.SERVICO"; } }
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

    #region Coluna: PASSO
    private ColumnPasso pvColPasso;

    public sealed class ColumnPasso : TDataColumn {

      public ColumnPasso() {
        this.ColumnName = "passo";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "Passo";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_andamento.passo"; } }
      public override string OriginalCol { get { return "passo"; } }
      public override string RefCol { get { return "LY_ANDAMENTO.PASSO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 4; } }
      public override int Precision { get { return 3; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(3)"; } }

    }
    #endregion

    #region Coluna: SOLICITACAO
    private ColumnSolicitacao pvColSolicitacao;

    public sealed class ColumnSolicitacao : TDataColumn {

      public ColumnSolicitacao() {
        this.ColumnName = "solicitacao";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "Solicitação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_andamento.solicitacao"; } }
      public override string OriginalCol { get { return "solicitacao"; } }
      public override string RefCol { get { return "LY_ANDAMENTO.SOLICITACAO"; } }
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

    #region Coluna: ITEM_SOLICITACAO
    private ColumnItem_solicitacao pvColItem_solicitacao;

    public sealed class ColumnItem_solicitacao : TDataColumn {

      public ColumnItem_solicitacao() {
        this.ColumnName = "item_solicitacao";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "Item de Solicitação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_andamento.item_solicitacao"; } }
      public override string OriginalCol { get { return "item_solicitacao"; } }
      public override string RefCol { get { return "LY_ANDAMENTO.ITEM_SOLICITACAO"; } }
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

    #region Coluna: COMENTARIO
    private ColumnComentario pvColComentario;

    public sealed class ColumnComentario : TDataColumn {

      public ColumnComentario() {
        this.ColumnName = "comentario";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Comentário";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_andamento.comentario"; } }
      public override string OriginalCol { get { return "comentario"; } }
      public override string RefCol { get { return "LY_ANDAMENTO.COMENTARIO"; } }
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


      protected override string GetName(){return "Data";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_andamento.data"; } }
      public override string OriginalCol { get { return "data"; } }
      public override string RefCol { get { return "LY_ANDAMENTO.DATA"; } }
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

    #region Coluna: STATUS
    private ColumnStatus pvColStatus;

    public sealed class ColumnStatus : TDataColumn {

      public ColumnStatus() {
        this.ColumnName = "status";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Status";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_andamento.status"; } }
      public override string OriginalCol { get { return "status"; } }
      public override string RefCol { get { return "LY_ANDAMENTO.STATUS"; } }
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

    #region Coluna: SETOR
    private ColumnSetor pvColSetor;

    public sealed class ColumnSetor : TDataColumn {

      public ColumnSetor() {
        this.ColumnName = "setor";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Setor";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_andamento.setor"; } }
      public override string OriginalCol { get { return "setor"; } }
      public override string RefCol { get { return "LY_ANDAMENTO.SETOR"; } }
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

    #region Coluna: USUARIO
    private ColumnUsuario pvColUsuario;

    public sealed class ColumnUsuario : TDataColumn {

      public ColumnUsuario() {
        this.ColumnName = "usuario";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Usuário";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_andamento.usuario"; } }
      public override string OriginalCol { get { return "usuario"; } }
      public override string RefCol { get { return "LY_ANDAMENTO.USUARIO"; } }
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

    #region Coluna: PROXIMO_SETOR
    private ColumnProximo_setor pvColProximo_setor;

    public sealed class ColumnProximo_setor : TDataColumn {

      public ColumnProximo_setor() {
        this.ColumnName = "proximo_setor";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Próximo Setor";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_andamento.proximo_setor"; } }
      public override string OriginalCol { get { return "proximo_setor"; } }
      public override string RefCol { get { return "LY_ANDAMENTO.PROXIMO_SETOR"; } }
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

    #region Coluna: PROXIMO_USUARIO
    private ColumnProximo_usuario pvColProximo_usuario;

    public sealed class ColumnProximo_usuario : TDataColumn {

      public ColumnProximo_usuario() {
        this.ColumnName = "proximo_usuario";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Próximo Usuário";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_andamento.proximo_usuario"; } }
      public override string OriginalCol { get { return "proximo_usuario"; } }
      public override string RefCol { get { return "LY_ANDAMENTO.PROXIMO_USUARIO"; } }
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

    #region Coluna: ATENDIMENTO
    private ColumnAtendimento pvColAtendimento;

    public sealed class ColumnAtendimento : TDataColumn {

      public ColumnAtendimento() {
        this.ColumnName = "atendimento";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "Atendimento";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_andamento.atendimento"; } }
      public override string OriginalCol { get { return "atendimento"; } }
      public override string RefCol { get { return "LY_ANDAMENTO.ATENDIMENTO"; } }
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
    #endregion
  }

  public partial class Ly_correspondencia : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_correspondencia() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_correspondencia");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_correspondencia");
      }
      catch{}
    }

    public Ly_correspondencia() {
      pvColProtocolo = new ColumnProtocolo(); Columns.Add(pvColProtocolo);
      pvColDescricao = new ColumnDescricao(); Columns.Add(pvColDescricao);
      pvColUnidade_remetente = new ColumnUnidade_remetente(); Columns.Add(pvColUnidade_remetente);
      pvColUnidade_destinataria = new ColumnUnidade_destinataria(); Columns.Add(pvColUnidade_destinataria);
      pvColSetor_destino = new ColumnSetor_destino(); Columns.Add(pvColSetor_destino);
      pvColUsuario_destino = new ColumnUsuario_destino(); Columns.Add(pvColUsuario_destino);
      pvColDt_envio = new ColumnDt_envio(); Columns.Add(pvColDt_envio);
      pvColDt_recebimento = new ColumnDt_recebimento(); Columns.Add(pvColDt_recebimento);
      pvColSedex = new ColumnSedex(); Columns.Add(pvColSedex);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_correspondencia");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @protocolo, string @descricao, string @unidade_remetente, 
                      string @unidade_destinataria, DateTime? @dt_envio) {
      Row row = NewRow();

      row.Protocolo = @protocolo;
      row.Descricao = @descricao;
      row.Unidade_remetente = @unidade_remetente;
      row.Unidade_destinataria = @unidade_destinataria;
      row.Dt_envio = @dt_envio;

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
      get { return "LY_CORRESPONDENCIA"; }
    }

    protected override sealed string Name { get { return this.TableName; } }

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
      SetPrimaryKey(new DataColumn[] { pvColProtocolo });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_correspondencia_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_correspondencia_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_correspondencia_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_correspondencia_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkProtocolo", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "PROTOCOLO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("protocolo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PROTOCOLO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("descricao", OleDbType.VarChar, 600, ParameterDirection.InputOutput, true, 0, 0, "DESCRICAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("unidade_remetente", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "UNIDADE_REMETENTE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("unidade_destinataria", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "UNIDADE_DESTINATARIA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("setor_destino", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SETOR_DESTINO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("usuario_destino", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "USUARIO_DESTINO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("dt_envio", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_ENVIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_recebimento", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_RECEBIMENTO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("sedex", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SEDEX", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "protocolo") { result.Add(toLower ? "protocolo" : column); continue; }
          if(lower == "descricao") { result.Add(toLower ? "descricao" : column); continue; }
          if(lower == "unidade_remetente") { result.Add(toLower ? "unidade_remetente" : column); continue; }
          if(lower == "unidade_destinataria") { result.Add(toLower ? "unidade_destinataria" : column); continue; }
          if(lower == "setor_destino") { result.Add(toLower ? "setor_destino" : column); continue; }
          if(lower == "usuario_destino") { result.Add(toLower ? "usuario_destino" : column); continue; }
          if(lower == "dt_envio") { result.Add(toLower ? "dt_envio" : column); continue; }
          if(lower == "dt_recebimento") { result.Add(toLower ? "dt_recebimento" : column); continue; }
          if(lower == "sedex") { result.Add(toLower ? "sedex" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "protocolo", "descricao", "unidade_remetente", "unidade_destinataria", 
                                           "setor_destino", "usuario_destino", "dt_envio", "dt_recebimento", "sedex" }
                          : new string[] { "PROTOCOLO", "DESCRICAO", "UNIDADE_REMETENTE", "UNIDADE_DESTINATARIA", 
                                           "SETOR_DESTINO", "USUARIO_DESTINO", "DT_ENVIO", "DT_RECEBIMENTO", "SEDEX" });
        else
          result.AddRange(toLower
                          ? new string[] { "protocolo", "descricao", "unidade_remetente", "unidade_destinataria", 
                                           "setor_destino", "usuario_destino", "dt_envio", "dt_recebimento", "sedex" }
                          : new string[] { "PROTOCOLO", "DESCRICAO", "UNIDADE_REMETENTE", "UNIDADE_DESTINATARIA", 
                                           "SETOR_DESTINO", "USUARIO_DESTINO", "DT_ENVIO", "DT_RECEBIMENTO", "SEDEX" });

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
        "ly_correspondencia.protocolo"
      });
      if(scColumns.Contains("descricao")) list.Add("ly_correspondencia.descricao");
      if(scColumns.Contains("unidade_remetente")) list.Add("ly_correspondencia.unidade_remetente");
      if(scColumns.Contains("unidade_destinataria")) list.Add("ly_correspondencia.unidade_destinataria");
      if(scColumns.Contains("setor_destino")) list.Add("ly_correspondencia.setor_destino");
      if(scColumns.Contains("usuario_destino")) list.Add("ly_correspondencia.usuario_destino");
      if(scColumns.Contains("dt_envio")) list.Add("ly_correspondencia.dt_envio");
      if(scColumns.Contains("dt_recebimento")) list.Add("ly_correspondencia.dt_recebimento");
      if(scColumns.Contains("sedex")) list.Add("ly_correspondencia.sedex");
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
          "FROM ly_correspondencia " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_correspondencia");
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
        da.TableMappings.Add("Table", "Ly_correspondencia");
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
            if (dr["PROTOCOLO"] != null && string.IsNullOrEmpty(dr["PROTOCOLO"].ToString()))
              dr["PROTOCOLO"] = DBNull.Value;
            if (dr["DESCRICAO"] != null && string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
              dr["DESCRICAO"] = DBNull.Value;
            if (dr["UNIDADE_REMETENTE"] != null && string.IsNullOrEmpty(dr["UNIDADE_REMETENTE"].ToString()))
              dr["UNIDADE_REMETENTE"] = DBNull.Value;
            if (dr["UNIDADE_DESTINATARIA"] != null && string.IsNullOrEmpty(dr["UNIDADE_DESTINATARIA"].ToString()))
              dr["UNIDADE_DESTINATARIA"] = DBNull.Value;
            if (dr["SETOR_DESTINO"] != null && string.IsNullOrEmpty(dr["SETOR_DESTINO"].ToString()))
              dr["SETOR_DESTINO"] = DBNull.Value;
            if (dr["USUARIO_DESTINO"] != null && string.IsNullOrEmpty(dr["USUARIO_DESTINO"].ToString()))
              dr["USUARIO_DESTINO"] = DBNull.Value;
            if (dr["SEDEX"] != null && string.IsNullOrEmpty(dr["SEDEX"].ToString()))
              dr["SEDEX"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["PROTOCOLO"] == null || string.IsNullOrEmpty(dr["PROTOCOLO"].ToString()))
            {
              return "Favor informar o Protocolo.";
            }

            if (dr["PROTOCOLO"] != null && !string.IsNullOrEmpty(dr["PROTOCOLO"].ToString()))
            {
              if (dr["PROTOCOLO"].ToString().Length > 20)
              {
                return "O Protocolo deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["DESCRICAO"] == null || string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              return "Favor informar o Descricao.";
            }

            if (dr["DESCRICAO"] != null && !string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              if (dr["DESCRICAO"].ToString().Length > 500)
              {
                return "O Descricao deve possuir no máximo 500 caracteres.";
              }
            }

            if (dr["UNIDADE_REMETENTE"] == null || string.IsNullOrEmpty(dr["UNIDADE_REMETENTE"].ToString()))
            {
              return "Favor informar o Unidade_remetente.";
            }

            if (dr["UNIDADE_REMETENTE"] != null && !string.IsNullOrEmpty(dr["UNIDADE_REMETENTE"].ToString()))
            {
              if (dr["UNIDADE_REMETENTE"].ToString().Length > 20)
              {
                return "O Unidade_remetente deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["UNIDADE_DESTINATARIA"] == null || string.IsNullOrEmpty(dr["UNIDADE_DESTINATARIA"].ToString()))
            {
              return "Favor informar o Unidade_destinataria.";
            }

            if (dr["UNIDADE_DESTINATARIA"] != null && !string.IsNullOrEmpty(dr["UNIDADE_DESTINATARIA"].ToString()))
            {
              if (dr["UNIDADE_DESTINATARIA"].ToString().Length > 20)
              {
                return "O Unidade_destinataria deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["SETOR_DESTINO"] != null && !string.IsNullOrEmpty(dr["SETOR_DESTINO"].ToString()))
            {
              if (dr["SETOR_DESTINO"].ToString().Length > 20)
              {
                return "O Setor_destino deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["USUARIO_DESTINO"] != null && !string.IsNullOrEmpty(dr["USUARIO_DESTINO"].ToString()))
            {
              if (dr["USUARIO_DESTINO"].ToString().Length > 20)
              {
                return "O Usuario_destino deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["DT_ENVIO"] == null || string.IsNullOrEmpty(dr["DT_ENVIO"].ToString()))
            {
              return "Favor informar o Dt_envio.";
            }

            if (dr["DT_ENVIO"] != null && dr["DT_ENVIO"].ToString() != "")
            {
              dr["DT_ENVIO"] = Convert.ToDateTime(dr["DT_ENVIO"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_RECEBIMENTO"] != null && dr["DT_RECEBIMENTO"].ToString() != "")
            {
              dr["DT_RECEBIMENTO"] = Convert.ToDateTime(dr["DT_RECEBIMENTO"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["SEDEX"] != null && !string.IsNullOrEmpty(dr["SEDEX"].ToString()))
            {
              if (dr["SEDEX"].ToString().Length > 20)
              {
                return "O Sedex deve possuir no máximo 20 caracteres.";
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

    public static Ly_correspondencia Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_correspondencia Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_correspondencia tab = new Ly_correspondencia();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_correspondencia tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @protocolo, string @descricao, string @unidade_remetente, string @unidade_destinataria, 
        string @setor_destino, string @usuario_destino, DateTime? @dt_envio, DateTime? @dt_recebimento, 
        string @sedex
      ) {
        return (Row)Add(new object[] {
          @protocolo, @descricao, @unidade_remetente, @unidade_destinataria, @setor_destino, 
          @usuario_destino, @dt_envio, @dt_recebimento, @sedex
        });
      }

      public Row Find(
        string @protocolo
      ) {
        return (Row)Find(new object[] {
          @protocolo
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
      /// PROTOCOLO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Protocolo { get { return (string)this["protocolo"]; } set { this["protocolo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO: VARCHAR(500), Not Null
      /// </summary>
      public string Descricao { get { return (string)this["descricao"]; } set { this["descricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// UNIDADE_REMETENTE: VARCHAR(20), Not Null
      /// </summary>
      public string Unidade_remetente { get { return (string)this["unidade_remetente"]; } set { this["unidade_remetente"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// UNIDADE_DESTINATARIA: VARCHAR(20), Not Null
      /// </summary>
      public string Unidade_destinataria { get { return (string)this["unidade_destinataria"]; } set { this["unidade_destinataria"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SETOR_DESTINO: VARCHAR(20)
      /// </summary>
      public string Setor_destino { get { return (string)this["setor_destino"]; } set { this["setor_destino"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// USUARIO_DESTINO: VARCHAR(20)
      /// </summary>
      public string Usuario_destino { get { return (string)this["usuario_destino"]; } set { this["usuario_destino"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_ENVIO: DATETIME, Not Null
      /// </summary>
      public DateTime? Dt_envio { get { return (DateTime?)this["dt_envio"]; } set { this["dt_envio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_RECEBIMENTO: DATETIME
      /// </summary>
      public DateTime? Dt_recebimento { get { return (DateTime?)this["dt_recebimento"]; } set { this["dt_recebimento"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SEDEX: VARCHAR(20)
      /// </summary>
      public string Sedex { get { return (string)this["sedex"]; } set { this["sedex"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @protocolo, string @descricao, string @unidade_remetente, string @unidade_destinataria, 
                               DateTime? @dt_envio) {
        return Insert(connection,
                      @protocolo, @descricao, @unidade_remetente, @unidade_destinataria, @dt_envio,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @protocolo, string @descricao, string @unidade_remetente, string @unidade_destinataria, 
                               DateTime? @dt_envio,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_correspondencia(),
          new string[] { "protocolo", "descricao", "unidade_remetente", "unidade_destinataria", 
                         "dt_envio" },
          new object[] { @protocolo, @descricao, @unidade_remetente, @unidade_destinataria, 
                         @dt_envio },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkProtocolo,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_correspondencia(),
          new string[] { "protocolo" },
          new object[] { pkProtocolo },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkProtocolo) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_correspondencia(),
          new string[] { "protocolo" },
          new object[] { pkProtocolo }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkProtocolo) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_correspondencia(),
          new string[] { "protocolo" },
          new object[] { pkProtocolo }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Protocolo { get { return (string)row["protocolo", DataRowVersion.Original]; } }
        public string Descricao { get { return (string)row["descricao", DataRowVersion.Original]; } }
        public string Unidade_remetente { get { return (string)row["unidade_remetente", DataRowVersion.Original]; } }
        public string Unidade_destinataria { get { return (string)row["unidade_destinataria", DataRowVersion.Original]; } }
        public string Setor_destino { get { return (string)row["setor_destino", DataRowVersion.Original]; } }
        public string Usuario_destino { get { return (string)row["usuario_destino", DataRowVersion.Original]; } }
        public DateTime? Dt_envio { get { return (DateTime?)row["dt_envio", DataRowVersion.Original]; } }
        public DateTime? Dt_recebimento { get { return (DateTime?)row["dt_recebimento", DataRowVersion.Original]; } }
        public string Sedex { get { return (string)row["sedex", DataRowVersion.Original]; } }

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
    #region Coluna: PROTOCOLO
    private ColumnProtocolo pvColProtocolo;

    public sealed class ColumnProtocolo : TDataColumn {

      public ColumnProtocolo() {
        this.ColumnName = "protocolo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_correspondencia.protocolo"; } }
      public override string OriginalCol { get { return "protocolo"; } }
      public override string RefCol { get { return "LY_CORRESPONDENCIA.PROTOCOLO"; } }
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
      public override string FullCol { get { return "ly_correspondencia.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "LY_CORRESPONDENCIA.DESCRICAO"; } }
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

    #region Coluna: UNIDADE_REMETENTE
    private ColumnUnidade_remetente pvColUnidade_remetente;

    public sealed class ColumnUnidade_remetente : TDataColumn {

      public ColumnUnidade_remetente() {
        this.ColumnName = "unidade_remetente";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_correspondencia.unidade_remetente"; } }
      public override string OriginalCol { get { return "unidade_remetente"; } }
      public override string RefCol { get { return "LY_CORRESPONDENCIA.UNIDADE_REMETENTE"; } }
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

    #region Coluna: UNIDADE_DESTINATARIA
    private ColumnUnidade_destinataria pvColUnidade_destinataria;

    public sealed class ColumnUnidade_destinataria : TDataColumn {

      public ColumnUnidade_destinataria() {
        this.ColumnName = "unidade_destinataria";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_correspondencia.unidade_destinataria"; } }
      public override string OriginalCol { get { return "unidade_destinataria"; } }
      public override string RefCol { get { return "LY_CORRESPONDENCIA.UNIDADE_DESTINATARIA"; } }
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

    #region Coluna: SETOR_DESTINO
    private ColumnSetor_destino pvColSetor_destino;

    public sealed class ColumnSetor_destino : TDataColumn {

      public ColumnSetor_destino() {
        this.ColumnName = "setor_destino";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_correspondencia.setor_destino"; } }
      public override string OriginalCol { get { return "setor_destino"; } }
      public override string RefCol { get { return "LY_CORRESPONDENCIA.SETOR_DESTINO"; } }
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

    #region Coluna: USUARIO_DESTINO
    private ColumnUsuario_destino pvColUsuario_destino;

    public sealed class ColumnUsuario_destino : TDataColumn {

      public ColumnUsuario_destino() {
        this.ColumnName = "usuario_destino";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_correspondencia.usuario_destino"; } }
      public override string OriginalCol { get { return "usuario_destino"; } }
      public override string RefCol { get { return "LY_CORRESPONDENCIA.USUARIO_DESTINO"; } }
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

    #region Coluna: DT_ENVIO
    private ColumnDt_envio pvColDt_envio;

    public sealed class ColumnDt_envio : TDataColumn {

      public ColumnDt_envio() {
        this.ColumnName = "dt_envio";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_correspondencia.dt_envio"; } }
      public override string OriginalCol { get { return "dt_envio"; } }
      public override string RefCol { get { return "LY_CORRESPONDENCIA.DT_ENVIO"; } }
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

    #region Coluna: DT_RECEBIMENTO
    private ColumnDt_recebimento pvColDt_recebimento;

    public sealed class ColumnDt_recebimento : TDataColumn {

      public ColumnDt_recebimento() {
        this.ColumnName = "dt_recebimento";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_correspondencia.dt_recebimento"; } }
      public override string OriginalCol { get { return "dt_recebimento"; } }
      public override string RefCol { get { return "LY_CORRESPONDENCIA.DT_RECEBIMENTO"; } }
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

    #region Coluna: SEDEX
    private ColumnSedex pvColSedex;

    public sealed class ColumnSedex : TDataColumn {

      public ColumnSedex() {
        this.ColumnName = "sedex";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_correspondencia.sedex"; } }
      public override string OriginalCol { get { return "sedex"; } }
      public override string RefCol { get { return "LY_CORRESPONDENCIA.SEDEX"; } }
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

  public partial class Ly_correspondencia_docs : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_correspondencia_docs() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_correspondencia_docs");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_correspondencia_docs");
      }
      catch{}
    }

    public Ly_correspondencia_docs() {
      pvColProtocolo = new ColumnProtocolo(); Columns.Add(pvColProtocolo);
      pvColSeq_doc = new ColumnSeq_doc(); Columns.Add(pvColSeq_doc);
      pvColDocumento = new ColumnDocumento(); Columns.Add(pvColDocumento);
      pvColDescricao = new ColumnDescricao(); Columns.Add(pvColDescricao);
      pvColRecebido = new ColumnRecebido(); Columns.Add(pvColRecebido);
      pvColSolicitacao = new ColumnSolicitacao(); Columns.Add(pvColSolicitacao);
      pvColAluno = new ColumnAluno(); Columns.Add(pvColAluno);
      pvColNome_aluno = new ColumnNome_aluno(); Columns.Add(pvColNome_aluno);
      pvColNum_func = new ColumnNum_func(); Columns.Add(pvColNum_func);
      pvColNome_docente = new ColumnNome_docente(); Columns.Add(pvColNome_docente);
      pvColDt_envio = new ColumnDt_envio(); Columns.Add(pvColDt_envio);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_correspondencia_docs");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @protocolo, string @documento, string @descricao) {
      Row row = NewRow();

      row.Protocolo = @protocolo;
      row.Documento = @documento;
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
      get { return "LY_CORRESPONDENCIA_DOCS"; }
    }

    protected override sealed string Name { get { return this.TableName; } }

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
      SetPrimaryKey(new DataColumn[] { pvColProtocolo, pvColSeq_doc });
      pvColSeq_doc.AllowDBNull = true;
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_correspondencia_docs_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_correspondencia_docs_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_correspondencia_docs_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_correspondencia_docs_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkProtocolo", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "PROTOCOLO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkSeq_doc", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "SEQ_DOC", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("protocolo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PROTOCOLO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("seq_doc", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "SEQ_DOC", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("documento", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "DOCUMENTO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("descricao", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "DESCRICAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("recebido", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "RECEBIDO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("solicitacao", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "SOLICITACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "protocolo") { result.Add(toLower ? "protocolo" : column); continue; }
          if(lower == "seq_doc") { result.Add(toLower ? "seq_doc" : column); continue; }
          if(lower == "documento") { result.Add(toLower ? "documento" : column); continue; }
          if(lower == "descricao") { result.Add(toLower ? "descricao" : column); continue; }
          if(lower == "recebido") { result.Add(toLower ? "recebido" : column); continue; }
          if(lower == "solicitacao") { result.Add(toLower ? "solicitacao" : column); continue; }
          if(!tabPrincipal) {
            if(lower == "aluno") { result.Add(toLower ? "aluno" : column); continue; }
            if(lower == "nome_aluno") { result.Add(toLower ? "nome_aluno" : column); continue; }
            if(lower == "num_func") { result.Add(toLower ? "num_func" : column); continue; }
            if(lower == "nome_docente") { result.Add(toLower ? "nome_docente" : column); continue; }
            if(lower == "dt_envio") { result.Add(toLower ? "dt_envio" : column); continue; }
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "protocolo", "seq_doc", "documento", "descricao", "recebido", "solicitacao" }
                          : new string[] { "PROTOCOLO", "SEQ_DOC", "DOCUMENTO", "DESCRICAO", "RECEBIDO", "SOLICITACAO" });
        else
          result.AddRange(toLower
                          ? new string[] { "protocolo", "seq_doc", "documento", "descricao", "recebido", "solicitacao", 
                                           "aluno", "nome_aluno", "num_func", "nome_docente", "dt_envio" }
                          : new string[] { "PROTOCOLO", "SEQ_DOC", "DOCUMENTO", "DESCRICAO", "RECEBIDO", "SOLICITACAO", 
                                           "ALUNO", "NOME_ALUNO", "NUM_FUNC", "NOME_DOCENTE", "DT_ENVIO" });

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
        "ly_correspondencia_docs.protocolo", "ly_correspondencia_docs.seq_doc"
      });
      if(scColumns.Contains("documento")) list.Add("ly_correspondencia_docs.documento");
      if(scColumns.Contains("descricao")) list.Add("ly_correspondencia_docs.descricao");
      if(scColumns.Contains("recebido")) list.Add("ly_correspondencia_docs.recebido");
      if(scColumns.Contains("solicitacao")) list.Add("ly_correspondencia_docs.solicitacao");
      if(scColumns.Contains("aluno")) list.Add("ly_solicitacao_serv.aluno");
      if(scColumns.Contains("nome_aluno")) list.Add("ly_aluno.nome_compl nome_aluno");
      if(scColumns.Contains("num_func")) list.Add("ly_solicitacao_serv.num_func");
      if(scColumns.Contains("nome_docente")) list.Add("ly_pessoa.nome_compl nome_docente");
      if(scColumns.Contains("dt_envio")) list.Add("ly_correspondencia.dt_envio");
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
          "FROM ly_correspondencia_docs " +
               "INNER JOIN ly_correspondencia " +
                 "ON ly_correspondencia.protocolo = ly_correspondencia_docs.protocolo " +
               "LEFT JOIN ly_solicitacao_serv " +
                         "LEFT JOIN ly_aluno " +
                           "ON ly_aluno.aluno = ly_solicitacao_serv.aluno " +
                         "LEFT JOIN ly_docente " +
                           "ON ly_docente.num_func = ly_solicitacao_serv.num_func " +
                 "ON ly_solicitacao_serv.solicitacao = ly_correspondencia_docs.solicitacao " +
                  "LEFT JOIN ly_pessoa " +
                       "ON ly_pessoa.pessoa = ly_docente.pessoa " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_correspondencia_docs");
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
        da.TableMappings.Add("Table", "Ly_correspondencia_docs");
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
            if (dr["PROTOCOLO"] != null && string.IsNullOrEmpty(dr["PROTOCOLO"].ToString()))
              dr["PROTOCOLO"] = DBNull.Value;
            if (dr["DOCUMENTO"] != null && string.IsNullOrEmpty(dr["DOCUMENTO"].ToString()))
              dr["DOCUMENTO"] = DBNull.Value;
            if (dr["DESCRICAO"] != null && string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
              dr["DESCRICAO"] = DBNull.Value;
            if (dr["RECEBIDO"] != null && string.IsNullOrEmpty(dr["RECEBIDO"].ToString()))
              dr["RECEBIDO"] = DBNull.Value;
            if (dr["SOLICITACAO"] != null && string.IsNullOrEmpty(dr["SOLICITACAO"].ToString()))
              dr["SOLICITACAO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["PROTOCOLO"] == null || string.IsNullOrEmpty(dr["PROTOCOLO"].ToString()))
            {
              return "Favor informar o Protocolo.";
            }

            if (dr["PROTOCOLO"] != null && !string.IsNullOrEmpty(dr["PROTOCOLO"].ToString()))
            {
              if (dr["PROTOCOLO"].ToString().Length > 20)
              {
                return "O Protocolo deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["DOCUMENTO"] == null || string.IsNullOrEmpty(dr["DOCUMENTO"].ToString()))
            {
              return "Favor informar o Documento.";
            }

            if (dr["DOCUMENTO"] != null && !string.IsNullOrEmpty(dr["DOCUMENTO"].ToString()))
            {
              if (dr["DOCUMENTO"].ToString().Length > 20)
              {
                return "O Documento deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["DESCRICAO"] == null || string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              return "Favor informar o Descricao.";
            }

            if (dr["DESCRICAO"] != null && !string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              if (dr["DESCRICAO"].ToString().Length > 100)
              {
                return "O Descricao deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["RECEBIDO"] != null && !string.IsNullOrEmpty(dr["RECEBIDO"].ToString()))
            {
              if (dr["RECEBIDO"].ToString().Length > 1)
              {
                return "O Recebido deve possuir no máximo 1 caractere.";
              }
            }

            if (dr["SOLICITACAO"] != null && !string.IsNullOrEmpty(dr["SOLICITACAO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["SOLICITACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["SOLICITACAO"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["SOLICITACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["SOLICITACAO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Solicitacao deve ser um valor inteiro com 10 dígitos no máximo.";
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

    public static Ly_correspondencia_docs Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_correspondencia_docs Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_correspondencia_docs tab = new Ly_correspondencia_docs();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_correspondencia_docs tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @protocolo, decimal? @seq_doc, string @documento, string @descricao, string @recebido, 
        decimal? @solicitacao
      ) {
        return (Row)Add(new object[] {
          @protocolo, @seq_doc, @documento, @descricao, @recebido, @solicitacao
        });
      }

      public Row Find(
        string @protocolo, decimal? @seq_doc
      ) {
        return (Row)Find(new object[] {
          @protocolo, @seq_doc
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
      /// PROTOCOLO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Protocolo { get { return (string)this["protocolo"]; } set { this["protocolo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SEQ_DOC: NUMERIC(10), Primary Key, AutoNumber, Not Null
      /// </summary>
      public decimal? Seq_doc { get { return (decimal?)this["seq_doc"]; } set { this["seq_doc"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DOCUMENTO: VARCHAR(20), Not Null
      /// </summary>
      public string Documento { get { return (string)this["documento"]; } set { this["documento"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO: VARCHAR(100), Not Null
      /// </summary>
      public string Descricao { get { return (string)this["descricao"]; } set { this["descricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// RECEBIDO: CHAR(1)
      /// </summary>
      public string Recebido { get { return (string)this["recebido"]; } set { this["recebido"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SOLICITACAO: NUMERIC(10)
      /// </summary>
      public decimal? Solicitacao { get { return (decimal?)this["solicitacao"]; } set { this["solicitacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ALUNO: VARCHAR(20)
      /// </summary>
      public string Aluno { get { return (string)this["aluno"]; } set { this["aluno"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME_COMPL: VARCHAR(100), Not Null
      /// </summary>
      public string Nome_aluno { get { return (string)this["nome_aluno"]; } set { this["nome_aluno"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NUM_FUNC: NUMERIC(15)
      /// </summary>
      public decimal? Num_func { get { return (decimal?)this["num_func"]; } set { this["num_func"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME_COMPL: VARCHAR(100), Not Null
      /// </summary>
      public string Nome_docente { get { return (string)this["nome_docente"]; } set { this["nome_docente"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_ENVIO: DATETIME, Not Null
      /// </summary>
      public DateTime? Dt_envio { get { return (DateTime?)this["dt_envio"]; } set { this["dt_envio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @protocolo, string @documento, string @descricao) {
        return Insert(connection,
                      @protocolo, @documento, @descricao,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @protocolo, string @documento, string @descricao,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_correspondencia_docs(),
          new string[] { "protocolo", "documento", "descricao" },
          new object[] { @protocolo, @documento, @descricao },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkProtocolo, decimal? pkSeq_doc,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_correspondencia_docs(),
          new string[] { "protocolo", "seq_doc" },
          new object[] { pkProtocolo, pkSeq_doc },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkProtocolo, decimal? pkSeq_doc) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_correspondencia_docs(),
          new string[] { "protocolo", "seq_doc" },
          new object[] { pkProtocolo, pkSeq_doc }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkProtocolo, decimal? pkSeq_doc) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_correspondencia_docs(),
          new string[] { "protocolo", "seq_doc" },
          new object[] { pkProtocolo, pkSeq_doc }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Protocolo { get { return (string)row["protocolo", DataRowVersion.Original]; } }
        public decimal? Seq_doc { get { return (decimal?)row["seq_doc", DataRowVersion.Original]; } }
        public string Documento { get { return (string)row["documento", DataRowVersion.Original]; } }
        public string Descricao { get { return (string)row["descricao", DataRowVersion.Original]; } }
        public string Recebido { get { return (string)row["recebido", DataRowVersion.Original]; } }
        public decimal? Solicitacao { get { return (decimal?)row["solicitacao", DataRowVersion.Original]; } }
        public string Aluno { get { return (string)row["aluno", DataRowVersion.Original]; } }
        public string Nome_aluno { get { return (string)row["nome_aluno", DataRowVersion.Original]; } }
        public decimal? Num_func { get { return (decimal?)row["num_func", DataRowVersion.Original]; } }
        public string Nome_docente { get { return (string)row["nome_docente", DataRowVersion.Original]; } }
        public DateTime? Dt_envio { get { return (DateTime?)row["dt_envio", DataRowVersion.Original]; } }

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
    #region Coluna: PROTOCOLO
    private ColumnProtocolo pvColProtocolo;

    public sealed class ColumnProtocolo : TDataColumn {

      public ColumnProtocolo() {
        this.ColumnName = "protocolo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_correspondencia_docs.protocolo"; } }
      public override string OriginalCol { get { return "protocolo"; } }
      public override string RefCol { get { return "LY_CORRESPONDENCIA_DOCS.PROTOCOLO"; } }
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

    #region Coluna: SEQ_DOC
    private ColumnSeq_doc pvColSeq_doc;

    public sealed class ColumnSeq_doc : TDataColumn {

      public ColumnSeq_doc() {
        this.ColumnName = "seq_doc";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_correspondencia_docs.seq_doc"; } }
      public override string OriginalCol { get { return "seq_doc"; } }
      public override string RefCol { get { return "LY_CORRESPONDENCIA_DOCS.SEQ_DOC"; } }
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

    #region Coluna: DOCUMENTO
    private ColumnDocumento pvColDocumento;

    public sealed class ColumnDocumento : TDataColumn {

      public ColumnDocumento() {
        this.ColumnName = "documento";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_correspondencia_docs.documento"; } }
      public override string OriginalCol { get { return "documento"; } }
      public override string RefCol { get { return "LY_CORRESPONDENCIA_DOCS.DOCUMENTO"; } }
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
      public override string FullCol { get { return "ly_correspondencia_docs.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "LY_CORRESPONDENCIA_DOCS.DESCRICAO"; } }
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

    #region Coluna: RECEBIDO
    private ColumnRecebido pvColRecebido;

    public sealed class ColumnRecebido : TDataColumn {

      public ColumnRecebido() {
        this.ColumnName = "recebido";
        this.DataType = typeof(string);
        this.DefaultValue = "N";

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_correspondencia_docs.recebido"; } }
      public override string OriginalCol { get { return "recebido"; } }
      public override string RefCol { get { return "LY_CORRESPONDENCIA_DOCS.RECEBIDO"; } }
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

    #region Coluna: SOLICITACAO
    private ColumnSolicitacao pvColSolicitacao;

    public sealed class ColumnSolicitacao : TDataColumn {

      public ColumnSolicitacao() {
        this.ColumnName = "solicitacao";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_correspondencia_docs.solicitacao"; } }
      public override string OriginalCol { get { return "solicitacao"; } }
      public override string RefCol { get { return "LY_CORRESPONDENCIA_DOCS.SOLICITACAO"; } }
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

    #region Coluna: ALUNO
    private ColumnAluno pvColAluno;

    public sealed class ColumnAluno : TDataColumn {

      public ColumnAluno() {
        this.ColumnName = "aluno";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Aluno";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_solicitacao_serv.aluno"; } }
      public override string OriginalCol { get { return "aluno"; } }
      public override string RefCol { get { return "LY_SOLICITACAO_SERV.ALUNO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 20; } }
      public override string SqlServerType { get { return "VARCHAR(20)"; } }

    }
    #endregion

    #region Coluna: NOME_ALUNO
    private ColumnNome_aluno pvColNome_aluno;

    public sealed class ColumnNome_aluno : TDataColumn {

      public ColumnNome_aluno() {
        this.ColumnName = "nome_aluno";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "nome completo";}
      protected override string GetCaption(){return "Nome";}
      protected override string GetDescricao(){return "Nome do aluno completo";}
      public override string FullCol { get { return "ly_aluno.nome_compl"; } }
      public override string OriginalCol { get { return "nome_compl"; } }
      public override string RefCol { get { return "LY_ALUNO.NOME_COMPL"; } }
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

    #region Coluna: NUM_FUNC
    private ColumnNum_func pvColNum_func;

    public sealed class ColumnNum_func : TDataColumn {

      public ColumnNum_func() {
        this.ColumnName = "num_func";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_solicitacao_serv.num_func"; } }
      public override string OriginalCol { get { return "num_func"; } }
      public override string RefCol { get { return "LY_SOLICITACAO_SERV.NUM_FUNC"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
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

    #region Coluna: NOME_DOCENTE
    private ColumnNome_docente pvColNome_docente;

    public sealed class ColumnNome_docente : TDataColumn {

      public ColumnNome_docente() {
        this.ColumnName = "nome_docente";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "nome completo";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
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

    #region Coluna: DT_ENVIO
    private ColumnDt_envio pvColDt_envio;

    public sealed class ColumnDt_envio : TDataColumn {

      public ColumnDt_envio() {
        this.ColumnName = "dt_envio";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_correspondencia.dt_envio"; } }
      public override string OriginalCol { get { return "dt_envio"; } }
      public override string RefCol { get { return "LY_CORRESPONDENCIA.DT_ENVIO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 10; } }
      public override string SqlServerType { get { return "DATETIME"; } }

    }
    #endregion
    #endregion
  }

  public partial class Ly_documentos_necessarios : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_documentos_necessarios() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_documentos_necessarios");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_documentos_necessarios");
      }
      catch{}
    }

    public Ly_documentos_necessarios() {
      pvColServico = new ColumnServico(); Columns.Add(pvColServico);
      pvColDocumento = new ColumnDocumento(); Columns.Add(pvColDocumento);
      pvColNome = new ColumnNome(); Columns.Add(pvColNome);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_documentos_necessarios");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @servico, string @documento) {
      Row row = NewRow();

      row.Servico = @servico;
      row.Documento = @documento;

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
      get { return "LY_DOCUMENTOS_NECESSARIOS"; }
    }

    protected override sealed string Name { get { return this.TableName; } }

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
      SetPrimaryKey(new DataColumn[] { pvColServico, pvColDocumento });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_documentos_necessari_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_documentos_necessari_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_documentos_necessari_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_documentos_necessari_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkServico", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "SERVICO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkDocumento", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "DOCUMENTO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("servico", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SERVICO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("documento", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "DOCUMENTO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
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
          if(lower == "servico") { result.Add(toLower ? "servico" : column); continue; }
          if(lower == "documento") { result.Add(toLower ? "documento" : column); continue; }
          if(lower == "nome") { result.Add(toLower ? "nome" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "servico", "documento", "nome" }
                          : new string[] { "SERVICO", "DOCUMENTO", "NOME" });
        else
          result.AddRange(toLower
                          ? new string[] { "servico", "documento", "nome" }
                          : new string[] { "SERVICO", "DOCUMENTO", "NOME" });

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
        "ly_documentos_necessarios.servico", "ly_documentos_necessarios.documento"
      });
      if(scColumns.Contains("nome")) list.Add("ly_documentos_necessarios.nome");
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
          "FROM ly_documentos_necessarios " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_documentos_necessarios");
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
        da.TableMappings.Add("Table", "Ly_documentos_necessarios");
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
            if (dr["SERVICO"] != null && string.IsNullOrEmpty(dr["SERVICO"].ToString()))
              dr["SERVICO"] = DBNull.Value;
            if (dr["DOCUMENTO"] != null && string.IsNullOrEmpty(dr["DOCUMENTO"].ToString()))
              dr["DOCUMENTO"] = DBNull.Value;
            if (dr["NOME"] != null && string.IsNullOrEmpty(dr["NOME"].ToString()))
              dr["NOME"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["SERVICO"] == null || string.IsNullOrEmpty(dr["SERVICO"].ToString()))
            {
              return "Favor informar o Servico.";
            }

            if (dr["SERVICO"] != null && !string.IsNullOrEmpty(dr["SERVICO"].ToString()))
            {
              if (dr["SERVICO"].ToString().Length > 20)
              {
                return "O Servico deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["DOCUMENTO"] == null || string.IsNullOrEmpty(dr["DOCUMENTO"].ToString()))
            {
              return "Favor informar o Documento.";
            }

            if (dr["DOCUMENTO"] != null && !string.IsNullOrEmpty(dr["DOCUMENTO"].ToString()))
            {
              if (dr["DOCUMENTO"].ToString().Length > 20)
              {
                return "O Documento deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["NOME"] != null && !string.IsNullOrEmpty(dr["NOME"].ToString()))
            {
              if (dr["NOME"].ToString().Length > 100)
              {
                return "O Nome deve possuir no máximo 100 caracteres.";
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

    public static Ly_documentos_necessarios Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_documentos_necessarios Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_documentos_necessarios tab = new Ly_documentos_necessarios();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_documentos_necessarios tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @servico, string @documento, string @nome
      ) {
        return (Row)Add(new object[] {
          @servico, @documento, @nome
        });
      }

      public Row Find(
        string @servico, string @documento
      ) {
        return (Row)Find(new object[] {
          @servico, @documento
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
      /// SERVICO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Servico { get { return (string)this["servico"]; } set { this["servico"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DOCUMENTO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Documento { get { return (string)this["documento"]; } set { this["documento"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(100)
      /// </summary>
      public string Nome { get { return (string)this["nome"]; } set { this["nome"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @servico, string @documento) {
        return Insert(connection,
                      @servico, @documento,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @servico, string @documento,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_documentos_necessarios(),
          new string[] { "servico", "documento" },
          new object[] { @servico, @documento },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkServico, string pkDocumento,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_documentos_necessarios(),
          new string[] { "servico", "documento" },
          new object[] { pkServico, pkDocumento },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkServico, string pkDocumento) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_documentos_necessarios(),
          new string[] { "servico", "documento" },
          new object[] { pkServico, pkDocumento }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkServico, string pkDocumento) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_documentos_necessarios(),
          new string[] { "servico", "documento" },
          new object[] { pkServico, pkDocumento }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Servico { get { return (string)row["servico", DataRowVersion.Original]; } }
        public string Documento { get { return (string)row["documento", DataRowVersion.Original]; } }
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
    #region Coluna: SERVICO
    private ColumnServico pvColServico;

    public sealed class ColumnServico : TDataColumn {

      public ColumnServico() {
        this.ColumnName = "servico";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_documentos_necessarios.servico"; } }
      public override string OriginalCol { get { return "servico"; } }
      public override string RefCol { get { return "LY_DOCUMENTOS_NECESSARIOS.SERVICO"; } }
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

    #region Coluna: DOCUMENTO
    private ColumnDocumento pvColDocumento;

    public sealed class ColumnDocumento : TDataColumn {

      public ColumnDocumento() {
        this.ColumnName = "documento";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_documentos_necessarios.documento"; } }
      public override string OriginalCol { get { return "documento"; } }
      public override string RefCol { get { return "LY_DOCUMENTOS_NECESSARIOS.DOCUMENTO"; } }
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
      public override string FullCol { get { return "ly_documentos_necessarios.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "LY_DOCUMENTOS_NECESSARIOS.NOME"; } }
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

  public partial class Ly_fluxo_de_andamento : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_fluxo_de_andamento() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_fluxo_de_andamento");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_fluxo_de_andamento");
      }
      catch{}
    }

    public Ly_fluxo_de_andamento() {
      pvColServico = new ColumnServico(); Columns.Add(pvColServico);
      pvColPasso = new ColumnPasso(); Columns.Add(pvColPasso);
      pvColProx_passo = new ColumnProx_passo(); Columns.Add(pvColProx_passo);
      pvColSetor = new ColumnSetor(); Columns.Add(pvColSetor);
      pvColTipo_andamento = new ColumnTipo_andamento(); Columns.Add(pvColTipo_andamento);
      pvColDescricao = new ColumnDescricao(); Columns.Add(pvColDescricao);
      pvColPrazo = new ColumnPrazo(); Columns.Add(pvColPrazo);
      pvColTipo_mensagem = new ColumnTipo_mensagem(); Columns.Add(pvColTipo_mensagem);
      pvColMensagem = new ColumnMensagem(); Columns.Add(pvColMensagem);
      pvColNum_dias = new ColumnNum_dias(); Columns.Add(pvColNum_dias);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_fluxo_de_andamento");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @servico, decimal? @passo, string @setor) {
      Row row = NewRow();

      row.Servico = @servico;
      row.Passo = @passo;
      row.Setor = @setor;

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
      get { return "LY_FLUXO_DE_ANDAMENTO"; }
    }

    protected override sealed string Name { get { return "Fluxo de Andamento de Serviço"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColServico, pvColPasso });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_fluxo_de_andamento_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_fluxo_de_andamento_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_fluxo_de_andamento_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_fluxo_de_andamento_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkServico", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "SERVICO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkPasso", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "PASSO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("servico", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SERVICO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("passo", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "PASSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("prox_passo", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "PROX_PASSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("setor", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SETOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("tipo_andamento", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TIPO_ANDAMENTO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("descricao", OleDbType.VarChar, 355, ParameterDirection.InputOutput, true, 0, 0, "DESCRICAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("prazo", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "PRAZO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("tipo_mensagem", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TIPO_MENSAGEM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("mensagem", OleDbType.VarChar, 355, ParameterDirection.InputOutput, true, 0, 0, "MENSAGEM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("num_dias", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "NUM_DIAS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "servico") { result.Add(toLower ? "servico" : column); continue; }
          if(lower == "passo") { result.Add(toLower ? "passo" : column); continue; }
          if(lower == "prox_passo") { result.Add(toLower ? "prox_passo" : column); continue; }
          if(lower == "setor") { result.Add(toLower ? "setor" : column); continue; }
          if(lower == "tipo_andamento") { result.Add(toLower ? "tipo_andamento" : column); continue; }
          if(lower == "descricao") { result.Add(toLower ? "descricao" : column); continue; }
          if(lower == "prazo") { result.Add(toLower ? "prazo" : column); continue; }
          if(lower == "tipo_mensagem") { result.Add(toLower ? "tipo_mensagem" : column); continue; }
          if(lower == "mensagem") { result.Add(toLower ? "mensagem" : column); continue; }
          if(lower == "num_dias") { result.Add(toLower ? "num_dias" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "servico", "passo", "prox_passo", "setor", "tipo_andamento", "descricao", 
                                           "prazo", "tipo_mensagem", "mensagem", "num_dias" }
                          : new string[] { "SERVICO", "PASSO", "PROX_PASSO", "SETOR", "TIPO_ANDAMENTO", "DESCRICAO", 
                                           "PRAZO", "TIPO_MENSAGEM", "MENSAGEM", "NUM_DIAS" });
        else
          result.AddRange(toLower
                          ? new string[] { "servico", "passo", "prox_passo", "setor", "tipo_andamento", "descricao", 
                                           "prazo", "tipo_mensagem", "mensagem", "num_dias" }
                          : new string[] { "SERVICO", "PASSO", "PROX_PASSO", "SETOR", "TIPO_ANDAMENTO", "DESCRICAO", 
                                           "PRAZO", "TIPO_MENSAGEM", "MENSAGEM", "NUM_DIAS" });

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
        "ly_fluxo_de_andamento.servico", "ly_fluxo_de_andamento.passo"
      });
      if(scColumns.Contains("prox_passo")) list.Add("ly_fluxo_de_andamento.prox_passo");
      if(scColumns.Contains("setor")) list.Add("ly_fluxo_de_andamento.setor");
      if(scColumns.Contains("tipo_andamento")) list.Add("ly_fluxo_de_andamento.tipo_andamento");
      if(scColumns.Contains("descricao")) list.Add("ly_fluxo_de_andamento.descricao");
      if(scColumns.Contains("prazo")) list.Add("ly_fluxo_de_andamento.prazo");
      if(scColumns.Contains("tipo_mensagem")) list.Add("ly_fluxo_de_andamento.tipo_mensagem");
      if(scColumns.Contains("mensagem")) list.Add("ly_fluxo_de_andamento.mensagem");
      if(scColumns.Contains("num_dias")) list.Add("ly_fluxo_de_andamento.num_dias");
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
          "FROM ly_fluxo_de_andamento " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_fluxo_de_andamento");
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
        da.TableMappings.Add("Table", "Ly_fluxo_de_andamento");
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
            if (dr["SERVICO"] != null && string.IsNullOrEmpty(dr["SERVICO"].ToString()))
              dr["SERVICO"] = DBNull.Value;
            if (dr["PASSO"] != null && string.IsNullOrEmpty(dr["PASSO"].ToString()))
              dr["PASSO"] = DBNull.Value;
            if (dr["PROX_PASSO"] != null && string.IsNullOrEmpty(dr["PROX_PASSO"].ToString()))
              dr["PROX_PASSO"] = DBNull.Value;
            if (dr["SETOR"] != null && string.IsNullOrEmpty(dr["SETOR"].ToString()))
              dr["SETOR"] = DBNull.Value;
            if (dr["TIPO_ANDAMENTO"] != null && string.IsNullOrEmpty(dr["TIPO_ANDAMENTO"].ToString()))
              dr["TIPO_ANDAMENTO"] = DBNull.Value;
            if (dr["DESCRICAO"] != null && string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
              dr["DESCRICAO"] = DBNull.Value;
            if (dr["PRAZO"] != null && string.IsNullOrEmpty(dr["PRAZO"].ToString()))
              dr["PRAZO"] = DBNull.Value;
            if (dr["TIPO_MENSAGEM"] != null && string.IsNullOrEmpty(dr["TIPO_MENSAGEM"].ToString()))
              dr["TIPO_MENSAGEM"] = DBNull.Value;
            if (dr["MENSAGEM"] != null && string.IsNullOrEmpty(dr["MENSAGEM"].ToString()))
              dr["MENSAGEM"] = DBNull.Value;
            if (dr["NUM_DIAS"] != null && string.IsNullOrEmpty(dr["NUM_DIAS"].ToString()))
              dr["NUM_DIAS"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["SERVICO"] == null || string.IsNullOrEmpty(dr["SERVICO"].ToString()))
            {
              return "Favor informar o serviço.";
            }

            if (dr["SERVICO"] != null && !string.IsNullOrEmpty(dr["SERVICO"].ToString()))
            {
              if (dr["SERVICO"].ToString().Length > 20)
              {
                return "O serviço deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["PASSO"] == null || string.IsNullOrEmpty(dr["PASSO"].ToString()))
            {
              return "Favor informar o passo.";
            }

            if (dr["PASSO"] != null && !string.IsNullOrEmpty(dr["PASSO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["PASSO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PASSO"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["PASSO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PASSO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O passo deve ser um valor inteiro com 3 dígitos no máximo.";
              }
            }

            if (dr["PROX_PASSO"] != null && !string.IsNullOrEmpty(dr["PROX_PASSO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["PROX_PASSO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PROX_PASSO"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["PROX_PASSO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PROX_PASSO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O próximo passo deve ser um valor inteiro com 3 dígitos no máximo.";
              }
            }

            if (dr["SETOR"] == null || string.IsNullOrEmpty(dr["SETOR"].ToString()))
            {
              return "Favor informar o setor.";
            }

            if (dr["SETOR"] != null && !string.IsNullOrEmpty(dr["SETOR"].ToString()))
            {
              if (dr["SETOR"].ToString().Length > 15)
              {
                return "O setor deve possuir no máximo 15 caracteres.";
              }
            }

            if (dr["TIPO_ANDAMENTO"] != null && !string.IsNullOrEmpty(dr["TIPO_ANDAMENTO"].ToString()))
            {
              if (dr["TIPO_ANDAMENTO"].ToString().Length > 15)
              {
                return "O tipo de andamento deve possuir no máximo 15 caracteres.";
              }

              if (dr["TIPO_ANDAMENTO"] != null && !string.IsNullOrEmpty(dr["TIPO_ANDAMENTO"].ToString()))
              {
                List<object> lstTIPO_ANDAMENTO = new List<object>(new object[]{"Execução", "Deferimento", "Atendimento", "Entrega", "Envio Corresp.", "Receb.Corresp."});
                if(!lstTIPO_ANDAMENTO.Contains(dr["TIPO_ANDAMENTO"].ToString()))
                {
                  return "Os valores permitidos para o tipo de andamento são ''Execução'', ''Deferimento'', ''Atendimento'', ''Entrega'', ''Envio Corresp.'' ou ''Receb.Corresp.''.";
                }
              }
            }

            if (dr["DESCRICAO"] != null && !string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              if (dr["DESCRICAO"].ToString().Length > 255)
              {
                return "A descrição deve possuir no máximo 255 caracteres.";
              }
            }

            if (dr["PRAZO"] != null && !string.IsNullOrEmpty(dr["PRAZO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["PRAZO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PRAZO"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["PRAZO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PRAZO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O prazo deve ser um valor inteiro com 3 dígitos no máximo.";
              }
            }

            if (dr["TIPO_MENSAGEM"] != null && !string.IsNullOrEmpty(dr["TIPO_MENSAGEM"].ToString()))
            {
              if (dr["TIPO_MENSAGEM"].ToString().Length > 15)
              {
                return "O tipo de mensagem deve possuir no máximo 15 caracteres.";
              }
            }

            if (dr["MENSAGEM"] != null && !string.IsNullOrEmpty(dr["MENSAGEM"].ToString()))
            {
              if (dr["MENSAGEM"].ToString().Length > 255)
              {
                return "A mensagem deve possuir no máximo 255 caracteres.";
              }
            }

            if (dr["NUM_DIAS"] != null && !string.IsNullOrEmpty(dr["NUM_DIAS"].ToString()))
            {
              if ((Convert.ToDecimal(dr["NUM_DIAS"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["NUM_DIAS"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["NUM_DIAS"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["NUM_DIAS"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O número de dias deve ser um valor inteiro com 3 dígitos no máximo.";
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

    public static Ly_fluxo_de_andamento Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_fluxo_de_andamento Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_fluxo_de_andamento tab = new Ly_fluxo_de_andamento();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_fluxo_de_andamento tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @servico, decimal? @passo, decimal? @prox_passo, string @setor, string @tipo_andamento, 
        string @descricao, decimal? @prazo, string @tipo_mensagem, string @mensagem, decimal? @num_dias
      ) {
        return (Row)Add(new object[] {
          @servico, @passo, @prox_passo, @setor, @tipo_andamento, @descricao, @prazo, @tipo_mensagem, 
          @mensagem, @num_dias
        });
      }

      public Row Find(
        string @servico, decimal? @passo
      ) {
        return (Row)Find(new object[] {
          @servico, @passo
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
      /// SERVICO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Servico { get { return (string)this["servico"]; } set { this["servico"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PASSO: NUMERIC(3), Primary Key, Not Null
      /// </summary>
      public decimal? Passo { get { return (decimal?)this["passo"]; } set { this["passo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PROX_PASSO: NUMERIC(3)
      /// </summary>
      public decimal? Prox_passo { get { return (decimal?)this["prox_passo"]; } set { this["prox_passo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SETOR: VARCHAR(15), Not Null
      /// </summary>
      public string Setor { get { return (string)this["setor"]; } set { this["setor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TIPO_ANDAMENTO: VARCHAR(15)
      /// </summary>
      public string Tipo_andamento { get { return (string)this["tipo_andamento"]; } set { this["tipo_andamento"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO: VARCHAR(255)
      /// </summary>
      public string Descricao { get { return (string)this["descricao"]; } set { this["descricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PRAZO: NUMERIC(3)
      /// </summary>
      public decimal? Prazo { get { return (decimal?)this["prazo"]; } set { this["prazo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TIPO_MENSAGEM: VARCHAR(15)
      /// </summary>
      public string Tipo_mensagem { get { return (string)this["tipo_mensagem"]; } set { this["tipo_mensagem"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// MENSAGEM: VARCHAR(255)
      /// </summary>
      public string Mensagem { get { return (string)this["mensagem"]; } set { this["mensagem"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NUM_DIAS: NUMERIC(3)
      /// </summary>
      public decimal? Num_dias { get { return (decimal?)this["num_dias"]; } set { this["num_dias"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @servico, decimal? @passo, string @setor) {
        return Insert(connection,
                      @servico, @passo, @setor,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @servico, decimal? @passo, string @setor,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_fluxo_de_andamento(),
          new string[] { "servico", "passo", "setor" },
          new object[] { @servico, @passo, @setor },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkServico, decimal? pkPasso,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_fluxo_de_andamento(),
          new string[] { "servico", "passo" },
          new object[] { pkServico, pkPasso },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkServico, decimal? pkPasso) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_fluxo_de_andamento(),
          new string[] { "servico", "passo" },
          new object[] { pkServico, pkPasso }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkServico, decimal? pkPasso) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_fluxo_de_andamento(),
          new string[] { "servico", "passo" },
          new object[] { pkServico, pkPasso }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Servico { get { return (string)row["servico", DataRowVersion.Original]; } }
        public decimal? Passo { get { return (decimal?)row["passo", DataRowVersion.Original]; } }
        public decimal? Prox_passo { get { return (decimal?)row["prox_passo", DataRowVersion.Original]; } }
        public string Setor { get { return (string)row["setor", DataRowVersion.Original]; } }
        public string Tipo_andamento { get { return (string)row["tipo_andamento", DataRowVersion.Original]; } }
        public string Descricao { get { return (string)row["descricao", DataRowVersion.Original]; } }
        public decimal? Prazo { get { return (decimal?)row["prazo", DataRowVersion.Original]; } }
        public string Tipo_mensagem { get { return (string)row["tipo_mensagem", DataRowVersion.Original]; } }
        public string Mensagem { get { return (string)row["mensagem", DataRowVersion.Original]; } }
        public decimal? Num_dias { get { return (decimal?)row["num_dias", DataRowVersion.Original]; } }

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
    #region Coluna: SERVICO
    private ColumnServico pvColServico;

    public sealed class ColumnServico : TDataColumn {

      public ColumnServico() {
        this.ColumnName = "servico";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "serviço";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_fluxo_de_andamento.servico"; } }
      public override string OriginalCol { get { return "servico"; } }
      public override string RefCol { get { return "LY_FLUXO_DE_ANDAMENTO.SERVICO"; } }
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

    #region Coluna: PASSO
    private ColumnPasso pvColPasso;

    public sealed class ColumnPasso : TDataColumn {

      public ColumnPasso() {
        this.ColumnName = "passo";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "passo";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_fluxo_de_andamento.passo"; } }
      public override string OriginalCol { get { return "passo"; } }
      public override string RefCol { get { return "LY_FLUXO_DE_ANDAMENTO.PASSO"; } }
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

    }
    #endregion

    #region Coluna: PROX_PASSO
    private ColumnProx_passo pvColProx_passo;

    public sealed class ColumnProx_passo : TDataColumn {

      public ColumnProx_passo() {
        this.ColumnName = "prox_passo";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "próximo passo";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_fluxo_de_andamento.prox_passo"; } }
      public override string OriginalCol { get { return "prox_passo"; } }
      public override string RefCol { get { return "LY_FLUXO_DE_ANDAMENTO.PROX_PASSO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 4; } }
      public override int Precision { get { return 3; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(3)"; } }

    }
    #endregion

    #region Coluna: SETOR
    private ColumnSetor pvColSetor;

    public sealed class ColumnSetor : TDataColumn {

      public ColumnSetor() {
        this.ColumnName = "setor";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "setor";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_fluxo_de_andamento.setor"; } }
      public override string OriginalCol { get { return "setor"; } }
      public override string RefCol { get { return "LY_FLUXO_DE_ANDAMENTO.SETOR"; } }
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

    #region Coluna: TIPO_ANDAMENTO
    private ColumnTipo_andamento pvColTipo_andamento;

    public sealed class ColumnTipo_andamento : TDataColumn {

      public ColumnTipo_andamento() {
        this.ColumnName = "tipo_andamento";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "tipo de andamento";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_fluxo_de_andamento.tipo_andamento"; } }
      public override string OriginalCol { get { return "tipo_andamento"; } }
      public override string RefCol { get { return "LY_FLUXO_DE_ANDAMENTO.TIPO_ANDAMENTO"; } }
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
      public override string FullCol { get { return "ly_fluxo_de_andamento.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "LY_FLUXO_DE_ANDAMENTO.DESCRICAO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 255; } }
      public override string SqlServerType { get { return "VARCHAR(255)"; } }

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
      public override string FullCol { get { return "ly_fluxo_de_andamento.prazo"; } }
      public override string OriginalCol { get { return "prazo"; } }
      public override string RefCol { get { return "LY_FLUXO_DE_ANDAMENTO.PRAZO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 4; } }
      public override int Precision { get { return 3; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(3)"; } }

    }
    #endregion

    #region Coluna: TIPO_MENSAGEM
    private ColumnTipo_mensagem pvColTipo_mensagem;

    public sealed class ColumnTipo_mensagem : TDataColumn {

      public ColumnTipo_mensagem() {
        this.ColumnName = "tipo_mensagem";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "tipo de mensagem";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_fluxo_de_andamento.tipo_mensagem"; } }
      public override string OriginalCol { get { return "tipo_mensagem"; } }
      public override string RefCol { get { return "LY_FLUXO_DE_ANDAMENTO.TIPO_MENSAGEM"; } }
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

    #region Coluna: MENSAGEM
    private ColumnMensagem pvColMensagem;

    public sealed class ColumnMensagem : TDataColumn {

      public ColumnMensagem() {
        this.ColumnName = "mensagem";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "mensagem";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_fluxo_de_andamento.mensagem"; } }
      public override string OriginalCol { get { return "mensagem"; } }
      public override string RefCol { get { return "LY_FLUXO_DE_ANDAMENTO.MENSAGEM"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 255; } }
      public override string SqlServerType { get { return "VARCHAR(255)"; } }

    }
    #endregion

    #region Coluna: NUM_DIAS
    private ColumnNum_dias pvColNum_dias;

    public sealed class ColumnNum_dias : TDataColumn {

      public ColumnNum_dias() {
        this.ColumnName = "num_dias";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "número de dias";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_fluxo_de_andamento.num_dias"; } }
      public override string OriginalCol { get { return "num_dias"; } }
      public override string RefCol { get { return "LY_FLUXO_DE_ANDAMENTO.NUM_DIAS"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 4; } }
      public override int Precision { get { return 3; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(3)"; } }

    }
    #endregion
    #endregion
  }

  public partial class Ly_itens_solicit_serv : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_itens_solicit_serv() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_itens_solicit_serv");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_itens_solicit_serv");
      }
      catch{}
    }

    public Ly_itens_solicit_serv() {
      pvColSolicitacao = new ColumnSolicitacao(); Columns.Add(pvColSolicitacao);
      pvColItem_solicitacao = new ColumnItem_solicitacao(); Columns.Add(pvColItem_solicitacao);
      pvColServico = new ColumnServico(); Columns.Add(pvColServico);
      pvColLanc_deb = new ColumnLanc_deb(); Columns.Add(pvColLanc_deb);
      pvColQtd = new ColumnQtd(); Columns.Add(pvColQtd);
      pvColObs = new ColumnObs(); Columns.Add(pvColObs);
      pvColProva_revisao = new ColumnProva_revisao(); Columns.Add(pvColProva_revisao);
      pvColDeferido = new ColumnDeferido(); Columns.Add(pvColDeferido);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_itens_solicit_serv");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(decimal? @solicitacao, decimal? @item_solicitacao, string @servico, 
                      decimal? @qtd) {
      Row row = NewRow();

      row.Solicitacao = @solicitacao;
      row.Item_solicitacao = @item_solicitacao;
      row.Servico = @servico;
      row.Qtd = @qtd;

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
      get { return "LY_ITENS_SOLICIT_SERV"; }
    }

    protected override sealed string Name { get { return "Item de Solicitação de Serviço"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColSolicitacao, pvColItem_solicitacao });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_itens_solicit_serv_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_itens_solicit_serv_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_itens_solicit_serv_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_itens_solicit_serv_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkSolicitacao", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "SOLICITACAO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkItem_solicitacao", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ITEM_SOLICITACAO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("solicitacao", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "SOLICITACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("item_solicitacao", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ITEM_SOLICITACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("servico", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SERVICO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("lanc_deb", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "LANC_DEB", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("qtd", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "QTD", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("obs", OleDbType.VarChar, 7100, ParameterDirection.InputOutput, true, 0, 0, "OBS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("prova_revisao", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PROVA_REVISAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("deferido", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "DEFERIDO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));

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
          if(lower == "solicitacao") { result.Add(toLower ? "solicitacao" : column); continue; }
          if(lower == "item_solicitacao") { result.Add(toLower ? "item_solicitacao" : column); continue; }
          if(lower == "servico") { result.Add(toLower ? "servico" : column); continue; }
          if(lower == "lanc_deb") { result.Add(toLower ? "lanc_deb" : column); continue; }
          if(lower == "qtd") { result.Add(toLower ? "qtd" : column); continue; }
          if(lower == "obs") { result.Add(toLower ? "obs" : column); continue; }
          if(lower == "prova_revisao") { result.Add(toLower ? "prova_revisao" : column); continue; }
          if(lower == "deferido") { result.Add(toLower ? "deferido" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "solicitacao", "item_solicitacao", "servico", "lanc_deb", "qtd", 
                                           "obs", "prova_revisao", "deferido" }
                          : new string[] { "SOLICITACAO", "ITEM_SOLICITACAO", "SERVICO", "LANC_DEB", "QTD", 
                                           "OBS", "PROVA_REVISAO", "DEFERIDO" });
        else
          result.AddRange(toLower
                          ? new string[] { "solicitacao", "item_solicitacao", "servico", "lanc_deb", "qtd", 
                                           "obs", "prova_revisao", "deferido" }
                          : new string[] { "SOLICITACAO", "ITEM_SOLICITACAO", "SERVICO", "LANC_DEB", "QTD", 
                                           "OBS", "PROVA_REVISAO", "DEFERIDO" });

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
        "ly_itens_solicit_serv.solicitacao", "ly_itens_solicit_serv.item_solicitacao"
      });
      if(scColumns.Contains("servico")) list.Add("ly_itens_solicit_serv.servico");
      if(scColumns.Contains("lanc_deb")) list.Add("ly_itens_solicit_serv.lanc_deb");
      if(scColumns.Contains("qtd")) list.Add("ly_itens_solicit_serv.qtd");
      if(scColumns.Contains("obs")) list.Add("ly_itens_solicit_serv.obs");
      if(scColumns.Contains("prova_revisao")) list.Add("ly_itens_solicit_serv.prova_revisao");
      if(scColumns.Contains("deferido")) list.Add("ly_itens_solicit_serv.deferido");
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
          "FROM ly_itens_solicit_serv " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_itens_solicit_serv");
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
        da.TableMappings.Add("Table", "Ly_itens_solicit_serv");
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
            if (dr["SOLICITACAO"] != null && string.IsNullOrEmpty(dr["SOLICITACAO"].ToString()))
              dr["SOLICITACAO"] = DBNull.Value;
            if (dr["ITEM_SOLICITACAO"] != null && string.IsNullOrEmpty(dr["ITEM_SOLICITACAO"].ToString()))
              dr["ITEM_SOLICITACAO"] = DBNull.Value;
            if (dr["SERVICO"] != null && string.IsNullOrEmpty(dr["SERVICO"].ToString()))
              dr["SERVICO"] = DBNull.Value;
            if (dr["LANC_DEB"] != null && string.IsNullOrEmpty(dr["LANC_DEB"].ToString()))
              dr["LANC_DEB"] = DBNull.Value;
            if (dr["QTD"] != null && string.IsNullOrEmpty(dr["QTD"].ToString()))
              dr["QTD"] = DBNull.Value;
            if (dr["OBS"] != null && string.IsNullOrEmpty(dr["OBS"].ToString()))
              dr["OBS"] = DBNull.Value;
            if (dr["PROVA_REVISAO"] != null && string.IsNullOrEmpty(dr["PROVA_REVISAO"].ToString()))
              dr["PROVA_REVISAO"] = DBNull.Value;
            if (dr["DEFERIDO"] != null && string.IsNullOrEmpty(dr["DEFERIDO"].ToString()))
              dr["DEFERIDO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["SOLICITACAO"] == null || string.IsNullOrEmpty(dr["SOLICITACAO"].ToString()))
            {
              return "Favor informar a Solicitação.";
            }

            if (dr["SOLICITACAO"] != null && !string.IsNullOrEmpty(dr["SOLICITACAO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["SOLICITACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["SOLICITACAO"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["SOLICITACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["SOLICITACAO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A Solicitação deve ser um valor inteiro com 10 dígitos no máximo.";
              }
            }

            if (dr["ITEM_SOLICITACAO"] == null || string.IsNullOrEmpty(dr["ITEM_SOLICITACAO"].ToString()))
            {
              return "Favor informar o Item de Solicitção.";
            }

            if (dr["ITEM_SOLICITACAO"] != null && !string.IsNullOrEmpty(dr["ITEM_SOLICITACAO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ITEM_SOLICITACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ITEM_SOLICITACAO"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["ITEM_SOLICITACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ITEM_SOLICITACAO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Item de Solicitção deve ser um valor inteiro com 10 dígitos no máximo.";
              }
            }

            if (dr["SERVICO"] == null || string.IsNullOrEmpty(dr["SERVICO"].ToString()))
            {
              return "Favor informar o Serviço.";
            }

            if (dr["SERVICO"] != null && !string.IsNullOrEmpty(dr["SERVICO"].ToString()))
            {
              if (dr["SERVICO"].ToString().Length > 20)
              {
                return "O Serviço deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["LANC_DEB"] != null && !string.IsNullOrEmpty(dr["LANC_DEB"].ToString()))
            {
              if ((Convert.ToDecimal(dr["LANC_DEB"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["LANC_DEB"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["LANC_DEB"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["LANC_DEB"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A Código da Dívida deve ser um valor inteiro com 10 dígitos no máximo.";
              }
            }

            if (dr["QTD"] == null || string.IsNullOrEmpty(dr["QTD"].ToString()))
            {
              return "Favor informar a Quanditade.";
            }

            if (dr["QTD"] != null && !string.IsNullOrEmpty(dr["QTD"].ToString()))
            {
              if ((Convert.ToDecimal(dr["QTD"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["QTD"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["QTD"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["QTD"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A Quanditade deve ser um valor inteiro com 10 dígitos no máximo.";
              }
            }

            if (dr["OBS"] != null && !string.IsNullOrEmpty(dr["OBS"].ToString()))
            {
              if (dr["OBS"].ToString().Length > 7000)
              {
                return "O Observação deve possuir no máximo 7000 caracteres.";
              }
            }

            if (dr["PROVA_REVISAO"] != null && !string.IsNullOrEmpty(dr["PROVA_REVISAO"].ToString()))
            {
              if (dr["PROVA_REVISAO"].ToString().Length > 100)
              {
                return "A Revisão de Prova deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["DEFERIDO"] != null && !string.IsNullOrEmpty(dr["DEFERIDO"].ToString()))
            {
              if (dr["DEFERIDO"].ToString().Length > 1)
              {
                return "O Deferido deve possuir no máximo 1 caractere.";
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

    public static Ly_itens_solicit_serv Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_itens_solicit_serv Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_itens_solicit_serv tab = new Ly_itens_solicit_serv();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_itens_solicit_serv tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @solicitacao, decimal? @item_solicitacao, string @servico, decimal? @lanc_deb, 
        decimal? @qtd, string @obs, string @prova_revisao, string @deferido
      ) {
        return (Row)Add(new object[] {
          @solicitacao, @item_solicitacao, @servico, @lanc_deb, @qtd, @obs, @prova_revisao, 
          @deferido
        });
      }

      public Row Find(
        decimal? @solicitacao, decimal? @item_solicitacao
      ) {
        return (Row)Find(new object[] {
          @solicitacao, @item_solicitacao
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
      /// SOLICITACAO: NUMERIC(10), Primary Key, Not Null
      /// </summary>
      public decimal? Solicitacao { get { return (decimal?)this["solicitacao"]; } set { this["solicitacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ITEM_SOLICITACAO: NUMERIC(10), Primary Key, Not Null
      /// </summary>
      public decimal? Item_solicitacao { get { return (decimal?)this["item_solicitacao"]; } set { this["item_solicitacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SERVICO: VARCHAR(20), Not Null
      /// </summary>
      public string Servico { get { return (string)this["servico"]; } set { this["servico"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// LANC_DEB: NUMERIC(10)
      /// </summary>
      public decimal? Lanc_deb { get { return (decimal?)this["lanc_deb"]; } set { this["lanc_deb"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// QTD: NUMERIC(10), Not Null
      /// </summary>
      public decimal? Qtd { get { return (decimal?)this["qtd"]; } set { this["qtd"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// OBS: VARCHAR(7000)
      /// </summary>
      public string Obs { get { return (string)this["obs"]; } set { this["obs"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PROVA_REVISAO: VARCHAR(100)
      /// </summary>
      public string Prova_revisao { get { return (string)this["prova_revisao"]; } set { this["prova_revisao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DEFERIDO: CHAR(1)
      /// </summary>
      public string Deferido { get { return (string)this["deferido"]; } set { this["deferido"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               decimal? @solicitacao, decimal? @item_solicitacao, string @servico, decimal? @qtd) {
        return Insert(connection,
                      @solicitacao, @item_solicitacao, @servico, @qtd,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               decimal? @solicitacao, decimal? @item_solicitacao, string @servico, decimal? @qtd,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_itens_solicit_serv(),
          new string[] { "solicitacao", "item_solicitacao", "servico", "qtd" },
          new object[] { @solicitacao, @item_solicitacao, @servico, @qtd },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkSolicitacao, decimal? pkItem_solicitacao,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_itens_solicit_serv(),
          new string[] { "solicitacao", "item_solicitacao" },
          new object[] { pkSolicitacao, pkItem_solicitacao },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkSolicitacao, decimal? pkItem_solicitacao) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_itens_solicit_serv(),
          new string[] { "solicitacao", "item_solicitacao" },
          new object[] { pkSolicitacao, pkItem_solicitacao }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              decimal? pkSolicitacao, decimal? pkItem_solicitacao) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_itens_solicit_serv(),
          new string[] { "solicitacao", "item_solicitacao" },
          new object[] { pkSolicitacao, pkItem_solicitacao }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public decimal? Solicitacao { get { return (decimal?)row["solicitacao", DataRowVersion.Original]; } }
        public decimal? Item_solicitacao { get { return (decimal?)row["item_solicitacao", DataRowVersion.Original]; } }
        public string Servico { get { return (string)row["servico", DataRowVersion.Original]; } }
        public decimal? Lanc_deb { get { return (decimal?)row["lanc_deb", DataRowVersion.Original]; } }
        public decimal? Qtd { get { return (decimal?)row["qtd", DataRowVersion.Original]; } }
        public string Obs { get { return (string)row["obs", DataRowVersion.Original]; } }
        public string Prova_revisao { get { return (string)row["prova_revisao", DataRowVersion.Original]; } }
        public string Deferido { get { return (string)row["deferido", DataRowVersion.Original]; } }

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
    #region Coluna: SOLICITACAO
    private ColumnSolicitacao pvColSolicitacao;

    public sealed class ColumnSolicitacao : TDataColumn {

      public ColumnSolicitacao() {
        this.ColumnName = "solicitacao";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "Solicitação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_itens_solicit_serv.solicitacao"; } }
      public override string OriginalCol { get { return "solicitacao"; } }
      public override string RefCol { get { return "LY_ITENS_SOLICIT_SERV.SOLICITACAO"; } }
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

    #region Coluna: ITEM_SOLICITACAO
    private ColumnItem_solicitacao pvColItem_solicitacao;

    public sealed class ColumnItem_solicitacao : TDataColumn {

      public ColumnItem_solicitacao() {
        this.ColumnName = "item_solicitacao";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "Item de Solicitção";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_itens_solicit_serv.item_solicitacao"; } }
      public override string OriginalCol { get { return "item_solicitacao"; } }
      public override string RefCol { get { return "LY_ITENS_SOLICIT_SERV.ITEM_SOLICITACAO"; } }
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

    #region Coluna: SERVICO
    private ColumnServico pvColServico;

    public sealed class ColumnServico : TDataColumn {

      public ColumnServico() {
        this.ColumnName = "servico";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Serviço";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_itens_solicit_serv.servico"; } }
      public override string OriginalCol { get { return "servico"; } }
      public override string RefCol { get { return "LY_ITENS_SOLICIT_SERV.SERVICO"; } }
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

    #region Coluna: LANC_DEB
    private ColumnLanc_deb pvColLanc_deb;

    public sealed class ColumnLanc_deb : TDataColumn {

      public ColumnLanc_deb() {
        this.ColumnName = "lanc_deb";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "Código da Dívida";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_itens_solicit_serv.lanc_deb"; } }
      public override string OriginalCol { get { return "lanc_deb"; } }
      public override string RefCol { get { return "LY_ITENS_SOLICIT_SERV.LANC_DEB"; } }
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

    #region Coluna: QTD
    private ColumnQtd pvColQtd;

    public sealed class ColumnQtd : TDataColumn {

      public ColumnQtd() {
        this.ColumnName = "qtd";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "Quanditade";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_itens_solicit_serv.qtd"; } }
      public override string OriginalCol { get { return "qtd"; } }
      public override string RefCol { get { return "LY_ITENS_SOLICIT_SERV.QTD"; } }
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

    #region Coluna: OBS
    private ColumnObs pvColObs;

    public sealed class ColumnObs : TDataColumn {

      public ColumnObs() {
        this.ColumnName = "obs";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Observação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_itens_solicit_serv.obs"; } }
      public override string OriginalCol { get { return "obs"; } }
      public override string RefCol { get { return "LY_ITENS_SOLICIT_SERV.OBS"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 7000; } }
      public override string SqlServerType { get { return "VARCHAR(7000)"; } }

    }
    #endregion

    #region Coluna: PROVA_REVISAO
    private ColumnProva_revisao pvColProva_revisao;

    public sealed class ColumnProva_revisao : TDataColumn {

      public ColumnProva_revisao() {
        this.ColumnName = "prova_revisao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Revisão de Prova";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_itens_solicit_serv.prova_revisao"; } }
      public override string OriginalCol { get { return "prova_revisao"; } }
      public override string RefCol { get { return "LY_ITENS_SOLICIT_SERV.PROVA_REVISAO"; } }
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

    #region Coluna: DEFERIDO
    private ColumnDeferido pvColDeferido;

    public sealed class ColumnDeferido : TDataColumn {

      public ColumnDeferido() {
        this.ColumnName = "deferido";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_itens_solicit_serv.deferido"; } }
      public override string OriginalCol { get { return "deferido"; } }
      public override string RefCol { get { return "LY_ITENS_SOLICIT_SERV.DEFERIDO"; } }
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

  public partial class Ly_solicitacao_serv : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_solicitacao_serv() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_solicitacao_serv");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_solicitacao_serv");
      }
      catch{}
    }

    public Ly_solicitacao_serv() {
      pvColSolicitacao = new ColumnSolicitacao(); Columns.Add(pvColSolicitacao);
      pvColAluno = new ColumnAluno(); Columns.Add(pvColAluno);
      pvColData = new ColumnData(); Columns.Add(pvColData);
      pvColObs = new ColumnObs(); Columns.Add(pvColObs);
      pvColAtendimento = new ColumnAtendimento(); Columns.Add(pvColAtendimento);
      pvColNum_func = new ColumnNum_func(); Columns.Add(pvColNum_func);
      pvColUsuario = new ColumnUsuario(); Columns.Add(pvColUsuario);
      pvColOperadoraBilhetagemId = new ColumnOperadoraBilhetagemId(); Columns.Add(pvColOperadoraBilhetagemId);
      
      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_solicitacao_serv");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(DateTime? @data) {
      Row row = NewRow();

      row.Data = @data;

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
      get { return "LY_SOLICITACAO_SERV"; }
    }

    protected override sealed string Name { get { return "Solicitação de Serviço"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColSolicitacao });
      pvColSolicitacao.AllowDBNull = true;
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_solicitacao_serv_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_solicitacao_serv_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_solicitacao_serv_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_solicitacao_serv_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkSolicitacao", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "SOLICITACAO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("solicitacao", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "SOLICITACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("aluno", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "ALUNO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("data", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DATA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("obs", OleDbType.VarChar, 7100, ParameterDirection.InputOutput, true, 0, 0, "OBS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("atendimento", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ATENDIMENTO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("num_func", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 25, 10, "NUM_FUNC", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("usuario", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "USUARIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("operadorabilhetagemid", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 3, 0, "OPERADORABILHETAGEMID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "solicitacao") { result.Add(toLower ? "solicitacao" : column); continue; }
          if(lower == "aluno") { result.Add(toLower ? "aluno" : column); continue; }
          if(lower == "data") { result.Add(toLower ? "data" : column); continue; }
          if(lower == "obs") { result.Add(toLower ? "obs" : column); continue; }
          if(lower == "atendimento") { result.Add(toLower ? "atendimento" : column); continue; }
          if(lower == "num_func") { result.Add(toLower ? "num_func" : column); continue; }
          if(lower == "usuario") { result.Add(toLower ? "usuario" : column); continue; }
          if(lower == "operadorabilhetagemid") { result.Add(toLower ? "operadorabilhetagemid" : column); continue; }
          if (!tabPrincipal)
          {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "solicitacao", "aluno", "data", "obs", "atendimento", "num_func", 
                                           "usuario", "operadorabilhetagemid"}
                          : new string[] { "SOLICITACAO", "ALUNO", "DATA", "OBS", "ATENDIMENTO", "NUM_FUNC", 
                                           "USUARIO", "OPERADORABILHETAGEMID" });
        else
          result.AddRange(toLower
                          ? new string[] { "solicitacao", "aluno", "data", "obs", "atendimento", "num_func", 
                                           "usuario", "operadorabilhetagemid"}
                          : new string[] { "SOLICITACAO", "ALUNO", "DATA", "OBS", "ATENDIMENTO", "NUM_FUNC", 
                                           "USUARIO", "OPERADORABILHETAGEMID" });

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
        "ly_solicitacao_serv.solicitacao"
      });
      if(scColumns.Contains("aluno")) list.Add("ly_solicitacao_serv.aluno");
      if(scColumns.Contains("data")) list.Add("ly_solicitacao_serv.data");
      if(scColumns.Contains("obs")) list.Add("ly_solicitacao_serv.obs");
      if(scColumns.Contains("atendimento")) list.Add("ly_solicitacao_serv.atendimento");
      if(scColumns.Contains("num_func")) list.Add("ly_solicitacao_serv.num_func");
      if(scColumns.Contains("usuario")) list.Add("ly_solicitacao_serv.usuario");
      if(scColumns.Contains("operadorabilhetagemid")) list.Add("ly_solicitacao_serv.operadorabilhetagemid");
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
          "FROM ly_solicitacao_serv " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_solicitacao_serv");
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
        da.TableMappings.Add("Table", "Ly_solicitacao_serv");
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
            if (dr["ALUNO"] != null && string.IsNullOrEmpty(dr["ALUNO"].ToString()))
              dr["ALUNO"] = DBNull.Value;
            if (dr["OBS"] != null && string.IsNullOrEmpty(dr["OBS"].ToString()))
              dr["OBS"] = DBNull.Value;
            if (dr["ATENDIMENTO"] != null && string.IsNullOrEmpty(dr["ATENDIMENTO"].ToString()))
              dr["ATENDIMENTO"] = DBNull.Value;
            if (dr["NUM_FUNC"] != null && string.IsNullOrEmpty(dr["NUM_FUNC"].ToString()))
              dr["NUM_FUNC"] = DBNull.Value;
            if (dr["USUARIO"] != null && string.IsNullOrEmpty(dr["USUARIO"].ToString()))
                dr["USUARIO"] = DBNull.Value;
            if (dr["OPERADORABILHETAGEMID"] != null && string.IsNullOrEmpty(dr["OPERADORABILHETAGEMID"].ToString()))
                dr["OPERADORABILHETAGEMID"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["ALUNO"] != null && !string.IsNullOrEmpty(dr["ALUNO"].ToString()))
            {
              if (dr["ALUNO"].ToString().Length > 20)
              {
                return "O Aluno deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["DATA"] == null || string.IsNullOrEmpty(dr["DATA"].ToString()))
            {
              return "Favor informar a Data.";
            }

            if (dr["DATA"] != null && dr["DATA"].ToString() != "")
            {
              dr["DATA"] = Convert.ToDateTime(dr["DATA"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["OBS"] != null && !string.IsNullOrEmpty(dr["OBS"].ToString()))
            {
              if (dr["OBS"].ToString().Length > 7000)
              {
                return "A Observação deve possuir no máximo 7000 caracteres.";
              }
            }

            if (dr["ATENDIMENTO"] != null && !string.IsNullOrEmpty(dr["ATENDIMENTO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ATENDIMENTO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ATENDIMENTO"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["ATENDIMENTO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ATENDIMENTO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Atendimento deve ser um valor inteiro com 10 dígitos no máximo.";
              }
            }

            if (dr["NUM_FUNC"] != null && !string.IsNullOrEmpty(dr["NUM_FUNC"].ToString()))
            {
              if ((Convert.ToDecimal(dr["NUM_FUNC"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["NUM_FUNC"]).ToString("0.############").Split(',')[0].Length > 15) || (Convert.ToDecimal(dr["NUM_FUNC"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["NUM_FUNC"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Num_func deve ser um valor inteiro com 15 dígitos no máximo.";
              }
            }

            if (dr["USUARIO"] != null && !string.IsNullOrEmpty(dr["USUARIO"].ToString()))
            {
              if (dr["USUARIO"].ToString().Length > 20)
              {
                return "O Usuario deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["OPERADORABILHETAGEMID"] != null && !string.IsNullOrEmpty(dr["OPERADORABILHETAGEMID"].ToString()))
            {
                if ((Convert.ToDecimal(dr["OPERADORABILHETAGEMID"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["OPERADORABILHETAGEMID"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["OPERADORABILHETAGEMID"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["OPERADORABILHETAGEMID"]).ToString("0.############").Split(',')[1].Length > 0))
                {
                    return "A Operadora Bilhetagem deve ser um valor inteiro com 3 dígitos no máximo.";
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

    public static Ly_solicitacao_serv Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_solicitacao_serv Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_solicitacao_serv tab = new Ly_solicitacao_serv();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_solicitacao_serv tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @solicitacao, string @aluno, DateTime? @data, string @obs, decimal? @atendimento,
        decimal? @num_func, string @usuario, decimal? @operadorabilhetagemid
      ) {
        return (Row)Add(new object[] {
          @solicitacao, @aluno, @data, @obs, @atendimento, @num_func, @usuario, @operadorabilhetagemid
        });
      }

      public Row Find(
        decimal? @solicitacao
      ) {
        return (Row)Find(new object[] {
          @solicitacao
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
      /// SOLICITACAO: NUMERIC(10), Primary Key, AutoNumber, Not Null
      /// </summary>
      public decimal? Solicitacao { get { return (decimal?)this["solicitacao"]; } set { this["solicitacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ALUNO: VARCHAR(20)
      /// </summary>
      public string Aluno { get { return (string)this["aluno"]; } set { this["aluno"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DATA: DATETIME, Not Null
      /// </summary>
      public DateTime? Data { get { return (DateTime?)this["data"]; } set { this["data"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// OBS: VARCHAR(7000)
      /// </summary>
      public string Obs { get { return (string)this["obs"]; } set { this["obs"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ATENDIMENTO: NUMERIC(10)
      /// </summary>
      public decimal? Atendimento { get { return (decimal?)this["atendimento"]; } set { this["atendimento"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NUM_FUNC: NUMERIC(15)
      /// </summary>
      public decimal? Num_func { get { return (decimal?)this["num_func"]; } set { this["num_func"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// USUARIO: VARCHAR(20)
      /// </summary>
      public string Usuario { get { return (string)this["usuario"]; } set { this["usuario"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// OPERADORABILHETAGEMID: NUMERIC(3)
      /// </summary>
      public decimal? OperadoraBilhetagemId { get { return (decimal?)this["operadorabilhetagemid"]; } set { this["operadorabilhetagemid"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      

      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               DateTime? @data) {
        return Insert(connection,
                      @data,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               DateTime? @data,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_solicitacao_serv(),
          new string[] { "data" },
          new object[] { @data },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkSolicitacao,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_solicitacao_serv(),
          new string[] { "solicitacao" },
          new object[] { pkSolicitacao },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkSolicitacao) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_solicitacao_serv(),
          new string[] { "solicitacao" },
          new object[] { pkSolicitacao }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              decimal? pkSolicitacao) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_solicitacao_serv(),
          new string[] { "solicitacao" },
          new object[] { pkSolicitacao }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public decimal? Solicitacao { get { return (decimal?)row["solicitacao", DataRowVersion.Original]; } }
        public string Aluno { get { return (string)row["aluno", DataRowVersion.Original]; } }
        public DateTime? Data { get { return (DateTime?)row["data", DataRowVersion.Original]; } }
        public string Obs { get { return (string)row["obs", DataRowVersion.Original]; } }
        public decimal? Atendimento { get { return (decimal?)row["atendimento", DataRowVersion.Original]; } }
        public decimal? Num_func { get { return (decimal?)row["num_func", DataRowVersion.Original]; } }
        public string Usuario { get { return (string)row["usuario", DataRowVersion.Original]; } }
        public decimal? OperadoraBilhetagemId { get { return (decimal?)row["operadorabilhetagemid", DataRowVersion.Original]; } }

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
    #region Coluna: SOLICITACAO
    private ColumnSolicitacao pvColSolicitacao;

    public sealed class ColumnSolicitacao : TDataColumn {

      public ColumnSolicitacao() {
        this.ColumnName = "solicitacao";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "Solicitação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_solicitacao_serv.solicitacao"; } }
      public override string OriginalCol { get { return "solicitacao"; } }
      public override string RefCol { get { return "LY_SOLICITACAO_SERV.SOLICITACAO"; } }
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

    #region Coluna: ALUNO
    private ColumnAluno pvColAluno;

    public sealed class ColumnAluno : TDataColumn {

      public ColumnAluno() {
        this.ColumnName = "aluno";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Aluno";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_solicitacao_serv.aluno"; } }
      public override string OriginalCol { get { return "aluno"; } }
      public override string RefCol { get { return "LY_SOLICITACAO_SERV.ALUNO"; } }
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

    #region Coluna: DATA
    private ColumnData pvColData;

    public sealed class ColumnData : TDataColumn {

      public ColumnData() {
        this.ColumnName = "data";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "Data";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_solicitacao_serv.data"; } }
      public override string OriginalCol { get { return "data"; } }
      public override string RefCol { get { return "LY_SOLICITACAO_SERV.DATA"; } }
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

    #region Coluna: OBS
    private ColumnObs pvColObs;

    public sealed class ColumnObs : TDataColumn {

      public ColumnObs() {
        this.ColumnName = "obs";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Observação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_solicitacao_serv.obs"; } }
      public override string OriginalCol { get { return "obs"; } }
      public override string RefCol { get { return "LY_SOLICITACAO_SERV.OBS"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 7000; } }
      public override string SqlServerType { get { return "VARCHAR(7000)"; } }

    }
    #endregion

    #region Coluna: ATENDIMENTO
    private ColumnAtendimento pvColAtendimento;

    public sealed class ColumnAtendimento : TDataColumn {

      public ColumnAtendimento() {
        this.ColumnName = "atendimento";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "Atendimento";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_solicitacao_serv.atendimento"; } }
      public override string OriginalCol { get { return "atendimento"; } }
      public override string RefCol { get { return "LY_SOLICITACAO_SERV.ATENDIMENTO"; } }
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

    #region Coluna: NUM_FUNC
    private ColumnNum_func pvColNum_func;

    public sealed class ColumnNum_func : TDataColumn {

      public ColumnNum_func() {
        this.ColumnName = "num_func";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_solicitacao_serv.num_func"; } }
      public override string OriginalCol { get { return "num_func"; } }
      public override string RefCol { get { return "LY_SOLICITACAO_SERV.NUM_FUNC"; } }
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

    #region Coluna: USUARIO
    private ColumnUsuario pvColUsuario;

    public sealed class ColumnUsuario : TDataColumn {

      public ColumnUsuario() {
        this.ColumnName = "usuario";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_solicitacao_serv.usuario"; } }
      public override string OriginalCol { get { return "usuario"; } }
      public override string RefCol { get { return "LY_SOLICITACAO_SERV.USUARIO"; } }
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

    #region Coluna: OPERADORABILHETAGEMID
    private ColumnOperadoraBilhetagemId pvColOperadoraBilhetagemId;

    public sealed class ColumnOperadoraBilhetagemId : TDataColumn
    {

        public ColumnOperadoraBilhetagemId()
        {
            this.ColumnName = "operadorabilhetagemid";
            this.DataType = typeof(decimal);

        }


        protected override string GetName() { return ""; }
        protected override string GetCaption() { return ""; }
        protected override string GetDescricao() { return ""; }
        public override string FullCol { get { return "ly_solicitacao_serv.operadorabilhetagemid"; } }
        public override string OriginalCol { get { return "operadorabilhetagemid"; } }
        public override string RefCol { get { return "LY_SOLICITACAO_SERV.OPERADORABILHETAGEMID"; } }
        public override bool PrimaryKey { get { return false; } }
        public override bool IsAux { get { return false; } }
        /// <summary>
        /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
        /// </summary>
        public override bool NotNull { get { return false; } }
        public override int Size { get { return 5; } }
        public override int Precision { get { return 3; } }
        public override int Scale { get { return 0; } }
        public override string SqlServerType { get { return "NUMERIC(3)"; } }

    }
    #endregion

    #endregion
  }

  public partial class Ly_tabela_servicos : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_tabela_servicos() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_tabela_servicos");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_tabela_servicos");
      }
      catch{}
    }

    public Ly_tabela_servicos() {
      pvColServico = new ColumnServico(); Columns.Add(pvColServico);
      pvColCodigo_lanc = new ColumnCodigo_lanc(); Columns.Add(pvColCodigo_lanc);
      pvColDescricao = new ColumnDescricao(); Columns.Add(pvColDescricao);
      pvColCusto_unitario = new ColumnCusto_unitario(); Columns.Add(pvColCusto_unitario);
      pvColSolicitavel = new ColumnSolicitavel(); Columns.Add(pvColSolicitavel);
      pvColSistema = new ColumnSistema(); Columns.Add(pvColSistema);
      pvColGrupo_relatorio = new ColumnGrupo_relatorio(); Columns.Add(pvColGrupo_relatorio);
      pvColRelatorio = new ColumnRelatorio(); Columns.Add(pvColRelatorio);
      pvColRevisao_prova = new ColumnRevisao_prova(); Columns.Add(pvColRevisao_prova);
      pvColPrazo_entrega = new ColumnPrazo_entrega(); Columns.Add(pvColPrazo_entrega);
      pvColPagamento_pre_execucao = new ColumnPagamento_pre_execucao(); Columns.Add(pvColPagamento_pre_execucao);
      pvColImprime_via_web = new ColumnImprime_via_web(); Columns.Add(pvColImprime_via_web);
      pvColNum_solicit_nao_pagas = new ColumnNum_solicit_nao_pagas(); Columns.Add(pvColNum_solicit_nao_pagas);
      pvColSolicitavel_web = new ColumnSolicitavel_web(); Columns.Add(pvColSolicitavel_web);
      pvColTipo_servico = new ColumnTipo_servico(); Columns.Add(pvColTipo_servico);
      pvColMax_qtde_solic = new ColumnMax_qtde_solic(); Columns.Add(pvColMax_qtde_solic);
      pvColExige_autorizacao = new ColumnExige_autorizacao(); Columns.Add(pvColExige_autorizacao);
      pvColRefaz_contrato = new ColumnRefaz_contrato(); Columns.Add(pvColRefaz_contrato);
      pvColAluno = new ColumnAluno(); Columns.Add(pvColAluno);
      pvColDocente = new ColumnDocente(); Columns.Add(pvColDocente);
      pvColObs_visivel = new ColumnObs_visivel(); Columns.Add(pvColObs_visivel);
      pvColVisualizacao_setor = new ColumnVisualizacao_setor(); Columns.Add(pvColVisualizacao_setor);
      pvColPrazo_cancelamento = new ColumnPrazo_cancelamento(); Columns.Add(pvColPrazo_cancelamento);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_tabela_servicos");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @servico, string @descricao, string @solicitavel, string @revisao_prova, 
                      decimal? @prazo_entrega, string @solicitavel_web) {
      Row row = NewRow();

      row.Servico = @servico;
      row.Descricao = @descricao;
      row.Solicitavel = @solicitavel;
      row.Revisao_prova = @revisao_prova;
      row.Prazo_entrega = @prazo_entrega;
      row.Solicitavel_web = @solicitavel_web;

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
      get { return "LY_TABELA_SERVICOS"; }
    }

    protected override sealed string Name { get { return "Tabela de Serviço"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColServico });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_tabela_servicos_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_tabela_servicos_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_tabela_servicos_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_tabela_servicos_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkServico", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "SERVICO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("servico", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SERVICO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("codigo_lanc", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CODIGO_LANC", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("descricao", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "DESCRICAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("custo_unitario", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 24, 11, "CUSTO_UNITARIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("solicitavel", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SOLICITAVEL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("sistema", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SISTEMA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("grupo_relatorio", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "GRUPO_RELATORIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("relatorio", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "RELATORIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("revisao_prova", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "REVISAO_PROVA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("prazo_entrega", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "PRAZO_ENTREGA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("pagamento_pre_execucao", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PAGAMENTO_PRE_EXECUCAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("imprime_via_web", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "IMPRIME_VIA_WEB", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("num_solicit_nao_pagas", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "NUM_SOLICIT_NAO_PAGAS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("solicitavel_web", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SOLICITAVEL_WEB", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("tipo_servico", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TIPO_SERVICO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("max_qtde_solic", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "MAX_QTDE_SOLIC", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("exige_autorizacao", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "EXIGE_AUTORIZACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("refaz_contrato", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "REFAZ_CONTRATO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("aluno", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "ALUNO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("docente", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "DOCENTE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("obs_visivel", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "OBS_VISIVEL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("visualizacao_setor", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "VISUALIZACAO_SETOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
      cm.Parameters.Add(new TParameter("prazo_cancelamento", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "PRAZO_CANCELAMENTO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "servico") { result.Add(toLower ? "servico" : column); continue; }
          if(lower == "codigo_lanc") { result.Add(toLower ? "codigo_lanc" : column); continue; }
          if(lower == "descricao") { result.Add(toLower ? "descricao" : column); continue; }
          if(lower == "custo_unitario") { result.Add(toLower ? "custo_unitario" : column); continue; }
          if(lower == "solicitavel") { result.Add(toLower ? "solicitavel" : column); continue; }
          if(lower == "sistema") { result.Add(toLower ? "sistema" : column); continue; }
          if(lower == "grupo_relatorio") { result.Add(toLower ? "grupo_relatorio" : column); continue; }
          if(lower == "relatorio") { result.Add(toLower ? "relatorio" : column); continue; }
          if(lower == "revisao_prova") { result.Add(toLower ? "revisao_prova" : column); continue; }
          if(lower == "prazo_entrega") { result.Add(toLower ? "prazo_entrega" : column); continue; }
          if(lower == "pagamento_pre_execucao") { result.Add(toLower ? "pagamento_pre_execucao" : column); continue; }
          if(lower == "imprime_via_web") { result.Add(toLower ? "imprime_via_web" : column); continue; }
          if(lower == "num_solicit_nao_pagas") { result.Add(toLower ? "num_solicit_nao_pagas" : column); continue; }
          if(lower == "solicitavel_web") { result.Add(toLower ? "solicitavel_web" : column); continue; }
          if(lower == "tipo_servico") { result.Add(toLower ? "tipo_servico" : column); continue; }
          if(lower == "max_qtde_solic") { result.Add(toLower ? "max_qtde_solic" : column); continue; }
          if(lower == "exige_autorizacao") { result.Add(toLower ? "exige_autorizacao" : column); continue; }
          if(lower == "refaz_contrato") { result.Add(toLower ? "refaz_contrato" : column); continue; }
          if(lower == "aluno") { result.Add(toLower ? "aluno" : column); continue; }
          if(lower == "docente") { result.Add(toLower ? "docente" : column); continue; }
          if(lower == "obs_visivel") { result.Add(toLower ? "obs_visivel" : column); continue; }
          if(lower == "visualizacao_setor") { result.Add(toLower ? "visualizacao_setor" : column); continue; }
          if(lower == "prazo_cancelamento") { result.Add(toLower ? "prazo_cancelamento" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "servico", "codigo_lanc", "descricao", "custo_unitario", "solicitavel", 
                                           "sistema", "grupo_relatorio", "relatorio", "revisao_prova", "prazo_entrega", "pagamento_pre_execucao", 
                                           "imprime_via_web", "num_solicit_nao_pagas", "solicitavel_web", "tipo_servico", "max_qtde_solic", 
                                           "exige_autorizacao", "refaz_contrato", "aluno", "docente", "obs_visivel", "visualizacao_setor", 
                                           "prazo_cancelamento" }
                          : new string[] { "SERVICO", "CODIGO_LANC", "DESCRICAO", "CUSTO_UNITARIO", "SOLICITAVEL", 
                                           "SISTEMA", "GRUPO_RELATORIO", "RELATORIO", "REVISAO_PROVA", "PRAZO_ENTREGA", "PAGAMENTO_PRE_EXECUCAO", 
                                           "IMPRIME_VIA_WEB", "NUM_SOLICIT_NAO_PAGAS", "SOLICITAVEL_WEB", "TIPO_SERVICO", "MAX_QTDE_SOLIC", 
                                           "EXIGE_AUTORIZACAO", "REFAZ_CONTRATO", "ALUNO", "DOCENTE", "OBS_VISIVEL", "VISUALIZACAO_SETOR", 
                                           "PRAZO_CANCELAMENTO" });
        else
          result.AddRange(toLower
                          ? new string[] { "servico", "codigo_lanc", "descricao", "custo_unitario", "solicitavel", 
                                           "sistema", "grupo_relatorio", "relatorio", "revisao_prova", "prazo_entrega", "pagamento_pre_execucao", 
                                           "imprime_via_web", "num_solicit_nao_pagas", "solicitavel_web", "tipo_servico", "max_qtde_solic", 
                                           "exige_autorizacao", "refaz_contrato", "aluno", "docente", "obs_visivel", "visualizacao_setor", 
                                           "prazo_cancelamento" }
                          : new string[] { "SERVICO", "CODIGO_LANC", "DESCRICAO", "CUSTO_UNITARIO", "SOLICITAVEL", 
                                           "SISTEMA", "GRUPO_RELATORIO", "RELATORIO", "REVISAO_PROVA", "PRAZO_ENTREGA", "PAGAMENTO_PRE_EXECUCAO", 
                                           "IMPRIME_VIA_WEB", "NUM_SOLICIT_NAO_PAGAS", "SOLICITAVEL_WEB", "TIPO_SERVICO", "MAX_QTDE_SOLIC", 
                                           "EXIGE_AUTORIZACAO", "REFAZ_CONTRATO", "ALUNO", "DOCENTE", "OBS_VISIVEL", "VISUALIZACAO_SETOR", 
                                           "PRAZO_CANCELAMENTO" });

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
        "ly_tabela_servicos.servico"
      });
      if(scColumns.Contains("codigo_lanc")) list.Add("ly_tabela_servicos.codigo_lanc");
      if(scColumns.Contains("descricao")) list.Add("ly_tabela_servicos.descricao");
      if(scColumns.Contains("custo_unitario")) list.Add("ly_tabela_servicos.custo_unitario");
      if(scColumns.Contains("solicitavel")) list.Add("ly_tabela_servicos.solicitavel");
      if(scColumns.Contains("sistema")) list.Add("ly_tabela_servicos.sistema");
      if(scColumns.Contains("grupo_relatorio")) list.Add("ly_tabela_servicos.grupo_relatorio");
      if(scColumns.Contains("relatorio")) list.Add("ly_tabela_servicos.relatorio");
      if(scColumns.Contains("revisao_prova")) list.Add("ly_tabela_servicos.revisao_prova");
      if(scColumns.Contains("prazo_entrega")) list.Add("ly_tabela_servicos.prazo_entrega");
      if(scColumns.Contains("pagamento_pre_execucao")) list.Add("ly_tabela_servicos.pagamento_pre_execucao");
      if(scColumns.Contains("imprime_via_web")) list.Add("ly_tabela_servicos.imprime_via_web");
      if(scColumns.Contains("num_solicit_nao_pagas")) list.Add("ly_tabela_servicos.num_solicit_nao_pagas");
      if(scColumns.Contains("solicitavel_web")) list.Add("ly_tabela_servicos.solicitavel_web");
      if(scColumns.Contains("tipo_servico")) list.Add("ly_tabela_servicos.tipo_servico");
      if(scColumns.Contains("max_qtde_solic")) list.Add("ly_tabela_servicos.max_qtde_solic");
      if(scColumns.Contains("exige_autorizacao")) list.Add("ly_tabela_servicos.exige_autorizacao");
      if(scColumns.Contains("refaz_contrato")) list.Add("ly_tabela_servicos.refaz_contrato");
      if(scColumns.Contains("aluno")) list.Add("ly_tabela_servicos.aluno");
      if(scColumns.Contains("docente")) list.Add("ly_tabela_servicos.docente");
      if(scColumns.Contains("obs_visivel")) list.Add("ly_tabela_servicos.obs_visivel");
      if(scColumns.Contains("visualizacao_setor")) list.Add("ly_tabela_servicos.visualizacao_setor");
      if(scColumns.Contains("prazo_cancelamento")) list.Add("ly_tabela_servicos.prazo_cancelamento");
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
          "FROM ly_tabela_servicos " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_tabela_servicos");
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
        da.TableMappings.Add("Table", "Ly_tabela_servicos");
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
            if (dr["SERVICO"] != null && string.IsNullOrEmpty(dr["SERVICO"].ToString()))
              dr["SERVICO"] = DBNull.Value;
            if (dr["CODIGO_LANC"] != null && string.IsNullOrEmpty(dr["CODIGO_LANC"].ToString()))
              dr["CODIGO_LANC"] = DBNull.Value;
            if (dr["DESCRICAO"] != null && string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
              dr["DESCRICAO"] = DBNull.Value;
            if (dr["CUSTO_UNITARIO"] != null && string.IsNullOrEmpty(dr["CUSTO_UNITARIO"].ToString()))
              dr["CUSTO_UNITARIO"] = DBNull.Value;
            if (dr["SOLICITAVEL"] != null && string.IsNullOrEmpty(dr["SOLICITAVEL"].ToString()))
              dr["SOLICITAVEL"] = DBNull.Value;
            if (dr["SISTEMA"] != null && string.IsNullOrEmpty(dr["SISTEMA"].ToString()))
              dr["SISTEMA"] = DBNull.Value;
            if (dr["GRUPO_RELATORIO"] != null && string.IsNullOrEmpty(dr["GRUPO_RELATORIO"].ToString()))
              dr["GRUPO_RELATORIO"] = DBNull.Value;
            if (dr["RELATORIO"] != null && string.IsNullOrEmpty(dr["RELATORIO"].ToString()))
              dr["RELATORIO"] = DBNull.Value;
            if (dr["REVISAO_PROVA"] != null && string.IsNullOrEmpty(dr["REVISAO_PROVA"].ToString()))
              dr["REVISAO_PROVA"] = DBNull.Value;
            if (dr["PRAZO_ENTREGA"] != null && string.IsNullOrEmpty(dr["PRAZO_ENTREGA"].ToString()))
              dr["PRAZO_ENTREGA"] = DBNull.Value;
            if (dr["PAGAMENTO_PRE_EXECUCAO"] != null && string.IsNullOrEmpty(dr["PAGAMENTO_PRE_EXECUCAO"].ToString()))
              dr["PAGAMENTO_PRE_EXECUCAO"] = DBNull.Value;
            if (dr["IMPRIME_VIA_WEB"] != null && string.IsNullOrEmpty(dr["IMPRIME_VIA_WEB"].ToString()))
              dr["IMPRIME_VIA_WEB"] = DBNull.Value;
            if (dr["NUM_SOLICIT_NAO_PAGAS"] != null && string.IsNullOrEmpty(dr["NUM_SOLICIT_NAO_PAGAS"].ToString()))
              dr["NUM_SOLICIT_NAO_PAGAS"] = DBNull.Value;
            if (dr["SOLICITAVEL_WEB"] != null && string.IsNullOrEmpty(dr["SOLICITAVEL_WEB"].ToString()))
              dr["SOLICITAVEL_WEB"] = DBNull.Value;
            if (dr["TIPO_SERVICO"] != null && string.IsNullOrEmpty(dr["TIPO_SERVICO"].ToString()))
              dr["TIPO_SERVICO"] = DBNull.Value;
            if (dr["MAX_QTDE_SOLIC"] != null && string.IsNullOrEmpty(dr["MAX_QTDE_SOLIC"].ToString()))
              dr["MAX_QTDE_SOLIC"] = DBNull.Value;
            if (dr["EXIGE_AUTORIZACAO"] != null && string.IsNullOrEmpty(dr["EXIGE_AUTORIZACAO"].ToString()))
              dr["EXIGE_AUTORIZACAO"] = DBNull.Value;
            if (dr["REFAZ_CONTRATO"] != null && string.IsNullOrEmpty(dr["REFAZ_CONTRATO"].ToString()))
              dr["REFAZ_CONTRATO"] = DBNull.Value;
            if (dr["ALUNO"] != null && string.IsNullOrEmpty(dr["ALUNO"].ToString()))
              dr["ALUNO"] = DBNull.Value;
            if (dr["DOCENTE"] != null && string.IsNullOrEmpty(dr["DOCENTE"].ToString()))
              dr["DOCENTE"] = DBNull.Value;
            if (dr["OBS_VISIVEL"] != null && string.IsNullOrEmpty(dr["OBS_VISIVEL"].ToString()))
              dr["OBS_VISIVEL"] = DBNull.Value;
            if (dr["VISUALIZACAO_SETOR"] != null && string.IsNullOrEmpty(dr["VISUALIZACAO_SETOR"].ToString()))
              dr["VISUALIZACAO_SETOR"] = DBNull.Value;
            if (dr["PRAZO_CANCELAMENTO"] != null && string.IsNullOrEmpty(dr["PRAZO_CANCELAMENTO"].ToString()))
              dr["PRAZO_CANCELAMENTO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["SERVICO"] == null || string.IsNullOrEmpty(dr["SERVICO"].ToString()))
            {
              return "Favor informar o Serviço.";
            }

            if (dr["SERVICO"] != null && !string.IsNullOrEmpty(dr["SERVICO"].ToString()))
            {
              if (dr["SERVICO"].ToString().Length > 20)
              {
                return "O Serviço deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["CODIGO_LANC"] != null && !string.IsNullOrEmpty(dr["CODIGO_LANC"].ToString()))
            {
              if (dr["CODIGO_LANC"].ToString().Length > 20)
              {
                return "O Código de Lançamento deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["DESCRICAO"] == null || string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              return "Favor informar a Descrição.";
            }

            if (dr["DESCRICAO"] != null && !string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              if (dr["DESCRICAO"].ToString().Length > 100)
              {
                return "A Descrição deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["CUSTO_UNITARIO"] != null && !string.IsNullOrEmpty(dr["CUSTO_UNITARIO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["CUSTO_UNITARIO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["CUSTO_UNITARIO"]).ToString("0.############").Split(',')[0].Length > 8) || (Convert.ToDecimal(dr["CUSTO_UNITARIO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["CUSTO_UNITARIO"]).ToString("0.############").Split(',')[1].Length > 6))
              {
                return "O Custo Unitário deve ser um valor numérico com, no máximo, 8 dígitos na parte inteira e 6 casas decimal.";
              }
            }

            if (dr["SOLICITAVEL"] == null || string.IsNullOrEmpty(dr["SOLICITAVEL"].ToString()))
            {
              return "Favor informar o Solicitável.";
            }

            if (dr["SOLICITAVEL"] != null && !string.IsNullOrEmpty(dr["SOLICITAVEL"].ToString()))
            {
              if (dr["SOLICITAVEL"].ToString().Length > 1)
              {
                return "O Solicitável deve possuir no máximo 1 caractere.";
              }
            }

            if (dr["SISTEMA"] != null && !string.IsNullOrEmpty(dr["SISTEMA"].ToString()))
            {
              if (dr["SISTEMA"].ToString().Length > 12)
              {
                return "O Sistema deve possuir no máximo 12 caracteres.";
              }
            }

            if (dr["GRUPO_RELATORIO"] != null && !string.IsNullOrEmpty(dr["GRUPO_RELATORIO"].ToString()))
            {
              if (dr["GRUPO_RELATORIO"].ToString().Length > 50)
              {
                return "O Grupo de Relatório deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["RELATORIO"] != null && !string.IsNullOrEmpty(dr["RELATORIO"].ToString()))
            {
              if (dr["RELATORIO"].ToString().Length > 50)
              {
                return "O Retatório deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["REVISAO_PROVA"] == null || string.IsNullOrEmpty(dr["REVISAO_PROVA"].ToString()))
            {
              return "Favor informar a Revisão de Prova.";
            }

            if (dr["REVISAO_PROVA"] != null && !string.IsNullOrEmpty(dr["REVISAO_PROVA"].ToString()))
            {
              if (dr["REVISAO_PROVA"].ToString().Length > 1)
              {
                return "A Revisão de Prova deve possuir no máximo 1 caractere.";
              }
            }

            if (dr["PRAZO_ENTREGA"] == null || string.IsNullOrEmpty(dr["PRAZO_ENTREGA"].ToString()))
            {
              return "Favor informar o Prazo de Entrega.";
            }

            if (dr["PRAZO_ENTREGA"] != null && !string.IsNullOrEmpty(dr["PRAZO_ENTREGA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["PRAZO_ENTREGA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PRAZO_ENTREGA"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["PRAZO_ENTREGA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PRAZO_ENTREGA"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Prazo de Entrega deve ser um valor inteiro com 3 dígitos no máximo.";
              }

              if (dr["PRAZO_ENTREGA"] != null && !string.IsNullOrEmpty(dr["PRAZO_ENTREGA"].ToString()))
              {
                if (Convert.ToDecimal(dr["PRAZO_ENTREGA"]) < 0)
                {
                  return "O Prazo de Entrega deve ser maior ou igual a 0.";
                }
              }
            }

            if (dr["PAGAMENTO_PRE_EXECUCAO"] != null && !string.IsNullOrEmpty(dr["PAGAMENTO_PRE_EXECUCAO"].ToString()))
            {
              if (dr["PAGAMENTO_PRE_EXECUCAO"].ToString().Length > 1)
              {
                return "O Pagamento Pré Executado deve possuir no máximo 1 caractere.";
              }

              if (dr["PAGAMENTO_PRE_EXECUCAO"] != null && !string.IsNullOrEmpty(dr["PAGAMENTO_PRE_EXECUCAO"].ToString()))
              {
                List<object> lstPAGAMENTO_PRE_EXECUCAO = new List<object>(new object[]{"N", "S"});
                if(!lstPAGAMENTO_PRE_EXECUCAO.Contains(dr["PAGAMENTO_PRE_EXECUCAO"].ToString()))
                {
                  return "Os valores permitidos para o Pagamento Pré Executado são ''N'' ou ''S''.";
                }
              }
            }

            if (dr["IMPRIME_VIA_WEB"] != null && !string.IsNullOrEmpty(dr["IMPRIME_VIA_WEB"].ToString()))
            {
              if (dr["IMPRIME_VIA_WEB"].ToString().Length > 1)
              {
                return "O Imprime_via_web deve possuir no máximo 1 caractere.";
              }
            }

            if (dr["NUM_SOLICIT_NAO_PAGAS"] != null && !string.IsNullOrEmpty(dr["NUM_SOLICIT_NAO_PAGAS"].ToString()))
            {
              if ((Convert.ToDecimal(dr["NUM_SOLICIT_NAO_PAGAS"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["NUM_SOLICIT_NAO_PAGAS"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["NUM_SOLICIT_NAO_PAGAS"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["NUM_SOLICIT_NAO_PAGAS"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Solicitações Não Pagas deve ser um valor inteiro com 3 dígitos no máximo.";
              }
            }

            if (dr["SOLICITAVEL_WEB"] == null || string.IsNullOrEmpty(dr["SOLICITAVEL_WEB"].ToString()))
            {
              return "Favor informar o Solicitavel_web.";
            }

            if (dr["SOLICITAVEL_WEB"] != null && !string.IsNullOrEmpty(dr["SOLICITAVEL_WEB"].ToString()))
            {
              if (dr["SOLICITAVEL_WEB"].ToString().Length > 1)
              {
                return "O Solicitavel_web deve possuir no máximo 1 caractere.";
              }

              if (dr["SOLICITAVEL_WEB"] != null && !string.IsNullOrEmpty(dr["SOLICITAVEL_WEB"].ToString()))
              {
                List<object> lstSOLICITAVEL_WEB = new List<object>(new object[]{"N", "S"});
                if(!lstSOLICITAVEL_WEB.Contains(dr["SOLICITAVEL_WEB"].ToString()))
                {
                  return "Os valores permitidos para o Solicitavel_web são ''N'' ou ''S''.";
                }
              }
            }

            if (dr["TIPO_SERVICO"] != null && !string.IsNullOrEmpty(dr["TIPO_SERVICO"].ToString()))
            {
              if (dr["TIPO_SERVICO"].ToString().Length > 20)
              {
                return "O Tipo_servico deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["MAX_QTDE_SOLIC"] != null && !string.IsNullOrEmpty(dr["MAX_QTDE_SOLIC"].ToString()))
            {
              if ((Convert.ToDecimal(dr["MAX_QTDE_SOLIC"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["MAX_QTDE_SOLIC"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["MAX_QTDE_SOLIC"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["MAX_QTDE_SOLIC"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A Quantidade máxima por solicitação deve ser um valor inteiro com 3 dígitos no máximo.";
              }
            }

            if (dr["EXIGE_AUTORIZACAO"] != null && !string.IsNullOrEmpty(dr["EXIGE_AUTORIZACAO"].ToString()))
            {
              if (dr["EXIGE_AUTORIZACAO"].ToString().Length > 1)
              {
                return "O Exige_autorizacao deve possuir no máximo 1 caractere.";
              }
            }

            if (dr["REFAZ_CONTRATO"] != null && !string.IsNullOrEmpty(dr["REFAZ_CONTRATO"].ToString()))
            {
              if (dr["REFAZ_CONTRATO"].ToString().Length > 1)
              {
                return "O Refaz_contrato deve possuir no máximo 1 caractere.";
              }
            }

            if (dr["ALUNO"] != null && !string.IsNullOrEmpty(dr["ALUNO"].ToString()))
            {
              if (dr["ALUNO"].ToString().Length > 1)
              {
                return "O Aluno deve possuir no máximo 1 caractere.";
              }
            }

            if (dr["DOCENTE"] != null && !string.IsNullOrEmpty(dr["DOCENTE"].ToString()))
            {
              if (dr["DOCENTE"].ToString().Length > 1)
              {
                return "O Docente deve possuir no máximo 1 caractere.";
              }
            }

            if (dr["OBS_VISIVEL"] != null && !string.IsNullOrEmpty(dr["OBS_VISIVEL"].ToString()))
            {
              if (dr["OBS_VISIVEL"].ToString().Length > 1)
              {
                return "O Obs_visivel deve possuir no máximo 1 caractere.";
              }
            }

            if (dr["VISUALIZACAO_SETOR"] != null && !string.IsNullOrEmpty(dr["VISUALIZACAO_SETOR"].ToString()))
            {
              if (dr["VISUALIZACAO_SETOR"].ToString().Length > 1)
              {
                return "O Visualizacao_setor deve possuir no máximo 1 caractere.";
              }
            }

            if (dr["PRAZO_CANCELAMENTO"] != null && !string.IsNullOrEmpty(dr["PRAZO_CANCELAMENTO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["PRAZO_CANCELAMENTO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PRAZO_CANCELAMENTO"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["PRAZO_CANCELAMENTO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PRAZO_CANCELAMENTO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Prazo_cancelamento deve ser um valor inteiro com 10 dígitos no máximo.";
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

    public static Ly_tabela_servicos Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_tabela_servicos Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_tabela_servicos tab = new Ly_tabela_servicos();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_tabela_servicos tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @servico, string @codigo_lanc, string @descricao, decimal? @custo_unitario, 
        string @solicitavel, string @sistema, string @grupo_relatorio, string @relatorio, 
        string @revisao_prova, decimal? @prazo_entrega, string @pagamento_pre_execucao, string @imprime_via_web, 
        decimal? @num_solicit_nao_pagas, string @solicitavel_web, string @tipo_servico, decimal? @max_qtde_solic, 
        string @exige_autorizacao, string @refaz_contrato, string @aluno, string @docente, 
        string @obs_visivel, string @visualizacao_setor, decimal? @prazo_cancelamento
      ) {
        return (Row)Add(new object[] {
          @servico, @codigo_lanc, @descricao, @custo_unitario, @solicitavel, @sistema, @grupo_relatorio, 
          @relatorio, @revisao_prova, @prazo_entrega, @pagamento_pre_execucao, @imprime_via_web, 
          @num_solicit_nao_pagas, @solicitavel_web, @tipo_servico, @max_qtde_solic, @exige_autorizacao, 
          @refaz_contrato, @aluno, @docente, @obs_visivel, @visualizacao_setor, @prazo_cancelamento
        });
      }

      public Row Find(
        string @servico
      ) {
        return (Row)Find(new object[] {
          @servico
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
      /// SERVICO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Servico { get { return (string)this["servico"]; } set { this["servico"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CODIGO_LANC: VARCHAR(20)
      /// </summary>
      public string Codigo_lanc { get { return (string)this["codigo_lanc"]; } set { this["codigo_lanc"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO: VARCHAR(100), Not Null
      /// </summary>
      public string Descricao { get { return (string)this["descricao"]; } set { this["descricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CUSTO_UNITARIO: NUMERIC(14, 6)
      /// </summary>
      public decimal? Custo_unitario { get { return (decimal?)this["custo_unitario"]; } set { this["custo_unitario"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SOLICITAVEL: CHAR(1), Not Null
      /// </summary>
      public string Solicitavel { get { return (string)this["solicitavel"]; } set { this["solicitavel"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SISTEMA: VARCHAR(12)
      /// </summary>
      public string Sistema { get { return (string)this["sistema"]; } set { this["sistema"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// GRUPO_RELATORIO: VARCHAR(50)
      /// </summary>
      public string Grupo_relatorio { get { return (string)this["grupo_relatorio"]; } set { this["grupo_relatorio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// RELATORIO: VARCHAR(50)
      /// </summary>
      public string Relatorio { get { return (string)this["relatorio"]; } set { this["relatorio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// REVISAO_PROVA: CHAR(1), Not Null
      /// </summary>
      public string Revisao_prova { get { return (string)this["revisao_prova"]; } set { this["revisao_prova"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PRAZO_ENTREGA: NUMERIC(3), Not Null
      /// </summary>
      public decimal? Prazo_entrega { get { return (decimal?)this["prazo_entrega"]; } set { this["prazo_entrega"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PAGAMENTO_PRE_EXECUCAO: CHAR(1)
      /// </summary>
      public string Pagamento_pre_execucao { get { return (string)this["pagamento_pre_execucao"]; } set { this["pagamento_pre_execucao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// IMPRIME_VIA_WEB: CHAR(1)
      /// </summary>
      public string Imprime_via_web { get { return (string)this["imprime_via_web"]; } set { this["imprime_via_web"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NUM_SOLICIT_NAO_PAGAS: NUMERIC(3)
      /// </summary>
      public decimal? Num_solicit_nao_pagas { get { return (decimal?)this["num_solicit_nao_pagas"]; } set { this["num_solicit_nao_pagas"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SOLICITAVEL_WEB: CHAR(1), Not Null
      /// </summary>
      public string Solicitavel_web { get { return (string)this["solicitavel_web"]; } set { this["solicitavel_web"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TIPO_SERVICO: VARCHAR(20)
      /// </summary>
      public string Tipo_servico { get { return (string)this["tipo_servico"]; } set { this["tipo_servico"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// MAX_QTDE_SOLIC: NUMERIC(3)
      /// </summary>
      public decimal? Max_qtde_solic { get { return (decimal?)this["max_qtde_solic"]; } set { this["max_qtde_solic"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// EXIGE_AUTORIZACAO: CHAR(1)
      /// </summary>
      public string Exige_autorizacao { get { return (string)this["exige_autorizacao"]; } set { this["exige_autorizacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// REFAZ_CONTRATO: CHAR(1)
      /// </summary>
      public string Refaz_contrato { get { return (string)this["refaz_contrato"]; } set { this["refaz_contrato"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ALUNO: CHAR(1)
      /// </summary>
      public string Aluno { get { return (string)this["aluno"]; } set { this["aluno"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DOCENTE: CHAR(1)
      /// </summary>
      public string Docente { get { return (string)this["docente"]; } set { this["docente"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// OBS_VISIVEL: CHAR(1)
      /// </summary>
      public string Obs_visivel { get { return (string)this["obs_visivel"]; } set { this["obs_visivel"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// VISUALIZACAO_SETOR: CHAR(1)
      /// </summary>
      public string Visualizacao_setor { get { return (string)this["visualizacao_setor"]; } set { this["visualizacao_setor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PRAZO_CANCELAMENTO: NUMERIC(10)
      /// </summary>
      public decimal? Prazo_cancelamento { get { return (decimal?)this["prazo_cancelamento"]; } set { this["prazo_cancelamento"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @servico, string @descricao, string @solicitavel, string @revisao_prova, decimal? @prazo_entrega, 
                               string @solicitavel_web) {
        return Insert(connection,
                      @servico, @descricao, @solicitavel, @revisao_prova, @prazo_entrega, @solicitavel_web,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @servico, string @descricao, string @solicitavel, string @revisao_prova, decimal? @prazo_entrega, 
                               string @solicitavel_web,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_tabela_servicos(),
          new string[] { "servico", "descricao", "solicitavel", "revisao_prova", "prazo_entrega", 
                         "solicitavel_web" },
          new object[] { @servico, @descricao, @solicitavel, @revisao_prova, @prazo_entrega, 
                         @solicitavel_web },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkServico,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_tabela_servicos(),
          new string[] { "servico" },
          new object[] { pkServico },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkServico) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_tabela_servicos(),
          new string[] { "servico" },
          new object[] { pkServico }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkServico) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_tabela_servicos(),
          new string[] { "servico" },
          new object[] { pkServico }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Servico { get { return (string)row["servico", DataRowVersion.Original]; } }
        public string Codigo_lanc { get { return (string)row["codigo_lanc", DataRowVersion.Original]; } }
        public string Descricao { get { return (string)row["descricao", DataRowVersion.Original]; } }
        public decimal? Custo_unitario { get { return (decimal?)row["custo_unitario", DataRowVersion.Original]; } }
        public string Solicitavel { get { return (string)row["solicitavel", DataRowVersion.Original]; } }
        public string Sistema { get { return (string)row["sistema", DataRowVersion.Original]; } }
        public string Grupo_relatorio { get { return (string)row["grupo_relatorio", DataRowVersion.Original]; } }
        public string Relatorio { get { return (string)row["relatorio", DataRowVersion.Original]; } }
        public string Revisao_prova { get { return (string)row["revisao_prova", DataRowVersion.Original]; } }
        public decimal? Prazo_entrega { get { return (decimal?)row["prazo_entrega", DataRowVersion.Original]; } }
        public string Pagamento_pre_execucao { get { return (string)row["pagamento_pre_execucao", DataRowVersion.Original]; } }
        public string Imprime_via_web { get { return (string)row["imprime_via_web", DataRowVersion.Original]; } }
        public decimal? Num_solicit_nao_pagas { get { return (decimal?)row["num_solicit_nao_pagas", DataRowVersion.Original]; } }
        public string Solicitavel_web { get { return (string)row["solicitavel_web", DataRowVersion.Original]; } }
        public string Tipo_servico { get { return (string)row["tipo_servico", DataRowVersion.Original]; } }
        public decimal? Max_qtde_solic { get { return (decimal?)row["max_qtde_solic", DataRowVersion.Original]; } }
        public string Exige_autorizacao { get { return (string)row["exige_autorizacao", DataRowVersion.Original]; } }
        public string Refaz_contrato { get { return (string)row["refaz_contrato", DataRowVersion.Original]; } }
        public string Aluno { get { return (string)row["aluno", DataRowVersion.Original]; } }
        public string Docente { get { return (string)row["docente", DataRowVersion.Original]; } }
        public string Obs_visivel { get { return (string)row["obs_visivel", DataRowVersion.Original]; } }
        public string Visualizacao_setor { get { return (string)row["visualizacao_setor", DataRowVersion.Original]; } }
        public decimal? Prazo_cancelamento { get { return (decimal?)row["prazo_cancelamento", DataRowVersion.Original]; } }

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
    #region Coluna: SERVICO
    private ColumnServico pvColServico;

    public sealed class ColumnServico : TDataColumn {

      public ColumnServico() {
        this.ColumnName = "servico";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Serviço";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_tabela_servicos.servico"; } }
      public override string OriginalCol { get { return "servico"; } }
      public override string RefCol { get { return "LY_TABELA_SERVICOS.SERVICO"; } }
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

    #region Coluna: CODIGO_LANC
    private ColumnCodigo_lanc pvColCodigo_lanc;

    public sealed class ColumnCodigo_lanc : TDataColumn {

      public ColumnCodigo_lanc() {
        this.ColumnName = "codigo_lanc";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Código de Lançamento";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_tabela_servicos.codigo_lanc"; } }
      public override string OriginalCol { get { return "codigo_lanc"; } }
      public override string RefCol { get { return "LY_TABELA_SERVICOS.CODIGO_LANC"; } }
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

    #region Coluna: DESCRICAO
    private ColumnDescricao pvColDescricao;

    public sealed class ColumnDescricao : TDataColumn {

      public ColumnDescricao() {
        this.ColumnName = "descricao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Descrição";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_tabela_servicos.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "LY_TABELA_SERVICOS.DESCRICAO"; } }
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

    #region Coluna: CUSTO_UNITARIO
    private ColumnCusto_unitario pvColCusto_unitario;

    public sealed class ColumnCusto_unitario : TDataColumn {

      public ColumnCusto_unitario() {
        this.ColumnName = "custo_unitario";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "Custo Unitário";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_tabela_servicos.custo_unitario"; } }
      public override string OriginalCol { get { return "custo_unitario"; } }
      public override string RefCol { get { return "LY_TABELA_SERVICOS.CUSTO_UNITARIO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 16; } }
      public override int Precision { get { return 14; } }
      public override int Scale { get { return 6; } }
      public override string SqlServerType { get { return "NUMERIC(14, 6)"; } }

    }
    #endregion

    #region Coluna: SOLICITAVEL
    private ColumnSolicitavel pvColSolicitavel;

    public sealed class ColumnSolicitavel : TDataColumn {

      public ColumnSolicitavel() {
        this.ColumnName = "solicitavel";
        this.DataType = typeof(string);
        this.DefaultValue = "S";

      }


      protected override string GetName(){return "Solicitável";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_tabela_servicos.solicitavel"; } }
      public override string OriginalCol { get { return "solicitavel"; } }
      public override string RefCol { get { return "LY_TABELA_SERVICOS.SOLICITAVEL"; } }
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

    #region Coluna: SISTEMA
    private ColumnSistema pvColSistema;

    public sealed class ColumnSistema : TDataColumn {

      public ColumnSistema() {
        this.ColumnName = "sistema";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Sistema";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_tabela_servicos.sistema"; } }
      public override string OriginalCol { get { return "sistema"; } }
      public override string RefCol { get { return "LY_TABELA_SERVICOS.SISTEMA"; } }
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

    #region Coluna: GRUPO_RELATORIO
    private ColumnGrupo_relatorio pvColGrupo_relatorio;

    public sealed class ColumnGrupo_relatorio : TDataColumn {

      public ColumnGrupo_relatorio() {
        this.ColumnName = "grupo_relatorio";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Grupo de Relatório";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_tabela_servicos.grupo_relatorio"; } }
      public override string OriginalCol { get { return "grupo_relatorio"; } }
      public override string RefCol { get { return "LY_TABELA_SERVICOS.GRUPO_RELATORIO"; } }
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

    #region Coluna: RELATORIO
    private ColumnRelatorio pvColRelatorio;

    public sealed class ColumnRelatorio : TDataColumn {

      public ColumnRelatorio() {
        this.ColumnName = "relatorio";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Retatório";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_tabela_servicos.relatorio"; } }
      public override string OriginalCol { get { return "relatorio"; } }
      public override string RefCol { get { return "LY_TABELA_SERVICOS.RELATORIO"; } }
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

    #region Coluna: REVISAO_PROVA
    private ColumnRevisao_prova pvColRevisao_prova;

    public sealed class ColumnRevisao_prova : TDataColumn {

      public ColumnRevisao_prova() {
        this.ColumnName = "revisao_prova";
        this.DataType = typeof(string);
        this.DefaultValue = "N";

      }


      protected override string GetName(){return "Revisão de Prova";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_tabela_servicos.revisao_prova"; } }
      public override string OriginalCol { get { return "revisao_prova"; } }
      public override string RefCol { get { return "LY_TABELA_SERVICOS.REVISAO_PROVA"; } }
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

    #region Coluna: PRAZO_ENTREGA
    private ColumnPrazo_entrega pvColPrazo_entrega;

    public sealed class ColumnPrazo_entrega : TDataColumn {

      public ColumnPrazo_entrega() {
        this.ColumnName = "prazo_entrega";
        this.DataType = typeof(decimal);
        this.DefaultValue = 0m;

      }


      protected override string GetName(){return "Prazo de Entrega";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_tabela_servicos.prazo_entrega"; } }
      public override string OriginalCol { get { return "prazo_entrega"; } }
      public override string RefCol { get { return "LY_TABELA_SERVICOS.PRAZO_ENTREGA"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 4; } }
      public override int Precision { get { return 3; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(3)"; } }

      public override object MinValue { get { return 0; } }

    }
    #endregion

    #region Coluna: PAGAMENTO_PRE_EXECUCAO
    private ColumnPagamento_pre_execucao pvColPagamento_pre_execucao;

    public sealed class ColumnPagamento_pre_execucao : TDataColumn {

      public ColumnPagamento_pre_execucao() {
        this.ColumnName = "pagamento_pre_execucao";
        this.DataType = typeof(string);
        this.DefaultValue = "N";

      }


      protected override string GetName(){return "Pagamento Pré Executado";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_tabela_servicos.pagamento_pre_execucao"; } }
      public override string OriginalCol { get { return "pagamento_pre_execucao"; } }
      public override string RefCol { get { return "LY_TABELA_SERVICOS.PAGAMENTO_PRE_EXECUCAO"; } }
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

    #region Coluna: IMPRIME_VIA_WEB
    private ColumnImprime_via_web pvColImprime_via_web;

    public sealed class ColumnImprime_via_web : TDataColumn {

      public ColumnImprime_via_web() {
        this.ColumnName = "imprime_via_web";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_tabela_servicos.imprime_via_web"; } }
      public override string OriginalCol { get { return "imprime_via_web"; } }
      public override string RefCol { get { return "LY_TABELA_SERVICOS.IMPRIME_VIA_WEB"; } }
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

    #region Coluna: NUM_SOLICIT_NAO_PAGAS
    private ColumnNum_solicit_nao_pagas pvColNum_solicit_nao_pagas;

    public sealed class ColumnNum_solicit_nao_pagas : TDataColumn {

      public ColumnNum_solicit_nao_pagas() {
        this.ColumnName = "num_solicit_nao_pagas";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "Solicitações Não Pagas";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_tabela_servicos.num_solicit_nao_pagas"; } }
      public override string OriginalCol { get { return "num_solicit_nao_pagas"; } }
      public override string RefCol { get { return "LY_TABELA_SERVICOS.NUM_SOLICIT_NAO_PAGAS"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 4; } }
      public override int Precision { get { return 3; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(3)"; } }

    }
    #endregion

    #region Coluna: SOLICITAVEL_WEB
    private ColumnSolicitavel_web pvColSolicitavel_web;

    public sealed class ColumnSolicitavel_web : TDataColumn {

      public ColumnSolicitavel_web() {
        this.ColumnName = "solicitavel_web";
        this.DataType = typeof(string);
        this.DefaultValue = "S";

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_tabela_servicos.solicitavel_web"; } }
      public override string OriginalCol { get { return "solicitavel_web"; } }
      public override string RefCol { get { return "LY_TABELA_SERVICOS.SOLICITAVEL_WEB"; } }
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

    #region Coluna: TIPO_SERVICO
    private ColumnTipo_servico pvColTipo_servico;

    public sealed class ColumnTipo_servico : TDataColumn {

      public ColumnTipo_servico() {
        this.ColumnName = "tipo_servico";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_tabela_servicos.tipo_servico"; } }
      public override string OriginalCol { get { return "tipo_servico"; } }
      public override string RefCol { get { return "LY_TABELA_SERVICOS.TIPO_SERVICO"; } }
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

    #region Coluna: MAX_QTDE_SOLIC
    private ColumnMax_qtde_solic pvColMax_qtde_solic;

    public sealed class ColumnMax_qtde_solic : TDataColumn {

      public ColumnMax_qtde_solic() {
        this.ColumnName = "max_qtde_solic";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "Quantidade máxima por solicitação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_tabela_servicos.max_qtde_solic"; } }
      public override string OriginalCol { get { return "max_qtde_solic"; } }
      public override string RefCol { get { return "LY_TABELA_SERVICOS.MAX_QTDE_SOLIC"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 4; } }
      public override int Precision { get { return 3; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(3)"; } }

    }
    #endregion

    #region Coluna: EXIGE_AUTORIZACAO
    private ColumnExige_autorizacao pvColExige_autorizacao;

    public sealed class ColumnExige_autorizacao : TDataColumn {

      public ColumnExige_autorizacao() {
        this.ColumnName = "exige_autorizacao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_tabela_servicos.exige_autorizacao"; } }
      public override string OriginalCol { get { return "exige_autorizacao"; } }
      public override string RefCol { get { return "LY_TABELA_SERVICOS.EXIGE_AUTORIZACAO"; } }
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

    #region Coluna: REFAZ_CONTRATO
    private ColumnRefaz_contrato pvColRefaz_contrato;

    public sealed class ColumnRefaz_contrato : TDataColumn {

      public ColumnRefaz_contrato() {
        this.ColumnName = "refaz_contrato";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_tabela_servicos.refaz_contrato"; } }
      public override string OriginalCol { get { return "refaz_contrato"; } }
      public override string RefCol { get { return "LY_TABELA_SERVICOS.REFAZ_CONTRATO"; } }
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

    #region Coluna: ALUNO
    private ColumnAluno pvColAluno;

    public sealed class ColumnAluno : TDataColumn {

      public ColumnAluno() {
        this.ColumnName = "aluno";
        this.DataType = typeof(string);
        this.DefaultValue = "S";

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_tabela_servicos.aluno"; } }
      public override string OriginalCol { get { return "aluno"; } }
      public override string RefCol { get { return "LY_TABELA_SERVICOS.ALUNO"; } }
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

    #region Coluna: DOCENTE
    private ColumnDocente pvColDocente;

    public sealed class ColumnDocente : TDataColumn {

      public ColumnDocente() {
        this.ColumnName = "docente";
        this.DataType = typeof(string);
        this.DefaultValue = "N";

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_tabela_servicos.docente"; } }
      public override string OriginalCol { get { return "docente"; } }
      public override string RefCol { get { return "LY_TABELA_SERVICOS.DOCENTE"; } }
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

    #region Coluna: OBS_VISIVEL
    private ColumnObs_visivel pvColObs_visivel;

    public sealed class ColumnObs_visivel : TDataColumn {

      public ColumnObs_visivel() {
        this.ColumnName = "obs_visivel";
        this.DataType = typeof(string);
        this.DefaultValue = "S";

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_tabela_servicos.obs_visivel"; } }
      public override string OriginalCol { get { return "obs_visivel"; } }
      public override string RefCol { get { return "LY_TABELA_SERVICOS.OBS_VISIVEL"; } }
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

    #region Coluna: VISUALIZACAO_SETOR
    private ColumnVisualizacao_setor pvColVisualizacao_setor;

    public sealed class ColumnVisualizacao_setor : TDataColumn {

      public ColumnVisualizacao_setor() {
        this.ColumnName = "visualizacao_setor";
        this.DataType = typeof(string);
        this.DefaultValue = "N";

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_tabela_servicos.visualizacao_setor"; } }
      public override string OriginalCol { get { return "visualizacao_setor"; } }
      public override string RefCol { get { return "LY_TABELA_SERVICOS.VISUALIZACAO_SETOR"; } }
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

    #region Coluna: PRAZO_CANCELAMENTO
    private ColumnPrazo_cancelamento pvColPrazo_cancelamento;

    public sealed class ColumnPrazo_cancelamento : TDataColumn {

      public ColumnPrazo_cancelamento() {
        this.ColumnName = "prazo_cancelamento";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_tabela_servicos.prazo_cancelamento"; } }
      public override string OriginalCol { get { return "prazo_cancelamento"; } }
      public override string RefCol { get { return "LY_TABELA_SERVICOS.PRAZO_CANCELAMENTO"; } }
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
    #endregion
  }

  public partial class Serv_solicitacao : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Serv_solicitacao() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Serv_solicitacao");
        ctorCustom = CRCustomLib.GetCustomConstructor("Serv_solicitacao");
      }
      catch{}
    }

    public Serv_solicitacao() {
      pvColSolicitacao = new ColumnSolicitacao(); Columns.Add(pvColSolicitacao);
      pvColItem_solicitacao = new ColumnItem_solicitacao(); Columns.Add(pvColItem_solicitacao);
      pvColServico = new ColumnServico(); Columns.Add(pvColServico);
      pvColLanc_deb = new ColumnLanc_deb(); Columns.Add(pvColLanc_deb);
      pvColQtd = new ColumnQtd(); Columns.Add(pvColQtd);
      pvColObs = new ColumnObs(); Columns.Add(pvColObs);
      pvColAluno = new ColumnAluno(); Columns.Add(pvColAluno);
      pvColNome_compl = new ColumnNome_compl(); Columns.Add(pvColNome_compl);
      pvColData = new ColumnData(); Columns.Add(pvColData);
      pvColDescricao = new ColumnDescricao(); Columns.Add(pvColDescricao);
      pvColStatus = new ColumnStatus(); Columns.Add(pvColStatus);
      pvColPagamento_pre_execucao = new ColumnPagamento_pre_execucao(); Columns.Add(pvColPagamento_pre_execucao);

      InitRows(new RowCollection(BaseRows));
      SetTableName("serv_solicitacao");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      throw new InvalidOperationException("O método NewTypedRow() é inválido para o DataTable serv_solicitacao.");
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
      get { return "serv_solicitacao"; }
    }

    protected override sealed string Name { get { return this.TableName; } }

    [Browsable(false)]
    public override sealed bool ReadOnly {
      get { return true; }
    }

    [Browsable(false)]
    public new RowCollection Rows { get { return (RowCollection)base.Rows; } }


    protected override sealed DataRow NewRowFromBuilder(DataRowBuilder builder) {
      return new Row(builder);
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
          if(lower == "solicitacao") { result.Add(toLower ? "solicitacao" : column); continue; }
          if(lower == "item_solicitacao") { result.Add(toLower ? "item_solicitacao" : column); continue; }
          if(lower == "servico") { result.Add(toLower ? "servico" : column); continue; }
          if(lower == "lanc_deb") { result.Add(toLower ? "lanc_deb" : column); continue; }
          if(lower == "qtd") { result.Add(toLower ? "qtd" : column); continue; }
          if(lower == "obs") { result.Add(toLower ? "obs" : column); continue; }
          if(lower == "aluno") { result.Add(toLower ? "aluno" : column); continue; }
          if(lower == "nome_compl") { result.Add(toLower ? "nome_compl" : column); continue; }
          if(lower == "data") { result.Add(toLower ? "data" : column); continue; }
          if(lower == "descricao") { result.Add(toLower ? "descricao" : column); continue; }
          if(lower == "status") { result.Add(toLower ? "status" : column); continue; }
          if(lower == "pagamento_pre_execucao") { result.Add(toLower ? "pagamento_pre_execucao" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "solicitacao", "item_solicitacao", "servico", "lanc_deb", "qtd", 
                                           "obs", "aluno", "nome_compl", "data", "descricao", "status", "pagamento_pre_execucao" }
                          : new string[] { "SOLICITACAO", "ITEM_SOLICITACAO", "SERVICO", "LANC_DEB", "QTD", 
                                           "OBS", "ALUNO", "NOME_COMPL", "DATA", "DESCRICAO", "STATUS", "PAGAMENTO_PRE_EXECUCAO" });
        else
          result.AddRange(toLower
                          ? new string[] { "solicitacao", "item_solicitacao", "servico", "lanc_deb", "qtd", 
                                           "obs", "aluno", "nome_compl", "data", "descricao", "status", "pagamento_pre_execucao" }
                          : new string[] { "SOLICITACAO", "ITEM_SOLICITACAO", "SERVICO", "LANC_DEB", "QTD", 
                                           "OBS", "ALUNO", "NOME_COMPL", "DATA", "DESCRICAO", "STATUS", "PAGAMENTO_PRE_EXECUCAO" });

      return result;
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
        "serv_solicitacao.solicitacao", "serv_solicitacao.item_solicitacao", "serv_solicitacao.servico", 
        "serv_solicitacao.lanc_deb", "serv_solicitacao.qtd", "serv_solicitacao.obs", "serv_solicitacao.aluno", 
        "serv_solicitacao.nome_compl", "serv_solicitacao.data", "serv_solicitacao.descricao", 
        "serv_solicitacao.status", "serv_solicitacao.pagamento_pre_execucao"
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
          "FROM serv_solicitacao " +
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
            count = da.Fill(ds, startRecord, maxRecords, "serv_solicitacao");
            if(fakeds) ds.Tables.Remove(this);
          }
          catch(Exception exc) {
            throw new Exception(exc.Message + " " +
                                "Select statement: [" + select + "], " +
                                "Parameters: [" + StrLib.EnumerableToStr(whereValues) + "]");
          }
        }

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
      throw new InvalidOperationException("O método Put() é inválido para o DataTable serv_solicitacao");
    }

    public void ValidaConvEmptyStrToNull(ref DataRow dr)
    {
      if (dr != null)
      {
            if (dr["SOLICITACAO"] != null && string.IsNullOrEmpty(dr["SOLICITACAO"].ToString()))
              dr["SOLICITACAO"] = DBNull.Value;
            if (dr["ITEM_SOLICITACAO"] != null && string.IsNullOrEmpty(dr["ITEM_SOLICITACAO"].ToString()))
              dr["ITEM_SOLICITACAO"] = DBNull.Value;
            if (dr["SERVICO"] != null && string.IsNullOrEmpty(dr["SERVICO"].ToString()))
              dr["SERVICO"] = DBNull.Value;
            if (dr["LANC_DEB"] != null && string.IsNullOrEmpty(dr["LANC_DEB"].ToString()))
              dr["LANC_DEB"] = DBNull.Value;
            if (dr["QTD"] != null && string.IsNullOrEmpty(dr["QTD"].ToString()))
              dr["QTD"] = DBNull.Value;
            if (dr["OBS"] != null && string.IsNullOrEmpty(dr["OBS"].ToString()))
              dr["OBS"] = DBNull.Value;
            if (dr["ALUNO"] != null && string.IsNullOrEmpty(dr["ALUNO"].ToString()))
              dr["ALUNO"] = DBNull.Value;
            if (dr["NOME_COMPL"] != null && string.IsNullOrEmpty(dr["NOME_COMPL"].ToString()))
              dr["NOME_COMPL"] = DBNull.Value;
            if (dr["DESCRICAO"] != null && string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
              dr["DESCRICAO"] = DBNull.Value;
            if (dr["STATUS"] != null && string.IsNullOrEmpty(dr["STATUS"].ToString()))
              dr["STATUS"] = DBNull.Value;
            if (dr["PAGAMENTO_PRE_EXECUCAO"] != null && string.IsNullOrEmpty(dr["PAGAMENTO_PRE_EXECUCAO"].ToString()))
              dr["PAGAMENTO_PRE_EXECUCAO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["SOLICITACAO"] == null || string.IsNullOrEmpty(dr["SOLICITACAO"].ToString()))
            {
              return "Favor informar o Solicitacao.";
            }

            if (dr["SOLICITACAO"] != null && !string.IsNullOrEmpty(dr["SOLICITACAO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["SOLICITACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["SOLICITACAO"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["SOLICITACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["SOLICITACAO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Solicitacao deve ser um valor inteiro com 10 dígitos no máximo.";
              }
            }

            if (dr["ITEM_SOLICITACAO"] == null || string.IsNullOrEmpty(dr["ITEM_SOLICITACAO"].ToString()))
            {
              return "Favor informar o Item_solicitacao.";
            }

            if (dr["ITEM_SOLICITACAO"] != null && !string.IsNullOrEmpty(dr["ITEM_SOLICITACAO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ITEM_SOLICITACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ITEM_SOLICITACAO"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["ITEM_SOLICITACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ITEM_SOLICITACAO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Item_solicitacao deve ser um valor inteiro com 10 dígitos no máximo.";
              }
            }

            if (dr["SERVICO"] == null || string.IsNullOrEmpty(dr["SERVICO"].ToString()))
            {
              return "Favor informar o Servico.";
            }

            if (dr["SERVICO"] != null && !string.IsNullOrEmpty(dr["SERVICO"].ToString()))
            {
              if (dr["SERVICO"].ToString().Length > 20)
              {
                return "O Servico deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["LANC_DEB"] != null && !string.IsNullOrEmpty(dr["LANC_DEB"].ToString()))
            {
              if ((Convert.ToDecimal(dr["LANC_DEB"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["LANC_DEB"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["LANC_DEB"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["LANC_DEB"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Lanc_deb deve ser um valor inteiro com 10 dígitos no máximo.";
              }
            }

            if (dr["QTD"] == null || string.IsNullOrEmpty(dr["QTD"].ToString()))
            {
              return "Favor informar o Qtd.";
            }

            if (dr["QTD"] != null && !string.IsNullOrEmpty(dr["QTD"].ToString()))
            {
              if ((Convert.ToDecimal(dr["QTD"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["QTD"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["QTD"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["QTD"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Qtd deve ser um valor inteiro com 10 dígitos no máximo.";
              }
            }

            if (dr["OBS"] != null && !string.IsNullOrEmpty(dr["OBS"].ToString()))
            {
              if (dr["OBS"].ToString().Length > 2000)
              {
                return "O Obs deve possuir no máximo 2000 caracteres.";
              }
            }

            if (dr["ALUNO"] == null || string.IsNullOrEmpty(dr["ALUNO"].ToString()))
            {
              return "Favor informar o Aluno.";
            }

            if (dr["ALUNO"] != null && !string.IsNullOrEmpty(dr["ALUNO"].ToString()))
            {
              if (dr["ALUNO"].ToString().Length > 20)
              {
                return "O Aluno deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["NOME_COMPL"] == null || string.IsNullOrEmpty(dr["NOME_COMPL"].ToString()))
            {
              return "Favor informar o Nome_compl.";
            }

            if (dr["NOME_COMPL"] != null && !string.IsNullOrEmpty(dr["NOME_COMPL"].ToString()))
            {
              if (dr["NOME_COMPL"].ToString().Length > 100)
              {
                return "O Nome_compl deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["DATA"] == null || string.IsNullOrEmpty(dr["DATA"].ToString()))
            {
              return "Favor informar o Data.";
            }

            if (dr["DATA"] != null && dr["DATA"].ToString() != "")
            {
              dr["DATA"] = Convert.ToDateTime(dr["DATA"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DESCRICAO"] == null || string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              return "Favor informar o Descricao.";
            }

            if (dr["DESCRICAO"] != null && !string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              if (dr["DESCRICAO"].ToString().Length > 100)
              {
                return "O Descricao deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["STATUS"] != null && !string.IsNullOrEmpty(dr["STATUS"].ToString()))
            {
              if (dr["STATUS"].ToString().Length > 15)
              {
                return "O Status deve possuir no máximo 15 caracteres.";
              }
            }

            if (dr["PAGAMENTO_PRE_EXECUCAO"] != null && !string.IsNullOrEmpty(dr["PAGAMENTO_PRE_EXECUCAO"].ToString()))
            {
              if (dr["PAGAMENTO_PRE_EXECUCAO"].ToString().Length > 1)
              {
                return "O Pagamento_pre_execucao deve possuir no máximo 1 caractere.";
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

    public static Serv_solicitacao Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Serv_solicitacao Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Serv_solicitacao tab = new Serv_solicitacao();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Serv_solicitacao tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Find(
        decimal? @solicitacao, decimal? @item_solicitacao, string @servico, decimal? @lanc_deb, 
        decimal? @qtd, string @obs, string @aluno, string @nome_compl, DateTime? @data, string @descricao, 
        string @status, string @pagamento_pre_execucao
      ) {
        return (Row)Find(new object[] {
          @solicitacao, @item_solicitacao, @servico, @lanc_deb, @qtd, @obs, @aluno, @nome_compl, 
          @data, @descricao, @status, @pagamento_pre_execucao
        });
      }


      public new Row this[int index] { get { return (Row)base[index]; } }

      public void Remove(Row row) { base.Remove(row); }

    }

    public sealed class Row : TDataRow {
      internal Row(DataRowBuilder rb) : base(rb) {
      }


      /// <summary>
      /// SOLICITACAO: NUMERIC(10), Not Null
      /// </summary>
      public decimal? Solicitacao { get { return (decimal?)this["solicitacao"]; } set { this["solicitacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ITEM_SOLICITACAO: NUMERIC(10), Not Null
      /// </summary>
      public decimal? Item_solicitacao { get { return (decimal?)this["item_solicitacao"]; } set { this["item_solicitacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SERVICO: VARCHAR(20), Not Null
      /// </summary>
      public string Servico { get { return (string)this["servico"]; } set { this["servico"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// LANC_DEB: NUMERIC(10)
      /// </summary>
      public decimal? Lanc_deb { get { return (decimal?)this["lanc_deb"]; } set { this["lanc_deb"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// QTD: NUMERIC(10), Not Null
      /// </summary>
      public decimal? Qtd { get { return (decimal?)this["qtd"]; } set { this["qtd"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// OBS: VARCHAR(2000)
      /// </summary>
      public string Obs { get { return (string)this["obs"]; } set { this["obs"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ALUNO: VARCHAR(20), Not Null
      /// </summary>
      public string Aluno { get { return (string)this["aluno"]; } set { this["aluno"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME_COMPL: VARCHAR(100), Not Null
      /// </summary>
      public string Nome_compl { get { return (string)this["nome_compl"]; } set { this["nome_compl"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DATA: DATETIME, Not Null
      /// </summary>
      public DateTime? Data { get { return (DateTime?)this["data"]; } set { this["data"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO: VARCHAR(100), Not Null
      /// </summary>
      public string Descricao { get { return (string)this["descricao"]; } set { this["descricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// STATUS: VARCHAR(15)
      /// </summary>
      public string Status { get { return (string)this["status"]; } set { this["status"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PAGAMENTO_PRE_EXECUCAO: CHAR(1)
      /// </summary>
      public string Pagamento_pre_execucao { get { return (string)this["pagamento_pre_execucao"]; } set { this["pagamento_pre_execucao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }



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
    #region Coluna: SOLICITACAO
    private ColumnSolicitacao pvColSolicitacao;

    public sealed class ColumnSolicitacao : TDataColumn {

      public ColumnSolicitacao() {
        this.ColumnName = "solicitacao";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "serv_solicitacao.solicitacao"; } }
      public override string OriginalCol { get { return "solicitacao"; } }
      public override string RefCol { get { return "serv_solicitacao.SOLICITACAO"; } }
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

    #region Coluna: ITEM_SOLICITACAO
    private ColumnItem_solicitacao pvColItem_solicitacao;

    public sealed class ColumnItem_solicitacao : TDataColumn {

      public ColumnItem_solicitacao() {
        this.ColumnName = "item_solicitacao";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "serv_solicitacao.item_solicitacao"; } }
      public override string OriginalCol { get { return "item_solicitacao"; } }
      public override string RefCol { get { return "serv_solicitacao.ITEM_SOLICITACAO"; } }
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

    #region Coluna: SERVICO
    private ColumnServico pvColServico;

    public sealed class ColumnServico : TDataColumn {

      public ColumnServico() {
        this.ColumnName = "servico";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "serv_solicitacao.servico"; } }
      public override string OriginalCol { get { return "servico"; } }
      public override string RefCol { get { return "serv_solicitacao.SERVICO"; } }
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

    #region Coluna: LANC_DEB
    private ColumnLanc_deb pvColLanc_deb;

    public sealed class ColumnLanc_deb : TDataColumn {

      public ColumnLanc_deb() {
        this.ColumnName = "lanc_deb";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "serv_solicitacao.lanc_deb"; } }
      public override string OriginalCol { get { return "lanc_deb"; } }
      public override string RefCol { get { return "serv_solicitacao.LANC_DEB"; } }
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

    #region Coluna: QTD
    private ColumnQtd pvColQtd;

    public sealed class ColumnQtd : TDataColumn {

      public ColumnQtd() {
        this.ColumnName = "qtd";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "serv_solicitacao.qtd"; } }
      public override string OriginalCol { get { return "qtd"; } }
      public override string RefCol { get { return "serv_solicitacao.QTD"; } }
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

    #region Coluna: OBS
    private ColumnObs pvColObs;

    public sealed class ColumnObs : TDataColumn {

      public ColumnObs() {
        this.ColumnName = "obs";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "serv_solicitacao.obs"; } }
      public override string OriginalCol { get { return "obs"; } }
      public override string RefCol { get { return "serv_solicitacao.OBS"; } }
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

    #region Coluna: ALUNO
    private ColumnAluno pvColAluno;

    public sealed class ColumnAluno : TDataColumn {

      public ColumnAluno() {
        this.ColumnName = "aluno";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "serv_solicitacao.aluno"; } }
      public override string OriginalCol { get { return "aluno"; } }
      public override string RefCol { get { return "serv_solicitacao.ALUNO"; } }
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

    #region Coluna: NOME_COMPL
    private ColumnNome_compl pvColNome_compl;

    public sealed class ColumnNome_compl : TDataColumn {

      public ColumnNome_compl() {
        this.ColumnName = "nome_compl";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "serv_solicitacao.nome_compl"; } }
      public override string OriginalCol { get { return "nome_compl"; } }
      public override string RefCol { get { return "serv_solicitacao.NOME_COMPL"; } }
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

    #region Coluna: DATA
    private ColumnData pvColData;

    public sealed class ColumnData : TDataColumn {

      public ColumnData() {
        this.ColumnName = "data";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "serv_solicitacao.data"; } }
      public override string OriginalCol { get { return "data"; } }
      public override string RefCol { get { return "serv_solicitacao.DATA"; } }
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
      public override string FullCol { get { return "serv_solicitacao.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "serv_solicitacao.DESCRICAO"; } }
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

    #region Coluna: STATUS
    private ColumnStatus pvColStatus;

    public sealed class ColumnStatus : TDataColumn {

      public ColumnStatus() {
        this.ColumnName = "status";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "serv_solicitacao.status"; } }
      public override string OriginalCol { get { return "status"; } }
      public override string RefCol { get { return "serv_solicitacao.STATUS"; } }
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

    #region Coluna: PAGAMENTO_PRE_EXECUCAO
    private ColumnPagamento_pre_execucao pvColPagamento_pre_execucao;

    public sealed class ColumnPagamento_pre_execucao : TDataColumn {

      public ColumnPagamento_pre_execucao() {
        this.ColumnName = "pagamento_pre_execucao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "serv_solicitacao.pagamento_pre_execucao"; } }
      public override string OriginalCol { get { return "pagamento_pre_execucao"; } }
      public override string RefCol { get { return "serv_solicitacao.PAGAMENTO_PRE_EXECUCAO"; } }
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

  public partial class Vw_andamento_bt : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Vw_andamento_bt() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Vw_andamento_bt");
        ctorCustom = CRCustomLib.GetCustomConstructor("Vw_andamento_bt");
      }
      catch{}
    }

    public Vw_andamento_bt() {
      pvColNome_compl = new ColumnNome_compl(); Columns.Add(pvColNome_compl);
      pvColServico = new ColumnServico(); Columns.Add(pvColServico);
      pvColSolicitacao = new ColumnSolicitacao(); Columns.Add(pvColSolicitacao);
      pvColAluno = new ColumnAluno(); Columns.Add(pvColAluno);
      pvColCurso = new ColumnCurso(); Columns.Add(pvColCurso);
      pvColE_mail_interno = new ColumnE_mail_interno(); Columns.Add(pvColE_mail_interno);
      pvColTurno = new ColumnTurno(); Columns.Add(pvColTurno);
      pvColCurriculo = new ColumnCurriculo(); Columns.Add(pvColCurriculo);
      pvColSerie = new ColumnSerie(); Columns.Add(pvColSerie);
      pvColSit_aluno = new ColumnSit_aluno(); Columns.Add(pvColSit_aluno);
      pvColAndamento = new ColumnAndamento(); Columns.Add(pvColAndamento);

      InitRows(new RowCollection(BaseRows));
      SetTableName("vw_andamento_bt");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      throw new InvalidOperationException("O método NewTypedRow() é inválido para o DataTable VW_ANDAMENTO_BT.");
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
      get { return "VW_ANDAMENTO_BT"; }
    }

    protected override sealed string Name { get { return this.TableName; } }

    [Browsable(false)]
    public override sealed bool ReadOnly {
      get { return true; }
    }

    [Browsable(false)]
    public new RowCollection Rows { get { return (RowCollection)base.Rows; } }


    protected override sealed DataRow NewRowFromBuilder(DataRowBuilder builder) {
      return new Row(builder);
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
          if(lower == "nome_compl") { result.Add(toLower ? "nome_compl" : column); continue; }
          if(lower == "servico") { result.Add(toLower ? "servico" : column); continue; }
          if(lower == "solicitacao") { result.Add(toLower ? "solicitacao" : column); continue; }
          if(lower == "aluno") { result.Add(toLower ? "aluno" : column); continue; }
          if(lower == "curso") { result.Add(toLower ? "curso" : column); continue; }
          if(lower == "e_mail_interno") { result.Add(toLower ? "e_mail_interno" : column); continue; }
          if(lower == "turno") { result.Add(toLower ? "turno" : column); continue; }
          if(lower == "curriculo") { result.Add(toLower ? "curriculo" : column); continue; }
          if(lower == "serie") { result.Add(toLower ? "serie" : column); continue; }
          if(lower == "sit_aluno") { result.Add(toLower ? "sit_aluno" : column); continue; }
          if(lower == "andamento") { result.Add(toLower ? "andamento" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "nome_compl", "servico", "solicitacao", "aluno", "curso", "e_mail_interno", 
                                           "turno", "curriculo", "serie", "sit_aluno", "andamento" }
                          : new string[] { "NOME_COMPL", "SERVICO", "SOLICITACAO", "ALUNO", "CURSO", "E_MAIL_INTERNO", 
                                           "TURNO", "CURRICULO", "SERIE", "SIT_ALUNO", "ANDAMENTO" });
        else
          result.AddRange(toLower
                          ? new string[] { "nome_compl", "servico", "solicitacao", "aluno", "curso", "e_mail_interno", 
                                           "turno", "curriculo", "serie", "sit_aluno", "andamento" }
                          : new string[] { "NOME_COMPL", "SERVICO", "SOLICITACAO", "ALUNO", "CURSO", "E_MAIL_INTERNO", 
                                           "TURNO", "CURRICULO", "SERIE", "SIT_ALUNO", "ANDAMENTO" });

      return result;
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
        "vw_andamento_bt.nome_compl", "vw_andamento_bt.servico", "vw_andamento_bt.solicitacao", 
        "vw_andamento_bt.aluno", "vw_andamento_bt.curso", "vw_andamento_bt.e_mail_interno", 
        "vw_andamento_bt.turno", "vw_andamento_bt.curriculo", "vw_andamento_bt.serie", "vw_andamento_bt.sit_aluno", 
        "vw_andamento_bt.andamento"
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
          "FROM vw_andamento_bt " +
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
            count = da.Fill(ds, startRecord, maxRecords, "vw_andamento_bt");
            if(fakeds) ds.Tables.Remove(this);
          }
          catch(Exception exc) {
            throw new Exception(exc.Message + " " +
                                "Select statement: [" + select + "], " +
                                "Parameters: [" + StrLib.EnumerableToStr(whereValues) + "]");
          }
        }

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
      throw new InvalidOperationException("O método Put() é inválido para o DataTable VW_ANDAMENTO_BT");
    }

    public void ValidaConvEmptyStrToNull(ref DataRow dr)
    {
      if (dr != null)
      {
            if (dr["NOME_COMPL"] != null && string.IsNullOrEmpty(dr["NOME_COMPL"].ToString()))
              dr["NOME_COMPL"] = DBNull.Value;
            if (dr["SERVICO"] != null && string.IsNullOrEmpty(dr["SERVICO"].ToString()))
              dr["SERVICO"] = DBNull.Value;
            if (dr["SOLICITACAO"] != null && string.IsNullOrEmpty(dr["SOLICITACAO"].ToString()))
              dr["SOLICITACAO"] = DBNull.Value;
            if (dr["ALUNO"] != null && string.IsNullOrEmpty(dr["ALUNO"].ToString()))
              dr["ALUNO"] = DBNull.Value;
            if (dr["CURSO"] != null && string.IsNullOrEmpty(dr["CURSO"].ToString()))
              dr["CURSO"] = DBNull.Value;
            if (dr["E_MAIL_INTERNO"] != null && string.IsNullOrEmpty(dr["E_MAIL_INTERNO"].ToString()))
              dr["E_MAIL_INTERNO"] = DBNull.Value;
            if (dr["TURNO"] != null && string.IsNullOrEmpty(dr["TURNO"].ToString()))
              dr["TURNO"] = DBNull.Value;
            if (dr["CURRICULO"] != null && string.IsNullOrEmpty(dr["CURRICULO"].ToString()))
              dr["CURRICULO"] = DBNull.Value;
            if (dr["SERIE"] != null && string.IsNullOrEmpty(dr["SERIE"].ToString()))
              dr["SERIE"] = DBNull.Value;
            if (dr["SIT_ALUNO"] != null && string.IsNullOrEmpty(dr["SIT_ALUNO"].ToString()))
              dr["SIT_ALUNO"] = DBNull.Value;
            if (dr["ANDAMENTO"] != null && string.IsNullOrEmpty(dr["ANDAMENTO"].ToString()))
              dr["ANDAMENTO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["NOME_COMPL"] == null || string.IsNullOrEmpty(dr["NOME_COMPL"].ToString()))
            {
              return "Favor informar o Nome_compl.";
            }

            if (dr["NOME_COMPL"] != null && !string.IsNullOrEmpty(dr["NOME_COMPL"].ToString()))
            {
              if (dr["NOME_COMPL"].ToString().Length > 100)
              {
                return "O Nome_compl deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["SERVICO"] == null || string.IsNullOrEmpty(dr["SERVICO"].ToString()))
            {
              return "Favor informar o Servico.";
            }

            if (dr["SERVICO"] != null && !string.IsNullOrEmpty(dr["SERVICO"].ToString()))
            {
              if (dr["SERVICO"].ToString().Length > 20)
              {
                return "O Servico deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["SOLICITACAO"] == null || string.IsNullOrEmpty(dr["SOLICITACAO"].ToString()))
            {
              return "Favor informar o Solicitacao.";
            }

            if (dr["SOLICITACAO"] != null && !string.IsNullOrEmpty(dr["SOLICITACAO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["SOLICITACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["SOLICITACAO"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["SOLICITACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["SOLICITACAO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Solicitacao deve ser um valor inteiro com 10 dígitos no máximo.";
              }
            }

            if (dr["ALUNO"] == null || string.IsNullOrEmpty(dr["ALUNO"].ToString()))
            {
              return "Favor informar o Aluno.";
            }

            if (dr["ALUNO"] != null && !string.IsNullOrEmpty(dr["ALUNO"].ToString()))
            {
              if (dr["ALUNO"].ToString().Length > 20)
              {
                return "O Aluno deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["CURSO"] == null || string.IsNullOrEmpty(dr["CURSO"].ToString()))
            {
              return "Favor informar o Curso.";
            }

            if (dr["CURSO"] != null && !string.IsNullOrEmpty(dr["CURSO"].ToString()))
            {
              if (dr["CURSO"].ToString().Length > 20)
              {
                return "O Curso deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["E_MAIL_INTERNO"] != null && !string.IsNullOrEmpty(dr["E_MAIL_INTERNO"].ToString()))
            {
              if (dr["E_MAIL_INTERNO"].ToString().Length > 100)
              {
                return "O E_mail_interno deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["TURNO"] == null || string.IsNullOrEmpty(dr["TURNO"].ToString()))
            {
              return "Favor informar o Turno.";
            }

            if (dr["TURNO"] != null && !string.IsNullOrEmpty(dr["TURNO"].ToString()))
            {
              if (dr["TURNO"].ToString().Length > 20)
              {
                return "O Turno deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["CURRICULO"] == null || string.IsNullOrEmpty(dr["CURRICULO"].ToString()))
            {
              return "Favor informar o Curriculo.";
            }

            if (dr["CURRICULO"] != null && !string.IsNullOrEmpty(dr["CURRICULO"].ToString()))
            {
              if (dr["CURRICULO"].ToString().Length > 20)
              {
                return "O Curriculo deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["SERIE"] == null || string.IsNullOrEmpty(dr["SERIE"].ToString()))
            {
              return "Favor informar o Serie.";
            }

            if (dr["SERIE"] != null && !string.IsNullOrEmpty(dr["SERIE"].ToString()))
            {
              if ((Convert.ToDecimal(dr["SERIE"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["SERIE"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["SERIE"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["SERIE"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Serie deve ser um valor inteiro com 3 dígitos no máximo.";
              }
            }

            if (dr["SIT_ALUNO"] == null || string.IsNullOrEmpty(dr["SIT_ALUNO"].ToString()))
            {
              return "Favor informar o Sit_aluno.";
            }

            if (dr["SIT_ALUNO"] != null && !string.IsNullOrEmpty(dr["SIT_ALUNO"].ToString()))
            {
              if (dr["SIT_ALUNO"].ToString().Length > 15)
              {
                return "O Sit_aluno deve possuir no máximo 15 caracteres.";
              }
            }

            if (dr["ANDAMENTO"] == null || string.IsNullOrEmpty(dr["ANDAMENTO"].ToString()))
            {
              return "Favor informar o Andamento.";
            }

            if (dr["ANDAMENTO"] != null && !string.IsNullOrEmpty(dr["ANDAMENTO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ANDAMENTO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ANDAMENTO"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["ANDAMENTO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ANDAMENTO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Andamento deve ser um valor inteiro com 10 dígitos no máximo.";
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

    public static Vw_andamento_bt Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Vw_andamento_bt Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Vw_andamento_bt tab = new Vw_andamento_bt();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Vw_andamento_bt tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Find(
        string @nome_compl, string @servico, decimal? @solicitacao, string @aluno, string @curso, 
        string @e_mail_interno, string @turno, string @curriculo, decimal? @serie, string @sit_aluno, 
        decimal? @andamento
      ) {
        return (Row)Find(new object[] {
          @nome_compl, @servico, @solicitacao, @aluno, @curso, @e_mail_interno, @turno, @curriculo, 
          @serie, @sit_aluno, @andamento
        });
      }


      public new Row this[int index] { get { return (Row)base[index]; } }

      public void Remove(Row row) { base.Remove(row); }

    }

    public sealed class Row : TDataRow {
      internal Row(DataRowBuilder rb) : base(rb) {
      }


      /// <summary>
      /// NOME_COMPL: VARCHAR(100), Not Null
      /// </summary>
      public string Nome_compl { get { return (string)this["nome_compl"]; } set { this["nome_compl"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SERVICO: VARCHAR(20), Not Null
      /// </summary>
      public string Servico { get { return (string)this["servico"]; } set { this["servico"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SOLICITACAO: NUMERIC(10), Not Null
      /// </summary>
      public decimal? Solicitacao { get { return (decimal?)this["solicitacao"]; } set { this["solicitacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ALUNO: VARCHAR(20), Not Null
      /// </summary>
      public string Aluno { get { return (string)this["aluno"]; } set { this["aluno"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CURSO: VARCHAR(20), Not Null
      /// </summary>
      public string Curso { get { return (string)this["curso"]; } set { this["curso"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// E_MAIL_INTERNO: VARCHAR(100)
      /// </summary>
      public string E_mail_interno { get { return (string)this["e_mail_interno"]; } set { this["e_mail_interno"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TURNO: VARCHAR(20), Not Null
      /// </summary>
      public string Turno { get { return (string)this["turno"]; } set { this["turno"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CURRICULO: VARCHAR(20), Not Null
      /// </summary>
      public string Curriculo { get { return (string)this["curriculo"]; } set { this["curriculo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SERIE: NUMERIC(3), Not Null
      /// </summary>
      public decimal? Serie { get { return (decimal?)this["serie"]; } set { this["serie"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SIT_ALUNO: VARCHAR(15), Not Null
      /// </summary>
      public string Sit_aluno { get { return (string)this["sit_aluno"]; } set { this["sit_aluno"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ANDAMENTO: NUMERIC(10), Not Null
      /// </summary>
      public decimal? Andamento { get { return (decimal?)this["andamento"]; } set { this["andamento"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }



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
    #region Coluna: NOME_COMPL
    private ColumnNome_compl pvColNome_compl;

    public sealed class ColumnNome_compl : TDataColumn {

      public ColumnNome_compl() {
        this.ColumnName = "nome_compl";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "vw_andamento_bt.nome_compl"; } }
      public override string OriginalCol { get { return "nome_compl"; } }
      public override string RefCol { get { return "VW_ANDAMENTO_BT.NOME_COMPL"; } }
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

    #region Coluna: SERVICO
    private ColumnServico pvColServico;

    public sealed class ColumnServico : TDataColumn {

      public ColumnServico() {
        this.ColumnName = "servico";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "vw_andamento_bt.servico"; } }
      public override string OriginalCol { get { return "servico"; } }
      public override string RefCol { get { return "VW_ANDAMENTO_BT.SERVICO"; } }
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

    #region Coluna: SOLICITACAO
    private ColumnSolicitacao pvColSolicitacao;

    public sealed class ColumnSolicitacao : TDataColumn {

      public ColumnSolicitacao() {
        this.ColumnName = "solicitacao";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "vw_andamento_bt.solicitacao"; } }
      public override string OriginalCol { get { return "solicitacao"; } }
      public override string RefCol { get { return "VW_ANDAMENTO_BT.SOLICITACAO"; } }
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

    #region Coluna: ALUNO
    private ColumnAluno pvColAluno;

    public sealed class ColumnAluno : TDataColumn {

      public ColumnAluno() {
        this.ColumnName = "aluno";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "vw_andamento_bt.aluno"; } }
      public override string OriginalCol { get { return "aluno"; } }
      public override string RefCol { get { return "VW_ANDAMENTO_BT.ALUNO"; } }
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

    #region Coluna: CURSO
    private ColumnCurso pvColCurso;

    public sealed class ColumnCurso : TDataColumn {

      public ColumnCurso() {
        this.ColumnName = "curso";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "vw_andamento_bt.curso"; } }
      public override string OriginalCol { get { return "curso"; } }
      public override string RefCol { get { return "VW_ANDAMENTO_BT.CURSO"; } }
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

    #region Coluna: E_MAIL_INTERNO
    private ColumnE_mail_interno pvColE_mail_interno;

    public sealed class ColumnE_mail_interno : TDataColumn {

      public ColumnE_mail_interno() {
        this.ColumnName = "e_mail_interno";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "vw_andamento_bt.e_mail_interno"; } }
      public override string OriginalCol { get { return "e_mail_interno"; } }
      public override string RefCol { get { return "VW_ANDAMENTO_BT.E_MAIL_INTERNO"; } }
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

    #region Coluna: TURNO
    private ColumnTurno pvColTurno;

    public sealed class ColumnTurno : TDataColumn {

      public ColumnTurno() {
        this.ColumnName = "turno";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "vw_andamento_bt.turno"; } }
      public override string OriginalCol { get { return "turno"; } }
      public override string RefCol { get { return "VW_ANDAMENTO_BT.TURNO"; } }
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

    #region Coluna: CURRICULO
    private ColumnCurriculo pvColCurriculo;

    public sealed class ColumnCurriculo : TDataColumn {

      public ColumnCurriculo() {
        this.ColumnName = "curriculo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "vw_andamento_bt.curriculo"; } }
      public override string OriginalCol { get { return "curriculo"; } }
      public override string RefCol { get { return "VW_ANDAMENTO_BT.CURRICULO"; } }
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

    #region Coluna: SERIE
    private ColumnSerie pvColSerie;

    public sealed class ColumnSerie : TDataColumn {

      public ColumnSerie() {
        this.ColumnName = "serie";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "vw_andamento_bt.serie"; } }
      public override string OriginalCol { get { return "serie"; } }
      public override string RefCol { get { return "VW_ANDAMENTO_BT.SERIE"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 4; } }
      public override int Precision { get { return 3; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(3)"; } }

    }
    #endregion

    #region Coluna: SIT_ALUNO
    private ColumnSit_aluno pvColSit_aluno;

    public sealed class ColumnSit_aluno : TDataColumn {

      public ColumnSit_aluno() {
        this.ColumnName = "sit_aluno";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "vw_andamento_bt.sit_aluno"; } }
      public override string OriginalCol { get { return "sit_aluno"; } }
      public override string RefCol { get { return "VW_ANDAMENTO_BT.SIT_ALUNO"; } }
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

    #region Coluna: ANDAMENTO
    private ColumnAndamento pvColAndamento;

    public sealed class ColumnAndamento : TDataColumn {

      public ColumnAndamento() {
        this.ColumnName = "andamento";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "vw_andamento_bt.andamento"; } }
      public override string OriginalCol { get { return "andamento"; } }
      public override string RefCol { get { return "VW_ANDAMENTO_BT.ANDAMENTO"; } }
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
}

