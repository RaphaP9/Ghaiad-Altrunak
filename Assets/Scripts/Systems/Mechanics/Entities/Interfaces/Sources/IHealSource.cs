using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealSource
{
    public string GetHealSourceName();
    public string GetHealSourceDescription();
    public Sprite GetHealSourceSprite();
}
