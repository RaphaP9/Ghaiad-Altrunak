using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGoldSource
{
    public string GetGoldSourceName();
    public string GetGoldSourceDescription();
    public Sprite GetGoldSourceSprite();
}
