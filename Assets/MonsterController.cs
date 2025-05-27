using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f;
    private Transform playerTransform;

    [SerializeField]
    private float maxHealth = 50f;
    private float currentHealth;

    [SerializeField]
    private float damage = 10f;    // 몬스터가 플레이어에게 주는 데미지

    void Start()
    {
        currentHealth = maxHealth;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // 플레이어 방향으로 이동
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 대상이 플레이어인지 확인
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
                Die(); // 데미지를 준 후 즉시 사망
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(0f, currentHealth - damage);
        
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        // 몬스터 사망 처리
        Destroy(gameObject);
    }
}
