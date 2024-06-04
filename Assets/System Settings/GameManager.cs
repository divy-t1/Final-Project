
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

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
    public Transform[] spawnPoints;
    private List<Vector3> availableSpawnPoints = new List<Vector3>();

    public List<Sprite> itemSprites;
    private List<Item> items = new List<Item>();
    public GameObject treasureChestPrefab;
    private List<TreasureChest> treasureChests = new List<TreasureChest>();
    private List<Vector3> availableChestSpawnPoints = new List<Vector3>();
    public Transform[] chestSpawnPoints;

    private int totalScore = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI tempValueText;
    public int TotalScore => totalScore;  // Expose totalScore for saving

    void Start() {
        InitializeSpawnPoints(); 
        InitializeMazeObjects(); 
        LoadPlayerData(); 
        InitializeItems(); 
        InitializeChestSpawnPoints();
        InitializeTreasureChests();// Initialize treasure chests
        UpdateScoreUI(); 
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
            
            // Set the total score from loaded data
            totalScore = loadedData.score;
            UpdateScoreUI();
        }
        else
        {
            Debug.LogError("Failed to load player data.");
        }
    } 

    // Switch Scene to Options Menu 
    public void BackToMain() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
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
        foreach (var mazeObject in mazeObjects) {
            mazeObject.currentPrefabCount = 0;  // Reset currentPrefabCount here
        }

        for (int i = 0; i < mazeObjects.Count; i++) {
            SpawnPrefabRecursive(i);
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

            // Check if we still need more instances
        if (mazeObjects[index].currentPrefabCount < mazeObjects[index].maintainPrefabCount) {
            SpawnPrefabRecursive(index); // Recursive call to continue spawning
        }
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
////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void InitializeItems() {
        // Add item information to the list
        items.Add(new Item("Gold Coin", 10, itemSprites[0]));
        items.Add(new Item("Silver Coin", 5, itemSprites[1]));
        items.Add(new Item("Empty Chest", 0, itemSprites[2]));
        items.Add(new Item("Diamond", 100, itemSprites[3]));

        SelectionSortItems(items);  // Sort items based on their values
    }

    private void InitializeChestSpawnPoints() {
        foreach (var spawnPoint in chestSpawnPoints) {
            availableChestSpawnPoints.Add(spawnPoint.position); 
        }
        Debug.Log("Initialized " + availableChestSpawnPoints.Count + " chest spawn points.");
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

    private void ShuffleItems(List<Item> items) {
        for (int i = items.Count - 1; i > 0; i--) {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            Item temp = items[i];
            items[i] = items[randomIndex];
            items[randomIndex] = temp;
        }
    }

    private void InitializeTreasureChests() {
        foreach (var position in availableChestSpawnPoints) {
            SpawnTreasureChest(position);
        }
    }

    private void SpawnTreasureChest(Vector3 position) {
        GameObject chestObject = Instantiate(treasureChestPrefab, position, Quaternion.identity);
        TreasureChest treasureChest = chestObject.GetComponent<TreasureChest>();

        Item randomItem = items[UnityEngine.Random.Range(0, items.Count)];
        treasureChest.Initialize(randomItem, this);
        treasureChests.Add(treasureChest);
    }
//////////////////////////////////////////////////////////////////////////////////////////////
    public void AddToScore(int value) {
        totalScore += value;
        scoreText.text = "Score: " + totalScore;
    }

    // Method to update the score UI
    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + totalScore;
    }

    public void DisplayTempValue(int value) {
        StartCoroutine(DisplayTempValueCoroutine(value));
    }

    private IEnumerator DisplayTempValueCoroutine(int value) {
        tempValueText.text = "+" + value;
        tempValueText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        tempValueText.gameObject.SetActive(false);
    }

} 


/*
private void SpawnTreasureChests(Vector3[] positions, int goldCount, int silverCount, int emptyCount, int diamondCount) {
    // Create a list to store the items for spawning
    List<Item> itemsToSpawn = new List<Item>();

    // Add specified number of gold chests
    for (int i = 0; i < goldCount; i++) {
        itemsToSpawn.Add(new Item("Gold Coin", 10, itemSprites[0]));
    }

    // Add specified number of silver chests
    for (int i = 0; i < silverCount; i++) {
        itemsToSpawn.Add(new Item("Silver Coin", 5, itemSprites[1]));
    }

    // Add specified number of empty chests
    for (int i = 0; i < emptyCount; i++) {
        itemsToSpawn.Add(new Item("Empty Chest", 0, itemSprites[2]));
    }

    // Add specified number of diamond chests
    for (int i = 0; i < diamondCount; i++) {
        itemsToSpawn.Add(new Item("Diamond", 100, itemSprites[3]));
    }

    // Shuffle the list to ensure randomness
    ShuffleItems(itemsToSpawn);

    // Spawn treasure chests with the shuffled items
    for (int i = 0; i < positions.Length; i++) {
        GameObject chestObject = Instantiate(treasureChestPrefab, positions[i], Quaternion.identity);
        TreasureChest treasureChest = chestObject.GetComponent<TreasureChest>();
        treasureChest.Initialize(itemsToSpawn[i], this);
        treasureChests.Add(treasureChest);
    }
}

private void ShuffleItems(List<Item> items) {
        for (int i = items.Count - 1; i > 0; i--) {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            Item temp = items[i];
            items[i] = items[randomIndex];
            items[randomIndex] = temp;
        }
    }

    public void AddToScore(int value) {
        totalScore += value;
        scoreText.text = "Score: " + totalScore;
    }

    public void DisplayTempValue(int value) {
        StartCoroutine(DisplayTempValueCoroutine(value));
    }

    private IEnumerator DisplayTempValueCoroutine(int value) {
        tempValueText.text = "+" + value;
        tempValueText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        tempValueText.gameObject.SetActive(false);
    }

    We've added a method InitializeTreasureChests to specify the counts for each type of chest (goldCount, silverCount, emptyCount, and diamondCount) and spawn chests accordingly.
The SpawnTreasureChests method now takes additional parameters for the counts of each type of chest.
We've added methods AddToScore to update the total score and DisplayTempValue to display the temporary value when a chest is opened.
The DisplayTempValueCoroutine method uses a coroutine to display the temporary value for 2 seconds before hiding it.

*/


