using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSC322_InformationRetrieval
{
    /// <summary>
    /// A serializable Filematch class.
    /// Matches document ids to files.
    /// </summary>
    [Serializable]
    public class FileMatch
    {
        private readonly Dictionary<int, FileInfo> docIdToFile = new Dictionary<int, FileInfo>();
        private static FileMatch docIdToFileInstance;

        //Singleton design pattern
        private FileMatch()
        {
        }

        /// <summary>
        /// Gets the inverted index.
        /// </summary>
        /// <returns>Return an instance of the inverted index.</returns>
        public static FileMatch GetInstance()
        {
            // Returns only one instance of an inverted index for every word to be indexed 
            if (docIdToFileInstance != null) return docIdToFileInstance;
            docIdToFileInstance = new FileMatch();

            return docIdToFileInstance;
        }

        /// <summary>
        /// Add a file to the match file of the specified id.
        /// </summary>
        /// <param name="docId">document id</param>
        /// <param name="file">The file to match the document id</param>
        public void Add(int docId, FileInfo file)
        {
            if (!docIdToFile.ContainsKey(docId))
                docIdToFile[docId] = file;
        }

        /// <summary>
        /// An indexer that returns the file of the specified document id.
        /// </summary>
        /// <param name="docId"></param>
        public FileInfo this[int docId]
        {
            get { return docIdToFile[docId]; }
        }

        /// <summary>
        /// Remove file of the specified id.
        /// </summary>
        /// <param name="docId">Document id of the file to be removed.</param>
        /// <returns></returns>
        public bool Remove(int docId)
        {
            if (!docIdToFile.ContainsKey(docId)) return false;
            docIdToFile.Remove(docId);
            return true;
        }

        /// <summary>
        /// Display's how the FileMatch is represented.
        /// </summary>
        /// <returns>Retruns a string representation of the FileMatch class.</returns>
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