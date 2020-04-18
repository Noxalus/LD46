using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private int _baseHealth = 1;

    public delegate void ItemEventHandler();
    public event ItemEventHandler OnDied;

    protected int _health;

    public virtual void Initialize()
    {
        _health = _baseHealth;
    }

    public void TakeDamage(int amount)
    {
        _health -= amount;

        if (_health <= 0)
        {
            Destroy(gameObject);
            OnDied?.Invoke();
        }
    }
}
