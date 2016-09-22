using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC322_InformationRetrieval
{
    class Indexer
    {
        private InvertedIndex invertedIndex;

        public Indexer()
        {
            invertedIndex = new InvertedIndex();
        }

        public static void Index(DirectoryInfo directory)
        {
            var files[] = Directory.GetFiles(@"c:users\phlip\desktop", "*.pdf|.", SearchOption.AllDirectories);
        }
    }
}
