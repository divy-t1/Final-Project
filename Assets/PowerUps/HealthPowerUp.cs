using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/HealthBuff")]
public class HealthPowerUp : PowerUpEffect
{
    public int amount; 
    public override void Apply(GameObject target)
    {
        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>(); 
        playerHealth.currentHealth += amount; 

        HealthBar healthbar = target.GetComponent<HealthBar>(); 
        healthbar.SetHealth(playerHealth.currentHealth); 
        
    }
}
