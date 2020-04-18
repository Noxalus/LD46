using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : Item
{
    public NavMeshAgent Agent = null;

    [SerializeField]
    protected int _attack = 1;

    [SerializeField]
    protected float _actionFrequency = 1f; // Seconds

    [SerializeField]
    private List<string> _itemsToWatchTags = new List<string>();

    [Header("Debug")]

    [SerializeField] // To debug
    protected Item _currentActiveTarget;

    [SerializeField] // To debug
    protected List<Item> _surroundingTargets = new List<Item>();

    protected Item _currentLocationTarget;
    protected float _actionTimer = 0f;

    public override void Initialize()
    {
        base.Initialize();

        _actionTimer = _actionFrequency;
    }

    protected override void InternalUpdate()
    {
        base.InternalUpdate();

        _actionTimer -= Time.deltaTime;

        if (_actionTimer <= 0)
        {
            ExecuteAction();
            _actionTimer = _actionFrequency;
        }

        // Move toward affected target
        if (_currentLocationTarget != null)
        {
            Agent.SetDestination(_currentLocationTarget.transform.position);
        }
    }

    public void SetTarget(Item target)
    {
        _currentLocationTarget = target;

        // Stop movement if the unit doesn't have target
        Agent.isStopped = target == null;
    }

    protected virtual void ExecuteAction()
    {
    }

    protected override void OnItemEnter(Item item)
    {
        if (!_itemsToWatchTags.Contains(item.tag))
        {
            Debug.LogWarning($"{gameObject.name} don't need to watch {item.name}");
            return;
        }

        // We reached the target!
        if (_currentLocationTarget == item)
        {
            _currentActiveTarget = item;
        }

        if (!_surroundingTargets.Contains(item))
        {
            _surroundingTargets.Add(item);
        }
    }

    protected override void OnItemExit(Item item)
    {
        _surroundingTargets.Remove(item);

        if (item == _currentActiveTarget)
        {
            _currentActiveTarget = null;
            FindNewTarget();
        }
    }

    protected virtual void FindNewTarget()
    {
        foreach (var target in _surroundingTargets)
        {
            if (target != null)
            {
                _currentActiveTarget = target;
            }
        }

        // Still no target?
        if (_currentActiveTarget == null)
        {
            _surroundingTargets.Clear();

            // Ask GameManager a new target to move at?
        }
    }
}
