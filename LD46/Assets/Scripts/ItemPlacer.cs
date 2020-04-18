using UnityEngine;
using UnityEngine.AI;

public class ItemPlacer : MonoBehaviour
{
    [SerializeField]
    private Camera _camera = null;

    [SerializeField]
    private NavMeshSurface _navMesh = null;

    [SerializeField]
    private LayerMask _groundMask = 0;

    private GameObject _currentItem = null;

    void Start()
    {
        
    }

    void Update()
    {
        if (_currentItem != null)
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundMask, QueryTriggerInteraction.Ignore))
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Instantiate(_currentItem, hit.point, Quaternion.identity);
                    _navMesh.BuildNavMesh();
                }
            }
        }
    }

    public void SetItem(GameObject item)
    {
        _currentItem = item;
    }
}
