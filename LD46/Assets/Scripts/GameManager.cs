using UnityEngine;
using UnityEngine.AI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private Camera _camera = null;


    [SerializeField]
    private NavMeshAgent _king = null;


    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                NavMeshPath path = new NavMeshPath();
                _king.CalculatePath(hit.point, path);
                if (path.status == NavMeshPathStatus.PathPartial)
                {
                    Debug.Log("Can't reach the destination");
                }

                _king.SetDestination(hit.point);
            }
        }
    }
}
