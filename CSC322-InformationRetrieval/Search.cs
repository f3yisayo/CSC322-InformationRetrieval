using System.Collections.Generic;
using System.IO;
using System.Linq;

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


        public List<string> DoSearch()
        {
            var query = new Query(stringToSearch, pathToSearchFrom);
            var queryResult = query.QueryString();
            var rankResult = new Ranking(query.EachTermSearched(), queryResult, query.GetInvertedIndex()).Rank();

            if (rankResult.Count == 0)
                return null;

            return rankResult.Select(docId => files[docId].FullName).ToList();
        }
    }
}