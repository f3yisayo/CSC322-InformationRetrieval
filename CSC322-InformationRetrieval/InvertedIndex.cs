using System;
using System.Collections.Generic;
using System.Text;

namespace CSC322_InformationRetrieval
{
    /// <summary>
    /// The Inverted Index Class encapsulates the inverted index data structure.
    /// Stores a mapping of document terms to documentId's and the term's position the documnet.
    /// </summary>
    [Serializable]
    public class InvertedIndex
    {
        private readonly Dictionary<string, List<Tuple>> invertedIndex = new Dictionary<string, List<Tuple>>();
        private static InvertedIndex myIndex;


        private InvertedIndex()
        {
        }

        /// <summary>
        /// Follows the singleton design pattern for returning one instance of the class
        /// </summary>
        /// <returns>Returns a single instance of the InvertedIndex class</returns>
        public static InvertedIndex GetInstance()
        {
            // Returns only one instance of an inverted index for every word to be indexed 
            if (myIndex != null) return myIndex;
            myIndex = new InvertedIndex();

            return myIndex;
        }

        /// <summary>
        /// The indexer for the inverted index class.
        /// </summary>
        /// <param name="key">The term in the inverted index</param>
        /// <returns>Returns a list of tuples of document Id's and positions of the term</returns>
        public List<Tuple> this[string key]
        {
            get { return invertedIndex[key]; }
        }

        /// <summary>
        /// Checks if a term is in the inverted index
        /// </summary>
        /// <param name="key">The term being checked</param>
        /// <returns></returns>
        public bool ContainTerm(string key)
        {
            return invertedIndex.ContainsKey(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">The term to be added</param>
        /// <param name="idPos">A tuple of the the document Id and the position of the term</param>
        public void Add(string key, Tuple idPos)
        {
            if (invertedIndex.ContainsKey(key))
                invertedIndex[key].Add(idPos);
            else
            {
                var list = new List<Tuple>() {idPos};
                invertedIndex.Add(key, list);
            }
        }

        /// <summary>
        /// Removes a term in the inverted index
        /// </summary>
        /// <param name="key">The term being removed</param>
        /// <returns>Returns a boolean signalling the sucess or failure of the remove method </returns>
        public bool Remove(string key)
        {
            if (!invertedIndex.ContainsKey(key)) return false;
            invertedIndex.Remove(key);
            return true;
        }

        /// <summary>
        /// Gets the number of time the term occurs in the the document of the specified document Id
        /// </summary>
        /// <param name="term">The term being counted in the document</param>
        /// <param name="docId">The document id specifying the document</param>
        /// <returns></returns>
        public int TermFrequency(string term, int docId)
        {
            int count = 0;
            if (invertedIndex.ContainsKey(term))
            {
                foreach (var tuple in invertedIndex[term])
                {
                    if (tuple.DocumentId == docId)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Gets the number of terms in the inverted index
        /// </summary>
        /// <returns>Number of terms in the inverted index</returns>
        public int NumberOfterms()
        {
            return invertedIndex.Count;
        }

        /// <summary>
        /// Gets the number of documents the term appears in
        /// </summary>
        /// <param name="term">The term being counted</param>
        /// <returns>Returns the a document count for the term appears in</returns>
        public int DocumentFrequency(string term)
        {
            SortedSet<int> documents = new SortedSet<int>();
            if (invertedIndex.ContainsKey(term)) //if the term is present in the invertedIndex
            {
                foreach (var tuple in invertedIndex[term])
                {
                    documents.Add(tuple.DocumentId);
                    //get the docIds of in the postings list of the term
                }
            }

            return documents.Count;
        }

        /// <summary>
        /// Displays the inverted index
        /// </summary>
        /// <returns>A string representation of the inverted index</returns>
        public override string ToString()
        {
            // Show what has been indexed.
            StringBuilder builder = new StringBuilder();
            foreach (var key in invertedIndex.Keys)
            {
                builder.AppendLine("[" + key + "]" + "=>" + PrintSet(invertedIndex[key]));
            }
            return builder.ToString();
        }

        private string PrintSet(List<Tuple> set)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var value in set)
            {
                builder.Append(value + " ");
            }
            return builder.ToString();
        }

        /// <summary>
        /// A serializable tuple class that stores a documentID, Position pair.
        /// </summary>
        [Serializable]
        public class Tuple
        {
            private readonly int documentID;
            private readonly int position;

            /// <summary>
            /// Returns the document id of a tuple
            /// </summary>
            public int DocumentId
            {
                get { return documentID; }
            }

            /// <summary>
            /// Returns the position of a tuple
            /// </summary>
            public int Position
            {
                get { return position; }
            }

            /// <summary>
            /// Tuple constructor it takes a document id and a position value
            /// </summary>
            /// <param name="documentId"></param>
            /// <param name="position"></param>
            public Tuple(int documentId, int position)
            {
                documentID = documentId;
                this.position = position;
            }

            /// <summary>
            /// Displays the Tuple
            /// </summary>
            /// <returns>Returns a string representattion of the Tuple</returns>
            public override string ToString()
            {
                return "(" + documentID + "," + position + ")";
            }
        }
    }
}