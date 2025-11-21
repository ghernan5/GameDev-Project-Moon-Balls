using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SpearBoss : MonoBehaviour
{
    private Vector2 target;
    public GameObject spear;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool facingRight = true;
    private bool canMove = true;
    private bool attacking = false;
    private bool exhausted = false;
    [SerializeField] float speed;
    [SerializeField] int health;


    void Start()
    {
        speed = speed == 0 ? 0.075f : speed;
        health = health == 0 ? 1 : health;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (canMove && !attacking && !exhausted)
        {
            target = FindFirstObjectByType<playerScript>().transform.position;
            Vector2 direction;
            //
            if(Vector3.Distance(transform.position, target) > 4f)
            {
                direction = (target - rb.position).normalized;
                Debug.Log("Direction: " + direction);
            }
            else
            {
                //get direction for spear and attack
                direction = (target - rb.position).normalized;
                StartCoroutine(ThrustAttack(direction));
                direction = Vector3.zero; // may not need this
            }
            rb.linearVelocity = direction * speed;
            if ((rb.linearVelocityX > 0 && !facingRight) || (rb.linearVelocityX < 0 && facingRight)) Flip();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Weapon") && (!attacking || exhausted))
        {
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            health -= 1; // maybe take in a damage parameter
            StartCoroutine(FlashRed());
            if(health <= 0)
            {
                StartCoroutine(Knockback(knockbackDirection, 15f, 0.2f));
                StartCoroutine(Die());
            }
            else
            {
                StartCoroutine(Knockback(knockbackDirection, 10f, 0.3f));
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
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }

    IEnumerator Die()
    {
        Debug.Log("Dying");
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    IEnumerator ThrustAttack(Vector2 direction)
    {
        //allow not to get knockback when attacking
        Debug.Log("Beginning Thrust Attack!");
        attacking = true;

        //snap spear to face player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (!facingRight) angle = 180f - angle;
        spear.transform.localRotation = Quaternion.Euler(0, 0, angle - 90f);
        yield return new WaitForSeconds(0.5f); // giving player time to react

        speed += 5f;
        rb.linearVelocity = direction * speed;
        yield return new WaitForSeconds(1f); //charge length
        speed -= 5f;
        attacking = false;
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        exhausted = true;
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(1.0f);
        spear.transform.localRotation = Quaternion.Euler(0,0,0);
        yield return new WaitForSeconds(1.0f);
        exhausted = false;
    }
}
