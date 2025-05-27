using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;
    private float currentHealth;

    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private float invincibilityDuration = 1f;  // 무적 지속시간
    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        StartCoroutine(DebugHealthStatus());
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float horizontalInput = 0f;
        float verticalInput = 0f;

        // WASD 키 입력 감지
        if (Input.GetKey(KeyCode.A)) horizontalInput = -1f;
        if (Input.GetKey(KeyCode.D)) horizontalInput = 1f;
        if (Input.GetKey(KeyCode.W)) verticalInput = 1f;
        if (Input.GetKey(KeyCode.S)) verticalInput = -1f;

        // 이동 방향 계산 및 적용
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f);
        transform.position += movement * moveSpeed * Time.deltaTime;
    }

    IEnumerator DebugHealthStatus()
    {
        while (true)
        {
            Debug.Log($"현재 체력: {currentHealth} / {maxHealth}");
            yield return new WaitForSeconds(1f);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible) return;  // 무적상태면 데미지를 받지 않음

        currentHealth = Mathf.Max(0f, currentHealth - damage);
        
        if (currentHealth <= 0f)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityEffect());
        }
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    private IEnumerator InvincibilityEffect()
    {
        isInvincible = true;
        float elapsedTime = 0f;
        
        // 깜빡임 효과
        while (elapsedTime < invincibilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            elapsedTime += 0.1f;
        }

        spriteRenderer.enabled = true;  // 깜빡임 종료 후 스프라이트 보이게
        isInvincible = false;
    }

    private void Die()
    {
        // 플레이어 사망 처리
        gameObject.SetActive(false);
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.EndGame();
    }
}
