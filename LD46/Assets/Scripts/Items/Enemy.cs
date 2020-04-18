
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    [SerializeField]
    private Item _currentAttackTarget;

    [SerializeField]
    private List<Item> _surroundingTargets = new List<Item>();

    private Item _currentLocationTarget;

    public override void Initialize()
    {
        base.Initialize();

        _currentLocationTarget = _gameManager.King;
    }

    protected override void InternalUpdate()
    {
        base.InternalUpdate();

        if (_currentLocationTarget != null)
        {
            Agent.SetDestination(_currentLocationTarget.transform.position);
        }
    }

    protected override void ExecuteAction()
    {
        base.ExecuteAction();

        if (_currentAttackTarget != null)
        {
            _currentAttackTarget.TakeDamage(_attack);
        }
        else
        {
            // Search for new target
            FindNewTarget();
        }
    }

    protected override void OnItemEnter(Item item)
    {
        Debug.Log("Found item, start to attack it!");
        _currentAttackTarget = item;

        if (!_surroundingTargets.Contains(item))
        {
            _surroundingTargets.Add(item);
        }
    }

    protected override void OnItemExit(Item item)
    {
        _surroundingTargets.Remove(item);

        if (item == _currentAttackTarget)
        {
            FindNewTarget();
        }
    }

    private void FindNewTarget()
    {
        foreach (var target in _surroundingTargets)
        {
            if (target != null)
            {
                _currentAttackTarget = target;
            }
        }

        // Still no target?
        if (_currentAttackTarget == null)
        {
            _surroundingTargets.Clear();

            // Ask GameManager a new target to move at
        }
    }
}
