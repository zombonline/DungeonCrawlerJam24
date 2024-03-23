using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torso : MonoBehaviour
{
    [SerializeField] GameObject[] legPrefabs, armPrefabs, headPrefabs;
    [SerializeField] Transform[] headPositions, armPositions, legPositions;
    private void Awake()
    {
        AssignBodyParts();
    }
    public void AssignBodyParts()
    {
        for(int i = 0; i < headPositions.Length; i++)
        {
            var newHead = Instantiate(headPrefabs[Random.Range(0, headPrefabs.Length)], headPositions[i]);
        }
        for (int i = 0; i < armPositions.Length; i++)
        {
            var newArm = Instantiate(armPrefabs[Random.Range(0, armPrefabs.Length)], armPositions[i]);
        }
        for (int i = 0; i < legPositions.Length; i++)
        {
            var newLeg = Instantiate(legPrefabs[Random.Range(0, legPrefabs.Length)], legPositions[i]);
        }
    }
}
