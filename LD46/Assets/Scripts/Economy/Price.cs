using UnityEngine;

[CreateAssetMenu(fileName = "Price.asset", menuName = "LD46/Price")]
public class Price : ScriptableObject
{
    [SerializeField]
    private int _wood;
    [SerializeField]
    private int _rock;
    [SerializeField]
    private int _gold;

    public int Wood => _wood;
    public int Rock => _rock;
    public int Gold => _gold;
}
