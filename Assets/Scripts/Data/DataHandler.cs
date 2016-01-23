using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class DataHandler<T> where T : struct
{
    public string filePath { get; private set; }
    public string fileName { get; private set; }

    public DataHandler(string fileName)
    {
        this.fileName = fileName;
        filePath = Application.persistentDataPath + '/' + fileName;
    }

    public bool DeleteFile()
    {
        File.Delete(filePath);
        Debug.Log("Deleted " + filePath);
        return true;
    }

    public bool CanLoad()
    {
        return File.Exists(filePath);
    }

    public Nullable<T> Load()
    {
        Nullable<T> data = null;

        if (CanLoad())
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);
            data = (T)binaryFormatter.Deserialize(file);
            file.Close();

            Debug.Log("Loaded " + filePath);
        }
        else
        {
            Debug.Log("Could not load " + filePath);
        }

        return data;
    }

    public bool Save(T data)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(filePath);
        binaryFormatter.Serialize(file, data);
        file.Close();

        Debug.Log("Saved " + filePath);

        return true;
    }
}
