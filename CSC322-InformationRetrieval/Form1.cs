using System.Diagnostics;
using System.Drawing;
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

        // TODO: Remove the InvertedIndex TextBox later

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            // Clear the textbox before displaying anything new
            textBox1.Clear();
            // Display indexed results
            if (indexPath != null)
                textBox1.Text = new Indexer(indexFile).Index(new DirectoryInfo(indexPath));
            else
            {
                MessageBox.Show(@"Select a path!");
            }
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
            var directory = Path.GetDirectoryName(currentWorkingDirecory);
            if (directory != null) indexFile = Path.Combine(directory, "index.dat");
        }


        private void searchButton_Click(object sender, System.EventArgs e)
        {
            resultsLabel.Text = "";
            string queryString = searchTextBox.Text.Trim();
            Search search = new Search(queryString, indexFile);

            if (string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                resultsLabel.ForeColor = Color.Crimson;
                resultsLabel.Text = @"Invalid Query";
                return;
            }
            if (search.DoSearch() == null)
            {
                // Clear the list
                listBox1.DataSource = null;
                resultsLabel.ForeColor = Color.Crimson;
                resultsLabel.Text = @"No match found";
            }
            else
            {
                listBox1.DataSource = search.DoSearch();
                resultsLabel.ForeColor = Color.MediumSeaGreen;
                resultsLabel.Text = listBox1.Items.Count < 2
                    ? listBox1.Items.Count + " file found"
                    : listBox1.Items.Count + " files found";
            }

            // The first file in the listBox should NOT be selected by default.
            listBox1.SelectedIndex = -1;
        }

        private void listBox1_DoubleClick(object sender, System.EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                var selectedFile = listBox1.Text;
                Debug.WriteLine(selectedFile);
                Process.Start(selectedFile);
            }
        }
    }
}