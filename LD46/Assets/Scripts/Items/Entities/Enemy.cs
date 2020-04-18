public class Enemy : Unit
{
    public override void Initialize()
    {
        base.Initialize();

        _currentLocationTarget = _gameManager.King;
    }

    protected override void InternalUpdate()
    {
        base.InternalUpdate();
    }

    protected override void ExecuteAction()
    {
        base.ExecuteAction();

        if (_currentActiveTarget != null)
        {
            _currentActiveTarget.TakeDamage(_attack);
        }
        else
        {
            // Search for new target
            FindNewTarget();
        }
    }

    protected override void OnItemEnter(Item item)
    {
        base.OnItemEnter(item);

        // Focus unit instead of chasing the king
        if (item is Unit)
        {
            SetTarget(_currentActiveTarget);
        }
    }

    protected override void FindNewTarget()
    {
        base.FindNewTarget();

        if (_currentActiveTarget == null)
        {
            SetTarget(_gameManager.King);
        }
    }
}
