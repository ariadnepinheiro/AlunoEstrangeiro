using System;
using System.Collections;
using System.Data;
using Techne.Data;
using Techne.Library;

namespace Techne
{
    namespace Library
    {
        /// <summary>
        ///   Classe que guarda uma lista de erros
        /// </summary>
        [Serializable]
        public class ErrorList : IEnumerable, ICloneable
        {
            public static string Separator_Def = ", ";

            private readonly ArrayList m_List;

            public ErrorList()
            {
                this.m_List = new ArrayList();
            }

            public ErrorList(string errorString) : this()
            {
                Add(errorString);
            }

            public static ErrorList Empty
            {
                get
                {
                    return new ErrorList();
                }
            }

            public int Count
            {
                get
                {
                    return this.m_List.Count;
                }
            }

            /// <summary>
            ///   Devolve a lista de campos contidos na lista de erros, sem repetições.
            ///   ATENÇÃO: Os nomes serão devolvidos em lowercase.
            /// </summary>
            public string[] FieldList
            {
                get
                {
                    var list = new ArrayList();
                    foreach (Error error in this.m_List)
                    {
                        var field = error.Field.ToLower();
                        if (!list.Contains(field))
                        {
                            list.Add(field);
                        }
                    }

                    return (string[])list.ToArray(typeof (string));
                }
            }

            /// <summary>
            ///   Devolve um array de strings contendo mensagens relacionadas ao campo informado como indexador.
            /// </summary>
            public string[] this[string Field]
            {
                get
                {
                    var aux = new ArrayList();
                    Field = Field.ToUpper();
                    foreach (Error e in this.m_List)
                    {
                        if (e.Field.ToUpper() == Field)
                        {
                            aux.Add(e.Message);
                        }
                    }

                    return (string[])aux.ToArray(typeof (string));
                }
            }

            /// <summary>
            ///   Transforma os erros contidos num DataRow em Techne.Library.ErrorList.
            ///   Inverso do método SetDataRowErrors().
            /// </summary>
            public static ErrorList CreateFromDataRow(DataRow row)
            {
                var errors = new ErrorList();

                if (row.HasErrors)
                {
                    if (row.RowError.Trim().Length > 0)
                    {
                        errors.Add(row.RowError, string.Empty);
                    }

                    foreach (DataColumn column in row.Table.Columns)
                    {
                        var colError = row.GetColumnError(column);
                        if (colError != string.Empty)
                        {
                            errors.Add(colError, column.ColumnName);
                        }
                    }
                }

                return errors;
            }

            public override string ToString()
            {
                return this.ToString(Separator_Def);
            }

            public void Add(string message, string field)
            {
                this.m_List.Add(new Error(message, field));
            }

            public void Add(string ErrorString)
            {
                this.AddErrorString(ErrorString);
            }

            public void Add(ErrorList list)
            {
                foreach (Error err in list)
                {
                    this.Add(err.Message, err.Field);
                }
            }

            public void CopyToConnection(TConnection connection)
            {
                foreach (Error error in this.m_List)
                {
                    connection.SetError(error.Message, error.Field);
                }
            }

            public void CopyToDataRow(DataRow row)
            {
                this.CopyToDataRow(row, Separator_Def);
            }

            // TODO Alterar para não sobreescrever as mensagens eventualmente já existentes.
            public void CopyToDataRow(DataRow row, string separator)
            {
                var unidentCol = new ArrayList(this[string.Empty]);

                // Também coloca em RowError as mensagens relacionadas a colunas não encontradas na tabela.
                foreach (var field in this.FieldList)
                {
                    if (field != string.Empty && row.Table.Columns.IndexOf(field) < 0)
                    {
                        unidentCol.AddRange(this[field]);
                    }
                }

                row.RowError = StrLib.EnumerableToStr(unidentCol, separator);

                foreach (DataColumn column in row.Table.Columns)
                {
                    row.SetColumnError(column, StrLib.EnumerableToStr(this[column.ColumnName], separator));
                }
            }

            public IEnumerator GetEnumerator()
            {
                return this.m_List.GetEnumerator();
            }

            public string ToString(string messageDelimiter)
            {
                var messages = TechLib.EnumerableItemProperty(this.m_List, "Message");
                if (messages.Length != 0)
                {
                    return StrLib.EnumerableToStr(messages, messageDelimiter);
                }

                return string.Empty;
            }

            private void AddErrorString(string s)
            {
                if (s == null)
                {
                    return;
                }

                int p;
                string msg;

                while (s.Length > 0)
                {
                    p = s.IndexOf('|');
                    if (p < 0)
                    {
                        this.Add(s, string.Empty);
                        return;
                    }

                    msg = s.Substring(0, p);
                    s = s.Substring(p + 1);

                    p = s.IndexOf('|');
                    if (p < 0)
                    {
                        this.Add(msg, s);
                        return;
                    }

                    this.Add(msg, s.Substring(0, p));
                    s = s.Substring(p + 1);
                }
            }

            object ICloneable.Clone()
            {
                var errList = new ErrorList();
                errList.Add(this);
                return errList;
            }

            [Serializable]
            private struct Error
            {
                private readonly string m_Field;

                private readonly string m_Message;

                public Error(string message, string field)
                {
                    if ((message == null || message.Trim().Length == 0) &&
                        (field == null || field.Trim().Length == 0))
                    {
                        throw new ArgumentException("Pelo menos um dos parâmetros (message ou field) deve ser não nulo.");
                    }

                    this.m_Message = message;
                    this.m_Field = field;
                }

                public string Field
                {
                    get
                    {
                        return this.m_Field;
                    }
                }

                public string Message
                {
                    get
                    {
                        return this.m_Message;
                    }
                }

                public override string ToString()
                {
                    return this.m_Message + "|" + this.m_Field;
                }
            }
        }
    }

    internal class Errors
    {
        public static void CopyToDataRow(string errorString, DataRow row)
        {
            CopyToDataRow(errorString, row, Library.ErrorList.Separator_Def);
        }

        public static void CopyToDataRow(string errorString, DataRow row, string separator)
        {
            new ErrorList(errorString).CopyToDataRow(row);
        }
    }
}