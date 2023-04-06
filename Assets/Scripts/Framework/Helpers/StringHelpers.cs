using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Framework.Helpers
{
    public static class StringHelpers
    {
        public enum InputFormatType
        {
            CamelCase,
            SnakeCase
        }

        public static string ToReadableString(this string str, InputFormatType inputFormatType = InputFormatType.CamelCase)
        {
            switch (inputFormatType)
            {
                case InputFormatType.CamelCase:
                    return Regex.Replace(str, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
                case InputFormatType.SnakeCase:
                    return str.Replace('_', ' ');
                default:
                    throw new NotSupportedException($"Could not support input format type: {inputFormatType}");
            }
        }

        public static string WrapWithTag(this string str, string tag, string tagValue = null)
        {
            return string.IsNullOrEmpty(tagValue) ? $"<{tag}>{str}</{tag}>" : $"<{tag}={tagValue}>{str}</{tag}>";
        }

        public static string WrapWithColor(this string str, Color color, bool includeAlpha = true)
        {
            return str.WrapWithTag("color", color.ToHtmlString(includeAlpha, true));
        }

        public static string WrapWithColor(this string str, string colorHexacode, bool includeAlpha = true)
        {
            if (ColorUtility.TryParseHtmlString(colorHexacode, out Color color))
            {
                return str.WrapWithColor(color);
            }

            return str;
        }

        public static string ColorizeKeywords<TKeyword>(this string str, IReadOnlyDictionary<TKeyword, Color> colorByKeyword, IReadOnlyDictionary<TKeyword, string> synonymByKeyword) where TKeyword : Enum
        {
            Array keywords = Enum.GetValues(typeof(TKeyword));
            int keywordsLength = keywords.Length;

            for (int i = 0; i < keywordsLength; ++i)
            {
                TKeyword keyword = (TKeyword)keywords.GetValue(i);
                if (colorByKeyword.TryGetValue(keyword, out Color color))
                {
                    string keywordAsString = keyword.ToString().ToReadableString().ToLower();
                    string keywordAstringWithColor = keywordAsString.WrapWithColor(color);
                    string preStr = str;
                    str = Regex.Replace(str, $@"\b{keywordAsString}\b", keywordAstringWithColor);
                    if (preStr == str)
                    {
                        if (synonymByKeyword.TryGetValue(keyword, out string synonyms))
                        {
                            string[] synonymsAsList = synonyms.Split(';');

                            for (int j = 0; j < synonymsAsList.Length; ++j)
                            {
                                string synonym = synonymsAsList[j];

                                if (string.IsNullOrWhiteSpace(synonym))
                                {
                                    continue;
                                }

                                keywordAstringWithColor = synonym.WrapWithColor(color);

                                str = Regex.Replace(str, synonym, keywordAstringWithColor);
                            }
                        }
                    }
                }
            }

            return str;
        }
    }
}