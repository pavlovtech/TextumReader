using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Linguistics.Dictionary;
using Newtonsoft.Json.Linq;
using WatiN.Core;
using Form = System.Windows.Forms.Form;

namespace LingvoDictApp
{
    public partial class Form1 : Form
    {
        IWebDictionary _webDictionary = new BablaWebDictionary();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = "http://translate.google.com/translate_a/t?client=t&sl=en&tl=ru&hl=en&sc=2&ie=UTF-8&oe=UTF-8&ssel=0&tsel=0&q=treat";
            WebRequest wr = WebRequest.Create(url);
            Stream resp = wr.GetResponse().GetResponseStream();
            StreamReader respReader = new StreamReader(resp);

            string result = respReader.ReadToEnd();

            var x = getTranslations(result);

            textBox1.Text = result;
        }

        private string[] getTranslations(string data)
        {
            var list = data.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

            var x = SplitIntoWords(list[0], 2);

            int amountToTake = 3;

            string [] result = new string[0];

            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].Contains("verb"))
                {
                    result = result.Concat(SplitIntoWords(list[++i], amountToTake)).ToArray();
                }
                if (list[i].Contains("noun"))
                {
                    result = result.Concat(SplitIntoWords(list[++i], amountToTake)).ToArray();
                }
                if (list[i].Contains("particle"))
                {
                    result = result.Concat(SplitIntoWords(list[++i], amountToTake)).ToArray();
                }
                if (list[i].Contains("adjective"))
                {
                    result = result.Concat(SplitIntoWords(list[++i], amountToTake)).ToArray();
                }
                if (list[i].Contains("adverb"))
                {
                    result = result.Concat(SplitIntoWords(list[++i], amountToTake)).ToArray();
                }
                if (list[i].Contains("preposition"))
                {
                    result = result.Concat(SplitIntoWords(list[++i], amountToTake)).ToArray();
                }
            }

            return list;
        }

        private static string[] SplitIntoWords(string data, int amountToTake)
        {
            return data.Split(new char[] { ',', '"', '\\' }, StringSplitOptions.RemoveEmptyEntries)
                .Take(amountToTake).ToArray();
        }
    }
}
