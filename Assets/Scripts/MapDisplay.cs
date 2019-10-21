using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is going to take the noiseMap from MapGenerator and turn it into a texture
public class MapDisplay : MonoBehaviour
{
    public Renderer textureRenderer;

    public void DrawTexture (Texture2D texture)
    {
        // we wanna set the texture so that we can edit it outside of runtime
        textureRenderer.sharedMaterial.mainTexture = texture;
        // we wanna match the size of the plane to the noiseMap
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);

    }
}
