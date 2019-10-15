using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class will not be attached to any object and does therefore note extend MonoBehaviour
//This class create no Instances and is therefore Static

public static class Noise
{
    public static float [,] GenerateNoiseMap (int width, int height, float noiseScale)
    {
        float[,] noiseMap = new float[width, height];

        //Handle divide by zero

        if (noiseScale == 0)
        {
            noiseScale = 0.00001f;
        }

        for (int x=0; x < width; x++)
        {
            for (int y=0; y < height; y++)
            {
                float sampleX = x / noiseScale;
                float sampleY = y / noiseScale;

                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                noiseMap[x, y] = perlinValue;
            }
        }

        return noiseMap; 

    }
}
