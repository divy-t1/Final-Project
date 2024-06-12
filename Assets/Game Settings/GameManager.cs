using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
class ObjectData {
    public GameObject prefab; // Prefab for the maze object
    public int maintainPrefabCount; // Desired number of instances
    public int currentPrefabCount; // Current number of instances
}

[Serializable]
public class GameManager : MonoBehaviour {
    public PlayerHealth playerHealth; // Reference to player health component
    public Transform playerTransform; // Reference to player transform
    [SerializeField] private List<ObjectData> mazeObjects; // List of maze objects
    public Transform[] spawnPoints; // Array of spawn points for maze objects
    private List<Vector3> availableSpawnPoints = new List<Vector3>(); // Available spawn points

    public List<Sprite> itemSprites; // List of sprites for items
    private List<Item> items = new List<Item>(); // List of items
    public GameObject treasureChestPrefab; // Prefab for the closed treasure chest
    private List<TreasureChest> treasureChests = new List<TreasureChest>(); // List of treasure chests
    private List<Vector3> availableChestSpawnPoints = new List<Vector3>(); // Available chest spawn points
    public Transform[] chestSpawnPoints; // Array of chest spawn points

    private int totalScore = 0; // Player's total score
    public TextMeshProUGUI scoreText; // UI text element for displaying score
    public TextMeshProUGUI tempValueText; // UI text element for displaying temporary values
    public int TotalScore => totalScore; // Property to get the total score

    // Calls all methods that initialize in the beginning
    void Start() {
        InitializeSpawnPoints();
        InitializeMazeObjects();
        LoadPlayerData();
        InitializeItems();
        InitializeChestSpawnPoints();
        InitializeTreasureChests();
        UpdateScoreUI();
        // Order is important, can't change otherwise errors are gonna be thrown 
    }

    // Method to save player data by referencing the method in the save system script
    public void SavePlayerData() {
        if (playerTransform != null) {
            SaveSystem.SavePlayer(this);
        } else {
            Debug.LogError("Player Transform is null. Cannot save player data.");
        }
    }

