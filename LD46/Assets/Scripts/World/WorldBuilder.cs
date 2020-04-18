using System.Collections.Generic;
using UnityEngine;

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
        if (_chunks.Count == 0)
        {
            WorldChunk chunk = Instantiate(_worldChunk);
            chunk.Initialize(this, null, EDirection.None);

            _chunks.Add(chunk);
        }
        else
        {
            // Get the world chunks that don't have all their neighbor yet
        }

        GameManager.Instance.NavMeshSurface.BuildNavMesh();
    }
}
