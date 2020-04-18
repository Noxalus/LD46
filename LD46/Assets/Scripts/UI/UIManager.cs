using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _woodAmountText;

    [SerializeField]
    private TextMeshProUGUI _rockAmountText;

    [SerializeField]
    private TextMeshProUGUI _goldAmountText;

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
}
