using System;
using System.Collections.Generic;

namespace CSC322_InformationRetrieval
{
    class Query
    {
        private readonly string queryString;
        private readonly InvertedIndex invertedIndex;

        public Query(string queryString, string queryPath)
        {
            this.queryString = queryString;
            invertedIndex = new Serializer<InvertedIndex>(queryPath).Deserialize();
        }

        //Handles one word and free text query.
        public SortedSet<int> QueryString()
        {
            List<SortedSet<int>> eachWordsDocIds = new List<SortedSet<int>>();
            SortedSet<int> result = new SortedSet<int>();
            var wordsInQuery = queryString.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            //separate each word in the query
            for (int index = 0; index < wordsInQuery.Length; index++)
            {
                eachWordsDocIds.Add(new SortedSet<int>());
                var term = WordToTerm(wordsInQuery[index]);
                if (invertedIndex.ContainTerm(term)) //if the term is present in the invertedIndex
                {
                    foreach (var tuple in invertedIndex[term])
                    {
                        eachWordsDocIds[index].Add(tuple.DocumentId);
                        //get the docIds of in the postings list of the term
                    }
                }
                result.UnionWith(eachWordsDocIds[index]); //Union the idList of each term with the final result.
            }
            return result;
        }

        private string WordToTerm(string word)
        {
            return new PorterStemmer().StemWord(word.ToLower());
        }
    }
}