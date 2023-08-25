namespace MagicVilla_API.Extentions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Extension method to check if a string is null or empty.
        /// </summary>
        /// <param name="value">The string value to check.</param>
        /// <returns>True if the string is null or empty; otherwise, false.</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            // Use the built-in string.IsNullOrEmpty method for the check.
            return string.IsNullOrEmpty(value);
        }
    }
}
