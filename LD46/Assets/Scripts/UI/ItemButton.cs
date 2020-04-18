using TMPro;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    [SerializeField]
    private Item _itemPrefab = null;

    //[SerializeField]
    //private Price _itemPrice = null;

    [SerializeField]
    private TextMeshProUGUI _woodCost = null;

    [SerializeField]
    private TextMeshProUGUI _rockCost = null;

    [SerializeField]
    private TextMeshProUGUI _goldCost = null;

    public void Start()
    {
        _woodCost.text = _itemPrefab.Price.Wood.ToString();
        _rockCost.text = _itemPrefab.Price.Rock.ToString();
        _goldCost.text = _itemPrefab.Price.Gold.ToString();
    }

    public void ChangeItem()
    {
        GameManager.Instance.ItemPlacer.SetItem(_itemPrefab);
    }
}
