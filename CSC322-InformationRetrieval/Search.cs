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
            var results = query.QueryString();

            if (results.Count == 0)
                return null;

            return results.Select(docId => files[docId].ToString()).ToList();
        }

     }
}