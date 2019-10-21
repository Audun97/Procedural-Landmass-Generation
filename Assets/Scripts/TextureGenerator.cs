using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class will have a method which creates a texture from a one-dimensional colormap
public static class TextureGenerator
{
    public static Texture2D TextureFromColourmap(Color[] colourMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    }

    public static Texture2D TextureFromNoisemap(float[,] noiseMap)
    {
        // (0) and (1) refers to the dimensions
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        // It is all more efficient when I first put all the values in an Array and then set the Pixels
        // Note that the Color Array is only one dimensional

        Color[] colourMap = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }

        return TextureFromColourmap(colourMap, width, height);
    }
}
