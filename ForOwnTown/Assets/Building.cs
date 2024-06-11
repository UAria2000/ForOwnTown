using UnityEngine;

public class Building : MonoBehaviour
{
    public enum BuildingType { LumberMill, Quarry, Farm, TownHall }
    public BuildingType buildingType;
    public int productionRate = 50;
    public int maxLevel = 10;
    private int level = 1;
    private GameManager gameManager;
    private BuildingInfoUI buildingInfoUI;

    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        buildingInfoUI = GameObject.FindObjectOfType<BuildingInfoUI>();
    }

    void Update()
    {
        if (IsMouseOver())
        {
            string actions = "Upgrade: U\nProduce: P";
            string resources = GetResourceInfo();

            buildingInfoUI.UpdateBuildingInfo(buildingType.ToString(), level, actions, resources);

            if (Input.GetKeyDown(KeyCode.P))
            {
                Produce();
            }

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

    public void Produce()
    {
        if (gameManager.actionPoints > 0)
        {
            int resourceAmount = 0;
            bool canProduce = true;

            switch (buildingType)
            {
                case BuildingType.LumberMill:
                    resourceAmount = productionRate * level * gameManager.population;
                    gameManager.AddResource("wood", resourceAmount);
                    break;
                case BuildingType.Quarry:
                    resourceAmount = productionRate * level * gameManager.population;
                    gameManager.AddResource("stone", resourceAmount);
                    break;
                case BuildingType.Farm:
                    resourceAmount = (productionRate / 5) * level * gameManager.population;
                    gameManager.AddResource("food", resourceAmount);
                    break;
                case BuildingType.TownHall:
                    int foodCost = 100 * gameManager.population * gameManager.population;
                    if (gameManager.food >= foodCost)
                    {
                        gameManager.food -= foodCost;
                        int populationIncrease = 0;
                        if (level <= 2)
                        {
                            populationIncrease = 1;
                        }
                        else if (level <= 4)
                        {
                            populationIncrease = 2;
                        }
                        else if (level <= 6)
                        {
                            populationIncrease = 3;
                        }
                        else if (level <= 8)
                        {
                            populationIncrease = 4;
                        }
                        else if (level <= 10)
                        {
                            populationIncrease = 5;
                        }
                        gameManager.population += populationIncrease;
                        gameManager.RecoverTownHealth();
                    }
                    else
                    {
                        Debug.Log("Not enough food to increase population");
                        canProduce = false;
                    }
                    break;
            }

            if (canProduce)
            {
                gameManager.UseActionPoint();
            }
        }
    }


    public void Upgrade()
    {
        if (level < maxLevel && gameManager.actionPoints > 0)
        {
            int woodCost = 0;
            int stoneCost = 0;
            bool canUpgrade = true;

            switch (buildingType)
            {
                case BuildingType.LumberMill:
                    woodCost = (int)Mathf.Pow(level, 3) * 100;
                    stoneCost = (int)Mathf.Pow(level, 3) * 50;
                    break;
                case BuildingType.Quarry:
                    woodCost = (int)Mathf.Pow(level, 3) * 50;
                    stoneCost = (int)Mathf.Pow(level, 3) * 100;
                    break;
                case BuildingType.Farm:
                    woodCost = (int)Mathf.Pow(level, 3) * 50;
                    stoneCost = (int)Mathf.Pow(level, 3) * 50;
                    break;
                case BuildingType.TownHall:
                    woodCost = (int)Mathf.Pow(level, 3) * 100;
                    stoneCost = (int)Mathf.Pow(level, 3) * 50;
                    break;
            }

            if (gameManager.wood >= woodCost && gameManager.stone >= stoneCost)
            {
                gameManager.wood -= woodCost;
                gameManager.stone -= stoneCost;
                level++;
            }
            else
            {
                Debug.Log("Not enough resources to upgrade building");
                canUpgrade = false;
            }

            if (canUpgrade)
            {
                gameManager.UseActionPoint();
            }
        }
    }

    private string GetResourceInfo()
    {
        int woodCost = 0;
        int stoneCost = 0;
        int productionAmount = 0;
        int foodCost = 0;

        switch (buildingType)
        {
            case BuildingType.LumberMill:
                woodCost = (int)Mathf.Pow(level, 3) * 100;
                stoneCost = (int)Mathf.Pow(level, 3) * 50;
                productionAmount = productionRate * level * gameManager.population;
                break;
            case BuildingType.Quarry:
                woodCost = (int)Mathf.Pow(level, 3) * 50;
                stoneCost = (int)Mathf.Pow(level, 3) * 100;
                productionAmount = productionRate * level * gameManager.population;
                break;
            case BuildingType.Farm:
                woodCost = (int)Mathf.Pow(level, 3) * 50;
                stoneCost = (int)Mathf.Pow(level, 3) * 50;
                productionAmount = (productionRate / 5) * level * gameManager.population;
                break;
            case BuildingType.TownHall:
                woodCost = (int)Mathf.Pow(level, 3) * 100;
                stoneCost = (int)Mathf.Pow(level, 3) * 50;
                foodCost = 100 * gameManager.population * gameManager.population;
                break;
        }

        string resourceInfo = $"Upgrade Cost: Wood: {woodCost}, Stone: {stoneCost}";
        if (buildingType != BuildingType.TownHall)
        {
            resourceInfo += $"\nProduction Amount: {productionAmount}";
        }
        else
        {
            resourceInfo += $"\nProduction Cost: Food: {foodCost}";
        }

        return resourceInfo;
    }
}
