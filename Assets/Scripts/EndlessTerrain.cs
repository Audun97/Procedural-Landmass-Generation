﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    public const float maxViewDistance = 300;
    public Transform viewer;
    public static Vector2 viewerPosition;
    int chunkSize;
    int chunkVisable;

    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();


    // Start is called before the first frame update
    void Start()
    {
        chunkSize = MapGenerator.mapChunkSize - 1;
        chunkVisable = Mathf.RoundToInt(maxViewDistance / chunkSize);
    }

    void UpdateVisableChunks ()
    {
        int currentChunkCordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        for (int offsetX = -chunkVisable; offsetX < chunkVisable; offsetX++)
        {
            for (int offsetY =-chunkVisable; offsetY < chunkVisable; offsetY++)
            {
                Vector2 viewedChunkCord = new Vector2(currentChunkCordX + offsetX, currentChunkCordY + offsetY);

                if (terrainChunkDictionary.ContainsKey(viewedChunkCord)) 
                {
                    terrainChunkDictionary[viewedChunkCord].UpdateTerrainChunk();
                }
                else
                {
                    terrainChunkDictionary.Add(viewedChunkCord, new TerrainChunk(chunkSize, viewedChunkCord));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public class TerrainChunk
    {
        Vector2 position;
        GameObject meshObject;
        Bounds bounds;

        public TerrainChunk(int size, Vector2 cord)
        {
            position = size * cord;
            Vector3 positionV3 = new Vector3(position.x, 0, position.y);
            bounds = new Bounds(position, Vector3.one * size);
            meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            meshObject.transform.position = positionV3;
            // devide by 10 becauae the default plane is scaled by 10
            meshObject.transform.localScale = Vector3.one * size / 10f;
            SetVisable(false);
        }
        public void UpdateTerrainChunk()
        {
            float viewerDistanceFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
            bool visable = viewerDistanceFromNearestEdge <= maxViewDistance;
            SetVisable(visable);
        }

        public void SetVisable (bool visable)
        {
            meshObject.SetActive(visable);
        }
    }
}


