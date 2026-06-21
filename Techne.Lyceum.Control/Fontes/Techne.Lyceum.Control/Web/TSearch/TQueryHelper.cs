using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Techne.Web
{
    internal class TQueryHelper
    {
        /// <summary>
        ///   Cria um objeto de um determinado tipo
        /// </summary>
        /// <param name = "typeName"></param>
        /// <returns></returns>
        internal static object CreateObject(string typeName)
        {
            Type queryObjectType = null;
            object queryObject = null;

            try
            {
                queryObjectType = System.Web.Compilation.BuildManager.GetType(typeName, false, true);
            }
            catch
            {
            }

            if (queryObjectType != null)
            {
                queryObject = queryObjectType.Assembly.CreateInstance(queryObjectType.FullName);
            }

            return queryObject;
        }

        internal static object ExecuteQuery(string typeName, string methodName, IDictionary<string, object> pars, object key, int maxRows)
        {
            object ret = null;
            object queryObject = null;

            try
            {
                queryObject = CreateObject(typeName);
            }
            catch
            {
            }

            if (queryObject != null)
            {
                ret = ExecuteMethod(queryObject, methodName, pars, key, maxRows);
            }

            return ret;
        }

        internal static Hashtable GetRowJSON(string typeName, string methodName, IDictionary<string, object> pars, object key)
        {
            object ret = null;
            Hashtable json = null;

            ret = ExecuteQuery(typeName, methodName, pars, key, 1);

            DataView dv = null;
            if (ret is DataTable)
            {
                dv = ((DataTable)ret).DefaultView;
            }
            else if (ret is DataView)
            {
                dv = (DataView)ret;
            }

            if (dv != null && dv.Count > 0)
            {
                json = new Hashtable();
                foreach (DataColumn col in dv.Table.Columns)
                {
                    json.Add(col.ColumnName.ToLower(), dv[0][col.ColumnName] == DBNull.Value ? null : dv[0][col.ColumnName]);
                }
            }

            return json;
        }

        private static object ExecuteMethod(object queryObject, string methodName, IDictionary<string, object> pars, object key, int maxRows)
        {
            object ret = null;
            if (queryObject == null)
            {
                return null;
            }

            var mi = queryObject.GetType().GetMethod(methodName, new[] { typeof (IDictionary<string, object>), typeof (object), typeof (int) });
            if (mi != null)
            {
                return mi.Invoke(queryObject, new[] { pars, key, maxRows });
            }

            mi = queryObject.GetType().GetMethod(methodName, new[] { typeof (IDictionary<string, object>), typeof (object) });
            if (mi != null)
            {
                ret = mi.Invoke(queryObject, new[] { pars, key });
            }

            return ret;
        }
    }
}