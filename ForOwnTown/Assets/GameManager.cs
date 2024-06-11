using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int wood = 0;
    public int stone = 0;
    public int food = 0;
    public int population = 1;
    public int actionPoints = 4;
    public int dayCount = 1;

    public float nightDuration = 60.0f;
    private bool isDay = true;
    private float nightTimer;
    private int townHealth;

    public Text resourceText;
    public Text dayNightText;
    public Text gameOverText;
    public Text townHealthText;
    public Text wallHealthText;
    public Text wallTypeText;

    private MonsterManager monsterManager;
    public Wall currentWall;

    public Light directionalLight;
    private float originalLightIntensity;

    void Start()
    {
        monsterManager = GameObject.FindObjectOfType<MonsterManager>();
        actionPoints = population + 4;
        townHealth = GetMaxTownHealth();
        UpdateResourceUI();
        UpdateDayNightUI();
        UpdateTownHealth();
        gameOverText.enabled = false;

        if (directionalLight != null)
        {
            originalLightIntensity = directionalLight.intensity;
        }
    }

    void Update()
    {
        if (!isDay)
        {
            nightTimer -= Time.deltaTime;
            if (nightTimer <= 0)
            {
                StartDay();
            }
            UpdateDayNightUI();
        }
    }

    public void UseActionPoint()
    {
        if (actionPoints > 0)
        {
            actionPoints--;
            UpdateResourceUI();
            if (actionPoints == 0)
            {
                StartNight();
            }
        }
    }

    void StartDay()
    {
        isDay = true;
        actionPoints = population + 4;
        dayCount++;
        UpdateResourceUI();
        UpdateDayNightUI();
        RecoverTownHealth();

        if (directionalLight != null)
        {
            directionalLight.intensity = originalLightIntensity;
        }
    }

    void StartNight()
    {
        isDay = false;
        nightTimer = nightDuration;
        monsterManager.StartNight(dayCount);
        UpdateDayNightUI();

        if (directionalLight != null)
        {
            directionalLight.intensity = originalLightIntensity * 0.2f;
        }
    }

    void UpdateResourceUI()
    {
        resourceText.text = $"Wood: {wood}\nStone: {stone}\nFood: {food}\nPopulation: {population}\nAction Points: {actionPoints}\nDay: {dayCount}";
        UpdateTownHealth();
    }

    void UpdateDayNightUI()
    {
        if (isDay)
        {
            dayNightText.text = "Day";
        }
        else
        {
            dayNightText.text = $"Night\nTime Left: {Mathf.Ceil(nightTimer)}s";
        }
    }

    void UpdateTownHealth()
    {
        townHealthText.text = $"Town Health: {townHealth}/{GetMaxTownHealth()}";
    }

    int GetMaxTownHealth()
    {
        return population * 100;
    }

    public void AddResource(string type, int amount)
    {
        switch (type)
        {
            case "wood":
                wood += amount;
                break;
            case "stone":
                stone += amount;
                break;
            case "food":
                food += amount;
                break;
        }
        UpdateResourceUI();
    }

    public void IncreasePopulation()
    {
        if (food >= 10)
        {
            food -= 10;
            population++;
            townHealth = GetMaxTownHealth();
            UpdateResourceUI();
            UpdateTownHealth();
        }
    }

    public void TakeTownDamage(int damage)
    {
        townHealth -= damage;
        while (townHealth <= (population - 1) * 100 && population > 0)
        {
            population--;
            actionPoints = population;
        }

        UpdateResourceUI();
        UpdateTownHealth();

        if (population <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameOverText.enabled = true;
        gameOverText.text = "Game Over";

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터 모드에서 실행 중지
#else
        Application.Quit();
#endif
    }

    public void UpdateWallHealthUI()
    {
        if (currentWall != null)
        {
            wallTypeText.text = $"Wall Type: {currentWall.wallType}";
            wallHealthText.text = $"Wall Health: {currentWall.health}/{currentWall.maxHealth}";
        }
        else
        {
            wallTypeText.text = "No Wall";
            wallHealthText.text = "";
        }
    }

    public void SetCurrentWall(Wall wall)
    {
        currentWall = wall;
        UpdateWallHealthUI();
    }

    public void OnAllMonstersDefeated()
    {
        if (!isDay)
        {
            StartDay();
        }
    }

    public void RecoverTownHealth()
    {
        townHealth = GetMaxTownHealth();
        UpdateTownHealth();
    }
}
