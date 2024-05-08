using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class TrapTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage; 
    private Rigidbody2D rb; 
    public HealthBar healthBar; 
    public PlayerHealth playerHealth; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        playerHealth.currentHealth += damage; 
        healthBar.SetHealth(playerHealth.currentHealth);
    }
}
