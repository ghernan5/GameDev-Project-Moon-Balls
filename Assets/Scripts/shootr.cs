using System.Collections;
using UnityEngine;

public class shootr : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject target;
    public GameObject spawnLocation;

    void Start()
    {
        StartCoroutine(SpawnBullets());
    }

    IEnumerator SpawnBullets()
    {
        while (true)
        {
            Quaternion rot = Quaternion.LookRotation(Vector3.forward, target.transform.position - spawnLocation.transform.position);
            Instantiate(bulletPrefab, spawnLocation.transform.position, rot);
            yield return new WaitForSeconds(1f);
        }
    }
}
