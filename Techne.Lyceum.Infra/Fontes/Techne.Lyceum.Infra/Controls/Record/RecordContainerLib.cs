using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Data;
using Techne.Library;

namespace Techne.Controls
{
    public enum RecordManagerMode
    {
        View, 
        Edit, 
        New
    }

    internal class RecordContainerLib
    {
        public static bool CanCommit(IRecordContainer container)
        {
            // O container precisa estar em modo New ou Edit
            if (container.Mode != RecordManagerMode.New &&
                container.Mode != RecordManagerMode.Edit)
            {
                return false;
            }

            var permission = TControl.GetPermission(container);
            if (permission != null && permission.ReadOnly)
            {
                return false;
            }

            var childManagers = TControl.GetChildManagers((Control)container);

            // Se não existirem managers filhos, pode comitar
            if (childManagers.Length == 0)
            {
                return true;
            }

                // Se existirem managers filhos, pelo menos um deve permitir Commit e nenhum deve ser root
            else
            {
                var success = false;
                foreach (var childManager in childManagers)
                {
                    // Se pelo menos um manager filho for root, o container não pode comitar
                    if (childManager.IsRoot)
                    {
                        return false;
                    }

                    // Se pelo menos um manager filho permitir commit, já existem condições para o container poder comitar
                    if (ContainerManagerLib.CanCommit(childManager))
                    {
                        success = true;
                    }
                }

                return success;
            }
        }

        public static bool CanDelete(IRecordContainer container)
        {
            // Para deletar, precisa estar em View ou Edit
            if (container.Mode != RecordManagerMode.View &&
                container.Mode != RecordManagerMode.Edit)
            {
                return false;
            }

            // O container não pode apresentar um registro que já esteja deletado
            if (container.Deleted)
            {
                return false;
            }

            var permission = TControl.GetPermission(container);
            if (permission != null && permission.ReadOnly)
            {
                return false;
            }

            var childManagers = TControl.GetChildManagers((Control)container);

            // Se existirem managers filhos, todos devem permitir deleção
            if (childManagers.Length != 0)
            {
                foreach (var childManager in childManagers)
                {
                    if (!ContainerManagerLib.CanDelete(childManager))
                    {
                        return false;
                    }
                }
            }

            // O container poderá deletar se existir registro válido
            return container.PrimaryKeyValues != null;
        }

