
using Component;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
    }
}
