﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CSC322_InformationRetrieval
{
    [Serializable]
    public class InvertedIndex
    {
        private readonly Dictionary<string, List<Tuple>> invertedIndex = new Dictionary<string, List<Tuple>>();
        private static InvertedIndex myIndex;


        private InvertedIndex()
        {
        }

        public static InvertedIndex GetInstance()
        {
            // Returns only one instance of an inverted index for every word to be indexed 
            if (myIndex != null) return myIndex;
            myIndex = new InvertedIndex();

            return myIndex;
        }

        public List<Tuple> this[string key]
        {
            get { return invertedIndex[key]; }
        }

        public bool ContainTerm(string key)
        {
            return invertedIndex.ContainsKey(key);
        }

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

        public bool Remove(string key)
        {
            if (!invertedIndex.ContainsKey(key)) return false;
            invertedIndex.Remove(key);
            return true;
        }

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

        public int NumberOfterms()
        {
            return invertedIndex.Count;
        }

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

        [Serializable]
        public class Tuple
        {
            private readonly int documentID;
            private readonly int position;

            public int DocumentId
            {
                get { return documentID; }
            }

            public int Position
            {
                get { return position; }
            }

            public Tuple(int documentId, int position)
            {
                documentID = documentId;
                this.position = position;
            }

            public override string ToString()
            {
                return "(" + documentID + "," + position + ")";
            }
        }
    }
}