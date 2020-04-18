using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemWorldUI : MonoBehaviour
{
    [SerializeField]
    protected Canvas _worldSpaceCanvas = null;

    [SerializeField]
    protected TextMeshProUGUI _amountText = null;

    [SerializeField]
    protected Image _selectionCircle = null;

    [SerializeField]
    private Image _healthBar = null;

    public void Initialize(Camera camera)
    {
        _worldSpaceCanvas.worldCamera = camera;

        _amountText.enabled = false;
        _selectionCircle.enabled = false;
        //_healthBar.enabled = false;
    }

    public void UpdateHealthBar(float v)
    {
        _healthBar.fillAmount = v;
    }
}
