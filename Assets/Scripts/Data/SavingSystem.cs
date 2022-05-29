using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/**
 * pass data from one scene to another scene
 */

public class SavingSystem 
{
    
    public static void SaveData(MyData data)
    {

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/data.sav";
        FileStream stream = new FileStream(path, FileMode.Create);

        MyData mData = new MyData(data);

        formatter.Serialize(stream, data);
        stream.Close();


    }

    public static MyData LoadData()
    {

        string path = Application.persistentDataPath + "/data.sav";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            MyData mData = formatter.Deserialize(stream) as MyData;
            stream.Close();

            return mData;

        }
        else
        {
            // file not found
            return null;
        }

    }

}
