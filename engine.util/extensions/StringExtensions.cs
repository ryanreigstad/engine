namespace engine.util.extensions
{
    public static class StringExtensions
    {
        public static string Formatted(this string s, params object[] args)
        {
            return string.Format(s, args);
        }
    }
}
