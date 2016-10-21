using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace CSC322_InformationRetrieval
{
    /// <summary>
    /// The Inverted Index Class encapsulates the inverted index data structure.
    /// Stores a mapping of document terms to document ids and the term's position in the documnet.
    /// </summary>
    [Serializable]
    public class InvertedIndex
    {
        private readonly Dictionary<string, List<Tuple>> invertedIndex = new Dictionary<string, List<Tuple>>();
        private static InvertedIndex myIndex;
        private readonly SortedSet<int> documentFrequency = new SortedSet<int>();


        private InvertedIndex()
        {
        }

        /// <summary>
        /// Gets the inverted index
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
            get
            {
                Contract.Requires(key != null);
                return invertedIndex[key];
            }
        }

        /// <summary>
        /// Checks if a term is in the inverted index
        /// </summary>
        /// <param name="key">The term being checked</param>
        /// <returns></returns>
        public bool ContainTerm(string key)
        {
            Contract.Requires(key != null);
            return invertedIndex.ContainsKey(key);
        }

        /// <summary>
        /// Adds a term to the inverted index
        /// </summary>
        /// <param name="term">The term to be added</param>
        /// <param name="idPos">A tuple of the the document Id and the position of the term</param>
        public void Add(string term, Tuple idPos)
        {
            Contract.Requires(term != null);
            if (invertedIndex.ContainsKey(term))
            {
                invertedIndex[term].Add(idPos);
                documentFrequency.Add(idPos.DocumentId);
            }
            else
            {
                var list = new List<Tuple>() {idPos};
                invertedIndex.Add(term, list);
                documentFrequency.Add(idPos.DocumentId);
            }
        }

        /// <summary>
        /// Removes a term in the inverted index
        /// </summary>
        /// <param name="term">The term being removed</param>
        /// <returns>Returns a boolean signalling the sucess or failure of the remove method </returns>
        public bool Remove(string term)
        {
            Contract.Requires(term != null);
            if (!invertedIndex.ContainsKey(term)) return false;
            invertedIndex.Remove(term);
            return true;
        }

        /// <summary>
        /// Gets the number of occurrence the term occurs in the the document of the specified document Id
        /// </summary>
        /// <param name="term">The term being counted in the document</param>
        /// <param name="docId">The document id specifying the document</param>
        /// <returns></returns>
        public int TermFrequency(string term, int docId)
        {
            Contract.Requires(term != null);
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
        /// <returns>Number of documnets in the inverted index</returns>
        public int NumberOfDocuments()
        {
            return documentFrequency.Count;
            //return invertedIndex.Count;
        }

        /// <summary>
        /// Gets the number of documents the term appears in
        /// </summary>
        /// <param name="term">The term being counted</param>
        /// <returns>Returns the a document count for the term it appears in</returns>
        public int DocumentFrequency(string term)
        {
            Contract.Requires(term != null);
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
                builder.Append("[" + key + "]" + "=>" + PrintSet(invertedIndex[key]));
            }
            return builder.ToString();
        }

        private string PrintSet(List<Tuple> set)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var value in set)
            {
                builder.Append(value + "");
            }
            return builder.ToString();
        }

        /// <summary>
        /// A serializable tuple class that stores a document id, Position pair.
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