using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 


public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Max health the character can have
    public int currentHealth; // Current or constantly updated health value of character
    public HealthBar healthBar; // Reference to the slider componenet that will show health 


    void Start()
    {
        if (healthBar == null)
        {
            // If health bar is not assigned, warning to player that there is no health bar reference attached
            Debug.LogWarning("HealthBar reference is not assigned."); 
        }

        // At the start, sets the current health of the character to the max health it can get
        currentHealth = maxHealth;
        // Then updates the slider as well to reflect that 
        healthBar.SetMaxHealth(maxHealth);
    }

    // Method to set health value, just like in health bar, it updates the graphic of the health bar
    // This methods sets the value and updates the slider at once 
    public void SetHealth(int health)
    {
        // Sets the current health to the given health in argument to update characters current health 
        currentHealth = health;
        // Makes it so that current health can't go over the max health and under 0
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        } else if (currentHealth < 0) {
            currentHealth = 0;
        }
        healthBar.SetHealth(currentHealth);

        // Just makes it so that the die function is called if health hits or goes below 0
        if (currentHealth <= 0)
        { 
            Die();
        }
    }

    // Method to stop movement of character and give user warning their player has died 
    private void Die()
    {
        Debug.LogWarning("The character has died.");
        // Assuming you have a script controlling the player's movement called PlayerMovement
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
    }
}

