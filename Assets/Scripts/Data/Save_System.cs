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

        Save_Data save = new Save_Data(GameObject_Manager.instance.allEnemies, player);
        formatter.Serialize(stream, save);
        stream.Close();
    }

    public static Save_Data LoadPlayer()
    {
        string path = Application.persistentDataPath + "/save.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Save_Data save = formatter.Deserialize(stream) as Save_Data;
            stream.Close();
            GameObject_Manager.instance.wasGameLoaded = true;

            return save;
        }
        else
        {
            Debug.LogError("There is no save file");
            return null;
        }
    }

}
