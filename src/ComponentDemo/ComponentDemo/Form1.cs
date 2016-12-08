
using Component;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComponentDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

     
        private void button1_Click_1(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            NSOperationQueue queue = new NSOperationQueue();

            for (int i=0; i<100; i++)
            {
                queue.Add(new NSOperation());
            }
            listBox1.Items.Add("Start");
            listBox1.Items.Add("Left count:"+queue.Count().ToString());

            NSSleep.Sleep(2000);
            queue.Stop();
            listBox1.Items.Add("Stop");
            listBox1.Items.Add("Left count:" + queue.Count().ToString());

            NSSleep.Sleep(2000);
            queue.Start();
            listBox1.Items.Add("Start");
            listBox1.Items.Add("Left count:" + queue.Count().ToString());

            NSSleep.Sleep(2000);
            queue.CancelAll();
            listBox1.Items.Add("CancelAll");
            listBox1.Items.Add("Left count:" + queue.Count().ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String context = textBox1.Text;
            String pattern = textBox2.Text;
            String result = NSRegex.Do(context, pattern);
            MessageBox.Show(result);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread t1 = new Thread(()=> {
                for (int i = 0; i <= 10; i++)
                {
                    NSRunOnMainThread.Run(this, new Action(() => { label1.Text = i + "s"; }));
                    Thread.Sleep(500);
                }
            });
            t1.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= 10; i++)
            {
                label2.Text = i + "s";
                NSSleep.Sleep(500);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            UIRichTextBox.AppendText(richTextBox1, DateTime.Now.ToString()+"\n", Color.Blue);
            UIRichTextBox.ScrollToEnd(richTextBox1);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            tabControl1.Width = this.Width - 40;
            tabControl1.Height = this.Height - 65;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url;
            if (e.Link.LinkData != null)
                url = e.Link.LinkData.ToString();
            else
                url = (sender as LinkLabel).Text.Substring(e.Link.Start, e.Link.Length);

            if (!url.Contains("://"))
                url = "http://" + url;

            var si = new ProcessStartInfo(url);
            Process.Start(si);
            (sender as LinkLabel).LinkVisited = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            NSMySQL.URL = "http://ehealth.asus.com/vivobaby/php/MySQLTool.php";
            ArrayList tables = NSMySQL.GetTables("vivobaby");
            foreach (String table in tables)
            {
                listBox2.Items.Add(table);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a table");
                return;
            }
            String dbname = "vivobaby";
            String tablename = listBox2.SelectedItem.ToString();
            NSMySQL.URL = "http://ehealth.asus.com/vivobaby/php/MySQLTool.php";
            NSMySQLData data = NSMySQL.GetData(dbname, tablename, String.Format("SELECT * FROM {0}", tablename));

            if (data.E.Message.Length > 0)
            {
                MessageBox.Show(data.E.Message);
                return;
            }
            dataGridView1.ColumnCount = data.Columns.Count;
            for (int i = 0; i < data.Columns.Count; i++)
            {
                dataGridView1.Columns[i].Name = (String)data.Columns[i];
            }

            dataGridView1.Rows.Clear();
            foreach (Dictionary<String, String> dict in data.Datas)
            {
                ArrayList itemList = new ArrayList();

                foreach (String title in data.Columns)
                {
                    itemList.Add(dict[title]);
                }
                dataGridView1.Rows.Add(itemList.ToArray());
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.HeaderCell.Value = (row.Index + 1).ToString();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            List<NSSerialPortInfo> infos = NSSerialPort.Search();
            foreach (NSSerialPortInfo info in infos)
            {
                listView1.Items.Add(new ListViewItem(new[] { info.ComPort, info.DeviceID, info.Caption, info.Description }));
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
            NSBoardCast.form = this;
            NSBoardCast.Recv(8888, "echo from server", 5000);
            NSBoardCast.Send("255.255.255.255", 8888, "ping", 2000);
            NSBoardCast.DidResponedChanged += DidResponedChanged;
        }

        private void DidResponedChanged(String host, String msg)
        {
            listBox3.Items.Add(String.Format("{0}:{1}", host, msg));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Class1.Test();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            NSJObject obj = new NSJObject();
            obj.Add("test1", "testvalue1");
            obj.Add("test2", "testvalue2");
            obj.Add("test3", 1);
            obj.Add("test4", true);
            NSJArray obj2 = new NSJArray("test2", "test2value", 3, false);

            NSJObject obj3 = new NSJObject();
            obj3.Add("test8", "testvalue8");
            obj3.Add("test9", "testvalue9");


            obj2.Add(obj3);
            obj.Add("test5", obj2);

            textBox3.Text = "";
            textBox3.Text += "NSJObject obj = new NSJObject();\r\n";
            textBox3.Text += "obj.Add(\"test1\", \"testvalue1\");\r\n";
            textBox3.Text += "obj.Add(\"test2\", \"testvalue2\");\r\n";
            textBox3.Text += "obj.Add(\"test3\", 1);\r\n";
            textBox3.Text += "obj.Add(\"test4\", true);\r\n";
            textBox3.Text += "\r\n";
            textBox3.Text += "\r\n";

            textBox3.Text += "NSJObject obj3 = new NSJObject();\r\n";
            textBox3.Text += "obj3.Add(\"test8\", \"testvalue8\");\r\n";
            textBox3.Text += "obj3.Add(\"test9\", \"testvalue9\");\r\n";
            textBox3.Text += "\r\n";
            textBox3.Text += "\r\n";

            textBox3.Text += "NSJArray obj2 = new NSJArray(\"test2\", \"test2value\", 3, false);\r\n";
            textBox3.Text += "obj2.Add(obj3);\r\n";
            textBox3.Text += "obj.Add(\"test5\", obj2);\r\n";
            textBox3.Text += "\r\n";
            textBox3.Text += "\r\n";
            textBox3.Text += obj.ToJson();
        }
    }
}
