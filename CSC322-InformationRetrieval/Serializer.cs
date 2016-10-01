using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CSC322_InformationRetrieval
{
    public class Serializer<T> where T : InvertedIndex
    {
        private readonly string path;

        private readonly BinaryFormatter formatter;

        public Serializer(string path)
        {
            this.path = path;
            formatter = new BinaryFormatter();
        }

        public void Serialize(T invertedIndex)
        {
            var stream = File.Open(path, FileMode.Create);
            formatter.Serialize(stream, invertedIndex);
            stream.Close();
        }

        public T Deserialize()
        {
            var stream = File.Open(path, FileMode.Open);
            var invertedIndex = (T) formatter.Deserialize(stream);
            stream.Close();
            return invertedIndex;
        }
    }
}