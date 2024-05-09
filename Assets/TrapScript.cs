using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class TrapScript : MonoBehaviour
{
    public int damage = 10; 
    private Rigidbody2D charRB; 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        
        if (collision.gameObject.tag == "Player") {
            GameObject p = collision.gameObject;
            charRB = p.GetComponent<Rigidbody2D>();
            p.transform.position = new Vector3(1.55f, -0.22f, 0.00f); 
            charRB.velocity = Vector3.zero; 
            //transform.position = respawn.transform.position; 
            p.GetComponent<PlayerHealth>().TakeDamage(damage);

            Destroy(gameObject); 


        }
    }

    public void spawnObject() {
        Instantiate(gameObject, transform.position, Quaternion.identity); 
    }
}
