using UnityEngine;

public class Resource : Item
{
    [SerializeField]
    private GameObject Mesh = null;

    [SerializeField]
    private ParticleSystem _collectFx = null;

    [SerializeField]
    private float _minMeshScale = 0.2f;

    [SerializeField]
    private int _minQuantity = 1;

    [SerializeField]
    private int _maxQuantity = 50;

    private int _initialQuantity;
    protected int _quantity;

    protected EResourceType _type = EResourceType.Unknown;

    public EResourceType Type => _type;

    public void Initialize(int quantity)
    {
        base.Initialize();

        _initialQuantity = quantity;
        _quantity = _initialQuantity;

        UI.HideHPBar(true);
    }

    public void Collect(int production)
    {
        _quantity -= production;

        // Update scale according damage for building/resources

        //Vector3 minScale = Vector3.one * _minMeshScale; // 0.8f
        float factor = Mathf.Clamp(((float)_quantity / _initialQuantity), _minMeshScale, 1f);
        Vector3 meshScale = Vector3.one * factor;
        //// max = 0.8f => when quantity is max => we want the mesh scale to be the highest
        //// min = 0f => when quantity is 0 => we want the mesh scale to be _minMeshScale

        //Vector3 scale = (Vector3.one - meshScale);
        //// max = 1f when mesh scale is 0
        //// min = 0.2f when mesh scale is 0.8f

        Mesh.transform.localScale = meshScale;
        GameManager.Instance.IncreaseCurrency(_type, production);

        _collectFx.Play();

        if (_quantity <= 0)
        {
            Kill();
        }
    }
}
