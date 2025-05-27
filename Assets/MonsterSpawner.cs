using UnityEngine;
using System.Collections;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public float spawnInterval = 2f;        // 몬스터 스폰 주기
    public float minDistanceFromPlayer = 5f; // 플레이어로부터의 최소 거리
    
    private Transform playerTransform;
    private PlayerController playerController;
    private BoxCollider2D groundCollider;    // Ground의 경계 확인용
    private bool isSpawning = false;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerController = player.GetComponent<PlayerController>();
        }
        groundCollider = GetComponent<BoxCollider2D>();
        StartCoroutine(SpawnMonsters());
    }

    IEnumerator SpawnMonsters()
    {
        while (true)
        {
            if (playerTransform != null && playerController != null && playerController.IsAlive())
            {
                SpawnMonsterInValidPosition();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnMonsterInValidPosition()
    {
        // Ground의 경계 가져오기
        Vector2 groundMin = groundCollider.bounds.min;
        Vector2 groundMax = groundCollider.bounds.max;

        int maxAttempts = 30; // 최대 시도 횟수
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            // 랜덤 위치 생성
            Vector2 randomPosition = new Vector2(
                Random.Range(groundMin.x, groundMax.x),
                Random.Range(groundMin.y, groundMax.y)
            );

            // 플레이어와의 거리 확인
            float distanceToPlayer = Vector2.Distance(randomPosition, playerTransform.position);

            // 조건 체크: 플레이어와의 최소 거리를 만족하고 Ground 내부인지
            if (distanceToPlayer >= minDistanceFromPlayer && 
                IsPositionInsideGround(randomPosition))
            {
                Instantiate(monsterPrefab, randomPosition, Quaternion.identity);
                break;
            }

            attempts++;
        }
    }

    bool IsPositionInsideGround(Vector2 position)
    {
        return groundCollider.bounds.Contains(position);
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
}
