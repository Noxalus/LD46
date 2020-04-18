using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Resource
{
    public override void Initialize()
    {
        base.Initialize();

        _type = EResourceType.Wood;
    }
}
