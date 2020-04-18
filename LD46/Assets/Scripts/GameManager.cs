using UnityEngine;
using UnityEngine.AI;

public class GameManager : Singleton<GameManager>
{
    public King King = null;
    public Camera Camera = null;

    [SerializeField]
    private ItemPlacer _itemPlacer = null;

    [SerializeField]
    private ItemSelector _itemSelector = null;

    void Start()
    {
        _itemPlacer.OnItemChanged += OnItemChanged;
        _itemPlacer.OnItemPlaced += OnItemPlaced;
        _itemSelector.OnItemSelected += OnItemSelected;
    }

    private void OnItemPlaced(Item item)
    {
        Debug.Log($"Placed an item: {item.name}");
    }

    private void OnItemChanged(Item item)
    {
        bool itemPlacerClosed = item == null;

        _itemPlacer.Enable(!itemPlacerClosed);
        _itemSelector.Enable(itemPlacerClosed);
    }

    private void OnItemSelected(Item item)
    {
    }

    void Update()
    {
        // Attack enemies using mouse click
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Enemy"), QueryTriggerInteraction.Ignore))
            {
                Debug.Log($"Hit {hit.collider.name}");

                var item = hit.collider.GetComponent<Item>();

                if (item != null)
                {
                    item.TakeDamage(1);
                }
            }
        }
    }
}
