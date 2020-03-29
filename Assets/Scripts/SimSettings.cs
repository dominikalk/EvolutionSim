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

    public float population;
    public float maxAge;
    public float maxEnergy;
    public float maxHealth;
    public float rowdiness;
    public float speed;
    public float size;
    public float range;

    public Stat defaultRabbitStat = new Stat()
    {
        maxHealth = 100,
        maxEnergy = 100,
        speed = 1,
        size = 1,
        rowdinessMultiplier = 1,
        maxAge = 2,
        range = 4
    };

    // Start is called before the first frame update
    void Start()
    {
        objectOffset = Random.Range(0, 100);
        Time.timeScale = 1;
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
        }
    }

    IEnumerator checkAverage()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);

            maxAge = 0;
            rowdiness = 0;
            speed = 0;
            size = 0;
            range = 0;
            maxHealth = 0;
            maxEnergy = 0;
            Rabbit[] rabbits = FindObjectsOfType<Rabbit>();

            foreach (Rabbit rabbit in rabbits)
            {
                maxAge += rabbit.stat.maxAge;
                rowdiness += rabbit.stat.rowdinessMultiplier;
                speed += rabbit.stat.speed;
                size += rabbit.stat.size;
                range += rabbit.stat.range;
                maxHealth += rabbit.stat.maxHealth;
                maxEnergy += rabbit.stat.maxEnergy;
            }
            population = rabbits.Length;
            maxAge /= rabbits.Length;
            rowdiness /= rabbits.Length;
            speed /= rabbits.Length;
            size /= rabbits.Length;
            range /= rabbits.Length;
            maxEnergy /= rabbits.Length;
            maxHealth /= rabbits.Length;
        }
    }
}
