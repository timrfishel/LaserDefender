﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationController : MonoBehaviour
{

    public float width = 11f;
    public float height = 4.5f;
    private bool movingRight = true;
    private float xmax;
    private float xmin;
    public float speed = 2f;
    public float spawnDelay = 0.5f;


    public GameObject enemyPrefab;

    // Use this for initialization
    void Start()
    {

        float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera));
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera));
        xmax = rightEdge.x;
        xmin = leftEdge.x;

        SpawnEnemies();
        
    }

    void SpawnEnemies() {
        foreach (Transform child in transform)
        {
            GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = child;
        }
    }
    void SpawnUntilFull() {
        Transform freeposition = NextFreePosition();
        if (freeposition) {
            GameObject enemy = Instantiate(enemyPrefab, freeposition.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = freeposition;
        }
        if (NextFreePosition())
        {
            Invoke("SpawnUntilFull", spawnDelay);
        }

    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
    }


    // Update is called once per frame
    void Update()
    {
        if (movingRight)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0);
        }
        else
        {
            transform.position += new Vector3(-speed * Time.deltaTime, 0);
        }

        float rightEdgeOfFormation = transform.position.x + (0.5f * width);
        float leftEdgeOfFormation = transform.position.x - (0.5f * width);


        if (leftEdgeOfFormation < xmin)
        {
            movingRight = true;
        }
        else if (rightEdgeOfFormation > xmax)
        {
            movingRight = false;
        }

        if (AllMembersDead()) {
            Debug.Log("Emptry Formation");
            SpawnUntilFull();
        }

    }
    Transform NextFreePosition() {
        foreach (Transform childPositionGameObject in transform) {
            if (childPositionGameObject.childCount == 0) {
                return childPositionGameObject;
             }
        }
        return null;
    }
    bool AllMembersDead() {
        foreach (Transform childPositionGameObject in transform) {
            if (childPositionGameObject.childCount > 0) {
                return false;
            }
        }
        return true;
    }
}
