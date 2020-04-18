using UnityEngine;

public class Resource : Item
{
    [SerializeField]
    private int _minQuantity = 1;

    [SerializeField]
    private int _maxQuantity = 50;

    protected int _quantity;

    protected EResourceType _type = EResourceType.Unknown;

    public override void Initialize()
    {
        base.Initialize();

        _quantity = Random.Range(_minQuantity, _maxQuantity + 1);
    }
}
