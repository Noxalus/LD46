using UnityEngine;
using UnityEngine.AI;

public class ItemPlacer : MonoBehaviour
{
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

    private GameObject _currentItem = null;
    private GameObject _ghostItem = null;

    void Start()
    {
        
    }

    void Update()
    {
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
                Instantiate(_currentItem, _ghostItem.transform.position, Quaternion.identity);
                _navMesh.BuildNavMesh();
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                SetItem(null);
            }
        }
    }

    public void SetItem(GameObject item)
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
    }

    private void CreateGhost(GameObject item)
    {
        _ghostItem = Instantiate(item);
        _ghostItem.name = "Ghost";
        _ghostItem.layer = _ghostLayer;

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
