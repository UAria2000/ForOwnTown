using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public GameObject goblinPrefab;
    public GameObject orcPrefab;
    public Transform goblinSpawnPoint;
    public Transform orcSpawnPoint;
    private GameManager gameManager;
    private GameObject activeMonster = null;

    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    public void StartNight(int dayCount)
    {
        if (activeMonster != null)
        {
            Destroy(activeMonster);
        }

        if (dayCount <= 10)
        {
            SpawnGoblin(dayCount);
        }
        else
        {
            SpawnOrc(dayCount);
        }
    }

    void SpawnGoblin(int dayCount)
    {
        if (goblinSpawnPoint == null)
        {
            Debug.LogError("Goblin spawn point is not set");
            return;
        }

        if (activeMonster != null) return;

        activeMonster = Instantiate(goblinPrefab, goblinSpawnPoint.position, goblinSpawnPoint.rotation);
        Goblin goblinScript = activeMonster.GetComponent<Goblin>();
        goblinScript.SetStats(dayCount);
        goblinScript.OnDeath += OnMonsterDeath;
    }

    void SpawnOrc(int dayCount)
    {
        if (orcSpawnPoint == null)
        {
            Debug.LogError("Orc spawn point is not set");
            return;
        }

        if (activeMonster != null) return;

        activeMonster = Instantiate(orcPrefab, orcSpawnPoint.position, orcSpawnPoint.rotation);
        Orc orcScript = activeMonster.GetComponent<Orc>();
        orcScript.SetStats(dayCount);
        orcScript.OnDeath += OnMonsterDeath;
    }

    void OnMonsterDeath(GameObject monster)
    {
        if (activeMonster == monster)
        {
            activeMonster = null;
            gameManager.OnAllMonstersDefeated();
        }
    }

    public bool AreAllMonstersDead()
    {
        return activeMonster == null;
    }
}
