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
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using HtmlAgilityPack;
using Linguistics.Dictionary;
using Newtonsoft.Json.Linq;
using LinqToExcel;
using Form = System.Windows.Forms.Form;

namespace LingvoDictApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XDocument xmlResult = XDocument.Load("WORDFrequencyList.xml");
            var records = xmlResult.Descendants("record").
                Select(x => new
                {
                    Position = x.Element("Position").Value,
                    Word = x.Element("Word").Value
                });


            textBox1.Text = "";
        }


    }
}