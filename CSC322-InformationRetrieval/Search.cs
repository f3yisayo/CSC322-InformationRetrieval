using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSC322_InformationRetrieval
{
    /// <summary>
    /// Search for strings in the inverted index.
    /// </summary>
    public class Search
    {
        private readonly string stringToSearch;
        private readonly string pathToSearchFrom;
        private readonly FileMatch files;

        /// <summary>
        /// A two arguement search constructor
        /// </summary>
        /// <param name="stringToSearch">The string to search</param>
        /// <param name="pathToSearchFrom">The path to search from</param>
        public Search(string stringToSearch, string pathToSearchFrom)
        {
            this.stringToSearch = stringToSearch;
            this.pathToSearchFrom = pathToSearchFrom;
            string pathTorestoreFiles = Path.GetDirectoryName(pathToSearchFrom) + @"\file.dat";
            files = new Serializer<FileMatch>(pathTorestoreFiles).Deserialize();
        }

        /// <summary>
        /// Searches the inverted index against a query string
        /// </summary>
        /// <param name="maxHit">Optional numbers of results to return</param>
        /// <returns>Returns a list of documents found else returns null</returns>
        public List<string> DoSearch(int maxHit = 10)
        {
            var query = new Query(stringToSearch, pathToSearchFrom);
            var queryResult = query.QueryString();
            var rankResult = new Ranking(query.EachTermSearched(), queryResult, query.GetInvertedIndex()).Rank(maxHit);

            if (rankResult.Count == 0)
                return null;

            return rankResult.Select(docId => files[docId].FullName).ToList();
        }
    }
}