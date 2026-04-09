using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2.5f;
    public float chaseRange = 12f;
    public float attackRange = 1.8f;
    public int damage = 1;
    public float attackCooldown = 1.0f;
    public int health = 3;

    private Transform player;
    private float lastAttackTime;

    void Start()
    {
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= chaseRange)
        {
            ChasePlayer(distance);
        }
    }

    void ChasePlayer(float distance)
    {
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(targetPosition);

        if (distance > attackRange)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(player.position.x, transform.position.y, player.position.z),
                moveSpeed * Time.deltaTime
            );
        }
        else
        {
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            PlayerCharacter playerHealth = player.GetComponent<PlayerCharacter>();
            if (playerHealth != null)
            {
                playerHealth.Hurt(damage);
            }

            lastAttackTime = Time.time;
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log(gameObject.name + " Health: " + health);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}