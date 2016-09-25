using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using Toxy;
using System.Text.RegularExpressions;

namespace CSC322_InformationRetrieval
{
    class Indexer
    {
        private const string Charset = "windows-1251";
        public Indexer() { }

        public string Index(DirectoryInfo directory)
        {
            StringBuilder builder = new StringBuilder();
            PorterStemmer stemmer = new PorterStemmer();//create the stemmer object

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
                string document;
                try
                {
                    //checks if file has an html or xml extension.
                    if (file.Extension == ".html" || file.Extension == ".xml")
                        document = FilesWithoutTags(file);
                    else
                        document = parser.Parse();

                    // Split with separators and ignore empty spaces.
                    foreach (var word in document.ToLower().Split(separators, StringSplitOptions.RemoveEmptyEntries))
                    {
                        //stems word before adding it to the inverted index.
                        InvertedIndex.GetInstance().Add(stemmer.StemWord(word), new InvertedIndex.Tuple(docId, wordPosition++));
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

        private string RemoveAllTags(string inputString)
        {
            //starts with < sees zero or more characters which are not > and ends with >
            string output = Regex.Replace(inputString, "<[^>]*>", "");
            return output;
        }

        private string FilesWithoutTags(FileInfo inputFile)
        {
            StringBuilder result = new StringBuilder();
            StreamReader reader = null;

            try
            {
                Encoding encoding = Encoding.GetEncoding(Charset);
                reader = new StreamReader(inputFile.FullName, encoding);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = RemoveAllTags(line);//replaces the tag elements in each line of the file with an empty space. 
                    result.AppendLine(line);
                }
            }
            catch (Exception e) when (e is IOException || e is NullReferenceException || e is ZipException)
            {
                MessageBox.Show("Please close all programs using the files you want to search.");
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return result.ToString();
        }
    }


}


