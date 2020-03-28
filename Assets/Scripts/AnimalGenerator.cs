using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalGenerator : MonoBehaviour
{
    public GameObject rabbit;
    public GameObject fox;
    public GameObject wolf;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateAnimals()
    {
        SimSettings simSettings = FindObjectOfType<SimSettings>();
        Stat[] parentStats = new Stat[] 
        {
            new Stat(),
            new Stat()
        };

        parentStats[0].maxHealth = 100;
        parentStats[0].maxEnergy = 100;
        parentStats[0].speed = 1;
        parentStats[0].size = 10;
        parentStats[0].rowdinessMultiplier = 1;
        parentStats[0].maxAge = 2;
        parentStats[0].range = 4;

        parentStats[1].maxHealth = 100;
        parentStats[1].maxEnergy = 100;
        parentStats[1].speed = 1;
        parentStats[1].size = 10;
        parentStats[1].rowdinessMultiplier = 1;
        parentStats[1].maxAge = 2;
        parentStats[1].range = 4;

        int rabbitsLeft = simSettings.rabbits;
        int xPos;
        int yPos;

        while (true)
        {
            xPos = Random.Range(0, simSettings.terrainSize);
            yPos = Random.Range(0, simSettings.terrainSize);
            if(!simSettings.usedBlocks[xPos, yPos] && simSettings.blockHeights[xPos, yPos] >= 10)
            {
                GameObject newAnimal = Instantiate(rabbit, new Vector3(xPos + 0.5f, simSettings.blockHeights[xPos, yPos], simSettings.terrainSize - yPos - 0.5f), Quaternion.identity);
                newAnimal.GetComponent<Rabbit>().xPos = xPos;
                newAnimal.GetComponent<Rabbit>().yPos = yPos;
                newAnimal.GetComponent<Rabbit>().parentStats = parentStats;
                simSettings.usedBlocks[xPos, yPos] = true;
                newAnimal.transform.parent = gameObject.transform;
                rabbitsLeft -= 1;
            }
            if (rabbitsLeft == 0)
            {
                break;
            }
        }

        simSettings.stage += 1;
    }
}
