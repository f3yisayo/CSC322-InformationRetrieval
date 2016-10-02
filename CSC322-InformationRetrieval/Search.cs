using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace CSC322_InformationRetrieval
{
    class Search
    {
        private readonly string stringToSearch;
        private readonly string pathToSearchFrom;
        private readonly FileMatch files;
        public List<string> matchList = new List<string>();

        public Search(string stringToSearch, string pathToSearchFrom)
        {
            this.stringToSearch = stringToSearch;
            this.pathToSearchFrom = pathToSearchFrom;
            string pathTorestoreFiles = Path.GetDirectoryName(pathToSearchFrom) + @"\file.dat";
            files = new Serializer<FileMatch>(pathTorestoreFiles).Deserialize();
        }


        public bool DoSearch()
        {
            var query = new Query(stringToSearch, pathToSearchFrom);
            var results = query.QueryString();

            if (results.Count == 0)
                return false;

            foreach (var docId in results)
            {
                matchList.Add(files[docId].ToString());
            }

            return true;
        }

     }
}