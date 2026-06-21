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
  public class Curriculo : TDataSet {
    private const string DataSetName_Def = "Curriculo";
    private static ConstructorInfo ctorTCustom = null;
    private CustomBase tcustom = null;
    #region Variáveis private para as tabelas deste dataset
    private Ly_aula_docente pvTabLy_aula_docente;
    private Ly_aula_docente_tipo pvTabLy_aula_docente_tipo;
    private Ly_curriculo pvTabLy_curriculo;
    private Ly_curriculo_unidade_fisica pvTabLy_curriculo_unidade_fisica;
    private Ly_hor_aula pvTabLy_hor_aula;
    private Ly_hor_oper pvTabLy_hor_oper;
    private Ly_padaces_funcao pvTabLy_padaces_funcao;
    private Ly_padaces_licenca pvTabLy_padaces_licenca;
    private Ly_padaces_turma pvTabLy_padaces_turma;
    private Ly_periodo_cursos pvTabLy_periodo_cursos;
    private Ly_serie pvTabLy_serie;
    private Ly_serie_sufixo pvTabLy_serie_sufixo;
    private Ly_unidade_ensino_cursos pvTabLy_unidade_ensino_cursos;
    private Ly_valor_glp pvTabLy_valor_glp;
    private Ly_verba_glp_ano_mes pvTabLy_verba_glp_ano_mes;
    #endregion

    static Curriculo() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Curriculo");
      }
      catch{}
    }

    public Curriculo() : base() {
      DataSetName = DataSetName_Def;

      pvTabLy_aula_docente = new Ly_aula_docente();
      pvTabLy_aula_docente_tipo = new Ly_aula_docente_tipo();
      pvTabLy_curriculo = new Ly_curriculo();
      pvTabLy_curriculo_unidade_fisica = new Ly_curriculo_unidade_fisica();
      pvTabLy_hor_aula = new Ly_hor_aula();
      pvTabLy_hor_oper = new Ly_hor_oper();
      pvTabLy_padaces_funcao = new Ly_padaces_funcao();
      pvTabLy_padaces_licenca = new Ly_padaces_licenca();
      pvTabLy_padaces_turma = new Ly_padaces_turma();
      pvTabLy_periodo_cursos = new Ly_periodo_cursos();
      pvTabLy_serie = new Ly_serie();
      pvTabLy_serie_sufixo = new Ly_serie_sufixo();
      pvTabLy_unidade_ensino_cursos = new Ly_unidade_ensino_cursos();
      pvTabLy_valor_glp = new Ly_valor_glp();
      pvTabLy_verba_glp_ano_mes = new Ly_verba_glp_ano_mes();

      Tables.Add(pvTabLy_aula_docente);
      Tables.Add(pvTabLy_aula_docente_tipo);
      Tables.Add(pvTabLy_curriculo);
      Tables.Add(pvTabLy_curriculo_unidade_fisica);
      Tables.Add(pvTabLy_hor_aula);
      Tables.Add(pvTabLy_hor_oper);
      Tables.Add(pvTabLy_padaces_funcao);
      Tables.Add(pvTabLy_padaces_licenca);
      Tables.Add(pvTabLy_padaces_turma);
      Tables.Add(pvTabLy_periodo_cursos);
      Tables.Add(pvTabLy_serie);
      Tables.Add(pvTabLy_serie_sufixo);
      Tables.Add(pvTabLy_unidade_ensino_cursos);
      Tables.Add(pvTabLy_valor_glp);
      Tables.Add(pvTabLy_verba_glp_ano_mes);

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
          pvTabLy_aula_docente.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_aula_docente); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_aula_docente_tipo.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_aula_docente_tipo); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_curriculo.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_curriculo); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_curriculo_unidade_fisica.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_curriculo_unidade_fisica); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_hor_aula.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_hor_aula); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_hor_oper.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_hor_oper); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_padaces_funcao.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_padaces_funcao); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_padaces_licenca.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_padaces_licenca); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_padaces_turma.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_padaces_turma); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_periodo_cursos.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_periodo_cursos); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_serie.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_serie); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_serie_sufixo.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_serie_sufixo); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_unidade_ensino_cursos.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_unidade_ensino_cursos); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_valor_glp.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_valor_glp); if(row != null) return DataLib.RowErrorsToString(row);
          pvTabLy_verba_glp_ano_mes.Put(connection); row = DataLib.FirstRowWithError(pvTabLy_verba_glp_ano_mes); if(row != null) return DataLib.RowErrorsToString(row);
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
    public Ly_aula_docente Ly_aula_docente { get { return pvTabLy_aula_docente; } }

    [
      Browsable(false),
    ]
    public Ly_aula_docente_tipo Ly_aula_docente_tipo { get { return pvTabLy_aula_docente_tipo; } }

    [
      Browsable(false),
    ]
    public Ly_curriculo Ly_curriculo { get { return pvTabLy_curriculo; } }

    [
      Browsable(false),
    ]
    public Ly_curriculo_unidade_fisica Ly_curriculo_unidade_fisica { get { return pvTabLy_curriculo_unidade_fisica; } }

    [
      Browsable(false),
    ]
    public Ly_hor_aula Ly_hor_aula { get { return pvTabLy_hor_aula; } }

    [
      Browsable(false),
    ]
    public Ly_hor_oper Ly_hor_oper { get { return pvTabLy_hor_oper; } }

    [
      Browsable(false),
    ]
    public Ly_padaces_funcao Ly_padaces_funcao { get { return pvTabLy_padaces_funcao; } }

    [
      Browsable(false),
    ]
    public Ly_padaces_licenca Ly_padaces_licenca { get { return pvTabLy_padaces_licenca; } }

    [
      Browsable(false),
    ]
    public Ly_padaces_turma Ly_padaces_turma { get { return pvTabLy_padaces_turma; } }

    [
      Browsable(false),
    ]
    public Ly_periodo_cursos Ly_periodo_cursos { get { return pvTabLy_periodo_cursos; } }

    [
      Browsable(false),
    ]
    public Ly_serie Ly_serie { get { return pvTabLy_serie; } }

    [
      Browsable(false),
    ]
    public Ly_serie_sufixo Ly_serie_sufixo { get { return pvTabLy_serie_sufixo; } }

    [
      Browsable(false),
    ]
    public Ly_unidade_ensino_cursos Ly_unidade_ensino_cursos { get { return pvTabLy_unidade_ensino_cursos; } }

    [
      Browsable(false),
    ]
    public Ly_valor_glp Ly_valor_glp { get { return pvTabLy_valor_glp; } }

    [
      Browsable(false),
    ]
    public Ly_verba_glp_ano_mes Ly_verba_glp_ano_mes { get { return pvTabLy_verba_glp_ano_mes; } }

    #endregion

    public abstract class CustomBase {
      public abstract string Put(Techne.Lyceum.CR.Curriculo ds, TConnectionWritable connection);
    }

  }


  public partial class Ly_aula_docente : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_aula_docente() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_aula_docente");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_aula_docente");
      }
      catch{}
    }

    public Ly_aula_docente() {
      pvColNum_func = new ColumnNum_func(); Columns.Add(pvColNum_func);
      pvColTurno = new ColumnTurno(); Columns.Add(pvColTurno);
      pvColFaculdade = new ColumnFaculdade(); Columns.Add(pvColFaculdade);
      pvColDia_semana = new ColumnDia_semana(); Columns.Add(pvColDia_semana);
      pvColAula = new ColumnAula(); Columns.Add(pvColAula);
      pvColDisciplina = new ColumnDisciplina(); Columns.Add(pvColDisciplina);
      pvColTurma = new ColumnTurma(); Columns.Add(pvColTurma);
      pvColAno = new ColumnAno(); Columns.Add(pvColAno);
      pvColSemestre = new ColumnSemestre(); Columns.Add(pvColSemestre);
      pvColTipo = new ColumnTipo(); Columns.Add(pvColTipo);
      pvColData_inicio = new ColumnData_inicio(); Columns.Add(pvColData_inicio);
      pvColData_fim = new ColumnData_fim(); Columns.Add(pvColData_fim);
      pvColTipo_docente = new ColumnTipo_docente(); Columns.Add(pvColTipo_docente);
      pvColTipo_tutoria = new ColumnTipo_tutoria(); Columns.Add(pvColTipo_tutoria);
      pvColStamp_atualizacao = new ColumnStamp_atualizacao(); Columns.Add(pvColStamp_atualizacao);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_aula_docente");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(decimal? @num_func, string @turno, string @faculdade, decimal? @dia_semana, 
                      decimal? @aula, string @disciplina, string @turma, decimal? @ano, decimal? @semestre, 
                      DateTime? @data_inicio) {
      Row row = NewRow();

      row.Num_func = @num_func;
      row.Turno = @turno;
      row.Faculdade = @faculdade;
      row.Dia_semana = @dia_semana;
      row.Aula = @aula;
      row.Disciplina = @disciplina;
      row.Turma = @turma;
      row.Ano = @ano;
      row.Semestre = @semestre;
      row.Data_inicio = @data_inicio;

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
      get { return "LY_AULA_DOCENTE"; }
    }

    protected override sealed string Name { get { return "Aulas do docente"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColNum_func, pvColTurno, pvColFaculdade, pvColDia_semana, 
                                       pvColAula, pvColDisciplina, pvColTurma, pvColAno, pvColSemestre, pvColData_inicio });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_aula_docente_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_aula_docente_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_aula_docente_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_aula_docente_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkNum_func", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "NUM_FUNC", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkTurno", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "TURNO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkFaculdade", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "FACULDADE", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkDia_semana", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "DIA_SEMANA", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkAula", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "AULA", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkDisciplina", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "DISCIPLINA", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkTurma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "TURMA", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkAno", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ANO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkSemestre", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "SEMESTRE", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkData_inicio", OleDbType.Date, 0, ParameterDirection.Input, true, 0, 0, "DATA_INICIO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("num_func", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 25, 10, "NUM_FUNC", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("turno", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TURNO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("faculdade", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "FACULDADE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("dia_semana", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "DIA_SEMANA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("aula", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "AULA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("disciplina", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "DISCIPLINA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("turma", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TURMA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("ano", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ANO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("semestre", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "SEMESTRE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("tipo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TIPO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("data_inicio", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DATA_INICIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("data_fim", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DATA_FIM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("tipo_docente", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TIPO_DOCENTE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("tipo_tutoria", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TIPO_TUTORIA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
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
          if(lower == "num_func") { result.Add(toLower ? "num_func" : column); continue; }
          if(lower == "turno") { result.Add(toLower ? "turno" : column); continue; }
          if(lower == "faculdade") { result.Add(toLower ? "faculdade" : column); continue; }
          if(lower == "dia_semana") { result.Add(toLower ? "dia_semana" : column); continue; }
          if(lower == "aula") { result.Add(toLower ? "aula" : column); continue; }
          if(lower == "disciplina") { result.Add(toLower ? "disciplina" : column); continue; }
          if(lower == "turma") { result.Add(toLower ? "turma" : column); continue; }
          if(lower == "ano") { result.Add(toLower ? "ano" : column); continue; }
          if(lower == "semestre") { result.Add(toLower ? "semestre" : column); continue; }
          if(lower == "tipo") { result.Add(toLower ? "tipo" : column); continue; }
          if(lower == "data_inicio") { result.Add(toLower ? "data_inicio" : column); continue; }
          if(lower == "data_fim") { result.Add(toLower ? "data_fim" : column); continue; }
          if(lower == "tipo_docente") { result.Add(toLower ? "tipo_docente" : column); continue; }
          if(lower == "tipo_tutoria") { result.Add(toLower ? "tipo_tutoria" : column); continue; }
          if(lower == "stamp_atualizacao") { result.Add(toLower ? "stamp_atualizacao" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "num_func", "turno", "faculdade", "dia_semana", "aula", "disciplina", 
                                           "turma", "ano", "semestre", "tipo", "data_inicio", "data_fim", "tipo_docente", "tipo_tutoria", 
                                           "stamp_atualizacao" }
                          : new string[] { "NUM_FUNC", "TURNO", "FACULDADE", "DIA_SEMANA", "AULA", "DISCIPLINA", 
                                           "TURMA", "ANO", "SEMESTRE", "TIPO", "DATA_INICIO", "DATA_FIM", "TIPO_DOCENTE", "TIPO_TUTORIA", 
                                           "STAMP_ATUALIZACAO" });
        else
          result.AddRange(toLower
                          ? new string[] { "num_func", "turno", "faculdade", "dia_semana", "aula", "disciplina", 
                                           "turma", "ano", "semestre", "tipo", "data_inicio", "data_fim", "tipo_docente", "tipo_tutoria", 
                                           "stamp_atualizacao" }
                          : new string[] { "NUM_FUNC", "TURNO", "FACULDADE", "DIA_SEMANA", "AULA", "DISCIPLINA", 
                                           "TURMA", "ANO", "SEMESTRE", "TIPO", "DATA_INICIO", "DATA_FIM", "TIPO_DOCENTE", "TIPO_TUTORIA", 
                                           "STAMP_ATUALIZACAO" });

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
        "ly_aula_docente.num_func", "ly_aula_docente.turno", "ly_aula_docente.faculdade", 
        "ly_aula_docente.dia_semana", "ly_aula_docente.aula", "ly_aula_docente.disciplina", 
        "ly_aula_docente.turma", "ly_aula_docente.ano", "ly_aula_docente.semestre", "ly_aula_docente.data_inicio"
      });
      if(scColumns.Contains("tipo")) list.Add("ly_aula_docente.tipo");
      if(scColumns.Contains("data_fim")) list.Add("ly_aula_docente.data_fim");
      if(scColumns.Contains("tipo_docente")) list.Add("ly_aula_docente.tipo_docente");
      if(scColumns.Contains("tipo_tutoria")) list.Add("ly_aula_docente.tipo_tutoria");
      if(scColumns.Contains("stamp_atualizacao")) list.Add("ly_aula_docente.stamp_atualizacao");
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
          "FROM ly_aula_docente " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_aula_docente");
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
        da.TableMappings.Add("Table", "Ly_aula_docente");
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
            if (dr["NUM_FUNC"] != null && string.IsNullOrEmpty(dr["NUM_FUNC"].ToString()))
              dr["NUM_FUNC"] = DBNull.Value;
            if (dr["TURNO"] != null && string.IsNullOrEmpty(dr["TURNO"].ToString()))
              dr["TURNO"] = DBNull.Value;
            if (dr["FACULDADE"] != null && string.IsNullOrEmpty(dr["FACULDADE"].ToString()))
              dr["FACULDADE"] = DBNull.Value;
            if (dr["DIA_SEMANA"] != null && string.IsNullOrEmpty(dr["DIA_SEMANA"].ToString()))
              dr["DIA_SEMANA"] = DBNull.Value;
            if (dr["AULA"] != null && string.IsNullOrEmpty(dr["AULA"].ToString()))
              dr["AULA"] = DBNull.Value;
            if (dr["DISCIPLINA"] != null && string.IsNullOrEmpty(dr["DISCIPLINA"].ToString()))
              dr["DISCIPLINA"] = DBNull.Value;
            if (dr["TURMA"] != null && string.IsNullOrEmpty(dr["TURMA"].ToString()))
              dr["TURMA"] = DBNull.Value;
            if (dr["ANO"] != null && string.IsNullOrEmpty(dr["ANO"].ToString()))
              dr["ANO"] = DBNull.Value;
            if (dr["SEMESTRE"] != null && string.IsNullOrEmpty(dr["SEMESTRE"].ToString()))
              dr["SEMESTRE"] = DBNull.Value;
            if (dr["TIPO"] != null && string.IsNullOrEmpty(dr["TIPO"].ToString()))
              dr["TIPO"] = DBNull.Value;
            if (dr["TIPO_DOCENTE"] != null && string.IsNullOrEmpty(dr["TIPO_DOCENTE"].ToString()))
              dr["TIPO_DOCENTE"] = DBNull.Value;
            if (dr["TIPO_TUTORIA"] != null && string.IsNullOrEmpty(dr["TIPO_TUTORIA"].ToString()))
              dr["TIPO_TUTORIA"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["NUM_FUNC"] == null || string.IsNullOrEmpty(dr["NUM_FUNC"].ToString()))
            {
              return "Favor informar o cógido do docente.";
            }

            if (dr["NUM_FUNC"] != null && !string.IsNullOrEmpty(dr["NUM_FUNC"].ToString()))
            {
              if ((Convert.ToDecimal(dr["NUM_FUNC"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["NUM_FUNC"]).ToString("0.############").Split(',')[0].Length > 15) || (Convert.ToDecimal(dr["NUM_FUNC"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["NUM_FUNC"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O cógido do docente deve ser um valor inteiro com 15 dígitos no máximo.";
              }
            }

            if (dr["TURNO"] == null || string.IsNullOrEmpty(dr["TURNO"].ToString()))
            {
              return "Favor informar o turno.";
            }

            if (dr["TURNO"] != null && !string.IsNullOrEmpty(dr["TURNO"].ToString()))
            {
              if (dr["TURNO"].ToString().Length > 20)
              {
                return "O turno deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["FACULDADE"] == null || string.IsNullOrEmpty(dr["FACULDADE"].ToString()))
            {
              return "Favor informar a unidade.";
            }

            if (dr["FACULDADE"] != null && !string.IsNullOrEmpty(dr["FACULDADE"].ToString()))
            {
              if (dr["FACULDADE"].ToString().Length > 20)
              {
                return "A unidade deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["DIA_SEMANA"] == null || string.IsNullOrEmpty(dr["DIA_SEMANA"].ToString()))
            {
              return "Favor informar o dia da semana.";
            }

            if (dr["DIA_SEMANA"] != null && !string.IsNullOrEmpty(dr["DIA_SEMANA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["DIA_SEMANA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["DIA_SEMANA"]).ToString("0.############").Split(',')[0].Length > 1) || (Convert.ToDecimal(dr["DIA_SEMANA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["DIA_SEMANA"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O dia da semana deve ser um valor inteiro com 1 dígito no máximo.";
              }

              if (dr["DIA_SEMANA"] != null && !string.IsNullOrEmpty(dr["DIA_SEMANA"].ToString()))
              {
                if (Convert.ToDecimal(dr["DIA_SEMANA"]) < 1)
                {
                  return "O dia da semana deve ser maior ou igual a 1.";
                }
              }

              if (dr["DIA_SEMANA"] != null && !string.IsNullOrEmpty(dr["DIA_SEMANA"].ToString()))
              {
                if (Convert.ToDecimal(dr["DIA_SEMANA"]) > 7)
                {
                  return "O dia da semana deve ser menor ou igual a 7.";
                }
              }
            }

            if (dr["AULA"] == null || string.IsNullOrEmpty(dr["AULA"].ToString()))
            {
              return "Favor informar a aula.";
            }

            if (dr["AULA"] != null && !string.IsNullOrEmpty(dr["AULA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["AULA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["AULA"]).ToString("0.############").Split(',')[0].Length > 5) || (Convert.ToDecimal(dr["AULA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["AULA"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A aula deve ser um valor inteiro com 5 dígitos no máximo.";
              }
            }

            if (dr["DISCIPLINA"] == null || string.IsNullOrEmpty(dr["DISCIPLINA"].ToString()))
            {
              return "Favor informar a disciplina.";
            }

            if (dr["DISCIPLINA"] != null && !string.IsNullOrEmpty(dr["DISCIPLINA"].ToString()))
            {
              if (dr["DISCIPLINA"].ToString().Length > 20)
              {
                return "A disciplina deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["TURMA"] == null || string.IsNullOrEmpty(dr["TURMA"].ToString()))
            {
              return "Favor informar a turma.";
            }

            if (dr["TURMA"] != null && !string.IsNullOrEmpty(dr["TURMA"].ToString()))
            {
              if (dr["TURMA"].ToString().Length > 50)
              {
                return "A turma deve possuir no máximo 50 caracteres.";
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

            if (dr["TIPO"] != null && !string.IsNullOrEmpty(dr["TIPO"].ToString()))
            {
              if (dr["TIPO"].ToString().Length > 20)
              {
                return "O Tipo deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["DATA_INICIO"] == null || string.IsNullOrEmpty(dr["DATA_INICIO"].ToString()))
            {
              return "Favor informar a data de início.";
            }

            if (dr["DATA_INICIO"] != null && dr["DATA_INICIO"].ToString() != "")
            {
              dr["DATA_INICIO"] = Convert.ToDateTime(dr["DATA_INICIO"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DATA_FIM"] != null && dr["DATA_FIM"].ToString() != "")
            {
              dr["DATA_FIM"] = Convert.ToDateTime(dr["DATA_FIM"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["TIPO_DOCENTE"] != null && !string.IsNullOrEmpty(dr["TIPO_DOCENTE"].ToString()))
            {
              if (dr["TIPO_DOCENTE"].ToString().Length > 20)
              {
                return "O Tipo_docente deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["TIPO_TUTORIA"] != null && !string.IsNullOrEmpty(dr["TIPO_TUTORIA"].ToString()))
            {
              if (dr["TIPO_TUTORIA"].ToString().Length > 20)
              {
                return "O Tipo_tutoria deve possuir no máximo 20 caracteres.";
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

    public static Ly_aula_docente Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_aula_docente Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_aula_docente tab = new Ly_aula_docente();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_aula_docente tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @num_func, string @turno, string @faculdade, decimal? @dia_semana, decimal? @aula, 
        string @disciplina, string @turma, decimal? @ano, decimal? @semestre, string @tipo, 
        DateTime? @data_inicio, DateTime? @data_fim, string @tipo_docente, string @tipo_tutoria, 
        DateTime? @stamp_atualizacao
      ) {
        return (Row)Add(new object[] {
          @num_func, @turno, @faculdade, @dia_semana, @aula, @disciplina, @turma, @ano, @semestre, 
          @tipo, @data_inicio, @data_fim, @tipo_docente, @tipo_tutoria, @stamp_atualizacao
        });
      }

      public Row Find(
        decimal? @num_func, string @turno, string @faculdade, decimal? @dia_semana, decimal? @aula, 
        string @disciplina, string @turma, decimal? @ano, decimal? @semestre, DateTime? @data_inicio
      ) {
        return (Row)Find(new object[] {
          @num_func, @turno, @faculdade, @dia_semana, @aula, @disciplina, @turma, @ano, @semestre, 
          @data_inicio
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
      /// NUM_FUNC: NUMERIC(15), Primary Key, Not Null
      /// </summary>
      public decimal? Num_func { get { return (decimal?)this["num_func"]; } set { this["num_func"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TURNO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Turno { get { return (string)this["turno"]; } set { this["turno"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// FACULDADE: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Faculdade { get { return (string)this["faculdade"]; } set { this["faculdade"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DIA_SEMANA: NUMERIC(1), Primary Key, Not Null
      /// </summary>
      public decimal? Dia_semana { get { return (decimal?)this["dia_semana"]; } set { this["dia_semana"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// AULA: NUMERIC(5), Primary Key, Not Null
      /// </summary>
      public decimal? Aula { get { return (decimal?)this["aula"]; } set { this["aula"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
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
      /// TIPO: VARCHAR(20)
      /// </summary>
      public string Tipo { get { return (string)this["tipo"]; } set { this["tipo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DATA_INICIO: DATETIME, Primary Key, Not Null
      /// </summary>
      public DateTime? Data_inicio { get { return (DateTime?)this["data_inicio"]; } set { this["data_inicio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DATA_FIM: DATETIME
      /// </summary>
      public DateTime? Data_fim { get { return (DateTime?)this["data_fim"]; } set { this["data_fim"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TIPO_DOCENTE: VARCHAR(20)
      /// </summary>
      public string Tipo_docente { get { return (string)this["tipo_docente"]; } set { this["tipo_docente"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TIPO_TUTORIA: VARCHAR(20)
      /// </summary>
      public string Tipo_tutoria { get { return (string)this["tipo_tutoria"]; } set { this["tipo_tutoria"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// STAMP_ATUALIZACAO: DATETIME
      /// </summary>
      public DateTime? Stamp_atualizacao { get { return (DateTime?)this["stamp_atualizacao"]; } set { this["stamp_atualizacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               decimal? @num_func, string @turno, string @faculdade, decimal? @dia_semana, decimal? @aula, 
                               string @disciplina, string @turma, decimal? @ano, decimal? @semestre, DateTime? @data_inicio) {
        return Insert(connection,
                      @num_func, @turno, @faculdade, @dia_semana, @aula, @disciplina, @turma, @ano, @semestre, 
                      @data_inicio,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               decimal? @num_func, string @turno, string @faculdade, decimal? @dia_semana, decimal? @aula, 
                               string @disciplina, string @turma, decimal? @ano, decimal? @semestre, DateTime? @data_inicio,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_aula_docente(),
          new string[] { "num_func", "turno", "faculdade", "dia_semana", "aula", "disciplina", 
                         "turma", "ano", "semestre", "data_inicio" },
          new object[] { @num_func, @turno, @faculdade, @dia_semana, @aula, @disciplina, @turma, 
                         @ano, @semestre, @data_inicio },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkNum_func, string pkTurno, string pkFaculdade, decimal? pkDia_semana, decimal? pkAula, 
                               string pkDisciplina, string pkTurma, decimal? pkAno, decimal? pkSemestre, DateTime? pkData_inicio,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_aula_docente(),
          new string[] { "num_func", "turno", "faculdade", "dia_semana", "aula", "disciplina", 
                         "turma", "ano", "semestre", "data_inicio" },
          new object[] { pkNum_func, pkTurno, pkFaculdade, pkDia_semana, pkAula, pkDisciplina, 
                         pkTurma, pkAno, pkSemestre, pkData_inicio },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkNum_func, string pkTurno, string pkFaculdade, decimal? pkDia_semana, decimal? pkAula, 
                                string pkDisciplina, string pkTurma, decimal? pkAno, decimal? pkSemestre, DateTime? pkData_inicio) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_aula_docente(),
          new string[] { "num_func", "turno", "faculdade", "dia_semana", "aula", "disciplina", 
                         "turma", "ano", "semestre", "data_inicio" },
          new object[] { pkNum_func, pkTurno, pkFaculdade, pkDia_semana, pkAula, pkDisciplina, 
                         pkTurma, pkAno, pkSemestre, pkData_inicio }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              decimal? pkNum_func, string pkTurno, string pkFaculdade, decimal? pkDia_semana, decimal? pkAula, 
                              string pkDisciplina, string pkTurma, decimal? pkAno, decimal? pkSemestre, DateTime? pkData_inicio) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_aula_docente(),
          new string[] { "num_func", "turno", "faculdade", "dia_semana", "aula", "disciplina", 
                         "turma", "ano", "semestre", "data_inicio" },
          new object[] { pkNum_func, pkTurno, pkFaculdade, pkDia_semana, pkAula, pkDisciplina, 
                         pkTurma, pkAno, pkSemestre, pkData_inicio }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public decimal? Num_func { get { return (decimal?)row["num_func", DataRowVersion.Original]; } }
        public string Turno { get { return (string)row["turno", DataRowVersion.Original]; } }
        public string Faculdade { get { return (string)row["faculdade", DataRowVersion.Original]; } }
        public decimal? Dia_semana { get { return (decimal?)row["dia_semana", DataRowVersion.Original]; } }
        public decimal? Aula { get { return (decimal?)row["aula", DataRowVersion.Original]; } }
        public string Disciplina { get { return (string)row["disciplina", DataRowVersion.Original]; } }
        public string Turma { get { return (string)row["turma", DataRowVersion.Original]; } }
        public decimal? Ano { get { return (decimal?)row["ano", DataRowVersion.Original]; } }
        public decimal? Semestre { get { return (decimal?)row["semestre", DataRowVersion.Original]; } }
        public string Tipo { get { return (string)row["tipo", DataRowVersion.Original]; } }
        public DateTime? Data_inicio { get { return (DateTime?)row["data_inicio", DataRowVersion.Original]; } }
        public DateTime? Data_fim { get { return (DateTime?)row["data_fim", DataRowVersion.Original]; } }
        public string Tipo_docente { get { return (string)row["tipo_docente", DataRowVersion.Original]; } }
        public string Tipo_tutoria { get { return (string)row["tipo_tutoria", DataRowVersion.Original]; } }
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
    #region Coluna: NUM_FUNC
    private ColumnNum_func pvColNum_func;

    public sealed class ColumnNum_func : TDataColumn {

      public ColumnNum_func() {
        this.ColumnName = "num_func";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "cógido do docente";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente.num_func"; } }
      public override string OriginalCol { get { return "num_func"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE.NUM_FUNC"; } }
      public override bool PrimaryKey { get { return true; } }
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

    #region Coluna: TURNO
    private ColumnTurno pvColTurno;

    public sealed class ColumnTurno : TDataColumn {

      public ColumnTurno() {
        this.ColumnName = "turno";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "turno";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente.turno"; } }
      public override string OriginalCol { get { return "turno"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE.TURNO"; } }
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

    #region Coluna: FACULDADE
    private ColumnFaculdade pvColFaculdade;

    public sealed class ColumnFaculdade : TDataColumn {

      public ColumnFaculdade() {
        this.ColumnName = "faculdade";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "unidade";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente.faculdade"; } }
      public override string OriginalCol { get { return "faculdade"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE.FACULDADE"; } }
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

    #region Coluna: DIA_SEMANA
    private ColumnDia_semana pvColDia_semana;

    public sealed class ColumnDia_semana : TDataColumn {

      public ColumnDia_semana() {
        this.ColumnName = "dia_semana";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "dia da semana";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente.dia_semana"; } }
      public override string OriginalCol { get { return "dia_semana"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE.DIA_SEMANA"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 2; } }
      public override int Precision { get { return 1; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(1)"; } }

      public override object MinValue { get { return 1; } }

      public override object MaxValue { get { return 7; } }

    }
    #endregion

    #region Coluna: AULA
    private ColumnAula pvColAula;

    public sealed class ColumnAula : TDataColumn {

      public ColumnAula() {
        this.ColumnName = "aula";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "aula";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente.aula"; } }
      public override string OriginalCol { get { return "aula"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE.AULA"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 4; } }
      public override int Precision { get { return 5; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(5)"; } }

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
      protected override string GetCaption(){return "Código do Tipo ";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente.disciplina"; } }
      public override string OriginalCol { get { return "disciplina"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE.DISCIPLINA"; } }
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


      protected override string GetName(){return "turma";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente.turma"; } }
      public override string OriginalCol { get { return "turma"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE.TURMA"; } }
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
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente.ano"; } }
      public override string OriginalCol { get { return "ano"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE.ANO"; } }
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
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente.semestre"; } }
      public override string OriginalCol { get { return "semestre"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE.SEMESTRE"; } }
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
      public override string FullCol { get { return "ly_aula_docente.tipo"; } }
      public override string OriginalCol { get { return "tipo"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE.TIPO"; } }
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

    #region Coluna: DATA_INICIO
    private ColumnData_inicio pvColData_inicio;

    public sealed class ColumnData_inicio : TDataColumn {

      public ColumnData_inicio() {
        this.ColumnName = "data_inicio";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data de início";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente.data_inicio"; } }
      public override string OriginalCol { get { return "data_inicio"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE.DATA_INICIO"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 10; } }
      public override string SqlServerType { get { return "DATETIME"; } }

    }
    #endregion

    #region Coluna: DATA_FIM
    private ColumnData_fim pvColData_fim;

    public sealed class ColumnData_fim : TDataColumn {

      public ColumnData_fim() {
        this.ColumnName = "data_fim";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data de fim";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente.data_fim"; } }
      public override string OriginalCol { get { return "data_fim"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE.DATA_FIM"; } }
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

    #region Coluna: TIPO_DOCENTE
    private ColumnTipo_docente pvColTipo_docente;

    public sealed class ColumnTipo_docente : TDataColumn {

      public ColumnTipo_docente() {
        this.ColumnName = "tipo_docente";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente.tipo_docente"; } }
      public override string OriginalCol { get { return "tipo_docente"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE.TIPO_DOCENTE"; } }
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

    #region Coluna: TIPO_TUTORIA
    private ColumnTipo_tutoria pvColTipo_tutoria;

    public sealed class ColumnTipo_tutoria : TDataColumn {

      public ColumnTipo_tutoria() {
        this.ColumnName = "tipo_tutoria";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente.tipo_tutoria"; } }
      public override string OriginalCol { get { return "tipo_tutoria"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE.TIPO_TUTORIA"; } }
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
      public override string FullCol { get { return "ly_aula_docente.stamp_atualizacao"; } }
      public override string OriginalCol { get { return "stamp_atualizacao"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE.STAMP_ATUALIZACAO"; } }
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

  public partial class Ly_aula_docente_tipo : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_aula_docente_tipo() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_aula_docente_tipo");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_aula_docente_tipo");
      }
      catch{}
    }

    public Ly_aula_docente_tipo() {
      pvColNum_func = new ColumnNum_func(); Columns.Add(pvColNum_func);
      pvColTurno = new ColumnTurno(); Columns.Add(pvColTurno);
      pvColFaculdade = new ColumnFaculdade(); Columns.Add(pvColFaculdade);
      pvColDia_semana = new ColumnDia_semana(); Columns.Add(pvColDia_semana);
      pvColAula = new ColumnAula(); Columns.Add(pvColAula);
      pvColDisciplina = new ColumnDisciplina(); Columns.Add(pvColDisciplina);
      pvColTurma = new ColumnTurma(); Columns.Add(pvColTurma);
      pvColAno = new ColumnAno(); Columns.Add(pvColAno);
      pvColSemestre = new ColumnSemestre(); Columns.Add(pvColSemestre);
      pvColData_inicio = new ColumnData_inicio(); Columns.Add(pvColData_inicio);
      pvColData_fim = new ColumnData_fim(); Columns.Add(pvColData_fim);
      pvColTipo_aula = new ColumnTipo_aula(); Columns.Add(pvColTipo_aula);
      pvColStamp_atualizacao = new ColumnStamp_atualizacao(); Columns.Add(pvColStamp_atualizacao);
      pvColId_docente_funcao_glp = new ColumnId_docente_funcao_glp(); Columns.Add(pvColId_docente_funcao_glp);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_aula_docente_tipo");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(decimal? @num_func, string @turno, string @faculdade, decimal? @dia_semana, 
                      decimal? @aula, string @disciplina, string @turma, decimal? @ano, decimal? @semestre, 
                      DateTime? @data_inicio) {
      Row row = NewRow();

      row.Num_func = @num_func;
      row.Turno = @turno;
      row.Faculdade = @faculdade;
      row.Dia_semana = @dia_semana;
      row.Aula = @aula;
      row.Disciplina = @disciplina;
      row.Turma = @turma;
      row.Ano = @ano;
      row.Semestre = @semestre;
      row.Data_inicio = @data_inicio;

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
      get { return "LY_AULA_DOCENTE_TIPO"; }
    }

    protected override sealed string Name { get { return "Tipos de aulas do docente"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColNum_func, pvColTurno, pvColFaculdade, pvColDia_semana, 
                                       pvColAula, pvColDisciplina, pvColTurma, pvColAno, pvColSemestre, pvColData_inicio });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_aula_docente_tipo_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_aula_docente_tipo_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_aula_docente_tipo_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_aula_docente_tipo_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkNum_func", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "NUM_FUNC", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkTurno", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "TURNO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkFaculdade", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "FACULDADE", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkDia_semana", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "DIA_SEMANA", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkAula", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "AULA", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkDisciplina", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "DISCIPLINA", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkTurma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "TURMA", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkAno", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ANO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkSemestre", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "SEMESTRE", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkData_inicio", OleDbType.Date, 0, ParameterDirection.Input, true, 0, 0, "DATA_INICIO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("num_func", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 25, 10, "NUM_FUNC", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("turno", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TURNO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("faculdade", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "FACULDADE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("dia_semana", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "DIA_SEMANA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("aula", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "AULA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("disciplina", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "DISCIPLINA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("turma", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TURMA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("ano", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ANO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("semestre", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "SEMESTRE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("data_inicio", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DATA_INICIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("data_fim", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DATA_FIM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("tipo_aula", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TIPO_AULA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("stamp_atualizacao", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "STAMP_ATUALIZACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("id_docente_funcao_glp", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ID_DOCENTE_FUNCAO_GLP", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "num_func") { result.Add(toLower ? "num_func" : column); continue; }
          if(lower == "turno") { result.Add(toLower ? "turno" : column); continue; }
          if(lower == "faculdade") { result.Add(toLower ? "faculdade" : column); continue; }
          if(lower == "dia_semana") { result.Add(toLower ? "dia_semana" : column); continue; }
          if(lower == "aula") { result.Add(toLower ? "aula" : column); continue; }
          if(lower == "disciplina") { result.Add(toLower ? "disciplina" : column); continue; }
          if(lower == "turma") { result.Add(toLower ? "turma" : column); continue; }
          if(lower == "ano") { result.Add(toLower ? "ano" : column); continue; }
          if(lower == "semestre") { result.Add(toLower ? "semestre" : column); continue; }
          if(lower == "data_inicio") { result.Add(toLower ? "data_inicio" : column); continue; }
          if(lower == "data_fim") { result.Add(toLower ? "data_fim" : column); continue; }
          if(lower == "tipo_aula") { result.Add(toLower ? "tipo_aula" : column); continue; }
          if(lower == "stamp_atualizacao") { result.Add(toLower ? "stamp_atualizacao" : column); continue; }
          if(lower == "id_docente_funcao_glp") { result.Add(toLower ? "id_docente_funcao_glp" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "num_func", "turno", "faculdade", "dia_semana", "aula", "disciplina", 
                                           "turma", "ano", "semestre", "data_inicio", "data_fim", "tipo_aula", "stamp_atualizacao", 
                                           "id_docente_funcao_glp" }
                          : new string[] { "NUM_FUNC", "TURNO", "FACULDADE", "DIA_SEMANA", "AULA", "DISCIPLINA", 
                                           "TURMA", "ANO", "SEMESTRE", "DATA_INICIO", "DATA_FIM", "TIPO_AULA", "STAMP_ATUALIZACAO", 
                                           "ID_DOCENTE_FUNCAO_GLP" });
        else
          result.AddRange(toLower
                          ? new string[] { "num_func", "turno", "faculdade", "dia_semana", "aula", "disciplina", 
                                           "turma", "ano", "semestre", "data_inicio", "data_fim", "tipo_aula", "stamp_atualizacao", 
                                           "id_docente_funcao_glp" }
                          : new string[] { "NUM_FUNC", "TURNO", "FACULDADE", "DIA_SEMANA", "AULA", "DISCIPLINA", 
                                           "TURMA", "ANO", "SEMESTRE", "DATA_INICIO", "DATA_FIM", "TIPO_AULA", "STAMP_ATUALIZACAO", 
                                           "ID_DOCENTE_FUNCAO_GLP" });

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
        "ly_aula_docente_tipo.num_func", "ly_aula_docente_tipo.turno", "ly_aula_docente_tipo.faculdade", 
        "ly_aula_docente_tipo.dia_semana", "ly_aula_docente_tipo.aula", "ly_aula_docente_tipo.disciplina", 
        "ly_aula_docente_tipo.turma", "ly_aula_docente_tipo.ano", "ly_aula_docente_tipo.semestre", 
        "ly_aula_docente_tipo.data_inicio"
      });
      if(scColumns.Contains("data_fim")) list.Add("ly_aula_docente_tipo.data_fim");
      if(scColumns.Contains("tipo_aula")) list.Add("ly_aula_docente_tipo.tipo_aula");
      if(scColumns.Contains("stamp_atualizacao")) list.Add("ly_aula_docente_tipo.stamp_atualizacao");
      if(scColumns.Contains("id_docente_funcao_glp")) list.Add("ly_aula_docente_tipo.id_docente_funcao_glp");
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
          "FROM ly_aula_docente_tipo " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_aula_docente_tipo");
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
        da.TableMappings.Add("Table", "Ly_aula_docente_tipo");
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
            if (dr["NUM_FUNC"] != null && string.IsNullOrEmpty(dr["NUM_FUNC"].ToString()))
              dr["NUM_FUNC"] = DBNull.Value;
            if (dr["TURNO"] != null && string.IsNullOrEmpty(dr["TURNO"].ToString()))
              dr["TURNO"] = DBNull.Value;
            if (dr["FACULDADE"] != null && string.IsNullOrEmpty(dr["FACULDADE"].ToString()))
              dr["FACULDADE"] = DBNull.Value;
            if (dr["DIA_SEMANA"] != null && string.IsNullOrEmpty(dr["DIA_SEMANA"].ToString()))
              dr["DIA_SEMANA"] = DBNull.Value;
            if (dr["AULA"] != null && string.IsNullOrEmpty(dr["AULA"].ToString()))
              dr["AULA"] = DBNull.Value;
            if (dr["DISCIPLINA"] != null && string.IsNullOrEmpty(dr["DISCIPLINA"].ToString()))
              dr["DISCIPLINA"] = DBNull.Value;
            if (dr["TURMA"] != null && string.IsNullOrEmpty(dr["TURMA"].ToString()))
              dr["TURMA"] = DBNull.Value;
            if (dr["ANO"] != null && string.IsNullOrEmpty(dr["ANO"].ToString()))
              dr["ANO"] = DBNull.Value;
            if (dr["SEMESTRE"] != null && string.IsNullOrEmpty(dr["SEMESTRE"].ToString()))
              dr["SEMESTRE"] = DBNull.Value;
            if (dr["TIPO_AULA"] != null && string.IsNullOrEmpty(dr["TIPO_AULA"].ToString()))
              dr["TIPO_AULA"] = DBNull.Value;
            if (dr["ID_DOCENTE_FUNCAO_GLP"] != null && string.IsNullOrEmpty(dr["ID_DOCENTE_FUNCAO_GLP"].ToString()))
              dr["ID_DOCENTE_FUNCAO_GLP"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["NUM_FUNC"] == null || string.IsNullOrEmpty(dr["NUM_FUNC"].ToString()))
            {
              return "Favor informar o docente.";
            }

            if (dr["NUM_FUNC"] != null && !string.IsNullOrEmpty(dr["NUM_FUNC"].ToString()))
            {
              if ((Convert.ToDecimal(dr["NUM_FUNC"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["NUM_FUNC"]).ToString("0.############").Split(',')[0].Length > 15) || (Convert.ToDecimal(dr["NUM_FUNC"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["NUM_FUNC"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O docente deve ser um valor inteiro com 15 dígitos no máximo.";
              }
            }

            if (dr["TURNO"] == null || string.IsNullOrEmpty(dr["TURNO"].ToString()))
            {
              return "Favor informar o turno.";
            }

            if (dr["TURNO"] != null && !string.IsNullOrEmpty(dr["TURNO"].ToString()))
            {
              if (dr["TURNO"].ToString().Length > 20)
              {
                return "O turno deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["FACULDADE"] == null || string.IsNullOrEmpty(dr["FACULDADE"].ToString()))
            {
              return "Favor informar a unidade física.";
            }

            if (dr["FACULDADE"] != null && !string.IsNullOrEmpty(dr["FACULDADE"].ToString()))
            {
              if (dr["FACULDADE"].ToString().Length > 20)
              {
                return "A unidade física deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["DIA_SEMANA"] == null || string.IsNullOrEmpty(dr["DIA_SEMANA"].ToString()))
            {
              return "Favor informar o dia da semana.";
            }

            if (dr["DIA_SEMANA"] != null && !string.IsNullOrEmpty(dr["DIA_SEMANA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["DIA_SEMANA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["DIA_SEMANA"]).ToString("0.############").Split(',')[0].Length > 1) || (Convert.ToDecimal(dr["DIA_SEMANA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["DIA_SEMANA"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O dia da semana deve ser um valor inteiro com 1 dígito no máximo.";
              }

              if (dr["DIA_SEMANA"] != null && !string.IsNullOrEmpty(dr["DIA_SEMANA"].ToString()))
              {
                if (Convert.ToDecimal(dr["DIA_SEMANA"]) < 1)
                {
                  return "O dia da semana deve ser maior ou igual a 1.";
                }
              }

              if (dr["DIA_SEMANA"] != null && !string.IsNullOrEmpty(dr["DIA_SEMANA"].ToString()))
              {
                if (Convert.ToDecimal(dr["DIA_SEMANA"]) > 7)
                {
                  return "O dia da semana deve ser menor ou igual a 7.";
                }
              }
            }

            if (dr["AULA"] == null || string.IsNullOrEmpty(dr["AULA"].ToString()))
            {
              return "Favor informar a aula.";
            }

            if (dr["AULA"] != null && !string.IsNullOrEmpty(dr["AULA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["AULA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["AULA"]).ToString("0.############").Split(',')[0].Length > 5) || (Convert.ToDecimal(dr["AULA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["AULA"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A aula deve ser um valor inteiro com 5 dígitos no máximo.";
              }
            }

            if (dr["DISCIPLINA"] == null || string.IsNullOrEmpty(dr["DISCIPLINA"].ToString()))
            {
              return "Favor informar a disciplina.";
            }

            if (dr["DISCIPLINA"] != null && !string.IsNullOrEmpty(dr["DISCIPLINA"].ToString()))
            {
              if (dr["DISCIPLINA"].ToString().Length > 20)
              {
                return "A disciplina deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["TURMA"] == null || string.IsNullOrEmpty(dr["TURMA"].ToString()))
            {
              return "Favor informar a turma.";
            }

            if (dr["TURMA"] != null && !string.IsNullOrEmpty(dr["TURMA"].ToString()))
            {
              if (dr["TURMA"].ToString().Length > 50)
              {
                return "A turma deve possuir no máximo 50 caracteres.";
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

            if (dr["DATA_INICIO"] == null || string.IsNullOrEmpty(dr["DATA_INICIO"].ToString()))
            {
              return "Favor informar a data início.";
            }

            if (dr["DATA_INICIO"] != null && dr["DATA_INICIO"].ToString() != "")
            {
              dr["DATA_INICIO"] = Convert.ToDateTime(dr["DATA_INICIO"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DATA_FIM"] != null && dr["DATA_FIM"].ToString() != "")
            {
              dr["DATA_FIM"] = Convert.ToDateTime(dr["DATA_FIM"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["TIPO_AULA"] != null && !string.IsNullOrEmpty(dr["TIPO_AULA"].ToString()))
            {
              if (dr["TIPO_AULA"].ToString().Length > 20)
              {
                return "O tipo de aula deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["STAMP_ATUALIZACAO"] != null && dr["STAMP_ATUALIZACAO"].ToString() != "")
            {
              dr["STAMP_ATUALIZACAO"] = Convert.ToDateTime(dr["STAMP_ATUALIZACAO"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["ID_DOCENTE_FUNCAO_GLP"] != null && !string.IsNullOrEmpty(dr["ID_DOCENTE_FUNCAO_GLP"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ID_DOCENTE_FUNCAO_GLP"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_DOCENTE_FUNCAO_GLP"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["ID_DOCENTE_FUNCAO_GLP"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ID_DOCENTE_FUNCAO_GLP"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O id do pedido da glp deve ser um valor inteiro com 10 dígitos no máximo.";
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

    public static Ly_aula_docente_tipo Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_aula_docente_tipo Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_aula_docente_tipo tab = new Ly_aula_docente_tipo();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_aula_docente_tipo tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @num_func, string @turno, string @faculdade, decimal? @dia_semana, decimal? @aula, 
        string @disciplina, string @turma, decimal? @ano, decimal? @semestre, DateTime? @data_inicio, 
        DateTime? @data_fim, string @tipo_aula, DateTime? @stamp_atualizacao, decimal? @id_docente_funcao_glp
      ) {
        return (Row)Add(new object[] {
          @num_func, @turno, @faculdade, @dia_semana, @aula, @disciplina, @turma, @ano, @semestre, 
          @data_inicio, @data_fim, @tipo_aula, @stamp_atualizacao, @id_docente_funcao_glp
        });
      }

      public Row Find(
        decimal? @num_func, string @turno, string @faculdade, decimal? @dia_semana, decimal? @aula, 
        string @disciplina, string @turma, decimal? @ano, decimal? @semestre, DateTime? @data_inicio
      ) {
        return (Row)Find(new object[] {
          @num_func, @turno, @faculdade, @dia_semana, @aula, @disciplina, @turma, @ano, @semestre, 
          @data_inicio
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
      /// NUM_FUNC: NUMERIC(15), Primary Key, Not Null
      /// </summary>
      public decimal? Num_func { get { return (decimal?)this["num_func"]; } set { this["num_func"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TURNO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Turno { get { return (string)this["turno"]; } set { this["turno"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// FACULDADE: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Faculdade { get { return (string)this["faculdade"]; } set { this["faculdade"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DIA_SEMANA: NUMERIC(1), Primary Key, Not Null
      /// </summary>
      public decimal? Dia_semana { get { return (decimal?)this["dia_semana"]; } set { this["dia_semana"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// AULA: NUMERIC(5), Primary Key, Not Null
      /// </summary>
      public decimal? Aula { get { return (decimal?)this["aula"]; } set { this["aula"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
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
      /// DATA_INICIO: DATETIME, Primary Key, Not Null
      /// </summary>
      public DateTime? Data_inicio { get { return (DateTime?)this["data_inicio"]; } set { this["data_inicio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DATA_FIM: DATETIME
      /// </summary>
      public DateTime? Data_fim { get { return (DateTime?)this["data_fim"]; } set { this["data_fim"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TIPO_AULA: VARCHAR(20)
      /// </summary>
      public string Tipo_aula { get { return (string)this["tipo_aula"]; } set { this["tipo_aula"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// STAMP_ATUALIZACAO: DATETIME
      /// </summary>
      public DateTime? Stamp_atualizacao { get { return (DateTime?)this["stamp_atualizacao"]; } set { this["stamp_atualizacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ID_DOCENTE_FUNCAO_GLP: NUMERIC(10)
      /// </summary>
      public decimal? Id_docente_funcao_glp { get { return (decimal?)this["id_docente_funcao_glp"]; } set { this["id_docente_funcao_glp"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               decimal? @num_func, string @turno, string @faculdade, decimal? @dia_semana, decimal? @aula, 
                               string @disciplina, string @turma, decimal? @ano, decimal? @semestre, DateTime? @data_inicio) {
        return Insert(connection,
                      @num_func, @turno, @faculdade, @dia_semana, @aula, @disciplina, @turma, @ano, @semestre, 
                      @data_inicio,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               decimal? @num_func, string @turno, string @faculdade, decimal? @dia_semana, decimal? @aula, 
                               string @disciplina, string @turma, decimal? @ano, decimal? @semestre, DateTime? @data_inicio,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_aula_docente_tipo(),
          new string[] { "num_func", "turno", "faculdade", "dia_semana", "aula", "disciplina", 
                         "turma", "ano", "semestre", "data_inicio" },
          new object[] { @num_func, @turno, @faculdade, @dia_semana, @aula, @disciplina, @turma, 
                         @ano, @semestre, @data_inicio },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkNum_func, string pkTurno, string pkFaculdade, decimal? pkDia_semana, decimal? pkAula, 
                               string pkDisciplina, string pkTurma, decimal? pkAno, decimal? pkSemestre, DateTime? pkData_inicio,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_aula_docente_tipo(),
          new string[] { "num_func", "turno", "faculdade", "dia_semana", "aula", "disciplina", 
                         "turma", "ano", "semestre", "data_inicio" },
          new object[] { pkNum_func, pkTurno, pkFaculdade, pkDia_semana, pkAula, pkDisciplina, 
                         pkTurma, pkAno, pkSemestre, pkData_inicio },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkNum_func, string pkTurno, string pkFaculdade, decimal? pkDia_semana, decimal? pkAula, 
                                string pkDisciplina, string pkTurma, decimal? pkAno, decimal? pkSemestre, DateTime? pkData_inicio) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_aula_docente_tipo(),
          new string[] { "num_func", "turno", "faculdade", "dia_semana", "aula", "disciplina", 
                         "turma", "ano", "semestre", "data_inicio" },
          new object[] { pkNum_func, pkTurno, pkFaculdade, pkDia_semana, pkAula, pkDisciplina, 
                         pkTurma, pkAno, pkSemestre, pkData_inicio }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              decimal? pkNum_func, string pkTurno, string pkFaculdade, decimal? pkDia_semana, decimal? pkAula, 
                              string pkDisciplina, string pkTurma, decimal? pkAno, decimal? pkSemestre, DateTime? pkData_inicio) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_aula_docente_tipo(),
          new string[] { "num_func", "turno", "faculdade", "dia_semana", "aula", "disciplina", 
                         "turma", "ano", "semestre", "data_inicio" },
          new object[] { pkNum_func, pkTurno, pkFaculdade, pkDia_semana, pkAula, pkDisciplina, 
                         pkTurma, pkAno, pkSemestre, pkData_inicio }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public decimal? Num_func { get { return (decimal?)row["num_func", DataRowVersion.Original]; } }
        public string Turno { get { return (string)row["turno", DataRowVersion.Original]; } }
        public string Faculdade { get { return (string)row["faculdade", DataRowVersion.Original]; } }
        public decimal? Dia_semana { get { return (decimal?)row["dia_semana", DataRowVersion.Original]; } }
        public decimal? Aula { get { return (decimal?)row["aula", DataRowVersion.Original]; } }
        public string Disciplina { get { return (string)row["disciplina", DataRowVersion.Original]; } }
        public string Turma { get { return (string)row["turma", DataRowVersion.Original]; } }
        public decimal? Ano { get { return (decimal?)row["ano", DataRowVersion.Original]; } }
        public decimal? Semestre { get { return (decimal?)row["semestre", DataRowVersion.Original]; } }
        public DateTime? Data_inicio { get { return (DateTime?)row["data_inicio", DataRowVersion.Original]; } }
        public DateTime? Data_fim { get { return (DateTime?)row["data_fim", DataRowVersion.Original]; } }
        public string Tipo_aula { get { return (string)row["tipo_aula", DataRowVersion.Original]; } }
        public DateTime? Stamp_atualizacao { get { return (DateTime?)row["stamp_atualizacao", DataRowVersion.Original]; } }
        public decimal? Id_docente_funcao_glp { get { return (decimal?)row["id_docente_funcao_glp", DataRowVersion.Original]; } }

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
    #region Coluna: NUM_FUNC
    private ColumnNum_func pvColNum_func;

    public sealed class ColumnNum_func : TDataColumn {

      public ColumnNum_func() {
        this.ColumnName = "num_func";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "docente";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente_tipo.num_func"; } }
      public override string OriginalCol { get { return "num_func"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE_TIPO.NUM_FUNC"; } }
      public override bool PrimaryKey { get { return true; } }
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

    #region Coluna: TURNO
    private ColumnTurno pvColTurno;

    public sealed class ColumnTurno : TDataColumn {

      public ColumnTurno() {
        this.ColumnName = "turno";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "turno";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente_tipo.turno"; } }
      public override string OriginalCol { get { return "turno"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE_TIPO.TURNO"; } }
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

    #region Coluna: FACULDADE
    private ColumnFaculdade pvColFaculdade;

    public sealed class ColumnFaculdade : TDataColumn {

      public ColumnFaculdade() {
        this.ColumnName = "faculdade";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "unidade física";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente_tipo.faculdade"; } }
      public override string OriginalCol { get { return "faculdade"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE_TIPO.FACULDADE"; } }
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

    #region Coluna: DIA_SEMANA
    private ColumnDia_semana pvColDia_semana;

    public sealed class ColumnDia_semana : TDataColumn {

      public ColumnDia_semana() {
        this.ColumnName = "dia_semana";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "dia da semana";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente_tipo.dia_semana"; } }
      public override string OriginalCol { get { return "dia_semana"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE_TIPO.DIA_SEMANA"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 2; } }
      public override int Precision { get { return 1; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(1)"; } }

      public override object MinValue { get { return 1; } }

      public override object MaxValue { get { return 7; } }

    }
    #endregion

    #region Coluna: AULA
    private ColumnAula pvColAula;

    public sealed class ColumnAula : TDataColumn {

      public ColumnAula() {
        this.ColumnName = "aula";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "aula";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente_tipo.aula"; } }
      public override string OriginalCol { get { return "aula"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE_TIPO.AULA"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 4; } }
      public override int Precision { get { return 5; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(5)"; } }

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
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente_tipo.disciplina"; } }
      public override string OriginalCol { get { return "disciplina"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE_TIPO.DISCIPLINA"; } }
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


      protected override string GetName(){return "turma";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente_tipo.turma"; } }
      public override string OriginalCol { get { return "turma"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE_TIPO.TURMA"; } }
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
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente_tipo.ano"; } }
      public override string OriginalCol { get { return "ano"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE_TIPO.ANO"; } }
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
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente_tipo.semestre"; } }
      public override string OriginalCol { get { return "semestre"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE_TIPO.SEMESTRE"; } }
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

    #region Coluna: DATA_INICIO
    private ColumnData_inicio pvColData_inicio;

    public sealed class ColumnData_inicio : TDataColumn {

      public ColumnData_inicio() {
        this.ColumnName = "data_inicio";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data início";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente_tipo.data_inicio"; } }
      public override string OriginalCol { get { return "data_inicio"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE_TIPO.DATA_INICIO"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 10; } }
      public override string SqlServerType { get { return "DATETIME"; } }

    }
    #endregion

    #region Coluna: DATA_FIM
    private ColumnData_fim pvColData_fim;

    public sealed class ColumnData_fim : TDataColumn {

      public ColumnData_fim() {
        this.ColumnName = "data_fim";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data final";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente_tipo.data_fim"; } }
      public override string OriginalCol { get { return "data_fim"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE_TIPO.DATA_FIM"; } }
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

    #region Coluna: TIPO_AULA
    private ColumnTipo_aula pvColTipo_aula;

    public sealed class ColumnTipo_aula : TDataColumn {

      public ColumnTipo_aula() {
        this.ColumnName = "tipo_aula";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "tipo de aula";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente_tipo.tipo_aula"; } }
      public override string OriginalCol { get { return "tipo_aula"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE_TIPO.TIPO_AULA"; } }
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

    #region Coluna: STAMP_ATUALIZACAO
    private ColumnStamp_atualizacao pvColStamp_atualizacao;

    public sealed class ColumnStamp_atualizacao : TDataColumn {

      public ColumnStamp_atualizacao() {
        this.ColumnName = "stamp_atualizacao";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "stamp atualização";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente_tipo.stamp_atualizacao"; } }
      public override string OriginalCol { get { return "stamp_atualizacao"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE_TIPO.STAMP_ATUALIZACAO"; } }
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

    #region Coluna: ID_DOCENTE_FUNCAO_GLP
    private ColumnId_docente_funcao_glp pvColId_docente_funcao_glp;

    public sealed class ColumnId_docente_funcao_glp : TDataColumn {

      public ColumnId_docente_funcao_glp() {
        this.ColumnName = "id_docente_funcao_glp";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "id do pedido da glp";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_aula_docente_tipo.id_docente_funcao_glp"; } }
      public override string OriginalCol { get { return "id_docente_funcao_glp"; } }
      public override string RefCol { get { return "LY_AULA_DOCENTE_TIPO.ID_DOCENTE_FUNCAO_GLP"; } }
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

  public partial class Ly_curriculo_unidade_fisica : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_curriculo_unidade_fisica() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_curriculo_unidade_fisica");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_curriculo_unidade_fisica");
      }
      catch{}
    }

    public Ly_curriculo_unidade_fisica() {
      pvColCurso = new ColumnCurso(); Columns.Add(pvColCurso);
      pvColTurno = new ColumnTurno(); Columns.Add(pvColTurno);
      pvColCurriculo = new ColumnCurriculo(); Columns.Add(pvColCurriculo);
      pvColFaculdade = new ColumnFaculdade(); Columns.Add(pvColFaculdade);
      pvColChave = new ColumnChave(); Columns.Add(pvColChave);
      pvColTurma_associada = new ColumnTurma_associada(); Columns.Add(pvColTurma_associada);
      pvColData_ini = new ColumnData_ini(); Columns.Add(pvColData_ini);
      pvColData_fim = new ColumnData_fim(); Columns.Add(pvColData_fim);
      pvColCarga_hor = new ColumnCarga_hor(); Columns.Add(pvColCarga_hor);
      pvColCurso_vinculado = new ColumnCurso_vinculado(); Columns.Add(pvColCurso_vinculado);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_curriculo_unidade_fisica");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @curso, string @turno, string @curriculo, string @faculdade, 
                      decimal? @chave) {
      Row row = NewRow();

      row.Curso = @curso;
      row.Turno = @turno;
      row.Curriculo = @curriculo;
      row.Faculdade = @faculdade;
      row.Chave = @chave;

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
      get { return "LY_CURRICULO_UNIDADE_FISICA"; }
    }

    protected override sealed string Name { get { return "Unidade Física"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColCurso, pvColTurno, pvColCurriculo, pvColFaculdade, 
                                       pvColChave });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_curriculo_unidade_fi_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_curriculo_unidade_fi_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_curriculo_unidade_fi_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_curriculo_unidade_fi_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkCurso", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "CURSO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkTurno", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "TURNO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkCurriculo", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "CURRICULO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkFaculdade", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "FACULDADE", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkChave", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "CHAVE", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("curso", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CURSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("turno", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TURNO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("curriculo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CURRICULO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("faculdade", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "FACULDADE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("chave", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "CHAVE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("turma_associada", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TURMA_ASSOCIADA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("data_ini", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DATA_INI", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("data_fim", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DATA_FIM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("carga_hor", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "CARGA_HOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("curso_vinculado", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CURSO_VINCULADO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "curso") { result.Add(toLower ? "curso" : column); continue; }
          if(lower == "turno") { result.Add(toLower ? "turno" : column); continue; }
          if(lower == "curriculo") { result.Add(toLower ? "curriculo" : column); continue; }
          if(lower == "faculdade") { result.Add(toLower ? "faculdade" : column); continue; }
          if(lower == "chave") { result.Add(toLower ? "chave" : column); continue; }
          if(lower == "turma_associada") { result.Add(toLower ? "turma_associada" : column); continue; }
          if(lower == "data_ini") { result.Add(toLower ? "data_ini" : column); continue; }
          if(lower == "data_fim") { result.Add(toLower ? "data_fim" : column); continue; }
          if(lower == "carga_hor") { result.Add(toLower ? "carga_hor" : column); continue; }
          if(lower == "curso_vinculado") { result.Add(toLower ? "curso_vinculado" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "curso", "turno", "curriculo", "faculdade", "chave", "turma_associada", 
                                           "data_ini", "data_fim", "carga_hor", "curso_vinculado" }
                          : new string[] { "CURSO", "TURNO", "CURRICULO", "FACULDADE", "CHAVE", "TURMA_ASSOCIADA", 
                                           "DATA_INI", "DATA_FIM", "CARGA_HOR", "CURSO_VINCULADO" });
        else
          result.AddRange(toLower
                          ? new string[] { "curso", "turno", "curriculo", "faculdade", "chave", "turma_associada", 
                                           "data_ini", "data_fim", "carga_hor", "curso_vinculado" }
                          : new string[] { "CURSO", "TURNO", "CURRICULO", "FACULDADE", "CHAVE", "TURMA_ASSOCIADA", 
                                           "DATA_INI", "DATA_FIM", "CARGA_HOR", "CURSO_VINCULADO" });

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
        "ly_curriculo_unidade_fisica.curso", "ly_curriculo_unidade_fisica.turno", "ly_curriculo_unidade_fisica.curriculo", 
        "ly_curriculo_unidade_fisica.faculdade", "ly_curriculo_unidade_fisica.chave"
      });
      if(scColumns.Contains("turma_associada")) list.Add("ly_curriculo_unidade_fisica.turma_associada");
      if(scColumns.Contains("data_ini")) list.Add("ly_curriculo_unidade_fisica.data_ini");
      if(scColumns.Contains("data_fim")) list.Add("ly_curriculo_unidade_fisica.data_fim");
      if(scColumns.Contains("carga_hor")) list.Add("ly_curriculo_unidade_fisica.carga_hor");
      if(scColumns.Contains("curso_vinculado")) list.Add("ly_curriculo_unidade_fisica.curso_vinculado");
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
          "FROM ly_curriculo_unidade_fisica " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_curriculo_unidade_fisica");
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
        da.TableMappings.Add("Table", "Ly_curriculo_unidade_fisica");
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
            if (dr["CURSO"] != null && string.IsNullOrEmpty(dr["CURSO"].ToString()))
              dr["CURSO"] = DBNull.Value;
            if (dr["TURNO"] != null && string.IsNullOrEmpty(dr["TURNO"].ToString()))
              dr["TURNO"] = DBNull.Value;
            if (dr["CURRICULO"] != null && string.IsNullOrEmpty(dr["CURRICULO"].ToString()))
              dr["CURRICULO"] = DBNull.Value;
            if (dr["FACULDADE"] != null && string.IsNullOrEmpty(dr["FACULDADE"].ToString()))
              dr["FACULDADE"] = DBNull.Value;
            if (dr["CHAVE"] != null && string.IsNullOrEmpty(dr["CHAVE"].ToString()))
              dr["CHAVE"] = DBNull.Value;
            if (dr["TURMA_ASSOCIADA"] != null && string.IsNullOrEmpty(dr["TURMA_ASSOCIADA"].ToString()))
              dr["TURMA_ASSOCIADA"] = DBNull.Value;
            if (dr["CARGA_HOR"] != null && string.IsNullOrEmpty(dr["CARGA_HOR"].ToString()))
              dr["CARGA_HOR"] = DBNull.Value;
            if (dr["CURSO_VINCULADO"] != null && string.IsNullOrEmpty(dr["CURSO_VINCULADO"].ToString()))
              dr["CURSO_VINCULADO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["CURSO"] == null || string.IsNullOrEmpty(dr["CURSO"].ToString()))
            {
              return "Favor informar o curso.";
            }

            if (dr["CURSO"] != null && !string.IsNullOrEmpty(dr["CURSO"].ToString()))
            {
              if (dr["CURSO"].ToString().Length > 20)
              {
                return "O curso deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["TURNO"] == null || string.IsNullOrEmpty(dr["TURNO"].ToString()))
            {
              return "Favor informar o turno.";
            }

            if (dr["TURNO"] != null && !string.IsNullOrEmpty(dr["TURNO"].ToString()))
            {
              if (dr["TURNO"].ToString().Length > 20)
              {
                return "O turno deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["CURRICULO"] == null || string.IsNullOrEmpty(dr["CURRICULO"].ToString()))
            {
              return "Favor informar o currículo.";
            }

            if (dr["CURRICULO"] != null && !string.IsNullOrEmpty(dr["CURRICULO"].ToString()))
            {
              if (dr["CURRICULO"].ToString().Length > 20)
              {
                return "O currículo deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["FACULDADE"] == null || string.IsNullOrEmpty(dr["FACULDADE"].ToString()))
            {
              return "Favor informar a unidade.";
            }

            if (dr["FACULDADE"] != null && !string.IsNullOrEmpty(dr["FACULDADE"].ToString()))
            {
              if (dr["FACULDADE"].ToString().Length > 20)
              {
                return "A unidade deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["CHAVE"] == null || string.IsNullOrEmpty(dr["CHAVE"].ToString()))
            {
              return "Favor informar a chave.";
            }

            if (dr["CHAVE"] != null && !string.IsNullOrEmpty(dr["CHAVE"].ToString()))
            {
              if ((Convert.ToDecimal(dr["CHAVE"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["CHAVE"]).ToString("0.############").Split(',')[0].Length > 9) || (Convert.ToDecimal(dr["CHAVE"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["CHAVE"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A chave deve ser um valor inteiro com 9 dígitos no máximo.";
              }
            }

            if (dr["TURMA_ASSOCIADA"] != null && !string.IsNullOrEmpty(dr["TURMA_ASSOCIADA"].ToString()))
            {
              if (dr["TURMA_ASSOCIADA"].ToString().Length > 50)
              {
                return "A turma associada deve possuir no máximo 50 caracteres.";
              }
            }

            if (dr["DATA_INI"] != null && dr["DATA_INI"].ToString() != "")
            {
              dr["DATA_INI"] = Convert.ToDateTime(dr["DATA_INI"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DATA_FIM"] != null && dr["DATA_FIM"].ToString() != "")
            {
              dr["DATA_FIM"] = Convert.ToDateTime(dr["DATA_FIM"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["CARGA_HOR"] != null && !string.IsNullOrEmpty(dr["CARGA_HOR"].ToString()))
            {
              if ((Convert.ToDecimal(dr["CARGA_HOR"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["CARGA_HOR"]).ToString("0.############").Split(',')[0].Length > 8) || (Convert.ToDecimal(dr["CARGA_HOR"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["CARGA_HOR"]).ToString("0.############").Split(',')[1].Length > 2))
              {
                return "A carga horária deve ser um valor numérico com, no máximo, 8 dígitos na parte inteira e 2 casas decimal.";
              }
            }

            if (dr["CURSO_VINCULADO"] != null && !string.IsNullOrEmpty(dr["CURSO_VINCULADO"].ToString()))
            {
              if (dr["CURSO_VINCULADO"].ToString().Length > 20)
              {
                return "O Curso_vinculado deve possuir no máximo 20 caracteres.";
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

    public static Ly_curriculo_unidade_fisica Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_curriculo_unidade_fisica Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_curriculo_unidade_fisica tab = new Ly_curriculo_unidade_fisica();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_curriculo_unidade_fisica tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @curso, string @turno, string @curriculo, string @faculdade, decimal? @chave, 
        string @turma_associada, DateTime? @data_ini, DateTime? @data_fim, decimal? @carga_hor, 
        string @curso_vinculado
      ) {
        return (Row)Add(new object[] {
          @curso, @turno, @curriculo, @faculdade, @chave, @turma_associada, @data_ini, @data_fim, 
          @carga_hor, @curso_vinculado
        });
      }

      public Row Find(
        string @curso, string @turno, string @curriculo, string @faculdade, decimal? @chave
      ) {
        return (Row)Find(new object[] {
          @curso, @turno, @curriculo, @faculdade, @chave
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
      /// CURSO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Curso { get { return (string)this["curso"]; } set { this["curso"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TURNO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Turno { get { return (string)this["turno"]; } set { this["turno"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CURRICULO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Curriculo { get { return (string)this["curriculo"]; } set { this["curriculo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// FACULDADE: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Faculdade { get { return (string)this["faculdade"]; } set { this["faculdade"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CHAVE: NUMERIC(9), Primary Key, Not Null
      /// </summary>
      public decimal? Chave { get { return (decimal?)this["chave"]; } set { this["chave"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TURMA_ASSOCIADA: VARCHAR(20)
      /// </summary>
      public string Turma_associada { get { return (string)this["turma_associada"]; } set { this["turma_associada"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DATA_INI: DATETIME
      /// </summary>
      public DateTime? Data_ini { get { return (DateTime?)this["data_ini"]; } set { this["data_ini"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DATA_FIM: DATETIME
      /// </summary>
      public DateTime? Data_fim { get { return (DateTime?)this["data_fim"]; } set { this["data_fim"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CARGA_HOR: NUMERIC(10, 2)
      /// </summary>
      public decimal? Carga_hor { get { return (decimal?)this["carga_hor"]; } set { this["carga_hor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CURSO_VINCULADO: VARCHAR(20)
      /// </summary>
      public string Curso_vinculado { get { return (string)this["curso_vinculado"]; } set { this["curso_vinculado"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @curso, string @turno, string @curriculo, string @faculdade, decimal? @chave) {
        return Insert(connection,
                      @curso, @turno, @curriculo, @faculdade, @chave,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @curso, string @turno, string @curriculo, string @faculdade, decimal? @chave,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_curriculo_unidade_fisica(),
          new string[] { "curso", "turno", "curriculo", "faculdade", "chave" },
          new object[] { @curso, @turno, @curriculo, @faculdade, @chave },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkCurso, string pkTurno, string pkCurriculo, string pkFaculdade, decimal? pkChave,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_curriculo_unidade_fisica(),
          new string[] { "curso", "turno", "curriculo", "faculdade", "chave" },
          new object[] { pkCurso, pkTurno, pkCurriculo, pkFaculdade, pkChave },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkCurso, string pkTurno, string pkCurriculo, string pkFaculdade, decimal? pkChave) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_curriculo_unidade_fisica(),
          new string[] { "curso", "turno", "curriculo", "faculdade", "chave" },
          new object[] { pkCurso, pkTurno, pkCurriculo, pkFaculdade, pkChave }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkCurso, string pkTurno, string pkCurriculo, string pkFaculdade, decimal? pkChave) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_curriculo_unidade_fisica(),
          new string[] { "curso", "turno", "curriculo", "faculdade", "chave" },
          new object[] { pkCurso, pkTurno, pkCurriculo, pkFaculdade, pkChave }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Curso { get { return (string)row["curso", DataRowVersion.Original]; } }
        public string Turno { get { return (string)row["turno", DataRowVersion.Original]; } }
        public string Curriculo { get { return (string)row["curriculo", DataRowVersion.Original]; } }
        public string Faculdade { get { return (string)row["faculdade", DataRowVersion.Original]; } }
        public decimal? Chave { get { return (decimal?)row["chave", DataRowVersion.Original]; } }
        public string Turma_associada { get { return (string)row["turma_associada", DataRowVersion.Original]; } }
        public DateTime? Data_ini { get { return (DateTime?)row["data_ini", DataRowVersion.Original]; } }
        public DateTime? Data_fim { get { return (DateTime?)row["data_fim", DataRowVersion.Original]; } }
        public decimal? Carga_hor { get { return (decimal?)row["carga_hor", DataRowVersion.Original]; } }
        public string Curso_vinculado { get { return (string)row["curso_vinculado", DataRowVersion.Original]; } }

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
    #region Coluna: CURSO
    private ColumnCurso pvColCurso;

    public sealed class ColumnCurso : TDataColumn {

      public ColumnCurso() {
        this.ColumnName = "curso";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "curso";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_curriculo_unidade_fisica.curso"; } }
      public override string OriginalCol { get { return "curso"; } }
      public override string RefCol { get { return "LY_CURRICULO_UNIDADE_FISICA.CURSO"; } }
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

    #region Coluna: TURNO
    private ColumnTurno pvColTurno;

    public sealed class ColumnTurno : TDataColumn {

      public ColumnTurno() {
        this.ColumnName = "turno";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "turno";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_curriculo_unidade_fisica.turno"; } }
      public override string OriginalCol { get { return "turno"; } }
      public override string RefCol { get { return "LY_CURRICULO_UNIDADE_FISICA.TURNO"; } }
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

    #region Coluna: CURRICULO
    private ColumnCurriculo pvColCurriculo;

    public sealed class ColumnCurriculo : TDataColumn {

      public ColumnCurriculo() {
        this.ColumnName = "curriculo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "currículo";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_curriculo_unidade_fisica.curriculo"; } }
      public override string OriginalCol { get { return "curriculo"; } }
      public override string RefCol { get { return "LY_CURRICULO_UNIDADE_FISICA.CURRICULO"; } }
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

    #region Coluna: FACULDADE
    private ColumnFaculdade pvColFaculdade;

    public sealed class ColumnFaculdade : TDataColumn {

      public ColumnFaculdade() {
        this.ColumnName = "faculdade";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "unidade";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_curriculo_unidade_fisica.faculdade"; } }
      public override string OriginalCol { get { return "faculdade"; } }
      public override string RefCol { get { return "LY_CURRICULO_UNIDADE_FISICA.FACULDADE"; } }
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

    #region Coluna: CHAVE
    private ColumnChave pvColChave;

    public sealed class ColumnChave : TDataColumn {

      public ColumnChave() {
        this.ColumnName = "chave";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "chave";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_curriculo_unidade_fisica.chave"; } }
      public override string OriginalCol { get { return "chave"; } }
      public override string RefCol { get { return "LY_CURRICULO_UNIDADE_FISICA.CHAVE"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 10; } }
      public override int Precision { get { return 9; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(9)"; } }

    }
    #endregion

    #region Coluna: TURMA_ASSOCIADA
    private ColumnTurma_associada pvColTurma_associada;

    public sealed class ColumnTurma_associada : TDataColumn {

      public ColumnTurma_associada() {
        this.ColumnName = "turma_associada";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "turma associada";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_curriculo_unidade_fisica.turma_associada"; } }
      public override string OriginalCol { get { return "turma_associada"; } }
      public override string RefCol { get { return "LY_CURRICULO_UNIDADE_FISICA.TURMA_ASSOCIADA"; } }
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

    #region Coluna: DATA_INI
    private ColumnData_ini pvColData_ini;

    public sealed class ColumnData_ini : TDataColumn {

      public ColumnData_ini() {
        this.ColumnName = "data_ini";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data de início";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_curriculo_unidade_fisica.data_ini"; } }
      public override string OriginalCol { get { return "data_ini"; } }
      public override string RefCol { get { return "LY_CURRICULO_UNIDADE_FISICA.DATA_INI"; } }
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

    #region Coluna: DATA_FIM
    private ColumnData_fim pvColData_fim;

    public sealed class ColumnData_fim : TDataColumn {

      public ColumnData_fim() {
        this.ColumnName = "data_fim";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data de sim";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_curriculo_unidade_fisica.data_fim"; } }
      public override string OriginalCol { get { return "data_fim"; } }
      public override string RefCol { get { return "LY_CURRICULO_UNIDADE_FISICA.DATA_FIM"; } }
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

    #region Coluna: CARGA_HOR
    private ColumnCarga_hor pvColCarga_hor;

    public sealed class ColumnCarga_hor : TDataColumn {

      public ColumnCarga_hor() {
        this.ColumnName = "carga_hor";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "carga horária";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_curriculo_unidade_fisica.carga_hor"; } }
      public override string OriginalCol { get { return "carga_hor"; } }
      public override string RefCol { get { return "LY_CURRICULO_UNIDADE_FISICA.CARGA_HOR"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 12; } }
      public override int Precision { get { return 10; } }
      public override int Scale { get { return 2; } }
      public override string SqlServerType { get { return "NUMERIC(10, 2)"; } }

    }
    #endregion

    #region Coluna: CURSO_VINCULADO
    private ColumnCurso_vinculado pvColCurso_vinculado;

    public sealed class ColumnCurso_vinculado : TDataColumn {

      public ColumnCurso_vinculado() {
        this.ColumnName = "curso_vinculado";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_curriculo_unidade_fisica.curso_vinculado"; } }
      public override string OriginalCol { get { return "curso_vinculado"; } }
      public override string RefCol { get { return "LY_CURRICULO_UNIDADE_FISICA.CURSO_VINCULADO"; } }
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

  public partial class Ly_hor_aula : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_hor_aula() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_hor_aula");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_hor_aula");
      }
      catch{}
    }

    public Ly_hor_aula() {
      pvColTurno = new ColumnTurno(); Columns.Add(pvColTurno);
      pvColFaculdade = new ColumnFaculdade(); Columns.Add(pvColFaculdade);
      pvColDia_semana = new ColumnDia_semana(); Columns.Add(pvColDia_semana);
      pvColAula = new ColumnAula(); Columns.Add(pvColAula);
      pvColDisciplina = new ColumnDisciplina(); Columns.Add(pvColDisciplina);
      pvColTurma = new ColumnTurma(); Columns.Add(pvColTurma);
      pvColAno = new ColumnAno(); Columns.Add(pvColAno);
      pvColSemestre = new ColumnSemestre(); Columns.Add(pvColSemestre);
      pvColDependencia = new ColumnDependencia(); Columns.Add(pvColDependencia);
      pvColTipo = new ColumnTipo(); Columns.Add(pvColTipo);
      pvColFrequencia = new ColumnFrequencia(); Columns.Add(pvColFrequencia);
      pvColHoraini_aula = new ColumnHoraini_aula(); Columns.Add(pvColHoraini_aula);
      pvColHorafim_aula = new ColumnHorafim_aula(); Columns.Add(pvColHorafim_aula);
      pvColQtde_aula = new ColumnQtde_aula(); Columns.Add(pvColQtde_aula);
      pvColStamp_atualizacao = new ColumnStamp_atualizacao(); Columns.Add(pvColStamp_atualizacao);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_hor_aula");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @turno, string @faculdade, decimal? @dia_semana, decimal? @aula, 
                      string @disciplina, string @turma, decimal? @ano, decimal? @semestre) {
      Row row = NewRow();

      row.Turno = @turno;
      row.Faculdade = @faculdade;
      row.Dia_semana = @dia_semana;
      row.Aula = @aula;
      row.Disciplina = @disciplina;
      row.Turma = @turma;
      row.Ano = @ano;
      row.Semestre = @semestre;

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
      get { return "LY_HOR_AULA"; }
    }

    protected override sealed string Name { get { return "Horário de Aula"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColTurno, pvColFaculdade, pvColDia_semana, pvColAula, 
                                       pvColDisciplina, pvColTurma, pvColAno, pvColSemestre });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_hor_aula_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_hor_aula_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_hor_aula_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_hor_aula_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkTurno", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "TURNO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkFaculdade", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "FACULDADE", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkDia_semana", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "DIA_SEMANA", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkAula", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "AULA", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkDisciplina", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "DISCIPLINA", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkTurma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "TURMA", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkAno", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ANO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkSemestre", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "SEMESTRE", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("turno", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TURNO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("faculdade", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "FACULDADE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("dia_semana", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "DIA_SEMANA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("aula", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "AULA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("disciplina", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "DISCIPLINA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("turma", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TURMA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("ano", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ANO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("semestre", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "SEMESTRE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("dependencia", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "DEPENDENCIA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("tipo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TIPO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("frequencia", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "FREQUENCIA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("horaini_aula", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "HORAINI_AULA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("horafim_aula", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "HORAFIM_AULA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("qtde_aula", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "QTDE_AULA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
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
          if(lower == "turno") { result.Add(toLower ? "turno" : column); continue; }
          if(lower == "faculdade") { result.Add(toLower ? "faculdade" : column); continue; }
          if(lower == "dia_semana") { result.Add(toLower ? "dia_semana" : column); continue; }
          if(lower == "aula") { result.Add(toLower ? "aula" : column); continue; }
          if(lower == "disciplina") { result.Add(toLower ? "disciplina" : column); continue; }
          if(lower == "turma") { result.Add(toLower ? "turma" : column); continue; }
          if(lower == "ano") { result.Add(toLower ? "ano" : column); continue; }
          if(lower == "semestre") { result.Add(toLower ? "semestre" : column); continue; }
          if(lower == "dependencia") { result.Add(toLower ? "dependencia" : column); continue; }
          if(lower == "tipo") { result.Add(toLower ? "tipo" : column); continue; }
          if(lower == "frequencia") { result.Add(toLower ? "frequencia" : column); continue; }
          if(lower == "horaini_aula") { result.Add(toLower ? "horaini_aula" : column); continue; }
          if(lower == "horafim_aula") { result.Add(toLower ? "horafim_aula" : column); continue; }
          if(lower == "qtde_aula") { result.Add(toLower ? "qtde_aula" : column); continue; }
          if(lower == "stamp_atualizacao") { result.Add(toLower ? "stamp_atualizacao" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "turno", "faculdade", "dia_semana", "aula", "disciplina", "turma", 
                                           "ano", "semestre", "dependencia", "tipo", "frequencia", "horaini_aula", "horafim_aula", 
                                           "qtde_aula", "stamp_atualizacao" }
                          : new string[] { "TURNO", "FACULDADE", "DIA_SEMANA", "AULA", "DISCIPLINA", "TURMA", 
                                           "ANO", "SEMESTRE", "DEPENDENCIA", "TIPO", "FREQUENCIA", "HORAINI_AULA", "HORAFIM_AULA", 
                                           "QTDE_AULA", "STAMP_ATUALIZACAO" });
        else
          result.AddRange(toLower
                          ? new string[] { "turno", "faculdade", "dia_semana", "aula", "disciplina", "turma", 
                                           "ano", "semestre", "dependencia", "tipo", "frequencia", "horaini_aula", "horafim_aula", 
                                           "qtde_aula", "stamp_atualizacao" }
                          : new string[] { "TURNO", "FACULDADE", "DIA_SEMANA", "AULA", "DISCIPLINA", "TURMA", 
                                           "ANO", "SEMESTRE", "DEPENDENCIA", "TIPO", "FREQUENCIA", "HORAINI_AULA", "HORAFIM_AULA", 
                                           "QTDE_AULA", "STAMP_ATUALIZACAO" });

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
        "ly_hor_aula.turno", "ly_hor_aula.faculdade", "ly_hor_aula.dia_semana", "ly_hor_aula.aula", 
        "ly_hor_aula.disciplina", "ly_hor_aula.turma", "ly_hor_aula.ano", "ly_hor_aula.semestre"
      });
      if(scColumns.Contains("dependencia")) list.Add("ly_hor_aula.dependencia");
      if(scColumns.Contains("tipo")) list.Add("ly_hor_aula.tipo");
      if(scColumns.Contains("frequencia")) list.Add("ly_hor_aula.frequencia");
      if(scColumns.Contains("horaini_aula")) list.Add("ly_hor_aula.horaini_aula");
      if(scColumns.Contains("horafim_aula")) list.Add("ly_hor_aula.horafim_aula");
      if(scColumns.Contains("qtde_aula")) list.Add("ly_hor_aula.qtde_aula");
      if(scColumns.Contains("stamp_atualizacao")) list.Add("ly_hor_aula.stamp_atualizacao");
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
          "FROM ly_hor_aula " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_hor_aula");
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
        da.TableMappings.Add("Table", "Ly_hor_aula");
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
            if (dr["TURNO"] != null && string.IsNullOrEmpty(dr["TURNO"].ToString()))
              dr["TURNO"] = DBNull.Value;
            if (dr["FACULDADE"] != null && string.IsNullOrEmpty(dr["FACULDADE"].ToString()))
              dr["FACULDADE"] = DBNull.Value;
            if (dr["DIA_SEMANA"] != null && string.IsNullOrEmpty(dr["DIA_SEMANA"].ToString()))
              dr["DIA_SEMANA"] = DBNull.Value;
            if (dr["AULA"] != null && string.IsNullOrEmpty(dr["AULA"].ToString()))
              dr["AULA"] = DBNull.Value;
            if (dr["DISCIPLINA"] != null && string.IsNullOrEmpty(dr["DISCIPLINA"].ToString()))
              dr["DISCIPLINA"] = DBNull.Value;
            if (dr["TURMA"] != null && string.IsNullOrEmpty(dr["TURMA"].ToString()))
              dr["TURMA"] = DBNull.Value;
            if (dr["ANO"] != null && string.IsNullOrEmpty(dr["ANO"].ToString()))
              dr["ANO"] = DBNull.Value;
            if (dr["SEMESTRE"] != null && string.IsNullOrEmpty(dr["SEMESTRE"].ToString()))
              dr["SEMESTRE"] = DBNull.Value;
            if (dr["DEPENDENCIA"] != null && string.IsNullOrEmpty(dr["DEPENDENCIA"].ToString()))
              dr["DEPENDENCIA"] = DBNull.Value;
            if (dr["TIPO"] != null && string.IsNullOrEmpty(dr["TIPO"].ToString()))
              dr["TIPO"] = DBNull.Value;
            if (dr["FREQUENCIA"] != null && string.IsNullOrEmpty(dr["FREQUENCIA"].ToString()))
              dr["FREQUENCIA"] = DBNull.Value;
            if (dr["QTDE_AULA"] != null && string.IsNullOrEmpty(dr["QTDE_AULA"].ToString()))
              dr["QTDE_AULA"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["TURNO"] == null || string.IsNullOrEmpty(dr["TURNO"].ToString()))
            {
              return "Favor informar o turno.";
            }

            if (dr["TURNO"] != null && !string.IsNullOrEmpty(dr["TURNO"].ToString()))
            {
              if (dr["TURNO"].ToString().Length > 20)
              {
                return "O turno deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["FACULDADE"] == null || string.IsNullOrEmpty(dr["FACULDADE"].ToString()))
            {
              return "Favor informar a unidade.";
            }

            if (dr["FACULDADE"] != null && !string.IsNullOrEmpty(dr["FACULDADE"].ToString()))
            {
              if (dr["FACULDADE"].ToString().Length > 20)
              {
                return "A unidade deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["DIA_SEMANA"] == null || string.IsNullOrEmpty(dr["DIA_SEMANA"].ToString()))
            {
              return "Favor informar o dia da semana.";
            }

            if (dr["DIA_SEMANA"] != null && !string.IsNullOrEmpty(dr["DIA_SEMANA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["DIA_SEMANA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["DIA_SEMANA"]).ToString("0.############").Split(',')[0].Length > 1) || (Convert.ToDecimal(dr["DIA_SEMANA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["DIA_SEMANA"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O dia da semana deve ser um valor inteiro com 1 dígito no máximo.";
              }

              if (dr["DIA_SEMANA"] != null && !string.IsNullOrEmpty(dr["DIA_SEMANA"].ToString()))
              {
                if (Convert.ToDecimal(dr["DIA_SEMANA"]) < 1)
                {
                  return "O dia da semana deve ser maior ou igual a 1.";
                }
              }

              if (dr["DIA_SEMANA"] != null && !string.IsNullOrEmpty(dr["DIA_SEMANA"].ToString()))
              {
                if (Convert.ToDecimal(dr["DIA_SEMANA"]) > 7)
                {
                  return "O dia da semana deve ser menor ou igual a 7.";
                }
              }
            }

            if (dr["AULA"] == null || string.IsNullOrEmpty(dr["AULA"].ToString()))
            {
              return "Favor informar a aula.";
            }

            if (dr["AULA"] != null && !string.IsNullOrEmpty(dr["AULA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["AULA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["AULA"]).ToString("0.############").Split(',')[0].Length > 5) || (Convert.ToDecimal(dr["AULA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["AULA"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A aula deve ser um valor inteiro com 5 dígitos no máximo.";
              }
            }

            if (dr["DISCIPLINA"] == null || string.IsNullOrEmpty(dr["DISCIPLINA"].ToString()))
            {
              return "Favor informar a disciplina.";
            }

            if (dr["DISCIPLINA"] != null && !string.IsNullOrEmpty(dr["DISCIPLINA"].ToString()))
            {
              if (dr["DISCIPLINA"].ToString().Length > 20)
              {
                return "A disciplina deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["TURMA"] == null || string.IsNullOrEmpty(dr["TURMA"].ToString()))
            {
              return "Favor informar a turma.";
            }

            if (dr["TURMA"] != null && !string.IsNullOrEmpty(dr["TURMA"].ToString()))
            {
              if (dr["TURMA"].ToString().Length > 50)
              {
                return "A turma deve possuir no máximo 50 caracteres.";
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

            if (dr["DEPENDENCIA"] != null && !string.IsNullOrEmpty(dr["DEPENDENCIA"].ToString()))
            {
              if (dr["DEPENDENCIA"].ToString().Length > 20)
              {
                return "A dependência deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["TIPO"] != null && !string.IsNullOrEmpty(dr["TIPO"].ToString()))
            {
              if (dr["TIPO"].ToString().Length > 20)
              {
                return "O Tipo deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["FREQUENCIA"] != null && !string.IsNullOrEmpty(dr["FREQUENCIA"].ToString()))
            {
              if (dr["FREQUENCIA"].ToString().Length > 20)
              {
                return "A frequência deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["HORAINI_AULA"] != null && dr["HORAINI_AULA"].ToString() != "")
            {
              dr["HORAINI_AULA"] = Convert.ToDateTime(dr["HORAINI_AULA"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["HORAFIM_AULA"] != null && dr["HORAFIM_AULA"].ToString() != "")
            {
              dr["HORAFIM_AULA"] = Convert.ToDateTime(dr["HORAFIM_AULA"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["QTDE_AULA"] != null && !string.IsNullOrEmpty(dr["QTDE_AULA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["QTDE_AULA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["QTDE_AULA"]).ToString("0.############").Split(',')[0].Length > 8) || (Convert.ToDecimal(dr["QTDE_AULA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["QTDE_AULA"]).ToString("0.############").Split(',')[1].Length > 2))
              {
                return "A quantidade de aulas deve ser um valor numérico com, no máximo, 8 dígitos na parte inteira e 2 casas decimal.";
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

    public static Ly_hor_aula Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_hor_aula Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_hor_aula tab = new Ly_hor_aula();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_hor_aula tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @turno, string @faculdade, decimal? @dia_semana, decimal? @aula, string @disciplina, 
        string @turma, decimal? @ano, decimal? @semestre, string @dependencia, string @tipo, 
        string @frequencia, DateTime? @horaini_aula, DateTime? @horafim_aula, decimal? @qtde_aula, 
        DateTime? @stamp_atualizacao
      ) {
        return (Row)Add(new object[] {
          @turno, @faculdade, @dia_semana, @aula, @disciplina, @turma, @ano, @semestre, @dependencia, 
          @tipo, @frequencia, @horaini_aula, @horafim_aula, @qtde_aula, @stamp_atualizacao
        });
      }

      public Row Find(
        string @turno, string @faculdade, decimal? @dia_semana, decimal? @aula, string @disciplina, 
        string @turma, decimal? @ano, decimal? @semestre
      ) {
        return (Row)Find(new object[] {
          @turno, @faculdade, @dia_semana, @aula, @disciplina, @turma, @ano, @semestre
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
      /// TURNO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Turno { get { return (string)this["turno"]; } set { this["turno"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// FACULDADE: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Faculdade { get { return (string)this["faculdade"]; } set { this["faculdade"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DIA_SEMANA: NUMERIC(1), Primary Key, Not Null
      /// </summary>
      public decimal? Dia_semana { get { return (decimal?)this["dia_semana"]; } set { this["dia_semana"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// AULA: NUMERIC(5), Primary Key, Not Null
      /// </summary>
      public decimal? Aula { get { return (decimal?)this["aula"]; } set { this["aula"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DISCIPLINA: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Disciplina { get { return (string)this["disciplina"]; } set { this["disciplina"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TURMA: VARCHAR(50), Primary Key, Not Null
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
      /// DEPENDENCIA: VARCHAR(20)
      /// </summary>
      public string Dependencia { get { return (string)this["dependencia"]; } set { this["dependencia"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TIPO: VARCHAR(20)
      /// </summary>
      public string Tipo { get { return (string)this["tipo"]; } set { this["tipo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// FREQUENCIA: VARCHAR(20)
      /// </summary>
      public string Frequencia { get { return (string)this["frequencia"]; } set { this["frequencia"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// HORAINI_AULA: DATETIME
      /// </summary>
      public DateTime? Horaini_aula { get { return (DateTime?)this["horaini_aula"]; } set { this["horaini_aula"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// HORAFIM_AULA: DATETIME
      /// </summary>
      public DateTime? Horafim_aula { get { return (DateTime?)this["horafim_aula"]; } set { this["horafim_aula"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// QTDE_AULA: NUMERIC(10, 2)
      /// </summary>
      public decimal? Qtde_aula { get { return (decimal?)this["qtde_aula"]; } set { this["qtde_aula"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// STAMP_ATUALIZACAO: DATETIME
      /// </summary>
      public DateTime? Stamp_atualizacao { get { return (DateTime?)this["stamp_atualizacao"]; } set { this["stamp_atualizacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @turno, string @faculdade, decimal? @dia_semana, decimal? @aula, string @disciplina, 
                               string @turma, decimal? @ano, decimal? @semestre) {
        return Insert(connection,
                      @turno, @faculdade, @dia_semana, @aula, @disciplina, @turma, @ano, @semestre,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @turno, string @faculdade, decimal? @dia_semana, decimal? @aula, string @disciplina, 
                               string @turma, decimal? @ano, decimal? @semestre,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_hor_aula(),
          new string[] { "turno", "faculdade", "dia_semana", "aula", "disciplina", "turma", 
                         "ano", "semestre" },
          new object[] { @turno, @faculdade, @dia_semana, @aula, @disciplina, @turma, @ano, 
                         @semestre },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkTurno, string pkFaculdade, decimal? pkDia_semana, decimal? pkAula, string pkDisciplina, 
                               string pkTurma, decimal? pkAno, decimal? pkSemestre,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_hor_aula(),
          new string[] { "turno", "faculdade", "dia_semana", "aula", "disciplina", "turma", 
                         "ano", "semestre" },
          new object[] { pkTurno, pkFaculdade, pkDia_semana, pkAula, pkDisciplina, pkTurma, 
                         pkAno, pkSemestre },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkTurno, string pkFaculdade, decimal? pkDia_semana, decimal? pkAula, string pkDisciplina, 
                                string pkTurma, decimal? pkAno, decimal? pkSemestre) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_hor_aula(),
          new string[] { "turno", "faculdade", "dia_semana", "aula", "disciplina", "turma", 
                         "ano", "semestre" },
          new object[] { pkTurno, pkFaculdade, pkDia_semana, pkAula, pkDisciplina, pkTurma, 
                         pkAno, pkSemestre }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkTurno, string pkFaculdade, decimal? pkDia_semana, decimal? pkAula, string pkDisciplina, 
                              string pkTurma, decimal? pkAno, decimal? pkSemestre) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_hor_aula(),
          new string[] { "turno", "faculdade", "dia_semana", "aula", "disciplina", "turma", 
                         "ano", "semestre" },
          new object[] { pkTurno, pkFaculdade, pkDia_semana, pkAula, pkDisciplina, pkTurma, 
                         pkAno, pkSemestre }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Turno { get { return (string)row["turno", DataRowVersion.Original]; } }
        public string Faculdade { get { return (string)row["faculdade", DataRowVersion.Original]; } }
        public decimal? Dia_semana { get { return (decimal?)row["dia_semana", DataRowVersion.Original]; } }
        public decimal? Aula { get { return (decimal?)row["aula", DataRowVersion.Original]; } }
        public string Disciplina { get { return (string)row["disciplina", DataRowVersion.Original]; } }
        public string Turma { get { return (string)row["turma", DataRowVersion.Original]; } }
        public decimal? Ano { get { return (decimal?)row["ano", DataRowVersion.Original]; } }
        public decimal? Semestre { get { return (decimal?)row["semestre", DataRowVersion.Original]; } }
        public string Dependencia { get { return (string)row["dependencia", DataRowVersion.Original]; } }
        public string Tipo { get { return (string)row["tipo", DataRowVersion.Original]; } }
        public string Frequencia { get { return (string)row["frequencia", DataRowVersion.Original]; } }
        public DateTime? Horaini_aula { get { return (DateTime?)row["horaini_aula", DataRowVersion.Original]; } }
        public DateTime? Horafim_aula { get { return (DateTime?)row["horafim_aula", DataRowVersion.Original]; } }
        public decimal? Qtde_aula { get { return (decimal?)row["qtde_aula", DataRowVersion.Original]; } }
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
    #region Coluna: TURNO
    private ColumnTurno pvColTurno;

    public sealed class ColumnTurno : TDataColumn {

      public ColumnTurno() {
        this.ColumnName = "turno";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "turno";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_hor_aula.turno"; } }
      public override string OriginalCol { get { return "turno"; } }
      public override string RefCol { get { return "LY_HOR_AULA.TURNO"; } }
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

    #region Coluna: FACULDADE
    private ColumnFaculdade pvColFaculdade;

    public sealed class ColumnFaculdade : TDataColumn {

      public ColumnFaculdade() {
        this.ColumnName = "faculdade";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "unidade";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_hor_aula.faculdade"; } }
      public override string OriginalCol { get { return "faculdade"; } }
      public override string RefCol { get { return "LY_HOR_AULA.FACULDADE"; } }
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

    #region Coluna: DIA_SEMANA
    private ColumnDia_semana pvColDia_semana;

    public sealed class ColumnDia_semana : TDataColumn {

      public ColumnDia_semana() {
        this.ColumnName = "dia_semana";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "dia da semana";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_hor_aula.dia_semana"; } }
      public override string OriginalCol { get { return "dia_semana"; } }
      public override string RefCol { get { return "LY_HOR_AULA.DIA_SEMANA"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 2; } }
      public override int Precision { get { return 1; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(1)"; } }

      public override object MinValue { get { return 1; } }

      public override object MaxValue { get { return 7; } }

    }
    #endregion

    #region Coluna: AULA
    private ColumnAula pvColAula;

    public sealed class ColumnAula : TDataColumn {

      public ColumnAula() {
        this.ColumnName = "aula";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "aula";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_hor_aula.aula"; } }
      public override string OriginalCol { get { return "aula"; } }
      public override string RefCol { get { return "LY_HOR_AULA.AULA"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 4; } }
      public override int Precision { get { return 5; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(5)"; } }

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
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_hor_aula.disciplina"; } }
      public override string OriginalCol { get { return "disciplina"; } }
      public override string RefCol { get { return "LY_HOR_AULA.DISCIPLINA"; } }
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


      protected override string GetName(){return "turma";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_hor_aula.turma"; } }
      public override string OriginalCol { get { return "turma"; } }
      public override string RefCol { get { return "LY_HOR_AULA.TURMA"; } }
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
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_hor_aula.ano"; } }
      public override string OriginalCol { get { return "ano"; } }
      public override string RefCol { get { return "LY_HOR_AULA.ANO"; } }
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
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_hor_aula.semestre"; } }
      public override string OriginalCol { get { return "semestre"; } }
      public override string RefCol { get { return "LY_HOR_AULA.SEMESTRE"; } }
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
      public override string FullCol { get { return "ly_hor_aula.dependencia"; } }
      public override string OriginalCol { get { return "dependencia"; } }
      public override string RefCol { get { return "LY_HOR_AULA.DEPENDENCIA"; } }
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
      public override string FullCol { get { return "ly_hor_aula.tipo"; } }
      public override string OriginalCol { get { return "tipo"; } }
      public override string RefCol { get { return "LY_HOR_AULA.TIPO"; } }
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

    #region Coluna: FREQUENCIA
    private ColumnFrequencia pvColFrequencia;

    public sealed class ColumnFrequencia : TDataColumn {

      public ColumnFrequencia() {
        this.ColumnName = "frequencia";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "frequência";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_hor_aula.frequencia"; } }
      public override string OriginalCol { get { return "frequencia"; } }
      public override string RefCol { get { return "LY_HOR_AULA.FREQUENCIA"; } }
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

    #region Coluna: HORAINI_AULA
    private ColumnHoraini_aula pvColHoraini_aula;

    public sealed class ColumnHoraini_aula : TDataColumn {

      public ColumnHoraini_aula() {
        this.ColumnName = "horaini_aula";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "hora início aula";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_hor_aula.horaini_aula"; } }
      public override string OriginalCol { get { return "horaini_aula"; } }
      public override string RefCol { get { return "LY_HOR_AULA.HORAINI_AULA"; } }
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

    #region Coluna: HORAFIM_AULA
    private ColumnHorafim_aula pvColHorafim_aula;

    public sealed class ColumnHorafim_aula : TDataColumn {

      public ColumnHorafim_aula() {
        this.ColumnName = "horafim_aula";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "hora fim aula";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_hor_aula.horafim_aula"; } }
      public override string OriginalCol { get { return "horafim_aula"; } }
      public override string RefCol { get { return "LY_HOR_AULA.HORAFIM_AULA"; } }
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

    #region Coluna: QTDE_AULA
    private ColumnQtde_aula pvColQtde_aula;

    public sealed class ColumnQtde_aula : TDataColumn {

      public ColumnQtde_aula() {
        this.ColumnName = "qtde_aula";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "quantidade de aulas";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_hor_aula.qtde_aula"; } }
      public override string OriginalCol { get { return "qtde_aula"; } }
      public override string RefCol { get { return "LY_HOR_AULA.QTDE_AULA"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 12; } }
      public override int Precision { get { return 10; } }
      public override int Scale { get { return 2; } }
      public override string SqlServerType { get { return "NUMERIC(10, 2)"; } }

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
      public override string FullCol { get { return "ly_hor_aula.stamp_atualizacao"; } }
      public override string OriginalCol { get { return "stamp_atualizacao"; } }
      public override string RefCol { get { return "LY_HOR_AULA.STAMP_ATUALIZACAO"; } }
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

  public partial class Ly_hor_oper : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_hor_oper() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_hor_oper");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_hor_oper");
      }
      catch{}
    }

    public Ly_hor_oper() {
      pvColTurno = new ColumnTurno(); Columns.Add(pvColTurno);
      pvColFaculdade = new ColumnFaculdade(); Columns.Add(pvColFaculdade);
      pvColDia_semana = new ColumnDia_semana(); Columns.Add(pvColDia_semana);
      pvColAula = new ColumnAula(); Columns.Add(pvColAula);
      pvColHoraini_aula = new ColumnHoraini_aula(); Columns.Add(pvColHoraini_aula);
      pvColHorafim_aula = new ColumnHorafim_aula(); Columns.Add(pvColHorafim_aula);
      pvColCurso = new ColumnCurso(); Columns.Add(pvColCurso);
      pvColCurriculo = new ColumnCurriculo(); Columns.Add(pvColCurriculo);
      pvColSerie = new ColumnSerie(); Columns.Add(pvColSerie);
      pvColOrdem = new ColumnOrdem(); Columns.Add(pvColOrdem);
      pvColStamp_atualizacao = new ColumnStamp_atualizacao(); Columns.Add(pvColStamp_atualizacao);
      pvColDuracao_aula = new ColumnDuracao_aula(); Columns.Add(pvColDuracao_aula);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_hor_oper");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @turno, string @faculdade, decimal? @dia_semana, decimal? @aula, 
                      DateTime? @horaini_aula, DateTime? @horafim_aula) {
      Row row = NewRow();

      row.Turno = @turno;
      row.Faculdade = @faculdade;
      row.Dia_semana = @dia_semana;
      row.Aula = @aula;
      row.Horaini_aula = @horaini_aula;
      row.Horafim_aula = @horafim_aula;

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
      get { return "LY_HOR_OPER"; }
    }

    protected override sealed string Name { get { return "Horário Operacional"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColTurno, pvColFaculdade, pvColDia_semana, pvColAula });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_hor_oper_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_hor_oper_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_hor_oper_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_hor_oper_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkTurno", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "TURNO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkFaculdade", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "FACULDADE", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkDia_semana", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "DIA_SEMANA", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkAula", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "AULA", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("turno", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TURNO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("faculdade", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "FACULDADE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("dia_semana", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "DIA_SEMANA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("aula", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "AULA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("horaini_aula", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "HORAINI_AULA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("horafim_aula", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "HORAFIM_AULA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("curso", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CURSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("curriculo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CURRICULO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("serie", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "SERIE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("ordem", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ORDEM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("stamp_atualizacao", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "STAMP_ATUALIZACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("duracao_aula", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "DURACAO_AULA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "turno") { result.Add(toLower ? "turno" : column); continue; }
          if(lower == "faculdade") { result.Add(toLower ? "faculdade" : column); continue; }
          if(lower == "dia_semana") { result.Add(toLower ? "dia_semana" : column); continue; }
          if(lower == "aula") { result.Add(toLower ? "aula" : column); continue; }
          if(lower == "horaini_aula") { result.Add(toLower ? "horaini_aula" : column); continue; }
          if(lower == "horafim_aula") { result.Add(toLower ? "horafim_aula" : column); continue; }
          if(lower == "curso") { result.Add(toLower ? "curso" : column); continue; }
          if(lower == "curriculo") { result.Add(toLower ? "curriculo" : column); continue; }
          if(lower == "serie") { result.Add(toLower ? "serie" : column); continue; }
          if(lower == "ordem") { result.Add(toLower ? "ordem" : column); continue; }
          if(lower == "stamp_atualizacao") { result.Add(toLower ? "stamp_atualizacao" : column); continue; }
          if(lower == "duracao_aula") { result.Add(toLower ? "duracao_aula" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "turno", "faculdade", "dia_semana", "aula", "horaini_aula", "horafim_aula", 
                                           "curso", "curriculo", "serie", "ordem", "stamp_atualizacao", "duracao_aula" }
                          : new string[] { "TURNO", "FACULDADE", "DIA_SEMANA", "AULA", "HORAINI_AULA", "HORAFIM_AULA", 
                                           "CURSO", "CURRICULO", "SERIE", "ORDEM", "STAMP_ATUALIZACAO", "DURACAO_AULA" });
        else
          result.AddRange(toLower
                          ? new string[] { "turno", "faculdade", "dia_semana", "aula", "horaini_aula", "horafim_aula", 
                                           "curso", "curriculo", "serie", "ordem", "stamp_atualizacao", "duracao_aula" }
                          : new string[] { "TURNO", "FACULDADE", "DIA_SEMANA", "AULA", "HORAINI_AULA", "HORAFIM_AULA", 
                                           "CURSO", "CURRICULO", "SERIE", "ORDEM", "STAMP_ATUALIZACAO", "DURACAO_AULA" });

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
        "ly_hor_oper.turno", "ly_hor_oper.faculdade", "ly_hor_oper.dia_semana", "ly_hor_oper.aula"
      });
      if(scColumns.Contains("horaini_aula")) list.Add("ly_hor_oper.horaini_aula");
      if(scColumns.Contains("horafim_aula")) list.Add("ly_hor_oper.horafim_aula");
      if(scColumns.Contains("curso")) list.Add("ly_hor_oper.curso");
      if(scColumns.Contains("curriculo")) list.Add("ly_hor_oper.curriculo");
      if(scColumns.Contains("serie")) list.Add("ly_hor_oper.serie");
      if(scColumns.Contains("ordem")) list.Add("ly_hor_oper.ordem");
      if(scColumns.Contains("stamp_atualizacao")) list.Add("ly_hor_oper.stamp_atualizacao");
      if(scColumns.Contains("duracao_aula")) list.Add("ly_hor_oper.duracao_aula");
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
          "FROM ly_hor_oper " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_hor_oper");
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
        da.TableMappings.Add("Table", "Ly_hor_oper");
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
            if (dr["TURNO"] != null && string.IsNullOrEmpty(dr["TURNO"].ToString()))
              dr["TURNO"] = DBNull.Value;
            if (dr["FACULDADE"] != null && string.IsNullOrEmpty(dr["FACULDADE"].ToString()))
              dr["FACULDADE"] = DBNull.Value;
            if (dr["DIA_SEMANA"] != null && string.IsNullOrEmpty(dr["DIA_SEMANA"].ToString()))
              dr["DIA_SEMANA"] = DBNull.Value;
            if (dr["AULA"] != null && string.IsNullOrEmpty(dr["AULA"].ToString()))
              dr["AULA"] = DBNull.Value;
            if (dr["CURSO"] != null && string.IsNullOrEmpty(dr["CURSO"].ToString()))
              dr["CURSO"] = DBNull.Value;
            if (dr["CURRICULO"] != null && string.IsNullOrEmpty(dr["CURRICULO"].ToString()))
              dr["CURRICULO"] = DBNull.Value;
            if (dr["SERIE"] != null && string.IsNullOrEmpty(dr["SERIE"].ToString()))
              dr["SERIE"] = DBNull.Value;
            if (dr["ORDEM"] != null && string.IsNullOrEmpty(dr["ORDEM"].ToString()))
              dr["ORDEM"] = DBNull.Value;
            if (dr["DURACAO_AULA"] != null && string.IsNullOrEmpty(dr["DURACAO_AULA"].ToString()))
              dr["DURACAO_AULA"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["TURNO"] == null || string.IsNullOrEmpty(dr["TURNO"].ToString()))
            {
              return "Favor informar o turno.";
            }

            if (dr["TURNO"] != null && !string.IsNullOrEmpty(dr["TURNO"].ToString()))
            {
              if (dr["TURNO"].ToString().Length > 20)
              {
                return "O turno deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["FACULDADE"] == null || string.IsNullOrEmpty(dr["FACULDADE"].ToString()))
            {
              return "Favor informar a unidade.";
            }

            if (dr["FACULDADE"] != null && !string.IsNullOrEmpty(dr["FACULDADE"].ToString()))
            {
              if (dr["FACULDADE"].ToString().Length > 20)
              {
                return "A unidade deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["DIA_SEMANA"] == null || string.IsNullOrEmpty(dr["DIA_SEMANA"].ToString()))
            {
              return "Favor informar a dia da semana.";
            }

            if (dr["DIA_SEMANA"] != null && !string.IsNullOrEmpty(dr["DIA_SEMANA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["DIA_SEMANA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["DIA_SEMANA"]).ToString("0.############").Split(',')[0].Length > 1) || (Convert.ToDecimal(dr["DIA_SEMANA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["DIA_SEMANA"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A dia da semana deve ser um valor inteiro com 1 dígito no máximo.";
              }

              if (dr["DIA_SEMANA"] != null && !string.IsNullOrEmpty(dr["DIA_SEMANA"].ToString()))
              {
                if (Convert.ToDecimal(dr["DIA_SEMANA"]) < 1)
                {
                  return "A dia da semana deve ser maior ou igual a 1.";
                }
              }

              if (dr["DIA_SEMANA"] != null && !string.IsNullOrEmpty(dr["DIA_SEMANA"].ToString()))
              {
                if (Convert.ToDecimal(dr["DIA_SEMANA"]) > 7)
                {
                  return "A dia da semana deve ser menor ou igual a 7.";
                }
              }
            }

            if (dr["AULA"] == null || string.IsNullOrEmpty(dr["AULA"].ToString()))
            {
              return "Favor informar a aula.";
            }

            if (dr["AULA"] != null && !string.IsNullOrEmpty(dr["AULA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["AULA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["AULA"]).ToString("0.############").Split(',')[0].Length > 5) || (Convert.ToDecimal(dr["AULA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["AULA"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A aula deve ser um valor inteiro com 5 dígitos no máximo.";
              }
            }

            if (dr["HORAINI_AULA"] == null || string.IsNullOrEmpty(dr["HORAINI_AULA"].ToString()))
            {
              return "Favor informar a hora de fim aula.";
            }

            if (dr["HORAINI_AULA"] != null && dr["HORAINI_AULA"].ToString() != "")
            {
              dr["HORAINI_AULA"] = Convert.ToDateTime(dr["HORAINI_AULA"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["HORAFIM_AULA"] == null || string.IsNullOrEmpty(dr["HORAFIM_AULA"].ToString()))
            {
              return "Favor informar a hora de início aula.";
            }

            if (dr["HORAFIM_AULA"] != null && dr["HORAFIM_AULA"].ToString() != "")
            {
              dr["HORAFIM_AULA"] = Convert.ToDateTime(dr["HORAFIM_AULA"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["CURSO"] != null && !string.IsNullOrEmpty(dr["CURSO"].ToString()))
            {
              if (dr["CURSO"].ToString().Length > 20)
              {
                return "O Curso deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["CURRICULO"] != null && !string.IsNullOrEmpty(dr["CURRICULO"].ToString()))
            {
              if (dr["CURRICULO"].ToString().Length > 20)
              {
                return "O Currículo deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["SERIE"] != null && !string.IsNullOrEmpty(dr["SERIE"].ToString()))
            {
              if ((Convert.ToDecimal(dr["SERIE"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["SERIE"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["SERIE"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["SERIE"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A Série deve ser um valor inteiro com 3 dígitos no máximo.";
              }
            }

            if (dr["ORDEM"] != null && !string.IsNullOrEmpty(dr["ORDEM"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ORDEM"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ORDEM"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["ORDEM"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ORDEM"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A Ordem deve ser um valor inteiro com 3 dígitos no máximo.";
              }
            }

            if (dr["STAMP_ATUALIZACAO"] != null && dr["STAMP_ATUALIZACAO"].ToString() != "")
            {
              dr["STAMP_ATUALIZACAO"] = Convert.ToDateTime(dr["STAMP_ATUALIZACAO"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DURACAO_AULA"] != null && !string.IsNullOrEmpty(dr["DURACAO_AULA"].ToString()))
            {
              if ((Convert.ToDecimal(dr["DURACAO_AULA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["DURACAO_AULA"]).ToString("0.############").Split(',')[0].Length > 8) || (Convert.ToDecimal(dr["DURACAO_AULA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["DURACAO_AULA"]).ToString("0.############").Split(',')[1].Length > 2))
              {
                return "O Duracao_aula deve ser um valor numérico com, no máximo, 8 dígitos na parte inteira e 2 casas decimal.";
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

    public static Ly_hor_oper Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_hor_oper Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_hor_oper tab = new Ly_hor_oper();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_hor_oper tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @turno, string @faculdade, decimal? @dia_semana, decimal? @aula, DateTime? @horaini_aula, 
        DateTime? @horafim_aula, string @curso, string @curriculo, decimal? @serie, decimal? @ordem, 
        DateTime? @stamp_atualizacao, decimal? @duracao_aula
      ) {
        return (Row)Add(new object[] {
          @turno, @faculdade, @dia_semana, @aula, @horaini_aula, @horafim_aula, @curso, @curriculo, 
          @serie, @ordem, @stamp_atualizacao, @duracao_aula
        });
      }

      public Row Find(
        string @turno, string @faculdade, decimal? @dia_semana, decimal? @aula
      ) {
        return (Row)Find(new object[] {
          @turno, @faculdade, @dia_semana, @aula
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
      /// TURNO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Turno { get { return (string)this["turno"]; } set { this["turno"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// FACULDADE: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Faculdade { get { return (string)this["faculdade"]; } set { this["faculdade"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DIA_SEMANA: NUMERIC(1), Primary Key, Not Null
      /// </summary>
      public decimal? Dia_semana { get { return (decimal?)this["dia_semana"]; } set { this["dia_semana"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// AULA: NUMERIC(3), Primary Key, Not Null
      /// </summary>
      public decimal? Aula { get { return (decimal?)this["aula"]; } set { this["aula"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// HORAINI_AULA: DATETIME, Not Null
      /// </summary>
      public DateTime? Horaini_aula { get { return (DateTime?)this["horaini_aula"]; } set { this["horaini_aula"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// HORAFIM_AULA: DATETIME, Not Null
      /// </summary>
      public DateTime? Horafim_aula { get { return (DateTime?)this["horafim_aula"]; } set { this["horafim_aula"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CURSO: VARCHAR(20)
      /// </summary>
      public string Curso { get { return (string)this["curso"]; } set { this["curso"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CURRICULO: VARCHAR(20)
      /// </summary>
      public string Curriculo { get { return (string)this["curriculo"]; } set { this["curriculo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SERIE: NUMERIC(3)
      /// </summary>
      public decimal? Serie { get { return (decimal?)this["serie"]; } set { this["serie"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ORDEM: NUMERIC(3)
      /// </summary>
      public decimal? Ordem { get { return (decimal?)this["ordem"]; } set { this["ordem"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// STAMP_ATUALIZACAO: DATETIME
      /// </summary>
      public DateTime? Stamp_atualizacao { get { return (DateTime?)this["stamp_atualizacao"]; } set { this["stamp_atualizacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DURACAO_AULA: NUMERIC(10, 2)
      /// </summary>
      public decimal? Duracao_aula { get { return (decimal?)this["duracao_aula"]; } set { this["duracao_aula"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @turno, string @faculdade, decimal? @dia_semana, decimal? @aula, DateTime? @horaini_aula, 
                               DateTime? @horafim_aula) {
        return Insert(connection,
                      @turno, @faculdade, @dia_semana, @aula, @horaini_aula, @horafim_aula,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @turno, string @faculdade, decimal? @dia_semana, decimal? @aula, DateTime? @horaini_aula, 
                               DateTime? @horafim_aula,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_hor_oper(),
          new string[] { "turno", "faculdade", "dia_semana", "aula", "horaini_aula", "horafim_aula" },
          new object[] { @turno, @faculdade, @dia_semana, @aula, @horaini_aula, @horafim_aula },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkTurno, string pkFaculdade, decimal? pkDia_semana, decimal? pkAula,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_hor_oper(),
          new string[] { "turno", "faculdade", "dia_semana", "aula" },
          new object[] { pkTurno, pkFaculdade, pkDia_semana, pkAula },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkTurno, string pkFaculdade, decimal? pkDia_semana, decimal? pkAula) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_hor_oper(),
          new string[] { "turno", "faculdade", "dia_semana", "aula" },
          new object[] { pkTurno, pkFaculdade, pkDia_semana, pkAula }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkTurno, string pkFaculdade, decimal? pkDia_semana, decimal? pkAula) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_hor_oper(),
          new string[] { "turno", "faculdade", "dia_semana", "aula" },
          new object[] { pkTurno, pkFaculdade, pkDia_semana, pkAula }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Turno { get { return (string)row["turno", DataRowVersion.Original]; } }
        public string Faculdade { get { return (string)row["faculdade", DataRowVersion.Original]; } }
        public decimal? Dia_semana { get { return (decimal?)row["dia_semana", DataRowVersion.Original]; } }
        public decimal? Aula { get { return (decimal?)row["aula", DataRowVersion.Original]; } }
        public DateTime? Horaini_aula { get { return (DateTime?)row["horaini_aula", DataRowVersion.Original]; } }
        public DateTime? Horafim_aula { get { return (DateTime?)row["horafim_aula", DataRowVersion.Original]; } }
        public string Curso { get { return (string)row["curso", DataRowVersion.Original]; } }
        public string Curriculo { get { return (string)row["curriculo", DataRowVersion.Original]; } }
        public decimal? Serie { get { return (decimal?)row["serie", DataRowVersion.Original]; } }
        public decimal? Ordem { get { return (decimal?)row["ordem", DataRowVersion.Original]; } }
        public DateTime? Stamp_atualizacao { get { return (DateTime?)row["stamp_atualizacao", DataRowVersion.Original]; } }
        public decimal? Duracao_aula { get { return (decimal?)row["duracao_aula", DataRowVersion.Original]; } }

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
    #region Coluna: TURNO
    private ColumnTurno pvColTurno;

    public sealed class ColumnTurno : TDataColumn {

      public ColumnTurno() {
        this.ColumnName = "turno";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "turno";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_hor_oper.turno"; } }
      public override string OriginalCol { get { return "turno"; } }
      public override string RefCol { get { return "LY_HOR_OPER.TURNO"; } }
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

    #region Coluna: FACULDADE
    private ColumnFaculdade pvColFaculdade;

    public sealed class ColumnFaculdade : TDataColumn {

      public ColumnFaculdade() {
        this.ColumnName = "faculdade";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "unidade";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_hor_oper.faculdade"; } }
      public override string OriginalCol { get { return "faculdade"; } }
      public override string RefCol { get { return "LY_HOR_OPER.FACULDADE"; } }
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

    #region Coluna: DIA_SEMANA
    private ColumnDia_semana pvColDia_semana;

    public sealed class ColumnDia_semana : TDataColumn {

      public ColumnDia_semana() {
        this.ColumnName = "dia_semana";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "dia da semana";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_hor_oper.dia_semana"; } }
      public override string OriginalCol { get { return "dia_semana"; } }
      public override string RefCol { get { return "LY_HOR_OPER.DIA_SEMANA"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 2; } }
      public override int Precision { get { return 1; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(1)"; } }

      public override object MinValue { get { return 1; } }

      public override object MaxValue { get { return 7; } }

    }
    #endregion

    #region Coluna: AULA
    private ColumnAula pvColAula;

    public sealed class ColumnAula : TDataColumn {

      public ColumnAula() {
        this.ColumnName = "aula";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "aula";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_hor_oper.aula"; } }
      public override string OriginalCol { get { return "aula"; } }
      public override string RefCol { get { return "LY_HOR_OPER.AULA"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 5; } }
      public override int Precision { get { return 5; } }
      public override int Scale { get { return 0; } }
      public override string SqlServerType { get { return "NUMERIC(5)"; } }

    }
    #endregion

    #region Coluna: HORAINI_AULA
    private ColumnHoraini_aula pvColHoraini_aula;

    public sealed class ColumnHoraini_aula : TDataColumn {

      public ColumnHoraini_aula() {
        this.ColumnName = "horaini_aula";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "hora de fim aula";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_hor_oper.horaini_aula"; } }
      public override string OriginalCol { get { return "horaini_aula"; } }
      public override string RefCol { get { return "LY_HOR_OPER.HORAINI_AULA"; } }
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

    #region Coluna: HORAFIM_AULA
    private ColumnHorafim_aula pvColHorafim_aula;

    public sealed class ColumnHorafim_aula : TDataColumn {

      public ColumnHorafim_aula() {
        this.ColumnName = "horafim_aula";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "hora de início aula";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_hor_oper.horafim_aula"; } }
      public override string OriginalCol { get { return "horafim_aula"; } }
      public override string RefCol { get { return "LY_HOR_OPER.HORAFIM_AULA"; } }
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

    #region Coluna: CURSO
    private ColumnCurso pvColCurso;

    public sealed class ColumnCurso : TDataColumn {

      public ColumnCurso() {
        this.ColumnName = "curso";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Curso";}
      protected override string GetCaption(){return "Curso";}
      protected override string GetDescricao(){return "Curso";}
      public override string FullCol { get { return "ly_hor_oper.curso"; } }
      public override string OriginalCol { get { return "curso"; } }
      public override string RefCol { get { return "LY_HOR_OPER.CURSO"; } }
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

    #region Coluna: CURRICULO
    private ColumnCurriculo pvColCurriculo;

    public sealed class ColumnCurriculo : TDataColumn {

      public ColumnCurriculo() {
        this.ColumnName = "curriculo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Currículo";}
      protected override string GetCaption(){return "Currículo";}
      protected override string GetDescricao(){return "Currículo";}
      public override string FullCol { get { return "ly_hor_oper.curriculo"; } }
      public override string OriginalCol { get { return "curriculo"; } }
      public override string RefCol { get { return "LY_HOR_OPER.CURRICULO"; } }
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

    #region Coluna: SERIE
    private ColumnSerie pvColSerie;

    public sealed class ColumnSerie : TDataColumn {

      public ColumnSerie() {
        this.ColumnName = "serie";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "Série";}
      protected override string GetCaption(){return "Série";}
      protected override string GetDescricao(){return "Série";}
      public override string FullCol { get { return "ly_hor_oper.serie"; } }
      public override string OriginalCol { get { return "serie"; } }
      public override string RefCol { get { return "LY_HOR_OPER.SERIE"; } }
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

    #region Coluna: ORDEM
    private ColumnOrdem pvColOrdem;

    public sealed class ColumnOrdem : TDataColumn {

      public ColumnOrdem() {
        this.ColumnName = "ordem";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "Ordem";}
      protected override string GetCaption(){return "Ordem";}
      protected override string GetDescricao(){return "Ordem";}
      public override string FullCol { get { return "ly_hor_oper.ordem"; } }
      public override string OriginalCol { get { return "ordem"; } }
      public override string RefCol { get { return "LY_HOR_OPER.ORDEM"; } }
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
      public override string FullCol { get { return "ly_hor_oper.stamp_atualizacao"; } }
      public override string OriginalCol { get { return "stamp_atualizacao"; } }
      public override string RefCol { get { return "LY_HOR_OPER.STAMP_ATUALIZACAO"; } }
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

    #region Coluna: DURACAO_AULA
    private ColumnDuracao_aula pvColDuracao_aula;

    public sealed class ColumnDuracao_aula : TDataColumn {

      public ColumnDuracao_aula() {
        this.ColumnName = "duracao_aula";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_hor_oper.duracao_aula"; } }
      public override string OriginalCol { get { return "duracao_aula"; } }
      public override string RefCol { get { return "LY_HOR_OPER.DURACAO_AULA"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override int Size { get { return 12; } }
      public override int Precision { get { return 10; } }
      public override int Scale { get { return 2; } }
      public override string SqlServerType { get { return "NUMERIC(10, 2)"; } }

    }
    #endregion
    #endregion
  }

  public partial class Ly_padaces_funcao : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_padaces_funcao() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_padaces_funcao");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_padaces_funcao");
      }
      catch{}
    }

    public Ly_padaces_funcao() {
      pvColPadaces = new ColumnPadaces(); Columns.Add(pvColPadaces);
      pvColFuncao = new ColumnFuncao(); Columns.Add(pvColFuncao);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_padaces_funcao");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @padaces, string @funcao) {
      Row row = NewRow();

      row.Padaces = @padaces;
      row.Funcao = @funcao;

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
      get { return "LY_PADACES_FUNCAO"; }
    }

    protected override sealed string Name { get { return "Padrão de acesso para as funções"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColPadaces, pvColFuncao });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_padaces_funcao_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_padaces_funcao_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_padaces_funcao_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_padaces_funcao_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkPadaces", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "PADACES", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkFuncao", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "FUNCAO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("padaces", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PADACES", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("funcao", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "FUNCAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "funcao") { result.Add(toLower ? "funcao" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "padaces", "funcao" }
                          : new string[] { "PADACES", "FUNCAO" });
        else
          result.AddRange(toLower
                          ? new string[] { "padaces", "funcao" }
                          : new string[] { "PADACES", "FUNCAO" });

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
        "ly_padaces_funcao.padaces", "ly_padaces_funcao.funcao"
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
          "FROM ly_padaces_funcao " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_padaces_funcao");
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
        da.TableMappings.Add("Table", "Ly_padaces_funcao");
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
            if (dr["FUNCAO"] != null && string.IsNullOrEmpty(dr["FUNCAO"].ToString()))
              dr["FUNCAO"] = DBNull.Value;
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

            if (dr["FUNCAO"] == null || string.IsNullOrEmpty(dr["FUNCAO"].ToString()))
            {
              return "Favor informar o função.";
            }

            if (dr["FUNCAO"] != null && !string.IsNullOrEmpty(dr["FUNCAO"].ToString()))
            {
              if (dr["FUNCAO"].ToString().Length > 20)
              {
                return "O função deve possuir no máximo 20 caracteres.";
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

    public static Ly_padaces_funcao Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_padaces_funcao Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_padaces_funcao tab = new Ly_padaces_funcao();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_padaces_funcao tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @padaces, string @funcao
      ) {
        return (Row)Add(new object[] {
          @padaces, @funcao
        });
      }

      public Row Find(
        string @padaces, string @funcao
      ) {
        return (Row)Find(new object[] {
          @padaces, @funcao
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
      /// FUNCAO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Funcao { get { return (string)this["funcao"]; } set { this["funcao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @padaces, string @funcao) {
        return Insert(connection,
                      @padaces, @funcao,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @padaces, string @funcao,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_padaces_funcao(),
          new string[] { "padaces", "funcao" },
          new object[] { @padaces, @funcao },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkPadaces, string pkFuncao,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_padaces_funcao(),
          new string[] { "padaces", "funcao" },
          new object[] { pkPadaces, pkFuncao },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkPadaces, string pkFuncao) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_padaces_funcao(),
          new string[] { "padaces", "funcao" },
          new object[] { pkPadaces, pkFuncao }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkPadaces, string pkFuncao) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_padaces_funcao(),
          new string[] { "padaces", "funcao" },
          new object[] { pkPadaces, pkFuncao }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Padaces { get { return (string)row["padaces", DataRowVersion.Original]; } }
        public string Funcao { get { return (string)row["funcao", DataRowVersion.Original]; } }

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
      public override string FullCol { get { return "ly_padaces_funcao.padaces"; } }
      public override string OriginalCol { get { return "padaces"; } }
      public override string RefCol { get { return "LY_PADACES_FUNCAO.PADACES"; } }
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

    #region Coluna: FUNCAO
    private ColumnFuncao pvColFuncao;

    public sealed class ColumnFuncao : TDataColumn {

      public ColumnFuncao() {
        this.ColumnName = "funcao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "função";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_padaces_funcao.funcao"; } }
      public override string OriginalCol { get { return "funcao"; } }
      public override string RefCol { get { return "LY_PADACES_FUNCAO.FUNCAO"; } }
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

  public partial class Ly_padaces_licenca : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_padaces_licenca() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_padaces_licenca");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_padaces_licenca");
      }
      catch{}
    }

    public Ly_padaces_licenca() {
      pvColPadaces = new ColumnPadaces(); Columns.Add(pvColPadaces);
      pvColMotivo = new ColumnMotivo(); Columns.Add(pvColMotivo);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_padaces_licenca");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @padaces, string @motivo) {
      Row row = NewRow();

      row.Padaces = @padaces;
      row.Motivo = @motivo;

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
      get { return "LY_PADACES_LICENCA"; }
    }

    protected override sealed string Name { get { return "Padrão de acesso para as licenças"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColPadaces, pvColMotivo });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_padaces_licenca_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_padaces_licenca_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_padaces_licenca_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_padaces_licenca_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkPadaces", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "PADACES", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkMotivo", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "MOTIVO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("padaces", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PADACES", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("motivo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "MOTIVO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
          if(lower == "motivo") { result.Add(toLower ? "motivo" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "padaces", "motivo" }
                          : new string[] { "PADACES", "MOTIVO" });
        else
          result.AddRange(toLower
                          ? new string[] { "padaces", "motivo" }
                          : new string[] { "PADACES", "MOTIVO" });

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
        "ly_padaces_licenca.padaces", "ly_padaces_licenca.motivo"
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
          "FROM ly_padaces_licenca " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_padaces_licenca");
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
        da.TableMappings.Add("Table", "Ly_padaces_licenca");
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
            if (dr["MOTIVO"] != null && string.IsNullOrEmpty(dr["MOTIVO"].ToString()))
              dr["MOTIVO"] = DBNull.Value;
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

            if (dr["MOTIVO"] == null || string.IsNullOrEmpty(dr["MOTIVO"].ToString()))
            {
              return "Favor informar a licença.";
            }

            if (dr["MOTIVO"] != null && !string.IsNullOrEmpty(dr["MOTIVO"].ToString()))
            {
              if (dr["MOTIVO"].ToString().Length > 100)
              {
                return "A licença deve possuir no máximo 100 caracteres.";
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

    public static Ly_padaces_licenca Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_padaces_licenca Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_padaces_licenca tab = new Ly_padaces_licenca();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_padaces_licenca tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @padaces, string @motivo
      ) {
        return (Row)Add(new object[] {
          @padaces, @motivo
        });
      }

      public Row Find(
        string @padaces, string @motivo
      ) {
        return (Row)Find(new object[] {
          @padaces, @motivo
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
      /// MOTIVO: VARCHAR(100), Primary Key, Not Null
      /// </summary>
      public string Motivo { get { return (string)this["motivo"]; } set { this["motivo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @padaces, string @motivo) {
        return Insert(connection,
                      @padaces, @motivo,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @padaces, string @motivo,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_padaces_licenca(),
          new string[] { "padaces", "motivo" },
          new object[] { @padaces, @motivo },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkPadaces, string pkMotivo,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_padaces_licenca(),
          new string[] { "padaces", "motivo" },
          new object[] { pkPadaces, pkMotivo },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkPadaces, string pkMotivo) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_padaces_licenca(),
          new string[] { "padaces", "motivo" },
          new object[] { pkPadaces, pkMotivo }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkPadaces, string pkMotivo) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_padaces_licenca(),
          new string[] { "padaces", "motivo" },
          new object[] { pkPadaces, pkMotivo }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Padaces { get { return (string)row["padaces", DataRowVersion.Original]; } }
        public string Motivo { get { return (string)row["motivo", DataRowVersion.Original]; } }

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
      public override string FullCol { get { return "ly_padaces_licenca.padaces"; } }
      public override string OriginalCol { get { return "padaces"; } }
      public override string RefCol { get { return "LY_PADACES_LICENCA.PADACES"; } }
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

    #region Coluna: MOTIVO
    private ColumnMotivo pvColMotivo;

    public sealed class ColumnMotivo : TDataColumn {

      public ColumnMotivo() {
        this.ColumnName = "motivo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "licença";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_padaces_licenca.motivo"; } }
      public override string OriginalCol { get { return "motivo"; } }
      public override string RefCol { get { return "LY_PADACES_LICENCA.MOTIVO"; } }
      public override bool PrimaryKey { get { return true; } }
      public override bool IsAux { get { return false; } }
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

  public partial class Ly_padaces_turma : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_padaces_turma() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_padaces_turma");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_padaces_turma");
      }
      catch{}
    }

    public Ly_padaces_turma() {
      pvColCurso = new ColumnCurso(); Columns.Add(pvColCurso);
      pvColPadaces = new ColumnPadaces(); Columns.Add(pvColPadaces);
      pvColOperacao = new ColumnOperacao(); Columns.Add(pvColOperacao);
      pvColDt_inicio = new ColumnDt_inicio(); Columns.Add(pvColDt_inicio);
      pvColDt_fim = new ColumnDt_fim(); Columns.Add(pvColDt_fim);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_padaces_turma");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @curso, string @padaces, string @operacao, DateTime? @dt_inicio, 
                      DateTime? @dt_fim) {
      Row row = NewRow();

      row.Curso = @curso;
      row.Padaces = @padaces;
      row.Operacao = @operacao;
      row.Dt_inicio = @dt_inicio;
      row.Dt_fim = @dt_fim;

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
      get { return "LY_PADACES_TURMA"; }
    }

    protected override sealed string Name { get { return "Padrão de acesso para as turmas"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColCurso, pvColPadaces, pvColOperacao });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_padaces_turma_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_padaces_turma_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_padaces_turma_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_padaces_turma_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkCurso", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "CURSO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkPadaces", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "PADACES", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkOperacao", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "OPERACAO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("curso", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CURSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("padaces", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PADACES", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("operacao", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "OPERACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("dt_inicio", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_INICIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_fim", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_FIM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));

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
          if(lower == "curso") { result.Add(toLower ? "curso" : column); continue; }
          if(lower == "padaces") { result.Add(toLower ? "padaces" : column); continue; }
          if(lower == "operacao") { result.Add(toLower ? "operacao" : column); continue; }
          if(lower == "dt_inicio") { result.Add(toLower ? "dt_inicio" : column); continue; }
          if(lower == "dt_fim") { result.Add(toLower ? "dt_fim" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "curso", "padaces", "operacao", "dt_inicio", "dt_fim" }
                          : new string[] { "CURSO", "PADACES", "OPERACAO", "DT_INICIO", "DT_FIM" });
        else
          result.AddRange(toLower
                          ? new string[] { "curso", "padaces", "operacao", "dt_inicio", "dt_fim" }
                          : new string[] { "CURSO", "PADACES", "OPERACAO", "DT_INICIO", "DT_FIM" });

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
        "ly_padaces_turma.curso", "ly_padaces_turma.padaces", "ly_padaces_turma.operacao"
      });
      if(scColumns.Contains("dt_inicio")) list.Add("ly_padaces_turma.dt_inicio");
      if(scColumns.Contains("dt_fim")) list.Add("ly_padaces_turma.dt_fim");
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
          "FROM ly_padaces_turma " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_padaces_turma");
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
        da.TableMappings.Add("Table", "Ly_padaces_turma");
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
            if (dr["CURSO"] != null && string.IsNullOrEmpty(dr["CURSO"].ToString()))
              dr["CURSO"] = DBNull.Value;
            if (dr["PADACES"] != null && string.IsNullOrEmpty(dr["PADACES"].ToString()))
              dr["PADACES"] = DBNull.Value;
            if (dr["OPERACAO"] != null && string.IsNullOrEmpty(dr["OPERACAO"].ToString()))
              dr["OPERACAO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["CURSO"] == null || string.IsNullOrEmpty(dr["CURSO"].ToString()))
            {
              return "Favor informar o curso.";
            }

            if (dr["CURSO"] != null && !string.IsNullOrEmpty(dr["CURSO"].ToString()))
            {
              if (dr["CURSO"].ToString().Length > 20)
              {
                return "O curso deve possuir no máximo 20 caracteres.";
              }
            }

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

            if (dr["OPERACAO"] == null || string.IsNullOrEmpty(dr["OPERACAO"].ToString()))
            {
              return "Favor informar a operação.";
            }

            if (dr["OPERACAO"] != null && !string.IsNullOrEmpty(dr["OPERACAO"].ToString()))
            {
              if (dr["OPERACAO"].ToString().Length > 20)
              {
                return "A operação deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["DT_INICIO"] == null || string.IsNullOrEmpty(dr["DT_INICIO"].ToString()))
            {
              return "Favor informar a data inicial.";
            }

            if (dr["DT_INICIO"] != null && dr["DT_INICIO"].ToString() != "")
            {
              dr["DT_INICIO"] = Convert.ToDateTime(dr["DT_INICIO"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_FIM"] == null || string.IsNullOrEmpty(dr["DT_FIM"].ToString()))
            {
              return "Favor informar a data final.";
            }

            if (dr["DT_FIM"] != null && dr["DT_FIM"].ToString() != "")
            {
              dr["DT_FIM"] = Convert.ToDateTime(dr["DT_FIM"]).ToString("dd/MM/yyyy HH:mm:ss");
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

    public static Ly_padaces_turma Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_padaces_turma Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_padaces_turma tab = new Ly_padaces_turma();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_padaces_turma tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @curso, string @padaces, string @operacao, DateTime? @dt_inicio, DateTime? @dt_fim
      ) {
        return (Row)Add(new object[] {
          @curso, @padaces, @operacao, @dt_inicio, @dt_fim
        });
      }

      public Row Find(
        string @curso, string @padaces, string @operacao
      ) {
        return (Row)Find(new object[] {
          @curso, @padaces, @operacao
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
      /// CURSO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Curso { get { return (string)this["curso"]; } set { this["curso"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// PADACES: VARCHAR(14), Primary Key, Not Null
      /// </summary>
      public string Padaces { get { return (string)this["padaces"]; } set { this["padaces"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// OPERACAO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Operacao { get { return (string)this["operacao"]; } set { this["operacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_INICIO: DATETIME, Not Null
      /// </summary>
      public DateTime? Dt_inicio { get { return (DateTime?)this["dt_inicio"]; } set { this["dt_inicio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_FIM: DATETIME, Not Null
      /// </summary>
      public DateTime? Dt_fim { get { return (DateTime?)this["dt_fim"]; } set { this["dt_fim"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @curso, string @padaces, string @operacao, DateTime? @dt_inicio, DateTime? @dt_fim) {
        return Insert(connection,
                      @curso, @padaces, @operacao, @dt_inicio, @dt_fim,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @curso, string @padaces, string @operacao, DateTime? @dt_inicio, DateTime? @dt_fim,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_padaces_turma(),
          new string[] { "curso", "padaces", "operacao", "dt_inicio", "dt_fim" },
          new object[] { @curso, @padaces, @operacao, @dt_inicio, @dt_fim },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkCurso, string pkPadaces, string pkOperacao,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_padaces_turma(),
          new string[] { "curso", "padaces", "operacao" },
          new object[] { pkCurso, pkPadaces, pkOperacao },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkCurso, string pkPadaces, string pkOperacao) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_padaces_turma(),
          new string[] { "curso", "padaces", "operacao" },
          new object[] { pkCurso, pkPadaces, pkOperacao }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkCurso, string pkPadaces, string pkOperacao) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_padaces_turma(),
          new string[] { "curso", "padaces", "operacao" },
          new object[] { pkCurso, pkPadaces, pkOperacao }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Curso { get { return (string)row["curso", DataRowVersion.Original]; } }
        public string Padaces { get { return (string)row["padaces", DataRowVersion.Original]; } }
        public string Operacao { get { return (string)row["operacao", DataRowVersion.Original]; } }
        public DateTime? Dt_inicio { get { return (DateTime?)row["dt_inicio", DataRowVersion.Original]; } }
        public DateTime? Dt_fim { get { return (DateTime?)row["dt_fim", DataRowVersion.Original]; } }

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
    #region Coluna: CURSO
    private ColumnCurso pvColCurso;

    public sealed class ColumnCurso : TDataColumn {

      public ColumnCurso() {
        this.ColumnName = "curso";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "curso";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_padaces_turma.curso"; } }
      public override string OriginalCol { get { return "curso"; } }
      public override string RefCol { get { return "LY_PADACES_TURMA.CURSO"; } }
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
      public override string FullCol { get { return "ly_padaces_turma.padaces"; } }
      public override string OriginalCol { get { return "padaces"; } }
      public override string RefCol { get { return "LY_PADACES_TURMA.PADACES"; } }
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

    #region Coluna: OPERACAO
    private ColumnOperacao pvColOperacao;

    public sealed class ColumnOperacao : TDataColumn {

      public ColumnOperacao() {
        this.ColumnName = "operacao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "operação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_padaces_turma.operacao"; } }
      public override string OriginalCol { get { return "operacao"; } }
      public override string RefCol { get { return "LY_PADACES_TURMA.OPERACAO"; } }
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

    #region Coluna: DT_INICIO
    private ColumnDt_inicio pvColDt_inicio;

    public sealed class ColumnDt_inicio : TDataColumn {

      public ColumnDt_inicio() {
        this.ColumnName = "dt_inicio";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data inicial";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_padaces_turma.dt_inicio"; } }
      public override string OriginalCol { get { return "dt_inicio"; } }
      public override string RefCol { get { return "LY_PADACES_TURMA.DT_INICIO"; } }
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


      protected override string GetName(){return "data final";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_padaces_turma.dt_fim"; } }
      public override string OriginalCol { get { return "dt_fim"; } }
      public override string RefCol { get { return "LY_PADACES_TURMA.DT_FIM"; } }
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
    #endregion
  }

  public partial class Ly_periodo_cursos : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_periodo_cursos() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_periodo_cursos");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_periodo_cursos");
      }
      catch{}
    }

    public Ly_periodo_cursos() {
      pvColCurso = new ColumnCurso(); Columns.Add(pvColCurso);
      pvColModalidade = new ColumnModalidade(); Columns.Add(pvColModalidade);
      pvColNivel = new ColumnNivel(); Columns.Add(pvColNivel);
      pvColDt_matricula_inicio = new ColumnDt_matricula_inicio(); Columns.Add(pvColDt_matricula_inicio);
      pvColDt_matricula_fim = new ColumnDt_matricula_fim(); Columns.Add(pvColDt_matricula_fim);
      pvColDt_cria_turma_inicio = new ColumnDt_cria_turma_inicio(); Columns.Add(pvColDt_cria_turma_inicio);
      pvColDt_cria_turma_fim = new ColumnDt_cria_turma_fim(); Columns.Add(pvColDt_cria_turma_fim);
      pvColDt_digita_nota_inicio = new ColumnDt_digita_nota_inicio(); Columns.Add(pvColDt_digita_nota_inicio);
      pvColDt_digita_nota_fim = new ColumnDt_digita_nota_fim(); Columns.Add(pvColDt_digita_nota_fim);
      pvColDt_lanc_falta_inicio = new ColumnDt_lanc_falta_inicio(); Columns.Add(pvColDt_lanc_falta_inicio);
      pvColDt_lanc_falta_fim = new ColumnDt_lanc_falta_fim(); Columns.Add(pvColDt_lanc_falta_fim);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_periodo_cursos");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @curso) {
      Row row = NewRow();

      row.Curso = @curso;

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
      get { return "LY_PERIODO_CURSOS"; }
    }

    protected override sealed string Name { get { return "Período de criação das Turmas"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColCurso });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_periodo_cursos_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_periodo_cursos_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_periodo_cursos_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_periodo_cursos_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkCurso", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "CURSO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("curso", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CURSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("modalidade", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "MODALIDADE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("nivel", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NIVEL", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("dt_matricula_inicio", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_MATRICULA_INICIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_matricula_fim", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_MATRICULA_FIM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_cria_turma_inicio", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_CRIA_TURMA_INICIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_cria_turma_fim", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_CRIA_TURMA_FIM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_digita_nota_inicio", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_DIGITA_NOTA_INICIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_digita_nota_fim", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_DIGITA_NOTA_FIM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_lanc_falta_inicio", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_LANC_FALTA_INICIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_lanc_falta_fim", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_LANC_FALTA_FIM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));

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
          if(lower == "curso") { result.Add(toLower ? "curso" : column); continue; }
          if(lower == "modalidade") { result.Add(toLower ? "modalidade" : column); continue; }
          if(lower == "nivel") { result.Add(toLower ? "nivel" : column); continue; }
          if(lower == "dt_matricula_inicio") { result.Add(toLower ? "dt_matricula_inicio" : column); continue; }
          if(lower == "dt_matricula_fim") { result.Add(toLower ? "dt_matricula_fim" : column); continue; }
          if(lower == "dt_cria_turma_inicio") { result.Add(toLower ? "dt_cria_turma_inicio" : column); continue; }
          if(lower == "dt_cria_turma_fim") { result.Add(toLower ? "dt_cria_turma_fim" : column); continue; }
          if(lower == "dt_digita_nota_inicio") { result.Add(toLower ? "dt_digita_nota_inicio" : column); continue; }
          if(lower == "dt_digita_nota_fim") { result.Add(toLower ? "dt_digita_nota_fim" : column); continue; }
          if(lower == "dt_lanc_falta_inicio") { result.Add(toLower ? "dt_lanc_falta_inicio" : column); continue; }
          if(lower == "dt_lanc_falta_fim") { result.Add(toLower ? "dt_lanc_falta_fim" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "curso", "modalidade", "nivel", "dt_matricula_inicio", "dt_matricula_fim", 
                                           "dt_cria_turma_inicio", "dt_cria_turma_fim", "dt_digita_nota_inicio", "dt_digita_nota_fim", 
                                           "dt_lanc_falta_inicio", "dt_lanc_falta_fim" }
                          : new string[] { "CURSO", "MODALIDADE", "NIVEL", "DT_MATRICULA_INICIO", "DT_MATRICULA_FIM", 
                                           "DT_CRIA_TURMA_INICIO", "DT_CRIA_TURMA_FIM", "DT_DIGITA_NOTA_INICIO", "DT_DIGITA_NOTA_FIM", 
                                           "DT_LANC_FALTA_INICIO", "DT_LANC_FALTA_FIM" });
        else
          result.AddRange(toLower
                          ? new string[] { "curso", "modalidade", "nivel", "dt_matricula_inicio", "dt_matricula_fim", 
                                           "dt_cria_turma_inicio", "dt_cria_turma_fim", "dt_digita_nota_inicio", "dt_digita_nota_fim", 
                                           "dt_lanc_falta_inicio", "dt_lanc_falta_fim" }
                          : new string[] { "CURSO", "MODALIDADE", "NIVEL", "DT_MATRICULA_INICIO", "DT_MATRICULA_FIM", 
                                           "DT_CRIA_TURMA_INICIO", "DT_CRIA_TURMA_FIM", "DT_DIGITA_NOTA_INICIO", "DT_DIGITA_NOTA_FIM", 
                                           "DT_LANC_FALTA_INICIO", "DT_LANC_FALTA_FIM" });

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
        "ly_periodo_cursos.curso"
      });
      if(scColumns.Contains("modalidade")) list.Add("ly_periodo_cursos.modalidade");
      if(scColumns.Contains("nivel")) list.Add("ly_periodo_cursos.nivel");
      if(scColumns.Contains("dt_matricula_inicio")) list.Add("ly_periodo_cursos.dt_matricula_inicio");
      if(scColumns.Contains("dt_matricula_fim")) list.Add("ly_periodo_cursos.dt_matricula_fim");
      if(scColumns.Contains("dt_cria_turma_inicio")) list.Add("ly_periodo_cursos.dt_cria_turma_inicio");
      if(scColumns.Contains("dt_cria_turma_fim")) list.Add("ly_periodo_cursos.dt_cria_turma_fim");
      if(scColumns.Contains("dt_digita_nota_inicio")) list.Add("ly_periodo_cursos.dt_digita_nota_inicio");
      if(scColumns.Contains("dt_digita_nota_fim")) list.Add("ly_periodo_cursos.dt_digita_nota_fim");
      if(scColumns.Contains("dt_lanc_falta_inicio")) list.Add("ly_periodo_cursos.dt_lanc_falta_inicio");
      if(scColumns.Contains("dt_lanc_falta_fim")) list.Add("ly_periodo_cursos.dt_lanc_falta_fim");
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
          "FROM ly_periodo_cursos " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_periodo_cursos");
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
        da.TableMappings.Add("Table", "Ly_periodo_cursos");
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
            if (dr["CURSO"] != null && string.IsNullOrEmpty(dr["CURSO"].ToString()))
              dr["CURSO"] = DBNull.Value;
            if (dr["MODALIDADE"] != null && string.IsNullOrEmpty(dr["MODALIDADE"].ToString()))
              dr["MODALIDADE"] = DBNull.Value;
            if (dr["NIVEL"] != null && string.IsNullOrEmpty(dr["NIVEL"].ToString()))
              dr["NIVEL"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

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

            if (dr["MODALIDADE"] != null && !string.IsNullOrEmpty(dr["MODALIDADE"].ToString()))
            {
              if (dr["MODALIDADE"].ToString().Length > 20)
              {
                return "A Modalidade deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["NIVEL"] != null && !string.IsNullOrEmpty(dr["NIVEL"].ToString()))
            {
              if (dr["NIVEL"].ToString().Length > 20)
              {
                return "O Nível deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["DT_MATRICULA_INICIO"] != null && dr["DT_MATRICULA_INICIO"].ToString() != "")
            {
              dr["DT_MATRICULA_INICIO"] = Convert.ToDateTime(dr["DT_MATRICULA_INICIO"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_MATRICULA_FIM"] != null && dr["DT_MATRICULA_FIM"].ToString() != "")
            {
              dr["DT_MATRICULA_FIM"] = Convert.ToDateTime(dr["DT_MATRICULA_FIM"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_CRIA_TURMA_INICIO"] != null && dr["DT_CRIA_TURMA_INICIO"].ToString() != "")
            {
              dr["DT_CRIA_TURMA_INICIO"] = Convert.ToDateTime(dr["DT_CRIA_TURMA_INICIO"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_CRIA_TURMA_FIM"] != null && dr["DT_CRIA_TURMA_FIM"].ToString() != "")
            {
              dr["DT_CRIA_TURMA_FIM"] = Convert.ToDateTime(dr["DT_CRIA_TURMA_FIM"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_DIGITA_NOTA_INICIO"] != null && dr["DT_DIGITA_NOTA_INICIO"].ToString() != "")
            {
              dr["DT_DIGITA_NOTA_INICIO"] = Convert.ToDateTime(dr["DT_DIGITA_NOTA_INICIO"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_DIGITA_NOTA_FIM"] != null && dr["DT_DIGITA_NOTA_FIM"].ToString() != "")
            {
              dr["DT_DIGITA_NOTA_FIM"] = Convert.ToDateTime(dr["DT_DIGITA_NOTA_FIM"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_LANC_FALTA_INICIO"] != null && dr["DT_LANC_FALTA_INICIO"].ToString() != "")
            {
              dr["DT_LANC_FALTA_INICIO"] = Convert.ToDateTime(dr["DT_LANC_FALTA_INICIO"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_LANC_FALTA_FIM"] != null && dr["DT_LANC_FALTA_FIM"].ToString() != "")
            {
              dr["DT_LANC_FALTA_FIM"] = Convert.ToDateTime(dr["DT_LANC_FALTA_FIM"]).ToString("dd/MM/yyyy HH:mm:ss");
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

    public static Ly_periodo_cursos Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_periodo_cursos Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_periodo_cursos tab = new Ly_periodo_cursos();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_periodo_cursos tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @curso, string @modalidade, string @nivel, DateTime? @dt_matricula_inicio, 
        DateTime? @dt_matricula_fim, DateTime? @dt_cria_turma_inicio, DateTime? @dt_cria_turma_fim, 
        DateTime? @dt_digita_nota_inicio, DateTime? @dt_digita_nota_fim, DateTime? @dt_lanc_falta_inicio, 
        DateTime? @dt_lanc_falta_fim
      ) {
        return (Row)Add(new object[] {
          @curso, @modalidade, @nivel, @dt_matricula_inicio, @dt_matricula_fim, @dt_cria_turma_inicio, 
          @dt_cria_turma_fim, @dt_digita_nota_inicio, @dt_digita_nota_fim, @dt_lanc_falta_inicio, 
          @dt_lanc_falta_fim
        });
      }

      public Row Find(
        string @curso
      ) {
        return (Row)Find(new object[] {
          @curso
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
      /// CURSO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Curso { get { return (string)this["curso"]; } set { this["curso"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// MODALIDADE: VARCHAR(20)
      /// </summary>
      public string Modalidade { get { return (string)this["modalidade"]; } set { this["modalidade"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// NIVEL: VARCHAR(20)
      /// </summary>
      public string Nivel { get { return (string)this["nivel"]; } set { this["nivel"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_MATRICULA_INICIO: DATETIME
      /// </summary>
      public DateTime? Dt_matricula_inicio { get { return (DateTime?)this["dt_matricula_inicio"]; } set { this["dt_matricula_inicio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_MATRICULA_FIM: DATETIME
      /// </summary>
      public DateTime? Dt_matricula_fim { get { return (DateTime?)this["dt_matricula_fim"]; } set { this["dt_matricula_fim"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_CRIA_TURMA_INICIO: DATETIME
      /// </summary>
      public DateTime? Dt_cria_turma_inicio { get { return (DateTime?)this["dt_cria_turma_inicio"]; } set { this["dt_cria_turma_inicio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_CRIA_TURMA_FIM: DATETIME
      /// </summary>
      public DateTime? Dt_cria_turma_fim { get { return (DateTime?)this["dt_cria_turma_fim"]; } set { this["dt_cria_turma_fim"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_DIGITA_NOTA_INICIO: DATETIME
      /// </summary>
      public DateTime? Dt_digita_nota_inicio { get { return (DateTime?)this["dt_digita_nota_inicio"]; } set { this["dt_digita_nota_inicio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_DIGITA_NOTA_FIM: DATETIME
      /// </summary>
      public DateTime? Dt_digita_nota_fim { get { return (DateTime?)this["dt_digita_nota_fim"]; } set { this["dt_digita_nota_fim"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_LANC_FALTA_INICIO: DATETIME
      /// </summary>
      public DateTime? Dt_lanc_falta_inicio { get { return (DateTime?)this["dt_lanc_falta_inicio"]; } set { this["dt_lanc_falta_inicio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_LANC_FALTA_FIM: DATETIME
      /// </summary>
      public DateTime? Dt_lanc_falta_fim { get { return (DateTime?)this["dt_lanc_falta_fim"]; } set { this["dt_lanc_falta_fim"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @curso) {
        return Insert(connection,
                      @curso,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @curso,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_periodo_cursos(),
          new string[] { "curso" },
          new object[] { @curso },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkCurso,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_periodo_cursos(),
          new string[] { "curso" },
          new object[] { pkCurso },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkCurso) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_periodo_cursos(),
          new string[] { "curso" },
          new object[] { pkCurso }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkCurso) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_periodo_cursos(),
          new string[] { "curso" },
          new object[] { pkCurso }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Curso { get { return (string)row["curso", DataRowVersion.Original]; } }
        public string Modalidade { get { return (string)row["modalidade", DataRowVersion.Original]; } }
        public string Nivel { get { return (string)row["nivel", DataRowVersion.Original]; } }
        public DateTime? Dt_matricula_inicio { get { return (DateTime?)row["dt_matricula_inicio", DataRowVersion.Original]; } }
        public DateTime? Dt_matricula_fim { get { return (DateTime?)row["dt_matricula_fim", DataRowVersion.Original]; } }
        public DateTime? Dt_cria_turma_inicio { get { return (DateTime?)row["dt_cria_turma_inicio", DataRowVersion.Original]; } }
        public DateTime? Dt_cria_turma_fim { get { return (DateTime?)row["dt_cria_turma_fim", DataRowVersion.Original]; } }
        public DateTime? Dt_digita_nota_inicio { get { return (DateTime?)row["dt_digita_nota_inicio", DataRowVersion.Original]; } }
        public DateTime? Dt_digita_nota_fim { get { return (DateTime?)row["dt_digita_nota_fim", DataRowVersion.Original]; } }
        public DateTime? Dt_lanc_falta_inicio { get { return (DateTime?)row["dt_lanc_falta_inicio", DataRowVersion.Original]; } }
        public DateTime? Dt_lanc_falta_fim { get { return (DateTime?)row["dt_lanc_falta_fim", DataRowVersion.Original]; } }

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
    #region Coluna: CURSO
    private ColumnCurso pvColCurso;

    public sealed class ColumnCurso : TDataColumn {

      public ColumnCurso() {
        this.ColumnName = "curso";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Curso";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_periodo_cursos.curso"; } }
      public override string OriginalCol { get { return "curso"; } }
      public override string RefCol { get { return "LY_PERIODO_CURSOS.CURSO"; } }
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

    #region Coluna: MODALIDADE
    private ColumnModalidade pvColModalidade;

    public sealed class ColumnModalidade : TDataColumn {

      public ColumnModalidade() {
        this.ColumnName = "modalidade";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Modalidade";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_periodo_cursos.modalidade"; } }
      public override string OriginalCol { get { return "modalidade"; } }
      public override string RefCol { get { return "LY_PERIODO_CURSOS.MODALIDADE"; } }
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

    #region Coluna: NIVEL
    private ColumnNivel pvColNivel;

    public sealed class ColumnNivel : TDataColumn {

      public ColumnNivel() {
        this.ColumnName = "nivel";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Nível";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_periodo_cursos.nivel"; } }
      public override string OriginalCol { get { return "nivel"; } }
      public override string RefCol { get { return "LY_PERIODO_CURSOS.NIVEL"; } }
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

    #region Coluna: DT_MATRICULA_INICIO
    private ColumnDt_matricula_inicio pvColDt_matricula_inicio;

    public sealed class ColumnDt_matricula_inicio : TDataColumn {

      public ColumnDt_matricula_inicio() {
        this.ColumnName = "dt_matricula_inicio";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "Data inicial da matrícula";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_periodo_cursos.dt_matricula_inicio"; } }
      public override string OriginalCol { get { return "dt_matricula_inicio"; } }
      public override string RefCol { get { return "LY_PERIODO_CURSOS.DT_MATRICULA_INICIO"; } }
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

    #region Coluna: DT_MATRICULA_FIM
    private ColumnDt_matricula_fim pvColDt_matricula_fim;

    public sealed class ColumnDt_matricula_fim : TDataColumn {

      public ColumnDt_matricula_fim() {
        this.ColumnName = "dt_matricula_fim";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "Data final da matrícula";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_periodo_cursos.dt_matricula_fim"; } }
      public override string OriginalCol { get { return "dt_matricula_fim"; } }
      public override string RefCol { get { return "LY_PERIODO_CURSOS.DT_MATRICULA_FIM"; } }
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

    #region Coluna: DT_CRIA_TURMA_INICIO
    private ColumnDt_cria_turma_inicio pvColDt_cria_turma_inicio;

    public sealed class ColumnDt_cria_turma_inicio : TDataColumn {

      public ColumnDt_cria_turma_inicio() {
        this.ColumnName = "dt_cria_turma_inicio";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "Data inicial de criação das turmas";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_periodo_cursos.dt_cria_turma_inicio"; } }
      public override string OriginalCol { get { return "dt_cria_turma_inicio"; } }
      public override string RefCol { get { return "LY_PERIODO_CURSOS.DT_CRIA_TURMA_INICIO"; } }
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

    #region Coluna: DT_CRIA_TURMA_FIM
    private ColumnDt_cria_turma_fim pvColDt_cria_turma_fim;

    public sealed class ColumnDt_cria_turma_fim : TDataColumn {

      public ColumnDt_cria_turma_fim() {
        this.ColumnName = "dt_cria_turma_fim";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "Data final de criação das turmas";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_periodo_cursos.dt_cria_turma_fim"; } }
      public override string OriginalCol { get { return "dt_cria_turma_fim"; } }
      public override string RefCol { get { return "LY_PERIODO_CURSOS.DT_CRIA_TURMA_FIM"; } }
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

    #region Coluna: DT_DIGITA_NOTA_INICIO
    private ColumnDt_digita_nota_inicio pvColDt_digita_nota_inicio;

    public sealed class ColumnDt_digita_nota_inicio : TDataColumn {

      public ColumnDt_digita_nota_inicio() {
        this.ColumnName = "dt_digita_nota_inicio";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "Data inicial de digitação das notas";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_periodo_cursos.dt_digita_nota_inicio"; } }
      public override string OriginalCol { get { return "dt_digita_nota_inicio"; } }
      public override string RefCol { get { return "LY_PERIODO_CURSOS.DT_DIGITA_NOTA_INICIO"; } }
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

    #region Coluna: DT_DIGITA_NOTA_FIM
    private ColumnDt_digita_nota_fim pvColDt_digita_nota_fim;

    public sealed class ColumnDt_digita_nota_fim : TDataColumn {

      public ColumnDt_digita_nota_fim() {
        this.ColumnName = "dt_digita_nota_fim";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "Data final de digitação das notas";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_periodo_cursos.dt_digita_nota_fim"; } }
      public override string OriginalCol { get { return "dt_digita_nota_fim"; } }
      public override string RefCol { get { return "LY_PERIODO_CURSOS.DT_DIGITA_NOTA_FIM"; } }
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

    #region Coluna: DT_LANC_FALTA_INICIO
    private ColumnDt_lanc_falta_inicio pvColDt_lanc_falta_inicio;

    public sealed class ColumnDt_lanc_falta_inicio : TDataColumn {

      public ColumnDt_lanc_falta_inicio() {
        this.ColumnName = "dt_lanc_falta_inicio";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "Data inicial de lançamento de faltas";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_periodo_cursos.dt_lanc_falta_inicio"; } }
      public override string OriginalCol { get { return "dt_lanc_falta_inicio"; } }
      public override string RefCol { get { return "LY_PERIODO_CURSOS.DT_LANC_FALTA_INICIO"; } }
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

    #region Coluna: DT_LANC_FALTA_FIM
    private ColumnDt_lanc_falta_fim pvColDt_lanc_falta_fim;

    public sealed class ColumnDt_lanc_falta_fim : TDataColumn {

      public ColumnDt_lanc_falta_fim() {
        this.ColumnName = "dt_lanc_falta_fim";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "Data final de lançamento de faltas";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_periodo_cursos.dt_lanc_falta_fim"; } }
      public override string OriginalCol { get { return "dt_lanc_falta_fim"; } }
      public override string RefCol { get { return "LY_PERIODO_CURSOS.DT_LANC_FALTA_FIM"; } }
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

  public partial class Ly_serie : TDataTable
  {
      private static ConstructorInfo ctorTCustom = null;
      private static ConstructorInfo ctorCustom = null;
      private CustomBase tcustom = null;
      private CustomBase custom = null;

      static Ly_serie()
      {
          try
          {
              ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_serie");
              ctorCustom = CRCustomLib.GetCustomConstructor("Ly_serie");
          }
          catch { }
      }

      public Ly_serie()
      {
          pvColCurso = new ColumnCurso(); Columns.Add(pvColCurso);
          pvColTurno = new ColumnTurno(); Columns.Add(pvColTurno);
          pvColCurriculo = new ColumnCurriculo(); Columns.Add(pvColCurriculo);
          pvColSerie = new ColumnSerie(); Columns.Add(pvColSerie);
          pvColDescricao = new ColumnDescricao(); Columns.Add(pvColDescricao);
          pvColServico = new ColumnServico(); Columns.Add(pvColServico);
          pvColServico_cred = new ColumnServico_cred(); Columns.Add(pvColServico_cred);
          pvColAtlz_max_discip = new ColumnAtlz_max_discip(); Columns.Add(pvColAtlz_max_discip);
          pvColComplemento1 = new ColumnComplemento1(); Columns.Add(pvColComplemento1);
          pvColComplemento2 = new ColumnComplemento2(); Columns.Add(pvColComplemento2);
          pvColIdade_minima = new ColumnIdade_minima(); Columns.Add(pvColIdade_minima);
          pvColDia_aniv = new ColumnDia_aniv(); Columns.Add(pvColDia_aniv);
          pvColMes_aniv = new ColumnMes_aniv(); Columns.Add(pvColMes_aniv);
          pvColConsolidada = new ColumnConsolidada(); Columns.Add(pvColConsolidada);
          pvColStamp_atualizacao = new ColumnStamp_atualizacao(); Columns.Add(pvColStamp_atualizacao);
          pvColDt_extincao = new ColumnDt_extincao(); Columns.Add(pvColDt_extincao);
          pvColCurso_seguinte = new ColumnCurso_seguinte(); Columns.Add(pvColCurso_seguinte);
          pvColSerie_seguinte = new ColumnSerie_seguinte(); Columns.Add(pvColSerie_seguinte);
          pvColAno_serie_concluinte = new ColumnAno_serie_concluinte(); Columns.Add(pvColAno_serie_concluinte);
          pvColMatricula = new ColumnMatricula(); Columns.Add(pvColMatricula);
          pvColDt_cadastro = new ColumnDt_cadastro(); Columns.Add(pvColDt_cadastro);
          pvColEmite_certificacao = new ColumnEmite_certificacao(); Columns.Add(pvColEmite_certificacao);

          InitRows(new RowCollection(BaseRows));
          SetPrimaryKey();
          SetTableName("ly_serie");

          if (ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
          if (ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
      }


      public new Row NewRow()
      {
          return (Row)base.NewRow();
      }

      public Row NewRow(string @curso, string @turno, string @curriculo, decimal? @serie,
                        string @descricao)
      {
          Row row = NewRow();

          row.Curso = @curso;
          row.Turno = @turno;
          row.Curriculo = @curriculo;
          row.Serie = @serie;
          row.Descricao = @descricao;

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
          get { return "LY_SERIE"; }
      }

      protected override sealed string Name { get { return "Série"; } }

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
          SetPrimaryKey(new DataColumn[] { pvColCurso, pvColTurno, pvColCurriculo, pvColSerie });
      }


      private static TCommand CreateCommand(TConnection cn, CommandOperation operation)
      {
          bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
          TCommand cm = CreateDataTableCommand(cn, operation);
          cm.CommandType = CommandType.StoredProcedure;
          cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

          switch (operation)
          {
              case CommandOperation.Select: cm.CommandText = "zLy_serie_GetCurrentValues"; break;
              case CommandOperation.Insert: cm.CommandText = "zLy_serie_Cmd"; break;
              case CommandOperation.Update: cm.CommandText = "zLy_serie_Cmd"; break;
              case CommandOperation.Delete: cm.CommandText = "zLy_serie_Cmd"; break;
              default: throw new InvalidOperationException();
          }


          if (operation == CommandOperation.Select)
          {
              cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
              cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
          }

          cm.Parameters.Add(new TParameter("pkCurso", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "CURSO", DataRowVersion.Original, DBNull.Value));
          cm.Parameters.Add(new TParameter("pkTurno", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "TURNO", DataRowVersion.Original, DBNull.Value));
          cm.Parameters.Add(new TParameter("pkCurriculo", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "CURRICULO", DataRowVersion.Original, DBNull.Value));
          cm.Parameters.Add(new TParameter("pkSerie", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "SERIE", DataRowVersion.Original, DBNull.Value));

          cm.Parameters.Add(new TParameter("curso", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CURSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("turno", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TURNO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("curriculo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CURRICULO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("serie", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "SERIE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
          cm.Parameters.Add(new TParameter("descricao", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "DESCRICAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("servico", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SERVICO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("servico_cred", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SERVICO_CRED", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("atlz_max_discip", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ATLZ_MAX_DISCIP", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
          cm.Parameters.Add(new TParameter("complemento1", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "COMPLEMENTO1", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("complemento2", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "COMPLEMENTO2", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("idade_minima", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "IDADE_MINIMA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
          cm.Parameters.Add(new TParameter("dia_aniv", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "DIA_ANIV", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
          cm.Parameters.Add(new TParameter("mes_aniv", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "MES_ANIV", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
          cm.Parameters.Add(new TParameter("consolidada", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CONSOLIDADA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
          cm.Parameters.Add(new TParameter("stamp_atualizacao", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "STAMP_ATUALIZACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
          cm.Parameters.Add(new TParameter("dt_extincao", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_EXTINCAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
          cm.Parameters.Add(new TParameter("curso_seguinte", OleDbType.VarChar, 20, ParameterDirection.InputOutput, true, 0, 0, "CURSO_SEGUINTE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("serie_seguinte", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "SERIE_SEGUINTE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
          cm.Parameters.Add(new TParameter("ano_serie_concluinte", OleDbType.Char, 1, ParameterDirection.InputOutput, true, 0, 0, "ANO_SERIE_CONCLUINTE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
          cm.Parameters.Add(new TParameter("matricula", OleDbType.VarChar, 8, ParameterDirection.InputOutput, true, 0, 0, "MATRICULA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
          cm.Parameters.Add(new TParameter("dt_cadastro", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_CADASTRO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
          cm.Parameters.Add(new TParameter("emite_certificacao", OleDbType.Char, 1, ParameterDirection.InputOutput, true, 0, 0, "EMITE_CERTIFICACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));


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
                  if (lower == "curso") { result.Add(toLower ? "curso" : column); continue; }
                  if (lower == "turno") { result.Add(toLower ? "turno" : column); continue; }
                  if (lower == "curriculo") { result.Add(toLower ? "curriculo" : column); continue; }
                  if (lower == "serie") { result.Add(toLower ? "serie" : column); continue; }
                  if (lower == "descricao") { result.Add(toLower ? "descricao" : column); continue; }
                  if (lower == "servico") { result.Add(toLower ? "servico" : column); continue; }
                  if (lower == "servico_cred") { result.Add(toLower ? "servico_cred" : column); continue; }
                  if (lower == "atlz_max_discip") { result.Add(toLower ? "atlz_max_discip" : column); continue; }
                  if (lower == "complemento1") { result.Add(toLower ? "complemento1" : column); continue; }
                  if (lower == "complemento2") { result.Add(toLower ? "complemento2" : column); continue; }
                  if (lower == "idade_minima") { result.Add(toLower ? "idade_minima" : column); continue; }
                  if (lower == "dia_aniv") { result.Add(toLower ? "dia_aniv" : column); continue; }
                  if (lower == "mes_aniv") { result.Add(toLower ? "mes_aniv" : column); continue; }
                  if (lower == "consolidada") { result.Add(toLower ? "consolidada" : column); continue; }
                  if (lower == "stamp_atualizacao") { result.Add(toLower ? "stamp_atualizacao" : column); continue; }
                  if (lower == "dt_extincao") { result.Add(toLower ? "dt_extincao" : column); continue; }
                  if (lower == "curso_seguinte") { result.Add(toLower ? "curso_seguinte" : column); continue; }
                  if (lower == "serie_seguinte") { result.Add(toLower ? "serie_seguinte" : column); continue; }
                  if (lower == "ano_serie_concluinte") { result.Add(toLower ? "ano_serie_concluinte" : column); continue; }
                  if (lower == "matricula") { result.Add(toLower ? "matricula" : column); continue; }
                  if (lower == "dt_cadastro") { result.Add(toLower ? "dt_cadastro" : column); continue; }
                  if (lower == "emite_certificacao") { result.Add(toLower ? "emite_certificacao" : column); continue; }

                  if (!tabPrincipal)
                  {
                  }
              }

          if (result.Count == 0)
              if (tabPrincipal)
                  result.AddRange(toLower
                                  ? new string[] { "curso", "turno", "curriculo", "serie", "descricao", "servico", 
                                           "servico_cred", "atlz_max_discip", "complemento1", "complemento2", "idade_minima", 
                                           "dia_aniv", "mes_aniv", "consolidada", "stamp_atualizacao", "dt_extincao",
                                           "curso_seguinte", "serie_seguinte", "ano_serie_concluinte", "matricula", "dt_cadastro" }
                                  : new string[] { "CURSO", "TURNO", "CURRICULO", "SERIE", "DESCRICAO", "SERVICO", 
                                           "SERVICO_CRED", "ATLZ_MAX_DISCIP", "COMPLEMENTO1", "COMPLEMENTO2", "IDADE_MINIMA", 
                                           "DIA_ANIV", "MES_ANIV", "CONSOLIDADA", "STAMP_ATUALIZACAO", "DT_EXTINCAO",
                                           "CURSO_SEGUINTE", "SERIE_SEGUINTE", "ANO_SERIE_CONCLUINTE", "MATRICULA", "DT_CADASTRO","EMITE_CERTIFICACAO" });
              else
                  result.AddRange(toLower
                                  ? new string[] { "curso", "turno", "curriculo", "serie", "descricao", "servico", 
                                           "servico_cred", "atlz_max_discip", "complemento1", "complemento2", "idade_minima", 
                                           "dia_aniv", "mes_aniv", "consolidada", "stamp_atualizacao", "dt_extincao",
                                           "curso_seguinte", "serie_seguinte", "ano_serie_concluinte", "matricula", "dt_cadastro" }
                                  : new string[] { "CURSO", "TURNO", "CURRICULO", "SERIE", "DESCRICAO", "SERVICO", 
                                           "SERVICO_CRED", "ATLZ_MAX_DISCIP", "COMPLEMENTO1", "COMPLEMENTO2", "IDADE_MINIMA", 
                                           "DIA_ANIV", "MES_ANIV", "CONSOLIDADA", "STAMP_ATUALIZACAO", "DT_EXTINCAO",
                                           "CURSO_SEGUINTE", "SERIE_SEGUINTE", "ANO_SERIE_CONCLUINTE", "MATRICULA", "DT_CADASTRO","EMITE_CERTIFICACAO" });

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
        "ly_serie.curso", "ly_serie.turno", "ly_serie.curriculo", "ly_serie.serie"
      });
          if (scColumns.Contains("descricao")) list.Add("ly_serie.descricao");
          if (scColumns.Contains("servico")) list.Add("ly_serie.servico");
          if (scColumns.Contains("servico_cred")) list.Add("ly_serie.servico_cred");
          if (scColumns.Contains("atlz_max_discip")) list.Add("ly_serie.atlz_max_discip");
          if (scColumns.Contains("complemento1")) list.Add("ly_serie.complemento1");
          if (scColumns.Contains("complemento2")) list.Add("ly_serie.complemento2");
          if (scColumns.Contains("idade_minima")) list.Add("ly_serie.idade_minima");
          if (scColumns.Contains("dia_aniv")) list.Add("ly_serie.dia_aniv");
          if (scColumns.Contains("mes_aniv")) list.Add("ly_serie.mes_aniv");
          if (scColumns.Contains("consolidada")) list.Add("ly_serie.consolidada");
          if (scColumns.Contains("stamp_atualizacao")) list.Add("ly_serie.stamp_atualizacao");
          if (scColumns.Contains("dt_extincao")) list.Add("ly_serie.dt_extincao");
          if (scColumns.Contains("curso_seguinte")) list.Add("ly_serie.curso_seguinte");
          if (scColumns.Contains("serie_seguinte")) list.Add("ly_serie.serie_seguinte");
          if (scColumns.Contains("ano_serie_concluinte")) list.Add("ly_serie.ano_serie_concluinte");
          if (scColumns.Contains("matricula")) list.Add("ly_serie.matricula");
          if (scColumns.Contains("dt_cadastro")) list.Add("ly_serie.dt_cadastro");
          if (scColumns.Contains("emite_certificacao")) list.Add("ly_serie.emite_certificacao");
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
              "FROM ly_serie " +
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
                      count = da.Fill(ds, startRecord, maxRecords, "ly_serie");
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
              da.TableMappings.Add("Table", "Ly_serie");
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
              if (dr["CURSO"] != null && string.IsNullOrEmpty(dr["CURSO"].ToString()))
                  dr["CURSO"] = DBNull.Value;
              if (dr["TURNO"] != null && string.IsNullOrEmpty(dr["TURNO"].ToString()))
                  dr["TURNO"] = DBNull.Value;
              if (dr["CURRICULO"] != null && string.IsNullOrEmpty(dr["CURRICULO"].ToString()))
                  dr["CURRICULO"] = DBNull.Value;
              if (dr["SERIE"] != null && string.IsNullOrEmpty(dr["SERIE"].ToString()))
                  dr["SERIE"] = DBNull.Value;
              if (dr["DESCRICAO"] != null && string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
                  dr["DESCRICAO"] = DBNull.Value;
              if (dr["SERVICO"] != null && string.IsNullOrEmpty(dr["SERVICO"].ToString()))
                  dr["SERVICO"] = DBNull.Value;
              if (dr["SERVICO_CRED"] != null && string.IsNullOrEmpty(dr["SERVICO_CRED"].ToString()))
                  dr["SERVICO_CRED"] = DBNull.Value;
              if (dr["ATLZ_MAX_DISCIP"] != null && string.IsNullOrEmpty(dr["ATLZ_MAX_DISCIP"].ToString()))
                  dr["ATLZ_MAX_DISCIP"] = DBNull.Value;
              if (dr["COMPLEMENTO1"] != null && string.IsNullOrEmpty(dr["COMPLEMENTO1"].ToString()))
                  dr["COMPLEMENTO1"] = DBNull.Value;
              if (dr["COMPLEMENTO2"] != null && string.IsNullOrEmpty(dr["COMPLEMENTO2"].ToString()))
                  dr["COMPLEMENTO2"] = DBNull.Value;
              if (dr["IDADE_MINIMA"] != null && string.IsNullOrEmpty(dr["IDADE_MINIMA"].ToString()))
                  dr["IDADE_MINIMA"] = DBNull.Value;
              if (dr["DIA_ANIV"] != null && string.IsNullOrEmpty(dr["DIA_ANIV"].ToString()))
                  dr["DIA_ANIV"] = DBNull.Value;
              if (dr["MES_ANIV"] != null && string.IsNullOrEmpty(dr["MES_ANIV"].ToString()))
                  dr["MES_ANIV"] = DBNull.Value;
              if (dr["CONSOLIDADA"] != null && string.IsNullOrEmpty(dr["CONSOLIDADA"].ToString()))
                  dr["CONSOLIDADA"] = DBNull.Value;
              if (dr["CURSO_SEGUINTE"] != null && string.IsNullOrEmpty(dr["CURSO_SEGUINTE"].ToString()))
                  dr["CURSO_SEGUINTE"] = DBNull.Value;
              if (dr["ANO_SERIE_CONCLUINTE"] != null && string.IsNullOrEmpty(dr["ANO_SERIE_CONCLUINTE"].ToString()))
                  dr["ANO_SERIE_CONCLUINTE"] = DBNull.Value;
              if (dr["MATRICULA"] != null && string.IsNullOrEmpty(dr["MATRICULA"].ToString()))
                  dr["MATRICULA"] = DBNull.Value;
              if (dr["EMITE_CERTIFICACAO"] != null && string.IsNullOrEmpty(dr["EMITE_CERTIFICACAO"].ToString()))
                  dr["EMITE_CERTIFICACAO"] = DBNull.Value;
          }
      }

      public string ValidaCampos(DataRow dr)
      {
          if (dr != null)
          {

              if (dr["CURSO"] == null || string.IsNullOrEmpty(dr["CURSO"].ToString()))
              {
                  return "Favor informar o curso.";
              }

              if (dr["CURSO"] != null && !string.IsNullOrEmpty(dr["CURSO"].ToString()))
              {
                  if (dr["CURSO"].ToString().Length > 20)
                  {
                      return "O curso deve possuir no máximo 20 caracteres.";
                  }
              }

              if (dr["TURNO"] == null || string.IsNullOrEmpty(dr["TURNO"].ToString()))
              {
                  return "Favor informar o turno.";
              }

              if (dr["TURNO"] != null && !string.IsNullOrEmpty(dr["TURNO"].ToString()))
              {
                  if (dr["TURNO"].ToString().Length > 20)
                  {
                      return "O turno deve possuir no máximo 20 caracteres.";
                  }
              }

              if (dr["CURRICULO"] == null || string.IsNullOrEmpty(dr["CURRICULO"].ToString()))
              {
                  return "Favor informar o currículo.";
              }

              if (dr["CURRICULO"] != null && !string.IsNullOrEmpty(dr["CURRICULO"].ToString()))
              {
                  if (dr["CURRICULO"].ToString().Length > 20)
                  {
                      return "O currículo deve possuir no máximo 20 caracteres.";
                  }
              }

              if (dr["SERIE"] == null || string.IsNullOrEmpty(dr["SERIE"].ToString()))
              {
                  return "Favor informar a série.";
              }

              if (dr["SERIE"] != null && !string.IsNullOrEmpty(dr["SERIE"].ToString()))
              {
                  if ((Convert.ToDecimal(dr["SERIE"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["SERIE"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["SERIE"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["SERIE"]).ToString("0.############").Split(',')[1].Length > 0))
                  {
                      return "A série deve ser um valor inteiro com 3 dígitos no máximo.";
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

              if (dr["SERVICO"] != null && !string.IsNullOrEmpty(dr["SERVICO"].ToString()))
              {
                  if (dr["SERVICO"].ToString().Length > 20)
                  {
                      return "O Servico deve possuir no máximo 20 caracteres.";
                  }
              }

              if (dr["SERVICO_CRED"] != null && !string.IsNullOrEmpty(dr["SERVICO_CRED"].ToString()))
              {
                  if (dr["SERVICO_CRED"].ToString().Length > 20)
                  {
                      return "O Servico_cred deve possuir no máximo 20 caracteres.";
                  }
              }

              if (dr["ATLZ_MAX_DISCIP"] != null && !string.IsNullOrEmpty(dr["ATLZ_MAX_DISCIP"].ToString()))
              {
                  if ((Convert.ToDecimal(dr["ATLZ_MAX_DISCIP"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ATLZ_MAX_DISCIP"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["ATLZ_MAX_DISCIP"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ATLZ_MAX_DISCIP"]).ToString("0.############").Split(',')[1].Length > 0))
                  {
                      return "O Atlz_max_discip deve ser um valor inteiro com 3 dígitos no máximo.";
                  }
              }

              if (dr["COMPLEMENTO1"] != null && !string.IsNullOrEmpty(dr["COMPLEMENTO1"].ToString()))
              {
                  if (dr["COMPLEMENTO1"].ToString().Length > 50)
                  {
                      return "O Complemento1 deve possuir no máximo 50 caracteres.";
                  }
              }

              if (dr["COMPLEMENTO2"] != null && !string.IsNullOrEmpty(dr["COMPLEMENTO2"].ToString()))
              {
                  if (dr["COMPLEMENTO2"].ToString().Length > 50)
                  {
                      return "O Complemento2 deve possuir no máximo 50 caracteres.";
                  }
              }

              if (dr["IDADE_MINIMA"] != null && !string.IsNullOrEmpty(dr["IDADE_MINIMA"].ToString()))
              {
                  if ((Convert.ToDecimal(dr["IDADE_MINIMA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["IDADE_MINIMA"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["IDADE_MINIMA"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["IDADE_MINIMA"]).ToString("0.############").Split(',')[1].Length > 0))
                  {
                      return "A idade mínima deve ser um valor inteiro com 10 dígitos no máximo.";
                  }
              }

              if (dr["DIA_ANIV"] != null && !string.IsNullOrEmpty(dr["DIA_ANIV"].ToString()))
              {
                  if ((Convert.ToDecimal(dr["DIA_ANIV"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["DIA_ANIV"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["DIA_ANIV"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["DIA_ANIV"]).ToString("0.############").Split(',')[1].Length > 0))
                  {
                      return "O dia do aniversário deve ser um valor inteiro com 10 dígitos no máximo.";
                  }
              }

              if (dr["MES_ANIV"] != null && !string.IsNullOrEmpty(dr["MES_ANIV"].ToString()))
              {
                  if ((Convert.ToDecimal(dr["MES_ANIV"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["MES_ANIV"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["MES_ANIV"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["MES_ANIV"]).ToString("0.############").Split(',')[1].Length > 0))
                  {
                      return "O mês de aniversário deve ser um valor inteiro com 10 dígitos no máximo.";
                  }
              }

              if (dr["CONSOLIDADA"] != null && !string.IsNullOrEmpty(dr["CONSOLIDADA"].ToString()))
              {
                  if (dr["CONSOLIDADA"].ToString().Length > 1)
                  {
                      return "O Consolidada deve possuir no máximo 1 caractere.";
                  }
              }

              if (dr["STAMP_ATUALIZACAO"] != null && dr["STAMP_ATUALIZACAO"].ToString() != "")
              {
                  dr["STAMP_ATUALIZACAO"] = Convert.ToDateTime(dr["STAMP_ATUALIZACAO"]).ToString("dd/MM/yyyy HH:mm:ss");
              }

              if (dr["DT_EXTINCAO"] != null && dr["DT_EXTINCAO"].ToString() != "")
              {
                  dr["DT_EXTINCAO"] = Convert.ToDateTime(dr["DT_EXTINCAO"]).ToString("dd/MM/yyyy HH:mm:ss");
              }

              if (dr["CURSO_SEGUINTE"] != null && !string.IsNullOrEmpty(dr["CURSO_SEGUINTE"].ToString()))
              {
                  if (dr["CURSO_SEGUINTE"].ToString().Length > 20)
                  {
                      return "O curso seguinte deve possuir no máximo 20 caracteres.";
                  }
              }

              if (dr["ANO_SERIE_CONCLUINTE"] != null && !string.IsNullOrEmpty(dr["ANO_SERIE_CONCLUINTE"].ToString()))
              {
                  if (dr["ANO_SERIE_CONCLUINTE"].ToString().Length > 1)
                  {
                      return "A ano série concluinte deve possuir no máximo 1 caractere.";
                  }
              }

              if (dr["MATRICULA"] != null && !string.IsNullOrEmpty(dr["MATRICULA"].ToString()))
              {
                  if (dr["MATRICULA"].ToString().Length > 8)
                  {
                      return "A matrícula deve possuir no máximo 8 caracteres.";
                  }
              }

              if (dr["EMITE_CERTIFICACAO"] != null && !string.IsNullOrEmpty(dr["EMITE_CERTIFICACAO"].ToString()))
              {
                  if (dr["EMITE_CERTIFICACAO"].ToString().Length > 1)
                  {
                      return "Emite certificação deve possuir no máximo 1 caracter.";
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

      public static Ly_serie Query(TConnection connection, string where, params object[] whereValues)
      {
          return Query(connection, false, where, whereValues);
      }

      public static Ly_serie Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues)
      {
          Ly_serie tab = new Ly_serie();
          tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
          return tab;
      }

      public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues)
      {
          Ly_serie tab = Query(connection, false, where, whereValues);
          return tab.Rows.Count > 0 ? tab.Rows[0] : null;
      }


      public sealed class RowCollection : TDataRowCollection
      {
          internal RowCollection(DataRowCollection rows)
              : base(rows)
          {
          }


          public Row Add(
            string @curso, string @turno, string @curriculo, decimal? @serie, string @descricao,
            string @servico, string @servico_cred, decimal? @atlz_max_discip, string @complemento1,
            string @complemento2, decimal? @idade_minima, decimal? @dia_aniv, decimal? @mes_aniv,
            string @consolidada, DateTime? @stamp_atualizacao, DateTime? @dt_extincao,
            string @curso_seguinte, decimal? @serie_seguinte, string @ano_serie_concluinte,
            string @matricula, DateTime? @dt_cadastro, string @emite_certificacao
          )
          {
              return (Row)Add(new object[] {
          @curso, @turno, @curriculo, @serie, @descricao, @servico, @servico_cred, @atlz_max_discip, 
          @complemento1, @complemento2, @idade_minima, @dia_aniv, @mes_aniv, @consolidada, 
          @stamp_atualizacao, @dt_extincao, @curso_seguinte, @serie_seguinte, @ano_serie_concluinte,
          @matricula, @dt_cadastro,@emite_certificacao
        });
          }

          public Row Find(
            string @curso, string @turno, string @curriculo, decimal? @serie
          )
          {
              return (Row)Find(new object[] {
          @curso, @turno, @curriculo, @serie
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
          /// CURSO: VARCHAR(20), Primary Key, Not Null
          /// </summary>
          public string Curso { get { return (string)this["curso"]; } set { this["curso"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// TURNO: VARCHAR(20), Primary Key, Not Null
          /// </summary>
          public string Turno { get { return (string)this["turno"]; } set { this["turno"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// CURRICULO: VARCHAR(20), Primary Key, Not Null
          /// </summary>
          public string Curriculo { get { return (string)this["curriculo"]; } set { this["curriculo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// SERIE: NUMERIC(3), Primary Key, Not Null
          /// </summary>
          public decimal? Serie { get { return (decimal?)this["serie"]; } set { this["serie"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// DESCRICAO: VARCHAR(100), Not Null
          /// </summary>
          public string Descricao { get { return (string)this["descricao"]; } set { this["descricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// SERVICO: VARCHAR(20)
          /// </summary>
          public string Servico { get { return (string)this["servico"]; } set { this["servico"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// SERVICO_CRED: VARCHAR(20)
          /// </summary>
          public string Servico_cred { get { return (string)this["servico_cred"]; } set { this["servico_cred"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// ATLZ_MAX_DISCIP: NUMERIC(3)
          /// </summary>
          public decimal? Atlz_max_discip { get { return (decimal?)this["atlz_max_discip"]; } set { this["atlz_max_discip"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// COMPLEMENTO1: VARCHAR(50)
          /// </summary>
          public string Complemento1 { get { return (string)this["complemento1"]; } set { this["complemento1"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// COMPLEMENTO2: VARCHAR(50)
          /// </summary>
          public string Complemento2 { get { return (string)this["complemento2"]; } set { this["complemento2"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// IDADE_MINIMA: NUMERIC(10)
          /// </summary>
          public decimal? Idade_minima { get { return (decimal?)this["idade_minima"]; } set { this["idade_minima"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// DIA_ANIV: NUMERIC(10)
          /// </summary>
          public decimal? Dia_aniv { get { return (decimal?)this["dia_aniv"]; } set { this["dia_aniv"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// MES_ANIV: NUMERIC(10)
          /// </summary>
          public decimal? Mes_aniv { get { return (decimal?)this["mes_aniv"]; } set { this["mes_aniv"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// CONSOLIDADA: CHAR(1)
          /// </summary>
          public string Consolidada { get { return (string)this["consolidada"]; } set { this["consolidada"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// STAMP_ATUALIZACAO: DATETIME
          /// </summary>
          public DateTime? Stamp_atualizacao { get { return (DateTime?)this["stamp_atualizacao"]; } set { this["stamp_atualizacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// DT_EXTINCAO: DATETIME
          /// </summary>
          public DateTime? Dt_extincao { get { return (DateTime?)this["dt_extincao"]; } set { this["dt_extincao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// CURSO_SEGUINTE: VARCHAR(20)
          /// </summary>
          public string Curso_seguinte { get { return (string)this["curso_seguinte"]; } set { this["curso_seguinte"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// SERIE_SEGUINTE: NUMERIC(3, 0)
          /// </summary>
          public decimal? Serie_seguinte { get { return (decimal?)this["serie_seguinte"]; } set { this["serie_seguinte"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// ANO_SERIE_CONCLUINTE: CHAR(1)
          /// </summary>
          public string Ano_serie_concluinte { get { return (string)this["ano_serie_concluinte"]; } set { this["ano_serie_concluinte"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// MATRICULA: VARCHAR(8)
          /// </summary>
          public string Matricula { get { return (string)this["matricula"]; } set { this["matricula"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// DT_CADASTRO: DATETIME
          /// </summary>
          public DateTime? Dt_cadastro { get { return (DateTime?)this["dt_cadastro"]; } set { this["dt_cadastro"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
          /// <summary>
          /// EMITE_CERTIFICACAO: CHAR(1)
          /// </summary>
          public string Emite_certificacao { get { return (string)this["emite_certificacao"]; } set { this["emite_certificacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


          public OriginalValues Original { get { return original; } }




          public static Row Insert(TConnectionWritable connection,
                                   string @curso, string @turno, string @curriculo, decimal? @serie, string @descricao)
          {
              return Insert(connection,
                            @curso, @turno, @curriculo, @serie, @descricao,
                            null);
          }

          public static Row Insert(TConnectionWritable connection,
                                   string @curso, string @turno, string @curriculo, decimal? @serie, string @descricao,
                                   string columnList, params object[] values)
          {
              return (Row)Insert(
                connection, new Techne.Lyceum.CR.Ly_serie(),
                new string[] { "curso", "turno", "curriculo", "serie", "descricao" },
                new object[] { @curso, @turno, @curriculo, @serie, @descricao },
                columnList, values
              );
          }

          public static Row Update(TConnectionWritable connection,
                                   string pkCurso, string pkTurno, string pkCurriculo, decimal? pkSerie,
                                   string columnList, params object[] values)
          {
              return (Row)Update(
                connection, new Techne.Lyceum.CR.Ly_serie(),
                new string[] { "curso", "turno", "curriculo", "serie" },
                new object[] { pkCurso, pkTurno, pkCurriculo, pkSerie },
                columnList, values
              );
          }

          public static bool Delete(TConnectionWritable connection,
                                    string pkCurso, string pkTurno, string pkCurriculo, decimal? pkSerie)
          {
              return Delete(
                connection, new Techne.Lyceum.CR.Ly_serie(),
                new string[] { "curso", "turno", "curriculo", "serie" },
                new object[] { pkCurso, pkTurno, pkCurriculo, pkSerie }
              );
          }


          /// <summary>
          /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
          /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
          /// Dá exception se mais de um registro for encontrado.
          /// </summary>
          /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
          public static Row Query(TConnection connection,
                                  string pkCurso, string pkTurno, string pkCurriculo, decimal? pkSerie)
          {
              return (Row)Query(
                connection, new Techne.Lyceum.CR.Ly_serie(),
                new string[] { "curso", "turno", "curriculo", "serie" },
                new object[] { pkCurso, pkTurno, pkCurriculo, pkSerie }
              );
          }


          public struct OriginalValues
          {
              private Row row;

              internal OriginalValues(Row row)
              {
                  this.row = row;
              }


              public string Curso { get { return (string)row["curso", DataRowVersion.Original]; } }
              public string Turno { get { return (string)row["turno", DataRowVersion.Original]; } }
              public string Curriculo { get { return (string)row["curriculo", DataRowVersion.Original]; } }
              public decimal? Serie { get { return (decimal?)row["serie", DataRowVersion.Original]; } }
              public string Descricao { get { return (string)row["descricao", DataRowVersion.Original]; } }
              public string Servico { get { return (string)row["servico", DataRowVersion.Original]; } }
              public string Servico_cred { get { return (string)row["servico_cred", DataRowVersion.Original]; } }
              public decimal? Atlz_max_discip { get { return (decimal?)row["atlz_max_discip", DataRowVersion.Original]; } }
              public string Complemento1 { get { return (string)row["complemento1", DataRowVersion.Original]; } }
              public string Complemento2 { get { return (string)row["complemento2", DataRowVersion.Original]; } }
              public decimal? Idade_minima { get { return (decimal?)row["idade_minima", DataRowVersion.Original]; } }
              public decimal? Dia_aniv { get { return (decimal?)row["dia_aniv", DataRowVersion.Original]; } }
              public decimal? Mes_aniv { get { return (decimal?)row["mes_aniv", DataRowVersion.Original]; } }
              public string Consolidada { get { return (string)row["consolidada", DataRowVersion.Original]; } }
              public DateTime? Stamp_atualizacao { get { return (DateTime?)row["stamp_atualizacao", DataRowVersion.Original]; } }
              public DateTime? Dt_extincao { get { return (DateTime?)row["dt_extincao", DataRowVersion.Original]; } }
              public string Curso_seguinte { get { return (string)row["curso_seguinte", DataRowVersion.Original]; } }
              public decimal? Serie_seguinte { get { return (decimal?)row["serie_seguinte", DataRowVersion.Original]; } }
              public string Ano_serie_concluinte { get { return (string)row["ano_serie_concluinte", DataRowVersion.Original]; } }
              public string Matricula { get { return (string)row["matricula", DataRowVersion.Original]; } }
              public DateTime? Dt_cadastro { get { return (DateTime?)row["dt_cadastro", DataRowVersion.Original]; } }
              public string Emite_certificacao { get { return (string)row["emite_certificacao", DataRowVersion.Original]; } }


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
      #region Coluna: CURSO
      private ColumnCurso pvColCurso;

      public sealed class ColumnCurso : TDataColumn
      {

          public ColumnCurso()
          {
              this.ColumnName = "curso";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "curso"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_serie.curso"; } }
          public override string OriginalCol { get { return "curso"; } }
          public override string RefCol { get { return "LY_SERIE.CURSO"; } }
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

      #region Coluna: TURNO
      private ColumnTurno pvColTurno;

      public sealed class ColumnTurno : TDataColumn
      {

          public ColumnTurno()
          {
              this.ColumnName = "turno";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "turno"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_serie.turno"; } }
          public override string OriginalCol { get { return "turno"; } }
          public override string RefCol { get { return "LY_SERIE.TURNO"; } }
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

      #region Coluna: CURRICULO
      private ColumnCurriculo pvColCurriculo;

      public sealed class ColumnCurriculo : TDataColumn
      {

          public ColumnCurriculo()
          {
              this.ColumnName = "curriculo";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "currículo"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_serie.curriculo"; } }
          public override string OriginalCol { get { return "curriculo"; } }
          public override string RefCol { get { return "LY_SERIE.CURRICULO"; } }
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

      #region Coluna: SERIE
      private ColumnSerie pvColSerie;

      public sealed class ColumnSerie : TDataColumn
      {

          public ColumnSerie()
          {
              this.ColumnName = "serie";
              this.DataType = typeof(decimal);

          }


          protected override string GetName() { return "série"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_serie.serie"; } }
          public override string OriginalCol { get { return "serie"; } }
          public override string RefCol { get { return "LY_SERIE.SERIE"; } }
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

      #region Coluna: DESCRICAO
      private ColumnDescricao pvColDescricao;

      public sealed class ColumnDescricao : TDataColumn
      {

          public ColumnDescricao()
          {
              this.ColumnName = "descricao";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return "descrição"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_serie.descricao"; } }
          public override string OriginalCol { get { return "descricao"; } }
          public override string RefCol { get { return "LY_SERIE.DESCRICAO"; } }
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

      public sealed class ColumnServico : TDataColumn
      {

          public ColumnServico()
          {
              this.ColumnName = "servico";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return ""; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_serie.servico"; } }
          public override string OriginalCol { get { return "servico"; } }
          public override string RefCol { get { return "LY_SERIE.SERVICO"; } }
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

      #region Coluna: SERVICO_CRED
      private ColumnServico_cred pvColServico_cred;

      public sealed class ColumnServico_cred : TDataColumn
      {

          public ColumnServico_cred()
          {
              this.ColumnName = "servico_cred";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return ""; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_serie.servico_cred"; } }
          public override string OriginalCol { get { return "servico_cred"; } }
          public override string RefCol { get { return "LY_SERIE.SERVICO_CRED"; } }
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

      #region Coluna: ATLZ_MAX_DISCIP
      private ColumnAtlz_max_discip pvColAtlz_max_discip;

      public sealed class ColumnAtlz_max_discip : TDataColumn
      {

          public ColumnAtlz_max_discip()
          {
              this.ColumnName = "atlz_max_discip";
              this.DataType = typeof(decimal);

          }


          protected override string GetName() { return ""; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_serie.atlz_max_discip"; } }
          public override string OriginalCol { get { return "atlz_max_discip"; } }
          public override string RefCol { get { return "LY_SERIE.ATLZ_MAX_DISCIP"; } }
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

      #region Coluna: COMPLEMENTO1
      private ColumnComplemento1 pvColComplemento1;

      public sealed class ColumnComplemento1 : TDataColumn
      {

          public ColumnComplemento1()
          {
              this.ColumnName = "complemento1";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return ""; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_serie.complemento1"; } }
          public override string OriginalCol { get { return "complemento1"; } }
          public override string RefCol { get { return "LY_SERIE.COMPLEMENTO1"; } }
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

      #region Coluna: COMPLEMENTO2
      private ColumnComplemento2 pvColComplemento2;

      public sealed class ColumnComplemento2 : TDataColumn
      {

          public ColumnComplemento2()
          {
              this.ColumnName = "complemento2";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return ""; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_serie.complemento2"; } }
          public override string OriginalCol { get { return "complemento2"; } }
          public override string RefCol { get { return "LY_SERIE.COMPLEMENTO2"; } }
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

      #region Coluna: IDADE_MINIMA
      private ColumnIdade_minima pvColIdade_minima;

      public sealed class ColumnIdade_minima : TDataColumn
      {

          public ColumnIdade_minima()
          {
              this.ColumnName = "idade_minima";
              this.DataType = typeof(decimal);

          }


          protected override string GetName() { return "idade mínima"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_serie.idade_minima"; } }
          public override string OriginalCol { get { return "idade_minima"; } }
          public override string RefCol { get { return "LY_SERIE.IDADE_MINIMA"; } }
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

      #region Coluna: DIA_ANIV
      private ColumnDia_aniv pvColDia_aniv;

      public sealed class ColumnDia_aniv : TDataColumn
      {

          public ColumnDia_aniv()
          {
              this.ColumnName = "dia_aniv";
              this.DataType = typeof(decimal);

          }


          protected override string GetName() { return "dia do aniversário"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_serie.dia_aniv"; } }
          public override string OriginalCol { get { return "dia_aniv"; } }
          public override string RefCol { get { return "LY_SERIE.DIA_ANIV"; } }
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

      #region Coluna: MES_ANIV
      private ColumnMes_aniv pvColMes_aniv;

      public sealed class ColumnMes_aniv : TDataColumn
      {

          public ColumnMes_aniv()
          {
              this.ColumnName = "mes_aniv";
              this.DataType = typeof(decimal);

          }


          protected override string GetName() { return "mês de aniversário"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_serie.mes_aniv"; } }
          public override string OriginalCol { get { return "mes_aniv"; } }
          public override string RefCol { get { return "LY_SERIE.MES_ANIV"; } }
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

      #region Coluna: CONSOLIDADA
      private ColumnConsolidada pvColConsolidada;

      public sealed class ColumnConsolidada : TDataColumn
      {

          public ColumnConsolidada()
          {
              this.ColumnName = "consolidada";
              this.DataType = typeof(string);
              this.DefaultValue = "N";

          }


          protected override string GetName() { return ""; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_serie.consolidada"; } }
          public override string OriginalCol { get { return "consolidada"; } }
          public override string RefCol { get { return "LY_SERIE.CONSOLIDADA"; } }
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

      #region Coluna: STAMP_ATUALIZACAO
      private ColumnStamp_atualizacao pvColStamp_atualizacao;

      public sealed class ColumnStamp_atualizacao : TDataColumn
      {

          public ColumnStamp_atualizacao()
          {
              this.ColumnName = "stamp_atualizacao";
              this.DataType = typeof(DateTime);

          }


          protected override string GetName() { return ""; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_serie.stamp_atualizacao"; } }
          public override string OriginalCol { get { return "stamp_atualizacao"; } }
          public override string RefCol { get { return "LY_SERIE.STAMP_ATUALIZACAO"; } }
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

      #region Coluna: DT_EXTINCAO
      private ColumnDt_extincao pvColDt_extincao;

      public sealed class ColumnDt_extincao : TDataColumn
      {

          public ColumnDt_extincao()
          {
              this.ColumnName = "dt_extincao";
              this.DataType = typeof(DateTime);

          }


          protected override string GetName() { return "data de extinção"; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_serie.dt_extincao"; } }
          public override string OriginalCol { get { return "dt_extincao"; } }
          public override string RefCol { get { return "LY_SERIE.DT_EXTINCAO"; } }
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

      #region Coluna: CURSO_SEGUINTE
      private ColumnCurso_seguinte pvColCurso_seguinte;

      public sealed class ColumnCurso_seguinte : TDataColumn
      {

          public ColumnCurso_seguinte()
          {
              this.ColumnName = "curso_seguinte";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return ""; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_serie.curso_seguinte"; } }
          public override string OriginalCol { get { return "curso_seguinte"; } }
          public override string RefCol { get { return "LY_SERIE.CURSO_SEGUINTE"; } }
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

      #region Coluna: SERIE_SEGUINTE
      private ColumnSerie_seguinte pvColSerie_seguinte;

      public sealed class ColumnSerie_seguinte : TDataColumn
      {

          public ColumnSerie_seguinte()
          {
              this.ColumnName = "serie_seguinte";
              this.DataType = typeof(decimal);

          }


          protected override string GetName() { return ""; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_serie.serie_seguinte"; } }
          public override string OriginalCol { get { return "serie_seguinte"; } }
          public override string RefCol { get { return "LY_SERIE.SERIE_SEGUINTE"; } }
          public override bool PrimaryKey { get { return false; } }
          public override bool IsAux { get { return false; } }
          /// <summary>
          /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
          /// </summary>
          public override bool NotNull { get { return false; } }
          public override string SqlServerType { get { return "NUMERIC(3, 0)"; } }

      }
      #endregion

      #region Coluna: ANO_SERIE_CONCLUINTE
      private ColumnAno_serie_concluinte pvColAno_serie_concluinte;

      public sealed class ColumnAno_serie_concluinte : TDataColumn
      {

          public ColumnAno_serie_concluinte()
          {
              this.ColumnName = "ano_serie_concluinte";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return ""; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_serie.ano_serie_concluinte"; } }
          public override string OriginalCol { get { return "ano_serie_concluinte"; } }
          public override string RefCol { get { return "LY_SERIE.ANO_SERIE_CONCLUINTE"; } }
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

      #region Coluna: MATRICULA
      private ColumnMatricula pvColMatricula;

      public sealed class ColumnMatricula : TDataColumn
      {

          public ColumnMatricula()
          {
              this.ColumnName = "matricula";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return ""; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_serie.matricula"; } }
          public override string OriginalCol { get { return "matricula"; } }
          public override string RefCol { get { return "LY_SERIE.MATRICULA"; } }
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

      public sealed class ColumnDt_cadastro : TDataColumn
      {

          public ColumnDt_cadastro()
          {
              this.ColumnName = "dt_cadastro";
              this.DataType = typeof(DateTime);

          }


          protected override string GetName() { return ""; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_serie.dt_cadastro"; } }
          public override string OriginalCol { get { return "dt_cadastro"; } }
          public override string RefCol { get { return "LY_SERIE.DT_CADASTRO"; } }
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

      #region Coluna: EMITE_CERTIFICACAO
      private ColumnEmite_certificacao pvColEmite_certificacao;

      public sealed class ColumnEmite_certificacao : TDataColumn
      {

          public ColumnEmite_certificacao()
          {
              this.ColumnName = "emite_certificacao";
              this.DataType = typeof(string);

          }


          protected override string GetName() { return ""; }
          protected override string GetCaption() { return ""; }
          protected override string GetDescricao() { return ""; }
          public override string FullCol { get { return "ly_serie.emite_certificacao"; } }
          public override string OriginalCol { get { return "emite_certificacao"; } }
          public override string RefCol { get { return "LY_SERIE.EMITE_CERTIFICACAO"; } }
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

  public partial class Ly_serie_sufixo : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_serie_sufixo() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_serie_sufixo");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_serie_sufixo");
      }
      catch{}
    }

    public Ly_serie_sufixo() {
      pvColCurso = new ColumnCurso(); Columns.Add(pvColCurso);
      pvColTurno = new ColumnTurno(); Columns.Add(pvColTurno);
      pvColCurriculo = new ColumnCurriculo(); Columns.Add(pvColCurriculo);
      pvColSerie = new ColumnSerie(); Columns.Add(pvColSerie);
      pvColSufixo = new ColumnSufixo(); Columns.Add(pvColSufixo);
      pvColDescricao = new ColumnDescricao(); Columns.Add(pvColDescricao);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_serie_sufixo");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @curso, string @turno, string @curriculo, decimal? @serie, 
                      string @sufixo) {
      Row row = NewRow();

      row.Curso = @curso;
      row.Turno = @turno;
      row.Curriculo = @curriculo;
      row.Serie = @serie;
      row.Sufixo = @sufixo;

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
      get { return "LY_SERIE_SUFIXO"; }
    }

    protected override sealed string Name { get { return "Sufixo da série"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColCurso, pvColTurno, pvColCurriculo, pvColSerie, 
                                       pvColSufixo });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_serie_sufixo_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_serie_sufixo_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_serie_sufixo_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_serie_sufixo_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkCurso", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "CURSO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkTurno", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "TURNO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkCurriculo", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "CURRICULO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkSerie", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "SERIE", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkSufixo", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "SUFIXO", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("curso", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CURSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("turno", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TURNO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("curriculo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CURRICULO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("serie", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "SERIE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("sufixo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SUFIXO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
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
          if(lower == "curso") { result.Add(toLower ? "curso" : column); continue; }
          if(lower == "turno") { result.Add(toLower ? "turno" : column); continue; }
          if(lower == "curriculo") { result.Add(toLower ? "curriculo" : column); continue; }
          if(lower == "serie") { result.Add(toLower ? "serie" : column); continue; }
          if(lower == "sufixo") { result.Add(toLower ? "sufixo" : column); continue; }
          if(lower == "descricao") { result.Add(toLower ? "descricao" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "curso", "turno", "curriculo", "serie", "sufixo", "descricao" }
                          : new string[] { "CURSO", "TURNO", "CURRICULO", "SERIE", "SUFIXO", "DESCRICAO" });
        else
          result.AddRange(toLower
                          ? new string[] { "curso", "turno", "curriculo", "serie", "sufixo", "descricao" }
                          : new string[] { "CURSO", "TURNO", "CURRICULO", "SERIE", "SUFIXO", "DESCRICAO" });

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
        "ly_serie_sufixo.curso", "ly_serie_sufixo.turno", "ly_serie_sufixo.curriculo", "ly_serie_sufixo.serie", 
        "ly_serie_sufixo.sufixo"
      });
      if(scColumns.Contains("descricao")) list.Add("ly_serie_sufixo.descricao");
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
          "FROM ly_serie_sufixo " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_serie_sufixo");
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
        da.TableMappings.Add("Table", "Ly_serie_sufixo");
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
            if (dr["CURSO"] != null && string.IsNullOrEmpty(dr["CURSO"].ToString()))
              dr["CURSO"] = DBNull.Value;
            if (dr["TURNO"] != null && string.IsNullOrEmpty(dr["TURNO"].ToString()))
              dr["TURNO"] = DBNull.Value;
            if (dr["CURRICULO"] != null && string.IsNullOrEmpty(dr["CURRICULO"].ToString()))
              dr["CURRICULO"] = DBNull.Value;
            if (dr["SERIE"] != null && string.IsNullOrEmpty(dr["SERIE"].ToString()))
              dr["SERIE"] = DBNull.Value;
            if (dr["SUFIXO"] != null && string.IsNullOrEmpty(dr["SUFIXO"].ToString()))
              dr["SUFIXO"] = DBNull.Value;
            if (dr["DESCRICAO"] != null && string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
              dr["DESCRICAO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["CURSO"] == null || string.IsNullOrEmpty(dr["CURSO"].ToString()))
            {
              return "Favor informar o curso.";
            }

            if (dr["CURSO"] != null && !string.IsNullOrEmpty(dr["CURSO"].ToString()))
            {
              if (dr["CURSO"].ToString().Length > 20)
              {
                return "O curso deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["TURNO"] == null || string.IsNullOrEmpty(dr["TURNO"].ToString()))
            {
              return "Favor informar o turno.";
            }

            if (dr["TURNO"] != null && !string.IsNullOrEmpty(dr["TURNO"].ToString()))
            {
              if (dr["TURNO"].ToString().Length > 20)
              {
                return "O turno deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["CURRICULO"] == null || string.IsNullOrEmpty(dr["CURRICULO"].ToString()))
            {
              return "Favor informar o currículo.";
            }

            if (dr["CURRICULO"] != null && !string.IsNullOrEmpty(dr["CURRICULO"].ToString()))
            {
              if (dr["CURRICULO"].ToString().Length > 20)
              {
                return "O currículo deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["SERIE"] == null || string.IsNullOrEmpty(dr["SERIE"].ToString()))
            {
              return "Favor informar a série.";
            }

            if (dr["SERIE"] != null && !string.IsNullOrEmpty(dr["SERIE"].ToString()))
            {
              if ((Convert.ToDecimal(dr["SERIE"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["SERIE"]).ToString("0.############").Split(',')[0].Length > 3) || (Convert.ToDecimal(dr["SERIE"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["SERIE"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A série deve ser um valor inteiro com 3 dígitos no máximo.";
              }
            }

            if (dr["SUFIXO"] == null || string.IsNullOrEmpty(dr["SUFIXO"].ToString()))
            {
              return "Favor informar o sufixo.";
            }

            if (dr["SUFIXO"] != null && !string.IsNullOrEmpty(dr["SUFIXO"].ToString()))
            {
              if (dr["SUFIXO"].ToString().Length > 20)
              {
                return "O sufixo deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["DESCRICAO"] != null && !string.IsNullOrEmpty(dr["DESCRICAO"].ToString()))
            {
              if (dr["DESCRICAO"].ToString().Length > 100)
              {
                return "A descrição deve possuir no máximo 100 caracteres.";
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

    public static Ly_serie_sufixo Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_serie_sufixo Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_serie_sufixo tab = new Ly_serie_sufixo();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_serie_sufixo tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @curso, string @turno, string @curriculo, decimal? @serie, string @sufixo, 
        string @descricao
      ) {
        return (Row)Add(new object[] {
          @curso, @turno, @curriculo, @serie, @sufixo, @descricao
        });
      }

      public Row Find(
        string @curso, string @turno, string @curriculo, decimal? @serie, string @sufixo
      ) {
        return (Row)Find(new object[] {
          @curso, @turno, @curriculo, @serie, @sufixo
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
      /// CURSO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Curso { get { return (string)this["curso"]; } set { this["curso"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TURNO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Turno { get { return (string)this["turno"]; } set { this["turno"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CURRICULO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Curriculo { get { return (string)this["curriculo"]; } set { this["curriculo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SERIE: NUMERIC(3), Primary Key, Not Null
      /// </summary>
      public decimal? Serie { get { return (decimal?)this["serie"]; } set { this["serie"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// SUFIXO: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Sufixo { get { return (string)this["sufixo"]; } set { this["sufixo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DESCRICAO: VARCHAR(100)
      /// </summary>
      public string Descricao { get { return (string)this["descricao"]; } set { this["descricao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @curso, string @turno, string @curriculo, decimal? @serie, string @sufixo) {
        return Insert(connection,
                      @curso, @turno, @curriculo, @serie, @sufixo,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @curso, string @turno, string @curriculo, decimal? @serie, string @sufixo,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_serie_sufixo(),
          new string[] { "curso", "turno", "curriculo", "serie", "sufixo" },
          new object[] { @curso, @turno, @curriculo, @serie, @sufixo },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkCurso, string pkTurno, string pkCurriculo, decimal? pkSerie, string pkSufixo,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_serie_sufixo(),
          new string[] { "curso", "turno", "curriculo", "serie", "sufixo" },
          new object[] { pkCurso, pkTurno, pkCurriculo, pkSerie, pkSufixo },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkCurso, string pkTurno, string pkCurriculo, decimal? pkSerie, string pkSufixo) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_serie_sufixo(),
          new string[] { "curso", "turno", "curriculo", "serie", "sufixo" },
          new object[] { pkCurso, pkTurno, pkCurriculo, pkSerie, pkSufixo }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkCurso, string pkTurno, string pkCurriculo, decimal? pkSerie, string pkSufixo) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_serie_sufixo(),
          new string[] { "curso", "turno", "curriculo", "serie", "sufixo" },
          new object[] { pkCurso, pkTurno, pkCurriculo, pkSerie, pkSufixo }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Curso { get { return (string)row["curso", DataRowVersion.Original]; } }
        public string Turno { get { return (string)row["turno", DataRowVersion.Original]; } }
        public string Curriculo { get { return (string)row["curriculo", DataRowVersion.Original]; } }
        public decimal? Serie { get { return (decimal?)row["serie", DataRowVersion.Original]; } }
        public string Sufixo { get { return (string)row["sufixo", DataRowVersion.Original]; } }
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
    #region Coluna: CURSO
    private ColumnCurso pvColCurso;

    public sealed class ColumnCurso : TDataColumn {

      public ColumnCurso() {
        this.ColumnName = "curso";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "curso";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_serie_sufixo.curso"; } }
      public override string OriginalCol { get { return "curso"; } }
      public override string RefCol { get { return "LY_SERIE_SUFIXO.CURSO"; } }
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

    #region Coluna: TURNO
    private ColumnTurno pvColTurno;

    public sealed class ColumnTurno : TDataColumn {

      public ColumnTurno() {
        this.ColumnName = "turno";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "turno";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_serie_sufixo.turno"; } }
      public override string OriginalCol { get { return "turno"; } }
      public override string RefCol { get { return "LY_SERIE_SUFIXO.TURNO"; } }
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

    #region Coluna: CURRICULO
    private ColumnCurriculo pvColCurriculo;

    public sealed class ColumnCurriculo : TDataColumn {

      public ColumnCurriculo() {
        this.ColumnName = "curriculo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "currículo";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_serie_sufixo.curriculo"; } }
      public override string OriginalCol { get { return "curriculo"; } }
      public override string RefCol { get { return "LY_SERIE_SUFIXO.CURRICULO"; } }
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

    #region Coluna: SERIE
    private ColumnSerie pvColSerie;

    public sealed class ColumnSerie : TDataColumn {

      public ColumnSerie() {
        this.ColumnName = "serie";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "série";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_serie_sufixo.serie"; } }
      public override string OriginalCol { get { return "serie"; } }
      public override string RefCol { get { return "LY_SERIE_SUFIXO.SERIE"; } }
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

    #region Coluna: SUFIXO
    private ColumnSufixo pvColSufixo;

    public sealed class ColumnSufixo : TDataColumn {

      public ColumnSufixo() {
        this.ColumnName = "sufixo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "sufixo";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_serie_sufixo.sufixo"; } }
      public override string OriginalCol { get { return "sufixo"; } }
      public override string RefCol { get { return "LY_SERIE_SUFIXO.SUFIXO"; } }
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
      public override string FullCol { get { return "ly_serie_sufixo.descricao"; } }
      public override string OriginalCol { get { return "descricao"; } }
      public override string RefCol { get { return "LY_SERIE_SUFIXO.DESCRICAO"; } }
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

  public partial class Ly_unidade_ensino_cursos : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_unidade_ensino_cursos() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_unidade_ensino_cursos");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_unidade_ensino_cursos");
      }
      catch{}
    }

    public Ly_unidade_ensino_cursos() {
      pvColUnidade_ens = new ColumnUnidade_ens(); Columns.Add(pvColUnidade_ens);
      pvColOrdem = new ColumnOrdem(); Columns.Add(pvColOrdem);
      pvColCurso = new ColumnCurso(); Columns.Add(pvColCurso);
      pvColTurno = new ColumnTurno(); Columns.Add(pvColTurno);
      pvColCurriculo = new ColumnCurriculo(); Columns.Add(pvColCurriculo);
      pvColAto = new ColumnAto(); Columns.Add(pvColAto);
      pvColDt_implantacao = new ColumnDt_implantacao(); Columns.Add(pvColDt_implantacao);
      pvColDt_do = new ColumnDt_do(); Columns.Add(pvColDt_do);
      pvColObservacoes = new ColumnObservacoes(); Columns.Add(pvColObservacoes);
      pvColMatricula = new ColumnMatricula(); Columns.Add(pvColMatricula);
      pvColDt_cadastro = new ColumnDt_cadastro(); Columns.Add(pvColDt_cadastro);
      pvColDt_alteracao = new ColumnDt_alteracao(); Columns.Add(pvColDt_alteracao);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_unidade_ensino_cursos");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(string @unidade_ens, decimal? @ordem, string @curso) {
      Row row = NewRow();

      row.Unidade_ens = @unidade_ens;
      row.Ordem = @ordem;
      row.Curso = @curso;

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
      get { return "LY_UNIDADE_ENSINO_CURSOS"; }
    }

    protected override sealed string Name { get { return "Curso Autorizado por Unidade"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColUnidade_ens, pvColOrdem });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_unidade_ensino_curso_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_unidade_ensino_curso_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_unidade_ensino_curso_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_unidade_ensino_curso_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkUnidade_ens", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "UNIDADE_ENS", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkOrdem", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ORDEM", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("unidade_ens", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "UNIDADE_ENS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("ordem", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ORDEM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("curso", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CURSO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("turno", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TURNO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("curriculo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CURRICULO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("ato", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "ATO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("dt_implantacao", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_IMPLANTACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_do", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_DO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("observacoes", OleDbType.VarChar, 500, ParameterDirection.InputOutput, true, 0, 0, "OBSERVACOES", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("matricula", OleDbType.VarChar, 8, ParameterDirection.InputOutput, true, 0, 0, "MATRICULA", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("dt_cadastro", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_CADASTRO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
      cm.Parameters.Add(new TParameter("dt_alteracao", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DT_ALTERACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));

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
          if(lower == "unidade_ens") { result.Add(toLower ? "unidade_ens" : column); continue; }
          if(lower == "ordem") { result.Add(toLower ? "ordem" : column); continue; }
          if(lower == "curso") { result.Add(toLower ? "curso" : column); continue; }
          if(lower == "turno") { result.Add(toLower ? "turno" : column); continue; }
          if(lower == "curriculo") { result.Add(toLower ? "curriculo" : column); continue; }
          if(lower == "ato") { result.Add(toLower ? "ato" : column); continue; }
          if(lower == "dt_implantacao") { result.Add(toLower ? "dt_implantacao" : column); continue; }
          if(lower == "dt_do") { result.Add(toLower ? "dt_do" : column); continue; }
          if(lower == "observacoes") { result.Add(toLower ? "observacoes" : column); continue; }
          if(lower == "matricula") { result.Add(toLower ? "matricula" : column); continue; }
          if(lower == "dt_cadastro") { result.Add(toLower ? "dt_cadastro" : column); continue; }
          if(lower == "dt_alteracao") { result.Add(toLower ? "dt_alteracao" : column); continue; }

          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "unidade_ens", "ordem", "curso", "turno", "curriculo", "ato", "dt_implantacao", 
                                           "dt_do", "observacoes", "matricula", "dt_cadastro", "dt_alteracao" }
                          : new string[] { "UNIDADE_ENS", "ORDEM", "CURSO", "TURNO", "CURRICULO", "ATO", "DT_IMPLANTACAO", 
                                           "DT_DO", "OBSERVACOES", "MATRICULA", "DT_CADASTRO", "DT_ALTERACAO" });
        else
          result.AddRange(toLower
                          ? new string[] { "unidade_ens", "ordem", "curso", "turno", "curriculo", "ato", "dt_implantacao", 
                                           "dt_do", "observacoes", "matricula", "dt_cadastro", "dt_alteracao" }
                          : new string[] { "UNIDADE_ENS", "ORDEM", "CURSO", "TURNO", "CURRICULO", "ATO", "DT_IMPLANTACAO", 
                                           "DT_DO", "OBSERVACOES", "MATRICULA", "DT_CADASTRO", "DT_ALTERACAO" });

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
        "ly_unidade_ensino_cursos.unidade_ens", "ly_unidade_ensino_cursos.ordem"
      });
      if(scColumns.Contains("curso")) list.Add("ly_unidade_ensino_cursos.curso");
      if(scColumns.Contains("turno")) list.Add("ly_unidade_ensino_cursos.turno");
      if(scColumns.Contains("curriculo")) list.Add("ly_unidade_ensino_cursos.curriculo");
      if(scColumns.Contains("ato")) list.Add("ly_unidade_ensino_cursos.ato");
      if(scColumns.Contains("dt_implantacao")) list.Add("ly_unidade_ensino_cursos.dt_implantacao");
      if(scColumns.Contains("dt_do")) list.Add("ly_unidade_ensino_cursos.dt_do");
      if(scColumns.Contains("observacoes")) list.Add("ly_unidade_ensino_cursos.observacoes");
      if(scColumns.Contains("matricula")) list.Add("ly_unidade_ensino_cursos.matricula");
      if(scColumns.Contains("dt_cadastro")) list.Add("ly_unidade_ensino_cursos.dt_cadastro");
      if(scColumns.Contains("dt_alteracao")) list.Add("ly_unidade_ensino_cursos.dt_alteracao");
      
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
          "FROM ly_unidade_ensino_cursos " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_unidade_ensino_cursos");
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
        da.TableMappings.Add("Table", "Ly_unidade_ensino_cursos");
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
            if (dr["UNIDADE_ENS"] != null && string.IsNullOrEmpty(dr["UNIDADE_ENS"].ToString()))
              dr["UNIDADE_ENS"] = DBNull.Value;
            if (dr["ORDEM"] != null && string.IsNullOrEmpty(dr["ORDEM"].ToString()))
              dr["ORDEM"] = DBNull.Value;
            if (dr["CURSO"] != null && string.IsNullOrEmpty(dr["CURSO"].ToString()))
              dr["CURSO"] = DBNull.Value;
            if (dr["TURNO"] != null && string.IsNullOrEmpty(dr["TURNO"].ToString()))
              dr["TURNO"] = DBNull.Value;
            if (dr["CURRICULO"] != null && string.IsNullOrEmpty(dr["CURRICULO"].ToString()))
              dr["CURRICULO"] = DBNull.Value;
            if (dr["ATO"] != null && string.IsNullOrEmpty(dr["ATO"].ToString()))
              dr["ATO"] = DBNull.Value;
            if (dr["OBSERVACOES"] != null && string.IsNullOrEmpty(dr["OBSERVACOES"].ToString()))
              dr["OBSERVACOES"] = DBNull.Value;
            if (dr["MATRICULA"] != null && string.IsNullOrEmpty(dr["MATRICULA"].ToString()))
              dr["MATRICULA"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["UNIDADE_ENS"] == null || string.IsNullOrEmpty(dr["UNIDADE_ENS"].ToString()))
            {
              return "Favor informar a Unidade de Ensino.";
            }

            if (dr["UNIDADE_ENS"] != null && !string.IsNullOrEmpty(dr["UNIDADE_ENS"].ToString()))
            {
              if (dr["UNIDADE_ENS"].ToString().Length > 20)
              {
                return "A Unidade de Ensino deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["ORDEM"] == null || string.IsNullOrEmpty(dr["ORDEM"].ToString()))
            {
              return "Favor informar a Ordem.";
            }

            if (dr["ORDEM"] != null && !string.IsNullOrEmpty(dr["ORDEM"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ORDEM"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ORDEM"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["ORDEM"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ORDEM"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "A Ordem deve ser um valor inteiro com 10 dígitos no máximo.";
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

            if (dr["TURNO"] != null && !string.IsNullOrEmpty(dr["TURNO"].ToString()))
            {
              if (dr["TURNO"].ToString().Length > 20)
              {
                return "O Turno deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["CURRICULO"] != null && !string.IsNullOrEmpty(dr["CURRICULO"].ToString()))
            {
              if (dr["CURRICULO"].ToString().Length > 20)
              {
                return "O Currículo deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["ATO"] != null && !string.IsNullOrEmpty(dr["ATO"].ToString()))
            {
              if (dr["ATO"].ToString().Length > 100)
              {
                return "O ato deve possuir no máximo 100 caracteres.";
              }
            }

            if (dr["DT_IMPLANTACAO"] != null && dr["DT_IMPLANTACAO"].ToString() != "")
            {
              dr["DT_IMPLANTACAO"] = Convert.ToDateTime(dr["DT_IMPLANTACAO"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["DT_DO"] != null && dr["DT_DO"].ToString() != "")
            {
              dr["DT_DO"] = Convert.ToDateTime(dr["DT_DO"]).ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (dr["OBSERVACOES"] != null && !string.IsNullOrEmpty(dr["OBSERVACOES"].ToString()))
            {
              if (dr["OBSERVACOES"].ToString().Length > 500)
              {
                return "As observações deve possuir no máximo 500 caracteres.";
              }
            }

            if (dr["MATRICULA"] != null && !string.IsNullOrEmpty(dr["MATRICULA"].ToString()))
            {
              if (dr["MATRICULA"].ToString().Length > 100)
              {
                return "A matrícula deve possuir no máximo 100 caracteres.";
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

    public static Ly_unidade_ensino_cursos Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_unidade_ensino_cursos Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_unidade_ensino_cursos tab = new Ly_unidade_ensino_cursos();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_unidade_ensino_cursos tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        string @unidade_ens, decimal? @ordem, string @curso, string @turno, string @curriculo, 
        string @ato, DateTime? @dt_implantacao, DateTime? @dt_do, string @observacoes,
        string @matricula, DateTime? @dt_cadastro, DateTime? @dt_alteracao
      ) {
        return (Row)Add(new object[] {
          @unidade_ens, @ordem, @curso, @turno, @curriculo, @ato, @dt_implantacao, @dt_do,
          @observacoes, @matricula, @dt_cadastro, @dt_alteracao
        });
      }

      public Row Find(
        string @unidade_ens, decimal? @ordem
      ) {
        return (Row)Find(new object[] {
          @unidade_ens, @ordem
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
      /// UNIDADE_ENS: VARCHAR(20), Primary Key, Not Null
      /// </summary>
      public string Unidade_ens { get { return (string)this["unidade_ens"]; } set { this["unidade_ens"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ORDEM: NUMERIC(10), Primary Key, Not Null
      /// </summary>
      public decimal? Ordem { get { return (decimal?)this["ordem"]; } set { this["ordem"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CURSO: VARCHAR(20), Not Null
      /// </summary>
      public string Curso { get { return (string)this["curso"]; } set { this["curso"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TURNO: VARCHAR(20)
      /// </summary>
      public string Turno { get { return (string)this["turno"]; } set { this["turno"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// CURRICULO: VARCHAR(20)
      /// </summary>
      public string Curriculo { get { return (string)this["curriculo"]; } set { this["curriculo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ATO: VARCHAR(100)
      /// </summary>
      public string Ato { get { return (string)this["ato"]; } set { this["ato"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_IMPLANTACAO: DATETIME
      /// </summary>
      public DateTime? Dt_implantacao { get { return (DateTime?)this["dt_implantacao"]; } set { this["dt_implantacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_DO: DATETIME
      /// </summary>
      public DateTime? Dt_do { get { return (DateTime?)this["dt_do"]; } set { this["dt_do"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// OBSERVACOES: VARCHAR(500)
      /// </summary>
      public string Observacoes { get { return (string)this["observacoes"]; } set { this["observacoes"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// MATRICULA: VARCHAR(8)
      /// </summary>
      public string Matricula { get { return (string)this["matricula"]; } set { this["matricula"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_CADASTRO: DATETIME
      /// </summary>
      public DateTime? Dt_cadastro { get { return (DateTime?)this["dt_cadastro"]; } set { this["dt_cadastro"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// DT_ALTERACAO: DATETIME
      /// </summary>
      public DateTime? Dt_alteracao { get { return (DateTime?)this["dt_alteracao"]; } set { this["dt_alteracao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               string @unidade_ens, decimal? @ordem, string @curso) {
        return Insert(connection,
                      @unidade_ens, @ordem, @curso,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               string @unidade_ens, decimal? @ordem, string @curso,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_unidade_ensino_cursos(),
          new string[] { "unidade_ens", "ordem", "curso" },
          new object[] { @unidade_ens, @ordem, @curso },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               string pkUnidade_ens, decimal? pkOrdem,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_unidade_ensino_cursos(),
          new string[] { "unidade_ens", "ordem" },
          new object[] { pkUnidade_ens, pkOrdem },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                string pkUnidade_ens, decimal? pkOrdem) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_unidade_ensino_cursos(),
          new string[] { "unidade_ens", "ordem" },
          new object[] { pkUnidade_ens, pkOrdem }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              string pkUnidade_ens, decimal? pkOrdem) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_unidade_ensino_cursos(),
          new string[] { "unidade_ens", "ordem" },
          new object[] { pkUnidade_ens, pkOrdem }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public string Unidade_ens { get { return (string)row["unidade_ens", DataRowVersion.Original]; } }
        public decimal? Ordem { get { return (decimal?)row["ordem", DataRowVersion.Original]; } }
        public string Curso { get { return (string)row["curso", DataRowVersion.Original]; } }
        public string Turno { get { return (string)row["turno", DataRowVersion.Original]; } }
        public string Curriculo { get { return (string)row["curriculo", DataRowVersion.Original]; } }
        public string Ato { get { return (string)row["ato", DataRowVersion.Original]; } }
        public DateTime? Dt_implantacao { get { return (DateTime?)row["dt_implantacao", DataRowVersion.Original]; } }
        public DateTime? Dt_do { get { return (DateTime?)row["dt_do", DataRowVersion.Original]; } }
        public string Observacoes { get { return (string)row["observacoes", DataRowVersion.Original]; } }
        public string Matricula { get { return (string)row["matricula", DataRowVersion.Original]; } }
        public DateTime? Dt_cadastro { get { return (DateTime?)row["dt_cadastro", DataRowVersion.Original]; } }
        public DateTime? Dt_alteracao { get { return (DateTime?)row["dt_alteracao", DataRowVersion.Original]; } }
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
    #region Coluna: UNIDADE_ENS
    private ColumnUnidade_ens pvColUnidade_ens;

    public sealed class ColumnUnidade_ens : TDataColumn {

      public ColumnUnidade_ens() {
        this.ColumnName = "unidade_ens";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Unidade de Ensino";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_unidade_ensino_cursos.unidade_ens"; } }
      public override string OriginalCol { get { return "unidade_ens"; } }
      public override string RefCol { get { return "LY_UNIDADE_ENSINO_CURSOS.UNIDADE_ENS"; } }
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

    #region Coluna: ORDEM
    private ColumnOrdem pvColOrdem;

    public sealed class ColumnOrdem : TDataColumn {

      public ColumnOrdem() {
        this.ColumnName = "ordem";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "Ordem";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_unidade_ensino_cursos.ordem"; } }
      public override string OriginalCol { get { return "ordem"; } }
      public override string RefCol { get { return "LY_UNIDADE_ENSINO_CURSOS.ORDEM"; } }
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

    #region Coluna: CURSO
    private ColumnCurso pvColCurso;

    public sealed class ColumnCurso : TDataColumn {

      public ColumnCurso() {
        this.ColumnName = "curso";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Curso";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_unidade_ensino_cursos.curso"; } }
      public override string OriginalCol { get { return "curso"; } }
      public override string RefCol { get { return "LY_UNIDADE_ENSINO_CURSOS.CURSO"; } }
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

    #region Coluna: TURNO
    private ColumnTurno pvColTurno;

    public sealed class ColumnTurno : TDataColumn {

      public ColumnTurno() {
        this.ColumnName = "turno";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Turno";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_unidade_ensino_cursos.turno"; } }
      public override string OriginalCol { get { return "turno"; } }
      public override string RefCol { get { return "LY_UNIDADE_ENSINO_CURSOS.TURNO"; } }
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

    #region Coluna: CURRICULO
    private ColumnCurriculo pvColCurriculo;

    public sealed class ColumnCurriculo : TDataColumn {

      public ColumnCurriculo() {
        this.ColumnName = "curriculo";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "Currículo";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_unidade_ensino_cursos.curriculo"; } }
      public override string OriginalCol { get { return "curriculo"; } }
      public override string RefCol { get { return "LY_UNIDADE_ENSINO_CURSOS.CURRICULO"; } }
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

    #region Coluna: ATO
    private ColumnAto pvColAto;

    public sealed class ColumnAto : TDataColumn {

      public ColumnAto() {
        this.ColumnName = "ato";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "ato";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_unidade_ensino_cursos.ato"; } }
      public override string OriginalCol { get { return "ato"; } }
      public override string RefCol { get { return "LY_UNIDADE_ENSINO_CURSOS.ATO"; } }
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

    #region Coluna: DT_IMPLANTACAO
    private ColumnDt_implantacao pvColDt_implantacao;

    public sealed class ColumnDt_implantacao : TDataColumn {

      public ColumnDt_implantacao() {
        this.ColumnName = "dt_implantacao";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data da implantação";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_unidade_ensino_cursos.dt_implantacao"; } }
      public override string OriginalCol { get { return "dt_implantacao"; } }
      public override string RefCol { get { return "LY_UNIDADE_ENSINO_CURSOS.DT_IMPLANTACAO"; } }
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

    #region Coluna: DT_DO
    private ColumnDt_do pvColDt_do;

    public sealed class ColumnDt_do : TDataColumn {

      public ColumnDt_do() {
        this.ColumnName = "dt_do";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "data d.o.";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_unidade_ensino_cursos.dt_do"; } }
      public override string OriginalCol { get { return "dt_do"; } }
      public override string RefCol { get { return "LY_UNIDADE_ENSINO_CURSOS.DT_DO"; } }
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

    #region Coluna: OBSERVACOES
    private ColumnObservacoes pvColObservacoes;

    public sealed class ColumnObservacoes : TDataColumn {

      public ColumnObservacoes() {
        this.ColumnName = "observacoes";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "observacoes";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_unidade_ensino_cursos.observacoes"; } }
      public override string OriginalCol { get { return "observacoes"; } }
      public override string RefCol { get { return "LY_UNIDADE_ENSINO_CURSOS.OBSERVACOES"; } }
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

    #region Coluna: MATRICULA
    private ColumnMatricula pvColMatricula;

    public sealed class ColumnMatricula : TDataColumn {

      public ColumnMatricula() {
        this.ColumnName = "matricula";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "matricula";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_unidade_ensino_cursos.matricula"; } }
      public override string OriginalCol { get { return "matricula"; } }
      public override string RefCol { get { return "LY_UNIDADE_ENSINO_CURSOS.MATRICULA"; } }
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


      protected override string GetName(){return "dt_cadastro";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_unidade_ensino_cursos.dt_cadastro"; } }
      public override string OriginalCol { get { return "dt_cadastro"; } }
      public override string RefCol { get { return "LY_UNIDADE_ENSINO_CURSOS.DT_CADASTRO"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return false; } }
      public override string SqlServerType { get { return "DATETIME"; } }

    }
    #endregion

    #region Coluna: DT_ALTERACAO
    private ColumnDt_alteracao pvColDt_alteracao;

    public sealed class ColumnDt_alteracao : TDataColumn {

      public ColumnDt_alteracao() {
        this.ColumnName = "dt_alteracao";
        this.DataType = typeof(DateTime);

      }


      protected override string GetName(){return "dt_alteracao";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_unidade_ensino_cursos.dt_alteracao"; } }
      public override string OriginalCol { get { return "dt_alteracao"; } }
      public override string RefCol { get { return "LY_UNIDADE_ENSINO_CURSOS.DT_ALTERACAO"; } }
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

  public partial class Ly_valor_glp : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_valor_glp() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_valor_glp");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_valor_glp");
      }
      catch{}
    }

    public Ly_valor_glp() {
      pvColAno = new ColumnAno(); Columns.Add(pvColAno);
      pvColMes = new ColumnMes(); Columns.Add(pvColMes);
      pvColFuncao = new ColumnFuncao(); Columns.Add(pvColFuncao);
      pvColValor = new ColumnValor(); Columns.Add(pvColValor);
      pvColTipo = new ColumnTipo(); Columns.Add(pvColTipo);
      pvColId_valor_glp = new ColumnId_valor_glp(); Columns.Add(pvColId_valor_glp);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_valor_glp");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(decimal? @ano, decimal? @mes, string @funcao, decimal? @valor) {
      Row row = NewRow();

      row.Ano = @ano;
      row.Mes = @mes;
      row.Funcao = @funcao;
      row.Valor = @valor;

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
      get { return "LY_VALOR_GLP"; }
    }

    protected override sealed string Name { get { return "Valor da GLP"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColId_valor_glp });
      pvColId_valor_glp.AllowDBNull = true;
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_valor_glp_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_valor_glp_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_valor_glp_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_valor_glp_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkId_valor_glp", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ID_VALOR_GLP", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("ano", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ANO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("mes", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "MES", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("funcao", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "FUNCAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("valor", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "VALOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("tipo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TIPO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
      cm.Parameters.Add(new TParameter("id_valor_glp", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ID_VALOR_GLP", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));

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
          if(lower == "ano") { result.Add(toLower ? "ano" : column); continue; }
          if(lower == "mes") { result.Add(toLower ? "mes" : column); continue; }
          if(lower == "funcao") { result.Add(toLower ? "funcao" : column); continue; }
          if(lower == "valor") { result.Add(toLower ? "valor" : column); continue; }
          if(lower == "tipo") { result.Add(toLower ? "tipo" : column); continue; }
          if(lower == "id_valor_glp") { result.Add(toLower ? "id_valor_glp" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "ano", "mes", "funcao", "valor", "tipo", "id_valor_glp" }
                          : new string[] { "ANO", "MES", "FUNCAO", "VALOR", "TIPO", "ID_VALOR_GLP" });
        else
          result.AddRange(toLower
                          ? new string[] { "ano", "mes", "funcao", "valor", "tipo", "id_valor_glp" }
                          : new string[] { "ANO", "MES", "FUNCAO", "VALOR", "TIPO", "ID_VALOR_GLP" });

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
        "ly_valor_glp.id_valor_glp"
      });
      if(scColumns.Contains("ano")) list.Add("ly_valor_glp.ano");
      if(scColumns.Contains("mes")) list.Add("ly_valor_glp.mes");
      if(scColumns.Contains("funcao")) list.Add("ly_valor_glp.funcao");
      if(scColumns.Contains("valor")) list.Add("ly_valor_glp.valor");
      if(scColumns.Contains("tipo")) list.Add("ly_valor_glp.tipo");
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
          "FROM ly_valor_glp " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_valor_glp");
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
        da.TableMappings.Add("Table", "Ly_valor_glp");
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
            if (dr["ANO"] != null && string.IsNullOrEmpty(dr["ANO"].ToString()))
              dr["ANO"] = DBNull.Value;
            if (dr["MES"] != null && string.IsNullOrEmpty(dr["MES"].ToString()))
              dr["MES"] = DBNull.Value;
            if (dr["FUNCAO"] != null && string.IsNullOrEmpty(dr["FUNCAO"].ToString()))
              dr["FUNCAO"] = DBNull.Value;
            if (dr["VALOR"] != null && string.IsNullOrEmpty(dr["VALOR"].ToString()))
              dr["VALOR"] = DBNull.Value;
            if (dr["TIPO"] != null && string.IsNullOrEmpty(dr["TIPO"].ToString()))
              dr["TIPO"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

            if (dr["ANO"] == null || string.IsNullOrEmpty(dr["ANO"].ToString()))
            {
              return "Favor informar o Ano.";
            }

            if (dr["ANO"] != null && !string.IsNullOrEmpty(dr["ANO"].ToString()))
            {
              if ((Convert.ToDecimal(dr["ANO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ANO"]).ToString("0.############").Split(',')[0].Length > 4) || (Convert.ToDecimal(dr["ANO"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["ANO"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O Ano deve ser um valor inteiro com 4 dígitos no máximo.";
              }
            }

            if (dr["MES"] == null || string.IsNullOrEmpty(dr["MES"].ToString()))
            {
              return "Favor informar o mês.";
            }

            if (dr["MES"] != null && !string.IsNullOrEmpty(dr["MES"].ToString()))
            {
              if ((Convert.ToDecimal(dr["MES"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["MES"]).ToString("0.############").Split(',')[0].Length > 2) || (Convert.ToDecimal(dr["MES"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["MES"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O mês deve ser um valor inteiro com 2 dígitos no máximo.";
              }
            }

            if (dr["FUNCAO"] == null || string.IsNullOrEmpty(dr["FUNCAO"].ToString()))
            {
              return "Favor informar a função.";
            }

            if (dr["FUNCAO"] != null && !string.IsNullOrEmpty(dr["FUNCAO"].ToString()))
            {
              if (dr["FUNCAO"].ToString().Length > 20)
              {
                return "A função deve possuir no máximo 20 caracteres.";
              }
            }

            if (dr["VALOR"] == null || string.IsNullOrEmpty(dr["VALOR"].ToString()))
            {
              return "Favor informar o Valor.";
            }

            if (dr["VALOR"] != null && !string.IsNullOrEmpty(dr["VALOR"].ToString()))
            {
              if ((Convert.ToDecimal(dr["VALOR"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["VALOR"]).ToString("0.############").Split(',')[0].Length > 8) || (Convert.ToDecimal(dr["VALOR"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["VALOR"]).ToString("0.############").Split(',')[1].Length > 2))
              {
                return "O Valor deve ser um valor numérico com, no máximo, 8 dígitos na parte inteira e 2 casas decimal.";
              }
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

    public static Ly_valor_glp Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_valor_glp Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_valor_glp tab = new Ly_valor_glp();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_valor_glp tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @ano, decimal? @mes, string @funcao, decimal? @valor, string @tipo, decimal? @id_valor_glp
      ) {
        return (Row)Add(new object[] {
          @ano, @mes, @funcao, @valor, @tipo, @id_valor_glp
        });
      }

      public Row Find(
        decimal? @id_valor_glp
      ) {
        return (Row)Find(new object[] {
          @id_valor_glp
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
      /// ANO: NUMERIC(4), Not Null
      /// </summary>
      public decimal? Ano { get { return (decimal?)this["ano"]; } set { this["ano"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// MES: NUMERIC(2), Not Null
      /// </summary>
      public decimal? Mes { get { return (decimal?)this["mes"]; } set { this["mes"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// FUNCAO: VARCHAR(20), Not Null
      /// </summary>
      public string Funcao { get { return (string)this["funcao"]; } set { this["funcao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// VALOR: NUMERIC(10, 2), Not Null
      /// </summary>
      public decimal? Valor { get { return (decimal?)this["valor"]; } set { this["valor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// TIPO: VARCHAR(20)
      /// </summary>
      public string Tipo { get { return (string)this["tipo"]; } set { this["tipo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// ID_VALOR_GLP: NUMERIC(10), Primary Key, AutoNumber, Not Null
      /// </summary>
      public decimal? Id_valor_glp { get { return (decimal?)this["id_valor_glp"]; } set { this["id_valor_glp"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               decimal? @ano, decimal? @mes, string @funcao, decimal? @valor) {
        return Insert(connection,
                      @ano, @mes, @funcao, @valor,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               decimal? @ano, decimal? @mes, string @funcao, decimal? @valor,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_valor_glp(),
          new string[] { "ano", "mes", "funcao", "valor" },
          new object[] { @ano, @mes, @funcao, @valor },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkId_valor_glp,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_valor_glp(),
          new string[] { "id_valor_glp" },
          new object[] { pkId_valor_glp },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkId_valor_glp) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_valor_glp(),
          new string[] { "id_valor_glp" },
          new object[] { pkId_valor_glp }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              decimal? pkId_valor_glp) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_valor_glp(),
          new string[] { "id_valor_glp" },
          new object[] { pkId_valor_glp }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public decimal? Ano { get { return (decimal?)row["ano", DataRowVersion.Original]; } }
        public decimal? Mes { get { return (decimal?)row["mes", DataRowVersion.Original]; } }
        public string Funcao { get { return (string)row["funcao", DataRowVersion.Original]; } }
        public decimal? Valor { get { return (decimal?)row["valor", DataRowVersion.Original]; } }
        public string Tipo { get { return (string)row["tipo", DataRowVersion.Original]; } }
        public decimal? Id_valor_glp { get { return (decimal?)row["id_valor_glp", DataRowVersion.Original]; } }

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
    #region Coluna: ANO
    private ColumnAno pvColAno;

    public sealed class ColumnAno : TDataColumn {

      public ColumnAno() {
        this.ColumnName = "ano";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "Ano";}
      protected override string GetCaption(){return "Ano";}
      protected override string GetDescricao(){return "Ano";}
      public override string FullCol { get { return "ly_valor_glp.ano"; } }
      public override string OriginalCol { get { return "ano"; } }
      public override string RefCol { get { return "LY_VALOR_GLP.ANO"; } }
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

    #region Coluna: MES
    private ColumnMes pvColMes;

    public sealed class ColumnMes : TDataColumn {

      public ColumnMes() {
        this.ColumnName = "mes";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "mês";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_valor_glp.mes"; } }
      public override string OriginalCol { get { return "mes"; } }
      public override string RefCol { get { return "LY_VALOR_GLP.MES"; } }
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

    #region Coluna: FUNCAO
    private ColumnFuncao pvColFuncao;

    public sealed class ColumnFuncao : TDataColumn {

      public ColumnFuncao() {
        this.ColumnName = "funcao";
        this.DataType = typeof(string);

      }


      protected override string GetName(){return "função";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_valor_glp.funcao"; } }
      public override string OriginalCol { get { return "funcao"; } }
      public override string RefCol { get { return "LY_VALOR_GLP.FUNCAO"; } }
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

    #region Coluna: VALOR
    private ColumnValor pvColValor;

    public sealed class ColumnValor : TDataColumn {

      public ColumnValor() {
        this.ColumnName = "valor";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "Valor";}
      protected override string GetCaption(){return "Valor";}
      protected override string GetDescricao(){return "Valor";}
      public override string FullCol { get { return "ly_valor_glp.valor"; } }
      public override string OriginalCol { get { return "valor"; } }
      public override string RefCol { get { return "LY_VALOR_GLP.VALOR"; } }
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
      public override string FullCol { get { return "ly_valor_glp.tipo"; } }
      public override string OriginalCol { get { return "tipo"; } }
      public override string RefCol { get { return "LY_VALOR_GLP.TIPO"; } }
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

    #region Coluna: ID_VALOR_GLP
    private ColumnId_valor_glp pvColId_valor_glp;

    public sealed class ColumnId_valor_glp : TDataColumn {

      public ColumnId_valor_glp() {
        this.ColumnName = "id_valor_glp";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "chave";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_valor_glp.id_valor_glp"; } }
      public override string OriginalCol { get { return "id_valor_glp"; } }
      public override string RefCol { get { return "LY_VALOR_GLP.ID_VALOR_GLP"; } }
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
    #endregion
  }

  public partial class Ly_verba_glp_ano_mes : TDataTable {
    private static ConstructorInfo ctorTCustom = null;
    private static ConstructorInfo ctorCustom = null;
    private CustomBase tcustom = null;
    private CustomBase custom = null;

    static Ly_verba_glp_ano_mes() {
      try
      {
        ctorTCustom = CRCustomLib.GetTCustomConstructor("Ly_verba_glp_ano_mes");
        ctorCustom = CRCustomLib.GetCustomConstructor("Ly_verba_glp_ano_mes");
      }
      catch{}
    }

    public Ly_verba_glp_ano_mes() {
      pvColAno = new ColumnAno(); Columns.Add(pvColAno);
      pvColMes = new ColumnMes(); Columns.Add(pvColMes);
      pvColValor = new ColumnValor(); Columns.Add(pvColValor);

      InitRows(new RowCollection(BaseRows));
      SetPrimaryKey();
      SetTableName("ly_verba_glp_ano_mes");

      if(ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
      if(ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
    }


    public new Row NewRow() {
      return (Row)base.NewRow();
    }

    public Row NewRow(decimal? @ano, decimal? @mes, decimal? @valor) {
      Row row = NewRow();

      row.Ano = @ano;
      row.Mes = @mes;
      row.Valor = @valor;

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
      get { return "LY_VERBA_GLP_ANO_MES"; }
    }

    protected override sealed string Name { get { return "Verba de glp por ano e mês"; } }

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
      SetPrimaryKey(new DataColumn[] { pvColAno, pvColMes });
    }


    private static TCommand CreateCommand(TConnection cn, CommandOperation operation) {
      bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
      TCommand cm = CreateDataTableCommand(cn, operation);
      cm.CommandType = CommandType.StoredProcedure;
      cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

      switch(operation) {
        case CommandOperation.Select: cm.CommandText = "zLy_verba_glp_ano_mes_GetCurrentValues"; break;
        case CommandOperation.Insert: cm.CommandText = "zLy_verba_glp_ano_mes_Cmd"; break;
        case CommandOperation.Update: cm.CommandText = "zLy_verba_glp_ano_mes_Cmd"; break;
        case CommandOperation.Delete: cm.CommandText = "zLy_verba_glp_ano_mes_Cmd"; break;
        default: throw new InvalidOperationException();
      }


      if(operation == CommandOperation.Select) {
        cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
        cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
      }

      cm.Parameters.Add(new TParameter("pkAno", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "ANO", DataRowVersion.Original, DBNull.Value));
      cm.Parameters.Add(new TParameter("pkMes", OleDbType.Numeric, 0, ParameterDirection.Input, true, 0, 0, "MES", DataRowVersion.Original, DBNull.Value));

      cm.Parameters.Add(new TParameter("ano", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "ANO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
      cm.Parameters.Add(new TParameter("mes", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "MES", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
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
          if(lower == "ano") { result.Add(toLower ? "ano" : column); continue; }
          if(lower == "mes") { result.Add(toLower ? "mes" : column); continue; }
          if(lower == "valor") { result.Add(toLower ? "valor" : column); continue; }
          if(!tabPrincipal) {
          }
        }

      if(result.Count == 0)
        if(tabPrincipal)
          result.AddRange(toLower
                          ? new string[] { "ano", "mes", "valor" }
                          : new string[] { "ANO", "MES", "VALOR" });
        else
          result.AddRange(toLower
                          ? new string[] { "ano", "mes", "valor" }
                          : new string[] { "ANO", "MES", "VALOR" });

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
        "ly_verba_glp_ano_mes.ano", "ly_verba_glp_ano_mes.mes"
      });
      if(scColumns.Contains("valor")) list.Add("ly_verba_glp_ano_mes.valor");
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
          "FROM ly_verba_glp_ano_mes " +
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
            count = da.Fill(ds, startRecord, maxRecords, "ly_verba_glp_ano_mes");
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
        da.TableMappings.Add("Table", "Ly_verba_glp_ano_mes");
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
            if (dr["ANO"] != null && string.IsNullOrEmpty(dr["ANO"].ToString()))
              dr["ANO"] = DBNull.Value;
            if (dr["MES"] != null && string.IsNullOrEmpty(dr["MES"].ToString()))
              dr["MES"] = DBNull.Value;
            if (dr["VALOR"] != null && string.IsNullOrEmpty(dr["VALOR"].ToString()))
              dr["VALOR"] = DBNull.Value;
      }
    }

    public string ValidaCampos(DataRow dr)
    {
      if (dr != null)
      {

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

            if (dr["MES"] == null || string.IsNullOrEmpty(dr["MES"].ToString()))
            {
              return "Favor informar o mês.";
            }

            if (dr["MES"] != null && !string.IsNullOrEmpty(dr["MES"].ToString()))
            {
              if ((Convert.ToDecimal(dr["MES"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["MES"]).ToString("0.############").Split(',')[0].Length > 2) || (Convert.ToDecimal(dr["MES"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["MES"]).ToString("0.############").Split(',')[1].Length > 0))
              {
                return "O mês deve ser um valor inteiro com 2 dígitos no máximo.";
              }
            }

            if (dr["VALOR"] == null || string.IsNullOrEmpty(dr["VALOR"].ToString()))
            {
              return "Favor informar o valor.";
            }

            if (dr["VALOR"] != null && !string.IsNullOrEmpty(dr["VALOR"].ToString()))
            {
              if ((Convert.ToDecimal(dr["VALOR"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["VALOR"]).ToString("0.############").Split(',')[0].Length > 10) || (Convert.ToDecimal(dr["VALOR"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["VALOR"]).ToString("0.############").Split(',')[1].Length > 2))
              {
                return "O valor deve ser um valor numérico com, no máximo, 8 dígitos na parte inteira e 2 casas decimal.";
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

    public static Ly_verba_glp_ano_mes Query(TConnection connection, string where, params object[] whereValues) {
      return Query(connection, false, where, whereValues);
    }

    public static Ly_verba_glp_ano_mes Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues) {
      Ly_verba_glp_ano_mes tab = new Ly_verba_glp_ano_mes();
      tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
      return tab;
    }

    public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues) {
      Ly_verba_glp_ano_mes tab = Query(connection, false, where, whereValues);
      return tab.Rows.Count > 0 ? tab.Rows[0] : null;
    }


    public sealed class RowCollection : TDataRowCollection {
      internal RowCollection(DataRowCollection rows) : base(rows) {
      }


      public Row Add(
        decimal? @ano, decimal? @mes, decimal? @valor
      ) {
        return (Row)Add(new object[] {
          @ano, @mes, @valor
        });
      }

      public Row Find(
        decimal? @ano, decimal? @mes
      ) {
        return (Row)Find(new object[] {
          @ano, @mes
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
      /// ANO: NUMERIC(4), Primary Key, Not Null
      /// </summary>
      public decimal? Ano { get { return (decimal?)this["ano"]; } set { this["ano"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// MES: NUMERIC(2), Primary Key, Not Null
      /// </summary>
      public decimal? Mes { get { return (decimal?)this["mes"]; } set { this["mes"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
      /// <summary>
      /// VALOR: NUMERIC(10, 2), Not Null
      /// </summary>
      public decimal? Valor { get { return (decimal?)this["valor"]; } set { this["valor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


      public OriginalValues Original { get { return original; } }




      public static Row Insert(TConnectionWritable connection,
                               decimal? @ano, decimal? @mes, decimal? @valor) {
        return Insert(connection,
                      @ano, @mes, @valor,
                      null);
      }

      public static Row Insert(TConnectionWritable connection,
                               decimal? @ano, decimal? @mes, decimal? @valor,
                               string columnList, params object[] values) {
        return (Row)Insert(
          connection, new Techne.Lyceum.CR.Ly_verba_glp_ano_mes(),
          new string[] { "ano", "mes", "valor" },
          new object[] { @ano, @mes, @valor },
          columnList, values
        );
      }

      public static Row Update(TConnectionWritable connection,
                               decimal? pkAno, decimal? pkMes,
                               string columnList, params object[] values) {
        return (Row)Update(
          connection, new Techne.Lyceum.CR.Ly_verba_glp_ano_mes(),
          new string[] { "ano", "mes" },
          new object[] { pkAno, pkMes },
          columnList, values
        );
      }

      public static bool Delete(TConnectionWritable connection,
                                decimal? pkAno, decimal? pkMes) {
        return Delete(
          connection, new Techne.Lyceum.CR.Ly_verba_glp_ano_mes(),
          new string[] { "ano", "mes" },
          new object[] { pkAno, pkMes }
        );
      }


      /// <summary>
      /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
      /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
      /// Dá exception se mais de um registro for encontrado.
      /// </summary>
      /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
      public static Row Query(TConnection connection,
                              decimal? pkAno, decimal? pkMes) {
        return (Row)Query(
          connection, new Techne.Lyceum.CR.Ly_verba_glp_ano_mes(),
          new string[] { "ano", "mes" },
          new object[] { pkAno, pkMes }
        );
      }


      public struct OriginalValues {
        private Row row;

        internal OriginalValues(Row row) {
          this.row = row;
        }


        public decimal? Ano { get { return (decimal?)row["ano", DataRowVersion.Original]; } }
        public decimal? Mes { get { return (decimal?)row["mes", DataRowVersion.Original]; } }
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
      public override string FullCol { get { return "ly_verba_glp_ano_mes.ano"; } }
      public override string OriginalCol { get { return "ano"; } }
      public override string RefCol { get { return "LY_VERBA_GLP_ANO_MES.ANO"; } }
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

    #region Coluna: MES
    private ColumnMes pvColMes;

    public sealed class ColumnMes : TDataColumn {

      public ColumnMes() {
        this.ColumnName = "mes";
        this.DataType = typeof(decimal);

      }


      protected override string GetName(){return "mês";}
      protected override string GetCaption(){return "";}
      protected override string GetDescricao(){return "";}
      public override string FullCol { get { return "ly_verba_glp_ano_mes.mes"; } }
      public override string OriginalCol { get { return "mes"; } }
      public override string RefCol { get { return "LY_VERBA_GLP_ANO_MES.MES"; } }
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
      public override string FullCol { get { return "ly_verba_glp_ano_mes.valor"; } }
      public override string OriginalCol { get { return "valor"; } }
      public override string RefCol { get { return "LY_VERBA_GLP_ANO_MES.VALOR"; } }
      public override bool PrimaryKey { get { return false; } }
      public override bool IsAux { get { return false; } }
      /// <summary>
      /// Esta propriedade indica se a coluna no banco de dados é NotNull ou não. Diferente de AllowDBNull.
      /// </summary>
      public override bool NotNull { get { return true; } }
      public override int Size { get { return 14; } }
      public override int Precision { get { return 12; } }
      public override int Scale { get { return 2; } }
      public override string SqlServerType { get { return "NUMERIC(12, 2)"; } }

    }
    #endregion
    #endregion
  }
}

