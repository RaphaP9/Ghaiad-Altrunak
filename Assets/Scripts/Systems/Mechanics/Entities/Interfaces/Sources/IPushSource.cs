using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPushSource
{
    public string GetPushSourceName();
    public string GetPushSourceDescription();
    public Sprite GetPushSourceSprite();
}
