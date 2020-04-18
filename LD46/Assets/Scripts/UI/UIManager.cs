using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _woodAmountText;

    [SerializeField]
    private TextMeshProUGUI _rockAmountText;

    [SerializeField]
    private TextMeshProUGUI _goldAmountText;

    [SerializeField]
    private TextMeshProUGUI _timerText;

    [SerializeField]
    private GameObject _gameOverScreen = null;

    [SerializeField]
    private TextMeshProUGUI _gameOverDescription = null;

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

    internal void UpdateTimer(float timer)
    {
        _timerText.text = TimeSpan.FromSeconds(timer).ToString(@"mm\:ss");
    }
}
