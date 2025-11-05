using System.Runtime.CompilerServices;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    private Vector2 target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        target = FindFirstObjectByType<playerScript>().transform.position;
        transform.position = Vector2.MoveTowards(transform.position,target,.1f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "sword") Destroy(gameObject);
    }
}
