using UnityEngine;
using UnityEngine.AI;

public class ItemPlacer : MonoBehaviour
{
    public delegate void ItemPlacerEventHandler(Item item);
    public event ItemPlacerEventHandler OnItemChanged;
    public event ItemPlacerEventHandler OnItemPlaced;

    [SerializeField]
    private Camera _camera = null;

    [SerializeField]
    private NavMeshSurface _navMesh = null;

    [SerializeField]
    private Material _ghostMaterial = null;

    [SerializeField]
    private Material _ghostMaterialDisabled = null;

    [SerializeField]
    private LayerMask _groundLayer = 0;

    [SerializeField]
    private LayerMask _ghostLayer = 0;

    private Item _currentItem = null;
    private GameObject _ghostItem = null;

    private bool _isEnabled = false;

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
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            bool isOnGround = false;

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundLayer, QueryTriggerInteraction.Ignore))
            {
                _ghostItem.transform.position = hit.point;
                isOnGround = true;
            }

            _ghostItem.SetActive(isOnGround);

            if (isOnGround && Input.GetKeyDown(KeyCode.Mouse0))
            {
                Item item = Instantiate(_currentItem, _ghostItem.transform.position, Quaternion.identity);
                _navMesh.BuildNavMesh();
                OnItemPlaced?.Invoke(item);
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                SetItem(null);
            }
        }
    }

    public void SetItem(Item item)
    {
        if (_ghostItem != null)
        {
            Destroy(_ghostItem);
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
        _ghostItem = Instantiate(item).gameObject;
        _ghostItem.name = "Ghost";
        _ghostItem.gameObject.layer = _ghostLayer;

        // Set ghost material on all meshes
        MeshRenderer[] meshRenderers = _ghostItem.GetComponentsInChildren<MeshRenderer>();

        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.material = _ghostMaterial;
        }

        // Remove all colliders (not triggers)
        Collider[] colliders = _ghostItem.GetComponentsInChildren<Collider>();

        foreach (var collider in colliders)
        {
            if (!collider.isTrigger)
            {
                Destroy(collider);
            }
        }

        Rigidbody rb = _ghostItem.GetComponent<Rigidbody>();
        Destroy(rb);

        _ghostItem.SetActive(false);
    }
}
