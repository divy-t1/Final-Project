using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerButtons : MonoBehaviour
{
    public PlayerHealth playerHealth; // Reference to the PlayerHealth component

    // Called when the save button is clicked
    public void SavePlayer()
    {
        //add code later
    }

    // Called when the load button is clicked
    public void LoadPlayer()
    {/*
        PlayerData data = SaveSystem.LoadPlayer();

        if (data != null) {
            playerHealth.currentHealth = data.health;
            playerHealth.healthBar.SetHealth(playerHealth.currentHealth);

            Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
            playerHealth.transform.position = position;
        } else {
            Debug.LogError("Failed to load player data.");
        }*/
    }
}

