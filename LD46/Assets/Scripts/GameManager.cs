﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : Singleton<GameManager>
{
    public Camera MainCamera = null;
    public NavMeshSurface NavMeshSurface = null;
    public WorldBuilder WorldBuilder = null;

    [SerializeField]
    private UIManager _uiManager = null;

    [SerializeField]
    private King _kingPrefab = null;

    [SerializeField]
    private Soldier _firstSoldierPrefab = null;

    [SerializeField]
    private Worker _firstWorkerPrefab = null;

    [SerializeField]
    private ItemPlacer _itemPlacer = null;

    [SerializeField]
    private ItemSelector _itemSelector = null;

    private List<Building> _buildings = new List<Building>();
    private List<Unit> _units = new List<Unit>();
    private List<Unit> _enemies = new List<Unit>();
    private List<Resource> _resources = new List<Resource>();
    private List<Building> _enemyBuildings = new List<Building>();

    private int _wood = 0;
    private int _rock = 0;
    private int _gold = 0;

    private King _king;

    private float _timer;
    private bool _isGameOver;

    private Coroutine _worldBuilderCoroutine;

    private Vector3 _initialCameraPosition;
    private Quaternion _initialCameraRotation;

    public King King => _king;
    public int Wood => _wood;
    public int Rock => _rock;
    public int Gold => _gold;

    public ItemPlacer ItemPlacer => _itemPlacer;

    void Start()
    {
        _itemPlacer.OnItemChanged += OnItemChanged;
        _itemPlacer.OnItemPlaced += OnItemPlaced;
        _itemSelector.OnItemSelected += OnItemSelected;

        _initialCameraPosition = MainCamera.transform.position;
        _initialCameraRotation = MainCamera.transform.rotation;

        Initialize();
    }

    public void Retry()
    {
        Clear();
        Initialize();
    }

    private void Initialize()
    {
        _wood = 0;
        _rock = 0;
        _gold = 0;

#if DEBUG
        _wood = 99;
        _rock = 99;
        _gold = 99;
#endif

        _timer = 0;
        _isGameOver = false;
        UIRefreshCurrencies();

        MainCamera.transform.position = _initialCameraPosition;
        MainCamera.transform.rotation = _initialCameraRotation;

        if (_worldBuilderCoroutine != null)
        {
            StopCoroutine(_worldBuilderCoroutine);
            _worldBuilderCoroutine = null;
        }

        _uiManager.Initialize();
        WorldBuilder.Initialize();

        // Instantiate first units
        _king = Instantiate(_kingPrefab, Vector3.zero, Quaternion.identity) as King;
        _king.OnDied += OnKingDied;

        _units.Add(_king);
        _units.Add(Instantiate(_firstSoldierPrefab, Vector3.forward, Quaternion.identity));
        _units.Add(Instantiate(_firstWorkerPrefab, Vector3.right, Quaternion.identity));

        _worldBuilderCoroutine = StartCoroutine(WorldBuilderCoroutine());
    }

    private void Clear()
    {
        foreach (var item in _enemies)
        {
            if (item != null)
                Destroy(item.gameObject);
        }

        _enemies.Clear();

        foreach (var item in _units)
        {
            if (item != null)
                Destroy(item.gameObject);
        }

        _units.Clear();

        foreach (var item in _buildings)
        {
            if (item != null)
                Destroy(item.gameObject);
        }

        _buildings.Clear();

        foreach (var item in _resources)
        {
            if (item != null)
                Destroy(item.gameObject);
        }

        _resources.Clear();

        foreach (var item in _enemyBuildings)
        {
            if (item != null)
                Destroy(item.gameObject);
        }

        _enemyBuildings.Clear();

        WorldBuilder.Clear();
    }

    private void OnKingDied(Item item)
    {
        GameOver();
    }

    private void GameOver()
    {
        _isGameOver = true;
        _uiManager.ShowGameOver(_timer);

    }

    // Add chunk regularly
    private IEnumerator WorldBuilderCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            WorldBuilder.GenerateNewChunk();
        }
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
        if (_isGameOver)
        {
            return;
        }

        _timer += Time.deltaTime;
        UpdateUITimer();

#if DEBUG
        #region  Debug

        //// Attack enemies using mouse click
        //if (Input.GetKeyDown(KeyCode.Mouse1))
        //{
        //    Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);

        //    if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Enemy"), QueryTriggerInteraction.Ignore))
        //    {
        //        Debug.Log($"Hit {hit.collider.name}");

        //        var item = hit.collider.GetComponent<Item>();

        //        if (item != null)
        //        {
        //            item.TakeDamage(1);
        //        }
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            WorldBuilder.GenerateNewChunk();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(GenerateChunksCoroutine());
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Retry();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            GameOver();
        }

        #endregion
#endif
    }

    private IEnumerator GenerateChunksCoroutine()
    {
        for (int i = 0; i < 1000; i++)
        {
            WorldBuilder.GenerateNewChunk();
            yield return new WaitForSeconds(0.01f);
        }

        yield return null;
    }

    public void IncreaseCurrency(EResourceType type, int production)
    {
        switch (type)
        {
            case EResourceType.Wood:
                _wood = Mathf.Clamp(_wood + production, 0, 99);
                break;
            case EResourceType.Rock:
                _rock = Mathf.Clamp(_rock + production, 0, 99);
                break;
            case EResourceType.Gold:
                _gold = Mathf.Clamp(_gold + production, 0, 99);
                break;
            default:
                Debug.LogError($"Unknown currency: {type}");
                break;
        }

        UIRefreshCurrencies();
    }

    private void UIRefreshCurrencies()
    {
        _uiManager.SetWoodAmount(_wood);
        _uiManager.SetRockAmount(_rock);
        _uiManager.SetGoldAmount(_gold);
    }

    private void UpdateUITimer()
    {
        _uiManager.UpdateTimer(_timer);
    }

    public void BuyItem(Item item)
    {
        if (item.Price == null)
        {
            Debug.LogError($"This item has no price: {item.name}");
            return;
        }

        _wood -= item.Price.Wood;
        _rock -= item.Price.Rock;
        _gold -= item.Price.Gold;

        UIRefreshCurrencies();
    }

    public void AddResources(Resource resource)
    {
        _resources.Add(resource);
    }

    public void AddEnemy(Enemy enemy)
    {
        _enemies.Add(enemy);
    }

    public void AddEnemyBuilding(Building building)
    {
        _enemyBuildings.Add(building);
        building.OnDied += OnEnemyBuildingDied;
    }

    private void OnEnemyBuildingDied(Item item)
    {
        item.OnDied -= OnEnemyBuildingDied;

        _enemyBuildings.Remove(item as Building);
        NavMeshSurface.BuildNavMesh();
    }
}
