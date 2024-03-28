using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpawnableItem
{
    public ScriptableObject item;
    [Range(0, 1)]
    public float spawnChance;
}
public class ItemSpawner : MonoBehaviour
{
    [SerializeField] SpawnableItem[] spawnableItems;
    [SerializeField] int maxSpawnableItems, minDistFromPlayer, minDistFromItems;

    float maxChance;
    [SerializeField] Item itemPrefab;
    List<Item> spawnedItems = new List<Item>();
    MazeGenerator mazeGenerator;
    Transform player;
    
    void Awake()
    {
        mazeGenerator = FindObjectOfType<MazeGenerator>();
        player = FindObjectOfType<PlayerMovement>().transform;
        foreach(SpawnableItem spawnableItem in spawnableItems)
        {
            maxChance += spawnableItem.spawnChance;
        }
    }
    
    void Start()
    {
        SpawnItems();
    }   

    public void SpawnItems()
    {
        for(int i = 0; i < maxSpawnableItems; i++)
        {
            Vector3Int randomSpawnPoint = mazeGenerator.GetMazeTiles()[Random.Range(0, mazeGenerator.GetMazeTiles().Count)];
            
            bool validSpawnPoint = false;
            while(validSpawnPoint == false)
            {
                randomSpawnPoint = mazeGenerator.GetMazeTiles()[Random.Range(0, mazeGenerator.GetMazeTiles().Count)];
                if(Vector3.Distance(randomSpawnPoint, player.position) < minDistFromPlayer)
                {
                    continue;
                }
                foreach(Item spawnedItem in spawnedItems)
                {
                    if(Vector3.Distance(randomSpawnPoint, spawnedItem.transform.position) < minDistFromItems)
                    {
                        continue;
                    }
                }
                validSpawnPoint = true;
            }
            Item newItem = Instantiate(itemPrefab, randomSpawnPoint, Quaternion.identity);
            newItem.SetItem(GetRandomItem());
        }
    }

    public ScriptableObject GetRandomItem()
    {
        float randomVal = Random.Range(0, maxChance);
        float currentVal = 0;
        ScriptableObject returnVal = null;
        foreach(SpawnableItem spawnableItem in spawnableItems)
        {
            returnVal = spawnableItem.item;
            currentVal += spawnableItem.spawnChance;
            if(currentVal >= randomVal)
            {
                break;
            }
        }
        if(returnVal == null)
        {
            Debug.Log("Unable to allocate item");
        }
        return returnVal;
        
    }
}
