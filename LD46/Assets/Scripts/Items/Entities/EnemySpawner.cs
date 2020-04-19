using UnityEngine;

public class EnemySpawner : Building
{
    [SerializeField]
    private Transform _spawnPosition = null;

    [SerializeField]
    private Enemy _enemyPrefab = null;

    public void Initialize(float? spawnFrequence = null)
    {
        base.Initialize();

        if (spawnFrequence.HasValue)
        {
            _actionFrequency = spawnFrequence.Value;
        }

        // Execute action directly
        //_actionTimer = 0;
        ExecuteAction();
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
        GameManager.Instance.AddEnemy(
            Instantiate(_enemyPrefab, _spawnPosition)
        );
    }
}
