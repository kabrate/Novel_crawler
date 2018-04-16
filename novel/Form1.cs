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
using System.Threading;

namespace novel
{
    public partial class Form1 : Form
    {
        Thread t;

        public Form1()
        {
            InitializeComponent();
        }

        private void GetNovel()
        {
            //FileStream fs = new FileStream("E:\\novel.txt",FileMode.Create);
            using (StreamWriter writer = new StreamWriter("E:\\novel.txt"))
            {
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
                int i = 0;
                Action<int> actionProgressMax = (invokeData) =>
                {
                    this.progressBar1.Maximum = invokeData;
                };
                Invoke(actionProgressMax, address.Count);
                foreach (string ad in address)
                {
                    HtmlWeb web2 = new HtmlWeb();
                    var doc2 = web.LoadFromWebAsync($"{ad}").Result;
                    var content = doc2.DocumentNode.SelectNodes("//dd[@id='contents']");
                    var title = doc2.DocumentNode.SelectNodes("//dd/h1");
                    string str = (title[0].InnerText + "\r\n" + content[0].InnerText);
                    str = str.Replace("&nbsp;", " ");
                    writer.WriteLine(str);
                    //System.Diagnostics.Debug.WriteLine($"{i}/{address.Count}");
                    Action<int> actionProgress = (invokeData) => this.progressBar1.Value = invokeData;
                    Action<int> labelText = (invokeData) => this.label1.Text = $"{invokeData}/{address.Count} {(int)((double)invokeData / address.Count * 100)}%";
                    Invoke(actionProgress, i);
                    Invoke(labelText, i);
                    i++;
                }
            }
        }
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

        private void Form1_Load(object sender, EventArgs e)
        {
            t = new Thread(GetNovel);
            t.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (t.ThreadState == ThreadState.Running || t.ThreadState == ThreadState.WaitSleepJoin)
            {
                var flag = MessageBox.Show("爬虫正在进行中，确认关闭？", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (flag == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }
            t.Abort();
        }
    }
}
