using UnityEngine;
using UnityEngine.AI;

public class ItemSelector : MonoBehaviour
{
    public delegate void ItemSelectorEventHandler(Item item);
    public event ItemSelectorEventHandler OnItemSelected;

    [SerializeField]
    private Camera _camera = null;

    [SerializeField]
    private LayerMask _selectionLayer = -1;

    private bool _isEnabled = true;

    private Item _currentSelection = null;
    private bool _isCurrentSelectionUnit = false;

    public void Enable(bool enable)
    {
        _isEnabled = enable;
    }

    private void Update()
    {
        if (!_isEnabled)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _selectionLayer, QueryTriggerInteraction.Ignore))
            {
                Item item = hit.collider.GetComponent<Item>();

                if (item != null)
                {
                    ChangeSelection(item);
                }
            }
            else
            {
                ChangeSelection(null);
            }
        }
        else if (_currentSelection != null && _isCurrentSelectionUnit && Input.GetKeyDown(KeyCode.Mouse1))
        {
            Unit currentSelectedUnit = _currentSelection as Unit;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, -1, QueryTriggerInteraction.Ignore))
            {
                NavMeshPath path = new NavMeshPath();
                currentSelectedUnit.Agent.CalculatePath(hit.point, path);
                if (path.status == NavMeshPathStatus.PathPartial)
                {
                    Debug.Log("Can't reach the destination");
                }

                Item targetItem = hit.collider.GetComponent<Item>();

                if (targetItem != null)
                {
                    currentSelectedUnit.SetTarget(targetItem);
                }
                else
                {
                    currentSelectedUnit.SetTarget(null);
                    currentSelectedUnit.Agent.SetDestination(hit.point);
                    currentSelectedUnit.Agent.isStopped = false;
                }

                if (currentSelectedUnit.tag == "Unit")
                {
                    GameManager.Instance.AudioManager.PlayExecuteOrderSound();
                }
            }
        }
    }

    private void ChangeSelection(Item selection)
    {
        if (_currentSelection != null)
        {
            _currentSelection.Unselect();
        }

        _currentSelection = selection;

        if (_currentSelection != null)
        {
            _currentSelection.Select();
            _isCurrentSelectionUnit = _currentSelection is Unit;
        }

        OnItemSelected?.Invoke(selection);
    }
}
