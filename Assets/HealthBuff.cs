using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; 

public class HealthBuff : MonoBehaviour
{
    public int amount; 
    public HealthBar healthBar; 
    public PlayerHealth playerHealth; 

  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player"){
            playerHealth.currentHealth += amount; 
            healthBar.SetHealth(playerHealth.currentHealth); 
            Destroy(gameObject); 
            //SpawnObject(); 
        }
    }

    private void SpawnObject() {
        Instantiate(gameObject, transform.position, Quaternion.identity); 
    }
}
