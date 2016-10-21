using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CSC322_InformationRetrieval
{
    partial class Form1 : Form
    {
        private DialogResult dialogResult;
        private string indexPath;
        private string indexFile;
        private string currentWorkingDirecory;
        private int maxHit;

        public Form1()
        {
            InitializeComponent();
        }


        private void browseButton_Click(object sender, System.EventArgs e)
        {
            dialogResult = folderBrowserDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                indexPath = folderBrowserDialog.SelectedPath;
                pathTextbox.Text = indexPath;
                progressBar1.Visible = true;
                percentageText.Visible = true;
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            // On starting the app, get the current working directory, so it can be used to save the index to disk
            currentWorkingDirecory = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = Path.GetDirectoryName(currentWorkingDirecory);
            if (directory != null) indexFile = Path.Combine(directory, "index.dat");
            comboBox1.SelectedIndex = 1;
            progressBar1.Visible = true;
            percentageText.Visible = false;
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
            if (search.DoSearch(maxHit) == null)
            {
                // Clear the list
                listBox1.DataSource = null;
                resultsLabel.ForeColor = Color.Crimson;
                resultsLabel.Text = @"No match found";
            }
            else
            {
                listBox1.DataSource = search.DoSearch(maxHit);
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

        private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            maxHit = int.Parse(comboBox1.SelectedItem.ToString());
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var indexer = new Indexer(indexFile);
            if (indexPath != null)
            {
                var files = new DirectoryInfo(indexPath);
                int i = 0;
                int interval = 100/files.GetFiles().Length;
                foreach (var file in files.GetFiles())
                {
                    indexer.IndexDoc(file);
                    backgroundWorker1.ReportProgress(i);
                    i += interval;
                }
                backgroundWorker1.ReportProgress(i + (99 - i));
            }
            indexer.SaveToDisk();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            percentageText.Text = e.ProgressPercentage + @" %";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender,
            RunWorkerCompletedEventArgs e)
        {
            progressBar1.Visible = false;
            percentageText.Text = "Indexing Complete";
            percentageText.ForeColor = Color.MediumSeaGreen;
        }
    }
}