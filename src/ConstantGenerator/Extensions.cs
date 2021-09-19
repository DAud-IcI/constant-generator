using System.Linq;

namespace System.Collections.Generic
{
    public static class Extensions
    {
        public static TValue GetMaybe<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default)
        {
            return dictionary.TryGetValue(key, out var result)
                ? result
                : defaultValue;
        }

        /// <summary>
        /// Converts snake_case, kebab-case and scene.case to PascalCase.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string text) =>
            string.Join(
                string.Empty,
                text.Split('-', '_', '.').Select(word => word[..1].ToUpper()));
    }
}