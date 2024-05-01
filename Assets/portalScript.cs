using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalScript : MonoBehaviour
{
    /*
    If you want to deal damage to the player through collisions, this is not the way to go. 
    As you said it yourself, you basically want to apply different amounts of damage to the player on 
    collision. So what you can do is create an asbtract baseclass, or an interface, 
    "DamagingCollisionObject", which has some function "GetDamageAmount". This function would return a 
    different amount of damage, depending on the class implementing it or inheriting it. A Skeleton may 
    deal 5 damage on collision, while Lava may deal 500 and instantly kill the player.Now when the player 
    collides with something you can check if the other object (collision.gameObject) has a component of 
    type DamagingCollisionObject, and if so you can deal GetDamageAmount many units of damage to the players 
    health. I already implied this, but you really should not have a function for any possible damage source 
    inside your health script. The amount of damage should come from the object dealing the damage. 
    Anything else will reduce scalability and increase the amount of maintenance for your project.
    */
    public Rigidbody2D charRB;
    
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision) {
        //float speed = 5F; 
        //Vector3 m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (collision.gameObject.name == "Circle") {
            transform.position = new Vector3(-10.62f, 3.614f, 0.00f); 
            charRB.velocity = Vector3.zero; 

        

        }
        Debug.Log("Trigger enter"); 
    }
}
