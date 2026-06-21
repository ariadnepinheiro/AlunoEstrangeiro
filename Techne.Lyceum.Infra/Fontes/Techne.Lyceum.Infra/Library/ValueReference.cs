using System;
using System.Globalization;
using System.Web.UI;
using Techne.Controls;

namespace Techne
{
    internal abstract class ValueReference
    {
        private readonly string reference;

        protected ValueReference(string reference)
        {
            this.reference = reference;
        }

        public string Reference
        {
            get
            {
                return this.reference;
            }
        }

        /// <summary>
        ///   Faz o parse de uma string no formato "[referência-1], [referência-2]... [referência-n]",
        ///   onde cada [referência-n] está no formato "[valor]", "#[controle]#", "$[campo]$" ou "@[parâmetro]@".
        ///   Devolve um array contendo o valor de cada uma dessas referências.
        /// </summary>
        /// <param name = "start">
        ///   Controle que define o escopo da busca (IRecordContainer e a TPage).
        ///   Pode ser o próprio IRecordContainer.
        /// </param>
        public static object[] GetValues(string s, Control start, Type[] types)
        {
            return GetValues(ParseList(s), start, types);
        }

        /// <summary>
        ///   Recebe um array de referências e devolve um array de mesmo
        ///   tamanho contendo o valor de cada uma dessas referências.
        /// </summary>
        /// <param name = "start">
        ///   Controle que define o escopo da busca (IRecordContainer e a TPage).
        ///   Pode ser o próprio IRecordContainer.
        /// </param>
        public static object[] GetValues(ValueReference[] references, Control start, Type[] types)
        {
            if (start == null)
            {
                throw new ArgumentNullException();
            }

            var recordContainer = start is IRecordContainer ? (IRecordContainer)start : TControl.GetRecordContainer(start);
            var page = start.Page as TPage;

            var result = new object[references.Length];

            for (var i = 0; i < references.Length; i++)
            {
                try
                {
                    if (references[i] is DirectValueReference)
                    {
                        result[i] = ((DirectValueReference)references[i]).GetValue(types[i]);
                    }
                    else if (references[i] is ControlReference)
                    {
                        result[i] = ((ControlReference)references[i]).GetValue(start);
                    }
                    else if (references[i] is FieldReference)
                    {
                        if (recordContainer == null)
                        {
                            throw new ArgumentException("O controle não está contido em um manager.");
                        }

                        result[i] = ((FieldReference)references[i]).GetValue(recordContainer);
                    }
                    else if (references[i] is ParameterReference)
                    {
                        if (page == null)
                        {
                            throw new ArgumentException("O controle não está contido em uma TPage.");
                        }

                        if (types == null)
                        {
                            throw new ArgumentException();
                        }

                        result[i] = ((ParameterReference)references[i]).GetValue(page, types[i]);
                    }
                    else
                    {
                        throw new NotImplementedException("ValueReference.GetValues(): O tipo " + references[i].GetType().FullName + " não foi tratado.");
                    }
                }
                catch (Exception exc)
                {
                    if (exc is NotImplementedException)
                    {
                        throw;
                    }

                    throw new InvalidOperationException("Houve um problema ao obter o valor da referência '" + references[i] + "'.", exc);
                }
            }

            return result;
        }

        /// <summary>
        ///   Faz o parse de uma string no formato "[valor]", "#[controle]#", "$[campo]$" ou "@[parâmetro]@".
        ///   Devolve, respectivamente, DirectValueReference, ControlReference, FieldReference ou ParameterReference.
        /// </summary>
        public static ValueReference Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }

            if ((s.Length >= 2) && (s[0] == s[s.Length - 1]))
            {
                var token = s.Substring(1, s.Length - 2);
                if (s[0] == '#')
                {
                    return new ControlReference(token);
                }

                if (s[0] == '$')
                {
                    return new FieldReference(token);
                }

                if (s[0] == '@')
                {
                    return new ParameterReference(token);
                }
            }

