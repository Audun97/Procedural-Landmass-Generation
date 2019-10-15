using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is going to take the noiseMap from MapGenerator and turn it into a texture
public class MapDisplay : MonoBehaviour
{
    public Renderer textureRenderer;

    public void DrawNoiseMap (float [,] noiseMap)
    {
        // (0) and (1) refers to the dimensions
        int mapWidth = noiseMap.GetLength (0);
        int mapHeight = noiseMap.GetLength(1);

        Texture2D texture = new Texture2D(mapWidth, mapHeight);

        // It is all more efficient when I first put all the values in an Array and then set the Pixels
        // Note that the Color Array is only one dimensional

        Color[] mapColour = new Color [mapWidth * mapHeight];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                mapColour[y * mapWidth + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }

        texture.SetPixels(mapColour);
        texture.Apply();

        // we wanna set the texture so that we can edit it outside of runtime
        textureRenderer.sharedMaterial.mainTexture = texture;
        // we wanna match the size of the plane to the noiseMap
        textureRenderer.transform.localScale = new Vector3(mapWidth, 1, mapHeight);

    }
}
