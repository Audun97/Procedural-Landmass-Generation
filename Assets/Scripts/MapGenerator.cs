using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class should define the public values
//this class should fetch  the noisemapfrom class Noise

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode {NoiseMap, ColourMap, Mesh};
    public DrawMode drawMode;

    public const int mapChunkSize = 241;
    [Range (0,6)]
    public int levelOfDetail;

    public float noiseScale;

    public bool autoUpdate;

    public int octaves;
    [Range(0, 1)]
    public float persistence;
    public float lacunarity;

    public int seed;

    public float heightMultiplier;
    public AnimationCurve meshHeightCurve;
    public Vector2 offset;

    public TerrainType[] regions;

    public void GenerateMap()
    {
        float[,] heightMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, lacunarity, persistence, offset);

        // we wanna assign colors to specific altitude values
        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];

        for (int x = 0; x < mapChunkSize; x++)
        {
            for (int y = 0; y < mapChunkSize; y++)
            {
                float currentAltitude = heightMap[x, y];

                // set each altitude per pixel to coulour defined in struct
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentAltitude <= regions[i].altitude)
                    {
                        colourMap[y * mapChunkSize + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();

        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromNoisemap(heightMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourmap(colourMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(heightMap, heightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourmap(colourMap, mapChunkSize, mapChunkSize));
        }

    }


    void OnValidate()
    {
        if (octaves < 0)
        {
            octaves = 0;
        }
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
    }

    //So that it will show up in the inspector
    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float altitude;
        public Color colour;


    }
}
