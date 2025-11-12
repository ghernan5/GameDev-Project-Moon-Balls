using Mono.Cecil.Cil;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    private Vector2 target;
    private Rigidbody2D rb;
    private float movement;
    public bool facingRight = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        target = FindFirstObjectByType<playerScript>().transform.position;
        transform.position = Vector2.MoveTowards(transform.position, target, .075f);
        movement = target.x-transform.position.x;
        if (movement > 0.01 && !facingRight) Flip();
        else if (movement < 0.01 && facingRight) Flip();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "sword") Destroy(gameObject);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
