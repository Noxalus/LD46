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

    private void Start()
    {
        _initialHealthBarRotation = _healthBar.transform.rotation;
    }

    public void Initialize(Camera camera)
    {
        _worldSpaceCanvas.worldCamera = camera;

        //_amountText.enabled = false;
        //_selectionCircle.enabled = false;
        //_healthBar.enabled = false;
    }

    public void UpdateHealthBar(float v)
    {
        _healthBar.fillAmount = v;
    }

    public void AmountChanged(int amount)
    {
        _amountText.text = amount.ToString();
        _amountText.color = amount > 0 ? Color.green : Color.red;

        _animator.SetTrigger("AmountChanged");
    }

    private void Update()
    {
        _healthBar.transform.rotation = _initialHealthBarRotation * _worldSpaceCanvas.worldCamera.transform.rotation;
    }
}
