using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : Item
{
    public NavMeshAgent Agent = null;

    [SerializeField]
    private ParticleSystem _hurtFx = null;

    [SerializeField]
    private SFXCollection _hitSounds = null;

    [SerializeField]
    private SFXCollection _hurtSounds = null;

    [SerializeField]
    protected int _attack = 1;

    [SerializeField]
    private List<string> _itemsToWatchTags = new List<string>();

    [Header("Debug")]

    [SerializeField] // To debug
    protected Item _currentActiveTarget;

    [SerializeField] // To debug
    protected List<Item> _surroundingTargets = new List<Item>();

    public override void Initialize()
    {
        base.Initialize();
    }

    protected override void InternalUpdate()
    {
        base.InternalUpdate();

        if (Agent)
        {
            if (_animator)
            {
                _animator.SetBool("IsMoving", Agent.velocity.magnitude > Agent.speed * 0.7f);
            }

            // Move toward affected target
            if (_currentLocationTarget != null)
            {
                if (Agent != null)
                {
                    Agent.SetDestination(_currentLocationTarget.transform.position);
                }

                if (_surroundingTargets.Contains(_currentLocationTarget))
                {
                    SetActiveTarget(_currentLocationTarget);
                    SetLocationTarget(null);
                }
            }
        }
    }

    public override void Select()
    {
        base.Select();

        // Check it's a player's unit
        if (tag == "Unit")
        {
            GameManager.Instance.AudioManager.PlaySelectUnitSound();
        }
    }

    public void SetLocationTarget(Item target)
    {
        _currentLocationTarget = target;

        if (Agent)
        {
            // Stop movement if the unit doesn't have target
            Agent.isStopped = target == null;
        }
    }

    public virtual void SetActiveTarget(Item target)
    {
        _currentActiveTarget = target;

        if (target != null)
        {
            transform.LookAt(_currentActiveTarget.transform);
        }
    }

    protected override void OnItemEnter(Item item)
    {
        if (!_itemsToWatchTags.Contains(item.tag))
        {
            //Debug.LogWarning($"{gameObject.name} don't need to watch {item.name}");
            return;
        }

        // We reached the target!
        if (_currentLocationTarget == item)
        {
            SetActiveTarget(item);
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
            SetActiveTarget(null);
            FindNewTarget();
        }
    }

    protected virtual void FindNewTarget()
    {
        foreach (var target in _surroundingTargets)
        {
            if (target != null)
            {
                SetActiveTarget(_currentActiveTarget);
            }
        }

        // Still no target?
        if (_currentActiveTarget == null)
        {
            _surroundingTargets.Clear();

            // Ask GameManager a new target to move at?
        }
    }

    protected void Attack()
    {
        _currentActiveTarget.TakeDamage(_attack);

        if (_animator != null)
        {
            _animator.SetTrigger("Attack");
        }

        if (_audioSource != null && _hitSounds != null)
        {
            _audioSource.PlayOneShot(_hitSounds.GetRandomSound());
        }
    }

    public override void TakeDamage(int amount)
    {
        if (_hurtFx)
        {
            _hurtFx.Play();
        }

        if (_audioSource != null && _hurtSounds != null)
        {
            _audioSource.PlayOneShot(_hurtSounds.GetRandomSound());
        }

        base.TakeDamage(amount);
    }

    public override void Kill()
    {
        base.Kill();
    }
}
