using UnityEngine;
using UnityEngine.AI;

public class GameManager : Singleton<GameManager>
{
    public King King = null;

    [SerializeField]
    private Camera _camera = null;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                NavMeshPath path = new NavMeshPath();
                King.Agent.CalculatePath(hit.point, path);
                if (path.status == NavMeshPathStatus.PathPartial)
                {
                    Debug.Log("Can't reach the destination");
                }

                King.Agent.SetDestination(hit.point);
            }
        }
    }
}
