using System;
using System.IO;
using System.Xml.Serialization;

namespace WesleyNS.General.Utils.NUnit
{
    [Serializable]
    public class MyClass
    {
        public int InternalInt;
        public string InternalValue = "";

        public MyClass()
        {
        }

        public MyClass(string strVal, int intVal)
        {
            InternalValue = strVal;
            InternalInt = intVal;
        }
    }

    public class DictionarySerialization
    {
        private SerializableDictionary<string, MyClass> dict;

        public void SetUp()
        {
            dict = new SerializableDictionary<string, MyClass>();
            dict.Add("First", new MyClass("FirstVal", 11));
            dict.Add("Second", new MyClass("SecondVal", 12));
        }

        public void TearDown()
        {
            dict = null;
        }

        public void Serialize()
        {
            var s = new XmlSerializer(typeof (SerializableDictionary<string, MyClass>));
            string path = Environment.CurrentDirectory + @"\SerializableDictionary.xml";
            using (var sw = new StreamWriter(path))
            {
                s.Serialize(sw, dict);
            }
        }

        public void SerializeDeserializeEmpty()
        {
            var emptyDict = new SerializableDictionary<string, MyClass>();

            var s = new XmlSerializer(typeof (SerializableDictionary<string, MyClass>));
            string path = Environment.CurrentDirectory + @"\SerializableEmptyDictionary.xml";

            using (var sw = new StreamWriter(path))
            {
                s.Serialize(sw, emptyDict);
            }

            SerializableDictionary<string, MyClass> newEmptyDict = null;
            using (var sr = new StreamReader(path))
            {
                newEmptyDict = s.Deserialize(sr) as SerializableDictionary<string, MyClass>;
            }
        }

        public void SerializeDesrialize()
        {
            var s = new XmlSerializer(typeof (SerializableDictionary<string, MyClass>));
            string path = Environment.CurrentDirectory + @"\SerializableDictionary.xml";
            using (var sw = new StreamWriter(path))
            {
                s.Serialize(sw, dict);
            }

            SerializableDictionary<string, MyClass> newDict = null;
            using (var sr = new StreamReader(path))
            {
                newDict = s.Deserialize(sr) as SerializableDictionary<string, MyClass>;
            }
        }
    }
}