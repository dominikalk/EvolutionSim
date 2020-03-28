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
            simSettings.defaultRabbitStat,
            simSettings.defaultRabbitStat
        };

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
                Rabbit rabbitScript = newAnimal.GetComponent<Rabbit>();
                rabbitScript.xPos = xPos;
                rabbitScript.yPos = yPos;
                rabbitScript.parentStats = parentStats;
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
