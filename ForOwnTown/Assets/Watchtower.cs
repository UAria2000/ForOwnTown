using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watchtower : MonoBehaviour
{
    public int level = 1;
    public int maxLevel = 10;
    public float attackInterval = 1f;
    public int damage;
    private GameManager gameManager;
    private BuildingInfoUI buildingInfoUI;
    private float attackTimer;

    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        buildingInfoUI = GameObject.FindObjectOfType<BuildingInfoUI>();
        attackTimer = attackInterval;
        UpdateDamage();
    }

    void Update()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            Attack();
            attackTimer = attackInterval;
        }

        if (IsMouseOver())
        {
            string actions = "Upgrade: U";
            string resources = GetResourceInfo();

            buildingInfoUI.UpdateWatchtowerInfo(level, actions, resources);

            if (Input.GetKeyDown(KeyCode.U))
            {
                Upgrade();
            }
        }
    }

    bool IsMouseOver()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject == gameObject;
        }
        return false;
    }

    void UpdateDamage()
    {
        damage = 50 * level * gameManager.population;
    }

    void Attack()
    {
        Monster closestMonster = FindClosestMonster();
        if (closestMonster != null)
        {
            closestMonster.TakeDamage(damage);
        }
    }

    Monster FindClosestMonster()
    {
        Monster[] monsters = GameObject.FindObjectsOfType<Monster>();
        Monster closestMonster = null;
        float closestDistance = Mathf.Infinity;

        foreach (Monster monster in monsters)
        {
            float distance = Vector3.Distance(transform.position, monster.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestMonster = monster;
            }
        }

        return closestMonster;
    }

    void Upgrade()
    {
        if (level < maxLevel && gameManager.actionPoints > 0)
        {
            int woodCost = (int)Mathf.Pow(level, 3) * 200;
            int stoneCost = (int)Mathf.Pow(level, 3) * 200;

            if (gameManager.wood >= woodCost && gameManager.stone >= stoneCost)
            {
                gameManager.wood -= woodCost;
                gameManager.stone -= stoneCost;
                level++;
                UpdateDamage();
                gameManager.UseActionPoint();
            }
        }
    }

    private string GetResourceInfo()
    {
        int woodCost = (int)Mathf.Pow(level, 3) * 200;
        int stoneCost = (int)Mathf.Pow(level, 3) * 200;

        string resourceInfo = $"Upgrade Cost: Wood: {woodCost}, Stone: {stoneCost}";

        return resourceInfo;
    }
}
