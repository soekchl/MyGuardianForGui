using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics; 

namespace MyGuardianForGui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            listView1.Columns[1].Width = listView1.Width / 4;
            listView1.Columns[2].Width = listView1.Width / 2;

            timer1.Interval = 1000;
            timer1.Start();

            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult r = MessageBox.Show("确认关闭？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (r.GetHashCode() == 2)
            {
                e.Cancel = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "可执行文件(*.exe)|*.exe";
            if (fd.ShowDialog() == DialogResult.OK)
            {

                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (fd.SafeFileName == listView1.Items[i].SubItems[1].Text &&
                        fd.FileName == listView1.Items[i].SubItems[2].Text)
                    {
                        MessageBox.Show("已经存在！");
                        return;
                    }
                }

                int n = listView1.Items.Count;
                listView1.Items.Add((n + 1).ToString());
                listView1.Items[n].UseItemStyleForSubItems = false;

                listView1.Items[n].SubItems.Add(fd.SafeFileName);
                listView1.Items[n].SubItems.Add(fd.FileName);
                listView1.Items[n].SubItems.Add("未知");
                listView1.Items[n].SubItems.Add( "关");
                listView1.Items[n].SubItems[3].BackColor = Color.LightGray;
                listView1.Items[n].SubItems[4].BackColor = Color.OrangeRed;
                listView1.Items[n].SubItems.Add("0");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择需要删除的应用");
                return;
            }

            if (listView1.SelectedItems.Count > 1)
            {
                MessageBox.Show("删除只能一个一个删除");
                return;
            }
            string name = listView1.SelectedItems[0].SubItems[1].Text;
            DialogResult r = MessageBox.Show("确认删除 名称："+name+" ？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (r.GetHashCode() == 2)
            {
                return;
            }
            listView1.Items.Remove(listView1.SelectedItems[0]);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择需要切换守护状态的应用");
                return;
            }

            for ( int i = 0; i<listView1.SelectedItems.Count; i++)
            {
                if (listView1.SelectedItems[i].SubItems[4].Text == "关")
                {
                    listView1.SelectedItems[i].SubItems[4].Text = "开";
                    listView1.SelectedItems[i].SubItems[4].BackColor = Color.LightGreen;
                }
                else
                {
                    listView1.SelectedItems[i].SubItems[4].Text = "关";
                    listView1.SelectedItems[i].SubItems[4].BackColor = Color.LightGray;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (IsHave(listView1.Items[i].SubItems[1].Text) )
                {
                    if (!listView1.Items[i].SubItems[3].Text.Equals("运行中"))
                    {
                        listView1.Items[i].SubItems[3].BackColor = Color.LightGreen;
                        listView1.Items[i].SubItems[3].Text = "运行中";
                    }
                    if (!listView1.Items[i].SubItems[5].Text.Equals("0"))
                    {
                        listView1.Items[i].SubItems[5].Text = "0";
                    }
                }
                else
                {
                    if (!listView1.Items[i].SubItems[3].Text.Equals("停止"))
                    {
                        listView1.Items[i].SubItems[3].BackColor = Color.OrangeRed;
                        listView1.Items[i].SubItems[3].Text = "停止";
                    }

                    if (listView1.Items[i].SubItems[4].Text.Equals("开"))
                    {
                        try
                        {
                            System.Diagnostics.Process.Start(listView1.Items[i].SubItems[2].Text);
                            
                        }
                        catch (Exception a) 
                        {
                            listView1.Items[i].SubItems[4].Text = "关";
                            listView1.Items[i].SubItems[4].BackColor = Color.HotPink;
                            if (!show_error)
                            {
                                show_error = true;
                                MessageBox.Show(listView1.Items[i].SubItems[1].Text + a.Message);
                                show_error = false;
                            }
                        }
                        listView1.Items[i].SubItems[5].Text = (Convert.ToInt32(listView1.Items[i].SubItems[5].Text) + 1).ToString();
                    }
                }
            }
        }

        private bool IsHave(string name)
        {
            if (name.Length > 4)
            {
                name = name.Substring(0, name.Length - 4);
            }
            Process[] vProcesses = Process.GetProcesses();
            foreach (Process vProcess in vProcesses)
            {
                if (vProcess.ProcessName.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
