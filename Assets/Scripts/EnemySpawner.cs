using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    MazeGenerator mazeGenerator;
    Transform player;

    [SerializeField] float minDistFromPlayer = 15f;
    [SerializeField] int maxActiveEnemies = 5;
    [SerializeField] OverworldEnemyMovement enemyPrefab;
    void Awake()
    {
        mazeGenerator = FindObjectOfType<MazeGenerator>();
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    public void RemoveAllEnmies()
    {
        OverworldEnemyMovement[] enemies = FindObjectsOfType<OverworldEnemyMovement>();
        foreach(OverworldEnemyMovement enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
    }

    public void SpawnEnemies()
    {
        int activeEnemies = FindObjectsOfType<OverworldEnemyMovement>().Length;
        for(int i = activeEnemies; i < maxActiveEnemies; i++)
        {
            Vector3Int randomSpawnPoint = mazeGenerator.GetMazeTiles()[Random.Range(0, mazeGenerator.GetMazeTiles().Count)];
            while(Vector3.Distance(randomSpawnPoint, player.position) < minDistFromPlayer)
            {
                randomSpawnPoint = mazeGenerator.GetMazeTiles()[Random.Range(0, mazeGenerator.GetMazeTiles().Count)];
            }
            OverworldEnemyMovement enemy = Instantiate(enemyPrefab, randomSpawnPoint, Quaternion.identity);
        }
    }
}
