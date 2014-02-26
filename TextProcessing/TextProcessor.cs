using System;
using System.Text;
using System.Threading.Tasks;

namespace TextumReader.Utilities
{
    static public class TextProcessor
    {
        static public string WrapWordsInTag(string text, string tag)
        {
            string openTag = String.Format("<{0}>", tag);
            string closeTag = String.Format("</{0}>", tag);

            StringBuilder sb = new StringBuilder();

            string[] wordList = text.Split(new Char[] { ' ', ',', '.', ':', '(', ')', '?', '!' },
                StringSplitOptions.RemoveEmptyEntries);

            Parallel.ForEach(wordList, word =>
            {
                lock (sb) { sb.Append(openTag + word + closeTag + ' '); } 
            });

            sb.Replace("\n", "<br>");
            return sb.ToString();
        }
    }
}
