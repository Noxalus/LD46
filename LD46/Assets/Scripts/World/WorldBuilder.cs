using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public enum EDirection
{
    None,
    Up,
    Right,
    Down,
    Left
}

public class WorldBuilder : MonoBehaviour
{
    [SerializeField]
    private WorldChunk _worldChunk = null;

    [SerializeField]
    private float _chunkStep = 20f;

    private List<WorldChunk> _chunks = new List<WorldChunk>();

    public float ChunkStep => _chunkStep;

    public void Initialize()
    {
        GenerateNewChunk();
    }

    public void GenerateNewChunk()
    {
        Stopwatch time = new Stopwatch();
        time.Start();

        WorldChunk newChunk = Instantiate(_worldChunk);

        // First chunk
        if (_chunks.Count == 0)
        {
            newChunk.Initialize(this, null, EDirection.None);
            var animator = newChunk.GetComponent<Animator>();

            if (animator != null)
            {
                Destroy(animator);
            }
        }
        else
        {
            // Get the world chunks that don't have all their neighbor yet
            List<WorldChunk> expandableChunks = new List<WorldChunk>();
            
            foreach (var chunk in _chunks)
            {
                List<EDirection> currentChunkEmptyNeighbors = chunk.GetEmptyNeighbors();

                if (currentChunkEmptyNeighbors.Count > 0)
                {
                    expandableChunks.Add(chunk);
                }
            }

            WorldChunk randomExpandableChunk = expandableChunks[Random.Range(0, expandableChunks.Count)];
            List<EDirection> emptyNeighbors = randomExpandableChunk.GetEmptyNeighbors();
            int randomDirectionIndex = Random.Range(0, emptyNeighbors.Count);
            EDirection randomDirection = emptyNeighbors[randomDirectionIndex];

            int enemySpawnerCount = Random.Range(1, 3);
            newChunk.Initialize(this, randomExpandableChunk, GetInverseDirection(randomDirection), enemySpawnerCount);
        }

        _chunks.Add(newChunk);

        time.Stop();
        Debug.Log($"Time to generate a new chunk: {time.ElapsedMilliseconds}ms");

        time.Restart();
        GameManager.Instance.NavMeshSurface.BuildNavMesh();
        time.Stop();

        Debug.Log($"Time to build navmesh: {time.ElapsedMilliseconds}ms");
    }

    public Dictionary<EDirection, WorldChunk> CheckSurroundingNeighbors(Vector3 chunkPosition, EDirection except = EDirection.None)
    {
        Dictionary<EDirection, WorldChunk> neighbors = new Dictionary<EDirection, WorldChunk>();

        var directionsToCheck = new Dictionary<EDirection, Vector3>();

        var leftPosition = chunkPosition + _chunkStep * DirectionToVector(EDirection.Left);
        var upPosition = chunkPosition + _chunkStep * DirectionToVector(EDirection.Up);
        var rightPosition = chunkPosition + _chunkStep * DirectionToVector(EDirection.Right);
        var downPosition = chunkPosition + _chunkStep * DirectionToVector(EDirection.Down);

        if (except != EDirection.Left)
            directionsToCheck.Add(EDirection.Left, leftPosition);
        if (except != EDirection.Up)
            directionsToCheck.Add(EDirection.Up, upPosition);
        if (except != EDirection.Right)
            directionsToCheck.Add(EDirection.Right, rightPosition);
        if (except != EDirection.Down)
            directionsToCheck.Add(EDirection.Down, downPosition);

        foreach (var chunk in _chunks)
        {
            foreach (var directionPair in directionsToCheck)
            {
                if (chunk.transform.position.Equals(directionPair.Value))
                {
                    neighbors.Add(directionPair.Key, chunk);
                }
            }
        }

        return neighbors;
    }

    public void Clear()
    {
        foreach (var chunk in _chunks)
        {
            if (chunk != null)
                Destroy(chunk.gameObject);
        }

        _chunks.Clear();
    }

    public EDirection GetInverseDirection(EDirection direction)
    {
        switch (direction)
        {
            case EDirection.Down:
                return EDirection.Up;
            case EDirection.Up:
                return EDirection.Down;
            case EDirection.Left:
                return EDirection.Right;
            case EDirection.Right:
                return EDirection.Left;
            default:
                throw new Exception("Unknown direction");
        }
    }

    public Vector3 DirectionToVector(EDirection direction)
    {
        switch (direction)
        {
            case EDirection.Down:
                return Vector3.back;
            case EDirection.Up:
                return Vector3.forward;
            case EDirection.Left:
                return Vector3.left;
            case EDirection.Right:
                return Vector3.right;
            default:
                throw new Exception("Unknown direction");
        }
    }
}
