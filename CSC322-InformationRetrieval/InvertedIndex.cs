using System.Collections.Generic;
using System.Text;

namespace CSC322_InformationRetrieval
{
    class InvertedIndex
    {
        private readonly Dictionary<string, List<Tuple>> invertedIndex = new Dictionary<string, List<Tuple>>();
        private static InvertedIndex myIndex;

        private InvertedIndex(){}

        public static InvertedIndex GetInstance()
        {
            // Returns only one instance of an inverted index for every word to be indexed 
            if (myIndex == null)
                myIndex = new InvertedIndex();

            return myIndex;
        }

        public void Add(string key, Tuple idPos)
        {
            if (invertedIndex.ContainsKey(key))
                invertedIndex[key].Add(idPos);
            else
            {
                var list = new List<Tuple>() {idPos};
                invertedIndex.Add(key, list);
            }
        }

        public bool Remove(string key)
        {
            if (invertedIndex.ContainsKey(key))
            {
                invertedIndex.Remove(key);
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            // Show what has been indexed.
            StringBuilder builder = new StringBuilder();
            foreach (var key in invertedIndex.Keys)
            {
                builder.AppendLine("[" + key + "]" + "=>" + PrintSet(invertedIndex[key]));
            }
            return builder.ToString();
        }

        private string PrintSet(List<Tuple> set)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var value in set)
            {
                builder.Append(value + " ");
            }
            return builder.ToString();
        }

        public class Tuple
        {
            private int documentID;
            private int position;

            public int DocumentId { get; private set; }
            public int Position { get; private set; }

            public Tuple(int documentId, int position)
            {
                documentID = documentId;
                this.position = position;
            }

            public override string ToString()
            {
                return "(" + documentID + "," + position + ")";
            }
        }
    }
}
