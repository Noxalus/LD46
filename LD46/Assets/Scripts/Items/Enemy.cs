
public class Enemy : Unit
{
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    private void Update()
    {
        Agent.SetDestination(_gameManager.King.transform.position);
    }
}