    // Method to load player data by referencing the method in the save system script
    public void LoadPlayerData() {
        PlayerData loadedData = SaveSystem.LoadPlayer();
        if (loadedData != null) {
            // As the data is retuned, it assigns the properties to the player character from the player data
            playerHealth.SetHealth(loadedData.health);

            Vector3 position = new Vector3(loadedData.position[0], loadedData.position[1], loadedData.position[2]);
            playerTransform.position = position;

            totalScore = loadedData.score;
            UpdateScoreUI();
        } else {
            Debug.LogWarning("No save data found or failed to load. Starting new game.");
            // Initialize player data to default values if no save data is found
            playerHealth.SetHealth(playerHealth.maxHealth);
            playerTransform.position = Vector3.zero;
            totalScore = 0;
            UpdateScoreUI();
        }
    }

   
////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void InitializeSpawnPoints() {
        availableSpawnPoints.Clear(); // Clear the list of available spawn points
        if (spawnPoints != null) {
            foreach (var spawnPoint in spawnPoints) {
                if (spawnPoint != null) {
                    availableSpawnPoints.Add(spawnPoint.position); // Add spawn point positions to the list
                }
            }
            Debug.Log("Initialized " + availableSpawnPoints.Count + " spawn points.");
        }
    }

    private void InitializeMazeObjects() {
        foreach (var mazeObject in mazeObjects) {
            mazeObject.currentPrefabCount = 0; // Reset the current prefab count
        }

        for (int i = 0; i < mazeObjects.Count; i++) {
            SpawnPrefabRecursive(i); // Recursively spawn prefabs for the beginning 
        }
    }

    void SpawnPrefabRecursive(int index) {
        if (mazeObjects[index].currentPrefabCount >= mazeObjects[index].maintainPrefabCount) {
            Debug.Log("Limit reached for " + mazeObjects[index].prefab.name + ". No need to spawn more prefabs.");
            return; // Stop if the needed number of prefabs is reached
        }

        if (availableSpawnPoints.Count == 0) {
            Debug.Log("No available spawn points left");
            return; // Stop if there are no available spawn points left in the list
        }

        // Select a random spawn point, assign it and then take off the list so other prefabs don't randomly use same spawn point and spawn over each other
        int randomIndex = UnityEngine.Random.Range(0, availableSpawnPoints.Count);
        Vector3 spawnPoint = availableSpawnPoints[randomIndex];
        availableSpawnPoints.RemoveAt(randomIndex);

        // Create a new prefab of the item using the index and spawn point position 
        GameObject newPrefab = Instantiate(mazeObjects[index].prefab, spawnPoint, Quaternion.identity);
        mazeObjects[index].currentPrefabCount++; // Add one to the count to show prefab has been spawned

        // Set the gameManager and ObjectIndex 
        IMazeObject newMazeObject = newPrefab.GetComponent<IMazeObject>();
        newMazeObject.gameManager = this;
        newMazeObject.ObjectIndex = index;
        Debug.Log("Spawned " + mazeObjects[index].prefab.name + ". Current count: " + mazeObjects[index].currentPrefabCount);

        // Keep doing till count is reached by recursively counting 
        if (mazeObjects[index].currentPrefabCount < mazeObjects[index].maintainPrefabCount) {
            SpawnPrefabRecursive(index); // Recursive call to continue spawning
        }
    }

    // Method to inform gameManager that the prefab has been destroyed
    public void PrefabDestroyed(int index, Vector3 position) {
        mazeObjects[index].currentPrefabCount--;
        // Decreases count to show a prefab has been removed
        Debug.Log(mazeObjects[index].prefab.name + " destroyed. Current count: " + mazeObjects[index].currentPrefabCount);

        // Call the method to recursively spawn till count is reached and add back position into the list 
        SpawnPrefabRecursive(index);
        availableSpawnPoints.Add(position);
    }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void InitializeItems() {
        // Initialize list of items with their name and value into their respective index 
        items.Add(new Item("Gold Coin", 10, itemSprites[0]));
        items.Add(new Item("Silver Coin", 5, itemSprites[1]));
        items.Add(new Item("Empty Chest", 0, itemSprites[2]));
        items.Add(new Item("Diamond", 100, itemSprites[3]));

        SelectionSortItems(items);  // Sort items based on their values
    }

    private void InitializeChestSpawnPoints() {
        availableChestSpawnPoints.Clear(); // Clear the list of available chest spawn points
        foreach (var spawnPoint in chestSpawnPoints) {
            availableChestSpawnPoints.Add(spawnPoint.position); // Add chest spawn point positions to the list
        }
        Debug.Log("Initialized " + availableChestSpawnPoints.Count + " chest spawn points.");
    }

    private void SelectionSortItems(List<Item> items) {
        for (int i = 0; i < items.Count - 1; i++) {
            int minIndex = i;
            for (int j = i + 1; j < items.Count; j++) {
                if (items[j].value < items[minIndex].value) {
                    minIndex = j; // Find the item with the smallest value
                }
            }
            // Swap the items to sort them by value
            Item temp = items[minIndex];
            items[minIndex] = items[i];
            items[i] = temp;
        }
    }

    private void InitializeTreasureChests() {
        foreach (var position in availableChestSpawnPoints) {
            SpawnTreasureChest(position); // Spawn treasure chests at each spawn point
        }
    }

    private void SpawnTreasureChest(Vector3 position) {
        GameObject chestObject = Instantiate(treasureChestPrefab, position, Quaternion.identity); // Spawn the closed treasure chest prefab
        TreasureChest treasureChest = chestObject.GetComponent<TreasureChest>();

        Item randomItem = items[UnityEngine.Random.Range(0, items.Count)]; // Select a random item from the list
        treasureChest.Initialize(randomItem, this); // Initialize the treasure chest with the item and game manager
        treasureChests.Add(treasureChest); // Add the treasure chest to the list
    }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void AddToScore(int value) {
        totalScore += value; // Increase the total score
        scoreText.text = "Score: " + totalScore; // Update the score text
    }

    private void UpdateScoreUI() {
        scoreText.text = "Score: " + totalScore; // Update the score text
    }

    public void DisplayTempValue(int value) {
        StartCoroutine(DisplayTempValueCoroutine(value)); // Display the temporary value
    }

    private IEnumerator DisplayTempValueCoroutine(int value) {
        tempValueText.text = "+" + value; // Set the temporary value text
        tempValueText.gameObject.SetActive(true); // Show the temporary value text
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        tempValueText.gameObject.SetActive(false); // Hide the temporary value text
    }
}
