using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    bool moving = false;
    void Update()
    {
        if (Input.GetKey(KeyCode.A) && !moving)
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                AttemptToMove(-transform.right);
            }
            else
            {
                StartCoroutine(RotatePlayer(-90));
            }
        }
        if (Input.GetKey(KeyCode.D) && !moving)
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                AttemptToMove(transform.right);
            }
            else
            {
                StartCoroutine(RotatePlayer(90));
            }
        }
        if (Input.GetKey(KeyCode.W) && !moving)
        {
            AttemptToMove(transform.forward);
        }
        if (Input.GetKey(KeyCode.S) && !moving)
        {
            AttemptToMove(-transform.forward);
        }
    }
    void AttemptToMove(Vector3 dir)
    {
        Vector3Int targetTile = new Vector3Int((int)transform.position.x, 0, (int)transform.position.z)
                + new Vector3Int(Mathf.RoundToInt(dir.x), 0, Mathf.RoundToInt(dir.z));
        if (FindObjectOfType<MazeGenerator>().CheckValidTile(targetTile))
        {
            StartCoroutine(MovePlayer(targetTile));
        }
    }
    IEnumerator MovePlayer(Vector3Int targetTile)
    {
        moving = true;
        while(transform.position != targetTile)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetTile, .05f);
            yield return new WaitForSeconds(.01f);
        }
        transform.position = targetTile;
        FindObjectOfType<Minimap>().UpdateMinimap(transform.position);
        moving = false;
    }
    IEnumerator RotatePlayer(int rotationAmount)
    {
        if(Input.GetKey(KeyCode.W))
        {
            Vector3Int targetTile = new Vector3Int((int)transform.position.x, 0, (int)transform.position.z)
            + Vector3Int.RoundToInt(transform.right) * (int)Mathf.Sign(rotationAmount);
            Debug.Log(FindObjectOfType<MazeGenerator>().CheckValidTile(targetTile));
            Debug.Log(targetTile);  
            if (!FindObjectOfType<MazeGenerator>().CheckValidTile(targetTile))
            {
                yield break;
            }
        }
        moving = true;
        for(int i = 0; i < MathF.Abs(rotationAmount); i++)
        {
            transform.Rotate(0, MathF.Sign(rotationAmount), 0);
            yield return new WaitForSeconds(.0025f);
        }
        FindObjectOfType<Minimap>().UpdatePlayerIconDirection(transform.eulerAngles);
        moving = false;
    }
}
