using UnityEngine;

public class Rock : Resource
{
    public override void Initialize()
    {
        base.Initialize();

        _type = EResourceType.Rock;
    }
}
