
public class Enemy : Unit
{
    private GameManager _gameManager;

    private void Start()
    {
        Initialize();

        _gameManager = GameManager.Instance;
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    private void Update()
    {
        Agent.SetDestination(_gameManager.King.transform.position);
    }
}
