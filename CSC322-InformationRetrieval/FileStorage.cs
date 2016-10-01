using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC322_InformationRetrieval
{
    [Serializable]
    class FileStorage
    {
        private readonly Dictionary<int, FileInfo> docIdToFile = new Dictionary<int, FileInfo>();

        private static FileStorage docIdToFileInstance;


        private FileStorage()
        {
        }

        public static FileStorage GetInstance()
        {
            // Returns only one instance of an inverted index for every word to be indexed 
            if (docIdToFileInstance != null) return docIdToFileInstance;
            docIdToFileInstance = new FileStorage();

            return docIdToFileInstance;
        }

        public void Add(int key, FileInfo file)
        {
            if (!docIdToFile.ContainsKey(key))
                docIdToFile[key] = file;
        }

        public bool Remove(int key)
        {
            if (!docIdToFile.ContainsKey(key)) return false;
            docIdToFile.Remove(key);
            return true;
        }

        public override string ToString()
        {
            // Show what has been indexed.
            StringBuilder builder = new StringBuilder();
            foreach (var key in docIdToFile.Keys)
            {
                builder.AppendLine("[" + key + "]" + "=>" + docIdToFile[key]);
            }
            return builder.ToString();
        }
    }
}