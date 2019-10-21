using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class should define the public values
//this class should fetch  the noisemapfrom class Noise

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColourMap };
    public DrawMode drawMode;

    public int width;
    public int height;
    public float noiseScale;

    public bool autoUpdate;

    public int octaves;
    [Range(0, 1)]
    public float persistence;
    public float lacunarity;

    public int seed;
    
    public Vector2 offset;

    public TerrainType[] regions;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(width, height, seed, noiseScale, octaves, lacunarity, persistence, offset);

        // we wanna assign colors to specific altitude values
        Color[] colourMap = new Color[width * height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float currentAltitude = noiseMap[x, y];

                // set each altitude per pixel to coulour defined in struct
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentAltitude <= regions[i].altitude)
                    {
                        colourMap[y * width + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();

        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromNoisemap(noiseMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourmap(colourMap, width, height));
        }

    }


    void OnValidate()
    {
        if (width < 1)
        {
            width = 1;
        }
        if (height < 1)
        {
            height = 1;
        }
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
