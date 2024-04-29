using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxHealth = 10; 
    public int currentHealth; 
       void Start()
    {
        currentHealth = maxHealth; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TakeDamage(int amount) {
        currentHealth -= amount; 

        if (currentHealth <= 0) {
            //when health is lower or equal to zero, game should be over 
            //add script later 
        }
    }

    public void Heal(int amount) {
        currentHealth += amount; 

        if (currentHealth > maxHealth) {
            currentHealth = maxHealth; 
        }
    }
}
