namespace Seeduc.Infra.Data
{
    using System.Text;

    public class ContextQuery
    {
        public ContextQuery()
        {
            this.ContextQueryType = ContextQueryType.Sql;
            this.Parameters = new ContextQueryParameters();
        }

        public ContextQuery(string command)
            : this()
        {
            this.Command = command;
        }

        public ContextQuery(string command, ContextQueryParameters parameters)
            : this()
        {
            this.Command = command;
            this.Parameters = parameters;
        }

        public ContextQuery(string command, params ContextQueryParameter[] parameters)
            : this()
        {
            this.Command = command;
            this.Parameters.AddRange(parameters);
        }

        public string Command { get; set; }

        public ContextQueryType ContextQueryType { get; set; }

        public ContextQueryParameters Parameters { get; internal set; }

        public static bool IsEmpty(ContextQuery contextQuery)
        {
            return contextQuery == null
                   || string.IsNullOrEmpty(contextQuery.Command);
        }

        public string ToPlainSql()
        {
            if (IsEmpty(this))
            {
                return string.Empty;
            }

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("\nCommand:\n{0}\n", this.Command);

            if (this.Parameters.Count == 0)
            {
                return stringBuilder.ToString();
            }

            stringBuilder.Append("\nParameters:\n");

            foreach (var parameter in this.Parameters)
            {
                stringBuilder.AppendFormat("{{ Key = {0}, Value = {1} }}\n", parameter.Name, parameter.Value);
            }

            return stringBuilder.ToString();
        }
    }
}