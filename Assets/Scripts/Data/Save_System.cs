using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Save_System
{
   public static void SavePlayer(Player_Controler player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        Player_Data data = new Player_Data(player);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static Player_Data LoadPlayer()
    {
        string path = Application.persistentDataPath + "/save.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Player_Data data = formatter.Deserialize(stream) as Player_Data;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("There is no save file");
            return null;
        }
    }

}
