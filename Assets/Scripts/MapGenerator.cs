using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class should define the public values
//this class should fetch  the noisemapfrom class Noise

public class MapGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public float noiseScale;

    public bool autoUpdate;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(width, height, noiseScale);

        MapDisplay display = FindObjectOfType<MapDisplay>();
        display.DrawNoiseMap(noiseMap);
    }
}
