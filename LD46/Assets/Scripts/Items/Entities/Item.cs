﻿using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    protected ItemWorldUI UI = null;

    [SerializeField]
    protected Animator _animator = null;

    [SerializeField]
    private int _baseHealth = 1;

    public Price Price;

    [SerializeField]
    protected float _actionFrequency = 1f; // Seconds

    public delegate void ItemEventHandler(Item item);
    public delegate void ItemCollisionEventHandler(Item item, int collisionCount);

    public event ItemEventHandler OnDied;
    public event ItemCollisionEventHandler OnCollisionTriggerEnter;
    public event ItemCollisionEventHandler OnCollisionTriggerExit;

    protected int _hp;
    private int _collisionCount = 0;

    public Animator Animator => _animator;

    private List<string> _allowedItemTags = new List<string>()
    {
        "Building", "Unit", "Enemy", "Resource", "EnemyBuilding"
    };

    protected Item _currentLocationTarget;
    protected float _actionTimer = 0f;

    public int HP => _hp;

    private void Start()
    {
        Initialize();
    }

    public virtual void Initialize()
    {
        _hp = _baseHealth;
        _actionTimer = _actionFrequency;
     
        UI.Initialize(GameManager.Instance.MainCamera, this);
    }

    private void Update()
    {
        InternalUpdate();
    }

    protected virtual void InternalUpdate()
    {
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

    public void TakeDamage(int amount)
    {
        _hp -= amount;

        UI.UpdateHealthBar(_hp);
        UI.AmountChanged(-amount);

        if (_hp <= 0)
        {
            Kill();
            OnDied?.Invoke(this);
        }
    }

    public virtual void Kill()
    {
        Destroy(gameObject);
    }

    public void Select()
    {
        UI.Select();
    }

    public void Unselect()
    {
        UI.Unselect();
    }

    public void OnTriggerEnter(Collider other)
    {
        _collisionCount++;

        //Debug.Log($"Enter into {other.name}");

        Item item = other.gameObject.GetComponent<Item>();

        if (item != null && _allowedItemTags.Contains(item.tag))
        {
            OnItemEnter(item);
        }

        OnCollisionTriggerEnter?.Invoke(this, _collisionCount);
    }

    public void OnTriggerExit(Collider other)
    {
        _collisionCount--;

        //Debug.Log($"Exit from {other.name}");

        Item item = other.gameObject.GetComponent<Item>();

        if (item != null)
        {
            OnItemExit(item);
        }

        OnCollisionTriggerExit?.Invoke(this, _collisionCount);
    }

    protected virtual void OnItemEnter(Item item)
    {
    }

    protected virtual void OnItemExit(Item item)
    {
    }
}
