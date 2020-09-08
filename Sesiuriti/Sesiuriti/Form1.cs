using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Sesiuriti
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Controller();
        }



        private void Controller()
        {
            OpenButton.Visible = false;
            textBox1.Visible = false;
            AddButton.Visible = false;
            CancelButton.Visible = false;
            LoadingLabel.Visible = false;
            FileNameLabel.Visible = false;
            FileNameTextBox.Visible = false;

            PoliciesTreeView.CheckBoxes = true;

            string[] paresedFilesDir = getParsedFilesDir();
            updatePoliciesList(paresedFilesDir);

        }

        private void updatePoliciesList(string[] policiesList)
        {
            comboBox1.Items.Clear();
            foreach (string s in policiesList)
            {
                comboBox1.Items.Add(Path.GetFileName(s));
            }
        }

        private string[] getParsedFilesDir()
        {
            var path = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);
            return Directory.GetFiles(path, "*.xml");
        }




        public OpenFileDialog ofd = new OpenFileDialog();
        private void button2_Click(object sender, EventArgs e)
        {
            ofd.Filter = "AUDIT|*.audit";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
                FileNameTextBox.Text = (Path.GetFileNameWithoutExtension(ofd.SafeFileName));


            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength != 0)
            {
                OpenButton.Visible = false;
                textBox1.Visible = false;
                AddButton.Visible = false;
                CancelButton.Visible = false;
                FileNameLabel.Visible = false;
                FileNameTextBox.Visible = false;

                Cursor = Cursors.WaitCursor;
                

                AuditFileParser parser = new AuditFileParser(textBox1.Text, FileNameTextBox.Text);
                parser.fileParser();
                updatePoliciesList(getParsedFilesDir());

                Cursor = Cursors.Arrow;
                AddNewButton.Visible = true;
            }
        }


        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            PoliciesTreeView.Nodes.Clear();
            string name = comboBox1.SelectedItem.ToString();
            name = name.Substring(0, name.Length - 4);
            TreeNode mainNode =  PoliciesTreeView.Nodes.Add(name);
            XML_Traverser xmlTraverser = new XML_Traverser(comboBox1.SelectedItem.ToString());
            xmlTraverser.AddNode(xmlTraverser.root, mainNode);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddNewButton.Visible = false;
            OpenButton.Visible = true;
            textBox1.Visible = true;
            AddButton.Visible = true;
            CancelButton.Visible = true;
            FileNameLabel.Visible = true;
            FileNameTextBox.Visible = true;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenButton.Visible = false;
            textBox1.Visible = false;
            CancelButton.Visible = false;
            AddButton.Visible = false;
            AddNewButton.Visible = true;
            FileNameLabel.Visible = false;
            FileNameTextBox.Visible = false;
        }
    }
}
