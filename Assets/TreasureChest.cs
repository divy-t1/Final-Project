using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to store information about each item
[Serializable]
public class Item {
    public string name;
    public int value;
    public Sprite sprite;

    public Item(string name, int value, Sprite sprite) {
        this.name = name;
        this.value = value;
        this.sprite = sprite;
    }
}
public class TreasureChest : MonoBehaviour
{
    public Sprite closedChestSprite;  // Sprite for closed chest
    private Item item;                // Item stored in the chest
    private bool isOpened = false;    // Flag to track if the chest is opened
    private SpriteRenderer spriteRenderer;  // Reference to SpriteRenderer component
    private GameManager gameManager;  // Reference to the GameManager

    void Start() {
        // Initializes the sprite renderer and sets the chest image to the closed one 
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closedChestSprite;  // Set closed chest sprite initially
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !isOpened) {
            OpenChest();  // Open the chest if player collides with it
        }
    }

    void OpenChest() {
        isOpened = true;  // Set the chest as opened
        spriteRenderer.sprite = item.sprite;  // Change the sprite to display the item
        if (gameManager != null) {
            gameManager.AddToScore(item.value);  // Add the item's value to the score
            gameManager.DisplayTempValue(item.value);  // Display the item's value temporarily
        } else {
            Debug.LogWarning("GameManager reference is missing in TreasureChest");
        }
    }

    public void Initialize(Item item, GameManager gameManager) {
        this.item = item;
        this.gameManager = gameManager;
    }
}
