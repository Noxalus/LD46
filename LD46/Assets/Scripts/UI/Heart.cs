using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    [SerializeField]
    private Image _heartImage = null;

    public void Empty(bool value)
    {
        _heartImage.enabled = !value;
    }
}
