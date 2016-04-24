namespace Uspelite.Services.Web
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Contracts;

    public class ShliokavitsaConvertService : IShliokavitsaConvertService
    {
        public string Convert(string text)
        {
            var words =
                text.ToLower().Split(new char[] {' ', '.', ':', '?', '/', '!', '№', '\'', '@', '#', '$', '(', ')', '%', '$', '=', '+' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();

            var wordsCount = words.Length;
            for (int i = 0; i <= wordsCount - 1; i++)
            {
                var word = words[i];

                foreach (var letter in word)
                {
                    if (bulgarianToLatinDictionary.ContainsKey(letter))
                    {
                        sb.Append(bulgarianToLatinDictionary[letter]);
                    }
                    else if ((65 <= letter && letter <= 90) || (97 <= letter && letter <= 122) || (48 <= letter && letter >= 57))
                    {
                        sb.Append(letter);
                    }
                }

                if (wordsCount - 1 != i)
                {
                    sb.Append('-');
                }
            }

            return sb.ToString().TrimEnd('-');
        }

        private static Dictionary<char, string> bulgarianToLatinDictionary = new Dictionary<char, string>
        {
           { 'а', "a" },
           { 'б', "b" },
           { 'в', "v" },
           { 'г', "g" },
           { 'д', "d" },
           { 'е', "e" },
           { 'ж', "j" },
           { 'з', "z" },
           { 'и', "i" },
           { 'й', "i" },
           { 'к', "k" },
           { 'л', "l" },
           { 'м', "m" },
           { 'н', "n" },
           { 'о', "o" },
           { 'п', "p" },
           { 'р', "r" },
           { 'с', "s" },
           { 'т', "t" },
           { 'у', "u" },
           { 'ф', "f" },
           { 'х', "h" },
           { 'ц', "c" },
           { 'ч', "ch" },
           { 'ш', "sh" },
           { 'щ', "sht" },
           { 'ъ', "u" },
           { 'ь', "ut" },
           { 'ю', "yu" },
           { 'я', "ya" },
        };
    }
}
