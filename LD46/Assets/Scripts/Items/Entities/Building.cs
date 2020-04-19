using UnityEngine;

public class Building : Item
{
    public override void Initialize()
    {
        base.Initialize();

        UI.HideHPBar(true);
    }
}
