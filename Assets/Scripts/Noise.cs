using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class will not be attached to any object and does therefore note extend MonoBehaviour
//This class create no Instances and is therefore Static

public static class Noise
{
    public enum NormalizeMode {Local, Gloal};
    public static float [,] GenerateNoiseMap (int width, int height, int seed, float noiseScale, int octaves, float lacunarity, float persistence, Vector2 offset, NormalizeMode normalizeMode)
    {
        float[,] noiseMap = new float[width, height];

        float maxPossibleNoiseHeight = 0;

        float maxLocalNoiseHeight = float.MinValue;
        float minLocalNoiseHeight = float.MaxValue;

        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        float amplitude = 1;

        //so we wanna get lots of uniqe noisemaps. To do that we'll make each octave sample its points from random locations
        System.Random random = new System.Random(seed);
        Vector2 [] octaveOffsets = new Vector2 [octaves];

        for (int i = 0; i < octaves; i++)
        {
            float offsetX = random.Next(-100000, 100000) + offset.x;
            float offsetY = random.Next(-100000, 100000) - offset.y;

            octaveOffsets [i] = new Vector2 (offsetX, offsetY);

            // we want toto assign the right value to the maxPossibleNoiseHeight. To do this we set the perlinvalue equal to 1 every octave

            maxPossibleNoiseHeight += amplitude;

            //amplituse is also dependant on persistence
            amplitude *= persistence;


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
                amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                //Loop to run through all of our Octaves
                for (int i = 0; i < octaves; i++)
                {
                    //the higher the frequency the farther away the sample points will be. 

                    float sampleX = (x-halfWidth + octaveOffsets[i].x) / noiseScale * frequency;
                    float sampleY = (y-halfHeight + octaveOffsets[i].y) / noiseScale * frequency;

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
                if (noiseHeight < minLocalNoiseHeight)
                {
                    minLocalNoiseHeight = noiseHeight;
                }
                else if (noiseHeight > maxLocalNoiseHeight)
                {
                    maxLocalNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        //This only works for one Chunk

        // Normalizing
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (normalizeMode == NormalizeMode.Local)
                {
                    //This only works for one Chunk because maxLocalNoiseHeight and minLocalNoiseHeight will have slightly different values for for other chunks

                    /*this function returns a value between 1 and 0. The maxLocalNoiseHeight and minLocalNoiseHeight values define the start and end of the line. 
                     * noiseMap[x, y] is a location between a and b. 
                     */
                    noiseMap[x, y] = Mathf.InverseLerp(maxLocalNoiseHeight, minLocalNoiseHeight, noiseMap[x, y]);
                }
                else
                {
                    //we need to reverse the opersation which let us have negative perlin values
                    //then we devide by the maxPossibleNoiseHeight
                    //why divide by maxPossibleNoiseHeight?
                    //our noiseMap[x, y] is never going to get any close to the maxPossibleNoiseHeight. Therefore we are going to lower it a bit
                    float globalHeight = (noiseMap[x, y] + 1) / (2f * maxPossibleNoiseHeight / 1.5f);
                    noiseMap[x, y] = Mathf.Clamp(globalHeight, 0 , int.MaxValue);
                }
            }
        }

                return noiseMap; 

    }
}
