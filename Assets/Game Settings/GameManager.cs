using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
class ObjectData {
    public GameObject prefab;
    public int maintainPrefabCount;
    public int currentPrefabCount;
}

[Serializable]
public class GameManager : MonoBehaviour {
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
    public int TotalScore => totalScore;

    void Start() {
        InitializeSpawnPoints();
        InitializeMazeObjects();
        LoadPlayerData();
        InitializeItems();
        InitializeChestSpawnPoints();
        InitializeTreasureChests();
        UpdateScoreUI();
    }

    public void SavePlayerData() {
        if (playerTransform != null) {
            SaveSystem.SavePlayer(this);
        } else {
            Debug.LogError("Player Transform is null. Cannot save player data.");
        }
    }

    public void LoadPlayerData() {
        PlayerData loadedData = SaveSystem.LoadPlayer();
        if (loadedData != null) {
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
        availableSpawnPoints.Clear();
        if (spawnPoints != null) {
            foreach (var spawnPoint in spawnPoints) {
                if (spawnPoint != null) {
                    availableSpawnPoints.Add(spawnPoint.position);
                }
            }
            Debug.Log("Initialized " + availableSpawnPoints.Count + " spawn points.");
        }
    }

    private void InitializeMazeObjects() {
        foreach (var mazeObject in mazeObjects) {
            mazeObject.currentPrefabCount = 0;
        }

        for (int i = 0; i < mazeObjects.Count; i++) {
            SpawnPrefabRecursive(i);
        }
    }

    void SpawnPrefabRecursive(int index) {
        if (mazeObjects[index].currentPrefabCount >= mazeObjects[index].maintainPrefabCount) {
            Debug.Log("Limit reached for " + mazeObjects[index].prefab.name + ". No need to spawn more prefabs.");
            return;
        }

        if (availableSpawnPoints.Count == 0) {
            Debug.Log("No available spawn points left");
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, availableSpawnPoints.Count);
        Vector3 spawnPoint = availableSpawnPoints[randomIndex];
        availableSpawnPoints.RemoveAt(randomIndex);

        GameObject newPrefab = Instantiate(mazeObjects[index].prefab, spawnPoint, Quaternion.identity);
        mazeObjects[index].currentPrefabCount++;

        IMazeObject newMazeObject = newPrefab.GetComponent<IMazeObject>();
        newMazeObject.gameManager = this;
        newMazeObject.ObjectIndex = index;
        Debug.Log("Spawned " + mazeObjects[index].prefab.name + ". Current count: " + mazeObjects[index].currentPrefabCount);

        if (mazeObjects[index].currentPrefabCount < mazeObjects[index].maintainPrefabCount) {
            SpawnPrefabRecursive(index); // Recursive call to continue spawning
        }
    }

    public void PrefabDestroyed(int index, Vector3 position) {
        mazeObjects[index].currentPrefabCount--;
        Debug.Log(mazeObjects[index].prefab.name + " destroyed. Current count: " + mazeObjects[index].currentPrefabCount);

        SpawnPrefabRecursive(index);
        availableSpawnPoints.Add(position);
    }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void InitializeItems() {
        items.Add(new Item("Gold Coin", 10, itemSprites[0]));
        items.Add(new Item("Silver Coin", 5, itemSprites[1]));
        items.Add(new Item("Empty Chest", 0, itemSprites[2]));
        items.Add(new Item("Diamond", 100, itemSprites[3]));

        SelectionSortItems(items);  // Sort items based on their values
    }

    private void InitializeChestSpawnPoints() {
        availableChestSpawnPoints.Clear();
        foreach (var spawnPoint in chestSpawnPoints) {
            availableChestSpawnPoints.Add(spawnPoint.position);
        }
        Debug.Log("Initialized " + availableChestSpawnPoints.Count + " chest spawn points.");
    }

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
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void AddToScore(int value) {
        totalScore += value;
        scoreText.text = "Score: " + totalScore;
    }

    private void UpdateScoreUI() {
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
