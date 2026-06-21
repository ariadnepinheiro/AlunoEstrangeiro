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
  public class Frequencia : TDataSet {
    private const string DataSetName_Def = "Frequencia";
    private static ConstructorInfo ctorTCustom = null;
    private CustomBase tcustom = null;
    #region Variáveis private para as tabelas deste dataset
    private Ly_comp_lista pvTabLy_comp_lista;
    #endregion

    static Frequencia() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Frequencia");
      }
      catch{}
    }

    public Frequencia() : base() {
      DataSetName = DataSetName_Def;

      pvTabLy_comp_lista = new Ly_comp_lista();

      Tables.Add(pvTabLy_comp_lista);

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
          pvTabLy_comp_lista.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_comp_lista); if(row != null) return DataLib.RowErrorsToString(row);
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
    public Ly_comp_lista Ly_comp_lista { get { return pvTabLy_comp_lista; } }

    #endregion

    public abstract class CustomBase {
      public abstract string Put(Techne.Lyceum.CR.Frequencia ds, TConnectionWritable connection);
    }

  }

  public partial class Ly_comp_lista : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_comp_lista() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_comp_lista");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_comp_lista");
      }
      catch{}
    }

    public Ly_comp_lista() {
      pvColLista = new ColumnLista(); Columns.Add(pvColLista);
      pvColDisciplina = new ColumnDisciplina(); Columns.Add(pvColDisciplina);
      pvColTurma = new ColumnTurma(); Columns.Add(pvColTurma);
      pvColAno = new ColumnAno(); Columns.Add(pvColAno);
      pvColSemestre = new ColumnSemestre(); Columns.Add(pvColSemestre);
      pvColAluno = new ColumnAluno(); Columns.Add(pvColAluno);
      pvColOcorrencia = new ColumnOcorrencia(); Columns.Add(pvColOcorrencia);
      pvColObservacao = new ColumnObservacao(); Columns.Add(pvColObservacao);
      pvColStamp_atualizacao = new ColumnStamp_atualizacao(); Columns.Add(pvColStamp_atualizacao);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_comp_lista");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(decimal? @lista, string @disciplina, string @turma, decimal? @ano, 
                      decimal? @semestre, string @aluno, string @ocorrencia) {
      Row row = NewRow();

      row.Lista = @lista;
      row.Disciplina = @disciplina;
      row.Turma = @turma;
      row.Ano = @ano;
      row.Semestre = @semestre;
      row.Aluno = @aluno;
      row.Ocorrencia = @ocorrencia;

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
      get { return "LY_COMP_LISTA"; }
    }

    protected override sealed string Name { get { return "Frequência"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColLista, pvColDisciplina, pvColTurma, pvColAno, 
                                       pvColSemestre, pvColAluno });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_comp_lista_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_comp_lista_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_comp_lista_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_comp_lista_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkLista", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "LISTA", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkDisciplina", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "DISCIPLINA", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkTurma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "TURMA", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkAno", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ANO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkSemestre", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "SEMESTRE", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkAluno", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "ALUNO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("lista", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "LISTA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("disciplina", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "DISCIPLINA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("turma", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TURMA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("ano", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ANO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("semestre", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "SEMESTRE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("aluno", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "ALUNO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("ocorrencia", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "OCORRENCIA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("observacao", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "OBSERVACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("stamp_atualizacao", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "STAMP_ATUALIZACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));

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
          if(lower == "lista") { result.Add(toLower ? "lista" : column); continue; }
          if(lower == "disciplina") { result.Add(toLower ? "disciplina" : column); continue; }
          if(lower == "turma") { result.Add(toLower ? "turma" : column); continue; }
          if(lower == "ano") { result.Add(toLower ? "ano" : column); continue; }
          if(lower == "semestre") { result.Add(toLower ? "semestre" : column); continue; }
          if(lower == "aluno") { result.Add(toLower ? "aluno" : column); continue; }
          if(lower == "ocorrencia") { result.Add(toLower ? "ocorrencia" : column); continue; }
          if(lower == "observacao") { result.Add(toLower ? "observacao" : column); continue; }
          if(lower == "stamp_atualizacao") { result.Add(toLower ? "stamp_atualizacao" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "lista", "disciplina", "turma", "ano", "semestre", "aluno", "ocorrencia", 
                                           "observacao", "stamp_atualizacao" }
                          : new string[] { "LISTA", "DISCIPLINA", "TURMA", "ANO", "SEMESTRE", "ALUNO", "OCORRENCIA", 
                                           "OBSERVACAO", "STAMP_ATUALIZACAO" });
        else
          result.AddRange(toLower
                          ? new string[] { "lista", "disciplina", "turma", "ano", "semestre", "aluno", "ocorrencia", 
                                           "observacao", "stamp_atualizacao" }
                          : new string[] { "LISTA", "DISCIPLINA", "TURMA", "ANO", "SEMESTRE", "ALUNO", "OCORRENCIA", 
                                           "OBSERVACAO", "STAMP_ATUALIZACAO" });

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
        "ly_comp_lista.lista", "ly_comp_lista.disciplina", "ly_comp_lista.turma", "ly_comp_lista.ano", 
        "ly_comp_lista.semestre", "ly_comp_lista.aluno"
      });
      if(scColumns.Contains("ocorrencia")) list.Add("ly_comp_lista.ocorrencia");
      if(scColumns.Contains("observacao")) list.Add("ly_comp_lista.observacao");
      if(scColumns.Contains("stamp_atualizacao")) list.Add("ly_comp_lista.stamp_atualizacao");
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
          "FROM ly_comp_lista " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_comp_lista");
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
        da.TableMappings.Add("Table", "Ly_comp_lista");
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
            if (dr["LISTA"] != null && string.IsNullOrEmpty(dr["LISTA"].ToString()))
              dr["LISTA"] = DBNull.Value;
            if (dr["DISCIPLINA"] != null && string.IsNullOrEmpty(dr["DISCIPLINA"].ToString()))
              dr["DISCIPLINA"] = DBNull.Value;
            if (dr["TURMA"] != null && string.IsNullOrEmpty(dr["TURMA"].ToString()))
              dr["TURMA"] = DBNull.Value;
            if (dr["ANO"] != null && string.IsNullOrEmpty(dr["ANO"].ToString()))
              dr["ANO"] = DBNull.Value;
            if (dr["SEMESTRE"] != null && string.IsNullOrEmpty(dr["SEMESTRE"].ToString()))
              dr["SEMESTRE"] = DBNull.Value;
            if (dr["ALUNO"] != null && string.IsNullOrEmpty(dr["ALUNO"].ToString()))
              dr["ALUNO"] = DBNull.Value;
            if (dr["OCORRENCIA"] != null && string.IsNullOrEmpty(dr["OCORRENCIA"].ToString()))
              dr["OCORRENCIA"] = DBNull.Value;
            if (dr["OBSERVACAO"] != null && string.IsNullOrEmpty(dr["OBSERVACAO"].ToString()))
              dr["OBSERVACAO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["LISTA"] == null || string.IsNullOrEmpty(dr["LISTA"].ToString()))
            {
              return "Favor informar o lista.";
            }

            if (dr["LISTA"] != null && !string.IsNullOrEmpty(dr["LISTA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["LISTA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["LISTA"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["LISTA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["LISTA"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O lista deve ser um valor inteiro com 10 dígitos no máximo.";
              }
            }

            if (dr["DISCIPLINA"] == null || string.IsNullOrEmpty(dr["DISCIPLINA"].ToString()))
            {
              return "Favor informar o disciplina.";
            }

            if (dr["DISCIPLINA"] != null && !string.IsNullOrEmpty(dr["DISCIPLINA"].ToString()))
            {
              if (dr["DISCIPLINA"].ToString().Length > 20)
              {
                return "O disciplina deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["TURMA"] == null || string.IsNullOrEmpty(dr["TURMA"].ToString()))
            {
              return "Favor informar o truma.";
            }

            if (dr["TURMA"] != null && !string.IsNullOrEmpty(dr["TURMA"].ToString()))
            {
              if (dr["TURMA"].ToString().Length > 50)
              {
                return "O truma deve possuir no máximo 50 caracteres.";
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

            if (dr["ALUNO"] == null || string.IsNullOrEmpty(dr["ALUNO"].ToString()))
            {
              return "Favor informar o aluno.";
            }

            if (dr["ALUNO"] != null && !string.IsNullOrEmpty(dr["ALUNO"].ToString()))
            {
              if (dr["ALUNO"].ToString().Length > 20)
              {
                return "O aluno deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["OCORRENCIA"] == null || string.IsNullOrEmpty(dr["OCORRENCIA"].ToString()))
            {
              return "Favor informar o ocorrência.";
            }

            if (dr["OCORRENCIA"] != null && !string.IsNullOrEmpty(dr["OCORRENCIA"].ToString()))
            {
              if (dr["OCORRENCIA"].ToString().Length > 15)
              {
                return "O ocorrência deve possuir no máximo 15 caracteres.";
              }
            }

            if (dr["OBSERVACAO"] != null && !string.IsNullOrEmpty(dr["OBSERVACAO"].ToString()))
            {
              if (dr["OBSERVACAO"].ToString().Length > 15)
              {
                return "O observação deve possuir no máximo 15 caracteres.";
              }
            }

            if (dr["STAMP_ATUALIZACAO"] != null && dr["STAMP_ATUALIZACAO"].ToString() != "")
            {
              dr["STAMP_ATUALIZACAO"] = Convert.ToDateTime(dr["STAMP_ATUALIZACAO"]).ToString("dd/MM/yyyy HH:mm:ss");
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

    public static Ly_comp_lista Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_comp_lista Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_comp_lista tab = new Ly_comp_lista();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_comp_lista tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @lista, string @disciplina, string @turma, decimal? @ano, decimal? @semestre, 
        string @aluno, string @ocorrencia, string @observacao, DateTime? @stamp_atualizacao
      ) {
        return (Row)Add(new object[] {
          @lista, @disciplina, @turma, @ano, @semestre, @aluno, @ocorrencia, @observacao, @stamp_atualizacao
        });
      }

      public Row Find(
        decimal? @lista, string @disciplina, string @turma, decimal? @ano, decimal? @semestre, 
        string @aluno
      ) {
        return (Row)Find(new object[] {
          @lista, @disciplina, @turma, @ano, @semestre, @aluno
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
      /// LISTA: NUMERIC(10), Primary Key, Not Null
      /// </summary>
      public decimal? Lista { get { return (decimal?)this["lista"]; } set { this["lista"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DISCIPLINA: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Disciplina { get { return (string)this["disciplina"]; } set { this["disciplina"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TURMA: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Turma { get { return (string)this["turma"]; } set { this["turma"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ANO: NUMERIC(4), Primary Key, Not Null
      /// </summary>
      public decimal? Ano { get { return (decimal?)this["ano"]; } set { this["ano"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SEMESTRE: NUMERIC(2), Primary Key, Not Null
      /// </summary>
      public decimal? Semestre { get { return (decimal?)this["semestre"]; } set { this["semestre"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ALUNO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Aluno { get { return (string)this["aluno"]; } set { this["aluno"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// OCORRENCIA: VARCHAR(15), Not Null
      /// </summary>
      public string Ocorrencia { get { return (string)this["ocorrencia"]; } set { this["ocorrencia"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// OBSERVACAO: VARCHAR(15)
      /// </summary>
      public string Observacao { get { return (string)this["observacao"]; } set { this["observacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// STAMP_ATUALIZACAO: DATETIME
      /// </summary>
      public DateTime? Stamp_atualizacao { get { return (DateTime?)this["stamp_atualizacao"]; } set { this["stamp_atualizacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               decimal? @lista, string @disciplina, string @turma, decimal? @ano, decimal? @semestre, 
                               string @aluno, string @ocorrencia) {
        return Insert(connection,
                      @lista, @disciplina, @turma, @ano, @semestre, @aluno, @ocorrencia,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               decimal? @lista, string @disciplina, string @turma, decimal? @ano, decimal? @semestre, 
                               string @aluno, string @ocorrencia,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_comp_lista(),
          new string[] { "lista", "disciplina", "turma", "ano", "semestre", "aluno", "ocorrencia" },
          new object[] { @lista, @disciplina, @turma, @ano, @semestre, @aluno, @ocorrencia },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkLista, string pkDisciplina, string pkTurma, decimal? pkAno, decimal? pkSemestre, 
                               string pkAluno,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_comp_lista(),
          new string[] { "lista", "disciplina", "turma", "ano", "semestre", "aluno" },
          new object[] { pkLista, pkDisciplina, pkTurma, pkAno, pkSemestre, pkAluno },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkLista, string pkDisciplina, string pkTurma, decimal? pkAno, decimal? pkSemestre, 
                                string pkAluno) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_comp_lista(),
          new string[] { "lista", "disciplina", "turma", "ano", "semestre", "aluno" },
          new object[] { pkLista, pkDisciplina, pkTurma, pkAno, pkSemestre, pkAluno }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              decimal? pkLista, string pkDisciplina, string pkTurma, decimal? pkAno, decimal? pkSemestre, 
                              string pkAluno) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_comp_lista(),
          new string[] { "lista", "disciplina", "turma", "ano", "semestre", "aluno" },
          new object[] { pkLista, pkDisciplina, pkTurma, pkAno, pkSemestre, pkAluno }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public decimal? Lista { get { return (decimal?)row["lista", DataRowVersion.Original]; } }
        public string Disciplina { get { return (string)row["disciplina", DataRowVersion.Original]; } }
        public string Turma { get { return (string)row["turma", DataRowVersion.Original]; } }
        public decimal? Ano { get { return (decimal?)row["ano", DataRowVersion.Original]; } }
        public decimal? Semestre { get { return (decimal?)row["semestre", DataRowVersion.Original]; } }
        public string Aluno { get { return (string)row["aluno", DataRowVersion.Original]; } }
        public string Ocorrencia { get { return (string)row["ocorrencia", DataRowVersion.Original]; } }
        public string Observacao { get { return (string)row["observacao", DataRowVersion.Original]; } }
        public DateTime? Stamp_atualizacao { get { return (DateTime?)row["stamp_atualizacao", DataRowVersion.Original]; } }

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
    #region Coluna: LISTA
    private ColumnLista pvColLista;

    public sealed class ColumnLista : TDataColumn {

      public ColumnLista() {
        this.ColumnName = "lista";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "lista";}
      protected override string GetCaption(){return "Lista";}
      protected override string GetDescricao(){return "Lista de aluno relacionada à discilpina";}
      public override string FullCol { get { return "ly_comp_lista.lista"; } }
      public override string OriginalCol { get { return "lista"; } }
      public override string RefCol { get { return "LY_COMP_LISTA.LISTA"; } }
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

    #region Coluna: DISCIPLINA
    private ColumnDisciplina pvColDisciplina;

    public sealed class ColumnDisciplina : TDataColumn {

      public ColumnDisciplina() {
        this.ColumnName = "disciplina";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "disciplina";}
      protected override string GetCaption(){return "Disciplina";}
      protected override string GetDescricao(){return "Disciplina";}
      public override string FullCol { get { return "ly_comp_lista.disciplina"; } }
      public override string OriginalCol { get { return "disciplina"; } }
      public override string RefCol { get { return "LY_COMP_LISTA.DISCIPLINA"; } }
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

    #region Coluna: TURMA
    private ColumnTurma pvColTurma;

    public sealed class ColumnTurma : TDataColumn {

      public ColumnTurma() {
        this.ColumnName = "turma";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "truma";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "Turma";}
      public override string FullCol { get { return "ly_comp_lista.turma"; } }
      public override string OriginalCol { get { return "turma"; } }
      public override string RefCol { get { return "LY_COMP_LISTA.TURMA"; } }
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

    #region Coluna: ANO
    private ColumnAno pvColAno;

    public sealed class ColumnAno : TDataColumn {

      public ColumnAno() {
        this.ColumnName = "ano";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "ano";}
      protected override string GetCaption(){return "Ano";}
      protected override string GetDescricao(){return "Ano";}
      public override string FullCol { get { return "ly_comp_lista.ano"; } }
      public override string OriginalCol { get { return "ano"; } }
      public override string RefCol { get { return "LY_COMP_LISTA.ANO"; } }
      public override bool PrimaryKey { get { return true; } }
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
      protected override string GetCaption(){return "Semestre";}
      protected override string GetDescricao(){return "Semestre";}
      public override string FullCol { get { return "ly_comp_lista.semestre"; } }
      public override string OriginalCol { get { return "semestre"; } }
      public override string RefCol { get { return "LY_COMP_LISTA.SEMESTRE"; } }
      public override bool PrimaryKey { get { return true; } }
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

    #region Coluna: ALUNO
    private ColumnAluno pvColAluno;

    public sealed class ColumnAluno : TDataColumn {

      public ColumnAluno() {
        this.ColumnName = "aluno";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "aluno";}
      protected override string GetCaption(){return "Aluno";}
      protected override string GetDescricao(){return "Código do aluno";}
      public override string FullCol { get { return "ly_comp_lista.aluno"; } }
      public override string OriginalCol { get { return "aluno"; } }
      public override string RefCol { get { return "LY_COMP_LISTA.ALUNO"; } }
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

    #region Coluna: OCORRENCIA
    private ColumnOcorrencia pvColOcorrencia;

    public sealed class ColumnOcorrencia : TDataColumn {

      public ColumnOcorrencia() {
        this.ColumnName = "ocorrencia";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "ocorrência";}
      protected override string GetCaption(){return "Ocorrência";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_comp_lista.ocorrencia"; } }
      public override string OriginalCol { get { return "ocorrencia"; } }
      public override string RefCol { get { return "LY_COMP_LISTA.OCORRENCIA"; } }
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

    #region Coluna: OBSERVACAO
    private ColumnObservacao pvColObservacao;

    public sealed class ColumnObservacao : TDataColumn {

      public ColumnObservacao() {
        this.ColumnName = "observacao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "observação";}
      protected override string GetCaption(){return "Observação";}
      protected override string GetDescricao(){return "Observações";}
      public override string FullCol { get { return "ly_comp_lista.observacao"; } }
      public override string OriginalCol { get { return "observacao"; } }
      public override string RefCol { get { return "LY_COMP_LISTA.OBSERVACAO"; } }
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

    #region Coluna: STAMP_ATUALIZACAO
    private ColumnStamp_atualizacao pvColStamp_atualizacao;

    public sealed class ColumnStamp_atualizacao : TDataColumn {

      public ColumnStamp_atualizacao() {
        this.ColumnName = "stamp_atualizacao";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_comp_lista.stamp_atualizacao"; } }
      public override string OriginalCol { get { return "stamp_atualizacao"; } }
      public override string RefCol { get { return "LY_COMP_LISTA.STAMP_ATUALIZACAO"; } }
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
}

