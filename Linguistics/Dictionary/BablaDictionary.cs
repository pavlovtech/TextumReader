using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Linguistics.Dictionary;
using Linguistics.Utilities;

namespace Linguistics.Dictionary
{
    public class BablaDictionary: IDictionary
    {
        public WordTranslation GetTranslation(string word, string translationDirection)
        {
            string query = String.Format("http://www.babla.ru/{0}/{1}", translationDirection, word);

            string result;
            try
            {
                result = HttpQuery.Get(query);
            }
            catch(Exception ex)
            {
                throw new Exception("Http connection problem", ex);
            }
                
            var html = new HtmlDocument();
            html.LoadHtml(result);
            IEnumerable<HtmlNode> quickResultSection;
            string correctName;

            try
            {
                quickResultSection = html.DocumentNode.SelectNodes("//div[@class='quick-result-section'][1]//a[@class='muted-link']");
                correctName = html.DocumentNode.SelectSingleNode("//a[@class='result-link'][1]").InnerText;
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot parse results", ex);
            }
            
            List<string> translations = new List<string>();

            foreach (var node in quickResultSection)
            {
                translations.Add(node.InnerText);
            }

            return new WordTranslation() { Translations = translations.ToArray(), WordName = correctName };
        }
    }
}
