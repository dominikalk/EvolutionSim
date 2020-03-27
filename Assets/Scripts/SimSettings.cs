using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimSettings: MonoBehaviour
{
    // User Set Properties
    public int terrainSize;
    public int objectThickness;
    public int objectOffset;
    public float evolMultplier = 1;

    // Other Properties
    public bool[,] usedBlocks;
    public float[,] blockHeights;
    public int stage = 1;
    bool stage1;
    bool stage2;
    bool stage3;

    // Start is called before the first frame update
    void Start()
    {
        objectOffset = Random.Range(0, 100);
    }

    // Update is called once per frame
    void Update()
    {
        if(stage == 1 && !stage1)
        {
            FindObjectOfType<TerrainGenerator>().CreatePlane();
            stage1 = true;
        }
        if (stage == 2 && !stage2)
        {
            FindObjectOfType<ObjectGenerator>().generateObjects();
            stage2 = true;
        }
        if (stage == 3 && !stage3)
        {
            FindObjectOfType<PlantGenerator>().generatePlants();
            stage3 = true;
        }
    }
}
