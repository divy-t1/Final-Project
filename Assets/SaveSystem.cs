using UnityEngine;
using System.IO; 
using System.Runtime.Serialization.Formatters.Binary;
using System.Data.Common;

public static class SaveSystem
{
    // Adjust the path to save the player data
    private static readonly string path = Application.persistentDataPath + "/player.data";

    public static void SavePlayer(GameManager gameManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        
        FileStream stream = new FileStream(path, FileMode.Create);

        // Get the position from the playerTransform in the GameManager
        Vector3 position = gameManager.playerTransform.position;

        PlayerData data = new PlayerData(gameManager.playerHealth.currentHealth, 
        new float[] { position.x, position.y, position.z });
        
        try
        {
            formatter.Serialize(stream, data);
            Debug.Log("Player data saved successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save player data: " + e.Message);
        }
        finally
        {
            stream.Close();
        }  
    }

    public static PlayerData LoadPlayer()
    {
        if (File.Exists(path))
        {
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

        }
        else
        {
            Debug.LogError("Saved File not found in " + path); 
            return null; 
        }
    }
}