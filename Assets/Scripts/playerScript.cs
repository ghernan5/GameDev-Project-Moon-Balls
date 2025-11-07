using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class playerScript : MonoBehaviour
{
    public float speed;
    public GameObject sword;
    [SerializeField] private float swingSpeed = 10f; //angular speed
    [SerializeField] private float swingAngle = 90f; //angle to swing
    private bool isSwinging = false;
    private bool facingRight = true;
    private Quaternion startingSwordRotation;
    private Vector3 startingSwordPosition;
    private Vector2 movement;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sword.SetActive(false);
    }

    void FixedUpdate()
    {
    }

    void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
        rb.linearVelocity = movement * speed;
            if (movement.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (movement.x < 0 && facingRight)
        {
            Flip();
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
            StartCoroutine(SwingAttack()); //this can be changed
        }
    }

    private IEnumerator SwingAttack()
    {
        isSwinging = true;
        float rotated = 0f;
        float swingDirection = facingRight ? 1f : -1f;

        while (rotated < swingAngle)
        {
            sword.transform.RotateAround(transform.localPosition, Vector3.forward, -swingSpeed * swingDirection);
            rotated += swingSpeed;
            yield return null;
        }

        sword.SetActive(false);
        sword.transform.localRotation = startingSwordRotation;
        sword.transform.localPosition = startingSwordPosition;
        isSwinging = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {//TODO: fix
        if (collision.collider.name.Contains("enemy")&&!isSwinging) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    
}
