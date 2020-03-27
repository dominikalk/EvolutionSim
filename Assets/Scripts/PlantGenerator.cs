﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGenerator : MonoBehaviour
{
    SimSettings simSettings;
    public GameObject[] plants;

    // Start is called before the first frame update
    void Start()
    {
        simSettings = FindObjectOfType<SimSettings>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void generatePlants()
    {
        int terrainSize = simSettings.terrainSize;
        int objectThickness = simSettings.objectThickness * 2;
        float[,] blockHeights = simSettings.blockHeights;

        for (int i = 0; i < terrainSize; i++)
        {
            for (int v = 0; v < terrainSize; v++)
            {
                float xCoord = (v / 40f) + simSettings.objectOffset;
                float zCoord = (i / 40f) + simSettings.objectOffset;

                float tempPerlin = Mathf.PerlinNoise(xCoord, zCoord);
                if (tempPerlin <= 0)
                {
                    tempPerlin = 0.0001f;
                }
                int isSpawn = Random.Range(0, (int)Mathf.Round(objectThickness / tempPerlin));
                int rotation = Random.Range(0, 3);
                int whatObject = Random.Range(0, plants.Length);
                if (blockHeights[v, i] > 10 && isSpawn == 5 && !simSettings.usedBlocks[v, i])
                {
                    GameObject newObject = Instantiate(plants[whatObject], new Vector3(v + 0.5f, blockHeights[v, i], terrainSize - i - 0.5f), Quaternion.Euler(0, rotation * 90f, 0));
                    newObject.transform.parent = gameObject.transform;
                    simSettings.usedBlocks[v, i] = true;
                }
            }
        }
    }
}
