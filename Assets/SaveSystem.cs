using UnityEngine;
using System.IO; 
using System.Runtime.Serialization.Formatters.Binary;
using System.Data.Common;

public static class SaveSystem { // a static class makes it so that the class isn't instantiatable and wont make multiple versions elsewhere 
    private static readonly string path = Application.persistentDataPath + "/player.data"; 
    
        public static void SavePlayer(PlayerHealth playerHealth) {
            BinaryFormatter formatter = new BinaryFormatter(); 
            
            FileStream stream = new FileStream(path, FileMode.Create);

            PlayerData data = new PlayerData(playerHealth); //calling the contructor from the other class
            //calls it and gets the same data from it instead of us having to set it up

            try {
                formatter.Serialize(stream, data);
                Debug.Log("Player data saved successfully.");
            } catch (System.Exception e) {
                Debug.LogError("Failed to save player data: " + e.Message);
            } finally {
                stream.Close();
            }  
        }

        public static PlayerData LoadPlayer() {

            
            if (File.Exists(path)) {
                BinaryFormatter formatter = new BinaryFormatter(); 
                try
                {
                    using (FileStream stream = new FileStream(path, FileMode.Open))
                    {
                        PlayerData data = formatter.Deserialize(stream) as PlayerData;
                        Debug.Log("Player data loaded successfully.");
                        return data;
                    }
                }
                catch (IOException e)
                {
                    Debug.LogError("Failed to load player data: " + e.Message);
                    return null;
                }

            } else {
                Debug.LogError("Saved File not found in " + path); 
                return null; 
            }
        }
    
}

/* 
using UnityEngine;
using System.IO;
using System.Text.Json;

public static class SaveSystem
{
    private static readonly string path = Application.persistentDataPath + "/player.json";

    public static void SavePlayer(PlayerData playerData)
    {
        try
        {
            string jsonData = JsonSerializer.Serialize(playerData); // For Newtonsoft.Json use JsonConvert.SerializeObject(data)
            File.WriteAllText(path, jsonData);
            Debug.Log("Player data saved successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save player data: " + e.Message);
        }
    }

    public static PlayerData LoadPlayer()
    {
        if (File.Exists(path))
        {
            try
            {
                string jsonData = File.ReadAllText(path);
                PlayerData data = JsonSerializer.Deserialize<PlayerData>(jsonData); // For Newtonsoft.Json use JsonConvert.DeserializeObject<PlayerData>(jsonData)
                Debug.Log("Player data loaded successfully.");
                return data;
            }
            catch (IOException e)
            {
                Debug.LogError("Failed to load player data: " + e.Message);
                return null;
            }
            catch (System.Exception e)
            {
                Debug.LogError("An error occurred: " + e.Message);
                return null;
            }
        }
        else
        {
            Debug.LogError("Saved file not found in " + path);
            return null;
        }
    }
}

*/ 