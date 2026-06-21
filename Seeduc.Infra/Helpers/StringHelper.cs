namespace Seeduc.Infra.Helpers
{
    using System.Collections.Generic;
    using Seeduc.Infra.Extensions;

    public static class StringHelper
    {
        public static ICollection<string> Clean(ICollection<string> words)
        {
            var list = new List<string>();

            foreach (var word in words)
            {
                list.Add(Clean(word));
            }

            return list;
        }

        public static string Clean(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return word;
            }

            return word.RemoveDiacritics().RemoveDuplicateSpaces().ToLower();
        }
    }
}