
using JsonTreeView;
using Newtonsoft.Json;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HttpJsonViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var port = 9128;
            this.Text = Text + " 127.0.0.1:" + port;
            var server = new SimpleTcpServer().Start(port);
            server.Delimiter = 0x0A;
            server.DelimiterDataReceived += (sndr, msg) => {

                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate {
                        listBox1.Items.Add(msg.MessageString);
                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    }));
                    return;
                }   
            };
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                dynamic parsedJson = JsonConvert.DeserializeObject(listBox1.SelectedItem.ToString().Trim());     
                richTextBox1.Text = JsonConvert.SerializeObject(parsedJson, Formatting.Indented) ;

                JsonTreeView.Nodes.Clear();
                JsonTreeView.LoadJsonToTreeView(listBox1.SelectedItem.ToString().Trim());
              //  JsonTreeView.ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid Json: " + ex.ToString());
            }
        }

        private void JsonTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            JsonTreeView.SelectedNode = e.Node;
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                menu.Show(Cursor.Position);
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string json = JsonTreeView.SelectedNode.Text;
            Clipboard.SetText(json.Substring(json.IndexOf(": ")+2));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            JsonTreeView.Nodes.Clear();
            listBox1.Items.Clear();
        }
    }
}
