using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Forsunkov
{
    public class NozzlesListXml
    {
        private static List<NozzlesDataList> Items = new List<NozzlesDataList>();
        public void SaveToXml(List<NozzlesDataList> data)
        {
            string Path = Environment.GetEnvironmentVariable("LOCALAPPDATA") + "/InjectorXml/";
            if (Directory.Exists(Path) == false)
            {
                Directory.CreateDirectory(Path);
            }
            if (File.Exists(Path + "NozzlesData.nldb") == false)
            {
                var create = File.Create(Path + "NozzlesData.nldb");
                create.Close();
            }
            var stream = File.OpenRead(Path + "NozzlesData.nldb");
            var Serializer = new XmlSerializer(Items.GetType());
            if (stream.Length > 200)
            {
                Items = (List<NozzlesDataList>)Serializer.Deserialize(stream);
            }
            for (int i = 0; i < data.Count; i++)
            {
                Items.Add(data[i]);
            }
            stream.Close();
            MemoryStream ms = new MemoryStream();
            StreamWriter Writer = new StreamWriter(ms);
            Serializer.Serialize(Writer, Items);
            Writer.Flush();
            File.WriteAllBytes(Path + "NozzlesData.nldb", ms.ToArray());
        }
        public static List<NozzlesDataList> LoadFromXml()
        {
            string Path = Environment.GetEnvironmentVariable("LOCALAPPDATA") + "/InjectorXml/";
            var stream = File.OpenRead(Path + "NozzlesData.nldb");
            var Serializer = new XmlSerializer(Items.GetType());
            return (List<NozzlesDataList>)Serializer.Deserialize(stream);
        }
    }
    [Serializable]
    public class NozzlesDataList
    {
        public string ModelCar { get; set; }
        public string NozzleType { get; set; }
        public string EngineModel { get; set; }
        public double NozzleResistance { get; set; }
        public double NozzleFrequency { get; set; }
        public double NozzleBorehole { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
    }
}
