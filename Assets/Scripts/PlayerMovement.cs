using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    bool moving = false;
    void Update()
    {
        if (Input.GetKey(KeyCode.W) && !moving)
        {
            Vector3Int targetTile = new Vector3Int((int)transform.position.x, 0, (int)transform.position.z)
                + new Vector3Int(Mathf.RoundToInt(transform.forward.x), 0, Mathf.RoundToInt(transform.forward.z));       
            if(FindObjectOfType<MazeGenerator>().CheckValidTile(targetTile))
            {
                StartCoroutine(MovePlayer(targetTile));
            }
        }
        if (Input.GetKey(KeyCode.A) && !moving)
        {
            StartCoroutine(RotatePlayer(-90));
        }
        if (Input.GetKey(KeyCode.D) && !moving)
        {
            StartCoroutine(RotatePlayer(90));
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
