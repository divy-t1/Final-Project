using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxHealth = 3; 
    public int currentHealth; 
    public Animator anim; 
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
            anim.SetBool("IsDead", true); 
        }
    }

    public void Heal(int amount) {
        currentHealth += amount; 

        if (currentHealth > maxHealth) {
            currentHealth = maxHealth; 
        }
    }
}
