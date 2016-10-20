using System;
using System.Collections.Generic;
using System.Linq;

namespace CSC322_InformationRetrieval
{
    /// <summary>
    /// Ranks the files found after searching.
    /// </summary>
    public class Ranking
    {
        private readonly SortedSet<int> resultsFound;
        private readonly string[] terms;
        private readonly InvertedIndex invertedIndex;

        /// <summary>
        ///A three arguement ranking class contructor.
        /// </summary>
        /// <param name="terms">terms in the query.</param>
        /// <param name="resultsFound">a list of documents found.</param>
        /// <param name="invertedIndex">The inverted index.</param>
        public Ranking(string[] terms, SortedSet<int> resultsFound, InvertedIndex invertedIndex)
        {
            this.terms = terms;
            this.invertedIndex = invertedIndex;
            this.resultsFound = resultsFound;
        }

        /// <summary>
        /// Ranks the documents found from the query according to the TF-IDF weighting.
        /// </summary>
        /// <param name="maxHit"></param>
        /// <returns></returns>
        public List<int> Rank(int maxHit)
        {
            Dictionary<int, double> idToRankValue = new Dictionary<int, double>();
            foreach (var docId in resultsFound)
            {
                double temp = 0;
                foreach (var term in terms)
                {
                    temp += (TfIdOfWeightOfTerm(term, docId));
                }
                idToRankValue.Add(docId, temp);
            }

            var items = from pair in idToRankValue
                orderby pair.Value descending
                select pair;

            List<int> results = items.Select(keyvalue => keyvalue.Key).Take(maxHit).ToList();
            return results;
        }

        private double LogTermFrequency(string term, int docId)
        {
            if (invertedIndex.TermFrequency(term, docId) == 0) return 0;
            return 1 + Math.Log10(invertedIndex.TermFrequency(term, docId));
        }

        private double InverseDocumentFrequency(string term)
        {
            return Math.Log10(invertedIndex.NumberOfDocuments()*1.0/invertedIndex.DocumentFrequency(term));
        }

        private double TfIdOfWeightOfTerm(string term, int docId)
        {
            return LogTermFrequency(term, docId)*InverseDocumentFrequency(term);
        }
    }
}