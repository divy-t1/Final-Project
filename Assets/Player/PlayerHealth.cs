using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; 
    public int currentHealth; 
    public HealthBar healthBar; 
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
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            TakeDamage(20); 
        }
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage; 
        Debug.Log("take damage");
        healthBar.SetHealth(currentHealth); 
    }

    public void Heal(int amount){
        currentHealth += amount; 

        healthBar.SetHealth(currentHealth); 
    }
}
