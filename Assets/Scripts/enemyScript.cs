using UnityEngine;

public class enemyScript : MonoBehaviour
{
    private Vector2 target;
    private Rigidbody2D rb;
    private float movement;
    public bool facingRight = true;
    [SerializeField]
    float speed;
    float damage;

    void Start()
    {
        damage = damage == 0 ? 1f : damage;
        speed = speed == 0 ? 0.075f : speed;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        target = FindFirstObjectByType<playerScript>().transform.position;
        transform.position = Vector2.MoveTowards(transform.position, target, speed);

        movement = target.x-transform.position.x;
        if (movement > 0 && !facingRight) Flip();
        else if (movement < 0 && facingRight) Flip();
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
