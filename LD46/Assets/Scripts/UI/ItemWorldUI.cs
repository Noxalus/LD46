using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ResourceSpriteDictionary : SerializableDictionary<EResourceType, Sprite> { }

public class ItemWorldUI : MonoBehaviour
{
    [SerializeField]
    protected Canvas _worldSpaceCanvas = null;

    [SerializeField]
    private Animator _animator = null;

    [SerializeField]
    protected GameObject _amount = null;

    [SerializeField]
    protected TextMeshProUGUI _amountText = null;

    [SerializeField]
    private Color _amountPositiveColor = Color.green;

    [SerializeField]
    private Color _amountNegativeColor = Color.red;

    [SerializeField]
    protected Image _selectionCircle = null;

    [SerializeField]
    private HPBar _hpBar = null;

    [SerializeField]
    private Image _resourceIcon = null;

    [SerializeField]
    private ResourceSpriteDictionary _resourceSprites = new ResourceSpriteDictionary();

    private void Start()
    {
        Unselect();
    }

    public void Initialize(Camera camera, Item link)
    {
        _worldSpaceCanvas.worldCamera = camera;
        _hpBar.Initialize(link.HP);
    }

    public void UpdateHealthBar(int hp)
    {
        _hpBar.UpdateHP(hp);
    }

    public void HideHPBar(bool hide)
    {
        _hpBar.Hide(hide);
    }

    public void AmountChanged(int amount, EResourceType resource = EResourceType.Unknown)
    {
        string amountString = amount > 0 ? "+" : "";
        amountString += amount.ToString();

        _amountText.text = amountString;
        _amountText.color = amount > 0 ? _amountPositiveColor : _amountNegativeColor;

        if (resource != EResourceType.Unknown)
        {
            _resourceIcon.sprite = _resourceSprites[resource];
        }
        else
        {
            _resourceIcon.sprite = null;
        }

        if (amount < 0)
        {
            _animator.SetTrigger("TakeDamage");
        }
        else
        {
            _animator.SetTrigger("CollectResource");
        }
    }

    private void Update()
    {
        if (_hpBar)
            _hpBar.transform.rotation = _worldSpaceCanvas.worldCamera.transform.rotation;

        if (_amount)
            _amount.transform.rotation = _worldSpaceCanvas.worldCamera.transform.rotation;
    }

    public void Select()
    {
        _selectionCircle.enabled = true;
    }

    public void Unselect()
    {
        _selectionCircle.enabled = false;
    }
}
