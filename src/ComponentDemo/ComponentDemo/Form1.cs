
using Component;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    }
}
