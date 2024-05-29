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
    public int maintainPrefabCount;
    public int currentPrefabCount;
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
    //public TextMeshProUGUI scoreboardText; // UI Text component to display the score
    //private List<TreasurePoint> treasurePoints = new List<TreasurePoint>();
    private int playerScore = 0;
    public List<Sprite> itemSprites;     // List of sprites for different items
    public TextMeshProUGUI itemValueText;  // Text UI component to display item value
    private List<Item> items = new List<Item>();   // List to store item information
    public GameObject treasureChestPrefab; // Reference to the prefab of the treasure chest
    private List<TreasureChest> treasureChests = new List<TreasureChest>();  // List to store spawned treasure chests



    void Start() {
        InitializeSpawnPoints(); 
        InitializeMazeObjects(); 
        LoadPlayerData(); 
        InitializeTreasureChests();// Initialize treasure chests
        //UpdateScoreboard(); // Initialize scoreboard
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
            //UpdateScoreboard();

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
        if (mazeObjects[index].currentPrefabCount >= mazeObjects[index].maintainPrefabCount)
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
    
    private void InitializeItems() {
        // Add item information to the list
        items.Add(new Item("Gold Coin", 10, itemSprites[0]));
        items.Add(new Item("Silver Coin", 5, itemSprites[1]));
        items.Add(new Item("Bronze Coin", 1, itemSprites[2]));
        items.Add(new Item("Diamond", 100, itemSprites[3]));

        SelectionSortItems(items);  // Sort items based on their values
    }

    // Method to sort items based on their values using selection sort algorithm
    private void SelectionSortItems(List<Item> items) {
        for (int i = 0; i < items.Count - 1; i++) {
            int minIndex = i;
            for (int j = i + 1; j < items.Count; j++) {
                if (items[j].value < items[minIndex].value) {
                    minIndex = j;
                }
            }
            Item temp = items[minIndex];
            items[minIndex] = items[i];
            items[i] = temp;
        }
    }

    // Method to initialize treasure chests at spawn points
    private void InitializeTreasureChests() {
        foreach (var position in availableSpawnPoints) {
            SpawnTreasureChest(position);  // Spawn a treasure chest at each spawn point
        }
    }

    // Method to spawn a treasure chest at a given position
    private void SpawnTreasureChest(Vector3 position) {
        // Instantiate the treasure chest prefab
        GameObject chestObject = Instantiate(treasureChestPrefab, position, Quaternion.identity);
        TreasureChest treasureChest = chestObject.GetComponent<TreasureChest>();

        // Select a random item for the chest
        Item randomItem = items[UnityEngine.Random.Range(0, items.Count)];
        treasureChest.Initialize(randomItem, this);  // Initialize the chest with the selected item and reference to GameManager
        treasureChests.Add(treasureChest);  // Add the chest to the list of spawned chests
    }

    // Method to display the value of the collected item
    public void DisplayItemValue(int value) {
        itemValueText.text = "Item Value: " + value;
    }

} 





