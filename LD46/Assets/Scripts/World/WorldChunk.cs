using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[Serializable]
public class NeighborChunksDictionary : SerializableDictionary<EDirection, WorldChunk> { }

public class WorldChunk : MonoBehaviour
{
    //[SerializeField]
    //private NavMeshSurface _navMeshSurface = null;

    [SerializeField]
    private float _spaceBetweenResources = 5f;

    [SerializeField]
    private float _resourcesProbability = 0.5f;

    [SerializeField]
    private List<Tree> _treePrefabs = new List<Tree>();

    [SerializeField]
    private List<Rock> _rockPrefabs = new List<Rock>();

    [SerializeField]
    private List<Gold> _goldPrefabs = new List<Gold>();

    [SerializeField]
    private List<EnemySpawner> _enemySpawnerPrefabs = new List<EnemySpawner>();

    [SerializeField] // For debug
    private NeighborChunksDictionary _neighbors = new NeighborChunksDictionary();

    private WorldBuilder _worldBuilder;

    public void Initialize(
        WorldBuilder worldBuilder, 
        WorldChunk parent, 
        EDirection parentDirection,
        int enemySpawnerMaxCount = 0)
    {
        _worldBuilder = worldBuilder;

        Vector3 newChunkPosition = Vector3.zero;

        if (parent != null)
        {
            _neighbors[parentDirection] = parent;

            EDirection newChunkDirection = worldBuilder.GetInverseDirection(parentDirection);
            newChunkPosition = parent.transform.position;
            newChunkPosition += worldBuilder.ChunkStep * worldBuilder.DirectionToVector(newChunkDirection);

            parent.SetNeighbor(this, newChunkDirection);

            var surroundingNeihbors = worldBuilder.CheckSurroundingNeighbors(newChunkPosition, parentDirection);
            foreach (var chunkPair in surroundingNeihbors)
            {
                SetNeighbor(chunkPair.Value, chunkPair.Key);
                chunkPair.Value.SetNeighbor(this, worldBuilder.GetInverseDirection(chunkPair.Key));
                //Debug.Log($"Added neighbor to the {chunkPair.Key} => {chunkPair.Value.name}");
            }
        }

        gameObject.name = $"({newChunkPosition.x}, {newChunkPosition.z})";

        transform.position = newChunkPosition;

        GenerateResources();
        GenerateEnemies(enemySpawnerMaxCount);

        //_navMeshSurface.BuildNavMesh();
    }

    public void SetNeighbor(WorldChunk newChunk, EDirection direction)
    {
        _neighbors[direction] = newChunk;
    }

    public List<EDirection> GetEmptyNeighbors()
    {
        List<EDirection> emptyNeighbors = new List<EDirection>()
        {
            EDirection.Up, EDirection.Right, EDirection.Down, EDirection.Left
        };

        foreach (EDirection direction in _neighbors.Keys)
        {
            emptyNeighbors.Remove(direction);
        }

        return emptyNeighbors;
    }

    private void GenerateResources()
    {
        Vector4 bounds = GetBounds();

        Vector2 bottomLeftBound = new Vector2(bounds.x, bounds.y);
        Vector2 topRightBound = new Vector2(bounds.z, bounds.w);


        for (float y = bottomLeftBound.y; y < topRightBound.y; y += _spaceBetweenResources)
        {
            for (float x = bottomLeftBound.x; x < topRightBound.x; x += _spaceBetweenResources)
            {
                if (Random.value > _resourcesProbability)
                {
                    continue;
                }

                Vector2 randomOffset = Vector2.zero;

                if (x != bottomLeftBound.x && x != topRightBound.x)
                {
                    int factor = Random.value > 0.5f ? 1 : -1;
                    randomOffset.x += factor * Random.Range(0, _spaceBetweenResources / 2f);
                }

                if (y != bottomLeftBound.y && y != topRightBound.y)
                {
                    int factor = Random.value > 0.5f ? 1 : -1;
                    randomOffset.y += factor * Random.Range(0, _spaceBetweenResources / 2f);
                }

                Debug.Log($"Place item on position: {x}, {y}");

                Resource resourceInstance;

                if (Random.value < 0.33f)
                {
                    resourceInstance = Instantiate(
                        _treePrefabs[Random.Range(0, _treePrefabs.Count)], 
                        new Vector3(x + randomOffset.x, 0, y + randomOffset.y), 
                        Quaternion.identity
                    );
                }
                else if (Random.value < 0.66f)
                {
                    resourceInstance = Instantiate(
                        _rockPrefabs[Random.Range(0, _rockPrefabs.Count)], 
                        new Vector3(x + randomOffset.x, 0, y + randomOffset.y), 
                        Quaternion.identity
                    );
                }
                else
                {
                    resourceInstance = Instantiate(
                        _goldPrefabs[Random.Range(0, _goldPrefabs.Count)], 
                        new Vector3(x + randomOffset.x, 0, y + randomOffset.y), 
                        Quaternion.identity
                    );
                }

                resourceInstance.transform.Rotate(Vector3.up, Random.Range(0, 360));
                GameManager.Instance.AddResources(resourceInstance);
            }
        }
    }

    private void GenerateEnemies(int maxCount)
    {
        int count = Random.Range(0, maxCount + 1);

        for (int i = 0; i < count; i++)
        {
            Vector4 bounds = GetBounds();

            Vector3 randomPosition = new Vector3(
                Random.Range(bounds.x, bounds.z),
                0,
                Random.Range(bounds.y, bounds.w)
            );

            var instance = Instantiate(
                _enemySpawnerPrefabs[Random.Range(0, _enemySpawnerPrefabs.Count)],
                randomPosition,
                Quaternion.identity
            );
            instance.transform.Rotate(Vector3.up, Random.Range(0, 360));

            instance.Initialize(Random.Range(20, 150));

            GameManager.Instance.AddEnemyBuilding(instance);
        }
    }

    private Vector4 GetBounds()
    {
        return new Vector4(
            transform.position.x - _worldBuilder.ChunkStep / 2 + 1,
            transform.position.z - _worldBuilder.ChunkStep / 2 + 1,
            transform.position.x + _worldBuilder.ChunkStep / 2,
            transform.position.z + _worldBuilder.ChunkStep / 2
        );
    }
}
