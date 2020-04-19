using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfiguration.asset", menuName = "LD46/GameConfiguration")]
public class GameConfiguration : ScriptableObject
{
    // 10 difficuly levels
    // Difficulty starts at 1 and increase according time and/or units count
    
    public List<float> WorldChunkSpawnFrequency = new List<float>() // Seconds
    {
        60, 80, 100, 120, 150, 200, 250, 300, 500, 550
    };

    
    public List<int> EnemySpawnerMinOnNewChunk = new List<int>()
    {
        0, 0, 1, 1, 2, 2, 3, 3, 4, 5
    };

    
    public List<int> EnemySpawnerMaxOnNewChunk = new List<int>()
    {
        1, 1, 1, 2, 3, 4, 5, 5, 7, 10
    };

    
    public List<float> EnemySpawnerMinSpawnFrequency = new List<float>()
    {
        60, 60, 50, 40, 30, 20, 10, 10, 5, 1
    };

    
    public List<float> EnemySpawnerMaxSpawnFrequency = new List<float>()
    {
        150, 120, 100, 80, 50, 40, 30, 30, 20, 10
    };

    
    public List<float> ResourcePercentageOnNewChunk = new List<float>()
    {
        0.05f, 0.08f, 0.1f, 0.12f, 0.15f, 0.2f, 0.25f, 0.3f, 0.5f, 0.7f
    };

    
    public List<int> ResourceQuantityMinOnNewChunk = new List<int>()
    {
        1, 2, 2, 5, 7, 7, 10, 12, 15, 15
    };

    
    public List<int> ResourceQuantityMaxOnNewChunk = new List<int>()
    {
        5, 7, 10, 15, 17, 20, 25, 35, 45, 50
    };

    #region Prices

    
    public List<Dictionary<EResourceType, int>> SoldierPrice = new List<Dictionary<EResourceType, int>>
    {
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        }
    };

    
    public List<Dictionary<EResourceType, int>> WorkerPrice = new List<Dictionary<EResourceType, int>>
    {
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        }
    };

    
    public List<Dictionary<EResourceType, int>> WallPrice = new List<Dictionary<EResourceType, int>>
    {
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        },
        new Dictionary<EResourceType, int>()
        {
            { EResourceType.Wood, 5 },
            { EResourceType.Rock, 5 },
            { EResourceType.Gold, 3 },
        }
    };


    #endregion


}
