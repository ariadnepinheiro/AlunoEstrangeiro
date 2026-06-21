namespace Seeduc.Infra.Extensions
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static int CountWords(this string value)
        {
            return ExtractWords(value).Length;
        }

        public static string[] ExtractWords(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new string[0];
            }

            return value.Split(
                new[]
                {
                    " "
                },
                StringSplitOptions.RemoveEmptyEntries);
        }

        public static bool IsNotNullOrEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static string RemoveDiacritics(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            var normalizedString = value.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            for (var i = 0; i < normalizedString.Length; i++)
            {
                var c = normalizedString[i];

                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString();
        }

        public static string RemoveDuplicateSpaces(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return Regex.Replace(value, @"[\s]+", " ", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        }
    }
}