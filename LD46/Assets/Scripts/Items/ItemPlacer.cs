using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemPlacer : MonoBehaviour
{
    public delegate void ItemPlacerEventHandler(Item item);
    public event ItemPlacerEventHandler OnItemChanged;
    public event ItemPlacerEventHandler OnItemPlaced;

    [SerializeField]
    private Camera _camera = null;

    [SerializeField]
    private Material _ghostMaterial = null;

    [SerializeField]
    private Material _ghostMaterialDisabled = null;

    [SerializeField]
    private LayerMask _groundLayer = 0;

    [SerializeField]
    private LayerMask _uiLayer = 0;

    [SerializeField]
    private LayerMask _ghostLayer = 0;

    private Item _currentItem = null;
    private GameObject _ghostItemGameObject = null;
    private MeshRenderer[] _ghostMeshRenderers;

    private bool _isEnabled = false;
    private bool _canPlace = true;

    public void Enable(bool enable)
    {
        _isEnabled = enable;
    }

    void Update()
    {
        if (!_isEnabled)
        {
            return;
        }

        if (_currentItem != null)
        {
            // Rotate the mesh
            if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl))
            {
                _ghostItemGameObject.transform.Rotate(Vector3.up, 90f);
            }

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            bool isOnGround = false;

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundLayer, QueryTriggerInteraction.Ignore))
            {
                _ghostItemGameObject.transform.position = hit.point;
                isOnGround = true;
            }

            _ghostItemGameObject.SetActive(isOnGround);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (isOnGround)
                {
                    if (_canPlace)
                    {
                        if (!EventSystem.current.IsPointerOverGameObject() && CanBuy(_currentItem))
                        {
                            Buy(_currentItem);
                        }
                        else
                        {
                            SetItem(null);
                        }
                    }
                    else
                    {
                        Debug.Log("Can't place item (it collides with another one)");
                    }
                }
                else
                {
                    Debug.Log("Can't place item (it's not on the ground)");
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                SetItem(null);
            }
        }
    }

    public void SetItem(Item item)
    {
        if (_ghostItemGameObject != null)
        {
            Destroy(_ghostItemGameObject);
        }

        _currentItem = item;

        if (item != null)
        {
            CreateGhost(item);
        }

        OnItemChanged?.Invoke(_currentItem);
    }

    private void CreateGhost(Item item)
    {
        var ghostItem = Instantiate(item);
        ghostItem.CanInteract(false);
        _ghostItemGameObject = ghostItem.gameObject;
        _ghostItemGameObject.name = "Ghost";
        _ghostItemGameObject.tag = "Ghost";
        //_ghostItemGameObject.gameObject.layer = _ghostLayer;

        // Set ghost material on all meshes
        _ghostMeshRenderers = _ghostItemGameObject.GetComponentsInChildren<MeshRenderer>();

        foreach (var meshRenderer in _ghostMeshRenderers)
        {
            meshRenderer.material = _ghostMaterial;
        }

        // Remove all colliders (not triggers)
        //Collider[] colliders = _ghostItemGameObject.GetComponentsInChildren<Collider>();

        //foreach (var collider in colliders)
        //{
        //    if (!collider.isTrigger)
        //    {
        //        Destroy(collider);
        //    }
        //}

        //// Remove rigid body
        //Rigidbody rigidBody = _ghostItemGameObject.GetComponent<Rigidbody>();

        //if (rigidBody != null)
        //{
        //    Destroy(rigidBody);
        //}

        // Remove nav mesh agent
        NavMeshAgent navMeshAgent = _ghostItemGameObject.GetComponent<NavMeshAgent>();

        if (navMeshAgent != null)
        {
            Destroy(navMeshAgent);
        }

        // Remove nav mesh agent
        Canvas canvas = _ghostItemGameObject.GetComponentInChildren<Canvas>();

        if (canvas)
        {
            Destroy(canvas.gameObject);
        }

        ghostItem.OnCollisionTriggerEnter += OnGhostCollisionTriggerEnter;
        ghostItem.OnCollisionTriggerExit += OnGhostCollisionTriggerExit;

        _ghostItemGameObject.SetActive(false);
        _canPlace = true;
    }

    private void OnGhostCollisionTriggerEnter(Item item, int collisionCount)
    {
        UpdateGhostMaterial(collisionCount == 0);
    }

    private void OnGhostCollisionTriggerExit(Item item, int collisionCount)
    {
        UpdateGhostMaterial(collisionCount == 0);
    }

    private void UpdateGhostMaterial(bool canPlace)
    {
        if (_canPlace != canPlace)
        {
            Material material = canPlace ? _ghostMaterial : _ghostMaterialDisabled;

            foreach (var meshRenderer in _ghostMeshRenderers)
            {
                meshRenderer.material = material;
            }
        }

        _canPlace = canPlace;
    }

    private bool CanBuy(Item item)
    {
        if (item.Price == null)
        {
            Debug.LogError($"This item has no price: {item.name}");
            return false;
        }

        GameManager gameManager = GameManager.Instance;

        if (item.Price.Wood > gameManager.Wood)
        {
            return false;
        }

        if (item.Price.Rock > gameManager.Rock)
        {
            return false;
        }

        if (item.Price.Gold > gameManager.Gold)
        {
            return false;
        }

        return true;
    }

    private void Buy(Item item)
    {
        GameManager.Instance.BuyItem(item);
        Item itemInstance = Instantiate(item, _ghostItemGameObject.transform.position, _ghostItemGameObject.transform.rotation);
        itemInstance.Initialize();

        itemInstance.Animator.SetTrigger("Spawn");
        OnItemPlaced?.Invoke(itemInstance);
    }
}
