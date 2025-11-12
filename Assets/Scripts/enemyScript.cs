using Mono.Cecil.Cil;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    private Vector2 target;
    private Rigidbody2D rb;
    private float movement;
    bool facingRight = true;
    [SerializeField] float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        target = FindFirstObjectByType<playerScript>().transform.position;
        Vector2 direction = (target - rb.position).normalized;
        rb.linearVelocity = direction * speed;

        if ((rb.linearVelocityX > 0 && !facingRight) || (rb.linearVelocityX < 0 && facingRight)) Flip();
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
