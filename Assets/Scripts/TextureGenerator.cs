using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureGenerator
{

    // (0) and (1) refers to the dimensions
    int mapWidth = noiseMap.GetLength(0);
    int mapHeight = noiseMap.GetLength(1);

    Texture2D texture = new Texture2D(mapWidth, mapHeight);

    // It is all more efficient when I first put all the values in an Array and then set the Pixels
    // Note that the Color Array is only one dimensional

    Color[] mapColour = new Color[mapWidth * mapHeight];

        for (int y = 0; y<mapHeight; y++)
        {
            for (int x = 0; x<mapWidth; x++)
            {
                mapColour[y * mapWidth + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }

        texture.SetPixels(mapColour);
        texture.Apply();
}
