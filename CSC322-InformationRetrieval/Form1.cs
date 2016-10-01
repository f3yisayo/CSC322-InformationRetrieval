using System.IO;
using System.Windows.Forms;

namespace CSC322_InformationRetrieval
{
    public partial class Form1 : Form
    {
        private DialogResult dialogResult;
        private string indexPath;
        private string indexFile;
        private string currentWorkingDirecory;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            // Clear the textbox before displaying anything new
            textBox1.Clear();
            // Display indexed results
            textBox1.Text = new Indexer(indexFile).Index(new DirectoryInfo(indexPath));
        }

        private void browseButton_Click(object sender, System.EventArgs e)
        {
            dialogResult = folderBrowserDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                indexPath = folderBrowserDialog.SelectedPath;
                pathTextbox.Text = indexPath;
            }
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            // On starting the app, get the current working directory, so it can be used to save the index to disk
            currentWorkingDirecory = System.Reflection.Assembly.GetExecutingAssembly().Location;

            // After getting the current working directory of the assembly, get the directory name to avoid something like
            // C:\TestDirectory\SubDirectory\App.exe been treated as a path to save the index file. 
            var directory = Path.GetDirectoryName(currentWorkingDirecory);
            if (directory != null) indexFile = Path.Combine(directory, "index.dat");
        }


        private void searchButton_Click(object sender, System.EventArgs e)
        {
            string queryString = textBox2.Text.Trim();

            textBox2.Text = new Search(queryString, indexFile).SearchString();
        }
    }
}