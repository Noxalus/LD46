﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : Item
{
    public NavMeshAgent Agent = null;

    [SerializeField]
    protected int _attack = 1;

    [SerializeField]
    private List<string> _itemsToWatchTags = new List<string>();

    [Header("Debug")]

    [SerializeField] // To debug
    protected Item _currentActiveTarget;

    [SerializeField] // To debug
    protected List<Item> _surroundingTargets = new List<Item>();

    protected Rigidbody _rigidbody;

    public override void Initialize()
    {
        base.Initialize();

        _rigidbody = GetComponent<Rigidbody>();
    }

    protected override void InternalUpdate()
    {
        base.InternalUpdate();

        if (_animator && _rigidbody)
        {
            var mag = Agent.velocity.magnitude > 0;

            Debug.Log($"[{name}] => {Agent.velocity.magnitude}");

            _animator.SetBool("IsMoving", Agent.velocity.magnitude > Agent.speed * 0.7f);
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

    protected bool IsInterestingForMe(Item item)
    {
        return _itemsToWatchTags.Contains(item.tag);
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