        public static bool CanEnterAddNew(IRecordContainer container)
        {
            // Se o container não estiver em View, não pode entrar em New
            if (container.Mode != RecordManagerMode.View)
            {
                return false;
            }

            var permission = TControl.GetPermission(container);
            if (permission != null && permission.ReadOnly)
            {
                return false;
            }

            // Se existir um manager filho que não permita New, o container não pode entrar em New
            foreach (var childManager in TControl.GetChildManagers((Control)container))
            {
                if (!ContainerManagerLib.CanEnterAddNew(childManager))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///   Um container pode entrar em modo Edit quando ele mesmo puder entrar
        ///   em modo Edit e seus managers puderem entrar em modo Edit ou New.
        /// </summary>
        public static bool CanEnterEdit(IRecordContainer container)
        {
            // Se o container não estiver em View, não pode entrar em Edit
            if (container.Mode != RecordManagerMode.View)
            {
                return false;
            }

            // O container não pode apresentar um registro que já esteja deletado
            if (container.Deleted)
            {
                return false;
            }

            var permission = TControl.GetPermission(container);
            if (permission != null && permission.ReadOnly)
            {
                return false;
            }

            var childManagers = TControl.GetChildManagers((Control)container);

            foreach (var childManager in childManagers)
            {
                if (!ContainerManagerLib.CanEnterEdit(childManager))
                {
                    // Se a chamada aceita New, mas o manager filho não puder entrar nesse modo, devolve false
                    if (!ContainerManagerLib.CanEnterAddNew(childManager))
                    {
                        return false;
                    }
                }
            }

            return childManagers.Length != 0 || container.PrimaryKeyValues != null;
        }

        public static bool CanUndelete(IRecordContainer container)
        {
            if (!container.Deleted)
            {
                return false;
            }

            var permission = TControl.GetPermission(container);
            if (permission != null && permission.ReadOnly)
            {
                return false;
            }

            // O container pode ter um registro removido, mas estar em modo AddNew (RecordManager)
            return container.Mode == RecordManagerMode.View;
        }

        /// <summary>
        ///   Copia os valores dos TControls sob o IRecordContainer informado para o DataRow.
        /// </summary>
        public static void CopyTControlsToDataRow(IRecordContainer container, DataRow row, bool useStoredValues)
        {
            if (!(container is Control))
            {
                throw new ArgumentException("O container informado não é do tipo " + typeof (Control).FullName + ".");
            }

            var table = row.Table;

            row.BeginEdit();

            foreach (DataColumn col in table.Columns)
            {
                var controls = TControl.GetChildTControls((Control)container, col.ColumnName, false);
                if (controls.Length > 1)
                {
                    throw new Exception("Foi encontrado mais de um controle associado à coluna '" + col.ColumnName + "'.");
                }

                if (controls.Length == 1)
                {
                    var ctl = controls[0];

                    // Somente copia o valor do controle para o DataRow se a coluna correspondente
                    // for do tipo TDataColumn e ela for da tabela principal (IsAux = false).
                    if (col is TDataColumn && !((TDataColumn)col).IsAux)
                    {
// if(col.DataType != ctl.DataType)

// throw new InvalidOperationException(((Control)ctl).ID + ".DataType (DbType." + ctl.DataType + ") não é compatível com o tipo da coluna " + col.ColumnName + " (" + col.DataType.FullName + ")");
                        row[col] = ctl.DBValue.ToObject();

                        if (ctl is ITControlEditable)
                        {
                            var valueErr = ((ITControlEditable)ctl).GetValueError();
                            if (valueErr != string.Empty)
                            {
                                row.SetColumnError(col, valueErr);
                            }
                        }
                    }
                }
                else if (useStoredValues)
                {
                    var manager = TControl.GetManager((Control)container);
                    if (StrLib.IndexOfInsensitive(col.ColumnName, manager.StoreColumns) >= 0)
                    {
                        row[col] = container[col.ColumnName].ToObject();
                    }
                }
            }

            row.EndEdit();
        }

        /// <summary>
        ///   Distribui as mensagens de erro contidas num DataRow pelos controles correspondentes.
        ///   As mensagens não relacionadas a colunas ou relacionadas a colunas que não possuam controles
        ///   correspondentes serão adicionadas à lista de erros do container.
        /// </summary>
        public static void DistributeErrorMessages(IRecordContainerInternal container, 
                                                   DataRow row)
        {
            DistributeErrorMessages(container, ErrorList.CreateFromDataRow(row));
        }

        /// <summary>
        ///   Distribui as mensagens de erro pelos controles correspondentes.
        ///   As mensagens não relacionadas a colunas ou relacionadas a colunas que não possuam controles
        ///   correspondentes serão adicionadas à lista de erros do container.
        /// </summary>
        public static void DistributeErrorMessages(IRecordContainerInternal container, 
                                                   ErrorList errors)
        {
            if (!(container is Control))
            {
                throw new ArgumentException("O container informado não é do tipo " + typeof (Control).FullName + ".");
            }

            var generalMsgs = new ArrayList();

            foreach (var field in errors.FieldList)
            {
                if (field.Length == 0)
                {
                    generalMsgs.AddRange(errors[string.Empty]);
                }
                else
                {
                    // Busca, inicialmente, os ITControl's relacionados ao campo que são ReadOnly = false
                    var fieldControls = TControl.GetChildTControls((Control)container, field, true);

// Se não encontrou nenhum ITControl editável relacionado ao campo, busca entre os ITControl's com ReadOnly = true
                    if (fieldControls.Length == 0)
                    {
                        fieldControls = TControl.GetChildTControls((Control)container, field, false);
                    }

                    if (fieldControls.Length == 0)
                    {
                        // Se não encontrou ITControl relacionado ao campo, trata-o como erro genérico
                        generalMsgs.Add(field + ": " + StrLib.EnumerableToStr(errors[field], ", "));
                    }
                    else
                    {
                        // Seta a mensagem nos ITControl's relacionados ao campo
                        foreach (var fieldControl in fieldControls)
                        {
                            if (((Control)fieldControl).Visible && fieldControl.ControlMessageType != ControlMessageType.None)
                            {
                                fieldControl.Msg = StrLib.EnumerableToStr(errors[field], "<BR/>");
                            }
                            else
                            {
                                generalMsgs.Add(field + ": " + StrLib.EnumerableToStr(errors[field], ", "));
                            }
                        }
                    }
                }
            }

            // Adiciona os erros genéricos à lista de erros do container
            foreach (string generalMsg in generalMsgs)
            {
                container.AddMessage(generalMsg, true);
            }
        }

        /// <summary>
        ///   Chama RecordEditorLib.EnterEditionMode() e seta o Mode do container para AddNew.
        ///   Chama ContainerManagerLib.EnterAddNewMode() para cada um dos managers filhos.
        /// </summary>
        public static void EnterAddNewMode(IRecordContainerInternal container)
        {
            var manager = container is IContainerManager ? (IContainerManager)container
                              : TControl.GetManager((Control)container);

            if (!RecordContainerLib.CanEnterAddNew(container))
            {
                throw new InvalidOperationException("O manager " + ((Control)manager).UniqueID + " não pode entrar em modo EnterAddNew");
            }

            RecordEditorLib.EnterEditionMode(container, manager.Table);
            container.SetMode(RecordManagerMode.New, ContainerManagerAction.EnterAddNew);

            foreach (IContainerManagerInternal childManager in TControl.GetChildManagers((Control)container))
            {
                ContainerManagerLib.EnterAddNewMode(childManager);
                childManager.IsRoot = false;
            }
        }

        /// <summary>
        ///   Chama RecordEditorLib.EnterEditionMode() e seta o Mode do container para Edit.
        ///   Chama ContainerManagerLib.EnterEditMode() para cada um dos managers filhos.
        /// </summary>
        public static void EnterEditMode(IRecordContainerInternal container)
        {
            var manager = container is IContainerManager ? (IContainerManager)container
                              : TControl.GetManager((Control)container);

            if (!RecordContainerLib.CanEnterEdit(container))
            {
                throw new InvalidOperationException("O manager " + ((Control)manager).UniqueID + " não pode entrar em modo EnterEdit");
            }

            // A verificação container.PrimaryKeyValues != null não é feita porque
            // o container pode não ter registro, mas pode ter managers internos.
            RecordEditorLib.EnterEditionMode(container, manager.Table);
            container.SetMode(RecordManagerMode.Edit, ContainerManagerAction.EnterEdit);

            foreach (IContainerManagerInternal childManager in TControl.GetChildManagers((Control)container))
            {
                ContainerManagerLib.EnterEditMode(childManager);
                childManager.IsRoot = false;
            }
        }

        /// <summary>
        ///   Obtém a lista de colunas que possuem controles associados ao DataTable informado, sem repetição.
        ///   Os nomes são retornados em lowercase.
        /// </summary>
        /// <param name = "dataTable">DataTable no qual as colunas devem estar contidas. Se for informado
        ///   null para este parâmetro, não realiza a verificação de existência no DataTable.</param>
        public static string[] GetColumnNames(IRecordContainer container, TDataTable dataTable)
        {
            var columnNames = new ArrayList();

            foreach (var control in TControl.GetChildTControls((Control)container))
            {
                var lower = control.ColumnName.ToLower();
                if (lower.Length > 0)
                {
                    if ((dataTable == null || dataTable.ContainsColumn(lower)) && !columnNames.Contains(lower))
                    {
                        columnNames.Add(lower);
                    }
                }
            }

            return (string[])columnNames.ToArray(typeof (string));
        }

        public static IRecordContainer[] GetDirectChildContainers(Control container)
        {
            var containers = (IRecordContainer[])TechLib.FindControls(typeof (IRecordContainer), container, false, new[] { typeof (IRecordContainer) });
            return containers;
        }

        public static DbType GetFieldDataType(IRecordContainer container, string fieldName)
        {
            var manager = container.Manager;
            if (manager == null)
            {
                throw new ArgumentException("O container informado não pertence a um manager.");
            }

            return DbObject.ToDbType(manager.Table.Columns[fieldName].DataType);
        }

        /// <summary>
        ///   Obtém do banco de dados o registro correspondente à primary key do container informado.
        ///   O registro será trazido para a tabela informada.
        ///   Caso o container não contenha nenhum registro, devolve null.
        ///   Se não encontrar, devolve null também.
        /// </summary>
        /// <param name = "connection">Se informado null, obtém uma conexão a partir do TechneDS que contém a tabela informada.</param>
        /// <param name = "getAllColumns">Se false, busca apenas as colunas que possuem TControl associado no container.</param>
        public static TDataRow GetRecordFromDb(IRecordContainer container, 
                                               TConnection connection, 
                                               TDataTable table, 
                                               bool getAllColumns)
        {
            if (!(container is Control))
            {
                throw new ArgumentException("O container informado não é do tipo " + typeof (Control).FullName + ".");
            }

            if (container.PrimaryKeyValues == null)
            {
                return null;
            }
            else
            {
                string[] columns;
                if (getAllColumns)
                {
                    columns = new string[0];
                }
                else
                {
                    // Adiciona colunas NotNull mesmo se não possuírem TControl's associados. É necessário para Updates.
                    columns = GetColumnNames(container, table);
                }

                var cn = connection;
                if (cn == null)
                {
                    cn = table.CreateConnection();
                }

                cn.Open();

                TDataRow row;
                try
                {
                    row = DataLib2.GetRecord(cn, table, container.PrimaryKeyValues, columns, true);
                }
                catch
                {
                    cn.Rollback();
                    throw;
                }
                finally
                {
                    cn.Close();
                }

                return row;
            }
        }

        /// <summary>
        ///   Devolve o TDataRow correspondente à primary key armazenada no container.
        ///   O manager ao qual o container pertence deve estar associado a um TDataTable que já contenha esse DataRow.
        ///   Devolve null se não existir primary key (DbObject[0]).
        /// </summary>
        public static TDataRow GetRecordFromTable(IRecordContainer container)
        {
            IContainerManager manager;
            if (container is IContainerManager)
            {
                manager = (IContainerManager)container;
            }
            else
            {
                manager = TControl.GetManager((Control)container);
                if (manager == null)
                {
                    throw new InvalidOperationException("Não foi encontrado o manager correspondente ao container informado");
                }
            }

            if (container.PrimaryKeyValues == null)
            {
                throw new InvalidOperationException("Não existe registro corrente no container");
            }

            if (container.PrimaryKeyValues.Length == 0)
            {
                return null;
            }
            else
            {
                var table = manager.Table;
                return table.Rows.Find(container.PrimaryKeyValues);
            }
        }

        public static IList GetState(IRecordContainer container)
        {
            var list = new ArrayList();

            list.Add((int)container.Mode);
            list.Add(container.Changed);
            list.Add(container.Deleted);
            list.Add(container.HistInsertUser);
            list.Add(container.HistInsertStamp);
            list.Add(container.PrimaryKeyValues != null ? DbObject.ToObjectArray(container.PrimaryKeyValues) : null);

            return list;
        }

        public static DbObject IndexerGet(IRecordContainer container, string columnName)
        {
            // Verifica se existe TControl associado à coluna
            // Na primeira tentativa, não restringe.
            var controls = TControl.GetChildTControls((Control)container, columnName, false);

            if (controls.Length > 1)
            {
                // Se encontrou mais de um controle, tenta novamente restringindo somente aos editáveis.
                controls = TControl.GetChildTControls((Control)container, columnName, true);
                if (controls.Length > 1 && container.Mode != RecordManagerMode.View)
                {
                    throw new Exception("Foi encontrado mais de um controle associado à coluna '" + columnName + "' e o container não está em modo View.");
                }
            }

            if (controls.Length == 0)
            {
                var manager = container is IContainerManager ? (IContainerManager)container
                                  : TControl.GetManager((Control)container);
                if (manager == null)
                {
                    throw new InvalidOperationException("O manager do container " + ((Control)container).UniqueID + " não foi encontrado");
                }

                // Verifica se a coluna faz parte da primary key
                var pkIndex = -1;
                if (manager.Table != null && manager.Table.Columns.Contains(columnName))
                {
                    pkIndex = Array.IndexOf(manager.Table.PrimaryKey, manager.Table.Columns[columnName]);
                }

                if (pkIndex >= 0)
                {
                    return container.PrimaryKeyValues[pkIndex];
                }

                    // Verifica se a coluna foi citada em StoreColumns
                else
                {
                    var index = StrLib.IndexOfInsensitive(columnName, manager.StoreColumns);
                    if (index < 0)
                    {
                        throw new ArgumentException("Não foi encontrado controle associado à coluna '" + columnName + "'. " +
                                                    "A coluna também não foi informada na propriedade StoreColumns.");
                    }

                    return container.StoredValues[index];
                }
            }
            else
            {
                return controls[0].DBValue;
            }
        }

        public static void IndexerSet(IRecordContainer container, string columnName, DbObject value)
        {
            var controls = TControl.GetChildTControls((Control)container, columnName, false);

            if (controls.Length == 0)
            {
                throw new ArgumentException("Não foi encontrado controle associado à coluna '" + columnName + "'.");
            }

            foreach (var control in controls)
            {
                control.DBValue = value;
            }
        }

        /// <summary>
        ///   Chama ResetValue() de cada um dos TControl's.
        /// </summary>
        /// <param name = "useKeepValueAfterSaveFlag">Considera o flag KeepValueAfterSave de cada controle.
        ///   Se false, reseta o valor de todos os controles independentemente do valor desse flag.</param>
        public static void ResetTControls(IRecordContainer container, 
                                          bool useKeepValueAfterSaveFlag)
        {
            if (!(container is Control))
            {
                throw new ArgumentException("O container informado não é do tipo " + typeof (Control).FullName + ".");
            }

            foreach (var control in TControl.GetChildTControls((Control)container))
            {
                ((WebControl)control).Enabled = true;

                var tcontrol = control as ITControlEditable;

                if (tcontrol != null && tcontrol.Mode != ControlMode.Edit)
                {
                    throw new InvalidOperationException("O método RecordContainerLib.ResetTControls() não será executado porque o controle " + ((Control)control).UniqueID + " está no modo " + ((ITControlEditable)control).Mode);
                }

                if (!useKeepValueAfterSaveFlag || tcontrol != null && !tcontrol.KeepValueAfterSave)
                {
                    control.ResetValue();
                }
            }
        }

        public static void SetClientValidationAttributes(IRecordContainerInternal container, 
                                                         DataTable table, 
                                                         bool ShowMessageBox, 
                                                         ControlMessageType MessageType, 
                                                         bool EnableClientScript)
        {
            if (!(container is Control))
            {
                throw new ArgumentException("O container informado não é do tipo " + typeof (Control).FullName + ".");
            }

            if (table == null)
            {
                throw new ArgumentNullException();
            }

            var listControls = new ArrayList();

            foreach (DataColumn col in table.Columns)
            {
                var required = false;
                var fieldname = string.Empty;

                if (col is TDataColumn)
                {
                    required = ((TDataColumn)col).NotNull;
                    fieldname = ((TDataColumn)col).GetName();
                }

                var checkDataType = col.DataType == typeof (DateTime) ||
                                    col.DataType == typeof (int) ||
                                    col.DataType == typeof (long) ||
                                    col.DataType == typeof (byte) ||
                                    col.DataType == typeof (decimal) ||
                                    col.DataType == typeof (float) ||
                                    col.DataType == typeof (double);

                foreach (var control in TControl.GetChildTControls((Control)container, col.ColumnName, false))
                {
                    var tcontrol = control as TControlEditable;

                    if (tcontrol != null && !tcontrol.ReadOnly)
                    {
                        if (required && tcontrol.RequiredFieldValidation == ValidationOption.NotSet)
                        {
                            tcontrol.RequiredFieldValidation = ValidationOption.True;
                        }

                        if (checkDataType && tcontrol.RequiredFieldValidation == ValidationOption.NotSet)
                        {
                            tcontrol.DataTypeValidation = ValidationOption.True;
                        }

                        if (fieldname != null && fieldname.Length > 0)
                        {
                            tcontrol.FieldName = fieldname;
                        }

                        tcontrol.EnableClientScript = EnableClientScript;

                        listControls.Add(tcontrol);
                    }

                    if (control.ControlMessageType == ControlMessageType.NotSet)
                    {
                        control.ControlMessageType = MessageType;
                    }
                }
            }

            if (container is WebControl)
            {
                ((WebControl)container).Attributes.Add("validatedcontrols", StrLib.EnumerableToStr(TechLib.EnumerableItemProperty(listControls, "ClientID"), ","));
                if (ShowMessageBox)
                {
                    ((WebControl)container).Attributes.Add("showmessagebox", "true");
                }
            }
        }

        public static void SetControlsDataTypes(ITControl[] controls, DataTable dataTable)
        {
            if (controls == null || dataTable == null)
            {
                throw new ArgumentNullException();
            }

            var columns = dataTable.Columns;
            foreach (var control in controls)
            {
                if (control.ColumnName.Length > 0)
                {
                    if (columns.Contains(control.ColumnName))
                    {
                        var column = columns[control.ColumnName];

// DataType
                        control.DataType = DbObject.ToDbType(column.DataType);

// NullAllowed (para TDropDownListBase)
                        if (control is TDropDownListBase && column is TDataColumn)
                        {
                            ((TDropDownListBase)control).NullAllowed = !((TDataColumn)column).NotNull;
                        }
                    }
                    else if (!TControl.InDesignMode((Control)control))
                    {
                        throw new InvalidOperationException("A coluna '" + control.ColumnName + "' indicada na propriedade " + ((Control)control).ID + ".ColumnName não existe no data table " + dataTable.TableName + ".");
                    }
                }
            }
        }

        public static void SetControlsMode(IRecordContainer container)
        {
            var mode = container.Mode == RecordManagerMode.View ? ControlMode.View : ControlMode.Edit;

            foreach (var control in TControl.GetChildTControls((Control)container))
            {
                if (control is ITControlEditable && (control.ColumnName.Length > 0 || ((ITControlEditable)control).FollowContainerMode))
                {
                    ((ITControlEditable)control).Mode = mode;
                }
            }
        }

        /// <summary>
        ///   Seta os valores dos controles de acordo com o DataRow informado. Seta também a primary key do container.
        ///   Se DataRow for null, limpa o valor dos controles (NÃO utiliza o valor do contexto) e reseta a primary
        ///   key do container.
        ///   As mensagens de erro das colunas que não possuem controle correspondente são adicionadas ao RowError.
        /// </summary>
        public static void SetDataRow(IRecordContainerInternal container, DataRow row)
        {
            if (!(container is Control))
            {
                throw new ArgumentException("O container informado não é do tipo " + typeof (Control).FullName + ".");
            }

            var manager = container is IContainerManager ? (IContainerManager)container
                              : TControl.GetManager((Control)container);
            var storeColumns = manager == null ? new string[0] : manager.StoreColumns;
            var rowValues = new DbObject[storeColumns.Length];

            // A situação DataRowState.Added ocorre no modo AddNew do TDataGridBase
            if (row == null || row.RowState == DataRowState.Added)
            {
                container.SetPrimaryKeyValues(null);
                ResetTControls(container, false);

                for (var i = 0; i < storeColumns.Length; i++)
                {
                    rowValues[i] = DBNull.Value;
                }

                container.SetDeleted(false);
                container.SetHistInfo(string.Empty, DateTime.MinValue);
            }
            else
            {
                var table = row.Table;
                var columns = table.Columns;

                // PrimaryKey do container deve ser setado antes de setar os valores de cada um
                // dos controles nele contidos. TControl.ReplaceControlIdentifiers() assume isso.
                container.SetPrimaryKeyValues(DataLib.GetRowValues(row, table.PrimaryKey));

                // Seta o DBValue de cada um dos controles
                // A primeira versão deste bloco percorria as colunas da tabela, e,
                // para cada uma delas, setava o valor dos controles associados.
                // Isso não funcionava quando um TSearch referenciava um outro controle cuja
                // coluna associada vinha depois da coluna associada ao TSearch no DataTable.
                foreach (var control in TControl.GetChildTControls((Control)container))
                {
                    if (control.ColumnName.Length > 0)
                    {
                        control.DBValue = DbObject.ToDbObject(row[control.ColumnName]);
                        ((WebControl)control).Enabled = true;
                    }
                }

                if (!TControl.InDesignMode((Control)container))
                {
                    DistributeErrorMessages(container, row);

                    // container.SetStoredValues();
                    var tableColumns = table.Columns;
                    for (var i = 0; i < storeColumns.Length; i++)
                    {
                        rowValues[i] = tableColumns.Contains(storeColumns[i]) ? DbObject.ToDbObject(row[storeColumns[i]]) : DBNull.Value;
                    }
                }

                container.SetDeleted(row is TDataRow ? ((TDataRow)row).Deleted : false);
                if (table is TDataTable && ((TDataTable)table).HistoryEnabled)
                {
                    container.SetHistInfo(row["Usuario Cad"].ToString(), row["Stamp Cad"] is DBNull ? DateTime.MinValue : (DateTime)row["Stamp Cad"]);
                }
                else
                {
                    container.SetHistInfo(string.Empty, DateTime.MinValue);
                }
            }

            container.SetStoredValues(rowValues);
        }

        public static void SetMode(IRecordContainerInternal container)
        {
            SetMode(container, RecordManagerMode.View, ContainerManagerAction.Commit, false);
        }

        public static void SetMode(IRecordContainerInternal container, 
                                   RecordManagerMode oldMode, 
                                   ContainerManagerAction action)
        {
            SetMode(container, oldMode, action, true);
        }

        public static void SetState(IRecordContainerInternal container, 
                                    IList stateList)
        {
            var offset = 0;

            container.SetMode((RecordManagerMode)stateList[offset++]);
            container.SetChanged((bool)stateList[offset++]);
            container.SetDeleted((bool)stateList[offset++]);
            container.SetHistInfo((string)stateList[offset++], (DateTime)stateList[offset++]);
            {
// PrimaryKeyValues
                var primaryKeyValues = (object[])stateList[offset++];
                container.SetPrimaryKeyValues(primaryKeyValues != null ? DbObject.ToDbObjectArray(primaryKeyValues) : null);
            }
        }

        public static void WriteDebugInfo(HtmlTextWriter writer, IRecordContainer container)
        {
            writer.WriteLine("<B>Mode: </B>" + container.Mode + "<BR/>");
            writer.WriteLine("<B>Changed: </B>" + container.Changed + "<BR/>");
            writer.WriteLine("<B>PrimaryKeyValues: </B>" + (container.PrimaryKeyValues == null ? "&lt;null&gt;" : "{ " + StrLib.EnumerableToStr(container.PrimaryKeyValues, ", ") + " }") + "<BR/>");
            writer.WriteLine("<B>Deleted: </B>" + container.Deleted + "<BR/>");
        }

        private static void SetMode(IRecordContainerInternal container, 
                                    RecordManagerMode oldMode, 
                                    ContainerManagerAction action, 
                                    bool fireModeChangedEvent)
        {
            switch (container.Mode)
            {
                case RecordManagerMode.New:
                    SetControlsMode(container);
                    ResetTControls(container, false);
                    break;

                case RecordManagerMode.Edit:
                    SetControlsMode(container);
                    break;

                case RecordManagerMode.View:
                    container.SetChanged(false);
                    SetControlsMode(container);
                    break;

                default:
                    throw new InvalidOperationException();
            }

            if (fireModeChangedEvent)
            {
                container.OnPostContainerOperation(new PostContainerOperationEventArgs(container, oldMode, action));
            }
        }
    }
}