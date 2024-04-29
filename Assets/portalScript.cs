using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D charRB;
    public HealthScript healthScript; 

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

            //var healthComponent =  
            if (healthScript.currentHealth >= 0) {
                //healthScript.TakeDamage(1); 
            }

        }
        Debug.Log("Trigger enter"); 
    }
}
