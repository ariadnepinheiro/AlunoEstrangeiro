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

namespace Techne.HadesLyc.CR
{
    public class Setor : TDataSet
    {
        private const string DataSetName_Def = "Setor";
        private static ConstructorInfo ctorTCustom = null;
        private CustomBase tcustom = null;
        #region Variáveis private para as tabelas deste dataset
        private Hd_setor pvTabHd_setor;
        #endregion

        static Setor()
        {
            try
            {
                ctorTCustom = CRCustomLib.GetTCustomConstructor("Setor");
            }
            catch { }
        }

        public Setor()
            : base()
        {
            DataSetName = DataSetName_Def;

            pvTabHd_setor = new Hd_setor();

            Tables.Add(pvTabHd_setor);

            CreateRelations();

            if (ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
        }


        [DefaultValue(DataSetName_Def)]
        public new string DataSetName
        {
            get { return base.DataSetName; }
            set { base.DataSetName = value == null ? string.Empty : value; }
        }

        private void CreateRelations()
        {
        }

        public override TConnection CreateConnection() { return Config.CreateConnection(); }

        public override TConnectionWritable CreateWritableConnection(TPermission permission) { return Config.CreateWritableConnection(permission); }

        public virtual string Put(TConnectionWritable connection)
        {
            connection.Open(true);
            try
            {
                if (tcustom != null)
                    return tcustom.Put(this, connection);

                else
                {
                    DataRow row;
                    pvTabHd_setor.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_setor); if (row != null) return DataLib.RowErrorsToString(row);
                    return string.Empty;
                }
            }
            catch
            {
                connection.Rollback();
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DataRelationCollection Relations { get { return base.Relations; } }


        #region Propriedades para acesso às tabelas
        [
          Browsable(false),
        ]
        public Hd_setor Hd_setor { get { return pvTabHd_setor; } }

        #endregion

        public abstract class CustomBase
        {
            public abstract string Put(Techne.HadesLyc.CR.Setor ds, TConnectionWritable connection);
        }

    }


    public partial class Hd_setor : TDataTable
    {
        private static ConstructorInfo ctorTCustom = null;
        private static ConstructorInfo ctorCustom = null;
        private CustomBase tcustom = null;
        private CustomBase custom = null;

        static Hd_setor()
        {
            try
            {
                ctorTCustom = CRCustomLib.GetTCustomConstructor("Hd_setor");
                ctorCustom = CRCustomLib.GetCustomConstructor("Hd_setor");
            }
            catch { }
        }

        public Hd_setor()
        {
            pvColSetor = new ColumnSetor(); Columns.Add(pvColSetor);
            pvColNome = new ColumnNome(); Columns.Add(pvColNome);
            pvColSetorpai = new ColumnSetorpai(); Columns.Add(pvColSetorpai);
            pvColTipo_setor = new ColumnTipo_setor(); Columns.Add(pvColTipo_setor);
            pvColDtini = new ColumnDtini(); Columns.Add(pvColDtini);
            pvColDtfim = new ColumnDtfim(); Columns.Add(pvColDtfim);
            pvColNumerolog = new ColumnNumerolog(); Columns.Add(pvColNumerolog);
            pvColComplemento = new ColumnComplemento(); Columns.Add(pvColComplemento);
            pvColFone = new ColumnFone(); Columns.Add(pvColFone);
            pvColFax = new ColumnFax(); Columns.Add(pvColFax);
            pvColAtivo = new ColumnAtivo(); Columns.Add(pvColAtivo);
            pvColMunicipio = new ColumnMunicipio(); Columns.Add(pvColMunicipio);
            pvColBairro = new ColumnBairro(); Columns.Add(pvColBairro);
            pvColLogradouro = new ColumnLogradouro(); Columns.Add(pvColLogradouro);
            pvColTrecho_logr = new ColumnTrecho_logr(); Columns.Add(pvColTrecho_logr);
            pvColCnpj = new ColumnCnpj(); Columns.Add(pvColCnpj);
            pvColPais = new ColumnPais(); Columns.Add(pvColPais);
            pvColCep = new ColumnCep(); Columns.Add(pvColCep);
            pvColStamp_atualizacao = new ColumnStamp_atualizacao(); Columns.Add(pvColStamp_atualizacao);
            pvColObservacao = new ColumnObservacao(); Columns.Add(pvColObservacao);
            pvColNome02 = new ColumnNome02(); Columns.Add(pvColNome02);
            pvColNome03 = new ColumnNome03(); Columns.Add(pvColNome03);
            pvColnovosetor = new Columnnovosetor(); Columns.Add(pvColnovosetor);
            pvColtipoComando = new ColumntipoComando(); Columns.Add(pvColtipoComando);

            InitRows(new RowCollection(BaseRows));
            SetPrimaryKey();
            SetTableName("hd_setor");

            if (ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
            if (ctorCustom != null) custom = (CustomBase)ctorCustom.Invoke(new object[0]);
        }


        public new Row NewRow()
        {
            return (Row)base.NewRow();
        }

        public Row NewRow(string @setor, string @nome)
        {
            Row row = NewRow();

            row.Setor = @setor;
            row.Nome = @nome;

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
            get { return "HD_SETOR"; }
        }

        protected override sealed string Name { get { return "Coordenadoria"; } }

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
            SetPrimaryKey(new DataColumn[] { pvColSetor });
        }


        private static TCommand CreateCommand(TConnection cn, CommandOperation operation)
        {
            bool sqlServer = cn.Rdbms == Rdbms.SQLServer;
            TCommand cm = CreateDataTableCommand(cn, operation);
            cm.CommandType = CommandType.StoredProcedure;
            cm.UpdatedRowSource = UpdateRowSource.OutputParameters;

            switch (operation)
            {
                case CommandOperation.Select: cm.CommandText = "zHd_setor_GetCurrentValues"; break;
                case CommandOperation.Insert: cm.CommandText = "zHd_setor_Cmd"; break;
                case CommandOperation.Update: cm.CommandText = "zHd_setor_Cmd"; break;
                case CommandOperation.Delete: cm.CommandText = "zHd_setor_Cmd"; break;
                default: throw new InvalidOperationException();
            }


            if (operation == CommandOperation.Select)
            {
                cm.Parameters.Add(new TParameter("idioma", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, "pt-BR"));
                cm.Parameters.Add(new TParameter("force", OleDbType.Integer, 0, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, 1));
            }

            cm.Parameters.Add(new TParameter("pkSetor", OleDbType.VarChar, 0, ParameterDirection.Input, true, 0, 0, "SETOR", DataRowVersion.Original, DBNull.Value));

            cm.Parameters.Add(new TParameter("setor", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SETOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
            cm.Parameters.Add(new TParameter("nome", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "NOME", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
            cm.Parameters.Add(new TParameter("setorpai", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "SETORPAI", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
            cm.Parameters.Add(new TParameter("tipo_setor", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TIPO_SETOR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
            cm.Parameters.Add(new TParameter("dtini", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DTINI", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
            cm.Parameters.Add(new TParameter("dtfim", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "DTFIM", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
            cm.Parameters.Add(new TParameter("numerolog", OleDbType.Numeric, 0, ParameterDirection.InputOutput, true, 20, 10, "NUMEROLOG", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(-99992.93)));
            cm.Parameters.Add(new TParameter("complemento", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "COMPLEMENTO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
            cm.Parameters.Add(new TParameter("fone", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "FONE", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
            cm.Parameters.Add(new TParameter("fax", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "FAX", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
            cm.Parameters.Add(new TParameter("ativo", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "ATIVO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_CHAR__")));
            cm.Parameters.Add(new TParameter("municipio", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "MUNICIPIO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
            cm.Parameters.Add(new TParameter("bairro", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "BAIRRO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
            cm.Parameters.Add(new TParameter("logradouro", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "LOGRADOURO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
            cm.Parameters.Add(new TParameter("trecho_logr", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "TRECHO_LOGR", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
            cm.Parameters.Add(new TParameter("cnpj", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CNPJ", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
            cm.Parameters.Add(new TParameter("pais", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "PAIS", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
            cm.Parameters.Add(new TParameter("cep", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "CEP", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
            cm.Parameters.Add(new TParameter("stamp_atualizacao", OleDbType.Date, 0, ParameterDirection.InputOutput, true, 0, 0, "STAMP_ATUALIZACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)(DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture))));
            cm.Parameters.Add(new TParameter("observacao", OleDbType.VarChar, 2100, ParameterDirection.InputOutput, true, 0, 0, "OBSERVACAO", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));
            cm.Parameters.Add(new TParameter("novosetor", OleDbType.VarChar, 200, ParameterDirection.InputOutput, true, 0, 0, "novosetor", DataRowVersion.Current, operation == CommandOperation.Insert ? DBNull.Value : (DbObject)("__UNDEF_VARCHAR__")));

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
                    if (lower == "setor") { result.Add(toLower ? "setor" : column); continue; }
                    if (lower == "nome") { result.Add(toLower ? "nome" : column); continue; }
                    if (lower == "setorpai") { result.Add(toLower ? "setorpai" : column); continue; }
                    if (lower == "tipo_setor") { result.Add(toLower ? "tipo_setor" : column); continue; }
                    if (lower == "dtini") { result.Add(toLower ? "dtini" : column); continue; }
                    if (lower == "dtfim") { result.Add(toLower ? "dtfim" : column); continue; }
                    if (lower == "numerolog") { result.Add(toLower ? "numerolog" : column); continue; }
                    if (lower == "complemento") { result.Add(toLower ? "complemento" : column); continue; }
                    if (lower == "fone") { result.Add(toLower ? "fone" : column); continue; }
                    if (lower == "fax") { result.Add(toLower ? "fax" : column); continue; }
                    if (lower == "ativo") { result.Add(toLower ? "ativo" : column); continue; }
                    if (lower == "municipio") { result.Add(toLower ? "municipio" : column); continue; }
                    if (lower == "bairro") { result.Add(toLower ? "bairro" : column); continue; }
                    if (lower == "logradouro") { result.Add(toLower ? "logradouro" : column); continue; }
                    if (lower == "trecho_logr") { result.Add(toLower ? "trecho_logr" : column); continue; }
                    if (lower == "cnpj") { result.Add(toLower ? "cnpj" : column); continue; }
                    if (lower == "pais") { result.Add(toLower ? "pais" : column); continue; }
                    if (lower == "cep") { result.Add(toLower ? "cep" : column); continue; }
                    if (lower == "stamp_atualizacao") { result.Add(toLower ? "stamp_atualizacao" : column); continue; }
                    if (lower == "observacao") { result.Add(toLower ? "observacao" : column); continue; }
                    if (!tabPrincipal)
                    {
                        if (lower == "nome02") { result.Add(toLower ? "nome02" : column); continue; }
                        if (lower == "nome03") { result.Add(toLower ? "nome03" : column); continue; }
                    }
                    if (lower == "novosetor") { result.Add(toLower ? "novosetor" : column); continue; }
                }

            if (result.Count == 0)
                if (tabPrincipal)
                    result.AddRange(toLower
                                    ? new string[] { "setor", "nome", "setorpai", "tipo_setor", "dtini", "dtfim", "numerolog", 
                                           "complemento", "fone", "fax", "ativo", "municipio", "bairro", "logradouro", "trecho_logr", 
                                           "cnpj", "pais", "cep", "stamp_atualizacao", "observacao","novosetor" }
                                    : new string[] { "SETOR", "NOME", "SETORPAI", "TIPO_SETOR", "DTINI", "DTFIM", "NUMEROLOG", 
                                           "COMPLEMENTO", "FONE", "FAX", "ATIVO", "MUNICIPIO", "BAIRRO", "LOGRADOURO", "TRECHO_LOGR", 
                                           "CNPJ", "PAIS", "CEP", "STAMP_ATUALIZACAO", "OBSERVACAO","novosetor" });
                else
                    result.AddRange(toLower
                                    ? new string[] { "setor", "nome", "setorpai", "tipo_setor", "dtini", "dtfim", "numerolog", 
                                           "complemento", "fone", "fax", "ativo", "municipio", "bairro", "logradouro", "trecho_logr", 
                                           "cnpj", "pais", "cep", "stamp_atualizacao", "observacao", "nome02", "nome03","novosetor" }
                                    : new string[] { "SETOR", "NOME", "SETORPAI", "TIPO_SETOR", "DTINI", "DTFIM", "NUMEROLOG", 
                                           "COMPLEMENTO", "FONE", "FAX", "ATIVO", "MUNICIPIO", "BAIRRO", "LOGRADOURO", "TRECHO_LOGR", 
                                           "CNPJ", "PAIS", "CEP", "STAMP_ATUALIZACAO", "OBSERVACAO", "NOME02", "NOME03","novosetor" });

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
        "hd_setor.setor"
      });
            if (scColumns.Contains("nome")) list.Add("hd_setor.nome");
            if (scColumns.Contains("setorpai")) list.Add("hd_setor.setorpai");
            if (scColumns.Contains("tipo_setor")) list.Add("hd_setor.tipo_setor");
            if (scColumns.Contains("dtini")) list.Add("hd_setor.dtini");
            if (scColumns.Contains("dtfim")) list.Add("hd_setor.dtfim");
            if (scColumns.Contains("numerolog")) list.Add("hd_setor.numerolog");
            if (scColumns.Contains("complemento")) list.Add("hd_setor.complemento");
            if (scColumns.Contains("fone")) list.Add("hd_setor.fone");
            if (scColumns.Contains("fax")) list.Add("hd_setor.fax");
            if (scColumns.Contains("ativo")) list.Add("hd_setor.ativo");
            if (scColumns.Contains("municipio")) list.Add("hd_setor.municipio");
            if (scColumns.Contains("bairro")) list.Add("hd_setor.bairro");
            if (scColumns.Contains("logradouro")) list.Add("hd_setor.logradouro");
            if (scColumns.Contains("trecho_logr")) list.Add("hd_setor.trecho_logr");
            if (scColumns.Contains("cnpj")) list.Add("hd_setor.cnpj");
            if (scColumns.Contains("pais")) list.Add("hd_setor.pais");
            if (scColumns.Contains("cep")) list.Add("hd_setor.cep");
            if (scColumns.Contains("stamp_atualizacao")) list.Add("hd_setor.stamp_atualizacao");
            if (scColumns.Contains("observacao")) list.Add("hd_setor.observacao");
            if (scColumns.Contains("nome02")) list.Add("hd_bairro.nome nome02");
            if (scColumns.Contains("nome03")) list.Add("hd_logradouro.nome nome03");
            if (scColumns.Contains("novosetor")) list.Add("hd_setor.novosetor");

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
                "FROM hd_setor " +
                     "LEFT JOIN hd_bairro " +
                       "ON hd_bairro.bairro = hd_setor.bairro AND " +
                          "hd_bairro.municipio = hd_setor.municipio " +
                     "LEFT JOIN hd_logradouro " +
                       "ON hd_logradouro.logradouro = hd_setor.logradouro AND " +
                          "hd_logradouro.municipio = hd_setor.municipio " +
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
                        count = da.Fill(ds, startRecord, maxRecords, "hd_setor");
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
                da.TableMappings.Add("Table", "Hd_setor");
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
                if (dr["SETOR"] != null && string.IsNullOrEmpty(dr["SETOR"].ToString()))
                    dr["SETOR"] = DBNull.Value;
                if (dr["NOME"] != null && string.IsNullOrEmpty(dr["NOME"].ToString()))
                    dr["NOME"] = DBNull.Value;
                if (dr["SETORPAI"] != null && string.IsNullOrEmpty(dr["SETORPAI"].ToString()))
                    dr["SETORPAI"] = DBNull.Value;
                if (dr["TIPO_SETOR"] != null && string.IsNullOrEmpty(dr["TIPO_SETOR"].ToString()))
                    dr["TIPO_SETOR"] = DBNull.Value;
                if (dr["NUMEROLOG"] != null && string.IsNullOrEmpty(dr["NUMEROLOG"].ToString()))
                    dr["NUMEROLOG"] = DBNull.Value;
                if (dr["COMPLEMENTO"] != null && string.IsNullOrEmpty(dr["COMPLEMENTO"].ToString()))
                    dr["COMPLEMENTO"] = DBNull.Value;
                if (dr["FONE"] != null && string.IsNullOrEmpty(dr["FONE"].ToString()))
                    dr["FONE"] = DBNull.Value;
                if (dr["FAX"] != null && string.IsNullOrEmpty(dr["FAX"].ToString()))
                    dr["FAX"] = DBNull.Value;
                if (dr["ATIVO"] != null && string.IsNullOrEmpty(dr["ATIVO"].ToString()))
                    dr["ATIVO"] = DBNull.Value;
                if (dr["MUNICIPIO"] != null && string.IsNullOrEmpty(dr["MUNICIPIO"].ToString()))
                    dr["MUNICIPIO"] = DBNull.Value;
                if (dr["BAIRRO"] != null && string.IsNullOrEmpty(dr["BAIRRO"].ToString()))
                    dr["BAIRRO"] = DBNull.Value;
                if (dr["LOGRADOURO"] != null && string.IsNullOrEmpty(dr["LOGRADOURO"].ToString()))
                    dr["LOGRADOURO"] = DBNull.Value;
                if (dr["TRECHO_LOGR"] != null && string.IsNullOrEmpty(dr["TRECHO_LOGR"].ToString()))
                    dr["TRECHO_LOGR"] = DBNull.Value;
                if (dr["CNPJ"] != null && string.IsNullOrEmpty(dr["CNPJ"].ToString()))
                    dr["CNPJ"] = DBNull.Value;
                if (dr["PAIS"] != null && string.IsNullOrEmpty(dr["PAIS"].ToString()))
                    dr["PAIS"] = DBNull.Value;
                if (dr["CEP"] != null && string.IsNullOrEmpty(dr["CEP"].ToString()))
                    dr["CEP"] = DBNull.Value;
                if (dr["OBSERVACAO"] != null && string.IsNullOrEmpty(dr["OBSERVACAO"].ToString()))
                    dr["OBSERVACAO"] = DBNull.Value;

                if (dr["novosetor"] != null && string.IsNullOrEmpty(dr["novosetor"].ToString()))
                    dr["novosetor"] = DBNull.Value;
            }
        }

        public string ValidaCampos(DataRow dr)
        {
            if (dr != null)
            {

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

                if (dr["SETORPAI"] != null && !string.IsNullOrEmpty(dr["SETORPAI"].ToString()))
                {
                    if (dr["SETORPAI"].ToString().Length > 15)
                    {
                        return "O setor pai deve possuir no máximo 15 caracteres.";
                    }
                }

                if (dr["TIPO_SETOR"] != null && !string.IsNullOrEmpty(dr["TIPO_SETOR"].ToString()))
                {
                    if (dr["TIPO_SETOR"].ToString().Length > 20)
                    {
                        return "O tipo do setor deve possuir no máximo 20 caracteres.";
                    }
                }

                if (dr["DTINI"] != null && dr["DTINI"].ToString() != "")
                {
                    dr["DTINI"] = Convert.ToDateTime(dr["DTINI"]).ToString("dd/MM/yyyy HH:mm:ss");
                }

                if (dr["DTFIM"] != null && dr["DTFIM"].ToString() != "")
                {
                    dr["DTFIM"] = Convert.ToDateTime(dr["DTFIM"]).ToString("dd/MM/yyyy HH:mm:ss");
                }

                if (dr["NUMEROLOG"] != null && !string.IsNullOrEmpty(dr["NUMEROLOG"].ToString()))
                {
                    if ((Convert.ToDecimal(dr["NUMEROLOG"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["NUMEROLOG"]).ToString("0.############").Split(',')[0].Length > 6) || (Convert.ToDecimal(dr["NUMEROLOG"]).ToString("0.############").Split(',').Length > 1 && Convert.ToDecimal(dr["NUMEROLOG"]).ToString("0.############").Split(',')[1].Length > 0))
                    {
                        return "O número deve ser um valor inteiro com 6 dígitos no máximo.";
                    }

                    if (dr["NUMEROLOG"] != null && !string.IsNullOrEmpty(dr["NUMEROLOG"].ToString()))
                    {
                        if (Convert.ToDecimal(dr["NUMEROLOG"]) < (0))
                        {
                            return "O número deve ser maior ou igual a (0).";
                        }
                    }
                }

                if (dr["COMPLEMENTO"] != null && !string.IsNullOrEmpty(dr["COMPLEMENTO"].ToString()))
                {
                    if (dr["COMPLEMENTO"].ToString().Length > 20)
                    {
                        return "O complemento deve possuir no máximo 20 caracteres.";
                    }
                }

                if (dr["FONE"] != null && !string.IsNullOrEmpty(dr["FONE"].ToString()))
                {
                    if (dr["FONE"].ToString().Length > 15)
                    {
                        return "O telefone deve possuir no máximo 15 caracteres.";
                    }
                }

                if (dr["FAX"] != null && !string.IsNullOrEmpty(dr["FAX"].ToString()))
                {
                    if (dr["FAX"].ToString().Length > 15)
                    {
                        return "O fax deve possuir no máximo 15 caracteres.";
                    }
                }

                if (dr["ATIVO"] != null && !string.IsNullOrEmpty(dr["ATIVO"].ToString()))
                {
                    if (dr["ATIVO"].ToString().Length > 1)
                    {
                        return "O ativo deve possuir no máximo 1 caractere.";
                    }

                    if (dr["ATIVO"] != null && !string.IsNullOrEmpty(dr["ATIVO"].ToString()))
                    {
                        List<object> lstATIVO = new List<object>(new object[] { "N", "S" });
                        if (!lstATIVO.Contains(dr["ATIVO"].ToString()))
                        {
                            return "Os valores permitidos para o ativo são ''N'' ou ''S''.";
                        }
                    }
                }

                if (dr["MUNICIPIO"] != null && !string.IsNullOrEmpty(dr["MUNICIPIO"].ToString()))
                {
                    if (dr["MUNICIPIO"].ToString().Length > 10)
                    {
                        return "O município deve possuir no máximo 10 caracteres.";
                    }
                }

                if (dr["BAIRRO"] != null && !string.IsNullOrEmpty(dr["BAIRRO"].ToString()))
                {
                    if (dr["BAIRRO"].ToString().Length > 10)
                    {
                        return "O bairro deve possuir no máximo 10 caracteres.";
                    }
                }

                if (dr["LOGRADOURO"] != null && !string.IsNullOrEmpty(dr["LOGRADOURO"].ToString()))
                {
                    if (dr["LOGRADOURO"].ToString().Length > 10)
                    {
                        return "O logradouro deve possuir no máximo 10 caracteres.";
                    }
                }

                if (dr["TRECHO_LOGR"] != null && !string.IsNullOrEmpty(dr["TRECHO_LOGR"].ToString()))
                {
                    if (dr["TRECHO_LOGR"].ToString().Length > 10)
                    {
                        return "O trecho do logradouro deve possuir no máximo 10 caracteres.";
                    }
                }

                if (dr["CNPJ"] != null && !string.IsNullOrEmpty(dr["CNPJ"].ToString()))
                {
                    if (dr["CNPJ"].ToString().Length > 19)
                    {
                        return "O cnpj deve possuir no máximo 19 caracteres.";
                    }
                }

                if (dr["PAIS"] != null && !string.IsNullOrEmpty(dr["PAIS"].ToString()))
                {
                    if (dr["PAIS"].ToString().Length > 10)
                    {
                        return "O país deve possuir no máximo 10 caracteres.";
                    }
                }

                if (dr["CEP"] != null && !string.IsNullOrEmpty(dr["CEP"].ToString()))
                {
                    if (dr["CEP"].ToString().Length > 9)
                    {
                        return "O cep deve possuir no máximo 9 caracteres.";
                    }
                }

                if (dr["STAMP_ATUALIZACAO"] != null && dr["STAMP_ATUALIZACAO"].ToString() != "")
                {
                    dr["STAMP_ATUALIZACAO"] = Convert.ToDateTime(dr["STAMP_ATUALIZACAO"]).ToString("dd/MM/yyyy HH:mm:ss");
                }

                if (dr["OBSERVACAO"] != null && !string.IsNullOrEmpty(dr["OBSERVACAO"].ToString()))
                {
                    if (dr["OBSERVACAO"].ToString().Length > 2000)
                    {
                        return "A observação deve possuir no máximo 2000 caracteres.";
                    }
                }

                if (dr["novosetor"] != null && !string.IsNullOrEmpty(dr["novosetor"].ToString()))
                {
                    if (dr["novosetor"].ToString().Length > 15)
                    {
                        return "O novo setor deve possuir no máximo 15 caracteres.";
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

        public static Hd_setor Query(TConnection connection, string where, params object[] whereValues)
        {
            return Query(connection, false, where, whereValues);
        }

        public static Hd_setor Query(TConnection connection, bool getDeletedRecords, string where, params object[] whereValues)
        {
            Hd_setor tab = new Hd_setor();
            tab.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, true, getDeletedRecords);
            return tab;
        }

        public static Row QueryFirstRow(TConnection connection, string where, params object[] whereValues)
        {
            Hd_setor tab = Query(connection, false, where, whereValues);
            return tab.Rows.Count > 0 ? tab.Rows[0] : null;
        }


        public sealed class RowCollection : TDataRowCollection
        {
            internal RowCollection(DataRowCollection rows)
                : base(rows)
            {
            }


            public Row Add(
              string @setor, string @nome, string @setorpai, string @tipo_setor, DateTime? @dtini,
              DateTime? @dtfim, decimal? @numerolog, string @complemento, string @fone, string @fax,
              string @ativo, string @municipio, string @bairro, string @logradouro, string @trecho_logr,
              string @cnpj, string @pais, string @cep, DateTime? @stamp_atualizacao, string @observacao, string @novosetor
            )
            {
                return (Row)Add(new object[] {
          @setor, @nome, @setorpai, @tipo_setor, @dtini, @dtfim, @numerolog, @complemento, 
          @fone, @fax, @ativo, @municipio, @bairro, @logradouro, @trecho_logr, @cnpj, @pais, 
          @cep, @stamp_atualizacao, @observacao, @novosetor
        });
            }

            public Row Find(
              string @setor
            )
            {
                return (Row)Find(new object[] {
          @setor
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
            /// SETOR: VARCHAR(15), Primary Key, Not Null
            /// </summary>
            public string Setor { get { return (string)this["setor"]; } set { this["setor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
            /// <summary>
            /// NOME: VARCHAR(100), Not Null
            /// </summary>
            public string Nome { get { return (string)this["nome"]; } set { this["nome"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
            /// <summary>
            /// SETORPAI: VARCHAR(15)
            /// </summary>
            public string Setorpai { get { return (string)this["setorpai"]; } set { this["setorpai"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
            /// <summary>
            /// TIPO_SETOR: VARCHAR(20)
            /// </summary>
            public string Tipo_setor { get { return (string)this["tipo_setor"]; } set { this["tipo_setor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
            /// <summary>
            /// DTINI: DATETIME
            /// </summary>
            public DateTime? Dtini { get { return (DateTime?)this["dtini"]; } set { this["dtini"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
            /// <summary>
            /// DTFIM: DATETIME
            /// </summary>
            public DateTime? Dtfim { get { return (DateTime?)this["dtfim"]; } set { this["dtfim"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
            /// <summary>
            /// NUMEROLOG: NUMERIC(6)
            /// </summary>
            public decimal? Numerolog { get { return (decimal?)this["numerolog"]; } set { this["numerolog"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
            /// <summary>
            /// COMPLEMENTO: VARCHAR(20)
            /// </summary>
            public string Complemento { get { return (string)this["complemento"]; } set { this["complemento"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
            /// <summary>
            /// FONE: VARCHAR(15)
            /// </summary>
            public string Fone { get { return (string)this["fone"]; } set { this["fone"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
            /// <summary>
            /// FAX: VARCHAR(15)
            /// </summary>
            public string Fax { get { return (string)this["fax"]; } set { this["fax"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
            /// <summary>
            /// ATIVO: CHAR(1)
            /// </summary>
            public string Ativo { get { return (string)this["ativo"]; } set { this["ativo"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
            /// <summary>
            /// MUNICIPIO: VARCHAR(10)
            /// </summary>
            public string Municipio { get { return (string)this["municipio"]; } set { this["municipio"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
            /// <summary>
            /// BAIRRO: VARCHAR(10)
            /// </summary>
            public string Bairro { get { return (string)this["bairro"]; } set { this["bairro"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
            /// <summary>
            /// LOGRADOURO: VARCHAR(10)
            /// </summary>
            public string Logradouro { get { return (string)this["logradouro"]; } set { this["logradouro"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
            /// <summary>
            /// TRECHO_LOGR: VARCHAR(10)
            /// </summary>
            public string Trecho_logr { get { return (string)this["trecho_logr"]; } set { this["trecho_logr"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
            /// <summary>
            /// CNPJ: VARCHAR(19)
            /// </summary>
            public string Cnpj { get { return (string)this["cnpj"]; } set { this["cnpj"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
            /// <summary>
            /// PAIS: VARCHAR(10)
            /// </summary>
            public string Pais { get { return (string)this["pais"]; } set { this["pais"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
            /// <summary>
            /// CEP: VARCHAR(9)
            /// </summary>
            public string Cep { get { return (string)this["cep"]; } set { this["cep"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
            /// <summary>
            /// STAMP_ATUALIZACAO: DATETIME
            /// </summary>
            public DateTime? Stamp_atualizacao { get { return (DateTime?)this["stamp_atualizacao"]; } set { this["stamp_atualizacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
            /// <summary>
            /// OBSERVACAO: VARCHAR(2000)
            /// </summary>
            public string Observacao { get { return (string)this["observacao"]; } set { this["observacao"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
            /// <summary>
            /// NOME: VARCHAR(50), Not Null
            /// </summary>
            public string Nome02 { get { return (string)this["nome02"]; } set { this["nome02"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }
            /// <summary>
            /// NOME: VARCHAR(100), Not Null
            /// </summary>
            public string Nome03 { get { return (string)this["nome03"]; } set { this["nome03"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }

            /// <summary>
            /// novosetor: VARCHAR(15)
            /// </summary>
            public string novosetor { get { return (string)this["novosetor"]; } set { this["novosetor"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }

            public OriginalValues Original { get { return original; } }

            public string tipoComando { get { return (string)this["tipoComando"]; } set { this["tipoComando"] = (value == null ? DBNull.Value : DbObject.ToDbObject(value)); } }


            public static Row Insert(TConnectionWritable connection,
                                     string @setor, string @nome)
            {
                return Insert(connection,
                              @setor, @nome,
                              null);
            }

            public static Row Insert(TConnectionWritable connection,
                                     string @setor, string @nome,
                                     string columnList, params object[] values)
            {
                return (Row)Insert(
                  connection, new Techne.HadesLyc.CR.Hd_setor(),
                  new string[] { "setor", "nome" },
                  new object[] { @setor, @nome },
                  columnList, values
                );
            }

            

            public static Row Update(TConnectionWritable connection,
                                     string pkSetor,
                                     string columnList, params object[] values)
            {
                return (Row)Update(
                  connection, new Techne.HadesLyc.CR.Hd_setor(),
                  new string[] { "setor" },
                  new object[] { pkSetor },
                  columnList, values
                );
            }

            public static bool Delete(TConnectionWritable connection,
                                      string pkSetor)
            {
                return Delete(
                  connection, new Techne.HadesLyc.CR.Hd_setor(),
                  new string[] { "setor" },
                  new object[] { pkSetor }
                );
            }


            /// <summary>
            /// Obtém um registro do banco. Um TDataTable é instanciado para acomodar o registro obtido.
            /// Se nenhum registro for encontrado, devolve null, e conexão trará erro correspondente.
            /// Dá exception se mais de um registro for encontrado.
            /// </summary>
            /// <param name="connection">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
            public static Row Query(TConnection connection,
                                    string pkSetor)
            {
                return (Row)Query(
                  connection, new Techne.HadesLyc.CR.Hd_setor(),
                  new string[] { "setor" },
                  new object[] { pkSetor }
                );
            }


            public struct OriginalValues
            {
                private Row row;

                internal OriginalValues(Row row)
                {
                    this.row = row;
                }


                public string Setor { get { return (string)row["setor", DataRowVersion.Original]; } }
                public string Nome { get { return (string)row["nome", DataRowVersion.Original]; } }
                public string Setorpai { get { return (string)row["setorpai", DataRowVersion.Original]; } }
                public string Tipo_setor { get { return (string)row["tipo_setor", DataRowVersion.Original]; } }
                public DateTime? Dtini { get { return (DateTime?)row["dtini", DataRowVersion.Original]; } }
                public DateTime? Dtfim { get { return (DateTime?)row["dtfim", DataRowVersion.Original]; } }
                public decimal? Numerolog { get { return (decimal?)row["numerolog", DataRowVersion.Original]; } }
                public string Complemento { get { return (string)row["complemento", DataRowVersion.Original]; } }
                public string Fone { get { return (string)row["fone", DataRowVersion.Original]; } }
                public string Fax { get { return (string)row["fax", DataRowVersion.Original]; } }
                public string Ativo { get { return (string)row["ativo", DataRowVersion.Original]; } }
                public string Municipio { get { return (string)row["municipio", DataRowVersion.Original]; } }
                public string Bairro { get { return (string)row["bairro", DataRowVersion.Original]; } }
                public string Logradouro { get { return (string)row["logradouro", DataRowVersion.Original]; } }
                public string Trecho_logr { get { return (string)row["trecho_logr", DataRowVersion.Original]; } }
                public string Cnpj { get { return (string)row["cnpj", DataRowVersion.Original]; } }
                public string Pais { get { return (string)row["pais", DataRowVersion.Original]; } }
                public string Cep { get { return (string)row["cep", DataRowVersion.Original]; } }
                public DateTime? Stamp_atualizacao { get { return (DateTime?)row["stamp_atualizacao", DataRowVersion.Original]; } }
                public string Observacao { get { return (string)row["observacao", DataRowVersion.Original]; } }
                public string Nome02 { get { return (string)row["nome02", DataRowVersion.Original]; } }
                public string Nome03 { get { return (string)row["nome03", DataRowVersion.Original]; } }
                public string novosetor { get { return (string)row["novosetor", DataRowVersion.Original]; } }

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
        #region Coluna: SETOR
        private ColumnSetor pvColSetor;

        public sealed class ColumnSetor : TDataColumn
        {

            public ColumnSetor()
            {
                this.ColumnName = "setor";
                this.DataType = typeof(string);

            }


            protected override string GetName() { return "setor"; }
            protected override string GetCaption() { return ""; }
            protected override string GetDescricao() { return ""; }
            public override string FullCol { get { return "hd_setor.setor"; } }
            public override string OriginalCol { get { return "setor"; } }
            public override string RefCol { get { return "HD_SETOR.SETOR"; } }
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
            public override string FullCol { get { return "hd_setor.nome"; } }
            public override string OriginalCol { get { return "nome"; } }
            public override string RefCol { get { return "HD_SETOR.NOME"; } }
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

        #region Coluna: SETORPAI
        private ColumnSetorpai pvColSetorpai;

        public sealed class ColumnSetorpai : TDataColumn
        {

            public ColumnSetorpai()
            {
                this.ColumnName = "setorpai";
                this.DataType = typeof(string);

            }


            protected override string GetName() { return "setor pai"; }
            protected override string GetCaption() { return ""; }
            protected override string GetDescricao() { return ""; }
            public override string FullCol { get { return "hd_setor.setorpai"; } }
            public override string OriginalCol { get { return "setorpai"; } }
            public override string RefCol { get { return "HD_SETOR.SETORPAI"; } }
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

        #region Coluna: TIPO_SETOR
        private ColumnTipo_setor pvColTipo_setor;

        public sealed class ColumnTipo_setor : TDataColumn
        {

            public ColumnTipo_setor()
            {
                this.ColumnName = "tipo_setor";
                this.DataType = typeof(string);

            }


            protected override string GetName() { return "tipo do setor"; }
            protected override string GetCaption() { return ""; }
            protected override string GetDescricao() { return ""; }
            public override string FullCol { get { return "hd_setor.tipo_setor"; } }
            public override string OriginalCol { get { return "tipo_setor"; } }
            public override string RefCol { get { return "HD_SETOR.TIPO_SETOR"; } }
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

        #region Coluna: DTINI
        private ColumnDtini pvColDtini;

        public sealed class ColumnDtini : TDataColumn
        {

            public ColumnDtini()
            {
                this.ColumnName = "dtini";
                this.DataType = typeof(DateTime);

            }


            protected override string GetName() { return "data inicial"; }
            protected override string GetCaption() { return ""; }
            protected override string GetDescricao() { return ""; }
            public override string FullCol { get { return "hd_setor.dtini"; } }
            public override string OriginalCol { get { return "dtini"; } }
            public override string RefCol { get { return "HD_SETOR.DTINI"; } }
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

        #region Coluna: DTFIM
        private ColumnDtfim pvColDtfim;

        public sealed class ColumnDtfim : TDataColumn
        {

            public ColumnDtfim()
            {
                this.ColumnName = "dtfim";
                this.DataType = typeof(DateTime);

            }


            protected override string GetName() { return "data final"; }
            protected override string GetCaption() { return ""; }
            protected override string GetDescricao() { return ""; }
            public override string FullCol { get { return "hd_setor.dtfim"; } }
            public override string OriginalCol { get { return "dtfim"; } }
            public override string RefCol { get { return "HD_SETOR.DTFIM"; } }
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

        #region Coluna: NUMEROLOG
        private ColumnNumerolog pvColNumerolog;

        public sealed class ColumnNumerolog : TDataColumn
        {

            public ColumnNumerolog()
            {
                this.ColumnName = "numerolog";
                this.DataType = typeof(decimal);

            }


            protected override string GetName() { return "número"; }
            protected override string GetCaption() { return ""; }
            protected override string GetDescricao() { return ""; }
            public override string FullCol { get { return "hd_setor.numerolog"; } }
            public override string OriginalCol { get { return "numerolog"; } }
            public override string RefCol { get { return "HD_SETOR.NUMEROLOG"; } }
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

        #region Coluna: COMPLEMENTO
        private ColumnComplemento pvColComplemento;

        public sealed class ColumnComplemento : TDataColumn
        {

            public ColumnComplemento()
            {
                this.ColumnName = "complemento";
                this.DataType = typeof(string);

            }


            protected override string GetName() { return "complemento"; }
            protected override string GetCaption() { return ""; }
            protected override string GetDescricao() { return ""; }
            public override string FullCol { get { return "hd_setor.complemento"; } }
            public override string OriginalCol { get { return "complemento"; } }
            public override string RefCol { get { return "HD_SETOR.COMPLEMENTO"; } }
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
            public override string FullCol { get { return "hd_setor.fone"; } }
            public override string OriginalCol { get { return "fone"; } }
            public override string RefCol { get { return "HD_SETOR.FONE"; } }
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

        #region Coluna: FAX
        private ColumnFax pvColFax;

        public sealed class ColumnFax : TDataColumn
        {

            public ColumnFax()
            {
                this.ColumnName = "fax";
                this.DataType = typeof(string);

            }


            protected override string GetName() { return "fax"; }
            protected override string GetCaption() { return ""; }
            protected override string GetDescricao() { return ""; }
            public override string FullCol { get { return "hd_setor.fax"; } }
            public override string OriginalCol { get { return "fax"; } }
            public override string RefCol { get { return "HD_SETOR.FAX"; } }
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

        #region Coluna: ATIVO
        private ColumnAtivo pvColAtivo;

        public sealed class ColumnAtivo : TDataColumn
        {

            public ColumnAtivo()
            {
                this.ColumnName = "ativo";
                this.DataType = typeof(string);

            }


            protected override string GetName() { return "ativo"; }
            protected override string GetCaption() { return ""; }
            protected override string GetDescricao() { return ""; }
            public override string FullCol { get { return "hd_setor.ativo"; } }
            public override string OriginalCol { get { return "ativo"; } }
            public override string RefCol { get { return "HD_SETOR.ATIVO"; } }
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

        #region Coluna: MUNICIPIO
        private ColumnMunicipio pvColMunicipio;

        public sealed class ColumnMunicipio : TDataColumn
        {

            public ColumnMunicipio()
            {
                this.ColumnName = "municipio";
                this.DataType = typeof(string);

            }


            protected override string GetName() { return "município"; }
            protected override string GetCaption() { return ""; }
            protected override string GetDescricao() { return ""; }
            public override string FullCol { get { return "hd_setor.municipio"; } }
            public override string OriginalCol { get { return "municipio"; } }
            public override string RefCol { get { return "HD_SETOR.MUNICIPIO"; } }
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
            public override string FullCol { get { return "hd_setor.bairro"; } }
            public override string OriginalCol { get { return "bairro"; } }
            public override string RefCol { get { return "HD_SETOR.BAIRRO"; } }
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

        #region Coluna: LOGRADOURO
        private ColumnLogradouro pvColLogradouro;

        public sealed class ColumnLogradouro : TDataColumn
        {

            public ColumnLogradouro()
            {
                this.ColumnName = "logradouro";
                this.DataType = typeof(string);

            }


            protected override string GetName() { return "logradouro"; }
            protected override string GetCaption() { return ""; }
            protected override string GetDescricao() { return ""; }
            public override string FullCol { get { return "hd_setor.logradouro"; } }
            public override string OriginalCol { get { return "logradouro"; } }
            public override string RefCol { get { return "HD_SETOR.LOGRADOURO"; } }
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

        #region Coluna: TRECHO_LOGR
        private ColumnTrecho_logr pvColTrecho_logr;

        public sealed class ColumnTrecho_logr : TDataColumn
        {

            public ColumnTrecho_logr()
            {
                this.ColumnName = "trecho_logr";
                this.DataType = typeof(string);

            }


            protected override string GetName() { return "trecho do logradouro"; }
            protected override string GetCaption() { return ""; }
            protected override string GetDescricao() { return ""; }
            public override string FullCol { get { return "hd_setor.trecho_logr"; } }
            public override string OriginalCol { get { return "trecho_logr"; } }
            public override string RefCol { get { return "HD_SETOR.TRECHO_LOGR"; } }
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

        #region Coluna: CNPJ
        private ColumnCnpj pvColCnpj;

        public sealed class ColumnCnpj : TDataColumn
        {

            public ColumnCnpj()
            {
                this.ColumnName = "cnpj";
                this.DataType = typeof(string);

            }


            protected override string GetName() { return "cnpj"; }
            protected override string GetCaption() { return ""; }
            protected override string GetDescricao() { return ""; }
            public override string FullCol { get { return "hd_setor.cnpj"; } }
            public override string OriginalCol { get { return "cnpj"; } }
            public override string RefCol { get { return "HD_SETOR.CNPJ"; } }
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

        #region Coluna: PAIS
        private ColumnPais pvColPais;

        public sealed class ColumnPais : TDataColumn
        {

            public ColumnPais()
            {
                this.ColumnName = "pais";
                this.DataType = typeof(string);

            }


            protected override string GetName() { return "país"; }
            protected override string GetCaption() { return ""; }
            protected override string GetDescricao() { return ""; }
            public override string FullCol { get { return "hd_setor.pais"; } }
            public override string OriginalCol { get { return "pais"; } }
            public override string RefCol { get { return "HD_SETOR.PAIS"; } }
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
            public override string FullCol { get { return "hd_setor.cep"; } }
            public override string OriginalCol { get { return "cep"; } }
            public override string RefCol { get { return "HD_SETOR.CEP"; } }
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
            public override string FullCol { get { return "hd_setor.stamp_atualizacao"; } }
            public override string OriginalCol { get { return "stamp_atualizacao"; } }
            public override string RefCol { get { return "HD_SETOR.STAMP_ATUALIZACAO"; } }
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

        #region Coluna: OBSERVACAO
        private ColumnObservacao pvColObservacao;

        public sealed class ColumnObservacao : TDataColumn
        {

            public ColumnObservacao()
            {
                this.ColumnName = "observacao";
                this.DataType = typeof(string);

            }


            protected override string GetName() { return "observação"; }
            protected override string GetCaption() { return ""; }
            protected override string GetDescricao() { return ""; }
            public override string FullCol { get { return "hd_setor.observacao"; } }
            public override string OriginalCol { get { return "observacao"; } }
            public override string RefCol { get { return "HD_SETOR.OBSERVACAO"; } }
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

        #region Coluna: NOME02
        private ColumnNome02 pvColNome02;

        public sealed class ColumnNome02 : TDataColumn
        {

            public ColumnNome02()
            {
                this.ColumnName = "nome02";
                this.DataType = typeof(string);

            }


            protected override string GetName() { return "nome do bairro"; }
            protected override string GetCaption() { return ""; }
            protected override string GetDescricao() { return ""; }
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

        #region Coluna: NOME03
        private ColumnNome03 pvColNome03;

        public sealed class ColumnNome03 : TDataColumn
        {

            public ColumnNome03()
            {
                this.ColumnName = "nome03";
                this.DataType = typeof(string);

            }


            protected override string GetName() { return "nome do logradouro"; }
            protected override string GetCaption() { return ""; }
            protected override string GetDescricao() { return ""; }
            public override string FullCol { get { return "hd_logradouro.nome"; } }
            public override string OriginalCol { get { return "nome"; } }
            public override string RefCol { get { return "HD_LOGRADOURO.NOME"; } }
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

        #region Coluna: novosetor
        private Columnnovosetor pvColnovosetor;

        public sealed class Columnnovosetor : TDataColumn
        {

            public Columnnovosetor()
            {
                this.ColumnName = "novosetor";
                this.DataType = typeof(string);

            }


            protected override string GetName() { return "novo setor"; }
            protected override string GetCaption() { return ""; }
            protected override string GetDescricao() { return ""; }
            public override string FullCol { get { return "hd_setor.novosetor"; } }
            public override string OriginalCol { get { return "novosetor"; } }
            public override string RefCol { get { return "HD_SETOR.novosetor"; } }
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

        #region Coluna: tipoComando
        private ColumntipoComando pvColtipoComando;

        public sealed class ColumntipoComando : TDataColumn
        {

            public ColumntipoComando()
            {
                this.ColumnName = "tipoComando";
                this.DataType = typeof(string);

            }


            protected override string GetName() { return "tipoComando"; }
            protected override string GetCaption() { return ""; }
            protected override string GetDescricao() { return ""; }
            public override string FullCol { get { return "hd_setor.tipoComando"; } }
            public override string OriginalCol { get { return "tipoComando"; } }
            public override string RefCol { get { return "HD_SETOR.tipoComando"; } }
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

