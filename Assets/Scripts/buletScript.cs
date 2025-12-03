using UnityEditor;
using UnityEngine;

public class buletScript : MonoBehaviour
{
    public Collider2D player;

    void Start()
    {
        player = FindAnyObjectByType<playerScript>().gameObject.GetComponent<PolygonCollider2D>();
    }
    void Update()
    {
        transform.position += transform.up * 7 * Time.deltaTime;
        Debug.Log(GetComponent<PolygonCollider2D>().IsTouching(player));
    }
    void OnBecameInvisible()
    {
        //Debug.Log(transform.position);
        Destroy(gameObject);
    }

    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.up*7);
    }*/

    void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.name);
        if (collision.CompareTag("Player"))
        {
            player.gameObject.GetComponent<playerScript>().HitByBullet();
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Weapon")) Destroy(gameObject);
    }
}
