using UnityEngine;

public class Orc : Monster
{
    private float damageInterval = 1.5f;
    private int damageAmount;

    public void SetStats(int dayCount)
    {
        this.health = 250 * (dayCount - 10);
        this.damageAmount = 25 * dayCount;
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
