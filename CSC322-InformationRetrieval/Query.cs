using System;
using System.Collections.Generic;

namespace CSC322_InformationRetrieval
{
    class Query
    {
        private readonly string queryString;
        private readonly InvertedIndex invertedIndex;

        Query(string queryString, string queryPath)
        {
            this.queryString = queryString;
            invertedIndex = new Serializer<InvertedIndex>(queryPath).Deserialize();
        }

        //Handles one word and free text query.
        public SortedSet<int> QueryString()
        {
            SortedSet<int>[] eachWordsDocIds = {};
            SortedSet<int> result = new SortedSet<int>();
            var wordsInQuery = queryString.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            //separate each word in the query
            for (int index = 0; index < wordsInQuery.Length; index++)
            {
                eachWordsDocIds[index] = new SortedSet<int>();
                var word = wordsInQuery[index]; //each word in the query term
                if (invertedIndex.ContainTerm(WordToTerm(word))) //if the term is present in the invertedIndex
                {
                    foreach (var tuple in invertedIndex[word])
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