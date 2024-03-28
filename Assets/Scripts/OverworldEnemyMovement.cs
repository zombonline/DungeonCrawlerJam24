using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldEnemyMovement : MonoBehaviour
{
    float wanderRadius = 10;
    bool moving = false;
    [SerializeField] float wanderTimeMin, wanderTimeMax;
    float wanderTimer;

    Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }
    void Update()
    {
        if(CheckIfPlayerInComatRange())
        {
            FindObjectOfType<CombatManager>().BeginEncounter(CombatState.PLAYERTURN);
            Destroy(gameObject);
        }
        
        if (moving)
        {
            return;
        }
        if(CheckForPlayer())
        {
            MoveForward();
        }
        wanderTimer -= Time.deltaTime;
        if (wanderTimer <= 0)
        {
            float randomNumber = UnityEngine.Random.Range(0, 100);
            if (randomNumber > 25)
            {
                MoveForward();
            }
            else if (randomNumber > 12.5f)
            {
                StartCoroutine(Rotate(90));
            }
            else
            {
                StartCoroutine(Rotate(-90));
            }
            wanderTimer = UnityEngine.Random.Range(wanderTimeMin, wanderTimeMax);
        }
    }

    private void MoveForward()
    {
        Vector3Int targetTile = new Vector3Int((int)transform.position.x, 0, (int)transform.position.z)
                + new Vector3Int(Mathf.RoundToInt(transform.forward.x), 0, Mathf.RoundToInt(transform.forward.z));
        if (FindObjectOfType<MazeGenerator>().CheckValidTile(targetTile))
        {
            StartCoroutine(Move(targetTile));
        }
        else
        {
            StartCoroutine(Rotate(90));
        }
    }

    private bool CheckForPlayer()
    {
        bool result = false;
        RaycastHit visionRay;
        if (Physics.Raycast(transform.position, transform.forward, out visionRay, 10))
        {
            Debug.Log(visionRay.collider.gameObject.name);
            Debug.DrawRay(transform.position, transform.forward * 10, Color.red);
            if (visionRay.collider.CompareTag("Player"))
            {
                result = true;
            }
        }
        return result;
    }

    private bool CheckIfPlayerInComatRange()
    {
        RaycastHit combatRay;
        if (Physics.Raycast(transform.position, transform.forward, out combatRay, 1))
        {
            return(combatRay.collider.CompareTag("Player"));
        }
        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(initialPosition, wanderRadius);
    }
    IEnumerator Move(Vector3Int targetTile)
    {
        moving = true;
        while(transform.position != targetTile)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetTile, .05f);
            yield return new WaitForSeconds(.01f);
        }
        transform.position = targetTile;
        moving = false;
    }
    IEnumerator Rotate(int rotationAmount)
    {
        moving = true;
        for(int i = 0; i < MathF.Abs(rotationAmount); i++)
        {
            transform.Rotate(0, MathF.Sign(rotationAmount), 0);
            yield return new WaitForSeconds(.0025f);
        }
        moving = false;
    }
}
