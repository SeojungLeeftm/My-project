using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 20f;
    private Rigidbody2D bulletRigidbody;

    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
        Transform nearestMonster = FindNearestMonster();
        
        if (nearestMonster != null)
        {
            Vector2 direction = (nearestMonster.position - transform.position).normalized;
            bulletRigidbody.linearVelocity = direction * speed;
        }
        
        Destroy(gameObject, 3f);
    }

    Transform FindNearestMonster()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        float nearestDistance = Mathf.Infinity;
        Transform target = null;
        
        foreach (GameObject monster in monsters)
        {
            float distance = Vector2.Distance(transform.position, monster.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                target = monster.transform;
            }
        }
        return target;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            MonsterController monster = collision.gameObject.GetComponent<MonsterController>();
            if (monster != null)
            {
                monster.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
