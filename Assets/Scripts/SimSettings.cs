using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimSettings: MonoBehaviour
{
    // User Set Properties
    public int terrainSize;
    public int objectThickness;
    public float evolMultplier = 1;
    public int rabbits;
    public int foxes;
    public int wolves;

    // Other Properties
    public int objectOffset;
    public bool[,] usedBlocks;
    public float[,] blockHeights;
    public int stage = 1;
    bool stage1;
    bool stage2;
    bool stage3;
    bool stage4;

    public float rabbitPop = 1;
    public float foxPop;
    public float wolfPop;

    public Stat defaultRabbitStat = new Stat()
    {
        speed = 1,
        size = 1,
        rowdinessMultiplier = 10,
        maxAge = 2,
        range = 4
    };

    public Stat defaultFoxStat = new Stat()
    {
        speed = 0.75f,
        size = 2,
        rowdinessMultiplier = 20,
        maxAge = 5,
        range = 10
    };

    public Stat defaultWolfStat = new Stat()
    {
        speed = 0.6f,
        size = 3,
        rowdinessMultiplier = 20,
        maxAge = 8,
        range = 15
    };

    // Start is called before the first frame update
    void Start()
    {
        objectOffset = Random.Range(0, 100);
        Time.timeScale = 10;
        StartCoroutine("checkAverage");
    }

    // Update is called once per frame
    void Update()
    {
        if(stage == 1 && !stage1)
        {
            FindObjectOfType<TerrainGenerator>().GeneratePlane();
            stage1 = true;
        }
        if (stage == 2 && !stage2)
        {
            FindObjectOfType<ObjectGenerator>().GenerateObjects();
            stage2 = true;
        }
        if (stage == 3 && !stage3)
        {
            FindObjectOfType<PlantGenerator>().GeneratePlants();
            stage3 = true;
        }
        if(stage == 4 && !stage4)
        {
            FindObjectOfType<AnimalGenerator>().GenerateAnimals();
            stage4 = true;
        }
    }

    IEnumerator checkAverage()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);

            rabbitPop = FindObjectsOfType<Rabbit>().Length;
            foxPop = FindObjectsOfType<Fox>().Length;
            wolfPop = FindObjectsOfType<Wolf>().Length;
        }
    }
}
