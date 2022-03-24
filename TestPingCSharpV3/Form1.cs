using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace TestPingCSharpV3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BPing_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            if (TextBox1.Text != "" || ListBox2.Items.Count > 0)
            {
                PingOptions opcje = new PingOptions
                {
                    Ttl = (int)numericUpDown2.Value,
                    DontFragment = true
                };
                string dane = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] bufor = Encoding.ASCII.GetBytes(dane);
                int timeout = 120;
                if (TextBox1.Text != "")
                {
                    for (int i = 0; i < (int)numericUpDown1.Value; i++)
                        listBox1.Items.Add(this.WyslijPing(TextBox1.Text, timeout, bufor, opcje));
                    listBox1.Items.Add("----------------");
                }
                if (ListBox2.Items.Count > 0)
                {
                    foreach (string host in ListBox2.Items)
                    {
                        for (int i = 0; i < (int)numericUpDown1.Value; i++)
                            listBox1.Items.Add(this.WyslijPing(host, timeout, bufor, opcje));
                        listBox1.Items.Add("----------------");
                    }
                }
            }
            else
            {
                MessageBox.Show("No addresses have been entered", "Error");
            }
        }

        private void BAdd_Click(object sender, EventArgs e)
        {
            if (TextBox2.Text != String.Empty)
                if (TextBox2.Text.Trim().Length > 0)
                {
                    ListBox2.Items.Add(TextBox2.Text);
                    TextBox1.Clear();
                }
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) BPing_Click(sender, e);
        }

        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) BAdd_Click(sender, e);
        }

        private void ListBox2_DoubleClick(object sender, EventArgs e)
        {
            if (ListBox2.SelectedIndex != -1)
                ListBox2.Items.RemoveAt(ListBox2.SelectedIndex);
        }

        private string WyslijPing(string adres, int timeout, byte[] bufor, PingOptions opcje)
        {
            Ping ping = new Ping();
            try
            {
                PingReply odpowiedz = ping.Send(adres, timeout, bufor, opcje);
                if (odpowiedz.Status == IPStatus.Success)
                    return "Answer from " + adres + " bytes=" +
                    odpowiedz.Buffer.Length + " time=" + odpowiedz.RoundtripTime + "ms TTL=" +
                    odpowiedz.Options.Ttl;
                else
                    return "Error:" + adres + " " + odpowiedz.Status.ToString();
            }
            catch (Exception ex)
            {
                return "Error:" + adres + " " + ex.Message;
            }
        }
    }
}
