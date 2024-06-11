using UnityEngine;

public class Monster : MonoBehaviour
{
    public delegate void MonsterDeathHandler(GameObject monster);
    public event MonsterDeathHandler OnDeath;

    public int health;
    public int damage;
    public float speed;
    public float attackCooldown = 1.0f;
    public float lifeSpan = 60f;

    protected float attackTimer;
    protected Transform target;
    protected Wall wall;
    protected float lifeTimer;
    protected GameManager gameManager;
    protected bool isAttackingWall = false;

    protected virtual void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        target = GameObject.Find("TownCenter")?.transform;
        wall = GameObject.FindObjectOfType<Wall>();
        lifeTimer = lifeSpan;
        attackTimer = attackCooldown;
    }

    protected virtual void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            Die();
            return;
        }

        attackTimer -= Time.deltaTime;

        if (wall == null)
        {
            MoveTowardsTarget();
        }

        if (attackTimer <= 0)
        {
            if (isAttackingWall)
            {
                AttackWall();
            }
            else if (wall == null && target != null)
            {
                AttackTown();
            }
            attackTimer = attackCooldown;
        }
    }

    protected virtual void MoveTowardsTarget()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            float distanceThisFrame = speed * Time.deltaTime;

            transform.Translate(direction * distanceThisFrame, Space.World);
        }
        else
        {
            Debug.LogWarning("Target is null");
        }
    }

    protected virtual void AttackWall()
    {
        if (wall != null)
        {
            wall.TakeDamage(damage);
        }
    }

    protected void AttackTown()
    {
        if (gameManager != null)
        {
            gameManager.TakeTownDamage(damage);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        OnDeath?.Invoke(gameObject);
        Destroy(gameObject);
    }

    protected void InvokeOnDeath()
    {
        OnDeath?.Invoke(gameObject);
    }

    public void SetTargetToTown()
    {
        wall = null;
        target = GameObject.Find("TownCenter").transform;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isAttackingWall = true;
            wall = collision.gameObject.GetComponent<Wall>();
        }
        else if (collision.gameObject.CompareTag("TownCenter"))
        {
            isAttackingWall = false;
        }
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isAttackingWall = false;
            wall = null;
        }
    }
}
