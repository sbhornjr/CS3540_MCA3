using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject redEnemyPrefab;
    public GameObject goldEnemyPrefab;

    public int level = 1;
    public float spawnTime = 3f;
    public float xMin = 2;
    public float xMax = 30;
    public float yMin = 1.32f;
    public float yMax = 1.32f;
    public float zMin = -2;
    public float zMax = 25;

    GameObject[] prefabs;
    int numSpawnedRemaining;

    // Start is called before the first frame update
    void Start()
    {
        numSpawnedRemaining = LevelManager.numSkeletonsRemaining;
        prefabs = new GameObject[3] { enemyPrefab, redEnemyPrefab, goldEnemyPrefab };
        InvokeRepeating("SpawnEnemies", spawnTime, spawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.isGameOver) CancelInvoke();
    }

    void SpawnEnemies()
    {
        if (numSpawnedRemaining > 0)
        {
            numSpawnedRemaining--;
            Vector3 enemyPosition;

            enemyPosition.x = Random.Range(xMin, xMax);
            enemyPosition.y = Random.Range(yMin, yMax);
            enemyPosition.z = Random.Range(zMin, zMax);

            var num = Random.Range(1, level + 1);

            GameObject spawnedEnemy = Instantiate(prefabs[num - 1], enemyPosition, transform.rotation);

            if (num == 3)
            {
                spawnedEnemy.transform.position = new Vector3(Random.Range(-10, 50), Random.Range(yMin, 15), Random.Range(-10, 50));
            }

            spawnedEnemy.transform.parent = gameObject.transform;
        }
    }
}
