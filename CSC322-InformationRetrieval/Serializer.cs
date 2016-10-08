using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CSC322_InformationRetrieval
{
    /// <summary>
    /// A generic serializer class
    /// </summary>
    /// <typeparam name="T">class Type</typeparam>
    public class Serializer<T>
    {
        private readonly string path;

        private readonly BinaryFormatter formatter;

        /// <summary>
        /// Serializer constructor
        /// </summary>
        /// <param name="path">Path to serialize to</param>
        public Serializer(string path)
        {
            this.path = path;
            formatter = new BinaryFormatter();
        }

        /// <summary>
        /// Serialize  the object specified
        /// </summary>
        /// <param name="objectToSerialize">Object to serialize</param>
        public void Serialize(T objectToSerialize)
        {
            var stream = File.Open(path, FileMode.Create);
            formatter.Serialize(stream, objectToSerialize);
            stream.Close();
        }

        /// <summary>
        /// Deserialize the object initially serialized
        /// </summary>
        /// <returns>Returns the deserialized object</returns>
        public T Deserialize()
        {
            var stream = File.Open(path, FileMode.Open);
            var invertedIndex = (T) formatter.Deserialize(stream);
            stream.Close();
            return invertedIndex;
        }
    }
}