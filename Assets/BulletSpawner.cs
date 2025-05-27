using UnityEngine;
using System.Collections;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float spawnSpeed = 1f; // 총알 생성 주기 (초)
    private bool isSpawning = false;

    void Start()
    {
        StartCoroutine(SpawnBullets());
    }

    IEnumerator SpawnBullets()
    {
        while (true)
        {
            // 몬스터가 하나라도 있는지 확인
            if (GameObject.FindGameObjectWithTag("Monster") != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(spawnSpeed);
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
}
