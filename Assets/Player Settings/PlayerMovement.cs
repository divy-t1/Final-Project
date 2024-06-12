using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;  // Movement speed of the player
    private Rigidbody2D rb2D;  // Reference to the player's Rigidbody2D component
    private UnityEngine.Vector2 movement;  // Stores the player's movement input
    private float originalSpeed;  // To store the original movement speed

    public Sprite frontSprite;  // Sprite for facing front
    public Sprite backSprite;   // Sprite for facing back
    public Sprite leftSprite;   // Sprite for facing left
    public Sprite rightSprite;  // Sprite for facing right

    private SpriteRenderer spriteRenderer;  // Reference to the SpriteRenderer component of the player 

    void Awake()
    {
        // Get the Rigidbody2D component attached to the player
        rb2D = GetComponent<Rigidbody2D>();
        originalSpeed = speed;  // Save the original move speed
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Get horizontal and vertical input (arrow keys or WASD keys)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Change sprite based on movement direction
        if (movement.x > 0) {
            spriteRenderer.sprite = rightSprite;
        } else if (movement.x < 0) {
            spriteRenderer.sprite = leftSprite;
        } else if (movement.y > 0) {
            spriteRenderer.sprite = backSprite;
        } else if (movement.y < 0) {
            spriteRenderer.sprite = frontSprite;
        }
    }

    void FixedUpdate()
    {
        // Calculate the new position of the player based on the input and movement speed
        UnityEngine.Vector2 newPosition = rb2D.position + movement * speed * Time.fixedDeltaTime;

        // Move the player's Rigidbody2D to the new position using the method MovePosition
        rb2D.MovePosition(newPosition);
    }

    public void StartSpeedBuff(float speedIncrease, float duration)
    {
        Debug.Log("Starting speed buff: " + speedIncrease + " for " + duration + " seconds.");
        StartCoroutine(SpeedBuffCoroutine(speedIncrease, duration));
    }

    private IEnumerator SpeedBuffCoroutine(float speedIncrease, float duration)
    {
        speed += speedIncrease;  // Increase the move speed
        Debug.Log("Speed increased to: " + speed);
        yield return new WaitForSeconds(duration);  // Wait for the duration of the buff
        speed = originalSpeed;  // Reset the move speed to the original value
        Debug.Log("Speed reset to original value: " + speed);
    }
}
