using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    // Path variable to use in other methods, shows the path that the data is saved in 
    // Making it private and readonly controls how its used and can't change it 
    private static readonly string path = Application.persistentDataPath + "/player.data";

    // Method to save the data of the character, using data from gameManager specificallhy 
    public static void SavePlayer(GameManager gameManager) {
        // Shows that the file is a binary file as it uses a Binary Formatter 
        BinaryFormatter formatter = new BinaryFormatter();
        // Create a stream of file to the path that is in create mode to create new or overwrite existing file, to keep updating the player data and access current data
        FileStream stream = new FileStream(path, FileMode.Create);

        // Get the position from the playerTransform in the GameManager
        Vector3 position = gameManager.playerTransform.position;

        // Create a PlayerData object with the data from gameManager 
        PlayerData data = new PlayerData(gameManager.playerHealth.currentHealth,
        new float[] { position.x, position.y, position.z }, gameManager.TotalScore);
        
        try {
            // Serialize the data into the file stream in binary file 
            formatter.Serialize(stream, data);
            Debug.Log("Player data saved successfully.");
        } catch (System.Exception e) {
            Debug.LogError("Failed to save player data: " + e.Message);
        } finally {
            // Close stream to release the data 
            stream.Close();
        }
    }

    // Method to load or access the player data saved in the path and file stream 
    public static PlayerData LoadPlayer() {
        // Check if any file exists in the path 
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            try {
                // Use a file stream to open a file by putting the file stream in Open mode
                using (FileStream stream = new FileStream(path, FileMode.Open)) {
                    // If there is nothing in the file stream, crash system and inform user no saved data
                    if (stream.Length == 0) {
                        Debug.LogError("Save file is empty. Unable to load player data.");
                        return null;
                    }
                    // Deserialze the PlayerData object from binary into regular code
                    PlayerData data = formatter.Deserialize(stream) as PlayerData;
                    Debug.Log("Player data loaded successfully.");
                    // return that same data which has the health, position and score 
                    return data;
                }
            } catch (IOException e) {
                Debug.LogError("Failed to load player data: " + e.Message);
                return null;
            } catch (System.Runtime.Serialization.SerializationException e) {
                Debug.LogError("Failed to deserialize player data: " + e.Message);
                return null;
            }
        } else {
            Debug.LogWarning("Save file not found in " + path);
            return null;
        }
    }
}
