using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FindAll
{
    public partial class Form1 : Form
    {
        private DateTime starTime;
        private DateTime endTime;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            starTime = DateTime.Now;
            //var sb = new StringBuilder();
            //if (!string.IsNullOrEmpty(txtPath.Text))
            //{
            //    foreach (var file in Directory.GetFiles(txtPath.Text, txtPattern.Text, SearchOption.AllDirectories))
            //    {
            //        if (File.ReadAllText(file).Contains(txtKeyword.Text))
            //        {
            //            sb.AppendLine(file);
            //        }
            //    }
            //}

            //txtOutput.Clear();
            //txtOutput.Text = string.IsNullOrEmpty(sb.ToString()) ? "End" : sb.ToString();

            var af = new AsynFile();
            af.AccessCompleted += FindCompleted;
            af.FindFiles(txtPath.Text, txtPattern.Text, txtKeyword.Text);

            //endTime = DateTime.Now;
            //MessageBox.Show(string.Format("Time: {0}", (endTime - starTime).TotalSeconds.ToString()));
        }

        private void FindCompleted(SortedSet<string> files)
        {
            var sb = new StringBuilder();
            
            foreach (var file in files)
            {
                sb.AppendLine(file);
            }
            
            AppendTextBox(sb.ToString());
        }

        private void AppendTextBox(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                return;
            }
            txtOutput.Clear();
            txtOutput.Text += value;

            endTime = DateTime.Now;
            MessageBox.Show(string.Format("Time: {0}", (endTime - starTime).TotalSeconds.ToString()));
        }

        private void txtPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                txtPath.Text = folderBrowserDialog1.SelectedPath;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }
}
