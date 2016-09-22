using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC322_InformationRetrieval
{
    class InvertedIndex
    {
        internal class Tuple
        {
            private int fieldId;
            private int position;

            public int FieldId { get; private set; }
            public int Position { get; private set; }
            Tuple(int fieldId, int position)
            {
                this.fieldId = fieldId;
                this.position = position;
            }
            public override string ToString()
            {
                return "(" + fieldId + "," + position + ")";
            }
        }

        internal class TupleComparer : IComparer<Tuple>
        {
            public int Compare(Tuple x, Tuple y)
            {
                return x.FieldId.CompareTo(y.FieldId);
            }
        }

        private readonly Dictionary<string, SortedSet<Tuple>> invertedIndex;
        public InvertedIndex()
        {
            invertedIndex = new Dictionary<string, SortedSet<Tuple>>();
        }

        public void Add(string key, Tuple idPos)
        {
            if (invertedIndex.ContainsKey(key))
            {
                invertedIndex[key].Add(idPos);
            }
            else
            {
                var list = new SortedSet<Tuple>(new TupleComparer()) { idPos };
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
        //public static string Test()
        //{
        //    string path = @"C:\Users\Philip\Desktop\Lecture Note.pdf";
        //    var parser = ParserFactory.CreateText(new ParserContext(path));
        //    var document = parser.Parse();
        //    return document.ToString();
        //}
    }
}
