using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using Toxy;

namespace CSC322_InformationRetrieval
{
    class Indexer
    {
        
        public Indexer(){  }

        public string Index(DirectoryInfo directory)
        {
            StringBuilder builder = new StringBuilder();
            
            //Get files with specified extensions.
            string[] extensions = new[] { ".txt", ".pdf", ".doc", ".docx", ".ppt", ".ppts", ".xls", ".xlsx", ".html", ".xml" };
            FileInfo[] files =
                directory.EnumerateFiles("*", SearchOption.AllDirectories)
                     .Where(f => extensions.Contains(f.Extension.ToLower()))
                     .ToArray();
            int docId = 1;
            foreach (var file in files)
            {
                // If the file doesn't exist, skip the current iteration (Thanks Resharper!)
                if (!file.Exists) continue;
                //use toxy to extract string from files.
                var parser = ParserFactory.CreateText(new ParserContext(file.FullName));
                // \u2022 is the unicode for a bullet symbol. 
                var separators = new[]
                {
                    ' ', '\u2022', '’', '\"', '“', '!', '\'', '\\', '/', '_', '(', ')', '-', ',', ':', '?', ';', '.', '\r', '\n'
                };
                int wordPosition = 1;
                try
                {
                    var document = parser.Parse();
                    // Split with separators and ignore empty spaces.
                    foreach (var word in document.ToLower().Split(separators, StringSplitOptions.RemoveEmptyEntries))
                    {
                        InvertedIndex.GetInstance().Add(word, new InvertedIndex.Tuple(docId, wordPosition++));
                    }
                }
                catch (Exception e) when (e is IOException || e is NullReferenceException || e is ZipException)
                {
                    MessageBox.Show("Please close all programs using the files you want to search.");
                }

                docId++;
            }

            return InvertedIndex.GetInstance().ToString();
        }
    }
}
