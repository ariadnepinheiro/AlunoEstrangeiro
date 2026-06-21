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
  public class ProcessoSeletivo : TDataSet {
    private const string DataSetName_Def = "ProcessoSeletivo";
    private static ConstructorInfo ctorTCustom = null;
    private CustomBase tcustom = null;
    #region Variáveis private para as tabelas deste dataset
    private Ly_candidato_doc_contrato pvTabLy_candidato_doc_contrato;
    private Ly_candidato_doc_documentos pvTabLy_candidato_doc_documentos;
    private Ly_candidato_doc_experiencias pvTabLy_candidato_doc_experiencias;
    private Ly_candidato_doc_sit_avaliacao pvTabLy_candidato_doc_sit_avaliacao;
    private Ly_candidato_doc_solicitacoes pvTabLy_candidato_doc_solicitacoes;
    private Ly_candidato_doc_titulacoes pvTabLy_candidato_doc_titulacoes;
    private Ly_candidato_docente pvTabLy_candidato_docente;
    private Ly_concurso_doc_experiencia pvTabLy_concurso_doc_experiencia;
    private Ly_concurso_doc_habilitacao pvTabLy_concurso_doc_habilitacao;
    private Ly_concurso_doc_sit_avaliacao pvTabLy_concurso_doc_sit_avaliacao;
    private Ly_concurso_doc_titulacoes pvTabLy_concurso_doc_titulacoes;
    private Ly_concurso_docente pvTabLy_concurso_docente;
    private Ly_concurso_experiencia pvTabLy_concurso_experiencia;
    private Ly_concurso_titulacao pvTabLy_concurso_titulacao;
    #endregion

    static ProcessoSeletivo() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("ProcessoSeletivo");
      }
      catch{}
    }

    public ProcessoSeletivo() : base() {
      DataSetName = DataSetName_Def;

      pvTabLy_candidato_doc_contrato = new Ly_candidato_doc_contrato();
      pvTabLy_candidato_doc_documentos = new Ly_candidato_doc_documentos();
      pvTabLy_candidato_doc_experiencias = new Ly_candidato_doc_experiencias();
      pvTabLy_candidato_doc_sit_avaliacao = new Ly_candidato_doc_sit_avaliacao();
      pvTabLy_candidato_doc_solicitacoes = new Ly_candidato_doc_solicitacoes();
      pvTabLy_candidato_doc_titulacoes = new Ly_candidato_doc_titulacoes();
      pvTabLy_candidato_docente = new Ly_candidato_docente();
      pvTabLy_concurso_doc_experiencia = new Ly_concurso_doc_experiencia();
      pvTabLy_concurso_doc_habilitacao = new Ly_concurso_doc_habilitacao();
      pvTabLy_concurso_doc_sit_avaliacao = new Ly_concurso_doc_sit_avaliacao();
      pvTabLy_concurso_doc_titulacoes = new Ly_concurso_doc_titulacoes();
      pvTabLy_concurso_docente = new Ly_concurso_docente();
      pvTabLy_concurso_experiencia = new Ly_concurso_experiencia();
      pvTabLy_concurso_titulacao = new Ly_concurso_titulacao();

      Tables.Add(pvTabLy_candidato_doc_contrato);
      Tables.Add(pvTabLy_candidato_doc_documentos);
      Tables.Add(pvTabLy_candidato_doc_experiencias);
      Tables.Add(pvTabLy_candidato_doc_sit_avaliacao);
      Tables.Add(pvTabLy_candidato_doc_solicitacoes);
      Tables.Add(pvTabLy_candidato_doc_titulacoes);
      Tables.Add(pvTabLy_candidato_docente);
      Tables.Add(pvTabLy_concurso_doc_experiencia);
      Tables.Add(pvTabLy_concurso_doc_habilitacao);
      Tables.Add(pvTabLy_concurso_doc_sit_avaliacao);
      Tables.Add(pvTabLy_concurso_doc_titulacoes);
      Tables.Add(pvTabLy_concurso_docente);
      Tables.Add(pvTabLy_concurso_experiencia);
      Tables.Add(pvTabLy_concurso_titulacao);

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
          pvTabLy_candidato_doc_contrato.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_candidato_doc_contrato); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_candidato_doc_documentos.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_candidato_doc_documentos); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_candidato_doc_experiencias.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_candidato_doc_experiencias); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_candidato_doc_sit_avaliacao.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_candidato_doc_sit_avaliacao); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_candidato_doc_solicitacoes.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_candidato_doc_solicitacoes); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_candidato_doc_titulacoes.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_candidato_doc_titulacoes); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_candidato_docente.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_candidato_docente); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_concurso_doc_experiencia.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_concurso_doc_experiencia); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_concurso_doc_habilitacao.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_concurso_doc_habilitacao); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_concurso_doc_sit_avaliacao.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_concurso_doc_sit_avaliacao); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_concurso_doc_titulacoes.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_concurso_doc_titulacoes); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_concurso_docente.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_concurso_docente); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_concurso_experiencia.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_concurso_experiencia); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_concurso_titulacao.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_concurso_titulacao); if(row != null) return DataLib.RowErrorsToString(row);
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
    public Ly_candidato_doc_contrato Ly_candidato_doc_contrato { get { return pvTabLy_candidato_doc_contrato; } }

    [
      Browsable(false),
    ]
    public Ly_candidato_doc_documentos Ly_candidato_doc_documentos { get { return pvTabLy_candidato_doc_documentos; } }

    [
      Browsable(false),
    ]
    public Ly_candidato_doc_experiencias Ly_candidato_doc_experiencias { get { return pvTabLy_candidato_doc_experiencias; } }

    [
      Browsable(false),
    ]
    public Ly_candidato_doc_sit_avaliacao Ly_candidato_doc_sit_avaliacao { get { return pvTabLy_candidato_doc_sit_avaliacao; } }

    [
      Browsable(false),
    ]
    public Ly_candidato_doc_solicitacoes Ly_candidato_doc_solicitacoes { get { return pvTabLy_candidato_doc_solicitacoes; } }

    [
      Browsable(false),
    ]
    public Ly_candidato_doc_titulacoes Ly_candidato_doc_titulacoes { get { return pvTabLy_candidato_doc_titulacoes; } }

    [
      Browsable(false),
    ]
    public Ly_candidato_docente Ly_candidato_docente { get { return pvTabLy_candidato_docente; } }

    [
      Browsable(false),
    ]
    public Ly_concurso_doc_experiencia Ly_concurso_doc_experiencia { get { return pvTabLy_concurso_doc_experiencia; } }

    [
      Browsable(false),
    ]
    public Ly_concurso_doc_habilitacao Ly_concurso_doc_habilitacao { get { return pvTabLy_concurso_doc_habilitacao; } }

    [
      Browsable(false),
    ]
    public Ly_concurso_doc_sit_avaliacao Ly_concurso_doc_sit_avaliacao { get { return pvTabLy_concurso_doc_sit_avaliacao; } }

    [
      Browsable(false),
    ]
    public Ly_concurso_doc_titulacoes Ly_concurso_doc_titulacoes { get { return pvTabLy_concurso_doc_titulacoes; } }

    [
      Browsable(false),
    ]
    public Ly_concurso_docente Ly_concurso_docente { get { return pvTabLy_concurso_docente; } }

    [
      Browsable(false),
    ]
    public Ly_concurso_experiencia Ly_concurso_experiencia { get { return pvTabLy_concurso_experiencia; } }

    [
      Browsable(false),
    ]
    public Ly_concurso_titulacao Ly_concurso_titulacao { get { return pvTabLy_concurso_titulacao; } }

    #endregion

    public abstract class CustomBase {
      public abstract string Put(Techne.Lyceum.CR.ProcessoSeletivo ds, TConnectionWritable connection);
    }

  }


  public partial class Ly_candidato_doc_contrato : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_candidato_doc_contrato() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_candidato_doc_contrato");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_candidato_doc_contrato");
      }
      catch{}
    }

    public Ly_candidato_doc_contrato() {
      pvColConcurso = new ColumnConcurso(); Columns.Add(pvColConcurso);
      pvColCandidato = new ColumnCandidato(); Columns.Add(pvColCandidato);
      pvColStatus = new ColumnStatus(); Columns.Add(pvColStatus);
      pvColDt_inicio_contrato = new ColumnDt_inicio_contrato(); Columns.Add(pvColDt_inicio_contrato);
      pvColDt_fim_contrato = new ColumnDt_fim_contrato(); Columns.Add(pvColDt_fim_contrato);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_candidato_doc_contrato");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @concurso, string @candidato, string @status) {
      Row row = NewRow();

      row.Concurso = @concurso;
      row.Candidato = @candidato;
      row.Status = @status;

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
      get { return "LY_CANDIDATO_DOC_CONTRATO"; }
    }

    protected override sealed string Name { get { return "Contrato do candidato a docente"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColConcurso, pvColCandidato });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_candidato_doc_contra_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_candidato_doc_contra_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_candidato_doc_contra_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_candidato_doc_contra_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkConcurso", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "CONCURSO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkCandidato", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "CANDIDATO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("concurso", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CONCURSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("candidato", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CANDIDATO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("status", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "STATUS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("dt_inicio_contrato", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_INICIO_CONTRATO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_fim_contrato", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_FIM_CONTRATO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));

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
          if(lower == "concurso") { result.Add(toLower ? "concurso" : column); continue; }
          if(lower == "candidato") { result.Add(toLower ? "candidato" : column); continue; }
          if(lower == "status") { result.Add(toLower ? "status" : column); continue; }
          if(lower == "dt_inicio_contrato") { result.Add(toLower ? "dt_inicio_contrato" : column); continue; }
          if(lower == "dt_fim_contrato") { result.Add(toLower ? "dt_fim_contrato" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "concurso", "candidato", "status", "dt_inicio_contrato", "dt_fim_contrato" }
                          : new string[] { "CONCURSO", "CANDIDATO", "STATUS", "DT_INICIO_CONTRATO", "DT_FIM_CONTRATO" });
        else
          result.AddRange(toLower
                          ? new string[] { "concurso", "candidato", "status", "dt_inicio_contrato", "dt_fim_contrato" }
                          : new string[] { "CONCURSO", "CANDIDATO", "STATUS", "DT_INICIO_CONTRATO", "DT_FIM_CONTRATO" });

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
        "ly_candidato_doc_contrato.concurso", "ly_candidato_doc_contrato.candidato"
      });
      if(scColumns.Contains("status")) list.Add("ly_candidato_doc_contrato.status");
      if(scColumns.Contains("dt_inicio_contrato")) list.Add("ly_candidato_doc_contrato.dt_inicio_contrato");
      if(scColumns.Contains("dt_fim_contrato")) list.Add("ly_candidato_doc_contrato.dt_fim_contrato");
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
          "FROM ly_candidato_doc_contrato " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_candidato_doc_contrato");
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
        da.TableMappings.Add("Table", "Ly_candidato_doc_contrato");
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
            if (dr["CONCURSO"] != null && string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
              dr["CONCURSO"] = DBNull.Value;
            if (dr["CANDIDATO"] != null && string.IsNullOrEmpty(dr["CANDIDATO"].ToString()))
              dr["CANDIDATO"] = DBNull.Value;
            if (dr["STATUS"] != null && string.IsNullOrEmpty(dr["STATUS"].ToString()))
              dr["STATUS"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["CONCURSO"] == null || string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
            {
              return "Favor informar o concurso.";
            }

            if (dr["CONCURSO"] != null && !string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
            {
              if (dr["CONCURSO"].ToString().Length > 20)
              {
                return "O concurso deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["CANDIDATO"] == null || string.IsNullOrEmpty(dr["CANDIDATO"].ToString()))
            {
              return "Favor informar o candidato.";
            }

            if (dr["CANDIDATO"] != null && !string.IsNullOrEmpty(dr["CANDIDATO"].ToString()))
            {
              if (dr["CANDIDATO"].ToString().Length > 20)
              {
                return "O candidato deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["STATUS"] == null || string.IsNullOrEmpty(dr["STATUS"].ToString()))
            {
              return "Favor informar o status.";
            }

            if (dr["STATUS"] != null && !string.IsNullOrEmpty(dr["STATUS"].ToString()))
            {
              if (dr["STATUS"].ToString().Length > 50)
              {
                return "O status deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["DT_INICIO_CONTRATO"] != null && dr["DT_INICIO_CONTRATO"].ToString() != "")
            {
              dr["DT_INICIO_CONTRATO"] = Convert.ToDateTime(dr["DT_INICIO_CONTRATO"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_FIM_CONTRATO"] != null && dr["DT_FIM_CONTRATO"].ToString() != "")
            {
              dr["DT_FIM_CONTRATO"] = Convert.ToDateTime(dr["DT_FIM_CONTRATO"]).ToString("dd/MM/yyyy HH:mm:ss");
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

    public static Ly_candidato_doc_contrato Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_candidato_doc_contrato Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_candidato_doc_contrato tab = new Ly_candidato_doc_contrato();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_candidato_doc_contrato tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @concurso, string @candidato, string @status, DateTime? @dt_inicio_contrato, 
        DateTime? @dt_fim_contrato
      ) {
        return (Row)Add(new object[] {
          @concurso, @candidato, @status, @dt_inicio_contrato, @dt_fim_contrato
        });
      }

      public Row Find(
        string @concurso, string @candidato
      ) {
        return (Row)Find(new object[] {
          @concurso, @candidato
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
      /// CONCURSO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Concurso { get { return (string)this["concurso"]; } set { this["concurso"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CANDIDATO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Candidato { get { return (string)this["candidato"]; } set { this["candidato"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// STATUS: VARCHAR(50), Not Null
      /// </summary>
      public string Status { get { return (string)this["status"]; } set { this["status"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_INICIO_CONTRATO: DATETIME
      /// </summary>
      public DateTime? Dt_inicio_contrato { get { return (DateTime?)this["dt_inicio_contrato"]; } set { this["dt_inicio_contrato"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_FIM_CONTRATO: DATETIME
      /// </summary>
      public DateTime? Dt_fim_contrato { get { return (DateTime?)this["dt_fim_contrato"]; } set { this["dt_fim_contrato"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @concurso, string @candidato, string @status) {
        return Insert(connection,
                      @concurso, @candidato, @status,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @concurso, string @candidato, string @status,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_candidato_doc_contrato(),
          new string[] { "concurso", "candidato", "status" },
          new object[] { @concurso, @candidato, @status },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkConcurso, string pkCandidato,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_candidato_doc_contrato(),
          new string[] { "concurso", "candidato" },
          new object[] { pkConcurso, pkCandidato },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkConcurso, string pkCandidato) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_candidato_doc_contrato(),
          new string[] { "concurso", "candidato" },
          new object[] { pkConcurso, pkCandidato }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkConcurso, string pkCandidato) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_candidato_doc_contrato(),
          new string[] { "concurso", "candidato" },
          new object[] { pkConcurso, pkCandidato }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Concurso { get { return (string)row["concurso", DataRowVersion.Original]; } }
        public string Candidato { get { return (string)row["candidato", DataRowVersion.Original]; } }
        public string Status { get { return (string)row["status", DataRowVersion.Original]; } }
        public DateTime? Dt_inicio_contrato { get { return (DateTime?)row["dt_inicio_contrato", DataRowVersion.Original]; } }
        public DateTime? Dt_fim_contrato { get { return (DateTime?)row["dt_fim_contrato", DataRowVersion.Original]; } }

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
    #region Coluna: CONCURSO
    private ColumnConcurso pvColConcurso;

    public sealed class ColumnConcurso : TDataColumn {

      public ColumnConcurso() {
        this.ColumnName = "concurso";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "concurso";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_contrato.concurso"; } }
      public override string OriginalCol { get { return "concurso"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_CONTRATO.CONCURSO"; } }
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

    #region Coluna: CANDIDATO
    private ColumnCandidato pvColCandidato;

    public sealed class ColumnCandidato : TDataColumn {

      public ColumnCandidato() {
        this.ColumnName = "candidato";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "candidato";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_contrato.candidato"; } }
      public override string OriginalCol { get { return "candidato"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_CONTRATO.CANDIDATO"; } }
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

    #region Coluna: STATUS
    private ColumnStatus pvColStatus;

    public sealed class ColumnStatus : TDataColumn {

      public ColumnStatus() {
        this.ColumnName = "status";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "status";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_contrato.status"; } }
      public override string OriginalCol { get { return "status"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_CONTRATO.STATUS"; } }
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

    #region Coluna: DT_INICIO_CONTRATO
    private ColumnDt_inicio_contrato pvColDt_inicio_contrato;

    public sealed class ColumnDt_inicio_contrato : TDataColumn {

      public ColumnDt_inicio_contrato() {
        this.ColumnName = "dt_inicio_contrato";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data inicial do contrato";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_contrato.dt_inicio_contrato"; } }
      public override string OriginalCol { get { return "dt_inicio_contrato"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_CONTRATO.DT_INICIO_CONTRATO"; } }
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

    #region Coluna: DT_FIM_CONTRATO
    private ColumnDt_fim_contrato pvColDt_fim_contrato;

    public sealed class ColumnDt_fim_contrato : TDataColumn {

      public ColumnDt_fim_contrato() {
        this.ColumnName = "dt_fim_contrato";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data final do contrato";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_contrato.dt_fim_contrato"; } }
      public override string OriginalCol { get { return "dt_fim_contrato"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_CONTRATO.DT_FIM_CONTRATO"; } }
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

  public partial class Ly_candidato_doc_documentos : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_candidato_doc_documentos() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_candidato_doc_documentos");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_candidato_doc_documentos");
      }
      catch{}
    }

    public Ly_candidato_doc_documentos() {
      pvColId_candidato_doc_documentos = new ColumnId_candidato_doc_documentos(); Columns.Add(pvColId_candidato_doc_documentos);
      pvColConcurso = new ColumnConcurso(); Columns.Add(pvColConcurso);
      pvColCandidato = new ColumnCandidato(); Columns.Add(pvColCandidato);
      pvColData = new ColumnData(); Columns.Add(pvColData);
      pvColStatus = new ColumnStatus(); Columns.Add(pvColStatus);
      pvColTipo = new ColumnTipo(); Columns.Add(pvColTipo);
      pvColCarga_horaria_atual = new ColumnCarga_horaria_atual(); Columns.Add(pvColCarga_horaria_atual);
      pvColCarga_horaria_nova = new ColumnCarga_horaria_nova(); Columns.Add(pvColCarga_horaria_nova);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_candidato_doc_documentos");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @concurso, string @candidato, DateTime? @data, string @status) {
      Row row = NewRow();

      row.Concurso = @concurso;
      row.Candidato = @candidato;
      row.Data = @data;
      row.Status = @status;

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
      get { return "LY_CANDIDATO_DOC_DOCUMENTOS"; }
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
      SetPrimaryKey(new DataColumn[] { pvColId_candidato_doc_documentos });
      pvColId_candidato_doc_documentos.AllowDBNull = true;
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_candidato_doc_docume_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_candidato_doc_docume_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_candidato_doc_docume_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_candidato_doc_docume_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId_candidato_doc_documentos", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID_CANDIDATO_DOC_DOCUMENTOS", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id_candidato_doc_documentos", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ID_CANDIDATO_DOC_DOCUMENTOS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("concurso", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CONCURSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("candidato", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CANDIDATO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("data", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DATA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("status", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "STATUS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("tipo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TIPO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("carga_horaria_atual", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "CARGA_HORARIA_ATUAL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("carga_horaria_nova", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "CARGA_HORARIA_NOVA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "id_candidato_doc_documentos") { result.Add(toLower ? "id_candidato_doc_documentos" : column); continue; }
          if(lower == "concurso") { result.Add(toLower ? "concurso" : column); continue; }
          if(lower == "candidato") { result.Add(toLower ? "candidato" : column); continue; }
          if(lower == "data") { result.Add(toLower ? "data" : column); continue; }
          if(lower == "status") { result.Add(toLower ? "status" : column); continue; }
          if(lower == "tipo") { result.Add(toLower ? "tipo" : column); continue; }
          if(lower == "carga_horaria_atual") { result.Add(toLower ? "carga_horaria_atual" : column); continue; }
          if(lower == "carga_horaria_nova") { result.Add(toLower ? "carga_horaria_nova" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id_candidato_doc_documentos", "concurso", "candidato", "data", 
                                           "status", "tipo", "carga_horaria_atual", "carga_horaria_nova" }
                          : new string[] { "ID_CANDIDATO_DOC_DOCUMENTOS", "CONCURSO", "CANDIDATO", "DATA", 
                                           "STATUS", "TIPO", "CARGA_HORARIA_ATUAL", "CARGA_HORARIA_NOVA" });
        else
          result.AddRange(toLower
                          ? new string[] { "id_candidato_doc_documentos", "concurso", "candidato", "data", 
                                           "status", "tipo", "carga_horaria_atual", "carga_horaria_nova" }
                          : new string[] { "ID_CANDIDATO_DOC_DOCUMENTOS", "CONCURSO", "CANDIDATO", "DATA", 
                                           "STATUS", "TIPO", "CARGA_HORARIA_ATUAL", "CARGA_HORARIA_NOVA" });

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
        "ly_candidato_doc_documentos.id_candidato_doc_documentos"
      });
      if(scColumns.Contains("concurso")) list.Add("ly_candidato_doc_documentos.concurso");
      if(scColumns.Contains("candidato")) list.Add("ly_candidato_doc_documentos.candidato");
      if(scColumns.Contains("data")) list.Add("ly_candidato_doc_documentos.data");
      if(scColumns.Contains("status")) list.Add("ly_candidato_doc_documentos.status");
      if(scColumns.Contains("tipo")) list.Add("ly_candidato_doc_documentos.tipo");
      if(scColumns.Contains("carga_horaria_atual")) list.Add("ly_candidato_doc_documentos.carga_horaria_atual");
      if(scColumns.Contains("carga_horaria_nova")) list.Add("ly_candidato_doc_documentos.carga_horaria_nova");
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
          "FROM ly_candidato_doc_documentos " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_candidato_doc_documentos");
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
        da.TableMappings.Add("Table", "Ly_candidato_doc_documentos");
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
            if (dr["CONCURSO"] != null && string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
              dr["CONCURSO"] = DBNull.Value;
            if (dr["CANDIDATO"] != null && string.IsNullOrEmpty(dr["CANDIDATO"].ToString()))
              dr["CANDIDATO"] = DBNull.Value;
            if (dr["STATUS"] != null && string.IsNullOrEmpty(dr["STATUS"].ToString()))
              dr["STATUS"] = DBNull.Value;
            if (dr["TIPO"] != null && string.IsNullOrEmpty(dr["TIPO"].ToString()))
              dr["TIPO"] = DBNull.Value;
            if (dr["CARGA_HORARIA_ATUAL"] != null && string.IsNullOrEmpty(dr["CARGA_HORARIA_ATUAL"].ToString()))
              dr["CARGA_HORARIA_ATUAL"] = DBNull.Value;
            if (dr["CARGA_HORARIA_NOVA"] != null && string.IsNullOrEmpty(dr["CARGA_HORARIA_NOVA"].ToString()))
              dr["CARGA_HORARIA_NOVA"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["CONCURSO"] == null || string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
            {
              return "Favor informar o Concurso.";
            }

            if (dr["CONCURSO"] != null && !string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
            {
              if (dr["CONCURSO"].ToString().Length > 20)
              {
                return "O Concurso deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["CANDIDATO"] == null || string.IsNullOrEmpty(dr["CANDIDATO"].ToString()))
            {
              return "Favor informar o Candidato.";
            }

            if (dr["CANDIDATO"] != null && !string.IsNullOrEmpty(dr["CANDIDATO"].ToString()))
            {
              if (dr["CANDIDATO"].ToString().Length > 20)
              {
                return "O Candidato deve possuir no máximo 20 caracteres.";
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

            if (dr["STATUS"] == null || string.IsNullOrEmpty(dr["STATUS"].ToString()))
            {
              return "Favor informar o Status.";
            }

            if (dr["STATUS"] != null && !string.IsNullOrEmpty(dr["STATUS"].ToString()))
            {
              if (dr["STATUS"].ToString().Length > 40)
              {
                return "O Status deve possuir no máximo 40 caracteres.";
              }
            }

            if (dr["TIPO"] != null && !string.IsNullOrEmpty(dr["TIPO"].ToString()))
            {
              if (dr["TIPO"].ToString().Length > 100)
              {
                return "O Tipo deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["CARGA_HORARIA_ATUAL"] != null && !string.IsNullOrEmpty(dr["CARGA_HORARIA_ATUAL"].ToString()))
            {
              if ((Convert.ToDecimal(dr["CARGA_HORARIA_ATUAL"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["CARGA_HORARIA_ATUAL"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["CARGA_HORARIA_ATUAL"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["CARGA_HORARIA_ATUAL"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Carga_horaria_atual deve ser um valor inteiro com 3 dígitos no máximo.";
              }
            }

            if (dr["CARGA_HORARIA_NOVA"] != null && !string.IsNullOrEmpty(dr["CARGA_HORARIA_NOVA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["CARGA_HORARIA_NOVA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["CARGA_HORARIA_NOVA"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["CARGA_HORARIA_NOVA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["CARGA_HORARIA_NOVA"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Carga_horaria_nova deve ser um valor inteiro com 3 dígitos no máximo.";
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

    public static Ly_candidato_doc_documentos Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_candidato_doc_documentos Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_candidato_doc_documentos tab = new Ly_candidato_doc_documentos();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_candidato_doc_documentos tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id_candidato_doc_documentos, string @concurso, string @candidato, DateTime? @data, 
        string @status, string @tipo, decimal? @carga_horaria_atual, decimal? @carga_horaria_nova
      ) {
        return (Row)Add(new object[] {
          @id_candidato_doc_documentos, @concurso, @candidato, @data, @status, @tipo, @carga_horaria_atual, 
          @carga_horaria_nova
        });
      }

      public Row Find(
        decimal? @id_candidato_doc_documentos
      ) {
        return (Row)Find(new object[] {
          @id_candidato_doc_documentos
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
      /// ID_CANDIDATO_DOC_DOCUMENTOS: NUMERIC(10), Primary Key, AutoNumber, Not Null
      /// </summary>
      public decimal? Id_candidato_doc_documentos { get { return (decimal?)this["id_candidato_doc_documentos"]; } set { this["id_candidato_doc_documentos"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CONCURSO: VARCHAR(20), Not Null
      /// </summary>
      public string Concurso { get { return (string)this["concurso"]; } set { this["concurso"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CANDIDATO: VARCHAR(20), Not Null
      /// </summary>
      public string Candidato { get { return (string)this["candidato"]; } set { this["candidato"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DATA: DATETIME, Not Null
      /// </summary>
      public DateTime? Data { get { return (DateTime?)this["data"]; } set { this["data"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// STATUS: VARCHAR(40), Not Null
      /// </summary>
      public string Status { get { return (string)this["status"]; } set { this["status"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TIPO: VARCHAR(100)
      /// </summary>
      public string Tipo { get { return (string)this["tipo"]; } set { this["tipo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CARGA_HORARIA_ATUAL: NUMERIC(3)
      /// </summary>
      public decimal? Carga_horaria_atual { get { return (decimal?)this["carga_horaria_atual"]; } set { this["carga_horaria_atual"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CARGA_HORARIA_NOVA: NUMERIC(3)
      /// </summary>
      public decimal? Carga_horaria_nova { get { return (decimal?)this["carga_horaria_nova"]; } set { this["carga_horaria_nova"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @concurso, string @candidato, DateTime? @data, string @status) {
        return Insert(connection,
                      @concurso, @candidato, @data, @status,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @concurso, string @candidato, DateTime? @data, string @status,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_candidato_doc_documentos(),
          new string[] { "concurso", "candidato", "data", "status" },
          new object[] { @concurso, @candidato, @data, @status },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId_candidato_doc_documentos,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_candidato_doc_documentos(),
          new string[] { "id_candidato_doc_documentos" },
          new object[] { pkId_candidato_doc_documentos },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId_candidato_doc_documentos) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_candidato_doc_documentos(),
          new string[] { "id_candidato_doc_documentos" },
          new object[] { pkId_candidato_doc_documentos }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              decimal? pkId_candidato_doc_documentos) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_candidato_doc_documentos(),
          new string[] { "id_candidato_doc_documentos" },
          new object[] { pkId_candidato_doc_documentos }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public decimal? Id_candidato_doc_documentos { get { return (decimal?)row["id_candidato_doc_documentos", DataRowVersion.Original]; } }
        public string Concurso { get { return (string)row["concurso", DataRowVersion.Original]; } }
        public string Candidato { get { return (string)row["candidato", DataRowVersion.Original]; } }
        public DateTime? Data { get { return (DateTime?)row["data", DataRowVersion.Original]; } }
        public string Status { get { return (string)row["status", DataRowVersion.Original]; } }
        public string Tipo { get { return (string)row["tipo", DataRowVersion.Original]; } }
        public decimal? Carga_horaria_atual { get { return (decimal?)row["carga_horaria_atual", DataRowVersion.Original]; } }
        public decimal? Carga_horaria_nova { get { return (decimal?)row["carga_horaria_nova", DataRowVersion.Original]; } }

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
    #region Coluna: ID_CANDIDATO_DOC_DOCUMENTOS
    private ColumnId_candidato_doc_documentos pvColId_candidato_doc_documentos;

    public sealed class ColumnId_candidato_doc_documentos : TDataColumn {

      public ColumnId_candidato_doc_documentos() {
        this.ColumnName = "id_candidato_doc_documentos";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_documentos.id_candidato_doc_documentos"; } }
      public override string OriginalCol { get { return "id_candidato_doc_documentos"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_DOCUMENTOS.ID_CANDIDATO_DOC_DOCUMENTOS"; } }
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

    #region Coluna: CONCURSO
    private ColumnConcurso pvColConcurso;

    public sealed class ColumnConcurso : TDataColumn {

      public ColumnConcurso() {
        this.ColumnName = "concurso";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_documentos.concurso"; } }
      public override string OriginalCol { get { return "concurso"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_DOCUMENTOS.CONCURSO"; } }
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

    #region Coluna: CANDIDATO
    private ColumnCandidato pvColCandidato;

    public sealed class ColumnCandidato : TDataColumn {

      public ColumnCandidato() {
        this.ColumnName = "candidato";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_documentos.candidato"; } }
      public override string OriginalCol { get { return "candidato"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_DOCUMENTOS.CANDIDATO"; } }
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
      public override string FullCol { get { return "ly_candidato_doc_documentos.data"; } }
      public override string OriginalCol { get { return "data"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_DOCUMENTOS.DATA"; } }
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


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_documentos.status"; } }
      public override string OriginalCol { get { return "status"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_DOCUMENTOS.STATUS"; } }
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
      public override string FullCol { get { return "ly_candidato_doc_documentos.tipo"; } }
      public override string OriginalCol { get { return "tipo"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_DOCUMENTOS.TIPO"; } }
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

    #region Coluna: CARGA_HORARIA_ATUAL
    private ColumnCarga_horaria_atual pvColCarga_horaria_atual;

    public sealed class ColumnCarga_horaria_atual : TDataColumn {

      public ColumnCarga_horaria_atual() {
        this.ColumnName = "carga_horaria_atual";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_documentos.carga_horaria_atual"; } }
      public override string OriginalCol { get { return "carga_horaria_atual"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_DOCUMENTOS.CARGA_HORARIA_ATUAL"; } }
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

    #region Coluna: CARGA_HORARIA_NOVA
    private ColumnCarga_horaria_nova pvColCarga_horaria_nova;

    public sealed class ColumnCarga_horaria_nova : TDataColumn {

      public ColumnCarga_horaria_nova() {
        this.ColumnName = "carga_horaria_nova";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_documentos.carga_horaria_nova"; } }
      public override string OriginalCol { get { return "carga_horaria_nova"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_DOCUMENTOS.CARGA_HORARIA_NOVA"; } }
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

  public partial class Ly_candidato_doc_experiencias : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_candidato_doc_experiencias() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_candidato_doc_experiencias");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_candidato_doc_experiencias");
      }
      catch{}
    }

    public Ly_candidato_doc_experiencias() {
      pvColConcurso = new ColumnConcurso(); Columns.Add(pvColConcurso);
      pvColCandidato = new ColumnCandidato(); Columns.Add(pvColCandidato);
      pvColExperiencia = new ColumnExperiencia(); Columns.Add(pvColExperiencia);
      pvColPontuacao = new ColumnPontuacao(); Columns.Add(pvColPontuacao);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_candidato_doc_experiencias");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @concurso, string @candidato, string @experiencia) {
      Row row = NewRow();

      row.Concurso = @concurso;
      row.Candidato = @candidato;
      row.Experiencia = @experiencia;

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
      get { return "LY_CANDIDATO_DOC_EXPERIENCIAS"; }
    }

    protected override sealed string Name { get { return "Experiência do candidato a docente"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColConcurso, pvColCandidato, pvColExperiencia });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_candidato_doc_experi_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_candidato_doc_experi_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_candidato_doc_experi_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_candidato_doc_experi_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkConcurso", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "CONCURSO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkCandidato", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "CANDIDATO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkExperiencia", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "EXPERIENCIA", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("concurso", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CONCURSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("candidato", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CANDIDATO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("experiencia", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "EXPERIENCIA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "concurso") { result.Add(toLower ? "concurso" : column); continue; }
          if(lower == "candidato") { result.Add(toLower ? "candidato" : column); continue; }
          if(lower == "experiencia") { result.Add(toLower ? "experiencia" : column); continue; }
          if(!tabPrincipal) {
            if(lower == "pontuacao") { result.Add(toLower ? "pontuacao" : column); continue; }
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "concurso", "candidato", "experiencia" }
                          : new string[] { "CONCURSO", "CANDIDATO", "EXPERIENCIA" });
        else
          result.AddRange(toLower
                          ? new string[] { "concurso", "candidato", "experiencia", "pontuacao" }
                          : new string[] { "CONCURSO", "CANDIDATO", "EXPERIENCIA", "PONTUACAO" });

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
        "ly_candidato_doc_experiencias.concurso", "ly_candidato_doc_experiencias.candidato", 
        "ly_candidato_doc_experiencias.experiencia"
      });
      if(scColumns.Contains("pontuacao")) list.Add("ly_concurso_doc_experiencia.pontuacao");
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
          "FROM ly_candidato_doc_experiencias " +
               "INNER JOIN ly_concurso_doc_experiencia " +
                 "ON ly_concurso_doc_experiencia.concurso = ly_candidato_doc_experiencias.concurso AND " +
                    "ly_concurso_doc_experiencia.experiencia = ly_candidato_doc_experiencias.experiencia " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_candidato_doc_experiencias");
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
        da.TableMappings.Add("Table", "Ly_candidato_doc_experiencias");
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
            if (dr["CONCURSO"] != null && string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
              dr["CONCURSO"] = DBNull.Value;
            if (dr["CANDIDATO"] != null && string.IsNullOrEmpty(dr["CANDIDATO"].ToString()))
              dr["CANDIDATO"] = DBNull.Value;
            if (dr["EXPERIENCIA"] != null && string.IsNullOrEmpty(dr["EXPERIENCIA"].ToString()))
              dr["EXPERIENCIA"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["CONCURSO"] == null || string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
            {
              return "Favor informar o concurso.";
            }

            if (dr["CONCURSO"] != null && !string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
            {
              if (dr["CONCURSO"].ToString().Length > 20)
              {
                return "O concurso deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["CANDIDATO"] == null || string.IsNullOrEmpty(dr["CANDIDATO"].ToString()))
            {
              return "Favor informar o candidato.";
            }

            if (dr["CANDIDATO"] != null && !string.IsNullOrEmpty(dr["CANDIDATO"].ToString()))
            {
              if (dr["CANDIDATO"].ToString().Length > 20)
              {
                return "O candidato deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["EXPERIENCIA"] == null || string.IsNullOrEmpty(dr["EXPERIENCIA"].ToString()))
            {
              return "Favor informar a experiência.";
            }

            if (dr["EXPERIENCIA"] != null && !string.IsNullOrEmpty(dr["EXPERIENCIA"].ToString()))
            {
              if (dr["EXPERIENCIA"].ToString().Length > 20)
              {
                return "A experiência deve possuir no máximo 20 caracteres.";
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

    public static Ly_candidato_doc_experiencias Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_candidato_doc_experiencias Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_candidato_doc_experiencias tab = new Ly_candidato_doc_experiencias();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_candidato_doc_experiencias tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @concurso, string @candidato, string @experiencia
      ) {
        return (Row)Add(new object[] {
          @concurso, @candidato, @experiencia
        });
      }

      public Row Find(
        string @concurso, string @candidato, string @experiencia
      ) {
        return (Row)Find(new object[] {
          @concurso, @candidato, @experiencia
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
      /// CONCURSO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Concurso { get { return (string)this["concurso"]; } set { this["concurso"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CANDIDATO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Candidato { get { return (string)this["candidato"]; } set { this["candidato"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// EXPERIENCIA: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Experiencia { get { return (string)this["experiencia"]; } set { this["experiencia"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PONTUACAO: NUMERIC(15), Not Null
      /// </summary>
      public decimal? Pontuacao { get { return (decimal?)this["pontuacao"]; } set { this["pontuacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @concurso, string @candidato, string @experiencia) {
        return Insert(connection,
                      @concurso, @candidato, @experiencia,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @concurso, string @candidato, string @experiencia,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_candidato_doc_experiencias(),
          new string[] { "concurso", "candidato", "experiencia" },
          new object[] { @concurso, @candidato, @experiencia },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkConcurso, string pkCandidato, string pkExperiencia,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_candidato_doc_experiencias(),
          new string[] { "concurso", "candidato", "experiencia" },
          new object[] { pkConcurso, pkCandidato, pkExperiencia },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkConcurso, string pkCandidato, string pkExperiencia) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_candidato_doc_experiencias(),
          new string[] { "concurso", "candidato", "experiencia" },
          new object[] { pkConcurso, pkCandidato, pkExperiencia }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkConcurso, string pkCandidato, string pkExperiencia) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_candidato_doc_experiencias(),
          new string[] { "concurso", "candidato", "experiencia" },
          new object[] { pkConcurso, pkCandidato, pkExperiencia }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Concurso { get { return (string)row["concurso", DataRowVersion.Original]; } }
        public string Candidato { get { return (string)row["candidato", DataRowVersion.Original]; } }
        public string Experiencia { get { return (string)row["experiencia", DataRowVersion.Original]; } }
        public decimal? Pontuacao { get { return (decimal?)row["pontuacao", DataRowVersion.Original]; } }

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
    #region Coluna: CONCURSO
    private ColumnConcurso pvColConcurso;

    public sealed class ColumnConcurso : TDataColumn {

      public ColumnConcurso() {
        this.ColumnName = "concurso";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "concurso";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_experiencias.concurso"; } }
      public override string OriginalCol { get { return "concurso"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_EXPERIENCIAS.CONCURSO"; } }
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

    #region Coluna: CANDIDATO
    private ColumnCandidato pvColCandidato;

    public sealed class ColumnCandidato : TDataColumn {

      public ColumnCandidato() {
        this.ColumnName = "candidato";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "candidato";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_experiencias.candidato"; } }
      public override string OriginalCol { get { return "candidato"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_EXPERIENCIAS.CANDIDATO"; } }
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

    #region Coluna: EXPERIENCIA
    private ColumnExperiencia pvColExperiencia;

    public sealed class ColumnExperiencia : TDataColumn {

      public ColumnExperiencia() {
        this.ColumnName = "experiencia";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "experiência";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_experiencias.experiencia"; } }
      public override string OriginalCol { get { return "experiencia"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_EXPERIENCIAS.EXPERIENCIA"; } }
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

    #region Coluna: PONTUACAO
    private ColumnPontuacao pvColPontuacao;

    public sealed class ColumnPontuacao : TDataColumn {

      public ColumnPontuacao() {
        this.ColumnName = "pontuacao";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "pontuação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_doc_experiencia.pontuacao"; } }
      public override string OriginalCol { get { return "pontuacao"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOC_EXPERIENCIA.PONTUACAO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 16; } }
      public override int Precision { get { return 15; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(15)"; } }

    }
    #endregion
    #endregion
  }

  public partial class Ly_candidato_doc_sit_avaliacao : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_candidato_doc_sit_avaliacao() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_candidato_doc_sit_avaliacao");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_candidato_doc_sit_avaliacao");
      }
      catch{}
    }

    public Ly_candidato_doc_sit_avaliacao() {
      pvColConcurso = new ColumnConcurso(); Columns.Add(pvColConcurso);
      pvColCandidato = new ColumnCandidato(); Columns.Add(pvColCandidato);
      pvColSituacao = new ColumnSituacao(); Columns.Add(pvColSituacao);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_candidato_doc_sit_avaliacao");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @concurso, string @candidato, string @situacao) {
      Row row = NewRow();

      row.Concurso = @concurso;
      row.Candidato = @candidato;
      row.Situacao = @situacao;

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
      get { return "LY_CANDIDATO_DOC_SIT_AVALIACAO"; }
    }

    protected override sealed string Name { get { return "Situação da avaliação do candidato a docente"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColConcurso, pvColCandidato, pvColSituacao });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_candidato_doc_sit_av_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_candidato_doc_sit_av_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_candidato_doc_sit_av_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_candidato_doc_sit_av_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkConcurso", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "CONCURSO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkCandidato", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "CANDIDATO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkSituacao", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "SITUACAO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("concurso", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CONCURSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("candidato", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CANDIDATO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("situacao", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SITUACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "concurso") { result.Add(toLower ? "concurso" : column); continue; }
          if(lower == "candidato") { result.Add(toLower ? "candidato" : column); continue; }
          if(lower == "situacao") { result.Add(toLower ? "situacao" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "concurso", "candidato", "situacao" }
                          : new string[] { "CONCURSO", "CANDIDATO", "SITUACAO" });
        else
          result.AddRange(toLower
                          ? new string[] { "concurso", "candidato", "situacao" }
                          : new string[] { "CONCURSO", "CANDIDATO", "SITUACAO" });

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
        "ly_candidato_doc_sit_avaliacao.concurso", "ly_candidato_doc_sit_avaliacao.candidato", 
        "ly_candidato_doc_sit_avaliacao.situacao"
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
          "FROM ly_candidato_doc_sit_avaliacao " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_candidato_doc_sit_avaliacao");
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
        da.TableMappings.Add("Table", "Ly_candidato_doc_sit_avaliacao");
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
            if (dr["CONCURSO"] != null && string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
              dr["CONCURSO"] = DBNull.Value;
            if (dr["CANDIDATO"] != null && string.IsNullOrEmpty(dr["CANDIDATO"].ToString()))
              dr["CANDIDATO"] = DBNull.Value;
            if (dr["SITUACAO"] != null && string.IsNullOrEmpty(dr["SITUACAO"].ToString()))
              dr["SITUACAO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["CONCURSO"] == null || string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
            {
              return "Favor informar o concurso.";
            }

            if (dr["CONCURSO"] != null && !string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
            {
              if (dr["CONCURSO"].ToString().Length > 20)
              {
                return "O concurso deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["CANDIDATO"] == null || string.IsNullOrEmpty(dr["CANDIDATO"].ToString()))
            {
              return "Favor informar o candidato.";
            }

            if (dr["CANDIDATO"] != null && !string.IsNullOrEmpty(dr["CANDIDATO"].ToString()))
            {
              if (dr["CANDIDATO"].ToString().Length > 20)
              {
                return "O candidato deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["SITUACAO"] == null || string.IsNullOrEmpty(dr["SITUACAO"].ToString()))
            {
              return "Favor informar a situação da avaliação.";
            }

            if (dr["SITUACAO"] != null && !string.IsNullOrEmpty(dr["SITUACAO"].ToString()))
            {
              if (dr["SITUACAO"].ToString().Length > 20)
              {
                return "A situação da avaliação deve possuir no máximo 20 caracteres.";
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

    public static Ly_candidato_doc_sit_avaliacao Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_candidato_doc_sit_avaliacao Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_candidato_doc_sit_avaliacao tab = new Ly_candidato_doc_sit_avaliacao();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_candidato_doc_sit_avaliacao tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @concurso, string @candidato, string @situacao
      ) {
        return (Row)Add(new object[] {
          @concurso, @candidato, @situacao
        });
      }

      public Row Find(
        string @concurso, string @candidato, string @situacao
      ) {
        return (Row)Find(new object[] {
          @concurso, @candidato, @situacao
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
      /// CONCURSO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Concurso { get { return (string)this["concurso"]; } set { this["concurso"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CANDIDATO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Candidato { get { return (string)this["candidato"]; } set { this["candidato"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SITUACAO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Situacao { get { return (string)this["situacao"]; } set { this["situacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @concurso, string @candidato, string @situacao) {
        return Insert(connection,
                      @concurso, @candidato, @situacao,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @concurso, string @candidato, string @situacao,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_candidato_doc_sit_avaliacao(),
          new string[] { "concurso", "candidato", "situacao" },
          new object[] { @concurso, @candidato, @situacao },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkConcurso, string pkCandidato, string pkSituacao,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_candidato_doc_sit_avaliacao(),
          new string[] { "concurso", "candidato", "situacao" },
          new object[] { pkConcurso, pkCandidato, pkSituacao },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkConcurso, string pkCandidato, string pkSituacao) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_candidato_doc_sit_avaliacao(),
          new string[] { "concurso", "candidato", "situacao" },
          new object[] { pkConcurso, pkCandidato, pkSituacao }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkConcurso, string pkCandidato, string pkSituacao) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_candidato_doc_sit_avaliacao(),
          new string[] { "concurso", "candidato", "situacao" },
          new object[] { pkConcurso, pkCandidato, pkSituacao }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Concurso { get { return (string)row["concurso", DataRowVersion.Original]; } }
        public string Candidato { get { return (string)row["candidato", DataRowVersion.Original]; } }
        public string Situacao { get { return (string)row["situacao", DataRowVersion.Original]; } }

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
    #region Coluna: CONCURSO
    private ColumnConcurso pvColConcurso;

    public sealed class ColumnConcurso : TDataColumn {

      public ColumnConcurso() {
        this.ColumnName = "concurso";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "concurso";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_sit_avaliacao.concurso"; } }
      public override string OriginalCol { get { return "concurso"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_SIT_AVALIACAO.CONCURSO"; } }
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

    #region Coluna: CANDIDATO
    private ColumnCandidato pvColCandidato;

    public sealed class ColumnCandidato : TDataColumn {

      public ColumnCandidato() {
        this.ColumnName = "candidato";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "candidato";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_sit_avaliacao.candidato"; } }
      public override string OriginalCol { get { return "candidato"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_SIT_AVALIACAO.CANDIDATO"; } }
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

    #region Coluna: SITUACAO
    private ColumnSituacao pvColSituacao;

    public sealed class ColumnSituacao : TDataColumn {

      public ColumnSituacao() {
        this.ColumnName = "situacao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "situação da avaliação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_sit_avaliacao.situacao"; } }
      public override string OriginalCol { get { return "situacao"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_SIT_AVALIACAO.SITUACAO"; } }
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
    #endregion
  }

  public partial class Ly_candidato_doc_solicitacoes : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_candidato_doc_solicitacoes() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_candidato_doc_solicitacoes");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_candidato_doc_solicitacoes");
      }
      catch{}
    }

    public Ly_candidato_doc_solicitacoes() {
      pvColId_candidato_doc_solicitacoes = new ColumnId_candidato_doc_solicitacoes(); Columns.Add(pvColId_candidato_doc_solicitacoes);
      pvColConcurso = new ColumnConcurso(); Columns.Add(pvColConcurso);
      pvColCandidato = new ColumnCandidato(); Columns.Add(pvColCandidato);
      pvColData = new ColumnData(); Columns.Add(pvColData);
      pvColStatus = new ColumnStatus(); Columns.Add(pvColStatus);
      pvColTipo = new ColumnTipo(); Columns.Add(pvColTipo);
      pvColCarga_horaria_atual = new ColumnCarga_horaria_atual(); Columns.Add(pvColCarga_horaria_atual);
      pvColCarga_horaria_nova = new ColumnCarga_horaria_nova(); Columns.Add(pvColCarga_horaria_nova);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_candidato_doc_solicitacoes");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @concurso, string @candidato, DateTime? @data, string @status) {
      Row row = NewRow();

      row.Concurso = @concurso;
      row.Candidato = @candidato;
      row.Data = @data;
      row.Status = @status;

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
      get { return "LY_CANDIDATO_DOC_SOLICITACOES"; }
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
      SetPrimaryKey(new DataColumn[] { pvColId_candidato_doc_solicitacoes });
      pvColId_candidato_doc_solicitacoes.AllowDBNull = true;
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_candidato_doc_solici_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_candidato_doc_solici_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_candidato_doc_solici_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_candidato_doc_solici_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId_candidato_doc_solicitacoes", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID_CANDIDATO_DOC_SOLICITACOES", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("id_candidato_doc_solicitacoes", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ID_CANDIDATO_DOC_SOLICITACOES", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("concurso", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CONCURSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("candidato", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CANDIDATO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("data", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DATA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("status", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "STATUS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("tipo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TIPO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("carga_horaria_atual", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "CARGA_HORARIA_ATUAL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("carga_horaria_nova", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "CARGA_HORARIA_NOVA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "id_candidato_doc_solicitacoes") { result.Add(toLower ? "id_candidato_doc_solicitacoes" : column); continue; }
          if(lower == "concurso") { result.Add(toLower ? "concurso" : column); continue; }
          if(lower == "candidato") { result.Add(toLower ? "candidato" : column); continue; }
          if(lower == "data") { result.Add(toLower ? "data" : column); continue; }
          if(lower == "status") { result.Add(toLower ? "status" : column); continue; }
          if(lower == "tipo") { result.Add(toLower ? "tipo" : column); continue; }
          if(lower == "carga_horaria_atual") { result.Add(toLower ? "carga_horaria_atual" : column); continue; }
          if(lower == "carga_horaria_nova") { result.Add(toLower ? "carga_horaria_nova" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "id_candidato_doc_solicitacoes", "concurso", "candidato", "data", 
                                           "status", "tipo", "carga_horaria_atual", "carga_horaria_nova" }
                          : new string[] { "ID_CANDIDATO_DOC_SOLICITACOES", "CONCURSO", "CANDIDATO", "DATA", 
                                           "STATUS", "TIPO", "CARGA_HORARIA_ATUAL", "CARGA_HORARIA_NOVA" });
        else
          result.AddRange(toLower
                          ? new string[] { "id_candidato_doc_solicitacoes", "concurso", "candidato", "data", 
                                           "status", "tipo", "carga_horaria_atual", "carga_horaria_nova" }
                          : new string[] { "ID_CANDIDATO_DOC_SOLICITACOES", "CONCURSO", "CANDIDATO", "DATA", 
                                           "STATUS", "TIPO", "CARGA_HORARIA_ATUAL", "CARGA_HORARIA_NOVA" });

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
        "ly_candidato_doc_solicitacoes.id_candidato_doc_solicitacoes"
      });
      if(scColumns.Contains("concurso")) list.Add("ly_candidato_doc_solicitacoes.concurso");
      if(scColumns.Contains("candidato")) list.Add("ly_candidato_doc_solicitacoes.candidato");
      if(scColumns.Contains("data")) list.Add("ly_candidato_doc_solicitacoes.data");
      if(scColumns.Contains("status")) list.Add("ly_candidato_doc_solicitacoes.status");
      if(scColumns.Contains("tipo")) list.Add("ly_candidato_doc_solicitacoes.tipo");
      if(scColumns.Contains("carga_horaria_atual")) list.Add("ly_candidato_doc_solicitacoes.carga_horaria_atual");
      if(scColumns.Contains("carga_horaria_nova")) list.Add("ly_candidato_doc_solicitacoes.carga_horaria_nova");
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
          "FROM ly_candidato_doc_solicitacoes " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_candidato_doc_solicitacoes");
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
        da.TableMappings.Add("Table", "Ly_candidato_doc_solicitacoes");
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
            if (dr["CONCURSO"] != null && string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
              dr["CONCURSO"] = DBNull.Value;
            if (dr["CANDIDATO"] != null && string.IsNullOrEmpty(dr["CANDIDATO"].ToString()))
              dr["CANDIDATO"] = DBNull.Value;
            if (dr["STATUS"] != null && string.IsNullOrEmpty(dr["STATUS"].ToString()))
              dr["STATUS"] = DBNull.Value;
            if (dr["TIPO"] != null && string.IsNullOrEmpty(dr["TIPO"].ToString()))
              dr["TIPO"] = DBNull.Value;
            if (dr["CARGA_HORARIA_ATUAL"] != null && string.IsNullOrEmpty(dr["CARGA_HORARIA_ATUAL"].ToString()))
              dr["CARGA_HORARIA_ATUAL"] = DBNull.Value;
            if (dr["CARGA_HORARIA_NOVA"] != null && string.IsNullOrEmpty(dr["CARGA_HORARIA_NOVA"].ToString()))
              dr["CARGA_HORARIA_NOVA"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["CONCURSO"] == null || string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
            {
              return "Favor informar o Concurso.";
            }

            if (dr["CONCURSO"] != null && !string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
            {
              if (dr["CONCURSO"].ToString().Length > 20)
              {
                return "O Concurso deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["CANDIDATO"] == null || string.IsNullOrEmpty(dr["CANDIDATO"].ToString()))
            {
              return "Favor informar o Candidato.";
            }

            if (dr["CANDIDATO"] != null && !string.IsNullOrEmpty(dr["CANDIDATO"].ToString()))
            {
              if (dr["CANDIDATO"].ToString().Length > 20)
              {
                return "O Candidato deve possuir no máximo 20 caracteres.";
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

            if (dr["STATUS"] == null || string.IsNullOrEmpty(dr["STATUS"].ToString()))
            {
              return "Favor informar o Status.";
            }

            if (dr["STATUS"] != null && !string.IsNullOrEmpty(dr["STATUS"].ToString()))
            {
              if (dr["STATUS"].ToString().Length > 40)
              {
                return "O Status deve possuir no máximo 40 caracteres.";
              }
            }

            if (dr["TIPO"] != null && !string.IsNullOrEmpty(dr["TIPO"].ToString()))
            {
              if (dr["TIPO"].ToString().Length > 100)
              {
                return "O Tipo deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["CARGA_HORARIA_ATUAL"] != null && !string.IsNullOrEmpty(dr["CARGA_HORARIA_ATUAL"].ToString()))
            {
              if ((Convert.ToDecimal(dr["CARGA_HORARIA_ATUAL"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["CARGA_HORARIA_ATUAL"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["CARGA_HORARIA_ATUAL"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["CARGA_HORARIA_ATUAL"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Carga_horaria_atual deve ser um valor inteiro com 3 dígitos no máximo.";
              }
            }

            if (dr["CARGA_HORARIA_NOVA"] != null && !string.IsNullOrEmpty(dr["CARGA_HORARIA_NOVA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["CARGA_HORARIA_NOVA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["CARGA_HORARIA_NOVA"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["CARGA_HORARIA_NOVA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["CARGA_HORARIA_NOVA"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Carga_horaria_nova deve ser um valor inteiro com 3 dígitos no máximo.";
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

    public static Ly_candidato_doc_solicitacoes Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_candidato_doc_solicitacoes Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_candidato_doc_solicitacoes tab = new Ly_candidato_doc_solicitacoes();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_candidato_doc_solicitacoes tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @id_candidato_doc_solicitacoes, string @concurso, string @candidato, DateTime? @data, 
        string @status, string @tipo, decimal? @carga_horaria_atual, decimal? @carga_horaria_nova
      ) {
        return (Row)Add(new object[] {
          @id_candidato_doc_solicitacoes, @concurso, @candidato, @data, @status, @tipo, @carga_horaria_atual, 
          @carga_horaria_nova
        });
      }

      public Row Find(
        decimal? @id_candidato_doc_solicitacoes
      ) {
        return (Row)Find(new object[] {
          @id_candidato_doc_solicitacoes
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
      /// ID_CANDIDATO_DOC_SOLICITACOES: NUMERIC(10), Primary Key, AutoNumber, Not Null
      /// </summary>
      public decimal? Id_candidato_doc_solicitacoes { get { return (decimal?)this["id_candidato_doc_solicitacoes"]; } set { this["id_candidato_doc_solicitacoes"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CONCURSO: VARCHAR(20), Not Null
      /// </summary>
      public string Concurso { get { return (string)this["concurso"]; } set { this["concurso"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CANDIDATO: VARCHAR(20), Not Null
      /// </summary>
      public string Candidato { get { return (string)this["candidato"]; } set { this["candidato"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DATA: DATETIME, Not Null
      /// </summary>
      public DateTime? Data { get { return (DateTime?)this["data"]; } set { this["data"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// STATUS: VARCHAR(40), Not Null
      /// </summary>
      public string Status { get { return (string)this["status"]; } set { this["status"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TIPO: VARCHAR(100)
      /// </summary>
      public string Tipo { get { return (string)this["tipo"]; } set { this["tipo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CARGA_HORARIA_ATUAL: NUMERIC(3)
      /// </summary>
      public decimal? Carga_horaria_atual { get { return (decimal?)this["carga_horaria_atual"]; } set { this["carga_horaria_atual"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CARGA_HORARIA_NOVA: NUMERIC(3)
      /// </summary>
      public decimal? Carga_horaria_nova { get { return (decimal?)this["carga_horaria_nova"]; } set { this["carga_horaria_nova"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @concurso, string @candidato, DateTime? @data, string @status) {
        return Insert(connection,
                      @concurso, @candidato, @data, @status,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @concurso, string @candidato, DateTime? @data, string @status,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_candidato_doc_solicitacoes(),
          new string[] { "concurso", "candidato", "data", "status" },
          new object[] { @concurso, @candidato, @data, @status },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId_candidato_doc_solicitacoes,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_candidato_doc_solicitacoes(),
          new string[] { "id_candidato_doc_solicitacoes" },
          new object[] { pkId_candidato_doc_solicitacoes },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId_candidato_doc_solicitacoes) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_candidato_doc_solicitacoes(),
          new string[] { "id_candidato_doc_solicitacoes" },
          new object[] { pkId_candidato_doc_solicitacoes }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              decimal? pkId_candidato_doc_solicitacoes) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_candidato_doc_solicitacoes(),
          new string[] { "id_candidato_doc_solicitacoes" },
          new object[] { pkId_candidato_doc_solicitacoes }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public decimal? Id_candidato_doc_solicitacoes { get { return (decimal?)row["id_candidato_doc_solicitacoes", DataRowVersion.Original]; } }
        public string Concurso { get { return (string)row["concurso", DataRowVersion.Original]; } }
        public string Candidato { get { return (string)row["candidato", DataRowVersion.Original]; } }
        public DateTime? Data { get { return (DateTime?)row["data", DataRowVersion.Original]; } }
        public string Status { get { return (string)row["status", DataRowVersion.Original]; } }
        public string Tipo { get { return (string)row["tipo", DataRowVersion.Original]; } }
        public decimal? Carga_horaria_atual { get { return (decimal?)row["carga_horaria_atual", DataRowVersion.Original]; } }
        public decimal? Carga_horaria_nova { get { return (decimal?)row["carga_horaria_nova", DataRowVersion.Original]; } }

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
    #region Coluna: ID_CANDIDATO_DOC_SOLICITACOES
    private ColumnId_candidato_doc_solicitacoes pvColId_candidato_doc_solicitacoes;

    public sealed class ColumnId_candidato_doc_solicitacoes : TDataColumn {

      public ColumnId_candidato_doc_solicitacoes() {
        this.ColumnName = "id_candidato_doc_solicitacoes";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_solicitacoes.id_candidato_doc_solicitacoes"; } }
      public override string OriginalCol { get { return "id_candidato_doc_solicitacoes"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_SOLICITACOES.ID_CANDIDATO_DOC_SOLICITACOES"; } }
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

    #region Coluna: CONCURSO
    private ColumnConcurso pvColConcurso;

    public sealed class ColumnConcurso : TDataColumn {

      public ColumnConcurso() {
        this.ColumnName = "concurso";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_solicitacoes.concurso"; } }
      public override string OriginalCol { get { return "concurso"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_SOLICITACOES.CONCURSO"; } }
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

    #region Coluna: CANDIDATO
    private ColumnCandidato pvColCandidato;

    public sealed class ColumnCandidato : TDataColumn {

      public ColumnCandidato() {
        this.ColumnName = "candidato";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_solicitacoes.candidato"; } }
      public override string OriginalCol { get { return "candidato"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_SOLICITACOES.CANDIDATO"; } }
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
      public override string FullCol { get { return "ly_candidato_doc_solicitacoes.data"; } }
      public override string OriginalCol { get { return "data"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_SOLICITACOES.DATA"; } }
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


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_solicitacoes.status"; } }
      public override string OriginalCol { get { return "status"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_SOLICITACOES.STATUS"; } }
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
      public override string FullCol { get { return "ly_candidato_doc_solicitacoes.tipo"; } }
      public override string OriginalCol { get { return "tipo"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_SOLICITACOES.TIPO"; } }
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

    #region Coluna: CARGA_HORARIA_ATUAL
    private ColumnCarga_horaria_atual pvColCarga_horaria_atual;

    public sealed class ColumnCarga_horaria_atual : TDataColumn {

      public ColumnCarga_horaria_atual() {
        this.ColumnName = "carga_horaria_atual";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_solicitacoes.carga_horaria_atual"; } }
      public override string OriginalCol { get { return "carga_horaria_atual"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_SOLICITACOES.CARGA_HORARIA_ATUAL"; } }
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

    #region Coluna: CARGA_HORARIA_NOVA
    private ColumnCarga_horaria_nova pvColCarga_horaria_nova;

    public sealed class ColumnCarga_horaria_nova : TDataColumn {

      public ColumnCarga_horaria_nova() {
        this.ColumnName = "carga_horaria_nova";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_solicitacoes.carga_horaria_nova"; } }
      public override string OriginalCol { get { return "carga_horaria_nova"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_SOLICITACOES.CARGA_HORARIA_NOVA"; } }
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

  public partial class Ly_candidato_doc_titulacoes : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_candidato_doc_titulacoes() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_candidato_doc_titulacoes");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_candidato_doc_titulacoes");
      }
      catch{}
    }

    public Ly_candidato_doc_titulacoes() {
      pvColConcurso = new ColumnConcurso(); Columns.Add(pvColConcurso);
      pvColCandidato = new ColumnCandidato(); Columns.Add(pvColCandidato);
      pvColTitulacao = new ColumnTitulacao(); Columns.Add(pvColTitulacao);
      pvColPontuacao = new ColumnPontuacao(); Columns.Add(pvColPontuacao);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_candidato_doc_titulacoes");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @concurso, string @candidato, string @titulacao) {
      Row row = NewRow();

      row.Concurso = @concurso;
      row.Candidato = @candidato;
      row.Titulacao = @titulacao;

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
      get { return "LY_CANDIDATO_DOC_TITULACOES"; }
    }

    protected override sealed string Name { get { return "Titulação do candidato a docente"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColConcurso, pvColCandidato, pvColTitulacao });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_candidato_doc_titula_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_candidato_doc_titula_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_candidato_doc_titula_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_candidato_doc_titula_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkConcurso", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "CONCURSO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkCandidato", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "CANDIDATO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkTitulacao", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "TITULACAO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("concurso", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CONCURSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("candidato", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CANDIDATO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("titulacao", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TITULACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "concurso") { result.Add(toLower ? "concurso" : column); continue; }
          if(lower == "candidato") { result.Add(toLower ? "candidato" : column); continue; }
          if(lower == "titulacao") { result.Add(toLower ? "titulacao" : column); continue; }
          if(!tabPrincipal) {
            if(lower == "pontuacao") { result.Add(toLower ? "pontuacao" : column); continue; }
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "concurso", "candidato", "titulacao" }
                          : new string[] { "CONCURSO", "CANDIDATO", "TITULACAO" });
        else
          result.AddRange(toLower
                          ? new string[] { "concurso", "candidato", "titulacao", "pontuacao" }
                          : new string[] { "CONCURSO", "CANDIDATO", "TITULACAO", "PONTUACAO" });

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
        "ly_candidato_doc_titulacoes.concurso", "ly_candidato_doc_titulacoes.candidato", 
        "ly_candidato_doc_titulacoes.titulacao"
      });
      if(scColumns.Contains("pontuacao")) list.Add("ly_concurso_doc_titulacoes.pontuacao");
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
          "FROM ly_candidato_doc_titulacoes " +
               "INNER JOIN ly_concurso_doc_titulacoes " +
                 "ON ly_concurso_doc_titulacoes.concurso = ly_candidato_doc_titulacoes.concurso AND " +
                    "ly_concurso_doc_titulacoes.titulacao = ly_candidato_doc_titulacoes.titulacao " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_candidato_doc_titulacoes");
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
        da.TableMappings.Add("Table", "Ly_candidato_doc_titulacoes");
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
            if (dr["CONCURSO"] != null && string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
              dr["CONCURSO"] = DBNull.Value;
            if (dr["CANDIDATO"] != null && string.IsNullOrEmpty(dr["CANDIDATO"].ToString()))
              dr["CANDIDATO"] = DBNull.Value;
            if (dr["TITULACAO"] != null && string.IsNullOrEmpty(dr["TITULACAO"].ToString()))
              dr["TITULACAO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["CONCURSO"] == null || string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
            {
              return "Favor informar o concurso.";
            }

            if (dr["CONCURSO"] != null && !string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
            {
              if (dr["CONCURSO"].ToString().Length > 20)
              {
                return "O concurso deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["CANDIDATO"] == null || string.IsNullOrEmpty(dr["CANDIDATO"].ToString()))
            {
              return "Favor informar o candidato.";
            }

            if (dr["CANDIDATO"] != null && !string.IsNullOrEmpty(dr["CANDIDATO"].ToString()))
            {
              if (dr["CANDIDATO"].ToString().Length > 20)
              {
                return "O candidato deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["TITULACAO"] == null || string.IsNullOrEmpty(dr["TITULACAO"].ToString()))
            {
              return "Favor informar a titulação.";
            }

            if (dr["TITULACAO"] != null && !string.IsNullOrEmpty(dr["TITULACAO"].ToString()))
            {
              if (dr["TITULACAO"].ToString().Length > 20)
              {
                return "A titulação deve possuir no máximo 20 caracteres.";
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

    public static Ly_candidato_doc_titulacoes Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_candidato_doc_titulacoes Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_candidato_doc_titulacoes tab = new Ly_candidato_doc_titulacoes();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_candidato_doc_titulacoes tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @concurso, string @candidato, string @titulacao
      ) {
        return (Row)Add(new object[] {
          @concurso, @candidato, @titulacao
        });
      }

      public Row Find(
        string @concurso, string @candidato, string @titulacao
      ) {
        return (Row)Find(new object[] {
          @concurso, @candidato, @titulacao
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
      /// CONCURSO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Concurso { get { return (string)this["concurso"]; } set { this["concurso"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CANDIDATO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Candidato { get { return (string)this["candidato"]; } set { this["candidato"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TITULACAO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Titulacao { get { return (string)this["titulacao"]; } set { this["titulacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PONTUACAO: NUMERIC(15), Not Null
      /// </summary>
      public decimal? Pontuacao { get { return (decimal?)this["pontuacao"]; } set { this["pontuacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @concurso, string @candidato, string @titulacao) {
        return Insert(connection,
                      @concurso, @candidato, @titulacao,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @concurso, string @candidato, string @titulacao,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_candidato_doc_titulacoes(),
          new string[] { "concurso", "candidato", "titulacao" },
          new object[] { @concurso, @candidato, @titulacao },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkConcurso, string pkCandidato, string pkTitulacao,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_candidato_doc_titulacoes(),
          new string[] { "concurso", "candidato", "titulacao" },
          new object[] { pkConcurso, pkCandidato, pkTitulacao },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkConcurso, string pkCandidato, string pkTitulacao) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_candidato_doc_titulacoes(),
          new string[] { "concurso", "candidato", "titulacao" },
          new object[] { pkConcurso, pkCandidato, pkTitulacao }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkConcurso, string pkCandidato, string pkTitulacao) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_candidato_doc_titulacoes(),
          new string[] { "concurso", "candidato", "titulacao" },
          new object[] { pkConcurso, pkCandidato, pkTitulacao }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Concurso { get { return (string)row["concurso", DataRowVersion.Original]; } }
        public string Candidato { get { return (string)row["candidato", DataRowVersion.Original]; } }
        public string Titulacao { get { return (string)row["titulacao", DataRowVersion.Original]; } }
        public decimal? Pontuacao { get { return (decimal?)row["pontuacao", DataRowVersion.Original]; } }

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
    #region Coluna: CONCURSO
    private ColumnConcurso pvColConcurso;

    public sealed class ColumnConcurso : TDataColumn {

      public ColumnConcurso() {
        this.ColumnName = "concurso";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "concurso";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_titulacoes.concurso"; } }
      public override string OriginalCol { get { return "concurso"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_TITULACOES.CONCURSO"; } }
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

    #region Coluna: CANDIDATO
    private ColumnCandidato pvColCandidato;

    public sealed class ColumnCandidato : TDataColumn {

      public ColumnCandidato() {
        this.ColumnName = "candidato";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "candidato";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_titulacoes.candidato"; } }
      public override string OriginalCol { get { return "candidato"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_TITULACOES.CANDIDATO"; } }
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

    #region Coluna: TITULACAO
    private ColumnTitulacao pvColTitulacao;

    public sealed class ColumnTitulacao : TDataColumn {

      public ColumnTitulacao() {
        this.ColumnName = "titulacao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "titulação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_candidato_doc_titulacoes.titulacao"; } }
      public override string OriginalCol { get { return "titulacao"; } }
      public override string RefCol { get { return "LY_CANDIDATO_DOC_TITULACOES.TITULACAO"; } }
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

    #region Coluna: PONTUACAO
    private ColumnPontuacao pvColPontuacao;

    public sealed class ColumnPontuacao : TDataColumn {

      public ColumnPontuacao() {
        this.ColumnName = "pontuacao";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "pontuação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_doc_titulacoes.pontuacao"; } }
      public override string OriginalCol { get { return "pontuacao"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOC_TITULACOES.PONTUACAO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return true; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 16; } }
      public override int Precision { get { return 15; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(15)"; } }

    }
    #endregion
    #endregion
  }

  public partial class Ly_candidato_docente : TDataTable
  {
      private static ConstructorInfo ctorTCustom = null;
      private static ConstructorInfo ctorCustom = null;
      private CustomBase tcustom = null;
      private CustomBase custom = null;

      static Ly_candidato_docente()
      {
          try
          {
              ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_candidato_docente");
              ctorCustom = CRCustomLib.GetCustomConstructor("Ly_candidato_docente");
          }
          catch { }
      }

      public Ly_candidato_docente()
      {
          pvColConcurso = new ColumnConcurso(); Columns.Add(pvColConcurso);
          pvColCandidato = new ColumnCandidato(); Columns.Add(pvColCandidato);
          pvColNome = new ColumnNome(); Columns.Add(pvColNome);
          pvColDt_nasc = new ColumnDt_nasc(); Columns.Add(pvColDt_nasc);
          pvColSexo = new ColumnSexo(); Columns.Add(pvColSexo);
          pvColNome_mae = new ColumnNome_mae(); Columns.Add(pvColNome_mae);
          pvColNome_pai = new ColumnNome_pai(); Columns.Add(pvColNome_pai);
          pvColEstado_civil = new ColumnEstado_civil(); Columns.Add(pvColEstado_civil);
          pvColPais_nasc = new ColumnPais_nasc(); Columns.Add(pvColPais_nasc);
          pvColNacionalidade = new ColumnNacionalidade(); Columns.Add(pvColNacionalidade);
          pvColMunicipio_nasc = new ColumnMunicipio_nasc(); Columns.Add(pvColMunicipio_nasc);
          pvColEnd_pais = new ColumnEnd_pais(); Columns.Add(pvColEnd_pais);
          pvColCep = new ColumnCep(); Columns.Add(pvColCep);
          pvColEnd_municipio = new ColumnEnd_municipio(); Columns.Add(pvColEnd_municipio);
          pvColEndereco = new ColumnEndereco(); Columns.Add(pvColEndereco);
          pvColEnd_num = new ColumnEnd_num(); Columns.Add(pvColEnd_num);
          pvColEnd_compl = new ColumnEnd_compl(); Columns.Add(pvColEnd_compl);
          pvColBairro = new ColumnBairro(); Columns.Add(pvColBairro);
          pvColFone = new ColumnFone(); Columns.Add(pvColFone);
          pvColCelular = new ColumnCelular(); Columns.Add(pvColCelular);
          pvColE_mail = new ColumnE_mail(); Columns.Add(pvColE_mail);
          pvColRg_tipo = new ColumnRg_tipo(); Columns.Add(pvColRg_tipo);
          pvColRg_num = new ColumnRg_num(); Columns.Add(pvColRg_num);
          pvColRg_emissor = new ColumnRg_emissor(); Columns.Add(pvColRg_emissor);
          pvColRg_uf = new ColumnRg_uf(); Columns.Add(pvColRg_uf);
          pvColRg_dtexp = new ColumnRg_dtexp(); Columns.Add(pvColRg_dtexp);
          pvColCpf = new ColumnCpf(); Columns.Add(pvColCpf);
          pvColPis_pasep = new ColumnPis_pasep(); Columns.Add(pvColPis_pasep);
          pvColCprof_num = new ColumnCprof_num(); Columns.Add(pvColCprof_num);
          pvColCprof_serie = new ColumnCprof_serie(); Columns.Add(pvColCprof_serie);
          pvColCprof_uf = new ColumnCprof_uf(); Columns.Add(pvColCprof_uf);
          pvColCprof_dtexp = new ColumnCprof_dtexp(); Columns.Add(pvColCprof_dtexp);
          pvColCategoria = new ColumnCategoria(); Columns.Add(pvColCategoria);
          pvColNucleo = new ColumnNucleo(); Columns.Add(pvColNucleo);
          pvColAgrupamento_ingresso = new ColumnAgrupamento_ingresso(); Columns.Add(pvColAgrupamento_ingresso);
          pvColStatus = new ColumnStatus(); Columns.Add(pvColStatus);
          pvColStatus_obs = new ColumnStatus_obs(); Columns.Add(pvColStatus_obs);
          pvColDt_convocacao = new ColumnDt_convocacao(); Columns.Add(pvColDt_convocacao);
          pvColDt_apresentacao = new ColumnDt_apresentacao(); Columns.Add(pvColDt_apresentacao);
          pvColHora_apresentacao = new ColumnHora_apresentacao(); Columns.Add(pvColHora_apresentacao);
          pvColCdrh = new ColumnCdrh(); Columns.Add(pvColCdrh);
          pvColDt_cdrh = new ColumnDt_cdrh(); Columns.Add(pvColDt_cdrh);
          pvColCqhi = new ColumnCqhi(); Columns.Add(pvColCqhi);
          pvColDt_cqhi = new ColumnDt_cqhi(); Columns.Add(pvColDt_cqhi);
          pvColRemessa_documento = new ColumnRemessa_documento(); Columns.Add(pvColRemessa_documento);
          pvColDt_remessa_documento = new ColumnDt_remessa_documento(); Columns.Add(pvColDt_remessa_documento);
          pvColDt_proposta = new ColumnDt_proposta(); Columns.Add(pvColDt_proposta);
          pvColUsulogin = new ColumnUsulogin(); Columns.Add(pvColUsulogin);
          pvColUsusenha = new ColumnUsusenha(); Columns.Add(pvColUsusenha);
          pvColCarga_horaria = new ColumnCarga_horaria(); Columns.Add(pvColCarga_horaria);
          pvColGi_seplag = new ColumnGi_seplag(); Columns.Add(pvColGi_seplag);
          pvColMunicipio_Proc = new ColumnMunicipio_Proc(); Columns.Add(pvColMunicipio_Proc);
          pvColFinalizado = new ColumnFinalizado(); Columns.Add(pvColFinalizado);
          pvColEtniaId = new ColumnEtniaID(); Columns.Add(pvColEtniaId);
          pvColCotaIdInscricao = new ColumnCotaIdInscricao(); Columns.Add(pvColCotaIdInscricao);
          pvColCotaIdConvocacao = new ColumnCotaIdConvocacao(); Columns.Add(pvColCotaIdConvocacao);
          pvColIdFuncional = new ColumnIdFuncional(); Columns.Add(pvColIdFuncional);
          pvColDataValidacaoInscricao = new ColumnDataValidacaoInscricao(); Columns.Add(pvColDataValidacaoInscricao);
          pvColRegionalId = new ColumnRegionalId(); Columns.Add(pvColRegionalId);
          pvColNecessidadeEspecialId = new ColumnNecessidadeEspecialId(); Columns.Add(pvColNecessidadeEspecialId);

          InitRows(new RowCollection(BaseRows));
          SetPrimaryKey();
          SetTableName("ly_candidato_docente");

          if (ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
          if (ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
      }


      public new Row NewRow()
      {
          return (Row)base.NewRow();
      }

      public Row NewRow(string @concurso, string @candidato, string @nome, DateTime? @dt_nasc,
                        string @sexo, string @estado_civil, string @pais_nasc, string @nacionalidade, string @municipio_nasc,
                        string @end_pais, string @cep, string @end_municipio, string @endereco, string @end_num,
                        string @bairro, string @rg_tipo, string @rg_num, string @rg_emissor, string @cpf,
                        decimal? @status)
      {
          Row row = NewRow();

          row.Concurso = @concurso;
          row.Candidato = @candidato;
          row.Nome = @nome;
          row.Dt_nasc = @dt_nasc;
          row.Sexo = @sexo;
          row.Estado_civil = @estado_civil;
          row.Pais_nasc = @pais_nasc;
          row.Nacionalidade = @nacionalidade;
          row.Municipio_nasc = @municipio_nasc;
          row.End_pais = @end_pais;
          row.Cep = @cep;
          row.End_municipio = @end_municipio;
          row.Endereco = @endereco;
          row.End_num = @end_num;
          row.Bairro = @bairro;
          row.Rg_tipo = @rg_tipo;
          row.Rg_num = @rg_num;
          row.Rg_emissor = @rg_emissor;
          row.Cpf = @cpf;
          row.Status = @status;

          return row;
      }

      public new Row[] Select() { return Select(string.Empty); }

      public new Row[] Select(string filterExpression) { return Select(filterExpression, string.Empty); }

      public new Row[] Select(string filterExpression, string sort) { return Select(filterExpression, sort, DataViewRowState.CurrentRows); }

      public new Row[] Select(string filterExpression, string sort, DataViewRowState recordStates)
      {
          DataRow[] rows = base.Select(filterExpression, sort, recordStates);
          Row[] result = new Row[rows.Length];
          rows.CopyTo(result, 0);
          return result;
      }


      [Browsable(false)]
      public override sealed string ConnectionString { get { return Config.ConnectionString; } }

      [Browsable(false)]
      public override sealed string MainTableName
      {
          get { return "LY_CANDIDATO_DOCENTE"; }
      }

      protected override sealed string Name { get { return "Candidato a docente"; } }

      [Browsable(false)]
      public override sealed bool ReadOnly
      {
          get { return false; }
      }

      [Browsable(false)]
      public new RowCollection Rows { get { return (RowCollection)base.Rows; } }


      protected override sealed DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
          return new Row(builder);
      }

      protected override sealed void SetPrimaryKey()
      {
          SetPrimaryKey(new DataColumn[] { pvColConcurso, pvColCandidato });
      }


      private static TCommand CreateCommand(TConnection cn, CommandOperation operation)
      {
          bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
          TCommand cm = CreateDataTableCommand(cn, operation);
          cm.CommandType = CommandType.StoredProcedure;
          cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

          switch (operation)
          {
              case CommandOperation.Select: cm.CommandText = "zLy_candidato_docente_GetCurrentValues"; break;
              case CommandOperation.Insert: cm.CommandText = "zLy_candidato_docente_Cmd"; break;
              case CommandOperation.Update: cm.CommandText = "zLy_candidato_docente_Cmd"; break;
              case CommandOperation.Delete: cm.CommandText = "zLy_candidato_docente_Cmd"; break;
              default: throw new InvalidOperationException();
          }


          if (operation == CommandOperation.Select)
          {
              cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
              cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
          }

          cm.Parameters.Add(new TParameter("pkConcurso", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "CONCURSO", DataRowVersion.Original, DBNull.Value));
          cm.Parameters.Add(new TParameter("pkCandidato", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "CANDIDATO", DataRowVersion.Original, DBNull.Value));

          cm.Parameters.Add(new TParameter("concurso", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CONCURSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("candidato", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CANDIDATO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("nome", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NOME", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("dt_nasc", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_NASC", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
          cm.Parameters.Add(new TParameter("sexo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SEXO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
          cm.Parameters.Add(new TParameter("nome_mae", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NOME_MAE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("nome_pai", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NOME_PAI", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("estado_civil", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "ESTADO_CIVIL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("pais_nasc", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PAIS_NASC", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("nacionalidade", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NACIONALIDADE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("municipio_nasc", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "MUNICIPIO_NASC", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("end_pais", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "END_PAIS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("cep", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CEP", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("end_municipio", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "END_MUNICIPIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("endereco", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "ENDERECO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("end_num", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "END_NUM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("end_compl", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "END_COMPL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("bairro", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "BAIRRO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("fone", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "FONE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("celular", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CELULAR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("e_mail", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "E_MAIL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("rg_tipo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "RG_TIPO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("rg_num", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "RG_NUM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("rg_emissor", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "RG_EMISSOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("rg_uf", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "RG_UF", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("rg_dtexp", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "RG_DTEXP", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
          cm.Parameters.Add(new TParameter("cpf", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CPF", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("pis_pasep", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PIS_PASEP", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("cprof_num", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CPROF_NUM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("cprof_serie", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CPROF_SERIE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("cprof_uf", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CPROF_UF", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("cprof_dtexp", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "CPROF_DTEXP", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
          cm.Parameters.Add(new TParameter("categoria", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CATEGORIA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("nucleo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NUCLEO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("agrupamento_ingresso", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "AGRUPAMENTO_INGRESSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("status", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "STATUS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
          cm.Parameters.Add(new TParameter("status_obs", OleDbType.VarChar, 2100, ParameterDirection.InputOutput, true, 0, 0, "STATUS_OBS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("dt_convocacao", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_CONVOCACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
          cm.Parameters.Add(new TParameter("dt_apresentacao", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_APRESENTACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
          cm.Parameters.Add(new TParameter("hora_apresentacao", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "HORA_APRESENTACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
          cm.Parameters.Add(new TParameter("cdrh", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CDRH", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("dt_cdrh", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_CDRH", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
          cm.Parameters.Add(new TParameter("cqhi", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CQHI", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("dt_cqhi", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_CQHI", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
          cm.Parameters.Add(new TParameter("remessa_documento", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "REMESSA_DOCUMENTO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("dt_remessa_documento", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_REMESSA_DOCUMENTO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
          cm.Parameters.Add(new TParameter("dt_proposta", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_PROPOSTA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
          cm.Parameters.Add(new TParameter("usulogin", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "USULOGIN", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("ususenha", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "USUSENHA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("carga_horaria", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "CARGA_HORARIA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
          cm.Parameters.Add(new TParameter("gi_seplag", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "GI_Seplag", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("municipio_proc", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "municipio_proc", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("finalizado", OleDbType.VarChar, 1, ParameterDirection.InputOutput, true, 0, 0, "finalizado", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("etniaid", OleDbType.Integer, 0, ParameterDirection.InputOutput, true, 0, 0, "etniaid", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
          cm.Parameters.Add(new TParameter("cotaidinscricao", OleDbType.Integer, 0, ParameterDirection.InputOutput, true, 0, 0, "cotaidinscricao", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
          cm.Parameters.Add(new TParameter("cotaidconvocacao", OleDbType.Integer, 0, ParameterDirection.InputOutput, true, 0, 0, "cotaidconvocacao", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
          cm.Parameters.Add(new TParameter("idfuncional", OleDbType.Integer, 0, ParameterDirection.InputOutput, true, 0, 0, "idfuncional", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
          cm.Parameters.Add(new TParameter("datavalidacaoinscricao", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "datavalidacaoinscricao", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
          cm.Parameters.Add(new TParameter("regionalid", OleDbType.Integer, 0, ParameterDirection.InputOutput, true, 0, 0, "regionalid", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
          cm.Parameters.Add(new TParameter("necessidadeespecialid", OleDbType.Integer, 0, ParameterDirection.InputOutput, true, 0, 0, "necessidadeespecialid", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

          switch (operation)
          {
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
      private static StringCollection FilterColumns(string[] columns, bool toLower, bool tabPrincipal)
      {
          StringCollection result = new StringCollection();

          if (columns != null)
              foreach (string column in columns)
              {
                  string lower = column.ToLower();
                  if (lower == "concurso") { result.Add(toLower ? "concurso" : column); continue; }
                  if (lower == "candidato") { result.Add(toLower ? "candidato" : column); continue; }
                  if (lower == "nome") { result.Add(toLower ? "nome" : column); continue; }
                  if (lower == "dt_nasc") { result.Add(toLower ? "dt_nasc" : column); continue; }
                  if (lower == "sexo") { result.Add(toLower ? "sexo" : column); continue; }                
                  if (lower == "nome_mae") { result.Add(toLower ? "nome_mae" : column); continue; }
                  if (lower == "nome_pai") { result.Add(toLower ? "nome_pai" : column); continue; }
                  if (lower == "estado_civil") { result.Add(toLower ? "estado_civil" : column); continue; }
                  if (lower == "pais_nasc") { result.Add(toLower ? "pais_nasc" : column); continue; }
                  if (lower == "nacionalidade") { result.Add(toLower ? "nacionalidade" : column); continue; }
                  if (lower == "municipio_nasc") { result.Add(toLower ? "municipio_nasc" : column); continue; }
                  if (lower == "end_pais") { result.Add(toLower ? "end_pais" : column); continue; }
                  if (lower == "cep") { result.Add(toLower ? "cep" : column); continue; }
                  if (lower == "end_municipio") { result.Add(toLower ? "end_municipio" : column); continue; }
                  if (lower == "endereco") { result.Add(toLower ? "endereco" : column); continue; }
                  if (lower == "end_num") { result.Add(toLower ? "end_num" : column); continue; }
                  if (lower == "end_compl") { result.Add(toLower ? "end_compl" : column); continue; }
                  if (lower == "bairro") { result.Add(toLower ? "bairro" : column); continue; }
                  if (lower == "fone") { result.Add(toLower ? "fone" : column); continue; }
                  if (lower == "celular") { result.Add(toLower ? "celular" : column); continue; }
                  if (lower == "e_mail") { result.Add(toLower ? "e_mail" : column); continue; }
                  if (lower == "rg_tipo") { result.Add(toLower ? "rg_tipo" : column); continue; }
                  if (lower == "rg_num") { result.Add(toLower ? "rg_num" : column); continue; }
                  if (lower == "rg_emissor") { result.Add(toLower ? "rg_emissor" : column); continue; }
                  if (lower == "rg_uf") { result.Add(toLower ? "rg_uf" : column); continue; }
                  if (lower == "rg_dtexp") { result.Add(toLower ? "rg_dtexp" : column); continue; }
                  if (lower == "cpf") { result.Add(toLower ? "cpf" : column); continue; }
                  if (lower == "pis_pasep") { result.Add(toLower ? "pis_pasep" : column); continue; }
                  if (lower == "cprof_num") { result.Add(toLower ? "cprof_num" : column); continue; }
                  if (lower == "cprof_serie") { result.Add(toLower ? "cprof_serie" : column); continue; }
                  if (lower == "cprof_uf") { result.Add(toLower ? "cprof_uf" : column); continue; }
                  if (lower == "cprof_dtexp") { result.Add(toLower ? "cprof_dtexp" : column); continue; }
                  if (lower == "categoria") { result.Add(toLower ? "categoria" : column); continue; }
                  if (lower == "nucleo") { result.Add(toLower ? "nucleo" : column); continue; }
                  if (lower == "agrupamento_ingresso") { result.Add(toLower ? "agrupamento_ingresso" : column); continue; }
                  if (lower == "status") { result.Add(toLower ? "status" : column); continue; }
                  if (lower == "status_obs") { result.Add(toLower ? "status_obs" : column); continue; }
                  if (lower == "dt_convocacao") { result.Add(toLower ? "dt_convocacao" : column); continue; }
                  if (lower == "dt_apresentacao") { result.Add(toLower ? "dt_apresentacao" : column); continue; }
                  if (lower == "hora_apresentacao") { result.Add(toLower ? "hora_apresentacao" : column); continue; }
                  if (lower == "cdrh") { result.Add(toLower ? "cdrh" : column); continue; }
                  if (lower == "dt_cdrh") { result.Add(toLower ? "dt_cdrh" : column); continue; }
                  if (lower == "cqhi") { result.Add(toLower ? "cqhi" : column); continue; }
                  if (lower == "dt_cqhi") { result.Add(toLower ? "dt_cqhi" : column); continue; }
                  if (lower == "remessa_documento") { result.Add(toLower ? "remessa_documento" : column); continue; }
                  if (lower == "dt_remessa_documento") { result.Add(toLower ? "dt_remessa_documento" : column); continue; }
                  if (lower == "dt_proposta") { result.Add(toLower ? "dt_proposta" : column); continue; }
                  if (lower == "usulogin") { result.Add(toLower ? "usulogin" : column); continue; }
                  if (lower == "ususenha") { result.Add(toLower ? "ususenha" : column); continue; }
                  if (lower == "carga_horaria") { result.Add(toLower ? "carga_horaria" : column); continue; }
                  if (lower == "gi_seplag") { result.Add(toLower ? "gi_seplag" : column); continue; }
                  if (lower == "municipio_proc") { result.Add(toLower ? "municipio_proc" : column); continue; }
                  if (lower == "finalizado") { result.Add(toLower ? "finalizado" : column); continue; }
                  if (lower == "etniaid") { result.Add(toLower ? "etniaid" : column); continue; }
                  if (lower == "cotaidinscricao") { result.Add(toLower ? "cotaidinscricao" : column); continue; }
                  if (lower == "cotaidconvocacao") { result.Add(toLower ? "cotaidconvocacao" : column); continue; }
                  if (lower == "idfuncional") { result.Add(toLower ? "idfuncional" : column); continue; }
                  if (lower == "datavalidacaoinscricao") { result.Add(toLower ? "datavalidacaoinscricao" : column); continue; }
                  if (lower == "regionalid") { result.Add(toLower ? "regionalid" : column); continue; }
                  if (lower == "necessidadeespecialid") { result.Add(toLower ? "necessidadeespecialid" : column); continue; }
                  if (!tabPrincipal)
                  {
                  }
              }

          if (result.Count == 0)
              if (tabPrincipal)
                  result.AddRange(toLower
                                  ? new string[] { "concurso", "candidato", "nome", "dt_nasc", "sexo", 
                                           "nome_mae", "nome_pai", "estado_civil", "pais_nasc", "nacionalidade", "municipio_nasc", 
                                           "end_pais", "cep", "end_municipio", "endereco", "end_num", "end_compl", "bairro", 
                                           "fone", "celular", "e_mail", "rg_tipo", "rg_num", "rg_emissor", "rg_uf", "rg_dtexp", 
                                           "cpf", "pis_pasep", "cprof_num", "cprof_serie", "cprof_uf", "cprof_dtexp", "categoria", 
                                           "nucleo", "agrupamento_ingresso", "status", "status_obs", "dt_convocacao", "dt_apresentacao", 
                                           "hora_apresentacao", "cdrh", "dt_cdrh", "cqhi", "dt_cqhi", "remessa_documento", "dt_remessa_documento", 
                                           "dt_proposta", "usulogin", "ususenha", "carga_horaria", "gi_seplag", "municipio_proc", "finalizado", "etniaid",
                                           "cotaidinscricao", "cotaidconvocacao", "idfuncional", "datavalidacaoinscricao", "regionalid", "necessidadeespecialid"}
                                  : new string[] { "CONCURSO", "CANDIDATO", "NOME", "DT_NASC", "SEXO", 
                                           "NOME_MAE", "NOME_PAI", "ESTADO_CIVIL", "PAIS_NASC", "NACIONALIDADE", "MUNICIPIO_NASC", 
                                           "END_PAIS", "CEP", "END_MUNICIPIO", "ENDERECO", "END_NUM", "END_COMPL", "BAIRRO", 
                                           "FONE", "CELULAR", "E_MAIL", "RG_TIPO", "RG_NUM", "RG_EMISSOR", "RG_UF", "RG_DTEXP", 
                                           "CPF", "PIS_PASEP", "CPROF_NUM", "CPROF_SERIE", "CPROF_UF", "CPROF_DTEXP", "CATEGORIA", 
                                           "NUCLEO", "AGRUPAMENTO_INGRESSO", "STATUS", "STATUS_OBS", "DT_CONVOCACAO", "DT_APRESENTACAO", 
                                           "HORA_APRESENTACAO", "CDRH", "DT_CDRH", "CQHI", "DT_CQHI", "REMESSA_DOCUMENTO", "DT_REMESSA_DOCUMENTO", 
                                           "DT_PROPOSTA", "USULOGIN", "USUSENHA", "CARGA_HORARIA", "GI_Seplag", "MUNICIPIO_PROC", "FINALIZADO", "ETNIAID",
                                           "COTAIDINSCRICAO", "COTAIDCONVOCACAO", "IDFUNCIONAL", "DATAVALIDACAOINSCRICAO", "REGIONALID", "NECESSIDADEESPECIALID"});
              else
                  result.AddRange(toLower
                                  ? new string[] { "concurso", "candidato", "nome", "dt_nasc", "sexo", 
                                           "nome_mae", "nome_pai", "estado_civil", "pais_nasc", "nacionalidade", "municipio_nasc", 
                                           "end_pais", "cep", "end_municipio", "endereco", "end_num", "end_compl", "bairro", 
                                           "fone", "celular", "e_mail", "rg_tipo", "rg_num", "rg_emissor", "rg_uf", "rg_dtexp", 
                                           "cpf", "pis_pasep", "cprof_num", "cprof_serie", "cprof_uf", "cprof_dtexp", "categoria", 
                                           "nucleo", "agrupamento_ingresso", "status", "status_obs", "dt_convocacao", "dt_apresentacao", 
                                           "hora_apresentacao", "cdrh", "dt_cdrh", "cqhi", "dt_cqhi", "remessa_documento", "dt_remessa_documento", 
                                           "dt_proposta", "usulogin", "ususenha", "carga_horaria", "gi_seplag", "municipio_proc", "finalizado",
                                           "etniaid", "cotaidinscricao", "cotaidconvocacao", "idfuncional", "datavalidacaoinscricao", "regionalid", "necessidadeespecialid"}
                                  : new string[] { "CONCURSO", "CANDIDATO", "NOME", "DT_NASC", "SEXO", 
                                           "NOME_MAE", "NOME_PAI", "ESTADO_CIVIL", "PAIS_NASC", "NACIONALIDADE", "MUNICIPIO_NASC", 
                                           "END_PAIS", "CEP", "END_MUNICIPIO", "ENDERECO", "END_NUM", "END_COMPL", "BAIRRO", 
                                           "FONE", "CELULAR", "E_MAIL", "RG_TIPO", "RG_NUM", "RG_EMISSOR", "RG_UF", "RG_DTEXP", 
                                           "CPF", "PIS_PASEP", "CPROF_NUM", "CPROF_SERIE", "CPROF_UF", "CPROF_DTEXP", "CATEGORIA", 
                                           "NUCLEO", "AGRUPAMENTO_INGRESSO", "STATUS", "STATUS_OBS", "DT_CONVOCACAO", "DT_APRESENTACAO", 
                                           "HORA_APRESENTACAO", "CDRH", "DT_CDRH", "CQHI", "DT_CQHI", "REMESSA_DOCUMENTO", "DT_REMESSA_DOCUMENTO", 
                                           "DT_PROPOSTA", "USULOGIN", "USUSENHA", "CARGA_HORARIA", "GI_Seplag", "MUNICIPIO_PROC", "FINALIZADO",
                                           "ETNIAID", "COTAIDINSCRICAO", "COTAIDCONVOCACAO", "IDFUNCIONAL", "DATAVALIDACAOINSCRICAO", "REGIONALID", "NECESSIDADEESPECIALID"});

          return result;
      }


      protected override sealed string PreOperationInternal(TDataRowUpdateEventArgs args)
      {
          string error = string.Empty;

          if (custom != null)
              switch (args.CommandOperation)
              {
                  case CommandOperation.Insert: error = custom.PreInsert((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
                  case CommandOperation.Update: error = custom.PreUpdate((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
                  case CommandOperation.Delete: error = custom.PreDelete((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
                  case CommandOperation.Undelete: error = custom.PreUndelete((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
                  default: throw new NotImplementedException();
              }

          if (error != null && error.Length == 0 && tcustom != null)
              switch (args.CommandOperation)
              {
                  case CommandOperation.Insert: error = tcustom.PreInsert((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
                  case CommandOperation.Update: error = tcustom.PreUpdate((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
                  case CommandOperation.Delete: error = tcustom.PreDelete((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
                  case CommandOperation.Undelete: error = tcustom.PreUndelete((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
                  default: throw new NotImplementedException();
              }

          if (error == null) throw new InvalidOperationException("Entry-points não podem devolver null. Devolva string.Empty para indicar inexistência de erros.");
          return error;
      }

      protected override sealed string PostOperationInternal(TDataRowUpdateEventArgs args)
      {
          string error = string.Empty;

          if (custom != null)
              switch (args.CommandOperation)
              {
                  case CommandOperation.Insert: error = custom.PostInsert((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
                  case CommandOperation.Update: error = custom.PostUpdate((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
                  case CommandOperation.Delete: error = custom.PostDelete((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
                  case CommandOperation.Undelete: error = custom.PostUndelete((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
                  default: throw new NotImplementedException();
              }

          if (error != null && error.Length == 0 && tcustom != null)
              switch (args.CommandOperation)
              {
                  case CommandOperation.Insert: error = tcustom.PostInsert((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
                  case CommandOperation.Update: error = tcustom.PostUpdate((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
                  case CommandOperation.Delete: error = tcustom.PostDelete((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
                  case CommandOperation.Undelete: error = tcustom.PostUndelete((Row)args.Row, (TConnectionWritable)args.Command.Connection); break;
                  default: throw new NotImplementedException();
              }

          if (error == null) throw new InvalidOperationException("Entry-points não podem devolver null. Devolva string.Empty para indicar inexistência de erros.");
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
                                     bool rowCountOnly)
      {
          if (where == null) throw new ArgumentNullException("where");
          if (order == null) throw new ArgumentNullException("order");
          TConnection cn = connection != null ? connection : Config.CreateConnection();
          bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
          where = where.Trim();

          int count;

          StringCollection scColumns = FilterColumns(columns, true, false);

          #region Build Select
          ArrayList list = new ArrayList();
          list.AddRange(new string[] {
        "ly_candidato_docente.concurso", "ly_candidato_docente.candidato"
      });
          if (scColumns.Contains("nome")) list.Add("ly_candidato_docente.nome");
          if (scColumns.Contains("dt_nasc")) list.Add("ly_candidato_docente.dt_nasc");
          if (scColumns.Contains("sexo")) list.Add("ly_candidato_docente.sexo");          
          if (scColumns.Contains("nome_mae")) list.Add("ly_candidato_docente.nome_mae");
          if (scColumns.Contains("nome_pai")) list.Add("ly_candidato_docente.nome_pai");
          if (scColumns.Contains("estado_civil")) list.Add("ly_candidato_docente.estado_civil");
          if (scColumns.Contains("pais_nasc")) list.Add("ly_candidato_docente.pais_nasc");
          if (scColumns.Contains("nacionalidade")) list.Add("ly_candidato_docente.nacionalidade");
          if (scColumns.Contains("municipio_nasc")) list.Add("ly_candidato_docente.municipio_nasc");
          if (scColumns.Contains("end_pais")) list.Add("ly_candidato_docente.end_pais");
          if (scColumns.Contains("cep")) list.Add("ly_candidato_docente.cep");
          if (scColumns.Contains("end_municipio")) list.Add("ly_candidato_docente.end_municipio");
          if (scColumns.Contains("endereco")) list.Add("ly_candidato_docente.endereco");
          if (scColumns.Contains("end_num")) list.Add("ly_candidato_docente.end_num");
          if (scColumns.Contains("end_compl")) list.Add("ly_candidato_docente.end_compl");
          if (scColumns.Contains("bairro")) list.Add("ly_candidato_docente.bairro");
          if (scColumns.Contains("fone")) list.Add("ly_candidato_docente.fone");
          if (scColumns.Contains("celular")) list.Add("ly_candidato_docente.celular");
          if (scColumns.Contains("e_mail")) list.Add("ly_candidato_docente.e_mail");
          if (scColumns.Contains("rg_tipo")) list.Add("ly_candidato_docente.rg_tipo");
          if (scColumns.Contains("rg_num")) list.Add("ly_candidato_docente.rg_num");
          if (scColumns.Contains("rg_emissor")) list.Add("ly_candidato_docente.rg_emissor");
          if (scColumns.Contains("rg_uf")) list.Add("ly_candidato_docente.rg_uf");
          if (scColumns.Contains("rg_dtexp")) list.Add("ly_candidato_docente.rg_dtexp");
          if (scColumns.Contains("cpf")) list.Add("ly_candidato_docente.cpf");
          if (scColumns.Contains("pis_pasep")) list.Add("ly_candidato_docente.pis_pasep");
          if (scColumns.Contains("cprof_num")) list.Add("ly_candidato_docente.cprof_num");
          if (scColumns.Contains("cprof_serie")) list.Add("ly_candidato_docente.cprof_serie");
          if (scColumns.Contains("cprof_uf")) list.Add("ly_candidato_docente.cprof_uf");
          if (scColumns.Contains("cprof_dtexp")) list.Add("ly_candidato_docente.cprof_dtexp");
          if (scColumns.Contains("categoria")) list.Add("ly_candidato_docente.categoria");
          if (scColumns.Contains("nucleo")) list.Add("ly_candidato_docente.nucleo");
          if (scColumns.Contains("agrupamento_ingresso")) list.Add("ly_candidato_docente.agrupamento_ingresso");
          if (scColumns.Contains("status")) list.Add("ly_candidato_docente.status");
          if (scColumns.Contains("status_obs")) list.Add("ly_candidato_docente.status_obs");
          if (scColumns.Contains("dt_convocacao")) list.Add("ly_candidato_docente.dt_convocacao");
          if (scColumns.Contains("dt_apresentacao")) list.Add("ly_candidato_docente.dt_apresentacao");
          if (scColumns.Contains("hora_apresentacao")) list.Add("ly_candidato_docente.hora_apresentacao");
          if (scColumns.Contains("cdrh")) list.Add("ly_candidato_docente.cdrh");
          if (scColumns.Contains("dt_cdrh")) list.Add("ly_candidato_docente.dt_cdrh");
          if (scColumns.Contains("cqhi")) list.Add("ly_candidato_docente.cqhi");
          if (scColumns.Contains("dt_cqhi")) list.Add("ly_candidato_docente.dt_cqhi");
          if (scColumns.Contains("remessa_documento")) list.Add("ly_candidato_docente.remessa_documento");
          if (scColumns.Contains("dt_remessa_documento")) list.Add("ly_candidato_docente.dt_remessa_documento");
          if (scColumns.Contains("dt_proposta")) list.Add("ly_candidato_docente.dt_proposta");
          if (scColumns.Contains("usulogin")) list.Add("ly_candidato_docente.usulogin");
          if (scColumns.Contains("ususenha")) list.Add("ly_candidato_docente.ususenha");
          if (scColumns.Contains("carga_horaria")) list.Add("ly_candidato_docente.carga_horaria");
          if (scColumns.Contains("gi_seplag")) list.Add("ly_candidato_docente.gi_seplag");
          if (scColumns.Contains("municipio_proc")) list.Add("ly_candidato_docente.municipio_proc");
          if (scColumns.Contains("finalizado")) list.Add("ly_candidato_docente.finalizado");
          if (scColumns.Contains("etniaid")) list.Add("ly_candidato_docente.etniaid");
          if (scColumns.Contains("cotaidinscricao")) list.Add("ly_candidato_docente.cotaidinscricao");
          if (scColumns.Contains("cotaidconvocacao")) list.Add("ly_candidato_docente.cotaidconvocacao");
          if (scColumns.Contains("idfuncional")) list.Add("ly_candidato_docente.idfuncional");
          if (scColumns.Contains("datavalidacaoinscricao")) list.Add("ly_candidato_docente.datavalidacaoinscricao");
          if (scColumns.Contains("regionalid")) list.Add("ly_candidato_docente.regionalid");
          if (scColumns.Contains("necessidadeespecialid")) list.Add("ly_candidato_docente.necessidadeespecialid");
          string select, columnList, orderby;
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
              "FROM ly_candidato_docente " +
              (where.Length == 0 ? string.Empty : " WHERE " + where) +
              orderby;

          #endregion
          if (rowCountOnly)
          {
              #region RowCountOnly
              using (TCommand cm = cn.CreateCommand(select, whereValues))
              {
                  try
                  {
                      count = (int)(decimal)cm.ExecuteScalar();
                  }
                  catch (Exception exc)
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
              using (TCommand cm = cn.CreateCommand(select, whereValues))
              {
                  TDataAdapter da = new TDataAdapter(cm);
                  try
                  {
                      DataSet ds = BaseDataSet;
                      bool fakeds = false;
                      if (ds == null)
                      {
                          fakeds = true;
                          ds = new DataSet();
                          ds.Tables.Add(this);
                      }
                      count = da.Fill(ds, startRecord, maxRecords, "ly_candidato_docente");
                      if (fakeds) ds.Tables.Remove(this);
                  }
                  catch (Exception exc)
                  {
                      throw new Exception(exc.Message + " " +
                                          "Select statement: [" + select + "], " +
                                          "Parameters: [" + StrLib.EnumerableToStr(whereValues) + "]");
                  }
              }

              if (createPrimaryKey) SetPrimaryKey();

              // Cria os nomes (caption) nas colunas da tabela do DataSet
              // de acordo com o idioma informado no construtor da classe.
              foreach (DataColumn dataColumn in Columns)
              {
                  TDataColumn c = dataColumn as TDataColumn;
                  if (c != null && scColumns.Contains(c.ColumnName.ToLower()))
                      c.ReadOnly = c.IsAux;
              }
              #endregion
          }

          return count;
      }

      public override sealed void Put(TConnectionWritable connection, DataRow[] dataRows)
      {
          if (connection == null) throw new ArgumentNullException();

          TDataAdapter da;
          {
              da = new TDataAdapter();
              da.TableMappings.Add("Table", "Ly_candidato_docente");
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
              if (dr["CONCURSO"] != null && string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
                  dr["CONCURSO"] = DBNull.Value;
              if (dr["CANDIDATO"] != null && string.IsNullOrEmpty(dr["CANDIDATO"].ToString()))
                  dr["CANDIDATO"] = DBNull.Value;
              if (dr["NOME"] != null && string.IsNullOrEmpty(dr["NOME"].ToString()))
                  dr["NOME"] = DBNull.Value;
              if (dr["SEXO"] != null && string.IsNullOrEmpty(dr["SEXO"].ToString()))
                  dr["SEXO"] = DBNull.Value;
              if (dr["NOME_MAE"] != null && string.IsNullOrEmpty(dr["NOME_MAE"].ToString()))
                  dr["NOME_MAE"] = DBNull.Value;
              if (dr["NOME_PAI"] != null && string.IsNullOrEmpty(dr["NOME_PAI"].ToString()))
                  dr["NOME_PAI"] = DBNull.Value;
              if (dr["ESTADO_CIVIL"] != null && string.IsNullOrEmpty(dr["ESTADO_CIVIL"].ToString()))
                  dr["ESTADO_CIVIL"] = DBNull.Value;
              if (dr["PAIS_NASC"] != null && string.IsNullOrEmpty(dr["PAIS_NASC"].ToString()))
                  dr["PAIS_NASC"] = DBNull.Value;
              if (dr["NACIONALIDADE"] != null && string.IsNullOrEmpty(dr["NACIONALIDADE"].ToString()))
                  dr["NACIONALIDADE"] = DBNull.Value;
              if (dr["MUNICIPIO_NASC"] != null && string.IsNullOrEmpty(dr["MUNICIPIO_NASC"].ToString()))
                  dr["MUNICIPIO_NASC"] = DBNull.Value;
              if (dr["END_PAIS"] != null && string.IsNullOrEmpty(dr["END_PAIS"].ToString()))
                  dr["END_PAIS"] = DBNull.Value;
              if (dr["CEP"] != null && string.IsNullOrEmpty(dr["CEP"].ToString()))
                  dr["CEP"] = DBNull.Value;
              if (dr["END_MUNICIPIO"] != null && string.IsNullOrEmpty(dr["END_MUNICIPIO"].ToString()))
                  dr["END_MUNICIPIO"] = DBNull.Value;
              if (dr["ENDERECO"] != null && string.IsNullOrEmpty(dr["ENDERECO"].ToString()))
                  dr["ENDERECO"] = DBNull.Value;
              if (dr["END_NUM"] != null && string.IsNullOrEmpty(dr["END_NUM"].ToString()))
                  dr["END_NUM"] = DBNull.Value;
              if (dr["END_COMPL"] != null && string.IsNullOrEmpty(dr["END_COMPL"].ToString()))
                  dr["END_COMPL"] = DBNull.Value;
              if (dr["BAIRRO"] != null && string.IsNullOrEmpty(dr["BAIRRO"].ToString()))
                  dr["BAIRRO"] = DBNull.Value;
              if (dr["FONE"] != null && string.IsNullOrEmpty(dr["FONE"].ToString()))
                  dr["FONE"] = DBNull.Value;
              if (dr["CELULAR"] != null && string.IsNullOrEmpty(dr["CELULAR"].ToString()))
                  dr["CELULAR"] = DBNull.Value;
              if (dr["E_MAIL"] != null && string.IsNullOrEmpty(dr["E_MAIL"].ToString()))
                  dr["E_MAIL"] = DBNull.Value;
              if (dr["RG_TIPO"] != null && string.IsNullOrEmpty(dr["RG_TIPO"].ToString()))
                  dr["RG_TIPO"] = DBNull.Value;
              if (dr["RG_NUM"] != null && string.IsNullOrEmpty(dr["RG_NUM"].ToString()))
                  dr["RG_NUM"] = DBNull.Value;
              if (dr["RG_EMISSOR"] != null && string.IsNullOrEmpty(dr["RG_EMISSOR"].ToString()))
                  dr["RG_EMISSOR"] = DBNull.Value;
              if (dr["RG_UF"] != null && string.IsNullOrEmpty(dr["RG_UF"].ToString()))
                  dr["RG_UF"] = DBNull.Value;
              if (dr["CPF"] != null && string.IsNullOrEmpty(dr["CPF"].ToString()))
                  dr["CPF"] = DBNull.Value;
              if (dr["PIS_PASEP"] != null && string.IsNullOrEmpty(dr["PIS_PASEP"].ToString()))
                  dr["PIS_PASEP"] = DBNull.Value;
              if (dr["CPROF_NUM"] != null && string.IsNullOrEmpty(dr["CPROF_NUM"].ToString()))
                  dr["CPROF_NUM"] = DBNull.Value;
              if (dr["CPROF_SERIE"] != null && string.IsNullOrEmpty(dr["CPROF_SERIE"].ToString()))
                  dr["CPROF_SERIE"] = DBNull.Value;
              if (dr["CPROF_UF"] != null && string.IsNullOrEmpty(dr["CPROF_UF"].ToString()))
                  dr["CPROF_UF"] = DBNull.Value;
              if (dr["CATEGORIA"] != null && string.IsNullOrEmpty(dr["CATEGORIA"].ToString()))
                  dr["CATEGORIA"] = DBNull.Value;
              if (dr["NUCLEO"] != null && string.IsNullOrEmpty(dr["NUCLEO"].ToString()))
                  dr["NUCLEO"] = DBNull.Value;
              if (dr["AGRUPAMENTO_INGRESSO"] != null && string.IsNullOrEmpty(dr["AGRUPAMENTO_INGRESSO"].ToString()))
                  dr["AGRUPAMENTO_INGRESSO"] = DBNull.Value;
              if (dr["STATUS"] != null && string.IsNullOrEmpty(dr["STATUS"].ToString()))
                  dr["STATUS"] = DBNull.Value;
              if (dr["STATUS_OBS"] != null && string.IsNullOrEmpty(dr["STATUS_OBS"].ToString()))
                  dr["STATUS_OBS"] = DBNull.Value;
              if (dr["CDRH"] != null && string.IsNullOrEmpty(dr["CDRH"].ToString()))
                  dr["CDRH"] = DBNull.Value;
              if (dr["CQHI"] != null && string.IsNullOrEmpty(dr["CQHI"].ToString()))
                  dr["CQHI"] = DBNull.Value;
              if (dr["REMESSA_DOCUMENTO"] != null && string.IsNullOrEmpty(dr["REMESSA_DOCUMENTO"].ToString()))
                  dr["REMESSA_DOCUMENTO"] = DBNull.Value;
              if (dr["USULOGIN"] != null && string.IsNullOrEmpty(dr["USULOGIN"].ToString()))
                  dr["USULOGIN"] = DBNull.Value;
              if (dr["USUSENHA"] != null && string.IsNullOrEmpty(dr["USUSENHA"].ToString()))
                  dr["USUSENHA"] = DBNull.Value;
              if (dr["CARGA_HORARIA"] != null && string.IsNullOrEmpty(dr["CARGA_HORARIA"].ToString()))
                  dr["CARGA_HORARIA"] = DBNull.Value;
              if (dr["GI_Seplag"] != null && string.IsNullOrEmpty(dr["GI_Seplag"].ToString()))
                  dr["GI_Seplag"] = DBNull.Value;
              if (dr["MUNICIPIO_PROC"] != null && string.IsNullOrEmpty(dr["MUNICIPIO_PROC"].ToString()))
                  dr["MUNICIPIO_PROC"] = DBNull.Value;
              if (dr["FINALIZADO"] != null && string.IsNullOrEmpty(dr["FINALIZADO"].ToString()))
                  dr["FINALIZADO"] = DBNull.Value;
          }
      }

      public string ValidaCampos(DataRow dr)
      {
          if (dr != null)
          {

              if (dr["CONCURSO"] == null || string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
              {
                  return "Favor informar o concurso.";
              }

              if (dr["CONCURSO"] != null && !string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
              {
                  if (dr["CONCURSO"].ToString().Length > 20)
                  {
                      return "O concurso deve possuir no máximo 20 caracteres.";
                  }
              }

              if (dr["CANDIDATO"] == null || string.IsNullOrEmpty(dr["CANDIDATO"].ToString()))
              {
                  return "Favor informar o candidato.";
              }

              if (dr["CANDIDATO"] != null && !string.IsNullOrEmpty(dr["CANDIDATO"].ToString()))
              {
                  if (dr["CANDIDATO"].ToString().Length > 20)
                  {
                      return "O candidato deve possuir no máximo 20 caracteres.";
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

              if (dr["DT_NASC"] == null || string.IsNullOrEmpty(dr["DT_NASC"].ToString()))
              {
                  return "Favor informar a data de nascimento.";
              }

              if (dr["DT_NASC"] != null && dr["DT_NASC"].ToString() != "")
              {
                  dr["DT_NASC"] = Convert.ToDateTime(dr["DT_NASC"]).ToString("dd/MM/yyyy HH:mm:ss");
              }

              if (dr["SEXO"] == null || string.IsNullOrEmpty(dr["SEXO"].ToString()))
              {
                  return "Favor informar o sexo.";
              }

              if (dr["SEXO"] != null && !string.IsNullOrEmpty(dr["SEXO"].ToString()))
              {
                  if (dr["SEXO"].ToString().Length > 1)
                  {
                      return "O sexo deve possuir no máximo 1 caractere.";
                  }
              }            

              if (dr["NOME_MAE"] != null && !string.IsNullOrEmpty(dr["NOME_MAE"].ToString()))
              {
                  if (dr["NOME_MAE"].ToString().Length > 100)
                  {
                      return "O nome da mãe deve possuir no máximo 100 caracteres.";
                  }
              }

              if (dr["NOME_PAI"] != null && !string.IsNullOrEmpty(dr["NOME_PAI"].ToString()))
              {
                  if (dr["NOME_PAI"].ToString().Length > 100)
                  {
                      return "O nome do pai deve possuir no máximo 100 caracteres.";
                  }
              }

              if (dr["ESTADO_CIVIL"] == null || string.IsNullOrEmpty(dr["ESTADO_CIVIL"].ToString()))
              {
                  return "Favor informar o estado civil.";
              }

              if (dr["ESTADO_CIVIL"] != null && !string.IsNullOrEmpty(dr["ESTADO_CIVIL"].ToString()))
              {
                  if (dr["ESTADO_CIVIL"].ToString().Length > 40)
                  {
                      return "O estado civil deve possuir no máximo 40 caracteres.";
                  }
              }

              if (dr["PAIS_NASC"] == null || string.IsNullOrEmpty(dr["PAIS_NASC"].ToString()))
              {
                  return "Favor informar o país de nascimento.";
              }

              if (dr["PAIS_NASC"] != null && !string.IsNullOrEmpty(dr["PAIS_NASC"].ToString()))
              {
                  if (dr["PAIS_NASC"].ToString().Length > 50)
                  {
                      return "O país de nascimento deve possuir no máximo 50 caracteres.";
                  }
              }

              if (dr["NACIONALIDADE"] == null || string.IsNullOrEmpty(dr["NACIONALIDADE"].ToString()))
              {
                  return "Favor informar o nacionalidade.";
              }

              if (dr["NACIONALIDADE"] != null && !string.IsNullOrEmpty(dr["NACIONALIDADE"].ToString()))
              {
                  if (dr["NACIONALIDADE"].ToString().Length > 15)
                  {
                      return "O nacionalidade deve possuir no máximo 15 caracteres.";
                  }
              }

              if (dr["MUNICIPIO_NASC"] == null || string.IsNullOrEmpty(dr["MUNICIPIO_NASC"].ToString()))
              {
                  return "Favor informar o município de nascimento.";
              }

              if (dr["MUNICIPIO_NASC"] != null && !string.IsNullOrEmpty(dr["MUNICIPIO_NASC"].ToString()))
              {
                  if (dr["MUNICIPIO_NASC"].ToString().Length > 20)
                  {
                      return "O município de nascimento deve possuir no máximo 20 caracteres.";
                  }
              }

              if (dr["END_PAIS"] == null || string.IsNullOrEmpty(dr["END_PAIS"].ToString()))
              {
                  return "Favor informar o país do endereço.";
              }

              if (dr["END_PAIS"] != null && !string.IsNullOrEmpty(dr["END_PAIS"].ToString()))
              {
                  if (dr["END_PAIS"].ToString().Length > 50)
                  {
                      return "O país do endereço deve possuir no máximo 50 caracteres.";
                  }
              }

              if (dr["CEP"] == null || string.IsNullOrEmpty(dr["CEP"].ToString()))
              {
                  return "Favor informar o cep.";
              }

              if (dr["CEP"] != null && !string.IsNullOrEmpty(dr["CEP"].ToString()))
              {
                  if (dr["CEP"].ToString().Length > 9)
                  {
                      return "O cep deve possuir no máximo 9 caracteres.";
                  }
              }

              if (dr["END_MUNICIPIO"] == null || string.IsNullOrEmpty(dr["END_MUNICIPIO"].ToString()))
              {
                  return "Favor informar o município do endereço.";
              }

              if (dr["END_MUNICIPIO"] != null && !string.IsNullOrEmpty(dr["END_MUNICIPIO"].ToString()))
              {
                  if (dr["END_MUNICIPIO"].ToString().Length > 20)
                  {
                      return "O município do endereço deve possuir no máximo 20 caracteres.";
                  }
              }

              if (dr["ENDERECO"] == null || string.IsNullOrEmpty(dr["ENDERECO"].ToString()))
              {
                  return "Favor informar o endereço.";
              }

              if (dr["ENDERECO"] != null && !string.IsNullOrEmpty(dr["ENDERECO"].ToString()))
              {
                  if (dr["ENDERECO"].ToString().Length > 50)
                  {
                      return "O endereço deve possuir no máximo 50 caracteres.";
                  }
              }

              if (dr["END_NUM"] == null || string.IsNullOrEmpty(dr["END_NUM"].ToString()))
              {
                  return "Favor informar o número do endereço.";
              }

              if (dr["END_NUM"] != null && !string.IsNullOrEmpty(dr["END_NUM"].ToString()))
              {
                  if (dr["END_NUM"].ToString().Length > 15)
                  {
                      return "O número do endereço deve possuir no máximo 15 caracteres.";
                  }
              }

              if (dr["END_COMPL"] != null && !string.IsNullOrEmpty(dr["END_COMPL"].ToString()))
              {
                  if (dr["END_COMPL"].ToString().Length > 50)
                  {
                      return "O complemento do endereço deve possuir no máximo 50 caracteres.";
                  }
              }

              if (dr["BAIRRO"] == null || string.IsNullOrEmpty(dr["BAIRRO"].ToString()))
              {
                  return "Favor informar o bairro.";
              }

              if (dr["BAIRRO"] != null && !string.IsNullOrEmpty(dr["BAIRRO"].ToString()))
              {
                  if (dr["BAIRRO"].ToString().Length > 50)
                  {
                      return "O bairro deve possuir no máximo 50 caracteres.";
                  }
              }

              if (dr["FONE"] != null && !string.IsNullOrEmpty(dr["FONE"].ToString()))
              {
                  if (dr["FONE"].ToString().Length > 30)
                  {
                      return "O telefone deve possuir no máximo 30 caracteres.";
                  }
              }

              if (dr["CELULAR"] != null && !string.IsNullOrEmpty(dr["CELULAR"].ToString()))
              {
                  if (dr["CELULAR"].ToString().Length > 30)
                  {
                      return "O celular deve possuir no máximo 30 caracteres.";
                  }
              }

              if (dr["E_MAIL"] != null && !string.IsNullOrEmpty(dr["E_MAIL"].ToString()))
              {
                  if (dr["E_MAIL"].ToString().Length > 100)
                  {
                      return "O email deve possuir no máximo 100 caracteres.";
                  }
              }

              //if (dr["RG_TIPO"] == null || string.IsNullOrEmpty(dr["RG_TIPO"].ToString()))
              //{
              //    return "Favor informar o tipo do documento de identificação.";
              //}

              //if (dr["RG_TIPO"] != null && !string.IsNullOrEmpty(dr["RG_TIPO"].ToString()))
              //{
              //    if (dr["RG_TIPO"].ToString().Length > 15)
              //    {
              //        return "O tipo do documento de identificação deve possuir no máximo 15 caracteres.";
              //    }
              //}

              //if (dr["RG_NUM"] == null || string.IsNullOrEmpty(dr["RG_NUM"].ToString()))
              //{
              //    return "Favor informar o número do documento de identificação.";
              //}

              //if (dr["RG_NUM"] != null && !string.IsNullOrEmpty(dr["RG_NUM"].ToString()))
              //{
              //    if (dr["RG_NUM"].ToString().Length > 20)
              //    {
              //        return "O número do documento de identificação deve possuir no máximo 20 caracteres.";
              //    }
              //}

              //if (dr["RG_EMISSOR"] == null || string.IsNullOrEmpty(dr["RG_EMISSOR"].ToString()))
              //{
              //    return "Favor informar o órgão emissor do documento de identificação.";
              //}

              //if (dr["RG_EMISSOR"] != null && !string.IsNullOrEmpty(dr["RG_EMISSOR"].ToString()))
              //{
              //    if (dr["RG_EMISSOR"].ToString().Length > 15)
              //    {
              //        return "O órgão emissor do documento de identificação deve possuir no máximo 15 caracteres.";
              //    }
              //}

              //if (dr["RG_UF"] != null && !string.IsNullOrEmpty(dr["RG_UF"].ToString()))
              //{
              //    if (dr["RG_UF"].ToString().Length > 2)
              //    {
              //        return "O estado do documento de identificação deve possuir no máximo 2 caracteres.";
              //    }
              //}

              //if (dr["RG_DTEXP"] != null && dr["RG_DTEXP"].ToString() != "")
              //{
              //    dr["RG_DTEXP"] = Convert.ToDateTime(dr["RG_DTEXP"]).ToString("dd/MM/yyyy HH:mm:ss");
              //}

              if (dr["CPF"] == null || string.IsNullOrEmpty(dr["CPF"].ToString()))
              {
                  return "Favor informar o cpf.";
              }

              if (dr["CPF"] != null && !string.IsNullOrEmpty(dr["CPF"].ToString()))
              {
                  if (dr["CPF"].ToString().Length > 14)
                  {
                      return "O cpf deve possuir no máximo 14 caracteres.";
                  }
              }

              if (dr["PIS_PASEP"] != null && !string.IsNullOrEmpty(dr["PIS_PASEP"].ToString()))
              {
                  if (dr["PIS_PASEP"].ToString().Length > 50)
                  {
                      return "O pis/pasep deve possuir no máximo 50 caracteres.";
                  }
              }

              if (dr["CPROF_NUM"] != null && !string.IsNullOrEmpty(dr["CPROF_NUM"].ToString()))
              {
                  if (dr["CPROF_NUM"].ToString().Length > 15)
                  {
                      return "O número da carteira profissional deve possuir no máximo 15 caracteres.";
                  }
              }

              if (dr["CPROF_SERIE"] != null && !string.IsNullOrEmpty(dr["CPROF_SERIE"].ToString()))
              {
                  if (dr["CPROF_SERIE"].ToString().Length > 15)
                  {
                      return "O número de série da carteira profissional deve possuir no máximo 15 caracteres.";
                  }
              }

              if (dr["CPROF_UF"] != null && !string.IsNullOrEmpty(dr["CPROF_UF"].ToString()))
              {
                  if (dr["CPROF_UF"].ToString().Length > 2)
                  {
                      return "O estado da carteira profissional deve possuir no máximo 2 caracteres.";
                  }
              }

              if (dr["CPROF_DTEXP"] != null && dr["CPROF_DTEXP"].ToString() != "")
              {
                  dr["CPROF_DTEXP"] = Convert.ToDateTime(dr["CPROF_DTEXP"]).ToString("dd/MM/yyyy HH:mm:ss");
              }

              if (dr["CATEGORIA"] != null && !string.IsNullOrEmpty(dr["CATEGORIA"].ToString()))
              {
                  if (dr["CATEGORIA"].ToString().Length > 20)
                  {
                      return "O cargo deve possuir no máximo 20 caracteres.";
                  }
              }

              if (dr["NUCLEO"] != null && !string.IsNullOrEmpty(dr["NUCLEO"].ToString()))
              {
                  if (dr["NUCLEO"].ToString().Length > 20)
                  {
                      return "A coordenadoria deve possuir no máximo 20 caracteres.";
                  }
              }

              if (dr["AGRUPAMENTO_INGRESSO"] != null && !string.IsNullOrEmpty(dr["AGRUPAMENTO_INGRESSO"].ToString()))
              {
                  if (dr["AGRUPAMENTO_INGRESSO"].ToString().Length > 50)
                  {
                      return "O grupo de habilitação de ingresso deve possuir no máximo 50 caracteres.";
                  }
              }

              if (dr["STATUS"] == null || string.IsNullOrEmpty(dr["STATUS"].ToString()))
              {
                  return "Favor informar o status.";
              }

              if (dr["STATUS"] != null && !string.IsNullOrEmpty(dr["STATUS"].ToString()))
              {
                  if ((Convert.ToDecimal(dr["STATUS"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["STATUS"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["STATUS"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["STATUS"]).ToString("0.############").Split(',')[1].Length > 0))
                  {
                      return "O status deve ser um valor inteiro com 3 dígitos no máximo.";
                  }
              }

              if (dr["STATUS_OBS"] != null && !string.IsNullOrEmpty(dr["STATUS_OBS"].ToString()))
              {
                  if (dr["STATUS_OBS"].ToString().Length > 2000)
                  {
                      return "A observação do status deve possuir no máximo 2000 caracteres.";
                  }
              }

              if (dr["DT_CONVOCACAO"] != null && dr["DT_CONVOCACAO"].ToString() != "")
              {
                  dr["DT_CONVOCACAO"] = Convert.ToDateTime(dr["DT_CONVOCACAO"]).ToString("dd/MM/yyyy HH:mm:ss");
              }

              if (dr["DT_APRESENTACAO"] != null && dr["DT_APRESENTACAO"].ToString() != "")
              {
                  dr["DT_APRESENTACAO"] = Convert.ToDateTime(dr["DT_APRESENTACAO"]).ToString("dd/MM/yyyy HH:mm:ss");
              }

              if (dr["HORA_APRESENTACAO"] != null && dr["HORA_APRESENTACAO"].ToString() != "")
              {
                  dr["HORA_APRESENTACAO"] = Convert.ToDateTime(dr["HORA_APRESENTACAO"]).ToString("dd/MM/yyyy HH:mm:ss");
              }

              if (dr["CDRH"] != null && !string.IsNullOrEmpty(dr["CDRH"].ToString()))
              {
                  if (dr["CDRH"].ToString().Length > 50)
                  {
                      return "O cdrh deve possuir no máximo 50 caracteres.";
                  }
              }

              if (dr["DT_CDRH"] != null && dr["DT_CDRH"].ToString() != "")
              {
                  dr["DT_CDRH"] = Convert.ToDateTime(dr["DT_CDRH"]).ToString("dd/MM/yyyy HH:mm:ss");
              }

              if (dr["CQHI"] != null && !string.IsNullOrEmpty(dr["CQHI"].ToString()))
              {
                  if (dr["CQHI"].ToString().Length > 50)
                  {
                      return "O cqhi deve possuir no máximo 50 caracteres.";
                  }
              }

              if (dr["DT_CQHI"] != null && dr["DT_CQHI"].ToString() != "")
              {
                  dr["DT_CQHI"] = Convert.ToDateTime(dr["DT_CQHI"]).ToString("dd/MM/yyyy HH:mm:ss");
              }

              if (dr["REMESSA_DOCUMENTO"] != null && !string.IsNullOrEmpty(dr["REMESSA_DOCUMENTO"].ToString()))
              {
                  if (dr["REMESSA_DOCUMENTO"].ToString().Length > 50)
                  {
                      return "A remessa de documentos deve possuir no máximo 50 caracteres.";
                  }
              }

              if (dr["DT_REMESSA_DOCUMENTO"] != null && dr["DT_REMESSA_DOCUMENTO"].ToString() != "")
              {
                  dr["DT_REMESSA_DOCUMENTO"] = Convert.ToDateTime(dr["DT_REMESSA_DOCUMENTO"]).ToString("dd/MM/yyyy HH:mm:ss");
              }

              if (dr["DT_PROPOSTA"] != null && dr["DT_PROPOSTA"].ToString() != "")
              {
                  dr["DT_PROPOSTA"] = Convert.ToDateTime(dr["DT_PROPOSTA"]).ToString("dd/MM/yyyy HH:mm:ss");
              }

              if (dr["USULOGIN"] != null && !string.IsNullOrEmpty(dr["USULOGIN"].ToString()))
              {
                  if (dr["USULOGIN"].ToString().Length > 50)
                  {
                      return "O Usulogin deve possuir no máximo 50 caracteres.";
                  }
              }

              if (dr["USUSENHA"] != null && !string.IsNullOrEmpty(dr["USUSENHA"].ToString()))
              {
                  if (dr["USUSENHA"].ToString().Length > 40)
                  {
                      return "O Ususenha deve possuir no máximo 40 caracteres.";
                  }
              }

              if (dr["CARGA_HORARIA"] != null && !string.IsNullOrEmpty(dr["CARGA_HORARIA"].ToString()))
              {
                  if ((Convert.ToDecimal(dr["CARGA_HORARIA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["CARGA_HORARIA"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["CARGA_HORARIA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["CARGA_HORARIA"]).ToString("0.############").Split(',')[1].Length > 0))
                  {
                      return "O Carga_horaria deve ser um valor inteiro com 3 dígitos no máximo.";
                  }
              }

              if (dr["GI_Seplag"] != null && !string.IsNullOrEmpty(dr["GI_Seplag"].ToString()))
              {
                  if (dr["GI_Seplag"].ToString().Length > 1)
                  {
                      return "O Gi_seplag deve possuir no máximo 1 caractere.";
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

      public static Ly_candidato_docente Query(TConnection connection, string where, params object[] whereValues)
      {
          return Query(connection, false, where, whereValues);
      }

      public static Ly_candidato_docente Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues)
      {
          Ly_candidato_docente tab = new Ly_candidato_docente();
          tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
          return tab;
      }

      public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues)
      {
          Ly_candidato_docente tab = Query(connection, false, where, whereValues);
          return tab.Rows.Count > 0 ? tab.Rows[0] : null;
      }


      public sealed class RowCollection : TDataRowCollection
      {
          internal RowCollection(DataRowCollection rows)
              : base(rows)
          {
          }


          public Row Add(
            string @concurso, string @candidato, string @nome, DateTime? @dt_nasc, string @sexo,
            string @nome_mae, string @nome_pai, string @estado_civil,
            string @pais_nasc, string @nacionalidade, string @municipio_nasc, string @end_pais,
            string @cep, string @end_municipio, string @endereco, string @end_num, string @end_compl,
            string @bairro, string @fone, string @celular, string @e_mail, string @rg_tipo, string @rg_num,
            string @rg_emissor, string @rg_uf, DateTime? @rg_dtexp, string @cpf, string @pis_pasep,
            string @cprof_num, string @cprof_serie, string @cprof_uf, DateTime? @cprof_dtexp,
            string @categoria, string @nucleo, string @agrupamento_ingresso, decimal? @status,
            string @status_obs, DateTime? @dt_convocacao, DateTime? @dt_apresentacao, DateTime? @hora_apresentacao,
            string @cdrh, DateTime? @dt_cdrh, string @cqhi, DateTime? @dt_cqhi, string @remessa_documento,
            DateTime? @dt_remessa_documento, DateTime? @dt_proposta, string @usulogin, string @ususenha,
            decimal? @carga_horaria, string @gi_seplag, string @municipio_proc, string @finalizado, int @etniaid,
            int? @cotaidinscricao, int? @cotaidconvocacao, int? @idfuncional, DateTime? @datavalidacaoinscricao, int? @regionalid,
            int? @necessidadeespecialid
          )
          {
              return (Row)Add(new object[] {
          @concurso, @candidato, @nome, @dt_nasc, @sexo, @nome_mae, 
          @nome_pai, @estado_civil, @pais_nasc, @nacionalidade, @municipio_nasc, @end_pais, 
          @cep, @end_municipio, @endereco, @end_num, @end_compl, @bairro, @fone, @celular, 
          @e_mail, @rg_tipo, @rg_num, @rg_emissor, @rg_uf, @rg_dtexp, @cpf, @pis_pasep, @cprof_num, 
          @cprof_serie, @cprof_uf, @cprof_dtexp, @categoria, @nucleo, @agrupamento_ingresso, 
          @status, @status_obs, @dt_convocacao, @dt_apresentacao, @hora_apresentacao, @cdrh, 
          @dt_cdrh, @cqhi, @dt_cqhi, @remessa_documento, @dt_remessa_documento, @dt_proposta, 
          @usulogin, @ususenha, @carga_horaria, @gi_seplag, @municipio_proc, @finalizado, @etniaid, 
          @cotaidinscricao, @cotaidconvocacao, @idfuncional, @datavalidacaoinscricao, @regionalid, @necessidadeespecialid
        });
          }

          public Row Find(
            string @concurso, string @candidato
          )
          {
              return (Row)Find(new object[] {
          @concurso, @candidato
        });
          }


          public new Row this[int index] { get { return (Row)base[index]; } }

          public void Add(Row row) { base.Add(row); }

          public void InsertAt(Row row, int pos) { base.InsertAt(row, pos); }

          public void Remove(Row row) { base.Remove(row); }

      }

      public sealed class Row : TDataRow
      {
          private OriginalValues original;

          internal Row(DataRowBuilder rb)
              : base(rb)
          {
              this.original = new OriginalValues(this);
          }


          /// <summary>
          /// CONCURSO: VARCHAR(20), Primary Key, Not Null
          /// </summary>
          public string Concurso { get { return (string)this["concurso"]; } set { this["concurso"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// CANDIDATO: VARCHAR(20), Primary Key, Not Null
          /// </summary>
          public string Candidato { get { return (string)this["candidato"]; } set { this["candidato"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// NOME: VARCHAR(100), Not Null
          /// </summary>
          public string Nome { get { return (string)this["nome"]; } set { this["nome"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// DT_NASC: DATETIME, Not Null
          /// </summary>
          public DateTime? Dt_nasc { get { return (DateTime?)this["dt_nasc"]; } set { this["dt_nasc"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// SEXO: CHAR(1), Not Null
          /// </summary>
          public string Sexo { get { return (string)this["sexo"]; } set { this["sexo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// NOME_MAE: VARCHAR(100)
          /// </summary>
          public string Nome_mae { get { return (string)this["nome_mae"]; } set { this["nome_mae"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// NOME_PAI: VARCHAR(100)
          /// </summary>
          public string Nome_pai { get { return (string)this["nome_pai"]; } set { this["nome_pai"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// ESTADO_CIVIL: VARCHAR(40), Not Null
          /// </summary>
          public string Estado_civil { get { return (string)this["estado_civil"]; } set { this["estado_civil"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// PAIS_NASC: VARCHAR(50), Not Null
          /// </summary>
          public string Pais_nasc { get { return (string)this["pais_nasc"]; } set { this["pais_nasc"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// NACIONALIDADE: VARCHAR(15), Not Null
          /// </summary>
          public string Nacionalidade { get { return (string)this["nacionalidade"]; } set { this["nacionalidade"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// MUNICIPIO_NASC: VARCHAR(20), Not Null
          /// </summary>
          public string Municipio_nasc { get { return (string)this["municipio_nasc"]; } set { this["municipio_nasc"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// END_PAIS: VARCHAR(50), Not Null
          /// </summary>
          public string End_pais { get { return (string)this["end_pais"]; } set { this["end_pais"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// CEP: VARCHAR(9), Not Null
          /// </summary>
          public string Cep { get { return (string)this["cep"]; } set { this["cep"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// END_MUNICIPIO: VARCHAR(20), Not Null
          /// </summary>
          public string End_municipio { get { return (string)this["end_municipio"]; } set { this["end_municipio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// ENDERECO: VARCHAR(50), Not Null
          /// </summary>
          public string Endereco { get { return (string)this["endereco"]; } set { this["endereco"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// END_NUM: VARCHAR(15), Not Null
          /// </summary>
          public string End_num { get { return (string)this["end_num"]; } set { this["end_num"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// END_COMPL: VARCHAR(50)
          /// </summary>
          public string End_compl { get { return (string)this["end_compl"]; } set { this["end_compl"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// BAIRRO: VARCHAR(50), Not Null
          /// </summary>
          public string Bairro { get { return (string)this["bairro"]; } set { this["bairro"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// FONE: VARCHAR(30)
          /// </summary>
          public string Fone { get { return (string)this["fone"]; } set { this["fone"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// CELULAR: VARCHAR(30)
          /// </summary>
          public string Celular { get { return (string)this["celular"]; } set { this["celular"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// E_MAIL: VARCHAR(100)
          /// </summary>
          public string E_mail { get { return (string)this["e_mail"]; } set { this["e_mail"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// RG_TIPO: VARCHAR(15), Not Null
          /// </summary>
          public string Rg_tipo { get { return (string)this["rg_tipo"]; } set { this["rg_tipo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// RG_NUM: VARCHAR(20), Not Null
          /// </summary>
          public string Rg_num { get { return (string)this["rg_num"]; } set { this["rg_num"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// RG_EMISSOR: VARCHAR(15), Not Null
          /// </summary>
          public string Rg_emissor { get { return (string)this["rg_emissor"]; } set { this["rg_emissor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// RG_UF: VARCHAR(2)
          /// </summary>
          public string Rg_uf { get { return (string)this["rg_uf"]; } set { this["rg_uf"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// RG_DTEXP: DATETIME
          /// </summary>
          public DateTime? Rg_dtexp { get { return (DateTime?)this["rg_dtexp"]; } set { this["rg_dtexp"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// CPF: VARCHAR(14), Not Null
          /// </summary>
          public string Cpf { get { return (string)this["cpf"]; } set { this["cpf"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// PIS_PASEP: VARCHAR(50)
          /// </summary>
          public string Pis_pasep { get { return (string)this["pis_pasep"]; } set { this["pis_pasep"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// CPROF_NUM: VARCHAR(15)
          /// </summary>
          public string Cprof_num { get { return (string)this["cprof_num"]; } set { this["cprof_num"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// CPROF_SERIE: VARCHAR(15)
          /// </summary>
          public string Cprof_serie { get { return (string)this["cprof_serie"]; } set { this["cprof_serie"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// CPROF_UF: VARCHAR(2)
          /// </summary>
          public string Cprof_uf { get { return (string)this["cprof_uf"]; } set { this["cprof_uf"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// CPROF_DTEXP: DATETIME
          /// </summary>
          public DateTime? Cprof_dtexp { get { return (DateTime?)this["cprof_dtexp"]; } set { this["cprof_dtexp"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// CATEGORIA: VARCHAR(20)
          /// </summary>
          public string Categoria { get { return (string)this["categoria"]; } set { this["categoria"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// NUCLEO: VARCHAR(20)
          /// </summary>
          public string Nucleo { get { return (string)this["nucleo"]; } set { this["nucleo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// AGRUPAMENTO_INGRESSO: VARCHAR(50)
          /// </summary>
          public string Agrupamento_ingresso { get { return (string)this["agrupamento_ingresso"]; } set { this["agrupamento_ingresso"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// STATUS: NUMERIC(3), Not Null
          /// </summary>
          public decimal? Status { get { return (decimal?)this["status"]; } set { this["status"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// STATUS_OBS: VARCHAR(2000)
          /// </summary>
          public string Status_obs { get { return (string)this["status_obs"]; } set { this["status_obs"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// DT_CONVOCACAO: DATETIME
          /// </summary>
          public DateTime? Dt_convocacao { get { return (DateTime?)this["dt_convocacao"]; } set { this["dt_convocacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// DT_APRESENTACAO: DATETIME
          /// </summary>
          public DateTime? Dt_apresentacao { get { return (DateTime?)this["dt_apresentacao"]; } set { this["dt_apresentacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// HORA_APRESENTACAO: DATETIME
          /// </summary>
          public DateTime? Hora_apresentacao { get { return (DateTime?)this["hora_apresentacao"]; } set { this["hora_apresentacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// CDRH: VARCHAR(50)
          /// </summary>
          public string Cdrh { get { return (string)this["cdrh"]; } set { this["cdrh"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// DT_CDRH: DATETIME
          /// </summary>
          public DateTime? Dt_cdrh { get { return (DateTime?)this["dt_cdrh"]; } set { this["dt_cdrh"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// CQHI: VARCHAR(50)
          /// </summary>
          public string Cqhi { get { return (string)this["cqhi"]; } set { this["cqhi"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// DT_CQHI: DATETIME
          /// </summary>
          public DateTime? Dt_cqhi { get { return (DateTime?)this["dt_cqhi"]; } set { this["dt_cqhi"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// REMESSA_DOCUMENTO: VARCHAR(50)
          /// </summary>
          public string Remessa_documento { get { return (string)this["remessa_documento"]; } set { this["remessa_documento"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// DT_REMESSA_DOCUMENTO: DATETIME
          /// </summary>
          public DateTime? Dt_remessa_documento { get { return (DateTime?)this["dt_remessa_documento"]; } set { this["dt_remessa_documento"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// DT_PROPOSTA: DATETIME
          /// </summary>
          public DateTime? Dt_proposta { get { return (DateTime?)this["dt_proposta"]; } set { this["dt_proposta"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// USULOGIN: VARCHAR(50)
          /// </summary>
          public string Usulogin { get { return (string)this["usulogin"]; } set { this["usulogin"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// USUSENHA: VARCHAR(40)
          /// </summary>
          public string Ususenha { get { return (string)this["ususenha"]; } set { this["ususenha"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// CARGA_HORARIA: NUMERIC(3)
          /// </summary>
          public decimal? Carga_horaria { get { return (decimal?)this["carga_horaria"]; } set { this["carga_horaria"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// GI_Seplag: VARCHAR(1)
          /// </summary>
          public string Gi_seplag { get { return (string)this["gi_seplag"]; } set { this["gi_seplag"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// MUNICIPIO_PROC: VARCHAR(20)
          /// </summary>
          public string Municipio_proc { get { return (string)this["municipio_proc"]; } set { this["municipio_proc"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// FINALIZADO: CHAR(1)
          /// </summary>
          public string Finalizado { get { return (string)this["finalizado"]; } set { this["finalizado"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }

          /// <summary>
          /// ETNIAID: TINYINT, NOT NULL
          /// </summary>
          public int EtniaId { get { return (int)this["etniaid"]; } set { this["etniaid"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }

          /// <summary>
          /// COTAIDINSCRICAO: TINYINT
          /// </summary>
          public int? CotaIdInscricao { get { return (int)this["cotaidinscricao"]; } set { this["cotaidinscricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }

          /// <summary>
          /// COTAIDCONVOCACAO: TINYINT
          /// </summary>
          public int? CotaIdConvocacao { get { return (int)this["cotaidconvocacao"]; } set { this["cotaidconvocacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }

          /// <summary>
          /// IDFUNCIONAL: INT
          /// </summary>
          public int? IdFuncional { get { return (int?)this["idfuncional"]; } set { this["idfuncional"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }

          /// <summary>
          /// DATAVALIDACAOINSCRICAO: DATETIME
          /// </summary>
          public DateTime? DataValidacaoInscricao { get { return (DateTime?)this["datavalidacaoinscricao"]; } set { this["datavalidacaoinscricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }

          /// <summary>
          /// REGIONALID: INT
          /// </summary>
          public int? RegionalId { get { return (int?)this["regionalid"]; } set { this["regionalid"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


          /// <summary>
          /// NECESSIDADEESPECIALID: INT
          /// </summary>
          public int? NecessidadeEspecialId { get { return (int?)this["necessidadeespecialid"]; } set { this["necessidadeespecialid"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
 

          public OriginalValues Original { get { return original; } }




          public static Row Insert(TConnectionWritable connection,
                                   string @concurso, string @candidato, string @nome, DateTime? @dt_nasc, string @sexo,
                                   string @estado_civil, string @pais_nasc, string @nacionalidade, string @municipio_nasc,
                                   string @end_pais, string @cep, string @end_municipio, string @endereco, string @end_num,
                                   string @bairro, string @rg_tipo, string @rg_num, string @rg_emissor, string @cpf,
                                   decimal? @status)
          {
              return Insert(connection,
                            @concurso, @candidato, @nome, @dt_nasc, @sexo, @estado_civil, @pais_nasc, @nacionalidade,
                            @municipio_nasc, @end_pais, @cep, @end_municipio, @endereco, @end_num, @bairro, @rg_tipo,
                            @rg_num, @rg_emissor, @cpf, @status,
                            null);
          }

          public static Row Insert(TConnectionWritable connection,
                                   string @concurso, string @candidato, string @nome, DateTime? @dt_nasc, string @sexo,
                                   string @estado_civil, string @pais_nasc, string @nacionalidade, string @municipio_nasc,
                                   string @end_pais, string @cep, string @end_municipio, string @endereco, string @end_num,
                                   string @bairro, string @rg_tipo, string @rg_num, string @rg_emissor, string @cpf,
                                   decimal? @status,
                                   string columnList, params object[] values)
          {
              return (Row)Insert(
                connection, new Techne.Lyceum.CR.Ly_candidato_docente(),
                new string[] { "concurso", "candidato", "nome", "dt_nasc", "sexo", "estado_civil", 
                         "pais_nasc", "nacionalidade", "municipio_nasc", "end_pais", "cep", "end_municipio", 
                         "endereco", "end_num", "bairro", "rg_tipo", "rg_num", "rg_emissor", "cpf", "status" },
                new object[] { @concurso, @candidato, @nome, @dt_nasc, @sexo, @estado_civil, @pais_nasc, 
                         @nacionalidade, @municipio_nasc, @end_pais, @cep, @end_municipio, @endereco, @end_num, 
                         @bairro, @rg_tipo, @rg_num, @rg_emissor, @cpf, @status },
                columnList, values
              );
          }

          public static Row Update(TConnectionWritable connection,
                                   string pkConcurso, string pkCandidato,
                                   string columnList, params object[] values)
          {
              return (Row)Update(
                connection, new Techne.Lyceum.CR.Ly_candidato_docente(),
                new string[] { "concurso", "candidato" },
                new object[] { pkConcurso, pkCandidato },
                columnList, values
              );
          }

          public static bool Delete(TConnectionWritable connection,
                                    string pkConcurso, string pkCandidato)
          {
              return Delete(
                connection, new Techne.Lyceum.CR.Ly_candidato_docente(),
                new string[] { "concurso", "candidato" },
                new object[] { pkConcurso, pkCandidato }
              );
          }


          /// <summary>
          /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
          /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
          /// Dá exception se mais de um registro for encontrado.
          /// </summary>
          /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
          public static Row Query(TConnection connection,
                                  string pkConcurso, string pkCandidato)
          {
              return (Row)Query(
                connection, new Techne.Lyceum.CR.Ly_candidato_docente(),
                new string[] { "concurso", "candidato" },
                new object[] { pkConcurso, pkCandidato }
              );
          }


          public struct OriginalValues
          {
              private Row row;

              internal OriginalValues(Row row)
              {
                  this.row = row;
              }


              public string Concurso { get { return (string)row["concurso", DataRowVersion.Original]; } }
              public string Candidato { get { return (string)row["candidato", DataRowVersion.Original]; } }
              public string Nome { get { return (string)row["nome", DataRowVersion.Original]; } }
              public DateTime? Dt_nasc { get { return (DateTime?)row["dt_nasc", DataRowVersion.Original]; } }
              public string Sexo { get { return (string)row["sexo", DataRowVersion.Original]; } }
              public string Nome_mae { get { return (string)row["nome_mae", DataRowVersion.Original]; } }
              public string Nome_pai { get { return (string)row["nome_pai", DataRowVersion.Original]; } }
              public string Estado_civil { get { return (string)row["estado_civil", DataRowVersion.Original]; } }
              public string Pais_nasc { get { return (string)row["pais_nasc", DataRowVersion.Original]; } }
              public string Nacionalidade { get { return (string)row["nacionalidade", DataRowVersion.Original]; } }
              public string Municipio_nasc { get { return (string)row["municipio_nasc", DataRowVersion.Original]; } }
              public string End_pais { get { return (string)row["end_pais", DataRowVersion.Original]; } }
              public string Cep { get { return (string)row["cep", DataRowVersion.Original]; } }
              public string End_municipio { get { return (string)row["end_municipio", DataRowVersion.Original]; } }
              public string Endereco { get { return (string)row["endereco", DataRowVersion.Original]; } }
              public string End_num { get { return (string)row["end_num", DataRowVersion.Original]; } }
              public string End_compl { get { return (string)row["end_compl", DataRowVersion.Original]; } }
              public string Bairro { get { return (string)row["bairro", DataRowVersion.Original]; } }
              public string Fone { get { return (string)row["fone", DataRowVersion.Original]; } }
              public string Celular { get { return (string)row["celular", DataRowVersion.Original]; } }
              public string E_mail { get { return (string)row["e_mail", DataRowVersion.Original]; } }
              public string Rg_tipo { get { return (string)row["rg_tipo", DataRowVersion.Original]; } }
              public string Rg_num { get { return (string)row["rg_num", DataRowVersion.Original]; } }
              public string Rg_emissor { get { return (string)row["rg_emissor", DataRowVersion.Original]; } }
              public string Rg_uf { get { return (string)row["rg_uf", DataRowVersion.Original]; } }
              public DateTime? Rg_dtexp { get { return (DateTime?)row["rg_dtexp", DataRowVersion.Original]; } }
              public string Cpf { get { return (string)row["cpf", DataRowVersion.Original]; } }
              public string Pis_pasep { get { return (string)row["pis_pasep", DataRowVersion.Original]; } }
              public string Cprof_num { get { return (string)row["cprof_num", DataRowVersion.Original]; } }
              public string Cprof_serie { get { return (string)row["cprof_serie", DataRowVersion.Original]; } }
              public string Cprof_uf { get { return (string)row["cprof_uf", DataRowVersion.Original]; } }
              public DateTime? Cprof_dtexp { get { return (DateTime?)row["cprof_dtexp", DataRowVersion.Original]; } }
              public string Categoria { get { return (string)row["categoria", DataRowVersion.Original]; } }
              public string Nucleo { get { return (string)row["nucleo", DataRowVersion.Original]; } }
              public string Agrupamento_ingresso { get { return (string)row["agrupamento_ingresso", DataRowVersion.Original]; } }
              public decimal? Status { get { return (decimal?)row["status", DataRowVersion.Original]; } }
              public string Status_obs { get { return (string)row["status_obs", DataRowVersion.Original]; } }
              public DateTime? Dt_convocacao { get { return (DateTime?)row["dt_convocacao", DataRowVersion.Original]; } }
              public DateTime? Dt_apresentacao { get { return (DateTime?)row["dt_apresentacao", DataRowVersion.Original]; } }
              public DateTime? Hora_apresentacao { get { return (DateTime?)row["hora_apresentacao", DataRowVersion.Original]; } }
              public string Cdrh { get { return (string)row["cdrh", DataRowVersion.Original]; } }
              public DateTime? Dt_cdrh { get { return (DateTime?)row["dt_cdrh", DataRowVersion.Original]; } }
              public string Cqhi { get { return (string)row["cqhi", DataRowVersion.Original]; } }
              public DateTime? Dt_cqhi { get { return (DateTime?)row["dt_cqhi", DataRowVersion.Original]; } }
              public string Remessa_documento { get { return (string)row["remessa_documento", DataRowVersion.Original]; } }
              public DateTime? Dt_remessa_documento { get { return (DateTime?)row["dt_remessa_documento", DataRowVersion.Original]; } }
              public DateTime? Dt_proposta { get { return (DateTime?)row["dt_proposta", DataRowVersion.Original]; } }
              public string Usulogin { get { return (string)row["usulogin", DataRowVersion.Original]; } }
              public string Ususenha { get { return (string)row["ususenha", DataRowVersion.Original]; } }
              public decimal? Carga_horaria { get { return (decimal?)row["carga_horaria", DataRowVersion.Original]; } }
              public string Gi_seplag { get { return (string)row["gi_seplag", DataRowVersion.Original]; } }
              public string Municipio_proc { get { return (string)row["municipio_proc", DataRowVersion.Original]; } }
              public string Finalizado { get { return (string)row["finalizado", DataRowVersion.Original]; } }
              public int EtniaId { get { return (int)row["etniaid", DataRowVersion.Original]; } }
              public int? CotaIdInscricao { get { return (int?)row["cotaidinscricao", DataRowVersion.Original]; } }
              public int? CotaIdConvocacao { get { return (int?)row["cotaidconvocacao", DataRowVersion.Original]; } }
              public int? IdFuncional { get { return (int?)row["idfuncional", DataRowVersion.Original]; } }
              public DateTime? DataValidacaoInscricao { get { return (DateTime?)row["datavalidacaoinscricao", DataRowVersion.Original]; } }
              public int? RegionalId { get { return (int?)row["regionalid", DataRowVersion.Original]; } }
              public int? NecessidadeEspecialId { get { return (int?)row["necessidadeespecialid", DataRowVersion.Original]; } }
          }

      }

      public abstract class CustomBase
      {
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
      #region Coluna: CONCURSO
      private ColumnConcurso pvColConcurso;

      public sealed class ColumnConcurso : TDataColumn
      {

          public ColumnConcurso()
          {
              this.ColumnName = "concurso";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "concurso"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.concurso"; } }
          public override string OriginalCol { get { return "concurso"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.CONCURSO"; } }
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

      #region Coluna: CANDIDATO
      private ColumnCandidato pvColCandidato;

      public sealed class ColumnCandidato : TDataColumn
      {

          public ColumnCandidato()
          {
              this.ColumnName = "candidato";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "candidato"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.candidato"; } }
          public override string OriginalCol { get { return "candidato"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.CANDIDATO"; } }
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

      public sealed class ColumnNome : TDataColumn
      {

          public ColumnNome()
          {
              this.ColumnName = "nome";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "nome"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.nome"; } }
          public override string OriginalCol { get { return "nome"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.NOME"; } }
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

      #region Coluna: DT_NASC
      private ColumnDt_nasc pvColDt_nasc;

      public sealed class ColumnDt_nasc : TDataColumn
      {

          public ColumnDt_nasc()
          {
              this.ColumnName = "dt_nasc";
              this.DataType = typeof(DateTime);

          }


          protected override string GetName() { return "data de nascimento"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.dt_nasc"; } }
          public override string OriginalCol { get { return "dt_nasc"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.DT_NASC"; } }
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

      #region Coluna: SEXO
      private ColumnSexo pvColSexo;

      public sealed class ColumnSexo : TDataColumn
      {

          public ColumnSexo()
          {
              this.ColumnName = "sexo";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "sexo"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.sexo"; } }
          public override string OriginalCol { get { return "sexo"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.SEXO"; } }
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

      #region Coluna: NOME_MAE
      private ColumnNome_mae pvColNome_mae;

      public sealed class ColumnNome_mae : TDataColumn
      {

          public ColumnNome_mae()
          {
              this.ColumnName = "nome_mae";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "nome da mãe"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.nome_mae"; } }
          public override string OriginalCol { get { return "nome_mae"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.NOME_MAE"; } }
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

      #region Coluna: NOME_PAI
      private ColumnNome_pai pvColNome_pai;

      public sealed class ColumnNome_pai : TDataColumn
      {

          public ColumnNome_pai()
          {
              this.ColumnName = "nome_pai";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "nome do pai"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.nome_pai"; } }
          public override string OriginalCol { get { return "nome_pai"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.NOME_PAI"; } }
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

      #region Coluna: ESTADO_CIVIL
      private ColumnEstado_civil pvColEstado_civil;

      public sealed class ColumnEstado_civil : TDataColumn
      {

          public ColumnEstado_civil()
          {
              this.ColumnName = "estado_civil";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "estado civil"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.estado_civil"; } }
          public override string OriginalCol { get { return "estado_civil"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.ESTADO_CIVIL"; } }
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

      #region Coluna: PAIS_NASC
      private ColumnPais_nasc pvColPais_nasc;

      public sealed class ColumnPais_nasc : TDataColumn
      {

          public ColumnPais_nasc()
          {
              this.ColumnName = "pais_nasc";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "país de nascimento"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.pais_nasc"; } }
          public override string OriginalCol { get { return "pais_nasc"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.PAIS_NASC"; } }
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

      #region Coluna: NACIONALIDADE
      private ColumnNacionalidade pvColNacionalidade;

      public sealed class ColumnNacionalidade : TDataColumn
      {

          public ColumnNacionalidade()
          {
              this.ColumnName = "nacionalidade";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "nacionalidade"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.nacionalidade"; } }
          public override string OriginalCol { get { return "nacionalidade"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.NACIONALIDADE"; } }
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

      #region Coluna: MUNICIPIO_NASC
      private ColumnMunicipio_nasc pvColMunicipio_nasc;

      public sealed class ColumnMunicipio_nasc : TDataColumn
      {

          public ColumnMunicipio_nasc()
          {
              this.ColumnName = "municipio_nasc";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "município de nascimento"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.municipio_nasc"; } }
          public override string OriginalCol { get { return "municipio_nasc"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.MUNICIPIO_NASC"; } }
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

      #region Coluna: END_PAIS
      private ColumnEnd_pais pvColEnd_pais;

      public sealed class ColumnEnd_pais : TDataColumn
      {

          public ColumnEnd_pais()
          {
              this.ColumnName = "end_pais";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "país do endereço"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.end_pais"; } }
          public override string OriginalCol { get { return "end_pais"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.END_PAIS"; } }
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

      #region Coluna: CEP
      private ColumnCep pvColCep;

      public sealed class ColumnCep : TDataColumn
      {

          public ColumnCep()
          {
              this.ColumnName = "cep";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "cep"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.cep"; } }
          public override string OriginalCol { get { return "cep"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.CEP"; } }
          public override bool PrimaryKey { get { return false; } }
          public override bool IsAux { get { return false; } }
          /// <summary>
          /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
          /// </summary>
          public override bool NotNull { get { return true; } }
          public override int Size { get { return 9; } }
          public override string SqlServerType { get { return "VARCHAR(9)"; } }

      }
      #endregion

      #region Coluna: END_MUNICIPIO
      private ColumnEnd_municipio pvColEnd_municipio;

      public sealed class ColumnEnd_municipio : TDataColumn
      {

          public ColumnEnd_municipio()
          {
              this.ColumnName = "end_municipio";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "município do endereço"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.end_municipio"; } }
          public override string OriginalCol { get { return "end_municipio"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.END_MUNICIPIO"; } }
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

      #region Coluna: ENDERECO
      private ColumnEndereco pvColEndereco;

      public sealed class ColumnEndereco : TDataColumn
      {

          public ColumnEndereco()
          {
              this.ColumnName = "endereco";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "endereço"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.endereco"; } }
          public override string OriginalCol { get { return "endereco"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.ENDERECO"; } }
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

      #region Coluna: END_NUM
      private ColumnEnd_num pvColEnd_num;

      public sealed class ColumnEnd_num : TDataColumn
      {

          public ColumnEnd_num()
          {
              this.ColumnName = "end_num";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "número do endereço"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.end_num"; } }
          public override string OriginalCol { get { return "end_num"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.END_NUM"; } }
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

      #region Coluna: END_COMPL
      private ColumnEnd_compl pvColEnd_compl;

      public sealed class ColumnEnd_compl : TDataColumn
      {

          public ColumnEnd_compl()
          {
              this.ColumnName = "end_compl";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "complemento do endereço"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.end_compl"; } }
          public override string OriginalCol { get { return "end_compl"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.END_COMPL"; } }
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

      #region Coluna: BAIRRO
      private ColumnBairro pvColBairro;

      public sealed class ColumnBairro : TDataColumn
      {

          public ColumnBairro()
          {
              this.ColumnName = "bairro";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "bairro"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.bairro"; } }
          public override string OriginalCol { get { return "bairro"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.BAIRRO"; } }
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

      #region Coluna: FONE
      private ColumnFone pvColFone;

      public sealed class ColumnFone : TDataColumn
      {

          public ColumnFone()
          {
              this.ColumnName = "fone";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "telefone"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.fone"; } }
          public override string OriginalCol { get { return "fone"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.FONE"; } }
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

      public sealed class ColumnCelular : TDataColumn
      {

          public ColumnCelular()
          {
              this.ColumnName = "celular";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "celular"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.celular"; } }
          public override string OriginalCol { get { return "celular"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.CELULAR"; } }
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

      #region Coluna: E_MAIL
      private ColumnE_mail pvColE_mail;

      public sealed class ColumnE_mail : TDataColumn
      {

          public ColumnE_mail()
          {
              this.ColumnName = "e_mail";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "email"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.e_mail"; } }
          public override string OriginalCol { get { return "e_mail"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.E_MAIL"; } }
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

      #region Coluna: RG_TIPO
      private ColumnRg_tipo pvColRg_tipo;

      public sealed class ColumnRg_tipo : TDataColumn
      {

          public ColumnRg_tipo()
          {
              this.ColumnName = "rg_tipo";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "tipo do documento de identificação"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.rg_tipo"; } }
          public override string OriginalCol { get { return "rg_tipo"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.RG_TIPO"; } }
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

      #region Coluna: RG_NUM
      private ColumnRg_num pvColRg_num;

      public sealed class ColumnRg_num : TDataColumn
      {

          public ColumnRg_num()
          {
              this.ColumnName = "rg_num";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "número do documento de identificação"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.rg_num"; } }
          public override string OriginalCol { get { return "rg_num"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.RG_NUM"; } }
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

      #region Coluna: RG_EMISSOR
      private ColumnRg_emissor pvColRg_emissor;

      public sealed class ColumnRg_emissor : TDataColumn
      {

          public ColumnRg_emissor()
          {
              this.ColumnName = "rg_emissor";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "órgão emissor do documento de identificação"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.rg_emissor"; } }
          public override string OriginalCol { get { return "rg_emissor"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.RG_EMISSOR"; } }
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

      #region Coluna: RG_UF
      private ColumnRg_uf pvColRg_uf;

      public sealed class ColumnRg_uf : TDataColumn
      {

          public ColumnRg_uf()
          {
              this.ColumnName = "rg_uf";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "estado do documento de identificação"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.rg_uf"; } }
          public override string OriginalCol { get { return "rg_uf"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.RG_UF"; } }
          public override bool PrimaryKey { get { return false; } }
          public override bool IsAux { get { return false; } }
          /// <summary>
          /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
          /// </summary>
          public override bool NotNull { get { return false; } }
          public override int Size { get { return 2; } }
          public override string SqlServerType { get { return "VARCHAR(2)"; } }

      }
      #endregion

      #region Coluna: RG_DTEXP
      private ColumnRg_dtexp pvColRg_dtexp;

      public sealed class ColumnRg_dtexp : TDataColumn
      {

          public ColumnRg_dtexp()
          {
              this.ColumnName = "rg_dtexp";
              this.DataType = typeof(DateTime);

          }


          protected override string GetName() { return "data de expedição do documto de identificação"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.rg_dtexp"; } }
          public override string OriginalCol { get { return "rg_dtexp"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.RG_DTEXP"; } }
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

      #region Coluna: CPF
      private ColumnCpf pvColCpf;

      public sealed class ColumnCpf : TDataColumn
      {

          public ColumnCpf()
          {
              this.ColumnName = "cpf";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "cpf"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.cpf"; } }
          public override string OriginalCol { get { return "cpf"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.CPF"; } }
          public override bool PrimaryKey { get { return false; } }
          public override bool IsAux { get { return false; } }
          /// <summary>
          /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
          /// </summary>
          public override bool NotNull { get { return true; } }
          public override int Size { get { return 14; } }
          public override string SqlServerType { get { return "VARCHAR(14)"; } }

      }
      #endregion

      #region Coluna: PIS_PASEP
      private ColumnPis_pasep pvColPis_pasep;

      public sealed class ColumnPis_pasep : TDataColumn
      {

          public ColumnPis_pasep()
          {
              this.ColumnName = "pis_pasep";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "pis/pasep"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.pis_pasep"; } }
          public override string OriginalCol { get { return "pis_pasep"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.PIS_PASEP"; } }
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

      #region Coluna: CPROF_NUM
      private ColumnCprof_num pvColCprof_num;

      public sealed class ColumnCprof_num : TDataColumn
      {

          public ColumnCprof_num()
          {
              this.ColumnName = "cprof_num";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "número da carteira profissional"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.cprof_num"; } }
          public override string OriginalCol { get { return "cprof_num"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.CPROF_NUM"; } }
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

      #region Coluna: CPROF_SERIE
      private ColumnCprof_serie pvColCprof_serie;

      public sealed class ColumnCprof_serie : TDataColumn
      {

          public ColumnCprof_serie()
          {
              this.ColumnName = "cprof_serie";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "número de série da carteira profissional"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.cprof_serie"; } }
          public override string OriginalCol { get { return "cprof_serie"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.CPROF_SERIE"; } }
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

      #region Coluna: CPROF_UF
      private ColumnCprof_uf pvColCprof_uf;

      public sealed class ColumnCprof_uf : TDataColumn
      {

          public ColumnCprof_uf()
          {
              this.ColumnName = "cprof_uf";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "estado da carteira profissional"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.cprof_uf"; } }
          public override string OriginalCol { get { return "cprof_uf"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.CPROF_UF"; } }
          public override bool PrimaryKey { get { return false; } }
          public override bool IsAux { get { return false; } }
          /// <summary>
          /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
          /// </summary>
          public override bool NotNull { get { return false; } }
          public override int Size { get { return 2; } }
          public override string SqlServerType { get { return "VARCHAR(2)"; } }

      }
      #endregion

      #region Coluna: CPROF_DTEXP
      private ColumnCprof_dtexp pvColCprof_dtexp;

      public sealed class ColumnCprof_dtexp : TDataColumn
      {

          public ColumnCprof_dtexp()
          {
              this.ColumnName = "cprof_dtexp";
              this.DataType = typeof(DateTime);

          }


          protected override string GetName() { return "data expedição da carteira profissional"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.cprof_dtexp"; } }
          public override string OriginalCol { get { return "cprof_dtexp"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.CPROF_DTEXP"; } }
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

      #region Coluna: CATEGORIA
      private ColumnCategoria pvColCategoria;

      public sealed class ColumnCategoria : TDataColumn
      {

          public ColumnCategoria()
          {
              this.ColumnName = "categoria";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "cargo"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.categoria"; } }
          public override string OriginalCol { get { return "categoria"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.CATEGORIA"; } }
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

      #region Coluna: NUCLEO
      private ColumnNucleo pvColNucleo;

      public sealed class ColumnNucleo : TDataColumn
      {

          public ColumnNucleo()
          {
              this.ColumnName = "nucleo";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "coordenadoria"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.nucleo"; } }
          public override string OriginalCol { get { return "nucleo"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.NUCLEO"; } }
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

      #region Coluna: AGRUPAMENTO_INGRESSO
      private ColumnAgrupamento_ingresso pvColAgrupamento_ingresso;

      public sealed class ColumnAgrupamento_ingresso : TDataColumn
      {

          public ColumnAgrupamento_ingresso()
          {
              this.ColumnName = "agrupamento_ingresso";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "grupo de habilitação de ingresso"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.agrupamento_ingresso"; } }
          public override string OriginalCol { get { return "agrupamento_ingresso"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.AGRUPAMENTO_INGRESSO"; } }
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

      #region Coluna: STATUS
      private ColumnStatus pvColStatus;

      public sealed class ColumnStatus : TDataColumn
      {

          public ColumnStatus()
          {
              this.ColumnName = "status";
              this.DataType = typeof(decimal);

          }


          protected override string GetName() { return "status"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.status"; } }
          public override string OriginalCol { get { return "status"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.STATUS"; } }
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

      #region Coluna: STATUS_OBS
      private ColumnStatus_obs pvColStatus_obs;

      public sealed class ColumnStatus_obs : TDataColumn
      {

          public ColumnStatus_obs()
          {
              this.ColumnName = "status_obs";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "observação do status"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.status_obs"; } }
          public override string OriginalCol { get { return "status_obs"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.STATUS_OBS"; } }
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

      #region Coluna: DT_CONVOCACAO
      private ColumnDt_convocacao pvColDt_convocacao;

      public sealed class ColumnDt_convocacao : TDataColumn
      {

          public ColumnDt_convocacao()
          {
              this.ColumnName = "dt_convocacao";
              this.DataType = typeof(DateTime);

          }


          protected override string GetName() { return "data da convocação"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.dt_convocacao"; } }
          public override string OriginalCol { get { return "dt_convocacao"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.DT_CONVOCACAO"; } }
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

      #region Coluna: DT_APRESENTACAO
      private ColumnDt_apresentacao pvColDt_apresentacao;

      public sealed class ColumnDt_apresentacao : TDataColumn
      {

          public ColumnDt_apresentacao()
          {
              this.ColumnName = "dt_apresentacao";
              this.DataType = typeof(DateTime);

          }


          protected override string GetName() { return "data de apresentação"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.dt_apresentacao"; } }
          public override string OriginalCol { get { return "dt_apresentacao"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.DT_APRESENTACAO"; } }
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

      #region Coluna: HORA_APRESENTACAO
      private ColumnHora_apresentacao pvColHora_apresentacao;

      public sealed class ColumnHora_apresentacao : TDataColumn
      {

          public ColumnHora_apresentacao()
          {
              this.ColumnName = "hora_apresentacao";
              this.DataType = typeof(DateTime);

          }


          protected override string GetName() { return "hora da apresentação"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.hora_apresentacao"; } }
          public override string OriginalCol { get { return "hora_apresentacao"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.HORA_APRESENTACAO"; } }
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

      #region Coluna: CDRH
      private ColumnCdrh pvColCdrh;

      public sealed class ColumnCdrh : TDataColumn
      {

          public ColumnCdrh()
          {
              this.ColumnName = "cdrh";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "cdrh"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.cdrh"; } }
          public override string OriginalCol { get { return "cdrh"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.CDRH"; } }
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

      #region Coluna: DT_CDRH
      private ColumnDt_cdrh pvColDt_cdrh;

      public sealed class ColumnDt_cdrh : TDataColumn
      {

          public ColumnDt_cdrh()
          {
              this.ColumnName = "dt_cdrh";
              this.DataType = typeof(DateTime);

          }


          protected override string GetName() { return "data do cdrh"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.dt_cdrh"; } }
          public override string OriginalCol { get { return "dt_cdrh"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.DT_CDRH"; } }
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

      #region Coluna: CQHI
      private ColumnCqhi pvColCqhi;

      public sealed class ColumnCqhi : TDataColumn
      {

          public ColumnCqhi()
          {
              this.ColumnName = "cqhi";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "cqhi"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.cqhi"; } }
          public override string OriginalCol { get { return "cqhi"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.CQHI"; } }
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

      #region Coluna: DT_CQHI
      private ColumnDt_cqhi pvColDt_cqhi;

      public sealed class ColumnDt_cqhi : TDataColumn
      {

          public ColumnDt_cqhi()
          {
              this.ColumnName = "dt_cqhi";
              this.DataType = typeof(DateTime);

          }


          protected override string GetName() { return "data do cqhi"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.dt_cqhi"; } }
          public override string OriginalCol { get { return "dt_cqhi"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.DT_CQHI"; } }
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

      #region Coluna: REMESSA_DOCUMENTO
      private ColumnRemessa_documento pvColRemessa_documento;

      public sealed class ColumnRemessa_documento : TDataColumn
      {

          public ColumnRemessa_documento()
          {
              this.ColumnName = "remessa_documento";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "remessa de documentos"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.remessa_documento"; } }
          public override string OriginalCol { get { return "remessa_documento"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.REMESSA_DOCUMENTO"; } }
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

      #region Coluna: DT_REMESSA_DOCUMENTO
      private ColumnDt_remessa_documento pvColDt_remessa_documento;

      public sealed class ColumnDt_remessa_documento : TDataColumn
      {

          public ColumnDt_remessa_documento()
          {
              this.ColumnName = "dt_remessa_documento";
              this.DataType = typeof(DateTime);

          }


          protected override string GetName() { return "data da remessa do documento"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.dt_remessa_documento"; } }
          public override string OriginalCol { get { return "dt_remessa_documento"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.DT_REMESSA_DOCUMENTO"; } }
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

      #region Coluna: DT_PROPOSTA
      private ColumnDt_proposta pvColDt_proposta;

      public sealed class ColumnDt_proposta : TDataColumn
      {

          public ColumnDt_proposta()
          {
              this.ColumnName = "dt_proposta";
              this.DataType = typeof(DateTime);

          }


          protected override string GetName() { return "data da proposta"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.dt_proposta"; } }
          public override string OriginalCol { get { return "dt_proposta"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.DT_PROPOSTA"; } }
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

      #region Coluna: USULOGIN
      private ColumnUsulogin pvColUsulogin;

      public sealed class ColumnUsulogin : TDataColumn
      {

          public ColumnUsulogin()
          {
              this.ColumnName = "usulogin";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return ""; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.usulogin"; } }
          public override string OriginalCol { get { return "usulogin"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.USULOGIN"; } }
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

      #region Coluna: USUSENHA
      private ColumnUsusenha pvColUsusenha;

      public sealed class ColumnUsusenha : TDataColumn
      {

          public ColumnUsusenha()
          {
              this.ColumnName = "ususenha";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return ""; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.ususenha"; } }
          public override string OriginalCol { get { return "ususenha"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.USUSENHA"; } }
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

      #region Coluna: CARGA_HORARIA
      private ColumnCarga_horaria pvColCarga_horaria;

      public sealed class ColumnCarga_horaria : TDataColumn
      {

          public ColumnCarga_horaria()
          {
              this.ColumnName = "carga_horaria";
              this.DataType = typeof(decimal);

          }


          protected override string GetName() { return ""; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.carga_horaria"; } }
          public override string OriginalCol { get { return "carga_horaria"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.CARGA_HORARIA"; } }
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

      #region Coluna: GI_Seplag
      private ColumnGi_seplag pvColGi_seplag;

      public sealed class ColumnGi_seplag : TDataColumn
      {

          public ColumnGi_seplag()
          {
              this.ColumnName = "gi_seplag";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return ""; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.gi_seplag"; } }
          public override string OriginalCol { get { return "gi_seplag"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.GI_Seplag"; } }
          public override bool PrimaryKey { get { return false; } }
          public override bool IsAux { get { return false; } }
          /// <summary>
          /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
          /// </summary>
          public override bool NotNull { get { return false; } }
          public override int Size { get { return 1; } }
          public override string SqlServerType { get { return "VARCHAR(1)"; } }

      }
      #endregion

      #region Coluna: MUNICIPIO_PROC
      private ColumnMunicipio_Proc pvColMunicipio_Proc;

      public sealed class ColumnMunicipio_Proc : TDataColumn
      {

          public ColumnMunicipio_Proc()
          {
              this.ColumnName = "municipio_proc";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "municipio_proc"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.municipio_proc"; } }
          public override string OriginalCol { get { return "municipio_proc"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.MUNICIPIO_PROC"; } }
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

      #region Coluna: FINALIZADO
      private ColumnFinalizado pvColFinalizado;

      public sealed class ColumnFinalizado : TDataColumn
      {

          public ColumnFinalizado()
          {
              this.ColumnName = "finalizado";
              this.DataType = typeof(string);

          }

          protected override string GetName() { return "finalizado"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.finalizado"; } }
          public override string OriginalCol { get { return "finalizado"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.FINALIZADO"; } }
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

      #region Coluna: ETNIAID
      private ColumnEtniaID pvColEtniaId;

      public sealed class ColumnEtniaID : TDataColumn
      {

          public ColumnEtniaID()
          {
              this.ColumnName = "etniaid";
              this.DataType = typeof(int);

          }

          protected override string GetName() { return "etniaid"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.etniaid"; } }
          public override string OriginalCol { get { return "etniaid"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.ETNIAID"; } }
          public override bool PrimaryKey { get { return false; } }
          public override bool IsAux { get { return false; } }
          /// <summary>
          /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
          /// </summary>
          public override bool NotNull { get { return true; } }
          public override string SqlServerType { get { return "INT"; } }

      }
      #endregion

      #region Coluna: COTAIDINSCRICAO
      private ColumnCotaIdInscricao pvColCotaIdInscricao;

      public sealed class ColumnCotaIdInscricao : TDataColumn
      {

          public ColumnCotaIdInscricao()
          {
              this.ColumnName = "cotaidinscricao";
              this.DataType = typeof(int);

          }

          protected override string GetName() { return "cotaidinscricao"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.cotaidinscricao"; } }
          public override string OriginalCol { get { return "cotaidinscricao"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.COTAIDINSCRICAO"; } }
          public override bool PrimaryKey { get { return false; } }
          public override bool IsAux { get { return false; } }
          /// <summary>
          /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
          /// </summary>
          public override bool NotNull { get { return false; } }
          public override string SqlServerType { get { return "INT"; } }

      }
      #endregion

      #region Coluna: COTAIDCONVOCACAO
      private ColumnCotaIdConvocacao pvColCotaIdConvocacao;

      public sealed class ColumnCotaIdConvocacao : TDataColumn
      {

          public ColumnCotaIdConvocacao()
          {
              this.ColumnName = "cotaidconvocacao";
              this.DataType = typeof(int);

          }

          protected override string GetName() { return "cotaidconvocacao"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.cotaidconvocacao"; } }
          public override string OriginalCol { get { return "cotaidconvocacao"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.COTAIDCONVOCACAO"; } }
          public override bool PrimaryKey { get { return false; } }
          public override bool IsAux { get { return false; } }
          /// <summary>
          /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
          /// </summary>
          public override bool NotNull { get { return false; } }
          public override string SqlServerType { get { return "INT"; } }

      }
      #endregion

      #region Coluna: IDFUNCIONAL
      private ColumnIdFuncional pvColIdFuncional;

      public sealed class ColumnIdFuncional : TDataColumn
      {

          public ColumnIdFuncional()
          {
              this.ColumnName = "idfuncional";
              this.DataType = typeof(int);

          }

          protected override string GetName() { return "idfuncional"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.idfuncional"; } }
          public override string OriginalCol { get { return "idfuncional"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.IDFUNCIONAL"; } }
          public override bool PrimaryKey { get { return false; } }
          public override bool IsAux { get { return false; } }
          /// <summary>
          /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
          /// </summary>
          public override bool NotNull { get { return false; } }
          public override string SqlServerType { get { return "INT"; } }

      }
      #endregion

      #region Coluna: DTVALIDACAOINSCRICAO
      private ColumnDataValidacaoInscricao pvColDataValidacaoInscricao;

      public sealed class ColumnDataValidacaoInscricao : TDataColumn
      {

          public ColumnDataValidacaoInscricao()
          {
              this.ColumnName = "datavalidacaoinscricao";
              this.DataType = typeof(DateTime);

          }

          protected override string GetName() { return "datavalidacaoinscricao"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.datavalidacaoinscricao"; } }
          public override string OriginalCol { get { return "datavalidacaoinscricao"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.DATAVALIDACAOINSCRICAO"; } }
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

      #region Coluna: REGIONALID
      private ColumnRegionalId pvColRegionalId;

      public sealed class ColumnRegionalId : TDataColumn
      {

          public ColumnRegionalId()
          {
              this.ColumnName = "regionalid";
              this.DataType = typeof(int);

          }

          protected override string GetName() { return "regionalid"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.regionalid"; } }
          public override string OriginalCol { get { return "regionalid"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.REGIONALID"; } }
          public override bool PrimaryKey { get { return false; } }
          public override bool IsAux { get { return false; } }
          /// <summary>
          /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
          /// </summary>
          public override bool NotNull { get { return false; } }
          public override string SqlServerType { get { return "INT"; } }

      }
      #endregion

      #region Coluna: NECESSIDADEESPECIALID
      private ColumnNecessidadeEspecialId pvColNecessidadeEspecialId;

      public sealed class ColumnNecessidadeEspecialId : TDataColumn
      {

          public ColumnNecessidadeEspecialId()
          {
              this.ColumnName = "necessidadeespecialid";
              this.DataType = typeof(int);

          }


          protected override string GetName() { return "necessidade especial"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_candidato_docente.necessidadeespecialid"; } }
          public override string OriginalCol { get { return "necessidadeespecialid"; } }
          public override string RefCol { get { return "LY_CANDIDATO_DOCENTE.NECESSIDADEESPECIALID"; } }
          public override bool PrimaryKey { get { return false; } }
          public override bool IsAux { get { return false; } }
          /// <summary>
          /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
          /// </summary>
          public override bool NotNull { get { return false; } }
          public override string SqlServerType { get { return "INT"; } }

      }
      #endregion

      #endregion
  }

  public partial class Ly_concurso_doc_experiencia : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_concurso_doc_experiencia() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_concurso_doc_experiencia");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_concurso_doc_experiencia");
      }
      catch{}
    }

    public Ly_concurso_doc_experiencia() {
      pvColConcurso = new ColumnConcurso(); Columns.Add(pvColConcurso);
      pvColExperiencia = new ColumnExperiencia(); Columns.Add(pvColExperiencia);
      pvColPontuacao = new ColumnPontuacao(); Columns.Add(pvColPontuacao);
      pvColDescricao = new ColumnDescricao(); Columns.Add(pvColDescricao);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_concurso_doc_experiencia");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @concurso, string @experiencia, decimal? @pontuacao) {
      Row row = NewRow();

      row.Concurso = @concurso;
      row.Experiencia = @experiencia;
      row.Pontuacao = @pontuacao;

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
      get { return "LY_CONCURSO_DOC_EXPERIENCIA"; }
    }

    protected override sealed string Name { get { return "Experiências dos Concursos"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColConcurso, pvColExperiencia });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_concurso_doc_experie_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_concurso_doc_experie_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_concurso_doc_experie_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_concurso_doc_experie_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkConcurso", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "CONCURSO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkExperiencia", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "EXPERIENCIA", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("concurso", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CONCURSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("experiencia", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "EXPERIENCIA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("pontuacao", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 25, 10, "PONTUACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "concurso") { result.Add(toLower ? "concurso" : column); continue; }
          if(lower == "experiencia") { result.Add(toLower ? "experiencia" : column); continue; }
          if(lower == "pontuacao") { result.Add(toLower ? "pontuacao" : column); continue; }
          if(!tabPrincipal) {
            if(lower == "descricao") { result.Add(toLower ? "descricao" : column); continue; }
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "concurso", "experiencia", "pontuacao" }
                          : new string[] { "CONCURSO", "EXPERIENCIA", "PONTUACAO" });
        else
          result.AddRange(toLower
                          ? new string[] { "concurso", "experiencia", "pontuacao", "descricao" }
                          : new string[] { "CONCURSO", "EXPERIENCIA", "PONTUACAO", "DESCRICAO" });

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
        "ly_concurso_doc_experiencia.concurso", "ly_concurso_doc_experiencia.experiencia"
      });
      if(scColumns.Contains("pontuacao")) list.Add("ly_concurso_doc_experiencia.pontuacao");
      if(scColumns.Contains("descricao")) list.Add("ly_concurso_experiencia.descricao");
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
          "FROM ly_concurso_doc_experiencia " +
               "INNER JOIN ly_concurso_experiencia " +
                 "ON ly_concurso_experiencia.experiencia = ly_concurso_doc_experiencia.experiencia " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_concurso_doc_experiencia");
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
        da.TableMappings.Add("Table", "Ly_concurso_doc_experiencia");
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
            if (dr["CONCURSO"] != null && string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
              dr["CONCURSO"] = DBNull.Value;
            if (dr["EXPERIENCIA"] != null && string.IsNullOrEmpty(dr["EXPERIENCIA"].ToString()))
              dr["EXPERIENCIA"] = DBNull.Value;
            if (dr["PONTUACAO"] != null && string.IsNullOrEmpty(dr["PONTUACAO"].ToString()))
              dr["PONTUACAO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["CONCURSO"] == null || string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
            {
              return "Favor informar o concurso.";
            }

            if (dr["CONCURSO"] != null && !string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
            {
              if (dr["CONCURSO"].ToString().Length > 20)
              {
                return "O concurso deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["EXPERIENCIA"] == null || string.IsNullOrEmpty(dr["EXPERIENCIA"].ToString()))
            {
              return "Favor informar a experiência.";
            }

            if (dr["EXPERIENCIA"] != null && !string.IsNullOrEmpty(dr["EXPERIENCIA"].ToString()))
            {
              if (dr["EXPERIENCIA"].ToString().Length > 20)
              {
                return "A experiência deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["PONTUACAO"] == null || string.IsNullOrEmpty(dr["PONTUACAO"].ToString()))
            {
              return "Favor informar a pontuação.";
            }

            if (dr["PONTUACAO"] != null && !string.IsNullOrEmpty(dr["PONTUACAO"].ToString()))
            {
                if ((Convert.ToDecimal(dr["PONTUACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PONTUACAO"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["PONTUACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PONTUACAO"]).ToString("0.############").Split(',')[1].Length > 2))
                {
                    return "A pontuação deve ser um valor numérico com, no máximo, 10 dígitos na parte inteira e 2 casas decimal.";
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

    public static Ly_concurso_doc_experiencia Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_concurso_doc_experiencia Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_concurso_doc_experiencia tab = new Ly_concurso_doc_experiencia();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_concurso_doc_experiencia tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @concurso, string @experiencia, decimal? @pontuacao
      ) {
        return (Row)Add(new object[] {
          @concurso, @experiencia, @pontuacao
        });
      }

      public Row Find(
        string @concurso, string @experiencia
      ) {
        return (Row)Find(new object[] {
          @concurso, @experiencia
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
      /// CONCURSO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Concurso { get { return (string)this["concurso"]; } set { this["concurso"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// EXPERIENCIA: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Experiencia { get { return (string)this["experiencia"]; } set { this["experiencia"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PONTUACAO: NUMERIC(15), Not Null
      /// </summary>
      public decimal? Pontuacao { get { return (decimal?)this["pontuacao"]; } set { this["pontuacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO: VARCHAR(500), Not Null
      /// </summary>
      public string Descricao { get { return (string)this["descricao"]; } set { this["descricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @concurso, string @experiencia, decimal? @pontuacao) {
        return Insert(connection,
                      @concurso, @experiencia, @pontuacao,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @concurso, string @experiencia, decimal? @pontuacao,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_concurso_doc_experiencia(),
          new string[] { "concurso", "experiencia", "pontuacao" },
          new object[] { @concurso, @experiencia, @pontuacao },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkConcurso, string pkExperiencia,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_concurso_doc_experiencia(),
          new string[] { "concurso", "experiencia" },
          new object[] { pkConcurso, pkExperiencia },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkConcurso, string pkExperiencia) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_concurso_doc_experiencia(),
          new string[] { "concurso", "experiencia" },
          new object[] { pkConcurso, pkExperiencia }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkConcurso, string pkExperiencia) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_concurso_doc_experiencia(),
          new string[] { "concurso", "experiencia" },
          new object[] { pkConcurso, pkExperiencia }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Concurso { get { return (string)row["concurso", DataRowVersion.Original]; } }
        public string Experiencia { get { return (string)row["experiencia", DataRowVersion.Original]; } }
        public decimal? Pontuacao { get { return (decimal?)row["pontuacao", DataRowVersion.Original]; } }
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
    #region Coluna: CONCURSO
    private ColumnConcurso pvColConcurso;

    public sealed class ColumnConcurso : TDataColumn {

      public ColumnConcurso() {
        this.ColumnName = "concurso";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "concurso";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_doc_experiencia.concurso"; } }
      public override string OriginalCol { get { return "concurso"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOC_EXPERIENCIA.CONCURSO"; } }
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

    #region Coluna: EXPERIENCIA
    private ColumnExperiencia pvColExperiencia;

    public sealed class ColumnExperiencia : TDataColumn {

      public ColumnExperiencia() {
        this.ColumnName = "experiencia";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "experiência";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_doc_experiencia.experiencia"; } }
      public override string OriginalCol { get { return "experiencia"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOC_EXPERIENCIA.EXPERIENCIA"; } }
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

    #region Coluna: PONTUACAO
    private ColumnPontuacao pvColPontuacao;

    public sealed class ColumnPontuacao : TDataColumn {

      public ColumnPontuacao() {
        this.ColumnName = "pontuacao";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "pontuação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_doc_experiencia.pontuacao"; } }
      public override string OriginalCol { get { return "pontuacao"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOC_EXPERIENCIA.PONTUACAO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 12; } }
      public override int Precision { get { return 10; } }
      public override int Scale { get { return 2; } }
      public override string SqlServerType { get { return "NUMERIC(10,2)"; } }

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
      public override string FullCol { get { return "ly_concurso_experiencia.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "LY_CONCURSO_EXPERIENCIA.DESCRICAO"; } }
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

  public partial class Ly_concurso_doc_habilitacao : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_concurso_doc_habilitacao() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_concurso_doc_habilitacao");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_concurso_doc_habilitacao");
      }
      catch{}
    }

    public Ly_concurso_doc_habilitacao() {
      pvColConcurso = new ColumnConcurso(); Columns.Add(pvColConcurso);
      pvColNucleo = new ColumnNucleo(); Columns.Add(pvColNucleo);
      pvColCategoria = new ColumnCategoria(); Columns.Add(pvColCategoria);
      pvColAgrupamento = new ColumnAgrupamento(); Columns.Add(pvColAgrupamento);
      pvColVagas = new ColumnVagas(); Columns.Add(pvColVagas);
      pvColCarga_horaria = new ColumnCarga_horaria(); Columns.Add(pvColCarga_horaria);
      pvColDescricao02 = new ColumnDescricao02(); Columns.Add(pvColDescricao02);
      pvColDescricao04 = new ColumnDescricao04(); Columns.Add(pvColDescricao04);
      pvColNome = new ColumnNome(); Columns.Add(pvColNome);
      pvColMunicipio = new ColumnMunicipio(); Columns.Add(pvColMunicipio);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_concurso_doc_habilitacao");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @concurso, string @nucleo, string @categoria, string @agrupamento, 
                      decimal? @vagas, string @municipio) {
      Row row = NewRow();

      row.Concurso = @concurso;
      row.Nucleo = @nucleo;
      row.Categoria = @categoria;
      row.Agrupamento = @agrupamento;
      row.Vagas = @vagas;
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
      get { return "LY_CONCURSO_DOC_HABILITACAO"; }
    }

    protected override sealed string Name { get { return "Habilitação do concurso"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColConcurso, pvColNucleo, pvColCategoria, pvColAgrupamento, pvColMunicipio  });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_concurso_doc_habilit_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_concurso_doc_habilit_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_concurso_doc_habilit_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_concurso_doc_habilit_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkConcurso", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "CONCURSO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkNucleo", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "NUCLEO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkCategoria", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "CATEGORIA", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkAgrupamento", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "AGRUPAMENTO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkMunicipio_proc", OleDbType.VarChar, 200, ParameterDirection.Input, true, 0, 0, "MUNICIPIO_PROC", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("concurso", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CONCURSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("nucleo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NUCLEO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("categoria", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CATEGORIA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("agrupamento", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "AGRUPAMENTO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("vagas", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 25, 10, "VAGAS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("carga_horaria", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "CARGA_HORARIA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("municipio_proc", OleDbType.VarChar, 20, ParameterDirection.InputOutput, true, 0, 0, "MUNICIPIO_PROC", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "concurso") { result.Add(toLower ? "concurso" : column); continue; }
          if(lower == "nucleo") { result.Add(toLower ? "nucleo" : column); continue; }
          if(lower == "categoria") { result.Add(toLower ? "categoria" : column); continue; }
          if(lower == "agrupamento") { result.Add(toLower ? "agrupamento" : column); continue; }
          if(lower == "vagas") { result.Add(toLower ? "vagas" : column); continue; }
          if(lower == "carga_horaria") { result.Add(toLower ? "carga_horaria" : column); continue; }
          if(lower == "municipio_proc") { result.Add(toLower ? "municipio_proc" : column); continue; }
          if(!tabPrincipal) {
            if(lower == "descricao02") { result.Add(toLower ? "descricao02" : column); continue; }
            if(lower == "descricao04") { result.Add(toLower ? "descricao04" : column); continue; }
            if(lower == "nome") { result.Add(toLower ? "nome" : column); continue; }
            if (lower == "nome_municipio") { result.Add(toLower ? "nome_municipio" : column); continue; }

          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "concurso", "nucleo", "categoria", "agrupamento", "vagas", "carga_horaria", "municipio" }
                          : new string[] { "CONCURSO", "NUCLEO", "CATEGORIA", "AGRUPAMENTO", "VAGAS", "CARGA_HORARIA", "municipio_proc" });
        else
          result.AddRange(toLower
                          ? new string[] { "concurso", "nucleo", "categoria", "agrupamento", "vagas", "carga_horaria", 
                                           "descricao02", "descricao04", "nome", "municipio", "nome_municipio" }
                          : new string[] { "CONCURSO", "NUCLEO", "CATEGORIA", "AGRUPAMENTO", "VAGAS", "CARGA_HORARIA", 
                                           "DESCRICAO02", "DESCRICAO04", "NOME", "municipio_proc", "nome_municipio" });

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
        "ly_concurso_doc_habilitacao.concurso", "ly_concurso_doc_habilitacao.nucleo", "ly_concurso_doc_habilitacao.categoria", 
        "ly_concurso_doc_habilitacao.agrupamento", "ly_concurso_doc_habilitacao.municipio_proc"
      });
      if(scColumns.Contains("vagas")) list.Add("ly_concurso_doc_habilitacao.vagas");
      if(scColumns.Contains("carga_horaria")) list.Add("ly_concurso_doc_habilitacao.carga_horaria");
      if(scColumns.Contains("descricao02")) list.Add("ly_nucleo.descricao descricao02");
      if(scColumns.Contains("descricao04")) list.Add("ly_grupo_habilitacao.descricao descricao04");
      if(scColumns.Contains("nome")) list.Add("ly_categoria_docente.nome");
      if(scColumns.Contains("nome_municipio")) list.Add("MUNICIPIO.nome nome_municipio");
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
          "FROM ly_concurso_doc_habilitacao " +
               "INNER JOIN ly_grupo_habilitacao " +
                 "ON ly_grupo_habilitacao.agrupamento = ly_concurso_doc_habilitacao.agrupamento " +
               "INNER JOIN ly_categoria_docente " +
                 "ON ly_categoria_docente.categoria = ly_concurso_doc_habilitacao.categoria " +
               "INNER JOIN ly_nucleo " +
                 "ON ly_nucleo.nucleo = ly_concurso_doc_habilitacao.nucleo " +
               "LEFT JOIN MUNICIPIO " +
                 "ON MUNICIPIO.CODIGO = ly_concurso_doc_habilitacao.municipio_proc " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_concurso_doc_habilitacao");
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
        da.TableMappings.Add("Table", "Ly_concurso_doc_habilitacao");
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
            if (dr["CONCURSO"] != null && string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
              dr["CONCURSO"] = DBNull.Value;
            if (dr["NUCLEO"] != null && string.IsNullOrEmpty(dr["NUCLEO"].ToString()))
              dr["NUCLEO"] = DBNull.Value;
            if (dr["CATEGORIA"] != null && string.IsNullOrEmpty(dr["CATEGORIA"].ToString()))
              dr["CATEGORIA"] = DBNull.Value;
            if (dr["AGRUPAMENTO"] != null && string.IsNullOrEmpty(dr["AGRUPAMENTO"].ToString()))
              dr["AGRUPAMENTO"] = DBNull.Value;
            if (dr["VAGAS"] != null && string.IsNullOrEmpty(dr["VAGAS"].ToString()))
              dr["VAGAS"] = DBNull.Value;
            if (dr["CARGA_HORARIA"] != null && string.IsNullOrEmpty(dr["CARGA_HORARIA"].ToString()))
              dr["CARGA_HORARIA"] = DBNull.Value;
            if (dr["MUNICIPIO_PROC"] != null && string.IsNullOrEmpty(dr["MUNICIPIO_PROC"].ToString()))
                dr["MUNICIPIO_PROC"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["CONCURSO"] == null || string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
            {
              return "Favor informar o concurso.";
            }

            if (dr["CONCURSO"] != null && !string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
            {
              if (dr["CONCURSO"].ToString().Length > 20)
              {
                return "O concurso deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["NUCLEO"] == null || string.IsNullOrEmpty(dr["NUCLEO"].ToString()))
            {
              return "Favor informar a coordenadoria.";
            }

            if (dr["NUCLEO"] != null && !string.IsNullOrEmpty(dr["NUCLEO"].ToString()))
            {
              if (dr["NUCLEO"].ToString().Length > 20)
              {
                return "A coordenadoria deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["CATEGORIA"] == null || string.IsNullOrEmpty(dr["CATEGORIA"].ToString()))
            {
              return "Favor informar o cargo.";
            }

            if (dr["CATEGORIA"] != null && !string.IsNullOrEmpty(dr["CATEGORIA"].ToString()))
            {
              if (dr["CATEGORIA"].ToString().Length > 20)
              {
                return "O cargo deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["AGRUPAMENTO"] == null || string.IsNullOrEmpty(dr["AGRUPAMENTO"].ToString()))
            {
              return "Favor informar o grupo de habilitação.";
            }

            if (dr["AGRUPAMENTO"] != null && !string.IsNullOrEmpty(dr["AGRUPAMENTO"].ToString()))
            {
              if (dr["AGRUPAMENTO"].ToString().Length > 50)
              {
                return "O grupo de habilitação deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["VAGAS"] == null || string.IsNullOrEmpty(dr["VAGAS"].ToString()))
            {
              return "Favor informar as vagas.";
            }

            if (dr["VAGAS"] != null && !string.IsNullOrEmpty(dr["VAGAS"].ToString()))
            {
              if ((Convert.ToDecimal(dr["VAGAS"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["VAGAS"]).ToString("0.############").Split(',')[0].Length > 15) || (Convert.ToDecimal(dr["VAGAS"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["VAGAS"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "As vagas deve ser um valor inteiro com 15 dígitos no máximo.";
              }
            }

            if (dr["CARGA_HORARIA"] != null && !string.IsNullOrEmpty(dr["CARGA_HORARIA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["CARGA_HORARIA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["CARGA_HORARIA"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["CARGA_HORARIA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["CARGA_HORARIA"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A carga horária deve ser um valor inteiro com 3 dígitos no máximo.";
              }
            }

            if (dr["MUNICIPIO_PROC"] == null || string.IsNullOrEmpty(dr["MUNICIPIO_PROC"].ToString()))
            {
                return "Favor informar o município.";
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

    public static Ly_concurso_doc_habilitacao Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_concurso_doc_habilitacao Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_concurso_doc_habilitacao tab = new Ly_concurso_doc_habilitacao();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_concurso_doc_habilitacao tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @concurso, string @nucleo, string @categoria, string @agrupamento, decimal? @vagas, 
        decimal? @carga_horaria, string @municipio
      ) {
        return (Row)Add(new object[] {
          @concurso, @nucleo, @categoria, @agrupamento, @vagas, @carga_horaria, @municipio
        });
      }

      public Row Find(
        string @concurso, string @nucleo, string @categoria, string @agrupamento, string @municipio_proc
      ) {
        return (Row)Find(new object[] {
          @concurso, @nucleo, @categoria, @agrupamento, @municipio_proc
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
      /// CONCURSO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Concurso { get { return (string)this["concurso"]; } set { this["concurso"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NUCLEO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Nucleo { get { return (string)this["nucleo"]; } set { this["nucleo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CATEGORIA: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Categoria { get { return (string)this["categoria"]; } set { this["categoria"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// AGRUPAMENTO: VARCHAR(50), Primary Key, Not Null
      /// </summary>
      public string Agrupamento { get { return (string)this["agrupamento"]; } set { this["agrupamento"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// VAGAS: NUMERIC(15), Not Null
      /// </summary>
      public decimal? Vagas { get { return (decimal?)this["vagas"]; } set { this["vagas"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CARGA_HORARIA: NUMERIC(3)
      /// </summary>
      public decimal? Carga_horaria { get { return (decimal?)this["carga_horaria"]; } set { this["carga_horaria"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO: VARCHAR(100), Not Null
      /// </summary>
      public string Descricao02 { get { return (string)this["descricao02"]; } set { this["descricao02"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO: VARCHAR(200), Not Null
      /// </summary>
      public string Descricao04 { get { return (string)this["descricao04"]; } set { this["descricao04"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NOME: VARCHAR(100)
      /// </summary>
      public string Nome { get { return (string)this["nome"]; } set { this["nome"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NUCLEO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Municipio { get { return (string)this["municipio_proc"]; } set { this["municipio_proc"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @concurso, string @nucleo, string @categoria, string @agrupamento, decimal? @vagas, string @municipio) {
        return Insert(connection,
                      @concurso, @nucleo, @categoria, @agrupamento, @vagas, @municipio,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @concurso, string @nucleo, string @categoria, string @agrupamento, decimal? @vagas,string @municipio,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_concurso_doc_habilitacao(),
          new string[] { "concurso", "nucleo", "categoria", "agrupamento", "vagas", "municipio_proc" },
          new object[] { @concurso, @nucleo, @categoria, @agrupamento, @vagas, @municipio },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkConcurso, string pkNucleo, string pkCategoria, string pkAgrupamento, string pkMunicipio_proc,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_concurso_doc_habilitacao(),
          new string[] { "concurso", "nucleo", "categoria", "agrupamento", "municipio_proc" },
          new object[] { pkConcurso, pkNucleo, pkCategoria, pkAgrupamento, pkMunicipio_proc },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkConcurso, string pkNucleo, string pkCategoria, string pkAgrupamento, string pkMunicipio_proc)
      {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_concurso_doc_habilitacao(),
          new string[] { "concurso", "nucleo", "categoria", "agrupamento", "municipio_proc" },
          new object[] { pkConcurso, pkNucleo, pkCategoria, pkAgrupamento, pkMunicipio_proc }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkConcurso, string pkNucleo, string pkCategoria, string pkAgrupamento, string pkMunicipio_proc) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_concurso_doc_habilitacao(),
          new string[] { "concurso", "nucleo", "categoria", "agrupamento", "municipio_proc" },
          new object[] { pkConcurso, pkNucleo, pkCategoria, pkAgrupamento, pkMunicipio_proc }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Concurso { get { return (string)row["concurso", DataRowVersion.Original]; } }
        public string Nucleo { get { return (string)row["nucleo", DataRowVersion.Original]; } }
        public string Categoria { get { return (string)row["categoria", DataRowVersion.Original]; } }
        public string Agrupamento { get { return (string)row["agrupamento", DataRowVersion.Original]; } }
        public decimal? Vagas { get { return (decimal?)row["vagas", DataRowVersion.Original]; } }
        public decimal? Carga_horaria { get { return (decimal?)row["carga_horaria", DataRowVersion.Original]; } }
        public string Descricao02 { get { return (string)row["descricao02", DataRowVersion.Original]; } }
        public string Descricao04 { get { return (string)row["descricao04", DataRowVersion.Original]; } }
        public string Nome { get { return (string)row["nome", DataRowVersion.Original]; } }
        public string Municipio { get { return (string)row["municipio_proc", DataRowVersion.Original]; } }

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
    #region Coluna: CONCURSO
    private ColumnConcurso pvColConcurso;

    public sealed class ColumnConcurso : TDataColumn {

      public ColumnConcurso() {
        this.ColumnName = "concurso";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "concurso";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_doc_habilitacao.concurso"; } }
      public override string OriginalCol { get { return "concurso"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOC_HABILITACAO.CONCURSO"; } }
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

    #region Coluna: NUCLEO
    private ColumnNucleo pvColNucleo;

    public sealed class ColumnNucleo : TDataColumn {

      public ColumnNucleo() {
        this.ColumnName = "nucleo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "coordenadoria";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_doc_habilitacao.nucleo"; } }
      public override string OriginalCol { get { return "nucleo"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOC_HABILITACAO.NUCLEO"; } }
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

    #region Coluna: CATEGORIA
    private ColumnCategoria pvColCategoria;

    public sealed class ColumnCategoria : TDataColumn {

      public ColumnCategoria() {
        this.ColumnName = "categoria";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "cargo";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_doc_habilitacao.categoria"; } }
      public override string OriginalCol { get { return "categoria"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOC_HABILITACAO.CATEGORIA"; } }
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

    #region Coluna: AGRUPAMENTO
    private ColumnAgrupamento pvColAgrupamento;

    public sealed class ColumnAgrupamento : TDataColumn {

      public ColumnAgrupamento() {
        this.ColumnName = "agrupamento";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "grupo de habilitação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_doc_habilitacao.agrupamento"; } }
      public override string OriginalCol { get { return "agrupamento"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOC_HABILITACAO.AGRUPAMENTO"; } }
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

    #region Coluna: VAGAS
    private ColumnVagas pvColVagas;

    public sealed class ColumnVagas : TDataColumn {

      public ColumnVagas() {
        this.ColumnName = "vagas";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "vagas";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_doc_habilitacao.vagas"; } }
      public override string OriginalCol { get { return "vagas"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOC_HABILITACAO.VAGAS"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 16; } }
      public override int Precision { get { return 15; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(15)"; } }

    }
    #endregion

    #region Coluna: CARGA_HORARIA
    private ColumnCarga_horaria pvColCarga_horaria;

    public sealed class ColumnCarga_horaria : TDataColumn {

      public ColumnCarga_horaria() {
        this.ColumnName = "carga_horaria";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "carga horária";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_doc_habilitacao.carga_horaria"; } }
      public override string OriginalCol { get { return "carga_horaria"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOC_HABILITACAO.CARGA_HORARIA"; } }
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

    #region Coluna: DESCRICAO02
    private ColumnDescricao02 pvColDescricao02;

    public sealed class ColumnDescricao02 : TDataColumn {

      public ColumnDescricao02() {
        this.ColumnName = "descricao02";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "descrição";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_nucleo.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "LY_NUCLEO.DESCRICAO"; } }
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

    #region Coluna: DESCRICAO04
    private ColumnDescricao04 pvColDescricao04;

    public sealed class ColumnDescricao04 : TDataColumn {

      public ColumnDescricao04() {
        this.ColumnName = "descricao04";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "descrição";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_grupo_habilitacao.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "LY_GRUPO_HABILITACAO.DESCRICAO"; } }
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

    #region Coluna: NOME
    private ColumnNome pvColNome;

    public sealed class ColumnNome : TDataColumn {

      public ColumnNome() {
        this.ColumnName = "nome";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "descrição";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_categoria_docente.nome"; } }
      public override string OriginalCol { get { return "nome"; } }
      public override string RefCol { get { return "LY_CATEGORIA_DOCENTE.NOME"; } }
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


    #region Coluna: MUNICIPIO
    private ColumnMunicipio pvColMunicipio;

    public sealed class ColumnMunicipio : TDataColumn
    {

        public ColumnMunicipio()
        {
            this.ColumnName = "municipio_proc";
            this.DataType = typeof(string);

        }


        protected override string GetName() { return "municipio_proc"; }
        protected override string GetCaption() { return ""; }
        protected override string GetDescricao() { return ""; }
        public override string FullCol { get { return "ly_concurso_doc_habilitacao.municipio_proc"; } }
        public override string OriginalCol { get { return "municipio_proc"; } }
        public override string RefCol { get { return "LY_CONCURSO_DOC_HABILITACAO.MUNICIPIO_PROC"; } }
        public override bool PrimaryKey { get { return true ; } }
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

  public partial class Ly_concurso_doc_sit_avaliacao : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_concurso_doc_sit_avaliacao() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_concurso_doc_sit_avaliacao");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_concurso_doc_sit_avaliacao");
      }
      catch{}
    }

    public Ly_concurso_doc_sit_avaliacao() {
      pvColSituacao = new ColumnSituacao(); Columns.Add(pvColSituacao);
      pvColDescricao = new ColumnDescricao(); Columns.Add(pvColDescricao);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_concurso_doc_sit_avaliacao");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @situacao, string @descricao) {
      Row row = NewRow();

      row.Situacao = @situacao;
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
      get { return "LY_CONCURSO_DOC_SIT_AVALIACAO"; }
    }

    protected override sealed string Name { get { return "Situação da avaliação dos concursos de docentes"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColSituacao });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_concurso_doc_sit_ava_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_concurso_doc_sit_ava_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_concurso_doc_sit_ava_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_concurso_doc_sit_ava_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkSituacao", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "SITUACAO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("situacao", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SITUACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
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
          if(lower == "situacao") { result.Add(toLower ? "situacao" : column); continue; }
          if(lower == "descricao") { result.Add(toLower ? "descricao" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "situacao", "descricao" }
                          : new string[] { "SITUACAO", "DESCRICAO" });
        else
          result.AddRange(toLower
                          ? new string[] { "situacao", "descricao" }
                          : new string[] { "SITUACAO", "DESCRICAO" });

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
        "ly_concurso_doc_sit_avaliacao.situacao"
      });
      if(scColumns.Contains("descricao")) list.Add("ly_concurso_doc_sit_avaliacao.descricao");
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
          "FROM ly_concurso_doc_sit_avaliacao " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_concurso_doc_sit_avaliacao");
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
        da.TableMappings.Add("Table", "Ly_concurso_doc_sit_avaliacao");
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
            if (dr["SITUACAO"] != null && string.IsNullOrEmpty(dr["SITUACAO"].ToString()))
              dr["SITUACAO"] = DBNull.Value;
            if (dr["DESCRICAO"] != null && string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
              dr["DESCRICAO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["SITUACAO"] == null || string.IsNullOrEmpty(dr["SITUACAO"].ToString()))
            {
              return "Favor informar a situação da avaliação.";
            }

            if (dr["SITUACAO"] != null && !string.IsNullOrEmpty(dr["SITUACAO"].ToString()))
            {
              if (dr["SITUACAO"].ToString().Length > 20)
              {
                return "A situação da avaliação deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["DESCRICAO"] == null || string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              return "Favor informar a descrição.";
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

    public static Ly_concurso_doc_sit_avaliacao Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_concurso_doc_sit_avaliacao Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_concurso_doc_sit_avaliacao tab = new Ly_concurso_doc_sit_avaliacao();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_concurso_doc_sit_avaliacao tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @situacao, string @descricao
      ) {
        return (Row)Add(new object[] {
          @situacao, @descricao
        });
      }

      public Row Find(
        string @situacao
      ) {
        return (Row)Find(new object[] {
          @situacao
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
      /// SITUACAO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Situacao { get { return (string)this["situacao"]; } set { this["situacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO: VARCHAR(200), Not Null
      /// </summary>
      public string Descricao { get { return (string)this["descricao"]; } set { this["descricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @situacao, string @descricao) {
        return Insert(connection,
                      @situacao, @descricao,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @situacao, string @descricao,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_concurso_doc_sit_avaliacao(),
          new string[] { "situacao", "descricao" },
          new object[] { @situacao, @descricao },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkSituacao,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_concurso_doc_sit_avaliacao(),
          new string[] { "situacao" },
          new object[] { pkSituacao },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkSituacao) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_concurso_doc_sit_avaliacao(),
          new string[] { "situacao" },
          new object[] { pkSituacao }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkSituacao) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_concurso_doc_sit_avaliacao(),
          new string[] { "situacao" },
          new object[] { pkSituacao }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Situacao { get { return (string)row["situacao", DataRowVersion.Original]; } }
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
    #region Coluna: SITUACAO
    private ColumnSituacao pvColSituacao;

    public sealed class ColumnSituacao : TDataColumn {

      public ColumnSituacao() {
        this.ColumnName = "situacao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "situação da avaliação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_doc_sit_avaliacao.situacao"; } }
      public override string OriginalCol { get { return "situacao"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOC_SIT_AVALIACAO.SITUACAO"; } }
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


      protected override string GetName(){return "descrição";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_doc_sit_avaliacao.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOC_SIT_AVALIACAO.DESCRICAO"; } }
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
    #endregion
  }

  public partial class Ly_concurso_doc_titulacoes : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_concurso_doc_titulacoes() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_concurso_doc_titulacoes");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_concurso_doc_titulacoes");
      }
      catch{}
    }

    public Ly_concurso_doc_titulacoes() {
      pvColConcurso = new ColumnConcurso(); Columns.Add(pvColConcurso);
      pvColTitulacao = new ColumnTitulacao(); Columns.Add(pvColTitulacao);
      pvColPontuacao = new ColumnPontuacao(); Columns.Add(pvColPontuacao);
      pvColDescricao = new ColumnDescricao(); Columns.Add(pvColDescricao);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_concurso_doc_titulacoes");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @concurso, string @titulacao, decimal? @pontuacao) {
      Row row = NewRow();

      row.Concurso = @concurso;
      row.Titulacao = @titulacao;
      row.Pontuacao = @pontuacao;

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
      get { return "LY_CONCURSO_DOC_TITULACOES"; }
    }

    protected override sealed string Name { get { return "Titulações dos Concursos"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColConcurso, pvColTitulacao });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_concurso_doc_titulac_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_concurso_doc_titulac_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_concurso_doc_titulac_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_concurso_doc_titulac_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkConcurso", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "CONCURSO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkTitulacao", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "TITULACAO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("concurso", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CONCURSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("titulacao", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TITULACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("pontuacao", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 25, 10, "PONTUACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "concurso") { result.Add(toLower ? "concurso" : column); continue; }
          if(lower == "titulacao") { result.Add(toLower ? "titulacao" : column); continue; }
          if(lower == "pontuacao") { result.Add(toLower ? "pontuacao" : column); continue; }
          if(!tabPrincipal) {
            if(lower == "descricao") { result.Add(toLower ? "descricao" : column); continue; }
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "concurso", "titulacao", "pontuacao" }
                          : new string[] { "CONCURSO", "TITULACAO", "PONTUACAO" });
        else
          result.AddRange(toLower
                          ? new string[] { "concurso", "titulacao", "pontuacao", "descricao" }
                          : new string[] { "CONCURSO", "TITULACAO", "PONTUACAO", "DESCRICAO" });

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
        "ly_concurso_doc_titulacoes.concurso", "ly_concurso_doc_titulacoes.titulacao"
      });
      if(scColumns.Contains("pontuacao")) list.Add("ly_concurso_doc_titulacoes.pontuacao");
      if(scColumns.Contains("descricao")) list.Add("ly_concurso_titulacao.descricao");
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
          "FROM ly_concurso_doc_titulacoes " +
               "INNER JOIN ly_concurso_titulacao " +
                 "ON ly_concurso_titulacao.titulacao = ly_concurso_doc_titulacoes.titulacao " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_concurso_doc_titulacoes");
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
        da.TableMappings.Add("Table", "Ly_concurso_doc_titulacoes");
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
            if (dr["CONCURSO"] != null && string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
              dr["CONCURSO"] = DBNull.Value;
            if (dr["TITULACAO"] != null && string.IsNullOrEmpty(dr["TITULACAO"].ToString()))
              dr["TITULACAO"] = DBNull.Value;
            if (dr["PONTUACAO"] != null && string.IsNullOrEmpty(dr["PONTUACAO"].ToString()))
              dr["PONTUACAO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["CONCURSO"] == null || string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
            {
              return "Favor informar o concurso.";
            }

            if (dr["CONCURSO"] != null && !string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
            {
              if (dr["CONCURSO"].ToString().Length > 20)
              {
                return "O concurso deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["TITULACAO"] == null || string.IsNullOrEmpty(dr["TITULACAO"].ToString()))
            {
              return "Favor informar a titulação.";
            }

            if (dr["TITULACAO"] != null && !string.IsNullOrEmpty(dr["TITULACAO"].ToString()))
            {
              if (dr["TITULACAO"].ToString().Length > 20)
              {
                return "A titulação deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["PONTUACAO"] == null || string.IsNullOrEmpty(dr["PONTUACAO"].ToString()))
            {
              return "Favor informar a pontuação.";
            }           

            if (dr["PONTUACAO"] != null && !string.IsNullOrEmpty(dr["PONTUACAO"].ToString()))
            {
                if ((Convert.ToDecimal(dr["PONTUACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PONTUACAO"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["PONTUACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PONTUACAO"]).ToString("0.############").Split(',')[1].Length > 2))
                {
                    return "A pontuação deve ser um valor numérico com, no máximo, 10 dígitos na parte inteira e 2 casas decimal.";
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

    public static Ly_concurso_doc_titulacoes Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_concurso_doc_titulacoes Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_concurso_doc_titulacoes tab = new Ly_concurso_doc_titulacoes();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_concurso_doc_titulacoes tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @concurso, string @titulacao, decimal? @pontuacao
      ) {
        return (Row)Add(new object[] {
          @concurso, @titulacao, @pontuacao
        });
      }

      public Row Find(
        string @concurso, string @titulacao
      ) {
        return (Row)Find(new object[] {
          @concurso, @titulacao
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
      /// CONCURSO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Concurso { get { return (string)this["concurso"]; } set { this["concurso"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TITULACAO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Titulacao { get { return (string)this["titulacao"]; } set { this["titulacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PONTUACAO: NUMERIC(10.2), Not Null
      /// </summary>
      public decimal? Pontuacao { get { return (decimal?)this["pontuacao"]; } set { this["pontuacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO: VARCHAR(500), Not Null
      /// </summary>
      public string Descricao { get { return (string)this["descricao"]; } set { this["descricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @concurso, string @titulacao, decimal? @pontuacao) {
        return Insert(connection,
                      @concurso, @titulacao, @pontuacao,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @concurso, string @titulacao, decimal? @pontuacao,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_concurso_doc_titulacoes(),
          new string[] { "concurso", "titulacao", "pontuacao" },
          new object[] { @concurso, @titulacao, @pontuacao },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkConcurso, string pkTitulacao,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_concurso_doc_titulacoes(),
          new string[] { "concurso", "titulacao" },
          new object[] { pkConcurso, pkTitulacao },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkConcurso, string pkTitulacao) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_concurso_doc_titulacoes(),
          new string[] { "concurso", "titulacao" },
          new object[] { pkConcurso, pkTitulacao }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkConcurso, string pkTitulacao) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_concurso_doc_titulacoes(),
          new string[] { "concurso", "titulacao" },
          new object[] { pkConcurso, pkTitulacao }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Concurso { get { return (string)row["concurso", DataRowVersion.Original]; } }
        public string Titulacao { get { return (string)row["titulacao", DataRowVersion.Original]; } }
        public decimal? Pontuacao { get { return (decimal?)row["pontuacao", DataRowVersion.Original]; } }
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
    #region Coluna: CONCURSO
    private ColumnConcurso pvColConcurso;

    public sealed class ColumnConcurso : TDataColumn {

      public ColumnConcurso() {
        this.ColumnName = "concurso";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "concurso";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_doc_titulacoes.concurso"; } }
      public override string OriginalCol { get { return "concurso"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOC_TITULACOES.CONCURSO"; } }
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

    #region Coluna: TITULACAO
    private ColumnTitulacao pvColTitulacao;

    public sealed class ColumnTitulacao : TDataColumn {

      public ColumnTitulacao() {
        this.ColumnName = "titulacao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "titulação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_doc_titulacoes.titulacao"; } }
      public override string OriginalCol { get { return "titulacao"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOC_TITULACOES.TITULACAO"; } }
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

    #region Coluna: PONTUACAO
    private ColumnPontuacao pvColPontuacao;

    public sealed class ColumnPontuacao : TDataColumn {

      public ColumnPontuacao() {
        this.ColumnName = "pontuacao";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "pontuação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_doc_titulacoes.pontuacao"; } }
      public override string OriginalCol { get { return "pontuacao"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOC_TITULACOES.PONTUACAO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 12; } }
      public override int Precision { get { return 10; } }
      public override int Scale { get { return 2; } }
      public override string SqlServerType { get { return "NUMERIC(10,2)"; } }

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
      public override string FullCol { get { return "ly_concurso_titulacao.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "LY_CONCURSO_TITULACAO.DESCRICAO"; } }
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

  public partial class Ly_concurso_docente : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_concurso_docente() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_concurso_docente");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_concurso_docente");
      }
      catch{}
    }

    public Ly_concurso_docente() {
      pvColConcurso = new ColumnConcurso(); Columns.Add(pvColConcurso);
      pvColDescricao = new ColumnDescricao(); Columns.Add(pvColDescricao);
      pvColAno = new ColumnAno(); Columns.Add(pvColAno);
      pvColSemestre = new ColumnSemestre(); Columns.Add(pvColSemestre);
      pvColStatus = new ColumnStatus(); Columns.Add(pvColStatus);
      pvColTipo = new ColumnTipo(); Columns.Add(pvColTipo);
      pvColDt_inicio = new ColumnDt_inicio(); Columns.Add(pvColDt_inicio);
      pvColDt_fim = new ColumnDt_fim(); Columns.Add(pvColDt_fim);
      pvColDt_ini_inscr = new ColumnDt_ini_inscr(); Columns.Add(pvColDt_ini_inscr);
      pvColDt_fim_inscr = new ColumnDt_fim_inscr(); Columns.Add(pvColDt_fim_inscr);
      pvColDt_ini_consulta = new ColumnDt_ini_consulta(); Columns.Add(pvColDt_ini_consulta);
      pvColDt_fim_consulta = new ColumnDt_fim_consulta(); Columns.Add(pvColDt_fim_consulta);
      pvColDt_ini_convocacao = new ColumnDt_ini_convocacao(); Columns.Add(pvColDt_ini_convocacao);
      pvColDt_fim_convocacao = new ColumnDt_fim_convocacao(); Columns.Add(pvColDt_fim_convocacao);
      pvColDt_ini_ingresso = new ColumnDt_ini_ingresso(); Columns.Add(pvColDt_ini_ingresso);
      pvColDt_fim_ingresso = new ColumnDt_fim_ingresso(); Columns.Add(pvColDt_fim_ingresso);
      pvColNr_digitos_codigo = new ColumnNr_digitos_codigo(); Columns.Add(pvColNr_digitos_codigo);
      pvColQt_dias_convocacao = new ColumnQt_dias_convocacao(); Columns.Add(pvColQt_dias_convocacao);
      pvColQt_dias_reprovacao = new ColumnQt_dias_reprovacao(); Columns.Add(pvColQt_dias_reprovacao);
      pvColNr_resolucao = new ColumnNr_resolucao(); Columns.Add(pvColNr_resolucao);
      pvColPontuacao_minima = new ColumnPontuacao_minima(); Columns.Add(pvColPontuacao_minima);
      pvColPortaria = new ColumnPortaria(); Columns.Add(pvColPortaria);
      pvColDt_publicacao_do = new ColumnDt_publicacao_do(); Columns.Add(pvColDt_publicacao_do);
      pvColRestricaoanos = new ColumnRestricaoanos(); Columns.Add(pvColRestricaoanos);
      pvColObservacao = new ColumnObservacao(); Columns.Add(pvColObservacao);
	  pvColFuncaoID = new ColumnFuncaoID(); Columns.Add(pvColFuncaoID);
      pvColIndigena = new ColumnIndigena(); Columns.Add(pvColIndigena);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_concurso_docente");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @concurso, string @descricao, decimal? @ano, decimal? @semestre, 
                      string @status,string @tipo, DateTime? @dt_inicio, DateTime? @dt_fim, decimal? @nr_digitos_codigo, 
                      decimal? @pontuacao_minima) {
      Row row = NewRow();

      row.Concurso = @concurso;
      row.Descricao = @descricao;
      row.Ano = @ano;
      row.Semestre = @semestre;
      row.Status = @status;
      row.Tipo = @tipo;
      row.Dt_inicio = @dt_inicio;
      row.Dt_fim = @dt_fim;
      row.Nr_digitos_codigo = @nr_digitos_codigo;
      row.Pontuacao_minima = @pontuacao_minima;

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
      get { return "LY_CONCURSO_DOCENTE"; }
    }

    protected override sealed string Name { get { return "Concurso de docente"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColConcurso });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_concurso_docente_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_concurso_docente_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_concurso_docente_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_concurso_docente_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkConcurso", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "CONCURSO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("concurso", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CONCURSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("descricao", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "DESCRICAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("ano", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ANO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("semestre", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "SEMESTRE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("status", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "STATUS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("tipo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TIPO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("dt_inicio", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_INICIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_fim", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_FIM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_ini_inscr", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_INI_INSCR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_fim_inscr", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_FIM_INSCR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_ini_consulta", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_INI_CONSULTA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_fim_consulta", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_FIM_CONSULTA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_ini_convocacao", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_INI_CONVOCACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_fim_convocacao", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_FIM_CONVOCACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_ini_ingresso", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_INI_INGRESSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_fim_ingresso", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_FIM_INGRESSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("nr_digitos_codigo", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "NR_DIGITOS_CODIGO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("qt_dias_convocacao", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "QT_DIAS_CONVOCACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("qt_dias_reprovacao", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "QT_DIAS_REPROVACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("nr_resolucao", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NR_RESOLUCAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("pontuacao_minima", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 25, 10, "PONTUACAO_MINIMA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("portaria", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PORTARIA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("dt_publicacao_do", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_PUBLICACAO_DO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("restricaoanos", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "RESTRICAOANOS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("observacao", OleDbType.VarChar, 2100, ParameterDirection.InputOutput, true, 0, 0, "OBSERVACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
	  cm.Parameters.Add(new TParameter("funcaoid", OleDbType.VarChar, 200, ParameterDirection.InputOutput, false, 0, 0, "FUNCAOID", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("indigena", OleDbType.VarChar, 200, ParameterDirection.InputOutput, false, 0, 0, "INDIGENA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "concurso") { result.Add(toLower ? "concurso" : column); continue; }
          if(lower == "descricao") { result.Add(toLower ? "descricao" : column); continue; }
          if(lower == "ano") { result.Add(toLower ? "ano" : column); continue; }
          if(lower == "semestre") { result.Add(toLower ? "semestre" : column); continue; }
          if(lower == "status") { result.Add(toLower ? "status" : column); continue; }
          if (lower == "tipo") { result.Add(toLower ? "tipo" : column); continue; }
          if(lower == "dt_inicio") { result.Add(toLower ? "dt_inicio" : column); continue; }
          if(lower == "dt_fim") { result.Add(toLower ? "dt_fim" : column); continue; }
          if(lower == "dt_ini_inscr") { result.Add(toLower ? "dt_ini_inscr" : column); continue; }
          if(lower == "dt_fim_inscr") { result.Add(toLower ? "dt_fim_inscr" : column); continue; }
          if(lower == "dt_ini_consulta") { result.Add(toLower ? "dt_ini_consulta" : column); continue; }
          if(lower == "dt_fim_consulta") { result.Add(toLower ? "dt_fim_consulta" : column); continue; }
          if(lower == "dt_ini_convocacao") { result.Add(toLower ? "dt_ini_convocacao" : column); continue; }
          if(lower == "dt_fim_convocacao") { result.Add(toLower ? "dt_fim_convocacao" : column); continue; }
          if(lower == "dt_ini_ingresso") { result.Add(toLower ? "dt_ini_ingresso" : column); continue; }
          if(lower == "dt_fim_ingresso") { result.Add(toLower ? "dt_fim_ingresso" : column); continue; }
          if(lower == "nr_digitos_codigo") { result.Add(toLower ? "nr_digitos_codigo" : column); continue; }
          if(lower == "qt_dias_convocacao") { result.Add(toLower ? "qt_dias_convocacao" : column); continue; }
          if(lower == "qt_dias_reprovacao") { result.Add(toLower ? "qt_dias_reprovacao" : column); continue; }
          if(lower == "nr_resolucao") { result.Add(toLower ? "nr_resolucao" : column); continue; }
          if(lower == "pontuacao_minima") { result.Add(toLower ? "pontuacao_minima" : column); continue; }
          if(lower == "portaria") { result.Add(toLower ? "portaria" : column); continue; }
          if(lower == "dt_publicacao_do") { result.Add(toLower ? "dt_publicacao_do" : column); continue; }
          if(lower == "restricaoanos") { result.Add(toLower ? "restricaoanos" : column); continue; }
          if(lower == "observacao") { result.Add(toLower ? "observacao" : column); continue; }
		  if (lower == "funcaoid") { result.Add(toLower ? "funcaoid" : column); continue; }
          if (lower == "indigena") { result.Add(toLower ? "indigena" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "concurso", "descricao", "ano", "semestre", "status","tipo", "dt_inicio", 
                                           "dt_fim", "dt_ini_inscr", "dt_fim_inscr", "dt_ini_consulta", "dt_fim_consulta", "dt_ini_convocacao", 
                                           "dt_fim_convocacao", "dt_ini_ingresso", "dt_fim_ingresso", "nr_digitos_codigo", "qt_dias_convocacao", 
                                           "qt_dias_reprovacao", "nr_resolucao", "pontuacao_minima", "portaria", "dt_publicacao_do", 
                                           "restricaoanos", "observacao", "funcaoid","indigena" }
                          : new string[] { "CONCURSO", "DESCRICAO", "ANO", "SEMESTRE", "STATUS","TIPO", "DT_INICIO", 
                                           "DT_FIM", "DT_INI_INSCR", "DT_FIM_INSCR", "DT_INI_CONSULTA", "DT_FIM_CONSULTA", "DT_INI_CONVOCACAO", 
                                           "DT_FIM_CONVOCACAO", "DT_INI_INGRESSO", "DT_FIM_INGRESSO", "NR_DIGITOS_CODIGO", "QT_DIAS_CONVOCACAO", 
                                           "QT_DIAS_REPROVACAO", "NR_RESOLUCAO", "PONTUACAO_MINIMA", "PORTARIA", "DT_PUBLICACAO_DO", 
                                           "RESTRICAOANOS", "OBSERVACAO", "FUNCAOID","INDIGENA" });
        else
          result.AddRange(toLower
                          ? new string[] { "concurso", "descricao", "ano", "semestre", "status","tipo", "dt_inicio", 
                                           "dt_fim", "dt_ini_inscr", "dt_fim_inscr", "dt_ini_consulta", "dt_fim_consulta", "dt_ini_convocacao", 
                                           "dt_fim_convocacao", "dt_ini_ingresso", "dt_fim_ingresso", "nr_digitos_codigo", "qt_dias_convocacao", 
                                           "qt_dias_reprovacao", "nr_resolucao", "pontuacao_minima", "portaria", "dt_publicacao_do", 
                                           "restricaoanos", "observacao", "funcaoid","indigena" }
                          : new string[] { "CONCURSO", "DESCRICAO", "ANO", "SEMESTRE", "STATUS","TIPO", "DT_INICIO", 
                                           "DT_FIM", "DT_INI_INSCR", "DT_FIM_INSCR", "DT_INI_CONSULTA", "DT_FIM_CONSULTA", "DT_INI_CONVOCACAO", 
                                           "DT_FIM_CONVOCACAO", "DT_INI_INGRESSO", "DT_FIM_INGRESSO", "NR_DIGITOS_CODIGO", "QT_DIAS_CONVOCACAO", 
                                           "QT_DIAS_REPROVACAO", "NR_RESOLUCAO", "PONTUACAO_MINIMA", "PORTARIA", "DT_PUBLICACAO_DO", 
                                           "RESTRICAOANOS", "OBSERVACAO", "FUNCAOID","indigena" });

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
        "ly_concurso_docente.concurso"
      });
      if(scColumns.Contains("descricao")) list.Add("ly_concurso_docente.descricao");
      if(scColumns.Contains("ano")) list.Add("ly_concurso_docente.ano");
      if(scColumns.Contains("semestre")) list.Add("ly_concurso_docente.semestre");
      if(scColumns.Contains("status")) list.Add("ly_concurso_docente.status");
      if(scColumns.Contains("tipo")) list.Add("ly_concurso_docente.tipo");
      if(scColumns.Contains("dt_inicio")) list.Add("ly_concurso_docente.dt_inicio");
      if(scColumns.Contains("dt_fim")) list.Add("ly_concurso_docente.dt_fim");
      if(scColumns.Contains("dt_ini_inscr")) list.Add("ly_concurso_docente.dt_ini_inscr");
      if(scColumns.Contains("dt_fim_inscr")) list.Add("ly_concurso_docente.dt_fim_inscr");
      if(scColumns.Contains("dt_ini_consulta")) list.Add("ly_concurso_docente.dt_ini_consulta");
      if(scColumns.Contains("dt_fim_consulta")) list.Add("ly_concurso_docente.dt_fim_consulta");
      if(scColumns.Contains("dt_ini_convocacao")) list.Add("ly_concurso_docente.dt_ini_convocacao");
      if(scColumns.Contains("dt_fim_convocacao")) list.Add("ly_concurso_docente.dt_fim_convocacao");
      if(scColumns.Contains("dt_ini_ingresso")) list.Add("ly_concurso_docente.dt_ini_ingresso");
      if(scColumns.Contains("dt_fim_ingresso")) list.Add("ly_concurso_docente.dt_fim_ingresso");
      if(scColumns.Contains("nr_digitos_codigo")) list.Add("ly_concurso_docente.nr_digitos_codigo");
      if(scColumns.Contains("qt_dias_convocacao")) list.Add("ly_concurso_docente.qt_dias_convocacao");
      if(scColumns.Contains("qt_dias_reprovacao")) list.Add("ly_concurso_docente.qt_dias_reprovacao");
      if(scColumns.Contains("nr_resolucao")) list.Add("ly_concurso_docente.nr_resolucao");
      if(scColumns.Contains("pontuacao_minima")) list.Add("ly_concurso_docente.pontuacao_minima");
      if(scColumns.Contains("portaria")) list.Add("ly_concurso_docente.portaria");
      if(scColumns.Contains("dt_publicacao_do")) list.Add("ly_concurso_docente.dt_publicacao_do");
      if(scColumns.Contains("restricaoanos")) list.Add("ly_concurso_docente.restricaoanos");
      if(scColumns.Contains("observacao")) list.Add("ly_concurso_docente.observacao");
	  if (scColumns.Contains("funcaoid")) list.Add("ly_concurso_docente.funcaoid");
      if (scColumns.Contains("indigena")) list.Add("ly_concurso_docente.indigena");
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
          "FROM ly_concurso_docente " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_concurso_docente");
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
        da.TableMappings.Add("Table", "Ly_concurso_docente");
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
            if (dr["CONCURSO"] != null && string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
              dr["CONCURSO"] = DBNull.Value;
            if (dr["DESCRICAO"] != null && string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
              dr["DESCRICAO"] = DBNull.Value;
            if (dr["ANO"] != null && string.IsNullOrEmpty(dr["ANO"].ToString()))
              dr["ANO"] = DBNull.Value;
            if (dr["SEMESTRE"] != null && string.IsNullOrEmpty(dr["SEMESTRE"].ToString()))
              dr["SEMESTRE"] = DBNull.Value;
            if (dr["STATUS"] != null && string.IsNullOrEmpty(dr["STATUS"].ToString()))
              dr["STATUS"] = DBNull.Value;
            if (dr["TIPO"] != null && string.IsNullOrEmpty(dr["TIPO"].ToString()))
                dr["TIPO"] = DBNull.Value;
            if (dr["NR_DIGITOS_CODIGO"] != null && string.IsNullOrEmpty(dr["NR_DIGITOS_CODIGO"].ToString()))
              dr["NR_DIGITOS_CODIGO"] = DBNull.Value;
            if (dr["QT_DIAS_CONVOCACAO"] != null && string.IsNullOrEmpty(dr["QT_DIAS_CONVOCACAO"].ToString()))
              dr["QT_DIAS_CONVOCACAO"] = DBNull.Value;
            if (dr["QT_DIAS_REPROVACAO"] != null && string.IsNullOrEmpty(dr["QT_DIAS_REPROVACAO"].ToString()))
              dr["QT_DIAS_REPROVACAO"] = DBNull.Value;
            if (dr["NR_RESOLUCAO"] != null && string.IsNullOrEmpty(dr["NR_RESOLUCAO"].ToString()))
              dr["NR_RESOLUCAO"] = DBNull.Value;
            if (dr["PONTUACAO_MINIMA"] != null && string.IsNullOrEmpty(dr["PONTUACAO_MINIMA"].ToString()))
              dr["PONTUACAO_MINIMA"] = DBNull.Value;
            if (dr["PORTARIA"] != null && string.IsNullOrEmpty(dr["PORTARIA"].ToString()))
              dr["PORTARIA"] = DBNull.Value;
            if (dr["RESTRICAOANOS"] != null && string.IsNullOrEmpty(dr["RESTRICAOANOS"].ToString()))
              dr["RESTRICAOANOS"] = DBNull.Value;
            if (dr["OBSERVACAO"] != null && string.IsNullOrEmpty(dr["OBSERVACAO"].ToString()))
              dr["OBSERVACAO"] = DBNull.Value;
			if (dr["FUNCAOID"] != null && string.IsNullOrEmpty(dr["FUNCAOID"].ToString()))
				dr["FUNCAOID"] = DBNull.Value;
            if (dr["INDIGENA"] != null && string.IsNullOrEmpty(dr["FUNCAOID"].ToString()))
                dr["INDIGENA"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["CONCURSO"] == null || string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
            {
              return "Favor informar o concurso.";
            }

            if (dr["CONCURSO"] != null && !string.IsNullOrEmpty(dr["CONCURSO"].ToString()))
            {
              if (dr["CONCURSO"].ToString().Length > 20)
              {
                return "O concurso deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["DESCRICAO"] == null || string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              return "Favor informar a descrição.";
            }

            if (dr["DESCRICAO"] != null && !string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              if (dr["DESCRICAO"].ToString().Length > 100)
              {
                return "A descrição deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["ANO"] == null || string.IsNullOrEmpty(dr["ANO"].ToString()))
            {
              return "Favor informar o ano.";
            }

            if (dr["ANO"] != null && !string.IsNullOrEmpty(dr["ANO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ANO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ANO"]).ToString("0.############").Split(',')[0].Length > 4) || (Convert.ToDecimal(dr["ANO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ANO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O ano deve ser um valor inteiro com 4 dígitos no máximo.";
              }
            }

            if (dr["SEMESTRE"] == null || string.IsNullOrEmpty(dr["SEMESTRE"].ToString()))
            {
              return "Favor informar o semestre.";
            }

            if (dr["SEMESTRE"] != null && !string.IsNullOrEmpty(dr["SEMESTRE"].ToString()))
            {
              if ((Convert.ToDecimal(dr["SEMESTRE"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["SEMESTRE"]).ToString("0.############").Split(',')[0].Length > 2) || (Convert.ToDecimal(dr["SEMESTRE"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["SEMESTRE"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O semestre deve ser um valor inteiro com 2 dígitos no máximo.";
              }
            }

            if (dr["STATUS"] == null || string.IsNullOrEmpty(dr["STATUS"].ToString()))
            {
              return "Favor informar o status.";
            }

            if (dr["STATUS"] != null && !string.IsNullOrEmpty(dr["STATUS"].ToString()))
            {
              if (dr["STATUS"].ToString().Length > 40)
              {
                return "O status deve possuir no máximo 40 caracteres.";
              }
            }
            if (dr["TIPO"] == null || string.IsNullOrEmpty(dr["TIPO"].ToString()))
            {
                return "Favor informar o tipo.";
            }

            if (dr["TIPO"] != null && !string.IsNullOrEmpty(dr["TIPO"].ToString()))
            {
                if (dr["TIPO"].ToString().Length > 40)
                {
                    return "O TIPO deve possuir no máximo 40 caracteres.";
                }
            }

            if (dr["DT_INICIO"] == null || string.IsNullOrEmpty(dr["DT_INICIO"].ToString()))
            {
              return "Favor informar a data de início do concurso.";
            }

            if (dr["DT_INICIO"] != null && dr["DT_INICIO"].ToString() != "")
            {
              dr["DT_INICIO"] = Convert.ToDateTime(dr["DT_INICIO"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_FIM"] == null || string.IsNullOrEmpty(dr["DT_FIM"].ToString()))
            {
              return "Favor informar a data final do concurso.";
            }

            if (dr["DT_FIM"] != null && dr["DT_FIM"].ToString() != "")
            {
              dr["DT_FIM"] = Convert.ToDateTime(dr["DT_FIM"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_INI_INSCR"] != null && dr["DT_INI_INSCR"].ToString() != "")
            {
              dr["DT_INI_INSCR"] = Convert.ToDateTime(dr["DT_INI_INSCR"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_FIM_INSCR"] != null && dr["DT_FIM_INSCR"].ToString() != "")
            {
              dr["DT_FIM_INSCR"] = Convert.ToDateTime(dr["DT_FIM_INSCR"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_INI_CONSULTA"] != null && dr["DT_INI_CONSULTA"].ToString() != "")
            {
              dr["DT_INI_CONSULTA"] = Convert.ToDateTime(dr["DT_INI_CONSULTA"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_FIM_CONSULTA"] != null && dr["DT_FIM_CONSULTA"].ToString() != "")
            {
              dr["DT_FIM_CONSULTA"] = Convert.ToDateTime(dr["DT_FIM_CONSULTA"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_INI_CONVOCACAO"] != null && dr["DT_INI_CONVOCACAO"].ToString() != "")
            {
              dr["DT_INI_CONVOCACAO"] = Convert.ToDateTime(dr["DT_INI_CONVOCACAO"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_FIM_CONVOCACAO"] != null && dr["DT_FIM_CONVOCACAO"].ToString() != "")
            {
              dr["DT_FIM_CONVOCACAO"] = Convert.ToDateTime(dr["DT_FIM_CONVOCACAO"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_INI_INGRESSO"] != null && dr["DT_INI_INGRESSO"].ToString() != "")
            {
              dr["DT_INI_INGRESSO"] = Convert.ToDateTime(dr["DT_INI_INGRESSO"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_FIM_INGRESSO"] != null && dr["DT_FIM_INGRESSO"].ToString() != "")
            {
              dr["DT_FIM_INGRESSO"] = Convert.ToDateTime(dr["DT_FIM_INGRESSO"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["NR_DIGITOS_CODIGO"] == null || string.IsNullOrEmpty(dr["NR_DIGITOS_CODIGO"].ToString()))
            {
              return "Favor informar o número de dígitos do código do candidato.";
            }

            if (dr["NR_DIGITOS_CODIGO"] != null && !string.IsNullOrEmpty(dr["NR_DIGITOS_CODIGO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["NR_DIGITOS_CODIGO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["NR_DIGITOS_CODIGO"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["NR_DIGITOS_CODIGO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["NR_DIGITOS_CODIGO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O número de dígitos do código do candidato deve ser um valor inteiro com 3 dígitos no máximo.";
              }
            }

            if (dr["QT_DIAS_CONVOCACAO"] != null && !string.IsNullOrEmpty(dr["QT_DIAS_CONVOCACAO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["QT_DIAS_CONVOCACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["QT_DIAS_CONVOCACAO"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["QT_DIAS_CONVOCACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["QT_DIAS_CONVOCACAO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A quantidade de dias para a convocação deve ser um valor inteiro com 3 dígitos no máximo.";
              }
            }

            if (dr["QT_DIAS_REPROVACAO"] != null && !string.IsNullOrEmpty(dr["QT_DIAS_REPROVACAO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["QT_DIAS_REPROVACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["QT_DIAS_REPROVACAO"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["QT_DIAS_REPROVACAO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["QT_DIAS_REPROVACAO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A quantidade de dias para a reprovação deve ser um valor inteiro com 3 dígitos no máximo.";
              }
            }

            if (dr["NR_RESOLUCAO"] != null && !string.IsNullOrEmpty(dr["NR_RESOLUCAO"].ToString()))
            {
              if (dr["NR_RESOLUCAO"].ToString().Length > 20)
              {
                return "O número da resolução deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["PONTUACAO_MINIMA"] == null || string.IsNullOrEmpty(dr["PONTUACAO_MINIMA"].ToString()))
            {
              return "Favor informar a pontuação mínima.";
            }

            if (dr["PONTUACAO_MINIMA"] != null && !string.IsNullOrEmpty(dr["PONTUACAO_MINIMA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["PONTUACAO_MINIMA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PONTUACAO_MINIMA"]).ToString("0.############").Split(',')[0].Length > 15) || (Convert.ToDecimal(dr["PONTUACAO_MINIMA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["PONTUACAO_MINIMA"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A pontuação mínima deve ser um valor inteiro com 15 dígitos no máximo.";
              }
            }

            if (dr["PORTARIA"] != null && !string.IsNullOrEmpty(dr["PORTARIA"].ToString()))
            {
              if (dr["PORTARIA"].ToString().Length > 100)
              {
                return "A portaria deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["DT_PUBLICACAO_DO"] != null && dr["DT_PUBLICACAO_DO"].ToString() != "")
            {
              dr["DT_PUBLICACAO_DO"] = Convert.ToDateTime(dr["DT_PUBLICACAO_DO"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["RESTRICAOANOS"] != null && !string.IsNullOrEmpty(dr["RESTRICAOANOS"].ToString()))
            {
              if ((Convert.ToDecimal(dr["RESTRICAOANOS"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["RESTRICAOANOS"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["RESTRICAOANOS"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["RESTRICAOANOS"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "Os anos de restrição deve ser um valor inteiro com 3 dígitos no máximo.";
              }
            }

            if (dr["OBSERVACAO"] != null && !string.IsNullOrEmpty(dr["OBSERVACAO"].ToString()))
            {
              if (dr["OBSERVACAO"].ToString().Length > 2000)
              {
                return "A observação deve possuir no máximo 2000 caracteres.";
              }
            }
            if (dr["INDIGENA"] != null && !string.IsNullOrEmpty(dr["INDIGENA"].ToString()))
            {
                if (dr["INDIGENA"].ToString().Length > 1)
                {
                    return "O Indigena deve possuir no máximo 1 caracter.";
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

    public static Ly_concurso_docente Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_concurso_docente Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_concurso_docente tab = new Ly_concurso_docente();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_concurso_docente tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @concurso, string @descricao, decimal? @ano, decimal? @semestre, string @status, string @tipo,
        DateTime? @dt_inicio, DateTime? @dt_fim, DateTime? @dt_ini_inscr, DateTime? @dt_fim_inscr, 
        DateTime? @dt_ini_consulta, DateTime? @dt_fim_consulta, DateTime? @dt_ini_convocacao, 
        DateTime? @dt_fim_convocacao, DateTime? @dt_ini_ingresso, DateTime? @dt_fim_ingresso, 
        decimal? @nr_digitos_codigo, decimal? @qt_dias_convocacao, decimal? @qt_dias_reprovacao, 
        string @nr_resolucao, decimal? @pontuacao_minima, string @portaria, DateTime? @dt_publicacao_do, 
        decimal? @restricaoanos, string @observacao, string @funcaoid, string @indigena
          ) {
        return (Row)Add(new object[] {
          @concurso, @descricao, @ano, @semestre, @status,@tipo, @dt_inicio, @dt_fim, @dt_ini_inscr, 
          @dt_fim_inscr, @dt_ini_consulta, @dt_fim_consulta, @dt_ini_convocacao, @dt_fim_convocacao, 
          @dt_ini_ingresso, @dt_fim_ingresso, @nr_digitos_codigo, @qt_dias_convocacao, @qt_dias_reprovacao, 
          @nr_resolucao, @pontuacao_minima, @portaria, @dt_publicacao_do, @restricaoanos, @observacao, @funcaoid,@indigena
        });
      }

      public Row Find(
        string @concurso
      ) {
        return (Row)Find(new object[] {
          @concurso
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
      /// CONCURSO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Concurso { get { return (string)this["concurso"]; } set { this["concurso"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO: VARCHAR(100), Not Null
      /// </summary>
      public string Descricao { get { return (string)this["descricao"]; } set { this["descricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ANO: NUMERIC(4), Not Null
      /// </summary>
      public decimal? Ano { get { return (decimal?)this["ano"]; } set { this["ano"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SEMESTRE: NUMERIC(2), Not Null
      /// </summary>
      public decimal? Semestre { get { return (decimal?)this["semestre"]; } set { this["semestre"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// STATUS: VARCHAR(40), Not Null
      /// </summary>
      public string Status { get { return (string)this["status"]; } set { this["status"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TIPO: VARCHAR(40), Not Null
      /// </summary>
      public string Tipo { get { return (string)this["tipo"]; } set { this["tipo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }

        /// <summary>
      /// DT_INICIO: DATETIME, Not Null
      /// </summary>
      public DateTime? Dt_inicio { get { return (DateTime?)this["dt_inicio"]; } set { this["dt_inicio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_FIM: DATETIME, Not Null
      /// </summary>
      public DateTime? Dt_fim { get { return (DateTime?)this["dt_fim"]; } set { this["dt_fim"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_INI_INSCR: DATETIME
      /// </summary>
      public DateTime? Dt_ini_inscr { get { return (DateTime?)this["dt_ini_inscr"]; } set { this["dt_ini_inscr"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_FIM_INSCR: DATETIME
      /// </summary>
      public DateTime? Dt_fim_inscr { get { return (DateTime?)this["dt_fim_inscr"]; } set { this["dt_fim_inscr"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_INI_CONSULTA: DATETIME
      /// </summary>
      public DateTime? Dt_ini_consulta { get { return (DateTime?)this["dt_ini_consulta"]; } set { this["dt_ini_consulta"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_FIM_CONSULTA: DATETIME
      /// </summary>
      public DateTime? Dt_fim_consulta { get { return (DateTime?)this["dt_fim_consulta"]; } set { this["dt_fim_consulta"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_INI_CONVOCACAO: DATETIME
      /// </summary>
      public DateTime? Dt_ini_convocacao { get { return (DateTime?)this["dt_ini_convocacao"]; } set { this["dt_ini_convocacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_FIM_CONVOCACAO: DATETIME
      /// </summary>
      public DateTime? Dt_fim_convocacao { get { return (DateTime?)this["dt_fim_convocacao"]; } set { this["dt_fim_convocacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_INI_INGRESSO: DATETIME
      /// </summary>
      public DateTime? Dt_ini_ingresso { get { return (DateTime?)this["dt_ini_ingresso"]; } set { this["dt_ini_ingresso"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_FIM_INGRESSO: DATETIME
      /// </summary>
      public DateTime? Dt_fim_ingresso { get { return (DateTime?)this["dt_fim_ingresso"]; } set { this["dt_fim_ingresso"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NR_DIGITOS_CODIGO: NUMERIC(3), Not Null
      /// </summary>
      public decimal? Nr_digitos_codigo { get { return (decimal?)this["nr_digitos_codigo"]; } set { this["nr_digitos_codigo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// QT_DIAS_CONVOCACAO: NUMERIC(3)
      /// </summary>
      public decimal? Qt_dias_convocacao { get { return (decimal?)this["qt_dias_convocacao"]; } set { this["qt_dias_convocacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// QT_DIAS_REPROVACAO: NUMERIC(3)
      /// </summary>
      public decimal? Qt_dias_reprovacao { get { return (decimal?)this["qt_dias_reprovacao"]; } set { this["qt_dias_reprovacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NR_RESOLUCAO: VARCHAR(20)
      /// </summary>
      public string Nr_resolucao { get { return (string)this["nr_resolucao"]; } set { this["nr_resolucao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PONTUACAO_MINIMA: NUMERIC(15), Not Null
      /// </summary>
      public decimal? Pontuacao_minima { get { return (decimal?)this["pontuacao_minima"]; } set { this["pontuacao_minima"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PORTARIA: VARCHAR(100)
      /// </summary>
      public string Portaria { get { return (string)this["portaria"]; } set { this["portaria"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_PUBLICACAO_DO: DATETIME
      /// </summary>
      public DateTime? Dt_publicacao_do { get { return (DateTime?)this["dt_publicacao_do"]; } set { this["dt_publicacao_do"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// RESTRICAOANOS: NUMERIC(3)
      /// </summary>
      public decimal? Restricaoanos { get { return (decimal?)this["restricaoanos"]; } set { this["restricaoanos"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// OBSERVACAO: VARCHAR(2000)
      /// </summary>
      public string Observacao { get { return (string)this["observacao"]; } set { this["observacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
	  /// <summary>
	  /// FUNCAOID: VARCHAR(20)
	  /// </summary>
	  public string FuncaoID { get { return (string)this["funcaoid"]; } set { this["funcaoid"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// INDIGENA: CHAR(1)
      /// </summary>
      public string Indigena { get { return (string)this["indigena"]; } set { this["indigena"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @concurso, string @descricao, decimal? @ano, decimal? @semestre, string @status, string @tipo,
                               DateTime? @dt_inicio, DateTime? @dt_fim, decimal? @nr_digitos_codigo, decimal? @pontuacao_minima) {
        return Insert(connection,
                      @concurso, @descricao, @ano, @semestre, @status, @tipo, @dt_inicio, @dt_fim, @nr_digitos_codigo, 
                      @pontuacao_minima,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @concurso, string @descricao, decimal? @ano, decimal? @semestre, string @status, string @tipo,
                               DateTime? @dt_inicio, DateTime? @dt_fim, decimal? @nr_digitos_codigo, decimal? @pontuacao_minima,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_concurso_docente(),
          new string[] { "concurso", "descricao", "ano", "semestre", "status","tipo", "dt_inicio", 
                         "dt_fim", "nr_digitos_codigo", "pontuacao_minima" },
          new object[] { @concurso, @descricao, @ano, @semestre, @status,@tipo, @dt_inicio, @dt_fim, 
                         @nr_digitos_codigo, @pontuacao_minima },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkConcurso,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_concurso_docente(),
          new string[] { "concurso" },
          new object[] { pkConcurso },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkConcurso) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_concurso_docente(),
          new string[] { "concurso" },
          new object[] { pkConcurso }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkConcurso) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_concurso_docente(),
          new string[] { "concurso" },
          new object[] { pkConcurso }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Concurso { get { return (string)row["concurso", DataRowVersion.Original]; } }
        public string Descricao { get { return (string)row["descricao", DataRowVersion.Original]; } }
        public decimal? Ano { get { return (decimal?)row["ano", DataRowVersion.Original]; } }
        public decimal? Semestre { get { return (decimal?)row["semestre", DataRowVersion.Original]; } }
        public string Status { get { return (string)row["status", DataRowVersion.Original]; } }
        public string Tipo { get { return (string)row["tipo", DataRowVersion.Original]; } }
        public DateTime? Dt_inicio { get { return (DateTime?)row["dt_inicio", DataRowVersion.Original]; } }
        public DateTime? Dt_fim { get { return (DateTime?)row["dt_fim", DataRowVersion.Original]; } }
        public DateTime? Dt_ini_inscr { get { return (DateTime?)row["dt_ini_inscr", DataRowVersion.Original]; } }
        public DateTime? Dt_fim_inscr { get { return (DateTime?)row["dt_fim_inscr", DataRowVersion.Original]; } }
        public DateTime? Dt_ini_consulta { get { return (DateTime?)row["dt_ini_consulta", DataRowVersion.Original]; } }
        public DateTime? Dt_fim_consulta { get { return (DateTime?)row["dt_fim_consulta", DataRowVersion.Original]; } }
        public DateTime? Dt_ini_convocacao { get { return (DateTime?)row["dt_ini_convocacao", DataRowVersion.Original]; } }
        public DateTime? Dt_fim_convocacao { get { return (DateTime?)row["dt_fim_convocacao", DataRowVersion.Original]; } }
        public DateTime? Dt_ini_ingresso { get { return (DateTime?)row["dt_ini_ingresso", DataRowVersion.Original]; } }
        public DateTime? Dt_fim_ingresso { get { return (DateTime?)row["dt_fim_ingresso", DataRowVersion.Original]; } }
        public decimal? Nr_digitos_codigo { get { return (decimal?)row["nr_digitos_codigo", DataRowVersion.Original]; } }
        public decimal? Qt_dias_convocacao { get { return (decimal?)row["qt_dias_convocacao", DataRowVersion.Original]; } }
        public decimal? Qt_dias_reprovacao { get { return (decimal?)row["qt_dias_reprovacao", DataRowVersion.Original]; } }
        public string Nr_resolucao { get { return (string)row["nr_resolucao", DataRowVersion.Original]; } }
        public decimal? Pontuacao_minima { get { return (decimal?)row["pontuacao_minima", DataRowVersion.Original]; } }
        public string Portaria { get { return (string)row["portaria", DataRowVersion.Original]; } }
        public DateTime? Dt_publicacao_do { get { return (DateTime?)row["dt_publicacao_do", DataRowVersion.Original]; } }
        public decimal? Restricaoanos { get { return (decimal?)row["restricaoanos", DataRowVersion.Original]; } }
        public string Observacao { get { return (string)row["observacao", DataRowVersion.Original]; } }
		public string FuncaoID { get { return (string)row["funcaoid", DataRowVersion.Original]; } }
        public string Indigena { get { return (string)row["indigena", DataRowVersion.Original]; } }
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
    #region Coluna: CONCURSO
    private ColumnConcurso pvColConcurso;

    public sealed class ColumnConcurso : TDataColumn {

      public ColumnConcurso() {
        this.ColumnName = "concurso";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "concurso";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_docente.concurso"; } }
      public override string OriginalCol { get { return "concurso"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOCENTE.CONCURSO"; } }
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


      protected override string GetName(){return "descrição";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_docente.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOCENTE.DESCRICAO"; } }
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
      public override string FullCol { get { return "ly_concurso_docente.ano"; } }
      public override string OriginalCol { get { return "ano"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOCENTE.ANO"; } }
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

    }
    #endregion

    #region Coluna: SEMESTRE
    private ColumnSemestre pvColSemestre;

    public sealed class ColumnSemestre : TDataColumn {

      public ColumnSemestre() {
        this.ColumnName = "semestre";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "semestre";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_docente.semestre"; } }
      public override string OriginalCol { get { return "semestre"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOCENTE.SEMESTRE"; } }
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

    }
    #endregion

    #region Coluna: STATUS
    private ColumnStatus pvColStatus;

    public sealed class ColumnStatus : TDataColumn {

      public ColumnStatus() {
        this.ColumnName = "status";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "status";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_docente.status"; } }
      public override string OriginalCol { get { return "status"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOCENTE.STATUS"; } }
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

    #region Coluna: TIPO
    private ColumnTipo pvColTipo;

    public sealed class ColumnTipo : TDataColumn
    {

        public ColumnTipo()
        {
            this.ColumnName = "tipo";
            this.DataType = typeof(string);

        }


        protected override string GetName() { return "tipo"; }
        protected override string GetCaption() { return ""; }
        protected override string GetDescricao() { return ""; }
        public override string FullCol { get { return "ly_concurso_docente.tipo"; } }
        public override string OriginalCol { get { return "tipo"; } }
        public override string RefCol { get { return "LY_CONCURSO_DOCENTE.TIPO"; } }
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

    #region Coluna: DT_INICIO
    private ColumnDt_inicio pvColDt_inicio;

    public sealed class ColumnDt_inicio : TDataColumn {

      public ColumnDt_inicio() {
        this.ColumnName = "dt_inicio";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data de início do concurso";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_docente.dt_inicio"; } }
      public override string OriginalCol { get { return "dt_inicio"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOCENTE.DT_INICIO"; } }
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

    #region Coluna: DT_FIM
    private ColumnDt_fim pvColDt_fim;

    public sealed class ColumnDt_fim : TDataColumn {

      public ColumnDt_fim() {
        this.ColumnName = "dt_fim";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data final do concurso";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_docente.dt_fim"; } }
      public override string OriginalCol { get { return "dt_fim"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOCENTE.DT_FIM"; } }
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

    #region Coluna: DT_INI_INSCR
    private ColumnDt_ini_inscr pvColDt_ini_inscr;

    public sealed class ColumnDt_ini_inscr : TDataColumn {

      public ColumnDt_ini_inscr() {
        this.ColumnName = "dt_ini_inscr";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data inicial da inscrição";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_docente.dt_ini_inscr"; } }
      public override string OriginalCol { get { return "dt_ini_inscr"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOCENTE.DT_INI_INSCR"; } }
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

    #region Coluna: DT_FIM_INSCR
    private ColumnDt_fim_inscr pvColDt_fim_inscr;

    public sealed class ColumnDt_fim_inscr : TDataColumn {

      public ColumnDt_fim_inscr() {
        this.ColumnName = "dt_fim_inscr";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data final da inscrição";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_docente.dt_fim_inscr"; } }
      public override string OriginalCol { get { return "dt_fim_inscr"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOCENTE.DT_FIM_INSCR"; } }
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

    #region Coluna: DT_INI_CONSULTA
    private ColumnDt_ini_consulta pvColDt_ini_consulta;

    public sealed class ColumnDt_ini_consulta : TDataColumn {

      public ColumnDt_ini_consulta() {
        this.ColumnName = "dt_ini_consulta";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data inicial da consulta";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_docente.dt_ini_consulta"; } }
      public override string OriginalCol { get { return "dt_ini_consulta"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOCENTE.DT_INI_CONSULTA"; } }
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

    #region Coluna: DT_FIM_CONSULTA
    private ColumnDt_fim_consulta pvColDt_fim_consulta;

    public sealed class ColumnDt_fim_consulta : TDataColumn {

      public ColumnDt_fim_consulta() {
        this.ColumnName = "dt_fim_consulta";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data final da consulta";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_docente.dt_fim_consulta"; } }
      public override string OriginalCol { get { return "dt_fim_consulta"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOCENTE.DT_FIM_CONSULTA"; } }
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

    #region Coluna: DT_INI_CONVOCACAO
    private ColumnDt_ini_convocacao pvColDt_ini_convocacao;

    public sealed class ColumnDt_ini_convocacao : TDataColumn {

      public ColumnDt_ini_convocacao() {
        this.ColumnName = "dt_ini_convocacao";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data inicial da convocação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_docente.dt_ini_convocacao"; } }
      public override string OriginalCol { get { return "dt_ini_convocacao"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOCENTE.DT_INI_CONVOCACAO"; } }
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

    #region Coluna: DT_FIM_CONVOCACAO
    private ColumnDt_fim_convocacao pvColDt_fim_convocacao;

    public sealed class ColumnDt_fim_convocacao : TDataColumn {

      public ColumnDt_fim_convocacao() {
        this.ColumnName = "dt_fim_convocacao";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data final da convocação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_docente.dt_fim_convocacao"; } }
      public override string OriginalCol { get { return "dt_fim_convocacao"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOCENTE.DT_FIM_CONVOCACAO"; } }
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

    #region Coluna: DT_INI_INGRESSO
    private ColumnDt_ini_ingresso pvColDt_ini_ingresso;

    public sealed class ColumnDt_ini_ingresso : TDataColumn {

      public ColumnDt_ini_ingresso() {
        this.ColumnName = "dt_ini_ingresso";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data inicial do ingresso";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_docente.dt_ini_ingresso"; } }
      public override string OriginalCol { get { return "dt_ini_ingresso"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOCENTE.DT_INI_INGRESSO"; } }
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

    #region Coluna: DT_FIM_INGRESSO
    private ColumnDt_fim_ingresso pvColDt_fim_ingresso;

    public sealed class ColumnDt_fim_ingresso : TDataColumn {

      public ColumnDt_fim_ingresso() {
        this.ColumnName = "dt_fim_ingresso";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data final do ingresso";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_docente.dt_fim_ingresso"; } }
      public override string OriginalCol { get { return "dt_fim_ingresso"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOCENTE.DT_FIM_INGRESSO"; } }
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

    #region Coluna: NR_DIGITOS_CODIGO
    private ColumnNr_digitos_codigo pvColNr_digitos_codigo;

    public sealed class ColumnNr_digitos_codigo : TDataColumn {

      public ColumnNr_digitos_codigo() {
        this.ColumnName = "nr_digitos_codigo";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "número de dígitos do código do candidato";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_docente.nr_digitos_codigo"; } }
      public override string OriginalCol { get { return "nr_digitos_codigo"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOCENTE.NR_DIGITOS_CODIGO"; } }
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

    #region Coluna: QT_DIAS_CONVOCACAO
    private ColumnQt_dias_convocacao pvColQt_dias_convocacao;

    public sealed class ColumnQt_dias_convocacao : TDataColumn {

      public ColumnQt_dias_convocacao() {
        this.ColumnName = "qt_dias_convocacao";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "quantidade de dias para a convocação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_docente.qt_dias_convocacao"; } }
      public override string OriginalCol { get { return "qt_dias_convocacao"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOCENTE.QT_DIAS_CONVOCACAO"; } }
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

    #region Coluna: QT_DIAS_REPROVACAO
    private ColumnQt_dias_reprovacao pvColQt_dias_reprovacao;

    public sealed class ColumnQt_dias_reprovacao : TDataColumn {

      public ColumnQt_dias_reprovacao() {
        this.ColumnName = "qt_dias_reprovacao";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "quantidade de dias para a reprovação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_docente.qt_dias_reprovacao"; } }
      public override string OriginalCol { get { return "qt_dias_reprovacao"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOCENTE.QT_DIAS_REPROVACAO"; } }
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

    #region Coluna: NR_RESOLUCAO
    private ColumnNr_resolucao pvColNr_resolucao;

    public sealed class ColumnNr_resolucao : TDataColumn {

      public ColumnNr_resolucao() {
        this.ColumnName = "nr_resolucao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "número da resolução";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_docente.nr_resolucao"; } }
      public override string OriginalCol { get { return "nr_resolucao"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOCENTE.NR_RESOLUCAO"; } }
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

    #region Coluna: PONTUACAO_MINIMA
    private ColumnPontuacao_minima pvColPontuacao_minima;

    public sealed class ColumnPontuacao_minima : TDataColumn {

      public ColumnPontuacao_minima() {
        this.ColumnName = "pontuacao_minima";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "pontuação mínima";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_docente.pontuacao_minima"; } }
      public override string OriginalCol { get { return "pontuacao_minima"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOCENTE.PONTUACAO_MINIMA"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 16; } }
      public override int Precision { get { return 15; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(15)"; } }

    }
    #endregion

    #region Coluna: PORTARIA
    private ColumnPortaria pvColPortaria;

    public sealed class ColumnPortaria : TDataColumn {

      public ColumnPortaria() {
        this.ColumnName = "portaria";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "portaria";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_docente.portaria"; } }
      public override string OriginalCol { get { return "portaria"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOCENTE.PORTARIA"; } }
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

    #region Coluna: DT_PUBLICACAO_DO
    private ColumnDt_publicacao_do pvColDt_publicacao_do;

    public sealed class ColumnDt_publicacao_do : TDataColumn {

      public ColumnDt_publicacao_do() {
        this.ColumnName = "dt_publicacao_do";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data de publicação no diário oficial";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_docente.dt_publicacao_do"; } }
      public override string OriginalCol { get { return "dt_publicacao_do"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOCENTE.DT_PUBLICACAO_DO"; } }
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

    #region Coluna: RESTRICAOANOS
    private ColumnRestricaoanos pvColRestricaoanos;

    public sealed class ColumnRestricaoanos : TDataColumn {

      public ColumnRestricaoanos() {
        this.ColumnName = "restricaoanos";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "anos de restrição";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_docente.restricaoanos"; } }
      public override string OriginalCol { get { return "restricaoanos"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOCENTE.RESTRICAOANOS"; } }
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

    #region Coluna: OBSERVACAO
    private ColumnObservacao pvColObservacao;

    public sealed class ColumnObservacao : TDataColumn {

      public ColumnObservacao() {
        this.ColumnName = "observacao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "observação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_docente.observacao"; } }
      public override string OriginalCol { get { return "observacao"; } }
      public override string RefCol { get { return "LY_CONCURSO_DOCENTE.OBSERVACAO"; } }
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

	#region Coluna: FUNCAOID
	private ColumnFuncaoID pvColFuncaoID;

	public sealed class ColumnFuncaoID : TDataColumn
	{

		public ColumnFuncaoID()
		{
			this.ColumnName = "funcaoid";
			this.DataType = typeof(string);

		}


		protected override string GetName() { return "código da função"; }
		protected override string GetCaption() { return ""; }
		protected override string GetDescricao() { return ""; }
		public override string FullCol { get { return "ly_concurso_docente.funcaoid"; } }
		public override string OriginalCol { get { return "funcaoid"; } }
		public override string RefCol { get { return "LY_CONCURSO_DOCENTE.FUNCAOID"; } }
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

    #region Coluna: INDIGENA
    private ColumnIndigena pvColIndigena;

    public sealed class ColumnIndigena : TDataColumn
    {

        public ColumnIndigena()
        {
            this.ColumnName = "Indigena";
            this.DataType = typeof(string);

        }


        protected override string GetName() { return "Tipo Indigena"; }
        protected override string GetCaption() { return ""; }
        protected override string GetDescricao() { return ""; }
        public override string FullCol { get { return "ly_concurso_docente.Indigena"; } }
        public override string OriginalCol { get { return "Indigena"; } }
        public override string RefCol { get { return "LY_CONCURSO_DOCENTE.INDIGENA"; } }
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

  public partial class Ly_concurso_experiencia : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_concurso_experiencia() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_concurso_experiencia");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_concurso_experiencia");
      }
      catch{}
    }

    public Ly_concurso_experiencia() {
      pvColExperiencia = new ColumnExperiencia(); Columns.Add(pvColExperiencia);
      pvColDescricao = new ColumnDescricao(); Columns.Add(pvColDescricao);
      pvColOrigem = new ColumnOrigem(); Columns.Add(pvColOrigem);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_concurso_experiencia");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @experiencia, string @descricao) {
      Row row = NewRow();

      row.Experiencia = @experiencia;
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
      get { return "LY_CONCURSO_EXPERIENCIA"; }
    }

    protected override sealed string Name { get { return "Experiência do concurso de docentes"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColExperiencia });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_concurso_experiencia_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_concurso_experiencia_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_concurso_experiencia_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_concurso_experiencia_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkExperiencia", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "EXPERIENCIA", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("experiencia", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "EXPERIENCIA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("descricao", OleDbType.VarChar, 600, ParameterDirection.InputOutput, true, 0, 0, "DESCRICAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("origem", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "ORIGEM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "experiencia") { result.Add(toLower ? "experiencia" : column); continue; }
          if(lower == "descricao") { result.Add(toLower ? "descricao" : column); continue; }
          if(lower == "origem") { result.Add(toLower ? "origem" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "experiencia", "descricao", "origem" }
                          : new string[] { "EXPERIENCIA", "DESCRICAO", "ORIGEM" });
        else
          result.AddRange(toLower
                          ? new string[] { "experiencia", "descricao", "origem" }
                          : new string[] { "EXPERIENCIA", "DESCRICAO", "ORIGEM" });

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
        "ly_concurso_experiencia.experiencia"
      });
      if(scColumns.Contains("descricao")) list.Add("ly_concurso_experiencia.descricao");
      if(scColumns.Contains("origem")) list.Add("ly_concurso_experiencia.origem");
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
          "FROM ly_concurso_experiencia " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_concurso_experiencia");
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
        da.TableMappings.Add("Table", "Ly_concurso_experiencia");
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
            if (dr["EXPERIENCIA"] != null && string.IsNullOrEmpty(dr["EXPERIENCIA"].ToString()))
              dr["EXPERIENCIA"] = DBNull.Value;
            if (dr["DESCRICAO"] != null && string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
              dr["DESCRICAO"] = DBNull.Value;
            if (dr["ORIGEM"] != null && string.IsNullOrEmpty(dr["ORIGEM"].ToString()))
              dr["ORIGEM"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["EXPERIENCIA"] == null || string.IsNullOrEmpty(dr["EXPERIENCIA"].ToString()))
            {
              return "Favor informar a experiência.";
            }

            if (dr["EXPERIENCIA"] != null && !string.IsNullOrEmpty(dr["EXPERIENCIA"].ToString()))
            {
              if (dr["EXPERIENCIA"].ToString().Length > 20)
              {
                return "A experiência deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["DESCRICAO"] == null || string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              return "Favor informar a descrição.";
            }

            if (dr["DESCRICAO"] != null && !string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              if (dr["DESCRICAO"].ToString().Length > 500)
              {
                return "A descrição deve possuir no máximo 500 caracteres.";
              }
            }

            if (dr["ORIGEM"] != null && !string.IsNullOrEmpty(dr["ORIGEM"].ToString()))
            {
              if (dr["ORIGEM"].ToString().Length > 50)
              {
                return "O Origem deve possuir no máximo 50 caracteres.";
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

    public static Ly_concurso_experiencia Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_concurso_experiencia Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_concurso_experiencia tab = new Ly_concurso_experiencia();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_concurso_experiencia tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @experiencia, string @descricao, string @origem
      ) {
        return (Row)Add(new object[] {
          @experiencia, @descricao, @origem
        });
      }

      public Row Find(
        string @experiencia
      ) {
        return (Row)Find(new object[] {
          @experiencia
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
      /// EXPERIENCIA: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Experiencia { get { return (string)this["experiencia"]; } set { this["experiencia"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO: VARCHAR(500), Not Null
      /// </summary>
      public string Descricao { get { return (string)this["descricao"]; } set { this["descricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ORIGEM: VARCHAR(50)
      /// </summary>
      public string Origem { get { return (string)this["origem"]; } set { this["origem"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @experiencia, string @descricao) {
        return Insert(connection,
                      @experiencia, @descricao,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @experiencia, string @descricao,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_concurso_experiencia(),
          new string[] { "experiencia", "descricao" },
          new object[] { @experiencia, @descricao },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkExperiencia,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_concurso_experiencia(),
          new string[] { "experiencia" },
          new object[] { pkExperiencia },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkExperiencia) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_concurso_experiencia(),
          new string[] { "experiencia" },
          new object[] { pkExperiencia }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkExperiencia) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_concurso_experiencia(),
          new string[] { "experiencia" },
          new object[] { pkExperiencia }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Experiencia { get { return (string)row["experiencia", DataRowVersion.Original]; } }
        public string Descricao { get { return (string)row["descricao", DataRowVersion.Original]; } }
        public string Origem { get { return (string)row["origem", DataRowVersion.Original]; } }

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
    #region Coluna: EXPERIENCIA
    private ColumnExperiencia pvColExperiencia;

    public sealed class ColumnExperiencia : TDataColumn {

      public ColumnExperiencia() {
        this.ColumnName = "experiencia";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "experiência";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_experiencia.experiencia"; } }
      public override string OriginalCol { get { return "experiencia"; } }
      public override string RefCol { get { return "LY_CONCURSO_EXPERIENCIA.EXPERIENCIA"; } }
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


      protected override string GetName(){return "descrição";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_experiencia.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "LY_CONCURSO_EXPERIENCIA.DESCRICAO"; } }
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

    #region Coluna: ORIGEM
    private ColumnOrigem pvColOrigem;

    public sealed class ColumnOrigem : TDataColumn {

      public ColumnOrigem() {
        this.ColumnName = "origem";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_experiencia.origem"; } }
      public override string OriginalCol { get { return "origem"; } }
      public override string RefCol { get { return "LY_CONCURSO_EXPERIENCIA.ORIGEM"; } }
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

  public partial class Ly_concurso_titulacao : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_concurso_titulacao() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_concurso_titulacao");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_concurso_titulacao");
      }
      catch{}
    }

    public Ly_concurso_titulacao() {
      pvColTitulacao = new ColumnTitulacao(); Columns.Add(pvColTitulacao);
      pvColDescricao = new ColumnDescricao(); Columns.Add(pvColDescricao);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_concurso_titulacao");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @titulacao, string @descricao) {
      Row row = NewRow();

      row.Titulacao = @titulacao;
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
      get { return "LY_CONCURSO_TITULACAO"; }
    }

    protected override sealed string Name { get { return "Titulação do concurso de docentes"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColTitulacao });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_concurso_titulacao_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_concurso_titulacao_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_concurso_titulacao_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_concurso_titulacao_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkTitulacao", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "TITULACAO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("titulacao", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TITULACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("descricao", OleDbType.VarChar, 600, ParameterDirection.InputOutput, true, 0, 0, "DESCRICAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "titulacao") { result.Add(toLower ? "titulacao" : column); continue; }
          if(lower == "descricao") { result.Add(toLower ? "descricao" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "titulacao", "descricao" }
                          : new string[] { "TITULACAO", "DESCRICAO" });
        else
          result.AddRange(toLower
                          ? new string[] { "titulacao", "descricao" }
                          : new string[] { "TITULACAO", "DESCRICAO" });

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
        "ly_concurso_titulacao.titulacao"
      });
      if(scColumns.Contains("descricao")) list.Add("ly_concurso_titulacao.descricao");
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
          "FROM ly_concurso_titulacao " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_concurso_titulacao");
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
        da.TableMappings.Add("Table", "Ly_concurso_titulacao");
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
            if (dr["TITULACAO"] != null && string.IsNullOrEmpty(dr["TITULACAO"].ToString()))
              dr["TITULACAO"] = DBNull.Value;
            if (dr["DESCRICAO"] != null && string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
              dr["DESCRICAO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["TITULACAO"] == null || string.IsNullOrEmpty(dr["TITULACAO"].ToString()))
            {
              return "Favor informar a titulação.";
            }

            if (dr["TITULACAO"] != null && !string.IsNullOrEmpty(dr["TITULACAO"].ToString()))
            {
              if (dr["TITULACAO"].ToString().Length > 20)
              {
                return "A titulação deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["DESCRICAO"] == null || string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              return "Favor informar a descrição.";
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

    public static Ly_concurso_titulacao Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_concurso_titulacao Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_concurso_titulacao tab = new Ly_concurso_titulacao();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_concurso_titulacao tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @titulacao, string @descricao
      ) {
        return (Row)Add(new object[] {
          @titulacao, @descricao
        });
      }

      public Row Find(
        string @titulacao
      ) {
        return (Row)Find(new object[] {
          @titulacao
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
      /// TITULACAO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Titulacao { get { return (string)this["titulacao"]; } set { this["titulacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO: VARCHAR(500), Not Null
      /// </summary>
      public string Descricao { get { return (string)this["descricao"]; } set { this["descricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @titulacao, string @descricao) {
        return Insert(connection,
                      @titulacao, @descricao,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @titulacao, string @descricao,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_concurso_titulacao(),
          new string[] { "titulacao", "descricao" },
          new object[] { @titulacao, @descricao },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkTitulacao,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_concurso_titulacao(),
          new string[] { "titulacao" },
          new object[] { pkTitulacao },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkTitulacao) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_concurso_titulacao(),
          new string[] { "titulacao" },
          new object[] { pkTitulacao }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkTitulacao) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_concurso_titulacao(),
          new string[] { "titulacao" },
          new object[] { pkTitulacao }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Titulacao { get { return (string)row["titulacao", DataRowVersion.Original]; } }
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
    #region Coluna: TITULACAO
    private ColumnTitulacao pvColTitulacao;

    public sealed class ColumnTitulacao : TDataColumn {

      public ColumnTitulacao() {
        this.ColumnName = "titulacao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "titulação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_titulacao.titulacao"; } }
      public override string OriginalCol { get { return "titulacao"; } }
      public override string RefCol { get { return "LY_CONCURSO_TITULACAO.TITULACAO"; } }
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


      protected override string GetName(){return "descrição";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_concurso_titulacao.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "LY_CONCURSO_TITULACAO.DESCRICAO"; } }
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
    #endregion
  }
}

