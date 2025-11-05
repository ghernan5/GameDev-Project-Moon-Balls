using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerScript : MonoBehaviour
{
    public float speed;
    public GameObject sword;
    [SerializeField] private float swingSpeed = 10f;
    private bool isSwinging = false;
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
        // player rotation (maybe take out fixedupdate)
        if (movement.sqrMagnitude > 0.001f)
        {
            float targetAngle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.25f);
        }
    }

    void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
        rb.linearVelocity = movement * speed;
    }

    void OnSwing()
    {
        if (!isSwinging)
        {
            startingSwordRotation = sword.transform.localRotation;
            startingSwordPosition = sword.transform.localPosition;
            isSwinging = true;
            sword.SetActive(true);
            StartCoroutine(SwingAttack(90f)); //swinging sword by 90 degrees, can change it 
        }
    }

    private IEnumerator SwingAttack(float targetAngle)
    {
        isSwinging = true;
        float rotated = 0f;

        while (rotated < targetAngle)
        {
            sword.transform.RotateAround(transform.localPosition, Vector3.forward, swingSpeed);
            rotated += 5;
            yield return null;
        }

        sword.SetActive(false);
        sword.transform.localRotation = startingSwordRotation;
        sword.transform.localPosition = startingSwordPosition;
        isSwinging = false;
    }
    
}
