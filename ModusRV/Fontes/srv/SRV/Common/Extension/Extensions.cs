using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Data;

namespace SRV.Common.Extension
{
    public static class Extensions
    {

        /// <summary>
        /// Converte um objeto IList para um SelectList para montagem de combobox. Key e Value são do tipo string
        /// </summary>
        /// <typeparam name="TItem">Tipo do objeto da IList</typeparam>
        /// <param name="values">IList</param>
        /// <param name="key">define qual o atributo key</param>
        /// <param name="value">define qual o atributo value</param>
        /// <returns></returns>
        public static SelectList ToSelectList<TItem>(this IList<TItem> values,
            Expression<Func<TItem, string>> key, Expression<Func<TItem, string>> value)
        {
            var Key = key.Compile();
            var Value = value.Compile();

            Dictionary<string, string> list = values.ToDictionary(Key, Value);

            return new SelectList(list, "Key", "Value");
        }

        /// <summary>
        /// Converte um objeto IList para um SelectList para montagem de combobox. Key e Value são do tipo string
        /// </summary>
        /// <typeparam name="TItem">Tipo do objeto da IList</typeparam>
        /// <param name="values">IList</param>
        /// <param name="key">define qual o atributo key</param>
        /// <param name="value">define qual o atributo value</param>
        /// <param name="selectedValue">valor selecionado no SelectList</param>
        /// <returns></returns>
        public static SelectList ToSelectList<TItem>(this IList<TItem> values,
            Expression<Func<TItem, string>> key, Expression<Func<TItem, string>> value, object selectedValue)
        {
            var Key = key.Compile();
            var Value = value.Compile();

            Dictionary<string, string> list = values.ToDictionary(Key, Value);

            return new SelectList(list, "Key", "Value", selectedValue);
        }

        /// <summary>
        /// Converte um objeto IList para um SelectList para montagem de list com mais de uma item selecionado.
        /// Key e Value são do tipo string
        /// </summary>
        /// <typeparam name="TItem">Tipo do objeto da IList</typeparam>
        /// <param name="values">IList</param>
        /// <param name="key">define qual o atributo key</param>
        /// <param name="value">define qual o atributo value</param>
        /// <param name="selectedValue">valores selecionados no MultiSelectList</param>
        /// <returns></returns>
        public static MultiSelectList ToMultiSelectList<TItem>(this IList<TItem> values,
            Expression<Func<TItem, string>> key, Expression<Func<TItem, string>> value, IEnumerable selectedValue)
        {
            var Key = key.Compile();
            var Value = value.Compile();

            Dictionary<string, string> list = values.ToDictionary(Key, Value);

            return new MultiSelectList(list, "Key", "Value", selectedValue);
        }

        /// <summary>
        /// Converte um objeto IList para um SelectList para montagem de combobox. Key é int e Value string
        /// </summary>
        /// <typeparam name="TItem">Tipo do objeto da IList</typeparam>
        /// <param name="values">IList</param>
        /// <param name="key">define qual o atributo key</param>
        /// <param name="value">define qual o atributo value</param>
        /// <returns></returns>
        public static SelectList ToSelectList<TItem>(this IList<TItem> values,
            Expression<Func<TItem, int>> key, Expression<Func<TItem, string>> value)
        {
            var Key = key.Compile();
            var Value = value.Compile();

            Dictionary<int, string> list = values.ToDictionary(Key, Value);

            return new SelectList(list, "Key", "Value");
        }

        /// <summary>
        /// Converte um objeto IList para um SelectList para montagem de combobox. Key é int e Value string
        /// </summary>
        /// <typeparam name="TItem">Tipo do objeto da IList</typeparam>
        /// <param name="values">IList</param>
        /// <param name="key">define qual o atributo key</param>
        /// <param name="value">define qual o atributo value</param>
        /// <param name="selectedValue">valor selecionado no SelectList</param>
        /// <returns></returns>
        public static SelectList ToSelectList<TItem>(this IList<TItem> values,
            Expression<Func<TItem, int>> key, Expression<Func<TItem, string>> value, object selectedValue)
        {
            var Key = key.Compile();
            var Value = value.Compile();

            Dictionary<int, string> list = values.ToDictionary(Key, Value);

            return new SelectList(list, "Key", "Value", selectedValue);
        }

        /// <summary>
        /// Converte um objeto IList para um MultiSelectList para montagem de listbox com mais de um item selecionado.
        /// Key é int e Value string
        /// </summary>
        /// <typeparam name="TItem">Tipo do objeto da IList</typeparam>
        /// <param name="values">IList</param>
        /// <param name="key">define qual o atributo key</param>
        /// <param name="value">define qual o atributo value</param>
        /// <param name="selectedValue">valores selecionados no MultiSelectList</param>
        /// <returns></returns>
        public static MultiSelectList ToMultiSelectList<TItem>(this IList<TItem> values,
            Expression<Func<TItem, int>> key, Expression<Func<TItem, string>> value, IEnumerable selectedValue)
        {
            var Key = key.Compile();
            var Value = value.Compile();

            Dictionary<int, string> list = values.ToDictionary(Key, Value);

            return new MultiSelectList(list, "Key", "Value", selectedValue);
        }

        /// <summary>
        /// Retorna o texto do atributo Description associado a um elemento de um Enum.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string ToDescription<TEnum>(this TEnum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return en.ToString();
        }

        public static SelectList ToSelectList<TEnum>(this TEnum enumObj)
        {
            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new { ID = e, Name = e.ToDescription() };

            return new SelectList(values, "Id", "Name", enumObj);
        }

        /// <summary>
        /// Verificar se um DataRecord (ex: OracleDataReader) possui uma determinada coluna
        /// no seu resultado.
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

    }
}