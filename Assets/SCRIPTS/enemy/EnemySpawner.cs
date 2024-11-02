using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

public class EnemySpawner : HeatData
{
    public bool IsSpawning;
    private List<EnemyData> masterEnemyList = new List<EnemyData>();
    private List<EnemyUnit> spawnedEnemyList = new List<EnemyUnit>();
    [SerializeField] SortSprites sortSprites;
    //private int heatNumber = 1;
    private GameState gameState;
    void Start()
    {
        EventManager.OnEnemyKilled += EnemyKilled;
        GenerateMasterList(); 
        gameState = GameState.Instance;

        for (int i = 0; i < GameData.Instance.Tier; i++) {
            Spawn();
        }
    }
    void Update()
    {
        //Debug.Log(spawnedEnemyList.Count);
    }

    private void EnemyKilled(EnemyUnit enemy)
    {
        //Debug.Log(spawnedEnemyList.Contains(enemy));
        if (spawnedEnemyList.Contains(enemy))
        {
            spawnedEnemyList.Remove(enemy);
        }

        if (IsSpawning)
        {
            if (spawnedEnemyList.Count < GameState.Instance.MaxEnemiesOnScreen)
            {
                Spawn();
            }
        }

    }

    private void Spawn()
    {
        EnemyData data = GetRandomEnemy();
        GameObject newEnemy = Instantiate(data.enemyPrefab, RandomSpawnLocation(), Quaternion.identity);

        newEnemy.transform.parent = transform;
        newEnemy.GetComponent<EnemyConstructor>().Data = data;
        newEnemy.SetActive(true);
        
        spawnedEnemyList.Add(newEnemy.GetComponent<EnemyConstructor>().Initialize());
        sortSprites.RebuildSpriteList();
    }    
    private void Spawn(EnemyData data, Vector3 pos)
    {
        GameObject newEnemy = Instantiate(data.enemyPrefab, pos, Quaternion.identity);

        newEnemy.transform.parent = transform;
        newEnemy.GetComponent<EnemyConstructor>().Data = data;
        newEnemy.SetActive(true);


        newEnemy.GetComponent<EnemyConstructor>().Initialize();
        //spawnedEnemyList.Add(newEnemy.GetComponent<EnemyStats>());
        sortSprites.RebuildSpriteList();
    }
    private Vector3 RandomSpawnLocation()
    {
        float distanceFromPlayer = 11f;

        Vector2 randomDirection2D = Random.insideUnitCircle.normalized;
        Vector3 randomDirection = new Vector3(randomDirection2D.x, randomDirection2D.y, 0);

        Vector3 randomPosition = GameState.Instance.PlayerTransform.position + randomDirection * distanceFromPlayer;
        return randomPosition;
    }

    private EnemyData GetRandomEnemy()
    {
        //List<(string, float)> list;

        //switch (GameState.Instance.HeatNumber)
        //{
        //    case 1:
        //        list = heat1Enemies; break;
        //    case 2:
        //        list = heat2Enemies; break;
        //    case 3:
        //        list = heat3Enemies; break;
        //    default:
        //        list = null;
        //        break;
        //}

        //if (list != null)
        //{
        //    return GetEnemyFromList(GetWeightedRandomEnemyFromList(list));
        //}
        //else
        //{
        //    return null;
        //}
        return GetEnemyFromList(GetWeightedRandomEnemyFromList(heat1Enemies));
    }
    private EnemyData GetEnemyFromList(string enemyName)
    {
        enemyName = enemyName.ToLower();
        foreach (EnemyData enemy in masterEnemyList)
        {
            if (enemy.unitCommandName.ToLower() == enemyName)
            {
                return enemy;
            }
        }
        return null; // else return null
    }

    public string GetWeightedRandomEnemyFromList(List<(string, float)> heatEnemyList)
    {
        float total = 0;
        foreach (var enemy in heatEnemyList)
        {
            total += enemy.Item2;
        }

        // Use Unity's Random.Range to generate a random number between 0 and total
        float randomNumber = Random.Range(0f, total);
        float cumulative = 0f;

        foreach (var enemy in heatEnemyList)
        {
            cumulative += enemy.Item2;
            if (randomNumber <= cumulative)
            {
                return enemy.Item1;
            }
        }

        return null; // Fallback, should never happen if percentages are correct
    }

    public bool SpawnFromConsole(string commandEnemyName)
    {
        Vector3 vector = new Vector3(-7f, 0 ,0);
        EnemyData data = GetEnemyFromList(commandEnemyName);
        if (data != null) {
            Spawn(data, GameState.Instance.PlayerTransform.position + vector);
            return true;
        }
        //foreach (EnemyData enemy in masterEnemyList)
        //{
        //    if (enemy.unitCommandName == commandEnemyName) {
        //        Vector3 vector = new Vector3(-7f, 0 ,0);
        //        Spawn(enemy.enemyPrefab, GameState.Instance.PlayerTransform.position + vector);
        //        return true;
        //    }
        //}
        return true;
    }

    public GameObject DetermineClosestTargetToCursor(float radius=99f)
    {
        GameObject closest = null;
        float closestDistance = float.MaxValue;
        Vector3 attackedPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Transform child = gameObject.transform.GetChild(i);
            if (!child.gameObject.activeSelf) continue;
            float distance = Vector3.Distance(child.position, attackedPoint);

            if (distance < closestDistance)
            {
                closest = child.gameObject;
                closestDistance = distance;
            }
        }
        if (closestDistance < radius)
        {
            return closest;
        }
        else
        {
            return null;
        }


    }
    private void GenerateMasterList()
    {
        masterEnemyList.Clear();
        EnemyData[] enemies = Resources.LoadAll<EnemyData>("Enemies");
        foreach (EnemyData enemy in enemies)
        {
            if (enemy != null)
            {
                masterEnemyList.Add(enemy);
            }
        }
        //StartCoroutine(SpawningLoop());
    }
}
