
using UnityEngine;
using UnityEngine.InputSystem;

public class playerScript : MonoBehaviour
{

    // Sprint 1 Goal: Make basic movement and find best attacking method (game feel)
    public float speed;
    public int health; // work on later
    public float swordLength; // work on later

    private Vector2 movement;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        
    }

    void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
        rb.linearVelocity = movement * speed;
    }
    
    //player basic attack
    void OnSwing()
    {
        Debug.Log("player swings!");
    }
}
