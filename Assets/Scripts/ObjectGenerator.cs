using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] objects;
    public Vector3[] vertices;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateObjects()
    {
        SimSettings simSettings = FindObjectOfType<SimSettings>();
        int terrainSize = simSettings.terrainSize;
        int objectThickness = simSettings.objectThickness;
        bool[,] usedBlocks = new bool[terrainSize, terrainSize];
        float[,] topHeights = new float[terrainSize, terrainSize];
        float[,] blockHeights = new float[terrainSize, terrainSize];

        // Set Array Heights
        for(int i = 0; i < terrainSize; i++)
        {
            for(int v = 0; v < terrainSize; v++)
            {
                float botHeight = Mathf.Infinity;
                float topHeight = Mathf.NegativeInfinity;
                int[] corners = new int[]
                {
                    (i * terrainSize) + v + i,
                    (i * terrainSize) + v + i + 1,
                    (i * terrainSize) + terrainSize + i + v + 1,
                    (i * terrainSize) + terrainSize + i + v + 2,
                };

                foreach(int position in corners)
                {
                    if(vertices[position].y > topHeight)
                    {
                        topHeight = vertices[position].y;
                    }
                    if (vertices[position].y < botHeight)
                    {
                        botHeight = vertices[position].y;
                    }
                }

                blockHeights[terrainSize - 1 - v, i] = (topHeight + botHeight) / 2;
                topHeights[terrainSize - 1 - v, i] = botHeight;
            }
        }

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
                int whatObject = Random.Range(0, objects.Length);
                if (topHeights[v, i] > 10 && isSpawn == 5)
                {
                    GameObject newObject = Instantiate(objects[whatObject], new Vector3(v + 0.5f, topHeights[v, i], terrainSize - i - 0.5f), Quaternion.Euler(0, rotation * 90f, 0));
                    newObject.transform.parent = gameObject.transform;
                    usedBlocks[v, i] = true;
                }
            }
        }

        simSettings.blockHeights = blockHeights;
        simSettings.usedBlocks = usedBlocks;
        simSettings.stage += 1;
    }
}

/*
 - i started off knowing that i wanted the objects such as trees to be in the middle of the square, so i had to work out the lowest verted that made up the squares 
    so that the object wouldnt appear to be half floating
 - i messed up the value for the x and z coordinate so the trees were in the wrong position
 - then randomised the objects so there were more than just one
 - incorporated perlin noise with random so that there would be congregations of trees in some ares and not in others to look more natural
 - Tested if i put the used blocks in the right position in the array by instantiating the flower object everywhere where there wasnt one
 - i initially stored the usedBlocks in a 1 dimentional array, but then changed it to a 2 dimentional array for easy use
*/

