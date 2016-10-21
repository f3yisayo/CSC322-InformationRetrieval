using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using Toxy;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics.Contracts;


namespace CSC322_InformationRetrieval
{
    /// <summary>
    /// The indexer class indexes files.
    /// </summary>
    public class Indexer
    {
        private readonly string pathToIndexTo;
        private int docId = 1;
        private int wordPosition = 1;
        private readonly PorterStemmer stemmer;

        private readonly string[] extensions =
        {
            ".htm", ".txt", ".pdf", ".doc", ".docx", ".ppt", ".pptx", ".xls", ".xlsx", ".html",
            ".xml"
        };

        /// <summary>
        /// Indexer constructor that takes the path to index to
        /// </summary>
        /// <param name="pathToIndexTo">path to index to</param>
        public Indexer(string pathToIndexTo)
        {
            this.pathToIndexTo = pathToIndexTo;
            stemmer = new PorterStemmer(); //create the stemmer object
        }

        private void ProcessFile(FileInfo file)
        {
            if (!file.Exists) return;
            if (!extensions.Contains(file.Extension)) return;
            // \u2022 is the unicode for a bullet symbol. 
            var separators = new[]
            {
                ' ', '\u2022', '’', '\"', '“', '!', '\'', '\\', '/', '_', '(', ')', '-', ',', ':', '?', ';', '.',
                '\r', '\n', '|'
            };
            try
            {
                //use toxy to extract string from files.
                //parser = ParserFactory.CreateText(new ParserContext(file.FullName));
                //checks if file has an html or xml extension.

                string document;
                ITextParser parser;
                if (file.Extension == ".html" || file.Extension == ".htm" || file.Extension == ".xml")
                {
                    parser = ParserFactory.CreateText(new ParserContext(file.FullName));
                    string textWithTags = parser.Parse();
                    document = RemoveAllTags(textWithTags);
                }
                else if (file.Extension == ".pptx")
                {
                    document = ExtractPptxText(file);
                }
                else
                {
                    parser = ParserFactory.CreateText(new ParserContext(file.FullName));
                    document = parser.Parse();
                }

                // Split with separators and ignore empty spaces.
                foreach (var word in document.ToLower().Split(separators, StringSplitOptions.RemoveEmptyEntries))
                {
                    // Remove stop words and numeric data.
                    if (stopwords.Contains(word) || Regex.IsMatch(word, "\\d+")) continue;

                    //stems word before adding it to the inverted index.
                    InvertedIndex.GetInstance()
                        .Add(stemmer.StemWord(word.Trim()), new InvertedIndex.Tuple(docId, wordPosition++));
                }
            }
            catch (Exception e) when (e is IOException || e is NullReferenceException || e is ZipException)
            {
                MessageBox.Show(@"Please close all programs using the files you want to search.");
            }
            catch (Exception e) when (e is InvalidDataException)
            {
                MessageBox.Show(@"Invalid file format.");
            }

            FileMatch.GetInstance().Add(docId, file);
            docId++;
        }

        /// <summary>
        /// It indexes a directory
        /// </summary>
        /// <param name="directory">path to the directory to index</param>
        public void IndexDirectory(DirectoryInfo directory)
        {
            Contract.Requires(directory != null);
            FileInfo[] files =
                directory.EnumerateFiles("*", SearchOption.AllDirectories)
                    .Where(f => extensions.Contains(f.Extension.ToLower()))
                    .ToArray();


            foreach (var file in files)
            {
                // If the file doesn't exist, skip the current iteration (Thanks Resharper!)
                if (!file.Exists) continue;
                ProcessFile(file);
            }
            SaveToDisk();
        }

        /// <summary>
        /// Indexes a file without saving it to disk
        /// </summary>
        /// <param name="file">path to file to index</param>
        public void IndexFile(FileInfo file)
        {
            ProcessFile(file);
        }

        /// <summary>
        /// Saves files indexed to disk
        /// </summary>
        public void SaveToDisk()
        {
            string pathTostoreFiles = Path.GetDirectoryName(pathToIndexTo) + @"\file.dat";
            new Serializer<InvertedIndex>(pathToIndexTo).Serialize(InvertedIndex.GetInstance());
            new Serializer<FileMatch>(pathTostoreFiles).Serialize(FileMatch.GetInstance());
        }

        private string RemoveAllTags(string inputString)
        {
            //starts with < sees zero or more characters which are not > and ends with >
            string output = Regex.Replace(inputString, "<[^>]*>", "");
            return output;
        }

        private static string ExtractPptxText(FileInfo file)
        {
            StringBuilder result = new StringBuilder();

            var parser = ParserFactory.CreateSlideshow(new ParserContext(file.FullName));
            var slides = parser.Parse();

            for (int i = 0; i < slides.Slides.Count; i++)
            {
                result.Append(ConcatListstring(slides.Slides[i].Texts));
            }

            return result.ToString();
        }

        private static string ConcatListstring(List<string> inputString)
        {
            StringBuilder result = new StringBuilder();
            foreach (var word in inputString)
            {
                result.Append(word.Trim()).Append(" ");
            }
            return result.ToString();
        }

        private readonly List<string> stopwords = new List<string>()
        {
            "a",
            "able",
            "about",
            "across",
            "after",
            "all",
            "almost",
            "also",
            "am",
            "among",
            "an",
            "and",
            "any",
            "are",
            "as",
            "at",
            "be",
            "because",
            "been",
            "but",
            "by",
            "can",
            "cannot",
            "could",
            "dear",
            "did",
            "do",
            "does",
            "either",
            "else",
            "ever",
            "every",
            "for",
            "from",
            "get",
            "got",
            "had",
            "has",
            "have",
            "he",
            "her",
            "hers",
            "him",
            "his",
            "how",
            "however",
            "i",
            "if",
            "in",
            "into",
            "is",
            "it",
            "its",
            "just",
            "least",
            "let",
            "like",
            "likely",
            "may",
            "me",
            "might",
            "most",
            "must",
            "my",
            "neither",
            "no",
            "nor",
            "not",
            "of",
            "off",
            "often",
            "on",
            "only",
            "or",
            "other",
            "our",
            "own",
            "rather",
            "said",
            "say",
            "says",
            "she",
            "should",
            "since",
            "so",
            "some",
            "than",
            "that",
            "the",
            "their",
            "them",
            "then",
            "there",
            "these",
            "they",
            "this",
            "tis",
            "to",
            "too",
            "twas",
            "us",
            "wants",
            "was",
            "we",
            "were",
            "what",
            "when",
            "where",
            "which",
            "while",
            "who",
            "whom",
            "why",
            "will",
            "with",
            "would",
            "yet",
            "you",
            "your"
        };
    }
}