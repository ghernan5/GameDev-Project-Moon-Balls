using System.Collections;
using UnityEngine;

public class shootr : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject target;
    public GameObject spawnLocation;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    public AudioClip shootSound;
    public Sprite IdleSprite;
    public Sprite ShootSprite;

    void Start()
    {
        target = FindFirstObjectByType<playerScript>().gameObject;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(SpawnBullets());
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnEnable()
    {
        StartCoroutine(SpawnBullets());
    }

    IEnumerator SpawnBullets()
    {
        while (true)
        {
            Quaternion rot = Quaternion.LookRotation(Vector3.forward, target.transform.position - spawnLocation.transform.position);
            Instantiate(bulletPrefab, spawnLocation.transform.position, rot);
            audioSource.PlayOneShot(shootSound);
            StartCoroutine(ChangeSprite());
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator ChangeSprite()
    {
        spriteRenderer.sprite = ShootSprite;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.sprite = IdleSprite;
    }
}
