namespace FileUpload.Common.Utils
{
    public class StringUtils
    {
        public static string GetWordsFromString(string text, int numberOfWords)
        {
            if (String.IsNullOrWhiteSpace(text) || numberOfWords <= 0)
                return text;

            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return String.Join(' ', words.Take(numberOfWords).ToArray());
        }
    }
}
