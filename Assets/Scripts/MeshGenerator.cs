using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    // We'll return the meshData because Unity has a limitation when it comes to multithreading
    public static MeshData GenerateTerrainMesh (float [,] heightMap, float heightMultiplier, AnimationCurve _meshHeightCurve, int levelOfDetail)
    {
        AnimationCurve meshHeightCurve = new AnimationCurve(_meshHeightCurve.keys);
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topRightX = (width - 1) / -2f;
        float topRightZ = (height - 1) / 2f;
        int meshSimplificationIncrement = (levelOfDetail == 0)?1:levelOfDetail * 2;
        int verticesPerLine = (width - 1) / meshSimplificationIncrement + 1;

        MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);

        int vertexIndex = 0;

        for (int y = 0; y < height; y+=meshSimplificationIncrement)
        {
            for (int x = 0; x < width; x+=meshSimplificationIncrement)
            {

                //we want to start to generate the mesh from top left corner.
                meshData.vertices[vertexIndex] = new Vector3(topRightX + x, meshHeightCurve.Evaluate(heightMap[x,y]) * heightMultiplier * heightMap[x, y], topRightZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                //ignoring the right and bottom vertices because u can't build a triangle from them
                if (x < width-1 && y < height - 1) 
                {
                    meshData.AddTraiangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    meshData.AddTraiangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }
                vertexIndex++;
            }
        }
        return meshData;
    }
}

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs; 
    // let's create a UV Map which we can apply to our texture

    int triangleIndex;

    public MeshData (int width, int height)
    {
        vertices = new Vector3[width * height];
        triangles = new int[(width-1)*(height-1)*6];
        uvs = new Vector2[width * height];
    }

    // method to add traiangle
    public void AddTraiangle(int a, int b, int c) 
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex+1] = b;
        triangles[triangleIndex+2] = c;

        triangleIndex += 3;
    }

    public Mesh CreateMesh ()
    {
        Mesh mesh = new Mesh ();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        return mesh;
    }
}
