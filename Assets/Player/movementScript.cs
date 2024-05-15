using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class movementScript : MonoBehaviour
{
    // Start is called before the first frame update


    float speed = 5.0F;  
    public Rigidbody2D charRB; 
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        var move = new UnityEngine.Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0F); 
        //charRB.MovePosition(move * speed * Time.deltaTime);
        transform.position += move * speed * Time.deltaTime;

    }

    private void OnCollisionEnter2D (Collision2D collision) {
        if (collision.gameObject.name == "Maze Walls") {
            charRB.velocity = UnityEngine.Vector3.back* 5; 
        }
        //Debug.Log("Collided with wall");  
        
    }
}
