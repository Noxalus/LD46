using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas = null;

    [SerializeField]
    private TextMeshProUGUI _woodAmountText;

    [SerializeField]
    private TextMeshProUGUI _rockAmountText;

    [SerializeField]
    private TextMeshProUGUI _goldAmountText;

    [SerializeField]
    private TextMeshProUGUI _timerText;

    [SerializeField]
    private Image _musicDisableImage = null;

    [SerializeField]
    private List<ItemButton> _itemButtons = new List<ItemButton>();

    [SerializeField]
    private GameObject _gameOverScreen = null;

    [SerializeField]
    private TextMeshProUGUI _gameOverDescription = null;

    private void Start()
    {
        _itemButtons = _canvas.GetComponentsInChildren<ItemButton>().ToList();
    }

    public void Initialize()
    {
        _gameOverScreen.SetActive(false);
    }

    public void SetWoodAmount(int value)
    {
        _woodAmountText.text = value.ToString();
    }

    public void SetRockAmount(int value)
    {
        _rockAmountText.text = value.ToString();
    }

    public void SetGoldAmount(int value)
    {
        _goldAmountText.text = value.ToString();
    }

    public void ShowGameOver(float timer)
    {
        _gameOverScreen.SetActive(true);
        _gameOverDescription.text = $"You let your king died in {TimeSpan.FromSeconds(timer).ToString(@"mm\:ss")}";
    }

    public void UpdateTimer(float timer)
    {
        _timerText.text = TimeSpan.FromSeconds(timer).ToString(@"mm\:ss");
    }

    public void RefreshBottomBar()
    {
        foreach (var itemButton in _itemButtons)
        {
            itemButton.CheckCurrencies();
        }
    }

    public void SelectItem(Item item)
    {
        foreach (var itemButton in _itemButtons)
        {
            itemButton.Select(item == itemButton.Item);
        }
    }

    public void ToggleMusic()
    {
        _musicDisableImage.enabled = !_musicDisableImage.enabled;
    }
}
