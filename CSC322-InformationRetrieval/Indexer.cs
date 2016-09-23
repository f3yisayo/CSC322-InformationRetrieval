using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Toxy;

namespace CSC322_InformationRetrieval
{
    class Indexer
    {
        

        public Indexer()
        {
            
        }

        public  string Index(DirectoryInfo directory)
        {
            StringBuilder builder = new StringBuilder();
            var files = directory.GetFiles("*.txt");
            int docId = 1;
            foreach (var file in files)
            {
                if (file.Exists)
                {
                    //use toxy to extract string from files.
                    var parser = ParserFactory.CreateText(new ParserContext(file.FullName.ToString()));
                    string document = parser.Parse();


                    var separators = new char[] { ' ', ',', ':', '?', ';','.','\r','\n'};
                    int wordPosition = 1;
                    //split with separators and ignore empty spaces.
                    foreach (var word in document.ToLower().Split(separators, StringSplitOptions.RemoveEmptyEntries))
                    {
                        InvertedIndex.GetInstance().Add(word, new InvertedIndex.Tuple(docId, wordPosition++));
                    }

                   docId++;
                }
                
            }

            return InvertedIndex.GetInstance().ToString();

            
        }
    }
}
