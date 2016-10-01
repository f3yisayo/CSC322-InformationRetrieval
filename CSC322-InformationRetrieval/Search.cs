using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC322_InformationRetrieval
{
    class Search
    {
        private readonly string stringToSearch;
        private readonly string pathToSearchFrom;

        public Search(string stringToSearch, string pathToSearchFrom)
        {
            this.stringToSearch = stringToSearch;
            this.pathToSearchFrom = pathToSearchFrom;
        }


        public string SearchString()
        {
            if (string.IsNullOrWhiteSpace(stringToSearch)) return "Invalid query!";

            var query = new Query(stringToSearch, pathToSearchFrom);
            var result = query.QueryString();

            return PrintSet(result);
        }

        public string PrintSet(SortedSet<int> docIds)
        {
            if (docIds.Count == 0) return "No matches!";
            string result = "[";
            foreach (var docId in docIds)
            {
                result += docId + " ";
            }
            result += "]";
            return result;
        }
    }
}