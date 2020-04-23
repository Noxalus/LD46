public class Enemy : Unit
{
    public void Initialize(int health)
    {
        _baseHealth = health;

        base.Initialize();

        FindNewTarget();
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
            Attack();
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

        if (IsInterestingForMe(item))
        {
            // Focus unit instead of chasing the king
            if (item is Unit)
            {
                _currentActiveTarget = item;
                SetLocationTarget(_currentActiveTarget);
            }
        }
    }

    protected override void FindNewTarget()
    {
        base.FindNewTarget();

        if (_currentActiveTarget == null)
        {
            SetLocationTarget(GameManager.Instance.King);
        }
    }
}
