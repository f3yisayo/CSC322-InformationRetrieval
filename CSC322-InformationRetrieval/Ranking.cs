using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC322_InformationRetrieval
{
    class Ranking
    {
        private readonly SortedSet<int> resultsFound;
        private readonly string[] terms;
        private readonly InvertedIndex invertedIndex;

        public Ranking(string[] terms, SortedSet<int> resultsFound, InvertedIndex invertedIndex)
        {
            this.terms = terms;
            this.invertedIndex = invertedIndex;
            this.resultsFound = resultsFound;
        }

        public List<int> Rank()
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
                orderby pair.Value ascending
                select pair;

            return items.Select(keyvalue => keyvalue.Key).ToList();
        }

        private double LogTermFrequency(string term, int docId)
        {
            if (invertedIndex.TermFrequency(term, docId) == 0) return 0;
            return 1 + Math.Log10(invertedIndex.TermFrequency(term, docId));
        }

        private double InverseDocumentFrequency(string term)
        {
            return Math.Log10(invertedIndex.NumberOfterms()*1.0/invertedIndex.DocumentFrequency(term));
        }

        private double TfIdOfWeightOfTerm(string term, int docId)
        {
            return LogTermFrequency(term, docId)*InverseDocumentFrequency(term);
        }
    }
}