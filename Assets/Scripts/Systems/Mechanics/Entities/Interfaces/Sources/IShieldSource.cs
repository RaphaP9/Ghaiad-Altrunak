using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShieldSource
{
    public string GetShieldSourceName();
    public string GetShieldSourceDescription();
    public Sprite GetShieldSourceSprite();
}
