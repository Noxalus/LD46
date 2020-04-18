﻿public class Enemy : Unit
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
}
