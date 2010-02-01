using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AIMS.Libraries.CodeEditor.Syntax;
using AIMS.Libraries.CodeEditor.SyntaxFiles;

namespace parCer32
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            syntaxDocument1.SyntaxFile = SyntaxLanguage.CPP.ToString();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FileStream strm;
            OpenFileDialog ofn = new OpenFileDialog();
            ofn.Filter = "C Files (*.c)|*.c|Text Files (*.txt)|*.txt";
            ofn.Title = "Type File";

            if (ofn.ShowDialog() == DialogResult.OK)
            {


                strm = new FileStream(ofn.FileName, FileMode.Open, FileAccess.Read);
                StreamReader rdr = new StreamReader(strm);
                codeEditorControl1.Document.Text = rdr.ReadToEnd();
                rdr.Close();
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Scanner scanner = new Scanner();

            Parser parser = new Parser(scanner);

            ParseTree tree = parser.Parse(codeEditorControl1.Document.Text);
            int a = 0;
            foreach (ParseError err in tree.Errors)
            {

               statusError.Text = String.Format("Line: {0,3}, Column: {1,3} : {2}", err.Line, err.Column, err.Message);
                a++;
                break;
            }
            if (a == 0)
                statusError.Text = string.Format("No Errors Found.");

        }


    }
}
