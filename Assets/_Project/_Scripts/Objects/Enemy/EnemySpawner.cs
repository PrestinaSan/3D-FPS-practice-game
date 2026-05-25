using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int spawnCount;
    [SerializeField] private float spawnTime;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private ScoreManager scoreManager;
    private float time;
    private int scorePerLevel = 30, bigEnemyCount = 0, enemyIncreaseAmount = 0;
    void Start()
    {
        
    }

    void Update()
    {
        time += Time.deltaTime; 

        if (time >= spawnTime)
        {
            spawnCount = Random.Range(1, 3);
            if (scoreManager.GetScore() >= (enemyIncreaseAmount + 1) * scorePerLevel)
            {
                enemyIncreaseAmount++;
                bigEnemyCount++;
            }
            spawnCount += enemyIncreaseAmount;
            for (int i = 0; i < spawnCount; i++)
            {
                SpawnEnemy(0);
            }
            for (int i = 0; i < bigEnemyCount; i++)
            {
                SpawnEnemy(1);
            }
            time = 0;
            spawnTime = Random.Range(2f, 7f);
        }
    }
    private void SpawnEnemy(int spawnIndex)
    {
        Instantiate(enemyPrefabs[spawnIndex], new Vector3(Random.Range(-25, 25), 1.2f, Random.Range(-25, 25)), Quaternion.identity); 
    }

}

//time += Time.deltaTime; // put this in update

//if (time >= spawnTime)
//{
//    SpawnEnemy();
//    time = 0;
//}

//private void SpawnEnemy() // legacy spawn enemy script
//{
//    Instantiate(enemyPrefab, new Vector3(Random.Range(-25, 25), 1.2f, Random.Range(-25, 25)),Quaternion.identity);
//}