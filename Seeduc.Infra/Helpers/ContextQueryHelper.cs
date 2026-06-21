namespace Seeduc.Infra.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Seeduc.Infra.Data;

    public static class ContextQueryHelper
    {
        internal static ContextQuery Merge(Queue<ContextQuery> contextQueries)
        {
            if (contextQueries == null
                || contextQueries.Count == 0)
            {
                return new ContextQuery();
            }

            var commands = new StringBuilder();
            var parameters = new ContextQueryParameters();
            var i = 1;

            while (contextQueries.Count > 0)
            {
                var contextQuery = contextQueries.Dequeue();

                if (ContextQuery.IsEmpty(contextQuery))
                {
                    continue;
                }

                var command = string.Format(
                    "-- CONTEXT QUERY {0}\n{1};\n",
                    i,
                    contextQuery.Command);

                if (contextQuery.Parameters.Count > 0)
                {
                    var contextQueryParameters = contextQuery.Parameters.OrderByDescending(x => x.Name.Length);

                    foreach (var contextQueryParameter in contextQueryParameters)
                    {
                        var newContextQueryParameter = (ContextQueryParameter)contextQueryParameter.Clone();
                        var newParameterName = string.Format(
                            "{0}{1}",
                            contextQueryParameter.Name,
                            i);

                        command = command.Replace(contextQueryParameter.Name, newParameterName);

                        newContextQueryParameter.Name = newParameterName;

                        parameters.Add(newContextQueryParameter);
                    }
                }

                commands.Append(command);

                i++;
            }

            return new ContextQuery
                   {
                       Command = commands.ToString(),
                       ContextQueryType = ContextQueryType.Sql,
                       Parameters = parameters
                   };
        }
    }
}