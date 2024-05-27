using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

//Define a simple class to hold the information needed for each type of object.
[Serializable]
class ObjectData {
    public GameObject prefab;
    public int initialPrefabCount;
    public int currentPrefabCount;
}

//These classes will hold the data for each item and treasure point 
[Serializable]
public class Item {
    public string name;
    public int value;

    public Item(string name, int value) {
        this.name = name;
        this.value = value;
    }
}

[Serializable]
public class TreasurePoint {
    public Vector3 position;
    public List<Item> items;

    public TreasurePoint(Vector3 position, List<Item> items) {
        this.position = position;
        this.items = items;
    }
}
//////////////////////////////////////////////////////////////////////////////////////////////////////////
[Serializable]
public class GameManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Transform playerTransform;
    [SerializeField] private List<ObjectData> mazeObjects;
    public Transform[] spawnPoints; // an array of the transform portion of the spawnpoints 
    private List<Vector3> availableSpawnPoints = new List<Vector3>();
    public TextMeshProUGUI scoreboardText; // UI Text component to display the score
    private List<TreasurePoint> treasurePoints = new List<TreasurePoint>();
    private int playerScore = 0;


    void Start() {
        InitializeSpawnPoints(); 
        InitializeMazeObjects(); 
        LoadPlayerData(); 
        InitializeTreasurePoints(); // Initialize treasure points
        UpdateScoreboard(); // Initialize scoreboard
    } 

    // Method to initialze the list of available spawn points 
    private void InitializeSpawnPoints() {
        // Loop through each spawn point in the array of transform components 
        foreach (var spawnPoint in spawnPoints) {
            // Add the position of each spawn point to the list of the available spawn points
            availableSpawnPoints.Add(spawnPoint.position); 
        }
        Debug.Log("Initialized " + availableSpawnPoints.Count + " spawn points.");
    }

    // Initialize and spawn the initial prefabs for each spawnable object
    private void InitializeMazeObjects() {
        // Loop through each objectdata in the maze object list 
        for (int i = 0; i < mazeObjects.Count; i++) {
            // Reset currentPrefabCount to ensure correct initialization
            mazeObjects[i].currentPrefabCount = 0;
            // Spawn the initial number of prefabs specified for each object type 
            SpawnPrefabRecursive(i);
        }
    }

    // Method to initialize treasure points with items
    private void InitializeTreasurePoints() {
        // Iterate through each available spawn point position
        foreach (var position in availableSpawnPoints) {
            // Create a list of items for each treasure point
            List<Item> items = new List<Item> {
                new Item("Gold Coin", 10),  // Gold Coin worth 10 points
                new Item("Diamond", 100),   // Diamond worth 100 points
                new Item("Silver Coin", 5), // Silver Coin worth 5 points
                new Item("Trap", -20)       // Trap that reduces score by 20 points
            };
            // Sort the items list by their value using Selection Sort
            SelectionSortItems(items);
            // Create a new TreasurePoint with the current position and the sorted items list
            TreasurePoint treasurePoint = new TreasurePoint(position, items);
            // Add the created TreasurePoint to the list of treasure points
            treasurePoints.Add(treasurePoint);
        }
    }

    // Selection Sort algorithm to sort items by their value
    private void SelectionSortItems(List<Item> items) {
        // Iterate through the list of items (except the last item)
        for (int i = 0; i < items.Count - 1; i++) {
            int minIndex = i; // Assume the current item is the minimum
            // Iterate through the unsorted portion of the list
            for (int j = i + 1; j < items.Count; j++) {
                // Update minIndex if a smaller item is found
                if (items[j].value < items[minIndex].value) {
                    minIndex = j;
                }
            }
            // Swap the minimum item found with the current item
            Item temp = items[minIndex];
            items[minIndex] = items[i];
            items[i] = temp;
        }
    }

    // Linear search algorithm to find an item by name
    private Item FindItemByName(string name, List<Item> items) {
        // Iterate through the list of items
        foreach (var item in items) {
            // Compare each item's name with the provided name (case-insensitive)
            if (item.name.Equals(name, StringComparison.OrdinalIgnoreCase)) {
                return item; // Return the item if a match is found
            }
        }
        return null; // Return null if no match is found
    }

    // Method to update the scoreboard UI text with the current score
    private void UpdateScoreboard() {
        scoreboardText.text = "Score: " + playerScore;
    }

    // Method to handle the collection of an item by the player
    public void CollectItem(string itemName, Vector3 position) {
        // Find the treasure point at the specified position
        TreasurePoint treasurePoint = treasurePoints.Find(tp => tp.position == position);
        if (treasurePoint != null) {
            // Find the item within the treasure point's items list by name
            Item item = FindItemByName(itemName, treasurePoint.items);
            if (item != null) {
                playerScore += item.value; // Update the player's score with the item's value
                treasurePoint.items.Remove(item); // Remove the collected item from the list
                UpdateScoreboard(); // Update the scoreboard to reflect the new score
                Debug.Log("Collected: " + item.name + " | New Score: " + playerScore);
            }
        }
    }


    // Method to save player data
    public void SavePlayerData()
    {
        SaveSystem.SavePlayer(this);
    }
    

    public void LoadPlayerData()
    {
        PlayerData loadedData = SaveSystem.LoadPlayer();
        if (loadedData != null)
        {
            playerHealth.SetHealth(loadedData.health);

            // Set player position from loaded data
            Vector3 position = new Vector3(loadedData.position[0], loadedData.position[1], loadedData.position[2]);
            playerTransform.position = position;
            UpdateScoreboard();

            // Other data loading operations...
        }
        else
        {
            Debug.LogError("Failed to load player data.");
        }
    }

    void SpawnPrefabRecursive(int index)
    {
        // Base Case 1: check to see if the current number of prefab has reached the initial count
        if (mazeObjects[index].currentPrefabCount >= mazeObjects[index].initialPrefabCount)
        {
            Debug.Log("Limit reached for " + mazeObjects[index].prefab.name + ". No need to spawn more prefabs.");
            return; // Limit reached, no need to spawn any more prefabs
        }

        // Base Case 2: if there are no spawn points left 
        if (availableSpawnPoints.Count == 0) {
            Debug.Log("No available spawn points left"); 
            return; // no need to spawn anything 
        }

        // Randomly select a spawn point from the array
        int randomIndex = UnityEngine.Random.Range(0, availableSpawnPoints.Count);
        Vector3 spawnPoint = availableSpawnPoints[randomIndex]; 
        availableSpawnPoints.RemoveAt(randomIndex);

        // Instantiate the prefab at the spawn point
        GameObject newPrefab = Instantiate(mazeObjects[index].prefab, spawnPoint, Quaternion.identity);

        //increase the count of the prefabs by one 
        mazeObjects[index].currentPrefabCount++; 
        
        //When a new object is created, GameManager needs to give the object a reference
        //to this GameManager and tell the object which index of mazeObject is being
        //used to track its type of object.
        //This lets the spawned object properly call PrefabDestroyed later.
        IMazeObject newMazeObject = newPrefab.GetComponent<IMazeObject>();
        newMazeObject.gameManager = this;
        newMazeObject.ObjectIndex = index;
        Debug.Log("Spawned " + mazeObjects[index].prefab.name + ". Current count: " + mazeObjects[index].currentPrefabCount);

        // Recursive call to continue spawning 
        SpawnPrefabRecursive(index); 
    }

    public void PrefabDestroyed(int index, Vector3 position)
    {
        mazeObjects[index].currentPrefabCount--; // Decrease the current count by one
        Debug.Log(mazeObjects[index].prefab.name + " destroyed. Current count: " + mazeObjects[index].currentPrefabCount);

        // Spawn another prefab to maintain the count 
        SpawnPrefabRecursive(index); 

        //Don't add the previous position to the available list until after spawning
        //the new object. Otherwise there is a change you will spawn in exactly the same
        //place and the player will immediately collect it.
        availableSpawnPoints.Add(position);


    }
    

} 





