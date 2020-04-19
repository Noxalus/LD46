using System;
using UnityEngine;

public class Resource : Item
{
    [SerializeField]
    private int _minQuantity = 1;

    [SerializeField]
    private int _maxQuantity = 50;

    private int _initialQuantity;
    protected int _quantity;

    protected EResourceType _type = EResourceType.Unknown;

    public override void Initialize()
    {
        base.Initialize();

        _initialQuantity = UnityEngine.Random.Range(_minQuantity, _maxQuantity + 1);
        _quantity = _initialQuantity;

        UI.HideHPBar(true);
    }

    public void Collect(int production)
    {
        _quantity -= production;

        GameManager.Instance.IncreaseCurrency(_type, production);

        if (_quantity <= 0)
        {
            Kill();
        }
    }
}
