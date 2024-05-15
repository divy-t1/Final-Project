using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 


public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public int damage; 
    private bool isTakingDamage = false;

    void Start()
    {
        if (healthBar == null)
        {
            Debug.LogWarning("HealthBar reference is not assigned.");
        }

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void SetHealth(int health)
    {
        currentHealth = health;
        healthBar.SetHealth(currentHealth);
    }


    void Update()
    {
        
    }
}