            return new DirectValueReference(s);
        }

        /// <summary>
        ///   Faz o parse de uma string no formato "[referência-1], [referência-2]... [referência-n]",
        ///   onde cada [referência-n] está no formato "[valor]", "#[controle]#", "$[campo]$" ou "@[parâmetro]@".
        /// </summary>
        public static ValueReference[] ParseList(string s)
        {
            var item = s.Trim().Length > 0 ? s.Split(',') : new string[0];
            var result = new ValueReference[item.Length];
            for (var i = 0; i < item.Length; i++)
            {
                result[i] = Parse(item[i].Trim());
            }

            return result;
        }
    }

    internal sealed class DirectValueReference : ValueReference
    {
        internal DirectValueReference(string reference) : base(reference)
        {
        }

        public override string ToString()
        {
            return this.Reference;
        }

        /// <summary>
        ///   Faz o parse da propriedade Reference, devolvendo um valor no tipo indicado.
        /// </summary>
        public object GetValue(Type type)
        {
            if (typeof (IDbObject).IsAssignableFrom(type))
            {
                if (!typeof (ISpecific).IsAssignableFrom(type))
                {
                    throw new ArgumentException("Existe pelo menos um tipo fornecido derivado de IDbObject que não é derivado de ISpecific.");
                }

                if (type == typeof (VarChar))
                {
                    return DbObject.ToDbObject(this.Reference).ToSpecific(DbType.VarChar);
                }
                else if (type == typeof (Number))
                {
                    decimal d;
                    try
                    {
                        d = decimal.Parse(this.Reference, CultureInfo.InvariantCulture);
                    }
                    catch (Exception exc)
                    {
                        throw new InvalidOperationException("O valor '" + this.Reference + "' não pode ser convertido para Number: " + exc.Message);
                    }

                    return DbObject.ToDbObject(d).ToSpecific(DbType.Number);
                }
                else if (type == typeof (Date))
                {
                    DateTime d;
                    try
                    {
                        d = DateTime.Parse(this.Reference, CultureInfo.InvariantCulture);
                    }
                    catch (Exception exc)
                    {
                        throw new InvalidOperationException("O valor '" + this.Reference + "' não pode ser convertido para Date: " + exc.Message);
                    }

                    return DbObject.ToDbObject(d).ToSpecific(DbType.Date);
                }
                else
                {
                    throw new NotSupportedException("O tipo " + type.Name + " não pode ser inicializado via propriedade ParameterValues.");
                }
            }
            else if (type.IsEnum)
            {
                return Enum.Parse(type, this.Reference, true);
            }
            else
            {
                return Convert.ChangeType(this.Reference, type, CultureInfo.InvariantCulture);
            }
        }
    }

    internal sealed class ControlReference : ValueReference
    {
        internal ControlReference(string reference) : base(reference)
        {
        }

        public override string ToString()
        {
            return "#" + this.Reference + "#";
        }

        /// <summary>
        ///   Busca o controle a partir do NamingContainer no qual controle informado está contido.
        ///   Se o controle não for encontrado, devolve null.
        /// </summary>
        public Control GetControl(Control start)
        {
            Control scope = start, control = null;

            do
            {
                control = scope.FindControl(this.Reference);
                if (control != null)
                {
                    return control;
                }

                scope = scope.NamingContainer;
            }
            while (scope != null);

            return null;
        }

        /// <summary>
        ///   Busca o valor do controle a partir do NamingContainer no qual controle informado está contido.
        ///   Devolve null somente quando o controle é um TDropDownListBase com o valor "TODOS" selecionado.
        /// </summary>
        public IDbObject GetValue(Control start)
        {
            var control = this.GetControl(start);
            if (control == null)
            {
                throw new ArgumentException("O controle '" + this.Reference + "' não foi encontrado.");
            }

            if (!(control is ITControl))
            {
                throw new NotSupportedException("O tipo do controle '" + this.Reference + "' não é suportado.");
            }

            var c = (ITControl)control;

            if ((c.DBValue == null) && !(c is TDropDownListBase))
            {
                // DBValue == null em um controle que não é TDropDownList??
                throw new ApplicationException();
            }

            return c.DBValue != null ? c.DBValue.ToSpecific(c.DataType) : null;
        }
    }

    internal sealed class FieldReference : ValueReference
    {
        internal FieldReference(string reference) : base(reference)
        {
        }

        public override string ToString()
        {
            return "$" + this.Reference + "$";
        }

        /// <summary>
        ///   Busca o valor do campo referenciado no IRecordContainer fornecido.
        ///   Se existir um controle associado a esse campo, devolve o valor do primeiro
        ///   controle nessa situação, dando preferência àqueles com ReadOnly=false.
        ///   Caso não exista controle, mas o campo pertencer à primary key, utiliza o
        ///   valor da primary key. Caso contrário, utiliza o valor armazenado através
        ///   da propriedade StoredValues (PersistColumns na grid).
        /// </summary>
        public IDbObject GetValue(IRecordContainer recordContainer)
        {
            if (recordContainer == null)
            {
                throw new ArgumentNullException();
            }

            ITControl fieldControl;
            try
            {
                fieldControl = TControl.GetColumnControl((Control)recordContainer, this.Reference);
            }
            catch
            {
                fieldControl = null;
            }

            if (fieldControl == null)
            {
                var fieldType = RecordContainerLib.GetFieldDataType(recordContainer, this.Reference);
                return recordContainer[this.Reference].ToSpecific(fieldType);
            }
            else
            {
                return fieldControl.DBValue.ToSpecific(fieldControl.DataType);
            }
        }
    }

    internal sealed class ParameterReference : ValueReference
    {
        internal ParameterReference(string reference) : base(reference)
        {
        }

        public override string ToString()
        {
            return "@" + this.Reference + "@";
        }

        /// <summary>
        ///   Busca o valor do parâmetro referenciado.
        ///   Dá prioridade aos valores que implementam IDbObject (coleção Parameters de TPage)
        ///   sobre aqueles que não implementam (coleção AllParameters de TPage).
        /// </summary>
        /// <param name = "typeWhenNull">
        ///   Tipo ISpecific a ser utilizado quando o valor que implementa IDbObject é nulo.
        /// </param>
        public object GetValue(TPage page, Type typeWhenNull)
        {
            if (page.Parameters.IndexOfKey(this.Reference) >= 0)
            {
                return page.Parameters[this.Reference].ToSpecific(typeWhenNull);
            }
            else if (page.AllParameters.IndexOfKey(this.Reference) >= 0)
            {
                return page.AllParameters[this.Reference];
            }
            else
            {
                throw new InvalidOperationException("O parâmetro '" + this.Reference + "' não foi encontrado na página.");
            }
        }
    }
}