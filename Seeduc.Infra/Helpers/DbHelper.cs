namespace Seeduc.Infra.Helpers
{
    using System.Globalization;
    using System.Text.RegularExpressions;

    public static class DbHelper
    {
        public static string ConvertToColumnConvention(string entityProperty)
        {
            if (string.IsNullOrEmpty(entityProperty))
            {
                return entityProperty;
            }

            return Regex
                .Replace(entityProperty, "([A-Z])", " $1", RegexOptions.Compiled)
                .Trim()
                .Replace(" ", "_")
                .ToUpper();
        }

        public static string ConvertToEntityConvention(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
            {
                return columnName;
            }

            var brazil = CultureInfo.CreateSpecificCulture("pt-BR");

            return brazil
                .TextInfo
                .ToTitleCase(columnName.Replace("_", " "))
                .Replace(" ", string.Empty).Trim();
        }
    }
}