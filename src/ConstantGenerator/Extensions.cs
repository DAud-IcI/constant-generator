using System.Linq;

namespace System.Collections.Generic
{
    public static class Extensions
    {
        /// <summary>
        /// Converts snake_case, kebab-case and scene.case to PascalCase.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string text) =>
            string.Join(
                string.Empty,
                text
                    .Split('-', '_', '.')
                    .Where(word => word.Length >= 1)
                    .Select(word => word[..1].ToUpper() + word[1..]));
    }
}