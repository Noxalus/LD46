﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private ItemWorldUI _ui = null;

    [SerializeField]
    private int _baseHealth = 1;

    public delegate void ItemEventHandler(Item item);
    public event ItemEventHandler OnDied;

    protected GameManager _gameManager;
    protected int _health;

    private List<string> _allowedItemTags = new List<string>()
    {
        "Building", "Unit", "Enemy"
    };

    private void Start()
    {
        _gameManager = GameManager.Instance;

        Initialize();
    }

    public virtual void Initialize()
    {
        _health = _baseHealth;
        _ui.Initialize(_gameManager.Camera);
    }

    private void Update()
    {
        InternalUpdate();
    }

    protected virtual void InternalUpdate()
    {
    }

    public void TakeDamage(int amount)
    {
        _health -= amount;

        _ui.UpdateHealthBar((float)_health / _baseHealth);
        _ui.AmountChanged(-amount);

        if (_health <= 0)
        {
            Destroy(gameObject);
            Kill();
            OnDied?.Invoke(this);
        }
    }

    public virtual void Kill()
    {
    }

    public void Select()
    {
        _ui.Select();
    }

    public void Unselect()
    {
        _ui.Unselect();
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Enter into {other.name}");

        Item item = other.gameObject.GetComponent<Item>();

        if (item != null && _allowedItemTags.Contains(item.tag))
        {
            OnItemEnter(item);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log($"Exit from {other.name}");

        Item item = other.gameObject.GetComponent<Item>();

        if (item != null)
        {
            OnItemExit(item);
        }
    }

    protected virtual void OnItemEnter(Item item)
    {
    }

    protected virtual void OnItemExit(Item item)
    {
    }
}
