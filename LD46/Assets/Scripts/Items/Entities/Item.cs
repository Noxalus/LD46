using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    protected ItemWorldUI UI = null;

    [SerializeField]
    private int _baseHealth = 1;

    public delegate void ItemEventHandler(Item item);
    public delegate void ItemCollisionEventHandler(Item item, int collisionCount);

    public event ItemEventHandler OnDied;
    public event ItemCollisionEventHandler OnCollisionTriggerEnter;
    public event ItemCollisionEventHandler OnCollisionTriggerExit;

    protected GameManager _gameManager;
    protected int _health;
    private int _collisionCount = 0;

    private List<string> _allowedItemTags = new List<string>()
    {
        "Building", "Unit", "Enemy", "Resource"
    };

    private void Start()
    {
        _gameManager = GameManager.Instance;

        Initialize();
    }

    public virtual void Initialize()
    {
        _health = _baseHealth;
        UI.Initialize(_gameManager.Camera);
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

        UI.UpdateHealthBar((float)_health / _baseHealth);
        UI.AmountChanged(-amount);

        if (_health <= 0)
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

        Debug.Log($"Enter into {other.name}");

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

        Debug.Log($"Exit from {other.name}");

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
