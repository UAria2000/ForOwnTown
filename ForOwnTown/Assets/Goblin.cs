using UnityEngine;

public class Goblin : Monster
{
    private int damageInterval = 1;
    private int damageAmount;

    public void SetStats(int dayCount)
    {
        this.health = 50 * dayCount;
        this.damageAmount = 5 * dayCount;
        this.damage = 0;
    }

    protected override void Start()
    {
        base.Start();
        InvokeRepeating(nameof(DealDamage), damageInterval, damageInterval);
    }

    private void DealDamage()
    {
        if (wall != null)
        {
            wall.TakeDamage(damageAmount);
        }
        else
        {
            gameManager.TakeTownDamage(damageAmount);
        }
    }

    protected override void Die()
    {
        CancelInvoke(nameof(DealDamage));
        InvokeOnDeath();
        base.Die();
    }
}
