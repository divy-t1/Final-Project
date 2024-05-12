using UnityEngine;
using System.IO; 
using System.Runtime.Serialization.Formatters.Binary;
using System.Data.Common;

public static class SaveSystem
{ // a static class makes it so that the class isn't instantiatable and wont make multiple versions elsewhere
    
        public static void SavePlayer(PlayerHealth playerHealth) {
            BinaryFormatter formatter = new BinaryFormatter(); 
            string path = Application.persistentDataPath + "/player.data"; 
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

            string path = Application.persistentDataPath + "/player.data"; 
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
