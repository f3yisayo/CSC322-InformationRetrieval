using System.IO;
using System.Windows.Forms;

namespace CSC322_InformationRetrieval
{
    public partial class Form1 : Form
    {
        DialogResult dialogResult;
        string folderPath;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            // Clear the textbox before displaying anything new
            textBox1.Clear();
            // Display indexed results
            textBox1.Text = new Indexer(
                    @"C:\Users\Philip\Documents\Visual Studio 2015\Projects\CSC322-InformationRetrieval\CSC322-InformationRetrieval\bin\temp.dat")
                .
                Index(new DirectoryInfo(folderPath));
        }

        private void browseButton_Click(object sender, System.EventArgs e)
        {
            dialogResult = folderBrowserDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                folderPath = folderBrowserDialog.SelectedPath;
                pathTextbox.Text = folderPath;
            }
        }
    }
}