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

    // Start is called before the first frame update
    void Start()
    {
        objectOffset = Random.Range(0, 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
