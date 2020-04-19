using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField]
    private Heart _heartPrefab = null;

    [SerializeField]
    private RectTransform _heartHolder = null;

    [SerializeField]
    private CanvasGroup _canvasGroup = null;

    private List<Heart> _instances = new List<Heart>();

    private int _maxHp;

    public void Initialize(int hp)
    {
        _maxHp = hp;

        for (int i = 0; i < _maxHp; i++)
        {
            _instances.Add(Instantiate(_heartPrefab, _heartHolder));
        }
    }

    public void UpdateHP(int value)
    {
        for (int i = 0; i < _maxHp; i++)
        {
            _instances[i].Empty(i >= value);
        }
    }

    public void Hide(bool hide)
    {
        _canvasGroup.alpha = hide ? 0f : 1f;
    }
}
