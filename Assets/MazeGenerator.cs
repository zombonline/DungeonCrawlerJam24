using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MazeGenerator : MonoBehaviour
{
    List<Vector3Int> discoveredTiles = new List<Vector3Int>();
    Vector3Int[] cardinalDirections = new Vector3Int[4];
    [SerializeField] GameObject floorTilePrefab, wallTilePrefab;
    float turnChance;
    [SerializeField] float turnChanceStart = .1f;
    [SerializeField] float turnChanceIncrement = 0.1f;
    [SerializeField] int iterations, exploreDistance;
    private void Awake()
    {
        cardinalDirections[0] = Vector3Int.forward;
        cardinalDirections[1] = Vector3Int.back;
        cardinalDirections[2] = Vector3Int.left;
        cardinalDirections[3] = Vector3Int.right;

        GenerateMaze(iterations, exploreDistance);
        turnChance = turnChanceStart;
    }
    private void Update()
    {
        
    }

    private bool isDiagonal(Vector3Int val)
    {
        return (val.x!= 0 && val.z != 0);
    }


    public void GenerateMaze(int iterations, int exploreDistance)
    {
        discoveredTiles.Add(new Vector3Int(0, 0, 0));
        Instantiate(floorTilePrefab, new Vector3Int(0, 0, 0), Quaternion.identity);
        for(int i = 0; i < iterations; i++)
        {
            Vector3Int randomOrigin = discoveredTiles[Random.Range(0, discoveredTiles.Count)];

            Vector3Int currentOrigin = randomOrigin;

            Vector3Int randomDir = cardinalDirections[Random.Range(0, cardinalDirections.Length)];

            for (int j = 0; j < exploreDistance; j ++)
            {
                //check neigbours in cardinal directions
                bool validTileFound = false;
                while(!validTileFound)
                {                    
                    if(Random.Range(0f,1f) < turnChance)
                    {
                        randomDir = cardinalDirections[Random.Range(0, cardinalDirections.Length)];
                        turnChance = turnChanceStart;
                    }
                    else 
                    {
                        turnChance += turnChanceIncrement;
                    }
                    Vector3Int newOriginThisStep = currentOrigin + randomDir;

                    validTileFound = true;
                    List<Vector3Int> cardinalNeighbors = new List<Vector3Int>();
                    for (int k = 0; k < cardinalDirections.Length; k++)
                    {
                        if (discoveredTiles.Contains(newOriginThisStep + cardinalDirections[k]))
                        {
                            cardinalNeighbors.Add(cardinalDirections[k]);
                        }
                    }
                    for (int k = 0; k < cardinalNeighbors.Count; k++)
                    {
                        for (int l = k + 1; l < cardinalNeighbors.Count; l++)
                        {
                            Vector3Int intercardinalDirToCheck = cardinalNeighbors[k] + cardinalNeighbors[l];
                            if (isDiagonal(intercardinalDirToCheck) && discoveredTiles.Contains(newOriginThisStep+intercardinalDirToCheck)) 
                            {
                                validTileFound = false;
                                break;
                            }
                        }
                    }
                    if(validTileFound)
                    {
                        currentOrigin = newOriginThisStep;
                        validTileFound = !discoveredTiles.Contains(currentOrigin);
                        if (validTileFound)
                        {
                            discoveredTiles.Add(currentOrigin);
                            Instantiate(floorTilePrefab, currentOrigin, Quaternion.identity);
                        }
                    }
                }    
            }
        }
        for(int i = 0; i < discoveredTiles.Count; i++)
        {
            Vector3Int currentTile = discoveredTiles[i];
            for(int j = 0; j < cardinalDirections.Length; j++)
            {
                Vector3Int neighbor = currentTile + cardinalDirections[j];
                if (!discoveredTiles.Contains(neighbor))
                {
                    Instantiate(wallTilePrefab, neighbor + Vector3.up, Quaternion.identity);
                }
            }
        }
    }
    public bool CheckValidTile(Vector3Int pos)
    {
        
        foreach(Vector3Int tile in discoveredTiles)
        {
            if (tile.x == pos.x && tile.z == pos.z && tile.y == pos.y)
            {
                return true;
            }
        }
        
        return false;
    }
}
