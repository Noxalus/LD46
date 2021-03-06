﻿using UnityEngine;

public class EnemySpawner : Building
{
    [SerializeField]
    private Transform _spawnPosition = null;

    [SerializeField]
    private Enemy _enemyPrefab = null;

    public void Initialize(float? spawnFrequence = null)
    {
        if (spawnFrequence.HasValue)
        {
            _actionFrequency = spawnFrequence.Value;
        }

        base.Initialize();

        // Execute action directly
        //_actionTimer = 0;
        //ExecuteAction();
        SpawnEnemy();
    }

    protected override void InternalUpdate()
    {
        base.InternalUpdate();
    }

    protected override void ExecuteAction()
    {
        base.ExecuteAction();

        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        Enemy enemy = Instantiate(_enemyPrefab, _spawnPosition.position, _spawnPosition.rotation);

        enemy.Initialize(
            GameManager.Instance.GameConfiguration.EnemyHealth[GameManager.Instance.Difficulty]
        );

        GameManager.Instance.AddEnemy(enemy);
    }
}
