using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropSpawner : HeatData
{
    [SerializeField] private GameObject itemDrop;
    public int EnemiesKilled { get; private set; }
    void Start()
    {
        EventManager.OnEnemyKilled += EnemyKilled;
    }
    private void EnemyKilled(EnemyUnit enemy)
    {
        EnemiesKilled++;
        RollForItemDrop(enemy);
    }
    private void RollForItemDrop(EnemyUnit enemy)
    {
        if (EnemiesKilled == 4 || EnemiesKilled == 12) // guarantee drops on certain milestones 
        {
            CreateItemDrop(enemy.gameObject.transform.position);
        }
        else
        {
            float rand = Random.Range(0, 1f);
            float odds = 0.05f;
            if (odds > rand)
            {
                CreateItemDrop(enemy.gameObject.transform.position);
            }
        }

    }

    private void CreateItemDrop(Vector3 pos)
    {
        Instantiate(itemDrop, pos, Quaternion.identity);
    }
}
