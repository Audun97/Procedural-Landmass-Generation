using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    public const float maxViewDistance = 550;
    public Transform viewer;
    public Material material;
    public static Vector2 viewerPosition;
    public static MapGenerator mapGen;

    int chunkSize;
    int chunkVisable;

    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> TerrainChunksOutOfSight = new List<TerrainChunk>();

    // Start is called before the first frame update
    void Start()
    {
        mapGen = FindObjectOfType<MapGenerator>();
        chunkSize = MapGenerator.mapChunkSize - 1;
        chunkVisable = Mathf.RoundToInt(maxViewDistance / chunkSize);
    }

    void UpdateVisableChunks ()
    {
        int currentChunkCordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        // Lets loop through all the Chunks which are out of sight

        for (int i = 0; i< TerrainChunksOutOfSight.Count; i++)
        {
            TerrainChunksOutOfSight[i].SetVisable(false);
        }

        for (int offsetX = -chunkVisable; offsetX < chunkVisable; offsetX++)
        {
            for (int offsetY =-chunkVisable; offsetY < chunkVisable; offsetY++)
            {
                Vector2 viewedChunkCord = new Vector2(currentChunkCordX + offsetX, currentChunkCordY + offsetY);

                if (terrainChunkDictionary.ContainsKey(viewedChunkCord)) 
                {
                    terrainChunkDictionary[viewedChunkCord].UpdateTerrainChunk();

                    if (terrainChunkDictionary[viewedChunkCord].IsVisable())
                    {
                        TerrainChunksOutOfSight.Add(terrainChunkDictionary[viewedChunkCord]);
                    }
                }
                else
                {
                    terrainChunkDictionary.Add(viewedChunkCord, new TerrainChunk(chunkSize, viewedChunkCord, transform, material));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
        UpdateVisableChunks();
    }
    public class TerrainChunk
    {
        Vector2 position;
        GameObject meshObject;
        Bounds bounds;

        MapData mapData;
        MeshRenderer meshRenderer;
        MeshFilter meshFilter; 

        public TerrainChunk(int size, Vector2 cord, Transform parent, Material material)
        {
            position = size * cord;
            Vector3 positionV3 = new Vector3(position.x, 0, position.y);
            bounds = new Bounds(position, Vector3.one * size);
            meshObject = new GameObject("Terrain Chunk");
            meshObject.transform.position = positionV3;
            meshObject.transform.parent = parent;
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshRenderer.material = material;
            meshFilter = meshObject.AddComponent<MeshFilter>();
            // devide by 10 becauae the default plane is scaled by 10
        
            SetVisable(false);

            mapGen.RequestMapData(OnMapDataReceived);
        }

        void OnMapDataReceived(MapData mapData)
        {
            mapGen.RequestMeshData(mapData, OnMeshDataReceived);
        }

        void OnMeshDataReceived(MeshData meshData)
        {
            meshFilter.mesh = meshData.CreateMesh();
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
        public bool IsVisable() 
        {
            return meshObject.activeSelf;
        }
    }
}


