namespace Seeduc.Infra.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using Seeduc.Infra.Data;
    using Techne;

    public static class ContextQueryConverter
    {
        public static ContextQuery FromTechne(string sql, object[] parameters)
        {
            if (string.IsNullOrEmpty(sql))
            {
                return null;
            }

            var indexes = LocateInterrogations(sql);

            if (IsNotValid(indexes, parameters))
            {
                return null;
            }

            var contextQuery = new ContextQuery();

            if (indexes.Count == 0)
            {
                contextQuery.Command = sql;

                return contextQuery;
            }

            var command = new StringBuilder();
            var startIndex = 0;
            var i = 0;

            while (indexes.Count > 0)
            {
                var index = indexes.Dequeue();
                var parameter = string.Concat("@s", i);
                var parameterValue = parameters[i];

                command.Append(sql.Substring(startIndex, index - startIndex));
                command.Append(parameter);

                contextQuery.Parameters.Add(parameter, parameterValue ?? DBNull.Value);

                startIndex = index + 1;

                i++;
            }

            if (startIndex < sql.Length)
            {
                command.Append(sql.Substring(startIndex, sql.Length - startIndex));
            }

            contextQuery.Command = command.ToString();

            return contextQuery;
        }

        public static ContextQuery FromTechne(string sql, DbObject[] parameters)
        {
            if (parameters == null)
            {
                return FromTechne(sql, (object[])null);
            }

            var objectParameters = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                objectParameters[i] = parameters[i].IsNull ? null : parameters[i].ToObject();
            }

            return FromTechne(sql, objectParameters);
        }

        private static bool IsNotValid(ICollection indexes, ICollection parameters)
        {
            if (indexes.Count == 0
                && parameters != null
                && parameters.Count > 0)
            {
                return true;
            }

            if (indexes.Count > 0
                && parameters == null)
            {
                return true;
            }

            if (indexes.Count > 0
                && parameters != null
                && indexes.Count != parameters.Count)
            {
                return true;
            }

            return false;
        }

        private static Queue<int> LocateInterrogations(string sql)
        {
            int currentIndex;
            var startIndex = 0;
            var indexes = new Queue<int>();

            if (string.IsNullOrEmpty(sql))
            {
                return indexes;
            }

            while ((currentIndex = sql.IndexOf('?', startIndex)) >= 0)
            {
                indexes.Enqueue(currentIndex);

                startIndex = currentIndex + 1;
            }

            return indexes;
        }
    }
}