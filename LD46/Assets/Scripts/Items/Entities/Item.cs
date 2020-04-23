using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    protected ItemWorldUI UI = null;

    [SerializeField]
    protected Animator _animator = null;

    [SerializeField]
    protected int _baseHealth = 1;

    public Price Price;

    [SerializeField]
    protected AudioSource _audioSource = null;

    [SerializeField]
    protected float _actionFrequency = 1f; // Seconds

    [SerializeField]
    private SFXCollection _dieSounds = null;

    [Header("Debug")]

    [SerializeField]
    private bool _initializeOnStart = false;

    public delegate void ItemEventHandler(Item item);
    public delegate void ItemCollisionEventHandler(Item item, int collisionCount);

    public event ItemEventHandler OnDied;
    public event ItemCollisionEventHandler OnCollisionTriggerEnter;
    public event ItemCollisionEventHandler OnCollisionTriggerExit;

    protected int _hp;
    private int _collisionCount = 0;
    protected bool _isDead;
    protected bool _canInteract;

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
        if (_initializeOnStart)
        {
            Initialize();
        }
    }

    public void CanInteract(bool value)
    {
        _canInteract = value;
    }

    public virtual void Initialize()
    {
        _hp = _baseHealth;
        _actionTimer = _actionFrequency;
        _canInteract = true;

        UI.Initialize(GameManager.Instance.MainCamera, this);
    }

    private void Update()
    {
        if (!_canInteract)
        {
            return;
        }

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

    public virtual void TakeDamage(int amount)
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
        //_isDead = true;

        //Collider[] colliders = GetComponentsInChildren<Collider>(true);

        //foreach (var collider in colliders)
        //{
        //    Destroy(collider);
        //}

        //var audioSource = Instantiate(_audioSource);
        //audioSource.transform.position = transform.position;
        //audioSource.PlayOneShot(_dieSounds.GetRandomSound());

        // Worst thing to do ever, but no time left...

        if (_dieSounds)
        {
            var audioSourceGameObject = new GameObject("DeathSource");
            var audioSource = audioSourceGameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.minDistance = 1f;
            audioSource.maxDistance = 50f;
            audioSource.PlayOneShot(_dieSounds.GetRandomSound());

            var gameObjectVanisher = audioSourceGameObject.AddComponent<GameObjectVanisher>();
            gameObjectVanisher.Initialize(2f);
        }
        Destroy(gameObject);

        //StartCoroutine(PlayDeathAnimation());
    }

    private IEnumerator PlayDeathAnimation()
    {
        // TODO: Play death FX
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    public virtual void Select()
    {
        UI.Select();
    }

    public void Unselect()
    {
        UI.Unselect();
    }

    public void OnTriggerEnter(Collider other)
    {

        Debug.Log($"Enter into {other.name}");

        if (other.tag != "Ground")
        {
            _collisionCount++;
            Debug.Log($"Collision count: {_collisionCount}");
            OnCollisionTriggerEnter?.Invoke(this, _collisionCount);
        }

        Item item = other.gameObject.GetComponent<Item>();

        if (_canInteract && item != null && _allowedItemTags.Contains(item.tag))
        {
            OnItemEnter(item);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log($"Exit from {other.name}");

        if (other.tag != "Ground")
        {
            _collisionCount--;
            Debug.Log($"Collision count: {_collisionCount}");
            OnCollisionTriggerExit?.Invoke(this, _collisionCount);
        }

        Item item = other.gameObject.GetComponent<Item>();

        if (_canInteract && item != null)
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
