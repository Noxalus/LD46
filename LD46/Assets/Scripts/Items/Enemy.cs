
using UnityEngine;

public class Enemy : Unit
{
    private Unit _currentTarget;

    public override void Initialize()
    {
        base.Initialize();
    }

    protected override void InternalUpdate()
    {
        base.InternalUpdate();

        Agent.SetDestination(_gameManager.King.transform.position);
    }

    protected override void ExecuteAction()
    {
        base.ExecuteAction();

        if (_currentTarget != null)
        {
            _currentTarget.TakeDamage(_attack);
        }
    }

    protected override void OnItemEnter(Item item)
    {
        if (item is Unit unit)
        {
            Debug.Log("Found unity, start to attack it!");
            _currentTarget = unit;
        }
    }

    protected override void OnItemExit(Item item)
    {

    }
}
