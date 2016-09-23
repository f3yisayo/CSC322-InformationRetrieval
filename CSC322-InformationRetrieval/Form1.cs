using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Toxy;

namespace CSC322_InformationRetrieval
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            //display indexing results
            textBox1.Text = new Indexer().Index(new System.IO.DirectoryInfo(@"C:\Users\Feyisayo\Downloads"));
        }

    }
}
