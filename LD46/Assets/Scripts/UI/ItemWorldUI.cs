using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemWorldUI : MonoBehaviour
{
    [SerializeField]
    protected Canvas _worldSpaceCanvas = null;

    [SerializeField]
    private Animator _animator = null;

    [SerializeField]
    protected TextMeshProUGUI _amountText = null;

    [SerializeField]
    protected Image _selectionCircle = null;

    [SerializeField]
    private HPBar _hpBar = null;

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

    public void AmountChanged(int amount, bool showHealthBar = false)
    {
        string amountString = amount > 0 ? "+" : "";
        amountString += amount.ToString();

        _amountText.text = amountString;
        _amountText.color = amount > 0 ? Color.green : Color.red;

        if (showHealthBar)
        {
            _animator.SetTrigger("AmountChangedWithHealthBar");
        }
        else
        {
            _animator.SetTrigger("AmountChanged");
        }
    }

    private void Update()
    {
        _hpBar.transform.rotation = _worldSpaceCanvas.worldCamera.transform.rotation;
        _amountText.transform.rotation = _worldSpaceCanvas.worldCamera.transform.rotation;
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
