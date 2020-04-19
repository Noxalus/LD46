using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    [SerializeField]
    private Item _itemPrefab = null;

    [SerializeField]
    private Image _selectionBorder = null;

    //[SerializeField]
    //private Price _itemPrice = null;

    [SerializeField]
    private TextMeshProUGUI _woodCost = null;

    [SerializeField]
    private TextMeshProUGUI _rockCost = null;

    [SerializeField]
    private TextMeshProUGUI _goldCost = null;

    [SerializeField]
    private Image _itemIcon = null;

    public Item Item => _itemPrefab;

    public void Start()
    {
        _woodCost.text = _itemPrefab.Price.Wood.ToString();
        _rockCost.text = _itemPrefab.Price.Rock.ToString();
        _goldCost.text = _itemPrefab.Price.Gold.ToString();

        _selectionBorder.enabled = false;
    }

    public void Select(bool select)
    {
        _selectionBorder.enabled = select;
    }

    public void ChangeItem()
    {
        GameManager.Instance.ItemPlacer.SetItem(_itemPrefab);
    }

    public void CheckCurrencies()
    {
        bool canBuy = true;

        var gameManager = GameManager.Instance;

        if (gameManager.Wood >= _itemPrefab.Price.Wood)
        {
            _woodCost.color = Color.black;
            _woodCost.fontStyle = FontStyles.Normal;
        }
        else
        {
            _woodCost.color = Color.red;
            _woodCost.fontStyle = FontStyles.Bold;
            canBuy = false;
        }

        if (gameManager.Rock >= _itemPrefab.Price.Rock)
        {
            _rockCost.color = Color.black;
            _woodCost.fontStyle = FontStyles.Normal;
        }
        else
        {
            _rockCost.color = Color.red;
            _woodCost.fontStyle = FontStyles.Bold;
            canBuy = false;
        }

        if (gameManager.Gold >= _itemPrefab.Price.Gold)
        {
            _goldCost.color = Color.black;
            _woodCost.fontStyle = FontStyles.Normal;
        }
        else
        {
            _goldCost.color = Color.red;
            _woodCost.fontStyle = FontStyles.Bold;
            canBuy = false;
        }

        _itemIcon.color = canBuy ? Color.white : Color.red;
    }
}
