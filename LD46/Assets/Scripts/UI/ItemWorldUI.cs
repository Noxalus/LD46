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
    private Image _healthBar = null;

    private Quaternion _initialHealthBarRotation;
    private Quaternion _initialAmountTextRotation;

    private void Start()
    {
        _initialHealthBarRotation = _healthBar.transform.rotation;
        _initialAmountTextRotation = _amountText.transform.rotation;
        Unselect();
    }

    public void Initialize(Camera camera)
    {
        _worldSpaceCanvas.worldCamera = camera;
    }

    public void UpdateHealthBar(float v)
    {
        _healthBar.fillAmount = v;
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
        _healthBar.transform.rotation = _initialHealthBarRotation * _worldSpaceCanvas.worldCamera.transform.rotation;
        _amountText.transform.rotation = _initialAmountTextRotation * _worldSpaceCanvas.worldCamera.transform.rotation;
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
