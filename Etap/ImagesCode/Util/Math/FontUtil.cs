using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Util
{
    class FontUtil
    {

        public static string SplitToLines(string stringToSplit, int maximumLineLength)
        {
            return Regex.Replace(stringToSplit, @"(.{1," + maximumLineLength + @"})(?:\s|$)", "$1\n");
        }

        public static String SetTextWithMaxChars(string text, int charsPerLine = 30)
        {
            StringBuilder result = new StringBuilder();

            StringBuilder resultLine = new StringBuilder();
            StringBuilder resultWord = new StringBuilder();
            string[] words = text.Split(' ');

            int index = 0;
            for (int i = 0; i < text.Length; i++)
            {
               if(index++ <= charsPerLine)
                {
                    if (text[i].ToString() != "")
                    {
                        Console.WriteLine(index + " -> " + text[i]);
                        resultWord.Append(text[i]);
                        if (words.Contains(resultWord.ToString()))
                        {
                            Console.WriteLine(resultWord.ToString());
                            resultLine.Append(resultWord.ToString());
                            resultLine.Append(" ");
                            resultWord.Clear();
                        }
                    }
                } else
                {
                    Console.WriteLine("> " + resultLine.ToString() + "(" + index + ")");
                    result.Append(resultLine.ToString() + "-");
                    resultLine.Clear();

                    if(resultWord.Length > 0)
                    {
                        i -= resultWord.Length;
                        resultWord.Clear();
                    }

                    index = 0;
                }
            }

            return result.ToString();
        }
    }
}
