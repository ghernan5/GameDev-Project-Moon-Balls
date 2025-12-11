using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class playerScript : MonoBehaviour
{
    public float speed;
    [SerializeField] int health = 3;
    public GameObject sword;
    [SerializeField] private float swingSpeed = 10f;
    [SerializeField] private float swingAngle = 90f;
    private bool isSwinging = false;
    private bool facingRight = true;
    private bool canMove = true;
    private SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    private Quaternion startingSwordRotation;
    private Vector3 startingSwordPosition;
    private Vector2 movement;
    float rotated = 0f;
    float swingDirection;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        sword.SetActive(false);
    }

    void FixedUpdate()
    {
        if (isSwinging)
        {
            //stop player from moving while hit
            movement = Vector2.zero;
            swingDirection = facingRight ? 1f : -1f;
            if(rotated < swingAngle)
            {
                sword.transform.RotateAround(transform.localPosition, Vector3.forward, -swingSpeed * swingDirection);
                rotated += swingSpeed;
            } else
            {
                sword.SetActive(false);
                sword.transform.localRotation = startingSwordRotation;
                sword.transform.localPosition = startingSwordPosition;
                isSwinging = false;
                rotated = 0f;
            }
        }
    }

    void OnMove(InputValue value)
    {
        if (canMove)
        {
            movement = value.Get<Vector2>();
            rb.linearVelocity = movement * speed;
            if ((movement.x > 0 && !facingRight) || (movement.x < 0 && facingRight))
            {
                Flip();
            }
        }
    }

    void OnSwing()
    {
        if (!isSwinging)
        {
            startingSwordRotation = sword.transform.localRotation;
            startingSwordPosition = sword.transform.localPosition;
            isSwinging = true;
            sword.SetActive(true);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Weapon") || collision.collider.CompareTag("Bullet"))
        {
            //destroy bullet
            if (collision.collider.CompareTag("Bullet")){
                Destroy(collision.gameObject);
            }
            health -= 1; //
            if (health <= 0)
            {
                print("YOU DIED");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                print(health);
                Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
                StartCoroutine(Knockback(knockbackDirection, 10f, 0.3f));
                StartCoroutine(FlashRed());
                
            }
        }
        if (collision.collider.CompareTag("SpeedUp")) // change this to a tag, build if statements for names
        {
            Destroy(collision.collider.gameObject);
            Debug.Log("Player got a speedup!");
            StartCoroutine(SpeedPowerUp(5f));
        }
        if (collision.collider.CompareTag("GrowUp"))
        {
            Destroy(collision.collider.gameObject);
            Debug.Log("Player got a grower!");
            StartCoroutine(GrowPowerUp(5f));
        }
        if (collision.collider.CompareTag("HealthUp"))
        {
            Destroy(collision.collider.gameObject);
            Debug.Log("Player got aid!");
            if(health < 5)
            {
                health++;
            }
            Debug.Log("Health already Full!");
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

    IEnumerator SpeedPowerUp(float duration)
    {
        swingSpeed += 5f;
        speed += 5f;
        float timer = 0f;
        while(timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        speed -= 5f;
        swingSpeed -= 5f;
    }

    //This does not work properly, fix this now
    IEnumerator GrowPowerUp(float duration)
    {
        sword.transform.localScale += new Vector3(0f, 1f, 0f);
        yield return new WaitForSeconds(duration);
        sword.transform.localScale -= new Vector3(0f, 1f, 0f);
    }
}
