using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using HtmlAgilityPack;


namespace novel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FileStream fs = new FileStream("F:\\novel.txt",FileMode.Create);
            StreamWriter writer = new StreamWriter(fs);
            //WebRequest request = WebRequest.Create("http://www.23us.so/files/article/html/13/13655/index.html");
            //WebResponse response = (WebResponse)request.GetResponse();
            //Stream dataStream = response.GetResponseStream();
            //StreamReader reader = new StreamReader(dataStream, Encoding.GetEncoding("utf-8"));

            HtmlWeb web = new HtmlWeb();
            var doc = web.LoadFromWebAsync("http://www.23us.so/files/article/html/13/13655/index.html").Result;
            var nodes = doc.DocumentNode.SelectNodes("//table[@id='at']/tr/td/a");
            StringBuilder stringBuilder = new StringBuilder();
            List<string> address = new List<string>();
            foreach (var item in nodes)
            {
                address.Add(item.Attributes["href"].Value);
            }
            foreach (string ad in address )
            {
                HtmlWeb web2 = new HtmlWeb();
                var doc2 = web.LoadFromWebAsync($"{ad}").Result;
                var content = doc2.DocumentNode.SelectNodes("//dd[@id='contents']");
                var title = doc2.DocumentNode.SelectNodes("//dd/h1");
                string str=(title[0].InnerText+"\r\n"+ content[0].InnerText);
                str=str.Replace("&nbsp;","");
                writer.WriteLine(str);
                writer.Flush();
            }
            fs.Close();

            //HtmlWeb web = new HtmlWeb();
            //var doc = web.LoadFromWebAsync("http://www.23us.so/files/article/html/13/13655/5638724.html").Result;
            //var nodes = doc.DocumentNode.SelectNodes("//dd[@id='contents']");
            //StringBuilder stringBuilder = new StringBuilder();
            //string str="";

            //foreach (var item in nodes)
            //{
            //    str= ($"标题：{item.InnerText} ");
            //}
            //str=str.Replace("&nbsp;", "");
            //textBox1.Text = str;

        }
    }
}
