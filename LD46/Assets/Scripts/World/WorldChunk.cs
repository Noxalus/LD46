using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldChunk : MonoBehaviour
{
    private Dictionary<EDirection, WorldChunk> _neighbors = new Dictionary<EDirection, WorldChunk>();

    public void Initialize(WorldBuilder worldBuilder, WorldChunk parent, EDirection parentDirection)
    {
        Vector3 newChunkPosition = Vector3.zero;

        if (parent != null)
        {
            _neighbors[parentDirection] = parent;

            EDirection newChunkDirection = GetInverseDirection(parentDirection);
            newChunkPosition = parent.transform.position;
            newChunkPosition += worldBuilder.ChunkStep * DirectionToVector(newChunkDirection);
        }

        gameObject.name = $"({newChunkPosition.x}, {newChunkPosition.z})";

        transform.position = newChunkPosition;
    }

    private EDirection GetInverseDirection(EDirection direction)
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

    private Vector3 DirectionToVector(EDirection direction)
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
