using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : Singleton<GameManager>
{
    public King King = null;
    public Camera Camera = null;
    public NavMeshSurface NavMeshSurface = null;

    [SerializeField]
    private ItemPlacer _itemPlacer = null;

    [SerializeField]
    private ItemSelector _itemSelector = null;

    private List<Building> _buildings = new List<Building>();
    private List<Unit> _units = new List<Unit>();
    private List<Unit> _enemies = new List<Unit>();

    void Start()
    {
        _itemPlacer.OnItemChanged += OnItemChanged;
        _itemPlacer.OnItemPlaced += OnItemPlaced;
        _itemSelector.OnItemSelected += OnItemSelected;
    }

    private void OnItemPlaced(Item item)
    {
        Debug.Log($"Placed an item: {item.name}");

        if (item is Unit unit)
        {
            unit.OnDied += OnUnitDied;
            _units.Add(unit);
        }
        else if (item is Building building)
        {
            building.OnDied += OnBuildingDied;
            _buildings.Add(building);
            NavMeshSurface.BuildNavMesh();
        }
    }

    private void OnBuildingDied(Item item)
    {
        item.OnDied -= OnBuildingDied;
        NavMeshSurface.BuildNavMesh();
        _buildings.Remove(item as Building);
    }

    private void OnUnitDied(Item item)
    {
        item.OnDied -= OnUnitDied;
        _units.Remove(item as Unit);
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
