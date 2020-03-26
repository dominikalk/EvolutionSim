using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public Gradient gradient;
    public GameObject water;
    int terrainSize;

    // Start is called before the first frame update
    void Start()
    {
        terrainSize = FindObjectOfType<SimSettings>().terrainSize;
        CreatePlane();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreatePlane()
    {
        //Init Variables
        ObjectGenerator objectGenerator = FindObjectOfType<ObjectGenerator>();
        int arraySize = (terrainSize + 1) * (terrainSize + 1);
        Mesh newMesh = new Mesh();
        Vector3[] vertices = new Vector3[arraySize];
        Vector2[] uvs = new Vector2[arraySize];
        int[] triangles = new int[terrainSize * terrainSize * 2 * 3];
        float topVertex = Mathf.NegativeInfinity;
        float botVertex = Mathf.Infinity;

        // Set Vertex Values
        int vertexPos = 0;
        for (int i = terrainSize / 2; i >= ((terrainSize / 2) *(-1)); i--)
        {
            for (int v = terrainSize/2; v >= ((terrainSize/2)*(-1)); v--)
            {
                vertices[vertexPos] = new Vector3(v, 0, i);
                vertexPos += 1;
;            }
        }

        //Set UV values
        for(int i = 0, z = 0; z < terrainSize + 1; z++)
        {
            for(int x = 0; x < terrainSize + 1; x++)
            {
                uvs[i] = new Vector2((float)x / (terrainSize + 1), (float)z / (terrainSize + 1));
                i++;
            }
        }

        //Set Triangle Values
        int triPos = 0;
        for(int i = 0; i < arraySize - (terrainSize + 1); i++)
        {
            if(CheckEnd(i) || i == 0)
            {
                triangles[triPos] = i;
                triPos += 1;
                triangles[triPos] = i + (terrainSize + 1);
                triPos += 1;
                triangles[triPos] = i + (terrainSize + 2);
                triPos += 1;

                triangles[triPos] = i;
                triPos += 1;
                triangles[triPos] = i + (terrainSize + 2);
                triPos += 1;
                triangles[triPos] = i + 1;
                triPos += 1;
            }
            
        }

        // Add Smaller Noise
        int offset = Random.Range(0, 100);
        for (int i = 0; i < vertices.Length; i++)
        {
            float xVertex = vertices[i].x + (terrainSize / 2) + offset;
            float zVertex = vertices[i].z + (terrainSize / 2) + offset;
            vertices[i] = new Vector3(vertices[i].x, Mathf.PerlinNoise(xVertex / 3f, zVertex / 3f), vertices[i].z);
        }

        // Add Larger Noise
        offset = Random.Range(0, 100);
        for (int i = 0; i < vertices.Length; i++)
        {
            float xVertex = vertices[i].x + (terrainSize / 2) + offset;
            float zVertex = vertices[i].z + (terrainSize / 2) + offset;
            vertices[i] = new Vector3(vertices[i].x, vertices[i].y + Mathf.PerlinNoise(xVertex / 15f, zVertex / 15f) * 7f, vertices[i].z);
        }

        // Add Even Larger Noise
        offset = Random.Range(0, 100);
        for (int i = 0; i < vertices.Length; i++)
        {
            float xVertex = vertices[i].x + (terrainSize / 2) + offset;
            float zVertex = vertices[i].z + (terrainSize / 2) + offset;
            vertices[i] = new Vector3(vertices[i].x, vertices[i].y + Mathf.PerlinNoise(xVertex / 60f, zVertex / 60f) * 15f, vertices[i].z);
        }

        GetTopBotVertex(vertices, ref topVertex, ref botVertex);

        // Lower The Edge Vertices
        vertices = LowerEdges(vertices);

        // Colors
        Color[] colors = new Color[arraySize];
        for (int i = 0, z = 0; z < terrainSize + 1; z++)
        {
            for (int x = 0; x < terrainSize + 1; x++)
            {
                float height = vertices[i].y;
                if(height < botVertex)
                {
                    height = botVertex;
                }
                height = Mathf.InverseLerp((float)botVertex, (float)topVertex, height);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }

        // Set ObjectGenerator Vertices Before Flat Shading
        objectGenerator.vertices = vertices;

        // Flat Shader Code
        Vector3[] flatShadedVertices = new Vector3[triangles.Length];
        Vector2[] flatShadedUvs = new Vector2[triangles.Length];
        Color[] flatShadedColors = new Color[triangles.Length];
        for (int i = 0; i < triangles.Length; i++)
        {
            flatShadedVertices[i] = vertices[triangles[i]];
            flatShadedUvs[i] = uvs[triangles[i]];
            flatShadedColors[i] = colors[triangles[i]];
            triangles[i] = i;
        }
        vertices = flatShadedVertices;
        uvs = flatShadedUvs;
        colors = flatShadedColors;

        // Set Mesh to newMesh
        newMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        newMesh.vertices = vertices;
        newMesh.triangles = triangles;
        newMesh.colors = colors;
        newMesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = newMesh;
        GetComponent<MeshCollider>().sharedMesh = newMesh;

        gameObject.transform.position = new Vector3(terrainSize / 2, 0f, terrainSize / 2);
        water.transform.position = new Vector3(terrainSize / 2, 9f, terrainSize / 2);
        objectGenerator.generateObjects();
    }

    bool CheckEnd(int pos)
    {
        // convert to float to return float
        float fPos = pos;
        float fTerrainSize = terrainSize;

        if(((fPos + 1f) / (fTerrainSize + 1f)) % 1f == 0)
        {
            return false;
        }
        return true;
    }

    void GetTopBotVertex(Vector3[] vertices, ref float topVertex, ref float botVertex)
    {
        for(int i = 0; i < vertices.Length; i++)
        {
            if(vertices[i].y > topVertex)
            {
                topVertex = vertices[i].y;
            }
            if (vertices[i].y < botVertex)
            {
                botVertex = vertices[i].y;
            }
        }
    }

    bool CheckDiagonalRight(int pos, int i)
    {
        float TS1 = terrainSize + 1;
        float firstDiagonal = terrainSize * i;
        float secondDiagonal = -(terrainSize + 2) * i + (TS1 * TS1 + TS1);

        if (pos <= firstDiagonal || pos >= secondDiagonal)
        {
            return false;
        }
        return true;
    }

    bool CheckDiagonalLeft(int pos, int i)
    {
        float TS1 = terrainSize + 1;
        float firstDiagonal = (terrainSize + 2) * i - (terrainSize + 2);
        float secondDiagonal = -(terrainSize * i) + (TS1 * TS1) - 1;

        if (pos <= firstDiagonal || pos >= secondDiagonal)
        {
            return false;
        }
        return true;
    }

    Vector3[] LowerEdges(Vector3[] vertices)
    {
        float edgeSize = terrainSize / 4f;
        float farIn = 0f;

        // Lower the top and bottom sides
        for(int i = 0; i < edgeSize; i++)
        {
            for(int v = 0 + (int)farIn; v < terrainSize + 1 - farIn; v++)
            {
                if(i == 0)
                {
                    vertices[v + ((terrainSize + 1) * i)].y = 0f;
                    vertices[(terrainSize + 1) * (terrainSize + 1) - (i * (terrainSize + 1)) - v - 1].y = 0f;
                }
                else
                {
                    vertices[v + ((terrainSize + 1) * i)].y -= (edgeSize + 1f) / (farIn + 1f) - 1f;
                    if(vertices[v + ((terrainSize + 1) * i)].y < 0)
                    {
                        vertices[v + ((terrainSize + 1) * i)].y = 0;
                    }
                    vertices[(terrainSize + 1) * (terrainSize + 1) - (i * (terrainSize + 1)) - v - 1].y -= (edgeSize + 1f) / (farIn + 1f) - 1f;
                    if (vertices[(terrainSize + 1) * (terrainSize + 1) - (i * (terrainSize + 1)) - v - 1].y < 0)
                    {
                        vertices[(terrainSize + 1) * (terrainSize + 1) - (i * (terrainSize + 1)) - v - 1].y = 0;
                    }
                }
            }
            farIn += 1f;
        }

        // Lower the right side
        farIn = 0f;
        float fTerrainSize = terrainSize;
        for (int i = 0; i < edgeSize; i++)
        {
            for(int v = 0; v < (terrainSize + 1)*(terrainSize + 1); v++)
            {
                if ((v + farIn + 1f)/(fTerrainSize + 1f) % 1f == 0)
                {
                    if (CheckDiagonalRight(v, i + 1))
                    {
                        vertices[v].y -= (edgeSize + 1f) / (farIn + 1f) - 1f;
                        if(vertices[v].y < 0)
                        {
                            vertices[v].y = 0;
                        }
                    }
                }
            }
            farIn += 1f;
        }

        // Lower the Left size
        farIn = 0f;
        for (int i = 0; i < edgeSize; i++)
        {
            for (int v = 0; v < (terrainSize + 1) * (terrainSize + 1); v++)
            {
                if ((v - farIn + fTerrainSize + 1)/(fTerrainSize + 1) % 1f == 0)
                {
                    if (CheckDiagonalLeft(v, i + 1))
                    {
                        vertices[v].y -= (edgeSize + 1f) / (farIn + 1f) - 1f;
                        if (vertices[v].y < 0)
                        {
                            vertices[v].y = 0;
                        }
                    }
                }
            }
            farIn += 1f;
        }
        return vertices;
    }
}

/*
Problems:
 - creating a new mesh: i had to create a new instance of a mesh rather than editing the existing one otherwise i wouldnt be able
 to manipulate the size of the mesh
- After had to recreate all of the values (vertices, triangles, uvs and normals)
- for the triangles i had to figure out if it was at the right edge, in which case i should ignor it
     - the linear equation for the position of the edge points was (terrainSize + 1)n - 1
     - I back tracked this equation to figure out if it was an edge piece
- when dividing integers unity will return an integer (usually 0) so i have to convert to float before
- Had to add terrain/2 to the z and x vertex to give to perlin noise because otherwise it would give a symetrical landscape
- had to add offset to make the terrain different each load
- had to lower the edges, i used a reciprocal graph to do so (y = (edgeSize + 1) / (x + 1) - 1)
- over 254 wide, there arent enough vertices available and so there were wierd errors 
    - Meshes cannot store more that 65535 vertices, so i had to allow it to store more
- Corners were lowered twice because i didnt take way the diagonals
- Wanted flat shading (no look smooth; look low poly) so i had to not reuse vertices for the triangle
- Took ages trying to sort out the color arrar: the size was wierd because of the flatShader code, so i moved it before that and then adder colors to the flast shader code
*/
