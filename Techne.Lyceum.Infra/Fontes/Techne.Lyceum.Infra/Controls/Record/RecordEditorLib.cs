using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using Techne.Data;
using Techne.Library;

namespace Techne.Controls
{
    internal class RecordEditorLib
    {
        /// <summary>
        ///   Obtém o registro correspondente à primary key do container (se update) ou cria um novo,
        ///   atualiza esse registro com os valores correntes dos controles contidos no container e efetiva
        ///   a operação no banco de dados.
        ///   Este overload não seta a propriedade PrimaryKeyValues e nem a propriedade Mode do container.
        ///   IMPORTANTE: A propriedade EnforceConstraints do DataSet no qual a tabela está contida será
        ///   setada para false.
        /// </summary>
        public static bool Commit(IRecordContainerInternal container, 
                                  TConnectionWritable connection, 
                                  TDataTable table, 
                                  out DbObject[] newPrimaryKeyValues)
        {
            if (!(container is Control))
            {
                throw new ArgumentException("O container informado não é do tipo " + typeof (Control).FullName + ".");
            }

            if (container.Mode != RecordManagerMode.Edit && container.Mode != RecordManagerMode.New)
            {
                throw new InvalidOperationException("Não pode comitar no modo atual.");
            }

            var commit = true;
            var operation = container.Mode == RecordManagerMode.New ? PutOperationEnum.Insert : PutOperationEnum.Update;
            newPrimaryKeyValues = new DbObject[0];

            var permission = TControl.GetPermission(container);
            if (permission.ReadOnly)
            {
                container.AddMessage(TPermission.DenialMessage, true);
                return false;
            }

            var cn = connection;
            if (cn == null)
            {
                cn = table.CreateWritableConnection(permission);
            }

            cn.Open(true);

            try
            {
                // Obtém DataRow pronto para ser efetivado (com novos valores ou alterações)
                var row = container.Mode == RecordManagerMode.New || container.PrimaryKeyValues.Length == 0
                              ? table.NewRow()
                              : table.Rows.Find(container.PrimaryKeyValues);
                if (row == null)
                {
                    throw new ArgumentException("O registro do container não foi encontrado no DataTable informado.");
                }

                // Precisa estar num dataset para setar EnforceConstraints=false.
                bool removeFromDs;
                if (table.BaseDataSet == null)
                {
                    var ds = new DataSet();
                    ds.Tables.Add(table);
                    removeFromDs = true;
                }
                else
                {
                    removeFromDs = false;
                }

                ((DataTable)table).DataSet.EnforceConstraints = false;

                RecordContainerLib.CopyTControlsToDataRow(container, row, false);

                // Dispara PrePutDataRow
                var args = new PrePutDataRowArgs(operation, row, cn, container);
                container.OnPrePutDataRow(args);
                if (args.Row.HasErrors || args.CancelOperation)
                {
                    commit = false;
                }

                if (commit)
                {
                    if (args.SkipRow)
                    {
                        newPrimaryKeyValues = container.PrimaryKeyValues;
                    }
                    else
                    {
                        if (table != null && table.ReadOnly)
                        {
                            throw new InvalidOperationException(
                                "O TDataTable " + table.TableName + " ao qual o manager está associado é ReadOnly " +
                                "(provavelmente criado a partir de uma view). Commits não são permitidos."
                                );
                        }

                        // Se modo AddNew, adiciona o DataRow ao DataTable
                        if (container.Mode == RecordManagerMode.New)
                        {
                            var oldRow = table.Rows.Find(DataLib.GetRowValues(row, row.Table.PrimaryKey));
                            if (oldRow != null)
                            {
                                table.Rows.Remove(oldRow);
                            }

                            table.Rows.Add(row);
                        }

                        table.Put(cn, new DataRow[] { row });

                        if (row.HasErrors)
                        {
                            commit = false;
                        }
                        else
                        {
                            row.AcceptChanges();

                            var willRollback = cn.WillRollback;
                            var postArgs = new PostPutDataRowArgs(operation, row, cn, container);
                            container.OnPostPutDataRow(postArgs);
                            if (row.RowState != DataRowState.Unchanged)
                            {
                                throw new InvalidOperationException("Não é permitido alterar o registro em eventos Post (" + table.TableName + ", " + operation + ").");
                            }

// Verifica se Rollback() foi chamado no entry-point Post.
                            willRollback = !willRollback && cn.WillRollback;

                            if (postArgs.Error.Length > 0)
                            {
                                new ErrorList(postArgs.Error).CopyToDataRow(row);
                                commit = false;
                            }
                            else if (row.HasErrors)
                            {
                                commit = false;
                            }
                            else if (cn.ErrorCount > 0)
                            {
                                cn.GetErrors().CopyToDataRow(row);
                                commit = false;
                            }
                            else if (willRollback && commit)
                            {
                                throw new InvalidOperationException(
                                    "Foi executado um rollback, mas nenhuma mensagem de erro foi informada pelo evento Post. " +
                                    "Sete a propriedade Error do parâmetro do evento Post ao invés de fazer o rollback. "
                                    );
                            }

                            newPrimaryKeyValues = DataLib.GetRowValues(row, row.Table.PrimaryKey);
                        }
                    }
                }

                if (!commit)
                {
                    cn.Rollback();
                    newPrimaryKeyValues = container.PrimaryKeyValues; // Mantém a primary key
                    RecordContainerLib.DistributeErrorMessages(container, row);
                }

                if (removeFromDs)
                {
                    ((DataTable)table).DataSet.Tables.Remove(table);
                }
            }
            catch (Exception exc)
            {
                cn.Rollback();
                throw new Exception(exc.Message, exc);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

            return commit;
        }

        /// <summary>
        ///   O registro correspondente ao registro corrente no container será removido do banco de dados.
        ///   Ele já deve estar contido na tabela informada.
        /// </summary>
        /// <param name = "connection">Se informado null, obtém a conexão do TechneDS no qual a tabela está contida.</param>
        public static bool Delete(IRecordContainerInternal container, 
                                  TConnectionWritable connection, 
                                  TDataTable table)
        {
            if (!(container is Control))
            {
                throw new ArgumentException("O container informado não é do tipo " + typeof (Control).FullName + ".");
            }

            if (container.Mode != RecordManagerMode.Edit && container.Mode != RecordManagerMode.View)
            {
                throw new InvalidOperationException("Operação inválida neste modo.");
            }

            var commit = true;
            var permission = TControl.GetPermission(container);
            if (permission.ReadOnly)
            {
                container.AddMessage(TPermission.DenialMessage, true);
                return false;
            }

            var cn = connection;
            if (cn == null)
            {
                cn = table.CreateWritableConnection(permission);
            }

            cn.Open(true);

            try
            {
                TDataRow row = null;
                if (container.PrimaryKeyValues != null && container.PrimaryKeyValues.Length > 0)
                {
                    row = table.Rows.Find(container.PrimaryKeyValues);
                    if (row == null)
                    {
                        throw new InvalidOperationException(
                            "O registro a ser removido não foi encontrado. " +
                            "PrimaryKeyValues: { " + StrLib.EnumerableToStr(container.PrimaryKeyValues) + " }"
                            );
                    }
                }
                else
                {
                    row = table.NewRow();
                    RecordContainerLib.CopyTControlsToDataRow(container, row, true);
                    table.Rows.Add(row);
                }

                // Dispara PrePutDataRow
                var args = new PrePutDataRowArgs(PutOperationEnum.Delete, row, cn, container);
                container.OnPrePutDataRow(args);
                if (args.Row.HasErrors || args.CancelOperation)
                {
                    commit = false;
                }

                if (!args.SkipRow)
                {
                    if (commit)
                    {
                        if (table != null && table.ReadOnly)
                        {
                            throw new InvalidOperationException(
                                "O TDataTable " + table.TableName + " ao qual o manager está associado é ReadOnly " +
                                "(provavelmente criado a partir de uma view). Deletes não são permitidos."
                                );
                        }

                        if (row.RowState == DataRowState.Added)
                        {
                            throw new InvalidOperationException("Nenhum registro corrente a ser removido.");
                        }

                        row.Delete();
                        if (row.HasErrors)
                        {
                            commit = false;
                        }
                    }

                    if (commit)
                    {
                        table.Put(cn, new DataRow[] { row });
                        if (row.HasErrors)
                        {
                            commit = false;
                        }
                        else
                        {
                            

                            var postArgs = new PostPutDataRowArgs(PutOperationEnum.Delete, null, cn, container);
                            container.OnPostPutDataRow(postArgs);

                            if (postArgs.Error.Length > 0)
                            {
                                container.AddMessage(postArgs.Error, true);
                                commit = false;
                            }
                            else if (cn.ErrorCount > 0)
                            {
                                RecordContainerLib.DistributeErrorMessages(container, cn.GetErrors());
                                commit = false;
                            }
                            else if (cn.WillRollback && commit)
                            {
                                throw new InvalidOperationException(
                                    "Foi executado um rollback, mas nenhuma mensagem de erro foi informada pelo evento Post. " +
                                    "Sete a propriedade Error do parâmetro do evento Post ao invés de fazer o rollback. "
                                    );
                            }

                            
                        }
                    }
                }

                if (!commit)
                {
                    cn.Rollback();
                    RecordContainerLib.DistributeErrorMessages(container, row);
                }
            }
            catch (Exception exc)
            {
                cn.Rollback();
                throw new Exception(exc.Message, exc);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

            return commit;
        }

        /// <summary>
        ///   Verifica se o modo atual do container é diferente de View. Ele NÃO será alterado neste método,
        ///   devendo o chamador encarregar-se de setar container.Mode.
        ///   Limpa as mensagens dos controles, mas mantém seus valores.
        /// </summary>
        public static void EnterEditionMode(IRecordContainer container, TDataTable table)
        {
            if (!(container is Control))
            {
                throw new ArgumentException("O container informado não é do tipo " + typeof (Control).FullName + ".");
            }

            if (container.Mode != RecordManagerMode.View)
            {
                throw new InvalidOperationException("Não pode entrar em modo de edição no modo atual.");
            }

            foreach (var control in TControl.GetChildTControls((Control)container))
            {
                var column = (TDataColumn)table.Columns[control.ColumnName];

                if (column != null)
                {
                    if (control is TTextBox && control.ColumnName.Length > 0)
                    {
                        ((TTextBox)control).MaxLength = column.Size;
                        if (control.DataType == DbType.Number)
                        {
                            ((TTextBox)control).Precision = column.Precision;
                            ((TTextBox)control).Scale = column.Scale;
                            if (column.MinValue != null)
                            {
                                ((TTextBox)control).MinimumValue = Convert.ToString(column.MinValue, CultureInfo.InvariantCulture);
                            }

                            if (column.MaxValue != null)
                            {
                                ((TTextBox)control).MaximumValue = Convert.ToString(column.MaxValue, CultureInfo.InvariantCulture);
                            }
                        }
                    }

                    if (column.IsAux && control is ITControlEditable)
                    {
                        ((ITControlEditable)control).ReadOnly = true;
                    }
                }
            }
        }

        /// <summary>
        ///   Limpa as mensagens genéricas do container e as mensagens dos controles.
        ///   Recoloca os valores do registro corrente do container nos controles. Não realiza nova busca no
        ///   banco de dados.
        /// </summary>
        public static void Rollback(IRecordContainerInternal container, 
                                    TDataTable table)
        {
            if (!(container is Control))
            {
                throw new ArgumentException("O container informado não é do tipo " + typeof (Control).FullName + ".");
            }

            if (container.Mode != RecordManagerMode.Edit && container.Mode != RecordManagerMode.New)
            {
                throw new InvalidOperationException("Não pode cancelar alterações no modo atual.");
            }

            RecordContainerLib.SetDataRow(
                container, 
                container.PrimaryKeyValues == null || container.PrimaryKeyValues.Length == 0
                    ? null
                    : table.Rows.Find(container.PrimaryKeyValues)
                );
        }

        /// <summary>
        ///   O TDataRow correspondente ao registro contido no container já deve existir na tabela.
        /// </summary>
        public static bool Undelete(IRecordContainerInternal container, 
                                    TConnectionWritable connection, 
                                    TDataTable table)
        {
            if (!table.HistoryEnabled)
            {
                throw new InvalidOperationException("Operação de reinserção não disponível para esta tabela.");
            }

            var row = table.Rows.Find(container.PrimaryKeyValues);
            if (row == null)
            {
                throw new InvalidOperationException("O registro do container não foi encontrado na tabela informada.");
            }

            var permission = TControl.GetPermission(container);
            if (permission.ReadOnly)
            {
                container.AddMessage(TPermission.DenialMessage, true);
                return false;
            }

            connection.Open(true);
            try
            {
                var undeleted = row.Undelete(connection);
                if (!undeleted)
                {
                    connection.GetErrors().CopyToDataRow(row);

                    connection.Rollback();
                    RecordContainerLib.DistributeErrorMessages(container, row);
                }

                return undeleted;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
    }
}