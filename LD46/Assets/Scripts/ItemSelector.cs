using UnityEngine;

public class ItemSelector : MonoBehaviour
{
    public delegate void ItemSelectorEventHandler(Item item);
    public event ItemSelectorEventHandler OnItemSelected;

    [SerializeField]
    private Camera _camera = null;

    [SerializeField]
    private LayerMask _selectionLayer = -1;

    private bool _isEnabled = true;

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
                    OnItemSelected?.Invoke(item);
                }
            }
        }
    }
}
