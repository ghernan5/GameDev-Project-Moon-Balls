using UnityEditor;
using UnityEngine;

public class buletScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position+=transform.up * 5 * Time.deltaTime;
    }
    void OnBecameInvisible()
    {
        Debug.Log(transform.position);
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.up*5);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Weapon")) Destroy(gameObject);
    }
}
