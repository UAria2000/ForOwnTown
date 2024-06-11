using UnityEngine;

public class Wall : MonoBehaviour
{
    public enum WallType { Palisade, StoneWall }
    public WallType wallType = WallType.Palisade;
    public int level = 1;
    public int maxLevel = 10;
    public int health;
    public int maxHealth;

    private GameManager gameManager;
    private BuildingInfoUI buildingInfoUI;

    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        buildingInfoUI = GameObject.FindObjectOfType<BuildingInfoUI>();
        UpdateHealth();
        gameManager.SetCurrentWall(this);
    }

    void Update()
    {
        if (IsMouseOver())
        {
            string actions = "Upgrade: U\nRepair: R";
            string resources = GetResourceInfo();

            buildingInfoUI.UpdateWallInfo(wallType.ToString(), level, actions, resources);

            if (Input.GetKeyDown(KeyCode.U))
            {
                Upgrade();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Repair();
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

    void UpdateHealth()
    {
        if (wallType == WallType.Palisade)
        {
            maxHealth = level * 1000;
        }
        else if (wallType == WallType.StoneWall)
        {
            maxHealth = level * 10000;
        }
        health = maxHealth;
    }

    public void Repair()
    {
        if (gameManager.actionPoints > 0 && health < maxHealth)
        {
            int repairCost = (maxHealth - health) * 1;
            if (wallType == WallType.Palisade && gameManager.wood >= repairCost)
            {
                gameManager.wood -= repairCost;
                health = maxHealth;
                gameManager.UseActionPoint();
            }
            else if (wallType == WallType.StoneWall && gameManager.stone >= repairCost)
            {
                gameManager.stone -= repairCost;
                health = maxHealth;
                gameManager.UseActionPoint();
            }
        }
        gameManager.UpdateWallHealthUI();
    }

    public void Upgrade()
    {
        if (level < maxLevel && gameManager.actionPoints > 0)
        {
            int woodCost = 0;
            int stoneCost = 0;
            if (wallType == WallType.Palisade)
            {
                woodCost = (int)Mathf.Pow(level, 2) * 1000;
                stoneCost = (int)Mathf.Pow(level, 2) * 1000;
                if (gameManager.wood >= woodCost && gameManager.stone >= stoneCost)
                {
                    gameManager.wood -= woodCost;
                    gameManager.stone -= stoneCost;
                    level++;
                    UpdateHealth();
                    gameManager.UseActionPoint();
                }
            }
            else if (wallType == WallType.StoneWall)
            {
                woodCost = (int)Mathf.Pow(level, 3) * 200;
                stoneCost = (int)Mathf.Pow(level, 3) * 200;
                if (gameManager.wood >= woodCost && gameManager.stone >= stoneCost)
                {
                    gameManager.wood -= woodCost;
                    gameManager.stone -= stoneCost;
                    level++;
                    UpdateHealth();
                    gameManager.UseActionPoint();
                }
            }
        }
        gameManager.UpdateWallHealthUI();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            gameManager.TakeTownDamage(damage);
            Destroy(gameObject);
            gameManager.UpdateWallHealthUI();
            gameManager.currentWall = null;
        }
        else
        {
            gameManager.UpdateWallHealthUI();
        }
    }

    private string GetResourceInfo()
    {
        int woodCost = 0;
        int stoneCost = 0;
        int repairCost = (maxHealth - health) * 1;

        if (wallType == WallType.Palisade)
        {
            woodCost = (int)Mathf.Pow(level, 2) * 1000;
            stoneCost = (int)Mathf.Pow(level, 2) * 1000;
        }
        else if (wallType == WallType.StoneWall)
        {
            woodCost = (int)Mathf.Pow(level, 3) * 200;
            stoneCost = (int)Mathf.Pow(level, 3) * 200;
        }

        string resourceInfo = $"Upgrade Cost: Wood: {woodCost}, Stone: {stoneCost}\nRepair Cost: {repairCost}";

        return resourceInfo;
    }
}
