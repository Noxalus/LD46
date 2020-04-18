using UnityEngine;
using UnityEngine.AI;

public class GameManager : Singleton<GameManager>
{
    public King King = null;
    public Camera Camera = null;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

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
