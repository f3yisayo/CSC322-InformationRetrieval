using System.Collections.Generic;
using System.IO;

namespace CSC322_InformationRetrieval
{
    class Search
    {
        private readonly string stringToSearch;
        private readonly string pathToSearchFrom;
        private readonly FileMatch files;

        public Search(string stringToSearch, string pathToSearchFrom)
        {
            this.stringToSearch = stringToSearch;
            this.pathToSearchFrom = pathToSearchFrom;
            string pathTorestoreFiles = Path.GetDirectoryName(pathToSearchFrom) + @"\file.dat";
            files = new Serializer<FileMatch>(pathTorestoreFiles).Deserialize();
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
            string result = "";
            foreach (var docId in docIds)
            {
                result += files[docId] + ";";
            }
            return result;
        }
    }
}