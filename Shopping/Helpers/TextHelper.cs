namespace Shopping.Helpers
{
    public class TextHelper
    {
        public static string ShortenParagraphToMaxLength(string text, int maxLength)
        {
            if (text.Length <= maxLength)
            {
                return text;
            }

            string[] sentences = text.Split('.');
            if (sentences.Length > 1)
            {
                sentences = sentences.Take(sentences.Length - 1).ToArray();
                text = string.Join(".", sentences);
            }

            return ShortenParagraphToMaxLength(text, maxLength);
        }
    }
}
