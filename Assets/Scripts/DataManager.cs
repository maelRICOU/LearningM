using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Text;

[XmlRoot("Data")]


public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public string path;

    private XmlSerializer serializer = new XmlSerializer(typeof(Data));
    private Encoding encoding = Encoding.GetEncoding("UTF-8");

    public void Awake()
    {
        instance = this;
        SetPath();
    }

    public void SetPath()
    {
        path = Path.Combine(Application.persistentDataPath, "Data.xml");
    }

    public Data Load()
    {
        if (File.Exists(path))
        {
            return serializer.Deserialize(new FileStream(path, FileMode.Open)) as Data;
        }

        return null;
    }

    public void Save(List<NeuralNetwork> _nets)
    {
        serializer.Serialize(new StreamWriter(path, false, encoding), new Data { nets = _nets });
    }
}
