using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to store information about each item
[Serializable]
public class Item {
    public string name;      // Name of the item
    public int value;        // Value of the item
    public Sprite sprite;   // Sprite representing the item

    // Constructor to initialize item properties
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closedChestSprite;  // Set closed chest sprite initially
    }

    // Method called when another collider enters the chest's trigger zone
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !isOpened) {
            OpenChest();  // Open the chest if player collides with it
        }
    }

    // Method to open the chest
    void OpenChest() {
        isOpened = true;  // Set the chest as opened
        spriteRenderer.sprite = item.sprite;  // Change the sprite to display the item
        gameManager.DisplayItemValue(item.value);  // Display the item's value
    }

    // Method to initialize the chest with an item and GameManager reference
    public void Initialize(Item item, GameManager gameManager) {
        this.item = item;
        this.gameManager = gameManager;
    }
}
