using System.Collections;
using UnityEngine;

public class enemyShooter : MonoBehaviour
{
    private Vector2 target;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool facingRight = true;
    private bool canMove = true;
    [SerializeField] float speed;
    [SerializeField] float damage;
    [SerializeField] int health;

    void Start()
    {
        damage = damage == 0 ? 1f : damage;
        speed = speed == 0 ? 0.075f : speed;
        health = health == 0 ? 1 : health;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            target = FindFirstObjectByType<playerScript>().transform.position;
            Vector2 direction;
            //keep a distance away from the player
            if(Vector3.Distance(transform.position, target) < 5f )
            {
                direction = (target + rb.position).normalized;
            }
            else if(Vector3.Distance(transform.position, target) > 6f)
            {
                direction = (target - rb.position).normalized;
            }
            else
            {
                //remain still
                direction = Vector3.zero;
            }
            rb.linearVelocity = direction * speed;
            if ((rb.linearVelocityX > 0 && !facingRight) || (rb.linearVelocityX < 0 && facingRight)) Flip();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "sword")
        {
            health -= 1;
            if(health <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
                StartCoroutine(Knockback(knockbackDirection, 20f, 0.3f));
                StartCoroutine(FlashRed());
            }
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    IEnumerator Knockback(Vector2 direction, float force, float duration)
    {
        canMove = false;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        float timer = 0f;
        while (timer < duration)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, Time.deltaTime * 5f);
            timer += Time.deltaTime;
            yield return null;
        }
        rb.linearVelocity = Vector2.zero;
        canMove = true;
    }

    IEnumerator FlashRed()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = new Color(0f, -0.5f, -0.5f) + originalColor;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = originalColor;
    }

    //to do: enemy needs a bullet
}
