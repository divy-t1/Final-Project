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
    private Collider2D chestCollider; // Reference to Collider2D component
    private SpriteRenderer spriteRenderer;  // Reference to SpriteRenderer component
    private GameManager gameManager;  // Reference to the GameManager

    void Start() {
        // Initializes the sprite renderer and sets the chest image to the closed one 
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closedChestSprite;  // Set closed chest sprite initially

        // Initialize the collider
        chestCollider = GetComponent<Collider2D>();
        if (chestCollider == null) {
            Debug.LogError("Collider2D component is missing from the treasure chest.");
        } else {
            chestCollider.isTrigger = true;  // Make the chest passable initially
        }
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

        // Change the collider to be a regular collider after the chest is opened
        if (chestCollider != null) {
            chestCollider.isTrigger = false;  // Make the chest solid
        }
    }

    public void Initialize(Item item, GameManager gameManager) {
        this.item = item;
        this.gameManager = gameManager;
    }
}
