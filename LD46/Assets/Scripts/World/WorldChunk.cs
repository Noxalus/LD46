using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NeighborChunksDictionary : SerializableDictionary<EDirection, WorldChunk> { }

public class WorldChunk : MonoBehaviour
{
    [SerializeField] // For debug
    private NeighborChunksDictionary _neighbors = new NeighborChunksDictionary();

    public void Initialize(WorldBuilder worldBuilder, WorldChunk parent, EDirection parentDirection)
    {
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
}
