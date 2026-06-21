namespace Seeduc.Infra.Validation
{
    using System.Text.RegularExpressions;
    using Seeduc.Infra.Helpers;
    using Seeduc.Infra.Services;

    public static class TextValidator
    {
        public static bool HasCharRepetition(string text, int numberOfRepetitions)
        {
            if (numberOfRepetitions <= 0)
            {
                return false;
            }

            var pattern = string.Format(
                @"(\w)\1{{{0},}}", 
                numberOfRepetitions - 1);
            var regex = new Regex(pattern);

            return regex.IsMatch(text);
        }

        public static bool HasForbiddenWords(string text, IForbiddenWordsCollector forbiddenWordsCollector)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            var forbiddenWords = StringHelper.Clean(forbiddenWordsCollector.GetWords());
            var cleanedText = StringHelper.Clean(text);

            foreach (var forbiddenWord in forbiddenWords)
            {
                if (cleanedText.Contains(forbiddenWord))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasNumbers(string text)
        {
            var regex = new Regex(@"(\d)");

            return regex.IsMatch(text);
        }
    }
}