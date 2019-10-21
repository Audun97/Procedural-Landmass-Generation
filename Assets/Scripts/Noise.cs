using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class will not be attached to any object and does therefore note extend MonoBehaviour
//This class create no Instances and is therefore Static

public static class Noise
{
    public static float [,] GenerateNoiseMap (int width, int height, int seed, float noiseScale, int octaves, float lacunarity, float persistence, Vector2 offset)
    {
        float[,] noiseMap = new float[width, height];

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        //so we wanna get lots of uniqe noisemaps. To do that we'll make each octave sample its points from random locations
        System.Random random = new System.Random(seed);
        Vector2 [] octaveOffsets = new Vector2 [octaves];

        for (int i = 0; i < octaves; i++)
        {
            float offsetX = random.Next(-100000, 100000) + offset.x;
            float offsetY = random.Next(-100000, 100000) + offset.y;

            octaveOffsets [i] = new Vector2 (offsetX, offsetY);
        }

        //Handle divide by zero

        if (noiseScale == 0)
        {
            noiseScale = 0.00001f;
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                //Loop to run through all of our Octaves
                for (int i = 0; i < octaves; i++)
                {
                    //the higher the frequency the farther away the sample points will be. 

                    float sampleX = (x-halfWidth) / noiseScale * frequency + octaveOffsets[i].x;
                    float sampleY = (y-halfHeight) / noiseScale * frequency + octaveOffsets[i].y;

                    //Letting our noise have negative values makes the noise more interesting
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;

                    /*instead of setting our noiseMap directly to our perlin value we want to increase the noiseHeight by the perlinValue of each Octave
                     * 
                     * noiseMap[x, y] = perlinValue;
                     */
                    noiseHeight += perlinValue * amplitude;

                    //we want our amplitude to decrease in each succedent octave. Because the persistence has a value between 0 and 1 this will do this
                    amplitude *= persistence;
                    // we want our frequency to increace for each succedent octave. The lacunarity is bigger than 2
                    frequency *= lacunarity;

                }

                // so now we need to normalaize the values again to make them fit between 0 and 1
                //look to the setting of the max- and minheightvalues
                if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                else if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        // Normalizing
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(maxNoiseHeight, minNoiseHeight, noiseMap[x, y]);
            }
        }

                return noiseMap; 

    }
}
