using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MoonlitSystem.Strings
{
    public static class StringUtil
    {
        public static string RemoveChars(this string s, IEnumerable<char> separators)
        {
            var sb = new StringBuilder(s);
            foreach (var c in separators) { sb.Replace(c.ToString(), ""); }
            return sb.ToString();
        }

        public static string ReplaceChars(this string s, IEnumerable<char> separators, char newVal)
        {
            var sb = new StringBuilder(s);
            foreach (var c in separators) { sb.Replace(c, newVal); }
            return sb.ToString();
        }

        public static string Truncate(string value, int maxLength)
        {
            return value?.Substring(0, Math.Min(value.Length, maxLength));
        }

        public static string TitleCase(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return string.Empty;
            var cultureInfo = Thread.CurrentThread.CurrentCulture;
            var textInfo = cultureInfo.TextInfo;
            return textInfo.ToTitleCase(title);
        }

        // ReSharper disable once UnusedMember.Global
        public static string UpperCase(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return string.Empty;
            var cultureInfo = Thread.CurrentThread.CurrentCulture;
            var textInfo = cultureInfo.TextInfo;
            return textInfo.ToUpper(title);
        }

        public static string LowerCase(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return string.Empty;
            var cultureInfo = Thread.CurrentThread.CurrentCulture;
            var textInfo = cultureInfo.TextInfo;
            return textInfo.ToUpper(title);
        }

        public static string StripTagsCharArray(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return string.Empty;

            var array = new char[source.Length];
            var arrayIndex = 0;
            var inside = false;

            foreach (var let in source) {
                if (let == '<') {
                    inside = true;
                    continue;
                }
                if (let == '>') {
                    inside = false;
                    continue;
                }
                if (inside)
                    continue;

                array[arrayIndex] = let;
                arrayIndex++;
            }
            return new string(array, 0, arrayIndex);
        }

        public static string RandomString(char[] chars, int size)
        {
            if (chars == null)
                return string.Empty;
            var result = string.Empty;
            for (var i = 0; i < size; i++) {
                result += chars[UnityEngine.Random.Range(0, chars.Length)];
            }
            return result;
        }
    }
}