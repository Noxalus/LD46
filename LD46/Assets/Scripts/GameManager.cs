using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : Singleton<GameManager>
{
    public Camera MainCamera = null;
    public NavMeshSurface NavMeshSurface = null;
    public WorldBuilder WorldBuilder = null;

    public GameConfiguration GameConfiguration = null;

    public AudioManager AudioManager = null;

    [SerializeField]
    private GameObject _menuScreen = null;

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

    [Header("Debug")]

    [SerializeField]
    private Enemy _enemyPrefab = null;

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
    private bool _isMusicEnabled = true;
    private int _difficulty = 0;
    private int _itemPlacedCount = 0;

    public King King => _king;
    public int Wood => _wood;
    public int Rock => _rock;
    public int Gold => _gold;

    public int Difficulty => _difficulty;

    public ItemPlacer ItemPlacer => _itemPlacer;

    void Start()
    {
        _itemPlacer.OnItemChanged += OnItemChanged;
        _itemPlacer.OnItemPlaced += OnItemPlaced;
        _itemSelector.OnItemSelected += OnItemSelected;

        _initialCameraPosition = MainCamera.transform.position;
        _initialCameraRotation = MainCamera.transform.rotation;

#if DEBUG
        Initialize();
#else
        _menuScreen.SetActive(true);

#endif
    }

    public void Retry()
    {
        Clear();
        Initialize();
    }

    public void Initialize()
    {
        _menuScreen.SetActive(false);

        _wood = 0;
        _rock = 0;
        _gold = 0;

        _timer = 0;
        _isGameOver = false;
        _difficulty = 0;

        MainCamera.transform.position = _initialCameraPosition;
        MainCamera.transform.rotation = _initialCameraRotation;

        if (_worldBuilderCoroutine != null)
        {
            StopCoroutine(_worldBuilderCoroutine);
            _worldBuilderCoroutine = null;
        }

        if (_isMusicEnabled)
        {
            AudioManager.PlayMusic();
        }

        _uiManager.Initialize();
        WorldBuilder.Initialize();
        UIRefreshCurrencies();

        _worldBuilderCoroutine = StartCoroutine(WorldBuilderCoroutine());

        SpawnBaseUnits();
    }

    private void SpawnBaseUnits()
    {
        // Instantiate first units
        _king = Instantiate(_kingPrefab, Vector3.zero, Quaternion.identity) as King;
        _king.Initialize();
        _king.OnDied += OnKingDied;

        _units.Add(_king);

        var firstSoldier = Instantiate(_firstSoldierPrefab, Vector3.forward * 3f, Quaternion.identity);
        firstSoldier.Initialize();

        var firstWorker = Instantiate(_firstWorkerPrefab, Vector3.right * 3f, Quaternion.identity);
        firstWorker.Initialize();

        _units.Add(firstSoldier);
        _units.Add(firstWorker);
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
            yield return new WaitForSeconds(GameConfiguration.WorldChunkSpawnFrequency[_difficulty]);
            WorldBuilder.GenerateNewChunk();
        }
    }

    private void OnItemPlaced(Item item)
    {
        //Debug.Log($"Placed an item: {item.name}");

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

        _itemPlacedCount++;

        // Max difficulty already?
        if (_difficulty < GameConfiguration.ItemPlacedCountsToDifficulty.Count - 1)
        {
            if (GameConfiguration.ItemPlacedCountsToDifficulty[_difficulty + 1] < _itemPlacedCount)
            {
                _difficulty++;
            }
        }
    }

    private void OnBuildingDied(Item item)
    {
        item.OnDied -= OnBuildingDied;

        StartCoroutine(BuildNavMeshCoroutine());
        _buildings.Remove(item as Building);
    }

    private IEnumerator BuildNavMeshCoroutine()
    {
        yield return new WaitForEndOfFrame();
        NavMeshSurface.BuildNavMesh();
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

        _uiManager.SelectItem(item);
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

        if (_difficulty < GameConfiguration.TimeToNextDifficulty.Count)
        {
            if (GameConfiguration.TimeToNextDifficulty[_difficulty] < _timer)
            {
                _difficulty++;
            }
        }

        UpdateUIDifficulty();

#if DEBUG
        #region  Debug

        // Attack enemies using mouse click
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


        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore))
            {
                var enemy = Instantiate(_enemyPrefab, hit.point, Quaternion.identity);
                enemy.Initialize();

                _enemies.Add(enemy);
            }
        }

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

        if (Input.GetKeyDown(KeyCode.F))
        {
            _wood = 99;
            _rock = 99;
            _gold = 99;

            UIRefreshCurrencies();
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

        AudioManager.PlayPlaceItemSound();
    }

    private void UIRefreshCurrencies()
    {
        _uiManager.SetWoodAmount(_wood);
        _uiManager.SetRockAmount(_rock);
        _uiManager.SetGoldAmount(_gold);

        // Also refresh bottom bar
        _uiManager.RefreshBottomBar();
    }

    private void UpdateUITimer()
    {
        _uiManager.UpdateTimer(_timer);
    }

    private void UpdateUIDifficulty()
    {
        _uiManager.UpdateDifficulty(_difficulty);
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

    public void ToggleMusic()
    {
        _uiManager.ToggleMusic();

        if (_isMusicEnabled)
        {
            _isMusicEnabled = false;
            AudioManager.StopMusic();
        }
        else
        {
            _isMusicEnabled = true;
            AudioManager.PlayMusic();
        }
    }
}
