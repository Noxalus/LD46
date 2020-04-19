using System.Collections;
using UnityEngine;

public class GameObjectVanisher : MonoBehaviour
{
    public void Initialize(float timeBeforeVanish)
    {
        StartCoroutine(VanishCoroutine(timeBeforeVanish));
    }

    private IEnumerator VanishCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
