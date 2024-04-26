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

        //Move(GetDirection()); 
        
        var move = new UnityEngine.Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0F); 
        //charRB.MovePosition(move * speed * Time.deltaTime);
        transform.position += move * speed * Time.deltaTime;
        
        /*
        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
                transform.position += Vector3.down * speed * Time.deltaTime;
        } 
        */
    }
/*
    private void Move(UnityEngine.Vector2 direction) {
        charRB.AddForce(direction.normalized * speed * Time.deltaTime); 
    }

    private UnityEngine.Vector2 GetDirection() {
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical"); 
        return new UnityEngine.Vector2(horizontal, vertical); 
    }
    */
}
