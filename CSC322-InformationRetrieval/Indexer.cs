using System;
using System.IO;
using System.Text;
using Toxy;

namespace CSC322_InformationRetrieval
{
    class Indexer
    {
        
        public Indexer(){  }

        public string Index(DirectoryInfo directory)
        {
            StringBuilder builder = new StringBuilder();
            var files = directory.GetFiles("*.txt");
            int docId = 1;
            foreach (var file in files)
            {
                // If the file doesn't exist, skip the current iteration (Thanks Resharper!)
                if (!file.Exists) continue;
                //use toxy to extract string from files.
                var parser = ParserFactory.CreateText(new ParserContext(file.FullName));
                string document = parser.Parse();

                // \u2022 is the unicode for a bullet symbol. 
                var separators = new[] { ' ', '\u2022', '’', '\"', '!', '\'', '\\', '/', '_', '(', ')', '-', ',', ':', '?', ';','.','\r','\n'};
                int wordPosition = 1;
                // Split with separators and ignore empty spaces.
                foreach (var word in document.ToLower().Split(separators, StringSplitOptions.RemoveEmptyEntries))
                {
                    InvertedIndex.GetInstance().Add(word, new InvertedIndex.Tuple(docId, wordPosition++));
                }

                docId++;
            }

            return InvertedIndex.GetInstance().ToString();
        }
    }
}
