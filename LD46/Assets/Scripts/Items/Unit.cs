using UnityEngine;
using UnityEngine.AI;

public class Unit : Item
{
    public NavMeshAgent Agent = null;

    [SerializeField]
    protected int _attack = 1;

    [SerializeField]
    protected float _actionFrequency = 1f; // Seconds

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
    }

    protected virtual void ExecuteAction()
    {
    }
}
