using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; 
    public int currentHealth; 
    public HealthBar healthBar; 
    private bool isLoadingPlayerData = false; //bool to track whether player data is acc being loaded or not
    private bool isTakingDamage = false; 
    // Start is called before the first frame update
    void Start()
    {
        
        if (healthBar == null)
        {
            Debug.LogWarning("HealthBar reference is not assigned.");
        }
        currentHealth = maxHealth; 
        healthBar.SetMaxHealth(maxHealth); 
        
    }
    

    public void Heal(int amount){
        currentHealth += amount; 

        healthBar.SetHealth(currentHealth); 
    }

    public void SavePlayer() {
        SaveSystem.SavePlayer(this); 
    }

    public void LoadPlayer() {
        isLoadingPlayerData = true; //set bool to show that data is currently being loaded
        PlayerData data = SaveSystem.LoadPlayer();
        isLoadingPlayerData = false;  //set bool to show that data is done being loaded by reseting

        //update the health stats
        currentHealth = data.health; 
        healthBar.SetHealth(currentHealth);  

        //update the player position in the maze
        Vector3 position; 
        position.x = data.position[0]; 
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position; 
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage; 
        Debug.Log("take damage");
        healthBar.SetHealth(currentHealth);
        isTakingDamage = false;  
    }

    // Update is called once per frame
    void Update()
    {
        //can only occur when the player data is not being loaded and space bar is being pressed, so bool must be false
        if (!isLoadingPlayerData && Input.GetKeyDown(KeyCode.Space)) {
            if (!isTakingDamage)
            {
                TakeDamage(20);
                isTakingDamage = true;
            }
        } else {
            isTakingDamage = false; // Reset the flag if key is not pressed
        }
    }
}
