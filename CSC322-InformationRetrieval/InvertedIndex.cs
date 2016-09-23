using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC322_InformationRetrieval
{
     class InvertedIndex
    {
        public class Tuple 
        {
            private  int fieldId = 0;
            private int position;

            public int FieldId { get; private set; }
            public int Position { get; private set; }
            public Tuple(int fieldId, int position)
            {
                this.fieldId = fieldId;
                this.position = position;
            }
            public override string ToString()
            {
                return "(" + fieldId + "," + position + ")";
            }
          
        }

        private readonly Dictionary<string, List<Tuple>> invertedIndex = new Dictionary<string, List<Tuple>>();
        private static InvertedIndex myIndex = null;
        
        private InvertedIndex()
        {

        }
        public  static InvertedIndex GetInstance()
        {
            //returns only one instance of an inverted index for every word to be indexed 
            if (myIndex == null)
                 myIndex = new InvertedIndex();
            
           return myIndex;
        }
        public void Add(string key, Tuple idPos)
        {
            if (invertedIndex.ContainsKey(key))
            {
                invertedIndex[key].Add(idPos);
            }
            else
            {
                var list = new List<Tuple>() { idPos };
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
            //shows what has been indexed.
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
        //public static string Test()
        //{
        //    string path = @"C:\Users\Philip\Desktop\Lecture Note.pdf";
        //    var parser = ParserFactory.CreateText(new ParserContext(path));
        //    var document = parser.Parse();
        //    return document.ToString();
        //}
    }
}
